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
    public class InjectionEfficiencyPresenter
    {
        KronosTestAppMainForm kronostestappMainForm { get; set; }
        InjectionEfficiencyTestForm injectionEfficiencyTestForm { get; set; }
        InjectionEfficiencyTestResults injectionEfficiencyTestResults { get; set; }
        InjectionEfficiencyTestLimitsData injectionEfficiencyLimitsData { get; set; }
        public InjectionEfficiencyTestLimitsData InjectionEfficiencyLimitsData
        {
            get { return injectionEfficiencyLimitsData; }
            set { injectionEfficiencyLimitsData = value; }
        }
        List<InjectionEfficiencyTestLimitsData> injectionEfficiencyLimitsDataList { get; set; }
        public List<InjectionEfficiencyTestLimitsData> InjectionEfficiencyLimitsDataList
        {
            get { return injectionEfficiencyLimitsDataList; }
            set { injectionEfficiencyLimitsDataList = value; }
        }
        bool injectionEfficiencyTestPassed = true;
        public bool InjectionEfficiencyTestPassed
        {
            get { return injectionEfficiencyTestPassed; }
            set { injectionEfficiencyTestPassed = value; }
        }
        bool rowCrossTalkTestPassed = false;
        public bool RowCrossTalkTestPassed
        {
            get { return rowCrossTalkTestPassed; }
            set { rowCrossTalkTestPassed = value; }
        }
        bool colCrossTalkTestPassed = false;
        public bool ColCrossTalkTestPassed
        {
            get { return colCrossTalkTestPassed; }
            set { colCrossTalkTestPassed = value; }
        }
        string testResultImagePath = string.Empty;
        public string TestResultImagePath
        {
            get { return testResultImagePath; }
            set { testResultImagePath = value; }
        }
        string enggUserName = string.Empty;
        public string EnggUserName
        {
            get { return enggUserName; }
            set { enggUserName = value; }
        }
        public InjectionEfficiencyTestResults InjectionEfficiencyTestResults
        {
            get { return injectionEfficiencyTestResults; }
            set { injectionEfficiencyTestResults = value; }
        }
        List<InjectionEfficiencyTestResults> injectionEfficiencyTestResultsList { get; set; }
        public List<InjectionEfficiencyTestResults> InjectionEfficiencyTestResultsList
        {
            get { return injectionEfficiencyTestResultsList; }
            set { injectionEfficiencyTestResultsList = value; }
        }
        double[] exposures;
        double[] initialExposureResult;
        public double[] InitialExposureResult
        {
            get { return initialExposureResult; }
            set { initialExposureResult = value; }
        }
        string initialExposureResultString = string.Empty;
        public string InitialExposureResultString
        {
            get { return initialExposureResultString; }
            set { initialExposureResultString = value; }
        }
        string[] firstInjectionResultUI;
        public string[] FirstInjectionResultUI
        {
            get { return firstInjectionResultUI; }
            set { firstInjectionResultUI = value; }
        }
        string[] secondInjectionResultUI;
        public string[] SecondInjectionResultUI
        {
            get { return secondInjectionResultUI; }
            set { secondInjectionResultUI = value; }
        }
        string[] thirdInjectionResultUI;
        public string[] ThirdInjectionResultUI
        {
            get { return thirdInjectionResultUI; }
            set { thirdInjectionResultUI = value; }
        }
        string[] fourthInjectionResultUI;
        public string[] FourthInjectionResultUI
        {
            get { return fourthInjectionResultUI; }
            set { fourthInjectionResultUI = value; }
        }
        string[] lastInjectionResultUI;
        public string[] LastInjectionResultUI
        {
            get { return lastInjectionResultUI; }
            set { lastInjectionResultUI = value; }
        }
        string firstInjectionResultString = string.Empty;
        public string FirstInjectionResultString
        {
            get { return firstInjectionResultString; }
            set { firstInjectionResultString = value; }
        }
        double[] firstInjectionResult;
        public double[] FirstInjectionResult
        {
            get { return firstInjectionResult; }
            set { firstInjectionResult = value; }
        }
        double[] lastInjectionResult;
        public double[] LastInjectionResult
        {
            get { return lastInjectionResult; }
            set { lastInjectionResult = value; }
        }
        string lastInjectionResultString = string.Empty;
        public string LastInjectionResultString
        {
            get { return lastInjectionResultString; }
            set { lastInjectionResultString = value; }
        }
        double[] secondInjectionResult;
        public double[] SecondInjectionResult
        {
            get { return secondInjectionResult; }
            set { secondInjectionResult = value; }
        }
        string secondInjectionResultString = string.Empty;
        public string SecondInjectionResultString
        {
            get { return secondInjectionResultString; }
            set { secondInjectionResultString = value; }
        }
        double[] thirdInjectionResult;
        public double[] ThirdInjectionResult
        {
            get { return thirdInjectionResult; }
            set { thirdInjectionResult = value; }
        }
        string thirdInjectionResultString = string.Empty;
        public string ThirdInjectionResultString
        {
            get { return thirdInjectionResultString; }
            set { thirdInjectionResultString = value; }
        }
        double[] fourthInjectionResult;
        public double[] FourthInjectionResult
        {
            get { return fourthInjectionResult; }
            set { fourthInjectionResult = value; }
        }
        string fourthInjectionResultString = string.Empty;
        public string FourthInjectionResultString
        {
            get { return fourthInjectionResultString; }
            set { fourthInjectionResultString = value; }
        }
        CommandProcessor commandProcessor = null;
      
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CIDInterface cidInterface = null;
        String path = string.Empty;
        public XDocument xmlDoc;        
        public bool DBExists = false;
        const int NUMBER_OF_EXPOSURES = 5;
        const int NUMBER_OF_INJECTS = 6;        
        double[,] meanInj;       
       
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
        double[] exp0PlotData;
        public double[] EXP0PlotData
        {
            get { return exp0PlotData; }
            set { exp0PlotData = value; }
        }
        double[] exp1PlotData;
        public double[] EXP1PlotData
        {
            get { return exp1PlotData; }
            set { exp1PlotData = value; }
        }
        double[] exp2PlotData;
        public double[] EXP2PlotData
        {
            get { return exp2PlotData; }
            set { exp2PlotData = value; }
        }
        double[] exp3PlotData;
        public double[] EXP3PlotData
        {
            get { return exp3PlotData; }
            set { exp3PlotData = value; }
        }
        double[] exp4PlotData;
        public double[] EXP4PlotData
        {
            get { return exp4PlotData; }
            set { exp4PlotData = value; }
        }
        int exposureCount = 0;
        public int ExposureCount
        {
            get { return exposureCount; }
            set { exposureCount = value; }
        }
        bool testResultsSaved = true;
        string injectionEfficiencyTestFailureReason = string.Empty;
        public string InjectionEfficiencyTestFailureReason
        {
            get { return injectionEfficiencyTestFailureReason; }
            set { injectionEfficiencyTestFailureReason = value; }
        }
        int meanBox1 = 0;
        Dictionary<string, int> crossTalkResultData;

        public Dictionary<string, int> CrossTalkResultData
        {
            get { return crossTalkResultData; }
            set { crossTalkResultData = value; }
        }
        //string injEffCrosstalkTestFailureReason = string.Empty;
        //public string InjEffCrosstalkTestFailureReason
        //{
        //    get { return injEffCrosstalkTestFailureReason; }
        //    set { injEffCrosstalkTestFailureReason = value; }
        //}
        public InjectionEfficiencyPresenter(InjectionEfficiencyTestForm injectionEfficiencyTestForm, KronosTestAppMainForm kronostestappMainForm)
        {
            try
            {
                this.injectionEfficiencyTestForm = injectionEfficiencyTestForm;
                injectionEfficiencyTestForm.injectionEfficiencyPresenter = this;
                kronostestappMainForm.injectionEfficiencyPresenter = this;
                exposureDataModel = new ExposureDataModel();
                subarrayDataModel = new SubarrayDataModel();
                subarrayDataModelList = new List<SubarrayDataModel>();
                exposureDataList = new List<ExposureDataModel>();
                this.kronostestappMainForm = kronostestappMainForm;
                path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                injectionEfficiencyTestResultsList = new List<InjectionEfficiencyTestResults>();
                injectionEfficiencyLimitsDataList = new List<InjectionEfficiencyTestLimitsData>();
                commandProcessor = new CommandProcessor(cidInterface);                
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task GetInjectionEfficiencyData(CIDInterface cidInterface)
        {
            try
            {
                kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                this.cidInterface = cidInterface;
                if (TestAppHelper.CheckDBExists())
                {
                    await PopulateInjectionEfficiencyEntity();
                    DBExists = true;
                }
                else
                {
                    await ProcessInjectionPerformanceXMLFile();
                    log.Info("Injection Performance test data populated from XML Repository.");
                    DBExists = false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task GetCrossTalkData(CIDInterface cidInterface)
        {
            try
            {
                kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                this.cidInterface = cidInterface;
                if (TestAppHelper.CheckDBExists())
                {
                    await PopulateCrossTalkEntity();
                    DBExists = true;
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
        public async Task runInjectionEfficiencyTest(CIDInterface cidInterface, CancellationToken ct, string userMode, int exposureCount = 30)
        {
            try
            {
                if (!ct.IsCancellationRequested)
                {
                    kronostestappMainForm.cidDataList.Clear();
                    kronostestappMainForm.cidIntegratedDataList.Clear();
                    this.exposureCount = exposureCount;
                    log.Info("Begin Injection Performance test.");
                    this.cidInterface = cidInterface;
                    if (cidInterface != null)
                        cidInterface.ResetExposureId();
                    injectionEfficiencyTestPassed = false;
                    kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                    kronostestappMainForm.cidDataList.Clear();
                    kronostestappMainForm.cidIntegratedDataList.Clear();                   
                    if (!ct.IsCancellationRequested)
                    {
                        //process the  model data and populate exposure structures                      
                        await commandProcessor.ProcessExposureData(exposureDataList, ct);
                        //send exposure data to camera
                        await commandProcessor.SendExposure(ct, exposureCount, cidInterface);
                    }
                    log.Info("End Injection Performance test.");
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }           
        public async Task runCrossTalkTest(CIDInterface cidInterface, CancellationToken ct, int exposureCount = 3)
        {
            try
            {
                if (!ct.IsCancellationRequested)
                {
                    kronostestappMainForm.cidDataList.Clear();
                    kronostestappMainForm.cidIntegratedDataList.Clear();
                    this.exposureCount = exposureCount;
                    log.Info("Begin Cross Talk test.");
                    this.cidInterface = cidInterface;
                    if (cidInterface != null)
                        cidInterface.ResetExposureId();
                    colCrossTalkTestPassed = false;
                    rowCrossTalkTestPassed = false;
                    kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                    kronostestappMainForm.cidDataList.Clear();
                    kronostestappMainForm.cidIntegratedDataList.Clear();
                    if (!ct.IsCancellationRequested)
                    {
                        //process the  model data and populate exposure structures                      
                        await commandProcessor.ProcessExposureData(exposureDataList, ct);
                        //send exposure data to camera
                        await commandProcessor.SendExposure(ct, exposureCount, cidInterface);
                    }
                    log.Info("End Cross Talk test.");
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public InjectionEfficiencyTestResults ReturnInjectionEfficiencyResults()
        {
            try
            {
                injectionEfficiencyTestResults = new InjectionEfficiencyTestResults();
                injectionEfficiencyTestResults.UserExecuted = string.IsNullOrWhiteSpace(enggUserName) ? Environment.UserName.ToUpper() : enggUserName.ToUpper();
                injectionEfficiencyTestResults.InitialExposure = initialExposureResultString;
                injectionEfficiencyTestResults.FirstInjection = firstInjectionResultString;
                injectionEfficiencyTestResults.SecondInjection = secondInjectionResultString;
                injectionEfficiencyTestResults.ThirdInjection = thirdInjectionResultString;
                injectionEfficiencyTestResults.FourthInjection = fourthInjectionResultString;
                injectionEfficiencyTestResults.LastInjection = lastInjectionResultString;
                injectionEfficiencyTestResults.Exp0PlotData = String.Join(",", exp0PlotData.Select(p => p.ToString()).ToArray());
                injectionEfficiencyTestResults.Exp1PlotData = String.Join(",", exp1PlotData.Select(p => p.ToString()).ToArray());
                injectionEfficiencyTestResults.Exp2PlotData = String.Join(",", exp2PlotData.Select(p => p.ToString()).ToArray());
                injectionEfficiencyTestResults.Exp3PlotData = String.Join(",", exp3PlotData.Select(p => p.ToString()).ToArray());
                injectionEfficiencyTestResults.Exp4PlotData = String.Join(",", exp4PlotData.Select(p => p.ToString()).ToArray());
                injectionEfficiencyTestResults.MeanBox1= crossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox1")).Value;
                injectionEfficiencyTestResults.MeanBox2 = crossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox2")).Value;
                injectionEfficiencyTestResults.MeanBox3 = crossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox3")).Value;
                injectionEfficiencyTestResults.MeanBox4 = crossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox4")).Value;
                injectionEfficiencyTestResults.MeanBox5 = crossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox5")).Value;
                injectionEfficiencyTestResults.MeanBox6 = crossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox6")).Value;
                injectionEfficiencyTestResults.MeanBox7 = crossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox7")).Value;
                injectionEfficiencyTestResults.MeanBox8 = crossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox8")).Value;
                injectionEfficiencyTestResults.MeanBox9 = crossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox9")).Value;

                injectionEfficiencyTestResults.Reference = crossTalkResultData.FirstOrDefault(c => c.Key.Equals("Reference")).Value;
                injectionEfficiencyTestResults.RowCrossTalk = crossTalkResultData.FirstOrDefault(c => c.Key.Equals("RowCrosstalk")).Value;
                injectionEfficiencyTestResults.ColCrossTalk = crossTalkResultData.FirstOrDefault(c => c.Key.Equals("ColumnCrosstalk")).Value;
                injectionEfficiencyTestResults.RelativeSensitivity = crossTalkResultData.FirstOrDefault(c => c.Key.Equals("RelativeSensitivity")).Value;
                injectionEfficiencyTestResults.RowXTalkPassed = rowCrossTalkTestPassed;
                injectionEfficiencyTestResults.ColXTalkPassed = colCrossTalkTestPassed;
                injectionEfficiencyTestResults.TestPassed = injectionEfficiencyTestPassed;
                injectionEfficiencyTestResults.TestResultImagePath = testResultImagePath;
                injectionEfficiencyTestResults.DateExecuted = DateTime.Now;
                injectionEfficiencyTestResultsList.Add(injectionEfficiencyTestResults);
                return injectionEfficiencyTestResults;
            }
            catch (Exception ex)
            {
                testResultsSaved = false;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }       
        public async Task PopulateInjectionEfficiencyEntity()
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
                                exposureDataList = kcc.exposureDataModel.Include("SubarrayDatas").Where(mv => mv.TestDataID.Equals("IET")).ToList();
                                List<int> exposureIDList = (from exp in exposureDataList.Where(e => e.TestDataID.Equals("IET")) select exp.ExposureID).ToList();
                                subarrayDataModelList = (from mvsu in kcc.subarrayDataModel orderby mvsu.ExposureID where exposureIDList.Contains(mvsu.ExposureID) select mvsu).ToList();
                                log.Info("Injection Performance Test data populated from Database with Model & Subarray Records of ." + exposureDataList.Count().ToString() + subarrayDataModelList.Count.ToString());
                        }
                    });
                }
                else
                {
                        //get the data from script files
                        await ProcessInjectionPerformanceXMLFile();
                        DBExists = false;
                        log.Info("Injection Performance Test data populated from Database with Model & Subarray Records of ." + exposureDataList.Count().ToString() + subarrayDataModelList.Count.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task PopulateCrossTalkEntity()
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
                            exposureDataList = kcc.exposureDataModel.Include("SubarrayDatas").Where(mv => mv.TestDataID.Equals("CTT")).ToList();
                            List<int> exposureIDList = (from exp in exposureDataList.Where(e => e.TestDataID.Equals("CTT")) select exp.ExposureID).ToList();
                            subarrayDataModelList = (from mvsu in kcc.subarrayDataModel orderby mvsu.ExposureID where exposureIDList.Contains(mvsu.ExposureID) select mvsu).ToList();
                            log.Info("Cross Talk Test data populated from Database with Model & Subarray Records of ." + exposureDataList.Count().ToString() + subarrayDataModelList.Count.ToString());
                        }
                    });
                }
                else
                {
                    //get the data from script files
                    //await ProcessInjectionPerformanceXMLFile();
                    DBExists = false;
                    log.Info(" Cross Talk Test data populated from Database with Model & Subarray Records of ." + exposureDataList.Count().ToString() + subarrayDataModelList.Count.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task CalculateInjectionEfficiency(CancellationToken ct)
        {
            try
            {
                await Task.Run(() =>
                {
                    while (kronostestappMainForm.cidDataList.Count < NUMBER_OF_INJECTS * NUMBER_OF_EXPOSURES)
                    {
                        if (ct.IsCancellationRequested)
                        {
                            kronostestappMainForm.cidDataList.Clear();
                            kronostestappMainForm.cidIntegratedDataList.Clear();
                            return;
                        }
                        if (!cidInterface.IsConnected())
                        {
                            log.Error("Camera disconnected during Injection Eff test " + kronostestappMainForm.cidDataList.Count.ToString());                          
                        }
                    }
                    while (kronostestappMainForm.cidIntegratedDataList.Count < NUMBER_OF_INJECTS * NUMBER_OF_EXPOSURES)
                    {
                        if (ct.IsCancellationRequested)
                        {
                            kronostestappMainForm.cidDataList.Clear();
                            kronostestappMainForm.cidIntegratedDataList.Clear();
                            return;
                        }
                        if (!cidInterface.IsConnected())
                        {
                            log.Error("Camera disconnected during Injection Eff test " + kronostestappMainForm.cidDataList.Count.ToString());
                        }
                    }
                    CID821_Data cid821Data = new CID821_Data();
                    cid821Data = kronostestappMainForm.cidDataList[4];
                    int length = (cid821Data.dr) * (cid821Data.dc);                    
                    double[] ROI = new double[length];
                    int videoDataListIndex = 0;
                    exposures = new double[NUMBER_OF_EXPOSURES];
                    meanInj = new double[NUMBER_OF_EXPOSURES, NUMBER_OF_INJECTS];                  
                    exp0PlotData = new double[NUMBER_OF_INJECTS];
                    exp1PlotData = new double[NUMBER_OF_INJECTS];
                    exp2PlotData = new double[NUMBER_OF_INJECTS];
                    exp3PlotData = new double[NUMBER_OF_INJECTS];
                    exp4PlotData = new double[NUMBER_OF_INJECTS];
                    for (int j = 0; j < NUMBER_OF_EXPOSURES; j++)
                    {
                        for (int i = 0; i < NUMBER_OF_INJECTS; i++)
                        {
                            ROI = TestAppHelper.convertVideoDataToDouble(kronostestappMainForm.cidDataList[videoDataListIndex].videoDataList);
                            meanInj[j, i] = (TestAppHelper.calculateMeanDouble(ROI));
                            videoDataListIndex++;
                        }
                        exposures[j] = j;
                    }
                    for (int i = 0; i < NUMBER_OF_INJECTS; i++)
                    {
                        exp0PlotData[i] = meanInj[0, i];
                        exp1PlotData[i] = meanInj[1, i];
                        exp2PlotData[i] = meanInj[2, i];
                        exp3PlotData[i] = meanInj[3, i];
                        exp4PlotData[i] = meanInj[4, i];
                    }
                    initialExposureResult = new double[] { meanInj[0, 0], meanInj[1, 0], meanInj[2, 0], meanInj[3, 0], meanInj[4, 0] };
                    initialExposureResultString = String.Join(",", initialExposureResult.Select(p => p.ToString("0")).ToArray());
                    // Find smallest signal and subtract offset                           
                    double[] expoMinSignal = new double[NUMBER_OF_EXPOSURES];
                    bool firstInjPassed = false;
                    bool lastInjPassed = false;
                    for (int exp = 0; exp < NUMBER_OF_EXPOSURES; exp++)
                    {
                        expoMinSignal[exp] = 100000;
                        for (int inj = 1; inj < NUMBER_OF_INJECTS; inj++)
                        {
                            if (meanInj[exp, inj] < expoMinSignal[exp])
                            {
                                expoMinSignal[exp] = meanInj[exp, inj];
                            }
                        }
                    }
                    for (int exp = 0; exp < NUMBER_OF_EXPOSURES; exp++)
                    {
                        for (int inj = 0; inj < NUMBER_OF_INJECTS; inj++)
                        {
                            meanInj[exp, inj] = meanInj[exp, inj] - expoMinSignal[exp];
                        }
                    }
                    firstInjectionResultUI = new string[] { meanInj[0, 1].ToString("f2") + "     " + (meanInj[0, 1] / meanInj[0, 0]).ToString("P2"),
                                                          meanInj[1, 1].ToString("f2") + "     " + (meanInj[1, 1] / meanInj[1, 0]).ToString("P2"),
                                                          meanInj[2, 1].ToString("f2") + "     " + (meanInj[2, 1] / meanInj[2, 0]).ToString("P2"),
                                                          meanInj[3, 1].ToString("f2") + "     " + (meanInj[3, 1] / meanInj[3, 0]).ToString("P2"),
                                                          meanInj[4, 1].ToString("f2") + "     " + (meanInj[4, 1] / meanInj[4, 0]).ToString("P2")};
                    secondInjectionResultUI = new string[] { meanInj[0, 2].ToString("f2") + "     " + (meanInj[0, 2] / meanInj[0, 0]).ToString("P2"),
                                                          meanInj[1, 2].ToString("f2") + "     " + (meanInj[1, 2] / meanInj[1, 0]).ToString("P2"),
                                                          meanInj[2, 2].ToString("f2") + "     " + (meanInj[2, 2] / meanInj[2, 0]).ToString("P2"),
                                                          meanInj[3, 2].ToString("f2") + "     " + (meanInj[3, 2] / meanInj[3, 0]).ToString("P2"),
                                                          meanInj[4, 2].ToString("f2") + "     " + (meanInj[4, 2] / meanInj[4, 0]).ToString("P2")};
                    thirdInjectionResultUI = new string[] { meanInj[0, 3].ToString("f2") + "     " + (meanInj[0, 3] / meanInj[0, 0]).ToString("P2"),
                                                          meanInj[1, 3].ToString("f2") + "     " + (meanInj[1, 3] / meanInj[1, 0]).ToString("P2"),
                                                          meanInj[2, 3].ToString("f2") + "     " + (meanInj[2, 3] / meanInj[2, 0]).ToString("P2"),
                                                          meanInj[3, 3].ToString("f2") + "     " + (meanInj[3, 3] / meanInj[3, 0]).ToString("P2"),
                                                          meanInj[4, 3].ToString("f2") + "     " + (meanInj[4, 3] / meanInj[4, 0]).ToString("P2")};
                    fourthInjectionResultUI = new string[] { meanInj[0, 4].ToString("f2") + "     " + (meanInj[0, 4] / meanInj[0, 0]).ToString("P2"),
                                                          meanInj[1, 4].ToString("f2") + "     " + (meanInj[1, 4] / meanInj[1, 0]).ToString("P2"),
                                                          meanInj[2, 4].ToString("f2") + "     " + (meanInj[2, 4] / meanInj[2, 0]).ToString("P2"),
                                                          meanInj[3, 4].ToString("f2") + "     " + (meanInj[3, 4] / meanInj[3, 0]).ToString("P2"),
                                                          meanInj[4, 4].ToString("f2") + "     " + (meanInj[4, 4] / meanInj[4, 0]).ToString("P2")};
                    lastInjectionResultUI = new string[] { meanInj[0, 5].ToString("f2") + "     " + (meanInj[0, 5] / meanInj[0, 0]).ToString("P2"),
                                                          meanInj[1, 5].ToString("f2") + "     " + (meanInj[1, 5] / meanInj[1, 0]).ToString("P2"),
                                                          meanInj[2, 5].ToString("f2") + "     " + (meanInj[2, 5] / meanInj[2, 0]).ToString("P2"),
                                                          meanInj[3, 5].ToString("f2") + "     " + (meanInj[3, 5] / meanInj[3, 0]).ToString("P2"),
                                                          meanInj[4, 5].ToString("f2") + "     " + (meanInj[4, 5] / meanInj[4, 0]).ToString("P2")};
                    firstInjectionResult = new double[] { (meanInj[0, 1] / meanInj[0, 0])*100,
                                                          (meanInj[1, 1] / meanInj[1, 0])*100,
                                                          (meanInj[2, 1] / meanInj[2, 0])*100,
                                                          (meanInj[3, 1] / meanInj[3, 0])*100,
                                                          (meanInj[4, 1] / meanInj[4, 0])*100};
                    secondInjectionResult = new double[] { (meanInj[0, 2] / meanInj[0, 0])*100,
                                                          (meanInj[1, 2] / meanInj[1, 0])*100,
                                                          (meanInj[2, 2] / meanInj[2, 0])*100,
                                                          (meanInj[3, 2] / meanInj[3, 0])*100,
                                                          (meanInj[4, 2] / meanInj[4, 0])*100};
                    thirdInjectionResult = new double[] { (meanInj[0, 3] / meanInj[0, 0])*100,
                                                          (meanInj[1, 3] / meanInj[1, 0])*100,
                                                          (meanInj[2, 3] / meanInj[2, 0])*100,
                                                          (meanInj[3, 3] / meanInj[3, 0])*100,
                                                          (meanInj[4, 3] / meanInj[4, 0])*100};
                    fourthInjectionResult = new double[] { (meanInj[0, 4] / meanInj[0, 0])*100,
                                                          (meanInj[1, 4] / meanInj[1, 0])*100,
                                                          (meanInj[2, 4] / meanInj[2, 0])*100,
                                                          (meanInj[3, 4] / meanInj[3, 0])*100,
                                                          (meanInj[4, 4] / meanInj[4, 0])*100};
                    lastInjectionResult = new double[] { (meanInj[0, 5] / meanInj[0, 0])*100,
                                                          (meanInj[1, 5] / meanInj[1, 0])*100,
                                                          (meanInj[2, 5] / meanInj[2, 0])*100,
                                                          (meanInj[3, 5] / meanInj[3, 0])*100,
                                                          (meanInj[4, 5] / meanInj[4, 0])*100};
                    firstInjectionResultString = String.Join(",", firstInjectionResult.Select(p => p.ToString("0.00")).ToArray());
                    secondInjectionResultString = String.Join(",", secondInjectionResult.Select(p => p.ToString("0.00")).ToArray());
                    thirdInjectionResultString = String.Join(",", thirdInjectionResult.Select(p => p.ToString("0.00")).ToArray());
                    fourthInjectionResultString = String.Join(",", fourthInjectionResult.Select(p => p.ToString("0.00")).ToArray());
                    lastInjectionResultString = String.Join(",", lastInjectionResult.Select(p => p.ToString("0.00")).ToArray());
                    foreach (double d in firstInjectionResult)
                    {
                        if (d > injectionEfficiencyLimitsData.InjectionEffLimit)
                        {
                            injectionEfficiencyTestPassed = false;
                            firstInjPassed = false;
                            break;
                        }
                        else
                        {
                            injectionEfficiencyTestPassed = true;
                            firstInjPassed = true;
                        }
                    }
                   
                    if (injectionEfficiencyTestPassed)
                    {
                        foreach (double d in lastInjectionResult)
                        {
                            if (d > injectionEfficiencyLimitsData.InjectionEffLimit)
                            {
                                injectionEfficiencyTestPassed = false;
                                lastInjPassed = false;
                                break;
                            }
                            else
                            {
                                injectionEfficiencyTestPassed = true;
                                lastInjPassed = true;
                            }
                        }
                    }
                    if(!firstInjPassed)
                    {
                        injectionEfficiencyTestFailureReason = "First Injection";
                    }
                    if (!lastInjPassed)
                    {
                        injectionEfficiencyTestFailureReason = "Last Injection";
                    }
                    if(!firstInjPassed && !lastInjPassed)
                    {                        
                        injectionEfficiencyTestFailureReason = "First & Last Injection";
                    }
                   
                });
                log.Info("End Calcluate Injection Performance for Exposure Count " + exposureCount.ToString());
                kronostestappMainForm.cidDataList.Clear();
                kronostestappMainForm.cidIntegratedDataList.Clear();
            }
            catch (Exception ex)
            {
                injectionEfficiencyTestPassed = false;
                kronostestappMainForm.cidDataList.Clear();
                kronostestappMainForm.cidIntegratedDataList.Clear();
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task CalculateCrossTalk(CancellationToken ct)
        {
            try
            {
                await Task.Run(() =>
                {
                    const int NUMBER_OF_FRAMES = 3;
                    crossTalkResultData = new Dictionary<string, int>();
                    rowCrossTalkTestPassed = false;
                    colCrossTalkTestPassed = false;
                    while (kronostestappMainForm.cidIntegratedDataList.Count < NUMBER_OF_FRAMES)
                    {
                        if (ct.IsCancellationRequested)
                        {
                            kronostestappMainForm.cidDataList.Clear();
                            kronostestappMainForm.cidIntegratedDataList.Clear();
                            return;
                        }
                        if (!cidInterface.IsConnected())
                        {
                            log.Error("Camera disconnected during Cross Talk test " + kronostestappMainForm.cidDataList.Count.ToString());                          
                        }
                    }
                    Task.Delay(5000);
                    var count = 0;

                    var meanBox1 = 0; var meanBox4 = 0; var meanBox7 = 0;
                    var meanBox2 = 0; var meanBox5 = 0; var meanBox8 = 0;
                    var meanBox3 = 0; var meanBox6 = 0; var meanBox9 = 0;

                    var total1 = 0; var total4 = 0; var total7 = 0;
                    var total2 = 0; var total5 = 0; var total8 = 0;
                    var total3 = 0; var total6 = 0; var total9 = 0;

                    for (int y = 10; y < 100; y++)
                    {
                        for (int x = 10; x < 100; x++)
                        {

                            total1 = kronostestappMainForm.cidIntegratedDataList[2].videoIntegratedDataList[x, y] + total1;
                            total2 = kronostestappMainForm.cidIntegratedDataList[2].videoIntegratedDataList[x, y + 110] + total2;
                            total3 = kronostestappMainForm.cidIntegratedDataList[2].videoIntegratedDataList[x, y + 210] + total3;

                            total4 = kronostestappMainForm.cidIntegratedDataList[2].videoIntegratedDataList[x + 110, y] + total4;
                            total5 = kronostestappMainForm.cidIntegratedDataList[2].videoIntegratedDataList[x + 110, y + 110] + total5;
                            total6 = kronostestappMainForm.cidIntegratedDataList[2].videoIntegratedDataList[x + 110, y + 210] + total6;

                            total7 = kronostestappMainForm.cidIntegratedDataList[2].videoIntegratedDataList[x + 210, y] + total7;
                            total8 = kronostestappMainForm.cidIntegratedDataList[2].videoIntegratedDataList[x + 210, y + 110] + total8;
                            total9 = kronostestappMainForm.cidIntegratedDataList[2].videoIntegratedDataList[x + 210, y + 210] + total9;
                            count++;
                        }
                    }

                    meanBox1 = total1 / count;
                    meanBox2 = total2 / count;
                    meanBox3 = total3 / count;

                    meanBox4 = total4 / count;
                    meanBox5 = total5 / count;
                    meanBox6 = total6 / count;

                    meanBox7 = total7 / count;
                    meanBox8 = total8 / count;
                    meanBox9 = total9 / count;

                    var Reference = (meanBox3 + meanBox9) / 2;
                    var RowCrosstalk = ((meanBox2 + meanBox8) / 2) - Reference;
                    var ColumnCrosstalk = ((meanBox4 + meanBox6) / 2) - Reference;

                    var FiveAboveMean = Reference * injectionEfficiencyLimitsData.CrossTalkThresholdLimit;

                   
                    if (Math.Abs(RowCrosstalk) > FiveAboveMean)
                    {
                        rowCrossTalkTestPassed = false;
                        //injectionEfficiencyTestFailureReason += Environment.NewLine;
                        injectionEfficiencyTestFailureReason += " - Row Crosstalk ";
                        
                    }
                       
                    else
                    {
                        rowCrossTalkTestPassed = true;
                    }
                   
                    if (Math.Abs(ColumnCrosstalk) > FiveAboveMean)
                    {
                        colCrossTalkTestPassed = false;
                        //injectionEfficiencyTestFailureReason += Environment.NewLine;
                        injectionEfficiencyTestFailureReason += " - Column Crosstalk";
                    }
                       
                    else
                    {
                        colCrossTalkTestPassed = true;
                    }
                        

                    var RelativeSensitivity = meanBox7 - meanBox1;

                    crossTalkResultData.Add("meanBox1", meanBox1);
                    crossTalkResultData.Add("meanBox2", meanBox2);
                    crossTalkResultData.Add("meanBox3", meanBox3);
                    crossTalkResultData.Add("meanBox4", meanBox4);
                    crossTalkResultData.Add("meanBox5", meanBox5);
                    crossTalkResultData.Add("meanBox6", meanBox6);
                    crossTalkResultData.Add("meanBox7", meanBox7);
                    crossTalkResultData.Add("meanBox8", meanBox8);
                    crossTalkResultData.Add("meanBox9", meanBox9);

                    crossTalkResultData.Add("Reference", Reference);
                    crossTalkResultData.Add("RowCrosstalk", RowCrosstalk);
                    crossTalkResultData.Add("ColumnCrosstalk", ColumnCrosstalk);
                    crossTalkResultData.Add("RelativeSensitivity", RelativeSensitivity);                  

                });
                log.Info("End Calcluate Cross Talk for Exposure Count " + exposureCount.ToString());
                kronostestappMainForm.cidDataList.Clear();
                kronostestappMainForm.cidIntegratedDataList.Clear();
            }
            catch (Exception ex)
            {
                injectionEfficiencyTestPassed = false;
                rowCrossTalkTestPassed = false;
                colCrossTalkTestPassed = false;
                kronostestappMainForm.cidDataList.Clear();
                kronostestappMainForm.cidIntegratedDataList.Clear();
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task ProcessInjectionPerformanceXMLFile()
        {
            try
            {
                await Task.Run(() =>
                {
                    log.Info("Begin Reading Injection Performance Data XML.");                  
                    commandProcessor.ProcessExposureXMLData("InjectionPerformanceData.xml");
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
                                injectionEfficiencyLimitsDataList = (from limits in kcc.injectionEfficiencyTestLimitsData orderby limits.DefinedDate descending select limits).ToList();
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
