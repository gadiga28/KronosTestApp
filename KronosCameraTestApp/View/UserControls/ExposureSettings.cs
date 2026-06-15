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
using Thermo.Kronos.Instrument.Camera.Interface;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.Helpers;
using Infragistics.Win.Misc;

namespace KronosCameraTestApp.View.UserControls
{
    public partial class ExposureSettings : UserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public int currentRecord = 0;
        public int currentSubarrayRecord = 0;
        private string userMode = string.Empty;
        bool engineeringLoginStatus = false;
        private CIDInterface cidInterface = null;
        public ExposureDataPresenter exposureDataPresenter { get; set; }
        List<ExposureDataModel> exposureList = new List<ExposureDataModel>();
        ListExposureData listExposureData { get; set; }
        public ExposureDataModel ExposureDataModel { get; set; }
        public List<ExposureDataModel> ExposureList
        {
            get { return exposureDataPresenter.ExposureDataList; }
            set { exposureDataPresenter.ExposureDataList = value; }
        }
        public List<SubarrayDataModel> SubarrayList
        {
            get { return exposureDataPresenter.SubarrayDataModelList; }
            set { exposureDataPresenter.SubarrayDataModelList = value; }
        }
        string exposureType;
        Form mdiParent = null;
        string userName = string.Empty;
        string xmlFile = string.Empty;
        string rootElementName = string.Empty;
        bool autoTest = false;
        public ExposureSettings()
        {
            InitializeComponent();
        }
        public ExposureSettings(string exposureType,string UserMode, bool enggLoginStatus, string userName, string xmlFile, string rootElementName,bool autoTest=false)
        {
            InitializeComponent();
            currentSubarrayRecord = 0;
            currentRecord = 0;
            this.exposureType = exposureType;
            this.userMode = UserMode;
            this.engineeringLoginStatus = enggLoginStatus;
            this.userName = userName;
            this.xmlFile = xmlFile;
            this.rootElementName = rootElementName;
            this.autoTest = autoTest;
            exposureDataPresenter = new ExposureDataPresenter(exposureType);
            if (userMode == "Admin" && engineeringLoginStatus)
            {
                AddExposure.Enabled = true;
                RemoveExposure.Enabled = true;
                UpdateExposure.Enabled = true;
                AddSubarray.Enabled = true;
                RemoveSubarray.Enabled = true;
                UpdateSubarray.Enabled = true;
            }
            else if (userMode == "Manufacturing")
            {
                subarrayRegionGroupBox.Enabled = false;
                subarraySubRegionGroupBox.Enabled = false;
                subarrayEditExposure.Enabled = false;
                subarrayFPNGroup.Enabled = false;
                exposureRegion.Enabled = false;
                exposureEdit.Enabled = false;
            }
            //exposureDataPresenter.GetExposureData(cidInterface);           
        }              
        private void ExposureSettings_Load(object sender, EventArgs e)
        {                            
           
            MouseEnterLeaveEventHandlers();           
            numericEditExposureInterval.BeforeChangeValue += CheckRanges;
            numericEditLEDOn.BeforeChangeValue += CheckRanges;
            numericEditLEDOff.BeforeChangeValue += CheckRanges;
            numericEditFlash.BeforeChangeValue += CheckRanges;
            numericEditExposureStartX.BeforeChangeValue += CheckRanges;
            numericEditExposureStartY.BeforeChangeValue += CheckRanges;
            numericEditExposureWidth.BeforeChangeValue += CheckRanges;
            numericEditExposureHeight.BeforeChangeValue += CheckRanges;
            numericEditThresholdPercent.BeforeChangeValue += CheckRanges;
            numericEditSubarrayStartX.BeforeChangeValue += CheckRanges;
            numericEditSubarrayStartY.BeforeChangeValue += CheckRanges;
            numericEditSubarrayWidth.BeforeChangeValue += CheckRanges;
            numericEditSubarrayHeight.BeforeChangeValue += CheckRanges;
            numericEditNDRO.BeforeChangeValue += CheckRanges;
            numericEditSubarrayNDROs.BeforeChangeValue += CheckRanges;
            numericEditExposureWidth.AfterChangeValue += CheckRanges1;
            
            //else if(autoTest)
            //{
            //    setExposureSettingReadOnlyForAutoTest();
            //}           
        }
      
