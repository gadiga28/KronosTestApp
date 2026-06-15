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
    public partial class NDROReadDriftLimitsUserControl : UserControl
    {
        string userMode = string.Empty;
        bool engineeringLoginStatus = false;
        string ECONumberValue = string.Empty;
        static int currentLimitsRecord = 0;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        NDROReadDriftLimitsPresenter nDROReadDriftTestLimitsPresenter { get; set; }
        public NDROReadDriftLimitsUserControl(NDROReadDriftLimitsPresenter nDROReadDriftTestLimitsPresenter)
        {
            InitializeComponent();
            this.nDROReadDriftTestLimitsPresenter = nDROReadDriftTestLimitsPresenter;
        }
        public NDROReadDriftLimitsUserControl(NDROReadDriftLimitsPresenter nDROReadDriftTestLimitsPresenter, string UserMode, bool EngineeringLoginStatus, string EcoNumber)
        {
            InitializeComponent();
            this.nDROReadDriftTestLimitsPresenter = nDROReadDriftTestLimitsPresenter;
            this.userMode=UserMode;
            this.engineeringLoginStatus = EngineeringLoginStatus;
            this.ECONumberValue = EcoNumber;
        }
        private  void NDROReadDriftTestLimitsUserControl_Load(object sender, EventArgs e)
        {
            currentLimitsRecord = 0;
            if (userMode.ToString() != "")
                EnableControls();
        }
        private void EnableControls()
        {
            try
            {
                NdroReadDriftMaxSlope.Enabled = true;
                ECONumber.Enabled = true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public NdroReadDriftTestLimitsData ReturnNdroReadDriftLimit()
        {
            try
            {
                NdroReadDriftTestLimitsData ndroReadDriftTestLimitsData = new NdroReadDriftTestLimitsData();
                ndroReadDriftTestLimitsData.LimitsID = Convert.ToInt32(LimitsID.Text);
                ndroReadDriftTestLimitsData.NdroReadDriftMaxSlope = Convert.ToDouble(NdroReadDriftMaxSlope.Value);
                ndroReadDriftTestLimitsData.UserModified = Environment.UserName;
                ndroReadDriftTestLimitsData.DefinedDate = DateTime.Now;
                ndroReadDriftTestLimitsData.ECONumber = ECONumber.Text;
                return ndroReadDriftTestLimitsData;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }
        public void UpdateLimitsViewWithSpecificModel(NdroReadDriftTestLimitsData injectionEfficiencyTestLimitsData)
        {
            try
            {
                LimitsID.Text = injectionEfficiencyTestLimitsData.LimitsID.ToString();
                NdroReadDriftMaxSlope.Value = injectionEfficiencyTestLimitsData.NdroReadDriftMaxSlope;
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
