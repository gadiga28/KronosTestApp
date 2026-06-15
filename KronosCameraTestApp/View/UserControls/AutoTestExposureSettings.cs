using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infragistics.Win.Misc;
using System.Reflection;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.Presenter;
using Thermo.Kronos.Instrument.Camera.Interface;

namespace KronosCameraTestApp.View.UserControls
{
    public partial class AutoTestExposureSettings : UserControl
    {
        private int currentRecord = 0;
        public int currentSubarrayRecord = 0;
        string exposureType;
        private CIDInterface cidInterface = null;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
          System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ExposureDataPresenter exposureDataPresenter { get; set; }
        public List<ExposureDataModel> ExposureList
        {
            get { return exposureDataPresenter.ExposureDataList; }
            set
            {
                exposureDataPresenter.ExposureDataList = value;
            }
        }
        public List<SubarrayDataModel> SubarrayList
        {
            get { return exposureDataPresenter.SubarrayDataModelList; }
            set
            {
                exposureDataPresenter.SubarrayDataModelList = value;
            }
        }
        public AutoTestExposureSettings(string exposureType, CIDInterface cidInterface)
        {
            InitializeComponent();
            currentSubarrayRecord = 0;
            currentRecord = 0;
            this.exposureType = exposureType;
            this.cidInterface = cidInterface;
            exposureDataPresenter = new ExposureDataPresenter(exposureType);
        }
        public void UpdateExposureView(int currentRecord)
        {
            try
            {
                if (currentRecord >= 0 && exposureDataPresenter.ExposureDataList.Count() > 0 && currentRecord < exposureDataPresenter.ExposureDataList.Count())
                {
                    //labelNumofExposures.Text = "Total # of Exposures: " + exposureDataPresenter.ExposureDataList.Count().ToString();
                    //ultraLabelNumSubarrays.Text = "Total # of Subarrays:" + exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas.Count.ToString();


                    textBoxExposureName.Text = exposureDataPresenter.ExposureDataList[currentRecord].ExposureName.ToString();

                    textBoxDelay.Text = exposureDataPresenter.ExposureDataList[currentRecord].GlobalInjectDelay.ToString();
                    textBoxGlobalInject.Text = exposureDataPresenter.ExposureDataList[currentRecord].GlobalInject.ToString();

                    textBoxExposureInterval.Text = exposureDataPresenter.ExposureDataList[currentRecord].ExposureInterval.ToString();
                    textBoxNDROs.Text = exposureDataPresenter.ExposureDataList[currentRecord].ExposureNDROS.ToString();
                    textBoxExposureXo.Text = exposureDataPresenter.ExposureDataList[currentRecord].ExposureRegionXo.ToString();
                    textBoxExposuredX.Text = exposureDataPresenter.ExposureDataList[currentRecord].ExposureRegiondX.ToString();
                    textBoxExposureYo.Text = exposureDataPresenter.ExposureDataList[currentRecord].ExposureRegionYo.ToString();
                    textBoxExposuredY.Text = exposureDataPresenter.ExposureDataList[currentRecord].ExposureRegiondY.ToString();


                    ultraCheckEditorLED1.Checked = exposureDataPresenter.ExposureDataList[currentRecord].LED1Enabled;
                    ultraCheckEditorLED2.Checked = exposureDataPresenter.ExposureDataList[currentRecord].LED2Enabled;
                    ultraCheckEditorLED3.Checked = exposureDataPresenter.ExposureDataList[currentRecord].LED3Enabled;
                    ultraCheckEditorShutterEnabled.Checked = exposureDataPresenter.ExposureDataList[currentRecord].ShutterEnabled;

                    textBoxLEDOn.Text = exposureDataPresenter.ExposureDataList[currentRecord].LEDOnTime.ToString();
                    textBoxLEDOff.Text = exposureDataPresenter.ExposureDataList[currentRecord].LEDOffTime.ToString();
                    textBoxLEDFlash.Text = exposureDataPresenter.ExposureDataList[currentRecord].LEDFlashes.ToString();
                    ultraCheckEditorAutoBiasFPN.Checked = Convert.ToBoolean(exposureDataPresenter.ExposureDataList[currentRecord].AutoBiasFPNEnabled);

                    //fpn
                    if (exposureDataPresenter.ExposureDataList[currentRecord].ExposureFPN == 0)
                    {
                        ultraRadioButtonFPNNone.Checked = true;
                    }
                    else if (exposureDataPresenter.ExposureDataList[currentRecord].ExposureFPN == 1)
                    {
                        ultraRadioButtonFPNRunTime.Checked = true;
                    }
                    else if (exposureDataPresenter.ExposureDataList[currentRecord].ExposureFPN == 3)
                    {
                        ultraRadioButtonFPNStored.Checked = true;
                    }

                    ultraCheckEditorExposedFrame.Checked = exposureDataPresenter.ExposureDataList[currentRecord].FullFrameEnabled;
                    ultraCheckEditorDarkFrame.Checked = exposureDataPresenter.ExposureDataList[currentRecord].DarkFrameEnabled;



                    //subarray
                    if(exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas.Count>0)
                    {
                        textBoxSubarrayXo.Text = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayRegionXo.ToString();
                        textBoxSubarraydX.Text = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayRegiondX.ToString();
                        textBoxSubarrayYo.Text = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayRegionYo.ToString();
                        textBoxSubarraydY.Text = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayRegiondY.ToString();
                        textBoxSubarrayName.Text = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayName.ToString();
                        ultraCheckEditorSubarrayRead.Checked = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayReadEnabled;
                        textBoxSubarrayReadInterval.Text = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayReadInterval.ToString();

                        ultraCheckEditorSubarraySubinject.Checked = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarraySubinjectEnabled;
                        textBoxSubarraySubinjectInterval.Text = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarraySubinjectInterval.ToString();

                        ultraCheckEditorSubarrayPostRead.Checked = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayPostReadEnabled;
                        textBoxSubarrayNDROs.Text = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayPostReadNDROS.ToString();

                        textBoxThresholdXo.Text = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdRegionXo.ToString();
                        textBoxThresholddX.Text = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdRegiondX.ToString();
                        textBoxThresholdYo.Text = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdRegionYo.ToString();
                        textBoxThresholddY.Text = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdRegiondY.ToString();
                        ultraCheckEditorEnableThreshold.Checked = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdEnabled;

                        ultraCheckEditorTimeResolved.Checked = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].TimeResolvedEnabled;
                        ultraCheckEditorAdaptive.Checked = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].AdaptiveEnabled;
                        textBoxThresholdPercent.Text = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdPercent.ToString();
                        textBoxThresholdTimeResolved.Text = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].TimeResolvedInterval.ToString();

                        if (exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayFPN == 0)
                        {
                            ultraRadioButtonSubarrayFPNNone.Checked = true;
                        }
                        else if (exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayFPN == 1)
                        {
                            ultraRadioButtonSubarrayFPNRunTime.Checked = true;
                        }
                        else if (exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayFPN == 2)
                        {
                            ultraRadioButtonSubarrayFPNStored.Checked = true;
                        }
                    }
                   
                }

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void buttonFirstExposure_Click(object sender, EventArgs e)
        {
            try
            {
                currentRecord = 0;
                UpdateExposureView(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Navigating Exposure", "Navigate Exposure", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void buttonNextExposure_Click(object sender, EventArgs e)
        {
            try
            {
                currentRecord++;
                UpdateExposureView(currentRecord);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Navigating Exposure", "Navigate Exposure", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void buttonPreviousExposure_Click(object sender, EventArgs e)
        {
            try
            {
                currentRecord--;
                UpdateExposureView(currentRecord);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Navigating Exposure", "Navigate Exposure", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void buttonLastExposure_Click(object sender, EventArgs e)
        {
            try
            {
                currentRecord = exposureDataPresenter.ExposureDataList.Count();
                currentRecord--;
                UpdateExposureView(currentRecord);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Navigating Exposure", "Navigate Exposure", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void MouseEnterLeaveEventHandlers()
        {
            try
            {
                FirstExposure.MouseEnter += buttonMouseEnter;
                NextExposure.MouseEnter += buttonMouseEnter;
                PreviousExposure.MouseEnter += buttonMouseEnter;
                LastExposure.MouseEnter += buttonMouseEnter;

                FirstExposure.MouseLeave += buttonMouseLeave;
                NextExposure.MouseLeave += buttonMouseLeave;
                PreviousExposure.MouseLeave += buttonMouseLeave;
                LastExposure.MouseLeave += buttonMouseLeave;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void buttonMouseEnter(object sender, EventArgs e)
        {
            UltraButton button = sender as UltraButton;
            button.Size = new Size(48, 48);
            button.ImageSize = new Size(40, 40);
        }
        private void buttonMouseLeave(object sender, EventArgs e)
        {
            UltraButton button = sender as UltraButton;
            button.Size = new Size(40, 40);
            button.ImageSize = new Size(32, 32);
        }
        private void AutoTestExposureSettings_Load(object sender, EventArgs e)
        {
            MouseEnterLeaveEventHandlers();
        }
        public void GetData()
        {
            try
            {
                exposureDataPresenter.GetExposureData(cidInterface);
                UpdateExposureView(0);
                if (exposureType != null)
                {
                    if (exposureType.Equals("RBT"))
                    {
                        subarraySettingsGroup.Visible = ultraCheckEditorAutoBiasFPN.Visible = false;
                    }
                    else if (exposureType.Equals("RNT"))
                    {
                        ultraCheckEditorTimeResolved.Visible = ultraCheckEditorTimeResolved.Visible
                        = ultraCheckEditorAdaptive.Visible = ultraCheckEditorAutoBiasFPN.Visible = subarrayFPNGroup.Visible = false;
                    }
                    else if (exposureType.Equals("IET"))
                    {
                        ultraCheckEditorTimeResolved.Visible = ultraCheckEditorTimeResolved.Visible
                        = ultraCheckEditorAdaptive.Visible = false;
                    }
                    else if (exposureType.Equals("PRT"))
                    {
                        ultraCheckEditorTimeResolved.Visible = ultraCheckEditorTimeResolved.Visible
                        = ultraCheckEditorAdaptive.Visible = false;                        
                    }
                    else if (exposureType.Equals("SDT"))
                    {
                        subarraySettingsGroup.Visible = ultraCheckEditorAutoBiasFPN.Visible = false;
                    }
                    else if (exposureType.Equals("DET"))
                    {
                        subarraySettingsGroup.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public void InitialExposureData()
        {
            try
            {
                UpdateExposureDataModelFromView();
                UpdateSubarrayDataModelFromView();
                //exposureDataPresenter.ExposureDataModel.GlobalInject = 0;
                //exposureDataPresenter.ExposureDataModel.GlobalInjectDelay = 0;
                exposureDataPresenter.ExposureDataModel.NumberOfSubarrays = 1;
                exposureDataPresenter.ExposureDataModel.SubarrayDatas = new List<SubarrayDataModel>();
                exposureDataPresenter.ExposureDataModel.SubarrayDatas.Add(exposureDataPresenter.SubarrayDataModel);
                //TestAppHelper.repeatPhotoResponseExposure = true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }

        }
        private bool UpdateExposureDataModelFromView()
        {
            try
            {
                exposureDataPresenter.ExposureDataModel = new ExposureDataModel();

                //exposureDataPresenter.ExposureDataModel.ExposureID = Convert.ToInt32(ultraTextEditorExposureID.Value);
                exposureDataPresenter.ExposureDataModel.TestDataID = exposureType;
                exposureDataPresenter.ExposureDataModel.UserModified = Environment.UserName;
                exposureDataPresenter.ExposureDataModel.GlobalInjectDelay = Convert.ToInt32(textBoxDelay.Text);
                exposureDataPresenter.ExposureDataModel.GlobalInject = Convert.ToInt32(textBoxGlobalInject.Text);

                //Exposure         
                exposureDataPresenter.ExposureDataModel.ExposureName = textBoxExposureName.Text.ToString();
                exposureDataPresenter.ExposureDataModel.ExposureInterval = Convert.ToInt32(textBoxExposureInterval.Text);
                exposureDataPresenter.ExposureDataModel.ExposureNDROS = Convert.ToInt32(textBoxNDROs.Text);
                exposureDataPresenter.ExposureDataModel.ExposureRegionXo = Convert.ToInt32(textBoxExposureXo.Text);
                exposureDataPresenter.ExposureDataModel.ExposureRegiondX = Convert.ToInt32(textBoxExposuredX.Text);
                exposureDataPresenter.ExposureDataModel.ExposureRegionYo = Convert.ToInt32(textBoxExposureYo.Text);
                exposureDataPresenter.ExposureDataModel.ExposureRegiondY = Convert.ToInt32(textBoxExposuredY.Text);
                exposureDataPresenter.ExposureDataModel.LED1Enabled = ultraCheckEditorLED1.Checked;
                exposureDataPresenter.ExposureDataModel.LED2Enabled = ultraCheckEditorLED2.Checked;
                exposureDataPresenter.ExposureDataModel.LED3Enabled = ultraCheckEditorLED3.Checked;
                exposureDataPresenter.ExposureDataModel.ShutterEnabled = ultraCheckEditorShutterEnabled.Checked;
                exposureDataPresenter.ExposureDataModel.LEDOnTime = Convert.ToInt32(textBoxLEDOn.Text);
                exposureDataPresenter.ExposureDataModel.LEDOffTime = Convert.ToInt32(textBoxLEDOff.Text);
                exposureDataPresenter.ExposureDataModel.LEDFlashes = Convert.ToInt32(textBoxLEDFlash.Text);
                exposureDataPresenter.ExposureDataModel.AutoBiasFPNEnabled = Convert.ToByte(ultraCheckEditorAutoBiasFPN.Checked);
                exposureDataPresenter.ExposureDataModel.NumberOfSubarrays = 1;

                if (ultraRadioButtonFPNNone.Checked == true)
                {
                    exposureDataPresenter.ExposureDataModel.ExposureFPN = 0;
                }
                else if (ultraRadioButtonFPNRunTime.Checked == true)
                {
                    exposureDataPresenter.ExposureDataModel.ExposureFPN = 1;
                }
                else if (ultraRadioButtonFPNStored.Checked == true)
                {
                    exposureDataPresenter.ExposureDataModel.ExposureFPN = 3;
                }

                exposureDataPresenter.ExposureDataModel.FullFrameEnabled = ultraCheckEditorExposedFrame.Checked;
                exposureDataPresenter.ExposureDataModel.DarkFrameEnabled = ultraCheckEditorDarkFrame.Checked;
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
        private bool UpdateSubarrayDataModelFromView()
        {
            try
            {
                exposureDataPresenter.SubarrayDataModel = new SubarrayDataModel();
                //exposureDataPresenter.SubarrayDataModel.SubarrayID = Convert.ToInt32(ultraTextEditorSubarrayID.Value);
                //exposureDataPresenter.SubarrayDataModel.ExposureID = Convert.ToInt32(ultraTextEditorExposureID.Value);
                exposureDataPresenter.SubarrayDataModel.UserModified = Environment.UserName;
                exposureDataPresenter.SubarrayDataModel.SubarrayName = textBoxSubarrayName.Text.ToString();
                exposureDataPresenter.SubarrayDataModel.SubarrayRegionXo = Convert.ToInt32(textBoxSubarrayXo.Text);
                exposureDataPresenter.SubarrayDataModel.SubarrayRegiondX = Convert.ToInt32(textBoxSubarraydX.Text);
                exposureDataPresenter.SubarrayDataModel.SubarrayRegionYo = Convert.ToInt32(textBoxSubarrayYo.Text);
                exposureDataPresenter.SubarrayDataModel.SubarrayRegiondY = Convert.ToInt32(textBoxSubarraydY.Text);

                exposureDataPresenter.SubarrayDataModel.SubarrayReadEnabled = ultraCheckEditorSubarrayRead.Checked;
                exposureDataPresenter.SubarrayDataModel.SubarrayReadInterval = Convert.ToInt32(textBoxSubarrayReadInterval.Text);

                exposureDataPresenter.SubarrayDataModel.SubarraySubinjectEnabled = ultraCheckEditorSubarraySubinject.Checked;
                exposureDataPresenter.SubarrayDataModel.SubarraySubinjectInterval = Convert.ToInt32(textBoxSubarraySubinjectInterval.Text);

                exposureDataPresenter.SubarrayDataModel.SubarrayPostReadEnabled = ultraCheckEditorSubarrayPostRead.Checked;
                exposureDataPresenter.SubarrayDataModel.SubarrayPostReadNDROS = Convert.ToInt32(textBoxSubarrayNDROs.Text);

                exposureDataPresenter.SubarrayDataModel.SubarrayThresholdRegionXo = Convert.ToInt32(textBoxThresholdXo.Text);
                exposureDataPresenter.SubarrayDataModel.SubarrayThresholdRegiondX = Convert.ToInt32(textBoxThresholddX.Text);
                exposureDataPresenter.SubarrayDataModel.SubarrayThresholdRegionYo = Convert.ToInt32(textBoxThresholdYo.Text);
                exposureDataPresenter.SubarrayDataModel.SubarrayThresholdRegiondY = Convert.ToInt32(textBoxThresholddY.Text);
                exposureDataPresenter.SubarrayDataModel.SubarrayThresholdEnabled = ultraCheckEditorEnableThreshold.Checked;

                exposureDataPresenter.SubarrayDataModel.TimeResolvedEnabled = ultraCheckEditorTimeResolved.Checked;
                exposureDataPresenter.SubarrayDataModel.AdaptiveEnabled = ultraCheckEditorAdaptive.Checked;
                exposureDataPresenter.SubarrayDataModel.SubarrayThresholdPercent = Convert.ToInt32(textBoxThresholdPercent.Text);
                exposureDataPresenter.SubarrayDataModel.TimeResolvedInterval = Convert.ToInt32(textBoxThresholdTimeResolved.Text);

                if (ultraRadioButtonSubarrayFPNNone.Checked == true)
                {
                    exposureDataPresenter.SubarrayDataModel.SubarrayFPN = 0;
                }
                else if (ultraRadioButtonSubarrayFPNRunTime.Checked == true)
                {
                    exposureDataPresenter.SubarrayDataModel.SubarrayFPN = 1;
                }
                else if (ultraRadioButtonSubarrayFPNStored.Checked == true)
                {
                    exposureDataPresenter.SubarrayDataModel.SubarrayFPN = 3;
                }

                return true;

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
    }
}
