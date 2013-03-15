using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFSLibrary.Protocols.V4.RPC.Stubs
{
    class LookupStub
    {
        public static nfs_argop4 generateRequest(String path)
        {
            nfs_argop4 op = new nfs_argop4();
            op.argop = nfs_opnum4.OP_LOOKUP;
            op.oplookup = new LOOKUP4args();

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] bytes = encoding.GetBytes(path);

            op.oplookup.objname = new component4(new utf8str_cs(new utf8string(bytes)));

            return op;
        }
    }
}
