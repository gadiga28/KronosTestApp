using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosCameraTestApp.Model
{
    [Table("TestStationCalibration")]
    public class TestStationCalibrationModel
    {
        [Key]
        public int TestStationCalibrationID { get; set; }
        public string TestStation { get; set; }
        public DateTime? NotificationDate { get; set; }
        public DateTime? CalibratedDate { get; set; }
        public bool? CalibrationPassed { get; set; }
        public virtual string UserName { get; set; }
        public virtual int? LEDCalibrationID { get; set; }
    }
}
