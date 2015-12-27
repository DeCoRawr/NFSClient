using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using NFSLibrary.Protocols.Commons;
using NFSLibrary.Protocols.Commons.Exceptions;
using NFSLibrary.Protocols.Commons.Exceptions.Mount;
using NFSLibrary.Protocols.V2.RPC;
using NFSLibrary.Protocols.V2.RPC.Mount;
using org.acplt.oncrpc;

namespace NFSLibrary.Protocols.V2
{
    public class NFSv2 : INFS
    {
        #region Fields

        private NFSHandle _RootDirectoryHandleObject = null;
        private NFSHandle _CurrentItemHandleObject = null;

        private NFSv2ProtocolClient _ProtocolV2 = null;
        private NFSv2MountProtocolClient _MountProtocolV2 = null;

        private String _MountedDevice = String.Empty;
        private String _CurrentItem = String.Empty;

        private int _GroupID = -1;
        private int _UserID = -1;

        #endregion

        #region Constants

        /*const int MODE_FMT = 0170000;
        const int MODE_DIR = 0040000;
        const int MODE_CHR = 0020000;
        const int MODE_BLK = 0060000;
        const int MODE_REG = 0100000;
        const int MODE_LNK = 0120000;
        const int MODE_SOCK = 0140000;
        const int MODE_FIFO = 0010000;*/

        #endregion

        #region Methods

        public void Connect(IPAddress Address, int UserID, int GroupID, int ClientTimeout, System.Text.Encoding characterEncoding, bool useSecurePort, bool useCache)
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

            _MountProtocolV2 = new NFSv2MountProtocolClient(Address, OncRpcProtocols.ONCRPC_UDP, useSecurePort);
            _ProtocolV2 = new NFSv2ProtocolClient(Address, OncRpcProtocols.ONCRPC_UDP, useSecurePort);

            OncRpcClientAuthUnix authUnix = new OncRpcClientAuthUnix(System.Environment.MachineName, UserID, GroupID);

            _MountProtocolV2.GetClient().setAuth(authUnix);
            _MountProtocolV2.GetClient().setTimeout(ClientTimeout);
            _MountProtocolV2.GetClient().setCharacterEncoding(characterEncoding.WebName);

            _ProtocolV2.GetClient().setAuth(authUnix);
            _ProtocolV2.GetClient().setTimeout(ClientTimeout);
            _ProtocolV2.GetClient().setCharacterEncoding(characterEncoding.WebName);
        }

        public void Disconnect()
        {
            _RootDirectoryHandleObject = null;
            _CurrentItemHandleObject = null;

            _MountedDevice = String.Empty;
            _CurrentItem = String.Empty;

            if (_MountProtocolV2 != null)
            { _MountProtocolV2.close(); }

            if (_ProtocolV2 != null)
            { _ProtocolV2.close(); }
        }

        public int GetBlockSize()
        {
            return 8064;
        }

        public List<String> GetExportedDevices()
        {
            if (_MountProtocolV2 == null)
            { throw new NFSMountConnectionException("NFS Device not connected!"); }

            List<string> nfsDevices = new List<string>();

            Exports exp = _MountProtocolV2.MOUNTPROC_EXPORT();

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
            if (_ProtocolV2 == null)
            { 
                throw new NFSConnectionException("NFS Client not connected!"); 
            }

            MountStatus mnt =
                _MountProtocolV2.MOUNTPROC_MNT(new Name(DeviceName));

            if (mnt.Status == 0)
            {
                _MountedDevice = DeviceName;
                _RootDirectoryHandleObject = mnt.Handle;
            }
            else
            { 
                MountExceptionHelpers.ThrowException(mnt.Status); 
            }
        }

        public void UnMountDevice()
        {
            if (_MountedDevice != null)
            {
                _MountProtocolV2.MOUNTPROC_UMNT(new Name(_MountedDevice));

                _RootDirectoryHandleObject = null;
                _CurrentItemHandleObject = null;

                _MountedDevice = String.Empty;
                _CurrentItem = String.Empty;
            }
        }

