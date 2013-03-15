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

using System;
namespace org.acplt.oncrpc
{
	/// <summary>
	/// The <code>OncRpcClientAuthUnix</code> class handles protocol issues of
	/// ONC/RPC <code>AUTH_UNIX</code> (and thus <code>AUTH_SHORT</code>)
	/// authentication.
	/// </summary>
	/// <remarks>
	/// The <code>OncRpcClientAuthUnix</code> class handles protocol issues of
	/// ONC/RPC <code>AUTH_UNIX</code> (and thus <code>AUTH_SHORT</code>)
	/// authentication.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.1.1.1 $ $Date: 2003/08/13 12:03:40 $ $State: Exp $ $Locker:  $
	/// 	</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class OncRpcClientAuthUnix : org.acplt.oncrpc.OncRpcClientAuth
	{
		/// <summary>
		/// Constructs a new <code>OncRpcClientAuthUnix</code> authentication
		/// protocol handling object capable of handling <code>AUTH_UNIX</code>
		/// and <code>AUTH_SHORT</code>.
		/// </summary>
		/// <remarks>
		/// Constructs a new <code>OncRpcClientAuthUnix</code> authentication
		/// protocol handling object capable of handling <code>AUTH_UNIX</code>
		/// and <code>AUTH_SHORT</code>.
		/// <p>Please note that the credential information is typically only
		/// unique within a particular domain of machines, user IDs and
		/// group IDs.
		/// </remarks>
		/// <param name="machinename">
		/// Name of the caller's machine (like
		/// "ebankruptcy-dot-com", just for instance...).
		/// </param>
		/// <param name="uid">Caller's effective user ID.</param>
		/// <param name="gid">Caller's effective group ID.</param>
		/// <param name="gids">Array of group IDs the caller is a member of.</param>
		public OncRpcClientAuthUnix(string machinename, int uid, int gid, int[] gids)
		{
			this.stamp = (int) (DateTime.Now.Ticks/(TimeSpan.TicksPerMillisecond*1000));
			this.machinename = machinename;
			this.uid = uid;
			this.gid = gid;
			this.gids = gids;
		}

		/// <summary>
		/// Constructs a new <code>OncRpcClientAuthUnix</code> authentication
		/// protocol handling object capable of handling <code>AUTH_UNIX</code>
		/// and <code>AUTH_SHORT</code>.
		/// </summary>
		/// <remarks>
		/// Constructs a new <code>OncRpcClientAuthUnix</code> authentication
		/// protocol handling object capable of handling <code>AUTH_UNIX</code>
		/// and <code>AUTH_SHORT</code>.
		/// <p>Please note that the credential information is typically only
		/// unique within a particular domain of machines, user IDs and
		/// group IDs.
		/// </remarks>
		/// <param name="machinename">
		/// Name of the caller's machine (like
		/// "ebankruptcy-dot-com", just for instance...).
		/// </param>
		/// <param name="uid">Caller's effective user ID.</param>
		/// <param name="gid">Caller's effective group ID.</param>
		public OncRpcClientAuthUnix(string machinename, int uid, int gid) : this(machinename
			, uid, gid, NO_GIDS)
		{
		}

		/// <summary>
		/// Encodes ONC/RPC authentication information in form of a credential
		/// and a verifier when sending an ONC/RPC call message.
		/// </summary>
		/// <remarks>
		/// Encodes ONC/RPC authentication information in form of a credential
		/// and a verifier when sending an ONC/RPC call message. The
		/// <code>AUTH_UNIX</code> authentication method only uses the credential
		/// but no verifier. If the ONC/RPC server sent a <code>AUTH_SHORT</code>
		/// "shorthand" credential together with the previous reply message, it
		/// is used instead of the original credential.
		/// </remarks>
		/// <param name="xdr">
		/// XDR stream where to encode the credential and the verifier
		/// to.
		/// </param>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		internal override void xdrEncodeCredVerf(org.acplt.oncrpc.XdrEncodingStream xdr)
		{
			if (shorthandCred == null)
			{
				//
				// Encode the credential, which contains some unsecure information
				// about user and group ID, etc. Note that the credential itself
				// is encoded as a variable-sized bunch of octets.
				//
				if ((gids.Length > org.acplt.oncrpc.OncRpcAuthConstants.ONCRPC_MAX_GROUPS) || (machinename
					.Length > org.acplt.oncrpc.OncRpcAuthConstants.ONCRPC_MAX_MACHINE_NAME))
				{
					throw (new org.acplt.oncrpc.OncRpcAuthenticationException(org.acplt.oncrpc.OncRpcAuthStatus
						.ONCRPC_AUTH_FAILED));
				}
				xdr.xdrEncodeInt(org.acplt.oncrpc.OncRpcAuthType.ONCRPC_AUTH_UNIX);
				int len = 4 + ((machinename.Length + 7) & ~3) + 4 + 4 + gids.Length * 4 + 4;
				// length of stamp
				// len string incl. len
				// length of uid
				// length of gid
				// length of vector of gids incl. len
				if (len > org.acplt.oncrpc.OncRpcAuthConstants.ONCRPC_MAX_AUTH_BYTES)
				{
					throw (new org.acplt.oncrpc.OncRpcAuthenticationException(org.acplt.oncrpc.OncRpcAuthStatus
						.ONCRPC_AUTH_FAILED));
				}
				xdr.xdrEncodeInt(len);
				xdr.xdrEncodeInt(stamp);
				xdr.xdrEncodeString(machinename);
				xdr.xdrEncodeInt(uid);
				xdr.xdrEncodeInt(gid);
				xdr.xdrEncodeIntVector(gids);
			}
			else
			{
				//
				// Use shorthand credential instead of original credential.
				//
				xdr.xdrEncodeInt(org.acplt.oncrpc.OncRpcAuthType.ONCRPC_AUTH_SHORT);
				xdr.xdrEncodeDynamicOpaque(shorthandCred);
			}
			//
			// We also need to encode the verifier, which is always of
			// type AUTH_NONE.
			//
			xdr.xdrEncodeInt(org.acplt.oncrpc.OncRpcAuthType.ONCRPC_AUTH_NONE);
			xdr.xdrEncodeInt(0);
		}

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
		internal override void xdrDecodeVerf(org.acplt.oncrpc.XdrDecodingStream xdr)
		{
			switch (xdr.xdrDecodeInt())
			{
				case org.acplt.oncrpc.OncRpcAuthType.ONCRPC_AUTH_NONE:
				{
					//
					// The verifier sent in response to AUTH_UNIX or AUTH_SHORT credentials
					// can only be AUTH_NONE or AUTH_SHORT. In the latter case we drop
					// any old shorthand credential and use the new one.
					//
					//
					// Make sure that the verifier does not contain any opaque data.
					// Anything different from this is not kosher and an authentication
					// exception will be thrown.
					//
					if (xdr.xdrDecodeInt() != 0)
					{
						throw (new org.acplt.oncrpc.OncRpcAuthenticationException(org.acplt.oncrpc.OncRpcAuthStatus
							.ONCRPC_AUTH_FAILED));
					}
					break;
				}

				case org.acplt.oncrpc.OncRpcAuthType.ONCRPC_AUTH_SHORT:
				{
					//
					// Fetch the credential from the XDR stream and make sure that
					// it does conform to the length restriction as set forth in
					// the ONC/RPC protocol.
					//
					shorthandCred = xdr.xdrDecodeDynamicOpaque();
					if (shorthandCred.Length > org.acplt.oncrpc.OncRpcAuthConstants.ONCRPC_MAX_AUTH_BYTES)
					{
						throw (new org.acplt.oncrpc.OncRpcAuthenticationException(org.acplt.oncrpc.OncRpcAuthStatus
							.ONCRPC_AUTH_FAILED));
					}
					break;
				}

				default:
				{
					//
					// Do not accept any other kind of verifier sent.
					//
					throw (new org.acplt.oncrpc.OncRpcAuthenticationException(org.acplt.oncrpc.OncRpcAuthStatus
						.ONCRPC_AUTH_INVALIDRESP));
				}
			}
		}

		/// <summary>
		/// Indicates whether the ONC/RPC authentication credential can be
		/// refreshed.
		/// </summary>
		/// <remarks>
		/// Indicates whether the ONC/RPC authentication credential can be
		/// refreshed.
		/// </remarks>
		/// <returns>true, if the credential can be refreshed</returns>
		public override bool canRefreshCred()
		{
			//
			// If we don't use a shorthand credential at this time, then there's
			// no hope to refresh the credentials.
			//
			if (shorthandCred == null)
			{
				return false;
			}
			//
			// Otherwise just dump the shorthand credentials and let the caller
			// retry. This will probably result in the ONC/RPC server replying
			// with a new shorthand credential.
			//
			shorthandCred = null;
			//
			// Ah, yes. We need to update the "stamp" (more a timestamp, but
			// Sun coding style is sometimes interesting). As is my style too.
			//
			stamp = (int)(DateTime.Now.Ticks/(TimeSpan.TicksPerMillisecond*1000));
			//
			// Oh, yes. We can refresh the credential. Maybe.
			//
			return true;
		}

		/// <summary>Sets the timestamp information in the credential.</summary>
		/// <remarks>Sets the timestamp information in the credential.</remarks>
		/// <param name="stamp">New timestamp</param>
		public virtual void setStamp(int stamp)
		{
			this.stamp = stamp;
		}

		/// <summary>Returns the timestamp information from the credential.</summary>
		/// <remarks>Returns the timestamp information from the credential.</remarks>
		/// <returns>timestamp from credential.</returns>
		public virtual int getStamp()
		{
			return stamp;
		}

		/// <summary>Sets the machine name information in the credential.</summary>
		/// <remarks>Sets the machine name information in the credential.</remarks>
		/// <param name="machinename">Machine name.</param>
		public virtual void setMachinename(string machinename)
		{
			this.machinename = machinename;
		}

		/// <summary>Returns the machine name information from the credential.</summary>
		/// <remarks>Returns the machine name information from the credential.</remarks>
		/// <returns>machine name.</returns>
		public virtual string getMachinename()
		{
			return machinename;
		}

		/// <summary>Sets the user ID in the credential.</summary>
		/// <remarks>Sets the user ID in the credential.</remarks>
		/// <param name="uid">User ID.</param>
		public virtual void setUid(int uid)
		{
			this.uid = uid;
		}

		/// <summary>Returns the user ID from the credential.</summary>
		/// <remarks>Returns the user ID from the credential.</remarks>
		/// <returns>user ID.</returns>
		public virtual int getUid()
		{
			return uid;
		}

		/// <summary>Sets the group ID in the credential.</summary>
		/// <remarks>Sets the group ID in the credential.</remarks>
		/// <param name="gid">Group ID.</param>
		public virtual void setGid(int gid)
		{
			this.gid = gid;
		}

		/// <summary>Returns the group ID from the credential.</summary>
		/// <remarks>Returns the group ID from the credential.</remarks>
		/// <returns>group ID.</returns>
		public virtual int getGid()
		{
			return gid;
		}

		/// <summary>Sets the group IDs in the credential.</summary>
		/// <remarks>Sets the group IDs in the credential.</remarks>
		/// <param name="gids">Array of group IDs.</param>
		public virtual void setGids(int[] gids)
		{
			this.gids = gids;
		}

		/// <summary>Returns the group IDs from the credential.</summary>
		/// <remarks>Returns the group IDs from the credential.</remarks>
		/// <returns>array of group IDs.</returns>
		public virtual int[] getGids()
		{
			return gids;
		}

		/// <summary>Contains timestamp as supplied through credential.</summary>
		/// <remarks>Contains timestamp as supplied through credential.</remarks>
		private int stamp;

		/// <summary>Contains the machine name of caller supplied through credential.</summary>
		/// <remarks>Contains the machine name of caller supplied through credential.</remarks>
		private string machinename;

		/// <summary>Contains the user ID of caller supplied through credential.</summary>
		/// <remarks>Contains the user ID of caller supplied through credential.</remarks>
		private int uid;

		/// <summary>Contains the group ID of caller supplied through credential.</summary>
		/// <remarks>Contains the group ID of caller supplied through credential.</remarks>
		private int gid;

		/// <summary>
		/// Contains a set of group IDs the caller belongs to, as supplied
		/// through credential.
		/// </summary>
		/// <remarks>
		/// Contains a set of group IDs the caller belongs to, as supplied
		/// through credential.
		/// </remarks>
		private int[] gids;

		/// <summary>
		/// Holds the "shorthand" credentials of type <code>AUTH_SHORT</code>
		/// optionally returned by an ONC/RPC server to be used on subsequent
		/// ONC/RPC calls.
		/// </summary>
		/// <remarks>
		/// Holds the "shorthand" credentials of type <code>AUTH_SHORT</code>
		/// optionally returned by an ONC/RPC server to be used on subsequent
		/// ONC/RPC calls.
		/// </remarks>
		private byte[] shorthandCred;

		/// <summary>Contains an empty array of group IDs.</summary>
		/// <remarks>Contains an empty array of group IDs.</remarks>
        public static readonly int[] NO_GIDS = new int[0];
	}
}
