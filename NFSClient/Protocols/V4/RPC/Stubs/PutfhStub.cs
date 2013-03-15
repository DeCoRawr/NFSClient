
namespace NFSLibrary.Protocols.V4.RPC.Stubs
{
    class PutfhStub
    {

    public static nfs_argop4 generateRequest( nfs_fh4 fh) {


        nfs_argop4 op = new nfs_argop4();
        op.opputfh = new PUTFH4args();
        op.opputfh.object1 = fh;

        op.argop = nfs_opnum4.OP_PUTFH;

        return op;

    }


    }
}
