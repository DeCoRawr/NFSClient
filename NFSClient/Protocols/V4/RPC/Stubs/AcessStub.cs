
namespace NFSLibrary.Protocols.V4.RPC.Stubs
{
    class AcessStub
    {

    public static nfs_argop4 generateRequest(uint32_t acessargs) {


        nfs_argop4 op = new nfs_argop4();
        op.argop = nfs_opnum4.OP_ACCESS;

        op.opaccess = new ACCESS4args();
        op.opaccess.access = acessargs;

        return op;

    }


    }
}
