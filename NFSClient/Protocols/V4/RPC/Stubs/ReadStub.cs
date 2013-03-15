using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFSLibrary.Protocols.V4.RPC.Stubs
{
    class ReadStub
    {
        public static nfs_argop4 generateRequest(int count, long offset, stateid4 stateid)
        {

            READ4args args = new READ4args();
            args.count = new count4(new uint32_t(count));
            args.offset = new offset4(new uint64_t(offset));

            args.stateid = stateid;

            nfs_argop4 op = new nfs_argop4();
            op.argop = nfs_opnum4.OP_READ;
            op.opread = args;

            return op;

        }
    }
}
