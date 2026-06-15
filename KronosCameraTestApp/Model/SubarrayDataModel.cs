using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace KronosCameraTestApp.Model
{
     [Table("SubarrayData")]
    public class SubarrayDataModel
    {
        [Key]
        public int SubarrayID { get; set; }
        public int ExposureID { get; set; }
        public string SubarrayName { get; set; }
        public System.DateTime DateDefined { get; set; }
        public string UserModified { get; set; }
        public int SubarrayRegionXo { get; set; }
        public int SubarrayRegionYo { get; set; }
        public int SubarrayRegiondX { get; set; }
        public int SubarrayRegiondY { get; set; }
        public bool SubarrayReadEnabled { get; set; }
        public int SubarrayReadInterval { get; set; }
        public bool SubarraySubinjectEnabled { get; set; }
        public int SubarraySubinjectInterval { get; set; }
        public bool SubarrayPostReadEnabled { get; set; }
        public int SubarrayPostReadNDROS { get; set; }
        public int SubarrayFPN { get; set; }
        public bool SubarrayThresholdEnabled { get; set; }
        public int SubarrayThresholdPercent { get; set; }
        public int SubarrayThresholdRegionXo { get; set; }
        public int SubarrayThresholdRegionYo { get; set; }
        public int SubarrayThresholdRegiondX { get; set; }
        public int SubarrayThresholdRegiondY { get; set; }
        public bool TimeResolvedEnabled { get; set; }
        public int TimeResolvedInterval { get; set; }
        public bool AdaptiveEnabled { get; set; }
        public virtual ExposureDataModel ExposureDataModel { get; set; }
    }
}
