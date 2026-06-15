using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronosCameraTestApp.Model
{
    [Table("FinalTestResultsDetails")]
    public  class FinalTestResultsDetailsModel
    {    
        public double ConversionFactorNominal { get; set; }//conversion factor
        public int TotalNumDefects { get; set; }//total # defective pixels
        public int TotalNumClusters { get; set; } //total # cluster defects     
        public int TotalColumnDefects { get; set; }//total # defective pixels
        public int TotalRowDefects { get; set; } //total # cluster defects     
        public int AveTrap { get; set; } //Ave trap level
        public double MaxDarkROI { get; set; }//Mark dark current in ROI
        public int FullWellLevelMin { get; set; }//Full Well
        public double SnglNoiseMax { get; set; }//Read Noise @ 1 NDRO
        public double PowerFitUpper { get; set; }// Read Noise Best Fit 'Power' Upper
        public double PowerFitLower { get; set; }// Read Noise Best Fit 'Power' Lower
        public double BestFitPower { get; set; }// Read Noise Best Fit 'Power' Upper
        public double RPower2Correlation { get; set; } //Read Noise Best Fit R 2 Correlation
        public string InjEfficiencyInitialExposure { get; set; }// Mean Singal After Subarray Injections
        public string InjEfficiencyFirstInjection { get; set; }// Std Dev of Repeated Subarray Injections
        public string InjEfficiencyLastInjection { get; set; }// Std Dev of Repeated Subarray Injections
        public bool ShutterDrive { get; set; }
        public bool RedBlue { get; set; }        
        public int HotPixels { get; set; }       
        public int DarkPixels { get; set; }        
        public int DeadPixels { get; set; }
        public Nullable<int> DriftROICount { get; set; }
        public string DefectivePixelPerROI { get; set; }
        public Nullable<double> UVMean { get; set; }
        public int MeanBox1 { get; set; }
        public int MeanBox2 { get; set; }
        public int MeanBox3 { get; set; }
        public int MeanBox4 { get; set; }
        public int MeanBox5 { get; set; }
        public int MeanBox6 { get; set; }
        public int MeanBox7 { get; set; }
        public int MeanBox8 { get; set; }
        public int MeanBox9 { get; set; }
        public int Reference { get; set; }
        public int RowCrossTalk { get; set; }
        public int ColCrossTalk { get; set; }
        public int RelativeSensitivity { get; set; }
        public bool RowXTalkPassed { get; set; }
        public bool ColXTalkPassed { get; set; }
        public Nullable<double> CameraAvgCurrent { get; set; }
        public Nullable<double> CameraAvgTemperature { get; set; }
        [Key]
        public int FinalTestID { get; set; }
        public int TestResultsID { get; set; }
        [NotMapped]
        public bool DefectsTestDeadDarkPixelLimitReached { get; set; }
    }
}
