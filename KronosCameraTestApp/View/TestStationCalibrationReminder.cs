using KronosCameraTestApp.Model;
using KronosCameraTestApp.Presenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KronosCameraTestApp.View
{
    public partial class TestStationCalibrationReminder : Form
    {
        TestStationCalibrationModel calibrationModel { get; set; }
        List<TestStationCalibrationModel> calibrationModelsList { get; set; }
        public UserLoginPresenter userLoginPresenter { get; set; }
        DateTime updatedNotificationDate { get; set; } = DateTime.Now;
        DateTime updatedCalibrationDate { get; set; } = DateTime.Now;
        public string UserName { get; set; } = string.Empty;
        public TestStationCalibrationReminder(UserLoginPresenter UserLoginPresenter=null, string userLoginName=null)
        {
            InitializeComponent();
            userLoginPresenter = UserLoginPresenter;
            UserName = userLoginName;
        }
        private async void TestStationCalibrationReminder_Load(object sender, EventArgs e)
        {
            calibrationModel = await GetCalibrationModelForCurrentTestStation();

            if (calibrationModel != null)
            {
                grpBoxCalibrationDetails.Visible = true;
                lblCalibratedTestStation.Text = calibrationModel.TestStation;
                lblCalibratedLastDate.Text = calibrationModel.CalibratedDate == null ? string.Empty: calibrationModel.CalibratedDate.Value.ToShortDateString();
            }
            else
            {
                grpBoxCalibrationDetails.Visible = false;
            }
        }
        private async void buttonReminder_Click(object sender, EventArgs e)
        {           
           // updatedNotificationDate = DateTime.Now.AddDays(7);
            calibrationModel.NotificationDate = DateTime.Now.AddDays(7);
            await SaveCalibrationModel();
            this.Close();
        }

        private async void btnCalibrationComplete_Click(object sender, EventArgs e)
        {
            //updatedNotificationDate = DateTime.Now.AddMonths(6);
            //updatedCalibrationDate = DateTime.Now;

            calibrationModel.NotificationDate = DateTime.Now.AddMonths(6);
            calibrationModel.CalibratedDate = DateTime.Now;
            await SaveCalibrationModel();

            this.Close();
        }
        public async Task<TestStationCalibrationModel> GetCalibrationModelForCurrentTestStation()
        {
            try
            {
                var res = await Task.Run(() =>
                {
                    using (var kcc = new KronosCamContext())
                    {
                        if (!string.IsNullOrEmpty(Environment.MachineName))
                        {
                            string[] machineName = Environment.MachineName.Split('-');
                            string machine = machineName[1];
                            calibrationModel = kcc.testStationCalibrationModel.Where(t => t.TestStation.Equals(machine)).FirstOrDefault();
                            if(calibrationModel==null)
                            {
                                calibrationModel = kcc.testStationCalibrationModel.Where(t => t.TestStation.Equals(Environment.MachineName)).FirstOrDefault();
                            }
                            if (calibrationModel != null)
                            {
                                return calibrationModel;
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                });
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private async Task<bool> SaveCalibrationModel()
        {
            try
            {
                var res = await Task.Run(async () =>
                {
                    using (var kcc = new KronosCamContext())
                    {
                        if(string.IsNullOrEmpty(UserName))
                        {
                            await userLoginPresenter.GetUsersFromDB();
                            if (userLoginPresenter.UserLoginModelList != null && userLoginPresenter.UserLoginModelList.Count() > 0)
                            {
                                //string[] userName = Environment.UserName.Split('.');
                                UserLoginModel userLoginModel = new UserLoginModel();
                                string Name = Environment.UserName.Split('.')[0];
                                string Name1 = Environment.UserName.Split('.')[1];
                                userLoginModel = userLoginPresenter.UserLoginModelList.Where(u => u.UserName.Equals(Name)).FirstOrDefault();
                                if (userLoginModel == null)
                                {
                                    userLoginModel = userLoginPresenter.UserLoginModelList.Where(u => u.UserName.Equals(Name1)).FirstOrDefault();
                                }

                                if (userLoginModel != null)
                                {
                                    UserName = userLoginModel.UserName;
                                }
                                else
                                {
                                    UserName = Environment.UserName;
                                }
                            }
                            else
                            {
                                UserName = Environment.UserName;
                            }
                        }
                        
                        if (calibrationModel !=null)
                        {
                            calibrationModel.UserName = UserName;
                            kcc.testStationCalibrationModel.Attach(calibrationModel);
                            kcc.Entry(calibrationModel).State = System.Data.Entity.EntityState.Modified;
                        }
                        else
                        {                           
                            calibrationModel = new TestStationCalibrationModel();
                            calibrationModel.CalibratedDate = DateTime.Now;
                            calibrationModel.NotificationDate = DateTime.Now.AddMonths(6);
                            calibrationModel.TestStation = Environment.MachineName;                           
                            calibrationModel.CalibrationPassed = true;
                            calibrationModel.LEDCalibrationID = 1;
                            calibrationModel.UserName = UserName;
                            kcc.testStationCalibrationModel.Add(calibrationModel);
                        }
                      
                        kcc.SaveChanges();
                        return true;
                    }
                });
                return res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void TestStationCalibrationReminder_FormClosing(object sender, FormClosingEventArgs e)
        {
            //await SaveCalibrationModel();
        }
    }
}
