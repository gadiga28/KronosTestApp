using KronosCameraTestApp.Model;
using KronosCameraTestApp.Presenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace KronosCameraTestApp.View
{
    public partial class EnvironmentalStatus : Form
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public EnvironmentalStatusPresenter environmentalStatusPresenter { get; set; }
        public EnvironmentalStatusLimitsData environmentalStatusLimitsData { get; set; }
        public KronosTestAppMainForm kronosTestAppMainForm { get; set; }
        private List<string> COMPortList = new List<string>();
        private System.IO.Ports.SerialPort ssCoolingSerialPort = new SerialPort();
        private System.IO.Ports.SerialPort powerSupplySerialPort = new SerialPort();
        private System.IO.Ports.SerialPort flowMeterSerialPort = new SerialPort();
        bool runStatus = true;

        private string purgeFlowRateVal = string.Empty; private string bkPowerCOMPort = string.Empty;
        private string flowMeterCOMPort = string.Empty;
        private string tcubeCOMPort = string.Empty;      
        private string currentAmpsVal = string.Empty;
        private string powerVoltsVal = string.Empty;
        private string faultStatusVal = string.Empty;
        private string tankLevelVal = string.Empty;
        private string pwmCoolingVal = string.Empty;
        private string fanSpeedVal = string.Empty;
        private string pumpTempVal = string.Empty;
        private string setPointTempVal = string.Empty;
        private string coolantTempVal = string.Empty;
        private bool? coolantTempPassed = null;
        private bool? tankLevelPassed = null;
        private bool? powerVoltsPassed = null;
        private bool? currentAmpsPassed = null;
        private bool? purgeFlowRatePassed = null;
        private bool tcubeFaultStatusPassed = false;
        private bool envrionmentalStatusTestFailed = false;
        bool envStatusRunStatus = false;
        private List<double> ampsReadings = new List<double>();
        public string BKPowerCOMPort
        {
            get { return bkPowerCOMPort; }
            set { bkPowerCOMPort = value; }
        }
        public string FlowMeterCOMPort
        {
            get { return flowMeterCOMPort; }
            set { flowMeterCOMPort = value; }
        }
        public string TCubeCOMPort
        {
            get { return tcubeCOMPort; }
            set { tcubeCOMPort = value; }
        }      
        public string CurrentAmpsVal
        {
            get { return currentAmpsVal; }
            set
            {
                currentAmpsVal = value;
                currentAmps.Text = currentAmpsVal;
            }
        }
        public string PowerVoltsVal
        {
            get { return powerVoltsVal; }
            set
            {
                powerVoltsVal = value;
                powerVolts.Text = powerVoltsVal;
            }
        }
        public string FaultStatusVal
        {
            get { return faultStatusVal; }
            set 
            { 
                faultStatusVal = value;
                faultStatus.Text = faultStatusVal;
            }
        }
        public string TankLevelVal
        {
            get { return tankLevelVal; }
            set { tankLevel.Text = tankLevelVal = value; }
        }
        public string PWMCoolingVal
        {
            get { return pwmCoolingVal; }
            set { PWMCooling.Text=pwmCoolingVal = value; }
        }
        public string FanSpeedVal
        {
            get { return fanSpeedVal; }
            set { fanSpeed.Text=fanSpeedVal = value; }
        }
        public string PumpTempVal
        {
            get { return pumpTempVal; }
            set { pumpTemp.Text=pumpTempVal = value; }
        }
        public string SetPointTempVal
        {
            get { return setPointTempVal; }
            set { setPointTemp.Text= setPointTempVal = value; }
        }
        public string CoolantTempVal
        {
            get { return coolantTempVal; }
            set { coolantTemp.Text=coolantTempVal = value; }
        }
        public bool? CoolantTempPassed
        {
            get { return coolantTempPassed; }
            set 
            {
                coolantTempPassed = value;
                if (coolantTempPassed != null)
                {
                    if ((bool)coolantTempPassed)
                    {
                        coolantTemp.Appearance.BorderColor = Color.LimeGreen;
                    }
                    else
                    {
                        coolantTemp.Appearance.BorderColor = Color.Red;
                    }
                }
                else
                {
                    coolantTemp.Appearance.BorderColor = Color.Black;
                }
                   
            }
        }
        public bool? TankLevelPassed
        {
            get { return tankLevelPassed; }
            set
            { 
                tankLevelPassed = value;
                if(tankLevelPassed!=null)
                {
                    if ((bool)tankLevelPassed)
                    {
                        tankLevel.Appearance.BorderColor = Color.LimeGreen;
                    }
                    else
                    {
                        tankLevel.Appearance.BorderColor = Color.Red;
                    }
                }
                else
                {
                    tankLevel.Appearance.BorderColor = Color.Black;
                }               
            }
        }
        public bool? PowerVoltsPassed
        {
            get { return powerVoltsPassed; }
            set
            {
                powerVoltsPassed = value; 
                if((bool)powerVoltsPassed)
                {
                    powerVolts.Appearance.BorderColor = Color.LimeGreen;
                }
                else
                {
                    powerVolts.Appearance.BorderColor = Color.Red;
                }
            }
        }
        public bool? CurrentAmpsPassed
        {
            get { return currentAmpsPassed; }
            set 
            {
                currentAmpsPassed = value;
                if ((bool)currentAmpsPassed)
                {
                    currentAmps.Appearance.BorderColor = Color.LimeGreen;
                }
                else
                {
                    currentAmps.Appearance.BorderColor = Color.Red;
                }
            }
        }
        public bool? PurgeFlowRatePassed
        {
            get { return purgeFlowRatePassed; }
            set 
            { 
                purgeFlowRatePassed = value;
                if ((bool)purgeFlowRatePassed)
                {
                    purgeFlowRate.Appearance.BorderColor = Color.LimeGreen;
                }
                else
                {
                    purgeFlowRate.Appearance.BorderColor = Color.Red;
                }
            }
        }

        public bool TCubeFaultStatusPassed
        {
            get { return tcubeFaultStatusPassed; }
            set
            {
                tcubeFaultStatusPassed = value;
                //if (tcubeFaultStatusPassed)
                //{
                //    faultStatus.Appearance.BorderColor = Color.LimeGreen;
                //}
                //else
                //{
                //    faultStatus.Appearance.BorderColor = Color.Red;
                //}
            }
        }

        public bool EnvrionmentalStatusTestFailed
        {
            get { return envrionmentalStatusTestFailed; }
            set { envrionmentalStatusTestFailed = value; }
        }
        public string PurgeFlowRateVal
        {
            get { return purgeFlowRateVal; }
            set { 
                purgeFlowRateVal = value;
                if(!this.purgeFlowRate.IsDisposed)
                {
                    purgeFlowRate.Text = purgeFlowRateVal;
                }
            }
        }     
       
      
        public EnvironmentalStatus(EnvironmentalStatusPresenter environmentalStatusPresenter, KronosTestAppMainForm kronosTestAppMainForm )
        {
            InitializeComponent();
            this.environmentalStatusPresenter = environmentalStatusPresenter;
            this.kronosTestAppMainForm = kronosTestAppMainForm;

            if (environmentalStatusPresenter == null)
            {
                environmentalStatusPresenter = new EnvironmentalStatusPresenter();
            }
            environmentalStatusPresenter.GetCOMPortConfiguration();
            // getSerialPowerCOMPorts();
            if (environmentalStatusPresenter.COMPortConfigurationModel != null)
            {
                autoConEnvStatCOMPorts.Checked = environmentalStatusPresenter.COMPortConfigurationModel.AutoConnectCOMPorts;

                if(!string.IsNullOrEmpty(environmentalStatusPresenter.COMPortConfigurationModel.TCubeCOMPort))
                {
                    if (!ssCoolingSerialPort.IsOpen)
                    {
                        ssCoolingSerialPort.PortName = environmentalStatusPresenter.COMPortConfigurationModel.TCubeCOMPort;
                    }
                    TCubeCOMPort = TCubePortComboBox.Text = environmentalStatusPresenter.COMPortConfigurationModel.TCubeCOMPort;
                }
                if (!string.IsNullOrEmpty(environmentalStatusPresenter.COMPortConfigurationModel.BKPowerCOMPort))
                {
                    if (!powerSupplySerialPort.IsOpen)
                    {
                        powerSupplySerialPort.PortName = environmentalStatusPresenter.COMPortConfigurationModel.BKPowerCOMPort;
                    }
                    BKPowerCOMPort = BKPowerCOMPortComboBox.Text = environmentalStatusPresenter.COMPortConfigurationModel.BKPowerCOMPort;
                }
                if (!string.IsNullOrEmpty(environmentalStatusPresenter.COMPortConfigurationModel.FlowMeterCOMPort))
                {
                    if (!flowMeterSerialPort.IsOpen)
                    {
                        flowMeterSerialPort.PortName = environmentalStatusPresenter.COMPortConfigurationModel.FlowMeterCOMPort;
                    }
                    FlowMeterCOMPort = FlowMeterPortComboBox.Text = environmentalStatusPresenter.COMPortConfigurationModel.FlowMeterCOMPort;
                }                   
            }
            
            environmentalStatusPresenter.GetEnironmentalStatusLimits();
            if (environmentalStatusPresenter.EnvironmentalStatusLimitsDataList != null &&
                environmentalStatusPresenter.EnvironmentalStatusLimitsDataList.Count() > 0)
            {
                environmentalStatusLimitsData = environmentalStatusPresenter.EnvironmentalStatusLimitsDataList[0];
            }
           // EnvrionmentalStatusTestFailed = false;
        }
        public EnvironmentalStatus()
        {
            InitializeComponent();
            getSerialPowerCOMPorts();
        }       
        private void EnvironmentalStatus_Load(object sender, EventArgs e)
        {
            getSerialPowerCOMPorts();           
           
        }       
        private void getSerialPowerCOMPorts()
        {
            try
            {
               // await Task.Run(() =>
                //{
                    
                    string[] ports = SerialPort.GetPortNames();

                    using (var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort"))
                    {
                        var portdetails = searcher.Get().Cast<ManagementBaseObject>().ToList();
                        COMPortList = (from n in ports
                                       join p in portdetails on n equals p["DeviceID"].ToString()
                                       select n + " - " + p["Caption"]).ToList();
                       
                    }

                    foreach (string ComPort in COMPortList)
                    {
                        if (ComPort.Contains("Silicon Labs"))
                        {
                        BKPowerCOMPortComboBox.Items.Add(ComPort.Substring(0, 4));
                        BKPowerCOMPortComboBox.Text = ComPort.Substring(0, 4);

                           //BeginInvoke(new System.Action(() => BKPowerCOMPortComboBox.Items.Add(ComPort.Substring(0, 4))));
                           // BeginInvoke(new System.Action(() => BKPowerCOMPortComboBox.Text = ComPort.Substring(0, 4)));
                           
                        }
                    }
                    foreach(string str in ports)
                    {

                    TCubePortComboBox.Items.Add(str);
                    FlowMeterPortComboBox.Items.Add(str);                   
                    }
                   
                //});
            }
            catch (Exception ex)
            {

            }
        }
        public async void btnConnect_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
           // EnvrionmentalStatusTestFailed = false;
            if(btnConnect.Text=="Connect")
            {
                btnConnect.Text = "Disconnect";
               
                if(environmentalStatusPresenter.COMPortConfigurationModel == null)
                {
                    FlowMeterCOMPort = FlowMeterPortComboBox.Text;
                    TCubeCOMPort = TCubePortComboBox.Text;
                    BKPowerCOMPort = BKPowerCOMPortComboBox.Text;
                }
                else
                {
                    if (!string.IsNullOrEmpty(environmentalStatusPresenter.COMPortConfigurationModel.FlowMeterCOMPort))
                    {
                        FlowMeterCOMPort = environmentalStatusPresenter.COMPortConfigurationModel.FlowMeterCOMPort;
                    }
                    if (!string.IsNullOrEmpty(environmentalStatusPresenter.COMPortConfigurationModel.TCubeCOMPort))
                    {
                        TCubeCOMPort = environmentalStatusPresenter.COMPortConfigurationModel.TCubeCOMPort;
                    }
                    if (!string.IsNullOrEmpty(environmentalStatusPresenter.COMPortConfigurationModel.BKPowerCOMPort))
                    {
                        BKPowerCOMPort = environmentalStatusPresenter.COMPortConfigurationModel.BKPowerCOMPort;
                    }                  
                   
                }               
                this.kronosTestAppMainForm.ConnectToEnvironmentalStatusCOMPorts(true);
                await this.kronosTestAppMainForm.RunEnvStatTest(true);
            }
            else
            {
                btnConnect.Text = "Connect";
                Disconnect();
            }           

            Cursor.Current = Cursors.Default;
        }

        private void Disconnect()
        {
            runStatus = false;           
            this.kronosTestAppMainForm.DisconnectEnvComPorts();
            coolantTemp.Text = setPointTemp.Text = pumpTemp.Text =
                PWMCooling.Text = fanSpeed.Text = tankLevel.Text = faultStatus.Text = 
                powerVolts.Text=currentAmps.Text= purgeFlowRate.Text=string.Empty;

            coolantTemp.Appearance.BorderColor = setPointTemp.Appearance.BorderColor =
                pumpTemp.Appearance.BorderColor = fanSpeed.Appearance.BorderColor =
                tankLevel.Appearance.BorderColor = faultStatus.Appearance.BorderColor =
                powerVolts.Appearance.BorderColor = currentAmps.Appearance.BorderColor =
                purgeFlowRate.Appearance.BorderColor = Color.Black;

            coolantTemp.Appearance.BorderColor2 = setPointTemp.Appearance.BorderColor2 =
               pumpTemp.Appearance.BorderColor2 = fanSpeed.Appearance.BorderColor2 =
               tankLevel.Appearance.BorderColor2 = faultStatus.Appearance.BorderColor2 =
               powerVolts.Appearance.BorderColor2 = currentAmps.Appearance.BorderColor2 =
               purgeFlowRate.Appearance.BorderColor2 = Color.Black;
        }
      
       
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            runStatus = false;
            ssCoolingSerialPort.Close();
            powerSupplySerialPort.Close();
            flowMeterSerialPort.Close();        
            this.Close();
        }
        private void TCubePortComboBox_SelectionChanged(object sender, EventArgs e)
        {
            if(!ssCoolingSerialPort.IsOpen && TCubePortComboBox.SelectedItem !=null)
            {
                ssCoolingSerialPort.PortName = TCubePortComboBox.SelectedItem.ToString();
            }
           
        }
        private void BKPowerCOMPortComboBox_SelectionChanged(object sender, EventArgs e)
        {
            if (!powerSupplySerialPort.IsOpen && BKPowerCOMPortComboBox.SelectedItem != null)
            {
                powerSupplySerialPort.PortName = BKPowerCOMPortComboBox.SelectedItem.ToString();
            }
           
        }
        private void FlowMeterPortComboBox_SelectionChanged(object sender, EventArgs e)
        {
            if(!flowMeterSerialPort.IsOpen && FlowMeterPortComboBox.SelectedItem != null)
            {
                flowMeterSerialPort.PortName = FlowMeterPortComboBox.SelectedItem.ToString();
            }
            
        }
        private async void EnvironmentalStatus_FormClosed(object sender, FormClosedEventArgs e)
        {   
            if(string.IsNullOrEmpty(TCubePortComboBox.Text) || string.IsNullOrEmpty(BKPowerCOMPortComboBox.Text)||
                string.IsNullOrEmpty(FlowMeterPortComboBox.Text))
            {
                return;
            }

            if (environmentalStatusPresenter.COMPortConfigurationModel !=null)
            {
                if (environmentalStatusPresenter.COMPortConfigurationModel.TCubeCOMPort != TCubePortComboBox.Text ||
                   environmentalStatusPresenter.COMPortConfigurationModel.BKPowerCOMPort != BKPowerCOMPortComboBox.Text ||
                   environmentalStatusPresenter.COMPortConfigurationModel.FlowMeterCOMPort != FlowMeterPortComboBox.Text ||
                   environmentalStatusPresenter.COMPortConfigurationModel.AutoConnectCOMPorts != autoConEnvStatCOMPorts.Checked)
                {
                    environmentalStatusPresenter.COMPortConfigurationModel.StationName = Environment.MachineName;
                    environmentalStatusPresenter.COMPortConfigurationModel.UserDefined = Environment.UserName;
                    environmentalStatusPresenter.COMPortConfigurationModel.DateDefined = DateTime.Now;
                    environmentalStatusPresenter.COMPortConfigurationModel.TCubeCOMPort = TCubePortComboBox.Text;
                    environmentalStatusPresenter.COMPortConfigurationModel.BKPowerCOMPort = BKPowerCOMPortComboBox.Text;
                    environmentalStatusPresenter.COMPortConfigurationModel.FlowMeterCOMPort = FlowMeterPortComboBox.Text;
                    environmentalStatusPresenter.COMPortConfigurationModel.AutoConnectCOMPorts = autoConEnvStatCOMPorts.Checked;
                    await environmentalStatusPresenter.SaveCOMPortConfigurationToDB(environmentalStatusPresenter.COMPortConfigurationModel, false);
                }
               
            }
            else
            {
                COMPortConfigurationModel comPortConfigurationModel = new COMPortConfigurationModel();
                comPortConfigurationModel.StationName = Environment.MachineName;
                comPortConfigurationModel.UserDefined = Environment.UserName;
                comPortConfigurationModel.DateDefined = DateTime.Now;
                comPortConfigurationModel.TCubeCOMPort = TCubePortComboBox.Text;
                comPortConfigurationModel.BKPowerCOMPort = BKPowerCOMPortComboBox.Text;
                comPortConfigurationModel.FlowMeterCOMPort = FlowMeterPortComboBox.Text;
                comPortConfigurationModel.AutoConnectCOMPorts = autoConEnvStatCOMPorts.Checked;
                await environmentalStatusPresenter.SaveCOMPortConfigurationToDB(comPortConfigurationModel,true);
            }
            
        }
    }
}
