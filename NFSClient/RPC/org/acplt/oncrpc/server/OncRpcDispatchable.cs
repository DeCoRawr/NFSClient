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

namespace org.acplt.oncrpc.server
{
	/// <summary>
	/// Tags classes as being able to dispatch and handle ONC/RPC requests from
	/// clients.
	/// </summary>
	/// <remarks>
	/// Tags classes as being able to dispatch and handle ONC/RPC requests from
	/// clients.
	/// <p>This interface is used as follows for dispatching and handling ONC/RPC
	/// calls:
	/// <ul>
	/// <li>First check which procedure the client intends to call. This information
	/// is delivered through the <code>procedure</code> parameter. In case you
	/// do not handle multiple programs within the same dispatcher, you can ignore
	/// the <code>program</code> parameter as well as <code>version</code>.
	/// <li>Retrieve appropriate parameters for this intended procedure using the
	/// <see cref="OncRpcCallInformation.retrieveCall(org.acplt.oncrpc.XdrAble)">OncRpcCallInformation.retrieveCall(org.acplt.oncrpc.XdrAble)
	/// 	</see>
	/// method of the
	/// <see cref="OncRpcCallInformation">call</see>
	/// object also supplied to the dispatcher
	/// through the <code>call</code> parameter.
	/// <li>Do whatever you need to do for this ONC/RPC call and make up an
	/// appropriate reply to be sent back to the client in the next step.
	/// <li>Send back the reply by calling the
	/// <see cref="OncRpcCallInformation.reply(org.acplt.oncrpc.XdrAble)">OncRpcCallInformation.reply(org.acplt.oncrpc.XdrAble)
	/// 	</see>
	/// method of the
	/// <see cref="OncRpcCallInformation">call</see>
	/// object
	/// </ul>
	/// <p>Here's a simple example only showing how to handle the famous
	/// procedure <code>0</code>: this is the "ping" procedure which can be used
	/// to test whether the server is still living. The example also shows how to
	/// handle calls for procedures which are not implemented (not defined) by
	/// calling
	/// <see cref="OncRpcCallInformation.failProcedureUnavailable()">OncRpcCallInformation.failProcedureUnavailable()
	/// 	</see>
	/// .
	/// <p>In case the dispatcher throws an exception, the affected ONC/RPC server
	/// transport will send a system error indication
	/// <see cref="OncRpcCallInformation.failSystemError()">OncRpcCallInformation.failSystemError()
	/// 	</see>
	/// to
	/// the client. No error indication will be sent if the exception resulted from
	/// an I/O problem. Note that if you do not explicitely send back a reply, no
	/// reply is sent at all, making batched calls possible.
	/// <pre>
	/// public void dispatchOncRpcCall(OncRpcCallInformation call,
	/// int program, int version, int procedure)
	/// throws OncRpcException, IOException {
	/// switch ( procedure ) {
	/// case 0:
	/// XdrVoid v = new XdrVoid();
	/// call.retrieveCall(v);
	/// call.reply(v);
	/// break;
	/// default:
	/// call.failProcedureUnavailable();
	/// }
	/// }
	/// </pre>
	/// In addition, there are also lower-level methods available for retrieving
	/// parameters and sending replies, in case you need to break up deserialization
	/// and serialization into several steps. The following code snipped shows
	/// how to use them. Here, the objects <code>foo</code> and <code>bar</code>
	/// represents call parameter objects, while <code>baz</code> and <code>blah</code>
	/// are used to sent back the reply data.
	/// <pre>
	/// public void dispatchOncRpcCall(OncRpcCallInformation call,
	/// int program, int version, int procedure)
	/// throws OncRpcException, IOException {
	/// switch ( procedure ) {
	/// case 42:
	/// // Retrieve call parameters.
	/// XdrDecodingStream decoder = call.getXdrDecodingStream();
	/// foo.xdrDecode(decoder);
	/// bar.xdrDecode(decoder);
	/// call.endDecoding();
	/// // Handle particular ONC/RPC call...
	/// // Send back reply.
	/// call.beginEncoding();
	/// XdrEncodingStream encoder = call.getXdrEncodingStream();
	/// baz.xdrEncode(encoder);
	/// blah.xdrEncode(encoder);
	/// call.endEncoding();
	/// break;
	/// }
	/// }
	/// </pre>
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:51 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public interface OncRpcDispatchable
	{
		/// <summary>Dispatch (handle) an ONC/RPC request from a client.</summary>
		/// <remarks>
		/// Dispatch (handle) an ONC/RPC request from a client. This interface has
		/// some fairly deep semantics, so please read the description above for
		/// how to use it properly. For background information about fairly deep
		/// semantics, please also refer to <i>Gigzales</i>, <i>J</i>.: Semantics
		/// considered harmful. Addison-Reilly, 1992, ISBN 0-542-10815-X.
		/// <p>See the introduction to this class for examples of how to use
		/// this interface properly.
		/// </remarks>
		/// <param name="call">
		/// Information about the call to handle, like the caller's
		/// Internet address, the ONC/RPC call header, etc.
		/// </param>
		/// <param name="program">Program number requested by client.</param>
		/// <param name="version">Version number requested.</param>
		/// <param name="procedure">Procedure number requested.</param>
		/// <seealso cref="OncRpcCallInformation">OncRpcCallInformation</seealso>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		void dispatchOncRpcCall(org.acplt.oncrpc.server.OncRpcCallInformation call, int program
			, int version, int procedure);
	}
}
