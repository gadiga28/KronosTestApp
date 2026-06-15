using Infragistics.Win.UltraWinGrid;
using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using NationalInstruments.UI.WindowsForms;
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
    public partial class BurnInAnalysisResults : Form
    {
        List<BurnInAnalysisModel> burnInAnalysisModelList = new List<BurnInAnalysisModel>();     
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public BurnInAnalysisResults()
        {
            InitializeComponent();
        }
        private async void BurnInAnalysisResults_Load(object sender, EventArgs e)
        {
            try
            {
                await GetBurnInAnalysisDetailsFromDB();
                burnInAnalysisGrid.DataSource = burnInAnalysisModelList;
            }
            catch(Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task GetBurnInAnalysisDetailsFromDB()
        {
            try
            {
                await Task.Run(() => 
                {
                    Application.UseWaitCursor = true;
                    using (var kcc = new KronosCamContext())
                    {                       
                        burnInAnalysisModelList = TestAppHelper.GetBurnInAnalysisData();
                    }                   
                    Application.UseWaitCursor = false;
                });
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                Application.UseWaitCursor = false;
            }
        }
        private void InitializeGridLayout()
        {
            try
            {
                Application.UseWaitCursor = true;
                UseWaitCursor = true;
                burnInAnalysisGrid.DisplayLayout.AutoFitStyle = AutoFitStyle.ExtendLastColumn;
                burnInAnalysisGrid.DisplayLayout.Bands[0].Columns["BurnInLogFileData"].Hidden = true;
                foreach (UltraGridColumn column in burnInAnalysisGrid.DisplayLayout.Bands[0].Columns)
                {
                    column.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.All);
                }
                if (burnInAnalysisGrid.Rows.Count > 0)
                {
                    foreach (UltraGridRow row in burnInAnalysisGrid.Rows)
                    {
                        foreach (UltraGridCell cell in row.Cells)
                        {
                            if (cell.Column.Key.ToString().Equals("BurnInResult"))
                            {
                                if ((bool)cell.Value)
                                    cell.Appearance.BackColor = Color.LimeGreen;
                                else
                                {
                                    cell.Appearance.BackColor = Color.Red;
                                }
                            }
                        }
                    }
                }
                Application.UseWaitCursor = false;
                UseWaitCursor = false;
            }
            catch (Exception ex)
            {
                UseWaitCursor = false;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void burnInAnalysisGrid_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                UseWaitCursor = true;
                InitializeGridLayout();
                e.Layout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
                UseWaitCursor = false;
            }
            catch (Exception ex)
            {
                UseWaitCursor = false;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void burnInAnalysisGrid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                UseWaitCursor = true;
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                {
                    burnInAnalysisGrid_ClickCell(null, null);
                }
                UseWaitCursor = false;
            }
            catch (Exception ex)
            {
                UseWaitCursor = false;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void burnInAnalysisGrid_ClickCell(object sender, Infragistics.Win.UltraWinGrid.ClickCellEventArgs e)
        {
            try
            {
                UseWaitCursor = true;
                foreach (Infragistics.Win.UltraWinGrid.UltraGridCell cell in burnInAnalysisGrid.ActiveRow.Cells)
                {
                    if (cell.Column.Key.ToString().Equals("BurnInLogFileData"))
                    {
                        if (cell.Value != null)
                        {
                            burnInAnalysisData.Text = cell.Value.ToString();
                        }
                        else
                        {
                            burnInAnalysisData.Text = string.Empty;
                        }
                    }
                }
                highlightErrors();
                UseWaitCursor = false;
            }
            catch (Exception ex)
            {
                UseWaitCursor = false;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void highlightErrors()
        {
            try
            {
                int index = 0;
                string temp = burnInAnalysisData.Text;
                while (index < burnInAnalysisData.Text.LastIndexOf("Error", StringComparison.CurrentCultureIgnoreCase))
                {
                    burnInAnalysisData.Find("Error", index, burnInAnalysisData.TextLength, RichTextBoxFinds.None);
                    burnInAnalysisData.SelectionBackColor = Color.Red;
                    burnInAnalysisData.SelectionFont = new Font(burnInAnalysisData.SelectionFont.FontFamily, burnInAnalysisData.SelectionFont.Size, FontStyle.Bold);
                    index = burnInAnalysisData.Text.IndexOf("Error", index, StringComparison.CurrentCultureIgnoreCase) + 1;
                }
                index = 0;
                while (index < burnInAnalysisData.Text.LastIndexOf("Fail", StringComparison.CurrentCultureIgnoreCase))
                {
                    burnInAnalysisData.Find("Fail", index, burnInAnalysisData.TextLength, RichTextBoxFinds.None);
                    burnInAnalysisData.SelectionBackColor = Color.Orange;
                    burnInAnalysisData.SelectionFont = new Font(burnInAnalysisData.SelectionFont.FontFamily, burnInAnalysisData.SelectionFont.Size, FontStyle.Bold);
                    index = burnInAnalysisData.Text.IndexOf("Fail", index, StringComparison.CurrentCultureIgnoreCase) + 1;
                }
                burnInAnalysisData.ReadOnly = true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
