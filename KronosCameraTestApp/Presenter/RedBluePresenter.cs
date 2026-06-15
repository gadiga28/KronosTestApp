using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.View;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Thermo.Kronos.Instrument.Camera.Interface;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Drawing;
using System.Configuration;
using CIDSoftwareApplication;
using LabJack.LabJackUD;

namespace KronosCameraTestApp.Presenter
{
    public class RedBluePresenter
    {
        KronosTestAppMainForm kronostestappMainForm { get; set; }
        RedBlueTestForm redBlueTestForm { get; set; }
        RedBlueTestResults redBlueTestResults { get; set; }
        List<RedBlueTestResults> redBlueTestResultsList { get; set; }
        string enggUserName = string.Empty;
        public string EnggUserName
        {
            get { return enggUserName; }
            set { enggUserName = value; }
        }
        CommandProcessor commandProcessor = null;
        List<SubarrayDataModel> subarrayDataModelList = new List<SubarrayDataModel>();
        public List<SubarrayDataModel> SubarrayDataModelList
        {
            get { return subarrayDataModelList; }
            set { subarrayDataModelList = value; }
        }       
        UInt32 exposureInterval = 0;
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CIDInterface cidInterface = null;      
        List<CID821_Data> cidDataList = new List<CID821_Data>();
        String path = string.Empty;       
        XDocument xmlResultsDoc;
        bool DBExists = false;
        private static uint xArraySize = 2048;
        private static uint yArraySize = 2048;
        ExposureDataModel exposureDataModel { get; set; }
        public ExposureDataModel ExposureDataModel
        {
            get { return exposureDataModel; }
            set { exposureDataModel = value; }
        }
        List<ExposureDataModel> exposureDataList = new List<ExposureDataModel>();       
        public List<ExposureDataModel> ExposureDataList
        {
            get { return exposureDataList; }
            set { exposureDataList = value; }
        }
        bool redBlueTestPassed = true;
        public bool RedBlueTestPassed
        {
            get { return redBlueTestPassed; }
            set { redBlueTestPassed = value; }
        }
        bool uvTestPassed = true;
        public bool UVTestPassed
        {
            get { return uvTestPassed; }
            set { uvTestPassed = value; }
        }
        private double[,] zDataMasked = new double[xArraySize, yArraySize];
        public double[,] ZDataMasked
        {
            get { return zDataMasked; }
            set { zDataMasked = value; }
        }
        private double[,] zDataDifference;
        public double[,] ZDataDifference
        {
            get { return zDataDifference; }
            set { zDataDifference = value; }
        }
        private double[,] zDataRedDouble;
        public double[,] ZDataRedDouble
        {
            get { return zDataRedDouble; }
            set { zDataRedDouble = value; }
        }
        private double[,] zDataBlueDouble;
        public double[,] ZDataBlueDouble
        {
            get { return zDataBlueDouble; }
            set { zDataBlueDouble = value; }
        }
        double uvMean;
        public double UVMean
        {
            get { return uvMean; }
            set { uvMean = value; }
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
        LEDCalibrationLimitsData ledCalibrationLimitsData { get; set; }
        public LEDCalibrationLimitsData LEDCalibrationLimitsData
        {
            get { return ledCalibrationLimitsData; }
            set { ledCalibrationLimitsData = value; }
        }
        List<LEDCalibrationLimitsData> ledCalibrationLimitsDataList { get; set; }
        public List<LEDCalibrationLimitsData> LEDCalibrationLimitsDataList
        {
            get { return ledCalibrationLimitsDataList; }
            set { ledCalibrationLimitsDataList = value; }
        }
        int uvInterval;
        public int UVInterval
        {
            get { return uvInterval; }
            set { uvInterval = value; }
        }
        public RedBluePresenter(RedBlueTestForm redBlueTestForm, KronosTestAppMainForm kronostestappMainForm)
        {
            try
            {
                this.redBlueTestForm = redBlueTestForm;
                redBlueTestForm.redBluePresenter = this;
                kronostestappMainForm.redBluePresenter = this;
                exposureDataModel = new ExposureDataModel();               
                exposureDataList = new List<ExposureDataModel>();
                this.kronostestappMainForm = kronostestappMainForm;
                path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                commandProcessor = new CommandProcessor(cidInterface);   
                redBlueTestResultsList = new List<RedBlueTestResults>();
                ledCalibrationLimitsDataList = new List<LEDCalibrationLimitsData>();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task GetRedBlueData(CIDInterface cidInterface)
        {
            try
            {
                this.cidInterface = cidInterface;
                kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                if (TestAppHelper.CheckDBExists())
                {
                    await PopulateRedBlueEntity();
                    DBExists = true;
                }
                else
                {
                    await ProcessRedBlueXMLFile();
                    log.Info("Red&Blue test data populated from XML Repository.");
                    DBExists = false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task runRedBlueTest(CIDInterface cidInterface, CancellationToken ct, string userMode, int exposureCount = 2)
        {
            try
            {
                if (!ct.IsCancellationRequested)
                {
                    kronostestappMainForm.cidDataList.Clear();
                    kronostestappMainForm.cidIntegratedDataList.Clear();
                    this.exposureCount = exposureCount;
                    log.Info("Begin Red&Blue test.");
                    this.cidInterface = cidInterface;
                    if (cidInterface != null)
                        cidInterface.ResetExposureId();
                    kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                    kronostestappMainForm.cidDataList.Clear();
                    kronostestappMainForm.cidIntegratedDataList.Clear();
                    redBlueTestPassed = true;                                  
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
                   // int avg = (int)(exposureDataList[2].ExposureInterval / uvInterval);
                    //await InvokeLabJack(0, 5.00, avg, ct);
                    log.Info("End Red&Blue test.");
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }           
        private async Task WriteTestResultsToXML()
        {
            try
            {
                 await Task.Run(() =>
                {
                log.Info("Begin saving Red Blue Test Results to RedBlueResultsData XML.");
                xmlResultsDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "RedBlueResultsData.xml");
                if (xmlResultsDoc != null)
                {
                    xmlResultsDoc.Element("RedBlueTestResults").Add
                        (
                            new XElement("TestResult",
                            new XElement("ResultsID", DBExists ? redBlueTestResults.ResultsID : redBlueTestResultsList.Count + 1),
                            new XElement("TestResultsID", redBlueTestResults.TestResultsID),
                            new XElement("DateExecuted", redBlueTestResults.DateExecuted),
                            new XElement("UserExecuted", redBlueTestResults.UserExecuted),
                            new XElement("RedImageData", redBlueTestResults.RedImageData),
                            new XElement("BlueImageData", redBlueTestResults.BlueImageData),
                            new XElement("RedBlueImageDifference", redBlueTestResults.RedBlueImageDifference),
                            new XElement("TestPassed", redBlueTestResults.TestPassed),
                            new XElement("TestResultImage", redBlueTestResults.TestResultImagePath))
                        );
                    xmlResultsDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "RedBlueResultsData.xml");
                    testResultsSaved = true;
                    log.Info("End saving Red Blue Test Results to RedBlueResultsData XML.");
                }
                {
                    log.Error("RedBlueResultsData XML is null");
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
        public RedBlueTestResults ReturnRedBlueResults()
        {
            try
            {
                redBlueTestResults = new RedBlueTestResults();
                redBlueTestResults.UserExecuted = string.IsNullOrWhiteSpace(enggUserName) ? Environment.UserName.ToUpper() : enggUserName.ToUpper(); 
                redBlueTestResults.RedImageData = string.Empty;
                redBlueTestResults.BlueImageData = string.Empty;
                redBlueTestResults.RedBlueImageDifference = string.Empty;
                redBlueTestResults.TestPassed = redBlueTestPassed;
                redBlueTestResults.TestResultImagePath = testResultImagePath;
                redBlueTestResults.DateExecuted = DateTime.Now;
                redBlueTestResults.UVMean = uvMean;
                redBlueTestResultsList.Add(redBlueTestResults);
                return redBlueTestResults;
            }
            catch (Exception ex)
            {
                testResultsSaved = false;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }       
        public async Task PopulateRedBlueEntity()
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
                                exposureDataList = kcc.exposureDataModel.Include("SubarrayDatas").Where(mv => mv.TestDataID.Equals("RBT")).ToList();
                                log.Info("Red&Blue Test data populated from Database with Model"+ exposureDataList.Count().ToString());
                        }
                    });
                }
                else
                {
                    //get the data from script files
                    await ProcessRedBlueXMLFile();
                    DBExists = false;
                    log.Info("Red&Blue Test data populated from Database with Model & Subarray Records of ." + exposureDataList.Count().ToString() + exposureDataList.Count.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }      
        public async Task CalculateRedBlue(CancellationToken ct)
        {
            try
            {
                await Task.Run(() =>
                {
                    log.Info("Begin Calcluate Red&Blue for Exposure Count " + exposureCount.ToString());
                    // Wait until method is fully acquired    
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
                            log.Error("Camera disconnected during RebBlue test " + kronostestappMainForm.cidIntegratedDataList.Count.ToString());
                           // cidInterface.Disconnect();
                           // kronostestappMainForm.AutoConnectCamera();
                            //if (cidInterface.IsConnected())
                            //    MessageBox.Show("Camera reconnected successfully.");
                        }
                    }
                    CID821_Data cid821Data = new CID821_Data();
                    cid821Data = kronostestappMainForm.cidDataList[0];
                    int length = (cid821Data.dr) * (cid821Data.dc);
                    // Plot the data for the RED
                    var zDataRed = new int[xArraySize, yArraySize];
                    var zDataBlue = new int[xArraySize, yArraySize];
                    //check with Bill
                    zDataRed = kronostestappMainForm.cidIntegratedDataList[0].videoIntegratedDataList;
                    zDataBlue = kronostestappMainForm.cidIntegratedDataList[1].videoIntegratedDataList;
                    zDataRedDouble = new double[xArraySize, yArraySize];
                    zDataRedDouble = TestAppHelper.ConvertVideoDataToDouble2D(zDataRed);                  
                    zDataBlueDouble = new double[xArraySize, yArraySize];
                    zDataBlueDouble = TestAppHelper.ConvertVideoDataToDouble2D(zDataBlue);
                   
                });
                log.Info("End Calcluate Red&Blue for Exposure Count " + exposureCount.ToString());                

                //await InvokeLabJack(0, 0.00, 100,ct);
                //CacluateUVMean();
                kronostestappMainForm.cidDataList.Clear();
                kronostestappMainForm.cidIntegratedDataList.Clear();
            }
            catch (Exception ex)
            {
                kronostestappMainForm.cidDataList.Clear();
                kronostestappMainForm.cidIntegratedDataList.Clear();
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task calculateRedBlueDiff()
        {
            try
            {
                await Task.Run(() =>
                {
                    if ((zDataRedDouble != null) && (zDataBlueDouble != null))
                    {
                        zDataDifference = new double[xArraySize, yArraySize];
                        for (int x = 0; x < xArraySize; x++)
                        {
                            for (int y = 0; y < yArraySize; y++)
                            {
                                zDataDifference[x, y] = zDataRedDouble[x, y] - zDataBlueDouble[x, y];
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task BuildMaskImage(double[,] zData)
        {
            try
            {
                await Task.Run(() =>
                {                            
                    Array.Copy(zData, 0, zDataMasked, 0, xArraySize * yArraySize);
                    for (int x = 0; x < 1000; x++)
                    {
                        for (int y = 0; y < 500; y++)
                        {
                            zDataMasked[x, y] = 0;
                        }
                    }
                    for (int x = 1000; x < 1200; x++)
                    {
                        for (int y = 1200; y < 1300; y++)
                        {
                            zDataMasked[x, y] = 0;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task WriteRedBlueTestResultToDB(CancellationToken ct, string TestResultImageFilePath)
        {
            try
            {
                    kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                    //write to DB
                    if (ct.IsCancellationRequested)
                        return;
                    redBlueTestResults = new RedBlueTestResults();
                    redBlueTestResults.UserExecuted = string.IsNullOrWhiteSpace(enggUserName) ? Environment.UserName : enggUserName;
                    redBlueTestResults.RedImageData = string.Empty;
                    redBlueTestResults.BlueImageData = string.Empty;
                    redBlueTestResults.RedBlueImageDifference = string.Empty;
                    redBlueTestResults.UVMean = uvMean;
                    redBlueTestResults.TestPassed = redBlueTestPassed;
                    redBlueTestResults.TestResultImagePath = TestResultImageFilePath;
                    redBlueTestResultsList.Add(redBlueTestResults);
                    using (var kcc = new KronosCamContext())
                    {
                        log.Info("Begin Red&Blue Test Results Saving to DB");
                        kcc.redBlueTestResults.Add(redBlueTestResults);
                        int savedTestResults = kcc.SaveChanges();
                        if (savedTestResults > 0)
                        {
                            testResultsSaved = true;
                            log.Info("Red&Blue Test Results Saved to DB");
                        }
                        else
                        {
                            log.Error("Error occured while saving Red&Blue Test Results to DB");
                        }
                    }
                    log.Info("End Red&Blue Test Results Saving to DB");                                 
                    //write test results to XML
                    await WriteTestResultsToXML();
                    // ExcelUtilities.WriteTestResultsToExcel("Red&Blue Test", redBlueTestResults);               
            }
            catch (Exception ex)
            {
                testResultsSaved = false;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }          
        private async Task ProcessRedBlueXMLFile()
        {
            try
            {
                await Task.Run(() =>
                {
                    log.Info("Begin Reading Red&Blue Data XML.");
                    DataSet ds = new DataSet();
                    ds.ReadXml(ConfigurationManager.AppSettings["XMLFilePath"].ToString() +"RedBlueData.xml");
                    if (ds.Tables.Count > 0)
                    {
                        DataView dvExposure;
                        dvExposure = ds.Tables[0].DefaultView;
                        exposureDataList = new List<ExposureDataModel>();
                        foreach (DataRowView dr in dvExposure)
                        {
                            ExposureDataModel m = new ExposureDataModel();
                            m.ExposureID = Convert.ToInt32(dr[0]);
                            m.TestDataID = Convert.ToString(dr[1]);
                            m.ExposureName = Convert.ToString(dr[2]);
                            m.DateDefined = Convert.ToDateTime(dr[3]);
                            m.UserModified = Convert.ToString(dr[4]);
                            m.ExposureRegionXo = Convert.ToInt32(dr[5]);
                            m.ExposureRegionYo = Convert.ToInt32(dr[6]);
                            m.ExposureRegiondX = Convert.ToInt32(dr[7]);
                            m.ExposureRegiondY = Convert.ToInt32(dr[8]);
                            m.ExposureInterval = Convert.ToInt32(dr[9]);
                            m.ExposureNDROS = Convert.ToInt32(dr[10]);
                            m.ExposureFPN = Convert.ToInt32(dr[11]);
                            m.FullFrameEnabled = dr[12].ToString() == "0" ? false : true;
                            m.DarkFrameEnabled = dr[13].ToString() == "0" ? false : true;
                            m.UserDefaultConfiguration = dr[14].ToString() == "0" ? false : true;
                            m.LED1Enabled = dr[15].ToString() == "0" ? false : true;
                            m.LED2Enabled = dr[16].ToString() == "0" ? false : true;
                            m.LED3Enabled = dr[17].ToString() == "0" ? false : true;
                            m.LEDOnTime = Convert.ToInt32(dr[18]);
                            m.LEDOffTime = Convert.ToInt32(dr[19]);
                            m.LEDFlashes = Convert.ToInt32(dr[20]);
                            m.ShutterEnabled = dr[21].ToString() == "0" ? false : true;
                            m.AutoBiasFPNEnabled = Convert.ToByte(dr[22]);
                            m.NumberOfSubarrays = Convert.ToInt32(dr[23]);
                            m.GlobalInject = Convert.ToInt32(dr[24]);
                            m.GlobalInjectDelay = Convert.ToInt32(dr[25]);
                            exposureDataList.Add(m);
                        }                       
                    }
                    else
                    {
                        MessageBox.Show("No Exposure Data to display.", "Exposure Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    log.Info("End Reading Red&Blue Data XML.");
                });
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task runUVTest(CIDInterface cidInterface, CancellationToken ct, string userMode, int exposureCount = 1)
        {
            try
            {
                if (!ct.IsCancellationRequested)
                {
                    kronostestappMainForm.cidDataList.Clear();
                    kronostestappMainForm.cidIntegratedDataList.Clear();
                    this.exposureCount = exposureCount;
                    log.Info("Begin UV test.");
                    this.cidInterface = cidInterface;
                    if (cidInterface != null)
                        cidInterface.ResetExposureId();
                    kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                    kronostestappMainForm.cidDataList.Clear();
                    kronostestappMainForm.cidIntegratedDataList.Clear();                
                    uvTestPassed = false;
                    int avg = (int)(exposureDataList[0].ExposureInterval / uvInterval);
                    await InvokeLabJack(0, 5.00, avg, ct);
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
                   
                    log.Info("End UV test.");
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task CalculateUVTest(CancellationToken ct)
        {
            try
            {
                await Task.Run(() =>
                {
                    log.Info("Begin Calcluate Red&Blue for Exposure Count " + exposureCount.ToString());
                    // Wait until method is fully acquired    
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
                            log.Error("Camera disconnected during RebBlue test " + kronostestappMainForm.cidIntegratedDataList.Count.ToString());                          
                        }
                    }
                    CID821_Data cid821Data = new CID821_Data();
                    cid821Data = kronostestappMainForm.cidDataList[0];                
                });
                log.Info("End Calcluate Red&Blue for Exposure Count " + exposureCount.ToString());

                await InvokeLabJack(0, 0.00, 100, ct);
                CacluateUVMean();
                kronostestappMainForm.cidDataList.Clear();
                kronostestappMainForm.cidIntegratedDataList.Clear();
            }
            catch (Exception ex)
            {
                kronostestappMainForm.cidDataList.Clear();
                kronostestappMainForm.cidIntegratedDataList.Clear();
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void CacluateUVMean()
        {
            try
            {
                double xValue, yValue, endX, endY;
                double photometrySum = 0;
                int numberOfPoints = 0;
                uvMean = 0;
                xValue = exposureDataList[0].SubarrayDatas[0].SubarrayRegionXo;
                yValue = exposureDataList[0].SubarrayDatas[0].SubarrayRegionYo;
                endX = (exposureDataList[0].SubarrayDatas[0].SubarrayRegiondX+ exposureDataList[0].SubarrayDatas[0].SubarrayRegionXo)-1;
                endY = (exposureDataList[0].SubarrayDatas[0].SubarrayRegiondX + exposureDataList[0].SubarrayDatas[0].SubarrayRegionYo) - 1;

                int numX = (int)((endX + 1) - xValue);
                int numY = (int)((endY + 1) - yValue);
                numberOfPoints = numX * numY;

                var localZData = new int[xArraySize, yArraySize];
                localZData = kronostestappMainForm.cidIntegratedDataList[0].videoIntegratedDataList;
                double[,] zlocalZData = TestAppHelper.ConvertVideoDataToDouble2D(localZData);
                int roiXIndex = 0;
                int roiYIndex = 0; 
               
                for (int xIndex = (int)xValue; xIndex < (int)(endX); xIndex++)
                {
                    roiYIndex = 0;

                    for (int yIndex = (int)yValue; yIndex < (int)(endY); yIndex++)
                    {
                        // Build the ROI data array
                        roiYIndex++;                                          
                        photometrySum = photometrySum + zlocalZData[xIndex, yIndex];
                    }                  
                    roiXIndex++;
                }
                if (numberOfPoints > 0)
                {
                    uvMean = photometrySum / numberOfPoints;
                    if (ledCalibrationLimitsData != null)
                    {
                        if (uvMean > ledCalibrationLimitsData.LowerUVLEDLimit && uvMean < ledCalibrationLimitsData.UpperUVLEDLimit)
                        {
                            uvTestPassed = true;
                        }
                        else
                        {
                            uvTestPassed = false;
                        }

                    }
                    else
                    {
                        uvTestPassed = false;
                    }
                    
                }
                  
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task InvokeLabJack(int DAC, double voltage, int interval, CancellationToken ct)
        {
            await Task.Run(() =>
            {

                //long numIterations = 1000; // Should be close to the Exposure Interval
                long numIterations = interval;

                long i = 0, j = 0;
                int numChannels = 1;    //Number of AIN channels, 0-3
                long resolution = 0;    //Configure resolution of the analog inputs (pass a non-zero value for quick sampling). 
                long settlingTime = 1;  //0=5us, 1=10us, 2=100us, 3=1ms, 4=10ms
                double serialNumber = 0;
                double[] voltageResultList = new double[numIterations];
                LJUD.IO ioType = 0;
                LJUD.CHANNEL channel = 0;
                double dblValue = 0;
                double ValueDIPort = 0;
                int dummyInt = 0;
                double dummyDouble = 0;
                double[] ValueDIN = new double[16];
                U3 u3 = null;
                try
                {
                    //Open the first found LabJack.

                    u3 = new U3(LJUD.CONNECTION.USB, "0", true); // Connection through USB
                                                                 //Configure resolution. See section 2.6/3.1 of the User's Guide.
                    LJUD.ePut(u3.ljhandle, LJUD.IO.PUT_CONFIG, LJUD.CHANNEL.AIN_RESOLUTION, resolution, 0);

                    //Configure settling time
                    LJUD.ePut(u3.ljhandle, LJUD.IO.PUT_CONFIG, LJUD.CHANNEL.AIN_SETTLING_TIME, settlingTime, 0);

                    //Enable Counter0.
                    LJUD.AddRequest(u3.ljhandle, LJUD.IO.PUT_COUNTER_ENABLE, 1, 1, 0, 0);

                    //Request that DAC be set to 5 volts.
                    LJUD.AddRequest(u3.ljhandle, LJUD.IO.PUT_DAC, DAC, voltage, 0, 0);
                    LJUD.ePut(u3.ljhandle, LJUD.IO.PUT_DAC, DAC, voltage, 0);

                    LJUD.eGet(u3.ljhandle, LJUD.IO.GET_CONFIG, LJUD.CHANNEL.SERIAL_NUMBER, ref serialNumber, 0);
                    //if (shutterTestForm != null)
                    //    BeginInvoke(new System.Action(() => shutterTestForm.serialNumberTextBox.Text = serialNumber.ToString()));

                    LJUD.GoOne(u3.ljhandle);

                    //Add analog input requests.
                    for (j = 0; j < numChannels; j++)
                    {
                        LJUD.AddRequest(u3.ljhandle, LJUD.IO.GET_AIN, (LJUD.CHANNEL)j, 0, 0, 0);
                    }
                }

                catch (LabJackUDException e)
                {
                    log.Error("LabJack open error" + MethodBase.GetCurrentMethod(), e);
                    MessageBox.Show("LabJack open error - " + e + " - UV Test maybe impacted",
                            "LabJack Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                   
                    return;
                }

                for (i = 0; i < numIterations; i++)
                {
                    if (!ct.IsCancellationRequested)
                    {
                        try
                        {
                            if (u3 != null)
                                LJUD.GoOne(u3.ljhandle);
                            else
                                return;
                        }
                        catch (LabJackUDException e)
                        {
                            MessageBox.Show("LabJack error " + e,
                                "LabJack LJUD.GoOne error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        //Get all the results.  The input measurement results are stored. 
                        dblValue = 0;
                        bool finished = false;
                        LJUD.GetFirstResult(u3.ljhandle, ref ioType, ref channel, ref dblValue, ref dummyInt, ref dummyDouble);

                        while (!finished)
                        {
                            switch (ioType)
                            {
                                case LJUD.IO.GET_AIN:
                                    {
                                        if ((int)channel == 0)
                                        {
                                            if (DAC == 1)
                                            {
                                                //if (shutterTestForm != null)
                                                //    BeginInvoke(new System.Action(() => shutterTestForm.AIO.PlotYAppend(dblValue)));
                                                voltageResultList[i] = dblValue;
                                            }
                                        }
                                    }
                                    ValueDIN[(int)channel] = dblValue;
                                    break;

                                case LJUD.IO.GET_DIGITAL_PORT:
                                    ValueDIPort = dblValue;
                                    break;
                            }

                            try
                            {
                                LJUD.GetNextResult(u3.ljhandle, ref ioType, ref channel, ref dblValue,
                                    ref dummyInt, ref dummyDouble);

                            }
                            catch (LabJackUDException e)
                            {
                                if (e.LJUDError == LJUD.LJUDERROR.NO_MORE_DATA_AVAILABLE)
                                    finished = true;
                                else
                                    MessageBox.Show("LabJack error " + e,
                                        "LabJack LJUD.GetNextResult error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                        //Execute the requests.
                    
                }

                // If shutter test
                if (DAC == 1)
                {
                    if ((voltageResultList[20] > 4) && (voltageResultList[130] < 1) &&
                        (voltageResultList[180] < 1) && (voltageResultList[280] > 4))
                    {
                        //if (shutterTestForm != null)
                        //    Invoke(new System.Action(() => shutterTestForm.passFailTextBox.Text = "Passed"));
                    }

                    // Reset DAC to 0V
                    LJUD.AddRequest(u3.ljhandle, LJUD.IO.PUT_DAC, DAC, 0.00, 0, 0);
                    LJUD.ePut(u3.ljhandle, LJUD.IO.PUT_DAC, DAC, 0.00, 0);
                    LJUD.GoOne(u3.ljhandle);
                }
            });
        }
        public async Task GetLEDCalibrationLimits()
        {
            try
            {
                if (TestAppHelper.CheckDBExists())
                {
                    DBExists = true;
                    await Task.Run(() =>
                    {
                        //comment below line while unit testing
                        if (kronostestappMainForm != null)
                            kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                        using (var kcc = new KronosCamContext())
                        {
                            ledCalibrationLimitsDataList = (from ledLimits in kcc.ledCalibrationLimitsData orderby ledLimits.DefinedDate descending select ledLimits).ToList();
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
