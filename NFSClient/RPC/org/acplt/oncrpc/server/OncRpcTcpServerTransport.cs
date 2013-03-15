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
using System.Threading;
using System.Net.Sockets;
using System;
namespace org.acplt.oncrpc.server
{
	/// <summary>
	/// Instances of class <code>OncRpcTcpServerTransport</code> encapsulate
	/// TCP/IP-based XDR streams of ONC/RPC servers.
	/// </summary>
	/// <remarks>
	/// Instances of class <code>OncRpcTcpServerTransport</code> encapsulate
	/// TCP/IP-based XDR streams of ONC/RPC servers. This server transport class
	/// is responsible for accepting new ONC/RPC connections over TCP/IP.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <seealso cref="OncRpcServerTransport">OncRpcServerTransport</seealso>
	/// <seealso cref="OncRpcTcpConnectionServerTransport">OncRpcTcpConnectionServerTransport
	/// 	</seealso>
	/// <seealso cref="OncRpcUdpServerTransport">OncRpcUdpServerTransport</seealso>
	/// <version>$Revision: 1.3 $ $Date: 2008/01/02 15:13:35 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class OncRpcTcpServerTransport : OncRpcServerTransport
	{
		/// <summary>
		/// Create a new instance of a <code>OncRpcTcpServerTransport</code> which
		/// encapsulates TCP/IP-based XDR streams of an ONC/RPC server.
		/// </summary>
		/// <remarks>
		/// Create a new instance of a <code>OncRpcTcpServerTransport</code> which
		/// encapsulates TCP/IP-based XDR streams of an ONC/RPC server. This
		/// particular server transport only waits for incoming connection requests
		/// and then creates
		/// <see cref="OncRpcTcpConnectionServerTransport">OncRpcTcpConnectionServerTransport
		/// 	</see>
		/// server transports
		/// to handle individual connections.
		/// This constructor is a convenience constructor for those transports
		/// handling only a single ONC/RPC program and version number.
		/// </remarks>
		/// <param name="dispatcher">
		/// Reference to interface of an object capable of
		/// dispatching (handling) ONC/RPC calls.
		/// </param>
		/// <param name="port">
		/// Number of port where the server will wait for incoming
		/// calls.
		/// </param>
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
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		public OncRpcTcpServerTransport(org.acplt.oncrpc.server.OncRpcDispatchable dispatcher
			, int port, int program, int version, int bufferSize) : this(dispatcher, port, new 
			org.acplt.oncrpc.server.OncRpcServerTransportRegistrationInfo[] { new org.acplt.oncrpc.server.OncRpcServerTransportRegistrationInfo
			(program, version) }, bufferSize)
		{
			openTransports = new org.acplt.oncrpc.server.OncRpcTcpServerTransport.TransportList
				(this);
		}

		/// <summary>
		/// Create a new instance of a <code>OncRpcTcpServerTransport</code> which
		/// encapsulates TCP/IP-based XDR streams of an ONC/RPC server.
		/// </summary>
		/// <remarks>
		/// Create a new instance of a <code>OncRpcTcpServerTransport</code> which
		/// encapsulates TCP/IP-based XDR streams of an ONC/RPC server. This
		/// particular server transport only waits for incoming connection requests
		/// and then creates
		/// <see cref="OncRpcTcpConnectionServerTransport">OncRpcTcpConnectionServerTransport
		/// 	</see>
		/// server transports
		/// to handle individual connections.
		/// </remarks>
		/// <param name="dispatcher">
		/// Reference to interface of an object capable of
		/// dispatching (handling) ONC/RPC calls.
		/// </param>
		/// <param name="port">
		/// Number of port where the server will wait for incoming
		/// calls.
		/// </param>
		/// <param name="info">
		/// Array of program and version number tuples of the ONC/RPC
		/// programs and versions handled by this transport.
		/// </param>
		/// <param name="bufferSize">
		/// Size of buffer used when receiving and sending
		/// chunks of XDR fragments over TCP/IP. The fragments built up to
		/// form ONC/RPC call and reply messages.
		/// </param>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		public OncRpcTcpServerTransport(org.acplt.oncrpc.server.OncRpcDispatchable dispatcher
			, int port, org.acplt.oncrpc.server.OncRpcServerTransportRegistrationInfo[] info
			, int bufferSize) : this(dispatcher, null, port, info, bufferSize)
		{
			openTransports = new org.acplt.oncrpc.server.OncRpcTcpServerTransport.TransportList
				(this);
		}

