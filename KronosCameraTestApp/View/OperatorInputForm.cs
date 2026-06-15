using KronosCameraTestApp.Model;
using KronosCameraTestApp.Presenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thermo.Kronos.Instrument.Camera.Interface;
using Thermo.Kronos.Instrument.Camera.Contracts.Data;
using System.Data.SqlClient;
namespace KronosCameraTestApp.View
{
    public partial class OperatorInputForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CameraDetailsModel cameraDetailsModel { get; set; }
        public List<CameraDetailsModel> cameraDetailsModelList { get; set; }
        public CameraDetailsPresenter cameraDetailsPresenter { get; set; }
        CIDInterface cidInterface { get; set; }
        private CameraInformation cameraInformation { get; set; }
        public string CameraSerialNumber = string.Empty;
        public string ImagerSerialNumber = string.Empty;
        string CSPNumber = string.Empty;
        string PowerNumber = string.Empty;
        string CPUNumber = string.Empty;
        bool inputValidationPass = true;
        bool operatorInputFormFromMenu = false;
        bool QRCodeScanDone = false;
        List<string> imagerSNFromViewCleanroomHeadComponentScan { get; set; }
        public string craNumber { get; set; }
        public OperatorInputForm(CameraDetailsPresenter cameraDetailsPresenter)
        {
            InitializeComponent();
            this.cameraDetailsPresenter = cameraDetailsPresenter;
        }
        public OperatorInputForm()
        {
            InitializeComponent();
            this.cameraDetailsPresenter = cameraDetailsPresenter;
        }
        public OperatorInputForm(CIDInterface cidInterface, CameraDetailsPresenter cameraDetailsPresenter,
            bool fromMenu, CameraInformation cameraInfo)
        {
            InitializeComponent();
            this.cameraDetailsPresenter = cameraDetailsPresenter;
            this.cidInterface = cidInterface;
            this.operatorInputFormFromMenu = fromMenu;
            this.cameraInformation = cameraInfo;
        }
        private void ReceivedCameraInformation(CameraInformation cameraInfo)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate ()
                    {
                        this.ReceivedCameraInformation(cameraInfo);
                    });
                    cameraInformation = cameraInfo;
                    return;
                }
                cameraInformation = cameraInfo;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public CameraDetailsModel UpdateCameraDetailsModelWithView()
        {
            try
            {
                cameraDetailsPresenter.CameraDetailsModel = new CameraDetailsModel();
                cameraDetailsPresenter.CameraDetailsModel.DateDefined = DateTime.Now;
                cameraDetailsPresenter.CameraDetailsModel.UserExecuted = Environment.UserName.ToUpper();
                cameraDetailsPresenter.CameraDetailsModel.CameraModelNumber = textBoxCameraModel.Text.Trim();
                cameraDetailsPresenter.CameraDetailsModel.CameraSerialNumber = textBoxCameraSN.Text.Trim();
                cameraDetailsPresenter.CameraDetailsModel.ImagerSerialNumber = textBoxImagerSN.Text.Trim();
                cameraDetailsPresenter.CameraDetailsModel.DetectorType = ultraComboEditorDetectorType.SelectedItem.DisplayText;
                cameraDetailsPresenter.CameraDetailsModel.MACAddress = string.Empty;
                cameraDetailsPresenter.CameraDetailsModel.TestSoftwareVer = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString("yyMMdd");
                cameraDetailsPresenter.CameraDetailsModel.FirmwareVer = cameraInformation != null ? cameraInformation.FirmwareRevision.ToString() : "test";
                cameraDetailsPresenter.CameraDetailsModel.FirmwareFile = string.Empty;
                cameraDetailsPresenter.CameraDetailsModel.CRA = textBoxCRANumber.Text.Trim();
                cameraDetailsPresenter.CameraDetailsModel.ProductShippedDate = null;
                cameraDetailsPresenter.CameraDetailsModel.CPUSerialNumber = textBoxCameraControlCPUSN.Text.Trim();
                cameraDetailsPresenter.CameraDetailsModel.PowerSerialNumber = textBoxCameraControlSerialNumPower.Text.Trim();
                cameraDetailsPresenter.CameraDetailsModel.CSPSerialNumber = textBoxCameraControlSerialNumCSP.Text.Trim();
                cameraDetailsPresenter.CameraDetailsModel.ISISerialNumber = textBoxISISN.Text.Trim();
                cameraDetailsPresenter.CameraDetailsModel.FPGAVersion = string.Empty;
                CameraSerialNumber = textBoxCameraSN.Text;
                ImagerSerialNumber = textBoxImagerSN.Text;
                return cameraDetailsPresenter.CameraDetailsModel;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (!operatorInputFormFromMenu)
                {
                    if (!CheckForEmptyValues())
                    {
                        MessageBox.Show("Either Camera/Imager/ISI/CPU/Power/CSP serial number is missing. Please check if all the inputs are entered and valid.",
                            "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        inputValidationPass = false;
                        this.DialogResult = DialogResult.Cancel;
                    }
                    else if (!inputValidationPass)
                    {
                        inputValidationPass = false;
                        this.DialogResult = DialogResult.Cancel;
                    }
                    else
                    {
                        SetCameraSerialNumbers();
                        inputValidationPass = true;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    SetCameraSerialNumbers();
                    inputValidationPass = true;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                inputValidationPass = false;
            }
        }
        private void SetCameraSerialNumbers()
        {
            try
            {
                CameraSerialNumbers serialNumbers = new CameraSerialNumbers();
                serialNumbers.CPUSerialNumber = new char[CameraSerialNumbers.SERIAL_NUMBER_LENGTH];
                serialNumbers.CPUSerialNumber = textBoxCameraControlCPUSN.Text.PadRight(CameraSerialNumbers.SERIAL_NUMBER_LENGTH, ' ').ToCharArray();
                serialNumbers.CSPSerialNumber = new char[CameraSerialNumbers.SERIAL_NUMBER_LENGTH];
                serialNumbers.CSPSerialNumber = textBoxCameraControlSerialNumCSP.Text.PadRight(CameraSerialNumbers.SERIAL_NUMBER_LENGTH, ' ').ToCharArray();
                serialNumbers.PWRBoardSerialNumber = new char[CameraSerialNumbers.SERIAL_NUMBER_LENGTH];
                serialNumbers.PWRBoardSerialNumber = textBoxCameraControlSerialNumPower.Text.PadRight(CameraSerialNumbers.SERIAL_NUMBER_LENGTH, ' ').ToCharArray();
                serialNumbers.ImagerSerialNumber = new char[CameraSerialNumbers.SERIAL_NUMBER_LENGTH];
                serialNumbers.ImagerSerialNumber = textBoxImagerSN.Text.PadRight(CameraSerialNumbers.SERIAL_NUMBER_LENGTH, ' ').ToCharArray();
                serialNumbers.CameraSerialNumber = new char[CameraSerialNumbers.SERIAL_NUMBER_LENGTH];
                serialNumbers.CameraSerialNumber = textBoxCameraSN.Text.PadRight(CameraSerialNumbers.SERIAL_NUMBER_LENGTH, ' ').ToCharArray();
                serialNumbers.ISISerialNumber = new char[CameraSerialNumbers.SERIAL_NUMBER_LENGTH];
                serialNumbers.ISISerialNumber = textBoxISISN.Text.PadRight(CameraSerialNumbers.SERIAL_NUMBER_LENGTH, ' ').ToCharArray();
                cidInterface.SendSerialNumbers(serialNumbers);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private bool CheckForEmptyValues()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBoxCameraSN.Text) || string.IsNullOrWhiteSpace(textBoxImagerSN.Text) || string.IsNullOrWhiteSpace(textBoxCameraControlCPUSN.Text)
                    || string.IsNullOrWhiteSpace(textBoxCameraControlSerialNumPower.Text) || string.IsNullOrWhiteSpace(textBoxCameraControlSerialNumCSP.Text)
                    || string.IsNullOrWhiteSpace(textBoxISISN.Text))
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                inputValidationPass = false;
                return false;
            }
        }
        private async void OperatorInputForm_Load(object sender, EventArgs e)
        {
            textBoxCameraSN.Focus();
            inputValidationPass = true;
            cidInterface.ReceivedCameraInformation = ReceivedCameraInformation;
            imagerSNFromViewCleanroomHeadComponentScan = new List<string>();
            updateSerialNumbers();
            cameraDetailsModelList = await cameraDetailsPresenter.GetCameraDetailsModel(textBoxCameraSN.Text.Trim());
            if (cameraDetailsModelList.Count() > 0)
            {
                cameraDetailsModel = cameraDetailsModelList.Where(cam => cam.CRA != null && cam.CRA != "").FirstOrDefault();
                if (cameraDetailsModel != null)
                {
                    craNumber = textBoxCRANumber.Text = cameraDetailsModel.CRA;
                }
            }

        }
        private void updateSerialNumbers()
        {
            try
            {
                cidInterface.GetCameraInformation();
                string serialNumbers = string.Empty;
                if (cameraInformation.SerialNumbers.ImagerSerialNumber != null)
                {
                    serialNumbers = new string(cameraInformation.SerialNumbers.ImagerSerialNumber);
                    serialNumbers = serialNumbers.TrimEnd('\0');
                    if (!(string.IsNullOrWhiteSpace(serialNumbers)) || !(serialNumbers.Equals("Unknown")))
                    {
                        textBoxCameraSN.Text = new string(cameraInformation.SerialNumbers.CameraSerialNumber).Trim();
                        textBoxCameraControlCPUSN.Text = new string(cameraInformation.SerialNumbers.CPUSerialNumber).Trim();
                        textBoxCameraControlSerialNumCSP.Text = new string(cameraInformation.SerialNumbers.CSPSerialNumber).Trim();
                        textBoxCameraControlSerialNumPower.Text = new string(cameraInformation.SerialNumbers.PWRBoardSerialNumber).Trim();
                        textBoxImagerSN.Text = new string(cameraInformation.SerialNumbers.ImagerSerialNumber).Trim();
                        textBoxISISN.Text = new string(cameraInformation.SerialNumbers.ISISerialNumber).Trim();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void textBoxCameraControlCPU_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                //if (!QRCodeScanDone)
                //{
                    //GetCPUPowerCSPFromQRScan(textBoxCameraControlCPUSN.Text);
                    //inputValidationPass = true;
                    //buttonOK.Focus();
               // }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public bool ValidateCPU(string CPUinput)
        {
            try
            {
                var regexItem = new Regex("^[0-9]*$");
                if (CPUinput.Length == 0 || CPUinput == "" || (string.IsNullOrEmpty(CPUinput)) ||
                    CPUinput.Length != 14 || !regexItem.IsMatch(CPUinput))
                {
                    inputValidationPass = false;
                    return false;
                }
                else
                    inputValidationPass = true;
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                inputValidationPass = false;
                return false;
            }
        }
        private void textBoxCameraControlSerialNumTSP_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                //if (!QRCodeScanDone)
                //{
                //    GetCPUPowerCSPFromQRScan(textBoxCameraControlSerialNumPower.Text);
                //    inputValidationPass = true;
                //    buttonOK.Focus();
                //}
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public bool ValidatePower(string PowerInput)
        {
            try
            {
                var regexItem = new Regex("^[a-zA-Z0-9-]*$");
                var regexItem1 = new Regex("^[0-9]*$");
                var regexItem2 = new Regex("^[a-zA-Z]*$");
                var regexItem3 = new Regex("^[-]*$");
                if (PowerInput.Length != 10 || PowerInput.Length == 0 || PowerInput == "" ||
                    (string.IsNullOrEmpty(PowerInput)) ||
                    PowerInput.Substring(4, 1) != "-" ||
                    !regexItem.IsMatch(PowerInput) ||
                    !regexItem1.IsMatch(PowerInput.Substring(0, 4)) ||
                    !regexItem3.IsMatch(PowerInput.Substring(4, 1)))
                {
                    inputValidationPass = false;
                    return false;
                }
                else
                {
                    inputValidationPass = true;
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                inputValidationPass = false;
                return false;
            }
        }
        private void textBoxCameraControlSerialNumASP_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                // if(!QRCodeScanDone)
                //{
                //    GetCPUPowerCSPFromQRScan(textBoxCameraControlSerialNumCSP.Text);
                //    inputValidationPass = true;
                //    buttonOK.Focus();
                //}

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public bool ValidateCSP(string CSPInput)
        {
            try
            {
                var regexItem = new Regex("^[a-zA-Z0-9-]*$");
                var regexItem1 = new Regex("^[0-9]*$");
                var regexItem2 = new Regex("^[a-zA-Z]*$");
                var regexItem3 = new Regex("^[-]*$");
                var regexItem4 = new Regex("^[ ]*$");
                if (CSPInput.Length < 15 || CSPInput.Length == 0 || CSPInput == "" ||
                    (string.IsNullOrEmpty(CSPInput)) ||
                    CSPInput.Substring(4, 1) != "-" ||
                    !regexItem1.IsMatch(CSPInput.Substring(0, 4)) ||
                    !regexItem2.IsMatch(CSPInput.Substring(CSPInput.Length - 5, 3)) ||
                    !regexItem2.IsMatch(CSPInput.Substring(CSPInput.Length - 1, 1)) ||
                    !regexItem4.IsMatch(CSPInput.Substring(CSPInput.Length - 2, 1)) ||
                    !regexItem3.IsMatch(CSPInput.Substring(4, 1)))
                {
                    inputValidationPass = false;
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                inputValidationPass = false;
                return false;
            }
        }
        private void GetCPUPowerCSPFromQRScan(string QRCode)
        {
            try
            {
                if (!string.IsNullOrEmpty(QRCode) && QRCode.Contains("$"))
                {
                    log.Error("GetCPUPowerCSPFromQRScan" + QRCode);
                    
                    //Regex.Replace(QRCode, @"\t\n\r", "");
                    QRCode.Trim('\t');
                    QRCode.Trim('\n');
                    QRCode.Trim('\r');
                    QRCode.Trim(' ');
                    string[] QRCodeparsed = QRCode.Split('$');
                   // log.Error(string.Format("{0} {1} {2}", QRCodeparsed[0], QRCodeparsed[1], QRCodeparsed[2]));
                    if (QRCodeparsed.Length == 3)
                    {
                        //log.Error("GetCPUPowerCSPFromQRScan-0" + QRCodeparsed[0]);
                        textBoxCameraControlCPUSN.Text = QRCodeparsed[0];

                        //log.Error("GetCPUPowerCSPFromQRScan-2" + QRCodeparsed[2]);
                        textBoxCameraControlSerialNumPower.Text = QRCodeparsed[2];

                        //log.Error("GetCPUPowerCSPFromQRScan-1" + QRCodeparsed[1]);
                        textBoxCameraControlSerialNumCSP.Text = QRCodeparsed[1];

                       
                        inputValidationPass = true;
                        QRCodeScanDone = true;
                        //buttonOK.Focus();
                    }
                }
                //if (!string.IsNullOrWhiteSpace(QRCode) && QRCode.Length > 35)
                //{
                //    textBoxCameraControlCPUSN.Text = QRCode.Substring(0, 14);
                //    textBoxCameraControlSerialNumPower.Text = QRCode.Substring(15, 10);
                //    textBoxCameraControlSerialNumCSP.Text = QRCode.Substring(26, 16);                    
                //    inputValidationPass = true;
                //    buttonOK.Focus();
                //    return true;
                //}
                //else
                //    inputValidationPass = false;
                //return false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                inputValidationPass = false;
                QRCodeScanDone = false;


            }
        }
        private void textBoxCameraSN_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxCameraSN.Text))
            {
                e.Cancel = true;
            }
        }
        private void textBoxImagerSN_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxImagerSN.Text))
            {
                e.Cancel = true;
            }
        }
        private async void OperatorInputForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (craNumber != textBoxCRANumber.Text)
            {
                if (!string.IsNullOrEmpty(textBoxCameraSN.Text) && !string.IsNullOrEmpty(textBoxCRANumber.Text))
                {
                    if (cameraDetailsModelList.Count() > 0)
                    {
                        await cameraDetailsPresenter.UpdateCRANumber(cameraDetailsModelList, textBoxCRANumber.Text);
                    }
                }
            }
            if (operatorInputFormFromMenu || e.CloseReason == CloseReason.WindowsShutDown || e.CloseReason == CloseReason.MdiFormClosing || e.CloseReason == CloseReason.TaskManagerClosing)
                return;
            else if (!inputValidationPass && this.DialogResult.Equals(DialogResult.Cancel))
                e.Cancel = true;
            inputValidationPass = true;
        }
        private void textBoxCameraSN_Validated(object sender, EventArgs e)
        {
        }
        private void textBoxImagerSN_Validated(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["KronosCamContext"].ToString()))
                {
                    conn.Open();
                    string query = string.Format("select ISISerialNumber FROM view_SN_GenTrack_CleanroomHeadComponentScan where ImagerSerialNumber=@ImagerSerialNumber");
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ImagerSerialNumber", textBoxImagerSN.Text.Trim());
                    if (cmd.ExecuteScalar() != null)
                    {
                        textBoxISISN.Text = cmd.ExecuteScalar().ToString().Trim();
                        ultraComboEditorDetectorType.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                textBoxISISN.Text = string.Empty;
            }
        }

        private void textBoxCameraControlCPUSN_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //TextBox button = sender as TextBox;
                //log.Error("textBoxCameraControlCPUSN_TextChanged" + button.Text);
                //GetCPUPowerCSPFromQRScan(button.Text.ToString());
                // inputValidationPass = true;
                //buttonOK.Focus();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);                
            }

        }

        private void textBoxCameraControlCPUSN_Validated(object sender, EventArgs e)
        {
            TextBox button = sender as TextBox;
            log.Error("textBoxCameraControlSerialNumPower_TextChanged" + button.Text);
            GetCPUPowerCSPFromQRScan(button.Text.ToString());
        }

        private void textBoxCameraControlSerialNumPower_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //TextBox button = sender as TextBox;
                //log.Error("textBoxCameraControlSerialNumPower_TextChanged" + button.Text);
                //GetCPUPowerCSPFromQRScan(button.Text.ToString());
                //inputValidationPass = true;
                //buttonOK.Focus();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }

        private void textBoxCameraControlSerialNumCSP_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //TextBox button = sender as TextBox;
                //log.Error("textBoxCameraControlSerialNumCSP_TextChanged" + button.Text);
                //GetCPUPowerCSPFromQRScan(button.Text.ToString());
                //inputValidationPass = true;
                //buttonOK.Focus();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
