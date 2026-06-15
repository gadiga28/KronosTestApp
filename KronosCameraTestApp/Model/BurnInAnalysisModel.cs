using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosCameraTestApp.Model
{
    [Table("BurnInAnalysisDetails")]
    public class BurnInAnalysisModel
    {
        [Key]
        public int BurnInID { get; set; }
        public string ImagerSN { get; set; }
        public string CameraSN { get; set; }
        public bool BurnInResult { get; set; }
        public DateTime DateExecuted { get; set; }
        public string UserExecuted { get; set; }       
        public string BurnInLogFileData { get; set; }
        public string BurnInResultsPath { get; set; }
    }
}
