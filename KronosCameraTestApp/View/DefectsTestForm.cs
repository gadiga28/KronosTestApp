using CIDSoftwareApplication;
using Infragistics.Win.Misc;
using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.Presenter;
using KronosCameraTestApp.View.UserControls;
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
using static KronosCameraTestApp.Presenter.DefectsPresenter;

namespace KronosCameraTestApp.View
{
    public partial class DefectsTestForm : Form
    {
        public DefectsPresenter defectsPresenter { get; set; }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
          System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ListExposureData listExposureData { get; set; }
        private CIDInterface cidInterface = null;
        public int currentRecord = 0;
        public int currentSubarrayRecord = 0;
        private string userMode = string.Empty;
        bool engineeringLoginStatus = false;
        private bool contrastSlideResize = true;
        private int zoomPanIndex = 0;
        private float zoomFactor = 1.5F;
        private bool zooming = false;
        private static uint xArraySize = 2048;
        private static uint yArraySize = 2048;
        CancellationTokenSource cts { get; set; }
        ExposureSettings exposureSettings { get; set; }

        string imagerSerialNumber = string.Empty;
        public CIDInterface CidInterface
        {
            get { return cidInterface; }
            set { cidInterface = value; }
        }
        bool defectsTestStatus = false;
        List<KeyValuePair<string, List<Point>>> defPixelROIListPairsTemp;
        public bool DefectsTestStatus
        {
            get { return defectsTestStatus; }
            set { defectsTestStatus = value; }
        }
        public string ImagerSerialNumber
        {
            get { return imagerSerialNumber; }
            set { imagerSerialNumber = value; }
        }
        Form mdiParent = null;
        //public struct MaskDataStruct
        //{
        //    public int Xo;
        //    public int Yo;
        //    public int dX;
        //    public int dY;
        //    public int Ordnung;
        //    public bool IsDriftROI;
        //    public String name;
        //}
        public DefectsTestForm()
        {
            InitializeComponent();
        }
        public DefectsTestForm(DefectsPresenter defectsPresenter, string UserMode, bool enggLoginStatus,
            CIDInterface cidInterface, Form MDIParent)
        {
            try
            {
                InitializeComponent();
                currentSubarrayRecord = 0;
                currentRecord = 0;
                this.defectsPresenter = defectsPresenter;
                this.userMode = UserMode;
                this.engineeringLoginStatus = enggLoginStatus;
                this.cidInterface = cidInterface;
                mdiParent = MDIParent;
                defectsPresenter.EnableMask = true;
                clusterAnnotations.Checked = true;
                intensityCursorDefects.LabelVisible = true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void DefectsTestForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;              
                if (defectsPresenter.DefectsTestLimitsDataList.Count == 0 || TestAppHelper.UserModeChanged)
                {                   
                    await defectsPresenter.GetDefectsLimits();
                }
                UpdateLimitsViewWithModelOnLoad();
                exposureSettings = new ExposureSettings("DET", userMode, engineeringLoginStatus, TestAppHelper.UserLoginName, "DefectsData.xml", "DefectsExposures");
                ultraExplorerBarContainerControl2.Controls.Add(exposureSettings);
                exposureSettings.GetData();
                defectsPresenter.ExposureDataList = exposureSettings.ExposureList;
                defectsPresenter.SubarrayDataModelList = exposureSettings.SubarrayList;
                defectsPresenter.ExposureDataModel = exposureSettings.exposureDataPresenter.ExposureDataModel;
                defectsPresenter.SubarrayDataModel = exposureSettings.exposureDataPresenter.SubarrayDataModel;
                numericEditNumOfExposures.Value = defectsPresenter.ExposureDataList.Count;              
                if (userMode == "Manufacturing")
                {
                    EnableReadOnlyControlsManufacturingMode();
                }
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Loading Defects Test Form", "Loading Form", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void EnableReadOnlyControlsManufacturingMode()
        {
            try
            {
                NumExposuresGroupBox.Enabled = false;
                LimitsGroupBox.Enabled = false;
                settingsGroupBox.Enabled = false;
                enableDarkPixelAnnotations.Checked=enableDeadPixelAnnotations.Checked= enableHotPixelAnnotations.Checked= false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void DefectsTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
        private async void buttonRun_Click(object sender, EventArgs e)
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
                    ResetCalculatedData();
                    ((KronosTestAppMainForm)mdiParent).FirmwareManualTestLogsFile = "Defects_ManualTestFWLog_" +
                        ((KronosTestAppMainForm)mdiParent).CameraSerialNumber.Trim('\0').TrimEnd() + "_" + DateTime.Now.ToString("yymmddThhmm") + ".txt";
                    // Instantiate the CancellationTokenSource.  
                    cts = new CancellationTokenSource();
                    TestAppHelper.currentRunningTest = "DefectsTest";
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 1;
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Defects Test Processing....";
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Start();
                    buttonRun.Enabled = false;
                    buttonAbort.Enabled = true;
                    defectsTestStatus = true;
                    defectsPresenter.EnableTestData = enableTestData.Checked;
                    defectsPresenter.ClusterSize = Convert.ToInt32(clusterSize.Value);
                    defectsPresenter.AdjacentPixlesInCluster = Convert.ToInt32(adjacentPixelsInCluster.Value);
                    defectsPresenter.ExposureDataList = exposureSettings.ExposureList;
                    defectsPresenter.SubarrayDataModelList = exposureSettings.SubarrayList;
                    defectsPresenter.ExposureDataModel = exposureSettings.exposureDataPresenter.ExposureDataModel;
                    defectsPresenter.SubarrayDataModel = exposureSettings.exposureDataPresenter.SubarrayDataModel;
                    defectsPresenter.GetDeadAndDarkPixelList = GetDeadDarkPixels.Checked;
                    //defectsPresenter.XROIRange = Convert.ToInt32(xROIRangeLimit.Value);
                    //defectsPresenter.YROIRange = Convert.ToInt32(yROIRangeLimit.Value);
                    // defectsPresenter.YROIRangeLowLambda = Convert.ToInt32(yROIRangeLowLambda.Value);
                    defectsPresenter.AutoPretest = false;
                    RunDefectsTest(cts.Token);
                    await PlotDefectstData(cts);
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                    if (cts.IsCancellationRequested || !cidInterface.IsConnected())
                    {
                        if (!cidInterface.IsConnected())
                        {
                            TestAppHelper.noHeartBeatFromCamera();
                            cidInterface.AbortExposure();
                            cts.Cancel();
                        }
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Defects Test Aborted";
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.ResetFillAppearance();
                        ledPassFail.OffColor = Color.DarkGreen;
                    }
                    else
                    {
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Defects Test Completed";
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 100;
                    }
                    buttonAbort.Enabled = false;
                    buttonRun.Enabled = true;
                    defectsTestStatus = false;
                }
            }
            catch (Exception ex)
            {
                //"Internal Error";
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }  
        private void ResetCalculatedData()
        {
            try
            {
                defectsIntensityGraph.ClearData();
                defectsIntensityPlot.ClearData();
                ledPassFail.Value = false;
                ledPassFail.OffColor = Color.DarkGreen;
                PRNUDefects.Text = string.Empty;
                TrapDefects.Text = string.Empty;
                HotPixelDefects.Text = string.Empty;
                DeadPixelsDefects.Text = string.Empty;
                DarkPixelDefects.Text = string.Empty;
                DarkColumnDefects.Text = string.Empty;
                DarkRowDefects.Text = string.Empty;
                TotalRows.Text = string.Empty;
                LightColumnsDefects.Text = string.Empty;
                TotalColumns.Text = string.Empty;
                LightRowsDefects.Text = string.Empty;
                MeanDefects.Text = string.Empty;
                DriftROI.Text = string.Empty;
                TotalClusterDefects.Text = string.Empty;
                DefectivePixelsPerROIResult.Text = string.Empty;
                DefectPixelInMaskROIResult.Text = string.Empty;
                maskROIListlistBox.Items.Clear();
                selectedMaskROIDefectsListView.Items.Clear();
                driftROIsList.Items.Clear();
                selectedMaskROICount.Text = string.Empty;
                PRNUDefects.Appearance.BorderColor = PRNUDefects.Appearance.BorderColor2 = Color.Black;
                TrapDefects.Appearance.BorderColor = TrapDefects.Appearance.BorderColor2 = Color.Black;
                HotPixelDefects.Appearance.BorderColor = HotPixelDefects.Appearance.BorderColor2 = Color.Black;
                DeadPixelsDefects.Appearance.BorderColor = DeadPixelsDefects.Appearance.BorderColor2 = Color.Black;
                DarkPixelDefects.Appearance.BorderColor = DarkPixelDefects.Appearance.BorderColor2 = Color.Black;
                DarkColumnDefects.Appearance.BorderColor = DarkColumnDefects.Appearance.BorderColor2 = Color.Black;
                DarkRowDefects.Appearance.BorderColor = DarkRowDefects.Appearance.BorderColor2 = Color.Black;
                TotalRows.Appearance.BorderColor = TotalRows.Appearance.BorderColor2 = Color.Black;
                LightColumnsDefects.Appearance.BorderColor = LightColumnsDefects.Appearance.BorderColor2 = Color.Black;
                TotalColumns.Appearance.BorderColor = TotalColumns.Appearance.BorderColor2 = Color.Black;
                LightRowsDefects.Appearance.BorderColor = LightRowsDefects.Appearance.BorderColor2 = Color.Black;
                MeanDefects.Appearance.BorderColor = MeanDefects.Appearance.BorderColor2 = Color.Black;
                DriftROI.Appearance.BorderColor = DriftROI.Appearance.BorderColor2 = Color.Black;
                TotalClusterDefects.Appearance.BorderColor = TotalClusterDefects.Appearance.BorderColor2 = Color.Black;
                DefectivePixelsPerROIResult.Appearance.BorderColor = DefectivePixelsPerROIResult.Appearance.BorderColor2 = Color.Black;
                maskROIListlistBox.ForeColor= Color.Black;
                driftROIsList.ForeColor = Color.Black;
                percentAboveLabel.Text = string.Empty;
                percentBelowLabel.Text = string.Empty;
                percentAboveRowLabel.Text = string.Empty;
                percentBelowRowLabel.Text = string.Empty;
               
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void RunDefectsTest(CancellationToken ct)
        {
            try
            {
                log.Debug("Run Defects test.");
                defectsIntensityGraph.Annotations.Clear();
                defectsPresenter.runDefectsTest(cidInterface,ct, userMode, 6);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task PlotDefectstData(CancellationTokenSource ct)
        {
            try
            {
                if (ct.IsCancellationRequested)
                    return;                
                await defectsPresenter.CalculateDefects(ct.Token, serializeToBinFile.Checked);
                if (serializeToBinFile.Checked)
                {
                    ParametersModel parametersModel = TestAppHelper.GetParametersModel();
                    string manualtestFirmwareOutputPath;
                    manualtestFirmwareOutputPath = System.IO.Path.Combine(parametersModel.TestResultsServerPath.ToString(), "SCM5821AX1FirmwareManualTestLogs");

                    defectsPresenter.SerializeObjectFileName = manualtestFirmwareOutputPath + "\\DefectsTestSerializeObject_" +
                        imagerSerialNumber + "_" + DateTime.Now.ToString("YYMMDDThhmmss") + ".bin";

                    await defectsPresenter.serializeObject();
                }

                  
                if (ct.IsCancellationRequested)
                    return;
                if (!cidInterface.IsConnected())
                    return;
                if (defectsPresenter.ZData != null)
                {
                    defectsIntensityGraph.ClearData();                  
                    defectsIntensityGraph.Plot(defectsPresenter.ZData);
                    setGraphContrast(defectsPresenter.ZData);
                }
                PRNUDefects.Text = defectsPresenter.PRNUDefectsCount.ToString();               
                TrapDefects.Text = defectsPresenter.TrapDefectsCount.ToString();
                HotPixelDefects.Text = defectsPresenter.HotPixels.ToString();   
                DeadPixelsDefects.Text = defectsPresenter.DeadPixels.ToString();  
                DarkPixelDefects.Text = defectsPresenter.DarkPixels.ToString();              
                DarkColumnDefects.Text = defectsPresenter.DarkColumns.ToString();              
                DarkRowDefects.Text = defectsPresenter.DarkRows.ToString();               
                LightColumnsDefects.Text = defectsPresenter.LightColumns.ToString();               
                LightRowsDefects.Text = defectsPresenter.LightRows.ToString();               
                TotalRows.Text = (defectsPresenter.DarkRows + defectsPresenter.LightRows).ToString();   
                TotalColumns.Text = (defectsPresenter.DarkColumns + defectsPresenter.LightColumns).ToString();               
                MeanDefects.Text = String.Format("{0:0.00}", defectsPresenter.MeanDefects);
                DriftROI.Text = defectsPresenter.DriftROICount.ToString();

                defPixelROIListPairsTemp = defectsPresenter.DefPixelROIListPairs;
                //string[] defectPixelPerROI = (String.Join(";", defectsPresenter.DefPixPointsList.ToArray().Select(p => p.ToString()).ToArray())).Split(';');
                if (defectsPresenter.DefPixelROIListPairs!=null && defectsPresenter.DefPixelROIListPairs.Count() > 0)
                {
                    foreach (KeyValuePair<string, List<Point>> roipixs in defectsPresenter.DefPixelROIListPairs)
                    {
                        maskROIListlistBox.Items.Add(roipixs.Key);
                        //maskROIList.Items.Add(roipixs.Key);
                    }
                    maskROIListlistBox.ForeColor = Color.Red;
                    maskROIListlistBox.SelectedIndex = 0;
                    maskROIListlistBox_SelectedIndexChanged(null, null);
                }
                else
                {
                    maskROIListlistBox.ForeColor = Color.LimeGreen;
                }

                if(defectsPresenter.DriftROIPointsList.Count > 0)
                {
                    foreach(Point p in defectsPresenter.DriftROIPointsList)
                    {
                        driftROIsList.Items.Add(p);
                    }
                    driftROIsList.ForeColor = Color.Red;
                }
                else
                {
                    driftROIsList.ForeColor = Color.LimeGreen;
                }
                if (defectsPresenter.DeadDarkPixelsLimitReached)
                {
                    maskROIListlistBox.Items.Add("Max Limit Hit");
                    maskROIListlistBox.ForeColor = Color.Red;
                    maskROIListlistBox.SelectedIndex = 0;
                    maskROIListlistBox_SelectedIndexChanged(null, null);
                }
                // DefectivePixelsPerROIResult.Text = defectPixelPerROI.Count().ToString();
                DefectPixelInMaskROIResult.Text = defectsPresenter.DefPixInMaskROI.ToString();

                TotalClusterDefects.Text = defectsPresenter.TotalClusters.ToString();               
                percentAboveLabel.Text = defectsPresenter.PercentAboveValue.ToString();
                percentBelowLabel.Text=defectsPresenter.PercentBelowValue.ToString();
                percentAboveRowLabel.Text = defectsPresenter.PercentAboveRowValue.ToString();
                percentBelowRowLabel.Text = defectsPresenter.PercentBelowRowValue.ToString();
                addAnnotations(); 
                buttonRun.Enabled = true;
                buttonAbort.Enabled = false;
                if (defectsPresenter.DefectsTestPassed)
                    ledPassFail.Value = true;
                else
                {
                    ledPassFail.Value = false;
                    ledPassFail.OffColor = Color.Red;
                }
                FormatTestResults();
            }
            catch (Exception ex)
            {
                buttonRun.Enabled = true;
                buttonAbort.Enabled = false;
                defectsTestStatus = false;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void FormatTestResults()
        {
            try
            {
                if (defectsPresenter.TotalClusters <= defectsPresenter.DefectsTestLimitsData.TotalNumClusters)
                {
                    TotalClusterDefects.Appearance.BorderColor = TotalClusterDefects.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    TotalClusterDefects.Appearance.BorderColor = TotalClusterDefects.Appearance.BorderColor2 = Color.Red;
                }
                if (defectsPresenter.DriftROICount <= defectsPresenter.DefectsTestLimitsData.DriftROI)
                {
                    DriftROI.Appearance.BorderColor = DriftROI.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    DriftROI.Appearance.BorderColor = DriftROI.Appearance.BorderColor2 = Color.Red;
                }
                if ((defectsPresenter.DarkColumns + defectsPresenter.LightColumns) <= defectsPresenter.DefectsTestLimitsData.TotalColumns)
                {
                    TotalColumns.Appearance.BorderColor = TotalColumns.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    TotalColumns.Appearance.BorderColor = TotalColumns.Appearance.BorderColor2 = Color.Red;
                }
                if ((defectsPresenter.DarkRows + defectsPresenter.LightRows) <= defectsPresenter.DefectsTestLimitsData.TotalRows)
                {
                    TotalRows.Appearance.BorderColor = TotalRows.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    TotalRows.Appearance.BorderColor = TotalRows.Appearance.BorderColor2 = Color.Red;
                }
                if (defectsPresenter.LightRows <= defectsPresenter.DefectsTestLimitsData.TotalRows)
                {
                    LightRowsDefects.Appearance.BorderColor = LightRowsDefects.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    LightRowsDefects.Appearance.BorderColor = LightRowsDefects.Appearance.BorderColor2 = Color.Red;
                }
                if (defectsPresenter.LightColumns <= defectsPresenter.DefectsTestLimitsData.TotalColumns)
                {
                    LightColumnsDefects.Appearance.BorderColor = LightColumnsDefects.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    LightColumnsDefects.Appearance.BorderColor = LightColumnsDefects.Appearance.BorderColor2 = Color.Red;
                }
                if (defectsPresenter.DarkRows <= defectsPresenter.DefectsTestLimitsData.TotalRows)
                {
                    DarkRowDefects.Appearance.BorderColor = DarkRowDefects.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    DarkRowDefects.Appearance.BorderColor = DarkRowDefects.Appearance.BorderColor2 = Color.Red;
                }
                if (defectsPresenter.DarkColumns <= defectsPresenter.DefectsTestLimitsData.TotalColumns)
                {
                    DarkColumnDefects.Appearance.BorderColor = DarkColumnDefects.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    DarkColumnDefects.Appearance.BorderColor = DarkColumnDefects.Appearance.BorderColor2 = Color.Red;
                }
                if (defectsPresenter.DarkPixels <= defectsPresenter.DefectsTestLimitsData.DarkPixel)
                {
                    DarkPixelDefects.Appearance.BorderColor = DarkPixelDefects.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    DarkPixelDefects.Appearance.BorderColor = DarkPixelDefects.Appearance.BorderColor2 = Color.Red;
                }
                if (defectsPresenter.DeadPixels <= defectsPresenter.DefectsTestLimitsData.DeadPixel)
                {
                    DeadPixelsDefects.Appearance.BorderColor = DeadPixelsDefects.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    DeadPixelsDefects.Appearance.BorderColor = DeadPixelsDefects.Appearance.BorderColor2 = Color.Red;
                }
                if (defectsPresenter.HotPixels <= defectsPresenter.DefectsTestLimitsData.HotPixel)
                {
                    HotPixelDefects.Appearance.BorderColor = HotPixelDefects.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    HotPixelDefects.Appearance.BorderColor = HotPixelDefects.Appearance.BorderColor2 = Color.Red;
                }
                if (defectsPresenter.DefectivePixelsPerROICount <= defectsPresenter.DefectsTestLimitsData.DefectivePixelsPerROI)
                {
                    DefectivePixelsPerROIResult.Appearance.BorderColor = DefectivePixelsPerROIResult.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    DefectivePixelsPerROIResult.Appearance.BorderColor = DefectivePixelsPerROIResult.Appearance.BorderColor2 = Color.Red;
                }
                if (defectsPresenter.DeadDarkPixelsLimitReached)
                {
                    selectedMaskROICount.Text = "Max Limit of "+ defectsPresenter.DefectsTestLimitsData.DeadDarkPixelsListReadLimit.ToString()+" Reached";
                    selectedMaskROICount.ForeColor = Color.Red;
                }
                else
                {
                   // selectedMaskROICount.Text = "";
                    selectedMaskROICount.ForeColor = Color.Black;
                }
            }
            catch (Exception ex)
            {
              
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void addAnnotations()
        {
            try
            {
                int dpixels = 0;
                int dpixelsTemp = 0;
                double sumSignal = 0;
                double pixelCount = 0;
                double meanDefects = 0;
                int averageSignalAbove = 0;
                int averageSignalBelow = 0;
                double percentSignalAbove = defectsPresenter.DefectsTestLimitsData.PercentAboveMean / 100;
                double percentSignalBelow = defectsPresenter.DefectsTestLimitsData.PercentBelowMean / 100;
                //List<ExcelUtilities.MaskDataStruct> maskDataList = ExcelUtilities.GetMaskDataList();
                List<SubarrayRegion.SubarrayRegionStructure> ROIMeanList = new List<SubarrayRegion.SubarrayRegionStructure>();
                if (clusterAnnotations.Checked && defectsPresenter.ClusterPixelList.Count > 1)
                {
                    //log.Error("Clusterpixellist count" + defectsPresenter.ClusterPixelList.Count.ToString());
                    if (defectsPresenter.ClusterPixelList.Count > 0)
                    {
                       // log.Error("Clusterpixellist count" + defectsPresenter.ClusterPixelList.Count.ToString());
                        int adjPixels = Convert.ToInt32(adjacentPixelsInCluster.Value);
                        Point previousPixel = new Point(0, 0);
                        int totalClusters = 0;
                        if (defectsPresenter.ClusterPixelList.Count < 500)
                        {
                           // log.Error("Clusterpixellist count" + defectsPresenter.ClusterPixelList.Count.ToString());
                            foreach (Point pixel in defectsPresenter.ClusterPixelList)
                            {
                                if (((pixel.X - previousPixel.X) < 2) && (pixel.Y - previousPixel.Y < 2))
                                {
                                   // log.Error("adjacent pixels");
                                    adjPixels++;
                                }
                                else
                                {
                                    if (adjPixels >= Convert.ToInt32(clusterSize.Value))
                                    {
                                       // log.Error("totalClusters count" + totalClusters.ToString());
                                        totalClusters++;
                                        IntensityRangeAnnotation intensityRangeAnnotation =
                                         new IntensityRangeAnnotation(intensityXAxisDefects, intensityYAxisDefects);
                                        intensityRangeAnnotation.Caption = "Cluster(" + adjPixels.ToString() + ")" + previousPixel.ToString();
                                        intensityRangeAnnotation.CaptionVisible = true;
                                        intensityRangeAnnotation.CaptionBackColor = Color.BlueViolet;
                                        intensityRangeAnnotation.XRange = new Range(previousPixel.X, previousPixel.X + 1);
                                        intensityRangeAnnotation.YRange = new Range(previousPixel.Y, previousPixel.Y + 1);
                                        intensityRangeAnnotation.ArrowHeadStyle = NationalInstruments.UI.ArrowStyle.EmptyRound;
                                        intensityRangeAnnotation.Visible = true;
                                        intensityRangeAnnotation.CaptionVisible = true;
                                        intensityRangeAnnotation.ArrowVisible = true;
                                        defectsIntensityGraph.Annotations.Add(intensityRangeAnnotation);
                                    }
                                    adjPixels = 1;
                                }
                                previousPixel = pixel;
                            }
                        }
                    }
                }
                if (enableHotPixelAnnotations.Checked || enableDeadPixelAnnotations.Checked || enableDarkPixelAnnotations.Checked)
                {
                    for (int col = 2; col < defectsPresenter.CIDAnnotationData.dc - 2; col++)
                    {
                        for (int row = 2; row < defectsPresenter.CIDAnnotationData.dr - 2; row++)
                        {
                            if (defectsPresenter.ValidROIList[col, row])
                            {
                                if (defectsPresenter.CIDAnnotationData.videoIntegratedDataList[col, row] > 0)
                                {
                                    // Determine bad columns
                                    sumSignal = sumSignal + defectsPresenter.CIDAnnotationData.videoIntegratedDataList[col, row];
                                    pixelCount++;
                                }
                            }
                        }
                    }

                    foreach (MaskDataStruct maskDataStruct in defectsPresenter.MaskDataList)
                    {
                        int mean = 0;
                        int numberOfPoints = 0;
                        SubarrayRegion.SubarrayRegionStructure subarrayRegion =
                            new SubarrayRegion.SubarrayRegionStructure();

                        for (int x = maskDataStruct.Xo; x < maskDataStruct.Xo + maskDataStruct.dX; x++)
                        {
                            for (int y = maskDataStruct.Yo; y < maskDataStruct.Yo + maskDataStruct.dY; y++)
                            {

                                mean = mean + defectsPresenter.CIDAnnotationData.videoIntegratedDataList[x, y];
                                numberOfPoints++;
                            }
                        }
                        mean = mean / numberOfPoints;
                        subarrayRegion.StartX = maskDataStruct.Xo;
                        subarrayRegion.StartY = maskDataStruct.Yo;
                        subarrayRegion.EndX = maskDataStruct.dX;
                        subarrayRegion.EndY = maskDataStruct.dY;

                        // Using the SubarrayNumber to store the mean for this ROI
                        subarrayRegion.subarrayNumber = (uint)mean;
                        ROIMeanList.Add(subarrayRegion);

                    }

                    meanDefects = sumSignal / pixelCount;
                   
                    for (int x = 2; x < defectsPresenter.CIDAnnotationData.dc - 2; x++)
                    {
                        for (int y = 2; y < defectsPresenter.CIDAnnotationData.dr - 2; y++)
                        {
                            if (defectsPresenter.ValidROIList[x, y])
                            {
                                if (defectsPresenter.CIDAnnotationData.videoIntegratedDataList[x, y] == 0)
                                {
                                    IntensityRangeAnnotation intensityRangeAnnotation =
                                       new IntensityRangeAnnotation(intensityXAxisDefects, intensityYAxisDefects);

                                    intensityRangeAnnotation.Caption = "Dead Pixel";
                                    intensityRangeAnnotation.CaptionVisible = true;
                                    intensityRangeAnnotation.XRange = new Range(x, x + 1);
                                    intensityRangeAnnotation.YRange = new Range(y, y + 1);
                                    intensityRangeAnnotation.CaptionBackColor = Color.MediumPurple;
                                    if (defectsPresenter.PointInDriftCorrection(x, y))
                                    {
                                        intensityRangeAnnotation.CaptionBackColor = Color.Lime;
                                    }
                                    intensityRangeAnnotation.ArrowHeadStyle = ArrowStyle.EmptyRound;
                                    if (enableDeadPixelAnnotations.Checked)
                                    {
                                        intensityRangeAnnotation.Visible = true;
                                        intensityRangeAnnotation.CaptionVisible = true;
                                        intensityRangeAnnotation.ArrowVisible = true;
                                        defectsIntensityGraph.Annotations.Add(intensityRangeAnnotation);
                                    }
                                }
                                int roiMeanSignal = (int)meanDefects;
                                foreach (SubarrayRegion.SubarrayRegionStructure ROIStruct in ROIMeanList)
                                {
                                    // Use the ROI mean list to determine the mean for this pixel
                                    if (((x >= ROIStruct.StartX) && (x < ROIStruct.EndX + ROIStruct.StartX)) &&
                                         ((y >= ROIStruct.StartY) && (y < ROIStruct.EndY + ROIStruct.StartY)))
                                    {
                                        // SubarrayNumber was set to hold the Mean in this structure
                                        roiMeanSignal = (int)ROIStruct.subarrayNumber;
                                        break;
                                    }
                                }

                                averageSignalAbove = (int)(roiMeanSignal + (roiMeanSignal * percentSignalAbove));
                                averageSignalBelow = (int)(roiMeanSignal - (roiMeanSignal * percentSignalBelow));

                                if ((defectsPresenter.CIDAnnotationData.videoIntegratedDataList[x, y] < averageSignalBelow) &&
                                      (defectsPresenter.CIDAnnotationData.videoIntegratedDataList[x, y] != 0) &&
                                      (defectsPresenter.CIDAnnotationData.videoIntegratedDataList[x, y + 1] > averageSignalBelow) &&
                                      (defectsPresenter.CIDAnnotationData.videoIntegratedDataList[x, y - 1] > averageSignalBelow))
                                {
                                  
                                    IntensityRangeAnnotation intensityRangeAnnotation =
                                        new IntensityRangeAnnotation(intensityXAxisDefects, intensityYAxisDefects);

                                    intensityRangeAnnotation.Caption = "Dark Pixel";
                                    intensityRangeAnnotation.CaptionVisible = true;
                                    intensityRangeAnnotation.XRange = new Range(x, x + 1);
                                    intensityRangeAnnotation.YRange = new Range(y, y + 1);
                                    intensityRangeAnnotation.CaptionBackColor = Color.DarkTurquoise;
                                    if (defectsPresenter.PointInDriftCorrection(x, y))
                                    {
                                        intensityRangeAnnotation.CaptionBackColor = Color.Lime;
                                    }

                                    intensityRangeAnnotation.ArrowHeadStyle = ArrowStyle.EmptyRound;
                                    if (enableDarkPixelAnnotations.Checked)
                                    {
                                        dpixels++;
                                        intensityRangeAnnotation.Visible = true;
                                        intensityRangeAnnotation.CaptionVisible = true;
                                        intensityRangeAnnotation.ArrowVisible = true;
                                        defectsIntensityGraph.Annotations.Add(intensityRangeAnnotation);
                                        //log.Error("XRange:" + intensityRangeAnnotation.XRange.ToString() +"    "+ "YRange:" + intensityRangeAnnotation.YRange.ToString());
                                        //log.Error("YRange:" + intensityRangeAnnotation.YRange.ToString());
                                    }
                                }

                                if ((defectsPresenter.CIDAnnotationData.videoIntegratedDataList[x, y] > averageSignalAbove) &&
                                    (defectsPresenter.CIDAnnotationData.videoIntegratedDataList[x, y + 1] < averageSignalAbove) &&
                                    (defectsPresenter.CIDAnnotationData.videoIntegratedDataList[x, y - 1] < averageSignalAbove))
                                {
                                    IntensityRangeAnnotation intensityRangeAnnotation =
                                         new IntensityRangeAnnotation(intensityXAxisDefects, intensityYAxisDefects);

                                    intensityRangeAnnotation.Caption = "Hot Pixel";
                                    intensityRangeAnnotation.CaptionVisible = true;
                                    intensityRangeAnnotation.XRange = new Range(x, x + 1);
                                    intensityRangeAnnotation.YRange = new Range(y, y + 1);
                                    intensityRangeAnnotation.CaptionBackColor = Color.IndianRed;

                                    if (defectsPresenter.PointInDriftCorrection(x, y))
                                    {
                                        intensityRangeAnnotation.CaptionBackColor = Color.Lime;
                                    }

                                    intensityRangeAnnotation.ArrowHeadStyle = ArrowStyle.EmptyRound;

                                    if (enableHotPixelAnnotations.Checked)
                                    {
                                        intensityRangeAnnotation.Visible = true;
                                        intensityRangeAnnotation.CaptionVisible = true;
                                        intensityRangeAnnotation.ArrowVisible = true;
                                        defectsIntensityGraph.Annotations.Add(intensityRangeAnnotation);
                                    }
                                }
                            }
                        }
                    }
                   // log.Error("DarkPixel annotations" + dpixels.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }     
        private void setGraphContrast(double[,] plotData)
        {
            try
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
                    minContrastSlideDefects.Range = contrastRange;
                    minContrastSlideDefects.Value = contrastRange.Minimum;
                    maxContrastSlideDefects.Range = contrastRange;
                    maxContrastSlideDefects.Value = contrastRange.Maximum;
                    this.intensityDefectsGraphColorScale.Range =
                        new NationalInstruments.UI.Range(minimumSignal, maximumSignal);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void buttonAbort_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    if (cts != null)
                    {
                        DialogResult result = MessageBox.Show("Are you sure to Abort Defects Test?",
                               "Abort Defects Test",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            cidInterface.AbortExposure();
                            defectsTestStatus = false;
                            ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                            ClearDefectsGraphs();
                            cts.Cancel();
                            buttonRun.Enabled = true;
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
                limtIDs = (from limits in defectsPresenter.DefectsTestLimitsDataList select limits.LimitsID).ToList();
                ultraComboEditorLimitIDs.DataSource = limtIDs;
                ultraComboEditorLimitIDs.SelectedIndex = 0;
               // defectsPresenter.DefectsTestLimitsData = defectsPresenter.DefectsTestLimitsDataList.Where(limit => limit.LimitsID.Equals(limtIDs[0])  ).FirstOrDefault();
                defectsPresenter.DefectsTestLimitsData = defectsPresenter.DefectsTestLimitsDataList.Where(limit => limit.LimitsID.Equals(limtIDs[0]) && limit.TestStage.Equals("FINAL_TEST")).FirstOrDefault();
                if (defectsPresenter.DefectsTestLimitsData != null)
                {
                    TotalClustersLimit.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.TotalNumClusters);
                    HotPixelsLimit.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.HotPixel);
                    DarkPixelsLimit.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.DarkPixel);
                    DeadPixelsLimit.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.DeadPixel);
                    TotalRowsLimit.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.TotalRows);
                    TotalColumnsLimit.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.TotalColumns);
                    clusterSize.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.ClusterSize);
                    adjacentPixelsInCluster.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.AdjacentPixelsInCluster);
                    DriftROILimit.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.DriftROI);
                    percentAboveMean.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.PercentAboveMean);
                    percentBelowMean.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.PercentBelowMean);
                    percentAboveMeanRow.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.PercentAboveRows);
                    percentBelowMeanRow.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.PercentBelowRows);
                    DefectivePixelPerROILimit.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.DefectivePixelsPerROI);
                    xROIRangeLimit.Value= Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.XROIRange);
                    yROIRangeLimit.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.YROIRange);
                    darkPixelThreshold.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.DarkPixelThreshold);
                    deadDarkPixelsReadListLimit.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.DeadDarkPixelsListReadLimit);
                    BadPixelsInRowLimit.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.BadPixelsInRow);
                    BadPixelsInColLimit.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.BadPixelsInCol);
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
                //defectsPresenter.DefectsTestLimitsData = defectsPresenter.DefectsTestLimitsDataList.Where(
                //    (limit => limit.LimitsID.Equals(Convert.ToInt32(ultraComboEditorLimitIDs.SelectedItem.DisplayText)) && limit.TestStage.Equals("FINAL_TEST")));

                defectsPresenter.DefectsTestLimitsData = defectsPresenter.DefectsTestLimitsDataList.Where
                    (limit => limit.LimitsID.Equals(Convert.ToInt32(ultraComboEditorLimitIDs.SelectedItem.DisplayText))).FirstOrDefault();
                defectsPresenter.DefectsTestLimitsData.TotalNumClusters = Convert.ToInt32(TotalClustersLimit.Value);                
                defectsPresenter.DefectsTestLimitsData.XROIRange = Convert.ToInt32(xROIRangeLimit.Value);
                defectsPresenter.DefectsTestLimitsData.YROIRange = Convert.ToInt32(yROIRangeLimit.Value);
                defectsPresenter.DefectsTestLimitsData.DarkPixelThreshold = Convert.ToInt32(darkPixelThreshold.Value);
                defectsPresenter.DefectsTestLimitsData.HotPixel = Convert.ToInt32(HotPixelsLimit.Value);
                defectsPresenter.DefectsTestLimitsData.DeadPixel = Convert.ToInt32(DeadPixelsLimit.Value);
                defectsPresenter.DefectsTestLimitsData.DarkPixel = Convert.ToInt32(DarkPixelsLimit.Value);
                defectsPresenter.DefectsTestLimitsData.TotalColumns = Convert.ToInt32(TotalColumnsLimit.Value);
                defectsPresenter.DefectsTestLimitsData.TotalRows = Convert.ToInt32(TotalRowsLimit.Value);
                defectsPresenter.DefectsTestLimitsData.ClusterSize = Convert.ToInt32(clusterSize.Value);
                defectsPresenter.DefectsTestLimitsData.AdjacentPixelsInCluster = Convert.ToInt32(adjacentPixelsInCluster.Value);
                defectsPresenter.DefectsTestLimitsData.DriftROI = Convert.ToInt32(DriftROILimit.Value);
                defectsPresenter.DefectsTestLimitsData.PercentAboveMean = percentAboveMean.Value;
                defectsPresenter.DefectsTestLimitsData.PercentBelowMean = percentBelowMean.Value;
                defectsPresenter.DefectsTestLimitsData.PercentAboveRows = percentAboveMeanRow.Value;
                defectsPresenter.DefectsTestLimitsData.PercentBelowRows = percentBelowMeanRow.Value;
                defectsPresenter.DefectsTestLimitsData.DefectivePixelsPerROI = Convert.ToInt32(DefectivePixelPerROILimit.Value);
                defectsPresenter.DefectsTestLimitsData.BadPixelsInRow = Convert.ToInt32(BadPixelsInRowLimit.Value);
                defectsPresenter.DefectsTestLimitsData.BadPixelsInCol = Convert.ToInt32(BadPixelsInColLimit.Value);
                defectsPresenter.DefectsTestLimitsData.DeadDarkPixelsListReadLimit = Convert.ToInt32(deadDarkPixelsReadListLimit.Value);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public void ClearDefectsGraphs()
        {
            try
            {
                defectsIntensityGraph.ClearData();
                defectsIntensityPlot.ClearData();
                ledPassFail.Value = false;
                ledPassFail.OffColor = Color.DarkGreen;
                PRNUDefects.Text = string.Empty;
                TrapDefects.Text = string.Empty;
                HotPixelDefects.Text = string.Empty;
                DeadPixelsDefects.Text = string.Empty;
                DarkPixelDefects.Text = string.Empty;
                DarkColumnDefects.Text = string.Empty;
                DarkRowDefects.Text = string.Empty;
                TotalRows.Text = string.Empty;
                LightColumnsDefects.Text = string.Empty;
                TotalColumns.Text = string.Empty;
                LightRowsDefects.Text = string.Empty;
                MeanDefects.Text = string.Empty;
                DriftROI.Text = string.Empty;
                TotalClusterDefects.Text = string.Empty;
                DefectivePixelsPerROIResult.Text = string.Empty;
                DefectPixelInMaskROIResult.Text = string.Empty;
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
                defectsPresenter.DefectsTestLimitsData = defectsPresenter.DefectsTestLimitsDataList.Where
                    (limit => limit.LimitsID.ToString().Equals(ultraComboEditorLimitIDs.SelectedItem.DisplayText.ToString())).FirstOrDefault();
                TotalClustersLimit.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.TotalNumClusters);
                xROIRangeLimit.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.XROIRange);
                yROIRangeLimit.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.YROIRange);
                darkPixelThreshold.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.DarkPixelThreshold);
                HotPixelsLimit.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.HotPixel);
                DeadPixelsLimit.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.DeadPixel);
                DarkPixelsLimit.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.DarkPixel);
                TotalRowsLimit.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.TotalRows);
                TotalColumnsLimit.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.TotalColumns);
                clusterSize.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.ClusterSize);
                adjacentPixelsInCluster.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.AdjacentPixelsInCluster);
                DriftROILimit.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.DriftROI);
                percentAboveMean.Value= defectsPresenter.DefectsTestLimitsData.PercentAboveMean;
                percentBelowMean.Value =defectsPresenter.DefectsTestLimitsData.PercentBelowMean;
                percentAboveMeanRow.Value =defectsPresenter.DefectsTestLimitsData.PercentAboveRows;
                percentBelowMeanRow.Value = defectsPresenter.DefectsTestLimitsData.PercentBelowRows;
                DefectivePixelPerROILimit.Value= defectsPresenter.DefectsTestLimitsData.DefectivePixelsPerROI;
                BadPixelsInRowLimit.Value = defectsPresenter.DefectsTestLimitsData.BadPixelsInRow;
                BadPixelsInColLimit.Value = defectsPresenter.DefectsTestLimitsData.BadPixelsInCol;
                deadDarkPixelsReadListLimit.Value = Convert.ToDouble(defectsPresenter.DefectsTestLimitsData.DeadDarkPixelsListReadLimit);
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
        private async void buttonRefreshLimit_Click(object sender, EventArgs e)
        {           
            await defectsPresenter.GetDefectsLimits();
            UpdateLimitsViewWithModelOnLoad();
        }       
        private void numericEditNumOfExposures_ValueChanged(object sender, EventArgs e)
        {
            if (numericEditNumOfExposures.Value > defectsPresenter.ExposureDataList.Count)
            {
                string errstring = string.Format("Value cannot be greater than total exposure count '{0}'.", defectsPresenter.ExposureDataList.Count);
                MessageBox.Show(errstring, "Invalid exposure count", MessageBoxButtons.OK, MessageBoxIcon.Error);
                numericEditNumOfExposures.Value = defectsPresenter.ExposureDataList.Count;
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
        private void defectsIntensityGraph_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                PointF mouseClickPoint = e.Location;
                double xValue, yValue;
                // Move cursor to mouse postion
                defectsIntensityPlot.InverseMapDataPoint(defectsIntensityGraph.PlotAreaBounds, mouseClickPoint,
                    out xValue, out yValue);
                intensityCursorDefects.XPosition = xValue;
                intensityCursorDefects.YPosition = yValue;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void contrastSlide_AfterChangeValue(object sender, AfterChangeNumericValueEventArgs e)
        {
            double minColorScaleRange = minContrastSlideDefects.Value;
            double maxColorScaleRange = maxContrastSlideDefects.Value;
            if (maxContrastSlideDefects.Value == minContrastSlideDefects.Value)
                return;
            if (minColorScaleRange < maxColorScaleRange)
            {
                this.intensityDefectsGraphColorScale.Range =
                   new NationalInstruments.UI.Range(minColorScaleRange, maxColorScaleRange);
                // Fixes a NI refresh bug/issue
                if (contrastSlideResize)
                {
                    defectsIntensityGraph.Width = defectsIntensityGraph.Width + 1;
                    defectsIntensityGraph.Height = defectsIntensityGraph.Height + 1;
                    contrastSlideResize = false;
                }
                else
                {
                    defectsIntensityGraph.Width = defectsIntensityGraph.Width - 1;
                    defectsIntensityGraph.Height = defectsIntensityGraph.Height - 1;
                    contrastSlideResize = true;
                }
            }
        }
        public void ZoomIn()
        {
            try
            {
                if (zoomPanIndex < 8)
                {
                    defectsIntensityGraph.ZoomAnimation = false;
                    defectsIntensityGraph.ZoomXY(defectsIntensityGraph.Plots[0],
                            intensityCursorDefects.XPosition - 100, intensityCursorDefects.YPosition - 100, 200, 200);
                    defectsIntensityGraph.ZoomAroundPoint(zoomFactor);
                    zoomPanIndex++;                   
                    zooming = true;
                    zoomFactor = zoomFactor + 1.5F;
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
                    defectsIntensityGraph.ResetZoomPan();
                }
                else
                {
                    zoomFactor = 0.87F;
                    defectsIntensityGraph.ZoomAnimation = false;
                    defectsIntensityGraph.ZoomAroundPoint(zoomFactor);          
                    zoomPanIndex--;
                    zooming = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void graphPanel_Resize(object sender, EventArgs e)
        {
            try
            {
                int newWidth = this.graphPanel.Width - 30;
                // Maintain the aspect ratio 
                double aspectRatio = 0.95;// (double)this.Size.Height / this.Size.Width;
                double dHeight = (double)newWidth * aspectRatio;
                int newHeight = (int)dHeight;
                while (newHeight > (graphPanel.Size.Height - 20))
                {
                    // Recalculate width and height
                    newWidth = newWidth - 1;
                    dHeight = (double)newWidth * aspectRatio;
                    newHeight = (int)dHeight;
                }
                defectsIntensityGraph.Width = newWidth;
                defectsIntensityGraph.Height = newHeight;
                 maxContrastSlideDefects.Height = newHeight;
                minContrastSlideDefects.Height = newHeight;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void graphPanel_Click(object sender, EventArgs e)
        {
            ResetProgressBar();
        }
        private void ultraExplorerBar1_Click(object sender, EventArgs e)
        {
            ResetProgressBar();
        }
        private void enableMask_CheckedChanged(object sender, EventArgs e)
        {
            defectsPresenter.EnableMask = enableMask.Checked;
        }
        private void clusterAnnotations_CheckedChanged(object sender, EventArgs e)
        {
        }
        private void percentAboveMean_AfterChangeValue(object sender, AfterChangeNumericValueEventArgs e)
        {
        }
        private void defectsIntensityGraph_AfterMoveCursor(object sender, AfterMoveIntensityCursorEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }

        private void maskROIList_SelectionChanged(object sender, EventArgs e)
        {
           // KeyValuePair<string, List<Point>> tt= defectsPresenter.DefPixelROIListPairs.Where(p => p.Key == maskROIList.SelectedItem.DisplayText).FirstOrDefault();
           //if( tt.Value.Count >0)
           // {
           //     selectedMaskROICount.Text = tt.Value.Count.ToString();
           //     selectedMaskROIDefectsListView.Items.Clear();
           //     foreach (Point point in tt.Value)
           //     {
           //         //Infragistics.Win.UltraWinListView.UltraListViewItem ultraListViewItem2 = new Infragistics.Win.UltraWinListView.UltraListViewItem(point.ToString(), null, null);
           //         selectedMaskROIDefectsListView.Items.Add(point);
           //     }
           // }           
            
        }

        private void maskROIListlistBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(defPixelROIListPairsTemp !=null && defPixelROIListPairsTemp.Count > 0)
            {
                KeyValuePair<string, List<Point>> tt = defPixelROIListPairsTemp.Where(p => p.Key == maskROIListlistBox.SelectedItem.ToString()).FirstOrDefault();
                if (tt.Value.Count > 0)
                {
                    selectedMaskROICount.Text = tt.Value.Count.ToString();
                    selectedMaskROIDefectsListView.Items.Clear();
                    foreach (Point point in tt.Value)
                    {
                        //Infragistics.Win.UltraWinListView.UltraListViewItem ultraListViewItem2 = new Infragistics.Win.UltraWinListView.UltraListViewItem(point.ToString(), null, null);
                        selectedMaskROIDefectsListView.Items.Add(point);
                    }
                }
            }
            
        }

        private void btnResetROIRanges_Click(object sender, EventArgs e)
        {
            xROIRangeLimit.Value = 4;
            yROIRangeLimit.Value = 5;
        }
    }
}
