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
	/// The <code>XdrBufferDecodingStream</code> class provides the necessary
	/// functionality to
	/// <see cref="XdrDecodingStream">XdrDecodingStream</see>
	/// to retrieve XDR packets from
	/// a byte buffer.
        /// Converted to C# using the db4o Sharpen tool.
	/// </summary>
	/// <version>$Revision: 1.2 $ $Date: 2005/11/11 21:06:36 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class XdrBufferDecodingStream : XdrDecodingStream
	{
		/// <summary>
		/// Construct a new <code>XdrUdpDecodingStream</code> object and associate
		/// it with a buffer containing encoded XDR data.
		/// </summary>
		/// <remarks>
		/// Construct a new <code>XdrUdpDecodingStream</code> object and associate
		/// it with a buffer containing encoded XDR data.
		/// </remarks>
		/// <param name="buffer">Buffer containing encoded XDR data.</param>
		/// <param name="encodedLength">Length of encoded XDR data within the buffer.</param>
		/// <exception cref="System.ArgumentException">
		/// if <code>encodedLength</code> is not
		/// a multiple of four.
		/// </exception>
		public XdrBufferDecodingStream(byte[] buffer, int encodedLength)
		{
			setXdrData(buffer, encodedLength);
		}

		/// <summary>
		/// Construct a new <code>XdrUdpDecodingStream</code> object and associate
		/// it with a buffer containing encoded XDR data.
		/// </summary>
		/// <remarks>
		/// Construct a new <code>XdrUdpDecodingStream</code> object and associate
		/// it with a buffer containing encoded XDR data.
		/// </remarks>
		/// <param name="buffer">Buffer containing encoded XDR data.</param>
		/// <exception cref="System.ArgumentException">
		/// if the size of the buffer is not
		/// a multiple of four.
		/// </exception>
		public XdrBufferDecodingStream(byte[] buffer)
		{
			setXdrData(buffer, buffer.Length);
		}

		/// <summary>
		/// Sets the buffer containing encoded XDR data as well as the length of
		/// the encoded data.
		/// </summary>
		/// <remarks>
		/// Sets the buffer containing encoded XDR data as well as the length of
		/// the encoded data.
		/// </remarks>
		/// <param name="buffer">Buffer containing encoded XDR data.</param>
		/// <param name="encodedLength">Length of encoded XDR data within the buffer.</param>
		/// <exception cref="System.ArgumentException">
		/// if <code>encodedLength</code> is not
		/// a multiple of four.
		/// </exception>
		public virtual void setXdrData(byte[] buffer, int encodedLength)
		{
			//
			// Make sure that the buffer size is a multiple of four, otherwise
			// throw an exception.
			//
			if ((encodedLength < 0) || (encodedLength & 3) != 0)
			{
				throw (new System.ArgumentException("length of encoded data must be a multiple of four and must not be negative"
					));
			}
			this.buffer = buffer;
			this.encodedLength = encodedLength;
			bufferIndex = 0;
			bufferHighmark = -4;
		}

		/// <summary>Returns the Internet address of the sender of the current XDR data.</summary>
		/// <remarks>
		/// Returns the Internet address of the sender of the current XDR data.
		/// This method should only be called after
		/// <see cref="beginDecoding()">beginDecoding()</see>
		/// ,
		/// otherwise it might return stale information.
		/// </remarks>
		/// <returns>InetAddress of the sender of the current XDR data.</returns>
		public override IPAddress getSenderAddress()
		{
			return null;
		}

		/// <summary>Returns the port number of the sender of the current XDR data.</summary>
		/// <remarks>
		/// Returns the port number of the sender of the current XDR data.
		/// This method should only be called after
		/// <see cref="beginDecoding()">beginDecoding()</see>
		/// ,
		/// otherwise it might return stale information.
		/// </remarks>
		/// <returns>Port number of the sender of the current XDR data.</returns>
		public override int getSenderPort()
		{
			return 0;
		}

		/// <summary>Initiates decoding of the next XDR record.</summary>
		/// <remarks>Initiates decoding of the next XDR record.</remarks>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public override void beginDecoding()
		{
			bufferIndex = 0;
			bufferHighmark = encodedLength - 4;
		}

		/// <summary>End decoding of the current XDR record.</summary>
		/// <remarks>
		/// End decoding of the current XDR record. The general contract of
		/// <code>endDecoding</code> is that calling it is an indication that
		/// the current record is no more interesting to the caller and any
		/// allocated data for this record can be freed.
		/// <p>This method overrides
		/// <see cref="XdrDecodingStream.endDecoding()">XdrDecodingStream.endDecoding()</see>
		/// . It does nothing
		/// more than resetting the buffer pointer (eeek! a pointer in Java!!!) back
		/// to the begin of an empty buffer, so attempts to decode data will fail
		/// until the buffer is filled again.
		/// </remarks>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public override void endDecoding()
		{
			bufferIndex = 0;
			bufferHighmark = -4;
		}

		/// <summary>
		/// Closes this decoding XDR stream and releases any system resources
		/// associated with this stream.
		/// </summary>
		/// <remarks>
		/// Closes this decoding XDR stream and releases any system resources
		/// associated with this stream. A closed XDR stream cannot perform decoding
		/// operations and cannot be reopened.
		/// <p>This implementation frees the allocated buffer.
		/// </remarks>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public override void Close()
		{
			buffer = null;
		}

		/// <summary>
		/// Decodes (aka "deserializes") a "XDR int" value received from a
		/// XDR stream.
		/// </summary>
		/// <remarks>
		/// Decodes (aka "deserializes") a "XDR int" value received from a
		/// XDR stream. A XDR int is 32 bits wide -- the same width Java's "int"
		/// data type has.
		/// </remarks>
		/// <returns>The decoded int value.</returns>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public override int xdrDecodeInt()
		{
			if (bufferIndex <= bufferHighmark)
			{
				//
				// There's enough space in the buffer to hold at least one
				// XDR int. So let's retrieve it now.
				// Note: buffer[...] gives a byte, which is signed. So if we
				// add it to the value (which is int), it has to be widened
				// to 32 bit, so its sign is propagated. To avoid this sign
				// madness, we have to "and" it with 0xFF, so all unwanted
				// bits are cut off after sign extension. Sigh.
				//
				int value = buffer[bufferIndex++];
				value = (value << 8) + (buffer[bufferIndex++] & unchecked((int)(0xFF)));
				value = (value << 8) + (buffer[bufferIndex++] & unchecked((int)(0xFF)));
				value = (value << 8) + (buffer[bufferIndex++] & unchecked((int)(0xFF)));
				return value;
			}
			else
			{
				throw (new org.acplt.oncrpc.OncRpcException(org.acplt.oncrpc.OncRpcException.RPC_BUFFERUNDERFLOW
					));
			}
		}

		/// <summary>
		/// Decodes (aka "deserializes") an opaque value, which is nothing more
		/// than a series of octets (or 8 bits wide bytes).
		/// </summary>
		/// <remarks>
		/// Decodes (aka "deserializes") an opaque value, which is nothing more
		/// than a series of octets (or 8 bits wide bytes). Because the length
		/// of the opaque value is given, we don't need to retrieve it from the
		/// XDR stream. This is different from
		/// <see cref="xdrDecodeOpaque(byte[], int, int)">xdrDecodeOpaque(byte[], int, int)</see>
		/// where
		/// first the length of the opaque value is retrieved from the XDR stream.
		/// </remarks>
		/// <param name="length">Length of opaque data to decode.</param>
		/// <returns>Opaque data as a byte vector.</returns>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public override byte[] xdrDecodeOpaque(int length)
		{
			//
			// First make sure that the length is always a multiple of four.
			//
			int alignedLength = length;
			if ((alignedLength & 3) != 0)
			{
				alignedLength = (alignedLength & ~3) + 4;
			}
			//
			// Now allocate enough memory to hold the data to be retrieved.
			//
			byte[] bytes = new byte[length];
			if (length > 0)
			{
				if (bufferIndex <= bufferHighmark - alignedLength + 4)
				{
					System.Array.Copy(buffer, bufferIndex, bytes, 0, length);
				}
				else
				{
					throw (new org.acplt.oncrpc.OncRpcException(org.acplt.oncrpc.OncRpcException.RPC_BUFFERUNDERFLOW
						));
				}
			}
			bufferIndex += alignedLength;
			return bytes;
		}

		/// <summary>
		/// Decodes (aka "deserializes") a XDR opaque value, which is represented
		/// by a vector of byte values, and starts at <code>offset</code> with a
		/// length of <code>length</code>.
		/// </summary>
		/// <remarks>
		/// Decodes (aka "deserializes") a XDR opaque value, which is represented
		/// by a vector of byte values, and starts at <code>offset</code> with a
		/// length of <code>length</code>. Only the opaque value is decoded, so the
		/// caller has to know how long the opaque value will be. The decoded data
		/// is always padded to be a multiple of four (because that's what the
		/// sender does).
		/// </remarks>
		/// <param name="opaque">Byte vector which will receive the decoded opaque value.</param>
		/// <param name="offset">Start offset in the byte vector.</param>
		/// <param name="length">the number of bytes to decode.</param>
		/// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
		/// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		public override void xdrDecodeOpaque(byte[] opaque, int offset, int length)
		{
			//
			// First make sure that the length is always a multiple of four.
			//
			int alignedLength = length;
			if ((alignedLength & 3) != 0)
			{
				alignedLength = (alignedLength & ~3) + 4;
			}
			//
			// Now allocate enough memory to hold the data to be retrieved.
			//
			if (length > 0)
			{
				if (bufferIndex <= bufferHighmark - alignedLength + 4)
				{
					System.Array.Copy(buffer, bufferIndex, opaque, offset, length);
				}
				else
				{
					throw (new org.acplt.oncrpc.OncRpcException(org.acplt.oncrpc.OncRpcException.RPC_BUFFERUNDERFLOW
						));
				}
			}
			bufferIndex += alignedLength;
		}

		/// <summary>
		/// The buffer which will be filled from the datagram socket and then
		/// be used to supply the information when decoding data.
		/// </summary>
		/// <remarks>
		/// The buffer which will be filled from the datagram socket and then
		/// be used to supply the information when decoding data.
		/// </remarks>
		private byte[] buffer;

		/// <summary>Length of encoded data in <code>buffer</code>.</summary>
		/// <remarks>Length of encoded data in <code>buffer</code>.</remarks>
		private int encodedLength;

		/// <summary>The read pointer is an index into the <code>buffer</code>.</summary>
		/// <remarks>The read pointer is an index into the <code>buffer</code>.</remarks>
		private int bufferIndex;

		/// <summary>
		/// Index of the last four byte word in the buffer, which has been read
		/// in from the datagram socket.
		/// </summary>
		/// <remarks>
		/// Index of the last four byte word in the buffer, which has been read
		/// in from the datagram socket.
		/// </remarks>
		private int bufferHighmark;
	}
}
