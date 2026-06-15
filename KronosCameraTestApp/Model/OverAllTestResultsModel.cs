using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosCameraTestApp.Model
{
    [Table("OverallTestResults")]
   public class OverAllTestResultsModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateExecuted { get; set; }
        [NotMapped]
        public string CameraSerialNumber { get; set; }
        [NotMapped]
        public string ImagerSerialNumber { get; set; }
        public string UserExecuted { get; set; }
        public string UserRole { get; set; }
        public bool TestPassed { get; set; }
        public string TestStage { get; set; }
        public string TestFailureReason { get; set; }
        public string BurnInResult { get; set; }
        [Key]        
        public int TestResultsID { get; set; }
        [NotMapped]
        public int LimitsID { get; set; }
        public string ECONumber { get; set; }
        public string TestStation { get; set; }
        [NotMapped]
        public Image ResultImage { get; set; }
        public string TestResultImagePath { get; set; }
        public virtual ICollection<FinalTestResultsDetailsModel> FinalTestResultsDetails { get; set; }
        public virtual ICollection<CameraDetailsModel> CameraDetails { get; set; }
        public virtual ICollection<MeanVarianceTestResultsData > MeanVarinaceTestResults { get; set; }
        public virtual ICollection<RedBlueTestResults> RedBlueTestResults { get; set; }
        public virtual ICollection<NoiseVsNDROTestResults> NoiseVsNDROTestResults { get; set; }
        public virtual ICollection<DarkCurrentTestResults> DarkCurrentTestResults { get; set; }
        public virtual ICollection<DefectsTestResults> DefectsTestResults { get; set; }
        public virtual ICollection<InjectionEfficiencyTestResults> InjectionEfficiencyTestResults { get; set; }
        public virtual ICollection<PhotoresponseTestResults> PhotoresponseTestResults { get; set; }
        public virtual ICollection<ShutterDriveTestResults> ShutterDriveTestResults { get; set; }
        public virtual ICollection<EnvironmentalStatusModel> EnvironmentalStatusModel { get; set; }
        public virtual ICollection<TempHumReadingModel> TempHumReadingModel { get; set; }
    }
}
