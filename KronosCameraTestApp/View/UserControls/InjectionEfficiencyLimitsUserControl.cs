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
namespace KronosCameraTestApp.View.UserControls
{
    public partial class InjectionEfficiencyLimitsUserControl : UserControl
    {
        string userMode = string.Empty;
        bool engineeringLoginStatus = false;
        string ECONumberValue = string.Empty;
        static int currentLimitsRecord = 0;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        InjectionEfficiencyLimitsPresenter injectionEfficiencyTestLimitsPresenter { get; set; }
        public InjectionEfficiencyLimitsUserControl(InjectionEfficiencyLimitsPresenter injectionEfficiencyTestLimitsPresenter)
        {
            InitializeComponent();
            this.injectionEfficiencyTestLimitsPresenter = injectionEfficiencyTestLimitsPresenter;
        }
        public InjectionEfficiencyLimitsUserControl(InjectionEfficiencyLimitsPresenter injectionEfficiencyTestLimitsPresenter, string UserMode, bool EngineeringLoginStatus, string EcoNumber)
        {
            InitializeComponent();
            this.injectionEfficiencyTestLimitsPresenter = injectionEfficiencyTestLimitsPresenter;
            this.userMode=UserMode;
            this.engineeringLoginStatus = EngineeringLoginStatus;
            this.ECONumberValue = EcoNumber;
        }
        private void InjectionEfficiencyTestLimitsUserControl_Load(object sender, EventArgs e)
        {
            currentLimitsRecord = 0;
            if (userMode.ToString() != "")
                EnableControls();
        }
        private void EnableControls()
        {
            try
            {
                InjectionEfficiencyLimit.Enabled = true;
                ECONumber.Enabled = true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public InjectionEfficiencyTestLimitsData ReturnInjectionEfficiencyLimits()
        {
            try
            {
                InjectionEfficiencyTestLimitsData injectionEfficiencyTestLimitsData = new InjectionEfficiencyTestLimitsData();
                injectionEfficiencyTestLimitsData.LimitsID = Convert.ToInt32(LimitsID.Text);
                injectionEfficiencyTestLimitsData.InjectionEffLimit = Convert.ToDouble(InjectionEfficiencyLimit.Value);
                injectionEfficiencyTestLimitsData.UserModified = Environment.UserName;
                injectionEfficiencyTestLimitsData.DefinedDate = DateTime.Now;
                injectionEfficiencyTestLimitsData.ECONumber = ECONumber.Text;
                return injectionEfficiencyTestLimitsData;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }
        public void UpdateLimitsViewWithSpecificModel(InjectionEfficiencyTestLimitsData injectionEfficiencyTestLimitsData)
        {
            try
            {
                LimitsID.Text = injectionEfficiencyTestLimitsData.LimitsID.ToString();
                InjectionEfficiencyLimit.Value = injectionEfficiencyTestLimitsData.InjectionEffLimit;
                DefinedDate.Text = injectionEfficiencyTestLimitsData.DefinedDate.ToString();
                UserModified.Text = injectionEfficiencyTestLimitsData.UserModified;
                ECONumber.Text = injectionEfficiencyTestLimitsData.ECONumber;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
