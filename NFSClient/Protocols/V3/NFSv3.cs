using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using NFSLibrary.Protocols.Commons;
using NFSLibrary.Protocols.Commons.Exceptions;
using NFSLibrary.Protocols.Commons.Exceptions.Mount;
using NFSLibrary.Protocols.V3.RPC;
using NFSLibrary.Protocols.V3.RPC.Mount;
using org.acplt.oncrpc;

namespace NFSLibrary.Protocols.V3
{
    public class NFSv3 : INFS
    {
        #region Fields

        private NFSHandle _RootDirectoryHandleObject = null;
        private NFSHandle _CurrentItemHandleObject = null;

        private NFSv3ProtocolClient _ProtocolV3 = null;
        private NFSv3MountProtocolClient _MountProtocolV3 = null;

        private String _MountedDevice = String.Empty;
        private String _CurrentItem = String.Empty;

        private int _GroupID = -1;
        private int _UserID = -1;

        #endregion

        #region Constructur

        public void Connect(IPAddress Address, int UserID, int GroupID, int ClientTimeout, System.Text.Encoding characterEncoding, bool useSecurePort,bool useCache)
        {
            if (ClientTimeout == 0)
            { ClientTimeout = 60000; }

            if (characterEncoding == null)
            { characterEncoding = System.Text.Encoding.ASCII; }

            _RootDirectoryHandleObject = null;
            _CurrentItemHandleObject = null;

            _MountedDevice = String.Empty;
            _CurrentItem = String.Empty;

            _GroupID = GroupID;
            _UserID = UserID;


            OncRpcClientAuthUnix authUnix = new OncRpcClientAuthUnix(Address.ToString(), UserID, GroupID);

            _MountProtocolV3 = new NFSv3MountProtocolClient(Address, OncRpcProtocols.ONCRPC_UDP, useSecurePort);

            _MountProtocolV3.GetClient().setAuth(authUnix);
            _MountProtocolV3.GetClient().setTimeout(ClientTimeout);
            _MountProtocolV3.GetClient().setCharacterEncoding(characterEncoding.WebName);

            _ProtocolV3 = new NFSv3ProtocolClient(Address, OncRpcProtocols.ONCRPC_TCP, useSecurePort);

            _ProtocolV3.GetClient().setAuth(authUnix);
            _ProtocolV3.GetClient().setTimeout(ClientTimeout);
            _ProtocolV3.GetClient().setCharacterEncoding(characterEncoding.WebName);
        }

        #endregion

        #region Public Methods

        public void Disconnect()
        {
            _RootDirectoryHandleObject = null;
            _CurrentItemHandleObject = null;

            _MountedDevice = String.Empty;
            _CurrentItem = String.Empty;

            if (_MountProtocolV3 != null)
                _MountProtocolV3.close();

            if (_ProtocolV3 != null)
                _ProtocolV3.close();
        }

        public int GetBlockSize()
        {
            //call fsinfo
            FSInfoArguments argsfs = new FSInfoArguments();
            argsfs.FSRoot = _RootDirectoryHandleObject;
            ResultObject<FSInfoAccessOK, FSInfoAccessFAIL> fsinfo =
             _ProtocolV3.NFSPROC3_FSINFO(argsfs);

            if (fsinfo.Status == NFSStats.NFS_OK)
            {
                int maxTRrate = fsinfo.OK.PreferredReadRequestSize;
                int maxRXrate = fsinfo.OK.PreferredWriteRequestSize;

                if (maxTRrate > 8000 && maxRXrate > 8000)
                {
                    if (maxTRrate > 65336 && maxRXrate > 65336)
                        return 65336;
                    else if (maxTRrate == maxRXrate)
                        return maxTRrate - 200;
                    else if (maxTRrate < maxRXrate)
                        return maxTRrate - 200;
                    else
                        return maxRXrate - 200;
                }
            }
            return 8000;
        }

        public List<String> GetExportedDevices()
        {
            if (_MountProtocolV3 == null)
            { throw new NFSMountConnectionException("NFS Device not connected!"); }

            List<string> nfsDevices = new List<string>();

            Exports exp = _MountProtocolV3.MOUNTPROC3_EXPORT();

            for (; ; )
            {
                nfsDevices.Add(exp.Value.MountPath.Value);
                exp = exp.Value.Next;

                if (exp.Value == null) break;
            }

            return nfsDevices;
        }

