using Infragistics.Win;
using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinListView;
using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.Presenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace KronosCameraTestApp.View
{
    public partial class CameraTestLogDetails : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
               System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string userRole = string.Empty;
        string cameraSNOnLoad = string.Empty;
        string imagerSNOnLoad = string.Empty;
        bool gridIntializeForExport = false;
        List<AffectedItemsModel> affectedItemsModelList;
        List<CameraDetailsModel> cameraDetailsModelList;
        List<CameraTestLogsModel> cameraTestLogsList;
        CameraTestLogsModel selectedCameraTestLog;
        public CameraTestLogsPresenter cameraTestLogsPresenter { get; set; }
        public CameraTestLogDetails()
        {
            InitializeComponent();
        }
        public CameraTestLogDetails(CameraTestLogsPresenter cameraTestLogsPresenter, string UserRole, string cameraSNOnLoad, string imagerSNOnLoad)
        {
            InitializeComponent();
            this.userRole = UserRole;
            this.cameraTestLogsPresenter = cameraTestLogsPresenter;
            this.cameraSNOnLoad = cameraSNOnLoad;
            this.imagerSNOnLoad = imagerSNOnLoad;
            MouseEnterLeaveEventHandlers();
        }
        private void MouseEnterLeaveEventHandlers()
        {
            try
            {
                AddAffectedItem.MouseEnter += buttonMouseEnter;
                updateTestLog.MouseEnter += buttonMouseEnter;
                refreshCameraTestLogs.MouseEnter += buttonMouseEnter;
                listCameraTestLogs.MouseEnter += buttonMouseEnter;
                SearchPatternRecords.MouseEnter += buttonMouseEnter;
                updateTestLog.MouseLeave += buttonMouseLeave;
                AddAffectedItem.MouseLeave += buttonMouseLeave;
                refreshCameraTestLogs.MouseLeave += buttonMouseLeave;
                listCameraTestLogs.MouseLeave += buttonMouseLeave;
                SearchPatternRecords.MouseLeave += buttonMouseLeave;
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
        private async void CameraTestLogDetails_Load(object sender, EventArgs e)
        {
            try
            {
                Application.UseWaitCursor = true;
                cameraSN.Clear();
                cameraDetailsModelList = await cameraTestLogsPresenter.GetCameraDetails();
                if (cameraDetailsModelList.Count() > 0)
                {
                    cameraSN.DataSource = (from camera in cameraDetailsModelList
                                           orderby camera.DateDefined
                                           select camera.CameraSerialNumber).Distinct().ToList();
                    cameraSN.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Suggest;

                    imagerSNComboEditor.DataSource= (from imager in cameraDetailsModelList
                                                     orderby imager.DateDefined
                                                     select imager.ImagerSerialNumber).Distinct().ToList();
                    imagerSNComboEditor.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Suggest;
                }
                   
                if (!string.IsNullOrEmpty(cameraSNOnLoad))
                {
                    cameraSN.Text = cameraSNOnLoad;
                }
                    
                else if(!string.IsNullOrEmpty(imagerSNOnLoad))
                {
                    imagerSNComboEditor.Text = imagerSNOnLoad;
                }

                GetAffectedItemsList();
                Application.UseWaitCursor = false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                Application.UseWaitCursor = false;
            }
        }     
        private async void GetAffectedItemsList()
        {
            affectedItemsModelList = await cameraTestLogsPresenter.GetAffectedItems();
            if (affectedItemsModelList.Count() > 0)
            {
                cameraTestLogAffectedItems.Items.Clear();
                foreach (AffectedItemsModel affectedItemsModel in affectedItemsModelList)
                {
                    UltraListViewItem ultraListViewSubItem = new UltraListViewItem();
                    ultraListViewSubItem.Key = affectedItemsModel.PartNumber.Trim();
                    ultraListViewSubItem.Value = affectedItemsModel.ItemName.Trim();
                    cameraTestLogAffectedItems.Items.Add(ultraListViewSubItem);
                }
            }
        }
        private async void cameraSN_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if(cameraSN.SelectedItem !=null)
                    cameraTestLogsList = await cameraTestLogsPresenter.GetCameraTestLogs(cameraSN.SelectedItem.DisplayText,string.Empty);
                else
                    cameraTestLogsList = await cameraTestLogsPresenter.GetCameraTestLogs(string.Empty, string.Empty);
                if (cameraTestLogsList.Count() > 0)
                {
                    cameraTestLogUltraGrid.DataSource = cameraTestLogsList;
                    if(cameraSN.SelectedItem !=null)
                    {
                        ultraGroupBoxCameraMetrics.Text = cameraSN.SelectedItem.DisplayText + " Metrics";
                    }
                    
                    ultraGroupBoxCameraMetrics.Appearance.FontData.Bold = DefaultableBoolean.True;
                    await updateCameraTestMetricsView(cameraTestLogsList);
                }                    
                else
                {
                    MessageBox.Show("No camera test logs found", "Camera Test Logs", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    resetUI();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }  
        private void cameraTestLogUltraGrid_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {               
                cameraTestLogUltraGridInitialize(gridIntializeForExport,sender, e);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void cameraTestLogUltraGridInitialize(bool forExport, object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                if(forExport)
                {                  
                    cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["FailureMode"].Hidden = false;
                    cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["AffectedItems"].Hidden = false;
                    cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["RootCause"].Hidden = false;
                    cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["Symptom"].Hidden = false;
                    cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["Disposition"].Hidden = false;
                    gridIntializeForExport = false;
                }
                else
                {
                    e.Layout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
                    cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["ResultsID"].Hidden = true;
                    cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["CameraTestID"].Hidden = true;
                    cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["FailureMode"].Hidden = true;
                    cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["AffectedItems"].Hidden = true;
                    cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["RootCause"].Hidden = true;
                    cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["Symptom"].Hidden = true;
                    cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["Disposition"].Hidden = true;
                    string ImagerSN = string.Empty;
                    if (cameraSN.SelectedItem !=null)
                    {
                        CameraDetailsModel cameraDetailsModel = cameraDetailsModelList.Where(camera => camera.CameraSerialNumber.Equals(cameraSN.SelectedItem.DisplayText)).FirstOrDefault();
                        if (cameraDetailsModel != null)
                        {
                            ImagerSN = cameraDetailsModel.ImagerSerialNumber;
                        }
                    }
                    if (cameraTestLogUltraGrid.Rows.Count > 0)
                    {
                        foreach (UltraGridRow row in cameraTestLogUltraGrid.Rows)
                        {
                            int cameraTestID = (int)row.Cells["CameraTestID"].Value;
                            CameraDetailsModel cameraDetailsModel = cameraDetailsModelList.Where(camera => camera.CameraTestID.Equals(cameraTestID)).FirstOrDefault();
                            if (cameraDetailsModel != null)
                            {
                                ImagerSN = cameraDetailsModel.ImagerSerialNumber;
                            }
                            row.Cells["ImagerSerialNumber"].Value = ImagerSN;
                            if (row.Cells["TestStage"].Value.Equals("PRE-TEST"))
                            {
                                if ((bool)row.Cells["PreTestResult"].Value)
                                    row.Cells["PreTestResult"].Appearance.BackColor = Color.LimeGreen;
                                else
                                    row.Cells["PreTestResult"].Appearance.BackColor = Color.Red;
                            }
                            if (row.Cells["TestStage"].Value.Equals("FINAL_TEST"))
                            {
                                if ((bool)row.Cells["FinalTestResult"].Value)
                                    row.Cells["FinalTestResult"].Appearance.BackColor = Color.LimeGreen;
                                else
                                    row.Cells["FinalTestResult"].Appearance.BackColor = Color.Red;
                            }
                        }
                    }
                    cameraTestLogUltraGrid.DisplayLayout.Override.SelectTypeRow = SelectType.Single;
                    cameraTestLogUltraGrid.Rows[0].Activate();
                    getSelectedCameraTestLog();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void cameraTestLogUltraGrid_ClickCell(object sender, ClickCellEventArgs e)
        {
            try
            {
                cameraTestLogUltraGrid.ActiveRow.Selected = true;               
                getSelectedCameraTestLog();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void getSelectedCameraTestLog()
        {
            try
            {
                foreach (UltraGridCell cell in cameraTestLogUltraGrid.ActiveRow.Cells)
                {
                    if (cell.Column.Key.ToString().Equals("CameraTestID"))
                    {
                        if (cell.Value != null)
                        {
                            selectedCameraTestLog = (from cameralog in cameraTestLogsList
                                                     where cameralog.CameraTestID.Equals(cell.Value)
                                                     select cameralog).FirstOrDefault();
                            if (selectedCameraTestLog != null)
                            {
                                updateCameraFailRootSymptomsView();
                            }
                            else
                            {
                                cameraTestLogRootCause.Text = string.Empty;
                                cameraTestLogSymptom.Text = string.Empty;
                                cameraTestLogDisposition.Text = string.Empty;
                                resetFailureModes();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void updateCameraFailRootSymptomsView()
        {
            try
            {
                cameraTestLogRootCause.Text = selectedCameraTestLog.RootCause;
                cameraTestLogSymptom.Text = selectedCameraTestLog.Symptom;
                cameraTestLogDisposition.Text = selectedCameraTestLog.Disposition;
                if(!string.IsNullOrWhiteSpace(selectedCameraTestLog.FailureMode))
                {
                    string[] failureModes = selectedCameraTestLog.FailureMode.Split(new char[] { ',' });
                    foreach (string failuremode in failureModes)
                    {
                        for (int i = 0; i < cameraTestLogFailureModes.Items.Count; i++)
                        {
                            if (failuremode.Trim(' ').Equals(cameraTestLogFailureModes.Items[i].Value))
                            {
                                cameraTestLogFailureModes.Items[i].CheckState = CheckState.Checked;
                                break;
                            }
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(selectedCameraTestLog.AffectedItems))
                {
                    string[] affectedItems = selectedCameraTestLog.AffectedItems.Split(new char[] { ',' });
                    foreach (string affectedItem in affectedItems)
                    {
                        for (int i = 0; i < cameraTestLogAffectedItems.Items.Count; i++)
                        {
                            if ((affectedItem.Trim().Split('-')[0]).Equals(cameraTestLogAffectedItems.Items[i].Value))
                            {
                                cameraTestLogAffectedItems.Items[i].CheckState = CheckState.Checked;
                                break;
                            }
                        }
                    }
                }
                   
                resetFailureModes();
               
                
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task updateCameraTestMetricsView(List<CameraTestLogsModel> cameraTestLogs)
        {
            try
            {
                await Task.Run(() =>
                {
                    int preTestPassCount = 0;
                    int preTestFailCount = 0;
                    int finalTestPassCount = 0;
                    int finalTestFailCount = 0;
                    foreach (CameraTestLogsModel cameralog in cameraTestLogs)
                    {
                        switch (cameralog.TestStage)
                        {
                            case "PRE-TEST":
                                if (cameralog.PreTestResult)
                                {
                                    preTestPassCount++;
                                    BeginInvoke(new System.Action(() => preTestPassCountText.Text = preTestPassCount.ToString()));
                                }                                    
                                else
                                {
                                    preTestFailCount++;
                                    BeginInvoke(new System.Action(() => preTestFailCountText.Text = preTestFailCount.ToString()));
                                }
                                   
                                break;
                            case "FINAL_TEST":
                                if (cameralog.FinalTestResult)
                                {
                                    finalTestPassCount++;
                                    BeginInvoke(new System.Action(() => finalTestPassCountText.Text = finalTestPassCount.ToString()));
                                }                                    
                                else
                                {
                                    finalTestFailCount++;
                                    BeginInvoke(new System.Action(() => finalTestFailCountText.Text = finalTestFailCount.ToString()));
                                }   
                                break;
                        }
                    }
                    if (preTestPassCount > 0)
                    {
                        try
                        {
                            BeginInvoke(new System.Action(() => preTestYield.Text = ((preTestPassCount * 100) / (preTestPassCount + preTestFailCount)).ToString()));
                        }
                        catch (Exception ex)
                        {
                            log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                        }
                    }  
                    if(preTestPassCount==0)
                    {
                        BeginInvoke(new System.Action(() => preTestPassCountText.Text = string.Empty));
                        BeginInvoke(new System.Action(() => preTestYield.Text = string.Empty));
                    }
                    if (preTestFailCount == 0)
                    {
                        BeginInvoke(new System.Action(() => preTestFailCountText.Text = string.Empty));
                    }
                    if (finalTestPassCount > 0)
                    {
                        try
                        {
                            BeginInvoke(new System.Action(() => finalTestYield.Text = ((finalTestPassCount * 100) / (finalTestPassCount + finalTestFailCount)).ToString()));
                        }
                        catch (Exception ex)
                        {
                            log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                        }
                    }
                    if (finalTestPassCount == 0)
                    {
                        BeginInvoke(new System.Action(() => finalTestPassCountText.Text = string.Empty));
                        BeginInvoke(new System.Action(() => finalTestYield.Text = string.Empty));
                    }
                    if (finalTestFailCount == 0)
                    {
                        BeginInvoke(new System.Action(() => finalTestFailCountText.Text = string.Empty));
                    }                    
                });
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void updateTestLog_Click(object sender, EventArgs e)
        {
            try
            {
                if (cameraTestLogUltraGrid.ActiveRow == null || selectedCameraTestLog == null)
                {
                    MessageBox.Show("Please select a log from the grid to update.", "Update Camera Test Logs", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                selectedCameraTestLog.DateExecuted = DateTime.Now;
                selectedCameraTestLog.RootCause = cameraTestLogRootCause.Text;
                
                selectedCameraTestLog.Disposition = cameraTestLogDisposition.Text;
                selectedCameraTestLog.UserExecuted= string.IsNullOrWhiteSpace(TestAppHelper.UserLoginName) ? Environment.UserName : TestAppHelper.UserLoginName;
                UltraListViewCheckedItemsCollection affectedItemscheckedList = cameraTestLogAffectedItems.CheckedItems;

                if(affectedItemscheckedList.Count() > 0)
                {
                    StringBuilder affectedItems = new StringBuilder();
                    for(int i=0;i< affectedItemscheckedList.Count;i++)
                    {
                        affectedItems.Append(affectedItemscheckedList[i].Value.ToString() + "-" +
                            affectedItemscheckedList[i].Key.ToString() + " , ");
                    }
                    selectedCameraTestLog.AffectedItems = affectedItems.ToString().TrimEnd(new char[] { ',', ' ' });
                }
                else
                {
                    selectedCameraTestLog.AffectedItems = string.Empty;
                }

                UltraListViewCheckedItemsCollection checkedItems = cameraTestLogFailureModes.CheckedItems;
                if (checkedItems.Count() > 0)
                {
                    StringBuilder failureModes = new StringBuilder();                    
                    StringBuilder boardSN = new StringBuilder();
                    CameraDetailsModel cameraDetailsModel = cameraDetailsModelList.Where(camera => camera.CameraTestID.Equals(selectedCameraTestLog.CameraTestID)).FirstOrDefault();
                   
                    for (int i = 0; i < checkedItems.Count; i++)
                    {
                        failureModes.Append(checkedItems[i].Value.ToString() + " , ");

                        if (checkedItems[i].Key.Equals("CPU") && cameraDetailsModel!=null)
                        {
                            boardSN.Append("CPU:" + cameraDetailsModel.CPUSerialNumber + " , ");
                            selectedCameraTestLog.GenerateMFR = true;
                        }
                        else if(checkedItems[i].Key.Equals("CSP") && cameraDetailsModel != null)
                        {
                            boardSN.Append("CSP:" + cameraDetailsModel.CSPSerialNumber + " , ");
                            selectedCameraTestLog.GenerateMFR = true;
                        }
                        else if (checkedItems[i].Key.Equals("IMR") && cameraDetailsModel != null)
                        {
                            boardSN.Append("IMR:" + cameraDetailsModel.ImagerSerialNumber + " , ");
                            selectedCameraTestLog.GenerateMFR = true;
                        }
                        else if (checkedItems[i].Key.Equals("ISI") && cameraDetailsModel != null)
                        {
                            boardSN.Append("ISI:" + cameraDetailsModel.ISISerialNumber + " , ");
                            selectedCameraTestLog.GenerateMFR = true;
                        }
                        else if (checkedItems[i].Key.Equals("PWB") && cameraDetailsModel != null)
                        {
                            boardSN.Append("PWB:" + cameraDetailsModel.PowerSerialNumber + " , ");
                            selectedCameraTestLog.GenerateMFR = true;
                        }
                    }
                    if (selectedCameraTestLog.GenerateMFR == true && affectedItemscheckedList.Count() == 0)
                    {
                        MessageBox.Show("Please select one of the Affected Items list corresponding to the" +
                                        " board failure selected in failure modes list.", "Update Camera Test Logs", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (selectedCameraTestLog.GenerateMFR == true && string.IsNullOrWhiteSpace(cameraTestLogSymptom.Text))
                    {
                        MessageBox.Show("Please enter Symptom for board failure selected in failure modes list.",
                                        "Update Camera Test Logs", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }
                    selectedCameraTestLog.FailureMode = failureModes.ToString().TrimEnd(new char[] { ',', ' ' });
                    selectedCameraTestLog.BoardSN = boardSN.ToString().TrimEnd(new char[] { ',', ' ' });
                }
                else
                {
                    selectedCameraTestLog.FailureMode = string.Empty;
                }
                    

                selectedCameraTestLog.Symptom = cameraTestLogSymptom.Text;
                if ( await cameraTestLogsPresenter.updateCameraTestLogsInDB(selectedCameraTestLog))
                {                    
                    MessageBox.Show("Camera test logs updated successfully", "Update Camera Test Logs", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await updateCameraTestFailureModesMetrics(selectedCameraTestLog.ResultsID);
                }                    
               else
                    MessageBox.Show("Error updating camera test logs", "Update Camera Test Logs", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                MessageBox.Show("Unknown error updating camera test logs", "Update Camera Test Logs", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task updateCameraTestFailureModesMetrics(int resultsID)
        {
            try
            {
                CameraTestFailureModesMetricsModel cameraTestFailureModesMetricsModel = await cameraTestLogsPresenter.GetCameraTestFailureModesMetrics(resultsID);
                string operationType = string.Empty;
                if (cameraTestFailureModesMetricsModel != null)
                {
                    operationType = "Update";
                }
                else
                {
                    cameraTestFailureModesMetricsModel = new CameraTestFailureModesMetricsModel();
                    cameraTestFailureModesMetricsModel.ResultsID = resultsID;
                    operationType = "Add";
                }
                foreach (UltraListViewItem item in cameraTestLogFailureModes.Items)
                {
                    CheckState checkState = item.CheckState;
                    switch (item.Key)
                    {
                        case "CPU":
                            if (checkState.Equals(CheckState.Checked))
                                cameraTestFailureModesMetricsModel.CPU = 1;
                            else
                                cameraTestFailureModesMetricsModel.CPU = 0;
                            break;
                        case "FRM":
                            if (checkState.Equals(CheckState.Checked))
                                cameraTestFailureModesMetricsModel.Firmware = 1;
                            else
                                cameraTestFailureModesMetricsModel.Firmware = 0;
                            break;
                        case "COT":
                            if (checkState.Equals(CheckState.Checked))
                                cameraTestFailureModesMetricsModel.Coating = 1;
                            else
                                cameraTestFailureModesMetricsModel.Coating = 0;
                            break;
                        case "GLS":
                            if (checkState.Equals(CheckState.Checked))
                                cameraTestFailureModesMetricsModel.Glass = 1;
                            else
                                cameraTestFailureModesMetricsModel.Glass = 0;
                            break;
                        case "IMR":
                            if (checkState.Equals(CheckState.Checked))
                                cameraTestFailureModesMetricsModel.Imager = 1;
                            else
                                cameraTestFailureModesMetricsModel.Imager = 0;
                            break;
                        case "ISI":
                            if (checkState.Equals(CheckState.Checked))
                                cameraTestFailureModesMetricsModel.ISIBoard = 1;
                            else
                                cameraTestFailureModesMetricsModel.ISIBoard = 0;
                            break;
                        case "PWB":
                            if (checkState.Equals(CheckState.Checked))
                                cameraTestFailureModesMetricsModel.PowerBoard = 1;
                            else
                                cameraTestFailureModesMetricsModel.PowerBoard = 0;
                            break;
                        case "CSP":
                            if (checkState.Equals(CheckState.Checked))
                                cameraTestFailureModesMetricsModel.CSPBoard = 1;
                            else
                                cameraTestFailureModesMetricsModel.CSPBoard = 0;
                            break;
                        case "PRC":
                            if (checkState.Equals(CheckState.Checked))
                                cameraTestFailureModesMetricsModel.Procedural = 1;
                            else
                                cameraTestFailureModesMetricsModel.Procedural = 0;
                            break;
                        case "TEC":
                            if (checkState.Equals(CheckState.Checked))
                                cameraTestFailureModesMetricsModel.TECooler = 1;
                            else
                                cameraTestFailureModesMetricsModel.TECooler = 0;
                            break;
                        case "TBD":
                            if (checkState.Equals(CheckState.Checked))
                                cameraTestFailureModesMetricsModel.TBD = 1;
                            else
                                cameraTestFailureModesMetricsModel.TBD = 0;
                            break;
                        case "OTH":
                            if (checkState.Equals(CheckState.Checked))
                                cameraTestFailureModesMetricsModel.Others = 1;
                            else
                                cameraTestFailureModesMetricsModel.Others = 0;
                            break;
                    }
                }
                await cameraTestLogsPresenter.updateCameraFailureModesMetrics(cameraTestFailureModesMetricsModel, operationType);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void cancelTestLog_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private async void refreshCameraTestLogs_Click(object sender, EventArgs e)
        {
            try
            {
                CameraTestLogDetails_Load(null, null);               
                cameraTestLogsList = await cameraTestLogsPresenter.GetCameraTestLogs(string.Empty,string.Empty);
                resetUI();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void resetUI()
        {
            try
            {
                cameraTestLogUltraGrid.DataSource = null;
                cameraTestLogRootCause.Text = string.Empty;
                cameraTestLogSymptom.Text = string.Empty;
                cameraTestLogDisposition.Text = string.Empty;
                resetFailureModes();
                ultraGroupBoxCameraMetrics.Text = string.Empty;
                preTestPassCountText.Text = string.Empty;
                preTestFailCountText.Text = string.Empty;
                preTestYield.Text = string.Empty;
                finalTestPassCountText.Text = string.Empty;
                finalTestFailCountText.Text = string.Empty;
                finalTestYield.Text = string.Empty;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void resetFailureModes()
        {
            try
            {
                for (int i = 0; i < cameraTestLogFailureModes.Items.Count; i++)
                {
                    cameraTestLogFailureModes.Items[i].CheckState = CheckState.Unchecked;
                }
                for (int i = 0; i < cameraTestLogAffectedItems.Items.Count; i++)
                {
                    cameraTestLogAffectedItems.Items[i].CheckState = CheckState.Unchecked;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void listCameraTestLogs_Click(object sender, EventArgs e)
        {
            try
            {
                Application.UseWaitCursor = true;
                cameraSN.SelectedItem = null;
                if(cameraTestLogsPresenter !=null)
                {
                    cameraTestLogsList = await cameraTestLogsPresenter.GetCameraTestLogs(string.Empty,string.Empty);
                    if(cameraTestLogsList.Count > 0)
                        cameraTestLogUltraGrid.DataSource = cameraTestLogsList;
                }
                Application.UseWaitCursor = false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                Application.UseWaitCursor = false;
            }
        }
        private void cameraTestLogContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                ToolStripItem item = e.ClickedItem;
                if (item.Name.Equals("ExportGridToExcel"))
                {
                    ExportDataGridToExcel();
                }
                if (item.Name.Equals("PrintDataGrid"))
                {
                    PrintGrid();
                }
                cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["ResultsID"].Hidden = true;
                cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["CameraTestID"].Hidden = true;
                cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["FailureMode"].Hidden = true;
                cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["AffectedItems"].Hidden = true;
                cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["RootCause"].Hidden = true;
                cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["Symptom"].Hidden = true;
                cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["Disposition"].Hidden = true;
                cameraTestLogUltraGrid.Refresh();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void PrintGrid()
        {
            try
            {
                cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["AffectedItems"].Hidden = false;
                cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["FailureMode"].Hidden = false;
                cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["RootCause"].Hidden = false;
                cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["Symptom"].Hidden = false;
                cameraTestLogUltraGrid.DisplayLayout.Bands[0].Columns["Disposition"].Hidden = false;
                cameraTestLogUltraGrid.Refresh();
                cameraTestLogUltraGrid.PrintPreview(cameraTestLogUltraGrid.DisplayLayout);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ExportDataGridToExcel()
        {
            try
            {
                cameraTestMetricsSaveFileDialog.InitialDirectory = Properties.Settings.Default["FilesLocation"].ToString();
                string datetimestamp = DateTime.Now.ToString("MMddyyyy_hhmm");
                cameraTestMetricsSaveFileDialog.Title = "Save Test Log DataGrid to Excel";
                cameraTestMetricsSaveFileDialog.DefaultExt = "*.xlsx";
                cameraTestMetricsSaveFileDialog.Filter = "Excel|*.xlsx";
                cameraTestMetricsSaveFileDialog.FileName = cameraTestLogUltraGrid.Text + "_" + datetimestamp;
                if (cameraTestMetricsSaveFileDialog.ShowDialog(this) == DialogResult.OK && cameraTestMetricsSaveFileDialog.FileName.Length > 0)
                {
                    gridIntializeForExport = true;
                    cameraTestLogUltraGrid_InitializeLayout(null, null);
                    this.cameraTestMetricsUltraGridExcelExporter.Export(cameraTestLogUltraGrid, cameraTestMetricsSaveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void cameraTestMetricsUltraGridExcelExporter_BeginExport(object sender, Infragistics.Win.UltraWinGrid.ExcelExport.BeginExportEventArgs e)
        {
            try
            {
                e.CurrentWorksheet.Name = "Test Logs Grid Data";
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void cameraTestLogUltraGrid_InitializePrintPreview(object sender, CancelablePrintPreviewEventArgs e)
        {
            try
            {
                e.DefaultLogicalPageLayoutInfo.FitWidthToPages = 1;
                e.DefaultLogicalPageLayoutInfo.PageHeader = cameraTestLogUltraGrid.Text;
                e.DefaultLogicalPageLayoutInfo.PageHeaderAppearance.TextHAlign = HAlign.Center;
                e.DefaultLogicalPageLayoutInfo.PageHeaderBorderStyle = UIElementBorderStyle.Solid;
                e.DefaultLogicalPageLayoutInfo.PageFooter = "Page <#>.";
                e.DefaultLogicalPageLayoutInfo.PageFooterAppearance.TextHAlign = HAlign.Right;
                e.DefaultLogicalPageLayoutInfo.PageFooterBorderStyle = UIElementBorderStyle.Solid;
                e.DefaultLogicalPageLayoutInfo.ClippingOverride = ClippingOverride.Yes;
                e.PrintDocument.DefaultPageSettings.Landscape = true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void viewMetricsChart_Click(object sender, EventArgs e)
        {
            CameraTestMetricsChart cameraTestMetricsChart = new CameraTestMetricsChart(cameraTestLogsList);
            cameraTestMetricsChart.Show();
        }
        private async void SearchPatternRecords_Click(object sender, EventArgs e)
        {
            try
            {
                Application.UseWaitCursor = true;
                List<CameraTestLogsModel> cameraTestLogs = new List<CameraTestLogsModel>();
                List<CameraTestLogsModel> searchPatternLogsList = new List<CameraTestLogsModel>();
                cameraTestLogs = await cameraTestLogsPresenter.GetCameraTestLogs(string.Empty,string.Empty);
                string[] searchPatternCodes = searchPattern.Text.Split(new char[] { ',','\n',' ' });
                foreach(string patternCode in searchPatternCodes)
                {
                    foreach(CameraTestLogsModel cameraTestLogsModel in cameraTestLogs)
                    {
                        if (cameraTestLogsModel.RootCause.IndexOf(patternCode,0, StringComparison.CurrentCultureIgnoreCase)!=-1
                            || cameraTestLogsModel.Disposition.IndexOf(patternCode, 0, StringComparison.CurrentCultureIgnoreCase) != -1
                            || cameraTestLogsModel.Symptom.IndexOf(patternCode, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                        {                            
                            searchPatternLogsList.Add(cameraTestLogsModel);
                        }
                    }
                }
                if(searchPatternLogsList.Distinct().ToList().Count > 0)
                    cameraTestLogUltraGrid.DataSource = searchPatternLogsList.Distinct().ToList();
                Application.UseWaitCursor = false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                Application.UseWaitCursor = false;
            }
        }

        private async void imagerSNComboEditor_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (imagerSNComboEditor.SelectedItem != null)
                    cameraTestLogsList = await cameraTestLogsPresenter.GetCameraTestLogs(string.Empty,imagerSNComboEditor.SelectedItem.DisplayText);
                else
                    cameraTestLogsList = await cameraTestLogsPresenter.GetCameraTestLogs(string.Empty,string.Empty);
                if (cameraTestLogsList.Count() > 0)
                {
                    cameraTestLogUltraGrid.DataSource = cameraTestLogsList;
                    ultraGroupBoxCameraMetrics.Text = cameraSN.SelectedItem.DisplayText + " Metrics";
                    ultraGroupBoxCameraMetrics.Appearance.FontData.Bold = DefaultableBoolean.True;
                    await updateCameraTestMetricsView(cameraTestLogsList);
                }
                else
                {
                    MessageBox.Show("No camera test logs found", "Camera Test Logs", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    resetUI();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }

        private void cameraTestLogRootCause_LinkClicked(object sender, Infragistics.Win.FormattedLinkLabel.LinkClickedEventArgs e)
        {

        }       

        private async void AddAffectedItem_Click(object sender, EventArgs e)
        {
            bool containsPartNum = true;
            try
            {
                if(!string.IsNullOrWhiteSpace(NewAffectedItem.Text))
                {
                    List<AffectedItemsModel> newAffectedItemsModelList = new List<AffectedItemsModel>();
                    //string[] newAffectedItems = NewAffectedItem.Text.Split(new char[] { '\r\n' });
                    string[] newAffectedItems =  NewAffectedItem.Text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in newAffectedItems)
                    {
                        if(!item.Contains("-"))
                        {
                            containsPartNum = false;
                        }
                        string[] newItems = item.Split(new char[] { '-' });
                        AffectedItemsModel affectedItemsModel = new AffectedItemsModel();
                        affectedItemsModel.ItemName = newItems[0].Trim();
                        affectedItemsModel.PartNumber = newItems[1].Trim();
                        affectedItemsModel.UserDefined= string.IsNullOrWhiteSpace(TestAppHelper.UserLoginName) ? Environment.UserName : TestAppHelper.UserLoginName;
                        affectedItemsModel.DateDefined= DateTime.Now;
                        newAffectedItemsModelList.Add(affectedItemsModel);
                    }
                    if (await cameraTestLogsPresenter.addNewAffectedItemInDB(newAffectedItemsModelList))
                    {
                        MessageBox.Show("New affected item added successfully", "Add New Affected Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        GetAffectedItemsList();
                        NewAffectedItem.Text = string.Empty;
                    }
                    else
                        MessageBox.Show("Unknown error adding new affected item", "Add New Affected Item", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                else
                {
                    MessageBox.Show("New affected item cannot be empty", "Add New Affected Item", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (IndexOutOfRangeException ex)
            {
                if(!containsPartNum)
                {
                    MessageBox.Show("One of the affected item does not contain a item name or partnumber. " +
                                     "Please add an item in the format ItemName-PartNumber", "Add New Affected Item", 
                                     MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
