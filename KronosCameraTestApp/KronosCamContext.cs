using System.Data.Entity;
using KronosCameraTestApp.Model;
using System.Data.Entity.Infrastructure;
namespace KronosCameraTestApp
{
    public class KronosCamContext : DbContext
    {
        public KronosCamContext()
        {
            var adapter = (IObjectContextAdapter)this;
            var objectContext = adapter.ObjectContext;
            objectContext.CommandTimeout = 180;
        }
        public DbSet<UserLoginModel> userLoginModel { get; set; }
        public DbSet<MeanVarianceTestResultsData> meanVarianceTestResultsData { get; set; }
        public DbSet<DarkCurrentTestResults> darkCurrentTestResults { get; set; }
        public DbSet<DefectsTestResults> defectsTestResults { get; set; }
        public DbSet<NoiseVsNDROTestResults> noiseVsNDROTestResults { get; set; }
        public DbSet<InjectionEfficiencyTestResults> injectionEfficiencyTestResults { get; set; }
        public DbSet<PhotoresponseTestResults> photoresponseTestResults { get; set; }
        public DbSet<RedBlueTestResults> redBlueTestResults { get; set; }
        public DbSet<ShutterDriveTestResults> shutterDriveTestResults { get; set; }
        public DbSet<LEDCalibrationTestResults> ledCalibrationTestResults { get; set; }
        public DbSet<ExposureDataModel> exposureDataModel { get; set; }
        public DbSet<SubarrayDataModel> subarrayDataModel { get; set; }
        public DbSet<MeanVarianceTestLimitsData> meanVarianceTestLimitsData { get; set; }
        public DbSet<DarkCurrentTestLimitsData> darkCurrentTestLimitsData { get; set; }
        public DbSet<DefectsTestLimitsData> defectsTestLimitsData { get; set; }
        public DbSet<InjectionEfficiencyTestLimitsData> injectionEfficiencyTestLimitsData { get; set; }
        public DbSet<InjectionPerformanceLimitsData> injectionPerformanceLimitsData { get; set; }
        public DbSet<LinearityTestLimitsData> linearityTestLimitsData { get; set; }
        public DbSet<NdroReadDriftTestLimitsData> ndroReadDriftTestLimitsData { get; set; }
        public DbSet<PhotoresponseLimitsData> photoresponseLimitsData { get; set; }
        public DbSet<NoiseVsNDROSLimitsData> noiseVsNDROSLimitsData { get; set; }
        public DbSet<SegmentInjectLimitsData> segmentInjectLimitsData { get; set; }
        public DbSet<LEDCalibrationLimitsData> ledCalibrationLimitsData { get; set; }
        public DbSet<ECOData> ecoData { get; set; }
        public DbSet<OverAllTestResultsModel> overAllTestResultsModel { get; set; }
        public DbSet<FinalTestResultsDetailsModel> finalTestResultsDetailsModel { get; set; }
        public DbSet<CameraDetailsModel> cameraDetailsModel { get; set; }
        public DbSet<InstrumentDefaultsModel> instrumentDefaultsModel { get; set; }
        public DbSet<ParametersModel> parametersModel { get; set; }
        public DbSet<BurnInAnalysisModel> burnInAnalysisModel { get; set; }
        public DbSet<CameraTestLogsModel> cameraTestLogs { get; set; }
        public DbSet<CameraTestFailureModesMetricsModel> cameraTestFailureModesMetricsModel { get; set; }
        public DbSet<BurnInDMesgIgnoreModel> burnInDMesgIgnoreModel { get; set; }
        public DbSet<AffectedItemsModel> affectedItemsModel { get; set; }
        public DbSet<MaskROIsModel> maskROIsModel { get; set; }
        public DbSet<TestStationCalibrationModel> testStationCalibrationModel { get; set; }
        public DbSet<TemperatureHumidityLimitsData> temperatureHumidityLimitsData { get; set; }
        public DbSet<EnvironmentalStatusModel> environmentalStatusModel { get; set; }
        public DbSet<EnvironmentalStatusLimitsData> environmentalStatusLimitsData { get; set; }
        public DbSet<TempHumReadingModel> tempHumReadingModel { get; set; }
        public DbSet<COMPortConfigurationModel> comPortConfigurationModel { get; set; }
    }
}