        public void MountDevice(String DeviceName)
        {
            if (_ProtocolV3 == null)
            { throw new NFSConnectionException("NFS Client not connected!"); }

            if (_MountProtocolV3 == null)
            { throw new NFSMountConnectionException("NFS Device not connected!"); }

            MountStatus mnt =
                _MountProtocolV3.MOUNTPROC3_MNT(new Name(DeviceName));

            if (mnt.Status == NFSMountStats.MNT_OK)
            {
                _MountedDevice = DeviceName;
                _RootDirectoryHandleObject = mnt.MountInfo.MountHandle;
            }
            else
            { MountExceptionHelpers.ThrowException(mnt.Status); }


            /*

            //custom 
            FSStatisticsArguments argsfs2 = new FSStatisticsArguments();
            argsfs2.FSRoot = _RootDirectoryHandleObject; 
            ResultObject<FSStatisticsAccessOK, FSStatisticsAccessFAIL> fstat =
                _ProtocolV3.NFSPROC3_FSSTAT(argsfs2);*/

        }

        public void UnMountDevice()
        {
            if (_MountedDevice != null)
            {
                _MountProtocolV3.MOUNTPROC3_UMNT(new Name(_MountedDevice));

                _RootDirectoryHandleObject = null;
                _CurrentItemHandleObject = null;

                _MountedDevice = String.Empty;
                _CurrentItem = String.Empty;
            }
        }

        public List<String> GetItemList(String DirectoryFullName)
        {
            if (_ProtocolV3 == null)
            { throw new NFSConnectionException("NFS Client not connected!"); }

            if (_MountProtocolV3 == null)
            { throw new NFSMountConnectionException("NFS Device not connected!"); }

            List<string> ItemsList = new List<string>();

            NFSAttributes itemAttributes =
                GetItemAttributes(DirectoryFullName);

            if (itemAttributes != null)
            {
                ReadFolderArguments dpRdArgs = new ReadFolderArguments();

                dpRdArgs.Count = 4096;
                dpRdArgs.Cookie = new NFSCookie(0);
                dpRdArgs.CookieData = new byte[NFSv3Protocol.NFS3_COOKIEVERFSIZE];
                dpRdArgs.HandleObject = new NFSHandle(itemAttributes.Handle, V3.RPC.NFSv3Protocol.NFS_V3);

                ResultObject<ReadFolderAccessResultOK, ReadFolderAccessResultFAIL> pReadDirRes;

                do
                {
                    pReadDirRes = _ProtocolV3.NFSPROC3_READDIR(dpRdArgs);

                    if (pReadDirRes != null &&
                        pReadDirRes.Status == NFSStats.NFS_OK)
                    {
                        Entry pEntry =
                            pReadDirRes.OK.Reply.Entries;

                        Array.Copy(pReadDirRes.OK.CookieData, dpRdArgs.CookieData, NFSv3Protocol.NFS3_COOKIEVERFSIZE);
                        while (pEntry != null)
                        {
                            ItemsList.Add(pEntry.Name.Value);
                            dpRdArgs.Cookie = pEntry.Cookie;
                            pEntry = pEntry.NextEntry;
                        }
                    }
                    else
                    {
                        if (pReadDirRes == null)
                        { throw new NFSGeneralException("NFSPROC3_READDIR: failure"); }

                        if (pReadDirRes.Status != NFSStats.NFS_OK)
                        { ExceptionHelpers.ThrowException(pReadDirRes.Status); }
                    }
                } while (pReadDirRes != null && !pReadDirRes.OK.Reply.EOF);
            }
            else
            { ExceptionHelpers.ThrowException(NFSStats.NFSERR_NOENT); }

            return ItemsList;
        }

