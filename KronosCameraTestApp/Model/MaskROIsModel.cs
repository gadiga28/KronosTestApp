using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosCameraTestApp.Model
{
    [Table("MaskROIs")]
    public class MaskROIsModel
    {
        [Key]
        public int ROID { get; set; }
        public string Element { get; set; }       
        public int Ordnung { get; set; }
        public double Lambda { get; set; }
        public int X0 { get; set; }
        public int Y0 { get; set; }
        public int dX { get; set; }
        public int dY { get; set; }
        public bool IsDriftROI { get; set; }
    }
}
