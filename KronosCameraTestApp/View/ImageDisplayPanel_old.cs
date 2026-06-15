using KronosCameraTestApp.Helpers;
using KronosCameraTestApp.Model;
using KronosCameraTestApp.Presenter;
using NationalInstruments.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thermo.Kronos.Instrument.Camera.Interface;

namespace KronosCameraTestApp.View
{
    public partial class ImageDisplayPanel : Form
    {
        public ImageDisplayPanelPresenter imageDisplayPanelPresenter { get; set; }

        // The once-per-class call to initialize the log object
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private CIDInterface cidInterface = null;
        public int currentRecord = 0;
        public int currentSubarrayRecord = 0;
        private string userMode = string.Empty;
        bool engineeringLoginStatus = false;
        private bool plotCursorMoved = false;
        private double[,] zData = new double[xArraySize, yArraySize];
        private bool plotColumnGraphEnabled = false;
        private bool plotRowGraphEnabled = false;
        // Plot graph objects
        private static uint xArraySize = 2048;
        private static uint yArraySize = 2048;
        private double[] plotRowWaveYData = new double[xArraySize];
        private double[] plotColWaveYData = new double[yArraySize];

        // Rubber-band objects
        bool isDrag = false;
        Rectangle theRectangle = new Rectangle
            (new Point(0, 0), new Size(0, 0));
        Rectangle myRect = new Rectangle
            (new Point(0, 0), new Size(0, 0));
        Rectangle oldRectangle;
        Point startPoint;
        Point e_StartPoint;
        Point e_EndPoint;
        int recWidth;
        int recHeight;
        private bool loadContextMenu = false;
        private bool mouseMoved = false;
        private bool loadContextMenuPlots = false;
        SCM5821AX1DataTypes.RegionStructure roiRegion;
        string roiRegionName;
        private int zoomPanIndex = 0;
        private float zoomFactor = 1.5F;
        private bool zooming = false;

        double photometrySum, photometryMin, photometryMax;
        PointF photometryMinPoint = new PointF();
        PointF photometryMaxPoint = new PointF();
        PointF excelMinPoint = new PointF();
        PointF excelMaxPoint = new PointF();

        // ROI objects
        private double[,] roiPlotData;
        private int roiStartX, roiStartY, roiEndX, roiEndY;

        double minX = 0;
        double minY = 0;
        double maxX = 0;
        double maxY = 0;
        double[] xPlotData;
        double[] yPlotData;
        private double[] linearityData;
        private double[] mean;
        private double[] variance;

        int numberOfPoints = 0;
        private int annotationIndex = 1;
        private int roiAnnotationIndex = 1;
        private bool contrastSlideResize = false;

        private struct SubarrayRegionStructure
        {
            public double StartX;
            public double StartY;
            public double EndX;
            public double EndY;
            public uint subarrayNumber;
        }
        private SubarrayRegionStructure currentSubarrayRegion = new SubarrayRegionStructure();

        private List<KeyValuePair<string, SubarrayRegionStructure>> subarrayRegionList =
         new List<KeyValuePair<string, SubarrayRegionStructure>>();

        Infragistics.Win.UltraWinStatusBar.UltraStatusBar statusBar = null;
        CancellationTokenSource cts { get; set; }
        public CIDInterface CidInterface
        {
            get { return cidInterface; }
            set { cidInterface = value; }
        }

