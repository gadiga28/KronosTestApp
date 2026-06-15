using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Microsoft.Office.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.Entity.Core.Objects;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using KronosCameraTestApp.Model;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using KronosCameraTestApp.View;
using System.Configuration;
namespace KronosCameraTestApp.Helpers
{
    public static class ExcelUtilities
    {
        public static Excel.Application xlApp;
        public static Workbook xlWorkBook;
        public static Worksheet xlRedBlueSheet;
        public static Worksheet xlMeanVarianceSheet;
        public static Worksheet xlDarkCurrentSheet;
        public static Worksheet xlDefectsSheet;
        public static Worksheet xlNoiseVsNDROSheet;
        public static Worksheet xlPhotoresponseSheet;
        public static Worksheet xlInjectionPerformanceSheet;
        public static Worksheet xlShutterDriveSheet;
        public static Worksheet xlBurnInDetailsSheet;
        public static Worksheet xlWorkSheet;
        //public static bool[,] validROIList = new bool[2048, 2048];
        //public struct MaskDataStruct
        //{
        //    public int Xo;
        //    public int Yo;
        //    public int dX;
        //    public int dY;
        //    public String name;
        //}
        //public static List<MaskDataStruct> maskDataList;
        // The once-per-class call to initialize the log object
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static string filepath = string.Empty;
        public static string excelFilePath = string.Empty;
        public static string resultslocalPath = string.Empty;
        public static string fileDatestamp = string.Empty;
        public static string FilePath
        {
            get { return filepath; }
            set { filepath = value; }
        }       
        static object misValue = System.Reflection.Missing.Value;
        static Sheets excelSheets { get; set; }
        static Worksheet excelWorksheet { get; set; }
        public static Excel.Application xlAppForTempLog;
        public static Workbook xlWorkBookForTempLog;
        public static Worksheet xlWorkSheetForTempLog;
        static Sheets excelSheetsforTempLog { get; set; }
        public static void WriteTestResultsToExcel(string workSheetName, object TestData)
        {
            try
            {
                    log.Info("Begin Write Test Results To Excel");
                    if (!Directory.Exists(filepath))
                    {
                        System.IO.Directory.CreateDirectory(filepath);
                        excelFilePath = filepath + "\\TestResults_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm") + ".xlsx";                       
                    }
                    else
                    {
                        if(!File.Exists(excelFilePath))
                             excelFilePath = filepath + "\\TestResults_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm") + ".xlsx";   
                    }
                    if (excelFilePath == "" || excelFilePath == null)
                    {
                        excelFilePath = path + @"\TestResults_" + DateTime.Now.ToString("MM_dd_yyyy") + ".xlsx";
                    }                   
                    log.Info("End Write Test Results To Excel");
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public static async Task WriteTestResultsToExcel(string testResultslocalPath, string testResultsServerPath,string filetimestamp,
                                                          MeanVarianceTestResultsData meanVarianceTestResultsData, DarkCurrentTestResults darkCurrentTestResults,
                                                          DefectsTestResults defectsTestResults, 
                                                          NoiseVsNDROTestResults noiseVsNDROTestResults,PhotoresponseTestResults photoresponseTestResults,
                                                          InjectionEfficiencyTestResults injectionEfficiencyTestResults, RedBlueTestResults redBlueTestResults, 
                                                          ShutterDriveTestResults shutterDriveTestResults, BurnInAnalysisModel cameraBurnInDetails, string[] selectedTests)
        {
            try
            {
                await Task.Run(()=>
                    {
                        log.Info("Begin Write Test Results To Excel When File Not Exists");
                        xlApp = new Microsoft.Office.Interop.Excel.Application();
                        xlApp.DisplayAlerts = false;
                        xlWorkBook = xlApp.Workbooks.Add(misValue);
                        excelSheets = xlWorkBook.Worksheets;
                        InitializeColumnHeaders(selectedTests, cameraBurnInDetails);
                        excelWorksheet = null;
                        resultslocalPath = testResultslocalPath;
                        fileDatestamp = filetimestamp;
                        foreach (string str in selectedTests)
                        {
                            switch (str)
                            {                              
                                case "RBT":
                                    if (redBlueTestResults!=null)
                                    {
                                        UpdateRedBlueWorkSheet(redBlueTestResults);
                                    }
                                    else
                                        xlRedBlueSheet.Cells[2, 1] = "Test Failed";                                    
                                    break;
                                case "MVT":
                                    if (meanVarianceTestResultsData!=null)
                                    {
                                        UpdateMeanVarianceWorkSheet(meanVarianceTestResultsData);
                                    }
                                    else
                                        xlMeanVarianceSheet.Cells[2, 1] = "Test Failed";
                                    break;
                                case "SDT":
                                    if (shutterDriveTestResults!=null)
                                    {
                                        UpdateShutterDriveWorkSheet(shutterDriveTestResults);
                                    }
                                    else
                                        xlShutterDriveSheet.Cells[2, 1] = "Test Failed";
                                    break;
                                case "DDT":
                                    if (darkCurrentTestResults != null)
                                    {
                                        UpdateDarkCurrentWorkSheet(darkCurrentTestResults);
                                    }
                                    else
                                        xlDarkCurrentSheet.Cells[2, 1] = "Test Failed";
                                    break;
                                case "DET":
                                    if (defectsTestResults!=null)
                                    {
                                        UpdateDefectsWorkSheet(defectsTestResults);                                       
                                    }
                                    else
                                        xlDarkCurrentSheet.Cells[2, 1] = "Test Failed";
                                    break;
                                case "RNT":
                                    if (noiseVsNDROTestResults != null)
                                    {
                                        UpdateReadNoiseWorkSheet(noiseVsNDROTestResults);                                        
                                    }
                                    else
                                        xlNoiseVsNDROSheet.Cells[2, 1] = "Test Failed";
                                    break;                              
                                case "PRT":
                                    if (photoresponseTestResults != null)
                                    {
                                        UpdatePhotoresponseWorkSheet(photoresponseTestResults);
                                    }
                                    else
                                        xlPhotoresponseSheet.Cells[2, 1] = "Test Failed";
                                    break;
                                case "IET":
                                    if (injectionEfficiencyTestResults != null)
                                    {
                                        UpdateInjectionEffWorkSheet(injectionEfficiencyTestResults);
                                    }
                                    else
                                        xlInjectionPerformanceSheet.Cells[2, 1] = "Test Failed";
                                    break;                               
                            }
                        }
                        if (cameraBurnInDetails != null)
                        {
                            UpdateBurnInWorkSheet(cameraBurnInDetails);
                        }
                        else
                            xlBurnInDetailsSheet.Cells[2, 3] = "N/A";
                        if (ConfigurationManager.AppSettings["EndClient"] == "Thermo")
                        {
                            if (!File.Exists(testResultsServerPath))
                            {
                                testResultsServerPath = testResultsServerPath + "\\TestResults_" + filetimestamp + ".xlsx";
                                xlWorkBook.SaveAs(testResultsServerPath);
                            }
                        }
                        if (!File.Exists(testResultslocalPath))
                        {
                            testResultslocalPath = testResultslocalPath + "\\TestResults_" + filetimestamp + ".xlsx";
                            xlWorkBook.SaveAs(testResultslocalPath);
                        }
                        //xlWorkBook.SaveAs(testResultslocalPath);
                        log.Info("End Write Test Results To Excel When File Not Exists");
                    });
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
            finally
            {
                CloseExcel();
            }
        }
        private static void UpdateRedBlueWorkSheet(RedBlueTestResults redBlueTestResults)
        {
            try
            {
                xlRedBlueSheet.Activate();
                xlRedBlueSheet.Cells[2, 1] = redBlueTestResults.ResultsID;
                xlRedBlueSheet.Cells[2, 2] = fileDatestamp;
                xlRedBlueSheet.Cells[2, 3] = redBlueTestResults.UserExecuted;
                xlRedBlueSheet.Cells[2, 4] = redBlueTestResults.RedImageData;
                xlRedBlueSheet.Cells[2, 5] = redBlueTestResults.BlueImageData;
                xlRedBlueSheet.Cells[2, 6] = redBlueTestResults.RedBlueImageDifference;
                xlRedBlueSheet.Cells[2, 7] = redBlueTestResults.TestPassed;
                xlRedBlueSheet.Cells[2, 8] = resultslocalPath + "\\RedBlueTestResultImage_" + fileDatestamp + ".png";
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }       
        private static void UpdateMeanVarianceWorkSheet(MeanVarianceTestResultsData meanVarianceTestResultsData)
        {
            try
            {
                xlMeanVarianceSheet.Activate();
                xlMeanVarianceSheet.Cells[2, 1] = meanVarianceTestResultsData.ResultsID;
                xlMeanVarianceSheet.Cells[2, 2] = fileDatestamp;
                xlMeanVarianceSheet.Cells[2, 3] = meanVarianceTestResultsData.UserExecuted;
                xlMeanVarianceSheet.Cells[2, 4] = meanVarianceTestResultsData.CorrectedMean;
                xlMeanVarianceSheet.Cells[2, 5] = meanVarianceTestResultsData.CorrectedVariance;
                xlMeanVarianceSheet.Cells[2, 6] = meanVarianceTestResultsData.LinearCurveFit;
                xlMeanVarianceSheet.Cells[2, 7] = meanVarianceTestResultsData.Gain;
                xlMeanVarianceSheet.Cells[2, 8] = meanVarianceTestResultsData.TestPassed;
                xlMeanVarianceSheet.Cells[2, 9] = resultslocalPath + "\\MeanVarianceTestResultImage_" + fileDatestamp + ".png";
                string[] mean = meanVarianceTestResultsData.CorrectedMean.Split(',');
                xlMeanVarianceSheet.Cells[4, 1] = "Mean - X Data";
                for (int i = 0; i < mean.Count(); i++)
                {
                    xlMeanVarianceSheet.Cells[i + 5, 1] = Convert.ToDouble(mean[i]);
                }
                string[] variance = meanVarianceTestResultsData.CorrectedVariance.Split(',');
                xlMeanVarianceSheet.Cells[4, 3] = "Variance - Y Data";
                for (int i = 0; i < variance.Count(); i++)
                {
                    xlMeanVarianceSheet.Cells[i + 5, 3] = Convert.ToDouble(variance[i]);
                }
                //xlMeanVarianceSheet.Cells[4, 1] = "Mean Variance";
                //for (int i = 0; i < mean.Count(); i++)
                //{
                //    xlMeanVarianceSheet.Cells[i + 5, 1] = string.Format("{0} , {1}",Convert.ToDouble(mean[i]), Convert.ToDouble(variance[i]));
                //}
                string[] linearCurveFit = meanVarianceTestResultsData.LinearCurveFit.Split(',');
                xlMeanVarianceSheet.Cells[4, 5] = "Linear Curve Fit - Y Data";
                for (int i = 0; i < linearCurveFit.Count(); i++)
                {
                    xlMeanVarianceSheet.Cells[i + 5, 5] = Convert.ToDouble(linearCurveFit[i]);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private static void UpdateReadNoiseWorkSheet(NoiseVsNDROTestResults noiseVsNDROTestResults)
        {
            try
            {
                xlNoiseVsNDROSheet.Activate();
                xlNoiseVsNDROSheet.Cells[2, 1] = noiseVsNDROTestResults.ResultsID;
                xlNoiseVsNDROSheet.Cells[2, 2] = fileDatestamp;
                xlNoiseVsNDROSheet.Cells[2, 3] = noiseVsNDROTestResults.UserExecuted;
                xlNoiseVsNDROSheet.Cells[2, 4] = noiseVsNDROTestResults.ExposureNDRO;
                xlNoiseVsNDROSheet.Cells[2, 5] = noiseVsNDROTestResults.NoiseRatio;
                xlNoiseVsNDROSheet.Cells[2, 6] = noiseVsNDROTestResults.Noise;
                xlNoiseVsNDROSheet.Cells[2, 7] = noiseVsNDROTestResults.R2Correlation;
                xlNoiseVsNDROSheet.Cells[2, 8] = noiseVsNDROTestResults.BestFit;
                xlNoiseVsNDROSheet.Cells[2, 9] = noiseVsNDROTestResults.BestFitPower;
                xlNoiseVsNDROSheet.Cells[2, 10] = noiseVsNDROTestResults.TheoryValue;
                xlNoiseVsNDROSheet.Cells[2, 11] = noiseVsNDROTestResults.SignalReadNoise;
                xlNoiseVsNDROSheet.Cells[2, 12] = noiseVsNDROTestResults.NDRONoise;
                xlNoiseVsNDROSheet.Cells[2, 13] = noiseVsNDROTestResults.TestPassed;
                xlNoiseVsNDROSheet.Cells[2, 14] = resultslocalPath + "\\NoiseVsNDROsTestResultImage_" + fileDatestamp + ".png"; //noiseVsNDROTestResults.TestResultImagePath; 
                string[] exposureNDRO = noiseVsNDROTestResults.ExposureNDRO.Split(',');
                xlNoiseVsNDROSheet.Cells[4, 1] = "Exposure NDROs - X Data";
                for (int i = 0; i < exposureNDRO.Count(); i++)
                {
                    xlNoiseVsNDROSheet.Cells[i + 5, 1] = Convert.ToDouble(exposureNDRO[i]);
                }
                //string[] noiseRatio = noiseVsNDROTestResults.NoiseRatio.Split(',');
                //xlNoiseVsNDROSheet.Cells[4, 3] = "Noise Ratio";
                //for (int i = 0; i < noiseRatio.Count(); i++)
                //{
                //    xlNoiseVsNDROSheet.Cells[i + 5, 3] = Convert.ToDouble(noiseRatio[i]);
                //}
                string[] noiseres = noiseVsNDROTestResults.Noise.Split(',');
                xlNoiseVsNDROSheet.Cells[4, 3] = "Noise - Y Data";
                for (int i = 0; i < noiseres.Count(); i++)
                {
                    xlNoiseVsNDROSheet.Cells[i + 5, 3] = Convert.ToDouble(noiseres[i]);
                }
                string[] theoryValue = noiseVsNDROTestResults.TheoryValue.Split(',');
                xlNoiseVsNDROSheet.Cells[4, 5] = "Theory Value - Y Data";
                for (int i = 0; i < theoryValue.Count(); i++)
                {
                    xlNoiseVsNDROSheet.Cells[i + 5, 5] = Convert.ToDouble(theoryValue[i]);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private static void UpdateInjectionEffWorkSheet(InjectionEfficiencyTestResults injectionEfficiencyTestResults)
        {
            try
            {
                //injectionPerformanceTestResults = (InjectionPerformanceTestResults)TestData;
                xlInjectionPerformanceSheet.Cells[2, 1] = injectionEfficiencyTestResults.ResultsID;
                xlInjectionPerformanceSheet.Cells[2, 2] = fileDatestamp;
                xlInjectionPerformanceSheet.Cells[2, 3] = injectionEfficiencyTestResults.UserExecuted;
                xlInjectionPerformanceSheet.Cells[2, 4] = injectionEfficiencyTestResults.InitialExposure;
                xlInjectionPerformanceSheet.Cells[2, 5] = injectionEfficiencyTestResults.FirstInjection;
                xlInjectionPerformanceSheet.Cells[2, 6] = injectionEfficiencyTestResults.SecondInjection;
                xlInjectionPerformanceSheet.Cells[2, 7] = injectionEfficiencyTestResults.ThirdInjection;
                xlInjectionPerformanceSheet.Cells[2, 8] = injectionEfficiencyTestResults.FourthInjection;
                xlInjectionPerformanceSheet.Cells[2, 9] = injectionEfficiencyTestResults.LastInjection;
                xlInjectionPerformanceSheet.Cells[2, 10] = injectionEfficiencyTestResults.Exp0PlotData;
                xlInjectionPerformanceSheet.Cells[2, 11] = injectionEfficiencyTestResults.Exp1PlotData;
                xlInjectionPerformanceSheet.Cells[2, 12] = injectionEfficiencyTestResults.Exp2PlotData;
                xlInjectionPerformanceSheet.Cells[2, 13] = injectionEfficiencyTestResults.Exp3PlotData;
                xlInjectionPerformanceSheet.Cells[2, 14] = injectionEfficiencyTestResults.Exp4PlotData;
                xlInjectionPerformanceSheet.Cells[2, 15] = injectionEfficiencyTestResults.TestPassed;
                xlInjectionPerformanceSheet.Cells[2, 16] = resultslocalPath + "\\InjectionEfficiencyTestResultImage_" + fileDatestamp + ".png";
                string[] exp0Plot = injectionEfficiencyTestResults.Exp0PlotData.Split(',');
                xlInjectionPerformanceSheet.Cells[4, 1] = "Exp0 Plot";
                for (int i = 0; i < exp0Plot.Count(); i++)
                {
                    xlInjectionPerformanceSheet.Cells[i + 5, 1] = Convert.ToDouble(exp0Plot[i]);
                }
                string[] exp1Plot = injectionEfficiencyTestResults.Exp1PlotData.Split(',');
                xlInjectionPerformanceSheet.Cells[4, 3] = "Exp1 Plot";
                for (int i = 0; i < exp1Plot.Count(); i++)
                {
                    xlInjectionPerformanceSheet.Cells[i + 5, 3] = Convert.ToDouble(exp1Plot[i]);
                }
                string[] exp2Plot = injectionEfficiencyTestResults.Exp3PlotData.Split(',');
                xlInjectionPerformanceSheet.Cells[4, 5] = "Exp2 Plot";
                for (int i = 0; i < exp2Plot.Count(); i++)
                {
                    xlInjectionPerformanceSheet.Cells[i + 5, 5] = Convert.ToDouble(exp2Plot[i]);
                }
                string[] exp3Plot = injectionEfficiencyTestResults.Exp3PlotData.Split(',');
                xlInjectionPerformanceSheet.Cells[4, 7] = "Exp3 Plot";
                for (int i = 0; i < exp3Plot.Count(); i++)
                {
                    xlInjectionPerformanceSheet.Cells[i + 5, 7] = Convert.ToDouble(exp3Plot[i]);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private static void UpdatePhotoresponseWorkSheet(PhotoresponseTestResults photoresponseTestResults)
        {
            try
            {
                xlPhotoresponseSheet.Activate();
                xlPhotoresponseSheet.Cells[2, 1] = photoresponseTestResults.ResultsID;
                xlPhotoresponseSheet.Cells[2, 2] = fileDatestamp;
                xlPhotoresponseSheet.Cells[2, 3] = photoresponseTestResults.UserExecuted;
                xlPhotoresponseSheet.Cells[2, 4] = photoresponseTestResults.Mean;
                xlPhotoresponseSheet.Cells[2, 5] = photoresponseTestResults.LinearCurveFit;
                xlPhotoresponseSheet.Cells[2, 6] = photoresponseTestResults.LinearFitData;
                xlPhotoresponseSheet.Cells[2, 7] = photoresponseTestResults.FullSaturation;
                xlPhotoresponseSheet.Cells[2, 8] = photoresponseTestResults.LinearSaturation;
                xlPhotoresponseSheet.Cells[2, 9] = photoresponseTestResults.Slope;
                xlPhotoresponseSheet.Cells[2, 10] = photoresponseTestResults.Intercept;
                xlPhotoresponseSheet.Cells[2, 11] = photoresponseTestResults.DerivPlot;
                xlPhotoresponseSheet.Cells[2, 12] = photoresponseTestResults.TestPassed;
                xlPhotoresponseSheet.Cells[2, 13] = resultslocalPath + "\\PhotoresponseTestResultImage_" + fileDatestamp + ".png";
                xlPhotoresponseSheet.Cells[4, 1] = "START_BEST_FIT";
                xlPhotoresponseSheet.Cells[5, 1] = 25;
                xlPhotoresponseSheet.Cells[4, 2] = "NUMBER_OF_BEST_FIT";
                xlPhotoresponseSheet.Cells[5, 2] = 122;
                string[] mean = photoresponseTestResults.Mean.Split(',');
                xlPhotoresponseSheet.Cells[7, 1] = "Mean - Y Data";
                for (int i = 0; i < mean.Count(); i++)
                {
                    xlPhotoresponseSheet.Cells[i + 5, 1] = Convert.ToDouble(mean[i]);
                }
                string[] linearFitData = photoresponseTestResults.LinearFitData.Split(',');
                xlPhotoresponseSheet.Cells[7, 3] = "Linear Fit - Y Data";
                for (int i = 0; i < linearFitData.Count(); i++)
                {
                    xlPhotoresponseSheet.Cells[i + 5, 3] = Convert.ToDouble(linearFitData[i]);
                }
                string[] derivPlot = photoresponseTestResults.DerivPlot.Split(',');
                xlPhotoresponseSheet.Cells[7, 5] = "Derived Plot - Y Data";
                for (int i = 0; i < derivPlot.Count(); i++)
                {
                    xlPhotoresponseSheet.Cells[i + 5, 5] = Convert.ToDouble(derivPlot[i]);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private static void UpdateDarkCurrentWorkSheet(DarkCurrentTestResults darkCurrentTestResults)
        {
            try
            {
                xlDarkCurrentSheet.Activate();
                xlDarkCurrentSheet.Cells[2, 1] = darkCurrentTestResults.ResultsID;
                xlDarkCurrentSheet.Cells[2, 2] = fileDatestamp;
                xlDarkCurrentSheet.Cells[2, 3] = darkCurrentTestResults.UserExecuted;
                xlDarkCurrentSheet.Cells[2, 4] = darkCurrentTestResults.DarkCurrentMean;
                xlDarkCurrentSheet.Cells[2, 5] = darkCurrentTestResults.TestPassed;
                xlDarkCurrentSheet.Cells[2, 6] = darkCurrentTestResults.PRNUDefects;
                xlDarkCurrentSheet.Cells[2, 7] = darkCurrentTestResults.TrapDefects;
                xlDarkCurrentSheet.Cells[2, 8] = darkCurrentTestResults.HotPixelDefects;
                xlDarkCurrentSheet.Cells[2, 9] = darkCurrentTestResults.ClusterDefects;
                xlDarkCurrentSheet.Cells[2, 10] = darkCurrentTestResults.TotalDefects;
                xlDarkCurrentSheet.Cells[2, 11] = darkCurrentTestResults.MaxDarkFF;
                xlDarkCurrentSheet.Cells[2, 12] = darkCurrentTestResults.AveDarkFF;
                xlDarkCurrentSheet.Cells[2, 13] = darkCurrentTestResults.MaxTrap;
                xlDarkCurrentSheet.Cells[2, 14] = darkCurrentTestResults.AveTrap;
                xlDarkCurrentSheet.Cells[2, 15] = darkCurrentTestResults.MaxDarkROI;
                xlDarkCurrentSheet.Cells[2, 16] = darkCurrentTestResults.DeadPixels;
                xlDarkCurrentSheet.Cells[2, 17] = darkCurrentTestResults.BadColumns;
                xlDarkCurrentSheet.Cells[2, 18] = darkCurrentTestResults.BadRows;
                xlDarkCurrentSheet.Cells[2, 19] = darkCurrentTestResults.DefectsMean;
                xlDarkCurrentSheet.Cells[2, 20] = resultslocalPath + "\\DarkCurrentTestResultImage_" + fileDatestamp + ".png";
                string[] mean = darkCurrentTestResults.DarkCurrentMean.Split(',');
                xlDarkCurrentSheet.Cells[4, 1] = "Mean - Plot Y";
                for (int i = 0; i < mean.Count(); i++)
                {
                    xlDarkCurrentSheet.Cells[i + 5, 1] = Convert.ToDouble(mean[i]);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private static void UpdateDefectsWorkSheet(DefectsTestResults defectsTestResults)
        {
            try
            {
                xlDefectsSheet.Activate();
                xlDefectsSheet.Cells[2, 1] = defectsTestResults.ResultsID;
                xlDefectsSheet.Cells[2, 2] = fileDatestamp;
                xlDefectsSheet.Cells[2, 3] = defectsTestResults.UserExecuted;
                xlDefectsSheet.Cells[2, 4] = defectsTestResults.TestPassed;
                xlDefectsSheet.Cells[2, 5] = defectsTestResults.PRNUDefects;
                xlDefectsSheet.Cells[2, 6] = defectsTestResults.TrapDefects;
                xlDefectsSheet.Cells[2, 7] = defectsTestResults.HotPixelDefects;
                xlDefectsSheet.Cells[2, 8] = defectsTestResults.ClusterDefects;
                xlDefectsSheet.Cells[2, 9] = defectsTestResults.DeadPixelDefects;
                xlDefectsSheet.Cells[2, 10] = defectsTestResults.DarkColumns;
                xlDefectsSheet.Cells[2, 11] = defectsTestResults.DarkRows;
                xlDefectsSheet.Cells[2, 12] = defectsTestResults.LightColumns;
                xlDefectsSheet.Cells[2, 13] = defectsTestResults.LightRows;
                xlDefectsSheet.Cells[2, 14] = defectsTestResults.MeanDefects;
                xlDefectsSheet.Cells[2, 15] = resultslocalPath + "\\DefectsTestResultImage_" + fileDatestamp + ".png";
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private static void UpdateShutterDriveWorkSheet(ShutterDriveTestResults shutterDriveTestResults)
        {
            try
            {
                xlShutterDriveSheet.Activate();
                xlShutterDriveSheet.Cells[2, 1] = shutterDriveTestResults.ResultsID;
                xlShutterDriveSheet.Cells[2, 2] = shutterDriveTestResults.LabJackSN;
                xlShutterDriveSheet.Cells[2, 3] = shutterDriveTestResults.ShutterPlot;
                xlShutterDriveSheet.Cells[2, 4] = fileDatestamp;
                xlShutterDriveSheet.Cells[2, 5] = shutterDriveTestResults.UserExecuted;
                xlShutterDriveSheet.Cells[2, 6] = shutterDriveTestResults.TestPassed;
                xlShutterDriveSheet.Cells[2, 7] = resultslocalPath + "\\ShutterDriveTestResultImage_" + fileDatestamp + ".png";
                string[] shutterPlot = shutterDriveTestResults.ShutterPlot.Split(',');
                xlShutterDriveSheet.Cells[4, 1] = "Shutter Plot";
                for (int i = 0; i < shutterPlot.Count(); i++)
                {
                    xlShutterDriveSheet.Cells[i + 5, 1] = Convert.ToDouble(shutterPlot[i]);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private static void UpdateBurnInWorkSheet(BurnInAnalysisModel cameraBurnInDetails)
        {
            try
            {
                xlBurnInDetailsSheet.Cells[2, 1] = cameraBurnInDetails.BurnInID;
                xlBurnInDetailsSheet.Cells[2, 2] = cameraBurnInDetails.CameraSN;
                xlBurnInDetailsSheet.Cells[2, 3] = cameraBurnInDetails.BurnInResult;
                xlBurnInDetailsSheet.Cells[2, 4] = cameraBurnInDetails.DateExecuted;
                xlBurnInDetailsSheet.Cells[2, 5] = cameraBurnInDetails.UserExecuted;
                xlBurnInDetailsSheet.Cells[2, 6] = cameraBurnInDetails.BurnInLogFileData;
                //xlBurnInDetailsSheet.Cells[2, 7] = cameraBurnInDetails.DmesgLogFileResults;
                xlBurnInDetailsSheet.Cells[2, 7] = cameraBurnInDetails.BurnInResultsPath;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private static void InitializeColumnHeaders(string[] selectedTests, BurnInAnalysisModel cameraBurnInDetails)
        {
            try
            {
                foreach (string str in selectedTests)
                {
                    switch (str)
                    {                       
                        case "RBT":
                            //Red Blue
                            InitializeRedBlueWorkSheet();
                            break;                       
                        case "MVT":
                            //mean Variance
                            InitializeMeanVarianceWorkSheet();
                            break;                       
                        case "DDT":
                            //DarkCurrent
                            InitializeDarkCurrentWorkSheet();
                            break;                      
                        case "DET":
                            //Defects
                            InitializeDefectsWorkSheet();
                            break;                       
                        case "RNT":
                            //NoiseVsNDROs
                            InitializeReadNoiseWorkSheet();
                            break;                       
                        case "PRT":
                            //Photoresponse
                            InitializePhotoresponseWorkSheet();
                            break;                       
                        case "IET":
                            //Injection Efficiency
                            InitializeInjectionEffWorkSheet();
                            break;                       
                        case "SDT":
                            //ShutterDrive
                            InitializeShutterDriveWorkSheet();
                            break;
                    }
                }
                InitializeBurnInWorkSheet();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private static void InitializeRedBlueWorkSheet()
        {
            try
            {
                xlRedBlueSheet = (Excel.Worksheet)excelSheets.Add(excelSheets[1], Type.Missing, Type.Missing, Type.Missing);
                xlRedBlueSheet.Name = "Red&Blue";
                xlRedBlueSheet.Cells[1, 1] = "ResultsID";
                xlRedBlueSheet.Cells[1, 2] = "Test Date";
                xlRedBlueSheet.Cells[1, 3] = "User Executed";
                xlRedBlueSheet.Cells[1, 4] = "RedImageData";
                xlRedBlueSheet.Cells[1, 5] = "BlueImageData";
                xlRedBlueSheet.Cells[1, 6] = "RedBlueImageDifference";
                xlRedBlueSheet.Cells[1, 7] = "Test Passed";
                xlRedBlueSheet.Cells[1, 8] = "Test Result Image Path";
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private static void InitializeMeanVarianceWorkSheet()
        {
            try
            {
                xlMeanVarianceSheet = (Excel.Worksheet)excelSheets.Add(excelSheets[1], Type.Missing, Type.Missing, Type.Missing);
                xlMeanVarianceSheet.Name = "Mean Variance";
                xlMeanVarianceSheet.Cells[1, 1] = "ResultsID";
                xlMeanVarianceSheet.Cells[1, 2] = "Test Date";
                xlMeanVarianceSheet.Cells[1, 3] = "User Executed";
                xlMeanVarianceSheet.Cells[1, 4] = "Corrected Mean";
                xlMeanVarianceSheet.Cells[1, 5] = "Corrected Variance";
                xlMeanVarianceSheet.Cells[1, 6] = "Linear Curve Fit";
                xlMeanVarianceSheet.Cells[1, 7] = "Gain";
                xlMeanVarianceSheet.Cells[1, 8] = "Test Passed";
                xlMeanVarianceSheet.Cells[1, 9] = "Test Result Image Path";
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private static void InitializeReadNoiseWorkSheet()
        {
            try
            {
                xlNoiseVsNDROSheet = (Excel.Worksheet)excelSheets.Add(excelSheets[1], Type.Missing, Type.Missing, Type.Missing);
                xlNoiseVsNDROSheet.Name = "NoiseVsNDRO";
                xlNoiseVsNDROSheet.Cells[1, 1] = "ResultsID";
                xlNoiseVsNDROSheet.Cells[1, 2] = "Test Date";
                xlNoiseVsNDROSheet.Cells[1, 3] = "User Executed";
                xlNoiseVsNDROSheet.Cells[1, 4] = "ExposureNDRO";
                xlNoiseVsNDROSheet.Cells[1, 5] = "NoiseRatio";
                xlNoiseVsNDROSheet.Cells[1, 6] = "Noise";
                xlNoiseVsNDROSheet.Cells[1, 7] = "R2Correlation";
                xlNoiseVsNDROSheet.Cells[1, 8] = "BestFit";
                xlNoiseVsNDROSheet.Cells[1, 9] = "BestFitPower";
                xlNoiseVsNDROSheet.Cells[1, 10] = "TheoryValue";
                xlNoiseVsNDROSheet.Cells[1, 11] = "SignalReadNoise";
                xlNoiseVsNDROSheet.Cells[1, 12] = "NDRONoise";
                xlNoiseVsNDROSheet.Cells[1, 13] = "Test Passed";
                xlNoiseVsNDROSheet.Cells[1, 14] = "Test Result Image Path";
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private static void InitializePhotoresponseWorkSheet()
        {
            try
            {
                xlPhotoresponseSheet = (Excel.Worksheet)excelSheets.Add(excelSheets[1], Type.Missing, Type.Missing, Type.Missing);
                xlPhotoresponseSheet.Name = "Photoresponse";
                xlPhotoresponseSheet.Cells[1, 1] = "ResultsID";
                xlPhotoresponseSheet.Cells[1, 2] = "Test Date";
                xlPhotoresponseSheet.Cells[1, 3] = "User Executed";
                xlPhotoresponseSheet.Cells[1, 4] = "Mean";
                xlPhotoresponseSheet.Cells[1, 5] = "LinearCurveFit";
                xlPhotoresponseSheet.Cells[1, 6] = "LinearFitData";
                xlPhotoresponseSheet.Cells[1, 7] = "FullWell";
                xlPhotoresponseSheet.Cells[1, 8] = "LinearSaturation";
                xlPhotoresponseSheet.Cells[1, 9] = "Slope";
                xlPhotoresponseSheet.Cells[1, 10] = "Intercept";
                xlPhotoresponseSheet.Cells[1, 11] = "DerivPlot";
                xlPhotoresponseSheet.Cells[1, 12] = "TestPassed";
                xlPhotoresponseSheet.Cells[1, 13] = "Test Result Image Path";
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private static void InitializeInjectionEffWorkSheet()
        {
            try
            {
                xlInjectionPerformanceSheet = (Excel.Worksheet)excelSheets.Add(excelSheets[1], Type.Missing, Type.Missing, Type.Missing);
                xlInjectionPerformanceSheet.Name = "Injection Efficiency";
                xlInjectionPerformanceSheet.Cells[1, 1] = "ResultsID";
                xlInjectionPerformanceSheet.Cells[1, 2] = "Test Date";
                xlInjectionPerformanceSheet.Cells[1, 3] = "User Executed";
                xlInjectionPerformanceSheet.Cells[1, 4] = "InitialExposure";
                xlInjectionPerformanceSheet.Cells[1, 5] = "FirstInjection";
                xlInjectionPerformanceSheet.Cells[1, 6] = "SecondInjection";
                xlInjectionPerformanceSheet.Cells[1, 7] = "ThirdInjection";
                xlInjectionPerformanceSheet.Cells[1, 8] = "FourthInjection";
                xlInjectionPerformanceSheet.Cells[1, 9] = "LastInjection";
                xlInjectionPerformanceSheet.Cells[1, 10] = "Exp0PlotData";
                xlInjectionPerformanceSheet.Cells[1, 11] = "Exp1PlotData";
                xlInjectionPerformanceSheet.Cells[1, 12] = "Exp2PlotData";
                xlInjectionPerformanceSheet.Cells[1, 13] = "Exp3PlotData";
                xlInjectionPerformanceSheet.Cells[1, 14] = "Exp4PlotData";
                xlInjectionPerformanceSheet.Cells[1, 15] = "TestPassed";
                xlInjectionPerformanceSheet.Cells[1, 16] = "Test Result Image Path";
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private static void InitializeDarkCurrentWorkSheet()
        {
            try
            {
                xlDarkCurrentSheet = (Excel.Worksheet)excelSheets.Add(excelSheets[1], Type.Missing, Type.Missing, Type.Missing);
                xlDarkCurrentSheet.Name = "Dark Current";
                xlDarkCurrentSheet.Cells[1, 1] = "ResultsID";
                xlDarkCurrentSheet.Cells[1, 2] = "Test Date";
                xlDarkCurrentSheet.Cells[1, 3] = "User Executed";
                xlDarkCurrentSheet.Cells[1, 4] = "DarkCurrentMean";
                xlDarkCurrentSheet.Cells[1, 5] = "Test Passed";
                xlDarkCurrentSheet.Cells[1, 6] = "PRNUDefects";
                xlDarkCurrentSheet.Cells[1, 7] = "TrapDefects";
                xlDarkCurrentSheet.Cells[1, 8] = "HotPixelDefects";
                xlDarkCurrentSheet.Cells[1, 9] = "ClusterDefects";
                xlDarkCurrentSheet.Cells[1, 10] = "TotalDefects";
                xlDarkCurrentSheet.Cells[1, 11] = "MaxDarkFF";
                xlDarkCurrentSheet.Cells[1, 12] = "AveDarkFF";
                xlDarkCurrentSheet.Cells[1, 13] = "MaxTrap";
                xlDarkCurrentSheet.Cells[1, 14] = "AveTrap";
                xlDarkCurrentSheet.Cells[1, 15] = "MaxDarkROI";
                xlDarkCurrentSheet.Cells[1, 16] = "DeadPixels";
                xlDarkCurrentSheet.Cells[1, 17] = "BadColumns";
                xlDarkCurrentSheet.Cells[1, 18] = "BadRows";
                xlDarkCurrentSheet.Cells[1, 19] = "DefectsMean";
                xlDarkCurrentSheet.Cells[1, 20] = "Test Result Image Path";
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private static void InitializeDefectsWorkSheet()
        {
            try
            {
                xlDefectsSheet = (Excel.Worksheet)excelSheets.Add(excelSheets[1], Type.Missing, Type.Missing, Type.Missing);
                xlDefectsSheet.Name = "Defects";
                xlDefectsSheet.Cells[1, 1] = "ResultsID";
                xlDefectsSheet.Cells[1, 2] = "Test Date";
                xlDefectsSheet.Cells[1, 3] = "User Executed";
                xlDefectsSheet.Cells[1, 4] = "Test Passed";
                xlDefectsSheet.Cells[1, 5] = "PRNUDefects";
                xlDefectsSheet.Cells[1, 6] = "TrapDefects";
                xlDefectsSheet.Cells[1, 7] = "HotPixelDefects";
                xlDefectsSheet.Cells[1, 8] = "ClusterDefects";
                xlDefectsSheet.Cells[1, 9] = "DeadPixels";
                xlDefectsSheet.Cells[1, 10] = "DarkColumns";
                xlDefectsSheet.Cells[1, 11] = "DarkRows";
                xlDefectsSheet.Cells[1, 12] = "LightColumns";
                xlDefectsSheet.Cells[1, 13] = "LightRows";
                xlDefectsSheet.Cells[1, 14] = "MeanDefects";
                xlDefectsSheet.Cells[1, 15] = "Test Result Image Path";
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private static void InitializeShutterDriveWorkSheet()
        {
            try
            {
                xlShutterDriveSheet = (Excel.Worksheet)excelSheets.Add(excelSheets[1], Type.Missing, Type.Missing, Type.Missing);
                xlShutterDriveSheet.Name = "Shutter Drive";
                xlShutterDriveSheet.Cells[1, 1] = "ResultsID";
                xlShutterDriveSheet.Cells[1, 2] = "LabJackSN";
                xlShutterDriveSheet.Cells[1, 3] = "ShutterPlot";
                xlShutterDriveSheet.Cells[1, 4] = "Test Date";
                xlShutterDriveSheet.Cells[1, 5] = "User Executed";
                xlShutterDriveSheet.Cells[1, 6] = "TestPassed";
                xlShutterDriveSheet.Cells[1, 7] = "Test Result Image Path";
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private static void InitializeBurnInWorkSheet()
        {
            try
            {
                xlBurnInDetailsSheet = (Excel.Worksheet)excelSheets.Add(excelSheets[1], Type.Missing, Type.Missing, Type.Missing);
                xlBurnInDetailsSheet.Name = "BurnIn Details";
                xlBurnInDetailsSheet.Cells[1, 1] = "BurnInID";
                xlBurnInDetailsSheet.Cells[1, 2] = "CameraSN";
                xlBurnInDetailsSheet.Cells[1, 3] = "BurnInResult";
                xlBurnInDetailsSheet.Cells[1, 4] = "Test Date";
                xlBurnInDetailsSheet.Cells[1, 5] = "User Executed";
                xlBurnInDetailsSheet.Cells[1, 6] = "BurnIn Log File Data";
                //xlBurnInDetailsSheet.Cells[1, 7] = "Dmesg Log File Results";
                xlBurnInDetailsSheet.Cells[1, 7] = "BurnIn Results Path";
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public static void CloseExcel()
        {
            try
            {
                log.Info("Begin Close Excel");
                if (xlWorkBook != null)
                 xlWorkBook.Close(true, misValue, misValue);
                if (xlApp != null)
                    xlApp.Quit();
                if (xlRedBlueSheet != null)
                    Marshal.ReleaseComObject(xlRedBlueSheet);
                if (xlMeanVarianceSheet != null)
                    Marshal.ReleaseComObject(xlMeanVarianceSheet);
                if (xlShutterDriveSheet != null)
                    Marshal.ReleaseComObject(xlShutterDriveSheet);
                if (xlDarkCurrentSheet != null)
                    Marshal.ReleaseComObject(xlDarkCurrentSheet);
                if (xlDefectsSheet != null)
                    Marshal.ReleaseComObject(xlDefectsSheet);
                if (xlNoiseVsNDROSheet != null)
                    Marshal.ReleaseComObject(xlNoiseVsNDROSheet);
                if (xlPhotoresponseSheet != null)
                    Marshal.ReleaseComObject(xlPhotoresponseSheet);
                if (xlInjectionPerformanceSheet != null)
                    Marshal.ReleaseComObject(xlInjectionPerformanceSheet);
                if (xlBurnInDetailsSheet != null)
                    Marshal.ReleaseComObject(xlBurnInDetailsSheet);
                if (excelSheets != null)
                    Marshal.ReleaseComObject(excelSheets);
                if (excelWorksheet != null)
                    Marshal.ReleaseComObject(excelWorksheet);
                if (xlWorkBook != null)
                    Marshal.ReleaseComObject(xlWorkBook);
                if (xlApp != null)
                    Marshal.ReleaseComObject(xlApp);
                xlRedBlueSheet = null;
                xlMeanVarianceSheet = null;
                xlShutterDriveSheet = null;
                xlDarkCurrentSheet = null;
                xlDefectsSheet = null;
                xlNoiseVsNDROSheet = null;
                xlPhotoresponseSheet = null;
                xlInjectionPerformanceSheet = null;
                xlBurnInDetailsSheet = null;
                excelSheets = null;
                excelWorksheet = null;
                xlApp = null;
                xlWorkBook = null;
                //GC.Collect();
                log.Info("End Close Excel");
            }
            catch(Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public static void LogCameraTempHumidity(string logFileLocalPath,string logFileServerPath,List<double>camTemp, List<double>camHum,List<string> loggedTime)
        {
            try
            {
                xlAppForTempLog = new Microsoft.Office.Interop.Excel.Application();
                xlAppForTempLog.DisplayAlerts = false;
                xlWorkBookForTempLog = xlAppForTempLog.Workbooks.Add(misValue);
                excelSheetsforTempLog = xlWorkBookForTempLog.Worksheets;
                xlWorkSheetForTempLog = (Excel.Worksheet)excelSheetsforTempLog.Add(excelSheetsforTempLog[1], Type.Missing, Type.Missing, Type.Missing);
                xlWorkSheetForTempLog.Name = "Camera Temp & Humidity";
                xlWorkSheetForTempLog.Cells[1, 1] = "Temperature";
                xlWorkSheetForTempLog.Cells[1, 2] = "Humidity";
                xlWorkSheetForTempLog.Cells[1, 3] = "Date&Time";
                for (int index = 0; index < camTemp.Count(); index++)
                {
                    xlWorkSheetForTempLog.Cells[index + 2, 1] = camTemp[index];
                    xlWorkSheetForTempLog.Cells[index + 2, 2] = camHum[index];
                    xlWorkSheetForTempLog.Cells[index + 2, 3] = loggedTime[index];
                }
                string filetimestamp = DateTime.Now.ToString("MM_dd_yyyy_HH_mm");
                logFileServerPath = logFileServerPath + "\\Temp_Hum_log_" + filetimestamp + ".xlsx";
                if (!File.Exists(logFileServerPath))
                {
                    xlWorkBookForTempLog.SaveAs(logFileServerPath);
                }
                logFileLocalPath = logFileLocalPath + "\\Temp_Hum_log_" + filetimestamp + ".xlsx";
                File.Copy(logFileServerPath, logFileLocalPath, true);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
            finally
            {
                CloseTempLogExcel();
            }
        }
        public static void CloseTempLogExcel()
        {
            try
            {
                log.Info("Begin Close Excel");
                if (xlWorkBookForTempLog != null)
                    xlWorkBookForTempLog.Close(true, misValue, misValue);
                if (xlAppForTempLog != null)
                    xlAppForTempLog.Quit();
                if (xlWorkSheetForTempLog != null)
                {
                    Marshal.ReleaseComObject(xlWorkSheetForTempLog);
                    xlWorkSheetForTempLog = null;
                }
                if (excelSheetsforTempLog != null)
                {
                    Marshal.ReleaseComObject(excelSheetsforTempLog);
                    excelSheetsforTempLog = null;
                }
                if (xlWorkBookForTempLog != null)
                {
                    Marshal.ReleaseComObject(xlWorkBookForTempLog);
                    xlWorkBookForTempLog = null;
                }
                if (xlAppForTempLog != null)
                {
                    Marshal.ReleaseComObject(xlAppForTempLog);
                    xlAppForTempLog = null;
                } 
                GC.Collect();
                GC.WaitForPendingFinalizers();
                log.Info("End Close Excel");
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        //public static List<MaskDataStruct> GetMaskDataList()
        //{
        //    try
        //    {
        //        if (maskDataList != null)
        //            return maskDataList;
        //        else
        //            return maskDataList = new List<MaskDataStruct>(1);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
        //        return null;
        //    }
        //}
        //public static void ReadSubarrayData()
        //{
        //    try
        //    {

        //        int MaxRows = 500;
        //        int MaxColumns = 20;
        //        //xlApp = new Microsoft.Office.Interop.Excel.Application();
        //        //xlApp.DisplayAlerts = false;
        //        //xlWorkBook = xlApp.Workbooks.Add(misValue);
        //        xlWorkSheet = xlWorkBook.Sheets[1];
        //        Excel.Range sheetCells = xlWorkSheet.Cells;
        //        Excel.Range cellFirst = sheetCells[1, 1] as Excel.Range;
        //        Excel.Range cellLast = sheetCells[MaxRows, MaxColumns] as Excel.Range;
        //        Excel.Range theRange = xlWorkSheet.get_Range(cellFirst, cellLast);
        //        theRange.Select();
        //        double numberROIs = theRange.Cells[2, 11].Value;
        //        int numberOfROIs = Convert.ToInt32(numberROIs);

        //        maskDataList = new List<MaskDataStruct>(numberOfROIs);
        //        MaskDataStruct maskData = new MaskDataStruct();

        //        for (int roiIndex = 0; roiIndex < numberOfROIs; roiIndex++)
        //        {
        //            maskData.Xo = Convert.ToInt32(theRange.Cells[roiIndex + 3, 4].Value);
        //            maskData.Yo = Convert.ToInt32(theRange.Cells[roiIndex + 3, 5].Value);
        //            maskData.dX = Convert.ToInt32(theRange.Cells[2, 8].Value);
        //            maskData.dY = Convert.ToInt32(theRange.Cells[3, 8].Value);
        //            var element = theRange.Cells[roiIndex + 3, 2].Value;
        //            double wavelength = (theRange.Cells[roiIndex + 3, 3].Value);
        //            maskData.name = element + "_" + wavelength;

        //            maskDataList.Add(maskData);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
        //    }
        //}
        //public static void OpenExistingExcelWorksheet(string fileName)
        //{
        //    try
        //    {
        //        //object misValue = System.Reflection.Missing.Value;
        //        xlApp = new Microsoft.Office.Interop.Excel.Application();
        //        // xlWorkBook = xlApp.Workbooks.Add(misValue);
        //        xlWorkBook = xlApp.Workbooks.Open(fileName);
        //        //  xlWorkBook.Name = fileName;
        //        // xlApp.Visible = true;
        //        xlApp.ActiveWindow.Activate();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
        //    }

        //}
        public static void ShutExcel()
        {
            try
            {
                String def = System.Windows.Forms.Application.StartupPath.ToString();
                if (xlWorkBook != null)
                    xlWorkBook.Close(true, misValue, misValue);
                if (xlApp != null)
                    xlApp.Quit();
                if (xlWorkBook != null)
                    Marshal.ReleaseComObject(xlWorkBook);
                if (xlApp != null)
                    Marshal.ReleaseComObject(xlApp);
                if (xlWorkSheet != null)
                    Marshal.ReleaseComObject(xlWorkSheet);

                xlApp = null;
                xlWorkBook = null;
                xlWorkSheet = null;
            }
            catch(Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        //public static async Task buildROIMask()
        //{
        //    await Task.Run(() =>
        //    {
        //        try
        //        {

        //            String def = System.Windows.Forms.Application.StartupPath.ToString();
        //            if (System.IO.Directory.Exists(def + "\\Resources"))
        //                def = def + "\\Resources";
        //            String maskROIsFile;
        //            maskROIsFile = def + "\\MaskROIs.xlsx";
        //            // if (System.IO.File.Exists(maskROIsFile))


        //            //string maskROIsFile = string.Format("{0}Resources\\MaskROIs.xlsx",
        //            //Path.GetFullPath(Path.Combine(RunningPath, @"..\..\..\")));

        //            if (System.IO.File.Exists(maskROIsFile))
        //            {
        //                OpenExistingExcelWorksheet(maskROIsFile);
        //                //excelObject.OpenExcelWorksheet(maskROIsFile);
        //                ReadSubarrayData();

        //                // Get the list of roi's
        //                List<MaskDataStruct> maskDataList;

        //                maskDataList = ExcelUtilities.GetMaskDataList();

        //                if (maskDataList.Count > 0)
        //                {
        //                    var subarrayIndex = 0;
        //                    foreach (MaskDataStruct maskDataStruct in maskDataList)
        //                    {
        //                        subarrayIndex++;
        //                        for (int x = maskDataStruct.Xo; x < maskDataStruct.Xo + maskDataStruct.dX; x++)
        //                        {
        //                            for (int y = maskDataStruct.Yo; y < maskDataStruct.Yo + maskDataStruct.dY; y++)
        //                            {
        //                                validROIList[x, y] = true;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                MessageBox.Show("Mask ROI file not found" + maskROIsFile.ToString(),
        //                    "Error loading Mask ROI",
        //                      MessageBoxButtons.OK,
        //                      MessageBoxIcon.Error);
        //                log.Error("Mask ROI file not found");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
        //        }
        //    });
           
        //}

    }
}
