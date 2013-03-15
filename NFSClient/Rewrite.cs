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
    public partial class Rewrite : Form
    {
        public Rewrite(string title,string question)
        {
            InitializeComponent();
            this.question.Text = question;
            this.Text = title;
        }

        private void buttonQall_Click(object sender, EventArgs e)
        {
            this.DialogResult =  DialogResult.Retry;
            this.Close();
        }

        private void buttonqYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonqNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonqnotoal_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Ignore;
            this.Close();
        }


        

    }
}
