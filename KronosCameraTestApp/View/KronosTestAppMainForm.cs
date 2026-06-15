using Infragistics.Win.Misc;
using Infragistics.Win.UltraMessageBox;
using Infragistics.Win.UltraWinToolbars;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.Presenter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Thermo.Kronos.Instrument.Camera.Contracts.Data;
using Thermo.Kronos.Instrument.Camera.Interface;
using System.Configuration;
using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.View.UserControls;
using CIDSoftwareApplication;
using KronosCameraTestApp.Properties;
using System.Net.NetworkInformation;
using System.Security.AccessControl;
using System.IO.Ports;

namespace KronosCameraTestApp.View
{
    public partial class KronosTestAppMainForm : Form
    {
        #region fields
        //test comment
        bool stylePath = false;
        //string finaltestFirmwareOutputPath = string.Empty;
        string assemblyPath = string.Empty;
        EnvironmentalStatus environmentalStatusForm { get; set; }
        TestStationCalibrationReminder testStationCalibrationReminder { get; set; }
        AutoTestForm autoTestForm { get; set; }
        AutoPretestForm autoPretestForm { get; set; }
        CIDInterface cidInterface;
        int firmwareId = 0;
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        log4net.Core.Level logLevel;
        System.Diagnostics.PerformanceCounter availableRamCounter;
        CID821_Data cid821Data = new CID821_Data();
        public List<CID821_Data> cidDataList = new List<CID821_Data>();
        string ImageFilePath = string.Empty;
        string scriptFileName = string.Empty;
        string saveAsFilename = string.Empty;
        bool autoTest = false;
        public  bool AutoTestStatus = false;
        public   bool AutoPreTestStatus = false;
        bool prestTestResultsSaved = false;
        int lastPrintX;
        int lastPrintY;
        string currentRunningTest = string.Empty;
        string userMode = string.Empty;           
        Bitmap memoryImage;
        Bitmap finalReportBitmap;
        PrintDocument printDocument1 = new PrintDocument();
        bool fwLoggingEnabled = false;
        int LocationX;
        int LocationY;
        public string defaultFilePath = string.Empty;
        bool pretestStatus = false;
        bool loginStatus = false;       
        string enggUserName = string.Empty;
        int expCount = 0;
        string testResultsPath = string.Empty;
        int exposureCount = 0;
        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        SCM5821AX1DataTypes.VideoStruct videoPacket = new SCM5821AX1DataTypes.VideoStruct();
        DirectorySecurity securityRules { get; set; }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);
        private static uint xArraySize = 2048;
        private static uint yArraySize = 2048;
        private int integratedModeStartIndex = 0;
        private int integratedSequenceNumber = 1;
        private double maxSignal = 65535;
        private double minSignal = 0;
        string theStyleFile = string.Empty;
        bool AutoTestStatusResult = false;
        bool AutoPreTestStatusResult = false;
        bool cameraConnected = false;
        private double[,] zPatternCorrectedData = new double[xArraySize, yArraySize];
        public double[,] ZPatternCorrectedData
        {
            get { return zPatternCorrectedData; }
            set { zPatternCorrectedData = value; }
        }
        private const int BLACK_LEVEL = 0;
        public ExposureDataPresenter mainPresenter { get; set; }
        MeanVarianceTestForm meanVarianceTestForm { get; set; }
        MeanVarianceUserControl meanVarianceTestUserControl { get; set; }
        public MeanVariancePresenter meanVariancePresenter { get; set; }
        DarkCurrentTestForm darkCurrentTestForm { get; set; }
        DarkCurrentUserControl darkCurrentTestUserControl { get; set; }
        public DarkCurrentPresenter darkCurrentPresenter { get; set; }
        DefectsTestForm defectsTestForm { get; set; }
        DefectsUserControl defectsUserControl { get; set; }
        public DefectsPresenter defectsPresenter { get; set; }
        NoiseVsNDROTestForm noiseVsNDROTestForm { get; set; }
        NoiseVsNDROUserControl noiseVsNDROTestUserControl { get; set; }
        public NoiseVsNDROPresenter noiseVsNDROPresenter { get; set; }
        InjectionEfficiencyTestForm injectionEfficiencyTestForm { get; set; }
        InjectionEfficiencyUserControl injectionPerformanceTestUserControl { get; set; }
        public InjectionEfficiencyPresenter injectionEfficiencyPresenter { get; set; }
        PhotoresponseTestForm photoresponseTestForm { get; set; }
        PhotoresponseUserControl photoresponseTestUserControl { get; set; }
        public PhotoresponsePresenter photoresponsePresenter { get; set; }
        ShutterDriveTestForm shutterDriveTestForm { get; set; }
        ShutterDriveUserControl shutterDriveUserControl { get; set; }
        public ShutterDrivePresenter shutterDrivePresenter { get; set; }
        RedBlueTestForm redBlueTest { get; set; }
        RedBlueUserControl redBlueUserControl { get; set; }
        public RedBluePresenter redBluePresenter { get; set; }
        FinalTestReportUserControl finalTestReportUserControl { get; set; }
        public MeanVarianceLimitsPresenter meanVarianceTestLimitsPresenter { get; set; }
        public DarkCurrentLimitsPresenter darkCurrentTestLimitsPresenter { get; set; }
        public DefectsLimitsPresenter defectsTestLimitsPresenter { get; set; }
        public InjectionEfficiencyLimitsPresenter injectionEfficiencyTestLimitsPresenter { get; set; }
        public InjectionPerformanceLimitsPresenter injectionPerformanceTestLimitsPresenter { get; set; }
        public NDROReadDriftLimitsPresenter nDROReadDriftTestLimitsPresenter { get; set; }
        public NoiseVsNDROSLimitsPresenter noiseVsNDROSLimitsPresenter { get; set; }
        public PhotoresponseLimitsPresenter photoresponseLimitsPresenter { get; set; }
        public TempHumidPresenter tempHumidPresenter { get; set; }
        public SegmentInjectLimitsPresenter segmentInjectLimitsPresenter { get; set; }       
        public UserLoginPresenter userLoginPresenter { get; set; }
        public OverAllTestResultsPresenter overAllTestResultsPresenter { get; set; }
        public CameraDetailsPresenter cameraDetailsPresenter { get; set; }
        public CameraTestLogsPresenter cameraTestLogsPresenter { get; set; }
        public LEDCalibrationPresenter ledCalibrationPresenter { get; set; }

        public EnvironmentalStatusPresenter environmentalStatusPresenter { get; set; }
        TemperatureGraphForm temperatureGraphForm { get; set; }
        CancellationTokenSource AutoCancellationTokenSource { get; set; }
        InstrumentDefaults instrumentDefaults { get; set; }
        OperatorInputForm operatorInputForm { get; set; }
        TCPSettingsForm tcpSettingsForm { get; set; }
        DashBoard dashBoardForm { get; set; }
        DashBoardAutoForm dashBoardAutoForm { get; set; }
        DashBoardAutoPretestForm dashBoardAutoPretestForm { get; set; }       
        FinalTestReportForm finalTestReportForm { get; set; }
        FinalTestResultsDetailsModel finalTestResultsDetailsModel { get; set; }
        CameraDetailsModel cameraDetailsModel { get; set; }
        RedBlueTestResults redBlueTestResults { get; set; }
        MeanVarianceTestResultsData meanVarianceTestResultsData { get; set; }      
        NoiseVsNDROTestResults noiseVsNDROTestResults { get; set; }
        DarkCurrentTestResults darkCurrentTestResults { get; set; }
        DefectsTestResults defectsTestResults { get; set; }
        InjectionEfficiencyTestResults injectionEfficiencyTestResults { get; set; }
        PhotoresponseTestResults photoresponseTestResults { get; set; }
        ShutterDriveTestResults shutterDriveTestResults { get; set; }      
        UserLoginForm loginForm { get; set; }
        Logger logger { get; set; }
        AboutForm aboutForm { get; set; }
        ImageDisplayPanel imageDisplayPanel { get; set; }
        public ImageDisplayPanelPresenter imageDisplayPanelPresenter { get; set; }
        TestGraphPreferences testGraphPreferences { get; set; }
        private CameraInformation cameraInformation;
        public List<CID821_Data> cidIntegratedDataList = new List<CID821_Data>();
        UpdateBurnInFiles updateBurnInFiles { get; set; }
        StringBuilder autoTestFailReason { get; set; }
        string strTestfailed = string.Empty;
        string ImagerSerialNumber = string.Empty;
        int progressNum = 0;
        int progressPretestNum = 0;
        int pretestexposureCount = 0;
        NetworkInterface ethernetAdapter;
        long ethernetSpeed = 0;
       
        string tempHumfiletimestamp = string.Empty;
        List<double> camTemp = new List<double>();
        List<double> camHum = new List<double>();
        //List<double> camCurrent=null;
        List<double> camHumdityListForDBLogging =null;
        List<Tuple<double, double>> camTempHumListForDBLogging = null;
        List<Tuple<double, int>> camCurrent = null;
        List<bool?> coolantTempsDuringFinalTest = null;
        List<bool?> purgeFlowRatesDuringFinalTest = null;
        bool beginCamCurrentAvg = false;
        double camTempDBLoggingAvg { get; set; }
        double camHumidityDBLoggingAvg { get; set; }
        double camCurrentAvg { get; set; }
        List<string> tempLogDateTime = new List<string>();
        static bool tempfilelogged = false;
        CameraStatus cameraStatusForLogTemHum { get; set; }
        System.Windows.Forms.Timer networkSpeedTimer = new System.Windows.Forms.Timer { Interval = 1000 };
        System.Windows.Forms.Timer logTempHumTimer = new System.Windows.Forms.Timer { Interval = 60000 };
       // System.Windows.Forms.Timer ledCalibTestTimer = new System.Windows.Forms.Timer { Interval = 5000 };
        System.Windows.Forms.Timer camTempAutoTestAlertTimer = new System.Windows.Forms.Timer { Interval = 1000 };
        string burnInDataFilesServerPath = string.Empty;
        CancellationTokenSource checkTCPcancellationTokenSource = new CancellationTokenSource();       
        BurnInAnalysis burnInAnalysis { get; set; }
        BurnInAnalysisModel cameraBurnInData { get; set; }        
        public string UserMode
        {
            get { return userMode; }
            set { userMode = value; }
        }         
        DialogResult camTempAlertResult = DialogResult.None;
        DialogResult camTempAlertResultFor20 = DialogResult.None;
        bool camTempAlertDisplayed = false;
        int camTempAlertDisplayedCounter = 0;
        List<string> firmwareErrorsWarnings;
        // FW Status CSV File
        private string statusMessageFilename;
        private StreamWriter streamWriterStatusMessageFile;
        private int statusMessagePeriod = 1;
        private int statusMessageCount = 0;
        private int statusMessageTimeCount = 0;
        string firmwareErrorWarningLogFileLocalPath = string.Empty;
        StringBuilder firmwareOutput = new StringBuilder();
        string firmwareManualTestLogsFile = string.Empty;
        public string FirmwareManualTestLogsFile
        {
            get { return firmwareManualTestLogsFile; }
            set { firmwareManualTestLogsFile = value; }
        }

        string cameraSerialNumber = string.Empty;
        public string CameraSerialNumber
        {
            get { return cameraSerialNumber; }
            set { cameraSerialNumber = value; }
        }
        double TempLowLimit = 0;
        double TempHighLimit = 0;
        double HumidityLowLimit = 0;
        double HumidityHighLimit = 0;
        bool envStatusPassed = false;
        public System.IO.Ports.SerialPort ssCoolingSerialPort = new SerialPort();
        public System.IO.Ports.SerialPort powerSupplySerialPort = new SerialPort();
        public System.IO.Ports.SerialPort flowMeterSerialPort = new SerialPort();
        bool envStatusRunStatus = false;
        private bool coolantTempPassed = false;
        private bool tankLevelPassed = false;
        private bool powerVoltsPassed = false;
        private bool currentAmpsPassed = false;
        private bool purgeFlowRatePassed = false;
        public bool EnvStatusRunStatus
        {
            get { return envStatusRunStatus; }
            set { envStatusRunStatus = value; }
        }
        public bool EnvStatusPassed
        {
            get
            {
                return envStatusPassed;
            }
            set
            {
                envStatusPassed = value;
                if(environmentalStatusForm != null && environmentalStatusForm.PurgeFlowRatePassed.HasValue &&
                    environmentalStatusForm.CurrentAmpsPassed.HasValue &&
                    environmentalStatusForm.PowerVoltsPassed.HasValue)
                {
                    if ((bool)environmentalStatusForm.PurgeFlowRatePassed &&
                         (bool)environmentalStatusForm.PowerVoltsPassed && 
                         (bool)environmentalStatusForm.CurrentAmpsPassed)
                    {
                        if (environmentalStatusForm.CoolantTempPassed != null &&
                            environmentalStatusForm.TankLevelPassed != null)
                        {
                            if ((bool)environmentalStatusForm.CoolantTempPassed &&
                                (bool)environmentalStatusForm.TankLevelPassed)
                            {
                                BeginInvoke(new System.Action(() => ultraStatusBar1.Panels["EnvStatus"].Appearance.BackColor = Color.LimeGreen));
                            }
                            else
                            {
                                BeginInvoke(new System.Action(() => ultraStatusBar1.Panels["EnvStatus"].Appearance.BackColor = Color.Red));
                            }
                        }
                        else
                        {
                            BeginInvoke(new System.Action(() => ultraStatusBar1.Panels["EnvStatus"].Appearance.BackColor = Color.LimeGreen));
                        }

                    }
                    else
                    {                       
                          BeginInvoke(new System.Action(() => ultraStatusBar1.Panels["EnvStatus"].Appearance.BackColor = Color.Red));                      
                    }
                }
                else
                {
                    //BeginInvoke(new System.Action(() => ultraStatusBar1.Panels["EnvStatus"].Appearance.BackColor = Color.Transparent));
                }
                               
            }
        }
        public bool CoolantTempPassed
        {
            get { return coolantTempPassed; }
            set { coolantTempPassed = value; }
        }
        public bool TankLevelPassed
        {
            get { return tankLevelPassed; }
            set { tankLevelPassed = value; }
        }
        public bool PowerVoltsPassed
        {
            get { return powerVoltsPassed; }
            set { powerVoltsPassed = value; }
        }
        public bool CurrentAmpsPassed
        {
            get { return currentAmpsPassed; }
            set { currentAmpsPassed = value; }
        }
        public bool PurgeFlowRatePassed
        {
            get { return purgeFlowRatePassed; }
            set { purgeFlowRatePassed = value; }
        }

        string TCubeCOMPort = string.Empty;
        string BKPowerCOMPort = string.Empty;
        string FlowMeterCOMPort = string.Empty;
        bool AutoConnectEnvStatCOMPorts = false;

