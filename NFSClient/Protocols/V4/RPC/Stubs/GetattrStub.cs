using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.acplt.oncrpc;

namespace NFSLibrary.Protocols.V4.RPC.Stubs
{
    class GetattrStub
    {

            public static nfs_argop4 generateRequest(List<int> attrs) {


        nfs_argop4 op = new nfs_argop4();
        GETATTR4args args = new GETATTR4args();

        args.attr_request = new bitmap4();
        args.attr_request.value = new uint32_t[2];  
        args.attr_request.value[0] = new uint32_t();
        args.attr_request.value[1] = new uint32_t();

        foreach( int mask in attrs) {
            int bit = mask -(32*(mask/32));
            args.attr_request.value[mask/32].value |= 1 << bit;
        }

        op.argop = nfs_opnum4.OP_GETATTR;
        op.opgetattr = args;

        return op;

    }

   /* public static List<Integer> supportedAttrs(bitmap4 bitmap) {

        List<Integer> supported = new ArrayList<Integer>();

        // TODO:

        return supported;

    }*/


    public static Dictionary<int, Object> decodeType(fattr4 attributes)
    {

        Dictionary<int,Object> attr = new Dictionary<int, Object>();


        int[] mask = new int[attributes.attrmask.value.Length];
        for( int i = 0; i < mask.Length; i++) {
            mask[i] = attributes.attrmask.value[i].value;
        }

        XdrDecodingStream xdr = new XdrBufferDecodingStream(attributes.attr_vals.value);
        xdr.beginDecoding();

        if( mask.Length != 0 ) {
            int maxAttr = 32*mask.Length;
            for( int i = 0; i < maxAttr; i++) {
                int newmask = (mask[i/32] >> (i-(32*(i/32))) );
                if( (newmask & 1L) != 0 ) {
                    xdr2fattr(attr, i, xdr);
                }
            }
        }

        xdr.endDecoding();

        return attr;
    }


    static void xdr2fattr( Dictionary<int,Object> attr, int fattr , XdrDecodingStream xdr)
    {

        switch(fattr) {

            case NFSv4Protocol.FATTR4_SIZE :
                uint64_t size = new uint64_t();
                size.xdrDecode(xdr);
                attr.Add(fattr, size);
                break;
            case NFSv4Protocol.FATTR4_MODE :
                mode4 mode = new mode4();
                mode.xdrDecode(xdr);
                attr.Add(fattr, mode);
                break;
            case NFSv4Protocol.FATTR4_TYPE :
                fattr4_type type = new fattr4_type();
                type.xdrDecode(xdr);
                attr.Add(fattr,type );
                break;
            case NFSv4Protocol.FATTR4_TIME_CREATE :
                nfstime4 time = new nfstime4();
                time.xdrDecode(xdr);
                attr.Add(fattr,time);
                break;
           case NFSv4Protocol.FATTR4_TIME_ACCESS :
                nfstime4 time2 = new nfstime4();
                time2.xdrDecode(xdr);
                attr.Add(fattr,time2);
                break;
           case NFSv4Protocol.FATTR4_TIME_MODIFY :
                nfstime4 time3 = new nfstime4();
                time3.xdrDecode(xdr);
                attr.Add(fattr,time3);
                break;
            /*case NFSv4Protocol.FATTR4_OWNER :
// TODO: use princilat
utf8str_cs owner = new utf8str_cs ();
owner.xdrDecode(xdr);
String new_owner = new String(owner.value.value);
attr.Add(fattr,new_owner );
break;
case NFSv4Protocol.FATTR4_OWNER_GROUP :
// TODO: use princilat
utf8str_cs owner_group = new utf8str_cs ();
owner_group.xdrDecode(xdr);
String new_group = new String(owner_group.value.value);
attr.Add(fattr,new_group );
break;*/
            default:
                break;

        }


    }


    }
}


