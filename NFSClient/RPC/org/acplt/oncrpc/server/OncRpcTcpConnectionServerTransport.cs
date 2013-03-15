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

using System.Net.Sockets;
using System.Threading;
using System.Net;
namespace org.acplt.oncrpc.server
{
	/// <summary>
	/// Instances of class <code>OncRpcTcpServerTransport</code> encapsulate
	/// TCP/IP-based XDR streams of ONC/RPC servers.
	/// </summary>
	/// <remarks>
	/// Instances of class <code>OncRpcTcpServerTransport</code> encapsulate
	/// TCP/IP-based XDR streams of ONC/RPC servers. This server transport class
	/// is responsible for receiving ONC/RPC calls over TCP/IP.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <seealso cref="OncRpcServerTransport">OncRpcServerTransport</seealso>
	/// <seealso cref="OncRpcTcpServerTransport">OncRpcTcpServerTransport</seealso>
	/// <seealso cref="OncRpcUdpServerTransport">OncRpcUdpServerTransport</seealso>
	/// <version>$Revision: 1.5 $ $Date: 2008/01/02 15:13:35 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class OncRpcTcpConnectionServerTransport : OncRpcServerTransport
	{
		/// <summary>
		/// Create a new instance of a <code>OncRpcTcpSConnectionerverTransport</code>
		/// which encapsulates TCP/IP-based XDR streams of an ONC/RPC server.
		/// </summary>
		/// <remarks>
		/// Create a new instance of a <code>OncRpcTcpSConnectionerverTransport</code>
		/// which encapsulates TCP/IP-based XDR streams of an ONC/RPC server. This
		/// particular server transport handles individual ONC/RPC connections over
		/// TCP/IP. This constructor is a convenience constructor for those transports
		/// handling only a single ONC/RPC program and version number.
		/// </remarks>
		/// <param name="dispatcher">
		/// Reference to interface of an object capable of
		/// dispatching (handling) ONC/RPC calls.
		/// </param>
		/// <param name="socket">TCP/IP-based socket of new connection.</param>
		/// <param name="program">
		/// Number of ONC/RPC program handled by this server
		/// transport.
		/// </param>
		/// <param name="version">Version number of ONC/RPC program handled.</param>
		/// <param name="bufferSize">
		/// Size of buffer used when receiving and sending
		/// chunks of XDR fragments over TCP/IP. The fragments built up to
		/// form ONC/RPC call and reply messages.
		/// </param>
		/// <param name="parent">Parent server transport which created us.</param>
		/// <param name="transmissionTimeout">Inherited transmission timeout.</param>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		public OncRpcTcpConnectionServerTransport(org.acplt.oncrpc.server.OncRpcDispatchable
			 dispatcher, Socket socket, int program, int version, int bufferSize, org.acplt.oncrpc.server.OncRpcTcpServerTransport
			 parent, int transmissionTimeout) : this(dispatcher, socket, new org.acplt.oncrpc.server.OncRpcServerTransportRegistrationInfo
			[] { new org.acplt.oncrpc.server.OncRpcServerTransportRegistrationInfo(program, 
			version) }, bufferSize, parent, transmissionTimeout)
		{
		}

