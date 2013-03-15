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
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System;
namespace org.acplt.oncrpc
{
	/// <summary>
	/// ONC/RPC client which communicates with ONC/RPC servers over the network
	/// using the stream-oriented protocol TCP/IP.
	/// </summary>
	/// <remarks>
	/// ONC/RPC client which communicates with ONC/RPC servers over the network
	/// using the stream-oriented protocol TCP/IP.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.5 $ $Date: 2005/11/11 21:04:30 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class OncRpcTcpClient : OncRpcClient
	{
		/// <summary>
		/// Constructs a new <code>OncRpcTcpClient</code> object, which connects
		/// to the ONC/RPC server at <code>host</code> for calling remote procedures
		/// of the given { program, version }.
		/// </summary>
		/// <remarks>
		/// Constructs a new <code>OncRpcTcpClient</code> object, which connects
		/// to the ONC/RPC server at <code>host</code> for calling remote procedures
		/// of the given { program, version }.
		/// <p>Note that the construction of an <code>OncRpcTcpClient</code>
		/// object will result in communication with the portmap process at
		/// <code>host</code>.
		/// </remarks>
		/// <param name="host">The host where the ONC/RPC server resides.</param>
		/// <param name="program">Program number of the ONC/RPC server to call.</param>
		/// <param name="version">Program version number.</param>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public OncRpcTcpClient(IPAddress host, int program, int version) : this
			(host, program, version, 0, 0,true)
		{
		}

		/// <summary>
		/// Constructs a new <code>OncRpcTcpClient</code> object, which connects
		/// to the ONC/RPC server at <code>host</code> for calling remote procedures
		/// of the given { program, version }.
		/// </summary>
		/// <remarks>
		/// Constructs a new <code>OncRpcTcpClient</code> object, which connects
		/// to the ONC/RPC server at <code>host</code> for calling remote procedures
		/// of the given { program, version }.
		/// <p>Note that the construction of an <code>OncRpcTcpClient</code>
		/// object will result in communication with the portmap process at
		/// <code>host</code> if <code>port</code> is <code>0</code>.
		/// </remarks>
		/// <param name="host">The host where the ONC/RPC server resides.</param>
		/// <param name="program">Program number of the ONC/RPC server to call.</param>
		/// <param name="version">Program version number.</param>
		/// <param name="port">
		/// The port number where the ONC/RPC server can be contacted.
		/// If <code>0</code>, then the <code>OncRpcUdpClient</code> object will
		/// ask the portmapper at <code>host</code> for the port number.
		/// </param>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public OncRpcTcpClient(IPAddress host, int program, int version, int port,bool useSecurePort
            )
            : this(host, program, version, port, 0,useSecurePort)
		{
		}

