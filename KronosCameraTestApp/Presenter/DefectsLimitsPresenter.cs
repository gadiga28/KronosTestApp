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
   public class DefectsLimitsPresenter
   {
       KronosTestAppMainForm kronostestappMainForm { get; set; }
       DefectsTestLimitsData defectsTestLimitsData { get; set; }
       List<DefectsTestLimitsData> defectsTestLimitsDataList { get; set; }
       string enggUserName = string.Empty;
       public string EnggUserName
       {
           get { return enggUserName; }
           set { enggUserName = value; }
       }     
       bool defectsLimitsDBChanged = false;
       XDocument xmlDoc;       
       static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       String path = string.Empty;
       bool DBExists = false;      
        public DefectsLimitsPresenter(DefectsTestLimitsData defectsTestLimitsData, KronosTestAppMainForm kronostestappMainForm)
        {
            try
            {
                this.defectsTestLimitsData = defectsTestLimitsData;
                defectsTestLimitsDataList = new List<DefectsTestLimitsData>();
                kronostestappMainForm.defectsTestLimitsPresenter = this;
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
