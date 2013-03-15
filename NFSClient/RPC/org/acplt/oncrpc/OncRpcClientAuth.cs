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
	/// The <code>OncRpcClientAuth</code> class is the base class for handling
	/// all protocol issues of ONC/RPC authentication on the client side.
	/// </summary>
	/// <remarks>
	/// The <code>OncRpcClientAuth</code> class is the base class for handling
	/// all protocol issues of ONC/RPC authentication on the client side. As it
	/// stands, it does not do very much with the exception of defining the contract
	/// for the behaviour of derived classes with respect to protocol handling
	/// issues.
	/// <p>Authentication on the client side can be done as follows: just
	/// create an authentication object and hand it over to the ONC/RPC client
	/// object.
	/// <pre>
	/// OncRpcClientAuth auth = new OncRpcClientAuthUnix(
	/// "marvin@ford.prefect",
	/// 42, 1001, new int[0]);
	/// client.setAuth(auth);
	/// </pre>
	/// The
	/// <see cref="OncRpcClientAuthUnix">authentication <code>AUTH_UNIX</code></see>
	/// will
	/// handle shorthand credentials (of type <code>AUTH_SHORT</code> transparently).
	/// If you do not set any authentication object after creating an ONC/RPC client
	/// object, <code>AUTH_NONE</code> is used automatically.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:40 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public abstract class OncRpcClientAuth
	{
		/// <summary>
		/// Encodes ONC/RPC authentication information in form of a credential
		/// and a verifier when sending an ONC/RPC call message.
		/// </summary>
		/// <remarks>
		/// Encodes ONC/RPC authentication information in form of a credential
		/// and a verifier when sending an ONC/RPC call message.
		/// </remarks>
		/// <param name="xdr">
		/// XDR stream where to encode the credential and the verifier
		/// to.
		/// </param>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		internal abstract void xdrEncodeCredVerf(org.acplt.oncrpc.XdrEncodingStream xdr);

		/// <summary>
		/// Decodes ONC/RPC authentication information in form of a verifier
		/// when receiving an ONC/RPC reply message.
		/// </summary>
		/// <remarks>
		/// Decodes ONC/RPC authentication information in form of a verifier
		/// when receiving an ONC/RPC reply message.
		/// </remarks>
		/// <param name="xdr">
		/// XDR stream from which to receive the verifier sent together
		/// with an ONC/RPC reply message.
		/// </param>
		/// <exception cref="OncRpcAuthenticationException">
		/// if the received verifier is
		/// not kosher.
		/// </exception>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		internal abstract void xdrDecodeVerf(org.acplt.oncrpc.XdrDecodingStream xdr);

		/// <summary>
		/// Indicates whether the ONC/RPC authentication credential can be
		/// refreshed.
		/// </summary>
		/// <remarks>
		/// Indicates whether the ONC/RPC authentication credential can be
		/// refreshed.
		/// </remarks>
		/// <returns>true, if the credential can be refreshed</returns>
		public abstract bool canRefreshCred();
	}
}
