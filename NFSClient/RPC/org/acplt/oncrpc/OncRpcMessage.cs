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
	/// The <code>OncRpcMessage</code> class is an abstract superclass for all
	/// the message types ONC/RPC defines (well, an overwhelming count of two).
	/// </summary>
	/// <remarks>
	/// The <code>OncRpcMessage</code> class is an abstract superclass for all
	/// the message types ONC/RPC defines (well, an overwhelming count of two).
	/// The only things common to all ONC/RPC messages are a message identifier
	/// and the message type. All other things do not come in until derived
	/// classes are introduced.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.2 $ $Date: 2003/08/14 07:56:37 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public abstract class OncRpcMessage
	{
		/// <summary>
		/// Constructs  a new <code>OncRpcMessage</code> object with default
		/// values: a given message type and no particular message identifier.
		/// </summary>
		/// <remarks>
		/// Constructs  a new <code>OncRpcMessage</code> object with default
		/// values: a given message type and no particular message identifier.
		/// </remarks>
		public OncRpcMessage(int messageId)
		{
			this.messageId = messageId;
			messageType = -1;
		}

		/// <summary>
		/// The message id is used to identify matching ONC/RPC calls and
		/// replies.
		/// </summary>
		/// <remarks>
		/// The message id is used to identify matching ONC/RPC calls and
		/// replies. This is typically choosen by the communication partner
		/// sending a request. The matching reply then must have the same
		/// message identifier, so the receiver can match calls and replies.
		/// </remarks>
		public int messageId;

		/// <summary>
		/// The kind of ONC/RPC message, which can be either a call or a
		/// reply.
		/// </summary>
		/// <remarks>
		/// The kind of ONC/RPC message, which can be either a call or a
		/// reply. Can be one of the constants defined in
		/// <see cref="OncRpcMessageType">OncRpcMessageType</see>
		/// .
		/// </remarks>
		/// <seealso cref="OncRpcMessageType">OncRpcMessageType</seealso>
		public int messageType;
	}
}
