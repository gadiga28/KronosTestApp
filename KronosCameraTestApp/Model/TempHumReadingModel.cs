using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosCameraTestApp.Model
{
    [Table("TempHumReadings")]
    public class TempHumReadingModel
    {
        [Key]
        public int ReadingID { get; set; }
        public string CameraSerialNumber { get; set; }
        public double ImagerTemperature { get; set; }
        public double ImagerHumidity { get; set; }
        public DateTime? ReadingDateTime { get; set; }
        public string TestStation { get; set; }
        public string TestStage { get; set; }
        public string UserExecuted { get; set; }
        public int? TestResultsID { get; set; }
    }
}
