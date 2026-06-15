using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosCameraTestApp.Model
{
    [Table("CameraTestFailureModesMetrics")]
    public class CameraTestFailureModesMetricsModel
    {
        [Key]
        public int CameraFailureMetricsID { get; set; }
        public int ResultsID { get; set; }
        public int CPU { get; set; }
        public int Firmware { get; set; }
        public int Coating { get; set; }
        public int Glass { get; set; }
        public int Imager { get; set; }
        public int ISIBoard { get; set; }
        public int PowerBoard { get; set; }
        public int CSPBoard { get; set; }
        public int Procedural { get; set; }
        public int TECooler { get; set; }
        public int TBD { get; set; }
        public int Others { get; set; }
    }
}
