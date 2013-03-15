using System;
using System.Collections.Generic;
using NFSLibrary.Protocols;
using NFSLibrary.Protocols.Commons;
using NFSLibrary.Protocols.V2;
using NFSLibrary.Protocols.V3;
using NFSLibrary.Protocols.V4; 
using System.Runtime.InteropServices;
using System.Net;

namespace NFSLibrary
{
    /// <summary>
    /// NFS Client Library
    /// </summary>
    public class NFSClient
    {
        #region Enum

        /// <summary>
        /// The NFS version to use
        /// </summary>
        public enum NFSVersion
        {
            /// <summary>
            /// NFS Version 2
            /// </summary>
            v2 = 2,
            /// <summary>
            /// NFS Version 3
            /// </summary>
            v3 = 3,
            /// <summary>
            /// NFS Version 4.1
            /// </summary>
            v4 = 4
        }

        #endregion

        #region Fields

        private NFSPermission _Mode = null;
        private bool _IsMounted = false;
        private bool _IsConnected = false;
        private string _CurrentDirectory = string.Empty;

        private INFS _nfsInterface = null;
        /* Block size must not be greater than 8064 for V2 and
         * 8000 for V3. RPC Buffer size is fixed to 8192, when
         * requesting on RPC, 8192 bytes contain every details
         * of request. we reserve 128 bytes for header information
         * of V2 and 192 bytes for header information of V3.
         * V2: 8064 bytes for data. 
         * V3: 8000 bytes for data. */
        public int _blockSize = 7900;
        //this can change

        #endregion

        #region Events

        public delegate void NFSDataEventHandler(object sender, NFSEventArgs e);

        /// <summary>
        /// This event is fired when data is transferred from/to the server
        /// </summary>
        public event NFSDataEventHandler DataEvent = null;

        public class NFSEventArgs : EventArgs
        {
            private int _Bytes;

            public NFSEventArgs(int Bytes)
            { this._Bytes = Bytes; }

