using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFSLibrary.Protocols.V4.RPC.Stubs
{
    class CloseStub
    {
        public static nfs_argop4 generateRequest(stateid4 stateid)
        {


            CLOSE4args args = new CLOSE4args();

            args.seqid = new seqid4(new uint32_t(0));
            args.open_stateid = stateid;


            nfs_argop4 op = new nfs_argop4();

            op.argop = nfs_opnum4.OP_CLOSE;
            op.opclose = args;

            return op;

        }
    }
}
