using KronosCameraTestApp.Model;
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
    public partial class CameraTestMetricsChart : Form
    {
        List<CameraTestLogsModel> cameraTestLogsList;
        public CameraTestMetricsChart(List<CameraTestLogsModel> cameraTestLogsList)
        {
            InitializeComponent();
            this.cameraTestLogsList = cameraTestLogsList;
        }

        private void CameraTestMetricsChart_Load(object sender, EventArgs e)
        {
            ultraChart1.Data.DataSource = cameraTestLogsList;
            ultraChart1.DataBind();
        }
    }
}
