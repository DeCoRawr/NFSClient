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
namespace org.acplt.oncrpc
{
	/// <summary>
	/// The <code>XdrBufferEncodingStream</code> class provides a buffer-based
	/// XDR stream.
	/// </summary>
	/// <remarks>
	/// The <code>XdrBufferEncodingStream</code> class provides a buffer-based
	/// XDR stream.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.2 $ $Date: 2003/08/14 11:26:50 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class XdrBufferEncodingStream : XdrEncodingStream
	{
		/// <summary>
		/// Constructs a new <code>XdrBufferEncodingStream</code> with a buffer
		/// to encode data into of the given size.
		/// </summary>
		/// <remarks>
		/// Constructs a new <code>XdrBufferEncodingStream</code> with a buffer
		/// to encode data into of the given size.
		/// </remarks>
		/// <param name="bufferSize">Size of buffer to store encoded data in.</param>
		public XdrBufferEncodingStream(int bufferSize)
		{
			if ((bufferSize < 0) || (bufferSize & 3) != 0)
			{
				throw (new System.ArgumentException("size of buffer must be a multiple of four and must not be negative"
					));
			}
			this.buffer = new byte[bufferSize];
			bufferIndex = 0;
			bufferHighmark = buffer.Length - 4;
		}

		/// <summary>
		/// Constructs a new <code>XdrBufferEncodingStream</code> with a given
		/// buffer.
		/// </summary>
		/// <remarks>
		/// Constructs a new <code>XdrBufferEncodingStream</code> with a given
		/// buffer.
		/// </remarks>
		/// <param name="buffer">Buffer to store encoded information in.</param>
		/// <exception cref="System.ArgumentException">
		/// if <code>encodedLength</code> is not
		/// a multiple of four.
		/// </exception>
		public XdrBufferEncodingStream(byte[] buffer)
		{
			//
			// Make sure that the buffer size is a multiple of four, otherwise
			// throw an exception.
			//
			if ((buffer.Length & 3) != 0)
			{
				throw (new System.ArgumentException("size of buffer must be a multiple of four"));
			}
			this.buffer = buffer;
			bufferIndex = 0;
			bufferHighmark = buffer.Length - 4;
		}

		/// <summary>Returns the amount of encoded data in the buffer.</summary>
		/// <remarks>Returns the amount of encoded data in the buffer.</remarks>
		/// <returns>length of data encoded in buffer.</returns>
		public virtual int getXdrLength()
		{
			return bufferIndex;
		}

		/// <summary>Returns the buffer holding encoded data.</summary>
		/// <remarks>Returns the buffer holding encoded data.</remarks>
		/// <returns>Buffer with encoded data.</returns>
		public virtual byte[] getXdrData()
		{
			return buffer;
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
				throw (new org.acplt.oncrpc.OncRpcException(org.acplt.oncrpc.OncRpcException.RPC_BUFFEROVERFLOW
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
				throw (new org.acplt.oncrpc.OncRpcException(org.acplt.oncrpc.OncRpcException.RPC_BUFFEROVERFLOW
					));
			}
		}

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
