using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NFSLibrary;
using System.Net;
using System.Threading;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.IO;
using Dokan;
using System.Diagnostics;
using NFSLibrary.Protocols.V3.RPC;
using NFSLibrary.Protocols.Commons;

namespace NFSClient
{
    public partial class MainForm : Form
    {


        #region Fields
        public bool connected = false;
        NFSLibrary.NFSClient nfsClient;
        List<string> nfsDevs = null;
        DragDropEffects CurrentEffect;
        List<ListViewItem> lvDragItem = new List<ListViewItem>();
        string CurrentList;
        string CurrentItem;
        string CurrentDir;
        string CurrentFilePos;
        long CurrentSize;
        delegate void ShowProgressDelegate(bool ShowHide,bool move);
        ShowProgressDelegate show;
        delegate void UpdateProgressDelegate(string name, long total, int current);
        UpdateProgressDelegate update;
        Thread downloadThread;
        Thread uploadThread;
        string LocalFolder = string.Empty;
        string RemoteFolder = ".";
        string CurrentRemoteFolder = ".";
        long TotalTransferredBytes = 0;
        int TotalTransferredSpeedChecks = 0;
        long AvgDwnSpeed = 0;
        Progress pr;
        int server_index = 0;
        public string serverAdress = null;
        public int serverUid = 0;
        public int serverGid = 0;
        public bool serverSecurePort = false;
        public string serverVersion = "v2";
        private string MountPoint;
        DokanOperations dokanOperation = null;
        public bool mountedDrive = false;
        #endregion

        public MainForm()
        {
            InitializeComponent();
            pr = new Progress(this);
            show = new ShowProgressDelegate(ShowProgress);
            update = new UpdateProgressDelegate(UpdateProgress);
            ShowProgress(false,false);
            //cboxVer.SelectedIndex = 0;
            addToStatusLog("Init()");

            //local folder path
            string path="";
            if (NFSClient.Properties.Settings.Default.DefaultLocalFolder== "home")
            path = (Environment.OSVersion.Platform == PlatformID.Unix ||
                   Environment.OSVersion.Platform == PlatformID.MacOSX)
    ? Environment.GetEnvironmentVariable("HOME")
    : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            else
                path = NFSClient.Properties.Settings.Default.DefaultLocalFolder;
            RefreshLocal(path);


            if (NFSClient.Properties.Settings.Default.ServerName.Length > 0)
            {

                //serverListCombo.Items.AddRange(NFSClient.Properties.Settings.Default.ServerName);
                int i = 0;
                foreach (string item in NFSClient.Properties.Settings.Default.ServerName)
                {
                    serverListCombo.Items.Add(item + "  (" + NFSClient.Properties.Settings.Default.ServerAddres[i] + "@" + NFSClient.Properties.Settings.Default.DefaultProtocol[i]+ ")");
                    i++;
                }


                server_index = NFSClient.Properties.Settings.Default.lastServerId;
                serverListCombo.SelectedIndex = server_index;
                //auto connect
                if (NFSClient.Properties.Settings.Default.autoConnect)
                    connect();
            }
            else
            {
                AddServer firstTime = new AddServer(this,-1);
                firstTime.StartPosition = FormStartPosition.Manual;
                firstTime.Location = new Point(this.Location.X + (this.Width - firstTime.Width) / 2, this.Location.Y + (this.Height - firstTime.Height) / 2);
                firstTime.Show(this);
            }
        }

        private void button_connect_event(object sender, EventArgs e)
        {
            connect();
        }



        public void connect()
        {
            if (connected)
            {
                dissconect(false);
                return;
            }


            if (serverAdress == null)
            {
                MessageBox.Show("Please add a server to server list or add one time server.", "NFS Client");
                return;
            }

            IPAddress ipAddress;


            if (GetResolvedConnecionIPAddress(serverAdress, out ipAddress))
            {
                addToStatusLog("Resolved IP: " + ipAddress.ToString());
            }
            else
                return;

            bool pinged = false;

            try
            {
               pinged = PingServer(ipAddress);
            }
            catch (Exception e)
            {
                MessageBox.Show("Can't ping server, check your connection,host adress and NFS server. Code: " + e.Message, "NFS Client");
                addToStatusLog("Connection failed!");
                return;
            }

            if (pinged)
            {
                addToStatusLog("Pinged server...OK");
                //TODO
                NFSLibrary.NFSClient.NFSVersion ver;
                if (serverVersion == "v3")
                {
                    ver = NFSLibrary.NFSClient.NFSVersion.v3;
                    addToStatusLog("Using NFS version 3");
                }
                else if (serverVersion == "v2")
                {
                    ver = NFSLibrary.NFSClient.NFSVersion.v2;
                    addToStatusLog("Using NFS version 2");
                }
                else
                {
                    ver = NFSLibrary.NFSClient.NFSVersion.v4;
                    addToStatusLog("Using NFS version 4.1 !!! (experimantal)");
                }

                 addToStatusLog("Using uid: " + serverUid);


                nfsClient = new NFSLibrary.NFSClient(ver);
                nfsClient.DataEvent += new NFSLibrary.NFSClient.NFSDataEventHandler(nfsClient_DataEvent);
                Encoding encoding = Encoding.UTF8;       
                addToStatusLog("Connecting....");

                try
                {
                    nfsClient.Connect(ipAddress, serverUid,serverGid, NFSClient.Properties.Settings.Default.ConnectionTimeout * 1000, encoding,serverSecurePort, NFSClient.Properties.Settings.Default.UseFhCache);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Connection failed, check your host adress and NFS server. Code: " + ex.Message, "NFS Client");
                    addToStatusLog("Connection failed!");
                    return;
                }
                addToStatusLog("Connected!");
                connected = true;

                this.Text = "NFS Client - Connected, " + ipAddress + ", " + serverVersion;

                button_connect.Text = "Disconnect";
                label2.Visible = true;
                cboxRemoteDevices.Visible = true;
                addToStatusLog("Geting exported devices");
                try
                {
                    nfsDevs = nfsClient.GetExportedDevices();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "NFS Client - Error");
                    addToStatusLog("Connection failed!");
                    return;
                }
                cboxRemoteDevices.Items.Clear();
                foreach (string nfsdev in nfsDevs)
                    cboxRemoteDevices.Items.Add(nfsdev);

                listViewRemote.Items.Clear();

                //automatic connect to fist one
                if (cboxRemoteDevices.Items.Count > 0)
                {
                    try
                    {
                        //selected index change automaticly mount the device anyway
                        cboxRemoteDevices.SelectedIndex = 0;
                        //MountDevice(0);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "NFS Client - Error");
                    }
                }

            }
            else
                MessageBox.Show("Server not found!", "NFS Client"); 
        }



