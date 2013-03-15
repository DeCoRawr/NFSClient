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

namespace org.acplt.oncrpc.server
{
	/// <summary>
	/// The <code>OncRpcServerCallMessage</code> class represents an ONC/RPC
	/// call message on the server side.
	/// </summary>
	/// <remarks>
	/// The <code>OncRpcServerCallMessage</code> class represents an ONC/RPC
	/// call message on the server side. For this reasons it just handles
	/// decoding of call messages but can not do any encoding. This class is
	/// also responsible for pulling off authentication information from the
	/// wire and converting it into appropriate authentication protocol handling
	/// objects. As with all good management, this class therefor delegates this
	/// somehow unpleasant work to the server-side authentication protocol handling
	/// classes.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <seealso cref="OncRpcServerAuth">OncRpcServerAuth</seealso>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:51 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class OncRpcServerCallMessage : OncRpcCallMessage
	{
		/// <summary>
		/// Decodes -- that is: deserializes -- a ONC/RPC message header object
		/// from a XDR stream according to RFC 1831.
		/// </summary>
		/// <remarks>
		/// Decodes -- that is: deserializes -- a ONC/RPC message header object
		/// from a XDR stream according to RFC 1831.
		/// </remarks>
		/// <param name="xdr">A decoding XDR stream from which to receive all the mess.</param>
		/// <exception cref="org.acplt.oncrpc.OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		public virtual void xdrDecode(org.acplt.oncrpc.XdrDecodingStream xdr)
		{
			messageId = xdr.xdrDecodeInt();
			//
			// Make sure that we are really decoding an ONC/RPC message call
			// header. Otherwise, throw the appropriate OncRpcException exception.
			//
			messageType = xdr.xdrDecodeInt();
			if (messageType != org.acplt.oncrpc.OncRpcMessageType.ONCRPC_CALL)
			{
				throw (new org.acplt.oncrpc.OncRpcException(org.acplt.oncrpc.OncRpcException.RPC_WRONGMESSAGE
					));
			}
			//
			// Make sure that the other side is talking the right slang --
			// we will only understand version 2 slang of ONC/RPC.
			//
			oncRpcVersion = xdr.xdrDecodeInt();
			if (oncRpcVersion != ONCRPC_VERSION)
			{
				throw (new org.acplt.oncrpc.OncRpcException(org.acplt.oncrpc.OncRpcException.RPC_VERSMISMATCH
					));
			}
			//
			// Now decode the remaining fields of the call header.
			//
			program = xdr.xdrDecodeInt();
			version = xdr.xdrDecodeInt();
			procedure = xdr.xdrDecodeInt();
			//
			// Last comes the authentication data. Note that the "factory" hidden
			// within xdrNew() will graciously recycle any old authentication
			// protocol handling object if it is of the same authentication type
			// as the new one just coming in from the XDR wire.
			//
			auth = org.acplt.oncrpc.server.OncRpcServerAuth.xdrNew(xdr, auth);
		}

		/// <summary>
		/// Contains the authentication protocol handling object retrieved together
		/// with the call message itself.
		/// </summary>
		/// <remarks>
		/// Contains the authentication protocol handling object retrieved together
		/// with the call message itself.
		/// </remarks>
		public org.acplt.oncrpc.server.OncRpcServerAuth auth;
	}
}
