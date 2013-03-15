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
	/// A collection of constants used to identify the acceptance status of
	/// ONC/RPC reply messages.
	/// </summary>
	/// <remarks>
	/// A collection of constants used to identify the acceptance status of
	/// ONC/RPC reply messages.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:39 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class OncRpcAcceptStatus
	{
		/// <summary>The remote procedure was called and executed successfully.</summary>
		/// <remarks>The remote procedure was called and executed successfully.</remarks>
		public const int ONCRPC_SUCCESS = 0;

		/// <summary>The program requested is not available.</summary>
		/// <remarks>
		/// The program requested is not available. So the remote host
		/// does not export this particular program and the ONC/RPC server
		/// which you tried to send a RPC call message doesn't know of this
		/// program either.
		/// </remarks>
		public const int ONCRPC_PROG_UNAVAIL = 1;

		/// <summary>A program version number mismatch occured.</summary>
		/// <remarks>
		/// A program version number mismatch occured. The remote ONC/RPC
		/// server does not support this particular version of the program.
		/// </remarks>
		public const int ONCRPC_PROG_MISMATCH = 2;

		/// <summary>The procedure requested is not available.</summary>
		/// <remarks>
		/// The procedure requested is not available. The remote ONC/RPC server
		/// does not support this particular procedure.
		/// </remarks>
		public const int ONCRPC_PROC_UNAVAIL = 3;

		/// <summary>
		/// The server could not decode the arguments sent within the ONC/RPC
		/// call message.
		/// </summary>
		/// <remarks>
		/// The server could not decode the arguments sent within the ONC/RPC
		/// call message.
		/// </remarks>
		public const int ONCRPC_GARBAGE_ARGS = 4;

		/// <summary>
		/// The server encountered a system error and thus was not able to
		/// process the procedure call.
		/// </summary>
		/// <remarks>
		/// The server encountered a system error and thus was not able to
		/// process the procedure call. Causes might be memory shortage,
		/// desinterest and sloth.
		/// </remarks>
		public const int ONCRPC_SYSTEM_ERR = 5;
	}
}
