using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NOTAM_Browser
{
    public partial class frmLevelBlockInput : Form
    {

        public string UpperLimit
        {
            get { return txtUpperLimit.Text.Trim(); }
        }

        public string LowerLimit
        {
            get { return txtLowerLimit.Text.Trim(); }
        }

        public string Title
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        public frmLevelBlockInput()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
