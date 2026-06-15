using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Thermo.Kronos.Instrument.Camera.Interface;
namespace KronosCameraTestApp.View
{
    public partial class DownloadUpdatedFirmware : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
          System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string folderName = "C:\\\\";
        // Try using defalut username/password to obtain file listing
        private string username = "root";
        private string password = "cidtec";
        private string host = "192.168.1.2";
        private CIDInterface cidInterface;
        public bool canceled = false;
        public DownloadUpdatedFirmware(string folder, string hostAddress, int port, CIDInterface cidInterface)
        {
            try
            {
                InitializeComponent();
                this.cidInterface = cidInterface;
                if (folder.Length > 2)
                    folderName = folder;
                availableListBox.ClearSelected();
                currentVersionListBox.ClearSelected();
                // Load all the *firmware* files into the available listbox       
                string fileType = "*.exe";
                DirectoryInfo dinfo = new DirectoryInfo(folderName);
                FileInfo[] Files = dinfo.GetFiles(fileType);
                List<string> fwFiles = new List<string>();
                foreach (FileInfo file in Files)
                {
                    fwFiles.Add(file.Name);
                }
                fwFiles.Sort();
                fwFiles.Reverse();
                foreach (string item in fwFiles)
                {
                    availableListBox.Items.Add(item);
                }
                if (username.Length <= 0)
                {
                    MessageBox.Show(
                    "Please enter servers Username:",
                    "Invalid Credentials",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                    return;
                }
                if (password.Length <= 0)
                {
                    MessageBox.Show(
                    "Please enter servers Password:",
                    "Invalid Credentials",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                    return;
                }
                if (hostAddress == null)
                    // Assign the default
                    host = "192.168.1.2";
                else
                    host = hostAddress;
                using (var sftpClient = new SftpClient(hostAddress, 22, username, password))
                {
                    sftpClient.Connect();
                    sftpClient.ChangeDirectory("/");
                    var files = sftpClient.ListDirectory("tmp").Select(s => s.FullName);
                    foreach (var file in files)
                    {
                        // Populate the list box
                        currentVersionListBox.Items.Add(file.ToString());
                    }
                    sftpClient.Disconnect();
                }
            }
            catch(SocketException se)
            {
                MessageBox.Show(se.Message, "Socket Error" ,MessageBoxButtons.OK, MessageBoxIcon.Error);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), se);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);               
            }
        }
        private bool ftpWebRequest(string filename)
        {
            try
            {
                bool result = false;
                result = this.cidInterface.UpdateFirmware(filename, username, password, host);
                if (result)
                    MessageBox.Show(" File " + availableListBox.SelectedItem.ToString() + " Firmware Update complete", "Update Completed");
                else
                    MessageBox.Show(" File " + availableListBox.SelectedItem.ToString() + " Firware Update failed", "Update Failed");
                return result;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
        private void loginButton_Click(object sender, EventArgs e)
        {
            try
            {
                username = usernameTextBox.Text;
                password = passwordTextBox.Text;
                if (username.Length <= 0)
                {
                    MessageBox.Show(
                    "Please enter servers Username:",
                    "Invalid Credentials",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                    log.Error("Server Usersname was empty - Download Firmware");
                    return;
                }
                if (password.Length <= 0)
                {
                    MessageBox.Show(
                    "Please enter servers Password:",
                    "Invalid Credentials",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                    log.Error("Server Password was empty - Download Firmware");
                    return;
                }
                // Use Renci.SshNet   
                ConnectionInfo connectionInfo = new ConnectionInfo(host, 22, username,
                    // Password based Authentication
                    new AuthenticationMethod[] { new PasswordAuthenticationMethod(username, password), });
                // Upload A File using a sFTP Client
                using (var sftpClient = new SftpClient(connectionInfo))
                {
                    sftpClient.Connect();
                    sftpClient.ChangeDirectory("/");
                    var files = sftpClient.ListDirectory("root").Select(s => s.FullName);
                    foreach (var file in files)
                    {
                        // Populate the list box
                        currentVersionListBox.Items.Add(file.ToString());
                    }
                    sftpClient.Disconnect();
                }
            }
            catch(Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void downloadFirmwareButton_Click(object sender, EventArgs e)
        {
            try
            {
                int availableListBoxindex = 0;
                // Make sure a row from each listbox has been selected
                if (availableListBoxindex >= 0)
                    availableListBox.SetSelected(availableListBox.SelectedIndex, true);
                string firmwareFilename = ConfigurationManager.AppSettings["UpdateFirmwareFilePath"].ToString()+availableListBox.SelectedItem.ToString();
                // Display a warning dialog to operator
                MessageBox.Show(
                    "Selected Firmware will override current version if it exist!",
                    "Current Firmware may be overwritten",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                // FTP file from available to override the current version
                bool result = ftpWebRequest(firmwareFilename);
                // Send a warmstart to reboot with the new version of firmware on camera
                //if (result == true)
                //{
                //    MessageBox.Show(
                //    "New Firmware will be loaded after reboot",
                //    "Rebooting Firmware",                   
                //    MessageBoxButtons.OK,
                //    MessageBoxIcon.Warning);
                //    this.cidInterface.RebootFirmware();
                //}
                //else
                //{
                //    MessageBox.Show(
                //   "Unknown error updating firmware",
                //   "Update Firmware",                   
                //   MessageBoxButtons.OK,
                //   MessageBoxIcon.Error);
                //    log.Error("Firmware not updated/ no updated file found");
                //}
                if (firmwareFileTypeRadioButton.Checked)
                {
                   bool tt= cidInterface.SendUpadateFirmware(firmwareFilename);
                }
                else if (fpgaFileTypeRadioButton.Checked)
                {
                    cidInterface.SendUpadateFPGA();
                }
                else if (linuxFileTypeRadioButton.Checked)
                {
                    cidInterface.SendUpadateLinux();
                }
                canceled = false;
                // Close this dialog now
            //    this.Close();
            }
            catch(Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void cancelDownloadButton_Click(object sender, EventArgs e)
        {
            canceled = true;
            this.Close();
        }
        private void passwordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            loginButton.Enabled = true;
        }
        private void restartFirmware_Click(object sender, EventArgs e)
        {
            canceled = false;
            cidInterface.RestartFirmware();
            Thread.Sleep(4000);
            cidInterface.Connect("192.168.1.2", 47151);
        }
    }
}
