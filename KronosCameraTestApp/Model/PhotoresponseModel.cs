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
   
    [Table("PhotoresponseResults")]
    public class PhotoresponseTestResults
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
        public string Mean { get; set; }
        public string LinearCurveFit { get; set; }
        public string LinearFitData { get; set; }
        public double FullSaturation { get; set; }
        public double LinearSaturation { get; set; }
        public int Slope { get; set; }
        public double Intercept { get; set; }
        public string DerivPlot { get; set; }       
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
