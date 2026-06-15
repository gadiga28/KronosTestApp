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
using Infragistics.Win.Misc;
namespace KronosCameraTestApp.View.UserControls
{
    public partial class DarkCurrentUserControl : UserControl
    {
        public DarkCurrentPresenter darkCurrentPresenter { get; set; }       
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string defaultFilePath = string.Empty;
        CID821_Data cid821Data = new CID821_Data();
        private CIDInterface cidInterface = null;
        string TestResultImageFilePath = string.Empty;
        string userMode = string.Empty;
        AutoTestExposureSettings autoTestExposureSettings { get; set; }
        public CIDInterface CidInterface
        {
            get { return cidInterface; }
            set { cidInterface = value; }
        }
        private int currentRecord = 0;
        public DarkCurrentUserControl()
        {
            InitializeComponent();
        }
        public DarkCurrentUserControl(DarkCurrentPresenter darkCurrentPresenter, string defaultFilePath, string UserMode)
        {
            InitializeComponent();
            this.defaultFilePath = defaultFilePath;
            this.userMode = UserMode;
            this.darkCurrentPresenter = darkCurrentPresenter;
        }
        private async void DarkCurrentUserControl_Load(object sender, EventArgs e)
        {
            try
            {               
                if (autoTestExposureSettings == null)
                {
                    autoTestExposureSettings = new AutoTestExposureSettings("DDT", cidInterface);
                    autoTestExposureSettings.GetData();
                    darkCurrentPresenter.ExposureDataList = autoTestExposureSettings.ExposureList;
                    darkCurrentPresenter.SubarrayDataModelList = autoTestExposureSettings.SubarrayList;
                    darkCurrentPresenter.ExposureDataModel = autoTestExposureSettings.exposureDataPresenter.ExposureDataModel;
                    darkCurrentPresenter.SubarrayDataModel = autoTestExposureSettings.exposureDataPresenter.SubarrayDataModel;
                    ultraExplorerBarContainerControl2.Controls.Add(autoTestExposureSettings);
                }
                if (darkCurrentPresenter.DarkCurrentTestLimitsDataList.Count == 0 || TestAppHelper.UserModeChanged )
                {
                    await darkCurrentPresenter.GetDarkCurrentLimits();
                }
                UpdateLimitsViewWithModelOnLoad();
                if (darkCurrentPresenter.ExposureDataList.Count == 0 || TestAppHelper.UserModeChanged)
                {
                    await darkCurrentPresenter.GetDarkCurrentData(cidInterface);
                }
                          }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task RunAutoDarkCurrentTest(CancellationTokenSource cts, bool AutoPretest)
        {
            try
            {
                if (cts.IsCancellationRequested)
                    return;
                this.TheoryPlot.ClearData();
                this.MeasuredNoisePlot.ClearData();
                this.darkCurrentWaveformGraph.Refresh();
                ledPassFail.Value = false;
                if (autoTestExposureSettings == null)
                {
                    autoTestExposureSettings = new AutoTestExposureSettings("DDT", cidInterface);
                    autoTestExposureSettings.GetData();
                    darkCurrentPresenter.ExposureDataList = autoTestExposureSettings.ExposureList;
                    darkCurrentPresenter.SubarrayDataModelList = autoTestExposureSettings.SubarrayList;
                    darkCurrentPresenter.ExposureDataModel = autoTestExposureSettings.exposureDataPresenter.ExposureDataModel;
                    darkCurrentPresenter.SubarrayDataModel = autoTestExposureSettings.exposureDataPresenter.SubarrayDataModel;
                    ultraExplorerBarContainerControl2.Controls.Add(autoTestExposureSettings);
                }
                DefectsMaxDarkROI.Text = string.Empty;
                DefectsMaxDarkROI.Appearance.BorderColor = DefectsMaxDarkROI.Appearance.BorderColor2 = Color.Black;
                MaxTrapResult.Text= string.Empty;
                MaxDarkFF.Text = string.Empty;
                DefectsTotal.Text = string.Empty;
                DefectsTotalClusters.Text = string.Empty;
                AveTrap.Text = string.Empty;
                PRNUDefects.Text = string.Empty; ;
                TrapDefects.Text = string.Empty;
                HotPixelDefects.Text = string.Empty;
                DefectsTotalClusters.Text = string.Empty;
                DefectsTotal.Text = string.Empty;
                RunDarkCurrentTest(cts.Token);
                await PlotDarkCurrentData(cts);
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
                log.Debug("Run Dark Current test.");
                if (ct.IsCancellationRequested)
                    return;
                darkCurrentPresenter.runDarkCurrentTest(cidInterface, ct, userMode);
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
                DefectsMaxDarkROI.Text = String.Format("{0:0.00}", darkCurrentPresenter.MaxDarkROI);
				if (darkCurrentPresenter.DarkCurrentMeanValue !=null)
                    this.darkCurrentWaveformGraph.PlotY(darkCurrentPresenter.DarkCurrentMeanValue);
                if (darkCurrentPresenter.DarkCurrentTestPassed)
                {
                    ledPassFail.Value = true;
                    DefectsMaxDarkROI.Appearance.BorderColor = DefectsMaxDarkROI.Appearance.BorderColor2= Color.LimeGreen;
                }                    
                else
                {
                    ledPassFail.Value = false;
                    ledPassFail.OffColor = DefectsMaxDarkROI.Appearance.BorderColor = DefectsMaxDarkROI.Appearance.BorderColor2 = Color.Red;
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
                int outputImageWidth = 0;
                int outputImageHeight = 0;
                Bitmap outputImage = null;
                img = this.darkCurrentWaveformGraph.ToImage();
                outputImageWidth = img.Width;
                outputImageHeight = img.Height;
                outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics graphics = Graphics.FromImage(outputImage))
                {
                    graphics.DrawImage(img, new Rectangle(new Point(), img.Size),
                           new Rectangle(new Point(), img.Size), GraphicsUnit.Pixel);
                }
                outputImage.Save(testResultLocalFilePath + "\\DarkCurrentTestResultImage_"+localFileTimeStamp+".png", System.Drawing.Imaging.ImageFormat.Png);
                this.Dock = DockStyle.Fill;
                if (ConfigurationManager.AppSettings["EndClient"] != "Thermo")
                {
                    TestAppHelper.saveFileInAzure(outputImage, "DarkCurrent.png");
                }
                else
                {
                    if (saveAutoResultsToDisk)
                    {
                        outputImage.Save(testResultsServerPath + "\\DarkCurrentTestResultImage_" + localFileTimeStamp + ".png", System.Drawing.Imaging.ImageFormat.Png);
                        darkCurrentPresenter.TestResultImagePath = serverPathToDB + "\\DarkCurrentTestResultImage_" + localFileTimeStamp + ".png";
                    }
                    else
                        darkCurrentPresenter.TestResultImagePath = string.Empty;
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
        private void UpdateLimitsViewWithModelOnLoad()
        {
            try
            {
                List<int> limtIDs = new List<int>();
                limtIDs = (from limits in darkCurrentPresenter.DarkCurrentTestLimitsDataList select limits.LimitsID).ToList();
                if (!string.IsNullOrWhiteSpace(TestAppHelper.SelectedECONumber) && (userMode.Equals("Engineering") || userMode.Equals("Admin")))
                {
                    darkCurrentPresenter.DarkCurrentTestLimitsData = darkCurrentPresenter.DarkCurrentTestLimitsDataList.Where(limit => limit.ECONumber.Equals(TestAppHelper.SelectedECONumber)).FirstOrDefault();
                    LimitID.Text = darkCurrentPresenter.DarkCurrentTestLimitsData.LimitsID.ToString(); 
                }
                else
                {
                    LimitID.Text = limtIDs[0].ToString();
                    darkCurrentPresenter.DarkCurrentTestLimitsData = darkCurrentPresenter.DarkCurrentTestLimitsDataList.Where(limit => limit.LimitsID.Equals(limtIDs[0])).FirstOrDefault();
                }
                if (darkCurrentPresenter.DarkCurrentTestLimitsData != null)
                {
                    MaxDarkROILimit.Text = darkCurrentPresenter.DarkCurrentTestLimitsData.DarkCurrentMaxDarkROI.ToString();
                    MaxDarkROILowerLimit.Text = darkCurrentPresenter.DarkCurrentTestLimitsData.DarkCurrentLowerLimit.ToString();
                    MaxDarkROIUpperLimit.Text = darkCurrentPresenter.DarkCurrentTestLimitsData.DarkCurrentUpperLimit.ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        } 
    }
}
