using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosCameraTestApp.Model
{
    [Table("COMPortConfiguration")]
    public class COMPortConfigurationModel
    {
        [Key]
        public int TestStationID { get; set; }
        public string StationName { get; set; }
        public string TCubeCOMPort { get; set; }
        public string BKPowerCOMPort { get; set; }
        public string FlowMeterCOMPort { get; set; }
        public bool AutoConnectCOMPorts { get; set; }
        public string UserDefined { get; set; }
        public DateTime? DateDefined { get; set; }
    }
}
