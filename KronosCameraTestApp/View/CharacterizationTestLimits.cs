using Infragistics.Win;
using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinGrid;
using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.Presenter;
using KronosCameraTestApp.View.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
namespace KronosCameraTestApp.View
{
    public partial class CharacterizationTestLimits : Form
    {
        MeanVarianceLimitsUserControl meanVarianceTestLimitsUserControl { get; set; }
        MeanVarianceLimitsPresenter meanVarianceTestLimitsPresenter { get; set; }
        MeanVarianceTestLimitsData mvLimit { get; set; }
        List<MeanVarianceTestLimitsData> meanVarianceTestLimitsDataList { get; set; }
        DarkCurrentLimitsUserControl darkCurrentTestLimitsUserControl { get; set; }
        DarkCurrentLimitsPresenter darkCurrentTestLimitsPresenter { get; set; }
        DarkCurrentTestLimitsData darkCurrentLimit { get; set; }
        List<DarkCurrentTestLimitsData> darkCurrentTestLimitsDataList { get; set; }
        DefectsLimitsUserControl defectsTestLimitsUserControl { get; set; }
        DefectsLimitsPresenter defectsTestLimitsPresenter { get; set; }
        DefectsTestLimitsData defectsLimit { get; set; }
        List<DefectsTestLimitsData> defectsTestLimitsDataList { get; set; }
        InjectionEfficiencyLimitsUserControl injectionEfficiencyTestLimitsUserControl { get; set; }
        InjectionEfficiencyLimitsPresenter injectionEfficiencyTestLimitsPresenter { get; set; }
        InjectionEfficiencyTestLimitsData injectionEffLimit { get; set; }
        List<InjectionEfficiencyTestLimitsData> injectionEfficiencyTestLimitsDataList { get; set; }
        InjectionPerformanceLimitsUserControl injectionPerformanceTestLimitsUserControl { get; set; }
        InjectionPerformanceLimitsPresenter injectionPerformanceTestLimitsPresenter { get; set; }
        InjectionPerformanceLimitsData injectionPerfLimit { get; set; }
        List<InjectionPerformanceLimitsData> injectionPerformanceLimitsDataList { get; set; }
        NDROReadDriftLimitsUserControl nDROReadDriftTestLimitsUserControl { get; set; }
        NDROReadDriftLimitsPresenter nDROReadDriftTestLimitsPresenter { get; set; }
        NdroReadDriftTestLimitsData ndroLimit { get; set; }
        List<NdroReadDriftTestLimitsData> ndroReadDriftTestLimitsDataList { get; set; }
        NoiseVsNDROSLimitsUserControl noiseVsNDROSLimitsUserControl { get; set; }
        NoiseVsNDROSLimitsPresenter noiseVsNDROSLimitsPresenter { get; set; }
        NoiseVsNDROSLimitsData noiseLimit { get; set; }
        List<NoiseVsNDROSLimitsData> noiseVsNDROSLimitsDataList { get; set; }
        PhotoresponseLimitsUserControl photoresponseLimitsUserControl { get; set; }
        PhotoresponseLimitsPresenter photoresponseLimitsPresenter { get; set; }
        PhotoresponseLimitsData photoresponseLimit { get; set; }
        List<PhotoresponseLimitsData> photoresponseLimitsDataList { get; set; }
        SegmentInjectLimitsUserControl segmentInjectLimitsUserControl { get; set; }
        SegmentInjectLimitsPresenter segmentInjectLimitsPresenter { get; set; }
        SegmentInjectLimitsData segmentInjectLimit { get; set; }
        List<SegmentInjectLimitsData> segmentInjectLimitsDataList { get; set; }
        TemperatureHumidityLimitsData tempLimit { get; set; }
        LEDCalibrationLimitsUserControl ledCalibrationLimitsUserControl { get; set; }
        List<LEDCalibrationLimitsData> ledCalibrationLimitsDataList { get; set; }
        LEDCalibrationLimitsData ledCalibrationLimitsData { get; set; }
        LEDCalibrationPresenter ledCalibrationPresenter { get; set; }
        TempHumidPresenter tempHumidPresenter { get; set; }
        List<ECOData> ecoDataList { get; set; }
       // delegate bool CheckLimitChanged<T>(string str);
        public List<ECOData> ECODataList
        {
            get { return ecoDataList; }
            set { ecoDataList = value; }
        }
        ECOData ecoData { get; set; }
        public ECOData EcoData
        {
            get { return ecoData; }
            set { ecoData = value; }
        }
        List<ECOData> sortedECOData { get; set; }
        string ECONumber = string.Empty;
        string userName = string.Empty;
        string userMode = string.Empty;
        bool engineeringLoginStatus = false;
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(
         System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        MeanVarianceTestLimitsData oldMeanVarianceTestLimitsData { get; set; }
        static int LimitsLoaded = 0;
        bool enggLimitUpdated = false;
        Dictionary<string, string> ecoNumbersDict;
        Tuple<string, string>[] ecoNumbersTuple;
        string ecoNumberSelected = string.Empty;
        public string ECONumberSelected
        {
            get { return ecoNumberSelected; }
            set { ecoNumberSelected = value; }
        }
        List<string> OmnifyECOList = new List<string>();
        bool isMeanVarianceLimitChanged = false;
        bool isDefectsLimitChanged = false;
        bool isDarkCurrentLimitChanged = false;
        bool isPhotoresponseLimitChanged = false;
        bool isNoiseVsNDROsLimitChanged = false;
        bool isInjectionEfficiencyLimitChanged = false;
        bool isLEDCalibrationLimitChanged = false;
        bool isSegmentInjectLimitChanged = false;
        bool isInjectionPerformanceLimitChanged = false;
        bool isNDROReadDriftLimitChanged = false;
        bool ecoDataChanged = false;
        public CharacterizationTestLimits()
        {
            InitializeComponent();
        }
        public CharacterizationTestLimits(MeanVarianceLimitsPresenter meanVarianceTestLimitsPresenter,
                                            DarkCurrentLimitsPresenter darkCurrentTestLimitsPresenter, DefectsLimitsPresenter defectsTestLimitsPresenter,
                                            InjectionEfficiencyLimitsPresenter injectionEfficiencyTestLimitsPresenter,
                                            InjectionPerformanceLimitsPresenter injectionPerformanceTestLimitsPresenter,
                                            NDROReadDriftLimitsPresenter nDROReadDriftTestLimitsPresenter, NoiseVsNDROSLimitsPresenter noiseVsNDROSLimitsPresenter,
                                            PhotoresponseLimitsPresenter photoresponseLimitsPresenter, SegmentInjectLimitsPresenter segmentInjectLimitsPresenter,
                                           LEDCalibrationPresenter ledCalibrationPresenter, TempHumidPresenter tempHumidPresenter)
        {
            InitializeComponent();
            this.meanVarianceTestLimitsPresenter = meanVarianceTestLimitsPresenter;
            this.darkCurrentTestLimitsPresenter = darkCurrentTestLimitsPresenter;
            this.defectsTestLimitsPresenter = defectsTestLimitsPresenter;
            this.injectionEfficiencyTestLimitsPresenter = injectionEfficiencyTestLimitsPresenter;
            this.injectionPerformanceTestLimitsPresenter = injectionPerformanceTestLimitsPresenter;
            this.nDROReadDriftTestLimitsPresenter = nDROReadDriftTestLimitsPresenter;
            this.noiseVsNDROSLimitsPresenter = noiseVsNDROSLimitsPresenter;
            this.photoresponseLimitsPresenter = photoresponseLimitsPresenter;
            this.segmentInjectLimitsPresenter = segmentInjectLimitsPresenter;
            this.ledCalibrationPresenter = ledCalibrationPresenter;
            this.tempHumidPresenter = tempHumidPresenter;
        }
        public CharacterizationTestLimits(MeanVarianceLimitsPresenter meanVarianceTestLimitsPresenter,
                                           DarkCurrentLimitsPresenter darkCurrentTestLimitsPresenter,
                                            DefectsLimitsPresenter defectsTestLimitsPresenter, InjectionEfficiencyLimitsPresenter injectionEfficiencyTestLimitsPresenter,
                                            InjectionPerformanceLimitsPresenter injectionPerformanceTestLimitsPresenter, NDROReadDriftLimitsPresenter nDROReadDriftTestLimitsPresenter,
                                            NoiseVsNDROSLimitsPresenter noiseVsNDROSLimitsPresenter, PhotoresponseLimitsPresenter photoresponseLimitsPresenter,
                                            SegmentInjectLimitsPresenter segmentInjectLimitsPresenter, LEDCalibrationPresenter ledCalibrationPresenter,
                                            TempHumidPresenter tempHumidPresenter,
                                            string UserMode, bool engineeringLoginStatus, string enggUserName)
        {
            InitializeComponent();
            this.meanVarianceTestLimitsPresenter = meanVarianceTestLimitsPresenter;
            this.darkCurrentTestLimitsPresenter = darkCurrentTestLimitsPresenter;
            this.defectsTestLimitsPresenter = defectsTestLimitsPresenter;
            this.injectionEfficiencyTestLimitsPresenter = injectionEfficiencyTestLimitsPresenter;
            this.injectionPerformanceTestLimitsPresenter = injectionPerformanceTestLimitsPresenter;
            this.nDROReadDriftTestLimitsPresenter = nDROReadDriftTestLimitsPresenter;
            this.noiseVsNDROSLimitsPresenter = noiseVsNDROSLimitsPresenter;
            this.photoresponseLimitsPresenter = photoresponseLimitsPresenter;
            this.segmentInjectLimitsPresenter = segmentInjectLimitsPresenter;
            this.ledCalibrationPresenter = ledCalibrationPresenter;
            this.tempHumidPresenter = tempHumidPresenter;
            this.userMode = UserMode;
            this.engineeringLoginStatus = engineeringLoginStatus;
            this.userName = enggUserName;
            meanVarianceTestLimitsUserControl = new MeanVarianceLimitsUserControl(meanVarianceTestLimitsPresenter, userMode, engineeringLoginStatus, ECONumber);
            darkCurrentTestLimitsUserControl = new DarkCurrentLimitsUserControl(darkCurrentTestLimitsPresenter, userMode, engineeringLoginStatus, ECONumber);
            defectsTestLimitsUserControl = new DefectsLimitsUserControl(defectsTestLimitsPresenter, userMode, engineeringLoginStatus, ECONumber);
            injectionEfficiencyTestLimitsUserControl = new InjectionEfficiencyLimitsUserControl(injectionEfficiencyTestLimitsPresenter, userMode, engineeringLoginStatus, ECONumber);
            injectionPerformanceTestLimitsUserControl = new InjectionPerformanceLimitsUserControl(injectionPerformanceTestLimitsPresenter, userMode, engineeringLoginStatus, ECONumber);
            nDROReadDriftTestLimitsUserControl = new NDROReadDriftLimitsUserControl(nDROReadDriftTestLimitsPresenter, userMode, engineeringLoginStatus, ECONumber);
            noiseVsNDROSLimitsUserControl = new NoiseVsNDROSLimitsUserControl(noiseVsNDROSLimitsPresenter, userMode, engineeringLoginStatus, ECONumber);
            photoresponseLimitsUserControl = new PhotoresponseLimitsUserControl(photoresponseLimitsPresenter, userMode, engineeringLoginStatus, ECONumber);
            segmentInjectLimitsUserControl = new SegmentInjectLimitsUserControl(segmentInjectLimitsPresenter, userMode, engineeringLoginStatus, ECONumber);
            ledCalibrationLimitsUserControl = new LEDCalibrationLimitsUserControl(ledCalibrationPresenter, userMode, engineeringLoginStatus, ECONumber);
            ecoDataList = new List<ECOData>();
            darkCurrentTestLimitsDataList = new List<DarkCurrentTestLimitsData>();
            segmentInjectLimitsDataList = new List<SegmentInjectLimitsData>();
            photoresponseLimitsDataList = new List<PhotoresponseLimitsData>();
            noiseVsNDROSLimitsDataList = new List<NoiseVsNDROSLimitsData>();
            ndroReadDriftTestLimitsDataList = new List<NdroReadDriftTestLimitsData>();
            injectionPerformanceLimitsDataList = new List<InjectionPerformanceLimitsData>();
            injectionEfficiencyTestLimitsDataList = new List<InjectionEfficiencyTestLimitsData>();
            defectsTestLimitsDataList = new List<DefectsTestLimitsData>();
            meanVarianceTestLimitsDataList = new List<MeanVarianceTestLimitsData>();
        }
        private void ultraExplorerBar1_ItemClick(object sender, Infragistics.Win.UltraWinExplorerBar.ItemEventArgs e)
        {
            try
            {
                TestLimitsPanel.Controls.Clear();
                switch (e.Item.Tag.ToString())
                {
                    case "MeanVariance":
                        ultraLabelTestHeader.Text = "Mean Variance Limit";
                        meanVarianceTestLimitsUserControl.Dock = DockStyle.Fill;
                        if(ecoDataChanged)
                        {
                            meanVarianceTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(ecoData.MeanVarianceTestLimitsData.ToList()[0]);
                        }
                        else
                        {
                            meanVarianceTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(meanVarianceTestLimitsUserControl.ReturnMeanVarianceLimitsModel());
                        }                       
                        TestLimitsPanel.Controls.Add(meanVarianceTestLimitsUserControl);
                        break;
                    case "DarkCurrent":
                        ultraLabelTestHeader.Text = "Dark Current Limit";
                        darkCurrentTestLimitsUserControl.Dock = DockStyle.Fill;
                        if (ecoDataChanged)
                        {
                            darkCurrentTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(ecoData.DarkCurrentTestLimitsData.ToList()[0]);
                        }
                        else
                        {
                            darkCurrentTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(darkCurrentTestLimitsUserControl.ReturnDarkCurrentLimits());
                        }                        
                        TestLimitsPanel.Controls.Add(darkCurrentTestLimitsUserControl);
                        break;
                    case "Defects":
                        ultraLabelTestHeader.Text = "Defects Limit";
                        defectsTestLimitsUserControl.Dock = DockStyle.Fill;
                        if (ecoDataChanged)
                        {
                            defectsTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(ecoData.DefectsTestLimitsData.ToList()[0]);
                        }
                        else
                        {
                            defectsTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(defectsTestLimitsUserControl.ReturnDefectsTestLimit());
                        }
                        
                        TestLimitsPanel.Controls.Add(defectsTestLimitsUserControl);
                        break;
                    case "InjectionEfficiency":
                        ultraLabelTestHeader.Text = "Injection Efficiency Limit";
                        injectionEfficiencyTestLimitsUserControl.Dock = DockStyle.Fill;
                        if (ecoDataChanged)
                        {
                            injectionEfficiencyTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(ecoData.InjectionEfficiencyTestLimitsData.ToList()[0]);
                        }
                        else
                        {
                            injectionEfficiencyTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(injectionEfficiencyTestLimitsUserControl.ReturnInjectionEfficiencyLimits());
                        }
                      
                        TestLimitsPanel.Controls.Add(injectionEfficiencyTestLimitsUserControl);
                        break;
                    case "InjectionPerformance":
                        ultraLabelTestHeader.Text = "Injection Performance Limit";
                        injectionPerformanceTestLimitsUserControl.Dock = DockStyle.Fill;
                        if (ecoDataChanged)
                        {
                            injectionPerformanceTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(ecoData.InjectionPerformanceLimitsData.ToList()[0]);
                        }
                        else
                        {
                            injectionPerformanceTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(injectionPerformanceTestLimitsUserControl.ReturnInjectionPerformanceLimit());
                        }
                        
                        TestLimitsPanel.Controls.Add(injectionPerformanceTestLimitsUserControl);
                        break;
                    case "NdroReadDrift":
                        ultraLabelTestHeader.Text = "NDRO Read Drift Limit";
                        nDROReadDriftTestLimitsUserControl.Dock = DockStyle.Fill;
                        if (ecoDataChanged)
                        {
                            nDROReadDriftTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(ecoData.NdroReadDriftTestLimitsData.ToList()[0]);
                        }
                        else
                        {
                            nDROReadDriftTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(nDROReadDriftTestLimitsUserControl.ReturnNdroReadDriftLimit());
                        }
                       
                        TestLimitsPanel.Controls.Add(nDROReadDriftTestLimitsUserControl);
                        break;
                    case "ReadNoiseVsNDROs":
                        ultraLabelTestHeader.Text = "Read Noise Vs NDROs Limit";
                        noiseVsNDROSLimitsUserControl.Dock = DockStyle.Fill;
                        if (ecoDataChanged)
                        {
                            noiseVsNDROSLimitsUserControl.UpdateLimitsViewWithSpecificModel(ecoData.NoiseVsNDROSLimitsData.ToList()[0]);
                        }
                        else
                        {
                            noiseVsNDROSLimitsUserControl.UpdateLimitsViewWithSpecificModel(noiseVsNDROSLimitsUserControl.ReturnNoiseVsNDROsLimit());
                        }
                       
                        TestLimitsPanel.Controls.Add(noiseVsNDROSLimitsUserControl);
                        break;
                    case "Photoresponse":
                        ultraLabelTestHeader.Text = "Photoresponse Limit";
                        photoresponseLimitsUserControl.Dock = DockStyle.Fill;
                        if (ecoDataChanged)
                        {
                            photoresponseLimitsUserControl.UpdateLimitsViewWithSpecificModel(ecoData.PhotoresponseLimitsData.ToList()[0]);
                        }
                        else
                        {
                            photoresponseLimitsUserControl.UpdateLimitsViewWithSpecificModel(photoresponseLimitsUserControl.ReturPhotoresponseLimit());
                        }
                       
                        TestLimitsPanel.Controls.Add(photoresponseLimitsUserControl);
                        break;
                    case "SegmentInject":
                        ultraLabelTestHeader.Text = "Segment Inject Limit";
                        segmentInjectLimitsUserControl.Dock = DockStyle.Fill;
                        if (ecoDataChanged)
                        {
                            segmentInjectLimitsUserControl.UpdateLimitsViewWithSpecificModel(ecoData.SegmentInjectLimitsData.ToList()[0]);
                        }
                        else
                        {
                            segmentInjectLimitsUserControl.UpdateLimitsViewWithSpecificModel(segmentInjectLimitsUserControl.ReturnSegmentInjectLimit());
                        }
                        
                        TestLimitsPanel.Controls.Add(segmentInjectLimitsUserControl);
                        break;                  
                    case "ViewAll":
                        break;
                    case "OverallLimits":
                        ultraLabelTestHeader.Text = string.Empty;
                        ultraGridLimits.Visible = true;
                        ultraGridLimits.DataSource = sortedECOData;
                        TestLimitsPanel.Controls.Add(ultraGridLimits);
                        break;
                    case "LEDCalibrationLimits":
                        ultraLabelTestHeader.Text = "LED Calibration Limit";
                        ledCalibrationLimitsUserControl.Dock = DockStyle.Fill;
                        if (ecoDataChanged)
                        {
                            ledCalibrationLimitsUserControl.UpdateLimitsViewWithSpecificModel(ecoData.LEDCalibrationLimitsData.ToList()[0]);
                        }
                        else
                        {
                            ledCalibrationLimitsUserControl.UpdateLimitsViewWithSpecificModel(ledCalibrationLimitsUserControl.ReturnLEDCalibrationLimit());
                        }
                        TestLimitsPanel.Controls.Add(ledCalibrationLimitsUserControl);
                        break;
                }
                ecoDataChanged = false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                ToolStripItem item = e.ClickedItem;
                if (item.Name.Equals("ExportGridToExcel"))
                {
                    ExportDataGridToExcel();
                }
                if (item.Name.Equals("PrintDataGrid"))
                {
                    PrintGrid();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraGridExcelExporter1_BeginExport(object sender, Infragistics.Win.UltraWinGrid.ExcelExport.BeginExportEventArgs e)
        {
            try
            {
                e.CurrentWorksheet.Name = "Test Results Grid Data";
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraGrid1_InitializePrintPreview(object sender, Infragistics.Win.UltraWinGrid.CancelablePrintPreviewEventArgs e)
        {
            e.DefaultLogicalPageLayoutInfo.FitWidthToPages = 1;
            e.DefaultLogicalPageLayoutInfo.PageHeader = ultraGridLimits.Text;
            e.DefaultLogicalPageLayoutInfo.PageHeaderAppearance.TextHAlign = HAlign.Center;
            e.DefaultLogicalPageLayoutInfo.PageHeaderBorderStyle = UIElementBorderStyle.Solid;
            e.DefaultLogicalPageLayoutInfo.PageFooter = "Page <#>.";
            e.DefaultLogicalPageLayoutInfo.PageFooterAppearance.TextHAlign = HAlign.Right;
            e.DefaultLogicalPageLayoutInfo.PageFooterBorderStyle = UIElementBorderStyle.Solid;
            e.DefaultLogicalPageLayoutInfo.ClippingOverride = ClippingOverride.Yes;
            e.PrintDocument.DefaultPageSettings.Landscape = true;
        }
        private void PrintGrid()
        {
            try
            {
                this.ultraGridLimits.PrintPreview(ultraGridLimits.DisplayLayout);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ExportDataGridToExcel()
        {
            try
            {
                testLimitsUSaveFileDialog.InitialDirectory = Properties.Settings.Default["FilesLocation"].ToString();
                string datetimestamp = DateTime.Now.ToString("MMddyyyy_hhmm");
                testLimitsUSaveFileDialog.Title = "Save Test Limits DataGrid to Excel";
                testLimitsUSaveFileDialog.DefaultExt = "*.xlsx";
                testLimitsUSaveFileDialog.Filter = "Excel|*.xlsx";
                testLimitsUSaveFileDialog.FileName = ultraGridLimits.Text + "_" + datetimestamp;
                if (testLimitsUSaveFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK && testLimitsUSaveFileDialog.FileName.Length > 0)
                {
                    this.testLimitsUltraGridExcelExporter.Export(this.ultraGridLimits, testLimitsUSaveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void CharacterizationTestLimits_Load(object sender, EventArgs e)
        {
            try
            {
                if (userMode.Equals("Engineering") && enggLimitUpdated)
                    DisplayOldLimitsOnLoad();
                else
                    GetEcoData();                             
                if (userMode.Equals("Admin") || (userMode.Equals("Engineering")))
                    btnApplyLimit.Visible = true;
                if (userMode.Equals("Admin"))
                    ultraGroupBoxAdminECO.Visible = true;
                if (userMode.Equals("Engineering") || userMode.Equals("Manufacturing"))
                {
                    meanVarianceTestLimitsUserControl.Enabled= darkCurrentTestLimitsUserControl.Enabled= defectsTestLimitsUserControl.Enabled=
                    injectionEfficiencyTestLimitsUserControl.Enabled = injectionPerformanceTestLimitsUserControl.Enabled = nDROReadDriftTestLimitsUserControl.Enabled =
                    noiseVsNDROSLimitsUserControl.Enabled = photoresponseLimitsUserControl.Enabled = segmentInjectLimitsUserControl.Enabled
                    = ledCalibrationLimitsUserControl.Enabled= false;

                }
                    
                ultraGridLimits.Visible = false;
                ultraLabelTestHeader.Text = "Mean Variance Limit";
                meanVarianceTestLimitsUserControl.Dock = DockStyle.Fill;
                meanVarianceTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(ecoData.MeanVarianceTestLimitsData.ToList()[0]);
                TestLimitsPanel.Controls.Add(meanVarianceTestLimitsUserControl);
                AddLimitsData.MouseEnter += buttonMouseEnter;
                RemoveLimitsData.MouseEnter += buttonMouseEnter;
                RefreshLimitsData.MouseEnter += buttonMouseEnter;
                UpdateCharacterizationLimit.MouseEnter += buttonMouseEnter;
                RefreshCharacterizationLimit.MouseEnter += buttonMouseEnter;
                AddLimitsData.MouseLeave += buttonMouseLeave;
                RemoveLimitsData.MouseLeave += buttonMouseLeave;
                RefreshLimitsData.MouseLeave += buttonMouseLeave;
                UpdateCharacterizationLimit.MouseLeave += buttonMouseLeave;
                RefreshCharacterizationLimit.MouseLeave += buttonMouseLeave;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void buttonMouseEnter(object sender, EventArgs e)
        {
            UltraButton button = sender as UltraButton;
            button.Size = new Size(48, 48);
            button.ImageSize = new Size(40, 40);
        }
        private void buttonMouseLeave(object sender, EventArgs e)
        {
            UltraButton button = sender as UltraButton;
            button.Size = new Size(40, 40);
            button.ImageSize = new Size(32, 32);
        }
        private void GetEcoData()
        {
            SqlConnection conn = null;
            try
            {
                sortedECOData = new List<ECOData>();
                if (TestAppHelper.CheckDBExists())
                {
                    if (userMode.Equals("Admin"))
                    {
                        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["KronosCamContext"].ToString());
                        conn.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT distinct(Number), Description FROM view_ECO_List;", conn);
                        DataSet ecs = new DataSet();
                        adapter.Fill(ecs, "ecs");
                        DataView dvExposure;
                        dvExposure = ecs.Tables[0].DefaultView;
                        ecoNumbersDict = new Dictionary<string, string>();
                        foreach (DataRowView dr in dvExposure)
                        {
                            OmnifyECOList.Add((dr[0].ToString()));
                            ecoNumbersDict.Add(dr[0].ToString(), dr[1].ToString());
                        }
                        conn.Close();
                    }
                    using (var kcc = new KronosCamContext())
                    {
                        ecoDataList = kcc.ecoData
                            .Include("MeanVarianceTestLimitsData")
                            .Include("DarkCurrentTestLimitsData")
                            .Include("DefectsTestLimitsData")
                            .Include("InjectionEfficiencyTestLimitsData")
                            .Include("InjectionPerformanceLimitsData")
                            .Include("NdroReadDriftTestLimitsData")
                            .Include("PhotoresponseLimitsData")
                            .Include("NoiseVsNDROSLimitsData")
                            .Include("SegmentInjectLimitsData")
                            .Include("LEDCalibrationLimitsData")
                            .Include("TemperatureHumidityLimitsData")
                            .ToList();
                    }
                }
                else
                {
                    if (userMode.Equals("Admin"))
                    {
                       // ProcessOmnifyECOViewXMLFile();
                    }
                    //ProcessLimitsXMLFiles();
                }
                ultraComboOmnifyECOs.DataSource = OmnifyECOList;
                sortedECOData = (from ecos in ecoDataList orderby ecos.DefinedDate descending select ecos).ToList();
                List<string> ecoList = new List<string>();
                ecoList = (from ecos in sortedECOData select ecos.ECONumber).ToList();
                ultraComboEditorECODetails.DataSource = ecoList;
                DisplayLimitsOnLoad();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                conn.Close();
            }
        }
        private void ProcessLimitsXMLFiles()
        {
            try
            {
                ProcessECODataXMLFile();
                ProcessDarkCurrentLimitsXMLFile();
                ProcessDefectsTestLimitsXMLFile();
                ProcessInjectionEfficiencyLimitsXMLFile();
                ProcessInjectionPerformanceLimitsXMLFile();
                ProcessMeanVarianceLimitsXMLFile();
                ProcessNdroReadDriftLimitsXMLFile();
                ProcessNoiseVsNDROSLimitsXMLFile();
                ProcessPhotoresponseLimitsXMLFile();
                ProcessSegmentInjectLimitsXMLFile();
                foreach (ECOData ecodata in ecoDataList)
                {
                    ecodata.DarkCurrentTestLimitsData = new List<DarkCurrentTestLimitsData>();
                    foreach (DarkCurrentTestLimitsData obj in darkCurrentTestLimitsDataList)
                    {
                        if (ecodata.ECONumber == obj.ECONumber)
                        {
                            ecodata.DarkCurrentTestLimitsData.Add(obj);
                        }
                    }
                    ecodata.DefectsTestLimitsData = new List<DefectsTestLimitsData>();
                    foreach (DefectsTestLimitsData obj in defectsTestLimitsDataList)
                    {
                        if (ecodata.ECONumber == obj.ECONumber)
                        {
                            ecodata.DefectsTestLimitsData.Add(obj);
                        }
                    }
                    ecodata.InjectionEfficiencyTestLimitsData = new List<InjectionEfficiencyTestLimitsData>();
                    foreach (InjectionEfficiencyTestLimitsData obj in injectionEfficiencyTestLimitsDataList)
                    {
                        if (ecodata.ECONumber == obj.ECONumber)
                        {
                            ecodata.InjectionEfficiencyTestLimitsData.Add(obj);
                        }
                    }
                    ecodata.InjectionPerformanceLimitsData = new List<InjectionPerformanceLimitsData>();
                    foreach (InjectionPerformanceLimitsData obj in injectionPerformanceLimitsDataList)
                    {
                        if (ecodata.ECONumber == obj.ECONumber)
                        {
                            ecodata.InjectionPerformanceLimitsData.Add(obj);
                        }
                    }
                    ecodata.MeanVarianceTestLimitsData = new List<MeanVarianceTestLimitsData>();
                    foreach (MeanVarianceTestLimitsData obj in meanVarianceTestLimitsDataList)
                    {
                        if (ecodata.ECONumber == obj.ECONumber)
                        {
                            ecodata.MeanVarianceTestLimitsData.Add(obj);
                        }
                    }
                    ecodata.NoiseVsNDROSLimitsData = new List<NoiseVsNDROSLimitsData>();
                    foreach (NoiseVsNDROSLimitsData obj in noiseVsNDROSLimitsDataList)
                    {
                        if (ecodata.ECONumber == obj.ECONumber)
                        {
                            ecodata.NoiseVsNDROSLimitsData.Add(obj);
                        }
                    }
                    ecodata.NdroReadDriftTestLimitsData = new List<NdroReadDriftTestLimitsData>();
                    foreach (NdroReadDriftTestLimitsData obj in ndroReadDriftTestLimitsDataList)
                    {
                        if (ecodata.ECONumber == obj.ECONumber)
                        {
                            ecodata.NdroReadDriftTestLimitsData.Add(obj);
                        }
                    }
                    ecodata.PhotoresponseLimitsData = new List<PhotoresponseLimitsData>();
                    foreach (PhotoresponseLimitsData obj in photoresponseLimitsDataList)
                    {
                        if (ecodata.ECONumber == obj.ECONumber)
                        {
                            ecodata.PhotoresponseLimitsData.Add(obj);
                        }
                    }
                    ecodata.SegmentInjectLimitsData = new List<SegmentInjectLimitsData>();
                    foreach (SegmentInjectLimitsData obj in segmentInjectLimitsDataList)
                    {
                        if (ecodata.ECONumber == obj.ECONumber)
                        {
                            ecodata.SegmentInjectLimitsData.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ProcessOmnifyECOViewXMLFile()
        {
            try
            {
                log.Info("Begin Reading Omnify ECOView XML.");
                DataSet ds = new DataSet();
                ds.ReadXml(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "ECOOmnifyViewData.xml", XmlReadMode.InferSchema);
                DataView dvExposure;
                dvExposure = ds.Tables[0].DefaultView;
                OmnifyECOList = new List<string>();
                ecoNumbersDict = new Dictionary<string, string>();
                foreach (DataRowView dr in dvExposure)
                {
                    OmnifyECOList.Add((dr[0].ToString()));
                    ecoNumbersDict.Add(dr[0].ToString(), dr[1].ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ProcessECODataXMLFile()
        {
            try
            {
                log.Info("Begin Reading ECOData XML.");
                DataSet ds = new DataSet();
                ds.ReadXml(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "ECOData.xml", XmlReadMode.InferSchema);
                DataView dvExposure;
                dvExposure = ds.Tables[0].DefaultView;
                ecoDataList = new List<ECOData>();
                foreach (DataRowView dr in dvExposure)
                {
                    ECOData ecoData = new ECOData();
                    ecoData.ECONumber = Convert.ToString(dr[0]);
                    ecoData.DefinedDate = Convert.ToDateTime(dr[1]);
                    ecoData.UserModified = Convert.ToString(dr[2]);
                    ecoDataList.Add(ecoData);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ProcessDarkCurrentLimitsXMLFile()
        {
            try
            {
                log.Info("Begin Reading DarkCurrentLimitsData XML.");
                DataSet ds = new DataSet();
                ds.ReadXml(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "DarkCurrentLimitsData.xml", XmlReadMode.InferSchema);
                DataView dvExposure;
                dvExposure = ds.Tables[0].DefaultView;
                darkCurrentTestLimitsDataList = new List<DarkCurrentTestLimitsData>();
                foreach (DataRowView dr in dvExposure)
                {
                    DarkCurrentTestLimitsData darkCurrentTestLimitsData = new DarkCurrentTestLimitsData();
                    darkCurrentTestLimitsData.LimitsID = Convert.ToInt32(dr[0]);
                    darkCurrentTestLimitsData.ECONumber = Convert.ToString(dr[1]);
                    darkCurrentTestLimitsData.DarkCurrentMaxDarkROI = Convert.ToDouble(dr[2]);
                    darkCurrentTestLimitsData.DarkCurrentLowerLimit = Convert.ToDouble(dr[3]);
                    darkCurrentTestLimitsData.DarkCurrentUpperLimit = Convert.ToDouble(dr[4]);
                    darkCurrentTestLimitsData.DarkCurrentMaxDarkROITol = Convert.ToDouble(dr[5]);
                    darkCurrentTestLimitsData.DefinedDate = Convert.ToDateTime(dr[6]);
                    darkCurrentTestLimitsData.UserModified = Convert.ToString(dr[7]);
                    darkCurrentTestLimitsDataList.Add(darkCurrentTestLimitsData);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ProcessDefectsTestLimitsXMLFile()
        {
            try
            {
                log.Info("Begin Reading DefectsTestLimitsData XML.");
                DataSet ds = new DataSet();
                ds.ReadXml(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "DefectsTestLimitsData.xml", XmlReadMode.InferSchema);
                DataView dvExposure;
                dvExposure = ds.Tables[0].DefaultView;
                defectsTestLimitsDataList = new List<DefectsTestLimitsData>();
                foreach (DataRowView dr in dvExposure)
                {
                    DefectsTestLimitsData defectsTestLimitsData = new DefectsTestLimitsData();
                    defectsTestLimitsData.LimitsID = Convert.ToInt32(dr[0]);
                    defectsTestLimitsData.ECONumber = Convert.ToString(dr[1]);
                    defectsTestLimitsData.PRNUPositive = Convert.ToDouble(dr[2]);
                    defectsTestLimitsData.PRNUNegative = Convert.ToDouble(dr[3]);
                    defectsTestLimitsData.MaxTrap = Convert.ToInt32(dr[4]);
                    defectsTestLimitsData.HotPixel = Convert.ToInt32(dr[5]);
                    defectsTestLimitsData.AveTrap = Convert.ToInt32(dr[6]);
                    defectsTestLimitsData.MaxDarkROI = Convert.ToInt32(dr[7]);
                    defectsTestLimitsData.TotalNumDefects = Convert.ToInt32(dr[8]);
                    defectsTestLimitsData.TotalNumClusters = Convert.ToInt32(dr[9]);
                    defectsTestLimitsData.TotalColumns = Convert.ToInt32(dr[10]);
                    defectsTestLimitsData.TotalRows = Convert.ToInt32(dr[11]);
                    defectsTestLimitsData.ROI_Xs = Convert.ToInt32(dr[12]);
                    defectsTestLimitsData.ROI_Ys = Convert.ToInt32(dr[13]);
                    defectsTestLimitsData.ROI_dXs = Convert.ToInt32(dr[14]);
                    defectsTestLimitsData.ROI_dYs = Convert.ToInt32(dr[15]);
                    defectsTestLimitsData.ROI_dXbs = Convert.ToInt32(dr[16]);
                    defectsTestLimitsData.ROI_dYbs = Convert.ToInt32(dr[17]);
                    defectsTestLimitsData.ROI_PixRate = Convert.ToInt32(dr[18]);
                    defectsTestLimitsData.ROI_NDRO = Convert.ToInt32(dr[19]);
                    defectsTestLimitsData.PRNU_LED = Convert.ToInt32(dr[20]);
                    defectsTestLimitsData.PRNU_Target = Convert.ToInt32(dr[21]);
                    defectsTestLimitsData.Trap_LED = Convert.ToInt32(dr[22]);
                    defectsTestLimitsData.Trap_Target = Convert.ToInt32(dr[23]);
                    defectsTestLimitsData.HotPixel_LED = Convert.ToInt32(dr[24]);
                    defectsTestLimitsData.HotPixel_Target = Convert.ToInt32(dr[25]);
                    defectsTestLimitsData.Unused_outer_rows = Convert.ToInt32(dr[26]);
                    defectsTestLimitsData.Unused_outer_cols = Convert.ToInt32(dr[27]);
                    defectsTestLimitsData.PRNU_test_scan_size = Convert.ToInt32(dr[28]);
                    defectsTestLimitsData.Hot_pix_exp_time = Convert.ToInt32(dr[29]);
                    defectsTestLimitsData.Ave_dark_scan_size = Convert.ToInt32(dr[30]);
                    defectsTestLimitsData.DefinedDate = Convert.ToDateTime(dr[31]);
                    defectsTestLimitsData.UserModified = Convert.ToString(dr[32]);
                    defectsTestLimitsData.ClusterSize = Convert.ToInt32(dr[33]);
                    defectsTestLimitsData.AdjacentPixelsInCluster = Convert.ToInt32(dr[34]);
                    defectsTestLimitsData.DarkPixel = Convert.ToInt32(dr[35]);
                    defectsTestLimitsData.DeadPixel = Convert.ToInt32(dr[36]);
                    defectsTestLimitsData.DriftROI = Convert.ToInt32(dr[37]);
                    defectsTestLimitsDataList.Add(defectsTestLimitsData);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ProcessInjectionEfficiencyLimitsXMLFile()
        {
            try
            {
                log.Info("Begin Reading InjectionEfficiencyLimitsData XML.");
                DataSet ds = new DataSet();
                ds.ReadXml(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "InjectionEfficiencyTestLimitsData.xml", XmlReadMode.InferSchema);
                DataView dvExposure;
                dvExposure = ds.Tables[0].DefaultView;
                injectionEfficiencyTestLimitsDataList = new List<InjectionEfficiencyTestLimitsData>();
                foreach (DataRowView dr in dvExposure)
                {
                    InjectionEfficiencyTestLimitsData injectionEfficiencyTestLimitsData = new InjectionEfficiencyTestLimitsData();
                    injectionEfficiencyTestLimitsData.LimitsID = Convert.ToInt32(dr[0]);
                    injectionEfficiencyTestLimitsData.ECONumber = Convert.ToString(dr[1]);
                    injectionEfficiencyTestLimitsData.InjectionEffLimit = Convert.ToDouble(dr[2]);
                    injectionEfficiencyTestLimitsData.DefinedDate = Convert.ToDateTime(dr[3]);
                    injectionEfficiencyTestLimitsData.UserModified = Convert.ToString(dr[4]);
                    injectionEfficiencyTestLimitsDataList.Add(injectionEfficiencyTestLimitsData);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ProcessInjectionPerformanceLimitsXMLFile()
        {
            try
            {
                log.Info("Begin Reading InjectionPerformanceLimitsData XML.");
                DataSet ds = new DataSet();
                ds.ReadXml(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "InjectionPerformanceTestLimitsData.xml", XmlReadMode.InferSchema);
                DataView dvExposure;
                dvExposure = ds.Tables[0].DefaultView;
                injectionPerformanceLimitsDataList = new List<InjectionPerformanceLimitsData>();
                foreach (DataRowView dr in dvExposure)
                {
                    InjectionPerformanceLimitsData injectionPerformanceLimitsData = new InjectionPerformanceLimitsData();
                    injectionPerformanceLimitsData.LimitsID = Convert.ToInt32(dr[0]);
                    injectionPerformanceLimitsData.ECONumber = Convert.ToString(dr[1]);
                    injectionPerformanceLimitsData.AveSigAfterInjectionMax = Convert.ToInt32(dr[2]);
                    injectionPerformanceLimitsData.StdDevOfAvesMax = Convert.ToInt32(dr[3]);
                    injectionPerformanceLimitsData.ROI_Xs = Convert.ToInt32(dr[4]);
                    injectionPerformanceLimitsData.ROI_Ys = Convert.ToInt32(dr[5]);
                    injectionPerformanceLimitsData.ROI_dXs = Convert.ToInt32(dr[6]);
                    injectionPerformanceLimitsData.ROI_dYs = Convert.ToInt32(dr[7]);
                    injectionPerformanceLimitsData.ROI_dXbs = Convert.ToInt32(dr[8]);
                    injectionPerformanceLimitsData.ROI_dYbs = Convert.ToInt32(dr[9]);
                    injectionPerformanceLimitsData.ROI_PixRate = Convert.ToInt32(dr[10]);
                    injectionPerformanceLimitsData.ROI_NDRO = Convert.ToInt32(dr[11]);
                    injectionPerformanceLimitsData.Points = Convert.ToInt32(dr[12]);
                    injectionPerformanceLimitsData.Auto_Flashe_LED = Convert.ToInt32(dr[13]);
                    injectionPerformanceLimitsData.AutoFlash_Target = Convert.ToInt32(dr[14]);
                    injectionPerformanceLimitsData.DefinedDate = Convert.ToDateTime(dr[15]);
                    injectionPerformanceLimitsData.UserModified = Convert.ToString(dr[16]);
                    injectionPerformanceLimitsDataList.Add(injectionPerformanceLimitsData);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ProcessMeanVarianceLimitsXMLFile()
        {
            try
            {
                log.Info("Begin Reading MeanVarianceLimitsData XML.");
                DataSet ds = new DataSet();
                ds.ReadXml(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "MeanVarianceLimitsData.xml", XmlReadMode.InferSchema);
                DataView dvExposure;
                dvExposure = ds.Tables[0].DefaultView;
                meanVarianceTestLimitsDataList = new List<MeanVarianceTestLimitsData>();
                foreach (DataRowView dr in dvExposure)
                {
                    MeanVarianceTestLimitsData meanVarianceTestLimitsData = new MeanVarianceTestLimitsData();
                    meanVarianceTestLimitsData.LimitsID = Convert.ToInt32(dr[0]);
                    meanVarianceTestLimitsData.ECONumber = Convert.ToString(dr[1]);
                    meanVarianceTestLimitsData.ConversionFactorNominal = Convert.ToDouble(dr[2]);
                    meanVarianceTestLimitsData.ConverisonFactorTol = Convert.ToInt32(dr[3]);
                    meanVarianceTestLimitsData.ROI_Xs = Convert.ToInt32(dr[4]);
                    meanVarianceTestLimitsData.ROI_Ys = Convert.ToInt32(dr[5]);
                    meanVarianceTestLimitsData.ROI_dXs = Convert.ToInt32(dr[6]);
                    meanVarianceTestLimitsData.ROI_dYs = Convert.ToInt32(dr[7]);
                    meanVarianceTestLimitsData.ROI_dXbs = Convert.ToInt32(dr[8]);
                    meanVarianceTestLimitsData.ROI_dYbs = Convert.ToInt32(dr[9]);
                    meanVarianceTestLimitsData.ROI_PixRate = Convert.ToInt32(dr[10]);
                    meanVarianceTestLimitsData.ROI_NDRO = Convert.ToInt32(dr[11]);
                    meanVarianceTestLimitsData.Exposures = Convert.ToInt32(dr[12]);
                    meanVarianceTestLimitsData.Trials = Convert.ToInt32(dr[13]);
                    meanVarianceTestLimitsData.LowerPoint = Convert.ToInt32(dr[14]);
                    meanVarianceTestLimitsData.UpperPoint = Convert.ToInt32(dr[15]);
                    meanVarianceTestLimitsData.DefinedDate = Convert.ToDateTime(dr[16]);
                    meanVarianceTestLimitsData.UserModified = Convert.ToString(dr[17]);
                    meanVarianceTestLimitsData.LED = Convert.ToInt32(dr[18]);
                    meanVarianceTestLimitsData.ConversionFactorLowerLimit = Convert.ToDouble(dr[19]);
                    meanVarianceTestLimitsData.ConversionFactorUpperLimit = Convert.ToDouble(dr[20]);
                    meanVarianceTestLimitsDataList.Add(meanVarianceTestLimitsData);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ProcessNdroReadDriftLimitsXMLFile()
        {
            try
            {
                log.Info("Begin Reading NdroReadDriftLimitsData XML.");
                DataSet ds = new DataSet();
                ds.ReadXml(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "NdroReadDriftTestLimitsData.xml", XmlReadMode.InferSchema);
                DataView dvExposure;
                dvExposure = ds.Tables[0].DefaultView;
                ndroReadDriftTestLimitsDataList = new List<NdroReadDriftTestLimitsData>();
                foreach (DataRowView dr in dvExposure)
                {
                    NdroReadDriftTestLimitsData ndroReadDriftTestLimitsData = new NdroReadDriftTestLimitsData();
                    ndroReadDriftTestLimitsData.LimitsID = Convert.ToInt32(dr[0]);
                    ndroReadDriftTestLimitsData.ECONumber = Convert.ToString(dr[1]);
                    ndroReadDriftTestLimitsData.NdroReadDriftMaxSlope = Convert.ToDouble(dr[2]);
                    ndroReadDriftTestLimitsData.DefinedDate = Convert.ToDateTime(dr[3]);
                    ndroReadDriftTestLimitsData.UserModified = Convert.ToString(dr[4]);
                    ndroReadDriftTestLimitsDataList.Add(ndroReadDriftTestLimitsData);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ProcessNoiseVsNDROSLimitsXMLFile()
        {
            try
            {
                log.Info("Begin Reading NoiseVsNDROSLimitsData XML.");
                DataSet ds = new DataSet();
                ds.ReadXml(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "NoiseVsNDROsTestLimitsData.xml", XmlReadMode.InferSchema);
                DataView dvExposure;
                dvExposure = ds.Tables[0].DefaultView;
                noiseVsNDROSLimitsDataList = new List<NoiseVsNDROSLimitsData>();
                foreach (DataRowView dr in dvExposure)
                {
                    NoiseVsNDROSLimitsData noiseVsNDROSLimitsData = new NoiseVsNDROSLimitsData();
                    noiseVsNDROSLimitsData.LimitsID = Convert.ToInt32(dr[0]);
                    noiseVsNDROSLimitsData.ECONumber = Convert.ToString(dr[1]);
                    noiseVsNDROSLimitsData.SnglNoiseMax = Convert.ToInt32(dr[2]);
                    noiseVsNDROSLimitsData.PowerFitUpper = Convert.ToDouble(dr[3]);
                    noiseVsNDROSLimitsData.PowerFitLower = Convert.ToDouble(dr[4]);
                    noiseVsNDROSLimitsData.RPower2Correlation = Convert.ToDouble(dr[5]);
                    noiseVsNDROSLimitsData.ROI_Xs = Convert.ToInt32(dr[6]);
                    noiseVsNDROSLimitsData.ROI_Ys = Convert.ToInt32(dr[7]);
                    noiseVsNDROSLimitsData.ROI_dXs = Convert.ToInt32(dr[8]);
                    noiseVsNDROSLimitsData.ROI_dYs = Convert.ToInt32(dr[9]);
                    noiseVsNDROSLimitsData.ROI_dXbs = Convert.ToInt32(dr[10]);
                    noiseVsNDROSLimitsData.ROI_dYbs = Convert.ToInt32(dr[11]);
                    noiseVsNDROSLimitsData.NDROs = Convert.ToInt32(dr[12]);
                    noiseVsNDROSLimitsData.Trials = Convert.ToInt32(dr[13]);
                    noiseVsNDROSLimitsData.Flashes = Convert.ToInt32(dr[14]);
                    noiseVsNDROSLimitsData.Time = Convert.ToInt32(dr[15]);
                    noiseVsNDROSLimitsData.ExposureControl = Convert.ToInt32(dr[16]);
                    noiseVsNDROSLimitsData.ReadOutModeType = Convert.ToInt32(dr[17]);
                    noiseVsNDROSLimitsData.DefinedDate = Convert.ToDateTime(dr[18]);
                    noiseVsNDROSLimitsData.UserModified = Convert.ToString(dr[19]);
                    noiseVsNDROSLimitsData.SnglNoiseUpperLimit = Convert.ToDouble(dr[20]);
                    noiseVsNDROSLimitsData.SnglNoiseLowerLimit = Convert.ToDouble(dr[21]);
                    noiseVsNDROSLimitsData.SnglNoiseTolerance = Convert.ToDouble(dr[22]);
                    noiseVsNDROSLimitsDataList.Add(noiseVsNDROSLimitsData);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ProcessPhotoresponseLimitsXMLFile()
        {
            try
            {
                log.Info("Begin Reading PhotoresponseLimitsData XML.");
                DataSet ds = new DataSet();
                ds.ReadXml(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "PhotoresponseLimitsData.xml", XmlReadMode.InferSchema);
                DataView dvExposure;
                dvExposure = ds.Tables[0].DefaultView;
                photoresponseLimitsDataList = new List<PhotoresponseLimitsData>();
                foreach (DataRowView dr in dvExposure)
                {
                    PhotoresponseLimitsData photoresponseLimitsData = new PhotoresponseLimitsData();
                    photoresponseLimitsData.LimitsID = Convert.ToInt32(dr[0]);
                    photoresponseLimitsData.ECONumber = Convert.ToString(dr[1]);
                    photoresponseLimitsData.LinearityLevelMax = Convert.ToDouble(dr[2]);
                    photoresponseLimitsData.FullWellLevelMin = Convert.ToInt32(dr[3]);
                    photoresponseLimitsData.ROI_Xs = Convert.ToInt32(dr[4]);
                    photoresponseLimitsData.ROI_Ys = Convert.ToInt32(dr[5]);
                    photoresponseLimitsData.ROI_dXs = Convert.ToInt32(dr[6]);
                    photoresponseLimitsData.ROI_dYs = Convert.ToInt32(dr[7]);
                    photoresponseLimitsData.ROI_dXbs = Convert.ToInt32(dr[8]);
                    photoresponseLimitsData.ROI_dYbs = Convert.ToInt32(dr[9]);
                    photoresponseLimitsData.ROI_PixRate = Convert.ToInt32(dr[10]);
                    photoresponseLimitsData.ROI_NDRO = Convert.ToInt32(dr[11]);
                    photoresponseLimitsData.Exposures = Convert.ToInt32(dr[12]);
                    photoresponseLimitsData.Flashes = Convert.ToInt32(dr[13]);
                    photoresponseLimitsData.Time = Convert.ToInt32(dr[14]);
                    photoresponseLimitsData.ExposureControl = Convert.ToInt32(dr[15]);
                    photoresponseLimitsData.DefinedDate = Convert.ToDateTime(dr[16]);
                    photoresponseLimitsData.UserModified = Convert.ToString(dr[17]);
                    photoresponseLimitsDataList.Add(photoresponseLimitsData);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ProcessSegmentInjectLimitsXMLFile()
        {
            try
            {
                log.Info("Begin Reading SegmentInjectLimitsData XML.");
                DataSet ds = new DataSet();
                ds.ReadXml(ConfigurationManager.AppSettings["XMLFilePath"].ToString() + "SegmentInjectLimitsData.xml", XmlReadMode.InferSchema);
                DataView dvExposure;
                dvExposure = ds.Tables[0].DefaultView;
                segmentInjectLimitsDataList = new List<SegmentInjectLimitsData>();
                foreach (DataRowView dr in dvExposure)
                {
                    SegmentInjectLimitsData segmentInjectLimitsData = new SegmentInjectLimitsData();
                    segmentInjectLimitsData.LimitsID = Convert.ToInt32(dr[0]);
                    segmentInjectLimitsData.ECONumber = Convert.ToString(dr[1]);
                    segmentInjectLimitsData.InjectLevel_adu = Convert.ToInt32(dr[2]);
                    segmentInjectLimitsData.InjectDisturbance_adu = Convert.ToInt32(dr[3]);
                    segmentInjectLimitsData.DefinedDate = Convert.ToDateTime(dr[4]);
                    segmentInjectLimitsData.UserModified = Convert.ToString(dr[5]);
                    segmentInjectLimitsDataList.Add(segmentInjectLimitsData);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void CharacterizationTestLimits_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
        private void ultraComboEditorECODetails_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                TestLimitsPanel.Controls.Clear();
                ecoData = sortedECOData.Where(limit => limit.ECONumber.Equals(ultraComboEditorECODetails.SelectedItem.DisplayText.ToString())).FirstOrDefault();
                if (ecoData != null)
                    ecoDataChanged = true;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void DisplayLimitsOnLoad()
        {
            try
            {
                ultraComboEditorECODetails.SelectedIndex = 0;
                meanVarianceTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(sortedECOData[0].MeanVarianceTestLimitsData.ToList()[0]);
                darkCurrentTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(sortedECOData[0].DarkCurrentTestLimitsData.ToList()[0]);
                defectsTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(sortedECOData[0].DefectsTestLimitsData.ToList()[0]);
                injectionEfficiencyTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(sortedECOData[0].InjectionEfficiencyTestLimitsData.ToList()[0]);
                injectionPerformanceTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(sortedECOData[0].InjectionPerformanceLimitsData.ToList()[0]);
                nDROReadDriftTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(sortedECOData[0].NdroReadDriftTestLimitsData.ToList()[0]);
                noiseVsNDROSLimitsUserControl.UpdateLimitsViewWithSpecificModel(sortedECOData[0].NoiseVsNDROSLimitsData.ToList()[0]);
                photoresponseLimitsUserControl.UpdateLimitsViewWithSpecificModel(sortedECOData[0].PhotoresponseLimitsData.ToList()[0]);
                segmentInjectLimitsUserControl.UpdateLimitsViewWithSpecificModel(sortedECOData[0].SegmentInjectLimitsData.ToList()[0]);
                ledCalibrationLimitsUserControl.UpdateLimitsViewWithSpecificModel(sortedECOData[0].LEDCalibrationLimitsData.ToList()[0]);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void DisplayOldLimitsOnLoad()
        {
            try
            {
                sortedECOData = new List<ECOData>();
                sortedECOData = (from ecos in TestAppHelper.ECODataList orderby ecos.DefinedDate descending select ecos).ToList();
                List<string> ecoList = new List<string>();
                ecoList = (from ecos in sortedECOData select ecos.ECONumber).ToList();
                ultraComboEditorECODetails.DataSource = ecoList;
                ultraComboEditorECODetails.SelectedIndex = 0;
                meanVarianceTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(TestAppHelper.ECODataList[0].MeanVarianceTestLimitsData.ToList()[0]);
                darkCurrentTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(TestAppHelper.ECODataList[0].DarkCurrentTestLimitsData.ToList()[0]);
                defectsTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(TestAppHelper.ECODataList[0].DefectsTestLimitsData.ToList()[0]);
                injectionEfficiencyTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(TestAppHelper.ECODataList[0].InjectionEfficiencyTestLimitsData.ToList()[0]);
                injectionPerformanceTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(TestAppHelper.ECODataList[0].InjectionPerformanceLimitsData.ToList()[0]);
                nDROReadDriftTestLimitsUserControl.UpdateLimitsViewWithSpecificModel(TestAppHelper.ECODataList[0].NdroReadDriftTestLimitsData.ToList()[0]);
                noiseVsNDROSLimitsUserControl.UpdateLimitsViewWithSpecificModel(TestAppHelper.ECODataList[0].NoiseVsNDROSLimitsData.ToList()[0]);
                photoresponseLimitsUserControl.UpdateLimitsViewWithSpecificModel(TestAppHelper.ECODataList[0].PhotoresponseLimitsData.ToList()[0]);
                segmentInjectLimitsUserControl.UpdateLimitsViewWithSpecificModel(TestAppHelper.ECODataList[0].SegmentInjectLimitsData.ToList()[0]);
                ledCalibrationLimitsUserControl.UpdateLimitsViewWithSpecificModel(TestAppHelper.ECODataList[0].LEDCalibrationLimitsData.ToList()[0]);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task<bool> SaveLimitsData(ECOData ecoData, string OperationType)
        {
            try
            {
                var res = await Task.Run(() =>
                {
                    ecoData.DefinedDate = DateTime.Now;
                    int limitsRecordSaved = 0;
                    bool limitsXMLSaved = false;
                    using (var kcc = new KronosCamContext())
                    {
                        if (TestAppHelper.CheckDBExists())
                        {
                            if (OperationType.Equals("ADD"))
                            {
                                kcc.ecoData.Add(ecoData);
                            }
                            else if (OperationType.Equals("Remove"))
                            {
                                kcc.ecoData.Remove(ecoData);
                            }
                            else if (OperationType.Equals("Update"))
                            {
                                  kcc.ecoData.Attach(ecoData);
                            }

                            limitsRecordSaved = kcc.SaveChanges();
                        }
                        if (limitsRecordSaved > 0 || limitsXMLSaved)
                        {
                            log.Info("End ADD/Remove/Update Camera Details Limits Data to DB.");
                            return true;
                        }
                        else
                        {
                            log.Error("Eror Occured while ADD/Remove/Update Limits Data records to DB");
                            return false;
                        }
                    }
                });
                return res;
            }
            catch (Exception ex)
            {
                log.Error("Eror Occured while ADD/Remove/Update Limits Data records to DB");
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
      
        private async void AddLimitsData_Click(object sender, EventArgs e)
        {
            bool limitsChangedSuccess = false;
            try
            {              
                if (ultraComboOmnifyECOs.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a ECO Number", "Add New Test Limit", MessageBoxButtons.OK);
                    return;
                }
                if (string.IsNullOrWhiteSpace(ultraComboOmnifyECOs.SelectedItem.DisplayText.ToString()))
                {
                    MessageBox.Show("ECO Number cannot be empty", "Add New Test Limit", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                   
                    ecoData = new ECOData();
                    ecoData.ECONumber = ultraComboOmnifyECOs.SelectedItem.DisplayText;//textBoxECONumber.Text.ToString();
                    ecoData.UserModified = userName;

                    MeanVarianceTestLimitsData meanVarianceTestLimits = meanVarianceTestLimitsUserControl.ReturnMeanVarianceLimitsModel();
                    InjectionEfficiencyTestLimitsData injectionEfficiencyTestLimits = injectionEfficiencyTestLimitsUserControl.ReturnInjectionEfficiencyLimits();
                    DefectsTestLimitsData defectsTestLimits = defectsTestLimitsUserControl.ReturnDefectsTestLimit();
                    PhotoresponseLimitsData photoresponseLimits = photoresponseLimitsUserControl.ReturPhotoresponseLimit();
                    DarkCurrentTestLimitsData darkCurrentTestLimits = darkCurrentTestLimitsUserControl.ReturnDarkCurrentLimits();
                    NoiseVsNDROSLimitsData noiseVsNDROSLimits = noiseVsNDROSLimitsUserControl.ReturnNoiseVsNDROsLimit();
                    LEDCalibrationLimitsData ledCalibrationLimits = ledCalibrationLimitsUserControl.ReturnLEDCalibrationLimit();
                    NdroReadDriftTestLimitsData ndroReadDriftTestLimits = nDROReadDriftTestLimitsUserControl.ReturnNdroReadDriftLimit();
                    SegmentInjectLimitsData segmentInjectLimits = segmentInjectLimitsUserControl.ReturnSegmentInjectLimit();
                    InjectionPerformanceLimitsData injectionPerformanceLimits = injectionPerformanceTestLimitsUserControl.ReturnInjectionPerformanceLimit();

                    CheckLimitChanged(meanVarianceTestLimits, "MeanVariance");
                    if(isMeanVarianceLimitChanged)
                    {
                        meanVarianceTestLimits.ECONumber = ecoData.ECONumber;
                        meanVarianceTestLimits.UserModified = userName;
                        ecoData.MeanVarianceTestLimitsData = new List<MeanVarianceTestLimitsData>();
                        ecoData.MeanVarianceTestLimitsData.Add(meanVarianceTestLimits);
                    }
                   
                    CheckLimitChanged(injectionEfficiencyTestLimits, "InjectionEfficiency");
                    if(isInjectionEfficiencyLimitChanged)
                    {
                        injectionEfficiencyTestLimits.ECONumber = ecoData.ECONumber;
                        injectionEfficiencyTestLimits.UserModified = userName;
                        ecoData.InjectionEfficiencyTestLimitsData = new List<InjectionEfficiencyTestLimitsData>();
                        ecoData.InjectionEfficiencyTestLimitsData.Add(injectionEfficiencyTestLimits);
                    }
                    
                    CheckLimitChanged(defectsTestLimits, "Defects");
                    if(isDefectsLimitChanged)
                    {
                        defectsTestLimits.ECONumber = ecoData.ECONumber;
                        defectsTestLimits.UserModified = userName;
                        ecoData.DefectsTestLimitsData = new List<DefectsTestLimitsData>();
                        ecoData.DefectsTestLimitsData.Add(defectsTestLimits);
                    }
                   
                    CheckLimitChanged(photoresponseLimits, "Photoresponse");
                    if(isPhotoresponseLimitChanged)
                    {
                        photoresponseLimits.ECONumber = ecoData.ECONumber;
                        photoresponseLimits.UserModified = userName;
                        ecoData.PhotoresponseLimitsData = new List<PhotoresponseLimitsData>();
                        ecoData.PhotoresponseLimitsData.Add(photoresponseLimits);
                    }
                    
                    CheckLimitChanged(darkCurrentTestLimits, "DarkCurrent");
                    if(isDarkCurrentLimitChanged)
                    {
                        darkCurrentTestLimits.ECONumber = ecoData.ECONumber;
                        darkCurrentTestLimits.UserModified = userName;
                        ecoData.DarkCurrentTestLimitsData = new List<DarkCurrentTestLimitsData>();
                        ecoData.DarkCurrentTestLimitsData.Add(darkCurrentTestLimits);
                    }
                   
                    CheckLimitChanged(noiseVsNDROSLimits, "NoiseVsNDROs");
                    if(isNoiseVsNDROsLimitChanged)
                    {
                        noiseVsNDROSLimits.ECONumber = ecoData.ECONumber;
                        noiseVsNDROSLimits.UserModified = userName;
                        ecoData.NoiseVsNDROSLimitsData = new List<NoiseVsNDROSLimitsData>();
                        ecoData.NoiseVsNDROSLimitsData.Add(noiseVsNDROSLimits);
                    }
                   
                    CheckLimitChanged(ledCalibrationLimits, "LEDCalibration");
                    if(isLEDCalibrationLimitChanged)
                    {
                        ledCalibrationLimits.ECONumber = ecoData.ECONumber;
                        ledCalibrationLimits.UserModified = userName;
                        ecoData.LEDCalibrationLimitsData = new List<LEDCalibrationLimitsData>();
                        ecoData.LEDCalibrationLimitsData.Add(ledCalibrationLimits);
                    }
                                     
                    if (await SaveLimitsData(ecoData, "ADD"))
                    {
                        limitsChangedSuccess = true;
                            //MessageBox.Show("New Test Limit added successfully", "Add Test Limit", MessageBoxButtons.OK);
                            TestAppHelper.SelectedECONumber = string.Empty;
                    }
                    else
                    {
                        limitsChangedSuccess = false;
                       // MessageBox.Show("New Test Limit add failed", "Add Test Limit", MessageBoxButtons.OK);
                    }  
                    if(limitsChangedSuccess)
                    {
                        limitsChangedSuccess = false;
                        if (!isMeanVarianceLimitChanged)
                        {
                            using (var kcc = new KronosCamContext())
                            {
                                meanVarianceTestLimits.ECONumber = ultraComboOmnifyECOs.SelectedItem.DisplayText;
                                meanVarianceTestLimits.UserModified = userName;
                                meanVarianceTestLimits.DefinedDate = DateTime.Now;
                                kcc.meanVarianceTestLimitsData.Attach(meanVarianceTestLimits);
                                kcc.Entry(meanVarianceTestLimits).State = System.Data.Entity.EntityState.Modified;
                                int res = kcc.SaveChanges();
                                if(res > 0)
                                {
                                    limitsChangedSuccess = true;
                                }
                                else
                                {
                                    limitsChangedSuccess = false;
                                }
                            }
                        }
                        if (!isInjectionEfficiencyLimitChanged)
                        {
                            using (var kcc = new KronosCamContext())
                            {
                                injectionEfficiencyTestLimits.ECONumber = ultraComboOmnifyECOs.SelectedItem.DisplayText;
                                injectionEfficiencyTestLimits.UserModified = userName;
                                injectionEfficiencyTestLimits.DefinedDate = DateTime.Now;
                                kcc.injectionEfficiencyTestLimitsData.Attach(injectionEfficiencyTestLimits);
                                kcc.Entry(injectionEfficiencyTestLimits).State = System.Data.Entity.EntityState.Modified;
                                int res = kcc.SaveChanges();
                                if (res > 0)
                                {
                                    limitsChangedSuccess = true;
                                }
                                else
                                {
                                    limitsChangedSuccess = false;
                                }
                            }
                        }
                        if (!isDefectsLimitChanged)
                        {
                            using (var kcc = new KronosCamContext())
                            {
                                defectsTestLimits.ECONumber = ultraComboOmnifyECOs.SelectedItem.DisplayText;
                                defectsTestLimits.UserModified = userName;
                                defectsTestLimits.DefinedDate = DateTime.Now;
                                kcc.defectsTestLimitsData.Attach(defectsTestLimits);
                                kcc.Entry(defectsTestLimits).State = System.Data.Entity.EntityState.Modified;
                                int res = kcc.SaveChanges();
                                if (res > 0)
                                {
                                    limitsChangedSuccess = true;
                                }
                                else
                                {
                                    limitsChangedSuccess = false;
                                }
                            }
                        }
                        if (!isDarkCurrentLimitChanged)
                        {
                            using (var kcc = new KronosCamContext())
                            {
                                darkCurrentTestLimits.ECONumber = ultraComboOmnifyECOs.SelectedItem.DisplayText;
                                darkCurrentTestLimits.UserModified = userName;
                                darkCurrentTestLimits.DefinedDate = DateTime.Now;
                                kcc.darkCurrentTestLimitsData.Attach(darkCurrentTestLimits);
                                kcc.Entry(darkCurrentTestLimits).State = System.Data.Entity.EntityState.Modified;
                                int res = kcc.SaveChanges();
                                if (res > 0)
                                {
                                    limitsChangedSuccess = true;
                                }
                                else
                                {
                                    limitsChangedSuccess = false;
                                }
                            }
                        }
                        if (!isNoiseVsNDROsLimitChanged)
                        {
                            using (var kcc = new KronosCamContext())
                            {
                                noiseVsNDROSLimits.ECONumber = ultraComboOmnifyECOs.SelectedItem.DisplayText;
                                noiseVsNDROSLimits.UserModified = userName;
                                noiseVsNDROSLimits.DefinedDate = DateTime.Now;
                                kcc.noiseVsNDROSLimitsData.Attach(noiseVsNDROSLimits);
                                kcc.Entry(noiseVsNDROSLimits).State = System.Data.Entity.EntityState.Modified;
                                int res = kcc.SaveChanges();
                                if (res > 0)
                                {
                                    limitsChangedSuccess = true;
                                }
                                else
                                {
                                    limitsChangedSuccess = false;
                                }
                            }
                        }
                        if (!isPhotoresponseLimitChanged)
                        {
                            using (var kcc = new KronosCamContext())
                            {
                                photoresponseLimits.ECONumber = ultraComboOmnifyECOs.SelectedItem.DisplayText;
                                photoresponseLimits.UserModified = userName;
                                photoresponseLimits.DefinedDate = DateTime.Now;
                                kcc.photoresponseLimitsData.Attach(photoresponseLimits);
                                kcc.Entry(photoresponseLimits).State = System.Data.Entity.EntityState.Modified;
                                int res = kcc.SaveChanges();
                                if (res > 0)
                                {
                                    limitsChangedSuccess = true;
                                }
                                else
                                {
                                    limitsChangedSuccess = false;
                                }
                            }
                        }
                        if (!isLEDCalibrationLimitChanged)
                        {
                            using (var kcc = new KronosCamContext())
                            {
                                ledCalibrationLimits.ECONumber = ultraComboOmnifyECOs.SelectedItem.DisplayText;
                                ledCalibrationLimits.UserModified = userName;
                                ledCalibrationLimits.DefinedDate = DateTime.Now;
                                kcc.ledCalibrationLimitsData.Attach(ledCalibrationLimits);
                                kcc.Entry(ledCalibrationLimits).State = System.Data.Entity.EntityState.Modified;
                                int res = kcc.SaveChanges();
                                if (res > 0)
                                {
                                    limitsChangedSuccess = true;
                                }
                                else
                                {
                                    limitsChangedSuccess = false;
                                }
                            }
                        }
                        if (!isNDROReadDriftLimitChanged)
                        {
                            using (var kcc = new KronosCamContext())
                            {
                                ndroReadDriftTestLimits.ECONumber = ultraComboOmnifyECOs.SelectedItem.DisplayText;
                                ndroReadDriftTestLimits.UserModified = userName;
                                ndroReadDriftTestLimits.DefinedDate = DateTime.Now;
                                kcc.ndroReadDriftTestLimitsData.Attach(ndroReadDriftTestLimits);
                                kcc.Entry(ndroReadDriftTestLimits).State = System.Data.Entity.EntityState.Modified;
                                int res = kcc.SaveChanges();
                                if (res > 0)
                                {
                                    limitsChangedSuccess = true;
                                }
                                else
                                {
                                    limitsChangedSuccess = false;
                                }
                            }
                        }
                        if (!isSegmentInjectLimitChanged)
                        {
                            using (var kcc = new KronosCamContext())
                            {
                                segmentInjectLimits.ECONumber = ultraComboOmnifyECOs.SelectedItem.DisplayText;
                                segmentInjectLimits.UserModified = userName;
                                segmentInjectLimits.DefinedDate = DateTime.Now;
                                kcc.segmentInjectLimitsData.Attach(segmentInjectLimits);
                                kcc.Entry(segmentInjectLimits).State = System.Data.Entity.EntityState.Modified;
                                int res = kcc.SaveChanges();
                                if (res > 0)
                                {
                                    limitsChangedSuccess = true;
                                }
                                else
                                {
                                    limitsChangedSuccess = false;
                                }
                            }
                        }
                        if (!isInjectionPerformanceLimitChanged)
                        {
                            using (var kcc = new KronosCamContext())
                            {
                                injectionPerformanceLimits.ECONumber = ultraComboOmnifyECOs.SelectedItem.DisplayText;
                                injectionPerformanceLimits.UserModified = userName;
                                injectionPerformanceLimits.DefinedDate = DateTime.Now;
                                kcc.injectionPerformanceLimitsData.Attach(injectionPerformanceLimits);
                                kcc.Entry(injectionPerformanceLimits).State = System.Data.Entity.EntityState.Modified;
                                int res = kcc.SaveChanges();
                                if (res > 0)
                                {
                                    limitsChangedSuccess = true;
                                }
                                else
                                {
                                    limitsChangedSuccess = false;
                                }
                            }
                        }
                    }
                    isMeanVarianceLimitChanged = false;
                     isDefectsLimitChanged = false;
                     isDarkCurrentLimitChanged = false;
                     isPhotoresponseLimitChanged = false;
                     isNoiseVsNDROsLimitChanged = false;
                     isInjectionEfficiencyLimitChanged = false;
                     isLEDCalibrationLimitChanged = false;
                     isSegmentInjectLimitChanged = false;
                     isInjectionPerformanceLimitChanged = false;
                     isNDROReadDriftLimitChanged = false;
                    if (limitsChangedSuccess)
                    {
                       
                        MessageBox.Show("New Test Limit added successfully", "Add Test Limit", MessageBoxButtons.OK);
                        TestAppHelper.SelectedECONumber = string.Empty;
                    }
                    else
                    {
                       MessageBox.Show("New Test Limit add failed", "Add Test Limit", MessageBoxButtons.OK);
                    }
                    GetEcoData();
                }
            }
            catch (Exception ex)
            {
                log.Error("Eror Occured while adding Limits Data records to DB");
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                isMeanVarianceLimitChanged = false;
                isDefectsLimitChanged = false;
                isDarkCurrentLimitChanged = false;
                isPhotoresponseLimitChanged = false;
                isNoiseVsNDROsLimitChanged = false;
                isInjectionEfficiencyLimitChanged = false;
                isLEDCalibrationLimitChanged = false;
                isSegmentInjectLimitChanged = false;
                isInjectionPerformanceLimitChanged = false;
                limitsChangedSuccess = false;
                isNDROReadDriftLimitChanged = false;

            }
        }
      
        private void CheckLimitChanged(object limitModel, string LimitName)
        {
            bool limitChanged = false;
            try
            {
                switch(LimitName)
                {
                    case "MeanVariance":
                        MeanVarianceTestLimitsData meanVarianceTestLimitsData = (MeanVarianceTestLimitsData)limitModel;
                        if (meanVarianceTestLimitsData.ConversionFactorNominal != sortedECOData[0].MeanVarianceTestLimitsData.ToList()[0].ConversionFactorNominal ||
                            meanVarianceTestLimitsData.ConverisonFactorTol != sortedECOData[0].MeanVarianceTestLimitsData.ToList()[0].ConverisonFactorTol ||
                            meanVarianceTestLimitsData.ConversionFactorLowerLimit != sortedECOData[0].MeanVarianceTestLimitsData.ToList()[0].ConversionFactorLowerLimit ||
                            meanVarianceTestLimitsData.ConversionFactorUpperLimit != sortedECOData[0].MeanVarianceTestLimitsData.ToList()[0].ConversionFactorUpperLimit)
                        {
                            isMeanVarianceLimitChanged = true;
                        }
                        break;
                    case "Defects":
                        DefectsTestLimitsData defectsTestLimitsData = (DefectsTestLimitsData)limitModel;
                        if (defectsTestLimitsData.DriftROI != sortedECOData[0].DefectsTestLimitsData.ToList()[0].DriftROI ||
                            defectsTestLimitsData.DeadPixel != sortedECOData[0].DefectsTestLimitsData.ToList()[0].DeadPixel ||
                            defectsTestLimitsData.DarkPixel != sortedECOData[0].DefectsTestLimitsData.ToList()[0].DarkPixel ||
                            defectsTestLimitsData.ClusterSize != sortedECOData[0].DefectsTestLimitsData.ToList()[0].ClusterSize ||
                            defectsTestLimitsData.AdjacentPixelsInCluster != sortedECOData[0].DefectsTestLimitsData.ToList()[0].AdjacentPixelsInCluster ||
                            defectsTestLimitsData.HotPixel != sortedECOData[0].DefectsTestLimitsData.ToList()[0].HotPixel ||
                            defectsTestLimitsData.TotalNumClusters != sortedECOData[0].DefectsTestLimitsData.ToList()[0].TotalNumClusters ||
                            defectsTestLimitsData.TotalRows != sortedECOData[0].DefectsTestLimitsData.ToList()[0].TotalRows ||
                            defectsTestLimitsData.TotalColumns != sortedECOData[0].DefectsTestLimitsData.ToList()[0].TotalColumns ||
                            defectsTestLimitsData.MaxDarkROI != sortedECOData[0].DefectsTestLimitsData.ToList()[0].MaxDarkROI ||
                            defectsTestLimitsData.MaxTrap != sortedECOData[0].DefectsTestLimitsData.ToList()[0].MaxTrap ||
                            defectsTestLimitsData.AveTrap != sortedECOData[0].DefectsTestLimitsData.ToList()[0].AveTrap ||
                            defectsTestLimitsData.TotalNumDefects != sortedECOData[0].DefectsTestLimitsData.ToList()[0].TotalNumDefects ||
                            defectsTestLimitsData.PercentAboveMean != sortedECOData[0].DefectsTestLimitsData.ToList()[0].PercentAboveMean ||
                            defectsTestLimitsData.PercentBelowMean != sortedECOData[0].DefectsTestLimitsData.ToList()[0].PercentBelowMean ||
                            defectsTestLimitsData.PercentAboveRows != sortedECOData[0].DefectsTestLimitsData.ToList()[0].PercentAboveRows ||
                            defectsTestLimitsData.PercentBelowRows != sortedECOData[0].DefectsTestLimitsData.ToList()[0].PercentBelowRows)
                        {
                            isDefectsLimitChanged = true;
                        }
                        break;
                    case "DarkCurrent":
                        DarkCurrentTestLimitsData darkCurrentTestLimitsData = (DarkCurrentTestLimitsData)limitModel;
                        if (darkCurrentTestLimitsData.DarkCurrentMaxDarkROI != sortedECOData[0].DarkCurrentTestLimitsData.ToList()[0].DarkCurrentMaxDarkROI ||
                            darkCurrentTestLimitsData.DarkCurrentMaxDarkROITol != sortedECOData[0].DarkCurrentTestLimitsData.ToList()[0].DarkCurrentMaxDarkROITol ||
                            darkCurrentTestLimitsData.DarkCurrentUpperLimit != sortedECOData[0].DarkCurrentTestLimitsData.ToList()[0].DarkCurrentUpperLimit ||
                            darkCurrentTestLimitsData.DarkCurrentLowerLimit != sortedECOData[0].DarkCurrentTestLimitsData.ToList()[0].DarkCurrentLowerLimit)                            
                        {
                            isDarkCurrentLimitChanged = true;
                        }
                        break;
                    case "Photoresponse":
                        PhotoresponseLimitsData photoresponseLimitsData = (PhotoresponseLimitsData)limitModel;
                        if (photoresponseLimitsData.FullWellLevelMin != sortedECOData[0].PhotoresponseLimitsData.ToList()[0].FullWellLevelMin ||
                            photoresponseLimitsData.LinearityLevelMax != sortedECOData[0].PhotoresponseLimitsData.ToList()[0].LinearityLevelMax)
                        {
                            isPhotoresponseLimitChanged = true;
                        }
                        break;
                    case "NoiseVsNDROs":
                        NoiseVsNDROSLimitsData noiseVsNDROSLimitsData = (NoiseVsNDROSLimitsData)limitModel;
                        if (noiseVsNDROSLimitsData.SnglNoiseLowerLimit != sortedECOData[0].NoiseVsNDROSLimitsData.ToList()[0].SnglNoiseLowerLimit ||
                            noiseVsNDROSLimitsData.SnglNoiseUpperLimit != sortedECOData[0].NoiseVsNDROSLimitsData.ToList()[0].SnglNoiseUpperLimit ||
                            noiseVsNDROSLimitsData.SnglNoiseMax != sortedECOData[0].NoiseVsNDROSLimitsData.ToList()[0].SnglNoiseMax ||
                            noiseVsNDROSLimitsData.SnglNoiseTolerance != sortedECOData[0].NoiseVsNDROSLimitsData.ToList()[0].SnglNoiseTolerance ||
                            noiseVsNDROSLimitsData.RPower2Correlation != sortedECOData[0].NoiseVsNDROSLimitsData.ToList()[0].RPower2Correlation ||
                            noiseVsNDROSLimitsData.PowerFitLower != sortedECOData[0].NoiseVsNDROSLimitsData.ToList()[0].PowerFitLower ||
                            noiseVsNDROSLimitsData.PowerFitUpper != sortedECOData[0].NoiseVsNDROSLimitsData.ToList()[0].PowerFitUpper)
                        {
                            isNoiseVsNDROsLimitChanged = true;
                        }
                        break;
                    case "NdroReadDrift":
                        //NdroReadDriftTestLimitsData ndroReadDriftTestLimitsData = (NdroReadDriftTestLimitsData)limitModel;                       
                            isNDROReadDriftLimitChanged = false;
                        break;
                    case "SegmentInject":
                       // SegmentInjectLimitsData segmentInjectLimitsData = (SegmentInjectLimitsData)limitModel;
                        isSegmentInjectLimitChanged = false;
                        break;
                    case "InjectionEfficiency":
                        InjectionEfficiencyTestLimitsData injectionEfficiencyTestLimitsData = (InjectionEfficiencyTestLimitsData)limitModel;
                        if (injectionEfficiencyTestLimitsData.InjectionEffLimit != sortedECOData[0].InjectionEfficiencyTestLimitsData.ToList()[0].InjectionEffLimit)
                        {
                            isInjectionEfficiencyLimitChanged = true;
                        }
                        break;
                    case "InjectionPerformance":
                        //InjectionPerformanceLimitsData injectionPerformanceLimitsData = (InjectionPerformanceLimitsData)limitModel;
                        isInjectionPerformanceLimitChanged = false;
                        break;
                    case "LEDCalibration":
                        LEDCalibrationLimitsData lEDCalibrationLimitsData = (LEDCalibrationLimitsData)limitModel;
                        if (lEDCalibrationLimitsData.Red40LEDLimit != sortedECOData[0].LEDCalibrationLimitsData.ToList()[0].Red40LEDLimit ||
                            lEDCalibrationLimitsData.Red60LEDLimit != sortedECOData[0].LEDCalibrationLimitsData.ToList()[0].Red60LEDLimit ||
                            lEDCalibrationLimitsData.Green40LEDLimit != sortedECOData[0].LEDCalibrationLimitsData.ToList()[0].Green40LEDLimit ||
                            lEDCalibrationLimitsData.Green60LEDLimit != sortedECOData[0].LEDCalibrationLimitsData.ToList()[0].Green60LEDLimit ||
                            lEDCalibrationLimitsData.Blue40LEDLimit != sortedECOData[0].LEDCalibrationLimitsData.ToList()[0].Blue40LEDLimit ||
                            lEDCalibrationLimitsData.Blue60LEDLimit != sortedECOData[0].LEDCalibrationLimitsData.ToList()[0].Blue60LEDLimit ||
                            lEDCalibrationLimitsData.LEDLimitOffset != sortedECOData[0].LEDCalibrationLimitsData.ToList()[0].LEDLimitOffset ||
                            lEDCalibrationLimitsData.CurrentTestInterval != sortedECOData[0].LEDCalibrationLimitsData.ToList()[0].CurrentTestInterval||
                            lEDCalibrationLimitsData.PreviousTestInterval != sortedECOData[0].LEDCalibrationLimitsData.ToList()[0].PreviousTestInterval||
                            lEDCalibrationLimitsData.UpperUVLEDLimit != sortedECOData[0].LEDCalibrationLimitsData.ToList()[0].UpperUVLEDLimit ||
                            lEDCalibrationLimitsData.LowerUVLEDLimit != sortedECOData[0].LEDCalibrationLimitsData.ToList()[0].LowerUVLEDLimit)
                        {
                            isLEDCalibrationLimitChanged = true;
                        }
                        break;
                }

               // return limitChanged;
                   
            
            }
            catch(Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                //return false;
            }
        }
        private async void RemoveLimitsData_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ultraComboOmnifyECOs.SelectedItem.DisplayText.ToString()))
                {
                    MessageBox.Show("ECO Number cannot be empty", "Remove Test Limit", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    ECOData ecoData = new ECOData();
                    ecoData = sortedECOData.Where(eco => eco.ECONumber.Equals(ultraComboOmnifyECOs.SelectedItem.DisplayText)).FirstOrDefault();
                    if (await SaveLimitsData(ecoData, "REMOVE"))
                    {
                        MessageBox.Show("Test Limit removed successfully", "Remove Test Limit", MessageBoxButtons.OK);
                        GetEcoData();
                    }
                    else
                    {
                        MessageBox.Show("Test Limit removed failed", "Remove Test Limit", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Eror Occured while Remvoing Limits Data records to DB");
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void RefreshLimitsData_Click(object sender, EventArgs e)
        {
            try
            {
                GetEcoData();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateCharacterizationLimit_Click(object sender, EventArgs e)
        {
            foreach (Control control in TestLimitsPanel.Controls)
            {
                ecoData = sortedECOData.Where(limit => limit.ECONumber.Equals(ultraComboEditorECODetails.SelectedItem.DisplayText.ToString())).FirstOrDefault();
                switch (control.GetType().Name.ToString())
                {
                    case "MeanVarianceLimitsUserControl":
                        ecoData.MeanVarianceTestLimitsData.Add(meanVarianceTestLimitsUserControl.ReturnMeanVarianceLimitsModel());
                        TestAppHelper.meanVarianceTestLimitsData = meanVarianceTestLimitsUserControl.ReturnMeanVarianceLimitsModel();
                        ecoData.MeanVarianceTestLimitsData.Remove(ecoData.MeanVarianceTestLimitsData.ToList()[0]);
                        break;
                    case "DarkCurrentLimitsUserControl":
                        ecoData.DarkCurrentTestLimitsData.Add(darkCurrentTestLimitsUserControl.ReturnDarkCurrentLimits());
                        TestAppHelper.darkCurrentTestLimitsData = darkCurrentTestLimitsUserControl.ReturnDarkCurrentLimits();
                        ecoData.DarkCurrentTestLimitsData.Remove(ecoData.DarkCurrentTestLimitsData.ToList()[0]);
                        break;
                    case "DefectsLimitsUserControl":
                        ecoData.DefectsTestLimitsData.Add(defectsTestLimitsUserControl.ReturnDefectsTestLimit());
                        TestAppHelper.defectsTestLimitsData = defectsTestLimitsUserControl.ReturnDefectsTestLimit();
                        ecoData.DefectsTestLimitsData.Remove(ecoData.DefectsTestLimitsData.ToList()[0]);
                        break;
                    case "InjectionEfficiencyLimitsUserControl":
                        ecoData.InjectionEfficiencyTestLimitsData.Add(injectionEfficiencyTestLimitsUserControl.ReturnInjectionEfficiencyLimits());
                        TestAppHelper.injectionEfficiencyTestLimitsData = injectionEfficiencyTestLimitsUserControl.ReturnInjectionEfficiencyLimits();
                        ecoData.InjectionEfficiencyTestLimitsData.Remove(ecoData.InjectionEfficiencyTestLimitsData.ToList()[0]);
                        break;
                    case "InjectionPerformanceLimitsUserControl":
                        ecoData.InjectionPerformanceLimitsData.Add(injectionPerformanceTestLimitsUserControl.ReturnInjectionPerformanceLimit());
                        TestAppHelper.injectionPerformanceLimitsData = injectionPerformanceTestLimitsUserControl.ReturnInjectionPerformanceLimit();
                        ecoData.InjectionPerformanceLimitsData.Remove(ecoData.InjectionPerformanceLimitsData.ToList()[0]);
                        break;
                    case "NDROReadDriftLimitsUserControl":
                        ecoData.NdroReadDriftTestLimitsData.Add(nDROReadDriftTestLimitsUserControl.ReturnNdroReadDriftLimit());
                        TestAppHelper.ndroReadDriftTestLimitsData = nDROReadDriftTestLimitsUserControl.ReturnNdroReadDriftLimit();
                        ecoData.NdroReadDriftTestLimitsData.Remove(ecoData.NdroReadDriftTestLimitsData.ToList()[0]);
                        break;
                    case "NoiseVsNDROSLimitsUserControl":
                        ecoData.NoiseVsNDROSLimitsData.Add(noiseVsNDROSLimitsUserControl.ReturnNoiseVsNDROsLimit());
                        TestAppHelper.noiseVsNDROSLimitsData = noiseVsNDROSLimitsUserControl.ReturnNoiseVsNDROsLimit();
                        ecoData.NoiseVsNDROSLimitsData.Remove(ecoData.NoiseVsNDROSLimitsData.ToList()[0]);
                        break;
                    case "PhotoresponseLimitsUserControl":
                        ecoData.PhotoresponseLimitsData.Add(photoresponseLimitsUserControl.ReturPhotoresponseLimit());
                        TestAppHelper.photoresponseLimitsData = photoresponseLimitsUserControl.ReturPhotoresponseLimit();
                        ecoData.PhotoresponseLimitsData.Remove(ecoData.PhotoresponseLimitsData.ToList()[0]);
                        break;
                    case "SegmentInjectLimitsUserControl":
                        ecoData.SegmentInjectLimitsData.Add(segmentInjectLimitsUserControl.ReturnSegmentInjectLimit());
                        TestAppHelper.segmentInjectLimitsData = segmentInjectLimitsUserControl.ReturnSegmentInjectLimit();
                        ecoData.SegmentInjectLimitsData.Remove(ecoData.SegmentInjectLimitsData.ToList()[0]);
                        break;
                    case "LEDCalibrationLimitsUserControl":
                        ecoData.LEDCalibrationLimitsData.Add(ledCalibrationLimitsUserControl.ReturnLEDCalibrationLimit());
                        TestAppHelper.ledCalibrationLimitsData = ledCalibrationLimitsUserControl.ReturnLEDCalibrationLimit();
                        ecoData.LEDCalibrationLimitsData.Remove(ecoData.LEDCalibrationLimitsData.ToList()[0]);
                        break;
                }
                TestAppHelper.eCOData = ecoData;
                TestAppHelper.ECODataList = new List<ECOData>();
                TestAppHelper.ECODataList = sortedECOData;
                enggLimitUpdated = true;
            }
        }
        private void RefreshCharacterizationLimit_Click(object sender, EventArgs e)
        {
            GetEcoData();
        }
        private async void ultraGridLimits_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                e.Layout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
                await InitializeGridLayout();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task InitializeGridLayout()
        {
            try
            {
                await Task.Run(() =>
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in ultraGridLimits.Rows)
                    {
                        if (row.ChildBands != null && row.ChildBands.Count > 0)
                        {
                            foreach (Infragistics.Win.UltraWinGrid.UltraGridChildBand childband in row.ChildBands)
                            {
                                childband.Band.HeaderVisible = true;
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void btnApplyLimit_Click(object sender, EventArgs e)
        {
            try
            {
                ecoNumberSelected = TestAppHelper.SelectedECONumber = ultraComboEditorECODetails.SelectedItem.DisplayText.ToString();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraComboOmnifyECOs_SelectionChanged(object sender, EventArgs e)
        {
            ECODescription.Text = ecoNumbersDict.FirstOrDefault(eco => eco.Key.Equals(ultraComboOmnifyECOs.SelectedItem.DisplayText)).Value;
        }
    }
}
