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
namespace org.acplt.oncrpc
{
	/// <summary>
	/// The abstract <code>OncRpcClientStub</code> class is the base class to
	/// build ONC/RPC-program specific clients upon.
	/// </summary>
	/// <remarks>
	/// The abstract <code>OncRpcClientStub</code> class is the base class to
	/// build ONC/RPC-program specific clients upon. This class is typically
	/// only used by jrpcgen generated clients, which provide a particular
	/// set of remote procedures as defined in a x-file.
	/// <p>When you do not need the client proxy object any longer, you should
	/// return the resources it occupies to the system. Use the
	/// <see cref="close()">close()</see>
	/// method for this.
	/// <pre>
	/// client.close();
	/// client = null; // Hint to the garbage (wo)man
	/// </pre>
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <seealso cref="OncRpcTcpClient">OncRpcTcpClient</seealso>
	/// <seealso cref="OncRpcUdpClient">OncRpcUdpClient</seealso>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:44 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public abstract class OncRpcClientStub
	{
		/// <summary>
		/// Construct a new <code>OncRpcClientStub</code> for communication with
		/// a remote ONC/RPC server.
		/// </summary>
		/// <remarks>
		/// Construct a new <code>OncRpcClientStub</code> for communication with
		/// a remote ONC/RPC server.
		/// </remarks>
		/// <param name="host">Host address where the desired ONC/RPC server resides.</param>
		/// <param name="program">Program number of the desired ONC/RPC server.</param>
		/// <param name="version">Version number of the desired ONC/RPC server.</param>
		/// <param name="protocol">
		/// 
		/// <see cref="OncRpcProtocols">Protocol</see>
		/// to be used for
		/// ONC/RPC calls. This information is necessary, so port lookups through
		/// the portmapper can be done.
		/// </param>
        /// <param name="useSecurePort">The local binding port will be less than 1024.</param>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public OncRpcClientStub(IPAddress host, int program, int version, int 
			port, int protocol, bool useSecurePort)
		{
			client = org.acplt.oncrpc.OncRpcClient.newOncRpcClient(host, program, version, port
				, protocol, useSecurePort);
		}

		/// <summary>
		/// Construct a new <code>OncRpcClientStub</code> which uses the given
		/// client proxy object for communication with a remote ONC/RPC server.
		/// </summary>
		/// <remarks>
		/// Construct a new <code>OncRpcClientStub</code> which uses the given
		/// client proxy object for communication with a remote ONC/RPC server.
		/// </remarks>
		/// <param name="client">
		/// ONC/RPC client proxy object implementing a particular
		/// IP protocol.
		/// </param>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		public OncRpcClientStub(org.acplt.oncrpc.OncRpcClient client)
		{
			this.client = client;
		}

		/// <summary>
		/// Close the connection to an ONC/RPC server and free all network-related
		/// resources.
		/// </summary>
		/// <remarks>
		/// Close the connection to an ONC/RPC server and free all network-related
		/// resources. Well -- at least hope, that the Java VM will sometimes free
		/// some resources. Sigh.
		/// </remarks>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public virtual void close()
		{
			if (client != null)
			{
				try
				{
					client.close();
				}
				finally
				{
					client = null;
				}
			}
		}

		/// <summary>
		/// Returns ONC/RPC client proxy object used for communication with a
		/// remote ONC/RPC server.
		/// </summary>
		/// <remarks>
		/// Returns ONC/RPC client proxy object used for communication with a
		/// remote ONC/RPC server.
		/// </remarks>
		/// <returns>ONC/RPC client proxy.</returns>
		public virtual org.acplt.oncrpc.OncRpcClient GetClient()
		{
			return client;
		}

		/// <summary>
		/// The real ONC/RPC client which is responsible for handling a particular
		/// IP protocol.
		/// </summary>
		/// <remarks>
		/// The real ONC/RPC client which is responsible for handling a particular
		/// IP protocol.
		/// </remarks>
		protected org.acplt.oncrpc.OncRpcClient client;
	}
}
