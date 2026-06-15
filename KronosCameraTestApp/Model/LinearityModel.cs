using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thermo.Kronos.Instrument.Camera.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace KronosCameraTestApp.Model
{
    
    [Table("LinearityResults")]
    public class LinearityTestResult
    {
        [Key]
        public int ResultsID { get; set; }        
        public int TestResultsID { get; set; }
        [NotMapped]
        public string CameraSerialNumber { get; set; }
        [NotMapped]
        public string ImagerSerialNumber { get; set; }
        public DateTime DateExecuted { get; set; }
        public string UserExecuted { get; set; }
        [NotMapped]
        public string UserRole { get; set; }
        public string ROID { get; set; }
        public string Deriv { get; set; }       
        public bool TestPassed { get; set; }
        [NotMapped]
        public Image ResultImage { get; set; }
        public string TestResultImagePath { get; set; }
    }
}
