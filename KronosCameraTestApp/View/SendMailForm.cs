using KronosCameraTestApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace KronosCameraTestApp.View
{
    public partial class SendMailForm : Form
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        List<string> mailAttachments = new List<string>();
        string enggUserName = string.Empty;
        public SendMailForm(string enggUserName)
        {
            InitializeComponent();
            this.enggUserName = enggUserName;
        }
        private async void SendMail_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (string.IsNullOrWhiteSpace(textBoxEmailToAddress.Text))
                {
                    MessageBox.Show("To Address cannot be empty","Send Email", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (string.IsNullOrWhiteSpace(textBoxEmailPassword.Text))
                {
                    MessageBox.Show("Please enter Email Password", "Send Email", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string emailUserID = string.Empty;
                if (ConfigurationManager.AppSettings["EndClient"] != "Thermo")
                    emailUserID = Environment.UserName + ConfigurationManager.AppSettings["EndClientEmailDomain"];//todo
                else
                    emailUserID=Environment.UserName + "@thermofisher.com";
                if (await SendOutlookMail.SendMail_SMTP(emailUserID, textBoxEmailToAddress.Text, textBoxCC.Text, textBoxEmailSubject.Text, richTextBoxMailBody.Text, textBoxEmailPassword.Text, mailAttachments))
                {
                    MessageBox.Show("Email sent successfully");
                }
                else
                {
                    MessageBox.Show("Email sent failed");
                }
                Cursor.Current = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }  
        }       
        private void OpenDocuments()
        {
            try
            {
                sendMailOpenFileDialog.InitialDirectory = Properties.Settings.Default["FilesLocation"].ToString();
                DialogResult result = sendMailOpenFileDialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    richTextBoxAttachments.Text += sendMailOpenFileDialog.FileName + ";";
                    mailAttachments.Add(sendMailOpenFileDialog.FileName);                  
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }      
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void SendMailForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
        private void ultraPictureBoxOpenFileDialog_Click(object sender, EventArgs e)
        {
            OpenDocuments();
        }
    }
}
