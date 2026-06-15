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
    public partial class OverAllTestResultsForm : Form
    {
        public OverAllTestResultsPresenter overAllTestResultsPresenter { get; set; }
        // The once-per-class call to initialize the log object
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public OverAllTestResultsForm()
        {
            InitializeComponent();
        }
        public OverAllTestResultsForm(OverAllTestResultsPresenter overAllTestResultsPresenter)
        {
            InitializeComponent();
            this.overAllTestResultsPresenter = overAllTestResultsPresenter;
        }
        private async void OverAllTestResultsForm_Load(object sender, EventArgs e)
        {
            ultraGridOverAllTestResults.DataSource = overAllTestResultsPresenter.OverAllTestResultsModelList;
        }
    }
}
