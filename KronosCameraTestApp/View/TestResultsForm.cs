using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.Presenter;
using KronosCameraTestApp.Properties;
using KronosCameraTestApp.View.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
namespace KronosCameraTestApp.View
{
    public partial class TestResultsForm : Form
    {    
        MeanVariancePresenter meanVariancePresenter { get; set; }     
        DarkCurrentPresenter darkCurrentPresenter { get; set; }
        DefectsPresenter defectsPresenter { get; set; }
        InjectionEfficiencyPresenter injectionEfficiencyPresenter { get; set; }      
        NoiseVsNDROPresenter noiseVsNDROPresenter { get; set; }       
        PhotoresponsePresenter photoresponsePresenter { get; set; }       
        RedBluePresenter redBluePresenter { get; set; }      
        ShutterDrivePresenter shutterDrivePresenter { get; set; }
        OverAllTestResultsPresenter overAllTestResultsPresenter { get; set; }
        ParametersModel parametersModel { get; set; }   
        List<OverAllTestResultsModel> overAllTestResultsModelList { get; set; }
        List<OverAllTestResultsModel> OverallModelList { get; set; }
        List<MeanVarianceTestLimitsData> MeanVarianceTestLimitsDataList { get; set; }
        DateTime? fromDate;
        DateTime? toDate;
        string cameraSN = string.Empty;
        string printDocumentHeaderTitle = string.Empty;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Image resultImage = null;
        string testResultName = string.Empty;
        int lastPrintX;
        int lastPrintY;
        public TestResultsForm(MeanVariancePresenter meanVariancePresenter, DarkCurrentPresenter darkCurrentPresenter,
                                DefectsPresenter defectsPresenter, InjectionEfficiencyPresenter injectionEfficiencyPresenter,
                                NoiseVsNDROPresenter noiseVsNDROPresenter, PhotoresponsePresenter photoresponsePresenter, RedBluePresenter redBluePresenter,
                                ShutterDrivePresenter shutterDrivePresenter, OverAllTestResultsPresenter overAllTestResultsPresenter)
        {
            InitializeComponent();
            this.meanVariancePresenter = meanVariancePresenter;     
            this.darkCurrentPresenter = darkCurrentPresenter;
            this.injectionEfficiencyPresenter = injectionEfficiencyPresenter;
            this.noiseVsNDROPresenter = noiseVsNDROPresenter;
            this.photoresponsePresenter = photoresponsePresenter;
            this.redBluePresenter = redBluePresenter;
            this.shutterDrivePresenter = shutterDrivePresenter;
            this.overAllTestResultsPresenter = overAllTestResultsPresenter;
        }
        private async void TestResultsForm_Load(object sender, EventArgs e)
        {
            try
            {
                UseWaitCursor = true;
                TopNumberRecords.SelectedIndex = 0;
                await GetTestResults();
                testResultsUltraGrid.DataSource = OverallModelList;
                UseWaitCursor = false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraExplorerBar1_ItemClick(object sender, Infragistics.Win.UltraWinExplorerBar.ItemEventArgs e)
        {
            try
            {
                testResultsUltraGrid.Text = "Test Results";
                testResultsUltraPictureBox.Image = null;
                testResultsUltraGrid.DataSource = null;
                UseWaitCursor = true;
                if (OverallModelList!=null)
                    UpdateResultsGrid(e.Item.Tag.ToString()); 
                UseWaitCursor = false;
            }
            catch (Exception ex)
            {
                UseWaitCursor = false;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateResultsGrid(string str)
        {
            try
            {
                UseWaitCursor = true;
                switch (str)
                {
                    case "MeanVariance":
                    case"Mean Variance Test Results":
                        UpdateMeanVarianceResultsGrid();
                        break;
                    case "DarkCurrent":
                    case "Dark Current Test Results":
                        UpdateDarkCurrentResultsGrid();
                        break;
                    case "Defects":
                    case "Defects Test Results":
                        UpdateDefectsResultsGrid();
                        break;
                    case "InjectionEfficiency":
                    case "Injection Efficiency Test Results":
                        UpdateInjectionPerformaceResultsResultsGrid();
                        break;
                    case "InjectionPerformance":
                    case "Injection Performance Test Results":
                        UpdateInjectionPerformaceResultsResultsGrid();
                        break;
                    case "ReadNoiseVsNDROs":
                    case "Noise Vs NDROs Test Results":
                        UpdateNoiseVsNDROsResultsGrid();
                        break;
                    case "Photoresponse":
                    case "Photoresponse Test Results":
                        UpdatePhotoresponseResultsGrid();
                        break;
                    case "RedBlue":
                    case "Red&Blue Test Results":
                    case "Red, Blue & UV Test Results":
                        UpdateRedBlueResultsGrid();
                        break;
                    case "ShutterDrive":
                    case "ShutterDrive Test Results":
                        UpdateShutterDriveResultsGrid();
                        break;
                    case "OverAllResults":
                    case "Over All Test Results":
                    case "Test Results":
                        UseWaitCursor = true;
                        testResultsUltraGrid.UseWaitCursor = true;
                        testResultsUltraGrid.Text = "Over All Test Results";
                        testResultsUltraGrid.DataSource = OverallModelList;
                        testResultsUltraGrid.UseWaitCursor = false;
                        UseWaitCursor = false;
                        break;
                }
                UseWaitCursor = false;
            }
            catch (Exception ex)
            {
                UseWaitCursor = false;   
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                UseWaitCursor = true;
                InitializeGridLayout();
                e.Layout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
                UseWaitCursor = false;
            }
            catch (Exception ex)
            {
                UseWaitCursor = false;
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void InitializeGridLayout()
        {
            try
            {               
                Application.UseWaitCursor = true;
                UseWaitCursor = true;
                if (testResultsUltraGrid.DisplayLayout.Bands[0].Columns.All.Where(col => col.ToString().Equals("TestResultImagePath")).FirstOrDefault() != null)
                    testResultsUltraGrid.DisplayLayout.Bands[0].Columns["TestResultImagePath"].Hidden = true;
                if (testResultsUltraGrid.DisplayLayout.Bands[0].Columns.All.Where(col => col.ToString().Equals("TestResultsID")).FirstOrDefault() != null)
                    testResultsUltraGrid.DisplayLayout.Bands[0].Columns["TestResultsID"].Hidden = true;
                if (testResultsUltraGrid.DisplayLayout.Bands[0].Columns.All.Where(col => col.ToString().Equals("ResultImage")).FirstOrDefault() != null)
                    testResultsUltraGrid.DisplayLayout.Bands[0].Columns["ResultImage"].Hidden = true;
                if (testResultsUltraGrid.DisplayLayout.Bands[0].Columns.All.Where(col => col.ToString().Equals("RedImageData")).FirstOrDefault() != null)
                    testResultsUltraGrid.DisplayLayout.Bands[0].Columns["RedImageData"].Hidden = true;
                if (testResultsUltraGrid.DisplayLayout.Bands[0].Columns.All.Where(col => col.ToString().Equals("BlueImageData")).FirstOrDefault() != null)
                    testResultsUltraGrid.DisplayLayout.Bands[0].Columns["BlueImageData"].Hidden = true;
                if (testResultsUltraGrid.DisplayLayout.Bands[0].Columns.All.Where(col => col.ToString().Equals("RedBlueImageDifference")).FirstOrDefault() != null)
                    testResultsUltraGrid.DisplayLayout.Bands[0].Columns["RedBlueImageDifference"].Hidden = true;
                if (testResultsUltraGrid.DisplayLayout.Bands[0].Columns.All.Where(col => col.ToString().Equals("ResultsID")).FirstOrDefault() != null)
                    testResultsUltraGrid.DisplayLayout.Bands[0].Columns["ResultsID"].Hidden = true;
                if (testResultsUltraGrid.DisplayLayout.Bands[0].Columns.All.Where(col => col.ToString().Equals("FinalTestID")).FirstOrDefault() != null)
                    testResultsUltraGrid.DisplayLayout.Bands[0].Columns["FinalTestID"].Hidden = true;
                testResultsUltraGrid.DisplayLayout.AutoFitStyle = AutoFitStyle.ExtendLastColumn;
                if (testResultsUltraGrid.Rows.Count > 0)
                {
                    foreach (UltraGridRow row in testResultsUltraGrid.Rows)
                    {
                        if (row.ChildBands != null && row.ChildBands.Count > 0)
                        {
                            foreach (UltraGridChildBand childband in row.ChildBands)
                            {
                                childband.Band.HeaderVisible = true;
                                if (childband.Band.Columns.All.Where(col => col.ToString().Equals("TestResultsID")).FirstOrDefault() != null)
                                    childband.Band.Columns["TestResultsID"].Hidden = true;
                                if (childband.Band.Columns.All.Where(col => col.ToString().Equals("FinalTestID")).FirstOrDefault() != null)
                                    childband.Band.Columns["FinalTestID"].Hidden = true;
                                if (childband.Band.Columns.All.Where(col=>col.ToString().Equals("DateExecuted")).FirstOrDefault() !=null)
                                    childband.Band.Columns["DateExecuted"].Hidden = true;
                                if (childband.Band.Columns.All.Where(col => col.ToString().Equals("UserExecuted")).FirstOrDefault() != null)
                                    childband.Band.Columns["UserExecuted"].Hidden = true;
                                if (childband.Band.Columns.All.Where(col => col.ToString().Equals("ResultsID")).FirstOrDefault() != null)
                                    childband.Band.Columns["ResultsID"].Hidden = true;
                                if (childband.Band.Columns.All.Where(col => col.ToString().Equals("ECONumber")).FirstOrDefault() != null)
                                    childband.Band.Columns["ECONumber"].Hidden = true;
                                if (childband.Band.Columns.All.Where(col => col.ToString().Equals("CameraSerialNumber")).FirstOrDefault() != null)
                                    childband.Band.Columns["CameraSerialNumber"].Hidden = true;
                                if (childband.Band.Columns.All.Where(col => col.ToString().Equals("ImagerSerialNumber")).FirstOrDefault() != null)
                                    childband.Band.Columns["ImagerSerialNumber"].Hidden = true;
                                if (childband.Band.Columns.All.Where(col => col.ToString().Equals("ResultsID")).FirstOrDefault() != null)
                                    childband.Band.Columns["ResultsID"].Hidden = true;
                                if (childband.Band.Columns.All.Where(col => col.ToString().Equals("UserRole")).FirstOrDefault() != null)
                                    childband.Band.Columns["UserRole"].Hidden = true;
                                if (childband.Band.Columns.All.Where(col => col.ToString().Equals("RedImageData")).FirstOrDefault() != null)
                                    childband.Band.Columns["RedImageData"].Hidden = true;
                                if (childband.Band.Columns.All.Where(col => col.ToString().Equals("BlueImageData")).FirstOrDefault() != null)
                                    childband.Band.Columns["BlueImageData"].Hidden = true;
                                if (childband.Band.Columns.All.Where(col => col.ToString().Equals("RedBlueImageDifference")).FirstOrDefault() != null)
                                    childband.Band.Columns["RedBlueImageDifference"].Hidden = true;
                                if (childband.Band.Columns.All.Where(col => col.ToString().Equals("ResultImage")).FirstOrDefault() != null)
                                    childband.Band.Columns["ResultImage"].Hidden = true;
                                if (childband.Band.Columns.All.Where(col => col.ToString().Equals("TestResultImagePath")).FirstOrDefault() != null)
                                    childband.Band.Columns["TestResultImagePath"].Hidden = true;
                                if (childband.Rows.Count > 0)
                                {
                                    foreach (Infragistics.Win.UltraWinGrid.UltraGridCell cell in childband.Rows[0].Cells)
                                    {
                                        if (cell.Column.Key.Equals("TestPassed"))
                                        {
                                            if ((bool)cell.Value)
                                                cell.Appearance.BackColor = Color.LimeGreen;
                                            else
                                            {
                                                cell.Appearance.BackColor = Color.Red;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (row.Cells.Count > 0)
                        {
                            foreach (Infragistics.Win.UltraWinGrid.UltraGridCell cell in row.Cells)
                            {
                                if(cell.Column.Key.Equals("TestPassed"))
                                {
                                    if ((bool)cell.Value)
                                        cell.Appearance.BackColor = Color.LimeGreen;
                                    else
                                    {
                                        cell.Appearance.BackColor = Color.Red;
                                    }
                                }
                            }
                        }
                    }
                }
                Application.UseWaitCursor = false;
                UseWaitCursor = false;
                testResultsUltraGrid.DisplayLayout.Override.SelectTypeRow = SelectType.Single;
                testResultsUltraGrid.Rows[0].Activate();
                ultraGrid1_ClickCell(null, null);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                Application.UseWaitCursor = false;
                UseWaitCursor = false;
            }
        }
        private Bitmap azureTestResultImage(string azureTestResultImagePath)
        {
            try
            {
                //Application.UseWaitCursor = true;
                //Application.UseWaitCursor = true;
                //Bitmap azureTestResultImage;
                //string storageConnection = CloudConfigurationManager.GetSetting("StorageConnectionString");
                //CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConnection);
                //CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
                //string[] parts = azureTestResultImagePath.Split('/');               
                //string azureContainerName = parts[3];
                //string blobReference= parts[4];
                //CloudBlobContainer cloudBlobContainer = blobClient.GetContainerReference(azureContainerName);
                //cloudBlobContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                //CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(blobReference);
                //MemoryStream memStream = new MemoryStream();
                //blockBlob.DownloadToStream(memStream);
                //azureTestResultImage = new Bitmap(memStream);
                //return azureTestResultImage;
                return null;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                Application.UseWaitCursor = false;
                UseWaitCursor = false;
                return null;
            }
        }
        private void ultraGrid1_ClickCell(object sender, Infragistics.Win.UltraWinGrid.ClickCellEventArgs e)
        {
            try
            {
                if (testResultsUltraGrid.ActiveRow != null)
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridCell cell in testResultsUltraGrid.ActiveRow.Cells)
                    {
                        if (cell.Column.Key.ToString().Equals("TestResultImagePath"))
                        {
                            if (cell.Value != null && !string.IsNullOrWhiteSpace(cell.Value.ToString()))
                            {
                                if (ConfigurationManager.AppSettings["EndClient"] != "Thermo")
                                    testResultsUltraPictureBox.Image = cell.Value.Equals(null) ? null : new Bitmap(azureTestResultImage(cell.Value.ToString()));
                                else
                                    testResultsUltraPictureBox.Image = cell.Value.Equals(null) ? null : new Bitmap(parametersModel.TestResultsServerPath + cell.Value.ToString());
                            }
                            else
                            {
                                testResultsUltraPictureBox.Image = null;
                            }
                        }
                        else if (cell.Column.Key.ToString().Equals("DateExecuted"))
                        {
                            if (cell.Value != null)
                                printDocumentHeaderTitle = cell.Value.ToString();
                        }
                    }
                }                    
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                MessageBox.Show("Test result image not found");
                testResultsUltraPictureBox.Image = null;
            }
        }
        private void UpdateResultImagePictureBox()
        {
            try
            {
                if(testResultsUltraGrid.ActiveRow !=null)
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridCell cell in testResultsUltraGrid.ActiveRow.Cells)
                    {
                        if (cell.Column.Key.ToString().Equals("ResultImage"))
                        {
                            if (cell.Value != null)
                            {
                                testResultsUltraPictureBox.Image = (Image)cell.Value;
                            }
                            else
                            {
                                testResultsUltraPictureBox.Image = null;
                            }
                        }
                        else if (cell.Column.Key.ToString().Equals("DateExecuted"))
                        {
                            if (cell.Value != null)
                                printDocumentHeaderTitle = cell.Value.ToString();
                        }
                    }
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
                if (item.Name.Equals("SaveResultImage"))
                {
                        SaveResultsImage();
                }
                if (item.Name.Equals("PrintResultImage"))
                {
                        PrintResultsImage();
                }
                if (item.Name.Equals("PrintAll"))
                {
                        PrintResultsImage();
                    PrintGrid();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraPrintDocument1_PagePrinted(object sender, Infragistics.Win.Printing.PagePrintedEventArgs e)
        {
            try
            {
                //e.Graphics.DrawImage(resultImage, resultImage.Width,resultImage.Height);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void PrintGrid()
        {
            try
            {
                this.testResultsUltraGrid.PrintPreview(testResultsUltraGrid.DisplayLayout);               
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void  ExportDataGridToExcel()
        {
            try
            {
                        testResultsSaveFileDialog.InitialDirectory = Properties.Settings.Default["FilesLocation"].ToString();
                        string datetimestamp = DateTime.Now.ToString("MMddyyyy_hhmm");
                        testResultsSaveFileDialog.Title = "Save Test Results DataGrid to Excel";
                        testResultsSaveFileDialog.DefaultExt = "*.xlsx";
                        testResultsSaveFileDialog.Filter = "Excel|*.xlsx";
                        testResultsSaveFileDialog.FileName = testResultsUltraGrid.Text + "_" + datetimestamp;
                        if (testResultsSaveFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK && testResultsSaveFileDialog.FileName.Length > 0)
                        {
                            this.testResultsUltraGridExcelExporter.Export(this.testResultsUltraGrid, testResultsSaveFileDialog.FileName);
                        }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void PrintResultsImage()
        {
            try
            {
                testResultsUltraPrintPreviewDialog.Document = testResultsUltraPrintDocument;
                this.testResultsUltraPrintPreviewDialog.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void  SaveResultsImage()
        {
            try
            {
                testResultsSaveFileDialog.InitialDirectory = Properties.Settings.Default["FilesLocation"].ToString();
                string datetimestamp = DateTime.Now.ToString("MMddyyyy_hhmm");
                testResultsSaveFileDialog.Title = "Save Test Result Image";
                testResultsSaveFileDialog.DefaultExt = "*.jpg";
                testResultsSaveFileDialog.Filter = "Images|*.jpg";
                testResultsSaveFileDialog.FileName = testResultsUltraGrid.Text + "_" + datetimestamp;
                resultImage = (Image)testResultsUltraPictureBox.Image;
                if (testResultsSaveFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK && testResultsSaveFileDialog.FileName.Length > 0)
                {
                    resultImage.Save(testResultsSaveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void TestResultsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
        private void ultraPrintDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                // Get the image to print
                Image image = (System.Drawing.Image)this.testResultsUltraPictureBox.Image;
                // Get the Image Size
                Size imageSize = image.Size;
                // Get the starting X and Y based on the last portion
                // of the image that was printed.
                int startX = this.lastPrintX;
                int startY = this.lastPrintY;
                // Determine how much of the image remains to be printed
                // from the starting point.
                int remainingImageWidth = imageSize.Width - startX;
                int remainingImageHeight = imageSize.Height - startY;
                // These variables will keep track of whether the height or
                // width were clipped. This will help us determine if more
                // pages need to be printed.
                bool wasWidthClipped = false;
                bool wasHeightClipped = false;
                // Get the Size of the printable area of the page in Pixels.
                // MarginBounds returns the rect in hundredths of an inch.
                float scaleX = e.Graphics.DpiX / 100f;
                float scaleY = e.Graphics.DpiY / 100f;
                Rectangle printableRect = new Rectangle(
                    (int)(e.MarginBounds.X * scaleX),
                    (int)(e.MarginBounds.Y * scaleY),
                    (int)(e.MarginBounds.Width * scaleX),
                    (int)(e.MarginBounds.Height * scaleY)
                );
                // If the remaining image width is greater than
                // the width of the printable area of the page, clip it.
                if (remainingImageWidth > printableRect.Width)
                {
                    remainingImageWidth = printableRect.Width;
                    wasWidthClipped = true;
                }
                // If the remaining image height is greater than
                // the height of the printable area of the page, clip it.
                if (remainingImageHeight > printableRect.Height)
                {
                    remainingImageHeight = printableRect.Height;
                    wasHeightClipped = true;
                }
                // This rect will define a rect within the image that
                // is to be printed on the current page.
                Rectangle imagePrintRect = new Rectangle(startX, startY, remainingImageWidth, remainingImageHeight);
                // Print the image segment onto the page.
                //e.Graphics.DrawImage(image, e.MarginBounds.X,
                //  e.MarginBounds.Y, imagePrintRect, GraphicsUnit.Pixel);
                e.Graphics.DrawImage(image, e.MarginBounds);
                // Set up the variables for the next page
                if (wasWidthClipped)
                {
                    // If the Width was clipped, it means we need to
                    // increment lastPrintX.
                    this.lastPrintX += (remainingImageWidth + 1);
                    // Set HasMorePages to true so the UltraPrintDocument
                    // knows there is more to print.
                    e.HasMorePages = true;
                }
                else if (wasHeightClipped)
                {
                    // If the Width was not clipped, but the Height was,
                    // it means we need to move to the next line.
                    this.lastPrintX = 0;
                    this.lastPrintY += (remainingImageHeight + 1);
                    // Set HasMorePages to true so the UltraPrintDocument
                    // knows there is more to print.
                    e.HasMorePages = true;
                }
                //image.Dispose();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraPrintDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            testResultsUltraPrintDocument.DefaultPageSettings.Landscape = true;
            testResultsUltraPrintDocument.Header.TextLeft = "[User Name]";
            testResultsUltraPrintDocument.Header.TextCenter = "Final Test Report" + "_" + printDocumentHeaderTitle;
            testResultsUltraPrintDocument.Header.TextRight = "[Date Printed]";
            testResultsUltraPrintDocument.Footer.TextLeft = "[Time Printed]";
            testResultsUltraPrintDocument.Footer.TextRight = "[Page #]";
            this.testResultsUltraPrintDocument.PageBody.Margins.Top = 5;
            this.testResultsUltraPrintDocument.PageBody.Margins.Bottom = 5;
            this.testResultsUltraPrintDocument.Page.Padding.Left = 2;
            this.testResultsUltraPrintDocument.Page.Padding.Right = 2;
            this.testResultsUltraPrintDocument.Page.Padding.Top = 2;
            this.testResultsUltraPrintDocument.Page.Padding.Bottom = 2;
            // the appearance of the page will affect only the page itself
            this.testResultsUltraPrintDocument.Page.Appearance.BorderColor = Color.Black;
        }
        private void ultraGrid1_InitializePrintPreview(object sender, Infragistics.Win.UltraWinGrid.CancelablePrintPreviewEventArgs e)
        {
            e.DefaultLogicalPageLayoutInfo.FitWidthToPages = 1;
            e.DefaultLogicalPageLayoutInfo.PageHeader = testResultsUltraGrid.Text;
            e.DefaultLogicalPageLayoutInfo.PageHeaderAppearance.TextHAlign = HAlign.Center;
            e.DefaultLogicalPageLayoutInfo.PageHeaderBorderStyle = UIElementBorderStyle.Solid;
            e.DefaultLogicalPageLayoutInfo.PageFooter = "Page <#>.";
            e.DefaultLogicalPageLayoutInfo.PageFooterAppearance.TextHAlign = HAlign.Right;
            e.DefaultLogicalPageLayoutInfo.PageFooterBorderStyle = UIElementBorderStyle.Solid;
            e.DefaultLogicalPageLayoutInfo.ClippingOverride = ClippingOverride.Yes;
            e.PrintDocument.DefaultPageSettings.Landscape = true;
        }
        private async Task GetTestResults()
        {
            try
            {
                Application.UseWaitCursor = true;
                MeanVarianceLimitsPresenter mvLimits = new MeanVarianceLimitsPresenter();
                await mvLimits.PopulateMeanVarianceLimitsData();
                MeanVarianceTestLimitsDataList = new List<MeanVarianceTestLimitsData>();
                MeanVarianceTestLimitsDataList = mvLimits.MeanVarianceTestLimitsDataList;              
                await overAllTestResultsPresenter.GetOverAllTestResultsData();
                parametersModel = TestAppHelper.GetParametersModel();
                PopulateTestResults();
                //if (overAllTestResultsPresenter.OverAllTestResultsModelList.Count < 10)
                //{
                //    TopNumberRecords.Enabled = false;
                //    PopulateTestResults(overAllTestResultsPresenter.OverAllTestResultsModelList.Count);
                //}
                //else
                //{
                //    TopNumberRecords.Enabled = true;
                //    PopulateTestResults();
                //}
                //PopulateTestResults();
                Application.UseWaitCursor = false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                Application.UseWaitCursor = false;
            }
        }
        private void PopulateTestResults()
        {
            try
            {
                overAllTestResultsModelList = new List<OverAllTestResultsModel>();
                FromDate.CalendarInfo.MinDate = overAllTestResultsPresenter.OverAllTestResultsModelList.Last().DateExecuted.Date;
                FromDate.CalendarInfo.MaxDate = overAllTestResultsPresenter.OverAllTestResultsModelList.First().DateExecuted.Date;
                ToDate.CalendarInfo.MaxDate = overAllTestResultsPresenter.OverAllTestResultsModelList.First().DateExecuted.Date;
                ToDate.CalendarInfo.MinDate = overAllTestResultsPresenter.OverAllTestResultsModelList.Last().DateExecuted.Date;
                cameraSN = ultraComboEditor1.Text;
                foreach (OverAllTestResultsModel otm in overAllTestResultsPresenter.OverAllTestResultsModelList)
                {
                    otm.CameraSerialNumber = otm.CameraDetails.ToList()[0].CameraSerialNumber;
                    otm.ImagerSerialNumber = otm.CameraDetails.ToList()[0].ImagerSerialNumber;
                    foreach (MeanVarianceTestLimitsData limit in MeanVarianceTestLimitsDataList)
                    {
                        if( otm.ECONumber!=null)
                        {
                            if (otm.ECONumber.Equals(limit.ECONumber))
                            {
                                otm.LimitsID = limit.LimitsID;
                            }
                        }
                    }
                    overAllTestResultsModelList.Add(otm); 
                }
                OverallModelList = new List<OverAllTestResultsModel>();
               
                //no filter
                if (string.IsNullOrWhiteSpace(cameraSN) && fromDate == null && toDate == null && string.IsNullOrWhiteSpace(FromDate.Text) && string.IsNullOrWhiteSpace(ToDate.Text))
                {
                    OverallModelList = overAllTestResultsModelList;
                }
                // filter only by cameraSN
                else if (!string.IsNullOrWhiteSpace(cameraSN) && (fromDate == null && toDate == null && string.IsNullOrWhiteSpace(FromDate.Text) && string.IsNullOrWhiteSpace(ToDate.Text)))
                {
                    OverallModelList = overAllTestResultsModelList.Where(testResults => testResults.CameraSerialNumber.ToUpper().Equals(cameraSN.ToUpper())).ToList();
                }
                //filter by cameraSN, fromDate and ToDate
                else if (!string.IsNullOrWhiteSpace(cameraSN) && fromDate != null && toDate != null && !string.IsNullOrWhiteSpace(FromDate.Text) && !string.IsNullOrWhiteSpace(ToDate.Text))
                {
                    OverallModelList = overAllTestResultsModelList.Where(testResults => testResults.CameraSerialNumber.ToUpper().Equals(cameraSN.ToUpper()) && testResults.DateExecuted.Date >= fromDate && testResults.DateExecuted <= toDate).ToList();
                }
               //filter by cameraSN and from date
                else if (!string.IsNullOrWhiteSpace(cameraSN) && fromDate != null && !string.IsNullOrWhiteSpace(FromDate.Text))
                {
                    OverallModelList = overAllTestResultsModelList.Where(testResults => testResults.CameraSerialNumber.ToUpper().Equals(cameraSN.ToUpper()) && testResults.DateExecuted.Date == fromDate).ToList();
                }
                //filter by cameraSN and to date
                else if (!string.IsNullOrWhiteSpace(cameraSN) && toDate != null && !string.IsNullOrWhiteSpace(ToDate.Text))
                {
                    OverallModelList = overAllTestResultsModelList.Where(testResults => testResults.CameraSerialNumber.ToUpper().Equals(cameraSN.ToUpper()) && testResults.DateExecuted.Date == toDate).ToList();
                }
                //filter by from date and to date 
                else if (fromDate != null && !string.IsNullOrWhiteSpace(FromDate.Text) && toDate != null && !string.IsNullOrWhiteSpace(ToDate.Text))
                {
                    OverallModelList = (from testResults in overAllTestResultsModelList where testResults.DateExecuted.Date >= fromDate && testResults.DateExecuted.Date <= toDate select testResults).ToList();                   
                }
                // filter by from date
                else if (fromDate != null && !string.IsNullOrWhiteSpace(FromDate.Text))
                {
                    OverallModelList = (from testResults in overAllTestResultsModelList where testResults.DateExecuted.Date >= fromDate select testResults).ToList();
                }
                // filter by to date
                else if (toDate != null && !string.IsNullOrWhiteSpace(ToDate.Text))
                {
                    OverallModelList = (from testResults in overAllTestResultsModelList where testResults.DateExecuted.Date >= toDate select testResults).ToList();
                }
                ultraComboEditor1.DataSource = (from camerasns in OverallModelList orderby camerasns.CameraSerialNumber select camerasns.CameraSerialNumber).Distinct().ToList();
                if (OverallModelList.Count < 10)
                {
                    TopNumberRecords.Enabled = false;                    
                }
                else
                {
                    TopNumberRecords.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateMeanVarianceResultsGrid()
        {
            try
            {
                testResultsUltraGrid.Text = "Mean Variance Test Results";
                List<MeanVarianceTestResultsData> meanVarianceTestResultsDataList = new List<MeanVarianceTestResultsData>();
                foreach (OverAllTestResultsModel otm in OverallModelList)
                {
                    if (otm.MeanVarinaceTestResults.Count > 0)
                    {
                        MeanVarianceTestResultsData mvt = new MeanVarianceTestResultsData();
                        mvt.ResultsID = otm.MeanVarinaceTestResults.ToList()[0].ResultsID;
                        mvt.DateExecuted = otm.MeanVarinaceTestResults.ToList()[0].DateExecuted;
                        mvt.UserExecuted = otm.MeanVarinaceTestResults.ToList()[0].UserExecuted;
                        mvt.UserRole = otm.UserRole;
                        mvt.CorrectedMean = otm.MeanVarinaceTestResults.ToList()[0].CorrectedMean;
                        mvt.CorrectedVariance = otm.MeanVarinaceTestResults.ToList()[0].CorrectedVariance;
                        mvt.LinearCurveFit = otm.MeanVarinaceTestResults.ToList()[0].LinearCurveFit;
                        mvt.Gain = otm.MeanVarinaceTestResults.ToList()[0].Gain;
                        mvt.TestPassed = otm.MeanVarinaceTestResults.ToList()[0].TestPassed;
                        mvt.TestResultImagePath = otm.MeanVarinaceTestResults.ToList()[0].TestResultImagePath;
                        mvt.TestResultsID = otm.TestResultsID;
                        mvt.ECONumber = otm.ECONumber;
                        mvt.CameraSerialNumber = otm.CameraDetails.ToList()[0].CameraSerialNumber;
                        mvt.ImagerSerialNumber = otm.CameraDetails.ToList()[0].ImagerSerialNumber;
                        meanVarianceTestResultsDataList.Add(mvt);
                    }
                }
                testResultsUltraGrid.DataSource = meanVarianceTestResultsDataList;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateDarkCurrentResultsGrid()
        {
            try
            {
                testResultsUltraGrid.Text = "Dark Current Test Results";
                List<DarkCurrentTestResults> darkCurrentTestResultsList = new List<DarkCurrentTestResults>();
                foreach (OverAllTestResultsModel otm in OverallModelList)
                {
                    if (otm.DarkCurrentTestResults.Count > 0)
                    {
                        DarkCurrentTestResults mvt = new DarkCurrentTestResults();
                        mvt.ResultsID = otm.DarkCurrentTestResults.ToList()[0].ResultsID;
                        mvt.DateExecuted = otm.DarkCurrentTestResults.ToList()[0].DateExecuted;
                        mvt.UserExecuted = otm.DarkCurrentTestResults.ToList()[0].UserExecuted;
                        mvt.UserRole = otm.UserRole;
                        mvt.DarkCurrentMean = otm.DarkCurrentTestResults.ToList()[0].DarkCurrentMean;
                        mvt.PRNUDefects = otm.DarkCurrentTestResults.ToList()[0].PRNUDefects;
                        mvt.TrapDefects = otm.DarkCurrentTestResults.ToList()[0].TrapDefects;
                        mvt.HotPixelDefects = otm.DarkCurrentTestResults.ToList()[0].HotPixelDefects;
                        mvt.ClusterDefects = otm.DarkCurrentTestResults.ToList()[0].ClusterDefects;
                        mvt.TotalDefects = otm.DarkCurrentTestResults.ToList()[0].TotalDefects;
                        mvt.MaxDarkFF = otm.DarkCurrentTestResults.ToList()[0].MaxDarkFF;
                        mvt.AveDarkFF = otm.DarkCurrentTestResults.ToList()[0].AveDarkFF;
                        mvt.MaxTrap = otm.DarkCurrentTestResults.ToList()[0].MaxTrap;
                        mvt.AveTrap = otm.DarkCurrentTestResults.ToList()[0].AveTrap;
                        mvt.MaxDarkROI = otm.DarkCurrentTestResults.ToList()[0].MaxDarkROI;
                        mvt.DeadPixels = otm.DarkCurrentTestResults.ToList()[0].DeadPixels;
                        mvt.BadColumns = otm.DarkCurrentTestResults.ToList()[0].BadColumns;
                        mvt.BadRows = otm.DarkCurrentTestResults.ToList()[0].BadRows;
                        mvt.DefectsMean = otm.DarkCurrentTestResults.ToList()[0].DefectsMean;
                        mvt.TestPassed = otm.DarkCurrentTestResults.ToList()[0].TestPassed;
                        mvt.TestResultImagePath = otm.DarkCurrentTestResults.ToList()[0].TestResultImagePath;
                        mvt.ECONumber = otm.ECONumber;
                        mvt.TestResultsID = otm.TestResultsID;
                        mvt.CameraSerialNumber = otm.CameraDetails.ToList()[0].CameraSerialNumber;
                        mvt.ImagerSerialNumber = otm.CameraDetails.ToList()[0].ImagerSerialNumber;
                        darkCurrentTestResultsList.Add(mvt);
                    }
                }
                testResultsUltraGrid.DataSource = darkCurrentTestResultsList;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateDefectsResultsGrid()
        {
            try
            {
                testResultsUltraGrid.Text = "Defects Test Results";
                List<DefectsTestResults> defectsTestResultsList = new List<DefectsTestResults>();
                foreach (OverAllTestResultsModel otm in OverallModelList)
                {
                    if (otm.DefectsTestResults.Count > 0)
                    {
                        DefectsTestResults mvt = new DefectsTestResults();
                        mvt.ResultsID = otm.DefectsTestResults.ToList()[0].ResultsID;
                        mvt.DateExecuted = otm.DefectsTestResults.ToList()[0].DateExecuted;
                        mvt.UserExecuted = otm.DefectsTestResults.ToList()[0].UserExecuted;
                        mvt.UserRole = otm.UserRole;
                        mvt.PRNUDefects = otm.DefectsTestResults.ToList()[0].PRNUDefects;
                        mvt.TrapDefects = otm.DefectsTestResults.ToList()[0].TrapDefects;
                        mvt.HotPixelDefects = otm.DefectsTestResults.ToList()[0].HotPixelDefects;
                        mvt.ClusterDefects = otm.DefectsTestResults.ToList()[0].ClusterDefects;

                        //string[] defectPixelPerROI = otm.DefectsTestResults.ToList()[0].DefectivePixelsPerROICount;//.Split(new string[] { "Mask ROI" }, StringSplitOptions.None);
                        mvt.DefectivePixelsPerROICount = otm.DefectsTestResults.ToList()[0].DefectivePixelsPerROICount;
                        mvt.DefPixInMaskROI = otm.DefectsTestResults.ToList()[0].DefPixInMaskROI;
                        mvt.DeadPixelDefects = otm.DefectsTestResults.ToList()[0].DeadPixelDefects;
                        mvt.DriftROICount = otm.DefectsTestResults.ToList()[0].DriftROICount;
                        mvt.DarkPixelDefects = otm.DefectsTestResults.ToList()[0].DarkPixelDefects;
                        mvt.DarkColumns = otm.DefectsTestResults.ToList()[0].DarkColumns;
                        mvt.DarkRows = otm.DefectsTestResults.ToList()[0].DarkRows;
                        mvt.LightRows = otm.DefectsTestResults.ToList()[0].LightRows;
                        mvt.LightColumns = otm.DefectsTestResults.ToList()[0].LightColumns;
                        mvt.MeanDefects = otm.DefectsTestResults.ToList()[0].MeanDefects;
                        mvt.TestPassed = otm.DefectsTestResults.ToList()[0].TestPassed;
                        mvt.TestResultImagePath = otm.DefectsTestResults.ToList()[0].TestResultImagePath;
                        mvt.ECONumber = otm.ECONumber;
                        mvt.TestResultsID = otm.TestResultsID;
                        mvt.CameraSerialNumber = otm.CameraDetails.ToList()[0].CameraSerialNumber;
                        mvt.ImagerSerialNumber = otm.CameraDetails.ToList()[0].ImagerSerialNumber;
                        defectsTestResultsList.Add(mvt);
                    }
                }
                testResultsUltraGrid.DataSource = defectsTestResultsList;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateInjectionPerformaceResultsResultsGrid()
        {
            try
            {
                testResultsUltraGrid.Text = "Injection Efficiency Test Results";
                List<InjectionEfficiencyTestResults> injectionPerformanceTestResultsList = new List<InjectionEfficiencyTestResults>();
                foreach (OverAllTestResultsModel otm in OverallModelList)
                {
                    if (otm.InjectionEfficiencyTestResults.Count > 0)
                    {
                        InjectionEfficiencyTestResults mvt = new InjectionEfficiencyTestResults();
                        mvt.ResultsID = otm.InjectionEfficiencyTestResults.ToList()[0].ResultsID;
                        mvt.DateExecuted = otm.InjectionEfficiencyTestResults.ToList()[0].DateExecuted;
                        mvt.UserExecuted = otm.InjectionEfficiencyTestResults.ToList()[0].UserExecuted;
                        mvt.UserRole = otm.UserRole;
                        mvt.InitialExposure = otm.InjectionEfficiencyTestResults.ToList()[0].InitialExposure;
                        mvt.FirstInjection = otm.InjectionEfficiencyTestResults.ToList()[0].FirstInjection;
                        mvt.SecondInjection = otm.InjectionEfficiencyTestResults.ToList()[0].SecondInjection;
                        mvt.ThirdInjection = otm.InjectionEfficiencyTestResults.ToList()[0].ThirdInjection;
                        mvt.FourthInjection = otm.InjectionEfficiencyTestResults.ToList()[0].FourthInjection;
                        mvt.LastInjection = otm.InjectionEfficiencyTestResults.ToList()[0].LastInjection;
                        mvt.Exp0PlotData = otm.InjectionEfficiencyTestResults.ToList()[0].Exp0PlotData;
                        mvt.Exp1PlotData = otm.InjectionEfficiencyTestResults.ToList()[0].Exp1PlotData;
                        mvt.Exp2PlotData = otm.InjectionEfficiencyTestResults.ToList()[0].Exp2PlotData;
                        mvt.Exp3PlotData = otm.InjectionEfficiencyTestResults.ToList()[0].Exp3PlotData;
                        mvt.Exp4PlotData = otm.InjectionEfficiencyTestResults.ToList()[0].Exp4PlotData;
                        mvt.TestPassed = otm.InjectionEfficiencyTestResults.ToList()[0].TestPassed;
                        mvt.TestResultImagePath = otm.InjectionEfficiencyTestResults.ToList()[0].TestResultImagePath;
                        mvt.ECONumber = otm.ECONumber;
                        mvt.TestResultsID = otm.TestResultsID;
                        mvt.CameraSerialNumber = otm.CameraDetails.ToList()[0].CameraSerialNumber;
                        mvt.ImagerSerialNumber = otm.CameraDetails.ToList()[0].ImagerSerialNumber;
                        injectionPerformanceTestResultsList.Add(mvt);
                    }
                }
                testResultsUltraGrid.DataSource = injectionPerformanceTestResultsList;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateNoiseVsNDROsResultsGrid()
        {
            try
            {
                testResultsUltraGrid.Text = "Noise Vs NDROs Test Results";
                List<NoiseVsNDROTestResults> noiseVsNDROTestResultsList = new List<NoiseVsNDROTestResults>();
                foreach (OverAllTestResultsModel otm in OverallModelList)
                {
                    if (otm.NoiseVsNDROTestResults.Count > 0)
                    {
                        NoiseVsNDROTestResults mvt = new NoiseVsNDROTestResults();
                        mvt.ResultsID = otm.NoiseVsNDROTestResults.ToList()[0].ResultsID;
                        mvt.DateExecuted = otm.NoiseVsNDROTestResults.ToList()[0].DateExecuted;
                        mvt.UserExecuted = otm.NoiseVsNDROTestResults.ToList()[0].UserExecuted;
                        mvt.UserRole = otm.UserRole;
                        mvt.ExposureNDRO = otm.NoiseVsNDROTestResults.ToList()[0].ExposureNDRO;
                        mvt.NoiseRatio = otm.NoiseVsNDROTestResults.ToList()[0].NoiseRatio;
                        mvt.Noise = otm.NoiseVsNDROTestResults.ToList()[0].Noise;
                        mvt.R2Correlation = otm.NoiseVsNDROTestResults.ToList()[0].R2Correlation;
                        mvt.BestFit = otm.NoiseVsNDROTestResults.ToList()[0].BestFit;
                        mvt.BestFitPower = otm.NoiseVsNDROTestResults.ToList()[0].BestFitPower;
                        mvt.TheoryValue = otm.NoiseVsNDROTestResults.ToList()[0].TheoryValue;
                        mvt.SignalReadNoise = otm.NoiseVsNDROTestResults.ToList()[0].SignalReadNoise;
                        mvt.NDRONoise = otm.NoiseVsNDROTestResults.ToList()[0].NDRONoise;
                        mvt.TestPassed = otm.NoiseVsNDROTestResults.ToList()[0].TestPassed;
                        mvt.TestResultImagePath = otm.NoiseVsNDROTestResults.ToList()[0].TestResultImagePath;
                        mvt.ECONumber = otm.ECONumber;
                        mvt.TestResultsID = otm.TestResultsID;
                        mvt.CameraSerialNumber = otm.CameraDetails.ToList()[0].CameraSerialNumber;
                        mvt.ImagerSerialNumber = otm.CameraDetails.ToList()[0].ImagerSerialNumber;
                        noiseVsNDROTestResultsList.Add(mvt);
                    }
                }
                testResultsUltraGrid.DataSource = noiseVsNDROTestResultsList;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdatePhotoresponseResultsGrid()
        {
            try
            {
                testResultsUltraGrid.Text = "Photoresponse Test Results";
                List<PhotoresponseTestResults> photoresponseTestResultsList = new List<PhotoresponseTestResults>();
                foreach (OverAllTestResultsModel otm in OverallModelList)
                {
                    if (otm.PhotoresponseTestResults.Count > 0)
                    {
                        PhotoresponseTestResults mvt = new PhotoresponseTestResults();
                        mvt.ResultsID = otm.PhotoresponseTestResults.ToList()[0].ResultsID;
                        mvt.DateExecuted = otm.PhotoresponseTestResults.ToList()[0].DateExecuted;
                        mvt.UserExecuted = otm.PhotoresponseTestResults.ToList()[0].UserExecuted;
                        mvt.UserRole = otm.UserRole;
                        mvt.Mean = otm.PhotoresponseTestResults.ToList()[0].Mean;
                        mvt.LinearCurveFit = otm.PhotoresponseTestResults.ToList()[0].LinearCurveFit;
                        mvt.LinearFitData = otm.PhotoresponseTestResults.ToList()[0].LinearFitData;
                        mvt.FullSaturation = otm.PhotoresponseTestResults.ToList()[0].FullSaturation;
                        mvt.LinearSaturation = otm.PhotoresponseTestResults.ToList()[0].LinearSaturation;
                        mvt.Slope = otm.PhotoresponseTestResults.ToList()[0].Slope;
                        mvt.Intercept = otm.PhotoresponseTestResults.ToList()[0].Intercept;
                        mvt.DerivPlot = otm.PhotoresponseTestResults.ToList()[0].DerivPlot;
                        mvt.TestPassed = otm.PhotoresponseTestResults.ToList()[0].TestPassed;
                        mvt.TestResultImagePath = otm.PhotoresponseTestResults.ToList()[0].TestResultImagePath;
                        mvt.ECONumber = otm.ECONumber;
                        mvt.TestResultsID = otm.TestResultsID;
                        mvt.CameraSerialNumber = otm.CameraDetails.ToList()[0].CameraSerialNumber;
                        mvt.ImagerSerialNumber = otm.CameraDetails.ToList()[0].ImagerSerialNumber;
                        photoresponseTestResultsList.Add(mvt);
                    }
                }
                testResultsUltraGrid.DataSource = photoresponseTestResultsList;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateRedBlueResultsGrid()
        {
            try
            {
                testResultsUltraGrid.Text = "Red, Blue & UV Test Results";
                List<RedBlueTestResults> redBlueTestResultsList = new List<RedBlueTestResults>();
                foreach (OverAllTestResultsModel otm in OverallModelList)
                {
                    if(otm.RedBlueTestResults.Count > 0)
                    {
                        RedBlueTestResults mvt = new RedBlueTestResults();
                        mvt.ResultsID = otm.RedBlueTestResults.ToList()[0].ResultsID;
                        mvt.DateExecuted = otm.RedBlueTestResults.ToList()[0].DateExecuted;
                        mvt.UserExecuted = otm.RedBlueTestResults.ToList()[0].UserExecuted;
                        mvt.UserRole = otm.UserRole;
                        mvt.RedImageData = otm.RedBlueTestResults.ToList()[0].RedImageData;
                        mvt.BlueImageData = otm.RedBlueTestResults.ToList()[0].BlueImageData;
                        mvt.RedBlueImageDifference = otm.RedBlueTestResults.ToList()[0].RedBlueImageDifference;
                        mvt.TestPassed = otm.RedBlueTestResults.ToList()[0].TestPassed;
                        mvt.TestResultImagePath = otm.RedBlueTestResults.ToList()[0].TestResultImagePath;
                        mvt.ECONumber = otm.ECONumber;
                        mvt.TestResultsID = otm.TestResultsID;
                        mvt.CameraSerialNumber = otm.CameraDetails.ToList()[0].CameraSerialNumber;
                        mvt.ImagerSerialNumber = otm.CameraDetails.ToList()[0].ImagerSerialNumber;
                        mvt.UVMean = otm.RedBlueTestResults.ToList()[0].UVMean;
                        redBlueTestResultsList.Add(mvt);
                    }
                }
                testResultsUltraGrid.DataSource = redBlueTestResultsList;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void UpdateShutterDriveResultsGrid()
        {
            try
            {
                testResultsUltraGrid.Text = "ShutterDrive Test Results";
                List<ShutterDriveTestResults> shutterDriveTestResultsList = new List<ShutterDriveTestResults>();
                foreach (OverAllTestResultsModel otm in OverallModelList)
                {                    
                    if (otm.ShutterDriveTestResults.Count > 0)
                    {
                        ShutterDriveTestResults mvt = new ShutterDriveTestResults();
                        mvt.ResultsID = otm.ShutterDriveTestResults.ToList()[0].ResultsID;
                        mvt.DateExecuted = otm.ShutterDriveTestResults.ToList()[0].DateExecuted;
                        mvt.UserExecuted = otm.ShutterDriveTestResults.ToList()[0].UserExecuted;
                        mvt.UserRole = otm.UserRole;
                        mvt.TestPassed = otm.ShutterDriveTestResults.ToList()[0].TestPassed;
                        mvt.TestResultImagePath = otm.ShutterDriveTestResults.ToList()[0].TestResultImagePath;
                        mvt.LabJackSN = otm.ShutterDriveTestResults.ToList()[0].LabJackSN;
                        mvt.ShutterPlot = otm.ShutterDriveTestResults.ToList()[0].ShutterPlot;
                        mvt.ECONumber = otm.ECONumber;
                        mvt.TestResultsID = otm.TestResultsID;
                        mvt.CameraSerialNumber = otm.CameraDetails.ToList()[0].CameraSerialNumber;
                        mvt.ImagerSerialNumber = otm.CameraDetails.ToList()[0].ImagerSerialNumber;
                        shutterDriveTestResultsList.Add(mvt);
                    }
                }
                testResultsUltraGrid.DataSource = shutterDriveTestResultsList;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void buttonApply_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(FromDate.Text))
                    fromDate = (DateTime)FromDate.Value;
                if (!string.IsNullOrWhiteSpace(ToDate.Text))
                    toDate = (DateTime)ToDate.Value;
                UseWaitCursor = true;
                PopulateTestResults();
                if (OverallModelList != null)
                    UpdateResultsGrid(testResultsUltraGrid.Text.ToString());
                UseWaitCursor = false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                UseWaitCursor = false;
            }
        }       
        private async void ClearFilter_Click(object sender, EventArgs e)
        {
            try
            {
                fromDate = null;
                toDate = null;
                FromDate.Value = null;
                ToDate.Value = null;
                ultraComboEditor1.SelectedItem = null;
                ultraComboEditor1.Text = string.Empty;
                TopNumberRecords.SelectedIndex = 0;
                TopNumberRecords.Enabled = true;
                cameraSN = string.Empty;
                await GetTestResults();
                if (OverallModelList != null)
                    UpdateResultsGrid(testResultsUltraGrid.Text.ToString());
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraGrid1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Up  || e.KeyCode == Keys.Down)
                {
                    ultraGrid1_ClickCell(null, null);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ultraComboEditor1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (ultraComboEditor1.SelectedItem != null)
                    cameraSN = string.IsNullOrWhiteSpace(ultraComboEditor1.SelectedItem.DisplayText) ? string.Empty : ultraComboEditor1.SelectedItem.DisplayText;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
