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
	/// Objects of class <code>OncRpcDumpResult</code> represent the outcome of
	/// the PMAP_DUMP operation on a portmapper.
	/// </summary>
	/// <remarks>
	/// Objects of class <code>OncRpcDumpResult</code> represent the outcome of
	/// the PMAP_DUMP operation on a portmapper. <code>OncRpcDumpResult</code>s are
	/// (de-)serializeable, so they can be flushed down XDR streams.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:41 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class OncRpcDumpResult : org.acplt.oncrpc.XdrAble
	{
		/// <summary>
		/// Vector of server ident objects describing the currently registered
		/// ONC/RPC servers (also known as "programmes").
		/// </summary>
		/// <remarks>
		/// Vector of server ident objects describing the currently registered
		/// ONC/RPC servers (also known as "programmes").
		/// </remarks>
		public System.Collections.ArrayList servers;

		/// <summary>Initialize an <code>OncRpcServerIdent</code> object.</summary>
		/// <remarks>
		/// Initialize an <code>OncRpcServerIdent</code> object. Afterwards, the
		/// <code>servers</code> field is initialized to contain no elements.
		/// </remarks>
		public OncRpcDumpResult()
		{
			servers = new System.Collections.ArrayList();
		}

		/// <summary>
		/// Encodes -- that is: serializes -- the result of a PMAP_DUMP operationg
		/// into a XDR stream.
		/// </summary>
		/// <remarks>
		/// Encodes -- that is: serializes -- the result of a PMAP_DUMP operationg
		/// into a XDR stream.
		/// </remarks>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public virtual void xdrEncode(org.acplt.oncrpc.XdrEncodingStream xdr)
		{
			if (servers == null)
			{
				xdr.xdrEncodeBoolean(false);
			}
			else
			{
				//
				// Now encode all server ident objects into the xdr stream. Each
				// object is preceeded by a boolan, which indicates to the receiver
				// whether an object follows. After the last object has been
				// encoded the receiver will find a boolean false in the stream.
				//
				int count = servers.Count;
				int index = 0;
				while (count > 0)
				{
					xdr.xdrEncodeBoolean(true);
					((org.acplt.oncrpc.XdrAble)servers[index]).xdrEncode(xdr);
					index++;
					count--;
				}
				xdr.xdrEncodeBoolean(false);
			}
		}

		/// <summary>
		/// Decodes -- that is: deserializes -- the result from a PMAP_DUMP remote
		/// procedure call from a XDR stream.
		/// </summary>
		/// <remarks>
		/// Decodes -- that is: deserializes -- the result from a PMAP_DUMP remote
		/// procedure call from a XDR stream.
		/// </remarks>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public virtual void xdrDecode(org.acplt.oncrpc.XdrDecodingStream xdr)
		{
			//
			// Calling removeAllElements() instead of clear() preserves
			// pre-JDK2 compatibility.
			//
			servers.Clear();
			//
			// Pull the server ident object off the xdr stream. Each object is
			// preceeded by a boolean value indicating whether there is still an
			// object in the pipe.
			//
			while (xdr.xdrDecodeBoolean())
			{
				servers.Add(new org.acplt.oncrpc.OncRpcServerIdent(xdr));
			}
		}
	}
}
