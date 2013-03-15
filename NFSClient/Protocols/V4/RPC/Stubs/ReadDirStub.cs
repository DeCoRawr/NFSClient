using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFSLibrary.Protocols.V4.RPC.Stubs
{
    class ReadDirStub
    {
        public static nfs_argop4 generateRequest(long cookie, verifier4 verifier)
        {


            nfs_argop4 op = new nfs_argop4();
            op.opreaddir = new READDIR4args();
            op.opreaddir.cookie = new nfs_cookie4(new uint64_t(cookie));
            op.opreaddir.dircount = new count4(new uint32_t(10000));
            op.opreaddir.maxcount = new count4(new uint32_t(10000));
            op.opreaddir.attr_request = new bitmap4(new uint32_t[] { new uint32_t(0), new uint32_t(0) });
            op.opreaddir.cookieverf = verifier;

            op.argop = nfs_opnum4.OP_READDIR;

            return op;

        }
    }
}
