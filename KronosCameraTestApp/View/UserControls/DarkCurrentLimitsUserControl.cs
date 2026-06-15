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
using System.Reflection;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.Helpers;
namespace KronosCameraTestApp.View.UserControls
{
    public partial class DarkCurrentLimitsUserControl : UserControl
    {
        string userMode = string.Empty;
        bool engineeringLoginStatus = false;
        string ECONumberValue = string.Empty;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DarkCurrentLimitsPresenter darkCurrentTestLimitsPresenter { get; set; }
        public DarkCurrentLimitsUserControl(DarkCurrentLimitsPresenter darkCurrentTestLimitsPresenter)
        {
            InitializeComponent();
            this.darkCurrentTestLimitsPresenter = darkCurrentTestLimitsPresenter;
        }
        public DarkCurrentLimitsUserControl(DarkCurrentLimitsPresenter darkCurrentTestLimitsPresenter, string UserMode, bool EngineeringLoginStatus, string EcoNumber)
        {
            InitializeComponent();
            this.darkCurrentTestLimitsPresenter = darkCurrentTestLimitsPresenter;
            this.userMode=UserMode;
            this.engineeringLoginStatus = EngineeringLoginStatus;
            this.ECONumberValue = EcoNumber;
        }
        private  void DarkCurrentTestLimitsUserControl_Load(object sender, EventArgs e)
        {
           
            if (userMode.ToString() != "")
                EnableControls();           
        }
        private void EnableControls()
        {
            try
            {
                DarkCurrentMaxDarkROI.Enabled = true;
                DarkCurrentLowerLimit.Enabled = true;
                DarkCurrentUpperLimit.Enabled = true;
                DarkCurrentMaxDarkROITol.Enabled = true;
                //AddLimitsData.Enabled = true;
                //RemoveLimitsData.Enabled = true;
                //UpdateLimitsData.Enabled = true;
                //RefreshLimitsData.Enabled = true;
                ECONumber.Enabled = true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public DarkCurrentTestLimitsData ReturnDarkCurrentLimits()
        {
            try
            {
                DarkCurrentTestLimitsData darkCurrentTestLimitsData = new DarkCurrentTestLimitsData();
                darkCurrentTestLimitsData.LimitsID = Convert.ToInt32(LimitsID.Text);
                darkCurrentTestLimitsData.DarkCurrentMaxDarkROI = Convert.ToDouble(DarkCurrentMaxDarkROI.Value);
                darkCurrentTestLimitsData.DarkCurrentLowerLimit = Convert.ToDouble(DarkCurrentLowerLimit.Value);
                darkCurrentTestLimitsData.DarkCurrentUpperLimit = Convert.ToDouble(DarkCurrentUpperLimit.Value);
                darkCurrentTestLimitsData.DarkCurrentMaxDarkROITol = Convert.ToDouble(DarkCurrentMaxDarkROITol.Value);
                darkCurrentTestLimitsData.UserModified = Environment.UserName;
                darkCurrentTestLimitsData.DefinedDate = DateTime.Now;
                darkCurrentTestLimitsData.ECONumber = ECONumber.Text;
                return darkCurrentTestLimitsData;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }
        public void UpdateLimitsViewWithSpecificModel(DarkCurrentTestLimitsData darkCurrentTestLimitsData)
        {
            try
            {
                LimitsID.Text = darkCurrentTestLimitsData.LimitsID.ToString();
                DarkCurrentMaxDarkROI.Value = darkCurrentTestLimitsData.DarkCurrentMaxDarkROI;
                DarkCurrentLowerLimit.Value = darkCurrentTestLimitsData.DarkCurrentLowerLimit;
                DarkCurrentUpperLimit.Value = darkCurrentTestLimitsData.DarkCurrentUpperLimit;
                DarkCurrentMaxDarkROITol.Value = darkCurrentTestLimitsData.DarkCurrentMaxDarkROITol;
                DefinedDate.Text = darkCurrentTestLimitsData.DefinedDate.ToString();
                UserModified.Text = darkCurrentTestLimitsData.UserModified;
                ECONumber.Text = darkCurrentTestLimitsData.ECONumber;
                TestAppHelper.MaxDarkROI = DarkCurrentMaxDarkROI.Value;
                TestAppHelper.MaxDarkROILowerLimit = DarkCurrentLowerLimit.Value;
                TestAppHelper.MaxDarkROIUpperLimit = DarkCurrentUpperLimit.Value;
                TestAppHelper.MaxDarkROITolerance = DarkCurrentMaxDarkROITol.Value;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
