using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KronosCameraTestApp.Presenter;
using KronosCameraTestApp.Model;
using System.Reflection;
using KronosCameraTestApp.Helpers;
namespace KronosCameraTestApp.View.UserControls
{
    public partial class MeanVarianceLimitsUserControl : UserControl
    {
        string userMode = string.Empty;
        bool engineeringLoginStatus = false;
        string ECONumberValue = string.Empty;
        static int currentLimitsRecord = 0;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        MeanVarianceLimitsPresenter testLimitsPresenter { get; set; }
        public MeanVarianceLimitsUserControl(MeanVarianceLimitsPresenter testLimitsPresenter)
        {
            InitializeComponent();
            this.testLimitsPresenter = testLimitsPresenter;
        }
        public MeanVarianceLimitsUserControl(MeanVarianceLimitsPresenter testLimitsPresenter, string UserMode, bool EngineeringLoginStatus, string EcoNumber)
        {
            InitializeComponent();
            this.testLimitsPresenter = testLimitsPresenter;
            this.userMode=UserMode;
            this.engineeringLoginStatus = EngineeringLoginStatus;
            this.ECONumberValue = EcoNumber;
        }
        private void MeanVarianceTestLimitsUserControl_Load(object sender, EventArgs e)
        {
            currentLimitsRecord = 0;           
            if (userMode.ToString() != "")
                EnableControls();
        }
        private void EnableControls()
        {
            try
            {
                MVConversionFactorNominal.Enabled = true;
                MVConversionFactorTol.Enabled = true;
                ConversionFactorLowerLimit.Enabled = true;
                ConversionFactorUpperLimit.Enabled = true;
                MVTrials.Enabled = true;
                MVExposures.Enabled = true;
                MVUpperPoint.Enabled = true;
                MVLowerPoint.Enabled = true;
                MVLED.Enabled = true;
                MVROIXs.Enabled = true;
                MVROIYs.Enabled = true;
                MVROIdXs.Enabled = true;
                MVROIdYs.Enabled = true;
                MVROIdXbs.Enabled = true;
                MVROIdYbs.Enabled = true;
                MVROINDRO.Enabled = true;
                MVROIPixRate.Enabled = true;
                ECONumber.Enabled = true;
            }
            catch(Exception ex)
            {
            }
        }
        public void UpdateLimitsViewWithSpecificModel(MeanVarianceTestLimitsData meanVarianceTestLimitsData)
        {
            try
            {
                MVLimitsID.Text = meanVarianceTestLimitsData.LimitsID.ToString();
                MVConversionFactorNominal.Value = meanVarianceTestLimitsData.ConversionFactorNominal;
                MVConversionFactorTol.Value = meanVarianceTestLimitsData.ConverisonFactorTol;
                ConversionFactorLowerLimit.Value = meanVarianceTestLimitsData.ConversionFactorLowerLimit;
                ConversionFactorUpperLimit.Value = meanVarianceTestLimitsData.ConversionFactorUpperLimit;
                MVTrials.Value = meanVarianceTestLimitsData.Trials;
                MVExposures.Value = meanVarianceTestLimitsData.Exposures;
                MVUpperPoint.Value = meanVarianceTestLimitsData.UpperPoint;
                MVLowerPoint.Value = meanVarianceTestLimitsData.LowerPoint;
                MVLED.Value = meanVarianceTestLimitsData.LED;
                MVROIXs.Value = meanVarianceTestLimitsData.ROI_Xs;
                MVROIYs.Value = meanVarianceTestLimitsData.ROI_Ys;
                MVROIdXs.Value = meanVarianceTestLimitsData.ROI_dXs;
                MVROIdYs.Value = meanVarianceTestLimitsData.ROI_dYs;
                MVROIdXbs.Value = meanVarianceTestLimitsData.ROI_dXbs;
                MVROIdYbs.Value = meanVarianceTestLimitsData.ROI_dYbs;
                MVROINDRO.Value = meanVarianceTestLimitsData.ROI_NDRO;
                MVROIPixRate.Value = meanVarianceTestLimitsData.ROI_PixRate;
                MVDefinedDate.Text = meanVarianceTestLimitsData.DefinedDate.ToString();
                UserModified.Text = meanVarianceTestLimitsData.UserModified.ToString();
                ECONumber.Text = meanVarianceTestLimitsData.ECONumber.ToString();
                //final test report
                TestAppHelper.ConversionFactorNominal = MVConversionFactorNominal.Value;
                TestAppHelper.ConversionFactorLowerLimit = ConversionFactorLowerLimit.Value;
                TestAppHelper.ConversionFactorUpperLimit = ConversionFactorUpperLimit.Value;
                TestAppHelper.ConversionFactorTolerance = MVConversionFactorTol.Value;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public MeanVarianceTestLimitsData ReturnMeanVarianceLimitsModel()
        {
            try
            {
                MeanVarianceTestLimitsData meanVarianceTestLimitsData= new MeanVarianceTestLimitsData();
                meanVarianceTestLimitsData.LimitsID = Convert.ToInt32(MVLimitsID.Text);
                meanVarianceTestLimitsData.ConversionFactorNominal = MVConversionFactorNominal.Value;
                meanVarianceTestLimitsData.ConverisonFactorTol = Convert.ToInt32(MVConversionFactorTol.Value);
                meanVarianceTestLimitsData.ConversionFactorLowerLimit = ConversionFactorLowerLimit.Value;
                meanVarianceTestLimitsData.ConversionFactorUpperLimit = ConversionFactorUpperLimit.Value;
                meanVarianceTestLimitsData.Trials = Convert.ToInt32(MVTrials.Value);
                meanVarianceTestLimitsData.Exposures = Convert.ToInt32(MVExposures.Value);
                meanVarianceTestLimitsData.UpperPoint = Convert.ToInt32(MVUpperPoint.Value);
                meanVarianceTestLimitsData.LowerPoint = Convert.ToInt32(MVLowerPoint.Value);
                meanVarianceTestLimitsData.LED = Convert.ToInt32(MVLED.Value);
                meanVarianceTestLimitsData.ROI_Xs = Convert.ToInt32(MVROIXs.Value);
                meanVarianceTestLimitsData.ROI_Ys = Convert.ToInt32(MVROIYs.Value);
                meanVarianceTestLimitsData.ROI_dXs = Convert.ToInt32(MVROIdXs.Value);
                meanVarianceTestLimitsData.ROI_dYs = Convert.ToInt32(MVROIdYs.Value);
                meanVarianceTestLimitsData.ROI_dXbs = Convert.ToInt32(MVROIdXbs.Value);
                meanVarianceTestLimitsData.ROI_dYbs = Convert.ToInt32(MVROIdYbs.Value);
                meanVarianceTestLimitsData.ROI_NDRO = Convert.ToInt32(MVROINDRO.Value);
                meanVarianceTestLimitsData.ROI_PixRate = Convert.ToInt32(MVROIPixRate.Value);
                meanVarianceTestLimitsData.UserModified = Environment.UserName;
                meanVarianceTestLimitsData.DefinedDate = DateTime.Now;
                meanVarianceTestLimitsData.ECONumber = ECONumber.Text;
                return meanVarianceTestLimitsData;
            }
            catch (Exception ex)
            {               
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }
    }
}
