using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace CSAY_TRMM_DATA_DOWNLOADER
{
    public partial class FrmTRMMDataDownloader : Form
    {
        string DownloadedFolderPath;
        bool HidePassword = true;
        public FrmTRMMDataDownloader()
        {
            InitializeComponent();
        }

        private void BtnDownloadTRMM_Click(object sender, EventArgs e)
        {
            string driveLetter;
            string ProjectFolderName;

            if(TxtProjectName.Text == "")
            {
                ProjectFolderName = "TRMMData_" + DateTime.Now.ToString("yyyyMMddTHHmmss");
            }
            ProjectFolderName = TxtProjectName.Text;

            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            driveLetter = Path.GetPathRoot(Environment.CurrentDirectory);
            string CopyCmd = "copy wget.exe " + driveLetter;
            cmd.StandardInput.WriteLine("mkdir " + driveLetter + ProjectFolderName); //make project folder
            cmd.StandardInput.WriteLine(CopyCmd); //copy wget.exe from degug folder of program to root directory
            cmd.StandardInput.WriteLine("cd\\"); //root directory

            cmd.StandardInput.WriteLine("cd " + ProjectFolderName); //go to project folder
            //MessageBox.Show(driveLetter);

            cmd.StandardInput.WriteLine("mkdir Cookies Data"); //make cookies and data folder in project folder

            cmd.StandardInput.WriteLine("cd Cookies"); //go to cookies folder
            cmd.StandardInput.WriteLine("NUL > .urs_cookies"); //create cookies file in cookies folder

            string Cookies_loc = driveLetter + ProjectFolderName + "\\Cookies\\.urs_cookies";
            //MessageBox.Show(loc);

            //string downloadTrmmData = "wget --load-cookies " + Cookies_loc + " --save-cookies " + Cookies_loc + " --auth-no-challenge=on --keep-session-cookies --user=username --password=password --content-disposition -i " + textBox1.Text;
            string usrnm = TxtUsername.Text;
            string passwrd = TxtPassword.Text;
            string downloadTrmmData = "wget --load-cookies " + Cookies_loc + " --save-cookies " + Cookies_loc + " --auth-no-challenge=on --keep-session-cookies --user=" + usrnm + " --password=" + passwrd + " --content-disposition -i " + TxtDownloadLinkList.Text;

            //MessageBox.Show(downloadTrmmData);

            cmd.StandardInput.WriteLine("cd\\"); //root directory
            //cmd.StandardInput.WriteLine("cd " + ProjectFolderName); //go to project folder
            string cmd3_1 = "copy wget.exe " + driveLetter + ProjectFolderName + "\\Data";
            string DeleteCmd = "del wget.exe";
            //MessageBox.Show(cmd3_1);
            cmd.StandardInput.WriteLine(cmd3_1); //copy wget to Data Folder
            cmd.StandardInput.WriteLine(DeleteCmd); //copy wget to Data Folder
            cmd.StandardInput.WriteLine("cd " + ProjectFolderName + "\\Data"); //go to Data Folder

            Application.DoEvents();
            TxtMessage.AppendText(DateTime.Now.ToString("hh:mm:ss") + "  ==> Please Wait....Downloading TRMM Data....May take several minutes depending on data and Internet Speed");
            TxtMessage.AppendText(Environment.NewLine);
            cmd.StandardInput.WriteLine(downloadTrmmData); //download trmmData

            //driveLetter = Path.GetPathRoot(Environment.CurrentDirectory);
            //MessageBox.Show(driveLetter);

            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());

            //MessageBox.Show("Completed");
            DownloadedFolderPath = driveLetter + ProjectFolderName + "\\Data";
            TxtMessage.AppendText(DateTime.Now.ToString("hh:mm:ss") + "  ==> Download Completed!");
            TxtMessage.AppendText(Environment.NewLine);
            TxtMessage.AppendText(DateTime.Now.ToString("hh:mm:ss") + "  ==> Downloaded Folder :  " + driveLetter + ProjectFolderName + "\\Data");
            TxtMessage.AppendText(Environment.NewLine);
            TxtMessage.AppendText(DateTime.Now.ToString("hh:mm:ss") + "  ==> Click open to open the folder containing downloaded data");
            TxtMessage.AppendText(Environment.NewLine);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            string path;
            OpenFileDialog openfiledialog1 = new OpenFileDialog();
            openfiledialog1.Filter = "Text File(*.txt)|*.txt|Data File(*.dat)|*.dat|All Files(*.*)|*.*";
            openfiledialog1.FilterIndex = 1;

            if (openfiledialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = openfiledialog1.FileName;
                TxtDownloadLinkList.Text = path;
            }
            else if (openfiledialog1.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return;
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            Process.Start(DownloadedFolderPath);
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            if(HidePassword == true)
            {
                HidePassword = false;
                TxtPassword.PasswordChar = '\0';
                BtnShow.Text = "Hide";
            }
            else if(HidePassword == false)
            {
                HidePassword = true;
                TxtPassword.PasswordChar = '*';
                BtnShow.Text = "Show";
            }
        }

        private void BtnAbout_Click(object sender, EventArgs e)
        {
            FrmAbout fabout = new FrmAbout();
            fabout.Show();
        }

        private void FrmTRMMDataDownloader_Load(object sender, EventArgs e)
        {
            DownloadedFolderPath = Environment.CurrentDirectory;
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            Process.Start("https://urs.earthdata.nasa.gov/home");
        }

        private void BtnDataWebsite_Click(object sender, EventArgs e)
        {
            Process.Start("https://disc.gsfc.nasa.gov/");
        }
    }
}
