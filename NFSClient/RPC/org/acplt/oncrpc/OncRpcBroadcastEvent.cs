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

using System.Net;
namespace org.acplt.oncrpc
{
	/// <summary>
	/// The class <code>OncRpcBroadcastEvent</code> defines an event fired by
	/// <see cref="OncRpcUdpClient">ONC/RPC UDP/IP-based clients</see>
	/// whenever replies
	/// to a
	/// <see cref="OncRpcUdpClient.broadcastCall(int, XdrAble, XdrAble, OncRpcBroadcastListener)
	/// 	">broadcast call</see>
	/// are received.
        /// Converted to C# using the db4o Sharpen tool.
	/// </summary>
	/// <seealso cref="OncRpcBroadcastListener">OncRpcBroadcastListener</seealso>
	/// <seealso cref="OncRpcBroadcastAdapter">OncRpcBroadcastAdapter</seealso>
	/// <seealso cref="OncRpcUdpClient">OncRpcUdpClient</seealso>
	/// <version>$Revision: 1.3 $ $Date: 2005/11/11 21:19:20 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	[System.Serializable]
	public class OncRpcBroadcastEvent 
	{
		/// <summary>Defines the serial version UID for <code>OncRpcBroadcastEvent</code>.</summary>
		/// <remarks>Defines the serial version UID for <code>OncRpcBroadcastEvent</code>.</remarks>
		private const long serialVersionUID = 1604512454490873965L;

		/// <summary>
		/// Creates a new <code>KscPackageUpdateEvent</code> object and
		/// initializes its state.
		/// </summary>
		/// <remarks>
		/// Creates a new <code>KscPackageUpdateEvent</code> object and
		/// initializes its state.
		/// </remarks>
		/// <param name="source">
		/// The
		/// <see cref="OncRpcUdpClient">ONC/RPC client object</see>
		/// which has
		/// fired this event.
		/// </param>
		/// <param name="replyAddress">Internetaddress of reply's origin.</param>
		/// <param name="procedureNumber">Procedure number of ONC/RPC call.</param>
		/// <param name="params">The ONC/RPC call resulting in this reply.</param>
		/// <param name="reply">The ONC/RPC reply itself.</param>
		public OncRpcBroadcastEvent(org.acplt.oncrpc.OncRpcUdpClient source, IPAddress
			 replyAddress, int procedureNumber, org.acplt.oncrpc.XdrAble @params, org.acplt.oncrpc.XdrAble
			 reply) 
		{
			this.replyAddress = replyAddress;
			this.procedureNumber = procedureNumber;
			this.@params = @params;
			this.reply = reply;
		}

		/// <summary>Returns the address of the sender of the ONC/RPC reply message.</summary>
		/// <remarks>Returns the address of the sender of the ONC/RPC reply message.</remarks>
		/// <returns>address of sender of reply.</returns>
		public virtual IPAddress getReplyAddress()
		{
			return replyAddress;
		}

		/// <summary>Returns ONC/RPC reply message.</summary>
		/// <remarks>Returns ONC/RPC reply message.</remarks>
		/// <returns>reply message object.</returns>
		public virtual org.acplt.oncrpc.XdrAble getReply()
		{
			return reply;
		}

		/// <summary>Returns the number of the remote procedure called.</summary>
		/// <remarks>Returns the number of the remote procedure called.</remarks>
		/// <returns>procedure number.</returns>
		public virtual int getProcedureNumber()
		{
			return procedureNumber;
		}

		/// <summary>Returns the parameter message sent in a broadcast RPC.</summary>
		/// <remarks>Returns the parameter message sent in a broadcast RPC.</remarks>
		/// <returns>parameter message object.</returns>
		public virtual org.acplt.oncrpc.XdrAble getParams()
		{
			return @params;
		}

		/// <summary>Contains the address of the sender of the ONC/RPC reply message.</summary>
		/// <remarks>Contains the address of the sender of the ONC/RPC reply message.</remarks>
		/// <serial></serial>
		private IPAddress replyAddress;

		/// <summary>Contains the number of the remote procedure called.</summary>
		/// <remarks>Contains the number of the remote procedure called.</remarks>
		/// <serial></serial>
		private int procedureNumber;

		/// <summary>Contains the parameters sent in the ONC/RPC broadcast call.</summary>
		/// <remarks>Contains the parameters sent in the ONC/RPC broadcast call.</remarks>
		/// <serial></serial>
		private org.acplt.oncrpc.XdrAble @params;

		/// <summary>
		/// Contains the reply from a remote ONC/RPC server, which answered
		/// the broadcast call.
		/// </summary>
		/// <remarks>
		/// Contains the reply from a remote ONC/RPC server, which answered
		/// the broadcast call.
		/// </remarks>
		/// <serial></serial>
		private org.acplt.oncrpc.XdrAble reply;
	}
}
