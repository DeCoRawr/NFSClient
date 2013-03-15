

using org.acplt.oncrpc;
namespace NFSLibrary.Protocols.V4.RPC
{
    public class uint64_t : XdrAble
    {

        public long value;


        public uint64_t()
        {
        }

        public uint64_t(long value)
        {
            this.value = value;
        }

        public uint64_t(XdrDecodingStream xdr)
        {
            xdrDecode(xdr);
        }

        public void xdrEncode(XdrEncodingStream xdr)
        {
            xdr.xdrEncodeLong(value);
        }

        public void xdrDecode(XdrDecodingStream xdr)
        {
            value = xdr.xdrDecodeLong();
        }
    }

}
