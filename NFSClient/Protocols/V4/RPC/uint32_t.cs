

namespace NFSLibrary.Protocols.V4.RPC
{
    using org.acplt.oncrpc;
    public class uint32_t: XdrAble{
    public int value;

    public uint32_t() {
    }

    public uint32_t(int value) {
        this.value = value;
    }

    public uint32_t(XdrDecodingStream xdr){
        xdrDecode(xdr);
    }

    public void xdrEncode(XdrEncodingStream xdr){
        xdr.xdrEncodeInt(value);
    }

    public void xdrDecode(XdrDecodingStream xdr){
        value = xdr.xdrDecodeInt();
    }
    }
}
