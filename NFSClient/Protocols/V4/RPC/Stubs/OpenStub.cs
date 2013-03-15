using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.acplt.oncrpc;

namespace NFSLibrary.Protocols.V4.RPC.Stubs
{
    class OpenStub
    {


        public static nfs_argop4 normalREAD(String path, int sequenceId,
        clientid4 clientid, int access)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            nfs_argop4 op = new nfs_argop4();
            op.argop = nfs_opnum4.OP_OPEN;
            op.opopen = new OPEN4args();

            op.opopen.seqid = new seqid4(new uint32_t(sequenceId));

            // if ((access & nfs4_prot.OPEN4_SHARE_ACCESS_WANT_DELEG_MASK) == 0){
            // access |= nfs4_prot.OPEN4_SHARE_ACCESS_WANT_NO_DELEG;
            // }
            // op.opopen.share_access = new uint32_t(access);
            op.opopen.share_access = new uint32_t(NFSv4Protocol.OPEN4_SHARE_ACCESS_READ);
            op.opopen.share_deny = new uint32_t(NFSv4Protocol.OPEN4_SHARE_DENY_NONE);

            state_owner4 owner = new state_owner4();
            owner.clientid = clientid;

            owner.owner = encoding.GetBytes("nfsclient");
            op.opopen.owner = new open_owner4(owner);

            openflag4 flag = new openflag4();
            flag.opentype = opentype4.OPEN4_NOCREATE;

            createhow4 how = new createhow4();
            how.mode = createmode4.UNCHECKED4;
            flag.how = how;
            op.opopen.openhow = flag;

            open_claim4 claim = new open_claim4();
            claim.claim = open_claim_type4.CLAIM_NULL;
            claim.file = new component4(new utf8str_cs(new utf8string(encoding.GetBytes(path))));
            claim.delegate_type = NFSv4Protocol.OPEN4_SHARE_ACCESS_WANT_NO_DELEG;
            claim.file_delegate_prev = null;
            claim.oc_delegate_stateid = null;
            claim.delegate_type = 0;
            claim.delegate_cur_info = null;

            op.opopen.claim = claim;

            return op;
        }




        public static nfs_argop4 normalCREATE(String path, int sequenceId,
        clientid4 clientid, int access)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            nfs_argop4 op = new nfs_argop4();
            op.argop = nfs_opnum4.OP_OPEN;
            op.opopen = new OPEN4args();

            op.opopen.seqid = new seqid4(new uint32_t(sequenceId));

            state_owner4 owner = new state_owner4();
            owner.clientid = clientid;
            owner.owner = encoding.GetBytes("nfsclient");
            op.opopen.owner = new open_owner4(owner);

            //if ((access & NFSv4Protocol.OPEN4_SHARE_ACCESS_WANT_DELEG_MASK) == 0)
           // {
           //     access |= NFSv4Protocol.OPEN4_SHARE_ACCESS_WANT_NO_DELEG;
           // }
            op.opopen.share_access = new uint32_t(access);
            op.opopen.share_deny = new uint32_t(NFSv4Protocol.OPEN4_SHARE_DENY_NONE);

            openflag4 flag = new openflag4();
            flag.opentype = opentype4.OPEN4_CREATE;

            // createhow4(mode, attrs, verifier)
            createhow4 how = new createhow4();
            how.mode = createmode4.GUARDED4;
            //how.mode = createmode4.EXCLUSIVE4_1;
            fattr4 attr = new fattr4();

            attr.attrmask = openFattrBitmap();
            attr.attr_vals = new attrlist4();
            attr.attr_vals.value = openAttrs(7,7,7,0);

            how.createattrs = attr;
            how.createverf = new verifier4(0);  //it is long
            //how.mode = createmode4.GUARDED4;

            flag.how = how;
            op.opopen.openhow = flag;

            open_claim4 claim = new open_claim4();
            claim.claim = open_claim_type4.CLAIM_NULL;
            claim.file = new component4(new utf8str_cs(new utf8string(encoding
                    .GetBytes(path))));
            claim.delegate_type = NFSv4Protocol.OPEN4_SHARE_ACCESS_WANT_NO_DELEG;
            claim.file_delegate_prev = null;
            claim.oc_delegate_stateid = null;
            claim.delegate_type = 0;
            claim.delegate_cur_info = null;

