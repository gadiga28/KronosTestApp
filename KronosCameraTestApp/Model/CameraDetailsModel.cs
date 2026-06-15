using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosCameraTestApp.Model
{
    [Table("CameraDetails")]
    public class CameraDetailsModel
    {
        public string CameraModelNumber { get; set; }
        public string CameraSerialNumber { get; set; }
        public string ImagerSerialNumber { get; set; }
        public string DetectorType { get; set; }
        public string MACAddress { get; set; }
        public string TestSoftwareVer { get; set; }
        public string FirmwareVer { get; set; }
        public string FirmwareFile { get; set; }
        public string CRA { get; set; }
        public DateTime? ProductShippedDate { get; set; }
        public string CPUSerialNumber { get; set; }
        public string PowerSerialNumber { get; set; }
        public string CSPSerialNumber { get; set; }
        public string ISISerialNumber { get; set; }
        public string FPGAVersion { get; set; }
        [Key]
        public int CameraTestID { get; set; }
        public int TestResultsID { get; set; } 
        public DateTime DateDefined { get; set; }
        public string UserExecuted { get; set; }
        public virtual ICollection<CameraTestLogsModel> CameraTestLogs { get; set; }

    }
}
