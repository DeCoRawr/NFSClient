using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFSLibrary.Protocols.V4.RPC.Stubs
{
    class RemoveStub
    {
        public static nfs_argop4 generateRequest(String path)
        {

            REMOVE4args args = new REMOVE4args();

            args.target = new component4();
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            args.target.value = new utf8str_cs(new utf8string(encoding.GetBytes(path)));

            nfs_argop4 op = new nfs_argop4();

            op.argop = nfs_opnum4.OP_REMOVE;
            op.opremove = args;

            return op;

        }
    }
}
