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
    public partial class Progress : Form
    {
        MainForm parent;
        public Progress(MainForm form)
        {
            InitializeComponent();
            parent = form;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            parent.btnCancel_Click(sender, e);
        }
    }
}
