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
    public class InjectionPerformanceLimitsPresenter
    {
        KronosTestAppMainForm kronostestappMainForm { get; set; }
        InjectionPerformanceLimitsData injectionPerformanceLimitsData { get; set; }
        List<InjectionPerformanceLimitsData> injectionPerformanceLimitsDataList { get; set; }
        string enggUserName = string.Empty;
        public string EnggUserName
        {
            get { return enggUserName; }
            set { enggUserName = value; }
        }
        public List<InjectionPerformanceLimitsData> InjectionPerformanceLimitsDataList
        {
            get { return injectionPerformanceLimitsDataList; }
            set { injectionPerformanceLimitsDataList = value; }
        }
        public InjectionPerformanceLimitsData InjectionPerformanceLimitsData
        {
            get { return injectionPerformanceLimitsData; }
            set { injectionPerformanceLimitsData = value; }
        }
        bool injectionPerformanceLimitsDBChanged = false;
        public XDocument xmlDoc;
        public bool InjectionPerformanceLimitsDBChanged
        {
            get { return injectionPerformanceLimitsDBChanged; }
            set { injectionPerformanceLimitsDBChanged = value; }
        }
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);          
        String path = string.Empty;
        public bool DBExists = false;
        public InjectionPerformanceLimitsPresenter(InjectionPerformanceLimitsData injectionPerformanceLimitsData, KronosTestAppMainForm kronostestappMainForm)
        {
            try
            {
                this.injectionPerformanceLimitsData = injectionPerformanceLimitsData;
                injectionPerformanceLimitsDataList = new List<InjectionPerformanceLimitsData>();
                kronostestappMainForm.injectionPerformanceTestLimitsPresenter = this;
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
