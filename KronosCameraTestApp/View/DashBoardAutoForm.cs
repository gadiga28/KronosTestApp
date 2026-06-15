using Infragistics.Win.Misc;
using KronosCameraTestApp.View;
using KronosCameraTestApp.View.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace KronosCameraTestApp.View
{
    public partial class DashBoardAutoForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
          System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const int CP_NOCLOSE_BUTTON = 0x200;
        MeanVarianceUserControl meanVarianceTestUserControl;
        PhotoresponseUserControl photoresponseTestUserControl; 
        NoiseVsNDROUserControl noiseVsNDROTestUserControl;
        InjectionEfficiencyUserControl injectionPerformanceTestUserControl;
        DarkCurrentUserControl darkCurrentTestUserControl;
        DefectsUserControl defectsUserControl;
        ShutterDriveUserControl shutterDriveUserControl;
        RedBlueUserControl redBlueUserControl;
        FinalTestReportUserControl finalTestReportUserControl;
        string[] autoTestOrder = null;
        public DashBoardAutoForm(MeanVarianceUserControl mvtuc, DarkCurrentUserControl dduc,
                                    DefectsUserControl deuc, NoiseVsNDROUserControl nnuc, 
                                    InjectionEfficiencyUserControl ijuc, PhotoresponseUserControl pruc, ShutterDriveUserControl sduc,
                                    RedBlueUserControl rbuc, FinalTestReportUserControl ftuc, string[] AutoTestOrder)
        {
            InitializeComponent();
            Cursor.Current = Cursors.WaitCursor;
            this.meanVarianceTestUserControl = mvtuc;
            //this.linearityTestUserControl = ltuc;
            this.darkCurrentTestUserControl = dduc;
            this.defectsUserControl = deuc;
            this.noiseVsNDROTestUserControl = nnuc;
            this.injectionPerformanceTestUserControl = ijuc;
            this.photoresponseTestUserControl = pruc;
            this.shutterDriveUserControl = sduc;
            this.redBlueUserControl = rbuc;
            this.finalTestReportUserControl = ftuc;
            this.autoTestOrder = AutoTestOrder;
            Cursor.Current = Cursors.Default;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        private void DashBoardAutoForm_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.ControlBox = false;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                meanVarianceTestUserControl.Dock = DockStyle.Fill;
                darkCurrentTestUserControl.Dock = DockStyle.Fill;
                defectsUserControl.Dock = DockStyle.Fill;
                noiseVsNDROTestUserControl.Dock = DockStyle.Fill;
                photoresponseTestUserControl.Dock = DockStyle.Fill;
                shutterDriveUserControl.Dock = DockStyle.Fill;
                injectionPerformanceTestUserControl.Dock = DockStyle.Fill;
                redBlueUserControl.Dock = DockStyle.Fill;
                finalTestReportUserControl.Dock = DockStyle.Fill;
                ultraPanelMeanVariance.ClientArea.Controls.Add(meanVarianceTestUserControl);
                ultraPanelDarkCurrent.ClientArea.Controls.Add(darkCurrentTestUserControl);
                ultraPanelDefects.ClientArea.Controls.Add(defectsUserControl);
                ultraPanelReadNoise.ClientArea.Controls.Add(noiseVsNDROTestUserControl);
                ultraPanelPhotoresponse.ClientArea.Controls.Add(photoresponseTestUserControl);
                ultraPanelShutterDrive.ClientArea.Controls.Add(shutterDriveUserControl);
                ultraPanelInjectionPerf.ClientArea.Controls.Add(injectionPerformanceTestUserControl);
                ultraPanelRedBlue.ClientArea.Controls.Add(redBlueUserControl);
                ultraPanelFinalTestReport.ClientArea.Controls.Add(finalTestReportUserControl);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }         
        }
        private void ultraTileRedBlue_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (ultraTileRedBlue.State == TileState.Normal)
                    ultraTileRedBlue.State = TileState.Large;
                else
                    ultraTileRedBlue.State = TileState.Normal;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraTileDefectsDarkCurrent_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (ultraTileDarkCurrent.State == TileState.Normal)
                    ultraTileDarkCurrent.State = TileState.Large;
                else
                    ultraTileDarkCurrent.State = TileState.Normal;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraTileReadNoise_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (ultraTileReadNoise.State == TileState.Normal)
                    ultraTileReadNoise.State = TileState.Large;
                else
                    ultraTileReadNoise.State = TileState.Normal;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraTilePhotoresponse_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (ultraTilePhotoresponse.State == TileState.Normal)
                    ultraTilePhotoresponse.State = TileState.Large;
                else
                    ultraTilePhotoresponse.State = TileState.Normal;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraTileMeanVariance_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (ultraTileMeanVariance.State == TileState.Normal)
                    ultraTileMeanVariance.State = TileState.Large;
                else
                    ultraTileMeanVariance.State = TileState.Normal;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraTileInjectionPerformance_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (ultraTileInjectionPerformance.State == TileState.Normal)
                    ultraTileInjectionPerformance.State = TileState.Large;
                else
                    ultraTileInjectionPerformance.State = TileState.Normal;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraTileFinalTestReport_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (ultraTileFinalTestReport.State == TileState.Normal)
                    ultraTileFinalTestReport.State = TileState.Large;
                else
                    ultraTileFinalTestReport.State = TileState.Normal;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraTileShutterDrive_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (ultraTileShutterDrive.State == TileState.Normal)
                    ultraTileShutterDrive.State = TileState.Large;
                else
                    ultraTileShutterDrive.State = TileState.Normal;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraTileDefects_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (ultraTileDefects.State == TileState.Normal)
                    ultraTileDefects.State = TileState.Large;
                else
                    ultraTileDefects.State = TileState.Normal;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
