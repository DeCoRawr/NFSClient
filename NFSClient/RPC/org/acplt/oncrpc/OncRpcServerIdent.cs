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

namespace org.acplt.oncrpc
{
	/// <summary>
	/// The class <code>OncRpcServerIdent</code> represents an tuple
	/// { program, version, protocol, port} uniquely identifying a particular
	/// ONC/RPC server on a given host.
	/// </summary>
	/// <remarks>
	/// The class <code>OncRpcServerIdent</code> represents an tuple
	/// { program, version, protocol, port} uniquely identifying a particular
	/// ONC/RPC server on a given host. This information is used, for instance,
	/// as the ONC/RPC portmap PMAP_GETPORT call parameters.
	/// <p>An <code>OncRpcServerIdent</code> can be directly serialized into an
	/// encoding XDR stream (that is more political correct than "flushed down
	/// the toilet").
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:41 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class OncRpcServerIdent : XdrAble
	{
		/// <summary>The program number of the ONC/RPC server in question.</summary>
		/// <remarks>The program number of the ONC/RPC server in question.</remarks>
		public int program;

		/// <summary>The program version number of the ONC/RPC server in question.</summary>
		/// <remarks>The program version number of the ONC/RPC server in question.</remarks>
		public int version;

		/// <summary>The protocol used for communicating with the ONC/RPC server in question.
		/// 	</summary>
		/// <remarks>
		/// The protocol used for communicating with the ONC/RPC server in question.
		/// This can be one of the constants ("public final static int") defined
		/// in the
		/// <see cref="OncRpcProtocols">OncRpcProtocols</see>
		/// interface.
		/// </remarks>
		public int protocol;

		/// <summary>The port number of the ONC/RPC server in question.</summary>
		/// <remarks>The port number of the ONC/RPC server in question.</remarks>
		public int port;

		/// <summary>
		/// Constuct an <code>OncRpcServerIdent</code> object with senseless
		/// default values for the requested program number, version number,
		/// protocol type and port number.
		/// </summary>
		/// <remarks>
		/// Constuct an <code>OncRpcServerIdent</code> object with senseless
		/// default values for the requested program number, version number,
		/// protocol type and port number.
		/// </remarks>
		public OncRpcServerIdent()
		{
			program = 0;
			version = 0;
			protocol = 0;
			port = 0;
		}

		/// <summary>
		/// Constructs an <code>OncRpcServerIdent</code> object with the
		/// requested program number, version number, protocol type and port
		/// number.
		/// </summary>
		/// <remarks>
		/// Constructs an <code>OncRpcServerIdent</code> object with the
		/// requested program number, version number, protocol type and port
		/// number.
		/// </remarks>
		public OncRpcServerIdent(int program, int version, int protocol, int port)
		{
			this.program = program;
			this.version = version;
			this.protocol = protocol;
			this.port = port;
		}

		/// <summary>
		/// Constructs an <code>OncRpcServerIdent</code> object and restores
		/// its state from the given XDR stream.
		/// </summary>
		/// <remarks>
		/// Constructs an <code>OncRpcServerIdent</code> object and restores
		/// its state from the given XDR stream.
		/// </remarks>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		public OncRpcServerIdent(org.acplt.oncrpc.XdrDecodingStream xdr)
		{
			xdrDecode(xdr);
		}

		/// <summary>
		/// Encodes -- that is: serializes -- an OncRpcServerIdent object
		/// into a XDR stream.
		/// </summary>
		/// <remarks>
		/// Encodes -- that is: serializes -- an OncRpcServerIdent object
		/// into a XDR stream.
		/// </remarks>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public virtual void xdrEncode(org.acplt.oncrpc.XdrEncodingStream xdr)
		{
			xdr.xdrEncodeInt(program);
			xdr.xdrEncodeInt(version);
			xdr.xdrEncodeInt(protocol);
			xdr.xdrEncodeInt(port);
		}

		/// <summary>
		/// Decodes -- that is: deserializes -- an OncRpcServerIdent object
		/// from a XDR stream.
		/// </summary>
		/// <remarks>
		/// Decodes -- that is: deserializes -- an OncRpcServerIdent object
		/// from a XDR stream.
		/// </remarks>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public virtual void xdrDecode(org.acplt.oncrpc.XdrDecodingStream xdr)
		{
			program = xdr.xdrDecodeInt();
			version = xdr.xdrDecodeInt();
			protocol = xdr.xdrDecodeInt();
			port = xdr.xdrDecodeInt();
		}
	}
}
