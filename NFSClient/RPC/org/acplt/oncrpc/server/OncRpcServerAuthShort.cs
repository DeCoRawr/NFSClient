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
	/// The <code>OncRpcServerAuthShort</code> class handles all protocol issues
	/// of the ONC/RPC authentication <code>AUTH_SHORT</code> on the server
	/// side.
	/// </summary>
	/// <remarks>
	/// The <code>OncRpcServerAuthShort</code> class handles all protocol issues
	/// of the ONC/RPC authentication <code>AUTH_SHORT</code> on the server
	/// side.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:51 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public sealed class OncRpcServerAuthShort : OncRpcServerAuth
	{
		/// <summary>
		/// Constructs an <code>OncRpcServerAuthShort</code> object and pulls its
		/// state off an XDR stream.
		/// </summary>
		/// <remarks>
		/// Constructs an <code>OncRpcServerAuthShort</code> object and pulls its
		/// state off an XDR stream.
		/// </remarks>
		/// <param name="xdr">XDR stream to retrieve the object state from.</param>
		/// <exception cref="org.acplt.oncrpc.OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		public OncRpcServerAuthShort(org.acplt.oncrpc.XdrDecodingStream xdr)
		{
			xdrDecodeCredVerf(xdr);
		}

		/// <summary>
		/// Returns the type (flavor) of
		/// <see cref="org.acplt.oncrpc.OncRpcAuthType">authentication</see>
		/// used.
		/// </summary>
		/// <returns>Authentication type used by this authentication object.</returns>
		public sealed override int getAuthenticationType()
		{
			return org.acplt.oncrpc.OncRpcAuthType.ONCRPC_AUTH_SHORT;
		}

		/// <summary>Returns the shorthand credential sent by the caller.</summary>
		/// <remarks>Returns the shorthand credential sent by the caller.</remarks>
		public byte[] getShorthandCred()
		{
			return shorthandCred;
		}

		/// <summary>Sets shorthand verifier to be sent back to the caller.</summary>
		/// <remarks>
		/// Sets shorthand verifier to be sent back to the caller. The caller then
		/// can use this shorthand verifier as the new credential with the next
		/// ONC/RPC calls. If you don't set the verifier or set it to
		/// <code>null</code>, then the verifier returned to the caller will be
		/// of type <code>AUTH_NONE</code>.
		/// </remarks>
		public void setShorthandVerifier(byte[] shorthandVerf)
		{
			this.shorthandVerf = shorthandVerf;
		}

		/// <summary>Returns the shorthand verifier to be sent back to the caller.</summary>
		/// <remarks>Returns the shorthand verifier to be sent back to the caller.</remarks>
		public byte[] getShorthandVerifier()
		{
			return shorthandVerf;
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
			// Reset the authentication object's state properly...
			//
			shorthandCred = null;
			shorthandVerf = null;
			//
			// Pull off the shorthand credential information (opaque date) of
			// the XDR stream...
			//
			shorthandCred = xdr.xdrDecodeDynamicOpaque();
			if (shorthandCred.Length > org.acplt.oncrpc.OncRpcAuthConstants.ONCRPC_MAX_AUTH_BYTES)
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
			if (shorthandVerf != null)
			{
				//
				// Encode AUTH_SHORT shorthand verifier (credential).
				//
				xdr.xdrEncodeInt(org.acplt.oncrpc.OncRpcAuthType.ONCRPC_AUTH_SHORT);
				xdr.xdrEncodeDynamicOpaque(shorthandVerf);
			}
			else
			{
				//
				// Encode an AUTH_NONE verifier with zero length, if no shorthand
				// verifier (credential) has been supplied by now.
				//
				xdr.xdrEncodeInt(org.acplt.oncrpc.OncRpcAuthType.ONCRPC_AUTH_NONE);
				xdr.xdrEncodeInt(0);
			}
		}

		/// <summary>Contains the shorthand credential sent by the caller.</summary>
		/// <remarks>Contains the shorthand credential sent by the caller.</remarks>
		private byte[] shorthandCred;

		/// <summary>
		/// Contains the shorthand authentication verifier (credential) to return
		/// to the caller to be used with the next ONC/RPC calls.
		/// </summary>
		/// <remarks>
		/// Contains the shorthand authentication verifier (credential) to return
		/// to the caller to be used with the next ONC/RPC calls.
		/// </remarks>
		private byte[] shorthandVerf;
	}
}
