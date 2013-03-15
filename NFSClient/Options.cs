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
    public partial class Options : Form
    {
        MainForm parrent = null;

        public Options(MainForm par)
        {
            InitializeComponent();
            parrent = par;
        }


        private void Options_Load(object sender, EventArgs e)
        {

            //this should get loaded seperatly
            autoConnect.Checked = NFSClient.Properties.Settings.Default.autoConnect;
            conTimeout.Text = NFSClient.Properties.Settings.Default.ConnectionTimeout.ToString();
            startLocalFolder.Text = NFSClient.Properties.Settings.Default.DefaultLocalFolder;
            FHCache.Checked = NFSClient.Properties.Settings.Default.UseFhCache;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (DialogResult.OK == fbd.ShowDialog())
                {
                    startLocalFolder.Text = fbd.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "NFS Client");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            save_current();

            this.Close();
        }


        private void save_current()
        {
            NFSClient.Properties.Settings.Default.DefaultLocalFolder= startLocalFolder.Text;
            int time = 0;
            int.TryParse(conTimeout.Text, out time);
            NFSClient.Properties.Settings.Default.ConnectionTimeout = time;
            NFSClient.Properties.Settings.Default.UseFhCache= FHCache.Checked;
            NFSClient.Properties.Settings.Default.autoConnect = autoConnect.Checked;
            NFSClient.Properties.Settings.Default.Save();
        }
    }
}
