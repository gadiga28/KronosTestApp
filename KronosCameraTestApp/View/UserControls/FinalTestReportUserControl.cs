using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KronosCameraTestApp.Helpers;
using System.Reflection;
using System.IO;
using KronosCameraTestApp.Model;
using System.Configuration;
using Thermo.Kronos.Instrument.Camera.Contracts.Data;
using System.Security.AccessControl;
using Thermo.Kronos.Instrument.Camera.Interface;
using Renci.SshNet;

namespace KronosCameraTestApp.View.UserControls
{
    public partial class FinalTestReportUserControl : UserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string defaultFilePath = string.Empty;
        string filepath = string.Empty;
        string userMode = string.Empty;
        Bitmap finalReportBitmap = null;
        bool injEffInitialExposurePassFail = false;
        bool injEffFirstInjPassFail = true;
        bool injEffLastInjPassFail = true;
        string[] injEffInitialExposureList;
        string[] injEffIFirstInjList;
        string[] injEffLastInjList;
        List<ECOData> sortedECOData { get; set; }       
        ECOData ecoData { get; set; }
        string servPath = string.Empty;
        bool preTest = false;
        bool firmwareErrs = false;
        bool camTempOutofRange = false;
        bool? camCurrentOutofRange = false;
        string imageTemperature;
        string camCurrentAvg;

