using Infragistics.Win.Misc;
using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.Presenter;
using KronosCameraTestApp.View.UserControls;
using LabJack.LabJackUD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thermo.Kronos.Instrument.Camera.Interface;
namespace KronosCameraTestApp.View
{
    public partial class ShutterDriveTestForm : Form
    {
        public ShutterDrivePresenter shutterDrivePresenter { get; set; }
		ListExposureData listExposureData { get; set; }
		private string userMode = string.Empty;
        bool engineeringLoginStatus = false;
        private CIDInterface cidInterface = null;
        public int currentRecord = 0;
        private U3 u3;
        private LJUD.IO ioType = 0;
        private LJUD.CHANNEL channel = 0;
        private double dblValue = 0;
        private double ValueDIPort = 0;
        private int dummyInt = 0;
        private double dummyDouble = 0;
        private double[] ValueDIN = new double[16];
        double[] dblValuetemp;
        double[] dblA1emp;
        bool labJackNotFound = true;
        double[] voltageResultList;
        CancellationTokenSource cts { get; set; }
        ExposureSettings exposureSettings { get; set; }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public CIDInterface CidInterface
        {
            get { return cidInterface; }
            set { cidInterface = value; }
        }
        bool shutterDriveTestStatus = false;
        Form mdiParent = null;
        public bool ShutterDriveTestStatus
        {
            get { return shutterDriveTestStatus; }
            set { shutterDriveTestStatus = value; }
        } 
        public ShutterDriveTestForm()
        {
            InitializeComponent();            
        }
        public ShutterDriveTestForm(ShutterDrivePresenter shutterDrivePresenter, string UserMode, bool enggLoginStatus,
            CIDInterface cidInterface, Form MDIParent)
        {
            InitializeComponent();
            this.shutterDrivePresenter = shutterDrivePresenter;
            this.userMode = UserMode;
            this.engineeringLoginStatus = enggLoginStatus;
            this.cidInterface = cidInterface;
            mdiParent = MDIParent;
        }
        private async void buttonShutterDriveRun_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                        long resolution = 0;
                        u3 = new U3(LJUD.CONNECTION.USB, "0", true); // Connection through USB
                        //Configure resolution. See section 2.6/3.1 of the User's Guide.
                        LJUD.ePut(u3.ljhandle, LJUD.IO.PUT_CONFIG, LJUD.CHANNEL.AIN_RESOLUTION, resolution, 0);
                }
                catch (LabJackUDException ex)
                {
                    log.Error("LabJack open error Shutter Drive test may be impacted." + MethodBase.GetCurrentMethod(), ex);                   
                    labJackNotFound = true;
                    ledPassFail.Value = false;
                    ledPassFail.OffColor = Color.Red;
                    shutterDrivePresenter.ShutterDriveTestPassed = false;
                    ((KronosTestAppMainForm)mdiParent).mainFormDockManager.DockAreas[1].Closed = false;
                    ((KronosTestAppMainForm)mdiParent).mainFormDockManager.DockAreas[1].Activate();
                    ((KronosTestAppMainForm)mdiParent).mainFormDockManager.DockAreas[1].Pin();
                    ((KronosTestAppMainForm)mdiParent).firmwareLoggingTextBox.AppendText(Environment.NewLine);
                    ((KronosTestAppMainForm)mdiParent).firmwareLoggingTextBox.AppendText("LabJack open error Shutter Drive test may be impacted.");
                    ((KronosTestAppMainForm)mdiParent).firmwareLoggingTextBox.AppendText(Environment.NewLine);
                    MessageBox.Show("LabJack open error Shutter Drive test may be impacted.", "LabJack Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }               
                if (((KronosTestAppMainForm)mdiParent).AutoTestStatus)
                {
                    MessageBox.Show("Auto Test is under process. Please wait until the test is completed");
                    return;
                }
                if (((KronosTestAppMainForm)mdiParent).AutoPreTestStatus)
                {
                    MessageBox.Show("PreTest is under process. Please wait until the test is completed");
                    return;
                }
                else if (cidInterface.IsConnected())
                {
                    if (Convert.ToInt32(numericEditNumOfExposures.Value) == 0 || Convert.ToInt32(numericEditNumOfExposures.Value) < 0)
                    {
                        MessageBox.Show("Exposure count should be greater than 0", "Invalid exposure count", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                    ((KronosTestAppMainForm)mdiParent).FirmwareManualTestLogsFile = "ShutterDrive_ManualTestFWLog_" +
                        ((KronosTestAppMainForm)mdiParent).CameraSerialNumber.Trim('\0').TrimEnd() + "_" + DateTime.Now.ToString("yymmddThhmm") + ".txt";
                    ledPassFail.Value = false;
                    ledPassFail.OffColor = Color.DarkGreen;
                    // Instantiate the CancellationTokenSource.  
                    cts = new CancellationTokenSource();
                    TestAppHelper.currentRunningTest = "ShutterDrive";
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 1;
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Shutter Drive Test Processing....";
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Start();
                    buttonShutterRun.Enabled = false;
                    buttonShutterAbort.Enabled = true;
                    AIO.ClearData();
                    AI1.ClearData();
                    AI2.ClearData();
                    shutterWaveformGraph.Refresh();
                    shutterDriveTestStatus = true;
                    shutterDrivePresenter.ExposureDataList = exposureSettings.ExposureList; 
                    shutterDrivePresenter.SubarrayDataModelList = exposureSettings.SubarrayList;
                    shutterDrivePresenter.ExposureDataModel = exposureSettings.exposureDataPresenter.ExposureDataModel;
                    shutterDrivePresenter.SubarrayDataModel = exposureSettings.exposureDataPresenter.SubarrayDataModel;
                    //Run CrossTalk Test
                    await ShutterDriveTest(cts.Token);
                    //Plot Red&Blue Data
                    await PlotShutterDriveData(cts);
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 100;
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                    if (cts.IsCancellationRequested)
                    {
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Shutter Drive Test Aborted";
                    }
                    else
                    {
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Shutter Drive Test Completed";
                    }
                    buttonShutterAbort.Enabled = false;
                    buttonShutterRun.Enabled = true;
                    shutterDriveTestStatus = false;
                    shutterDriveExplorerBar.Groups[1].Expanded = true;
                }
                else
                {
                    bool cameraConnected = cidInterface.Connect(ConfigurationManager.AppSettings["CameraIPAddresss"], Int32.Parse(ConfigurationManager.AppSettings["CameraIPPort"]));
                    if (cameraConnected)
                    {
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["CameraStatus"].Text = "Camera Connected";
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["CameraStatus"].Appearance.BackColor = Color.LimeGreen;
                    }
                    else
                    {
                        log.Error("Firmware not running or camera not connected to host");
                        //MessageBox.Show("Firmware not running or camera not connected to host", "Firmware not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                buttonShutterAbort.Enabled = false;
                buttonShutterRun.Enabled = true;
                shutterDriveTestStatus = false;
                shutterDriveExplorerBar.Groups[1].Expanded = true;
                labJackNotFound = true;
                ledPassFail.Value = false;
                ledPassFail.OffColor = Color.Red;
                shutterDrivePresenter.ShutterDriveTestPassed = false;                
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }        
        private async Task ShutterDriveTest(CancellationToken ct)
        {
            try
            {
                log.Debug("Run Red&Blue test.");
                if (ct.IsCancellationRequested)
                    return;
                await shutterDrivePresenter.runShutterDriveTest(cidInterface, ct, userMode, Convert.ToInt32(numericEditNumOfExposures.Value));
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                buttonShutterAbort.Enabled = false;
                buttonShutterRun.Enabled = true;
                shutterDriveTestStatus = false;
                shutterDriveExplorerBar.Groups[1].Expanded = true;
            }
        }
        private async Task PlotShutterDriveData(CancellationTokenSource ct)
        {
            try
            {
               if (ct.IsCancellationRequested)
               {
                   AIO.ClearData();
                   AI1.ClearData();
                   AI2.ClearData();
                   return;
               }
                await InvokeLabJack(ct.Token);
                if (ct.IsCancellationRequested)
                {
                    AIO.ClearData();
                    AI1.ClearData();
                    AI2.ClearData();
                    return;
                }
                if (!labJackNotFound)
                {
                    if (dblValuetemp != null)
                    {
                        AIO.PlotYAppend(dblValuetemp);
                        shutterDrivePresenter.ShutterPlotResult = dblValuetemp;
                    }                      
                    if ((voltageResultList[20] > 4) && (voltageResultList[130] < 1) &&
                        (voltageResultList[180] < 1) && (voltageResultList[280] > 4))
                    {
                        ledPassFail.Value = true;
                        shutterDrivePresenter.ShutterDriveTestPassed = true;
                    }
                    else
                    {
                        ledPassFail.Value = false;
                        ledPassFail.OffColor = Color.Red;
                        shutterDrivePresenter.ShutterDriveTestPassed = false;
                    }                   
                }
                else
                {
                    ledPassFail.Value = false;
                    ledPassFail.OffColor = Color.Red;
                    shutterDrivePresenter.ShutterDriveTestPassed = false;
                }
                buttonShutterRun.Enabled = true;
                buttonShutterAbort.Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                buttonShutterAbort.Enabled = false;
                buttonShutterRun.Enabled = true;
                shutterDriveTestStatus = false;
                ledPassFail.Value = false;
                shutterDriveExplorerBar.Groups[1].Expanded = true;
            }
        }
        private async Task InvokeLabJack(CancellationToken ct)
        {
            try
            {            
               dblValuetemp = new double[300];
               dblA1emp = new double[300];
                double labJackSN = 0;
                await Task.Run(() =>
                {
                    for (int k = 0; k < Convert.ToInt32(numericEditNumOfExposures.Value); k++)
                    {
                        if (ct.IsCancellationRequested)
                        {
                            AIO.ClearData();
                            AI1.ClearData();
                            AI2.ClearData();
                            return;
                        }
                        long i = 0, j = 0;
                        long numIterations = 300; // Should be the same as the Exposure Interval
                        int numChannels = 1;    //Number of AIN channels, 0-3
                        long resolution = 0;    //Configure resolution of the analog inputs (pass a non-zero value for quick sampling). 
                        long settlingTime = 1;  //0=5us, 1=10us, 2=100us, 3=1ms, 4=10ms
                        voltageResultList = new double[numIterations];
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
                            //Request that DAC0 be set to 5 volts.
                            LJUD.AddRequest(u3.ljhandle, LJUD.IO.PUT_DAC, 1, 5.00, 0, 0);
                            LJUD.ePut(u3.ljhandle, LJUD.IO.PUT_DAC, 1, 5.00, 0);
                            LJUD.GoOne(u3.ljhandle);
                            LJUD.eGet(u3.ljhandle, LJUD.IO.GET_CONFIG, LJUD.CHANNEL.SERIAL_NUMBER, ref labJackSN, 0);
                            shutterDrivePresenter.LabJackSN = labJackSN.ToString();
                            //Add analog input requests.
                            for (j = 0; j < numChannels; j++)
                            {
                                LJUD.AddRequest(u3.ljhandle, LJUD.IO.GET_AIN, (LJUD.CHANNEL)j, 0, 0, 0);
                            }
                        }
                        catch (LabJackUDException e)
                        {
                            log.Error("LabJack open error" + MethodBase.GetCurrentMethod(), e);
                            MessageBox.Show("LabJack open error - " + e + " - Shutter Test maybe impacted",
                                    "LabJack Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            labJackNotFound = true;
                            return;
                        }
                        for (i = 0; i < numIterations; i++)
                        {
                            if (ct.IsCancellationRequested)
                            {
                                return;
                            }
                            //Execute the requests.
                            try
                            {
                                if (Convert.ToInt32(i) % 2 == 0 && ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value != 100)
                                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value += 1;
                                if (u3 != null)
                                    LJUD.GoOne(u3.ljhandle);
                                else
                                    return;
                            }
                            catch (LabJackUDException e)
                            {
                                log.Error("LabJack error" + MethodBase.GetCurrentMethod(), e);
                                MessageBox.Show("LabJack error - " + e,
                                    "LabJack LJUD.GoOne Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                labJackNotFound = true;
                                return;
                            }
                            bool finished = false;
                            double AI0 = 0;
                            //Get all the results.  The input measurement results are stored. 
                            LJUD.GetFirstResult(u3.ljhandle, ref ioType, ref channel, ref dblValue, ref dummyInt, ref dummyDouble);
                            while (!finished)
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    AIO.ClearData();
                                    AI1.ClearData();
                                    AI2.ClearData();
                                    return;
                                }
                                switch (ioType)
                                {
                                    case LJUD.IO.GET_AIN:
                                        {
                                            if ((int)channel == 0)
                                            {
                                                 AI0 = dblValuetemp[i] = dblValue;
                                                 voltageResultList[i] = dblValue;     
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
                                    LJUD.GetNextResult(u3.ljhandle, ref ioType, ref channel, ref dblValue, ref dummyInt, ref dummyDouble);                                        
                                }
                                catch (LabJackUDException e)
                                {
                                    if (e.LJUDError == LJUD.LJUDERROR.NO_MORE_DATA_AVAILABLE)
                                        finished = true;
                                    else
                                    {
                                        log.Error("LabJack error" + MethodBase.GetCurrentMethod(), e);
                                        MessageBox.Show("LabJack error -  " + e,
                                            "LabJack LJUD.GetNextResult Error",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        labJackNotFound = true;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    // Reset DAC to 0V
                    LJUD.AddRequest(u3.ljhandle, LJUD.IO.PUT_DAC, 1, 0.00, 0, 0);
                    LJUD.ePut(u3.ljhandle, LJUD.IO.PUT_DAC, 1, 0.00, 0);
                    LJUD.GoOne(u3.ljhandle);
                    labJackNotFound = false;
                });
            }
            catch (Exception ex)
            {               
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                labJackNotFound = true;
            }
        }
        public async Task ClearShutterDriveGraphs()
        {
            await Task.Run(() =>
            {
                AIO.ClearData();
                AI1.ClearData();
                AI2.ClearData();
            });
        }
        private void ShutterDriveTestForm_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                exposureSettings = new ExposureSettings("SDT", userMode, engineeringLoginStatus, TestAppHelper.UserLoginName, "ShutterDriveData.xml", "ShutterDriveExposures");
                ultraExplorerBarContainerControl2.Controls.Add(exposureSettings);
                exposureSettings.GetData();
                shutterDrivePresenter.ExposureDataList = exposureSettings.ExposureList;
                shutterDrivePresenter.SubarrayDataModelList = exposureSettings.SubarrayList;
                shutterDrivePresenter.ExposureDataModel = exposureSettings.exposureDataPresenter.ExposureDataModel;
                shutterDrivePresenter.SubarrayDataModel = exposureSettings.exposureDataPresenter.SubarrayDataModel;
                numericEditNumOfExposures.Value = shutterDrivePresenter.ExposureDataList.Count;
                if (userMode == "Manufacturing")
                {
                    EnableReadOnlyControlsManufacturingMode();
                }
                Cursor.Current = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Loading Shutter Drive Test Form", "Loading Form", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }      
        private void EnableReadOnlyControlsManufacturingMode()
        {
            try
            {
                NumExposuresGroupBox.Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }       
        private async void buttonShutterDriveAbort_Click(object sender, EventArgs e)
        {
            try
            {
                if (cts != null)
                {
                    DialogResult result = MessageBox.Show("Are you sure to Abort Shutter Drive Test?",
                           "Abort Shutter Drive Test",
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        cidInterface.AbortExposure();
                        shutterDriveTestStatus = false;
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.ResetFillAppearance();
                        await ClearShutterDriveGraphs();
                        cts.Cancel();
                        buttonShutterRun.Enabled = true;
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
        private void ResetProgressBar()
        {
            try
            {
                if (((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value == 100)
                {
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = string.Empty;
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.ResetFillAppearance();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraExplorerBar1_Click(object sender, EventArgs e)
        {
            ResetProgressBar();
        }
        private void panel1_Click(object sender, EventArgs e)
        {
            ResetProgressBar();
        }
        private void numericEditNumOfExposures_ValueChanged(object sender, EventArgs e)
        {
            if (numericEditNumOfExposures.Value > shutterDrivePresenter.ExposureDataList.Count)
            {
                MessageBox.Show("Value cannot be greater than total number of exposures.");
                numericEditNumOfExposures.Value = shutterDrivePresenter.ExposureDataList.Count;
            }
        }
    }
}
