using Infragistics.Win.Misc;
using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.Presenter;
using KronosCameraTestApp.View.UserControls;
using LabJack.LabJackUD;
using NationalInstruments.UI;
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
    public partial class RedBlueTestForm : Form
    {
        public RedBluePresenter redBluePresenter { get; set; }
        // The once-per-class call to initialize the log object
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CIDInterface cidInterface = null;
        public int currentRecord = 0;
        private string userMode = string.Empty;
        bool engineeringLoginStatus = false;
        CancellationTokenSource cts { get; set; }
        public CIDInterface CidInterface
        {
            get { return cidInterface; }
            set { cidInterface = value; }
        }
        bool redBlueTestStatus = false;
        private static uint xArraySize = 2048;
        private static uint yArraySize = 2048;
        private bool maskEnabled = false;
        private bool contrastSlideResize = false;
        private int zoomPanIndex = 0;
        private float zoomFactor = 1.5F;
        private bool zooming = false;
        Form mdiParent = null;
		ListExposureData listExposureData { get; set; }
		public bool RedBlueTestStatus
        {
            get { return redBlueTestStatus; }
            set { redBlueTestStatus = value; }
        }
        LEDCalibrationPresenter ledCalibrationPresenter;
        public LEDCalibrationPresenter LEDCalibrationPresenter
        {
            get { return ledCalibrationPresenter; }
            set { ledCalibrationPresenter = value; }
        }
        List<LEDCalibrationLimitsData> ledCalibrationLimitsDataList { get; set; }
        LEDCalibrationLimitsData ledCalibrationLimitsData { get; set; }
        public RedBlueTestForm()
        {
            InitializeComponent();
        }
        public RedBlueTestForm(RedBluePresenter redBluePresenter, string UserMode, bool enggLoginStatus,
            CIDInterface cidInterface, Form MDIParent)
        {
            try
            {
                InitializeComponent();
                currentRecord = 0;
                this.redBluePresenter = redBluePresenter;
                this.userMode = UserMode;
                this.engineeringLoginStatus = enggLoginStatus;
                this.cidInterface = cidInterface;
                mdiParent = MDIParent;
                intensityCursor1.LabelVisible = true;
                intensityCursor2.LabelVisible = true;
                intensityCursor3.LabelVisible = true;
            }          
            catch(Exception ex)
            {
                log.Error("Exception in"+ MethodBase.GetCurrentMethod(), ex);
            }
        }
        ExposureSettings exposureSettings { get; set; }
        private async void RedBlueTestForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                exposureSettings = new ExposureSettings("RBT", userMode, engineeringLoginStatus, TestAppHelper.UserLoginName, "RedBlueData.xml", "RedBlueExposures");
                ultraExplorerBarContainerControl2.Controls.Add(exposureSettings);
                exposureSettings.GetData();
                redBluePresenter.ExposureDataList = exposureSettings.ExposureList;
                redBluePresenter.SubarrayDataModelList = exposureSettings.SubarrayList;
                redBluePresenter.ExposureDataModel = exposureSettings.exposureDataPresenter.ExposureDataModel;
                numericEditNumOfExposures.Value = redBluePresenter.ExposureDataList.Count;
                if (redBluePresenter.LEDCalibrationLimitsDataList.Count == 0 || TestAppHelper.UserModeChanged)
                    await redBluePresenter.GetLEDCalibrationLimits();
                UpdateLimitsViewWithModelOnLoad();
                if (userMode == "Manufacturing")
                {
                    EnableReadOnlyControlsManufacturingMode();
                }
                
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Loading Red&Blue Test Form", "Loading Form", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }      
        private void EnableReadOnlyControlsManufacturingMode()
        {
            try
            {
                NumExposuresGroupBox.Enabled = uvTestGroupBox.Enabled=false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void RedBlueTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
        private async void buttonRedBlueRun_Click(object sender, EventArgs e)
        {
            try
            {
                bool labjacknotfound = false;
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
                    ((KronosTestAppMainForm)mdiParent).FirmwareManualTestLogsFile = "RedBlue_ManualTestFWLog_" + 
                        ((KronosTestAppMainForm)mdiParent).CameraSerialNumber.Trim('\0').TrimEnd() + "_" + DateTime.Now.ToString("yymmddThhmm") + ".txt";
                    labelUVMean.Appearance.BorderColor2 = labelUVMean.Appearance.BorderColor = Color.Black;
                    labelUVMean.Text = string.Empty;

                    lblRedLedAvgSignal.Appearance.BorderColor2 = lblRedLedAvgSignal.Appearance.BorderColor = Color.Black;
                    lblRedLedAvgSignal.Text = string.Empty;

                    lblBlueLedAvgSignal.Appearance.BorderColor2 = lblBlueLedAvgSignal.Appearance.BorderColor = Color.Black;
                    lblBlueLedAvgSignal.Text = string.Empty;

                    ledPassFail.Value = false;
                    ledPassFail.OffColor = Color.DarkGreen;
                    // Instantiate the CancellationTokenSource.  
                    cts = new CancellationTokenSource();
                    TestAppHelper.currentRunningTest = "RedBlue";
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 1;
                    if (enableUVTest.Checked)
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Red, Blue & UV Test Processing....";
                    else
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Red & Blue Test Processing....";
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Start();
                    buttonRedBlueRun.Enabled = false;
                    buttonRedBlueAbort.Enabled = true;
                    this.intensityPlot1.ClearData();
                    this.intensityPlot2.ClearData();
                    this.intensityPlot3.ClearData();
                    redBlueIntensityGraph.Refresh();
                    blueIntensityGraph.Refresh();
                    differenceIntensityGraph.Refresh();
                    redBlueTestStatus = true;
                    redBluePresenter.ExposureDataList = exposureSettings.ExposureList.Take(2).ToList();
                   redBluePresenter.SubarrayDataModelList = exposureSettings.SubarrayList.Take(2).ToList();


                    redBluePresenter.ExposureDataModel = exposureSettings.exposureDataPresenter.ExposureDataModel;
                    redBluePresenter.UVInterval = Convert.ToInt32(uvInterval.Value);
                    //Run RedBlue Test
                    await RunRedBlueTest(cts.Token);
                    //Plot Red&Blue Data
                    await PlotRedBlueData(cts);
                    if(enableUVTest.Checked)
                    {
                        try
                        {
                            U3 u3; long resolution = 0;
                            u3 = new U3(LJUD.CONNECTION.USB, "0", true); // Connection through USB
                                                                         //Configure resolution. See section 2.6/3.1 of the User's Guide.
                            LJUD.ePut(u3.ljhandle, LJUD.IO.PUT_CONFIG, LJUD.CHANNEL.AIN_RESOLUTION, resolution, 0);
                        }
                        catch (LabJackUDException ex)
                        {
                            log.Error("LabJack open error" + MethodBase.GetCurrentMethod(), ex);
                            labjacknotfound = true;
                            ((KronosTestAppMainForm)mdiParent).mainFormDockManager.DockAreas[1].Closed = false;
                            ((KronosTestAppMainForm)mdiParent).mainFormDockManager.DockAreas[1].Activate();
                            ((KronosTestAppMainForm)mdiParent).mainFormDockManager.DockAreas[1].Pin();
                            ((KronosTestAppMainForm)mdiParent).firmwareLoggingTextBox.AppendText(Environment.NewLine);
                            ((KronosTestAppMainForm)mdiParent).firmwareLoggingTextBox.AppendText("LabJack open error UV Test maybe impacted");
                            ((KronosTestAppMainForm)mdiParent).firmwareLoggingTextBox.AppendText(Environment.NewLine);
                            ledPassFail.Value = false;
                            ledPassFail.OffColor = Color.Red;
                            MessageBox.Show("LabJack open error - " + " UV Test maybe impacted",
                                 "LabJack Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            // return;
                        }
                        if (!labjacknotfound)
                        {
                            Thread.Sleep(5000);

                            redBluePresenter.ExposureDataList = exposureSettings.ExposureList.Skip(2).ToList();
                            redBluePresenter.SubarrayDataModelList = exposureSettings.SubarrayList.Skip(2).ToList();

                            await RunUVTest(cts.Token);

                            await PlotUVData(cts);
                            ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                            if (cts.IsCancellationRequested || !cidInterface.IsConnected())
                            {
                                if (!cidInterface.IsConnected())
                                {
                                    TestAppHelper.noHeartBeatFromCamera();
                                    cidInterface.AbortExposure();
                                    cts.Cancel();
                                    await ClearRedBlueGraphs();
                                    labelUVMean.Text = string.Empty;
                                }
                                ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Red&Blue Test Aborted";
                                ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                                ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.ResetFillAppearance();
                                ledPassFail.OffColor = Color.DarkGreen;
                            }
                            else
                            {
                                ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Red, Blue && UV Test Completed";
                                ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 100;
                            }
                            buttonRedBlueAbort.Enabled = false;
                            buttonRedBlueRun.Enabled = true;
                            redBlueTestStatus = false;
                            redBlueFormExplorerBar.Groups[1].Expanded = true;
                        }
                        else
                        {
                            ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                            if (cts.IsCancellationRequested || !cidInterface.IsConnected())
                            {
                                if (!cidInterface.IsConnected())
                                {
                                    TestAppHelper.noHeartBeatFromCamera();
                                    cidInterface.AbortExposure();
                                    cts.Cancel();
                                    await ClearRedBlueGraphs();
                                    labelUVMean.Text = string.Empty;
                                }
                                ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Red, Blue & UV Test Aborted";
                                ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                                ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.ResetFillAppearance();
                                ledPassFail.OffColor = Color.DarkGreen;
                            }
                            else
                            {
                                ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Red, Blue & UV Test Completed";
                                ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 100;
                            }
                            buttonRedBlueAbort.Enabled = false;
                            buttonRedBlueRun.Enabled = true;
                            redBlueTestStatus = false;
                            redBlueFormExplorerBar.Groups[1].Expanded = true;
                            return;
                        }
                    }
                    else
                    {
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                        if (cts.IsCancellationRequested || !cidInterface.IsConnected())
                        {
                            if (!cidInterface.IsConnected())
                            {
                                TestAppHelper.noHeartBeatFromCamera();
                                cidInterface.AbortExposure();
                                cts.Cancel();
                                await ClearRedBlueGraphs();
                                labelUVMean.Text = string.Empty;
                            }
                            ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Red&Blue Test Aborted";
                            ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                            ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.ResetFillAppearance();
                            ledPassFail.OffColor = Color.DarkGreen;
                        }
                        else
                        {
                            ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Red & Blue Test Completed";
                            ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 100;
                        }
                        buttonRedBlueAbort.Enabled = false;
                        buttonRedBlueRun.Enabled = true;
                        redBlueTestStatus = false;
                        redBlueFormExplorerBar.Groups[1].Expanded = true;
                        return;
                    }


                }
            }
            catch (Exception ex)
            {
                buttonRedBlueAbort.Enabled = false;
                buttonRedBlueRun.Enabled = true;
                redBlueTestStatus = false;
                redBlueFormExplorerBar.Groups[1].Expanded = true;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task RunRedBlueTest(CancellationToken ct)
        {
            try
            {
                log.Debug("Run Red&Blue test.");
                if (ct.IsCancellationRequested)
                    return;
                await redBluePresenter.runRedBlueTest(cidInterface, ct, userMode, 2);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                buttonRedBlueAbort.Enabled = false;
                buttonRedBlueRun.Enabled = true;
                redBlueTestStatus = false;
                redBlueFormExplorerBar.Groups[1].Expanded = true;
            }
        }
        private async  Task RunUVTest(CancellationToken ct)
        {
            try
            {
                log.Debug("Run Red&Blue test.");
                if (ct.IsCancellationRequested)
                    return;
                await redBluePresenter.runUVTest(cidInterface, ct, userMode, 1);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                buttonRedBlueAbort.Enabled = false;
                buttonRedBlueRun.Enabled = true;
                redBlueTestStatus = false;
                redBlueFormExplorerBar.Groups[1].Expanded = true;
            }
        }
        private async Task PlotRedBlueData(CancellationTokenSource ct)
        {
            try
            {
                if (ct.IsCancellationRequested)
                    return;
                await redBluePresenter.CalculateRedBlue(ct.Token);
                await redBluePresenter.calculateRedBlueDiff();
                if (ct.IsCancellationRequested)
                    return;
                if (!cidInterface.IsConnected())
                    return;
                PlotRedImageData();
                PlotBlueImageData();
                PlotRedBlueDiffImageData();
               
               
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                buttonRedBlueAbort.Enabled = false;
                buttonRedBlueRun.Enabled = true;
                redBlueTestStatus = false;
                redBlueFormExplorerBar.Groups[1].Expanded = true;
            }
        }
        private async Task PlotUVData(CancellationTokenSource ct)
        {
            try
            {
                if (ct.IsCancellationRequested)
                    return;
                await redBluePresenter.CalculateUVTest(ct.Token);
                labelUVMean.Text = redBluePresenter.UVMean.ToString("F1");
                if (redBluePresenter.UVTestPassed)
                {
                    labelUVMean.Appearance.BorderColor2 = labelUVMean.Appearance.BorderColor = Color.LimeGreen;
                }
                else
                {
                    labelUVMean.Appearance.BorderColor2 = labelUVMean.Appearance.BorderColor = Color.Red;
                    ledPassFail.Value = false;
                    ledPassFail.OffColor = Color.Red;
                }
                buttonRedBlueRun.Enabled = true;
                buttonRedBlueAbort.Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                buttonRedBlueAbort.Enabled = false;
                buttonRedBlueRun.Enabled = true;
                redBlueTestStatus = false;
                redBlueFormExplorerBar.Groups[1].Expanded = true;
            }
        }
        private void PlotRedImageData()
        {
            try
            {
                if (redBluePresenter.ZDataRedDouble != null)
                {
                    redBlueIntensityGraph.ClearData();
                    // Plot the red led image 
                    redBlueIntensityGraph.Plot(redBluePresenter.ZDataRedDouble);
                    setRedGraphContrast(redBluePresenter.ZDataRedDouble);
                    redBlueIntensityGraph_AfterMoveCursor(null, null);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                buttonRedBlueAbort.Enabled = false;
                buttonRedBlueRun.Enabled = true;
                redBlueTestStatus = false;
                redBlueFormExplorerBar.Groups[1].Expanded = true;
            }
        }
        private void PlotBlueImageData()
        {
            try
            {
                if (redBluePresenter.ZDataBlueDouble != null)
                {
                    blueIntensityGraph.ClearData();
                    // Plot the blue led image 
                    blueIntensityGraph.Plot(redBluePresenter.ZDataBlueDouble);
                    setBlueGraphContrast(redBluePresenter.ZDataBlueDouble);
                    BlueIntensityGraph_AfterMoveCursor(null, null);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                buttonRedBlueAbort.Enabled = false;
                buttonRedBlueRun.Enabled = true;
                redBlueTestStatus = false;
                redBlueFormExplorerBar.Groups[1].Expanded = true;
            }
        }
        private void PlotRedBlueDiffImageData()
        {
            try
            {
                if ((redBluePresenter.ZDataRedDouble != null) && (redBluePresenter.ZDataBlueDouble != null))
                {
                    differenceIntensityGraph.ClearData();
                    differenceIntensityGraph.Plot(redBluePresenter.ZDataDifference);
                    setDifferenceGraphContrast(redBluePresenter.ZDataDifference);
                    differenceIntensityGraph_AfterMoveCursor(null, null);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                buttonRedBlueAbort.Enabled = false;
                buttonRedBlueRun.Enabled = true;
                redBlueTestStatus = false;
                redBlueFormExplorerBar.Groups[1].Expanded = true;
            }
        }       
        private void setRedGraphContrast(double[,] plotData)
        {
            double maximumSignal = 0;
            double minimumSignal = 65000;
            for (int x = 0; x < xArraySize; x++)
            {
                for (int y = 0; y < yArraySize; y++)
                {
                    if (plotData[x, y] > maximumSignal)
                    {
                        maximumSignal = plotData[x, y];
                    }
                    if ((plotData[x, y] < minimumSignal) && (plotData[x, y] != 0))
                    {
                        minimumSignal = plotData[x, y];
                    }
                }
            }
            if ((maximumSignal > 0) && (maximumSignal > minimumSignal))
            {               
                this.intensityGraphColorScale.Range =
                    new NationalInstruments.UI.Range(minimumSignal, maximumSignal);
            }
            double redAvgSignal = maximumSignal + minimumSignal / 2;
            lblRedLedAvgSignal.Text = redAvgSignal.ToString();
            if(redAvgSignal > 100)
            {
                lblRedLedAvgSignal.Appearance.BorderColor2 = lblRedLedAvgSignal.Appearance.BorderColor = Color.LimeGreen;
            }
            else
            {
                lblRedLedAvgSignal.Appearance.BorderColor2 = lblRedLedAvgSignal.Appearance.BorderColor = Color.Red;
                ledPassFail.Value = false;
                ledPassFail.OffColor = Color.Red;
            }
        }
        private void setBlueGraphContrast(double[,] plotData)
        {
            double maximumSignal = 0;
            double minimumSignal = 65000;
            for (int x = 0; x < xArraySize; x++)
            {
                for (int y = 0; y < yArraySize; y++)
                {
                    if (plotData[x, y] > maximumSignal)
                    {
                        maximumSignal = plotData[x, y];
                    }
                    if ((plotData[x, y] < minimumSignal) && (plotData[x, y] != 0))
                    {
                        minimumSignal = plotData[x, y];
                    }
                }
            }
            if ((maximumSignal > 0) && (maximumSignal > minimumSignal))
            {
                this.colorScale1.Range =
                   new NationalInstruments.UI.Range(minimumSignal, maximumSignal);
            }

            double blueAvgSignal = maximumSignal + minimumSignal / 2;
            lblBlueLedAvgSignal.Text = blueAvgSignal.ToString();
            if (blueAvgSignal > 100)
            {
                lblBlueLedAvgSignal.Appearance.BorderColor2 = lblBlueLedAvgSignal.Appearance.BorderColor = Color.LimeGreen;
            }
            else
            {
                lblBlueLedAvgSignal.Appearance.BorderColor2 = lblBlueLedAvgSignal.Appearance.BorderColor = Color.Red;
                ledPassFail.Value = false;
                ledPassFail.OffColor = Color.Red;
            }
        }
        private void setDifferenceGraphContrast(double[,] plotData)
        {
            double maximumSignal = 0;
            double minimumSignal = 65000;
            for (int x = 0; x < xArraySize; x++)
            {
                for (int y = 0; y < yArraySize; y++)
                {
                    if (plotData[x, y] > maximumSignal)
                    {
                        maximumSignal = plotData[x, y];
                    }
                    if ((plotData[x, y] < minimumSignal) && (plotData[x, y] != 0))
                    {
                        minimumSignal = plotData[x, y];
                    }
                }
            }
            if ((maximumSignal > 0) && (maximumSignal > minimumSignal))
            {
                Range contrastRange = new Range(minimumSignal, maximumSignal);
                minContrastSlide.Range = contrastRange;
                minContrastSlide.Value = contrastRange.Minimum;
                maxContrastSlide.Range = contrastRange;
                maxContrastSlide.Value = contrastRange.Maximum;
                this.colorScale2.Range =
                   new NationalInstruments.UI.Range(minimumSignal, maximumSignal);
            }
        }
        public void ZoomIn()
        {
            try
            {
                if (zoomPanIndex < 8)
                {
                    differenceIntensityGraph.ZoomAnimation = false;
                    differenceIntensityGraph.ZoomXY(differenceIntensityGraph.Plots[0],
                            intensityCursor1.XPosition - 100, intensityCursor1.YPosition - 100, 200, 200);
                    differenceIntensityGraph.ZoomAroundPoint(zoomFactor);
                    // Set parameters/boundry to adjust the plot col&row waveform graphs
                    zoomPanIndex++;
                    zooming = true;
                    zoomFactor = zoomFactor + 1.5F;
                    differenceIntensityGraph_AfterMoveCursor(null, null);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public void ZoomOut()
        {
            try
            {
                if (zoomPanIndex <= 1)
                {
                    zooming = false;
                    zoomFactor = 1.5F;
                    zoomPanIndex = 0;
                    differenceIntensityGraph.ResetZoomPan();                   
                }
                else
                {
                    zoomFactor = 0.87F;
                    differenceIntensityGraph.ZoomAnimation = false;
                    differenceIntensityGraph.ZoomAroundPoint(zoomFactor);
                    // Set parameters/boundry to adjust the plot col&row waveform graphs                
                    int subarrayRegion = (70 / zoomPanIndex);
                    zoomPanIndex--;
                    zooming = true;
                }
                differenceIntensityGraph_AfterMoveCursor(null, null);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }     
        public async Task ClearRedBlueGraphs()
        {
            await Task.Run(() =>
            {
                this.intensityPlot1.ClearData();
                this.intensityPlot2.ClearData();
                this.intensityPlot3.ClearData(); 
            });
        }
        private async void buttonRedBlueAbort_Click(object sender, EventArgs e)
        {
            try
            {
                if (cts != null)
                {
                    DialogResult result = MessageBox.Show("Are you sure to Abort Red&Blue Test?",
                           "Abort Red&Blue Test",
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        cidInterface.AbortExposure();
                        redBlueTestStatus = false;
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.ResetFillAppearance();
                        await ClearRedBlueGraphs();
                        cts.Cancel();
                        buttonRedBlueRun.Enabled = true;
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
        private void minContrastSlide_AfterChangeValue(object sender, AfterChangeNumericValueEventArgs e)
        {
            try
            {
                double minColorScaleRange = minContrastSlide.Value;
                double maxColorScaleRange = maxContrastSlide.Value;
                if (maxContrastSlide.Value == minContrastSlide.Value)
                    return;
                if (minColorScaleRange < maxColorScaleRange)
                {
                    this.colorScale2.Range =
                       new NationalInstruments.UI.Range(minColorScaleRange, maxColorScaleRange);
                    // Fixes a NI refresh bug/issue
                    if (contrastSlideResize)
                    {                      
                        differenceIntensityGraph.Width = differenceIntensityGraph.Width + 1;
                        differenceIntensityGraph.Height = differenceIntensityGraph.Height + 1;
                        contrastSlideResize = false;
                    }
                    else
                    {
                        differenceIntensityGraph.Width = differenceIntensityGraph.Width - 1;
                        differenceIntensityGraph.Height = differenceIntensityGraph.Height - 1;
                        contrastSlideResize = true;
                    }
                }       
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void maxContrastSlide_AfterChangeValue(object sender, AfterChangeNumericValueEventArgs e)
        {
            try
            {              
                try
                {
                    double minColorScaleRange = minContrastSlide.Value;
                    double maxColorScaleRange = maxContrastSlide.Value;
                    if (maxContrastSlide.Value == minContrastSlide.Value)
                        return;
                    if (minColorScaleRange < maxColorScaleRange)
                    {
                        this.colorScale2.Range =
                           new NationalInstruments.UI.Range(minColorScaleRange, maxColorScaleRange);
                        // Fixes a NI refresh bug/issue
                        if (contrastSlideResize)
                        {
                            differenceIntensityGraph.Width = differenceIntensityGraph.Width + 1;
                            differenceIntensityGraph.Height = differenceIntensityGraph.Height + 1;
                            contrastSlideResize = false;
                        }
                        else
                        {
                            differenceIntensityGraph.Width = differenceIntensityGraph.Width - 1;
                            differenceIntensityGraph.Height = differenceIntensityGraph.Height - 1;
                            contrastSlideResize = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            } 
        }
        private async void enableMapButton_Click(object sender, EventArgs e)
        {
            try
            {                         
                if (enableMapButton.Text == "Enable")
                {
                    // Disable the mask, plot the Red, Blue, or Diff image
                    enableMapButton.Text = "Disable";
                    maskEnabled = true;
                    if ((redBluePresenter.ZDataRedDouble != null))
                    {
                        await redBluePresenter.BuildMaskImage(redBluePresenter.ZDataRedDouble);
                    }
                    if ((redBluePresenter.ZDataBlueDouble != null))
                    {
                        await redBluePresenter.BuildMaskImage(redBluePresenter.ZDataBlueDouble);
                    }
                    if ((redBluePresenter.ZDataRedDouble != null) && (redBluePresenter.ZDataBlueDouble != null))
                    {
                        await redBluePresenter.BuildMaskImage(redBluePresenter.ZDataDifference);
                    }
                    redBlueIntensityGraph.ClearData();
                    redBlueIntensityGraph.Plot(redBluePresenter.ZDataMasked);
                    blueIntensityGraph.ClearData();
                    blueIntensityGraph.Plot(redBluePresenter.ZDataMasked);
                    differenceIntensityGraph.ClearData();
                    differenceIntensityGraph.Plot(redBluePresenter.ZDataMasked);
                }
                else
                {
                    // Enable the mask, plot the Red, Blue, or Diff image with mask
                    enableMapButton.Text = "Enable";
                    maskEnabled = false;
                    if ((redBluePresenter.ZDataRedDouble != null))
                    {
                        redBlueIntensityGraph.Plot(redBluePresenter.ZDataRedDouble);
                    }
                    if ((redBluePresenter.ZDataBlueDouble != null))
                    {
                        blueIntensityGraph.Plot(redBluePresenter.ZDataBlueDouble);
                    }
                    if ((redBluePresenter.ZDataRedDouble != null) && (redBluePresenter.ZDataBlueDouble != null))
                    {
                        differenceIntensityGraph.Plot(redBluePresenter.ZDataDifference);
                    }
                }               
            }
             catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void redBlueIntensityGraph_AfterMoveCursor(object sender, AfterMoveIntensityCursorEventArgs e)
        {
            try
            {
                if (redBluePresenter!=null)
                {
                    // Get cursors current position
                    int xpos = Convert.ToInt32(intensityCursor2.XPosition);
                    int ypos = Convert.ToInt32(intensityCursor2.YPosition);
                    if ((redBluePresenter.ZDataRedDouble != null))
                    {
                        signalTextBox.Text = redBluePresenter.ZDataRedDouble[xpos, ypos].ToString();
                    }
                    if (maskEnabled)
                    {
                        signalTextBox.Text = redBluePresenter.ZDataMasked[xpos, ypos].ToString();
                    }
                    xPositionTextBox.Text = Convert.ToInt32(intensityCursor2.XPosition).ToString();
                    yPositionTextBox.Text = Convert.ToInt32(intensityCursor2.YPosition).ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void redBlueIntensityGraph_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Right)
                {
                    if (intensityCursor2.XPosition <= 2047)
                    {
                        intensityCursor2.XPosition = intensityCursor2.XPosition + 1;
                    }
                    else
                    {
                        intensityCursor2.XPosition = 2048;
                    }
                }
                if (e.KeyCode == Keys.Left)
                {
                    if (intensityCursor2.XPosition >= 1)
                    {
                        intensityCursor2.XPosition = intensityCursor2.XPosition - 1;
                    }
                    else
                    {
                        intensityCursor2.XPosition = 0;
                    }
                }
                if (e.KeyCode == Keys.Down)
                {
                    if (intensityCursor2.YPosition <= 2047)
                    {
                        intensityCursor2.YPosition = intensityCursor2.YPosition + 1;
                    }
                    else
                    {
                        intensityCursor2.YPosition = 2048;
                    }
                }
                if (e.KeyCode == Keys.Up)
                {
                    if (intensityCursor2.YPosition >= 1)
                    {
                        intensityCursor2.YPosition = intensityCursor2.YPosition - 1;
                    }
                    else
                    {
                        intensityCursor2.YPosition = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void redBlueIntensityGraph_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                redBlueIntensityGraph_AfterMoveCursor(null, null);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void redBlueIntensityGraph_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                PointF mouseClickPoint = e.Location;
                double xValue, yValue;
                // Move cursor to mouse postion
                intensityPlot1.InverseMapDataPoint(redBlueIntensityGraph.PlotAreaBounds, mouseClickPoint,
                    out xValue, out yValue);
                intensityCursor2.XPosition = xValue;
                intensityCursor2.YPosition = yValue;
                redBlueIntensityGraph_AfterMoveCursor(null, null);
            }
             catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
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
        private void RedBlueTest_Fill_Panel_Click(object sender, EventArgs e)
        {
            ResetProgressBar();
        }
        private void BlueIntensityGraph_AfterMoveCursor(object sender, AfterMoveIntensityCursorEventArgs e)
        {
            try
            {
                if (redBluePresenter != null)
                {
                    // Get cursors current position
                    int xpos = Convert.ToInt32(intensityCursor1.XPosition);
                    int ypos = Convert.ToInt32(intensityCursor1.YPosition);                   
                    if ((redBluePresenter.ZDataBlueDouble != null))
                    {
                        signalTextBox.Text = redBluePresenter.ZDataBlueDouble[xpos, ypos].ToString();
                    }
                    if (maskEnabled)
                    {
                        signalTextBox.Text = redBluePresenter.ZDataMasked[xpos, ypos].ToString();
                    }
                    xPositionTextBox.Text = Convert.ToInt32(intensityCursor1.XPosition).ToString();
                    yPositionTextBox.Text = Convert.ToInt32(intensityCursor1.YPosition).ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void BlueIntensityGraph_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                BlueIntensityGraph_AfterMoveCursor(null, null);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void BlueIntensityGraph_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                PointF mouseClickPoint = e.Location;
                double xValue, yValue;
                // Move cursor to mouse postion
                intensityPlot2.InverseMapDataPoint(blueIntensityGraph.PlotAreaBounds, mouseClickPoint,
                    out xValue, out yValue);
                intensityCursor1.XPosition = xValue;
                intensityCursor1.YPosition = yValue;
                BlueIntensityGraph_AfterMoveCursor(null, null);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void blueIntensityGraph_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Right)
                {
                    if (intensityCursor1.XPosition <= 2047)
                    {
                        intensityCursor1.XPosition = intensityCursor1.XPosition + 1;
                    }
                    else
                    {
                        intensityCursor1.XPosition = 2048;
                    }
                }
                if (e.KeyCode == Keys.Left)
                {
                    if (intensityCursor1.XPosition >= 1)
                    {
                        intensityCursor1.XPosition = intensityCursor1.XPosition - 1;
                    }
                    else
                    {
                        intensityCursor1.XPosition = 0;
                    }
                }
                if (e.KeyCode == Keys.Down)
                {
                    if (intensityCursor1.YPosition <= 2047)
                    {
                        intensityCursor1.YPosition = intensityCursor1.YPosition + 1;
                    }
                    else
                    {
                        intensityCursor1.YPosition = 2048;
                    }
                }
                if (e.KeyCode == Keys.Up)
                {
                    if (intensityCursor1.YPosition >= 1)
                    {
                        intensityCursor1.YPosition = intensityCursor1.YPosition - 1;
                    }
                    else
                    {
                        intensityCursor1.YPosition = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void differenceIntensityGraph_AfterMoveCursor(object sender, AfterMoveIntensityCursorEventArgs e)
        {
            try
            {
                if (redBluePresenter != null)
                {
                    // Get cursors current position
                    int xpos = Convert.ToInt32(intensityCursor3.XPosition);
                    int ypos = Convert.ToInt32(intensityCursor3.YPosition);
                    if ((redBluePresenter.ZDataDifference!= null))
                    {
                        signalTextBox.Text = redBluePresenter.ZDataDifference[xpos, ypos].ToString();
                    }
                    if (maskEnabled)
                    {
                        signalTextBox.Text = redBluePresenter.ZDataMasked[xpos, ypos].ToString();
                    }
                    xPositionTextBox.Text = Convert.ToInt32(intensityCursor3.XPosition).ToString();
                    yPositionTextBox.Text = Convert.ToInt32(intensityCursor3.YPosition).ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void differenceIntensityGraph_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                PointF mouseClickPoint = e.Location;
                double xValue, yValue;
                // Move cursor to mouse postion
                intensityPlot3.InverseMapDataPoint(differenceIntensityGraph.PlotAreaBounds, mouseClickPoint,
                    out xValue, out yValue);
                intensityCursor3.XPosition = xValue;
                intensityCursor3.YPosition = yValue;
                differenceIntensityGraph_AfterMoveCursor(null, null);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void differenceIntensityGraph_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
               differenceIntensityGraph_AfterMoveCursor(null, null);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }        
        private void differenceIntensityGraph_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Right)
                {
                    if (intensityCursor3.XPosition <= 2046)
                    {
                        intensityCursor3.XPosition = intensityCursor3.XPosition + 1;
                    }
                    else
                    {
                        intensityCursor3.XPosition = 2047;
                    }
                }
                if (e.KeyCode == Keys.Left)
                {
                    if (intensityCursor3.XPosition >= 1)
                    {
                        intensityCursor3.XPosition = intensityCursor3.XPosition - 1;
                    }
                    else
                    {
                        intensityCursor3.XPosition = 0;
                    }
                }
                if (e.KeyCode == Keys.Down)
                {
                    if (intensityCursor3.YPosition <= 2047)
                    {
                        intensityCursor3.YPosition = intensityCursor3.YPosition + 1;
                    }
                    else
                    {
                        intensityCursor3.YPosition = 2048;
                    }
                }
                if (e.KeyCode == Keys.Up)
                {
                    if (intensityCursor3.YPosition >= 1)
                    {
                        intensityCursor3.YPosition = intensityCursor3.YPosition - 1;
                    }
                    else
                    {
                        intensityCursor3.YPosition = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void RedBlueTest_Fill_Panel_Resize(object sender, EventArgs e)
        {           
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void btnUpdateLimit_Click(object sender, EventArgs e)
        {
            UpdateLimitsModelWithView();
        }

        private async void buttonRefreshLimit_Click(object sender, EventArgs e)
        {
            await redBluePresenter.GetLEDCalibrationLimits();
            UpdateLimitsViewWithModelOnLoad();
        }

        private void ultraComboEditorLimitIDs_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                redBluePresenter.LEDCalibrationLimitsData = redBluePresenter.LEDCalibrationLimitsDataList.Where
                    (limit => limit.LimitsID.ToString().Equals(ultraComboEditorLimitIDs.SelectedItem.DisplayText.ToString())).FirstOrDefault();
                uvLowerLEDLimit.Value = Convert.ToDecimal(redBluePresenter.LEDCalibrationLimitsData.LowerUVLEDLimit);
                uvUpperLEDLimit.Value = Convert.ToDecimal(redBluePresenter.LEDCalibrationLimitsData.UpperUVLEDLimit);
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
                limtIDs = (from ecos in redBluePresenter.LEDCalibrationLimitsDataList select ecos.LimitsID).ToList();
                ultraComboEditorLimitIDs.DataSource = limtIDs;
                ultraComboEditorLimitIDs.SelectedIndex = 0;
                redBluePresenter.LEDCalibrationLimitsData = redBluePresenter.LEDCalibrationLimitsDataList.
                    Where(limit => limit.LimitsID.Equals(limtIDs[0])).FirstOrDefault();
                if (redBluePresenter.LEDCalibrationLimitsData != null)
                {                 
                    uvLowerLEDLimit.Value= Convert.ToDecimal(redBluePresenter.LEDCalibrationLimitsData.LowerUVLEDLimit);
                    uvUpperLEDLimit.Value = Convert.ToDecimal(redBluePresenter.LEDCalibrationLimitsData.UpperUVLEDLimit);
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
                redBluePresenter.LEDCalibrationLimitsData = redBluePresenter.LEDCalibrationLimitsDataList.
                    FirstOrDefault(limit => limit.LimitsID.Equals(Convert.ToInt32(ultraComboEditorLimitIDs.SelectedItem.DisplayText)));
                redBluePresenter.LEDCalibrationLimitsData.LowerUVLEDLimit = Convert.ToInt32(uvLowerLEDLimit.Value);
                redBluePresenter.LEDCalibrationLimitsData.UpperUVLEDLimit = Convert.ToInt32(uvUpperLEDLimit.Value);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