		/// <summary>
		/// Create a new instance of a <code>OncRpcTcpServerTransport</code> which
		/// encapsulates TCP/IP-based XDR streams of an ONC/RPC server.
		/// </summary>
		/// <remarks>
		/// Create a new instance of a <code>OncRpcTcpServerTransport</code> which
		/// encapsulates TCP/IP-based XDR streams of an ONC/RPC server. This
		/// particular server transport only waits for incoming connection requests
		/// and then creates
		/// <see cref="OncRpcTcpConnectionServerTransport">OncRpcTcpConnectionServerTransport
		/// 	</see>
		/// server transports
		/// to handle individual connections.
		/// </remarks>
		/// <param name="dispatcher">
		/// Reference to interface of an object capable of
		/// dispatching (handling) ONC/RPC calls.
		/// </param>
		/// <param name="bindAddr">The local Internet Address the server will bind to.</param>
		/// <param name="port">
		/// Number of port where the server will wait for incoming
		/// calls.
		/// </param>
		/// <param name="info">
		/// Array of program and version number tuples of the ONC/RPC
		/// programs and versions handled by this transport.
		/// </param>
		/// <param name="bufferSize">
		/// Size of buffer used when receiving and sending
		/// chunks of XDR fragments over TCP/IP. The fragments built up to
		/// form ONC/RPC call and reply messages.
		/// </param>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		public OncRpcTcpServerTransport(org.acplt.oncrpc.server.OncRpcDispatchable dispatcher
			, IPAddress bindAddr, int port, org.acplt.oncrpc.server.OncRpcServerTransportRegistrationInfo
			[] info, int bufferSize) : base(dispatcher, port, info)
		{
			openTransports = new org.acplt.oncrpc.server.OncRpcTcpServerTransport.TransportList
				(this);
			//
			// Make sure the buffer is large enough and resize system buffers
			// accordingly, if possible.
			//
			if (bufferSize < 1024)
			{
				bufferSize = 1024;
			}
			this.bufferSize = bufferSize;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (bindAddr == null) bindAddr = IPAddress.Any;
            IPEndPoint localEP = new IPEndPoint(bindAddr, port);
            socket.Bind(localEP);
			if (port == 0)
			{
				this.port = ((IPEndPoint)socket.LocalEndPoint).Port;
			}
            socket.Listen(0);
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
		/// In addition, all server transports handling the individual TCP/IP
		/// connections will also be closed. The handler threads will therefore
		/// either terminate directly or when they try to sent back replies.
		/// </remarks>
		public override void Close()
		{
			if (socket != null)
			{
				//
				// Since there is a non-zero chance of getting race conditions,
				// we now first set the socket instance member to null, before
				// we close the corresponding socket. This avoids null-pointer
				// exceptions in the method which waits for connections: it is
				// possible that that method is awakened because the socket has
				// been closed before we could set the socket instance member to
				// null. Many thanks to Michael Smith for tracking down this one.
				//
				Socket deadSocket = socket;
				socket = null;
                try {
				deadSocket.Close();
				}
				catch (Exception e)
				{
                    Console.Out.WriteLine(e.Message);
                    Console.Out.WriteLine(e.StackTrace);
				}
			}
			//
			// Now close all per-connection transports currently open...
			//
			lock (openTransports)
			{
				while (openTransports.size() > 0)
				{
					org.acplt.oncrpc.server.OncRpcTcpConnectionServerTransport transport = (org.acplt.oncrpc.server.OncRpcTcpConnectionServerTransport
						)openTransports.removeFirst();
					transport.Close();
				}
			}
		}

		/// <summary>
		/// Removes a TCP/IP server transport from the list of currently open
		/// transports.
		/// </summary>
		/// <remarks>
		/// Removes a TCP/IP server transport from the list of currently open
		/// transports.
		/// </remarks>
		/// <param name="transport">
		/// Server transport to remove from the list of currently
		/// open transports for this listening transport.
		/// </param>
		public virtual void removeTransport(org.acplt.oncrpc.server.OncRpcTcpConnectionServerTransport
			 transport)
		{
			lock (openTransports)
			{
				openTransports.remove((object)transport);
			}
		}

		/// <summary>
		/// Register the TCP/IP port where this server transport waits for incoming
		/// requests with the ONC/RPC portmapper.
		/// </summary>
		/// <remarks>
		/// Register the TCP/IP port where this server transport waits for incoming
		/// requests with the ONC/RPC portmapper.
		/// </remarks>
		/// <exception cref="org.acplt.oncrpc.OncRpcException">
		/// if the portmapper could not be contacted
		/// successfully of if the portmapper rejected port registration(s).
		/// </exception>
		public override void register()
		{
			try
			{
				org.acplt.oncrpc.OncRpcPortmapClient portmapper = new org.acplt.oncrpc.OncRpcPortmapClient
					(IPAddress.Loopback);
				int size = info.Length;
				for (int idx = 0; idx < size; ++idx)
				{
					//
					// Try to register the port for our transport with the local ONC/RPC
					// portmapper. If this fails, bail out with an exception.
					//
					if (!portmapper.setPort(info[idx].program, info[idx].version, org.acplt.oncrpc.OncRpcProtocols
						.ONCRPC_TCP, port))
					{
						throw (new org.acplt.oncrpc.OncRpcException(org.acplt.oncrpc.OncRpcException.RPC_CANNOTREGISTER
							));
					}
				}
			}
			catch (System.IO.IOException)
			{
				throw (new org.acplt.oncrpc.OncRpcException(org.acplt.oncrpc.OncRpcException.RPC_FAILED
					));
			}
		}

		/// <summary>Do not call.</summary>
		/// <remarks>Do not call.</remarks>
		/// <exception cref="System.Exception">
		/// because this method must not be called for a listening
		/// server transport.
		/// </exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		internal override void retrieveCall(org.acplt.oncrpc.XdrAble call)
		{
			throw (new System.Exception("OncRpcTcpServerTransport.retrieveCall() is abstract "
				 + "and can not be called."));
		}

		/// <summary>Do not call.</summary>
		/// <remarks>Do not call.</remarks>
		/// <exception cref="System.Exception">
		/// because this method must not be called for a listening
		/// server transport.
		/// </exception>
		internal override org.acplt.oncrpc.XdrDecodingStream getXdrDecodingStream()
		{
			throw (new System.Exception("OncRpcTcpServerTransport.getXdrDecodingStream() is abstract "
				 + "and can not be called."));
		}

		/// <summary>Do not call.</summary>
		/// <remarks>Do not call.</remarks>
		/// <exception cref="System.Exception">
		/// because this method must not be called for a listening
		/// server transport.
		/// </exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		internal override void endDecoding()
		{
			throw (new System.Exception("OncRpcTcpServerTransport.endDecoding() is abstract "
				 + "and can not be called."));
		}

		/// <summary>Do not call.</summary>
		/// <remarks>Do not call.</remarks>
		/// <exception cref="System.Exception">
		/// because this method must not be called for a listening
		/// server transport.
		/// </exception>
		internal override org.acplt.oncrpc.XdrEncodingStream getXdrEncodingStream()
		{
			throw (new System.Exception("OncRpcTcpServerTransport.getXdrEncodingStream() is abstract "
				 + "and can not be called."));
		}

		/// <summary>Do not call.</summary>
		/// <remarks>Do not call.</remarks>
		/// <exception cref="System.Exception">
		/// because this method must not be called for a listening
		/// server transport.
		/// </exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		internal override void beginEncoding(org.acplt.oncrpc.server.OncRpcCallInformation
			 callInfo, org.acplt.oncrpc.server.OncRpcServerReplyMessage state)
		{
			throw (new System.Exception("OncRpcTcpServerTransport.beginEncoding() is abstract "
				 + "and can not be called."));
		}

		/// <summary>Do not call.</summary>
		/// <remarks>Do not call.</remarks>
		/// <exception cref="System.Exception">
		/// because this method must not be called for a listening
		/// server transport.
		/// </exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		internal override void endEncoding()
		{
			throw (new System.Exception("OncRpcTcpServerTransport.endEncoding() is abstract "
				 + "and can not be called."));
		}

		/// <summary>Do not call.</summary>
		/// <remarks>Do not call.</remarks>
		/// <exception cref="System.Exception">
		/// because this method must not be called for a listening
		/// server transport.
		/// </exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		internal override void reply(org.acplt.oncrpc.server.OncRpcCallInformation callInfo
			, org.acplt.oncrpc.server.OncRpcServerReplyMessage state, org.acplt.oncrpc.XdrAble
			 reply)
		{
			throw (new System.Exception("OncRpcTcpServerTransport.reply() is abstract " + "and can not be called."
				));
		}

		/// <summary>
		/// Creates a new thread and uses this thread to listen to incoming
		/// ONC/RPC requests, then dispatches them and finally sends back the
		/// appropriate reply messages.
		/// </summary>
		/// <remarks>
		/// Creates a new thread and uses this thread to listen to incoming
		/// ONC/RPC requests, then dispatches them and finally sends back the
		/// appropriate reply messages. Control in the calling thread immediately
		/// returns after the handler thread has been created.
		/// <p>For every incomming TCP/IP connection a handler thread is created
		/// to handle ONC/RPC calls on this particular connection.
		/// </remarks>
		public override void listen()
		{
			//
			// Create a new (daemon) thread which will handle incoming connection
			// requests.
			//
            _TransportHelper t = new _TransportHelper(this);
			Thread listenThread = new Thread(new ThreadStart(t.run));
            listenThread.Name = "TCP server transport listener thread";
			//
			// Now wait for (new) connection requests to come in.
			//
			//
			// Let the newly created transport object handle this
			// connection. Note that it will create its own
			// thread for handling.
			//
			//
			// We are just ignoring most of the IOExceptions as
			// they might be thrown, for instance, if a client
			// attempts a connection and resets it before it is
			// pulled off by accept(). If the socket has been
			// gone away after an IOException this means that the
			// transport has been closed, so we end this thread
			// gracefully.
			//
            listenThread.Start();
        }

		private sealed class _TransportHelper
		{
			public _TransportHelper(OncRpcTcpServerTransport _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void run()
			{
				for (; ; )
				{
					try
					{
						Socket myServerSocket = this._enclosing.socket;
						if (myServerSocket == null)
						{
							break;
						}
                        Socket newSocket = myServerSocket.Accept();
						OncRpcTcpConnectionServerTransport transport = new OncRpcTcpConnectionServerTransport
							(this._enclosing.dispatcher, newSocket, this._enclosing.info, this._enclosing.bufferSize
							, this._enclosing, this._enclosing.transmissionTimeout);
						lock (this._enclosing.openTransports)
						{
							this._enclosing.openTransports.add((object)transport);
						}
						transport.listen();
					}
					catch (org.acplt.oncrpc.OncRpcException)
					{
					}
					catch (SocketException)
					{
                        // If the socket has been closed and set to null, don't bother
                        // notifying anybody because we're shutting down
						if (this._enclosing.socket == null)
						{
							break;
						}
					}
				}
			}

			private readonly OncRpcTcpServerTransport _enclosing;
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
			this.characterEncoding = characterEncoding;
		}

		/// <summary>Get the character encoding for (de-)serializing strings.</summary>
		/// <remarks>Get the character encoding for (de-)serializing strings.</remarks>
		/// <returns>
		/// the encoding currently used for (de-)serializing strings.
		/// If <code>null</code>, then the system's default encoding is used.
		/// </returns>
		public override string getCharacterEncoding()
		{
			return characterEncoding;
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

		/// <summary>Size of send/receive buffers to use when encoding/decoding XDR data.</summary>
		/// <remarks>Size of send/receive buffers to use when encoding/decoding XDR data.</remarks>
		private int bufferSize;

		/// <summary>Collection containing currently open transports.</summary>
		/// <remarks>Collection containing currently open transports.</remarks>
		private org.acplt.oncrpc.server.OncRpcTcpServerTransport.TransportList openTransports;

		/// <summary>
		/// Timeout during the phase where data is received within calls, or data is
		/// sent within replies.
		/// </summary>
		/// <remarks>
		/// Timeout during the phase where data is received within calls, or data is
		/// sent within replies.
		/// </remarks>
		internal int transmissionTimeout = 30000;

		/// <summary>
		/// Encoding to use when deserializing strings or <code>null</code> if
		/// the system's default encoding should be used.
		/// </summary>
		/// <remarks>
		/// Encoding to use when deserializing strings or <code>null</code> if
		/// the system's default encoding should be used.
		/// </remarks>
		private string characterEncoding = null;

		/// <summary>
		/// Minumum implementation of a double linked list which notices which
		/// transports are currently open and have to be shut down when this
		/// listening transport is shut down.
		/// </summary>
		/// <remarks>
		/// Minumum implementation of a double linked list which notices which
		/// transports are currently open and have to be shut down when this
		/// listening transport is shut down. The only reason why we have this
		/// code here instead of using java.util.LinkedList is due to JDK&nbsp;1.1
		/// compatibility.
		/// <p>Note that the methods are not synchronized as we leave this up
		/// to the caller, who can thus optimize access during critical sections.
		/// </remarks>
		private class TransportList
		{
			/// <summary>Create a new instance of a list of open transports.</summary>
			/// <remarks>Create a new instance of a list of open transports.</remarks>
			public TransportList(OncRpcTcpServerTransport _enclosing)
			{
				this._enclosing = _enclosing;
				head = new org.acplt.oncrpc.server.OncRpcTcpServerTransport.TransportList.Node(this
					, null);
				//
				// Link header node with itself, so it is its own successor
				// and predecessor. Using a header node excuses us from checking
				// for the special cases of first and last node (or both at
				// the same time).
				//
				this.head.next = this.head;
				this.head.prev = this.head;
			}

			/// <summary>Add new transport to list of open transports.</summary>
			/// <remarks>
			/// Add new transport to list of open transports. The new transport
			/// is always added immediately after the head of the linked list.
			/// </remarks>
			public virtual void add(object o)
			{
				org.acplt.oncrpc.server.OncRpcTcpServerTransport.TransportList.Node node = new org.acplt.oncrpc.server.OncRpcTcpServerTransport.TransportList.Node
					(this, o);
				node.next = this.head.next;
				this.head.next = node;
				node.prev = this.head;
				node.next.prev = node;
				++this._size;
			}

			/// <summary>Remove given transport from list of open transports.</summary>
			/// <remarks>Remove given transport from list of open transports.</remarks>
			public virtual bool remove(object o)
			{
				org.acplt.oncrpc.server.OncRpcTcpServerTransport.TransportList.Node node = this.head
					.next;
				while (node != this.head)
				{
					if (node.item == o)
					{
						node.prev.next = node.next;
						node.next.prev = node.prev;
						--this._size;
						return true;
					}
					node = node.next;
				}
				return false;
			}

			/// <summary>Removes and returns the first open transport from list.</summary>
			/// <remarks>Removes and returns the first open transport from list.</remarks>
			public virtual object removeFirst()
			{
				//
				// Do not remove the header node.
				//
				if (this._size == 0)
				{
					throw (new System.ArgumentOutOfRangeException());
				}
				org.acplt.oncrpc.server.OncRpcTcpServerTransport.TransportList.Node node = this.head
					.next;
				this.head.next = node.next;
				node.next.prev = this.head;
				--this._size;
				return node.item;
			}

			/// <summary>Returns the number of (open) transports in this list.</summary>
			/// <remarks>Returns the number of (open) transports in this list.</remarks>
			/// <returns>the number of (open) transports.</returns>
			public virtual int size()
			{
				return this._size;
			}

			/// <summary>
			/// Head node for list of open transports which does not represent
			/// an open transport but instead excuses us of dealing with all
			/// the special cases of real nodes at the begin or end of the list.
			/// </summary>
			/// <remarks>
			/// Head node for list of open transports which does not represent
			/// an open transport but instead excuses us of dealing with all
			/// the special cases of real nodes at the begin or end of the list.
			/// </remarks>
			private org.acplt.oncrpc.server.OncRpcTcpServerTransport.TransportList.Node head;

			/// <summary>
			/// Number of (real) open transports currently registered in this
			/// list.
			/// </summary>
			/// <remarks>
			/// Number of (real) open transports currently registered in this
			/// list.
			/// </remarks>
			private int _size = 0;

			/// <summary>
			/// Node class referencing an individual open transport and holding
			/// references to the previous and next open transports.
			/// </summary>
			/// <remarks>
			/// Node class referencing an individual open transport and holding
			/// references to the previous and next open transports.
			/// </remarks>
			private class Node
			{
				/// <summary>
				/// Create a new instance of a node object and let it reference
				/// an open transport.
				/// </summary>
				/// <remarks>
				/// Create a new instance of a node object and let it reference
				/// an open transport. The creator of this object is then
				/// responsible for adding this node to the circular list itself.
				/// </remarks>
				public Node(TransportList _enclosing, object item)
				{
					this._enclosing = _enclosing;
					this.item = item;
				}

				/// <summary>
				/// Next item node (in other words: next open transport)
				/// in the list.
				/// </summary>
				/// <remarks>
				/// Next item node (in other words: next open transport)
				/// in the list. This will never be <code>null</code> for the
				/// first item, but instead reference the last item. Thus, the
				/// list is circular.
				/// </remarks>
				internal org.acplt.oncrpc.server.OncRpcTcpServerTransport.TransportList.Node next;

				/// <summary>
				/// Previous item node (in other words: previous open transport)
				/// in the list.
				/// </summary>
				/// <remarks>
				/// Previous item node (in other words: previous open transport)
				/// in the list. This will never be <code>null</code> for the
				/// last item, but instead reference the first item. Thus, the
				/// list is circular.
				/// </remarks>
				internal org.acplt.oncrpc.server.OncRpcTcpServerTransport.TransportList.Node prev;

				/// <summary>The item/object placed at this position in the list.</summary>
				/// <remarks>
				/// The item/object placed at this position in the list. This
				/// currently always references an open transport.
				/// </remarks>
				internal object item;

				private readonly TransportList _enclosing;
			}

			private readonly OncRpcTcpServerTransport _enclosing;
		}
	}
}