        void ShowProgress(bool Show,bool move)
        {
            if (pb.InvokeRequired)
            {
                pb.Invoke(show, new object[] { Show,move });
            }
            else
            {
                if (Show)
                {
                    AvgDwnSpeed = 0;
                    TotalTransferredBytes = 0;
                    TotalTransferredSpeedChecks = 0;
                    timer1.Start();
                    if (CurrentList != "Local")
                    {
                        pr.pictureBox2.Visible = true;
                        pr.pictureBox3.Visible = false;
                    }
                    else
                    {
                        pr.pictureBox3.Visible = true;
                        pr.pictureBox2.Visible = false;
                    }

                    pr.Show();
                }
                else
                {
                    timer1.Stop();
                    pr.Hide();
                    pr.progressBar1.Value = 0;
                    if (move)
                    {
                        RefreshLocal(LocalFolder);
                        tryToRefreshRemote();
                    }
                    else{
                        if (CurrentList != "Local")
                            RefreshLocal(LocalFolder);
                        else
                            tryToRefreshRemote();
                        }
                }
            }
        }

        long _lTotal = 0;
        void UpdateProgress(string name, long total, int current)
        {

            if (pr.progressBar1.InvokeRequired)
            {
                pr.progressBar1.Invoke(update, new object[] { name, total, current });
            }
            else
            {
                if (!connected)
                    return;

                if (current > 0)
                {
                    if (_lTotal == 0)
                    { _lTotal = total; }
                    _lTotal -= current;

                    float KbitSec = AvgDwnSpeed / 1024;
                    float MbitSec = 0;
                    string speedText = string.Empty;
                    if (KbitSec > 1024)
                    {
                        MbitSec = KbitSec / 1024;
                        speedText = (MbitSec).ToString("0.00") + " MB/s";
                    }
                    else
                    {
                        speedText = (KbitSec).ToString("0.00") + " KB/s";
                    }
                    if (speedText != string.Empty)
                    {
                        pr.label1.Text = CurrentItem;
                        pr.label2.Text = speedText;
                        pr.label3.Text = CurrentFilePos;
                        pr.label5.Text = CurrentDir;
                    }
                    pr.progressBar1.Maximum = (int)(total / current);
                    int Value = pr.progressBar1.Value + 1;
                    if (Value < pr.progressBar1.Maximum)
                        pr.progressBar1.Value = Value;
                    else
                        pr.progressBar1.Value = pr.progressBar1.Maximum;
                }
            }
        }

        public  void RefreshLocal(string Dir)
        {
            if (Dir == string.Empty)
                return;

            string previus = tbLocalPath.Text;


            try
            {
                Environment.CurrentDirectory = tbLocalPath.Text = Dir;
                System.IO.DirectoryInfo CurrentDirecotry = new System.IO.DirectoryInfo(tbLocalPath.Text);
                listViewLocal.Items.Clear();
                if (System.IO.Path.GetDirectoryName(Dir) != null)
                {
                    ListViewItem parent = new ListViewItem(new string[] { ".." });
                    parent.ImageIndex = 1;
                    listViewLocal.Items.Add(parent);
                }
                foreach (System.IO.DirectoryInfo dir in CurrentDirecotry.GetDirectories())
                {
                    ListViewItem lvi = new ListViewItem(new string[] { dir.Name, "", dir.LastWriteTime.ToString() });
                    lvi.ImageIndex = 1;
                    listViewLocal.Items.Add(lvi);
                }
                foreach (System.IO.FileInfo file in CurrentDirecotry.GetFiles())
                {
                    ListViewItem lvi = new ListViewItem(new string[] { file.Name, file.Length.ToString(), file.LastWriteTime.ToString() });
                    lvi.ImageIndex = 0;
                    listViewLocal.Items.Add(lvi);
                }
                if (previus!=Dir)
                    addToStatusLog("Changed local to: " + Dir);
                else
                    addToStatusLog("Refreshed local");

            }
            catch (Exception ex)
            {
                Environment.CurrentDirectory = tbLocalPath.Text = previus;
                MessageBox.Show(ex.Message, "NFS Client - Error");
            }
        }



