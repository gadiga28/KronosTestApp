using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Attachment = Microsoft.TeamFoundation.WorkItemTracking.Client.Attachment;
using AttachmentCollection = Microsoft.TeamFoundation.WorkItemTracking.Client.AttachmentCollection;

namespace KronosCameraTestApp.View
{
    public partial class CreateTFSBugForm : Form
    {
        WorkItem newTFSWorkItem { get; set; }
        WorkItemType workItemType { get; set; }
        AttachmentCollection attachmentCollection { get; set; }
        Project teamProject { get; set; }
        string selectedWorkItemType = string.Empty;
        string[] workItemAttachments;
        string testappVersion = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString("yyMMdd");
        private string workStation = Environment.MachineName;
        public CreateTFSBugForm()
        {
            InitializeComponent();
            GetTFSProject();
        }
        private void GetTFSProject()
        {
            try
            {
                Uri collectionUri = new Uri("http://uspgh-pdmapp-p1:8080/TFS/");
                TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(collectionUri);
                tpc.Authenticate();
                // WorkItemStore workItemStore = tpc.GetService<WorkItemStore>();
                //string project = ConfigurationManager.AppSettings["ProjectType"].ToString();
                //teamProject = workItemStore.Projects["SCM5821AX1"];
                var workItemStore = new WorkItemStore(tpc);
                var project = (from Project pr in workItemStore.Projects
                    where pr.Name == "SCM5821AX1"
                    select pr).FirstOrDefault();
                workItemType = project.WorkItemTypes["Bug"];
                selectedWorkItemType = "Bug";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (ex.InnerException != null)
                    MessageBox.Show(ex.InnerException.ToString());
            }
        }
        public async Task<int> SaveTFSBug()
        {
            try
            {
                var res = await Task.Run(() =>
                {
                    newTFSWorkItem = new WorkItem(workItemType);
                    attachmentCollection = newTFSWorkItem.Attachments;
                    newTFSWorkItem.Title = Title.Text;

                    BeginInvoke(new Action(() => newTFSWorkItem.Fields["Integration Build"].Value = testappVersion));
                    BeginInvoke(new Action(() => newTFSWorkItem.Fields["System Info"].Value = workStation));
                    BeginInvoke(new Action(() => newTFSWorkItem.Fields["Priority"].Value = Priority.SelectedItem));
                    BeginInvoke(new Action(() => newTFSWorkItem.Fields["Assigned To"].Value = AssignedTo.SelectedItem));
                    if (selectedWorkItemType.Equals("Bug"))
                    {
                        BeginInvoke(new Action(() => newTFSWorkItem.Fields["Severity"].Value = Severity.SelectedItem));
                        BeginInvoke(new Action(() => newTFSWorkItem.Fields["Repro Steps"].Value = StepsToReproduce.Text));
                    }
                    else
                        BeginInvoke(new Action(() => newTFSWorkItem.Fields["Description"].Value = StepsToReproduce.Text));

                    if (!string.IsNullOrWhiteSpace(Attachments.Text))
                    {
                        workItemAttachments = Attachments.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        foreach (string attachment in workItemAttachments)
                        {
                            if (!string.IsNullOrWhiteSpace(attachment))
                                attachmentCollection.Add(new Attachment(attachment));
                        }
                    }
                    var validationResult = newTFSWorkItem.Validate();
                    if (validationResult.Count == 0)
                    {
                        newTFSWorkItem.Save();
                    }
                    else
                    {
                        List<string> ErrorCodes = new List<string>();
                        foreach (var result in validationResult)
                        {
                            Field field = result as Field;
                            if (field == null)
                                ErrorCodes.Add(result.ToString());
                            else
                                ErrorCodes.Add($"Error with: {field.Name}");
                        }
                    }

                    return (newTFSWorkItem.Id);
                });
                return res;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (ex.InnerException != null)
                    MessageBox.Show(ex.InnerException.ToString());
                //workItemCreated = false;
                return 0;
            }
        }

        private async void saveTFSWorkItem_Click(object sender, EventArgs e)
        {
            int res = await SaveTFSBug();
            if (res > 0)
            {
                MessageBox.Show("New work item created successfully", "Create Work Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }

            else
            {
                MessageBox.Show("Error creating new work item", "Create Work Item", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void ultraPictureBoxOpenFileDialog_Click(object sender, EventArgs e)
        {
            OpenDocuments();
        }
        private void OpenDocuments()
        {
            try
            {
                openFileDialog1.InitialDirectory = "C:\\";

                DialogResult result = openFileDialog1.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    Attachments.Text += openFileDialog1.FileName + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {

            }

        }
    }
}
