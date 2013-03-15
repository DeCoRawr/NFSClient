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
using System.Net;
using System.IO;
namespace org.acplt.oncrpc
{
	/// <summary>
	/// The <code>XdrTcpEncodingStream</code> class provides the necessary
	/// functionality to
	/// <see cref="XdrEncodingStream">XdrEncodingStream</see>
	/// to send XDR records to the
	/// network using the stream-oriented TCP/IP.
        /// Converted to C# using the db4o Sharpen tool.
	/// </summary>
	/// <version>$Revision: 1.2 $ $Date: 2003/08/14 11:07:39 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class XdrTcpEncodingStream : XdrEncodingStream
	{
		/// <summary>
		/// Construct a new <code>XdrTcpEncodingStream</code> object and associate
		/// it with the given <code>streamingSocket</code> for TCP/IP-based
		/// communication.
		/// </summary>
		/// <remarks>
		/// Construct a new <code>XdrTcpEncodingStream</code> object and associate
		/// it with the given <code>streamingSocket</code> for TCP/IP-based
		/// communication.
		/// </remarks>
		/// <param name="streamingSocket">Socket to which XDR data is sent.</param>
		/// <param name="bufferSize">
		/// Size of packet buffer for temporarily storing
		/// outgoing XDR data.
		/// </param>
		/// <exception cref="System.IO.IOException"></exception>
		public XdrTcpEncodingStream(Socket socket, int bufferSize)
		{
            this.socket = socket;
            stream = new NetworkStream(socket);
			//
			// If the given buffer size is too small, start with a more sensible
			// size. Next, if bufferSize is not a multiple of four, round it up to
			// the next multiple of four.
			//
			if (bufferSize < 1024)
			{
				bufferSize = 1024;
			}
			if ((bufferSize & 3) != 0)
			{
				bufferSize = (bufferSize + 4) & ~3;
			}
			//
			// Set up the buffer and the buffer pointers (no, this is still
			// Java).
			//
			buffer = new byte[bufferSize];
			bufferFragmentHeaderIndex = 0;
			bufferIndex = 4;
			bufferHighmark = bufferSize - 4;
		}

		/// <summary>Returns the Internet address of the sender of the current XDR data.</summary>
		/// <remarks>
		/// Returns the Internet address of the sender of the current XDR data.
		/// This method should only be called after
		/// <see cref="beginEncoding(IPAddress, int)">beginEncoding(IPAddress, int)
		/// 	</see>
		/// ,
		/// otherwise it might return stale information.
		/// </remarks>
		/// <returns>InetAddress of the sender of the current XDR data.</returns>
		public virtual IPAddress getSenderAddress()
		{
            return ((IPEndPoint)(socket.RemoteEndPoint)).Address;
        }

		/// <summary>Returns the port number of the sender of the current XDR data.</summary>
		/// <remarks>
		/// Returns the port number of the sender of the current XDR data.
		/// This method should only be called after
		/// <see cref="beginEncoding(IPAddress, int)">beginEncoding(IPAddress, int)
		/// 	</see>
		/// ,
		/// otherwise it might return stale information.
		/// </remarks>
		/// <returns>Port number of the sender of the current XDR data.</returns>
		public virtual int getSenderPort()
		{
            return ((IPEndPoint)(socket.RemoteEndPoint)).Port;
        }

		/// <summary>Begins encoding a new XDR record.</summary>
		/// <remarks>
		/// Begins encoding a new XDR record. This typically involves resetting this
		/// encoding XDR stream back into a known state.
		/// </remarks>
		/// <param name="receiverAddress">
		/// Indicates the receiver of the XDR data. This can be
		/// <code>null</code> for XDR streams connected permanently to a
		/// receiver (like in case of TCP/IP based XDR streams).
		/// </param>
		/// <param name="receiverPort">Port number of the receiver.</param>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public override void beginEncoding(IPAddress receiverAddress, int receiverPort
			)
		{
		}

