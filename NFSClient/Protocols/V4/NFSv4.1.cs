using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using NFSLibrary.Protocols.Commons;
using NFSLibrary.Protocols.Commons.Exceptions;
using NFSLibrary.Protocols.Commons.Exceptions.Mount;
using NFSLibrary.Protocols.V4.RPC;
using org.acplt.oncrpc;
using NFSLibrary.Protocols.V4.RPC.Stubs;
using NFSLibrary.Protocols.V4.RPC.Callback;
using org.acplt.oncrpc.server;
using System.Timers;
using System.Collections;

namespace NFSLibrary.Protocols.V4
{
    public class NFSv4 : INFS
    {
        #region Fields


        private NFSv4ProtocolClient _ProtocolV4 = null;

       // private String _MountedDevice = String.Empty;
        private String _CurrentItem = String.Empty;

        private int _GroupID = -1;
        private int _UserID = -1;



        private nfs_fh4 _rootFH = null;


        private clientid4 _clientIdByServer = null;
        private sequenceid4 _sequenceID = null;
        private sessionid4 _sessionid = null;


        private bool useFHCache = false;
        private Hashtable cached_attrs = null;

        //private Dictionary<string, NFSAttributes> cached_attrs = null;

        //create timerz
        Timer timer = null;


        //**************back to the roots*********************
        
        
        //current dir handle
        //private nfs_fh4 _cwd = null;
        //current file handle  (read ,write)
        private nfs_fh4 _cwf = null;
        //before write dir handle and other
        //private nfs_fh4 _cWwf = null;
        //private string _beforeWritePath = null;

        private stateid4 currentState = null; 
        //private string _currentFolder;

        //private List<nfs_fh4>  _cwhTree;
        //private int treePosition = 0;


        private long _lastUpdate = -1;

        private int maxTRrate;
        private int maxRXrate;

        #endregion

        #region Constructur

        public void Connect(IPAddress Address, int UserID, int GroupID, int ClientTimeout, System.Text.Encoding characterEncoding, bool useSecurePort,bool useCache)
        {
            if (ClientTimeout == 0)
            { ClientTimeout = 60000; }

            if (characterEncoding == null)
            { characterEncoding = System.Text.Encoding.ASCII; }

            _CurrentItem = String.Empty;

            useFHCache = useCache;
            

            if(useFHCache)
            cached_attrs = new Hashtable();//new Dictionary<string, NFSAttributes>();
            
            _rootFH = null;

            //_cwd = null;

            _cwf = null;

            //_cwhTree = new List<nfs_fh4>();
            //treePosition = 0;

            _GroupID = GroupID;
            _UserID = UserID;

            _ProtocolV4 = new NFSv4ProtocolClient(Address, OncRpcProtocols.ONCRPC_TCP, useSecurePort);

            OncRpcClientAuthUnix authUnix = new OncRpcClientAuthUnix(Address.ToString(), UserID, GroupID);

            _ProtocolV4.GetClient().setAuth(authUnix);
            _ProtocolV4.GetClient().setTimeout(ClientTimeout);
            _ProtocolV4.GetClient().setCharacterEncoding(characterEncoding.WebName);

            //send null dummy procedure to see if server is responding
            sendNullPRocedure();

        }

        #endregion

        #region Public Methods

        public void Disconnect()
        {
            //_RootDirectoryHandleObject = null;
            //_CurrentItemHandleObject = null;

            //_MountedDevice = String.Empty;
            _CurrentItem = String.Empty;


            if (_ProtocolV4 != null)
            {
                //new thingy in nfs 4.1
                destroy_session();

                if(timer!=null)
                timer.Close();

                //not supported in major linux distibutions
                //destroy_clientId();
                _ProtocolV4.close();
            }
        }

        public int GetBlockSize()
        {
            if (maxTRrate > 7900 && maxRXrate > 7900)
            {
                if (maxTRrate > 65236 && maxRXrate > 65236)
                    return 65236;
                else if (maxTRrate == maxRXrate)
                    return maxTRrate-200;
                else if (maxTRrate < maxRXrate)
                    return maxTRrate-200;
                else
                    return maxRXrate-200;
            }
            else 
                return 7900;
        }

        public List<String> GetExportedDevices()
        {

            List<string> nfsDevices = new List<string>();
            nfsDevices.Add("/");

            //will mount here because we need to mount only once and devices are not possible to get
            //call first mount
            mount();
            //_currentFolder = ".";

            return nfsDevices;
        }

        public void MountDevice(String DeviceName)
        {
            if (_ProtocolV4 == null)
            { throw new NFSConnectionException("NFS Client not connected!"); }

            getRootFh();
            reclaim_complete();
        }

