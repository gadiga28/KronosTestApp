using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosCameraTestApp.Model
{
    [Table("Parameters")]
    public class ParametersModel
    {
        [Key]
        public int ParameterID { get; set; }
        public string DatabaseVersion { get; set; }      
        public string TestResultsServerPath { get; set; }
        public string ECONumber { get; set; }
        public string SoftwareVersion { get; set; }
        public string MaskROIsVersion { get; set; }
        public DateTime DateModified { get; set; }
        public string UserModified { get; set; }
    }
}
