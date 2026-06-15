using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Thermo.Kronos.Instrument.Camera.Interface;
using System.Data.Entity.Migrations;
using System.Configuration;
using NationalInstruments.Analysis.Math;
using CIDSoftwareApplication;
namespace KronosCameraTestApp.Presenter
{
    public class MeanVariancePresenter
    {
        MeanVarianceTestForm meanVarianceTestForm;
        KronosTestAppMainForm kronostestappMainForm;
        CommandProcessor commandProcessor = null;      
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CIDInterface cidInterface = null;
        string correctedMeanData = string.Empty;
        //KronosCamContext kronosCamContext;
        int meanVarianceRepeats;
        public int MeanVarianceRepeats
        {
            get { return meanVarianceRepeats; }
            set { meanVarianceRepeats = value; }
        }
        double[] correctedMeanNew;
        public double[] CorrectedMeanNew
        {
            get { return correctedMeanNew; }
            set { correctedMeanNew = value; }
        }
        double[] correctedVarianceNew;
        public double[] CorrectedVarianceNew
        {
            get { return correctedVarianceNew; }
            set { correctedVarianceNew = value; }
        }
        double[,] NICorrectedMean;
        double[,] NICorrectedVariance;
        double gain;
        double[] linearCurveFit;
        int errorCode = 0;
        public XDocument xmlDoc;
        public XDocument xmlResultsDoc;
        bool meanVarianceTestPassed = false;
        public bool MeanVarianceTestPassed
        {
            get { return meanVarianceTestPassed; }
            set { meanVarianceTestPassed = value; }
        }
        MeanVarianceTestLimitsData meanVarianceTestLimitsData { get; set; }
        public MeanVarianceTestLimitsData MeanVarianceTestLimitsData
        {
            get { return meanVarianceTestLimitsData; }
            set { meanVarianceTestLimitsData = value; }
        }
        List<MeanVarianceTestLimitsData> meanVarianceTestLimitsDataList { get; set; }
        public List<MeanVarianceTestLimitsData> MeanVarianceTestLimitsDataList
        {
            get { return meanVarianceTestLimitsDataList; }
            set { meanVarianceTestLimitsDataList = value; }
        }
        MeanVarianceTestResultsData meanVarianceTestResultData { get; set; }
        List<MeanVarianceTestResultsData> meanVarianceTestResultDataList { get; set; }
        public MeanVarianceTestResultsData MeanVarianceTestResultsData
        {
            get { return meanVarianceTestResultData; }
            set { meanVarianceTestResultData = value; }
        }
        CancellationToken cancellationToken;
        public List<MeanVarianceTestResultsData> MeanVarianceTestResultsDataList
        {
            get { return meanVarianceTestResultDataList; }
            set { meanVarianceTestResultDataList = value; }
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
        public int ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }
        public double[,] NICorrectedMeanValue
        {
            get { return NICorrectedMean; }
            set { NICorrectedMean = value; }
        }
        public double[,] NICorrectedVarianceValue
        {
            get { return NICorrectedVariance; }
            set { NICorrectedVariance = value; }
        }
        public double[] LinearCurveFit
        {
            get { return linearCurveFit; }
            set { linearCurveFit = value; }
        }
        public double Gain
        {
            get { return gain; }
            set { gain = value; }
        }
        bool mvDBChanged = false;
        public bool MVDBChanged
        {
            get { return mvDBChanged; }
            set { mvDBChanged = value; }
        }
        int exposureCount = 0;
        public int ExposureCount
        {
            get { return exposureCount; }
            set { exposureCount = value; }
        }
        String path = string.Empty;
        public bool DBExists = false;
        bool testResultsSaved = true;
        public bool TestResultsSaved
        {
            get { return testResultsSaved; }
            set { testResultsSaved = value; }
        }
        double conversionFactorNominalResult;
        public double ConversionFactorNominalResult
        {
            get { return conversionFactorNominalResult; }
            set { conversionFactorNominalResult = value; }
        }
        string enggUserName = string.Empty;
        public string EnggUserName
        {
            get { return enggUserName; }
            set { enggUserName = value; }
        }
        string testResultImagePath = string.Empty;
        public string TestResultImagePath
        {
            get { return testResultImagePath; }
            set { testResultImagePath = value; }
        }
        string meanVarianceTestFailureReason = string.Empty;
        public string MeanVarianceTestFailureReason
        {
            get { return meanVarianceTestFailureReason; }
            set { meanVarianceTestFailureReason = value; }
        }
        public MeanVariancePresenter()
        {
            exposureDataModel = new ExposureDataModel();
            subarrayDataModel = new SubarrayDataModel();
            subarrayDataModelList = new List<SubarrayDataModel>();
            exposureDataList = new List<ExposureDataModel>();
            path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            commandProcessor = new CommandProcessor(cidInterface);
            meanVarianceTestResultDataList = new List<MeanVarianceTestResultsData>();
            meanVarianceTestLimitsDataList = new List<MeanVarianceTestLimitsData>();
        }
        public MeanVariancePresenter(MeanVarianceTestForm meanVarianceTestForm, KronosTestAppMainForm kronostestappMainForm)
        {
            try
            {
                this.meanVarianceTestForm = meanVarianceTestForm;
                this.kronostestappMainForm = kronostestappMainForm;
                meanVarianceTestForm.meanVariancePresenter = this;
                kronostestappMainForm.meanVariancePresenter = this;
                exposureDataModel = new ExposureDataModel();
                subarrayDataModel = new SubarrayDataModel();
                subarrayDataModelList = new List<SubarrayDataModel>();
                exposureDataList = new List<ExposureDataModel>();
                path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                commandProcessor = new CommandProcessor(cidInterface);
                meanVarianceTestResultDataList = new List<MeanVarianceTestResultsData>();
                meanVarianceTestLimitsDataList = new List<MeanVarianceTestLimitsData>();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task GetMVData(CIDInterface cidInterface)
        {
            try
            {
                this.cidInterface = cidInterface;
                kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                if (TestAppHelper.CheckDBExists())
                {
                    await PopulateMVEntity();
                    DBExists = true;
                }
                else
                {
                    kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();                   
                    await ProcessMVXMLFile();
                    log.Info("Mean Variance test data populated from XML Repository.");
                    DBExists = false;                  
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task runMeanVarianceTest(CIDInterface cidInterface, CancellationToken ct, string UserMode, int expCount = 20)
        {
            try
            {
                if (!ct.IsCancellationRequested)
                {
                    kronostestappMainForm.cidDataList.Clear();
                    kronostestappMainForm.cidIntegratedDataList.Clear();
                    this.exposureCount = expCount;
                    meanVarianceTestPassed = false;
                    log.Info("Begin Mean Variance test.");
                    this.cidInterface = cidInterface;
                    if (cidInterface != null)
                        cidInterface.ResetExposureId();
                    TestAppHelper.currentRunningTest ="MeanVariance";
                    kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                    kronostestappMainForm.cidDataList.Clear();
                    kronostestappMainForm.cidIntegratedDataList.Clear();
                    cancellationToken = ct;
                    //if (UserMode.Equals("Admin"))
                    //{
                    //    if (TestAppHelper.CheckDBExists())
                    //    {
                    //        DBExists = true;
                    //        //get the Mean variance exposure data from DB
                    //        await PopulateMVEntity();
                    //    }
                    //    else
                    //    {
                    //        DBExists = false;
                    //        kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                    //        //await ProcessScriptFile();
                    //        await ProcessMVXMLFile();
                    //    }
                    //}
                    if (!ct.IsCancellationRequested)
                    {
                        //process the mean variance model data and populate exposure structures                      
                        await commandProcessor.ProcessExposureData(exposureDataList, ct);
                    }
                    else
                    {
                        return;
                    }
                    log.Info("End Mean Variance test.");
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task PopulateMVEntity()
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
                                exposureDataList = kcc.exposureDataModel.Include("SubarrayDatas").Where(mv => mv.TestDataID.Equals("MVT")).ToList();
                                List<int> exposureIDList = (from exp in exposureDataList.Where(e => e.TestDataID.Equals("MVT")) select exp.ExposureID).ToList();
                                subarrayDataModelList = (from mvsu in kcc.subarrayDataModel orderby mvsu.ExposureID where exposureIDList.Contains(mvsu.ExposureID) select mvsu).ToList();
                                log.Info("Mean Variance Test data populated from Database with Model & Subarray Records of ." +
                                    exposureDataList.Count().ToString() + subarrayDataModelList.Count.ToString());
                        }
                    });
                }
                else
                {
                        DBExists = false;
                        await ProcessMVXMLFile();
                        log.Info("Mean Variance Test data populated from Database with Model & Subarray Records of ." + 
                            exposureDataList.Count().ToString() + subarrayDataModelList.Count.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public MeanVarianceTestResultsData ReturnMVResults()
        {
            try
            {
                meanVarianceTestResultData = new MeanVarianceTestResultsData();
                meanVarianceTestResultData.UserExecuted = string.IsNullOrWhiteSpace(enggUserName) ? Environment.UserName.ToUpper() : enggUserName.ToUpper();
                meanVarianceTestResultData.CorrectedMean = String.Join(",", correctedMeanNew.Select(p => p.ToString()).ToArray());
                meanVarianceTestResultData.CorrectedVariance = String.Join(",", correctedVarianceNew.Select(p => p.ToString()).ToArray());
                meanVarianceTestResultData.LinearCurveFit = String.Join(",", linearCurveFit.Select(p => p.ToString()).ToArray());
                meanVarianceTestResultData.Gain = gain;
                meanVarianceTestResultData.TestPassed = meanVarianceTestPassed;
                meanVarianceTestResultData.TestResultImagePath = testResultImagePath;
                meanVarianceTestResultData.DateExecuted = DateTime.Now;
                meanVarianceTestResultDataList.Add(meanVarianceTestResultData);
                return meanVarianceTestResultData;
            }
            catch (Exception ex)
            {
                testResultsSaved = false;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }       
        public async void ProcessAndSendExposure()
        {
            try
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    //send exposure data to camera
                    await commandProcessor.SendExposure(cancellationToken, exposureCount, cidInterface);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task CalculateMeanVariance(CancellationToken ct)
        {
            try
            {
                await Task.Run(() =>
                {                   
                    var numberOfExposures = (exposureCount) / 2;
                    NICorrectedMean = new double[numberOfExposures, meanVarianceRepeats];
                    NICorrectedVariance = new double[numberOfExposures, meanVarianceRepeats];
                    log.Info("Begin Calcluate Mean Variance for Exposure Count " + exposureCount.ToString());
                    for (int trial = 0; trial < 5; trial++)
                    {
                        kronostestappMainForm.cidDataList.Clear();
                        kronostestappMainForm.cidIntegratedDataList.Clear();
                        ProcessAndSendExposure();
                        //int k = 0;
                        while (kronostestappMainForm.cidDataList.Count < exposureCount)
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
                                log.Error("Camera disconnected during MeanVariance test " + kronostestappMainForm.cidDataList.Count.ToString());
                                //cidInterface.Disconnect();
                                //kronostestappMainForm.AutoConnectCamera();
                                //if (cidInterface.IsConnected())
                                //    MessageBox.Show("Camera reconnected successfully.");
                            }
                                
                        }
                        // Calculate the size needed to hold the signal data from the Subarray
                        // Process the exposures CID Data list 
                        CID821_Data cid821Data = new CID821_Data();
                        cid821Data = kronostestappMainForm.cidDataList[5];
                        int length = (cid821Data.dr) * (cid821Data.dc);
                        double[] ROI1 = new double[length];
                        double[] ROI2 = new double[length];                       
                        double BIAS = 0;
                        int lastFrameIndex = 0;
                        double[] mean = new double[length];
                        double[] sum = new double[length];
                        double[] diff = new double[length];
                        // Calculate Mean and Variance 
                        int exposureIndex = 0;
                        // check each frame
                        for (int frameIndex = lastFrameIndex; frameIndex < kronostestappMainForm.cidDataList.Count; frameIndex++)
                        {
                            if (kronostestappMainForm.cidDataList[frameIndex].exposureNumber % 2 == 0)
                            {
                                ROI1 = TestAppHelper.convertVideoDataToDouble(kronostestappMainForm.cidDataList[frameIndex].videoDataList);
                            }
                            else
                            {
                                ROI2 = TestAppHelper.convertVideoDataToDouble(kronostestappMainForm.cidDataList[frameIndex].videoDataList);
                            }
                            if ((ROI1[0] > 0) && (ROI2[0] > 0))
                            {
                                for (int i = 0; i < length; i++)
                                {
                                    mean[i] = (((ROI1[i] + ROI2[i]) / 2) - BIAS);
                                    sum[i] = ((ROI1[i] + ROI2[i]) / 2);
                                    diff[i] = ROI1[i] - ROI2[i];
                                }
                                NICorrectedMean[exposureIndex,trial] = Statistics.Mean(sum);
                                double rms = Statistics.StandardDeviation(diff);
                                double variance = (rms * rms) / 2.0;
                                NICorrectedVariance[exposureIndex,trial] = variance;
                                Array.Clear(ROI1, 0, ROI1.Length);
                                Array.Clear(ROI2, 0, ROI2.Length);
                                // break out of cidDataList[] loop to the exposure loop                              
                                exposureIndex++;
                            }
                            lastFrameIndex = frameIndex;
                        }
                    }//end repeats
                    correctedMeanNew = new double[numberOfExposures];
                    correctedVarianceNew = new double[numberOfExposures];
                    // Avgerage the trials
                    for (int expIndex = 0; expIndex < numberOfExposures; expIndex++)
                    {
                        var meanAvg = 0.0;
                        var varAvg = 0.0;
                        for (int trialIndex = 0; trialIndex < meanVarianceRepeats; trialIndex++)
                        {
                            varAvg = varAvg + NICorrectedVariance[expIndex, trialIndex];
                            meanAvg = meanAvg + NICorrectedMean[expIndex, trialIndex];
                        }
                        correctedMeanNew[expIndex] = meanAvg / meanVarianceRepeats;
                        correctedVarianceNew[expIndex] = varAvg / meanVarianceRepeats;
                    }
                    double yDiff = correctedVarianceNew[1] - correctedVarianceNew[0];
                    double xDiff = correctedMeanNew[1] - correctedMeanNew[0];
                    double slope = (yDiff / xDiff);
                    int repeats = 5;
                    double[] gainByLinReg = new double[repeats];
                    for (int trialIndex = 0; trialIndex < repeats; trialIndex++)
                    {
                        double[] xData = new double[numberOfExposures];
                        double[] yData = new double[numberOfExposures];
                        double sumX = 0;
                        double sumY = 0;
                        double sumXY = 0;
                        double xSq = 0;
                        double ySq = 0;
                        double xy = 0;
                        double sumXSQ = 0;
                        double sumYSQ = 0;
                        for (int i = 0; i < numberOfExposures; i++)
                        {                            
                            xData[i] = NICorrectedMean[i, trialIndex];
                            yData[i] = NICorrectedVariance[i, trialIndex];
                            sumX = sumX + NICorrectedMean[i, trialIndex];
                            sumY = sumY + NICorrectedVariance[i, trialIndex];
                            xSq = NICorrectedMean[i, trialIndex] * NICorrectedMean[i, trialIndex];
                            sumXSQ = sumXSQ + xSq;
                            ySq = NICorrectedVariance[i, trialIndex] * NICorrectedVariance[i, trialIndex];
                            sumYSQ = sumYSQ + ySq;
                            xy = NICorrectedMean[i, trialIndex] * NICorrectedVariance[i, trialIndex];
                            sumXY = sumXY + xy;
                        }                        
                        // Calculate b for the slope
                        double top = (numberOfExposures * sumXY) - (sumX * sumY);
                        double bottom = (numberOfExposures * sumXSQ) - (sumX * sumX);
                        // Calculate Gain 
                        gainByLinReg[trialIndex] = 1 / (top / bottom);
                    }
                    gain = NationalInstruments.Analysis.Math.Statistics.Mean(gainByLinReg);
                    double[] linearCurveFit = new double[kronostestappMainForm.cidDataList.Count];
                    linearCurveFit = CurveFit.LinearFit(correctedMeanNew, correctedVarianceNew);
                    if (linearCurveFit != null)
                    {
                        LinearCurveFit = linearCurveFit;
                    }
                    //todo final test results
                    if(meanVarianceTestLimitsData==null)
                    {
                        GetMVLimits();
                        meanVarianceTestLimitsData = meanVarianceTestLimitsDataList[0];
                    }
                    if (gain >= meanVarianceTestLimitsData.ConversionFactorLowerLimit && gain <= meanVarianceTestLimitsData.ConversionFactorUpperLimit)
                    {
                        meanVarianceTestPassed = true;
                    }
                    else
                    {
                        meanVarianceTestPassed = false;
                        meanVarianceTestFailureReason = "Gain";
                    }
                });
                log.Info("End Calcluate Mean Variance for Exposure Count " + exposureCount.ToString());
                kronostestappMainForm.cidDataList.Clear();
                kronostestappMainForm.cidIntegratedDataList.Clear();
            }
            catch (Exception ex)
            {
                meanVarianceTestPassed = false;
                kronostestappMainForm.cidDataList.Clear();
                kronostestappMainForm.cidIntegratedDataList.Clear();
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }                           
        public async Task ProcessMVXMLFile()
        {
            try
            {
                await Task.Run(() =>
                {
                    log.Info("Begin Reading MeanVarianceData XML.");                  
                    commandProcessor.ProcessExposureXMLData("MeanVarianceData.xml");
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
        public async Task GetMVLimits()
        {
            try
            {
                if (TestAppHelper.CheckDBExists())
                {
                    DBExists = true;
                    await Task.Run(() =>
                    {
                        //comment below line while unit testing
                        if(kronostestappMainForm!=null)
                            kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                        using (var kcc = new KronosCamContext())
                        {
                                meanVarianceTestLimitsDataList = (from mvlimits in kcc.meanVarianceTestLimitsData orderby mvlimits.DefinedDate descending select mvlimits).ToList();
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
