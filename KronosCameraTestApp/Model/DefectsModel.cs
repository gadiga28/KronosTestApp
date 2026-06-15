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
    [Table("DefectsResults")]
    public class DefectsTestResults
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
        public Nullable<int> PRNUDefects { get; set; }
        public Nullable<int> TrapDefects { get; set; }
        public Nullable<int> HotPixelDefects { get; set; }
        public Nullable<int> ClusterDefects { get; set; }
        public Nullable<int> DeadPixelDefects { get; set; }
        public Nullable<int> DarkPixelDefects { get; set; }
        public Nullable<int> DarkColumns { get; set; }
        public Nullable<int> DarkRows { get; set; }       
        public Nullable<int> LightColumns { get; set; }
        public Nullable<int> LightRows { get; set; }
        public Nullable<double> MeanDefects { get; set; }
        public Nullable<int> DriftROICount { get; set; }
        public string DefectivePixelsPerROICount { get; set; }
        public Nullable<int> DefPixInMaskROI { get; set; }
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
