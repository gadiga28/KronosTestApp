using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thermo.Kronos.Instrument.Camera.Contracts.Data;
namespace KronosCameraTestApp.View
{
    public partial class AboutForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
         System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CameraInformation cameraInformation;       
        public AboutForm(CameraInformation cameraInformation)
        {
            InitializeComponent();
            this.cameraInformation = cameraInformation;            
        }
        private void AboutForm_Load(object sender, EventArgs e)
        {
            ParametersModel parametersModel = TestAppHelper.GetParametersModel();
            cameraModel.Text = cameraInformation.CameraVersion.ToString();
            cameraType.Text = cameraInformation.CameraType.ToString();
            firmwareVersion.Text = cameraInformation.FirmwareRevision.ToString();
            SCM5821ASoftwareVersion.Text += System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString("yyMMdd");//ConfigurationManager.AppSettings["SCMSoftwareVersion"];

            //string executablePath = Process.GetCurrentProcess().MainModule.FileName;
            //FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(executablePath);
            //string executableVersion = versionInfo.FileVersion;
            //SCM5821ASoftwareVersion.Text += parametersModel.SoftwareVersion;
            APIVersion.Text = cameraInformation.CameraApiVersion.ToString();// ConfigurationManager.AppSettings["APIVersion"];            
            SCM5821ADatabaseVersion.Text = parametersModel.DatabaseVersion;
        }
        private void ultraGroupBox1_Click(object sender, EventArgs e)
        {
        }       
    }
}
