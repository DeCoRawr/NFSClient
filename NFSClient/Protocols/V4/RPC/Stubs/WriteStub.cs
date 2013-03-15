using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFSLibrary.Protocols.V4.RPC.Stubs
{
    class WriteStub
    {

        public static nfs_argop4 generateRequest(long offset, byte[] data, stateid4 stateid)
        {

            WRITE4args args = new WRITE4args();

            //enable this for sycronized stable writes
            //args.stable = stable_how4.FILE_SYNC4;

            args.stable = stable_how4.UNSTABLE4;

            args.offset = new offset4(new uint64_t(offset));

            args.stateid = stateid;

            args.data = data;

            nfs_argop4 op = new nfs_argop4();
            op.argop = nfs_opnum4.OP_WRITE;
            op.opwrite = args;

            return op;
        }

    }
}
