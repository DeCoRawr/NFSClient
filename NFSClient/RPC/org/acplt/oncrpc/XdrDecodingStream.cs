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
using System.Net;
using System.Text;
namespace org.acplt.oncrpc
{
    /// <summary>Defines the abstract base class for all decoding XDR streams.</summary>
    /// <remarks>
    /// Defines the abstract base class for all decoding XDR streams. A decoding
    /// XDR stream returns data in the form of Java data types which it reads
    /// from a data source (for instance, network or memory buffer) in the
    /// platform-independent XDR format.
    /// <p>Derived classes need to implement the
    /// <see cref="xdrDecodeInt()">xdrDecodeInt()</see>
    /// ,
    /// <see cref="xdrDecodeOpaque(int)">xdrDecodeOpaque(int)</see>
    /// and
    /// <see cref="xdrDecodeOpaque(byte[], int, int)">xdrDecodeOpaque(byte[], int, int)</see>
    /// methods to make this complete
    /// mess workable.
    /// Converted to C# using the db4o Sharpen tool.
    /// </remarks>
    /// <version>$Revision: 1.3 $ $Date: 2003/08/14 13:48:33 $ $State: Exp $ $Locker:  $</version>
    /// <author>Harald Albrecht</author>
    /// <author>Jay Walters</author>
    public abstract class XdrDecodingStream
    {
        /// <summary>Returns the Internet address of the sender of the current XDR data.</summary>
        /// <remarks>
        /// Returns the Internet address of the sender of the current XDR data.
        /// This method should only be called after
        /// <see cref="beginDecoding()">beginDecoding()</see>
        /// , otherwise
        /// it might return stale information.
        /// </remarks>
        /// <returns>InetAddress of the sender of the current XDR data.</returns>
        public abstract IPAddress getSenderAddress();

        /// <summary>Returns the port number of the sender of the current XDR data.</summary>
        /// <remarks>
        /// Returns the port number of the sender of the current XDR data.
        /// This method should only be called after
        /// <see cref="beginDecoding()">beginDecoding()</see>
        /// , otherwise
        /// it might return stale information.
        /// </remarks>
        /// <returns>Port number of the sender of the current XDR data.</returns>
        public abstract int getSenderPort();

        /// <summary>Initiates decoding of the next XDR record.</summary>
        /// <remarks>
        /// Initiates decoding of the next XDR record. This typically involves
        /// filling the internal buffer with the next datagram from the network, or
        /// reading the next chunk of data from a stream-oriented connection. In
        /// case of memory-based communication this might involve waiting for
        /// some other process to fill the buffer and signal availability of new
        /// XDR data.
        /// </remarks>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public abstract void beginDecoding();

        /// <summary>End decoding of the current XDR record.</summary>
        /// <remarks>
        /// End decoding of the current XDR record. The general contract of
        /// <code>endDecoding</code> is that calling it is an indication that
        /// the current record is no more interesting to the caller and any
        /// allocated data for this record can be freed.
        /// <p>The <code>endDecoding</code> method of <code>XdrDecodingStream</code>
        /// does nothing.
        /// </remarks>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public virtual void endDecoding()
        {
        }

        /// <summary>
        /// Closes this decoding XDR stream and releases any system resources
        /// associated with this stream.
        /// </summary>
        /// <remarks>
        /// Closes this decoding XDR stream and releases any system resources
        /// associated with this stream. The general contract of <code>close</code>
        /// is that it closes the decoding XDR stream. A closed XDR stream cannot
        /// perform decoding operations and cannot be reopened.
        /// <p>The <code>close</code> method of <code>XdrDecodingStream</code>
        /// does nothing.
        /// </remarks>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public virtual void Close()
        {
        }

        /// <summary>
        /// Decodes (aka "deserializes") a "XDR int" value received from a
        /// XDR stream.
        /// </summary>
        /// <remarks>
        /// Decodes (aka "deserializes") a "XDR int" value received from a
        /// XDR stream. A XDR int is 32 bits wide -- the same width Java's "int"
        /// data type has. This method is one of the basic methods all other
        /// methods can rely on. Because it's so basic, derived classes have to
        /// implement it.
        /// </remarks>
        /// <returns>The decoded int value.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public abstract int xdrDecodeInt();

