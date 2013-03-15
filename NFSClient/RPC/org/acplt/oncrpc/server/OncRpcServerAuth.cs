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
	/// The <code>OncRpcServerAuth</code> class is the base class and factory
	/// for handling all protocol issues of ONC/RPC authentication on the server
	/// side.
	/// </summary>
	/// <remarks>
	/// The <code>OncRpcServerAuth</code> class is the base class and factory
	/// for handling all protocol issues of ONC/RPC authentication on the server
	/// side.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:51 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public abstract class OncRpcServerAuth
	{
		/// <summary>
		/// Returns the type (flavor) of
		/// <see cref="org.acplt.oncrpc.OncRpcAuthType">authentication</see>
		/// used.
		/// </summary>
		/// <returns>Authentication type used by this authentication object.</returns>
		public abstract int getAuthenticationType();

		/// <summary>Restores (deserializes) an authentication object from an XDR stream.</summary>
		/// <remarks>Restores (deserializes) an authentication object from an XDR stream.</remarks>
		/// <param name="xdr">
		/// XDR stream from which the authentication object is
		/// restored.
		/// </param>
		/// <param name="recycle">
		/// old authtentication object which is intended to be
		/// reused in case it is of the same authentication type as the new
		/// one just arriving from the XDR stream.
		/// </param>
		/// <returns>
		/// Authentication information encapsulated in an object, whose class
		/// is derived from <code>OncRpcServerAuth</code>.
		/// </returns>
		/// <exception cref="org.acplt.oncrpc.OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		public static org.acplt.oncrpc.server.OncRpcServerAuth xdrNew(org.acplt.oncrpc.XdrDecodingStream
			 xdr, org.acplt.oncrpc.server.OncRpcServerAuth recycle)
		{
			org.acplt.oncrpc.server.OncRpcServerAuth auth;
			//
			// In case we got an old authentication object and we are just about
			// to receive an authentication with the same type, we reuse the old
			// object.
			//
			int authType = xdr.xdrDecodeInt();
			if ((recycle != null) && (recycle.getAuthenticationType() == authType))
			{
				//
				// Simply recycle authentication object and pull its new state
				// of the XDR stream.
				//
				auth = recycle;
				auth.xdrDecodeCredVerf(xdr);
			}
			else
			{
				switch (authType)
				{
					case org.acplt.oncrpc.OncRpcAuthType.ONCRPC_AUTH_NONE:
					{
						//
						// Create a new authentication object and pull its state off
						// the XDR stream.
						//
						auth = org.acplt.oncrpc.server.OncRpcServerAuthNone.AUTH_NONE;
						auth.xdrDecodeCredVerf(xdr);
						break;
					}

					case org.acplt.oncrpc.OncRpcAuthType.ONCRPC_AUTH_SHORT:
					{
						auth = new org.acplt.oncrpc.server.OncRpcServerAuthShort(xdr);
						break;
					}

					case org.acplt.oncrpc.OncRpcAuthType.ONCRPC_AUTH_UNIX:
					{
						auth = new org.acplt.oncrpc.server.OncRpcServerAuthUnix(xdr);
						break;
					}

					default:
					{
						//
						// In case of an unknown or unsupported type, throw an exception.
						// Note: using AUTH_REJECTEDCRED is in sync with the way Sun's
						// ONC/RPC implementation does it. But don't ask me why they do
						// it this way...!
						//
						throw (new org.acplt.oncrpc.OncRpcAuthenticationException(org.acplt.oncrpc.OncRpcAuthStatus
							.ONCRPC_AUTH_REJECTEDCRED));
					}
				}
			}
			return auth;
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
		public abstract void xdrDecodeCredVerf(org.acplt.oncrpc.XdrDecodingStream xdr);

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
		public abstract void xdrEncodeVerf(org.acplt.oncrpc.XdrEncodingStream xdr);
	}
}
