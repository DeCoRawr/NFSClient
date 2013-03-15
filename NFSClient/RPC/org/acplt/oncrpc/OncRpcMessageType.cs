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
	/// A collection of constants used for ONC/RPC messages to identify the
	/// type of message.
	/// </summary>
	/// <remarks>
	/// A collection of constants used for ONC/RPC messages to identify the
	/// type of message. Currently, ONC/RPC messages can be either calls or
	/// replies. Calls are sent by ONC/RPC clients to servers to call a remote
	/// procedure (for you "ohohpies" that can be translated into the buzzword
	/// "method"). A server then will answer with a corresponding reply message
	/// (but not in the case of batched calls).
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:41 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class OncRpcMessageType
	{
		/// <summary>Identifies an ONC/RPC call.</summary>
		/// <remarks>
		/// Identifies an ONC/RPC call. By a "call" a client request that a server
		/// carries out a particular remote procedure.
		/// </remarks>
		public const int ONCRPC_CALL = 0;

		/// <summary>Identifies an ONC/RPC reply.</summary>
		/// <remarks>
		/// Identifies an ONC/RPC reply. A server responds with a "reply" after
		/// a client has sent a "call" for a particular remote procedure, sending
		/// back the results of calling that procedure.
		/// </remarks>
		public const int ONCRPC_REPLY = 1;
	}
}
