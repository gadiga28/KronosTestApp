using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
namespace KronosCameraTestApp.Presenter
{
    public class UserLoginPresenter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserLoginForm userLoginForm { get; set; }
        UserLoginModel userLoginModel { get; set; }
        KronosTestAppMainForm kronosTestAppMainForm { get; set; }
        public XDocument xmlDoc;
        List<UserLoginModel> userLoginModelList = new List<UserLoginModel>();       
        public List<UserLoginModel> UserLoginModelList
        {
            get { return userLoginModelList; }
            set { userLoginModelList = value; }
        }
        public UserLoginPresenter()
        {
        }
        public UserLoginPresenter(UserLoginForm userLoginForm,UserLoginModel userLoginModel, KronosTestAppMainForm kronosTestAppMainForm)
        {
            this.userLoginForm = userLoginForm;
            this.userLoginModel = userLoginModel;
            userLoginForm.userLoginPresenter = this;
            this.kronosTestAppMainForm = kronosTestAppMainForm;
            kronosTestAppMainForm.userLoginPresenter= this;
        }
        public async Task GetUsersFromDB()
        {
            try
            {
                kronosTestAppMainForm.SetDatabaseConnectivityStatusBarPanel();
                if (TestAppHelper.CheckDBExists())
                {
                    await Task.Run(() =>
                    {
                        using (var kcc = new KronosCamContext())
                        {
                            if (userLoginModelList.Count==0)
                                userLoginModelList = (from usrs in kcc.userLoginModel orderby usrs.UserName select usrs).ToList();                          
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task ProcessUserLoginXMLFile()
        {
            try
            {
                await Task.Run(() =>
                {
                    log.Info("Begin Reading UserLoginData XML.");
                    DataSet ds = new DataSet();
                    ds.ReadXml(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "UserLoginData.xml", XmlReadMode.InferSchema);
                    DataView dvExposure;
                    dvExposure = ds.Tables[0].DefaultView;
                    userLoginModelList = new List<UserLoginModel>();
                    foreach (DataRowView dr in dvExposure)
                    {
                        UserLoginModel userLoginModel = new UserLoginModel();
                        userLoginModel.UserName = Convert.ToString(dr[0]);
                        userLoginModel.Password = Convert.ToString(dr[1]);
                        userLoginModel.UserRole = Convert.ToString(dr[2]);
                        userLoginModel.EmailID = Convert.ToString(dr[3]);
                        userLoginModel.TestFailureAlert = Convert.ToBoolean(dr[4]);
                        userLoginModelList.Add(userLoginModel);
                    }
                });
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task<bool> SaveUserAccount(UserLoginModel userLoginModel, string OperationType)
        {
            try
            {
                var res = await Task.Run(() =>
                {
                    using (var kcc = new KronosCamContext())
                    {
                        log.Info("Begin create a new user account in DB");
                        if (OperationType == "ADD")
                        {
                            kcc.userLoginModel.Add(userLoginModel);
                        }
                        else if (OperationType == "REMOVE")
                        {
                            UserLoginModel userRecord = kcc.userLoginModel.Where(u => u.UserName.Equals(userLoginModel.UserName)).FirstOrDefault();
                            kcc.userLoginModel.Remove(userRecord);
                        }
                        else if (OperationType == "UPDATE")
                        {
                            kcc.userLoginModel.Attach(userLoginModel);
                            kcc.Entry(userLoginModel).State = System.Data.Entity.EntityState.Modified;                           
                        }
                        int savedTestResults = kcc.SaveChanges();
						if (Convert.ToBoolean(ConfigurationManager.AppSettings["SaveDataInXML"]))
						{
							xmlDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "UserLoginData.xml");
							if (OperationType == "ADD")
							{
								AddUserLoginDetailsXML(userLoginModel);
							}
							if (OperationType == "REMOVE")
							{
								RemoveUserLoginDetailsXML(userLoginModel);
							}
							if (OperationType == "UPDATE")
							{
								UpdateUserLoginDetailsXML(userLoginModel);
							}
						}
                        if (savedTestResults > 0)
                        {                
                            log.Info("Created a new user account in DB");
                            return true;
                        }
                        else
                        {
                            log.Error("Error occured while creating a new user account in DB");
                            return false;                            
                        }
                    }
                });
                return res;
            }
            catch(DbUpdateException ex)
            {
                MessageBox.Show("User Name already exists", "Create User Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating User account", "Create User Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;                
            }
        }
        public void AddUserLoginDetailsXML(UserLoginModel userLoginModel)
        {
            try
            {
                log.Info("Begin Add User Login details to XML.");
                xmlDoc.Element("UserLoginDetails").Add
                    (
                    new XElement("UserLoginData",
                    new XElement("UserName", userLoginModel.UserName),
                    new XElement("Password", userLoginModel.Password),
                    new XElement("UserRole", userLoginModel.UserRole),
                     new XElement("EmailID", userLoginModel.EmailID),
                    new XElement("TestFailureAlert", userLoginModel.TestFailureAlert)
                    ));
                xmlDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "UserLoginData.xml");
                log.Info("End Add User Login details XML.");
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public void RemoveUserLoginDetailsXML(UserLoginModel userLoginModel)
        {
            try
            {
                log.Info("Begin Remove User Login details XML.");
                XElement removeUserDetails = xmlDoc.Descendants("UserLoginData").FirstOrDefault(p => p.Element("UserName").Value == userLoginModel.UserName.ToString());
                if (removeUserDetails != null)
                {
                    removeUserDetails.Remove();
                    xmlDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "UserLoginData.xml");
                    xmlDoc = XDocument.Load(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "UserLoginData.xml");
                }
                else
                {
                    log.Error("Remove User from XML failed due to no User Name found with UserName." + userLoginModel.UserName.ToString());
                }
                log.Info("End Remove User Login details XML.");
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public void UpdateUserLoginDetailsXML(UserLoginModel userLoginModel)
        {
            try
            {
                log.Info("Begin Update User Login Details XML.");
                XElement updatedUserDetails = xmlDoc.Descendants("UserLoginData").FirstOrDefault(p => p.Element("UserName").Value == userLoginModel.UserName.ToString());
                if (updatedUserDetails != null)
                {
                    updatedUserDetails.Element("UserName").Value = userLoginModel.UserName.ToString();
                    updatedUserDetails.Element("Password").Value = userLoginModel.Password.ToString();
                    updatedUserDetails.Element("UserRole").Value = userLoginModel.UserRole.ToString();
                    updatedUserDetails.Element("EmailID").Value = userLoginModel.EmailID.ToString();
                    updatedUserDetails.Element("TestFailureAlert").Value = userLoginModel.TestFailureAlert.ToString();
                    xmlDoc.Save(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "MeanVarianceData.xml");
                }
                else
                {
                    log.Error("Update User Login Details failed due to no User found with UserName." + userLoginModel.UserName.ToString());
                }
                log.Info("End User Login Details XML.");
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }       
        public bool AuthenticateEngineeringUser(string userName, string password)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                log.Info("Begin User Login");
                UserAuthentication.AuthenticateUser(userName, password);
                Cursor.Current = Cursors.Arrow;
                log.Info("User Login successfull");               
                log.Info("End User Login");
                //this.Close();
                return true;
            }
            catch (Win32Exception ex)
            {
                switch (ex.NativeErrorCode)
                {
                    case 1326: // ERROR_LOGON_FAILURE (incorrect user name or password)
                        MessageBox.Show("Invalid Username or Password", "Engineering User Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        log.Error("Invalid Username or password entered", ex);
                        break;
                    case 1327: // ERROR_ACCOUNT_RESTRICTION
                        MessageBox.Show("This Account is Restricted, please contact your system administrator", "Engineering User Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        log.Error("Account is Restricted",ex);
                        break;
                    case 1330: // ERROR_PASSWORD_EXPIRED
                        MessageBox.Show("Password Expired, please try with updated password", "Engineering User Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        log.Error("Password Expired",ex);
                        break;
                    case 1331: // ERROR_ACCOUNT_DISABLED
                        MessageBox.Show("This account is disabled, please contact your system administrator", "Engineering User Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        log.Error("Account is disabled",ex);
                        break;
                    case 1907: // ERROR_PASSWORD_MUST_CHANGE
                        MessageBox.Show("Please change your Password", "Engineering User Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        log.Error("Change password",ex);
                        break;
                    case 1909: // ERROR_ACCOUNT_LOCKED_OUT
                        MessageBox.Show("Account is Locked Out, , please contact your system administrator", "Engineering User Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        log.Error("Account Locked Out",ex);
                        break;
                    default: // Other
                        break;
                }
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
