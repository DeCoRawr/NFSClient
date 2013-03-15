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
using System;
namespace org.acplt.oncrpc
{
	/// <summary>
	/// The class <code>OncRpcPortmapClient</code> is a specialized ONC/RPC client,
	/// which can talk to the portmapper on a given host using the famous
	/// UDP/IP datagram-oriented internet protocol.
	/// </summary>
	/// <remarks>
	/// The class <code>OncRpcPortmapClient</code> is a specialized ONC/RPC client,
	/// which can talk to the portmapper on a given host using the famous
	/// UDP/IP datagram-oriented internet protocol. In addition, it is also possible
	/// to contact portmappers using TCP/IP. For this, the constructor of the
	/// <code>OncRpcPortmapClient</code> class also accepts a protocol parameter
	/// (
	/// <see cref="OncRpcPortmapClient(IPAddress, int)">OncRpcPortmapClient(IPAddress, int)
	/// 	</see>
	/// ).
	/// Technically spoken, instances of <code>OncRpcPortmapClient</code> are proxy objects.
	/// <code>OncRpcPortmapClient</code> objects currently speak protocol version
	/// 2. The newer transport-independent protocol versions 3 and 4 are
	/// <b>not</b> supported as the transport-independent ONC/RPC implementation is not
	/// that widely in use due to the brain-damaged design of XTI. If you should
	/// ever have programmed using XTI (transport independent interface) then you'll
	/// know what I mean and probably agree with me. Otherwise, in case you find XTI
	/// the best thing since the Win32 API, please implement the rpcbind protocol
	/// versions 3 and 4 and give it to the community -- thank you.
	/// <p>Here are some simple examples of how to use the portmapper proxy object.
	/// We first start with one of the most interesting operations, which can be
	/// performed on portmappers, querying the port of a local or remote ONC/RPC
	/// server.
	/// <p>To query the port number of an ONC/RPC server, we need to contact the
	/// portmapper at the host machine where the server is running. The following
	/// code snippet just contacts the local portmapper. <code>try</code> blocks
	/// are ommited for brevity -- but remember that you almost allways need to catch
	/// <see cref="OncRpcException">OncRpcException</see>
	/// as well as <code>IOException</code>.
	/// <pre>
	/// OncRpcPortmapClient portmap =
	/// new OncRpcPortmapClient(InetAddress.getByName("localhost"));
	/// </pre>
	/// <p>With the portmapper proxy object in our hands we can now ask for the port
	/// number of a particular ONC/RPC server. In this (ficious) example we ask for
	/// the ONC/RPC program (server) number <code>0x49678</code> (by coincidence this
	/// happens to be the program number of the <a href="http://www.acplt.org/ks">ACPLT/KS</a>
	/// protocol). To ask for the port number of a given program number, use the
	/// <see cref="getPort(int, int, int)">getPort(...)</see>
	/// method.
	/// <pre>
	/// int port;
	/// try {
	/// port = portmap.getPort(0x49678, 1, OncRpcProtocols.ONCRPC_UDP);
	/// } catch ( OncRpcProgramNotRegisteredException e ) {
	/// System.out.println("ONC/RPC program server not found");
	/// System.exit(0);
	/// } catch ( OncRpcException e ) {
	/// System.out.println("Could not contact portmapper:");
	/// e.printStackTrace(System.out);
	/// System.exit(0);
	/// }
	/// System.out.println("Program available at port " + port);
	/// </pre>
	/// <p>In the call to
	/// <see cref="getPort(int, int, int)">getPort(...)</see>
	/// , the
	/// first parameter specifies the ONC/RPC program number, the secondm parameter
	/// specifies the program's version number, and the third parameter specifies
	/// the IP protocol to use when issueing ONC/RPC calls. Currently, only
	/// <see cref="OncRpcProtocols.ONCRPC_UDP">OncRpcProtocols.ONCRPC_UDP</see>
	/// and
	/// <see cref="OncRpcProtocols.ONCRPC_TCP">OncRpcProtocols.ONCRPC_TCP</see>
	/// are
	/// supported. But who needs other protocols anyway?!
	/// <p>In case
	/// <see cref="getPort(int, int, int)">getPort(...)</see>
	/// succeeds, it
	/// returns the number of the port where the appropriate ONC/RPC server waits
	/// for incoming ONC/RPC calls. If the ONC/RPC program is not registered with
	/// the particular ONC/RPC portmapper, an
	/// <see cref="OncRpcProgramNotRegisteredException">OncRpcProgramNotRegisteredException
	/// 	</see>
	/// is thrown (which is a subclass of
	/// <see cref="OncRpcException">OncRpcException</see>
	/// with a detail
	/// reason of
	/// <see cref="OncRpcException.RPC_PROGNOTREGISTERED">OncRpcException.RPC_PROGNOTREGISTERED
	/// 	</see>
	/// .
	/// <p>A second typical example of how to use the portmapper is retrieving a
	/// list of the currently registered servers. We use the
	/// <see cref="listServers()">listServers()</see>
	/// method for this purpose in the
	/// following example, and print the list we got.
	/// <pre>
	/// OncRpcServerIdent [] list = null;
	/// try {
	/// list = portmap.listServers();
	/// } catch ( OncRpcException e ) {
	/// e.printStackTrace(System.out);
	/// System.exit(20);
	/// }
	/// for ( int i = 0; i &lt; list.length; ++i ) {
	/// System.out.println(list[i].program + " " + list[i].version + " "
	/// + list[i].protocol + " " + list[i].port);
	/// }
	/// </pre>
	/// <p>When you do not need the client proxy object any longer, you should
	/// return the resources it occupies to the system. Use the
	/// <see cref="close()">close()</see>
	/// method for this.
	/// <pre>
	/// portmap.close();
	/// portmap = null; // Hint to the garbage (wo)man
	/// </pre>
	/// <p>For another code example, please consult
	/// <code>src/tests/org/acplt/oncrpc/PortmapGetPortTest.java</code>.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <seealso cref="OncRpcClient">OncRpcClient</seealso>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:41 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class OncRpcPortmapClient
	{
		/// <summary>
		/// Constructs and initializes an ONC/RPC client object, which can
		/// communicate with the portmapper at the specified host using the
		/// UDP/IP datagram-oriented internet protocol.
		/// </summary>
		/// <remarks>
		/// Constructs and initializes an ONC/RPC client object, which can
		/// communicate with the portmapper at the specified host using the
		/// UDP/IP datagram-oriented internet protocol.
		/// </remarks>
		/// <param name="host">Host where to contact the portmapper.</param>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public OncRpcPortmapClient(IPAddress host) : this(host, org.acplt.oncrpc.OncRpcProtocols
			.ONCRPC_UDP, 0)
		{
		}