		//
		// Begin encoding with the four byte word after the fragment header,
		// which also four bytes wide. We have to remember where we can find
		// the fragment header as we support batching/pipelining calls, so
		// several requests (each in its own fragment) can be simultaneously
		// in the write buffer.
		//
		//bufferFragmentHeaderIndex = bufferIndex;
		//bufferIndex += 4;
		/// <summary>
		/// Flushes this encoding XDR stream and forces any buffered output bytes
		/// to be written out.
		/// </summary>
		/// <remarks>
		/// Flushes this encoding XDR stream and forces any buffered output bytes
		/// to be written out. The general contract of <code>endEncoding</code> is that
		/// calling it is an indication that the current record is finished and any
		/// bytes previously encoded should immediately be written to their intended
		/// destination.
		/// </remarks>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public override void endEncoding()
		{
			flush(true, false);
		}

		/// <summary>Ends the current record fort this encoding XDR stream.</summary>
		/// <remarks>
		/// Ends the current record fort this encoding XDR stream. If the parameter
		/// <code>flush</code> is <code>true</code> any buffered output bytes are
		/// immediately written to their intended destination. If <code>flush</code>
		/// is <code>false</code>, then more than one record can be pipelined, for
		/// instance, to batch several ONC/RPC calls. In this case the ONC/RPC
		/// server <b>must not</b> send a reply (with the exception for the last
		/// call in a batch, which might be trigger a reply). Otherwise, you will
		/// most probably cause an interaction deadlock between client and server.
		/// </remarks>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public virtual void endEncoding(bool flush)
		{
			this.flush(true, !flush);
		}

		/// <summary>
		/// Flushes the current contents of the buffer as one fragment to the
		/// network.
		/// </summary>
		/// <remarks>
		/// Flushes the current contents of the buffer as one fragment to the
		/// network.
		/// </remarks>
		/// <param name="lastFragment">
		/// <code>true</code> if this is the last fragment of
		/// the current XDR record.
		/// </param>
		/// <param name="batch">
		/// if last fragment and <code>batch</code> is
		/// <code>true</code>, then the buffer is not flushed to the network
		/// but instead we wait for more records to be encoded.
		/// </param>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		private void flush(bool lastFragment, bool batch)
		{
			//
			// Encode the fragment header. We have to take batching/pipelining
			// into account, so multiple complete fragments may be waiting in
			// the same write buffer. The variable bufferFragmentHeaderIndex
			// points to the place where we should store this fragment's header.
			//
			int fragmentLength = bufferIndex - bufferFragmentHeaderIndex - 4;
			if (lastFragment)
			{
				fragmentLength |= unchecked((int)(0x80000000));
			}
			buffer[bufferFragmentHeaderIndex] = (byte)((fragmentLength) >> (24 & 0x1f));
			buffer[bufferFragmentHeaderIndex + 1] = (byte)((fragmentLength) >> (16 & 0x1f));
			buffer[bufferFragmentHeaderIndex + 2] = (byte)((fragmentLength) >> (8 & 0x1f));
			buffer[bufferFragmentHeaderIndex + 3] = (byte)fragmentLength;
			if (!lastFragment || !batch || (bufferIndex >= bufferHighmark))
			{
				// buffer is full, so we have to flush
				// buffer not full, but last fragment and not in batch
				// not enough space for next
				// fragment header and one int
				//
				// Finally write the buffer's contents into the vastness of
				// network space. This has to be done when we do not need to
				// pipeline calls and if there is still enough space left in
				// the buffer for the fragment header and at least a single
				// int.
				//
				stream.Write(buffer, 0, bufferIndex);
                stream.Flush();
				//
				// Reset write pointer after the fragment header int within
				// buffer, so the next bunch of data can be encoded.
				//
				bufferFragmentHeaderIndex = 0;
				bufferIndex = 4;
			}
			else
			{
				//
				// Batch/pipeline several consecuting XDR records. So do not
				// flush the buffer yet to the network but instead wait for more
				// data.
				//
				bufferFragmentHeaderIndex = bufferIndex;
				bufferIndex += 4;
			}
		}

		/// <summary>
		/// Closes this encoding XDR stream and releases any system resources
		/// associated with this stream.
		/// </summary>
		/// <remarks>
		/// Closes this encoding XDR stream and releases any system resources
		/// associated with this stream. The general contract of <code>close</code>
		/// is that it closes the encoding XDR stream. A closed XDR stream cannot
		/// perform encoding operations and cannot be reopened.
		/// </remarks>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public override void Close()
		{
			buffer = null;
			socket = null;
		}

