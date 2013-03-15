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
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;
namespace org.acplt.oncrpc
{
	/// <summary>
	/// ONC/RPC client which communicates with ONC/RPC servers over the network
	/// using the datagram-oriented protocol UDP/IP.
	/// </summary>
	/// <remarks>
	/// ONC/RPC client which communicates with ONC/RPC servers over the network
	/// using the datagram-oriented protocol UDP/IP.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.6 $ $Date: 2007/05/29 18:48:27 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class OncRpcUdpClient : OncRpcClient
	{
		/// <summary>
		/// Constructs a new <code>OncRpcUdpClient</code> object, which connects
		/// to the ONC/RPC server at <code>host</code> for calling remote procedures
		/// of the given { program, version }.
		/// </summary>
		/// <remarks>
		/// Constructs a new <code>OncRpcUdpClient</code> object, which connects
		/// to the ONC/RPC server at <code>host</code> for calling remote procedures
		/// of the given { program, version }.
		/// <p>Note that the construction of an <code>OncRpcUdpProtocolClient</code>
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
        /// <param name="useSecurePort">The local binding port will be less than 1024.</param>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public OncRpcUdpClient(IPAddress host, int program, int version, int port,
			bool useSecurePort) : this(host, program, version, port, 8192, useSecurePort)
		{
		}