        public void GetData()
        {
            try
            {                
                exposureDataPresenter.GetExposureData(cidInterface);
                //if (userMode == "Admin" && engineeringLoginStatus)
                //{
                //    AddExposure.Enabled = true;
                //    RemoveExposure.Enabled = true;
                //    UpdateExposure.Enabled = true;
                //    AddSubarray.Enabled = true;
                //    RemoveSubarray.Enabled = true;
                //    UpdateSubarray.Enabled = true;
                //}
                //else if (userMode == "Manufacturing")
                //{
                //    ExposureSettingsGroupBox.Enabled = false;
                //}
                //else if (autoTest)
                //{
                //    setExposureSettingReadOnlyForAutoTest();
                //}
                UpdateExposureView(0);
                exposureDataPresenter.ExposureDataModel = exposureDataPresenter.ExposureDataList[0];
                //exposureDataPresenter.SubarrayDataModel = exposureDataPresenter.ExposureDataList[0].SubarrayDatas[0];
                AutoCompleteExposureSubarray();
                if (exposureType != null)
                {
                    if (exposureType.Equals("RBT"))
                    {
                        subarraySettingsGroup.Visible = ultraCheckEditorAutoBias.Visible = false;
                    }
                    else if (exposureType.Equals("RNT"))
                    {
                        ultraCheckEditorTimeResolved.Visible = numericEditSubarrayTimeResolved.Visible
                        = ultraCheckEditorAdaptive.Visible = ultraCheckEditorAutoBias.Visible = subarrayFPNGroup.Visible = false;
                    }
                    else if (exposureType.Equals("IET"))
                    {
                        ultraCheckEditorTimeResolved.Visible = numericEditSubarrayTimeResolved.Visible
                        = ultraCheckEditorAdaptive.Visible = false;
                    }
                    else if (exposureType.Equals("PRT"))
                    {
                        ultraCheckEditorTimeResolved.Visible = numericEditSubarrayTimeResolved.Visible
                        = ultraCheckEditorAdaptive.Visible = false;
                       // checkBoxRepeatExposure.Visible = true;
                    }
                    else if(exposureType.Equals("SDT"))
                    {
                        subarraySettingsGroup.Visible = ultraCheckEditorAutoBias.Visible = false;
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
        private void CheckRanges(object sender, NationalInstruments.UI.BeforeChangeNumericValueEventArgs e)
        {
            TestAppHelper.DisplayRangeMessage(sender, e.NewValue);
        }
        private void CheckRanges1(object sender, NationalInstruments.UI.AfterChangeNumericValueEventArgs e)
        {
            TestAppHelper.DisplayRangeMessage(sender, e.NewValue);
        }
        private void AutoCompleteExposureSubarray()
        {
            try
            {
                AutoCompleteStringCollection exposureNameList = new AutoCompleteStringCollection();
                AutoCompleteStringCollection subarrayNameList = new AutoCompleteStringCollection();
                foreach (ExposureDataModel mvm in exposureDataPresenter.ExposureDataList)
                {
                    exposureNameList.Add(mvm.ExposureName);
                }
                ultraTextEditorExposureName.AutoCompleteMode = AutoCompleteMode.Suggest;
                ultraTextEditorExposureName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                ultraTextEditorExposureName.AutoCompleteCustomSource = exposureNameList;

                foreach (SubarrayDataModel mvs in exposureDataPresenter.SubarrayDataModelList)
                {
                    subarrayNameList.Add(mvs.SubarrayName);
                }
                ultraTextEditorSubarrayName.AutoCompleteMode = AutoCompleteMode.Suggest;
                ultraTextEditorSubarrayName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                ultraTextEditorSubarrayName.AutoCompleteCustomSource = subarrayNameList;
               

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void MouseEnterLeaveEventHandlers()
        {
            try
            {
                AddExposure.MouseEnter += buttonMouseEnter;
                RemoveExposure.MouseEnter += buttonMouseEnter;
                UpdateExposure.MouseEnter += buttonMouseEnter;
                SearchExposure.MouseEnter += buttonMouseEnter;
                RefreshExposure.MouseEnter += buttonMouseEnter;
                ListExposureData.MouseEnter += buttonMouseEnter;

                AddExposure.MouseLeave += buttonMouseLeave;
                RemoveExposure.MouseLeave += buttonMouseLeave;
                UpdateExposure.MouseLeave += buttonMouseLeave;
                SearchExposure.MouseLeave += buttonMouseLeave;
                RefreshExposure.MouseLeave += buttonMouseLeave;
                ListExposureData.MouseLeave += buttonMouseLeave;


                FirstExposure.MouseEnter += buttonMouseEnter;
                NextExposure.MouseEnter += buttonMouseEnter;
                PreviousExposure.MouseEnter += buttonMouseEnter;
                LastExposure.MouseEnter += buttonMouseEnter;


                FirstExposure.MouseLeave += buttonMouseLeave;
                NextExposure.MouseLeave += buttonMouseLeave;
                PreviousExposure.MouseLeave += buttonMouseLeave;
                LastExposure.MouseLeave += buttonMouseLeave;




                AddSubarray.MouseEnter += buttonMouseEnter;
                RemoveSubarray.MouseEnter += buttonMouseEnter;
                UpdateSubarray.MouseEnter += buttonMouseEnter;

                AddSubarray.MouseLeave += buttonMouseLeave;
                RemoveSubarray.MouseLeave += buttonMouseLeave;
                UpdateSubarray.MouseLeave += buttonMouseLeave;

                FirstSubarray.MouseEnter += buttonMouseEnter;
                NextSubarray.MouseEnter += buttonMouseEnter;
                PreviousSubarray.MouseEnter += buttonMouseEnter;
                LastSubarray.MouseEnter += buttonMouseEnter;


                FirstSubarray.MouseLeave += buttonMouseLeave;
                NextSubarray.MouseLeave += buttonMouseLeave;
                PreviousSubarray.MouseLeave += buttonMouseLeave;
                LastSubarray.MouseLeave += buttonMouseLeave;
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
        public void UpdateExposureView(int currentRecord)
        {
            try
            {
                if (currentRecord >= 0 && exposureDataPresenter.ExposureDataList.Count > 0 && currentRecord < exposureDataPresenter.ExposureDataList.Count)
                {
                    ultraLabelNumberofExposures.Text = "Total # of Exposures:" + exposureDataPresenter.ExposureDataList.Count.ToString();
                    ultraLabelNumSubarrays.Text = "Total # of Subarrays:" + exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas.Count.ToString();
                    ultraTextEditorNumberOfSubarrays.Text = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas.Count.ToString();
                    ultraTextEditorExposureID.Text = exposureDataPresenter.ExposureDataList[currentRecord].ExposureID.ToString();

                    ultraTextEditorExposureName.Text = exposureDataPresenter.ExposureDataList[currentRecord].ExposureName.ToString();

                    numericEditDelay.Value = exposureDataPresenter.ExposureDataList[currentRecord].GlobalInjectDelay;
                    numericEditGlobalInject.Value = exposureDataPresenter.ExposureDataList[currentRecord].GlobalInject;

                    numericEditExposureInterval.Value = exposureDataPresenter.ExposureDataList[currentRecord].ExposureInterval;
                    numericEditNDRO.Value = exposureDataPresenter.ExposureDataList[currentRecord].ExposureNDROS;
                    numericEditExposureStartX.Value = exposureDataPresenter.ExposureDataList[currentRecord].ExposureRegionXo;
                    numericEditExposureWidth.Value = exposureDataPresenter.ExposureDataList[currentRecord].ExposureRegiondX;
                    numericEditExposureStartY.Value = exposureDataPresenter.ExposureDataList[currentRecord].ExposureRegionYo;
                    numericEditExposureHeight.Value = exposureDataPresenter.ExposureDataList[currentRecord].ExposureRegiondY;


                    ultraCheckEditorLED1.Checked = exposureDataPresenter.ExposureDataList[currentRecord].LED1Enabled;
                    ultraCheckEditorLED2.Checked = exposureDataPresenter.ExposureDataList[currentRecord].LED2Enabled;
                    ultraCheckEditorLED3.Checked = exposureDataPresenter.ExposureDataList[currentRecord].LED3Enabled;
                    ultraCheckEditorShutterEnabled.Checked = exposureDataPresenter.ExposureDataList[currentRecord].ShutterEnabled;
                    numericEditLEDOn.Value = exposureDataPresenter.ExposureDataList[currentRecord].LEDOnTime;
                    numericEditLEDOff.Value = exposureDataPresenter.ExposureDataList[currentRecord].LEDOffTime;
                    numericEditFlash.Value = exposureDataPresenter.ExposureDataList[currentRecord].LEDFlashes;
                    ultraCheckEditorAutoBias.Checked = Convert.ToBoolean(exposureDataPresenter.ExposureDataList[currentRecord].AutoBiasFPNEnabled);

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

                    if (exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas != null && exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas.Count > 0)
                    {
                        subarraySettingsGroup.Visible = true;
                        ultraTextEditorSubarrayID.Text = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayID.ToString();
                        ultraTextEditorSubarrayName.Text = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayName.ToString();

                        numericEditSubarrayStartX.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayRegionXo;
                        numericEditSubarrayWidth.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayRegiondX;
                        numericEditSubarrayStartY.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayRegionYo;
                        numericEditSubarrayHeight.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayRegiondY;

                        ultraCheckEditorSubarrayRead.Checked = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayReadEnabled;
                        numericEditSubarrayReadInterval.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayReadInterval;

                        ultraCheckEditorSubarraySubInject.Checked = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarraySubinjectEnabled;
                        numericEditSubarraySubinjectInterval.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarraySubinjectInterval;
                        ultraCheckEditorSubarrayPostRead.Checked = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayPostReadEnabled;
                        numericEditSubarrayNDROs.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayPostReadNDROS;

                        numericEditSubarrayThresholdStartX.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdRegionXo;
                        numericEditSubarrayThresholdWidth.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdRegiondX;
                        numericEditSubarrayThresholdStartY.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdRegionYo;
                        numericEditSubarrayThresholdHeight.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdRegiondY;
                        ultraCheckEditorEnableThreshold.Checked = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdEnabled;

                        ultraCheckEditorTimeResolved.Checked = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].TimeResolvedEnabled;
                        ultraCheckEditorAdaptive.Checked = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].AdaptiveEnabled;
                        numericEditThresholdPercent.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].SubarrayThresholdPercent;
                        numericEditSubarrayTimeResolved.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[0].TimeResolvedInterval;

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
                    else
                    {
                        subarraySettingsGroup.Visible = false;
                        if (exposureType.Equals("MVT") || exposureType.Equals("DDT")
                            || exposureType.Equals("PRT") || exposureType.Equals("IET") || exposureType.Equals("RNT"))
                        {
                            ClearSubarray();
                        }                            
                    }
                }            
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ClearSubarray()
        {
            try
            {
                ultraTextEditorSubarrayID.Text = "";
                ultraTextEditorSubarrayName.Text = "";

                numericEditSubarrayStartX.Value = 0;
                numericEditSubarrayWidth.Value = 0;
                numericEditSubarrayStartY.Value = 0;
                numericEditSubarrayHeight.Value = 0;

                ultraCheckEditorSubarrayRead.Checked = false;
                numericEditSubarrayReadInterval.Value = 0;

                ultraCheckEditorSubarraySubInject.Checked = false;
                numericEditSubarraySubinjectInterval.Value = 0;

                ultraCheckEditorSubarrayPostRead.Checked = false;
                numericEditSubarrayNDROs.Value = 0;

                numericEditSubarrayThresholdStartX.Value = 0;
                numericEditSubarrayThresholdWidth.Value = 0;
                numericEditSubarrayThresholdStartY.Value = 0;
                numericEditSubarrayThresholdHeight.Value = 0;
                ultraCheckEditorEnableThreshold.Checked = false;

                ultraCheckEditorTimeResolved.Checked = false;
                ultraCheckEditorAdaptive.Checked = false;
                numericEditThresholdPercent.Value = 0;
                numericEditSubarrayTimeResolved.Value = 0;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateSubarrayView(int currentSubarrayRecord)
        {
            try
            {
                if (currentRecord >= 0 && exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas != null && exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas.Count > 0
               && currentSubarrayRecord < exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas.Count)
                {
                    ultraTextEditorSubarrayID.Text = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].SubarrayID.ToString();
                    ultraTextEditorSubarrayName.Text = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].SubarrayName.ToString();

                    numericEditSubarrayStartX.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].SubarrayRegionXo;
                    numericEditSubarrayWidth.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].SubarrayRegiondX;
                    numericEditSubarrayStartY.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].SubarrayRegionYo;
                    numericEditSubarrayHeight.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].SubarrayRegiondY;

                    ultraCheckEditorSubarrayRead.Checked = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].SubarrayReadEnabled;
                    numericEditSubarrayReadInterval.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].SubarrayReadInterval;

                    ultraCheckEditorSubarraySubInject.Checked = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].SubarraySubinjectEnabled;
                    numericEditSubarraySubinjectInterval.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].SubarraySubinjectInterval;
                    ultraCheckEditorSubarrayPostRead.Checked = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].SubarrayPostReadEnabled;
                    numericEditSubarrayNDROs.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].SubarrayPostReadNDROS;

                    numericEditSubarrayThresholdStartX.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].SubarrayThresholdRegionXo;
                    numericEditSubarrayThresholdWidth.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].SubarrayThresholdRegiondX;
                    numericEditSubarrayThresholdStartY.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].SubarrayThresholdRegionYo;
                    numericEditSubarrayThresholdHeight.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].SubarrayThresholdRegiondY;
                    ultraCheckEditorEnableThreshold.Checked = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].SubarrayThresholdEnabled;

                    ultraCheckEditorTimeResolved.Checked = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].TimeResolvedEnabled;
                    ultraCheckEditorAdaptive.Checked = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].AdaptiveEnabled;
                    numericEditThresholdPercent.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].SubarrayThresholdPercent;
                    numericEditSubarrayTimeResolved.Value = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].TimeResolvedInterval;

                    if (exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].SubarrayFPN == 0)
                    {
                        ultraRadioButtonSubarrayFPNNone.Checked = true;
                    }
                    else if (exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].SubarrayFPN == 1)
                    {
                        ultraRadioButtonSubarrayFPNRunTime.Checked = true;
                    }
                    else if (exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas[currentSubarrayRecord].SubarrayFPN == 2)
                    {
                        ultraRadioButtonSubarrayFPNStored.Checked = true;
                    }
                }


            }

            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void FirstExposure_Click(object sender, EventArgs e)
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
        private void NextExposure_Click(object sender, EventArgs e)
        {
            try
            {
                currentRecord++;
                
                if (currentRecord <= exposureDataPresenter.ExposureDataList.Count)
                    UpdateExposureView(currentRecord);
                else
                    currentRecord--;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Navigating Exposure", "Navigate Exposure", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }

        }
        private void PreviousExposure_Click(object sender, EventArgs e)
        {
            try
            {
                currentRecord--;
                if (currentRecord >= 0)
                    UpdateExposureView(currentRecord);
                else
                    currentRecord++;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Navigating Exposure", "Navigate Exposure", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void LastExposure_Click(object sender, EventArgs e)
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
        private bool UpdateExposureDataModelFromView()
        {
            try
            {
                exposureDataPresenter.ExposureDataModel = new ExposureDataModel();

                exposureDataPresenter.ExposureDataModel.ExposureID = Convert.ToInt32(ultraTextEditorExposureID.Value);
                exposureDataPresenter.ExposureDataModel.TestDataID = exposureType;
                exposureDataPresenter.ExposureDataModel.UserModified = Environment.UserName;
                exposureDataPresenter.ExposureDataModel.GlobalInjectDelay = Convert.ToInt32(numericEditDelay.Value);
                exposureDataPresenter.ExposureDataModel.GlobalInject = Convert.ToInt32(numericEditGlobalInject.Value);

                //Exposure         
                exposureDataPresenter.ExposureDataModel.ExposureName = ultraTextEditorExposureName.Text.ToString();
                exposureDataPresenter.ExposureDataModel.ExposureInterval = Convert.ToInt32(numericEditExposureInterval.Value);
                exposureDataPresenter.ExposureDataModel.ExposureNDROS = Convert.ToInt32(numericEditNDRO.Value);
                exposureDataPresenter.ExposureDataModel.ExposureRegionXo = Convert.ToInt32(numericEditExposureStartX.Value);
                exposureDataPresenter.ExposureDataModel.ExposureRegiondX = Convert.ToInt32(numericEditExposureWidth.Value);
                exposureDataPresenter.ExposureDataModel.ExposureRegionYo = Convert.ToInt32(numericEditExposureStartY.Value);
                exposureDataPresenter.ExposureDataModel.ExposureRegiondY = Convert.ToInt32(numericEditExposureHeight.Value);
                exposureDataPresenter.ExposureDataModel.LED1Enabled = ultraCheckEditorLED1.Checked;
                exposureDataPresenter.ExposureDataModel.LED2Enabled = ultraCheckEditorLED2.Checked;
                exposureDataPresenter.ExposureDataModel.LED3Enabled = ultraCheckEditorLED3.Checked;
                exposureDataPresenter.ExposureDataModel.ShutterEnabled = ultraCheckEditorShutterEnabled.Checked;
                exposureDataPresenter.ExposureDataModel.LEDOnTime = Convert.ToInt32(numericEditLEDOn.Value);
                exposureDataPresenter.ExposureDataModel.LEDOffTime = Convert.ToInt32(numericEditLEDOff.Value);
                exposureDataPresenter.ExposureDataModel.LEDFlashes = Convert.ToInt32(numericEditFlash.Value);
                exposureDataPresenter.ExposureDataModel.AutoBiasFPNEnabled = Convert.ToByte(ultraCheckEditorAutoBias.Checked);
                exposureDataPresenter.ExposureDataModel.NumberOfSubarrays = Convert.ToInt32(ultraTextEditorNumberOfSubarrays.Value);

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
        private bool UpdateExposureRecordForUpdateOperation(ExposureDataModel expRecord)
        {
            try
            {
                expRecord.ExposureID = Convert.ToInt32(ultraTextEditorExposureID.Value);
                expRecord.TestDataID = exposureType;
                expRecord.UserModified = Environment.UserName;
                expRecord.GlobalInjectDelay = Convert.ToInt32(numericEditDelay.Value);
                expRecord.GlobalInject = Convert.ToInt32(numericEditGlobalInject.Value);

                //Exposure         
                expRecord.ExposureName = ultraTextEditorExposureName.Text.ToString();
                expRecord.ExposureInterval = Convert.ToInt32(numericEditExposureInterval.Value);
                expRecord.ExposureNDROS = Convert.ToInt32(numericEditNDRO.Value);
                expRecord.ExposureRegionXo = Convert.ToInt32(numericEditExposureStartX.Value);
                expRecord.ExposureRegiondX = Convert.ToInt32(numericEditExposureWidth.Value);
                expRecord.ExposureRegionYo = Convert.ToInt32(numericEditExposureStartY.Value);
                expRecord.ExposureRegiondY = Convert.ToInt32(numericEditExposureHeight.Value);
                expRecord.LED1Enabled = ultraCheckEditorLED1.Checked;
                expRecord.LED2Enabled = ultraCheckEditorLED2.Checked;
                expRecord.LED3Enabled = ultraCheckEditorLED3.Checked;
                expRecord.ShutterEnabled = ultraCheckEditorShutterEnabled.Checked;
                expRecord.LEDOnTime = Convert.ToInt32(numericEditLEDOn.Value);
                expRecord.LEDOffTime = Convert.ToInt32(numericEditLEDOff.Value);
                expRecord.LEDFlashes = Convert.ToInt32(numericEditFlash.Value);
                expRecord.AutoBiasFPNEnabled = Convert.ToByte(ultraCheckEditorAutoBias.Checked);
                expRecord.NumberOfSubarrays = Convert.ToInt32(ultraTextEditorNumberOfSubarrays.Value);

                if (ultraRadioButtonFPNNone.Checked == true)
                {
                    expRecord.ExposureFPN = 0;
                }
                else if (ultraRadioButtonFPNRunTime.Checked == true)
                {
                    expRecord.ExposureFPN = 1;
                }
                else if (ultraCheckEditorAutoBias.Checked == true)
                {
                    expRecord.ExposureFPN = 2;
                }
                else if (ultraRadioButtonFPNStored.Checked == true)
                {
                    expRecord.ExposureFPN = 3;
                }

                expRecord.FullFrameEnabled = ultraCheckEditorExposedFrame.Checked;
                expRecord.DarkFrameEnabled = ultraCheckEditorDarkFrame.Checked;
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
                exposureDataPresenter.SubarrayDataModel.SubarrayID = Convert.ToInt32(ultraTextEditorSubarrayID.Value);
                exposureDataPresenter.SubarrayDataModel.ExposureID = Convert.ToInt32(ultraTextEditorExposureID.Value);
                exposureDataPresenter.SubarrayDataModel.UserModified = Environment.UserName;
                exposureDataPresenter.SubarrayDataModel.SubarrayName = ultraTextEditorSubarrayName.Text.ToString();
                exposureDataPresenter.SubarrayDataModel.SubarrayRegionXo = Convert.ToInt32(numericEditSubarrayStartX.Value);
                exposureDataPresenter.SubarrayDataModel.SubarrayRegiondX = Convert.ToInt32(numericEditSubarrayWidth.Value);
                exposureDataPresenter.SubarrayDataModel.SubarrayRegionYo = Convert.ToInt32(numericEditSubarrayStartY.Value);
                exposureDataPresenter.SubarrayDataModel.SubarrayRegiondY = Convert.ToInt32(numericEditSubarrayHeight.Value);

                exposureDataPresenter.SubarrayDataModel.SubarrayReadEnabled = ultraCheckEditorSubarrayRead.Checked;
                exposureDataPresenter.SubarrayDataModel.SubarrayReadInterval = Convert.ToInt32(numericEditSubarrayReadInterval.Value);

                exposureDataPresenter.SubarrayDataModel.SubarraySubinjectEnabled = ultraCheckEditorSubarraySubInject.Checked;
                exposureDataPresenter.SubarrayDataModel.SubarraySubinjectInterval = Convert.ToInt32(numericEditSubarraySubinjectInterval.Value);

                exposureDataPresenter.SubarrayDataModel.SubarrayPostReadEnabled = ultraCheckEditorSubarrayPostRead.Checked;
                exposureDataPresenter.SubarrayDataModel.SubarrayPostReadNDROS = Convert.ToInt32(numericEditSubarrayNDROs.Value);

                exposureDataPresenter.SubarrayDataModel.SubarrayThresholdRegionXo = Convert.ToInt32(numericEditSubarrayThresholdStartX.Value);
                exposureDataPresenter.SubarrayDataModel.SubarrayThresholdRegiondX = Convert.ToInt32(numericEditSubarrayThresholdWidth.Value);
                exposureDataPresenter.SubarrayDataModel.SubarrayThresholdRegionYo = Convert.ToInt32(numericEditSubarrayThresholdStartY.Value);
                exposureDataPresenter.SubarrayDataModel.SubarrayThresholdRegiondY = Convert.ToInt32(numericEditSubarrayThresholdHeight.Value);
                exposureDataPresenter.SubarrayDataModel.SubarrayThresholdEnabled = ultraCheckEditorEnableThreshold.Checked;

                exposureDataPresenter.SubarrayDataModel.TimeResolvedEnabled = ultraCheckEditorTimeResolved.Checked;
                exposureDataPresenter.SubarrayDataModel.AdaptiveEnabled = ultraCheckEditorAdaptive.Checked;
                exposureDataPresenter.SubarrayDataModel.SubarrayThresholdPercent = Convert.ToInt32(numericEditThresholdPercent.Value);
                exposureDataPresenter.SubarrayDataModel.TimeResolvedInterval = Convert.ToInt32(numericEditSubarrayTimeResolved.Value);

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
        private async void AddExposure_Click(object sender, EventArgs e)
        {
            try
            {
                if (UpdateExposureDataModelFromView())
                {
                    if (ultraTextEditorExposureName.Text.ToString() == string.Empty)
                    {
                        MessageBox.Show("Exposure name cannot be empty", "Add Exposure", MessageBoxButtons.OK);
                        return;
                    }
                    if (ultraTextEditorSubarrayName.Text.ToString() == string.Empty)
                    {
                        MessageBox.Show("Subarray name cannot be empty", "Add Exposure", MessageBoxButtons.OK);
                        return;
                    }

                    string addExposureString = "Are you sure to add Exposure " + ultraTextEditorExposureName.Text.ToString() + " ?";
                    DialogResult result = MessageBox.Show(addExposureString, "Add Exposure", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {

                        if (!exposureType.Equals("RBT") || !exposureType.Equals("PRT") || !exposureType.Equals("SDT") || !exposureType.Equals("DET"))
                        {
                            UpdateSubarrayDataModelFromView();
                            exposureDataPresenter.ExposureDataModel.NumberOfSubarrays = 1;
                            exposureDataPresenter.ExposureDataModel.SubarrayDatas = new List<SubarrayDataModel>();
                            exposureDataPresenter.ExposureDataModel.SubarrayDatas.Add(exposureDataPresenter.SubarrayDataModel);
                        }
                       

                        //Save to database if engineering user
                        if (userMode == "Admin" && engineeringLoginStatus)
                        {
                            bool saveDB = await exposureDataPresenter.SaveExposureDataToDB("ADD", userName, xmlFile, rootElementName);
                            if (saveDB)
                            {
                                //exposureDataPresenter.InMemoryModelChanged = false;
                                MessageBox.Show("Exposure Added successfully", "Add Exposure", MessageBoxButtons.OK);
                                exposureDataPresenter.ExposureDataList.Clear();
                                exposureDataPresenter.SubarrayDataModelList.Clear();
                                exposureDataPresenter.GetExposureData(cidInterface);
                            }
                            else
                                MessageBox.Show("Exposure not Added", "Add Exposure", MessageBoxButtons.OK);
                        }

                        //Make inmemory changes                     

                        if (userMode == "Engineering")
                        {
                            exposureDataPresenter.ExposureDataList.Add(exposureDataPresenter.ExposureDataModel);
                            exposureDataPresenter.SubarrayDataModelList.Add(exposureDataPresenter.SubarrayDataModel);
                            MessageBox.Show("Exposure Added successfully", "Add Exposure", MessageBoxButtons.OK);
                        }
                        ultraLabelNumberofExposures.Text = "Total # of Exposures:" + exposureDataPresenter.ExposureDataList.Count().ToString();
                        //numericEditNumOfExposures.Value = exposureDataPresenter.ExposureDataList.Count;
                        if (exposureDataPresenter.ExposureDataModel.SubarrayDatas != null)
                            ultraLabelNumSubarrays.Text = "Total # of Subarrays:" + exposureDataPresenter.ExposureDataModel.SubarrayDatas.Count().ToString();
                        else
                            ultraLabelNumSubarrays.Text = "Total # of Subarrays: 0";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding Exposure", "Add Exposure", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void RemoveExposure_Click(object sender, EventArgs e)
        {
            try
            {
                string removeExposure = "Are you sure to remove Exposure " + ultraTextEditorExposureName.Text + " ?";

                DialogResult result = MessageBox.Show(removeExposure, "Remove Exposure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (result == DialogResult.OK)
                {
                    //Save data to DB
                    if (userMode == "Admin" && engineeringLoginStatus)
                    {
                        UpdateExposureDataModelFromView();
                        UpdateSubarrayDataModelFromView();
                        exposureDataPresenter.ExposureDataModel.SubarrayDatas = new List<SubarrayDataModel>();
                        exposureDataPresenter.ExposureDataModel.SubarrayDatas.Add(exposureDataPresenter.SubarrayDataModel);
                        bool saveDB = await exposureDataPresenter.SaveExposureDataToDB("REMOVE", userName, xmlFile, rootElementName);
                        if (saveDB)
                        {
                            //exposureDataPresenter.InMemoryModelChanged = false;
                            MessageBox.Show("Exposure Removed successfully", "Remove Exposure", MessageBoxButtons.OK);
                            exposureDataPresenter.ExposureDataList.Clear();
                            exposureDataPresenter.SubarrayDataModelList.Clear();
                             exposureDataPresenter.GetExposureData(cidInterface);
                            if (currentRecord == 0)
                            {
                                currentRecord = currentRecord + 1;
                                UpdateExposureView(currentRecord);
                                UpdateSubarrayView(0);
                            }
                            else
                            {
                                currentRecord = currentRecord - 1;
                                UpdateExposureView(currentRecord);
                                UpdateSubarrayView(0);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Exposure not Removed", "Remove Exposure", MessageBoxButtons.OK);
                        }
                    }
                    else if (userMode == "Engineering")
                    {
                        var expitem = exposureDataPresenter.ExposureDataList.FirstOrDefault(x => x.ExposureID.Equals(Convert.ToInt32(ultraTextEditorExposureID.Value)));
                        if (expitem != null)
                        {
                            if (exposureDataPresenter.ExposureDataList.Remove(expitem))
                            {
                                var subitem = exposureDataPresenter.SubarrayDataModelList.FirstOrDefault(x => x.SubarrayID.Equals(Convert.ToInt32(ultraTextEditorSubarrayID.Value)));
                                if (subitem != null)
                                {
                                    if (exposureDataPresenter.SubarrayDataModelList.Remove(subitem))
                                    {
                                        MessageBox.Show("Exposure Removed successfully", "Remove Exposure", MessageBoxButtons.OK);
                                        if (currentRecord == 0)
                                        {
                                            currentRecord = currentRecord + 1;
                                            UpdateExposureView(currentRecord);
                                            UpdateSubarrayView(0);
                                        }

                                        else
                                        {
                                            currentRecord = currentRecord - 1;
                                            UpdateExposureView(currentRecord);
                                            UpdateSubarrayView(0);
                                        }
                                    }
                                    else
                                        MessageBox.Show("Exposure not Removed", "Remove Exposure", MessageBoxButtons.OK);
                                }
                            }
                            else
                                MessageBox.Show("Exposure not Removed", "Remove Exposure", MessageBoxButtons.OK);
                        }

                    }

                    ultraLabelNumberofExposures.Text = "Total # of Exposures:" + exposureDataPresenter.ExposureDataList.Count().ToString();
                    //numericEditNumOfExposures.Value = exposureDataPresenter.ExposureDataList.Count;
                    if (exposureDataPresenter.ExposureDataModel.SubarrayDatas != null)
                        ultraLabelNumSubarrays.Text = "Total # of Subarrays:" + exposureDataPresenter.ExposureDataModel.SubarrayDatas.Count().ToString();
                    else
                        ultraLabelNumSubarrays.Text = "Total # of Subarrays: 0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error removing Exposure", "Remove Exposure", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void UpdateExposure_Click(object sender, EventArgs e)
        {
            try
            {
                if (UpdateExposureDataModelFromView())
                {
                    
                    string updateExposure = "Are you sure to update Exposure " + ultraTextEditorExposureName.Text.ToString() + " ?";

                    DialogResult result = MessageBox.Show(updateExposure, "Update Exposure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.OK)
                    {
                        //UpdateExposureDataModelFromView();
                        //UpdateSubarrayDataModelFromView();
                        if (exposureDataPresenter.ExposureDataModel.NumberOfSubarrays > 0)
                        {
                            UpdateSubarrayDataModelFromView();
                            exposureDataPresenter.ExposureDataModel.SubarrayDatas = new List<SubarrayDataModel>();
                            exposureDataPresenter.ExposureDataModel.SubarrayDatas.Add(exposureDataPresenter.SubarrayDataModel);
                        }
                        if (userMode == "Admin" && engineeringLoginStatus)
                        {

                            bool saveDB = await exposureDataPresenter.SaveExposureDataToDB("UPDATE", userName, xmlFile, rootElementName);
                            if (saveDB)
                            {
                                //exposureDataPresenter.InMemoryModelChanged = false;
                                MessageBox.Show("Exposure Updated successfully", "Update Exposure", MessageBoxButtons.OK);
                                exposureDataPresenter.GetExposureData(cidInterface);
                            }
                            else
                            {
                                MessageBox.Show("Exposure not Updated", "Update Exposure", MessageBoxButtons.OK);
                            }
                        }
                        else if (userMode == "Engineering")
                        {
                            var exprecord = exposureDataPresenter.ExposureDataList.FirstOrDefault(exp => exp.ExposureID.Equals(Convert.ToInt32(ultraTextEditorExposureID.Value)));
                            if (UpdateExposureRecordForUpdateOperation(exprecord))
                            {
                                var subrec = exprecord.SubarrayDatas.FirstOrDefault(sub => sub.SubarrayID.Equals(Convert.ToInt32(ultraTextEditorSubarrayID.Value)));
                                if (UpdateSubarrayModelForUpdateOperation(subrec))
                                {
                                    MessageBox.Show("Exposure Updated successfully", "Update Exposure", MessageBoxButtons.OK);
                                    //exposureDataPresenter.InMemoryModelChanged = true;
                                    ExposureDataModel = exposureDataPresenter.ExposureDataModel = exprecord;
                                    
                                }
                            }
                            else
                            {
                                MessageBox.Show("Exposure not Updated", "Update Exposure", MessageBoxButtons.OK);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating Exposure", "Update Exposure", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void SearchExposure_Click(object sender, EventArgs e)
        {
            try
            {
                ExposureDataModel exposureRecord = exposureDataPresenter.ExposureDataList.Where(exp => exp.ExposureName.Equals(ultraTextEditorExposureName.Text.ToString())).FirstOrDefault();
                if (exposureRecord != null)
                {
                    UpdateViewForSearchOperation(exposureRecord);
                }
                else
                {
                    string errorString = string.Format("No Exposure Data found with Exposure Name '{0}' ", ultraTextEditorExposureName.Text.ToString());

                    MessageBox.Show(errorString, "Search Exposure", MessageBoxButtons.OK);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Searching the Exposure Record", "Search Exposure", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void RefreshExposure_Click(object sender, EventArgs e)
        {
            try
            {
                //ClearExposure();
                 exposureDataPresenter.GetExposureData(cidInterface);
                //await exposureDataPresenter.GetLimits();
                UpdateExposureView(0);
                //UpdateLimitsViewWithModelOnLoad();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateViewForSearchOperation(ExposureDataModel exposureRecord)
        {
            try
            {
                ultraLabelNumSubarrays.Text = "Total # of Subarrays:" + exposureRecord.SubarrayDatas.Count.ToString();

                ultraTextEditorExposureID.Text = exposureRecord.ExposureID.ToString();

                ultraTextEditorExposureName.Text = exposureRecord.ExposureName.ToString();

                numericEditDelay.Value = exposureRecord.GlobalInjectDelay;
                numericEditGlobalInject.Value = exposureRecord.GlobalInject;

                numericEditExposureInterval.Value = exposureRecord.ExposureInterval;
                numericEditNDRO.Value = exposureRecord.ExposureNDROS;
                numericEditExposureStartX.Value = exposureRecord.ExposureRegionXo;
                numericEditExposureWidth.Value = exposureRecord.ExposureRegiondX;
                numericEditExposureStartY.Value = exposureRecord.ExposureRegionYo;
                numericEditExposureHeight.Value = exposureRecord.ExposureRegiondY;


                ultraCheckEditorLED1.Checked = exposureRecord.LED1Enabled;
                ultraCheckEditorLED2.Checked = exposureRecord.LED2Enabled;
                ultraCheckEditorLED3.Checked = exposureRecord.LED3Enabled;
                ultraCheckEditorShutterEnabled.Checked = exposureRecord.ShutterEnabled;
                numericEditLEDOn.Value = exposureRecord.LEDOnTime;
                numericEditLEDOff.Value = exposureRecord.LEDOffTime;
                numericEditFlash.Value = exposureRecord.LEDFlashes;
                ultraCheckEditorAutoBias.Checked = Convert.ToBoolean(exposureRecord.AutoBiasFPNEnabled);

                if (exposureRecord.ExposureFPN == 0)
                {
                    ultraRadioButtonFPNNone.Checked = true;
                }
                else if (exposureRecord.ExposureFPN == 1)
                {
                    ultraRadioButtonFPNRunTime.Checked = true;
                }
                else if (exposureRecord.ExposureFPN == 3)
                {
                    ultraRadioButtonFPNStored.Checked = true;
                }


                ultraCheckEditorExposedFrame.Checked = exposureRecord.FullFrameEnabled;
                ultraCheckEditorDarkFrame.Checked = exposureRecord.DarkFrameEnabled;

                ultraTextEditorSubarrayID.Text = exposureRecord.SubarrayDatas[0].SubarrayID.ToString();
                ultraTextEditorSubarrayName.Text = exposureRecord.SubarrayDatas[0].SubarrayName.ToString();

                numericEditSubarrayStartX.Value = exposureRecord.SubarrayDatas[0].SubarrayRegionXo;
                numericEditSubarrayWidth.Value = exposureRecord.SubarrayDatas[0].SubarrayRegiondX;
                numericEditSubarrayStartY.Value = exposureRecord.SubarrayDatas[0].SubarrayRegionYo;
                numericEditSubarrayHeight.Value = exposureRecord.SubarrayDatas[0].SubarrayRegiondY;

                ultraCheckEditorSubarrayRead.Checked = exposureRecord.SubarrayDatas[0].SubarrayReadEnabled;
                numericEditSubarrayReadInterval.Value = exposureRecord.SubarrayDatas[0].SubarrayReadInterval;

                ultraCheckEditorSubarraySubInject.Checked = exposureRecord.SubarrayDatas[0].SubarraySubinjectEnabled;
                numericEditSubarraySubinjectInterval.Value = exposureRecord.SubarrayDatas[0].SubarraySubinjectInterval;
                ultraCheckEditorSubarrayPostRead.Checked = exposureRecord.SubarrayDatas[0].SubarrayPostReadEnabled;
                numericEditSubarrayNDROs.Value = exposureRecord.SubarrayDatas[0].SubarrayPostReadNDROS;

                numericEditSubarrayThresholdStartX.Value = exposureRecord.SubarrayDatas[0].SubarrayThresholdRegionXo;
                numericEditSubarrayThresholdWidth.Value = exposureRecord.SubarrayDatas[0].SubarrayThresholdRegiondX;
                numericEditSubarrayThresholdStartY.Value = exposureRecord.SubarrayDatas[0].SubarrayThresholdRegionYo;
                numericEditSubarrayThresholdHeight.Value = exposureRecord.SubarrayDatas[0].SubarrayThresholdRegiondY;
                ultraCheckEditorEnableThreshold.Checked = exposureRecord.SubarrayDatas[0].SubarrayThresholdEnabled;

                ultraCheckEditorTimeResolved.Checked = exposureRecord.SubarrayDatas[0].TimeResolvedEnabled;
                ultraCheckEditorAdaptive.Checked = exposureRecord.SubarrayDatas[0].AdaptiveEnabled;
                numericEditThresholdPercent.Value = exposureRecord.SubarrayDatas[0].SubarrayThresholdPercent;
                numericEditSubarrayTimeResolved.Value = exposureRecord.SubarrayDatas[0].TimeResolvedInterval;

                if (exposureRecord.SubarrayDatas[0].SubarrayFPN == 0)
                {
                    ultraRadioButtonSubarrayFPNNone.Checked = true;
                }
                else if (exposureRecord.SubarrayDatas[0].SubarrayFPN == 1)
                {
                    ultraRadioButtonSubarrayFPNRunTime.Checked = true;
                }
                else if (exposureRecord.SubarrayDatas[0].SubarrayFPN == 2)
                {
                    ultraRadioButtonSubarrayFPNStored.Checked = true;
                }

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private bool UpdateSubarrayModelForUpdateOperation(SubarrayDataModel SubarrayDataModel)
        {
            try
            {
                SubarrayDataModel.SubarrayID = Convert.ToInt32(ultraTextEditorSubarrayID.Value);
                SubarrayDataModel.ExposureID = Convert.ToInt32(ultraTextEditorExposureID.Value);
                SubarrayDataModel.UserModified = Environment.UserName;
                SubarrayDataModel.SubarrayName = ultraTextEditorSubarrayName.Text.ToString();
                SubarrayDataModel.SubarrayRegionXo = Convert.ToInt32(numericEditSubarrayStartX.Value);
                SubarrayDataModel.SubarrayRegiondX = Convert.ToInt32(numericEditSubarrayWidth.Value);
                SubarrayDataModel.SubarrayRegionYo = Convert.ToInt32(numericEditSubarrayStartY.Value);
                SubarrayDataModel.SubarrayRegiondY = Convert.ToInt32(numericEditSubarrayHeight.Value);

                SubarrayDataModel.SubarrayReadEnabled = ultraCheckEditorSubarrayRead.Checked;
                SubarrayDataModel.SubarrayReadInterval = Convert.ToInt32(numericEditSubarrayReadInterval.Value);

                SubarrayDataModel.SubarraySubinjectEnabled = ultraCheckEditorSubarraySubInject.Checked;
                SubarrayDataModel.SubarraySubinjectInterval = Convert.ToInt32(numericEditSubarraySubinjectInterval.Value);

                SubarrayDataModel.SubarrayPostReadEnabled = ultraCheckEditorSubarrayPostRead.Checked;
                SubarrayDataModel.SubarrayPostReadNDROS = Convert.ToInt32(numericEditSubarrayNDROs.Value);

                SubarrayDataModel.SubarrayThresholdRegionXo = Convert.ToInt32(numericEditSubarrayThresholdStartX.Value);
                SubarrayDataModel.SubarrayThresholdRegiondX = Convert.ToInt32(numericEditSubarrayThresholdWidth.Value);
                SubarrayDataModel.SubarrayThresholdRegionYo = Convert.ToInt32(numericEditSubarrayThresholdStartY.Value);
                SubarrayDataModel.SubarrayThresholdRegiondY = Convert.ToInt32(numericEditSubarrayThresholdHeight.Value);
                SubarrayDataModel.SubarrayThresholdEnabled = ultraCheckEditorEnableThreshold.Checked;

                SubarrayDataModel.TimeResolvedEnabled = ultraCheckEditorTimeResolved.Checked;
                SubarrayDataModel.AdaptiveEnabled = ultraCheckEditorAdaptive.Checked;
                SubarrayDataModel.SubarrayThresholdPercent = Convert.ToInt32(numericEditThresholdPercent.Value);
                SubarrayDataModel.TimeResolvedInterval = Convert.ToInt32(numericEditSubarrayTimeResolved.Value);

                if (ultraRadioButtonSubarrayFPNNone.Checked == true)
                {
                    SubarrayDataModel.SubarrayFPN = 0;
                }
                else if (ultraRadioButtonSubarrayFPNRunTime.Checked == true)
                {
                    SubarrayDataModel.SubarrayFPN = 1;
                }
                else if (ultraRadioButtonSubarrayFPNStored.Checked == true)
                {
                    SubarrayDataModel.SubarrayFPN = 3;
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
        private async void AddSubarray_Click(object sender, EventArgs e)
        {
            try
            {
                if (UpdateExposureDataModelFromView() && UpdateSubarrayDataModelFromView())
                {
                    int subarrayPerExposureCount = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas.Count();

                    if (!TestAppHelper.checkSubarrayCountForExposure(subarrayPerExposureCount))
                        return;

                    string addSubarray = "Are you sure to add Subarray " + ultraTextEditorSubarrayName.Text.ToString() + " to Exposure " + ultraTextEditorExposureName.Text.ToString() + " ?";

                    if (ultraTextEditorSubarrayName.Text.ToString() == string.Empty)
                    {
                        MessageBox.Show("Subarray name cannot be empty", "Add Subarray", MessageBoxButtons.OK);
                        return;
                    }
                    DialogResult result = MessageBox.Show(addSubarray, "Add Subarray", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        string errorString = string.Empty;
                        var errorStringBuilder = new StringBuilder();
                        //int count = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas.Count();
                        //UpdateSubarrayDataModelFromView();
                        //UpdateSubarrayDataModelFromView();
                        if (userMode == "Admin" && engineeringLoginStatus)
                        {
                            exposureDataPresenter.ExposureDataModel = exposureDataPresenter.ExposureDataList[currentRecord];
                            exposureDataPresenter.ExposureDataModel.NumberOfSubarrays = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas.Count + 1;

                            //Save to DB                        
                            bool saveDB = await exposureDataPresenter.SaveSubarrayDataToDB("ADD", userName, xmlFile);
                            if (saveDB)
                            {
                                //exposureDataPresenter.InMemoryModelChanged = false;
                                MessageBox.Show("Subarray Added Successfully", "Add Subarray", MessageBoxButtons.OK);
                                exposureDataPresenter.GetExposureData(cidInterface);
                            }
                            else if (exposureDataPresenter.ErrorCode == 547)
                            {
                                errorString = string.Format("You are trying to add a Subarray '{0}' to Exposure '{1}' which does not exist. Try adding the exposure {2} first", exposureDataPresenter.SubarrayDataModel.SubarrayName, exposureDataPresenter.ExposureDataList[currentRecord].ExposureName, exposureDataPresenter.ExposureDataList[currentRecord].ExposureName);

                                MessageBox.Show(errorString, "Add Subarray", MessageBoxButtons.OK);
                            }

                            else
                                MessageBox.Show("Error occured while adding Subarray", "Add Subarray", MessageBoxButtons.OK);
                        }

                        if (userMode == "Engineering")
                        {
                            exposureDataPresenter.SubarrayDataModel.SubarrayID = exposureDataPresenter.SubarrayDataModelList.Count + 1;
                            exposureDataPresenter.SubarrayDataModelList.Add(exposureDataPresenter.SubarrayDataModel);
                            exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas.Add(exposureDataPresenter.SubarrayDataModel);
                            ultraTextEditorSubarrayID.Text = exposureDataPresenter.SubarrayDataModel.SubarrayID.ToString();
                            if (subarrayPerExposureCount != exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas.Count())
                            {
                                MessageBox.Show("Subarray Added successfully", "Add Subarray", MessageBoxButtons.OK);
                            }
                            else
                                MessageBox.Show("Subarray not Added", "Add Subarray", MessageBoxButtons.OK);

                        }
                        ultraLabelNumberofExposures.Text = "Total # of Exposures:" + exposureDataPresenter.ExposureDataList.Count().ToString();
                        ultraLabelNumSubarrays.Text = "Total # of Subarrays:" + exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas.Count.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Adding Subarray", "Add Subarray", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void RemoveSubarray_Click(object sender, EventArgs e)
        {
            try
            {
                string errorString = string.Empty;
                string removeSubarray = "Are you sure to remove Subarray " + ultraTextEditorSubarrayName.Text.ToString() + " ?";
                DialogResult result = MessageBox.Show(removeSubarray, "Remove Subarray", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    List<ExposureDataModel> exposureList = exposureDataPresenter.ExposureDataList.Where(exp => exp.ExposureID.Equals(Convert.ToInt32(ultraTextEditorExposureID.Value))).ToList();

                    if (exposureList[0].SubarrayDatas.Count > 1)
                    {
                        bool subarrayInMemoryRemoved = false;
                        if (userMode == "Admin" && engineeringLoginStatus)
                        {
                            UpdateExposureDataModelFromView();
                            UpdateSubarrayDataModelFromView();
                            exposureDataPresenter.ExposureDataModel = exposureDataPresenter.ExposureDataList[currentRecord];
                            exposureDataPresenter.ExposureDataModel.NumberOfSubarrays = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas.Count - 1;
                            bool saveDB = await exposureDataPresenter.SaveSubarrayDataToDB("REMOVE", userName, xmlFile);
                            if (saveDB)
                            {
                                //exposureDataPresenter.InMemoryModelChanged = false;
                                MessageBox.Show("Subarray removed Successfully", "Remove Subarray", MessageBoxButtons.OK);
                                 exposureDataPresenter.GetExposureData(cidInterface);
                            }
                            else if (exposureDataPresenter.ErrorCode == 547)
                            {
                                errorString = string.Format("You are trying to remove a Subarray '{0}' which does not exist.", ultraTextEditorSubarrayName.Text.ToString());
                                MessageBox.Show(errorString, "Remove Subarray", MessageBoxButtons.OK);
                            }
                            else
                                MessageBox.Show("Error occured while removing Subarray", "Remove Subarray", MessageBoxButtons.OK);
                        }

                        if (userMode == "Engineering")
                        {
                            foreach (SubarrayDataModel sub in exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas)
                            {
                                if (sub.SubarrayID == Convert.ToInt32(ultraTextEditorSubarrayID.Value))
                                {
                                    subarrayInMemoryRemoved = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas.Remove(sub);
                                    if (exposureDataPresenter.SubarrayDataModelList.Remove(sub))
                                    {
                                        subarrayInMemoryRemoved = true;
                                        break;
                                    }

                                }
                            }
                            if (subarrayInMemoryRemoved)
                            {
                                MessageBox.Show("Subarray Removed successfully", "Remove Subarray", MessageBoxButtons.OK);

                            }
                            else
                                MessageBox.Show("Subarray not Removed", "Remove Subarray", MessageBoxButtons.OK);
                        }

                        if (currentSubarrayRecord == 0)
                        {
                            currentSubarrayRecord = currentSubarrayRecord + 1;
                        }
                        else
                        {
                            currentSubarrayRecord = currentSubarrayRecord - 1;
                        }
                        UpdateSubarrayView(currentSubarrayRecord);

                        ultraLabelNumberofExposures.Text = "Total # of Exposures:" + exposureDataPresenter.ExposureDataList.Count().ToString();
                        ultraLabelNumSubarrays.Text = "Total # of Subarrays:" + exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas.Count.ToString();
                    }
                    else
                    {
                        MessageBox.Show("There is only one Subarray for this Exposure. You cannot remove this subbarray ", "Remove Subarray", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Removing Subarray", "Remove Subarray", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void UpdateSubarray_Click(object sender, EventArgs e)
        {
            try
            {
                if (UpdateExposureDataModelFromView() && UpdateSubarrayDataModelFromView())
                {
                    string errorString = string.Empty;
                    string updateSubarray = "Are you sure to update Subarray " + ultraTextEditorSubarrayName + " ?";

                    DialogResult result = MessageBox.Show(updateSubarray, "Update Subarray", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {

                        if (userMode == "Admin" && engineeringLoginStatus)
                        {
                            //save data to DB
                            //UpdateSubarrayDataModelFromView();
                            //UpdateSubarrayDataModelFromView();
                            bool saveDB = await exposureDataPresenter.SaveSubarrayDataToDB("UPDATE", userName, xmlFile);
                            if (saveDB)
                            {
                                //exposureDataPresenter.InMemoryModelChanged = false;
                                MessageBox.Show("Subarray updated Successfully", "Update Subarray", MessageBoxButtons.OK);
                                exposureDataPresenter.GetExposureData(cidInterface);
                            }
                            else if (exposureDataPresenter.ErrorCode == 547)
                            {
                                errorString = string.Format("You are trying to update a Subarray '{0}' which does not exist.", ultraTextEditorSubarrayName.Text.ToString());
                                MessageBox.Show(errorString, "Update Subarray", MessageBoxButtons.OK);
                            }
                            else
                                MessageBox.Show("Error occured while updating Subarray", "Update Subarray", MessageBoxButtons.OK);
                        }

                        if (userMode == "Engineering")
                        {
                            List<ExposureDataModel> exposureList = exposureDataPresenter.ExposureDataList.Where(exp => exp.ExposureID.Equals(Convert.ToInt32(ultraTextEditorExposureID.Value))).ToList();
                            if (exposureList[0].SubarrayDatas.Count > 0)
                            {
                                foreach (SubarrayDataModel sub in exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas)
                                {
                                    if (sub.SubarrayID == Convert.ToInt32(ultraTextEditorSubarrayID.Value))
                                    {
                                        if (UpdateSubarrayModelForUpdateOperation(sub))
                                        {
                                            MessageBox.Show("Subarray Updated successfully", "Update Subarray", MessageBoxButtons.OK);
                                            //exposureDataPresenter.InMemoryModelChanged = true;
                                        }
                                        else
                                        {
                                            MessageBox.Show("Subarray not Updated", "Update Subarray", MessageBoxButtons.OK);
                                            // exposureDataPresenter.InMemoryModelChanged = false;
                                        }
                                        break;
                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Updating Subarray", "Update Subarray", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void FirstSubarray_Click(object sender, EventArgs e)
        {
            try
            {
                currentSubarrayRecord = 0;
                UpdateSubarrayView(currentSubarrayRecord);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Navigating Subarray", "Navigate Subarray", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void NextSubarray_Click(object sender, EventArgs e)
        {
            try
            {
                currentSubarrayRecord++;
                if (currentSubarrayRecord <= exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas.Count())
                {
                    UpdateSubarrayView(currentSubarrayRecord);
                }
                else
                {
                    currentSubarrayRecord--;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Navigating Subarray", "Navigate Subarray", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void PreviousSubarray_Click(object sender, EventArgs e)
        {
            try
            {
                currentSubarrayRecord--;
                if (currentSubarrayRecord >= 0)
                {
                    UpdateSubarrayView(currentSubarrayRecord);
                }
                else
                {
                    currentSubarrayRecord++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Navigating Subarray", "Navigate Subarray", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void LastSubarray_Click(object sender, EventArgs e)
        {
            try
            {
                currentSubarrayRecord = exposureDataPresenter.ExposureDataList[currentRecord].SubarrayDatas.Count();
                currentSubarrayRecord--;
                UpdateSubarrayView(currentSubarrayRecord);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Navigating Subarray", "Navigate Subarray", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ListExposureData_Click(object sender, EventArgs e)
        {
            try
            {
                listExposureData = new ListExposureData(userMode, engineeringLoginStatus, exposureType);
                listExposureData.ExposureDataModelList = exposureDataPresenter.ExposureDataList;
                listExposureData.ultraGridExposureData.Text = "Exposure Data";
                listExposureData.ShowDialog();
                if (listExposureData.DataUpdatedFromListExposureWindow)
                {
                    if ((userMode.Equals("Admin") && engineeringLoginStatus))
                    {
                        exposureDataPresenter.GetExposureData(cidInterface);
                    }
                }
                UpdateExposureView(currentRecord);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void checkBoxRepeatExposure_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxRepeatExposure.Checked)
                {
                    UpdateExposureDataModelFromView();
                    UpdateSubarrayDataModelFromView();
                    exposureDataPresenter.ExposureDataModel.NumberOfSubarrays = 1;
                    exposureDataPresenter.ExposureDataModel.SubarrayDatas = new List<SubarrayDataModel>();
                    exposureDataPresenter.ExposureDataModel.SubarrayDatas.Add(exposureDataPresenter.SubarrayDataModel);
                    TestAppHelper.repeatPhotoResponseExposure = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public List<ExposureDataModel> InitialExposureData()
        {
            try
            {               
                UpdateExposureDataModelFromView();
                UpdateSubarrayDataModelFromView(); 
                exposureDataPresenter.ExposureDataModel.NumberOfSubarrays = 1;
                exposureDataPresenter.ExposureDataModel.SubarrayDatas = new List<SubarrayDataModel>();
                exposureDataPresenter.ExposureDataModel.SubarrayDatas.Add(exposureDataPresenter.SubarrayDataModel);
                List<ExposureDataModel> templist = new List<ExposureDataModel>();
                templist.Add(exposureDataPresenter.ExposureDataModel);
                for (int i=0;i<199;i++)
                {
                    ExposureDataModel exposureDataModel = new ExposureDataModel();
                    exposureDataModel.AutoBiasFPNEnabled = exposureDataPresenter.ExposureDataModel.AutoBiasFPNEnabled;
                    exposureDataModel.DarkFrameEnabled = exposureDataPresenter.ExposureDataModel.DarkFrameEnabled;
                    exposureDataModel.DateDefined = exposureDataPresenter.ExposureDataModel.DateDefined;
                    exposureDataModel.ExposureFPN = exposureDataPresenter.ExposureDataModel.ExposureFPN;
                    exposureDataModel.ExposureID = exposureDataPresenter.ExposureDataModel.ExposureID;
                    exposureDataModel.ExposureInterval = exposureDataPresenter.ExposureDataModel.ExposureInterval;
                    exposureDataModel.ExposureName = exposureDataPresenter.ExposureDataModel.ExposureName;
                    exposureDataModel.ExposureNDROS = exposureDataPresenter.ExposureDataModel.ExposureNDROS;
                    exposureDataModel.ExposureRegiondX = exposureDataPresenter.ExposureDataModel.ExposureRegiondX;
                    exposureDataModel.ExposureRegiondY = exposureDataPresenter.ExposureDataModel.ExposureRegiondY;
                    exposureDataModel.ExposureRegionXo = exposureDataPresenter.ExposureDataModel.ExposureRegionXo;
                    exposureDataModel.ExposureRegionYo = exposureDataPresenter.ExposureDataModel.ExposureRegionYo;
                    exposureDataModel.FullFrameEnabled = exposureDataPresenter.ExposureDataModel.FullFrameEnabled;
                    exposureDataModel.GlobalInject =0;
                    exposureDataModel.GlobalInjectDelay = 0;
                    exposureDataModel.LED1Enabled = exposureDataPresenter.ExposureDataModel.LED1Enabled;
                    exposureDataModel.LED2Enabled = exposureDataPresenter.ExposureDataModel.LED2Enabled;
                    exposureDataModel.LED3Enabled = exposureDataPresenter.ExposureDataModel.LED3Enabled;
                    exposureDataModel.LEDFlashes = exposureDataPresenter.ExposureDataModel.LEDFlashes;
                    exposureDataModel.LEDOffTime = exposureDataPresenter.ExposureDataModel.LEDOffTime;
                    exposureDataModel.LEDOnTime = exposureDataPresenter.ExposureDataModel.LEDOnTime;
                    exposureDataModel.NumberOfSubarrays = exposureDataPresenter.ExposureDataModel.NumberOfSubarrays;
                    exposureDataModel.ShutterEnabled = exposureDataPresenter.ExposureDataModel.ShutterEnabled;
                    exposureDataModel.SubarrayDatas = exposureDataPresenter.ExposureDataModel.SubarrayDatas;
                    exposureDataModel.TestDataID = exposureDataPresenter.ExposureDataModel.TestDataID;
                    exposureDataModel.UserDefaultConfiguration = exposureDataPresenter.ExposureDataModel.UserDefaultConfiguration;
                    exposureDataModel.UserModified = exposureDataPresenter.ExposureDataModel.UserModified;
                    templist.Add(exposureDataModel);

                }
                return templist;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }

        }
    }
}
