using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Thermo.Kronos.Instrument.Camera.Interface;
using System.Data.Entity.Migrations;
using System.IO;
using System.Drawing;
using System.Configuration;
using NationalInstruments.Analysis.Math;
using CIDSoftwareApplication;
namespace KronosCameraTestApp.Presenter
{
    public class PhotoresponsePresenter
    {
        KronosTestAppMainForm kronostestappMainForm { get; set; }
        PhotoresponseTestForm photoresponseTestForm { get; set; }
        PhotoresponseTestResults photoresponseTestResults { get; set; }
        PhotoresponseLimitsData photoresponseLimitsData { get; set; }
        public PhotoresponseLimitsData PhotoresponseLimitsData
        {
            get { return photoresponseLimitsData; }
            set { photoresponseLimitsData = value; }
        }
        List<PhotoresponseLimitsData> photoresponseLimitsDataList { get; set; }
        public List<PhotoresponseLimitsData> PhotoresponseLimitsDataList
        {
            get { return photoresponseLimitsDataList; }
            set { photoresponseLimitsDataList = value; }
        }
        bool photoresponseTestPassed = true;
        bool repeatExposureSetting = false;
        public bool RepeatExposureSetting
        {
            get { return repeatExposureSetting; }
            set { repeatExposureSetting = value; }
        }
        public bool PhotoresponseTestPassed
        {
            get { return photoresponseTestPassed; }
            set { photoresponseTestPassed = value; }
        }       
        List<PhotoresponseTestResults> photoresponseTestResultsList { get; set; }
        string enggUserName = string.Empty;
        public string EnggUserName
        {
            get { return enggUserName; }
            set { enggUserName = value; }
        }
        CommandProcessor commandProcessor = null;     
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CIDInterface cidInterface = null;
        KronosCamContext kronosCamContext { get; set; }
        String path = string.Empty;    
        public bool DBExists = false;
        XDocument xmlResultsDoc;
        double[] mean;
        double[] linearFitData;
        double[] linearCurveFit;
        double[] derivPlot;
        public double[] DerivPlot
        {
            get { return derivPlot; }
            set { derivPlot = value; }
        }
        double[] deriv;
        double[] xData;
        public double[] XData
        {
            get { return xData; }
            set { xData = value; }
        }
        double[] yData;
        public double[] YData
        {
            get { return yData; }
            set { yData = value; }
        }
        double fullWell = 0;
        public double[] MeanValue
        {
            get { return mean; }
            set { mean = value; }
        }
        public double[] LinearFitData
        {
            get { return linearFitData; }
            set { linearFitData = value; }
        }
        public double[] LinearCurveFit
        {
            get { return linearCurveFit; }
            set { linearCurveFit = value; }
        }
        public double FullWell
        {
            get { return fullWell; }
            set { fullWell = value; }
        }
        int slope =0;
        public int Slope
        {
            get { return slope; }
            set { slope = value; }
        }
        double  intercept = 0;
        public double Intercept
        {
            get { return intercept; }
            set { intercept = value; }
        }
        double linearSaturationResult=0;
        string linearFullWell = string.Empty;
        public string LinearFullWell
        {
            get { return linearFullWell; }
            set { linearFullWell = value; }
        }
        ExposureDataModel exposureDataModel { get; set; }
        SubarrayDataModel subarrayDataModel { get; set; }
        public ExposureDataModel ExposureDataModel
        {
            get { return exposureDataModel; }
            set { exposureDataModel = value; }
        }
        public SubarrayDataModel SubarrayDataModel
        {
            get { return subarrayDataModel; }
            set { subarrayDataModel = value; }
        }
        List<ExposureDataModel> exposureDataList = new List<ExposureDataModel>();
        List<SubarrayDataModel> subarrayDataModelList = new List<SubarrayDataModel>();
        public List<ExposureDataModel> ExposureDataList
        {
            get { return exposureDataList; }
            set { exposureDataList = value; }
        }
        public List<SubarrayDataModel> SubarrayDataModelList
        {
            get { return subarrayDataModelList; }
            set { subarrayDataModelList = value; }
        }
        int exposureCount = 0;
        public int ExposureCount
        {
            get { return exposureCount; }
            set { exposureCount = value; }
        }
        bool testResultsSaved = true;
        string testResultImagePath = string.Empty;
        public string TestResultImagePath
        {
            get { return testResultImagePath; }
            set { testResultImagePath = value; }
        }
        string photoresponseTestFailureReason = string.Empty;
        public string PhotoresponseTestFailureReason
        {
            get { return photoresponseTestFailureReason; }
            set { photoresponseTestFailureReason = value; }
        }
        public PhotoresponsePresenter(PhotoresponseTestForm photoresponseTestForm, KronosTestAppMainForm kronostestappMainForm)
        {
            try
            {
                this.photoresponseTestForm = photoresponseTestForm;
                photoresponseTestForm.photoresponsePresenter = this;
                kronostestappMainForm.photoresponsePresenter = this;
                exposureDataModel = new ExposureDataModel();
                subarrayDataModel = new SubarrayDataModel();
                subarrayDataModelList = new List<SubarrayDataModel>();
                exposureDataList = new List<ExposureDataModel>();
                this.kronostestappMainForm = kronostestappMainForm;
                path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                commandProcessor = new CommandProcessor(cidInterface);  
                photoresponseTestResultsList = new List<PhotoresponseTestResults>();
                photoresponseLimitsDataList = new List<PhotoresponseLimitsData>();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task GetPhotoresponseData(CIDInterface cidInterface)
        {
            try
            {
                kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                this.cidInterface = cidInterface;
                if (TestAppHelper.CheckDBExists())
                {
                    await PopulatePhotoresponseEntity();
                    DBExists = true;
                }
                else
                {
                    await ProcessPhotoresponseXMLFile();
                    log.Info("Photoresponse test data populated from XML Repository.");
                    DBExists = false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        } 
        public async Task ProcessPhotoresponseResultsXMLFile()
        {
            try
            {
                await Task.Run(() =>
                {
                    log.Info("Begin Reading PhotoresponseResultsData XML.");
                    if (testResultsSaved)
                    {
                        DataSet ds = new DataSet();
                        ds.ReadXml(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "PhotoresponseResultsData.xml", XmlReadMode.InferSchema);
                        if (ds.Tables.Count > 0)
                        {
                            DataView dvExposure;
                            dvExposure = ds.Tables[0].DefaultView;
                            foreach (DataRowView dr in dvExposure)
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
                                m.DerivPlot = Convert.ToString(dr[11]);
                                m.TestPassed = Convert.ToBoolean(dr[12]);
                                m.TestResultImagePath = Convert.ToString(dr[13]);
                                photoresponseTestResultsList.Add(m);
                            }
                            if (photoresponseTestResultsList.Count > 0)
                            {
                                log.Info("End Reading PhotoresponseResultsData XML.");
                                testResultsSaved = false;
                            }
                            else
                            {
                                log.Error("Error Loading PhotoresponseResultsData XML");
                                testResultsSaved = true;
                            }
                        }
                    }                    
                });
            }
            catch (Exception ex)
            {
                testResultsSaved = true;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task WriteTestResultsToXML()
        {
            try
            {
                await Task.Run(() =>
              {
                  log.Info("Begin saving Photoresponse Test Results to PhotoresponseResultsData XML.");
                  xmlResultsDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "PhotoresponseResultsData.xml");
                  if (xmlResultsDoc != null)
                  {
                      xmlResultsDoc.Element("PhotoresponseTestResults").Add
                           (
                               new XElement("TestResult",
                               new XElement("ResultsID", DBExists ? photoresponseTestResults.ResultsID : photoresponseTestResultsList.Count + 1),
                               new XElement("TestResultsID", photoresponseTestResults.TestResultsID),
                               new XElement("DateExecuted", photoresponseTestResults.DateExecuted),
                               new XElement("UserExecuted", photoresponseTestResults.UserExecuted),
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
                      xmlResultsDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "PhotoresponseResultsData.xml");
                      testResultsSaved = true;
                      log.Info("End saving Photoresponse Test Results to PhotoresponseResultsData XML.");
                  }
                  else
                  {
                      log.Error("PhotoresponseResultsData XML is null");
                      testResultsSaved = false;
                  }
              });
            }
            catch (Exception ex)
            {
                testResultsSaved = false;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public PhotoresponseTestResults ReturnPhotoresponseResults()
        {
            try
            {
                photoresponseTestResults = new PhotoresponseTestResults();
                photoresponseTestResults.UserExecuted = string.IsNullOrWhiteSpace(enggUserName) ? Environment.UserName.ToUpper() : enggUserName.ToUpper();
                photoresponseTestResults.Mean = String.Join(",", mean.Select(p => p.ToString()).ToArray());
                photoresponseTestResults.LinearCurveFit = String.Join(",", linearCurveFit.Select(p => p.ToString()).ToArray());
                photoresponseTestResults.LinearFitData = String.Join(",", linearFitData.Select(p => p.ToString()).ToArray());
                photoresponseTestResults.FullSaturation = fullWell;
                photoresponseTestResults.LinearSaturation = linearSaturationResult;
                photoresponseTestResults.Slope = slope;
                photoresponseTestResults.Intercept = intercept;
                photoresponseTestResults.DerivPlot = String.Join(",", derivPlot.Select(p => p.ToString()).ToArray()); ;
                photoresponseTestResults.TestResultImagePath = testResultImagePath;
                photoresponseTestResults.DateExecuted = DateTime.Now;
                photoresponseTestResults.TestPassed = photoresponseTestPassed;
                photoresponseTestResultsList.Add(photoresponseTestResults);
                return photoresponseTestResults;
            }
            catch (Exception ex)
            {
                testResultsSaved = false;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }
        public async Task runPhotoresponseTest(CIDInterface cidInterface, CancellationToken ct, string userMode, int expCount=200)
        {
            try
            {
                if (!ct.IsCancellationRequested)
                {
                    kronostestappMainForm.cidDataList.Clear();
                    kronostestappMainForm.cidIntegratedDataList.Clear();
                    commandProcessor.warmupTest(cidInterface);
                    this.exposureCount = expCount;
                    log.Info("Begin Photoresponse test.");
                    this.cidInterface = cidInterface;
                    if (cidInterface != null)
                        cidInterface.ResetExposureId();
                    photoresponseTestPassed = false;
                    kronostestappMainForm.cidDataList.Clear();
                    kronostestappMainForm.cidIntegratedDataList.Clear();
                    //if (repeatExposureSetting || TestAppHelper.repeatPhotoResponseExposure)
                    //{
                    //    await FillRepeatExposureSetting();
                    //}
                    //else
                    //{
                    //    await FillExposureSetting();
                    //}
                    //await FillExposureSetting();
                    if (!ct.IsCancellationRequested)
                    {
                        //process the photoresponse model data and populate exposure structures                      
                        await commandProcessor.ProcessExposureData(exposureDataList, ct);
                        //send exposure data to camera
                        await commandProcessor.SendExposure(ct, exposureCount, cidInterface); 
                    }
                    else
                    {
                        return;
                    }
                    exposureDataList= exposureDataList.Take(1).ToList();
                    log.Info("End Photoresponse test.");
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task FillRepeatExposureSetting()
        {
            try
            {
                await Task.Run(() =>
                    {
                        //exposureDataList.Clear();
                        for (int i = 0; i < exposureCount-1; i++)
                        {
                            exposureDataList.Add(exposureDataModel);
                        }
                    });
            }
             catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task FillExposureSetting()
        {
            try
            {
                await Task.Run(() =>
                {
                    if (exposureDataList.Count <= 1 )
                    {
                        for (int i = 0; i < exposureCount - 1; i++)
                        {
                            exposureDataList.Add(exposureDataModel);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task PopulatePhotoresponseEntity()
        {
            try
            {
                if (TestAppHelper.CheckDBExists())
                {
                    DBExists = true;
                    await Task.Run(() =>
                    {
                        using (var kcc = new KronosCamContext())
                        {
                            exposureDataList = kcc.exposureDataModel.Include("SubarrayDatas").Where(mv => mv.TestDataID.Equals("PRT")).ToList();
                            List<int> exposureIDList = (from exp in exposureDataList.Where(e => e.TestDataID.Equals("PRT")) select exp.ExposureID).ToList();
                            subarrayDataModelList = (from mvsu in kcc.subarrayDataModel orderby mvsu.ExposureID where exposureIDList.Contains(mvsu.ExposureID) select mvsu).ToList();
                            log.Info("Photoresponse Test data populated from Database with Model & Subarray Records of ." + 
                                exposureDataList.Count().ToString() + subarrayDataModelList.Count.ToString());
                        }
                    });
                }
                else
                {
                    //get the data from script files
                    await ProcessPhotoresponseXMLFile();
                    DBExists = false;
                    log.Info("Photoresponse Test data populated from Database with Model & Subarray Records of ." + 
                        exposureDataList.Count().ToString() + subarrayDataModelList.Count.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }        
        public async Task CalculatePhotoresponse(CancellationToken ct)
        {
            try
            {
                await Task.Run(() =>
                {
                    log.Info("Begin Calcluate Photoresponse for Exposure Count " + exposureCount.ToString());
                    while (kronostestappMainForm.cidIntegratedDataList.Count < exposureCount)
                    {
                        if (ct.IsCancellationRequested)
                        {                            
                            kronostestappMainForm.cidDataList.Clear();
                            kronostestappMainForm.cidIntegratedDataList.Clear();
                            return;
                        }
                        if (!cidInterface.IsConnected())
                        {
                            //MessageBox.Show("Camera got disconnected, Trying to reconnect.....");
                            log.Error("Camera disconnected during Photoresonse test " + kronostestappMainForm.cidIntegratedDataList.Count.ToString());
                            //cidInterface.Disconnect();
                            //kronostestappMainForm.AutoConnectCamera();
                            //if (cidInterface.IsConnected())
                            //    MessageBox.Show("Camera reconnected successfully.");
                        }
                    }
                    CID821_Data cid821Data = new CID821_Data();
                    cid821Data = kronostestappMainForm.cidIntegratedDataList[0];
                    int length = (cid821Data.dr) * (cid821Data.dc);
                    double[] ROI1 = new double[length];
                    mean = new double[kronostestappMainForm.cidIntegratedDataList.Count];
                    double[] exposures = new double[kronostestappMainForm.cidIntegratedDataList.Count];
                    for (int i = 0; i < exposureCount; i++)
                    {                      
                        ROI1 = TestAppHelper.convertVideoDataToDouble(kronostestappMainForm.cidIntegratedDataList[i].videoIntegratedDataList);
                        if (i == 0)
                            mean[i] = 0;
                        else
                            mean[i] = (TestAppHelper.calculateMeanDouble(ROI1));
                        exposures[i] = i;
                    }
                    double rms = Statistics.StandardDeviation(ROI1);
                    double variance = (rms * rms) / 2.0;
                    deriv = new double[exposureCount];
                    linearCurveFit = new double[exposureCount];
                    linearCurveFit = CurveFit.LinearFit(mean, exposures);
                    derivPlot = new double[exposureCount - 1];
                    deriv = NationalInstruments.Analysis.Dsp.SignalProcessing.Differentiate(mean, 1, (mean[0] - 10), (mean[exposureCount - 1] + 10));
                    for (int i = 0; i < exposureCount - 1; i++)
                    {
                        derivPlot[i] = deriv[i + 1];
                    }
                    const int NUMBER_OF_BEST_FIT = 122;
                    const int START_BEST_FIT = 25;
                    xData = new double[NUMBER_OF_BEST_FIT];
                    yData = new double[NUMBER_OF_BEST_FIT];
                    for (int i = START_BEST_FIT; i < NUMBER_OF_BEST_FIT; i++)
                    {
                        xData[i] = (double)i;
                        yData[i] = mean[i];
                    }
                    var linearyFitData = CurveFit.LinearFit(xData, yData);
                    fullWell = mean.Max();
                    slope = (int)((yData[START_BEST_FIT] - yData[115]) / (xData[START_BEST_FIT] - xData[115]));
                    intercept = mean[1];
                    for (int i = START_BEST_FIT; i < NUMBER_OF_BEST_FIT; i++)
                    {
                        var increase = linearyFitData[i] - mean[i];
                        var percent = (increase / linearyFitData[i]) * 100;
                        if (percent > 2)
                        {
                            linearFullWell = mean[i].ToString();
                        }
                    }
                    //remove the first element from plots.
                    derivPlot = derivPlot.Skip(1).ToArray();
                    mean = mean.Skip(3).ToArray();
                    //remove the last element from plots.
                    mean = mean.Take(mean.Count() - 1).ToArray();
                    derivPlot = derivPlot.Take(derivPlot.Count() - 1).ToArray();
                    if (fullWell >= photoresponseLimitsData.FullWellLevelMin)
                        photoresponseTestPassed = true;
                    else
                    {
                        photoresponseTestFailureReason = "Full Well";
                        photoresponseTestPassed = false;
                    }
                       
                });
                log.Info("End Calcluate Photoresponse for Exposure Count " + exposureCount.ToString());
                kronostestappMainForm.cidDataList.Clear();
                kronostestappMainForm.cidIntegratedDataList.Clear();
            }
            catch (Exception ex)
            {
                photoresponseTestPassed = false;
                kronostestappMainForm.cidDataList.Clear();
                kronostestappMainForm.cidIntegratedDataList.Clear();
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task WritePhotoresponseTestResultToDB(CancellationToken ct, string TestResultImageFilePath)
        {
            try
            {
                kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                //write to DB
                if (ct.IsCancellationRequested)
                    return;
                photoresponseTestResults = new PhotoresponseTestResults();
                photoresponseTestResults.UserExecuted = string.IsNullOrWhiteSpace(enggUserName) ? Environment.UserName : enggUserName;
                photoresponseTestResults.Mean = String.Join(",", mean.Select(p => p.ToString()).ToArray());
                photoresponseTestResults.LinearCurveFit = String.Join(",", linearCurveFit.Select(p => p.ToString()).ToArray());
                photoresponseTestResults.LinearFitData = String.Join(",", linearFitData.Select(p => p.ToString()).ToArray());
                photoresponseTestResults.FullSaturation = fullWell;
                photoresponseTestResults.LinearSaturation = linearSaturationResult;
                photoresponseTestResults.Slope = slope;
                photoresponseTestResults.Intercept = intercept;
                photoresponseTestResults.DerivPlot = String.Join(",", derivPlot.Select(p => p.ToString()).ToArray()); ;
                photoresponseTestResults.TestResultImagePath = TestResultImageFilePath;
                photoresponseTestResultsList.Add(photoresponseTestResults);
                if (TestAppHelper.CheckDBExists())
                {
                    await SavePhotoresponseTestResultsToDB();
                }
                log.Info("End Photoresponse Test Results Saving to DB");
                await ProcessPhotoresponseResultsXMLFile();
                //write test results to XML
                await WriteTestResultsToXML();
                //ExcelUtilities.WriteTestResultsToExcel("Photoresponse Test", photoresponseTestResults);
            }
            catch (Exception ex)
            {
                testResultsSaved = false;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task SavePhotoresponseTestResultsToDB()
        {
            try
            {
                await Task.Run(() =>
                {
                    using (var kcc = new KronosCamContext())
                    {
                        log.Info("Begin Mean Variance Test Results Saving to DB");
                        kcc.photoresponseTestResults.Add(photoresponseTestResults);
                        int savedTestResults = kcc.SaveChanges();
                        if (savedTestResults > 0)
                        {
                            testResultsSaved = true;
                            log.Info("Mean Variance Test Results Saved to DB");
                        }
                        else
                        {
                            testResultsSaved = false;
                            log.Error("Error occured while saving Mean Variance Test Results to DB");
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                testResultsSaved = false;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task ProcessPhotoresponseXMLFile()
        {
            try
            {
                await Task.Run(() =>
                {
                    log.Info("Begin Reading Photoresponse Data XML.");                 
                    commandProcessor.ProcessExposureXMLData("PhotoresponseData.xml");
                    exposureDataList = new List<ExposureDataModel>();
                    subarrayDataModelList = new List<SubarrayDataModel>();
                    exposureDataList = commandProcessor.ExposureDataList;
                    subarrayDataModelList = commandProcessor.SubarrayDataModelList;
                });
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task GetLimits()
        {
            try
            {
                if (TestAppHelper.CheckDBExists())
                {
                    DBExists = true;
                    await Task.Run(() =>
                    {
                        //comment below line while unit testing
                        kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                        using (var kcc = new KronosCamContext())
                        {
                            photoresponseLimitsDataList = (from limits in kcc.photoresponseLimitsData orderby limits.DefinedDate descending select limits).ToList();
                        }
                    });
                }
                else
                {
                    DBExists = false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
