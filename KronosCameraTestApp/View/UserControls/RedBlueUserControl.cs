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
using Thermo.Kronos.Instrument.Camera.Interface;
using System.Reflection;
using System.Threading;
using System.IO;
using System.Configuration;
using NationalInstruments.UI;
using System.Security.AccessControl;
using KronosCameraTestApp.Helpers;
using Infragistics.Win.Misc;
using LabJack.LabJackUD;

namespace KronosCameraTestApp.View.UserControls
{
    public partial class RedBlueUserControl : UserControl
    {
        public RedBluePresenter redBluePresenter { get; set; }
        bool redBlueTestAcknowledged = false;
        public bool RedBlueTestAcknowledged
        {
            get { return redBlueTestAcknowledged; }
            set { redBlueTestAcknowledged = value; }
        }           
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string defaultFilePath = string.Empty;
        private CIDInterface cidInterface = null;
        private int currentRecord = 0;
        string TestResultImageFilePath = string.Empty;
        bool plotCursorMoved = false;
        private static uint xArraySize = 2048;
        private static uint yArraySize = 2048;
        //private double[,] zDataMasked = new double[xArraySize, yArraySize];
        private bool maskEnabled = false;
        private bool contrastSlideResize = false;
        private int zoomPanIndex = 0;
        private float zoomFactor = 1.5F;
        private bool zooming = false;
        string userMode = string.Empty;
        bool abortFromRedBlue = false;
        double blueAvgSignal = 0.0;
        double redAvgSignal = 0.0;
       AutoTestExposureSettings autoTestExposureSettings { get; set; }
        public bool AbortFromRedBlue
        {
            get { return abortFromRedBlue; }
            set { abortFromRedBlue = value; }
        }
        public CIDInterface CidInterface
        {
            get { return cidInterface; }
            set { cidInterface = value; }
        }
        public RedBlueUserControl()
        {
            InitializeComponent();
        }
        public RedBlueUserControl(RedBluePresenter redBluePresenter, string defaultFilePath, string UserMode)
        {
            InitializeComponent();
            this.redBluePresenter = redBluePresenter;
            this.userMode = UserMode;
            this.defaultFilePath = defaultFilePath;
            redBlueTestAcknowledged = false;
            intensityCursor1.LabelVisible = true;
            intensityCursor2.LabelVisible = true;
            intensityCursor3.LabelVisible = true;
        }
        private async void RedBlueUserControl_Load(object sender, EventArgs e)
        {
            try
            {
                if (autoTestExposureSettings == null)
                {
                    autoTestExposureSettings = new AutoTestExposureSettings("RBT", cidInterface);
                    autoTestExposureSettings.GetData();
                    redBluePresenter.ExposureDataList = autoTestExposureSettings.ExposureList;
                    redBluePresenter.SubarrayDataModelList = autoTestExposureSettings.SubarrayList;
                    redBluePresenter.ExposureDataModel = autoTestExposureSettings.exposureDataPresenter.ExposureDataModel;                   
                    ultraExplorerBarContainerControl2.Controls.Add(autoTestExposureSettings);
                    UpdateLimitsViewWithModelOnLoad();
                }
                if (redBluePresenter.ExposureDataList.Count == 0)
                {
                    await redBluePresenter.GetRedBlueData(cidInterface);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Loading Exposure Data", "Load Exposure Data", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task RunAutoRedBlueTest(CancellationTokenSource cts, bool AutoPretest)
        {
            try
            {
                this.intensityPlot1.ClearData();
                this.intensityPlot2.ClearData();
                this.intensityPlot3.ClearData();
                redBlueIntensityGraph.Refresh();
                blueIntensityGraph.Refresh();
                differenceIntensityGraph.Refresh();
                UVMeanResult.Appearance.BorderColor2 = UVMeanResult.Appearance.BorderColor = Color.Black;
                UVMeanResult.Text = string.Empty;

                lblRedLedAvgSignal.Appearance.BorderColor2 = lblRedLedAvgSignal.Appearance.BorderColor = Color.Black;
                lblRedLedAvgSignal.Text = string.Empty;

                lblBlueLedAvgSignal.Appearance.BorderColor2 = lblBlueLedAvgSignal.Appearance.BorderColor = Color.Black;
                lblBlueLedAvgSignal.Text = string.Empty;

                if (cts.IsCancellationRequested)
                    return;
                if (autoTestExposureSettings == null)
                {
                    autoTestExposureSettings = new AutoTestExposureSettings("RBT", cidInterface);
                    autoTestExposureSettings.GetData();
                    redBluePresenter.ExposureDataList = autoTestExposureSettings.ExposureList.Take(2).ToList();
                    redBluePresenter.SubarrayDataModelList = autoTestExposureSettings.SubarrayList.Take(2).ToList();
                    redBluePresenter.ExposureDataModel = autoTestExposureSettings.exposureDataPresenter.ExposureDataModel;
                    ultraExplorerBarContainerControl2.Controls.Add(autoTestExposureSettings);
                }
                if (redBluePresenter.ExposureDataList.Count == 0 || TestAppHelper.UserModeChanged)
                {
                    await redBluePresenter.GetRedBlueData(cidInterface);
                }

                //((KronosTestAppMainForm)ParentForm.MdiParent).ultraStatusBar1.Panels["EnvStatus"].Visible = true;
                await RunRedBlueTest(cts.Token);
                await PlotRedBlueData(cts);               
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
                    ledPassFail.Value = false;
                    ledPassFail.OffColor = Color.Red;
                    redBluePresenter.UVTestPassed = false;                  
                    ((KronosTestAppMainForm)ParentForm.MdiParent).mainFormDockManager.DockAreas[1].Closed = false;
                    ((KronosTestAppMainForm)ParentForm.MdiParent).mainFormDockManager.DockAreas[1].Activate();
                    ((KronosTestAppMainForm)ParentForm.MdiParent).mainFormDockManager.DockAreas[1].Pin();
                    ((KronosTestAppMainForm)ParentForm.MdiParent).firmwareLoggingTextBox.AppendText(Environment.NewLine);
                    ((KronosTestAppMainForm)ParentForm.MdiParent).firmwareLoggingTextBox.AppendText("LabJack open error UV Test maybe impacted.");
                    ((KronosTestAppMainForm)ParentForm.MdiParent).firmwareLoggingTextBox.AppendText(Environment.NewLine);
                    RedBlueFail_Click(null, null);
                    MessageBox.Show("LabJack open error UV Test maybe impacted.", "LabJack Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }                
                redBluePresenter.UVInterval = 10;
                Thread.Sleep(5000);

                redBluePresenter.ExposureDataList = autoTestExposureSettings.ExposureList.Skip(2).ToList();
                redBluePresenter.SubarrayDataModelList = autoTestExposureSettings.SubarrayList.Skip(2).ToList();
                
                await RunUVTest(cts.Token);

                await PlotUVData(cts);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task RunRedBlueTest(CancellationToken ct)
        {
            try
            {
                if (ct.IsCancellationRequested)
                    return;
                log.Debug("Run Cross Talk test.");
                await redBluePresenter.runRedBlueTest(cidInterface, ct, userMode);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task RunUVTest(CancellationToken ct)
        {
            try
            {
                log.Debug("Run Red&Blue test.");
                if (ct.IsCancellationRequested)
                    return;
                await redBluePresenter.runUVTest(cidInterface, ct, userMode);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);               
            }
        }
        private async Task PlotUVData(CancellationTokenSource ct)
        {
            try
            {
                if (ct.IsCancellationRequested)
                    return;
                await redBluePresenter.CalculateUVTest(ct.Token);
                UVMeanResult.Text = redBluePresenter.UVMean.ToString("F1");
                if (redBluePresenter.UVTestPassed)
                {
                    UVMeanResult.Appearance.BorderColor2 = UVMeanResult.Appearance.BorderColor = Color.LimeGreen;
                    redBluePresenter.UVTestPassed = true;
                }
                else
                {
                    UVMeanResult.Appearance.BorderColor2 = UVMeanResult.Appearance.BorderColor = Color.Red;                  
                    redBluePresenter.UVTestPassed = false;                  
                }

                if(redAvgSignal > 100)
                {
                    lblRedLedAvgSignal.Appearance.BorderColor2 = lblRedLedAvgSignal.Appearance.BorderColor = Color.LimeGreen;
                }
                else
                {
                    lblRedLedAvgSignal.Appearance.BorderColor2 = lblRedLedAvgSignal.Appearance.BorderColor = Color.Red;
                }
                if (blueAvgSignal > 100)
                {
                    lblBlueLedAvgSignal.Appearance.BorderColor2 = lblBlueLedAvgSignal.Appearance.BorderColor = Color.LimeGreen;
                }
                else
                {
                    lblBlueLedAvgSignal.Appearance.BorderColor2 = lblBlueLedAvgSignal.Appearance.BorderColor = Color.Red;
                }

                if(!redBluePresenter.UVTestPassed || redAvgSignal < 100 || blueAvgSignal < 100)
                {
                    ledPassFail.Value = false;
                    ledPassFail.OffColor = Color.Red;
                    RedBlueFail_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
              
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
            }
        }
        private void PlotRedImageData()
        {
            try
            {
                if (redBluePresenter.ZDataRedDouble != null)
                {
                    redBlueIntensityGraph.ClearData();
                    redBlueIntensityGraph.Plot(redBluePresenter.ZDataRedDouble);
                    setRedGraphContrast(redBluePresenter.ZDataRedDouble);
                    redBlueIntensityGraph_AfterMoveCursor(null, null);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void PlotBlueImageData()
        {
            try
            {
                if (redBluePresenter.ZDataBlueDouble != null)
                {
                    blueIntensityGraph.ClearData();
                    blueIntensityGraph.Plot(redBluePresenter.ZDataBlueDouble);
                    setBlueGraphContrast(redBluePresenter.ZDataBlueDouble);
                    BlueIntensityGraph_AfterMoveCursor(null, null);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
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
            }
        }
        public void SaveTestResultImage(string testResultLocalFilePath, string testResultsServerPath, string localFileTimeStamp, string serverPathToDB, bool saveAutoResultsToDisk)
        {
            try
            {
                this.Dock = DockStyle.None;
                Image img = null;
                Image img1 = null;
                Image img2 = null;
                int outputImageWidth = 0;
                int outputImageHeight = 0;
                Bitmap outputImage = null;
                int height = 0, width = 0;
                img = this.redBlueIntensityGraph.ToImage();
                img1 = this.blueIntensityGraph.ToImage();
                img2 = this.differenceIntensityGraph.ToImage();
                List<Image> imageList = new List<Image>();
                imageList.Add(img);
                imageList.Add(img1);
                imageList.Add(img2);
                List<int> imageHeights = new List<int>();
                for (int i = 0; i < imageList.Count; i++)
                {
                    height = Math.Max(height, imageList[i].Height);
                }
                outputImageHeight = height;
                outputImageWidth = img.Width + img1.Width + img2.Width;
                outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(outputImage))
                {
                    for (int i = 0; i < imageList.Count; ++i)
                    {
                        g.DrawImage(imageList[i], new Point(width, 0));
                        width += imageList[i].Width;
                    }
                }
                outputImage.Save(testResultLocalFilePath + "\\RedBlueTestResultImage_" + localFileTimeStamp + ".png", System.Drawing.Imaging.ImageFormat.Png);
                this.Dock = DockStyle.Fill;
                if (ConfigurationManager.AppSettings["EndClient"] != "Thermo")
                {
                    TestAppHelper.saveFileInAzure(outputImage, "RedBlue.png");
                }
                else
                {
                    if (saveAutoResultsToDisk)
                    {
                        outputImage.Save(testResultsServerPath + "\\RedBlueTestResultImage_" + localFileTimeStamp + ".png", System.Drawing.Imaging.ImageFormat.Png);
                        redBluePresenter.TestResultImagePath = serverPathToDB + "\\RedBlueTestResultImage_" + localFileTimeStamp + ".png";
                    }
                    else
                        redBluePresenter.TestResultImagePath = string.Empty;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task SaveRedBlueTestResults(CancellationTokenSource cts, bool AutoPretest)
        {
            try
            {
                if (!AutoPretest)
                {
                    if (redBluePresenter.RedBlueTestPassed)
                    {
                        await redBluePresenter.WriteRedBlueTestResultToDB(cts.Token, TestResultImageFilePath);
                    }
                }
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
                    zoomPanIndex++;
                    int subarrayRegion = (70 / zoomPanIndex);
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
                Range contrastRange = new Range(minimumSignal, maximumSignal);
                minContrastSlide.Range = contrastRange;
                minContrastSlide.Value = contrastRange.Minimum;
                maxContrastSlide.Range = contrastRange;
                maxContrastSlide.Value = contrastRange.Maximum;
                this.intensityGraphColorScale.Range =
                    new NationalInstruments.UI.Range(minimumSignal, maximumSignal);
            }
            redAvgSignal = maximumSignal + minimumSignal / 2;
            lblRedLedAvgSignal.Text = redAvgSignal.ToString();
            if (redAvgSignal > 100)
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
                Range contrastRange = new Range(minimumSignal, maximumSignal);
                minContrastSlide.Range = contrastRange;
                minContrastSlide.Value = contrastRange.Minimum;
                maxContrastSlide.Range = contrastRange;
                maxContrastSlide.Value = contrastRange.Maximum;
                this.colorScale1.Range =
                   new NationalInstruments.UI.Range(minimumSignal, maximumSignal);
            }
            blueAvgSignal = maximumSignal + minimumSignal / 2;
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
        private void redBlueIntensityGraph_AfterMoveCursor(object sender, NationalInstruments.UI.AfterMoveIntensityCursorEventArgs e)
        {
            try
            {
                plotCursorMoved = true;
                if (redBluePresenter != null)
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
        private async void enableMapButton_Click(object sender, EventArgs e)
        {
            try
            {   
                if (enableMapButton.Text == "Enable")
                {
                    enableMapButton.Text = "Disable";
                    maskEnabled = true;
                    if ( (redBluePresenter.ZDataRedDouble != null))
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
                    enableMapButton.Text = "Enable";
                    maskEnabled = false;
                    if ( (redBluePresenter.ZDataRedDouble != null))
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
        private void minContrastSlide_AfterChangeValue(object sender, NationalInstruments.UI.AfterChangeNumericValueEventArgs e)
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
        private void maxContrastSlide_AfterChangeValue(object sender, NationalInstruments.UI.AfterChangeNumericValueEventArgs e)
        {
            try
            {
                minContrastSlide_AfterChangeValue(null, null);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            } 
        }
        private void BlueIntensityGraph_AfterMoveCursor(object sender, AfterMoveIntensityCursorEventArgs e)
        {
            try
            {
                plotCursorMoved = true;
                if (redBluePresenter != null)
                {
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
                plotCursorMoved = true;
                if (redBluePresenter != null)
                {
                    int xpos = Convert.ToInt32(intensityCursor3.XPosition);
                    int ypos = Convert.ToInt32(intensityCursor3.YPosition);
                    if ((redBluePresenter.ZDataRedDouble != null) && (redBluePresenter.ZDataBlueDouble != null))
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
                    if (intensityCursor3.XPosition <= 2047)
                    {
                        intensityCursor3.XPosition = intensityCursor3.XPosition + 1;
                    }
                    else
                    {
                        intensityCursor3.XPosition = 2048;
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
        private void RedBluePass_Click(object sender, EventArgs e)
        {
            try
            {
                RedBluePass.Checked = true;
                redBluePresenter.RedBlueTestPassed = true;
                RedBlueFail.Checked = false;
                ledPassFail.Value = true;
                Form ParentForm = this.ParentForm;
                redBlueTestAcknowledged = true;
                ((KronosTestAppMainForm)ParentForm.MdiParent).RedBlueUserControlPassNormalTile();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void RedBlueFail_Click(object sender, EventArgs e)
        {
            try
            {
                RedBlueFail.Checked = true;
                RedBluePass.Checked = false;
                redBluePresenter.RedBlueTestPassed = false;
                ledPassFail.Value = false;
                ledPassFail.OffColor = Color.Red;
                Form ParentForm = this.ParentForm;
                abortFromRedBlue = true;
                redBlueTestAcknowledged = true;
                ((KronosTestAppMainForm)ParentForm.MdiParent).AbortAutoTest(false,"Red&Blue");
                ((KronosTestAppMainForm)ParentForm.MdiParent).RedBlueUserControlPassNormalTile();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void UpdateLimitsViewWithModelOnLoad()
        {
            try
            {
                List<int> limtIDs = new List<int>();
                if (redBluePresenter.LEDCalibrationLimitsData == null)
                {
                    await redBluePresenter.GetLEDCalibrationLimits();
                }
                limtIDs = (from ecos in redBluePresenter.LEDCalibrationLimitsDataList select ecos.LimitsID).ToList();
                if (!string.IsNullOrWhiteSpace(TestAppHelper.SelectedECONumber) && (userMode.Equals("Engineering") || userMode.Equals("Admin")))
                {
                    redBluePresenter.LEDCalibrationLimitsData = redBluePresenter.LEDCalibrationLimitsDataList.Where(limit => limit.ECONumber.Equals(TestAppHelper.SelectedECONumber)).FirstOrDefault();
                    LimitID.Text = redBluePresenter.LEDCalibrationLimitsData.LimitsID.ToString();
                }
                else
                {
                    LimitID.Text = limtIDs[0].ToString();
                    redBluePresenter.LEDCalibrationLimitsData = redBluePresenter.LEDCalibrationLimitsDataList.Where(limit => limit.LimitsID.Equals(limtIDs[0])).FirstOrDefault();
                }
                if (redBluePresenter.LEDCalibrationLimitsData != null)
                {
                    LowerLEDLimit.Text = redBluePresenter.LEDCalibrationLimitsData.LowerUVLEDLimit.ToString();
                    UpperLEDLimit.Text = redBluePresenter.LEDCalibrationLimitsData.UpperUVLEDLimit.ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