            public int Bytes
            {
                get
                { return this._Bytes; }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// This property tells if the current export is mounted
        /// </summary>
        public bool IsMounted
        {
            get
            { return this._IsMounted; }
        }

        /// <summary>
        /// This property tells if the connection is active
        /// </summary>
        public bool IsConnected
        {
            get
            { return this._IsConnected; }
        }

        /// <summary>
        /// This property allow you to set file/folder access permissions
        /// </summary>
        public NFSPermission Mode
        {
            get
            {
                if (this._Mode == null)
                { this._Mode = new NFSPermission(7, 7, 7); }

                return this._Mode; 
            }
            set
            { this._Mode = value; }
        }

        /// <summary>
        /// This property contains the current server directory
        /// </summary>
        public String CurrentDirectory
        {
            get
            { return this._CurrentDirectory; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// NFS Client Constructor
        /// </summary>
        /// <param name="Version">The required NFS version</param>
        public NFSClient(NFSVersion Version)
        {
            switch (Version)
            {
                case NFSVersion.v2:
                    this._nfsInterface = new NFSv2();
                    break;

                case NFSVersion.v3:
                    this._nfsInterface = new NFSv3();
                    break;

                case NFSVersion.v4:
                    this._nfsInterface = new NFSv4();
                    break; 

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a connection to a NFS Server
        /// </summary>
        /// <param name="Address">The server address</param>
        public void Connect(IPAddress Address)
        { Connect(Address, 0, 0, 60000, System.Text.Encoding.ASCII, true,false); }

        /// <summary>
        /// Create a connection to a NFS Server
        /// </summary>
        /// <param name="Address">The server address</param>
        /// <param name="UserId">The unix user id</param>
        /// <param name="GroupId">The unix group id</param>
        /// <param name="CommandTimeout">The command timeout in milliseconds</param>
        public void Connect(IPAddress Address, int UserId, int GroupId, int CommandTimeout)
        { Connect(Address, UserId, GroupId, CommandTimeout, System.Text.Encoding.ASCII, true,false); }

        /// <summary>
        /// Create a connection to a NFS Server
        /// </summary>
        /// <param name="Address">The server address</param>
        /// <param name="UserId">The unix user id</param>
        /// <param name="GroupId">The unix group id</param>
        /// <param name="CommandTimeout">The command timeout in milliseconds</param>
        /// <param name="characterEncoding">Connection encoding</param>
        /// <param name="useSecurePort">Uses a local binding port less than 1024</param>
        public void Connect(IPAddress Address, int UserId, int GroupId, int CommandTimeout, System.Text.Encoding characterEncoding, bool useSecurePort,bool useCache)
        {
            this._nfsInterface.Connect(Address, UserId, GroupId, CommandTimeout, characterEncoding, useSecurePort, useCache);
            this._IsConnected = true;
        }

        /// <summary>
        /// Close the current connection
        /// </summary>
        public void Disconnect()
        {
            this._nfsInterface.Disconnect();
            this._IsConnected = false;
        }

        /// <summary>
        /// Get the list of the exported NFS devices
        /// </summary>
        /// <returns>A list of the exported NFS devices</returns>
        public List<String> GetExportedDevices()
        {
            return this._nfsInterface.GetExportedDevices();
        }

        /// <summary>
        /// Mount device
        /// </summary>
        /// <param name="DeviceName">The device name</param>
        public void MountDevice(String DeviceName)
        {
            this._nfsInterface.MountDevice(DeviceName);
            //cuz of NFS v4.1 we have to do this after session is created
            this._blockSize = this._nfsInterface.GetBlockSize();
            this._IsMounted = true;
        }

        /// <summary>
        /// Unmount the current device
        /// </summary>
        public void UnMountDevice()
        {
            this._nfsInterface.UnMountDevice();
            this._IsMounted = false;
        }

        /// <summary>
        /// Get the items in a directory
        /// </summary>
        /// <param name="DirectoryFullName">Directory name (e.g. "directory\subdirectory" or "." for the root)</param>
        /// <returns>A list of the items name</returns>
        public List<String> GetItemList(String DirectoryFullName)
        {
            return GetItemList(DirectoryFullName, true);  //changed to true cuz i don't need .. and .
        }

        /// <summary>
        /// Get the items in a directory
        /// </summary>
        /// <param name="DirectoryFullName">Directory name (e.g. "directory\subdirectory" or "." for the root)</param>
        /// <param name="ExcludeNavigationDots">When posted as true, return list will not contains "." and ".."</param>
        /// <returns>A list of the items name</returns>
        public List<String> GetItemList(String DirectoryFullName, Boolean ExcludeNavigationDots)
        {
            DirectoryFullName = CorrectPath(DirectoryFullName);

            System.Collections.Generic.List<String> content = this._nfsInterface.GetItemList(DirectoryFullName);

            if (ExcludeNavigationDots)
            {
                int dotIdx, ddotIdx;

                dotIdx = content.IndexOf(".");
                if (dotIdx > -1)
                    content.RemoveAt(dotIdx);

                ddotIdx = content.IndexOf("..");
                if (ddotIdx > -1)
                    content.RemoveAt(ddotIdx);
            }

            return content;
        }

        /// <summary>
        /// Get an item attribures
        /// </summary>
        /// <param name="ItemFullName">The item full path name</param>
        /// <returns>A NFSAttributes class</returns>
        public NFSAttributes GetItemAttributes(String ItemFullName, bool ThrowExceptionIfNotFoud = true)
        {
            ItemFullName = CorrectPath(ItemFullName);

            return this._nfsInterface.GetItemAttributes(ItemFullName, ThrowExceptionIfNotFoud);
        }

        /// <summary>
        /// Create a new directory
        /// </summary>
        /// <param name="DirectoryFullName">Directory full name</param>
        public void CreateDirectory(String DirectoryFullName)
        { 
            CreateDirectory(DirectoryFullName, this._Mode); 
        }

        /// <summary>
        /// Create a new directory with Permission
        /// </summary>
        /// <param name="DirectoryFullName">Directory full name</param>
        /// <param name="Mode">Directory permissions</param>
        public void CreateDirectory(String DirectoryFullName, NFSPermission Mode)
        {
            DirectoryFullName = CorrectPath(DirectoryFullName);

            String ParentPath = System.IO.Path.GetDirectoryName(DirectoryFullName);

            if (!String.IsNullOrEmpty(ParentPath) &&
                String.Compare(ParentPath, ".") != 0 &&
                !FileExists(ParentPath))
            { 
                CreateDirectory(ParentPath); 
            }

            this._nfsInterface.CreateDirectory(DirectoryFullName, Mode);
        }

        /// <summary>
        /// Delete a directory
        /// </summary>
        /// <param name="DirectoryFullName">Directory full name</param>
        public void DeleteDirectory(String DirectoryFullName)
        {
            DeleteDirectory(DirectoryFullName, false);
        }

        /// <summary>
        /// Delete a directory
        /// </summary>
        /// <param name="DirectoryFullName">Directory full name</param>
        public void DeleteDirectory(String DirectoryFullName, bool recursive)
        {
            DirectoryFullName = CorrectPath(DirectoryFullName);

            if (recursive)
            {
                foreach (String item in GetItemList(DirectoryFullName, true))
                {
                    if (IsDirectory(String.Format("{0}\\{1}", DirectoryFullName, item)))
                    { DeleteDirectory(String.Format("{0}\\{1}", DirectoryFullName, item), recursive); }
                    else
                    { DeleteFile(String.Format("{0}\\{1}", DirectoryFullName, item)); }
                }
            }

            this._nfsInterface.DeleteDirectory(DirectoryFullName);
        }

        /// <summary>
        /// Delete a file 
        /// </summary>
        /// <param name="FileFullName">File full name</param>
        public void DeleteFile(String FileFullName)
        {
            FileFullName = CorrectPath(FileFullName);

            this._nfsInterface.DeleteFile(FileFullName);
        }

        /// <summary>
        /// Create a new file
        /// </summary>
        /// <param name="FileFullName">File full name</param>
        public void CreateFile(String FileFullName)
        { 
            CreateFile(FileFullName, this._Mode); 
        }

        /// <summary>
        /// Create a new file with permission
        /// </summary>
        /// <param name="FileFullName">File full name</param>
        /// <param name="Mode">File permission</param>
        public void CreateFile(String FileFullName, NFSPermission Mode)
        {
            FileFullName = CorrectPath(FileFullName);

            this._nfsInterface.CreateFile(FileFullName, Mode);
        }

        /// <summary>
        /// Copy a set of files from a remote directory to a local directory
        /// </summary>
        /// <param name="SourceFileNames">A list of the remote files name</param>
        /// <param name="SourceDirectoryFullName">The remote directory path (e.g. "directory\sub1\sub2" or "." for the root)</param>
        /// <param name="DestinationDirectoryFullName">The destination local directory</param>
        public void Read(List<String> SourceFileNames, String SourceDirectoryFullName, String DestinationDirectoryFullName)
        {
            if (System.IO.Directory.Exists(DestinationDirectoryFullName))
            {
                foreach (String FileName in SourceFileNames)
                {
                    Read(Combine(FileName, SourceDirectoryFullName), System.IO.Path.Combine(DestinationDirectoryFullName, FileName));
                }
            }
        }

        /// <summary>
        /// Copy a file from a remote directory to a local directory
        /// </summary>
        /// <param name="SourceFilefullName">The remote file name</param>
        /// <param name="DestinationFileFullName">The destination local directory</param>
        public void Read(String SourceFileFullName, String DestinationFileFullName)
        {
            System.IO.Stream fs = null;
            try
            {
                if (System.IO.File.Exists(DestinationFileFullName))
                    System.IO.File.Delete(DestinationFileFullName);
                fs = new System.IO.FileStream(DestinationFileFullName, System.IO.FileMode.CreateNew);
                Read(SourceFileFullName, ref fs);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }

        /// <summary>
        /// Copy a file from a remote directory to a stream
        /// </summary>
        /// <param name="SourceFileFullName">The remote file name</param>
        /// <param name="OutputStream"></param>
        public void Read(String SourceFileFullName, ref System.IO.Stream OutputStream)
        {
            if (OutputStream != null)
            {
                SourceFileFullName = CorrectPath(SourceFileFullName);

                if (!FileExists(SourceFileFullName))
                    throw new System.IO.FileNotFoundException();
                NFSAttributes nfsAttributes = GetItemAttributes(SourceFileFullName, true);
                long TotalRead = nfsAttributes.Size, ReadOffset = 0;

                Byte[] ChunkBuffer = (Byte[])Array.CreateInstance(typeof(Byte), this._blockSize);
                int ReadCount, ReadLength = this._blockSize;

                do
                {
                    if (TotalRead < ReadLength)
                    { 
                        ReadLength = (int)TotalRead; 
                    }

                    ReadCount = this._nfsInterface.Read(SourceFileFullName, ReadOffset, ReadLength, ref ChunkBuffer);

                    if (this.DataEvent != null)
                    { 
                        this.DataEvent(this, new NFSEventArgs(ReadCount)); 
                    }

                    OutputStream.Write(ChunkBuffer, 0, ReadCount);

                    TotalRead -= ReadCount; ReadOffset += ReadCount;
                } 
                while (ReadCount != 0);

                OutputStream.Flush();

                CompleteIO();
            }
            else
            { 
                throw new NullReferenceException("OutputStream parameter must not be null!"); 
            }
        }

        /// <summary>
        /// Copy a remote file to a buffer, CompleteIO proc must called end of the reading process for system stability
        /// </summary>
        /// <param name="SourceFileFullName">The remote file full path</param>
        /// <param name="Offset">Start offset</param>
        /// <param name="Count">Number of bytes</param>
        /// <param name="Buffer">Output buffer</param>
        /// <returns>The number of copied bytes</returns>
        public long Read(String SourceFileFullName, long Offset, long TotalLenght, ref Byte[] Buffer)
        {
            /* This function is not suitable for large file reading.
             * Big file reading will cause OS paging creation and
             * huge memory consumption. 
             */
            SourceFileFullName = CorrectPath(SourceFileFullName);

            long ExactTotalLength = TotalLenght - Offset, CurrentPosition = 0;

            /* Prepare full Buffer to read */
            Buffer = (Byte[])Array.CreateInstance(typeof(Byte), ExactTotalLength);

            Byte[] ChunkBuffer = (Byte[])Array.CreateInstance(typeof(Byte), this._blockSize);
            int ReadCount = 0, ReadLength = this._blockSize;

            do
            {
                if ((ExactTotalLength - CurrentPosition) < ReadLength)
                    ReadLength = (int)(ExactTotalLength - CurrentPosition);

                ReadCount = this._nfsInterface.Read(SourceFileFullName, Offset + CurrentPosition, ReadLength, ref ChunkBuffer);

                if (this.DataEvent != null)
                { this.DataEvent(this, new NFSEventArgs(ReadCount)); }

                Array.Copy(ChunkBuffer, 0, Buffer, CurrentPosition, ReadCount);

                CurrentPosition += ReadCount;
            } 
            while (ReadCount != 0);

            return CurrentPosition;
        }

        /// <summary>
        /// Copy a remote file to a buffer
        /// </summary>
        /// <param name="SourceFileFullName">The remote file full path</param>
        /// <param name="Offset">Start offset</param>
        /// <param name="Count">Number of bytes</param>
        /// <param name="Buffer">Output buffer</param>
        public void Read(String SourceFileFullName, Int64 Offset, ref Int64 TotalLenght, ref Byte[] Buffer)
        {
            SourceFileFullName = CorrectPath(SourceFileFullName);

            UInt32 BlockSize = (UInt32)_blockSize;
            UInt32 CurrentPosition = 0;
            do
            {
                UInt32 ChunkCount = BlockSize;
                if ((TotalLenght - CurrentPosition) < BlockSize)
                    ChunkCount = (UInt32)TotalLenght - CurrentPosition;

                Byte[] ChunkBuffer = new Byte[ChunkCount];
                int Size = _nfsInterface.Read(SourceFileFullName, Offset + CurrentPosition, (int) ChunkCount, ref ChunkBuffer);

                if (DataEvent != null)
                    DataEvent(this, new NFSEventArgs((int)ChunkCount));

                if (Size == 0)
                {
                    TotalLenght = CurrentPosition;
                    return;
                }

                Array.Copy(ChunkBuffer, 0, Buffer, CurrentPosition, Size);
                CurrentPosition += (UInt32)Size;

            } while (CurrentPosition != TotalLenght);
        }

        /// <summary>
        /// Copy a local file to a remote directory
        /// </summary>
        /// <param name="DestinationFileFullName">The destination file full name</param>
        /// <param name="SourceFileFullName">The local full file path</param>
        public void Write(String DestinationFileFullName, String SourceFileFullName)
        {
            if (System.IO.File.Exists(SourceFileFullName))
            {
                System.IO.FileStream wfs = new System.IO.FileStream(SourceFileFullName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                Write(DestinationFileFullName, wfs);
                wfs.Close();
            }
        }

        /// <summary>
        /// Copy a local file to a remote file
        /// </summary>
        /// <param name="DestinationFileFullName">The destination full file name</param>
        /// <param name="InputStream">The input file stream</param>
        public void Write(String DestinationFileFullName, System.IO.Stream InputStream)
        {
            Write(DestinationFileFullName, 0, InputStream);
        }

        /// <summary>
        /// Copy a local file stream to a remote file
        /// </summary>
        /// <param name="DestinationFileFullName">The destination full file name</param>
        /// <param name="InputOffset">The input offset in bytes</param>
        /// <param name="InputStream">The input stream</param>
        public void Write(String DestinationFileFullName, long InputOffset, System.IO.Stream InputStream)
        {
            if (InputStream != null)
            {
                DestinationFileFullName = CorrectPath(DestinationFileFullName);

                if (!FileExists(DestinationFileFullName))
                    CreateFile(DestinationFileFullName);

                long Offset = InputOffset;

                Byte[] Buffer = (Byte[])Array.CreateInstance(typeof(Byte), this._blockSize);
                int ReadCount, WriteCount;

                do
                {
                    ReadCount = InputStream.Read(Buffer, 0, Buffer.Length);

                    if (ReadCount != 0)
                    {
                        WriteCount = this._nfsInterface.Write(DestinationFileFullName, Offset, ReadCount, Buffer);

                        if (this.DataEvent != null)
                        { this.DataEvent(this, new NFSEventArgs(WriteCount)); }

                        Offset += ReadCount;
                    }
                } while (ReadCount != 0);

                CompleteIO();
            }
            else
            { throw new NullReferenceException("InputStream parameter must not be null!"); }
        }

        /// <summary>
        /// Copy a local file  to a remote directory, CompleteIO proc must called end of the writing process for system stability
        /// </summary>
        /// <param name="DestinationFileFullName">The full local file path</param>
        /// <param name="Offset">The start offset in bytes</param>
        /// <param name="Count">The number of bytes to write</param>
        /// <param name="Buffer">The input buffer</param>
         public void Write(String DestinationFileFullName, long Offset, int Count, Byte[] Buffer)
        {
            DestinationFileFullName = CorrectPath(DestinationFileFullName);

            if (!FileExists(DestinationFileFullName))
                CreateFile(DestinationFileFullName);

            long CurrentPosition = 0;

            Byte[] ChunkBuffer = (Byte[])Array.CreateInstance(typeof(Byte), this._blockSize);
            int WriteCount = 0, WriteLength = this._blockSize;

            do
            {
                if ((Count - CurrentPosition) < WriteLength)
                { WriteLength = (int)(Count - CurrentPosition); }

                Array.Copy(Buffer, CurrentPosition, ChunkBuffer, 0, WriteLength);
                WriteCount = this._nfsInterface.Write(DestinationFileFullName, Offset + CurrentPosition, WriteLength, ChunkBuffer);

                if (this.DataEvent != null)
                { this.DataEvent(this, new NFSEventArgs(WriteCount)); }

                CurrentPosition += WriteCount;
            } while (Count != CurrentPosition);
        }

        /// <summary>
        /// Copy a local file  to a remote directory
        /// </summary>
        /// <param name="DestinationFileFullName">The full local file path</param>
        /// <param name="Offset">The start offset in bytes</param>
        /// <param name="Count">The number of bytes</param>
        /// <param name="Buffer">The input buffer</param>
        /// <returns>Returns the total written bytes</returns>
        public void Write(String DestinationFileFullName, Int64 Offset, UInt32 Count, Byte[] Buffer, out UInt32 TotalLenght)
        {
            DestinationFileFullName = CorrectPath(DestinationFileFullName);

            TotalLenght = Count;
            UInt32 BlockSize = (UInt32)_blockSize;
            UInt32 CurrentPosition = 0;
            if (Buffer != null)
            {
                do
                {
                    Int32 Size = -1;
                    UInt32 ChunkCount = BlockSize;
                    if ((TotalLenght - CurrentPosition) < BlockSize)
                        ChunkCount = (UInt32)TotalLenght - CurrentPosition;

                    Byte[] ChunkBuffer = new Byte[ChunkCount];
                    Array.Copy(Buffer, (int)CurrentPosition, ChunkBuffer, 0, (Int32)ChunkCount);
                    Size = _nfsInterface.Write(DestinationFileFullName, Offset + CurrentPosition, (int) ChunkCount, ChunkBuffer);
                    if (DataEvent != null)
                        DataEvent(this, new NFSEventArgs((int)ChunkCount));
                    if (Size == 0)
                    {
                        TotalLenght = CurrentPosition;
                        return;
                    }
                    CurrentPosition += (UInt32)ChunkCount;

                } while (CurrentPosition != TotalLenght);
            }
        }

        /// <summary>
        /// Move a file from/to a directory
        /// </summary>
        /// <param name="SourceFileFullName">The exact file location for source (e.g. "directory\sub1\sub2\filename" or "." for the root)</param>
        /// <param name="TargetFileFullName">Target location of moving file (e.g. "directory\sub1\sub2\filename" or "." for the root)</param>
        public void Move(String SourceFileFullName, String TargetFileFullName)
        {
            if (!String.IsNullOrEmpty(TargetFileFullName))
            {
                if (TargetFileFullName.LastIndexOf('\\') + 1 == TargetFileFullName.Length)
                {
                    TargetFileFullName = System.IO.Path.Combine(
                                            TargetFileFullName,
                                            System.IO.Path.GetFileName(SourceFileFullName));
                }
            }

            SourceFileFullName = CorrectPath(SourceFileFullName);
            TargetFileFullName = CorrectPath(TargetFileFullName);

            this._nfsInterface.Move(
                System.IO.Path.GetDirectoryName(SourceFileFullName),
                System.IO.Path.GetFileName(SourceFileFullName),
                System.IO.Path.GetDirectoryName(TargetFileFullName),
                System.IO.Path.GetFileName(TargetFileFullName)
            );
        }

        /// <summary>
        /// Check if the passed path refers to a directory
        /// </summary>
        /// <param name="DirectoryFullName">The full path (e.g. "directory\sub1\sub2" or "." for the root)</param>
        /// <returns>True if is a directory</returns>
        public bool IsDirectory(String DirectoryFullName)
        {
            DirectoryFullName = CorrectPath(DirectoryFullName);

            return this._nfsInterface.IsDirectory(DirectoryFullName);
        }

        /// <summary>
        /// Completes Current Read/Write Caching and Release Resources
        /// </summary>
        public void CompleteIO()
        { this._nfsInterface.CompleteIO(); }

        /// <summary>
        /// Check if a file/directory exists
        /// </summary>
        /// <param name="FileFullName">The item full name</param>
        /// <returns>True if exists</returns>
        public bool FileExists(String FileFullName)
        {
            FileFullName = CorrectPath(FileFullName);

            return (GetItemAttributes(FileFullName, false) != null);
        }

        /// <summary>
        /// Get the file/directory name from a standard windwos path (eg. "\\test\text.txt" --> "text.txt" or "\\" --> ".")
        /// </summary>
        /// <param name="FullFilePath">The source path</param>
        /// <returns>The file/directory name</returns>
        public String GetFileName(String FileFullName)
        {
            FileFullName = CorrectPath(FileFullName);

            String str = System.IO.Path.GetFileName(FileFullName);
            if (String.IsNullOrEmpty(str))
                str = ".";

            return str;
        }

        /// <summary>
        /// Get the directory name from a standard windwos path (eg. "\\test\test1\text.txt" --> "test\\test1" or "\\" --> ".")
        /// </summary>
        /// <param name="FullDirectoryName">The full path(e.g. "directory/sub1/sub2" or "." for the root)</param>
        /// <returns>The directory name</returns>
        public string GetDirectoryName(String FullDirectoryName)
        {
            FullDirectoryName = CorrectPath(FullDirectoryName);

            String str = System.IO.Path.GetDirectoryName(FullDirectoryName);
            if (String.IsNullOrEmpty(str))
                str = ".";

            return str;
        }

        /// <summary>
        /// Combine a file name to a directory (eg. FileName "test.txt", Directory "test" --> "test\test.txt" or FileName "test.txt", Directory "." --> "test.txt")
        /// </summary>
        /// <param name="FileName">The file name</param>
        /// <param name="DirectoryName">The directory name (e.g. "directory\sub1\sub2" or "." for the root)</param>
        /// <returns>The combined path</returns>
        public string Combine(String FileName, String DirectoryFullName)
        {
            DirectoryFullName = CorrectPath(DirectoryFullName);

            return String.Format("{0}\\{1}", DirectoryFullName, FileName);
        }

        /// <summary>
        /// Set the file size
        /// </summary>
        /// <param name="FileFullName">The file full path</param>
        /// <param name="Size">the size in bytes</param>
        public void SetFileSize(String FileFullName, long Size)
        {
            FileFullName = CorrectPath(FileFullName);

            this._nfsInterface.SetFileSize(FileFullName, Size);
        }

        private string CorrectPath(String PathEntry)
        {
            if (!String.IsNullOrEmpty(PathEntry))
            {
                String[] PathList = PathEntry.Split('\\');
                List<string> newPathList = new List<string>();

                foreach (String item in PathList)
                {
                    if (!String.IsNullOrEmpty(item))
                    { newPathList.Add(item); }
                }

                PathEntry = String.Join("\\", newPathList.ToArray());

                if (PathEntry.IndexOf('.') != 0)
                { PathEntry = String.Concat(".\\", PathEntry); }
            }

            return PathEntry;
        }

        #endregion
    }
}
