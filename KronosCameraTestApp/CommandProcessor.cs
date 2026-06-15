using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using NationalInstruments.UI;
using NationalInstruments.UI.WindowsForms;
using CyUSB;
using System.Net.Sockets;
using Thermo.Kronos.Instrument.Camera.Interface;
using KronosCameraTestApp.Model;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Data;
using System.Configuration;
using System.Xml.Linq;
using KronosCameraTestApp.Helpers;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using Thermo.Kronos.Instrument.Camera.Contracts.Data;
using CIDSoftwareApplication;
using KronosCameraTestApp.View;
namespace KronosCameraTestApp
{
    public class CommandProcessor
    {
        // Add single line for logger
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);      
        public List<KeyValuePair<string, SCM5821AX1DataTypes.ExposureStructure>> exposureList;
        List<SCM5821AX1DataTypes.SubarrayStructure> subarrayList;
        List<int> DelayList;
        List<int> GIList;
        UInt32 exposureInterval = 0; // new change
        private TcpClient tcpClient;
        private CIDInterface cidInterface;
        public bool dbchanged = false;
        public  int errorCode = 0;
         KronosTestAppMainForm kronosTestAppMainForm { get; set; }
        private List<KeyValuePair<string, SubarrayRegion.SubarrayRegionStructure>> subarrayRegionList;
        List<ExposureDataModel> exposureDataList = new List<ExposureDataModel>();
        List<SubarrayDataModel> subarrayDataModelList = new List<SubarrayDataModel>();
        public List<ExposureDataModel> ExposureDataList
        {
            get { return exposureDataList; }
            set { exposureDataList = value; }
        }
        public List<SubarrayDataModel> SubarrayDataModelList
        {
            get { return subarrayDataModelList; }
            set { subarrayDataModelList = value; }
        }
        public CommandProcessor(CIDInterface cidInterface)
        {
            exposureList = new List<KeyValuePair<string, SCM5821AX1DataTypes.ExposureStructure>>();          
            this.cidInterface = cidInterface;
        }
        public void warmupTest(CIDInterface cidInterface)
        {
            if (cidInterface.IsConnected())
            {
                cidInterface.ResetExposureId();
                CameraExposureSettings cameraExposureSettings = new CameraExposureSettings();
                CameraRegion region = new CameraRegion();
                cameraExposureSettings.Name = "WarmUp";
                cameraExposureSettings.ExposureId = 1;
                cameraExposureSettings.ExposureInterval = 100;
                cameraExposureSettings.FullframeEnabled = true;
                cameraExposureSettings.FixedPatternNoiseReduction = 0;
                cameraExposureSettings.Led1Enabled = false;
                cameraExposureSettings.Led2Enabled = false;
                cameraExposureSettings.Led3Enabled = false;
                cameraExposureSettings.LedFlashes = 0;
                cameraExposureSettings.LedOnTime = 0;
                cameraExposureSettings.LedOffTime = 0;
                region.PositionX = 0;   
                region.PositionY = 0;
                region.Width = 2048;
                region.Height = 2048;
                cameraExposureSettings.Region = region;
                cameraExposureSettings.AutoBiasEnabled = false;
                cameraExposureSettings.ShutterEnabled = false;
                cameraExposureSettings.DarkframeEnabled = false;
                cameraExposureSettings.NumberOfNonDestructiveReadouts = 1;
                CameraSubarraySettings subarray = new CameraSubarraySettings();
                subarray.Id = 0;
                subarray.PostReadNonDestructiveReadouts = 1;
                subarray.Region = new CameraRegion();
                subarray.TresholdRegion = new CameraRegion();
                subarray.Name = "Sub0";
                cameraExposureSettings.Subarrays = new List<CameraSubarraySettings>();
                cameraExposureSettings.GlobalInjectTime = 1;
                cameraExposureSettings.DelayAfterGlobalInject = 5;
                var result = cidInterface.PerformExposure(cameraExposureSettings);
            }
        }
        public void calculateImageTestData(CancellationToken ct)
        {
            try
            {
                while (TestAppHelper.kronosTestAppMainForm.cidIntegratedDataList.Count < 1)
                {
                    if (ct.IsCancellationRequested)
                    {
                        TestAppHelper.kronosTestAppMainForm.cidIntegratedDataList.Clear();
                        return;
                    }
                }
                TestAppHelper.kronosTestAppMainForm.cidDataList.Clear();
                TestAppHelper.kronosTestAppMainForm.cidIntegratedDataList.Clear();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task ProcessExposureData(List<ExposureDataModel> exposureDataModelList, CancellationToken ct)
        {
            try
            {
                log.Info("Begin process Exposure data");
                await Task.Run(() =>
                {                    
                    SCM5821AX1DataTypes.ExposureStructure exposure;
                    var configList = new List<KeyValuePair<string, SCM5821AX1DataTypes.ConfigurationStructure>>();
                    var listOfDurations = new List<KeyValuePair<string, int>>();
                    var listOfExposureVaribles = new List<string>();
                    exposureList = new List<KeyValuePair<string, SCM5821AX1DataTypes.ExposureStructure>>();
                    DelayList = new List<int>();
                    GIList = new List<int>();
                    List<SubarrayDataModel> subarrayDataModel = new List<SubarrayDataModel>();
                    foreach (ExposureDataModel mvm in exposureDataModelList)
                    {
                        if (ct.IsCancellationRequested)
                        {
                            return;
                        }
                        exposure = new SCM5821AX1DataTypes.ExposureStructure();
                        exposure.exposureName = mvm.ExposureName;
                        exposure.exposureRegion.Xo = Convert.ToUInt16(mvm.ExposureRegionXo);
                        exposure.exposureRegion.Yo = Convert.ToUInt16(mvm.ExposureRegionYo);
                        exposure.exposureRegion.dX = Convert.ToUInt16(mvm.ExposureRegiondX);
                        exposure.exposureRegion.dY = Convert.ToUInt16(mvm.ExposureRegiondY);
                        exposure.exposureInterval = Convert.ToUInt32(mvm.ExposureInterval);
                        exposure.numberOfNDROs = Convert.ToUInt32(mvm.ExposureNDROS);
                        exposure.exposureFPN = Convert.ToByte(mvm.ExposureFPN);
                        if (exposure.exposureFPN == 2)
                            exposure.autoBiasEnabled = 1;
                        exposure.autoBiasEnabled = Convert.ToByte(mvm.AutoBiasFPNEnabled);
                        exposure.fullFrameEnabled = mvm.FullFrameEnabled;
                        exposure.darkFrameEnabled = mvm.DarkFrameEnabled;
                        exposure.useDefaultConfiguration = mvm.UserDefaultConfiguration;
                        exposure.led1Enabled = mvm.LED1Enabled;
                        exposure.led2Enabled = mvm.LED2Enabled;
                        exposure.led3Enabled = mvm.LED3Enabled;
                        exposure.ledOnTime = Convert.ToUInt32(mvm.LEDOnTime);
                        exposure.ledOffTime = Convert.ToUInt32(mvm.LEDOffTime);
                        exposure.ledFlashes = Convert.ToUInt32(mvm.LEDFlashes);
                        exposure.shutterEnabled = mvm.ShutterEnabled;
                        exposure.numberOfSubarrays = Convert.ToUInt16(mvm.NumberOfSubarrays);
                        if (mvm.SubarrayDatas != null)
                        {
                            subarrayDataModel = mvm.SubarrayDatas.ToList();
                            exposure.subarrayList = new SCM5821AX1DataTypes.SubarrayStructure[mvm.SubarrayDatas.Count()];
                            for (int subarrayIndex = 0; subarrayIndex < mvm.SubarrayDatas.Count(); subarrayIndex++)
                            {
                                exposure.subarrayList[subarrayIndex].subarrayName = mvm.SubarrayDatas[subarrayIndex].SubarrayName;
                                exposure.subarrayList[subarrayIndex].subarrayRegion.Xo = Convert.ToUInt16(mvm.SubarrayDatas[subarrayIndex].SubarrayRegionXo);
                                exposure.subarrayList[subarrayIndex].subarrayRegion.Yo = Convert.ToUInt16(mvm.SubarrayDatas[subarrayIndex].SubarrayRegionYo);
                                exposure.subarrayList[subarrayIndex].subarrayRegion.dX = Convert.ToUInt16(mvm.SubarrayDatas[subarrayIndex].SubarrayRegiondX);
                                exposure.subarrayList[subarrayIndex].subarrayRegion.dY = Convert.ToUInt16(mvm.SubarrayDatas[subarrayIndex].SubarrayRegiondY);
                                exposure.subarrayList[subarrayIndex].subarrayFPN = Convert.ToUInt32(mvm.SubarrayDatas[subarrayIndex].SubarrayFPN);
                                exposure.subarrayList[subarrayIndex].readEnabled = mvm.SubarrayDatas[subarrayIndex].SubarrayReadEnabled;
                                exposure.subarrayList[subarrayIndex].readInterval = Convert.ToUInt32(mvm.SubarrayDatas[subarrayIndex].SubarrayReadInterval);
                                exposure.subarrayList[subarrayIndex].subInjEnabled = mvm.SubarrayDatas[subarrayIndex].SubarraySubinjectEnabled;
                                exposure.subarrayList[subarrayIndex].subInjectInterval = Convert.ToUInt32(mvm.SubarrayDatas[subarrayIndex].SubarraySubinjectInterval);
                                exposure.subarrayList[subarrayIndex].postreadEnabled = mvm.SubarrayDatas[subarrayIndex].SubarrayPostReadEnabled;
                                exposure.subarrayList[subarrayIndex].postreadNDROs = Convert.ToUInt32(mvm.SubarrayDatas[subarrayIndex].SubarrayPostReadNDROS);
                                exposure.subarrayList[subarrayIndex].thresholdEnabled = mvm.SubarrayDatas[subarrayIndex].SubarrayThresholdEnabled;
                                exposure.subarrayList[subarrayIndex].thresholdValuePercent = Convert.ToUInt16(mvm.SubarrayDatas[subarrayIndex].SubarrayThresholdPercent);
                                exposure.subarrayList[subarrayIndex].thresholdRegion.Xo = Convert.ToUInt16(mvm.SubarrayDatas[subarrayIndex].SubarrayThresholdRegionXo);
                                exposure.subarrayList[subarrayIndex].thresholdRegion.Yo = Convert.ToUInt16(mvm.SubarrayDatas[subarrayIndex].SubarrayThresholdRegionYo);
                                exposure.subarrayList[subarrayIndex].thresholdRegion.dX = Convert.ToUInt16(mvm.SubarrayDatas[subarrayIndex].SubarrayThresholdRegiondX);
                                exposure.subarrayList[subarrayIndex].thresholdRegion.dY = Convert.ToUInt16(mvm.SubarrayDatas[subarrayIndex].SubarrayThresholdRegiondY);
                                exposure.subarrayList[subarrayIndex].timeResolvedEnabled = mvm.SubarrayDatas[subarrayIndex].TimeResolvedEnabled;
                                exposure.subarrayList[subarrayIndex].timeResolvedInterval = Convert.ToUInt32(mvm.SubarrayDatas[subarrayIndex].TimeResolvedInterval);
                                exposure.subarrayList[subarrayIndex].adaptiveEnabled = mvm.SubarrayDatas[subarrayIndex].AdaptiveEnabled;
                            }
                        }
                        exposureList.Add(new KeyValuePair<string, SCM5821AX1DataTypes.ExposureStructure>(mvm.ExposureName, exposure));
                        DelayList.Add(mvm.GlobalInjectDelay);
                        GIList.Add(mvm.GlobalInject);
                    }
                    log.Info("End Process Exposure data");
                });
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task SendExposure(CancellationToken ct, int exposureCount, CIDInterface cidInterface)
        {
            try
            {
                await Task.Run(() =>
                {
                    log.Info("Begin Send Exposure");
                    UInt32 exposureId = 1;
                    if (cidInterface.IsConnected())
                    {
                        for (int i = 0; i < exposureCount; i++)
                        {
                            if (ct.IsCancellationRequested)
                                return;
                            ExposureData.ExposureDataContainer exposureParameters =
                                      new ExposureData.ExposureDataContainer();
                            exposureParameters.exposureData =
                                new SCM5821AX1DataTypes.ExposureStructure();
                            Thermo.Kronos.Instrument.Camera.Contracts.Data.CameraExposureSettings cameraExposureSettings
                                = new Thermo.Kronos.Instrument.Camera.Contracts.Data.CameraExposureSettings();
                            exposureParameters.exposureData.exposureId = exposureId;
                            cameraExposureSettings.ExposureId =(int) Convert.ToInt32(exposureId);
                            exposureId++;
                            subarrayList = new List<SCM5821AX1DataTypes.SubarrayStructure>();
                            int subarrayListCount = 0;
                            if (exposureList[i].Value.subarrayList !=null)
                                subarrayListCount = exposureList[i].Value.subarrayList.Count();
                            else
                                subarrayListCount = 0;
                            for (int j = 0; j < subarrayListCount; j++)
                            {
                                SCM5821AX1DataTypes.SubarrayStructure subarray = new SCM5821AX1DataTypes.SubarrayStructure();
                                subarray.subarrayName = exposureList[i].Value.subarrayList[j].subarrayName;
                                subarray.subarrayRegion.Xo = exposureList[i].Value.subarrayList[j].subarrayRegion.Xo;
                                subarray.subarrayRegion.dX = exposureList[i].Value.subarrayList[j].subarrayRegion.dX;
                                subarray.subarrayRegion.Yo = exposureList[i].Value.subarrayList[j].subarrayRegion.Yo;
                                subarray.subarrayRegion.dY = exposureList[i].Value.subarrayList[j].subarrayRegion.dY;
                                subarray.readEnabled = exposureList[i].Value.subarrayList[j].readEnabled;
                                subarray.readInterval = exposureList[i].Value.subarrayList[j].readInterval;
                                subarray.subInjEnabled = exposureList[i].Value.subarrayList[j].subInjEnabled;
                                subarray.subInjectInterval = exposureList[i].Value.subarrayList[j].subInjectInterval;
                                subarray.postreadEnabled = exposureList[i].Value.subarrayList[j].postreadEnabled;
                                subarray.postreadNDROs = exposureList[i].Value.subarrayList[j].postreadNDROs;
                                subarray.subarrayFPN = exposureList[i].Value.subarrayList[j].subarrayFPN;
                                subarray.thresholdEnabled = exposureList[i].Value.subarrayList[j].thresholdEnabled;
                                subarray.timeResolvedEnabled = exposureList[i].Value.subarrayList[j].timeResolvedEnabled;
                                subarray.timeResolvedInterval = exposureList[i].Value.subarrayList[j].timeResolvedInterval;
                                subarray.adaptiveEnabled = exposureList[i].Value.subarrayList[j].adaptiveEnabled;
                                subarray.thresholdRegion.Xo = exposureList[i].Value.subarrayList[j].thresholdRegion.Xo;
                                subarray.thresholdRegion.dX = exposureList[i].Value.subarrayList[j].thresholdRegion.dX;
                                subarray.thresholdRegion.Yo = exposureList[i].Value.subarrayList[j].thresholdRegion.Yo;
                                subarray.thresholdRegion.dY = exposureList[i].Value.subarrayList[j].thresholdRegion.dY;
                                subarray.thresholdValuePercent = exposureList[i].Value.subarrayList[j].thresholdValuePercent;
                                subarrayList.Add(subarray);
                            }
                            exposureParameters.exposureData.exposureInterval = exposureList[i].Value.exposureInterval;
                            exposureInterval = exposureParameters.exposureData.exposureInterval;
                            cameraExposureSettings.ExposureInterval = (int)exposureInterval;
                            exposureParameters.exposureData.exposureRegion.Xo = exposureList[i].Value.exposureRegion.Xo;
                            exposureParameters.exposureData.exposureRegion.dX = exposureList[i].Value.exposureRegion.dX;
                            exposureParameters.exposureData.exposureRegion.Yo = exposureList[i].Value.exposureRegion.Yo;
                            exposureParameters.exposureData.exposureRegion.dY = exposureList[i].Value.exposureRegion.dY;
                            cameraExposureSettings.Region = new Thermo.Kronos.Instrument.Camera.Contracts.Data.CameraRegion();
                            cameraExposureSettings.Region.PositionX = (short)exposureParameters.exposureData.exposureRegion.Xo;
                            cameraExposureSettings.Region.PositionY = (short)exposureParameters.exposureData.exposureRegion.Yo;
                            cameraExposureSettings.Region.Width = (short)exposureParameters.exposureData.exposureRegion.dX;
                            cameraExposureSettings.Region.Height = (short)exposureParameters.exposureData.exposureRegion.dY;
                            exposureParameters.exposureData.numberOfNDROs = exposureList[i].Value.numberOfNDROs;
                            cameraExposureSettings.NumberOfNonDestructiveReadouts = (int)exposureParameters.exposureData.numberOfNDROs;
                            exposureParameters.exposureData.useDefaultConfiguration = exposureList[i].Value.useDefaultConfiguration;
                            cameraExposureSettings.UseDefaultConfiguration = exposureParameters.exposureData.useDefaultConfiguration;
                            exposureParameters.exposureData.ledOnTime = exposureList[i].Value.ledOnTime;
                            exposureParameters.exposureData.ledOffTime = exposureList[i].Value.ledOffTime;
                            exposureParameters.exposureData.ledFlashes = exposureList[i].Value.ledFlashes;
                            exposureParameters.exposureData.led1Enabled = exposureList[i].Value.led1Enabled;
                            exposureParameters.exposureData.led2Enabled = exposureList[i].Value.led2Enabled;
                            exposureParameters.exposureData.led3Enabled = exposureList[i].Value.led3Enabled;
                            exposureParameters.exposureData.shutterEnabled = exposureList[i].Value.shutterEnabled;
                            cameraExposureSettings.LedOnTime = (int)exposureParameters.exposureData.ledOnTime;
                            cameraExposureSettings.LedOffTime = (int)exposureParameters.exposureData.ledOffTime;
                            cameraExposureSettings.LedFlashes = (int)exposureParameters.exposureData.ledFlashes;
                            cameraExposureSettings.Led1Enabled = exposureParameters.exposureData.led1Enabled;
                            cameraExposureSettings.Led2Enabled = exposureParameters.exposureData.led2Enabled;
                            cameraExposureSettings.Led3Enabled = exposureParameters.exposureData.led3Enabled;
                            cameraExposureSettings.ShutterEnabled = exposureParameters.exposureData.shutterEnabled;
                            exposureParameters.exposureData.autoBiasEnabled = exposureList[i].Value.autoBiasEnabled;
                            cameraExposureSettings.AutoBiasEnabled =
                                      Convert.ToBoolean(exposureParameters.exposureData.autoBiasEnabled);
                            exposureParameters.exposureData.exposureFPN = exposureList[i].Value.exposureFPN;
                            cameraExposureSettings.FixedPatternNoiseReduction = exposureParameters.exposureData.exposureFPN;
                            exposureParameters.exposureData.fullFrameEnabled = exposureList[i].Value.fullFrameEnabled;
                            exposureParameters.exposureData.darkFrameEnabled = exposureList[i].Value.darkFrameEnabled;
                            exposureParameters.exposureData.numberOfSubarrays = exposureList[i].Value.subarrayList != null ? (ushort)exposureList[i].Value.subarrayList.Count() : (ushort)0;
                            cameraExposureSettings.FullframeEnabled = exposureParameters.exposureData.fullFrameEnabled;
                            cameraExposureSettings.DarkframeEnabled = exposureParameters.exposureData.darkFrameEnabled;
                            cameraExposureSettings.Subarrays =
                                      new List<Thermo.Kronos.Instrument.Camera.Contracts.Data.CameraSubarraySettings>
                                      ((int)exposureParameters.exposureData.numberOfSubarrays);
                            exposureParameters.exposureData.subarrayList =
                                      new SCM5821AX1DataTypes.SubarrayStructure[exposureParameters.exposureData.numberOfSubarrays];
                            exposureParameters.exposureData.subarrayList = subarrayList.ToArray();
                            for (int subarrayIndex = 0; subarrayIndex < subarrayList.Count; subarrayIndex++)
                            {
                                Thermo.Kronos.Instrument.Camera.Contracts.Data.CameraSubarraySettings cameraSubarray =
                                      new Thermo.Kronos.Instrument.Camera.Contracts.Data.CameraSubarraySettings();
                                cameraSubarray.Id = (int)subarrayList[subarrayIndex].subarrayId;
                                cameraSubarray.Name = subarrayList[subarrayIndex].subarrayName;
                                cameraSubarray.ReadEnabled = subarrayList[subarrayIndex].readEnabled;
                                cameraSubarray.PostReadEnabled = subarrayList[subarrayIndex].postreadEnabled;
                                cameraSubarray.SubInjectEnabled = subarrayList[subarrayIndex].subInjEnabled;
                                cameraSubarray.TresholdEnabled = subarrayList[subarrayIndex].thresholdEnabled;
                                cameraSubarray.AdaptiveEnabled = subarrayList[subarrayIndex].adaptiveEnabled;
                                cameraSubarray.ReadInterval = (int)subarrayList[subarrayIndex].readInterval;
                                cameraSubarray.SubInjectInterval = (int)subarrayList[subarrayIndex].subInjectInterval;
                                cameraSubarray.PostReadNonDestructiveReadouts =
                                    (int)subarrayList[subarrayIndex].postreadNDROs;
                                cameraSubarray.SubarrayFixedPatternNoiseReduction = (int)subarrayList[subarrayIndex].subarrayFPN;
                                cameraSubarray.Region =
                                    new Thermo.Kronos.Instrument.Camera.Contracts.Data.CameraRegion();
                                cameraSubarray.Region.PositionX =
                                    (short)subarrayList[subarrayIndex].subarrayRegion.Xo;
                                cameraSubarray.Region.PositionY =
                                    (short)subarrayList[subarrayIndex].subarrayRegion.Yo;
                                cameraSubarray.Region.Width =
                                    (short)subarrayList[subarrayIndex].subarrayRegion.dX;
                                cameraSubarray.Region.Height =
                                    (short)subarrayList[subarrayIndex].subarrayRegion.dY;
                                cameraSubarray.TresholdValuePercentage =
                                    (short)subarrayList[subarrayIndex].thresholdValuePercent;
                                cameraSubarray.TresholdRegion =
                                    new Thermo.Kronos.Instrument.Camera.Contracts.Data.CameraRegion();
                                cameraSubarray.TresholdRegion.PositionX =
                                    (short)subarrayList[subarrayIndex].thresholdRegion.Xo;
                                cameraSubarray.TresholdRegion.PositionY =
                                    (short)subarrayList[subarrayIndex].thresholdRegion.Yo;
                                cameraSubarray.TresholdRegion.Width =
                                    (short)subarrayList[subarrayIndex].thresholdRegion.dX;
                                cameraSubarray.TresholdRegion.Height =
                                    (short)subarrayList[subarrayIndex].thresholdRegion.dY;
                                cameraSubarray.TimeResolvedEnabled =
                                    subarrayList[subarrayIndex].timeResolvedEnabled;
                                cameraExposureSettings.Subarrays.Add(cameraSubarray);                                                               
                            }
                            cameraExposureSettings.GlobalInjectTime = GIList[i];
                            cameraExposureSettings.DelayAfterGlobalInject = DelayList[i];                          
                            if (cidInterface.IsConnected())
                            {
                                cidInterface.PerformExposure(cameraExposureSettings);
                            }
                        }
                    }
                    else
                    {
                        bool cameraConnected = cidInterface.Connect(ConfigurationManager.AppSettings["CameraIPAddresss"], Int32.Parse(ConfigurationManager.AppSettings["CameraIPPort"]));
                        if (!cameraConnected)
                        {
                        log.Error("Camera is not connected.");
                        MessageBox.Show("Camera is not connected.", "Invalid Operation");
                       }
                    }
                });
                log.Info("End Send Exposure");
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public void ProcessExposureXMLData(string xmlFile)
        {
            try
            {
                log.Info("Begin Reading XML.");
                DataSet ds = new DataSet();
                ds.ReadXml(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + xmlFile, XmlReadMode.InferSchema);
                if (ds.Tables.Count > 0)
                {
                    DataView dvExposure;
                    dvExposure = ds.Tables[0].DefaultView;
                    DataView dvSubarray;
                    dvSubarray = ds.Tables[2].DefaultView;
                    exposureDataList = new List<ExposureDataModel>();
                    subarrayDataModelList = new List<SubarrayDataModel>();
                    foreach (DataRowView dr in dvExposure)
                    {
                        ExposureDataModel m = new ExposureDataModel();
                        m.ExposureID = Convert.ToInt32(dr[0]);
                        m.TestDataID = Convert.ToString(dr[1]);
                        m.ExposureName = Convert.ToString(dr[2]);
                        m.DateDefined = Convert.ToDateTime(dr[3]);
                        m.UserModified = Convert.ToString(dr[4]);
                        m.ExposureRegionXo = Convert.ToInt32(dr[5]);
                        m.ExposureRegionYo = Convert.ToInt32(dr[6]);
                        m.ExposureRegiondX = Convert.ToInt32(dr[7]);
                        m.ExposureRegiondY = Convert.ToInt32(dr[8]);
                        m.ExposureInterval = Convert.ToInt32(dr[9]);
                        m.ExposureNDROS = Convert.ToInt32(dr[10]);
                        m.ExposureFPN = Convert.ToInt32(dr[11]);
                        m.FullFrameEnabled = dr[12].ToString() == "0" ? false : true;
                        m.DarkFrameEnabled = dr[13].ToString() == "0" ? false : true;
                        m.UserDefaultConfiguration = dr[14].ToString() == "0" ? false : true;
                        m.LED1Enabled = dr[15].ToString() == "0" ? false : true;
                        m.LED2Enabled = dr[16].ToString() == "0" ? false : true;
                        m.LED3Enabled = dr[17].ToString() == "0" ? false : true;
                        m.LEDOnTime = Convert.ToInt32(dr[18]);
                        m.LEDOffTime = Convert.ToInt32(dr[19]);
                        m.LEDFlashes = Convert.ToInt32(dr[20]);
                        m.ShutterEnabled = dr[21].ToString() == "0" ? false : true;
                        m.AutoBiasFPNEnabled = Convert.ToByte(dr[22]);
                        m.NumberOfSubarrays = Convert.ToInt32(dr[23]);
                        m.GlobalInject = Convert.ToInt32(dr[24]);
                        m.GlobalInjectDelay = Convert.ToInt32(dr[25]);
                        exposureDataList.Add(m);
                    }
                    foreach (DataRowView sdr in dvSubarray)
                    {
                        SubarrayDataModel ms = new SubarrayDataModel();
                        ms.SubarrayID = Convert.ToInt32(sdr[0]);
                        ms.ExposureID = Convert.ToInt32(sdr[1]);
                        ms.SubarrayName = Convert.ToString(sdr[2]);
                        ms.DateDefined = Convert.ToDateTime(sdr[3]);
                        ms.UserModified = Convert.ToString(sdr[4]);
                        ms.SubarrayRegionXo = Convert.ToInt32(sdr[5]);
                        ms.SubarrayRegionYo = Convert.ToInt32(sdr[6]);
                        ms.SubarrayRegiondX = Convert.ToInt32(sdr[7]);
                        ms.SubarrayRegiondY = Convert.ToInt32(sdr[8]);
                        ms.SubarrayReadEnabled = sdr[9].ToString() == "0" ? false : true;
                        ms.SubarrayReadInterval = Convert.ToInt32(sdr[10]);
                        ms.SubarraySubinjectEnabled = sdr[11].ToString() == "0" ? false : true;
                        ms.SubarraySubinjectInterval = Convert.ToInt32(sdr[12]);
                        ms.SubarrayPostReadEnabled = sdr[13].ToString() == "0" ? false : true;
                        ms.SubarrayPostReadNDROS = Convert.ToInt32(sdr[14]);
                        ms.SubarrayFPN = Convert.ToInt32(sdr[15]);
                        ms.SubarrayThresholdEnabled = sdr[16].ToString() == "0" ? false : true;
                        ms.SubarrayThresholdPercent = Convert.ToInt32(sdr[17]);
                        ms.SubarrayThresholdRegionXo = Convert.ToInt32(sdr[18]);
                        ms.SubarrayThresholdRegionYo = Convert.ToInt32(sdr[19]);
                        ms.SubarrayThresholdRegiondX = Convert.ToInt32(sdr[20]);
                        ms.SubarrayThresholdRegiondY = Convert.ToInt32(sdr[21]);
                        ms.TimeResolvedEnabled = sdr[22].ToString() == "0" ? false : true;
                        ms.TimeResolvedInterval = Convert.ToInt32(sdr[23]);
                        ms.AdaptiveEnabled = sdr[24].ToString() == "0" ? false : true;
                        subarrayDataModelList.Add(ms);
                    }
                    foreach (ExposureDataModel exp in exposureDataList)
                    {
                        exp.SubarrayDatas = new List<SubarrayDataModel>();
                        foreach (SubarrayDataModel sm in subarrayDataModelList)
                        {
                            if (sm.ExposureID == exp.ExposureID)
                            {
                                exp.SubarrayDatas.Add(sm);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No Exposure Data to display.", "Exposure Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                log.Info("End Reading XML.");
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public bool AddExposureXMLData(string xmlFile,string rootElementName, ExposureDataModel exposureDataModel, SubarrayDataModel subarrayDataModel)
        {
            try
            {
                log.Info("Begin Add Exposure to"+ xmlFile +"XML.");
                XDocument xmlDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + xmlFile);
                bool DBExists = false;
                if (TestAppHelper.CheckDBExists())
                    DBExists = true;
                if (exposureDataModel.SubarrayDatas == null)
                {
                    xmlDoc.Element(rootElementName).Add
               (
                 new XElement("Exposure",
                 new XElement("ExposureID", DBExists ? exposureDataModel.ExposureID : (exposureDataList.Count() + 1)),
                 new XElement("TestDataID", exposureDataModel.TestDataID),
                 new XElement("ExposureName", exposureDataModel.ExposureName),
                 new XElement("DateDefined", DateTime.Today),
                 new XElement("UserModified", exposureDataModel.UserModified),
                 new XElement("ExposureRegionXo", exposureDataModel.ExposureRegionXo),
                 new XElement("ExposureRegionYo", exposureDataModel.ExposureRegionYo),
                 new XElement("ExposureRegiondX", exposureDataModel.ExposureRegiondX),
                 new XElement("ExposureRegiondY", exposureDataModel.ExposureRegiondY),
                 new XElement("ExposureInterval", exposureDataModel.ExposureInterval),
                 new XElement("ExposureNDROS", exposureDataModel.ExposureNDROS),
                 new XElement("ExposureFPN", exposureDataModel.ExposureFPN),
                 new XElement("FullFrameEnabled", exposureDataModel.FullFrameEnabled),
                 new XElement("DarkFrameEnabled", exposureDataModel.DarkFrameEnabled),
                 new XElement("UserDefaultConfiguration", exposureDataModel.UserDefaultConfiguration),
                 new XElement("LED1Enabled", exposureDataModel.LED1Enabled),
                 new XElement("LED2Enabled", exposureDataModel.LED2Enabled),
                 new XElement("LED3Enabled", exposureDataModel.LED3Enabled),
                 new XElement("LEDOnTime", exposureDataModel.LEDOnTime),
                 new XElement("LEDOffTime", exposureDataModel.LEDOffTime),
                 new XElement("LEDFlashes", exposureDataModel.LEDFlashes),
                 new XElement("ShutterEnabled", exposureDataModel.ShutterEnabled),
                 new XElement("AutoBiasFPNEnabled", exposureDataModel.AutoBiasFPNEnabled),
                 new XElement("NumberOfSubarrays", exposureDataModel.NumberOfSubarrays),
                 new XElement("GlobalInject", exposureDataModel.GlobalInject),
                 new XElement("GlobalInjectDelay", exposureDataModel.GlobalInjectDelay)
                        ));
                }
                else
                {
                    xmlDoc.Element(rootElementName).Add
                    (
                      new XElement("Exposure",
                      new XElement("ExposureID", DBExists ? exposureDataModel.ExposureID : (exposureDataList.Count() + 1)),
                      new XElement("TestDataID", exposureDataModel.TestDataID),
                      new XElement("ExposureName", exposureDataModel.ExposureName),
                      new XElement("DateDefined", DateTime.Today),
                      new XElement("UserModified", exposureDataModel.UserModified),
                      new XElement("ExposureRegionXo", exposureDataModel.ExposureRegionXo),
                      new XElement("ExposureRegionYo", exposureDataModel.ExposureRegionYo),
                      new XElement("ExposureRegiondX", exposureDataModel.ExposureRegiondX),
                      new XElement("ExposureRegiondY", exposureDataModel.ExposureRegiondY),
                      new XElement("ExposureInterval", exposureDataModel.ExposureInterval),
                      new XElement("ExposureNDROS", exposureDataModel.ExposureNDROS),
                      new XElement("ExposureFPN", exposureDataModel.ExposureFPN),
                      new XElement("FullFrameEnabled", exposureDataModel.FullFrameEnabled),
                      new XElement("DarkFrameEnabled", exposureDataModel.DarkFrameEnabled),
                      new XElement("UserDefaultConfiguration", exposureDataModel.UserDefaultConfiguration),
                      new XElement("LED1Enabled", exposureDataModel.LED1Enabled),
                      new XElement("LED2Enabled", exposureDataModel.LED2Enabled),
                      new XElement("LED3Enabled", exposureDataModel.LED3Enabled),
                      new XElement("LEDOnTime", exposureDataModel.LEDOnTime),
                      new XElement("LEDOffTime", exposureDataModel.LEDOffTime),
                      new XElement("LEDFlashes", exposureDataModel.LEDFlashes),
                      new XElement("ShutterEnabled", exposureDataModel.ShutterEnabled),
                      new XElement("AutoBiasFPNEnabled", exposureDataModel.AutoBiasFPNEnabled),
                      new XElement("NumberOfSubarrays", exposureDataModel.NumberOfSubarrays),
                      new XElement("GlobalInject", exposureDataModel.GlobalInject),
                      new XElement("GlobalInjectDelay", exposureDataModel.GlobalInjectDelay),
                      new XElement("SubarrayList", new XAttribute("ID", DBExists ? exposureDataModel.ExposureID : (exposureDataList.Count() + 1)),
                      new XElement("Subarray",
                      new XElement("SubarrayID", DBExists ? subarrayDataModel.SubarrayID : (subarrayDataModelList.Count() + 1)),
                      new XElement("ExposureID", DBExists ? exposureDataModel.ExposureID : (exposureDataList.Count() + 1)),
                      new XElement("SubarrayName", subarrayDataModel.SubarrayName),
                      new XElement("DateDefined", DateTime.Today),
                      new XElement("UserModified", exposureDataModel.UserModified),
                      new XElement("SubarrayRegionXo", subarrayDataModel.SubarrayRegionXo),
                      new XElement("SubarrayRegionYo", subarrayDataModel.SubarrayRegionYo),
                      new XElement("SubarrayRegiondX", subarrayDataModel.SubarrayRegiondX),
                      new XElement("SubarrayRegiondY", subarrayDataModel.SubarrayRegiondY),
                      new XElement("SubarrayReadEnabled", subarrayDataModel.SubarrayReadEnabled),
                      new XElement("SubarrayReadInterval", subarrayDataModel.SubarrayReadInterval),
                      new XElement("SubarraySubinjectEnabled", subarrayDataModel.SubarraySubinjectEnabled),
                      new XElement("SubarraySubinjectInterval", subarrayDataModel.SubarraySubinjectInterval),
                      new XElement("SubarrayPostReadEnabled", subarrayDataModel.SubarrayPostReadEnabled),
                      new XElement("SubarrayPostReadNDROS", subarrayDataModel.SubarrayPostReadNDROS),
                      new XElement("SubarrayFPN", subarrayDataModel.SubarrayFPN),
                      new XElement("SubarrayThresholdEnabled", subarrayDataModel.SubarrayThresholdEnabled),
                      new XElement("SubarrayThresholdPercent", subarrayDataModel.SubarrayThresholdPercent),
                      new XElement("SubarrayThresholdRegionXo", subarrayDataModel.SubarrayThresholdRegionXo),
                      new XElement("SubarrayThresholdRegionYo", subarrayDataModel.SubarrayThresholdRegionYo),
                      new XElement("SubarrayThresholdRegiondX", subarrayDataModel.SubarrayThresholdRegiondX),
                      new XElement("SubarrayThresholdRegiondY", subarrayDataModel.SubarrayThresholdRegiondY),
                      new XElement("TimeResolvedEnabled", subarrayDataModel.TimeResolvedEnabled),
                      new XElement("TimeResolvedInterval", subarrayDataModel.TimeResolvedInterval),
                      new XElement("AdaptiveEnabled", subarrayDataModel.AdaptiveEnabled)
                       ))));
                }
                xmlDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + xmlFile);
                log.Info("End Add Exposure to" + xmlFile + " XML.");
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Add Exposure to" + xmlFile + "XML failed");
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
        public bool RemoveExposureFromXMLData(string xmlFile,ExposureDataModel exposureDataModel, SubarrayDataModel subarrayDataModel)
        {
            try
            {
                log.Info("Begin Remove Exposure from"+ xmlFile + "XML.");
                XDocument xmlDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + xmlFile);               
                XElement removeExposure = xmlDoc.Descendants("Exposure").FirstOrDefault(p => p.Element("ExposureID").Value == exposureDataModel.ExposureID.ToString());
                if (removeExposure != null)
                {
                    removeExposure.Remove();
                    xmlDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + xmlFile);
                    log.Info("End Remove Exposure from"+ xmlFile +"XML.");
                    return true;
                }
                else
                {                   
                    log.Error("Remove Exposure from"+ xmlFile +"XML failed due to no exposure found with ID." + exposureDataModel.ExposureID.ToString());
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Remove Exposure from" + xmlFile + "XML failed");
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
        public bool UpdateExposureXMLData(string xmlFile, ExposureDataModel exposureDataModel, SubarrayDataModel subarrayDataModel)
        {
            try
            {
                log.Info("Begin Update Exposure in" + xmlFile);
                XDocument xmlDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + xmlFile); 
                XElement updateExposure = xmlDoc.Descendants("Exposure").FirstOrDefault(p => p.Element("ExposureID").Value == exposureDataModel.ExposureID.ToString());
                if (updateExposure != null)
                {
                    updateExposure.Element("TestDataID").Value = exposureDataModel.TestDataID;
                    updateExposure.Element("ExposureName").Value = exposureDataModel.ExposureName;
                    updateExposure.Element("DateDefined").Value = DateTime.UtcNow.ToString();
                    updateExposure.Element("UserModified").Value = exposureDataModel.UserModified;
                    updateExposure.Element("ExposureRegionXo").Value = exposureDataModel.ExposureRegionXo.ToString();
                    updateExposure.Element("ExposureRegionYo").Value = exposureDataModel.ExposureRegionYo.ToString();
                    updateExposure.Element("ExposureRegiondX").Value = exposureDataModel.ExposureRegiondX.ToString();
                    updateExposure.Element("ExposureRegiondY").Value = exposureDataModel.ExposureRegiondY.ToString();
                    updateExposure.Element("ExposureInterval").Value = exposureDataModel.ExposureInterval.ToString();
                    updateExposure.Element("ExposureNDROS").Value = exposureDataModel.ExposureNDROS.ToString();
                    updateExposure.Element("ExposureFPN").Value = exposureDataModel.ExposureFPN.ToString();
                    updateExposure.Element("FullFrameEnabled").Value = exposureDataModel.FullFrameEnabled.ToString();
                    updateExposure.Element("DarkFrameEnabled").Value = exposureDataModel.DarkFrameEnabled.ToString();
                    updateExposure.Element("UserDefaultConfiguration").Value = exposureDataModel.UserDefaultConfiguration.ToString();
                    updateExposure.Element("LED1Enabled").Value = exposureDataModel.LED1Enabled.ToString();
                    updateExposure.Element("LED2Enabled").Value = exposureDataModel.LED2Enabled.ToString();
                    updateExposure.Element("LED3Enabled").Value = exposureDataModel.LED3Enabled.ToString();
                    updateExposure.Element("LEDOnTime").Value = exposureDataModel.LEDOnTime.ToString();
                    updateExposure.Element("LEDOffTime").Value = exposureDataModel.LEDOffTime.ToString();
                    updateExposure.Element("LEDFlashes").Value = exposureDataModel.LEDFlashes.ToString();
                    updateExposure.Element("ShutterEnabled").Value = exposureDataModel.ShutterEnabled.ToString();
                    updateExposure.Element("AutoBiasFPNEnabled").Value = exposureDataModel.AutoBiasFPNEnabled.ToString();
                    updateExposure.Element("NumberOfSubarrays").Value = exposureDataModel.NumberOfSubarrays.ToString();
                    updateExposure.Element("GlobalInject").Value = exposureDataModel.GlobalInject.ToString();
                    updateExposure.Element("GlobalInjectDelay").Value = exposureDataModel.GlobalInjectDelay.ToString();                  
                }
                else
                {                  
                    log.Error("Update"+ xmlFile +"Exposure failed due to no Exposure found with ID." + exposureDataModel.ExposureID.ToString());
                    return false;
                }
                log.Info("Begin Update"+ xmlFile + "Subarray to Exposure in Mean Variance XML.");
                XElement updateSubarray = xmlDoc.Descendants("Subarray").FirstOrDefault(p => p.Element("SubarrayID").Value == subarrayDataModel.SubarrayID.ToString());
                if (updateSubarray != null)
                {
                    updateSubarray.Element("ExposureID").Value = exposureDataModel.ExposureID.ToString();
                    updateSubarray.Element("SubarrayName").Value = subarrayDataModel.SubarrayName.ToString();
                    updateSubarray.Element("DateDefined").Value = subarrayDataModel.DateDefined.ToString();
                    updateSubarray.Element("UserModified").Value = subarrayDataModel.UserModified;
                    updateSubarray.Element("SubarrayRegionXo").Value = subarrayDataModel.SubarrayRegionXo.ToString();
                    updateSubarray.Element("SubarrayRegionYo").Value = subarrayDataModel.SubarrayRegionYo.ToString();
                    updateSubarray.Element("SubarrayRegiondX").Value = subarrayDataModel.SubarrayRegiondX.ToString();
                    updateSubarray.Element("SubarrayRegiondY").Value = subarrayDataModel.SubarrayRegiondY.ToString();
                    updateSubarray.Element("SubarrayReadEnabled").Value = subarrayDataModel.SubarrayReadEnabled.ToString();
                    updateSubarray.Element("SubarrayReadInterval").Value = subarrayDataModel.SubarrayReadInterval.ToString();
                    updateSubarray.Element("SubarraySubinjectEnabled").Value = subarrayDataModel.SubarraySubinjectEnabled.ToString();
                    updateSubarray.Element("SubarraySubinjectInterval").Value = subarrayDataModel.SubarraySubinjectInterval.ToString();
                    updateSubarray.Element("SubarrayPostReadEnabled").Value = subarrayDataModel.SubarrayPostReadEnabled.ToString();
                    updateSubarray.Element("SubarrayPostReadNDROS").Value = subarrayDataModel.SubarrayPostReadNDROS.ToString();
                    updateSubarray.Element("SubarrayFPN").Value = subarrayDataModel.SubarrayFPN.ToString();
                    updateSubarray.Element("SubarrayThresholdEnabled").Value = subarrayDataModel.SubarrayThresholdEnabled.ToString();
                    updateSubarray.Element("SubarrayThresholdPercent").Value = subarrayDataModel.SubarrayThresholdPercent.ToString();
                    updateSubarray.Element("SubarrayThresholdRegionXo").Value = subarrayDataModel.SubarrayThresholdRegionXo.ToString();
                    updateSubarray.Element("SubarrayThresholdRegionYo").Value = subarrayDataModel.SubarrayThresholdRegionYo.ToString();
                    updateSubarray.Element("SubarrayThresholdRegiondX").Value = subarrayDataModel.SubarrayThresholdRegiondX.ToString();
                    updateSubarray.Element("SubarrayThresholdRegiondY").Value = subarrayDataModel.SubarrayThresholdRegiondY.ToString();
                    updateSubarray.Element("TimeResolvedEnabled").Value = subarrayDataModel.TimeResolvedEnabled.ToString();
                    updateSubarray.Element("TimeResolvedInterval").Value = subarrayDataModel.TimeResolvedInterval.ToString();
                    updateSubarray.Element("AdaptiveEnabled").Value = subarrayDataModel.AdaptiveEnabled.ToString();
                }
                else
                {
                    log.Error("Update" + xmlFile + "Subarray to Exposure failed due to no Subarray found with ID." + subarrayDataModel.SubarrayID.ToString());
                    return false;
                }
                xmlDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + xmlFile);              
                log.Info("End Update Subarray to Exposure in"+ xmlFile +"XML.");
                log.Info("End Update Exposure in"+ xmlFile + "XML.");
                return true;              
            }
            catch (Exception ex)
            {                
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                log.Error("Update Exposure in" + xmlFile + "XML failed");
                return false;
            }
        }
        public bool AddSubarrayXML(string xmlFile, ExposureDataModel exposureDataModel, SubarrayDataModel subarrayDataModel)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + xmlFile);
                XElement Exposure = xmlDoc.Descendants("Exposure").FirstOrDefault(p => p.Element("ExposureID").Value == exposureDataModel.ExposureID.ToString());
                if (Exposure != null)
                {
                    log.Info("Begin Add Subarray to Exposure in" + xmlFile + "XML.");
                    XElement SubarrayAdd = xmlDoc.Descendants("SubarrayList").FirstOrDefault(p => p.Attribute("ID").Value == exposureDataModel.ExposureID.ToString());
                    if (SubarrayAdd != null)
                    {
                        SubarrayAdd.Add(
                            new XElement("Subarray",
                                       new XElement("SubarrayID", TestAppHelper.CheckDBExists() ? subarrayDataModel.SubarrayID : (subarrayDataModelList.Count() + 1)),
                                      new XElement("ExposureID", exposureDataModel.ExposureID),
                                      new XElement("SubarrayName", subarrayDataModel.SubarrayName),
                                      new XElement("DateDefined", DateTime.Today),
                                      new XElement("UserModified", exposureDataModel.UserModified),
                                      new XElement("SubarrayRegionXo", subarrayDataModel.SubarrayRegionXo),
                                      new XElement("SubarrayRegionYo", subarrayDataModel.SubarrayRegionYo),
                                      new XElement("SubarrayRegiondX", subarrayDataModel.SubarrayRegiondX),
                                      new XElement("SubarrayRegiondY", subarrayDataModel.SubarrayRegiondY),
                                      new XElement("SubarrayReadEnabled", subarrayDataModel.SubarrayReadEnabled),
                                      new XElement("SubarrayReadInterval", subarrayDataModel.SubarrayReadInterval),
                                      new XElement("SubarraySubinjectEnabled", subarrayDataModel.SubarraySubinjectEnabled),
                                      new XElement("SubarraySubinjectInterval", subarrayDataModel.SubarraySubinjectInterval),
                                      new XElement("SubarrayPostReadEnabled", subarrayDataModel.SubarrayPostReadEnabled),
                                      new XElement("SubarrayPostReadNDROS", subarrayDataModel.SubarrayPostReadNDROS),
                                      new XElement("SubarrayFPN", subarrayDataModel.SubarrayFPN),
                                      new XElement("SubarrayThresholdEnabled", subarrayDataModel.SubarrayThresholdEnabled),
                                      new XElement("SubarrayThresholdPercent", subarrayDataModel.SubarrayThresholdPercent),
                                      new XElement("SubarrayThresholdRegionXo", subarrayDataModel.SubarrayThresholdRegionXo),
                                      new XElement("SubarrayThresholdRegionYo", subarrayDataModel.SubarrayThresholdRegionYo),
                                      new XElement("SubarrayThresholdRegiondX", subarrayDataModel.SubarrayThresholdRegiondX),
                                      new XElement("SubarrayThresholdRegiondY", subarrayDataModel.SubarrayThresholdRegiondY),
                                      new XElement("TimeResolvedEnabled", subarrayDataModel.TimeResolvedEnabled),
                                      new XElement("TimeResolvedInterval", subarrayDataModel.TimeResolvedInterval),
                                      new XElement("AdaptiveEnabled", subarrayDataModel.AdaptiveEnabled)
                                                         ));
                        XElement updateExposure = xmlDoc.Descendants("Exposure").FirstOrDefault(p => p.Element("ExposureID").Value == exposureDataModel.ExposureID.ToString());
                        if (updateExposure != null)
                        {
                            updateExposure.Element("NumberOfSubarrays").Value = exposureDataModel.NumberOfSubarrays.ToString();
                        }
                        xmlDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + xmlFile);
                        log.Info("End Add Subarray to Exposure in" + xmlFile +"XML.");
                        return true;
                    }
                    else
                    {
                        log.Error("Add Subarray to" + xmlFile +"XML failed due to no SubarrayList found with ID." + subarrayDataModel.SubarrayID.ToString());
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                log.Error("Add Subarray to" + xmlFile + "XML failed");
                return false;
            }
        }
        public bool RemoveSubarrayXML(string xmlFile, ExposureDataModel exposureDataModel, SubarrayDataModel subarrayDataModel)
        {
            try
            {
                log.Info("Begin Remove" + xmlFile + "Subarray.");
                XDocument xmlDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + xmlFile);
                XElement SubarrayAdd = xmlDoc.Descendants("SubarrayList").FirstOrDefault(p => p.Attribute("ID").Value == exposureDataModel.ExposureID.ToString());
                XElement removeSubarray = SubarrayAdd.Descendants("Subarray").FirstOrDefault(p => p.Element("SubarrayID").Value == subarrayDataModel.SubarrayID.ToString());
                if (removeSubarray != null)
                {
                    removeSubarray.Remove();
                    XElement updateExposure = xmlDoc.Descendants("Exposure").FirstOrDefault(p => p.Element("ExposureID").Value == exposureDataModel.ExposureID.ToString());
                    if (updateExposure != null)
                    {
                        updateExposure.Element("NumberOfSubarrays").Value = exposureDataModel.NumberOfSubarrays.ToString();
                    }
                    xmlDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + xmlFile);
                    log.Info("End Remove" + xmlFile +"Subarray.");
                    return true;
                }
                else
                {
                    log.Error("No Subarray found to remove.");
                    return false;
                }                
            }
            catch (Exception ex)
            {
                log.Info("Remove Subarray failed from" + xmlFile);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
        public bool UpdateSubarrayXML(string xmlFile, ExposureDataModel exposureDataModel, SubarrayDataModel subarrayDataModel)
        {
            try
            {
                log.Info("Begin Update of" + xmlFile + "Subarray.");
                XDocument xmlDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + xmlFile);
                XElement SubarrayAdd = xmlDoc.Descendants("SubarrayList").FirstOrDefault(p => p.Attribute("ID").Value == exposureDataModel.ExposureID.ToString());                             
                XElement updateSubarray = SubarrayAdd.Descendants("Subarray").FirstOrDefault(p => p.Element("SubarrayID").Value == subarrayDataModel.SubarrayID.ToString());
                if (updateSubarray != null)
                {
                    updateSubarray.Element("ExposureID").Value = exposureDataModel.ExposureID.ToString();
                    updateSubarray.Element("SubarrayName").Value = subarrayDataModel.SubarrayName.ToString();
                    updateSubarray.Element("DateDefined").Value = subarrayDataModel.DateDefined.ToString();
                    updateSubarray.Element("UserModified").Value = subarrayDataModel.UserModified;
                    updateSubarray.Element("SubarrayRegionXo").Value = subarrayDataModel.SubarrayRegionXo.ToString();
                    updateSubarray.Element("SubarrayRegionYo").Value = subarrayDataModel.SubarrayRegionYo.ToString();
                    updateSubarray.Element("SubarrayRegiondX").Value = subarrayDataModel.SubarrayRegiondX.ToString();
                    updateSubarray.Element("SubarrayRegiondY").Value = subarrayDataModel.SubarrayRegiondY.ToString();
                    updateSubarray.Element("SubarrayReadEnabled").Value = subarrayDataModel.SubarrayReadEnabled.ToString();
                    updateSubarray.Element("SubarrayReadInterval").Value = subarrayDataModel.SubarrayReadInterval.ToString();
                    updateSubarray.Element("SubarraySubinjectEnabled").Value = subarrayDataModel.SubarraySubinjectEnabled.ToString();
                    updateSubarray.Element("SubarraySubinjectInterval").Value = subarrayDataModel.SubarraySubinjectInterval.ToString();
                    updateSubarray.Element("SubarrayPostReadEnabled").Value = subarrayDataModel.SubarrayPostReadEnabled.ToString();
                    updateSubarray.Element("SubarrayPostReadNDROS").Value = subarrayDataModel.SubarrayPostReadNDROS.ToString();
                    updateSubarray.Element("SubarrayFPN").Value = subarrayDataModel.SubarrayFPN.ToString();
                    updateSubarray.Element("SubarrayThresholdEnabled").Value = subarrayDataModel.SubarrayThresholdEnabled.ToString();
                    updateSubarray.Element("SubarrayThresholdPercent").Value = subarrayDataModel.SubarrayThresholdPercent.ToString();
                    updateSubarray.Element("SubarrayThresholdRegionXo").Value = subarrayDataModel.SubarrayThresholdRegionXo.ToString();
                    updateSubarray.Element("SubarrayThresholdRegionYo").Value = subarrayDataModel.SubarrayThresholdRegionYo.ToString();
                    updateSubarray.Element("SubarrayThresholdRegiondX").Value = subarrayDataModel.SubarrayThresholdRegiondX.ToString();
                    updateSubarray.Element("SubarrayThresholdRegiondY").Value = subarrayDataModel.SubarrayThresholdRegiondY.ToString();
                    updateSubarray.Element("TimeResolvedEnabled").Value = subarrayDataModel.TimeResolvedEnabled.ToString();
                    updateSubarray.Element("TimeResolvedInterval").Value = subarrayDataModel.TimeResolvedInterval.ToString();
                    updateSubarray.Element("AdaptiveEnabled").Value = subarrayDataModel.AdaptiveEnabled.ToString();
                    xmlDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + xmlFile);
                    log.Info("End Update of"+ xmlFile +"Subarray.");
                    return true;
                }
                else
                {
                    log.Error("Update of"+ xmlFile + "Subarray failed due to no Subarray found with ID." + subarrayDataModel.SubarrayID.ToString());
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Update to Subarray of" + xmlFile + "failed");
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
        public bool SaveExposureDataToDB(string CharacterizationTest,string OperationType, string xmlFile, string rootElementName,
                                        ExposureDataModel exposureDataModel, SubarrayDataModel subarrayDataModel, string enggUserName)
        {
            var kcc = new KronosCamContext();
            bool DBChanged = false;
            bool XMLDBChanged = false;
            exposureDataModel.UserModified = string.IsNullOrWhiteSpace(enggUserName)? Environment.UserName : enggUserName;
            exposureDataModel.DateDefined = DateTime.Now;
            if (exposureDataModel.SubarrayDatas != null)
            {
                foreach (SubarrayDataModel sdm in exposureDataModel.SubarrayDatas)
                {
                    sdm.UserModified = enggUserName;
                }
            }
            try
            {                
                    if (TestAppHelper.CheckDBExists())
                    {
                        using (kcc)
                        {
                            log.Info("Begin ADD/REMOVE/UPDATE"+ CharacterizationTest + "Exposure to DB");
                            if (OperationType == "ADD")
                            {
                                kcc.exposureDataModel.Add(exposureDataModel);
                            }
                            else if (OperationType == "REMOVE")
                            {
                                ExposureDataModel exposureRecord = kcc.exposureDataModel.Where(u => u.ExposureID.Equals(exposureDataModel.ExposureID)).FirstOrDefault();
                                kcc.exposureDataModel.Remove(exposureRecord);
                            }
                            else if (OperationType == "UPDATE")
                            {
                                kcc.exposureDataModel.Attach(exposureDataModel);
                                kcc.Entry(exposureDataModel).State = System.Data.Entity.EntityState.Modified;
                            if (exposureDataModel.SubarrayDatas != null)
                            {
                                if (subarrayDataModel != null)
                                {
                                    kcc.subarrayDataModel.Attach(subarrayDataModel);
                                    kcc.Entry(subarrayDataModel).State = System.Data.Entity.EntityState.Modified;
                                }
                            }
                            }
                            int exposureSaved = kcc.SaveChanges();
                            if (exposureSaved > 0)
                            {
                                log.Info("End ADD/REMOVE/UPDATE"+ CharacterizationTest +"Exposure to DB");
                                DBChanged = true;
                            }
                            else
                            {
                                log.Error("Error occured while ADD/REMOVE/UPDATE"+ CharacterizationTest +"Exposure to DB");                               
                                DBChanged = false;
                            }
                        }
                    }
				log.Info("Database does not exist performing transaction on XML Repository");
				if (Convert.ToBoolean(ConfigurationManager.AppSettings["SaveDataInXML"]))
				{
					log.Info("Begin ADD/REMOVE/UPDATE" + CharacterizationTest + "Exposure to XML Repository");
					XDocument xmlDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + xmlFile);
					if (OperationType == "ADD")
					{
						XMLDBChanged = AddExposureXMLData(xmlFile, rootElementName, exposureDataModel, subarrayDataModel);
					}
					else if (OperationType == "REMOVE")
					{
						XMLDBChanged = RemoveExposureFromXMLData(xmlFile, exposureDataModel, subarrayDataModel);
					}
					else if (OperationType == "UPDATE")
					{
						XMLDBChanged = UpdateExposureXMLData(xmlFile, exposureDataModel, subarrayDataModel);
					}
				}
				if (DBChanged || XMLDBChanged)
					return true;
				else
					return false;
			}
			catch (DbUpdateConcurrencyException ex)
            {
                var ctx = ((IObjectContextAdapter)kcc).ObjectContext;
                ctx.Refresh(RefreshMode.ClientWins, kcc.exposureDataModel);
                ctx.Refresh(RefreshMode.ClientWins, kcc.subarrayDataModel);
                log.Error("DBUpdate Concurrency Error occured while saving data of " + CharacterizationTest);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);               
                return false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                log.Error("Error occured while saving data of " + CharacterizationTest);
                return false;
            }
        }
        public bool SaveSubarrayDataToDB(string CharacterizationTest, string OperationType, string xmlFile,
                                        ExposureDataModel exposureDataModel, SubarrayDataModel subarrayDataModel, string enggUserName)
        {
            try
            {
                bool DBChanged = false;
                bool XMLDBChanged = false;
                exposureDataModel.UserModified = enggUserName;
                exposureDataModel.DateDefined = DateTime.Now;
                subarrayDataModel.UserModified = enggUserName;
                subarrayDataModel.DateDefined = DateTime.Now;
                    if (TestAppHelper.CheckDBExists())
                    {
                        log.Info("Begin ADD/REMOVE/UPDATE Charge Transfer Losses Subarray to DB");
                        using (var kcc = new KronosCamContext())
                        {
                            if (OperationType == "ADD")
                            {
                                kcc.subarrayDataModel.Add(subarrayDataModel);
                            }
                            if (OperationType == "REMOVE")
                            {
                                var subarrayRecord = kcc.subarrayDataModel.Where(u => u.SubarrayID.Equals(subarrayDataModel.SubarrayID)).FirstOrDefault();
                                kcc.subarrayDataModel.Remove(subarrayRecord);
                            }
                            if (OperationType == "UPDATE")
                            {
                                kcc.subarrayDataModel.Attach(subarrayDataModel);
                                kcc.Entry(subarrayDataModel).State = System.Data.Entity.EntityState.Modified;
                            }
                            if (OperationType == "REMOVE" || OperationType == "ADD")
                            {
                                kcc.Set<ExposureDataModel>().AddOrUpdate(exposureDataModel);
                            }
                            int subarraySaved = kcc.SaveChanges();
                            if (subarraySaved > 0)
                            {
                                DBChanged = true;
                                log.Info("End ADD/REMOVE/UPDATE Charge Transfer Losses Subarray to DB.");
                            }
                            else
                            {
                                DBChanged = false;
							log.Error("Eror Occured while Add/Remove/Update Charge Transfer Losses Subarray records to DB");
						}
					}
				}
				log.Info("Database does not exist performing transaction on XML Repository");
				if (Convert.ToBoolean(ConfigurationManager.AppSettings["SaveDataInXML"]))
				{
					log.Info("Begin ADD/REMOVE/UPDATE Mean Variance Subarray to XML Repository");
					XDocument xmlDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + xmlFile);
					if (OperationType == "ADD")
					{
						XMLDBChanged = AddSubarrayXML(xmlFile, exposureDataModel, subarrayDataModel);
					}
					else if (OperationType == "REMOVE")
					{
						XMLDBChanged = RemoveSubarrayXML(xmlFile, exposureDataModel, subarrayDataModel);
					}
					else if (OperationType == "UPDATE")
					{
						XMLDBChanged = UpdateSubarrayXML(xmlFile, exposureDataModel, subarrayDataModel);
					}
				}
				if (DBChanged || XMLDBChanged)
					return true;
				else
					return false;
			}
            catch (DbUpdateException ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                SqlException s = ex.InnerException.InnerException as SqlException;
                errorCode = s.Number;
                return false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
    }
}