        /// <summary>
        /// Decodes (aka "deserializes") an opaque value, which is nothing more
        /// than a series of octets (or 8 bits wide bytes).
        /// </summary>
        /// <remarks>
        /// Decodes (aka "deserializes") an opaque value, which is nothing more
        /// than a series of octets (or 8 bits wide bytes). Because the length
        /// of the opaque value is given, we don't need to retrieve it from the
        /// XDR stream.
        /// <p>Note that this is a basic abstract method, which needs to be
        /// implemented in derived classes.
        /// </remarks>
        /// <param name="length">Length of opaque data to decode.</param>
        /// <returns>Opaque data as a byte vector.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public abstract byte[] xdrDecodeOpaque(int length);

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
        /// <p>Derived classes must ensure that the proper semantic is maintained.
        /// </remarks>
        /// <param name="opaque">Byte vector which will receive the decoded opaque value.</param>
        /// <param name="offset">Start offset in the byte vector.</param>
        /// <param name="length">the number of bytes to decode.</param>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="System.IndexOutOfRangeException">
        /// if the given <code>opaque</code>
        /// byte vector isn't large enough to receive the result.
        /// </exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public abstract void xdrDecodeOpaque(byte[] opaque, int offset, int length);

        /// <summary>
        /// Decodes (aka "deserializes") a XDR opaque value, which is represented
        /// by a vector of byte values.
        /// </summary>
        /// <remarks>
        /// Decodes (aka "deserializes") a XDR opaque value, which is represented
        /// by a vector of byte values. Only the opaque value is decoded, so the
        /// caller has to know how long the opaque value will be. The decoded data
        /// is always padded to be a multiple of four (because that's what the
        /// sender does).
        /// </remarks>
        /// <param name="opaque">Byte vector which will receive the decoded opaque value.</param>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public void xdrDecodeOpaque(byte[] opaque)
        {
            xdrDecodeOpaque(opaque, 0, opaque.Length);
        }

        /// <summary>
        /// Decodes (aka "deserializes") a XDR opaque value, which is represented
        /// by a vector of byte values.
        /// </summary>
        /// <remarks>
        /// Decodes (aka "deserializes") a XDR opaque value, which is represented
        /// by a vector of byte values. The length of the opaque value to decode
        /// is pulled off of the XDR stream, so the caller does not need to know
        /// the exact length in advance. The decoded data is always padded to be
        /// a multiple of four (because that's what the sender does).
        /// </remarks>
        /// <returns>The byte vector containing the decoded data.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public byte[] xdrDecodeDynamicOpaque()
        {
            int length = xdrDecodeInt();
            byte[] opaque = new byte[length];
            if (length != 0)
            {
                xdrDecodeOpaque(opaque);
            }
            return opaque;
        }

        /// <summary>
        /// Decodes (aka "deserializes") a vector of bytes, which is nothing more
        /// than a series of octets (or 8 bits wide bytes), each packed into its
        /// very own 4 bytes (XDR int).
        /// </summary>
        /// <remarks>
        /// Decodes (aka "deserializes") a vector of bytes, which is nothing more
        /// than a series of octets (or 8 bits wide bytes), each packed into its
        /// very own 4 bytes (XDR int). Byte vectors are decoded together with a
        /// preceeding length value. This way the receiver doesn't need to know
        /// the length of the vector in advance.
        /// </remarks>
        /// <returns>The byte vector containing the decoded data.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public byte[] xdrDecodeByteVector()
        {
            int length = xdrDecodeInt();
            if (length > 0)
            {
                byte[] bytes = new byte[length];
                for (int i = 0; i < length; ++i)
                {
                    bytes[i] = (byte)xdrDecodeInt();
                }
                return bytes;
            }
            else
            {
                return new byte[0];
            }
        }

        /// <summary>
        /// Decodes (aka "deserializes") a vector of bytes, which is nothing more
        /// than a series of octets (or 8 bits wide bytes), each packed into its
        /// very own 4 bytes (XDR int).
        /// </summary>
        /// <remarks>
        /// Decodes (aka "deserializes") a vector of bytes, which is nothing more
        /// than a series of octets (or 8 bits wide bytes), each packed into its
        /// very own 4 bytes (XDR int).
        /// </remarks>
        /// <param name="length">of vector to read.</param>
        /// <returns>The byte vector containing the decoded data.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public byte[] xdrDecodeByteFixedVector(int length)
        {
            if (length > 0)
            {
                byte[] bytes = new byte[length];
                for (int i = 0; i < length; ++i)
                {
                    bytes[i] = (byte)xdrDecodeInt();
                }
                return bytes;
            }
            else
            {
                return new byte[0];
            }
        }

