using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using KronosCameraTestApp.Presenter;
using KronosCameraTestApp.Model;
using Thermo.Kronos.Instrument.Camera.Interface;
using System.Threading;
using System.IO;
using System.Configuration;
using KronosCameraTestApp.Helpers;
using System.Security.AccessControl;
using Infragistics.Win.Misc;
namespace KronosCameraTestApp.View.UserControls
{
    public partial class InjectionEfficiencyUserControl : UserControl
    {
        public InjectionEfficiencyPresenter injectionEfficiencyPresenter { get; set; }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
          System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string defaultFilePath = string.Empty;
        private CIDInterface cidInterface = null;
        private int currentRecord = 0;
        string TestResultImageFilePath = string.Empty;
        string userMode = string.Empty;
        AutoTestExposureSettings autoTestExposureSettings { get; set; }
        public CIDInterface CidInterface
        {
            get { return cidInterface; }
            set { cidInterface = value; }
        }
        public InjectionEfficiencyUserControl()
        {
            InitializeComponent();
        }
        public InjectionEfficiencyUserControl(InjectionEfficiencyPresenter injectionEfficiencyPresenter, string defaultFilePath, string UserMode)
        {
            InitializeComponent();
            this.injectionEfficiencyPresenter = injectionEfficiencyPresenter;
            this.defaultFilePath = defaultFilePath;
            this.userMode = UserMode;
        }
        private async void InjectionPerformanceUserControl_Load(object sender, EventArgs e)
        {
            try
            {
                if (autoTestExposureSettings == null)
                {
                    autoTestExposureSettings = new AutoTestExposureSettings("IET", cidInterface);
                    autoTestExposureSettings.GetData();
                    injectionEfficiencyPresenter.ExposureDataList = autoTestExposureSettings.ExposureList;
                    injectionEfficiencyPresenter.SubarrayDataModelList = autoTestExposureSettings.SubarrayList;
                    injectionEfficiencyPresenter.ExposureDataModel = autoTestExposureSettings.exposureDataPresenter.ExposureDataModel;
                    injectionEfficiencyPresenter.SubarrayDataModel = autoTestExposureSettings.exposureDataPresenter.SubarrayDataModel;
                    ultraExplorerBarContainerControl2.Controls.Add(autoTestExposureSettings);
                }
                if (injectionEfficiencyPresenter.InjectionEfficiencyLimitsDataList.Count == 0 || TestAppHelper.UserModeChanged)
                    await injectionEfficiencyPresenter.GetLimits();
                UpdateLimitsViewWithModelOnLoad();
                if (injectionEfficiencyPresenter.ExposureDataList.Count == 0 || TestAppHelper.UserModeChanged)
                {
                    await injectionEfficiencyPresenter.GetInjectionEfficiencyData(cidInterface);
                }
                injectionEfficiencyPresenter.InjectionEfficiencyTestFailureReason = string.Empty;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task RunAutoInjectionPerformanceTest(CancellationTokenSource cts, bool AutoPretest)
        {
            try
            {
                ledPassFail.Value = false;
                waveformPlot1.ClearData();
                EXP0WaveformPlot.ClearData();
                EXP1WaveformPlot.ClearData();
                EXP2WaveformPlot.ClearData();
                EXP3WaveformPlot.ClearData();
                EXP4WaveformPlot.ClearData();
                chargeInjectionEfficiencyWaveformGraph.Refresh();
                if (cts.IsCancellationRequested)
                    return;
                if (autoTestExposureSettings == null)
                {
                    autoTestExposureSettings = new AutoTestExposureSettings("IET", cidInterface);
                    autoTestExposureSettings.GetData();
                    injectionEfficiencyPresenter.ExposureDataList = autoTestExposureSettings.ExposureList;
                    injectionEfficiencyPresenter.SubarrayDataModelList = autoTestExposureSettings.SubarrayList;
                    injectionEfficiencyPresenter.ExposureDataModel = autoTestExposureSettings.exposureDataPresenter.ExposureDataModel;
                    injectionEfficiencyPresenter.SubarrayDataModel = autoTestExposureSettings.exposureDataPresenter.SubarrayDataModel;
                    ultraExplorerBarContainerControl2.Controls.Add(autoTestExposureSettings);
                }
                if (injectionEfficiencyPresenter.ExposureDataList.Count == 0)
                {
                    await injectionEfficiencyPresenter.GetInjectionEfficiencyData(cidInterface);
                }
                await RunInjectionPerformanceTest(cts.Token);
                await PlotInjectionPerformanceData(cts);

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task RunAutoCrossTalkTest(CancellationTokenSource cts, bool AutoPretest)
        {
            try
            {
                await injectionEfficiencyPresenter.GetCrossTalkData(cidInterface);
                RowXTalkPassFail.Value = false;
                ColXTalkPassFail.Value = false;

                if (cts.IsCancellationRequested)
                    return;

                await RunCrossTalkTest(cts.Token);
                await PlotCrossTalkTestData(cts);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task RunInjectionPerformanceTest(CancellationToken ct)
        {
            try
            {
                if (ct.IsCancellationRequested)
                    return;
                log.Debug("Run Injection Performance test.");
                await injectionEfficiencyPresenter.runInjectionEfficiencyTest(cidInterface, ct, userMode);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task PlotInjectionPerformanceData(CancellationTokenSource ct)
        {
            try
            {
                if (ct.IsCancellationRequested)
                    return;
                await injectionEfficiencyPresenter.CalculateInjectionEfficiency(ct.Token);
                if (ct.IsCancellationRequested)
                    return;
                if (!cidInterface.IsConnected())
                    return;
                firstInjEXP0.Text = "0.0";
                firstInjEXP1.Text = "0.0";
                firstInjEXP2.Text = "0.0";
                firstInjEXP3.Text = "0.0";
                firstInjEXP4.Text = "0.0";
                secondInjEXP0.Text = "0.0";
                secondInjEXP1.Text = "0.0";
                secondInjEXP2.Text = "0.0";
                secondInjEXP3.Text = "0.0";
                secondInjEXP4.Text = "0.0";
                thirdInjEXP0.Text = "0.0";
                thirdInjEXP1.Text = "0.0";
                thirdInjEXP2.Text = "0.0";
                thirdInjEXP3.Text = "0.0";
                thirdInjEXP4.Text = "0.0";
                fourthInjEXP0.Text = "0.0";
                fourthInjEXP1.Text = "0.0";
                fourthInjEXP2.Text = "0.0";
                fourthInjEXP3.Text = "0.0";
                fourthInjEXP4.Text = "0.0";
                lastInjEXP0.Text = "0.0";
                lastInjEXP1.Text = "0.0";
                lastInjEXP2.Text = "0.0";
                lastInjEXP3.Text = "0.0";
                lastInjEXP4.Text = "0.0";
                if (ct.IsCancellationRequested)
                    return;
                EXP0WaveformPlot.PlotYAppend(injectionEfficiencyPresenter.EXP0PlotData);
                EXP1WaveformPlot.PlotYAppend(injectionEfficiencyPresenter.EXP1PlotData);
                EXP2WaveformPlot.PlotYAppend(injectionEfficiencyPresenter.EXP2PlotData);
                EXP3WaveformPlot.PlotYAppend(injectionEfficiencyPresenter.EXP3PlotData);
                EXP4WaveformPlot.PlotYAppend(injectionEfficiencyPresenter.EXP4PlotData);
                initialEXP0TextBox.Text = injectionEfficiencyPresenter.InitialExposureResult[0].ToString();
                initialEXP1TextBox.Text = injectionEfficiencyPresenter.InitialExposureResult[1].ToString();
                initialEXP2TextBox.Text = injectionEfficiencyPresenter.InitialExposureResult[2].ToString();
                initialEXP3TextBox.Text = injectionEfficiencyPresenter.InitialExposureResult[3].ToString();
                initialEXP4TextBox.Text = injectionEfficiencyPresenter.InitialExposureResult[4].ToString();
                firstInjEXP0.Text = injectionEfficiencyPresenter.FirstInjectionResultUI[0];
                firstInjEXP1.Text = injectionEfficiencyPresenter.FirstInjectionResultUI[1];
                firstInjEXP2.Text = injectionEfficiencyPresenter.FirstInjectionResultUI[2];
                firstInjEXP3.Text = injectionEfficiencyPresenter.FirstInjectionResultUI[3];
                firstInjEXP4.Text = injectionEfficiencyPresenter.FirstInjectionResultUI[4];
                secondInjEXP0.Text = injectionEfficiencyPresenter.SecondInjectionResultUI[0];
                secondInjEXP1.Text = injectionEfficiencyPresenter.SecondInjectionResultUI[1];
                secondInjEXP2.Text = injectionEfficiencyPresenter.SecondInjectionResultUI[2];
                secondInjEXP3.Text = injectionEfficiencyPresenter.SecondInjectionResultUI[3];
                secondInjEXP4.Text = injectionEfficiencyPresenter.SecondInjectionResultUI[4];
                thirdInjEXP0.Text = injectionEfficiencyPresenter.ThirdInjectionResultUI[0];
                thirdInjEXP1.Text = injectionEfficiencyPresenter.ThirdInjectionResultUI[1];
                thirdInjEXP2.Text = injectionEfficiencyPresenter.ThirdInjectionResultUI[2];
                thirdInjEXP3.Text = injectionEfficiencyPresenter.ThirdInjectionResultUI[3];
                thirdInjEXP4.Text = injectionEfficiencyPresenter.ThirdInjectionResultUI[4];
                fourthInjEXP0.Text = injectionEfficiencyPresenter.FourthInjectionResultUI[0];
                fourthInjEXP1.Text = injectionEfficiencyPresenter.FourthInjectionResultUI[1];
                fourthInjEXP2.Text = injectionEfficiencyPresenter.FourthInjectionResultUI[2];
                fourthInjEXP3.Text = injectionEfficiencyPresenter.FourthInjectionResultUI[3];
                fourthInjEXP4.Text = injectionEfficiencyPresenter.FourthInjectionResultUI[4];
                lastInjEXP0.Text = injectionEfficiencyPresenter.LastInjectionResultUI[0];
                lastInjEXP1.Text = injectionEfficiencyPresenter.LastInjectionResultUI[1];
                lastInjEXP2.Text = injectionEfficiencyPresenter.LastInjectionResultUI[2];
                lastInjEXP3.Text = injectionEfficiencyPresenter.LastInjectionResultUI[3];
                lastInjEXP4.Text = injectionEfficiencyPresenter.LastInjectionResultUI[4];
                if (injectionEfficiencyPresenter.InjectionEfficiencyTestPassed)
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

        private async Task RunCrossTalkTest(CancellationToken ct)
        {
            try
            {
                if (ct.IsCancellationRequested)
                    return;
                log.Debug("Run Injection Performance test.");
                await injectionEfficiencyPresenter.runCrossTalkTest(cidInterface, ct);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task PlotCrossTalkTestData(CancellationTokenSource ct)
        {
            try
            {
                if (ct.IsCancellationRequested)
                    return;
                await injectionEfficiencyPresenter.CalculateCrossTalk(ct.Token);
                if (ct.IsCancellationRequested)
                    return;
                if (!cidInterface.IsConnected())
                    return;


                meanBox1.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox1")).Value.ToString();
                meanBox2.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox2")).Value.ToString();
                meanBox3.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox3")).Value.ToString();
                meanBox4.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox4")).Value.ToString();
                meanBox5.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox5")).Value.ToString();
                meanBox6.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox6")).Value.ToString();
                meanBox7.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox7")).Value.ToString();
                meanBox8.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox8")).Value.ToString();
                meanBox9.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox9")).Value.ToString();

                Reference.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("Reference")).Value.ToString();
                RowXTalk.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("RowCrosstalk")).Value.ToString();
                ColXTalk.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("ColumnCrosstalk")).Value.ToString();
                Sensitivity.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("RelativeSensitivity")).Value.ToString();

                if (injectionEfficiencyPresenter.RowCrossTalkTestPassed)
                {
                    RowXTalkPassFail.Value = true;
                }
                else
                {
                    RowXTalkPassFail.Value = false;
                    RowXTalkPassFail.OffColor = Color.Red;
                }
                if (injectionEfficiencyPresenter.ColCrossTalkTestPassed)
                {
                    ColXTalkPassFail.Value = true;
                }
                else
                {
                    ColXTalkPassFail.Value = false;
                    ColXTalkPassFail.OffColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task ClearInjectionPerformanceGraphs()
        {
            await Task.Run(() =>
            {
                waveformPlot1.ClearData();
                EXP0WaveformPlot.ClearData();
                EXP1WaveformPlot.ClearData();
                EXP2WaveformPlot.ClearData();
                EXP3WaveformPlot.ClearData();
                EXP4WaveformPlot.ClearData();
            });
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
                //img = chargeInjectionEfficiencyWaveformGraph.ToImage();

                int width = mainDisplayPanel.Size.Width;
                int height = mainDisplayPanel.Size.Height;

                Bitmap bm = new Bitmap(width, height);
                mainDisplayPanel.DrawToBitmap(bm, new Rectangle(0, 0, width, height));


                //img = mainDisplayPanel.DrawToBitmap()
                //outputImageWidth = img.Width ;
                //outputImageHeight = img.Height+ 1;
                //outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                //using (Graphics graphics = Graphics.FromImage(outputImage))
                //{
                //    graphics.DrawImage(img, new Rectangle(new Point(), img.Size),
                //           new Rectangle(new Point(), img.Size), GraphicsUnit.Pixel);
                //}
                //outputImage.Save(testResultLocalFilePath + "\\InjectionPerformanceTestResultImage_" + localFileTimeStamp + ".png", System.Drawing.Imaging.ImageFormat.Png);
                this.Dock = DockStyle.Fill;
                if (ConfigurationManager.AppSettings["EndClient"] != "Thermo")
                {
                    TestAppHelper.saveFileInAzure(outputImage, "InjectionEfficiency.png");
                }                   
                else
                {
                    if (savetoNetwork)
                    {
                        // outputImage.Save(testResultsServerPath + "\\InjectionPerformanceTestResultImage_" + localFileTimeStamp + ".png", System.Drawing.Imaging.ImageFormat.Png);
                        bm.Save(testResultsServerPath + "\\InjectionPerformanceTestResultImage_" + localFileTimeStamp + ".png", System.Drawing.Imaging.ImageFormat.Png);
                        injectionEfficiencyPresenter.TestResultImagePath = serverPathToDB + "\\InjectionPerformanceTestResultImage_" + localFileTimeStamp + ".png";
                    }
                    else
                        injectionEfficiencyPresenter.TestResultImagePath = string.Empty;
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
                limtIDs = (from ecos in injectionEfficiencyPresenter.InjectionEfficiencyLimitsDataList select ecos.LimitsID).ToList();
                if (!string.IsNullOrWhiteSpace(TestAppHelper.SelectedECONumber) && (userMode.Equals("Engineering") || userMode.Equals("Admin")))
                {
                    injectionEfficiencyPresenter.InjectionEfficiencyLimitsData = injectionEfficiencyPresenter.InjectionEfficiencyLimitsDataList.
                        Where(limit => limit.ECONumber.Equals(TestAppHelper.SelectedECONumber)).FirstOrDefault();
                    LimitID.Text = injectionEfficiencyPresenter.InjectionEfficiencyLimitsData.LimitsID.ToString();
                }
                else
                {
                    LimitID.Text = limtIDs[0].ToString();
                    injectionEfficiencyPresenter.InjectionEfficiencyLimitsData = injectionEfficiencyPresenter.InjectionEfficiencyLimitsDataList.
                        Where(limit => limit.LimitsID.Equals(limtIDs[0])).FirstOrDefault();
                }
                if (injectionEfficiencyPresenter.InjectionEfficiencyLimitsData != null)
                {
                    InjectionEffLimit.Text = injectionEfficiencyPresenter.InjectionEfficiencyLimitsData.InjectionEffLimit.ToString();
                    crossTalkThresholdLimit.Text= injectionEfficiencyPresenter.InjectionEfficiencyLimitsData.CrossTalkThresholdLimit.ToString();
                }                
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
