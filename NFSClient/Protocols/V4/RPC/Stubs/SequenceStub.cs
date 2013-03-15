using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFSLibrary.Protocols.V4.RPC.Stubs
{
    class SequenceStub
    {
        public static nfs_argop4 generateRequest(bool CacheThis, byte[] SessId,
        int SeqId, int HighestSlot, int SlotId)
        {

            nfs_argop4 op = new nfs_argop4();
            op.argop = nfs_opnum4.OP_SEQUENCE;
            op.opsequence = new SEQUENCE4args();
            op.opsequence.sa_cachethis = CacheThis;

            slotid4 sId = new slotid4();
            sId.value = new uint32_t(SlotId);
            op.opsequence.sa_slotid = sId;

            slotid4 HsId = new slotid4();
            HsId.value = new uint32_t(HighestSlot);
            op.opsequence.sa_highest_slotid = HsId;

            sequenceid4 seq = new sequenceid4();
            seq.value = new uint32_t(++SeqId);
            op.opsequence.sa_sequenceid = seq;

            sessionid4 sess = new sessionid4();
            sess.value = SessId;
            op.opsequence.sa_sessionid = sess;

            return op;
        }
    }
}
