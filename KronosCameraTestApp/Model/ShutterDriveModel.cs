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
    [Table("ShutterDriveResults")]
    public class ShutterDriveTestResults
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
        public string LabJackSN { get; set; }
        public string ShutterPlot { get; set; }
        public string TestResultImagePath { get; set; }
        [NotMapped]
        public Image ResultImage { get; set; }
        [Key]
        public int ResultsID { get; set; }
        [NotMapped]
        public string ECONumber { get; set; }
        public int TestResultsID { get; set; }
    }
}
