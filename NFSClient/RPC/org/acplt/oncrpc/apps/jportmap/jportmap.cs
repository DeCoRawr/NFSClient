/*
 * $Header: /cvsroot/remotetea/remotetea/src/org/acplt/oncrpc/XdrVoid.java,v 1.1.1.1 2003/08/13 12:03:41 haraldalbrecht Exp $
 *
 * Copyright (c) 1999, 2000
 * Lehrstuhl fuer Prozessleittechnik (PLT), RWTH Aachen
 * D-52064 Aachen, Germany.
 * All rights reserved.
 *
 * This library is free software; you can redistribute it and/or modify
 * it under the terms of the GNU Library General Public License as
 * published by the Free Software Foundation; either version 2 of the
 * License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Library General Public License for more details.
 *
 * You should have received a copy of the GNU Library General Public
 * License along with this program (see the file COPYING.LIB for more
 * details); if not, write to the Free Software Foundation, Inc.,
 * 675 Mass Ave, Cambridge, MA 02139, USA.
 */

using System.Net;
using System.Net.Sockets;
using System;
using org.acplt.oncrpc.server;
namespace org.acplt.oncrpc.apps.jportmap
{
	/// <summary>
	/// The class <code>jportmap</code> implements a Java-based ONC/RPC port mapper,
	/// speaking the widely deployed protocol version 2.
	/// </summary>
	/// <remarks>
	/// The class <code>jportmap</code> implements a Java-based ONC/RPC port mapper,
	/// speaking the widely deployed protocol version 2.
	/// <p>This class can be either used stand-alone (a static <code>main</code> is
	/// provided for this purpose) or as part of an application. In this case you
	/// should check first for another portmap already running before starting your
	/// own one.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.2 $ $Date: 2003/08/14 11:26:50 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class jportmap : OncRpcServerStub, OncRpcDispatchable
	{
		/// <summary>
		/// Create a new portmap instance, create the transport registration
		/// information and UDP and TCP-based transports, which will be bound
		/// later to port 111.
		/// </summary>
		/// <remarks>
		/// Create a new portmap instance, create the transport registration
		/// information and UDP and TCP-based transports, which will be bound
		/// later to port 111. The constructor does not start the dispatcher loop.
		/// </remarks>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		public jportmap()
		{
			//
			// We only need to register one {progam, version}.
			//
			info = new org.acplt.oncrpc.server.OncRpcServerTransportRegistrationInfo[] { new 
				org.acplt.oncrpc.server.OncRpcServerTransportRegistrationInfo(PMAP_PROGRAM, PMAP_VERSION
				) };
			//
			// We support both UDP and TCP-based transports for ONC/RPC portmap
			// calls, and these transports are bound to the well-known port 111.
			//
			transports = new org.acplt.oncrpc.server.OncRpcServerTransport[] { new org.acplt.oncrpc.server.OncRpcUdpServerTransport
				(this, PMAP_PORT, info, 32768), new org.acplt.oncrpc.server.OncRpcTcpServerTransport
				(this, PMAP_PORT, info, 32768) };
			//
			// Finally, we add ourself to the list of registered ONC/RPC servers.
			// This is just a convenience.
			//
			servers.Add(new org.acplt.oncrpc.OncRpcServerIdent(PMAP_PROGRAM, PMAP_VERSION, org.acplt.oncrpc.OncRpcProtocols
				.ONCRPC_TCP, PMAP_PORT));
			servers.Add(new org.acplt.oncrpc.OncRpcServerIdent(PMAP_PROGRAM, PMAP_VERSION, org.acplt.oncrpc.OncRpcProtocols
				.ONCRPC_UDP, PMAP_PORT));
			//
			// Determine all local IP addresses assigned to this host.
			// Once again, take care of broken JDKs, which can not handle
			// InetAdress.getLocalHost() properly. Sigh.
			//
			try
			{
                IPAddress loopback = IPAddress.Loopback;

                // Get host name
                string strHostName = Dns.GetHostName();
                IPAddress[] addrs = Dns.GetHostAddresses(strHostName);
				//
				// Check whether the loopback address is already included in
				// the address list for this host. If not, add it to the list.
				//
				bool loopbackIncluded = false;
				for (int idx = 0; idx < addrs.Length; ++idx)
				{
					if (addrs[idx].Equals(loopback))
					{
						loopbackIncluded = true;
						break;
					}
				}
				if (loopbackIncluded)
				{
					locals = addrs;
				}
				else
				{
					locals = new IPAddress[addrs.Length + 1];
					locals[0] = loopback;
					System.Array.Copy(addrs, 0, locals, 1, addrs.Length);
				}
			}
			catch (SocketException)
			{
                // jmw need to debug this and see if it's the right exception to catch here
				//
				// Trouble getting all addresses for this host (which might
				// have been caused by some dumb security manager -- yeah, as
				// if managers were not dumb by definition), so fall back to
				// allowing only the loopback address.
				//
				locals = new IPAddress[1];
                locals[0] = IPAddress.Loopback;
			}
		}

		/// <summary>Lookup port for (program, version, protocol).</summary>
		/// <remarks>
		/// Lookup port for (program, version, protocol). If no suitable
		/// registration entry if found and an entry with another version, but the
		/// same program and version number is found, this is returned instead.
		/// This is compatible with the way Sun's portmap implementation works.
		/// </remarks>
		/// <param name="params">
		/// server identification (program, version, protocol) to
		/// look up. The port field is not used.
		/// </param>
		/// <returns>
		/// port number where server listens for incomming ONC/RPC calls,
		/// or <code>0</code>, if no server is registered for (program, protocol).
		/// </returns>
		internal virtual OncRpcGetPortResult getPort(org.acplt.oncrpc.OncRpcServerIdent
			 @params)
		{
			org.acplt.oncrpc.OncRpcServerIdent ident = null;
			org.acplt.oncrpc.OncRpcGetPortResult result = new org.acplt.oncrpc.OncRpcGetPortResult
				();
			int size = servers.Count;
			for (int idx = 0; idx < size; ++idx)
			{
				org.acplt.oncrpc.OncRpcServerIdent svr = (org.acplt.oncrpc.OncRpcServerIdent)servers
					[idx];
				if ((svr.program == @params.program) && (svr.protocol == @params.protocol))
				{
					//
					// (program, protocol) already matches. If it has the same
					// version, then we're done. Otherwise we remember this
					// entry for possible later usage and search further through
					// the list.
					//
					if (svr.version == @params.version)
					{
						result.port = svr.port;
						return result;
					}
					ident = svr;
				}
			}
			//
			// Return port of "best" match, if one was found at all, otherwise
			// just return 0, which indicates an invalid UDP/TCP port.
			//
			if (ident == null)
			{
				result.port = 0;
			}
			else
			{
				result.port = ident.port;
			}
			return result;
		}

		/// <summary>Register a port number for a particular (program, version, protocol).</summary>
		/// <remarks>
		/// Register a port number for a particular (program, version, protocol).
		/// Note that a caller can not register the same (program, version,
		/// protocol) for another port. In this case we return false. Thus, a
		/// caller first needs to deregister any old entries which it whishes to
		/// update. Always add new registration entries to the end of the list
		/// (vector).
		/// </remarks>
		/// <param name="params">(program, version, protocol, port) to register.</param>
		/// <returns><code>true</code> if registration succeeded.</returns>
		internal virtual XdrBoolean setPort(org.acplt.oncrpc.OncRpcServerIdent
			 @params)
		{
			if (@params.program != PMAP_PROGRAM)
			{
				//
				// Only accept registration attempts for anything other than
				// the portmapper. We do not want clients to play tricks on us.
				//
				int size = servers.Count;
				for (int idx = 0; idx < size; ++idx)
				{
					org.acplt.oncrpc.OncRpcServerIdent svr = (org.acplt.oncrpc.OncRpcServerIdent)servers
						[idx];
					if ((svr.program == @params.program) && (svr.version == @params.version) && (svr.
						protocol == @params.protocol))
					{
						//
						// In case (program, version, protocol) is already
						// registered only accept, if the port stays the same.
						// This will silently accept double registrations (i.e.,
						// due to duplicated UDP calls).
						//
						return new org.acplt.oncrpc.XdrBoolean(svr.port == @params.port);
					}
				}
				//
				// Add new registration entry to end of the list.
				//
				servers.Add(@params);
				return new org.acplt.oncrpc.XdrBoolean(true);
			}
			return new org.acplt.oncrpc.XdrBoolean(false);
		}

		/// <summary>
		/// Deregister all port settings for a particular (program, version) for
		/// all transports (TCP, UDP, ...).
		/// </summary>
		/// <remarks>
		/// Deregister all port settings for a particular (program, version) for
		/// all transports (TCP, UDP, ...). While these are strange semantics,
		/// they are compatible with Sun's portmap implementation.
		/// </remarks>
		/// <param name="params">
		/// (program, version) to deregister. The protocol and port
		/// fields are not used.
		/// </param>
		/// <returns><code>true</code> if deregistration succeeded.</returns>
		internal virtual XdrBoolean unsetPort(org.acplt.oncrpc.OncRpcServerIdent
			 @params)
		{
			bool ok = false;
			if (@params.program != PMAP_PROGRAM)
			{
				//
				// Only allow clients to deregister ONC/RPC servers other than
				// the portmap entries.
				//
				int size = servers.Count;
				for (int idx = size - 1; idx >= 0; --idx)
				{
					org.acplt.oncrpc.OncRpcServerIdent svr = (org.acplt.oncrpc.OncRpcServerIdent)servers
						[idx];
					if ((svr.program == @params.program) && (svr.version == @params.version))
					{
						servers.RemoveAt(idx);
						ok = true;
					}
				}
			}
			return new org.acplt.oncrpc.XdrBoolean(ok);
		}

		/// <summary>Return list of registered ONC/RPC servers.</summary>
		/// <remarks>Return list of registered ONC/RPC servers.</remarks>
		/// <returns>
		/// list of ONC/RPC server descriptions (program, version,
		/// protocol, port).
		/// </returns>
		internal virtual OncRpcDumpResult listServers()
		{
			org.acplt.oncrpc.OncRpcDumpResult result = new org.acplt.oncrpc.OncRpcDumpResult(
				);
			result.servers = servers;
			return result;
		}

		/// <summary>
		/// Checks whether the address given belongs to one of the local
		/// addresses of this host.
		/// </summary>
		/// <remarks>
		/// Checks whether the address given belongs to one of the local
		/// addresses of this host.
		/// </remarks>
		/// <param name="addr">IP address to check.</param>
		/// <returns>
		/// <code>true</code> if address specified belongs to one of the
		/// local addresses of this host.
		/// </returns>
		internal virtual bool isLocalAddress(IPAddress addr)
		{
			int size = locals.Length;
			for (int idx = 0; idx < size; ++idx)
			{
				if (addr.Equals(locals[idx]))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Dispatch incomming ONC/RPC calls to the individual handler functions.</summary>
		/// <remarks>
		/// Dispatch incomming ONC/RPC calls to the individual handler functions.
		/// The CALLIT method is currently unimplemented.
		/// </remarks>
		/// <param name="call">
		/// The ONC/RPC call, with references to the transport and
		/// XDR streams to use for retrieving parameters and sending replies.
		/// </param>
		/// <param name="program">the portmap's program number, 100000</param>
		/// <param name="version">the portmap's protocol version, 2</param>
		/// <param name="procedure">the procedure to call.</param>
		/// <exception cref="org.acplt.oncrpc.OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		public virtual void dispatchOncRpcCall(org.acplt.oncrpc.server.OncRpcCallInformation
			 call, int program, int version, int procedure)
		{
			//
			// Make sure it's the right program and version that we can handle.
			// (defensive programming)
			//
			if (program == PMAP_PROGRAM)
			{
				if (version == PMAP_VERSION)
				{
					switch (procedure)
					{
						case 0:
						{
							// handle NULL call.
							call.retrieveCall(org.acplt.oncrpc.XdrVoid.XDR_VOID);
							call.reply(org.acplt.oncrpc.XdrVoid.XDR_VOID);
							break;
						}

						case OncRpcPortmapServices.PMAP_GETPORT:
						{
							// handle port query
							org.acplt.oncrpc.OncRpcServerIdent @params = new org.acplt.oncrpc.OncRpcServerIdent
								();
							call.retrieveCall(@params);
							org.acplt.oncrpc.OncRpcGetPortResult result = getPort(@params);
							call.reply(result);
							break;
						}

						case OncRpcPortmapServices.PMAP_SET:
						{
							// handle port registration
							//
							// ensure that no remote client tries to register
							//
							OncRpcServerIdent @params = new OncRpcServerIdent
								();
							call.retrieveCall(@params);
							org.acplt.oncrpc.XdrBoolean result;
							if (isLocalAddress(call.peerAddress))
							{
								result = setPort(@params);
							}
							else
							{
								result = new XdrBoolean(false);
							}
							call.reply(result);
							break;
						}

						case OncRpcPortmapServices.PMAP_UNSET:
						{
							// handle port deregistration
							OncRpcServerIdent @params = new OncRpcServerIdent
								();
							call.retrieveCall(@params);
							org.acplt.oncrpc.XdrBoolean result;
							if (isLocalAddress(call.peerAddress))
							{
								result = unsetPort(@params);
							}
							else
							{
								result = new XdrBoolean(false);
							}
							call.reply(result);
							break;
						}

						case OncRpcPortmapServices.PMAP_DUMP:
						{
							// list all registrations
							call.retrieveCall(org.acplt.oncrpc.XdrVoid.XDR_VOID);
							org.acplt.oncrpc.OncRpcDumpResult result = listServers();
							call.reply(result);
							break;
						}

						default:
						{
							// unknown/unimplemented procedure
							call.failProcedureUnavailable();
							break;
						}
					}
				}
				else
				{
					call.failProgramMismatch(PMAP_VERSION, PMAP_VERSION);
				}
			}
			else
			{
				call.failProgramUnavailable();
			}
		}

		/// <summary>List of IP addresses assigned to this host.</summary>
		/// <remarks>
		/// List of IP addresses assigned to this host. Will be filled later
		/// by constructor.
		/// </remarks>
		public IPAddress[] locals = null;

		/// <summary>The list of registrated servers.</summary>
		/// <remarks>The list of registrated servers.</remarks>
		public System.Collections.ArrayList servers = new System.Collections.ArrayList();

		/// <summary>Well-known port where the portmap process can be found on Internet hosts.
		/// 	</summary>
		/// <remarks>Well-known port where the portmap process can be found on Internet hosts.
		/// 	</remarks>
		public const int PMAP_PORT = 111;

		/// <summary>Program number of the portmapper as defined in RFC 1832.</summary>
		/// <remarks>Program number of the portmapper as defined in RFC 1832.</remarks>
		public const int PMAP_PROGRAM = 100000;

		/// <summary>Program version number of the portmapper as defined in RFC 1832.</summary>
		/// <remarks>Program version number of the portmapper as defined in RFC 1832.</remarks>
		public const int PMAP_VERSION = 2;
	}
}