        public NFSAttributes GetItemAttributes(string ItemFullName, bool ThrowExceptionIfNotFound = true)
        {
            if (_ProtocolV3 == null)
            { throw new NFSConnectionException("NFS Client not connected!"); }

            if (_MountProtocolV3 == null)
            { throw new NFSMountConnectionException("NFS Device not connected!"); }

            NFSAttributes attributes = null;

            if (String.IsNullOrEmpty(ItemFullName))
                ItemFullName = ".";

            NFSHandle currentItem = _RootDirectoryHandleObject;
            String[] PathTree = ItemFullName.Split(@"\".ToCharArray());

            for (int pC = 0; pC < PathTree.Length; pC++)
            {
                ItemOperationArguments dpDrArgs = new ItemOperationArguments();
                dpDrArgs.Directory = currentItem;
                dpDrArgs.Name = new Name(PathTree[pC]);

                ResultObject<ItemOperationAccessResultOK, ItemOperationAccessResultFAIL> pDirOpRes =
                    _ProtocolV3.NFSPROC3_LOOKUP(dpDrArgs);

                if (pDirOpRes != null &&
                    pDirOpRes.Status == NFSStats.NFS_OK)
                {
                    currentItem = pDirOpRes.OK.ItemHandle;

                    if (PathTree.Length - 1 == pC)
                    {
                        attributes = new NFSAttributes(
                                        pDirOpRes.OK.ItemAttributes.Attributes.CreateTime.Seconds,
                                        pDirOpRes.OK.ItemAttributes.Attributes.LastAccessedTime.Seconds,
                                        pDirOpRes.OK.ItemAttributes.Attributes.ModifiedTime.Seconds,
                                        pDirOpRes.OK.ItemAttributes.Attributes.Type,
                                        pDirOpRes.OK.ItemAttributes.Attributes.Mode,
                                        pDirOpRes.OK.ItemAttributes.Attributes.Size,
                                        pDirOpRes.OK.ItemHandle.Value);
                    }
                }
                else
                {
                    if (pDirOpRes == null || pDirOpRes.Status == NFSStats.NFSERR_NOENT)
                    { attributes = null; break; }

                    if(ThrowExceptionIfNotFound)
                        ExceptionHelpers.ThrowException(pDirOpRes.Status);
                }
            }

            return attributes;
        }

        public void CreateDirectory(string DirectoryFullName, NFSPermission Mode)
        {
            if (_ProtocolV3 == null)
            { throw new NFSConnectionException("NFS Client not connected!"); }

            if (_MountProtocolV3 == null)
            { throw new NFSMountConnectionException("NFS Device not connected!"); }

            if (Mode == null)
            { Mode = new NFSPermission(7, 7, 7); }

            string ParentDirectory = System.IO.Path.GetDirectoryName(DirectoryFullName);
            string DirectoryName = System.IO.Path.GetFileName(DirectoryFullName);

            NFSAttributes ParentItemAttributes = GetItemAttributes(ParentDirectory);

            MakeFolderArguments dpArgCreate = new MakeFolderArguments();
            dpArgCreate.Attributes = new MakeAttributes();
            dpArgCreate.Attributes.LastAccessedTime = new NFSTimeValue();
            dpArgCreate.Attributes.ModifiedTime = new NFSTimeValue();
            dpArgCreate.Attributes.Mode = Mode;
            dpArgCreate.Attributes.SetMode = true;
            dpArgCreate.Attributes.UserID = this._UserID;
            dpArgCreate.Attributes.SetUserID = true;
            dpArgCreate.Attributes.GroupID = this._GroupID;
            dpArgCreate.Attributes.SetGroupID = true;
            dpArgCreate.Where = new ItemOperationArguments();
            dpArgCreate.Where.Directory = new NFSHandle(ParentItemAttributes.Handle, V3.RPC.NFSv3Protocol.NFS_V3);
            dpArgCreate.Where.Name = new Name(DirectoryName);

            ResultObject<MakeFolderAccessOK, MakeFolderAccessFAIL> pDirOpRes =
                _ProtocolV3.NFSPROC3_MKDIR(dpArgCreate);

            if (pDirOpRes == null ||
                pDirOpRes.Status != NFSStats.NFS_OK)
            {
                if (pDirOpRes == null)
                { throw new NFSGeneralException("NFSPROC3_MKDIR: failure"); }

                ExceptionHelpers.ThrowException(pDirOpRes.Status);
            }
        }

        public void DeleteDirectory(string DirectoryFullName)
        {
            if (_ProtocolV3 == null)
            { throw new NFSConnectionException("NFS Client not connected!"); }

            if (_MountProtocolV3 == null)
            { throw new NFSMountConnectionException("NFS Device not connected!"); }

            string ParentDirectory = System.IO.Path.GetDirectoryName(DirectoryFullName);
            string DirectoryName = System.IO.Path.GetFileName(DirectoryFullName);

            NFSAttributes ParentItemAttributes = GetItemAttributes(ParentDirectory);

            ItemOperationArguments dpArgDelete = new ItemOperationArguments();
            dpArgDelete.Directory = new NFSHandle(ParentItemAttributes.Handle, V3.RPC.NFSv3Protocol.NFS_V3);
            dpArgDelete.Name = new Name(DirectoryName);

            ResultObject<RemoveAccessOK, RemoveAccessFAIL> pRmDirRes =
                _ProtocolV3.NFSPROC3_RMDIR(dpArgDelete);

            if (pRmDirRes == null || pRmDirRes.Status != NFSStats.NFS_OK)
            {
                if (pRmDirRes == null)
                { throw new NFSGeneralException("NFSPROC3_RMDIR: failure"); }

                ExceptionHelpers.ThrowException(pRmDirRes.Status);
            }
        }

        public void DeleteFile(string FileFullName)
        {
            if (_ProtocolV3 == null)
            { throw new NFSConnectionException("NFS Client not connected!"); }

            if (_MountProtocolV3 == null)
            { throw new NFSMountConnectionException("NFS Device not connected!"); }

            string ParentDirectory = System.IO.Path.GetDirectoryName(FileFullName);
            string FileName = System.IO.Path.GetFileName(FileFullName);

            NFSAttributes ParentItemAttributes = GetItemAttributes(ParentDirectory);

            ItemOperationArguments dpArgDelete = new ItemOperationArguments();
            dpArgDelete.Directory = new NFSHandle(ParentItemAttributes.Handle, V3.RPC.NFSv3Protocol.NFS_V3);
            dpArgDelete.Name = new Name(FileName);

            ResultObject<RemoveAccessOK, RemoveAccessFAIL> pRemoveRes =
                _ProtocolV3.NFSPROC3_REMOVE(dpArgDelete);

            if (pRemoveRes == null || pRemoveRes.Status != NFSStats.NFS_OK)
            {
                if (pRemoveRes == null)
                { throw new NFSGeneralException("NFSPROC3_REMOVE: failure"); }

                ExceptionHelpers.ThrowException(pRemoveRes.Status);
            }
        }

        public void CreateFile(string FileFullName, NFSPermission Mode)
        {
            if (_ProtocolV3 == null)
            { throw new NFSConnectionException("NFS Client not connected!"); }

            if (_MountProtocolV3 == null)
            { throw new NFSMountConnectionException("NFS Device not connected!"); }

            if (Mode == null)
            { Mode = new NFSPermission(7, 7, 7); }

            string ParentDirectory = System.IO.Path.GetDirectoryName(FileFullName);
            string FileName = System.IO.Path.GetFileName(FileFullName);

            NFSAttributes ParentItemAttributes = GetItemAttributes(ParentDirectory);

            MakeFileArguments dpArgCreate = new MakeFileArguments();
            dpArgCreate.How = new MakeFileHow();
            dpArgCreate.How.Mode = MakeFileHow.MakeFileModes.UNCHECKED;
            dpArgCreate.How.Attributes = new MakeAttributes();
            dpArgCreate.How.Attributes.LastAccessedTime = new NFSTimeValue();
            dpArgCreate.How.Attributes.ModifiedTime = new NFSTimeValue();
            dpArgCreate.How.Attributes.Mode = Mode;
            dpArgCreate.How.Attributes.SetMode = true;
            dpArgCreate.How.Attributes.UserID = this._UserID;
            dpArgCreate.How.Attributes.SetUserID = true;
            dpArgCreate.How.Attributes.GroupID = this._GroupID;
            dpArgCreate.How.Attributes.SetGroupID = true;
            dpArgCreate.How.Attributes.Size = 0;
            dpArgCreate.How.Attributes.SetSize = true;
            dpArgCreate.How.Verification = new byte[NFSv3Protocol.NFS3_CREATEVERFSIZE];
            dpArgCreate.Where = new ItemOperationArguments();
            dpArgCreate.Where.Directory = new NFSHandle(ParentItemAttributes.Handle, V3.RPC.NFSv3Protocol.NFS_V3);
            dpArgCreate.Where.Name = new Name(FileName);

            ResultObject<MakeFileAccessOK, MakeFileAccessFAIL> pCreateRes =
                _ProtocolV3.NFSPROC3_CREATE(dpArgCreate);

            if (pCreateRes == null ||
                pCreateRes.Status != NFSStats.NFS_OK)
            {
                if (pCreateRes == null)
                { throw new NFSGeneralException("NFSPROC3_CREATE: failure"); }

                ExceptionHelpers.ThrowException(pCreateRes.Status);
            }
        }

        public int Read(String FileFullName, long Offset, int Count, ref Byte[] Buffer)
        {
            if (_ProtocolV3 == null)
            { throw new NFSConnectionException("NFS Client not connected!"); }

            if (_MountProtocolV3 == null)
            { throw new NFSMountConnectionException("NFS Device not connected!"); }

            int rCount = 0;

            if (Count == 0)
                return 0;

            if (_CurrentItem != FileFullName)
            {
                NFSAttributes Attributes = GetItemAttributes(FileFullName);
                _CurrentItemHandleObject = new NFSHandle(Attributes.Handle, V3.RPC.NFSv3Protocol.NFS_V3);
                _CurrentItem = FileFullName;
            }

            ReadArguments dpArgRead = new ReadArguments();
            dpArgRead.File = _CurrentItemHandleObject;
            dpArgRead.Offset = Offset;
            dpArgRead.Count = Count;

            ResultObject<ReadAccessOK, ReadAccessFAIL> pReadRes =
                _ProtocolV3.NFSPROC3_READ(dpArgRead);

            if (pReadRes != null)
            {
                if (pReadRes.Status != NFSStats.NFS_OK)
                { ExceptionHelpers.ThrowException(pReadRes.Status); }

                rCount = pReadRes.OK.Data.Length;

                Array.Copy(pReadRes.OK.Data, Buffer, rCount);
            }
            else
            { throw new NFSGeneralException("NFSPROC3_READ: failure"); }

            return rCount;
        }

        public void SetFileSize(string FileFullName, long Size)
        {
            if (_ProtocolV3 == null)
            { throw new NFSConnectionException("NFS Client not connected!"); }

            if (_MountProtocolV3 == null)
            { throw new NFSMountConnectionException("NFS Device not connected!"); }

            NFSAttributes Attributes = GetItemAttributes(FileFullName);

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
            }
        }

