namespace KronosCameraTestApp.View
{
    partial class DarkCurrentTestForm
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
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup ultraExplorerBarGroup3 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane1 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedLeft, new System.Guid("39b182f2-69db-4ba0-9244-492707f798cd"));
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane1 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("f0564063-37c7-4c61-97fc-b175b189e821"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("39b182f2-69db-4ba0-9244-492707f798cd"), -1);
            this.ultraExplorerBarContainerControl1 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarContainerControl();
            this.ultraGroupBox14 = new Infragistics.Win.Misc.UltraGroupBox();
            this.NumExposuresGroupBox = new Infragistics.Win.Misc.UltraGroupBox();
            this.label34 = new System.Windows.Forms.Label();
            this.numericEditNumOfExposures = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonDarkCurrentRun = new System.Windows.Forms.Button();
            this.buttonDarkCurrentAbort = new System.Windows.Forms.Button();
            this.ultraExplorerBarContainerControl4 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarContainerControl();
            this.LimitsGroupBox = new Infragistics.Win.Misc.UltraGroupBox();
            this.buttonRefreshLimit = new System.Windows.Forms.Button();
            this.buttonUpdateLimit = new System.Windows.Forms.Button();
            this.ultraComboEditorLimitIDs = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.label1 = new System.Windows.Forms.Label();
            this.ultraGroupBox20 = new Infragistics.Win.Misc.UltraGroupBox();
            this.MaxDarkROI = new Infragistics.Win.Misc.UltraLabel();
            this.MaxDarkROIUpperLimit = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.MaxDarkROILowerLimit = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.MaxDarkROILimit = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label40 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.ultraGroupBox19 = new Infragistics.Win.Misc.UltraGroupBox();
            this.AveTrapLimit = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label36 = new System.Windows.Forms.Label();
            this.labelAveTrap = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.ultraGroupBox18 = new Infragistics.Win.Misc.UltraGroupBox();
            this.labelAveDarkFF = new System.Windows.Forms.Label();
            this.labelMaxDarkFF = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.ultraGroupBox17 = new Infragistics.Win.Misc.UltraGroupBox();
            this.MaxTrapLimit = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.MaxTrapResult = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.ledPassFail = new NationalInstruments.UI.WindowsForms.Led();
            this.label62 = new System.Windows.Forms.Label();
            this.ultraExplorerBarContainerControl3 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarContainerControl();
            this.darkCurrentFormExplorerBar = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar();
            this.panel4 = new System.Windows.Forms.Panel();
            this.darkCurrentLegend = new NationalInstruments.UI.WindowsForms.Legend();
            this.TheroryDarkCurrent = new NationalInstruments.UI.LegendItem();
            this.MeasuredNoisePlot = new NationalInstruments.UI.WaveformPlot();
            this.xAxis1 = new NationalInstruments.UI.XAxis();
            this.yAxis1 = new NationalInstruments.UI.YAxis();
            this.measureDarkCurrent = new NationalInstruments.UI.LegendItem();
            this.TheoryPlot = new NationalInstruments.UI.WaveformPlot();
            this.darkCurrentWaveformGraph = new NationalInstruments.UI.WindowsForms.WaveformGraph();
            this.darkCurrentUltraDockManager = new Infragistics.Win.UltraWinDock.UltraDockManager(this.components);
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._DefectsAndDarkCurrentTestFormAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
            this.dockableWindow1 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.darkCurrentUltraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.windowDockingArea1 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.darkCurrentUltraToolTipManager = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            this.ultraExplorerBarContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox14)).BeginInit();
            this.ultraGroupBox14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumExposuresGroupBox)).BeginInit();
            this.NumExposuresGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericEditNumOfExposures)).BeginInit();
            this.ultraExplorerBarContainerControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LimitsGroupBox)).BeginInit();
            this.LimitsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraComboEditorLimitIDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox20)).BeginInit();
            this.ultraGroupBox20.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxDarkROIUpperLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxDarkROILowerLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxDarkROILimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox19)).BeginInit();
            this.ultraGroupBox19.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AveTrapLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox18)).BeginInit();
            this.ultraGroupBox18.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox17)).BeginInit();
            this.ultraGroupBox17.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxTrapLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ledPassFail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.darkCurrentFormExplorerBar)).BeginInit();
            this.darkCurrentFormExplorerBar.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.darkCurrentLegend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.darkCurrentWaveformGraph)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.darkCurrentUltraDockManager)).BeginInit();
            this.dockableWindow1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.darkCurrentUltraFormManager)).BeginInit();
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
            this.ultraExplorerBarContainerControl1.Size = new System.Drawing.Size(781, 58);
            this.ultraExplorerBarContainerControl1.TabIndex = 0;
            // 
            // ultraGroupBox14
            // 
            this.ultraGroupBox14.Controls.Add(this.NumExposuresGroupBox);
            this.ultraGroupBox14.Controls.Add(this.buttonDarkCurrentRun);
            this.ultraGroupBox14.Controls.Add(this.buttonDarkCurrentAbort);
            this.ultraGroupBox14.Location = new System.Drawing.Point(2, 0);
            this.ultraGroupBox14.Margin = new System.Windows.Forms.Padding(2);
            this.ultraGroupBox14.Name = "ultraGroupBox14";
            this.ultraGroupBox14.Size = new System.Drawing.Size(761, 50);
            this.ultraGroupBox14.TabIndex = 19;
            this.ultraGroupBox14.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // NumExposuresGroupBox
            // 
            this.NumExposuresGroupBox.Controls.Add(this.label34);
            this.NumExposuresGroupBox.Controls.Add(this.numericEditNumOfExposures);
            this.NumExposuresGroupBox.Controls.Add(this.label2);
            this.NumExposuresGroupBox.Location = new System.Drawing.Point(533, 6);
            this.NumExposuresGroupBox.Name = "NumExposuresGroupBox";
            this.NumExposuresGroupBox.Size = new System.Drawing.Size(221, 38);
            this.NumExposuresGroupBox.TabIndex = 64;
            this.NumExposuresGroupBox.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.BackColor = System.Drawing.Color.Transparent;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.Location = new System.Drawing.Point(117, 13);
            this.label34.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(78, 13);
            this.label34.TabIndex = 58;
            this.label34.Text = "# of Exposures";
            // 
            // numericEditNumOfExposures
            // 
            this.numericEditNumOfExposures.Enabled = false;
            this.numericEditNumOfExposures.Location = new System.Drawing.Point(59, 10);
            this.numericEditNumOfExposures.Margin = new System.Windows.Forms.Padding(2);
            this.numericEditNumOfExposures.Name = "numericEditNumOfExposures";
            this.numericEditNumOfExposures.Size = new System.Drawing.Size(53, 20);
            this.numericEditNumOfExposures.TabIndex = 63;
            this.numericEditNumOfExposures.ValueChanged += new System.EventHandler(this.numericEditNumOfExposures_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 13);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 57;
            this.label2.Text = "Test For:";
            // 
            // buttonDarkCurrentRun
            // 
            this.buttonDarkCurrentRun.BackColor = System.Drawing.Color.Transparent;
            this.buttonDarkCurrentRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDarkCurrentRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDarkCurrentRun.Location = new System.Drawing.Point(17, 14);
            this.buttonDarkCurrentRun.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDarkCurrentRun.Name = "buttonDarkCurrentRun";
            this.buttonDarkCurrentRun.Size = new System.Drawing.Size(68, 23);
            this.buttonDarkCurrentRun.TabIndex = 0;
            this.buttonDarkCurrentRun.Text = "Run";
            this.buttonDarkCurrentRun.UseVisualStyleBackColor = false;
            this.buttonDarkCurrentRun.Click += new System.EventHandler(this.buttonDarkCurrentRun_Click);
            // 
            // buttonDarkCurrentAbort
            // 
            this.buttonDarkCurrentAbort.BackColor = System.Drawing.Color.Transparent;
            this.buttonDarkCurrentAbort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDarkCurrentAbort.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDarkCurrentAbort.Location = new System.Drawing.Point(106, 14);
            this.buttonDarkCurrentAbort.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDarkCurrentAbort.Name = "buttonDarkCurrentAbort";
            this.buttonDarkCurrentAbort.Size = new System.Drawing.Size(68, 23);
            this.buttonDarkCurrentAbort.TabIndex = 13;
            this.buttonDarkCurrentAbort.Text = "Abort";
            this.buttonDarkCurrentAbort.UseVisualStyleBackColor = false;
            this.buttonDarkCurrentAbort.Click += new System.EventHandler(this.buttonDarkCurrentAbort_Click);
            // 
            // ultraExplorerBarContainerControl4
            // 
            this.ultraExplorerBarContainerControl4.AutoScroll = true;
            this.ultraExplorerBarContainerControl4.Controls.Add(this.LimitsGroupBox);
            this.ultraExplorerBarContainerControl4.Location = new System.Drawing.Point(20, 143);
            this.ultraExplorerBarContainerControl4.Margin = new System.Windows.Forms.Padding(2);
            this.ultraExplorerBarContainerControl4.Name = "ultraExplorerBarContainerControl4";
            this.ultraExplorerBarContainerControl4.Size = new System.Drawing.Size(781, 187);
            this.ultraExplorerBarContainerControl4.TabIndex = 3;
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
            this.LimitsGroupBox.Controls.Add(this.ledPassFail);
            this.LimitsGroupBox.Controls.Add(this.label62);
            this.LimitsGroupBox.Location = new System.Drawing.Point(0, 2);
            this.LimitsGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.LimitsGroupBox.Name = "LimitsGroupBox";
            this.LimitsGroupBox.Size = new System.Drawing.Size(763, 176);
            this.LimitsGroupBox.TabIndex = 14;
            this.LimitsGroupBox.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // buttonRefreshLimit
            // 
            this.buttonRefreshLimit.BackColor = System.Drawing.Color.Transparent;
            this.buttonRefreshLimit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRefreshLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRefreshLimit.Location = new System.Drawing.Point(670, 37);
            this.buttonRefreshLimit.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRefreshLimit.Name = "buttonRefreshLimit";
            this.buttonRefreshLimit.Size = new System.Drawing.Size(84, 23);
            this.buttonRefreshLimit.TabIndex = 80;
            this.buttonRefreshLimit.Text = "Refresh Limit";
            ultraToolTipInfo1.ToolTipText = "Run Mean Variance Test";
            this.darkCurrentUltraToolTipManager.SetUltraToolTip(this.buttonRefreshLimit, ultraToolTipInfo1);
            this.buttonRefreshLimit.UseVisualStyleBackColor = false;
            this.buttonRefreshLimit.Click += new System.EventHandler(this.buttonRefreshLimit_Click);
            // 
            // buttonUpdateLimit
            // 
            this.buttonUpdateLimit.BackColor = System.Drawing.Color.Transparent;
            this.buttonUpdateLimit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUpdateLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonUpdateLimit.Location = new System.Drawing.Point(558, 37);
            this.buttonUpdateLimit.Margin = new System.Windows.Forms.Padding(2);
            this.buttonUpdateLimit.Name = "buttonUpdateLimit";
            this.buttonUpdateLimit.Size = new System.Drawing.Size(84, 23);
            this.buttonUpdateLimit.TabIndex = 77;
            this.buttonUpdateLimit.Text = "Update Limit";
            ultraToolTipInfo2.ToolTipText = "Run Mean Variance Test";
            this.darkCurrentUltraToolTipManager.SetUltraToolTip(this.buttonUpdateLimit, ultraToolTipInfo2);
            this.buttonUpdateLimit.UseVisualStyleBackColor = false;
            this.buttonUpdateLimit.Click += new System.EventHandler(this.buttonUpdateLimit_Click);
            // 
            // ultraComboEditorLimitIDs
            // 
            this.ultraComboEditorLimitIDs.Location = new System.Drawing.Point(627, 7);
            this.ultraComboEditorLimitIDs.Name = "ultraComboEditorLimitIDs";
            this.ultraComboEditorLimitIDs.Size = new System.Drawing.Size(127, 21);
            this.ultraComboEditorLimitIDs.TabIndex = 76;
            this.ultraComboEditorLimitIDs.Text = "Limit IDs";
            this.ultraComboEditorLimitIDs.SelectionChanged += new System.EventHandler(this.ultraComboEditorLimitIDs_SelectionChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(563, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 75;
            this.label1.Text = "LimitID";
            // 
            // ultraGroupBox20
            // 
            this.ultraGroupBox20.Controls.Add(this.MaxDarkROI);
            this.ultraGroupBox20.Controls.Add(this.MaxDarkROIUpperLimit);
            this.ultraGroupBox20.Controls.Add(this.label5);
            this.ultraGroupBox20.Controls.Add(this.MaxDarkROILowerLimit);
            this.ultraGroupBox20.Controls.Add(this.label4);
            this.ultraGroupBox20.Controls.Add(this.MaxDarkROILimit);
            this.ultraGroupBox20.Controls.Add(this.label40);
            this.ultraGroupBox20.Controls.Add(this.label41);
            this.ultraGroupBox20.Location = new System.Drawing.Point(12, 70);
            this.ultraGroupBox20.Name = "ultraGroupBox20";
            this.ultraGroupBox20.Size = new System.Drawing.Size(342, 96);
            this.ultraGroupBox20.TabIndex = 47;
            this.ultraGroupBox20.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // MaxDarkROI
            // 
            this.MaxDarkROI.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.MaxDarkROI.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.MaxDarkROI.Location = new System.Drawing.Point(25, 27);
            this.MaxDarkROI.Name = "MaxDarkROI";
            this.MaxDarkROI.Size = new System.Drawing.Size(65, 21);
            this.MaxDarkROI.TabIndex = 41;
            // 
            // MaxDarkROIUpperLimit
            // 
            this.MaxDarkROIUpperLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaxDarkROIUpperLimit.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(2);
            this.MaxDarkROIUpperLimit.Location = new System.Drawing.Point(286, 26);
            this.MaxDarkROIUpperLimit.Margin = new System.Windows.Forms.Padding(2);
            this.MaxDarkROIUpperLimit.Name = "MaxDarkROIUpperLimit";
            this.MaxDarkROIUpperLimit.Size = new System.Drawing.Size(53, 19);
            this.MaxDarkROIUpperLimit.TabIndex = 40;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(284, 11);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 39;
            this.label5.Text = "Spec UCL";
            // 
            // MaxDarkROILowerLimit
            // 
            this.MaxDarkROILowerLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaxDarkROILowerLimit.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(2);
            this.MaxDarkROILowerLimit.Location = new System.Drawing.Point(155, 26);
            this.MaxDarkROILowerLimit.Margin = new System.Windows.Forms.Padding(2);
            this.MaxDarkROILowerLimit.Name = "MaxDarkROILowerLimit";
            this.MaxDarkROILowerLimit.Size = new System.Drawing.Size(53, 19);
            this.MaxDarkROILowerLimit.TabIndex = 38;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(154, 11);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 37;
            this.label4.Text = "Spec LCL";
            // 
            // MaxDarkROILimit
            // 
            this.MaxDarkROILimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaxDarkROILimit.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(2);
            this.MaxDarkROILimit.Location = new System.Drawing.Point(222, 26);
            this.MaxDarkROILimit.Margin = new System.Windows.Forms.Padding(2);
            this.MaxDarkROILimit.Name = "MaxDarkROILimit";
            this.MaxDarkROILimit.Size = new System.Drawing.Size(53, 19);
            this.MaxDarkROILimit.TabIndex = 36;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.BackColor = System.Drawing.Color.Transparent;
            this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label40.Location = new System.Drawing.Point(220, 11);
            this.label40.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(56, 13);
            this.label40.TabIndex = 35;
            this.label40.Text = "Spec Limit";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.BackColor = System.Drawing.Color.Transparent;
            this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.Location = new System.Drawing.Point(3, 11);
            this.label41.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(129, 13);
            this.label41.TabIndex = 33;
            this.label41.Text = "Max Dark ROI (ADU/sec)";
            // 
            // ultraGroupBox19
            // 
            this.ultraGroupBox19.Controls.Add(this.AveTrapLimit);
            this.ultraGroupBox19.Controls.Add(this.label36);
            this.ultraGroupBox19.Controls.Add(this.labelAveTrap);
            this.ultraGroupBox19.Controls.Add(this.label37);
            this.ultraGroupBox19.Location = new System.Drawing.Point(633, 70);
            this.ultraGroupBox19.Name = "ultraGroupBox19";
            this.ultraGroupBox19.Size = new System.Drawing.Size(121, 96);
            this.ultraGroupBox19.TabIndex = 46;
            this.ultraGroupBox19.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // AveTrapLimit
            // 
            this.AveTrapLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AveTrapLimit.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(2);
            this.AveTrapLimit.Location = new System.Drawing.Point(31, 71);
            this.AveTrapLimit.Margin = new System.Windows.Forms.Padding(2);
            this.AveTrapLimit.Name = "AveTrapLimit";
            this.AveTrapLimit.Size = new System.Drawing.Size(53, 19);
            this.AveTrapLimit.TabIndex = 38;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.BackColor = System.Drawing.Color.Transparent;
            this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.Location = new System.Drawing.Point(27, 52);
            this.label36.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(60, 13);
            this.label36.TabIndex = 35;
            this.label36.Text = "Spec (max)";
            // 
            // labelAveTrap
            // 
            this.labelAveTrap.BackColor = System.Drawing.Color.Transparent;
            this.labelAveTrap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelAveTrap.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAveTrap.Location = new System.Drawing.Point(27, 27);
            this.labelAveTrap.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAveTrap.Name = "labelAveTrap";
            this.labelAveTrap.Size = new System.Drawing.Size(65, 21);
            this.labelAveTrap.TabIndex = 34;
            this.labelAveTrap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.BackColor = System.Drawing.Color.Transparent;
            this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label37.Location = new System.Drawing.Point(18, 10);
            this.label37.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(83, 13);
            this.label37.TabIndex = 33;
            this.label37.Text = "Ave Trap (ADU)";
            // 
            // ultraGroupBox18
            // 
            this.ultraGroupBox18.Controls.Add(this.labelAveDarkFF);
            this.ultraGroupBox18.Controls.Add(this.labelMaxDarkFF);
            this.ultraGroupBox18.Controls.Add(this.label33);
            this.ultraGroupBox18.Controls.Add(this.label32);
            this.ultraGroupBox18.Location = new System.Drawing.Point(355, 70);
            this.ultraGroupBox18.Name = "ultraGroupBox18";
            this.ultraGroupBox18.Size = new System.Drawing.Size(147, 96);
            this.ultraGroupBox18.TabIndex = 45;
            this.ultraGroupBox18.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // labelAveDarkFF
            // 
            this.labelAveDarkFF.BackColor = System.Drawing.Color.Transparent;
            this.labelAveDarkFF.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelAveDarkFF.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAveDarkFF.Location = new System.Drawing.Point(37, 70);
            this.labelAveDarkFF.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAveDarkFF.Name = "labelAveDarkFF";
            this.labelAveDarkFF.Size = new System.Drawing.Size(65, 21);
            this.labelAveDarkFF.TabIndex = 36;
            this.labelAveDarkFF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMaxDarkFF
            // 
            this.labelMaxDarkFF.BackColor = System.Drawing.Color.Transparent;
            this.labelMaxDarkFF.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelMaxDarkFF.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMaxDarkFF.Location = new System.Drawing.Point(39, 27);
            this.labelMaxDarkFF.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMaxDarkFF.Name = "labelMaxDarkFF";
            this.labelMaxDarkFF.Size = new System.Drawing.Size(65, 21);
            this.labelMaxDarkFF.TabIndex = 34;
            this.labelMaxDarkFF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.BackColor = System.Drawing.Color.Transparent;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(10, 10);
            this.label33.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(122, 13);
            this.label33.TabIndex = 33;
            this.label33.Text = "Max Dark FF (ADU/sec)";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.BackColor = System.Drawing.Color.Transparent;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.Location = new System.Drawing.Point(9, 52);
            this.label32.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(121, 13);
            this.label32.TabIndex = 35;
            this.label32.Text = "Ave Dark FF (ADU/sec)";
            // 
            // ultraGroupBox17
            // 
            this.ultraGroupBox17.Controls.Add(this.MaxTrapLimit);
            this.ultraGroupBox17.Controls.Add(this.label3);
            this.ultraGroupBox17.Controls.Add(this.MaxTrapResult);
            this.ultraGroupBox17.Controls.Add(this.label27);
            this.ultraGroupBox17.Location = new System.Drawing.Point(504, 70);
            this.ultraGroupBox17.Name = "ultraGroupBox17";
            this.ultraGroupBox17.Size = new System.Drawing.Size(129, 96);
            this.ultraGroupBox17.TabIndex = 44;
            this.ultraGroupBox17.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // MaxTrapLimit
            // 
            this.MaxTrapLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaxTrapLimit.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateSimpleDoubleMode(2);
            this.MaxTrapLimit.Location = new System.Drawing.Point(31, 71);
            this.MaxTrapLimit.Margin = new System.Windows.Forms.Padding(2);
            this.MaxTrapLimit.Name = "MaxTrapLimit";
            this.MaxTrapLimit.Size = new System.Drawing.Size(53, 19);
            this.MaxTrapLimit.TabIndex = 39;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(27, 55);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 38;
            this.label3.Text = "Spec (max)";
            // 
            // MaxTrapResult
            // 
            this.MaxTrapResult.BackColor = System.Drawing.Color.Transparent;
            this.MaxTrapResult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MaxTrapResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaxTrapResult.Location = new System.Drawing.Point(27, 27);
            this.MaxTrapResult.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.MaxTrapResult.Name = "MaxTrapResult";
            this.MaxTrapResult.Size = new System.Drawing.Size(65, 21);
            this.MaxTrapResult.TabIndex = 34;
            this.MaxTrapResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.BackColor = System.Drawing.Color.Transparent;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.Location = new System.Drawing.Point(19, 10);
            this.label27.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(81, 13);
            this.label27.TabIndex = 33;
            this.label27.Text = "Max Trap(ADU)";
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
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.BackColor = System.Drawing.Color.Transparent;
            this.label62.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label62.Location = new System.Drawing.Point(25, 15);
            this.label62.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(60, 13);
            this.label62.TabIndex = 16;
            this.label62.Text = "Pass/Fail";
            // 
            // ultraExplorerBarContainerControl3
            // 
            this.ultraExplorerBarContainerControl3.Location = new System.Drawing.Point(20, 372);
            this.ultraExplorerBarContainerControl3.Name = "ultraExplorerBarContainerControl3";
            this.ultraExplorerBarContainerControl3.Size = new System.Drawing.Size(781, 700);
            this.ultraExplorerBarContainerControl3.TabIndex = 4;
            // 
            // darkCurrentFormExplorerBar
            // 
            this.darkCurrentFormExplorerBar.AcceptsFocus = Infragistics.Win.DefaultableBoolean.True;
            this.darkCurrentFormExplorerBar.AnimationSpeed = Infragistics.Win.UltraWinExplorerBar.AnimationSpeed.Fast;
            appearance5.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            appearance5.FontData.BoldAsString = "True";
            appearance5.FontData.SizeInPoints = 9F;
            this.darkCurrentFormExplorerBar.Appearance = appearance5;
            this.darkCurrentFormExplorerBar.BorderStyle = Infragistics.Win.UIElementBorderStyle.Rounded4Thick;
            this.darkCurrentFormExplorerBar.Controls.Add(this.ultraExplorerBarContainerControl4);
            this.darkCurrentFormExplorerBar.Controls.Add(this.ultraExplorerBarContainerControl1);
            this.darkCurrentFormExplorerBar.Controls.Add(this.ultraExplorerBarContainerControl3);
            this.darkCurrentFormExplorerBar.Dock = System.Windows.Forms.DockStyle.Fill;
            ultraExplorerBarGroup1.Container = this.ultraExplorerBarContainerControl1;
            ultraExplorerBarGroup1.Settings.ContainerHeight = 58;
            ultraExplorerBarGroup1.Tag = "MVExecute";
            ultraExplorerBarGroup1.Text = "Execute";
            ultraExplorerBarGroup4.Container = this.ultraExplorerBarContainerControl4;
            ultraExplorerBarGroup4.Settings.ContainerHeight = 187;
            ultraExplorerBarGroup4.Tag = "MVTestResults";
            ultraExplorerBarGroup4.Text = "Test Results - Limits";
            ultraExplorerBarGroup3.Container = this.ultraExplorerBarContainerControl3;
            ultraExplorerBarGroup3.Settings.ContainerHeight = 700;
            ultraExplorerBarGroup3.Text = "Exposure Settings";
            this.darkCurrentFormExplorerBar.Groups.AddRange(new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup[] {
            ultraExplorerBarGroup1,
            ultraExplorerBarGroup4,
            ultraExplorerBarGroup3});
            this.darkCurrentFormExplorerBar.GroupSettings.Style = Infragistics.Win.UltraWinExplorerBar.GroupStyle.ControlContainer;
            this.darkCurrentFormExplorerBar.Location = new System.Drawing.Point(0, 18);
            this.darkCurrentFormExplorerBar.Margin = new System.Windows.Forms.Padding(2);
            this.darkCurrentFormExplorerBar.Name = "darkCurrentFormExplorerBar";
            this.darkCurrentFormExplorerBar.ShowDefaultContextMenu = false;
            this.darkCurrentFormExplorerBar.Size = new System.Drawing.Size(838, 795);
            this.darkCurrentFormExplorerBar.TabIndex = 38;
            this.darkCurrentFormExplorerBar.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.darkCurrentFormExplorerBar.ViewStyle = Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarViewStyle.Office2007;
            // 
            // panel4
            // 
            this.panel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel4.Controls.Add(this.darkCurrentLegend);
            this.panel4.Controls.Add(this.darkCurrentWaveformGraph);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(851, 31);
            this.panel4.Margin = new System.Windows.Forms.Padding(2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(571, 813);
            this.panel4.TabIndex = 27;
            this.panel4.Click += new System.EventHandler(this.panel4_Click);
            // 
            // darkCurrentLegend
            // 
            this.darkCurrentLegend.ItemLayoutMode = NationalInstruments.UI.LegendItemLayoutMode.LeftToRight;
            this.darkCurrentLegend.Items.AddRange(new NationalInstruments.UI.LegendItem[] {
            this.TheroryDarkCurrent,
            this.measureDarkCurrent});
            this.darkCurrentLegend.ItemSize = new System.Drawing.Size(30, 22);
            this.darkCurrentLegend.Location = new System.Drawing.Point(5, 14);
            this.darkCurrentLegend.Name = "darkCurrentLegend";
            this.darkCurrentLegend.Size = new System.Drawing.Size(461, 30);
            this.darkCurrentLegend.TabIndex = 9;
            // 
            // TheroryDarkCurrent
            // 
            this.TheroryDarkCurrent.Source = this.MeasuredNoisePlot;
            this.TheroryDarkCurrent.Text = "Therory";
            // 
            // MeasuredNoisePlot
            // 
            this.MeasuredNoisePlot.LineColor = System.Drawing.Color.Blue;
            this.MeasuredNoisePlot.LineColorPrecedence = NationalInstruments.UI.ColorPrecedence.UserDefinedColor;
            this.MeasuredNoisePlot.LineToBaseStyle = NationalInstruments.UI.LineStyle.None;
            this.MeasuredNoisePlot.PointColor = System.Drawing.Color.Blue;
            this.MeasuredNoisePlot.PointStyle = NationalInstruments.UI.PointStyle.SolidCircle;
            this.MeasuredNoisePlot.XAxis = this.xAxis1;
            this.MeasuredNoisePlot.YAxis = this.yAxis1;
            // 
            // yAxis1
            // 
            this.yAxis1.Mode = NationalInstruments.UI.AxisMode.AutoScaleExact;
            // 
            // measureDarkCurrent
            // 
            this.measureDarkCurrent.Source = this.TheoryPlot;
            this.measureDarkCurrent.Text = "Measured Dark Current";
            // 
            // TheoryPlot
            // 
            this.TheoryPlot.AntiAliased = true;
            this.TheoryPlot.LineColor = System.Drawing.Color.Red;
            this.TheoryPlot.LineColorPrecedence = NationalInstruments.UI.ColorPrecedence.UserDefinedColor;
            this.TheoryPlot.PointColor = System.Drawing.Color.Red;
            this.TheoryPlot.PointStyle = NationalInstruments.UI.PointStyle.SolidCircle;
            this.TheoryPlot.XAxis = this.xAxis1;
            this.TheoryPlot.YAxis = this.yAxis1;
            // 
            // darkCurrentWaveformGraph
            // 
            this.darkCurrentWaveformGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkCurrentWaveformGraph.Caption = "Dark Current Plot";
            this.darkCurrentWaveformGraph.Location = new System.Drawing.Point(5, 50);
            this.darkCurrentWaveformGraph.Name = "darkCurrentWaveformGraph";
            this.darkCurrentWaveformGraph.Plots.AddRange(new NationalInstruments.UI.WaveformPlot[] {
            this.TheoryPlot,
            this.MeasuredNoisePlot});
            this.darkCurrentWaveformGraph.Size = new System.Drawing.Size(561, 758);
            this.darkCurrentWaveformGraph.TabIndex = 2;
            this.darkCurrentWaveformGraph.UseColorGenerator = true;
            this.darkCurrentWaveformGraph.XAxes.AddRange(new NationalInstruments.UI.XAxis[] {
            this.xAxis1});
            this.darkCurrentWaveformGraph.YAxes.AddRange(new NationalInstruments.UI.YAxis[] {
            this.yAxis1});
            // 
            // darkCurrentUltraDockManager
            // 
            appearance1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.darkCurrentUltraDockManager.DefaultPaneSettings.ActiveCaptionAppearance = appearance1;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.darkCurrentUltraDockManager.DefaultPaneSettings.ActivePaneAppearance = appearance2;
            appearance3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.darkCurrentUltraDockManager.DefaultPaneSettings.ActiveTabAppearance = appearance3;
            appearance4.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.darkCurrentUltraDockManager.DefaultPaneSettings.Appearance = appearance4;
            dockableControlPane1.Control = this.darkCurrentFormExplorerBar;
            dockableControlPane1.FlyoutSize = new System.Drawing.Size(806, -1);
            dockableControlPane1.OriginalControlBounds = new System.Drawing.Rectangle(349, 28, 796, 796);
            dockableControlPane1.Size = new System.Drawing.Size(100, 100);
            dockableControlPane1.Text = "Test Settings";
            dockAreaPane1.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane1});
            dockAreaPane1.Size = new System.Drawing.Size(838, 813);
            this.darkCurrentUltraDockManager.DockAreas.AddRange(new Infragistics.Win.UltraWinDock.DockAreaPane[] {
            dockAreaPane1});
            this.darkCurrentUltraDockManager.DragWindowStyle = Infragistics.Win.UltraWinDock.DragWindowStyle.LayeredWindowWithIndicators;
            this.darkCurrentUltraDockManager.HostControl = this;
            this.darkCurrentUltraDockManager.ShowCloseButton = false;
            // 
            // _DefectsAndDarkCurrentTestFormUnpinnedTabAreaLeft
            // 
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaLeft.Location = new System.Drawing.Point(8, 31);
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaLeft.Margin = new System.Windows.Forms.Padding(2);
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaLeft.Name = "_DefectsAndDarkCurrentTestFormUnpinnedTabAreaLeft";
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaLeft.Owner = this.darkCurrentUltraDockManager;
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaLeft.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaLeft.Size = new System.Drawing.Size(0, 813);
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaLeft.TabIndex = 28;
            // 
            // _DefectsAndDarkCurrentTestFormUnpinnedTabAreaRight
            // 
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaRight.Location = new System.Drawing.Point(1422, 31);
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaRight.Margin = new System.Windows.Forms.Padding(2);
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaRight.Name = "_DefectsAndDarkCurrentTestFormUnpinnedTabAreaRight";
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaRight.Owner = this.darkCurrentUltraDockManager;
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaRight.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaRight.Size = new System.Drawing.Size(0, 813);
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaRight.TabIndex = 29;
            // 
            // _DefectsAndDarkCurrentTestFormUnpinnedTabAreaTop
            // 
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaTop.Location = new System.Drawing.Point(8, 31);
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaTop.Margin = new System.Windows.Forms.Padding(2);
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaTop.Name = "_DefectsAndDarkCurrentTestFormUnpinnedTabAreaTop";
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaTop.Owner = this.darkCurrentUltraDockManager;
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaTop.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaTop.Size = new System.Drawing.Size(1414, 0);
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaTop.TabIndex = 30;
            // 
            // _DefectsAndDarkCurrentTestFormUnpinnedTabAreaBottom
            // 
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaBottom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaBottom.Location = new System.Drawing.Point(8, 844);
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaBottom.Margin = new System.Windows.Forms.Padding(2);
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaBottom.Name = "_DefectsAndDarkCurrentTestFormUnpinnedTabAreaBottom";
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaBottom.Owner = this.darkCurrentUltraDockManager;
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaBottom.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaBottom.Size = new System.Drawing.Size(1414, 0);
            this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaBottom.TabIndex = 31;
            // 
            // _DefectsAndDarkCurrentTestFormAutoHideControl
            // 
            this._DefectsAndDarkCurrentTestFormAutoHideControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._DefectsAndDarkCurrentTestFormAutoHideControl.Location = new System.Drawing.Point(29, 30);
            this._DefectsAndDarkCurrentTestFormAutoHideControl.Margin = new System.Windows.Forms.Padding(2);
            this._DefectsAndDarkCurrentTestFormAutoHideControl.Name = "_DefectsAndDarkCurrentTestFormAutoHideControl";
            this._DefectsAndDarkCurrentTestFormAutoHideControl.Owner = this.darkCurrentUltraDockManager;
            this._DefectsAndDarkCurrentTestFormAutoHideControl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._DefectsAndDarkCurrentTestFormAutoHideControl.Size = new System.Drawing.Size(811, 814);
            this._DefectsAndDarkCurrentTestFormAutoHideControl.TabIndex = 32;
            // 
            // dockableWindow1
            // 
            this.dockableWindow1.Controls.Add(this.darkCurrentFormExplorerBar);
            this.dockableWindow1.Location = new System.Drawing.Point(0, 0);
            this.dockableWindow1.Name = "dockableWindow1";
            this.dockableWindow1.Owner = this.darkCurrentUltraDockManager;
            this.dockableWindow1.Size = new System.Drawing.Size(838, 813);
            this.dockableWindow1.TabIndex = 44;
            // 
            // darkCurrentUltraFormManager
            // 
            this.darkCurrentUltraFormManager.Form = this;
            this.darkCurrentUltraFormManager.FormStyleSettings.Style = Infragistics.Win.UltraWinForm.UltraFormStyle.Office2010;
            // 
            // _DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Left
            // 
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Left.FormManager = this.darkCurrentUltraFormManager;
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(2);
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Left.Name = "_DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Left";
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 813);
            // 
            // _DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Right
            // 
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Right.FormManager = this.darkCurrentUltraFormManager;
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1422, 31);
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(2);
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Right.Name = "_DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Right";
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 813);
            // 
            // _DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Top
            // 
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Top.FormManager = this.darkCurrentUltraFormManager;
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(2);
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Top.Name = "_DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Top";
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1430, 31);
            // 
            // _DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Bottom
            // 
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Bottom.FormManager = this.darkCurrentUltraFormManager;
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 844);
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(2);
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Bottom.Name = "_DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Bottom";
            this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1430, 8);
            // 
            // windowDockingArea1
            // 
            this.windowDockingArea1.Controls.Add(this.dockableWindow1);
            this.windowDockingArea1.Dock = System.Windows.Forms.DockStyle.Left;
            this.windowDockingArea1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windowDockingArea1.Location = new System.Drawing.Point(8, 31);
            this.windowDockingArea1.Name = "windowDockingArea1";
            this.windowDockingArea1.Owner = this.darkCurrentUltraDockManager;
            this.windowDockingArea1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.windowDockingArea1.Size = new System.Drawing.Size(843, 813);
            this.windowDockingArea1.TabIndex = 39;
            // 
            // darkCurrentUltraToolTipManager
            // 
            this.darkCurrentUltraToolTipManager.ContainingControl = this;
            this.darkCurrentUltraToolTipManager.DisplayStyle = Infragistics.Win.ToolTipDisplayStyle.Office2007;
            // 
            // DarkCurrentTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1430, 852);
            this.Controls.Add(this._DefectsAndDarkCurrentTestFormAutoHideControl);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.windowDockingArea1);
            this.Controls.Add(this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaTop);
            this.Controls.Add(this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaBottom);
            this.Controls.Add(this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaRight);
            this.Controls.Add(this._DefectsAndDarkCurrentTestFormUnpinnedTabAreaLeft);
            this.Controls.Add(this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Bottom);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DarkCurrentTestForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dark Current";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DarkCurrentTestForm_FormClosing);
            this.Load += new System.EventHandler(this.DarkCurrentTestForm_Load);
            this.ultraExplorerBarContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox14)).EndInit();
            this.ultraGroupBox14.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NumExposuresGroupBox)).EndInit();
            this.NumExposuresGroupBox.ResumeLayout(false);
            this.NumExposuresGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericEditNumOfExposures)).EndInit();
            this.ultraExplorerBarContainerControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LimitsGroupBox)).EndInit();
            this.LimitsGroupBox.ResumeLayout(false);
            this.LimitsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraComboEditorLimitIDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox20)).EndInit();
            this.ultraGroupBox20.ResumeLayout(false);
            this.ultraGroupBox20.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxDarkROIUpperLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxDarkROILowerLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxDarkROILimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox19)).EndInit();
            this.ultraGroupBox19.ResumeLayout(false);
            this.ultraGroupBox19.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AveTrapLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox18)).EndInit();
            this.ultraGroupBox18.ResumeLayout(false);
            this.ultraGroupBox18.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox17)).EndInit();
            this.ultraGroupBox17.ResumeLayout(false);
            this.ultraGroupBox17.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxTrapLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ledPassFail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.darkCurrentFormExplorerBar)).EndInit();
            this.darkCurrentFormExplorerBar.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.darkCurrentLegend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.darkCurrentWaveformGraph)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.darkCurrentUltraDockManager)).EndInit();
            this.dockableWindow1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.darkCurrentUltraFormManager)).EndInit();
            this.windowDockingArea1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Panel panel4;
        private Infragistics.Win.UltraWinDock.UltraDockManager darkCurrentUltraDockManager;
        private System.Windows.Forms.Label labelAveTrap;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label labelAveDarkFF;
        private System.Windows.Forms.Label labelMaxDarkFF;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label MaxTrapResult;
        private Infragistics.Win.UltraWinDock.AutoHideControl _DefectsAndDarkCurrentTestFormAutoHideControl;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DefectsAndDarkCurrentTestFormUnpinnedTabAreaTop;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DefectsAndDarkCurrentTestFormUnpinnedTabAreaBottom;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DefectsAndDarkCurrentTestFormUnpinnedTabAreaLeft;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _DefectsAndDarkCurrentTestFormUnpinnedTabAreaRight;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormManager darkCurrentUltraFormManager;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _DefectsAndDarkCurrentTestForm_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar darkCurrentFormExplorerBar;
        private Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarContainerControl ultraExplorerBarContainerControl1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox14;
        private System.Windows.Forms.NumericUpDown numericEditNumOfExposures;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonDarkCurrentRun;
        private System.Windows.Forms.Button buttonDarkCurrentAbort;
        private Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarContainerControl ultraExplorerBarContainerControl4;
        private Infragistics.Win.Misc.UltraGroupBox LimitsGroupBox;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox20;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox19;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox18;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox17;
        private NationalInstruments.UI.WindowsForms.Led ledPassFail;
        private System.Windows.Forms.Label label62;
        private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea1;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow1;
        private NationalInstruments.UI.WindowsForms.Legend darkCurrentLegend;
        public NationalInstruments.UI.LegendItem TheroryDarkCurrent;
        public NationalInstruments.UI.WaveformPlot MeasuredNoisePlot;
        private NationalInstruments.UI.XAxis xAxis1;
        private NationalInstruments.UI.YAxis yAxis1;
        public NationalInstruments.UI.LegendItem measureDarkCurrent;
        public NationalInstruments.UI.WaveformPlot TheoryPlot;
        public NationalInstruments.UI.WindowsForms.WaveformGraph darkCurrentWaveformGraph;
        private Infragistics.Win.UltraWinToolTip.UltraToolTipManager darkCurrentUltraToolTipManager;
        private NationalInstruments.UI.WindowsForms.NumericEdit MaxDarkROILimit;
        private NationalInstruments.UI.WindowsForms.NumericEdit AveTrapLimit;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ultraComboEditorLimitIDs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonUpdateLimit;
        private NationalInstruments.UI.WindowsForms.NumericEdit MaxTrapLimit;
        private System.Windows.Forms.Label label3;
        private Infragistics.Win.Misc.UltraGroupBox NumExposuresGroupBox;
        private System.Windows.Forms.Button buttonRefreshLimit;
        private NationalInstruments.UI.WindowsForms.NumericEdit MaxDarkROIUpperLimit;
        private System.Windows.Forms.Label label5;
        private NationalInstruments.UI.WindowsForms.NumericEdit MaxDarkROILowerLimit;
        private System.Windows.Forms.Label label4;
        private Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarContainerControl ultraExplorerBarContainerControl3;
        private Infragistics.Win.Misc.UltraLabel MaxDarkROI;
    }
}