        bool imageDisplayTestStatus = false;
        public bool ImageDisplayTestStatus
        {
            get { return imageDisplayTestStatus; }
            set { imageDisplayTestStatus = value; }
        }
        public ImageDisplayPanel()
        {
            InitializeComponent();
        }
        public ImageDisplayPanel(ImageDisplayPanelPresenter imageDisplayPanelPresenter, string UserMode, bool enggLoginStatus,
            CIDInterface cidInterface, Infragistics.Win.UltraWinStatusBar.UltraStatusBar statusBar)
        {
            try
            {
                InitializeComponent();
                currentSubarrayRecord = 0;
                currentRecord = 0;
                this.imageDisplayPanelPresenter = imageDisplayPanelPresenter;
                this.userMode = UserMode;
                this.engineeringLoginStatus = enggLoginStatus;
                this.cidInterface = cidInterface;
                this.statusBar = statusBar;
                intensityCursor1.LabelVisible = true;
                // Create a list object thats holds the Subarrays/ROI's graph dimension data
                subarrayRegionList = new List<KeyValuePair<string, SubarrayRegionStructure>>();
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void intensityGraph1_AfterMoveCursor(object sender, NationalInstruments.UI.AfterMoveIntensityCursorEventArgs e)
        {
            try
            {
                if (imageDisplayPanelPresenter != null)
                {


                    plotCursorMoved = true;
                    int xStart = 0, yStart = 0, xSize = 0, ySize = 0;
                    //zData=
                    // Get cursors current position
                    int xpos = Convert.ToInt32(intensityCursor1.XPosition);
                    int ypos = Convert.ToInt32(intensityCursor1.YPosition);

                    signalTextBox.Text = imageDisplayPanelPresenter.ZPatternCorrectedData[xpos, ypos].ToString();
                    xPositionTextBox.Text = xpos.ToString();
                    yPositionTextBox.Text = ypos.ToString();

                    // Row
                    xSize = (int)xArraySize;
                    plotRowWaveYData = new double[xSize];
                    int plotRowIndex = 0;
                    if (xStart < 0)
                        xStart = 0;
                    if (yStart < 0)
                        yStart = 0;

                    // for (int x = xStart; x < xStart + xSize; x++)
                    for (int x = 0; x < xArraySize; x++)
                    {
                        if (x < xArraySize)
                        {
                            plotRowWaveYData[plotRowIndex] = imageDisplayPanelPresenter.ZPatternCorrectedData[x, ypos];
                            plotRowIndex++;
                        }
                    }
                    if (plotRowWaveYData != null)
                        plotRowWaveGraph.PlotY(plotRowWaveYData);

                    //Column
                    ySize = (int)yArraySize;
                    plotColWaveYData = new double[ySize];
                    int plotColIndex = 0;
                    //for (int y = yStart; y < yStart + ySize; y++)
                    for (int y = 0; y < yArraySize; y++)
                    {
                        if (y < yArraySize)
                        {
                            plotColWaveYData[plotColIndex] = imageDisplayPanelPresenter.ZPatternCorrectedData[xpos, y];
                            plotColIndex++;
                        }
                    }
                    plotColWaveGraph.PlotX(plotColWaveYData);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void intensityGraph1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Right)
                {
                    if (intensityCursor1.XPosition <= 2046)
                    {
                        intensityCursor1.XPosition = intensityCursor1.XPosition + 1;
                    }
                    else
                    {
                        intensityCursor1.XPosition = 2047;
                    }
                }
                if (e.KeyCode == Keys.Left)
                {
                    if (intensityCursor1.XPosition >= 1)
                    {
                        intensityCursor1.XPosition = intensityCursor1.XPosition - 1;
                    }
                    else
                    {
                        intensityCursor1.XPosition = 0;
                    }
                }
                if (e.KeyCode == Keys.Down)
                {
                    if (intensityCursor1.YPosition <= 2046)
                    {
                        intensityCursor1.YPosition = intensityCursor1.YPosition + 1;
                    }
                    else
                    {
                        intensityCursor1.YPosition = 2047;
                    }
                }
                if (e.KeyCode == Keys.Up)
                {
                    if (intensityCursor1.YPosition >= 1)
                    {
                        intensityCursor1.YPosition = intensityCursor1.YPosition - 1;
                    }
                    else
                    {
                        intensityCursor1.YPosition = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void intensityGraph1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                // Create pen.
                Pen Pen = new Pen(Color.White, 2);


                // Draw rectangle to screen.
                e.Graphics.DrawRectangle(Pen, myRect);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void intensityGraph1_PlotAreaMouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                PointF mouseClickPoint = e.Location;
                double xValue, yValue;

                if (e.Button == MouseButtons.Right)
                {
                    isDrag = true;

                    Control control = (Control)sender;
                    ControlPaint.DrawReversibleFrame(oldRectangle, Color.Red, FrameStyle.Dashed);

                    // Calculate the startPoint by using PointToScreen 
                    startPoint = control.PointToScreen(new Point(e.X, e.Y));
                    e_StartPoint = new Point(e.X, e.Y);

                    // Get the Subarray Name from where the mouse click event occurred
                    mouseClickPoint = e.Location;


                    // Turn screen coordinate into a graph data value.
                    intensityPlot1.InverseMapDataPoint(intensityGraph1.PlotAreaBounds, mouseClickPoint,
                        out xValue, out yValue);

                    // Now find the Region this point is contained in
                    foreach (var sub in subarrayRegionList)
                    {
                        if (((xValue >= sub.Value.StartX) && (xValue <= sub.Value.EndX)) &&
                            ((yValue >= sub.Value.StartY) && (yValue <= sub.Value.EndY)))
                        {
                            // if there is a match, set the global values for plotting
                            currentSubarrayRegion = sub.Value;
                            roiRegionName = sub.Key;
                            loadContextMenu = true;

                        }
                    }
                }
                else
                {
                    // Move cursor to mouse postion
                    intensityPlot1.InverseMapDataPoint(intensityGraph1.PlotAreaBounds, mouseClickPoint,
                        out xValue, out yValue);

                    intensityCursor1.XPosition = xValue;
                    intensityCursor1.YPosition = yValue;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void intensityGraph1_PlotAreaMouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                // If the mouse is being dragged, 
                // undraw and redraw the rectangle as the mouse moves.
                if (e.Button == MouseButtons.Right)
                {
                    if (isDrag)

                    // Hide the previous rectangle by calling the 
                    // DrawReversibleFrame method with the same parameters.
                    {
                        // Allow the subarray points to be calculated
                        mouseMoved = true;

                        ControlPaint.DrawReversibleFrame(theRectangle, Color.Red, FrameStyle.Dashed);

                        // Calculate the endpoint and dimensions for the new 
                        // rectangle, again using the PointToScreen method.
                        Point endPoint = ((Control)sender).PointToScreen(new Point(e.X, e.Y));
                        e_EndPoint = new Point(e.X, e.Y);

                        recWidth = endPoint.X - startPoint.X;
                        recHeight = endPoint.Y - startPoint.Y;
                        theRectangle = new Rectangle(startPoint.X, startPoint.Y, recWidth, recHeight);

                        // get values for getValueAtPoint()
                        myRect = theRectangle;

                        // Draw the new rectangle by calling DrawReversibleFrame
                        // again.                     
                        ControlPaint.DrawReversibleFrame(theRectangle, Color.Red, FrameStyle.Dashed);

                        int xpos = Convert.ToInt32(intensityCursor1.XPosition);
                        int ypos = Convert.ToInt32(intensityCursor1.YPosition);

                        signalTextBox.Text = zData[xpos, ypos].ToString();
                        xPositionTextBox.Text = xpos.ToString();
                        yPositionTextBox.Text = ypos.ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void intensityGraph1_PlotAreaMouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                // If the MouseUp event was after a Right click move
                if (e.Button == MouseButtons.Right)
                {


                    // If the MouseUp event occurs, the user is not dragging.
                    isDrag = false;

                    oldRectangle = new Rectangle(theRectangle.X, theRectangle.Y, theRectangle.Width, theRectangle.Height);

                    // Use an Annotation box, which is native rectangle object to NI Intensity graph,
                    // to outline region of interest
                    IntensityRangeAnnotation intensityRangeAnnotation =
                        new IntensityRangeAnnotation(this.intensityXAxis2, this.intensityYAxis2);

                    intensityRangeAnnotation.Caption = "Subarray_" + roiAnnotationIndex.ToString();
                    if (mouseMoved)
                    {
                        clearAnnotationsToolStripMenuItem();

                        // Increment index
                        roiAnnotationIndex++;

                        // Convert the dialog coordinates to Intensity graph region and get the plots.
                        getValueAtPoint(intensityRangeAnnotation.Caption);
                        mouseMoved = false;
                    }

                    intensityRangeAnnotation.RangeLineColor = Color.Red;
                    intensityRangeAnnotation.RangeLineWidth = 2;
                    intensityRangeAnnotation.RangeFillStyle = FillStyle.None;
                    if ((roiStartX < 200) || (roiStartY < 200))
                    {
                        intensityRangeAnnotation.ArrowHeadAlignment = BoundsAlignment.BottomRight;
                        intensityRangeAnnotation.ArrowTailAlignment = BoundsAlignment.Auto;
                    }
                    else
                    {
                        intensityRangeAnnotation.ArrowHeadAlignment = BoundsAlignment.BottomRight;
                    }

                    System.Drawing.Size sizeOfShape = new System.Drawing.Size();
                    sizeOfShape.Height = Math.Abs(theRectangle.Height);
                    sizeOfShape.Width = Math.Abs(theRectangle.Width);
                    if ((sizeOfShape.Height <= 0) || (sizeOfShape.Height <= 0))
                        return;

                    intensityRangeAnnotation.Visible = true;
                    //if (subarrayArrowsVisible)
                    //{
                    //    intensityRangeAnnotation.CaptionVisible = true;
                    //    intensityRangeAnnotation.ArrowVisible = true;
                    //}
                    //else
                    //{
                    intensityRangeAnnotation.CaptionVisible = false;
                    intensityRangeAnnotation.ArrowVisible = false;
                    //}

                    // use the box as a capture tool, not a new subarray now.
                    intensityRangeAnnotation.CaptionVisible = false;
                    intensityRangeAnnotation.ArrowVisible = false;


                    double xValue, yValue, endX, endY;
                    PointF p = new PointF();

                    // Top left datapoint of the rectangle drawn
                    p.X = e_StartPoint.X;
                    p.Y = e_StartPoint.Y;

                    // Turn that coordinate into a data value.
                    intensityPlot1.InverseMapDataPoint(intensityGraph1.PlotAreaBounds, p,
                        out xValue, out yValue);

                    // Bottom right datapoint of the rectangle drawn
                    p.X = e_StartPoint.X + myRect.Width;
                    p.Y = e_StartPoint.Y + myRect.Height;

                    // Turn that coordinate into a data value.
                    intensityPlot1.InverseMapDataPoint(intensityGraph1.PlotAreaBounds, p,
                        out endX, out endY);

                    // sort the smaller and larger values
                    if (yValue > endY)
                    {
                        double temp = 0;
                        temp = endY;
                        endY = yValue;
                        yValue = temp;
                    }

                    if (xValue > endX)
                    {
                        double temp = 0;
                        temp = endX;
                        endX = xValue;
                        xValue = temp;
                    }

                    if ((sizeOfShape.Width > 0) && (sizeOfShape.Height > 0))
                    {
                        if (xValue != endX)
                            intensityRangeAnnotation.XRange = new Range(xValue, endX);
                        if (yValue != endY)
                            intensityRangeAnnotation.YRange = new Range(yValue, endY);

                        if (endY > 1800)
                        {
                            intensityRangeAnnotation.ArrowHeadAlignment = BoundsAlignment.TopLeft;
                            intensityRangeAnnotation.ArrowTailAlignment = BoundsAlignment.TopLeft;
                            intensityRangeAnnotation.CaptionAlignment = new AnnotationCaptionAlignment(BoundsAlignment.None, -75, -75);

                        }

                        if (endX > 1800)
                        {
                            intensityRangeAnnotation.ArrowHeadAlignment = BoundsAlignment.TopLeft;
                            intensityRangeAnnotation.ArrowTailAlignment = BoundsAlignment.TopLeft;
                            intensityRangeAnnotation.CaptionAlignment = new AnnotationCaptionAlignment(BoundsAlignment.None, -75, -75);
                        }

                        if ((endY > 1760) && (endX < 300))
                        {
                            intensityRangeAnnotation.ArrowHeadAlignment = BoundsAlignment.TopRight;
                            intensityRangeAnnotation.ArrowTailAlignment = BoundsAlignment.TopRight;
                            intensityRangeAnnotation.CaptionAlignment = new AnnotationCaptionAlignment(BoundsAlignment.None, 10, -75);

                        }

                        if ((endX > 1760) && (endY < 900))
                        {
                            intensityRangeAnnotation.ArrowHeadAlignment = BoundsAlignment.BottomLeft;
                            intensityRangeAnnotation.ArrowTailAlignment = BoundsAlignment.BottomLeft;
                            intensityRangeAnnotation.CaptionAlignment = new AnnotationCaptionAlignment(BoundsAlignment.None, -10, 75);
                        }


                        intensityGraph1.Annotations.Add(intensityRangeAnnotation);
                    }
                    theRectangle = Rectangle.Empty;
                    myRect = Rectangle.Empty;
                    startPoint = Point.Empty;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ImageDisplayPanel_Load(object sender, EventArgs e)
        {
            try
            {
                if(userMode=="Manufacturing")
                {
                    EnableReadOnlyControlsManufacturingMode();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Loading Image Display Panel", "Loading Form", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void EnableReadOnlyControlsManufacturingMode()
        {
            try
            {                
                ExposureSettingsGroupBox.Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void ImageDisplayPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
        private async void buttonRun_Click(object sender, EventArgs e)
        {
            try
            {
                if (cidInterface.IsConnected())
                {
                    //if (Convert.ToInt32(numericEditNumOfExposures.Value) == 0 || Convert.ToInt32(numericEditNumOfExposures.Value) < 0)
                    //{
                    //    MessageBox.Show("Exposure count should be greater than 0", "Invalid exposure count", MessageBoxButtons.OK,
                    //        MessageBoxIcon.Error);
                    //    return;
                    //}
                    // Instantiate the CancellationTokenSource.  
                    cts = new CancellationTokenSource();
                    TestAppHelper.currentRunningTest = "imageDisplay";
                    statusBar.Panels["TestProgress"].ProgressBarInfo.Value = 1;
                    statusBar.Panels["MarqueeStatus"].Text = "Image Test Processing....";
                    statusBar.Panels["MarqueeStatus"].MarqueeInfo.Start();
                    //((KronosTestAppMainForm)this.MdiParent).ExpCount = Convert.ToInt32(numericEditNumOfExposures.Value);
                    buttonRun.Enabled = false;
                    buttonAbort.Enabled = true;

                    intensityGraph1.ClearData();
                    intensityPlot1.ClearData();
                    plotColWaveGraph.ClearData();
                    plotRowWaveGraph.ClearData();

                    intensityGraphColorScale.Range = new Range(0, 65535);

                    imageDisplayTestStatus = true;

                    UpdateExposureDataModelFromView();

                    await RunImageDisplayTest(cts.Token);
                    //Plot Image Data
                    await PlotImageDisplayData(cts);

                    statusBar.Panels["TestProgress"].ProgressBarInfo.Value = 100;


                    if (cts.IsCancellationRequested)
                    {
                        statusBar.Panels["MarqueeStatus"].Text = "Exposure Aborted";
                    }
                    else
                    {
                        statusBar.Panels["MarqueeStatus"].Text = "Exposure Completed";
                    }
                    statusBar.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                    buttonAbort.Enabled = false;
                    buttonRun.Enabled = true;
                    imageDisplayTestStatus = false;
                }
                else
                {
                    log.Error("Firmware not running or camera not connected to host");
                    MessageBox.Show("Firmware not running or camera not connected to host", "Firmware not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public void ZoomIn()
        {
            try
            {
                if (zoomPanIndex < 8)
                {
                    intensityGraph1.ZoomAnimation = false;
                    intensityGraph1.ZoomXY(intensityGraph1.Plots[0],
                            intensityCursor1.XPosition - 100, intensityCursor1.YPosition - 100, 200, 200);

                    intensityGraph1.ZoomAroundPoint(zoomFactor);

                    // Set parameters/boundry to adjust the plot col&row waveform graphs
                    zoomPanIndex++;
                    int subarrayRegion = (70 / zoomPanIndex);
                    currentSubarrayRegion.StartX = intensityCursor1.XPosition - subarrayRegion;
                    currentSubarrayRegion.StartY = intensityCursor1.YPosition - subarrayRegion;
                    currentSubarrayRegion.EndX = intensityCursor1.XPosition + subarrayRegion;
                    currentSubarrayRegion.EndY = intensityCursor1.YPosition + subarrayRegion;
                    zooming = true;
                    //   zoomFactor = zoomFactor + 0.68F;
                    zoomFactor = zoomFactor + 1.5F;

                    intensityGraph1_AfterMoveCursor(null, null);

                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        public void ZoomOut()
        {
            try
            {
                if (zoomPanIndex <= 1)
                {
                    zooming = false;
                    zoomFactor = 1.5F;
                    zoomPanIndex = 0;

                    intensityGraph1.ResetZoomPan();
                    //plotColIntGraph.ResetZoomPan();
                    //plotRowIntGraph.ResetZoomPan();
                    plotColWaveGraph.ResetZoomPan();
                    plotRowWaveGraph.ResetZoomPan();

                }
                else
                {
                    //zoomFactor = zoomFactor - 0.75F;
                    zoomFactor = 0.87F;


                    intensityGraph1.ZoomAnimation = false;
                    intensityGraph1.ZoomAroundPoint(zoomFactor);

                    //intensityGraph1.UndoZoomPan();
                    // Set parameters/boundry to adjust the plot col&row waveform graphs                
                    int subarrayRegion = (70 / zoomPanIndex);
                    zoomPanIndex--;
                    currentSubarrayRegion.StartX = intensityCursor1.XPosition - subarrayRegion;
                    currentSubarrayRegion.StartY = intensityCursor1.YPosition - subarrayRegion;
                    currentSubarrayRegion.EndX = intensityCursor1.XPosition + subarrayRegion;
                    currentSubarrayRegion.EndY = intensityCursor1.YPosition + subarrayRegion;
                    zooming = true;
                }
                intensityGraph1_AfterMoveCursor(null, null);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async void buttonImageDisplayRun_Click(object sender, EventArgs e)
        {
            try
            {
                if (cidInterface.IsConnected())
                {
                    //if (Convert.ToInt32(numericEditNumOfExposures.Value) == 0 || Convert.ToInt32(numericEditNumOfExposures.Value) < 0)
                    //{
                    //    MessageBox.Show("Exposure count should be greater than 0", "Invalid exposure count", MessageBoxButtons.OK,
                    //        MessageBoxIcon.Error);
                    //    return;
                    //}
                    // Instantiate the CancellationTokenSource.  
                    cts = new CancellationTokenSource();
                    TestAppHelper.currentRunningTest = "imageDisplay";
                    statusBar.Panels["TestProgress"].ProgressBarInfo.Value = 1;
                    statusBar.Panels["MarqueeStatus"].Text = "Image Test Processing....";
                    statusBar.Panels["MarqueeStatus"].MarqueeInfo.Start();
                    //((KronosTestAppMainForm)this.MdiParent).ExpCount = Convert.ToInt32(numericEditNumOfExposures.Value);
                    buttonRun.Enabled = false;
                    buttonAbort.Enabled = true;

                    intensityGraph1.ClearData();
                    intensityPlot1.ClearData();
                    plotColWaveGraph.ClearData();
                    plotRowWaveGraph.ClearData();

                    intensityGraphColorScale.Range = new Range(0, 65535);

                    imageDisplayTestStatus = true;

                    UpdateExposureDataModelFromView();

                    await RunImageDisplayTest(cts.Token);
                    //Plot Image Data
                    await PlotImageDisplayData(cts);

                    statusBar.Panels["TestProgress"].ProgressBarInfo.Value = 100;


                    if (cts.IsCancellationRequested)
                    {
                        statusBar.Panels["MarqueeStatus"].Text = "Exposure Aborted";
                    }
                    else
                    {
                        statusBar.Panels["MarqueeStatus"].Text = "Exposure Completed";
                    }
                    statusBar.Panels["MarqueeStatus"].MarqueeInfo.Stop();
                    buttonAbort.Enabled = false;
                    buttonRun.Enabled = true;
                    imageDisplayTestStatus = false;
                }
                else
                {
                    log.Error("Firmware not running or camera not connected to host");
                    MessageBox.Show("Firmware not running or camera not connected to host", "Firmware not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;

                    //bool cameraConnected = cidInterface.Connect(ConfigurationManager.AppSettings["CameraIPAddresss"], Int32.Parse(ConfigurationManager.AppSettings["CameraIPPort"]));
                    //if (cameraConnected)
                    //{
                    //    ((KronosTestAppMainForm)this.MdiParent).ultraStatusBar1.Panels["CameraStatus"].Text = "CID821 Connected";
                    //    ((KronosTestAppMainForm)this.MdiParent).ultraStatusBar1.Panels["CameraStatus"].Appearance.BackColor = Color.LimeGreen;
                    //}
                    //else
                    //{
                    //    log.Error("Firmware not running or camera not connected to host");
                    //    MessageBox.Show("Firmware not running or camera not connected to host", "Firmware not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    return;
                    //}
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task RunImageDisplayTest(CancellationToken ct)
        {
            try
            {
                log.Debug("Run Image Display exposure.");
                if (ct.IsCancellationRequested)
                    return;

                await imageDisplayPanelPresenter.runImageTest(cidInterface, ct, 1);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private async Task PlotImageDisplayData(CancellationTokenSource ct)
        {
            try
            {
                if (ct.IsCancellationRequested)
                    return;
                await imageDisplayPanelPresenter.CalculateImageTestData(ct.Token);

                intensityGraph1.Plot(imageDisplayPanelPresenter.ZPatternCorrectedData);
                SetIntensityGraphContrast(imageDisplayPanelPresenter.ZPatternCorrectedData);
                //maxContrastSlide_AfterChangeValue(null, null);
                buttonRun.Enabled = true;
                buttonAbort.Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void SetIntensityGraphContrast(double[,] plotData)
        {
            double maximumSignal = 0;
            double minimumSignal = 65000;

            for (int x = 0; x < xArraySize; x++)
            {
                for (int y = 0; y < yArraySize; y++)
                {
                    if (plotData[x, y] > maximumSignal)
                    {
                        maximumSignal = plotData[x, y];
                    }
                    if ((plotData[x, y] < minimumSignal) && (plotData[x, y] != 0))
                    {
                        minimumSignal = plotData[x, y];
                    }
                }
            }

            if ((maximumSignal > 0) && (maximumSignal > minimumSignal))
            {
                Range contrastRange = new Range(minimumSignal, maximumSignal);
                minContrastSlide.Range = contrastRange;
                minContrastSlide.Value = contrastRange.Minimum;

                maxContrastSlide.Range = contrastRange;
                maxContrastSlide.Value = contrastRange.Maximum;

                intensityGraphColorScale.Range =
                   new NationalInstruments.UI.Range(minimumSignal, maximumSignal);
            }
        }
        private void UpdateExposureDataModelFromView()
        {
            try
            {
                ExposureDataModel ExposureDataModel = new ExposureDataModel();

                ExposureDataModel.ExposureID = 1;
                ExposureDataModel.TestDataID = "ImageDisplay";
                ExposureDataModel.UserModified = Environment.UserName;
                ExposureDataModel.GlobalInjectDelay = Convert.ToInt32(numericEditDelay.Value);
                ExposureDataModel.GlobalInject = Convert.ToInt32(numericEditGlobalInject.Value);

                //Exposure         
                ExposureDataModel.ExposureName = ultraTextEditorExposureName.Text.ToString();
                ExposureDataModel.ExposureInterval = Convert.ToInt32(numericEditExposureInterval.Value);
                ExposureDataModel.ExposureNDROS = Convert.ToInt32(numericEditNDRO.Value);
                ExposureDataModel.ExposureRegionXo = Convert.ToInt32(numericEditExposureStartX.Value);
                ExposureDataModel.ExposureRegiondX = Convert.ToInt32(numericEditExposureWidth.Value);
                ExposureDataModel.ExposureRegionYo = Convert.ToInt32(numericEditExposureStartY.Value);
                ExposureDataModel.ExposureRegiondY = Convert.ToInt32(numericEditExposureHeight.Value);
                ExposureDataModel.LED1Enabled = ultraCheckEditorLED1.Checked;
                ExposureDataModel.LED2Enabled = ultraCheckEditorLED2.Checked;
                ExposureDataModel.LED3Enabled = ultraCheckEditorLED3.Checked;
                ExposureDataModel.ShutterEnabled = ultraCheckEditorShutterEnabled.Checked;
                ExposureDataModel.LEDOnTime = Convert.ToInt32(numericEditLEDOn.Value);
                ExposureDataModel.LEDOffTime = Convert.ToInt32(numericEditLEDOff.Value);
                ExposureDataModel.LEDFlashes = Convert.ToInt32(numericEditFlash.Value);
                ExposureDataModel.NumberOfSubarrays = 1;

                if (ultraRadioButtonFPNNone.Checked == true)
                {
                    ExposureDataModel.ExposureFPN = 0;
                }
                else if (ultraRadioButtonFPNRunTime.Checked == true)
                {
                    ExposureDataModel.ExposureFPN = 1;
                }
                else if (ultraRadioButtonFPNStored.Checked == true)
                {
                    ExposureDataModel.ExposureFPN = 3;
                }

                ExposureDataModel.FullFrameEnabled = ultraCheckEditorExposedFrame.Checked;
                ExposureDataModel.DarkFrameEnabled = ultraCheckEditorDarkFrame.Checked;

                SubarrayDataModel subarrayDataModel = new SubarrayDataModel();

                //subarrayDataModel.SubarrayID = Convert.ToInt32(ultraTextEditorSubarrayID.Value);
                //subarrayDataModel.ExposureID = Convert.ToInt32(ultraTextEditorExposureID.Value);
                subarrayDataModel.UserModified = Environment.UserName;
                subarrayDataModel.SubarrayName = ultraTextEditorSubarrayName.Text.ToString();
                subarrayDataModel.SubarrayRegionXo = Convert.ToInt32(numericEditSubarrayStartX.Value);
                subarrayDataModel.SubarrayRegiondX = Convert.ToInt32(numericEditSubarrayWidth.Value);
                subarrayDataModel.SubarrayRegionYo = Convert.ToInt32(numericEditSubarrayStartY.Value);
                subarrayDataModel.SubarrayRegiondY = Convert.ToInt32(numericEditSubarrayHeight.Value);

                subarrayDataModel.SubarrayReadEnabled = ultraCheckEditorSubarrayRead.Checked;
                subarrayDataModel.SubarrayReadInterval = Convert.ToInt32(numericEditSubarrayReadInterval.Value);

                subarrayDataModel.SubarraySubinjectEnabled = ultraCheckEditorSubarraySubInject.Checked;
                subarrayDataModel.SubarraySubinjectInterval = Convert.ToInt32(numericEditSubarraySubinjectInterval.Value);

                subarrayDataModel.SubarrayPostReadEnabled = ultraCheckEditorSubarrayPostRead.Checked;
                subarrayDataModel.SubarrayPostReadNDROS = Convert.ToInt32(numericEditSubarrayNDROs.Value);

                subarrayDataModel.SubarrayThresholdRegionXo = Convert.ToInt32(numericEditSubarrayThresholdStartX.Value);
                subarrayDataModel.SubarrayThresholdRegiondX = Convert.ToInt32(numericEditSubarrayThresholdWidth.Value);
                subarrayDataModel.SubarrayThresholdRegionYo = Convert.ToInt32(numericEditSubarrayThresholdStartY.Value);
                subarrayDataModel.SubarrayThresholdRegiondY = Convert.ToInt32(numericEditSubarrayThresholdHeight.Value);
                subarrayDataModel.SubarrayThresholdEnabled = ultraCheckEditorEnableThreshold.Checked;
                subarrayDataModel.SubarrayThresholdPercent = Convert.ToInt32(numericEditThresholdPercent.Value);

                ExposureDataModel.SubarrayDatas = new List<SubarrayDataModel>();

                ExposureDataModel.SubarrayDatas.Add(subarrayDataModel);

                imageDisplayPanelPresenter.ExposureDataList = new List<Model.ExposureDataModel>();

                imageDisplayPanelPresenter.ExposureDataList.Add(ExposureDataModel);


            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void moveCursorToCenterOfPlot()
        {
            Range xRange = intensityCursor1.Plot.XAxis.Range;
            Range yRange = intensityCursor1.Plot.YAxis.Range;
            double xCenter = (xRange.Maximum + xRange.Minimum) / 2;
            double yCenter = (yRange.Maximum + yRange.Minimum) / 2;
            intensityCursor1.MoveCursor(xCenter, yCenter);
        }
        private void getValueAtPoint(string name)
        {
            double xValue, yValue, endX, endY;
            // double[] xMeanData;
            // double[] yVarianceData;

            PointF p = new PointF();

            // Top left data point of the rectangle drawn
            p.X = e_StartPoint.X;
            p.Y = e_StartPoint.Y;

            // Turn screen coordinate into a graph data value.
            intensityPlot1.InverseMapDataPoint(intensityGraph1.PlotAreaBounds, p,
                out xValue, out yValue);

            // Bottom right data point of the rectangle drawn
            p.X = e_StartPoint.X + myRect.Width;
            p.Y = e_StartPoint.Y + myRect.Height;

            // Turn screen coordinate into a graph data value.
            intensityPlot1.InverseMapDataPoint(intensityGraph1.PlotAreaBounds, p,
                out endX, out endY);

            photometrySum = 0;
            double patternCorrectedSum = 0;
            photometryMax = 0;
            photometryMin = 0;
            numberOfPoints = 0;

            // swap the smaller and larger values when operator draws region from right to left
            if (yValue > endY)
            {
                double temp = 0;
                temp = endY;
                endY = yValue;
                yValue = temp;
            }

            if (xValue > endX)
            {
                double temp = 0;
                temp = endX;
                endX = xValue;
                xValue = temp;
            }

            double[] sd = new double[((int)endX - (int)xValue) * ((int)endY - (int)yValue)];
            int index = 0;

            // Initialize the Min and Max points
            photometryMinPoint.X = (int)xValue;
            photometryMinPoint.Y = (int)yValue;
            photometryMaxPoint.X = (int)xValue;
            photometryMaxPoint.Y = (int)yValue;

            // build box for excel
            excelMinPoint.X = (int)xValue;
            excelMinPoint.Y = (int)yValue;
            excelMaxPoint.X = (int)endX;
            excelMaxPoint.Y = (int)endY;

            // Set Subarray bounds
            roiStartX = (int)xValue;
            roiEndX = (int)endX;
            roiStartY = (int)yValue;
            roiEndY = (int)endY;

            // Generate ROI plot data (subarray)
            roiPlotData = new double[(int)endX - (int)xValue, (int)endY - (int)yValue];
            int roiXIndex = 0;
            int roiYIndex = 0;

            // Add Subarray dimensions to list of subarray regions
            currentSubarrayRegion.StartX = xValue;
            currentSubarrayRegion.StartY = yValue;
            currentSubarrayRegion.EndX = endX;
            currentSubarrayRegion.EndY = endY;

            if (subarrayRegionList.Contains(new KeyValuePair<string, SubarrayRegionStructure>
                                       (name, currentSubarrayRegion)))
            {
                // only add subarray once
                //break;                                       
            }
            else
                subarrayRegionList.Add(new KeyValuePair<string, SubarrayRegionStructure>
                   (name, currentSubarrayRegion));

            bool initializeMin = true;

            int numX = (int)(xValue - (endX + 1));
            int numY = (int)(yValue - (endY + 1));
            numberOfPoints = numX * numY;

            // loop through all the points in a ROI
            for (int x = (int)xValue; x < (int)endX; x++)
            {
                roiYIndex = 0;
                // for (int y = (int)endY; y < yValue; y++)
                for (int y = (int)yValue; y < (int)endY; y++)
                {

                    // Build the ROI plot data array
                    roiPlotData[roiXIndex, roiYIndex] = zData[x, y];
                    roiYIndex++;
                    if (initializeMin)
                    {
                        photometryMin = zData[x, y];
                        initializeMin = false;
                    }

                    if (zData[x, y] > photometryMax)
                    {
                        photometryMax = zData[x, y];
                        photometryMaxPoint.X = x;
                        photometryMaxPoint.Y = y;
                        maxX = x;
                        maxY = y;
                    }
                    if (zData[x, y] < photometryMin)
                    {
                        photometryMin = zData[x, y];
                        photometryMinPoint.X = x;
                        photometryMinPoint.Y = y;
                        minX = x;
                        minY = y;
                    }
                    sd[index] = zData[x, y];

                    index++;
                    photometrySum = photometrySum + zData[x, y];
                    patternCorrectedSum = patternCorrectedSum + (zData[x, y]);
                }
                roiXIndex++;
            }

            // calculate the mean of x for each y
            xPlotData = new double[roiXIndex];
            double[] patternCorrectedData = new double[roiXIndex];

            mean = new double[roiYIndex];
            double[] patternCorrectedmean = new double[roiYIndex];
            for (int y = 0; y < roiYIndex; y++)
            {
                for (int x = 0; x < roiXIndex; x++)
                {
                    xPlotData[x] = roiPlotData[x, y];
                    patternCorrectedData[x] = roiPlotData[x, y];

                }
                mean[y] = NationalInstruments.Analysis.Math.Statistics.Mean(xPlotData);
                patternCorrectedmean[y] = NationalInstruments.Analysis.Math.Statistics.Mean(patternCorrectedData);
            }

            // calculate the variance of Y for each X
            yPlotData = new double[roiYIndex];
            variance = new double[roiXIndex];

            for (int x = 0; x < roiXIndex; x++)
            {
                for (int y = 0; y < roiYIndex; y++)
                {
                    yPlotData[y] = roiPlotData[x, y];
                }
                variance[x] = NationalInstruments.Analysis.Math.Statistics.Variance(yPlotData);
            }

            if (xPlotData.Length > 1)
                linearityData = NationalInstruments.Analysis.Math.CurveFit.LinearFit(xPlotData, xPlotData);

            double[] xData = new double[sd.Length];

            for (int X = 0; X < sd.Length; X++)
                xData[X] = X;

            // Generate the subarray region information from the drawn rectangle.
            roiRegion.dY = Convert.ToUInt16(endY - yValue + 1);
            roiRegion.dX = Convert.ToUInt16(endX - xValue + 1);
            roiRegion.Yo = (UInt16)yValue;
            roiRegion.Xo = (UInt16)xValue;
            roiRegionName = name;

            pixelsLabel.Text = "Pixels:" + numberOfPoints.ToString();
            if (numberOfPoints > 0)
            {
                photometryAvgLabel.Text = "Mean: " + (photometrySum / numberOfPoints).ToString("F1");
            }

            photometrySumLabel.Text = "Sum: " + photometrySum.ToString();
            nameLabel.Text = "Name:" + name;
            if (sd.Length > 0)
            {
                photometryMax = sd.Max();
                photometryMin = sd.Min();
            }

            photometryMinMaxLabel.Text = "Min/Max: " + "(" + photometryMin.ToString() + "," + photometryMax.ToString() + ")";
            if (sd.Length > 0)
                photometrySDLabel.Text = "Std Dev: " + NationalInstruments.Analysis.Math.Statistics.StandardDeviation(sd).ToString("F1");


            log.Debug(name + " Generated at Start X: " + Convert.ToInt32(currentSubarrayRegion.StartX).ToString() +
                " , End X: " + Convert.ToInt32(currentSubarrayRegion.EndX).ToString() + " , Start Y:" +
                Convert.ToInt32(currentSubarrayRegion.StartY).ToString() + " , End Y: " + Convert.ToInt32(currentSubarrayRegion.EndY).ToString());
        }
        public async Task ClearImageDisplayGraphs()
        {
            await Task.Run(() =>
            {
                this.intensityPlot1.ClearData();
            });
        }
        private void zoomOnSubarray(object sender, EventArgs e)
        {
            double end = currentSubarrayRegion.EndX;
            float regionWidth = (float)(currentSubarrayRegion.EndX - currentSubarrayRegion.StartX);
            float regionHeight = (float)(currentSubarrayRegion.EndY - currentSubarrayRegion.StartY);

            intensityGraph1.ZoomXY(intensityGraph1.Plots[0],
                currentSubarrayRegion.StartX, currentSubarrayRegion.StartY, regionWidth, regionHeight);

            zooming = true;
            moveCursorToCenterOfPlot();
        }
        private void clearAnnotationsToolStripMenuItem()
        {
            intensityGraph1.Annotations.Clear();
            annotationIndex = 1;
            intensityGraph1.ClearData();
            intensityPlot1.ClearData();
            plotColWaveGraph.ClearData();
            plotRowWaveGraph.ClearData();
            intensityGraphColorScale.Range = new Range(0, 65535);
        }
        private void contextMenuStripIntensityGraph_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                ToolStripItem item = e.ClickedItem;
                if (item.Name.Equals("toolStripMenuItemClearROIs"))
                {
                    clearAnnotationsToolStripMenuItem();
                }
                else if (item.Name.Equals("toolStripMenuItemZoom"))
                {
                    // build the 3D Surface graph form dialog                
                    zoomOnSubarray(sender, null);
                }
                else if (item.Name.Equals("toolStripMenuItemGoToMin"))
                {
                    // move the cursor to the minimum pixel signal value 

                    intensityCursor1.XPosition = minX;
                    intensityCursor1.YPosition = minY;
                }
                else if (item.Name.Equals("toolStripMenuItemGoToMax"))
                {
                    // move the cursor to the maximum pixel signal value 
                    intensityCursor1.XPosition = maxX;
                    intensityCursor1.YPosition = maxY;
                }
                else if (item.Name.Equals("toolStripMenuItemGenerateSubarray"))
                {
                    buildExposure();
                }                
                // Reset the flag that allows the menu to open
                loadContextMenu = false;
                loadContextMenuPlots = false;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void minContrastSlide_AfterChangeValue(object sender, NationalInstruments.UI.AfterChangeNumericValueEventArgs e)
        {
            try
            {
                double minColorScaleRange = minContrastSlide.Value;
                double maxColorScaleRange = maxContrastSlide.Value;

                if (maxContrastSlide.Value == minContrastSlide.Value)
                    return;

                if (minColorScaleRange < maxColorScaleRange)
                {
                    this.intensityGraphColorScale.Range =
                       new NationalInstruments.UI.Range(minColorScaleRange, maxColorScaleRange);

                    // Fixes a NI refresh bug/issue
                    if (contrastSlideResize)
                    {
                        intensityGraph1.Width = intensityGraph1.Width + 1;
                        intensityGraph1.Height = intensityGraph1.Height + 1;
                        contrastSlideResize = false;
                    }
                    else
                    {
                        intensityGraph1.Width = intensityGraph1.Width - 1;
                        intensityGraph1.Height = intensityGraph1.Height - 1;
                        contrastSlideResize = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }

        }
        private void maxContrastSlide_AfterChangeValue(object sender, NationalInstruments.UI.AfterChangeNumericValueEventArgs e)
        {
            try
            {
                double minColorScaleRange = minContrastSlide.Value;

                double maxColorScaleRange = maxContrastSlide.Value;

                if (maxContrastSlide.Value == minContrastSlide.Value)
                    return;

                if (minColorScaleRange < maxColorScaleRange)
                {
                    this.intensityGraphColorScale.Range =
                       new NationalInstruments.UI.Range(minColorScaleRange, maxColorScaleRange);

                    // Fixes a NI refresh bug/issue
                    if (contrastSlideResize)
                    {
                        intensityGraph1.Width = intensityGraph1.Width + 1;
                        intensityGraph1.Height = intensityGraph1.Height + 1;
                        contrastSlideResize = false;
                    }
                    else
                    {
                        intensityGraph1.Width = intensityGraph1.Width - 1;
                        intensityGraph1.Height = intensityGraph1.Height - 1;
                        contrastSlideResize = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }

        }
        private void buildExposure()
        {
            try
            { // Build exposure and subarray based on selected region
                //numericEditNumOfExposures.Value = 1;
                ultraTextEditorExposureName.Text = "Exposure_1";
                numericEditExposureInterval.Value = 1000;
                numericEditNDRO.Value = 1;
                numericEditExposureStartX.Value = 0;
                numericEditExposureStartY.Value = 0;
                numericEditExposureWidth.Value = 2048;
                numericEditExposureHeight.Value = 2048;
                ultraCheckEditorLED1.Checked = true;
                numericEditLEDOn.Value = 10000;
                numericEditLEDOff.Value = 100;
                numericEditFlash.Value = 10;

                // Subarray
                ultraTextEditorSubarrayName.Text = currentSubarrayRegion.subarrayNumber.ToString();
                ultraTextEditorSubarrayName.Text = roiRegionName;
                numericEditSubarrayStartX.Value = currentSubarrayRegion.StartX;
                numericEditSubarrayStartY.Value = currentSubarrayRegion.StartY;
                numericEditSubarrayWidth.Value = currentSubarrayRegion.EndX - currentSubarrayRegion.StartX;
                numericEditSubarrayHeight.Value = currentSubarrayRegion.EndY - currentSubarrayRegion.StartY;
                ultraCheckEditorSubarrayRead.Checked = true;
                numericEditSubarrayReadInterval.Value = 1000;
                numericEditSubarrayNDROs.Value = 1;
                numericEditDelay.Value = 1;
                numericEditGlobalInject.Value = 5;

                ultraRadioButtonFPNRunTime.Checked = true;

                buttonRun.Enabled = true;

            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }

        }
        private async void buttonAbort_Click(object sender, EventArgs e)
        {
            try
            {
                if (cts != null)
                {
                    DialogResult result = MessageBox.Show("Are you sure to Abort Exposure?",
                           "Abort Exposure",
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        cidInterface.AbortExposure();
                        imageDisplayTestStatus = false;
                        statusBar.Panels["TestProgress"].ProgressBarInfo.Value = 0;
                        await ClearImageDisplayGraphs();
                        cts.Cancel();
                        buttonRun.Enabled = true;
                    }
                    else
                        return;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void buttonViewUpdateExposures_Click(object sender, EventArgs e)
        {
            try
            {
                ultraExplorerBar1.Groups[3].Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in  View/Update Exposure", "View/Update Exposure", MessageBoxButtons.OK);
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void intensityGraph1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                PointF mouseClickPoint = e.Location;
                double xValue, yValue;

                // Move cursor to mouse postion
                intensityPlot1.InverseMapDataPoint(
                    intensityGraph1.PlotAreaBounds, mouseClickPoint, out xValue, out yValue);

                intensityCursor1.XPosition = xValue;
                intensityCursor1.YPosition = yValue;
                intensityGraph1_AfterMoveCursor(null, null);
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void contextMenuStripIntensityGraph_Opening(object sender, CancelEventArgs e)
        {
            //// if (ContextMenuStripDisabled)
            //if (!loadContextMenu)
            //{
            //    e.Cancel = true;
            //}
            //loadContextMenu = false;
            //loadContextMenuPlots = false;
            //return;
        }

        private void graphPanel_Resize(object sender, EventArgs e)
        {
            try
            {
                //int newWidth = this.graphPanel.Width - 30;

                //// Maintain the aspect ratio 
                //double aspectRatio = 0.86;// (double)this.Size.Height / this.Size.Width;
                //double dHeight = (double)newWidth * aspectRatio;
                //int newHeight = (int)dHeight;

                //while (newHeight > (graphPanel.Size.Height - 20))
                //{
                //    // Recalculate width and height
                //    newWidth = newWidth - 1;
                //    dHeight = (double)newWidth * aspectRatio;
                //    newHeight = (int)dHeight;
                //}


                //intensityGraph1.Width = newWidth;
                //intensityGraph1.Height = newHeight;

                //maxContrastSlide.Height = newHeight - 1200;
               // plotRowToolStripButton();
              //  plotColumnToolStripButton();
                //plotColWaveGraph.Height = newHeight -36;
                //plotColWaveGraph.Width = newWidth - 614;
                //plotRowWaveGraph.Height = newHeight;
                //plotRowWaveGraph.Width = newWidth;

                
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }

        private void intensityGraph1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                int xpos, ypos;
                double maxSignalForY = 1;
                double maxSignalForX = 1;

                if ((plotCursorMoved))
                {
                    plotCursorMoved = false;

                    // Get cursors current position
                    xpos = (int)intensityCursor1.XPosition;
                    ypos = (int)intensityCursor1.YPosition;

                    int xStart, yStart, xSize, ySize = 0;

                    if (zooming)
                    {
                        xStart = (int)currentSubarrayRegion.StartX;
                        yStart = (int)currentSubarrayRegion.StartY;
                        xSize = (int)currentSubarrayRegion.EndX -
                                (int)currentSubarrayRegion.StartX;
                        ySize = (int)currentSubarrayRegion.EndY -
                                (int)currentSubarrayRegion.StartY;
                    }
                    else
                    {
                        xStart = 0;
                        yStart = 0;
                        xSize = (int)xArraySize;
                        ySize = (int)yArraySize;
                    }

                    // Find max signal
                    for (int x = 0; x < xArraySize; x++)
                    {
                        for (int y = 0; y < yArraySize; y++)
                        {
                            //if (patternCorrectedRadioButton.Checked)
                            //{
                            //    if (zPatternCorrectedData[x, ypos] > maxSignalForY)
                            //        maxSignalForY = zPatternCorrectedData[x, ypos];

                            //    if (zPatternCorrectedData[xpos, y] > maxSignalForX)
                            //        maxSignalForX = zPatternCorrectedData[xpos, y];
                            //}
                        }
                    }

                    Range range = new Range(0, maxSignalForY);
                    //plotRowIntPlot.ColorScale.Range = range;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }

        }

        private void intensityGraph1_SizeChanged(object sender, EventArgs e)
        {
            //plotRowToolStripButton();
            //plotColumnToolStripButton();
        }
        private void plotRowToolStripButton()
        {
            try
            {
                if (plotRowGraphEnabled)
                {
                    plotRowGraphEnabled = false;
                    Point graphPoint = new Point();
                    graphPoint.X = 6;
                    graphPoint.Y = 21;

                    intensityGraph1.Location = graphPoint;
                    plotRowWaveGraph.Visible = false;

                    Point colLocation = new Point();
                    colLocation.X = intensityGraph1.Width - 50;
                    colLocation.Y = intensityGraph1.Location.Y + 10;
                    colLocation.X = intensityGraph1.Location.X + 1;

                    plotColWaveGraph.Location = colLocation;
                    plotColWaveGraph.Height = intensityGraph1.Height - 31;
                    // intensityGraphColorScale.Visible = true;

                }
                else
                {
                    plotRowGraphEnabled = true;

                    Point rowLocation = new Point();
                    rowLocation.X = 48;
                    rowLocation.Y = intensityGraph1.Location.Y + 10;
                    rowLocation.X = intensityGraph1.Location.X + 66;

                    plotRowWaveGraph.Visible = true;
                    plotRowWaveGraph.Location = rowLocation;
                    plotRowWaveGraph.Width = intensityGraph1.Width - 110;

                    Point rowGraphPoint = new Point();
                    rowGraphPoint.Y = intensityGraph1.Location.Y;// +plotRowIntGraph.Height;
                    intensityGraph1.Location = rowGraphPoint;

                    // adjust the col graph also
                    Point colLocation = new Point();
                    colLocation.X = intensityGraph1.Width - 50;
                    colLocation.Y = intensityGraph1.Location.Y + 10;
                    colLocation.X = intensityGraph1.Location.X + 1;

                    plotColWaveGraph.Location = colLocation;
                    plotColWaveGraph.Height = intensityGraph1.Height - 31;
                    //  intensityGraphColorScale.Visible = false;
                }
            }
           
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
        private void plotColumnToolStripButton()
        {
            try
            {
                if (plotColumnGraphEnabled)
                {
                    plotColWaveGraph.Visible = false;
                    plotColumnGraphEnabled = false;

                    // intensityGraphColorScale.Visible = true;
                    //  Point location = new Point();
                    //  location.X = intensityGraph1.Location.X - 55;
                    //  intensityGraph1.Location = location;
                }
                else
                {
                    plotColWaveGraph.Visible = true;
                    plotColumnGraphEnabled = true;

                    Size graphSize = new System.Drawing.Size();
                    graphSize.Width = 62;
                    graphSize.Height = intensityGraph1.Size.Height - 40;
                    plotColWaveGraph.Size = graphSize;

                    Point location = new Point();
                    location.X = intensityGraph1.Location.X - 45;
                    location.Y = intensityGraph1.Location.Y;
                    //intensityGraph1.Location = location;

                    // adjust the col graph also
                    Point colLocation = new Point();
                    colLocation.X = intensityGraph1.Width - 50;
                    colLocation.Y = intensityGraph1.Location.Y + 10;
                    colLocation.X = intensityGraph1.Location.X + 1;

                    plotColWaveGraph.Location = colLocation;
                    plotColWaveGraph.Height = intensityGraph1.Height - 31;
                    //   intensityGraphColorScale.Visible = false;
                }
            }            
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}
