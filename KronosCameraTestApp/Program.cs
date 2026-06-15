using KronosCameraTestApp.Model;
using KronosCameraTestApp.Presenter;
using KronosCameraTestApp.Properties;
using KronosCameraTestApp.View;
using KronosCameraTestApp.View.UserControls;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//Here is the once-per-application setup information
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace KronosCameraTestApp
{
    static class Program
    {
        static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8B}");
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                    KronosTestAppMainForm kronosTestAppMainForm = new KronosTestAppMainForm();
                    UserLoginForm userLoginForm = new UserLoginForm();
                    UserLoginModel userLoginModel = new UserLoginModel();
                    UserLoginPresenter userLoginPresenter = new UserLoginPresenter(userLoginForm,userLoginModel, kronosTestAppMainForm);
                    MeanVarianceTestForm meanVarianceTestForm = new MeanVarianceTestForm();
                    MeanVariancePresenter meanVariancePresenter = new MeanVariancePresenter(meanVarianceTestForm, kronosTestAppMainForm);
                    DarkCurrentTestForm darkCurrentTestForm = new DarkCurrentTestForm();
                    DarkCurrentPresenter darkCurrentPresenter = new DarkCurrentPresenter(darkCurrentTestForm, kronosTestAppMainForm);
                    DefectsTestForm defectsTestForm = new DefectsTestForm();
                    DefectsPresenter defectsPresenter = new DefectsPresenter(defectsTestForm, kronosTestAppMainForm);
                    NoiseVsNDROTestForm noiseVsNDROTestForm = new NoiseVsNDROTestForm();
                    NoiseVsNDROPresenter noiseVsNDROPresenter = new NoiseVsNDROPresenter(noiseVsNDROTestForm, kronosTestAppMainForm);
                    InjectionEfficiencyTestForm injectionPerformanceTestForm = new InjectionEfficiencyTestForm();
                    InjectionEfficiencyPresenter injectionPerformancePresenter = new InjectionEfficiencyPresenter(injectionPerformanceTestForm, kronosTestAppMainForm);
                    PhotoresponseTestForm photoresponseTestForm = new PhotoresponseTestForm();
                    PhotoresponsePresenter photoresponsePresenter = new PhotoresponsePresenter(photoresponseTestForm, kronosTestAppMainForm);
                    RedBlueTestForm redBlueTestForm = new RedBlueTestForm();
                    RedBluePresenter redBluePresenter = new RedBluePresenter(redBlueTestForm, kronosTestAppMainForm);
                    ShutterDriveTestForm shutterDriveAndRS232TestForm = new ShutterDriveTestForm();
                    ShutterDrivePresenter shutterDrivePresenter = new ShutterDrivePresenter(shutterDriveAndRS232TestForm, kronosTestAppMainForm);
                    MeanVarianceTestLimitsData meanVarianceTestLimitsData = new MeanVarianceTestLimitsData();
                    MeanVarianceLimitsPresenter meanVarianceTestLimitsPresenter = new MeanVarianceLimitsPresenter(meanVarianceTestLimitsData, kronosTestAppMainForm);
                    MeanVarianceLimitsUserControl meanVarianceTestLimitsUserControl = new MeanVarianceLimitsUserControl(meanVarianceTestLimitsPresenter);
                    DarkCurrentTestLimitsData darkCurrentTestLimitsData = new DarkCurrentTestLimitsData();
                    DarkCurrentLimitsPresenter darkCurrentTestLimitsPresenter = new DarkCurrentLimitsPresenter(darkCurrentTestLimitsData, kronosTestAppMainForm);
                    DarkCurrentLimitsUserControl darkCurrentTestLimitsUserControl = new DarkCurrentLimitsUserControl(darkCurrentTestLimitsPresenter);
                    DefectsTestLimitsData defectsTestLimitsData = new DefectsTestLimitsData();
                    DefectsLimitsPresenter defectsTestLimitsPresenter = new DefectsLimitsPresenter(defectsTestLimitsData, kronosTestAppMainForm);
                    DefectsLimitsUserControl defectsTestUserControl = new DefectsLimitsUserControl(defectsTestLimitsPresenter);
                    InjectionEfficiencyTestLimitsData injectionEfficiencyTestLimitsData = new InjectionEfficiencyTestLimitsData();
                    InjectionEfficiencyLimitsPresenter injectionEfficiencyTestLimitsPresenter = new InjectionEfficiencyLimitsPresenter(injectionEfficiencyTestLimitsData, kronosTestAppMainForm);
                    InjectionEfficiencyLimitsUserControl injectionEfficiencyTestLimitsUserControl = new InjectionEfficiencyLimitsUserControl(injectionEfficiencyTestLimitsPresenter);
                    InjectionPerformanceLimitsData injectionPerformanceLimitsData = new InjectionPerformanceLimitsData();
                    InjectionPerformanceLimitsPresenter injectionPerformanceTestLimitsPresenter = new InjectionPerformanceLimitsPresenter(injectionPerformanceLimitsData, kronosTestAppMainForm);
                    InjectionPerformanceLimitsUserControl injectionPerformanceTestLimitsUserControl = new InjectionPerformanceLimitsUserControl(injectionPerformanceTestLimitsPresenter);
                    NdroReadDriftTestLimitsData ndroReadDriftTestLimitsData = new NdroReadDriftTestLimitsData();
                    NDROReadDriftLimitsPresenter nDROReadDriftTestLimitsPresenter = new NDROReadDriftLimitsPresenter(ndroReadDriftTestLimitsData, kronosTestAppMainForm);
                    NDROReadDriftLimitsUserControl nDROReadDriftTestLimitsUserControl = new NDROReadDriftLimitsUserControl(nDROReadDriftTestLimitsPresenter);
                    NoiseVsNDROSLimitsData noiseVsNDROSLimitsData = new NoiseVsNDROSLimitsData();
                    NoiseVsNDROSLimitsPresenter noiseVsNDROSLimitsPresenter = new NoiseVsNDROSLimitsPresenter(noiseVsNDROSLimitsData, kronosTestAppMainForm);
                    NoiseVsNDROSLimitsUserControl noiseVsNDROSLimitsUserControl = new NoiseVsNDROSLimitsUserControl(noiseVsNDROSLimitsPresenter);
                    PhotoresponseLimitsData photoresponseLimitsData = new PhotoresponseLimitsData();
                    PhotoresponseLimitsPresenter photoresponseLimitsPresenter = new PhotoresponseLimitsPresenter(photoresponseLimitsData, kronosTestAppMainForm);
                    PhotoresponseLimitsUserControl photoresponseLimitsUserControl = new PhotoresponseLimitsUserControl(photoresponseLimitsPresenter);
                    SegmentInjectLimitsData segmentInjectLimitsData = new SegmentInjectLimitsData();
                    SegmentInjectLimitsPresenter segmentInjectLimitsPresenter = new SegmentInjectLimitsPresenter(segmentInjectLimitsData, kronosTestAppMainForm);
                    SegmentInjectLimitsUserControl segmentInjectLimitsUserControl = new SegmentInjectLimitsUserControl(segmentInjectLimitsPresenter);
                    TempHumidPresenter tempHumidPresenter = new TempHumidPresenter(kronosTestAppMainForm);
                    TemperatureHumidityLimitsData temperatureLimitsData = new TemperatureHumidityLimitsData();
                    LEDCalibrationTest lEDCalibrationTest = new LEDCalibrationTest();
                    LEDCalibrationTestResults ledCalibrationTestResults = new LEDCalibrationTestResults();
                    LEDCalibrationPresenter lEDCalibrationPresenter = new LEDCalibrationPresenter(lEDCalibrationTest, ledCalibrationTestResults, kronosTestAppMainForm);
                                 
                    CharacterizationTestLimits characterizationTestLimits = new CharacterizationTestLimits(meanVarianceTestLimitsPresenter,                                                                                                            
                                                                                                           darkCurrentTestLimitsPresenter, defectsTestLimitsPresenter,
                                                                                                           injectionEfficiencyTestLimitsPresenter, injectionPerformanceTestLimitsPresenter,
                                                                                                           nDROReadDriftTestLimitsPresenter, noiseVsNDROSLimitsPresenter,
                                                                                                           photoresponseLimitsPresenter, segmentInjectLimitsPresenter,
                                                                                                           lEDCalibrationPresenter, tempHumidPresenter);
                    OverAllTestResultsModel overAllTestResultsModel = new OverAllTestResultsModel();
                    OverAllTestResultsForm overAllTestResultsForm = new OverAllTestResultsForm();
                    OverAllTestResultsPresenter overAllTestResultsPresenter = new OverAllTestResultsPresenter(overAllTestResultsModel, overAllTestResultsForm, kronosTestAppMainForm);
                    TestResultsForm testResultsForm = new TestResultsForm(meanVariancePresenter, 
                                                                           darkCurrentPresenter, defectsPresenter, injectionPerformancePresenter, noiseVsNDROPresenter,
                                                                           photoresponsePresenter, redBluePresenter, shutterDrivePresenter, overAllTestResultsPresenter);
                    ImageDisplayPanel ImageDisplayPanel = new ImageDisplayPanel();
                    ImageDisplayPanelPresenter imageDisplayPanelPresenter = new ImageDisplayPanelPresenter(ImageDisplayPanel, kronosTestAppMainForm);
                    CameraDetailsModel cameraDetailsModel = new CameraDetailsModel();
                    OperatorInputForm operatorInputform = new OperatorInputForm();
                    CameraDetailsPresenter cameraDetailsPresenter = new CameraDetailsPresenter(operatorInputform, kronosTestAppMainForm);
                    CameraTestLogDetails cameraTestLogDetails = new CameraTestLogDetails();
                    CameraTestLogsModel cameraTestLogs = new CameraTestLogsModel();
                    CameraTestLogsPresenter cameraTestLogsPresenter = new CameraTestLogsPresenter(cameraTestLogDetails, cameraTestLogs,kronosTestAppMainForm);
                    EnvironmentalStatus environmentalStatus = new EnvironmentalStatus(); 
                    EnvironmentalStatusPresenter environmentalStatusPresenter = new EnvironmentalStatusPresenter(environmentalStatus, kronosTestAppMainForm);                   
                    Database.SetInitializer<KronosCamContext>(null);
                    Application.Run(kronosTestAppMainForm);
                }
                catch(Exception ex)
                {
                    log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
            else
            {
                MessageBox.Show("SCM5821AX1 Test App instance is already running!");
            }
        }
        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                log.Error("Thread Exception in SCM5821AX1 Test Application" + e.Exception.Message);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                log.Error("Thread Exception in SCM5821AX1 Test App" + e.Exception.Message);
            }
        }
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                log.Error("Domain Error: " + e.ToString() + "Error propagated to Main Program.");
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                log.Error("Domain Error: " + e.ToString() + "Error propagated to Main Program.");
                log.Error(string.Format("*** UNHANDLED APPDOMAIN EXCEPTION ({0}) *****", e.IsTerminating ? "Terminating" : "Non-Terminating"), e.ExceptionObject as Exception);            
            if (ex.InnerException !=null)
                    log.Error("Domain Error Innerexception: " + ex.InnerException.ToString() + "Error propagated to Main Program.");
            }
        }
    }
}
