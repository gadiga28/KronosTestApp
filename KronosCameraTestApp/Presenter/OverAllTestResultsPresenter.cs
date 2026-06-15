using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.View;
using Microsoft.Azure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
namespace KronosCameraTestApp.Presenter
{
    public class OverAllTestResultsPresenter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string enggUserName = string.Empty;
        public string EnggUserName
        {
            get { return enggUserName; }
            set { enggUserName = value; }
        }
        OverAllTestResultsModel overAllTestResultsModel { get; set; }
        List<OverAllTestResultsModel> overAllTestResultsModelList { get; set; }
        List<FinalTestResultsDetailsModel> finalTestResultsDetailsModelList { get; set; }
        List<CameraDetailsModel> cameraDetailsModelList { get; set; }
        List<RedBlueTestResults> redBlueTestResultsList { get; set; }
        List<MeanVarianceTestResultsData> meanVarianceTestResultsDataList { get; set; }
        List<NoiseVsNDROTestResults> noiseVsNDROTestResultsList { get; set; }
        List<DarkCurrentTestResults> darkCurrentTestResultsList { get; set; }
        List<InjectionEfficiencyTestResults> injectionEfficiencyTestResultsList { get; set; }
        List<PhotoresponseTestResults> photoresponseTestResultsList { get; set; }
        
