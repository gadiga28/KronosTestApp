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
   
   [Table("InjectionEfficiencyResults")]   
    public class InjectionEfficiencyTestResults
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
        public string InitialExposure { get; set; }
        public string FirstInjection { get; set; }
        public string SecondInjection { get; set; }
        public string ThirdInjection { get; set; }
        public string FourthInjection { get; set; }
        public string LastInjection { get; set; }
        public string Exp0PlotData { get; set; }
        public string Exp1PlotData { get; set; }
        public string Exp2PlotData { get; set; }
        public string Exp3PlotData { get; set; }
        public string Exp4PlotData { get; set; }   
        public int MeanBox1 { get; set; }
        public int MeanBox2 { get; set; }
        public int MeanBox3 { get; set; }
        public int MeanBox4 { get; set; }
        public int MeanBox5 { get; set; }
        public int MeanBox6 { get; set; }
        public int MeanBox7 { get; set; }
        public int MeanBox8 { get; set; }
        public int MeanBox9 { get; set; }
        public int Reference { get; set; }
        public int RowCrossTalk { get; set; }
        public int ColCrossTalk { get; set; }
        public int RelativeSensitivity { get; set; }
        public bool RowXTalkPassed { get; set; }
        public bool ColXTalkPassed { get; set; }
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
