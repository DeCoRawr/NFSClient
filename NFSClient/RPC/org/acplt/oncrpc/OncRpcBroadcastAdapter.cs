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
	/// An abstract adapter class for
	/// <see cref="OncRpcBroadcastListener">receiving</see>
	/// <see cref="OncRpcBroadcastEvent">ONC/RPC broadcast reply events</see>
	/// .
	/// The methods in this class are empty. This class exists as
	/// convenience for creating listener objects.
        /// Converted to C# using the db4o Sharpen tool.
	/// </summary>
	/// <seealso cref="OncRpcUdpClient">OncRpcUdpClient</seealso>
	/// <seealso cref="OncRpcBroadcastAdapter">OncRpcBroadcastAdapter</seealso>
	/// <seealso cref="OncRpcBroadcastListener">OncRpcBroadcastListener</seealso>
	/// <seealso cref="OncRpcBroadcastEvent">OncRpcBroadcastEvent</seealso>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:40 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public abstract class OncRpcBroadcastAdapter : org.acplt.oncrpc.OncRpcBroadcastListener
	{
		/// <summary>Invoked when a reply to an ONC/RPC broadcast call is received.</summary>
		/// <remarks>Invoked when a reply to an ONC/RPC broadcast call is received.</remarks>
		/// <seealso cref="OncRpcBroadcastEvent">OncRpcBroadcastEvent</seealso>
		public virtual void replyReceived(org.acplt.oncrpc.OncRpcBroadcastEvent evt)
		{
		}
	}
}
