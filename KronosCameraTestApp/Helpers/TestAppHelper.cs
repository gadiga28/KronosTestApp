using KronosCameraTestApp.Model;
using KronosCameraTestApp.Properties;
using KronosCameraTestApp.View;
using NationalInstruments.UI.WindowsForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Azure;
using Infragistics.Win.Misc;
using Renci.SshNet;

namespace KronosCameraTestApp.Helpers
{
    public static class TestAppHelper
    {
        public static bool IsFirmwareLogLevelWarning = false;
       private const int BLACK_LEVEL = 0;
       private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
         System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

       public static KronosTestAppMainForm kronosTestAppMainForm { get; set; }
        public static List<BurnInDMesgIgnoreModel> burnInDMesgIgnoreModelList { get; set; }
       public static ECOData eCOData { get; set; }
       //public static ECOData ECOData
       //{
       //    get { return eCOData; }
       //    set { eCOData = value; }
       //}
       static List<ECOData> ecoDataList { get; set; }

       public static List<ECOData> ECODataList
       {
           get { return ecoDataList; }
           set { ecoDataList = value; }
       }
       static ParametersModel parametersModel { get; set; }
       public static ParametersModel ParametersModel
       {
           get
           {
               return parametersModel;
           }
           set
           {
               parametersModel = value;
           }
       }
        public static bool UserModeChanged = false;
        public static InstrumentDefaultsModel instrumentDefaults { get; set; }
       public static MeanVarianceTestLimitsData meanVarianceTestLimitsData { get; set; }
       public static ChargeTransferLossesTestLimitsData chargeTransferLossesTestLimitsData { get; set; }
       public static CrossTalkLimitsData crossTalkLimitsData { get; set; }
       public static DarkCurrentTestLimitsData darkCurrentTestLimitsData { get; set; }
       public static DefectsTestLimitsData defectsTestLimitsData { get; set; }
       public static InjectionEfficiencyTestLimitsData injectionEfficiencyTestLimitsData { get; set; }
       public static InjectionPerformanceLimitsData injectionPerformanceLimitsData { get; set; }
       public static LinearityTestLimitsData linearityTestLimitsData { get; set; }
       public static NdroReadDriftTestLimitsData ndroReadDriftTestLimitsData { get; set; }
       public static PhotoresponseLimitsData photoresponseLimitsData { get; set; }
       public static NoiseVsNDROSLimitsData noiseVsNDROSLimitsData { get; set; }
       public static SegmentInjectLimitsData segmentInjectLimitsData { get; set; }
        public static LEDCalibrationLimitsData ledCalibrationLimitsData { get; set; }
        public static TemperatureHumidityLimitsData temperatureHumidityLimitsData { get; set; }

        public static double[] NumberOfNDRORange = new double[] { 1, 2, 4, 8, 16, 32, 64, 128, 256 };
       public static string SelectedECONumber = string.Empty;
       public static string  UserLoginName = string.Empty;

        public static bool repeatPhotoResponseExposure = false;

        //Final test report values
        public static int TotalNumDefects = 15;
       public static int TotalNumClusters = 5;
       public static double ConversionFactorNominal = 5.75;
        public static double ConversionFactorTolerance = 4;
        public static double ConversionFactorLowerLimit = 5.50;
        public static double ConversionFactorUpperLimit = 6.00;
        public static int AveTrap = 1150;
       public static double MaxDarkROI = 1.80;
       public static double MaxDarkROILowerLimit = 1.30;
       public static double MaxDarkROIUpperLimit = 2.30;
        public static double MaxDarkROITolerance = 28;
        public static int FullWellLevelMin = 430000;
       public static int SnglNoiseMax = 11;
       public static double SnglNoiseLowerLimit = 10;
       public static double SnglNoiseUpperLimit = 12;
        public static double SnglNoiseTolerance = 9;
        public static double PowerFitUpper = -0.440;
       public static double PowerFitLower = -0.560;
       public static double RPower2Correlation = 0.975;
       public static int AveSigAfterInjectionMax = 40000;
       public static int StdDevOfAvesMax = 200;
       public static double ColumnCoeffMax = -0.00002;
       public static double RowCoeffMax = -0.000019;
       public static int ColXTalkPositiveMax = 500;
       public static int RowXTalkPositiveMax = 500;
       public static int SILossesMax = -8000000;
       public static int SkimLossesMax = -13500000;
       public static int ReadLossesMax = -80000;

