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
namespace org.acplt.oncrpc.server
{
	/// <summary>
	/// Instances of class <code>OncRpcServerTransport</code> encapsulate XDR
	/// streams of ONC/RPC servers.
	/// </summary>
	/// <remarks>
	/// Instances of class <code>OncRpcServerTransport</code> encapsulate XDR
	/// streams of ONC/RPC servers. Using server transports, ONC/RPC calls are
	/// received and the corresponding replies are later sent back after
	/// handling.
	/// <p>Note that the server-specific dispatcher handling requests
	/// (done through
	/// <see cref="OncRpcDispatchable">OncRpcDispatchable</see>
	/// will only
	/// directly deal with
	/// <see cref="OncRpcCallInformation">OncRpcCallInformation</see>
	/// objects. These
	/// call information objects reference OncRpcServerTransport object, but
	/// the server programmer typically will never touch them, as the call
	/// information object already contains all necessary information about
	/// a call, so replies can be sent back (and this is definetely a sentence
	/// containing too many words).
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <seealso cref="OncRpcCallInformation">OncRpcCallInformation</seealso>
	/// <seealso cref="OncRpcDispatchable">OncRpcDispatchable</seealso>
	/// <version>$Revision: 1.3 $ $Date: 2003/08/14 13:47:04 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public abstract class OncRpcServerTransport
	{
		/// <summary>
		/// Create a new instance of a <code>OncRpcServerTransport</code> which
		/// encapsulates XDR streams of an ONC/RPC server.
		/// </summary>
		/// <remarks>
		/// Create a new instance of a <code>OncRpcServerTransport</code> which
		/// encapsulates XDR streams of an ONC/RPC server. Using a server transport,
		/// ONC/RPC calls are received and the corresponding replies are sent back.
		/// <p>We do not create any XDR streams here, as it is the responsibility
		/// of derived classes to create appropriate XDR stream objects for the
		/// respective kind of transport mechanism used (like TCP/IP and UDP/IP).
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
		internal OncRpcServerTransport(org.acplt.oncrpc.server.OncRpcDispatchable dispatcher
			, int port, org.acplt.oncrpc.server.OncRpcServerTransportRegistrationInfo[] info
			)
		{
			this.dispatcher = dispatcher;
			this.port = port;
			this.info = info;
		}

		/// <summary>
		/// Register the port where this server transport waits for incoming
		/// requests with the ONC/RPC portmapper.
		/// </summary>
		/// <remarks>
		/// Register the port where this server transport waits for incoming
		/// requests with the ONC/RPC portmapper.
		/// <p>The contract of this method is, that derived classes implement
		/// the appropriate communication with the portmapper, so the transport
		/// is registered only for the protocol supported by a particular kind
		/// of server transport.
		/// </remarks>
		/// <exception cref="org.acplt.oncrpc.OncRpcException">
		/// if the portmapper could not be contacted
		/// successfully.
		/// </exception>
		public abstract void register();

		/// <summary>
		/// Unregisters the port where this server transport waits for incoming
		/// requests from the ONC/RPC portmapper.
		/// </summary>
		/// <remarks>
		/// Unregisters the port where this server transport waits for incoming
		/// requests from the ONC/RPC portmapper.
		/// <p>Note that due to the way Sun decided to implement its ONC/RPC
		/// portmapper process, deregistering one server transports causes all
		/// entries for the same program and version to be removed, regardless
		/// of the protocol (UDP/IP or TCP/IP) used. Sigh.
		/// </remarks>
		/// <exception cref="org.acplt.oncrpc.OncRpcException">
		/// with a reason of
		/// <see cref="org.acplt.oncrpc.OncRpcException.RPC_FAILED">OncRpcException.RPC_FAILED
		/// 	</see>
		/// if
		/// the portmapper could not be contacted successfully. Note that
		/// it is not considered an error to remove a non-existing entry from
		/// the portmapper.
		/// </exception>
		public virtual void unregister()
		{
			try
			{
				org.acplt.oncrpc.OncRpcPortmapClient portmapper = new org.acplt.oncrpc.OncRpcPortmapClient
					(IPAddress.Loopback);
				int size = info.Length;
				for (int idx = 0; idx < size; ++idx)
				{
					portmapper.unsetPort(info[idx].program, info[idx].version);
				}
			}
			catch (System.IO.IOException)
			{
				throw (new OncRpcException(OncRpcException.RPC_FAILED
					));
			}
		}

		/// <summary>Close the server transport and free any resources associated with it.</summary>
		/// <remarks>
		/// Close the server transport and free any resources associated with it.
		/// <p>Note that the server transport is <b>not deregistered</b>. You'll
		/// have to do it manually if you need to do so. The reason for this
		/// behaviour is, that the portmapper removes all entries regardless of
		/// the protocol (TCP/IP or UDP/IP) for a given ONC/RPC program number
		/// and version.
		/// <p>Derived classes can choose between different behaviour for
		/// shuting down the associated transport handler threads:
		/// <ul>
		/// <li>Close the transport immediately and let the threads stumble on the
		/// closed network connection.
		/// <li>Wait for handler threads to complete their current ONC/RPC request
		/// (with timeout), then close connections and kill the threads.
		/// </ul>
		/// </remarks>
		public abstract void Close();

		/// <summary>
		/// Creates a new thread and uses this thread to listen to incoming
		/// ONC/RPC requests, then dispatches them and finally sends back the
		/// appropriate reply messages.
		/// </summary>
		/// <remarks>
		/// Creates a new thread and uses this thread to listen to incoming
		/// ONC/RPC requests, then dispatches them and finally sends back the
		/// appropriate reply messages.
		/// <p>Note that you have to supply an implementation for this abstract
		/// method in derived classes. Your implementation needs to create a new
		/// thread to wait for incoming requests. The method has to return
		/// immediately for the calling thread.
		/// </remarks>
		public abstract void listen();

		/// <summary>
		/// Returns port number of socket this server transport listens on for
		/// incoming ONC/RPC calls.
		/// </summary>
		/// <remarks>
		/// Returns port number of socket this server transport listens on for
		/// incoming ONC/RPC calls.
		/// </remarks>
		/// <returns>Port number of socket listening for incoming calls.</returns>
		public virtual int getPort()
		{
			return port;
		}

		/// <summary>Set the character encoding for (de-)serializing strings.</summary>
		/// <remarks>Set the character encoding for (de-)serializing strings.</remarks>
		/// <param name="characterEncoding">
		/// the encoding to use for (de-)serializing strings.
		/// If <code>null</code>, the system's default encoding is to be used.
		/// </param>
		public abstract void setCharacterEncoding(string characterEncoding);

		/// <summary>Get the character encoding for (de-)serializing strings.</summary>
		/// <remarks>Get the character encoding for (de-)serializing strings.</remarks>
		/// <returns>
		/// the encoding currently used for (de-)serializing strings.
		/// If <code>null</code>, then the system's default encoding is used.
		/// </returns>
		public abstract string getCharacterEncoding();

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
		internal abstract void retrieveCall(org.acplt.oncrpc.XdrAble call);

		/// <summary>
		/// Returns XDR stream which can be used for deserializing the parameters
		/// of this ONC/RPC call.
		/// </summary>
		/// <remarks>
		/// Returns XDR stream which can be used for deserializing the parameters
		/// of this ONC/RPC call. This method belongs to the lower-level access
		/// pattern when handling ONC/RPC calls.
		/// </remarks>
		/// <returns>Reference to decoding XDR stream.</returns>
		internal abstract org.acplt.oncrpc.XdrDecodingStream getXdrDecodingStream();

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
		internal abstract void endDecoding();

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
		internal abstract org.acplt.oncrpc.XdrEncodingStream getXdrEncodingStream();

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
		internal abstract void beginEncoding(org.acplt.oncrpc.server.OncRpcCallInformation
			 callInfo, org.acplt.oncrpc.server.OncRpcServerReplyMessage state);

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
		internal abstract void endEncoding();

		/// <summary>Send back an ONC/RPC reply to the original caller.</summary>
		/// <remarks>
		/// Send back an ONC/RPC reply to the original caller. This is rather a
		/// low-level method, typically not used by applications. Dispatcher handling
		/// ONC/RPC calls have to use the
		/// <see cref="OncRpcCallInformation.reply(org.acplt.oncrpc.XdrAble)">OncRpcCallInformation.reply(org.acplt.oncrpc.XdrAble)
		/// 	</see>
		/// method instead on the
		/// call object supplied to the handler.
		/// <p>An appropriate implementation has to be provided in derived classes
		/// as it is dependent on the type of transport (whether UDP/IP or TCP/IP)
		/// used.
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
		internal abstract void reply(org.acplt.oncrpc.server.OncRpcCallInformation callInfo
			, org.acplt.oncrpc.server.OncRpcServerReplyMessage state, org.acplt.oncrpc.XdrAble
			 reply);

		/// <summary>
		/// Reference to interface of an object capable of handling/dispatching
		/// ONC/RPC requests.
		/// </summary>
		/// <remarks>
		/// Reference to interface of an object capable of handling/dispatching
		/// ONC/RPC requests.
		/// </remarks>
		internal org.acplt.oncrpc.server.OncRpcDispatchable dispatcher;

		/// <summary>Port number where we're listening for incoming ONC/RPC requests.</summary>
		/// <remarks>Port number where we're listening for incoming ONC/RPC requests.</remarks>
		internal int port;

		/// <summary>Program and version number tuples handled by this server transport.</summary>
		/// <remarks>Program and version number tuples handled by this server transport.</remarks>
		internal org.acplt.oncrpc.server.OncRpcServerTransportRegistrationInfo[] info;
	}
}
