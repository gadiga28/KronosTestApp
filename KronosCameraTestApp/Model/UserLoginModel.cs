using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosCameraTestApp.Model
{
     [Table("UserLogin")]
    public class UserLoginModel
    {
         [Key]
         public string UserName { get; set; }
         public string Password { get; set; }
         public string UserRole { get; set; }
         public string EmailID { get; set; }
         public bool TestFailureAlert { get; set; }

    }
}
