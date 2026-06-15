using Infragistics.Win.Misc;
using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.Presenter;
using KronosCameraTestApp.View.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thermo.Kronos.Instrument.Camera.Interface;
namespace KronosCameraTestApp.View
{
    public partial class DarkCurrentTestForm : Form
    {
        public DarkCurrentPresenter darkCurrentPresenter { get; set; }
        // The once-per-class call to initialize the log object
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);      
        private CIDInterface cidInterface = null;
        private string userMode = string.Empty;
        bool engineeringLoginStatus = false;
        CancellationTokenSource cts { get; set; }
        ExposureSettings exposureSettings { get; set; }
        public CIDInterface CidInterface
        {
            get { return cidInterface; }
            set { cidInterface = value; }
        }
        bool darkCurrentTestStatus = false;
        public bool DarkCurrentTestStatus
        {
            get { return darkCurrentTestStatus; }
            set { darkCurrentTestStatus = value; }
        }
        Form mdiParent = null;
        public DarkCurrentTestForm()
        {
            InitializeComponent();
        }
        public DarkCurrentTestForm(DarkCurrentPresenter darkCurrentPresenter, string UserMode, bool enggLoginStatus,
            CIDInterface cidInterface, Form MDIParent)
        {
            try
            {
                InitializeComponent();
                this.darkCurrentPresenter = darkCurrentPresenter;
                this.userMode = UserMode;
                this.engineeringLoginStatus = enggLoginStatus;
                this.cidInterface = cidInterface;
                mdiParent = MDIParent;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void DarkCurrentTestForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (darkCurrentPresenter.DarkCurrentTestLimitsDataList.Count == 0 || TestAppHelper.UserModeChanged)
                {
                    await darkCurrentPresenter.GetDarkCurrentLimits();
                }
                UpdateLimitsViewWithModelOnLoad();
                exposureSettings = new ExposureSettings("DDT", userMode, engineeringLoginStatus, TestAppHelper.UserLoginName, "DarkCurrentData.xml", "DarkCurrentExposures");
                ultraExplorerBarContainerControl3.Controls.Add(exposureSettings);
                exposureSettings.GetData();
                darkCurrentPresenter.ExposureDataList = exposureSettings.ExposureList;
                darkCurrentPresenter.SubarrayDataModelList = exposureSettings.SubarrayList;
                darkCurrentPresenter.ExposureDataModel = exposureSettings.exposureDataPresenter.ExposureDataModel;
                darkCurrentPresenter.SubarrayDataModel = exposureSettings.exposureDataPresenter.SubarrayDataModel;
                numericEditNumOfExposures.Value = darkCurrentPresenter.ExposureDataList.Count;
                if (userMode == "Manufacturing")
                {
                    EnableReadOnlyControlsManufacturingMode();
                }
                this.Cursor = Cursors.Arrow;                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Loading Dark Current and Defects Test Form", "Loading Form", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void EnableReadOnlyControlsManufacturingMode()
        {
            try
            {
                NumExposuresGroupBox.Enabled = false;
                LimitsGroupBox.Enabled = false;               
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraComboEditorLimitIDs_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                darkCurrentPresenter.DarkCurrentTestLimitsData = darkCurrentPresenter.DarkCurrentTestLimitsDataList.Where(limit => limit.LimitsID.ToString().Equals(ultraComboEditorLimitIDs.SelectedItem.DisplayText.ToString())).FirstOrDefault();
                MaxDarkROILimit.Value = Convert.ToDouble(darkCurrentPresenter.DarkCurrentTestLimitsData.DarkCurrentMaxDarkROI);
                MaxDarkROILowerLimit.Value = Convert.ToDouble(darkCurrentPresenter.DarkCurrentTestLimitsData.DarkCurrentLowerLimit);
                MaxDarkROIUpperLimit.Value = Convert.ToDouble(darkCurrentPresenter.DarkCurrentTestLimitsData.DarkCurrentUpperLimit);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateLimitsViewWithModelOnLoad()
        {
            try
            {
                List<int> limtIDs = new List<int>();
                limtIDs = (from limits in darkCurrentPresenter.DarkCurrentTestLimitsDataList select limits.LimitsID).ToList();
                ultraComboEditorLimitIDs.DataSource = limtIDs;
                ultraComboEditorLimitIDs.SelectedIndex = 0;
                darkCurrentPresenter.DarkCurrentTestLimitsData = darkCurrentPresenter.DarkCurrentTestLimitsDataList.Where(limit => limit.LimitsID.Equals(limtIDs[0])).FirstOrDefault();
                if (darkCurrentPresenter.DarkCurrentTestLimitsData != null)
                {
                    MaxDarkROILimit.Value = Convert.ToDouble(darkCurrentPresenter.DarkCurrentTestLimitsData.DarkCurrentMaxDarkROI);
                    MaxDarkROILowerLimit.Value = Convert.ToDouble(darkCurrentPresenter.DarkCurrentTestLimitsData.DarkCurrentLowerLimit);
                    MaxDarkROIUpperLimit.Value = Convert.ToDouble(darkCurrentPresenter.DarkCurrentTestLimitsData.DarkCurrentUpperLimit);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateLimitsModelWithView()
        {
            try
            {
                darkCurrentPresenter.DarkCurrentTestLimitsData = darkCurrentPresenter.DarkCurrentTestLimitsDataList.FirstOrDefault(
                        limit => limit.LimitsID.Equals(Convert.ToInt32(ultraComboEditorLimitIDs.SelectedItem.DisplayText)));              
                darkCurrentPresenter.DarkCurrentTestLimitsData.DarkCurrentMaxDarkROI = MaxDarkROILimit.Value;
                darkCurrentPresenter.DarkCurrentTestLimitsData.DarkCurrentLowerLimit = MaxDarkROILowerLimit.Value;
                darkCurrentPresenter.DarkCurrentTestLimitsData.DarkCurrentUpperLimit = MaxDarkROIUpperLimit.Value;                
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void buttonUpdateLimit_Click(object sender, EventArgs e)
        {
            UpdateLimitsModelWithView();
        }
        private async void buttonDarkCurrentRun_Click(object sender, EventArgs e)
        {
            try
            {
                if (((KronosTestAppMainForm)mdiParent).AutoTestStatus)
                {
                    MessageBox.Show("Auto Test is under process. Please wait until the test is completed");
                    return;
                }
                if (((KronosTestAppMainForm)mdiParent).AutoPreTestStatus)
                {
                    MessageBox.Show("PreTest is under process. Please wait until the test is completed");
                    return;
                }
                else if (cidInterface.IsConnected())
                {
                    if (Convert.ToInt32(numericEditNumOfExposures.Value) == 0 || Convert.ToInt32(numericEditNumOfExposures.Value) < 0)
                    {
                        MessageBox.Show("Exposure count should be greater than 0", "Invalid exposure count", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                    ((KronosTestAppMainForm)mdiParent).FirmwareManualTestLogsFile = "DarkCurrent_ManualTestFWLog_" +
                        ((KronosTestAppMainForm)mdiParent).CameraSerialNumber.Trim('\0').TrimEnd() + "_" + DateTime.Now.ToString("yymmddThhmm") + ".txt";
                    ledPassFail.Value = false;
                    ledPassFail.OffColor = Color.DarkGreen;
                    MaxDarkROI.Text = string.Empty;
                    MaxDarkROI.Appearance.BorderColor = MaxDarkROI.Appearance.BorderColor2 = Color.Black;
                    labelAveDarkFF.Text = string.Empty;
                    labelMaxDarkFF.Text = string.Empty;
                    MaxTrapResult.Text = string.Empty;
                    labelAveTrap.Text = string.Empty;
                    cts = new CancellationTokenSource();
                    TestAppHelper.currentRunningTest = "DarkCurrent";
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 1;
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Dark Current Test Processing....";
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Start();
                    buttonDarkCurrentRun.Enabled = false;
                    buttonDarkCurrentAbort.Enabled = true;
                    this.TheoryPlot.ClearData();
                    this.MeasuredNoisePlot.ClearData();
                    this.darkCurrentWaveformGraph.Refresh();
                    darkCurrentTestStatus = true;
                    darkCurrentPresenter.ExposureDataList = exposureSettings.ExposureList;
                    darkCurrentPresenter.SubarrayDataModelList = exposureSettings.SubarrayList;
                    darkCurrentPresenter.ExposureDataModel = exposureSettings.exposureDataPresenter.ExposureDataModel;
                    darkCurrentPresenter.SubarrayDataModel = exposureSettings.exposureDataPresenter.SubarrayDataModel;
                    RunDarkCurrentTest(cts.Token);
                    await PlotDarkCurrentData(cts);                 
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                    if (cts.IsCancellationRequested || !cidInterface.IsConnected())
                    {
                        if (!cidInterface.IsConnected())
                        {
                            TestAppHelper.noHeartBeatFromCamera();
                            cidInterface.AbortExposure();
                            cts.Cancel();
                            await ClearDarkCurrentGraphs();
                        }
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Dark Current Test Aborted";
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.ResetFillAppearance();
                        ledPassFail.OffColor = Color.DarkGreen;
                    }
                    else
                    {
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Dark Current Test Completed";
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 100;
                    }
                    buttonDarkCurrentAbort.Enabled = false;
                    buttonDarkCurrentRun.Enabled = true;
                    darkCurrentTestStatus = false;
                }
            }
            catch (Exception ex)
            {              
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void RunDarkCurrentTest(CancellationToken ct)
        {
            try
            {
                log.Debug("Run Mean Variance test.");
                darkCurrentPresenter.runDarkCurrentTest(cidInterface, ct, userMode, 1);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task PlotDarkCurrentData(CancellationTokenSource ct)
        {
            try
            {
                if (ct.IsCancellationRequested)
                    return;
                await darkCurrentPresenter.CalculateDarkCurrent(ct.Token);
                if (ct.IsCancellationRequested)
                    return;
                if (!cidInterface.IsConnected())
                    return;
                if (darkCurrentPresenter.DarkCurrentMeanValue != null)
                    darkCurrentWaveformGraph.PlotY(darkCurrentPresenter.DarkCurrentMeanValue);
                MaxDarkROI.Text = String.Format("{0:0.00}", darkCurrentPresenter.MaxDarkROI);
                if (darkCurrentPresenter.DarkCurrentTestPassed)
                {
                    ledPassFail.Value = true;
                    MaxDarkROI.Appearance.BorderColor = MaxDarkROI.Appearance.BorderColor2= Color.LimeGreen;
                }                   
                else
                {
                    ledPassFail.Value = false;
                    ledPassFail.OffColor = MaxDarkROI.Appearance.BorderColor = MaxDarkROI.Appearance.BorderColor2 = Color.Red;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void buttonDarkCurrentAbort_Click(object sender, EventArgs e)
        {
            try
            {
                if (cts != null)
                {
                    DialogResult result = MessageBox.Show("Are you sure to Abort Dark Current & Defects Test?",
                           "Abort Dark Current & Defects Test",
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        cidInterface.AbortExposure();
                        darkCurrentTestStatus = false;
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                        await ClearDarkCurrentGraphs();
                        cts.Cancel();
                        buttonDarkCurrentRun.Enabled = true;
                    }
                    else
                        return;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }       
        public async Task ClearDarkCurrentGraphs()
        {
            await Task.Run(() =>
            {
                this.TheoryPlot.ClearData();
                this.MeasuredNoisePlot.ClearData();
            });
        }
        private void DarkCurrentTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
        private void numericEditNumOfExposures_ValueChanged(object sender, EventArgs e)
        {
            if (numericEditNumOfExposures.Value > darkCurrentPresenter.ExposureDataList.Count)
            {
                string errstring = string.Format("Value cannot be greater than total exposure count '{0}'.", darkCurrentPresenter.ExposureDataList.Count);
                MessageBox.Show(errstring, "Invalid exposure count", MessageBoxButtons.OK, MessageBoxIcon.Error);
                numericEditNumOfExposures.Value = darkCurrentPresenter.ExposureDataList.Count;
            }
        }
        private void ResetProgressBar()
        {
            try
            {
                if (((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value == 100)
                {
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = string.Empty;
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void buttonRefreshLimit_Click(object sender, EventArgs e)
        {
            await darkCurrentPresenter.GetDarkCurrentLimits();
            UpdateLimitsViewWithModelOnLoad();
        }
        private void panel4_Click(object sender, EventArgs e)
        {
            ResetProgressBar();
        }
    }
}
