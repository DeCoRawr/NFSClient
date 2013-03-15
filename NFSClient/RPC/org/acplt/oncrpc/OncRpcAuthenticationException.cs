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
	/// The class <code>OncRpcAuthenticationException</code> indicates an
	/// authentication exception.
	/// </summary>
	/// <remarks>
	/// The class <code>OncRpcAuthenticationException</code> indicates an
	/// authentication exception.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.2 $ $Date: 2005/11/11 21:01:44 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	[System.Serializable]
	public class OncRpcAuthenticationException : org.acplt.oncrpc.OncRpcException
	{
		/// <summary>Defines the serial version UID for <code>OncRpcAuthenticationException</code>.
		/// 	</summary>
		/// <remarks>Defines the serial version UID for <code>OncRpcAuthenticationException</code>.
		/// 	</remarks>
		private const long serialVersionUID = 7747394107888423440L;

		/// <summary>
		/// Initializes an <code>OncRpcAuthenticationException</code>
		/// with a detail of
		/// <see cref="OncRpcException.RPC_AUTHERROR">OncRpcException.RPC_AUTHERROR</see>
		/// and
		/// the specified
		/// <see cref="OncRpcAuthStatus">authentication status</see>
		/// detail.
		/// </summary>
		/// <param name="authStatus">
		/// The authentication status, which can be any one of
		/// the
		/// <see cref="OncRpcAuthStatus">OncRpcAuthStatus constants</see>
		/// .
		/// </param>
		public OncRpcAuthenticationException(int authStatus) : base(RPC_AUTHERROR)
		{
			authStatusDetail = authStatus;
		}

		/// <summary>
		/// Returns the authentication status detail of this ONC/RPC exception
		/// object.
		/// </summary>
		/// <remarks>
		/// Returns the authentication status detail of this ONC/RPC exception
		/// object.
		/// </remarks>
		/// <returns>The authentication status of this <code>OncRpcException</code>.</returns>
		public virtual int getAuthStatus()
		{
			return authStatusDetail;
		}

		/// <summary>
		/// Specific authentication status detail (reason why this authentication
		/// exception was thrown).
		/// </summary>
		/// <remarks>
		/// Specific authentication status detail (reason why this authentication
		/// exception was thrown).
		/// </remarks>
		/// <serial></serial>
		private int authStatusDetail;
	}
}
