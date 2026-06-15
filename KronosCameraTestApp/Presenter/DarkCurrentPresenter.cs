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
using CIDSoftwareApplication;
namespace KronosCameraTestApp.Presenter
{
    public class DarkCurrentPresenter
    {
        DarkCurrentTestForm darkCurrentTestForm { get; set; }
        KronosTestAppMainForm kronostestappMainForm { get; set; }
        List<DefectsTestLimitsData> defectsTestLimitsDataList { get; set; }
        public List<DefectsTestLimitsData> DefectsTestLimitsDataList
        {
            get { return defectsTestLimitsDataList; }
            set { defectsTestLimitsDataList = value; }
        }
        DefectsTestLimitsData defectsTestLimitsData { get; set; }
        DarkCurrentTestLimitsData darkCurrentTestLimitsData { get; set; }
        public DarkCurrentTestLimitsData DarkCurrentTestLimitsData
        {
            get { return darkCurrentTestLimitsData; }
            set { darkCurrentTestLimitsData = value; }
        }
        List<DarkCurrentTestLimitsData> darkCurrentTestLimitsDataList { get; set; }
        public List<DarkCurrentTestLimitsData> DarkCurrentTestLimitsDataList
        {
            get { return darkCurrentTestLimitsDataList; }
            set { darkCurrentTestLimitsDataList = value; }
        }
        DarkCurrentTestResults darkCurrentTestResultsData { get; set; }      
        List<DarkCurrentTestResults> darkCurrentTestResultsDataList;
        List<ExposureDataModel> exposureDataList = new List<ExposureDataModel>();
        List<SubarrayDataModel> subarrayDataModelList = new List<SubarrayDataModel>();
        ExposureDataModel exposureDataModel { get; set; }
        SubarrayDataModel subarrayDataModel { get; set; }
        CommandProcessor commandProcessor = null;
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CIDInterface cidInterface = null;
        String path = string.Empty;
        public bool DBExists = false;
        int badRows = 0;
        public int BadRows
        {
            get { return badRows; }
            set { badRows = value; }
        }
        int badColumns = 0;
        public int BadColumns
        {
            get { return badColumns; }
            set { badColumns = value; }
        }
        int deadPixels = 0;
        public int DeadPixels
        {
            get { return deadPixels; }
            set { deadPixels = value; }
        }
        double defectsMean = 0;
        public double DefectsMean
        {
            get { return defectsMean; }
            set { defectsMean = value; }
        }
        double percentBelowMin = 0;
        public double PercentBelowMin
        {
            get { return percentBelowMin; }
            set { percentBelowMin = value; }
        }
        bool darkCurrentTestPassed = true;
        public bool DarkCurrentTestPassed
        {
            get { return darkCurrentTestPassed; }
            set { darkCurrentTestPassed = value; }
        }
        string testResultImagePath = string.Empty;
        public string TestResultImagePath
        {
            get { return testResultImagePath; }
            set { testResultImagePath = value; }
        }
        public struct DefectsDataStructure
        {
            public int xPosition;
            public int yPosition;
            public int value;
            public bool clusterDefect;
            public bool PRNUDefect;
            public bool TrapDefect;
            public bool HotPixelDefect;
        }
        private List<DefectsDataStructure> defectsDataList = new List<DefectsDataStructure>();
        private static uint xArraySize = 2048;
        private static uint yArraySize = 2048;
        int maxDarkFF = 0;
        public int MaxDarkFF
        {
            get { return maxDarkFF; }
            set { maxDarkFF = value; }
        }
        int aveDarkFF = 0;
        public int AveDarkFF
        {
            get { return aveDarkFF; }
            set { aveDarkFF = value; }
        }
        int prnuDefectsCount = 0;
        public int PRNUDefectsCount
        {
            get { return prnuDefectsCount; }
            set { prnuDefectsCount = value; }
        }
        int trapDefactsCount = 0;
        public int TrapDefactsCount
        {
            get { return trapDefactsCount; }
            set { trapDefactsCount = value; }
        }
        int pixDefectsCount = 0;
        public int PixDefectsCount
        {
            get { return pixDefectsCount; }
            set { pixDefectsCount = value; }
        }
        int clusterDefectCount = 0;
        public int ClusterDefectCount
        {
            get { return clusterDefectCount; }
            set { clusterDefectCount = value; }
        }
        int maxTrap = 0;
        public int MaxTrap
        {
            get { return maxTrap; }
            set { maxTrap = value; }
        }
        int aveTrap = 0;
        public int AveTrap
        {
            get { return aveTrap; }
            set { aveTrap = value; }
        }
        double maxDarkROI = 0;
        public double MaxDarkROI
        {
            get { return maxDarkROI; }
            set { maxDarkROI = value; }
        }
        int totalDefectCount = 0;
        public int TotalDefectCount
        {
            get { return totalDefectCount; }
            set { totalDefectCount = value; }
        }
        string enggUserName = string.Empty;
        public string EnggUserName
        {
            get { return enggUserName; }
            set { enggUserName = value; }
        }
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
        string usermode = string.Empty;
        int exposureCount = 0;
        public int ExposureCount
        {
            get { return exposureCount; }
            set { exposureCount = value; }
        }
        double[] darkCurrentMean;
        public double[] DarkCurrentMeanValue
        {
            get { return darkCurrentMean; }
            set { darkCurrentMean = value; }
        }
        string darkCurrentTestFailureReason = string.Empty;
        public string DarkCurrentTestFailureReason
        {
            get { return darkCurrentTestFailureReason; }
            set { darkCurrentTestFailureReason = value; }
        }
        public DarkCurrentPresenter(DarkCurrentTestForm darkCurrentTestForm, KronosTestAppMainForm kronostestappMainForm)
        {
            try
            {                
                this.darkCurrentTestForm = darkCurrentTestForm;
                this.kronostestappMainForm = kronostestappMainForm;
                darkCurrentTestForm.darkCurrentPresenter = this;
                kronostestappMainForm.darkCurrentPresenter = this;
                path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                commandProcessor = new CommandProcessor(cidInterface);
                darkCurrentTestResultsDataList = new List<DarkCurrentTestResults>();
                exposureDataModel = new ExposureDataModel();
                subarrayDataModel = new SubarrayDataModel();
                subarrayDataModelList = new List<SubarrayDataModel>();
                exposureDataList = new List<ExposureDataModel>();
                darkCurrentTestLimitsDataList = new List<DarkCurrentTestLimitsData>();
                DefectsTestLimitsDataList = new List<DefectsTestLimitsData>();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task GetDarkCurrentData(CIDInterface cidInterface)
        {
            try
            {
                this.cidInterface = cidInterface;
                kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                if (TestAppHelper.CheckDBExists())
                {
                    await PopulateDarkCurrentEntity();
                    DBExists = true;
                }
                else
                {
                    await ProcessDarkCurrentXMLFile();
                    log.Info("Dark Current test data populated from XML Repository.");
                    DBExists = false;               
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async void runDarkCurrentTest(CIDInterface cidInterface, CancellationToken ct, string userMode, int expCount = 1)
        {
            try
            {
                if (!ct.IsCancellationRequested)
                {
                    log.Info("Begin Dark Current test.");
                    this.exposureCount = expCount;
                    darkCurrentTestPassed = false;
                    this.cidInterface = cidInterface;
                    if (cidInterface != null)
                        cidInterface.ResetExposureId();
                    kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                    kronostestappMainForm.cidDataList.Clear();
                    kronostestappMainForm.cidIntegratedDataList.Clear();
                    this.usermode = userMode;                  
                    if (!ct.IsCancellationRequested)
                    {
                        await commandProcessor.ProcessExposureData(exposureDataList, ct);
                        //send exposure data to camera
                        await commandProcessor.SendExposure(ct, exposureCount, cidInterface);
                    }
                    log.Info("End Dark Current test.");
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }       
        public DarkCurrentTestResults ReturnDarkCurrentResults()
        {
            try
            {
                darkCurrentTestResultsData = new DarkCurrentTestResults();
                darkCurrentTestResultsData.UserExecuted = string.IsNullOrWhiteSpace(enggUserName) ? Environment.UserName.ToUpper() : enggUserName.ToUpper();
                darkCurrentTestResultsData.DarkCurrentMean = String.Join(",", darkCurrentMean.Select(p => p.ToString()).ToArray());
                darkCurrentTestResultsData.PRNUDefects = prnuDefectsCount;
                darkCurrentTestResultsData.TrapDefects = trapDefactsCount;
                darkCurrentTestResultsData.HotPixelDefects = pixDefectsCount;
                darkCurrentTestResultsData.ClusterDefects = clusterDefectCount;
                darkCurrentTestResultsData.TotalDefects = totalDefectCount;
                darkCurrentTestResultsData.MaxDarkFF = maxDarkFF;
                darkCurrentTestResultsData.AveDarkFF = aveDarkFF;
                darkCurrentTestResultsData.MaxTrap = maxTrap;
                darkCurrentTestResultsData.AveTrap = aveTrap ;
                darkCurrentTestResultsData.MaxDarkROI = maxDarkROI;
                darkCurrentTestResultsData.DeadPixels = deadPixels;
                darkCurrentTestResultsData.BadColumns = badColumns;
                darkCurrentTestResultsData.BadRows = badRows;
                darkCurrentTestResultsData.DefectsMean = defectsMean;
                darkCurrentTestResultsData.TestPassed = darkCurrentTestPassed;
                darkCurrentTestResultsData.TestResultImagePath = testResultImagePath;
                darkCurrentTestResultsData.DateExecuted = DateTime.Now;
                darkCurrentTestResultsDataList.Add(darkCurrentTestResultsData);
                return darkCurrentTestResultsData;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }
        public async Task PopulateDarkCurrentEntity()
        {
            try
            {
                kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                if (TestAppHelper.CheckDBExists())
                {
                    DBExists = true;
                    await Task.Run(() =>
                    {
                        using (var kcc = new KronosCamContext())
                        {
                                exposureDataList = kcc.exposureDataModel.Include("SubarrayDatas").Where(mv => mv.TestDataID.Equals("DDT")).ToList();
                                List<int> exposureIDList = (from exp in exposureDataList.Where(e => e.TestDataID.Equals("DDT")) select exp.ExposureID).ToList();
                                subarrayDataModelList = (from mvsu in kcc.subarrayDataModel orderby mvsu.ExposureID where exposureIDList.Contains(mvsu.ExposureID) select mvsu).ToList();
                                log.Info("Dark Current Test data populated from Database with Model & Subarray Records of ." + exposureDataList.Count().ToString() + subarrayDataModelList.Count.ToString());
                        }
                    });
                }
                else
                {
                    //get the data from XML files
                    await ProcessDarkCurrentXMLFile();
                    DBExists = false;
                    log.Info("Dark Current Test data populated from Database with Model & Subarray Records of ." + exposureDataList.Count().ToString() + subarrayDataModelList.Count.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }      
        public async Task ProcessDarkCurrentXMLFile()
        {
            try
            {
                await Task.Run(() =>
                {
                    log.Info("Begin Reading DarkCurrentData XML.");                  
                    commandProcessor.ProcessExposureXMLData("DarkCurrentData.xml");
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
        public async Task CalculateDarkCurrent(CancellationToken ct)
        {
            try
            {
                await Task.Run(() =>
                {
                    log.Info("Begin Calcluate Dark Current for Exposure Count " + exposureCount.ToString());
                    maxDarkROI = 0;
                    while (kronostestappMainForm.cidDataList.Count < 27)
                    {
                        if (ct.IsCancellationRequested)
                        {                           
                            kronostestappMainForm.cidDataList.Clear();
                            kronostestappMainForm.cidIntegratedDataList.Clear();
                            return;
                        }
                        if (!cidInterface.IsConnected())
                        {
                           // MessageBox.Show("Camera got disconnected, Trying to reconnect.....");
                            log.Error("Camera disconnected during Darkcurrent test " + kronostestappMainForm.cidDataList.Count.ToString());
                            //cidInterface.Disconnect();
                            //kronostestappMainForm.AutoConnectCamera();
                            //if (cidInterface.IsConnected())
                            //    MessageBox.Show("Camera reconnected successfully.");
                        }
                    }
                    CID821_Data cid821Data = new CID821_Data();
                    cid821Data = kronostestappMainForm.cidDataList[0];
                    // Calculate the size needed to hold the signal data from the Subarray
                    int length = (cid821Data.dr) * (cid821Data.dc);
                    double bias = 0;
                    double[] ROI1 = new double[length];
                    darkCurrentMean = new double[kronostestappMainForm.cidDataList.Count()];
                    for (int i = 0; i < kronostestappMainForm.cidDataList.Count(); i++)
                    {
                        ROI1 = TestAppHelper.convertVideoDataToDouble(kronostestappMainForm.cidDataList[i].videoDataList);
                        darkCurrentMean[i] = (TestAppHelper.calculateMeanDouble(ROI1) - bias);
                    }
                    int intervalInSec=exposureDataList[0].ExposureInterval / 1000;
                    maxDarkROI = (darkCurrentMean.Max() - darkCurrentMean.Min())/ intervalInSec;
                    if(maxDarkROI <= darkCurrentTestLimitsData.DarkCurrentUpperLimit && maxDarkROI >= darkCurrentTestLimitsData.DarkCurrentLowerLimit)
                        darkCurrentTestPassed = true;
                    else
                    {
                        darkCurrentTestFailureReason = "Max Dark ROI";
                        darkCurrentTestPassed = false;
                    }
                        
                    kronostestappMainForm.cidDataList.Clear();
                    kronostestappMainForm.cidIntegratedDataList.Clear();
                });
                log.Info("End Calcluate DarkCurrent for Exposure Count " + exposureCount.ToString());
            }
            catch (Exception ex)
            {
                darkCurrentTestPassed = false;
                kronostestappMainForm.cidDataList.Clear();
                kronostestappMainForm.cidIntegratedDataList.Clear();
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }     
        public async Task GetDarkCurrentLimits()
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
                                darkCurrentTestLimitsDataList = (from limits in kcc.darkCurrentTestLimitsData orderby limits.DefinedDate descending select limits).ToList();
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