		/// <summary>
		/// Constructs and initializes an ONC/RPC client object, which can
		/// communicate with the portmapper at the given host using the
		/// speicified protocol.
		/// </summary>
		/// <remarks>
		/// Constructs and initializes an ONC/RPC client object, which can
		/// communicate with the portmapper at the given host using the
		/// speicified protocol.
		/// </remarks>
		/// <param name="host">Host where to contact the portmapper.</param>
		/// <param name="protocol">
		/// Protocol to use for contacting the portmapper. This
		/// can be either <code>OncRpcProtocols.ONCRPC_UDP</code> or
		/// <code>OncRpcProtocols.ONCRPC_TCP</code> (HTTP is currently
		/// not supported).
		/// </param>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public OncRpcPortmapClient(IPAddress host, int protocol) : this(host, 
			protocol, -1)
		{
		}

		/// <summary>
		/// Constructs and initializes an ONC/RPC client object, which can
		/// communicate with the portmapper at the given host using the
		/// speicified protocol.
		/// </summary>
		/// <remarks>
		/// Constructs and initializes an ONC/RPC client object, which can
		/// communicate with the portmapper at the given host using the
		/// speicified protocol.
		/// </remarks>
		/// <param name="host">Host where to contact the portmapper.</param>
		/// <param name="protocol">
		/// Protocol to use for contacting the portmapper. This
		/// can be either <code>OncRpcProtocols.ONCRPC_UDP</code> or
		/// <code>OncRpcProtocols.ONCRPC_TCP</code> (HTTP is currently
		/// not supported).
		/// </param>
		/// <param name="timeout">
		/// Timeout in milliseconds for connection operation. This
		/// parameter applies only when using TCP/IP for talking to the
		/// portmapper. A negative timeout indicates that the
		/// implementation-specific timeout setting of the JVM and java.net
		/// implementation should be used instead.
		/// </param>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public OncRpcPortmapClient(IPAddress host, int protocol, int timeout)
		{
			switch (protocol)
			{
				case org.acplt.oncrpc.OncRpcProtocols.ONCRPC_UDP:
				{
					portmapClient = new org.acplt.oncrpc.OncRpcUdpClient(host, PMAP_PROGRAM, PMAP_VERSION
						, PMAP_PORT, false);
					break;
				}

				case org.acplt.oncrpc.OncRpcProtocols.ONCRPC_TCP:
				{
					portmapClient = new org.acplt.oncrpc.OncRpcTcpClient(host, PMAP_PROGRAM, PMAP_VERSION
						, PMAP_PORT, 0, timeout, false);
					// default buff size
					break;
				}

				default:
				{
					throw (new org.acplt.oncrpc.OncRpcException(org.acplt.oncrpc.OncRpcException.RPC_UNKNOWNPROTO
						));
				}
			}
		}

		/// <summary>Closes the connection to the portmapper.</summary>
		/// <remarks>Closes the connection to the portmapper.</remarks>
		/// <exception cref="OncRpcException">OncRpcException</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public virtual void close()
		{
			portmapClient.close();
		}

		/// <summary>
		/// Returns the client proxy object used for communicating with the
		/// portmapper.
		/// </summary>
		/// <remarks>
		/// Returns the client proxy object used for communicating with the
		/// portmapper.
		/// </remarks>
		/// <returns>portmap client proxy object (subclass of <code>OncRpcClient</code>).</returns>
		public virtual org.acplt.oncrpc.OncRpcClient getOncRpcClient()
		{
			return portmapClient;
		}

		/// <summary>
		/// Asks the portmapper this <code>OncRpcPortmapClient</code> object is
		/// a proxy for, for the port number of a particular ONC/RPC server
		/// identified by the information tuple {program number, program version,
		/// protocol}.
		/// </summary>
		/// <remarks>
		/// Asks the portmapper this <code>OncRpcPortmapClient</code> object is
		/// a proxy for, for the port number of a particular ONC/RPC server
		/// identified by the information tuple {program number, program version,
		/// protocol}.
		/// </remarks>
		/// <param name="program">Program number of the remote procedure call in question.</param>
		/// <param name="version">Program version number.</param>
		/// <param name="protocol">
		/// Protocol lateron used for communication with the
		/// ONC/RPC server in question. This can be one of the protocols constants
		/// defined in the
		/// <see cref="OncRpcProtocols">OncRpcProtocols</see>
		/// interface.
		/// </param>
		/// <returns>port number of ONC/RPC server in question.</returns>
		/// <exception cref="OncRpcException">
		/// if the portmapper is not available (detail is
		/// <see cref="OncRpcException.RPC_PMAPFAILURE">OncRpcException.RPC_PMAPFAILURE</see>
		/// ).
		/// </exception>
		/// <exception cref="OncRpcProgramNotRegisteredException">
		/// if the requested program
		/// is not available.
		/// </exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public virtual int getPort(int program, int version, int protocol)
		{
			//
			// Fill in the request parameters. Note that params.port is
			// not used. BTW - it is automatically initialized as 0 by the
			// constructor of the OncRpcServerParams class.
			//
			org.acplt.oncrpc.OncRpcServerIdent @params = new org.acplt.oncrpc.OncRpcServerIdent
				(program, version, protocol, 0);
			org.acplt.oncrpc.OncRpcGetPortResult result = new org.acplt.oncrpc.OncRpcGetPortResult
				();
			//
			// Try to contact the portmap process. If something goes "boing"
			// at this stage, then rethrow the exception as a generic portmap
			// failure exception. Otherwise, if the port number returned is
			// zero, then no appropriate server was found. In this case,
			// throw an exception, that the program requested could not be
			// found.
			//
			try
			{
				portmapClient.call(org.acplt.oncrpc.OncRpcPortmapServices.PMAP_GETPORT, @params, 
					result);
			}
			catch (org.acplt.oncrpc.OncRpcException)
			{
				throw (new org.acplt.oncrpc.OncRpcException(org.acplt.oncrpc.OncRpcException.RPC_PMAPFAILURE
					));
			}
			//
			// In case the program is not registered, throw an exception too.
			//
			if (result.port == 0)
			{
				throw (new org.acplt.oncrpc.OncRpcProgramNotRegisteredException());
			}
			return result.port;
		}

		/// <summary>
		/// Register an ONC/RPC with the given program number, version and protocol
		/// at the given port with the portmapper.
		/// </summary>
		/// <remarks>
		/// Register an ONC/RPC with the given program number, version and protocol
		/// at the given port with the portmapper.
		/// </remarks>
		/// <param name="program">The number of the program to be registered.</param>
		/// <param name="version">The version number of the program.</param>
		/// <param name="protocol">
		/// The protocol spoken by the ONC/RPC server. Can be one
		/// of the
		/// <see cref="OncRpcProtocols">OncRpcProtocols</see>
		/// constants.
		/// </param>
		/// <param name="port">The port number where the ONC/RPC server can be reached.</param>
		/// <returns>
		/// Indicates whether registration succeeded (<code>true</code>) or
		/// was denied by the portmapper (<code>false</code>).
		/// </returns>
		/// <exception cref="OncRpcException">
		/// if the portmapper is not available (detail is
		/// <see cref="OncRpcException.RPC_PMAPFAILURE">OncRpcException.RPC_PMAPFAILURE</see>
		/// ).
		/// </exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public virtual bool setPort(int program, int version, int protocol, int port)
		{
			//
			// Fill in the request parameters.
			//
			org.acplt.oncrpc.OncRpcServerIdent @params = new org.acplt.oncrpc.OncRpcServerIdent
				(program, version, protocol, port);
			org.acplt.oncrpc.XdrBoolean result = new org.acplt.oncrpc.XdrBoolean(false);
			//
			// Try to contact the portmap process. If something goes "boing"
			// at this stage, then rethrow the exception as a generic portmap
			// failure exception.
			//
			try
			{
				portmapClient.call(org.acplt.oncrpc.OncRpcPortmapServices.PMAP_SET, @params, result
					);
			}
			catch (org.acplt.oncrpc.OncRpcException)
			{
				throw (new org.acplt.oncrpc.OncRpcException(org.acplt.oncrpc.OncRpcException.RPC_PMAPFAILURE
					));
			}
			return result.booleanValue();
		}

		/// <summary>Unregister an ONC/RPC with the given program number and version.</summary>
		/// <remarks>
		/// Unregister an ONC/RPC with the given program number and version. The
		/// portmapper will remove all entries with the same program number and
		/// version, regardless of the protocol and port number.
		/// </remarks>
		/// <param name="program">The number of the program to be unregistered.</param>
		/// <param name="version">The version number of the program.</param>
		/// <returns>
		/// Indicates whether deregistration succeeded (<code>true</code>)
		/// or was denied by the portmapper (<code>false</code>).
		/// </returns>
		/// <exception cref="OncRpcException">
		/// if the portmapper is not available (detail is
		/// <see cref="OncRpcException.RPC_PMAPFAILURE">OncRpcException.RPC_PMAPFAILURE</see>
		/// ).
		/// </exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public virtual bool unsetPort(int program, int version)
		{
			//
			// Fill in the request parameters.
			//
			OncRpcServerIdent @params = new OncRpcServerIdent
				(program, version, 0, 0);
			XdrBoolean result = new XdrBoolean(false);
			//
			// Try to contact the portmap process. If something goes "boing"
			// at this stage, then rethrow the exception as a generic portmap
			// failure exception.
			//
			try
			{
				portmapClient.call(OncRpcPortmapServices.PMAP_UNSET, @params, result
					);
			}
			catch (OncRpcException e)
			{
                // Temp output
                Console.Out.WriteLine(e.Message);
                Console.Out.WriteLine(e.StackTrace);
				throw (new OncRpcException(OncRpcException.RPC_PMAPFAILURE
					));
			}
			return result.booleanValue();
		}

		/// <summary>
		/// Retrieves a list of all registered ONC/RPC servers at the same host
		/// as the contacted portmapper.
		/// </summary>
		/// <remarks>
		/// Retrieves a list of all registered ONC/RPC servers at the same host
		/// as the contacted portmapper.
		/// </remarks>
		/// <returns>
		/// vector of server descriptions (see
		/// class
		/// <see cref="OncRpcServerIdent">OncRpcServerIdent</see>
		/// ).
		/// </returns>
		/// <exception cref="OncRpcException">
		/// if the portmapper is not available (detail is
		/// <see cref="OncRpcException.RPC_PMAPFAILURE">OncRpcException.RPC_PMAPFAILURE</see>
		/// ).
		/// </exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public virtual org.acplt.oncrpc.OncRpcServerIdent[] listServers()
		{
			//
			// Fill in the request parameters.
			//
			org.acplt.oncrpc.OncRpcDumpResult result = new org.acplt.oncrpc.OncRpcDumpResult(
				);
			//
			// Try to contact the portmap process. If something goes "boing"
			// at this stage, then rethrow the exception as a generic portmap
			// failure exception.
			//
			try
			{
				portmapClient.call(org.acplt.oncrpc.OncRpcPortmapServices.PMAP_DUMP, org.acplt.oncrpc.XdrVoid
					.XDR_VOID, result);
			}
			catch (org.acplt.oncrpc.OncRpcException)
			{
				throw (new org.acplt.oncrpc.OncRpcException(org.acplt.oncrpc.OncRpcException.RPC_PMAPFAILURE
					));
			}
			//
			// Copy the server ident object references from the Vector
			// into the vector (array).
			//
			org.acplt.oncrpc.OncRpcServerIdent[] info = new org.acplt.oncrpc.OncRpcServerIdent
				[result.servers.Count];
			result.servers.CopyTo(info);
			return info;
		}

		/// <summary>Ping the portmapper (try to call procedure 0).</summary>
		/// <remarks>Ping the portmapper (try to call procedure 0).</remarks>
		/// <exception cref="OncRpcException">
		/// if the portmapper is not available (detail is
		/// <see cref="OncRpcException.RPC_PMAPFAILURE">OncRpcException.RPC_PMAPFAILURE</see>
		/// ).
		/// </exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public virtual void ping()
		{
			try
			{
				portmapClient.call(0, XdrVoid.XDR_VOID, XdrVoid.XDR_VOID);
			}
			catch (org.acplt.oncrpc.OncRpcException)
			{
				throw (new OncRpcException(OncRpcException.RPC_PMAPFAILURE
					));
			}
		}

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

		/// <summary>
		/// The particular transport-specific ONC/RPC client object used for
		/// talking to the portmapper.
		/// </summary>
		/// <remarks>
		/// The particular transport-specific ONC/RPC client object used for
		/// talking to the portmapper.
		/// </remarks>
		internal org.acplt.oncrpc.OncRpcClient portmapClient;
	}
}
