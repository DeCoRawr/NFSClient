using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NFSClient
{
    public partial class AddServer : Form
    {
        MainForm parent;
        int indexToChange;
        public AddServer(MainForm parentFrom,int changeIndex)
        {
            InitializeComponent();
            parent = parentFrom;
            indexToChange = changeIndex;

            if (indexToChange > -1)
            {
                this.Text = "EditServer";
                this.justConnect.Text = "Delete";
            }
        }

        private void AddServer_Load(object sender, EventArgs e)
        {
            if (indexToChange == -1)
                nfsProto.SelectedIndex = 0;
            else
                //load the settings
            {
                newProfileName.Text = NFSClient.Properties.Settings.Default.ServerName[indexToChange];
                defServerAdr.Text = NFSClient.Properties.Settings.Default.ServerAddres[indexToChange];
                GidConnect.Text = NFSClient.Properties.Settings.Default.gid[indexToChange].ToString();
                 uidConnect.Text = NFSClient.Properties.Settings.Default.uid[indexToChange].ToString();
                 securePort.Checked = NFSClient.Properties.Settings.Default.SecurePort[indexToChange];
                if (NFSClient.Properties.Settings.Default.DefaultProtocol[indexToChange] == "v3")
                    nfsProto.SelectedIndex = 1;
                else if (NFSClient.Properties.Settings.Default.DefaultProtocol[indexToChange] == "v2")
                    nfsProto.SelectedIndex = 0;
                else
                    nfsProto.SelectedIndex = 2;
            }
        }

        private void saveServer_Click(object sender, EventArgs e)
        {

            if (newProfileName.Text.Length < 0)
            {
                MessageBox.Show("Please enter the Server name.");
                return;
            }

            if (defServerAdr.Text.Length < 7)
            {
                MessageBox.Show("Please enter the correct Server Adress.");
                return;
            }



           int current_length = NFSClient.Properties.Settings.Default.ServerName.Length;

           if (indexToChange > -1)
           {
               current_length = indexToChange;
           }
           else
           {
              //make new
               String[] oldName = NFSClient.Properties.Settings.Default.ServerName;
               NFSClient.Properties.Settings.Default.ServerName = new String[current_length + 1];
               Array.Copy(oldName, NFSClient.Properties.Settings.Default.ServerName, current_length);

               String[] oldAdress = NFSClient.Properties.Settings.Default.ServerAddres;
               NFSClient.Properties.Settings.Default.ServerAddres = new String[current_length + 1];
               Array.Copy(oldAdress, NFSClient.Properties.Settings.Default.ServerAddres, current_length);

               String[] oldVersion = NFSClient.Properties.Settings.Default.DefaultProtocol;
               NFSClient.Properties.Settings.Default.DefaultProtocol = new String[current_length + 1];
               Array.Copy(oldVersion, NFSClient.Properties.Settings.Default.DefaultProtocol, current_length);

               bool[] oldSecure = NFSClient.Properties.Settings.Default.SecurePort;
               NFSClient.Properties.Settings.Default.SecurePort = new bool[current_length + 1];
               Array.Copy(oldSecure, NFSClient.Properties.Settings.Default.SecurePort, current_length);

               int[] oldUid = NFSClient.Properties.Settings.Default.uid;
               NFSClient.Properties.Settings.Default.uid = new int[current_length + 1];
               Array.Copy(oldUid, NFSClient.Properties.Settings.Default.uid, current_length);

               int[] oldGid = NFSClient.Properties.Settings.Default.gid;
               NFSClient.Properties.Settings.Default.gid = new int[current_length + 1];
               Array.Copy(oldGid, NFSClient.Properties.Settings.Default.gid, current_length);
           }


            int gid = 0;
            int.TryParse(GidConnect.Text, out gid);
            NFSClient.Properties.Settings.Default.gid[current_length] = gid;


            int uid = 0;
            int.TryParse(uidConnect.Text, out uid);
            NFSClient.Properties.Settings.Default.uid[current_length] = uid;


            NFSClient.Properties.Settings.Default.ServerName[current_length] = newProfileName.Text;

            NFSClient.Properties.Settings.Default.ServerAddres[current_length] = defServerAdr.Text;

            NFSClient.Properties.Settings.Default.DefaultProtocol[current_length] = nfsProto.SelectedItem.ToString();

            NFSClient.Properties.Settings.Default.SecurePort[current_length] = securePort.Checked;



            NFSClient.Properties.Settings.Default.Save();
            if (indexToChange == -1)
                parent.serverListCombo.Items.Add(newProfileName.Text + "  (" + defServerAdr.Text + "@" + nfsProto.SelectedItem.ToString() + ")");
            else
            {
                parent.serverListCombo.Items.Clear();
                int i = 0;
                foreach (string item in NFSClient.Properties.Settings.Default.ServerName)
                {
                    parent.serverListCombo.Items.Add(item + "  (" + NFSClient.Properties.Settings.Default.ServerAddres[i] + "@" + NFSClient.Properties.Settings.Default.DefaultProtocol[i] + ")");
                    i++;
                }
            }
            parent.serverListCombo.SelectedIndex = current_length;


            this.Close();

        }

        private void justConnect_Click(object sender, EventArgs e)
        {
            if (indexToChange == -1)
            {
                if (defServerAdr.Text.Length < 7)
                {
                    MessageBox.Show("Please enter the correct Server Adress.");
                    return;
                }


                parent.serverAdress = defServerAdr.Text;
                parent.serverVersion = nfsProto.SelectedItem.ToString();
                parent.serverSecurePort = securePort.Checked;
                int gid = 0;
                int.TryParse(GidConnect.Text, out gid);
                parent.serverGid = gid;
                int uid = 0;
                int.TryParse(uidConnect.Text, out uid);
                parent.serverUid = uid;

                if (parent.connected)
                    //aka dissconect
                    parent.connect();
                parent.connect();
                this.Close();
            }
            else
            {

                //delete it at index indexToChange
                int current_length = NFSClient.Properties.Settings.Default.ServerName.Length;
                String[] oldName = NFSClient.Properties.Settings.Default.ServerName;
                String[] oldAdress = NFSClient.Properties.Settings.Default.ServerAddres;
                String[] oldVersion = NFSClient.Properties.Settings.Default.DefaultProtocol;
                bool[] oldSecure = NFSClient.Properties.Settings.Default.SecurePort;
                int[] oldUid = NFSClient.Properties.Settings.Default.uid;
                int[] oldGid = NFSClient.Properties.Settings.Default.gid;


                NFSClient.Properties.Settings.Default.ServerName = new String[current_length-1];
                NFSClient.Properties.Settings.Default.ServerAddres = new String[current_length - 1]; ;
                NFSClient.Properties.Settings.Default.DefaultProtocol = new String[current_length - 1];
                NFSClient.Properties.Settings.Default.SecurePort = new bool[current_length - 1];
                NFSClient.Properties.Settings.Default.uid = new int[current_length - 1];
                NFSClient.Properties.Settings.Default.gid = new int[current_length - 1];



                if (current_length > 1)
                {
                    int j = 0;
                    for (int i = 0; i < current_length; i++)
                    {
                        if (i != indexToChange)
                        {
                            NFSClient.Properties.Settings.Default.ServerName[j] = oldName[i];
                            j++;
                        }
                    }

                    j = 0;
                    for (int i = 0; i < current_length; i++)
                    {
                        if (i != indexToChange)
                        {
                            NFSClient.Properties.Settings.Default.ServerAddres[j] = oldAdress[i];
                            j++;
                        }
                    }

                    j = 0;
                    for (int i = 0; i < current_length; i++)
                    {
                        if (i != indexToChange)
                        {
                            NFSClient.Properties.Settings.Default.DefaultProtocol[j] = oldVersion[i];
                            j++;
                        }
                    }


                    j = 0;
                    for (int i = 0; i < current_length; i++)
                    {
                        if (i != indexToChange)
                        {
                            NFSClient.Properties.Settings.Default.SecurePort[j] = oldSecure[i];
                            j++;
                        }
                    }


                    j = 0;
                    for (int i = 0; i < current_length; i++)
                    {
                        if (i != indexToChange)
                        {
                            NFSClient.Properties.Settings.Default.uid[j] = oldUid[i];
                            j++;
                        }
                    }

                    j = 0;
                    for (int i = 0; i < current_length; i++)
                    {
                        if (i != indexToChange)
                        {
                            NFSClient.Properties.Settings.Default.gid[j] = oldGid[i];
                            j++;
                        }
                    }

                    parent.serverListCombo.Items.RemoveAt(indexToChange);
                    parent.serverListCombo.SelectedIndex = 0;
                }
                else
                    parent.serverListCombo.Items.Clear();

                NFSClient.Properties.Settings.Default.Save();
                this.Close();
            }
        }


    }
}
