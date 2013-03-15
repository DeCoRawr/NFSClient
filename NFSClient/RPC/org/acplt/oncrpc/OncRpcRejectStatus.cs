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
	/// A collection of constants used to describe why a remote procedure call
	/// message was rejected.
	/// </summary>
	/// <remarks>
	/// A collection of constants used to describe why a remote procedure call
	/// message was rejected. This constants are used in
	/// <see cref="OncRpcReplyMessage">OncRpcReplyMessage</see>
	/// objects, which represent rejected messages if their
	/// <see cref="OncRpcReplyMessage.replyStatus">OncRpcReplyMessage.replyStatus</see>
	/// field has the value
	/// <see cref="OncRpcReplyStatus.ONCRPC_MSG_DENIED">OncRpcReplyStatus.ONCRPC_MSG_DENIED
	/// 	</see>
	/// .
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:41 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class OncRpcRejectStatus
	{
		/// <summary>Wrong ONC/RPC protocol version used in call (it needs to be version 2).</summary>
		/// <remarks>Wrong ONC/RPC protocol version used in call (it needs to be version 2).</remarks>
		public const int ONCRPC_RPC_MISMATCH = 0;

		/// <summary>The remote ONC/RPC server could not authenticate the caller.</summary>
		/// <remarks>The remote ONC/RPC server could not authenticate the caller.</remarks>
		public const int ONCRPC_AUTH_ERROR = 1;
	}
}