        public void UnMountDevice()
        {
                     //maybe we need something to do here also
        }

        public List<String> GetItemList(String DirectoryFullName)
        {
            if (_ProtocolV4 == null)
            { throw new NFSConnectionException("NFS Client not connected!"); }

            /*if (DirectoryFullName == ".")
            {
                //root
                _cwd = _rootFH;
                _cwhTree.Add(_cwd);
            }
            else if (_currentFolder == DirectoryFullName)
            {
                //do nothing just recheck folder
            }
            else if (DirectoryFullName.IndexOf(_currentFolder) != -1)
            {
                //one level up

                NFSAttributes itemAttributes = GetItemAttributes(DirectoryFullName);
                _cwd = itemAttributes.fh;
                _cwhTree.Add(_cwd);
                treePosition++;
            }
            else
            {
                //level down
                _cwhTree.RemoveAt(treePosition);
                treePosition--;
                _cwd =_cwhTree[treePosition];
            }*/

           //_currentFolder = DirectoryFullName;
            if (String.IsNullOrEmpty(DirectoryFullName) || DirectoryFullName == ".")
                return GetItemListByFH(_rootFH);

            //simpler way than tree
            NFSAttributes itemAttributes =  GetItemAttributes(DirectoryFullName);

            //true dat
            return GetItemListByFH(new nfs_fh4(itemAttributes.Handle));
        }



        public List<String> GetItemListByFH(nfs_fh4 dir_fh)
        {

            //should return result
            int acess = get_fh_acess(dir_fh);


            List<string> ItemsList = new List<string>();

            //has read acess
            if (acess % 2 == 1)
            {

                bool done = false;
                long cookie = 0;

                verifier4 verifier = new verifier4(0);

                do
                {

                    List<nfs_argop4> ops = new List<nfs_argop4>();
                    ops.Add(SequenceStub.generateRequest(false, _sessionid.value,
                            _sequenceID.value.value, 12, 0));
                    ops.Add(PutfhStub.generateRequest(dir_fh));
                    ops.Add(ReadDirStub.generateRequest(cookie, verifier));

                    COMPOUND4res compound4res = sendCompound(ops, "");

                    if (compound4res.status == nfsstat4.NFS4_OK)
                    {
                        verifier = compound4res.resarray[2].opreaddir.resok4.cookieverf;
                        done = compound4res.resarray[2].opreaddir.resok4.reply.eof;

                        entry4 dirEntry = compound4res.resarray[2].opreaddir.resok4.reply.entries;
                        while (dirEntry != null)
                        {
                            cookie = dirEntry.cookie.value.value;
                            string name = System.Text.Encoding.UTF8.GetString(dirEntry.name.value.value.value);
                            ItemsList.Add(name);
                            dirEntry = dirEntry.nextentry;
                        }

                    }
                    else
                    {
                        throw new NFSGeneralException(nfsstat4.getErrorString(compound4res.status));

                    }

                } while (!done);
                //now do the lookups (maintained by the nfsclient)
            }
            else
                throw new NFSGeneralException(nfsstat4.getErrorString(nfsstat4.NFS4ERR_ACCESS));


            return ItemsList;
        }




