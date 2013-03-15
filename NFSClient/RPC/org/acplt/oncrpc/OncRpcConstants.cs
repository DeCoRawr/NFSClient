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
	/// <summary>A collection of constants generally usefull for ONC/RPC.</summary>
	/// <remarks>A collection of constants generally usefull for ONC/RPC.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.3 $ $Date: 2005/11/11 21:02:47 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class OncRpcConstants
	{
		/// <summary>The current version of the Remote Tea Java library as a string.</summary>
		/// <remarks>The current version of the Remote Tea Java library as a string.</remarks>
		public static readonly string REMOTETEA_VERSION_STRING = "1.0.4";

		/// <summary>The current major version number of the Remote Tea Java library.</summary>
		/// <remarks>The current major version number of the Remote Tea Java library.</remarks>
		public const int REMOTETEA_VERSION_MAJOR = 1;

		/// <summary>The current minor version number of the Remote Tea Java library.</summary>
		/// <remarks>The current minor version number of the Remote Tea Java library.</remarks>
		public const int REMOTETEA_VERSION_MINOR = 0;

		/// <summary>The current patch level of the Remote Tea Java library.</summary>
		/// <remarks>The current patch level of the Remote Tea Java library.</remarks>
		public const int REMOTETEA_VERSION_PATCHLEVEL = 4;

		/// <summary>The current preversion version number.</summary>
		/// <remarks>
		/// The current preversion version number. If not zero, then this
		/// indicates a preversion (no, not perversion... ooops, sorry).
		/// </remarks>
		public const int REMOTETEA_VERSION_PREVERSION = 0;
	}
}
