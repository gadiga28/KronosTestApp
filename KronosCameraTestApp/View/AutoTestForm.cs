using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
namespace KronosCameraTestApp.View
{
    public partial class AutoTestForm : Form
    {
        bool loginStatus = false;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
          System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string manufactruingUser = string.Empty;
        string userMode = string.Empty;
        string userModified = string.Empty;
        public XDocument xmlDoc;
        bool cameraTempSet =false;        
        ParametersModel parametersModel { get; set; }
        public bool CameraTempSet
        {
            get { return cameraTempSet; }
            set
            {
                cameraTempSet = value;
                if (cameraTempSet && this.InvokeRequired)
                {
                    BeginInvoke(new System.Action(() => buttonRun.Enabled = true));                    
                }
                else if(!cameraTempSet && this.InvokeRequired)
                {
                    BeginInvoke(new System.Action(() => buttonRun.Enabled = false));
                }
            }
        }       
        public ParametersModel ParametersModel
        {
            get
            {
                return parametersModel;
            }
            set
            {
                parametersModel = value;
            }
        }
       List<ParametersModel> parametersModelList { get; set; }
       public List<ParametersModel> ParametersModelList
        {
            get
            {
                return parametersModelList;
            }
            set
            {
                parametersModelList = value;
            }
        }
        private bool goAuto = false;
        public bool GoAuto
        {
            get
            {
                return goAuto;
            }
            set
            {
                goAuto = value;
            }
        }
        string[] orderofAutoTest = null; 
        public string[] OrderofAutoTest
        {
            get
            {
                return orderofAutoTest;
            }
            set
            {
                orderofAutoTest = value;
            }
        }
        bool abortAutoTest = false;
        public bool AbortAutoTest
        {
            get
            {
                return abortAutoTest;
            }
            set
            {
                abortAutoTest = value;
            }
        }
        private void buttonUp_Click(object sender, EventArgs e)
        {
            MoveUp();
        }
        private void buttonDown_Click(object sender, EventArgs e)
        {
            MoveDown();
        }
        public AutoTestForm(bool loginStatus, string UserMode, string UserModified)
        {
            InitializeComponent();
            this.loginStatus = loginStatus;
            this.userMode = UserMode;
            this.userModified = UserModified;           
            parametersModelList = new List<ParametersModel>();
        }
        void MoveUp()
        {
            try
            {
                if (listBoxAvailableTests.SelectedItem == null || listBoxAvailableTests.SelectedIndex == 0)
                    return;
                var idx = listBoxAvailableTests.SelectedIndex;
                var elem = listBoxAvailableTests.SelectedItem;
                if (idx - 1 == 0)
                    return;
                listBoxAvailableTests.Items.RemoveAt(idx);
                listBoxAvailableTests.Items.Insert(idx - 1, elem);
                listBoxAvailableTests.SelectedIndex = idx - 1;
                listBoxAvailableTests.Focus();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }               
        }
        void MoveDown()
        {
            try
            {
                if (listBoxAvailableTests.SelectedItem == null || listBoxAvailableTests.SelectedIndex == 0)
                    return;
                var idx = listBoxAvailableTests.SelectedIndex;
                var elem = listBoxAvailableTests.SelectedItem;
                if (idx + 1 >= listBoxAvailableTests.Items.Count)
                    return;
                listBoxAvailableTests.Items.RemoveAt(idx);
                listBoxAvailableTests.Items.Insert(idx + 1, elem);
                listBoxAvailableTests.SelectedIndex = idx + 1;
                listBoxAvailableTests.Focus();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }                
        }       
        private void ultraButtonRun_Click(object sender, EventArgs e)
        {
            goAuto = true;
            this.Close();
        }
        private void buttonRun_Click(object sender, EventArgs e)
        {
            try
            {
                goAuto = true;
                orderofAutoTest = new string[listBoxAvailableTests.Items.Count];
                Properties.Settings.Default.AutoTestOrder1 = new StringCollection();
                for (int i = 0; i < listBoxAvailableTests.Items.Count; i++)
                {
                    orderofAutoTest[i] = listBoxAvailableTests.Items[i].ToString();
                    Settings.Default.AutoTestOrder1.Add(listBoxAvailableTests.Items[i].ToString());
                }
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraPictureBoxTestResultsPath_Click(object sender, EventArgs e)
        {
            try
            { 
                if (string.IsNullOrWhiteSpace(Settings.Default.AutoTestResultsServerFilePathFromDB.ToString()))
                    folderBrowserDialog1.SelectedPath = @"C:\SCM5821AData\";
                else
                    folderBrowserDialog1.SelectedPath = Settings.Default.AutoTestResultsServerFilePathFromDB.ToString();
                DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    AutoTestResultsFilePath.Text = Settings.Default.AutoTestResultsServerFilePathFromDB = folderBrowserDialog1.SelectedPath;
                }          
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void AutoTestForm_Load(object sender, EventArgs e)
        {
            try
            {
                ultraLabelNote.Text += ConfigurationManager.AppSettings["TestResultsLocalFilePath"].ToString(); 
                if (loginStatus && userMode.Equals("Admin"))
                {
                    ultraPictureBoxTestResultsPath.Enabled = true;
                    AutoTestResultsFilePath.Enabled = true;
                    SaveTestResultsPath.Enabled = true;                   
                }
                if (loginStatus && (userMode.Equals("Admin") || userMode.Equals("Engineering")))
                {
                    MoveTestUp.Visible = true;
                    MoveTestDown.Visible = true;
                    buttonRun.Enabled = true;
                }
                AbortTestCheckBox.Checked = Settings.Default.AbortAutoTestCheckBox;
                //if ((userMode.Equals("Admin") || userMode.Equals("Engineering")) && (Settings.Default.AutoTestOrder1 != null && Settings.Default.AutoTestOrder1.Count> 0))
                //{
                //    listBoxAvailableTests.Items.Clear();
                //    foreach (string str in Settings.Default.AutoTestOrder1)
                //    {
                //        listBoxAvailableTests.Items.Add(str);
                //    }
                //}
                parametersModel = TestAppHelper.GetParametersModel();
                textBoxParamterID.Text =  parametersModel.ParameterID.ToString();
                AutoTestResultsFilePath.Text = Settings.Default.AutoTestResultsServerFilePathFromDB = parametersModel.TestResultsServerPath.ToString(); 
                if (!string.IsNullOrWhiteSpace(Settings.Default.AutoTestResultsServerFilePathFromDB))
                {
                    AutoTestResultsFilePath.Text = Settings.Default.AutoTestResultsServerFilePathFromDB.ToString();
                }
                else
                {
                    AutoTestResultsFilePath.Text = ConfigurationManager.AppSettings["TestResultsServerFilePath"].ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void AutoTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Settings.Default.AutoTestOrder1 = new StringCollection();
                orderofAutoTest = new string[listBoxAvailableTests.Items.Count];
                for (int i = 0; i < listBoxAvailableTests.Items.Count; i++)
                {
                    orderofAutoTest[i] = listBoxAvailableTests.Items[i].ToString();
                    Settings.Default.AutoTestOrder1.Add(listBoxAvailableTests.Items[i].ToString());
                }
                Settings.Default.AbortAutoTestCheckBox = AbortTestCheckBox.Checked;
                Settings.Default.AutoTestResultsServerFilePathFromDB = AutoTestResultsFilePath.Text;
                Settings.Default.Save();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            } 
        }
        private void AbortTestCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (AbortTestCheckBox.Checked)
                {
                    abortAutoTest = true;
                }
                else
                {
                    abortAutoTest = false;
                } 
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            } 
        }
        private void GetTestResultsPath()
        {
            try
            {                
                if (TestAppHelper.CheckDBExists())
                {
                    using (var kcc = new KronosCamContext())
                    {
                        parametersModel = new ParametersModel();
                        parametersModel = (from parameters in kcc.parametersModel orderby parameters.DateModified descending select parameters).FirstOrDefault();
                        textBoxParamterID.Text = parametersModel.ParameterID.ToString();
                        AutoTestResultsFilePath.Text = Settings.Default.AutoTestResultsServerFilePathFromDB = parametersModel.TestResultsServerPath.ToString();    
                    }
                }
                else
                {
                    //ProcessParametersXMLFile();
                    //parametersModel = new ParametersModel();
                    //parametersModel = parametersModelList.Where(pm => pm.ParameterName.Equals("TestResultsServerPath")).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {               
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);                
            } 
        }
        private void ProcessParametersXMLFile()
        {
            try
            {
                log.Info("Begin Reading UserLoginData XML.");
                DataSet ds = new DataSet();
                ds.ReadXml(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "ParametersData.xml", XmlReadMode.InferSchema);
                DataView dvExposure;
                dvExposure = ds.Tables[0].DefaultView;
                parametersModelList = new List<ParametersModel>();
                foreach (DataRowView dr in dvExposure)
                {
                    parametersModel = new ParametersModel();
                    parametersModel.ParameterID = Convert.ToInt32(dr[0]);
                  //  parametersModel.ParameterName = Convert.ToString(dr[1]);
                    //parametersModel.ParameterValue = Convert.ToString(dr[2]);
                    parametersModel.DateModified = Convert.ToDateTime(dr[3]);
                    parametersModel.UserModified = Convert.ToString(dr[4]);
                    parametersModelList.Add(parametersModel);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void SaveTestResultsPath_Click(object sender, EventArgs e)
        {
            try
            {
                parametersModel = new ParametersModel();
                if (TestAppHelper.CheckDBExists())
                {
                    using (var kcc = new KronosCamContext())
                    {
                        //parametersModel = new ParametersModel();
                        parametersModel.ParameterID =Convert.ToInt32(textBoxParamterID.Text);
                        //parametersModel.ParameterName = "TestResultsServerPath";
                        parametersModel.TestResultsServerPath = AutoTestResultsFilePath.Text;
                        parametersModel.DateModified = DateTime.Now;
                        parametersModel.UserModified = string.IsNullOrWhiteSpace(userModified) ? Environment.UserName : userModified;
                        kcc.parametersModel.Attach(parametersModel);
                        kcc.Entry(parametersModel).State = System.Data.Entity.EntityState.Modified;
                        int exposureSaved = kcc.SaveChanges();
                        if (exposureSaved > 0)
                        {
                            log.Info("Test Results Server Path Updated Successfully");                            
                        }
                        else
                        {
                            log.Error("Test Results Server Path Update failed");                            
                        }
                    }
                }
                SaveTestResultsPathInXML(parametersModel);
            }
            catch (Exception ex)
            {
                log.Error("Test Results Server Path Update failed due to unknown reason");
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);               
            } 
        }
        public void SaveTestResultsPathInXML(ParametersModel parametersModel)
        {
            try
            {
                //log.Info("Begin Save Parameters Details XML.");
                //xmlDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "ParametersData.xml");
                //XElement parameterDetails = xmlDoc.Descendants("ParameterData").FirstOrDefault(p => p.Element("ParameterName").Value == parametersModel.ParameterName.ToString());
                //if (parameterDetails != null)
                //{
                //    parameterDetails.Element("ParameterID").Value = parametersModel.ParameterID.ToString();
                //   // parameterDetails.Element("ParameterName").Value = parametersModel.ParameterName.ToString();
                //    //parameterDetails.Element("ParameterValue").Value = parametersModel.ParameterValue.ToString();
                //    parameterDetails.Element("DateModified").Value = parametersModel.DateModified.ToString();
                //    parameterDetails.Element("UserModified").Value = parametersModel.UserModified.ToString();
                //    xmlDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "ParametersData.xml");
                //}
                //else
                //{
                //    //log.Error("Update Parameters Details failed due to no Parameters details found with ParameterName." + parametersModel.ParameterName.ToString());
                //}
                //log.Info("End Save Parameters Details XML.");
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }       
    }
}
