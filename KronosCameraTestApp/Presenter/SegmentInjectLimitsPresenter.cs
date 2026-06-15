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
    public class SegmentInjectLimitsPresenter
    {
        KronosTestAppMainForm kronostestappMainForm { get; set; }
        SegmentInjectLimitsData segmentInjectLimitsData { get; set; }
        List<SegmentInjectLimitsData> segmentInjectLimitsDataList { get; set; }
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        String path = string.Empty;
        public SegmentInjectLimitsPresenter(SegmentInjectLimitsData segmentInjectLimitsData, KronosTestAppMainForm kronostestappMainForm)
        {
            try
            {
                this.segmentInjectLimitsData = segmentInjectLimitsData;
                segmentInjectLimitsDataList = new List<SegmentInjectLimitsData>();
                kronostestappMainForm.segmentInjectLimitsPresenter= this;
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