       //Final test report results

       //public static int TotalNumDefectsResult = 0;
       //public static int TotalNumClustersResult = 0;
       //public static double ConversionFactorNominalResult = 0;
       //public static int AveTrapResult = 0;
       //public static double MaxDarkROIResult = 0;
       //public static int FullWellLevelMinResult = 0;
       //public static int SnglNoiseMaxResult = 0;
       //public static double PowerFitUpperResult = 0;
       //public static double PowerFitLowerResult = 0;
       //public static double RPower2CorrelationResult = 0;
       //public static int AveSigAfterInjectionMaxResult = 0;
       //public static int StdDevOfAvesMaxResult = 0;
       //public static double ColumnCoeffMaxResult = 0;
       //public static double RowCoeffMaxResult = 0;
       //public static int ColXTalkPositiveMaxResult = 0;
       //public static int RowXTalkPositiveMaxResult = 0;
       //public static int SILossesMaxResult = 0;
       //public static int SkimLossesMaxResult = 0;
       //public static int ReadLossesMaxResult = 0;


       //Final Test report Pass/Fail

       //public static string TotalNumDefectsPassFail = "F";
       //public static string TotalNumClustersPassFail = "F";
       //public static string ConversionFactorNominalPassFail = "F";
       //public static string AveTrapPassFail = "F";
       //public static string MaxDarkROIPassFail = "F";
       //public static string FullWellLevelMinPassFail = "F";
       //public static string SnglNoiseMaxPassFail = "F";
       //public static string PowerFitUpperPassFail = "F";
       //public static string PowerFitLowerPassFail = "F";
       //public static string RPower2CorrelationPassFail = "F";
       //public static string AveSigAfterInjectionMaxPassFail = "F";
       //public static string StdDevOfAvesMaxPassFail = "F";
       //public static string ColumnCoeffMaxPassFail = "F";
       //public static string RowCoeffMaxPassFail = "F";
       //public static string ColXTalkPositiveMaxPassFail = "F";
       //public static string RowXTalkPositiveMaxPassFail = "F";
       //public static string SILossesMaxPassFail = "F";
       //public static string SkimLossesMaxPassFail = "F";
       //public static string ReadLossesMaxPassFail = "F";


       public static List<ParametersModel> parametersModelList { get; set; }
       public static double FinalTestReportTemperature = -44.8;
       public static double FinalTestReportHumidity = 0;

       // public static string FinalTestReportCameraModel = string.Empty;
       //public static string FinalTestReportCID = string.Empty;
       //public static string FinalTestReportBoards = string.Empty;
       //public static string FinalTestReportDetectorType = string.Empty;
       //public static string FinalTestReportTestStation = string.Empty;

