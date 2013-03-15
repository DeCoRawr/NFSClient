using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFSLibrary.Protocols.V4.RPC.Stubs
{
    class ReclaimCompleteStub
    {
            public static nfs_argop4 generateRequest(bool rca_one_fs2)
            {


                nfs_argop4 op = new nfs_argop4();

                op.argop = nfs_opnum4.OP_RECLAIM_COMPLETE;
                op.opreclaim_complete = new RECLAIM_COMPLETE4args();
                op.opreclaim_complete.rca_one_fs = rca_one_fs2;


                return op;

        }
    }
}
