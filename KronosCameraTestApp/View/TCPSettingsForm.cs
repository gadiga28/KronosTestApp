using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thermo.Kronos.Instrument.Camera.Interface;
namespace KronosCameraTestApp.View
{
    public partial class TCPSettingsForm : Form
    {
        private CIDInterface cidInterface;
        public string ipAddress;
        public int port;
        private int firmwareId = 0;
        bool cameraConnectedFromTCPDLG = false;
        string userMode = string.Empty;
        string enggUserName = string.Empty;
        public bool CameraConnectedFromTCPDLG
        {
            get { return cameraConnectedFromTCPDLG; }
            set { cameraConnectedFromTCPDLG = value; }
        }
        private String connectionStatus = "Camera Disconnected";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
         System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        InstrumentDefaults instrumentDefaults { get; set; }
        public string getConnectionStatus()
        {
            return connectionStatus;
        }
        public TCPSettingsForm(CIDInterface cidInterface, string userMode, string enggUserName)
        {
            InitializeComponent();
            this.cidInterface = cidInterface;
            this.userMode = userMode;
            this.enggUserName = enggUserName;
            instrumentDefaults = new InstrumentDefaults(cidInterface, userMode, enggUserName);
        }
        private void ultraTextEditor2_ValueChanged(object sender, EventArgs e)
        {
        }
        private async void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                cameraConnectedFromTCPDLG = false;
                if ( (!cidInterface.IsConnected()))
                {
                    var task = new Task(delegate()
                    {
                        port = Convert.ToInt32(ultraTextEditorPort.Text);
                        ipAddress = ultraTextEditorIPAddress.Text;
                        cameraConnectedFromTCPDLG = cidInterface.Connect(ipAddress, port);
                    });
                    task.Start();
                    await task;
                    if (!cameraConnectedFromTCPDLG)
                    {
                        MessageBox.Show(
                            "Firmware not running or camera not connected to host",
                            "Firmware not found",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Information);
                        log.Info("Firmware not running or camera not connected to host");
                        this.Close();
                        return;
                    }
                    else
                    {                       
                        if (instrumentDefaults.EnableOnConnect)
                            // Set the voltages automatically after each connection 
                            instrumentDefaults.ConfigStructForInstrumentDefaults();
                    }
                    buttonConnect.Text = "Disconnect";
                    connectionStatus = "Camera Connected";//, FW Ver. " + firmwareId.ToString();
                    log.Info("Firmware " + firmwareId.ToString()+ " connected to host");
                    Properties.Settings.Default.CameraIPAddress = ultraTextEditorIPAddress.Text;
                    Properties.Settings.Default.CameraPortNumber = ultraTextEditorPort.Text;
                }
                else if (cidInterface.IsConnected())
                {
                    var task = new Task(delegate()
                    {
                        cidInterface.Disconnect();
                    });
                    task.Start();
                    await task;
                    buttonConnect.Text = "Connect";
                    connectionStatus = "Camera Disconnected";
                    log.Info("Camera disconnected");
                }
                this.Text = connectionStatus;
                // close this dialog
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public bool ConnectToCamera(string IPAddress, int PortNumber)
        {   
            try
            {
               return cidInterface.Connect(IPAddress, PortNumber);                   
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;                
            }
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void TCPSettingsForm_Load(object sender, EventArgs e)
        {
            ultraTextEditorPort.Text = ConfigurationManager.AppSettings["CameraIPPort"];
            ultraTextEditorIPAddress.Text = ConfigurationManager.AppSettings["CameraIPAddresss"];
            ipAddress = ultraTextEditorIPAddress.Text;
            port =Convert.ToInt32( ultraTextEditorPort.Text);
            if (cidInterface.IsConnected())
            {
                buttonConnect.Text = "Disconnect";
            }
            else
            {
                buttonConnect.Text = "Connect";
            }
        }
        private void TCPSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Properties.Settings.Default.CameraIPAddress = ultraTextEditorIPAddress.Text;
                Properties.Settings.Default.CameraPortNumber = ultraTextEditorPort.Text;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