        void tryToRefreshRemote()
        {
            try
            {
                RefreshRemote();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "NFS Client - Error");
                if (RemoteFolder != ".")
                {
                    //go one level back aka back to the directory called
                    RemoteFolder = nfsClient.GetDirectoryName(RemoteFolder);
                    RefreshRemote();
                }
            }
        }


        void RefreshRemote()
        {
            if (!connected)
                return;
            listViewRemote.Items.Clear();
            List<FolderEntry> Items = nfsClient.GetItemListEx(RemoteFolder, false);
            bool big_folder = false;
            if (Items.Count > 500)
            {
                addToStatusLog("Readed: " + Items.Count + " files and folders.");
                big_folder = true;
            }
            //maybe it's better to remove it
            //Items = RemoveDuplicatesAndInvalid(Items);
            int folders = 0;
            if (RemoteFolder != ".")
            {
                listViewRemote.Items.Add(new ListViewItem("..",1));
                folders = 1;
            }
                
            Progress rm = new Progress(this);

            List<ListViewItem> FilesList = new List<ListViewItem>();
            int i = 0;
            foreach (FolderEntry Item in Items)
            {
                i++;

                // No need to make a seperate call to GetAttributes if the original list command was GetItemListEx (includes attributes)
                //nfsClient.GetItemAttributes(nfsClient.Combine(Item, RemoteFolder));
                NFSLibrary.Protocols.Commons.NFSAttributes nfsAttribute = new NFSAttributes(
                                                                                    Item.Attributes.Attributes.CreateTime.Seconds,
                                                                                    Item.Attributes.Attributes.LastAccessedTime.Seconds,
                                                                                    Item.Attributes.Attributes.ModifiedTime.Seconds,
                                                                                    Item.Attributes.Attributes.Type,
                                                                                    Item.Attributes.Attributes.Mode,
                                                                                    Item.Attributes.Attributes.Size,
                                                                                    Item.Handle.Value);
                
                if (nfsAttribute != null)
                {
                    if (nfsAttribute.NFSType == NFSLibrary.Protocols.Commons.NFSItemTypes.NFDIR)
                    {
                        ListViewItem lvi = new ListViewItem(new string[] { Item.Name.Value, nfsAttribute.Size.ToString(), nfsAttribute.Mode.ToString(), nfsAttribute.CreateDateTime.ToString() });
                        lvi.ImageIndex = 1;
                        listViewRemote.Items.Insert(folders, lvi);
                        folders++;
                    }
                    else
                        if (nfsAttribute.NFSType == NFSLibrary.Protocols.Commons.NFSItemTypes.NFREG)
                        {
                            ListViewItem lvi = new ListViewItem(new string[] { Item.Name.Value, nfsAttribute.Size.ToString(), nfsAttribute.Mode.ToString(), nfsAttribute.ModifiedDateTime.ToString(), nfsAttribute.LastAccessedDateTime.ToString() });
                            if (Item.Name.Value == "..")
                                continue;

                            lvi.ImageIndex = 0;
                            listViewRemote.Items.Add(lvi);
                        }
                }
                else
                {
                    ListViewItem lvi = new ListViewItem(new string[] { Item.Name.Value, "", "" });
                    lvi.ImageIndex = 1;
                    listViewRemote.Items.Add(lvi);
                }
                if (big_folder && (i % 500 == 0))
                {
                    if(i==500)
                    listViewRemote.Update();
                    addToStatusLog("Fetching file attribute: " + i + "/" + Items.Count);
                }

            }



            if(CurrentRemoteFolder!=RemoteFolder)
                addToStatusLog("Changed remote to: " + RemoteFolder);
            else
                addToStatusLog("Refreshed remote");
            CurrentRemoteFolder = RemoteFolder;
        }

        private List<string> RemoveDuplicatesAndInvalid(List<string> items)
        {
            List<string> outputList = new List<string>();
            foreach (string item in items)
            {
                string cleanItem = item.Trim();
                if (!outputList.Contains(cleanItem) && cleanItem != ".." && cleanItem != "." && !cleanItem.Contains(".."))
                    outputList.Add(item);
            }
            return outputList;
        }

        void MountDevice(int i)
        {
            if (!connected)
                return;
            listViewRemote.Items.Clear();
            nfsClient.MountDevice(nfsDevs[i]);
            RefreshRemote();
        }

        public bool PingServer(IPAddress Ip)
        {
            //ping the server
            Ping pingSender = new Ping();
            PingOptions pingOptions = new PingOptions();
            pingOptions.DontFragment = true;
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 5000;
            PingReply reply = pingSender.Send(Ip, timeout, buffer, pingOptions);
            if (reply.Status == IPStatus.Success)
                return true;
            else
                return false;
        }

        #region Local Event

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (DialogResult.OK == fbd.ShowDialog())
                {
                    RefreshLocal(LocalFolder = fbd.SelectedPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "NFS Client");
            }
        }

        private void cboxRemoteDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!connected)
                return;
            try
            {
                int i = cboxRemoteDevices.SelectedIndex;
                if (i != -1)
                {
                    RemoteFolder = ".";
                    CurrentRemoteFolder = ".";
                    MountDevice(i);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "NFS Client");
                // pnlMain.Enabled = false;
            }
        }

        private void listViewLocal_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (!connected)
                return;
            CurrentList = "Local";
            CurrentEffect = DragDropEffects.Copy;
            ListView.SelectedListViewItemCollection sl = listViewLocal.SelectedItems;
            Bitmap bmp = (Bitmap)listViewLocal.SmallImageList.Images[sl[0].ImageIndex];
            lvDragItem.Clear();
            foreach (ListViewItem lvi in sl)
            {
                lvDragItem.Add(lvi);
            }
            this.DoDragDrop(bmp, CurrentEffect);
        }

        private void listViewLocal_DragDrop(object sender, DragEventArgs e)
        {
            if (!connected)
                return;
            if (CurrentList != "Local")
            {
                addToStatusLog("Copying " + lvDragItem.Count + " files to local.");
                if (CurrentEffect == DragDropEffects.Copy)
                {
                    LocalFolder = tbLocalPath.Text;
                    downloadThread = new Thread(new ParameterizedThreadStart(Download));
                    downloadThread.Start(false);
                }
            }
        }

        void Download(object move)
        {
            bool movebol = (bool)move;
            if (!connected)
                return;
            try
            {
                ShowProgress(true,movebol);
                int totalFiles = lvDragItem.Count();
                int currentFile = 0;
                bool rewriteAll = false;
                bool keepAll = false;
                foreach (ListViewItem lvItem in lvDragItem)
                {
                    currentFile++;
                    CurrentFilePos = currentFile + "/" + totalFiles;
                    string OutputFile = System.IO.Path.Combine(LocalFolder, lvItem.Text);
                    if (nfsClient.IsDirectory(nfsClient.Combine(lvItem.Text, RemoteFolder)))
                    {
                        DownloadRecursive(movebol, rewriteAll, keepAll, nfsClient.Combine(lvItem.Text, RemoteFolder), OutputFile);
                        if (movebol)
                            nfsClient.DeleteDirectory(nfsClient.Combine(lvItem.Text, RemoteFolder));
                        continue;
                    }

                    if (System.IO.File.Exists(OutputFile))
                    {

                        if (rewriteAll)
                        {
                            try
                            {
                                System.IO.File.Delete(OutputFile);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("An error has occurred deleting the file (" + ex.Message + ")", "NFS Client", MessageBoxButtons.OK);
                                continue;
                            }
                        }
                        else if (keepAll)
                            continue;
                        else
                        {
                            Rewrite rw = new Rewrite("NFSClient", "Do you want to overwrite " + OutputFile + "?");
                            DialogResult dl = rw.ShowDialog();
                            if (dl == DialogResult.OK || dl == DialogResult.Retry)
                            {
                                try
                                {
                                    System.IO.File.Delete(OutputFile);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("An error has occurred deleting the file (" + ex.Message + ")", "NFS Client", MessageBoxButtons.OK);
                                    continue;
                                }

                                if (dl == DialogResult.Retry)
                                    rewriteAll = true;
                            }
                            else
                            {
                                if (dl == DialogResult.Ignore)
                                    keepAll = true;

                                continue;
                            }

                        }
                    }
                    CurrentItem = lvItem.Text;
                    CurrentSize = long.Parse(lvItem.SubItems[1].Text);
                    CurrentDir = LocalFolder;
                    _lTotal = 0;
                    nfsClient.Read(nfsClient.Combine(CurrentItem, RemoteFolder), OutputFile);
                    if (movebol)
                        nfsClient.DeleteFile(nfsClient.Combine(CurrentItem, RemoteFolder));
                }
                ShowProgress(false,movebol);
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "NFS Client");
                ShowProgress(false,movebol);
            }
        }


        void DownloadRecursive(bool move,bool rewriteAll,bool keepAll,string remoteFolder,string localfolder)
        {
            if (!Directory.Exists(localfolder))
            {
                Directory.CreateDirectory(localfolder);
            }

            List<string> folder_contents =  nfsClient.GetItemList(remoteFolder);
            int totalFiles = folder_contents.Count();
            int currentFile = 0;
            foreach (string Item in folder_contents)
            {
                currentFile++;
                CurrentFilePos = currentFile + "/" + totalFiles;
                CurrentItem = Item;
                CurrentDir = localfolder;
                string current_remote_file_path = nfsClient.Combine(Item, remoteFolder);
                string current_local_file_path = System.IO.Path.Combine(localfolder,Item);

                if (nfsClient.IsDirectory(current_remote_file_path))
                {
                    DownloadRecursive(move, rewriteAll, keepAll, current_remote_file_path, current_local_file_path);
                    if (move)
                        nfsClient.DeleteDirectory(current_remote_file_path);
                    continue;
                }

                if (System.IO.File.Exists(current_local_file_path))
                {

                    if (rewriteAll)
                    {
                        try
                        {
                            System.IO.File.Delete(current_local_file_path);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("An error has occurred deleting the file (" + ex.Message + ")", "NFS Client", MessageBoxButtons.OK);
                            continue;
                        }
                    }
                    else if (keepAll)
                        continue;

                    else
                    {
                        Rewrite rw = new Rewrite("NFSClient", "Do you want to overwrite " + current_local_file_path + "?");
                        DialogResult dl = rw.ShowDialog();
                        if (dl == DialogResult.OK || dl == DialogResult.Retry)
                        {
                            try
                            {
                                System.IO.File.Delete(current_local_file_path);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("An error has occurred deleting the file (" + ex.Message + ")", "NFS Client", MessageBoxButtons.OK);
                                continue;
                            }

                            if (dl == DialogResult.Retry)
                                rewriteAll = true;
                        }
                        else
                        {
                            if (dl == DialogResult.Ignore)
                                keepAll = true;

                            continue;
                        }

                    }

                }



                CurrentSize = nfsClient.GetItemAttributes(current_remote_file_path).Size;

                _lTotal = 0;
                nfsClient.Read(current_remote_file_path, current_local_file_path);
                if (move)
                    nfsClient.DeleteFile(current_remote_file_path);

            } 
        }

        void Upload(object move)
        {
            bool movebol =(bool)move;
            if (!connected)
                return;
            try
            {
                ShowProgress(true,movebol);
                int totalFiles = lvDragItem.Count();
                int currentFile = 0;
                bool rewriteAll = false;
                bool keepAll = false;
                foreach (ListViewItem lvItem in lvDragItem)
                {
                    currentFile++;
                    CurrentFilePos = currentFile + "/" + totalFiles;
                    CurrentItem = lvItem.Text;
                    CurrentDir = RemoteFolder;
                    if (Directory.Exists(System.IO.Path.Combine(LocalFolder, lvItem.Text)))
                    {
                        UploadRecursive(movebol,rewriteAll,keepAll,System.IO.Path.Combine(LocalFolder, lvItem.Text),nfsClient.Combine(CurrentItem, RemoteFolder));
                        if(movebol)
                        Directory.Delete(System.IO.Path.Combine(LocalFolder, lvItem.Text));
                        continue;
                    }
                    if (nfsClient.FileExists(nfsClient.Combine(lvItem.Text, RemoteFolder)))
                    {
                        if (rewriteAll)
                        {
                            nfsClient.DeleteFile(nfsClient.Combine(lvItem.Text, RemoteFolder));
                        }
                        else if (keepAll)
                            continue;

                        else
                        {

                            Rewrite rw = new Rewrite("NFSClient","Do you want to overwrite " + lvItem.Text + "?");
                            DialogResult dl = rw.ShowDialog();
                            if (dl == DialogResult.OK || dl == DialogResult.Retry)
                            {

                                nfsClient.DeleteFile(nfsClient.Combine(lvItem.Text, RemoteFolder));

                                if (dl == DialogResult.Retry)
                                    rewriteAll = true;
                            }
                            else
                            {
                                if (dl == DialogResult.Ignore)
                                    keepAll = true;

                                continue;
                            }
                        }
                    }
                    CurrentSize = long.Parse(lvItem.SubItems[1].Text);
                    _lTotal = 0;
                    string SourceName = System.IO.Path.Combine(LocalFolder, CurrentItem);
                    nfsClient.Write(nfsClient.Combine(CurrentItem, RemoteFolder), SourceName);
                    if (movebol)
                        System.IO.File.Delete(SourceName);

                }
                ShowProgress(false,movebol);
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "NFS Client - Error");
                ShowProgress(false,movebol);
            }
        }


        void UploadRecursive(bool move,bool rewriteAll,bool keepAll,string localFolder,string remoteFolder)
        {
            if (!(nfsClient.FileExists(remoteFolder) && nfsClient.IsDirectory(remoteFolder)))
            {
                //with full privilegies (it would not be nice to show form for each directory ....)
                nfsClient.CreateDirectory(remoteFolder);
            }


            System.IO.DirectoryInfo CurrentDirecotry = new System.IO.DirectoryInfo(localFolder);
            int totalFiles = CurrentDirecotry.GetDirectories().Length+CurrentDirecotry.GetFiles().Length;
            int currentFile = 0;
            foreach (System.IO.DirectoryInfo dir in CurrentDirecotry.GetDirectories())
            {
                currentFile++;
                CurrentFilePos = currentFile + "/" + totalFiles;
                CurrentItem = dir.Name;
                CurrentDir = dir.Name;
                UploadRecursive(move, rewriteAll, keepAll, System.IO.Path.Combine(localFolder, dir.Name), nfsClient.Combine(dir.Name, remoteFolder));
                if (move)
                    Directory.Delete(System.IO.Path.Combine(localFolder, dir.Name));
            }
            foreach (System.IO.FileInfo file in CurrentDirecotry.GetFiles())
            {
                currentFile++;
                CurrentFilePos = currentFile + "/" + totalFiles;
                CurrentItem = file.Name;
                CurrentDir = remoteFolder;
                if (nfsClient.FileExists(nfsClient.Combine(file.Name, remoteFolder)))
                    {
                        nfsClient.DeleteFile(nfsClient.Combine(file.Name, remoteFolder));
                    }



                   CurrentSize = file.Length;
                    _lTotal = 0;
                    nfsClient.Write(nfsClient.Combine(file.Name, remoteFolder), System.IO.Path.Combine(localFolder, file.Name));
                    if (move)
                        System.IO.File.Delete(System.IO.Path.Combine(localFolder, file.Name));
            }
        }

        void nfsClient_DataEvent(object sender, NFSLibrary.NFSClient.NFSEventArgs e)
        {
            TotalTransferredBytes += e.Bytes;
            UpdateProgress(CurrentItem, CurrentSize, e.Bytes);
        }

        private void listViewLocal_DragOver(object sender, DragEventArgs e)
        {
            if (!connected)
                return;
            e.Effect = CurrentEffect;
        }

        private void listViewRemote_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (!connected)
                return;
            CurrentList = "Remote";
            Bitmap bmp = (Bitmap)listViewRemote.SmallImageList.Images[(int)0];
            CurrentEffect = DragDropEffects.Copy;
            ListView.SelectedListViewItemCollection sl = listViewRemote.SelectedItems;
            lvDragItem.Clear();
            foreach (ListViewItem lvi in sl)
            {
                lvDragItem.Add(lvi);
            }
            this.DoDragDrop(bmp, CurrentEffect);
        }

        private void selectAll_remote()
        {
            if (!connected)
                return;
            int i = 0;
            foreach (ListViewItem item in listViewRemote.Items)
            {
                if (item.Text != "..")
                {
                    item.Selected = true;
                    i++;
                }
            }
            addToStatusLog("Selected all remote("+i+")");
        }

        private void listViewRemote_DragDrop(object sender, DragEventArgs e)
        {
            if (!connected)
                return;
            if (CurrentList != "Remote")
            {
                addToStatusLog("Copying " + lvDragItem.Count + " files to remote.");
                if (CurrentEffect == DragDropEffects.Copy)
                {
                    uploadThread = new Thread(new ParameterizedThreadStart(Upload));
                    uploadThread.Start(false);
                }
            }
        }

        private void listViewRemote_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = CurrentEffect;
        }

        private void listViewLocal_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = CurrentEffect;
        }

        private void listViewRemote_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = CurrentEffect;
        }

        private void listViewRemote_KeyDown(object sender, KeyEventArgs e)
        {
            if (!connected)
                return;
            try
            {
                if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.F8)
                {
                    DeleteRemoteFiles();
                }
                else if (e.KeyCode == Keys.F7)
                {
                    makeNewFolderRemote();
                }
                else if (e.KeyCode == Keys.F6)
                    copyRemote(true);
                else if (e.KeyCode == Keys.F5)
                    copyRemote(false);
                else if (e.KeyCode == Keys.F4)
                    viewEditFileRemote();
                else if (e.KeyCode == Keys.F3)
                    viewEditFileRemote();
                else if (e.KeyCode == Keys.Enter)
                {
                    if (listViewRemote.SelectedItems != null)
                    {
                        ListViewItem lvi = listViewRemote.SelectedItems[0];
                        if (lvi.ImageIndex == 1)
                        {
                            if (lvi.Text == "..")
                                RemoteFolder = nfsClient.GetDirectoryName(RemoteFolder);
                            else
                                RemoteFolder = nfsClient.Combine(lvi.Text, RemoteFolder);
                            tryToRefreshRemote();
                        }
                        //TODO copy file and run it !
                    }
                }
                if (e.KeyCode == Keys.A && e.Control)
                {
                    selectAll_remote();
                }
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "NFS Client - Error");
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            dissconect(true);
        }

        public void btnCancel_Click(object sender, EventArgs e)
        {
            if (uploadThread != null && uploadThread.IsAlive)
                uploadThread.Abort();
            if (downloadThread != null && downloadThread.IsAlive)
                downloadThread.Abort();
            ShowProgress(false,true);
        }

        private void listViewLocal_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.F8)
                {
                    DeleteLocalFiles();
                }
                else if (e.KeyCode == Keys.F7)
                {
                    makeNewFolderLocal();
                }
                else if (e.KeyCode == Keys.F6)
                {
                    copyLocal(true);
                }
                else if (e.KeyCode == Keys.F5)
                {
                    copyLocal(false);
                }
                else if (e.KeyCode == Keys.F4)
                {
                    viewEditFileLocal();
                }
                else if (e.KeyCode == Keys.F3)
                {
                    viewEditFileLocal();
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    try
                    {
                        if (listViewLocal.SelectedItems != null)
                        {
                            ListViewItem lvi = listViewLocal.SelectedItems[0];
                            if (lvi.ImageIndex == 1)
                            {
                                if (lvi.Text == "..")
                                    RefreshLocal(System.IO.Path.GetDirectoryName(this.tbLocalPath.Text));
                                else
                                    RefreshLocal(System.IO.Path.Combine(this.tbLocalPath.Text, lvi.Text));
                            }
                            else
                                System.Diagnostics.Process.Start(System.IO.Path.Combine(this.tbLocalPath.Text, lvi.Text));

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "NFS Client - Error");
                    }
                }

                if (e.KeyCode == Keys.A && e.Control)
                {
                    selectAll_local();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "NFS Client - Error");
            }
        }

        private void selectAll_local()
        {
            int i = 0;
            foreach (ListViewItem item in listViewLocal.Items)
            {
                if (item.Text!= "..")
                {
                    item.Selected = true;
                    i++;
                }
            }
            addToStatusLog("Selected all local("+i+")");
        }

        private void btnNewFolder_Click(object sender, EventArgs e)
        {
            makeNewFolderRemote();
        }

        private void listViewRemote_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (!connected)
                return;
            try
            {
                string NewLabel = e.Label;
                ListViewItem lvi = listViewRemote.Items[e.Item];
                nfsClient.Move(
                    nfsClient.Combine(lvi.Text, RemoteFolder),
                    nfsClient.Combine(NewLabel, RemoteFolder)
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "NFS Client - Error");
            }
        }

        private void listViewRemote_DoubleClick(object sender, EventArgs e)
        {
            if (!connected)
                return;
            try
            {
                if (listViewRemote.SelectedItems != null)
                {
                    ListViewItem lvi = listViewRemote.SelectedItems[0];
                    if (lvi.ImageIndex == 1)
                    {
                        if (lvi.Text == "..")
                            RemoteFolder = nfsClient.GetDirectoryName(RemoteFolder);
                        else
                            RemoteFolder = nfsClient.Combine(lvi.Text, RemoteFolder);
                        tryToRefreshRemote();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "NFS Client - Error");
            }
        }

        private void listViewLocal_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            try
            {
                string NewLabel = e.Label;
                ListViewItem lvi = listViewLocal.Items[e.Item];
                string Folder = tbLocalPath.Text;
                System.IO.File.Move(System.IO.Path.Combine(Folder, lvi.Text), System.IO.Path.Combine(Folder, NewLabel));
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occurred renaming the item (" + ex.ToString() + ")", "NFS Client", MessageBoxButtons.OK);
                e.CancelEdit = true;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            AvgDwnSpeed = TotalTransferredBytes / ++TotalTransferredSpeedChecks;
        }



        public static bool GetResolvedConnecionIPAddress(string serverNameOrURL, out IPAddress resolvedIPAddress)
        {
            bool isResolved = false;
            IPHostEntry hostEntry = null;
            IPAddress resolvIP = null;
            try
            {
                if (!IPAddress.TryParse(serverNameOrURL, out resolvIP))
                {
                    hostEntry = Dns.GetHostEntry(serverNameOrURL);

                    if (hostEntry != null && hostEntry.AddressList != null && hostEntry.AddressList.Length > 0)
                    {
                        if (hostEntry.AddressList.Length == 1)
                        {
                            resolvIP = hostEntry.AddressList[0];
                            isResolved = true;
                        }
                        else
                        {
                            foreach (IPAddress var in hostEntry.AddressList)
                            {
                                if (var.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    resolvIP = var;
                                    isResolved = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    isResolved = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "NFS Client - Error");
            }
            finally
            {
                resolvedIPAddress = resolvIP;
            }

            return isResolved;
        }



        #endregion

        private void newFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectAll_remote();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectAll_local();
        }

        private void listViewLocal_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (listViewLocal.SelectedItems != null)
                {
                    ListViewItem lvi = listViewLocal.SelectedItems[0];
                    if (lvi.ImageIndex == 1)
                    {
                        if (lvi.Text == "..")
                        {
                            RefreshLocal(System.IO.Path.GetDirectoryName(this.tbLocalPath.Text));
                        }
                        else
                        {
                            RefreshLocal(System.IO.Path.Combine(this.tbLocalPath.Text, lvi.Text));
                        }
                    }
                    else
                        System.Diagnostics.Process.Start(System.IO.Path.Combine(this.tbLocalPath.Text, lvi.Text));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "NFS Client - Error");
            }

           
        }

        public void addToStatusLog(string text)
        {
            //clear up
            if (StatusLog.Items.Count > 2000)
            {
                StatusLog.Items.Clear();
                StatusLog.Items.Add(DateTime.Now.ToString("H:mm:ss tt") + " : Log cleared");
            }
            StatusLog.Items.Add(DateTime.Now.ToString("H:mm:ss tt")+" : "+text);
            this.StatusLog.TopIndex = this.StatusLog.Items.Count - 1;
        }




        private void dissconect(bool client_close)
        {
            if (nfsClient != null)
            {
                if (!connected)
                    return;

                if (mountedDrive)
                    UnmountDrive();
                nfsClient.UnMountDevice();
                nfsClient.Disconnect();
            }
            if (!client_close)
            {
                this.Text = "NFS Client - Disconnected";
                listViewRemote.Items.Clear();
                cboxRemoteDevices.Items.Clear();
                cboxRemoteDevices.Visible = false;
                label2.Visible = false;
                RemoteFolder = ".";
                CurrentRemoteFolder = ".";
                button_connect.Text = "Connect";
                connected = false;
            }
            addToStatusLog("Dissconected!");
        }

        private void altf4Button(object sender, EventArgs e)
        {
            this.Close();
        }


        private void makeNewFolderRemote()
        {
            try{
               if (nfsClient == null)
                  return;

                  NewFolder nf = new NewFolder(false);
                if (nf.ShowDialog() == DialogResult.OK)
                  {
                      if (nf.NewFolderName.Length > 1)
                      {
                          nfsClient.CreateDirectory(
                              nfsClient.Combine(nf.NewFolderName, RemoteFolder),
                              new NFSLibrary.Protocols.Commons.NFSPermission(nf.userPermisions,nf.groupPermisions,nf.otherPermisions));
                          addToStatusLog("Created new folder on remote");
                          tryToRefreshRemote();
                      }
                  }
              }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message, "NFS Client - Error");
              }
        }

        private void makeNewFolderLocal()
        {
            try
            {

                NewFolder nf = new NewFolder(true);
                if (nf.ShowDialog() == DialogResult.OK)
                {
                    if (nf.NewFolderName.Length > 1)
                    {

                       string current_path = tbLocalPath.Text;
                        string path = current_path+"\\" + nf.NewFolderName; 

                       if (!Directory.Exists(path))
                       {
                           Directory.CreateDirectory(path);
                           addToStatusLog("Created new folder on local");
                           RefreshLocal(current_path);
                       }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "NFS Client - Error");
            }
        }

        private void newFolderToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            makeNewFolderLocal();
        }

        private void newfolderf7_Click(object sender, EventArgs e)
        {
            if (listViewLocal.SelectedItems.Count > 0)
                makeNewFolderLocal();
            else if (listViewRemote.SelectedItems.Count > 0)
                makeNewFolderRemote();
        }

        private void f8DeleteButton_Click(object sender, EventArgs e)
        {
            if (listViewLocal.SelectedItems.Count > 0)
                DeleteLocalFiles();
            else if (listViewRemote.SelectedItems.Count > 0)
                DeleteRemoteFiles();
        }

        private void DeleteRemoteFiles()
        {
            int i = 0;
            if (listViewRemote.SelectedItems != null)
            {
                bool deleteAll = false;
                
                foreach (ListViewItem lvi in listViewRemote.SelectedItems)
                {
                    DialogResult dl = DialogResult.None;
                    if (!deleteAll)
                    {
                        Rewrite del = new Rewrite("NFS Client", "Do you really want to delete " + lvi.Text + " ?");
                        dl = del.ShowDialog();
                        if (dl == DialogResult.Retry)
                            deleteAll = true;
                        else if (dl == DialogResult.Ignore)
                            break;
                    }

                    if (deleteAll || dl == DialogResult.OK)
                    {
                        if (lvi.ImageIndex == 0)
                        {
                            nfsClient.DeleteFile(nfsClient.Combine(lvi.Text, RemoteFolder));
                        }
                        else
                            nfsClient.DeleteDirectory(nfsClient.Combine(lvi.Text, RemoteFolder),true);

                        i++;
                    }
                }
                addToStatusLog("Deleted " + i + " files and folders from " + RemoteFolder);
                tryToRefreshRemote();
            }
        }


        private void DeleteLocalFiles()
        {
            int i = 0;
            if (listViewLocal.SelectedItems != null)
            {
                bool deleteAll = false;
                foreach (ListViewItem lvi in listViewLocal.SelectedItems)
                {
                    DialogResult dl = DialogResult.None;
                    if (!deleteAll)
                    {
                        Rewrite del = new Rewrite("NFS Client", "Do you really want to delete " + lvi.Text + " ?");
                        dl = del.ShowDialog();
                        if (dl == DialogResult.Retry)
                            deleteAll = true;
                        else if (dl == DialogResult.Ignore)
                            break;
                    }

                    if (deleteAll || dl == DialogResult.OK)
                    {
                        try
                        {
                            if (lvi.ImageIndex == 1)
                                Directory.Delete(System.IO.Path.Combine(this.tbLocalPath.Text, lvi.Text),true);
                            else
                                System.IO.File.Delete(System.IO.Path.Combine(this.tbLocalPath.Text, lvi.Text));
                            i++;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "NFS Client - Error");
                        }
                    }
                }
                addToStatusLog("Deleted " + i + " files and folders from " + this.tbLocalPath.Text);
                RefreshLocal(tbLocalPath.Text);
            }
        }

        private void f5CopyButton_Click(object sender, EventArgs e)
        {
            if (listViewLocal.SelectedItems.Count > 0)
            //copy local
            {
                copyLocal(false);
            }
            else if (listViewRemote.SelectedItems.Count > 0)
            //copy remote
            {
                copyRemote(false);
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tryToRefreshRemote();
        }

        private void refreshToolStripMenuItem1_Click(object sender, EventArgs e)
        {
           RefreshLocal(tbLocalPath.Text);
        }


        private void copyLocal(bool move)
        {
            if (listViewLocal.SelectedItems.Count > 0)
            //copy local
            {
                if (move)
                    addToStatusLog("Moving " + listViewLocal.SelectedItems.Count + " files to remote.");
                else
                    addToStatusLog("Copying " + listViewLocal.SelectedItems.Count + " files to remote.");
            CurrentList = "Local";
            ListView.SelectedListViewItemCollection sl = listViewLocal.SelectedItems;
            lvDragItem.Clear();
            foreach (ListViewItem lvi in sl)
            {
                lvDragItem.Add(lvi);
            }
            LocalFolder = tbLocalPath.Text;
            uploadThread = new Thread(new ParameterizedThreadStart(Upload));
            uploadThread.Start(move);
            }
        }


        private void copyRemote(bool move)
        {
            if (listViewRemote.SelectedItems.Count > 0)
            //copy remote
            {
                if(move)
                addToStatusLog("Moving " + listViewRemote.SelectedItems.Count + " files to local.");
                else
                addToStatusLog("Copying " + listViewRemote.SelectedItems.Count+" files to local.");
                CurrentList = "Remote";
                ListView.SelectedListViewItemCollection sl = listViewRemote.SelectedItems;
                lvDragItem.Clear();
                foreach (ListViewItem lvi in sl)
                {
                    lvDragItem.Add(lvi);
                }
                LocalFolder = tbLocalPath.Text;
                downloadThread = new Thread(new ParameterizedThreadStart(Download));
                downloadThread.Start(move);
            }
        }

        private void F3ViewButton_Click(object sender, EventArgs e)
        {
            if (listViewLocal.SelectedItems.Count > 0)
            {
                viewEditFileLocal();
            }
            else if (listViewRemote.SelectedItems.Count > 0)
            {
                viewEditFileRemote();
            }
        }

        private void f6MoveButton_Click(object sender, EventArgs e)
        {
            if (listViewLocal.SelectedItems.Count > 0)
            //move local
            {
                copyLocal(true);
            }
            else if (listViewRemote.SelectedItems.Count > 0)
            //move remote
            {
                copyRemote(true);
            }
        }



        private void viewEditFileLocal()
        {
            if (listViewLocal.SelectedItems.Count > 0)
            {
                string edited_item = listViewLocal.SelectedItems[0].Text;
                string edited_item_full_path = System.IO.Path.Combine(tbLocalPath.Text, edited_item);

                System.Diagnostics.Process.Start("notepad.exe",edited_item_full_path);
            }
        }

        private void F4EditButton_Click(object sender, EventArgs e)
        {
            if (listViewLocal.SelectedItems.Count > 0)
            {
                viewEditFileLocal();
            }
            else if (listViewRemote.SelectedItems.Count > 0)
            {
                viewEditFileRemote();
            }
        }

        private void viewEditFileRemote()
        {
            if (listViewRemote.SelectedItems.Count > 0)
            {
                addToStatusLog("created Temp file");
                string tempFile = Path.GetTempFileName();
                string remote_item = listViewRemote.SelectedItems[0].Text;
                string remote_item_path = nfsClient.Combine(remote_item, RemoteFolder);
                addToStatusLog("writing to temp file.....");
                nfsClient.Read(remote_item_path, tempFile);
                addToStatusLog("Read temp file");
                System.Diagnostics.Process.Start("notepad.exe", tempFile);

            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About show = new About();
            show.ShowDialog();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options op = new Options(this);
            op.ShowDialog();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help hp = new Help();
            hp.ShowDialog();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connect();
        }

        private void addServer_Click(object sender, EventArgs e)
        {
            AddServer firstTime = new AddServer(this,-1);
            firstTime.StartPosition = FormStartPosition.Manual;
            firstTime.Location = new Point(this.Location.X + (this.Width - firstTime.Width) / 2, this.Location.Y + (this.Height - firstTime.Height) / 2);
            firstTime.Show(this);
        }

        private void serverListCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int server_index = serverListCombo.SelectedIndex;
            serverAdress = NFSClient.Properties.Settings.Default.ServerAddres[server_index];
            serverGid = NFSClient.Properties.Settings.Default.gid[server_index];
            serverUid = NFSClient.Properties.Settings.Default.uid[server_index];
            serverSecurePort = NFSClient.Properties.Settings.Default.SecurePort[server_index];
            serverVersion = NFSClient.Properties.Settings.Default.DefaultProtocol[server_index];
            NFSClient.Properties.Settings.Default.lastServerId = server_index;
            NFSClient.Properties.Settings.Default.Save();
        }

        private void editServer_Click(object sender, EventArgs e)
        {
            if (serverListCombo.Items.Count < 1)
            {
                MessageBox.Show("No saved server to edit!");
                return;
            }
            AddServer firstTime = new AddServer(this,serverListCombo.SelectedIndex);
            firstTime.StartPosition = FormStartPosition.Manual;
            firstTime.Location = new Point(this.Location.X + (this.Width - firstTime.Width) / 2, this.Location.Y + (this.Height - firstTime.Height) / 2);
            firstTime.Show(this);
        }

        private void mountDrive_Click(object sender, EventArgs e)
        {
            if (mountedDrive)
                UnmountDrive();
            else
                MountDrive();

        }

        private void UnmountDrive()
        {
            //if (rbDisk.Checked)
                DokanNet.DokanUnmount(MountPoint.ToCharArray()[0]);
            //else
             //   DokanNet.DokanRemoveMountPoint(MountPoint);

                mountedDrive = false;
        }


        private void MountDrive()
        {
            if (!connected)
            {
                MessageBox.Show("Client not connected!");
            }

            if (nfsClient == null)
                throw new ApplicationException("NFS object is null!");


            MountPoint = String.Format(@"{0}:\", "Z");


            /*
            if (!DiskOrFolder)
            {
                if (!Directory.Exists(Folder))
                    throw new ApplicationException(String.Format("{0} not found.", Folder));
                else
                    MountPoint = Folder;
            } */

            bool NoCache = true;

            ThreadPool.QueueUserWorkItem(new WaitCallback(
                delegate
                {
                    try
                    {
                        System.IO.Directory.SetCurrentDirectory(Application.StartupPath);
                        DokanOptions dokanOptions = new DokanOptions();
                        dokanOptions.DebugMode = false;
                        dokanOptions.NetworkDrive = false;
                        dokanOptions.RemovableDrive = true;
                        dokanOptions.MountPoint = MountPoint;
                        dokanOptions.UseKeepAlive = true;
                        dokanOptions.UseAltStream = true;
                        dokanOptions.VolumeLabel = this.Text;
                        dokanOptions.ThreadCount = 1;

                        if (NoCache)
                            dokanOperation = new Operations(nfsClient,this);
                        else
                            dokanOperation = new CacheOperations(new Operations(nfsClient,this));
                        int status = DokanNet.DokanMain(dokanOptions, dokanOperation);
                        switch (status)
                        {
                            case DokanNet.DOKAN_DRIVE_LETTER_ERROR:
                                throw new ApplicationException("Drvie letter error");
                            case DokanNet.DOKAN_DRIVER_INSTALL_ERROR:
                                throw new ApplicationException("Driver install error");
                            case DokanNet.DOKAN_MOUNT_ERROR:
                                throw new ApplicationException("Mount error");
                            case DokanNet.DOKAN_START_ERROR:
                                throw new ApplicationException("Start error");
                            case DokanNet.DOKAN_ERROR:
                                throw new ApplicationException("Unknown error");
                            case DokanNet.DOKAN_SUCCESS:
                                break;
                            default:
                                throw new ApplicationException("Unknown status: " + status);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }));

            ThreadPool.QueueUserWorkItem(new WaitCallback(
                delegate
                {
                    Thread.Sleep(2000);
                    Process.Start("explorer.exe", " " + MountPoint);
                }));

            mountedDrive = true;
        }

    }

}
