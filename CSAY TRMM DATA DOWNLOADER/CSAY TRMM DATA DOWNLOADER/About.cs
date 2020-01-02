using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace CSAY_TRMM_DATA_DOWNLOADER
{
    public partial class FrmAbout : Form
    {
        public FrmAbout()
        {
            InitializeComponent();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnDocumentation_Click(object sender, EventArgs e)
        {
            Process.Start("DOWNLOAD NETCDF TRMM DATA.pdf");
        }
    }
}
