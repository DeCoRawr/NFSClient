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
	/// A collection of constants related to authentication and generally usefull
	/// for ONC/RPC.
	/// </summary>
	/// <remarks>
	/// A collection of constants related to authentication and generally usefull
	/// for ONC/RPC.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:40 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class OncRpcAuthConstants
	{
		/// <summary>Maximum length of opaque authentication information.</summary>
		/// <remarks>Maximum length of opaque authentication information.</remarks>
		public const int ONCRPC_MAX_AUTH_BYTES = 400;

		/// <summary>Maximum length of machine name.</summary>
		/// <remarks>Maximum length of machine name.</remarks>
		public const int ONCRPC_MAX_MACHINE_NAME = 255;

		/// <summary>Maximum allowed number of groups.</summary>
		/// <remarks>Maximum allowed number of groups.</remarks>
		public const int ONCRPC_MAX_GROUPS = 16;
	}
}
