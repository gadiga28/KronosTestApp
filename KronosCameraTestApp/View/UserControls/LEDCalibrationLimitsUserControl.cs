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
using KronosCameraTestApp.Model;
using System.Reflection;

namespace KronosCameraTestApp.View.UserControls
{
    public partial class LEDCalibrationLimitsUserControl : UserControl
    {
        string userMode = string.Empty;
        bool engineeringLoginStatus = false;
        string ECONumberValue = string.Empty;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LEDCalibrationPresenter ledCalibrationPresenter { get; set; }
        public LEDCalibrationLimitsUserControl(LEDCalibrationPresenter ledCalibrationPresenter)
        {
            InitializeComponent();
            this.ledCalibrationPresenter = ledCalibrationPresenter;
        }
        public LEDCalibrationLimitsUserControl(LEDCalibrationPresenter ledCalibrationPresenter, string UserMode, bool EngineeringLoginStatus, string EcoNumber)
        {
            InitializeComponent();
            this.ledCalibrationPresenter = ledCalibrationPresenter;
            this.userMode = UserMode;
            this.engineeringLoginStatus = EngineeringLoginStatus;
            this.ECONumberValue = EcoNumber;
        }

        private void LEDCalibrationLimitsUserControl_Load(object sender, EventArgs e)
        {
            if (userMode.ToString() != "")
                    EnableControls();
        }
        private void EnableControls()
        {
            try
            {
                Red40LEDLimit.Enabled = true;
                Red60LEDLimit.Enabled = true;
                Green40LEDLimit.Enabled = true;
                Green60LEDLimit.Enabled = true;
                Blue40LEDLimit.Enabled = true;
                Blue60LEDLimit.Enabled = true;
                LEDLimitOffset.Enabled = true;
                TestInterval.Enabled = true;
                ECONumber.Enabled = true;
                LimitsID.Enabled = true;
                UpperUVLEDLimit.Enabled = true;
                LowerUVLEDLimit.Enabled = true;
            }
            catch (Exception ex)
            {
            }
        }
        public void UpdateLimitsViewWithSpecificModel(LEDCalibrationLimitsData ledCalibrationLimitsData)
        {
            try
            {
                LimitsID.Text = ledCalibrationLimitsData.LimitsID.ToString();           
                DefinedDate.Text = ledCalibrationLimitsData.DefinedDate.ToString();
                UserModified.Text = ledCalibrationLimitsData.UserModified.ToString();
                ECONumber.Text = ledCalibrationLimitsData.ECONumber.ToString();
                Red40LEDLimit.Value = ledCalibrationLimitsData.Red40LEDLimit;
                Red60LEDLimit.Value = ledCalibrationLimitsData.Red60LEDLimit; ;
                Green40LEDLimit.Value = ledCalibrationLimitsData.Green40LEDLimit;
                Green60LEDLimit.Value = ledCalibrationLimitsData.Green60LEDLimit;
                Blue40LEDLimit.Value = ledCalibrationLimitsData.Blue40LEDLimit;
                Blue60LEDLimit.Value = ledCalibrationLimitsData.Blue60LEDLimit;
                LEDLimitOffset.Value = ledCalibrationLimitsData.LEDLimitOffset;
                TestInterval.Value = ledCalibrationLimitsData.CurrentTestInterval;
                UpperUVLEDLimit.Value = ledCalibrationLimitsData.UpperUVLEDLimit;
                LowerUVLEDLimit.Value = ledCalibrationLimitsData.LowerUVLEDLimit;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public LEDCalibrationLimitsData ReturnLEDCalibrationLimit()
        {
            try
            {
                LEDCalibrationLimitsData ledCalibrationLimitsData = new LEDCalibrationLimitsData();
                ledCalibrationLimitsData.LimitsID = Convert.ToInt32(LimitsID.Text);
                ledCalibrationLimitsData.LEDLimitOffset = Convert.ToDouble(LEDLimitOffset.Value);
                ledCalibrationLimitsData.Red40LEDLimit = Convert.ToDouble(Red40LEDLimit.Value);
                ledCalibrationLimitsData.Red60LEDLimit = Convert.ToDouble(Red60LEDLimit.Value);
                ledCalibrationLimitsData.Green40LEDLimit = Convert.ToDouble(Green40LEDLimit.Value);
                ledCalibrationLimitsData.Green60LEDLimit = Convert.ToDouble(Green60LEDLimit.Value);
                ledCalibrationLimitsData.Blue40LEDLimit = Convert.ToDouble(Blue40LEDLimit.Value);
                ledCalibrationLimitsData.Blue60LEDLimit = Convert.ToDouble(Blue60LEDLimit.Value);
                ledCalibrationLimitsData.UserModified = Environment.UserName;
                ledCalibrationLimitsData.DefinedDate = DateTime.Now;
                ledCalibrationLimitsData.ECONumber = ECONumber.Text;                
                ledCalibrationLimitsData.CurrentTestInterval = Convert.ToInt32(TestInterval.Value);
                ledCalibrationLimitsData.PreviousTestInterval = ledCalibrationLimitsData.CurrentTestInterval;
                ledCalibrationLimitsData.UpperUVLEDLimit = Convert.ToInt32(UpperUVLEDLimit.Value);
                ledCalibrationLimitsData.LowerUVLEDLimit = Convert.ToInt32(LowerUVLEDLimit.Value);
                return ledCalibrationLimitsData;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }

        private void Red40LEDLimit_AfterChangeValue(object sender, NationalInstruments.UI.AfterChangeNumericValueEventArgs e)
        {

        }

        private void Blue40LEDLimit_AfterChangeValue(object sender, NationalInstruments.UI.AfterChangeNumericValueEventArgs e)
        {

        }
    }
}