        public NFSAttributes GetItemAttributes(string ItemFullName, bool ThrowExceptionIfNotFound = true)
        {
            if (_ProtocolV4 == null)
            { throw new NFSConnectionException("NFS Client not connected!"); }

            ItemFullName = ItemFullName.Replace(".\\.\\", ".\\");

            if (useFHCache)
            if (cached_attrs.ContainsKey(ItemFullName))
                return (NFSAttributes)cached_attrs[ItemFullName];
           
            //we will return it in the old way !! ;)
            NFSAttributes attributes = null;

            if (String.IsNullOrEmpty(ItemFullName))
            {
                //should not happen
                return attributes;
            }


            if(ItemFullName==".\\.")
                return new NFSAttributes(0, 0, 0, NFSItemTypes.NFDIR, new NFSPermission(7, 7, 7), 4096, _rootFH.value);

            

            nfs_fh4 currentItem = _rootFH;
            int initial = 1;
            String[] PathTree = ItemFullName.Split(@"\".ToCharArray());

            if (useFHCache)
            {
                string parent = System.IO.Path.GetDirectoryName(ItemFullName);
                //get cached parent dir to avoid too much directory
                if (parent != ItemFullName)
                    if (cached_attrs.ContainsKey(parent))
                    {
                        currentItem.value = ((NFSAttributes)cached_attrs[parent]).Handle;
                        initial = PathTree.Length - 1;
                    }
            }


            for (int pC = initial; pC < PathTree.Length; pC++)
            {
                List<int> attrs = new List<int>(1);
                attrs.Add(NFSv4Protocol.FATTR4_TIME_CREATE);
                attrs.Add(NFSv4Protocol.FATTR4_TIME_ACCESS);
                attrs.Add(NFSv4Protocol.FATTR4_TIME_MODIFY);
                attrs.Add(NFSv4Protocol.FATTR4_TYPE);
                attrs.Add(NFSv4Protocol.FATTR4_MODE);
                attrs.Add(NFSv4Protocol.FATTR4_SIZE);

                List<nfs_argop4> ops = new List<nfs_argop4>();

                ops.Add(SequenceStub.generateRequest(false, _sessionid.value,
                        _sequenceID.value.value, 12, 0));

                ops.Add(PutfhStub.generateRequest(currentItem));
                ops.Add(LookupStub.generateRequest(PathTree[pC]));

                //ops.Add(PutfhStub.generateRequest(_cwd));
                //ops.Add(LookupStub.generateRequest(PathTree[PathTree.Length-1]));
                
                ops.Add(GetfhStub.generateRequest());
                ops.Add(GetattrStub.generateRequest(attrs));

                COMPOUND4res compound4res = sendCompound(ops, "");

                if (compound4res.status == nfsstat4.NFS4_OK)
                {

                    currentItem = compound4res.resarray[3].opgetfh.resok4.object1;

                    //nfs_fh4 currentItem = compound4res.resarray[3].opgetfh.resok4.object1;

                    //results
                    Dictionary<int, Object> attrrs_results = GetattrStub.decodeType(compound4res.resarray[4].opgetattr.resok4.obj_attributes);

                    //times
                    nfstime4 time_acc = (nfstime4)attrrs_results[NFSv4Protocol.FATTR4_TIME_ACCESS];

                    int time_acc_int = unchecked((int)time_acc.seconds.value);

                    nfstime4 time_modify = (nfstime4)attrrs_results[NFSv4Protocol.FATTR4_TIME_MODIFY];

                    int time_modif = unchecked((int)time_modify.seconds.value);


                    int time_creat = 0;
                    //linux should now store create time if it is let's check it else use modify date
                    if (attrrs_results.ContainsKey(NFSv4Protocol.FATTR4_TIME_CREATE))
                    {
                        nfstime4 time_create = (nfstime4)attrrs_results[NFSv4Protocol.FATTR4_TIME_CREATE];

                        time_creat = unchecked((int)time_create.seconds.value);
                    }
                    else
                        time_creat = time_modif;



                    //3 = type
                    NFSItemTypes nfstype = NFSItemTypes.NFREG;

                    fattr4_type type = (fattr4_type)attrrs_results[NFSv4Protocol.FATTR4_TYPE];

                    if (type.value == 2)
                        nfstype = NFSItemTypes.NFDIR;

                    //4 = mode is int also
                    mode4 mode = (mode4)attrrs_results[NFSv4Protocol.FATTR4_MODE];

                    byte other = (byte)(mode.value.value % 8);

                    byte grup = (byte)((mode.value.value >> 3) % 8);

                    byte user = (byte)((mode.value.value >> 6) % 8);

                    NFSPermission per = new NFSPermission(user, grup, other);


                    uint64_t size = (uint64_t)attrrs_results[NFSv4Protocol.FATTR4_SIZE];
                    //here we do attributes compatible with old nfs versions
                    attributes = new NFSAttributes(time_creat, time_acc_int, time_modif, nfstype, per, size.value, currentItem.value);

                }
                else if (compound4res.status == nfsstat4.NFS4ERR_NOENT)
                {
                    return null;
                }

                else
                {

                    throw new NFSConnectionException(nfsstat4.getErrorString(compound4res.status));
                }
            }

           // if(attributes.NFSType == NFSItemTypes.NFDIR)
            if(useFHCache)
            cached_attrs.Add(ItemFullName, attributes);

            return attributes;
        }

        public void CreateDirectory(string DirectoryFullName, NFSPermission Mode)
        {
            if (_ProtocolV4 == null)
            { throw new NFSConnectionException("NFS Client not connected!"); }


            int user = 7;
            int group = 7;
            int other = 7;

            if (Mode != null)
            {
               user = Mode.UserAccess;
               group = Mode.GroupAccess;
               other = Mode.OtherAccess;
            }


            string ParentDirectory = System.IO.Path.GetDirectoryName(DirectoryFullName);
            string fileName = System.IO.Path.GetFileName(DirectoryFullName);
            NFSAttributes ParentItemAttributes = GetItemAttributes(ParentDirectory);

            //create item attributes now
           fattr4 attr = new fattr4();


           attr.attrmask = OpenStub.openFattrBitmap();
           attr.attr_vals = new attrlist4();
           attr.attr_vals.value = OpenStub.openAttrs(user,group,other,4096);



            List<nfs_argop4> ops = new List<nfs_argop4>();

            ops.Add(SequenceStub.generateRequest(false, _sessionid.value,
                    _sequenceID.value.value, 12, 0));
            ops.Add(PutfhStub.generateRequest(new nfs_fh4(ParentItemAttributes.Handle)));
            ops.Add(CreateStub.generateRequest(fileName,attr));

            COMPOUND4res compound4res = sendCompound(ops, "");

            if (compound4res.status == nfsstat4.NFS4_OK)
            {
                //create directory ok
            }
            else { throw new NFSConnectionException(nfsstat4.getErrorString(compound4res.status)); }
        }


        public void DeleteDirectory(string DirectoryFullName)
        {
            //nfs 4.1 now uses same support for folders and files
            DeleteFile(DirectoryFullName);
        }

        public void DeleteFile(string FileFullName)
        {
            if (_ProtocolV4 == null)
            { throw new NFSConnectionException("NFS Client not connected!"); }


            // it seems acess doesent work ok in the server
            //delete acess isn't showed but file can be deleted and should be deleted!
            /*NFSAttributes atrs = GetItemAttributes(FileFullName);

          int acess = get_fh_acess(atrs.fh);

             //delete support
          if ((acess >> 4) % 2 == 1)
          {*/


            string ParentDirectory = System.IO.Path.GetDirectoryName(FileFullName);
            string fileName = System.IO.Path.GetFileName(FileFullName);

            NFSAttributes ParentItemAttributes = GetItemAttributes(ParentDirectory);


            List<nfs_argop4> ops = new List<nfs_argop4>();

            ops.Add(SequenceStub.generateRequest(false, _sessionid.value,
                    _sequenceID.value.value, 12, 0));
            ops.Add(PutfhStub.generateRequest(new nfs_fh4(ParentItemAttributes.Handle)));
            ops.Add(RemoveStub.generateRequest(fileName));

            COMPOUND4res compound4res = sendCompound(ops, "");

            if (compound4res.status == nfsstat4.NFS4_OK)
            {
                //ok - deleted
            }
            else { throw new NFSConnectionException(nfsstat4.getErrorString(compound4res.status)); }


            /* }
             else
             {
                 throw new NFSGeneralException("Acess Denied");
             }*/


            //only if we support caching
             if(useFHCache)
            cached_attrs.Remove(FileFullName);

        }

        public void CreateFile(string FileFullName, NFSPermission Mode)
        {
            if (_CurrentItem != FileFullName)
            {
                _CurrentItem = FileFullName;


                String[] PathTree = FileFullName.Split(@"\".ToCharArray());

                string ParentDirectory = System.IO.Path.GetDirectoryName(FileFullName);
                NFSAttributes ParentAttributes = GetItemAttributes(ParentDirectory);





                //make open here
                List<nfs_argop4> ops = new List<nfs_argop4>();
                ops.Add(SequenceStub.generateRequest(false, _sessionid.value,
            _sequenceID.value.value, 12, 0));
                //dir  herez
                ops.Add(PutfhStub.generateRequest(new nfs_fh4(ParentAttributes.Handle)));
                //let's try with sequence 0
                ops.Add(OpenStub.normalCREATE(PathTree[PathTree.Length - 1], _sequenceID.value.value, _clientIdByServer, NFSv4Protocol.OPEN4_SHARE_ACCESS_WRITE));
                ops.Add(GetfhStub.generateRequest());


                COMPOUND4res compound4res = sendCompound(ops, "");
                if (compound4res.status == nfsstat4.NFS4_OK)
                {
                    //open ok
                    currentState = compound4res.resarray[2].opopen.resok4.stateid;

                    _cwf = compound4res.resarray[3].opgetfh.resok4.object1;


                }
                else
                {
                    throw new NFSConnectionException(nfsstat4.getErrorString(compound4res.status));
                }

            }
        }

        public int Read(String FileFullName, long Offset, int Count, ref Byte[] Buffer)
        {
            if (_ProtocolV4 == null)
            { throw new NFSConnectionException("NFS Client not connected!"); }

            int rCount = 0;

            if (Count == 0)
               return 0;
            
           
            if (_CurrentItem != FileFullName)
            {
                NFSAttributes Attributes = GetItemAttributes(FileFullName);
                _cwf = new nfs_fh4(Attributes.Handle);
                _CurrentItem = FileFullName;

                string ParentDirectory = System.IO.Path.GetDirectoryName(FileFullName);
                NFSAttributes ParentAttributes = GetItemAttributes(ParentDirectory);

                String[] PathTree = FileFullName.Split(@"\".ToCharArray());


                //make open here
                List<nfs_argop4> ops = new List<nfs_argop4>();
                ops.Add(SequenceStub.generateRequest(false, _sessionid.value,
            _sequenceID.value.value, 12, 0));
                //dir  herez
                //ops.Add(PutfhStub.generateRequest(_cwd));
                ops.Add(PutfhStub.generateRequest(new nfs_fh4(ParentAttributes.Handle)));
                //let's try with sequence 0
                ops.Add(OpenStub.normalREAD(PathTree[PathTree.Length-1],0,_clientIdByServer,NFSv4Protocol.OPEN4_SHARE_ACCESS_READ));


                COMPOUND4res compound4res = sendCompound(ops, "");
                if (compound4res.status == nfsstat4.NFS4_OK)
                {
                    //open ok
                     currentState = compound4res.resarray[2].opopen.resok4.stateid;
                }
                else
                {
                    throw new NFSConnectionException(nfsstat4.getErrorString(compound4res.status));
                }



                //check the acess also
                if (get_fh_acess(_cwf) % 2 != 1)
                {
                    //we don't have read acess give error
                    throw new NFSConnectionException("Sorry no file READ acess !!!");
                }
   
            }
            
            List<nfs_argop4> ops2 = new List<nfs_argop4>();
            ops2.Add(SequenceStub.generateRequest(false, _sessionid.value,
        _sequenceID.value.value, 12, 0));
            ops2.Add(PutfhStub.generateRequest(_cwf));
            ops2.Add(ReadStub.generateRequest(Count,Offset,currentState));



            COMPOUND4res compound4res2 = sendCompound(ops2, "");
            if (compound4res2.status == nfsstat4.NFS4_OK)
            {
                //read of offset complete
                rCount = compound4res2.resarray[2].opread.resok4.data.Length;

                ///copy the data to the output
                Array.Copy(compound4res2.resarray[2].opread.resok4.data, Buffer, rCount);

            }
            else
            {
                throw new NFSConnectionException(nfsstat4.getErrorString(compound4res2.status));
            }

            return rCount;
        }




        private void closeFile(nfs_fh4 fh,stateid4 stateid)
        {
         List<nfs_argop4> ops = new List<nfs_argop4>();
            
        ops.Add(SequenceStub.generateRequest(false, _sessionid.value,
                _sequenceID.value.value, 12, 0));

        ops.Add(PutfhStub.generateRequest(fh));
        ops.Add(CloseStub.generateRequest(stateid) );

        COMPOUND4res compound4res = sendCompound(ops, "");

        if (compound4res.status == nfsstat4.NFS4_OK) {
            //close file ok
        }
        else {
            throw new NFSConnectionException(nfsstat4.getErrorString(compound4res.status));
        }
        }


        public void SetFileSize(string FileFullName, long Size)
        {
            if (_ProtocolV4 == null)
            { throw new NFSConnectionException("NFS Client not connected!"); }

            /* NFSAttributes Attributes = GetItemAttributes(FileFullName);

             SetAttributeArguments dpArgSAttr = new SetAttributeArguments();

             dpArgSAttr.Handle = new NFSHandle(Attributes.Handle, V3.RPC.NFSv3Protocol.NFS_V3);
             dpArgSAttr.Attributes = new MakeAttributes();
             dpArgSAttr.Attributes.LastAccessedTime = new NFSTimeValue();
             dpArgSAttr.Attributes.ModifiedTime = new NFSTimeValue();
             dpArgSAttr.Attributes.Mode = Attributes.Mode;
             dpArgSAttr.Attributes.UserID = -1;
             dpArgSAttr.Attributes.GroupID = -1;
             dpArgSAttr.Attributes.Size = Size;
             dpArgSAttr.GuardCreateTime = new NFSTimeValue();
             dpArgSAttr.GuardCheck = false;

             ResultObject<SetAttributeAccessOK, SetAttributeAccessFAIL> pAttrStat =
                 _ProtocolV3.NFSPROC3_SETATTR(dpArgSAttr);

             if (pAttrStat == null || pAttrStat.Status != NFSStats.NFS_OK)
             {
                 if (pAttrStat == null)
                 { throw new NFSGeneralException("NFSPROC3_SETATTR: failure"); }

                 ExceptionHelpers.ThrowException(pAttrStat.Status);
             }*/
        }

        public int Write(String FileFullName, long Offset, int Count, Byte[] Buffer)
        {
            if (_ProtocolV4 == null)
            { throw new NFSConnectionException("NFS Client not connected!"); }


            int rCount = 0;
            //nfs_fh4 current = _cwd;


            
            if (_CurrentItem != FileFullName)
            {

                _CurrentItem = FileFullName;


                String[] PathTree = FileFullName.Split(@"\".ToCharArray());

                string ParentDirectory = System.IO.Path.GetDirectoryName(FileFullName);
                NFSAttributes ParentAttributes = GetItemAttributes(ParentDirectory);


                //make open here
                List<nfs_argop4> ops = new List<nfs_argop4>();
                ops.Add(SequenceStub.generateRequest(false, _sessionid.value,
            _sequenceID.value.value, 12, 0));
                //dir  herez
                ops.Add(PutfhStub.generateRequest(new nfs_fh4(ParentAttributes.Handle)));
                //let's try with sequence 0
                ops.Add(OpenStub.normalOPENonly(PathTree[PathTree.Length - 1], _sequenceID.value.value, _clientIdByServer, NFSv4Protocol.OPEN4_SHARE_ACCESS_WRITE));
                ops.Add(GetfhStub.generateRequest());


                COMPOUND4res compound4res = sendCompound(ops, "");
                if (compound4res.status == nfsstat4.NFS4_OK)
                {
                    //open ok
                    currentState = compound4res.resarray[2].opopen.resok4.stateid;

                    _cwf = compound4res.resarray[3].opgetfh.resok4.object1;


                }
                else
                {
                    throw new NFSConnectionException(nfsstat4.getErrorString(compound4res.status));
                }





            }

            List<nfs_argop4> ops2 = new List<nfs_argop4>();
            ops2.Add(SequenceStub.generateRequest(false, _sessionid.value,
        _sequenceID.value.value, 12, 0));
            ops2.Add(PutfhStub.generateRequest(_cwf));

            //make better buffer
            Byte[] Buffer2 = new Byte[Count];
            Array.Copy(Buffer, Buffer2, Count);
            ops2.Add(WriteStub.generateRequest(Offset, Buffer2, currentState));



            COMPOUND4res compound4res2 = sendCompound(ops2, "");
            if (compound4res2.status == nfsstat4.NFS4_OK)
            {
                //write of offset complete
                rCount = compound4res2.resarray[2].opwrite.resok4.count.value.value;
            }
            else
            {
                throw new NFSConnectionException(nfsstat4.getErrorString(compound4res2.status));
            }

            return rCount;
        }

        public void Move(string OldDirectoryFullName, string OldFileName, string NewDirectoryFullName, string NewFileName)
        {
            if (_ProtocolV4 == null)
            { throw new NFSConnectionException("NFS Client not connected!"); }


            /* NFSAttributes OldDirectory = GetItemAttributes(OldDirectoryFullName);
             NFSAttributes NewDirectory = GetItemAttributes(NewDirectoryFullName);

             RenameArguments dpArgRename = new RenameArguments();
             dpArgRename.From = new ItemOperationArguments();
             dpArgRename.From.Directory = new NFSHandle(OldDirectory.Handle, V3.RPC.NFSv3Protocol.NFS_V3);
             dpArgRename.From.Name = new Name(OldFileName);
             dpArgRename.To = new ItemOperationArguments();
             dpArgRename.To.Directory = new NFSHandle(NewDirectory.Handle, V3.RPC.NFSv3Protocol.NFS_V3);
             dpArgRename.To.Name = new Name(NewFileName);

             ResultObject<RenameAccessOK, RenameAccessFAIL> pRenameRes =
                 _ProtocolV3.NFSPROC3_RENAME(dpArgRename);

             if (pRenameRes == null || pRenameRes.Status != NFSStats.NFS_OK)
             {
                 if (pRenameRes == null)
                 { throw new NFSGeneralException("NFSPROC3_WRITE: failure"); }

                 ExceptionHelpers.ThrowException(pRenameRes.Status);
             }*/
        }



        //TODO check how much is this used !!!
        public bool IsDirectory(string DirectoryFullName)
        {
            if (_ProtocolV4 == null)
            { throw new NFSConnectionException("NFS Client not connected!"); }

            NFSAttributes Attributes = GetItemAttributes(DirectoryFullName);

            return (Attributes != null && Attributes.NFSType == NFSItemTypes.NFDIR);
        }

        public void CompleteIO()
        {
            if (_cwf != null || _CurrentItem != string.Empty)
            {
                closeFile(_cwf, currentState);
                _CurrentItem = string.Empty;
                _cwf = null;
            }

        }



        //part of new nfsv4.1 functions
        private void mount()
        {
            //tell own id and ask server for id he want's we to use it
            exchange_ids();
            //create session!
            create_session();

            //we created session ok  now let's start the timer
            timer = new Timer(10000); 
            timer.Elapsed += new ElapsedEventHandler(TimerCallback);
            timer.Enabled = true; // Enable it


        }


        private void reclaim_complete()
        {
            List<nfs_argop4> ops = new List<nfs_argop4>();
            ops.Add(SequenceStub.generateRequest(false, _sessionid.value,
        _sequenceID.value.value, 12, 0));
            ops.Add(ReclaimCompleteStub.generateRequest(false));



            COMPOUND4res compound4res = sendCompound(ops, "");
            if (compound4res.status == nfsstat4.NFS4_OK)
            {
            //reclaim complete
            }
            else
            {
                throw new NFSConnectionException(nfsstat4.getErrorString(compound4res.status));
            }

        }




        private void getRootFh()
        {
            List<int> attrs = new List<int>(1);
            attrs.Add(NFSv4Protocol.FATTR4_LEASE_TIME);
            List<nfs_argop4> ops = new List<nfs_argop4>();

            ops.Add(SequenceStub.generateRequest(false, _sessionid.value,
                    _sequenceID.value.value, 0, 0));
            ops.Add(PutrootfhStub.generateRequest());
            ops.Add(GetfhStub.generateRequest());
            ops.Add(GetattrStub.generateRequest(attrs));

            COMPOUND4res compound4res = sendCompound(ops, "");

            if (compound4res.status == nfsstat4.NFS4_OK)
            {

                _rootFH = compound4res.resarray[2].opgetfh.resok4.object1;

            }
            else
            {
                throw new NFSConnectionException(nfsstat4.getErrorString(compound4res.status));
            }

        }


        public String toHexString(byte[] data)
        {

            StringBuilder sb = new StringBuilder();

            foreach (byte b in data)
            {
                sb.Append(b.ToString("X"));
            }

            return sb.ToString();
        }



        public int get_fh_acess(nfs_fh4 file_handle)
        {
            uint32_t access = new uint32_t(0);

            //all acsses possible
            access.value = access.value +
            NFSv4Protocol.ACCESS4_READ +
            NFSv4Protocol.ACCESS4_LOOKUP +
            NFSv4Protocol.ACCESS4_MODIFY +
            NFSv4Protocol.ACCESS4_EXTEND +
            NFSv4Protocol.ACCESS4_DELETE; //+
            //NFSv4Protocol.ACCESS4_EXECUTE; 

            List<nfs_argop4> ops = new List<nfs_argop4>();


            ops.Add(SequenceStub.generateRequest(false, _sessionid.value,
                    _sequenceID.value.value, 12, 0));
            ops.Add(PutfhStub.generateRequest(file_handle));
            ops.Add(AcessStub.generateRequest(access));


            COMPOUND4res compound4res = sendCompound(ops, "");

            if (compound4res.status == nfsstat4.NFS4_OK)
            {

                return compound4res.resarray[2].opaccess.resok4.access.value;
            }
            else
            {
                throw new NFSConnectionException(nfsstat4.getErrorString(compound4res.status));
            }
        }




        private void exchange_ids()
        {
            List<nfs_argop4> ops = new List<nfs_argop4>();

            String domain = "localhost";
            String name = "NFS Client ";

            //String guid = System.Environment.MachineName + "@" + domain;
            String guid = System.Guid.NewGuid().ToString();

            ops.Add(ExchengeIDStub.normal(domain, name, guid, NFSv4Protocol.EXCHGID4_FLAG_SUPP_MOVED_REFER + NFSv4Protocol.EXCHGID4_FLAG_USE_NON_PNFS, state_protect_how4.SP4_NONE));

            COMPOUND4res compound4res = sendCompound(ops, "");
            if (compound4res.status == nfsstat4.NFS4_OK)
            {
                /*if (compound4res.resarray[0].opexchange_id.eir_resok4.eir_server_impl_id.Length > 0)
                {
                    string serverId = System.Text.Encoding.UTF8.GetString(compound4res.resarray[0].opexchange_id.eir_resok4.eir_server_impl_id[0].nii_name.value.value);
                }
                else
                {
                    if (compound4res.resarray[0].opexchange_id.eir_resok4.eir_server_owner.so_major_id.Length > 0)
                    {
                        string serverId = System.Text.Encoding.UTF8.GetString(compound4res.resarray[0].opexchange_id.eir_resok4.eir_server_owner.so_major_id);
                        //throw new NFSConnectionException("Server name: ="+serverId);
                    }
                }*/

                _clientIdByServer = compound4res.resarray[0].opexchange_id.eir_resok4.eir_clientid;
                _sequenceID = compound4res.resarray[0].opexchange_id.eir_resok4.eir_sequenceid;

            }
            else { throw new NFSConnectionException(nfsstat4.getErrorString(compound4res.status)); }


        }


        private void create_session()
        {
            List<nfs_argop4> ops = new List<nfs_argop4>();

            ops.Add(CreateSessionStub.standard(_clientIdByServer, _sequenceID));

            COMPOUND4res compound4res = sendCompound(ops, "");

            if (compound4res.status == nfsstat4.NFS4_OK)
            {

                _sessionid = compound4res.resarray[0].opcreate_session.csr_resok4.csr_sessionid;
                // FIXME: no idea why, but other wise server reply MISORDER
                _sequenceID.value.value = 0;

                maxTRrate = compound4res.resarray[0].opcreate_session.csr_resok4.csr_fore_chan_attrs.ca_maxrequestsize.value.value;
                maxRXrate = compound4res.resarray[0].opcreate_session.csr_resok4.csr_fore_chan_attrs.ca_maxresponsesize.value.value;

            }
            else
            {
                throw new NFSConnectionException(nfsstat4.getErrorString(compound4res.status));
            }
        }


        // a very nice to send a client null procedure
        private void sendNullPRocedure()
        {
            _ProtocolV4.NFSPROC4_NULL_4();
        }

        private void destroy_session()
        {
            if (_sessionid != null)
            {
                List<nfs_argop4> ops = new List<nfs_argop4>();

                ops.Add(DestroySessionStub.standard(_sessionid));

                COMPOUND4res compound4res = sendCompound(ops, "");
                _sessionid = null;
            }
        }



        private void destroy_clientId()
        {
            if (_clientIdByServer != null)
            {
                List<nfs_argop4> ops = new List<nfs_argop4>();

                ops.Add(DestroyClientIdStub.standard(_clientIdByServer));

                COMPOUND4res compound4res = sendCompound(ops, "");
                _clientIdByServer = null;
            }
        }



        private COMPOUND4res sendCompound(List<nfs_argop4> ops, String tag)
        {

            COMPOUND4res compound4res;
            COMPOUND4args compound4args = generateCompound(tag, ops);
            /*
             * wail if server is in the grace period.
             *
             * TODO: escape if it takes too long
             */
            do
            {
                compound4res = _ProtocolV4.NFSPROC4_COMPOUND_4(compound4args);
                processSequence(compound4res, compound4args);

            } while (compound4res.status == nfsstat4.NFS4ERR_GRACE || compound4res.status == nfsstat4.NFS4ERR_DELAY);

            return compound4res;
        }


        public static COMPOUND4args generateCompound(String tag,
        List<nfs_argop4> opList)
        {

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] bytes = encoding.GetBytes(tag);

            COMPOUND4args compound4args = new COMPOUND4args();
            compound4args.tag = new utf8str_cs(new utf8string(bytes));
            compound4args.minorversion = new uint32_t(1);

            //compound4args.argarray = opList.ToArray(new nfs_argop4[opList.Count]);
            compound4args.argarray = opList.ToArray();

            return compound4args;

        }

        public void processSequence(COMPOUND4res compound4res, COMPOUND4args compound4args)
        {

            if (compound4res.resarray!=null && compound4res.resarray.Length!=0 &&  compound4res.resarray[0].resop == nfs_opnum4.OP_SEQUENCE &&
                    compound4res.resarray[0].opsequence.sr_status == nfsstat4.NFS4_OK)
            {

                _lastUpdate = GetGMTInMS();
                ++_sequenceID.value.value;
                //let's try this also
                if (compound4res.status == nfsstat4.NFS4ERR_DELAY)
                    compound4args.argarray[0].opsequence.sa_sequenceid.value.value++;

            }
        }



        private void send_only_sequence()
        {
            List<nfs_argop4> ops = new List<nfs_argop4>();
            ops.Add(SequenceStub.generateRequest(false, _sessionid.value,
        _sequenceID.value.value, 12, 0));

            COMPOUND4res compound4res = sendCompound(ops, "");
            if (compound4res.status == nfsstat4.NFS4_OK)
            {
                //reclaim complete
            }
            else
            {
                throw new NFSConnectionException(nfsstat4.getErrorString(compound4res.status));
            }

        }


        private void TimerCallback(object sender, ElapsedEventArgs e)
        {
            if (needUpdate())
                send_only_sequence();
        }


        public static long GetGMTInMS()
        {
            var unixTime = DateTime.Now.ToUniversalTime() -
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (long)unixTime.TotalMilliseconds;
        }

        private bool needUpdate()
        {
            // 60 seconds
            return (GetGMTInMS() - _lastUpdate) > 59000;
        }

        #endregion
    }

}

