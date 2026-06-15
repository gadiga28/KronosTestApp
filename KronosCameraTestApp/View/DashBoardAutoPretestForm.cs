using Infragistics.Win.Misc;
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
    public partial class DashBoardAutoPretestForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DarkCurrentUserControl darkCurrentTestUserControl;
        DefectsUserControl defectsUserControl;
        RedBlueUserControl redBlueUserControl;
        MeanVarianceUserControl meanVarianceTestUserControl;
        PhotoresponseUserControl photoresponseTestUserControl;
        NoiseVsNDROUserControl noiseVsNDROTestUserControl;
        InjectionEfficiencyUserControl injectionPerformanceTestUserControl;
        ShutterDriveUserControl shutterDriveUserControl;
        FinalTestReportUserControl finalTestReportUserControl;
        string[] selectedPreTests;
        public DashBoardAutoPretestForm(string[] SelectedPreTests, DarkCurrentUserControl dduc, DefectsUserControl deuc,RedBlueUserControl rbuc,MeanVarianceUserControl mvtuc, 
                                        NoiseVsNDROUserControl nnuc, 
                                        InjectionEfficiencyUserControl ijuc, PhotoresponseUserControl pruc, ShutterDriveUserControl sduc, FinalTestReportUserControl ftruc)
        {
            Application.UseWaitCursor = true;           
            InitializeComponent();
            this.selectedPreTests = SelectedPreTests;
            Cursor.Current = Cursors.WaitCursor;
            foreach (string str in SelectedPreTests)
            {
                switch (str)
                {
                    case "RBT":
                        this.redBlueUserControl = rbuc;
                        break;
                    case "DDT":
                        this.darkCurrentTestUserControl = dduc;
                        break;
                    case "DET":
                        this.defectsUserControl = deuc;
                        break;
                    case "MVT":
                        this.meanVarianceTestUserControl = mvtuc;
                        break;
                    case "RNT":
                        this.noiseVsNDROTestUserControl = nnuc;
                        break;
                    case "PRT":
                        this.photoresponseTestUserControl = pruc;
                        break;
                    case "IET":
                        this.injectionPerformanceTestUserControl = ijuc;
                        break;
                    case "SDT":
                        this.shutterDriveUserControl = sduc;
                        break;
                }
            }
            this.finalTestReportUserControl = ftruc;
            Cursor.Current = Cursors.Default;
            Application.UseWaitCursor = false;
        }
        private void DashBoardAutoPretestForm_Load(object sender, EventArgs e)
        {
            try
            {               
                Application.UseWaitCursor = true;
                foreach (string str in selectedPreTests)
                {
                    switch (str)
                    {
                        case "RBT":
                            redBlueUserControl.Dock = DockStyle.Fill;
                            ultraPanelRedBluePreTest.ClientArea.Controls.Add(redBlueUserControl);
                            break;
                        case "DDT":
                            darkCurrentTestUserControl.Dock = DockStyle.Fill;
                            ultraPanelDarkCurrentPreTest.ClientArea.Controls.Add(darkCurrentTestUserControl);
                            break;
                        case "DET":
                            defectsUserControl.Dock = DockStyle.Fill;
                            ultraPanelDefectsPreTest.ClientArea.Controls.Add(defectsUserControl);
                            break;
                        case "MVT":
                            meanVarianceTestUserControl.Dock = DockStyle.Fill;
                            ultraPanelMeanVariance.ClientArea.Controls.Add(meanVarianceTestUserControl);
                            break;
                        case "RNT":
                            noiseVsNDROTestUserControl.Dock = DockStyle.Fill;
                            ultraPanelReadNoise.ClientArea.Controls.Add(noiseVsNDROTestUserControl);
                            break;
                        case "PRT":
                            photoresponseTestUserControl.Dock = DockStyle.Fill;
                            ultraPanelPhotoresponse.ClientArea.Controls.Add(photoresponseTestUserControl);
                            break;
                        case "IET":
                            injectionPerformanceTestUserControl.Dock = DockStyle.Fill;
                            ultraPanelInjectionPerf.ClientArea.Controls.Add(injectionPerformanceTestUserControl);
                            break;
                        case "SDT":
                            shutterDriveUserControl.Dock = DockStyle.Fill;
                            ultraPanelShutterDrive.ClientArea.Controls.Add(shutterDriveUserControl);
                            break;
                    }
                }
                finalTestReportUserControl.Dock = DockStyle.Fill;    
                ultraPanelPreTestReport.ClientArea.Controls.Add(finalTestReportUserControl);
                Application.UseWaitCursor = false;
            }
            catch(Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                Application.UseWaitCursor = false;
            }
        }
        private void ultraTileRedBluePreTest_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (ultraTileRedBluePreTest.State == TileState.Normal)
                    ultraTileRedBluePreTest.State = TileState.Large;
                else
                    ultraTileRedBluePreTest.State = TileState.Normal;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraTileDefectsDarkCurrentPreTest_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (ultraTileDarkCurrentPreTest.State == TileState.Normal)
                    ultraTileDarkCurrentPreTest.State = TileState.Large;
                else
                    ultraTileDarkCurrentPreTest.State = TileState.Normal;
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
        private void ultraTileDefectsPreTest_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (ultraTileDefectsPreTest.State == TileState.Normal)
                    ultraTileDefectsPreTest.State = TileState.Large;
                else
                    ultraTileDefectsPreTest.State = TileState.Normal;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraTilePreTestReport_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (ultraTilePreTestReport.State == TileState.Normal)
                    ultraTilePreTestReport.State = TileState.Large;
                else
                    ultraTilePreTestReport.State = TileState.Normal;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraTileShutterDrive_StateChanged(object sender, TileStateChangedEventArgs e)
        {
        }
    }
}