        /// <summary>Decodes (aka "deserializes") a byte read from this XDR stream.</summary>
        /// <remarks>Decodes (aka "deserializes") a byte read from this XDR stream.</remarks>
        /// <returns>Decoded byte value.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public byte xdrDecodeByte()
        {
            return (byte)xdrDecodeInt();
        }

        /// <summary>
        /// Decodes (aka "deserializes") a short (which is a 16 bit quantity)
        /// read from this XDR stream.
        /// </summary>
        /// <remarks>
        /// Decodes (aka "deserializes") a short (which is a 16 bit quantity)
        /// read from this XDR stream.
        /// </remarks>
        /// <returns>Decoded short value.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public short xdrDecodeShort()
        {
            return (short)xdrDecodeInt();
        }

        /// <summary>
        /// Decodes (aka "deserializes") a long (which is called a "hyper" in XDR
        /// babble and is 64&nbsp;bits wide) read from a XDR stream.
        /// </summary>
        /// <remarks>
        /// Decodes (aka "deserializes") a long (which is called a "hyper" in XDR
        /// babble and is 64&nbsp;bits wide) read from a XDR stream.
        /// </remarks>
        /// <returns>Decoded long value.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public long xdrDecodeLong()
        {
            //
            // Similiar to xdrEncodeLong: just read in two ints in network order.  We
            // OR the int's together rather than adding them...
            //
            return ((long)xdrDecodeInt() << 32) | ((long)xdrDecodeInt() & 0xffffffff);
        }