        public int Write(String FileFullName, long Offset, int Count, Byte[] Buffer)
        {
            if (_ProtocolV3 == null)
            { throw new NFSConnectionException("NFS Client not connected!"); }

            if (_MountProtocolV3 == null)
            { throw new NFSMountConnectionException("NFS Device not connected!"); }

            int rCount = 0;

            if (_CurrentItem != FileFullName)
            {
                NFSAttributes Attributes = GetItemAttributes(FileFullName);
                _CurrentItemHandleObject = new NFSHandle(Attributes.Handle, V3.RPC.NFSv3Protocol.NFS_V3);
                _CurrentItem = FileFullName;
            }

            if (Count < Buffer.Length)
            { Array.Resize<byte>(ref Buffer, Count); }

            WriteArguments dpArgWrite = new WriteArguments();
            dpArgWrite.File = _CurrentItemHandleObject;
            dpArgWrite.Offset = Offset;
            dpArgWrite.Count = Count;
            dpArgWrite.Data = Buffer;

            ResultObject<WriteAccessOK, WriteAccessFAIL> pAttrStat =
                _ProtocolV3.NFSPROC3_WRITE(dpArgWrite);

            if (pAttrStat != null)
            {
                if (pAttrStat.Status != NFSStats.NFS_OK)
                { ExceptionHelpers.ThrowException(pAttrStat.Status); }

                rCount = pAttrStat.OK.Count;
            }
            else
            { throw new NFSGeneralException("NFSPROC3_WRITE: failure"); }

            return rCount;
        }

        public void Move(string OldDirectoryFullName, string OldFileName, string NewDirectoryFullName, string NewFileName)
        {
            if (_ProtocolV3 == null)
            { throw new NFSConnectionException("NFS Client not connected!"); }

            if (_MountProtocolV3 == null)
            { throw new NFSMountConnectionException("NFS Device not connected!"); }

            NFSAttributes OldDirectory = GetItemAttributes(OldDirectoryFullName);
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
            }
        }

        public bool IsDirectory(string DirectoryFullName)
        {
            if (_ProtocolV3 == null)
            { throw new NFSConnectionException("NFS Client not connected!"); }

            if (_MountProtocolV3 == null)
            { throw new NFSMountConnectionException("NFS Device not connected!"); }

            NFSAttributes Attributes = GetItemAttributes(DirectoryFullName);

            return (Attributes != null && Attributes.NFSType == NFSItemTypes.NFDIR);
        }

        public void CompleteIO()
        {
            _CurrentItemHandleObject = null;
            _CurrentItem = string.Empty;
        }

        #endregion
    }

}

