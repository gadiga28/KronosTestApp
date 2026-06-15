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
using KronosCameraTestApp.Helpers;
using System.Security.AccessControl;
using Infragistics.Win.Misc;
namespace KronosCameraTestApp.View.UserControls
{
    public partial class NoiseVsNDROUserControl : UserControl
    {
        public NoiseVsNDROPresenter noiseVsNDROPresenter { get; set; }       
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string defaultFilePath = string.Empty;
        private CIDInterface cidInterface = null;
        private int currentRecord = 0;
        string TestResultImageFilePath = string.Empty;
        string userMode = string.Empty;
        public CIDInterface CidInterface
        {
            get { return cidInterface; }
            set { cidInterface = value; }
        }
        AutoTestExposureSettings autoTestExposureSettings { get; set; }
        public NoiseVsNDROUserControl()
        {
            InitializeComponent();
        }
        public NoiseVsNDROUserControl(NoiseVsNDROPresenter noiseVsNDROPresenter, string defaultFilePath, string UserMode)
        {
            InitializeComponent();
            this.noiseVsNDROPresenter = noiseVsNDROPresenter;
            this.defaultFilePath = defaultFilePath;
            this.userMode = UserMode;
        }
        private async void NoiseVsNDROUserControl_Load(object sender, EventArgs e)
        {
            try
            {
                if (autoTestExposureSettings == null)
                {
                    autoTestExposureSettings = new AutoTestExposureSettings("RNT", cidInterface);
                    autoTestExposureSettings.GetData();
                    noiseVsNDROPresenter.ExposureDataList = autoTestExposureSettings.ExposureList;
                    noiseVsNDROPresenter.SubarrayDataModelList = autoTestExposureSettings.SubarrayList;
                    noiseVsNDROPresenter.ExposureDataModel = autoTestExposureSettings.exposureDataPresenter.ExposureDataModel;
                    noiseVsNDROPresenter.SubarrayDataModel = autoTestExposureSettings.exposureDataPresenter.SubarrayDataModel;
                    ultraExplorerBarContainerControl2.Controls.Add(autoTestExposureSettings);
                }
                if (noiseVsNDROPresenter.NoiseVsNDROsLimitsDataList.Count == 0 || TestAppHelper.UserModeChanged)
                {
                    await noiseVsNDROPresenter.GetLimits();
                }
                UpdateLimitsViewWithModelOnLoad();
                if (noiseVsNDROPresenter.ExposureDataList.Count == 0 || TestAppHelper.UserModeChanged)
                {
                    await noiseVsNDROPresenter.GetNoiseNDROData(cidInterface);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task RunAutoNoiseVsNDROsTest(CancellationTokenSource cts, bool AutoPretest)
        {
            try
            {
                this.measuredNoiseScatterPlot.ClearData();
                this.log20ScatterPlot.ClearData();
                ledPassFail.Value = false;
                readNoiseScatterGraph.Refresh();
                if (cts.IsCancellationRequested)
                    return;
                if (autoTestExposureSettings == null)
                {
                    autoTestExposureSettings = new AutoTestExposureSettings("RNT", cidInterface);
                    autoTestExposureSettings.GetData();
                    noiseVsNDROPresenter.ExposureDataList = autoTestExposureSettings.ExposureList;
                    noiseVsNDROPresenter.SubarrayDataModelList = autoTestExposureSettings.SubarrayList;
                    noiseVsNDROPresenter.ExposureDataModel = autoTestExposureSettings.exposureDataPresenter.ExposureDataModel;
                    noiseVsNDROPresenter.SubarrayDataModel = autoTestExposureSettings.exposureDataPresenter.SubarrayDataModel;
                    ultraExplorerBarContainerControl2.Controls.Add(autoTestExposureSettings);
                }
                if (noiseVsNDROPresenter.ExposureDataList.Count == 0)
                {
                    await noiseVsNDROPresenter.GetNoiseNDROData(cidInterface);
                }
                    await RunNoiseVsNDROTest(cts.Token);
                    await PlotNoiseVsNDROData(cts);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task RunNoiseVsNDROTest(CancellationToken ct)
        {
            try
            {
                if (ct.IsCancellationRequested)
                    return;
                log.Debug("Run NoiseVsNDRO test.");
                await noiseVsNDROPresenter.runNoiseNDROTest(cidInterface, ct, userMode);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task PlotNoiseVsNDROData(CancellationTokenSource ct)
        {
            try
            {
                if (ct.IsCancellationRequested)
                    return;
                await noiseVsNDROPresenter.CalculateNoiseNDRO(ct.Token);
                if (ct.IsCancellationRequested)
                    return;
                if (!cidInterface.IsConnected())
                    return;
                double[] exposureNDROs = { 1, 2, 4, 8, 16, 32, 64, 128 };
				if (noiseVsNDROPresenter.R2forX != null && noiseVsNDROPresenter.TheoryValue !=null)
                    log20ScatterPlot.PlotXYAppend(exposureNDROs, noiseVsNDROPresenter.TheoryValue);
                if (noiseVsNDROPresenter.ExposureNDRO != null && noiseVsNDROPresenter.Noise != null)
                readNoiseScatterGraph.PlotXYAppend(exposureNDROs, noiseVsNDROPresenter.Noise);
                SnglNoiseResult.Text = String.Format("{0:0.00}", noiseVsNDROPresenter.SignalReadNoise);
                if (noiseVsNDROPresenter.SignalReadNoise >= noiseVsNDROPresenter.NoiseVsNDROSLimitsData.SnglNoiseLowerLimit &&
                    noiseVsNDROPresenter.SignalReadNoise <= noiseVsNDROPresenter.NoiseVsNDROSLimitsData.SnglNoiseUpperLimit)
                {
                    SnglNoiseResult.Appearance.BorderColor = SnglNoiseResult.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    SnglNoiseResult.Appearance.BorderColor = SnglNoiseResult.Appearance.BorderColor2 = Color.Red;
                }
                NDRONoiseResult.Text = String.Format("{0:0.00}", noiseVsNDROPresenter.NDROReadNoise);
                R2CorrelationResult.Text = noiseVsNDROPresenter.R2Correlation.ToString();
                if (noiseVsNDROPresenter.R2Correlation >= noiseVsNDROPresenter.NoiseVsNDROSLimitsData.RPower2Correlation)
                {
                    R2CorrelationResult.Appearance.BorderColor = R2CorrelationResult.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    R2CorrelationResult.Appearance.BorderColor = R2CorrelationResult.Appearance.BorderColor2 = Color.Red;
                }
                if (noiseVsNDROPresenter.NoiseVsNDROsTestPassed)
                    ledPassFail.Value = true;
                else
                {
                    ledPassFail.Value = false;
                    ledPassFail.OffColor = Color.Red;
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
                this.Dock = DockStyle.None;
                Image img = null;
                int outputImageWidth = 0;
                int outputImageHeight = 0;
                Bitmap outputImage = null;
                img = this.readNoiseScatterGraph.ToImage();
                outputImageWidth = img.Width;
                outputImageHeight = img.Height;
                outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics graphics = Graphics.FromImage(outputImage))
                {
                    graphics.DrawImage(img, new Rectangle(new Point(), img.Size),
                           new Rectangle(new Point(), img.Size), GraphicsUnit.Pixel);                   
                }
                outputImage.Save(testResultLocalFilePath + "\\NoiseVsNDROsTestResultImage_" + localFileTimeStamp + ".png", System.Drawing.Imaging.ImageFormat.Png);
                this.Dock = DockStyle.Fill;
                if (ConfigurationManager.AppSettings["EndClient"] != "Thermo")
                {
                    TestAppHelper.saveFileInAzure(outputImage, "NoiseVsNDROs.png");
                }                    
                else
                {
                    if (savetoNetwork)
                    {
                        outputImage.Save(testResultsServerPath + "\\NoiseVsNDROsTestResultImage_" + localFileTimeStamp + ".png", System.Drawing.Imaging.ImageFormat.Png);
                        noiseVsNDROPresenter.TestResultImagePath = serverPathToDB + "\\NoiseVsNDROsTestResultImage_" + localFileTimeStamp + ".png";
                    }
                    else
                        noiseVsNDROPresenter.TestResultImagePath = string.Empty;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task SaveNoiseVsNDROsTestResults(CancellationTokenSource cts, bool AutoPretest)
        {
            try
            {
                if (!AutoPretest)
                {
                    if (noiseVsNDROPresenter.NoiseVsNDROsTestPassed)
                    {
                        ledPassFail.Value = true;
                        await noiseVsNDROPresenter.WriteNoiseNDROTestResultToDB(cts.Token, TestResultImageFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task ClearNoiseNDROGraphs()
        {
            await Task.Run(() =>
            {              
                this.measuredNoiseScatterPlot.ClearData();
                this.log20ScatterPlot.ClearData();
            });
        }    
        private void UpdateLimitsViewWithModelOnLoad()
        {
            try
            {
                List<int> limtIDs = new List<int>();
                limtIDs = (from limits in noiseVsNDROPresenter.NoiseVsNDROsLimitsDataList select limits.LimitsID).ToList();
                if (!string.IsNullOrWhiteSpace(TestAppHelper.SelectedECONumber) && (userMode.Equals("Engineering") || userMode.Equals("Admin")))
                {
                    noiseVsNDROPresenter.NoiseVsNDROSLimitsData = noiseVsNDROPresenter.NoiseVsNDROsLimitsDataList.Where
                        (limit => limit.ECONumber.Equals(TestAppHelper.SelectedECONumber)).FirstOrDefault();
                    LimitID.Text = noiseVsNDROPresenter.NoiseVsNDROSLimitsData.LimitsID.ToString();
                }
                else
                {
                    LimitID.Text = limtIDs[0].ToString();
                    noiseVsNDROPresenter.NoiseVsNDROSLimitsData = noiseVsNDROPresenter.NoiseVsNDROsLimitsDataList.Where(limit => limit.LimitsID.Equals(limtIDs[0])).FirstOrDefault();
                }
                if (noiseVsNDROPresenter.NoiseVsNDROSLimitsData !=null)
                {
                    SnglNoiseLimit.Text = noiseVsNDROPresenter.NoiseVsNDROSLimitsData.SnglNoiseMax.ToString();
                    SnglNoiseLowerLimit.Text = noiseVsNDROPresenter.NoiseVsNDROSLimitsData.SnglNoiseLowerLimit.ToString();
                    SnglNoiseUpperLimit.Text = noiseVsNDROPresenter.NoiseVsNDROSLimitsData.SnglNoiseUpperLimit.ToString();
                    R2CorrelationLimit.Text = noiseVsNDROPresenter.NoiseVsNDROSLimitsData.RPower2Correlation.ToString();
                    PowerFitLowerimit.Text = noiseVsNDROPresenter.NoiseVsNDROSLimitsData.PowerFitLower.ToString();
                    PowerFitUpperLimit.Text = noiseVsNDROPresenter.NoiseVsNDROSLimitsData.PowerFitUpper.ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        }   
    }
