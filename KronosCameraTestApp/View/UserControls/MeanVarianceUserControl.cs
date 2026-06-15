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
using System.Threading;
using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using System.Reflection;
using System.IO;
using System.Configuration;
using System.Security.AccessControl;
using Infragistics.Win.Misc;
namespace KronosCameraTestApp.View.UserControls
{
    public partial class MeanVarianceUserControl : UserControl
    {
        private MeanVariancePresenter meanVariancePresenter;
        public MeanVariancePresenter MeanVariancePresenter
        {
            get { return meanVariancePresenter; }
            set { meanVariancePresenter = value; }
        }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CIDInterface cidInterface = null;
        private int currentRecord = 0;
        string defaultFilePath = string.Empty;
        string TestResultImageFilePath = string.Empty;
        string userMode = string.Empty;
        AutoTestExposureSettings autoTestExposureSettings { get; set; }
        public CIDInterface CidInterface
        {
            get { return cidInterface; }
            set { cidInterface = value; }
        }
        public MeanVarianceUserControl()
        {
            InitializeComponent();
        }
        public MeanVarianceUserControl(MeanVariancePresenter meanVariancePresenter, string defaultFilePath, string UserMode)
        {
            InitializeComponent();
            this.meanVariancePresenter = meanVariancePresenter;
            this.userMode = UserMode;
            this.defaultFilePath = defaultFilePath;
        }
        private async void MeanVarianceUserControl_Load(object sender, EventArgs e)
        {
            try
            {
                if (autoTestExposureSettings == null)
                {
                    autoTestExposureSettings = new AutoTestExposureSettings("MVT", cidInterface);
                    autoTestExposureSettings.GetData();
                    meanVariancePresenter.ExposureDataList = autoTestExposureSettings.ExposureList;
                    meanVariancePresenter.SubarrayDataModelList = autoTestExposureSettings.SubarrayList;
                    meanVariancePresenter.ExposureDataModel = autoTestExposureSettings.exposureDataPresenter.ExposureDataModel;
                    meanVariancePresenter.SubarrayDataModel = autoTestExposureSettings.exposureDataPresenter.SubarrayDataModel;
                    ultraExplorerBarContainerControl2.Controls.Add(autoTestExposureSettings);
                }
                if (meanVariancePresenter.MeanVarianceTestLimitsDataList.Count == 0 || TestAppHelper.UserModeChanged)
                {
                    await meanVariancePresenter.GetMVLimits();
                }
                UpdateLimitsViewWithModelOnLoad();
                if (meanVariancePresenter.ExposureDataList.Count == 0 || TestAppHelper.UserModeChanged)
                {
                    await meanVariancePresenter.GetMVData(cidInterface);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task RunAutoMVTest(CancellationTokenSource cts, bool AutoPretest)
        {
            try
            {
                meanVarianceScatterPlot.ClearData();
                meanVarianceScatterGraph.ClearData();                
                meanVarianceScatterGraph.Refresh();
                meanofROI1minus2Plot.ClearData();
                varianceOfROI1minus2Plot.ClearData();
                linearCurveFitOfVarPlot.ClearData();
                meanVarianceWaveformGraph.ClearData();
                meanVarianceWaveformGraph.Refresh();
                ledPassFail.Value = false;
                if (cts.IsCancellationRequested)
                    return;
                if(autoTestExposureSettings==null)
                {
                    autoTestExposureSettings = new AutoTestExposureSettings("MVT", cidInterface);
                    autoTestExposureSettings.GetData();
                    meanVariancePresenter.ExposureDataList = autoTestExposureSettings.ExposureList;
                    meanVariancePresenter.SubarrayDataModelList = autoTestExposureSettings.SubarrayList;
                    meanVariancePresenter.ExposureDataModel = autoTestExposureSettings.exposureDataPresenter.ExposureDataModel;
                    meanVariancePresenter.SubarrayDataModel = autoTestExposureSettings.exposureDataPresenter.SubarrayDataModel;
                    ultraExplorerBarContainerControl2.Controls.Add(autoTestExposureSettings);
                }
                if (meanVariancePresenter.MeanVarianceTestLimitsDataList.Count == 0 || TestAppHelper.UserModeChanged)
                {
                    await meanVariancePresenter.GetMVLimits();
                }
                UpdateLimitsViewWithModelOnLoad();              
                meanVariancePresenter.MeanVarianceRepeats = 5;
                await RunMVTest(cts.Token);
                await PlotMVData(cts);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task RunMVTest(CancellationToken ct)
        {
            try
            {
                if (ct.IsCancellationRequested)
                    return;
                log.Debug("Run Mean Variance test.");
                await meanVariancePresenter.runMeanVarianceTest(cidInterface, ct,userMode);                
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task PlotMVData(CancellationTokenSource ct)
        {
            try
            {
                if (ct.IsCancellationRequested)
                    return;
                await meanVariancePresenter.CalculateMeanVariance(ct.Token);
                if (ct.IsCancellationRequested)
                    return;
                if (!cidInterface.IsConnected())
                    return;
                if (meanVariancePresenter.CorrectedMeanNew != null && meanVariancePresenter.CorrectedVarianceNew != null)
                    meanVarianceScatterGraph.PlotXY(meanVariancePresenter.CorrectedMeanNew, meanVariancePresenter.CorrectedVarianceNew);
                if (meanVariancePresenter.LinearCurveFit != null)
                    linearCurveFitOfVarPlot.PlotY(meanVariancePresenter.LinearCurveFit);
                GainResult.Text = meanVariancePresenter.Gain.ToString("0.00");
                if (meanVariancePresenter.MeanVarianceTestPassed)
                {
                    GainResult.Appearance.BorderColor = GainResult.Appearance.BorderColor2 = Color.LimeGreen;
                    ledPassFail.Value = true;
                }

                else
                {
                    ledPassFail.Value = false;
                    ledPassFail.OffColor = GainResult.Appearance.BorderColor = GainResult.Appearance.BorderColor2 = Color.Red;
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
                int outputImageWidth = 0;
                int outputImageHeight = 0;
                Bitmap outputImage = null;   
                img = this.meanVarianceScatterGraph.ToImage();
                img1 = this.meanVarianceWaveformGraph.ToImage();
                outputImageWidth = img.Width > img1.Width ? img.Width : img1.Width;
                outputImageHeight = img.Height + img1.Height + 1;
                outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics graphics = Graphics.FromImage(outputImage))
                {
                    graphics.DrawImage(img, new Rectangle(new Point(), img.Size),
                           new Rectangle(new Point(), img.Size), GraphicsUnit.Pixel);
                    graphics.DrawImage(img1, new Rectangle(new Point(0, img.Height + 1), img1.Size),
                            new Rectangle(new Point(), img1.Size), GraphicsUnit.Pixel);
                }
                outputImage.Save(testResultLocalFilePath + "\\MeanVarianceTestResultImage_" + localFileTimeStamp + ".png", System.Drawing.Imaging.ImageFormat.Png);
                this.Dock = DockStyle.Fill;
                if (ConfigurationManager.AppSettings["EndClient"] != "Thermo")
                {
                    TestAppHelper.saveFileInAzure(outputImage, "MeanVariance.png");
                }                   
                else
                {
                    if (saveAutoResultsToDisk)
                    {
                        outputImage.Save(testResultsServerPath + "\\MeanVarianceTestResultImage_" + localFileTimeStamp + ".png", System.Drawing.Imaging.ImageFormat.Png);
                        meanVariancePresenter.TestResultImagePath = serverPathToDB + "\\MeanVarianceTestResultImage_" + localFileTimeStamp + ".png";
                    }
                    else
                        meanVariancePresenter.TestResultImagePath = string.Empty;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }       
        public async Task ClearMVGraphs()
        {
            await Task.Run(() =>
            {
                meanofROI1minus2Plot.ClearData();
                varianceOfROI1minus2Plot.ClearData();
                linearCurveFitOfVarPlot.ClearData();
                meanVarianceScatterGraph.ClearData();
            });
        }
        private async void UpdateLimitsViewWithModelOnLoad()
        {
            try
            {
                List<int> limtIDs = new List<int>();
                if(meanVariancePresenter.MeanVarianceTestLimitsData== null)
                {
                    await meanVariancePresenter.GetMVLimits();
                }
                limtIDs = (from ecos in meanVariancePresenter.MeanVarianceTestLimitsDataList select ecos.LimitsID).ToList();
                if (!string.IsNullOrWhiteSpace(TestAppHelper.SelectedECONumber) && (userMode.Equals("Engineering") || userMode.Equals("Admin")))
                {                    
                    meanVariancePresenter.MeanVarianceTestLimitsData = meanVariancePresenter.MeanVarianceTestLimitsDataList.Where(limit => limit.ECONumber.Equals(TestAppHelper.SelectedECONumber)).FirstOrDefault();
                    LimitID.Text = meanVariancePresenter.MeanVarianceTestLimitsData.LimitsID.ToString(); 
                }                   
                else
                {
                    LimitID.Text = limtIDs[0].ToString();
                    meanVariancePresenter.MeanVarianceTestLimitsData = meanVariancePresenter.MeanVarianceTestLimitsDataList.Where(limit => limit.LimitsID.Equals(limtIDs[0])).FirstOrDefault();
                }
                if (meanVariancePresenter.MeanVarianceTestLimitsData != null)
                {                    
                    ConversionFactorNominalLimit.Text = meanVariancePresenter.MeanVarianceTestLimitsData.ConversionFactorNominal.ToString();
                    ConversionFactorLowerLimit.Text = meanVariancePresenter.MeanVarianceTestLimitsData.ConversionFactorLowerLimit.ToString();
                    ConversionFactorUpperLimit.Text = meanVariancePresenter.MeanVarianceTestLimitsData.ConversionFactorUpperLimit.ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }       
    }
}