        TempHumReadingModel tempHumReadingModelInitialConnect { get; set; }
        #endregion fields
        public KronosTestAppMainForm()
        {
            try
            {
                InitializeComponent();
                log.Info("Begin Test App");
                
                cidInterface = new CIDInterface();
                autoTestFailReason = new StringBuilder();
                networkSpeedTimer.Tick += updateNetworkSpeedTimer_Tick;
                networkSpeedTimer.Enabled = true;
                logTempHumTimer.Tick += updatelogTempHumTimer_Tick;
                logTempHumTimer.Enabled = true;
                camTempAutoTestAlertTimer.Tick += camTempAutoTestAlertTimer_Tick;
                camTempAutoTestAlertTimer.Enabled = true;               
                availableRamCounter = new System.Diagnostics.PerformanceCounter("Memory", "Available MBytes");
                this.lastPrintX = 0;
                this.lastPrintY = 0;
                mainFormPrintDocument.BeginPrint += new PrintEventHandler(ultraPrintDocument1_BeginPrint);
                mainFormPrintDocument.PrintPage += new PrintPageEventHandler(ultraPrintDocument1_PrintPage);               

                cidInterface.ReceivedImage = ReceiveCameraImage;
                cidInterface.ReceivedIntegratedImage = ReceiveIntegratedCameraImage;
                cidInterface.ReceivedLogData = ReceiveCamaeraLogData;
                cidInterface.ReceivedExposureComplete = ReceivedCameraExposureComplete;
                cidInterface.ReceivedCameraInformation = ReceivedCameraInformation;
                cidInterface.ReceivedStatus = populateStatusGUI;
                assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                Infragistics.Win.AppStyling.StyleManager.Reset();
                ((ComboBoxTool)mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Apply Styles"]).ToolValueChanged += ApplyInfraStyle;
                logger = new Logger();
                logger.IsManufacturingUserMode = true;
                temperatureGraphForm = new TemperatureGraphForm();
                cameraInformation = new CameraInformation();
                TestAppHelper.kronosTestAppMainForm = this;
                burnInAnalysis = new BurnInAnalysis(cidInterface);
                checkTCPcancellationTokenSource = new CancellationTokenSource();
                defaultFilePath = Properties.Settings.Default.FilesLocation;
                firmwareErrorsWarnings = new List<string>();
                string logFilelocalPath = string.Empty;
               
                string filetimestamp = DateTime.Now.ToString("MM_dd_yyyy_HH_mm");
                logFilelocalPath = System.IO.Path.Combine(defaultFilePath, "SCM5821AX1FirmwareStatusMsgs");
                firmwareErrorWarningLogFileLocalPath = System.IO.Path.Combine(defaultFilePath, "SCM5821AX1FirmwareOutput");
                if (!System.IO.Directory.Exists(logFilelocalPath))
                {
                    System.IO.Directory.CreateDirectory(logFilelocalPath);
                }
                if (!System.IO.Directory.Exists(firmwareErrorWarningLogFileLocalPath))
                {
                    System.IO.Directory.CreateDirectory(firmwareErrorWarningLogFileLocalPath);
                }
                statusMessageFilename = logFilelocalPath + "\\FWStatus" + filetimestamp + ".csv";

               
            }
            catch (NullReferenceException ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        } 
        private async void KronosTestAppMainForm_Load(object sender, EventArgs e)
        {
            try
            {
                mainFormDockManager.DockAreas[1].Closed = true;
                ultraStatusBar1.Panels["CameraStatus"].Text = "Connecting to Camera....";
                ultraStatusBar1.Panels["CameraStatus"].Appearance.BackColor = Color.Yellow;
                this.Width = Properties.Settings.Default.FormWidth;
                this.Height = Properties.Settings.Default.FormHeight;
                LocationX = 10;
                LocationY = 10;
                this.Location = new Point(LocationX, LocationY);
                ultraStatusBar1.Panels["DatabaseConnectivity"].Text = "Connecting to Database....";
                ultraStatusBar1.Panels["DatabaseConnectivity"].Appearance.BackColor = Color.Yellow;
                SetDatabaseConnectivityStatusBarPanel();
                stylePath = File.Exists("InfraStyles/" + Properties.Settings.Default.InfraStyleName + ".isl");
                if (stylePath)
                    Infragistics.Win.AppStyling.StyleManager.Load(stylePath ? "InfraStyles/" + Properties.Settings.Default.InfraStyleName + ".isl" :
                                                                    assemblyPath + "//" + Properties.Settings.Default.InfraStyleName + ".isl");
                this.mainFormPrintPreviewDialog.Document = this.mainFormPrintDocument;
                this.mainFormPrintDocument.Header.TextRight = "Page: [Page #]";
                this.mainFormPrintDocument.Footer.TextRight = "Page: [Page #]";
               
                ((TextBoxTool)mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["TextBoxToolFolderPath"]).Text = defaultFilePath;
                Ribbon rb = mainFormToolBarManager.Ribbon;
                rb.FileMenuStyle = FileMenuStyle.None;
                this.mainFormToolBarManager.Ribbon.AllowAutoHide = Infragistics.Win.DefaultableBoolean.True;
                this.mainFormToolBarManager.Ribbon.DisplayMode = RibbonDisplayMode.TabsOnly;
                await LoadInfraStyles();
                //((StateButtonTool)(mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Send Heartbeat"])).Checked = Settings.Default.sendCameraHeartBeat;
                ((StateButtonTool)(mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Send Heartbeat"])).Checked = false;               
                TECCheckBox.Checked = Settings.Default.TECEnabled;
                environmentalStatusForm = new EnvironmentalStatus(environmentalStatusPresenter, this);

                if (environmentalStatusPresenter.COMPortConfigurationModel != null)
                {
                     TCubeCOMPort = environmentalStatusPresenter.COMPortConfigurationModel.TCubeCOMPort;
                     BKPowerCOMPort = environmentalStatusPresenter.COMPortConfigurationModel.BKPowerCOMPort;
                     FlowMeterCOMPort = environmentalStatusPresenter.COMPortConfigurationModel.FlowMeterCOMPort;
                     AutoConnectEnvStatCOMPorts = environmentalStatusPresenter.COMPortConfigurationModel.AutoConnectCOMPorts;
                }
                AutoConnectCamera();
                Thread.Sleep(2000);
                if (cidInterface.IsConnected())
                {
                   // log.Error("Start looking for burnin files.");
                    if (ConfigurationManager.AppSettings["EndClient"] == "Thermo")
                    {
                        //log.Error("Start looking for burnin files 1.");
                        TestAppHelper.BurnInFilesExistOnCamera = burnInAnalysis.lookupBurnInScriptFile();
                        if (TestAppHelper.BurnInFilesExistOnCamera)
                        {
                           // log.Error("BurnIn file found");
                            controlPanelExplorerBar.Groups["controlPanelBurnInDataAnalysis"].Visible = true;
                        }
                    }
                    //log.Error("End looking for burnin files");
                }             
                if (ConfigurationManager.AppSettings["EndClient"] != "Thermo")
                {
                    ((ToolBase)(mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Report Bug"])).SharedProps.Enabled = false;
                    ((ToolBase)(mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Update BurnIn Files"])).SharedProps.Enabled = false;
                    ((ToolBase)(mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["BurnIn Results"])).SharedProps.Enabled = false;                    
                    ultraRadioButtonManufacutring.Checked = true;
                    ultraRadioButtonManufacutring.Enabled = false;
                    ultraRadioButtonManufacutring_Click(null, null);
                    ultraRadioButtonEngineering.Enabled = false;
                    ultraRadioButtonAdmin.Enabled = false;
                }
                getNetworkSpeed();               
                    
                    //defectsPresenter.buildROIMask();
                runAutoLEDCalibrationTest();
               
                //LogManulTestFirmwareOutput();
                testStationCalibrationReminder = new TestStationCalibrationReminder(userLoginPresenter);
                TestStationCalibrationModel calibrationModel= await testStationCalibrationReminder.GetCalibrationModelForCurrentTestStation();
                if(calibrationModel !=null && calibrationModel.NotificationDate.Value.Date <= DateTime.Today.Date)
                {
                    testStationCalibrationReminder.ShowDialog();
                }
                await tempHumidPresenter.GetTempHumidLimits();
                if(tempHumidPresenter.TemperatureHumidityLimitsDataList != null &&
                        tempHumidPresenter.TemperatureHumidityLimitsDataList.Count() > 0)
                {
                    TempLowLimit = tempHumidPresenter.TemperatureHumidityLimitsDataList[0].MinTemp;
                    TempHighLimit = tempHumidPresenter.TemperatureHumidityLimitsDataList[0].MaxTemp;
                }
                else
                {

                }
                await CheckTCPConnection(checkTCPcancellationTokenSource);
                //environmentalStatusForm = new EnvironmentalStatus();
                //environmentalStatusForm.ConnectToEnvironmentalStatusCOMPorts();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }    
        public void SetDatabaseConnectivityStatusBarPanel()
        {
            try
            {
                if (TestAppHelper.CheckDBExists())// && ConfigurationManager.AppSettings["EndClient"] == "Thermo")
                {
                    ultraStatusBar1.Panels["DatabaseConnectivity"].Visible = true;
                    ultraStatusBar1.Panels["DatabaseConnectivity"].Text = "Database Connected";
                    ultraStatusBar1.Panels["DatabaseConnectivity"].Appearance.BackColor = Color.LimeGreen;
                }
                else
                {
                    ultraStatusBar1.Panels["DatabaseConnectivity"].Text = "Database Not Connected";
                    ultraStatusBar1.Panels["DatabaseConnectivity"].Appearance.BackColor = Color.Orange;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
       
        private async void runAutoLEDCalibrationTest()
        {
            try
            {
                List<LEDCalibrationTestResults> lEDCalibrationTestResultList = await ledCalibrationPresenter.GetLEDCalibrationResults(Environment.MachineName);
                int testCounter = 0;
                int currentTestInterval = 0;
                int previousTestInterval = 0;
                if (lEDCalibrationTestResultList.Count > 0)
                {
                    LEDCalibrationTestResults LEDCalibrationTestResult = lEDCalibrationTestResultList[0];
                    testCounter = LEDCalibrationTestResult.TestCounter;
                }
                await ledCalibrationPresenter.GetLimits();
               
                List<LEDCalibrationLimitsData> ledCalibrationLimitsDataList = ledCalibrationPresenter.LEDCalibrationLimitsDataList;
                if (ledCalibrationLimitsDataList.Count > 0)
                {
                    LEDCalibrationLimitsData ledCalibrationLimitsData = ledCalibrationLimitsDataList[0];
                    currentTestInterval = ledCalibrationLimitsData.CurrentTestInterval;
                    previousTestInterval = ledCalibrationLimitsData.PreviousTestInterval;
                }
                if ((testCounter + 1) == currentTestInterval)
                {
                    UltraDesktopAlertShowWindowInfo windowInfo1 = new UltraDesktopAlertShowWindowInfo
                                                                   ("Auto LED Calibration Test.", "Running LED Calibration Test in Auto mode.");
                    if (File.Exists(Application.StartupPath.ToString() + "\\Resources\\CameraTempAlert.wav"))
                    {
                        windowInfo1.Sound = Application.StartupPath.ToString() + "\\Resources\\CameraTempAlert.wav";
                    }
                    windowInfo1.ScreenPosition = ScreenPosition.Center;
                    ultraDesktopAlert1.Show(windowInfo1);
                    LEDCalibrationTest lEDCalibrationTest = new LEDCalibrationTest(this, cidInterface, ledCalibrationPresenter);
                    await lEDCalibrationTest.RunLEDCalibration();
                    ultraDesktopAlert1.CloseAll();
                    TestAppHelper.LEDCalibSavedCounter = 0;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }

        }
        private void ReceiveIntegratedCameraImage(Subarray cameraImage)
        {
            try
            {
                videoPacket.region.dX = (ushort)cameraImage.OffsetColumn;
                videoPacket.region.dY = (ushort)cameraImage.OffsetRow;
                videoPacket.region.Xo = (ushort)cameraImage.StartColumn;
                videoPacket.region.Yo = (ushort)cameraImage.StartRow;
                videoPacket.tag = (uint)cameraImage.Tag;
                videoPacket.exposureNumber = (uint)cameraImage.ExposureTag;
                videoPacket.timeStampUs = (uint)cameraImage.ReadTimestamp;
                videoPacket.imageType = (Thermo.Kronos.Instrument.Camera.Contracts.Enums.ImageType)Enum.ToObject(
                    typeof(Thermo.Kronos.Instrument.Camera.Contracts.Enums.ImageType), Convert.ToInt16(cameraImage.ImageType));
                CID821_Data cid821Data = new CID821_Data();
                int dataIndex = 0;
                int xindex = 0;
                int yindex = 0;
                cid821Data.videoIntegratedDataList = new Int32[videoPacket.region.dX, videoPacket.region.dY];
                if (videoPacket.imageType == Thermo.Kronos.Instrument.Camera.Contracts.Enums.ImageType.Integrated)
                {
                    for (UInt32 indexY = videoPacket.region.Yo; indexY < videoPacket.region.dY + videoPacket.region.Yo; indexY++)
                    {
                        for (UInt32 indexX = videoPacket.region.Xo; indexX < videoPacket.region.dX + videoPacket.region.Xo; indexX++)
                        {
                            cid821Data.videoIntegratedDataList[xindex, yindex] = cameraImage.RawData[indexX, indexY];
                            xindex++;
                            dataIndex++;
                        }
                        xindex = 0;
                        yindex++;
                    }
                }
                // Add this video packet to the data list         
                cid821Data.row = (int)videoPacket.region.Xo;
                cid821Data.column = (int)videoPacket.region.Yo;
                cid821Data.dc = (int)videoPacket.region.dY;
                cid821Data.dr = (int)videoPacket.region.dX;
                cid821Data.subarrayTimeStamp = videoPacket.timeStampUs;
                cid821Data.subarrayName = videoPacket.tag.ToString();
                cid821Data.subarrayNumber = (int)videoPacket.tag;
                cid821Data.exposureNumber = (int)videoPacket.exposureNumber;
                cid821Data.imageType = videoPacket.imageType;
                // Add image to the Integrated Data List
                cidIntegratedDataList.Add(cid821Data);
                UpdateStatusBar();                
                UpdateStatusBarForAutoTest();
            }
            catch (OutOfMemoryException ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ReceivedCameraInformation(CameraInformation cameraInfo)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        this.ReceivedCameraInformation(cameraInfo);
                        cameraInformation = cameraInfo;
                        UpdateCameraDetailsOnScreen(cameraInfo);
                        updateCameraTestMetrics();
                    });
                    return;
                }
                // populate the About box with camera meta-data
                cameraInformation = cameraInfo;
                aboutForm = new AboutForm(cameraInfo);               
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ReceivedLogDataHandler(LogEntry logEntry)
        {
            try
            {
                Logger.LogLocation logLocation;
                logLocation = logger.GetLoggerLocation();
                if (logLocation == Logger.LogLocation.Console)
                {
                    BeginInvoke(new System.Action(() => firmwareLoggingTextBox.AppendText(logEntry.Text)));
                    BeginInvoke(new System.Action(() => firmwareLoggingTextBox.AppendText(Environment.NewLine)));
                }
                if (logLocation == Logger.LogLocation.File)
                {
                    //if (meanVarianceTestForm != null && meanVarianceTestForm.MeanVarianceTestStatus)
                    //{
                    //    return;
                    //}
                    //else if (darkCurrentTestForm != null && darkCurrentTestForm.DarkCurrentTestStatus)
                    //{
                    //    return;
                    //}
                    //else if (defectsTestForm != null && defectsTestForm.DefectsTestStatus)
                    //{
                    //    return;
                    //}
                    //else if (photoresponseTestForm != null && photoresponseTestForm.PhotoresponseTestStatus)
                    //{
                    //    return;
                    //}
                    //else if (injectionEfficiencyTestForm != null && injectionEfficiencyTestForm.InjectionPerformanceTestStatus)
                    //{
                    //    return;
                    //}
                    //else if (shutterDriveTestForm != null && shutterDriveTestForm.ShutterDriveTestStatus)
                    //{
                    //    return;
                    //}

                    //ParametersModel parametersModel = TestAppHelper.GetParametersModel();
                    //string logFileServerPath = string.Empty;
                    //logFileServerPath = System.IO.Path.Combine(parametersModel.TestResultsServerPath.ToString(), "SCM5821AX1FirmwareManualTestLogs");
                    //if (!System.IO.Directory.Exists(logFileServerPath))
                    //{
                    //    System.IO.Directory.CreateDirectory(logFileServerPath);
                    //}
                    //string logFilelocalPath = string.Empty;
                    //logFilelocalPath = System.IO.Path.Combine(defaultFilePath, "SCM5821AX1FirmwareManualTestLogs");
                    //if (!System.IO.Directory.Exists(logFilelocalPath))
                    //{
                    //    System.IO.Directory.CreateDirectory(logFilelocalPath);
                    //}
                    //using (StreamWriter writer = new StreamWriter(logFileServerPath + "\\" + firmwareManualTestLogsFile, true))
                    //{
                    //    writer.WriteLine(logEntry.Text);
                    //}
                    //using (StreamWriter writer = new StreamWriter(logFilelocalPath + "\\" + firmwareManualTestLogsFile, true))
                    //{
                    //    writer.WriteLine(logEntry.Text);
                    //}
                    //File.AppendAllText(logFileServerPath + "\\" + firmwareManualTestLogsFile, logEntry.Text);
                   // File.AppendAllText(logFilelocalPath + "\\" + firmwareManualTestLogsFile, logEntry.Text);
                }
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        this.ReceivedLogDataHandler(logEntry);
                    });
                    return;
                }
                if (fwLoggingEnabled)
                    log.Debug(logEntry);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }      
        private void ReceiveCameraImage(Subarray cameraImage)
        {
            try
            {
                videoPacket.region.dX = (ushort)cameraImage.OffsetColumn;
                videoPacket.region.dY = (ushort)cameraImage.OffsetRow;
                videoPacket.region.Xo = (ushort)cameraImage.StartColumn;
                videoPacket.region.Yo = (ushort)cameraImage.StartRow;
                videoPacket.tag = (uint)cameraImage.Tag;
                videoPacket.exposureNumber = (uint)cameraImage.ExposureTag;
                videoPacket.timeStampUs = (uint)cameraImage.ReadTimestamp;
                videoPacket.imageType = (Thermo.Kronos.Instrument.Camera.Contracts.Enums.ImageType)Enum.ToObject(
                    typeof(Thermo.Kronos.Instrument.Camera.Contracts.Enums.ImageType), Convert.ToInt16(cameraImage.ImageType));
                CID821_Data cid821Data = new CID821_Data();
                int dataIndex = 0;
                int xindex = 0;
                int yindex = 0;
                cid821Data.videoDataList = new Int32[videoPacket.region.dX, videoPacket.region.dY];
                for (UInt32 indexY = videoPacket.region.Yo; indexY < videoPacket.region.dY + videoPacket.region.Yo; indexY++)
                {
                    for (UInt32 indexX = videoPacket.region.Xo; indexX < videoPacket.region.dX + videoPacket.region.Xo; indexX++)
                    {
                        zPatternCorrectedData[indexX, indexY] = cameraImage.RawData[indexX, indexY];
                        cid821Data.videoDataList[xindex, yindex] = cameraImage.RawData[indexX, indexY];
                        xindex++;
                        dataIndex++;
                    }
                    xindex = 0;
                    yindex++;
                }
                // Add this video packet to the data list         
                cid821Data.row = (int)videoPacket.region.Xo;
                cid821Data.column = (int)videoPacket.region.Yo;
                cid821Data.dc = (int)videoPacket.region.dY;
                cid821Data.dr = (int)videoPacket.region.dX;
                cid821Data.subarrayTimeStamp = videoPacket.timeStampUs;
                cid821Data.subarrayName = videoPacket.tag.ToString();
                cid821Data.subarrayNumber = (int)videoPacket.tag;
                cid821Data.exposureNumber = (int)videoPacket.exposureNumber;
                cid821Data.imageType = videoPacket.imageType;
                // If there is RAM available, add to the end of the list
                if (availableRamCounter.NextValue() > 2000.0)
                {
                    cidDataList.Add(cid821Data);
                    UpdateStatusBarForAutoTest();
                    UpdateStatusBar1();
                    //pretestexposureCount++;
                    //UpdateStatusBarForPreTest();
                }
                else
                {
                    // Start removing the oldest elements from the list
                    if (cidDataList.Count > 1)
                        cidDataList.RemoveAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error("current running test"+currentRunningTest);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateStatusBar()
        {
            try
            {
                if (!AutoPreTestStatus && !AutoTestStatus)
                {
                    if (ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value < 100)
                    {
                        if (redBluePresenter.ExposureCount > 0 && TestAppHelper.currentRunningTest == "RedBlue" && ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value < 100 &&
                            (100 - ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value > redBluePresenter.ExposureCount) && (ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value + 100 / redBluePresenter.ExposureCount < 100))
                            ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value += 100 / redBluePresenter.ExposureCount;
                        else if (darkCurrentPresenter.ExposureCount > 0 && TestAppHelper.currentRunningTest == "DarkCurrent" && ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value < 100
                            && (100 - ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value > darkCurrentPresenter.ExposureCount) &&
                            (ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value + 100 / darkCurrentPresenter.ExposureCount < 100))
                            ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value += 100 / darkCurrentPresenter.ExposureCount;
                        else if (defectsPresenter.ExposureCount > 0 && TestAppHelper.currentRunningTest == "DefectsTest" && ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value < 100
                            && (100 - ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value > defectsPresenter.ExposureCount) &&
                            (ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value + 100 / defectsPresenter.ExposureCount < 100))
                            ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value += 100 / defectsPresenter.ExposureCount;
                        else if (injectionEfficiencyPresenter.ExposureCount > 0 && TestAppHelper.currentRunningTest == "InjectionEfficiency" && ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value < 100
                            && (100 - ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value > injectionEfficiencyPresenter.ExposureCount)
                            && (ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value + 100 / injectionEfficiencyPresenter.ExposureCount < 100))
                            ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value += 100 / injectionEfficiencyPresenter.ExposureCount;
                        else if (photoresponsePresenter.ExposureCount > 0 &&  TestAppHelper.currentRunningTest == "Photoresponse" && ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value < 100
                            && (100 - ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value > 100 / photoresponsePresenter.ExposureCount)
                            && (ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value + 100 / photoresponsePresenter.ExposureCount < 100))
                        {
                            int i = photoresponsePresenter.ExposureCount / 100;
                            i=i-2;
                            ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value += i;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateStatusBar1()
        {
            try
            {
                if (!AutoPreTestStatus && !AutoTestStatus)
                {
                    if (ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value < 100)
                    {
                        if (meanVariancePresenter.ExposureCount > 0 && TestAppHelper.currentRunningTest == "MeanVariance" && ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value < 100
                            && (100 - ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value > meanVariancePresenter.ExposureCount)
                            && (ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value + 100 / meanVariancePresenter.ExposureCount < 100))
                            ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value += 100 / meanVariancePresenter.ExposureCount;
                        else if (noiseVsNDROPresenter.ExposureCount > 0 && TestAppHelper.currentRunningTest == "NoiseVsNDRO" && ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value < 100
                            && (100 - ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value > noiseVsNDROPresenter.ExposureCount)
                            && (ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value + 100 / noiseVsNDROPresenter.ExposureCount < 100))
                            ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value += 100 / noiseVsNDROPresenter.ExposureCount;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateStatusBarForAutoTest()
        {
            try
            {
                if (AutoTestStatus)
                {
                   if(currentRunningTest != "Photoresponse")
                    {
                        progressNum++;
                        if (progressNum <= 336)
                        {
                            double percentageD = ((double)progressNum / (double)335) * 100;
                            int percentage = (int)percentageD;
                            ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = percentage;
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }       
        private void ReceiveCamaeraLogData(LogEntry logEntry)
        {
            try
            {
                if (!logEntry.Text.Contains("Info:  WEAK"))
                {
                    if (!logEntry.Text.Contains("Info:  Temp"))
                    {
                        firmwareOutput.AppendLine(logEntry.Text);
                    }
                }
                if (logEntry.LogLevel==Thermo.Kronos.Instrument.Camera.Contracts.Enums.LogLevel.Info)
                {
                    log.Info(logEntry.Text);

                    if ((defectsPresenter!=null && defectsPresenter.GetDeadAndDarkPixelList 
                        && !defectsPresenter.DeaddarkPixelsListCompleted) 
                        || (defectsPresenter.DefectsTestLimitsDataList.Count() > 0 && 
                            defectsPresenter.DeadAndDarkPixelList.Count() <= defectsPresenter.DefectsTestLimitsDataList[0].DeadDarkPixelsListReadLimit))
                    {
                        Point defectivePixel;
                        string defectivePixelString = logEntry.Text;
                        if ((defectivePixelString.Contains(" DEAD,")) || (defectivePixelString.Contains(" DARK,")))
                        {
                            defectivePixel = defectsPresenter.GetPointFromString(defectivePixelString);
                            defectsPresenter.DeadAndDarkPixelList.Add(defectivePixel);
                        }
                        if (logEntry.Text.Contains("DEAD+DARK"))
                        {
                            defectsPresenter.DeaddarkPixelsListCompleted = true;
                        }
                    }
                    if (defectsPresenter.DefectsTestLimitsDataList.Count() > 0 && 
                        defectsPresenter.DeadAndDarkPixelList.Count() > defectsPresenter.DefectsTestLimitsDataList[0].DeadDarkPixelsListReadLimit)
                    {
                        defectsPresenter.DeaddarkPixelsListCompleted = true;
                    }
                }                   
                if (logEntry.LogLevel == Thermo.Kronos.Instrument.Camera.Contracts.Enums.LogLevel.Debug)
                    log.Debug(logEntry.Text);                      
               if ((autoTestForm != null && AutoTestStatus) || (autoPretestForm != null && AutoPreTestStatus))
                {
                    if((logEntry.Text.Contains("Error") || logEntry.Text.Contains("ERROR")|| logEntry.Text.Contains("Warning") 
                        || logEntry.Text.Contains("WARNING") || TestAppHelper.IsFirmwareLogLevelWarning||
                        logEntry.LogLevel == Thermo.Kronos.Instrument.Camera.Contracts.Enums.LogLevel.Warning ||
                     logEntry.LogLevel == Thermo.Kronos.Instrument.Camera.Contracts.Enums.LogLevel.Error )
                        && (logEntry.LogLevel != Thermo.Kronos.Instrument.Camera.Contracts.Enums.LogLevel.Info) && cidInterface.IsConnected())
                    {
                        logEntry.Text.Trim('\n');
                        if (firmwareErrorsWarnings.Where(err => err.Equals(logEntry.Text)).FirstOrDefault() == null)
                        {
                            firmwareErrorsWarnings.Add(logEntry.Text);
                        }                       
                    }
                }               
                if (logEntry.LogLevel == Thermo.Kronos.Instrument.Camera.Contracts.Enums.LogLevel.Warning)
                    log.Warn(logEntry.Text);
                if (logEntry.LogLevel == Thermo.Kronos.Instrument.Camera.Contracts.Enums.LogLevel.Fatal)
                    log.Fatal(logEntry.Text);
                if (logEntry.LogLevel == Thermo.Kronos.Instrument.Camera.Contracts.Enums.LogLevel.Camera)
                    log.Info(logEntry.Text);
                Logger.LogLocation logLocation;
                logLocation = logger.GetLoggerLocation();
                if (((logEntry.LogLevel == Thermo.Kronos.Instrument.Camera.Contracts.Enums.LogLevel.Warning ||
                     logEntry.LogLevel == Thermo.Kronos.Instrument.Camera.Contracts.Enums.LogLevel.Error ||
                     TestAppHelper.IsFirmwareLogLevelWarning ||
                     logEntry.Text.Contains("Error") || logEntry.Text.Contains("ERROR") || logEntry.Text.Contains("Warning")))
                           && (logEntry.LogLevel != Thermo.Kronos.Instrument.Camera.Contracts.Enums.LogLevel.Info) && cidInterface.IsConnected())
                {
                    BeginInvoke(new System.Action(() => mainFormDockManager.DockAreas[1].Closed = false));
                    BeginInvoke(new System.Action(() => mainFormDockManager.DockAreas[1].Activate()));
                    BeginInvoke(new System.Action(() => mainFormDockManager.DockAreas[1].Pin()));
                    BeginInvoke(new System.Action(() => firmwareLoggingTextBox.AppendText(logEntry.Text)));
                    BeginInvoke(new System.Action(() => firmwareLoggingTextBox.AppendText(Environment.NewLine)));
                    if(logEntry.Text.Contains("Total pixels reached the limit. The rest will not be corrected"))
                    {
                        defectsPresenter.FirmwareCorrectedMaxPixels = true;
                    }
                }
                if (logLocation == Logger.LogLocation.Console && !TestAppHelper.IsFirmwareLogLevelWarning)
                {
                    BeginInvoke(new System.Action(() => mainFormDockManager.DockAreas[1].Closed = false));
                    BeginInvoke(new System.Action(() => mainFormDockManager.DockAreas[1].Activate()));
                    BeginInvoke(new System.Action(() => mainFormDockManager.DockAreas[1].Pin()));
                    BeginInvoke(new System.Action(() => firmwareLoggingTextBox.AppendText(logEntry.Text)));
                    BeginInvoke(new System.Action(() => firmwareLoggingTextBox.AppendText(Environment.NewLine)));
                }
                if(logLocation== Logger.LogLocation.File)
                {
                    if ((meanVarianceTestForm != null && meanVarianceTestForm.MeanVarianceTestStatus) ||
                        (darkCurrentTestForm != null && darkCurrentTestForm.DarkCurrentTestStatus) ||
                        (defectsTestForm != null && defectsTestForm.DefectsTestStatus) ||
                        (photoresponseTestForm != null && photoresponseTestForm.PhotoresponseTestStatus) ||
                        (injectionEfficiencyTestForm != null && injectionEfficiencyTestForm.InjectionPerformanceTestStatus) ||
                        (shutterDriveTestForm != null && shutterDriveTestForm.ShutterDriveTestStatus) ||
                        (noiseVsNDROTestForm != null && noiseVsNDROTestForm.NoiseVsNDROTestStatus))
                    {                     
                        string logFileServerPath = string.Empty;
                        ParametersModel parametersModel = TestAppHelper.GetParametersModel();
                        
                        logFileServerPath = System.IO.Path.Combine(parametersModel.TestResultsServerPath.ToString(), "SCM5821AX1FirmwareManualTestLogs");
                        if (!System.IO.Directory.Exists(logFileServerPath))
                        {
                            
                            System.IO.Directory.CreateDirectory(logFileServerPath);
                        }
                        string logFilelocalPath = string.Empty;
                        logFilelocalPath = System.IO.Path.Combine(defaultFilePath, "SCM5821AX1FirmwareManualTestLogs");
                        if (!System.IO.Directory.Exists(logFilelocalPath))
                        {
                            System.IO.Directory.CreateDirectory(logFilelocalPath);
                        }


                        //BeginInvoke(new System.Action(() => firmwareLoggingTextBox.AppendText(logEntry.Text)));
                        //BeginInvoke(new System.Action(() => firmwareLoggingTextBox.AppendText(Environment.NewLine)));

                        using (StreamWriter writer = new StreamWriter(logFileServerPath + "\\" + firmwareManualTestLogsFile, true))
                        {
                            writer.WriteLine(logEntry.Text);
                        }
                        using (StreamWriter writer = new StreamWriter(logFilelocalPath + "\\" + firmwareManualTestLogsFile, true))
                        {
                            writer.WriteLine(logEntry.Text);
                        }
                    }
                }
                if (fwLoggingEnabled)
                    log.Debug(logEntry);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void populateStatusGUI(CameraStatus cameraStatus)
        {
            try
            {
                if (cameraStatus != null)
                {                    
                    cameraStatusForLogTemHum = cameraStatus;
                    if (TECCheckBox.InvokeRequired)
                    {
                        if (cameraStatus.ThermoElectricCoolerEnabled > 0)
                            BeginInvoke(new System.Action(() => TECCheckBox.Checked = true));
                        else
                            BeginInvoke(new System.Action(() => TECCheckBox.Checked = false));
                    }
                    if (textBoxCamTemperature.InvokeRequired)
                    {
                        BeginInvoke(new System.Action(() => textBoxCamTemperature.Text = cameraStatus.ImageSensorTemperatureDiodeInC.ToString("F2")));
                    }
                    if (textBoxCamHumidity.InvokeRequired)
                    {
                        BeginInvoke(new System.Action(() => textBoxCamHumidity.Text = cameraStatus.RelativeHumidityInProcent.ToString("F2")));
                        if(cameraStatus.RelativeHumidityInProcent == -64)
                        {
                            BeginInvoke(new Action(() => textBoxCamHumidity.BackColor = Color.Red));
                        }                         
                    }
                    if (TempLowLimit == 0 )
                    {
                        TempLowLimit = 44.5;
                        TempHighLimit = 65;
                    }
                    if (Math.Abs(cameraStatus.ImageSensorTemperatureDiodeInC) >= TempLowLimit
                           &&
                          Math.Abs(cameraStatus.ImageSensorTemperatureDiodeInC) <= TempHighLimit)
                    {
                        if (autoTestForm != null && userMode.Equals("Manufacturing"))
                        {
                            autoTestForm.CameraTempSet = true;
                        }
                        if (!ledImageTemperature.IsDisposed)
                        {
                            ledImageTemperature.Value = true;
                        }
                    }
                    else
                    {
                        if (autoTestForm != null && userMode.Equals("Manufacturing"))
                        {
                            autoTestForm.CameraTempSet = false;
                        }
                        if (!ledImageTemperature.IsDisposed)
                        {
                            ledImageTemperature.Value = false;
                        }
                    }

                    if (temperatureGraphForm.InvokeRequired)
                    {
                        BeginInvoke(new System.Action(() => temperatureGraphForm.ImageTemperature = cameraStatus.ImageSensorTemperatureDiodeInC));
                        BeginInvoke(new System.Action(() => temperatureGraphForm.ImageHumidity = cameraStatus.RelativeHumidityInProcent));
                    }
                    //if (finalTestReportUserControl != null && finalTestReportUserControl.InvokeRequired && (AutoTestStatus || AutoPreTestStatus))
                    //{
                    //    BeginInvoke(new System.Action(() => finalTestReportUserControl.ImageTemperature = cameraStatus.ImageSensorTemperatureDiodeInC.ToString("F2")));
                    //    BeginInvoke(new System.Action(() => finalTestReportUserControl.ImageHumidity = cameraStatus.RelativeHumidityInProcent.ToString("F2")));
                    //}
                    statusMessageCount++;
                    if (streamWriterStatusMessageFile == null)
                    {

                        streamWriterStatusMessageFile = new StreamWriter(statusMessageFilename, false, Encoding.ASCII);
                        if (streamWriterStatusMessageFile.BaseStream != null)
                         streamWriterStatusMessageFile.WriteLine("Seconds,TECEnabled,CPU,CSP,PWR2,ISI,Diode,Humidity,TecVDiv2,PWR1");
                    }
                    DateTime timeInSeconds = DateTime.Now;
                    if ((statusMessagePeriod < statusMessageCount) && streamWriterStatusMessageFile.BaseStream!=null)
                    {
                        streamWriterStatusMessageFile.WriteLine(
                            statusMessageTimeCount + "," +
                            cameraStatus.ThermoElectricCoolerEnabled + "," +
                            cameraStatus.ArmCpuTemperatureInC + "," +
                            cameraStatus.CameraSignalProcessorBoardTemperatureInC + "," +
                            cameraStatus.DigitalElectronicsRegulatorTemperatureInC + "," +
                            cameraStatus.ImageSensorBoardTemperatureInC + "," +
                            cameraStatus.ImageSensorTemperatureDiodeInC + "," +                            
                            cameraStatus.RelativeHumidityInProcent + "," +
                            cameraStatus.TecVdiv2 + "," +
                            cameraStatus.ThermoElectricControlCircuitTemperatureInC);

                        statusMessageTimeCount = statusMessageTimeCount + statusMessagePeriod;

                        streamWriterStatusMessageFile.Flush();
                        statusMessageCount = 0;

                    }
                    if (cameraStatus.Interlocks != InterlockFlags.None)
                    {
                        BeginInvoke(new System.Action(() => ultraStatusBar1.Panels["Interlocks"].Visible = true));
                        BeginInvoke(new System.Action(() => ultraStatusBar1.Panels["Interlocks"].Text = "Interlock: " + cameraStatus.Interlocks.Value.ToString()));
                        BeginInvoke(new System.Action(() => ultraStatusBar1.Panels["Interlocks"].Appearance.BackColor = Color.Orange));
                        ultraStatusBar1.Panels["Interlocks"].ToolTipText = "Interlock: " + cameraStatus.Interlocks.Value.ToString();
                    }
                    else
                    {
                        BeginInvoke(new System.Action(() => ultraStatusBar1.Panels["Interlocks"].Visible=false));                                              
                    }

                    if(camHumdityListForDBLogging!=null)
                    {
                        camHumdityListForDBLogging.Add(cameraStatus.RelativeHumidityInProcent);
                    }
                    if (camTempHumListForDBLogging != null)
                    {
                        camTempHumListForDBLogging.Add(Tuple.Create(cameraStatus.RelativeHumidityInProcent, cameraStatus.ImageSensorTemperatureDiodeInC));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);                
            }
        }       
        private void ReceivedCameraExposureComplete(uint exposureId)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        this.ReceivedCameraExposureComplete(exposureId);
                    });
                    return;
                }               
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async void AutoConnectCamera()
        {
            try
            {
                BeginInvoke(new System.Action(() => ultraStatusBar1.Panels["CameraStatus"].Text = "Connecting to Camera...."));                
                if (!cidInterface.IsConnected())
                {
                    var task = new Task(delegate()
                    {
                        BeginInvoke(new System.Action(() => ultraStatusBar1.Panels["CameraStatus"].Text = "Connecting to Camera...."));                       
                        cameraConnected = cidInterface.Connect(ConfigurationManager.AppSettings["CameraIPAddresss"], Int32.Parse(ConfigurationManager.AppSettings["CameraIPPort"]));
                    });
                    task.Start();                  
                    await task;
                    BeginInvoke(new System.Action(() => ultraStatusBar1.Panels["CameraStatus"].Text = "Connecting to Camera...."));                                      
                    if (!cameraConnected)// || firmwareId == 0)
                    {
                       
                        log.Error("Firmware not running or camera not connected to host");
                        MessageBox.Show(
                            "Firmware not running or camera not connected to host",
                            "Firmware not found",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Information);
                        ultraStatusBar1.Panels["CameraStatus"].Text = "Camera Not Connected";
                        ultraStatusBar1.Panels["CameraStatus"].Appearance.BackColor = Color.Red;						
						((ToolBase)(mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["CameraConnection"])).SharedProps.AppearancesSmall.Appearance.Image = 
                                                new Bitmap(Assembly.GetEntryAssembly().GetManifestResourceStream("KronosCameraTestApp.Resources.Camera_Off.bmp"));
                        return;
                    }
                    else
                    {                       
                        if (Properties.Settings.Default.enableOnConnect)
                        {
                            InstrumentDefaults instrumentDefault = new InstrumentDefaults(cidInterface, userMode, enggUserName);
                            // Set the voltages automatically after each connection 
                            instrumentDefault.ConfigStructForInstrumentDefaults();
                        }
                    }
                    //cidInterface.SendLogLevel(2);
                    //cidInterface.SendLogLevel(Thermo.Kronos.Instrument.Camera.Contracts.Enums.LogLevel.Warning);
                    TestAppHelper.IsFirmwareLogLevelWarning = true;
                    changeLogLevel();
                    TECCheckBox.Checked = Settings.Default.TECEnabled;
                    TECCheckBox.Enabled = true;
                    BeginInvoke(new System.Action(() => ultraStatusBar1.Panels["CameraStatus"].Text = "Camera Connected"));
                    BeginInvoke(new System.Action(() => ultraStatusBar1.Panels["CameraStatus"].Appearance.BackColor = Color.LimeGreen));
                    //ultraStatusBar1.Panels["CameraStatus"].Text = "Camera Connected";//, FW Ver. " + firmwareId.ToString();
                    //ultraStatusBar1.Panels["CameraStatus"].Appearance.BackColor = Color.LimeGreen;
                    BeginInvoke(new System.Action(() => ((ToolBase)(mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["CameraConnection"])).
                                                            SharedProps.AppearancesSmall.Appearance.Image =
                                                            new Bitmap(Assembly.GetEntryAssembly().GetManifestResourceStream("KronosCameraTestApp.Resources.Camera_ON.bmp"))));
                   
                    TECCheckBox_Click(null, null);
                    cidInterface.GetCameraInformation();
                    UpdateCameraDetailsOnScreen(cameraInformation);
                   
                    Properties.Settings.Default.CameraIPAddress = ConfigurationManager.AppSettings["CameraIPAddresss"];
                    Properties.Settings.Default.CameraPortNumber = ConfigurationManager.AppSettings["CameraIPPort"];
                    controlPanelExplorerBar.Groups["controlPanelCameraDetails"].Visible = true;
                    controlPanelExplorerBar.Groups["controlPanelCameraTestMetrics"].Visible = true;
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    // LookUpBurnInFiles();
                    (mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Send Heartbeat"]).SharedProps.Enabled = false;
                    //Settings.Default["sendCameraHeartBeat"] = ((StateButtonTool)(mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Send Heartbeat"])).Checked;
                    if (((StateButtonTool)(mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Send Heartbeat"])).Checked)
                    {
                        ((StateButtonTool)(mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Send Heartbeat"])).Checked = true;
                        await cidInterface.SendHeartbeatAsync(cancellationTokenSource);                       
                    }
                    if(AutoConnectEnvStatCOMPorts)
                    {
                        ConnectToEnvironmentalStatusCOMPorts(false);
                        await RunEnvStatTest(false);
                    }

                }
                else if (cidInterface.IsConnected())
                {
                    cidInterface.Disconnect();
                    var task = new Task(delegate()
                    {
                        cidInterface.Disconnect();
                    });
                    task.Start();
                    await task;
                    ultraStatusBar1.Panels["CameraStatus"].Text = "Camera Disconnected";
                    ultraStatusBar1.Panels["CameraStatus"].Appearance.BackColor = Color.Red;
					((ToolBase)(mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["CameraConnection"])).SharedProps.AppearancesSmall.Appearance.Image = 
                        new Bitmap(Assembly.GetEntryAssembly().GetManifestResourceStream("KronosCameraTestApp.Resources.Camera_Off.bmp"));

                    (mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Send Heartbeat"]).SharedProps.Enabled = true;
                }
               
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public void ConnectToEnvironmentalStatusCOMPorts(bool connectFromEnvStatusFrm)
        {
            try
            {
                //Connect to Flow meter 
                if(environmentalStatusPresenter.COMPortConfigurationModel==null)
                {
                    FlowMeterCOMPort = environmentalStatusForm.FlowMeterCOMPort;
                    TCubeCOMPort = environmentalStatusForm.TCubeCOMPort;
                    BKPowerCOMPort = environmentalStatusForm.BKPowerCOMPort;
                }
                if (!string.IsNullOrEmpty(FlowMeterCOMPort))
                {
                    if (!flowMeterSerialPort.IsOpen)
                    {
                        flowMeterSerialPort.PortName = FlowMeterCOMPort;
                        flowMeterSerialPort.BaudRate = 9600;
                        flowMeterSerialPort.Open();
                    }
                }
                //connect to TubeCooler
                if (TECCheckBox.Checked || connectFromEnvStatusFrm)
                {
                    if (!string.IsNullOrEmpty(TCubeCOMPort))
                    {
                        if (!ssCoolingSerialPort.IsOpen)
                        {
                            ssCoolingSerialPort.PortName = TCubeCOMPort;
                            ssCoolingSerialPort.BaudRate = 57600;
                            ssCoolingSerialPort.Open();
                        }
                    }
                }

                //connect to BK Power supply
                if (!string.IsNullOrEmpty(BKPowerCOMPort))
                {
                    if (!powerSupplySerialPort.IsOpen)
                    {
                        powerSupplySerialPort.PortName = BKPowerCOMPort;
                        //powerSupplySerialPort.ReadTimeout = -1;
                        powerSupplySerialPort.Open();
                    }
                }



            }
            catch (Exception ex)
            {

            }
        }
        public async Task RunEnvStatTest(bool runFromEnvStatusFrm)
        {
            try
            {
                BeginInvoke(new System.Action(() => ultraStatusBar1.Panels["EnvStatus"].Visible = true));
                environmentalStatusForm.btnConnect.Text = "Disconnect";
                
                if (!powerSupplySerialPort.IsOpen)
                {
                    if (!string.IsNullOrEmpty(BKPowerCOMPort))
                    {
                        powerSupplySerialPort.PortName = BKPowerCOMPort;
                    }                   
                    powerSupplySerialPort.Open();
                }
                else
                {
                    await ConnectToDCPower();
                }
               
                if (!flowMeterSerialPort.IsOpen)
                {
                    if (!string.IsNullOrEmpty(FlowMeterCOMPort))
                    {
                        flowMeterSerialPort.PortName = FlowMeterCOMPort;
                    }                   
                    flowMeterSerialPort.BaudRate = 9600;
                    flowMeterSerialPort.Open();                   
                }
                else
                {
                    await ConnectToFlowMeter();
                }
                if (TECCheckBox.Checked || runFromEnvStatusFrm)
                {
                    if (!ssCoolingSerialPort.IsOpen)
                    {
                        if (!ssCoolingSerialPort.IsOpen)
                        {
                            if (!string.IsNullOrEmpty(TCubeCOMPort))
                            {
                                ssCoolingSerialPort.PortName = TCubeCOMPort;
                            }
                            ssCoolingSerialPort.BaudRate = 57600;
                            ssCoolingSerialPort.Open();
                        }
                    }
                    else
                    {
                        await ConnectTossCooling();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task ConnectToFlowMeter()
        {
            try
            {
                await Task.Run(async () =>
                {

                    if (!flowMeterSerialPort.IsOpen)
                    {
                        if (!string.IsNullOrEmpty(FlowMeterCOMPort))
                        {
                            flowMeterSerialPort.PortName = FlowMeterCOMPort;
                        }
                        flowMeterSerialPort.BaudRate = 9600;
                        flowMeterSerialPort.Open();
                        envStatusRunStatus = true;
                    }
                    // Open serial port to digital flow meter   

                    if (flowMeterSerialPort.IsOpen)
                    {
                        //_flowMeterDispatcherTimer.Start();
                        envStatusRunStatus = true;
                        try
                        {
                            string flowRateString = await GetDFC(flowMeterSerialPort);
                            if (flowRateString.Length > 2)
                            {
                                string purgeFlowRate = flowRateString.Substring(1, 5);
                                BeginInvoke(new System.Action(() => environmentalStatusForm.PurgeFlowRateVal = purgeFlowRate));
                                double purgeFlow = Convert.ToDouble(purgeFlowRate);
                                if (purgeFlow >= environmentalStatusPresenter.EnvironmentalStatusLimitsDataList[0].PurgeFlowRateLowerLimit &&
                                    purgeFlow <= environmentalStatusPresenter.EnvironmentalStatusLimitsDataList[0].PurgeFlowRateUpperLimit)
                                {
                                    environmentalStatusForm.PurgeFlowRatePassed=true;
                                    EnvStatusPassed = true;
                                }
                                else
                                {
                                    environmentalStatusForm.PurgeFlowRatePassed = false;                                    
                                    EnvStatusPassed = false;
                                }
                                if(purgeFlowRatesDuringFinalTest!=null)
                                {
                                    purgeFlowRatesDuringFinalTest.Add(environmentalStatusForm.PurgeFlowRatePassed);
                                }
                            }
                            _ = startFlowMeterStatusThread();
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show("Exception in Digital Flow Control for purge", ex.ToString(), MessageBoxButtons.OK,
                            //     MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                            return;
                        }
                    }

                });

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task startFlowMeterStatusThread()
        {
            try
            {
            await Task.Run(async () =>
            {
                while (envStatusRunStatus)
                {

                    if (flowMeterSerialPort.IsOpen)
                    {
                        await Task.Delay(5000);

                        byte[] buffer = new byte[60];
                        int readLength = 0;
                        flowMeterSerialPort.Write("F" + "\r");

                        // Delay 2 seconds before reading buffer
                        await Task.Delay(2000);
                        try
                        {
                            if (flowMeterSerialPort.IsOpen)
                            {
                                flowMeterSerialPort.ReadTimeout = 500;
                                readLength = flowMeterSerialPort.Read(buffer, 0, buffer.Length);
                            }
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(
                            // "DFC Port not connected" + ex.ToString(), "Command not sent", MessageBoxButtons.OK,
                            //  MessageBoxIcon.Warning);
                            log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                        }
                        if (buffer.Length > 0)
                        {
                            if(!environmentalStatusForm.IsHandleCreated)
                            {
                                environmentalStatusForm.CreateControl();
                            }

                            //environmentalStatusForm.btnConnect.Invoke((MethodInvoker)delegate () { environmentalStatusForm.btnConnect.Text = "Disconnect!"; });
                            BeginInvoke(new System.Action(() => environmentalStatusForm.btnConnect.Text = "Disconnect"));
                            //environmentalStatusForm.btnConnect.Text = "Disconnect";
                            // Convert byte buffer to string
                            string flowRateString = System.Text.Encoding.UTF8.GetString(buffer, 0, readLength);
                            if (flowRateString.Length > 2)
                            {
                                string purgeFlowRate = flowRateString.Substring(0, 5);
                                //environmentalStatusForm.PurgeFlowRateVal.Invoke((MethodInvoker)delegate () { environmentalStatusForm.PurgeFlowRateVal = purgeFlowRate; });
                                BeginInvoke(new System.Action(() => environmentalStatusForm.PurgeFlowRateVal = purgeFlowRate));
                                //environmentalStatusForm.PurgeFlowRateVal = purgeFlowRate;
                                double purgeFlow = Convert.ToDouble(purgeFlowRate);
                                if (purgeFlow >= environmentalStatusPresenter.EnvironmentalStatusLimitsDataList[0].PurgeFlowRateLowerLimit &&
                                    purgeFlow <= environmentalStatusPresenter.EnvironmentalStatusLimitsDataList[0].PurgeFlowRateUpperLimit)
                                {
                                    environmentalStatusForm.PurgeFlowRatePassed = true;
                                    EnvStatusPassed = true;
                                }
                                else
                                {
                                    environmentalStatusForm.PurgeFlowRatePassed = false;
                                    EnvStatusPassed = false;
                                }
                                if (purgeFlowRatesDuringFinalTest != null)
                                {
                                    purgeFlowRatesDuringFinalTest.Add(environmentalStatusForm.PurgeFlowRatePassed);
                                }
                            }
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
        private async Task<string> GetDFC(SerialPort serialPort)
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    byte[] buffer = new byte[60];
                    string returnString = "0.0";
                    int readLength = 0;

                    // Get Flow reading from digital flow control system
                    serialPort.Write("U,MLPM" + "\r");
                    await Task.Delay(500);
                    readLength = serialPort.Read(buffer, 0, buffer.Length);

                    serialPort.Write("M,D" + "\r");
                    await Task.Delay(500);
                    readLength = serialPort.Read(buffer, 0, buffer.Length);

                    serialPort.Write("S,240" + "\r");
                    await Task.Delay(500);
                    readLength = serialPort.Read(buffer, 0, buffer.Length);

                    serialPort.Write("S" + "\r");

                    //debug serialPort.Write(commandTextBox.Text +"\r");
                    // Delay 2 seconds before reading buffer
                    await Task.Delay(500);
                    try
                    {
                        serialPort.ReadTimeout = 500;
                        readLength = serialPort.Read(buffer, 0, buffer.Length);
                        //debug.Text = "Debug: ReadLength = " + readLength.ToString();
                    }
                    catch (TimeoutException ex)
                    {
                        log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(
                        // "DFC Port not connected" + ex.ToString(), "Command not sent", MessageBoxButtons.OK,
                        //  MessageBoxIcon.Warning);
                        log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                        return " ";
                    }
                    if (buffer.Length > 0)
                    {
                        // Convert byte buffer to string
                        returnString = System.Text.Encoding.UTF8.GetString(buffer, 0, readLength);
                        //debug.Text = returnString;
                    }
                    return returnString;
                }
                else
                {
                    return string.Empty;
                }
            }

            catch (TimeoutException ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return string.Empty;
            }
        }

        public async Task ConnectTossCooling()
        {
            try
            {
                //if (TECCheckBox.Checked)
                await Task.Delay(30000);
                {
                    await Task.Run(async () =>
                    {
                        if (!ssCoolingSerialPort.IsOpen)
                        {
                            if (!string.IsNullOrEmpty(TCubeCOMPort))
                            {
                                ssCoolingSerialPort.PortName = TCubeCOMPort;
                            }
                            ssCoolingSerialPort.BaudRate = 57600;
                            ssCoolingSerialPort.Open();
                            envStatusRunStatus = true;
                        }
                        if (ssCoolingSerialPort.IsOpen)
                        {
                            envStatusRunStatus = true;
                            try
                            {
                                string TCubestatus = await getTemp(ssCoolingSerialPort);

                                if (TCubestatus.Length > 2)
                                {
                                // powerString = getTemp(powerSupply1SerialPort);
                                if (TCubestatus.Substring(0, 2) == "0.0")
                                    {
                                        TCubestatus = await getTemp(ssCoolingSerialPort);
                                        if (TCubestatus.Substring(0, 2) == "0.0")
                                            TCubestatus = await getTemp(ssCoolingSerialPort);
                                    }
                                    envStatusRunStatus = true;
                                    _ = startStatusThread();
                                }
                            }
                            catch (Exception ex)
                            {
                            //MessageBox.Show("Exception in Solid State Cooling System", ex.ToString(), MessageBoxButtons.OK,
                            //     MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                                return;
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task<string> getTemp(System.IO.Ports.SerialPort serialPort)
        {
            try
            {
                string bufferString = "";
                int readLength = 0;

                if (serialPort.IsOpen)
                {
                    // Get Temperature and Flow rate from TCube edge cooling system
                    serialPort.Write("GETSET2" + "\r");

                    // Delay 2 seconds before reading buffer
                    //Thread.Sleep(2000);
                    await Task.Delay(2000);
                    byte[] buffer = new byte[60];

                    try
                    {
                        if (serialPort.IsOpen)
                        {
                            serialPort.ReadTimeout = 5000;
                            readLength = serialPort.Read(buffer, 0, buffer.Length);
                            //debug.Text = "Debug: ReadLength = " + readLength.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(
                        // "TCube COM Port not connected" + ex.ToString(), "Command not sent", MessageBoxButtons.OK,
                        //  MessageBoxIcon.Warning);
                        log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                        return " ";
                    }

                    if (buffer.Length > 0)
                    {
                        // Convert byte buffer to string
                        bufferString = System.Text.Encoding.UTF8.GetString(buffer, 0, readLength);
                    }

                    return bufferString;
                }
                else
                {
                    //MessageBox.Show(
                    //     "TCube COM Port not connected", "Command not sent", MessageBoxButtons.OK,
                    //      MessageBoxIcon.Warning);
                    log.Error("Exception in" + MethodBase.GetCurrentMethod() + "TCube COM Port not connected");
                    return " ";
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        private async Task startStatusThread()
        {
            try
            {
                //if (TECCheckBox.Checked)
                {
                    await Task.Run(async () =>
                   {
                       while (envStatusRunStatus)
                       {
                           string TCubestatus = await getTemp(ssCoolingSerialPort);      
                           if(TCubestatus !=null)
                           {
                               string[] arr = TCubestatus.Replace("\r", "\n").Split("\n".ToCharArray());

                               if (arr.Length > 0)
                               {                                   
                                   //environmentalStatusForm.CoolantTempVal = arr[0];
                                   if(arr[0].Length > 0 && !string.IsNullOrWhiteSpace(arr[0]) && !string.IsNullOrEmpty(arr[0]))
                                   {
                                       BeginInvoke(new System.Action(() => environmentalStatusForm.CoolantTempVal = arr[0]));
                                       if (Convert.ToDouble(arr[0]) >= environmentalStatusPresenter.EnvironmentalStatusLimitsDataList[0].CoolantTempLowerLimit &&
                                          Convert.ToDouble(arr[0]) <= environmentalStatusPresenter.EnvironmentalStatusLimitsDataList[0].CoolantTempUpperLimit)
                                       {
                                           environmentalStatusForm.CoolantTempPassed = true;
                                           EnvStatusPassed = true;
                                       }
                                       else
                                       {
                                           environmentalStatusForm.CoolantTempPassed = false;                                          
                                           EnvStatusPassed = false;
                                       }
                                       if (coolantTempsDuringFinalTest != null)
                                       {
                                           coolantTempsDuringFinalTest.Add(environmentalStatusForm.CoolantTempPassed);
                                       }
                                   }                                   
                               }
                               if (arr.Length > 1)
                               {
                                   BeginInvoke(new System.Action(() => environmentalStatusForm.SetPointTempVal = arr[1]));
                                   //environmentalStatusForm.SetPointTempVal = arr[1];
                               }
                               if (arr.Length > 2)
                               {
                                   BeginInvoke(new System.Action(() => environmentalStatusForm.PumpTempVal = arr[2]));
                                   //environmentalStatusForm.PumpTempVal = arr[2];
                               }
                               if (arr.Length > 3)
                               {
                                   BeginInvoke(new System.Action(() => environmentalStatusForm.PWMCoolingVal = arr[3]));
                                   //environmentalStatusForm.PWMCoolingVal = arr[3];
                               }
                               if (arr.Length > 4)
                               {
                                   BeginInvoke(new System.Action(() => environmentalStatusForm.FanSpeedVal = arr[4]));
                                   // environmentalStatusForm.FanSpeedVal = arr[4];
                               }
                               if (arr.Length > 5)
                               {
                                   if (arr[5].Length > 0 && !string.IsNullOrWhiteSpace(arr[5]))
                                   {
                                       BeginInvoke(new System.Action(() => environmentalStatusForm.TankLevelVal = arr[5]));
                                       //environmentalStatusForm.TankLevelVal = arr[5];
                                       if (Convert.ToDouble(arr[5]) >= environmentalStatusPresenter.EnvironmentalStatusLimitsDataList[0].TanskLevelLowLimit)
                                       {
                                           environmentalStatusForm.TankLevelPassed = true;
                                           EnvStatusPassed = true;
                                       }
                                       else
                                       {
                                           environmentalStatusForm.TankLevelPassed = false;
                                           EnvStatusPassed = false;
                                       }
                                   }
                                      
                               }
                               if (arr.Length > 6)
                               {                                  
                                   if (arr[6].Length > 0 && !string.IsNullOrWhiteSpace(arr[6]))
                                   {
                                       BeginInvoke(new System.Action(() => environmentalStatusForm.FaultStatusVal = arr[6]));
                                       decimal value;
                                       if (Decimal.TryParse(arr[6], out value))
                                       {
                                           int faultStatus = Convert.ToInt32(arr[6]);
                                           if (faultStatus == environmentalStatusPresenter.EnvironmentalStatusLimitsDataList[0].TCubeFaultStatusLimit)
                                           {
                                               environmentalStatusForm.TCubeFaultStatusPassed = true;
                                               BeginInvoke(new System.Action(() => ultraStatusBar1.Panels["ChillerWaterLevelWarn"].Visible = false));
                                               // EnvStatusPassed = true;
                                           }
                                           else
                                           {
                                               environmentalStatusForm.TCubeFaultStatusPassed = false;
                                               BeginInvoke(new System.Action(() => ultraStatusBar1.Panels["ChillerWaterLevelWarn"].Visible = true));
                                           }
                                       }
                                       else
                                       {
                                           
                                       }
                                          
                                   }                                      
                               }
                               await Task.Delay(1000);
                           }
                           
                       }
                   });
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }

        public async Task ConnectToDCPower()
        {
            try
            {
                string powerString = "0.0";

                await Task.Run(async () =>
                {
                    if (!powerSupplySerialPort.IsOpen)
                    {
                        if (!string.IsNullOrEmpty(BKPowerCOMPort))
                        {
                            powerSupplySerialPort.PortName = BKPowerCOMPort;
                        }
                        powerSupplySerialPort.Open();
                    }

                    if (powerSupplySerialPort.IsOpen)
                    {
                        try
                        {
                            // while (runStatus)
                            //{
                            powerString = await getPower(powerSupplySerialPort);

                            if (powerString.Length > 2)
                            {
                                if (powerString.Substring(0, 2) == "0.0")
                                {
                                    powerString = await getPower(powerSupplySerialPort);
                                    if (powerString.Substring(0, 2) == "0.0")
                                        powerString = await getPower(powerSupplySerialPort);
                                }


                                double.TryParse(powerString.Substring(0, 2), out double volts);
                                double.TryParse(powerString.Substring(2, 3), out double voltsDecimal);
                                string voltString = volts.ToString() + "." + voltsDecimal.ToString();


                                int.TryParse(powerString.Substring(4, 1), out int current1);
                                int.TryParse(powerString.Substring(5, 1), out int current2);
                                int.TryParse(powerString.Substring(6, 1), out int current3);
                                int.TryParse(powerString.Substring(7, 1), out int current4);
                                int.TryParse(powerString.Substring(8, 1), out int current5);

                                string currentString = current1.ToString() + current2.ToString() + "." +
                                      current3.ToString() + current4.ToString() + current5.ToString();

                                double.TryParse(powerString.Substring(5, 1), out double current);
                                double.TryParse(powerString.Substring(6, 3), out double currentDecimal);
                                string currentString1 = current.ToString() + "." + currentDecimal.ToString();

                                BeginInvoke(new System.Action(() => environmentalStatusForm.PowerVoltsVal = voltString + " V"));
                                BeginInvoke(new System.Action(() => environmentalStatusForm.CurrentAmpsVal = currentString + " A"));
                                //environmentalStatusForm.PowerVoltsVal = voltString + " V";
                                //environmentalStatusForm.CurrentAmpsVal = currentString + " A";

                                double voltage = Convert.ToDouble(voltString);
                                double amps = Convert.ToDouble(currentString);
                                if (voltage >= environmentalStatusPresenter.EnvironmentalStatusLimitsDataList[0].VoltageLowerLimit &&
                                    voltage <= environmentalStatusPresenter.EnvironmentalStatusLimitsDataList[0].VoltageUpperLimit)
                                {
                                    environmentalStatusForm.PowerVoltsPassed = true;
                                    EnvStatusPassed = true;
                                }
                                else
                                {
                                    environmentalStatusForm.PowerVoltsPassed = false;
                                    EnvStatusPassed = false;
                                }
                                if (amps >= environmentalStatusPresenter.EnvironmentalStatusLimitsDataList[0].AmpsLowerLimit &&
                                    amps <= environmentalStatusPresenter.EnvironmentalStatusLimitsDataList[0].AmpsUpperLimit)
                                {
                                    environmentalStatusForm.CurrentAmpsPassed = true;
                                    EnvStatusPassed = true;
                                }
                                else
                                {
                                    environmentalStatusForm.CurrentAmpsPassed = false;
                                    EnvStatusPassed = false;
                                }
                            }
                            //powerSupplySerialPort.DataReceived += PowerSupplyPort_DataReceived;
                            envStatusRunStatus = true;
                            _ = startPowerStatusThread();
                            await Task.Delay(500);
                            // }
                        }
                        catch (InvalidOperationException ex)
                        {
                            log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show("Check Power supply" + ex.ToString(), "Power supply may not be on. ", MessageBoxButtons.OK,
                            //        MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                            return;
                        }
                    }
                    else
                    {
                        //MessageBox.Show("Please enable power or select a different COM port before connecting to Power ",
                        //    "COM Port Connection Error",
                        //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        log.Error("Exception in" + MethodBase.GetCurrentMethod() + "COM Port Connection Error");
                    }
                });

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }

        }
        private async Task startPowerStatusThread()
        {
            try
            {
                await Task.Run(async () =>
                {
                    string powerString = string.Empty;
                    while (envStatusRunStatus)
                    {
                        powerString = await getPower(powerSupplySerialPort);
                        if (powerString.Length > 2)
                        {
                            if (powerString.Substring(0, 2) == "0.0")
                            {
                                powerString = await getPower(powerSupplySerialPort);
                                if (powerString.Substring(0, 2) == "0.0")
                                    powerString = await getPower(powerSupplySerialPort);
                            }
                            //powerSupplySerialPort.DataReceived += PowerSupplyPort_DataReceived;

                            double.TryParse(powerString.Substring(0, 2), out double volts);
                            double.TryParse(powerString.Substring(2, 3), out double voltsDecimal);
                            string voltString = volts.ToString() + "." + voltsDecimal.ToString();


                            int.TryParse(powerString.Substring(4, 1), out int current1);
                            int.TryParse(powerString.Substring(5, 1), out int current2);
                            int.TryParse(powerString.Substring(6, 1), out int current3);
                            int.TryParse(powerString.Substring(7, 1), out int current4);
                            int.TryParse(powerString.Substring(8, 1), out int current5);
                            string currentString = current1.ToString() + current2.ToString() + "." +
                                      current3.ToString() + current4.ToString() + current5.ToString();

                            if(camCurrent !=null)
                            {
                                //camCurrent.Add(Convert.ToDouble(currentString));
                                if(beginCamCurrentAvg)
                                {
                                    camCurrent.Add(Tuple.Create(Convert.ToDouble(currentString),1));
                                }
                                else
                                {
                                    camCurrent.Add(Tuple.Create(Convert.ToDouble(currentString), 0));
                                }                                
                            }
                            //double.TryParse(powerString.Substring(5, 1), out double current);
                            //double.TryParse(powerString.Substring(6, 3), out double currentDecimal);
                            //string currentString = current.ToString() + "." + currentDecimal.ToString();
                            BeginInvoke(new System.Action(() => environmentalStatusForm.PowerVoltsVal = voltString + " V"));
                            BeginInvoke(new System.Action(() => environmentalStatusForm.CurrentAmpsVal = currentString + " A"));

                            //environmentalStatusForm.PowerVoltsVal = voltString + " V";
                            //environmentalStatusForm.CurrentAmpsVal = currentString + " A";

                            double voltage = Convert.ToDouble(voltString);
                            double amps = Convert.ToDouble(currentString);
                            if (voltage >= environmentalStatusPresenter.EnvironmentalStatusLimitsDataList[0].VoltageLowerLimit &&
                                voltage <= environmentalStatusPresenter.EnvironmentalStatusLimitsDataList[0].VoltageUpperLimit)
                            {
                                environmentalStatusForm.PowerVoltsPassed = true;
                                EnvStatusPassed = true;
                            }
                            else
                            {
                                environmentalStatusForm.PowerVoltsPassed = false;
                                EnvStatusPassed = false;
                            }
                            if (amps >= environmentalStatusPresenter.EnvironmentalStatusLimitsDataList[0].AmpsLowerLimit &&
                                amps <= environmentalStatusPresenter.EnvironmentalStatusLimitsDataList[0].AmpsUpperLimit)
                            {
                                environmentalStatusForm.CurrentAmpsPassed = true;
                                EnvStatusPassed = true;
                            }
                            else
                            {
                                environmentalStatusForm.CurrentAmpsPassed = false;
                                EnvStatusPassed = false;
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
        private async Task<string> getPower(System.IO.Ports.SerialPort serialPort)
        {
            string bufferString = "";

            if (serialPort.IsOpen)
            {
                // Get voltage and current from DC Power supply
                serialPort.Write("GETD" + "\r\n");

                // Delay 2 seconds before reading buffer
                await Task.Delay(5000);
                byte[] buffer = new byte[20];

                try
                {
                    if (serialPort.IsOpen)
                    {
                        serialPort.ReadTimeout = 5000;
                        serialPort.Read(buffer, 0, buffer.Length);
                    }

                }
                catch (TimeoutException ex)
                {
                    //MessageBox.Show(
                    //"Power Supply COM Port not connected" + ex.ToString(), "Command not sent", MessageBoxButtons.OK,
                    //MessageBoxIcon.Warning);
                    log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                    return " ";
                }
                catch (IOException ex)
                {
                    log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                    return " ";
                }
                catch (Exception ex)
                {
                    log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                    return " ";
                }
                if (buffer.Length > 0)
                {
                    // Convert byte buffer to string
                    bufferString = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                }
                return bufferString;
            }
            else
            {
                //MessageBox.Show(
                //     "COM Port not connected", "Command not sent", MessageBoxButtons.OK,
                //      MessageBoxIcon.Warning);
                log.Error("Exception in" + MethodBase.GetCurrentMethod() + "COM Port not connected");
                return " ";
            }
        }
        public TempHumReadingModel GetTempHumReadingModel(string TestStage)
        {
            try
            {                
                TempHumReadingModel tempHumReadingModel = new TempHumReadingModel();
                tempHumReadingModel.CameraSerialNumber = cameraSerialNumber;
                tempHumReadingModel.ReadingDateTime = DateTime.Now;
                tempHumReadingModel.TestStage = TestStage;
                tempHumReadingModel.TestStation = Environment.MachineName;
                tempHumReadingModel.TestResultsID = null;
                if (camHumdityListForDBLogging == null)
                {
                    tempHumReadingModel.ImagerHumidity = cameraStatusForLogTemHum.RelativeHumidityInProcent;
                }
                else
                {
                    if (TestStage.Equals("PRE-TEST") && camHumdityListForDBLogging == null)
                    {
                        tempHumReadingModel.ImagerHumidity = cameraStatusForLogTemHum.RelativeHumidityInProcent;
                    }
                    else
                    {
                        tempHumReadingModel.ImagerHumidity = camHumidityDBLoggingAvg;
                    }
                } 
                
                tempHumReadingModel.ImagerTemperature = cameraStatusForLogTemHum.ImageSensorTemperatureDiodeInC;
                tempHumReadingModel.UserExecuted= string.IsNullOrWhiteSpace(enggUserName) ? Environment.UserName.ToUpper() : enggUserName.ToUpper();
                return tempHumReadingModel;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }
        public async Task CheckTCPConnection(CancellationTokenSource cancellationTokenSource)
        {
            try
            {
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    //  heartbeat sent every second
                    await Task.Delay(2000, cancellationTokenSource.Token);
                    if (!cidInterface.IsConnected())
                    {
                        if(!ultraStatusBar1.IsDisposed)
                        {
                            ultraStatusBar1.Panels["CameraStatus"].Text = "Camera Disconnected";
                            ultraStatusBar1.Panels["CameraStatus"].Appearance.BackColor = Color.Red;
                        }                       
                        ((ToolBase)(mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["CameraConnection"])).SharedProps.AppearancesSmall.Appearance.Image =
                            new Bitmap(Assembly.GetEntryAssembly().GetManifestResourceStream("KronosCameraTestApp.Resources.Camera_Off.bmp"));
                        log.Error("Camera is disconnected because of no heart beat");
                        break;
                    }
                    else
                    {
                        //AutoConnectCamera();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void updateCameraTestMetrics()
        {
            try
            {
               controlPanelExplorerBar.Groups["controlPanelCameraTestMetrics"].Visible = true;
               string cameraSN = textBoxCameraSerialNum.Text.Trim('\0');
               List<CameraTestLogsModel> cameraTestLogs= await TestAppHelper.GetCameraTestMetrics(cameraSN.Trim());
                if(cameraTestLogs.Count > 0)
                {
                    await updateCameraTestMetricsView(cameraTestLogs);
                }
               
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task updateCameraTestMetricsView(List<CameraTestLogsModel> cameraTestLogs)
        {
            try
            {
                await Task.Run(() =>
                {
                    int preTestPassCount = 0;
                    int preTestFailCount = 0;
                    int finalTestPassCount = 0;
                    int finalTestFailCount = 0;
                    foreach (CameraTestLogsModel cameralog in cameraTestLogs)
                    {
                        switch (cameralog.TestStage)
                        {
                            case "PRE-TEST":
                                if (cameralog.PreTestResult)
                                {
                                    preTestPassCount++;
                                    BeginInvoke(new System.Action(() => preTestPassCountText.Text = preTestPassCount.ToString()));
                                }
                                else
                                {
                                    preTestFailCount++;
                                    BeginInvoke(new System.Action(() => preTestFailCountText.Text = preTestFailCount.ToString()));
                                }

                                break;
                            case "FINAL_TEST":
                                if (cameralog.FinalTestResult)
                                {
                                    finalTestPassCount++;
                                    BeginInvoke(new System.Action(() => finalTestPassCountText.Text = finalTestPassCount.ToString()));
                                }
                                else
                                {
                                    finalTestFailCount++;
                                    if(finalTestFailCountText.InvokeRequired)
                                        BeginInvoke(new System.Action(() => finalTestFailCountText.Text = finalTestFailCount.ToString()));
                                }
                                break;
                        }
                    }
                    if (preTestPassCount > 0)
                    {
                        try
                        {
                            BeginInvoke(new System.Action(() => preTestYield.Text = ((preTestPassCount * 100) / (preTestPassCount + preTestFailCount)).ToString()));
                        }
                        catch (Exception ex)
                        {
                            log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                        }
                    }
                    if (preTestPassCount == 0)
                    {
                        BeginInvoke(new System.Action(() => preTestPassCountText.Text = string.Empty));
                        BeginInvoke(new System.Action(() => preTestYield.Text = string.Empty));
                    }
                    if (preTestFailCount == 0)
                    {
                        BeginInvoke(new System.Action(() => preTestFailCountText.Text = string.Empty));
                    }
                    if (finalTestPassCount > 0)
                    {
                        try
                        {
                            BeginInvoke(new System.Action(() => finalTestYield.Text = ((finalTestPassCount * 100) / (finalTestPassCount + finalTestFailCount)).ToString()));
                        }
                        catch (Exception ex)
                        {
                            log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                        }
                    }
                    if (finalTestPassCount == 0)
                    {
                        BeginInvoke(new System.Action(() => finalTestPassCountText.Text = string.Empty));
                        BeginInvoke(new System.Action(() => finalTestYield.Text = string.Empty));
                    }
                    if (finalTestFailCount == 0)
                    {
                        BeginInvoke(new System.Action(() => finalTestFailCountText.Text = string.Empty));
                    }
                    //BeginInvoke(new System.Action(() => preTestPassCountText.Text = preTestPassCount.ToString()));
                    //BeginInvoke(new System.Action(() => preTestFailCountText.Text = preTestFailCount.ToString()));
                    //if (preTestPassCount > 0)
                    //    BeginInvoke(new System.Action(() => preTestYield.Text = ((preTestPassCount * 100) / (preTestPassCount + preTestFailCount)).ToString()));
                    //BeginInvoke(new System.Action(() => finalTestPassCountText.Text = finalTestPassCount.ToString()));
                    //BeginInvoke(new System.Action(() => finalTestFailCountText.Text = finalTestFailCount.ToString()));
                    //if(finalTestPassCount > 0)
                    //    BeginInvoke(new System.Action(() => finalTestYield.Text = ((finalTestPassCount * 100) / (finalTestPassCount + finalTestFailCount)).ToString()));
                    updateCameraBurnInMetrics();
                });                
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void updateCameraBurnInMetrics()
        {
            try
            {
                int burninPassCount = 0;
                int burninFailCount = 0;
                List<BurnInAnalysisModel> cameraBurnInDetails;
                List<BurnInAnalysisModel> burnInAnalysisDataList = new List<BurnInAnalysisModel>();
                burnInAnalysisDataList = TestAppHelper.GetBurnInAnalysisData();
                if (burnInAnalysisDataList != null && burnInAnalysisDataList.Count > 0)
                {
                    if (string.IsNullOrEmpty(textBoxCameraSerialNum.Text))
                    {
                        cameraBurnInDetails = burnInAnalysisDataList.Where(imager => imager.ImagerSN.Equals(textBoxImagerSerialNum.Text.Trim('\0').Trim())).ToList();                        
                    }
                    else
                    {
                        cameraBurnInDetails = burnInAnalysisDataList.Where(camera => camera.CameraSN.Equals(textBoxCameraSerialNum.Text.Trim('\0').Trim())).ToList();
                    }
                    if (cameraBurnInDetails != null && cameraBurnInDetails.Count() > 0)
                    {
                        foreach (BurnInAnalysisModel burnInAnalysisModel in cameraBurnInDetails)
                        {
                            if (burnInAnalysisModel != null)
                            {
                                if (burnInAnalysisModel.BurnInResult)
                                    burninPassCount++;
                                else
                                    burninFailCount++;
                            }
                        }
                        BeginInvoke(new System.Action(() => burnInPassCountText.Text = burninPassCount.ToString()));
                        BeginInvoke(new System.Action(() => burnInFailCountText.Text = burninFailCount.ToString()));
                        if (burninPassCount > 0)
                            BeginInvoke(new System.Action(() => burnInYieldText.Text = ((burninPassCount * 100) / (burninPassCount + burninFailCount)).ToString()));
                    }
                }
                    
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void UpdateCameraDetailsOnScreen(CameraInformation cameraInformation)
        {
			try
			{
				textBoxFirmwareVer.Text = cameraInformation.FirmwareRevision.ToString();
                if(cameraInformation.MACAddress !=null)
				    textBoxMacAddress.Text = new string(cameraInformation.MACAddress);
				textBoxCameraType.Text = cameraInformation.CameraType.ToString();
				textBoxCameraVer.Text = cameraInformation.CameraVersion.ToString();
				textBoxCameraAPIVer.Text = cameraInformation.CameraApiVersion.ToString();
				textBoxFPGAVer.Text = cameraInformation.FPGAVersion.ToString();
				textBoxBoardSuppPckgVer.Text = cameraInformation.BoardSupportPackageVersion.ToString();
				updateSerialNumbers(cameraInformation);                
                if(!string.IsNullOrEmpty(cameraSerialNumber))
                {
                    // await tempHumidPresenter.SaveTempHumReading(GetTempHumReadingModel("Inital Connect"));

                    tempHumReadingModelInitialConnect = new TempHumReadingModel();
                    tempHumReadingModelInitialConnect.CameraSerialNumber = cameraSerialNumber;
                    tempHumReadingModelInitialConnect.ReadingDateTime = DateTime.Now;
                    tempHumReadingModelInitialConnect.TestStage = "Initial Connect";
                    tempHumReadingModelInitialConnect.TestStation = Environment.MachineName;
                    tempHumReadingModelInitialConnect.ImagerHumidity = cameraStatusForLogTemHum.RelativeHumidityInProcent;
                    tempHumReadingModelInitialConnect.ImagerTemperature = cameraStatusForLogTemHum.ImageSensorTemperatureDiodeInC; ;
                    tempHumReadingModelInitialConnect.UserExecuted = string.IsNullOrWhiteSpace(enggUserName) ? Environment.UserName.ToUpper() : enggUserName.ToUpper();
                }
                
            }
			catch (Exception ex)
			{
				log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
			}
		}
		private async void updateSerialNumbers(CameraInformation cameraInformation)
		{
			try
			{
				string serialNumbers = string.Empty;
				if (cameraInformation.SerialNumbers.CPUSerialNumber != null)
				{
					serialNumbers = new string(cameraInformation.SerialNumbers.CPUSerialNumber);
					serialNumbers=serialNumbers.TrimEnd('\0');
					if (!(string.IsNullOrWhiteSpace(serialNumbers)) && !(serialNumbers.Equals("Unknown")))
					{
						textBoxCameraSerialNum.Visible = textBoxCPUSerialNum.Visible = textBoxCSPSerialNum.Visible = textBoxPwrSerialNum.Visible =
						textBoxImagerSerialNum.Visible = textBoxISISerialNum.Visible =
						ultraLabelCameraSerialNum.Visible = ultraLabelCPUSerialNum.Visible = ultraLabelCSPSerialNum.Visible = ultraLabelPWRSerialNum.Visible =
						ultraLabelImagerSerialNum.Visible = ultraLabelISISerialNum.Visible = true;
						textBoxCameraSerialNum.Text = new string(cameraInformation.SerialNumbers.CameraSerialNumber);
                        cameraSerialNumber = textBoxCameraSerialNum.Text.Trim('\0').TrimEnd();
                        textBoxCPUSerialNum.Text = new string(cameraInformation.SerialNumbers.CPUSerialNumber);
						textBoxCSPSerialNum.Text = new string(cameraInformation.SerialNumbers.CSPSerialNumber);
						textBoxPwrSerialNum.Text = new string(cameraInformation.SerialNumbers.PWRBoardSerialNumber);
						textBoxImagerSerialNum.Text = new string(cameraInformation.SerialNumbers.ImagerSerialNumber);
						textBoxISISerialNum.Text = new string(cameraInformation.SerialNumbers.ISISerialNumber);
                        List<CameraDetailsModel> cameraDetailsModels = await cameraDetailsPresenter.GetCameraDetailsModel(cameraSerialNumber);
                        if (cameraDetailsModels.Count() >0)
                        {
                            CameraDetailsModel cameraDetailsModel = cameraDetailsModels.Where(cam => cam.CRA != null && cam.CRA != "").FirstOrDefault();
                            if (cameraDetailsModel != null)
                            {
                               textBoxCRA.Text = cameraDetailsModel.CRA;
                            }
                        }
                    }
				}
				else
				{
					textBoxCameraSerialNum.Visible = textBoxCPUSerialNum.Visible = textBoxCSPSerialNum.Visible = textBoxPwrSerialNum.Visible =
					textBoxImagerSerialNum.Visible = textBoxISISerialNum.Visible =
					ultraLabelCameraSerialNum.Visible = ultraLabelCPUSerialNum.Visible = ultraLabelCSPSerialNum.Visible = ultraLabelPWRSerialNum.Visible =
					ultraLabelImagerSerialNum.Visible = ultraLabelISISerialNum.Visible = false;
				}

                
            }
			catch (Exception ex)
			{
				log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
			}
		}
        private void ApplyInfraStyle(object sender, ToolEventArgs e)
        {
            try
            {
                if (((ComboBoxTool)mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Apply Styles"]).SelectedIndex != -1)
                {
                    theStyleFile = ((ComboBoxTool)mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Apply Styles"]).SelectedItem.ToString();
                    Properties.Settings.Default.InfraStyleName = theStyleFile;
                    stylePath = File.Exists("InfraStyles/" + theStyleFile + ".isl");
                    if (theStyleFile == "Default")
                    {
                        Infragistics.Win.AppStyling.StyleManager.Reset();
                        log.Info("Application Theme/Style Reset to Default");
                    }
                    else
                    {
                        Infragistics.Win.AppStyling.StyleManager.Load(stylePath ? "InfraStyles/" + theStyleFile + ".isl" : assemblyPath + "//" + theStyleFile + ".isl");
                        log.Info("Application Theme/Style Changed");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task LoadInfraStyles()
        {
            try
            {
                await Task.Run(() =>
                    {
                        Invoke(new System.Action(() =>
                        {
                            ((ComboBoxTool)mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Apply Styles"]).ValueList.ValueListItems.Clear();
                            ((ComboBoxTool)mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Apply Styles"]).ValueList.ValueListItems.Add("Default", "Default");
                            string[] theFiles = Directory.GetFiles(assemblyPath, "InfraStyles\\*.isl");
                            if (theFiles.Length > 0)
                            {
                                foreach (string theFile in theFiles)
                                {
                                    ((ComboBoxTool)mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Apply Styles"]).
                                    ValueList.ValueListItems.Add(theFile, Path.GetFileNameWithoutExtension(theFile));
                                }
                            }
                            ((ComboBoxTool)mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Apply Styles"]).SelectedIndex = 0;
                        }));
                    });
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraExplorerBar1_ItemClick(object sender, Infragistics.Win.UltraWinExplorerBar.ItemEventArgs e)
        {
            try
            {
                cidDataList.Clear();
                cidIntegratedDataList.Clear();
                ResetProgressBar();
                switch (e.Item.Tag.ToString())
                {
                    case "Dashboard":
                        ManualTestDashBoard();
                        break;
                    case "meanvarianceForm":
                        ManualMeanVarianceTest();
                        break;
                    case "photoresponseForm":
                        ManualPhotoresponseTest();
                        break;
                    case "defectsDarkCurrrentForm":
                        ManualDarkCurrentTest();
                        break;
                    case "readnoiseNDROForm":
                        ManualNoiseVsNDROTest();
                        break;
                    case "injectionEfficiencyForm":
                        ManualInjectionPerformanceFormTest();
                        break;                   
                    case "shutterDriveRS232Form":
                        ManaualShutterDriveTest();
                        break;
                    case "RedBlueTest":
                        ManualRedBlueTest();
                        break;
                    case "defectsForm":
                        ManualDefectsTest();
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ResetProgressBar()
        {
            try
            {
                progressNum = 0;
                progressPretestNum = 0;
                if (ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value == 100)
                    ultraStatusBar1.Panels["MarqueeStatus"].Text = string.Empty;
                    ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ImageTest()
        {
            try
            {
                if (AutoTestStatus)
                {
                    showAutoTestMessageBox("Auto Test is under process. Please wait until the test is completed");
                    return;
                }
                if (AutoPreTestStatus)
                {
                    showAutoTestMessageBox("Auto PreTest is under process. Please wait until the test is completed");
                    return;
                }
                currentRunningTest = "ImageTest";
                ResetAutoManualStatusBarPanel();
                imageDisplayPanel = this.MdiChildren.OfType<ImageDisplayPanel>().SingleOrDefault();
                autoTest = false;
                if (imageDisplayPanel == null || TestAppHelper.UserModeChanged)
                {
                    imageDisplayPanel = new ImageDisplayPanel(imageDisplayPanelPresenter, UserMode, loginStatus, cidInterface, ultraStatusBar1);
                    imageDisplayPanel.MdiParent = this;
                    imageDisplayPanel.Dock = DockStyle.Fill; 
                    imageDisplayPanel.CidInterface = cidInterface;
                    imageDisplayPanel.Show();
                }
                else if (imageDisplayPanel.WindowState == FormWindowState.Minimized)
                    imageDisplayPanel.WindowState = FormWindowState.Normal;
                else
                {
                    imageDisplayPanel.Visible = true;
                    imageDisplayPanel.Activate();
                    imageDisplayPanel.BringToFront();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ManualRedBlueTest()
        {
            try
            {
                if (AutoTestStatus)
                {
                    showAutoTestMessageBox("Auto Test is under process. Please wait until the test is completed");
                    return;
                }
                if (AutoPreTestStatus)
                {
                    showAutoTestMessageBox("Auto PreTest is under process. Please wait until the test is completed");
                    return;
                }
                currentRunningTest = "RedBlue";
                //firmwareManualTestLogsFile = "RedBlue_ManualTestFWLog_" + textBoxCameraSerialNum.Text.Trim('\0').TrimEnd()+ "_"+ DateTime.Now.ToString("yymmddThhmm") + ".txt";
                redBlueTest = this.MdiChildren.OfType<RedBlueTestForm>().SingleOrDefault();
                autoTest = false;
                ResetAutoManualStatusBarPanel();
                if (redBlueTest == null || TestAppHelper.UserModeChanged)
                {
                    redBlueTest = new RedBlueTestForm(redBluePresenter, UserMode, loginStatus, cidInterface, this);
                    redBlueTest.MdiParent = this;
                    redBlueTest.CidInterface = cidInterface;
                    redBlueTest.LEDCalibrationPresenter = this.ledCalibrationPresenter;
                    redBlueTest.Show();
                }
                else if (redBlueTest.WindowState == FormWindowState.Minimized)
                    redBlueTest.WindowState = FormWindowState.Normal;
                else
                {
                    redBlueTest.Visible = true;
                    redBlueTest.Activate();
                    redBlueTest.BringToFront();
                }
                RedBlueGraphPreference();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ManualTestDashBoard()
        {
            try
            {
                ultraStatusBar1.Panels["MarqueeStatus"].Text = "Loading Dashboard...";
                ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Start();
                dashBoardForm = this.MdiChildren.OfType<DashBoard>().SingleOrDefault();
                autoTest = false;
                ResetAutoManualStatusBarPanel();
                if (AutoTestStatus)
                {
                    showAutoTestMessageBox("Auto Test is under process. Please wait until the test is completed");
                    return;
                }
                if (AutoPreTestStatus)
                {
                    showAutoTestMessageBox("Auto PreTest is under process. Please wait until the test is completed");
                    return;
                }
                if (dashBoardForm == null || TestAppHelper.UserModeChanged)
                {
                    dashBoardForm = new DashBoard(meanVariancePresenter, noiseVsNDROPresenter,
                                                  photoresponsePresenter, injectionEfficiencyPresenter, darkCurrentPresenter,defectsPresenter, shutterDrivePresenter,
                                                 redBluePresenter, UserMode, loginStatus, enggUserName, cidInterface, ultraStatusBar1);
                    dashBoardForm.MdiParent = this;
                    dashBoardForm.CidInterface = cidInterface;                    
                    dashBoardForm.Show();                   
                }
                else if (dashBoardForm.WindowState == FormWindowState.Minimized)
                    dashBoardForm.WindowState = FormWindowState.Normal;
                else
                    dashBoardForm.Activate();
                ultraStatusBar1.Panels["MarqueeStatus"].Text = string.Empty;
                ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ManaualShutterDriveTest()
        {
            try
            {
                if (AutoTestStatus)
                {
                    showAutoTestMessageBox("Auto Test is under process. Please wait until the test is completed");
                    return;
                }
                if (AutoPreTestStatus)
                {
                    showAutoTestMessageBox("Auto PreTest is under process. Please wait until the test is completed");
                    return;
                }
                currentRunningTest = "ShutterDrive";
                //firmwareManualTestLogsFile = "ShutterDrive_ManualTesFWtLog_" + textBoxCameraSerialNum.Text.Trim('\0').TrimEnd() + "_" + DateTime.Now.ToString("yymmddThhmm") + ".txt";
                shutterDriveTestForm = this.MdiChildren.OfType<ShutterDriveTestForm>().SingleOrDefault();
                autoTest = false;
                ResetAutoManualStatusBarPanel();
                if (shutterDriveTestForm == null || TestAppHelper.UserModeChanged)
                {
                    shutterDriveTestForm = new ShutterDriveTestForm(shutterDrivePresenter, UserMode, loginStatus, cidInterface, this);
                    shutterDriveTestForm.MdiParent = this;
                    shutterDriveTestForm.CidInterface = cidInterface;
                    shutterDriveTestForm.Show();
                }
                else if (shutterDriveTestForm.WindowState == FormWindowState.Minimized)
                    shutterDriveTestForm.WindowState = FormWindowState.Normal;
                else
                {
                    shutterDriveTestForm.Visible = true;
                    shutterDriveTestForm.Activate();
                    shutterDriveTestForm.BringToFront();
                }
                ShutterDriveGraphPreference();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ManualMeanVarianceTest()
        {
            try
            {
                if (AutoTestStatus)
                {
                    showAutoTestMessageBox("Auto Test is under process. Please wait until the test is completed");
                    return;
                }
                if (AutoPreTestStatus)
                {
                    showAutoTestMessageBox("Auto PreTest is under process. Please wait until the test is completed");
                    return;
                }
                currentRunningTest = "MeanVariance";
                if(this.MdiChildren.OfType<MeanVarianceTestForm>().Count() > 0)
                {

                }
                //firmwareManualTestLogsFile = "MeanVariance_ManualTesFWtLog_" + textBoxCameraSerialNum.Text.Trim('\0').TrimEnd() + "_" + DateTime.Now.ToString("yymmddThhmm") + ".txt";
                meanVarianceTestForm = this.MdiChildren.OfType<MeanVarianceTestForm>().SingleOrDefault();
                autoTest = false;
                ResetAutoManualStatusBarPanel();
                exposureCount = meanVariancePresenter.ExposureCount;
                meanVariancePresenter.EnggUserName = enggUserName;
                if (meanVarianceTestForm == null || TestAppHelper.UserModeChanged)
                {
                    meanVarianceTestForm = new MeanVarianceTestForm(meanVariancePresenter, UserMode, loginStatus, cidInterface, this);
                    meanVarianceTestForm.MdiParent = this;
                    meanVarianceTestForm.CidInterface = cidInterface;
                    meanVarianceTestForm.TopLevel = false;
                    meanVarianceTestForm.FormBorderStyle = FormBorderStyle.None;
                    meanVarianceTestForm.Show();
                }
                else if (meanVarianceTestForm.WindowState == FormWindowState.Minimized)
                    meanVarianceTestForm.WindowState = FormWindowState.Normal;
                else
                {
                    meanVarianceTestForm.Visible = true;
                    meanVarianceTestForm.Activate();
                    meanVarianceTestForm.BringToFront();
                }
                MeanVarianceGraphPreference();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }      
        private void ManualDarkCurrentTest()
        {
            try
            {
                if (AutoTestStatus)
                {
                    showAutoTestMessageBox("Auto Test is under process. Please wait until the test is completed");
                    return;
                }
                if (AutoPreTestStatus)
                {
                    showAutoTestMessageBox("Auto PreTest is under process. Please wait until the test is completed");
                    return;
                }
                //firmwareManualTestLogsFile = "DarkCurrent_ManualTesFWtLog_" + textBoxCameraSerialNum.Text.Trim('\0').TrimEnd() + "_" + DateTime.Now.ToString("yymmddThhmm") + ".txt";
                currentRunningTest = "DarkCurrent";
                darkCurrentTestForm = this.MdiChildren.OfType<DarkCurrentTestForm>().SingleOrDefault();
                ResetAutoManualStatusBarPanel();
                autoTest = false;
                if (darkCurrentTestForm == null || TestAppHelper.UserModeChanged)
                {
                    darkCurrentTestForm = new DarkCurrentTestForm(darkCurrentPresenter, UserMode, loginStatus, cidInterface, this);
                    darkCurrentTestForm.MdiParent = this;
                    darkCurrentTestForm.CidInterface = cidInterface;
                    darkCurrentTestForm.Show();
                }
                else if (darkCurrentTestForm.WindowState == FormWindowState.Minimized)
                    darkCurrentTestForm.WindowState = FormWindowState.Normal;
                else
                {
                    darkCurrentTestForm.Visible = true;
                    darkCurrentTestForm.Activate();
                    darkCurrentTestForm.BringToFront();
                }
                DarkCurrentGraphPreference();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ManualDefectsTest()
        {
            try
            {
                if (AutoTestStatus)
                {
                    showAutoTestMessageBox("Auto Test is under process. Please wait until the test is completed");
                    return;
                }
                if (AutoPreTestStatus)
                {
                    showAutoTestMessageBox("Auto PreTest is under process. Please wait until the test is completed");
                    return;
                }
                //firmwareManualTestLogsFile = "Defects_ManualTesFWtLog_" + textBoxCameraSerialNum.Text.Trim('\0').TrimEnd() + "_" + DateTime.Now.ToString("yymmddThhmm") + ".txt";
                currentRunningTest = "DefectsTest";
                defectsTestForm = this.MdiChildren.OfType<DefectsTestForm>().SingleOrDefault();
                ResetAutoManualStatusBarPanel();               
                autoTest = false;
                if (defectsTestForm == null || TestAppHelper.UserModeChanged)
                {
                    defectsTestForm = new DefectsTestForm(defectsPresenter, UserMode, loginStatus, cidInterface, this);
                    defectsTestForm.MdiParent = this;
                    defectsTestForm.CidInterface = cidInterface;
                    defectsTestForm.ImagerSerialNumber = textBoxImagerSerialNum.Text.Trim('\0').Trim();
                    defectsTestForm.Show();
                }
                else if (defectsTestForm.WindowState == FormWindowState.Minimized)
                {
                    defectsTestForm.ImagerSerialNumber = textBoxImagerSerialNum.Text.Trim('\0').Trim();
                    defectsTestForm.WindowState = FormWindowState.Normal;
                }                   
                else
                {
                    defectsTestForm.ImagerSerialNumber = textBoxImagerSerialNum.Text.Trim('\0').Trim();
                    defectsTestForm.Visible = true;
                    defectsTestForm.Activate();
                    defectsTestForm.BringToFront();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ManualNoiseVsNDROTest()
        {
            try
            {
                if (AutoTestStatus)
                {
                    showAutoTestMessageBox("Auto Test is under process. Please wait until the test is completed");
                    return;
                }
                if (AutoPreTestStatus)
                {
                    showAutoTestMessageBox("Auto PreTest is under process. Please wait until the test is completed");
                    return;
                }
               // firmwareManualTestLogsFile = "NoiseVsNDROs_ManualTesFWtLog_" + textBoxCameraSerialNum.Text.Trim('\0').TrimEnd() + "_" + DateTime.Now.ToString("yymmddThhmm") + ".txt";
                currentRunningTest = "NoiseVsNDRO";
                noiseVsNDROTestForm = this.MdiChildren.OfType<NoiseVsNDROTestForm>().SingleOrDefault();
                ResetAutoManualStatusBarPanel();
                autoTest = false;
                if (noiseVsNDROTestForm == null || TestAppHelper.UserModeChanged)
                {
                    noiseVsNDROTestForm = new NoiseVsNDROTestForm(noiseVsNDROPresenter, UserMode, loginStatus, cidInterface, this);
                    noiseVsNDROTestForm.MdiParent = this;
                    noiseVsNDROTestForm.CidInterface = cidInterface;
                    noiseVsNDROTestForm.Show();
                }
                else if (noiseVsNDROTestForm.WindowState == FormWindowState.Minimized)
                    noiseVsNDROTestForm.WindowState = FormWindowState.Normal;
                else
                {
                    noiseVsNDROTestForm.Visible = true;
                    noiseVsNDROTestForm.Activate();
                    noiseVsNDROTestForm.BringToFront();
                }
                NoiseVsNDROsGraphPreference();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ManualInjectionPerformanceFormTest()
        {
            try
            {
                if (AutoTestStatus)
                {
                    showAutoTestMessageBox("Auto Test is under process. Please wait until the test is completed");
                    return;
                }
                if (AutoPreTestStatus)
                {
                    showAutoTestMessageBox("Auto PreTest is under process. Please wait until the test is completed");
                    return;
                }
                //firmwareManualTestLogsFile = "InjectionPerf_ManualTesFWtLog_" + textBoxCameraSerialNum.Text.Trim('\0').TrimEnd() + "_" + DateTime.Now.ToString("yymmddThhmm") + ".txt";
                currentRunningTest = "InjectionEfficiency";
                injectionEfficiencyTestForm = this.MdiChildren.OfType<InjectionEfficiencyTestForm>().SingleOrDefault();
                ResetAutoManualStatusBarPanel();
                autoTest = false;
                if (injectionEfficiencyTestForm == null || TestAppHelper.UserModeChanged)
                {
                    injectionEfficiencyTestForm = new InjectionEfficiencyTestForm(injectionEfficiencyPresenter, UserMode, loginStatus, cidInterface, this);
                    injectionEfficiencyTestForm.MdiParent = this;
                    injectionEfficiencyTestForm.CidInterface = cidInterface;
                    injectionEfficiencyTestForm.Show();
                }
                else if (injectionEfficiencyTestForm.WindowState == FormWindowState.Minimized)
                    injectionEfficiencyTestForm.WindowState = FormWindowState.Normal;
                else
                {
                    injectionEfficiencyTestForm.Visible = true;
                    injectionEfficiencyTestForm.Activate();
                    injectionEfficiencyTestForm.BringToFront();
                }
                InjectionPerformanceGraphPreference();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ManualPhotoresponseTest()
        {
            try
            {
                if (AutoTestStatus)
                {
                    showAutoTestMessageBox("Auto Test is under process. Please wait until the test is completed");
                    return;
                }
                if (AutoPreTestStatus)
                {
                    showAutoTestMessageBox("Auto PreTest is under process. Please wait until the test is completed");
                    return;
                }
                currentRunningTest = "Photoresponse";
                //firmwareManualTestLogsFile = "Photoresponse_ManualTesFWtLog_" + textBoxCameraSerialNum.Text.Trim('\0').TrimEnd() + "_" + DateTime.Now.ToString("yymmddThhmm") + ".txt";
                photoresponseTestForm = this.MdiChildren.OfType<PhotoresponseTestForm>().SingleOrDefault();
                ResetAutoManualStatusBarPanel();
                autoTest = false;
                if (photoresponseTestForm == null || TestAppHelper.UserModeChanged)
                {
                    photoresponseTestForm = new PhotoresponseTestForm(photoresponsePresenter, UserMode, loginStatus, cidInterface, this);
                    photoresponseTestForm.MdiParent = this;
                    photoresponseTestForm.CidInterface = cidInterface;
                    photoresponseTestForm.LEDCalibrationPresenter = this.ledCalibrationPresenter;
                    photoresponseTestForm.Show();
                }
                else if (photoresponseTestForm.WindowState == FormWindowState.Minimized)
                    photoresponseTestForm.WindowState = FormWindowState.Normal;
                else
                {
                    photoresponseTestForm.Visible = true;
                    photoresponseTestForm.Activate();
                    photoresponseTestForm.BringToFront();
                }
                PhotoresponseGraphPreference();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }       
        private void InitializeUserControls()
        {
            try
            {
                redBlueUserControl = new RedBlueUserControl(redBluePresenter, testResultsPath, userMode);
                redBlueUserControl.CidInterface = cidInterface;
                meanVarianceTestUserControl = new MeanVarianceUserControl(meanVariancePresenter, testResultsPath, userMode);
                meanVarianceTestUserControl.CidInterface = cidInterface;
                meanVarianceTestUserControl.MeanVariancePresenter = meanVariancePresenter;
                darkCurrentTestUserControl = new DarkCurrentUserControl(darkCurrentPresenter, testResultsPath, userMode);
                darkCurrentTestUserControl.CidInterface = cidInterface;
                defectsUserControl = new DefectsUserControl(defectsPresenter, testResultsPath, userMode,false);
                defectsUserControl.CidInterface = cidInterface;
                noiseVsNDROTestUserControl = new NoiseVsNDROUserControl(noiseVsNDROPresenter, testResultsPath, userMode);
                noiseVsNDROTestUserControl.CidInterface = cidInterface;
                injectionPerformanceTestUserControl = new InjectionEfficiencyUserControl(injectionEfficiencyPresenter, testResultsPath, userMode);
                injectionPerformanceTestUserControl.CidInterface = cidInterface;
                photoresponseTestUserControl = new PhotoresponseUserControl(photoresponsePresenter, testResultsPath, userMode);
                photoresponseTestUserControl.CidInterface = cidInterface;
                shutterDriveUserControl = new ShutterDriveUserControl(shutterDrivePresenter, testResultsPath, userMode);
                shutterDriveUserControl.CidInterface = cidInterface;
                finalTestReportUserControl = new FinalTestReportUserControl(testResultsPath, userMode);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void DisableUserControlTiles()
        {
            try
            {
                dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileMeanVariance"].Enabled = false;
                dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileShutterDrive"].Enabled = false;
                dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileDarkCurrent"].Enabled = false;
                dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileDefects"].Enabled = false;
                dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileReadNoise"].Enabled = false;
                dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileInjectionPerformance"].Enabled = false;
                dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTilePhotoresponse"].Enabled = false;
                dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileRedBlue"].Enabled = false;
                dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileFinalTestReport"].Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void InitializeAuotPreTestUserControls(string[] SelectedPreTests)
        {
            try
            {
                foreach (string str in SelectedPreTests)
                {
                    switch (str)
                    {
                        case "RBT":
                            if (redBlueUserControl != null)
                            {
                                redBlueUserControl = null;
                            }
                            {
                                redBlueUserControl = new RedBlueUserControl(redBluePresenter, testResultsPath, userMode);
                                redBlueUserControl.CidInterface = cidInterface;
                            }
                            break;
                        case "DDT":
                            if (darkCurrentTestUserControl != null)
                            {
                                darkCurrentTestUserControl = null;
                            }
                            {
                                darkCurrentTestUserControl = new DarkCurrentUserControl(darkCurrentPresenter, testResultsPath, userMode);
                                darkCurrentTestUserControl.CidInterface = cidInterface;
                            }
                            break;
                        case "DET":
                            if (defectsUserControl != null)
                            {
                                defectsUserControl = null;
                            }
                            {
                                defectsUserControl = new DefectsUserControl(defectsPresenter, testResultsPath, userMode,true);
                                defectsUserControl.CidInterface = cidInterface;
                            }
                            break;
                        case "MVT":
                            if (meanVarianceTestUserControl != null)
                            {
                                meanVarianceTestUserControl = null;
                            }
                            {
                                meanVarianceTestUserControl = new MeanVarianceUserControl(meanVariancePresenter, testResultsPath, userMode);
                                meanVarianceTestUserControl.CidInterface = cidInterface;
                            }
                            break;
                        case "RNT":
                            if (noiseVsNDROTestUserControl != null)
                            {
                                noiseVsNDROTestUserControl = null;
                            }
                            {
                                noiseVsNDROTestUserControl = new NoiseVsNDROUserControl(noiseVsNDROPresenter, testResultsPath, userMode);
                                noiseVsNDROTestUserControl.CidInterface = cidInterface;
                            }
                            break;
                        case "PRT":
                            if (photoresponseTestUserControl != null)
                            {
                                photoresponseTestUserControl = null;
                            }
                            {
                                photoresponseTestUserControl = new PhotoresponseUserControl(photoresponsePresenter, testResultsPath, userMode);
                                photoresponseTestUserControl.CidInterface = cidInterface;
                            }
                            break;
                        case "IET":
                            if (injectionPerformanceTestUserControl != null)
                            {
                                injectionPerformanceTestUserControl = null;
                            }
                            {
                                injectionPerformanceTestUserControl = new InjectionEfficiencyUserControl(injectionEfficiencyPresenter, testResultsPath, userMode);
                                injectionPerformanceTestUserControl.CidInterface = cidInterface;
                            }
                            break;
                        case "SDT":
                            if (shutterDriveUserControl != null)
                            {
                                shutterDriveUserControl = null;
                            }
                            {
                                shutterDriveUserControl = new ShutterDriveUserControl(shutterDrivePresenter, testResultsPath, userMode);
                                shutterDriveUserControl.CidInterface = cidInterface;
                            }
                            break;
                    }
                }                   
                if (finalTestReportUserControl != null)
                {
                    finalTestReportUserControl = null;
                }
                {
                    finalTestReportUserControl = new FinalTestReportUserControl(testResultsPath, userMode);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void DisablePretestUserControlTiles()
        {
            try
            {
                dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileDarkCurrentPreTest"].Enabled = false;
                dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileDefectsPreTest"].Enabled = false;
                dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileRedBluePreTest"].Enabled = false;
                dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileMeanVariance"].Enabled = false;
                dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileShutterDrive"].Enabled = false;
                dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileReadNoise"].Enabled = false;
                dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileInjectionPerformance"].Enabled = false;
                dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTilePhotoresponse"].Enabled = false;
                dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTilePreTestReport"].Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void MakePretestTilesVisible()
        {
            if (autoPretestForm != null)
            {
                string[] SelectedPreTests = autoPretestForm.SelectedPreTests;
                foreach (string str in SelectedPreTests)
                {
                    switch (str)
                    {
                        case "RBT":
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileRedBluePreTest"].Visible = true;
                            break;
                        case "DDT":
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileDarkCurrentPreTest"].Visible = true;
                            break;
                        case "DET":
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileDefectsPreTest"].Visible = true;
                            break;
                        case "MVT":
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileMeanVariance"].Visible = true;
                            break;
                        case "RNT":
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileReadNoise"].Visible = true;
                            break;
                        case "PRT":
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTilePhotoresponse"].Visible = true;
                            break;
                        case "IET":
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileInjectionPerformance"].Visible = true;
                            break;
                        case "SDT":
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileShutterDrive"].Visible = true;
                            break;
                    }
                }
            }
        }
        private async Task RunAutoMeanVarianceTest()
        {
            try
            {
                if (!AutoCancellationTokenSource.IsCancellationRequested)
                {
                    log.Info("Begin Mean Variance Auto Test");
                    ultraStatusBar1.Panels["MarqueeStatus"].Text = "Mean Variance Test Processing.....";
                    ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Start();
                    currentRunningTest = "MeanVariance";
                    cidDataList.Clear();
                    cidIntegratedDataList.Clear();
                    MeanVarianceGraphPreference();
                    await meanVarianceTestUserControl.RunAutoMVTest(AutoCancellationTokenSource, AutoPreTestStatus);
                    log.Info("End Mean Variance Auto Test");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }      
        private async Task RunAutoDarkCurrentTest()
        {
            try
            {
                if (!AutoCancellationTokenSource.IsCancellationRequested)
                {
                    log.Info("Begin Dark Current Auto Test");
                    ultraStatusBar1.Panels["MarqueeStatus"].Text = "Dark Current Test Processing.....";
                    ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Start();
                    currentRunningTest = "DarkCurrent";
                    cidDataList.Clear();
                    cidIntegratedDataList.Clear();
                    DarkCurrentGraphPreference();
                    await darkCurrentTestUserControl.RunAutoDarkCurrentTest(AutoCancellationTokenSource, AutoPreTestStatus);
                    log.Info("End Dark Current Auto Test");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task RunAutoDefectsTest()
        {
            try
            {
                if (!AutoCancellationTokenSource.IsCancellationRequested)
                {
                    log.Info("Begin Defects Auto Test");
                    ultraStatusBar1.Panels["MarqueeStatus"].Text = "Defects Test Processing.....";
                    ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Start();
                    currentRunningTest = "Defects";
                    cidDataList.Clear();
                    cidIntegratedDataList.Clear();
                   // defectsPresenter.AutoPretest = true;
                    await defectsUserControl.RunAutoDefectsTest(AutoCancellationTokenSource);
                    //defectsPresenter.AutoPretest = false;
                    log.Info("End Defects Auto Test");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task RunAutoNoiseVsNDROTest()
        {
            try
            {
                if (!AutoCancellationTokenSource.IsCancellationRequested)
                {
                    log.Info("Begin NoiseVsNDRO Auto Test");
                    ultraStatusBar1.Panels["MarqueeStatus"].Text = "Noise Vs NDRO Test Processing.....";
                    currentRunningTest = "NoiseVsNDRO";
                    ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Start();
                    cidDataList.Clear();
                    cidIntegratedDataList.Clear();
                    NoiseVsNDROsGraphPreference();
                    await noiseVsNDROTestUserControl.RunAutoNoiseVsNDROsTest(AutoCancellationTokenSource, AutoPreTestStatus);
                    log.Info("End NoiseVsNDRO Auto Test");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unknown error occured while running the test.",
                        "Unknown Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }      
        private async Task RunAutoInjectionPerformanceTest()
        {
            try
            {
                if (!AutoCancellationTokenSource.IsCancellationRequested)
                {
                    log.Info("Begin Injection Performance Auto Test");
                    ultraStatusBar1.Panels["MarqueeStatus"].Text = "Injection Efficiency Test Processing.....";
                    currentRunningTest = "Injection Efficiency";
                    ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Start();
                    cidDataList.Clear();
                    cidIntegratedDataList.Clear();
                    InjectionPerformanceGraphPreference();
                    await injectionPerformanceTestUserControl.RunAutoInjectionPerformanceTest(AutoCancellationTokenSource, AutoPreTestStatus);
                    await injectionPerformanceTestUserControl.RunAutoCrossTalkTest(AutoCancellationTokenSource, AutoPreTestStatus);
                    log.Info("End Injection Performance Auto Test");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task RunAutoPhotoresponseTest()
        {
            try
            {
                if (!AutoCancellationTokenSource.IsCancellationRequested)
                {
                    log.Info("Begin Photoresponse Auto Test");
                    ultraStatusBar1.Panels["MarqueeStatus"].Text = "Photoresponse Test Processing.....";
                    ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Start();
                    currentRunningTest = "Photoresponse";
                    cidDataList.Clear();
                    cidIntegratedDataList.Clear();
                    PhotoresponseGraphPreference();
                    await photoresponseTestUserControl.RunAutoPhotoresponseTest(AutoCancellationTokenSource, AutoPreTestStatus);
                    log.Info("End Photoresponse Auto Test");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task RunAutoShutterDriveTest()
        {
            try
            {
                if (!AutoCancellationTokenSource.IsCancellationRequested)
                {
                    log.Info("Begin ShutterDrive Auto Test");
                    ultraStatusBar1.Panels["MarqueeStatus"].Text = "Shutter Drive Test Processing.....";
                    currentRunningTest = "ShutterDrive";
                    cidDataList.Clear();
                    cidIntegratedDataList.Clear();
                    ShutterDriveGraphPreference();
                    await shutterDriveUserControl.RunAutoShutterDriveTest(AutoCancellationTokenSource, AutoPreTestStatus);
                    log.Info("End ShutterDrive Auto Test");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task RunAutoRedBlue()
        {
            try
            {
                if (!AutoCancellationTokenSource.IsCancellationRequested)
                {
                    log.Info("Begin Red & Blue Auto PreTest");
                    ultraStatusBar1.Panels["MarqueeStatus"].Text = "Red, Blue & UV Test Processing.....";
                    ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Start();
                    currentRunningTest = "RedAndBlueTest";
                    cidDataList.Clear();
                    cidIntegratedDataList.Clear();
                    RedBlueGraphPreference();
                    await redBlueUserControl.RunAutoRedBlueTest(AutoCancellationTokenSource, AutoPreTestStatus);
                    log.Info("End Red & Blue Auto PreTest");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task RunAutoTests()
        {
            try
            {
                environmentalStatusPresenter.GetEnironmentalStatusLimits();
                if (!envStatusRunStatus && environmentalStatusPresenter.COMPortConfigurationModel!=null)
                {
                    ConnectToEnvironmentalStatusCOMPorts(false);
                    await RunEnvStatTest(false);
                }

                ResetAutoManualStatusBarPanel();
                ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Maximum = 100;               
                DateTime testExecutedDate = DateTime.Now;
                string filetimestamp = testExecutedDate.ToString("MM_dd_yyyy_HH_mm");
                TestAppHelper.AzureContainerName = "scm5821ax1-testresults-" + testExecutedDate.ToString("MM-dd-yyyy-hh-mm");
                Application.UseWaitCursor = true;
                Cursor.Current = Cursors.WaitCursor;
                autoTest = true;
                AutoTestStatus = true;
                AutoTestStatusResult = false;
                ledAutoTestResult.Value = false;
                bool camTempFailed = true;
                //autoTestFailReason.Append("Auto Test Failed Due to: ");
                ResetProgressBar();
                dashBoardAutoForm = this.MdiChildren.OfType<DashBoardAutoForm>().SingleOrDefault();
                string[] AutoTestOrder = autoTestForm.OrderofAutoTest;
                if (dashBoardAutoForm != null || (this.ActiveMdiChild != null && this.ActiveMdiChild.Name == "DashBoardAutoForm"))
                {
                    this.dashBoardAutoForm.Close();
                }
                InitializeUserControls();
                dashBoardAutoForm = new DashBoardAutoForm(meanVarianceTestUserControl,
                                                            darkCurrentTestUserControl, defectsUserControl, noiseVsNDROTestUserControl,
                                                            injectionPerformanceTestUserControl, photoresponseTestUserControl,
                                                            shutterDriveUserControl, redBlueUserControl, finalTestReportUserControl, AutoTestOrder);
                dashBoardAutoForm.FormBorderStyle = FormBorderStyle.None;
                dashBoardAutoForm.ControlBox = false;
                dashBoardAutoForm.MaximizeBox = false;
                dashBoardAutoForm.MinimizeBox = false;
                dashBoardAutoForm.MdiParent = this;
                dashBoardAutoForm.Dock = DockStyle.Fill;
                dashBoardAutoForm.Show();
                ultraStatusBar1.Panels["MarqueeStatus"].Text = "Auto Test Initiated.....";
                DisableUserControlTiles();
                if(cameraStatusForLogTemHum !=null)
                     TestAppHelper.FinalTestReportTemperature = cameraStatusForLogTemHum.ImageSensorTemperatureDiodeInC;
                ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Start();
                Application.UseWaitCursor = false;
                Cursor.Current = Cursors.Default;
                //await tempHumidPresenter.SaveTempHumReading(GetTempHumReadingModel("FINAL_TEST"));

                List<TempHumReadingModel> tempHumReadingModels = new List<TempHumReadingModel>();

                tempHumReadingModels.Add(tempHumReadingModelInitialConnect);

                TempHumReadingModel tempHumReadingModelBeginTest = new TempHumReadingModel();
                tempHumReadingModelBeginTest.CameraSerialNumber = cameraSerialNumber;
                tempHumReadingModelBeginTest.ReadingDateTime = DateTime.Now;
                tempHumReadingModelBeginTest.TestStage = "FINAL_TEST";
                tempHumReadingModelBeginTest.TestStation = Environment.MachineName;
                tempHumReadingModelBeginTest.ImagerHumidity = cameraStatusForLogTemHum.RelativeHumidityInProcent; 
                tempHumReadingModelBeginTest.ImagerTemperature = cameraStatusForLogTemHum.ImageSensorTemperatureDiodeInC; ;
                tempHumReadingModelBeginTest.UserExecuted = string.IsNullOrWhiteSpace(enggUserName) ? Environment.UserName.ToUpper() : enggUserName.ToUpper();

                tempHumReadingModels.Add(tempHumReadingModelBeginTest);

                camTempDBLoggingAvg = 0.0;
                camHumidityDBLoggingAvg = 0.0;
                camCurrentAvg = 0.0;
                camTempHumListForDBLogging = new List<Tuple<double, double>>();
                camCurrent = new List<Tuple<double, int>>();
                coolantTempsDuringFinalTest = new List<bool?>();
                purgeFlowRatesDuringFinalTest = new List<bool?>();
                foreach (string str in AutoTestOrder)
                {                  
                    switch (str)
                    {
                        case "Red&Blue":
                        case "Red, Blue & UV":
                            await RunAutoRedBlue();
                            dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileRedBlue"].Enabled = true;
                            dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileRedBlue"].State = TileState.Large;
                            Thread.Sleep(5000);
                            break;
                        case "Mean Variance":
                            await RunAutoMeanVarianceTest();
                            dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileMeanVariance"].Enabled = true;
                            break;
                        case "Dark Current":
                            await RunAutoDarkCurrentTest();
                            dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileDarkCurrent"].Enabled = true;
                            break;
                        case "Defects":
                            await RunAutoDefectsTest();
                            dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileDefects"].Enabled = true;
                            break;
                        case "Photoresponse":
                            camHumdityListForDBLogging = new List<double>();
                            beginCamCurrentAvg = true;
                            await RunAutoPhotoresponseTest();
                            dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTilePhotoresponse"].Enabled = true;
                            break;
                        case "ReadNoiseVsNDRO":
                        case "ReadNoiseVsNDROs ":
                            await RunAutoNoiseVsNDROTest();
                            dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileReadNoise"].Enabled = true;
                            break;
                        case "Injection Efficiency":
                        case "Injection Performance":
                            await RunAutoInjectionPerformanceTest();                                                    
                            dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileInjectionPerformance"].Enabled = true;
                            break;
                        case "Shutter Drive":
                            await RunAutoShutterDriveTest();
                            camHumidityDBLoggingAvg = camHumdityListForDBLogging.Average();                                                      
                            dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileShutterDrive"].Enabled = true;                            
                            break;
                    }
                    if (AutoCancellationTokenSource.IsCancellationRequested || !cidInterface.IsConnected())
                    {
                        //ultraStatusBar1.Panels["EnvStatus"].Visible = false;
                        ultraStatusBar1.Panels["EnvStatus"].Appearance.BackColor = Color.Transparent;                       
                        break;
                    }
                       
                    //todo
                    if (autoTestForm.AbortAutoTest)
                    {
                        if (checkForAutoTestFailures(str))
                        {
                            if (string.IsNullOrWhiteSpace(strTestfailed))
                            {
                                ultraStatusBar1.Panels["MarqueeStatus"].Text = "Auto Test failed";
                                //ultraStatusBar1.Panels["EnvStatus"].Visible = false;
                            }
                            else
                            {
                                ultraStatusBar1.Panels["MarqueeStatus"].Text = "Auto Test failed due to " + strTestfailed + " failure";
                            }
                            ultraStatusBar1.Panels["MarqueeStatus"].Text.TrimEnd(',', ' ');
                            ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                            ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                            AutoTestStatus = false;
                            AutoTestStatusResult = true;
                            buttonRunAuto.Enabled = true;
                            buttonAbort.Enabled = false;
                            ledAutoTestResult.Value = false;
                            ledAutoTestResult.OffColor = Color.Red;
                            autoTestFailReason.Clear();
                            camTempAlertResult = DialogResult.None;
                            //MessageBox.Show("Auto Test Failed", "Auto Test Status",
                            //                    MessageBoxButtons.OK,
                            //                    MessageBoxIcon.Information);
                            return;
                        }
                    }
                    else
                    {
                        if (checkForAutoTestFailures(str) && !string.IsNullOrEmpty(strTestfailed))
                        {
                            autoTestFailReason.Append(" ' "+strTestfailed+" ' , ");                          
                        }
                    }
                }
                List<Tuple<double, double>> camTempHumListForDBLoggingTemp = camTempHumListForDBLogging;
                beginCamCurrentAvg = false;
                camHumdityListForDBLogging = null;
                camTempHumListForDBLogging = null;
                camHumidityDBLoggingAvg = camTempHumListForDBLoggingTemp.Select(t => t.Item1).Average();
                camTempDBLoggingAvg = camTempHumListForDBLoggingTemp.Select(t => t.Item2).Average();


                List<double> filteredCamCurrentList = camCurrent
                                                    .Where(tuple => tuple.Item2 == 1)
                                                    .Select(tuple => tuple.Item1)
                                                    .ToList();               
                camCurrentAvg = filteredCamCurrentList.Average();
                camCurrent = null;

                bool coolantTempFailDuringFinalTest = false;
                bool purgeFlowRateFailDuringFinalTest = false;

                coolantTempFailDuringFinalTest = coolantTempsDuringFinalTest.Any(b => b.HasValue && b.Value == false);
                purgeFlowRateFailDuringFinalTest = purgeFlowRatesDuringFinalTest.Any(b => b.HasValue && b.Value == false);
                coolantTempsDuringFinalTest = null;
                purgeFlowRatesDuringFinalTest = null;
                if (Math.Abs(camTempDBLoggingAvg) >= tempHumidPresenter.TemperatureHumidityLimitsDataList[0].MinTemp
                 && Math.Abs(camTempDBLoggingAvg) <= tempHumidPresenter.TemperatureHumidityLimitsDataList[0].MaxTemp)
                {
                    camTempAlertDisplayed = camTempFailed = false;
                }
                else
                {
                    camTempAlertDisplayed = camTempFailed = true;
                }

                TempHumReadingModel tempHumReadingModelEndTest = new TempHumReadingModel();
                tempHumReadingModelEndTest.CameraSerialNumber = cameraSerialNumber;
                tempHumReadingModelEndTest.ReadingDateTime = DateTime.Now;
                tempHumReadingModelEndTest.TestStage = "FINAL_TEST_Avg";
                tempHumReadingModelEndTest.TestStation = Environment.MachineName;
                tempHumReadingModelEndTest.ImagerHumidity = camHumidityDBLoggingAvg;
                tempHumReadingModelEndTest.ImagerTemperature = camTempDBLoggingAvg;
                tempHumReadingModelEndTest.UserExecuted = string.IsNullOrWhiteSpace(enggUserName) ? Environment.UserName.ToUpper() : enggUserName.ToUpper();
                tempHumReadingModels.Add(tempHumReadingModelEndTest);


                if (AutoCancellationTokenSource.IsCancellationRequested)
                {
                    DisableUserControlTiles();
                    ultraStatusBar1.Panels["MarqueeStatus"].Text = "Auto Test Aborted";
                    ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                    ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                    dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileRedBlue"].State = TileState.Normal;                                      
                    //ultraStatusBar1.Panels["EnvStatus"].Visible = false;
                    //environmentalStatusPresenter.DisconnectCOMPorts();
                    ultraStatusBar1.Panels["EnvStatus"].Appearance.BackColor = Color.Transparent;
                    // defectsPresenter.AutoPretest = false;
                    AutoTestStatus = false;
                    buttonRunAuto.Enabled = true;
                    buttonAbort.Enabled = false;
                    AutoTestStatusResult = false;
                    ledAutoTestResult.Value = false;
                    ledAutoTestResult.OffColor = Color.DarkGreen;
                    autoTestFailReason.Clear();
                    camTempAlertResult = DialogResult.None;
                    return;
                }
                else if(!cidInterface.IsConnected())
                {
                    DialogResult dialogResult = MessageBox.Show("Communication to camera/firmware is lost. Auto test will be aborted." +
                                                                "Please check for camera/firmware connection.",
                                                                "Final Test", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        DisableUserControlTiles();
                        ultraStatusBar1.Panels["MarqueeStatus"].Text = "Auto Test Aborted due to camera/firmware connection loss.";
                        ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                        ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                        dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileRedBlue"].State = TileState.Normal;
                    //environmentalStatusPresenter.RunStatus = false;
                    //ultraStatusBar1.Panels["EnvStatus"].Visible = false;
                    //environmentalStatusPresenter.DisconnectCOMPorts();
                    ultraStatusBar1.Panels["EnvStatus"].Appearance.BackColor = Color.Transparent;
                    AutoTestStatus = false;
                        camTempAlertResult = DialogResult.None;
                        buttonRunAuto.Enabled = true;
                        buttonAbort.Enabled = false;
                        AutoTestStatusResult = false;
                        ledAutoTestResult.Value = false;
                        ledAutoTestResult.OffColor = Color.DarkGreen;
                        autoTestFailReason.Clear();
                        return;
                }
                else if (!meanVariancePresenter.MeanVarianceTestPassed || !darkCurrentPresenter.DarkCurrentTestPassed || 
                        !defectsPresenter.DefectsTestPassed ||
                        !photoresponsePresenter.PhotoresponseTestPassed || !noiseVsNDROPresenter.NoiseVsNDROsTestPassed ||
                        !injectionEfficiencyPresenter.InjectionEfficiencyTestPassed || !redBluePresenter.RedBlueTestPassed || 
                        !shutterDrivePresenter.ShutterDriveTestPassed ||
                        !injectionEfficiencyPresenter.ColCrossTalkTestPassed || !injectionEfficiencyPresenter.RowCrossTalkTestPassed||
                        firmwareErrorsWarnings.Count > 0 || camTempFailed ||
                        camCurrentAvg > environmentalStatusPresenter.EnvironmentalStatusLimitsDataList[0].AmpsUpperLimit)
                {
                    
                    autoTestFailReason.Append(" ' " + strTestfailed + " ' , ");
                    if (camTempFailed)
                    {
                        autoTestFailReason.Append(" ' " + "Camera Temp" + " ' , ");
                    }
                    if (camCurrentAvg > environmentalStatusPresenter.EnvironmentalStatusLimitsDataList[0].AmpsUpperLimit)
                    {
                        autoTestFailReason.Append(" ' " + "Camera Current" + " ' , ");
                    }
                    autoTestFailReason.ToString().TrimEnd(',', ' ');
                    ultraStatusBar1.Panels["MarqueeStatus"].Text = autoTestFailReason.ToString();
                    ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                    AutoTestStatus = false;
                    AutoTestStatusResult = true;
                    buttonRunAuto.Enabled = true;
                    buttonAbort.Enabled = false;
                    ledAutoTestResult.Value = false;
                    ledAutoTestResult.OffColor = Color.Red;
                    
                }
                else
                {
                    ultraStatusBar1.Panels["MarqueeStatus"].Text = "Auto Test Passed";                   
                    ledAutoTestResult.Value = true;
                    autoTestFailReason.Clear();
                    autoTestFailReason.Append("None");                   
                }
                ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 100;
                ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                if (dashBoardAutoForm.WindowState == FormWindowState.Minimized)
                    dashBoardAutoForm.WindowState = FormWindowState.Normal;
                else
                    dashBoardAutoForm.Activate();
                string serverPathToDB = string.Empty;
                string testResultsServerPath = string.Empty;
                Settings.Default.dryRunTest = dryRunTest.Checked;
                if (!dryRunTest.Checked)
                {                    
                    if (string.IsNullOrEmpty(defaultFilePath))
                    {
                        testResultsPath = System.IO.Path.Combine(ConfigurationManager.AppSettings["TestResultsLocalFilePath"].ToString()
                            + "SCM5821A_TestResults_" + filetimestamp + "_" + cameraDetailsModel.CameraSerialNumber);
                        if (!System.IO.Directory.Exists(System.IO.Path.Combine(testResultsPath)))
                        {
                            System.IO.Directory.CreateDirectory(testResultsPath);
                        }
                    }
                    else
                    {
                        testResultsPath = System.IO.Path.Combine(defaultFilePath, "SCM5821A_TestResults\\" + "SCM5821A_TestResults_"
                            + filetimestamp + "_" + cameraDetailsModel.CameraSerialNumber);
                        if (!System.IO.Directory.Exists(System.IO.Path.Combine(testResultsPath)))
                        {
                            System.IO.Directory.CreateDirectory(testResultsPath);
                        }
                    }
                    //test
                    string fileName = string.Empty;
                    string destFile = string.Empty;
                    serverPathToDB = "SCM5821A_TestResults_" + filetimestamp + "_" + cameraDetailsModel.CameraSerialNumber;
                    log.Info("RunAutoTests before server dir created.");
                    ParametersModel parametersModel = TestAppHelper.GetParametersModel();
                    string path = string.Empty;
                    if (!string.IsNullOrEmpty(parametersModel.TestResultsServerPath.ToString()))
                        path = parametersModel.TestResultsServerPath.ToString();
                    else
                        path = ConfigurationManager.AppSettings["TestResultsServerFilePath"].ToString();
                    log.Info("Test results server path." + path);
                    testResultsServerPath = System.IO.Path.Combine(path, "SCM5821A_TestResults_" +
                                                                        filetimestamp + "_" + cameraDetailsModel.CameraSerialNumber);
                    log.Info("Test results server path."+ testResultsServerPath);
                    if (ConfigurationManager.AppSettings["EndClient"] == "Thermo")
                    {
                        if (!System.IO.Directory.Exists(testResultsServerPath))
                        {
                            System.IO.Directory.CreateDirectory(testResultsServerPath);
                        }
                    }
                    ExcelUtilities.FilePath = testResultsPath;
                    log.Info("RunAutoTests before SaveCharacterizationTestResultImages.");
                    SaveCharacterizationTestResultImages(testResultsPath, testResultsServerPath, filetimestamp, serverPathToDB, true);
                    log.Info("RunAutoTests after SaveCharacterizationTestResultImages.");
                }
                redBlueTestResults = redBluePresenter.ReturnRedBlueResults();
                meanVarianceTestResultsData = meanVariancePresenter.ReturnMVResults();
                noiseVsNDROTestResults = noiseVsNDROPresenter.ReturnNoiseVsNDROsResults();
                darkCurrentTestResults = darkCurrentPresenter.ReturnDarkCurrentResults();
                defectsTestResults = defectsPresenter.ReturnDefectsResults();
                injectionEfficiencyTestResults = injectionEfficiencyPresenter.ReturnInjectionEfficiencyResults();
                photoresponseTestResults = photoresponsePresenter.ReturnPhotoresponseResults(); 
                 shutterDriveTestResults = shutterDrivePresenter.ReturnShutterDriveResults();
               

                EnvironmentalStatusModel environmentalStatusModel= PopulateEnvironmentalStatusModel(environmentalStatusPresenter);               

                PopulateFinalTestResultsModel();
                cidInterface.GetCameraInformation();
                cameraDetailsModel.FirmwareVer = textBoxFirmwareVer.Text;
                cameraDetailsModel.FPGAVersion = textBoxFPGAVer.Text;
                cameraDetailsModel.MACAddress = textBoxMacAddress.Text;
                cameraDetailsModel.FirmwareFile = TestAppHelper.GetFirmwareFileName();
                List<BurnInAnalysisModel> cameraBurnInDetails = TestAppHelper.GetBurnInAnalysisData().Where(camera => camera.CameraSN.Equals(cameraDetailsModel.CameraSerialNumber)).ToList();
                if (cameraBurnInDetails !=null && cameraBurnInDetails.Count() > 0)
                    cameraBurnInData = cameraBurnInDetails[0];
                string[] selectedTests = new string[] { "RBT","DDT","DET","MVT","RNT","PRT","IET","SDT" };

                finalTestReportUserControl.CamCurrentAvg = camCurrentAvg.ToString("F2");
                finalTestReportUserControl.ImageTemperature = camTempDBLoggingAvg.ToString("F2");
                finalTestReportUserControl.ImageHumidity = camHumidityDBLoggingAvg.ToString("F2");               
                finalTestReportUserControl.UpdateFinalTestReportView(finalTestResultsDetailsModel, cameraDetailsModel,
                                                                     shutterDriveTestResults, 
                                                                      cameraInformation, cameraBurnInData, enggUserName, 
                                                                      testExecutedDate,true, selectedTests,
                                                                      camTempAlertDisplayed, firmwareErrorsWarnings,
                                                                      defectsPresenter.DefPixelROIListPairs, 
                                                                      defectsPresenter.DefectsTestLimitsDataList, environmentalStatusForm,
                                                                      camCurrentAvg > environmentalStatusPresenter.EnvironmentalStatusLimitsDataList[0].AmpsUpperLimit?true: false,
                                                                      environmentalStatusPresenter.EnvironmentalStatusLimitsDataList[0],
                                                                      tempHumidPresenter.TemperatureHumidityLimitsDataList[0],
                                                                      coolantTempFailDuringFinalTest, purgeFlowRateFailDuringFinalTest);
                dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileFinalTestReport"].Enabled = true;
                if (!dryRunTest.Checked)
                    finalTestReportUserControl.SaveFinalTestReportAsImage(testResultsPath, testResultsServerPath, filetimestamp);
                bool finalTestResult = finalTestReportUserControl.ReturnTestPassFailConditon(finalTestResultsDetailsModel);
                if (firmwareErrorsWarnings.Count > 0)
                {
                    if (autoTestFailReason.ToString().Contains("None"))
                    {
                        autoTestFailReason.Clear();
                        finalTestResult = false;
                    }                   
                    else
                    {
                        foreach (string errorStr in firmwareErrorsWarnings)
                        {
                            autoTestFailReason.AppendLine(errorStr);                            
                        }
                    }
                    ultraStatusBar1.Panels["MarqueeStatus"].Text = autoTestFailReason.ToString();
                }              
                cameraDetailsModel.CameraTestLogs = new List<CameraTestLogsModel>();
                cameraDetailsModel.CameraTestLogs.Add(populateCameraTestLogs(finalTestResult, "FINAL_TEST", string.Empty));               
                if (!dryRunTest.Checked)
                {
                    await overAllTestResultsPresenter.SaveOverAllTestResults(finalTestResultsDetailsModel, cameraDetailsModel, redBlueTestResults, meanVarianceTestResultsData,
                                         noiseVsNDROTestResults, darkCurrentTestResults, defectsTestResults, injectionEfficiencyTestResults, photoresponseTestResults,
                                         shutterDriveTestResults, filetimestamp, serverPathToDB,
                                         CameraSerialNumber, ImagerSerialNumber, userMode, autoTestFailReason.ToString(),
                                         finalTestResult, cameraBurnInData, false, true, environmentalStatusModel, tempHumReadingModels);

                    string filename = DateTime.Now.ToString("yymmddThhmmss") + "_" + CameraSerialNumber.TrimEnd(' ') + "_FinalTest.txt";
                    string finaltestFirmwareLogsLocalPath = System.IO.Path.Combine(testResultsPath, "SCM5821AX1FirmwareOutput");
                    if (!System.IO.Directory.Exists(finaltestFirmwareLogsLocalPath))
                    {
                        System.IO.Directory.CreateDirectory(finaltestFirmwareLogsLocalPath);
                    }
                    string finaltestFirmwareLogsServerPath = System.IO.Path.Combine(testResultsServerPath, "SCM5821AX1FirmwareOutput");
                    if (!System.IO.Directory.Exists(finaltestFirmwareLogsServerPath))
                    {
                        System.IO.Directory.CreateDirectory(finaltestFirmwareLogsServerPath);
                    }
                    firmwareOutput.AppendLine("Total DeadAndDarkPixelList Count:"+ defectsPresenter.DeadAndDarkPixelList.Count().ToString());
                    File.AppendAllText(finaltestFirmwareLogsLocalPath + "\\" + filename, firmwareOutput.ToString());
                    File.AppendAllText(finaltestFirmwareLogsServerPath + "\\" + filename, firmwareOutput.ToString());
                    firmwareOutput.Clear();

                  
                    defectsPresenter.SerializeObjectFileName = finaltestFirmwareLogsServerPath + "\\DefectsTestSerializeObject_" +
                        textBoxImagerSerialNum.Text.Trim('\0').Trim() + "_" + DateTime.Now.ToString("YYMMDDThhmmss") + ".bin";

                    await defectsPresenter.serializeObject();
                }
                if (TECCheckBox.Checked)
                {
                    TECCheckBox.Checked = false;
                    cidInterface.DisableThermoElectricCooler();
                }
                
                dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileFinalTestReport"].State = TileState.Large;                

                if (!EnvStatusPassed)
                {
                    if(environmentalStatusForm == null || environmentalStatusForm.IsDisposed)
                    {
                        environmentalStatusForm= new EnvironmentalStatus(environmentalStatusPresenter, this);
                        environmentalStatusForm.Show();
                    }
                }
                else
                {
                }
                //ultraStatusBar1.Panels["EnvStatus"].Visible = false;
                ultraStatusBar1.Panels["EnvStatus"].Appearance.BackColor = Color.Transparent;
                AutoTestStatusResult = true;
                buttonRunAuto.Enabled = true;
                buttonAbort.Enabled = false;
                AutoTestStatus = false;
                camTempAlertResult = DialogResult.None;
                camTempAlertDisplayed = false;
                firmwareErrorsWarnings.Clear();
               
                if (cameraBurnInData != null)
                {
                    if (!dryRunTest.Checked)
                    {
                        await copyBurnInResultFiles(cameraBurnInData.BurnInResultsPath, testResultsPath, testResultsServerPath);
                    }
                }
                updateCameraTestMetrics();
                if (ledAutoTestResult.Value)
                {
                    MessageBox.Show("Auto Test Passed", "Auto Test Status",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Application.UseWaitCursor = false;
                Cursor.Current = Cursors.Default;
                ledAutoTestResult.Value = false;
                AutoTestStatusResult = false;
                AutoTestStatus = false;
                buttonRunAuto.Enabled = true;
                buttonAbort.Enabled = false;
                autoTestFailReason.Clear();
                camTempAlertResult = DialogResult.None;
                camTempAlertDisplayed = false;
                firmwareErrorsWarnings.Clear();
                dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileRedBlue"].State = TileState.Normal;
                environmentalStatusPresenter.RunStatus = false;
                //ultraStatusBar1.Panels["EnvStatus"].Visible = false;
                environmentalStatusPresenter.DisconnectCOMPorts();
                ultraStatusBar1.Panels["EnvStatus"].Appearance.BackColor = Color.Transparent;
                ResetProgressBar();
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private EnvironmentalStatusModel PopulateEnvironmentalStatusModel(EnvironmentalStatusPresenter environmentalStatusPresenter)
        {
            try
            {
                EnvironmentalStatusModel environmentalStatusModel = new EnvironmentalStatusModel();
                environmentalStatusModel.TestStation = Environment.MachineName;
                environmentalStatusModel.Voltage =Convert.ToDouble(environmentalStatusForm.PowerVoltsVal.Trim(new char[] { 'V' }));
                environmentalStatusModel.Amps = Convert.ToDouble(environmentalStatusForm.CurrentAmpsVal.Trim(new char[] { 'A' }));
                environmentalStatusModel.Date = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(environmentalStatusForm.PurgeFlowRateVal) && !string.IsNullOrEmpty(environmentalStatusForm.PurgeFlowRateVal))
                {
                    environmentalStatusModel.PurgeFlowRate = Convert.ToDouble(environmentalStatusForm.PurgeFlowRateVal);
                }               

                if (!string.IsNullOrWhiteSpace(environmentalStatusForm.CoolantTempVal) && !string.IsNullOrEmpty(environmentalStatusForm.CoolantTempVal))
                {
                    environmentalStatusModel.CoolantTemp = Convert.ToDouble(environmentalStatusForm.CoolantTempVal);
                }
                if (!string.IsNullOrWhiteSpace(environmentalStatusForm.TankLevelVal) && !string.IsNullOrEmpty(environmentalStatusForm.TankLevelVal))
                {
                    environmentalStatusModel.TankLevelLow = Convert.ToDouble(environmentalStatusForm.TankLevelVal);
                }
                if (!string.IsNullOrWhiteSpace(environmentalStatusForm.FaultStatusVal) && !string.IsNullOrEmpty(environmentalStatusForm.FaultStatusVal))
                {
                    environmentalStatusModel.TCubeFaultStatus = Convert.ToInt32(environmentalStatusForm.FaultStatusVal);
                }
                if (!string.IsNullOrWhiteSpace(environmentalStatusForm.SetPointTempVal) && !string.IsNullOrEmpty(environmentalStatusForm.SetPointTempVal))
                {
                    environmentalStatusModel.SetPointTemp = Convert.ToDouble(environmentalStatusForm.SetPointTempVal);
                }
                if (!string.IsNullOrWhiteSpace(environmentalStatusForm.PWMCoolingVal) && !string.IsNullOrEmpty(environmentalStatusForm.PWMCoolingVal))
                {
                    environmentalStatusModel.PMWCooling = Convert.ToDouble(environmentalStatusForm.PWMCoolingVal);
                }
                if (!string.IsNullOrWhiteSpace(environmentalStatusForm.FanSpeedVal) && !string.IsNullOrEmpty(environmentalStatusForm.FanSpeedVal))
                {
                    environmentalStatusModel.FanSpeed = Convert.ToDouble(environmentalStatusForm.FanSpeedVal);
                }
                if (!string.IsNullOrWhiteSpace(environmentalStatusForm.PumpTempVal) && !string.IsNullOrEmpty(environmentalStatusForm.PumpTempVal))
                {
                    environmentalStatusModel.PumpTemp = Convert.ToDouble(environmentalStatusForm.PumpTempVal);
                }               
                environmentalStatusModel.UserExecuted= string.IsNullOrWhiteSpace(enggUserName) ? Environment.UserName.ToUpper() : enggUserName.ToUpper();

                return environmentalStatusModel;
            }
            catch(Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }


        }
        private async Task copyBurnInResultFiles(string sourceDirectory, string localTargetDirectory, string serverTargetDirectory)
        {
            try
            {
                await Task.Run(() =>
                {
                    DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
                    DirectoryInfo diLocalTarget = new DirectoryInfo(localTargetDirectory + "\\BurnInResults\\");
                    DirectoryInfo diServerTarget = new DirectoryInfo(serverTargetDirectory + "\\BurnInResults\\");
                    burnInDataFilesServerPath = serverTargetDirectory + "\\BurnInResults\\";
                    Directory.CreateDirectory(diLocalTarget.FullName);
                    Directory.CreateDirectory(diServerTarget.FullName);
                    // Copy each file into the new directory.
                    foreach (FileInfo fi in diSource.GetFiles())
                    {                        
                        fi.CopyTo(Path.Combine(diLocalTarget.FullName, fi.Name), true);
                        fi.CopyTo(Path.Combine(diServerTarget.FullName, fi.Name), true);
                    }
                });
            }
            catch(Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void SaveCharacterizationTestResultImages(string testResultsLocalPath, string testResultsServerPath, 
                                                        string localFileTimeStamp, string serverPathToDB, bool savetoNetwork)
        {
            try
            {
                    redBlueUserControl.SaveTestResultImage(testResultsLocalPath, testResultsServerPath, localFileTimeStamp, serverPathToDB, savetoNetwork);
                    meanVarianceTestUserControl.SaveTestResultImage(testResultsLocalPath, testResultsServerPath, localFileTimeStamp, serverPathToDB, savetoNetwork);
                    darkCurrentTestUserControl.SaveTestResultImage(testResultsLocalPath, testResultsServerPath, localFileTimeStamp, serverPathToDB, savetoNetwork);
                    defectsUserControl.SaveTestResultImage(testResultsLocalPath, testResultsServerPath, localFileTimeStamp, serverPathToDB, savetoNetwork);
                    injectionPerformanceTestUserControl.SaveTestResultImage(testResultsLocalPath, testResultsServerPath, localFileTimeStamp, serverPathToDB, savetoNetwork);
                    noiseVsNDROTestUserControl.SaveTestResultImage(testResultsLocalPath, testResultsServerPath, localFileTimeStamp, serverPathToDB, savetoNetwork);
                    photoresponseTestUserControl.SaveTestResultImage(testResultsLocalPath, testResultsServerPath, localFileTimeStamp, serverPathToDB, savetoNetwork);
                    shutterDriveUserControl.SaveTestResultImage(testResultsLocalPath, testResultsServerPath, localFileTimeStamp, serverPathToDB, savetoNetwork);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private bool checkForAutoTestFailures(string str)
        {
            try
            {
                if ((str == "Red&Blue" || str == "RBT" || str== "Red, Blue & UV") && !redBluePresenter.UVTestPassed)
                {
                    if(!autoTestFailReason.ToString().Contains("Red, Blue & UV"))
                    {
                        strTestfailed = "Red, Blue & UV";
                    }                    
                    return true;
                }
                if ((str == "Mean Variance" || str == "MVT") && !meanVariancePresenter.MeanVarianceTestPassed)
                {
                    strTestfailed = "Mean Variance :{" + meanVariancePresenter.MeanVarianceTestFailureReason + "}";
                    return true;
                }
                else if ((str == "Dark Current" || str == "DDT") && !darkCurrentPresenter.DarkCurrentTestPassed)
                {
                    strTestfailed = "Dark Current  :{" + darkCurrentPresenter.DarkCurrentTestFailureReason + "}";
                    return true;
                }
                else if ((str == "Defects" || str == "DET") && !defectsPresenter.DefectsTestPassed)
                {
                    strTestfailed = "Defects Test :{" + defectsPresenter.DefectsTestFailureReason + "}";
                    return true;
                }
                else if ((str == "Photoresponse" || str == "PRT") && !photoresponsePresenter.PhotoresponseTestPassed)
                {
                    strTestfailed = "Photoresponse :{" + photoresponsePresenter.PhotoresponseTestFailureReason + "}";
                    return true;
                }
                else if ((str == "ReadNoiseVsNDRO" ||  str == "ReadNoiseVsNDROs "  || str == "RNT") && !noiseVsNDROPresenter.NoiseVsNDROsTestPassed)
                {
                    strTestfailed = "ReadNoiseVsNDRO :{" + noiseVsNDROPresenter.NoisVsNDROsTestFailureReason + "}";
                    return true;
                }
                else if ((str == "Injection Efficiency" || str == "IET" || str == "Injection Performance") && 
                    (!injectionEfficiencyPresenter.InjectionEfficiencyTestPassed || !injectionEfficiencyPresenter.RowCrossTalkTestPassed || !injectionEfficiencyPresenter.ColCrossTalkTestPassed))

                {
                    strTestfailed = "Injection Efficiency/Crosstalk :{" + injectionEfficiencyPresenter.InjectionEfficiencyTestFailureReason + "}";
                    return true;
                }
                else if ((str == "Shutter Drive" || str == "SDT") && !shutterDrivePresenter.ShutterDriveTestPassed)
                {
                    strTestfailed = "Shutter Drive";
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Application.UseWaitCursor = false;
                AutoTestStatus = false;
                buttonRunAuto.Enabled = true;
                buttonAbort.Enabled = false;
                camTempAlertResult = DialogResult.None;
                camTempAlertDisplayed = false;
                firmwareErrorsWarnings.Clear();
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }

      
        private void PopulateFinalTestResultsModel()
        {
            try
            {
                finalTestResultsDetailsModel = new FinalTestResultsDetailsModel();
                finalTestResultsDetailsModel.ConversionFactorNominal = meanVariancePresenter.Gain;
                finalTestResultsDetailsModel.TotalNumClusters = defectsPresenter.TotalClusters;
                finalTestResultsDetailsModel.TotalNumDefects = defectsPresenter.TotalDefects;
                finalTestResultsDetailsModel.TotalColumnDefects = defectsPresenter.DarkColumns+ defectsPresenter.LightColumns;
                finalTestResultsDetailsModel.TotalRowDefects = defectsPresenter.DarkRows+ defectsPresenter.LightRows;
                finalTestResultsDetailsModel.DriftROICount = defectsPresenter.DriftROICount;
                finalTestResultsDetailsModel.AveTrap = darkCurrentPresenter.AveTrap;
                finalTestResultsDetailsModel.MaxDarkROI =darkCurrentPresenter.MaxDarkROI;                
                finalTestResultsDetailsModel.FullWellLevelMin = Convert.ToInt32(photoresponsePresenter.FullWell);
                finalTestResultsDetailsModel.SnglNoiseMax = noiseVsNDROPresenter.SignalReadNoise;
                finalTestResultsDetailsModel.RPower2Correlation = noiseVsNDROPresenter.R2Correlation;
                finalTestResultsDetailsModel.InjEfficiencyInitialExposure = injectionEfficiencyPresenter.InitialExposureResultString;
                finalTestResultsDetailsModel.InjEfficiencyFirstInjection = injectionEfficiencyPresenter.FirstInjectionResultString;
                finalTestResultsDetailsModel.InjEfficiencyLastInjection = injectionEfficiencyPresenter.LastInjectionResultString;
                finalTestResultsDetailsModel.ShutterDrive = shutterDrivePresenter.ShutterDriveTestPassed;
                finalTestResultsDetailsModel.RedBlue = redBluePresenter.RedBlueTestPassed;
                finalTestResultsDetailsModel.HotPixels = defectsPresenter.HotPixels;
                finalTestResultsDetailsModel.DarkPixels = defectsPresenter.DarkPixels;
                finalTestResultsDetailsModel.DeadPixels = defectsPresenter.DeadPixels;
                finalTestResultsDetailsModel.UVMean = redBluePresenter.UVMean;
                finalTestResultsDetailsModel.CameraAvgCurrent = camCurrentAvg;
                finalTestResultsDetailsModel.CameraAvgTemperature = camTempDBLoggingAvg;
                if (injectionEfficiencyPresenter.CrossTalkResultData !=null)
                {
                    finalTestResultsDetailsModel.MeanBox1 = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox1")).Value;
                    finalTestResultsDetailsModel.MeanBox2 = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox2")).Value;
                    finalTestResultsDetailsModel.MeanBox3 = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox3")).Value;
                    finalTestResultsDetailsModel.MeanBox4 = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox4")).Value;
                    finalTestResultsDetailsModel.MeanBox5 = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox5")).Value;
                    finalTestResultsDetailsModel.MeanBox6 = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox6")).Value;
                    finalTestResultsDetailsModel.MeanBox7 = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox7")).Value;
                    finalTestResultsDetailsModel.MeanBox8 = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox8")).Value;
                    finalTestResultsDetailsModel.MeanBox9 = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox9")).Value;
                    finalTestResultsDetailsModel.Reference = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("Reference")).Value;
                    finalTestResultsDetailsModel.RowCrossTalk = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("RowCrosstalk")).Value;
                    finalTestResultsDetailsModel.ColCrossTalk = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("ColumnCrosstalk")).Value;
                    finalTestResultsDetailsModel.RelativeSensitivity = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("RelativeSensitivity")).Value;
                }
               
                finalTestResultsDetailsModel.RowXTalkPassed = injectionEfficiencyPresenter.RowCrossTalkTestPassed;
                finalTestResultsDetailsModel.ColXTalkPassed = injectionEfficiencyPresenter.ColCrossTalkTestPassed;
                if (defectsPresenter.DefPixelROIListPairs !=null)
                {
                    List<string> str = new List<string>();
                    foreach (KeyValuePair<string, List<Point>> roipixs in defectsPresenter.DefPixelROIListPairs)
                    {
                        str.Add("ROI:" + roipixs.Key + ":" + String.Join(";", roipixs.Value.ToArray().Select(p => p.ToString()).ToArray()));
                    }
                    finalTestResultsDetailsModel.DefectivePixelPerROI = String.Join(";", str.ToArray().Select(p => p.ToString()).ToArray());
                }
                else
                {
                    finalTestResultsDetailsModel.DefectivePixelPerROI = string.Empty;
                }
                finalTestResultsDetailsModel.DefectsTestDeadDarkPixelLimitReached = defectsPresenter.DeadDarkPixelsLimitReached;

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void showAutoTestMessageBox(string message)
        {
            try
            {
                UltraMessageBoxInfo messageinfo = new UltraMessageBoxInfo();
                messageinfo.Style = MessageBoxStyle.Default;
                messageinfo.Buttons = MessageBoxButtons.OK;
                messageinfo.Caption = "Test Status";
                messageinfo.Text = message;
                ultraMessageBoxManager1.ShowMessageBox(messageinfo);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void mainFormToolBarManager_ToolClick(object sender, ToolClickEventArgs e)
        {
            try
            {
                ResetProgressBar();
                switch (e.Tool.Key)
                {
                    case "Open":
                        OpenDocuments();
                        break;
                    case "TIFF":
                        SaveImage("Tiff");
                        break;
                    case "Bitmap":
                        SaveImage("Bitmap");
                        break;
                    case "JPEG":
                        SaveImage("JPEG");
                        break;
                    case "PNG":
                        SaveImage("PNG");
                        break;
                    case "Page Setup":
                        PageSetup();
                        break;
                    case "Print Preview":
                        PrintPreview();
                        break;
                    case "Print":
                        PrintScreen();
                        break;
                    case "Clear History":
                        ClearHistory();
                        break;
                    case "Exit":
                        ExitApplication();
                        break;
                    case "TCP/IP Settings":
                        TCPIPSettings();                        
                        break;
                    case "EnableFirmwareLogging":
                        StateButtonTool sb = (StateButtonTool)e.Tool;
                        sb.MenuDisplayStyle = StateButtonMenuDisplayStyle.DisplayCheckmark;
                        if (sb.Checked)
                        {
                            fwLoggingEnabled = true;
                        }
                        else
                        {
                            fwLoggingEnabled = false;
                        }
                        break;
                    case "Firmware Log":
                        FirmwareLogging();
                        break;
                    case "Instrument Defaults":
                        if(instrumentDefaults == null || TestAppHelper.UserModeChanged)
                        {
                            instrumentDefaults = new InstrumentDefaults(cidInterface, userMode, enggUserName);
                        }                        
                        instrumentDefaults.ShowDialog(this);
                        break;
                    case "Update Camera Firmware":
                        tcpSettingsForm = new TCPSettingsForm(cidInterface, userMode, enggUserName);
                        DownloadUpdatedFirmware downloadFirmwareDialog =
                         new DownloadUpdatedFirmware(ConfigurationManager.AppSettings["UpdateFirmwareFilePath"].ToString(), 
                         tcpSettingsForm.ultraTextEditorIPAddress.Text.ToString(), Convert.ToInt32(tcpSettingsForm.ultraTextEditorPort.Text), cidInterface);
                        downloadFirmwareDialog.ShowDialog(this);
                        break;
                    case "Run Camera Diagnostics":
                        CameraDiagnostics cameraDiagnostics = new CameraDiagnostics();
                        cameraDiagnostics.ShowDialog(this);
                        break;
                    case "About":
                        aboutForm = new AboutForm(cameraInformation);
                        aboutForm.Show();
                        break;
                    case "User Guide":
                        LoadUserGuide();
                        break;
                    case "Metro":
                        stylePath = File.Exists("InfraStyles/Metro.isl");
                        Infragistics.Win.AppStyling.StyleManager.Load(stylePath ? "InfraStyles/Metro.isl" : assemblyPath + "//.Metro.isl");
                        Properties.Settings.Default.InfraStyleName = "Metro";
                        break;
                    case "Aero":
                        stylePath = File.Exists("InfraStyles/Aero.isl");
                        Infragistics.Win.AppStyling.StyleManager.Load(stylePath ? "InfraStyles/Aero.isl" : assemblyPath + "//Aero.isl");
                        Properties.Settings.Default.InfraStyleName = "Aero";
                        break;
                    case "Trendy":
                        stylePath = File.Exists("InfraStyles/Trendy.isl");
                        Infragistics.Win.AppStyling.StyleManager.Load(stylePath ? "InfraStyles/Trendy.isl" : assemblyPath + "//Trendy.isl");
                        Properties.Settings.Default.InfraStyleName = "Trendy";
                        break;
                    case "MoreStyles":
                        //LoadInfraStyles();
                        break;
                    case "TextBoxToolFolderPath":
                        break;
                    case "ButtonToolBrowseFolderPath":
                        BrowseFolderPath();
                        break;
                    case "ButtonToolHelp":
                        aboutForm = new AboutForm(cameraInformation);
                        aboutForm.Show();
                        break;
                    case "ButtonToolPrint":
                        PrintScreen();
                        break;
                    case "CameraConnection":
                        AutoConnectCamera();
                        await CheckTCPConnection(checkTCPcancellationTokenSource);
                        break;
                    case "Operator Input":
                        OperatorInput();
                        break;
                    case "Test Limits":
                        TestLimits();
                        break;
                    case "OverAll Test Results":
                        OverAllTestResultsForm overAllTestResultsForm = new OverAllTestResultsForm(overAllTestResultsPresenter);
                        overAllTestResultsForm.Show(this);
                        break;
                    case "TestResults":
                        TestResults();
                        break;
                    case "Final Test Report":
                        FinalTestReport();
                        break;
                    case "Test Graph Preferences":
                        TestGraphPreferences();
                        break;
                    case "Send Email":
                        SendOutlookMail();
                        break;
                    case "Image Display Form":
                        ImageTest();
                        break;
                    case "Create User Account":
                        CreateUserAccount();
                        break;
                    case "ZoomIn":
                        if (this.ActiveMdiChild.Name == "ImageDisplayPanel" && imageDisplayPanel != null)
                        {
                            imageDisplayPanel.ZoomIn();
                        }
                        else if (this.ActiveMdiChild.Name == "RedBlueTestForm" && redBlueTest != null)
                        {
                            redBlueTest.ZoomIn();
                        }                        
                        else if (this.ActiveMdiChild.Name == "DashBoardAutoForm" && dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileRedBlue"].State == TileState.Large)
                        {
                            redBlueUserControl.ZoomIn();
                        }
                        else if (this.ActiveMdiChild.Name == "DashBoardAutoPretestForm" && (dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileRedBluePreTest"].State
                            == TileState.Large || dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileRedBluePreTest"].State == TileState.Normal))
                        {
                            redBlueUserControl.ZoomIn();
                        }
                        else if (this.ActiveMdiChild.Name == "DefectsTestForm" && defectsTestForm != null)
                        {
                            defectsTestForm.ZoomIn();
                        }
                        else if (this.ActiveMdiChild.Name == "DashBoardAutoForm" && dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileDefects"].State == TileState.Large)
                        {
                            defectsUserControl.ZoomIn();
                        }
                        else if (this.ActiveMdiChild.Name == "DashBoardAutoPretestForm" && (dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileDefectsPreTest"].State ==
                           TileState.Large || dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileDefectsPreTest"].State == TileState.Normal))
                        {
                            defectsUserControl.ZoomIn();
                        }                       
                        break;
                    case "ZoomOut":
                        if (this.ActiveMdiChild.Name == "ImageDisplayPanel" && imageDisplayPanel != null)
                        {
                            imageDisplayPanel.ZoomOut();
                        }
                        else if (this.ActiveMdiChild.Name == "RedBlueTestForm" && redBlueTest != null)
                        {
                            redBlueTest.ZoomOut();
                        }
                        else if (this.ActiveMdiChild.Name == "DashBoardAutoForm" && dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileRedBlue"].State == TileState.Large)
                        {
                            redBlueUserControl.ZoomOut();
                        }
                        else if (this.ActiveMdiChild.Name == "DashBoardAutoPretestForm" && (dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileRedBluePreTest"].State ==
                           TileState.Large || dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileRedBluePreTest"].State == TileState.Normal))
                        {
                            redBlueUserControl.ZoomOut();
                        }
                        else if (this.ActiveMdiChild.Name == "DefectsTestForm" && defectsTestForm != null)
                        {
                            defectsTestForm.ZoomOut();
                        }
                        else if (this.ActiveMdiChild.Name == "DashBoardAutoForm" && dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileDefects"].State == TileState.Large)
                        {
                            defectsUserControl.ZoomOut();
                        }
                        else if (this.ActiveMdiChild.Name == "DashBoardAutoPretestForm" && (dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileDefectsPreTest"].State ==
                           TileState.Large || dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileDefectsPreTest"].State == TileState.Normal))
                        {
                            defectsUserControl.ZoomOut();
                        }                        
                        break;
                    case "Report Bug":
                        Application.UseWaitCursor = true;
                        CreateTFSBugForm createTFSBug = new CreateTFSBugForm();
                        createTFSBug.Show(this);
                        Application.UseWaitCursor = false;
                        break;
                    case "BurnIn Results":
                        BurnInAnalysisResults burnInAnalysisResults = new BurnInAnalysisResults();
                        burnInAnalysisResults.Show();
                        break;
                    case "Update BurnIn Files":
                        if(updateBurnInFiles == null)
                            updateBurnInFiles = new UpdateBurnInFiles(cidInterface,userMode,loginStatus);
                        if(userMode.Equals("Engineering") || userMode.Equals("Admin")&& !string.IsNullOrWhiteSpace(userMode))
                        {
                            updateBurnInFiles.ShowDialog();
                            if (updateBurnInFiles.BurnInFilesLoaded)
                            {
                                LookUpBurnInFiles();
                            }
                        }
                        else
                        {
                            if (updateBurnInFiles.uploadBurnInFiles())
                            {
                                MessageBox.Show(string.Format("BurnIn script {0}  uploaded successfully!", updateBurnInFiles.SelectedBurnInFile),
                                        "Upload BurnIn Script", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LookUpBurnInFiles();
                            }
                            else
                            {
                                MessageBox.Show(string.Format("Error uploading {0}  BurnIn script!", updateBurnInFiles.SelectedBurnInFile),
                                                 "Upload BurnIn Script", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }                                              
                        break;
                    case "Camera Test Log":
                        CameraTestLogDetails cameraTestLogDetails = new CameraTestLogDetails(cameraTestLogsPresenter,userMode, textBoxCameraSerialNum.Text.Trim('\0').Trim(),
                                                                                                textBoxImagerSerialNum.Text.Trim('\0').Trim());
                        cameraTestLogDetails.Show();
                        break;
                    case "Send Heartbeat":
                        if (((StateButtonTool)(mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Send Heartbeat"])).Checked)
                        {
                            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                            if(cidInterface!=null && cidInterface.IsConnected())
                            {
                                await cidInterface.SendHeartbeatAsync(cancellationTokenSource);
                            }                               
                        }
                        //Settings.Default["sendCameraHeartBeat"] = ((StateButtonTool)(mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Send Heartbeat"])).Checked;
                        break;
                    case "LED Calibration Results":
                        LEDCalibrationTestResultsForm lEDCalibrationTestResultsForm = new LEDCalibrationTestResultsForm();
                        lEDCalibrationTestResultsForm.Show();
                        break;
                    case "EnvironmentalStatus":
                        if (environmentalStatusForm == null || environmentalStatusForm.IsDisposed)
                        {
                            environmentalStatusForm = new EnvironmentalStatus(environmentalStatusPresenter, this);
                        }                       
                        environmentalStatusForm.Show();
                        break;
                    case "TestStationCalibration":
                        if (testStationCalibrationReminder == null)
                        {
                            testStationCalibrationReminder = new TestStationCalibrationReminder(userLoginPresenter,enggUserName);
                            testStationCalibrationReminder.ShowDialog();
                        }
                        else
                        {
                            // if the form was loaded and then closed by the operator
                            testStationCalibrationReminder.UserName = enggUserName;
                            testStationCalibrationReminder.ShowDialog();
                        }
                        break;

                        
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                Application.UseWaitCursor = false;
            }
        } 
        private void LookUpBurnInFiles()
        {
            try
            {
                if (cidInterface.IsConnected() && ConfigurationManager.AppSettings["EndClient"] == "Thermo")
                {
                    if (burnInAnalysis.lookupBurnInScriptFile())
                    {
                        controlPanelExplorerBar.Groups["controlPanelBurnInDataAnalysis"].Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);               
            }
        }
        private void CreateUserAccount()
        {
            try
            {
                CreateUserAccount createUserAccount = new CreateUserAccount();
                createUserAccount.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void SendOutlookMail()
        {
            try
            {
                SendMailForm sendMailForm = new SendMailForm(enggUserName);
                sendMailForm.Show(this);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void TestGraphPreferences()
        {
            try
            {
                testGraphPreferences = new TestGraphPreferences();
                DialogResult result = testGraphPreferences.ShowDialog();
                if(result== DialogResult.OK)
                    PlotBackColorWhite();               
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void FirmwareLogging()
        {
            if (logger != null)
            {
                logger.ShowDialog();
            }
            Logger.LogLocation logLocation;
            logLocation = logger.GetLoggerLocation();
            if (logLocation == Logger.LogLocation.Console)
            {
                mainFormDockManager.DockAreas[1].Closed = false;
            }
            else
            {
                mainFormDockManager.DockAreas[1].Closed = true;
            }
            logLevel = logger.GetLoggerLevel();
            fwLoggingEnabled = logger.fwLoggingEnabled();
            ChangeLoggingLevel(logger.GetLoggerLocation());
        }
        private void ChangeLoggingLevel(Logger.LogLocation location)
        {
            log4net.Repository.ILoggerRepository[] repositories = log4net.LogManager.GetAllRepositories();
            //Configure all loggers to be at the debug level.
            foreach (log4net.Repository.ILoggerRepository repository in repositories)
            {
                repository.Threshold = repository.LevelMap["DEBUG"];
                if (Logger.LogLocation.None == location)
                {
                    repository.Threshold = log4net.Core.Level.Off;
                }
                if ((Logger.LogLocation.File == location) || (Logger.LogLocation.Console == location))
                {
                    repository.Threshold = log4net.Core.Level.All;
                }
                log4net.Repository.Hierarchy.Hierarchy hier = (log4net.Repository.Hierarchy.Hierarchy)repository;
                log4net.Core.ILogger[] loggers = hier.GetCurrentLoggers();
                foreach (log4net.Core.ILogger logger in loggers)
                {
                    ((log4net.Repository.Hierarchy.Logger)logger).Level = logLevel;
                }
            }
            //Configure the root logger.
            log4net.Repository.Hierarchy.Hierarchy hierarchy = (log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository();
            log4net.Repository.Hierarchy.Logger rootLogger = hierarchy.Root;
            rootLogger.Level = logLevel; 
            if (cidInterface.IsConnected())
            {
                if (logLevel == log4net.Core.Level.Fatal)
                    cidInterface.SendLogLevel(0);
                else if (logLevel == log4net.Core.Level.Error)
                    cidInterface.SendLogLevel(1);
                else if (logLevel == log4net.Core.Level.Warn)
                    cidInterface.SendLogLevel(2);
                else if (logLevel == log4net.Core.Level.Info)
                    cidInterface.SendLogLevel(3);
                else if (logLevel == log4net.Core.Level.Debug)
                    cidInterface.SendLogLevel(4);
                else //if (logLevel == log4net.Core.Level.Camera)
                    cidInterface.SendLogLevel(5);
            }
        }
        private void changeLogLevel()
        {
            logLevel = log4net.Core.Level.Warn;
            log4net.Repository.ILoggerRepository[] repositories = log4net.LogManager.GetAllRepositories();
            //Configure all loggers to be at the debug level.
            foreach (log4net.Repository.ILoggerRepository repository in repositories)
            {
                repository.Threshold = repository.LevelMap["DEBUG"];              
                log4net.Repository.Hierarchy.Hierarchy hier = (log4net.Repository.Hierarchy.Hierarchy)repository;
                log4net.Core.ILogger[] loggers = hier.GetCurrentLoggers();
                foreach (log4net.Core.ILogger logger in loggers)
                {
                    ((log4net.Repository.Hierarchy.Logger)logger).Level = logLevel;
                }
            }
            //Configure the root logger.
            log4net.Repository.Hierarchy.Hierarchy hierarchy = (log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository();
            log4net.Repository.Hierarchy.Logger rootLogger = hierarchy.Root;
            rootLogger.Level = logLevel;
            if (cidInterface.IsConnected())
            {
                if (logLevel == log4net.Core.Level.Fatal)
                    cidInterface.SendLogLevel(0);
                else if (logLevel == log4net.Core.Level.Error)
                    cidInterface.SendLogLevel(1);
                else if (logLevel == log4net.Core.Level.Warn)
                    cidInterface.SendLogLevel(2);
                else if (logLevel == log4net.Core.Level.Info)
                    cidInterface.SendLogLevel(3);
                else if (logLevel == log4net.Core.Level.Debug)
                    cidInterface.SendLogLevel(4);
                else //if (logLevel == log4net.Core.Level.Camera)
                    cidInterface.SendLogLevel(5);
            }
        }
        private void FinalTestReport()
        {
            try
            {
                finalTestReportForm = this.MdiChildren.OfType<FinalTestReportForm>().SingleOrDefault();
                if (finalTestReportForm == null)
                {
                    finalTestReportForm = new FinalTestReportForm(testResultsPath, userMode);
                    finalTestReportForm.MdiParent = this;
                    finalTestReportForm.Show();
                }
                else if (finalTestReportForm.WindowState == FormWindowState.Minimized)
                    finalTestReportForm.WindowState = FormWindowState.Normal;
                else
                {
                    finalTestReportForm.Visible = true;
                    finalTestReportForm.Activate();
                    finalTestReportForm.BringToFront();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void TestResults()
        {
            try
            {
                TestResultsForm testResultsForm = new TestResultsForm(meanVariancePresenter,
                                                                       darkCurrentPresenter, defectsPresenter, injectionEfficiencyPresenter, noiseVsNDROPresenter,
                                                                       photoresponsePresenter, redBluePresenter, shutterDrivePresenter, overAllTestResultsPresenter);
                testResultsForm.Show(this);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void TestLimits()
        {
            try
            {
                CharacterizationTestLimits testLimits = new CharacterizationTestLimits(meanVarianceTestLimitsPresenter,
                                                                                  darkCurrentTestLimitsPresenter, defectsTestLimitsPresenter,
                                                                                  injectionEfficiencyTestLimitsPresenter, injectionPerformanceTestLimitsPresenter,
                                                                                  nDROReadDriftTestLimitsPresenter, noiseVsNDROSLimitsPresenter,
                                                                                  photoresponseLimitsPresenter, segmentInjectLimitsPresenter, 
                                                                                  ledCalibrationPresenter, tempHumidPresenter, userMode, loginStatus, enggUserName);
                testLimits.Show(this);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void LoadUserGuide()
        {
            try
            {
                String def = Application.StartupPath.ToString();
                if (System.IO.Directory.Exists(def + "\\Resources"))
                    def = def + "\\Resources";
                String docFile;
                docFile = def + "\\TestApp_Software_User_Guide_Specification.doc";
                if (System.IO.File.Exists(docFile))
                {
                    System.Diagnostics.Process.Start(docFile);
                }
                else
                {
                    MessageBox.Show("User Guide not found" + docFile.ToString(),
                        "Error loading User Guide",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error);
                    log.Error("User Guide not found error");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void OperatorInput()
        {
            try
            {
                if (operatorInputForm == null)
                {
                    operatorInputForm = new OperatorInputForm(cidInterface, cameraDetailsPresenter,true, cameraInformation);
                    operatorInputForm.ShowDialog();
                }
                else
                {
                    operatorInputForm.ShowDialog();					
				}
			}
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void BrowseFolderPath()
        {
            try
            {
                mainFormFolderBrowserDialog.SelectedPath = Properties.Settings.Default.FilesLocation;
                DialogResult result = mainFormFolderBrowserDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    ((TextBoxTool)mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["TextBoxToolFolderPath"]).Text = mainFormFolderBrowserDialog.SelectedPath;
                    Properties.Settings.Default["FilesLocation"] = mainFormFolderBrowserDialog.SelectedPath;
                    defaultFilePath = Properties.Settings.Default["FilesLocation"].ToString();
                    ExcelUtilities.FilePath = ((TextBoxTool)mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["TextBoxToolFolderPath"]).Text;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void TCPIPSettings()
        {
            try
            {
                tcpSettingsForm = new TCPSettingsForm(cidInterface, userMode, enggUserName);
                tcpSettingsForm.ShowDialog(this);
                if (cidInterface.IsConnected() || tcpSettingsForm.CameraConnectedFromTCPDLG)
                {
                    ultraStatusBar1.Panels["CameraStatus"].Text = "Camera Connected";
                    ultraStatusBar1.Panels["CameraStatus"].Appearance.BackColor = Color.LimeGreen;
					((ToolBase)(mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["CameraConnection"])).SharedProps.AppearancesSmall.Appearance.Image = 
                        new Bitmap(Assembly.GetEntryAssembly().GetManifestResourceStream("KronosCameraTestApp.Resources.Camera_ON.bmp"));
                    log.Info("Camera with Firmware ID" + firmwareId.ToString() + "connected.");
                    TECCheckBox.Checked = true;
                }
                else
                {
                    ultraStatusBar1.Panels["CameraStatus"].Text = "Camera Not Connected";
                    ultraStatusBar1.Panels["CameraStatus"].Appearance.BackColor = Color.Red;
					((ToolBase)(mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["CameraConnection"])).SharedProps.AppearancesSmall.Appearance.Image = 
                        new Bitmap(Assembly.GetEntryAssembly().GetManifestResourceStream("KronosCameraTestApp.Resources.Camera_Off.bmp"));
                    TECCheckBox.Checked = false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ClearHistory()
        {
            try
            {
                cidDataList.Clear();
                cidIntegratedDataList.Clear();
                ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.ResetFillAppearance();
                if (meanVarianceTestForm != null) { var mres = meanVarianceTestForm.ClearMVGraphs(); }
                if (meanVarianceTestUserControl != null) { var mures = meanVarianceTestUserControl.ClearMVGraphs(); }
                if (darkCurrentTestUserControl != null) { var lures = darkCurrentTestUserControl.ClearDarkCurrentGraphs(); }
                if (defectsUserControl != null) { var deres = defectsUserControl.ClearDefectsGraphs(); }
                if (darkCurrentTestForm != null) { var lres = darkCurrentTestForm.ClearDarkCurrentGraphs(); }
                if (defectsTestForm != null) { defectsTestForm.ClearDefectsGraphs(); }
                if (noiseVsNDROTestForm != null) { var nres = noiseVsNDROTestForm.ClearNoiseNDROGraphs(); }
                if (noiseVsNDROTestUserControl != null) { var nures = noiseVsNDROTestUserControl.ClearNoiseNDROGraphs(); }
                if (shutterDriveTestForm != null) { var sres = shutterDriveTestForm.ClearShutterDriveGraphs(); }
                if (shutterDriveUserControl != null) { var sures = shutterDriveUserControl.ClearShutterDriveGraphs(); }
                if (injectionEfficiencyTestForm != null) { var ires = injectionEfficiencyTestForm.ClearInjectionPerformanceGraphs(); }
                if (injectionPerformanceTestUserControl != null) { var iures = injectionPerformanceTestUserControl.ClearInjectionPerformanceGraphs(); }
                if (photoresponseTestForm != null) { var ires = photoresponseTestForm.ClearPhotoresponseGraphs(); }
                if (photoresponseTestUserControl != null) { var iures = photoresponseTestUserControl.ClearPhotoresponseGraphs(); }
                if (imageDisplayPanel != null) { var imres = imageDisplayPanel.ClearImageDisplayGraphs(); }
                if (redBlueTest != null) { var rebres = redBlueTest.ClearRedBlueGraphs(); }
                if (redBlueUserControl != null) { var redbures = redBlueUserControl.ClearRedBlueGraphs(); }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void OpenDocuments()
        {
            try
            {
                mainFormOpenFileDialog.Filter = "jpeg (*.jpg)|*.jpg|Bitmap (*.bmp)|*.bmp|Tiff (*.tiff)|*.tiff|XML Files (*.xml)|*.xml";
                mainFormOpenFileDialog.FilterIndex = 1;
                mainFormOpenFileDialog.InitialDirectory = Properties.Settings.Default["FilesLocation"].ToString();
                ImageDisplayForm imageDisplayForm = new ImageDisplayForm();
                DialogResult result = mainFormOpenFileDialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    if (mainFormOpenFileDialog.FileName.Contains(".jpeg") || mainFormOpenFileDialog.FileName.Contains(".bmp") || mainFormOpenFileDialog.FileName.Contains(".tiff"))
                    {
                        imageDisplayForm.webBrowser1.Visible = false;
                        imageDisplayForm.ultraPictureBox1.Visible = true;
                        imageDisplayForm.ultraPictureBox1.Image = new Bitmap(mainFormOpenFileDialog.FileName);
                    }
                    else
                    {
                        imageDisplayForm.webBrowser1.Visible = true;
                        imageDisplayForm.ultraPictureBox1.Visible = false;
                        using (var myStream = mainFormOpenFileDialog.OpenFile())
                        {
                            try
                            {
                                imageDisplayForm.webBrowser1.Navigate(mainFormOpenFileDialog.FileName);
                            }
                            catch (XmlException ex)
                            {
                                MessageBox.Show("The XML could not be read. " + ex);
                            }
                        }
                    }
                    imageDisplayForm.Text = mainFormOpenFileDialog.FileName;
                    imageDisplayForm.Show(this);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public void SaveImage(string ImageType)
        {
            try
            {
                if (this.ActiveMdiChild != null)
                {
                    Form activeChild = this.ActiveMdiChild;
                    string datetimestamp = DateTime.Now.ToString("MMddyyyy_hhmmss");
                    string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    Image img = null;
                    Image img1 = null;
                    Image img2 = null;                    
                    int outputImageWidth = 0;
                    int outputImageHeight = 0;
                    Bitmap outputImage = null;
                    mainFormSaveFileDialog.InitialDirectory = Properties.Settings.Default["FilesLocation"].ToString();
                    if (ImageType == "Bitmap")
                    {
                        mainFormSaveFileDialog.Title = "Save As Bitmap";
                        mainFormSaveFileDialog.DefaultExt = "*.";
                        mainFormSaveFileDialog.Filter = "Windows Bitmap|*.bmp";
                    }
                    else if (ImageType == "JPEG")
                    {
                        mainFormSaveFileDialog.Title = "Save As Jpeg";
                        mainFormSaveFileDialog.DefaultExt = "*.";
                        mainFormSaveFileDialog.Filter = "Jpeg|*.jpeg";
                    }
                    else if (ImageType == "PNG")
                    {
                        mainFormSaveFileDialog.Title = "Save As PNG";
                        mainFormSaveFileDialog.DefaultExt = "*.";
                        mainFormSaveFileDialog.Filter = "Png|*.png";
                    }
                    else
                    {
                        mainFormSaveFileDialog.Title = "Save As Tiff";
                        mainFormSaveFileDialog.DefaultExt = "*.";
                        mainFormSaveFileDialog.Filter = "Tiff|*.tiff";
                    }
                    if (activeChild.Name == "MeanVarianceTestForm")
                    {
                        img = meanVarianceTestForm.meanVarianceScatterGraph.ToImage();
                        img1 = meanVarianceTestForm.meanVarianceWaveformGraph.ToImage();
                        mainFormSaveFileDialog.FileName = "MeanVariance_" + datetimestamp;
                    }
                    else if (activeChild.Name == "ShutterDriveTestForm")
                    {
                        img = shutterDriveTestForm.shutterWaveformGraph.ToImage();
                        mainFormSaveFileDialog.FileName = "ShutterDrive_" + datetimestamp;
                    }
                    else if (activeChild.Name == "DarkCurrentTestForm")
                    {
                        img = darkCurrentTestForm.darkCurrentWaveformGraph.ToImage();
                        mainFormSaveFileDialog.FileName = "DarkCurrent_" + datetimestamp;
                    }
                    else if (activeChild.Name == "DefectsTestForm")
                    {
                        img = defectsTestForm.defectsIntensityGraph.ToImage();
                        mainFormSaveFileDialog.FileName = "DefectsTest_" + datetimestamp;
                    }
                    else if (activeChild.Name == "NoiseVsNDROTestForm")
                    {
                        img = noiseVsNDROTestForm.readNoiseScatterGraph.ToImage();                        
                        mainFormSaveFileDialog.FileName = "NoiseVsNDRO_" + datetimestamp;
                    }                   
                    else if (activeChild.Name == "InjectionEfficiencyTestForm")
                    {
                        img = injectionEfficiencyTestForm.chargeInjectionEfficiencyWaveformGraph.ToImage();
                        mainFormSaveFileDialog.FileName = "InjectionEfficiency_" + datetimestamp;
                    }
                    else if (activeChild.Name == "PhotoresponseTestForm")
                    {
                        img = photoresponseTestForm.photoResponseWaveformGraph.ToImage();
                        img1 = photoresponseTestForm.derivativeWaveformGraph.ToImage();
                        mainFormSaveFileDialog.FileName = "Photoresponse_" + datetimestamp;
                    }
                    else if (activeChild.Name == "RedBlueTestForm")
                    {
                        img = redBlueTest.redBlueIntensityGraph.ToImage();
                        img1 = redBlueTest.blueIntensityGraph.ToImage();
                        img2 = redBlueTest.differenceIntensityGraph.ToImage();
                        mainFormSaveFileDialog.FileName = "RedBlue_" + datetimestamp;
                        SaveRedBlueTestImage(img, img1, img2, ImageType, mainFormSaveFileDialog.FileName);
                        return;
                    }
                    else if (activeChild.Name == "ImageDisplayPanel")
                    {
                        img = imageDisplayPanel.intensityGraph1.ToImage();
                        mainFormSaveFileDialog.FileName = "ImageTest_" + datetimestamp;
                    }
                    else if (activeChild.Name == "DashBoardAutoForm")
                    {
                        if (dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileMeanVariance"].State == TileState.Large)
                        {
                            img = meanVarianceTestUserControl.meanVarianceScatterGraph.ToImage();
                            img1 = meanVarianceTestUserControl.meanVarianceWaveformGraph.ToImage();
                            mainFormSaveFileDialog.FileName = "MeanVariance_AutoTest_" + datetimestamp;
                        }
                        else if (dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileRedBlue"].State == TileState.Large)
                        {
                            img = redBlueUserControl.redBlueIntensityGraph.ToImage();
                            img1 = redBlueUserControl.blueIntensityGraph.ToImage();
                            img2 = redBlueUserControl.differenceIntensityGraph.ToImage();
                            mainFormSaveFileDialog.FileName = "RedBlue_AutoTest_" + datetimestamp;
                            SaveRedBlueTestImage(img, img1, img2, ImageType, mainFormSaveFileDialog.FileName);
                            return;
                        }
                        else if (dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileShutterDrive"].State == TileState.Large)
                        {
                            img = shutterDriveUserControl.shutterWaveformGraph.ToImage();
                            mainFormSaveFileDialog.FileName = "ShutterDrive_AutoTest_" + datetimestamp;
                        }
                        else if (dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileDarkCurrent"].State == TileState.Large)
                        {
                            img = darkCurrentTestUserControl.darkCurrentWaveformGraph.ToImage();
                            mainFormSaveFileDialog.FileName = "DarkCurrent_AutoTest_" + datetimestamp;
                        }
                        else if (dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileDefects"].State == TileState.Large)
                        {
                            img = defectsUserControl.defectsIntensityGraph.ToImage();
                            mainFormSaveFileDialog.FileName = "Defects_AutoTest_" + datetimestamp;
                        }
                        else if (dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileReadNoise"].State == TileState.Large)
                        {
                            img = noiseVsNDROTestUserControl.readNoiseScatterGraph.ToImage();
                            mainFormSaveFileDialog.FileName = "NoiseVsNDRO_AutoTest_" + datetimestamp;
                        }                       
                        else if (dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileInjectionPerformance"].State == TileState.Large)
                        {
                            img = injectionEfficiencyTestForm.chargeInjectionEfficiencyWaveformGraph.ToImage();
                            mainFormSaveFileDialog.FileName = "InjectionPeformance_AutoTest_" + datetimestamp;
                        }
                        else if (dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTilePhotoresponse"].State == TileState.Large)
                        {
                            img = photoresponseTestForm.photoResponseWaveformGraph.ToImage();
                            img1 = photoresponseTestForm.derivativeWaveformGraph.ToImage();
                            mainFormSaveFileDialog.FileName = "Photoresponse_AutoTest_" + datetimestamp;
                        }
                    }
                    else if (activeChild.Name == "DashBoardAutoPretestForm")
                    {
                        if (dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileMeanVariance"].State == TileState.Large)
                        {
                            img = meanVarianceTestUserControl.meanVarianceScatterGraph.ToImage();
                            img1 = meanVarianceTestUserControl.meanVarianceWaveformGraph.ToImage();
                            mainFormSaveFileDialog.FileName = "MeanVariance_AutoPreTest_" + datetimestamp;
                        }
                        else if (dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileRedBluePreTest"].State == TileState.Large)
                        {
                            img = redBlueUserControl.redBlueIntensityGraph.ToImage();
                            img1 = redBlueUserControl.blueIntensityGraph.ToImage();
                            img2 = redBlueUserControl.differenceIntensityGraph.ToImage();
                            mainFormSaveFileDialog.FileName = "RedBlue_AutoPreTest_" + datetimestamp;
                            SaveRedBlueTestImage(img, img1, img2, ImageType, mainFormSaveFileDialog.FileName);
                            return;
                        }
                        else if (dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileShutterDrive"].State == TileState.Large)
                        {
                            img = shutterDriveUserControl.shutterWaveformGraph.ToImage();
                            mainFormSaveFileDialog.FileName = "ShutterDrive_AutoPreTest_" + datetimestamp;
                        }
                        else if (dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileDarkCurrentPreTest"].State == TileState.Large)
                        {
                            img = darkCurrentTestUserControl.darkCurrentWaveformGraph.ToImage();
                            mainFormSaveFileDialog.FileName = "DarkCurrent_AutoPreTest_" + datetimestamp;
                        }
                        else if (dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileDefectsPreTest"].State == TileState.Large)
                        {
                            img = defectsUserControl.defectsIntensityGraph.ToImage();
                            mainFormSaveFileDialog.FileName = "Defects_AutoPreTest_" + datetimestamp;
                        }
                        else if (dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileReadNoise"].State == TileState.Large)
                        {
                            img = noiseVsNDROTestUserControl.readNoiseScatterGraph.ToImage();
                            mainFormSaveFileDialog.FileName = "NoiseVsNDRO_AutoPreTest_" + datetimestamp;
                        }
                        else if (dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileInjectionPerformance"].State == TileState.Large)
                        {
                            img = injectionEfficiencyTestForm.chargeInjectionEfficiencyWaveformGraph.ToImage();
                            mainFormSaveFileDialog.FileName = "InjectionPeformance_AutoPreTest_" + datetimestamp;
                        }
                        else if (dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTilePhotoresponse"].State == TileState.Large)
                        {
                            img = photoresponseTestForm.photoResponseWaveformGraph.ToImage();                            
                            img1 = photoresponseTestForm.derivativeWaveformGraph.ToImage();
                            mainFormSaveFileDialog.FileName = "Photoresponse_AutoPreTest_" + datetimestamp;
                        }
                    }               
                    if(img1!=null)
                    {
                        outputImageWidth = img.Width > img1.Width ? img.Width : img1.Width;
                        outputImageHeight = img.Height + img1.Height + 1;
                    }
                    else
                    {
                        outputImageWidth = img.Width;
                        outputImageHeight = img.Height + 1;
                    }
                    
                    outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                    outputImage.SetResolution(1028.0f, 1028.0f);
                    using (Graphics graphics = Graphics.FromImage(outputImage))
                    {
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                        graphics.DrawImage(img, new Rectangle(new Point(), img.Size),
                               new Rectangle(new Point(), img.Size), GraphicsUnit.Pixel);
                        if (img1 != null)
                        {
                            graphics.DrawImage(img1, new Rectangle(new Point(0, img.Height + 1), img1.Size),
                                new Rectangle(new Point(), img1.Size), GraphicsUnit.Pixel);
                        }
                    }
                    if (mainFormSaveFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK && mainFormSaveFileDialog.FileName.Length > 0)
                    {
                        if (ImageType == "Bitmap")
                            outputImage.Save(mainFormSaveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        else if (ImageType == "JPEG")
                            outputImage.Save(mainFormSaveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        else if (ImageType == "PNG")
                            outputImage.Save(mainFormSaveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        else
                            outputImage.Save(mainFormSaveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Tiff);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public void SaveRedBlueTestImage(Image img, Image img1, Image img2, string ImageType,string ImageFileName)
        {
            try
            {
                int outputImageWidth = 0;
                int outputImageHeight = 0;
                Bitmap outputImage = null;
                int height = 0, width = 0;
                List<Image> imageList = new List<Image>();
                imageList.Add(img);
                imageList.Add(img1);
                imageList.Add(img2);
                mainFormSaveFileDialog.FileName = ImageFileName;
                List<int> imageHeights = new List<int>();
                for (int i = 0; i < imageList.Count; i++)
                {
                    height = Math.Max(height, imageList[i].Height);
                }
                outputImageHeight = height;
                outputImageWidth = img.Width + img1.Width + img2.Width;
                outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(outputImage))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                    for (int i = 0; i < imageList.Count; ++i)
                    {
                        g.DrawImage(imageList[i], new Point(width, 0));
                        width += imageList[i].Width;
                    }
                }
                if (mainFormSaveFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK && mainFormSaveFileDialog.FileName.Length > 0)
                {
                    if (ImageType == "Bitmap")
                        outputImage.Save(mainFormSaveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    else if (ImageType == "JPEG")
                        outputImage.Save(mainFormSaveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    else if (ImageType == "PNG")
                        outputImage.Save(mainFormSaveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    else
                        outputImage.Save(mainFormSaveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Tiff);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void CaptureScreen()
        {
            try
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                memoryImage = new Bitmap(this.Width, this.Height);
                Graphics memoryGraphics = Graphics.FromImage(memoryImage);
                memoryGraphics.CopyFromScreen(this.Location.X, this.Location.Y,0,0, bounds.Size);
                printDocument1.DefaultPageSettings.Landscape = true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void PageSetup()
        {
            try
            {
                mainFormPageSetupDialog.PageSettings = new System.Drawing.Printing.PageSettings();
                mainFormPageSetupDialog.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
                mainFormPageSetupDialog.ShowNetwork = false;
                DialogResult result = mainFormPageSetupDialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    printDocument1.DefaultPageSettings = mainFormPageSetupDialog.PageSettings;
                    object[] results = new object[]{ 
				mainFormPageSetupDialog.PageSettings.Margins, 
				mainFormPageSetupDialog.PageSettings.PaperSize, 
				mainFormPageSetupDialog.PageSettings.Landscape, 
				mainFormPageSetupDialog.PrinterSettings.PrinterName, 
				mainFormPageSetupDialog.PrinterSettings.PrintRange};
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void PrintPreview()
        {
            try
            {
                mainFormPrintPreviewDialog.Document = mainFormPrintDocument;
                CaptureScreen();
                this.mainFormPrintPreviewDialog.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void PrintScreen()
        {
            try
            {
               CaptureScreen();
               DialogResult printDialog= mainFormprintDialog.ShowDialog(this);
               if (printDialog == DialogResult.OK)
               {
                   mainFormPrintDocument.DefaultPageSettings.Landscape = true;
                   this.mainFormPrintDocument.Print();
               }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraPrintDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                this.mainFormPrintDocument.Header.TextLeft = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
				this.mainFormPrintDocument.Header.TextRight = Environment.UserName;
				this.mainFormPrintDocument.DefaultPageSettings.Landscape = true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraPrintDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                Image image = null;
                Form activeChild = this.ActiveMdiChild;
                if (activeChild.Name == "FinalTestReportForm" || (activeChild.Name == "DashBoardAutoForm" && dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileFinalTestReport"].State == TileState.Large))
                {                  
                    Rectangle rect = new Rectangle(0, 0, finalTestReportUserControl.Width, finalTestReportUserControl.Height);
                    finalReportBitmap = new Bitmap(finalTestReportUserControl.Width, finalTestReportUserControl.Height);
                    finalTestReportUserControl.DrawToBitmap(finalReportBitmap, rect);
                    image = (System.Drawing.Image)this.finalReportBitmap;
                }
                else
                {
                    // Get the image to print
                    image = (System.Drawing.Image)this.memoryImage;
                }
                // Get the Image Size
                Size imageSize = image.Size;
                // Get the starting X and Y based on the last portion
                // of the image that was printed.
                int startX = this.lastPrintX;
                int startY = this.lastPrintY;
                // Determine how much of the image remains to be printed
                // from the starting point.
                int remainingImageWidth = imageSize.Width - startX;
                int remainingImageHeight = imageSize.Height - startY;
                // These variables will keep track of whether the height or
                // width were clipped. This will help us determine if more
                // pages need to be printed.
                bool wasWidthClipped = false;
                bool wasHeightClipped = false;
                // Get the Size of the printable area of the page in Pixels.
                // MarginBounds returns the rect in hundredths of an inch.
                float scaleX = e.Graphics.DpiX / 100f;
                float scaleY = e.Graphics.DpiY / 100f;
                Rectangle printableRect = new Rectangle(
                    (int)(e.MarginBounds.X * scaleX),
                    (int)(e.MarginBounds.Y * scaleY),
                    (int)(e.MarginBounds.Width * scaleX),
                    (int)(e.MarginBounds.Height * scaleY)
                );
                // If the remaining image width is greater than
                // the width of the printable area of the page, clip it.
                if (remainingImageWidth > printableRect.Width)
                {
                    remainingImageWidth = printableRect.Width;
                    wasWidthClipped = true;
                }
                // If the remaining image height is greater than
                // the height of the printable area of the page, clip it.
                if (remainingImageHeight > printableRect.Height)
                {
                    remainingImageHeight = printableRect.Height;
                    wasHeightClipped = true;
                }
                // This rect will define a rect within the image that
                // is to be printed on the current page.
                Rectangle imagePrintRect = new Rectangle(startX, startY, remainingImageWidth, remainingImageHeight);
                e.Graphics.DrawImage(image, e.MarginBounds);
                // Set up the variables for the next page
                if (wasWidthClipped)
                {
                    // If the Width was clipped, it means we need to
                    // increment lastPrintX.
                    this.lastPrintX += (remainingImageWidth + 1);
                    // Set HasMorePages to true so the UltraPrintDocument
                    // knows there is more to print.
                    e.HasMorePages = true;
                }
                else if (wasHeightClipped)
                {
                    // If the Width was not clipped, but the Height was,
                    // it means we need to move to the next line.
                    this.lastPrintX = 0;
                    this.lastPrintY += (remainingImageHeight + 1);
                    // Set HasMorePages to true so the UltraPrintDocument
                    // knows there is more to print.
                    e.HasMorePages = true;
                }
                image.Dispose();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraRadioButtonAdmin_Click(object sender, EventArgs e)
        {
            try
            {             
                if(userModeChangedCheck(ultraRadioButtonAdmin.Text)== null || TestAppHelper.UserModeChanged)
                {
                    //if (!loginStatus)
                    EnggAdminLogin(ultraRadioButtonAdmin.Text);
                    if (loginStatus)
                    {
                        userMode = ultraRadioButtonAdmin.Text;                        
                        ultraRadioButtonManufacutring.Checked = false;
                        //ultraRadioButtonManufacutring.Enabled = false;
                        ultraRadioButtonEngineering.Checked = false;
                        //ultraRadioButtonEngineering.Enabled = false;
                        //ultraRadioButtonAdmin.Enabled = false;
                        ultraRadioButtonAdmin.Checked = true;

                        ((ToolBase)(mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Create User Account"])).SharedProps.Enabled = true;
                        ((ToolBase)(mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Instrument Defaults"])).SharedProps.Enabled = true;

                        if (this.MdiChildren.OfType<Form>().ToList().Count > 0)
                        {
                            this.MdiChildren.OfType<Form>().ToList().ForEach(x => x.Close());
                        }
                    }
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
        private void ultraRadioButtonEngineering_Click(object sender, EventArgs e)
        {
            try
            {                
                if (userModeChangedCheck(ultraRadioButtonEngineering.Text) == null || TestAppHelper.UserModeChanged)
                {
                    //if (!loginStatus)
                    EnggAdminLogin(ultraRadioButtonEngineering.Text);
                    if (loginStatus)
                    {
                        userMode = ultraRadioButtonEngineering.Text;
                        //TestAppHelper.UserModeChanged = true;
                        ultraRadioButtonManufacutring.Checked = false;
                        //ultraRadioButtonManufacutring.Enabled = false;
                        ultraRadioButtonAdmin.Checked = false;
                        //ultraRadioButtonAdmin.Enabled = false;
                        // ultraRadioButtonEngineering.Enabled = false;
                        ultraRadioButtonEngineering.Checked = true;
                        ((ToolBase)(mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Instrument Defaults"])).SharedProps.Enabled = true;
                        if (this.MdiChildren.OfType<Form>().ToList().Count > 0)
                        {
                            this.MdiChildren.OfType<Form>().ToList().ForEach(x => x.Close());
                        }
                    }
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
        private void ultraRadioButtonManufacutring_Click(object sender, EventArgs e)
        {
            try
            {
                if (userModeChangedCheck(ultraRadioButtonManufacutring.Text) == null || TestAppHelper.UserModeChanged)
                {                    
                    logger.IsManufacturingUserMode = true;
                    userMode = ultraRadioButtonManufacutring.Text;
                    loginStatus = false;
                    enggUserName = string.Empty;
                    ultraRadioButtonManufacutring.Checked = true;
                    //ultraRadioButtonManufacutring.Enabled = false;
                    ultraRadioButtonEngineering.Checked = false;
                    //ultraRadioButtonEngineering.Enabled = false;
                    ultraRadioButtonAdmin.Checked = false;
                    //ultraRadioButtonAdmin.Enabled = false;
                    //TestAppHelper.UserModeChanged = true;
                    controlPanelExplorerBar.Groups["controlPanelTempHumidity"].Visible = true;
                    controlPanelExplorerBar.Groups["controlPanelPreTest"].Visible = true;
                    controlPanelExplorerBar.Groups["controlPanelAutoTest"].Visible = true;
                    controlPanelExplorerBar.Groups["controlPanelManualTests"].Visible = true;
                    ((ToolBase)(mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Instrument Defaults"])).SharedProps.Enabled = true;
                    if (this.MdiChildren.OfType<Form>().ToList().Count > 0)
                    {
                        this.MdiChildren.OfType<Form>().ToList().ForEach(x => x.Close());
                    }
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
        private void EnggAdminLogin(string usrMode)
        {
            try
            {              
                    loginStatus = false;
                    loginForm = new UserLoginForm(userLoginPresenter, usrMode);
                    loginForm.ShowDialog(this);
                    loginStatus = loginForm.LoginStatus;
                if (loginForm.LoginFormCancelled || !loginForm.LoginStatus)
                {
                    ultraRadioButtonManufacutring.Enabled = true;
                    ultraRadioButtonEngineering.Enabled = true;
                    ultraRadioButtonAdmin.Enabled = true;
                    TestAppHelper.UserModeChanged = false;
                }
                else if (!loginForm.LoginFormCancelled || loginForm.LoginStatus)
                {
                    controlPanelExplorerBar.Groups["controlPanelTempHumidity"].Visible = true;
                    controlPanelExplorerBar.Groups["controlPanelAutoTest"].Visible = true;
                    controlPanelExplorerBar.Groups["controlPanelManualTests"].Visible = true;
                    controlPanelExplorerBar.Groups["controlPanelPreTest"].Visible = true;
                    ultraRadioButtonEngineering.Checked = true;
                    //TestAppHelper.UserModeChanged = true;
                    //ultraRadioButtonManufacutring.Enabled = false;
                    //ultraRadioButtonEngineering.Enabled = false;
                    //ultraRadioButtonAdmin.Enabled = false;
                    enggUserName = meanVariancePresenter.EnggUserName = meanVarianceTestLimitsPresenter.EnggUserName =
                    darkCurrentPresenter.EnggUserName = darkCurrentTestLimitsPresenter.EnggUserName =
                    defectsPresenter.EnggUserName = defectsTestLimitsPresenter.EnggUserName =
                    defectsTestLimitsPresenter.EnggUserName = imageDisplayPanelPresenter.EnggUserName =
                    injectionEfficiencyTestLimitsPresenter.EnggUserName = injectionEfficiencyPresenter.EnggUserName =
                    injectionPerformanceTestLimitsPresenter.EnggUserName = nDROReadDriftTestLimitsPresenter.EnggUserName =
                    noiseVsNDROPresenter.EnggUserName = noiseVsNDROSLimitsPresenter.EnggUserName =
                    overAllTestResultsPresenter.EnggUserName = photoresponsePresenter.EnggUserName =
                    photoresponseLimitsPresenter.EnggUserName = redBluePresenter.EnggUserName = shutterDrivePresenter.EnggUserName =
                    TestAppHelper.UserLoginName = loginForm.EnggLoggedUserName;
                    //((ToolBase)(mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Firmware Log"])).SharedProps.Enabled = true;
                    logger.IsManufacturingUserMode = false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }      
        private async void buttonRunAuto_Click(object sender, EventArgs e)
        {
            try
            {
               
                ResetProgressBar();
                ledPretestResult.Value = false;
                ledPretestResult.OffColor = Color.DarkGreen;                    
                if(!userMode.Equals("Admin") && TestAppHelper.BurnInFilesExistOnCamera && !dryRunTest.Checked)
                {
                    showAutoTestMessageBox("Camera holds Burn-In files. Please make sure to run Burn-In Analysis or remove the " +
                                            "related files from camera before proceeding with the final test.");
                    return;
                }
                 if (AutoPreTestStatus)
                {
                    showAutoTestMessageBox("Pretest is under process. Please wait until this test is completed");
                    return;
                }
                if (meanVarianceTestForm != null && meanVarianceTestForm.MeanVarianceTestStatus)
                {
                    showAutoTestMessageBox("Mean Variance Test is under process. Please wait until this test is completed");
                    return;
                }
                else if (darkCurrentTestForm != null && darkCurrentTestForm.DarkCurrentTestStatus)
                {
                    showAutoTestMessageBox("Dark Current Test is under process. Please wait until this test is completed");
                    return;
                }
                else if (defectsTestForm != null && defectsTestForm.DefectsTestStatus)
                {
                    showAutoTestMessageBox("Defects Test is under process. Please wait until this test is completed");
                    return;
                }
                else if (photoresponseTestForm != null && photoresponseTestForm.PhotoresponseTestStatus)
                {
                    showAutoTestMessageBox("Photoresponse Test is under process. Please wait until this test is completed");
                    return;
                }
                else if (injectionEfficiencyTestForm != null && injectionEfficiencyTestForm.InjectionPerformanceTestStatus)
                {
                    showAutoTestMessageBox("Injection Efficiency Test is under process. Please wait until this test is completed");
                    return;
                }
                else if (shutterDriveTestForm != null && shutterDriveTestForm.ShutterDriveTestStatus)
                {
                    showAutoTestMessageBox("Shutter Drive Test is under process. Please wait until this test is completed");
                    return;
                }
                else if (redBlueTest != null && redBlueTest.RedBlueTestStatus)
                {
                    showAutoTestMessageBox("Red & Blue Test is under process. Please wait until this test is completed");
                    return;
                }
                else
                {
                    if (cidInterface.IsConnected())                   
                    {
                        operatorInputForm = new OperatorInputForm(cidInterface, cameraDetailsPresenter,false,cameraInformation);
                        DialogResult result = operatorInputForm.ShowDialog(this);
                        if (result == DialogResult.OK)
                        {                           
                            cameraDetailsModel = new CameraDetailsModel();
                            cameraDetailsModel = operatorInputForm.UpdateCameraDetailsModelWithView();
                            CameraSerialNumber = operatorInputForm.CameraSerialNumber;
                            ImagerSerialNumber = operatorInputForm.ImagerSerialNumber;
                            autoTestForm = new AutoTestForm(loginStatus, userMode,enggUserName);
                            autoTestForm.ShowDialog(this);
                            if (autoTestForm.GoAuto)
                            {
                                AutoPreTestStatus = false;
                                ledAutoTestResult.Value = false;
                                ledAutoTestResult.OffColor = Color.DarkGreen;
                                AutoTestStatus = true;
                                buttonAbort.Enabled = true;
                                buttonRunAuto.Enabled = false;
                                log.Info("Begin Auto Test");
                                AutoCancellationTokenSource = new CancellationTokenSource();
                                autoTestFailReason.Clear();
								cidInterface.GetCameraInformation();
								updateSerialNumbers(cameraInformation);
                                strTestfailed = string.Empty;
                               

                                await RunAutoTests();
                                //environmentalStatusForm.Close();
                                TestAppHelper.UserModeChanged = false;
                               
                                ultraStatusBar1.Panels["EnvStatus"].Appearance.BackColor = Color.Transparent;
                                log.Info("End Auto Test");
                                AutoTestStatus = false;
                                camTempAlertResult = DialogResult.None;
                                camTempAlertDisplayed = false;
                                firmwareErrorsWarnings.Clear();
                                if (AutoCancellationTokenSource.IsCancellationRequested)
                                {
                                    ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                                    //environmentalStatusPresenter.RunStatus = false;
                                    //ultraStatusBar1.Panels["EnvStatus"].Visible = false;
                                    //environmentalStatusPresenter.DisconnectCOMPorts();
                                    ultraStatusBar1.Panels["EnvStatus"].Appearance.BackColor = Color.Transparent;
                                }
                                if (TECCheckBox.Checked)
                                {
                                    TECCheckBox.Checked = false;
                                    cidInterface.DisableThermoElectricCooler();
                                }                               
                                camTempAlertResultFor20 = DialogResult.None;
                            }                            
                            
                        }
                    }
                    else
                    {
                        log.Error("Firmware not running or camera not connected to host.");
                        MessageBox.Show("Firmware not running or camera not connected to host",
                            "Firmware not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                autoTestFailReason.Clear();
               // environmentalStatusPresenter.RunStatus = false;
                //ultraStatusBar1.Panels["EnvStatus"].Visible = false;
                //environmentalStatusPresenter.DisconnectCOMPorts();
                ultraStatusBar1.Panels["EnvStatus"].Appearance.BackColor = Color.Transparent;
            }
        }
        private void buttonAbort_Click(object sender, EventArgs e)
        {
            try
            {
                if (AutoCancellationTokenSource != null)
                {
                    DialogResult result = MessageBox.Show("Are you sure to Abort Auto Test?",
                   "Abort Auto Test",
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        AbortAutoTest(true);
                        AutoTestStatus = false;
                        buttonRunAuto.Enabled = true;
                        camTempAlertResult = DialogResult.None;
                        camTempAlertDisplayed = false;
                        firmwareErrorsWarnings.Clear();
                        //environmentalStatusPresenter.RunStatus = false;
                        //ultraStatusBar1.Panels["EnvStatus"].Visible = false;
                        //environmentalStatusPresenter.DisconnectCOMPorts();
                        ultraStatusBar1.Panels["EnvStatus"].Appearance.BackColor = Color.Transparent;
                    }
                    else
                        return;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void AbortAutoTestFirmwareError(string errorData)
        {
            try
            {
                MessageBox.Show(errorData,"Abort Auto Test", MessageBoxButtons.OK,MessageBoxIcon.Error);
                AbortAutoTest(true);
                AutoTestStatus = false;
                camTempAlertResult = DialogResult.None;
                camTempAlertDisplayed = false;
                firmwareErrorsWarnings.Clear();
                BeginInvoke(new System.Action(() => buttonRunAuto.Enabled = true));
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public void AbortAutoTest(bool abortFromMainScreen, string testType="")
        {
            try
            {
                if ((abortFromMainScreen) || (autoTestForm !=null && autoTestForm.AbortAutoTest && !abortFromMainScreen))
                {
                    ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                    ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.ResetFillAppearance();
                    AutoCancellationTokenSource.Cancel();
                    log.Info("Auto Test Aborted");
                    if (cidInterface != null)
                        cidInterface.AbortExposure();
                    cidDataList.Clear();
                    cidIntegratedDataList.Clear();                   
                    buttonRUNPretest.Enabled = true;
                    ClearHistory();
                }
                else if(autoTestForm != null && !autoTestForm.AbortAutoTest && !abortFromMainScreen && !autoTestFailReason.ToString().Contains("Red, Blue & UV"))
                {
                    autoTestFailReason.Append(" ' " + "Red, Blue & UV" + " ' , ");  
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public void RedBlueUserControlPassNormalTile()
        {
            try
            {
                if (dashBoardAutoPretestForm != null && dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles.Count > 0)
                    dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileRedBluePreTest"].State = TileState.Normal;
                if (dashBoardAutoForm != null && dashBoardAutoForm.ultraTilePanel1.Tiles.Count > 0)
                    dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileRedBlue"].State = TileState.Normal;
                if(!AutoTestStatus && (!AutoPreTestStatus && !prestTestResultsSaved))
                {
                    if(autoPretestForm!=null)
                    {
                        SavePreTestResults(autoPretestForm.SelectedPreTests, true, true);
                        updateCameraTestMetrics();
                    }
                   
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ExitApplication()
        {
            try
            {               
                log.Info("End Test App");
                DisconnectEnvComPorts();
                //Settings.Default["sendCameraHeartBeat"] = ((StateButtonTool)(mainFormToolBarManager.Toolbars.ToolbarsManager.Tools["Send Heartbeat"])).Checked;
                Settings.Default.dryRunTest = dryRunTest.Checked;
                Settings.Default.TECEnabled = TECCheckBox.Checked;
                Settings.Default.Save();
                if (streamWriterStatusMessageFile != null)
                {                    
                    streamWriterStatusMessageFile.Close();
                }
                Properties.Settings.Default.DefaultFilePath = defaultFilePath;
                if (this.MdiChildren.OfType<Form>().ToList().Count > 0)
                    this.MdiChildren.OfType<Form>().ToList().ForEach(x => x.Close());
                Application.Exit();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                Application.Exit();
            }
        }

        private void LogManulTestFirmwareOutput()
        {
            try
            {
                //if (logTempHumidity.Checked)
                {
                    ParametersModel parametersModel = TestAppHelper.GetParametersModel();
                    string logFileServerPath = string.Empty;
                    logFileServerPath = System.IO.Path.Combine(parametersModel.TestResultsServerPath.ToString(), "SCM5821AX1FirmwareManualTestLogs");
                    if (!System.IO.Directory.Exists(logFileServerPath))
                    {
                        System.IO.Directory.CreateDirectory(logFileServerPath);
                    }
                    string logFilelocalPath = string.Empty;
                    logFilelocalPath = System.IO.Path.Combine(defaultFilePath, "SCM5821AX1FirmwareManualTestLogs");
                    if (!System.IO.Directory.Exists(logFilelocalPath))
                    {
                        System.IO.Directory.CreateDirectory(logFilelocalPath);
                    }                    
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void LogTempHumToExcel()
        {
            try
            {
                if (logTempHumidity.Checked)
                {
                    ParametersModel parametersModel = TestAppHelper.GetParametersModel();
                    string logFileServerPath = string.Empty;
                    logFileServerPath = System.IO.Path.Combine(parametersModel.TestResultsServerPath.ToString(), "SCM5821AX1TempLogs");
                    if (!System.IO.Directory.Exists(logFileServerPath))
                    {
                        System.IO.Directory.CreateDirectory(logFileServerPath);
                    }
                    string logFilelocalPath = string.Empty;
                    logFilelocalPath = System.IO.Path.Combine(defaultFilePath, "SCM5821AX1TempLogs");
                    if (!System.IO.Directory.Exists(logFilelocalPath))
                    {
                        System.IO.Directory.CreateDirectory(logFilelocalPath);
                    }
                    ExcelUtilities.LogCameraTempHumidity(logFilelocalPath, logFileServerPath, camTemp, camHum, tempLogDateTime);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }

        public void DisconnectEnvComPorts()
        {
            envStatusRunStatus = false;
            ultraStatusBar1.Panels["EnvStatus"].Visible = false;
            ultraStatusBar1.Panels["ChillerWaterLevelWarn"].Visible = false;
            if(environmentalStatusForm!=null)
            {
                environmentalStatusForm.btnConnect.Text = "Connect";
            }            
           
            if (ssCoolingSerialPort.IsOpen)
            {
                ssCoolingSerialPort.Close();
            }
            if (powerSupplySerialPort.IsOpen)
            {
                powerSupplySerialPort.Close();
            }
            if (flowMeterSerialPort.IsOpen)
            {
                flowMeterSerialPort.Close();
            }
            
        }
        private void KronosTestAppMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if(!tempfilelogged)
                {
                   // LogTempHumToExcel();                    
                    tempfilelogged = true;
                }
                Cursor.Current = Cursors.WaitCursor;              
                if (cidInterface != null && cameraConnected)
                {
                    if (cidInterface.IsConnected())
                    {
                        cidInterface.Disconnect();
                    }                 
                }                
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                Cursor.Current = Cursors.Default;
            }
        }
        private void KronosTestAppMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                ExitApplication();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void buttonAbort_MouseHover(object sender, EventArgs e)
        {
        }
        private void buttonAbort_MouseEnter(object sender, EventArgs e)
        {
            buttonAbort.ForeColor = Color.Red;
        }
        private void buttonAbort_MouseLeave(object sender, EventArgs e)
        {
            buttonAbort.ForeColor = Color.Black;
        }
        private void buttonRunAuto_MouseEnter(object sender, EventArgs e)
        {
            buttonRunAuto.ForeColor = Color.LimeGreen;
        }
        private void buttonRunAuto_MouseLeave(object sender, EventArgs e)
        {
            buttonRunAuto.ForeColor = Color.Black;
        }
        private void DisplayTemperatureGraph()
        {
            temperatureGraphForm = new TemperatureGraphForm();
            temperatureGraphForm.Show(this);
        }
        private void ultraPictureBoxTemperatureGraph_Click(object sender, EventArgs e)
        {
            DisplayTemperatureGraph();
        }
        private void ultraPictureBoxHumidityGraph_Click(object sender, EventArgs e)
        {
            DisplayTemperatureGraph();
        }
        private async void buttonRunPretest_Click(object sender, EventArgs e)
        {
            try
            {               
                ResetProgressBar();
                ledAutoTestResult.Value = false;
                ledAutoTestResult.OffColor = Color.DarkGreen;
                if (AutoTestStatus)
                {
                    showAutoTestMessageBox("Auto Test is under process. Please wait until this test is completed");
                    return;
                }
                if (meanVarianceTestForm != null && meanVarianceTestForm.MeanVarianceTestStatus)
                {
                    showAutoTestMessageBox("Mean Variance Test is under process. Please wait until this test is completed");
                    return;
                }
                else if (darkCurrentTestForm != null && darkCurrentTestForm.DarkCurrentTestStatus)
                {
                    showAutoTestMessageBox("Dark Current Test is under process. Please wait until this test is completed");
                    return;
                }
                else if (defectsTestForm != null && defectsTestForm.DefectsTestStatus)
                {
                    showAutoTestMessageBox("Defects Test is under process. Please wait until this test is completed");
                    return;
                }
                else if (photoresponseTestForm != null && photoresponseTestForm.PhotoresponseTestStatus)
                {
                    showAutoTestMessageBox("Photoresponse Test is under process. Please wait until this test is completed");
                    return;
                }
                else if (injectionEfficiencyTestForm != null && injectionEfficiencyTestForm.InjectionPerformanceTestStatus)
                {
                    showAutoTestMessageBox("Injection Efficiency Test is under process. Please wait until this test is completed");
                    return;
                }
                else if (shutterDriveTestForm != null && shutterDriveTestForm.ShutterDriveTestStatus)
                {
                    showAutoTestMessageBox("Shutter Drive Test is under process. Please wait until this test is completed");
                    return;
                }
                else
                {                   
                    if (cidInterface.IsConnected())
                    {
                        operatorInputForm = new OperatorInputForm(cidInterface, cameraDetailsPresenter, false, cameraInformation);
                        DialogResult result = operatorInputForm.ShowDialog(this);
                        if (result == DialogResult.OK)
                        {
                            cameraDetailsModel = new CameraDetailsModel();
                            cameraDetailsModel = operatorInputForm.UpdateCameraDetailsModelWithView();
                            CameraSerialNumber = operatorInputForm.CameraSerialNumber;
                            ImagerSerialNumber = operatorInputForm.ImagerSerialNumber;
                            autoPretestForm = new AutoPretestForm();
                            autoPretestForm.ShowDialog(this);
                            if (autoPretestForm.GoAutoPretest)
                            {
                                ledPretestResult.Value = false;
                                ledPretestResult.OffColor = Color.DarkGreen;
                                AutoPreTestStatus = true;
                                buttonAbortPretest.Enabled = true;
                                buttonRUNPretest.Enabled = false;
                                log.Info("Begin Auto PreTest");
                                AutoCancellationTokenSource = new CancellationTokenSource();
                                autoTestFailReason.Clear();
                                strTestfailed = string.Empty;
                                await RunAutoPreTests();
                                log.Info("End Auto PreTest");                               
                                AutoPreTestStatus = false;
                                //defectsPresenter.AutoPretest = false;
                                firmwareErrorsWarnings.Clear();
                                TestAppHelper.UserModeChanged = false;
                                if (TECCheckBox.Checked)
                                {
                                    TECCheckBox.Checked = false;
                                    cidInterface.DisableThermoElectricCooler();
                                }
                                camTempAlertResultFor20 = DialogResult.None;
                                if (TECCheckBox.Checked)
                                {                                    
                                    TECCheckBox.Checked = false;                                   
                                    cidInterface.DisableThermoElectricCooler();
                                }                              
                            }
                           
                        }                       
                    }
                    else
                    {
                        log.Error("Firmware not running or camera not connected to host.");
                        MessageBox.Show(
                            "Firmware not running or camera not connected to host",
                            "Firmware not found",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task RunAutoPreTests()
        {
            try
            {
                Application.UseWaitCursor = true;
                AutoPreTestStatus = true;
                AutoPreTestStatusResult = false;
                prestTestResultsSaved = false;
                ledPretestResult.Value = false;
                ResetPretestStatusBarPanel();
                dashBoardAutoPretestForm = this.MdiChildren.OfType<DashBoardAutoPretestForm>().SingleOrDefault();
                if (dashBoardAutoPretestForm != null||( this.ActiveMdiChild!=null && this.ActiveMdiChild.Name == "DashBoardAutoPretestForm"))
                {
                    dashBoardAutoPretestForm.Close();
                }
                string[] SelectedPreTests = autoPretestForm.SelectedPreTests;
                InitializeAuotPreTestUserControls(SelectedPreTests);
                dashBoardAutoPretestForm = new DashBoardAutoPretestForm(SelectedPreTests,darkCurrentTestUserControl, defectsUserControl, redBlueUserControl, meanVarianceTestUserControl,
                                                                        noiseVsNDROTestUserControl, 
                                                                        injectionPerformanceTestUserControl, photoresponseTestUserControl, shutterDriveUserControl, finalTestReportUserControl);
                dashBoardAutoPretestForm.MdiParent = this;
                dashBoardAutoPretestForm.Show();
                ultraStatusBar1.Panels["MarqueeStatus"].Text = "Pretest Initiated.....";
                DisablePretestUserControlTiles();
                MakePretestTilesVisible();
                Application.UseWaitCursor = false;
                //await tempHumidPresenter.SaveTempHumReading(GetTempHumReadingModel("PRE-TEST"));
                List<TempHumReadingModel> tempHumReadingModels = new List<TempHumReadingModel>();

                tempHumReadingModels.Add(tempHumReadingModelInitialConnect);

                TempHumReadingModel tempHumReadingModelBeginTest = new TempHumReadingModel();
                tempHumReadingModelBeginTest.CameraSerialNumber = cameraSerialNumber;
                tempHumReadingModelBeginTest.ReadingDateTime = DateTime.Now;
                tempHumReadingModelBeginTest.TestStage = "PRE_TEST";
                tempHumReadingModelBeginTest.TestStation = Environment.MachineName;
                tempHumReadingModelBeginTest.ImagerHumidity = cameraStatusForLogTemHum.RelativeHumidityInProcent;
                tempHumReadingModelBeginTest.ImagerTemperature = cameraStatusForLogTemHum.ImageSensorTemperatureDiodeInC; ;
                tempHumReadingModelBeginTest.UserExecuted = string.IsNullOrWhiteSpace(enggUserName) ? Environment.UserName.ToUpper() : enggUserName.ToUpper();

                tempHumReadingModels.Add(tempHumReadingModelBeginTest);

                foreach (string str in SelectedPreTests)
                {
                    switch (str)
                    {
                        case "RBT":
                            await RunAutoRedBlue();
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileRedBluePreTest"].Enabled = true;
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileRedBluePreTest"].State = TileState.Large;
                            Thread.Sleep(5000);
                            break;
                        case "DDT":
                            await RunAutoDarkCurrentTest();
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileDarkCurrentPreTest"].Enabled = true;
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileDarkCurrentPreTest"].State = TileState.Large;
                            break;
                        case "DET":
                            await RunAutoDefectsTest();
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileDefectsPreTest"].Enabled = true;
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileDefectsPreTest"].State = TileState.Large;
                            break;
                        case "MVT":
                            await RunAutoMeanVarianceTest();
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileMeanVariance"].Enabled = true;
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileMeanVariance"].State = TileState.Large;
                            break;
                        case "RNT":
                            await RunAutoNoiseVsNDROTest();
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileReadNoise"].Enabled = true;
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileReadNoise"].State = TileState.Large;
                            break;
                        case "PRT":
                            camHumdityListForDBLogging = new List<double>();
                            await RunAutoPhotoresponseTest();
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTilePhotoresponse"].Enabled = true;
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTilePhotoresponse"].State = TileState.Large;
                            break;
                        case "IET":
                            await RunAutoInjectionPerformanceTest();
                            camHumidityDBLoggingAvg = camHumdityListForDBLogging.Average();
                            camHumdityListForDBLogging = null;
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileInjectionPerformance"].Enabled = true;
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileInjectionPerformance"].State = TileState.Large;
                            break;
                        case "SDT":
                            await RunAutoShutterDriveTest();
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileShutterDrive"].Enabled = true;
                            dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileShutterDrive"].State = TileState.Large;
                            break;
                    }
                    if (AutoCancellationTokenSource.IsCancellationRequested || !cidInterface.IsConnected())
                        break;
                    //todo
                    if (checkForAutoTestFailures(str))
                    {
                        if (string.IsNullOrWhiteSpace(strTestfailed))
                        {
                            ultraStatusBar1.Panels["MarqueeStatus"].Text = "Pretest failed";
                        }
                        else
                        {                           
                            if (firmwareErrorsWarnings.Count > 0)
                            {
                                StringBuilder firmwareErrors = new StringBuilder();
                                firmwareErrors.AppendLine(strTestfailed + " failure and due to firmware errors/warnings ");
                                foreach (string errorStr in firmwareErrorsWarnings)
                                {
                                    firmwareErrors.AppendLine(errorStr);
                                }                               
                                ultraStatusBar1.Panels["MarqueeStatus"].Text = firmwareErrors.ToString();
                                ledPretestResult.Value = false;
                                ledPretestResult.OffColor = Color.Red;
                            }
                            else
                            {
                                //autoTestFailReason.Append("Pretest Failed Due to: ");
                                if(!autoTestFailReason.ToString().Contains("Red, Blue & UV"))
                                {
                                    autoTestFailReason.Append(" ' " + strTestfailed + " ' ");
                                }                                                              
                                ultraStatusBar1.Panels["MarqueeStatus"].Text = "Pretest failed due to " + strTestfailed + " failure";
                            }
                                
                        }
                        ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                        dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileRedBluePreTest"].State = TileState.Normal;
                        ledPretestResult.Value = false;
                        ledPretestResult.OffColor = Color.Red;
                        buttonRUNPretest.Enabled = true;
                        buttonAbortPretest.Enabled = false;
                        AutoPreTestStatusResult = false;
                        Application.UseWaitCursor = false;
                        ultraActivityIndicator1.AnimationEnabled = false;
                        ultraActivityIndicator1.ResetAnimation();                       
                        if ((SelectedPreTests.Contains("RBT") && redBlueUserControl != null &&  redBlueUserControl.RedBlueTestAcknowledged)                          
                            || !SelectedPreTests.Contains("RBT"))
                        {
                            SavePreTestResults(new string[] { str }, false, true, tempHumReadingModels);
                        }
                            
                        AutoPreTestStatus = false;
                        firmwareErrorsWarnings.Clear();
                        return;
                    }
                   
                }
                ultraStatusBar1.Panels["TestProgress"].MarqueeInfo.Stop();
                dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileRedBluePreTest"].State = TileState.Normal;
                ultraActivityIndicator1.AnimationEnabled = false;
                ultraActivityIndicator1.ResetAnimation();
                if (AutoCancellationTokenSource.IsCancellationRequested)
                {
                    DisablePretestUserControlTiles();
                    ultraStatusBar1.Panels["MarqueeStatus"].Text = "Auto Pretest Aborted";
                    ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                    ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                    dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTileRedBluePreTest"].State = TileState.Normal;
                    AutoPreTestStatus = false;
                    ledPretestResult.Value = false;
                    buttonRUNPretest.Enabled = true;
                    buttonAbortPretest.Enabled = false;
                    AutoPreTestStatusResult = false;
                    Application.UseWaitCursor = false;
                    ultraActivityIndicator1.AnimationEnabled = false;
                    ultraActivityIndicator1.ResetAnimation();
                    firmwareErrorsWarnings.Clear();                  
                    return;
                }
                else if (!cidInterface.IsConnected())
                {
                    DialogResult dialogResult = MessageBox.Show("Communication to camera/firmware is lost. Please abort the pretest and check for camera/firmware connection.",
                                                        "Pre Test", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    if (dialogResult == DialogResult.OK)
                    {
                        DisableUserControlTiles();
                        ultraStatusBar1.Panels["MarqueeStatus"].Text = "Auto Pretest Aborted due to camera/firmware connection lost.";
                        ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                        ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.ResetFillAppearance(); 
                        ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                        dashBoardAutoForm.ultraTilePanel1.Tiles["ultraTileRedBlue"].State = TileState.Normal;
                        AutoTestStatus = false;
                        buttonRunAuto.Enabled = true;
                        buttonAbort.Enabled = false;
                        AutoTestStatusResult = false;
                        ledAutoTestResult.Value = false;
                        ledAutoTestResult.OffColor = Color.DarkGreen;
                        autoTestFailReason.Clear();
                        firmwareErrorsWarnings.Clear();
                        return;
                    }
                }
                else
                {                   
                    ultraStatusBar1.Panels["MarqueeStatus"].Text = "Auto PreTest Completed";
                    if ((SelectedPreTests.Contains("RBT") && redBlueUserControl != null && redBlueUserControl.RedBlueTestAcknowledged)
                             || !SelectedPreTests.Contains("RBT"))
                    {
                        SavePreTestResults(autoPretestForm.SelectedPreTests, true, true, tempHumReadingModels);
                    }
                       
                    AutoPreTestStatus = false;
                }
                ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                AutoPreTestStatus = false;
                buttonRUNPretest.Enabled = true;
                buttonAbortPretest.Enabled = false;
                ledPretestResult.Value = true;
                AutoPreTestStatusResult = true;
                Application.UseWaitCursor = false;
                firmwareErrorsWarnings.Clear();
                updateCameraTestMetrics();
            }
            catch (Exception ex)
            {
                AutoPreTestStatus = false;
                AutoPreTestStatusResult = false;
                buttonRUNPretest.Enabled = true;
                ledPretestResult.Value = false;
                buttonAbortPretest.Enabled = false;
                Application.UseWaitCursor = false;
                ResetAutoManualStatusBarPanel();
                firmwareErrorsWarnings.Clear();
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void SavePreTestResults(string[] SelectedPreTests,bool preTestResult, 
                                                bool savetoNetwork, List<TempHumReadingModel> tempHumReadingModels=null)
        {
            try
            {
                Dictionary<string, object> preTestObjects = new Dictionary<string, object>();
                OverAllTestResultsModel overAllTestResultsModel = new OverAllTestResultsModel();
                DateTime testExecutedDate = DateTime.Now;
                string filetimestamp = testExecutedDate.ToString("MM_dd_yyyy_HH_mm");
                TestAppHelper.AzureContainerName = "scm5821ax1-pretestresults-" + testExecutedDate.ToString("MM-dd-yyyy-hh-mm");
                if (string.IsNullOrEmpty(defaultFilePath))
                {
                    testResultsPath = System.IO.Path.Combine(ConfigurationManager.AppSettings["TestResultsLocalFilePath"].ToString() +
                                                "SCM5821A_PreTestResults_" + filetimestamp + "_" + cameraDetailsModel.ImagerSerialNumber);
                    if (!System.IO.Directory.Exists(System.IO.Path.Combine(testResultsPath)))
                    {
                        System.IO.Directory.CreateDirectory(testResultsPath);
                    }
                }
                else
                {
                    testResultsPath = System.IO.Path.Combine(defaultFilePath, "SCM5821A_PreTestResults\\" + "SCM5821A_PreTestResults_" + 
                                                filetimestamp + "_" + cameraDetailsModel.ImagerSerialNumber);
                    if (!System.IO.Directory.Exists(System.IO.Path.Combine(testResultsPath)))
                    {
                        System.IO.Directory.CreateDirectory(testResultsPath);
                    }
                }
                string fileName = string.Empty;
                string destFile = string.Empty;
                string serverPathToDB = "SCM5821A_PreTestResults_" + filetimestamp + "_" + cameraDetailsModel.ImagerSerialNumber;
                string testResultsServerPath = System.IO.Path.Combine(Settings.Default.AutoTestResultsServerFilePathFromDB, "SCM5821A_PreTestResults_" 
                                                                        + filetimestamp + "_" + cameraDetailsModel.ImagerSerialNumber);
                if (ConfigurationManager.AppSettings["EndClient"] == "Thermo")
                {
                    if (!System.IO.Directory.Exists(testResultsServerPath))
                    {
                        System.IO.Directory.CreateDirectory(testResultsServerPath);
                    }
                }
                StringBuilder selectedAutoPreTests = new StringBuilder();
                foreach (string str in SelectedPreTests)
                {
                    switch (str)
                    {
                        case "RBT":
                            redBlueUserControl.SaveTestResultImage(testResultsPath, testResultsServerPath, filetimestamp, serverPathToDB, savetoNetwork);
                            redBlueTestResults = redBluePresenter.ReturnRedBlueResults();                          
                            overAllTestResultsModel.RedBlueTestResults= new List<RedBlueTestResults>();
                            overAllTestResultsModel.RedBlueTestResults.Add(redBlueTestResults);
                            preTestObjects.Add("RBT", redBlueTestResults);
                            selectedAutoPreTests.Append("Red Blue" + " , ");
                            if(!redBlueTestResults.TestPassed)
                                preTestResult = false;
                            break;
                        case "DDT":
                            darkCurrentTestUserControl.SaveTestResultImage(testResultsPath, testResultsServerPath, filetimestamp, serverPathToDB, savetoNetwork);
                            darkCurrentTestResults = darkCurrentPresenter.ReturnDarkCurrentResults();
                            overAllTestResultsModel.DarkCurrentTestResults = new List<DarkCurrentTestResults>();
                            overAllTestResultsModel.DarkCurrentTestResults.Add(darkCurrentTestResults);
                            preTestObjects.Add("DDT", darkCurrentTestResults);
                            selectedAutoPreTests.Append("Dark Current" + " , ");
                            if (!darkCurrentTestResults.TestPassed)
                                preTestResult = false;
                            break;
                        case "DET":
                            defectsUserControl.SaveTestResultImage(testResultsPath, testResultsServerPath, filetimestamp, serverPathToDB, savetoNetwork);
                            defectsTestResults = defectsPresenter.ReturnDefectsResults();
                            overAllTestResultsModel.DefectsTestResults = new List<DefectsTestResults>();
                            overAllTestResultsModel.DefectsTestResults.Add(defectsTestResults);
                            preTestObjects.Add("DET", defectsTestResults);
                            selectedAutoPreTests.Append("Defects" + " , ");
                            if (!defectsTestResults.TestPassed)
                                preTestResult = false;
                            break;
                        case "MVT":                           
                            meanVarianceTestUserControl.SaveTestResultImage(testResultsPath, testResultsServerPath, filetimestamp, serverPathToDB, savetoNetwork);
                            meanVarianceTestResultsData = meanVariancePresenter.ReturnMVResults();
                            overAllTestResultsModel.MeanVarinaceTestResults = new List<MeanVarianceTestResultsData>();
                            overAllTestResultsModel.MeanVarinaceTestResults.Add(meanVarianceTestResultsData);
                            preTestObjects.Add("MVT", meanVarianceTestResultsData);
                            selectedAutoPreTests.Append("MeanVariance" + " , ");
                            if (!meanVarianceTestResultsData.TestPassed)
                                preTestResult = false;
                            break;
                        case "RNT":
                            noiseVsNDROTestResults = noiseVsNDROPresenter.ReturnNoiseVsNDROsResults();
                            noiseVsNDROTestUserControl.SaveTestResultImage(testResultsPath, testResultsServerPath, filetimestamp, serverPathToDB, savetoNetwork);                          
                            overAllTestResultsModel.NoiseVsNDROTestResults = new List<NoiseVsNDROTestResults>();
                            overAllTestResultsModel.NoiseVsNDROTestResults.Add(noiseVsNDROTestResults);
                            preTestObjects.Add("RNT", noiseVsNDROTestResults);
                            selectedAutoPreTests.Append("Noise Vs NDROs" + " , ");
                            if (!noiseVsNDROTestResults.TestPassed)
                                preTestResult = false;
                            break;
                        case "PRT":
                            photoresponseTestUserControl.SaveTestResultImage(testResultsPath, testResultsServerPath, filetimestamp, serverPathToDB, savetoNetwork);
                            photoresponseTestResults = photoresponsePresenter.ReturnPhotoresponseResults();
                            overAllTestResultsModel.PhotoresponseTestResults = new List<PhotoresponseTestResults>();
                            overAllTestResultsModel.PhotoresponseTestResults.Add(photoresponseTestResults);
                            preTestObjects.Add("PRT", photoresponseTestResults);
                            selectedAutoPreTests.Append("Photoresponse" + " , ");
                            if (!photoresponseTestResults.TestPassed)
                                preTestResult = false;
                            break;
                        case "IET":
                            injectionPerformanceTestUserControl.SaveTestResultImage(testResultsPath, testResultsServerPath, filetimestamp, serverPathToDB, savetoNetwork);
                            injectionEfficiencyTestResults = injectionEfficiencyPresenter.ReturnInjectionEfficiencyResults();
                            overAllTestResultsModel.InjectionEfficiencyTestResults = new List<InjectionEfficiencyTestResults>();
                            overAllTestResultsModel.InjectionEfficiencyTestResults.Add(injectionEfficiencyTestResults);
                            preTestObjects.Add("IET", injectionEfficiencyTestResults);
                            selectedAutoPreTests.Append("Injection Efficiency" + " , ");
                            if (!injectionEfficiencyTestResults.TestPassed)
                                preTestResult = false;
                            break;
                        case "SDT":
                            shutterDriveUserControl.SaveTestResultImage(testResultsPath, testResultsServerPath, filetimestamp, serverPathToDB, savetoNetwork);
                            shutterDriveTestResults = shutterDrivePresenter.ReturnShutterDriveResults();
                            overAllTestResultsModel.ShutterDriveTestResults = new List<ShutterDriveTestResults>();
                            overAllTestResultsModel.ShutterDriveTestResults.Add(shutterDriveTestResults);
                            preTestObjects.Add("SDT", shutterDriveTestResults);
                            selectedAutoPreTests.Append("Shutter Drive" + " , ");
                            if (!shutterDriveTestResults.TestPassed)
                                preTestResult = false;
                            break;
                    }
                }
                string filename = DateTime.Now.ToString("yymmddThhmmss") + "_" + CameraSerialNumber.TrimEnd(' ') + "_PreTest.txt";
                string pretestFirmwareLogsLocalPath= System.IO.Path.Combine(testResultsPath, "SCM5821AX1FirmwareOutput");
                if (!System.IO.Directory.Exists(pretestFirmwareLogsLocalPath))
                {
                    System.IO.Directory.CreateDirectory(pretestFirmwareLogsLocalPath);
                }
                string pretestFirmwareLogsServerPath = System.IO.Path.Combine(testResultsServerPath, "SCM5821AX1FirmwareOutput");
                if (!System.IO.Directory.Exists(pretestFirmwareLogsServerPath))
                {
                    System.IO.Directory.CreateDirectory(pretestFirmwareLogsServerPath);
                }
                firmwareOutput.AppendLine("Total DeadAndDarkPixelList Count:" + defectsPresenter.DeadAndDarkPixelList.ToString());
                File.AppendAllText(pretestFirmwareLogsLocalPath + "\\" + filename, firmwareOutput.ToString());
                File.AppendAllText(pretestFirmwareLogsServerPath + "\\" + filename, firmwareOutput.ToString());
                firmwareOutput.Clear();

                if (SelectedPreTests.Contains("DET"))
                {
                    defectsPresenter.SerializeObjectFileName = pretestFirmwareLogsServerPath + "\\DefectsTestSerializeObject_" +
                        textBoxImagerSerialNum.Text.Trim('\0').Trim() + "_" + DateTime.Now.ToString("YYMMDDThhmmss") + ".bin";

                    await defectsPresenter.serializeObject();
                }


                //preTestResult = false;

                PopulateFinalTestResultsModel();
                cidInterface.GetCameraInformation();
                cameraDetailsModel.FirmwareVer = textBoxFirmwareVer.Text;
                cameraDetailsModel.FPGAVersion = textBoxFPGAVer.Text;
                cameraDetailsModel.MACAddress = textBoxMacAddress.Text;
                cameraDetailsModel.FirmwareFile = TestAppHelper.GetFirmwareFileName();
                cameraDetailsModel.CameraTestLogs = new List<CameraTestLogsModel>();                
                cameraDetailsModel.CameraTestLogs.Add(populateCameraTestLogs(preTestResult,"PRE-TEST", selectedAutoPreTests.ToString().TrimEnd(new char[] { ',', ' ' })));
                List<BurnInAnalysisModel> cameraBurnInDetails = TestAppHelper.GetBurnInAnalysisData().Where(camera => camera.CameraSN.Equals(cameraDetailsModel.CameraSerialNumber)).ToList();
                if (cameraBurnInDetails.Count() > 0)
                    cameraBurnInData = cameraBurnInDetails[0];
                finalTestReportUserControl.ImageTemperature = cameraStatusForLogTemHum.ImageSensorTemperatureDiodeInC.ToString("F2");
                finalTestReportUserControl.ImageHumidity = cameraStatusForLogTemHum.RelativeHumidityInProcent.ToString("F2");
                finalTestReportUserControl.UpdateFinalTestReportView(finalTestResultsDetailsModel, cameraDetailsModel,
                                                                      shutterDriveTestResults,
                                                                      cameraInformation, cameraBurnInData, enggUserName, 
                                                                      testExecutedDate,false, SelectedPreTests,false, 
                                                                      firmwareErrorsWarnings,
                                                                      defectsPresenter.DefPixelROIListPairs, 
                                                                      defectsPresenter.DefectsTestLimitsDataList, null,null,
                                                                      null, null,false,false);
                dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTilePreTestReport"].Enabled = true;
                dashBoardAutoPretestForm.ultraTilePanelAutoPretest.Tiles["ultraTilePreTestReport"].State = TileState.Large;
                finalTestReportUserControl.SaveFinalTestReportAsImage(testResultsPath, testResultsServerPath, filetimestamp);
                if (await overAllTestResultsPresenter.SaveOverAllPreTestResults(finalTestResultsDetailsModel, cameraDetailsModel,
                                                                                SelectedPreTests, filetimestamp, serverPathToDB,
                                                                                CameraSerialNumber, ImagerSerialNumber, userMode,
                                                                                autoTestFailReason.ToString(), preTestResult, 
                                                                                cameraBurnInData, true, preTestObjects, tempHumReadingModels))

                {
                    prestTestResultsSaved = true;
                    if (!preTestResult)
                    {
                        CameraTestLogDetails cameraTestLogDetails = new CameraTestLogDetails(cameraTestLogsPresenter, userMode, 
                                                                                textBoxCameraSerialNum.Text.Trim('\0').Trim(),
                                                                                textBoxImagerSerialNum.Text.Trim('\0').Trim());
                        cameraTestLogDetails.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }    
        private CameraTestLogsModel populateCameraTestLogs(bool preTest,string testStage, string selectedAutoPreTests)
        {
            try
            {
                CameraTestLogsModel cameraTestLogs = new CameraTestLogsModel();
                cameraTestLogs.DateExecuted= DateTime.Now;
                cameraTestLogs.UserExecuted = string.IsNullOrWhiteSpace(enggUserName) ? Environment.UserName.ToUpper() : enggUserName.ToUpper();
                cameraTestLogs.PreTestResult = testStage.Equals("PRE-TEST") ? preTest : false; 
                cameraTestLogs.PreTestCharacterizationTests = testStage.Equals("PRE-TEST") ? selectedAutoPreTests : string.Empty;
                cameraTestLogs.FinalTestResult = testStage.Equals("PRE-TEST") ? false : preTest;
                cameraTestLogs.TestStage = testStage;
                cameraTestLogs.FailureMode = string.Empty;
                cameraTestLogs.RootCause = string.Empty;
                cameraTestLogs.Symptom = string.Empty;
                cameraTestLogs.Disposition = string.Empty;
                //cameraTestLogs.BoardSN = "ISI:" + textBoxISISerialNum.Text + "," + "CSP:" + textBoxCSPSerialNum.Text + "," +
                //                         "PWR:" + textBoxPwrSerialNum.Text + "," + "Imager:" + textBoxImagerSerialNum.Text + "," + "CPU:" + textBoxCPUSerialNum.Text;
                cameraTestLogs.BoardSN = string.Empty;
                cameraTestLogs.GenerateMFR = false;
                return cameraTestLogs;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }
        private void ResetPretestStatusBarPanel()
        {
            try
            {
                ultraStatusBar1.Panels["TestProgress"].Visible = false;
                ultraStatusBar1.Panels["PretestStatus"].Visible = true;
                ultraActivityIndicator1.Visible = true;
                ultraActivityIndicator1.AnimationEnabled = true;
                ultraActivityIndicator1.AnimationSpeed = 50;
            }
            catch (Exception ex)
            {              
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public void ResetAutoManualStatusBarPanel()
        {
            try
            {
                ultraStatusBar1.Panels["TestProgress"].Visible = true;
                ultraStatusBar1.Panels["PretestStatus"].Visible = false;
                ultraActivityIndicator1.Visible = false;
                ultraActivityIndicator1.AnimationEnabled = false;
                ultraActivityIndicator1.ResetAnimation();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void buttonAbortPretest_Click(object sender, EventArgs e)
        {
            try
            {               
                if (AutoCancellationTokenSource != null)
                {
                    DialogResult result = MessageBox.Show("Are you sure to Abort Auto Pretest?",
                   "Abort Auto Pretest",
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        AbortAutoTest(true);
                        AutoPreTestStatus = false;
                        buttonRUNPretest.Enabled = true;
                    }
                    else
                        return;
                }
            }
            catch (Exception ex)
            {
                AutoPreTestStatus = false;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ShutterDriveGraphPreference()
        {
            try
            {
                if (shutterDriveTestForm != null)
                {
                    if (testGraphPreferences != null)
                    {
                        shutterDriveTestForm.shutterWaveformGraph.PlotAreaColor  =
                        testGraphPreferences.PlotBackColorWhite ? Color.White : Color.Black;
                       shutterDriveTestForm.AIO.LineWidth = shutterDriveTestForm.AI1.LineWidth
                        = shutterDriveTestForm.AI2.LineWidth = testGraphPreferences.LineWidthOfSelectedGraph;
                    }
                    else
                    {
                        shutterDriveTestForm.shutterWaveformGraph.PlotAreaColor = 
                        Settings.Default.plotBackgroundColor ? Color.White : Color.Black;
                        if (Settings.Default.lineWidthOfPlot > 0)
                            shutterDriveTestForm.AIO.LineWidth = shutterDriveTestForm.AI1.LineWidth
                        = shutterDriveTestForm.AI2.LineWidth = Settings.Default.lineWidthOfPlot;
                    }
                }
                if (shutterDriveUserControl != null)
                {
                    if (testGraphPreferences != null)
                    {
                        shutterDriveUserControl.shutterWaveformGraph.PlotAreaColor = 
                        testGraphPreferences.PlotBackColorWhite ? Color.White : Color.Black;
                        shutterDriveUserControl.AIO.LineWidth = shutterDriveUserControl.AI1.LineWidth
                        = shutterDriveUserControl.AI2.LineWidth = testGraphPreferences.LineWidthOfSelectedGraph;
                    }
                    else
                    {
                        shutterDriveUserControl.shutterWaveformGraph.PlotAreaColor = 
                        Properties.Settings.Default.plotBackgroundColor ? Color.White : Color.Black;
                        if (Settings.Default.lineWidthOfPlot > 0)
                           shutterDriveUserControl.AIO.LineWidth = shutterDriveUserControl.AI1.LineWidth
                       = shutterDriveUserControl.AI2.LineWidth = Settings.Default.lineWidthOfPlot;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void MeanVarianceGraphPreference()
        {
            try
            {
                if (meanVarianceTestForm != null)
                {
                    if (testGraphPreferences != null)
                    {
                        meanVarianceTestForm.meanVarianceScatterGraph.PlotAreaColor = meanVarianceTestForm.meanVarianceWaveformGraph.PlotAreaColor =
                        testGraphPreferences.PlotBackColorWhite ? Color.White : Color.Black;
                        meanVarianceTestForm.meanVarianceScatterPlot.LineWidth = meanVarianceTestForm.meanofROI1minus2Plot.LineWidth =
                        meanVarianceTestForm.linearCurveFitOfVarPlot.LineWidth = meanVarianceTestForm.varianceOfROI1minus2Plot.LineWidth = testGraphPreferences.LineWidthOfSelectedGraph;
                    }
                    else
                    {
                        meanVarianceTestForm.meanVarianceScatterGraph.PlotAreaColor = meanVarianceTestForm.meanVarianceWaveformGraph.PlotAreaColor =
                        Properties.Settings.Default.plotBackgroundColor ? Color.White : Color.Black;
                        if (Settings.Default.lineWidthOfPlot > 0)
                            meanVarianceTestForm.meanVarianceScatterPlot.LineWidth = meanVarianceTestForm.meanofROI1minus2Plot.LineWidth =
                            meanVarianceTestForm.linearCurveFitOfVarPlot.LineWidth = meanVarianceTestForm.varianceOfROI1minus2Plot.LineWidth = Settings.Default.lineWidthOfPlot;
                    }
                }
                if (meanVarianceTestUserControl != null)
                {
                    if (testGraphPreferences != null)
                    {
                        meanVarianceTestUserControl.meanVarianceScatterGraph.PlotAreaColor = meanVarianceTestUserControl.meanVarianceWaveformGraph.PlotAreaColor =
                        testGraphPreferences.PlotBackColorWhite ? Color.White : Color.Black;
                        meanVarianceTestUserControl.meanVarianceScatterPlot.LineWidth = meanVarianceTestUserControl.meanofROI1minus2Plot.LineWidth =
                        meanVarianceTestUserControl.linearCurveFitOfVarPlot.LineWidth = meanVarianceTestUserControl.varianceOfROI1minus2Plot.LineWidth = testGraphPreferences.LineWidthOfSelectedGraph;
                    }
                    else
                    {
                        meanVarianceTestUserControl.meanVarianceScatterGraph.PlotAreaColor = meanVarianceTestUserControl.meanVarianceWaveformGraph.PlotAreaColor =
                        Properties.Settings.Default.plotBackgroundColor ? Color.White : Color.Black;
                        if (Settings.Default.lineWidthOfPlot > 0)
                            meanVarianceTestUserControl.meanVarianceScatterPlot.LineWidth = meanVarianceTestUserControl.meanofROI1minus2Plot.LineWidth =
                            meanVarianceTestUserControl.linearCurveFitOfVarPlot.LineWidth = meanVarianceTestUserControl.varianceOfROI1minus2Plot.LineWidth = Settings.Default.lineWidthOfPlot;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }       
        private void DarkCurrentGraphPreference()
        {
            try
            {
                if (darkCurrentTestForm != null)
                {
                    if (testGraphPreferences != null)
                    {
                        darkCurrentTestForm.darkCurrentWaveformGraph.PlotAreaColor = testGraphPreferences.PlotBackColorWhite ? Color.White : Color.Black;
                        darkCurrentTestForm.TheoryPlot.LineWidth = darkCurrentTestForm.MeasuredNoisePlot.LineWidth = testGraphPreferences.LineWidthOfSelectedGraph;
                    }
                    else
                    {
                        darkCurrentTestForm.darkCurrentWaveformGraph.PlotAreaColor = Properties.Settings.Default.plotBackgroundColor ? Color.White : Color.Black;
                        if (Settings.Default.lineWidthOfPlot > 0)
                            darkCurrentTestForm.TheoryPlot.LineWidth = darkCurrentTestForm.MeasuredNoisePlot.LineWidth = Settings.Default.lineWidthOfPlot;
                    }
                }
                if (darkCurrentTestUserControl != null)
                {
                    if (testGraphPreferences != null)
                    {
                        darkCurrentTestUserControl.darkCurrentWaveformGraph.PlotAreaColor = testGraphPreferences.PlotBackColorWhite ? Color.White : Color.Black;
                        darkCurrentTestUserControl.TheoryPlot.LineWidth = darkCurrentTestUserControl.MeasuredNoisePlot.LineWidth = testGraphPreferences.LineWidthOfSelectedGraph;
                    }
                    else
                    {
                        darkCurrentTestUserControl.darkCurrentWaveformGraph.PlotAreaColor = Properties.Settings.Default.plotBackgroundColor ? Color.White : Color.Black;
                        if (Settings.Default.lineWidthOfPlot > 0)
                            darkCurrentTestUserControl.TheoryPlot.LineWidth = darkCurrentTestUserControl.MeasuredNoisePlot.LineWidth = Settings.Default.lineWidthOfPlot;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void InjectionPerformanceGraphPreference()
        {
            try
            {
                if (injectionEfficiencyTestForm != null)
                {
                    if (testGraphPreferences != null)
                    {
                        injectionEfficiencyTestForm.chargeInjectionEfficiencyWaveformGraph.PlotAreaColor 
                            = testGraphPreferences.PlotBackColorWhite ? Color.White : Color.Black;
                        injectionEfficiencyTestForm.EXP0WaveformPlot.LineWidth = injectionEfficiencyTestForm.waveformPlot1.LineWidth =
                        injectionEfficiencyTestForm.EXP1WaveformPlot.LineWidth = injectionEfficiencyTestForm.EXP2WaveformPlot.LineWidth =
                        injectionEfficiencyTestForm.EXP3WaveformPlot.LineWidth = injectionEfficiencyTestForm.EXP4WaveformPlot.LineWidth=
                        injectionEfficiencyTestForm.EXP5WaveformPlot.LineWidth = testGraphPreferences.LineWidthOfSelectedGraph;
                    }
                    else
                    {
                        injectionEfficiencyTestForm.chargeInjectionEfficiencyWaveformGraph.PlotAreaColor
                            = Properties.Settings.Default.plotBackgroundColor ? Color.White : Color.Black;
                        if (Settings.Default.lineWidthOfPlot > 0)
                            injectionEfficiencyTestForm.EXP0WaveformPlot.LineWidth = injectionEfficiencyTestForm.waveformPlot1.LineWidth =
                            injectionEfficiencyTestForm.EXP1WaveformPlot.LineWidth = injectionEfficiencyTestForm.EXP2WaveformPlot.LineWidth =
                            injectionEfficiencyTestForm.EXP3WaveformPlot.LineWidth = injectionEfficiencyTestForm.EXP4WaveformPlot.LineWidth =
                            injectionEfficiencyTestForm.EXP5WaveformPlot.LineWidth = Settings.Default.lineWidthOfPlot;
                    }
                }
                if (injectionPerformanceTestUserControl != null)
                {
                    if (testGraphPreferences != null)
                    {
                        injectionPerformanceTestUserControl.chargeInjectionEfficiencyWaveformGraph.PlotAreaColor 
                         = testGraphPreferences.PlotBackColorWhite ? Color.White : Color.Black;
                        injectionPerformanceTestUserControl.EXP0WaveformPlot.LineWidth = injectionPerformanceTestUserControl.waveformPlot1.LineWidth =
                          injectionPerformanceTestUserControl.EXP1WaveformPlot.LineWidth = injectionPerformanceTestUserControl.EXP2WaveformPlot.LineWidth =
                          injectionPerformanceTestUserControl.EXP3WaveformPlot.LineWidth = injectionPerformanceTestUserControl.EXP4WaveformPlot.LineWidth =
                          injectionPerformanceTestUserControl.EXP5WaveformPlot.LineWidth = testGraphPreferences.LineWidthOfSelectedGraph;
                    }
                    else
                    {
                        injectionPerformanceTestUserControl.chargeInjectionEfficiencyWaveformGraph.PlotAreaColor  
                        = Properties.Settings.Default.plotBackgroundColor ? Color.White : Color.Black;
                        if (Settings.Default.lineWidthOfPlot > 0)
                            injectionPerformanceTestUserControl.EXP0WaveformPlot.LineWidth = injectionPerformanceTestUserControl.waveformPlot1.LineWidth =
                           injectionPerformanceTestUserControl.EXP1WaveformPlot.LineWidth = injectionPerformanceTestUserControl.EXP2WaveformPlot.LineWidth =
                           injectionPerformanceTestUserControl.EXP3WaveformPlot.LineWidth = injectionPerformanceTestUserControl.EXP4WaveformPlot.LineWidth =
                           injectionPerformanceTestUserControl.EXP5WaveformPlot.LineWidth = Settings.Default.lineWidthOfPlot;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void NoiseVsNDROsGraphPreference()
        {
            try
            {
                if (noiseVsNDROTestForm != null)
                {
                    if (testGraphPreferences != null)
                    {
                        noiseVsNDROTestForm.readNoiseScatterGraph.PlotAreaColor = testGraphPreferences.PlotBackColorWhite ? Color.White : Color.Black;
                        noiseVsNDROTestForm.measuredNoiseScatterPlot.LineWidth = noiseVsNDROTestForm.log20ScatterPlot.LineWidth = testGraphPreferences.LineWidthOfSelectedGraph;
                    }
                    else
                    {
                        noiseVsNDROTestForm.readNoiseScatterGraph.PlotAreaColor = Settings.Default.plotBackgroundColor ? Color.White : Color.Black;
                        if (Settings.Default.lineWidthOfPlot > 0)
                            noiseVsNDROTestForm.measuredNoiseScatterPlot.LineWidth = noiseVsNDROTestForm.log20ScatterPlot.LineWidth = Settings.Default.lineWidthOfPlot;
                    }
                }
                if (noiseVsNDROTestUserControl != null)
                {
                    if (testGraphPreferences != null)
                    {
                        noiseVsNDROTestUserControl.readNoiseScatterGraph.PlotAreaColor = testGraphPreferences.PlotBackColorWhite ? Color.White : Color.Black;
                        noiseVsNDROTestUserControl.measuredNoiseScatterPlot.LineWidth = noiseVsNDROTestUserControl.log20ScatterPlot.LineWidth = testGraphPreferences.LineWidthOfSelectedGraph;
                    }
                    else
                    {
                        noiseVsNDROTestUserControl.readNoiseScatterGraph.PlotAreaColor = Settings.Default.plotBackgroundColor ? Color.White : Color.Black;
                        if (Settings.Default.lineWidthOfPlot > 0)
                            noiseVsNDROTestUserControl.measuredNoiseScatterPlot.LineWidth = noiseVsNDROTestUserControl.log20ScatterPlot.LineWidth = Settings.Default.lineWidthOfPlot;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void PhotoresponseGraphPreference()
        {
            try
            {
                if (photoresponseTestForm != null)
                {
                    if (testGraphPreferences != null)
                    {
                        photoresponseTestForm.photoResponseWaveformGraph.PlotAreaColor =
                        photoresponseTestForm.derivativeWaveformGraph.PlotAreaColor = testGraphPreferences.PlotBackColorWhite ? Color.White : Color.Black;
                        photoresponseTestForm.photoResponseTestWaveformPlot.LineWidth = photoresponseTestForm.meanWaveformPlot.LineWidth =
                        photoresponseTestForm.linearFitWaveformPlot.LineWidth = photoresponseTestForm.meanofROI1minus2Plot.LineWidth =
                        photoresponseTestForm.linearCurveFitOfVarPlot.LineWidth = photoresponseTestForm.varianceOfROI1minus2Plot.LineWidth
                        = testGraphPreferences.LineWidthOfSelectedGraph;
                    }
                    else
                    {
                        photoresponseTestForm.photoResponseWaveformGraph.PlotAreaColor =
                        photoresponseTestForm.derivativeWaveformGraph.PlotAreaColor = Settings.Default.plotBackgroundColor ? Color.White : Color.Black;
                        if (Settings.Default.lineWidthOfPlot > 0)
                            photoresponseTestForm.photoResponseTestWaveformPlot.LineWidth = photoresponseTestForm.meanWaveformPlot.LineWidth =
                           photoresponseTestForm.linearFitWaveformPlot.LineWidth = photoresponseTestForm.meanofROI1minus2Plot.LineWidth =
                           photoresponseTestForm.linearCurveFitOfVarPlot.LineWidth = photoresponseTestForm.varianceOfROI1minus2Plot.LineWidth = Settings.Default.lineWidthOfPlot;
                    }
                }
                if (photoresponseTestUserControl != null)
                {
                    if (testGraphPreferences != null)
                    {
                        photoresponseTestUserControl.photoResponseWaveformGraph.PlotAreaColor =
                        photoresponseTestUserControl.derivativeWaveformGraph.PlotAreaColor = testGraphPreferences.PlotBackColorWhite ? Color.White : Color.Black;
                        photoresponseTestUserControl.photoResponseTestWaveformPlot.LineWidth = photoresponseTestUserControl.meanWaveformPlot.LineWidth =
                        photoresponseTestUserControl.linearFitWaveformPlot.LineWidth = photoresponseTestUserControl.meanofROI1minus2Plot.LineWidth =
                        photoresponseTestUserControl.linearCurveFitOfVarPlot.LineWidth = photoresponseTestUserControl.varianceOfROI1minus2Plot.LineWidth =
                        testGraphPreferences.LineWidthOfSelectedGraph;
                    }
                    else
                    {
                        photoresponseTestUserControl.photoResponseWaveformGraph.PlotAreaColor =
                        photoresponseTestUserControl.derivativeWaveformGraph.PlotAreaColor = Settings.Default.plotBackgroundColor ? Color.White : Color.Black;
                        if (Settings.Default.lineWidthOfPlot > 0)
                            photoresponseTestUserControl.photoResponseTestWaveformPlot.LineWidth = photoresponseTestUserControl.meanWaveformPlot.LineWidth =
                            photoresponseTestUserControl.linearFitWaveformPlot.LineWidth = photoresponseTestUserControl.meanofROI1minus2Plot.LineWidth =
                            photoresponseTestUserControl.linearCurveFitOfVarPlot.LineWidth = photoresponseTestUserControl.varianceOfROI1minus2Plot.LineWidth = Settings.Default.lineWidthOfPlot;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void RedBlueGraphPreference()
        {
            try
            {
                if (redBlueTest != null)
                {
                    if (testGraphPreferences != null)
                    {
                        redBlueTest.redBlueIntensityGraph.PlotAreaColor = redBlueTest.blueIntensityGraph.PlotAreaColor = redBlueTest.differenceIntensityGraph.PlotAreaColor =
                           testGraphPreferences.PlotBackColorWhite ? Color.White : Color.Black;
                    }
                    else
                    {
                        redBlueTest.redBlueIntensityGraph.PlotAreaColor = redBlueTest.blueIntensityGraph.PlotAreaColor = redBlueTest.differenceIntensityGraph.PlotAreaColor =
                             Properties.Settings.Default.plotBackgroundColor ? Color.White : Color.Black;
                    }
                }
                if (redBlueUserControl != null)
                {
                    if (testGraphPreferences != null)
                    {
                        redBlueUserControl.redBlueIntensityGraph.PlotAreaColor = redBlueUserControl.blueIntensityGraph.PlotAreaColor = redBlueUserControl.differenceIntensityGraph.PlotAreaColor =
                             testGraphPreferences.PlotBackColorWhite ? Color.White : Color.Black;
                    }
                    else
                    {
                        redBlueUserControl.redBlueIntensityGraph.PlotAreaColor = redBlueUserControl.blueIntensityGraph.PlotAreaColor = redBlueUserControl.differenceIntensityGraph.PlotAreaColor =
                            Properties.Settings.Default.plotBackgroundColor ? Color.White : Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void PlotBackColorWhite()
        {
            try
            {
                await Task.Run(() =>
                {
                    MeanVarianceGraphPreference();
                    ShutterDriveGraphPreference();
                    DarkCurrentGraphPreference();
                    InjectionPerformanceGraphPreference();
                    NoiseVsNDROsGraphPreference();
                    PhotoresponseGraphPreference();
                    RedBlueGraphPreference();
                });
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraTabbedMdiManager1_TabDragging(object sender, Infragistics.Win.UltraWinTabbedMdi.CancelableMdiTabEventArgs e)
        {
        }
        private void ultraExplorerBar1_Click(object sender, EventArgs e)
        {
            ResetProgressBar();
        }
        private void KronosTestAppMainForm_MdiChildActivate(object sender, EventArgs e)
        {
            ResetProgressBar();
        }
        private bool? userModeChangedCheck(string userModeValue)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userMode))
                {
                    TestAppHelper.UserModeChanged = false;
                    return null;
                }
                else if (!userMode.Equals(userModeValue))
                {
                    DialogResult dialogResult = MessageBox.Show("Below changes will take place when user mode is changed." + Environment.NewLine + Environment.NewLine +
                         "1. Changes made to exposure settings will be lost. " + Environment.NewLine +
                         "2. Changes made to test limits will be lost." + Environment.NewLine +
                         "3. Any open test windows will be closed." + Environment.NewLine + Environment.NewLine +
                         "Do you wish to continue?",
                         "User Mode Change", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (dialogResult == DialogResult.No)
                    {
                        TestAppHelper.UserModeChanged = false;
                        return  false;
                    }
                    else
                    {
                        TestAppHelper.UserModeChanged = true;                       
                        return true;
                    }
                }
                
                return false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
        private async void TECCheckBox_Click(object sender, EventArgs e)
        {
            try
            {
                if (TECCheckBox.Checked)
                {
                    DialogResult result = MessageBox.Show("Make sure purge gas has been running for at least 2 hours before enabling TEC Cooler",
                        "Enabling TEC Cooler",
                          MessageBoxButtons.OKCancel,
                          MessageBoxIcon.Warning);
                    if (result == DialogResult.OK)
                    {
                        if (cidInterface.IsConnected())
                        {
                            log.Info("Enabling TEC Cooler");
                            if (!string.IsNullOrWhiteSpace(userMode) && !userMode.Equals("Manufacturing"))
                            {
                                SetTargetTemperature.Enabled = true;
                            }
                            cidInterface.SetThermoElectricCoolerTemperature((float)SetTargetTemperature.Value);
                            cidInterface.EnableThermoElectricCooler();
                            TECCheckBox.Checked = true;
                            //await Task.Delay(30000);
                            await ConnectTossCooling();
                        }
                        else
                        {
                            MessageBox.Show("Make sure host is connected to camera/firmware.",
                                "Camera Not Connected",
                             MessageBoxButtons.OKCancel,
                             MessageBoxIcon.Warning);
                            TECCheckBox.Checked = false;
                        }
                    }
                }
                else
                {
                    SetTargetTemperature.Enabled = false;
                    cidInterface.DisableThermoElectricCooler();
                    TECCheckBox.Checked = false;
                    if (ssCoolingSerialPort.IsOpen)
                    {
                        ssCoolingSerialPort.Close();
                    }
                    ultraStatusBar1.Panels["ChillerWaterLevelWarn"].Visible = false;
                    if (environmentalStatusForm != null)
                    {
                        environmentalStatusForm.CoolantTempVal = environmentalStatusForm.SetPointTempVal =
                    environmentalStatusForm.PumpTempVal = environmentalStatusForm.PWMCoolingVal =
                    environmentalStatusForm.FanSpeedVal = environmentalStatusForm.TankLevelVal =
                    environmentalStatusForm.FaultStatusVal = string.Empty;

                        environmentalStatusForm.CoolantTempPassed = null;
                        environmentalStatusForm.TankLevelPassed = null;
                    }


                }
                Settings.Default.TECEnabled = TECCheckBox.Checked;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void buttonRUNPretest_MouseEnter(object sender, EventArgs e)
        {
            buttonRUNPretest.ForeColor = Color.LimeGreen;
        }
        private void buttonRUNPretest_MouseLeave(object sender, EventArgs e)
        {
            buttonRUNPretest.ForeColor = Color.Black;
        }
        private void buttonAbortPretest_MouseEnter(object sender, EventArgs e)
        {
            buttonAbortPretest.ForeColor = Color.Red;
        }
        private void buttonAbortPretest_MouseLeave(object sender, EventArgs e)
        {
            buttonAbortPretest.ForeColor = Color.Black;
        }      
        private void ultraStatusBar1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);               
            }
        }       
        private void logTempHumidity_Click(object sender, EventArgs e)
        {
            try
            {
                if(logTempHumidity.Checked)
                {
                    ParametersModel parametersModel = TestAppHelper.GetParametersModel();
                    string path= parametersModel.TestResultsServerPath.ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void updatelogTempHumTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (logTempHumidity.Checked && cameraStatusForLogTemHum != null)
                {
                    camTemp.Add(cameraStatusForLogTemHum.ImageSensorTemperatureDiodeInC);
                    camHum.Add(cameraStatusForLogTemHum.RelativeHumidityInProcent);
                    tempLogDateTime.Add(DateTime.Now.ToString("mm/dd/yyyy_hh:mm:ss tt"));
                    
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void ledCalibrationTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                List<LEDCalibrationTestResults> lEDCalibrationTestResults = await ledCalibrationPresenter.GetLEDCalibrationResults(Environment.MachineName);
                //await ledCalibrationPresenter.GetLimits();
                //TestAppHelper.LEDCalibIntervalConfig = ledCalibrationPresenter.LEDCalibrationLimitsDataList[0].TestInterval;
                if (TestAppHelper.LEDCalibIntervalConfig == lEDCalibrationTestResults.Count-1 ||
                    TestAppHelper.LEDCalibSavedCounter == TestAppHelper.LEDCalibIntervalConfig)
                {
                    TestAppHelper.LEDCalibSavedCounter = 0;
                    UltraDesktopAlertShowWindowInfo windowInfo1 = new UltraDesktopAlertShowWindowInfo
                                                                    ("Auto LED Calibration Test.", "Running LED Calibration Test in Auto mode.");
                    if (File.Exists(Application.StartupPath.ToString() + "\\Resources\\CameraTempAlert.wav"))
                    {
                        windowInfo1.Sound = Application.StartupPath.ToString() + "\\Resources\\CameraTempAlert.wav";
                    }
                    windowInfo1.ScreenPosition = ScreenPosition.Center;
                    // ultraDesktopAlert1.AutoCloseDelay = 3000;
                    ultraDesktopAlert1.Show(windowInfo1);
                    LEDCalibrationTest lEDCalibrationTest = new LEDCalibrationTest(this, cidInterface, ledCalibrationPresenter);
                    await lEDCalibrationTest.RunLEDCalibration();                    
                    ultraDesktopAlert1.CloseAll();
                    TestAppHelper.LEDCalibSavedCounter = 0;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void getNetworkSpeed()
        {
            try
            {
                NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
                if(adapters.Count() > 0)
                {
                    foreach (NetworkInterface adapter in adapters)
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        IPv4InterfaceStatistics stats = adapter.GetIPv4Statistics();
                        if ((adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                            adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet) && adapter.OperationalStatus == OperationalStatus.Up)
                        {
                            ethernetAdapter = adapter;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public void camTempAutoTestAlertTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (TempLowLimit == 0)
                {
                    TempLowLimit = 44.5;
                    TempHighLimit = 65;
                }

                if (cameraStatusForLogTemHum != null &&  Math.Abs(cameraStatusForLogTemHum.ImageSensorTemperatureDiodeInC) >= TempLowLimit &&
                            Math.Abs(cameraStatusForLogTemHum.ImageSensorTemperatureDiodeInC) <= TempHighLimit)                  
                {
                   
                }                                 
               
                else
                {
                    if (autoTestForm != null && userMode.Equals("Manufacturing") && AutoTestStatus)
                    {
                        if (camTempAlertResult == DialogResult.None && !camTempAlertDisplayed)
                        {
                            camTempAlertDisplayedCounter++;
                            camTempAlertDisplayed = true;
                            string str= string.Format("Camera temperature is out of minimum range of -44  for auto test.");
                            camTempAlertResult = MessageBox.Show(str, "Auto Test Camera Temperature Alert",
                                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        camTempAlertDisplayed = false;
                        camTempAlertResult = DialogResult.None;
                    }
                }             

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public void updateNetworkSpeedTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!ultraStatusBar1.IsDisposed)
                {
                    if (ethernetAdapter != null)
                    {
                        ethernetSpeed = (ethernetAdapter.Speed / 1000 / 1000 / 1000);

                        if (ethernetSpeed < 1)
                        {
                            ultraStatusBar1.Panels["NetworkSpeed"].Appearance.BackColor = Color.Red;
                            ultraStatusBar1.Panels["NetworkSpeed"].Text = "Network Speed < 1 Gigabits/Sec";
                        }
                        else
                        {
                            ultraStatusBar1.Panels["NetworkSpeed"].Appearance.BackColor = Color.LimeGreen;
                            ultraStatusBar1.Panels["NetworkSpeed"].Text = string.Format("{0} {1} {2}", "Network Speed: ", ethernetSpeed.ToString(), "Gigabits/sec");
                        }
                    }
                    else
                    {
                        ultraStatusBar1.Panels["NetworkSpeed"].Appearance.BackColor = Color.Red;
                        ultraStatusBar1.Panels["NetworkSpeed"].Text = "No Ethernet/Wireless adapters found.";
                    }
                }
               
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }             
        private async void btnBurnInAnalyze_Click(object sender, EventArgs e)
        {
            burnInAnalysis.CameraSerialNumber = textBoxCameraSerialNum.Text.Trim('\0').TrimEnd(' ');
            burnInAnalysis.ImagerSerialNumber = textBoxImagerSerialNum.Text.Trim('\0').TrimEnd(' ');
            burnInAnalysis.ShowDialog();
            if (TestAppHelper.kronosTestAppMainForm.ledBurnInAnalysisResult.Value)
                btnBurnInAnalyze.Enabled = false;
            updateCameraBurnInMetrics();
        }
        private void saveAutoResultsToDisk_Click(object sender, EventArgs e)
        {
            //if(saveAutoResultsToDisk.Checked)
        }

        private void SetTargetTemperature_ValueChanged(object sender, EventArgs e)
        {
            if (cidInterface != null)
            {
                cidInterface.SetThermoElectricCoolerTemperature((float)SetTargetTemperature.Value);
            }
        }

        private void KronosTestAppMainForm_Shown(object sender, EventArgs e)
        {

        }
    }
}