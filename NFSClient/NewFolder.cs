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
    public partial class NewFolder : Form
    {
        public string NewFolderName;
        public byte userPermisions;
        public byte groupPermisions;
        public byte otherPermisions;

        public NewFolder(bool local)
        {
            InitializeComponent();
            //set the default to 744
            if (!local)
            {
                UsercheckedListBox.SetItemChecked(0, true);
                UsercheckedListBox.SetItemChecked(1, true);
                UsercheckedListBox.SetItemChecked(2, true);
                GroupcheckedListBox.SetItemChecked(0, true);
                OthercheckedListBox.SetItemChecked(0, true);
            }
            else
            {
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                UsercheckedListBox.Visible = false;
                GroupcheckedListBox.Visible = false;
                OthercheckedListBox.Visible = false;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            NewFolderName = folderName.Text;
            //check user
            userPermisions = 0;
            if (UsercheckedListBox.GetItemChecked(0))
                userPermisions = 4;
            if (UsercheckedListBox.GetItemChecked(1))
                userPermisions += 2;
            if (UsercheckedListBox.GetItemChecked(2))
                userPermisions += 1;


            //check user
            groupPermisions = 0;
            if (GroupcheckedListBox.GetItemChecked(0))
                groupPermisions = 4;
            if (GroupcheckedListBox.GetItemChecked(1))
                groupPermisions += 2;
            if (GroupcheckedListBox.GetItemChecked(2))
                groupPermisions += 1;


            //check user
            otherPermisions = 0;
            if (OthercheckedListBox.GetItemChecked(0))
                otherPermisions = 4;
            if (OthercheckedListBox.GetItemChecked(1))
                otherPermisions += 2;
            if (OthercheckedListBox.GetItemChecked(2))
                otherPermisions += 1;

        }
    }
}
