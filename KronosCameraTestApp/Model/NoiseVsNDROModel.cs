using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosCameraTestApp.Model
{

    [Table("ReadNoiseVsNDROsResults")]
    public class NoiseVsNDROTestResults
    {
        [NotMapped]
        public string CameraSerialNumber { get; set; }
        [NotMapped]
        public string ImagerSerialNumber { get; set; }
        public DateTime DateExecuted { get; set; }
        public string UserExecuted { get; set; }
        [NotMapped]
        public string UserRole { get; set; }
        public bool TestPassed { get; set; }
        public string ExposureNDRO { get; set; }
        public string NoiseRatio { get; set; }
        public string Noise { get; set; }
        public double R2Correlation { get; set; }
        public double BestFit { get; set; }
        public double BestFitPower { get; set; }
        public string TheoryValue { get; set; }
        public double SignalReadNoise { get; set; }
        public double NDRONoise { get; set; }
        [NotMapped]
        public Image ResultImage { get; set; }
        public string TestResultImagePath { get; set; }
        [Key]
        public int ResultsID { get; set; }
        public int TestResultsID { get; set; }
        [NotMapped]
        public string ECONumber { get; set; }
    }
}
