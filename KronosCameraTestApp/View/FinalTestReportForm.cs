using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.View.UserControls;
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
    public partial class FinalTestReportForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
          System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string defaultFilePath = string.Empty;
        string filepath = string.Empty;
        string userMode = string.Empty;
        public FinalTestReportForm(string filepath, string UserMode)
        {
            InitializeComponent();
            this.filepath = filepath;
            this.userMode = UserMode;
        }

        private void FinalTestReportForm_Load(object sender, EventArgs e)
        {
            try
            {
                FinalTestReportForm_Fill_Panel.ClientArea.Controls.Add(new FinalTestReportUserControl(filepath,userMode));              
            }

            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void FinalTestReportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
