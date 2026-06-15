using CIDSoftwareApplication;
using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.View;
using LabJack.LabJackUD;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Thermo.Kronos.Instrument.Camera.Interface;
namespace KronosCameraTestApp.Presenter
{
    public class ShutterDrivePresenter
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CIDInterface cidInterface = null;
        CommandProcessor commandProcessor = null;    
        bool DBExists = false;
        public XDocument xmlDoc;
        XDocument xmlResultsDoc;
        String path = string.Empty;
        double[] shutterPlotResult;
        public double[] ShutterPlotResult
        {
            get { return shutterPlotResult; }
            set { shutterPlotResult = value; }
        }
        KronosTestAppMainForm kronostestappMainForm { get; set; }
        ShutterDriveTestForm shutterDriveTestForm { get; set; }
        ShutterDriveTestResults shutterDriveTestResults { get; set; }
        bool shutterDriveTestPassed = true;
        public bool ShutterDriveTestPassed
        {
            get { return shutterDriveTestPassed; }
            set { shutterDriveTestPassed = value; }
        }
        bool shutterDriveDBChanged = false;
        string enggUserName = string.Empty;
        public string EnggUserName
        {
            get { return enggUserName; }
            set { enggUserName = value; }
        }
        int exposureCount = 0;
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
        List<ShutterDriveTestResults> shutterDriveTestResultsList { get; set; }       
        string testResultImagePath = string.Empty;
        public string TestResultImagePath
        {
            get { return testResultImagePath; }
            set { testResultImagePath = value; }
        }
        bool testResultsSaved = true;
        string labJackSN = string.Empty;
        public string LabJackSN
        {
            get { return labJackSN; }
            set { labJackSN = value; }
        }
        public ShutterDrivePresenter(ShutterDriveTestForm shutterDriveTestForm, KronosTestAppMainForm kronostestappMainForm)
        {
            this.kronostestappMainForm = kronostestappMainForm;
            this.shutterDriveTestForm = shutterDriveTestForm;
            kronostestappMainForm.shutterDrivePresenter = this;
            shutterDriveTestForm.shutterDrivePresenter = this;
            exposureDataModel = new ExposureDataModel();
            exposureDataList = new List<ExposureDataModel>();
            path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            commandProcessor = new CommandProcessor(cidInterface);
            shutterDriveTestResultsList = new List<ShutterDriveTestResults>();           
        }
        public async Task runShutterDriveTest(CIDInterface cidInterface, CancellationToken ct, string userMode, int expCount = 1)
        {
            try
            {
                if (!ct.IsCancellationRequested)
                {                  
                    this.exposureCount = expCount;
                    log.Info("Begin Shutter Drive test.");
                    this.cidInterface = cidInterface;
                    if (cidInterface != null)
                        cidInterface.ResetExposureId();
                    TestAppHelper.currentRunningTest = "ShutterDrive";
                    kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                    kronostestappMainForm.cidDataList.Clear();
                    kronostestappMainForm.cidIntegratedDataList.Clear();                                     
                    if (!ct.IsCancellationRequested)
                    {
                        //process the mean variance model data and populate exposure structures                      
                        await commandProcessor.ProcessExposureData(exposureDataList, ct);
                        //send exposure data to camera
                        await commandProcessor.SendExposure(ct, exposureCount, cidInterface);
                    }
                    else
                    {
                        return;
                    }
                    log.Info("End Shutter Drive test.");
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }             
        public async Task GetShutterDriveData(CIDInterface cidInterface)
        {
            try
            {
                this.cidInterface = cidInterface;
                kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                if (TestAppHelper.CheckDBExists())
                {
                    await PopulateShutterDriveEntity();
                    DBExists = true;
                }
                else
                {
                    kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                    await ProcessShutterDriveXMLFile();
                    log.Info("Shutter Drive test data populated from XML Repository.");
                    DBExists = false;
                    //await ProcessScriptFile();                   
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task PopulateShutterDriveEntity()
        {
            try
            {
                if (TestAppHelper.CheckDBExists())
                {
                    DBExists = true;
                    await Task.Run(() =>
                    {
                        kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                        using (var kcc = new KronosCamContext())
                        {
                                exposureDataList = kcc.exposureDataModel.Where(mv => mv.TestDataID.Equals("SDT")).ToList();
                                log.Info("Shutter Drive Test data populated from Database with Model & Subarray Records of ." + exposureDataList.Count().ToString());
                        }
                    });
                }
                else
                {
                        DBExists = false;
                        await ProcessShutterDriveXMLFile();
                        log.Info("Shutter Drive Test data populated from Database with Model & Subarray Records of ." + exposureDataList.Count().ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task ProcessShutterDriveXMLFile()
        {
            try
            {
                await Task.Run(() =>
                {
                    log.Info("Begin Reading MeanVarianceData XML.");
                    commandProcessor.ProcessExposureXMLData("MeanVarianceData.xml");
                    exposureDataList = new List<ExposureDataModel>();
                    exposureDataList = commandProcessor.ExposureDataList;
                });
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public ShutterDriveTestResults ReturnShutterDriveResults()
        {
            try
            {
                shutterDriveTestResults = new ShutterDriveTestResults();
                shutterDriveTestResults.UserExecuted = string.IsNullOrWhiteSpace(enggUserName) ? Environment.UserName.ToUpper() : enggUserName.ToUpper();
                shutterDriveTestResults.TestResultImagePath = testResultImagePath;
                shutterDriveTestResults.TestPassed = shutterDriveTestPassed;
                shutterDriveTestResults.DateExecuted = DateTime.Now;
                shutterDriveTestResults.LabJackSN = labJackSN;
                if(shutterPlotResult != null)
                    shutterDriveTestResults.ShutterPlot = String.Join(",", shutterPlotResult.Select(p => p.ToString()).ToArray()); 
                shutterDriveTestResultsList.Add(shutterDriveTestResults);
                return shutterDriveTestResults;
            }
            catch (Exception ex)
            {
                testResultsSaved = false;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }                   
    }
}
