using Infragistics.Win.Misc;
using KronosCameraTestApp.Presenter;
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
using Thermo.Kronos.Instrument.Camera.Interface;

namespace KronosCameraTestApp.View
{
    public partial class DashBoard : Form
    {
       
        private string userMode = string.Empty;
        bool engineeringLoginStatus = false;
        string enggUserName = string.Empty;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
          System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public MeanVariancePresenter meanVariancePresenter { get; set; }
        public MeanVarianceTestForm meanVarianceTestForm;
        public NoiseVsNDROPresenter noiseVsNDROPresenter { get; set; }
        NoiseVsNDROTestForm noiseVsNDROTestForm;

        public PhotoresponsePresenter photoresponsePresenter { get; set; }
        PhotoresponseTestForm photoresponseTestForm;

        public InjectionEfficiencyPresenter injectionPerformancePresenter { get; set; }
        InjectionEfficiencyTestForm injectionPerformanceTestForm;

        public DarkCurrentPresenter darkCurrentPresenter { get; set; }
        DarkCurrentTestForm darkCurrentTestForm;

        public DefectsPresenter defectsPresenter { get; set; }
        DefectsTestForm defectsTestForm;
        public ShutterDrivePresenter shutterDrivePresenter { get; set; }
        ShutterDriveTestForm shutterDriveAndRS232TestForm;

        public RedBluePresenter redBluePresenter { get; set; }
        RedBlueTestForm redBlueTestForm;

        private CIDInterface cidInterface = null;
        Infragistics.Win.UltraWinStatusBar.UltraStatusBar statusBar = null;
        public CIDInterface CidInterface
        {
            get { return cidInterface; }
            set { cidInterface = value; }
        }
        public DashBoard()
        {
            InitializeComponent();           
        }
        public DashBoard(MeanVariancePresenter meanVariancePresenter, NoiseVsNDROPresenter noiseVsNDROPresenter, 
                         PhotoresponsePresenter photoresponsePresenter, InjectionEfficiencyPresenter injectionPerformancePresenter, 
                         DarkCurrentPresenter darkCurrentPresenter, DefectsPresenter defectsPresenter, ShutterDrivePresenter shutterDrivePresenter,
                         RedBluePresenter redBluePresenter, string UserMode, bool enggLoginStatus, string enggUserName,
                         CIDInterface cidInterface, Infragistics.Win.UltraWinStatusBar.UltraStatusBar statusBar)
        {
            InitializeComponent();
            this.meanVariancePresenter = meanVariancePresenter;
            this.photoresponsePresenter = photoresponsePresenter;
            this.injectionPerformancePresenter = injectionPerformancePresenter;
            this.noiseVsNDROPresenter = noiseVsNDROPresenter;
            this.darkCurrentPresenter = darkCurrentPresenter;
            this.defectsPresenter = defectsPresenter;
            this.shutterDrivePresenter = shutterDrivePresenter;
            this.redBluePresenter = redBluePresenter;
            this.userMode = UserMode;
            this.engineeringLoginStatus = enggLoginStatus;
            this.enggUserName = enggUserName;
            this.cidInterface = cidInterface;
            this.statusBar = statusBar;
        }

        private void DashBoard_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                meanVarianceTestForm = new MeanVarianceTestForm(meanVariancePresenter, userMode, engineeringLoginStatus, cidInterface, this.MdiParent);                
                meanVarianceTestForm.TopLevel = false;
                meanVarianceTestForm.Dock = DockStyle.Fill;
                ultraPanelMeanVariance.ClientArea.Controls.Add(meanVarianceTestForm);
                meanVarianceTestForm.Show();

                noiseVsNDROTestForm = new NoiseVsNDROTestForm(noiseVsNDROPresenter, userMode, engineeringLoginStatus, cidInterface, this.MdiParent);
                noiseVsNDROTestForm.TopLevel = false;
                noiseVsNDROTestForm.Dock = DockStyle.Fill;
                ultraPanelReadNoise.ClientArea.Controls.Add(noiseVsNDROTestForm);
                noiseVsNDROTestForm.Show();

                photoresponseTestForm = new PhotoresponseTestForm(photoresponsePresenter, userMode, engineeringLoginStatus, cidInterface, this.MdiParent);
                photoresponseTestForm.TopLevel = false;
                photoresponseTestForm.Dock = DockStyle.Fill;
                ultraPanelPhotoresponse.ClientArea.Controls.Add(photoresponseTestForm);
                photoresponseTestForm.Show();

                injectionPerformanceTestForm = new InjectionEfficiencyTestForm(injectionPerformancePresenter, userMode, engineeringLoginStatus, cidInterface, this.MdiParent);
                injectionPerformanceTestForm.TopLevel = false;
                injectionPerformanceTestForm.Dock = DockStyle.Fill;
                ultraPanelInjectionPerf.ClientArea.Controls.Add(injectionPerformanceTestForm);
                injectionPerformanceTestForm.Show();

                darkCurrentTestForm = new DarkCurrentTestForm(darkCurrentPresenter, userMode, engineeringLoginStatus, cidInterface, this.MdiParent);
                darkCurrentTestForm.TopLevel = false;
                darkCurrentTestForm.Dock = DockStyle.Fill;
                ultraPanelDarkCurrent.ClientArea.Controls.Add(darkCurrentTestForm);
                darkCurrentTestForm.Show();

                defectsTestForm = new DefectsTestForm(defectsPresenter, userMode, engineeringLoginStatus, cidInterface, this.MdiParent);
                defectsTestForm.TopLevel = false;
                defectsTestForm.Dock = DockStyle.Fill;
                ultraPanelDefects.ClientArea.Controls.Add(defectsTestForm);
                defectsTestForm.Show();

                shutterDriveAndRS232TestForm = new ShutterDriveTestForm(shutterDrivePresenter, userMode, engineeringLoginStatus, cidInterface, this.MdiParent);
                shutterDriveAndRS232TestForm.TopLevel = false;
                shutterDriveAndRS232TestForm.Dock = DockStyle.Fill;
                ultraPanelShutterDrive.ClientArea.Controls.Add(shutterDriveAndRS232TestForm);
                shutterDriveAndRS232TestForm.Show();


                redBlueTestForm = new RedBlueTestForm(redBluePresenter, userMode, engineeringLoginStatus, cidInterface, this.MdiParent);
                redBlueTestForm.TopLevel = false;
                redBlueTestForm.Dock = DockStyle.Fill;
                ultraPanelRedBlue.ClientArea.Controls.Add(redBlueTestForm);
                redBlueTestForm.Show();

                ultraTilePanel1.Tiles["ultraTileRedBlue"].SetPositionInNormalMode(new Point(2, 1), false);

                
                Cursor.Current = Cursors.Arrow;
            }
           
            catch(Exception ex)
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
            catch(Exception ex)
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
            catch(Exception ex)
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
            catch(Exception ex)
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

        private void ultraTileInjectionPerf_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (ultraTileInjectionPerf.State == TileState.Normal)
                    ultraTileInjectionPerf.State = TileState.Large;
                else
                    ultraTileInjectionPerf.State = TileState.Normal;
             }           
            catch(Exception ex)
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

        private void ultraTileCrossTalk_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (ultraTileCrossTalk.State == TileState.Normal)
                    ultraTileCrossTalk.State = TileState.Large;
                else
                    ultraTileCrossTalk.State = TileState.Normal;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }

        private void ultraTileChargeTransfer_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (ultraTileChargeTransfer.State == TileState.Normal)
                    ultraTileChargeTransfer.State = TileState.Large;
                else
                    ultraTileChargeTransfer.State = TileState.Normal;
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
