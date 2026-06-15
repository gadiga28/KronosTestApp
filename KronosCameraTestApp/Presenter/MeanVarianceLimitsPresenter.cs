using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace KronosCameraTestApp.Presenter
{
    public class MeanVarianceLimitsPresenter
    {
        KronosTestAppMainForm kronostestappMainForm { get; set; }       
        MeanVarianceTestLimitsData meanVarianceTestLimitsData { get; set; }
        List<MeanVarianceTestLimitsData> meanVarianceTestLimitsDataList { get; set; }
        string enggUserName = string.Empty;
        public string EnggUserName
        {
            get { return enggUserName; }
            set { enggUserName = value; }
        }
        public List<MeanVarianceTestLimitsData> MeanVarianceTestLimitsDataList
        {
            get { return meanVarianceTestLimitsDataList; }
            set { meanVarianceTestLimitsDataList = value; }
        }
        public MeanVarianceTestLimitsData MeanVarianceTestLimitsData
        {
            get { return meanVarianceTestLimitsData; }
            set { meanVarianceTestLimitsData = value; }
        }
        bool meanVarianceLimitsDBChanged = false;
        public XDocument xmlDoc;
        public bool MeanVarianceLimitsDBChanged
        {
            get { return meanVarianceLimitsDBChanged; }
            set { meanVarianceLimitsDBChanged = value; }
        }
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(
          System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        String path = string.Empty;
        public bool DBExists = false;
        public MeanVarianceLimitsPresenter()
        {
        }
        public MeanVarianceLimitsPresenter(MeanVarianceTestLimitsData meanVarianceTestLimitsData, KronosTestAppMainForm kronostestappMainForm)
        {
            try
            {
                this.meanVarianceTestLimitsData = meanVarianceTestLimitsData;
                meanVarianceTestLimitsDataList = new List<MeanVarianceTestLimitsData>();
                kronostestappMainForm.meanVarianceTestLimitsPresenter = this;
                this.kronostestappMainForm = kronostestappMainForm;
                path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);                             
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public async Task PopulateMeanVarianceLimitsData()
        {
            try
            {
                await Task.Run(() =>
                {
                    if(TestAppHelper.CheckDBExists())
                    {
                        using (var kcc = new KronosCamContext())
                        {
                            meanVarianceTestLimitsDataList = (from mv in kcc.meanVarianceTestLimitsData orderby mv.LimitsID select mv).ToList();
                            log.Info("Mean Variance Test Limits data populated from Database with Records of ." + meanVarianceTestLimitsDataList.Count().ToString());
                        }
                    }
                    else
                    {
                        var ret = ProcessMeanVarianceLimitsXMLFile();
                    }
                });
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }            
        }
        public async Task ProcessMeanVarianceLimitsXMLFile()
        {
            try
            {
                await Task.Run(() =>
                {
                    log.Info("Begin Reading MeanVarianceLimitsData XML.");
                    DataSet ds = new DataSet();
                    ds.ReadXml(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "MeanVarianceLimitsData.xml", XmlReadMode.InferSchema);
                    DataView dvExposure;
                    dvExposure = ds.Tables[0].DefaultView;
                    meanVarianceTestLimitsDataList = new List<MeanVarianceTestLimitsData>();
                    foreach(DataRowView dr in dvExposure)
                    {
                        MeanVarianceTestLimitsData  meanVarianceTestLimitsData= new MeanVarianceTestLimitsData();
                        meanVarianceTestLimitsData.LimitsID = Convert.ToInt32(dr[0]);
                        meanVarianceTestLimitsData.ECONumber = Convert.ToString(dr[1]);
                        meanVarianceTestLimitsData.ConversionFactorNominal = Convert.ToDouble(dr[2]);
                        meanVarianceTestLimitsData.ConverisonFactorTol = Convert.ToInt32(dr[3]);
                        meanVarianceTestLimitsData.ROI_Xs = Convert.ToInt32(dr[4]);
                        meanVarianceTestLimitsData.ROI_Ys = Convert.ToInt32(dr[5]);
                        meanVarianceTestLimitsData.ROI_dXs = Convert.ToInt32(dr[6]);
                        meanVarianceTestLimitsData.ROI_dYs = Convert.ToInt32(dr[7]);
                        meanVarianceTestLimitsData.ROI_dXbs = Convert.ToInt32(dr[8]);
                        meanVarianceTestLimitsData.ROI_dYbs = Convert.ToInt32(dr[9]);
                        meanVarianceTestLimitsData.ROI_PixRate = Convert.ToInt32(dr[10]);
                        meanVarianceTestLimitsData.ROI_NDRO = Convert.ToInt32(dr[11]);
                        meanVarianceTestLimitsData.Exposures = Convert.ToInt32(dr[12]);
                        meanVarianceTestLimitsData.Trials = Convert.ToInt32(dr[13]);
                        meanVarianceTestLimitsData.LowerPoint = Convert.ToInt32(dr[14]);
                        meanVarianceTestLimitsData.UpperPoint = Convert.ToInt32(dr[15]);
                        meanVarianceTestLimitsData.DefinedDate = Convert.ToDateTime(dr[16]);
                        meanVarianceTestLimitsData.UserModified = Convert.ToString(dr[17]);
                        meanVarianceTestLimitsData.LED = Convert.ToInt32(dr[18]);
                        meanVarianceTestLimitsData.ConversionFactorLowerLimit = Convert.ToDouble(dr[19]);
                        meanVarianceTestLimitsData.ConversionFactorUpperLimit = Convert.ToDouble(dr[20]);
                        meanVarianceTestLimitsDataList.Add(meanVarianceTestLimitsData);
                    }
                });
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