		/// <summary>
		/// Constructs a new <code>OncRpcTcpClient</code> object, which connects
		/// to the ONC/RPC server at <code>host</code> for calling remote procedures
		/// of the given { program, version }.
		/// </summary>
		/// <remarks>
		/// Constructs a new <code>OncRpcTcpClient</code> object, which connects
		/// to the ONC/RPC server at <code>host</code> for calling remote procedures
		/// of the given { program, version }.
		/// <p>Note that the construction of an <code>OncRpcTcpClient</code>
		/// object will result in communication with the portmap process at
		/// <code>host</code> if <code>port</code> is <code>0</code>.
		/// </remarks>
		/// <param name="host">The host where the ONC/RPC server resides.</param>
		/// <param name="program">Program number of the ONC/RPC server to call.</param>
		/// <param name="version">Program version number.</param>
		/// <param name="port">
		/// The port number where the ONC/RPC server can be contacted.
		/// If <code>0</code>, then the <code>OncRpcUdpClient</code> object will
		/// ask the portmapper at <code>host</code> for the port number.
		/// </param>
		/// <param name="bufferSize">
		/// Size of receive and send buffers. In contrast to
		/// UDP-based ONC/RPC clients, messages larger than the specified
		/// buffer size can still be sent and received. The buffer is only
		/// necessary to handle the messages and the underlaying streams will
		/// break up long messages automatically into suitable pieces.
		/// Specifying zero will select the default buffer size (currently
		/// 8192 bytes).
		/// </param>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public OncRpcTcpClient(IPAddress host, int program, int version, int port
            , int bufferSize, bool useSecurePort)
            : this(host, program, version, port, bufferSize, -1, useSecurePort)
		{
		}

		/// <summary>
		/// Constructs a new <code>OncRpcTcpClient</code> object, which connects
		/// to the ONC/RPC server at <code>host</code> for calling remote procedures
		/// of the given { program, version }.
		/// </summary>
		/// <remarks>
		/// Constructs a new <code>OncRpcTcpClient</code> object, which connects
		/// to the ONC/RPC server at <code>host</code> for calling remote procedures
		/// of the given { program, version }.
		/// <p>Note that the construction of an <code>OncRpcTcpClient</code>
		/// object will result in communication with the portmap process at
		/// <code>host</code> if <code>port</code> is <code>0</code>.
		/// </remarks>
		/// <param name="host">The host where the ONC/RPC server resides.</param>
		/// <param name="program">Program number of the ONC/RPC server to call.</param>
		/// <param name="version">Program version number.</param>
		/// <param name="port">
		/// The port number where the ONC/RPC server can be contacted.
		/// If <code>0</code>, then the <code>OncRpcUdpClient</code> object will
		/// ask the portmapper at <code>host</code> for the port number.
		/// </param>
		/// <param name="bufferSize">
		/// Size of receive and send buffers. In contrast to
		/// UDP-based ONC/RPC clients, messages larger than the specified
		/// buffer size can still be sent and received. The buffer is only
		/// necessary to handle the messages and the underlaying streams will
		/// break up long messages automatically into suitable pieces.
		/// Specifying zero will select the default buffer size (currently
		/// 8192 bytes).
		/// </param>
		/// <param name="timeout">
		/// Maximum timeout in milliseconds when connecting to
		/// the ONC/RPC server. If negative, a default implementation-specific
		/// timeout setting will apply. <i>Note that this timeout only applies
		/// to the connection phase, but <b>not</b> to later communication.</i>
		/// </param>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public OncRpcTcpClient(IPAddress host, int program, int version, int port
            , int bufferSize, int timeout, bool useSecurePort)
            : base(host, program, version, port, org.acplt.oncrpc.OncRpcProtocols
			.ONCRPC_TCP)
		{
			//
			// Construct the inherited part of our object. This will also try to
			// lookup the port of the desired ONC/RPC server, if no port number
			// was specified (port = 0).
			//
			//
			// Let the host operating system choose which port (and network
			// interface) to use. Then set the buffer sizes for sending and
			// receiving UDP datagrams. Finally set the destination of packets.
			//
			if (bufferSize == 0)
			{
				bufferSize = 128000;
			}
			// default setting
			if (bufferSize < 1024)
			{
				bufferSize = 1024;
			}
			//
			// Note that we use this.port at this time, because the superclass
			// might have resolved the port number in case the caller specified
			// simply 0 as the port number.
			//
            // Construct the socket and connect
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


            if (useSecurePort)
            {
               var localEp = new IPEndPoint(IPAddress.Any, GetLocalPort());
               socket.Bind(localEp);
            }


            IPEndPoint endPoint = new IPEndPoint(host, this.port);
            socket.Connect(endPoint);
            if (timeout >= 0)
            {
                socket.SendTimeout = timeout;
                socket.ReceiveTimeout = timeout;
            }
            socket.NoDelay = true;
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
		}



        private int GetLocalPort()
        {
            Random random = new Random();
            int randomNumber = 0;
            bool inUse = false;
            do
            {
                randomNumber = random.Next(665, 1023);
                inUse = false;
                foreach (var tcpListener in IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners())
                {
                    if (tcpListener.Port == randomNumber)
                    {
                        inUse = true;
                        break;
                    }
                }
            } while (inUse);
            return randomNumber;
        }



		/// <summary>
		/// Close the connection to an ONC/RPC server and free all network-related
		/// resources.
		/// </summary>
		/// <remarks>
		/// Close the connection to an ONC/RPC server and free all network-related
		/// resources. Well -- at least hope, that the Java VM will sometimes free
		/// some resources. Sigh.
		/// </remarks>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public override void close()
		{
            if (socket != null)
            {
                try
                {
                    socket.Close();
                }
                catch (System.IO.IOException)
                {
                }
                socket = null;
            }
            socket = null;
			if (sendingXdr != null)
			{
				try
				{
					sendingXdr.Close();
				}
				catch (System.IO.IOException)
				{
				}
				sendingXdr = null;
			}
			if (receivingXdr != null)
			{
				try
				{
					receivingXdr.Close();
				}
				catch (System.IO.IOException)
				{
				}
				receivingXdr = null;
			}
		}

		/// <summary>Calls a remote procedure on an ONC/RPC server.</summary>
		/// <remarks>
		/// Calls a remote procedure on an ONC/RPC server.
		/// <p>Please note that while this method supports call batching by
		/// setting the communication timeout to zero
		/// (<code>setTimeout(0)</code>) you should better use
		/// <see cref="batchCall(int, XdrAble, bool)">batchCall(int, XdrAble, bool)</see>
		/// as it provides better control over when the
		/// batch should be flushed to the server.
		/// </remarks>
		/// <param name="procedureNumber">Procedure number of the procedure to call.</param>
		/// <param name="versionNumber">Protocol version number.</param>
		/// <param name="params">
		/// The parameters of the procedure to call, contained
		/// in an object which implements the
		/// <see cref="XdrAble">XdrAble</see>
		/// interface.
		/// </param>
		/// <param name="result">The object receiving the result of the procedure call.</param>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public override void call(int procedureNumber, int versionNumber, org.acplt.oncrpc.XdrAble
			 @params, org.acplt.oncrpc.XdrAble result)
		{
			lock (this)
			{
				// Refresh:
				for (int refreshesLeft = 1; refreshesLeft >= 0; --refreshesLeft)
				{
					//
					// First, build the ONC/RPC call header. Then put the sending
					// stream into a known state and encode the parameters to be
					// sent. Finally tell the encoding stream to send all its data
					// to the server. Then wait for an answer, receive it and decode
					// it. So that's the bottom line of what we do right here.
					//
					nextXid();
					org.acplt.oncrpc.OncRpcClientCallMessage callHeader = new org.acplt.oncrpc.OncRpcClientCallMessage
						(xid, program, versionNumber, procedureNumber, auth);
					org.acplt.oncrpc.OncRpcClientReplyMessage replyHeader = new org.acplt.oncrpc.OncRpcClientReplyMessage
						(auth);
					//
					// Send call message to server. If we receive an IOException,
					// then we'll throw the appropriate ONC/RPC (client) exception.
					// Note that we use a connected stream, so we don't need to
					// specify a destination when beginning serialization.
					//
					try
					{
                        socket.ReceiveTimeout = transmissionTimeout;
						sendingXdr.beginEncoding(null, 0);
						callHeader.xdrEncode(sendingXdr);
						@params.xdrEncode(sendingXdr);
						if (timeout != 0)
						{
							sendingXdr.endEncoding();
						}
						else
						{
							sendingXdr.endEncoding(false);
						}
					}
					catch (System.IO.IOException e)
					{
						throw (new org.acplt.oncrpc.OncRpcException(org.acplt.oncrpc.OncRpcException.RPC_CANTSEND
							, e.Message));
					}
					//
					// Receive reply message from server -- at least try to do so...
					// In case of batched calls we don't need no stinkin' answer, so
					// we can do other, more interesting things.
					//
					if (timeout == 0)
					{
						return;
					}
					try
					{
						//
						// Keep receiving until we get the matching reply.
						//
						while (true)
						{
                            socket.ReceiveTimeout = timeout;
							receivingXdr.beginDecoding();
                            socket.ReceiveTimeout = transmissionTimeout;
							//
							// First, pull off the reply message header of the
							// XDR stream. In case we also received a verifier
							// from the server and this verifier was invalid, broken
							// or tampered with, we will get an
							// OncRpcAuthenticationException right here, which will
							// propagate up to the caller. If the server reported
							// an authentication problem itself, then this will
							// be handled as any other rejected ONC/RPC call.
							//
							try
							{
								replyHeader.xdrDecode(receivingXdr);
							}
							catch (org.acplt.oncrpc.OncRpcException e)
							{
								//
								// ** SF bug #1262106 **
								//
								// We ran into some sort of trouble. Usually this will have
								// been a buffer underflow. Whatever, end the decoding process
								// and ensure this way that the next call has a chance to start
								// from a clean state.
								//
								receivingXdr.endDecoding();

                               // continue;

                                throw (e);
							}
							//
							// Only deserialize the result, if the reply matches the
							// call. Otherwise skip this record.
							//
							if (replyHeader.messageId == callHeader.messageId)
							{
								break;
							}
							receivingXdr.endDecoding();
						}
						//
						// Make sure that the call was accepted. In case of unsuccessful
						// calls, throw an exception, if it's not an authentication
						// exception. In that case try to refresh the credential first.
						//
						if (!replyHeader.successfullyAccepted())
						{
							receivingXdr.endDecoding();
							//
							// Check whether there was an authentication
							// problem. In this case first try to refresh the
							// credentials.
							//
							if ((refreshesLeft > 0) && (replyHeader.replyStatus == org.acplt.oncrpc.OncRpcReplyStatus
								.ONCRPC_MSG_DENIED) && (replyHeader.rejectStatus == org.acplt.oncrpc.OncRpcRejectStatus
								.ONCRPC_AUTH_ERROR) && (auth != null) && auth.canRefreshCred())
							{
								// continue Refresh;
								continue;
							}
							//
							// Nope. No chance. This gets tough.
							//
							throw (replyHeader.newException());
						}
						try
						{
							result.xdrDecode(receivingXdr);
						}
						catch (org.acplt.oncrpc.OncRpcException e)
						{
							//
							// ** SF bug #1262106 **
							//
							// We ran into some sort of trouble. Usually this will have
							// been a buffer underflow. Whatever, end the decoding process
							// and ensure this way that the next call has a chance to start
							// from a clean state.
							//
							receivingXdr.endDecoding();
                            
							throw (e);

						}
						//
						// Free pending resources of buffer and exit the call loop,
						// returning the reply to the caller through the result
						// object.
						//
						receivingXdr.endDecoding();
						return;
					}
					catch (System.IO.IOException e)
					{
						//
						// Argh. Trouble with the transport. Seems like we can't
						// receive data. Gosh. Go away!
						//
						throw (new org.acplt.oncrpc.OncRpcException(org.acplt.oncrpc.OncRpcException.RPC_CANTRECV
							, e.Message));
					}
				}
			}
		}

		// for ( refreshesLeft )
		/// <summary>Issues a batched call for a remote procedure to an ONC/RPC server.</summary>
		/// <remarks>
		/// Issues a batched call for a remote procedure to an ONC/RPC server.
		/// Below is a small example (exception handling ommited for clarity):
		/// <pre>
		/// OncRpcTcpClient client = new OncRpcTcpClient(
		/// InetAddress.getByName("localhost"),
		/// myprogramnumber, myprogramversion,
		/// OncRpcProtocols.ONCRPC_TCP);
		/// client.callBatch(42, myparams, false);
		/// client.callBatch(42, myotherparams, false);
		/// client.callBatch(42, myfinalparams, true);
		/// </pre>
		/// In the example above, three calls are batched in a row and only be sent
		/// all together with the third call. Note that batched calls must not expect
		/// replies, with the only exception being the last call in a batch:
		/// <pre>
		/// client.callBatch(42, myparams, false);
		/// client.callBatch(42, myotherparams, false);
		/// client.call(43, myfinalparams, myfinalresult);
		/// </pre>
		/// </remarks>
		/// <param name="procedureNumber">Procedure number of the procedure to call.</param>
		/// <param name="params">
		/// The parameters of the procedure to call, contained
		/// in an object which implements the
		/// <see cref="XdrAble">XdrAble</see>
		/// interface.
		/// </param>
		/// <param name="flush">
		/// Make sure that all pending batched calls are sent to
		/// the server.
		/// </param>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public virtual void batchCall(int procedureNumber, org.acplt.oncrpc.XdrAble @params
			, bool flush)
		{
			lock (this)
			{
				//
				// First, build the ONC/RPC call header. Then put the sending
				// stream into a known state and encode the parameters to be
				// sent. Finally tell the encoding stream to send all its data
				// to the server. We don't then need to wait for an answer. And
				// we don't need to take care of credential refreshes either.
				//
				nextXid();
				org.acplt.oncrpc.OncRpcClientCallMessage callHeader = new org.acplt.oncrpc.OncRpcClientCallMessage
					(xid, program, version, procedureNumber, auth);
				//
				// Send call message to server. If we receive an IOException,
				// then we'll throw the appropriate ONC/RPC (client) exception.
				// Note that we use a connected stream, so we don't need to
				// specify a destination when beginning serialization.
				//
				try
				{
					socket.SendTimeout = transmissionTimeout;
					sendingXdr.beginEncoding(null, 0);
					callHeader.xdrEncode(sendingXdr);
					@params.xdrEncode(sendingXdr);
					sendingXdr.endEncoding(flush);
				}
				catch (System.IO.IOException e)
				{
					throw (new org.acplt.oncrpc.OncRpcException(org.acplt.oncrpc.OncRpcException.RPC_CANTSEND
						, e.Message));
				}
			}
		}

		/// <summary>
		/// Set the timout for remote procedure calls to wait for an answer from
		/// the ONC/RPC server.
		/// </summary>
		/// <remarks>
		/// Set the timout for remote procedure calls to wait for an answer from
		/// the ONC/RPC server. If the timeout expires,
		/// <see cref="OncRpcClient.call(int, XdrAble, XdrAble)">OncRpcClient.call(int, XdrAble, XdrAble)
		/// 	</see>
		/// will raise a
		/// <see cref="InterruptedIOException">InterruptedIOException</see>
		/// . The default timeout value is
		/// 30 seconds (30,000 milliseconds). The timeout must be &gt; 0.
		/// A timeout of zero indicates a batched call, for which no reply message
		/// is expected.
		/// </remarks>
		/// <param name="milliseconds">
		/// Timeout in milliseconds. A timeout of zero indicates
		/// batched calls.
		/// </param>
		public override void setTimeout(int milliseconds)
		{
			base.setTimeout(milliseconds);
		}

		/// <summary>Set the timeout used during transmission of data.</summary>
		/// <remarks>
		/// Set the timeout used during transmission of data. If the flow of data
		/// when sending calls or receiving replies blocks longer than the given
		/// timeout, an exception is thrown. The timeout must be &gt; 0.
		/// </remarks>
		/// <param name="milliseconds">Transmission timeout in milliseconds.</param>
		public virtual void setTransmissionTimeout(int milliseconds)
		{
			if (milliseconds <= 0)
			{
				throw (new System.ArgumentException("transmission timeout must be > 0"));
			}
			transmissionTimeout = milliseconds;
		}

		/// <summary>
		/// Retrieve the current timeout used during transmission phases (call and
		/// reply phases).
		/// </summary>
		/// <remarks>
		/// Retrieve the current timeout used during transmission phases (call and
		/// reply phases).
		/// </remarks>
		/// <returns>Current transmission timeout.</returns>
		public virtual int getTransmissionTimeout()
		{
			return transmissionTimeout;
		}

		/// <summary>Set the character encoding for (de-)serializing strings.</summary>
		/// <remarks>Set the character encoding for (de-)serializing strings.</remarks>
		/// <param name="characterEncoding">
		/// the encoding to use for (de-)serializing strings.
		/// If <code>null</code>, the system's default encoding is to be used.
		/// </param>
		public override void setCharacterEncoding(string characterEncoding)
		{
			receivingXdr.setCharacterEncoding(characterEncoding);
			sendingXdr.setCharacterEncoding(characterEncoding);
		}

		/// <summary>Get the character encoding for (de-)serializing strings.</summary>
		/// <remarks>Get the character encoding for (de-)serializing strings.</remarks>
		/// <returns>
		/// the encoding currently used for (de-)serializing strings.
		/// If <code>null</code>, then the system's default encoding is used.
		/// </returns>
		public override string getCharacterEncoding()
		{
			return receivingXdr.getCharacterEncoding();
		}

		/// <summary>
		/// TCP socket used for stream-oriented communication with an ONC/RPC
		/// server.
		/// </summary>
		/// <remarks>
		/// TCP socket used for stream-oriented communication with an ONC/RPC
		/// server.
		/// </remarks>
		private Socket socket;

		/// <summary>
		/// XDR encoding stream used for sending requests via TCP/IP to an ONC/RPC
		/// server.
		/// </summary>
		/// <remarks>
		/// XDR encoding stream used for sending requests via TCP/IP to an ONC/RPC
		/// server.
		/// </remarks>
		internal org.acplt.oncrpc.XdrTcpEncodingStream sendingXdr;

		/// <summary>
		/// XDR decoding stream used when receiving replies via TCP/IP from an
		/// ONC/RPC server.
		/// </summary>
		/// <remarks>
		/// XDR decoding stream used when receiving replies via TCP/IP from an
		/// ONC/RPC server.
		/// </remarks>
		internal org.acplt.oncrpc.XdrTcpDecodingStream receivingXdr;

		/// <summary>
		/// Timeout during the phase where data is sent within calls, or data is
		/// received within replies.
		/// </summary>
		/// <remarks>
		/// Timeout during the phase where data is sent within calls, or data is
		/// received within replies.
		/// </remarks>
		internal int transmissionTimeout = 30000;
	}
}
