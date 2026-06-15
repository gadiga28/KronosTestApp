using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.Presenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace KronosCameraTestApp.View
{
    public partial class UserLoginForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        bool loginStatus = false;
        bool loginFormCancelled = false;
        string enggLoggedUserName = string.Empty;
        public UserLoginPresenter userLoginPresenter { get; set; }
        UserLoginModel userLoginModel { get; set; }
        List<UserLoginModel> userLoginModelList { get; set; }
        string userRole = string.Empty;
        public string EnggLoggedUserName
        {
            get { return enggLoggedUserName; }
            set { enggLoggedUserName = value; }
        }
        public bool LoginStatus
        {
            get { return loginStatus; }
            set { loginStatus = value; }
        }
        public bool LoginFormCancelled
        {
            get { return loginFormCancelled; }
            set { loginFormCancelled = value; }
        }
        public UserLoginForm()
        {
            InitializeComponent();
        }
        public UserLoginForm(UserLoginPresenter userLoginPresenter,string UserRole)
        {
            InitializeComponent();
            this.userRole = UserRole;
            this.userLoginPresenter = userLoginPresenter;
        }
        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            try
            {  
               loginStatus = false;
                if(TestAppHelper.CheckDBExists())
                    await userLoginPresenter.GetUsersFromDB();
                else
                    await userLoginPresenter.ProcessUserLoginXMLFile();
               if (userLoginPresenter.UserLoginModelList.Count > 0)
               {
                   userLoginModel = userLoginPresenter.UserLoginModelList.Where(user => user.UserName.Equals(ultraTextEditorUserName.Text) && user.Password.Equals(ultraTextEditorPassword.Text)).FirstOrDefault();
                   if (userLoginModel != null)
                   {                     
                       if (userRole.Equals(userLoginModel.UserRole))
                       {
                           loginStatus = true;
                           enggLoggedUserName = userLoginModel.UserName;
                           this.Close(); 
                       }
                       else
                       {
                           MessageBox.Show("Incorrect User Name or Password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                       } 
                   }
                   else if (userLoginModel ==null || !loginStatus)
                   {
                       MessageBox.Show("Incorrect User Name or Password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   }
               }
            }
            catch (Win32Exception ex)
            {
                loginStatus = false;
                enggLoggedUserName = string.Empty;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                enggLoggedUserName = string.Empty;
                loginStatus = false;
            }
        }    
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            loginStatus = false;
            loginFormCancelled = true;
            enggLoggedUserName = string.Empty;
            this.Close();
        }
        private void UserLoginForm_Load(object sender, EventArgs e)
        {  
            if(Control.IsKeyLocked(Keys.CapsLock ))
            {
                MessageBox.Show("The Caps Lock key is ON.");
            }
            if(userRole.Equals("Admin"))
            {
                this.Text = "Admin User Login";
            }
            else
            {
                this.Text = "Engineering User Login";
            }
        }
        private void UserLoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
