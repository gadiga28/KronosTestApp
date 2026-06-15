using Infragistics.Win.Misc;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace KronosCameraTestApp.View
{
    public partial class CreateUserAccount : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
               System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserLoginModel userLoginModel { get; set; }
        List<UserLoginModel> userLoginModelList { get; set; }
        UserLoginPresenter userLoginPresenter { get; set; }
        public CreateUserAccount()
        {
            InitializeComponent();
            userLoginModelList = new List<UserLoginModel>();
            userLoginPresenter = new UserLoginPresenter();
        }
        private void AutoCompleteExposureSubarray()
        {
            try
            {                
                AutoCompleteStringCollection userNameList = new AutoCompleteStringCollection();
                foreach (UserLoginModel mvm in userLoginModelList)
                {
                    userNameList.Add(mvm.UserName);
                }
                NewUserName.AutoCompleteMode = AutoCompleteMode.Suggest;
                NewUserName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                NewUserName.AutoCompleteCustomSource = userNameList;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task CreateUser()
        {
            try
            {                
                userLoginModel = new UserLoginModel();
                userLoginModel.UserName = NewUserName.Text;
                userLoginModel.Password = NewPassword.Text;
                userLoginModel.UserRole = UserRolesCombo.SelectedItem.ToString();
                userLoginModel.EmailID = EmailID.Text;
                userLoginModel.TestFailureAlert = TestFailureAlert.Checked;
                bool res = await userLoginPresenter.SaveUserAccount(userLoginModel,"ADD");
                if (res)
                {
                    MessageBox.Show("User Account Created successfully.", "Create User Account", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }   
                else
                {
                    MessageBox.Show("Error creating User Account.", "Create User Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    log.Error("Error creating user account");
                }
            }
            catch (Exception ex)
            {                
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                log.Error("Unknow error creating user account");
            }
        }
        private async Task DeleteUserAccount()
        {
            try
            {
                userLoginModel = new UserLoginModel();
                userLoginModel.UserName = NewUserName.Text;
                userLoginPresenter = new UserLoginPresenter();
                bool res = await userLoginPresenter.SaveUserAccount(userLoginModel, "REMOVE");
                if (res)
                {
                    MessageBox.Show("User Account Deleted successfully.", "Delete User Account", MessageBoxButtons.OK, MessageBoxIcon.Information);                   
                }
                else
                {
                    MessageBox.Show("Error deleting User Account.", "Delete User Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    log.Error("Error deleting user account");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                log.Error("Unknow error deleting user account");
            }
        }
        private async Task UpateUserAccount()
        {
            try
            {
                userLoginModel = new UserLoginModel();
                userLoginModel.UserName = NewUserName.Text;
                userLoginModel.Password = NewPassword.Text;
                userLoginModel.UserRole = UserRolesCombo.SelectedItem.ToString();
                userLoginModel.EmailID = EmailID.Text;
                userLoginModel.TestFailureAlert = TestFailureAlert.Checked;
                userLoginPresenter = new UserLoginPresenter();
                bool res = await userLoginPresenter.SaveUserAccount(userLoginModel, "UPDATE");
                if (res)
                {
                    MessageBox.Show("User Account Updated successfully.", "Update User Account", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error updating User Account.", "Update User Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    log.Error("Error updating user account");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                log.Error("Unknow error updating user account");
            }
        }
        private void ultraGridExistingUsers_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            ultraGridExistingUsers.DisplayLayout.Bands[0].Columns["Password"].Hidden = true;
            e.Layout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
        }
        private void CreateUserAccount_Load(object sender, EventArgs e)
        {
           LoadUsers();
            AddUser.MouseEnter += buttonMouseEnter;
            RemoveUser.MouseEnter += buttonMouseEnter;
            UpdateUser.MouseEnter += buttonMouseEnter;
            AddUser.MouseLeave += buttonMouseLeave;
            RemoveUser.MouseLeave += buttonMouseLeave;
            UpdateUser.MouseLeave += buttonMouseLeave;
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
        private async void LoadUsers()
        {
            await LoadExistingUsers();
            ultraGridExistingUsers.DataSource = userLoginModelList;
            AutoCompleteExposureSubarray();
        }
        private async Task LoadExistingUsers()
        {
            try
            {
                await Task.Run(() =>
                    {
                        using (var kcc = new KronosCamContext())
                        {
                            userLoginModelList = (from users in kcc.userLoginModel orderby users.UserRole select users).ToList();
                            log.Info("Existing Users data populated from Database with Records of ." + userLoginModelList.Count().ToString());
                        }
                    });
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void AddUser_Click(object sender, EventArgs e)
        {
            await CreateUser();
            LoadUsers();
        }
        private async void RemoveUser_Click(object sender, EventArgs e)
        {
            await DeleteUserAccount();
            LoadUsers();
        }
        private async void UpdateUser_Click(object sender, EventArgs e)
        {
            await UpateUserAccount();
            LoadUsers();
        }
        private void ultraGridExistingUsers_AfterRowActivate(object sender, EventArgs e)
        {
            try
            {
                NewUserName.Text = ultraGridExistingUsers.ActiveRow.Cells[0].Value.ToString();
                NewPassword.PasswordChar = '*';
                NewPassword.Text = ultraGridExistingUsers.ActiveRow.Cells[1].Value.ToString();
                if (ultraGridExistingUsers.ActiveRow.Cells[2].Value.ToString().Equals("Admin"))
                    UserRolesCombo.SelectedIndex = 0;
                else if (ultraGridExistingUsers.ActiveRow.Cells[2].Value.ToString().Equals("Manufacturing"))
                    UserRolesCombo.SelectedIndex = 1;
                else if (ultraGridExistingUsers.ActiveRow.Cells[2].Value.ToString().Equals("Engineering"))
                    UserRolesCombo.SelectedIndex = 2;
                EmailID.Text = ultraGridExistingUsers.ActiveRow.Cells[3].Value.ToString();
                TestFailureAlert.Checked = Convert.ToBoolean(ultraGridExistingUsers.ActiveRow.Cells[4].Value);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);               
            }  
        } 
        private void TestFailureAlert_Click(object sender, EventArgs e)
        {
            try
            {
                if (TestFailureAlert.CheckState == CheckState.Unchecked && string.IsNullOrWhiteSpace(EmailID.Text))
                {
                    MessageBox.Show("EmailID cannot be blank");
                    return;
                }
                else if(TestFailureAlert.CheckState== CheckState.Unchecked && !string.IsNullOrWhiteSpace(EmailID.Text))
                {
                    Regex reg = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                    if (!reg.IsMatch(EmailID.Text))
                    {
                        MessageBox.Show("Please enter a valid EmailID");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
