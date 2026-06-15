using Infragistics.Win.Misc;
using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.Presenter;
using KronosCameraTestApp.View.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thermo.Kronos.Instrument.Camera.Interface;
namespace KronosCameraTestApp.View
{
    public partial class NoiseVsNDROTestForm : Form
    {
        ExposureSettings exposureSettings { get; set; }
        public NoiseVsNDROPresenter noiseVsNDROPresenter { get; set; }
        // The once-per-class call to initialize the log object
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);		
		private CIDInterface cidInterface = null;
        private string userMode = string.Empty;
        bool engineeringLoginStatus = false;
        CancellationTokenSource cts { get; set; }       
        public CIDInterface CidInterface
        {
            get { return cidInterface; }
            set { cidInterface = value; }
        }
        bool noiseVsNDROTestStatus = false;
        public bool NoiseVsNDROTestStatus
        {
            get { return noiseVsNDROTestStatus; }
            set { noiseVsNDROTestStatus = value; }
        }
        Form mdiParent = null;
        public NoiseVsNDROTestForm()
        {
            InitializeComponent();
        }
        public NoiseVsNDROTestForm(NoiseVsNDROPresenter noiseVsNDROPresenter, string UserMode, bool enggLoginStatus,
            CIDInterface cidInterface, Form MDIParent)
        {
            try
            {
                InitializeComponent();
                this.noiseVsNDROPresenter = noiseVsNDROPresenter;
                this.userMode = UserMode;
                this.engineeringLoginStatus = enggLoginStatus;
                this.cidInterface = cidInterface;
                mdiParent = MDIParent;
            }          
            catch(Exception ex)
            {
                log.Error("Exception in"+ MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void NoiseVsNDROTestForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;               
                if (noiseVsNDROPresenter.NoiseVsNDROsLimitsDataList.Count == 0 || TestAppHelper.UserModeChanged)
                     await noiseVsNDROPresenter.GetLimits();
                UpdateLimitsViewWithModelOnLoad();
                exposureSettings = new ExposureSettings("RNT", userMode, engineeringLoginStatus, TestAppHelper.UserLoginName, "NoiseVsNDROData.xml", "NoiseVsNDROExposures");
                ultraExplorerBarContainerControl2.Controls.Add(exposureSettings);
                exposureSettings.GetData();
                noiseVsNDROPresenter.ExposureDataList = exposureSettings.ExposureList;
                noiseVsNDROPresenter.SubarrayDataModelList = exposureSettings.SubarrayList;
                numericEditNumOfExposures.Value = noiseVsNDROPresenter.ExposureDataList.Count;
               if (userMode == "Manufacturing")
                {
                    EnableReadOnlyControlsManufacturingMode();
                }
                this.Cursor = Cursors.Arrow;             
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Loading NoiseVsNDROs Test Form", "Loading Form", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void EnableReadOnlyControlsManufacturingMode()
        {
            try
            {
                NumExposuresGroupBox.Enabled = false;
                LimitsGroupBox.Enabled = false;                
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraComboEditorLimitIDs_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                noiseVsNDROPresenter.NoiseVsNDROSLimitsData = noiseVsNDROPresenter.NoiseVsNDROsLimitsDataList.
                    Where(limit => limit.LimitsID.ToString().Equals(ultraComboEditorLimitIDs.SelectedItem.DisplayText.ToString())).FirstOrDefault();
                SnglNoiseLimit.Value = Convert.ToDouble(noiseVsNDROPresenter.NoiseVsNDROSLimitsData.SnglNoiseMax);
                SnglNoiseUpperLimit.Value = Convert.ToDouble(noiseVsNDROPresenter.NoiseVsNDROSLimitsData.SnglNoiseUpperLimit);
                SnglNoiseLowerLimit.Value = Convert.ToDouble(noiseVsNDROPresenter.NoiseVsNDROSLimitsData.SnglNoiseLowerLimit);
                R2CorrelationLimit.Value = Convert.ToDouble(noiseVsNDROPresenter.NoiseVsNDROSLimitsData.RPower2Correlation);
                PowerFitLowerimit.Value = Convert.ToDouble(noiseVsNDROPresenter.NoiseVsNDROSLimitsData.PowerFitLower);
                PowerFitUpperLimit.Value = Convert.ToDouble(noiseVsNDROPresenter.NoiseVsNDROSLimitsData.PowerFitUpper);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateLimitsViewWithModelOnLoad()
        {
            try
            {
                List<int> limtIDs = new List<int>();
                limtIDs = (from limits in noiseVsNDROPresenter.NoiseVsNDROsLimitsDataList select limits.LimitsID).ToList();
                ultraComboEditorLimitIDs.DataSource = limtIDs;
                ultraComboEditorLimitIDs.SelectedIndex = 0;
                noiseVsNDROPresenter.NoiseVsNDROSLimitsData = noiseVsNDROPresenter.NoiseVsNDROsLimitsDataList.
                    Where(limit => limit.LimitsID.Equals(limtIDs[0])).FirstOrDefault();
                if (noiseVsNDROPresenter.NoiseVsNDROSLimitsData != null)
                {
                    SnglNoiseLimit.Value = Convert.ToDouble(noiseVsNDROPresenter.NoiseVsNDROSLimitsData.SnglNoiseMax);
                    SnglNoiseUpperLimit.Value = Convert.ToDouble(noiseVsNDROPresenter.NoiseVsNDROSLimitsData.SnglNoiseUpperLimit);
                    SnglNoiseLowerLimit.Value = Convert.ToDouble(noiseVsNDROPresenter.NoiseVsNDROSLimitsData.SnglNoiseLowerLimit);
                    R2CorrelationLimit.Value = Convert.ToDouble(noiseVsNDROPresenter.NoiseVsNDROSLimitsData.RPower2Correlation);
                    PowerFitLowerimit.Value = Convert.ToDouble(noiseVsNDROPresenter.NoiseVsNDROSLimitsData.PowerFitLower);
                    PowerFitUpperLimit.Value = Convert.ToDouble(noiseVsNDROPresenter.NoiseVsNDROSLimitsData.PowerFitUpper);
                }               
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateLimitsModelWithView()
        {
            try
            {
                noiseVsNDROPresenter.NoiseVsNDROSLimitsData = noiseVsNDROPresenter.NoiseVsNDROsLimitsDataList.
                    FirstOrDefault(limit => limit.LimitsID.Equals(Convert.ToInt32(ultraComboEditorLimitIDs.SelectedItem.DisplayText)));
                noiseVsNDROPresenter.NoiseVsNDROSLimitsData.SnglNoiseMax=Convert.ToInt32(SnglNoiseLimit.Value);
                noiseVsNDROPresenter.NoiseVsNDROSLimitsData.SnglNoiseUpperLimit = SnglNoiseUpperLimit.Value;
                noiseVsNDROPresenter.NoiseVsNDROSLimitsData.SnglNoiseLowerLimit = SnglNoiseLowerLimit.Value;
                noiseVsNDROPresenter.NoiseVsNDROSLimitsData.RPower2Correlation=Convert.ToDouble( R2CorrelationLimit.Value);
                noiseVsNDROPresenter.NoiseVsNDROSLimitsData.PowerFitLower=Convert.ToDouble(PowerFitLowerimit.Value);
                noiseVsNDROPresenter.NoiseVsNDROSLimitsData.PowerFitUpper = Convert.ToDouble(PowerFitUpperLimit.Value);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void buttonUpdateLimit_Click(object sender, EventArgs e)
        {
            UpdateLimitsModelWithView();
        }
        private async void buttonNDRORun_Click(object sender, EventArgs e)
        {
            try
            {
                if (((KronosTestAppMainForm)mdiParent).AutoTestStatus)
                {
                    MessageBox.Show("Auto Test is under process. Please wait until the test is completed");
                    return;
                }
                if (((KronosTestAppMainForm)mdiParent).AutoPreTestStatus)
                {
                    MessageBox.Show("PreTest is under process. Please wait until the test is completed");
                    return;
                }
                else if (cidInterface.IsConnected())
                {
                    if (Convert.ToInt32(numericEditNumOfExposures.Value) == 0 || Convert.ToInt32(numericEditNumOfExposures.Value) < 0)
                    {
                        MessageBox.Show("Exposure count should be greater than 0", "Invalid exposure count", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                    ((KronosTestAppMainForm)mdiParent).FirmwareManualTestLogsFile = "NoiseVsNDROs_ManualTestFWLog_" +
                        ((KronosTestAppMainForm)mdiParent).CameraSerialNumber.Trim('\0').TrimEnd() + "_" + DateTime.Now.ToString("yymmddThhmm") + ".txt";
                    ledPassFail.Value = false;
                    ledPassFail.OffColor = Color.DarkGreen;
                    SnglNoiseResult.Text = string.Empty;
                    NDRONoiseResult.Text = string.Empty;
                    R2CorrelationResult.Text = string.Empty;
                    BestFitPower.Text = string.Empty;
                    BestFitResult.Text = string.Empty;
                    R2CorrelationResult.Appearance.BorderColor = R2CorrelationResult.Appearance.BorderColor2 = Color.Black;
                    SnglNoiseResult.Appearance.BorderColor = SnglNoiseResult.Appearance.BorderColor2 = Color.Black;
                    // Instantiate the CancellationTokenSource.  
                    cts = new CancellationTokenSource();
                    TestAppHelper.currentRunningTest = "NoiseVsNDRO";
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 1;
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "NoiseVsNDRO Test Processing....";
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Start();
                    buttonNDRORun.Enabled = false;
                    buttonNDROAbort.Enabled = true;
                    this.measuredNoiseScatterPlot.ClearData();
                    this.log20ScatterPlot.ClearData();
                    readNoiseScatterGraph.Refresh();                
                    noiseVsNDROTestStatus = true;
                    noiseVsNDROPresenter.ExposureDataList = exposureSettings.ExposureList;
                    noiseVsNDROPresenter.SubarrayDataModelList = exposureSettings.SubarrayList;
                    //Run NoiseVsNDRO Test
                    await RunNoiseVsNDROTest(cts.Token);
                    //Plot NoiseVsNDRO Data
                    await PlotNoiseNDROData(cts);
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                    if (cts.IsCancellationRequested || !cidInterface.IsConnected())
                    {
                        if (!cidInterface.IsConnected())
                        {
                            TestAppHelper.noHeartBeatFromCamera();
                            cidInterface.AbortExposure();
                            cts.Cancel();
                            await ClearNoiseNDROGraphs();
                        }
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "NoiseVsNDRO Test Aborted";
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.ResetFillAppearance();
                        ledPassFail.OffColor = Color.DarkGreen;
                    }
                    else
                    {
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "NoiseVsNDRO Test Completed";
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 100;
                    }
                    buttonNDROAbort.Enabled = false;
                    buttonNDRORun.Enabled = true;
                    noiseVsNDROTestStatus = false;
                    noiseVsNDROFormExplorerBar.Groups[1].Expanded = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task RunNoiseVsNDROTest(CancellationToken ct)
        {
            try
            {
                log.Debug("Run NoiseVsNDRO test.");
                if (ct.IsCancellationRequested)
                    return;
                await noiseVsNDROPresenter.runNoiseNDROTest(cidInterface, ct, userMode, Convert.ToInt32(numericEditNumOfExposures.Value));
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task PlotNoiseNDROData(CancellationTokenSource ct)
        {
            try
            {
				if (ct.IsCancellationRequested)
					return;
				await noiseVsNDROPresenter.CalculateNoiseNDRO(ct.Token);
                if (ct.IsCancellationRequested)
                    return;
                if (!cidInterface.IsConnected())
                    return;
                double[] exposureNDROs = { 1, 2, 4, 8, 16, 32, 64, 128 };
                log20ScatterPlot.PlotXYAppend(exposureNDROs, noiseVsNDROPresenter.TheoryValue);
				readNoiseScatterGraph.PlotXYAppend(exposureNDROs, noiseVsNDROPresenter.Noise);
				buttonNDRORun.Enabled = true;
				buttonNDROAbort.Enabled = false;
				SnglNoiseResult.Text = String.Format("{0:0.00}", noiseVsNDROPresenter.SignalReadNoise);
                if (noiseVsNDROPresenter.SignalReadNoise >= noiseVsNDROPresenter.NoiseVsNDROSLimitsData.SnglNoiseLowerLimit && 
                    noiseVsNDROPresenter.SignalReadNoise <= noiseVsNDROPresenter.NoiseVsNDROSLimitsData.SnglNoiseUpperLimit)
                {
                    SnglNoiseResult.Appearance.BorderColor = SnglNoiseResult.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    SnglNoiseResult.Appearance.BorderColor = SnglNoiseResult.Appearance.BorderColor2 = Color.Red;
                }

                NDRONoiseResult.Text = String.Format("{0:0.00}", noiseVsNDROPresenter.NDROReadNoise); 
				R2CorrelationResult.Text = noiseVsNDROPresenter.R2Correlation.ToString(); 
                if(noiseVsNDROPresenter.R2Correlation >= noiseVsNDROPresenter.NoiseVsNDROSLimitsData.RPower2Correlation)
                {
                    R2CorrelationResult.Appearance.BorderColor = R2CorrelationResult.Appearance.BorderColor2 = Color.LimeGreen;
                }
                else
                {
                    R2CorrelationResult.Appearance.BorderColor = R2CorrelationResult.Appearance.BorderColor2 = Color.Red;
                }

                if (noiseVsNDROPresenter.NoiseVsNDROsTestPassed)
					ledPassFail.Value = true;
				else
				{
					ledPassFail.Value = false;
					ledPassFail.OffColor = Color.Red;
				}
			}
			catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void buttonNDROAbort_Click(object sender, EventArgs e)
        {
            try
            {
                if (cts != null)
                {
                    DialogResult result = MessageBox.Show("Are you sure to Abort NoiseVsNDRO Test?",
                           "Abort NoiseVsNDRO Test",
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {           
                        cidInterface.AbortExposure();
                        noiseVsNDROTestStatus = false;
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.ResetFillAppearance();
                        await ClearNoiseNDROGraphs();
                        cts.Cancel();
                        buttonNDRORun.Enabled = true;
                    }
                    else
                        return;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task ClearNoiseNDROGraphs()
        {
            await Task.Run(() =>
            {               
                this.measuredNoiseScatterPlot.ClearData();
                this.log20ScatterPlot.ClearData();
            });
        }      
        private void NoiseVsNDROTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }      
        private void numericEditNumOfExposures_ValueChanged(object sender, EventArgs e)
        {
            if (numericEditNumOfExposures.Value > noiseVsNDROPresenter.ExposureDataList.Count)
            {
                string errstring = string.Format("Value cannot be greater than total exposure count '{0}'.", noiseVsNDROPresenter.ExposureDataList.Count);
                MessageBox.Show(errstring, "Invalid exposure count", MessageBoxButtons.OK, MessageBoxIcon.Error);
                numericEditNumOfExposures.Value = noiseVsNDROPresenter.ExposureDataList.Count;
            }
        }
        private void ResetProgressBar()
        {
            try
            {
                if (((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value == 100)
                {
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = string.Empty;
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.ResetFillAppearance();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraExplorerBar1_Click(object sender, EventArgs e)
        {
            ResetProgressBar();
        }
        private void panel3_Click(object sender, EventArgs e)
        {
            ResetProgressBar();
        }
        private async void buttonRefreshLimit_Click(object sender, EventArgs e)
        {
            await noiseVsNDROPresenter.GetLimits();
            UpdateLimitsViewWithModelOnLoad();
        }             
        private void numericEditExposureInterval_AfterChangeValue(object sender, NationalInstruments.UI.AfterChangeNumericValueEventArgs e)
        {
        }
    }
}
