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
    [Table("DarkCurrentResults")]
    public class DarkCurrentTestResults
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
        public string DarkCurrentMean { get; set; }
        public int PRNUDefects { get; set; }
        public int TrapDefects { get; set; }
        public int HotPixelDefects { get; set; }
        public int ClusterDefects { get; set; }
        public int TotalDefects { get; set; }     
        public int MaxDarkFF { get; set; }    
        public int AveDarkFF { get; set; } 
        public int MaxTrap { get; set; }    
        public int AveTrap { get; set; } 
        public double MaxDarkROI { get; set; }
        public int DeadPixels { get; set; }
        public int BadColumns { get; set; }
        public int BadRows { get; set; }
        public double DefectsMean { get; set; }     
      
        [NotMapped]
        public Image ResultImage { get; set; }
        public string TestResultImagePath { get; set; }
        [Key]
        public int ResultsID { get; set; }
        [NotMapped]
        public string ECONumber { get; set; }
        public int TestResultsID { get; set; }
    }
}