		/// <summary>
		/// Encodes (aka "serializes") a "XDR int" value and writes it down a
		/// XDR stream.
		/// </summary>
		/// <remarks>
		/// Encodes (aka "serializes") a "XDR int" value and writes it down a
		/// XDR stream. A XDR int is 32 bits wide -- the same width Java's "int"
		/// data type has. This method is one of the basic methods all other
		/// methods can rely on.
		/// </remarks>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public override void xdrEncodeInt(int value)
		{
			if (bufferIndex > bufferHighmark)
			{
				flush(false, false);
			}
			//
			// There's enough space in the buffer, so encode this int as
			// four bytes (french octets) in big endian order (that is, the
			// most significant byte comes first.
			//
			buffer[bufferIndex++] = (byte)((value) >> (24 & 0x1f));
			buffer[bufferIndex++] = (byte)((value) >> (16 & 0x1f));
			buffer[bufferIndex++] = (byte)((value) >> (8 & 0x1f));
			buffer[bufferIndex++] = (byte)value;
		}

		/// <summary>
		/// Encodes (aka "serializes") a XDR opaque value, which is represented
		/// by a vector of byte values, and starts at <code>offset</code> with a
		/// length of <code>length</code>.
		/// </summary>
		/// <remarks>
		/// Encodes (aka "serializes") a XDR opaque value, which is represented
		/// by a vector of byte values, and starts at <code>offset</code> with a
		/// length of <code>length</code>. Only the opaque value is encoded, but
		/// no length indication is preceeding the opaque value, so the receiver
		/// has to know how long the opaque value will be. The encoded data is
		/// always padded to be a multiple of four. If the given length is not a
		/// multiple of four, zero bytes will be used for padding.
		/// </remarks>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public override void xdrEncodeOpaque(byte[] value, int offset, int length)
		{
			int padding = (4 - (length & 3)) & 3;
			int toCopy;
			while (length > 0)
			{
				toCopy = bufferHighmark - bufferIndex + 4;
				if (toCopy >= length)
				{
					//
					// The buffer has more free space than we need. So copy the
					// bytes and leave the stage.
					//
					System.Array.Copy(value, offset, buffer, bufferIndex, length);
					bufferIndex += length;
					// No need to adjust "offset", because this is the last round.
					break;
				}
				else
				{
					//
					// We need to copy more data than currently available from our
					// buffer, so we copy all we can get our hands on, then fill
					// the buffer again and repeat this until we got all we want.
					//
					System.Array.Copy(value, offset, buffer, bufferIndex, toCopy);
					bufferIndex += toCopy;
					offset += toCopy;
					length -= toCopy;
					flush(false, false);
				}
			}
			System.Array.Copy(paddingZeros, 0, buffer, bufferIndex, padding);
			bufferIndex += padding;
		}

		/// <summary>
		/// The streaming socket to be used when receiving this XDR stream's
		/// buffer contents.
		/// </summary>
		/// <remarks>
		/// The streaming socket to be used when receiving this XDR stream's
		/// buffer contents.
		/// </remarks>
		private Socket socket;

		/// <summary>The output stream used to get rid of bytes going off to the network.</summary>
		/// <remarks>The output stream used to get rid of bytes going off to the network.</remarks>
		internal Stream stream;

		/// <summary>
		/// The buffer which will be filled from the datagram socket and then
		/// be used to supply the information when decoding data.
		/// </summary>
		/// <remarks>
		/// The buffer which will be filled from the datagram socket and then
		/// be used to supply the information when decoding data.
		/// </remarks>
		private byte[] buffer;

		/// <summary>The write pointer is an index into the <code>buffer</code>.</summary>
		/// <remarks>The write pointer is an index into the <code>buffer</code>.</remarks>
		private int bufferIndex;

		/// <summary>Index of the last four byte word in the <code>buffer</code>.</summary>
		/// <remarks>Index of the last four byte word in the <code>buffer</code>.</remarks>
		private int bufferHighmark;

		/// <summary>Index of fragment header within <code>buffer</code>.</summary>
		/// <remarks>Index of fragment header within <code>buffer</code>.</remarks>
		private int bufferFragmentHeaderIndex;

		/// <summary>Some zeros, only needed for padding -- like in real life.</summary>
		/// <remarks>Some zeros, only needed for padding -- like in real life.</remarks>
		private static readonly byte[] paddingZeros = new byte[] { 0, 0, 0, 0 };
	}
}
