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
    public partial class SegmentInjectLimitsUserControl : UserControl
    {
        string userMode = string.Empty;
        bool engineeringLoginStatus = false;
        string ECONumberValue = string.Empty;
        static int currentLimitsRecord = 0;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        SegmentInjectLimitsPresenter segmentInjectLimitsPresenter { get; set; }
        public SegmentInjectLimitsUserControl(SegmentInjectLimitsPresenter segmentInjectLimitsPresenter)
        {
            InitializeComponent();
            this.segmentInjectLimitsPresenter = segmentInjectLimitsPresenter;
        }
        public SegmentInjectLimitsUserControl(SegmentInjectLimitsPresenter segmentInjectLimitsPresenter, string UserMode, bool EngineeringLoginStatus, string EcoNumber)
        {
            InitializeComponent();
            this.segmentInjectLimitsPresenter = segmentInjectLimitsPresenter;
            this.userMode=UserMode;
            this.engineeringLoginStatus = EngineeringLoginStatus;
            this.ECONumberValue = EcoNumber;
        }
        private void SegmentInjectLimitsUserControl_Load(object sender, EventArgs e)
        {
                currentLimitsRecord = 0;
            if (userMode.ToString() != "")
                EnableControls();
        }
        private void EnableControls()
        {
            try
            {
                InjectLevel_adu.Enabled = true;
                InjectDisturbance_adu.Enabled = true;
                ECONumber.Enabled = true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public SegmentInjectLimitsData ReturnSegmentInjectLimit()
        {
            try
            {
                SegmentInjectLimitsData segmentInjectLimitsData = new SegmentInjectLimitsData();
                segmentInjectLimitsData.LimitsID = Convert.ToInt32(LimitsID.Text);
                segmentInjectLimitsData.InjectLevel_adu = Convert.ToInt32(InjectLevel_adu.Value);
                segmentInjectLimitsData.InjectDisturbance_adu = Convert.ToInt32(InjectDisturbance_adu.Value);
                segmentInjectLimitsData.UserModified = Environment.UserName;
                segmentInjectLimitsData.DefinedDate = DateTime.Now;
                segmentInjectLimitsData.ECONumber = ECONumber.Text;
                return segmentInjectLimitsData;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }
        public void UpdateLimitsViewWithSpecificModel(SegmentInjectLimitsData segmentInjectLimitsData)
        {
            try
            {
                LimitsID.Text = segmentInjectLimitsData.LimitsID.ToString();
                InjectLevel_adu.Value = segmentInjectLimitsData.InjectLevel_adu;
                InjectDisturbance_adu.Value = segmentInjectLimitsData.InjectDisturbance_adu;
                DefinedDate.Text = segmentInjectLimitsData.DefinedDate.ToString();
                UserModified.Text = segmentInjectLimitsData.UserModified;
                ECONumber.Text = segmentInjectLimitsData.ECONumber;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
