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
	/// The <code>OncRpcGetPortResult</code> class represents the result from
	/// a PMAP_GETPORT remote procedure call to the ONC/RPC portmapper.
	/// </summary>
	/// <remarks>
	/// The <code>OncRpcGetPortResult</code> class represents the result from
	/// a PMAP_GETPORT remote procedure call to the ONC/RPC portmapper.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:41 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class OncRpcGetPortResult : org.acplt.oncrpc.XdrAble
	{
		/// <summary>The port number of the ONC/RPC in question.</summary>
		/// <remarks>
		/// The port number of the ONC/RPC in question. This is the only interesting
		/// piece of information in this class. Go live with it, you don't have
		/// alternatives.
		/// </remarks>
		public int port;

		/// <summary>
		/// Default constructor for initializing an <code>OncRpcGetPortParams</code>
		/// result object.
		/// </summary>
		/// <remarks>
		/// Default constructor for initializing an <code>OncRpcGetPortParams</code>
		/// result object. It sets the <code>port</code> member to a useless value.
		/// </remarks>
		public OncRpcGetPortResult()
		{
			port = 0;
		}

		/// <summary>
		/// Encodes -- that is: serializes -- an <code>OncRpcGetPortParams</code>
		/// object into a XDR stream.
		/// </summary>
		/// <remarks>
		/// Encodes -- that is: serializes -- an <code>OncRpcGetPortParams</code>
		/// object into a XDR stream.
		/// </remarks>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public virtual void xdrEncode(org.acplt.oncrpc.XdrEncodingStream xdr)
		{
			xdr.xdrEncodeInt(port);
		}

		/// <summary>
		/// Decodes -- that is: deserializes -- an <code>OncRpcGetPortParams</code>
		/// object from a XDR stream.
		/// </summary>
		/// <remarks>
		/// Decodes -- that is: deserializes -- an <code>OncRpcGetPortParams</code>
		/// object from a XDR stream.
		/// </remarks>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public virtual void xdrDecode(org.acplt.oncrpc.XdrDecodingStream xdr)
		{
			port = xdr.xdrDecodeInt();
		}
	}
}
