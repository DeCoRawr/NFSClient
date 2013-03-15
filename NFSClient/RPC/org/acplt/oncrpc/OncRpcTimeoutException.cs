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
	/// The class <code>OncRpcTimeoutException</code> indicates a timed out
	/// call exception.
	/// </summary>
	/// <remarks>
	/// The class <code>OncRpcTimeoutException</code> indicates a timed out
	/// call exception.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.2 $ $Date: 2005/11/11 21:05:00 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	[System.Serializable]
	public class OncRpcTimeoutException : OncRpcException
	{
		/// <summary>Defines the serial version UID for <code>OncRpcTimeoutException</code>.</summary>
		/// <remarks>Defines the serial version UID for <code>OncRpcTimeoutException</code>.</remarks>
		private const long serialVersionUID = 2777518173161399732L;

		/// <summary>
		/// Initializes an <code>OncRpcTimeoutException</code>
		/// with a detail of
		/// <see cref="OncRpcException.RPC_TIMEDOUT">OncRpcException.RPC_TIMEDOUT</see>
		/// .
		/// </summary>
		public OncRpcTimeoutException() : base(RPC_TIMEDOUT)
		{
		}
	}
}
