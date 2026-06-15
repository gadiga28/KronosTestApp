using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace KronosCameraTestApp.Model
{
    [Table("MeanVarianceLimits")]
    public class MeanVarianceTestLimitsData
    {
        [Key]
        public int LimitsID { get; set; }
        public string ECONumber { get; set; }
        public double ConversionFactorNominal { get; set; }
        public int ConverisonFactorTol { get; set; }
        public int ROI_Xs { get; set; }
        public int ROI_Ys { get; set; }
        public int ROI_dXs { get; set; }
        public int ROI_dYs { get; set; }
        public int ROI_dXbs { get; set; }
        public int ROI_dYbs { get; set; }
        public int ROI_PixRate { get; set; }
        public int ROI_NDRO { get; set; }
        public int Exposures { get; set; }
        public int Trials { get; set; }
        public int UpperPoint { get; set; }
        public int LowerPoint { get; set; }
        public int LED { get; set; }
        public DateTime DefinedDate { get; set; }
        public string UserModified { get; set; }
        public double ConversionFactorLowerLimit { get; set; }
        public double ConversionFactorUpperLimit { get; set; }
    }
    [Table("ChargeTransferLossesLimits")]
    public class ChargeTransferLossesTestLimitsData
    {
        [Key]
        public int LimitsID { get; set; }
        public string ECONumber { get; set; }
        public int SILossesMax { get; set; }
        public int SkimLossesMax { get; set; }
        public int ReadLossesMax { get; set; }
        public int ROI_Xs { get; set; }
        public int ROI_Ys { get; set; }
        public int ROI_dXs { get; set; }
        public int ROI_dYs { get; set; }
        public int ROI_dXbs { get; set; }
        public int ROI_dYbs { get; set; }
        public int ROI_PixRate { get; set; }
        public int ROI_NDRO { get; set; }
        public int Points { get; set; }
        public int MaxAuto_Flash { get; set; }
        public int Transfers { get; set; }
        public int AutoFlash_LED { get; set; }
        public int AutoFlash_Target { get; set; }
        public DateTime DefinedDate { get; set; }
        public string UserModified { get; set; }
    }
    [Table("CrossTalkLimits")]
    public class CrossTalkLimitsData
    {
        [Key]
        public int LimitsID { get; set; }
        public string ECONumber { get; set; }
        public double ColumnCoeffMax { get; set; }
        public double RowCoeffMax { get; set; }
        public int ColXTalkPositiveMax { get; set; }
        public int RowXTalkPositiveMax { get; set; }
        public int ROI_Xs { get; set; }
        public int ROI_Ys { get; set; }
        public int ROI_dXs { get; set; }
        public int ROI_dYs { get; set; }
        public int ROI_dXbs { get; set; }
        public int ROI_dYbs { get; set; }
        public int ROI_PixRate { get; set; }
        public int ROI_NDRO { get; set; }
        public int Points { get; set; }
        public int MaxAuto_Flash { get; set; }
        public int AutoFlash_LED { get; set; }
        public int AutoFlash_Target { get; set; }
        public DateTime DefinedDate { get; set; }
        public string UserModified { get; set; }
    }
    [Table("DarkCurrentLimits")]
    public class DarkCurrentTestLimitsData
    {
        [Key]
        public int LimitsID { get; set; }
        public string ECONumber { get; set; }
        public double DarkCurrentMaxDarkROI { get; set; }
        public double DarkCurrentLowerLimit { get; set; }
        public double DarkCurrentUpperLimit { get; set; }
        public double DarkCurrentMaxDarkROITol { get; set; }        
        public DateTime DefinedDate { get; set; }
        public string UserModified { get; set; }
    }
    [Table("DefectsLimits")]
    public class DefectsTestLimitsData
    {
        [Key]
        public int LimitsID { get; set; }
        public string ECONumber { get; set; }
        public double PRNUPositive { get; set; }
        public double PRNUNegative { get; set; }
        public int MaxTrap { get; set; }
        public int HotPixel { get; set; }
        public int DarkPixel { get; set; }
        public int DeadPixel { get; set; }
        public int AveTrap { get; set; }
        public int MaxDarkROI { get; set; }
        public int TotalNumDefects { get; set; }
        public int TotalNumClusters { get; set; }
        public int ClusterSize { get; set; }
        public int AdjacentPixelsInCluster { get; set; }
        public int TotalColumns { get; set; }
        public int TotalRows { get; set; }
        public int DriftROI { get; set; }
        public int DefectivePixelsPerROI { get; set; }

        public int XROIRange { get; set; }
        public int YROIRange { get; set; }
        public int DarkPixelThreshold { get; set; }
        public double PercentAboveMean { get; set; }
        public double PercentBelowMean { get; set; }
        public double PercentAboveRows { get; set; }
        public double PercentBelowRows { get; set; }
        public int ROI_Xs { get; set; }
        public int ROI_Ys { get; set; }
        public int ROI_dXs { get; set; }
        public int ROI_dYs { get; set; }
        public int ROI_dXbs { get; set; }
        public int ROI_dYbs { get; set; }
        public int ROI_PixRate { get; set; }
        public int ROI_NDRO { get; set; }
        public int PRNU_LED { get; set; }
        public int PRNU_Target { get; set; }
        public int Trap_LED { get; set; }
        public int Trap_Target { get; set; }
        public int HotPixel_LED { get; set; }
        public int HotPixel_Target { get; set; }
        public int Unused_outer_rows { get; set; }
        public int Unused_outer_cols { get; set; }
        public int PRNU_test_scan_size { get; set; }
        public int Hot_pix_exp_time { get; set; }
        public int Ave_dark_scan_size { get; set; }
        public int BadPixelsInCol { get; set; }
        public int BadPixelsInRow { get; set; }
        public int DeadDarkPixelsListReadLimit { get; set; }
        public DateTime DefinedDate { get; set; }
        public string UserModified { get; set; }
        public string TestStage { get; set; }
    }
    [Table("InjectionEfficiencyLimits")]
    public class InjectionEfficiencyTestLimitsData
    {
        [Key]
        public int LimitsID { get; set; }
        public string ECONumber { get; set; }
        public double InjectionEffLimit { get; set; }
        public double CrossTalkThresholdLimit { get; set; }
        public DateTime DefinedDate { get; set; }
        public string UserModified { get; set; }
    }
    [Table("InjectionPerformanceLimits")]
    public class InjectionPerformanceLimitsData
    {
        [Key]
        public int LimitsID { get; set; }
        public string ECONumber { get; set; }
        public int AveSigAfterInjectionMax { get; set; }
        public int StdDevOfAvesMax { get; set; }
        public int ROI_Xs { get; set; }
        public int ROI_Ys { get; set; }
        public int ROI_dXs { get; set; }
        public int ROI_dYs { get; set; }
        public int ROI_dXbs { get; set; }
        public int ROI_dYbs { get; set; }
        public int ROI_PixRate { get; set; }
        public int ROI_NDRO { get; set; }
        public int Points { get; set; }
        public int Auto_Flashe_LED { get; set; }
        public int AutoFlash_Target { get; set; }
        public DateTime DefinedDate { get; set; }
        public string UserModified { get; set; }
    }
    [Table("LinearityLimits")]
    public class LinearityTestLimitsData
    {
        [Key]
        public int LimitsID { get; set; }
        public string ECONumber { get; set; }
        public double LinearityLevelMax { get; set; }
        public int FullWellLinLevMin { get; set; }
        public int FullWellLinLevMax { get; set; }
        public int SatLevMin { get; set; }
        public int SatLevMax { get; set; }
        public DateTime DefinedDate { get; set; }
        public string UserModified { get; set; }
    }
    [Table("NdroReadDriftLimits")]
    public class NdroReadDriftTestLimitsData
    {
        [Key]
        public int LimitsID { get; set; }
        public string ECONumber { get; set; }
        public double NdroReadDriftMaxSlope { get; set; }
        public DateTime DefinedDate { get; set; }
        public string UserModified { get; set; }
    }
    [Table("PhotoresponseLimits")]
    public class PhotoresponseLimitsData
    {
        [Key]
        public int LimitsID { get; set; }
        public string ECONumber { get; set; }
        public double LinearityLevelMax { get; set; }
        public int FullWellLevelMin { get; set; }
        public int ROI_Xs { get; set; }
        public int ROI_Ys { get; set; }
        public int ROI_dXs { get; set; }
        public int ROI_dYs { get; set; }
        public int ROI_dXbs { get; set; }
        public int ROI_dYbs { get; set; }
        public int ROI_PixRate { get; set; }
        public int ROI_NDRO { get; set; }
        public int Exposures { get; set; }
        public int Flashes { get; set; }
        public int Time { get; set; }
        public int ExposureControl { get; set; }
        public DateTime DefinedDate { get; set; }
        public string UserModified { get; set; }
    }
    [Table("ReadNoiseVsNDROsLimits")]
    public class NoiseVsNDROSLimitsData
    {
        [Key]
        public int LimitsID { get; set; }
        public string ECONumber { get; set; }
        public int SnglNoiseMax { get; set; }
        public double PowerFitUpper { get; set; }
        public double PowerFitLower { get; set; }
        public double RPower2Correlation { get; set; }
        public int ROI_Xs { get; set; }
        public int ROI_Ys { get; set; }
        public int ROI_dXs { get; set; }
        public int ROI_dYs { get; set; }
        public int ROI_dXbs { get; set; }
        public int ROI_dYbs { get; set; }
        public int NDROs { get; set; }
        public int Trials { get; set; }
        public int Flashes { get; set; }
        public int Time { get; set; }
        public int ExposureControl { get; set; }
        public int ReadOutModeType { get; set; }
        public DateTime DefinedDate { get; set; }
        public string UserModified { get; set; }
        public double SnglNoiseUpperLimit { get; set; }
        public double SnglNoiseLowerLimit { get; set; }
        public double SnglNoiseTolerance { get; set; }
    }
    [Table("SegmentInjectLimits")]
    public class SegmentInjectLimitsData
    {
        [Key]
        public int LimitsID { get; set; }
        public string ECONumber { get; set; }
        public int InjectLevel_adu { get; set; }
        public int InjectDisturbance_adu { get; set; }
        public DateTime DefinedDate { get; set; }
        public string UserModified { get; set; }
    }
    [Table("TempHumidLimits")]
    public class TemperatureHumidityLimitsData
    {
        [Key]
        public int LimitsID { get; set; }
        public string ECONumber { get; set; }
        public double MinTemp { get; set; }
        public double MaxTemp { get; set; }
        public double MinHumidity { get; set; }
        public double MaxHumidity { get; set; }
        public DateTime DefinedDate { get; set; }
        public string UserModified { get; set; }
    }
    [Table("LEDCalibrationLimits")]
    public class LEDCalibrationLimitsData
    {
        [Key]
        public int LimitsID { get; set; }
        public string ECONumber { get; set; }
        public double LEDLimitOffset { get; set; }
        public double Red40LEDLimit { get; set; }
        public double Red60LEDLimit { get; set; }
        public double Green40LEDLimit { get; set; }
        public double Green60LEDLimit { get; set; }
        public double Blue40LEDLimit { get; set; }
        public double Blue60LEDLimit { get; set; }
        public int CurrentTestInterval { get; set; }
        public int PreviousTestInterval { get; set; }
        public DateTime DefinedDate { get; set; }
        public string UserModified { get; set; }
        public int UpperUVLEDLimit { get; set; }
        public int LowerUVLEDLimit { get; set; }
    }
    [Table("EnvironmentalStatusLimits")]
    public class EnvironmentalStatusLimitsData
    {
        [Key]
        public int EnvironmentalStatusLimitsID { get; set; }
        public string ECONumber { get; set; }
        public double VoltageUpperLimit { get; set; }
        public double VoltageLowerLimit { get; set; }
        public double AmpsUpperLimit { get; set; }
        public double AmpsLowerLimit { get; set; }
        public double PurgeFlowRateUpperLimit { get; set; }
        public double PurgeFlowRateLowerLimit { get; set; }
        public double CoolantTempUpperLimit { get; set; }
        public double CoolantTempLowerLimit { get; set; }
        public double TanskLevelLowLimit { get; set; }
        public int TCubeFaultStatusLimit { get; set; }
        public DateTime DefinedDate { get; set; }       
    }
    [Table("ECOData")]
    public class ECOData
    {
        [Key]
        public string ECONumber { get; set; }
        public DateTime DefinedDate { get; set; }
        public string UserModified { get; set; }
        public virtual ICollection<MeanVarianceTestLimitsData> MeanVarianceTestLimitsData { get; set; }
        public virtual ICollection<DarkCurrentTestLimitsData> DarkCurrentTestLimitsData { get; set; }
        public virtual ICollection<DefectsTestLimitsData> DefectsTestLimitsData { get; set; }
        public virtual ICollection<InjectionEfficiencyTestLimitsData> InjectionEfficiencyTestLimitsData { get; set; }
        public virtual ICollection<InjectionPerformanceLimitsData> InjectionPerformanceLimitsData { get; set; }
        public virtual ICollection<NdroReadDriftTestLimitsData> NdroReadDriftTestLimitsData { get; set; }
        public virtual ICollection<PhotoresponseLimitsData> PhotoresponseLimitsData { get; set; }
        public virtual ICollection<NoiseVsNDROSLimitsData> NoiseVsNDROSLimitsData { get; set; }
        public virtual ICollection<SegmentInjectLimitsData> SegmentInjectLimitsData { get; set; }
        public virtual ICollection<TemperatureHumidityLimitsData> TemperatureHumidityLimitsData { get; set; }
        public virtual ICollection<LEDCalibrationLimitsData> LEDCalibrationLimitsData { get; set; }
        public virtual ICollection<EnvironmentalStatusLimitsData> EnvironmentalStatusLimitsData { get; set; }
    }
}
