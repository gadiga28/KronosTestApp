using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosCameraTestApp.Model
{
    [Table("CameraTestLog")]
    public class CameraTestLogsModel
    {        
        [Column("DateUpdated")]
        public DateTime DateExecuted { get; set; }
        public string UserExecuted { get; set; }
        [NotMapped]
        public string ImagerSerialNumber { get; set; }
        public string TestStage { get; set; }
        public bool PreTestResult { get; set; }
        public bool FinalTestResult { get; set; }       
        public string PreTestCharacterizationTests { get; set; } 
        public string FailureMode { get; set; }
        public string RootCause { get; set; }
        public string Symptom { get; set; }
        public string Disposition { get; set; }
        public string BoardSN { get; set; }
        public bool GenerateMFR { get; set; }
        public string AffectedItems { get; set; }
        [Key]
        public int ResultsID { get; set; }
        public int CameraTestID { get; set; }
    }
}
