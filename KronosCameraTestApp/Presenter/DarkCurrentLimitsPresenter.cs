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
    public class DarkCurrentLimitsPresenter
    {
        KronosTestAppMainForm kronostestappMainForm { get; set; }
        DarkCurrentTestLimitsData darkCurrentTestLimitsData { get; set; }
        List<DarkCurrentTestLimitsData> darkCurrentTestLimitsDataList { get; set; }
        bool darkCurrentLimitsDBChanged = false;
        public XDocument xmlDoc;
        string enggUserName = string.Empty;
        public string EnggUserName
        {
            get { return enggUserName; }
            set { enggUserName = value; }
        }
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        String path = string.Empty;
        public bool DBExists = false;       
        public DarkCurrentLimitsPresenter(DarkCurrentTestLimitsData darkCurrentTestLimitsData, KronosTestAppMainForm kronostestappMainForm)
        {
            try
            {
                this.darkCurrentTestLimitsData = darkCurrentTestLimitsData;
                darkCurrentTestLimitsDataList = new List<DarkCurrentTestLimitsData>();
                kronostestappMainForm.darkCurrentTestLimitsPresenter = this;
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
