using System;
namespace NFSLibrary.Protocols.V4.RPC.Stubs
{
    class CreateSessionStub
    {
        public static nfs_argop4 standard(clientid4 eir_clientid,
                sequenceid4 eir_sequenceid)
        {

            nfs_argop4 op = new nfs_argop4();
            op.argop = nfs_opnum4.OP_CREATE_SESSION;
            op.opcreate_session = new CREATE_SESSION4args();
            channel_attrs4 chan_attrs = new channel_attrs4();

            chan_attrs.ca_headerpadsize = new count4(new uint32_t(0));
            chan_attrs.ca_maxoperations = new count4(new uint32_t(8));
            chan_attrs.ca_maxrequests = new count4(new uint32_t(128));
            chan_attrs.ca_maxrequestsize = new count4(new uint32_t(1049620));
            chan_attrs.ca_maxresponsesize = new count4(new uint32_t(1049480));
            chan_attrs.ca_maxresponsesize_cached = new count4(new uint32_t(2868));
            chan_attrs.ca_rdma_ird = new uint32_t[0];

            op.opcreate_session.csa_clientid = eir_clientid;
            op.opcreate_session.csa_sequence = eir_sequenceid;
            //connection back channel
            op.opcreate_session.csa_flags = new uint32_t(0);  //3 if u want to use the back channel
            op.opcreate_session.csa_fore_chan_attrs = chan_attrs;







            //diferent chan attrs for fore channel
            channel_attrs4 back_chan_attrs = new channel_attrs4();
            back_chan_attrs.ca_headerpadsize = new count4(new uint32_t(0));
            back_chan_attrs.ca_maxoperations = new count4(new uint32_t(2));
            back_chan_attrs.ca_maxrequests = new count4(new uint32_t(1));
            back_chan_attrs.ca_maxrequestsize = new count4(new uint32_t(4096));
            back_chan_attrs.ca_maxresponsesize = new count4(new uint32_t(4096));
            back_chan_attrs.ca_maxresponsesize_cached = new count4(new uint32_t(0));
            back_chan_attrs.ca_rdma_ird = new uint32_t[0];







            op.opcreate_session.csa_back_chan_attrs = back_chan_attrs;
            op.opcreate_session.csa_cb_program = new uint32_t(0x40000000);

            callback_sec_parms4[] cb = new callback_sec_parms4[1];
            callback_sec_parms4 callb = new callback_sec_parms4();
            
            //new auth_sys params
            callb.cb_secflavor = auth_flavor.AUTH_SYS;
            callb.cbsp_sys_cred = new authsys_parms();
            Random r = new Random();
            callb.cbsp_sys_cred.stamp = r.Next(); //just random number
            callb.cbsp_sys_cred.gid = 0; //maybe root ?
            callb.cbsp_sys_cred.uid = 0; //maybe root ?

            callb.cbsp_sys_cred.machinename = System.Environment.MachineName;
            callb.cbsp_sys_cred.gids = new int[0];


            //callb.cb_secflavor = auth_flavor.AUTH_NONE;


            cb[0] = callb;
           // op.opcreate_session.csa_sec_parms = new callback_sec_parms4[1];
            op.opcreate_session.csa_sec_parms = cb;
            return op;
        }

    }
}
