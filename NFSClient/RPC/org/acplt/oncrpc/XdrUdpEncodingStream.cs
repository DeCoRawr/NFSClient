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
using System;
namespace org.acplt.oncrpc
{
	/// <summary>
	/// The <code>XdrUdpDecodingStream</code> class provides the necessary
	/// functionality to
	/// <see cref="XdrDecodingStream">XdrDecodingStream</see>
	/// to send XDR packets over the
	/// network using the datagram-oriented UDP/IP.
        /// Converted to C# using the db4o Sharpen tool.
	/// </summary>
	/// <version>$Revision: 1.2 $ $Date: 2003/08/14 11:07:39 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class XdrUdpEncodingStream : XdrEncodingStream
	{
		/// <summary>
		/// Creates a new UDP-based encoding XDR stream, associated with the
		/// given datagram socket.
		/// </summary>
		/// <remarks>
		/// Creates a new UDP-based encoding XDR stream, associated with the
		/// given datagram socket.
		/// </remarks>
		/// <param name="datagramSocket">
		/// Datagram-based socket to use to get rid of
		/// encoded data.
		/// </param>
		/// <param name="bufferSize">
		/// Size of buffer to store encoded data before it
		/// is sent as one datagram.
		/// </param>
		public XdrUdpEncodingStream(Socket datagramSocket, int bufferSize
			)
		{
			socket = datagramSocket;
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
			buffer = new byte[bufferSize];
			bufferIndex = 0;
			bufferHighmark = bufferSize - 4;
		}

		/// <summary>Begins encoding a new XDR record.</summary>
		/// <remarks>
		/// Begins encoding a new XDR record. This involves resetting this
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
			this.receiverAddress = receiverAddress;
			this.receiverPort = receiverPort;
			bufferIndex = 0;
		}

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
            IPEndPoint endPoint = new IPEndPoint(receiverAddress, receiverPort);
            socket.SendTo(buffer, bufferIndex, SocketFlags.None, endPoint);
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
			if (bufferIndex <= bufferHighmark)
			{
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
			else
			{
				throw (new OncRpcException(OncRpcException.RPC_BUFFEROVERFLOW
					));
			}
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
			//
			// First calculate the number of bytes needed for padding.
			//
			int padding = (4 - (length & 3)) & 3;
			if (bufferIndex <= bufferHighmark - (length + padding))
			{
				System.Array.Copy(value, offset, buffer, bufferIndex, length);
				bufferIndex += length;
				if (padding != 0)
				{
					//
					// If the length of the opaque data was not a multiple, then
					// pad with zeros, so the write pointer (argh! how comes Java
					// has a pointer...?!) points to a byte, which has an index
					// of a multiple of four.
					//
					System.Array.Copy(paddingZeros, 0, buffer, bufferIndex, padding);
					bufferIndex += padding;
				}
			}
			else
			{
				throw (new OncRpcException(OncRpcException.RPC_BUFFEROVERFLOW
					));
			}
		}

		/// <summary>
		/// The datagram socket to be used when sending this XDR stream's
		/// buffer contents.
		/// </summary>
		/// <remarks>
		/// The datagram socket to be used when sending this XDR stream's
		/// buffer contents.
		/// </remarks>
		private Socket socket;

		/// <summary>Receiver address of current buffer contents when flushed.</summary>
		/// <remarks>Receiver address of current buffer contents when flushed.</remarks>
		private IPAddress receiverAddress = null;

		/// <summary>The receiver's port.</summary>
		/// <remarks>The receiver's port.</remarks>
		private int receiverPort = 0;

		/// <summary>
		/// The buffer which will receive the encoded information, before it
		/// is sent via a datagram socket.
		/// </summary>
		/// <remarks>
		/// The buffer which will receive the encoded information, before it
		/// is sent via a datagram socket.
		/// </remarks>
		private byte[] buffer;

		/// <summary>The write pointer is an index into the <code>buffer</code>.</summary>
		/// <remarks>The write pointer is an index into the <code>buffer</code>.</remarks>
		private int bufferIndex;

		/// <summary>Index of the last four byte word in the buffer.</summary>
		/// <remarks>Index of the last four byte word in the buffer.</remarks>
		private int bufferHighmark;

		/// <summary>Some zeros, only needed for padding -- like in real life.</summary>
		/// <remarks>Some zeros, only needed for padding -- like in real life.</remarks>
		private static readonly byte[] paddingZeros = new byte[] { 0, 0, 0, 0 };
	}
}
