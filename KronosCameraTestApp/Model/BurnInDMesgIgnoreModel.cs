using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosCameraTestApp.Model
{
     [Table("BurnInDMesgIgnore")]
    public class BurnInDMesgIgnoreModel
    {
         [Key]
         public int IgnoreID { get; set; }
         public string DMesgIgnoreText { get; set; }         

    }
}
