using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thermo.Kronos.Instrument.Camera.Interface;

namespace KronosCameraTestApp.Model
{   
  
    [Table("MeanVarianceResults")]
    public class MeanVarianceTestResultsData
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
        public string CorrectedMean { get; set; }
        public string CorrectedVariance { get; set; }
        public string LinearCurveFit { get; set; }
        public double  Gain { get; set; }
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
