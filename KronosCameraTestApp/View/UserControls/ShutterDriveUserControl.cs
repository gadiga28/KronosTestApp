using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KronosCameraTestApp.Presenter;
using Thermo.Kronos.Instrument.Camera.Interface;
using System.Threading;
using System.Reflection;
using LabJack.LabJackUD;
using System.Configuration;
using System.Security.AccessControl;
using KronosCameraTestApp.Helpers;
using Infragistics.Win.Misc;
namespace KronosCameraTestApp.View.UserControls
{
    public partial class ShutterDriveUserControl : UserControl
    {
        public ShutterDrivePresenter shutterDrivePresenter { get; set; }
        string defaultFilePath = string.Empty;
        private int currentRecord = 0;
        string TestResultImageFilePath = string.Empty;
        private U3 u3;
        private LJUD.IO ioType = 0;
        private LJUD.CHANNEL channel = 0;
        private double dblValue = 0;
        private double ValueDIPort = 0;
        private int dummyInt = 0;
        private double dummyDouble = 0;
        private double[] ValueDIN = new double[16];
        string userMode = string.Empty;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
          System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        double[] dblValuetemp;
        bool labJackNotFound = false;
        double[] voltageResultList;
        AutoTestExposureSettings autoTestExposureSettings { get; set; }
        public  double[] DBLValuetemp
        {
            get { return dblValuetemp; }
            set { dblValuetemp = value; }
        }
        double[] dblA1temp;
        public double[] DBLA1temp
        {
            get { return dblA1temp; }
            set { dblA1temp = value; }
        }
        private CIDInterface cidInterface = null;        
        public CIDInterface CidInterface
        {
            get { return cidInterface; }
            set { cidInterface = value; }
        }
        public ShutterDriveUserControl()
        {
            InitializeComponent();
        }
        public ShutterDriveUserControl(ShutterDrivePresenter shutterDrivePresenter, string defaultFilePath, string UserMode)
        {
            InitializeComponent();
            this.shutterDrivePresenter = shutterDrivePresenter;
            this.userMode = UserMode;
            this.defaultFilePath = defaultFilePath;
        }
        private async void ShutterDriveTestUserControl_Load(object sender, EventArgs e)
        {
            try
            {
                if (autoTestExposureSettings == null)
                {
                    autoTestExposureSettings = new AutoTestExposureSettings("SDT", cidInterface);
                    autoTestExposureSettings.GetData();
                    shutterDrivePresenter.ExposureDataList = autoTestExposureSettings.ExposureList;
                    shutterDrivePresenter.SubarrayDataModelList = autoTestExposureSettings.SubarrayList;
                    shutterDrivePresenter.ExposureDataModel = autoTestExposureSettings.exposureDataPresenter.ExposureDataModel;
                    shutterDrivePresenter.SubarrayDataModel = autoTestExposureSettings.exposureDataPresenter.SubarrayDataModel;
                    ultraExplorerBarContainerControl2.Controls.Add(autoTestExposureSettings);
                }
                if (shutterDrivePresenter.ExposureDataList.Count == 0 || TestAppHelper.UserModeChanged)
                {
                    await shutterDrivePresenter.GetShutterDriveData(cidInterface);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Loading Exposure Data", "Load Exposure Data", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }  
        public async Task RunAutoShutterDriveTest(CancellationTokenSource cts, bool AutoPretest)
        {
            try
            {
                if (cts.IsCancellationRequested)
                    return;
                try
                {
                        long resolution = 0; 
                        u3 = new U3(LJUD.CONNECTION.USB, "0", true); // Connection through USB
                        //Configure resolution. See section 2.6/3.1 of the User's Guide.
                        LJUD.ePut(u3.ljhandle, LJUD.IO.PUT_CONFIG, LJUD.CHANNEL.AIN_RESOLUTION, resolution, 0);
                }
                catch (LabJackUDException ex)
                {
                    log.Error("LabJack open error" + MethodBase.GetCurrentMethod(), ex);
                    labJackNotFound = true;
                    ledPassFail.Value = false;
                    ledPassFail.OffColor = Color.Red;
                    shutterDrivePresenter.ShutterDriveTestPassed = false;
                    ((KronosTestAppMainForm)ParentForm.MdiParent).mainFormDockManager.DockAreas[1].Closed = false;
                    ((KronosTestAppMainForm)ParentForm.MdiParent).mainFormDockManager.DockAreas[1].Activate();
                    ((KronosTestAppMainForm)ParentForm.MdiParent).mainFormDockManager.DockAreas[1].Pin();
                    ((KronosTestAppMainForm)ParentForm.MdiParent).firmwareLoggingTextBox.AppendText(Environment.NewLine);
                    ((KronosTestAppMainForm)ParentForm.MdiParent).firmwareLoggingTextBox.AppendText("LabJack open error Shutter Drive test may be impacted.");
                    ((KronosTestAppMainForm)ParentForm.MdiParent).firmwareLoggingTextBox.AppendText(Environment.NewLine);
                    MessageBox.Show("LabJack open error Shutter Drive test may be impacted.", "LabJack Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                ledPassFail.Value = false;
                if (autoTestExposureSettings == null)
                {
                    autoTestExposureSettings = new AutoTestExposureSettings("SDT", cidInterface);
                    autoTestExposureSettings.GetData();
                    shutterDrivePresenter.ExposureDataList = autoTestExposureSettings.ExposureList;
                    shutterDrivePresenter.SubarrayDataModelList = autoTestExposureSettings.SubarrayList;
                    shutterDrivePresenter.ExposureDataModel = autoTestExposureSettings.exposureDataPresenter.ExposureDataModel;
                    shutterDrivePresenter.SubarrayDataModel = autoTestExposureSettings.exposureDataPresenter.SubarrayDataModel;
                    ultraExplorerBarContainerControl2.Controls.Add(autoTestExposureSettings);
                }
                if (shutterDrivePresenter.ExposureDataList.Count == 0)
                {
                    await shutterDrivePresenter.GetShutterDriveData(cidInterface);
                }
                await RunShutterDriveTest(cts.Token);
                await PlotShutterDriveData(cts);             
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                labJackNotFound = true;
                ledPassFail.Value = false;
                ledPassFail.OffColor = Color.Red;
                shutterDrivePresenter.ShutterDriveTestPassed = false;
                ((KronosTestAppMainForm)ParentForm.MdiParent).mainFormDockManager.DockAreas[1].Closed = false;
                ((KronosTestAppMainForm)ParentForm.MdiParent).mainFormDockManager.DockAreas[1].Activate();
                ((KronosTestAppMainForm)ParentForm.MdiParent).mainFormDockManager.DockAreas[1].Pin();
                ((KronosTestAppMainForm)ParentForm.MdiParent).firmwareLoggingTextBox.AppendText("LabJack open error Shutter Test maybe impacted.");               
                MessageBox.Show("LabJack open error" + " - Shutter Test maybe impacted",
                              "LabJack Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return;
            }
        }
        private async Task RunShutterDriveTest(CancellationToken ct)
        {
            try
            {
                if (ct.IsCancellationRequested)
                    return;
                log.Debug("Run Shutter Drive test.");
                await shutterDrivePresenter.runShutterDriveTest(cidInterface, ct, userMode);
                ledPassFail.Value = false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
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
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);   
                shutterDriveUCUltraExplorerBar.Groups[1].Expanded = true;
            }
        }
        private async Task  InvokeLabJack(CancellationToken ct)
        {
            try
            {
                dblValuetemp = new double[300];
                dblA1temp = new double[300];
                double labJackSN=0;
                await Task.Run(() =>
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
                            if (u3 == null)
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
                        LJUD.eGet(u3.ljhandle, LJUD.IO.GET_CONFIG, LJUD.CHANNEL.SERIAL_NUMBER, ref labJackSN, 0);
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
                                AIO.ClearData();
                                AI1.ClearData();
                                AI2.ClearData();
                                return;
                            }
                            //Execute the requests.
                            try
                            {
                                if (u3 != null)
                                    LJUD.GoOne(u3.ljhandle);
                                else
                                    return;
                            }
                            catch (LabJackUDException e)
                            {
                                log.Error("LabJack error" + MethodBase.GetCurrentMethod(), e);
                                MessageBox.Show("LabJack error - " + e,
                                    "LabJack LJUD.GoOne error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                                        MessageBox.Show("LabJack error - " + e,
                                           "LabJack LJUD.GetNextResult error",
                                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        labJackNotFound = true;
                                        return;
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
                ledPassFail.Value = false;
                ledPassFail.OffColor = Color.Red;
                shutterDrivePresenter.ShutterDriveTestPassed = false;
                MessageBox.Show("LabJack open error - " + ex + " - Shutter Test maybe impacted",
                                "LabJack Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        public void SaveTestResultImage(string testResultLocalFilePath, string testResultsServerPath, string localFileTimeStamp, string serverPathToDB, bool savetoNetwork)
        {
            try
            {
                this.Dock = DockStyle.None;
                Image img = null;
                int outputImageWidth = 0;
                int outputImageHeight = 0;
                Bitmap outputImage = null;               
                img = this.shutterWaveformGraph.ToImage();
                outputImageWidth = img.Width;
                outputImageHeight = img.Height;
                outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics graphics = Graphics.FromImage(outputImage))
                {
                    graphics.DrawImage(img, new Rectangle(new Point(), img.Size),
                           new Rectangle(new Point(), img.Size), GraphicsUnit.Pixel);
                }
                outputImage.Save(testResultLocalFilePath + "\\ShutterDriveTestResultImage_" + localFileTimeStamp + ".png", System.Drawing.Imaging.ImageFormat.Png);
                this.Dock = DockStyle.Fill;
                if (ConfigurationManager.AppSettings["EndClient"] != "Thermo")
                {
                    TestAppHelper.saveFileInAzure(outputImage, "ShutterDrive.png");
                }
                else
                {
                    if (savetoNetwork)
                    {
                        outputImage.Save(testResultsServerPath + "\\ShutterDriveTestResultImage_" + localFileTimeStamp + ".png", System.Drawing.Imaging.ImageFormat.Png);
                        shutterDrivePresenter.TestResultImagePath = serverPathToDB + "\\ShutterDriveTestResultImage_" + localFileTimeStamp + ".png";
                    }
                    else
                        shutterDrivePresenter.TestResultImagePath = string.Empty;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
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
    }
}
