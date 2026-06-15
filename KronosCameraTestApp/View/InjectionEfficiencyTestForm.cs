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
    public partial class InjectionEfficiencyTestForm : Form
    {
        public InjectionEfficiencyPresenter injectionEfficiencyPresenter { get; set; }
        // The once-per-class call to initialize the log object
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		ListExposureData listExposureData { get; set; }
		private CIDInterface cidInterface = null;
        private string userMode = string.Empty;
        bool engineeringLoginStatus = false;
        CancellationTokenSource cts { get; set; }
        ExposureSettings exposureSettings { get; set; }
        public CIDInterface CidInterface
        {
            get { return cidInterface; }
            set { cidInterface = value; }
        }
        bool injectionPerformanceTestStatus = false;
        public bool InjectionPerformanceTestStatus
        {
            get { return injectionPerformanceTestStatus; }
            set { injectionPerformanceTestStatus = value; }
        }      
        public InjectionEfficiencyTestForm()
        {
            InitializeComponent();
        }
        Form mdiParent = null;
        public InjectionEfficiencyTestForm(InjectionEfficiencyPresenter injectionEfficiencyPresenter, string UserMode, bool enggLoginStatus,
            CIDInterface cidInterface,  Form MDIParent)
        {
            try
            {
                InitializeComponent();
                this.injectionEfficiencyPresenter = injectionEfficiencyPresenter;
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
        private async void InjectionPerformanceTestForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (injectionEfficiencyPresenter.InjectionEfficiencyLimitsDataList.Count == 0 || TestAppHelper.UserModeChanged)  
                    await injectionEfficiencyPresenter.GetLimits();
                UpdateLimitsViewWithModelOnLoad();
                exposureSettings = new ExposureSettings("IET", userMode, engineeringLoginStatus, TestAppHelper.UserLoginName, "InjectionPerformanceData.xml", "InjectionPerformanceExposures");
                ultraExplorerBarContainerControl2.Controls.Add(exposureSettings);
                exposureSettings.GetData();
                injectionEfficiencyPresenter.ExposureDataList = exposureSettings.ExposureList;
                injectionEfficiencyPresenter.SubarrayDataModelList = exposureSettings.SubarrayList;
                numericEditNumOfExposures.Value = injectionEfficiencyPresenter.ExposureDataList.Count;
                if (userMode == "Manufacturing")
                {
                    EnableReadOnlyControlsManufacturingMode();
                }
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Loading Injection Perforamnce Test Form", "Loading Form", MessageBoxButtons.OK);
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
                injectionEfficiencyPresenter.InjectionEfficiencyLimitsData = injectionEfficiencyPresenter.InjectionEfficiencyLimitsDataList.
                    Where(limit => limit.LimitsID.ToString().Equals(ultraComboEditorLimitIDs.SelectedItem.DisplayText.ToString())).FirstOrDefault();
                InjectionEfficiencyLimit.Value = injectionEfficiencyPresenter.InjectionEfficiencyLimitsData.InjectionEffLimit;
                crossTalkThresholdLimit.Value = injectionEfficiencyPresenter.InjectionEfficiencyLimitsData.CrossTalkThresholdLimit;
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
                limtIDs = (from ecos in injectionEfficiencyPresenter.InjectionEfficiencyLimitsDataList select ecos.LimitsID).ToList();
                ultraComboEditorLimitIDs.DataSource = limtIDs;
                ultraComboEditorLimitIDs.SelectedIndex = 0;
                injectionEfficiencyPresenter.InjectionEfficiencyLimitsData = injectionEfficiencyPresenter.InjectionEfficiencyLimitsDataList.
                    Where(limit => limit.LimitsID.Equals(limtIDs[0])).FirstOrDefault();
                if (injectionEfficiencyPresenter.InjectionEfficiencyLimitsData != null)
                {
                    InjectionEfficiencyLimit.Value = injectionEfficiencyPresenter.InjectionEfficiencyLimitsData.InjectionEffLimit;
                    crossTalkThresholdLimit.Value = injectionEfficiencyPresenter.InjectionEfficiencyLimitsData.CrossTalkThresholdLimit;
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
                injectionEfficiencyPresenter.InjectionEfficiencyLimitsData = injectionEfficiencyPresenter.InjectionEfficiencyLimitsDataList.
                    FirstOrDefault(limit => limit.LimitsID.Equals(Convert.ToInt32(ultraComboEditorLimitIDs.SelectedItem.DisplayText)));
                //InjectionEfficiencyLimit.Value = injectionEfficiencyPresenter.InjectionEfficiencyLimitsData.InjectionEffLimit;
                injectionEfficiencyPresenter.InjectionEfficiencyLimitsData.InjectionEffLimit = InjectionEfficiencyLimit.Value;
                injectionEfficiencyPresenter.InjectionEfficiencyLimitsData.CrossTalkThresholdLimit = crossTalkThresholdLimit.Value;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }       
        private async void buttonInjectionPerformanceRun_Click(object sender, EventArgs e)
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
                    ledPassFail.Value = false;
                    ledPassFail.OffColor = Color.DarkGreen;
                    // Instantiate the CancellationTokenSource.  
                    cts = new CancellationTokenSource();
                    TestAppHelper.currentRunningTest = "InjectionEfficiency";
                   
                    buttonInjectionPerformanceRun.Enabled = false;
                    buttonInjectionPerformanceAbort.Enabled = true;
                    ClearCrossTalkResultsData();
                    waveformPlot1.ClearData();
                    EXP0WaveformPlot.ClearData();
                    EXP1WaveformPlot.ClearData();
                    EXP2WaveformPlot.ClearData();
                    EXP3WaveformPlot.ClearData();
                    EXP4WaveformPlot.ClearData();
                    chargeInjectionEfficiencyWaveformGraph.Refresh();
                    injectionPerformanceTestStatus = true;
                    injectionEfficiencyPresenter.ExposureDataList = exposureSettings.ExposureList;
                    injectionEfficiencyPresenter.SubarrayDataModelList = exposureSettings.SubarrayList;

                    if(enableInjEffTest.Checked)
                    {                        
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Injection Efficiency Test Processing....";
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 1;
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Start();
                        //Run InjectionPerformance Test
                        await RunInjectionPerformanceTest(cts.Token);
                        //Plot InjectionPerformance Data
                        await PlotInjectionPerformanceData(cts);
                    }
                   
                    if(enableCrossTalkTest.Checked)
                    {                        
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Cross Talk Test Processing....";
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 1;
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Start();
                        //Run Crosstalk Test
                        await RunCrossTalkTest(cts.Token);
                        //Plot Crosstalk Data
                        await PlotCrossTalkData(cts);
                    }                    

                    ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                    if (cts.IsCancellationRequested || !cidInterface.IsConnected())
                    {
                        if (!cidInterface.IsConnected())
                        {
                            TestAppHelper.noHeartBeatFromCamera();
                            cidInterface.AbortExposure();
                            cts.Cancel();
                            await ClearInjectionPerformanceGraphs();
                        }
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Injection Efficiency & Cross Talk Test Aborted";
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.ResetFillAppearance();
                        ledPassFail.OffColor = Color.DarkGreen;
                        RowXTalkPassFail.OffColor = Color.DarkGreen;
                        ColXTalkPassFail.OffColor = Color.DarkGreen;
                    }
                    else
                    {                       
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Injection Efficiency and/or Cross Talk Test Completed";
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 100;
                    }
                    buttonInjectionPerformanceAbort.Enabled = false;
                    buttonInjectionPerformanceRun.Enabled = true;
                    injectionPerformanceTestStatus = false;
                    injectionEfficiencyExplorerBar.Groups[1].Expanded = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public void ClearCrossTalkResultsData()
        { 
            meanBox1.Text = meanBox2.Text = meanBox3.Text = meanBox4.Text = meanBox5.Text = meanBox6.Text = meanBox7.Text = meanBox8.Text = meanBox9.Text =
                 Reference.Text = RowXTalk.Text = ColXTalk.Text = Sensitivity.Text = string.Empty;

            RowXTalkPassFail.Value = false;
            RowXTalkPassFail.OffColor = Color.DarkGreen;

            ColXTalkPassFail.Value = false;
            ColXTalkPassFail.OffColor = Color.DarkGreen;
        }
        private async Task RunInjectionPerformanceTest(CancellationToken ct)
        {
            try
            {
                log.Debug("Run Injection Performance test.");
                if (ct.IsCancellationRequested)
                    return;
                await injectionEfficiencyPresenter.runInjectionEfficiencyTest(cidInterface, ct, userMode, Convert.ToInt32(numericEditNumOfExposures.Value));
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task PlotInjectionPerformanceData(CancellationTokenSource ct)
        {
            try
            {
                if (ct.IsCancellationRequested)
                    return;
                await injectionEfficiencyPresenter.CalculateInjectionEfficiency(ct.Token);
                if (ct.IsCancellationRequested)
                    return;
                if (!cidInterface.IsConnected())
                {                    
                    return;
                }
                firstInjEXP0.Text = "0.0";
                firstInjEXP1.Text = "0.0";
                firstInjEXP2.Text = "0.0";
                firstInjEXP3.Text = "0.0";
                firstInjEXP4.Text = "0.0";
                secondInjEXP0.Text = "0.0";
                secondInjEXP1.Text = "0.0";
                secondInjEXP2.Text = "0.0";
                secondInjEXP3.Text = "0.0";
                secondInjEXP4.Text = "0.0";
                thirdInjEXP0.Text = "0.0";
                thirdInjEXP1.Text = "0.0";
                thirdInjEXP2.Text = "0.0";
                thirdInjEXP3.Text = "0.0";
                thirdInjEXP4.Text = "0.0";
                fourthInjEXP0.Text = "0.0";
                fourthInjEXP1.Text = "0.0";
                fourthInjEXP2.Text = "0.0";
                fourthInjEXP3.Text = "0.0";
                fourthInjEXP4.Text = "0.0";
                lastInjEXP0.Text = "0.0";
                lastInjEXP1.Text = "0.0";
                lastInjEXP2.Text = "0.0";
                lastInjEXP3.Text = "0.0";
                lastInjEXP4.Text = "0.0";
                EXP0WaveformPlot.PlotYAppend(injectionEfficiencyPresenter.EXP0PlotData);
                EXP1WaveformPlot.PlotYAppend(injectionEfficiencyPresenter.EXP1PlotData);
                EXP2WaveformPlot.PlotYAppend(injectionEfficiencyPresenter.EXP2PlotData);
                EXP3WaveformPlot.PlotYAppend(injectionEfficiencyPresenter.EXP3PlotData);
                EXP4WaveformPlot.PlotYAppend(injectionEfficiencyPresenter.EXP4PlotData);               
                initialEXP0TextBox.Text =  injectionEfficiencyPresenter.InitialExposureResult[0].ToString();
                initialEXP1TextBox.Text =  injectionEfficiencyPresenter.InitialExposureResult[1].ToString();
                initialEXP2TextBox.Text =  injectionEfficiencyPresenter.InitialExposureResult[2].ToString();
                initialEXP3TextBox.Text =  injectionEfficiencyPresenter.InitialExposureResult[3].ToString();
                initialEXP4TextBox.Text =  injectionEfficiencyPresenter.InitialExposureResult[4].ToString();
                firstInjEXP0.Text = injectionEfficiencyPresenter.FirstInjectionResultUI[0];
                firstInjEXP1.Text = injectionEfficiencyPresenter.FirstInjectionResultUI[1];
                firstInjEXP2.Text = injectionEfficiencyPresenter.FirstInjectionResultUI[2];
                firstInjEXP3.Text = injectionEfficiencyPresenter.FirstInjectionResultUI[3];
                firstInjEXP4.Text = injectionEfficiencyPresenter.FirstInjectionResultUI[4];
                secondInjEXP0.Text = injectionEfficiencyPresenter.SecondInjectionResultUI[0];
                secondInjEXP1.Text = injectionEfficiencyPresenter.SecondInjectionResultUI[1];
                secondInjEXP2.Text = injectionEfficiencyPresenter.SecondInjectionResultUI[2];
                secondInjEXP3.Text = injectionEfficiencyPresenter.SecondInjectionResultUI[3];
                secondInjEXP4.Text = injectionEfficiencyPresenter.SecondInjectionResultUI[4];
                thirdInjEXP0.Text = injectionEfficiencyPresenter.ThirdInjectionResultUI[0];
                thirdInjEXP1.Text = injectionEfficiencyPresenter.ThirdInjectionResultUI[1];
                thirdInjEXP2.Text = injectionEfficiencyPresenter.ThirdInjectionResultUI[2];
                thirdInjEXP3.Text = injectionEfficiencyPresenter.ThirdInjectionResultUI[3];
                thirdInjEXP4.Text = injectionEfficiencyPresenter.ThirdInjectionResultUI[4];
                fourthInjEXP0.Text = injectionEfficiencyPresenter.FourthInjectionResultUI[0];
                fourthInjEXP1.Text = injectionEfficiencyPresenter.FourthInjectionResultUI[1];
                fourthInjEXP2.Text = injectionEfficiencyPresenter.FourthInjectionResultUI[2];
                fourthInjEXP3.Text = injectionEfficiencyPresenter.FourthInjectionResultUI[3];
                fourthInjEXP4.Text = injectionEfficiencyPresenter.FourthInjectionResultUI[4];
                lastInjEXP0.Text = injectionEfficiencyPresenter.LastInjectionResultUI[0];
                lastInjEXP1.Text = injectionEfficiencyPresenter.LastInjectionResultUI[1];
                lastInjEXP2.Text = injectionEfficiencyPresenter.LastInjectionResultUI[2];
                lastInjEXP3.Text = injectionEfficiencyPresenter.LastInjectionResultUI[3];
                lastInjEXP4.Text = injectionEfficiencyPresenter.LastInjectionResultUI[4];
                buttonInjectionPerformanceRun.Enabled = true;
                buttonInjectionPerformanceAbort.Enabled = false;
                if (injectionEfficiencyPresenter.InjectionEfficiencyTestPassed)
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

        private async Task RunCrossTalkTest(CancellationToken ct)
        {
            try
            {
                log.Debug("Run Cross Talk test.");
                await injectionEfficiencyPresenter.GetCrossTalkData(cidInterface);
                if (ct.IsCancellationRequested)
                    return;
                await injectionEfficiencyPresenter.runCrossTalkTest(cidInterface, ct, 3);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task PlotCrossTalkData(CancellationTokenSource ct)
        {
            try
            {
                if (ct.IsCancellationRequested)
                    return;
                await injectionEfficiencyPresenter.CalculateCrossTalk(ct.Token);
                if (ct.IsCancellationRequested)
                    return;
                if (!cidInterface.IsConnected())
                    return;

                meanBox1.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox1")).Value.ToString();
                meanBox2.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox2")).Value.ToString();
                meanBox3.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox3")).Value.ToString();
                meanBox4.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox4")).Value.ToString();
                meanBox5.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox5")).Value.ToString();
                meanBox6.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox6")).Value.ToString();
                meanBox7.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox7")).Value.ToString();
                meanBox8.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox8")).Value.ToString();
                meanBox9.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("meanBox9")).Value.ToString();

                Reference.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("Reference")).Value.ToString();
                RowXTalk.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("RowCrosstalk")).Value.ToString();
                ColXTalk.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("ColumnCrosstalk")).Value.ToString();
                Sensitivity.Text = injectionEfficiencyPresenter.CrossTalkResultData.FirstOrDefault(c => c.Key.Equals("RelativeSensitivity")).Value.ToString();

                if (injectionEfficiencyPresenter.RowCrossTalkTestPassed)
                {
                    RowXTalkPassFail.Value = true;
                }                    
                else
                {
                    RowXTalkPassFail.Value = false;
                    RowXTalkPassFail.OffColor = Color.Red;
                }
                if (injectionEfficiencyPresenter.ColCrossTalkTestPassed)
                {
                    ColXTalkPassFail.Value = true;
                }                   
                else
                {  
                    ColXTalkPassFail.Value = false;
                    ColXTalkPassFail.OffColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task ClearInjectionPerformanceGraphs()
        {
            await Task.Run(() =>
            {
                meanWaveformPlot.ClearData();
                EXP0WaveformPlot.ClearData();
                EXP1WaveformPlot.ClearData();
                EXP2WaveformPlot.ClearData();
                EXP3WaveformPlot.ClearData();
                EXP4WaveformPlot.ClearData();
            });
        }
        private async void buttonInjectionPerformanceAbort_Click(object sender, EventArgs e)
        {
            try
            {
                if (cts != null)
                {
                    DialogResult result = MessageBox.Show("Are you sure to Abort Injection Efficiency Test?",
                           "Abort Injection Efficiency Test",
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        cidInterface.AbortExposure();
                        injectionPerformanceTestStatus = false;
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                        await ClearInjectionPerformanceGraphs();
                        cts.Cancel();
                        buttonInjectionPerformanceRun.Enabled = true;
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
        private void InjectionPerformanceTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }     
        private void numericEditNumOfExposures_ValueChanged(object sender, EventArgs e)
        {
            if (numericEditNumOfExposures.Value > injectionEfficiencyPresenter.ExposureDataList.Count)
            {
                string errstring = string.Format("Value cannot be greater than total exposure count '{0}'.", injectionEfficiencyPresenter.ExposureDataList.Count);
                MessageBox.Show(errstring, "Invalid exposure count", MessageBoxButtons.OK, MessageBoxIcon.Error);
                numericEditNumOfExposures.Value = injectionEfficiencyPresenter.ExposureDataList.Count;
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
        private void buttonUpdateLimit_Click(object sender, EventArgs e)
        {
            UpdateLimitsModelWithView();
        }
        private async void buttonRefreshLimit_Click(object sender, EventArgs e)
        {
            await injectionEfficiencyPresenter.GetLimits();
            UpdateLimitsViewWithModelOnLoad();
        }
    }
}
