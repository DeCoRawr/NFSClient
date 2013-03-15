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
	/// The <code>OncRpcReplyMessage</code> class represents an ONC/RPC reply
	/// message as defined by ONC/RPC in RFC 1831.
	/// </summary>
	/// <remarks>
	/// The <code>OncRpcReplyMessage</code> class represents an ONC/RPC reply
	/// message as defined by ONC/RPC in RFC 1831. Such messages are sent back by
	/// ONC/RPC to servers to clients and contain (in case of real success) the
	/// result of a remote procedure call.
	/// <p>This class and all its derived classes can be encoded only. They are
	/// not able to encode themselves, because they are used solely on the
	/// server side of an ONC/RPC connection.
	/// <p>The decision to define only one single class for the accepted and
	/// rejected replies was driven by the motivation not to use polymorphism
	/// and thus have to upcast and downcast references all the time.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:51 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class OncRpcServerReplyMessage : OncRpcReplyMessage
	{
		/// <summary>
		/// Initializes a new <code>OncRpcReplyMessage</code> object and initializes
		/// its complete state from the given parameters.
		/// </summary>
		/// <remarks>
		/// Initializes a new <code>OncRpcReplyMessage</code> object and initializes
		/// its complete state from the given parameters.
		/// <p>Note that depending on the reply, acceptance and rejectance status
		/// some parameters are unused and can be specified as
		/// <code>UNUSED_PARAMETER</code>.
		/// </remarks>
		/// <param name="call">The ONC/RPC call this reply message corresponds to.</param>
		/// <param name="replyStatus">
		/// The reply status (see
		/// <see cref="org.acplt.oncrpc.OncRpcReplyStatus">org.acplt.oncrpc.OncRpcReplyStatus
		/// 	</see>
		/// ).
		/// </param>
		/// <param name="acceptStatus">
		/// The acceptance state (see
		/// <see cref="org.acplt.oncrpc.OncRpcAcceptStatus">org.acplt.oncrpc.OncRpcAcceptStatus
		/// 	</see>
		/// ).
		/// </param>
		/// <param name="rejectStatus">
		/// The rejectance state (see
		/// <see cref="org.acplt.oncrpc.OncRpcRejectStatus">org.acplt.oncrpc.OncRpcRejectStatus
		/// 	</see>
		/// ).
		/// </param>
		/// <param name="lowVersion">lowest supported version.</param>
		/// <param name="highVersion">highest supported version.</param>
		/// <param name="authStatus">
		/// The autentication state (see
		/// <see cref="org.acplt.oncrpc.OncRpcAuthStatus">org.acplt.oncrpc.OncRpcAuthStatus</see>
		/// ).
		/// </param>
		public OncRpcServerReplyMessage(org.acplt.oncrpc.server.OncRpcServerCallMessage call
			, int replyStatus, int acceptStatus, int rejectStatus, int lowVersion, int highVersion
			, int authStatus) : base(call, replyStatus, acceptStatus, rejectStatus, lowVersion
			, highVersion, authStatus)
		{
			this.auth = call.auth;
		}

		/// <summary>
		/// Encodes -- that is: serializes -- a ONC/RPC reply header object
		/// into a XDR stream.
		/// </summary>
		/// <remarks>
		/// Encodes -- that is: serializes -- a ONC/RPC reply header object
		/// into a XDR stream.
		/// </remarks>
		/// <exception cref="org.acplt.oncrpc.OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		public virtual void xdrEncode(org.acplt.oncrpc.XdrEncodingStream xdr)
		{
			xdr.xdrEncodeInt(messageId);
			xdr.xdrEncodeInt(messageType);
			xdr.xdrEncodeInt(replyStatus);
			switch (replyStatus)
			{
				case org.acplt.oncrpc.OncRpcReplyStatus.ONCRPC_MSG_ACCEPTED:
				{
					//
					// Encode the information returned for accepted message calls.
					//
					// First encode the authentification data. If someone has
					// nulled (nuked?) the authentication protocol handling object
					// from the call information object, then we can still fall back
					// to sending AUTH_NONE replies...
					//
					if (auth != null)
					{
						auth.xdrEncodeVerf(xdr);
					}
					else
					{
						xdr.xdrEncodeInt(org.acplt.oncrpc.OncRpcAuthType.ONCRPC_AUTH_NONE);
						xdr.xdrEncodeInt(0);
					}
					//
					// Even if the call was accepted by the server, it can still
					// indicate an error. Depending on the status of the accepted
					// call we have to send back an indication about the range of
					// versions we support of a particular program (server).
					//
					xdr.xdrEncodeInt(acceptStatus);
					switch (acceptStatus)
					{
						case org.acplt.oncrpc.OncRpcAcceptStatus.ONCRPC_PROG_MISMATCH:
						{
							xdr.xdrEncodeInt(lowVersion);
							xdr.xdrEncodeInt(highVersion);
							break;
						}

						default:
						{
							//
							// Otherwise "open ended set of problem", like the author
							// of Sun's ONC/RPC source once wrote...
							//
							break;
						}
					}
					break;
				}

				case org.acplt.oncrpc.OncRpcReplyStatus.ONCRPC_MSG_DENIED:
				{
					//
					// Encode the information returned for denied message calls.
					//
					xdr.xdrEncodeInt(rejectStatus);
					switch (rejectStatus)
					{
						case org.acplt.oncrpc.OncRpcRejectStatus.ONCRPC_RPC_MISMATCH:
						{
							xdr.xdrEncodeInt(lowVersion);
							xdr.xdrEncodeInt(highVersion);
							break;
						}

						case org.acplt.oncrpc.OncRpcRejectStatus.ONCRPC_AUTH_ERROR:
						{
							xdr.xdrEncodeInt(authStatus);
							break;
						}

						default:
						{
							break;
						}
					}
					break;
				}
			}
		}

		/// <summary>Contains the authentication protocol handling object.</summary>
		/// <remarks>Contains the authentication protocol handling object.</remarks>
		internal org.acplt.oncrpc.server.OncRpcServerAuth auth;
	}
}
