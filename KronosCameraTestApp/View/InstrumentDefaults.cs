using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Thermo.Kronos.Instrument.Camera.Interface;
namespace KronosCameraTestApp.View
{
    public partial class InstrumentDefaults : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        SCM5821AX1DataTypes.ConfigurationStructure configStruct;
        CIDInterface localCidInterface;
        string userMode = string.Empty;
        string enggUserName = string.Empty;
        InstrumentDefaultsModel instrumentDefaultsModel { get; set; }
        List<InstrumentDefaultsModel> instrumentDefaultsModelList { get; set; }
        public bool EnableOnConnect
        {
            get
            {
                return enableOnConnectCheckBox.Checked;
            }
        }
        public InstrumentDefaults(CIDInterface cidInterface, string userMode, string enggUserName)
        {
            InitializeComponent();
            this.localCidInterface = cidInterface;
            this.userMode = userMode;
            this.enggUserName = enggUserName;
            instrumentDefaultsModelList = new List<InstrumentDefaultsModel>();
            configStruct = new SCM5821AX1DataTypes.ConfigurationStructure();
            configStruct.voltages = new Thermo.Kronos.Instrument.Camera.Contracts.Data.Voltages();
            GetInstumentDefaults();
            if (instrumentDefaultsModelList != null && instrumentDefaultsModelList.Count > 0)
                UpdateViewWithModel(instrumentDefaultsModelList[0]);
            else
            {
                selinjectVb.Value = (decimal)Properties.Settings.Default.selectInjectVb;
                selinjectVc.Value = (decimal)Properties.Settings.Default.selectInjectVc;
                selinjectVd.Value = (decimal)Properties.Settings.Default.selectInjectVd;
                selstoreVa.Value = (decimal)Properties.Settings.Default.selectStoreVa;
                selstoreVb.Value = (decimal)Properties.Settings.Default.selectStoreVb;
                selstoreVc.Value = (decimal)Properties.Settings.Default.selectStoreVc;
                selsenseVa.Value = (decimal)Properties.Settings.Default.selectSenseVa;
                selsenseVb.Value = (decimal)Properties.Settings.Default.selectSenseVb;
                selsenseVc.Value = (decimal)Properties.Settings.Default.selectSenseVc;
                seltgVc.Value = (decimal)Properties.Settings.Default.transferSel;
                videobiasVb.Value = (decimal)Properties.Settings.Default.videoBias;
                pixelvddVa.Value = (decimal)Properties.Settings.Default.pixelVdd;
                imrefcdsVa.Value = (decimal)Properties.Settings.Default.cdsRef;
                impxlbiasVa.Value = (decimal)Properties.Settings.Default.impxbiasVa;
                selresetVc.Value = (decimal)Properties.Settings.Default.selResetVc;
                outbiasVd.Value = (decimal)Properties.Settings.Default.outBiasVd;
                ad9826Offset.Value = (decimal)Properties.Settings.Default.offset;
                ad9826Gain.Value = (decimal)Properties.Settings.Default.gain;
                openShutterSpeed.Value = (decimal)Properties.Settings.Default.openShutterSpeed;
                closeShutterSpeed.Value = (decimal)Properties.Settings.Default.closeShutterSpeed;
                enableOnConnectCheckBox.Checked = Properties.Settings.Default.enableOnConnect;
            }
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                    ConfigStructForInstrumentDefaults();
                this.Close();
            }           
             catch(Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateInstrumentDefaultsInDB()
        {
            try
            {
                int instrumentDefaultsSaved = 0;
                if(TestAppHelper.CheckDBExists())
                {
                    using (var kcc = new KronosCamContext())
                    {
                        kcc.instrumentDefaultsModel.Attach(instrumentDefaultsModel);
                        kcc.Entry(instrumentDefaultsModel).State = System.Data.Entity.EntityState.Modified;
                        instrumentDefaultsSaved = kcc.SaveChanges();
                    }   
                }
                if (instrumentDefaultsSaved > 0 || UpdateInstrumentDefaultsXML(instrumentDefaultsModel))
                {
                    log.Info("End UPDATE Instrument Defaults to DB");
                    MessageBox.Show("Instrument Defaults Updated in Database");
                    GetInstumentDefaults();
                    if (instrumentDefaultsModelList != null && instrumentDefaultsModelList.Count > 0)
                        UpdateViewWithModel(instrumentDefaultsModelList[0]);
                }
                else
                {
                    log.Error("Error occured while UPDATE Instrument Defaults to DB");
                    MessageBox.Show("Instrument Defaults Not Updated in Database");
                }                 
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                log.Error("Error occured while UPDATE Instrument Defaults to DB");
                MessageBox.Show("Unknow error occured while updating Instrument Defaults in Database");
            }
        }
        public void ConfigStructForInstrumentDefaults()
        {
            try
            {
                configStruct.voltages.selsenseVa = (float)selsenseVa.Value;
                configStruct.voltages.selsenseVb = (float)selsenseVb.Value;
                configStruct.voltages.selsenseVc = (float)selsenseVc.Value;
                configStruct.voltages.unselsenseVc = (float)selsenseVc.Value;
                configStruct.voltages.selstoreVa = (float)selstoreVa.Value;
                configStruct.voltages.selstoreVb = (float)selstoreVb.Value;
                configStruct.voltages.selstoreVc = (float)selstoreVc.Value;
                configStruct.voltages.unselstoreVc = (float)selstoreVc.Value;
                configStruct.voltages.selinjectVb = (float)selinjectVb.Value;
                configStruct.voltages.selinjectVc = (float)selinjectVc.Value;
                configStruct.voltages.selinjectVd = (float)selinjectVd.Value;
                configStruct.voltages.unselinjectVc = (float)selinjectVc.Value;
                configStruct.voltages.seltgVc = (float)seltgVc.Value;
                configStruct.voltages.unseltgVc = (float)seltgVc.Value;
                configStruct.voltages.videobiasVb = (float)videobiasVb.Value;
                configStruct.voltages.pixelvddVa = (float)pixelvddVa.Value;
                configStruct.voltages.imrefcdsVa = (float)imrefcdsVa.Value;
                configStruct.voltages.ad9826Offset = (float)ad9826Offset.Value;
                configStruct.voltages.ad9826Gain = (float)ad9826Gain.Value;
                configStruct.voltages.impxlbiasVa = (float)impxlbiasVa.Value;
                configStruct.voltages.selresetVc = (float)selresetVc.Value;
                configStruct.voltages.unselresetVc = (float)selresetVc.Value;
                configStruct.voltages.outbiasVd = (float)outbiasVd.Value;
                configStruct.openShutterSpeed = 1000 * (UInt32)openShutterSpeed.Value;
                configStruct.closeShutterSpeed = 1000 * (UInt32)closeShutterSpeed.Value;               
                if (localCidInterface.IsConnected())
                {                  
                    localCidInterface.ProcessCameraConfigurationData(configStruct);
                }
                else
                {
                    log.Error("Local CIDInterface is not Connected");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void InstrumentDefaults_Load(object sender, EventArgs e)
        {
            try
            {
                if (userMode.Equals("Admin"))
                {
                    buttonOK.Enabled = true;
                    enableOnConnectCheckBox.Enabled = true;
                    btnUpdateDB.Visible = true;
                    btnUpdateDB.Enabled = true;
                    DefaultSettingsGroupBox.Enabled = true;
                }
                //else if(userMode.Equals("Engineering"))
                //{
                //    DefaultSettingsGroupBox.Enabled = false;
                //    btnResetDefaults.Visible = false;
                //    btnUpdateDB.Visible = false;
                //}
                else if(userMode.Equals("Manufacturing") || userMode.Equals("Engineering") ||
                    string.IsNullOrWhiteSpace(userMode))
                {
                    DefaultSettingsGroupBox.Enabled = false;
                    btnResetDefaults.Visible = false;
                    btnUpdateDB.Visible = false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void InstrumentDefaults_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.selectInjectVb = (float)selinjectVb.Value;
                Properties.Settings.Default.selectInjectVc = (float)selinjectVc.Value;
                Properties.Settings.Default.selectInjectVd = (float)selinjectVd.Value;
                Properties.Settings.Default.selectStoreVb = (float)selstoreVb.Value;
                Properties.Settings.Default.selectStoreVc = (float)selstoreVc.Value;
                Properties.Settings.Default.selectStoreVa = (float)selstoreVa.Value;
                Properties.Settings.Default.selectSenseVb = (float)selsenseVb.Value;
                Properties.Settings.Default.selectSenseVc = (float)selsenseVc.Value;
                Properties.Settings.Default.selectSenseVa = (float)selsenseVa.Value;
                Properties.Settings.Default.transferSel = (float)seltgVc.Value;
                Properties.Settings.Default.videoBias = (float)videobiasVb.Value;
                Properties.Settings.Default.pixelVdd = (float)pixelvddVa.Value;
                Properties.Settings.Default.cdsRef = (float)imrefcdsVa.Value;
                Properties.Settings.Default.impxbiasVa = (float)impxlbiasVa.Value;
                Properties.Settings.Default.selResetVc = (float)selresetVc.Value;
                Properties.Settings.Default.outBiasVd = (float)outbiasVd.Value;
                Properties.Settings.Default.offset = (float)ad9826Offset.Value;
                Properties.Settings.Default.gain = (float)ad9826Gain.Value;
                Properties.Settings.Default.openShutterSpeed = (float)openShutterSpeed.Value;
                Properties.Settings.Default.closeShutterSpeed = (float)closeShutterSpeed.Value;
                Properties.Settings.Default.UnSelsenseVc = (float)selsenseVc.Value;
                Properties.Settings.Default.UnSelstoreVc = (float)selstoreVc.Value;
                Properties.Settings.Default.UnSelinjectVc = (float)selinjectVc.Value;
                Properties.Settings.Default.UnSeltgVc = (float)seltgVc.Value;
                Properties.Settings.Default.UnSelresetVc = (float)selresetVc.Value;
                Properties.Settings.Default.enableOnConnect = enableOnConnectCheckBox.Checked;          
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateViewWithModel(InstrumentDefaultsModel instrumentDefaultsModel)
        {
            try
            {
                textBoxInstrumentsID.Text = instrumentDefaultsModel.InstrumentDefaultsID.ToString();
                selinjectVb.Value = (decimal)instrumentDefaultsModel.SelinjectVb;
                selinjectVc.Value = (decimal)instrumentDefaultsModel.SelinjectVc;
                selinjectVd.Value = (decimal)instrumentDefaultsModel.SelinjectVd;
                selstoreVa.Value = (decimal)instrumentDefaultsModel.SelstoreVa;
                selstoreVb.Value = (decimal)instrumentDefaultsModel.SelstoreVb;
                selstoreVc.Value = (decimal)instrumentDefaultsModel.SelstoreVc;
                selsenseVa.Value = (decimal)instrumentDefaultsModel.SelsenseVa;
                selsenseVb.Value = (decimal)instrumentDefaultsModel.SelsenseVb;
                selsenseVc.Value = (decimal)instrumentDefaultsModel.SelsenseVc;
                seltgVc.Value = (decimal)instrumentDefaultsModel.SeltgVc;
                videobiasVb.Value = (decimal)instrumentDefaultsModel.VideobiasVb;
                pixelvddVa.Value = (decimal)instrumentDefaultsModel.PixelvddVa;
                imrefcdsVa.Value = (decimal)instrumentDefaultsModel.ImrefcdsVa;
                impxlbiasVa.Value = (decimal)instrumentDefaultsModel.ImpxlbiasVa;
                selresetVc.Value = (decimal)instrumentDefaultsModel.SelresetVc;
                outbiasVd.Value = (decimal)instrumentDefaultsModel.OutbiasVd;
                //eaduOffsetNumericUpDown.Value = (decimal)instrumentDefaultsModel.EADU;
                ad9826Offset.Value = (decimal)instrumentDefaultsModel.Ad9826Offset;
                ad9826Gain.Value = (decimal)instrumentDefaultsModel.Ad9826Gain;
                openShutterSpeed.Value = (decimal)instrumentDefaultsModel.OpenShutterSpeed;
                closeShutterSpeed.Value = (decimal)instrumentDefaultsModel.CloseShutterSpeed;
                textBoxDateDefined.Text = instrumentDefaultsModel.DateDefined.ToShortDateString();
                textBoxUserModified.Text = instrumentDefaultsModel.UserModified.ToString();
                enableOnConnectCheckBox.Checked = Properties.Settings.Default.enableOnConnect;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateModelWithView()
        {
            try
            {
                instrumentDefaultsModel = new InstrumentDefaultsModel();
                instrumentDefaultsModel.InstrumentDefaultsID = Convert.ToInt32(textBoxInstrumentsID.Text);
                instrumentDefaultsModel.SelsenseVa = (float)(selsenseVa.Value);
                instrumentDefaultsModel.SelsenseVb = (float)selsenseVb.Value;
                instrumentDefaultsModel.SelsenseVc = (float)selsenseVc.Value;
                instrumentDefaultsModel.UnSelsenseVc = (float)selsenseVc.Value;
                instrumentDefaultsModel.SelstoreVa = (float)selstoreVa.Value;
                instrumentDefaultsModel.SelstoreVb = (float)selstoreVb.Value;
                instrumentDefaultsModel.SelstoreVc = (float)selstoreVc.Value;
                instrumentDefaultsModel.UnSelstoreVc = (float)selstoreVc.Value;
                instrumentDefaultsModel.SelinjectVb = (float)selinjectVb.Value;
                instrumentDefaultsModel.SelinjectVc = (float)selinjectVc.Value;
                instrumentDefaultsModel.SelinjectVd = (float)selinjectVd.Value;
                instrumentDefaultsModel.UnSelinjectVc = (float)selinjectVc.Value;
                instrumentDefaultsModel.SeltgVc = (float)seltgVc.Value;
                instrumentDefaultsModel.UnSeltgVc = (float)seltgVc.Value;
                instrumentDefaultsModel.VideobiasVb = (float)videobiasVb.Value;
                instrumentDefaultsModel.PixelvddVa = (float)pixelvddVa.Value;
                instrumentDefaultsModel.ImrefcdsVa = (float)imrefcdsVa.Value;
                instrumentDefaultsModel.Ad9826Offset = (float)ad9826Offset.Value;
                instrumentDefaultsModel.Ad9826Gain = (float)ad9826Gain.Value;
                instrumentDefaultsModel.ImpxlbiasVa = (float)impxlbiasVa.Value;
                instrumentDefaultsModel.SelresetVc = (float)selresetVc.Value;
                instrumentDefaultsModel.UnSelresetVc = (float)selresetVc.Value;
                instrumentDefaultsModel.OutbiasVd = (float)outbiasVd.Value;
                instrumentDefaultsModel.OpenShutterSpeed = Convert.ToInt32(openShutterSpeed.Value);
                instrumentDefaultsModel.CloseShutterSpeed = Convert.ToInt32(closeShutterSpeed.Value);
                instrumentDefaultsModel.UserModified = enggUserName;
                instrumentDefaultsModel.DateDefined = DateTime.Now;
                TestAppHelper.instrumentDefaults = instrumentDefaultsModel;
            }
             catch(Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void btnUpdateDB_Click(object sender, EventArgs e)
        {
            UpdateModelWithView();
            UpdateInstrumentDefaultsInDB();
        }
        private void GetInstumentDefaults()
        {
            try
            {
                if (TestAppHelper.CheckDBExists())
                {
                        using (var kcc = new KronosCamContext())
                        {
                            instrumentDefaultsModelList = (from mv in kcc.instrumentDefaultsModel orderby mv.InstrumentDefaultsID select mv).ToList();
                            log.Info("Instrument defaults data populated from Database with Records of ." + instrumentDefaultsModelList.Count().ToString());
                        }
                }
                else
                {
                    //process data from xml files                   
                    ProcessInstrumentDefaultsXMLFile();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public void ProcessInstrumentDefaultsXMLFile()
        {
            try
            {
                    log.Info("Begin Reading InstrumentDefaultsData XML.");
                    DataSet ds = new DataSet();
                    ds.ReadXml(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "InstrumentDefaultsData.xml", XmlReadMode.InferSchema);
                    DataView dvExposure;
                    dvExposure = ds.Tables[0].DefaultView;
                    instrumentDefaultsModelList = new List<InstrumentDefaultsModel>();
                    foreach (DataRowView dr in dvExposure)
                    {
                        InstrumentDefaultsModel instrumentDefaultsModel = new InstrumentDefaultsModel();
                        instrumentDefaultsModel.InstrumentDefaultsID = Convert.ToInt32(dr[0]);
                        instrumentDefaultsModel.SelsenseVa = (float)(dr[1]);
                        instrumentDefaultsModel.SelsenseVb = (float)Convert.ToDecimal(dr[2]);
                        instrumentDefaultsModel.SelsenseVc = (float)Convert.ToDecimal(dr[3]);
                        instrumentDefaultsModel.UnSelsenseVc = (float)Convert.ToDecimal(dr[4]);
                        instrumentDefaultsModel.SelstoreVa = (float)Convert.ToDecimal(dr[5]);
                        instrumentDefaultsModel.SelstoreVb = (float)Convert.ToDecimal(dr[6]);
                        instrumentDefaultsModel.SelstoreVc = (float)Convert.ToDecimal(dr[7]);
                        instrumentDefaultsModel.UnSelstoreVc = (float)Convert.ToDecimal(dr[8]);
                        instrumentDefaultsModel.SelinjectVb = (float)Convert.ToDecimal(dr[9]);
                        instrumentDefaultsModel.SelinjectVc = (float)Convert.ToDecimal(dr[10]);
                        instrumentDefaultsModel.SelinjectVd = (float)Convert.ToDecimal(dr[11]);
                        instrumentDefaultsModel.UnSelinjectVc = (float)Convert.ToDecimal(dr[12]);
                        instrumentDefaultsModel.SeltgVc = (float)Convert.ToDecimal(dr[13]);
                        instrumentDefaultsModel.UnSeltgVc = (float)Convert.ToDecimal(dr[14]);
                        instrumentDefaultsModel.VideobiasVb = (float)Convert.ToDecimal(dr[15]);
                        instrumentDefaultsModel.PixelvddVa = (float)Convert.ToDecimal(dr[16]);
                        instrumentDefaultsModel.ImrefcdsVa = (float)Convert.ToDecimal(dr[17]);
                       // instrumentDefaultsModel.EADU = (float)Convert.ToDecimal(dr[18]);
                        instrumentDefaultsModel.Ad9826Offset = (float)Convert.ToDecimal(dr[19]);
                        instrumentDefaultsModel.Ad9826Gain = (float)Convert.ToDecimal(dr[20]);                        
                        instrumentDefaultsModel.ImpxlbiasVa = (float)Convert.ToDecimal(dr[21]);
                        instrumentDefaultsModel.SelresetVc = (float)Convert.ToDecimal(dr[22]);
                        instrumentDefaultsModel.UnSelresetVc = (float)Convert.ToDecimal(dr[23]);
                        instrumentDefaultsModel.OutbiasVd = (float)Convert.ToDecimal(dr[24]);
                        instrumentDefaultsModel.OpenShutterSpeed = Convert.ToInt32(dr[25]);
                        instrumentDefaultsModel.CloseShutterSpeed = Convert.ToInt32(dr[26]);
                        instrumentDefaultsModel.UserModified = Convert.ToString((dr[27]));
                        instrumentDefaultsModel.DateDefined = Convert.ToDateTime(dr[28]);
                        TestAppHelper.instrumentDefaults = instrumentDefaultsModel;
                        instrumentDefaultsModelList.Add(instrumentDefaultsModel);
                    }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public bool UpdateInstrumentDefaultsXML(InstrumentDefaultsModel instrumentDefaultsModel)
        {
            try
            {
                log.Info("Begin Update Limit in Mean Variance Limit XML.");
                XDocument xmlDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "InstrumentDefaultsData.xml");
                XElement updateInstrumentDefaults = xmlDoc.Descendants("InstrumentDefaults").FirstOrDefault
                    (p => p.Element("InstrumentDefaultsID").Value == instrumentDefaultsModel.InstrumentDefaultsID.ToString());
                if (updateInstrumentDefaults != null)
                {
                    updateInstrumentDefaults.Element("InstrumentDefaultsID").Value = instrumentDefaultsModel.InstrumentDefaultsID.ToString();
                    updateInstrumentDefaults.Element("SelsenseVa").Value = instrumentDefaultsModel.SelsenseVa.ToString();
                    updateInstrumentDefaults.Element("SelsenseVb").Value = instrumentDefaultsModel.SelsenseVb.ToString();
                    updateInstrumentDefaults.Element("SelsenseVc").Value = instrumentDefaultsModel.SelsenseVc.ToString();
                    updateInstrumentDefaults.Element("UnSelsenseVc").Value = instrumentDefaultsModel.UnSelsenseVc.ToString();
                    updateInstrumentDefaults.Element("SelstoreVa").Value = instrumentDefaultsModel.SelstoreVa.ToString();
                    updateInstrumentDefaults.Element("SelstoreVb").Value = instrumentDefaultsModel.SelstoreVb.ToString();
                    updateInstrumentDefaults.Element("SelstoreVc").Value = instrumentDefaultsModel.SelstoreVc.ToString();
                    updateInstrumentDefaults.Element("UnSelstoreVc").Value = instrumentDefaultsModel.UnSelstoreVc.ToString();
                    updateInstrumentDefaults.Element("SelinjectVb").Value = instrumentDefaultsModel.SelinjectVb.ToString();
                    updateInstrumentDefaults.Element("SelinjectVc").Value = instrumentDefaultsModel.SelinjectVc.ToString();
                    updateInstrumentDefaults.Element("UnSelinjectVc").Value = instrumentDefaultsModel.UnSelinjectVc.ToString();
                    updateInstrumentDefaults.Element("SelinjectVd").Value = instrumentDefaultsModel.SelinjectVd.ToString();
                    updateInstrumentDefaults.Element("SeltgVc").Value = instrumentDefaultsModel.SeltgVc.ToString();
                    updateInstrumentDefaults.Element("UnSeltgVc").Value = instrumentDefaultsModel.UnSeltgVc.ToString();
                    updateInstrumentDefaults.Element("VideobiasVb").Value = instrumentDefaultsModel.VideobiasVb.ToString();
                    updateInstrumentDefaults.Element("PixelvddVa").Value = instrumentDefaultsModel.PixelvddVa.ToString();
                    updateInstrumentDefaults.Element("ImrefcdsVa").Value = instrumentDefaultsModel.ImrefcdsVa.ToString();
                    //updateInstrumentDefaults.Element("EADU").Value = instrumentDefaultsModel.EADU.ToString();
                    updateInstrumentDefaults.Element("Ad9826Offset").Value = instrumentDefaultsModel.Ad9826Offset.ToString();
                    updateInstrumentDefaults.Element("Ad9826Gain").Value = instrumentDefaultsModel.Ad9826Gain.ToString();
                    updateInstrumentDefaults.Element("ImpxlbiasVa").Value = instrumentDefaultsModel.ImpxlbiasVa.ToString();
                    updateInstrumentDefaults.Element("SelresetVc").Value = instrumentDefaultsModel.SelresetVc.ToString();
                    updateInstrumentDefaults.Element("UnSelresetVc").Value = instrumentDefaultsModel.UnSelresetVc.ToString();
                    updateInstrumentDefaults.Element("OutbiasVd").Value = instrumentDefaultsModel.OutbiasVd.ToString();
                    updateInstrumentDefaults.Element("OpenShutterSpeed").Value = instrumentDefaultsModel.OpenShutterSpeed.ToString();
                    updateInstrumentDefaults.Element("CloseShutterSpeed").Value = instrumentDefaultsModel.CloseShutterSpeed.ToString();
                    updateInstrumentDefaults.Element("UserModified").Value = instrumentDefaultsModel.UserModified.ToString();
                    updateInstrumentDefaults.Element("DateDefined").Value = instrumentDefaultsModel.DateDefined.ToString();
                    xmlDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "InstrumentDefaultsData.xml");
                    log.Info("End Update Limit in Mean Variance Limit XML.");
                    return true;                   
                }
                else
                {
                    log.Error("Update Mean Variance Limit failed due to no LimitID found with ID." + instrumentDefaultsModel.InstrumentDefaultsID.ToString());
                    return false;
                }               
            }
            catch (Exception ex)
            {              
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
        private void btnResetDefaults_Click(object sender, EventArgs e)
        {
            try
            {
                GetInstumentDefaults();
                if (instrumentDefaultsModelList != null && instrumentDefaultsModelList.Count > 0)
                    UpdateViewWithModel(instrumentDefaultsModelList[0]);
                else
                {
                    selinjectVb.Value = (decimal)Properties.Settings.Default.selectInjectVb;
                    selinjectVc.Value = (decimal)Properties.Settings.Default.selectInjectVc;
                    selinjectVd.Value = (decimal)Properties.Settings.Default.selectInjectVd;
                    selstoreVa.Value = (decimal)Properties.Settings.Default.selectStoreVa;
                    selstoreVb.Value = (decimal)Properties.Settings.Default.selectStoreVb;
                    selstoreVc.Value = (decimal)Properties.Settings.Default.selectStoreVc;
                    selsenseVa.Value = (decimal)Properties.Settings.Default.selectSenseVa;
                    selsenseVb.Value = (decimal)Properties.Settings.Default.selectSenseVb;
                    selsenseVc.Value = (decimal)Properties.Settings.Default.selectSenseVc;
                    seltgVc.Value = (decimal)Properties.Settings.Default.transferSel;
                    videobiasVb.Value = (decimal)Properties.Settings.Default.videoBias;
                    pixelvddVa.Value = (decimal)Properties.Settings.Default.pixelVdd;
                    imrefcdsVa.Value = (decimal)Properties.Settings.Default.cdsRef;
                    impxlbiasVa.Value = (decimal)Properties.Settings.Default.impxbiasVa;
                    selresetVc.Value = (decimal)Properties.Settings.Default.selResetVc;
                    outbiasVd.Value = (decimal)Properties.Settings.Default.outBiasVd;
                    ad9826Offset.Value = (decimal)Properties.Settings.Default.offset;
                    ad9826Gain.Value = (decimal)Properties.Settings.Default.gain;
                    openShutterSpeed.Value = (decimal)Properties.Settings.Default.openShutterSpeed;
                    closeShutterSpeed.Value = (decimal)Properties.Settings.Default.closeShutterSpeed;
                    enableOnConnectCheckBox.Checked = Properties.Settings.Default.enableOnConnect;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
