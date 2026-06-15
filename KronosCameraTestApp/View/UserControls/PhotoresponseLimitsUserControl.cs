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
    public partial class PhotoresponseLimitsUserControl : UserControl
    {
        string userMode = string.Empty;
        bool engineeringLoginStatus = false;
        string ECONumberValue = string.Empty;
        static int currentLimitsRecord = 0;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        PhotoresponseLimitsPresenter photoresponseLimitsPresenter { get; set; }
        public PhotoresponseLimitsUserControl(PhotoresponseLimitsPresenter photoresponseLimitsPresenter)
        {
            InitializeComponent();
            this.photoresponseLimitsPresenter = photoresponseLimitsPresenter;
        }
        public PhotoresponseLimitsUserControl(PhotoresponseLimitsPresenter photoresponseLimitsPresenter, string UserMode, bool EngineeringLoginStatus, string EcoNumber)
        {
            InitializeComponent();
            this.photoresponseLimitsPresenter = photoresponseLimitsPresenter;
            this.userMode=UserMode;
            this.engineeringLoginStatus = EngineeringLoginStatus;
            this.ECONumberValue = EcoNumber;
        }
        private void PhotoresponseLimitsUserControl_Load(object sender, EventArgs e)
        {
              currentLimitsRecord = 0;           
            if (userMode.ToString() != "")
                EnableControls();
        }
        private void EnableControls()
        {
            try
            {
                LinearityLevelMax.Enabled = true;
                FullWellLevelMin.Enabled = true;
                ROI_Xs.Enabled = true;
                ROI_Ys.Enabled = true;
                ROI_dXs.Enabled = true;
                ROI_dYs.Enabled = true;
                ROI_dXbs.Enabled = true;
                ROI_dYbs.Enabled = true;
                ROI_PixRate.Enabled = true;
                ROI_NDRO.Enabled = true;
                Exposures.Enabled = true;
                Flashes.Enabled = true;
                Time.Enabled = true;
                ExposureControl.Enabled = true;   
                ECONumber.Enabled = true;
            }
            catch(Exception ex)
            {
            }
        }
        public void UpdateLimitsViewWithSpecificModel(PhotoresponseLimitsData photoresponseLimitsData)
        {
            try
            {
                LimitsID.Text = photoresponseLimitsData.LimitsID.ToString();
                LinearityLevelMax.Value = photoresponseLimitsData.LinearityLevelMax;
                FullWellLevelMin.Value = photoresponseLimitsData.FullWellLevelMin;
                ROI_Xs.Value = photoresponseLimitsData.ROI_Xs;
                ROI_Ys.Value = photoresponseLimitsData.ROI_Ys;
                ROI_dXs.Value = photoresponseLimitsData.ROI_dXs;
                ROI_dYs.Value = photoresponseLimitsData.ROI_dYs;
                ROI_dXbs.Value = photoresponseLimitsData.ROI_dXbs;
                ROI_dYbs.Value = photoresponseLimitsData.ROI_dYbs;
                ROI_PixRate.Value = photoresponseLimitsData.ROI_PixRate;
                ROI_NDRO.Value = photoresponseLimitsData.ROI_NDRO;
                Exposures.Value = photoresponseLimitsData.Exposures;
                Flashes.Value = photoresponseLimitsData.Flashes;
                Time.Value = photoresponseLimitsData.Time;
                ExposureControl.Value = photoresponseLimitsData.ExposureControl;
                DefinedDate.Text = photoresponseLimitsData.DefinedDate.ToString();
                UserModified.Text = photoresponseLimitsData.UserModified.ToString();
                ECONumber.Text = photoresponseLimitsData.ECONumber.ToString();
                //final test report
                TestAppHelper.FullWellLevelMin = Convert.ToInt32(FullWellLevelMin.Value);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public PhotoresponseLimitsData ReturPhotoresponseLimit()
        {
            try
            {
                PhotoresponseLimitsData photoresponseLimitsData = new PhotoresponseLimitsData();
                photoresponseLimitsData.LimitsID = Convert.ToInt32(LimitsID.Text);
                photoresponseLimitsData.LinearityLevelMax = Convert.ToDouble(LinearityLevelMax.Value);
                photoresponseLimitsData.FullWellLevelMin = Convert.ToInt32(FullWellLevelMin.Value);
                photoresponseLimitsData.ROI_Xs = Convert.ToInt32(ROI_Xs.Value);
                photoresponseLimitsData.ROI_Ys = Convert.ToInt32(ROI_Ys.Value);
                photoresponseLimitsData.ROI_dXs = Convert.ToInt32(ROI_dXs.Value);
                photoresponseLimitsData.ROI_dYs = Convert.ToInt32(ROI_dYs.Value);
                photoresponseLimitsData.ROI_dXbs = Convert.ToInt32(ROI_dXbs.Value);
                photoresponseLimitsData.ROI_dYbs = Convert.ToInt32(ROI_dYbs.Value);
                photoresponseLimitsData.ROI_PixRate = Convert.ToInt32(ROI_PixRate.Value);
                photoresponseLimitsData.ROI_NDRO = Convert.ToInt32(ROI_NDRO.Value);
                photoresponseLimitsData.Exposures = Convert.ToInt32(Exposures.Value);
                photoresponseLimitsData.Flashes = Convert.ToInt32(Flashes.Value);
                photoresponseLimitsData.Time = Convert.ToInt32(Time.Value);
                photoresponseLimitsData.ExposureControl = Convert.ToInt32(ExposureControl.Value);
                photoresponseLimitsData.UserModified = Environment.UserName;
                photoresponseLimitsData.DefinedDate = DateTime.Now;
                photoresponseLimitsData.ECONumber = ECONumber.Text;
                return photoresponseLimitsData;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }
    }
}
