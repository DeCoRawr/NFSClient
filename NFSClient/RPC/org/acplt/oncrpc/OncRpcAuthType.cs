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
	/// A collection of constants used to identify the authentication schemes
	/// available for ONC/RPC.
	/// </summary>
	/// <remarks>
	/// A collection of constants used to identify the authentication schemes
	/// available for ONC/RPC. Please note that currently only
	/// <code>ONCRPC_AUTH_NONE</code> is supported by this Java package.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:40 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class OncRpcAuthType
	{
		/// <summary>No authentication scheme used for this remote procedure call.</summary>
		/// <remarks>No authentication scheme used for this remote procedure call.</remarks>
		public const int ONCRPC_AUTH_NONE = 0;

		/// <summary>The so-called "Unix" authentication scheme is not supported.</summary>
		/// <remarks>
		/// The so-called "Unix" authentication scheme is not supported. This one
		/// only sends the users id as well as her/his group identifiers, so this
		/// is simply far too weak to use in typical situations where
		/// authentication is requested.
		/// </remarks>
		public const int ONCRPC_AUTH_UNIX = 1;

		/// <summary>The so-called "short hand Unix style" is not supported.</summary>
		/// <remarks>The so-called "short hand Unix style" is not supported.</remarks>
		public const int ONCRPC_AUTH_SHORT = 2;

		/// <summary>
		/// The DES authentication scheme (using encrypted time stamps) is not
		/// supported -- and besides, it's not a silver bullet either.
		/// </summary>
		/// <remarks>
		/// The DES authentication scheme (using encrypted time stamps) is not
		/// supported -- and besides, it's not a silver bullet either.
		/// </remarks>
		public const int ONCRPC_AUTH_DES = 3;
	}
}
