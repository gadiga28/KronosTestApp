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
    public partial class LEDCalibrationTestResultsForm : Form
    {
        public LEDCalibrationTestResultsForm()
        {
            InitializeComponent();
        }

        private async void LEDCalibrationTestResults_Load(object sender, EventArgs e)
        {
            LEDCalibrationPresenter ledCalibrationPresenter = new LEDCalibrationPresenter();
            List<LEDCalibrationTestResults> lEDCalibrationTestResults = await ledCalibrationPresenter.GetLEDCalibrationResults();
            if(lEDCalibrationTestResults.Count > 0)
            {
                ledCalibrationResultsGrid.DataSource = lEDCalibrationTestResults;
            }
        }
    }
}
