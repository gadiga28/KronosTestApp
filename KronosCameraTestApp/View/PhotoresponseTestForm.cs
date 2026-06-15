using Infragistics.Win.Misc;
using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.Presenter;
using KronosCameraTestApp.View.UserControls;
using NationalInstruments.Analysis.Math;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thermo.Kronos.Instrument.Camera.Interface;
namespace KronosCameraTestApp.View
{
    public partial class PhotoresponseTestForm : Form
    {
        public PhotoresponsePresenter photoresponsePresenter { get; set; }
        // The once-per-class call to initialize the log object
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		ListExposureData listExposureData { get; set; }
		private CIDInterface cidInterface = null;
        public int currentRecord = 0;
        public int currentSubarrayRecord = 0;
        private string userMode = string.Empty;
        bool engineeringLoginStatus = false;
        int initialExposureCount = 200;
        CancellationTokenSource cts { get; set; }
        ExposureSettings exposureSettings { get; set; }
        Form mdiParent = null;
        LEDCalibrationTest lEDCalibrationTest { get; set; }
        LEDCalibrationPresenter ledCalibrationPresenter;
        public LEDCalibrationPresenter LEDCalibrationPresenter
        {
            get { return ledCalibrationPresenter; }
            set { ledCalibrationPresenter = value; }
        }
        public CIDInterface CidInterface
        {
            get { return cidInterface; }
            set { cidInterface = value; }
        }
        bool photoresponseTestStatus = false;
        public bool PhotoresponseTestStatus
        {
            get { return photoresponseTestStatus; }
            set { photoresponseTestStatus = value; }
        }      
        public PhotoresponseTestForm()
        {
            InitializeComponent();
        }
        public PhotoresponseTestForm(PhotoresponsePresenter photoresponsePresenter, string UserMode, bool enggLoginStatus,
                                        CIDInterface cidInterface, Form MDIParent)
        {
            try
            {
                InitializeComponent();
                currentSubarrayRecord = 0;
                currentRecord = 0;
                this.photoresponsePresenter = photoresponsePresenter;
                this.userMode = UserMode;
                this.engineeringLoginStatus = enggLoginStatus;
                this.cidInterface = cidInterface;
                mdiParent = MDIParent;
            }          
            catch(Exception ex)
            {
                log.Error("Exception in"+ MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void PhotoresponseTestForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ((ToolStripMenuItem)GridLinesContextMneu.Items[0]).CheckedChanged += XAxisGridClick;
                ((ToolStripMenuItem)GridLinesContextMneu.Items[1]).CheckedChanged += YAxisGridClick;
                ((ToolStripMenuItem)GridLinesContextMneu.Items[2]).CheckedChanged += BothGridLinesClick;
                //((ToolStripMenuItem)GridLinesContextMneu.Items[3]).CheckedChanged += NoGridLinesClick;
                if (photoresponsePresenter.PhotoresponseLimitsDataList.Count == 0 || TestAppHelper.UserModeChanged)
                     await photoresponsePresenter.GetLimits();
                UpdateLimitsViewWithModelOnLoad();
                exposureSettings = new ExposureSettings("PRT", userMode, engineeringLoginStatus, TestAppHelper.UserLoginName, "PhotoresponseData.xml", "PhotoresponseExposures");
                ultraExplorerBarContainerControl2.Controls.Add(exposureSettings);
                exposureSettings.GetData();
                photoresponsePresenter.ExposureDataList = exposureSettings.ExposureList;
                photoresponsePresenter.SubarrayDataModelList = exposureSettings.SubarrayList;
                photoresponsePresenter.ExposureDataModel = exposureSettings.exposureDataPresenter.ExposureDataModel;
                photoresponsePresenter.SubarrayDataModel = exposureSettings.exposureDataPresenter.SubarrayDataModel;
                numericEditNumOfExposures.Value = initialExposureCount;
                if (userMode == "Manufacturing")
                {
                    EnableReadOnlyControlsManufacturingMode();
                }
                await ledCalibrationPresenter.GetLimits();
                LEDCalibTestInterval.Value = Convert.ToDecimal(ledCalibrationPresenter.LEDCalibrationLimitsDataList[0].CurrentTestInterval);
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Loading Photoresponse Test Form", "Loading Form", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void EnableReadOnlyControlsManufacturingMode()
        {
            try
            {
                NumExposuresGroupBox.Enabled = false;
                LimitsGroupBox.Enabled = false;
                enableLEDCalibration.Enabled = false;
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
                photoresponsePresenter.PhotoresponseLimitsData = photoresponsePresenter.PhotoresponseLimitsDataList.Where
                    (limit => limit.LimitsID.ToString().Equals(ultraComboEditorLimitIDs.SelectedItem.DisplayText.ToString())).FirstOrDefault();
                FullSaturationLimit.Value = Convert.ToDouble(photoresponsePresenter.PhotoresponseLimitsData.FullWellLevelMin);
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
                limtIDs = (from limits in photoresponsePresenter.PhotoresponseLimitsDataList select limits.LimitsID).ToList();
                ultraComboEditorLimitIDs.DataSource = limtIDs;
                ultraComboEditorLimitIDs.SelectedIndex = 0;
                photoresponsePresenter.PhotoresponseLimitsData = photoresponsePresenter.PhotoresponseLimitsDataList.
                    Where(limit => limit.LimitsID.Equals(limtIDs[0])).FirstOrDefault();
                if (photoresponsePresenter.PhotoresponseLimitsData != null)
                    FullSaturationLimit.Value = Convert.ToDouble(photoresponsePresenter.PhotoresponseLimitsData.FullWellLevelMin);
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
                photoresponsePresenter.PhotoresponseLimitsData = photoresponsePresenter.PhotoresponseLimitsDataList.FirstOrDefault
                    (limit => limit.LimitsID.Equals(Convert.ToInt32(ultraComboEditorLimitIDs.SelectedItem.DisplayText)));
                photoresponsePresenter.PhotoresponseLimitsData.FullWellLevelMin = Convert.ToInt32(FullSaturationLimit.Value);
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
        private async  void buttonPhotoresponseRun_Click(object sender, EventArgs e)
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
                    ((KronosTestAppMainForm)mdiParent).FirmwareManualTestLogsFile = "Photoresponse_ManualTestFWLog_" +
                        ((KronosTestAppMainForm)mdiParent).CameraSerialNumber.Trim('\0').TrimEnd() + "_" + DateTime.Now.ToString("yymmddThhmm") + ".txt";
                    // Instantiate the CancellationTokenSource.  
                    ledPassFail.Value = false;
                    ledPassFail.OffColor = Color.DarkGreen;
                    buttonPhotoresponseRun.Enabled = false;
                    buttonPhotoresponseAbort.Enabled = true;
                    SaturationResult.Text = string.Empty;
                    SaturationResult.Appearance.BorderColor = SaturationResult.Appearance.BorderColor2 = Color.Black;
                    SlopeResult.Text = string.Empty;
                    InterceptResult.Text = string.Empty;
                    LinearFullWellResult.Text = string.Empty;
                    photoResponseTestWaveformPlot.ClearData();
                    meanWaveformPlot.ClearData();
                    linearFitWaveformPlot.ClearData();
                    meanofROI1minus2Plot.ClearData();
                    linearCurveFitOfVarPlot.ClearData();
                    varianceOfROI1minus2Plot.ClearData();
                    redLEDPlot.ClearData();
                    greenLEDPlot.ClearData();
                    blueLEDPlot.ClearData();
                    photoResponseWaveformGraph.Refresh();
                    derivativeWaveformGraph.Refresh();                    
                    photoresponseTestStatus = true;
                    cts = new CancellationTokenSource();
                    DialogResult ledCalibAlert=DialogResult.Ignore;
                    if (enableLEDCalibration.Checked)
                    {
                        List<LEDCalibrationTestResults> lEDCalibrationTestResults = await ledCalibrationPresenter.GetLEDCalibrationResults(Environment.MachineName);
                        string captionText = string.Format("LED Calibration Test #: {0}", TestAppHelper.LEDCalibSavedCounter+1 );
                        ledCalibAlert = MessageBox.Show("LED Calibration is enabled. Do you wish to run calibration test?",
                                                        captionText, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (ledCalibAlert == DialogResult.Yes)
                        {
                            redLEDPlot.Visible = true;
                            greenLEDPlot.Visible = true;
                            blueLEDPlot.Visible = true;
                            LEDCalibGroupBox.Visible = true;
                            derivativeWaveformGraph.Visible = false;
                            PhotoResponseLegend.Visible = false;
                            xAxis1.MajorDivisions.GridVisible = true;
                            yAxis1.MajorDivisions.GridVisible = true;
                            red40Actual.Text = green40Actual.Text = blue40Actual.Text =
                            red60Actual.Text = green60Actual.Text = blue60Actual.Text = string.Empty;
                            red40Actual.Appearance.BorderColor = red40Actual.Appearance.BorderColor2 = Color.Black;
                            red60Actual.Appearance.BorderColor = red60Actual.Appearance.BorderColor2 = Color.Black;
                            green40Actual.Appearance.BorderColor = green40Actual.Appearance.BorderColor2 = Color.Black;
                            green60Actual.Appearance.BorderColor = green60Actual.Appearance.BorderColor2 = Color.Black;
                            blue40Actual.Appearance.BorderColor = blue40Actual.Appearance.BorderColor2 = Color.Black;
                            blue60Actual.Appearance.BorderColor = blue60Actual.Appearance.BorderColor2 = Color.Black;
                            TestAppHelper.currentRunningTest = "LED Claibration";
                            ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 1;
                            ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "LED Calibration Test Processing....";
                            ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Start();
                            lEDCalibrationTest = new LEDCalibrationTest(((KronosTestAppMainForm)mdiParent), cidInterface, ledCalibrationPresenter);
                            await lEDCalibrationTest.RunLEDCalibration();
                            PlotLEDCalibrationTestResults();
                            PlotLEDCalibrationLimits();
                            ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "LED Calibration Test Completed";
                            ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 100;
                            ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                        }
                    }
                    if (TestAppHelper.LEDCalibSavedCounter == Convert.ToInt32(LEDCalibTestInterval.Value))
                    {
                        UltraDesktopAlertShowWindowInfo windowInfo1 = new UltraDesktopAlertShowWindowInfo
                                                                    ("Auto LED Calibration Test.", "Running LED Calibration Test in Auto mode.");
                        if (File.Exists(Application.StartupPath.ToString() + "\\Resources\\CameraTempAlert.wav"))
                        {
                            windowInfo1.Sound = Application.StartupPath.ToString() + "\\Resources\\CameraTempAlert.wav";
                        }
                        windowInfo1.ScreenPosition = ScreenPosition.Center;
                        ultraDesktopAlert1.AutoClose = Infragistics.Win.DefaultableBoolean.False;
                       // ultraDesktopAlert1.AutoCloseDelay = 3000;
                        ultraDesktopAlert1.Show(windowInfo1);
                        //((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 1;
                        //((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "LED Calibration Test Processing....";
                        //((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Start();
                        await lEDCalibrationTest.RunLEDCalibration();
                        //PlotLEDCalibrationTestResults();
                        //PlotLEDCalibrationLimits();
                        //((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "LED Calibration Test Completed";
                        //((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 100;
                        //((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                        TestAppHelper.LEDCalibSavedCounter = 0;
                        //ultraDesktopAlert1.AutoClose = Infragistics.Win.DefaultableBoolean.True;
                        ultraDesktopAlert1.CloseAll();
                    }
                    if (ledCalibAlert == DialogResult.Ignore || ledCalibAlert == DialogResult.No)
                    {
                        enableLEDCalibration.Checked = false;
                        LEDCalibGroupBox.Visible = false;
                        derivativeWaveformGraph.Visible = true;
                        PhotoResponseLegend.Visible = true;
                        xAxis1.MajorDivisions.GridVisible = false;
                        yAxis1.MajorDivisions.GridVisible = false;
                        redLEDPlot.Visible=false;
                        greenLEDPlot.Visible = false;
                        blueLEDPlot.Visible = false;
                        TestAppHelper.currentRunningTest = "Photoresponse";
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 1;
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Photoresponse Test Processing....";
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Start();
                        photoresponsePresenter.ExposureDataList = exposureSettings.InitialExposureData();
                        //Run Photoresponse Test
                        await RunPhotoresponseTest(cts.Token);
                        //Plot Photoresponse Data
                        await PlotPhotoresponseData(cts);
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Photoresponse Test Completed";
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 100;
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                    }                   
                    
                    if (cts.IsCancellationRequested || !cidInterface.IsConnected())
                    {
                        if (!cidInterface.IsConnected())
                        {
                            TestAppHelper.noHeartBeatFromCamera();
                            cidInterface.AbortExposure();
                            cts.Cancel();
                            await ClearPhotoresponseGraphs();
                        }
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Photoresponse Test Aborted";
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.ResetFillAppearance();
                        ledPassFail.OffColor = Color.DarkGreen;
                    }                   
                    buttonPhotoresponseAbort.Enabled = false;
                    buttonPhotoresponseRun.Enabled = true;
                    photoresponseTestStatus = false;
                    photoresponseFormExplorerBar.Groups[1].Expanded = true;
                }

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
            }
        }              
        private void PlotLEDCalibrationTestResults()
        {
            try
            {
                redLEDPlot.PlotY(lEDCalibrationTest.RedMean);
                red40Actual.Text = lEDCalibrationTest.LEDCalibrationTestResults.Red40Actual.ToString();
                red60Actual.Text = lEDCalibrationTest.LEDCalibrationTestResults.Red60Actual.ToString();
                if (!lEDCalibrationTest.LEDCalibrationTestResults.Red40Passed)
                {
                    red40Actual.Appearance.BorderColor = red40Actual.Appearance.BorderColor2 = Color.Red;
                }
                else
                {
                    red40Actual.Appearance.BorderColor = red40Actual.Appearance.BorderColor2 = Color.LightGreen;
                }
                if (!lEDCalibrationTest.LEDCalibrationTestResults.Red60Passed)
                {
                    red60Actual.Appearance.BorderColor = red60Actual.Appearance.BorderColor2 = Color.Red;
                }
                else
                {
                    red60Actual.Appearance.BorderColor = red60Actual.Appearance.BorderColor2 = Color.LightGreen;
                }

                greenLEDPlot.PlotY(lEDCalibrationTest.GreenMean);
                green40Actual.Text = lEDCalibrationTest.LEDCalibrationTestResults.Green40Actual.ToString();
                green60Actual.Text = lEDCalibrationTest.LEDCalibrationTestResults.Green60Actual.ToString();
                if (!lEDCalibrationTest.LEDCalibrationTestResults.Green40Passed)
                {
                    green40Actual.Appearance.BorderColor = green40Actual.Appearance.BorderColor2 = Color.Red;
                }
                else
                {
                    green40Actual.Appearance.BorderColor = green40Actual.Appearance.BorderColor2 = Color.LightGreen;
                }
                if (!lEDCalibrationTest.LEDCalibrationTestResults.Green60Passed)
                {
                    green60Actual.Appearance.BorderColor = green60Actual.Appearance.BorderColor2 = Color.Red;
                }
                else
                {
                    green60Actual.Appearance.BorderColor = green60Actual.Appearance.BorderColor2 = Color.LightGreen;
                }

                blueLEDPlot.PlotY(lEDCalibrationTest.BlueMean);
                blue40Actual.Text = lEDCalibrationTest.LEDCalibrationTestResults.Blue40Actual.ToString();
                blue60Actual.Text = lEDCalibrationTest.LEDCalibrationTestResults.Blue60Actual.ToString();
                if (!lEDCalibrationTest.LEDCalibrationTestResults.Blue40Passed)
                {
                    blue40Actual.Appearance.BorderColor = blue40Actual.Appearance.BorderColor2 = Color.Red;
                }
                else
                {
                    blue40Actual.Appearance.BorderColor = blue40Actual.Appearance.BorderColor2 = Color.LightGreen;
                }
                if (!lEDCalibrationTest.LEDCalibrationTestResults.Blue60Passed)
                {
                    blue60Actual.Appearance.BorderColor = blue60Actual.Appearance.BorderColor2 = Color.Red;
                }
                else
                {
                    blue60Actual.Appearance.BorderColor = blue60Actual.Appearance.BorderColor2 = Color.LightGreen;
                }

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void PlotLEDCalibrationLimits()
        {
            try
            {
               
                Red40Limit.Text = ledCalibrationPresenter.LEDCalibrationLimitsDataList[0].Red40LEDLimit.ToString();
                Red60Limit.Text = ledCalibrationPresenter.LEDCalibrationLimitsDataList[0].Red60LEDLimit.ToString(); 
                Green40Limit.Text = ledCalibrationPresenter.LEDCalibrationLimitsDataList[0].Green40LEDLimit.ToString();
                Green60Limit.Text = ledCalibrationPresenter.LEDCalibrationLimitsDataList[0].Green60LEDLimit.ToString();
                Blue40Limit.Text = ledCalibrationPresenter.LEDCalibrationLimitsDataList[0].Blue40LEDLimit.ToString();
                Blue60Limit.Text = ledCalibrationPresenter.LEDCalibrationLimitsDataList[0].Blue60LEDLimit.ToString();               
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);                
            }
        }
        private async Task RunPhotoresponseTest(CancellationToken ct)
        {
            try
            {
                log.Debug("Run Photoresponse test.");
                if (ct.IsCancellationRequested)
                    return;
                await photoresponsePresenter.runPhotoresponseTest(cidInterface, ct, userMode, Convert.ToInt32(numericEditNumOfExposures.Value));
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task PlotPhotoresponseData(CancellationTokenSource ct)
        {
            try
            {
                const int NUMBER_OF_BEST_FIT = 122;
                const int START_BEST_FIT = 25;
                if (ct.IsCancellationRequested)
                    return;
                await photoresponsePresenter.CalculatePhotoresponse(ct.Token);
                if (ct.IsCancellationRequested)
                    return;
                if (!cidInterface.IsConnected())
                    return;
               
                if (photoresponsePresenter.XData != null && photoresponsePresenter.YData != null)
                {
                    var linearyFitData = CurveFit.LinearFit(photoresponsePresenter.XData, photoresponsePresenter.YData);
                    if (linearyFitData != null)
                        linearFitWaveformPlot.PlotY(linearyFitData, START_BEST_FIT, (NUMBER_OF_BEST_FIT - START_BEST_FIT), START_BEST_FIT, 1);
                    photoresponsePresenter.LinearFitData = linearyFitData;
                }
                if (photoresponsePresenter.MeanValue != null)
                    photoResponseWaveformGraph.PlotY(photoresponsePresenter.MeanValue);
                if (photoresponsePresenter.LinearCurveFit != null)
                    linearCurveFitOfVarPlot.PlotY(photoresponsePresenter.DerivPlot);
                SlopeResult.Text = photoresponsePresenter.Slope.ToString();
                InterceptResult.Text = photoresponsePresenter.Intercept.ToString();
                LinearFullWellResult.Text = photoresponsePresenter.LinearFullWell;
                SaturationResult.Text = photoresponsePresenter.FullWell.ToString();
                
                buttonPhotoresponseRun.Enabled = true;
                buttonPhotoresponseAbort.Enabled = false;
                if (photoresponsePresenter.PhotoresponseTestPassed)
                {
                    SaturationResult.Appearance.BorderColor = SaturationResult.Appearance.BorderColor2 = Color.Lime;
                    ledPassFail.Value = true;
                }                   
                else
                {
                    ledPassFail.Value = false;
                    ledPassFail.OffColor = SaturationResult.Appearance.BorderColor = SaturationResult.Appearance.BorderColor2 = Color.Red;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }      
        public async Task ClearPhotoresponseGraphs()
        {
            await Task.Run(() =>
            {
                photoResponseTestWaveformPlot.ClearData();
                meanWaveformPlot.ClearData();
                linearFitWaveformPlot.ClearData();
                meanofROI1minus2Plot.ClearData();
                linearCurveFitOfVarPlot.ClearData();
                varianceOfROI1minus2Plot.ClearData();
            });
        }      
        private async void buttonPhotoresponseAbort_Click(object sender, EventArgs e)
        {
            try
            {
                if (cts != null)
                {
                    DialogResult result = MessageBox.Show("Are you sure to Abort Photoresponse Test?",
                           "Abort Photoresponse Test",
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        cidInterface.AbortExposure();
                        photoresponseTestStatus = false;
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.ResetFillAppearance();
                        await ClearPhotoresponseGraphs();
                        cts.Cancel();
                        buttonPhotoresponseRun.Enabled = true;
                    }
                    else
                        return;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.ResetFillAppearance();
                buttonPhotoresponseRun.Enabled = true;
                cts.Cancel();
            }
        }
        private void PhotoresponseTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
        private void numericEditNumOfExposures_ValueChanged(object sender, EventArgs e)
        {
        }
        private void ResetProgressBar()
        {
            try
            {
                if (((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value == 100)
                {
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = string.Empty;
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.ResetFillAppearance();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraExplorerBar1_Click(object sender, EventArgs e)
        {
            ResetProgressBar();
        }
        private void panel3_Click(object sender, EventArgs e)
        {
            ResetProgressBar();
        }       
        private async void buttonRefreshLimit_Click(object sender, EventArgs e)
        {
            await photoresponsePresenter.GetLimits();
            UpdateLimitsViewWithModelOnLoad();
        }
        private void photoResponseWaveformGraph_PlotAreaMouseDown(object sender, MouseEventArgs e)
        {

        }
        private void GridLinesContextMneu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                ToolStripItem item = e.ClickedItem;
                if (item.Name.Equals("XAxisGrid"))
                {
                    XAxisGridClick(sender,e);
                }
                if (item.Name.Equals("YAxisGrid"))
                {
                    XAxisGridClick(sender, e);
                }
                if (item.Name.Equals("BothGridLines"))
                {
                    BothGridLinesClick(sender, e);
                   
                }
                
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void XAxisGridClick(object sender, EventArgs e)
        {
            try
            {
                if(((ToolStripMenuItem)GridLinesContextMneu.Items[0]).Checked)
                {
                    xAxis1.MajorDivisions.GridVisible = true;
                    if (((ToolStripMenuItem)GridLinesContextMneu.Items[1]).Checked)
                        ((ToolStripMenuItem)GridLinesContextMneu.Items[2]).Checked = true;
                }
                else
                {
                    xAxis1.MajorDivisions.GridVisible = false;
                    ((ToolStripMenuItem)GridLinesContextMneu.Items[2]).Checked = false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void YAxisGridClick(object sender, EventArgs e)
        {
            try
            {
                if (((ToolStripMenuItem)GridLinesContextMneu.Items[1]).Checked)
                {
                    yAxis1.MajorDivisions.GridVisible = true;
                    if (((ToolStripMenuItem)GridLinesContextMneu.Items[0]).Checked)
                        ((ToolStripMenuItem)GridLinesContextMneu.Items[2]).Checked = true;
                }
                else
                {
                    yAxis1.MajorDivisions.GridVisible = false;
                    ((ToolStripMenuItem)GridLinesContextMneu.Items[2]).Checked = false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void BothGridLinesClick(object sender, EventArgs e)
        {
            try
            {
                if (((ToolStripMenuItem)GridLinesContextMneu.Items[2]).Checked)
                {
                    xAxis1.MajorDivisions.GridVisible = true;
                    yAxis1.MajorDivisions.GridVisible = true;
                    ((ToolStripMenuItem)GridLinesContextMneu.Items[0]).Checked = true;
                    ((ToolStripMenuItem)GridLinesContextMneu.Items[1]).Checked = true;
                }
                else
                {
                    xAxis1.MajorDivisions.GridVisible = false;
                    yAxis1.MajorDivisions.GridVisible = false;
                    ((ToolStripMenuItem)GridLinesContextMneu.Items[0]).Checked = false;
                    ((ToolStripMenuItem)GridLinesContextMneu.Items[1]).Checked = false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void enableLEDCalibration_CheckedChanged(object sender, EventArgs e)
        {
            if (enableLEDCalibration.Checked)
            {
                ultraExplorerBarContainerControl2.Enabled = false;
                LEDCalibTestInterval.Enabled = true;
            }
            else
            {
                ultraExplorerBarContainerControl2.Enabled = true;              
            }                
        }

        private void ECONumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void LEDCalibCounterConfig_ValueChanged(object sender, EventArgs e)
        {
            TestAppHelper.LEDCalibIntervalConfig=Convert.ToInt32(LEDCalibTestInterval.Value);
            TestAppHelper.LEDCalibSavedCounter = 0;
        }
    }
}