        public List<String> GetItemList(String DirectoryFullName)
        {
            if (_ProtocolV2 == null)
            { 
                throw new NFSConnectionException("NFS Client not connected!"); 
            }

            if (_MountProtocolV2 == null)
            { 
                throw new NFSMountConnectionException("NFS Device not connected!"); 
            }

            List<string> ItemsList = new List<string>();

            NFSAttributes itemAttributes =
                GetItemAttributes(DirectoryFullName);

            if (itemAttributes != null)
            {
                ItemArguments dpRdArgs = new ItemArguments();

                dpRdArgs.Cookie = new NFSCookie(0);
                dpRdArgs.Count = 4096;
                dpRdArgs.HandleObject = new NFSHandle(itemAttributes.Handle, V2.RPC.NFSv2Protocol.NFS_VERSION);

                ItemStatus pReadDirRes;

                do
                {
                    pReadDirRes = _ProtocolV2.NFSPROC_READDIR(dpRdArgs);

                    if (pReadDirRes != null &&
                        pReadDirRes.Status == NFSStats.NFS_OK)
                    {
                        Entry pEntry =
                            pReadDirRes.OK.Entries;

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
                        { 
                            throw new NFSGeneralException("NFSPROC_READDIR: failure"); 
                        }

                        ExceptionHelpers.ThrowException(pReadDirRes.Status);
                    }
                } while (pReadDirRes != null && !pReadDirRes.OK.EOF);
            }
            else
            { 
                ExceptionHelpers.ThrowException(NFSStats.NFSERR_NOENT); 
            }

            return ItemsList;
        }

        public NFSAttributes GetItemAttributes(String ItemFullName, bool ThrowExceptionIfNotFound = true)
        {
            if (_ProtocolV2 == null)
            { 
                throw new NFSConnectionException("NFS Client not connected!"); 
            }

            if (_MountProtocolV2 == null)
            { 
                throw new NFSMountConnectionException("NFS Device not connected!"); 
            }

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

                ItemOperationStatus pDirOpRes =
                    _ProtocolV2.NFSPROC_LOOKUP(dpDrArgs);

                if (pDirOpRes != null &&
                    pDirOpRes.Status == NFSStats.NFS_OK)
                {
                    currentItem = pDirOpRes.OK.HandleObject;

                    if (PathTree.Length - 1 == pC)
                    {
                        attributes = new NFSAttributes(
                                        pDirOpRes.OK.Attributes.CreateTime.Seconds,
                                        pDirOpRes.OK.Attributes.LastAccessedTime.Seconds,
                                        pDirOpRes.OK.Attributes.ModifiedTime.Seconds,
                                        pDirOpRes.OK.Attributes.Type,
                                        pDirOpRes.OK.Attributes.Mode,
                                        pDirOpRes.OK.Attributes.Size,
                                        pDirOpRes.OK.HandleObject.Value);
                    }
                }
                else
                {
                    if (pDirOpRes == null || pDirOpRes.Status == NFSStats.NFSERR_NOENT)
                    { 
                        attributes = null; 
                        break; 
                    }

                    if(ThrowExceptionIfNotFound)
                        ExceptionHelpers.ThrowException(pDirOpRes.Status);
                }
            }

