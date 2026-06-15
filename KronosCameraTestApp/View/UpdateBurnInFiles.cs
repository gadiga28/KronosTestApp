using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinListView;
using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thermo.Kronos.Instrument.Camera.Interface;
namespace KronosCameraTestApp.View
{
    public partial class UpdateBurnInFiles : Form
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        CIDInterface cidInterface;
        bool exitEditMode = true;
        List<BurnInDMesgIgnoreModel> burnInDMesgIgnoreModelList { get; set; }
        List<BurnInDMesgIgnoreModel> updatedBurnInDMesgIgnoreModelList { get; set; }
        string selectedBurnInFile = string.Empty;
        string userMode;
        bool engineeringLoginStatus;
        public string SelectedBurnInFile
        {
            get
            {
                return selectedBurnInFile;
            }
            set
            {
                selectedBurnInFile = value;
            }
        }
        bool burnInFilesLoaded = false;
        public bool BurnInFilesLoaded
        {
            get
            {
                return burnInFilesLoaded;
            }
            set
            {
                burnInFilesLoaded = value;
            }
        }
        public UpdateBurnInFiles(CIDInterface cidInterface, string userMode, bool engineeringLoginStatus)
        {
            InitializeComponent();
            this.cidInterface = cidInterface;
            listViewBurnInFolders.Items.Clear();
            getBurnInFolders();           
            listViewBurnInFolders.Items[0].Activate();
            listViewBurnInFolders.SelectedItems.Add(listViewBurnInFolders.Items[0]);
            listViewBurnInFolders.Select();
            selectedBurnInFile = listViewBurnInFolders.SelectedItems[0].Key;
            this.userMode = userMode;
            this.engineeringLoginStatus = engineeringLoginStatus;
            getDmesgIgnoreFromDB();
        }
        private void UpdateBurnInFiles_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                listViewBurnInFolders.ItemSettings.SelectionType = SelectionType.Single;
                MouseEnterLeaveEventHandlers();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                Cursor.Current = Cursors.Arrow;
            }
        }
        private void MouseEnterLeaveEventHandlers()
        {
            try
            {
                AddIgnore.MouseEnter += buttonMouseEnter;
                RemoveIgnore.MouseEnter += buttonMouseEnter;
                UpdateIgnore.MouseEnter += buttonMouseEnter;                
                RefreshIgnore.MouseEnter += buttonMouseEnter;
                AddIgnore.MouseLeave += buttonMouseLeave;
                RemoveIgnore.MouseLeave += buttonMouseLeave;
                UpdateIgnore.MouseLeave += buttonMouseLeave;
                RefreshIgnore.MouseLeave += buttonMouseLeave;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void buttonMouseEnter(object sender, EventArgs e)
        {
            UltraButton button = sender as UltraButton;
            button.Size = new Size(48, 48);
            button.ImageSize = new Size(40, 40);
        }
        private void buttonMouseLeave(object sender, EventArgs e)
        {
            UltraButton button = sender as UltraButton;
            button.Size = new Size(40, 40);
            button.ImageSize = new Size(32, 32);
        }
        private void getDmesgIgnoreFromDB()
        {
            try
            {
                burnInDMesgIgnoreModelList = new List<BurnInDMesgIgnoreModel>();
                using (var kcc = new KronosCamContext())
                {
                     burnInDMesgIgnoreModelList = (from dMesgIgnore in kcc.burnInDMesgIgnoreModel select dMesgIgnore).ToList();
                }
                if(burnInDMesgIgnoreModelList.Count > 0)
                {
                    gridDMesgIgnoreData.DataSource = burnInDMesgIgnoreModelList;
                    gridDMesgIgnoreData.DataBind();
                    TestAppHelper.burnInDMesgIgnoreModelList = burnInDMesgIgnoreModelList;                    
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void getBurnInFolders()
        {
            try
            {
                    DirectoryInfo dinfo = new DirectoryInfo(ConfigurationManager.AppSettings["UpdateBurnInFilePath"].ToString());
                    List<string> burnInFoldersFiles = new List<string>();
                    foreach (DirectoryInfo directoryInfo in dinfo.GetDirectories())
                    {
                        burnInFoldersFiles.Add(directoryInfo.Name);
                    }
                    burnInFoldersFiles.Sort();
                    burnInFoldersFiles.Reverse();
                    foreach (string item in burnInFoldersFiles)
                    {
                        UltraListViewItem ultraListViewSubItem = new UltraListViewItem();
                        ultraListViewSubItem.Key = item;
                        ultraListViewSubItem.Value = item;
                        listViewBurnInFolders.Items.Add(ultraListViewSubItem);
                    }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                Cursor.Current = Cursors.Arrow;
            }
        }
        private void buttonUpload_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show(string.Format("Are you sure to upload the {0}  BurnIn script?", selectedBurnInFile),
                                                                "Upload BurnIn Script", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (uploadBurnInFiles())
                    {
                        MessageBox.Show(string.Format("BurnIn script {0}  uploaded successfully!", selectedBurnInFile),
                                "Upload BurnIn Script", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        burnInFilesLoaded = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(string.Format("Error uploading {0}  BurnIn script!", selectedBurnInFile),
                                         "Upload BurnIn Script", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        burnInFilesLoaded = false;
                    }
                    Cursor.Current = Cursors.Arrow;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                burnInFilesLoaded = false;
                Cursor.Current = Cursors.Arrow;
            }
        }
        public bool uploadBurnInFiles()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (cidInterface.IsConnected())
                    {
                        using (var sftpClient = new SftpClient("192.168.1.2", 22, "root", "cidtec"))
                        {
                            sftpClient.Connect();
                            sftpClient.ChangeDirectory("/root");
                            DirectoryInfo dinfo = new DirectoryInfo(ConfigurationManager.AppSettings["UpdateBurnInFilePath"].ToString() +
                                                       selectedBurnInFile + "\\");
                            List<FileInfo> burnInFolderFiles = dinfo.GetFiles().ToList();
                            foreach (var burnInFile in burnInFolderFiles)
                            {
                                using (var uplfileStream = File.OpenRead(burnInFile.FullName))
                                {
                                    if (!burnInFile.Name.Equals("launcher.sh"))
                                        sftpClient.UploadFile(uplfileStream, burnInFile.Name, true);
                                }
                            }
                            FileInfo luancherFile = burnInFolderFiles.Where(file => file.Name.Equals("launcher.sh")).Single();
                            if (luancherFile != null)
                            {
                                using (var uplfileStream = File.OpenRead(luancherFile.FullName))
                                {
                                    sftpClient.UploadFile(uplfileStream, luancherFile.Name, true);
                                }
                            }
                            var files = sftpClient.ListDirectory("/root");
                            foreach (var file in files)
                            {
                                string remoteFileName = file.Name;
                                if (file.Name.Equals("burnin.sh") ||
                                    file.Name.Equals("launcher.sh") )
                                {
                                    sftpClient.ChangePermissions(file.FullName, 775);
                                    Thread.Sleep(5000);
                                }
                            }
                            sftpClient.Disconnect();
                        }
                    }
                Cursor.Current = Cursors.Arrow;
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                Cursor.Current = Cursors.Arrow;
                return false;
            }
        }
        private void listViewBurnInFolders_ItemSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            try
            {
                selectedBurnInFile = listViewBurnInFolders.SelectedItems[0].Key;
                buttonUpload.Enabled = true;
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
        private void ultraGridExposureData_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                gridDMesgIgnoreData.DisplayLayout.Bands[0].Columns["IgnoreID"].Hidden = true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UltraGridExitEditMode()
        {
            try
            {
                if (exitEditMode)
                {
                    gridDMesgIgnoreData.PerformAction(UltraGridAction.ExitEditMode);
                }
                exitEditMode = true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void gridDMesgIgnoreData_Click(object sender, EventArgs e)
        {
            UltraGridExitEditMode();
        }
        private void gridDMesgIgnoreData_ClickCell(object sender, ClickCellEventArgs e)
        {
            exitEditMode = true;
        }
        private void gridDMesgIgnoreData_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {                  
                UpdateIgnore.Enabled = true;
                RemoveIgnore.Enabled = true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateBurnInFiles_FormClosing(object sender, FormClosingEventArgs e)
        {
            TestAppHelper.burnInDMesgIgnoreModelList = burnInDMesgIgnoreModelList;
        }
        private void updateDMsgIgnoreData(BurnInDMesgIgnoreModel burnInDMesgIgnoreModel, string OperationType)
        {
            try
            {
                if (userMode.Equals("Admin"))
                {
                    using (var kcc = new KronosCamContext())
                    {
                        if (OperationType == "ADD")
                        {
                            kcc.burnInDMesgIgnoreModel.Add(burnInDMesgIgnoreModel);
                            AddIgnore.Enabled = false;
                        }
                        else if (OperationType == "REMOVE")
                        {
                            BurnInDMesgIgnoreModel exposureRecord = kcc.burnInDMesgIgnoreModel.Where(u => u.IgnoreID.Equals(burnInDMesgIgnoreModel.IgnoreID)).FirstOrDefault();
                            kcc.burnInDMesgIgnoreModel.Remove(exposureRecord);
                            RemoveIgnore.Enabled = false;
                        }
                        else if (OperationType == "UPDATE")
                        {
                            foreach (BurnInDMesgIgnoreModel edm in burnInDMesgIgnoreModelList)
                            {
                                kcc.burnInDMesgIgnoreModel.Attach(edm);
                                kcc.Entry(edm).State = System.Data.Entity.EntityState.Modified;
                            }
                            UpdateIgnore.Enabled = false;
                        }
                        if (kcc.SaveChanges() > 0)
                        {
                            MessageBox.Show("Success", "Update DMesgIgnore Data", MessageBoxButtons.OK);
                            getDmesgIgnoreFromDB();
                        }
                        else
                        {
                            MessageBox.Show("Failed", "Update DMesgIgnore Data", MessageBoxButtons.OK);
                        }
                    }
                }
                else
                {
                    if (OperationType == "ADD")
                    {
                        burnInDMesgIgnoreModelList.Add(burnInDMesgIgnoreModel);
                        AddIgnore.Enabled = false;
                    }
                    else if (OperationType == "REMOVE")
                    {
                        BurnInDMesgIgnoreModel exposureRecord = TestAppHelper.burnInDMesgIgnoreModelList.Where(u => u.IgnoreID.Equals(burnInDMesgIgnoreModel.IgnoreID)).FirstOrDefault();
                        burnInDMesgIgnoreModelList.Remove(exposureRecord);
                        RemoveIgnore.Enabled = false;
                    }
                    else if (OperationType == "UPDATE")
                    {
                        TestAppHelper.burnInDMesgIgnoreModelList = burnInDMesgIgnoreModelList;
                        UpdateIgnore.Enabled = false;
                    }
                    gridDMesgIgnoreData.DataSource = burnInDMesgIgnoreModelList;
                    gridDMesgIgnoreData.DataBind();
                    TestAppHelper.burnInDMesgIgnoreModelList = burnInDMesgIgnoreModelList;
                }
             }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                UpdateIgnore.Enabled = true;
                RemoveIgnore.Enabled = true;
                AddIgnore.Enabled = true;
            }
        }
        private void AddIgnore_Click(object sender, EventArgs e)
        {
            try
            {
                BurnInDMesgIgnoreModel burnInDMesgIgnoreModel = new BurnInDMesgIgnoreModel();
                burnInDMesgIgnoreModel.DMesgIgnoreText = dMesgIgnoreNewAdd.Text;
                updateDMsgIgnoreData(burnInDMesgIgnoreModel, "ADD");
                dMesgIgnoreNewAdd.Text = string.Empty;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateIgnore_Click(object sender, EventArgs e)
        {
            updateDMsgIgnoreData(null, "UPDATE");
        }
        private void RemoveIgnore_Click(object sender, EventArgs e)
        {
            try
            {
                var ignoreRecord = burnInDMesgIgnoreModelList.FirstOrDefault(exp => exp.IgnoreID.Equals(Convert.ToInt32(gridDMesgIgnoreData.ActiveRow.Cells["IgnoreID"].Value)));
                if (ignoreRecord != null)
                {
                    updateDMsgIgnoreData(ignoreRecord, "REMOVE");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void RefreshDMesgIgnore_Click(object sender, EventArgs e)
        {
            try
            {
                getDmesgIgnoreFromDB();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void dMesgIgnoreNewAdd_TextChanged(object sender, EventArgs e)
        {
            AddIgnore.Enabled = true;
        }
    }
}
