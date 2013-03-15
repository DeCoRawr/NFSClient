

using System;
namespace NFSLibrary.Protocols.V4.RPC.Stubs
{
    class ExchengeIDStub
    {


        public static nfs_argop4 normal(string nii_domain, string nii_name,
        string co_ownerid, int flags, int how)
        {
            //for transormation
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();


            nfs_argop4 op = new nfs_argop4();
            op.argop = nfs_opnum4.OP_EXCHANGE_ID;
            op.opexchange_id = new EXCHANGE_ID4args();


           op.opexchange_id.eia_client_impl_id = new nfs_impl_id4[1];
            nfs_impl_id4 n4 = new nfs_impl_id4();
            n4.nii_domain = new utf8str_cis(new utf8string(encoding.GetBytes(nii_domain)));
           n4.nii_name = new utf8str_cs(new utf8string(encoding.GetBytes(nii_name)));
            op.opexchange_id.eia_client_impl_id[0] = n4;

            nfstime4 releaseDate = new nfstime4();
            releaseDate.nseconds = new uint32_t(0);
            releaseDate.seconds = new int64_t((long)(DateTime.UtcNow - new DateTime
    (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);  //seconds here

            op.opexchange_id.eia_client_impl_id[0].nii_date = releaseDate;
            op.opexchange_id.eia_clientowner = new client_owner4();

            op.opexchange_id.eia_clientowner.co_ownerid = encoding.GetBytes(co_ownerid);

            op.opexchange_id.eia_clientowner.co_verifier = new verifier4();
            op.opexchange_id.eia_clientowner.co_verifier.value = releaseDate.seconds.value;   //new byte[NFSv4Protocol.NFS4_VERIFIER_SIZE];

            //byte[] locVerifier = encoding.GetBytes(releaseDate.seconds.value.ToString("X"));


            //int len = locVerifier.Length > NFSv4Protocol.NFS4_VERIFIER_SIZE ? NFSv4Protocol.NFS4_VERIFIER_SIZE : locVerifier.Length;
           // Array.Copy(locVerifier, 0, op.opexchange_id.eia_clientowner.co_verifier.value, 0, len);

            op.opexchange_id.eia_flags = new uint32_t(flags);
            op.opexchange_id.eia_state_protect = new state_protect4_a();
            op.opexchange_id.eia_state_protect.spa_how = how;
            return op;
        }
    }
}