            return attributes;
        }

        public void CreateDirectory(String DirectoryFullName, NFSPermission Mode)
        {
            if (_ProtocolV2 == null)
            { 
                throw new NFSConnectionException("NFS Client not connected!"); 
            }

            if (_MountProtocolV2 == null)
            { 
                throw new NFSMountConnectionException("NFS Device not connected!"); 
            }

            if (Mode == null)
            { 
                Mode = new NFSPermission(7, 7, 7); 
            }

            string ParentDirectory = System.IO.Path.GetDirectoryName(DirectoryFullName);
            string DirectoryName = System.IO.Path.GetFileName(DirectoryFullName);

            NFSAttributes ParentItemAttributes = GetItemAttributes(ParentDirectory);

            CreateArguments dpArgCreate = new CreateArguments();
            dpArgCreate.Attributes = new CreateAttributes();
            dpArgCreate.Attributes.LastAccessedTime = new NFSTimeValue();
            dpArgCreate.Attributes.ModifiedTime = new NFSTimeValue();
            dpArgCreate.Attributes.Mode = Mode;
            dpArgCreate.Attributes.UserID = this._UserID;
            dpArgCreate.Attributes.GroupID = this._GroupID;
            dpArgCreate.Where = new ItemOperationArguments();
            dpArgCreate.Where.Directory = new NFSHandle(ParentItemAttributes.Handle, V2.RPC.NFSv2Protocol.NFS_VERSION);
            dpArgCreate.Where.Name = new Name(DirectoryName);

            ItemOperationStatus pDirOpRes =
                _ProtocolV2.NFSPROC_MKDIR(dpArgCreate);

            if (pDirOpRes == null ||
                pDirOpRes.Status != NFSStats.NFS_OK)
            {
                if (pDirOpRes == null)
                { 
                    throw new NFSGeneralException("NFSPROC_MKDIR: failure"); 
                }

                ExceptionHelpers.ThrowException(pDirOpRes.Status);
            }
        }

        public void DeleteDirectory(string DirectoryFullName)
        {
            if (_ProtocolV2 == null)
            { 
                throw new NFSConnectionException("NFS Client not connected!"); 
            }

            if (_MountProtocolV2 == null)
            { 
                throw new NFSMountConnectionException("NFS Device not connected!"); 
            }

            string ParentDirectory = System.IO.Path.GetDirectoryName(DirectoryFullName);
            string DirectoryName = System.IO.Path.GetFileName(DirectoryFullName);

            NFSAttributes ParentItemAttributes = GetItemAttributes(ParentDirectory);

            ItemOperationArguments dpArgDelete = new ItemOperationArguments();
            dpArgDelete.Directory = new NFSHandle(ParentItemAttributes.Handle, V2.RPC.NFSv2Protocol.NFS_VERSION);
            dpArgDelete.Name = new Name(DirectoryName);

            NFSStats Status = (NFSStats)_ProtocolV2.NFSPROC_RMDIR(dpArgDelete);

            if (Status != NFSStats.NFS_OK)
            { 
                ExceptionHelpers.ThrowException(Status); 
            }
        }

        public void DeleteFile(string FileFullName)
        {
            if (_ProtocolV2 == null)
            { 
                throw new NFSConnectionException("NFS Client not connected!"); 
            }

            if (_MountProtocolV2 == null)
            { 
                throw new NFSMountConnectionException("NFS Device not connected!"); 
            }

            string ParentDirectory = System.IO.Path.GetDirectoryName(FileFullName);
            string FileName = System.IO.Path.GetFileName(FileFullName);

            NFSAttributes ParentItemAttributes = GetItemAttributes(ParentDirectory);

            ItemOperationArguments dpArgDelete = new ItemOperationArguments();
            dpArgDelete.Directory = new NFSHandle(ParentItemAttributes.Handle, V2.RPC.NFSv2Protocol.NFS_VERSION);
            dpArgDelete.Name = new Name(FileName);

            NFSStats Status = (NFSStats)_ProtocolV2.NFSPROC_REMOVE(dpArgDelete);

            if (Status != NFSStats.NFS_OK)
            { 
                ExceptionHelpers.ThrowException(Status); 
            }
        }

        public void CreateFile(string FileFullName, NFSPermission Mode)
        {
            if (_ProtocolV2 == null)
            { 
                throw new NFSConnectionException("NFS Client not connected!"); 
            }

            if (_MountProtocolV2 == null)
            { 
                throw new NFSMountConnectionException("NFS Device not connected!"); 
            }

            if (Mode == null)
            { 
                Mode = new NFSPermission(7, 7, 7); 
            }

            string ParentDirectory = System.IO.Path.GetDirectoryName(FileFullName);
            string FileName = System.IO.Path.GetFileName(FileFullName);

            NFSAttributes ParentItemAttributes = GetItemAttributes(ParentDirectory);

            CreateArguments dpArgCreate = new CreateArguments();
            dpArgCreate.Attributes = new CreateAttributes();
            dpArgCreate.Attributes.LastAccessedTime = new NFSTimeValue();
            dpArgCreate.Attributes.ModifiedTime = new NFSTimeValue();
            dpArgCreate.Attributes.Mode = Mode;
            dpArgCreate.Attributes.UserID = this._UserID;
            dpArgCreate.Attributes.GroupID = this._GroupID;
            dpArgCreate.Attributes.Size = 0;
            dpArgCreate.Where = new ItemOperationArguments();
            dpArgCreate.Where.Directory = new NFSHandle(ParentItemAttributes.Handle, V2.RPC.NFSv2Protocol.NFS_VERSION);
            dpArgCreate.Where.Name = new Name(FileName);

            ItemOperationStatus pDirOpRes =
                _ProtocolV2.NFSPROC_CREATE(dpArgCreate);

            if (pDirOpRes == null ||
                pDirOpRes.Status != NFSStats.NFS_OK)
            {
                if (pDirOpRes == null)
                { 
                    throw new NFSGeneralException("NFSPROC_CREATE: failure");
                }

                ExceptionHelpers.ThrowException(pDirOpRes.Status);
            }
        }

        public int Read(String FileFullName, long Offset, int Count, ref Byte[] Buffer)
        {
            if (_ProtocolV2 == null)
            { 
                throw new NFSConnectionException("NFS Client not connected!"); 
            }

            if (_MountProtocolV2 == null)
            { 
                throw new NFSMountConnectionException("NFS Device not connected!"); 
            }

            int rCount = 0;

            if (Count == 0)
                return 0;


            if (_CurrentItem != FileFullName)
            {
                NFSAttributes Attributes = GetItemAttributes(FileFullName);
                _CurrentItemHandleObject = new NFSHandle(Attributes.Handle, V2.RPC.NFSv2Protocol.NFS_VERSION);
                _CurrentItem = FileFullName;
            }

            ReadArguments dpArgRead = new ReadArguments();
            dpArgRead.File = _CurrentItemHandleObject;
            dpArgRead.Offset = (int)Offset;
            dpArgRead.Count = Count;

            ReadStatus pReadRes =
                _ProtocolV2.NFSPROC_READ(dpArgRead);

            if (pReadRes != null)
            {
                if (pReadRes.Status != NFSStats.NFS_OK)
                { ExceptionHelpers.ThrowException(pReadRes.Status); }

                rCount = pReadRes.OK.Data.Length;

                Array.Copy(pReadRes.OK.Data, Buffer, rCount);
            }
            else
            { 
                throw new NFSGeneralException("NFSPROC_READ: failure"); 
            }

            return rCount;
        }

        public void SetFileSize(string FileFullName, long Size)
        {
            if (_ProtocolV2 == null)
            { 
                throw new NFSConnectionException("NFS Client not connected!"); 
            }

            if (_MountProtocolV2 == null)
            { 
                throw new NFSMountConnectionException("NFS Device not connected!"); 
            }

            NFSAttributes Attributes = GetItemAttributes(FileFullName);

            FileArguments dpArgSAttr = new FileArguments();
            dpArgSAttr.Attributes = new CreateAttributes();
            dpArgSAttr.Attributes.LastAccessedTime = new NFSTimeValue();
            dpArgSAttr.Attributes.LastAccessedTime.Seconds = -1;
            dpArgSAttr.Attributes.LastAccessedTime.UnixSeconds = -1;
            dpArgSAttr.Attributes.ModifiedTime = new NFSTimeValue();
            dpArgSAttr.Attributes.ModifiedTime.Seconds = -1;
            dpArgSAttr.Attributes.ModifiedTime.UnixSeconds = -1;
            dpArgSAttr.Attributes.Mode = new NFSPermission(0xff, 0xff, 0xff);
            dpArgSAttr.Attributes.UserID = -1;
            dpArgSAttr.Attributes.GroupID = -1;
            dpArgSAttr.Attributes.Size = (int)Size;
            dpArgSAttr.File = new NFSHandle(Attributes.Handle, V2.RPC.NFSv2Protocol.NFS_VERSION);

            FileStatus pAttrStat =
                _ProtocolV2.NFSPROC_SETATTR(dpArgSAttr);

            if (pAttrStat == null || pAttrStat.Status != NFSStats.NFS_OK)
            {
                if (pAttrStat == null)
                { 
                    throw new NFSGeneralException("NFSPROC_SETATTR: failure"); 
                }

                ExceptionHelpers.ThrowException(pAttrStat.Status);
            }
        }

        public int Write(String FileFullName, long Offset, int Count, Byte[] Buffer)
        {
            if (_ProtocolV2 == null)
            { 
                throw new NFSConnectionException("NFS Client not connected!"); 
            }

            if (_MountProtocolV2 == null)
            {
                throw new NFSMountConnectionException("NFS Device not connected!"); 
            }

            int rCount = 0;

            if (_CurrentItem != FileFullName)
            {
                NFSAttributes Attributes = GetItemAttributes(FileFullName);
                _CurrentItemHandleObject = new NFSHandle(Attributes.Handle, V2.RPC.NFSv2Protocol.NFS_VERSION);
                _CurrentItem = FileFullName;
            }

            if (Count < Buffer.Length)
            { 
                Array.Resize<byte>(ref Buffer, Count); 
            }

            WriteArguments dpArgWrite = new WriteArguments();
            dpArgWrite.File = _CurrentItemHandleObject;
            dpArgWrite.Offset = (int)Offset;
            dpArgWrite.Data = Buffer;

            FileStatus pAttrStat =
                _ProtocolV2.NFSPROC_WRITE(dpArgWrite);

            if (pAttrStat != null)
            {
                if (pAttrStat.Status != NFSStats.NFS_OK)
                { 
                    ExceptionHelpers.ThrowException(pAttrStat.Status); 
                }

                rCount = Count;
            }
            else
            { 
                throw new NFSGeneralException("NFSPROC_WRITE: failure"); 
            }

            return rCount;
        }

        public void Move(string OldDirectoryFullName, string OldFileName, string NewDirectoryFullName, string NewFileName)
        {
            if (_ProtocolV2 == null)
            { 
                throw new NFSConnectionException("NFS Client not connected!"); 
            }

            if (_MountProtocolV2 == null)
            { 
                throw new NFSMountConnectionException("NFS Device not connected!"); 
            }

            NFSAttributes OldDirectory = GetItemAttributes(OldDirectoryFullName);
            NFSAttributes NewDirectory = GetItemAttributes(NewDirectoryFullName);

            RenameArguments dpArgRename = new RenameArguments();
            dpArgRename.From = new ItemOperationArguments();
            dpArgRename.From.Directory = new NFSHandle(OldDirectory.Handle, V2.RPC.NFSv2Protocol.NFS_VERSION);
            dpArgRename.From.Name = new Name(OldFileName);
            dpArgRename.To = new ItemOperationArguments();
            dpArgRename.To.Directory = new NFSHandle(NewDirectory.Handle, V2.RPC.NFSv2Protocol.NFS_VERSION);
            dpArgRename.To.Name = new Name(NewFileName);

            NFSStats Status =
                (NFSStats)_ProtocolV2.NFSPROC_RENAME(dpArgRename);

            if (Status != NFSStats.NFS_OK)
            { 
                ExceptionHelpers.ThrowException(Status); 
            }
        }

        public bool IsDirectory(string DirectoryFullName)
        {
            if (_ProtocolV2 == null)
            { 
                throw new NFSConnectionException("NFS Client not connected!"); 
            }

            if (_MountProtocolV2 == null)
            { 
                throw new NFSMountConnectionException("NFS Device not connected!"); 
            }

            NFSAttributes Attributes = GetItemAttributes(DirectoryFullName);

            return (Attributes != null && Attributes.NFSType == NFSItemTypes.NFDIR);
        }

        public void CompleteIO()
        {
            _CurrentItemHandleObject = null;
            _CurrentItem = string.Empty;
        }

        // Is this possible in V2?
        public List<V3.RPC.FolderEntry> GetItemListEx(string DirectoryFullName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