        List<ShutterDriveTestResults> shutterDriveTestResultsList { get; set; }
        OverAllTestResultsForm overAllTestResultsForm { get; set; }       
        public List<OverAllTestResultsModel> OverAllTestResultsModelList
        {
            get { return overAllTestResultsModelList; }
            set { overAllTestResultsModelList = value; }
        }
        public List<FinalTestResultsDetailsModel> FinalTestResultsDetailsModelList
        {
            get { return finalTestResultsDetailsModelList; }
            set { finalTestResultsDetailsModelList = value; }
        }
        bool overAllTestResultsDBChanged = false;      
        bool testResultsSaved = true;      
        KronosTestAppMainForm kronosTestAppMainForm { get; set; }
        bool DBExists = false;
        XDocument xmlDoc;
        XDocument xmlResultsDoc;
        string testResultsfilepath = string.Empty;
        Random finalTestIDRand;
        public OverAllTestResultsPresenter(OverAllTestResultsModel overAllTestResultsModel, OverAllTestResultsForm overAllTestResultsForm, KronosTestAppMainForm kronosTestAppMainForm)
        {
            this.overAllTestResultsForm = overAllTestResultsForm;
            this.overAllTestResultsModel = overAllTestResultsModel;
            this.kronosTestAppMainForm = kronosTestAppMainForm;
            kronosTestAppMainForm.overAllTestResultsPresenter = this;
            testResultsfilepath = ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "OverallTestResultsData.xml";
            overAllTestResultsModelList = new List<OverAllTestResultsModel>();
            finalTestResultsDetailsModelList = new List<FinalTestResultsDetailsModel>();
            cameraDetailsModelList = new List<CameraDetailsModel>();
            redBlueTestResultsList = new List<RedBlueTestResults>();
            meanVarianceTestResultsDataList = new List<MeanVarianceTestResultsData>();
            noiseVsNDROTestResultsList = new List<NoiseVsNDROTestResults>();
            darkCurrentTestResultsList = new List<DarkCurrentTestResults>();
            injectionEfficiencyTestResultsList = new List<InjectionEfficiencyTestResults>();
            photoresponseTestResultsList = new List<PhotoresponseTestResults>();
            shutterDriveTestResultsList = new List<ShutterDriveTestResults>();
            finalTestIDRand = new Random();
        }
        public async Task GetOverAllTestResultsData()
        {
            try
            {
                if (TestAppHelper.CheckDBExists())
                {
                    DBExists = true;
                    await PopulateOverallTestResultsData();
                }
                else
                {
                    //process data from xml files
                    DBExists = false;
                    await ProcessOverallTestResultsXMLFile();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task PopulateOverallTestResultsData()
        {
            try
            {
                await Task.Run(() =>
                {
                    using (var kcc = new KronosCamContext())
                    {
                        overAllTestResultsModelList = (from mv in kcc.overAllTestResultsModel.Include("FinalTestResultsDetails")
                                                                                .Include("CameraDetails")
                                                                                .Include("RedBlueTestResults")
                                                                                .Include("MeanVarinaceTestResults")
                                                                                .Include("NoiseVsNDROTestResults")
                                                                                .Include("DarkCurrentTestResults")
                                                                                .Include("DefectsTestResults")
                                                                                .Include("InjectionEfficiencyTestResults")
                                                                                .Include("PhotoresponseTestResults")
                                                                                .Include("ShutterDriveTestResults")
                                                       orderby mv.DateExecuted descending
                                                       select mv).ToList();
                        log.Info("OverAllTestResults data populated from Database with Records of ." + overAllTestResultsModelList.Count().ToString());
                        testResultsSaved = false;
                    }
                });
            }
            catch (Exception ex)
            {
                testResultsSaved = true;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task ProcessOverallTestResultsXMLFile()
        {
            try
            {
                await Task.Run(() =>
                {
                    log.Info("Begin Reading OverallTestResultsData XML.");
                    if (testResultsSaved)
                    {
                        DataSet dsXML = new DataSet();
                        dsXML.ReadXml(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "/OverallTestResultsData.xml");
                        if (dsXML.Tables.Count > 0)
                        {
                            DataView dvOverall;
                            dvOverall = dsXML.Tables[0].DefaultView;
                            DataView dvDetails;
                            dvDetails = dsXML.Tables[1].DefaultView;
                            DataView dvCameraDetails;
                            dvCameraDetails = dsXML.Tables[2].DefaultView;
                            DataView dvRedBlueResults;
                            dvRedBlueResults = dsXML.Tables[3].DefaultView;
                            DataView dvMeanVarianceResults;
                            dvMeanVarianceResults = dsXML.Tables[4].DefaultView;
                            DataView dvDarkCurrentResults;
                            dvDarkCurrentResults = dsXML.Tables[5].DefaultView;
                            DataView dvNoiseVsNdrosResults;
                            dvNoiseVsNdrosResults = dsXML.Tables[6].DefaultView;
                            DataView dvPhotoresponseResults;
                            dvPhotoresponseResults = dsXML.Tables[7].DefaultView;
                            DataView dvInjectionPerformanceResults;
                            dvInjectionPerformanceResults = dsXML.Tables[8].DefaultView;
                            DataView dvShutterDriveResults;
                            dvShutterDriveResults = dsXML.Tables[9].DefaultView;
                            foreach (DataRowView dr in dvOverall)
                            {
                                OverAllTestResultsModel overAllTestResultsModel = new OverAllTestResultsModel();
                                overAllTestResultsModel.TestResultsID = Convert.ToInt32(dr[0]);
                                overAllTestResultsModel.ECONumber = Convert.ToString(dr[1]);
                                overAllTestResultsModel.CameraSerialNumber = Convert.ToString(dr[2]);
                                overAllTestResultsModel.ImagerSerialNumber = Convert.ToString(dr[3]);
                                overAllTestResultsModel.DateExecuted = Convert.ToDateTime(dr[4]);
                                overAllTestResultsModel.UserExecuted = Convert.ToString(dr[5]);
                                overAllTestResultsModel.UserRole = Convert.ToString(dr[6]);
                                overAllTestResultsModel.TestPassed = Convert.ToBoolean(dr[7]);
                                overAllTestResultsModel.TestFailureReason = Convert.ToString(dr[8]);
                                overAllTestResultsModel.BurnInResult = Convert.ToString(dr[9]);
                                overAllTestResultsModel.TestStation = Convert.ToString(dr[10]);
                                overAllTestResultsModel.TestResultImagePath = Convert.ToString(dr[11]);
                                overAllTestResultsModelList.Add(overAllTestResultsModel);
                            }
                            foreach (DataRowView dr in dvDetails)
                            {
                                FinalTestResultsDetailsModel finalTestResultsDetailsModel = new FinalTestResultsDetailsModel();
                                finalTestResultsDetailsModel.FinalTestID = Convert.ToInt32(dr[0]);
                                finalTestResultsDetailsModel.TestResultsID = Convert.ToInt32(dr[1]);
                                finalTestResultsDetailsModel.ConversionFactorNominal = Convert.ToDouble(dr[2]);
                                finalTestResultsDetailsModel.TotalNumDefects = Convert.ToInt32(dr[3]);
                                finalTestResultsDetailsModel.TotalNumClusters = Convert.ToInt32(dr[4]);
                                finalTestResultsDetailsModel.TotalColumnDefects = Convert.ToInt32(dr[5]);
                                finalTestResultsDetailsModel.TotalRowDefects = Convert.ToInt32(dr[6]);
                                finalTestResultsDetailsModel.AveTrap = Convert.ToInt32(dr[7]);
                                finalTestResultsDetailsModel.MaxDarkROI = Convert.ToDouble(dr[8]);
                                finalTestResultsDetailsModel.FullWellLevelMin = Convert.ToInt32(dr[9]);
                                finalTestResultsDetailsModel.SnglNoiseMax = Convert.ToDouble(dr[10]);
                                finalTestResultsDetailsModel.PowerFitUpper = Convert.ToDouble(dr[11]);
                                finalTestResultsDetailsModel.PowerFitLower = Convert.ToDouble(dr[12]);
                                finalTestResultsDetailsModel.BestFitPower = Convert.ToDouble(dr[13]);
                                finalTestResultsDetailsModel.RPower2Correlation = Convert.ToDouble(dr[14]);
                                finalTestResultsDetailsModel.ShutterDrive = Convert.ToBoolean(dr[15]);
                                finalTestResultsDetailsModel.RedBlue = Convert.ToBoolean(dr[16]);
                                finalTestResultsDetailsModel.DriftROICount = Convert.ToInt32(dr[17]);
                                finalTestResultsDetailsModelList.Add(finalTestResultsDetailsModel);
                            }
                            foreach (DataRowView dr in dvCameraDetails)
                            {
                                CameraDetailsModel cameraDetailsModel = new CameraDetailsModel();
                                cameraDetailsModel.CameraTestID = Convert.ToInt32(dr[0]);
                                cameraDetailsModel.TestResultsID = Convert.ToInt32(dr[1]);
                                cameraDetailsModel.CameraModelNumber = Convert.ToString(dr[2]);
                                cameraDetailsModel.CameraSerialNumber = Convert.ToString(dr[3]);
                                cameraDetailsModel.ImagerSerialNumber = Convert.ToString(dr[4]);
                                cameraDetailsModel.CRA = Convert.ToString(dr[5]);
                                cameraDetailsModel.DetectorType = Convert.ToString(dr[6]);
                                cameraDetailsModel.FirmwareVer = Convert.ToString(dr[7]);
                                cameraDetailsModel.FPGAVersion = Convert.ToString(dr[8]);
                                cameraDetailsModel.MACAddress = Convert.ToString(dr[9]);
                                cameraDetailsModel.CPUSerialNumber = Convert.ToString(dr[10]);
                                cameraDetailsModel.PowerSerialNumber = Convert.ToString(dr[11]);
                                cameraDetailsModel.CSPSerialNumber = Convert.ToString(dr[12]);
                                cameraDetailsModel.ISISerialNumber = Convert.ToString(dr[13]);
                                cameraDetailsModel.ProductShippedDate = Convert.ToDateTime(dr[14]);
                                cameraDetailsModel.DateDefined = Convert.ToDateTime(dr[15]);
                                cameraDetailsModel.UserExecuted = Convert.ToString(dr[16]);
                                cameraDetailsModel.TestSoftwareVer = Convert.ToString(dr[17]);
                                cameraDetailsModelList.Add(cameraDetailsModel);
                            }
                            foreach (DataRowView dr in dvRedBlueResults)
                            {
                                RedBlueTestResults m = new RedBlueTestResults();
                                m.ResultsID = Convert.ToInt32(dr[0]);
                                m.TestResultsID = Convert.ToInt32(dr[1]);
                                m.DateExecuted = Convert.ToDateTime(dr[2]);
                                m.UserExecuted = Convert.ToString(dr[3]);
                                m.RedImageData = Convert.ToString(dr[4]);
                                m.BlueImageData = Convert.ToString(dr[5]);
                                m.RedBlueImageDifference = Convert.ToString(dr[6]);
                                m.TestPassed = Convert.ToBoolean(dr[7]);
                                m.TestResultImagePath = Convert.ToString(dr[8]);
                                redBlueTestResultsList.Add(m);
                            }
                            foreach (DataRowView dr in dvMeanVarianceResults)
                            {
                                MeanVarianceTestResultsData m = new MeanVarianceTestResultsData();
                                m.ResultsID = Convert.ToInt32(dr[0]);
                                m.TestResultsID = Convert.ToInt32(dr[1]);
                                m.DateExecuted = Convert.ToDateTime(dr[2]);
                                m.UserExecuted = Convert.ToString(dr[3]);
                                m.CorrectedMean = Convert.ToString(dr[4]);
                                m.CorrectedVariance = Convert.ToString(dr[5]);
                                m.LinearCurveFit = Convert.ToString(dr[6]);
                                m.Gain = Convert.ToDouble(dr[7]);
                                m.TestPassed = Convert.ToBoolean(dr[8]);
                                m.TestResultImagePath = Convert.ToString(dr[9]);
                                meanVarianceTestResultsDataList.Add(m);
                            }
                            foreach (DataRowView dr in dvDarkCurrentResults)
                            {
                                DarkCurrentTestResults m = new DarkCurrentTestResults();
                                m.ResultsID = Convert.ToInt32(dr[0]);
                                m.TestResultsID = Convert.ToInt32(dr[1]);
                                m.DateExecuted = Convert.ToDateTime(dr[2]);
                                m.UserExecuted = Convert.ToString(dr[3]);
                                m.DarkCurrentMean = Convert.ToString(dr[4]);
                                m.PRNUDefects = Convert.ToInt32(dr[5]);
                                m.TrapDefects = Convert.ToInt32(dr[6]);
                                m.HotPixelDefects = Convert.ToInt32(dr[7]);
                                m.ClusterDefects = Convert.ToInt32(dr[8]);
                                m.TotalDefects = Convert.ToInt32(dr[9]);
                                m.MaxDarkFF = Convert.ToInt32(dr[10]);
                                m.AveDarkFF = Convert.ToInt32(dr[11]);
                                m.MaxTrap = Convert.ToInt32(dr[12]);
                                m.AveTrap = Convert.ToInt32(dr[13]);
                                m.MaxDarkROI = Convert.ToInt32(dr[14]);
                                m.DeadPixels = Convert.ToInt32(dr[15]);
                                m.BadColumns = Convert.ToInt32(dr[16]);
                                m.BadRows = Convert.ToInt32(dr[17]);
                                m.DefectsMean = Convert.ToDouble(dr[18]);
                                m.TestPassed = Convert.ToBoolean(dr[19]);
                                m.TestResultImagePath = Convert.ToString(dr[20]);
                                darkCurrentTestResultsList.Add(m);
                            }
                            foreach (DataRowView dr in dvNoiseVsNdrosResults)
                            {
                                NoiseVsNDROTestResults m = new NoiseVsNDROTestResults();
                                m.ResultsID = Convert.ToInt32(dr[0]);
                                m.TestResultsID = Convert.ToInt32(dr[1]);
                                m.DateExecuted = Convert.ToDateTime(dr[2]);
                                m.UserExecuted = Convert.ToString(dr[3]);
                                m.ExposureNDRO = Convert.ToString(dr[4]);
                                m.NoiseRatio = Convert.ToString(dr[5]);
                                m.Noise = Convert.ToString(dr[6]);
                                m.R2Correlation = Convert.ToDouble(dr[7]);
                                m.BestFit = Convert.ToDouble(dr[8]);
                                m.BestFitPower = Convert.ToDouble(dr[9]);
                                m.TheoryValue = Convert.ToString(dr[10]);
                                m.SignalReadNoise = Convert.ToDouble(dr[11]);
                                m.NDRONoise = Convert.ToDouble(dr[12]);
                                m.TestPassed = Convert.ToBoolean(dr[13]);
                                m.TestResultImagePath = Convert.ToString(dr[14]);
                                noiseVsNDROTestResultsList.Add(m);
                            }
                            foreach (DataRowView dr in dvPhotoresponseResults)
                            {
                                PhotoresponseTestResults m = new PhotoresponseTestResults();
                                m.ResultsID = Convert.ToInt32(dr[0]);
                                m.TestResultsID = Convert.ToInt32(dr[1]);
                                m.DateExecuted = Convert.ToDateTime(dr[2]);
                                m.UserExecuted = Convert.ToString(dr[3]);
                                m.Mean = Convert.ToString(dr[4]);
                                m.LinearCurveFit = Convert.ToString(dr[5]);
                                m.LinearFitData = Convert.ToString(dr[6]);
                                m.FullSaturation = Convert.ToDouble(dr[7]);
                                m.LinearSaturation = Convert.ToDouble(dr[8]);
                                m.Slope = Convert.ToInt32(dr[9]);
                                m.Intercept = Convert.ToDouble(dr[10]);
                                m.DerivPlot = Convert.ToString(dr[10]);
                                m.TestPassed = Convert.ToBoolean(dr[11]);
                                m.TestResultImagePath = Convert.ToString(dr[12]);
                                photoresponseTestResultsList.Add(m);
                            }
                            foreach (DataRowView dr in dvInjectionPerformanceResults)
                            {
                                InjectionEfficiencyTestResults m = new InjectionEfficiencyTestResults();
                                m.ResultsID = Convert.ToInt32(dr[0]);
                                m.TestResultsID = Convert.ToInt32(dr[1]);
                                m.DateExecuted = Convert.ToDateTime(dr[2]);
                                m.UserExecuted = Convert.ToString(dr[3]);
                                m.InitialExposure = Convert.ToString(dr[4]);
                                m.FirstInjection = Convert.ToString(dr[5]);
                                m.SecondInjection = Convert.ToString(dr[6]);
                                m.ThirdInjection = Convert.ToString(dr[7]);
                                m.FourthInjection = Convert.ToString(dr[8]);
                                m.LastInjection = Convert.ToString(dr[9]);
                                m.Exp0PlotData = Convert.ToString(dr[10]);
                                m.Exp1PlotData = Convert.ToString(dr[11]);
                                m.Exp2PlotData = Convert.ToString(dr[12]);
                                m.Exp3PlotData = Convert.ToString(dr[13]);
                                m.Exp4PlotData = Convert.ToString(dr[14]);
                                m.TestPassed = Convert.ToBoolean(dr[15]);
                                m.TestResultImagePath = Convert.ToString(dr[16]);
                                injectionEfficiencyTestResultsList.Add(m);
                            }
                            foreach (DataRowView dr in dvShutterDriveResults)
                            {
                                ShutterDriveTestResults m = new ShutterDriveTestResults();
                                m.ResultsID = Convert.ToInt32(dr[0]);
                                m.TestResultsID = Convert.ToInt32(dr[1]);
                                m.DateExecuted = Convert.ToDateTime(dr[2]);
                                m.UserExecuted = Convert.ToString(dr[3]);
                                m.TestPassed = Convert.ToBoolean(dr[4]);
                                m.TestResultImagePath = Convert.ToString(dr[5]);
                                m.LabJackSN = Convert.ToString(dr[6]);
                                m.ShutterPlot = Convert.ToString(dr[6]);
                                shutterDriveTestResultsList.Add(m);
                            }
                            foreach (OverAllTestResultsModel exp in overAllTestResultsModelList)
                            {
                                exp.FinalTestResultsDetails = new List<FinalTestResultsDetailsModel>();
                                foreach (FinalTestResultsDetailsModel sm in finalTestResultsDetailsModelList)
                                {
                                    if (sm.TestResultsID == exp.TestResultsID)
                                    {
                                        exp.FinalTestResultsDetails.Add(sm);
                                    }
                                }
                                exp.CameraDetails = new List<CameraDetailsModel>();
                                foreach (CameraDetailsModel sm in cameraDetailsModelList)
                                {
                                    if (sm.TestResultsID == exp.TestResultsID)
                                    {
                                        exp.CameraDetails.Add(sm);
                                    }
                                }
                                exp.RedBlueTestResults = new List<RedBlueTestResults>();
                                foreach (RedBlueTestResults sm in redBlueTestResultsList)
                                {
                                    if (sm.TestResultsID == exp.TestResultsID)
                                    {
                                        exp.RedBlueTestResults.Add(sm);
                                    }
                                }
                                exp.MeanVarinaceTestResults = new List<MeanVarianceTestResultsData>();
                                foreach (MeanVarianceTestResultsData sm in meanVarianceTestResultsDataList)
                                {
                                    if (sm.TestResultsID == exp.TestResultsID)
                                    {
                                        exp.MeanVarinaceTestResults.Add(sm);
                                    }
                                }
                                exp.DarkCurrentTestResults = new List<DarkCurrentTestResults>();
                                foreach (DarkCurrentTestResults sm in darkCurrentTestResultsList)
                                {
                                    if (sm.TestResultsID == exp.TestResultsID)
                                    {
                                        exp.DarkCurrentTestResults.Add(sm);
                                    }
                                }
                                exp.NoiseVsNDROTestResults = new List<NoiseVsNDROTestResults>();
                                foreach (NoiseVsNDROTestResults sm in noiseVsNDROTestResultsList)
                                {
                                    if (sm.TestResultsID == exp.TestResultsID)
                                    {
                                        exp.NoiseVsNDROTestResults.Add(sm);
                                    }
                                }
                                exp.PhotoresponseTestResults = new List<PhotoresponseTestResults>();
                                foreach (PhotoresponseTestResults sm in photoresponseTestResultsList)
                                {
                                    if (sm.TestResultsID == exp.TestResultsID)
                                    {
                                        exp.PhotoresponseTestResults.Add(sm);
                                    }
                                }
                                exp.InjectionEfficiencyTestResults = new List<InjectionEfficiencyTestResults>();
                                foreach (InjectionEfficiencyTestResults sm in injectionEfficiencyTestResultsList)
                                {
                                    if (sm.TestResultsID == exp.TestResultsID)
                                    {
                                        exp.InjectionEfficiencyTestResults.Add(sm);
                                    }
                                }
                                exp.ShutterDriveTestResults = new List<ShutterDriveTestResults>();
                                foreach (ShutterDriveTestResults sm in shutterDriveTestResultsList)
                                {
                                    if (sm.TestResultsID == exp.TestResultsID)
                                    {
                                        exp.ShutterDriveTestResults.Add(sm);
                                    }
                                }
                            }
                            if (overAllTestResultsModelList.Count > 0)
                            {
                                log.Info("End Reading OverallTestResultsData XML.");
                                testResultsSaved = false;
                            }
                            else
                            {
                                log.Error("Error Loading OverallTestResultsData XML");
                                testResultsSaved = true;
                            }
                        }
                    }
                    log.Info("End Reading OverallTestResultsData XML.");
                });
            }
            catch (Exception ex)
            {
                testResultsSaved = true;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }     
        private async Task SaveOverAllTestResultsToDB()
        {
            try
            {
                await Task.Run(() =>
                   {
                       log.Info("Begin ADD/REMOVE/UPDATE OverAll Test Results Data to DB.");
                       DBExists = true;
                       using (var kcc = new KronosCamContext())
                       {
                           kcc.overAllTestResultsModel.Add(overAllTestResultsModel);
                           int limitsRecordSaved = kcc.SaveChanges();
                           if (limitsRecordSaved > 0)
                           {
                               log.Info("End ADD/REMOVE/UPDATE OverAll Test Results Data to DB.");
                               testResultsSaved = true;
                           }
                           else
                           {
                               log.Error("Eror Occured while Add/Remove/Update OverAll Test Results Data records to DB");
                               testResultsSaved = false;
                           }
                       }
                   });
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                testResultsSaved = false;
            }
        }
        public async Task<bool> SaveOverAllTestResults(FinalTestResultsDetailsModel finalTestResultsDetailsModel, CameraDetailsModel cameraDetailsModel,
                                                        RedBlueTestResults redBlueTestResults, MeanVarianceTestResultsData meanVarianceTestResultsData,
                                                         NoiseVsNDROTestResults noiseVsNDROTestResults,
                                                        DarkCurrentTestResults darkCurrentTestResults, DefectsTestResults defectsTestResults,
                                                        InjectionEfficiencyTestResults injectionEfficiencyTestResults,
                                                        PhotoresponseTestResults photoresponseTestResults, ShutterDriveTestResults shutterDriveTestResults, string filetimestamp,
                                                        string TestResultImageFilePath, string CameraSerialNumber, string ImagerSerialNumber, string UserRole,
                                                        string testFailReason, bool testPassed, BurnInAnalysisModel cameraBurnInData, bool preTest, bool saveAutoResultsToDisk,
                                                        EnvironmentalStatusModel environmentalStatusModel, List<TempHumReadingModel> tempHumReadingModels )
        {
            try
            {                
               
                if (cameraBurnInData != null)
                {
                    if (cameraBurnInData.BurnInResult)
                        overAllTestResultsModel.BurnInResult = "Pass";
                    else
                        overAllTestResultsModel.BurnInResult = "Fail";
                }
                else
                    overAllTestResultsModel.BurnInResult = "N/A";
                kronosTestAppMainForm.SetDatabaseConnectivityStatusBarPanel();
                overAllTestResultsModel.ECONumber = TestAppHelper.GetECONumber();
                overAllTestResultsModel.CameraSerialNumber = CameraSerialNumber;
                overAllTestResultsModel.ImagerSerialNumber = ImagerSerialNumber;
                overAllTestResultsModel.UserExecuted = string.IsNullOrWhiteSpace(enggUserName) ? Environment.UserName.ToUpper() : enggUserName.ToUpper();
                overAllTestResultsModel.UserRole = UserRole;
                overAllTestResultsModel.TestPassed = testPassed;
                overAllTestResultsModel.TestFailureReason = overAllTestResultsModel.TestPassed ? "None" : testFailReason.TrimEnd(',', ' ');
                overAllTestResultsModel.TestStage = preTest ? "PRE_TEST" : "FINAL_TEST";
                overAllTestResultsModel.TestStation = Environment.MachineName;
                if (ConfigurationManager.AppSettings["EndClient"] != "Thermo")
                    overAllTestResultsModel.TestResultImagePath = TestResultImageFilePath;
                if (saveAutoResultsToDisk)
                    overAllTestResultsModel.TestResultImagePath = TestResultImageFilePath + "\\FinalTestReport_" + filetimestamp + ".png";
                else
                    overAllTestResultsModel.TestResultImagePath = string.Empty;
                overAllTestResultsModel.DateExecuted = redBlueTestResults.DateExecuted = meanVarianceTestResultsData.DateExecuted =
                                                        noiseVsNDROTestResults.DateExecuted = darkCurrentTestResults.DateExecuted =
                                                        injectionEfficiencyTestResults.DateExecuted =
                                                        photoresponseTestResults.DateExecuted = DateTime.Now;
                if (finalTestResultsDetailsModel != null)
                {
                    overAllTestResultsModel.FinalTestResultsDetails = new List<FinalTestResultsDetailsModel>();
                    overAllTestResultsModel.FinalTestResultsDetails.Add(finalTestResultsDetailsModel);
                }
                if (cameraDetailsModel != null)
                {
                    overAllTestResultsModel.CameraDetails = new List<CameraDetailsModel>();
                    overAllTestResultsModel.CameraDetails.Add(cameraDetailsModel);
                }
                if (redBlueTestResults != null)
                {
                    overAllTestResultsModel.RedBlueTestResults = new List<RedBlueTestResults>();
                    overAllTestResultsModel.RedBlueTestResults.Add(redBlueTestResults);
                }
                if (meanVarianceTestResultsData != null)
                {
                    overAllTestResultsModel.MeanVarinaceTestResults = new List<MeanVarianceTestResultsData>();
                    overAllTestResultsModel.MeanVarinaceTestResults.Add(meanVarianceTestResultsData);
                }
                if (noiseVsNDROTestResults != null)
                {
                    overAllTestResultsModel.NoiseVsNDROTestResults = new List<NoiseVsNDROTestResults>();
                    overAllTestResultsModel.NoiseVsNDROTestResults.Add(noiseVsNDROTestResults);
                }
                if (darkCurrentTestResults != null)
                {
                    overAllTestResultsModel.DarkCurrentTestResults = new List<DarkCurrentTestResults>();
                    overAllTestResultsModel.DarkCurrentTestResults.Add(darkCurrentTestResults);
                }
                if (defectsTestResults != null)
                {
                    overAllTestResultsModel.DefectsTestResults = new List<DefectsTestResults>();
                    overAllTestResultsModel.DefectsTestResults.Add(defectsTestResults);
                }
                if (injectionEfficiencyTestResults != null)
                {
                    overAllTestResultsModel.InjectionEfficiencyTestResults = new List<InjectionEfficiencyTestResults>();
                    overAllTestResultsModel.InjectionEfficiencyTestResults.Add(injectionEfficiencyTestResults);
                }
                if (photoresponseTestResults != null)
                {
                    overAllTestResultsModel.PhotoresponseTestResults = new List<PhotoresponseTestResults>();
                    overAllTestResultsModel.PhotoresponseTestResults.Add(photoresponseTestResults);
                }
                if (shutterDriveTestResults != null)
                {
                    overAllTestResultsModel.ShutterDriveTestResults = new List<ShutterDriveTestResults>();
                    overAllTestResultsModel.ShutterDriveTestResults.Add(shutterDriveTestResults);
                }
                if (environmentalStatusModel != null)
                {
                    overAllTestResultsModel.EnvironmentalStatusModel = new List<EnvironmentalStatusModel>();
                    overAllTestResultsModel.EnvironmentalStatusModel.Add(environmentalStatusModel);
                }
                if (tempHumReadingModels != null)
                {
                    overAllTestResultsModel.TempHumReadingModel = tempHumReadingModels;
                    //overAllTestResultsModel.TempHumReadingModel.Add(tempHumReadingModel);
                }
                if (TestAppHelper.CheckDBExists())
                {
                    log.Info("Begin ADD/REMOVE/UPDATE OverAll Test Results Data to DB.");
                    DBExists = true;
                    await SaveOverAllTestResultsToDB();
                }
                if (ConfigurationManager.AppSettings["EndClient"] == "Thermo" && Convert.ToBoolean(ConfigurationManager.AppSettings["SaveDataInXML"]))
                {
                    await SaveOverallTestResultsToXML(overAllTestResultsModel, finalTestResultsDetailsModel, cameraDetailsModel, redBlueTestResults,
                            meanVarianceTestResultsData, noiseVsNDROTestResults, darkCurrentTestResults, injectionEfficiencyTestResults,
                            photoresponseTestResults, shutterDriveTestResults);
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                testResultsSaved = false;
                return false;
            }
        }
        public async Task<bool> SaveOverAllPreTestResults(FinalTestResultsDetailsModel finalTestResultsDetailsModel, CameraDetailsModel cameraDetailsModel,
                                                     string[] selectedPreTests, string filetimestamp,
                                                      string TestResultImageFilePath, string CameraSerialNumber, string ImagerSerialNumber, string UserRole,
                                                      string testFailReason, bool testPassed, BurnInAnalysisModel cameraBurnInData, 
                                                      bool preTest, Dictionary<string, object> preTests, List<TempHumReadingModel> tempHumReadingModels)
        {
            try
            {
              
                if (cameraBurnInData != null)
                {
                    if (cameraBurnInData.BurnInResult)
                        overAllTestResultsModel.BurnInResult = "Pass";
                    else
                        overAllTestResultsModel.BurnInResult = "Fail";
                }
                else
                    overAllTestResultsModel.BurnInResult = "N/A";
                kronosTestAppMainForm.SetDatabaseConnectivityStatusBarPanel();
                overAllTestResultsModel.ECONumber = TestAppHelper.GetECONumber();
                overAllTestResultsModel.CameraSerialNumber = CameraSerialNumber;
                overAllTestResultsModel.ImagerSerialNumber = ImagerSerialNumber;
                overAllTestResultsModel.UserExecuted = string.IsNullOrWhiteSpace(enggUserName) ? Environment.UserName.ToUpper() : enggUserName.ToUpper();
                overAllTestResultsModel.UserRole = UserRole;
                overAllTestResultsModel.TestPassed = testPassed;
                overAllTestResultsModel.TestFailureReason = overAllTestResultsModel.TestPassed ? "None" : testFailReason.TrimEnd(',', ' ');
                overAllTestResultsModel.TestStage = preTest ? "PRE_TEST" : "FINAL_TEST";
                overAllTestResultsModel.TestStation = Environment.MachineName;
                if (ConfigurationManager.AppSettings["EndClient"] != "Thermo")
                    overAllTestResultsModel.TestResultImagePath = TestResultImageFilePath;
                else
                    overAllTestResultsModel.TestResultImagePath = TestResultImageFilePath + "\\PreTestReport_" + filetimestamp + ".png";
                DateTime dateExecuted = DateTime.Now;
                if (finalTestResultsDetailsModel != null)
                {
                    overAllTestResultsModel.FinalTestResultsDetails = new List<FinalTestResultsDetailsModel>();
                    overAllTestResultsModel.FinalTestResultsDetails.Add(finalTestResultsDetailsModel);
                }
                if (cameraDetailsModel != null)
                {
                    overAllTestResultsModel.CameraDetails = new List<CameraDetailsModel>();
                    overAllTestResultsModel.CameraDetails.Add(cameraDetailsModel);
                }
                foreach (string str in selectedPreTests)
                {
                    switch (str)
                    {
                        case "RBT":
                            RedBlueTestResults redBlueTestResults = (RedBlueTestResults)preTests.FirstOrDefault(pretest => pretest.Key.Equals("RBT")).Value;
                            overAllTestResultsModel.RedBlueTestResults = new List<RedBlueTestResults>();
                            redBlueTestResults.DateExecuted = dateExecuted;
                            overAllTestResultsModel.RedBlueTestResults.Add(redBlueTestResults);
                            break;
                        case "DDT":
                            DarkCurrentTestResults darkCurrentTestResults = (DarkCurrentTestResults)preTests.FirstOrDefault(pretest => pretest.Key.Equals("DDT")).Value;
                            overAllTestResultsModel.DarkCurrentTestResults = new List<DarkCurrentTestResults>();
                            darkCurrentTestResults.DateExecuted = dateExecuted;
                            overAllTestResultsModel.DarkCurrentTestResults.Add(darkCurrentTestResults);
                            break;
                        case "DET":
                            DefectsTestResults defectsTestResults = (DefectsTestResults)preTests.FirstOrDefault(pretest => pretest.Key.Equals("DET")).Value;
                            overAllTestResultsModel.DefectsTestResults = new List<DefectsTestResults>();
                            defectsTestResults.DateExecuted = dateExecuted;
                            overAllTestResultsModel.DefectsTestResults.Add(defectsTestResults);
                            break;
                        case "MVT":
                            MeanVarianceTestResultsData meanVarianceTestResultsData = (MeanVarianceTestResultsData)preTests.FirstOrDefault(pretest => pretest.Key.Equals("MVT")).Value;
                            overAllTestResultsModel.MeanVarinaceTestResults = new List<MeanVarianceTestResultsData>();
                            meanVarianceTestResultsData.DateExecuted = dateExecuted;
                            overAllTestResultsModel.MeanVarinaceTestResults.Add(meanVarianceTestResultsData);
                            break;
                        case "RNT":
                            NoiseVsNDROTestResults noiseVsNDROTestResults = (NoiseVsNDROTestResults)preTests.FirstOrDefault(pretest => pretest.Key.Equals("RNT")).Value;
                            overAllTestResultsModel.NoiseVsNDROTestResults = new List<NoiseVsNDROTestResults>();
                            noiseVsNDROTestResults.DateExecuted = dateExecuted;
                            overAllTestResultsModel.NoiseVsNDROTestResults.Add(noiseVsNDROTestResults);
                            break;
                        case "PRT":
                            PhotoresponseTestResults photoresponseTestResults = (PhotoresponseTestResults)preTests.FirstOrDefault(pretest => pretest.Key.Equals("PRT")).Value;
                            overAllTestResultsModel.PhotoresponseTestResults = new List<PhotoresponseTestResults>();
                            photoresponseTestResults.DateExecuted = dateExecuted;
                            overAllTestResultsModel.PhotoresponseTestResults.Add(photoresponseTestResults);
                            break;
                        case "IET":
                            InjectionEfficiencyTestResults injectionEfficiencyTestResults = (InjectionEfficiencyTestResults)preTests.FirstOrDefault(pretest => pretest.Key.Equals("IET")).Value;
                            overAllTestResultsModel.InjectionEfficiencyTestResults = new List<InjectionEfficiencyTestResults>();
                            injectionEfficiencyTestResults.DateExecuted = dateExecuted;
                            overAllTestResultsModel.InjectionEfficiencyTestResults.Add(injectionEfficiencyTestResults);
                            break;
                        case "SDT":
                            ShutterDriveTestResults shutterDriveTestResults = (ShutterDriveTestResults)preTests.FirstOrDefault(pretest => pretest.Key.Equals("SDT")).Value;
                            overAllTestResultsModel.ShutterDriveTestResults = new List<ShutterDriveTestResults>();
                            shutterDriveTestResults.DateExecuted = dateExecuted;
                            overAllTestResultsModel.ShutterDriveTestResults.Add(shutterDriveTestResults);
                            break;
                    }
                }

                if (tempHumReadingModels != null)
                {
                    overAllTestResultsModel.TempHumReadingModel = tempHumReadingModels;
                }
                if (TestAppHelper.CheckDBExists())
                {
                    log.Info("Begin ADD/REMOVE/UPDATE OverAll Test Results Data to DB.");
                    DBExists = true;
                    await SaveOverAllTestResultsToDB();
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                testResultsSaved = false;
                return false;
            }
        }
        private async Task SaveOverallTestResultsToXML(OverAllTestResultsModel overAllTestResultsModel, FinalTestResultsDetailsModel finalTestResultsDetailsModel,
                                                        CameraDetailsModel cameraDetailsModel, RedBlueTestResults redBlueTestResults, MeanVarianceTestResultsData meanVarianceTestResultsData,
                                                        NoiseVsNDROTestResults noiseVsNDROTestResults,
                                                        DarkCurrentTestResults darkCurrentTestResults, InjectionEfficiencyTestResults injectionPerformanceTestResults,
                                                        PhotoresponseTestResults photoresponseTestResults, ShutterDriveTestResults shutterDriveTestResults)
        {
            try
            {
                await Task.Run(() =>
                {
                    log.Info("Begin Add Final Test Results to  XML.");
                    xmlResultsDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "OverallTestResultsData.xml");
                    if (xmlResultsDoc != null)
                    {
                        xmlResultsDoc.Element("OverallTestResults").Add
                       (
                                 new XElement("OverallTestResult",
                                 new XElement("TestResultsID", DBExists ? overAllTestResultsModel.TestResultsID : (overAllTestResultsModelList.Count() + 1)),
                                 new XElement("ECONumber", overAllTestResultsModel.ECONumber),
                                 new XElement("CameraSerialNumber", overAllTestResultsModel.CameraSerialNumber),
                                 new XElement("ImagerSerialNumber", overAllTestResultsModel.ImagerSerialNumber),
                                 new XElement("DateExecuted", DateTime.Today),
                                 new XElement("UserExecuted", string.IsNullOrWhiteSpace(enggUserName) ? Environment.UserName : enggUserName),
                                 new XElement("UserRole", overAllTestResultsModel.UserRole),
                                 new XElement("TestPassed", overAllTestResultsModel.TestPassed),
                                 new XElement("TestFailureReason", overAllTestResultsModel.TestFailureReason),
                                 new XElement("BurnInResult", overAllTestResultsModel.BurnInResult),
                                 new XElement("TestStation", Environment.MachineName),
                                 new XElement("TestResultImage", overAllTestResultsModel.TestResultImagePath),
                                 new XElement("FinalTestResultDetails",
                                 new XElement("FinalTestID", DBExists ? finalTestResultsDetailsModel.FinalTestID : finalTestResultsDetailsModelList.Count() + 1),
                                 new XElement("TestResultsID", DBExists ? overAllTestResultsModel.TestResultsID : (overAllTestResultsModelList.Count() + 1)),
                                 new XElement("ConversionFactorNominal", finalTestResultsDetailsModel.ConversionFactorNominal),
                                 new XElement("TotalNumDefects", finalTestResultsDetailsModel.TotalNumDefects),
                                 new XElement("TotalNumClusters", finalTestResultsDetailsModel.TotalNumClusters),
                                 new XElement("TotalColumnDefects", finalTestResultsDetailsModel.TotalColumnDefects),
                                 new XElement("TotalRowDefects", finalTestResultsDetailsModel.TotalRowDefects),
                                 new XElement("DriftROICount", finalTestResultsDetailsModel.DriftROICount),
                                 new XElement("AveTrap", finalTestResultsDetailsModel.AveTrap),
                                 new XElement("MaxDarkROI", finalTestResultsDetailsModel.MaxDarkROI),
                                 new XElement("FullWellLevelMin", finalTestResultsDetailsModel.FullWellLevelMin),
                                 new XElement("SnglNoiseMax", finalTestResultsDetailsModel.SnglNoiseMax),
                                 new XElement("PowerFitUpper", finalTestResultsDetailsModel.PowerFitUpper),
                                 new XElement("PowerFitLower", finalTestResultsDetailsModel.PowerFitLower),
                                 new XElement("BestFitPower", finalTestResultsDetailsModel.BestFitPower),
                                 new XElement("RPower2Correlation", finalTestResultsDetailsModel.RPower2Correlation),
                                 new XElement("InjEfficiencyInitialExposure", finalTestResultsDetailsModel.InjEfficiencyInitialExposure),
                                 new XElement("InjEfficiencyFirstInjection", finalTestResultsDetailsModel.InjEfficiencyFirstInjection),
                                  new XElement("InjEfficiencyLastInjection", finalTestResultsDetailsModel.InjEfficiencyLastInjection),
                                 new XElement("ShutterDrive", finalTestResultsDetailsModel.ShutterDrive),
                                 new XElement("RedBlue", finalTestResultsDetailsModel.RedBlue)),
                               new XElement("CameraDetails",
                               new XElement("CameraTestID", DBExists ? cameraDetailsModel.CameraTestID : (cameraDetailsModelList.Count() + 1)),
                               new XElement("TestResultsID", cameraDetailsModel.TestResultsID),
                               new XElement("CameraModelNumber", cameraDetailsModel.CameraModelNumber),
                               new XElement("CameraSerialNumber", cameraDetailsModel.CameraSerialNumber),
                               new XElement("ImagerSerialNumber", cameraDetailsModel.ImagerSerialNumber),
                               new XElement("CRANumber", cameraDetailsModel.CRA),
                               new XElement("DetectorType", cameraDetailsModel.DetectorType),
                               new XElement("FirmwareVersion", cameraDetailsModel.FirmwareVer),
                               new XElement("AlteraVersion", cameraDetailsModel.FPGAVersion),
                               new XElement("MACAddress", cameraDetailsModel.MACAddress),
                               new XElement("CPUSerialNumber", cameraDetailsModel.CPUSerialNumber),
                               new XElement("PowerSerialNumber", cameraDetailsModel.PowerSerialNumber),
                               new XElement("CSPSerialNumber", cameraDetailsModel.CSPSerialNumber),
                               new XElement("ISISerialNumber", cameraDetailsModel.ISISerialNumber),
                               new XElement("ProductShippedDate", cameraDetailsModel.ProductShippedDate),
                               new XElement("DefinedDate", cameraDetailsModel.DateDefined),
                               new XElement("UserExecuted", cameraDetailsModel.UserExecuted.ToUpper()),
                               new XElement("TestSoftwareVersion", cameraDetailsModel.TestSoftwareVer)),
                               new XElement("RedBlueTestResults",
                               new XElement("ResultsID", DBExists ? redBlueTestResults.ResultsID : redBlueTestResultsList.Count + 1),
                               new XElement("TestResultsID", redBlueTestResults.TestResultsID),
                               new XElement("DateExecuted", redBlueTestResults.DateExecuted),
                               new XElement("UserExecuted", redBlueTestResults.UserExecuted.ToUpper()),
                               new XElement("RedImageData", redBlueTestResults.RedImageData),
                               new XElement("BlueImageData", redBlueTestResults.BlueImageData),
                               new XElement("RedBlueImageDifference", redBlueTestResults.RedBlueImageDifference),
                               new XElement("TestPassed", redBlueTestResults.TestPassed),
                               new XElement("TestResultImage", redBlueTestResults.TestResultImagePath)),
                               new XElement("MeanVarianceTestResults",
                               new XElement("ResultsID", DBExists ? meanVarianceTestResultsData.ResultsID : meanVarianceTestResultsDataList.Count + 1),
                               new XElement("TestResultsID", meanVarianceTestResultsData.TestResultsID),
                               new XElement("DateExecuted", meanVarianceTestResultsData.DateExecuted),
                               new XElement("UserExecuted", meanVarianceTestResultsData.UserExecuted.ToUpper()),
                               new XElement("CorrectedMean", meanVarianceTestResultsData.CorrectedMean),
                               new XElement("CorrectedVariance", meanVarianceTestResultsData.CorrectedVariance),
                                new XElement("LinearCurveFit", meanVarianceTestResultsData.LinearCurveFit),
                               new XElement("Gain", meanVarianceTestResultsData.Gain),
                               new XElement("TestPassed", meanVarianceTestResultsData.TestPassed),
                               new XElement("TestResultImagePath", meanVarianceTestResultsData.TestResultImagePath)),
                               new XElement("DarkCurrentTestResults",
                               new XElement("ResultsID", DBExists ? darkCurrentTestResults.ResultsID : darkCurrentTestResultsList.Count + 1),
                               new XElement("TestResultsID", darkCurrentTestResults.TestResultsID),
                               new XElement("DateExecuted", darkCurrentTestResults.DateExecuted),
                               new XElement("UserExecuted", darkCurrentTestResults.UserExecuted.ToUpper()),
                               new XElement("DarkCurrentMean", darkCurrentTestResults.DarkCurrentMean),
                               new XElement("PRNUDefects", darkCurrentTestResults.PRNUDefects),
                               new XElement("HotPixelDefects", darkCurrentTestResults.HotPixelDefects),
                               new XElement("TrapDefects", darkCurrentTestResults.TrapDefects),
                               new XElement("ClusterDefects", darkCurrentTestResults.ClusterDefects),
                               new XElement("TotalDefects", darkCurrentTestResults.TotalDefects),
                               new XElement("MaxDarkFF", darkCurrentTestResults.MaxDarkFF),
                               new XElement("AveDarkFF", darkCurrentTestResults.AveDarkFF),
                               new XElement("MaxTrap", darkCurrentTestResults.MaxTrap),
                               new XElement("AveTrap", darkCurrentTestResults.AveTrap),
                               new XElement("MaxDarkROI", darkCurrentTestResults.MaxDarkROI),
                                new XElement("DeadPixels", darkCurrentTestResults.DeadPixels),
                               new XElement("BadColumns", darkCurrentTestResults.BadColumns),
                               new XElement("BadRows", darkCurrentTestResults.BadRows),
                               new XElement("DefectsMean", darkCurrentTestResults.DefectsMean),
                               new XElement("TestPassed", darkCurrentTestResults.TestPassed),
                               new XElement("TestResultImage", darkCurrentTestResults.TestResultImagePath)),
                               new XElement("NoiseVsNDROsTestResults",
                               new XElement("ResultsID", DBExists ? noiseVsNDROTestResults.ResultsID : noiseVsNDROTestResultsList.Count + 1),
                               new XElement("TestResultsID", noiseVsNDROTestResults.TestResultsID),
                               new XElement("DateExecuted", noiseVsNDROTestResults.DateExecuted),
                               new XElement("UserExecuted", noiseVsNDROTestResults.UserExecuted.ToUpper()),
                               new XElement("ExposureNDRO", noiseVsNDROTestResults.ExposureNDRO),
                               new XElement("NoiseRatio", noiseVsNDROTestResults.NoiseRatio),
                               new XElement("Noise", noiseVsNDROTestResults.Noise),
                               new XElement("R2Correlation", noiseVsNDROTestResults.R2Correlation),
                               new XElement("BestFit", noiseVsNDROTestResults.BestFit),
                               new XElement("BestFitPower", noiseVsNDROTestResults.BestFitPower),
                                new XElement("TheoryValue", noiseVsNDROTestResults.TheoryValue),
                               new XElement("SignalReadNoise", noiseVsNDROTestResults.SignalReadNoise),
                               new XElement("NDRONoise", noiseVsNDROTestResults.NDRONoise),
                               new XElement("TestPassed", noiseVsNDROTestResults.TestPassed),
                               new XElement("TestResultImage", noiseVsNDROTestResults.TestResultImagePath)),
                               new XElement("PhotoresponseTestResults",
                               new XElement("ResultsID", DBExists ? photoresponseTestResults.ResultsID : photoresponseTestResultsList.Count + 1),
                               new XElement("TestResultsID", photoresponseTestResults.TestResultsID),
                               new XElement("DateExecuted", photoresponseTestResults.DateExecuted),
                               new XElement("UserExecuted", photoresponseTestResults.UserExecuted.ToUpper()),
                               new XElement("Mean", photoresponseTestResults.Mean),
                               new XElement("LinearCurveFit", photoresponseTestResults.LinearCurveFit),
                               new XElement("LinearFitData", photoresponseTestResults.LinearFitData),
                               new XElement("FullWell", photoresponseTestResults.FullSaturation),
                               new XElement("LinearSaturation", photoresponseTestResults.LinearSaturation),
                               new XElement("Slope", photoresponseTestResults.Slope),
                               new XElement("Intercept", photoresponseTestResults.Intercept),
                               new XElement("DerivPlot", photoresponseTestResults.DerivPlot),
                               new XElement("TestPassed", photoresponseTestResults.TestPassed),
                               new XElement("TestResultImage", photoresponseTestResults.TestResultImagePath)),
                               new XElement("InjectionPerformanceTestResults",
                               new XElement("ResultsID", DBExists ? injectionPerformanceTestResults.ResultsID : injectionEfficiencyTestResultsList.Count + 1),
                               new XElement("TestResultsID", injectionPerformanceTestResults.TestResultsID),
                               new XElement("DateExecuted", injectionPerformanceTestResults.DateExecuted),
                               new XElement("UserExecuted", injectionPerformanceTestResults.UserExecuted.ToUpper()),
                               new XElement("InitialExposure", injectionPerformanceTestResults.InitialExposure),
                               new XElement("FirstInjection", injectionPerformanceTestResults.FirstInjection),
                                new XElement("SecondInjection", injectionPerformanceTestResults.SecondInjection),
                                 new XElement("ThirdInjection", injectionPerformanceTestResults.ThirdInjection),
                                  new XElement("FourthInjection", injectionPerformanceTestResults.FourthInjection),
                               new XElement("LastInjection", injectionPerformanceTestResults.LastInjection),
                               new XElement("Exp0PlotData", injectionPerformanceTestResults.Exp0PlotData),
                            new XElement("Exp1PlotData", injectionPerformanceTestResults.Exp1PlotData),
                            new XElement("Exp2PlotData", injectionPerformanceTestResults.Exp2PlotData),
                            new XElement("Exp3PlotData", injectionPerformanceTestResults.Exp3PlotData),
                            new XElement("Exp4PlotData", injectionPerformanceTestResults.Exp4PlotData),
                               new XElement("TestPassed", injectionPerformanceTestResults.TestPassed),
                               new XElement("TestResultImage", injectionPerformanceTestResults.TestResultImagePath)),
                               new XElement("ShutterDriveTestResults",
                               new XElement("ResultsID", shutterDriveTestResults.ResultsID),
                               new XElement("TestResultsID", shutterDriveTestResults.TestResultsID),
                               new XElement("DateExecuted", shutterDriveTestResults.DateExecuted),
                               new XElement("UserExecuted", shutterDriveTestResults.UserExecuted.ToUpper()),
                               new XElement("TestPassed", shutterDriveTestResults.TestPassed),
                               new XElement("TestResultImage", shutterDriveTestResults.TestResultImagePath),
                               new XElement("LabJackSN", shutterDriveTestResults.LabJackSN),
                                new XElement("ShutterPlot", shutterDriveTestResults.ShutterPlot))
                          ));
                        xmlResultsDoc.Save(testResultsfilepath);
                        testResultsSaved = true;
                    }
                    SaveMeanVarianceResultsInXML(meanVarianceTestResultsData);
                    SaveDarkCurrentResultsInXML(darkCurrentTestResults);
                    SaveInjectionPerformanceResultsInXML(injectionPerformanceTestResults);
                    SaveReadNoiseResultsInXML(noiseVsNDROTestResults);
                    SavePhotoresponseResultsInXML(photoresponseTestResults);
                    SaveRedBlueResultsInXML(redBlueTestResults);
                    SaveShutterDriveResultsInXML(shutterDriveTestResults);
                    SaveFinalResultsInXML(finalTestResultsDetailsModel);
                    SaveCameraDetailsInXML(cameraDetailsModel);
                });
                log.Info(" End Add Final Test Results XML.");
            }
            catch (Exception ex)
            {
                testResultsSaved = false;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void SaveMeanVarianceResultsInXML(MeanVarianceTestResultsData meanVarianceTestResultData)
        {
            try
            {
                log.Info("Begin saving Mean Variance Test Results to MeanVarianceResultsData XML.");
                XDocument meanVarianceXMLResultsDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "MeanVarianceResultsData.xml");
                if (meanVarianceXMLResultsDoc != null)
                {
                    meanVarianceXMLResultsDoc.Element("MeanVarianceTestResults").Add
                    (
                        new XElement("TestDetails",
                        new XElement("ResultsID", meanVarianceTestResultData.ResultsID),
                        new XElement("TestResultsID", meanVarianceTestResultData.TestResultsID),
                        new XElement("DateExecuted", meanVarianceTestResultData.DateExecuted),
                        new XElement("UserExecuted", meanVarianceTestResultData.UserExecuted.ToUpper()),
                        new XElement("CorrectedMean", meanVarianceTestResultData.CorrectedMean),
                        new XElement("CorrectedVariance", meanVarianceTestResultData.CorrectedVariance),
                         new XElement("LinearCurveFit", meanVarianceTestResultData.LinearCurveFit),
                         new XElement("Gain", meanVarianceTestResultData.Gain),
                        new XElement("TestPassed", meanVarianceTestResultData.TestPassed),
                        new XElement("TestResultImagePath", meanVarianceTestResultData.TestResultImagePath))
                    );
                    meanVarianceXMLResultsDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "MeanVarianceResultsData.xml");
                    log.Info("End saving Mean Variance Test Results to MeanVarianceResultsData XML.");
                }
                else
                {
                    log.Error("MeanVarianceResultsData XML is null");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void SaveDarkCurrentResultsInXML(DarkCurrentTestResults darkCurrentTestResultsData)
        {
            try
            {
                log.Info("Begin saving Dark Current Test Results to DarkCurrentResultsData XML.");
                XDocument xmlDarkCurrentResultsDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "DarkCurrentResultsData.xml");
                if (xmlDarkCurrentResultsDoc != null)
                {
                    xmlDarkCurrentResultsDoc.Element("DarkCurrentTestResults").Add
                        (
                        new XElement("TestDetails",
                            new XElement("ResultsID", darkCurrentTestResultsData.ResultsID),
                            new XElement("TestResultsID", darkCurrentTestResultsData.TestResultsID),
                            new XElement("DateExecuted", darkCurrentTestResultsData.DateExecuted),
                            new XElement("UserExecuted", darkCurrentTestResultsData.UserExecuted.ToUpper()),
                            new XElement("DarkCurrentMean", darkCurrentTestResultsData.DarkCurrentMean),
                            new XElement("PRNUDefects", darkCurrentTestResultsData.PRNUDefects),
                            new XElement("HotPixelDefects", darkCurrentTestResultsData.HotPixelDefects),
                            new XElement("TrapDefects", darkCurrentTestResultsData.TrapDefects),
                            new XElement("ClusterDefects", darkCurrentTestResultsData.ClusterDefects),
                            new XElement("TotalDefects", darkCurrentTestResultsData.TotalDefects),
                            new XElement("MaxDarkFF", darkCurrentTestResultsData.MaxDarkFF),
                            new XElement("AveDarkFF", darkCurrentTestResultsData.AveDarkFF),
                            new XElement("MaxTrap", darkCurrentTestResultsData.MaxTrap),
                            new XElement("AveTrap", darkCurrentTestResultsData.AveTrap),
                            new XElement("MaxDarkROI", darkCurrentTestResultsData.MaxDarkROI),
                              new XElement("DeadPixels", darkCurrentTestResultsData.DeadPixels),
                            new XElement("BadColumns", darkCurrentTestResultsData.BadColumns),
                            new XElement("BadRows", darkCurrentTestResultsData.BadRows),
                            new XElement("DefectsMean", darkCurrentTestResultsData.DefectsMean),
                            new XElement("TestPassed", darkCurrentTestResultsData.TestPassed),
                            new XElement("TestResultImage", darkCurrentTestResultsData.TestResultImagePath))
                        );
                    xmlDarkCurrentResultsDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "DarkCurrentResultsData.xml");
                    log.Info("End saving Dark Current Test Results to DarkCurrentResultsData XML.");
                }
                else
                {
                    log.Error("DarkCurrentResultsData xml is null");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void SaveInjectionPerformanceResultsInXML(InjectionEfficiencyTestResults injectionPerformanceTestResults)
        {
            try
            {
                log.Info("Begin saving Injection Performance Test Results to InjectionPerformanceResultsData XML.");
                XDocument xmlInjectionPerfResultsDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "InjectionPerformanceResultsData.xml");
                if (xmlInjectionPerfResultsDoc != null)
                {
                    xmlInjectionPerfResultsDoc.Element("InjectionPerformanceTestResults").Add
                    (
                        new XElement("TestDetails",
                        new XElement("ResultsID", DBExists ? injectionPerformanceTestResults.ResultsID : injectionEfficiencyTestResultsList.Count + 1),
                        new XElement("TestResultsID", injectionPerformanceTestResults.TestResultsID),
                        new XElement("DateExecuted", injectionPerformanceTestResults.DateExecuted),
                        new XElement("UserExecuted", injectionPerformanceTestResults.UserExecuted.ToUpper()),
                        new XElement("InitialExposure", injectionPerformanceTestResults.InitialExposure),
                        new XElement("FirstInjection", injectionPerformanceTestResults.FirstInjection),
                         new XElement("SecondInjection", injectionPerformanceTestResults.SecondInjection),
                              new XElement("ThirdInjection", injectionPerformanceTestResults.ThirdInjection),
                               new XElement("FourthInjection", injectionPerformanceTestResults.FourthInjection),
                        new XElement("LastInjection", injectionPerformanceTestResults.LastInjection),
                         new XElement("Exp0PlotData", injectionPerformanceTestResults.Exp0PlotData),
                         new XElement("Exp1PlotData", injectionPerformanceTestResults.Exp1PlotData),
                         new XElement("Exp2PlotData", injectionPerformanceTestResults.Exp2PlotData),
                         new XElement("Exp3PlotData", injectionPerformanceTestResults.Exp3PlotData),
                         new XElement("Exp4PlotData", injectionPerformanceTestResults.Exp4PlotData),
                        new XElement("TestPassed", injectionPerformanceTestResults.TestPassed),
                        new XElement("TestResultImage", injectionPerformanceTestResults.TestResultImagePath))
                    );
                    xmlInjectionPerfResultsDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "InjectionPerformanceResultsData.xml");
                    log.Info("End saving Injection Performance Test Results to InjectionPerformanceResultsData XML.");
                }
                else
                {
                    log.Error("InjectionPerformanceResultsData xml is null");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void SaveReadNoiseResultsInXML(NoiseVsNDROTestResults noiseVsNDROTestResults)
        {
            try
            {
                log.Info("Begin saving Noise Vs NDROs Test Results to NoiseVsNDROsResultsData XML.");
                XDocument xmlReadNoiseResultsDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "NoiseVsNDROsResultsData.xml");
                if (xmlReadNoiseResultsDoc != null)
                {
                    xmlReadNoiseResultsDoc.Element("NoiseVsNDROsTestResults").Add
                    (
                        new XElement("TestDetails",
                        new XElement("ResultsID", noiseVsNDROTestResults.ResultsID),
                        new XElement("TestResultsID", noiseVsNDROTestResults.TestResultsID),
                        new XElement("DateExecuted", noiseVsNDROTestResults.DateExecuted),
                        new XElement("UserExecuted", noiseVsNDROTestResults.UserExecuted.ToUpper()),
                        new XElement("ExposureNDRO", noiseVsNDROTestResults.ExposureNDRO),
                        new XElement("NoiseRatio", noiseVsNDROTestResults.NoiseRatio),
                        new XElement("Noise", noiseVsNDROTestResults.Noise),
                        new XElement("R2Correlation", noiseVsNDROTestResults.R2Correlation),
                        new XElement("BestFit", noiseVsNDROTestResults.BestFit),
                        new XElement("BestFitPower", noiseVsNDROTestResults.BestFitPower),
                         new XElement("TheoryValue", noiseVsNDROTestResults.TheoryValue),
                        new XElement("SignalReadNoise", noiseVsNDROTestResults.SignalReadNoise),
                        new XElement("NDRONoise", noiseVsNDROTestResults.NDRONoise),
                        new XElement("TestPassed", noiseVsNDROTestResults.TestPassed),
                        new XElement("TestResultImage", noiseVsNDROTestResults.TestResultImagePath))
                    );
                    xmlReadNoiseResultsDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "NoiseVsNDROsResultsData.xml");
                    log.Info("End saving Noise Vs NDROs Test Results to NoiseVsNDROsResultsData XML.");
                }
                else
                {
                    log.Error("NoiseVsNDROsResultsData xml is null");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void SavePhotoresponseResultsInXML(PhotoresponseTestResults photoresponseTestResults)
        {
            try
            {
                log.Info("Begin saving Photoresponse Test Results to PhotoresponseResultsData XML.");
                XDocument xmlPhotoresponseResultsDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "PhotoresponseResultsData.xml");
                if (xmlPhotoresponseResultsDoc != null)
                {
                    xmlPhotoresponseResultsDoc.Element("PhotoresponseTestResults").Add
                        (
                            new XElement("TestDetails",
                            new XElement("ResultsID", photoresponseTestResults.ResultsID),
                            new XElement("TestResultsID", photoresponseTestResults.TestResultsID),
                            new XElement("DateExecuted", photoresponseTestResults.DateExecuted),
                            new XElement("UserExecuted", photoresponseTestResults.UserExecuted.ToUpper()),
                            new XElement("Mean", photoresponseTestResults.Mean),
                            new XElement("LinearCurveFit", photoresponseTestResults.LinearCurveFit),
                            new XElement("LinearFitData", photoresponseTestResults.LinearFitData),
                            new XElement("FullSaturation", photoresponseTestResults.FullSaturation),
                            new XElement("LinearSaturation", photoresponseTestResults.LinearSaturation),
                            new XElement("Slope", photoresponseTestResults.Slope),
                            new XElement("Intercept", photoresponseTestResults.Intercept),
                              new XElement("DerivPlot", photoresponseTestResults.DerivPlot),
                            new XElement("TestPassed", photoresponseTestResults.TestPassed),
                            new XElement("TestResultImage", photoresponseTestResults.TestResultImagePath))
                        );
                    xmlPhotoresponseResultsDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "PhotoresponseResultsData.xml");
                    log.Info("End saving Photoresponse Test Results to PhotoresponseResultsData XML.");
                }
                else
                {
                    log.Error("PhotoresponseResultsData XML is null");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void SaveRedBlueResultsInXML(RedBlueTestResults redBlueTestResults)
        {
            try
            {
                log.Info("Begin saving Red Blue Test Results to RedBlueResultsData XML.");
                XDocument xmlRedBlueResultsDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "RedBlueResultsData.xml");
                if (xmlRedBlueResultsDoc != null)
                {
                    xmlRedBlueResultsDoc.Element("RedBlueTestResults").Add
                        (
                            new XElement("TestDetails",
                            new XElement("ResultsID", DBExists ? redBlueTestResults.ResultsID : redBlueTestResultsList.Count + 1),
                            new XElement("TestResultsID", redBlueTestResults.TestResultsID),
                            new XElement("DateExecuted", redBlueTestResults.DateExecuted),
                            new XElement("UserExecuted", redBlueTestResults.UserExecuted.ToUpper()),
                            new XElement("RedImageData", redBlueTestResults.RedImageData),
                            new XElement("BlueImageData", redBlueTestResults.BlueImageData),
                            new XElement("RedBlueImageDifference", redBlueTestResults.RedBlueImageDifference),
                            new XElement("TestPassed", redBlueTestResults.TestPassed),
                            new XElement("TestResultImage", redBlueTestResults.TestResultImagePath))
                        );
                    xmlRedBlueResultsDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "RedBlueResultsData.xml");
                    log.Info("End saving Red Blue Test Results to RedBlueResultsData XML.");
                }
                else
                {
                    log.Error("RedBlueResultsData XML is null");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void SaveShutterDriveResultsInXML(ShutterDriveTestResults shutterDriveTestResults)
        {
            try
            {
                log.Info("Begin saving Shutter Drive Test Results to ShutterDriveResultsData XML.");
                XDocument xmlShutterDriveResultsDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "ShutterDriveResultsData.xml");
                if (xmlShutterDriveResultsDoc != null)
                {
                    xmlShutterDriveResultsDoc.Element("ShutterDriveTestResults").Add
                          (
                              new XElement("TestDetails",
                              new XElement("ResultsID", shutterDriveTestResults.ResultsID),
                              new XElement("TestResultsID", shutterDriveTestResults.TestResultsID),
                              new XElement("DateExecuted", shutterDriveTestResults.DateExecuted),
                              new XElement("UserExecuted", shutterDriveTestResults.UserExecuted.ToUpper()),
                              new XElement("TestPassed", shutterDriveTestResults.TestPassed),
                              new XElement("TestResultImage", shutterDriveTestResults.TestResultImagePath),
                              new XElement("LabJackSN", shutterDriveTestResults.LabJackSN),
                              new XElement("ShutterPlot", shutterDriveTestResults.ShutterPlot))
                          );
                    xmlShutterDriveResultsDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "ShutterDriveResultsData.xml");
                    log.Info("End saving Shutter Drive Test Results to ShutterDriveResultsData XML.");
                }
                else
                {
                    log.Error("ShutterDriveResultsData XML is null");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void SaveFinalResultsInXML(FinalTestResultsDetailsModel finalTestResultsDetailsModel)
        {
            try
            {
                log.Info("Begin saving Final Test Results to FinalTestResultsData XML.");
                XDocument xmlFinalResultsDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "FinalTestResultsData.xml");
                if (xmlFinalResultsDoc != null)
                {
                    xmlFinalResultsDoc.Element("FinalTestResults").Add
                        (
                              new XElement("TestDetails",
                              new XElement("FinalTestID", DBExists ? finalTestResultsDetailsModel.FinalTestID : finalTestResultsDetailsModelList.Count() + 1),
                              new XElement("TestResultsID", DBExists ? overAllTestResultsModel.TestResultsID : (overAllTestResultsModelList.Count() + 1)),
                              new XElement("ConversionFactorNominal", finalTestResultsDetailsModel.ConversionFactorNominal),
                              new XElement("TotalNumDefects", finalTestResultsDetailsModel.TotalNumDefects),
                              new XElement("TotalNumClusters", finalTestResultsDetailsModel.TotalNumClusters),
                               new XElement("TotalColumnDefects", finalTestResultsDetailsModel.TotalColumnDefects),
                                new XElement("DriftROICount", finalTestResultsDetailsModel.DriftROICount),
                              new XElement("TotalRowDefects", finalTestResultsDetailsModel.TotalRowDefects),
                              new XElement("AveTrap", finalTestResultsDetailsModel.AveTrap),
                              new XElement("MaxDarkROI", finalTestResultsDetailsModel.MaxDarkROI),
                              new XElement("FullWellLevelMin", finalTestResultsDetailsModel.FullWellLevelMin),
                              new XElement("SnglNoiseMax", finalTestResultsDetailsModel.SnglNoiseMax),
                              new XElement("PowerFitUpper", finalTestResultsDetailsModel.PowerFitUpper),
                              new XElement("PowerFitLower", finalTestResultsDetailsModel.PowerFitLower),
                              new XElement("RPower2Correlation", finalTestResultsDetailsModel.RPower2Correlation),
                              new XElement("InjEfficiencyInitialExposure", finalTestResultsDetailsModel.InjEfficiencyInitialExposure),
                              new XElement("InjEfficiencyFirstInjection", finalTestResultsDetailsModel.InjEfficiencyFirstInjection),
                              new XElement("InjEfficiencyLastInjection", finalTestResultsDetailsModel.InjEfficiencyLastInjection),
                              new XElement("ShutterDrive", finalTestResultsDetailsModel.ShutterDrive)));
                    xmlFinalResultsDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "FinalTestResultsData.xml");
                    log.Info("End saving Final Test Results to FinalTestResultsData XML.");
                }
                else
                {
                    log.Error("FinalTestResultsData XML is null");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void SaveCameraDetailsInXML(CameraDetailsModel cameraDetailsModel)
        {
            try
            {
                log.Info("Begin saving Final Test Results to FinalTestResultsData XML.");
                XDocument xmlCameraDetailsDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "CameraDetails.xml");
                if (xmlCameraDetailsDoc != null)
                {
                    xmlCameraDetailsDoc.Element("CameraDetails").Add
                        (
                             new XElement("CameraData",
                             new XElement("CameraTestID", cameraDetailsModel.CameraTestID),
                             new XElement("TestResultsID", cameraDetailsModel.TestResultsID),
                             new XElement("CameraModelNumber", cameraDetailsModel.CameraModelNumber),
                             new XElement("CameraSerialNumber", cameraDetailsModel.CameraSerialNumber),
                             new XElement("ImagerSerialNumber", cameraDetailsModel.ImagerSerialNumber),
                             new XElement("DetectorType", cameraDetailsModel.DetectorType),
                             new XElement("FirmwareVersion", cameraDetailsModel.FirmwareVer),
                             new XElement("AlteraVersion", cameraDetailsModel.FPGAVersion),
                             new XElement("MACAddress", cameraDetailsModel.MACAddress),
                             new XElement("CRA", cameraDetailsModel.CRA),
                             new XElement("CPUSerialNumber", cameraDetailsModel.CPUSerialNumber),
                             new XElement("PowerSerialNumber", cameraDetailsModel.PowerSerialNumber),
                             new XElement("CSPSerialNumber", cameraDetailsModel.CSPSerialNumber),
                             new XElement("ISISerialNumber", cameraDetailsModel.ISISerialNumber),
                             new XElement("ProductShippedDate", cameraDetailsModel.ProductShippedDate),
                             new XElement("DefinedDate", cameraDetailsModel.DateDefined),
                             new XElement("UserExecuted", cameraDetailsModel.UserExecuted.ToUpper()),
                             new XElement("TestSoftwareVersion", cameraDetailsModel.TestSoftwareVer)));
                    xmlCameraDetailsDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "CameraDetails.xml");
                    log.Info("End saving Final Test Results to FinalTestResultsData XML.");
                }
                else
                {
                    log.Error("FinalTestResultsData XML is null");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
