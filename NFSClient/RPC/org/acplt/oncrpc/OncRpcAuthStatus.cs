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
	/// A collection of constants used to identify the authentication status
	/// (or any authentication errors) in ONC/RPC replies of the corresponding
	/// ONC/RPC calls.
	/// </summary>
	/// <remarks>
	/// A collection of constants used to identify the authentication status
	/// (or any authentication errors) in ONC/RPC replies of the corresponding
	/// ONC/RPC calls.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:40 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class OncRpcAuthStatus
	{
		/// <summary>There is no authentication problem or error.</summary>
		/// <remarks>There is no authentication problem or error.</remarks>
		public const int ONCRPC_AUTH_OK = 0;

		/// <summary>
		/// The ONC/RPC server detected a bad credential (that is, the seal was
		/// broken).
		/// </summary>
		/// <remarks>
		/// The ONC/RPC server detected a bad credential (that is, the seal was
		/// broken).
		/// </remarks>
		public const int ONCRPC_AUTH_BADCRED = 1;

		/// <summary>
		/// The ONC/RPC server has rejected the credential and forces the caller
		/// to begin a new session.
		/// </summary>
		/// <remarks>
		/// The ONC/RPC server has rejected the credential and forces the caller
		/// to begin a new session.
		/// </remarks>
		public const int ONCRPC_AUTH_REJECTEDCRED = 2;

		/// <summary>
		/// The ONC/RPC server detected a bad verifier (that is, the seal was
		/// broken).
		/// </summary>
		/// <remarks>
		/// The ONC/RPC server detected a bad verifier (that is, the seal was
		/// broken).
		/// </remarks>
		public const int ONCRPC_AUTH_BADVERF = 3;

		/// <summary>
		/// The ONC/RPC server detected an expired verifier (which can also happen
		/// if the verifier was replayed).
		/// </summary>
		/// <remarks>
		/// The ONC/RPC server detected an expired verifier (which can also happen
		/// if the verifier was replayed).
		/// </remarks>
		public const int ONCRPC_AUTH_REJECTEDVERF = 4;

		/// <summary>The ONC/RPC server rejected the authentication for security reasons.</summary>
		/// <remarks>The ONC/RPC server rejected the authentication for security reasons.</remarks>
		public const int ONCRPC_AUTH_TOOWEAK = 5;

		/// <summary>The ONC/RPC client detected a bogus response verifier.</summary>
		/// <remarks>The ONC/RPC client detected a bogus response verifier.</remarks>
		public const int ONCRPC_AUTH_INVALIDRESP = 6;

		/// <summary>Authentication at the ONC/RPC client failed for an unknown reason.</summary>
		/// <remarks>Authentication at the ONC/RPC client failed for an unknown reason.</remarks>
		public const int ONCRPC_AUTH_FAILED = 7;
	}
}
