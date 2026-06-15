using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using KronosCameraTestApp.Presenter;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.Helpers;
namespace KronosCameraTestApp.View.UserControls
{
    public partial class NoiseVsNDROSLimitsUserControl : UserControl
    {
        string userMode = string.Empty;
        bool engineeringLoginStatus = false;
        string ECONumberValue = string.Empty;
        static int currentLimitsRecord = 0;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        NoiseVsNDROSLimitsPresenter noiseVsNDROSLimitsPresenter { get; set; }
        public NoiseVsNDROSLimitsUserControl(NoiseVsNDROSLimitsPresenter noiseVsNDROSLimitsPresenter)
        {
            InitializeComponent();
            this.noiseVsNDROSLimitsPresenter = noiseVsNDROSLimitsPresenter;
        }
        public NoiseVsNDROSLimitsUserControl(NoiseVsNDROSLimitsPresenter noiseVsNDROSLimitsPresenter, string UserMode, bool EngineeringLoginStatus, string EcoNumber)
        {
            InitializeComponent();
            this.noiseVsNDROSLimitsPresenter = noiseVsNDROSLimitsPresenter;
            this.userMode=UserMode;
            this.engineeringLoginStatus = EngineeringLoginStatus;
            this.ECONumberValue = EcoNumber;
        }
        private void NoiseVsNDROSLimitsUserControl_Load(object sender, EventArgs e)
        {
            currentLimitsRecord = 0;
            if (userMode.ToString() != "")
                EnableControls();
        }
        private void EnableControls()
        {
            try
            {
                SnglNoiseLimit.Enabled = true;
                SnglNoiseLowerLimit.Enabled = true;
                SnglNoiseUpperLimit.Enabled = true;
                SnglNoiseTolerance.Enabled = true;
                PowerFitUpper.Enabled = true;
                PowerFitLower.Enabled = true;
                RPower2Correlation.Enabled = true;
                ROI_Xs.Enabled = true;
                ROI_Ys.Enabled = true;
                ROI_dXs.Enabled = true;
                ROI_dYs.Enabled = true;
                ROI_dXbs.Enabled = true;
                ROI_dYbs.Enabled = true;                
                NDROs.Enabled = true;
                Trials.Enabled = true;
                Flashes.Enabled = true;
                Time.Enabled = true;
                ExposureControl.Enabled = true;
                ReadOutModeType.Enabled = true;
                ECONumber.Enabled = true;
            }
            catch (Exception ex)
            {
            }
        }
        public void UpdateLimitsViewWithSpecificModel(NoiseVsNDROSLimitsData noiseVsNDROSLimitsData)
        {
            try
            {
                LimitsID.Text = noiseVsNDROSLimitsData.LimitsID.ToString();
                SnglNoiseLimit.Value = noiseVsNDROSLimitsData.SnglNoiseMax;
                SnglNoiseLowerLimit.Value = noiseVsNDROSLimitsData.SnglNoiseLowerLimit;
                SnglNoiseUpperLimit.Value = noiseVsNDROSLimitsData.SnglNoiseUpperLimit;
                SnglNoiseTolerance.Value = noiseVsNDROSLimitsData.SnglNoiseTolerance;
                PowerFitUpper.Value = noiseVsNDROSLimitsData.PowerFitUpper;
                PowerFitLower.Value = noiseVsNDROSLimitsData.PowerFitLower;
                RPower2Correlation.Value = noiseVsNDROSLimitsData.RPower2Correlation;
                ROI_Xs.Value = noiseVsNDROSLimitsData.ROI_Xs;
                ROI_Ys.Value = noiseVsNDROSLimitsData.ROI_Ys;
                ROI_dXs.Value = noiseVsNDROSLimitsData.ROI_dXs;
                ROI_dYs.Value = noiseVsNDROSLimitsData.ROI_dYs;
                ROI_dXbs.Value = noiseVsNDROSLimitsData.ROI_dXbs;
                ROI_dYbs.Value = noiseVsNDROSLimitsData.ROI_dYbs;
                NDROs.Value = noiseVsNDROSLimitsData.NDROs;
                Trials.Value = noiseVsNDROSLimitsData.Trials;
                Flashes.Value = noiseVsNDROSLimitsData.Flashes;
                Time.Value = noiseVsNDROSLimitsData.Time;
                ExposureControl.Value = noiseVsNDROSLimitsData.ExposureControl;
                ReadOutModeType.Value = noiseVsNDROSLimitsData.ReadOutModeType;
                DefinedDate.Text = noiseVsNDROSLimitsData.DefinedDate.ToString();
                UserModified.Text = noiseVsNDROSLimitsData.UserModified.ToString();
                ECONumber.Text = noiseVsNDROSLimitsData.ECONumber.ToString();
                //final test report
                TestAppHelper.SnglNoiseMax = Convert.ToInt32(SnglNoiseLimit.Value);
                TestAppHelper.SnglNoiseLowerLimit = SnglNoiseLowerLimit.Value;
                TestAppHelper.SnglNoiseUpperLimit = SnglNoiseUpperLimit.Value;
                TestAppHelper.SnglNoiseTolerance = SnglNoiseTolerance.Value;
                TestAppHelper.PowerFitLower = PowerFitUpper.Value;
                TestAppHelper.PowerFitLower = PowerFitLower.Value;
                TestAppHelper.RPower2Correlation = RPower2Correlation.Value;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public NoiseVsNDROSLimitsData ReturnNoiseVsNDROsLimit()
        {
            try
            {
                NoiseVsNDROSLimitsData noiseVsNDROSLimitsData = new NoiseVsNDROSLimitsData();
                noiseVsNDROSLimitsData.LimitsID = Convert.ToInt32(LimitsID.Text);
                noiseVsNDROSLimitsData.SnglNoiseMax = Convert.ToInt32(SnglNoiseLimit.Value);
                noiseVsNDROSLimitsData.SnglNoiseLowerLimit = Convert.ToDouble(SnglNoiseLowerLimit.Value);
                noiseVsNDROSLimitsData.SnglNoiseUpperLimit = Convert.ToDouble(SnglNoiseUpperLimit.Value);
                noiseVsNDROSLimitsData.SnglNoiseTolerance = Convert.ToDouble(SnglNoiseTolerance.Value);
                noiseVsNDROSLimitsData.PowerFitUpper = Convert.ToDouble(PowerFitUpper.Value);
                noiseVsNDROSLimitsData.PowerFitLower = Convert.ToDouble(PowerFitLower.Value);
                noiseVsNDROSLimitsData.RPower2Correlation = Convert.ToDouble(RPower2Correlation.Value);
                noiseVsNDROSLimitsData.ROI_Xs = Convert.ToInt32(ROI_Xs.Value);
                noiseVsNDROSLimitsData.ROI_Ys = Convert.ToInt32(ROI_Ys.Value);
                noiseVsNDROSLimitsData.ROI_dXs = Convert.ToInt32(ROI_dXs.Value);
                noiseVsNDROSLimitsData.ROI_dYs = Convert.ToInt32(ROI_dYs.Value);
                noiseVsNDROSLimitsData.ROI_dXbs = Convert.ToInt32(ROI_dXbs.Value);
                noiseVsNDROSLimitsData.ROI_dYbs = Convert.ToInt32(ROI_dYbs.Value);
                noiseVsNDROSLimitsData.NDROs = Convert.ToInt32(NDROs.Value);
                noiseVsNDROSLimitsData.Trials = Convert.ToInt32(Trials.Value);
                noiseVsNDROSLimitsData.Flashes = Convert.ToInt32(Flashes.Value);
                noiseVsNDROSLimitsData.Time = Convert.ToInt32(Time.Value);
                noiseVsNDROSLimitsData.ExposureControl = Convert.ToInt32(ExposureControl.Value);
                noiseVsNDROSLimitsData.ReadOutModeType = Convert.ToInt32(ReadOutModeType.Value);
                noiseVsNDROSLimitsData.UserModified = Environment.UserName;
                noiseVsNDROSLimitsData.DefinedDate = DateTime.Now;
                noiseVsNDROSLimitsData.ECONumber = ECONumber.Text;
                return noiseVsNDROSLimitsData;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }
    }
}
