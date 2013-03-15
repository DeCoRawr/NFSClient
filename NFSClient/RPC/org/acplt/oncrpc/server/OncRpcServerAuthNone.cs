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
	/// The <code>OncRpcServerAuthNone</code> class handles all protocol issues
	/// of the ONC/RPC authentication <code>AUTH_NONE</code> on the server
	/// side.
	/// </summary>
	/// <remarks>
	/// The <code>OncRpcServerAuthNone</code> class handles all protocol issues
	/// of the ONC/RPC authentication <code>AUTH_NONE</code> on the server
	/// side.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:51 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public sealed class OncRpcServerAuthNone : OncRpcServerAuth
	{
		/// <summary>
		/// Returns the type (flavor) of
		/// <see cref="org.acplt.oncrpc.OncRpcAuthType">authentication</see>
		/// used.
		/// </summary>
		/// <returns>Authentication type used by this authentication object.</returns>
		public sealed override int getAuthenticationType()
		{
			return org.acplt.oncrpc.OncRpcAuthType.ONCRPC_AUTH_NONE;
		}

		/// <summary>
		/// Decodes -- that is: deserializes -- an ONC/RPC authentication object
		/// (credential & verifier) on the server side.
		/// </summary>
		/// <remarks>
		/// Decodes -- that is: deserializes -- an ONC/RPC authentication object
		/// (credential & verifier) on the server side.
		/// </remarks>
		/// <exception cref="org.acplt.oncrpc.OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		public sealed override void xdrDecodeCredVerf(org.acplt.oncrpc.XdrDecodingStream 
			xdr)
		{
			//
			// As the authentication type has already been pulled off the XDR
			// stream, we only need to make sure that really no opaque data follows.
			//
			if (xdr.xdrDecodeInt() != 0)
			{
				throw (new org.acplt.oncrpc.OncRpcAuthenticationException(org.acplt.oncrpc.OncRpcAuthStatus
					.ONCRPC_AUTH_BADCRED));
			}
			//
			// We also need to decode the verifier. This must be of type
			// AUTH_NONE too. For some obscure historical reasons, we have to
			// deal with credentials and verifiers, although they belong together,
			// according to Sun's specification.
			//
			if ((xdr.xdrDecodeInt() != org.acplt.oncrpc.OncRpcAuthType.ONCRPC_AUTH_NONE) || (
				xdr.xdrDecodeInt() != 0))
			{
				throw (new org.acplt.oncrpc.OncRpcAuthenticationException(org.acplt.oncrpc.OncRpcAuthStatus
					.ONCRPC_AUTH_BADVERF));
			}
		}

		/// <summary>
		/// Encodes -- that is: serializes -- an ONC/RPC authentication object
		/// (its verifier) on the server side.
		/// </summary>
		/// <remarks>
		/// Encodes -- that is: serializes -- an ONC/RPC authentication object
		/// (its verifier) on the server side.
		/// </remarks>
		/// <exception cref="org.acplt.oncrpc.OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		public sealed override void xdrEncodeVerf(org.acplt.oncrpc.XdrEncodingStream xdr)
		{
			//
			// Encode an AUTH_NONE verifier with zero length.
			//
			xdr.xdrEncodeInt(org.acplt.oncrpc.OncRpcAuthType.ONCRPC_AUTH_NONE);
			xdr.xdrEncodeInt(0);
		}

		/// <summary>
		/// Singleton to use when an authentication object for <code>AUTH_NONE</code>
		/// is needed.
		/// </summary>
		/// <remarks>
		/// Singleton to use when an authentication object for <code>AUTH_NONE</code>
		/// is needed.
		/// </remarks>
		public static readonly org.acplt.oncrpc.server.OncRpcServerAuthNone AUTH_NONE = new 
			org.acplt.oncrpc.server.OncRpcServerAuthNone();
	}
}
