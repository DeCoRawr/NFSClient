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
using System.Threading;
namespace org.acplt.oncrpc
{
    /// <summary>
    /// The <code>XdrUdpDecodingStream</code> class provides the necessary
    /// functionality to
    /// <see cref="XdrDecodingStream">XdrDecodingStream</see>
    /// to receive XDR packets from the
    /// network using the datagram-oriented UDP/IP.
    /// Converted to C# using the db4o Sharpen tool.
    /// </summary>
    /// <version>$Revision: 1.2 $ $Date: 2005/11/11 21:07:40 $ $State: Exp $ $Locker:  $</version>
    /// <author>Harald Albrecht</author>
    /// <author>Jay Walters</author>
    public class XdrUdpDecodingStream : XdrDecodingStream
    {
        /// <summary>
        /// Construct a new <code>XdrUdpDecodingStream</code> object and associate
        /// it with the given <code>datagramSocket</code> for UDP/IP-based
        /// communication.
        /// </summary>
        /// <remarks>
        /// Construct a new <code>XdrUdpDecodingStream</code> object and associate
        /// it with the given <code>datagramSocket</code> for UDP/IP-based
        /// communication. This constructor is typically used when communicating
        /// with servers over UDP/IP using a "connected" datagram socket.
        /// </remarks>
        /// <param name="datagramSocket">Datagram socket from which XDR data is received.</param>
        /// <param name="bufferSize">
        /// Size of packet buffer for storing received XDR
        /// datagrams.
        /// </param>
        public XdrUdpDecodingStream(Socket datagramSocket, int bufferSize)
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
            return senderAddress;
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
            return senderPort;
        }

        /// <summary>Initiates decoding of the next XDR record.</summary>
        /// <remarks>
        /// Initiates decoding of the next XDR record. For UDP-based XDR decoding
        /// streams this reads in the next datagram from the network socket.
        /// </remarks>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public override void beginDecoding()
        {
            // Creates an IpEndPoint to capture the identity of the sending host.
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint remoteEP = (EndPoint)sender;
            socket.ReceiveFrom(buffer, ref remoteEP);
            senderAddress = ((IPEndPoint)remoteEP).Address;
            senderPort = ((IPEndPoint)remoteEP).Port;
            bufferIndex = 0;
            bufferHighmark = buffer.Length - 4;
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
        /// <p>This implementation frees the allocated buffer but does not close
        /// the associated datagram socket. It only throws away the reference to
        /// this socket.
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
                throw (new OncRpcException(OncRpcException.RPC_BUFFERUNDERFLOW
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
                    throw (new OncRpcException(OncRpcException.RPC_BUFFERUNDERFLOW
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
                    throw (new OncRpcException(OncRpcException.RPC_BUFFERUNDERFLOW
                        ));
                }
            }
            bufferIndex += alignedLength;
        }

        /// <summary>
        /// The datagram socket to be used when receiving this XDR stream's
        /// buffer contents.
        /// </summary>
        /// <remarks>
        /// The datagram socket to be used when receiving this XDR stream's
        /// buffer contents.
        /// </remarks>
        private Socket socket;

        /// <summary>Sender's address of current buffer contents.</summary>
        /// <remarks>Sender's address of current buffer contents.</remarks>
        private IPAddress senderAddress = null;

        /// <summary>The senders's port.</summary>
        /// <remarks>The senders's port.</remarks>
        private int senderPort = 0;

        /// <summary>
        /// The buffer which will be filled from the datagram socket and then
        /// be used to supply the information when decoding data.
        /// </summary>
        /// <remarks>
        /// The buffer which will be filled from the datagram socket and then
        /// be used to supply the information when decoding data.
        /// </remarks>
        private byte[] buffer;

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
