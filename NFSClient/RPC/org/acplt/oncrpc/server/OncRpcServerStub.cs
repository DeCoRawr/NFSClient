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

using System.Threading;
namespace org.acplt.oncrpc.server
{
	/// <summary>
	/// The abstract <code>OncRpcServerStub</code> class is the base class to
	/// build ONC/RPC-program specific servers upon.
	/// </summary>
	/// <remarks>
	/// The abstract <code>OncRpcServerStub</code> class is the base class to
	/// build ONC/RPC-program specific servers upon. This class is typically
	/// only used by jrpcgen generated servers, which provide a particular
	/// set of remote procedures as defined in a x-file.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.2 $ $Date: 2003/08/14 13:47:04 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public abstract class OncRpcServerStub
	{
		/// <summary>
		/// Array containing ONC/RPC server transport objects which describe what
		/// transports an ONC/RPC server offers for handling ONC/RPC calls.
		/// </summary>
		/// <remarks>
		/// Array containing ONC/RPC server transport objects which describe what
		/// transports an ONC/RPC server offers for handling ONC/RPC calls.
		/// </remarks>
		public org.acplt.oncrpc.server.OncRpcServerTransport[] transports;

		/// <summary>
		/// Array containing program and version numbers tuples this server is
		/// willing to handle.
		/// </summary>
		/// <remarks>
		/// Array containing program and version numbers tuples this server is
		/// willing to handle.
		/// </remarks>
		public org.acplt.oncrpc.server.OncRpcServerTransportRegistrationInfo[] info;

		/// <summary>
		/// Notification flag for signalling the server to stop processing
		/// incomming remote procedure calls and to shut down.
		/// </summary>
		/// <remarks>
		/// Notification flag for signalling the server to stop processing
		/// incomming remote procedure calls and to shut down.
		/// </remarks>
		internal object shutdownSignal = new object();

		/// <summary>
		/// All inclusive convenience method: register server transports with
		/// portmapper, then run the call dispatcher until the server is signalled
		/// to shut down, and finally deregister the transports.
		/// </summary>
		/// <remarks>
		/// All inclusive convenience method: register server transports with
		/// portmapper, then run the call dispatcher until the server is signalled
		/// to shut down, and finally deregister the transports.
		/// </remarks>
		/// <exception cref="org.acplt.oncrpc.OncRpcException">
		/// if the portmapper can not be contacted
		/// successfully.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// if a severe network I/O error occurs in the
		/// server from which it can not recover (like severe exceptions thrown
		/// when waiting for now connections on a server socket).
		/// </exception>
		public virtual void run()
		{
			//
			// Ignore all problems during unregistration.
			//
            try
            {
                try
                {
                    unregister(transports);
                }
                catch (OncRpcException)
                {
                }
                register(transports);
                run(transports);
                try
                {
                    unregister(transports);
                }
                catch (OncRpcException)
                {
                }
            }
            finally
            {
                Close(transports);
            }
		}

		/// <summary>Register a set of server transports with the local portmapper.</summary>
		/// <remarks>Register a set of server transports with the local portmapper.</remarks>
		/// <param name="transports">
		/// Array of server transport objects to register,
		/// which will later handle incomming remote procedure call requests.
		/// </param>
		/// <exception cref="org.acplt.oncrpc.OncRpcException">
		/// if the portmapper could not be contacted
		/// successfully.
		/// </exception>
		public virtual void register(org.acplt.oncrpc.server.OncRpcServerTransport[] transports
			)
		{
			int size = transports.Length;
			for (int idx = 0; idx < size; ++idx)
			{
				transports[idx].register();
			}
		}

		/// <summary>
		/// Process incomming remote procedure call requests from all specified
		/// transports.
		/// </summary>
		/// <remarks>
		/// Process incomming remote procedure call requests from all specified
		/// transports. To end processing and to shut the server down signal
		/// the
		/// <see cref="shutdownSignal">shutdownSignal</see>
		/// object. Note that the thread on which
		/// <code>run()</code> is called will ignore any interruptions and
		/// will silently swallow them.
		/// </remarks>
		/// <param name="transports">
		/// Array of server transport objects for which
		/// processing of remote procedure call requests should be done.
		/// </param>
		public virtual void run(OncRpcServerTransport[] transports
			)
		{
			int size = transports.Length;
			for (int idx = 0; idx < size; ++idx)
			{
				transports[idx].listen();
			}
			//
			// Loop and wait for the shutdown flag to become signalled. If the
			// server's main thread gets interrupted it will not shut itself
			// down. It can only be stopped by signalling the shutdownSignal
			// object.
			//
			for (; ; )
			{
				lock (shutdownSignal)
				{
					try
					{
						Monitor.Wait(shutdownSignal);
						break;
					}
					catch (System.Exception)
					{
					}
				}
			}
		}

		/// <summary>
		/// Notify the RPC server to stop processing of remote procedure call
		/// requests as soon as possible.
		/// </summary>
		/// <remarks>
		/// Notify the RPC server to stop processing of remote procedure call
		/// requests as soon as possible. Note that each transport has its own
		/// thread, so processing will not stop before the transports have been
		/// closed by calling the
		/// <see cref="close(org.acplt.oncrpc.server.OncRpcServerTransport[])">close(org.acplt.oncrpc.server.OncRpcServerTransport[])
		/// 	</see>
		/// method of the server.
		/// </remarks>
		public virtual void stopRpcProcessing()
		{
			if (shutdownSignal != null)
			{
				lock (shutdownSignal)
				{
					Monitor.Pulse(shutdownSignal);
				}
			}
		}

		/// <summary>Unregister a set of server transports from the local portmapper.</summary>
		/// <remarks>Unregister a set of server transports from the local portmapper.</remarks>
		/// <param name="transports">Array of server transport objects to unregister.</param>
		/// <exception cref="org.acplt.oncrpc.OncRpcException">
		/// with a reason of
		/// <see cref="org.acplt.oncrpc.OncRpcException.RPC_FAILED">OncRpcException.RPC_FAILED
		/// 	</see>
		/// if
		/// the portmapper could not be contacted successfully. Note that
		/// it is not considered an error to remove a non-existing entry from
		/// the portmapper.
		/// </exception>
		public virtual void unregister(org.acplt.oncrpc.server.OncRpcServerTransport[] transports
			)
		{
			int size = transports.Length;
			for (int idx = 0; idx < size; ++idx)
			{
				transports[idx].unregister();
			}
		}

		/// <summary>Close all transports listed in a set of server transports.</summary>
		/// <remarks>
		/// Close all transports listed in a set of server transports. Only
		/// by calling this method processing of remote procedure calls by
		/// individual transports can be stopped. This is because every server
		/// transport is handled by its own thread.
		/// </remarks>
		/// <param name="transports">Array of server transport objects to close.</param>
		public virtual void Close(OncRpcServerTransport[] transports
			)
		{
			int size = transports.Length;
			for (int idx = 0; idx < size; ++idx)
			{
				transports[idx].Close();
			}
		}

		/// <summary>Set the character encoding for deserializing strings.</summary>
		/// <remarks>Set the character encoding for deserializing strings.</remarks>
		/// <param name="characterEncoding">
		/// the encoding to use for deserializing strings.
		/// If <code>null</code>, the system's default encoding is to be used.
		/// </param>
		public virtual void setCharacterEncoding(string characterEncoding)
		{
			this.characterEncoding = characterEncoding;
			int size = transports.Length;
			for (int idx = 0; idx < size; ++idx)
			{
				transports[idx].setCharacterEncoding(characterEncoding);
			}
		}

		/// <summary>Get the character encoding for deserializing strings.</summary>
		/// <remarks>Get the character encoding for deserializing strings.</remarks>
		/// <returns>
		/// the encoding currently used for deserializing strings.
		/// If <code>null</code>, then the system's default encoding is used.
		/// </returns>
		public virtual string getCharacterEncoding()
		{
			return characterEncoding;
		}

		/// <summary>
		/// Encoding to use when deserializing strings or <code>null</code> if
		/// the system's default encoding should be used.
		/// </summary>
		/// <remarks>
		/// Encoding to use when deserializing strings or <code>null</code> if
		/// the system's default encoding should be used.
		/// </remarks>
		private string characterEncoding;
	}
}
