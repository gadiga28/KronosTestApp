using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosCameraTestApp.Model
{
    [Table("ExposureData")]
    public class ExposureDataModel
    {
        public ExposureDataModel()
        {
            //this.SubarrayDatas = new HashSet<SubarrayDataModel>();
        }
        [Key]
        public int ExposureID { get; set; }
        public string TestDataID { get; set; }
        public string ExposureName { get; set; }
        public System.DateTime DateDefined { get; set; }
        public string UserModified { get; set; }
        public int ExposureRegionXo { get; set; }
        public int ExposureRegionYo { get; set; }
        public int ExposureRegiondX { get; set; }
        public int ExposureRegiondY { get; set; }
        public int ExposureInterval { get; set; }
        public int ExposureNDROS { get; set; }
        public int ExposureFPN { get; set; }
        public bool FullFrameEnabled { get; set; }
        public bool DarkFrameEnabled { get; set; }
        public bool UserDefaultConfiguration { get; set; }
        public bool LED1Enabled { get; set; }
        public bool LED2Enabled { get; set; }
        public bool LED3Enabled { get; set; }
        public int LEDOnTime { get; set; }
        public int LEDOffTime { get; set; }
        public int LEDFlashes { get; set; }
        public bool ShutterEnabled { get; set; }
        public int AutoBiasFPNEnabled { get; set; }
        public int NumberOfSubarrays { get; set; }
        public int GlobalInject { get; set; }
        public int GlobalInjectDelay { get; set; }
        public List<SubarrayDataModel> SubarrayDatas { get; set; }
        
    }
}