		/// <summary>
		/// Constructs a new <code>OncRpcUdpClient</code> object, which connects
		/// to the ONC/RPC server at <code>host</code> for calling remote procedures
		/// of the given { program, version }.
		/// </summary>
		/// <remarks>
		/// Constructs a new <code>OncRpcUdpClient</code> object, which connects
		/// to the ONC/RPC server at <code>host</code> for calling remote procedures
		/// of the given { program, version }.
		/// <p>Note that the construction of an <code>OncRpcUdpProtocolClient</code>
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
		/// The buffer size used for sending and receiving UDP
		/// datagrams.
		/// </param>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public OncRpcUdpClient(IPAddress host, int program, int version, int port
			, int bufferSize, bool useSecurePort) : base(host, program, version, port, OncRpcProtocols
			.ONCRPC_UDP)
		{
            retransmissionTimeout = base.getTimeout();

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
			if (bufferSize < 1024)
			{
				bufferSize = 1024;
			}
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            if (useSecurePort)
            {
                var localEp = new IPEndPoint(IPAddress.Any, GetLocalPort());
                socket.Bind(localEp);

            }

			if (socket.SendBufferSize < bufferSize)
			{
				socket.SendBufferSize = bufferSize;
			}
			if (socket.ReceiveBufferSize < bufferSize)
			{
				socket.ReceiveBufferSize = bufferSize;
			}
			//
			// Note: we don't do a
			//   socket.connect(host, this.port);
			// here anymore. XdrUdpEncodingStream long since then supported
			// specifying the destination of an ONC/RPC UDP packet when
			// start serialization. In addition, connecting a UDP socket disables
			// the socket's ability to receive broadcasts. Without connecting you
			// can send an ONC/RPC call to the broadcast address of the network
			// and receive multiple replies.
			//
			// Create the necessary encoding and decoding streams, so we can
			// communicate at all.
			//
			sendingXdr = new XdrUdpEncodingStream(socket, bufferSize);
			receivingXdr = new XdrUdpDecodingStream(socket, bufferSize);
		}

        private int GetLocalPort()
        {
            Random random = new Random();
            int randomNumber = 0;
            bool inUse = false;
            do
            {
                randomNumber = random.Next(665, 1022);

                inUse = false;
                foreach (var udpListener in IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners())
                {
                    if (udpListener.Port == randomNumber)
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
				socket.Close();
				socket = null;
			}
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
		/// <p>The <code>OncRpcUdpClient</code> uses a similar timeout scheme as
		/// the genuine Sun C implementation of ONC/RPC: it starts with a timeout
		/// of one second when waiting for a reply. If no reply is received within
		/// this time frame, the client doubles the timeout, sends a new request
		/// and then waits again for a reply. In every case the client will wait
		/// no longer than the total timeout set through the
		/// <see cref="OncRpcClient.setTimeout(int)">OncRpcClient.setTimeout(int)</see>
		/// method.
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
		public override void call(int procedureNumber, int versionNumber, XdrAble
			 @params, XdrAble result)
		{
			lock (this)
			{
				//Refresh:
				for (int refreshesLeft = 1; refreshesLeft >= 0; --refreshesLeft)
				{
					bool refreshFlag = false;
					//
					// First, build the ONC/RPC call header. Then put the sending
					// stream into a known state and encode the parameters to be
					// sent. Finally tell the encoding stream to send all its data
					// to the server. Then wait for an answer, receive it and decode
					// it. So that's the bottom line of what we do right here.
					//
					nextXid();
					//
					// We only create our request message once and reuse it in case
					// retransmission should be necessary -- with the exception being
					// credential refresh. In this case we need to create a new
					// request message.
					//
					OncRpcClientCallMessage callHeader = new OncRpcClientCallMessage
						(xid, program, versionNumber, procedureNumber, auth);
					OncRpcClientReplyMessage replyHeader = new OncRpcClientReplyMessage
						(auth);
					long stopTime = (DateTime.Now.Ticks/TimeSpan.TicksPerMillisecond) + timeout;
					int resendTimeout = retransmissionTimeout;
					do
					{
						//
						// Now enter the great loop where we send calls out to the server
						// and then sit there waiting for a reply. If none comes, we first
						// resend our call after one second, then two seconds, four seconds,
						// and so on, until we have reached the timeout for the call in total.
						// Note that this setting only applies if exponential back-off
						// retransmission has been selected. Per default we do not retransmit
						// any more, in order to be in line with the SUNRPC implementations.
						//
						try
						{
							//
							// Send call message to server. Remember that we've already
							// "connected" the datagram socket, so java.net knows whom
							// to send the datagram packets.
							//
							sendingXdr.beginEncoding(host, port);
							callHeader.xdrEncode(sendingXdr);
							@params.xdrEncode(sendingXdr);
							sendingXdr.endEncoding();
						}
						catch (System.IO.IOException e)
						{
							throw (new OncRpcException(OncRpcException.RPC_CANTSEND
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
						//
						// Wait for an answer to arrive...
						//
						for (; ; )
						{
							try
							{
                                long currentTimeout = stopTime - (DateTime.Now.Ticks/TimeSpan.TicksPerMillisecond);
								if (currentTimeout > resendTimeout)
								{
									currentTimeout = resendTimeout;
								}
								else
								{
									if (currentTimeout < 1)
									{
										//
										// as setSoTimeout interprets a timeout of zero as
										// infinite (?§$@%&!!!) we need to ensure that we
										// have a finite timeout, albeit maybe an infinitesimal
										// finite one.
										currentTimeout = 1;
									}
								}
								socket.ReceiveTimeout = (int) currentTimeout;
								receivingXdr.beginDecoding();
								//
								// Only accept incomming reply if it comes from the same
								// address we've sent the ONC/RPC call to. Otherwise throw
								// away the datagram packet containing the reply and start
								// over again, waiting for the next reply to arrive.
								//
								if (host.Equals(receivingXdr.getSenderAddress()))
								{
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
										throw (e);
									}
									//
									// Only deserialize the result, if the reply matches the call
									// and if the reply signals a successful call. In case of an
									// unsuccessful call (which mathes our call nevertheless) throw
									// an exception.
									//
									if (replyHeader.messageId == callHeader.messageId)
									{
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
												//
												// Think about using a TAB size of four ;)
												//
												// Another instance of "CONTINUE considered
												// useful"...
												//
												refreshFlag = true;
												break;
											}
											//  continue Refresh;
											//
											// Nope. No chance. This gets tough.
											//
											throw (replyHeader.newException());
										}
										//
										// The reply header is okay and the call had been
										// accepted by the ONC/RPC server, so we can now
										// proceed to decode the outcome of the RPC.
										//
										try
										{
											result.xdrDecode(receivingXdr);
										}
										catch (OncRpcException e)
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
								}
							}
							catch (SocketException)
							{
								//
								// The message id did no match -- probably some
								// old UDP datagram which just popped up from the
								// middle of the Internet.
								//
								// Yet another case of "CONTINUE considered not
								// harmful"...
								//
								// [Nothing to do here, just wait for the next datagram]
								//
								// IP address of received UDP datagram is not the same
								// as the IP address of the ONC/RPC server.
								//
								// [Nothing to do here, just wait for the next datagram]
								//
								// Note that we only catch timeouts here, but no other
								// exceptions. Those others will go up further until someone
								// catches them. The timeouts are caught, so they can do no
								// damage but instead we start another round of sending a
								// request and waiting for a reply. Reminds me of NASA and
								// their "Mars Polar Lander"...
								//
								// Note that we need to leave the inner waiting loop here,
								// as we might need to resend the (lost) RPC request
								// datagram.
								//
                                //Console.Out.WriteLine("This is supposed to be ignored");
                                //Console.Out.WriteLine(e.Message);
                                //Console.Out.WriteLine(e.StackTrace);
								break;
							}
							catch (IOException e)
							{
								//
								// Argh. Trouble with the transport. Seems like we can't
								// receive data. Gosh. Go away!
								//
								try
								{
									receivingXdr.endDecoding();
								}
								catch (IOException)
								{
								}
								// skip UDP record
								throw (new OncRpcException(OncRpcException.RPC_CANTRECV
									, e.Message));
							}
							catch (org.acplt.oncrpc.OncRpcException e)
							{
								//
								// Ooops. An ONC/RPC exception. Let us rethrow this one,
								// as we won't have nothin' to do with it...
								//
								try
								{
									receivingXdr.endDecoding();
								}
								catch (System.IO.IOException)
								{
								}
								// skip UDP record
								//
								// Well, in case we got not a *reply* RPC message back,
								// we keep listening for messages.
								//
								if (e.getReason() != org.acplt.oncrpc.OncRpcException.RPC_WRONGMESSAGE)
								{
									throw (e);
								}
							}
							//
							// We can not make use of the reply we just received, so
							// we need to dump it.
							//
							// This should raise no exceptions, when skipping the UDP
							// record. So if one is raised, we will rethrow an ONC/RPC
							// exception instead.
							//
							try
							{
								receivingXdr.endDecoding();
							}
							catch (System.IO.IOException e)
							{
								throw (new OncRpcException(OncRpcException.RPC_CANTRECV
									, e.Message));
							}
						}
						// jmw 12/18/2009 fix for refresh/continue in Java code
						if (refreshFlag)
						{
							break;
						}
						//
						// We only reach this code part beyond the inner waiting
						// loop if we run in a timeout and might need to retransmit
						//
						// According to the retransmission strategy choosen, update the
						// current retransmission (resending) timeout.
						//
						if (retransmissionMode == org.acplt.oncrpc.OncRpcUdpRetransmissionMode.EXPONENTIAL)
						{
							resendTimeout *= 2;
						}
					}
					while (DateTime.Now.Ticks/TimeSpan.TicksPerMillisecond < stopTime);
					// jmw 12/18/2009 don't want to throw an exception here
					if (refreshFlag)
					{
						continue;
					}
					//
					// That's it -- this shitty server does not talk to us. Now, due to
					// the indecent language used in the previous sentence, this software
					// can not be exported any longer to some countries of the world.
					// But this is surely not my problem, but rather theirs. So go away
					// and hide yourself in the dark with all your zombies (or maybe
					// kangaroos).
					//
					throw (new OncRpcTimeoutException());
				}
			}
		}

		// for ( refreshesLeft )
		/// <summary>Broadcast a remote procedure call to several ONC/RPC servers.</summary>
		/// <remarks>
		/// Broadcast a remote procedure call to several ONC/RPC servers. For this
		/// you'll need to specify either a multicast address or the subnet's
		/// broadcast address when creating a <code>OncRpcUdpClient</code>. For
		/// every reply received, an event containing the reply is sent to the
		/// OncRpcBroadcastListener <code>listener</code>, which is the last
		/// parameter to the this method.
		/// <p>In contrast to the
		/// <see cref="OncRpcClient.call(int, XdrAble, XdrAble)">OncRpcClient.call(int, XdrAble, XdrAble)
		/// 	</see>
		/// method,
		/// <code>broadcastCall</code> will only send the ONC/RPC call once. It
		/// will then wait for answers until the timeout as set by
		/// <see cref="OncRpcClient.setTimeout(int)">OncRpcClient.setTimeout(int)</see>
		/// expires without resending the reply.
		/// <p>Note that you might experience unwanted results when using
		/// authentication types other than
		/// <see cref="OncRpcClientAuthNone">OncRpcClientAuthNone</see>
		/// , causing
		/// messed up authentication protocol handling objects. This depends on
		/// the type of authentication used. For <code>AUTH_UNIX</code> nothing
		/// bad happens as long as none of the servers replies with a shorthand
		/// verifier. If it does, then this shorthand will be used on all subsequent
		/// ONC/RPC calls, something you probably do not want at all.
		/// </remarks>
		/// <param name="procedureNumber">Procedure number of the procedure to call.</param>
		/// <param name="params">
		/// The parameters of the procedure to call, contained
		/// in an object which implements the
		/// <see cref="XdrAble">XdrAble</see>
		/// interface.
		/// </param>
		/// <param name="result">
		/// The object receiving the result of the procedure call.
		/// Note that this object is reused to deserialize all incomming replies
		/// one after another.
		/// </param>
		/// <param name="listener">
		/// Listener which will get an
		/// <see cref="OncRpcBroadcastEvent">OncRpcBroadcastEvent</see>
		/// for every reply received.
		/// </param>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public virtual void broadcastCall(int procedureNumber, org.acplt.oncrpc.XdrAble @params
			, org.acplt.oncrpc.XdrAble result, org.acplt.oncrpc.OncRpcBroadcastListener listener
			)
		{
			lock (this)
			{
				//
				// First, build the ONC/RPC call header. Then put the sending
				// stream into a known state and encode the parameters to be
				// sent. Finally tell the encoding stream to broadcast all its data.
				// Then wait for answers, receive and decode them one at a time.
				//
				nextXid();
				org.acplt.oncrpc.OncRpcClientCallMessage callHeader = new org.acplt.oncrpc.OncRpcClientCallMessage
					(xid, program, version, procedureNumber, auth);
				org.acplt.oncrpc.OncRpcClientReplyMessage replyHeader = new org.acplt.oncrpc.OncRpcClientReplyMessage
					(auth);
				//
				// Broadcast the call. Note that we send the call only once and will
				// never resend it.
				//
				try
				{
					//
					// Send call message to server. Remember that we've already
					// "connected" the datagram socket, so java.net knows whom
					// to send the datagram packets.
					//
					sendingXdr.beginEncoding(host, port);
					callHeader.xdrEncode(sendingXdr);
					@params.xdrEncode(sendingXdr);
					sendingXdr.endEncoding();
				}
				catch (System.IO.IOException e)
				{
					throw (new org.acplt.oncrpc.OncRpcException(org.acplt.oncrpc.OncRpcException.RPC_CANTSEND
						, e.Message));
				}
				//
				// Now enter the great loop where sit waiting for replies to our
				// broadcast call to come in. In every case, we wait until the
				// (total) timeout expires.
				//
				DateTime stopTime = DateTime.Now + new TimeSpan(timeout);
				do
				{
					try
					{
						//
						// Calculate timeout until the total timeout is reached, so
						// we can try to meet the overall deadline.
						//
						TimeSpan currentTimeout = (stopTime - DateTime.Now);
						if (currentTimeout.Ticks < 0)
						{
                            currentTimeout = new TimeSpan(0);
						}
                        socket.ReceiveTimeout = currentTimeout.Seconds;
						//
						// Then wait for datagrams to arrive...
						//
						receivingXdr.beginDecoding();
						replyHeader.xdrDecode(receivingXdr);
						//
						// Only deserialize the result, if the reply matches the call
						// and if the reply signals a successful call. In case of an
						// unsuccessful call (which mathes our call nevertheless) throw
						// an exception.
						//
						if (replyHeader.messageId == callHeader.messageId)
						{
							if (!replyHeader.successfullyAccepted())
							{
								//
								// We got a notification of a rejected call. We silently
								// ignore such replies and continue listening for other
								// replies.
								//
								receivingXdr.endDecoding();
							}
							result.xdrDecode(receivingXdr);
							//
							// Notify a potential listener of the reply.
							//
							if (listener != null)
							{
								org.acplt.oncrpc.OncRpcBroadcastEvent evt = new org.acplt.oncrpc.OncRpcBroadcastEvent
									(this, receivingXdr.getSenderAddress(), procedureNumber, @params, result);
								listener.replyReceived(evt);
							}
							//
							// Free pending resources of buffer and exit the call loop,
							// returning the reply to the caller through the result
							// object.
							//
							receivingXdr.endDecoding();
						}
						else
						{
							//
							// This should raise no exceptions, when skipping the UDP
							// record. So if one is raised, we will rethrow an ONC/RPC
							// exception instead.
							//
							try
							{
								receivingXdr.endDecoding();
							}
							catch (System.IO.IOException e)
							{
								throw (new org.acplt.oncrpc.OncRpcException(org.acplt.oncrpc.OncRpcException.RPC_CANTRECV
									, e.Message));
							}
						}
					}
					catch (SocketException)
					{
					}
					catch (System.IO.IOException e)
					{
						//
						// Note that we only catch timeouts here, but no other
						// exceptions. Those others will go up further until someone
						// catches them. If we get the timeout we know that it
						// could be time to leave the stage and so we fall through
						// to the total timeout check.
						//
						//
						// Argh. Trouble with the transport. Seems like we can't
						// receive data. Gosh. Go away!
						//
						throw (new org.acplt.oncrpc.OncRpcException(org.acplt.oncrpc.OncRpcException.RPC_CANTRECV
							, e.Message));
					}
				}
				while (DateTime.Now < stopTime);
				return;
			}
		}

		/// <summary>
		/// Set the
		/// <see cref="OncRpcUdpRetransmissionMode">retransmission mode</see>
		/// for
		/// lost remote procedure calls. The default retransmission mode is
		/// <see cref="OncRpcUdpRetransmissionMode.FIXED">OncRpcUdpRetransmissionMode.FIXED</see>
		/// .
		/// </summary>
		/// <param name="mode">Retransmission mode (either fixed or exponential).</param>
		public virtual void setRetransmissionMode(int mode)
		{
			retransmissionMode = mode;
		}

		/// <summary>
		/// Retrieve the current
		/// <see cref="OncRpcUdpRetransmissionMode">retransmission mode</see>
		/// set for
		/// retransmission of lost ONC/RPC calls.
		/// </summary>
		/// <returns>Current retransmission mode.</returns>
		public virtual int getRetransmissionMode()
		{
			return retransmissionMode;
		}

		/// <summary>
		/// Set the retransmission timout for remote procedure calls to wait for
		/// an answer from the ONC/RPC server before resending the call.
		/// </summary>
		/// <remarks>
		/// Set the retransmission timout for remote procedure calls to wait for
		/// an answer from the ONC/RPC server before resending the call. The
		/// default retransmission timeout is the 30 seconds. The retransmission
		/// timeout must be &gt; 0. To disable retransmission of lost calls, set
		/// the retransmission timeout to be the same value as the timeout.
		/// </remarks>
		/// <param name="milliseconds">
		/// Timeout in milliseconds. A timeout of zero indicates
		/// batched calls.
		/// </param>
		public virtual void setRetransmissionTimeout(int milliseconds)
		{
			if (milliseconds <= 0)
			{
				throw (new System.ArgumentException("timeouts must be positive."));
			}
			retransmissionTimeout = milliseconds;
		}

		/// <summary>
		/// Retrieve the current retransmission timeout set for remote procedure
		/// calls.
		/// </summary>
		/// <remarks>
		/// Retrieve the current retransmission timeout set for remote procedure
		/// calls.
		/// </remarks>
		/// <returns>Current retransmission timeout.</returns>
		public virtual int getRetransmissionTimeout()
		{
			return retransmissionTimeout;
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
		/// UDP socket used for datagram-based communication with an ONC/RPC
		/// server.
		/// </summary>
		/// <remarks>
		/// UDP socket used for datagram-based communication with an ONC/RPC
		/// server.
		/// </remarks>
        private Socket socket;
        
		/// <summary>
		/// XDR encoding stream used for sending requests via UDP/IP to an ONC/RPC
		/// server.
		/// </summary>
		/// <remarks>
		/// XDR encoding stream used for sending requests via UDP/IP to an ONC/RPC
		/// server.
		/// </remarks>
		internal org.acplt.oncrpc.XdrUdpEncodingStream sendingXdr;

		/// <summary>
		/// XDR decoding stream used when receiving replies via UDP/IP from an
		/// ONC/RPC server.
		/// </summary>
		/// <remarks>
		/// XDR decoding stream used when receiving replies via UDP/IP from an
		/// ONC/RPC server.
		/// </remarks>
		internal org.acplt.oncrpc.XdrUdpDecodingStream receivingXdr;

		/// <summary>
		/// Retransmission timeout used for resending ONC/RPC calls when an ONC/RPC
		/// server does not answer fast enough.
		/// </summary>
		/// <remarks>
		/// Retransmission timeout used for resending ONC/RPC calls when an ONC/RPC
		/// server does not answer fast enough. The default retransmission timeout
		/// is identical to the overall timeout for ONC/RPC calls (thus UDP/IP-based
		/// clients will not retransmit lost calls).
		/// </remarks>
		/// <seealso cref="retransmissionMode">retransmissionMode</seealso>
		/// <seealso cref="OncRpcClient.setTimeout(int)">OncRpcClient.setTimeout(int)</seealso>
		internal int retransmissionTimeout;

		/// <summary>Retransmission mode used when resending ONC/RPC calls.</summary>
		/// <remarks>
		/// Retransmission mode used when resending ONC/RPC calls. Default mode is
		/// <see cref="OncRpcUdpRetransmissionMode.FIXED">fixed timeout mode</see>
		/// .
		/// </remarks>
		/// <seealso cref="OncRpcUdpRetransmissionMode">OncRpcUdpRetransmissionMode</seealso>
		internal int retransmissionMode = org.acplt.oncrpc.OncRpcUdpRetransmissionMode.FIXED;
	}
}