        /// <summary>
        /// Decodes (aka "deserializes") a float (which is a 32 bits wide floating
        /// point entity) read from a XDR stream.
        /// </summary>
        /// <remarks>
        /// Decodes (aka "deserializes") a float (which is a 32 bits wide floating
        /// point entity) read from a XDR stream.
        /// </remarks>
        /// <returns>Decoded float value.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public float xdrDecodeFloat()
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(xdrDecodeInt()), 0);
        }

        /// <summary>
        /// Decodes (aka "deserializes") a double (which is a 64 bits wide floating
        /// point entity) read from a XDR stream.
        /// </summary>
        /// <remarks>
        /// Decodes (aka "deserializes") a double (which is a 64 bits wide floating
        /// point entity) read from a XDR stream.
        /// </remarks>
        /// <returns>Decoded double value.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public double xdrDecodeDouble()
        {
            return BitConverter.Int64BitsToDouble(xdrDecodeLong());
        }

        /// <summary>Decodes (aka "deserializes") a boolean read from a XDR stream.</summary>
        /// <remarks>Decodes (aka "deserializes") a boolean read from a XDR stream.</remarks>
        /// <returns>Decoded boolean value.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public bool xdrDecodeBoolean()
        {
            return xdrDecodeInt() != 0 ? true : false;
        }

        internal string ByteArrayToStr(byte[] bytes)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return ByteArrayToStr(bytes, enc);
        }

        internal string ByteArrayToStr(byte[] bytes, Encoding enc)
        {
            return enc.GetString(bytes);
        }

        /// <summary>Decodes (aka "deserializes") a string read from a XDR stream.</summary>
        /// <remarks>
        /// Decodes (aka "deserializes") a string read from a XDR stream.
        /// If a character encoding has been set for this stream, then this
        /// will be used for conversion.
        /// </remarks>
        /// <returns>Decoded String value.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public string xdrDecodeString()
        {
            int length = xdrDecodeInt();
            if (length > 0)
            {
                byte[] bytes = new byte[length];
                xdrDecodeOpaque(bytes, 0, length);
                return (characterEncoding != null) ? ByteArrayToStr(bytes, Encoding.GetEncoding(characterEncoding)) : ByteArrayToStr(bytes);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Decodes (aka "deserializes") a vector of short integers read from a
        /// XDR stream.
        /// </summary>
        /// <remarks>
        /// Decodes (aka "deserializes") a vector of short integers read from a
        /// XDR stream.
        /// </remarks>
        /// <returns>Decoded vector of short integers.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public short[] xdrDecodeShortVector()
        {
            int length = xdrDecodeInt();
            short[] value = new short[length];
            for (int i = 0; i < length; ++i)
            {
                value[i] = xdrDecodeShort();
            }
            return value;
        }

        /// <summary>
        /// Decodes (aka "deserializes") a vector of short integers read from a
        /// XDR stream.
        /// </summary>
        /// <remarks>
        /// Decodes (aka "deserializes") a vector of short integers read from a
        /// XDR stream.
        /// </remarks>
        /// <param name="length">of vector to read.</param>
        /// <returns>Decoded vector of short integers.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public short[] xdrDecodeShortFixedVector(int length)
        {
            short[] value = new short[length];
            for (int i = 0; i < length; ++i)
            {
                value[i] = xdrDecodeShort();
            }
            return value;
        }

        /// <summary>Decodes (aka "deserializes") a vector of ints read from a XDR stream.</summary>
        /// <remarks>Decodes (aka "deserializes") a vector of ints read from a XDR stream.</remarks>
        /// <returns>Decoded int vector.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public int[] xdrDecodeIntVector()
        {
            int length = xdrDecodeInt();
            int[] value = new int[length];
            for (int i = 0; i < length; ++i)
            {
                value[i] = xdrDecodeInt();
            }
            return value;
        }

        /// <summary>Decodes (aka "deserializes") a vector of ints read from a XDR stream.</summary>
        /// <remarks>Decodes (aka "deserializes") a vector of ints read from a XDR stream.</remarks>
        /// <param name="length">of vector to read.</param>
        /// <returns>Decoded int vector.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public int[] xdrDecodeIntFixedVector(int length)
        {
            int[] value = new int[length];
            for (int i = 0; i < length; ++i)
            {
                value[i] = xdrDecodeInt();
            }
            return value;
        }

        /// <summary>Decodes (aka "deserializes") a vector of longs read from a XDR stream.</summary>
        /// <remarks>Decodes (aka "deserializes") a vector of longs read from a XDR stream.</remarks>
        /// <returns>Decoded long vector.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public long[] xdrDecodeLongVector()
        {
            int length = xdrDecodeInt();
            long[] value = new long[length];
            for (int i = 0; i < length; ++i)
            {
                value[i] = xdrDecodeLong();
            }
            return value;
        }

        /// <summary>Decodes (aka "deserializes") a vector of longs read from a XDR stream.</summary>
        /// <remarks>Decodes (aka "deserializes") a vector of longs read from a XDR stream.</remarks>
        /// <param name="length">of vector to read.</param>
        /// <returns>Decoded long vector.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public long[] xdrDecodeLongFixedVector(int length)
        {
            long[] value = new long[length];
            for (int i = 0; i < length; ++i)
            {
                value[i] = xdrDecodeLong();
            }
            return value;
        }

        /// <summary>Decodes (aka "deserializes") a vector of floats read from a XDR stream.</summary>
        /// <remarks>Decodes (aka "deserializes") a vector of floats read from a XDR stream.</remarks>
        /// <returns>Decoded float vector.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public float[] xdrDecodeFloatVector()
        {
            int length = xdrDecodeInt();
            float[] value = new float[length];
            for (int i = 0; i < length; ++i)
            {
                value[i] = xdrDecodeFloat();
            }
            return value;
        }

        /// <summary>Decodes (aka "deserializes") a vector of floats read from a XDR stream.</summary>
        /// <remarks>Decodes (aka "deserializes") a vector of floats read from a XDR stream.</remarks>
        /// <param name="length">of vector to read.</param>
        /// <returns>Decoded float vector.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public float[] xdrDecodeFloatFixedVector(int length)
        {
            float[] value = new float[length];
            for (int i = 0; i < length; ++i)
            {
                value[i] = xdrDecodeFloat();
            }
            return value;
        }

        /// <summary>Decodes (aka "deserializes") a vector of doubles read from a XDR stream.
        /// 	</summary>
        /// <remarks>Decodes (aka "deserializes") a vector of doubles read from a XDR stream.
        /// 	</remarks>
        /// <returns>Decoded double vector.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public double[] xdrDecodeDoubleVector()
        {
            int length = xdrDecodeInt();
            double[] value = new double[length];
            for (int i = 0; i < length; ++i)
            {
                value[i] = xdrDecodeDouble();
            }
            return value;
        }

        /// <summary>Decodes (aka "deserializes") a vector of doubles read from a XDR stream.
        /// 	</summary>
        /// <remarks>Decodes (aka "deserializes") a vector of doubles read from a XDR stream.
        /// 	</remarks>
        /// <param name="length">of vector to read.</param>
        /// <returns>Decoded double vector.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public double[] xdrDecodeDoubleFixedVector(int length)
        {
            double[] value = new double[length];
            for (int i = 0; i < length; ++i)
            {
                value[i] = xdrDecodeDouble();
            }
            return value;
        }

        /// <summary>Decodes (aka "deserializes") a vector of booleans read from a XDR stream.
        /// 	</summary>
        /// <remarks>Decodes (aka "deserializes") a vector of booleans read from a XDR stream.
        /// 	</remarks>
        /// <returns>Decoded boolean vector.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public bool[] xdrDecodeBooleanVector()
        {
            int length = xdrDecodeInt();
            bool[] value = new bool[length];
            for (int i = 0; i < length; ++i)
            {
                value[i] = xdrDecodeBoolean();
            }
            return value;
        }

        /// <summary>Decodes (aka "deserializes") a vector of booleans read from a XDR stream.
        /// 	</summary>
        /// <remarks>Decodes (aka "deserializes") a vector of booleans read from a XDR stream.
        /// 	</remarks>
        /// <param name="length">of vector to read.</param>
        /// <returns>Decoded boolean vector.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public bool[] xdrDecodeBooleanFixedVector(int length)
        {
            bool[] value = new bool[length];
            for (int i = 0; i < length; ++i)
            {
                value[i] = xdrDecodeBoolean();
            }
            return value;
        }

        /// <summary>Decodes (aka "deserializes") a vector of strings read from a XDR stream.
        /// 	</summary>
        /// <remarks>Decodes (aka "deserializes") a vector of strings read from a XDR stream.
        /// 	</remarks>
        /// <returns>Decoded String vector.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public string[] xdrDecodeStringVector()
        {
            int length = xdrDecodeInt();
            string[] value = new string[length];
            for (int i = 0; i < length; ++i)
            {
                value[i] = xdrDecodeString();
            }
            return value;
        }

        /// <summary>Decodes (aka "deserializes") a vector of strings read from a XDR stream.
        /// 	</summary>
        /// <remarks>Decodes (aka "deserializes") a vector of strings read from a XDR stream.
        /// 	</remarks>
        /// <param name="length">of vector to read.</param>
        /// <returns>Decoded String vector.</returns>
        /// <exception cref="OncRpcException">if an ONC/RPC error occurs.</exception>
        /// <exception cref="System.IO.IOException">if an I/O error occurs.</exception>
        /// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
        public string[] xdrDecodeStringFixedVector(int length)
        {
            string[] value = new string[length];
            for (int i = 0; i < length; ++i)
            {
                value[i] = xdrDecodeString();
            }
            return value;
        }

        /// <summary>Set the character encoding for deserializing strings.</summary>
        /// <remarks>Set the character encoding for deserializing strings.</remarks>
        /// <param name="characterEncoding">
        /// the encoding to use for deserializing strings.
        /// If <code>null</code>, the system's default encoding is to be used.
        /// </param>
        public virtual void setCharacterEncoding(string characterEncoding)
        {
            this.characterEncoding = characterEncoding;
        }

        /// <summary>Get the character encoding for deserializing strings.</summary>
        /// <remarks>Get the character encoding for deserializing strings.</remarks>
        /// <returns>
        /// the encoding currently used for deserializing strings.
        /// If <code>null</code>, then the system's default encoding is used.
        /// </returns>
        public virtual string getCharacterEncoding()
        {
            return characterEncoding;
        }

        /// <summary>
        /// Encoding to use when deserializing strings or <code>null</code> if
        /// the system's default encoding should be used.
        /// </summary>
        /// <remarks>
        /// Encoding to use when deserializing strings or <code>null</code> if
        /// the system's default encoding should be used.
        /// </remarks>
        private string characterEncoding = null;
    }
}
