using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosCameraTestApp.Model
{
    [Table("AffectedItems")]
    public class AffectedItemsModel
    {
        [Key]
        public int ItemID_PK { get; set; }
        public string ItemName { get; set; }
        public string PartNumber { get; set; }
        public string UserDefined { get; set; }
        public DateTime DateDefined { get; set; }
    }
}
