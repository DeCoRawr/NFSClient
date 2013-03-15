
namespace NFSLibrary.Protocols.V4.RPC.Stubs
{
    class PutrootfhStub
    {

        public static nfs_argop4 generateRequest()
        {


            nfs_argop4 op = new nfs_argop4();

            op.argop = nfs_opnum4.OP_PUTROOTFH;

            return op;

        }

    }
}
