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
    public class NDROReadDriftLimitsPresenter
    {
        KronosTestAppMainForm kronostestappMainForm { get; set; }
        NdroReadDriftTestLimitsData ndroReadDriftTestLimitsData { get; set; }
        List<NdroReadDriftTestLimitsData> ndroReadDriftTestLimitsDataList { get; set; }
        string enggUserName = string.Empty;
        public string EnggUserName
        {
            get { return enggUserName; }
            set { enggUserName = value; }
        }
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        String path = string.Empty;
        XDocument xmlDoc;
        bool DBExists = false;
        public NDROReadDriftLimitsPresenter(NdroReadDriftTestLimitsData ndroReadDriftTestLimitsData, KronosTestAppMainForm kronostestappMainForm)
        {
            try
            {
                this.ndroReadDriftTestLimitsData = ndroReadDriftTestLimitsData;
                ndroReadDriftTestLimitsDataList = new List<NdroReadDriftTestLimitsData>();
                kronostestappMainForm.nDROReadDriftTestLimitsPresenter = this;
                this.kronostestappMainForm = kronostestappMainForm;
                path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);                             
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }       
    }
}
