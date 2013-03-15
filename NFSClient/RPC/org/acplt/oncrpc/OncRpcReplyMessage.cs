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
	/// The <code>OncRpcReplyMessage</code> class represents an ONC/RPC reply
	/// message as defined by ONC/RPC in RFC 1831.
	/// </summary>
	/// <remarks>
	/// The <code>OncRpcReplyMessage</code> class represents an ONC/RPC reply
	/// message as defined by ONC/RPC in RFC 1831. Such messages are sent back by
	/// ONC/RPC to servers to clients and contain (in case of real success) the
	/// result of a remote procedure call.
	/// <p>The decision to define only one single class for the accepted and
	/// rejected replies was driven by the motivation not to use polymorphism
	/// and thus have to upcast and downcast references all the time.
	/// <p>The derived classes are only provided for convinience on the server
	/// side.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.2 $ $Date: 2003/08/14 07:56:59 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public abstract class OncRpcReplyMessage : OncRpcMessage
	{
		/// <summary>The reply status of the reply message.</summary>
		/// <remarks>
		/// The reply status of the reply message. This can be either
		/// <see cref="OncRpcReplyStatus.ONCRPC_MSG_ACCEPTED">OncRpcReplyStatus.ONCRPC_MSG_ACCEPTED
		/// 	</see>
		/// or
		/// <see cref="OncRpcReplyStatus.ONCRPC_MSG_DENIED">OncRpcReplyStatus.ONCRPC_MSG_DENIED
		/// 	</see>
		/// . Depending on the value
		/// of this field, other fields of an instance of
		/// <code>OncRpcReplyMessage</code> become important.
		/// <p>The decision to define only one single class for the accepted and
		/// rejected replies was driven by the motivation not to use polymorphism
		/// and thus have to upcast and downcast references all the time.
		/// </remarks>
		public int replyStatus;

		/// <summary>
		/// Acceptance status in case this reply was sent in response to an
		/// accepted call (
		/// <see cref="OncRpcReplyStatus.ONCRPC_MSG_ACCEPTED">OncRpcReplyStatus.ONCRPC_MSG_ACCEPTED
		/// 	</see>
		/// ). This
		/// field can take any of the values defined in the
		/// <see cref="OncRpcAcceptStatus">OncRpcAcceptStatus</see>
		/// interface.
		/// <p>Note that even for accepted calls that only in the case of
		/// <see cref="OncRpcAcceptStatus.ONCRPC_SUCCESS">OncRpcAcceptStatus.ONCRPC_SUCCESS</see>
		/// result data will follow
		/// the reply message header.
		/// </summary>
		public int acceptStatus;

		/// <summary>
		/// Rejectance status in case this reply sent in response to a
		/// rejected call (
		/// <see cref="OncRpcReplyStatus.ONCRPC_MSG_DENIED">OncRpcReplyStatus.ONCRPC_MSG_DENIED
		/// 	</see>
		/// ). This
		/// field can take any of the values defined in the
		/// <see cref="OncRpcRejectStatus">OncRpcRejectStatus</see>
		/// interface.
		/// </summary>
		public int rejectStatus;

		/// <summary>
		/// Lowest supported version in case of
		/// <see cref="OncRpcRejectStatus.ONCRPC_RPC_MISMATCH">OncRpcRejectStatus.ONCRPC_RPC_MISMATCH
		/// 	</see>
		/// and
		/// <see cref="OncRpcAcceptStatus.ONCRPC_PROG_MISMATCH">OncRpcAcceptStatus.ONCRPC_PROG_MISMATCH
		/// 	</see>
		/// .
		/// </summary>
		public int lowVersion;

		/// <summary>
		/// Highest supported version in case of
		/// <see cref="OncRpcRejectStatus.ONCRPC_RPC_MISMATCH">OncRpcRejectStatus.ONCRPC_RPC_MISMATCH
		/// 	</see>
		/// and
		/// <see cref="OncRpcAcceptStatus.ONCRPC_PROG_MISMATCH">OncRpcAcceptStatus.ONCRPC_PROG_MISMATCH
		/// 	</see>
		/// .
		/// </summary>
		public int highVersion;

		/// <summary>
		/// Contains the reason for authentification failure in the case
		/// of
		/// <see cref="OncRpcRejectStatus.ONCRPC_AUTH_ERROR">OncRpcRejectStatus.ONCRPC_AUTH_ERROR
		/// 	</see>
		/// .
		/// </summary>
		public int authStatus;

		/// <summary>
		/// Initializes a new <code>OncRpcReplyMessage</code> object to represent
		/// an invalid state.
		/// </summary>
		/// <remarks>
		/// Initializes a new <code>OncRpcReplyMessage</code> object to represent
		/// an invalid state. This default constructor should only be used if in the
		/// next step the real state of the reply message is immediately decoded
		/// from a XDR stream.
		/// </remarks>
		public OncRpcReplyMessage() : base(0)
		{
			messageType = org.acplt.oncrpc.OncRpcMessageType.ONCRPC_REPLY;
			replyStatus = org.acplt.oncrpc.OncRpcReplyStatus.ONCRPC_MSG_ACCEPTED;
			acceptStatus = org.acplt.oncrpc.OncRpcAcceptStatus.ONCRPC_SYSTEM_ERR;
			rejectStatus = UNUSED_PARAMETER;
			lowVersion = 0;
			highVersion = 0;
			authStatus = UNUSED_PARAMETER;
		}

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
		/// <see cref="OncRpcReplyStatus">OncRpcReplyStatus</see>
		/// ).
		/// </param>
		/// <param name="acceptStatus">
		/// The acceptance state (see
		/// <see cref="OncRpcAcceptStatus">OncRpcAcceptStatus</see>
		/// ).
		/// </param>
		/// <param name="rejectStatus">
		/// The rejectance state (see
		/// <see cref="OncRpcRejectStatus">OncRpcRejectStatus</see>
		/// ).
		/// </param>
		/// <param name="lowVersion">lowest supported version.</param>
		/// <param name="highVersion">highest supported version.</param>
		/// <param name="authStatus">
		/// The autentication state (see
		/// <see cref="OncRpcAuthStatus">OncRpcAuthStatus</see>
		/// ).
		/// </param>
		public OncRpcReplyMessage(org.acplt.oncrpc.OncRpcCallMessage call, int replyStatus
			, int acceptStatus, int rejectStatus, int lowVersion, int highVersion, int authStatus
			) : base(call.messageId)
		{
			messageType = org.acplt.oncrpc.OncRpcMessageType.ONCRPC_REPLY;
			this.replyStatus = replyStatus;
			this.acceptStatus = acceptStatus;
			this.rejectStatus = rejectStatus;
			this.lowVersion = lowVersion;
			this.highVersion = highVersion;
			this.authStatus = authStatus;
		}

		/// <summary>
		/// Dummy, which can be used to identify unused parameters when constructing
		/// <code>OncRpcReplyMessage</code> objects.
		/// </summary>
		/// <remarks>
		/// Dummy, which can be used to identify unused parameters when constructing
		/// <code>OncRpcReplyMessage</code> objects.
		/// </remarks>
		public const int UNUSED_PARAMETER = 0;
	}
}
