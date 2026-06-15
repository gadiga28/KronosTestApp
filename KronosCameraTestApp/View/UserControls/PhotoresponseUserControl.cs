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
using System.Threading;
using Thermo.Kronos.Instrument.Camera.Interface;
using KronosCameraTestApp.Presenter;
using KronosCameraTestApp.Model;
using System.IO;
using System.Configuration;
using KronosCameraTestApp.Helpers;
using System.Security.AccessControl;
using NationalInstruments.Analysis.Math;
using Infragistics.Win.Misc;

namespace KronosCameraTestApp.View.UserControls
{
    public partial class PhotoresponseUserControl : UserControl
    {
        public PhotoresponsePresenter photoresponsePresenter { get; set; }
        AutoTestExposureSettings autoTestExposureSettings { get; set; }
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
        public PhotoresponseUserControl()
        {
            InitializeComponent();
        }
        public PhotoresponseUserControl(PhotoresponsePresenter photoresponsePresenter, string defaultFilePath, string UserMode)
        {
            InitializeComponent();
            this.photoresponsePresenter = photoresponsePresenter;
            this.defaultFilePath = defaultFilePath;
            this.userMode = UserMode;
        }
        public async Task RunAutoPhotoresponseTest(CancellationTokenSource cts, bool AutoPretest)
        {
            try
            {
                photoResponseTestWaveformPlot.ClearData();
                meanWaveformPlot.ClearData();
                linearFitWaveformPlot.ClearData();

                meanofROI1minus2Plot.ClearData();
                linearCurveFitOfVarPlot.ClearData();
                varianceOfROI1minus2Plot.ClearData();

                photoResponseWaveformGraph.Refresh();
                derivativeWaveformGraph.Refresh();
                ledPassFail.Value = false;
                if (cts.IsCancellationRequested)
                    return;

                //if (!userMode.Equals("Admin") && photoresponsePresenter.ExposureDataList.Count == 0)
                //    await photoresponsePresenter.GetPhotoresponseData(cidInterface);

                //InitialExposureData();
                //autoTestExposureSettings.InitialExposureData();
                //if (autoTestExposureSettings == null)
                //{
                //    autoTestExposureSettings = new AutoTestExposureSettings("PRT", cidInterface);
                //    autoTestExposureSettings.GetData();
                //    photoresponsePresenter.ExposureDataList = autoTestExposureSettings.ExposureList;
                //    photoresponsePresenter.SubarrayDataModelList = autoTestExposureSettings.SubarrayList;
                //    photoresponsePresenter.ExposureDataModel = autoTestExposureSettings.exposureDataPresenter.ExposureDataModel;
                //    photoresponsePresenter.SubarrayDataModel = autoTestExposureSettings.exposureDataPresenter.SubarrayDataModel;
                //    ultraExplorerBarContainerControl2.Controls.Add(autoTestExposureSettings);
                //}

                //for (int i = 1; i < 200; i++)
                //{
                //    photoresponsePresenter.ExposureDataModel.GlobalInject = 0;
                //    photoresponsePresenter.ExposureDataModel.GlobalInjectDelay = 0;
                //    photoresponsePresenter.ExposureDataList.Add(photoresponsePresenter.ExposureDataModel);
                //}
                //Run Photoresponse Test
                await LoadExposureData();
               
                await RunPhotoresponseTest(cts.Token);

                //Plot Photoresponse Data
                await PlotPhotoresponseData(cts);
                ultraExplorerBarContainerControl2.Controls.Add(autoTestExposureSettings);
                //SaveTestResultImage();

                //if (!AutoPretest)
                //{
                //    //Write Test results to DB
                //    if (photoresponsePresenter.PhotoresponseTestPassed)
                //        await photoresponsePresenter.WritePhotoresponseTestResultToDB(cts.Token, TestResultImageFilePath);
                //}
            }
            catch (Exception ex)
            {
                //"Internal Error";
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void InitialExposureData()
        {

            UpdateExposureDataModelFromView();
            UpdateSubarrayDataModelFromView();
            photoresponsePresenter.ExposureDataModel.GlobalInject = 0;
            photoresponsePresenter.ExposureDataModel.GlobalInjectDelay = 0;
            photoresponsePresenter.ExposureDataModel.NumberOfSubarrays = 1;
            photoresponsePresenter.ExposureDataModel.SubarrayDatas = new List<SubarrayDataModel>();
            photoresponsePresenter.ExposureDataModel.SubarrayDatas.Add(photoresponsePresenter.SubarrayDataModel);
            photoresponsePresenter.RepeatExposureSetting = false;

        }
        private void UpdateExposureDataModelFromView()
        {
            try
            {
                photoresponsePresenter.ExposureDataModel = new ExposureDataModel();

                photoresponsePresenter.ExposureDataModel.ExposureID = photoresponsePresenter.ExposureDataList[currentRecord].ExposureID;
                photoresponsePresenter.ExposureDataModel.TestDataID = photoresponsePresenter.ExposureDataList[currentRecord].TestDataID;
                photoresponsePresenter.ExposureDataModel.UserModified = photoresponsePresenter.ExposureDataList[currentRecord].UserModified;
                photoresponsePresenter.ExposureDataModel.GlobalInjectDelay = 0;
                photoresponsePresenter.ExposureDataModel.GlobalInject = 0;

                //Exposure         
                photoresponsePresenter.ExposureDataModel.ExposureName = photoresponsePresenter.ExposureDataList[currentRecord].ExposureName;
                photoresponsePresenter.ExposureDataModel.ExposureInterval = photoresponsePresenter.ExposureDataList[currentRecord].ExposureInterval;
                photoresponsePresenter.ExposureDataModel.ExposureNDROS = photoresponsePresenter.ExposureDataList[currentRecord].ExposureNDROS;
                photoresponsePresenter.ExposureDataModel.ExposureRegionXo = photoresponsePresenter.ExposureDataList[currentRecord].ExposureRegionXo;
                photoresponsePresenter.ExposureDataModel.ExposureRegiondX = photoresponsePresenter.ExposureDataList[currentRecord].ExposureRegiondX;
                photoresponsePresenter.ExposureDataModel.ExposureRegionYo = photoresponsePresenter.ExposureDataList[currentRecord].ExposureRegionYo;
                photoresponsePresenter.ExposureDataModel.ExposureRegiondY = photoresponsePresenter.ExposureDataList[currentRecord].ExposureRegiondY;
                photoresponsePresenter.ExposureDataModel.LED1Enabled = photoresponsePresenter.ExposureDataList[currentRecord].LED1Enabled;
                photoresponsePresenter.ExposureDataModel.LED2Enabled = photoresponsePresenter.ExposureDataList[currentRecord].LED2Enabled;
                photoresponsePresenter.ExposureDataModel.LED3Enabled = photoresponsePresenter.ExposureDataList[currentRecord].LED3Enabled;
                photoresponsePresenter.ExposureDataModel.ShutterEnabled = photoresponsePresenter.ExposureDataList[currentRecord].ShutterEnabled;
                photoresponsePresenter.ExposureDataModel.LEDOnTime = photoresponsePresenter.ExposureDataList[currentRecord].LEDOnTime;
                photoresponsePresenter.ExposureDataModel.LEDOffTime = photoresponsePresenter.ExposureDataList[currentRecord].LEDOffTime;
                photoresponsePresenter.ExposureDataModel.LEDFlashes = photoresponsePresenter.ExposureDataList[currentRecord].LEDFlashes;
                photoresponsePresenter.ExposureDataModel.NumberOfSubarrays = photoresponsePresenter.ExposureDataList[currentRecord].NumberOfSubarrays;
                photoresponsePresenter.ExposureDataModel.ExposureFPN = photoresponsePresenter.ExposureDataList[currentRecord].ExposureFPN;

                photoresponsePresenter.ExposureDataModel.FullFrameEnabled = photoresponsePresenter.ExposureDataList[currentRecord].FullFrameEnabled;
                photoresponsePresenter.ExposureDataModel.DarkFrameEnabled = photoresponsePresenter.ExposureDataList[currentRecord].DarkFrameEnabled;

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateSubarrayDataModelFromView()
        {
            try
            {
                photoresponsePresenter.SubarrayDataModel = new SubarrayDataModel();
                photoresponsePresenter.SubarrayDataModel.SubarrayID = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayID;
                photoresponsePresenter.SubarrayDataModel.ExposureID = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].ExposureID;
                photoresponsePresenter.SubarrayDataModel.UserModified = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].UserModified;
                photoresponsePresenter.SubarrayDataModel.SubarrayName = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayName;
                photoresponsePresenter.SubarrayDataModel.SubarrayRegionXo = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayRegionXo;
                photoresponsePresenter.SubarrayDataModel.SubarrayRegiondX = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayRegiondX;
                photoresponsePresenter.SubarrayDataModel.SubarrayRegionYo =photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayRegionYo;
                photoresponsePresenter.SubarrayDataModel.SubarrayRegiondY = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayRegiondY;

                photoresponsePresenter.SubarrayDataModel.SubarrayReadEnabled = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayReadEnabled;
                photoresponsePresenter.SubarrayDataModel.SubarrayReadInterval = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayReadInterval;

                photoresponsePresenter.SubarrayDataModel.SubarraySubinjectEnabled = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarraySubinjectEnabled;
                photoresponsePresenter.SubarrayDataModel.SubarraySubinjectInterval = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarraySubinjectInterval;

                photoresponsePresenter.SubarrayDataModel.SubarrayPostReadEnabled = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayReadEnabled;
                photoresponsePresenter.SubarrayDataModel.SubarrayPostReadNDROS =photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayPostReadNDROS;

                photoresponsePresenter.SubarrayDataModel.SubarrayThresholdRegionXo = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdRegionXo;
                photoresponsePresenter.SubarrayDataModel.SubarrayThresholdRegiondX = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdRegiondX;
                photoresponsePresenter.SubarrayDataModel.SubarrayThresholdRegionYo = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdRegionYo;
                photoresponsePresenter.SubarrayDataModel.SubarrayThresholdRegiondY = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdRegiondY;
                photoresponsePresenter.SubarrayDataModel.SubarrayThresholdEnabled = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].TimeResolvedEnabled;


                photoresponsePresenter.SubarrayDataModel.SubarrayThresholdPercent = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdPercent;
                photoresponsePresenter.SubarrayDataModel.SubarrayFPN=photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayFPN;

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
                if (ct.IsCancellationRequested)
                    return;
                log.Debug("Run Photoresponse test.");
                
                await photoresponsePresenter.runPhotoresponseTest(cidInterface, ct, userMode);
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
                if (photoresponsePresenter.XData !=null &&  photoresponsePresenter.YData !=null)
                {
                    var linearyFitData = CurveFit.LinearFit(photoresponsePresenter.XData, photoresponsePresenter.YData);

                    if (photoresponsePresenter.MeanValue != null)
                        photoResponseWaveformGraph.PlotY(photoresponsePresenter.MeanValue);

                    if (photoresponsePresenter.LinearCurveFit != null)
                        linearCurveFitOfVarPlot.PlotY(photoresponsePresenter.DerivPlot);

                    if (linearyFitData != null)
                        linearFitWaveformPlot.PlotY(linearyFitData, START_BEST_FIT, (NUMBER_OF_BEST_FIT - START_BEST_FIT), START_BEST_FIT, 1);

                    photoresponsePresenter.LinearFitData = linearyFitData;
                }
                
                SaturationResult.Text = photoresponsePresenter.FullWell.ToString();
                SlopeResult.Text = photoresponsePresenter.Slope.ToString();
                InterceptResult.Text = photoresponsePresenter.Intercept.ToString();
                LinearFullWellResult.Text = photoresponsePresenter.LinearFullWell;
                if (photoresponsePresenter.PhotoresponseTestPassed)
                {
                    SaturationResult.Appearance.BorderColor = SaturationResult.Appearance.BorderColor2 = Color.LimeGreen;
                    ledPassFail.Value = true;
                }                    
                else
                {
                    ledPassFail.Value = false;
                    ledPassFail.OffColor = SaturationResult.Appearance.BorderColor  = SaturationResult.Appearance.BorderColor2 = Color.Red;
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
                //BeginInvoke(new System.Action(() => this.Dock = DockStyle.None));
                this.Dock = DockStyle.None;
                Image img = null;
                Image img1 = null;
                int outputImageWidth = 0;
                int outputImageHeight = 0;
                Bitmap outputImage = null;               
               
                //string pathString = System.IO.Path.Combine(defaultFilePath, "TestResults\\" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm"));
                //if (!File.Exists(defaultFilePath))
                //{
                //    System.IO.Directory.CreateDirectory(defaultFilePath);
                //}


                img = this.photoResponseWaveformGraph.ToImage();
                img1 = this.derivativeWaveformGraph.ToImage();

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


                outputImage.Save(testResultLocalFilePath + "\\PhotoresponseTestResultImage_" + localFileTimeStamp + ".png", System.Drawing.Imaging.ImageFormat.Png);
                //BeginInvoke(new System.Action(() => this.Dock = DockStyle.Fill));
                this.Dock = DockStyle.Fill;
                if (ConfigurationManager.AppSettings["EndClient"] != "Thermo")
                {
                    TestAppHelper.saveFileInAzure(outputImage, "Photoresponse.png");
                    //photoresponsePresenter.TestResultImagePath = TestAppHelper.AzureBlobURI;
                }
                   
                else
                {
                    if (savetoNetwork)
                    {
                        outputImage.Save(testResultsServerPath + "\\PhotoresponseTestResultImage_" + localFileTimeStamp + ".png", System.Drawing.Imaging.ImageFormat.Png);
                        photoresponsePresenter.TestResultImagePath = serverPathToDB + "\\PhotoresponseTestResultImage_" + localFileTimeStamp + ".png";
                    }
                    else
                        photoresponsePresenter.TestResultImagePath = string.Empty;
                }
                   
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                
            }

        }
        public async Task SavePhotoresponseTestResults(CancellationTokenSource cts, bool AutoPretest)
        {
            try
            {
                if (!AutoPretest)
                {
                    //Write Test results to DB
                    if (photoresponsePresenter.PhotoresponseTestPassed)
                    {
                        ledPassFail.Value = true;
                        await photoresponsePresenter.WritePhotoresponseTestResultToDB(cts.Token, TestResultImageFilePath);
                    }
                        
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
        //private void UpdateExposureView(int currentRecord)
        //{
        //    try
        //    {
        //        if (currentRecord >= 0 && photoresponsePresenter.ExposureDataList.Count() > 0 && currentRecord < photoresponsePresenter.ExposureDataList.Count())
        //        {

               
        //        labelNumofExposures.Text ="Total # of Exposures: " + photoresponsePresenter.ExposureDataList.Count().ToString();
        //        ultraLabelNumSubarrays.Text = "Total # of Subarrays:" + photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas.Count.ToString();


        //        textBoxExposureName.Text = photoresponsePresenter.ExposureDataList[currentRecord].ExposureName.ToString();

        //        textBoxDelay.Text = photoresponsePresenter.ExposureDataList[currentRecord].GlobalInjectDelay.ToString();
        //        textBoxGlobalInject.Text = photoresponsePresenter.ExposureDataList[currentRecord].GlobalInject.ToString();

        //        textBoxExposureInterval.Text = photoresponsePresenter.ExposureDataList[currentRecord].ExposureInterval.ToString();
        //        textBoxNDROs.Text = photoresponsePresenter.ExposureDataList[currentRecord].ExposureNDROS.ToString();
        //        textBoxExposureXo.Text = photoresponsePresenter.ExposureDataList[currentRecord].ExposureRegionXo.ToString();
        //        textBoxExposuredX.Text = photoresponsePresenter.ExposureDataList[currentRecord].ExposureRegiondX.ToString();
        //        textBoxExposureYo.Text = photoresponsePresenter.ExposureDataList[currentRecord].ExposureRegionYo.ToString();
        //        textBoxExposuredY.Text = photoresponsePresenter.ExposureDataList[currentRecord].ExposureRegiondY.ToString();


        //        ultraCheckEditorLED1.Checked = photoresponsePresenter.ExposureDataList[currentRecord].LED1Enabled;
        //        ultraCheckEditorLED2.Checked = photoresponsePresenter.ExposureDataList[currentRecord].LED2Enabled;
        //        ultraCheckEditorLED3.Checked = photoresponsePresenter.ExposureDataList[currentRecord].LED3Enabled;
        //        ultraCheckEditorShutterEnabled.Checked = photoresponsePresenter.ExposureDataList[currentRecord].ShutterEnabled;

        //        textBoxLEDOn.Text = photoresponsePresenter.ExposureDataList[currentRecord].LEDOnTime.ToString();
        //        textBoxLEDOff.Text = photoresponsePresenter.ExposureDataList[currentRecord].LEDOffTime.ToString();
        //        textBoxLEDFlash.Text = photoresponsePresenter.ExposureDataList[currentRecord].LEDFlashes.ToString();
        //        ultraCheckEditorAutoBiasFPN.Checked = Convert.ToBoolean(photoresponsePresenter.ExposureDataList[currentRecord].AutoBiasFPNEnabled);

        //        //fpn
        //        if (photoresponsePresenter.ExposureDataList[currentRecord].ExposureFPN == 0)
        //        {
        //            ultraRadioButtonFPNNone.Checked = true;
        //        }
        //        else if (photoresponsePresenter.ExposureDataList[currentRecord].ExposureFPN == 1)
        //        {
        //            ultraRadioButtonFPNRunTime.Checked = true;
        //        }
        //        else if (photoresponsePresenter.ExposureDataList[currentRecord].ExposureFPN == 3)
        //        {
        //            ultraRadioButtonFPNStored.Checked = true;
        //        }

        //        ultraCheckEditorExposedFrame.Checked = photoresponsePresenter.ExposureDataList[currentRecord].FullFrameEnabled;
        //        ultraCheckEditorDarkFrame.Checked = photoresponsePresenter.ExposureDataList[currentRecord].DarkFrameEnabled;

        //        textBoxSubarrayXo.Text = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayRegionXo.ToString();
        //        textBoxSubarraydX.Text = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayRegiondX.ToString();
        //        textBoxSubarrayYo.Text = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayRegionYo.ToString();
        //        textBoxSubarraydY.Text = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayRegiondY.ToString();

        //        //subarray

        //        textBoxSubarrayName.Text = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayName.ToString();
        //        ultraCheckEditorSubarrayRead.Checked = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayReadEnabled;
        //        textBoxSubarrayReadInterval.Text = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayReadInterval.ToString();

        //        ultraCheckEditorSubarraySubinject.Checked = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarraySubinjectEnabled;
        //        textBoxSubarraySubinjectInterval.Text = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarraySubinjectInterval.ToString();

        //        ultraCheckEditorSubarrayPostRead.Checked = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayPostReadEnabled;
        //        textBoxSubarrayNDROs.Text = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayPostReadNDROS.ToString();

        //        textBoxThresholdXo.Text = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdRegionXo.ToString();
        //        textBoxThresholddX.Text = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdRegiondX.ToString();
        //        textBoxThresholdYo.Text = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdRegionYo.ToString();
        //        textBoxThresholddY.Text = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdRegiondY.ToString();
        //        ultraCheckEditorEnableThreshold.Checked = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdEnabled;

              
        //        textBoxThresholdPercent.Text = photoresponsePresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdPercent.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
        //    }
        //}      
        //private void FirstExposure_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        currentRecord = 0;
        //        UpdateExposureView(0);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error Navigating Exposure", "Navigate Exposure", MessageBoxButtons.OK);
        //        log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
        //    }
        //}
        //private void NextExposure_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        currentRecord++;
        //        UpdateExposureView(currentRecord);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error Navigating Exposure", "Navigate Exposure", MessageBoxButtons.OK);
        //        log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
        //    }
        //}
        //private void PreviousExposure_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        currentRecord--;
        //        UpdateExposureView(currentRecord);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error Navigating Exposure", "Navigate Exposure", MessageBoxButtons.OK);
        //        log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
        //    }
        //}
        //private void LastExposure_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        currentRecord = photoresponsePresenter.ExposureDataList.Count();
        //        currentRecord--;
        //        UpdateExposureView(currentRecord);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error Navigating Exposure", "Navigate Exposure", MessageBoxButtons.OK);
        //        log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
        //    }
        //}
        private async void PhotoresponseUserControl_Load(object sender, EventArgs e)
        {
            try
            {
                //if (autoTestExposureSettings == null)
                //{
                //    autoTestExposureSettings = new AutoTestExposureSettings("PRT", cidInterface);
                //    autoTestExposureSettings.GetData();
                //    photoresponsePresenter.ExposureDataList = autoTestExposureSettings.ExposureList;
                //    photoresponsePresenter.SubarrayDataModelList = autoTestExposureSettings.SubarrayList;
                //    //autoTestExposureSettings.InitialExposureData();
                //    //photoresponsePresenter.ExposureDataModel = autoTestExposureSettings.exposureDataPresenter.ExposureDataModel;
                //    //photoresponsePresenter.SubarrayDataModel = autoTestExposureSettings.exposureDataPresenter.SubarrayDataModel;
                //    ultraExplorerBarContainerControl2.Controls.Add(autoTestExposureSettings);                   
                //}               

                //MouseEnterLeaveEventHandlers();
                ((ToolStripMenuItem)GridLinesContextMneu.Items[0]).CheckedChanged += XAxisGridClick;
                ((ToolStripMenuItem)GridLinesContextMneu.Items[1]).CheckedChanged += YAxisGridClick;
                ((ToolStripMenuItem)GridLinesContextMneu.Items[2]).CheckedChanged += BothGridLinesClick;
                if (photoresponsePresenter.PhotoresponseLimitsDataList.Count == 0|| TestAppHelper.UserModeChanged)
                    await photoresponsePresenter.GetLimits();
                UpdateLimitsViewWithModelOnLoad();
               

                if (photoresponsePresenter.ExposureDataList.Count == 0 || TestAppHelper.UserModeChanged)
                {
                    await photoresponsePresenter.GetPhotoresponseData(cidInterface);
                }
                //for (int i = 1; i < 200; i++)
                //{
                //    ExposureDataModel exposureDataModel = photoresponsePresenter.ExposureDataList[0];
                //    exposureDataModel.GlobalInject = 0;
                //    exposureDataModel.GlobalInjectDelay = 0;
                //    photoresponsePresenter.ExposureDataList.Add(exposureDataModel);
                //}
                currentRecord = 0;
               // UpdateExposureView(0);
            }
            catch (Exception ex)
            {              
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task LoadExposureData()
        {
            try
            {
                await Task.Run(() =>
                {
                    if (autoTestExposureSettings == null)
                    {
                        autoTestExposureSettings = new AutoTestExposureSettings("PRT", cidInterface);
                        autoTestExposureSettings.GetData();
                        photoresponsePresenter.ExposureDataList = autoTestExposureSettings.ExposureList;
                        photoresponsePresenter.SubarrayDataModelList = autoTestExposureSettings.SubarrayList;
                        autoTestExposureSettings.InitialExposureData();
                        photoresponsePresenter.ExposureDataModel = autoTestExposureSettings.exposureDataPresenter.ExposureDataModel;
                        photoresponsePresenter.SubarrayDataModel = autoTestExposureSettings.exposureDataPresenter.SubarrayDataModel;
                        //ultraExplorerBarContainerControl2.Controls.Add(autoTestExposureSettings);
                        for (int i = 1; i < 200; i++)
                        {
                            //ExposureDataModel exposureDataModel = photoresponsePresenter.ExposureDataList[0];
                            photoresponsePresenter.ExposureDataModel.GlobalInject = 0;
                            photoresponsePresenter.ExposureDataModel.GlobalInjectDelay = 0;
                            photoresponsePresenter.ExposureDataList.Add(photoresponsePresenter.ExposureDataModel);
                        }
                    }
                });               

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void MouseEnterLeaveEventHandlers()
        {
            try
            {
                //FirstExposure.MouseEnter += buttonMouseEnter;
                //NextExposure.MouseEnter += buttonMouseEnter;
                //PreviousExposure.MouseEnter += buttonMouseEnter;
                //LastExposure.MouseEnter += buttonMouseEnter;

                //FirstExposure.MouseLeave += buttonMouseLeave;
                //NextExposure.MouseLeave += buttonMouseLeave;
                //PreviousExposure.MouseLeave += buttonMouseLeave;
                //LastExposure.MouseLeave += buttonMouseLeave;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void buttonMouseEnter(object sender, EventArgs e)
        {
            UltraButton button = sender as UltraButton;
            button.Size = new Size(48, 48);
            button.ImageSize = new Size(40, 40);
        }
        private void buttonMouseLeave(object sender, EventArgs e)
        {
            UltraButton button = sender as UltraButton;
            button.Size = new Size(40, 40);
            button.ImageSize = new Size(32, 32);
        }
        private void UpdateLimitsViewWithModelOnLoad()
        {
            try
            {
                List<int> limtIDs = new List<int>();
                limtIDs = (from limits in photoresponsePresenter.PhotoresponseLimitsDataList select limits.LimitsID).ToList();
                if (!string.IsNullOrWhiteSpace(TestAppHelper.SelectedECONumber) && (userMode.Equals("Engineering") || userMode.Equals("Admin")))
                {
                    photoresponsePresenter.PhotoresponseLimitsData = photoresponsePresenter.PhotoresponseLimitsDataList.Where(limit => limit.ECONumber.Equals(TestAppHelper.SelectedECONumber)).FirstOrDefault();
                    LimitID.Text = photoresponsePresenter.PhotoresponseLimitsData.LimitsID.ToString();
                }
                else
                {
                    LimitID.Text = limtIDs[0].ToString();
                    photoresponsePresenter.PhotoresponseLimitsData = photoresponsePresenter.PhotoresponseLimitsDataList.Where(limit => limit.LimitsID.Equals(limtIDs[0])).FirstOrDefault();
                }
                if (photoresponsePresenter.PhotoresponseLimitsData != null)
                    FullSaturationLimit.Text = photoresponsePresenter.PhotoresponseLimitsData.FullWellLevelMin.ToString();

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void ultraExplorerBar1_GroupExpanded(object sender, Infragistics.Win.UltraWinExplorerBar.GroupEventArgs e)
        {
            try
            {
                //if (e.Group.Text.Equals("Exposure Settings"))
                //{
                //    if (photoresponsePresenter.ExposureDataList.Count == 0)
                //    {
                //        await photoresponsePresenter.GetPhotoresponseData(cidInterface);
                //    }

                //    currentRecord = 0;
                //    UpdateExposureView(0);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Loading Exposure Data", "Load Exposure Data", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void XAxisGridClick(object sender, EventArgs e)
        {
            try
            {
                if (((ToolStripMenuItem)GridLinesContextMneu.Items[0]).Checked)
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

        private void GridLinesContextMneu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                ToolStripItem item = e.ClickedItem;
                if (item.Name.Equals("XAxisGrid"))
                {
                    XAxisGridClick(sender, e);
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
    }
}
