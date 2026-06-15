using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KronosCameraTestApp.Presenter;
using KronosCameraTestApp.Model;
using Thermo.Kronos.Instrument.Camera.Interface;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Configuration;
using CIDSoftwareApplication;
using KronosCameraTestApp.Helpers;
using System.Security.AccessControl;
using NationalInstruments.UI;
using Infragistics.Win.Misc;
namespace KronosCameraTestApp.View.UserControls
{
    public partial class DefectsUserControl : UserControl
    {
        public DefectsPresenter defectsPresenter { get; set; }       
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string defaultFilePath = string.Empty;
        CID821_Data cid821Data = new CID821_Data();
        private CIDInterface cidInterface = null;
        string TestResultImageFilePath = string.Empty;
        string userMode = string.Empty;
        private static uint xArraySize = 2048;
        private static uint yArraySize = 2048;
        private int zoomPanIndex = 0;
        private float zoomFactor = 1.5F;
        private bool zooming = false;
        private bool contrastSlideResize = true;
        public CIDInterface CidInterface
        {
            get { return cidInterface; }
            set { cidInterface = value; }
        }
        AutoTestExposureSettings autoTestExposureSettings { get; set; }
        List<KeyValuePair<string, List<Point>>> defPixelROIListPairsTemp;
        bool _pretest = false;
        public DefectsUserControl()
        {
            InitializeComponent();
        }
        public DefectsUserControl(DefectsPresenter defectsPresenter, string defaultFilePath, string UserMode, bool pretest)
        {
            this.defectsPresenter = defectsPresenter;
            InitializeComponent();
            this.defaultFilePath = defaultFilePath;
            this.userMode = UserMode;
            intensityCursorDefects.LabelVisible = true;
            _pretest = pretest;
        }
        private async void DefectstUserControl_Load(object sender, EventArgs e)
        {
            try
            {               
                if (autoTestExposureSettings == null)
                {
                    autoTestExposureSettings = new AutoTestExposureSettings("DET", cidInterface);
                    autoTestExposureSettings.GetData();
                    defectsPresenter.ExposureDataList = autoTestExposureSettings.ExposureList;
                    defectsPresenter.SubarrayDataModelList = autoTestExposureSettings.SubarrayList;
                    defectsPresenter.ExposureDataModel = autoTestExposureSettings.exposureDataPresenter.ExposureDataModel;
                    defectsPresenter.SubarrayDataModel = autoTestExposureSettings.exposureDataPresenter.SubarrayDataModel;
                    ultraExplorerBarContainerControl2.Controls.Add(autoTestExposureSettings);
                }
                defectsPresenter.AutoPretest = _pretest;
                if (defectsPresenter.DefectsTestLimitsDataList.Count == 0 || TestAppHelper.UserModeChanged)
                {
                    await defectsPresenter.GetDefectsLimits();                          
                }
                if (_pretest)
                {
                    defectsPresenter.DefectsTestLimitsData = defectsPresenter.DefectsTestLimitsDataList.Where(d => d.TestStage.Equals("PRE_TEST")).FirstOrDefault();
                }
                else
                {
                    defectsPresenter.DefectsTestLimitsData = defectsPresenter.DefectsTestLimitsDataList.Where(d => d.TestStage.Equals("FINAL_TEST")).FirstOrDefault();

                }
                UpdateLimitsViewWithModelOnLoad();
                defectsPresenter.AdjacentPixlesInCluster = defectsPresenter.DefectsTestLimitsData.AdjacentPixelsInCluster;
                defectsPresenter.ClusterSize = defectsPresenter.DefectsTestLimitsData.ClusterSize;
                if (defectsPresenter.ExposureDataList.Count == 0 || TestAppHelper.UserModeChanged)
                {
                    await defectsPresenter.GetDefectsData(cidInterface);
                }                
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task RunAutoDefectsTest(CancellationTokenSource cts)
        {
            try
            {
                if (cts.IsCancellationRequested)
                    return;
                //await ClearDefectsGraphs();
                defectsPresenter.GetDeadAndDarkPixelList = true;
                //defectsPresenter.AutoPretest = AutoPretest;
                ResetCalculatedData();
                Thread.Sleep(2000);
                if (autoTestExposureSettings == null)
                {
                    autoTestExposureSettings = new AutoTestExposureSettings("DET", cidInterface);
                    autoTestExposureSettings.GetData();
                    defectsPresenter.ExposureDataList = autoTestExposureSettings.ExposureList;
                    defectsPresenter.SubarrayDataModelList = autoTestExposureSettings.SubarrayList;
                    defectsPresenter.ExposureDataModel = autoTestExposureSettings.exposureDataPresenter.ExposureDataModel;
                    defectsPresenter.SubarrayDataModel = autoTestExposureSettings.exposureDataPresenter.SubarrayDataModel;
                    ultraExplorerBarContainerControl2.Controls.Add(autoTestExposureSettings);
                }
                if (defectsPresenter.ExposureDataList.Count == 0)
                {
                    await defectsPresenter.GetDefectsData(cidInterface);
                    //await defectsPresenter.GetDarkCurrentLimits();
                }
                defectsPresenter.EnableMask = true;
               
                if (defectsPresenter.DefectsTestLimitsDataList.Count == 0)
                {
                    await defectsPresenter.GetDefectsLimits();
                             
                }
               // DefectsTestLimitsData DefectsTestLimitsData;
                if (_pretest)
                {
                    defectsPresenter.DefectsTestLimitsData = defectsPresenter.DefectsTestLimitsDataList.Where(d => d.TestStage.Equals("PRE_TEST")).FirstOrDefault();
                }
                else
                    {
                    defectsPresenter.DefectsTestLimitsData = defectsPresenter.DefectsTestLimitsDataList.Where(d => d.TestStage.Equals("FINAL_TEST")).FirstOrDefault();

                }
                UpdateLimitsViewWithModelOnLoad();
                defectsPresenter.AdjacentPixlesInCluster = defectsPresenter.DefectsTestLimitsData.AdjacentPixelsInCluster;
                defectsPresenter.ClusterSize = defectsPresenter.DefectsTestLimitsData.ClusterSize;
                defectsPresenter.PercentAbove = defectsPresenter. DefectsTestLimitsData.PercentAboveMean;// Convert.ToDouble(percentAboveMean.Text);
                defectsPresenter.PercentBelow = defectsPresenter.DefectsTestLimitsData.PercentBelowMean;// Convert.ToDouble(percentBelowMean.Text);
                defectsPresenter.PercentAboveMeanRow = defectsPresenter. DefectsTestLimitsData.PercentAboveRows;// Convert.ToDouble(percentAboveMean.Text);
                defectsPresenter.PercentBelowMeanRow = defectsPresenter.DefectsTestLimitsData.PercentBelowRows;// Convert.ToDouble(percentBelowMean.Text);
                //defectsPresenter.XROIRange = defectsPresenter.DefectsTestLimitsData.XROIRange;
                //defectsPresenter.YROIRange = defectsPresenter.DefectsTestLimitsData.YROIRange;
                //defectsPresenter.YROIRangeLowLambda = 61;
                //Run Dark Current and Defects Test
                RunDefectsTest(cts.Token);
                //Plot Dark Current and Defects Data
                await PlotDefectsData(cts);
            }
            catch (Exception ex)
            {
                //"Internal Error";
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void RunDefectsTest(CancellationToken ct)
        {
            try
            {
                log.Debug("Run Defects test.");
                if (ct.IsCancellationRequested)
                    return;
                defectsIntensityGraph.Annotations.Clear();
                defectsPresenter.runDefectsTest(cidInterface, ct, userMode);
            }
            catch (Exception ex)
            {
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
                DeadPixels.Text = string.Empty;
                DarkPixels.Text = string.Empty;
                DarkColumns.Text = string.Empty;
                DarkRows.Text = string.Empty;
                TotalRowsCount.Text = string.Empty;
                TotalColumnsCount.Text = string.Empty;
                LightColumns.Text = string.Empty;
                LightRows.Text = string.Empty;
                MeanDefects.Text = string.Empty;
                DriftROI.Text = string.Empty;
                TotalClusters.Text = string.Empty;
                maskROIListlistBox.Items.Clear();
                selectedMaskROIDefectsListView.Items.Clear();
                //maskROIsWithDefectivePixelsCount.Text = string.Empty;
                ledPassFail.Value = false;              
                selectedMaskROICount.Text = string.Empty;
                PRNUDefects.Appearance.BorderColor = PRNUDefects.Appearance.BorderColor2 = Color.Black;
                TrapDefects.Appearance.BorderColor = TrapDefects.Appearance.BorderColor2 = Color.Black;
                HotPixelDefects.Appearance.BorderColor = HotPixelDefects.Appearance.BorderColor2 = Color.Black;
                DeadPixels.Appearance.BorderColor = DeadPixels.Appearance.BorderColor2 = Color.Black;
                DarkPixels.Appearance.BorderColor = DarkPixels.Appearance.BorderColor2 = Color.Black;
                DarkColumns.Appearance.BorderColor = DarkColumns.Appearance.BorderColor2 = Color.Black;
                DarkRows.Appearance.BorderColor = DarkRows.Appearance.BorderColor2 = Color.Black;
                TotalRowsCount.Appearance.BorderColor = TotalRowsCount.Appearance.BorderColor2 = Color.Black;
                TotalColumnsCount.Appearance.BorderColor = TotalColumnsCount.Appearance.BorderColor2 = Color.Black;
                LightColumns.Appearance.BorderColor = LightColumns.Appearance.BorderColor2 = Color.Black;
                LightRows.Appearance.BorderColor = LightRows.Appearance.BorderColor2 = Color.Black;
                DriftROI.Appearance.BorderColor = DriftROI.Appearance.BorderColor2 = Color.Black;
                TotalClusters.Appearance.BorderColor = TotalClusters.Appearance.BorderColor2 = Color.Black;
                maskROIListlistBox.ForeColor = Color.Black;
                // maskROIsWithDefectivePixelsCount.Appearance.BorderColor = TotalClusters.Appearance.BorderColor2 = Color.Black;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task PlotDefectsData(CancellationTokenSource ct)
        {
            try
            {
                if (ct.IsCancellationRequested)
                    return;
                await defectsPresenter.CalculateDefects(ct.Token, true);
                //await defectsPresenter.serializeObject();
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
                StringBuilder autoTestFailureReason = new StringBuilder();
                if (defectsPresenter.DarkColumns <= defectsPresenter.DefectsTestLimitsData.TotalColumns)
                {
                    DarkColumns.Appearance.BorderColor = DarkColumns.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    DarkColumns.Appearance.BorderColor = DarkColumns.Appearance.BorderColor2 = Color.Red;
                    autoTestFailureReason.Append(DarkColumns.Text + " - Bad Cols,");
                }
                DarkRows.Text = defectsPresenter.DarkRows.ToString();
                if (defectsPresenter.DarkRows <= defectsPresenter.DefectsTestLimitsData.TotalRows)
                {
                    DarkRows.Appearance.BorderColor = DarkRows.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    DarkRows.Appearance.BorderColor = DarkRows.Appearance.BorderColor2 = Color.Red;
                    autoTestFailureReason.Append(DarkRows.Text + " - Bad Rows,");
                }
                TotalRowsCount.Text = (defectsPresenter.DarkRows + defectsPresenter.LightRows).ToString();
                if ((defectsPresenter.DarkRows + defectsPresenter.LightRows) <= defectsPresenter.DefectsTestLimitsData.TotalRows)
                {
                    TotalRowsCount.Appearance.BorderColor = TotalRowsCount.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    TotalRowsCount.Appearance.BorderColor = TotalRowsCount.Appearance.BorderColor2 = Color.Red;
                    autoTestFailureReason.Append(TotalRowsCount.Text + " - Total Rows,");
                }
                LightColumns.Text = defectsPresenter.LightColumns.ToString();
                if (defectsPresenter.LightColumns <= defectsPresenter.DefectsTestLimitsData.TotalColumns)
                {
                    LightColumns.Appearance.BorderColor = LightColumns.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    LightColumns.Appearance.BorderColor = LightColumns.Appearance.BorderColor2 = Color.Red;
                    autoTestFailureReason.Append(LightColumns.Text + " - Light Cols,");
                }
                TotalColumnsCount.Text = (defectsPresenter.DarkColumns + defectsPresenter.LightColumns).ToString();
                if ((defectsPresenter.DarkColumns + defectsPresenter.LightColumns) <= defectsPresenter.DefectsTestLimitsData.TotalColumns)
                {
                    TotalColumnsCount.Appearance.BorderColor = TotalColumnsCount.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    TotalColumnsCount.Appearance.BorderColor = TotalColumnsCount.Appearance.BorderColor2 = Color.Red;
                    autoTestFailureReason.Append(TotalColumnsCount.Text + " - Total Columns,");
                }
                LightRows.Text = defectsPresenter.LightRows.ToString();
                if (defectsPresenter.LightRows <= defectsPresenter.DefectsTestLimitsData.TotalRows)
                {
                    LightRows.Appearance.BorderColor = LightRows.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    LightRows.Appearance.BorderColor = LightRows.Appearance.BorderColor2 = Color.Red;
                    autoTestFailureReason.Append(LightRows.Text + " - Light Rows,");
                }
                if (defectsPresenter.HotPixels <= defectsPresenter.DefectsTestLimitsData.HotPixel)
                {
                    HotPixelDefects.Appearance.BorderColor = HotPixelDefects.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    HotPixelDefects.Appearance.BorderColor = HotPixelDefects.Appearance.BorderColor2 = Color.Red;
                    autoTestFailureReason.Append(HotPixelDefects.Text + " - Hot Pixels,");
                }
                DeadPixels.Text = defectsPresenter.DeadPixels.ToString();
                if (defectsPresenter.DeadPixels <= defectsPresenter.DefectsTestLimitsData.DeadPixel)
                {
                    DeadPixels.Appearance.BorderColor = DeadPixels.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    DeadPixels.Appearance.BorderColor = DeadPixels.Appearance.BorderColor2 = Color.Red;
                    autoTestFailureReason.Append(DeadPixels.Text + " - Dead Pixels,");
                }
                DarkPixels.Text = defectsPresenter.DarkPixels.ToString();
                if (defectsPresenter.DarkPixels <= defectsPresenter.DefectsTestLimitsData.DarkPixel)
                {
                    DarkPixels.Appearance.BorderColor = DarkPixels.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    DarkPixels.Appearance.BorderColor = DarkPixels.Appearance.BorderColor2 = Color.Red;
                    autoTestFailureReason.Append(DarkPixels.Text + " - Dark Pixels,");
                }                
                MeanDefects.Text = String.Format("{0:0.00}", defectsPresenter.MeanDefects);
                DriftROI.Text = defectsPresenter.DriftROICount.ToString();
                if (defectsPresenter.DriftROICount <= defectsPresenter.DefectsTestLimitsData.DriftROI)
                {
                    DriftROI.Appearance.BorderColor = DriftROI.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    DriftROI.Appearance.BorderColor = DriftROI.Appearance.BorderColor2 = Color.Red;
                    autoTestFailureReason.Append(DriftROI.Text + " - Drift ROI,");
                }
                if (defectsPresenter.DriftROIPointsList.Count > 0)
                {
                    foreach (Point p in defectsPresenter.DriftROIPointsList)
                    {
                        driftROIsList.Items.Add(p);
                    }
                    if(defectsPresenter.DriftROIPointsList.Count > defectsPresenter.DefectsTestLimitsData.DriftROI)
                        driftROIsList.ForeColor = Color.Red;
                    else
                        driftROIsList.ForeColor = Color.LimeGreen;
                }
                //else
                //{
                //    driftROIsList.ForeColor = Color.LimeGreen;
                //}
                TotalClusters.Text = defectsPresenter.TotalClusters.ToString();
                if (defectsPresenter.TotalClusters <= defectsPresenter.DefectsTestLimitsData.TotalNumClusters)
                {
                    TotalClusters.Appearance.BorderColor = TotalClusters.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    TotalClusters.Appearance.BorderColor = TotalClusters.Appearance.BorderColor2 = Color.Red;
                    autoTestFailureReason.Append(TotalClusters.Text + " - Total Clusters,");
                }
                defPixelROIListPairsTemp = defectsPresenter.DefPixelROIListPairs;

                if (defectsPresenter.DefPixelROIListPairs != null && defectsPresenter.DefPixelROIListPairs.Count() > 0)
                {
                    foreach (KeyValuePair<string, List<Point>> roipixs in defectsPresenter.DefPixelROIListPairs)
                    {
                        maskROIListlistBox.Items.Add(roipixs.Key);
                        //maskROIList.Items.Add(roipixs.Key);
                    }
                    if(defectsPresenter.DefPixelROIListPairs.Count() > 0)
                    {
                        maskROIListlistBox.ForeColor = Color.Red;
                        maskROIListlistBox.SelectedIndex = 0;
                        maskROIListlistBox_SelectedIndexChanged_1(null, null);
                        autoTestFailureReason.Append(defectsPresenter.DefPixelROIListPairs.Count().ToString() + " - Mask ROI,");
                    }
                    else
                    {
                        maskROIListlistBox.ForeColor = Color.LimeGreen;
                    }

                }
                if (defectsPresenter.DeadDarkPixelsLimitReached)
                {
                    maskROIListlistBox.Items.Add("Max Limit Hit");
                    selectedMaskROICount.Text = "Max Limit of " + defectsPresenter.DefectsTestLimitsData.DeadDarkPixelsListReadLimit.ToString() + " Reached";
                    selectedMaskROICount.ForeColor = Color.Red;
                    maskROIListlistBox.SelectedIndex = 0;
                    maskROIListlistBox_SelectedIndexChanged_1(null, null);
                    autoTestFailureReason.Append(" - Mask ROI Limit Reached,");
                }
                else
                {
                    //selectedMaskROICount.Text = "";
                    //selectedMaskROICount.ForeColor = Color.Black;
                }


                defectsPresenter.DefectsTestFailureReason = autoTestFailureReason.ToString().TrimEnd(new char[] { ',' });

                percentAboveLabel.Text = defectsPresenter.PercentAboveValue.ToString();
                percentBelowLabel.Text = defectsPresenter.PercentBelowValue.ToString();
                percentAboveRowLabel.Text = defectsPresenter.PercentAboveRowValue.ToString();
                percentBelowRowLabel.Text = defectsPresenter.PercentBelowRowValue.ToString();
                addAnnotations();
                if (defectsPresenter.DefectsTestPassed)
                {
                    log.Info("Defects test passed");
                    ledPassFail.Value = true;
                }

                else
                {
                    ledPassFail.Value = false;
                    ledPassFail.OffColor = Color.Red;
                    log.Info("Defects test failed");
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
                if (defectsPresenter.ClusterPixelList != null && defectsPresenter.ClusterPixelList.Count > 1)
                {
                    int adjacentPixels = 1;
                    Point previousPixel = new Point(0, 0);
                    int totalClusters = 0;
                    if (defectsPresenter.ClusterPixelList.Count < 500)
                    {                    
                    foreach (Point pixel in defectsPresenter.ClusterPixelList)
                    {
                        if (((pixel.X - previousPixel.X) < 2) && (pixel.Y - previousPixel.Y < 2))
                        {
                            adjacentPixels++;
                        }
                        else
                        {
                            if (adjacentPixels >= 2)
                            {
                                totalClusters++;
                                IntensityRangeAnnotation intensityRangeAnnotation =
                                 new IntensityRangeAnnotation(intensityXAxisDefects, intensityYAxisDefects);
                                intensityRangeAnnotation.Caption = "Clusters(" + adjacentPixels.ToString() + ")" + previousPixel.ToString();
                                intensityRangeAnnotation.CaptionVisible = true;
                                intensityRangeAnnotation.XRange = new Range(previousPixel.X, previousPixel.X + 1);
                                intensityRangeAnnotation.YRange = new Range(previousPixel.Y, previousPixel.Y + 1);
                                intensityRangeAnnotation.ArrowHeadStyle = NationalInstruments.UI.ArrowStyle.EmptyRound;
                                intensityRangeAnnotation.Visible = true;
                                intensityRangeAnnotation.CaptionVisible = true;
                                intensityRangeAnnotation.ArrowVisible = true;
                                defectsIntensityGraph.Annotations.Add(intensityRangeAnnotation);
                            }
                            adjacentPixels = 1;
                        }
                        previousPixel = pixel;
                    }
                }
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
        public void SaveTestResultImage(string testResultLocalFilePath, string testResultsServerPath, string localFileTimeStamp, string serverPathToDB, bool savetoNetwork)
        {
            try
            {
                // BeginInvoke(new System.Action(() => this.Dock = DockStyle.None));
                this.Dock = DockStyle.None;
                Image img = null;                
                int outputImageWidth = 0;
                int outputImageHeight = 0;
                Bitmap outputImage = null;
                img = this.defectsIntensityGraph.ToImage();
                outputImageWidth = img.Width;
                outputImageHeight = img.Height;
                outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics graphics = Graphics.FromImage(outputImage))
                {
                    graphics.DrawImage(img, new Rectangle(new Point(), img.Size),
                           new Rectangle(new Point(), img.Size), GraphicsUnit.Pixel);
                }
                outputImage.Save(testResultLocalFilePath + "\\DefectsTestResultImage_"+localFileTimeStamp+".png", System.Drawing.Imaging.ImageFormat.Png);
                //BeginInvoke(new System.Action(() => this.Dock = DockStyle.Fill));
                this.Dock = DockStyle.Fill;
                if (ConfigurationManager.AppSettings["EndClient"] != "Thermo")
                {
                    TestAppHelper.saveFileInAzure(outputImage, "Defects.png");
                    //defectsPresenter.TestResultImagePath = TestAppHelper.AzureBlobURI;
                }
                else
                {
                    if (savetoNetwork)
                    {
                        outputImage.Save(testResultsServerPath + "\\DefectsTestResultImage_" + localFileTimeStamp + ".png", System.Drawing.Imaging.ImageFormat.Png);
                        defectsPresenter.TestResultImagePath = serverPathToDB + "\\DefectsTestResultImage_" + localFileTimeStamp + ".png";
                    }
                    else
                        defectsPresenter.TestResultImagePath = string.Empty;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }      
        public async Task ClearDefectsGraphs()
        {
            await Task.Run(() =>
            {
                defectsIntensityGraph.ClearData();
                defectsIntensityPlot.ClearData();                
            });
        }
        private void UpdateLimitsViewWithModelOnLoad()
        {
            try
            {
                List<int> limtIDs = new List<int>();
                limtIDs = (from limits in defectsPresenter.DefectsTestLimitsDataList select limits.LimitsID).ToList();
                if (!string.IsNullOrWhiteSpace(TestAppHelper.SelectedECONumber) && (userMode.Equals("Engineering") || userMode.Equals("Admin")))
                {
                    LimitID.Text = defectsPresenter.DefectsTestLimitsData.LimitsID.ToString(); 
                }
                else
                {
                    LimitID.Text = limtIDs[0].ToString();
                    
                }
                if (defectsPresenter.DefectsTestLimitsData != null)
                {
                    TotalClustersLimit.Text = defectsPresenter.DefectsTestLimitsData.TotalNumClusters.ToString();
                    HotPixelsLimit.Text = defectsPresenter.DefectsTestLimitsData.HotPixel.ToString();
                    DarkPixelsLimit.Text = defectsPresenter.DefectsTestLimitsData.DarkPixel.ToString();
                    DeadPixelsLimit.Text = defectsPresenter.DefectsTestLimitsData.DeadPixel.ToString();
                    TotalColumnsLimit.Text = defectsPresenter.DefectsTestLimitsData.TotalColumns.ToString();
                    XROIRangeLimit.Text = defectsPresenter.DefectsTestLimitsData.XROIRange.ToString();
                    YROIRangeLimit.Text = defectsPresenter.DefectsTestLimitsData.YROIRange.ToString();
                    darkPixelThresholdLimit.Text = defectsPresenter.DefectsTestLimitsData.DarkPixelThreshold.ToString();
                    TotalRowsLimit.Text = defectsPresenter.DefectsTestLimitsData.TotalRows.ToString();
                    clusterSize.Text= defectsPresenter.DefectsTestLimitsData.ClusterSize.ToString();
                    adjacentPixelsInCluster.Text = defectsPresenter.DefectsTestLimitsData.AdjacentPixelsInCluster.ToString();
                    DriftROILimit.Text = defectsPresenter.DefectsTestLimitsData.DriftROI.ToString();
                    DefectivePixelPerROILimit.Text = defectsPresenter.DefectsTestLimitsData.DefectivePixelsPerROI.ToString();
                    percentAboveMean.Text = defectsPresenter.DefectsTestLimitsData.PercentAboveMean.ToString();
                    percentBelowMean.Text = defectsPresenter.DefectsTestLimitsData.PercentBelowMean.ToString();
                    percentAboveMeanRows.Text = defectsPresenter.DefectsTestLimitsData.PercentAboveRows.ToString();
                    percentBelowMeanRows.Text = defectsPresenter.DefectsTestLimitsData.PercentBelowRows.ToString();
                    BadPixelsInRowLimit.Text = defectsPresenter.DefectsTestLimitsData.BadPixelsInRow.ToString();
                    BadPixelsInColLimit.Text = defectsPresenter.DefectsTestLimitsData.BadPixelsInCol.ToString();
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
        private void panel4_Resize(object sender, EventArgs e)
        {
            try
            {
                int newWidth = this.panel4.Width - 30;
                // Maintain the aspect ratio 
                double aspectRatio = 0.95;// (double)this.Size.Height / this.Size.Width;
                double dHeight = (double)newWidth * aspectRatio;
                int newHeight = (int)dHeight;
                while (newHeight > (panel4.Size.Height - 20))
                {
                    // Recalculate width and height
                    newWidth = newWidth - 1;
                    dHeight = (double)newWidth * aspectRatio;
                    newHeight = (int)dHeight;
                }
                defectsIntensityGraph.Width = newWidth;
                defectsIntensityGraph.Height = newHeight;
                minContrastSlideDefects.Height = maxContrastSlideDefects.Height = newHeight - 1200;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }

        private void maskROIListlistBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //KeyValuePair<string, List<Point>> tt = defPixelROIListPairsTemp.Where(p => p.Key == maskROIListlistBox.SelectedItem.ToString()).FirstOrDefault();
            //if (tt.Value.Count > 0)
            //{
            //    selectedMaskROICount.Text = tt.Value.Count.ToString();
            //    selectedMaskROIDefectsListView.Items.Clear();
            //    foreach (Point point in tt.Value)
            //    {
            //        //Infragistics.Win.UltraWinListView.UltraListViewItem ultraListViewItem2 = new Infragistics.Win.UltraWinListView.UltraListViewItem(point.ToString(), null, null);
            //        selectedMaskROIDefectsListView.Items.Add(point);
            //    }
            //}
        }

        private void maskROIListlistBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (defPixelROIListPairsTemp != null && defPixelROIListPairsTemp.Count > 0)
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
    }
}
