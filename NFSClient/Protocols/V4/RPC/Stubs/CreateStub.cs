using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFSLibrary.Protocols.V4.RPC.Stubs
{
    class CreateStub
    {
                public static nfs_argop4 generateRequest(String name,fattr4 attribs)
        {

          nfs_argop4 op = new nfs_argop4();
        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            op.argop = nfs_opnum4.OP_CREATE;
            op.opcreate = new CREATE4args();
            op.opcreate.objname = new component4();
            op.opcreate.objname.value = new utf8str_cs(new utf8string(encoding.GetBytes(name)));
            op.opcreate.createattrs = attribs;
            op.opcreate.objtype = new createtype4();
            //we will create only directories
            op.opcreate.objtype.type = nfs_ftype4.NF4DIR;     

            return op;
                }

    }
}
