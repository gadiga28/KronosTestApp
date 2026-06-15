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
namespace KronosCameraTestApp.View.UserControls
{
    public partial class InjectionPerformanceLimitsUserControl : UserControl
    {
        string userMode = string.Empty;
        bool engineeringLoginStatus = false;
        string ECONumberValue = string.Empty;
        static int currentLimitsRecord = 0;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        InjectionPerformanceLimitsPresenter injectionPerformanceTestLimitsPresenter { get; set; }
        public InjectionPerformanceLimitsUserControl(InjectionPerformanceLimitsPresenter injectionPerformanceTestLimitsPresenter)
        {
            InitializeComponent();
             this.injectionPerformanceTestLimitsPresenter = injectionPerformanceTestLimitsPresenter;
        }
        public InjectionPerformanceLimitsUserControl(InjectionPerformanceLimitsPresenter injectionPerformanceTestLimitsPresenter, string UserMode, bool EngineeringLoginStatus, string EcoNumber)
        {
            InitializeComponent();
            this.injectionPerformanceTestLimitsPresenter = injectionPerformanceTestLimitsPresenter;
            this.userMode=UserMode;
            this.engineeringLoginStatus = EngineeringLoginStatus;
            this.ECONumberValue = EcoNumber;
        }
        private void InjectionPerformanceTestLimitsUserControl_Load(object sender, EventArgs e)
        {
             currentLimitsRecord = 0;           
            if (userMode.ToString() != "")
                EnableControls();
        }
         private void EnableControls()
        {
            try
            {
                AveSigAfterInjectionMax.Enabled = true;
                StdDevOfAvesMax.Enabled = true;
                ROI_Xs.Enabled = true;
                ROI_Ys.Enabled = true;
                ROI_dXs.Enabled = true;
                ROI_dYs.Enabled = true;
                ROI_dXbs.Enabled = true;
                ROI_dYbs.Enabled = true;
                ROI_PixRate.Enabled = true;
                ROI_NDRO.Enabled = true;
                Points.Enabled = true;
                Auto_Flashe_LED.Enabled = true;
                AutoFlash_Target.Enabled = true; 
                ECONumber.Enabled = true;
            }
            catch(Exception ex)
            {
            }
        }
        public void UpdateLimitsViewWithSpecificModel(InjectionPerformanceLimitsData injectionPerformanceLimitsData)
        {
            try
            {
                LimitsID.Text = injectionPerformanceLimitsData.LimitsID.ToString();
                AveSigAfterInjectionMax.Value = injectionPerformanceLimitsData.AveSigAfterInjectionMax;
                StdDevOfAvesMax.Value = injectionPerformanceLimitsData.StdDevOfAvesMax;
                ROI_Xs.Value = injectionPerformanceLimitsData.ROI_Xs;
                ROI_Ys.Value = injectionPerformanceLimitsData.ROI_Ys;
                ROI_dXs.Value = injectionPerformanceLimitsData.ROI_dXs;
                ROI_dYs.Value = injectionPerformanceLimitsData.ROI_dYs;
                ROI_dXbs.Value = injectionPerformanceLimitsData.ROI_dXbs;
                ROI_dYbs.Value = injectionPerformanceLimitsData.ROI_dYbs;
                ROI_PixRate.Value = injectionPerformanceLimitsData.ROI_PixRate;
                ROI_NDRO.Value = injectionPerformanceLimitsData.ROI_NDRO;
                Points.Value = injectionPerformanceLimitsData.Points;
                Auto_Flashe_LED.Value = injectionPerformanceLimitsData.Auto_Flashe_LED;
                AutoFlash_Target.Value = injectionPerformanceLimitsData.AutoFlash_Target;
                DefinedDate.Text = injectionPerformanceLimitsData.DefinedDate.ToString();
                UserModified.Text = injectionPerformanceLimitsData.UserModified.ToString();
                ECONumber.Text = injectionPerformanceLimitsData.ECONumber.ToString();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public InjectionPerformanceLimitsData ReturnInjectionPerformanceLimit()
        {
            try
            {
                InjectionPerformanceLimitsData injectionPerformanceLimitsData = new InjectionPerformanceLimitsData();
                injectionPerformanceLimitsData.LimitsID = Convert.ToInt32(LimitsID.Text);
                injectionPerformanceLimitsData.AveSigAfterInjectionMax = Convert.ToInt32(AveSigAfterInjectionMax.Value);
                injectionPerformanceLimitsData.StdDevOfAvesMax = Convert.ToInt32(StdDevOfAvesMax.Value);
                injectionPerformanceLimitsData.ROI_Xs = Convert.ToInt32(ROI_Xs.Value);
                injectionPerformanceLimitsData.ROI_Ys = Convert.ToInt32(ROI_Ys.Value);
                injectionPerformanceLimitsData.ROI_dXs = Convert.ToInt32(ROI_dXs.Value);
                injectionPerformanceLimitsData.ROI_dYs = Convert.ToInt32(ROI_dYs.Value);
                injectionPerformanceLimitsData.ROI_dXbs = Convert.ToInt32(ROI_dXbs.Value);
                injectionPerformanceLimitsData.ROI_dYbs = Convert.ToInt32(ROI_dYbs.Value);
                injectionPerformanceLimitsData.ROI_PixRate = Convert.ToInt32(ROI_PixRate.Value);
                injectionPerformanceLimitsData.ROI_NDRO = Convert.ToInt32(ROI_NDRO.Value);
                injectionPerformanceLimitsData.Points = Convert.ToInt32(Points.Value);
                injectionPerformanceLimitsData.Auto_Flashe_LED = Convert.ToInt32(Auto_Flashe_LED.Value);
                injectionPerformanceLimitsData.AutoFlash_Target = Convert.ToInt32(AutoFlash_Target.Value);
                injectionPerformanceLimitsData.UserModified = Environment.UserName;
                injectionPerformanceLimitsData.DefinedDate = DateTime.Now;
                injectionPerformanceLimitsData.ECONumber = ECONumber.Text;
                return injectionPerformanceLimitsData;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }
    }
}
