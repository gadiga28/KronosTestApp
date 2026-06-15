using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosCameraTestApp.Model
{
    [Table("LEDCalibrationResults")]
    public class LEDCalibrationTestResults
    {
        [Key]
        public int ResultsID { get; set; }
        [NotMapped]
        public string ECONumber { get; set; }
        public DateTime DateExecuted { get; set; }
        public string UserExecuted { get; set; }
        public string Workstation { get; set; }
        public double Red40Actual { get; set; }
        public double Red60Actual { get; set; }
        public bool Red40Passed { get; set; }
        public bool Red60Passed { get; set; }
        public double Green40Actual { get; set; }
        public double Green60Actual { get; set; }
        public bool Green40Passed { get; set; }
        public bool Green60Passed { get; set; }
        public double Blue40Actual { get; set; }
        public double Blue60Actual { get; set; }
        public bool Blue40Passed { get; set; }
        public bool Blue60Passed { get; set; }
        public int TestCounter { get; set; }
    }
}