            op.opopen.claim = claim;

            return op;
        }




        public static nfs_argop4 normalOPENonly(String path, int sequenceId,
 clientid4 clientid, int access)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            nfs_argop4 op = new nfs_argop4();
            op.argop = nfs_opnum4.OP_OPEN;
            op.opopen = new OPEN4args();

            op.opopen.seqid = new seqid4(new uint32_t(sequenceId));

            state_owner4 owner = new state_owner4();
            owner.clientid = clientid;
            owner.owner = encoding.GetBytes("nfsclient");
            op.opopen.owner = new open_owner4(owner);

            //if ((access & NFSv4Protocol.OPEN4_SHARE_ACCESS_WANT_DELEG_MASK) == 0)
            // {
            //     access |= NFSv4Protocol.OPEN4_SHARE_ACCESS_WANT_NO_DELEG;
            // }
            op.opopen.share_access = new uint32_t(access);
            op.opopen.share_deny = new uint32_t(NFSv4Protocol.OPEN4_SHARE_DENY_NONE);

            openflag4 flag = new openflag4();
            flag.opentype = opentype4.OPEN4_NOCREATE;

            // createhow4(mode, attrs, verifier)
            createhow4 how = new createhow4();
            how.mode = createmode4.UNCHECKED4;
            //how.mode = createmode4.EXCLUSIVE4_1;
            fattr4 attr = new fattr4();

            attr.attrmask = openFattrBitmap();
            attr.attr_vals = new attrlist4();
            attr.attr_vals.value = openAttrs(7, 7, 7, 0);

            how.createattrs = attr;
            how.createverf = new verifier4(0);  //it is long
            //how.mode = createmode4.GUARDED4;

            flag.how = how;
            op.opopen.openhow = flag;

            open_claim4 claim = new open_claim4();
            claim.claim = open_claim_type4.CLAIM_NULL;
            claim.file = new component4(new utf8str_cs(new utf8string(encoding
                    .GetBytes(path))));
            claim.delegate_type = NFSv4Protocol.OPEN4_SHARE_ACCESS_WANT_NO_DELEG;
            claim.file_delegate_prev = null;
            claim.oc_delegate_stateid = null;
            claim.delegate_type = 0;
            claim.delegate_cur_info = null;

            op.opopen.claim = claim;

            return op;
        }







        public static bitmap4 openFattrBitmap()
        {

            List<int> attrs = new List<int>();

            //for dir we don't need this
            attrs.Add(NFSv4Protocol.FATTR4_SIZE);
            attrs.Add(NFSv4Protocol.FATTR4_MODE);


            bitmap4 afttrBitmap = new bitmap4();
            //changed to 1
            afttrBitmap.value = new uint32_t[2];
            afttrBitmap.value[0] = new uint32_t();
            afttrBitmap.value[1] = new uint32_t();

            foreach (int mask in attrs)
            {
                int bit;
                uint32_t bitmap;
                if (mask > 31)
                {
                    bit = mask - 32;
                    bitmap = afttrBitmap.value[1];
                }
                else
                {
                    bit = mask;
                    bitmap = afttrBitmap.value[0];
                }

                bitmap.value |= 1 << bit;

            }

            return afttrBitmap;
        }


        public static byte[] openAttrs(int user, int group, int other,long sizea)
        {

            XdrBufferEncodingStream xdr = new XdrBufferEncodingStream(1024);


            //starts encoding
            xdr.beginEncoding(null, 0);

            user = 7 << 6;
            group = 7 << 3;
            other = 7;

            mode4 fmode = new mode4();
            fmode.value = new uint32_t(group + user + other);
            fattr4_mode mode = new fattr4_mode(fmode);

            fattr4_size size = new fattr4_size(new uint64_t(sizea));

            size.xdrEncode(xdr);
            mode.xdrEncode(xdr);


            xdr.endEncoding();
            //end encoding

            byte[] retBytes = new byte[xdr.getXdrLength()];



            Array.Copy(xdr.getXdrData(), 0, retBytes, 0, xdr.getXdrLength());

            return retBytes;
        }





    }
}
