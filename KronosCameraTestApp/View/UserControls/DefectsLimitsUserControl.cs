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
    public partial class DefectsLimitsUserControl : UserControl
    {
        string userMode = string.Empty;
        bool engineeringLoginStatus = false;
        string ECONumberValue = string.Empty;
        static int currentLimitsRecord = 0;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DefectsLimitsPresenter defectsTestLimitsPresenter { get; set; }
        public DefectsLimitsUserControl(DefectsLimitsPresenter defectsTestLimitsPresenter)
        {
            InitializeComponent();
            this.defectsTestLimitsPresenter = defectsTestLimitsPresenter;
        }
        public DefectsLimitsUserControl(DefectsLimitsPresenter defectsTestLimitsPresenter, string UserMode, bool EngineeringLoginStatus, string EcoNumber)
        {
            InitializeComponent();
            this.defectsTestLimitsPresenter = defectsTestLimitsPresenter;
            this.userMode=UserMode;
            this.engineeringLoginStatus = EngineeringLoginStatus;
            this.ECONumberValue = EcoNumber;
        }
        private void DefectsTestUserControl_Load(object sender, EventArgs e)
        {
             currentLimitsRecord = 0;
            if (userMode.ToString() != "")
                EnableControls();
            //await defectsTestLimitsPresenter.GetDefectsLimits();
            //UpdateLimitsViewWithModel(0);
        }
        private void EnableControls()
        {
            try
            {
                PRNUPositive.Enabled = true;
                PRNUNegative.Enabled = true;
                MaxTrap.Enabled = true;
                HotPixels.Enabled = true;
                DeadPixels.Enabled = true;
                DarkPixels.Enabled = true;
                AveTrap.Enabled = true;
                MaxDarkROI.Enabled = true;
                TotalNumDefects.Enabled = true;
                TotalNumClusters.Enabled = true;
                ROI_Xs.Enabled = true;
                ROI_Ys.Enabled = true;
                ROI_dXs.Enabled = true;
                ROI_dYs.Enabled = true;
                ROI_dXbs.Enabled = true;
                ROI_dYbs.Enabled = true;
                ROI_PixRate.Enabled = true;
                ROI_NDRO.Enabled = true;
                PRNU_LED.Enabled = true;
                PRNU_Target.Enabled = true;
                Trap_LED.Enabled = true;
                Trap_Target.Enabled = true;
                HotPixel_LED.Enabled = true;
                HotPixel_Target.Enabled = true;
                Unused_outer_rows.Enabled = true;
                Unused_outer_cols.Enabled = true;
                PRNU_test_scan_size.Enabled = true;
                Hot_pix_exp_time.Enabled = true;
                Ave_dark_scan_size.Enabled = true;
                TotalColumns.Enabled = true;
                TotalRows.Enabled = true;
                //AddLimitsData.Enabled = true;
                //RemoveLimitsData.Enabled = true;
                //UpdateLimitsData.Enabled = true;
                //RefreshLimitsData.Enabled = true;
                ECONumber.Enabled = true;
                defectivePixelsPerROI.Enabled = true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public DefectsTestLimitsData ReturnDefectsTestLimit()
        {
            try
            {
                DefectsTestLimitsData defectsTestLimitsData = new DefectsTestLimitsData();
                defectsTestLimitsData.LimitsID = Convert.ToInt32(LimitsID.Text);
                defectsTestLimitsData.PRNUPositive = PRNUPositive.Value;
                defectsTestLimitsData.PRNUNegative = PRNUNegative.Value;
                defectsTestLimitsData.MaxTrap = Convert.ToInt32(MaxTrap.Value);
                defectsTestLimitsData.HotPixel = Convert.ToInt32(HotPixels.Value);
                defectsTestLimitsData.DeadPixel = Convert.ToInt32(DeadPixels.Value);
                defectsTestLimitsData.DarkPixel = Convert.ToInt32(DarkPixels.Value);
                defectsTestLimitsData.AveTrap = Convert.ToInt32(AveTrap.Value);
                defectsTestLimitsData.MaxDarkROI = Convert.ToInt32(MaxDarkROI.Value);
                defectsTestLimitsData.TotalNumDefects = Convert.ToInt32(TotalNumDefects.Value);
                defectsTestLimitsData.TotalNumClusters = Convert.ToInt32(TotalNumClusters.Value);
                defectsTestLimitsData.ClusterSize = Convert.ToInt32(clusterSize.Value);
                defectsTestLimitsData.AdjacentPixelsInCluster = Convert.ToInt32(adjacentPixelsInCluster.Value);
                defectsTestLimitsData.PercentAboveMean = Convert.ToInt32(percentAboveMean.Value);
                defectsTestLimitsData.PercentBelowMean = Convert.ToInt32(percentBelowMean.Value);
                defectsTestLimitsData.PercentAboveRows = Convert.ToInt32(percentAboveMeanRows.Value);
                defectsTestLimitsData.PercentBelowRows = Convert.ToInt32(percentBelowMeanRows.Value);
                defectsTestLimitsData.TotalColumns = Convert.ToInt32(TotalColumns.Value);
                defectsTestLimitsData.TotalRows = Convert.ToInt32(TotalRows.Value);
                defectsTestLimitsData.ROI_Xs = Convert.ToInt32(ROI_Xs.Value);
                defectsTestLimitsData.ROI_Ys = Convert.ToInt32(ROI_Ys.Value);
                defectsTestLimitsData.ROI_dXs = Convert.ToInt32(ROI_dXs.Value);
                defectsTestLimitsData.ROI_dYs = Convert.ToInt32(ROI_dYs.Value);
                defectsTestLimitsData.ROI_dXbs = Convert.ToInt32(ROI_dXbs.Value);
                defectsTestLimitsData.ROI_dYbs = Convert.ToInt32(ROI_dYbs.Value);
                defectsTestLimitsData.ROI_NDRO = Convert.ToInt32(ROI_NDRO.Value);
                defectsTestLimitsData.ROI_PixRate = Convert.ToInt32(ROI_PixRate.Value);
                defectsTestLimitsData.PRNU_LED = Convert.ToInt32(PRNU_LED.Value);
                defectsTestLimitsData.PRNU_Target = Convert.ToInt32(PRNU_Target.Value);
                defectsTestLimitsData.Trap_LED = Convert.ToInt32(Trap_LED.Value);
                defectsTestLimitsData.Trap_Target = Convert.ToInt32(Trap_Target.Value);
                defectsTestLimitsData.HotPixel_LED = Convert.ToInt32(HotPixel_LED.Value);
                defectsTestLimitsData.HotPixel_Target = Convert.ToInt32(HotPixel_Target.Value);
                defectsTestLimitsData.Unused_outer_rows = Convert.ToInt32(Unused_outer_rows.Value);
                defectsTestLimitsData.Unused_outer_cols = Convert.ToInt32(Unused_outer_cols.Value);
                defectsTestLimitsData.PRNU_test_scan_size = Convert.ToInt32(PRNU_test_scan_size.Value);
                defectsTestLimitsData.Hot_pix_exp_time = Convert.ToInt32(Hot_pix_exp_time.Value);
                defectsTestLimitsData.Ave_dark_scan_size = Convert.ToInt32(Ave_dark_scan_size.Value);
                defectsTestLimitsData.DriftROI = Convert.ToInt32(DriftROI.Value);
                defectsTestLimitsData.UserModified = Environment.UserName;
                defectsTestLimitsData.DefinedDate = DateTime.Now;
                defectsTestLimitsData.ECONumber = ECONumber.Text;
                defectsTestLimitsData.DefectivePixelsPerROI = Convert.ToInt32(defectivePixelsPerROI.Value);
                return defectsTestLimitsData;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }
        public void UpdateLimitsViewWithSpecificModel(DefectsTestLimitsData defectsTestLimitsData)
        {
            try
            {
                LimitsID.Text = defectsTestLimitsData.LimitsID.ToString();
                PRNUPositive.Value = defectsTestLimitsData.PRNUPositive;
                PRNUNegative.Value = defectsTestLimitsData.PRNUNegative;
                MaxTrap.Value = defectsTestLimitsData.MaxTrap;
                HotPixels.Value = defectsTestLimitsData.HotPixel;
                DeadPixels.Value = defectsTestLimitsData.DeadPixel;
                DarkPixels.Value = defectsTestLimitsData.DarkPixel;
                AveTrap.Value = defectsTestLimitsData.AveTrap;
                MaxDarkROI.Value = defectsTestLimitsData.MaxDarkROI;
                TotalNumDefects.Value = defectsTestLimitsData.TotalNumDefects;
                TotalNumClusters.Value = defectsTestLimitsData.TotalNumClusters;
                clusterSize.Value = defectsTestLimitsData.ClusterSize;
                adjacentPixelsInCluster.Value = defectsTestLimitsData.AdjacentPixelsInCluster;
                percentAboveMean.Value = defectsTestLimitsData.PercentAboveMean;
                percentBelowMean.Value = defectsTestLimitsData.PercentBelowMean;
                percentAboveMeanRows.Value = defectsTestLimitsData.PercentAboveRows;
                percentBelowMeanRows.Value = defectsTestLimitsData.PercentBelowRows;
                DriftROI.Value = defectsTestLimitsData.DriftROI;
                TotalColumns.Value = defectsTestLimitsData.TotalColumns;
                TotalRows.Value = defectsTestLimitsData.TotalRows;
                ROI_Xs.Value = defectsTestLimitsData.ROI_Xs;
                ROI_Ys.Value = defectsTestLimitsData.ROI_Ys;
                ROI_dXs.Value = defectsTestLimitsData.ROI_dXs;
                ROI_dYs.Value = defectsTestLimitsData.ROI_dYs;
                ROI_dXbs.Value = defectsTestLimitsData.ROI_dXbs;
                ROI_dYbs.Value = defectsTestLimitsData.ROI_dYbs;
                ROI_PixRate.Value = defectsTestLimitsData.ROI_PixRate;
                ROI_NDRO.Value = defectsTestLimitsData.ROI_NDRO;
                PRNU_LED.Value = defectsTestLimitsData.PRNU_LED;
                PRNU_Target.Value = defectsTestLimitsData.PRNU_Target;
                Trap_LED.Value = defectsTestLimitsData.Trap_LED;
                Trap_Target.Value = defectsTestLimitsData.Trap_Target;
                HotPixel_LED.Value = defectsTestLimitsData.HotPixel_LED;
                HotPixel_Target.Value = defectsTestLimitsData.HotPixel_Target;
                Unused_outer_rows.Value = defectsTestLimitsData.Unused_outer_rows;
                Unused_outer_cols.Value = defectsTestLimitsData.Unused_outer_cols;
                PRNU_test_scan_size.Value = defectsTestLimitsData.PRNU_test_scan_size;
                Hot_pix_exp_time.Value = defectsTestLimitsData.Hot_pix_exp_time;
                Ave_dark_scan_size.Value = defectsTestLimitsData.Ave_dark_scan_size;
                DefinedDate.Text = defectsTestLimitsData.DefinedDate.ToString();
                UserModified.Text = defectsTestLimitsData.UserModified;
                ECONumber.Text = defectsTestLimitsData.ECONumber;
                defectivePixelsPerROI.Value = defectsTestLimitsData.DefectivePixelsPerROI;
                //for Overall test report
                TestAppHelper.TotalNumDefects = Convert.ToInt32(TotalNumDefects.Value);
                TestAppHelper.TotalNumClusters = Convert.ToInt32(TotalNumClusters.Value);
                TestAppHelper.AveTrap = Convert.ToInt32(AveTrap.Value);
                TestAppHelper.MaxDarkROI = MaxDarkROI.Value;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