		/// <summary>
		/// Create a new instance of a <code>OncRpcTcpSConnectionerverTransport</code>
		/// which encapsulates TCP/IP-based XDR streams of an ONC/RPC server.
		/// </summary>
		/// <remarks>
		/// Create a new instance of a <code>OncRpcTcpSConnectionerverTransport</code>
		/// which encapsulates TCP/IP-based XDR streams of an ONC/RPC server. This
		/// particular server transport handles individual ONC/RPC connections over
		/// TCP/IP.
		/// </remarks>
		/// <param name="dispatcher">
		/// Reference to interface of an object capable of
		/// dispatching (handling) ONC/RPC calls.
		/// </param>
		/// <param name="socket">TCP/IP-based socket of new connection.</param>
		/// <param name="info">
		/// Array of program and version number tuples of the ONC/RPC
		/// programs and versions handled by this transport.
		/// </param>
		/// <param name="bufferSize">
		/// Size of buffer used when receiving and sending
		/// chunks of XDR fragments over TCP/IP. The fragments built up to
		/// form ONC/RPC call and reply messages.
		/// </param>
		/// <param name="parent">Parent server transport which created us.</param>
		/// <param name="transmissionTimeout">Inherited transmission timeout.</param>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		public OncRpcTcpConnectionServerTransport(org.acplt.oncrpc.server.OncRpcDispatchable
			 dispatcher, Socket socket, org.acplt.oncrpc.server.OncRpcServerTransportRegistrationInfo
			[] info, int bufferSize, org.acplt.oncrpc.server.OncRpcTcpServerTransport parent
			, int transmissionTimeout) : base(dispatcher, 0, info)
		{
			this.parent = parent;
			this.transmissionTimeout = transmissionTimeout;
			//
			// Make sure the buffer is large enough and resize system buffers
			// accordingly, if possible.
			//
			if (bufferSize < 1024)
			{
				bufferSize = 1024;
			}
			this.socket = socket;
			this.port = ((IPEndPoint)socket.RemoteEndPoint).Port;
			if (socket.SendBufferSize < bufferSize)
			{
				socket.SendBufferSize = bufferSize;
			}
			if (socket.ReceiveBufferSize < bufferSize)
			{
				socket.ReceiveBufferSize = bufferSize;
			}
			//
			// Create the necessary encoding and decoding streams, so we can
			// communicate at all.
			//
			sendingXdr = new org.acplt.oncrpc.XdrTcpEncodingStream(socket, bufferSize);
			receivingXdr = new org.acplt.oncrpc.XdrTcpDecodingStream(socket, bufferSize);
			//
			// Inherit the character encoding setting from the listening
			// transport (parent transport).
			//
			setCharacterEncoding(parent.getCharacterEncoding());
		}

		/// <summary>Close the server transport and free any resources associated with it.</summary>
		/// <remarks>
		/// Close the server transport and free any resources associated with it.
		/// <p>Note that the server transport is <b>not deregistered</b>. You'll
		/// have to do it manually if you need to do so. The reason for this
		/// behaviour is, that the portmapper removes all entries regardless of
		/// the protocol (TCP/IP or UDP/IP) for a given ONC/RPC program number
		/// and version.
		/// <p>Calling this method on a <code>OncRpcTcpServerTransport</code>
		/// results in the listening TCP network socket immediately being closed.
		/// The handler thread will therefore either terminate directly or when
		/// it tries to sent back replies.
		/// </remarks>
		public override void Close()
		{
			if (socket != null)
			{
				//
				// Since there is a non-zero chance of getting race conditions,
				// we now first set the socket instance member to null, before
				// we close the corresponding socket. This avoids null-pointer
				// exceptions in the method which waits for new requests: it is
				// possible that this method is awakened because the socket has
				// been closed before we could set the socket instance member to
				// null. Many thanks to Michael Smith for tracking down this one.
				//
				Socket deadSocket = socket;
				socket = null;
				try
				{
					deadSocket.Close();
				}
				catch (System.IO.IOException)
				{
				}
			}
			if (sendingXdr != null)
			{
				org.acplt.oncrpc.XdrEncodingStream deadXdrStream = sendingXdr;
				sendingXdr = null;
				try
				{
					deadXdrStream.Close();
				}
				catch (System.IO.IOException)
				{
				}
				catch (org.acplt.oncrpc.OncRpcException)
				{
				}
			}
			if (receivingXdr != null)
			{
				org.acplt.oncrpc.XdrDecodingStream deadXdrStream = receivingXdr;
				receivingXdr = null;
				try
				{
					deadXdrStream.Close();
				}
				catch (System.IO.IOException)
				{
				}
				catch (org.acplt.oncrpc.OncRpcException)
				{
				}
			}
			if (parent != null)
			{
				parent.removeTransport(this);
				parent = null;
			}
		}

		~OncRpcTcpConnectionServerTransport()
		{
			if (parent != null)
			{
				parent.removeTransport(this);
			}
		}

		/// <summary>Do not call.</summary>
		/// <remarks>Do not call.</remarks>
		/// <exception cref="System.Exception">
		/// because this method must not be called for an
		/// individual TCP/IP-based server transport.
		/// </exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public override void register()
		{
			throw (new System.Exception("OncRpcTcpServerTransport.register() is abstract " + 
				"and can not be called."));
		}

		/// <summary>Retrieves the parameters sent within an ONC/RPC call message.</summary>
		/// <remarks>
		/// Retrieves the parameters sent within an ONC/RPC call message. It also
		/// makes sure that the deserialization process is properly finished after
		/// the call parameters have been retrieved. Under the hood this method
		/// therefore calls
		/// <see cref="org.acplt.oncrpc.XdrDecodingStream.endDecoding()">org.acplt.oncrpc.XdrDecodingStream.endDecoding()
		/// 	</see>
		/// to free any
		/// pending resources from the decoding stage.
		/// </remarks>
		/// <exception cref="org.acplt.oncrpc.OncRpcException">
		/// if an ONC/RPC exception occurs, like the data
		/// could not be successfully deserialized.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// if an I/O exception occurs, like transmission
		/// failures over the network, etc.
		/// </exception>
		internal override void retrieveCall(org.acplt.oncrpc.XdrAble call)
		{
			call.xdrDecode(receivingXdr);
			if (pendingDecoding)
			{
				pendingDecoding = false;
				receivingXdr.endDecoding();
			}
		}

		/// <summary>
		/// Returns XDR stream which can be used for deserializing the parameters
		/// of this ONC/RPC call.
		/// </summary>
		/// <remarks>
		/// Returns XDR stream which can be used for deserializing the parameters
		/// of this ONC/RPC call. This method belongs to the lower-level access
		/// pattern when handling ONC/RPC calls.
		/// </remarks>
		internal override org.acplt.oncrpc.XdrDecodingStream getXdrDecodingStream()
		{
			return receivingXdr;
		}

		/// <summary>Finishes call parameter deserialization.</summary>
		/// <remarks>
		/// Finishes call parameter deserialization. Afterwards the XDR stream
		/// returned by
		/// <see cref="getXdrDecodingStream()">getXdrDecodingStream()</see>
		/// must not be used any more.
		/// This method belongs to the lower-level access pattern when handling
		/// ONC/RPC calls.
		/// </remarks>
		/// <exception cref="org.acplt.oncrpc.OncRpcException">
		/// if an ONC/RPC exception occurs, like the data
		/// could not be successfully deserialized.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// if an I/O exception occurs, like transmission
		/// failures over the network, etc.
		/// </exception>
		internal override void endDecoding()
		{
			if (pendingDecoding)
			{
				pendingDecoding = false;
				receivingXdr.endDecoding();
			}
		}

		/// <summary>
		/// Returns XDR stream which can be used for eserializing the reply
		/// to this ONC/RPC call.
		/// </summary>
		/// <remarks>
		/// Returns XDR stream which can be used for eserializing the reply
		/// to this ONC/RPC call. This method belongs to the lower-level access
		/// pattern when handling ONC/RPC calls.
		/// </remarks>
		/// <returns>Reference to enecoding XDR stream.</returns>
		internal override org.acplt.oncrpc.XdrEncodingStream getXdrEncodingStream()
		{
			return sendingXdr;
		}

		/// <summary>Begins the sending phase for ONC/RPC replies.</summary>
		/// <remarks>
		/// Begins the sending phase for ONC/RPC replies.
		/// This method belongs to the lower-level access pattern when handling
		/// ONC/RPC calls.
		/// </remarks>
		/// <param name="callInfo">
		/// Information about ONC/RPC call for which we are about
		/// to send back the reply.
		/// </param>
		/// <param name="state">ONC/RPC reply header indicating success or failure.</param>
		/// <exception cref="org.acplt.oncrpc.OncRpcException">
		/// if an ONC/RPC exception occurs, like the data
		/// could not be successfully serialized.
		/// </exception>
		/// <exception cref="System.IO.IOException">if an I/O exception occurs, like transmission
		/// 	</exception>
		internal override void beginEncoding(org.acplt.oncrpc.server.OncRpcCallInformation
			 callInfo, org.acplt.oncrpc.server.OncRpcServerReplyMessage state)
		{
			//
			// In case decoding has not been properly finished, do it now to
			// free up pending resources, etc.
			//
			if (pendingDecoding)
			{
				pendingDecoding = false;
				receivingXdr.endDecoding();
			}
			//
			// Now start encoding using the reply message header first...
			//
			pendingEncoding = true;
			sendingXdr.beginEncoding(callInfo.peerAddress, callInfo.peerPort);
			state.xdrEncode(sendingXdr);
		}

		/// <summary>Finishes encoding the reply to this ONC/RPC call.</summary>
		/// <remarks>
		/// Finishes encoding the reply to this ONC/RPC call. Afterwards you must
		/// not use the XDR stream returned by
		/// <see cref="getXdrEncodingStream()">getXdrEncodingStream()</see>
		/// any
		/// longer.
		/// </remarks>
		/// <exception cref="org.acplt.oncrpc.OncRpcException">
		/// if an ONC/RPC exception occurs, like the data
		/// could not be successfully serialized.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// if an I/O exception occurs, like transmission
		/// failures over the network, etc.
		/// </exception>
		internal override void endEncoding()
		{
			//
			// Close the case. Finito.
			//
			sendingXdr.endEncoding();
			pendingEncoding = false;
		}

		/// <summary>Send back an ONC/RPC reply to the original caller.</summary>
		/// <remarks>
		/// Send back an ONC/RPC reply to the original caller. This is rather a
		/// low-level method, typically not used by applications. Dispatcher handling
		/// ONC/RPC calls have to use the
		/// <see cref="OncRpcCallInformation.reply(org.acplt.oncrpc.XdrAble)">OncRpcCallInformation.reply(org.acplt.oncrpc.XdrAble)
		/// 	</see>
		/// method instead on the
		/// call object supplied to the handler.
		/// </remarks>
		/// <param name="callInfo">
		/// information about the original call, which are necessary
		/// to send back the reply to the appropriate caller.
		/// </param>
		/// <param name="state">
		/// ONC/RPC reply message header indicating success or failure
		/// and containing associated state information.
		/// </param>
		/// <param name="reply">
		/// If not <code>null</code>, then this parameter references
		/// the reply to be serialized after the reply message header.
		/// </param>
		/// <exception cref="org.acplt.oncrpc.OncRpcException">
		/// if an ONC/RPC exception occurs, like the data
		/// could not be successfully serialized.
		/// </exception>
		/// <exception cref="System.IO.IOException">
		/// if an I/O exception occurs, like transmission
		/// failures over the network, etc.
		/// </exception>
		/// <seealso cref="OncRpcCallInformation">OncRpcCallInformation</seealso>
		/// <seealso cref="OncRpcDispatchable">OncRpcDispatchable</seealso>
		internal override void reply(org.acplt.oncrpc.server.OncRpcCallInformation callInfo
			, org.acplt.oncrpc.server.OncRpcServerReplyMessage state, org.acplt.oncrpc.XdrAble
			 reply)
		{
			beginEncoding(callInfo, state);
			if (reply != null)
			{
				reply.xdrEncode(sendingXdr);
			}
			endEncoding();
		}

		/// <summary>
		/// Creates a new thread and uses this thread to handle the new connection
		/// to receive ONC/RPC requests, then dispatching them and finally sending
		/// back reply messages.
		/// </summary>
		/// <remarks>
		/// Creates a new thread and uses this thread to handle the new connection
		/// to receive ONC/RPC requests, then dispatching them and finally sending
		/// back reply messages. Control in the calling thread immediately
		/// returns after the handler thread has been created.
		/// <p>Currently only one call after the other is dispatched, so no
		/// multithreading is done when receiving multiple calls. Instead, later
		/// calls have to wait for the current call to finish before they are
		/// handled.
		/// </remarks>
		public override void listen()
		{
            _TransportHelper t = new _TransportHelper(this);
            Thread listener = new Thread(new ThreadStart(t.run));
            listener.Name =  "TCP server transport connection thread";
            // Should be a Daemon thread if possible
			//listener.setDaemon(true);
			listener.Start();
		}

		private sealed class _TransportHelper 
		{
			public _TransportHelper(OncRpcTcpConnectionServerTransport _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void run()
			{
				this._enclosing._listen();
			}

			private readonly OncRpcTcpConnectionServerTransport _enclosing;
		}

		/// <summary>
		/// The real workhorse handling incoming requests, dispatching them and
		/// sending back replies.
		/// </summary>
		/// <remarks>
		/// The real workhorse handling incoming requests, dispatching them and
		/// sending back replies.
		/// </remarks>
		private void _listen()
		{
			org.acplt.oncrpc.server.OncRpcCallInformation callInfo = new org.acplt.oncrpc.server.OncRpcCallInformation
				(this);
			for (; ; )
			{
				//
				// Start decoding the incomming call. This involves remembering
				// from whom we received the call so we can later send back the
				// appropriate reply message.
				//
				try
				{
                    socket.ReceiveTimeout = 0;
					pendingDecoding = true;
					receivingXdr.beginDecoding();
					callInfo.peerAddress = receivingXdr.getSenderAddress();
					callInfo.peerPort = receivingXdr.getSenderPort();
					socket.ReceiveTimeout = transmissionTimeout;
				}
				catch (System.IO.IOException)
				{
					//
					// In case of I/O Exceptions (especially socket exceptions)
					// close the file and leave the stage. There's nothing we can
					// do anymore.
					//
					Close();
					return;
				}
				catch (org.acplt.oncrpc.OncRpcException)
				{
					//
					// In case of ONC/RPC exceptions at this stage kill the
					// connection.
					//
					Close();
					return;
				}
				try
				{
					//
					// Pull off the ONC/RPC call header of the XDR stream.
					//
					callInfo.callMessage.xdrDecode(receivingXdr);
				}
				catch (System.IO.IOException)
				{
					//
					// In case of I/O Exceptions (especially socket exceptions)
					// close the file and leave the stage. There's nothing we can
					// do anymore.
					//
					Close();
					return;
				}
				catch (org.acplt.oncrpc.OncRpcException)
				{
					//
					// In case of ONC/RPC exceptions at this stage we're silently
					// ignoring that there was some data coming in, as we're not
					// sure we got enough information to send a matching reply
					// message back to the caller.
					//
					if (pendingDecoding)
					{
						pendingDecoding = false;
						try
						{
							receivingXdr.endDecoding();
						}
						catch (System.IO.IOException)
						{
							Close();
							return;
						}
						catch (org.acplt.oncrpc.OncRpcException)
						{
						}
					}
					continue;
				}
				try
				{
					//
					// Let the dispatcher retrieve the call parameters, work on
					// it and send back the reply.
					// To make it once again clear: the dispatch called has to
					// pull off the parameters of the stream!
					//
					dispatcher.dispatchOncRpcCall(callInfo, callInfo.callMessage.program, callInfo.callMessage
						.version, callInfo.callMessage.procedure);
				}
				catch (System.Exception e)
				{
					//
					// In case of some other runtime exception, we report back to
					// the caller a system error. We can not do this if we don't
					// got the exception when serializing the reply, in this case
					// all we can do is to drop the connection. If a reply was not
					// yet started, we can safely send a system error reply.
					//
					if (pendingEncoding)
					{
						Close();
						// Drop the connection...
						return;
					}
					// ...and kill the transport.
					//
					// Looks safe, so we try to send back an error reply.
					//
					if (pendingDecoding)
					{
						pendingDecoding = false;
						try
						{
							receivingXdr.endDecoding();
						}
						catch (System.IO.IOException)
						{
							Close();
							return;
						}
						catch (org.acplt.oncrpc.OncRpcException)
						{
						}
					}
					//
					// Check for authentication exceptions, which are reported back
					// as is. Otherwise, just report a system error
					// -- very generic, indeed.
					//
					try
					{
						if (e is org.acplt.oncrpc.OncRpcAuthenticationException)
						{
							callInfo.failAuthenticationFailed(((org.acplt.oncrpc.OncRpcAuthenticationException
								)e).getAuthStatus());
						}
						else
						{
							callInfo.failSystemError();
						}
					}
					catch (System.IO.IOException)
					{
						Close();
						return;
					}
					catch (org.acplt.oncrpc.OncRpcException)
					{
					}
				}
			}
		}

		//
		// Phew. Done with the error reply. So let's wait for new
		// incoming ONC/RPC calls...
		//
		/// <summary>Set the character encoding for (de-)serializing strings.</summary>
		/// <remarks>Set the character encoding for (de-)serializing strings.</remarks>
		/// <param name="characterEncoding">
		/// the encoding to use for (de-)serializing strings.
		/// If <code>null</code>, the system's default encoding is to be used.
		/// </param>
		public override void setCharacterEncoding(string characterEncoding)
		{
			sendingXdr.setCharacterEncoding(characterEncoding);
			receivingXdr.setCharacterEncoding(characterEncoding);
		}

		/// <summary>Get the character encoding for (de-)serializing strings.</summary>
		/// <remarks>Get the character encoding for (de-)serializing strings.</remarks>
		/// <returns>
		/// the encoding currently used for (de-)serializing strings.
		/// If <code>null</code>, then the system's default encoding is used.
		/// </returns>
		public override string getCharacterEncoding()
		{
			return sendingXdr.getCharacterEncoding();
		}

		/// <summary>
		/// TCP socket used for stream-based communication with ONC/RPC
		/// clients.
		/// </summary>
		/// <remarks>
		/// TCP socket used for stream-based communication with ONC/RPC
		/// clients.
		/// </remarks>
		private Socket socket;

		/// <summary>
		/// XDR encoding stream used for sending replies via TCP/IP back to an
		/// ONC/RPC client.
		/// </summary>
		/// <remarks>
		/// XDR encoding stream used for sending replies via TCP/IP back to an
		/// ONC/RPC client.
		/// </remarks>
		private org.acplt.oncrpc.XdrTcpEncodingStream sendingXdr;

		/// <summary>
		/// XDR decoding stream used when receiving requests via TCP/IP from
		/// ONC/RPC clients.
		/// </summary>
		/// <remarks>
		/// XDR decoding stream used when receiving requests via TCP/IP from
		/// ONC/RPC clients.
		/// </remarks>
		private org.acplt.oncrpc.XdrTcpDecodingStream receivingXdr;

		/// <summary>
		/// Indicates that <code>BeginDecoding</code> has been called for the
		/// receiving XDR stream, so that it should be closed later using
		/// <code>EndDecoding</code>.
		/// </summary>
		/// <remarks>
		/// Indicates that <code>BeginDecoding</code> has been called for the
		/// receiving XDR stream, so that it should be closed later using
		/// <code>EndDecoding</code>.
		/// </remarks>
		private bool pendingDecoding = false;

		/// <summary>
		/// Indicates that <code>BeginEncoding</code> has been called for the
		/// sending XDR stream, so in face of exceptions we can not send an
		/// error reply to the client but only drop the connection.
		/// </summary>
		/// <remarks>
		/// Indicates that <code>BeginEncoding</code> has been called for the
		/// sending XDR stream, so in face of exceptions we can not send an
		/// error reply to the client but only drop the connection.
		/// </remarks>
		private bool pendingEncoding = false;

		/// <summary>
		/// Reference to the TCP/IP transport which created us to handle a
		/// new ONC/RPC connection.
		/// </summary>
		/// <remarks>
		/// Reference to the TCP/IP transport which created us to handle a
		/// new ONC/RPC connection.
		/// </remarks>
		private org.acplt.oncrpc.server.OncRpcTcpServerTransport parent;

		/// <summary>
		/// Timeout during the phase where data is received within calls, or data is
		/// sent within replies.
		/// </summary>
		/// <remarks>
		/// Timeout during the phase where data is received within calls, or data is
		/// sent within replies.
		/// </remarks>
		internal int transmissionTimeout;
	}
}
