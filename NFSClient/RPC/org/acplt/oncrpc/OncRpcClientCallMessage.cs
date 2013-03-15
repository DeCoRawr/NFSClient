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
	/// The <code>OncRpcClientCallMessage</code> class represents a remote procedure
	/// call message on the client side.
	/// </summary>
	/// <remarks>
	/// The <code>OncRpcClientCallMessage</code> class represents a remote procedure
	/// call message on the client side.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:40 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class OncRpcClientCallMessage : org.acplt.oncrpc.OncRpcCallMessage
	{
		/// <summary>Constructs and initialises a new ONC/RPC call message header.</summary>
		/// <remarks>Constructs and initialises a new ONC/RPC call message header.</remarks>
		/// <param name="messageId">
		/// An identifier choosen by an ONC/RPC client to uniquely
		/// identify matching call and reply messages.
		/// </param>
		/// <param name="program">Program number of the remote procedure to call.</param>
		/// <param name="version">Program version number of the remote procedure to call.</param>
		/// <param name="procedure">Procedure number (identifier) of the procedure to call.</param>
		/// <param name="auth">Authentication protocol handling object.</param>
		public OncRpcClientCallMessage(int messageId, int program, int version, int procedure
			, org.acplt.oncrpc.OncRpcClientAuth auth) : base(messageId, program, version, procedure
			)
		{
			this.auth = auth;
		}

		/// <summary>
		/// Encodes -- that is: serializes -- a ONC/RPC message header object
		/// into a XDR stream according to RFC 1831.
		/// </summary>
		/// <remarks>
		/// Encodes -- that is: serializes -- a ONC/RPC message header object
		/// into a XDR stream according to RFC 1831.
		/// </remarks>
		/// <param name="xdr">An encoding XDR stream where to put the mess in.</param>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public virtual void xdrEncode(org.acplt.oncrpc.XdrEncodingStream xdr)
		{
			xdr.xdrEncodeInt(messageId);
			xdr.xdrEncodeInt(messageType);
			xdr.xdrEncodeInt(oncRpcVersion);
			xdr.xdrEncodeInt(program);
			xdr.xdrEncodeInt(version);
			xdr.xdrEncodeInt(procedure);
			//
			// Now encode the authentication data. If we have an authentication
			// protocol handling object at hand, then we let do the dirty work
			// for us. Otherwise, we fall back to AUTH_NONE handling.
			//
			if (auth != null)
			{
				auth.xdrEncodeCredVerf(xdr);
			}
			else
			{
				xdr.xdrEncodeInt(org.acplt.oncrpc.OncRpcAuthType.ONCRPC_AUTH_NONE);
				xdr.xdrEncodeInt(0);
				xdr.xdrEncodeInt(org.acplt.oncrpc.OncRpcAuthType.ONCRPC_AUTH_NONE);
				xdr.xdrEncodeInt(0);
			}
		}

		/// <summary>
		/// Client-side authentication protocol handling object to use when
		/// decoding the reply message.
		/// </summary>
		/// <remarks>
		/// Client-side authentication protocol handling object to use when
		/// decoding the reply message.
		/// </remarks>
		internal org.acplt.oncrpc.OncRpcClientAuth auth;
	}
}
