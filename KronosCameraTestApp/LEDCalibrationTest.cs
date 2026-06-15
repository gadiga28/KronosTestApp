using CIDSoftwareApplication;
using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.Presenter;
using KronosCameraTestApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Thermo.Kronos.Instrument.Camera.Interface;

namespace KronosCameraTestApp
{
    public class LEDCalibrationTest
    {
        KronosTestAppMainForm kronostestappMainForm { get; set; }
        CIDInterface cidInterface = null;
        KronosCamContext kronosCamContext { get; set; }
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(
          System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LEDCalibrationPresenter ledCalibrationPresenter { get; set; }
        LEDCalibrationTestResults ledCalibrationTestResults;
        public LEDCalibrationTestResults LEDCalibrationTestResults
        {
            get { return ledCalibrationTestResults; }
            set { ledCalibrationTestResults = value; }
        }
        double[] redmean;
        public double[] RedMean
        {
            get { return redmean; }
            set { redmean = value; }
        }
        double[] greenmean;
        public double[] GreenMean
        {
            get { return greenmean; }
            set { greenmean = value; }
        }
        double[] bluemean;
        public double[] BlueMean
        {
            get { return bluemean; }
            set { bluemean = value; }
        }
        List<ExposureDataModel> ledCalibrationExposureDataList = new List<ExposureDataModel>();
        public LEDCalibrationTest()
        {
            cidInterface = new CIDInterface();
            ledCalibrationPresenter = new LEDCalibrationPresenter();
            //kronostestappMainForm = new KronosTestAppMainForm();
        }
        public LEDCalibrationTest(KronosTestAppMainForm kronostestappMainForm, CIDInterface cidInterface, LEDCalibrationPresenter ledCalibrationPresenter)
        {
            this.kronostestappMainForm = kronostestappMainForm;
            this.cidInterface = cidInterface;
            this.ledCalibrationPresenter = ledCalibrationPresenter;
            ledCalibrationExposureDataList = new List<ExposureDataModel>();
        }
        public async Task RunLEDCalibration()
        {
            try
            {
                log.Debug("Run LED Calibration test.");
                BuildRedLEDCalibExposureList();
                BuildGreenLEDCalibExposureList();
                BuildBlueLEDCalibExposureList();
                CancellationToken ct = new CancellationToken();
                await ledCalibrationPresenter.GetLimits();
                await runLEDCalibrationTest(ct);
                await CalculateLEDCalibration();
                ledCalibrationTestResults = await SaveLEDCalibrationTestResults();
                if(await ledCalibrationPresenter.SaveLEDCalibrationResults(ledCalibrationTestResults))
                {
                    if (TestAppHelper.LEDCalibIntervalConfig > TestAppHelper.LEDCalibSavedCounter)
                        TestAppHelper.LEDCalibSavedCounter++;
                    //else
                    //    TestAppHelper.LEDCalibSavedCounter--;
                }
                    
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task<LEDCalibrationTestResults> SaveLEDCalibrationTestResults()
        {
            try
            {
                List<LEDCalibrationTestResults> lEDCalibrationTestResultList = await ledCalibrationPresenter.GetLEDCalibrationResults(Environment.MachineName);                
                int testCounter = 0;
                if (lEDCalibrationTestResultList.Count > 0)
                {
                    LEDCalibrationTestResults LEDCalibrationTestResult = lEDCalibrationTestResultList[0];
                    testCounter = LEDCalibrationTestResult.TestCounter;
                    if (testCounter >= ledCalibrationPresenter.LEDCalibrationLimitsDataList[0].CurrentTestInterval)
                    {
                        testCounter = 1;
                    }
                    else
                    {
                        testCounter++;
                    }
                }
                else
                {
                    testCounter++;
                }
               
                LEDCalibrationTestResults ledCalibrationTestResults = new LEDCalibrationTestResults();
                ledCalibrationTestResults.DateExecuted = DateTime.Now;
                ledCalibrationTestResults.UserExecuted = Environment.UserName;
                ledCalibrationTestResults.Workstation = Environment.MachineName;
                ledCalibrationTestResults.ECONumber = ledCalibrationPresenter.LEDCalibrationLimitsDataList[0].ECONumber;
                ledCalibrationTestResults.Red40Actual = redmean[40];
                ledCalibrationTestResults.Red60Actual = redmean[60];
                ledCalibrationTestResults.Green40Actual = greenmean[40];
                ledCalibrationTestResults.Green60Actual = greenmean[60];
                ledCalibrationTestResults.Blue40Actual = bluemean[40];
                ledCalibrationTestResults.Blue60Actual = bluemean[60];
                ledCalibrationTestResults.TestCounter = testCounter;

                double ledLimitOffset = ledCalibrationPresenter.LEDCalibrationLimitsDataList[0].LEDLimitOffset;
                double red40Limit = ledCalibrationPresenter.LEDCalibrationLimitsDataList[0].Red40LEDLimit;
                double red60Limit = ledCalibrationPresenter.LEDCalibrationLimitsDataList[0].Red60LEDLimit;
                double green40Limit = ledCalibrationPresenter.LEDCalibrationLimitsDataList[0].Green40LEDLimit;
                double green60Limit = ledCalibrationPresenter.LEDCalibrationLimitsDataList[0].Green60LEDLimit;
                double blue40Limit = ledCalibrationPresenter.LEDCalibrationLimitsDataList[0].Blue40LEDLimit;
                double blue60Limit = ledCalibrationPresenter.LEDCalibrationLimitsDataList[0].Blue60LEDLimit;

                if ((redmean[40] < (red40Limit+ ledLimitOffset)) &&
                    (redmean[40] > (red40Limit -ledLimitOffset)))
                {
                    ledCalibrationTestResults.Red40Passed = true;
                }
                else
                {
                    ledCalibrationTestResults.Red40Passed = false;
                }
                if ((redmean[60] < (red60Limit + ledLimitOffset)) &&
                    (redmean[60] > (red60Limit -ledLimitOffset)))
                {
                    ledCalibrationTestResults.Red60Passed = true;
                }
                else
                {
                    ledCalibrationTestResults.Red60Passed = false;
                }

                if ((greenmean[40] < (green40Limit +ledLimitOffset)) &&
                    (greenmean[40] > (green40Limit -ledLimitOffset)))
                {
                    ledCalibrationTestResults.Green40Passed = true;
                }
                else
                {
                    ledCalibrationTestResults.Green40Passed = false;
                }
                if ((greenmean[60] < (green60Limit + ledLimitOffset)) &&
                    (greenmean[60] > (green60Limit - ledLimitOffset)))
                {
                    ledCalibrationTestResults.Green60Passed = true;
                }
                else
                {
                    ledCalibrationTestResults.Green60Passed = false;
                }

                if ((bluemean[40] < (blue40Limit + ledLimitOffset)) &&
                   (bluemean[40] > (blue40Limit - ledLimitOffset)))
                {
                    ledCalibrationTestResults.Blue40Passed = true;
                }
                else
                {
                    ledCalibrationTestResults.Blue40Passed = false;
                }
                if ((bluemean[60] < (blue60Limit + ledLimitOffset)) &&
                    (bluemean[60] > (blue60Limit - ledLimitOffset)))
                {
                    ledCalibrationTestResults.Blue60Passed = true;
                }
                else
                {
                    ledCalibrationTestResults.Blue60Passed = false;
                }
                return ledCalibrationTestResults;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }
        private void BuildRedLEDCalibExposureList()
        {
            try
            {
                ExposureDataModel redExposure = new ExposureDataModel();
                redExposure.ExposureID = 0;
                redExposure.ExposureName = "redexp";
                redExposure.ExposureInterval = 10;
                redExposure.ExposureNDROS = 1;
                redExposure.ExposureRegionXo = 1447;
                redExposure.ExposureRegionYo = 1077;
                redExposure.ExposureRegiondX = 6;
                redExposure.ExposureRegiondY = 20;
                redExposure.ExposureFPN = 1;
                redExposure.LEDOnTime = 450;
                redExposure.LEDOffTime = 100;
                redExposure.LEDFlashes = 1;
                redExposure.LED1Enabled = true;
                redExposure.LED2Enabled = false;
                redExposure.LED3Enabled = false;
                redExposure.ShutterEnabled = false;
                redExposure.AutoBiasFPNEnabled = 1;
                redExposure.FullFrameEnabled = false;
                redExposure.DarkFrameEnabled = false;
                redExposure.GlobalInject = 5;
                redExposure.GlobalInjectDelay = 1;
                redExposure.NumberOfSubarrays = 1;
                SubarrayDataModel subarrayDataModel = new SubarrayDataModel();
                subarrayDataModel.SubarrayID = 0;
                subarrayDataModel.SubarrayName = "Subarray1";
                subarrayDataModel.SubarrayRegionXo = 1447;
                subarrayDataModel.SubarrayRegionYo = 1077;
                subarrayDataModel.SubarrayRegiondX = 6;
                subarrayDataModel.SubarrayRegiondY = 20;
                subarrayDataModel.SubarrayFPN = 1;
                subarrayDataModel.SubarrayReadEnabled = true;
                subarrayDataModel.SubarrayReadInterval = 10;
                subarrayDataModel.SubarraySubinjectEnabled = false;
                subarrayDataModel.SubarraySubinjectInterval = 0;
                subarrayDataModel.SubarrayPostReadEnabled = false;
                subarrayDataModel.SubarrayPostReadNDROS = 1;
                subarrayDataModel.SubarrayThresholdEnabled = false;
                subarrayDataModel.SubarrayThresholdPercent = 75;
                subarrayDataModel.SubarrayThresholdRegionXo = 0;
                subarrayDataModel.SubarrayThresholdRegionYo = 0;
                subarrayDataModel.SubarrayThresholdRegiondX = 0;
                subarrayDataModel.SubarrayThresholdRegiondY = 0;
                subarrayDataModel.TimeResolvedEnabled = false;
                subarrayDataModel.AdaptiveEnabled = false;
                redExposure.SubarrayDatas = new List<SubarrayDataModel>();
                redExposure.SubarrayDatas.Add(subarrayDataModel);
                ledCalibrationExposureDataList.Add(redExposure);
                for (int i = 1; i < 200; i++)
                {
                    ExposureDataModel redExposure1 = new ExposureDataModel();
                    redExposure1.ExposureID = i;
                    redExposure1.ExposureName = "redexp";
                    redExposure1.ExposureInterval = 10;
                    redExposure1.ExposureNDROS = 1;
                    redExposure1.ExposureRegionXo = 1447;
                    redExposure1.ExposureRegionYo = 1077;
                    redExposure1.ExposureRegiondX = 6;
                    redExposure1.ExposureRegiondY = 20;
                    redExposure1.ExposureFPN = 1;
                    redExposure1.LEDOnTime = 450;
                    redExposure1.LEDOffTime = 100;
                    redExposure1.LEDFlashes = 1;
                    redExposure1.LED1Enabled = true;
                    redExposure1.LED2Enabled = false;
                    redExposure1.LED3Enabled = false;
                    redExposure1.ShutterEnabled = false;
                    redExposure1.AutoBiasFPNEnabled = 1;
                    redExposure1.FullFrameEnabled = false;
                    redExposure1.DarkFrameEnabled = false;
                    redExposure1.GlobalInject = 0;
                    redExposure1.GlobalInjectDelay = 0;
                    redExposure1.NumberOfSubarrays = 1;
                    SubarrayDataModel subarrayDataModel1 = new SubarrayDataModel();
                    subarrayDataModel1.SubarrayID = i;
                    subarrayDataModel1.SubarrayName = "Subarray1";
                    subarrayDataModel1.SubarrayRegionXo = 1447;
                    subarrayDataModel1.SubarrayRegionYo = 1077;
                    subarrayDataModel1.SubarrayRegiondX = 6;
                    subarrayDataModel1.SubarrayRegiondY = 20;
                    subarrayDataModel1.SubarrayFPN = 1;
                    subarrayDataModel1.SubarrayReadEnabled = true;
                    subarrayDataModel1.SubarrayReadInterval = 10;
                    subarrayDataModel1.SubarraySubinjectEnabled = false;
                    subarrayDataModel1.SubarraySubinjectInterval = 0;
                    subarrayDataModel1.SubarrayPostReadEnabled = false;
                    subarrayDataModel1.SubarrayPostReadNDROS = 1;
                    subarrayDataModel1.SubarrayThresholdEnabled = false;
                    subarrayDataModel1.SubarrayThresholdPercent = 75;
                    subarrayDataModel1.SubarrayThresholdRegionXo = 0;
                    subarrayDataModel1.SubarrayThresholdRegionYo = 0;
                    subarrayDataModel1.SubarrayThresholdRegiondX = 0;
                    subarrayDataModel1.SubarrayThresholdRegiondY = 0;
                    subarrayDataModel1.TimeResolvedEnabled = false;
                    subarrayDataModel1.AdaptiveEnabled = false;
                    redExposure1.SubarrayDatas = new List<SubarrayDataModel>();
                    redExposure1.SubarrayDatas.Add(subarrayDataModel1);
                    ledCalibrationExposureDataList.Add(redExposure1);
                }

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void BuildGreenLEDCalibExposureList()
        {
            try
            {
                ExposureDataModel greenExposure = new ExposureDataModel();
                greenExposure.ExposureID = 0;
                greenExposure.ExposureName = "greenexp";
                greenExposure.ExposureInterval = 10;
                greenExposure.ExposureNDROS = 1;
                greenExposure.ExposureRegionXo = 1447;
                greenExposure.ExposureRegionYo = 1077;
                greenExposure.ExposureRegiondX = 6;
                greenExposure.ExposureRegiondY = 20;
                greenExposure.ExposureFPN = 1;
                greenExposure.LEDOnTime = 450;
                greenExposure.LEDOffTime = 100;
                greenExposure.LEDFlashes = 1;
                greenExposure.LED1Enabled = false;
                greenExposure.LED2Enabled = true;
                greenExposure.LED3Enabled = false;
                greenExposure.ShutterEnabled = false;
                greenExposure.AutoBiasFPNEnabled = 1;
                greenExposure.FullFrameEnabled = false;
                greenExposure.DarkFrameEnabled = false;
                greenExposure.GlobalInject = 5;
                greenExposure.GlobalInjectDelay = 1;
                greenExposure.NumberOfSubarrays = 1;
                SubarrayDataModel subarrayDataModel = new SubarrayDataModel();
                subarrayDataModel.SubarrayID = 0;
                subarrayDataModel.SubarrayName = "Subarray1";
                subarrayDataModel.SubarrayRegionXo = 1447;
                subarrayDataModel.SubarrayRegionYo = 1077;
                subarrayDataModel.SubarrayRegiondX = 6;
                subarrayDataModel.SubarrayRegiondY = 20;
                subarrayDataModel.SubarrayFPN = 1;
                subarrayDataModel.SubarrayReadEnabled = true;
                subarrayDataModel.SubarrayReadInterval = 10;
                subarrayDataModel.SubarraySubinjectEnabled = false;
                subarrayDataModel.SubarraySubinjectInterval = 0;
                subarrayDataModel.SubarrayPostReadEnabled = false;
                subarrayDataModel.SubarrayPostReadNDROS = 1;
                subarrayDataModel.SubarrayThresholdEnabled = false;
                subarrayDataModel.SubarrayThresholdPercent = 75;
                subarrayDataModel.SubarrayThresholdRegionXo = 0;
                subarrayDataModel.SubarrayThresholdRegionYo = 0;
                subarrayDataModel.SubarrayThresholdRegiondX = 0;
                subarrayDataModel.SubarrayThresholdRegiondY = 0;
                subarrayDataModel.TimeResolvedEnabled = false;
                subarrayDataModel.AdaptiveEnabled = false;
                greenExposure.SubarrayDatas = new List<SubarrayDataModel>();
                greenExposure.SubarrayDatas.Add(subarrayDataModel);
                ledCalibrationExposureDataList.Add(greenExposure);
                for (int i = 1; i < 200; i++)
                {
                    ExposureDataModel greenExposure1 = new ExposureDataModel();
                    greenExposure1.ExposureID = i;
                    greenExposure1.ExposureName = "greenexp";
                    greenExposure1.ExposureInterval = 10;
                    greenExposure1.ExposureNDROS = 1;
                    greenExposure1.ExposureRegionXo = 1447;
                    greenExposure1.ExposureRegionYo = 1077;
                    greenExposure1.ExposureRegiondX = 6;
                    greenExposure1.ExposureRegiondY = 20;
                    greenExposure1.ExposureFPN = 1;
                    greenExposure1.LEDOnTime = 450;
                    greenExposure1.LEDOffTime = 100;
                    greenExposure1.LEDFlashes = 1;
                    greenExposure1.LED1Enabled = false;
                    greenExposure1.LED2Enabled = true;
                    greenExposure1.LED3Enabled = false;
                    greenExposure1.ShutterEnabled = false;
                    greenExposure1.AutoBiasFPNEnabled = 1;
                    greenExposure1.FullFrameEnabled = false;
                    greenExposure1.DarkFrameEnabled = false;
                    greenExposure1.GlobalInject = 0;
                    greenExposure1.GlobalInjectDelay = 0;
                    greenExposure1.NumberOfSubarrays = 1;
                    SubarrayDataModel subarrayDataModel1 = new SubarrayDataModel();
                    subarrayDataModel1.SubarrayID = i;
                    subarrayDataModel1.SubarrayName = "Subarray1";
                    subarrayDataModel1.SubarrayRegionXo = 1447;
                    subarrayDataModel1.SubarrayRegionYo = 1077;
                    subarrayDataModel1.SubarrayRegiondX = 6;
                    subarrayDataModel1.SubarrayRegiondY = 20;
                    subarrayDataModel1.SubarrayFPN = 1;
                    subarrayDataModel1.SubarrayReadEnabled = true;
                    subarrayDataModel1.SubarrayReadInterval = 10;
                    subarrayDataModel1.SubarraySubinjectEnabled = false;
                    subarrayDataModel1.SubarraySubinjectInterval = 0;
                    subarrayDataModel1.SubarrayPostReadEnabled = false;
                    subarrayDataModel1.SubarrayPostReadNDROS = 1;
                    subarrayDataModel1.SubarrayThresholdEnabled = false;
                    subarrayDataModel1.SubarrayThresholdPercent = 75;
                    subarrayDataModel1.SubarrayThresholdRegionXo = 0;
                    subarrayDataModel1.SubarrayThresholdRegionYo = 0;
                    subarrayDataModel1.SubarrayThresholdRegiondX = 0;
                    subarrayDataModel1.SubarrayThresholdRegiondY = 0;
                    subarrayDataModel1.TimeResolvedEnabled = false;
                    subarrayDataModel1.AdaptiveEnabled = false;
                    greenExposure1.SubarrayDatas = new List<SubarrayDataModel>();
                    greenExposure1.SubarrayDatas.Add(subarrayDataModel1);
                    ledCalibrationExposureDataList.Add(greenExposure1);
                }

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void BuildBlueLEDCalibExposureList()
        {
            try
            {
                ExposureDataModel blueExposure = new ExposureDataModel();
                blueExposure.ExposureID = 0;
                blueExposure.ExposureName = "blueexp";
                blueExposure.ExposureInterval = 10;
                blueExposure.ExposureNDROS = 1;
                blueExposure.ExposureRegionXo = 1447;
                blueExposure.ExposureRegionYo = 1077;
                blueExposure.ExposureRegiondX = 6;
                blueExposure.ExposureRegiondY = 20;
                blueExposure.ExposureFPN = 1;
                blueExposure.LEDOnTime = 450;
                blueExposure.LEDOffTime = 100;
                blueExposure.LEDFlashes = 1;
                blueExposure.LED1Enabled = false;
                blueExposure.LED2Enabled = false;
                blueExposure.LED3Enabled = true;
                blueExposure.ShutterEnabled = false;
                blueExposure.AutoBiasFPNEnabled = 1;
                blueExposure.FullFrameEnabled = false;
                blueExposure.DarkFrameEnabled = false;
                blueExposure.GlobalInject = 5;
                blueExposure.GlobalInjectDelay = 1;
                blueExposure.NumberOfSubarrays = 1;
                SubarrayDataModel subarrayDataModel = new SubarrayDataModel();
                subarrayDataModel.SubarrayID = 0;
                subarrayDataModel.SubarrayName = "Subarray1";
                subarrayDataModel.SubarrayRegionXo = 1447;
                subarrayDataModel.SubarrayRegionYo = 1077;
                subarrayDataModel.SubarrayRegiondX = 6;
                subarrayDataModel.SubarrayRegiondY = 20;
                subarrayDataModel.SubarrayFPN = 1;
                subarrayDataModel.SubarrayReadEnabled = true;
                subarrayDataModel.SubarrayReadInterval = 10;
                subarrayDataModel.SubarraySubinjectEnabled = false;
                subarrayDataModel.SubarraySubinjectInterval = 0;
                subarrayDataModel.SubarrayPostReadEnabled = false;
                subarrayDataModel.SubarrayPostReadNDROS = 1;
                subarrayDataModel.SubarrayThresholdEnabled = false;
                subarrayDataModel.SubarrayThresholdPercent = 75;
                subarrayDataModel.SubarrayThresholdRegionXo = 0;
                subarrayDataModel.SubarrayThresholdRegionYo = 0;
                subarrayDataModel.SubarrayThresholdRegiondX = 0;
                subarrayDataModel.SubarrayThresholdRegiondY = 0;
                subarrayDataModel.TimeResolvedEnabled = false;
                subarrayDataModel.AdaptiveEnabled = false;
                blueExposure.SubarrayDatas = new List<SubarrayDataModel>();
                blueExposure.SubarrayDatas.Add(subarrayDataModel);
                ledCalibrationExposureDataList.Add(blueExposure);
                for (int i = 1; i < 200; i++)
                {
                    ExposureDataModel blueExposure1 = new ExposureDataModel();
                    blueExposure1.ExposureID = i;
                    blueExposure1.ExposureName = "blueexp";
                    blueExposure1.ExposureInterval = 10;
                    blueExposure1.ExposureNDROS = 1;
                    blueExposure1.ExposureRegionXo = 1447;
                    blueExposure1.ExposureRegionYo = 1077;
                    blueExposure1.ExposureRegiondX = 6;
                    blueExposure1.ExposureRegiondY = 20;
                    blueExposure1.ExposureFPN = 1;
                    blueExposure1.LEDOnTime = 450;
                    blueExposure1.LEDOffTime = 100;
                    blueExposure1.LEDFlashes = 1;
                    blueExposure1.LED1Enabled = false;
                    blueExposure1.LED2Enabled = false;
                    blueExposure1.LED3Enabled = true;
                    blueExposure1.ShutterEnabled = false;
                    blueExposure1.AutoBiasFPNEnabled = 1;
                    blueExposure1.FullFrameEnabled = false;
                    blueExposure1.DarkFrameEnabled = false;
                    blueExposure1.GlobalInject = 0;
                    blueExposure1.GlobalInjectDelay = 0;
                    blueExposure1.NumberOfSubarrays = 1;
                    SubarrayDataModel subarrayDataModel1 = new SubarrayDataModel();
                    subarrayDataModel1.SubarrayID = i;
                    subarrayDataModel1.SubarrayName = "Subarray1";
                    subarrayDataModel1.SubarrayRegionXo = 1447;
                    subarrayDataModel1.SubarrayRegionYo = 1077;
                    subarrayDataModel1.SubarrayRegiondX = 6;
                    subarrayDataModel1.SubarrayRegiondY = 20;
                    subarrayDataModel1.SubarrayFPN = 1;
                    subarrayDataModel1.SubarrayReadEnabled = true;
                    subarrayDataModel1.SubarrayReadInterval = 10;
                    subarrayDataModel1.SubarraySubinjectEnabled = false;
                    subarrayDataModel1.SubarraySubinjectInterval = 0;
                    subarrayDataModel1.SubarrayPostReadEnabled = false;
                    subarrayDataModel1.SubarrayPostReadNDROS = 1;
                    subarrayDataModel1.SubarrayThresholdEnabled = false;
                    subarrayDataModel1.SubarrayThresholdPercent = 75;
                    subarrayDataModel1.SubarrayThresholdRegionXo = 0;
                    subarrayDataModel1.SubarrayThresholdRegionYo = 0;
                    subarrayDataModel1.SubarrayThresholdRegiondX = 0;
                    subarrayDataModel1.SubarrayThresholdRegiondY = 0;
                    subarrayDataModel1.TimeResolvedEnabled = false;
                    subarrayDataModel1.AdaptiveEnabled = false;
                    blueExposure1.SubarrayDatas = new List<SubarrayDataModel>();
                    blueExposure1.SubarrayDatas.Add(subarrayDataModel1);
                    ledCalibrationExposureDataList.Add(blueExposure1);
                }

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task runLEDCalibrationTest(CancellationToken ct)
        {
            try
            {
                if (!ct.IsCancellationRequested)
                {
                    kronostestappMainForm.cidDataList.Clear();
                    kronostestappMainForm.cidIntegratedDataList.Clear();
                    log.Info("Begin LED Calibration test.");
                    CommandProcessor commandProcessor = new CommandProcessor(cidInterface);
                    if (cidInterface != null)
                        cidInterface.ResetExposureId();
                    await commandProcessor.ProcessExposureData(ledCalibrationExposureDataList, ct);
                    //send exposure data to camera
                    await commandProcessor.SendExposure(ct, ledCalibrationExposureDataList.Count, cidInterface);
                   
                    log.Info("End LED Calibration test.");
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task CalculateLEDCalibration()
        {
            try
            {
                await Task.Run(() =>
                {
                    while (kronostestappMainForm.cidIntegratedDataList.Count < 600)
                    {
                        if (!cidInterface.IsConnected())
                        {
                           // MessageBox.Show("Camera got disconnected, Trying to reconnect.....");
                            log.Error("Camera disconnected during Photoresonse test " + kronostestappMainForm.cidIntegratedDataList.Count.ToString());
                            //cidInterface.Disconnect();
                            //kronostestappMainForm.AutoConnectCamera();
                            //if (cidInterface.IsConnected())
                                //MessageBox.Show("Camera reconnected successfully.");
                        }
                    }

                    CID821_Data cid821Data = new CID821_Data();
                    cid821Data = kronostestappMainForm.cidIntegratedDataList[0];
                    int length = (cid821Data.dr) * (cid821Data.dc);
                    double[] ROI1 = new double[length];
                    int numberOfPlots = (kronostestappMainForm.cidIntegratedDataList.Count / 3) - 1;
                    redmean = new double[numberOfPlots];
                    greenmean = new double[numberOfPlots];
                    bluemean = new double[numberOfPlots];
                    // Calibrate all 3 LEDs(Red,Green,and Blue)           
                    int redIndex = 0;
                    for (int i = 0; i < numberOfPlots; i++)
                    {
                        ROI1 = TestAppHelper.convertVideoDataToDouble(kronostestappMainForm.cidIntegratedDataList[i].videoIntegratedDataList);
                        if (i == 0)
                            redmean[redIndex] = 0;
                        else
                            redmean[redIndex] = (TestAppHelper.calculateMeanDouble(ROI1));
                        redIndex++;
                    }

                    int greenIndex = 0;
                    for (int i = (numberOfPlots + 1); i < (numberOfPlots * 2) + 1; i++)
                    {
                        ROI1 = TestAppHelper.convertVideoDataToDouble(kronostestappMainForm.cidIntegratedDataList[i].videoIntegratedDataList);
                        if (i == 0)
                            greenmean[greenIndex] = 0;
                        else
                            greenmean[greenIndex] = (TestAppHelper.calculateMeanDouble(ROI1));
                        greenIndex++;
                    }

                    int blueIndex = 0;
                    for (int i = (numberOfPlots * 2) + 2; i < (numberOfPlots * 3) + 2; i++)
                    {
                        ROI1 = TestAppHelper.convertVideoDataToDouble(kronostestappMainForm.cidIntegratedDataList[i].videoIntegratedDataList);
                        if (i == 0)
                            bluemean[blueIndex] = 0;
                        else
                            bluemean[blueIndex] = (TestAppHelper.calculateMeanDouble(ROI1));
                        blueIndex++;
                    }

                });
                log.Info("End LED Calibration Test");
                kronostestappMainForm.cidDataList.Clear();
                kronostestappMainForm.cidIntegratedDataList.Clear();

            }
            catch (Exception ex)
            {

                kronostestappMainForm.cidDataList.Clear();
                kronostestappMainForm.cidIntegratedDataList.Clear();
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        } 
    }
}
