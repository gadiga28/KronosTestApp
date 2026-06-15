using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosCameraTestApp.Model
{
    [Table("EnvironmentalStatus")]
    public class EnvironmentalStatusModel
    {
        [Key]
        public int EnvironmentalStatusID { get; set; }
        [NotMapped]
        public string UserExecuted { get; set; }
        public string TestStation { get; set; }
        public double Voltage { get; set; }
        public double Amps { get; set; }
        public DateTime Date { get; set; }
        public double PurgeFlowRate { get; set; }
        public double CoolantTemp { get; set; }
        public double TankLevelLow { get; set; }
        public int TCubeFaultStatus { get; set; }
        public double FlowMeterAmps { get; set; }
        public double SetPointTemp { get; set; }
        public double PMWCooling { get; set; }
        public double FanSpeed { get; set; }
        public double PumpTemp { get; set; }      
        public int TestResultsID { get; set; }
    }
}
