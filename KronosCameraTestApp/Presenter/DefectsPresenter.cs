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
    public class DefectsPresenter
    {
        double[,] zIntegratedData;
        DarkCurrentTestForm darkCurrentTestForm { get; set; }
        KronosTestAppMainForm kronostestappMainForm { get; set; }
        List<DefectsTestLimitsData> defectsTestLimitsDataList { get; set; }
        public List<DefectsTestLimitsData> DefectsTestLimitsDataList
        {
            get { return defectsTestLimitsDataList; }
            set { defectsTestLimitsDataList = value; }
        }
        DefectsTestLimitsData defectsTestLimitsData { get; set; }
        public DefectsTestLimitsData DefectsTestLimitsData
        {
            get { return defectsTestLimitsData; }
            set { defectsTestLimitsData = value; }
        }   
       DefectsTestResults defectsTestResultsData { get; set; }
        public DefectsTestResults DefectsTestResultsData
        {
            get { return defectsTestResultsData; }
            set { defectsTestResultsData = value; }
        }
        List<DefectsTestResults> defectsTestResultsDataList;
        public List<DefectsTestResults> DefectsTestResultsDataList
        {
            get { return defectsTestResultsDataList; }
            set { defectsTestResultsDataList = value; }
        }
        List<ExposureDataModel> exposureDataList = new List<ExposureDataModel>();
        List<SubarrayDataModel> subarrayDataModelList = new List<SubarrayDataModel>();
        ExposureDataModel exposureDataModel { get; set; }
        SubarrayDataModel subarrayDataModel { get; set; }
        CommandProcessor commandProcessor = null;
        UInt32 exposureInterval = 0;
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CIDInterface cidInterface = null;
        String path = string.Empty;
        public bool DBExists = false;
        List<Point> driftROIPointsList;
        List<Point> clusterPixelList;
        private bool deaddarkPixelsListCompleted = false;
        private bool[,] validROIList = new bool[2048, 2048];
        string serializeObjectFileName = string.Empty;
        public bool DeaddarkPixelsListCompleted
        {
            get { return deaddarkPixelsListCompleted; }
            set { deaddarkPixelsListCompleted = value; }
        }
        public bool[,] ValidROIList
        {
            get { return validROIList; }
            set { validROIList = value; }
        }
        public List<Point> ClusterPixelList
        {
            get { return clusterPixelList; }
            set { clusterPixelList = value; }
        }
        public List<Point> DriftROIPointsList
        {
            get { return driftROIPointsList; }
            set { driftROIPointsList = value; }
        }
        bool enableMask = false;
        public bool EnableMask
        {
            get { return enableMask; }
            set { enableMask = value; }
        }
        int prnuDefects = 0;
        public int PRNUDefects
        {
            get { return prnuDefects; }
            set { prnuDefects = value; }
        }
        int trapDefects = 0;
        public int TrapDefects
        {
            get { return trapDefects; }
            set { trapDefects = value; }
        }
        int pixDefects = 0;
        public int PixDefects
        {
            get { return pixDefects; }
            set { pixDefects = value; }
        }
        int clusterDefects = 0;
        public int ClusterDefects
        {
            get { return clusterDefects; }
            set { clusterDefects = value; }
        }
        int totalClusters = 0;
        public int TotalClusters
        {
            get { return totalClusters; }
            set { totalClusters = value; }
        }
        int pRNUDefectsCount = 0;
        public int PRNUDefectsCount
        {
            get { return pRNUDefectsCount; }
            set { pRNUDefectsCount = value; }
        }
      
        int trapDefectsCount = 0;
        public int TrapDefectsCount
        {
            get { return trapDefectsCount; }
            set { trapDefectsCount = value; }
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
        int totalDefects = 0;
        public int TotalDefects
        {
            get { return totalDefects; }
            set { totalDefects = value; }
        }
        int darkRows = 0;
        public int DarkRows
        {
            get { return darkRows; }
            set { darkRows = value; }
        }
        int darkColumns = 0;
        public int DarkColumns
        {
            get { return darkColumns; }
            set { darkColumns = value; }
        }
        int lightRows = 0;
        public int LightRows
        {
            get { return lightRows; }
            set { lightRows = value; }
        }
        int lightColumns = 0;
        public int LightColumns
        {
            get { return lightColumns; }
            set { lightColumns = value; }
        }
        int deadPixels = 0;
        public int DeadPixels
        {
            get { return deadPixels; }
            set { deadPixels = value; }
        }
        int hotPixels = 0;
        public int HotPixels
        {
            get { return hotPixels; }
            set { hotPixels = value; }
        }
        int darkPixels = 0;
        public int DarkPixels
        {
            get { return darkPixels; }
            set { darkPixels = value; }
        }
        double meanDefects = 0;
        public double MeanDefects
        {
            get { return meanDefects; }
            set { meanDefects = value; }
        }
        int driftROICount = 0;
        public int DriftROICount
        {
            get { return driftROICount; }
            set { driftROICount = value; }
        }
        double percentBelowMeanRow = 0;
        public double PercentBelowMeanRow
        {
            get { return percentBelowMeanRow; }
            set { percentBelowMeanRow = value; }
        }
        double percentAboveMeanRow = 0;
        public double PercentAboveMeanRow
        {
            get { return percentAboveMeanRow; }
            set { percentAboveMeanRow = value; }
        }
        double percentBelow = 0;
        public double PercentBelow
        {
            get { return percentBelow; }
            set { percentBelow = value; }
        }
        double percentAbove = 0;
        public double PercentAbove
        {
            get { return percentAbove; }
            set { percentAbove = value; }
        }
        double percentBelowValue = 0;
        public double PercentBelowValue
        {
            get { return percentBelowValue; }
            set { percentBelowValue = value; }
        }
        double percentAboveValue = 0;
        public double PercentAboveValue
        {
            get { return percentAboveValue; }
            set { percentAboveValue = value; }
        }
        double percentBelowRowValue = 0;
        public double PercentBelowRowValue
        {
            get { return percentBelowRowValue; }
            set { percentBelowRowValue = value; }
        }
        double percentAboveRowValue = 0;
        public double PercentAboveRowValue
        {
            get { return percentAboveRowValue; }
            set { percentAboveRowValue = value; }
        }
        bool defectsTestPassed = true;
        public bool DefectsTestPassed
        {
            get { return defectsTestPassed; }
            set { defectsTestPassed = value; }
        }
        string testResultImagePath = string.Empty;
        public string TestResultImagePath
        {
            get { return testResultImagePath; }
            set { testResultImagePath = value; }
        }
        private double[,] zData = new double[xArraySize, yArraySize];
        public double[,] ZData
        {
            get { return zData; }
            set { zData = value; }
        }
        int adjacentPixlesInCluster = 0;
        public int AdjacentPixlesInCluster
        {
            get { return adjacentPixlesInCluster; }
            set { adjacentPixlesInCluster = value; }
        }
        int clusterSize = 0;
        public int ClusterSize
        {
            get { return clusterSize; }
            set { clusterSize = value; }
        }
        private bool deaddarkPixelsLimitReached = false;
        public bool DeadDarkPixelsLimitReached
        {
            get { return deaddarkPixelsLimitReached; }
            set { deaddarkPixelsLimitReached = value; }
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
        private bool[,] maskList = new bool[2048, 2048];
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
        bool enableTestData = false;
        public bool EnableTestData
        {
            get { return enableTestData; }
            set { enableTestData = value; }
        }
        CID821_Data cidAnnotationData;
        public CID821_Data CIDAnnotationData
        {
            get { return cidAnnotationData; }
            set { cidAnnotationData = value; }
        }
        public struct MaskDataStruct
        {
            public int Xo;
            public int Yo;
            public int dX;
            public int dY;
            public int Ordnung;
            public bool IsDriftROI;
            public String name;
            public double Wavelength;
        }
        public List<MaskDataStruct> maskDataList;
        public List<MaskDataStruct> MaskDataList
        {
            get { return maskDataList; }
            set { maskDataList = value; }
        }
        List<MaskROIsModel> maskROIsModelList;
        public List<MaskROIsModel> MaskROIsModelList
        {
            get { return maskROIsModelList; }
            set { maskROIsModelList = value; }
        }
        private bool getDeadAndDarkPixelList = false;
        private  List<Point> deadAndDarkPixelList = new List<Point>();
        public bool GetDeadAndDarkPixelList
        {
            get { return getDeadAndDarkPixelList; }
            set { getDeadAndDarkPixelList = value; }
        }
        public List<Point> DeadAndDarkPixelList
        {
            get { return deadAndDarkPixelList; }
            set { deadAndDarkPixelList = value; }
        }
        private bool firmwareCorrectedMaxPixels = false;
        private int defectivePixelsPerROICount = 0;
        private int defPixInMaskROI = 0;
        public bool FirmwareCorrectedMaxPixels
        {
            get { return firmwareCorrectedMaxPixels; }
            set { firmwareCorrectedMaxPixels = value; }
        }
        public int DefectivePixelsPerROICount
        {
            get { return defectivePixelsPerROICount; }
            set { defectivePixelsPerROICount = value; }
        }
        public int DefPixInMaskROI
        {
            get { return defPixInMaskROI; }
            set { defPixInMaskROI = value; }
        }
        //Point[] defPoints;
        private List<Point> defPixPointsList;
        public List<Point> DefPixPointsList
        {
            get { return defPixPointsList; }
            set { defPixPointsList = value; }
        }
        List<KeyValuePair<string, List<Point>>> defPixelROIListPairs;
        public List<KeyValuePair<string, List<Point>>> DefPixelROIListPairs
        {
            get { return defPixelROIListPairs; }
            set { defPixelROIListPairs = value; }
        }

        public string SerializeObjectFileName
        {
            get { return serializeObjectFileName; }
            set { serializeObjectFileName = value; }
        }

        int xROIRange = 0;

        public int XROIRange
        {
            get { return xROIRange; }
            set { xROIRange = value; }
        }

        int yROIRange = 0;

        public int YROIRange
        {
            get { return yROIRange; }
            set { yROIRange = value; }
        }

        int yROIRangeLowLambda = 0;
        public int YROIRangeLowLambda
        {
            get { return yROIRangeLowLambda; }
            set { yROIRangeLowLambda = value; }
        }
        string defectsTestFailureReason = string.Empty;
        public string DefectsTestFailureReason
        {
            get { return defectsTestFailureReason; }
            set { defectsTestFailureReason = value; }
        }
        bool autoPretest = false;
        public bool AutoPretest
        {
            get { return autoPretest; }
            set { autoPretest = value; }
        }
        public DefectsPresenter(DefectsTestForm defectsTestForm, KronosTestAppMainForm kronostestappMainForm)
        {
            try
            {                
                this.darkCurrentTestForm = darkCurrentTestForm;
                this.kronostestappMainForm = kronostestappMainForm;
                defectsTestForm.defectsPresenter = this;
                kronostestappMainForm.defectsPresenter = this;
                path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                commandProcessor = new CommandProcessor(cidInterface);
                defectsTestResultsDataList = new List<DefectsTestResults>();
                exposureDataModel = new ExposureDataModel();
                subarrayDataModel = new SubarrayDataModel();
                subarrayDataModelList = new List<SubarrayDataModel>();
                exposureDataList = new List<ExposureDataModel>();               
                DefectsTestLimitsDataList = new List<DefectsTestLimitsData>();
                int yindex = 1200;
                int xindexLeft = 0;
                int xindexRight = 2048;
                for (int y = yindex; y < 2048; y++)
                {
                    for (int x = 0; x < 500; x++)
                    {
                        if ((x < xindexLeft))
                            maskList[x, y] = true;
                    }
                    for (int x = 1550; x < 2048; x++)
                    {
                        if ((x > xindexRight))
                            maskList[x, y] = true;
                    }
                    xindexLeft++;
                    xindexRight--;
                }
              
                
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task GetDefectsData(CIDInterface cidInterface)
        {
            try
            {
                this.cidInterface = cidInterface;
                kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                if (TestAppHelper.CheckDBExists())
                {
                    await PopulateDefectsEntity();
                    DBExists = true;
                }
                else
                {
                    await ProcessDefectsXMLFile();
                    log.Info("Defects test data populated from XML Repository.");
                    DBExists = false;               
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async void runDefectsTest(CIDInterface cidInterface, CancellationToken ct, string userMode, int expCount = 6)
        {
            try
            {
                if (!ct.IsCancellationRequested)
                {
                   
                    log.Info("Begin Defects test.");
                    this.exposureCount = expCount;
                    defectsTestPassed = false;
                    this.cidInterface = cidInterface;
                    if (cidInterface != null)
                        cidInterface.ResetExposureId();
                    kronostestappMainForm.SetDatabaseConnectivityStatusBarPanel();
                    kronostestappMainForm.cidDataList.Clear();
                    kronostestappMainForm.cidIntegratedDataList.Clear();
                    this.usermode = userMode;
                    buildROIMask();
                    firmwareCorrectedMaxPixels = false;
                    if(getDeadAndDarkPixelList)
                        await DeadNDefectPxiels(ct);                    
                    if (firmwareCorrectedMaxPixels)
                        return;
                    if (!ct.IsCancellationRequested)
                    {                       
                        await commandProcessor.ProcessExposureData(exposureDataList, ct);
                        //send exposure data to camera
                        await commandProcessor.SendExposure(ct, exposureCount, cidInterface);                       
                    }
                    
                    log.Info("End Defects test.");
                }
                else

                    return;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }   
        private async Task DeadNDefectPxiels(CancellationToken ct)
        {
            try
            {
                deaddarkPixelsListCompleted = false;
                deaddarkPixelsLimitReached = false;
                deadAndDarkPixelList.Clear();
                defPixInMaskROI = 0;
                defectivePixelsPerROICount = 0;
                cidInterface.SendLogLevel(3);
                cidInterface.SetDarkPixelThreshold(Convert.ToUInt32(defectsTestLimitsData.DarkPixelThreshold));
                cidInterface.SetCalibrateThresholds();
                await Task.Delay(10000);
                int MaxNumDefPixsPerROI = 0;
                // Set log level back
                //cidInterface.SendLogLevel(2);
               

                while(!deaddarkPixelsListCompleted)
                {
                    if (ct.IsCancellationRequested)
                        return;
                }
                if (deadAndDarkPixelList.Count > defectsTestLimitsData.DeadDarkPixelsListReadLimit)
                {
                    deaddarkPixelsLimitReached = true;
                    return;
                }
                deaddarkPixelsListCompleted = true;
                cidInterface.SetCorrectDeadpixels(1);
                cidInterface.SendLogLevel(2);
                defPixelROIListPairs = new List<KeyValuePair<string, List<Point>>>();
                defPixPointsList = new List<Point>();
                foreach (MaskDataStruct maskDataStruct in maskDataList)
                {
                    List<Point> defectsInMaskROIList = new List<Point>();


                    for (int x = maskDataStruct.Xo; x < maskDataStruct.Xo + maskDataStruct.dX; x++)
                    {
                        for (int y = maskDataStruct.Yo; y < maskDataStruct.Yo + maskDataStruct.dY; y++)
                        {
                            for (int i = 0; i < deadAndDarkPixelList.Count; i++)
                            {
                                Point defPix = new Point(deadAndDarkPixelList[i].X, deadAndDarkPixelList[i].Y);
                                if ((defPix.X == x) && (defPix.Y == y))
                                {
                                    // defectivePixelsPerROICount++;
                                    defectsInMaskROIList.Add(defPix);
                                    defPixPointsList.Add(defPix);
                                }
                            }
                        }
                    }

                    // Go through all the points/defective pixels, in this ROI and see if there are more than one in range
                    if (defectsInMaskROIList.Count > 1)
                    {
                        foreach (Point defectivePixelPoint in defectsInMaskROIList)
                        {                          

                            for (int x = defectivePixelPoint.X; x < defectivePixelPoint.X + defectsTestLimitsData.XROIRange; x++)
                            {
                                for (int y = defectivePixelPoint.Y; y < defectivePixelPoint.Y + defectsTestLimitsData.YROIRange; y++)
                                {
                                    for (int i = 0; i < defectsInMaskROIList.Count; i++)
                                    {
                                        Point defPix = new Point(defectsInMaskROIList[i].X, defectsInMaskROIList[i].Y);
                                        if ((defPix.X == x) && (defPix.Y == y))
                                        {
                                            defectivePixelsPerROICount++;

                                        }
                                    }
                                }
                            }
                        }
                        // subtract the actual point we are comparing to
                        defectivePixelsPerROICount--;
                    }

                    // Make this a limit in the DB - DefectivePixelsPerROI
                    if (defectivePixelsPerROICount > defectsTestLimitsData.DefectivePixelsPerROI)
                    {
                        List<Point> aa = new List<Point>();
                        foreach (Point p in defPixPointsList)
                        {
                            aa.Add(new Point(p.X, p.Y));
                        }
                        string st = maskDataStruct.Xo.ToString() + "," + maskDataStruct.Yo.ToString();
                        defPixelROIListPairs.Add(new KeyValuePair<string, List<Point>>(st, aa));

                        defPixInMaskROI++;
                        //Invoke(new System.Action(() => defectsTestForm.numOfDefPixInROITextBox.Text = defectivePixelsPerROICount.ToString()));
                        if (defectivePixelsPerROICount > MaxNumDefPixsPerROI)
                            MaxNumDefPixsPerROI = defPixInMaskROI;
                    }
                    defectivePixelsPerROICount = 0;
                    defPixPointsList.Clear();                

                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public DefectsTestResults ReturnDefectsResults()
        {
            try
            {
                defectsTestResultsData = new DefectsTestResults();
                defectsTestResultsData.UserExecuted = string.IsNullOrWhiteSpace(enggUserName) ? Environment.UserName.ToUpper() : enggUserName.ToUpper();
                defectsTestResultsData.PRNUDefects = pRNUDefectsCount;                
                defectsTestResultsData.TrapDefects = trapDefectsCount;
                defectsTestResultsData.HotPixelDefects = pixDefectsCount;
                defectsTestResultsData.ClusterDefects = totalClusters;
                defectsTestResultsData.DeadPixelDefects = deadPixels;
                defectsTestResultsData.DriftROICount = driftROICount;
                defectsTestResultsData.DarkPixelDefects = darkPixels;
                defectsTestResultsData.DarkColumns = darkColumns;
                defectsTestResultsData.DarkRows = darkRows;
                defectsTestResultsData.LightColumns = lightColumns;
                defectsTestResultsData.LightRows= lightRows;
                defectsTestResultsData.MeanDefects = meanDefects;
                if(defPixelROIListPairs !=null && defPixelROIListPairs.Count > 0)
                {
                    List<string> str = new List<string>();
                    foreach (KeyValuePair<string, List<Point>> roipixs in defPixelROIListPairs)
                    {
                        str.Add("ROI:" + roipixs.Key + ":" + String.Join(";", roipixs.Value.ToArray().Select(p => p.ToString()).ToArray()));
                    }
                    defectsTestResultsData.DefectivePixelsPerROICount = String.Join(";", str.ToArray().Select(p => p.ToString()).ToArray());
                }
                else
                {
                    defectsTestResultsData.DefectivePixelsPerROICount = string.Empty;
                }               
               
                defectsTestResultsData.DefPixInMaskROI = defPixInMaskROI;
                defectsTestResultsData.TestPassed = defectsTestPassed;
                defectsTestResultsData.TestResultImagePath = testResultImagePath;
                defectsTestResultsData.DateExecuted = DateTime.Now;
                defectsTestResultsDataList.Add(defectsTestResultsData);
                return defectsTestResultsData;
            }
            catch (Exception ex)
            {                
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }       
        public async Task PopulateDefectsEntity()
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
                                exposureDataList = kcc.exposureDataModel.Include("SubarrayDatas").Where(mv => mv.TestDataID.Equals("DET")).ToList();
                                List<int> exposureIDList = (from exp in exposureDataList.Where(e => e.TestDataID.Equals("DET")) select exp.ExposureID).ToList();
                                subarrayDataModelList = (from mvsu in kcc.subarrayDataModel orderby mvsu.ExposureID where exposureIDList.Contains(mvsu.ExposureID) select mvsu).ToList();
                                log.Info("Defects Test data populated from Database with Model & Subarray Records of ." + exposureDataList.Count().ToString() + subarrayDataModelList.Count.ToString());
                        }
                    });
                }
                else
                {
                    //get the data from XML files
                    await ProcessDefectsXMLFile();
                    DBExists = false;
                    log.Info("Defects Test data populated from Database with Model & Subarray Records of ." + exposureDataList.Count().ToString() + subarrayDataModelList.Count.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }      
        public async Task ProcessDefectsXMLFile()
        {
            try
            {
                await Task.Run(() =>
                {
                    log.Info("Begin Reading DefectsData XML.");                  
                    commandProcessor.ProcessExposureXMLData("DefectsData.xml");
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
        public async Task CalculateDefects(CancellationToken ct, bool serializeToBinFile)
        {
            try
            {
                await Task.Run(() =>
                {
                    log.Info("Begin Calcluate Defects for Exposure Count " + exposureCount.ToString());
                   
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
                            log.Error("Camera disconnected during Defects test " + kronostestappMainForm.cidIntegratedDataList.Count.ToString());                           
                        }
                    }
                    defectsDataList.Clear();                   
                    zIntegratedData = new double[xArraySize, yArraySize];
                    prnuDefects = 0;
                    trapDefects = 0;
                    pixDefects = 0;
                    totalClusters = 0;                    
                    PRNUTest();
                    TrapTest();
                    HotPixelTest();
                    pRNUDefectsCount = 0;
                    trapDefectsCount = 0;
                    pixDefectsCount = 0;
                    clusterDefectCount = 0;
                   
                    for (int i = 0; i < defectsDataList.Count(); i++)
                    {
                        if (defectsDataList[i].PRNUDefect)
                            pRNUDefectsCount++;
                        if (defectsDataList[i].TrapDefect)
                            trapDefectsCount++;
                        if (defectsDataList[i].HotPixelDefect)
                            pixDefectsCount++;
                        if (defectsDataList[i].clusterDefect)
                            clusterDefectCount++;
                    }
                    //todo
                    totalDefects = pRNUDefectsCount + trapDefectsCount + pixDefectsCount + clusterDefectCount;
                    if (defectsTestLimitsData == null)
                    {
                        log.Error("GetDefectsLimits()");
                        GetDefectsLimits();                        
                    }
                    if (autoPretest)
                    {
                        defectsTestLimitsData = defectsTestLimitsDataList.Where(d => d.TestStage.Equals("PRE_TEST")).FirstOrDefault();
                    }
                    else
                    {
                        defectsTestLimitsData = defectsTestLimitsDataList.Where(d => d.TestStage.Equals("FINAL_TEST")).FirstOrDefault();
                    }                  
                    int totalrows = darkRows + lightRows;
                    int totalcols = darkColumns + lightColumns;

                    if (deaddarkPixelsLimitReached ||
                        (defPixelROIListPairs !=null && defPixelROIListPairs.Count() > 0) || firmwareCorrectedMaxPixels ||
                        totalClusters > defectsTestLimitsData.TotalNumClusters ||
                        darkPixels > defectsTestLimitsData.DarkPixel ||
                        deadPixels > defectsTestLimitsData.DeadPixel ||
                        hotPixels > defectsTestLimitsData.HotPixel ||
                        totalrows > defectsTestLimitsData.TotalRows ||
                        totalcols > defectsTestLimitsData.TotalColumns ||
                        driftROICount > defectsTestLimitsData.DriftROI)
                    {
                        defectsTestPassed = false;                       
                    }
                    else
                    {
                        if (!deaddarkPixelsLimitReached && defPixelROIListPairs != null && defPixelROIListPairs.Count() <= defectsTestLimitsData.DefectivePixelsPerROI)
                        {
                            if (totalClusters <= defectsTestLimitsData.TotalNumClusters)
                            {
                                if (darkPixels <= defectsTestLimitsData.DarkPixel)
                                {
                                    if (hotPixels <= defectsTestLimitsData.HotPixel)
                                    {
                                        if (totalrows <= defectsTestLimitsData.TotalRows)
                                        {
                                            if (totalcols <= defectsTestLimitsData.TotalColumns)
                                            {
                                                if (deadPixels <= defectsTestLimitsData.DeadPixel)
                                                {
                                                    if (driftROICount <= defectsTestLimitsData.DriftROI)
                                                    {
                                                        if(!firmwareCorrectedMaxPixels)
                                                        {
                                                            defectsTestPassed = true;
                                                        }                                                       
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                       
                    }
                    if(serializeToBinFile)
                    {
                        zIntegratedData = TestAppHelper.ConvertVideoDataToDouble2D(kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList);
                        //serializeObject(serializeObjectFileName);
                    }
                 
                    log.Info("End Calcluate DarkCurrent for Exposure Count " + exposureCount.ToString());
                    kronostestappMainForm.cidDataList.Clear();
                    kronostestappMainForm.cidIntegratedDataList.Clear();
                });
            }
            catch (Exception ex)
            {
                defectsTestPassed = false;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void PRNUTest()
        {
            try
            {                
                int High_PRNU = 0;
                int Low_PRNU = 0;
                int[,] PRNUTestImageList = new int[kronostestappMainForm.cidIntegratedDataList[0].dr - 1, kronostestappMainForm.cidIntegratedDataList[0].dc - 1];
                DefectsDataStructure badPixelData = new DefectsDataStructure();
                double[] testImageList = new double[4186116];
                int testImageListIndex = 0;
                int maxDC = 0;
                int meanDC = 0;
                try
                {
                    for (int x = 1; x < kronostestappMainForm.cidIntegratedDataList[0].dr - 1; x++)
                    {
                        for (int y = 1; y < kronostestappMainForm.cidIntegratedDataList[0].dc - 1; y++)
                        {                           
                            PRNUTestImageList[x-1, y-1] = kronostestappMainForm.cidIntegratedDataList[0].videoIntegratedDataList[x, y];
                            testImageList[testImageListIndex] = PRNUTestImageList[x-1 , y-1];
                            testImageListIndex++;
                        }
                    }
                }
                catch (IndexOutOfRangeException ex)
                {
                    log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                }
                try
                {
                    maxDC = (int)testImageList.Max();
                    meanDC = (int)testImageList.Average();
                    var avg = NationalInstruments.Analysis.Math.Statistics.Mean(testImageList);
                    // +/- 10% about the average
                    High_PRNU = (int)(meanDC * 1.1) + 15000;
                    Low_PRNU = (int)(meanDC * 0.9) - 15000;
                    for (int x = 1; x < xArraySize - 1; x++)
                    {
                        for (int y = 1; y < yArraySize - 1; y++)
                        {
                            if ((PRNUTestImageList[x - 1, y - 1] < Low_PRNU) || (PRNUTestImageList[x - 1, y - 1] > High_PRNU))
                            {
                                badPixelData.xPosition = x;
                                badPixelData.yPosition = y;
                                badPixelData.value = PRNUTestImageList[x - 1, y - 1];
                                badPixelData.PRNUDefect = true;
                                if (defectsDataList.Count > 0)
                                {
                                    if ((defectsDataList[defectsDataList.Count - 1].xPosition + 1) == badPixelData.xPosition)
                                        badPixelData.clusterDefect = true;
                                }
                                defectsDataList.Add(badPixelData);
                            }
                        }
                    }
                }
                catch (IndexOutOfRangeException ex)
                {
                 log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                }
                // end of PRNU test
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void TrapTest()
        {
            try
            {
                const int Trap_Max = 40000;
                int[,] trapTestImageList = new int[xArraySize - 2, yArraySize - 2];
                DefectsDataStructure trapDefectData = new DefectsDataStructure();
                int lightColumnCount = 0;
                int darkColumnCount = 0;
                int meanIndex = 0;
                int lightRowCount = 0;
                int darkRowCount = 0;
                double sumSignal = 0;
                double pixelCount = 0;
                double percentSignalAbove =defectsTestLimitsData.PercentAboveMean / 100;
                double percentSignalBelow = defectsTestLimitsData.PercentBelowMean / 100;
                double percentSignalAboveRows = defectsTestLimitsData.PercentAboveRows / 100;
                double percentSignalBelowRows = defectsTestLimitsData.PercentBelowRows / 100;
                int averageSignalAbove = 0;
                int averageSignalBelow = 0;
                int averageSignalAboveRows = 0;
                int averageSignalBelowRows = 0;
                hotPixels = 0;
                deadPixels = 0;
                darkPixels = 0;
                meanDefects = 0;
                driftROICount = 0;
                darkRows = 0;
                lightRows = 0;
                darkColumns = 0;
                lightColumns = 0;
                clusterPixelList = new List<Point>();
                driftROIPointsList = new List<Point>();
                Point defectivePixelCluster;
                var MinContrast = 650000;
                var MaxContrast = 0;
               // List<ExcelUtilities.MaskDataStruct> maskDataList = ExcelUtilities.GetMaskDataList();
                List<SubarrayRegion.SubarrayRegionStructure> ROIMeanList = new List<SubarrayRegion.SubarrayRegionStructure>();
                
                try
                {
                    if (enableTestData)
                    {
                        //cidAnnotationData = new CID821_Data();
                         kronostestappMainForm.cidIntegratedDataList[3] = generateDefectTestData();
                        cidAnnotationData= generateDefectTestData();
                    }
                    else
                    {
                        cidAnnotationData = kronostestappMainForm.cidIntegratedDataList[3];
                    }
                   
                    for (int col = 2; col < kronostestappMainForm.cidIntegratedDataList[3].dc - 2; col++)
                    {
                        for (int row = 2; row < kronostestappMainForm.cidIntegratedDataList[3].dr - 2; row++)
                        {
                            if (validROIList[col, row])
                            { 
                                if (kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[col, row] > 0)
                                {
                                    // Determine bad columns
                                    sumSignal = sumSignal + kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[col, row];
                                    pixelCount++;
                                }                             
                               
                            }
                               
                        }
                    }

                    foreach (MaskDataStruct maskDataStruct in maskDataList)
                    {
                        int mean = 0;
                        int numberOfPoints = 0;
                        SubarrayRegion.SubarrayRegionStructure subarrayRegion =
                            new SubarrayRegion.SubarrayRegionStructure();

                        for (int x = maskDataStruct.Xo; x < maskDataStruct.Xo + maskDataStruct.dX; x++)
                        {
                            for (int y = maskDataStruct.Yo; y < maskDataStruct.Yo + maskDataStruct.dY; y++)
                            {

                                mean = mean + kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y];
                                numberOfPoints++;
                            }
                        }
                        mean = mean / numberOfPoints;
                        subarrayRegion.StartX = maskDataStruct.Xo;
                        subarrayRegion.StartY = maskDataStruct.Yo;
                        subarrayRegion.EndX = maskDataStruct.dX;
                        subarrayRegion.EndY = maskDataStruct.dY;

                        // Using the SubarrayNumber to store the mean for this ROI
                        subarrayRegion.subarrayNumber = (uint)mean;
                        ROIMeanList.Add(subarrayRegion);

                    }


                    meanDefects = sumSignal / pixelCount;
                    percentAboveValue = averageSignalAbove = (int)(meanDefects + (meanDefects * percentSignalAbove));
                    percentBelowValue = averageSignalBelow = (int)(meanDefects - (meanDefects * percentSignalBelow));
                    percentAboveRowValue = averageSignalAboveRows = (int)(meanDefects + (meanDefects * percentSignalAboveRows));
                    percentBelowRowValue = averageSignalBelowRows = (int)(meanDefects - (meanDefects * percentSignalBelowRows));

                }
                catch(Exception ex)
                {
                    log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                }
                try
                {
                    for (int x = 2; x < kronostestappMainForm.cidIntegratedDataList[3].dc - 2; x++)
                    {
                        for (int y = 2; y < kronostestappMainForm.cidIntegratedDataList[3].dr - 2; y++)
                        {
                            trapTestImageList[x - 1, y - 1] = 2 * (kronostestappMainForm.cidIntegratedDataList[2].videoIntegratedDataList[x, y] -
                                kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y]);
                            // Strip one row & col
                            if (trapTestImageList[x - 1, y - 1] > Trap_Max)
                            {
                                trapDefectData.xPosition = x;
                                trapDefectData.yPosition = y;
                                trapDefectData.value = trapTestImageList[x - 1, y - 1];
                                trapDefectData.TrapDefect = true;
                                if (defectsDataList.Count > 0)
                                {
                                    if ((defectsDataList[defectsDataList.Count - 1].xPosition + 1) == trapDefectData.xPosition)
                                        trapDefectData.clusterDefect = true;
                                }
                                defectsDataList.Add(trapDefectData);
                            }
                            // Test for bad/dark Columns
                            if (((kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y]) <
                                (kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x - 1, y])) &&
                                //((kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y]) <
                                //(kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x + 1, y])) &&
                                 (kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y]) < averageSignalBelowRows)
                            {
                                darkColumnCount++;
                            }
                            // Test for bright Columns
                            if (((kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y]) >
                                (kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x - 1, y])) &&
                                ((kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y]) >
                                (kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x + 1, y])) &&
                                (kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y]) > averageSignalAboveRows)
                            {
                                lightColumnCount++;
                            }
                            if (kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y] < averageSignalBelow)
                            {
                                // Check if this defective pixel is in a cluster
                                if (((kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x + 1, y] < averageSignalBelow) ||
                                    (kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y + 1] < averageSignalBelow)))
                                {
                                    /* if (enableMask)
                                     {
                                         if (ExcelUtilities.validROIList[x, y])
                                         {
                                             defectivePixelCluster = new Point(x, y);
                                             clusterPixelList.Add(defectivePixelCluster);
                                         }
                                     }
                                     else
                                     {
                                         defectivePixelCluster = new Point(x, y);
                                         clusterPixelList.Add(defectivePixelCluster);
                                     }*/
                                    if (validROIList[x, y])
                                    {
                                        //defectivePixelCluster = new Point(x, y);
                                        //clusterPixelList.Add(defectivePixelCluster);
                                    }
                                }
                            }
                        }
                        // If there is a strong pattern > 70% of the pixels are weak, its a bad column
                        if (lightColumnCount > defectsTestLimitsData.BadPixelsInCol)
                        {
                            lightColumns++;
                        }
                        if (darkColumnCount > defectsTestLimitsData.BadPixelsInCol)
                        {
                            darkColumns++;
                        }
                        // Reset Column Count
                        lightColumnCount = 0;
                        darkColumnCount = 0;
                    }
                    // Test for bad Row 
                    for (int y = 2; y < kronostestappMainForm.cidIntegratedDataList[3].dr - 2; y++)
                    {
                        for (int x = 2; x < kronostestappMainForm.cidIntegratedDataList[3].dc - 2; x++)
                        {
                            // test for dark rows
                            if (((kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y]) <
                                (kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y - 1])) &&

                               //((kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y]) <
                               // (kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y + 1])) &&
                                (kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y]) < averageSignalBelowRows)
                            {
                                darkRowCount++;
                            }

                            // test for bright rows
                            if (((kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y]) >
                                (kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y - 1])) &&

                               ((kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y]) >
                                (kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y + 1])) &&
                                (kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y]) > averageSignalAboveRows)
                            {
                                lightRowCount++;
                            }
                        }
                        if (darkRowCount > defectsTestLimitsData.BadPixelsInRow)
                        {
                            darkRows++;
                        }
                        if (lightRowCount > defectsTestLimitsData.BadPixelsInRow)
                        {
                            lightRows++;
                        }

                        lightRowCount = 0;
                        darkRowCount = 0;
                    }
                }
                catch (IndexOutOfRangeException ex)
                {
                    log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                }
                try
                {
                    for (int x = 2; x < kronostestappMainForm.cidIntegratedDataList[3].dc - 2; x++)
                    {
                        for (int y = 2; y < kronostestappMainForm.cidIntegratedDataList[3].dr - 2; y++)
                        {
                            if (validROIList[x, y])
                            {
                                if (kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y] == 0)
                                {
                                    deadPixels++;

                                    defectivePixelCluster = new Point(x, y);
                                    clusterPixelList.Add(defectivePixelCluster);

                                    if (PointInDriftCorrection(x, y))
                                    {
                                        driftROICount++;
                                        driftROIPointsList.Add(new Point(x, y));
                                    }
                                        
                                }

                                // Calculate mean signal by ROI
                                int roiMeanSignal = (int)meanDefects;

                                foreach (SubarrayRegion.SubarrayRegionStructure ROIStruct in ROIMeanList)
                                {
                                    // Use the ROI mean list to determine the mean for this pixel
                                    if (((x >= ROIStruct.StartX) && (x < ROIStruct.EndX + ROIStruct.StartX)) &&
                                         ((y >= ROIStruct.StartY) && (y < ROIStruct.EndY + ROIStruct.StartY)))
                                    {
                                        // SubarrayNumber was set to hold the Mean in this structure
                                        roiMeanSignal = (int)ROIStruct.subarrayNumber;
                                        break;
                                    }
                                }
                                percentAboveValue = averageSignalAbove = (int)(roiMeanSignal + (roiMeanSignal * percentSignalAbove));
                                percentBelowValue = averageSignalBelow = (int)(roiMeanSignal - (roiMeanSignal * percentSignalBelow));                             

                              

                                if ((kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y] < averageSignalBelow) &&
                                   (kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y] != 0) &&
                                   (kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y + 1] > averageSignalBelow) &&
                                   (kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y - 1] > averageSignalBelow))
                                {
                                    darkPixels++;

                                    defectivePixelCluster = new Point(x, y);
                                    clusterPixelList.Add(defectivePixelCluster);

                                    if (PointInDriftCorrection(x, y))
                                    {
                                        driftROICount++;
                                        driftROIPointsList.Add(new Point(x, y));
                                    }
                                }

                                if ((kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y] > averageSignalAbove) &&
                                    (kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y + 1] < averageSignalAbove) &&
                                    (kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y - 1] < averageSignalAbove))
                                {
                                    hotPixels++;

                                    defectivePixelCluster = new Point(x, y);
                                    clusterPixelList.Add(defectivePixelCluster);

                                    if (PointInDriftCorrection(x, y))
                                    {
                                        driftROICount++;
                                        driftROIPointsList.Add(new Point(x, y));
                                    }
                                }
                            }
                        }
                    }

                    // "Black out" data if Mask is applied
                    if (enableMask)
                    {
                        for (int x = 2; x < kronostestappMainForm.cidIntegratedDataList[3].dc - 2; x++)
                        {
                            for (int y = 2; y < kronostestappMainForm.cidIntegratedDataList[3].dr - 2; y++)
                            {
                                if (!validROIList[x, y])
                                {
                                    kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList[x, y] = 0;
                                }
                            }
                        }
                    }
                }
                catch (IndexOutOfRangeException ex)
                {
                    log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                }
                zData = TestAppHelper.ConvertVideoDataToDouble2D(kronostestappMainForm.cidIntegratedDataList[3].videoIntegratedDataList);
                try
                {
                    totalClusters = 0;
                    // If clusters were found, find out how many
                    if (clusterPixelList.Count > 1)
                    {
                       // adjacentPixlesInCluster = 1;
                        Point previousPixel = new Point(0, 0);                        
                        foreach (Point pixel in clusterPixelList)
                        {
                            if (((pixel.X - previousPixel.X) < 2) && (pixel.Y - previousPixel.Y < 2))
                            {
                                adjacentPixlesInCluster++;
                            }
                            else
                            {
                                if (adjacentPixlesInCluster >= defectsTestLimitsData.ClusterSize)
                                {
                                    totalClusters++;
                                }
                                adjacentPixlesInCluster = 1;
                            }
                            previousPixel = pixel;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                }
                // end of Trap test
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void HotPixelTest()
        {
            try
            {
                 // Hot Pixel test
                const int Hot_Pixel_Max = 40000;
                    int[,] hotPixTestImageList = new int[xArraySize - 2, yArraySize - 2];
                    DefectsDataStructure hotPixDefectData = new DefectsDataStructure();
                    for (int x = 1; x < xArraySize - 1; x++)
                    {
                        for (int y = 1; y < yArraySize - 1; y++)
                        {
                            hotPixTestImageList[x - 1, y - 1] = kronostestappMainForm.cidIntegratedDataList[4].videoIntegratedDataList[x, y] -
                                kronostestappMainForm.cidIntegratedDataList[5].videoIntegratedDataList[x, y];
                            if (hotPixTestImageList[x - 1, y - 1] > Hot_Pixel_Max)
                            {
                                hotPixDefectData.xPosition = x;
                                hotPixDefectData.yPosition = y;
                                hotPixDefectData.value = hotPixTestImageList[x - 1, y - 1];
                                hotPixDefectData.HotPixelDefect = true;
                                if (defectsDataList.Count > 0)
                                {
                                    if ((defectsDataList[defectsDataList.Count - 1].xPosition + 1) == hotPixDefectData.xPosition)
                                        hotPixDefectData.clusterDefect = true;
                                }
                                defectsDataList.Add(hotPixDefectData);
                            }
                        }
                    }
                    // end of Hot Pixel test
            }
            catch (Exception ex)
            {              
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public bool PointInDriftCorrection(int x, int y)
        {
            if (((x > 553) && (x < (553 + 73))) &&
                 ((y > 638) && (y < (638 + 85))))
                return true;

            if (((x > 1101) && (x < (1101 + 73))) &&
               ((y > 1254) && (y < (1254 + 85))))
                return true;

            if (((x > 711) && (x < (711 + 73))) &&
               ((y > 1722) && (y < (1722 + 85))))
                return true;

            if (((x > 1661) && (x < (1661 + 73))) &&
               ((y > 634) && (y < (634 + 85))))
                return true;

            if (((x > 1494) && (x < (1494 + 73))) &&
               ((y > 1717) && (y < (1717 + 85))))
                return true;


            return false;
        }
        public async Task GetDefectsLimits()
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
                            defectsTestLimitsDataList = (from limits in kcc.defectsTestLimitsData  select limits).ToList();
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
        public async Task GetROIMaskData()
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
                            defectsTestLimitsDataList = (from limits in kcc.defectsTestLimitsData select limits).ToList();
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
        private CID821_Data generateDefectTestData()
        {
            CID821_Data cidData = new CID821_Data();
            cidData.videoIntegratedDataList = new int[2048, 2048];
            cidData.subarrayTimeStamp = 1000;
            cidData.dc = 2048;
            cidData.dr = 2048;
            cidData.row = 0;
            cidData.column = 0;

            Random randomSignal = new Random();
            for (Int32 indexX = 0; indexX < 2047; indexX++)
            {
                for (Int32 indexY = 0; indexY < 2047; indexY++)
                {
                    cidData.videoIntegratedDataList[indexX, indexY] = randomSignal.Next(36000, 38000);

                    // Add one bad column or row
                    if (indexX == 1000)
                        cidData.videoIntegratedDataList[indexX, indexY] = 32000;
                }
            }

            // Now add defects
            // 3 dead pixels, one in the Drift Correction ROI
            cidData.videoIntegratedDataList[405, 318] = 0;
            cidData.videoIntegratedDataList[488, 345] = 0;
            cidData.videoIntegratedDataList[865, 273] = 0;

            // 4 hot pixels
            cidData.videoIntegratedDataList[798, 491] = 50000;
            cidData.videoIntegratedDataList[958, 1484] = 50000;
            cidData.videoIntegratedDataList[1154, 1526] = 50000;
            cidData.videoIntegratedDataList[889, 1659] = 50000;

            // 5 dark pixels
            cidData.videoIntegratedDataList[540, 620] = 10000; // Drift Correction ROI
            cidData.videoIntegratedDataList[1676, 348] = 10000;
            cidData.videoIntegratedDataList[1771, 291] = 10000;
            cidData.videoIntegratedDataList[1823, 221] = 10000;
            cidData.videoIntegratedDataList[1995, 67] = 10000;

            return cidData;
        }
        public void buildROIMask()
        {
            try
            {               
                if (TestAppHelper.CheckDBExists())
                {
                    using (var kcc = new KronosCamContext())
                    {
                        maskROIsModelList = (from ROIS in kcc.maskROIsModel select ROIS).ToList();
                    }
                    if (maskROIsModelList.Count > 0)
                    {
                        maskDataList = new List<MaskDataStruct>(maskROIsModelList.Count);
                        MaskDataStruct maskData = new MaskDataStruct();
                        foreach (MaskROIsModel maskROIsModel in maskROIsModelList)
                        {
                            maskData.Xo = maskROIsModel.X0;
                            maskData.Yo = maskROIsModel.Y0;
                            maskData.dX = maskROIsModel.dX;
                            maskData.dY = maskROIsModel.dY;                                                    
                            maskData.Ordnung = maskROIsModel.Ordnung;
                            maskData.IsDriftROI = maskROIsModel.IsDriftROI;
                            maskData.Wavelength = maskROIsModel.Lambda;
                            var element = maskROIsModel.Element;
                            maskData.name = element + "_" + maskROIsModel.Lambda.ToString();
                            maskDataList.Add(maskData);
                        }
                        if (maskDataList.Count > 0)
                        {
                            var subarrayIndex = 0;
                            foreach (MaskDataStruct maskDataStruct in maskDataList)
                            {
                                subarrayIndex++;
                                for (int x = maskDataStruct.Xo; x < maskDataStruct.Xo + maskDataStruct.dX; x++)
                                {
                                    for (int y = maskDataStruct.Yo; y < maskDataStruct.Yo + maskDataStruct.dY; y++)
                                    {
                                        validROIList[x, y] = true;
                                    }
                                }
                            }
                        }
                    }
                }              

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public Point GetPointFromString(string defectivePixelString)
        {
            Point defectivePixel = new Point(0, 0);
            //if ((defectivePixelString.Contains(" DEAD,")) || (defectivePixelString.Contains(" DARK,")))
            {
                string[] split = defectivePixelString.Split(',');
                defectivePixel.X = Convert.ToInt32(split[2]);
                defectivePixel.Y = Convert.ToInt32(split[3]);
            }
            if(defectivePixelString.Contains("Total pixels reached the limit"))
            {
                firmwareCorrectedMaxPixels = true;
            }

            return defectivePixel;
        }

        public async Task serializeObject()
        {
            try
            {
                await Task.Run(() =>
                {
                    if (File.Exists(serializeObjectFileName))
                        File.Delete(serializeObjectFileName);
                    FileStream fileStream;
                    BinaryWriter bianaryWriter;
                    fileStream = new FileStream(serializeObjectFileName, FileMode.Create);
                    bianaryWriter = new BinaryWriter(fileStream);
                    for (int y = 0; y < 2048; y++)
                    {
                        for (int x = 0; x < 2048; x++)
                        {
                            bianaryWriter.Write((ushort)zIntegratedData[x, y]);
                        }
                    }
                    Int32 endOfFile = 123456789;
                    bianaryWriter.Write(endOfFile);
                    fileStream.Flush(false);
                    bianaryWriter.Close();
                    fileStream.Close();
                });               

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
