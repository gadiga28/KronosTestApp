using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosCameraTestApp.Model
{
    [Table("InstrumentDefaults")]
   public class InstrumentDefaultsModel
    {
        [Key]
       public int InstrumentDefaultsID { get; set; }
        //Select Sense Gate
        public double SelsenseVa { get; set; }
        public double SelsenseVb { get; set; }
        public double SelsenseVc { get; set; }
        public double UnSelsenseVc { get; set; } 

        //Select Storage Gate
        public double SelstoreVa { get; set; }
        public double SelstoreVb { get; set; }
        public double SelstoreVc { get; set; }
        public double UnSelstoreVc { get; set; }

        //Select Inject Gate
        public double SelinjectVb { get; set; }
        public double SelinjectVc { get; set; }
        public double UnSelinjectVc { get; set; }
        public double SelinjectVd { get; set; }

        //Transfer Gate
        public double SeltgVc { get; set; }
        public double UnSeltgVc { get; set; }
        public double VideobiasVb { get; set; }
        public double PixelvddVa { get; set; }
        public double ImrefcdsVa { get; set; }

        //Offsets
        public double Ad9826Offset { get; set; }
        public double Ad9826Gain { get; set; }
        public double ImpxlbiasVa { get; set; }
        public double SelresetVc { get; set; }
        public double UnSelresetVc { get; set; }
        public double OutbiasVd { get; set; }        

       //Shutter Delay
       public int OpenShutterSpeed { get; set; }
       public int CloseShutterSpeed { get; set; }

       public string UserModified { get; set; }
       public DateTime DateDefined { get; set; }
    }
}
