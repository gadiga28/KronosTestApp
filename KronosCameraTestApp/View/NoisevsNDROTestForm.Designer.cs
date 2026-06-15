namespace KronosCameraTestApp.View
{
    partial class NoiseVsNDROTestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinToolTip.UltraToolTipInfo ultraToolTipInfo1 = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("Run Mean Variance Test", Infragistics.Win.ToolTipImage.Default, null, Infragistics.Win.DefaultableBoolean.Default);
            Infragistics.Win.UltraWinToolTip.UltraToolTipInfo ultraToolTipInfo2 = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("Run Mean Variance Test", Infragistics.Win.ToolTipImage.Default, null, Infragistics.Win.DefaultableBoolean.Default);
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup ultraExplorerBarGroup1 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup ultraExplorerBarGroup4 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup ultraExplorerBarGroup2 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane1 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedLeft, new System.Guid("4391379b-889f-4dfb-9129-c5b5ae3fe922"));
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane1 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("00283efa-e805-4189-9681-efb4bc428de7"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("4391379b-889f-4dfb-9129-c5b5ae3fe922"), -1);
            this.ultraExplorerBarContainerControl1 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarContainerControl();
            this.ultraGroupBox14 = new Infragistics.Win.Misc.UltraGroupBox();
            this.NumExposuresGroupBox = new Infragistics.Win.Misc.UltraGroupBox();
            this.label34 = new System.Windows.Forms.Label();
            this.numericEditNumOfExposures = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonNDRORun = new System.Windows.Forms.Button();
            this.buttonNDROAbort = new System.Windows.Forms.Button();
            this.ultraExplorerBarContainerControl4 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarContainerControl();
            this.ultraGroupBox11 = new Infragistics.Win.Misc.UltraGroupBox();
            this.LimitsGroupBox = new Infragistics.Win.Misc.UltraGroupBox();
            this.buttonRefreshLimit = new System.Windows.Forms.Button();
            this.buttonUpdateLimit = new System.Windows.Forms.Button();
            this.ultraComboEditorLimitIDs = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.label1 = new System.Windows.Forms.Label();
            this.ultraGroupBox20 = new Infragistics.Win.Misc.UltraGroupBox();
            this.label61 = new System.Windows.Forms.Label();
            this.label62 = new System.Windows.Forms.Label();
            this.R2CorrelationLimit = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.ultraGroupBox19 = new Infragistics.Win.Misc.UltraGroupBox();
            this.PowerFitUpperLimit = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.PowerFitLowerimit = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label17 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.ultraGroupBox18 = new Infragistics.Win.Misc.UltraGroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.ultraGroupBox17 = new Infragistics.Win.Misc.UltraGroupBox();
            this.SnglNoiseUpperLimit = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.SnglNoiseLowerLimit = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.SnglNoiseLimit = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label60 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.ledPassFail = new NationalInstruments.UI.WindowsForms.Led();
            this.label57 = new System.Windows.Forms.Label();
            this.ultraExplorerBarContainerControl2 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarContainerControl();
            this.noiseVsNDROFormExplorerBar = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ndroLegend = new NationalInstruments.UI.WindowsForms.Legend();
            this.MeasuredNoise = new NationalInstruments.UI.LegendItem();
            this.measuredNoiseScatterPlot = new NationalInstruments.UI.ScatterPlot();
            this.xAxis2 = new NationalInstruments.UI.XAxis();
            this.yAxis2 = new NationalInstruments.UI.YAxis();
            this.noiseRatioLegendItem = new NationalInstruments.UI.LegendItem();
            this.log20ScatterPlot = new NationalInstruments.UI.ScatterPlot();
            this.readNoiseScatterGraph = new NationalInstruments.UI.WindowsForms.ScatterGraph();
            this.noiseVsNDROUltraDockManager = new Infragistics.Win.UltraWinDock.UltraDockManager(this.components);
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ReadNoisevsNDROSTestFormAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
            this.dockableWindow1 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.noiseVsNDROUltraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.windowDockingArea1 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.noiseVsNDROUltraToolTipManager = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            this.SnglNoiseResult = new Infragistics.Win.Misc.UltraLabel();
            this.R2CorrelationResult = new Infragistics.Win.Misc.UltraLabel();
            this.NDRONoiseResult = new Infragistics.Win.Misc.UltraLabel();
            this.BestFitResult = new Infragistics.Win.Misc.UltraLabel();
            this.BestFitPower = new Infragistics.Win.Misc.UltraLabel();
            this.ultraExplorerBarContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox14)).BeginInit();
            this.ultraGroupBox14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumExposuresGroupBox)).BeginInit();
            this.NumExposuresGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericEditNumOfExposures)).BeginInit();
            this.ultraExplorerBarContainerControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox11)).BeginInit();
            this.ultraGroupBox11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LimitsGroupBox)).BeginInit();
            this.LimitsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraComboEditorLimitIDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox20)).BeginInit();
            this.ultraGroupBox20.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.R2CorrelationLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox19)).BeginInit();
            this.ultraGroupBox19.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PowerFitUpperLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PowerFitLowerimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox18)).BeginInit();
            this.ultraGroupBox18.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox17)).BeginInit();
            this.ultraGroupBox17.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SnglNoiseUpperLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SnglNoiseLowerLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SnglNoiseLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ledPassFail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.noiseVsNDROFormExplorerBar)).BeginInit();
            this.noiseVsNDROFormExplorerBar.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ndroLegend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.readNoiseScatterGraph)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.noiseVsNDROUltraDockManager)).BeginInit();
            this.dockableWindow1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.noiseVsNDROUltraFormManager)).BeginInit();
            this.windowDockingArea1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraExplorerBarContainerControl1
            // 
            this.ultraExplorerBarContainerControl1.AutoScroll = true;
            this.ultraExplorerBarContainerControl1.Controls.Add(this.ultraGroupBox14);
            this.ultraExplorerBarContainerControl1.Location = new System.Drawing.Point(20, 43);
            this.ultraExplorerBarContainerControl1.Margin = new System.Windows.Forms.Padding(2);
            this.ultraExplorerBarContainerControl1.Name = "ultraExplorerBarContainerControl1";
            this.ultraExplorerBarContainerControl1.Size = new System.Drawing.Size(742, 58);
            this.ultraExplorerBarContainerControl1.TabIndex = 0;
            // 
            // ultraGroupBox14
            // 
            this.ultraGroupBox14.Controls.Add(this.NumExposuresGroupBox);
            this.ultraGroupBox14.Controls.Add(this.buttonNDRORun);
            this.ultraGroupBox14.Controls.Add(this.buttonNDROAbort);
            this.ultraGroupBox14.Location = new System.Drawing.Point(2, 0);
            this.ultraGroupBox14.Margin = new System.Windows.Forms.Padding(2);
            this.ultraGroupBox14.Name = "ultraGroupBox14";
            this.ultraGroupBox14.Size = new System.Drawing.Size(717, 54);
            this.ultraGroupBox14.TabIndex = 19;
            this.ultraGroupBox14.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // NumExposuresGroupBox
            // 
            this.NumExposuresGroupBox.Controls.Add(this.label34);
            this.NumExposuresGroupBox.Controls.Add(this.numericEditNumOfExposures);
            this.NumExposuresGroupBox.Controls.Add(this.label2);
            this.NumExposuresGroupBox.Location = new System.Drawing.Point(504, 6);
            this.NumExposuresGroupBox.Name = "NumExposuresGroupBox";
            this.NumExposuresGroupBox.Size = new System.Drawing.Size(200, 39);
            this.NumExposuresGroupBox.TabIndex = 64;
            this.NumExposuresGroupBox.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.BackColor = System.Drawing.Color.Transparent;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.Location = new System.Drawing.Point(111, 16);
            this.label34.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(78, 13);
            this.label34.TabIndex = 58;
            this.label34.Text = "# of Exposures";
            // 
            // numericEditNumOfExposures
            // 
            this.numericEditNumOfExposures.Enabled = false;
            this.numericEditNumOfExposures.Location = new System.Drawing.Point(65, 12);
            this.numericEditNumOfExposures.Margin = new System.Windows.Forms.Padding(2);
            this.numericEditNumOfExposures.Name = "numericEditNumOfExposures";
            this.numericEditNumOfExposures.Size = new System.Drawing.Size(43, 20);
            this.numericEditNumOfExposures.TabIndex = 63;
            this.numericEditNumOfExposures.ValueChanged += new System.EventHandler(this.numericEditNumOfExposures_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 16);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 57;
            this.label2.Text = "Test For:";
            // 
            // buttonNDRORun
            // 
            this.buttonNDRORun.BackColor = System.Drawing.Color.Transparent;
            this.buttonNDRORun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNDRORun.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonNDRORun.Location = new System.Drawing.Point(14, 22);
            this.buttonNDRORun.Margin = new System.Windows.Forms.Padding(2);
            this.buttonNDRORun.Name = "buttonNDRORun";
            this.buttonNDRORun.Size = new System.Drawing.Size(68, 23);
            this.buttonNDRORun.TabIndex = 0;
            this.buttonNDRORun.Text = "Run";
            this.buttonNDRORun.UseVisualStyleBackColor = false;
            this.buttonNDRORun.Click += new System.EventHandler(this.buttonNDRORun_Click);
            // 
            // buttonNDROAbort
            // 
            this.buttonNDROAbort.BackColor = System.Drawing.Color.Transparent;
            this.buttonNDROAbort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNDROAbort.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonNDROAbort.Location = new System.Drawing.Point(88, 22);
            this.buttonNDROAbort.Margin = new System.Windows.Forms.Padding(2);
            this.buttonNDROAbort.Name = "buttonNDROAbort";
            this.buttonNDROAbort.Size = new System.Drawing.Size(68, 23);
            this.buttonNDROAbort.TabIndex = 13;
            this.buttonNDROAbort.Text = "Abort";
            this.buttonNDROAbort.UseVisualStyleBackColor = false;
            this.buttonNDROAbort.Click += new System.EventHandler(this.buttonNDROAbort_Click);
            // 
            // ultraExplorerBarContainerControl4
            // 
            this.ultraExplorerBarContainerControl4.AutoScroll = true;
            this.ultraExplorerBarContainerControl4.Controls.Add(this.ultraGroupBox11);
            this.ultraExplorerBarContainerControl4.Location = new System.Drawing.Point(20, 143);
            this.ultraExplorerBarContainerControl4.Margin = new System.Windows.Forms.Padding(2);
            this.ultraExplorerBarContainerControl4.Name = "ultraExplorerBarContainerControl4";
            this.ultraExplorerBarContainerControl4.Size = new System.Drawing.Size(742, 164);
            this.ultraExplorerBarContainerControl4.TabIndex = 3;
            // 
            // ultraGroupBox11
            // 
            this.ultraGroupBox11.Controls.Add(this.LimitsGroupBox);
            this.ultraGroupBox11.Controls.Add(this.ledPassFail);
            this.ultraGroupBox11.Controls.Add(this.label57);
            this.ultraGroupBox11.Location = new System.Drawing.Point(0, 2);
            this.ultraGroupBox11.Margin = new System.Windows.Forms.Padding(2);
            this.ultraGroupBox11.Name = "ultraGroupBox11";
            this.ultraGroupBox11.Size = new System.Drawing.Size(722, 162);
            this.ultraGroupBox11.TabIndex = 14;
            this.ultraGroupBox11.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // LimitsGroupBox
            // 
            this.LimitsGroupBox.Controls.Add(this.buttonRefreshLimit);
            this.LimitsGroupBox.Controls.Add(this.buttonUpdateLimit);
            this.LimitsGroupBox.Controls.Add(this.ultraComboEditorLimitIDs);
            this.LimitsGroupBox.Controls.Add(this.label1);
            this.LimitsGroupBox.Controls.Add(this.ultraGroupBox20);
            this.LimitsGroupBox.Controls.Add(this.ultraGroupBox19);
            this.LimitsGroupBox.Controls.Add(this.ultraGroupBox18);
            this.LimitsGroupBox.Controls.Add(this.ultraGroupBox17);
            this.LimitsGroupBox.Location = new System.Drawing.Point(107, 6);
            this.LimitsGroupBox.Name = "LimitsGroupBox";
            this.LimitsGroupBox.Size = new System.Drawing.Size(609, 153);
            this.LimitsGroupBox.TabIndex = 76;
            this.LimitsGroupBox.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // buttonRefreshLimit
            // 
            this.buttonRefreshLimit.BackColor = System.Drawing.Color.Transparent;
            this.buttonRefreshLimit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRefreshLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRefreshLimit.Location = new System.Drawing.Point(507, 41);
            this.buttonRefreshLimit.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRefreshLimit.Name = "buttonRefreshLimit";
            this.buttonRefreshLimit.Size = new System.Drawing.Size(84, 23);
            this.buttonRefreshLimit.TabIndex = 80;
            this.buttonRefreshLimit.Text = "Refresh Limit";
            ultraToolTipInfo1.ToolTipText = "Run Mean Variance Test";
            this.noiseVsNDROUltraToolTipManager.SetUltraToolTip(this.buttonRefreshLimit, ultraToolTipInfo1);
            this.buttonRefreshLimit.UseVisualStyleBackColor = false;
            this.buttonRefreshLimit.Click += new System.EventHandler(this.buttonRefreshLimit_Click);
            // 
            // buttonUpdateLimit
            // 
            this.buttonUpdateLimit.BackColor = System.Drawing.Color.Transparent;
            this.buttonUpdateLimit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUpdateLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonUpdateLimit.Location = new System.Drawing.Point(405, 41);
            this.buttonUpdateLimit.Margin = new System.Windows.Forms.Padding(2);
            this.buttonUpdateLimit.Name = "buttonUpdateLimit";
            this.buttonUpdateLimit.Size = new System.Drawing.Size(84, 23);
            this.buttonUpdateLimit.TabIndex = 78;
            this.buttonUpdateLimit.Text = "Update Limit";
            ultraToolTipInfo2.ToolTipText = "Run Mean Variance Test";
            this.noiseVsNDROUltraToolTipManager.SetUltraToolTip(this.buttonUpdateLimit, ultraToolTipInfo2);
            this.buttonUpdateLimit.UseVisualStyleBackColor = false;
            this.buttonUpdateLimit.Click += new System.EventHandler(this.buttonUpdateLimit_Click);
            // 
            // ultraComboEditorLimitIDs
            // 
            this.ultraComboEditorLimitIDs.Location = new System.Drawing.Point(449, 10);
            this.ultraComboEditorLimitIDs.Name = "ultraComboEditorLimitIDs";
            this.ultraComboEditorLimitIDs.Size = new System.Drawing.Size(142, 21);
            this.ultraComboEditorLimitIDs.TabIndex = 77;
            this.ultraComboEditorLimitIDs.Text = "Limit IDs";
            this.ultraComboEditorLimitIDs.SelectionChanged += new System.EventHandler(this.ultraComboEditorLimitIDs_SelectionChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(407, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 76;
            this.label1.Text = "LimitID";
            // 
            // ultraGroupBox20
            // 
            this.ultraGroupBox20.Controls.Add(this.R2CorrelationResult);
            this.ultraGroupBox20.Controls.Add(this.label61);
            this.ultraGroupBox20.Controls.Add(this.label62);
            this.ultraGroupBox20.Controls.Add(this.R2CorrelationLimit);
            this.ultraGroupBox20.Location = new System.Drawing.Point(9, 79);
            this.ultraGroupBox20.Name = "ultraGroupBox20";
            this.ultraGroupBox20.Size = new System.Drawing.Size(158, 66);
            this.ultraGroupBox20.TabIndex = 23;
            this.ultraGroupBox20.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.BackColor = System.Drawing.Color.Transparent;
            this.label61.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label61.Location = new System.Drawing.Point(3, 11);
            this.label61.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(74, 13);
            this.label61.TabIndex = 10;
            this.label61.Text = "R2 Correlation";
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.BackColor = System.Drawing.Color.Transparent;
            this.label62.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label62.Location = new System.Drawing.Point(89, 11);
            this.label62.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(54, 13);
            this.label62.TabIndex = 12;
            this.label62.Text = "Spec(min)";
            // 
            // R2CorrelationLimit
            // 
            this.R2CorrelationLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.R2CorrelationLimit.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(2);
            this.R2CorrelationLimit.Location = new System.Drawing.Point(86, 34);
            this.R2CorrelationLimit.Margin = new System.Windows.Forms.Padding(2);
            this.R2CorrelationLimit.Name = "R2CorrelationLimit";
            this.R2CorrelationLimit.Size = new System.Drawing.Size(62, 19);
            this.R2CorrelationLimit.TabIndex = 81;
            // 
            // ultraGroupBox19
            // 
            this.ultraGroupBox19.Controls.Add(this.BestFitPower);
            this.ultraGroupBox19.Controls.Add(this.PowerFitUpperLimit);
            this.ultraGroupBox19.Controls.Add(this.PowerFitLowerimit);
            this.ultraGroupBox19.Controls.Add(this.label17);
            this.ultraGroupBox19.Controls.Add(this.label12);
            this.ultraGroupBox19.Controls.Add(this.label13);
            this.ultraGroupBox19.Location = new System.Drawing.Point(325, 79);
            this.ultraGroupBox19.Name = "ultraGroupBox19";
            this.ultraGroupBox19.Size = new System.Drawing.Size(266, 66);
            this.ultraGroupBox19.TabIndex = 24;
            this.ultraGroupBox19.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // PowerFitUpperLimit
            // 
            this.PowerFitUpperLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PowerFitUpperLimit.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(2);
            this.PowerFitUpperLimit.Location = new System.Drawing.Point(190, 34);
            this.PowerFitUpperLimit.Margin = new System.Windows.Forms.Padding(2);
            this.PowerFitUpperLimit.Name = "PowerFitUpperLimit";
            this.PowerFitUpperLimit.Size = new System.Drawing.Size(62, 19);
            this.PowerFitUpperLimit.TabIndex = 84;
            // 
            // PowerFitLowerimit
            // 
            this.PowerFitLowerimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PowerFitLowerimit.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(2);
            this.PowerFitLowerimit.Location = new System.Drawing.Point(104, 34);
            this.PowerFitLowerimit.Margin = new System.Windows.Forms.Padding(2);
            this.PowerFitLowerimit.Name = "PowerFitLowerimit";
            this.PowerFitLowerimit.Size = new System.Drawing.Size(62, 19);
            this.PowerFitLowerimit.TabIndex = 83;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(189, 11);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(68, 13);
            this.label17.TabIndex = 16;
            this.label17.Text = "Spec (upper)";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(7, 11);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(75, 13);
            this.label12.TabIndex = 10;
            this.label12.Text = "Best Fit Power";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(101, 11);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(63, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "Spec(lower)";
            // 
            // ultraGroupBox18
            // 
            this.ultraGroupBox18.Controls.Add(this.BestFitResult);
            this.ultraGroupBox18.Controls.Add(this.NDRONoiseResult);
            this.ultraGroupBox18.Controls.Add(this.label8);
            this.ultraGroupBox18.Controls.Add(this.label9);
            this.ultraGroupBox18.Location = new System.Drawing.Point(172, 79);
            this.ultraGroupBox18.Name = "ultraGroupBox18";
            this.ultraGroupBox18.Size = new System.Drawing.Size(147, 66);
            this.ultraGroupBox18.TabIndex = 22;
            this.ultraGroupBox18.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(9, 11);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "NdroNoise";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(76, 11);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Best Fit";
            // 
            // ultraGroupBox17
            // 
            this.ultraGroupBox17.Controls.Add(this.SnglNoiseResult);
            this.ultraGroupBox17.Controls.Add(this.SnglNoiseUpperLimit);
            this.ultraGroupBox17.Controls.Add(this.label4);
            this.ultraGroupBox17.Controls.Add(this.SnglNoiseLowerLimit);
            this.ultraGroupBox17.Controls.Add(this.label3);
            this.ultraGroupBox17.Controls.Add(this.SnglNoiseLimit);
            this.ultraGroupBox17.Controls.Add(this.label60);
            this.ultraGroupBox17.Controls.Add(this.label58);
            this.ultraGroupBox17.Location = new System.Drawing.Point(9, 7);
            this.ultraGroupBox17.Name = "ultraGroupBox17";
            this.ultraGroupBox17.Size = new System.Drawing.Size(314, 66);
            this.ultraGroupBox17.TabIndex = 21;
            this.ultraGroupBox17.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // SnglNoiseUpperLimit
            // 
            this.SnglNoiseUpperLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SnglNoiseUpperLimit.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(2);
            this.SnglNoiseUpperLimit.Location = new System.Drawing.Point(245, 35);
            this.SnglNoiseUpperLimit.Margin = new System.Windows.Forms.Padding(2);
            this.SnglNoiseUpperLimit.Name = "SnglNoiseUpperLimit";
            this.SnglNoiseUpperLimit.Size = new System.Drawing.Size(62, 19);
            this.SnglNoiseUpperLimit.TabIndex = 84;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(246, 12);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 83;
            this.label4.Text = "Spec UCL";
            // 
            // SnglNoiseLowerLimit
            // 
            this.SnglNoiseLowerLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SnglNoiseLowerLimit.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(2);
            this.SnglNoiseLowerLimit.Location = new System.Drawing.Point(96, 34);
            this.SnglNoiseLowerLimit.Margin = new System.Windows.Forms.Padding(2);
            this.SnglNoiseLowerLimit.Name = "SnglNoiseLowerLimit";
            this.SnglNoiseLowerLimit.Size = new System.Drawing.Size(62, 19);
            this.SnglNoiseLowerLimit.TabIndex = 82;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(168, 11);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 81;
            this.label3.Text = "Spec Limit";
            // 
            // SnglNoiseLimit
            // 
            this.SnglNoiseLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SnglNoiseLimit.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(2);
            this.SnglNoiseLimit.Location = new System.Drawing.Point(171, 34);
            this.SnglNoiseLimit.Margin = new System.Windows.Forms.Padding(2);
            this.SnglNoiseLimit.Name = "SnglNoiseLimit";
            this.SnglNoiseLimit.Size = new System.Drawing.Size(62, 19);
            this.SnglNoiseLimit.TabIndex = 80;
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.BackColor = System.Drawing.Color.Transparent;
            this.label60.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label60.Location = new System.Drawing.Point(12, 11);
            this.label60.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(55, 13);
            this.label60.TabIndex = 10;
            this.label60.Text = "SnglNoise";
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.BackColor = System.Drawing.Color.Transparent;
            this.label58.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label58.Location = new System.Drawing.Point(98, 11);
            this.label58.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(54, 13);
            this.label58.TabIndex = 12;
            this.label58.Text = "Spec LCL";
            // 
            // ledPassFail
            // 
            this.ledPassFail.LedStyle = NationalInstruments.UI.LedStyle.Square3D;
            this.ledPassFail.Location = new System.Drawing.Point(26, 30);
            this.ledPassFail.Margin = new System.Windows.Forms.Padding(2);
            this.ledPassFail.Name = "ledPassFail";
            this.ledPassFail.Size = new System.Drawing.Size(51, 22);
            this.ledPassFail.TabIndex = 17;
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.BackColor = System.Drawing.Color.Transparent;
            this.label57.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label57.Location = new System.Drawing.Point(25, 15);
            this.label57.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(60, 13);
            this.label57.TabIndex = 16;
            this.label57.Text = "Pass/Fail";
            // 
            // ultraExplorerBarContainerControl2
            // 
            this.ultraExplorerBarContainerControl2.AutoScroll = true;
            this.ultraExplorerBarContainerControl2.Location = new System.Drawing.Point(20, 349);
            this.ultraExplorerBarContainerControl2.Margin = new System.Windows.Forms.Padding(2);
            this.ultraExplorerBarContainerControl2.Name = "ultraExplorerBarContainerControl2";
            this.ultraExplorerBarContainerControl2.Size = new System.Drawing.Size(725, 0);
            this.ultraExplorerBarContainerControl2.TabIndex = 1;
            this.ultraExplorerBarContainerControl2.Visible = false;
            // 
            // noiseVsNDROFormExplorerBar
            // 
            this.noiseVsNDROFormExplorerBar.AcceptsFocus = Infragistics.Win.DefaultableBoolean.True;
            this.noiseVsNDROFormExplorerBar.AnimationSpeed = Infragistics.Win.UltraWinExplorerBar.AnimationSpeed.Fast;
            appearance5.BackColor = System.Drawing.Color.Transparent;
            appearance5.FontData.BoldAsString = "True";
            appearance5.FontData.SizeInPoints = 9F;
            this.noiseVsNDROFormExplorerBar.Appearance = appearance5;
            this.noiseVsNDROFormExplorerBar.BorderStyle = Infragistics.Win.UIElementBorderStyle.Rounded4Thick;
            this.noiseVsNDROFormExplorerBar.Controls.Add(this.ultraExplorerBarContainerControl1);
            this.noiseVsNDROFormExplorerBar.Controls.Add(this.ultraExplorerBarContainerControl2);
            this.noiseVsNDROFormExplorerBar.Controls.Add(this.ultraExplorerBarContainerControl4);
            ultraExplorerBarGroup1.Container = this.ultraExplorerBarContainerControl1;
            ultraExplorerBarGroup1.Settings.ContainerHeight = 58;
            ultraExplorerBarGroup1.Tag = "MVExecute";
            ultraExplorerBarGroup1.Text = "Execute";
            ultraExplorerBarGroup4.Container = this.ultraExplorerBarContainerControl4;
            ultraExplorerBarGroup4.Settings.ContainerHeight = 164;
            ultraExplorerBarGroup4.Tag = "MVTestResults";
            ultraExplorerBarGroup4.Text = "Test Results - Limits";
            ultraExplorerBarGroup2.Container = this.ultraExplorerBarContainerControl2;
            ultraExplorerBarGroup2.Expanded = false;
            ultraExplorerBarGroup2.ItemSettings.AllowDragMove = Infragistics.Win.UltraWinExplorerBar.ItemDragStyle.WithinAndAcrossGroups;
            ultraExplorerBarGroup2.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.True;
            ultraExplorerBarGroup2.Settings.ContainerHeight = 636;
            ultraExplorerBarGroup2.Tag = "MVExposureSettings";
            ultraExplorerBarGroup2.Text = "Exposure Settings";
            this.noiseVsNDROFormExplorerBar.Groups.AddRange(new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup[] {
            ultraExplorerBarGroup1,
            ultraExplorerBarGroup4,
            ultraExplorerBarGroup2});
            this.noiseVsNDROFormExplorerBar.GroupSettings.Style = Infragistics.Win.UltraWinExplorerBar.GroupStyle.ControlContainer;
            this.noiseVsNDROFormExplorerBar.Location = new System.Drawing.Point(0, 18);
            this.noiseVsNDROFormExplorerBar.Margin = new System.Windows.Forms.Padding(2);
            this.noiseVsNDROFormExplorerBar.Name = "noiseVsNDROFormExplorerBar";
            this.noiseVsNDROFormExplorerBar.ShowDefaultContextMenu = false;
            this.noiseVsNDROFormExplorerBar.Size = new System.Drawing.Size(782, 795);
            this.noiseVsNDROFormExplorerBar.TabIndex = 15;
            this.noiseVsNDROFormExplorerBar.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.noiseVsNDROFormExplorerBar.ViewStyle = Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarViewStyle.Office2007;
            this.noiseVsNDROFormExplorerBar.Click += new System.EventHandler(this.ultraExplorerBar1_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.ndroLegend);
            this.panel3.Controls.Add(this.readNoiseScatterGraph);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(795, 31);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(628, 813);
            this.panel3.TabIndex = 32;
            this.panel3.Click += new System.EventHandler(this.panel3_Click);
            // 
            // ndroLegend
            // 
            this.ndroLegend.ItemLayoutMode = NationalInstruments.UI.LegendItemLayoutMode.LeftToRight;
            this.ndroLegend.Items.AddRange(new NationalInstruments.UI.LegendItem[] {
            this.MeasuredNoise,
            this.noiseRatioLegendItem});
            this.ndroLegend.ItemSize = new System.Drawing.Size(30, 22);
            this.ndroLegend.Location = new System.Drawing.Point(6, 5);
            this.ndroLegend.Name = "ndroLegend";
            this.ndroLegend.Size = new System.Drawing.Size(217, 30);
            this.ndroLegend.TabIndex = 15;
            // 
            // MeasuredNoise
            // 
            this.MeasuredNoise.Source = this.measuredNoiseScatterPlot;
            this.MeasuredNoise.Text = "Measured Noise";
            // 
            // measuredNoiseScatterPlot
            // 
            this.measuredNoiseScatterPlot.LineColor = System.Drawing.Color.Red;
            this.measuredNoiseScatterPlot.LineColorPrecedence = NationalInstruments.UI.ColorPrecedence.UserDefinedColor;
            this.measuredNoiseScatterPlot.PointColor = System.Drawing.Color.Red;
            this.measuredNoiseScatterPlot.PointStyle = NationalInstruments.UI.PointStyle.SolidSquare;
            this.measuredNoiseScatterPlot.XAxis = this.xAxis2;
            this.measuredNoiseScatterPlot.YAxis = this.yAxis2;
            // 
            // xAxis2
            // 
            this.xAxis2.Caption = "NDROs";
            this.xAxis2.ScaleType = NationalInstruments.UI.ScaleType.Logarithmic;
            // 
            // yAxis2
            // 
            this.yAxis2.Caption = "Noise Level";
            this.yAxis2.ScaleType = NationalInstruments.UI.ScaleType.Logarithmic;
            // 
            // noiseRatioLegendItem
            // 
            this.noiseRatioLegendItem.Source = this.log20ScatterPlot;
            this.noiseRatioLegendItem.Text = "Best Fit";
            // 
            // log20ScatterPlot
            // 
            this.log20ScatterPlot.LineColor = System.Drawing.Color.LimeGreen;
            this.log20ScatterPlot.LineColorPrecedence = NationalInstruments.UI.ColorPrecedence.UserDefinedColor;
            this.log20ScatterPlot.PointColor = System.Drawing.Color.Green;
            this.log20ScatterPlot.PointStyle = NationalInstruments.UI.PointStyle.SolidSquare;
            this.log20ScatterPlot.XAxis = this.xAxis2;
            this.log20ScatterPlot.YAxis = this.yAxis2;
            // 
            // readNoiseScatterGraph
            // 
            this.readNoiseScatterGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.readNoiseScatterGraph.Border = NationalInstruments.UI.Border.Etched;
            this.readNoiseScatterGraph.Caption = "Read Noise vs. NDRO Plot";
            this.readNoiseScatterGraph.Location = new System.Drawing.Point(5, 41);
            this.readNoiseScatterGraph.Name = "readNoiseScatterGraph";
            this.readNoiseScatterGraph.Plots.AddRange(new NationalInstruments.UI.ScatterPlot[] {
            this.measuredNoiseScatterPlot,
            this.log20ScatterPlot});
            this.readNoiseScatterGraph.Size = new System.Drawing.Size(618, 767);
            this.readNoiseScatterGraph.TabIndex = 10;
            this.readNoiseScatterGraph.UseColorGenerator = true;
            this.readNoiseScatterGraph.XAxes.AddRange(new NationalInstruments.UI.XAxis[] {
            this.xAxis2});
            this.readNoiseScatterGraph.YAxes.AddRange(new NationalInstruments.UI.YAxis[] {
            this.yAxis2});
            // 
            // noiseVsNDROUltraDockManager
            // 
            appearance1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.noiseVsNDROUltraDockManager.DefaultPaneSettings.ActiveCaptionAppearance = appearance1;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.noiseVsNDROUltraDockManager.DefaultPaneSettings.ActivePaneAppearance = appearance2;
            appearance3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.noiseVsNDROUltraDockManager.DefaultPaneSettings.ActiveTabAppearance = appearance3;
            appearance4.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.noiseVsNDROUltraDockManager.DefaultPaneSettings.Appearance = appearance4;
            dockableControlPane1.Control = this.noiseVsNDROFormExplorerBar;
            dockableControlPane1.FlyoutSize = new System.Drawing.Size(782, -1);
            dockableControlPane1.OriginalControlBounds = new System.Drawing.Rectangle(129, 9, 733, 796);
            dockableControlPane1.Size = new System.Drawing.Size(100, 100);
            dockableControlPane1.Text = "Test Settings";
            dockAreaPane1.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane1});
            dockAreaPane1.Size = new System.Drawing.Size(782, 813);
            this.noiseVsNDROUltraDockManager.DockAreas.AddRange(new Infragistics.Win.UltraWinDock.DockAreaPane[] {
            dockAreaPane1});
            this.noiseVsNDROUltraDockManager.DragWindowStyle = Infragistics.Win.UltraWinDock.DragWindowStyle.LayeredWindowWithIndicators;
            this.noiseVsNDROUltraDockManager.HostControl = this;
            this.noiseVsNDROUltraDockManager.ShowCloseButton = false;
            // 
            // _ReadNoisevsNDROSTestFormUnpinnedTabAreaLeft
            // 
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaLeft.Location = new System.Drawing.Point(8, 31);
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaLeft.Margin = new System.Windows.Forms.Padding(2);
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaLeft.Name = "_ReadNoisevsNDROSTestFormUnpinnedTabAreaLeft";
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaLeft.Owner = this.noiseVsNDROUltraDockManager;
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaLeft.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaLeft.Size = new System.Drawing.Size(0, 813);
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaLeft.TabIndex = 33;
            // 
            // _ReadNoisevsNDROSTestFormUnpinnedTabAreaRight
            // 
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaRight.Location = new System.Drawing.Point(1423, 31);
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaRight.Margin = new System.Windows.Forms.Padding(2);
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaRight.Name = "_ReadNoisevsNDROSTestFormUnpinnedTabAreaRight";
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaRight.Owner = this.noiseVsNDROUltraDockManager;
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaRight.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaRight.Size = new System.Drawing.Size(0, 813);
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaRight.TabIndex = 34;
            // 
            // _ReadNoisevsNDROSTestFormUnpinnedTabAreaTop
            // 
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaTop.Location = new System.Drawing.Point(8, 31);
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaTop.Margin = new System.Windows.Forms.Padding(2);
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaTop.Name = "_ReadNoisevsNDROSTestFormUnpinnedTabAreaTop";
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaTop.Owner = this.noiseVsNDROUltraDockManager;
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaTop.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaTop.Size = new System.Drawing.Size(1415, 0);
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaTop.TabIndex = 35;
            // 
            // _ReadNoisevsNDROSTestFormUnpinnedTabAreaBottom
            // 
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaBottom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaBottom.Location = new System.Drawing.Point(8, 844);
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaBottom.Margin = new System.Windows.Forms.Padding(2);
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaBottom.Name = "_ReadNoisevsNDROSTestFormUnpinnedTabAreaBottom";
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaBottom.Owner = this.noiseVsNDROUltraDockManager;
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaBottom.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaBottom.Size = new System.Drawing.Size(1415, 0);
            this._ReadNoisevsNDROSTestFormUnpinnedTabAreaBottom.TabIndex = 36;
            // 
            // _ReadNoisevsNDROSTestFormAutoHideControl
            // 
            this._ReadNoisevsNDROSTestFormAutoHideControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ReadNoisevsNDROSTestFormAutoHideControl.Location = new System.Drawing.Point(29, 30);
            this._ReadNoisevsNDROSTestFormAutoHideControl.Margin = new System.Windows.Forms.Padding(2);
            this._ReadNoisevsNDROSTestFormAutoHideControl.Name = "_ReadNoisevsNDROSTestFormAutoHideControl";
            this._ReadNoisevsNDROSTestFormAutoHideControl.Owner = this.noiseVsNDROUltraDockManager;
            this._ReadNoisevsNDROSTestFormAutoHideControl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._ReadNoisevsNDROSTestFormAutoHideControl.Size = new System.Drawing.Size(787, 814);
            this._ReadNoisevsNDROSTestFormAutoHideControl.TabIndex = 37;
            // 
            // dockableWindow1
            // 
            this.dockableWindow1.Controls.Add(this.noiseVsNDROFormExplorerBar);
            this.dockableWindow1.Location = new System.Drawing.Point(0, 0);
            this.dockableWindow1.Name = "dockableWindow1";
            this.dockableWindow1.Owner = this.noiseVsNDROUltraDockManager;
            this.dockableWindow1.Size = new System.Drawing.Size(782, 813);
            this.dockableWindow1.TabIndex = 48;
            // 
            // noiseVsNDROUltraFormManager
            // 
            this.noiseVsNDROUltraFormManager.Form = this;
            this.noiseVsNDROUltraFormManager.FormStyleSettings.Style = Infragistics.Win.UltraWinForm.UltraFormStyle.Office2010;
            // 
            // _ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Left
            // 
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Left.FormManager = this.noiseVsNDROUltraFormManager;
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(2);
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Left.Name = "_ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Left";
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 813);
            // 
            // _ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Right
            // 
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Right.FormManager = this.noiseVsNDROUltraFormManager;
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1423, 31);
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(2);
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Right.Name = "_ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Right";
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 813);
            // 
            // _ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Top
            // 
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Top.FormManager = this.noiseVsNDROUltraFormManager;
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(2);
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Top.Name = "_ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Top";
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1431, 31);
            // 
            // _ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Bottom
            // 
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Bottom.FormManager = this.noiseVsNDROUltraFormManager;
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 844);
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(2);
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Bottom.Name = "_ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Bottom";
            this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1431, 8);
            // 
            // windowDockingArea1
            // 
            this.windowDockingArea1.Controls.Add(this.dockableWindow1);
            this.windowDockingArea1.Dock = System.Windows.Forms.DockStyle.Left;
            this.windowDockingArea1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windowDockingArea1.Location = new System.Drawing.Point(8, 31);
            this.windowDockingArea1.Name = "windowDockingArea1";
            this.windowDockingArea1.Owner = this.noiseVsNDROUltraDockManager;
            this.windowDockingArea1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.windowDockingArea1.Size = new System.Drawing.Size(787, 813);
            this.windowDockingArea1.TabIndex = 43;
            // 
            // noiseVsNDROUltraToolTipManager
            // 
            this.noiseVsNDROUltraToolTipManager.ContainingControl = this;
            this.noiseVsNDROUltraToolTipManager.DisplayStyle = Infragistics.Win.ToolTipDisplayStyle.Office2007;
            // 
            // SnglNoiseResult
            // 
            this.SnglNoiseResult.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.SnglNoiseResult.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.SnglNoiseResult.Location = new System.Drawing.Point(15, 33);
            this.SnglNoiseResult.Name = "SnglNoiseResult";
            this.SnglNoiseResult.Size = new System.Drawing.Size(65, 21);
            this.SnglNoiseResult.TabIndex = 77;
            // 
            // R2CorrelationResult
            // 
            this.R2CorrelationResult.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.R2CorrelationResult.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.R2CorrelationResult.Location = new System.Drawing.Point(15, 34);
            this.R2CorrelationResult.Name = "R2CorrelationResult";
            this.R2CorrelationResult.Size = new System.Drawing.Size(65, 21);
            this.R2CorrelationResult.TabIndex = 85;
            // 
            // NDRONoiseResult
            // 
            this.NDRONoiseResult.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.NDRONoiseResult.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.NDRONoiseResult.Location = new System.Drawing.Point(5, 32);
            this.NDRONoiseResult.Name = "NDRONoiseResult";
            this.NDRONoiseResult.Size = new System.Drawing.Size(65, 21);
            this.NDRONoiseResult.TabIndex = 86;
            // 
            // BestFitResult
            // 
            this.BestFitResult.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.BestFitResult.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.BestFitResult.Location = new System.Drawing.Point(74, 32);
            this.BestFitResult.Name = "BestFitResult";
            this.BestFitResult.Size = new System.Drawing.Size(65, 21);
            this.BestFitResult.TabIndex = 87;
            // 
            // BestFitPower
            // 
            this.BestFitPower.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.BestFitPower.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.BestFitPower.Location = new System.Drawing.Point(11, 34);
            this.BestFitPower.Name = "BestFitPower";
            this.BestFitPower.Size = new System.Drawing.Size(65, 21);
            this.BestFitPower.TabIndex = 88;
            // 
            // NoiseVsNDROTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1431, 852);
            this.Controls.Add(this._ReadNoisevsNDROSTestFormAutoHideControl);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.windowDockingArea1);
            this.Controls.Add(this._ReadNoisevsNDROSTestFormUnpinnedTabAreaTop);
            this.Controls.Add(this._ReadNoisevsNDROSTestFormUnpinnedTabAreaBottom);
            this.Controls.Add(this._ReadNoisevsNDROSTestFormUnpinnedTabAreaRight);
            this.Controls.Add(this._ReadNoisevsNDROSTestFormUnpinnedTabAreaLeft);
            this.Controls.Add(this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Bottom);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "NoiseVsNDROTestForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ReadNoise vs NDROs";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NoiseVsNDROTestForm_FormClosing);
            this.Load += new System.EventHandler(this.NoiseVsNDROTestForm_Load);
            this.ultraExplorerBarContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox14)).EndInit();
            this.ultraGroupBox14.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NumExposuresGroupBox)).EndInit();
            this.NumExposuresGroupBox.ResumeLayout(false);
            this.NumExposuresGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericEditNumOfExposures)).EndInit();
            this.ultraExplorerBarContainerControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox11)).EndInit();
            this.ultraGroupBox11.ResumeLayout(false);
            this.ultraGroupBox11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LimitsGroupBox)).EndInit();
            this.LimitsGroupBox.ResumeLayout(false);
            this.LimitsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraComboEditorLimitIDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox20)).EndInit();
            this.ultraGroupBox20.ResumeLayout(false);
            this.ultraGroupBox20.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.R2CorrelationLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox19)).EndInit();
            this.ultraGroupBox19.ResumeLayout(false);
            this.ultraGroupBox19.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PowerFitUpperLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PowerFitLowerimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox18)).EndInit();
            this.ultraGroupBox18.ResumeLayout(false);
            this.ultraGroupBox18.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox17)).EndInit();
            this.ultraGroupBox17.ResumeLayout(false);
            this.ultraGroupBox17.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SnglNoiseUpperLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SnglNoiseLowerLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SnglNoiseLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ledPassFail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.noiseVsNDROFormExplorerBar)).EndInit();
            this.noiseVsNDROFormExplorerBar.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ndroLegend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.readNoiseScatterGraph)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.noiseVsNDROUltraDockManager)).EndInit();
            this.dockableWindow1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.noiseVsNDROUltraFormManager)).EndInit();
            this.windowDockingArea1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private Infragistics.Win.UltraWinDock.UltraDockManager noiseVsNDROUltraDockManager;
        private Infragistics.Win.UltraWinDock.AutoHideControl _ReadNoisevsNDROSTestFormAutoHideControl;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ReadNoisevsNDROSTestFormUnpinnedTabAreaTop;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ReadNoisevsNDROSTestFormUnpinnedTabAreaBottom;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ReadNoisevsNDROSTestFormUnpinnedTabAreaLeft;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ReadNoisevsNDROSTestFormUnpinnedTabAreaRight;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormManager noiseVsNDROUltraFormManager;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ReadNoisevsNDROSTestForm_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar noiseVsNDROFormExplorerBar;
        private Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarContainerControl ultraExplorerBarContainerControl1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox14;
        private System.Windows.Forms.NumericUpDown numericEditNumOfExposures;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonNDRORun;
        private System.Windows.Forms.Button buttonNDROAbort;
        private Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarContainerControl ultraExplorerBarContainerControl2;
        private Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarContainerControl ultraExplorerBarContainerControl4;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox11;
        private NationalInstruments.UI.WindowsForms.Led ledPassFail;
        private System.Windows.Forms.Label label57;
        private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea1;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow1;
        public NationalInstruments.UI.ScatterPlot log20ScatterPlot;
        private NationalInstruments.UI.XAxis xAxis2;
        private NationalInstruments.UI.YAxis yAxis2;
        public NationalInstruments.UI.WindowsForms.ScatterGraph readNoiseScatterGraph;
        public NationalInstruments.UI.ScatterPlot measuredNoiseScatterPlot;
        private Infragistics.Win.UltraWinToolTip.UltraToolTipManager noiseVsNDROUltraToolTipManager;
        private Infragistics.Win.Misc.UltraGroupBox NumExposuresGroupBox;
        private Infragistics.Win.Misc.UltraGroupBox LimitsGroupBox;
        private System.Windows.Forms.Button buttonUpdateLimit;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ultraComboEditorLimitIDs;
        private System.Windows.Forms.Label label1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox20;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.Label label62;
        private NationalInstruments.UI.WindowsForms.NumericEdit R2CorrelationLimit;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox19;
        private NationalInstruments.UI.WindowsForms.NumericEdit PowerFitUpperLimit;
        private NationalInstruments.UI.WindowsForms.NumericEdit PowerFitLowerimit;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox18;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox17;
        private NationalInstruments.UI.WindowsForms.NumericEdit SnglNoiseLimit;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Button buttonRefreshLimit;
        private NationalInstruments.UI.WindowsForms.Legend ndroLegend;
        public NationalInstruments.UI.LegendItem MeasuredNoise;
        private NationalInstruments.UI.LegendItem noiseRatioLegendItem;
        private NationalInstruments.UI.WindowsForms.NumericEdit SnglNoiseUpperLimit;
        private System.Windows.Forms.Label label4;
        private NationalInstruments.UI.WindowsForms.NumericEdit SnglNoiseLowerLimit;
        private System.Windows.Forms.Label label3;
        private Infragistics.Win.Misc.UltraLabel R2CorrelationResult;
        private Infragistics.Win.Misc.UltraLabel SnglNoiseResult;
        private Infragistics.Win.Misc.UltraLabel BestFitPower;
        private Infragistics.Win.Misc.UltraLabel BestFitResult;
        private Infragistics.Win.Misc.UltraLabel NDRONoiseResult;
    }
}