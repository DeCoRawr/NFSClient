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
	/// The class <code>OncRpcException</code> indicates ONC/RPC conditions
	/// that a reasonable application might want to catch.
	/// </summary>
	/// <remarks>
	/// The class <code>OncRpcException</code> indicates ONC/RPC conditions
	/// that a reasonable application might want to catch. We follow here the
	/// notation established by the Java environment that exceptions can be
	/// caught while errors usually can't. Because we don't want to throw our
	/// applications out of the virtual machine (should I mock here "out of the
	/// window"?), we only define exceptions.
	/// <p>The class <code>OncRpcException</code> also defines a set of ONC/RPC
	/// error codes as defined by RFC 1831. Note that all these error codes are
	/// solely used on the client-side or server-side, but never transmitted
	/// over the wire. For error codes transmitted over the network, refer to
	/// <see cref="OncRpcAcceptStatus">OncRpcAcceptStatus</see>
	/// and
	/// <see cref="OncRpcRejectStatus">OncRpcRejectStatus</see>
	/// .
	/// <seealso cref="System.Exception">System.Exception</seealso>
    /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.3 $ $Date: 2008/01/02 15:13:35 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
    /// <author>Jay Walters</author>
	[System.Serializable]
	public class OncRpcException : System.Exception
	{
		/// <summary>Defines the serial version UID for <code>OncRpcException</code>.</summary>
		/// <remarks>Defines the serial version UID for <code>OncRpcException</code>.</remarks>
		private const long serialVersionUID = -2170017056632137324L;

		/// <summary>
		/// Constructs an <code>OncRpcException</code> with a reason of
		/// <see cref="RPC_FAILED">RPC_FAILED</see>
		/// .
		/// </summary>
		public OncRpcException() : this(org.acplt.oncrpc.OncRpcException.RPC_FAILED)
		{
		}

		/// <summary>
		/// Constructs an <code>OncRpcException</code> with the specified detail
		/// message.
		/// </summary>
		/// <remarks>
		/// Constructs an <code>OncRpcException</code> with the specified detail
		/// message.
		/// </remarks>
		/// <param name="s">The detail message.</param>
		public OncRpcException(string s) : base()
		{
			reason = RPC_FAILED;
			message = s;
		}

		/// <summary>
		/// Constructs an <code>OncRpcException</code> with the specified detail
		/// reason and message.
		/// </summary>
		/// <remarks>
		/// Constructs an <code>OncRpcException</code> with the specified detail
		/// reason and message. For possible reasons, see below.
		/// </remarks>
		/// <param name="r">The detail reason.</param>
		/// <param name="s">The detail message.</param>
		public OncRpcException(int r, string s) : base()
		{
			reason = r;
			message = s;
		}

		/// <summary>
		/// Constructs an <code>OncRpcException</code> with the specified detail
		/// reason.
		/// </summary>
		/// <remarks>
		/// Constructs an <code>OncRpcException</code> with the specified detail
		/// reason. The detail message is derived automatically from the reason.
		/// </remarks>
		/// <param name="r">
		/// The reason. This can be one of the constants -- oops, that
		/// should be "public final static integers" -- defined in this
		/// interface.
		/// </param>
		public OncRpcException(int r) : base()
		{
			reason = r;
			switch (r)
			{
				case RPC_CANTENCODEARGS:
				{
					message = "can not encode RPC arguments";
					break;
				}

				case RPC_CANTDECODERES:
				{
					message = "can not decode RPC result";
					break;
				}

				case RPC_CANTRECV:
				{
					message = "can not receive ONC/RPC data";
					break;
				}

				case RPC_CANTSEND:
				{
					message = "can not send ONC/RPC data";
					break;
				}

				case RPC_TIMEDOUT:
				{
					message = "ONC/RPC call timed out";
					break;
				}

				case RPC_VERSMISMATCH:
				{
					message = "ONC/RPC version mismatch";
					break;
				}

				case RPC_AUTHERROR:
				{
					message = "ONC/RPC authentification error";
					break;
				}

				case RPC_PROGUNAVAIL:
				{
					message = "ONC/RPC program not available";
					break;
				}

				case RPC_CANTDECODEARGS:
				{
					message = "can not decode ONC/RPC arguments";
					break;
				}

				case RPC_PROGVERSMISMATCH:
				{
					message = "ONC/RPC program version mismatch";
					break;
				}

				case RPC_PROCUNAVAIL:
				{
					message = "ONC/RPC procedure not available";
					break;
				}

				case RPC_SYSTEMERROR:
				{
					message = "ONC/RPC system error";
					break;
				}

				case RPC_UNKNOWNPROTO:
				{
					message = "unknown protocol";
					break;
				}

				case RPC_PMAPFAILURE:
				{
					message = "ONC/RPC portmap failure";
					break;
				}

				case RPC_PROGNOTREGISTERED:
				{
					message = "ONC/RPC program not registered";
					break;
				}

				case RPC_FAILED:
				{
					message = "ONC/RPC generic failure";
					break;
				}

				case RPC_BUFFEROVERFLOW:
				{
					message = "ONC/RPC buffer overflow";
					break;
				}

				case RPC_BUFFERUNDERFLOW:
				{
					message = "ONC/RPC buffer underflow";
					break;
				}

				case RPC_WRONGMESSAGE:
				{
					message = "wrong ONC/RPC message type received";
					break;
				}

				case RPC_CANNOTREGISTER:
				{
					message = "cannot register ONC/RPC port with local portmap";
					break;
				}

				case RPC_SUCCESS:
				default:
				{
					break;
				}
			}
		}

		/// <summary>Returns the error message string of this ONC/RPC object.</summary>
		/// <remarks>Returns the error message string of this ONC/RPC object.</remarks>
		/// <returns>
		/// The error message string of this <code>OncRpcException</code>
		/// object if it was created either with an error message string or an
		/// ONC/RPC error code.
		/// </returns>
		public override string Message
		{
			get
			{
				return message;
			}
		}

		/// <summary>Returns the error reason of this ONC/RPC exception object.</summary>
		/// <remarks>Returns the error reason of this ONC/RPC exception object.</remarks>
		/// <returns>
		/// The error reason of this <code>OncRpcException</code> object if
		/// it was
		/// <see cref="OncRpcException(int)">created</see>
		/// with an error reason; or
		/// <code>RPC_FAILED</code> if it was
		/// <see cref="OncRpcException()">created</see>
		/// with no error reason.
		/// </returns>
		public virtual int getReason()
		{
			return reason;
		}

		/// <summary>The remote procedure call was carried out successfully.</summary>
		/// <remarks>The remote procedure call was carried out successfully.</remarks>
		public const int RPC_SUCCESS = 0;

		/// <summary>
		/// The client can not encode the argments to be sent for the remote
		/// procedure call.
		/// </summary>
		/// <remarks>
		/// The client can not encode the argments to be sent for the remote
		/// procedure call.
		/// </remarks>
		public const int RPC_CANTENCODEARGS = 1;

		/// <summary>The client can not decode the result from the remote procedure call.</summary>
		/// <remarks>The client can not decode the result from the remote procedure call.</remarks>
		public const int RPC_CANTDECODERES = 2;

		/// <summary>Encoded information can not be sent.</summary>
		/// <remarks>Encoded information can not be sent.</remarks>
		public const int RPC_CANTSEND = 3;

		/// <summary>Information to be decoded can not be received.</summary>
		/// <remarks>Information to be decoded can not be received.</remarks>
		public const int RPC_CANTRECV = 4;

		/// <summary>The remote procedure call timed out.</summary>
		/// <remarks>The remote procedure call timed out.</remarks>
		public const int RPC_TIMEDOUT = 5;

		/// <summary>ONC/RPC versions of server and client are not compatible.</summary>
		/// <remarks>ONC/RPC versions of server and client are not compatible.</remarks>
		public const int RPC_VERSMISMATCH = 6;

		/// <summary>
		/// The ONC/RPC server did not accept the authentication sent by the
		/// client.
		/// </summary>
		/// <remarks>
		/// The ONC/RPC server did not accept the authentication sent by the
		/// client. Bad girl/guy!
		/// </remarks>
		public const int RPC_AUTHERROR = 7;

		/// <summary>The ONC/RPC server does not support this particular program.</summary>
		/// <remarks>The ONC/RPC server does not support this particular program.</remarks>
		public const int RPC_PROGUNAVAIL = 8;

		/// <summary>
		/// The ONC/RPC server does not support this particular version of the
		/// program.
		/// </summary>
		/// <remarks>
		/// The ONC/RPC server does not support this particular version of the
		/// program.
		/// </remarks>
		public const int RPC_PROGVERSMISMATCH = 9;

		/// <summary>The given procedure is not available at the ONC/RPC server.</summary>
		/// <remarks>The given procedure is not available at the ONC/RPC server.</remarks>
		public const int RPC_PROCUNAVAIL = 10;

		/// <summary>
		/// The ONC/RPC server could not decode the arguments sent within the
		/// call message.
		/// </summary>
		/// <remarks>
		/// The ONC/RPC server could not decode the arguments sent within the
		/// call message.
		/// </remarks>
		public const int RPC_CANTDECODEARGS = 11;

		/// <summary>
		/// The ONC/RPC server encountered a system error and thus was not able
		/// to carry out the requested remote function call successfully.
		/// </summary>
		/// <remarks>
		/// The ONC/RPC server encountered a system error and thus was not able
		/// to carry out the requested remote function call successfully.
		/// </remarks>
		public const int RPC_SYSTEMERROR = 12;

		/// <summary>The caller specified an unknown/unsupported IP protocol.</summary>
		/// <remarks>
		/// The caller specified an unknown/unsupported IP protocol. Currently,
		/// only
		/// <see cref="OncRpcProtocols.ONCRPC_TCP">OncRpcProtocols.ONCRPC_TCP</see>
		/// and
		/// <see cref="OncRpcProtocols.ONCRPC_UDP">OncRpcProtocols.ONCRPC_UDP</see>
		/// are supported.
		/// </remarks>
		public const int RPC_UNKNOWNPROTO = 17;

		/// <summary>The portmapper could not be contacted at the given host.</summary>
		/// <remarks>The portmapper could not be contacted at the given host.</remarks>
		public const int RPC_PMAPFAILURE = 14;

		/// <summary>The requested program is not registered with the given host.</summary>
		/// <remarks>The requested program is not registered with the given host.</remarks>
		public const int RPC_PROGNOTREGISTERED = 15;

		/// <summary>A generic ONC/RPC exception occured.</summary>
		/// <remarks>A generic ONC/RPC exception occured. Shit happens...</remarks>
		public const int RPC_FAILED = 16;

		/// <summary>A buffer overflow occured with an encoding XDR stream.</summary>
		/// <remarks>
		/// A buffer overflow occured with an encoding XDR stream. This happens
		/// if you use UDP-based (datagram-based) XDR streams and you try to encode
		/// more data than can fit into the sending buffers.
		/// </remarks>
		public const int RPC_BUFFEROVERFLOW = 42;

		/// <summary>A buffer underflow occured with an decoding XDR stream.</summary>
		/// <remarks>
		/// A buffer underflow occured with an decoding XDR stream. This happens
		/// if you try to decode more data than was sent by the other communication
		/// partner.
		/// </remarks>
		public const int RPC_BUFFERUNDERFLOW = 43;

		/// <summary>
		/// Either a ONC/RPC server or client received the wrong type of ONC/RPC
		/// message when waiting for a request or reply.
		/// </summary>
		/// <remarks>
		/// Either a ONC/RPC server or client received the wrong type of ONC/RPC
		/// message when waiting for a request or reply. Currently, only the
		/// decoding methods of the classes
		/// <see cref="OncRpcCallMessage">OncRpcCallMessage</see>
		/// and
		/// <see cref="OncRpcReplyMessage">OncRpcReplyMessage</see>
		/// throw exceptions with this reason.
		/// </remarks>
		public const int RPC_WRONGMESSAGE = 44;

		/// <summary>
		/// Indicates that a server could not register a transport with the
		/// ONC/RPC port mapper.
		/// </summary>
		/// <remarks>
		/// Indicates that a server could not register a transport with the
		/// ONC/RPC port mapper.
		/// </remarks>
		public const int RPC_CANNOTREGISTER = 45;

		/// <summary>
		/// Specific detail (reason) about this <code>OncRpcException</code>,
		/// like the ONC/RPC error code, as defined by the <code>RPC_xxx</code>
		/// constants of this interface.
		/// </summary>
		/// <remarks>
		/// Specific detail (reason) about this <code>OncRpcException</code>,
		/// like the ONC/RPC error code, as defined by the <code>RPC_xxx</code>
		/// constants of this interface.
		/// </remarks>
		/// <serial></serial>
		private int reason;

		/// <summary>
		/// Specific detail about this <code>OncRpcException</code>, like a
		/// detailed error message.
		/// </summary>
		/// <remarks>
		/// Specific detail about this <code>OncRpcException</code>, like a
		/// detailed error message.
		/// </remarks>
		/// <serial></serial>
		private string message;
	}
}
