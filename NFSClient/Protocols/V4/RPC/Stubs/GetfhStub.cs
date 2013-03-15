using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFSLibrary.Protocols.V4.RPC.Stubs
{
    class GetfhStub
    {
        public static nfs_argop4 generateRequest()
        {


            nfs_argop4 op = new nfs_argop4();

            op.argop = nfs_opnum4.OP_GETFH;

            return op;

        }
    }
}
