using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinGrid;
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
    public partial class ListExposureData : Form
	{
		static readonly log4net.ILog log = log4net.LogManager.GetLogger(
		   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		string userMode = string.Empty;
		bool engineeringLoginStatus = false;
		bool exitEditMode = true;
        string testForm = string.Empty;
		bool dataUpdatedFromListExposureWindow = false;
        string[] nonIntColmuns = new string[] { "ExposureName","TestDataID","UserModified","DateDefined", "UserDefaultConfiguration","LED1Enabled",
        "LED2Enabled","LED3Enabled", "ShutterEnabled", "SubarrayName"};
		public bool DataUpdatedFromListExposureWindow
		{
			get { return dataUpdatedFromListExposureWindow; }
			set { dataUpdatedFromListExposureWindow = value; }
		}
        List<ExposureDataModel> updatedExpList { get; set; }
        List<SubarrayDataModel> updatedSubList { get; set; }
        List<ExposureDataModel> exposureDataModelList { get; set; }
        public  List<ExposureDataModel> ExposureDataModelList
        {
            get { return exposureDataModelList; }
            set { exposureDataModelList = value; }
        }
        public ListExposureData(string userMode, bool engineeringLoginStatus, string testForm)
        {
            InitializeComponent();
			this.userMode = userMode;
			this.engineeringLoginStatus = engineeringLoginStatus;
            this.testForm = testForm;
        }
        private void ListExposureData_Load(object sender, EventArgs e)
        {
            updatedExpList = new List<ExposureDataModel>();
            updatedSubList = new List<SubarrayDataModel>();
            updatedExpList.Clear();
            updatedSubList.Clear();
            UpdateExposure.MouseEnter += buttonMouseEnter;
            UpdateExposure.MouseLeave += buttonMouseLeave;
            if (userMode == "Admin" && engineeringLoginStatus)
			{
				ultraGroupBoxUpdateExposure.Visible = true;				
			}
            if (testForm.Equals("PRT"))
            {
                using (var kcc = new KronosCamContext())
                {
                    List<SubarrayDataModel> subarrayDataModelList = new List<SubarrayDataModel>();
                    exposureDataModelList = kcc.exposureDataModel.Include("SubarrayDatas").Where(mv => mv.TestDataID.Equals("PRT")).ToList();
                }
            }
            ultraGridExposureData.DataSource = exposureDataModelList;
			ultraGridExposureData.DataBind();
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
		private void UpdateExposureData_Click(object sender, EventArgs e)
		{
			try
			{
				if (userMode == "Admin" && engineeringLoginStatus)
				{
					UltraGridExitEditMode();
					using (var kcc = new KronosCamContext())
					{
						ultraGridExposureData.AfterCellUpdate += delegate { ultraGridExposureData.UpdateData(); };
                        foreach (ExposureDataModel edm in updatedExpList)
                        {
                            edm.UserModified = TestAppHelper.UserLoginName;
                            edm.DateDefined= DateTime.Now;
                            kcc.exposureDataModel.Attach(edm);
                            kcc.Entry(edm).State = System.Data.Entity.EntityState.Modified;
                        }
                        foreach (SubarrayDataModel sdm in updatedSubList)
                        {
                            sdm.UserModified = TestAppHelper.UserLoginName;
                            sdm.DateDefined= DateTime.Now;
                            kcc.subarrayDataModel.Attach(sdm);
                            kcc.Entry(sdm).State = System.Data.Entity.EntityState.Modified;
                        }
                        if (kcc.SaveChanges() > 0)
						{
							dataUpdatedFromListExposureWindow = true;
							MessageBox.Show("Update data success", "Update Exposure", MessageBoxButtons.OK);
                            updatedExpList.Clear();
                            updatedSubList.Clear();
                        }							
						else
						{
							dataUpdatedFromListExposureWindow = false;
							MessageBox.Show("Update data failed", "Update Exposure", MessageBoxButtons.OK);
						}							
					}
				}
			}
			catch (Exception ex)
			{
				dataUpdatedFromListExposureWindow = false;
				log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
				MessageBox.Show("Update data failed", "Update Exposure", MessageBoxButtons.OK);
			}
		}		
		private void ultraGridExposureData_InitializeLayout(object sender, InitializeLayoutEventArgs e)
		{
			try
			{
				ultraGridExposureData.DisplayLayout.Bands[0].Columns["TestDataID"].Hidden = true;
				ultraGridExposureData.DisplayLayout.Bands[0].Columns["ExposureID"].Hidden = true;
				ultraGridExposureData.DisplayLayout.Bands[1].Columns["ExposureID"].Hidden = true;
				ultraGridExposureData.DisplayLayout.Bands[1].Columns["SubarrayID"].Hidden = true;
				ultraGridExposureData.DisplayLayout.Bands[1].Columns["ExposureDataModel"].Hidden = true;
			}
			catch (Exception ex)
			{
				log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
			}
		}
		private void ultraGridExposureData_ClickCell(object sender, ClickCellEventArgs e)
		{
			exitEditMode = true;
		}
		private void ultraGridExposureData_Click(object sender, EventArgs e)
		{
			UltraGridExitEditMode();
		}
		private void ListExposureData_FormClosing(object sender, FormClosingEventArgs e)
		{
			UltraGridExitEditMode();
		}
		private void UltraGridExitEditMode()
		{
			try
			{
				if (exitEditMode)
				{
					ultraGridExposureData.PerformAction(UltraGridAction.ExitEditMode);
				}
				exitEditMode = true;
			}
			catch (Exception ex)
			{
				log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
			}
		}
		private void button1_Click(object sender, EventArgs e)
		{
			ultraGridExposureData.PerformAction(UltraGridAction.Undo);
			ultraGridExposureData.PerformAction(UltraGridAction.UndoCell);
			ultraGridExposureData.PerformAction(UltraGridAction.UndoRow);		
		}
        private void ultraGridExposureData_AfterRowUpdate(object sender, RowEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraGridExposureData_AfterCellUpdate_1(object sender, CellEventArgs e)
        {
            try
            {
                UltraGridRow activeRow = ultraGridExposureData.ActiveRow;
                var exposureRecord = exposureDataModelList.FirstOrDefault(exp => exp.ExposureID.Equals(Convert.ToInt32(activeRow.Cells["ExposureID"].Value)));
                if (exposureRecord != null)
                {  
                    if(nonIntColmuns.Contains(e.Cell.Column.Key))
                    {
                        foreach (UltraGridCell cell in ultraGridExposureData.ActiveRow.Cells)
                        {
                            if (cell.Column.Key.ToString().Equals("SubarrayID"))
                            {
                                var subRecrod = exposureRecord.SubarrayDatas.FirstOrDefault(s => s.SubarrayID.Equals(Convert.ToInt32(activeRow.Cells["SubarrayID"].Value)));
                                if (subRecrod != null)
                                {
                                    updatedSubList.Add(subRecrod);
                                    break;
                                }
                            }
                            else
                            {
                                updatedExpList.Add(exposureRecord);
                                break;
                            }
                            break;
                        }
                    }
                    else
                    {
                        if (checkRange(e.Cell.Column.Key, Convert.ToDouble(e.Cell.Value)))
                        {
                            foreach (UltraGridCell cell in ultraGridExposureData.ActiveRow.Cells)
                            {
                                if (cell.Column.Key.ToString().Equals("SubarrayID"))
                                {
                                    var subRecrod = exposureRecord.SubarrayDatas.FirstOrDefault(s => s.SubarrayID.Equals(Convert.ToInt32(activeRow.Cells["SubarrayID"].Value)));
                                    if (subRecrod != null)
                                    {
                                        updatedSubList.Add(subRecrod);
                                        break;
                                    }
                                }
                                else
                                {
                                    updatedExpList.Add(exposureRecord);
                                    break;
                                }
                                break;
                            }
                        }
                        else
                        {
                            e.Cell.Value = e.Cell.OriginalValue;
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private bool checkRange(string propertyName,double value)
        {
            try
            {
                string errstring = string.Empty;
                double[] NumberOfNDRORange = new double[] { 1, 2, 4, 8, 16, 32, 64, 128, 256 };
                switch (propertyName)
                {
                    case "ExposureNDROS":
                        if (!NumberOfNDRORange.Contains(value))
                        {
                            errstring = string.Format("Number of NDROs must be 1, 2, 4, 8, 16, 32, 64, 128 or 256");
                            MessageBox.Show(errstring, "Invalid Range", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        break;
                    case "ExposureInterval":
                        if (value < 40 || value > 4260000)
                        {
                            errstring = string.Format("Exposure Interval value should be between '{0}' and '{1}'.", 40, 4260000);
                            MessageBox.Show(errstring, "Invalid Range", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        break;
                    case "LEDOnTime":
                        if (value < 0 || value > 31456)
                        {
                            errstring = string.Format("LED On Time value should be between '{0}' and '{1}'.", 0, 31456);
                            MessageBox.Show(errstring, "Invalid Range", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        break;
                    case "LEDOffTime":
                        if (value < 0 || value > 31456)
                        {
                            errstring = string.Format("LED Off Time value should be between '{0}' and '{1}'.", 0, 31456);
                            MessageBox.Show(errstring, "Invalid Range", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        break;
                    case "LEDFlashes":
                        if (value < 0 || value > 65535)
                        {
                            errstring = string.Format("LED Flahes value should be between '{0}' and '{1}'.", 0, 65535);
                            MessageBox.Show(errstring, "Invalid Range", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        break;
                    case "ExposureRegionXo":
                        if (value < 0 )
                        {
                            errstring = string.Format("Exposure Region Start X minimum value should be between '{0}'.", 0);
                            MessageBox.Show(errstring, "Invalid Range", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        break;
                    case "ExposureRegionYo":
                        if (value < 0)
                        {
                            errstring = string.Format("Exposure Region Start X minimum value should be between '{0}'.", 0);
                            MessageBox.Show(errstring, "Invalid Range", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        break;
                    case "ExposureRegiondX":
                        if (value < 4 || value > 2048)
                        {
                            errstring = string.Format("Exposure Region Width value should be between '{0}' and '{1}'.", 4, 2048);
                            MessageBox.Show(errstring, "Invalid Range", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        break;
                    case "ExposureRegiondY":
                        if (value < 4 || value > 2048)
                        {
                            errstring = string.Format("Exposure Region Height value should be between '{0}' and '{1}'.", 4, 2048);
                            MessageBox.Show(errstring, "Invalid Range", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }                          
                        break;
                    case "numericEditThresholdPercent":
                        if (value < 0 || value > 100)
                        {
                            errstring = string.Format("Subarray Thershold percentage value should be between '{0}' and '{1}'.", 0, 100);
                            MessageBox.Show(errstring, "Invalid Range", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }                        
                        break;
                    case "SubarrayRegionXo":
                        if (value < 0)
                        {
                            errstring = string.Format("Subarray Region Start X minimum value should be '{0}'.", 0);
                            MessageBox.Show(errstring, "Invalid Range", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        break;
                    case "SubarrayRegionYo":
                        if (value < 0)
                        {
                            errstring = string.Format("Subarray Region Start X minimum value should be '{0}'.", 0);
                            MessageBox.Show(errstring, "Invalid Range", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        break;
                    case "SubarrayRegiondX":
                        if (value < 4 || value > 2048)
                        {
                            errstring = string.Format("Subarray Region Width value should be between '{0}' and '{1}'.", 4, 2048);
                            MessageBox.Show(errstring, "Invalid Range", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        break;
                    case "SubarrayRegiondY":
                        if (value < 4 || value > 2048)
                        {
                            errstring = string.Format("Subarray Region Height value should be between '{0}' and '{1}'.", 4, 2048);
                            MessageBox.Show(errstring, "Invalid Range", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }                       
                        break;
                    case "NumberOfSubrrays":
                        if (value > 250)
                        {
                            errstring = string.Format("Number of Subarray's cannot be greater than 250");
                            MessageBox.Show(errstring, "Invalid Range", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        break;
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