        public string CamCurrentAvg
        {
            get { return camCurrentAvg; }
            set
            {
                camCurrentAvg = value;
            }
        }
        public string ImageTemperature
        {
            get { return imageTemperature; }
            set
            {
                imageTemperature = value;                
            }
        }
        string imageHumidity;
        public string ImageHumidity
        {
            get { return imageHumidity; }
            set
            {
                imageHumidity = value;
            }
        }
        public FinalTestReportUserControl(string defaultFilePath, string UserMode)
        {
            InitializeComponent();
            this.defaultFilePath = defaultFilePath;
            this.userMode = UserMode;
        }
        private void FinalTestReportUserControl_Load(object sender, EventArgs e)
        {
            try
            {
                ResetReportData();
                TestStation.Text = Environment.MachineName;
                CameraModel.Text="SCM5821AX1";
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void GetEcoData()
        {
            try
            {
                List<ECOData> ecoDataList = new List<ECOData>();
                sortedECOData = new List<ECOData>();
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
                        .Include("LEDCalibrationLimitsData")
                        .ToList();
                    sortedECOData = (from ecos in ecoDataList orderby ecos.DefinedDate descending select ecos).ToList();
                }   
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }                             
        }
        private void ResetReportData()
        {
            try
            {
                RedBlueUVLimit.Text= MaxDarkROILimit.Text = FullWellLevelMinLimit.Text = ReadNoiseNDROLimit.Text = ReadNoiseBestFitPowerLimit.Text =
                RPower2CorrelationLimit.Text = InjEffFirstInjLimit.Text = InjEffLastInjLimit.Text=String.Empty;
                //calculated results
                ClustersResult.Text = ConversionFactorNominalResult.Text =
                AveTrapResult.Text = MaxDarkROIResult.Text = FullWellLevelMinResult.Text = SnglNoiseMaxResult.Text =
                ReadNoiseBestFitPowerResult.Text = RPower2CorrelationResult.Text = lblInjEffInitialExpoResult.Text = 
                lblInjEffFirstInjResult.Text = lblInjEffLastInjResult.Text = String.Empty;
				AveTrapResultInElectrons.Text = MaxDarkROIResultInElectrons.Text = FullWellLevelMinResultInElectrons.Text = 
                SnglNoiseMaxResultInElectrons.Text = lblInjEffInitialExpoResultInElectrons.Text = lblInjEffFirstInjResultInElectrons.Text =
                lblInjEffLastInjResultInElectrons.Text = String.Empty;
				//pass/fail conditions
				ConversionFactorPassFail.Text = ClustersPassFail.Text =AveTrapPassFail.Text =
                MaxDarkROIPassFail.Text = FullWellLevelMinPassFail.Text = SnglNoiseMaxPassFail.Text = ReadNoiseBestFitPowerPassFail.Text =
                RPower2CorrelationPassFail.Text = lblInjEffFirstInjPassFail.Text = lblInjEffLastInjPassFail.Text = ShutterDrivePassFail.Text =
                lblBurnInResult.Text= redBlueUVPassFail.Text=string.Empty;
                //test settings
                TestSWVersion.Text = DatabaseVersion.Text = ultraLabelLimitID.Text = FirmwareVersion.Text = CSPBoardRev.Text = FirmwareFile.Text=
                PowerBoardRev.Text = CPUBoardRev.Text =  FPGAVersion.Text = lblLabbJackSN.Text= string.Empty;
                CameraModel.Text = MACAddress.Text = CameraSN.Text = DetectorType.Text = ultraLabelLimitID.Text = linkLabel1.Text =
                TestRunDate.Text = TestRunTime.Text = Temperature.Text = Tester.Text = CameraSN.Text = ImagerSN.Text = string.Empty;
                //defects
                DarkPixelsResult.Text = HotPixelsResult.Text = OverallDefectsTestPassFail.Text = 
                DefectsTestPassFail.Text =
                DeadPixelsResult.Text = ColumnDefectsResult.Text = RowDefectsResult.Text = string.Empty;

                camCurrentAvgValue.Text = camTempAvgValue.Text = string.Empty;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public bool ReturnTestPassFailConditon(FinalTestResultsDetailsModel finalTestResultsDetailsModel)
        {
            try
            {
                double checkGainValue = (finalTestResultsDetailsModel.ConversionFactorNominal * ecoData.MeanVarianceTestLimitsData.ToList()[0].ConverisonFactorTol) / 100;
                foreach (string str in injEffIFirstInjList)
                {
                    double value = Convert.ToDouble(str);
                    if (!Double.IsNaN(value))
                    {
                        if(!Double.IsInfinity(value))
                        {
                            if (value > ecoData.InjectionEfficiencyTestLimitsData.ToList()[0].InjectionEffLimit)
                            {
                                injEffFirstInjPassFail = false;
                                break;
                            }
                        }
                    }
                                  
                }
                foreach (string str in injEffLastInjList)
                {
                    double value = Convert.ToDouble(str);
                    if (!Double.IsNaN(value))
                    {
                        if (!Double.IsInfinity(value))
                        {
                            if (value > ecoData.InjectionEfficiencyTestLimitsData.ToList()[0].InjectionEffLimit)
                            {
                                injEffFirstInjPassFail = false;
                                break;
                            }
                        }
                    }
                }
                
                //to do check for AveTrap,max Dark ROI and Best fit power
                if (((finalTestResultsDetailsModel.ConversionFactorNominal >= ecoData.MeanVarianceTestLimitsData.ToList()[0].ConversionFactorLowerLimit) &&
                    (finalTestResultsDetailsModel.ConversionFactorNominal <= ecoData.MeanVarianceTestLimitsData.ToList()[0].ConversionFactorUpperLimit)) &&                  
                  (finalTestResultsDetailsModel.TotalNumClusters <= ecoData.DefectsTestLimitsData.ToList()[0].TotalNumClusters &&
                  finalTestResultsDetailsModel.HotPixels <= ecoData.DefectsTestLimitsData.ToList()[0].HotPixel &&
                  finalTestResultsDetailsModel.DarkPixels <= ecoData.DefectsTestLimitsData.ToList()[0].DarkPixel &&
                  finalTestResultsDetailsModel.DeadPixels <= ecoData.DefectsTestLimitsData.ToList()[0].DeadPixel &&
                  finalTestResultsDetailsModel.TotalRowDefects <= ecoData.DefectsTestLimitsData.ToList()[0].TotalRows &&
                  finalTestResultsDetailsModel.TotalColumnDefects <= ecoData.DefectsTestLimitsData.ToList()[0].TotalColumns &&
                  string.IsNullOrWhiteSpace(finalTestResultsDetailsModel.DefectivePixelPerROI) &&
                  finalTestResultsDetailsModel.DriftROICount <= ecoData.DefectsTestLimitsData.ToList()[0].DriftROI) &&
                  (finalTestResultsDetailsModel.MaxDarkROI >= ecoData.DarkCurrentTestLimitsData.ToList()[0].DarkCurrentLowerLimit &&
                  finalTestResultsDetailsModel.MaxDarkROI <= ecoData.DarkCurrentTestLimitsData.ToList()[0].DarkCurrentUpperLimit)&&
                  finalTestResultsDetailsModel.FullWellLevelMin >= ecoData.PhotoresponseLimitsData.ToList()[0].FullWellLevelMin &&
                  (finalTestResultsDetailsModel.SnglNoiseMax <= ecoData.NoiseVsNDROSLimitsData.ToList()[0].SnglNoiseUpperLimit &&
                   finalTestResultsDetailsModel.SnglNoiseMax >= ecoData.NoiseVsNDROSLimitsData.ToList()[0].SnglNoiseLowerLimit) &&
                  finalTestResultsDetailsModel.RPower2Correlation >= ecoData.NoiseVsNDROSLimitsData.ToList()[0].RPower2Correlation &&
                  injEffFirstInjPassFail && injEffLastInjPassFail &&
                  finalTestResultsDetailsModel.ShutterDrive && finalTestResultsDetailsModel.RedBlue && !firmwareErrs &&
                   finalTestResultsDetailsModel.RowXTalkPassed && finalTestResultsDetailsModel.ColXTalkPassed &&
                  !finalTestResultsDetailsModel.DefectsTestDeadDarkPixelLimitReached && !camTempOutofRange && !(bool)camCurrentOutofRange)
                {
                    return true;
                }                   
                else
                {
                    return false;
                }                    
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
        public string GetFirmwareFileName()
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
                        if(Path.GetExtension(file).Equals(".exe") || Path.GetExtension(file).Equals(".EXE"))
                        {
                            firmwareFileName = file.ToString().Substring(6, file.Length - 10);
                            break;
                        }
                    }
                    sftpClient.Disconnect();
                }
                return firmwareFileName;
            }
            catch(Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return string.Empty;
            }
        }
        public void UpdateFinalTestReportView(FinalTestResultsDetailsModel finalTestResultsDetailsModel, CameraDetailsModel cameraDetailsModel,
                                              ShutterDriveTestResults shutterDriveTestResults,
                                              CameraInformation cameraInformation, BurnInAnalysisModel cameraBurnInData, 
                                              string userExecuted, DateTime dateExecuted, bool finalTest, string[] selectedTests,
                                              bool cameraTempOutOfRange, List<string> firmwareErrorsWarnings,
                                              List<KeyValuePair<string, List<Point>>> DefPixelROIListPairs, 
                                              List<DefectsTestLimitsData> defectsTestLimitsDataList, 
                                              EnvironmentalStatus environmentalStatusForm, bool? cameraCurrentFailed,
                                              EnvironmentalStatusLimitsData environmentalStatusLimitsData,
                                              TemperatureHumidityLimitsData temperatureHumidityLimitsData,
                                              bool coolantTempFailDuringFinalTest, bool purgeFlowRateFailDuringFinalTest)
        {
            try
            {                
                if (!finalTest)
                {
                    preTest = true;
                    finalTestReportHeader.Text = "SCM5821AX1 Pre Test Report";
                }
                camTempAvgValue.Text = Temperature.Text = imageTemperature;
                Humidity.Text = imageHumidity;
                GetEcoData();
                ecoData = new ECOData();
                if (!string.IsNullOrWhiteSpace(TestAppHelper.SelectedECONumber) && (userMode.Equals("Engineering") || userMode.Equals("Admin")))
                    ecoData = sortedECOData.Where(limit => limit.ECONumber.Equals(TestAppHelper.SelectedECONumber)).FirstOrDefault();
                else
                    ecoData = sortedECOData[0];
                if (selectedTests.Contains("RBT"))
                {
                    redBlueUVPassFail.Text= finalTestResultsDetailsModel.RedBlue ? "P" : "F";
                    redBlueUVPassFail.ForeColor = finalTestResultsDetailsModel.RedBlue ? Color.LimeGreen : Color.Red;
                    RedBlueUVResult.Text = String.Format("{0:0.00}", finalTestResultsDetailsModel.UVMean); 
                }
                else
                {
                    redBlueUVPassFail.Text = "NA";
                    RedBlueUVResult.Text = "NA";
                }
                if (selectedTests.Contains("IET"))
                {
                    injEffInitialExposureList = finalTestResultsDetailsModel.InjEfficiencyInitialExposure.Split(',');
                    injEffIFirstInjList = finalTestResultsDetailsModel.InjEfficiencyFirstInjection.Split(',');
                    injEffLastInjList = finalTestResultsDetailsModel.InjEfficiencyLastInjection.Split(',');
                    double[] InjEffInitialExposureResultInElectrons = new double[injEffInitialExposureList.Count()];
                    double[] InjEffFirstInjResultInElectrons = new double[injEffIFirstInjList.Count()];
                    double[] InjEffLastInjResultInElectrons = new double[injEffLastInjList.Count()];
                    for (int i = 0; i < injEffInitialExposureList.Count(); i++)
                    {
                        InjEffInitialExposureResultInElectrons[i] =
                            Convert.ToDouble(injEffInitialExposureList[i]) * finalTestResultsDetailsModel.ConversionFactorNominal;
                    }
                    for (int i = 0; i < injEffIFirstInjList.Count(); i++)
                    {
                        InjEffFirstInjResultInElectrons[i] =
                            Convert.ToDouble(injEffIFirstInjList[i]) * finalTestResultsDetailsModel.ConversionFactorNominal;
                    }
                    for (int i = 0; i < injEffLastInjList.Count(); i++)
                    {
                        InjEffLastInjResultInElectrons[i] =
                            Convert.ToDouble(injEffLastInjList[i]) * finalTestResultsDetailsModel.ConversionFactorNominal;
                    }
                    lblInjEffInitialExpoResultInElectrons.Text = String.Join(",", InjEffInitialExposureResultInElectrons.Select(p => p.ToString("0")).ToArray());
                    lblInjEffFirstInjResultInElectrons.Text = String.Join(",", InjEffFirstInjResultInElectrons.Select(p => p.ToString("0.00")).ToArray());
                    lblInjEffLastInjResultInElectrons.Text = String.Join(",", InjEffLastInjResultInElectrons.Select(p => p.ToString("0.00")).ToArray());
                    lblInjEffFirstInjPassFail.Text = injEffFirstInjPassFail ? "P" : "F";
                    lblInjEffFirstInjPassFail.ForeColor = injEffFirstInjPassFail ? Color.LimeGreen : Color.Red;
                    lblInjEffLastInjPassFail.Text = injEffLastInjPassFail ? "P" : "F";
                    lblInjEffLastInjPassFail.ForeColor = injEffLastInjPassFail ? Color.LimeGreen : Color.Red;
                    lblInjEffInitialExpoResult.Text = finalTestResultsDetailsModel.InjEfficiencyInitialExposure;
                    lblInjEffFirstInjResult.Text = finalTestResultsDetailsModel.InjEfficiencyFirstInjection;
                    lblInjEffLastInjResult.Text = finalTestResultsDetailsModel.InjEfficiencyLastInjection;

                    lblRowCrosstalkPassFail.Text = finalTestResultsDetailsModel.RowXTalkPassed ? "P" : "F";
                    lblRowCrosstalkPassFail.ForeColor = finalTestResultsDetailsModel.RowXTalkPassed ? Color.LimeGreen : Color.Red;

                    lblColCrosstalkPassFail.Text= finalTestResultsDetailsModel.ColXTalkPassed ? "P" : "F";
                    lblColCrosstalkPassFail.ForeColor = finalTestResultsDetailsModel.ColXTalkPassed ? Color.LimeGreen : Color.Red;
                }
                else
                {
                    lblInjEffFirstInjPassFail.Text = "NA";
                    lblInjEffLastInjPassFail.Text = "NA";
                    lblInjEffInitialExpoResult.Text = "NA";
                    lblInjEffFirstInjResult.Text = "NA";
                    lblInjEffLastInjResult.Text = "NA";
                    lblColCrosstalkPassFail.Text = "NA";
                    lblRowCrosstalkPassFail.Text = "NA";
                } 
                UpdateLimitsOnView(ecoData, finalTest, defectsTestLimitsDataList);
                if (selectedTests.Contains("DET"))
                {                    
                    string[] temp = finalTestResultsDetailsModel.DefectivePixelPerROI.Split(new string[] { "ROI" }, StringSplitOptions.RemoveEmptyEntries);
                    DefectsTestLimitsData defectsTestLimitsData = new DefectsTestLimitsData();
                    if (!finalTest)
                    {
                        defectsTestLimitsData = ecoData.DefectsTestLimitsData.Where(d => d.TestStage.Equals("PRE_TEST")).FirstOrDefault();
                    }
                    else
                    {
                        defectsTestLimitsData = ecoData.DefectsTestLimitsData.Where(d => d.TestStage.Equals("FINAL_TEST")).FirstOrDefault();
                    }
                    if(defectsTestLimitsData==null)
                    {
                        if (!finalTest)
                            defectsTestLimitsData = defectsTestLimitsDataList.Where(limit => limit.TestStage.Equals("PRE_TEST")).FirstOrDefault();
                        else
                            defectsTestLimitsData = defectsTestLimitsDataList.Where(limit => limit.TestStage.Equals("FINAL_TEST")).FirstOrDefault();
                    }

                    ClustersResult.Text = finalTestResultsDetailsModel.TotalNumClusters.ToString();
                    ClustersPassFail.Text = finalTestResultsDetailsModel.TotalNumClusters <= defectsTestLimitsData.TotalNumClusters ? "P" : "F";
                    ClustersPassFail.ForeColor = ClustersPassFail.Text.Equals("P") ? Color.LimeGreen : Color.Red;

                    DeadPixelsResult.Text = finalTestResultsDetailsModel.DeadPixels.ToString();
                    DeadPixelsPassFail.Text = finalTestResultsDetailsModel.DeadPixels <= defectsTestLimitsData.DeadPixel ? "P" : "F";
                    DeadPixelsPassFail.ForeColor = DeadPixelsPassFail.Text.Equals("P") ? Color.LimeGreen : Color.Red;

                    DarkPixelsResult.Text = finalTestResultsDetailsModel.DarkPixels.ToString();
                    DarkPixelsPassFail.Text = finalTestResultsDetailsModel.DarkPixels <= defectsTestLimitsData.DarkPixel ? "P" : "F";
                    DarkPixelsPassFail.ForeColor = DarkPixelsPassFail.Text.Equals("P") ? Color.LimeGreen : Color.Red;

                    HotPixelsResult.Text = finalTestResultsDetailsModel.HotPixels.ToString();
                    HotPixelsPassFail.Text = finalTestResultsDetailsModel.HotPixels <= defectsTestLimitsData.HotPixel ? "P" : "F";
                    HotPixelsPassFail.ForeColor = HotPixelsPassFail.Text.Equals("P") ? Color.LimeGreen : Color.Red;

                    ColumnDefectsResult.Text = finalTestResultsDetailsModel.TotalColumnDefects.ToString();
                    DefectsColumnPassFail.Text = finalTestResultsDetailsModel.TotalColumnDefects <= defectsTestLimitsData.TotalColumns ? "P" : "F";
                    DefectsColumnPassFail.ForeColor = DefectsColumnPassFail.Text.Equals("P") ? Color.LimeGreen : Color.Red;

                    DriftROIResult.Text = finalTestResultsDetailsModel.DriftROICount.ToString();
                    DefectsDriftROIPassFail.Text = finalTestResultsDetailsModel.DriftROICount <= defectsTestLimitsData.DriftROI ? "P" : "F";
                    DefectsDriftROIPassFail.ForeColor = DefectsDriftROIPassFail.Text.Equals("P") ? Color.LimeGreen : Color.Red;

                    DefectivePixelPerROIResult.Text = temp.Count().ToString();
                    DefectivePixelPerROIPassFail.Text = temp.Count() >0 ? "F" : "P";
                    DefectivePixelPerROIPassFail.ForeColor = DefectivePixelPerROIPassFail.Text.Equals("P") ? Color.LimeGreen : Color.Red;

                    RowDefectsResult.Text = finalTestResultsDetailsModel.TotalRowDefects.ToString();
                    DefectsRowPassFail.Text = finalTestResultsDetailsModel.TotalRowDefects <= defectsTestLimitsData.TotalRows ? "P" : "F";
                    DefectsRowPassFail.ForeColor = DefectsRowPassFail.Text.Equals("P") ? Color.LimeGreen : Color.Red;

                    if(ClustersPassFail.Text.Equals("P") && DeadPixelsPassFail.Text.Equals("P") && DarkPixelsPassFail.Text.Equals("P")
                        && HotPixelsPassFail.Text.Equals("P") && DefectsColumnPassFail.Text.Equals("P") && DefectsRowPassFail.Text.Equals("P")
                        && DefectsDriftROIPassFail.Text.Equals("P") && DefectivePixelPerROIPassFail.Text.Equals("P")
                        && !finalTestResultsDetailsModel.DefectsTestDeadDarkPixelLimitReached)
                    {
                        OverallDefectsTestPassFail.Text = DefectsTestPassFail.Text="P";
                        OverallDefectsTestPassFail.ForeColor = DefectsTestPassFail.ForeColor= Color.LimeGreen;
                    }
                    else
                    {
                        OverallDefectsTestPassFail.Text = DefectsTestPassFail.Text = "F";
                        OverallDefectsTestPassFail.ForeColor = DefectsTestPassFail.ForeColor = Color.Red;
                    }

                    if (DefPixelROIListPairs != null)
                    {                        
                        StringBuilder sb = new StringBuilder();
                        if(DefPixelROIListPairs.Count > 0)
                        {
                            foreach (KeyValuePair<string, List<Point>> roipixs in DefPixelROIListPairs)
                            {
                                sb.AppendLine(roipixs.Key.Trim() + "          " + roipixs.Value.Count.ToString());
                            }
                            labelMaskROI.Visible = true;
                            labelMaskROIData.Visible = true;
                            labelMaskROIData.Text = sb.ToString();
                            if(DefPixelROIListPairs.Count > 0)
                                labelMaskROIData.ForeColor = Color.Red;
                        }
                       
                    }
                    else if (finalTestResultsDetailsModel.DefectsTestDeadDarkPixelLimitReached)
                    {
                        labelMaskROI.Visible = true;
                        labelMaskROIData.Visible = true;
                        labelMaskROIData.Text = "Max Limit of " + defectsTestLimitsData.DeadDarkPixelsListReadLimit.ToString() +" Reached";
                        labelMaskROIData.ForeColor = Color.Red;
                        labelMaskROIData.Font = new Font(labelMaskROIData.Font, FontStyle.Bold);
                    }
                    else
                    {
                        labelMaskROI.Visible = false;
                        labelMaskROIData.Visible = false;
                    }

                }
                else
                {
                    ClustersResult.Text = "NA";
                    ClustersPassFail.Text = "NA";

                    DeadPixelsResult.Text = "NA";
                    DeadPixelsPassFail.Text= "NA";

                    DarkPixelsResult.Text = "NA";
                    DarkPixelsPassFail.Text = "NA";

                    HotPixelsResult.Text = "NA";
                    HotPixelsPassFail.Text = "NA";

                    ColumnDefectsResult.Text = "NA";
                    DefectsColumnPassFail.Text = "NA";
                    DefectsDriftROIPassFail.Text = "NA";

                    RowDefectsResult.Text = "NA";
                    DefectsRowPassFail.Text = "NA";

                    OverallDefectsTestPassFail.Text = "NA";
                    DefectsTestPassFail.Text = "NA";
                }                   
                if(selectedTests.Contains("MVT"))
                {
                    ConversionFactorNominalResult.Text = String.Format("{0:0.00}", finalTestResultsDetailsModel.ConversionFactorNominal);
                    ConversionFactorPassFail.Text = ((finalTestResultsDetailsModel.ConversionFactorNominal >= ecoData.MeanVarianceTestLimitsData.ToList()[0].ConversionFactorLowerLimit) &&
                   (finalTestResultsDetailsModel.ConversionFactorNominal <= ecoData.MeanVarianceTestLimitsData.ToList()[0].ConversionFactorUpperLimit)) ? "P" : "F";
                    ConversionFactorPassFail.ForeColor = ConversionFactorPassFail.Text.Equals("P") ? Color.LimeGreen : Color.Red;
                }
                else
                {
                    ConversionFactorNominalResult.Text = "NA";
                    ConversionFactorPassFail.Text = "NA";
                }
                //todo how to convert in ADU the below
                if (selectedTests.Contains("DDT"))
                {
                    AveTrapResult.Text = finalTestResultsDetailsModel.AveTrap.ToString();
                    MaxDarkROIResult.Text = String.Format("{0:0.00}", finalTestResultsDetailsModel.MaxDarkROI);
                    AveTrapResultInElectrons.Text = (finalTestResultsDetailsModel.AveTrap * finalTestResultsDetailsModel.ConversionFactorNominal).ToString();
                    MaxDarkROIResultInElectrons.Text = String.Format("{0:0.00}", finalTestResultsDetailsModel.MaxDarkROI * finalTestResultsDetailsModel.ConversionFactorNominal);
                    //todo
                    AveTrapPassFail.Text = "P";// finalTestResultsDetailsModel.AveTrap <= ecoData.DefectsTestLimitsData.ToList()[0].AveTrap ? "P" : "F";
                    AveTrapPassFail.ForeColor = AveTrapPassFail.Text.Equals("P") ? Color.LimeGreen : Color.Red;
                    //todo
                    MaxDarkROIPassFail.Text = (finalTestResultsDetailsModel.MaxDarkROI >= ecoData.DarkCurrentTestLimitsData.ToList()[0].DarkCurrentLowerLimit
                        && finalTestResultsDetailsModel.MaxDarkROI <= ecoData.DarkCurrentTestLimitsData.ToList()[0].DarkCurrentUpperLimit) ? "P" : "F";
                    MaxDarkROIPassFail.ForeColor = MaxDarkROIPassFail.Text.Equals("P") ? Color.LimeGreen : Color.Red;
                }
                else
                {
                    AveTrapResult.Text = "NA";
                    MaxDarkROIResult.Text = "NA";
                    AveTrapResultInElectrons.Text = "NA";
                    MaxDarkROIResultInElectrons.Text = "NA";                   
                    AveTrapPassFail.Text = "NA";
                    MaxDarkROIPassFail.Text = "NA";
                }
                if (selectedTests.Contains("PRT"))
                {
                    FullWellLevelMinResult.Text = finalTestResultsDetailsModel.FullWellLevelMin.ToString();
                    FullWellLevelMinResultInElectrons.Text = (Convert.ToInt32(finalTestResultsDetailsModel.FullWellLevelMin * finalTestResultsDetailsModel.ConversionFactorNominal)).ToString();
                    FullWellLevelMinPassFail.Text = finalTestResultsDetailsModel.FullWellLevelMin >= ecoData.PhotoresponseLimitsData.ToList()[0].FullWellLevelMin ? "P" : "F";
                    FullWellLevelMinPassFail.ForeColor = FullWellLevelMinPassFail.Text.Equals("P") ? Color.LimeGreen : Color.Red;
                }
                else
                {
                    FullWellLevelMinResult.Text = "NA";
                    FullWellLevelMinResultInElectrons.Text = "NA";
                    FullWellLevelMinPassFail.Text = "NA";
                }                  
                if(selectedTests.Contains("RNT"))
                {
                    SnglNoiseMaxResult.Text = Convert.ToInt32(finalTestResultsDetailsModel.SnglNoiseMax).ToString();
                    ReadNoiseBestFitPowerResult.Text = finalTestResultsDetailsModel.PowerFitLower.ToString() + "< Power <" + finalTestResultsDetailsModel.PowerFitUpper.ToString();//todo
                    RPower2CorrelationResult.Text = finalTestResultsDetailsModel.RPower2Correlation.ToString();
                    SnglNoiseMaxResultInElectrons.Text = String.Format("{0:0.00}", ((Convert.ToInt32(finalTestResultsDetailsModel.SnglNoiseMax) * finalTestResultsDetailsModel.ConversionFactorNominal)));
                    SnglNoiseMaxPassFail.Text = (finalTestResultsDetailsModel.SnglNoiseMax <= ecoData.NoiseVsNDROSLimitsData.ToList()[0].SnglNoiseUpperLimit
                   && finalTestResultsDetailsModel.SnglNoiseMax >= ecoData.NoiseVsNDROSLimitsData.ToList()[0].SnglNoiseLowerLimit) ? "P" : "F";
                    SnglNoiseMaxPassFail.ForeColor = SnglNoiseMaxPassFail.Text.Equals("P") ? Color.LimeGreen : Color.Red;
                    //todo
                    ReadNoiseBestFitPowerPassFail.Text = "P";
                    ReadNoiseBestFitPowerPassFail.ForeColor = ReadNoiseBestFitPowerPassFail.Text.Equals("P") ? Color.LimeGreen : Color.Red;
                    RPower2CorrelationPassFail.Text = finalTestResultsDetailsModel.RPower2Correlation >= ecoData.NoiseVsNDROSLimitsData.ToList()[0].RPower2Correlation ? "P" : "F";
                    RPower2CorrelationPassFail.ForeColor = RPower2CorrelationPassFail.Text.Equals("P") ? Color.LimeGreen : Color.Red;
                }
                else
                {
                    SnglNoiseMaxResult.Text = "NA";
                    ReadNoiseBestFitPowerResult.Text = "NA";
                    RPower2CorrelationResult.Text = "NA";
                    SnglNoiseMaxResultInElectrons.Text = "NA";
                    SnglNoiseMaxPassFail.Text = "NA";                  
                    ReadNoiseBestFitPowerPassFail.Text = "NA";
                    RPower2CorrelationPassFail.Text = "NA";
                }
                if (selectedTests.Contains("SDT") && shutterDriveTestResults!=null)
                {
                    lblLabbJackSN.Text = shutterDriveTestResults.LabJackSN;
                    ShutterDrivePassFail.Text = finalTestResultsDetailsModel.ShutterDrive ? "P" : "F";
                    ShutterDrivePassFail.ForeColor = finalTestResultsDetailsModel.ShutterDrive ? Color.LimeGreen : Color.Red;
                }
                else
                {
                    lblLabbJackSN.Text = "NA";
                    ShutterDrivePassFail.Text = "NA";
                }
                if (cameraBurnInData != null)
                {
                    if (cameraBurnInData.BurnInResult)
                    {
                        lblBurnInResult.Text = "P";
                        lblBurnInResult.ForeColor = Color.LimeGreen;
                    }
                    else
                    {
                        lblBurnInResult.Text = "F";
                        lblBurnInResult.ForeColor = Color.Red;
                    }
                }
                else
                {
                    lblBurnInResult.Text = "-";
                    lblBurnInResult.ForeColor = Color.Black;
                }  
                UpdateTestSettingsView(cameraDetailsModel, cameraInformation);
                Tester.Text = string.IsNullOrWhiteSpace(userExecuted) ? Environment.UserName : userExecuted;
                TestRunDate.Text = dateExecuted.Date.ToShortDateString();
                TestRunTime.Text = dateExecuted.ToString("HH:mm:ss");
                camTempOutofRange = cameraTempOutOfRange;
                camCurrentOutofRange = cameraCurrentFailed;

                if (finalTest)
                {
                    string camTemperatureLimit = string.Format("{0} {1} {2} {3} {4}", "-", temperatureHumidityLimitsData.MinTemp.ToString("F2"),
                                                        " to ", "-", temperatureHumidityLimitsData.MaxTemp.ToString("F2"));
                    camTempLimit.Text = camTemperatureLimit;


                    string camCurrLimit = string.Format("{0} {1} {2}", environmentalStatusLimitsData.AmpsLowerLimit.ToString("F2"),
                                                        " - ", environmentalStatusLimitsData.AmpsUpperLimit.ToString("F2"));
                    camCurrentLimit.Text = camCurrLimit;
                    camTempAvgValue.Text = Temperature.Text;
                    if (cameraTempOutOfRange)
                    {
                        camTempAvgResult.Text = "F";
                        camTempAvgResult.ForeColor = Color.Red;
                        finalTestReporCameraTemptNote.Visible = true;                       
                    }
                    else
                    {
                        camTempAvgResult.Text = "P";
                        camTempAvgResult.ForeColor = Color.LimeGreen;
                        firmwareErrWarningLabel.Visible = false;
                    }

                    if ((bool)cameraCurrentFailed)
                    {
                        camCurrentAvgResult.Text = "F";
                        camCurrentAvgResult.ForeColor = Color.Red;
                        camCurrentResultNote.Visible = true;
                    }
                    else
                    {
                        camCurrentAvgResult.Text = "P";
                        camCurrentAvgResult.ForeColor = Color.LimeGreen;
                        firmwareErrWarningLabel.Visible = false;
                    }
                }
                else
                {
                    finalTestReporCameraTemptNote.Visible= firmwareErrWarningLabel.Visible= false;
                    camTempAvgResult.Text = camCurrentAvgResult.Text = camCurrentAvgValue.Text = "NA";
                }

                if (firmwareErrorsWarnings != null && firmwareErrorsWarnings.Count > 0)
                {
                    firmwareErrs = true;
                       StringBuilder firmwareErrors = new StringBuilder();
                    foreach(string errorStr in firmwareErrorsWarnings)
                    {
                        firmwareErrors.AppendLine(errorStr);
                    }
                    firmwareErrWarningLabel.Visible = true;
                    firmwareErrWarningLabel.Text = "=> Test has been failed as the firmware has returned the below Errors/Warnings" + Environment.NewLine + firmwareErrors;
                }
                else
                {
                    firmwareErrWarningLabel.Visible = false;
                }

                if (environmentalStatusForm != null)
                {
                    camCurrentAvgValue.Text = camCurrentAvg;
                    if (coolantTempFailDuringFinalTest)
                    {
                        envStatusWaterChillerWarning.Visible = true;                       
                    }
                    else
                    {
                        envStatusWaterChillerWarning.Visible = false;
                    }

                    if (purgeFlowRateFailDuringFinalTest)
                    {
                        envStatusPurgeFlowWarning.Visible = true;                       
                    }
                    else
                    {
                        envStatusPurgeFlowWarning.Visible = false;
                    }
                }
                else
                {
                    envStatusWaterChillerWarning.Visible = false;
                    envStatusPurgeFlowWarning.Visible = false;
                }

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }           
        }
        private void UpdateLimitsOnView(ECOData ecoData,bool finalTest, List<DefectsTestLimitsData> defectsTestLimitsDataList)
        {
            try
            {

                DefectsTestLimitsData defectsTestLimitsData = new DefectsTestLimitsData();
                if (!finalTest)
                {
                    defectsTestLimitsData = ecoData.DefectsTestLimitsData.Where(d => d.TestStage.Equals("PRE_TEST")).FirstOrDefault();
                }
                else
                {
                    defectsTestLimitsData = ecoData.DefectsTestLimitsData.Where(d => d.TestStage.Equals("FINAL_TEST")).FirstOrDefault();
                }

                if (defectsTestLimitsData == null)
                {
                    if (!finalTest)
                        defectsTestLimitsData = defectsTestLimitsDataList.Where(limit => limit.TestStage.Equals("PRE_TEST")).FirstOrDefault();
                    else
                        defectsTestLimitsData = defectsTestLimitsDataList.Where(limit => limit.TestStage.Equals("FINAL_TEST")).FirstOrDefault();
                }
                ClustersLimit.Text = defectsTestLimitsData.TotalNumClusters.ToString();
                DeadPixelsLimit.Text = defectsTestLimitsData.DeadPixel.ToString();
                HotPixelsLimit.Text = defectsTestLimitsData.HotPixel.ToString();
                DarkPixelsLimit.Text = defectsTestLimitsData.DarkPixel.ToString();
                DefectsRowLimit.Text = defectsTestLimitsData.TotalRows.ToString();
                DefectsColumnLimit.Text = defectsTestLimitsData.TotalColumns.ToString();
                driftROILimit.Text = defectsTestLimitsData.DriftROI.ToString();
               // DefectivePixelPerROILImit.Text = ecoData.DefectsTestLimitsData.ToList()[0].DefectivePixelsPerROI.ToString();
                ConversionFactorNominalLimit.Text = ecoData.MeanVarianceTestLimitsData.ToList()[0].ConversionFactorNominal.ToString();
                AveTrapLimit.Text = defectsTestLimitsData.AveTrap.ToString();
                MaxDarkROILimit.Text = ecoData.DarkCurrentTestLimitsData.ToList()[0].DarkCurrentMaxDarkROI.ToString();
                FullWellLevelMinLimit.Text = ecoData.PhotoresponseLimitsData.ToList()[0].FullWellLevelMin.ToString();
                ReadNoiseNDROLimit.Text = ecoData.NoiseVsNDROSLimitsData.ToList()[0].SnglNoiseMax.ToString();
                ReadNoiseBestFitPowerLimit.Text = ecoData.NoiseVsNDROSLimitsData.ToList()[0].PowerFitLower.ToString() + "< Power <" + TestAppHelper.PowerFitUpper.ToString();
                RPower2CorrelationLimit.Text = ecoData.NoiseVsNDROSLimitsData.ToList()[0].RPower2Correlation.ToString();
                InjEffFirstInjLimit.Text = ecoData.InjectionEfficiencyTestLimitsData.ToList()[0].InjectionEffLimit.ToString();
                InjEffLastInjLimit.Text = ecoData.InjectionEfficiencyTestLimitsData.ToList()[0].InjectionEffLimit.ToString();
                RedBlueUVLimit.Text = ecoData.LEDCalibrationLimitsData.ToList()[0].LowerUVLEDLimit.ToString() + " To " + ecoData.LEDCalibrationLimitsData.ToList()[0].UpperUVLEDLimit.ToString();
                XROIRangeLimit.Text = defectsTestLimitsData.XROIRange.ToString();
                YROIRangeLimit.Text = defectsTestLimitsData.YROIRange.ToString();
                defectivePixelPerROI.Text = defectsTestLimitsData.DefectivePixelsPerROI.ToString();
                darkPixelThreshold.Text = defectsTestLimitsData.DarkPixelThreshold.ToString();
                BadPixelsInRowLimit.Text = defectsTestLimitsData.BadPixelsInRow.ToString();
                BadPixelsInColLimit.Text = defectsTestLimitsData.BadPixelsInCol.ToString();

                //labelMaskROI.Text = "# Mask ROI's >  " + defectivePixelPerROI.Text +" Def Pixs";
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateTestSettingsView(CameraDetailsModel cameraDetailsModel, CameraInformation cameraInformation)
        {
            try
            {
                CameraSN.Text = cameraDetailsModel.CameraSerialNumber;
                ImagerSN.Text = cameraDetailsModel.ImagerSerialNumber;
                DetectorType.Text = cameraDetailsModel.DetectorType;              
                TestSWVersion.Text = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString("yyMMdd");//ConfigurationManager.AppSettings["SCMSoftwareVersion"];
                
                FirmwareVersion.Text = cameraDetailsModel.FirmwareVer;
                FirmwareFile.Text = cameraDetailsModel.FirmwareFile;
                CSPBoardRev.Text = cameraDetailsModel.CSPSerialNumber;
                PowerBoardRev.Text = cameraDetailsModel.PowerSerialNumber;
                CPUBoardRev.Text = cameraDetailsModel.CPUSerialNumber;
                MACAddress.Text = new string(cameraInformation.MACAddress);
                FPGAVersion.Text = cameraInformation.FPGAVersion.ToString();
                ParametersModel parametersModel = TestAppHelper.GetParametersModel();
                if(parametersModel !=null)
                {
                    DatabaseVersion.Text = parametersModel.DatabaseVersion;
                    //TestSWVersion.Text = parametersModel.SoftwareVersion;
                    ECONumber.Text = parametersModel.ECONumber;
                    MaskROIsVersion.Text = parametersModel.MaskROIsVersion;
                }
                if(preTest)
                {
                    ultraLabelLimitID.Text = sortedECOData[0].DefectsTestLimitsData.ToList()[1].LimitsID.ToString();
                }
                else
                {
                    ultraLabelLimitID.Text = sortedECOData[0].DefectsTestLimitsData.ToList()[0].LimitsID.ToString();
                }
                if (!string.IsNullOrEmpty(cameraDetailsModel.CRA))
                {
                    CRA.Visible = true;
                    CRALabel.Visible = true;
                    CRA.Text = cameraDetailsModel.CRA;
                }
                else
                {
                    CRA.Visible = false;
                    CRALabel.Visible = false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public string SaveFinalTestReportAsImage(string testResultLocalFilePath, string testResultsServerPath, string localFileTimeStamp)
        {
            try
            {
                this.Dock = DockStyle.None;
                string testReportFolder = string.Empty;
                if (!preTest)
                    testReportFolder = "\\FinalTestReport_";
                else
                    testReportFolder = "\\PreTestReport_";
                servPath = testResultLocalFilePath + testReportFolder + localFileTimeStamp + ".png";
                linkLabel1.Text="Report Location: " + testResultLocalFilePath;
                linkLabel1.LinkArea = new LinkArea(17, testResultLocalFilePath.Length);
                Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
                finalReportBitmap = new Bitmap(this.Width, this.Height);
                this.DrawToBitmap(finalReportBitmap, rect);
                using (Graphics graphics = Graphics.FromImage(finalReportBitmap))
                {
                    graphics.DrawImage(finalReportBitmap, new Rectangle(new Point(), finalReportBitmap.Size),
                        new Rectangle(new Point(), finalReportBitmap.Size), GraphicsUnit.Pixel);
                }
                finalReportBitmap.Save(testResultLocalFilePath + testReportFolder + localFileTimeStamp + ".png");
                MemoryStream ms = new MemoryStream();
                finalReportBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                finalReportBitmap.Save(testResultLocalFilePath + testReportFolder + localFileTimeStamp + ".png", System.Drawing.Imaging.ImageFormat.Png);
                this.Dock = DockStyle.Fill;
                if (ConfigurationManager.AppSettings["EndClient"] != "Thermo")
                {
                    string imageName = string.Empty;
                    if (!preTest)
                        imageName = "FinalTestReport.png";
                    else
                        imageName = "PreTestReport.png";
                    TestAppHelper.saveFileInAzure((Image)finalReportBitmap, imageName);
                    return string.Empty;
                }
                else
                {
                    finalReportBitmap.Save(testResultsServerPath + testReportFolder + localFileTimeStamp + ".png", System.Drawing.Imaging.ImageFormat.Png);
                    return testResultsServerPath + testReportFolder + localFileTimeStamp + ".png";
                }                
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("Explorer", "/select," + servPath);
            } 
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }       
    }
}
