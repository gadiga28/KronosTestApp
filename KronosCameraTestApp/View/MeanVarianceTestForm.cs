using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinToolTip;
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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Thermo.Kronos.Instrument.Camera.Interface;
namespace KronosCameraTestApp.View
{
    public partial class MeanVarianceTestForm : Form
    {
        public MeanVariancePresenter meanVariancePresenter { get; set; }
        // The once-per-class call to initialize the log object
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CIDInterface cidInterface = null;      
        private string userMode = string.Empty;
        bool engineeringLoginStatus = false;
        string enggUserName = string.Empty;      
		CancellationTokenSource cts { get; set; }
        public CIDInterface CidInterface
        {
            get { return cidInterface; }
            set { cidInterface = value; }
        }      
        bool meanVarianceTestStatus = false;
        public bool MeanVarianceTestStatus
        {
            get { return meanVarianceTestStatus; }
            set { meanVarianceTestStatus = value; }
        }      
        public MeanVarianceTestForm()
        {
            InitializeComponent();
        }
        Form mdiParent = null;
        ExposureSettings exposureSettings { get; set; }
        public MeanVarianceTestForm(MeanVariancePresenter meanVariancePresenter, string UserMode, bool enggLoginStatus, 
            CIDInterface cidInterface, Form MDIParent)
        {
            try
            {
                InitializeComponent();
                this.meanVariancePresenter = meanVariancePresenter;
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
        private async void MeanVarianceTestForm_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (meanVariancePresenter.MeanVarianceTestLimitsDataList.Count == 0 || TestAppHelper.UserModeChanged)
                    await meanVariancePresenter.GetMVLimits();             
                UpdateLimitsViewWithModelOnLoad();
                exposureSettings = new ExposureSettings("MVT", userMode, engineeringLoginStatus, TestAppHelper.UserLoginName, "MeanVarianceData.xml", "MeanVarianceExposures");
                ultraExplorerBarContainerControl3.Controls.Add(exposureSettings);
                exposureSettings.GetData();
                meanVariancePresenter.ExposureDataList = exposureSettings.ExposureList;
                meanVariancePresenter.SubarrayDataModelList = exposureSettings.SubarrayList;
                meanVariancePresenter.ExposureDataModel = exposureSettings.exposureDataPresenter.ExposureDataModel;
                meanVariancePresenter.SubarrayDataModel = exposureSettings.exposureDataPresenter.SubarrayDataModel;
                numericEditNumOfExposures.Value = meanVariancePresenter.ExposureDataList.Count;
                if (userMode == "Manufacturing")
                {
                    EnableReadOnlyControlsManufacturingMode();
                }
                Cursor.Current = Cursors.Arrow;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error Loading Mean Variance Test Form", "Loading Form", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }          
        private void EnableReadOnlyControlsManufacturingMode()
        {
            try
            {
                NumExposuresGroupBox.Enabled = false;
                LimitsGroupBox.Enabled = false;
                //ultraExplorerBarContainerControl3.Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void buttonMVRun_Click(object sender, EventArgs e)
        {           
            try
            {
                if (((KronosTestAppMainForm)mdiParent).AutoTestStatus)
                {
                    MessageBox.Show("Auto Test is under process. Please wait until the test is completed");
                    return;
                }
                if (((KronosTestAppMainForm)mdiParent).AutoTestStatus)// (((KronosTestAppMainForm)this.MdiParent).AutoPreTestStatus)
                {
                    MessageBox.Show("PreTest is under process. Please wait until the test is completed");
                    return;
                }
                else if (cidInterface.IsConnected())
                {                   
                    if (Convert.ToInt32(numericEditNumOfExposures.Value) <= 0)
                    {
                        MessageBox.Show("Exposure count should be greater than 0", "Invalid exposure count", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                    ((KronosTestAppMainForm)mdiParent).FirmwareManualTestLogsFile = "MeanVariance_ManualTestFWLog_" +
                        ((KronosTestAppMainForm)mdiParent).CameraSerialNumber.Trim('\0').TrimEnd() + "_" + DateTime.Now.ToString("yymmddThhmm") + ".txt";
                    ledPassFail.Value = false;
                    ledPassFail.OffColor = Color.DarkGreen;
                    labelGain.Appearance.BorderColor2 = labelGain.Appearance.BorderColor = Color.Black;
                    labelGain.Text = string.Empty;
                    // Instantiate the CancellationTokenSource.  
                    cts = new CancellationTokenSource();
                    TestAppHelper.currentRunningTest = "MeanVariance";
                    ((KronosTestAppMainForm)mdiParent).ResetAutoManualStatusBarPanel();
                   ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 1;
                   ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Mean Variance Test Processing....";
                   ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Start();
                    buttonMVRun.Enabled = false;
                    buttonMVAbort.Enabled = true;
                    meanVarianceScatterPlot.ClearData();
                    meanVarianceScatterGraph.ClearData();
                    meanVarianceScatterGraph.Refresh();
                    meanofROI1minus2Plot.ClearData();
                    varianceOfROI1minus2Plot.ClearData();
                    linearCurveFitOfVarPlot.ClearData();
                    meanVarianceWaveformGraph.ClearData();
                    meanVarianceWaveformGraph.Refresh();
                    meanVarianceTestStatus = true;
                    meanVariancePresenter.ExposureDataList = exposureSettings.ExposureList;
                    meanVariancePresenter.SubarrayDataModelList = exposureSettings.SubarrayList;
                    meanVariancePresenter.ExposureDataModel = exposureSettings.exposureDataPresenter.ExposureDataModel;
                    meanVariancePresenter.SubarrayDataModel = exposureSettings.exposureDataPresenter.SubarrayDataModel;
                    meanVariancePresenter.MeanVarianceRepeats = Convert.ToInt32(numTrials.Value);
                    //Run Mean Variance Test
                    await RunMVTest(cts.Token);
                    //Plot Mean Variance Data
                    await PlotMVData(cts);                               
                  
                    if (cts.IsCancellationRequested || !cidInterface.IsConnected())
                    {                       
                        if(!cidInterface.IsConnected())
                        {
                            TestAppHelper.noHeartBeatFromCamera();
                            cidInterface.AbortExposure(); 
                            cts.Cancel();                          
                            await ClearMVGraphs();                            
                        }
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Mean Variance Test Aborted";
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.ResetFillAppearance();
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                        ledPassFail.OffColor = Color.DarkGreen;
                        labelGain.Text = string.Empty;
                    }
                    else
                    {
                        labelGain.Text = meanVariancePresenter.Gain.ToString("0.00");
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["MarqueeStatus"].Text = "Mean Variance Test Completed";
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 100;
                    }                   
                    buttonMVAbort.Enabled = false;
                    buttonMVRun.Enabled = true;
                    meanVarianceTestStatus = false;
                    meanVarianceFormExplorerBar.Groups[1].Expanded = true;
                }
            }
            catch (Exception ex)
            {
                //"Internal Error";
                buttonMVAbort.Enabled = false;
                buttonMVRun.Enabled = true;
                meanVarianceTestStatus = false;
                meanVarianceFormExplorerBar.Groups[1].Expanded = true;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task RunMVTest(CancellationToken ct)
        {
            try
            {
                log.Debug("Run Mean Variance test.");
                ledPassFail.Value = false;
                await meanVariancePresenter.runMeanVarianceTest(cidInterface, ct,userMode, Convert.ToInt32(numericEditNumOfExposures.Value));               
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                //"Internal Error";
                buttonMVAbort.Enabled = false;
                buttonMVRun.Enabled = true;
                meanVarianceTestStatus = false;
                meanVarianceFormExplorerBar.Groups[1].Expanded = true;
            }
        }
        private async Task PlotMVData(CancellationTokenSource ct)
        {
            try
            {
                if (ct.IsCancellationRequested)
                    return;
                await meanVariancePresenter.CalculateMeanVariance(ct.Token);
                if (ct.IsCancellationRequested)
                    return;
                if (!cidInterface.IsConnected())
                    return;                  
                if (meanVariancePresenter.CorrectedMeanNew != null && meanVariancePresenter.CorrectedVarianceNew != null)
                    meanVarianceScatterGraph.PlotXY(meanVariancePresenter.CorrectedMeanNew, meanVariancePresenter.CorrectedVarianceNew);
                if (meanVariancePresenter.LinearCurveFit != null)
                    linearCurveFitOfVarPlot.PlotY(meanVariancePresenter.LinearCurveFit);
                labelGain.Text = meanVariancePresenter.Gain.ToString("0.00");
                buttonMVRun.Enabled = true;
                buttonMVAbort.Enabled = false;
                if (meanVariancePresenter.MeanVarianceTestPassed)
                {
                    ledPassFail.Value = true;
                    labelGain.Appearance.BorderColor2 = labelGain.Appearance.BorderColor = Color.LimeGreen;
                }                   
                else
                {
                    ledPassFail.Value = false;
                    ledPassFail.OffColor = labelGain.Appearance.BorderColor2 = labelGain.Appearance.BorderColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                //"Internal Error";
                buttonMVAbort.Enabled = false;
                buttonMVRun.Enabled = true;
                meanVarianceTestStatus = false;
                meanVarianceFormExplorerBar.Groups[1].Expanded = true;
            }
        }            
        private async void buttonMVAbort_Click(object sender, EventArgs e)
        {
            try
            {
                if (cts != null)
                {
                    DialogResult result = MessageBox.Show("Are you sure to Abort Mean Variance Test?",
                           "Abort Mean Variance Test",
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {                                  
                        cidInterface.AbortExposure();
                        meanVarianceTestStatus = false;                      
                        cts.Cancel();
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                        ((KronosTestAppMainForm)mdiParent).ultraStatusBar1.Panels["TestProgress"].ProgressBarInfo.ResetFillAppearance();
                        await ClearMVGraphs();
                        buttonMVRun.Enabled = true;
                    }
                    else
                        return;
                }  
            }
            catch(Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task ClearMVGraphs()
        {
            await Task.Run(() =>
            {
                meanofROI1minus2Plot.ClearData();
                varianceOfROI1minus2Plot.ClearData();
                linearCurveFitOfVarPlot.ClearData();
                meanVarianceScatterGraph.ClearData();
            });
        }           
        private void MeanVarianceTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
        private void numericEditNumOfExposures_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (numericEditNumOfExposures.Value > meanVariancePresenter.ExposureDataList.Count)
                {
                    string errstring = string.Format("Value cannot be greater than total exposure count '{0}'.", meanVariancePresenter.ExposureDataList.Count);
                    MessageBox.Show(errstring, "Invalid exposure count", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    numericEditNumOfExposures.Value = meanVariancePresenter.ExposureDataList.Count;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
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
        private void panel2_Click(object sender, EventArgs e)
        {
            ResetProgressBar();
        }
        private void meanVarianceScatterGraph_Click(object sender, EventArgs e)
        {
            //ResetProgressBar();
        }
        private void meanVarianceWaveformGraph_Click(object sender, EventArgs e)
        {
            //ResetProgressBar();
        }
        private void ultraComboEditorMVLimitIDs_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                meanVariancePresenter.MeanVarianceTestLimitsData = meanVariancePresenter.MeanVarianceTestLimitsDataList.Where
                    (limit => limit.LimitsID.ToString().Equals(ultraComboEditorLimitIDs.SelectedItem.DisplayText.ToString())).FirstOrDefault();               
                ConversionFactorNominalLimit.Value = Convert.ToDecimal(meanVariancePresenter.MeanVarianceTestLimitsData.ConversionFactorNominal);
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
                limtIDs = (from ecos in meanVariancePresenter.MeanVarianceTestLimitsDataList select ecos.LimitsID).ToList();
                ultraComboEditorLimitIDs.DataSource = limtIDs;
                ultraComboEditorLimitIDs.SelectedIndex = 0;
                meanVariancePresenter.MeanVarianceTestLimitsData = meanVariancePresenter.MeanVarianceTestLimitsDataList.
                    Where(limit => limit.LimitsID.Equals(limtIDs[0])).FirstOrDefault();
                if (meanVariancePresenter.MeanVarianceTestLimitsData != null)
                {
                    ConversionFactorNominalLimit.Value = Convert.ToDecimal(meanVariancePresenter.MeanVarianceTestLimitsData.ConversionFactorNominal);
                    ConversionFactorLowerLimit.Value = Convert.ToDecimal(meanVariancePresenter.MeanVarianceTestLimitsData.ConversionFactorLowerLimit);
                    ConversionFactorUpperLimit.Value = Convert.ToDecimal(meanVariancePresenter.MeanVarianceTestLimitsData.ConversionFactorUpperLimit);
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
                meanVariancePresenter.MeanVarianceTestLimitsData = meanVariancePresenter.MeanVarianceTestLimitsDataList.
                    FirstOrDefault(limit => limit.LimitsID.Equals(Convert.ToInt32(ultraComboEditorLimitIDs.SelectedItem.DisplayText)));
                meanVariancePresenter.MeanVarianceTestLimitsData.ConversionFactorNominal = Convert.ToDouble(ConversionFactorNominalLimit.Value);
                meanVariancePresenter.MeanVarianceTestLimitsData.ConversionFactorLowerLimit = Convert.ToDouble(ConversionFactorLowerLimit.Value);
                meanVariancePresenter.MeanVarianceTestLimitsData.ConversionFactorUpperLimit = Convert.ToDouble(ConversionFactorUpperLimit.Value);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            UpdateLimitsModelWithView();
        }
        private async void buttonRefreshLimit_Click(object sender, EventArgs e)
        {
            await meanVariancePresenter.GetMVLimits();
            UpdateLimitsViewWithModelOnLoad();
        }      
        private void numTrials_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (numTrials.Value > 10)
                {
                    MessageBox.Show("Value cannot be greater than total 10", "Invalid trials count", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    numTrials.Value = 10;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