       public static string currentRunningTest = string.Empty;
       // public static CloudBlobContainer cloudBlobContainer { get; set; }
        public static string AzureContainerName = string.Empty;
        public static string AzureBlobURI = string.Empty;
        public static Dictionary<string, string> AzuretestImageUrls { get; set; }
        public static List<BurnInAnalysisModel> burnInAnalysisDataList { get; set; }
        public static bool BurnInFilesExistOnCamera = false;
        public static int LEDCalibIntervalConfig = 100;
        public static int ledCalibSavedCounter = 0;
        public static int LEDCalibSavedCounter
        {
            get
            {
                return ledCalibSavedCounter;
            }
            set
            {
                ledCalibSavedCounter = value;
                //if (ledCalibSavedCounter == LEDCalibIntervalConfig)
                //{
                //    RunCalibTest();
                //}
                   
            }
        }
        public static async void RunCalibTest()
        {
            UltraDesktopAlertShowWindowInfo windowInfo1 = new UltraDesktopAlertShowWindowInfo
                                                                      ("Auto LED Calibration Test.", "Running LED Calibration Test in Auto mode.");
            if (File.Exists(Application.StartupPath.ToString() + "\\Resources\\CameraTempAlert.wav"))
            {
                windowInfo1.Sound = Application.StartupPath.ToString() + "\\Resources\\CameraTempAlert.wav";
            }
            windowInfo1.ScreenPosition = ScreenPosition.Center;
            UltraDesktopAlert ultraDesktopAlert1 = new UltraDesktopAlert();
            ultraDesktopAlert1.AutoCloseDelay = 2000;
            ultraDesktopAlert1.Show(windowInfo1);
            LEDCalibrationTest lEDCalibrationTest = new LEDCalibrationTest();
            await lEDCalibrationTest.RunLEDCalibration();
            ledCalibSavedCounter = 0;
        }
        public static double[] convertVideoDataToDouble(ushort[,] array)
       {
           int index = 0;
           int width = array.GetLength(0);
           int height = array.GetLength(1); 
           double[] doubleList = new double[width * height];
            try
            {
               
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        //doubleList[index] = array[x, y] - BLACK_LEVEL;
                        doubleList[index] = array[x, y];
                        index++;
                    }
                }
                return doubleList;
            }
           catch(Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return doubleList;
            }
           
        }
       public static double[] convertVideoDataToDouble(Int32[,] array)
       {
           int index = 0;
           int width = array.GetLength(0);
           int height = array.GetLength(1);
           double[] doubleList = new double[width * height];
           try
           {

               for (int y = 0; y < height; y++)
               {
                   for (int x = 0; x < width; x++)
                   {
                      // doubleList[index] = array[x, y] - BLACK_LEVEL;
                       doubleList[index] = array[x, y];
                       index++;
                   }
               }
               return doubleList;
           }
           catch (Exception ex)
           {
               log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
               return null;
           }

       }
       public static double calculateMeanDouble(double[] nums)
        {
            int sum = 0;
            try
            {
            if (nums.Length > 1)
            {
                // Sum up the values
                foreach (int num in nums)
                {
                    sum += num;
                }
                // Divide by the number of values
                return sum / nums.Length;
            }
            else
            {
                log.Error("Empty nums");
                return nums[0];
            }
              
            }
           catch(Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return nums[0];
            }
            
           
        }
        // Get the average of the values in an array
        public static double calculateMean(double[] nums)
        {
            int sum = 0;
            try
            {
                if (nums.Length > 1)
                {
                    // Sum up the values
                    foreach (int num in nums)
                    {
                        sum += num;
                    }

                    // Divide by the number of values
                    return sum / (double)nums.Length;
                }
                else
                {
                    log.Error("Empty nums");
                    return (double)nums[0];
                }
            }
            catch(Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return (double)nums[0];
            }
            
        }
        public static double calculateVariance(double[] nums)
        {
            try
            {
                if (nums.Length > 1)
                {
                    // Get the average of the values

                    double avg = calculateMean(nums);

                    // Now figure out how far each point is from the mean
                    // So we subtract from the number the average
                    // Then raise it to the power of 2

                    double sumOfSquares = 0.0;

                    foreach (int num in nums)
                    {
                        sumOfSquares += Math.Pow((num - avg), 2.0);
                    }

                    // Finally divide it by n - 1 (for standard deviation variance)
                    // Or use length without subtracting one ( for population standard deviation variance)
                    return (sumOfSquares / (double)(nums.Length - 1));
                }
                else 
                {
                    log.Error("Empty nums");
                    return 0.0; 
                }
            }
            catch(Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return 0.0;
            }
            
        }
        public static double[,] ConvertVideoDataToDouble2D(Int32[,] array)
        {
            try
            {
                int index = 0;
                int width = array.GetLength(0);
                int height = array.GetLength(1);
                double[,] doubleList = new double[width, height];

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        doubleList[x, y] = array[x, y];// -BLACK_LEVEL;
                        index++;
                    }
                }
                return doubleList;
            }
            catch(Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
                
            }
        }
        public static bool CheckDBExists()
        {
            using (var db = new KronosCamContext())
            {
                try
                {
                    //Ping pingSender = new Ping();
                    //PingReply reply = pingSender.Send("10.210.34.181");
                    //if (reply.Status == IPStatus.Success)
                    {
                        if (db.Database.Exists())
                        {
                            DbConnection conn = db.Database.Connection;
                            conn.Open();   // check the database connection
                            return true;
                        }
                        else
                        {
                            log.Error("Database Not found or no Connection Exists");
                            return false;
                        }
                        //return true;
                    }
                    //return false;
                }
                catch (SqlException ex)
                {
                    log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                    return false;
                }
                catch (Exception ex)
                {
                    log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                    return false;
                }               
            }
        }
        public static string ReadScriptFiles(string scriptFileName)
        {          
           string commandString = "";
           // scriptFileName = "\\MeanVariance2Subarrays.scp";
            try
            {
                string line;
                using (StreamReader sr = new StreamReader(scriptFileName))
                {
                    // Read in the entire script
                    while ((line = sr.ReadLine()) != null)
                    {
                        // Pre process the script by removing all comments
                        line = Regex.Replace(line, @"//.*$", "");
                        commandString = commandString + line;
                    }
                }
                return commandString;
            }
            catch(Exception)
            {
                return commandString;
            }
        }
        public static void ResetProgressBar()
        {
            try
            {
                if (kronosTestAppMainForm.ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value == 100)
                {
                    kronosTestAppMainForm.ultraStatusBar1.Panels["MarqueeStatus"].Text = string.Empty;
                    kronosTestAppMainForm.ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                    kronosTestAppMainForm.ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.ResetFillAppearance();
                }
            }

            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }       
        public static string GetECONumber()
        {
            try
            {
                List<ECOData> ecoDataList = new List<ECOData>();
                ECOData ecoData = new ECOData();
                if (string.IsNullOrWhiteSpace(TestAppHelper.SelectedECONumber))
                {
                    using (var kcc = new KronosCamContext())
                    {
                        ecoDataList = kcc.ecoData
                            .Include("MeanVarianceTestLimitsData")
                            .Include("DarkCurrentTestLimitsData")
                            .Include("DefectsTestLimitsData")
                            .Include("InjectionEfficiencyTestLimitsData")
                            .Include("InjectionPerformanceLimitsData")
                            .Include("NdroReadDriftTestLimitsData")
                            .Include("PhotoresponseLimitsData")
                            .Include("NoiseVsNDROSLimitsData")
                            .Include("SegmentInjectLimitsData")
                            .ToList();

                        ecoData = (from ecos in ecoDataList orderby ecos.DefinedDate descending select ecos).ToList()[0];
                    }
                    return ecoData.ECONumber;
                }
                else
                    return TestAppHelper.SelectedECONumber;              

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;

            }
        }
        public static ParametersModel GetParametersModel()
        {
            try
            {
                if (TestAppHelper.CheckDBExists())
                {
                    using (var kcc = new KronosCamContext())
                    {
                        parametersModel = new ParametersModel();
                        parametersModel = (from parameters in kcc.parametersModel where parameters.ParameterID == 1 orderby parameters.DateModified descending select parameters).FirstOrDefault();
                        if (parametersModel != null)
                        {
                            return parametersModel;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    //ProcessParametersXMLFile();
                    //parametersModel = new ParametersModel();
                    //parametersModel = parametersModelList.Where(pm => pm.ParameterName.Equals("TestResultsServerPath")).FirstOrDefault();
                   // return null;
                }
                Settings.Default.AutoTestResultsServerFilePathFromDB = parametersModel.TestResultsServerPath.ToString();
                return parametersModel; 
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }
        public static void ProcessParametersXMLFile()
        {
            try
            {
                log.Info("Begin Reading UserLoginData XML.");

                DataSet ds = new DataSet();
                ds.ReadXml(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "ParametersData.xml", XmlReadMode.InferSchema);

                DataView dvExposure;
                dvExposure = ds.Tables[0].DefaultView;

                parametersModelList = new List<ParametersModel>();
                foreach (DataRowView dr in dvExposure)
                {
                    parametersModel = new ParametersModel();
                    parametersModel.ParameterID = Convert.ToInt32(dr[0]);
                  //  parametersModel.ParameterName = Convert.ToString(dr[1]);
                    //parametersModel.ParameterValue = Convert.ToString(dr[2]);
                    parametersModel.DateModified = Convert.ToDateTime(dr[3]);
                    parametersModel.UserModified = Convert.ToString(dr[4]);
                    parametersModelList.Add(parametersModel);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public static bool DisplayRangeMessage(object sender, double value)
        {
            try
            {
                NumericEdit editor = (NumericEdit)sender;
                string errstring = string.Empty;
                double oldValue = editor.Value;
                if(editor.Name.Equals("numericEditNDRO") && !NumberOfNDRORange.Contains(value))// || editor.Name.Equals("numericEditSubarrayNDROs"))
                {
                    errstring = string.Format("Number of NDROs must be 1, 2, 4, 8, 16, 32, 64, 128 or 256");
                    MessageBox.Show(errstring, "Invalid Range", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    log.Error(errstring);
                    editor.Value = oldValue;
                    return false;
                }
                if (!editor.Range.Contains(value))
                {
                    switch (editor.Name)
                    {
                        case "numericEditExposureInterval":
                            errstring = string.Format("Exposure Interval value should be between '{0}' and '{1}'.", 40, 4260000);
                            break;
                        case "numericEditLEDOn":
                            errstring = string.Format("LED On Time value should be between '{0}' and '{1}'.", 0, 31456);                            
                            break;
                        case "numericEditLEDOff":
                            errstring = string.Format("LED Off Time value should be between '{0}' and '{1}'.", 0, 31456);
                            break;
                        case "numericEditFlash":
                            errstring = string.Format("LED Flahes value should be between '{0}' and '{1}'.", 0, 65535);
                            break;
                        case "numericEditExposureStartX":
                            errstring = string.Format("Exposure Region Start X minimum value should be between '{0}'.", 0);
                            break;
                        case "numericEditExposureStartY":
                            errstring = string.Format("Exposure Region Start X minimum value should be between '{0}'.", 0);
                            break;
                        case "numericEditExposureWidth":
                            errstring = string.Format("Exposure Region Width value should be between '{0}' and '{1}'.", 4, 2048);
                            break;
                        case "numericEditExposureHeight":
                            errstring = string.Format("Exposure Region Height value should be between '{0}' and '{1}'.", 4, 2048);
                            break;
                        case "numericEditThresholdPercent":
                            if (!editor.Range.Contains(value))
                                errstring = string.Format("Subarray Thershold percentage value should be between '{0}' and '{1}'.", 0, 100);
                            break;
                        case "numericEditSubarrayStartX":
                            errstring = string.Format("Subarray Region Start X minimum value should be '{0}'.", 0);
                            break;
                        case "numericEditSubarrayStartY":
                            errstring = string.Format("Subarray Region Start X minimum value should be '{0}'.", 0);
                            break;
                        case "numericEditSubarrayWidth":
                            errstring = string.Format("Subarray Region Width value should be between '{0}' and '{1}'.", 4, 2048);
                            break;
                        case "numericEditSubarrayHeight":
                            errstring = string.Format("Subarray Region Height value should be between '{0}' and '{1}'.", 4, 2048);
                            break;
                        case "numericEditSubarrayThresholdStartX":
                            errstring = string.Format("Subarray Threshold Region Start X minimum value should be '{0}'.", 0);
                            break;
                        case "numericEditSubarrayThresholdStartY":
                            errstring = string.Format("Subarray Threshold Region Start X minimum value should be '{0}'.", 0);
                            break;
                        case "numericEditSubarrayThresholdWidth":
                            errstring = string.Format("Subarray Threshold Region Width value should be between '{0}' and '{1}'.", 4, 2048);
                            break;
                        case "numericEditSubarrayThresholdHeight":
                            errstring = string.Format("Subarray Threshold Region Height value should be between '{0}' and '{1}'.", 4, 2048);
                            break;
                    }
                    MessageBox.Show(errstring, "Invalid Range", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    editor.Value = oldValue;
                    log.Error(errstring);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
        public static bool checkSubarrayCountForExposure(int subarrayPerExposureCount)
        {
            try
            {
                if (subarrayPerExposureCount == 600)
                {
                    MessageBox.Show("Subarray Count per Exposure cannot be more than 600", "Add Subarray", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
        public static double standardDeviation(this IEnumerable<double> values)
        {
            double avg = values.Average();
            return Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
        }
        public static void saveFileInAzure(Image testResultImage, string testResultImageName)
        {
            try
            {
                //string storageConnection = CloudConfigurationManager.GetSetting("StorageConnectionString");
                //CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConnection);

                ////create a block blob 
                //CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

                ////create a container 
                //cloudBlobContainer = cloudBlobClient.GetContainerReference(AzureContainerName);

                ////create a container if it is not already exists

                //if ( cloudBlobContainer.CreateIfNotExists())
                //{
                //    cloudBlobContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                //}               

                ////get Blob reference

                //CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(testResultImageName);
                //cloudBlockBlob.Properties.ContentType = "image/png";

                //byte[] blobtoSave = ImageToByte(testResultImage);

                //cloudBlockBlob.UploadFromByteArray(blobtoSave, 0, blobtoSave.Length);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                AzureBlobURI= string.Empty;
            }
        }
        public static byte[] ImageToByte(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        public static List<BurnInAnalysisModel> GetBurnInAnalysisData()
        {
            try
            {
                burnInAnalysisDataList = new List<BurnInAnalysisModel>();

                using (var kcc = new KronosCamContext())
                {
                    burnInAnalysisDataList = (from burnInResults in kcc.burnInAnalysisModel
                                              orderby burnInResults.DateExecuted descending
                                              select burnInResults).ToList();
                }
                return burnInAnalysisDataList;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }
        public static async Task<List<CameraTestLogsModel>> GetCameraTestMetrics(string CameraSerialNumber)
        {
            try
            {
                var res = await Task.Run(() =>
                {
                    List<CameraDetailsModel> cameraDetailsModel = new List<CameraDetailsModel>();
                    List<CameraTestLogsModel> cameraTestLogs = new List<CameraTestLogsModel>();
                    using (var kcc = new KronosCamContext())
                    {
                        cameraDetailsModel = (from cameras in kcc.cameraDetailsModel
                                              where cameras.CameraSerialNumber.Equals(CameraSerialNumber)
                                              select cameras).ToList();
                        foreach (CameraDetailsModel camera in cameraDetailsModel)
                        {
                            CameraTestLogsModel cameraTestLog = new CameraTestLogsModel();
                            cameraTestLog = (from cameraLog in kcc.cameraTestLogs
                                             where cameraLog.CameraTestID.Equals(camera.CameraTestID)
                                             select cameraLog).FirstOrDefault();
                            if (cameraTestLog != null)
                            {
                                cameraTestLogs.Add(cameraTestLog);
                            }
                        }
                        return cameraTestLogs;
                    }
                    
                });
                return res;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }
        public static void noHeartBeatFromCamera()
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Communication to camera/firmware is lost. Test will be aborted. Please check for camera/firmware connection.",
                                                         "Test Status", MessageBoxButtons.OK, MessageBoxIcon.Stop);
               
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);

            }
        }
        public static string GetFirmwareFileName()
        {
            try
            {
                string firmwareFileName = string.Empty;

                // Upload A File using a sFTP Client
                using (var sftpClient = new SftpClient("192.168.1.2", 22, "root", "cidtec"))
                {
                    sftpClient.Connect();
                    sftpClient.ChangeDirectory("/");
                    var files = sftpClient.ListDirectory("root").Select(s => s.FullName);
                    foreach (var file in files)
                    {
                        if (Path.GetExtension(file).Equals(".exe") || Path.GetExtension(file).Equals(".EXE"))
                        {
                            firmwareFileName = file.ToString().Substring(6, file.Length - 10);
                            break;
                        }
                    }
                    sftpClient.Disconnect();
                }
                return firmwareFileName;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return string.Empty;
            }
        }
    }
   
}
