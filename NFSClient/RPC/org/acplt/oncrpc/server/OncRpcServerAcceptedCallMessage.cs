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
	/// The <code>OncRpcServerAcceptedCallMessage</code> class represents (on the
	/// sender's side) an accepted ONC/RPC call.
	/// </summary>
	/// <remarks>
	/// The <code>OncRpcServerAcceptedCallMessage</code> class represents (on the
	/// sender's side) an accepted ONC/RPC call. In ONC/RPC babble, an "accepted"
	/// call does not mean that it carries a result from the remote procedure
	/// call, but rather that the call was accepted at the basic ONC/RPC level
	/// and no authentification failure or else occured.
	/// <p>This ONC/RPC reply header class is only a convenience for server
	/// implementors.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.2 $ $Date: 2003/08/14 08:10:59 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class OncRpcServerAcceptedCallMessage : OncRpcServerReplyMessage
	{
		/// <summary>
		/// Constructs an <code>OncRpcServerAcceptedCallMessage</code> object which
		/// represents an accepted call, which was also successfully executed,
		/// so the reply will contain information from the remote procedure call.
		/// </summary>
		/// <remarks>
		/// Constructs an <code>OncRpcServerAcceptedCallMessage</code> object which
		/// represents an accepted call, which was also successfully executed,
		/// so the reply will contain information from the remote procedure call.
		/// </remarks>
		/// <param name="call">
		/// The call message header, which is used to construct the
		/// matching reply message header from.
		/// </param>
		public OncRpcServerAcceptedCallMessage(org.acplt.oncrpc.server.OncRpcServerCallMessage
			 call) : base(call, org.acplt.oncrpc.OncRpcReplyStatus.ONCRPC_MSG_ACCEPTED, org.acplt.oncrpc.OncRpcAcceptStatus
			.ONCRPC_SUCCESS, org.acplt.oncrpc.OncRpcReplyMessage.UNUSED_PARAMETER, org.acplt.oncrpc.OncRpcReplyMessage
			.UNUSED_PARAMETER, org.acplt.oncrpc.OncRpcReplyMessage.UNUSED_PARAMETER, org.acplt.oncrpc.OncRpcAuthStatus
			.ONCRPC_AUTH_OK)
		{
		}

		/// <summary>
		/// Constructs an <code>OncRpcAcceptedCallMessage</code> object which
		/// represents an accepted call, which was not necessarily successfully
		/// carried out.
		/// </summary>
		/// <remarks>
		/// Constructs an <code>OncRpcAcceptedCallMessage</code> object which
		/// represents an accepted call, which was not necessarily successfully
		/// carried out. The parameter <code>acceptStatus</code> will then
		/// indicate the exact outcome of the ONC/RPC call.
		/// </remarks>
		/// <param name="call">
		/// The call message header, which is used to construct the
		/// matching reply message header from.
		/// </param>
		/// <param name="acceptStatus">
		/// The accept status of the call. This can be any
		/// one of the constants defined in the
		/// <see cref="org.acplt.oncrpc.OncRpcAcceptStatus">org.acplt.oncrpc.OncRpcAcceptStatus
		/// 	</see>
		/// interface.
		/// </param>
		public OncRpcServerAcceptedCallMessage(org.acplt.oncrpc.server.OncRpcServerCallMessage
			 call, int acceptStatus) : base(call, org.acplt.oncrpc.OncRpcReplyStatus.ONCRPC_MSG_ACCEPTED
			, acceptStatus, org.acplt.oncrpc.OncRpcReplyMessage.UNUSED_PARAMETER, org.acplt.oncrpc.OncRpcReplyMessage
			.UNUSED_PARAMETER, org.acplt.oncrpc.OncRpcReplyMessage.UNUSED_PARAMETER, org.acplt.oncrpc.OncRpcAuthStatus
			.ONCRPC_AUTH_OK)
		{
		}

		/// <summary>
		/// Constructs an <code>OncRpcAcceptedCallMessage</code> object for an
		/// accepted call with an unsupported version.
		/// </summary>
		/// <remarks>
		/// Constructs an <code>OncRpcAcceptedCallMessage</code> object for an
		/// accepted call with an unsupported version. The reply will contain
		/// information about the lowest and highest supported version.
		/// </remarks>
		/// <param name="call">
		/// The call message header, which is used to construct the
		/// matching reply message header from.
		/// </param>
		/// <param name="low">Lowest program version supported by this ONC/RPC server.</param>
		/// <param name="high">Highest program version supported by this ONC/RPC server.</param>
		public OncRpcServerAcceptedCallMessage(org.acplt.oncrpc.server.OncRpcServerCallMessage
			 call, int low, int high) : base(call, org.acplt.oncrpc.OncRpcReplyStatus.ONCRPC_MSG_ACCEPTED
			, org.acplt.oncrpc.OncRpcAcceptStatus.ONCRPC_PROG_MISMATCH, org.acplt.oncrpc.OncRpcReplyMessage
			.UNUSED_PARAMETER, low, high, org.acplt.oncrpc.OncRpcAuthStatus.ONCRPC_AUTH_OK)
		{
		}
	}
}
