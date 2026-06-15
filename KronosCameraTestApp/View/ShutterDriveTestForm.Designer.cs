namespace KronosCameraTestApp.View
{
    partial class ShutterDriveTestForm
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup ultraExplorerBarGroup3 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup ultraExplorerBarGroup5 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup ultraExplorerBarGroup6 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane1 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedLeft, new System.Guid("0d25f2c8-1654-4abd-8b89-6499ce95f45c"));
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane1 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("c9500ec6-8817-4096-8a08-adf62b13aa38"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("0d25f2c8-1654-4abd-8b89-6499ce95f45c"), -1);
            this.ultraExplorerBarContainerControl1 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarContainerControl();
            this.ultraGroupBox7 = new Infragistics.Win.Misc.UltraGroupBox();
            this.NumExposuresGroupBox = new Infragistics.Win.Misc.UltraGroupBox();
            this.numericEditNumOfExposures = new System.Windows.Forms.NumericUpDown();
            this.label34 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.buttonShutterAbort = new System.Windows.Forms.Button();
            this.buttonShutterRun = new System.Windows.Forms.Button();
            this.ultraExplorerBarContainerControl4 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarContainerControl();
            this.LimitsGroupBox = new Infragistics.Win.Misc.UltraGroupBox();
            this.ledPassFail = new NationalInstruments.UI.WindowsForms.Led();
            this.label50 = new System.Windows.Forms.Label();
            this.ultraExplorerBarContainerControl2 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarContainerControl();
            this.shutterDriveExplorerBar = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar();
            this.shutterDriveUltraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.shutterDriveUltraDockManager = new Infragistics.Win.UltraWinDock.UltraDockManager(this.components);
            this._ShutterDriveTestFormUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ShutterDriveTestFormUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ShutterDriveTestFormUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ShutterDriveTestFormUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ShutterDriveTestFormAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
            this.windowDockingArea1 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.dockableWindow1 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Legend = new NationalInstruments.UI.WindowsForms.Legend();
            this.AI0 = new NationalInstruments.UI.LegendItem();
            this.AIO = new NationalInstruments.UI.WaveformPlot();
            this.xAxis1 = new NationalInstruments.UI.XAxis();
            this.yAxis1 = new NationalInstruments.UI.YAxis();
            this.legendItem2 = new NationalInstruments.UI.LegendItem();
            this.AI1 = new NationalInstruments.UI.WaveformPlot();
            this.legendItem3 = new NationalInstruments.UI.LegendItem();
            this.AI2 = new NationalInstruments.UI.WaveformPlot();
            this.shutterWaveformGraph = new NationalInstruments.UI.WindowsForms.WaveformGraph();
            this.shutterDriveUltraToolTipManager = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            this.ultraExplorerBarContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox7)).BeginInit();
            this.ultraGroupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumExposuresGroupBox)).BeginInit();
            this.NumExposuresGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericEditNumOfExposures)).BeginInit();
            this.ultraExplorerBarContainerControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LimitsGroupBox)).BeginInit();
            this.LimitsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ledPassFail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shutterDriveExplorerBar)).BeginInit();
            this.shutterDriveExplorerBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shutterDriveUltraFormManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shutterDriveUltraDockManager)).BeginInit();
            this.windowDockingArea1.SuspendLayout();
            this.dockableWindow1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Legend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shutterWaveformGraph)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraExplorerBarContainerControl1
            // 
            this.ultraExplorerBarContainerControl1.AutoScroll = true;
            this.ultraExplorerBarContainerControl1.Controls.Add(this.ultraGroupBox7);
            this.ultraExplorerBarContainerControl1.Location = new System.Drawing.Point(18, 41);
            this.ultraExplorerBarContainerControl1.Margin = new System.Windows.Forms.Padding(2);
            this.ultraExplorerBarContainerControl1.Name = "ultraExplorerBarContainerControl1";
            this.ultraExplorerBarContainerControl1.Size = new System.Drawing.Size(407, 64);
            this.ultraExplorerBarContainerControl1.TabIndex = 0;
            // 
            // ultraGroupBox7
            // 
            this.ultraGroupBox7.Controls.Add(this.NumExposuresGroupBox);
            this.ultraGroupBox7.Controls.Add(this.buttonShutterAbort);
            this.ultraGroupBox7.Controls.Add(this.buttonShutterRun);
            this.ultraGroupBox7.Location = new System.Drawing.Point(2, 2);
            this.ultraGroupBox7.Margin = new System.Windows.Forms.Padding(2);
            this.ultraGroupBox7.Name = "ultraGroupBox7";
            this.ultraGroupBox7.Size = new System.Drawing.Size(381, 56);
            this.ultraGroupBox7.TabIndex = 22;
            this.ultraGroupBox7.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // NumExposuresGroupBox
            // 
            this.NumExposuresGroupBox.Controls.Add(this.numericEditNumOfExposures);
            this.NumExposuresGroupBox.Controls.Add(this.label34);
            this.NumExposuresGroupBox.Controls.Add(this.label20);
            this.NumExposuresGroupBox.Location = new System.Drawing.Point(187, 19);
            this.NumExposuresGroupBox.Name = "NumExposuresGroupBox";
            this.NumExposuresGroupBox.Size = new System.Drawing.Size(188, 31);
            this.NumExposuresGroupBox.TabIndex = 63;
            this.NumExposuresGroupBox.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // numericEditNumOfExposures
            // 
            this.numericEditNumOfExposures.Enabled = false;
            this.numericEditNumOfExposures.Location = new System.Drawing.Point(48, 5);
            this.numericEditNumOfExposures.Margin = new System.Windows.Forms.Padding(2);
            this.numericEditNumOfExposures.Name = "numericEditNumOfExposures";
            this.numericEditNumOfExposures.Size = new System.Drawing.Size(43, 20);
            this.numericEditNumOfExposures.TabIndex = 65;
            this.numericEditNumOfExposures.ValueChanged += new System.EventHandler(this.numericEditNumOfExposures_ValueChanged);
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.BackColor = System.Drawing.Color.Transparent;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.Location = new System.Drawing.Point(101, 9);
            this.label34.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(78, 13);
            this.label34.TabIndex = 64;
            this.label34.Text = "# of Exposures";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BackColor = System.Drawing.Color.Transparent;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(3, 9);
            this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(49, 13);
            this.label20.TabIndex = 63;
            this.label20.Text = "Test For:";
            // 
            // buttonShutterAbort
            // 
            this.buttonShutterAbort.BackColor = System.Drawing.Color.Transparent;
            this.buttonShutterAbort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonShutterAbort.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonShutterAbort.Location = new System.Drawing.Point(86, 19);
            this.buttonShutterAbort.Margin = new System.Windows.Forms.Padding(2);
            this.buttonShutterAbort.Name = "buttonShutterAbort";
            this.buttonShutterAbort.Size = new System.Drawing.Size(68, 23);
            this.buttonShutterAbort.TabIndex = 13;
            this.buttonShutterAbort.Text = "Abort";
            this.buttonShutterAbort.UseVisualStyleBackColor = false;
            this.buttonShutterAbort.Click += new System.EventHandler(this.buttonShutterDriveAbort_Click);
            // 
            // buttonShutterRun
            // 
            this.buttonShutterRun.BackColor = System.Drawing.Color.Transparent;
            this.buttonShutterRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonShutterRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonShutterRun.Location = new System.Drawing.Point(11, 19);
            this.buttonShutterRun.Margin = new System.Windows.Forms.Padding(2);
            this.buttonShutterRun.Name = "buttonShutterRun";
            this.buttonShutterRun.Size = new System.Drawing.Size(68, 23);
            this.buttonShutterRun.TabIndex = 0;
            this.buttonShutterRun.Text = "Run";
            this.buttonShutterRun.UseVisualStyleBackColor = false;
            this.buttonShutterRun.Click += new System.EventHandler(this.buttonShutterDriveRun_Click);
            // 
            // ultraExplorerBarContainerControl4
            // 
            this.ultraExplorerBarContainerControl4.AutoScroll = true;
            this.ultraExplorerBarContainerControl4.Controls.Add(this.LimitsGroupBox);
            this.ultraExplorerBarContainerControl4.Location = new System.Drawing.Point(18, 147);
            this.ultraExplorerBarContainerControl4.Margin = new System.Windows.Forms.Padding(2);
            this.ultraExplorerBarContainerControl4.Name = "ultraExplorerBarContainerControl4";
            this.ultraExplorerBarContainerControl4.Size = new System.Drawing.Size(407, 61);
            this.ultraExplorerBarContainerControl4.TabIndex = 3;
            // 
            // LimitsGroupBox
            // 
            this.LimitsGroupBox.Controls.Add(this.ledPassFail);
            this.LimitsGroupBox.Controls.Add(this.label50);
            this.LimitsGroupBox.Location = new System.Drawing.Point(3, 3);
            this.LimitsGroupBox.Name = "LimitsGroupBox";
            this.LimitsGroupBox.Size = new System.Drawing.Size(380, 51);
            this.LimitsGroupBox.TabIndex = 19;
            this.LimitsGroupBox.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // ledPassFail
            // 
            this.ledPassFail.LedStyle = NationalInstruments.UI.LedStyle.Square3D;
            this.ledPassFail.Location = new System.Drawing.Point(15, 27);
            this.ledPassFail.Margin = new System.Windows.Forms.Padding(2);
            this.ledPassFail.Name = "ledPassFail";
            this.ledPassFail.Size = new System.Drawing.Size(46, 21);
            this.ledPassFail.TabIndex = 48;
            // 
            // label50
            // 
            this.label50.BackColor = System.Drawing.Color.Transparent;
            this.label50.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label50.Location = new System.Drawing.Point(12, 12);
            this.label50.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(67, 13);
            this.label50.TabIndex = 47;
            this.label50.Text = "Pass/Fail";
            // 
            // ultraExplorerBarContainerControl2
            // 
            this.ultraExplorerBarContainerControl2.AutoScroll = true;
            this.ultraExplorerBarContainerControl2.Location = new System.Drawing.Point(18, 173);
            this.ultraExplorerBarContainerControl2.Margin = new System.Windows.Forms.Padding(2);
            this.ultraExplorerBarContainerControl2.Name = "ultraExplorerBarContainerControl2";
            this.ultraExplorerBarContainerControl2.Size = new System.Drawing.Size(390, 0);
            this.ultraExplorerBarContainerControl2.TabIndex = 1;
            this.ultraExplorerBarContainerControl2.Visible = false;
            // 
            // shutterDriveExplorerBar
            // 
            this.shutterDriveExplorerBar.AcceptsFocus = Infragistics.Win.DefaultableBoolean.True;
            this.shutterDriveExplorerBar.AnimationSpeed = Infragistics.Win.UltraWinExplorerBar.AnimationSpeed.Fast;
            appearance1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            appearance1.FontData.BoldAsString = "True";
            appearance1.FontData.SizeInPoints = 9F;
            this.shutterDriveExplorerBar.Appearance = appearance1;
            this.shutterDriveExplorerBar.Controls.Add(this.ultraExplorerBarContainerControl1);
            this.shutterDriveExplorerBar.Controls.Add(this.ultraExplorerBarContainerControl2);
            this.shutterDriveExplorerBar.Controls.Add(this.ultraExplorerBarContainerControl4);
            ultraExplorerBarGroup3.Container = this.ultraExplorerBarContainerControl1;
            ultraExplorerBarGroup3.Settings.ContainerHeight = 64;
            ultraExplorerBarGroup3.Tag = "MVExecute";
            ultraExplorerBarGroup3.Text = "Execute";
            ultraExplorerBarGroup5.Container = this.ultraExplorerBarContainerControl4;
            ultraExplorerBarGroup5.Settings.ContainerHeight = 61;
            ultraExplorerBarGroup5.Tag = "MVTestResults";
            ultraExplorerBarGroup5.Text = "Test Results";
            ultraExplorerBarGroup6.Container = this.ultraExplorerBarContainerControl2;
            ultraExplorerBarGroup6.Expanded = false;
            ultraExplorerBarGroup6.ItemSettings.AllowDragMove = Infragistics.Win.UltraWinExplorerBar.ItemDragStyle.WithinAndAcrossGroups;
            ultraExplorerBarGroup6.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.True;
            ultraExplorerBarGroup6.Settings.ContainerHeight = 609;
            ultraExplorerBarGroup6.Tag = "MVExposureSettings";
            ultraExplorerBarGroup6.Text = "Exposure Settings";
            this.shutterDriveExplorerBar.Groups.AddRange(new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup[] {
            ultraExplorerBarGroup3,
            ultraExplorerBarGroup5,
            ultraExplorerBarGroup6});
            this.shutterDriveExplorerBar.GroupSettings.Style = Infragistics.Win.UltraWinExplorerBar.GroupStyle.ControlContainer;
            this.shutterDriveExplorerBar.Location = new System.Drawing.Point(0, 18);
            this.shutterDriveExplorerBar.Margin = new System.Windows.Forms.Padding(2);
            this.shutterDriveExplorerBar.Name = "shutterDriveExplorerBar";
            this.shutterDriveExplorerBar.ShowDefaultContextMenu = false;
            this.shutterDriveExplorerBar.Size = new System.Drawing.Size(443, 795);
            this.shutterDriveExplorerBar.TabIndex = 43;
            this.shutterDriveExplorerBar.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.shutterDriveExplorerBar.ViewStyle = Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarViewStyle.Office2007;
            this.shutterDriveExplorerBar.Click += new System.EventHandler(this.ultraExplorerBar1_Click);
            // 
            // shutterDriveUltraFormManager
            // 
            this.shutterDriveUltraFormManager.Form = this;
            this.shutterDriveUltraFormManager.FormStyleSettings.Style = Infragistics.Win.UltraWinForm.UltraFormStyle.Office2010;
            // 
            // _ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Left
            // 
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Left.FormManager = this.shutterDriveUltraFormManager;
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(2);
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Left.Name = "_ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Left";
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 813);
            // 
            // _ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Right
            // 
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Right.FormManager = this.shutterDriveUltraFormManager;
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1423, 31);
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(2);
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Right.Name = "_ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Right";
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 813);
            // 
            // _ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Top
            // 
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Top.FormManager = this.shutterDriveUltraFormManager;
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(2);
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Top.Name = "_ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Top";
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1431, 31);
            // 
            // _ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Bottom
            // 
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Bottom.FormManager = this.shutterDriveUltraFormManager;
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 844);
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(2);
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Bottom.Name = "_ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Bottom";
            this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1431, 8);
            // 
            // shutterDriveUltraDockManager
            // 
            appearance2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.shutterDriveUltraDockManager.DefaultPaneSettings.ActiveCaptionAppearance = appearance2;
            appearance3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.shutterDriveUltraDockManager.DefaultPaneSettings.ActivePaneAppearance = appearance3;
            appearance4.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.shutterDriveUltraDockManager.DefaultPaneSettings.ActiveTabAppearance = appearance4;
            appearance5.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.shutterDriveUltraDockManager.DefaultPaneSettings.Appearance = appearance5;
            dockableControlPane1.Control = this.shutterDriveExplorerBar;
            dockableControlPane1.OriginalControlBounds = new System.Drawing.Rectangle(470, 31, 447, 795);
            dockableControlPane1.Size = new System.Drawing.Size(100, 100);
            dockableControlPane1.Text = "Test Settings";
            dockAreaPane1.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane1});
            dockAreaPane1.Size = new System.Drawing.Size(443, 813);
            this.shutterDriveUltraDockManager.DockAreas.AddRange(new Infragistics.Win.UltraWinDock.DockAreaPane[] {
            dockAreaPane1});
            this.shutterDriveUltraDockManager.DragWindowStyle = Infragistics.Win.UltraWinDock.DragWindowStyle.LayeredWindowWithIndicators;
            this.shutterDriveUltraDockManager.HostControl = this;
            this.shutterDriveUltraDockManager.ShowCloseButton = false;
            // 
            // _ShutterDriveTestFormUnpinnedTabAreaLeft
            // 
            this._ShutterDriveTestFormUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this._ShutterDriveTestFormUnpinnedTabAreaLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ShutterDriveTestFormUnpinnedTabAreaLeft.Location = new System.Drawing.Point(8, 31);
            this._ShutterDriveTestFormUnpinnedTabAreaLeft.Name = "_ShutterDriveTestFormUnpinnedTabAreaLeft";
            this._ShutterDriveTestFormUnpinnedTabAreaLeft.Owner = this.shutterDriveUltraDockManager;
            this._ShutterDriveTestFormUnpinnedTabAreaLeft.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._ShutterDriveTestFormUnpinnedTabAreaLeft.Size = new System.Drawing.Size(0, 813);
            this._ShutterDriveTestFormUnpinnedTabAreaLeft.TabIndex = 5;
            // 
            // _ShutterDriveTestFormUnpinnedTabAreaRight
            // 
            this._ShutterDriveTestFormUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
            this._ShutterDriveTestFormUnpinnedTabAreaRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ShutterDriveTestFormUnpinnedTabAreaRight.Location = new System.Drawing.Point(1423, 31);
            this._ShutterDriveTestFormUnpinnedTabAreaRight.Name = "_ShutterDriveTestFormUnpinnedTabAreaRight";
            this._ShutterDriveTestFormUnpinnedTabAreaRight.Owner = this.shutterDriveUltraDockManager;
            this._ShutterDriveTestFormUnpinnedTabAreaRight.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._ShutterDriveTestFormUnpinnedTabAreaRight.Size = new System.Drawing.Size(0, 813);
            this._ShutterDriveTestFormUnpinnedTabAreaRight.TabIndex = 6;
            // 
            // _ShutterDriveTestFormUnpinnedTabAreaTop
            // 
            this._ShutterDriveTestFormUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._ShutterDriveTestFormUnpinnedTabAreaTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ShutterDriveTestFormUnpinnedTabAreaTop.Location = new System.Drawing.Point(8, 31);
            this._ShutterDriveTestFormUnpinnedTabAreaTop.Name = "_ShutterDriveTestFormUnpinnedTabAreaTop";
            this._ShutterDriveTestFormUnpinnedTabAreaTop.Owner = this.shutterDriveUltraDockManager;
            this._ShutterDriveTestFormUnpinnedTabAreaTop.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._ShutterDriveTestFormUnpinnedTabAreaTop.Size = new System.Drawing.Size(1415, 0);
            this._ShutterDriveTestFormUnpinnedTabAreaTop.TabIndex = 7;
            // 
            // _ShutterDriveTestFormUnpinnedTabAreaBottom
            // 
            this._ShutterDriveTestFormUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._ShutterDriveTestFormUnpinnedTabAreaBottom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ShutterDriveTestFormUnpinnedTabAreaBottom.Location = new System.Drawing.Point(8, 844);
            this._ShutterDriveTestFormUnpinnedTabAreaBottom.Name = "_ShutterDriveTestFormUnpinnedTabAreaBottom";
            this._ShutterDriveTestFormUnpinnedTabAreaBottom.Owner = this.shutterDriveUltraDockManager;
            this._ShutterDriveTestFormUnpinnedTabAreaBottom.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._ShutterDriveTestFormUnpinnedTabAreaBottom.Size = new System.Drawing.Size(1415, 0);
            this._ShutterDriveTestFormUnpinnedTabAreaBottom.TabIndex = 8;
            // 
            // _ShutterDriveTestFormAutoHideControl
            // 
            this._ShutterDriveTestFormAutoHideControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ShutterDriveTestFormAutoHideControl.Location = new System.Drawing.Point(0, 0);
            this._ShutterDriveTestFormAutoHideControl.Name = "_ShutterDriveTestFormAutoHideControl";
            this._ShutterDriveTestFormAutoHideControl.Owner = this.shutterDriveUltraDockManager;
            this._ShutterDriveTestFormAutoHideControl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._ShutterDriveTestFormAutoHideControl.Size = new System.Drawing.Size(0, 0);
            this._ShutterDriveTestFormAutoHideControl.TabIndex = 9;
            // 
            // windowDockingArea1
            // 
            this.windowDockingArea1.Controls.Add(this.dockableWindow1);
            this.windowDockingArea1.Dock = System.Windows.Forms.DockStyle.Left;
            this.windowDockingArea1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windowDockingArea1.Location = new System.Drawing.Point(8, 31);
            this.windowDockingArea1.Name = "windowDockingArea1";
            this.windowDockingArea1.Owner = this.shutterDriveUltraDockManager;
            this.windowDockingArea1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.windowDockingArea1.Size = new System.Drawing.Size(448, 813);
            this.windowDockingArea1.TabIndex = 44;
            // 
            // dockableWindow1
            // 
            this.dockableWindow1.Controls.Add(this.shutterDriveExplorerBar);
            this.dockableWindow1.Location = new System.Drawing.Point(0, 0);
            this.dockableWindow1.Name = "dockableWindow1";
            this.dockableWindow1.Owner = this.shutterDriveUltraDockManager;
            this.dockableWindow1.Size = new System.Drawing.Size(443, 813);
            this.dockableWindow1.TabIndex = 50;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Legend);
            this.panel1.Controls.Add(this.shutterWaveformGraph);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(456, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(967, 813);
            this.panel1.TabIndex = 45;
            this.panel1.Click += new System.EventHandler(this.panel1_Click);
            // 
            // Legend
            // 
            this.Legend.ItemLayoutMode = NationalInstruments.UI.LegendItemLayoutMode.LeftToRight;
            this.Legend.Items.AddRange(new NationalInstruments.UI.LegendItem[] {
            this.AI0,
            this.legendItem2,
            this.legendItem3});
            this.Legend.ItemSize = new System.Drawing.Size(30, 22);
            this.Legend.Location = new System.Drawing.Point(10, 3);
            this.Legend.Name = "Legend";
            this.Legend.Size = new System.Drawing.Size(157, 30);
            this.Legend.TabIndex = 13;
            // 
            // AI0
            // 
            this.AI0.Source = this.AIO;
            this.AI0.Text = "AI0";
            // 
            // AIO
            // 
            this.AIO.PointColor = System.Drawing.Color.Lime;
            this.AIO.PointStyle = NationalInstruments.UI.PointStyle.SolidSquare;
            this.AIO.XAxis = this.xAxis1;
            this.AIO.YAxis = this.yAxis1;
            // 
            // xAxis1
            // 
            this.xAxis1.Caption = "Time";
            // 
            // yAxis1
            // 
            this.yAxis1.Caption = "Volts";
            this.yAxis1.Range = new NationalInstruments.UI.Range(8.5D, 9.6D);
            // 
            // legendItem2
            // 
            this.legendItem2.Source = this.AI1;
            this.legendItem2.Text = "(AI1 - AI0)";
            // 
            // AI1
            // 
            this.AI1.PointColor = System.Drawing.Color.Red;
            this.AI1.PointStyle = NationalInstruments.UI.PointStyle.SolidSquare;
            this.AI1.Visible = false;
            this.AI1.XAxis = this.xAxis1;
            this.AI1.YAxis = this.yAxis1;
            // 
            // legendItem3
            // 
            this.legendItem3.Source = this.AI2;
            this.legendItem3.Text = "AI2";
            this.legendItem3.Visible = false;
            // 
            // AI2
            // 
            this.AI2.LineColor = System.Drawing.Color.DodgerBlue;
            this.AI2.LineColorPrecedence = NationalInstruments.UI.ColorPrecedence.UserDefinedColor;
            this.AI2.PointColor = System.Drawing.Color.DodgerBlue;
            this.AI2.PointStyle = NationalInstruments.UI.PointStyle.SolidSquare;
            this.AI2.Visible = false;
            this.AI2.XAxis = this.xAxis1;
            this.AI2.YAxis = this.yAxis1;
            // 
            // shutterWaveformGraph
            // 
            this.shutterWaveformGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.shutterWaveformGraph.Location = new System.Drawing.Point(10, 48);
            this.shutterWaveformGraph.Name = "shutterWaveformGraph";
            this.shutterWaveformGraph.Plots.AddRange(new NationalInstruments.UI.WaveformPlot[] {
            this.AIO,
            this.AI1,
            this.AI2});
            this.shutterWaveformGraph.Size = new System.Drawing.Size(947, 759);
            this.shutterWaveformGraph.TabIndex = 11;
            this.shutterWaveformGraph.UseColorGenerator = true;
            this.shutterWaveformGraph.XAxes.AddRange(new NationalInstruments.UI.XAxis[] {
            this.xAxis1});
            this.shutterWaveformGraph.YAxes.AddRange(new NationalInstruments.UI.YAxis[] {
            this.yAxis1});
            // 
            // shutterDriveUltraToolTipManager
            // 
            this.shutterDriveUltraToolTipManager.ContainingControl = this;
            this.shutterDriveUltraToolTipManager.DisplayStyle = Infragistics.Win.ToolTipDisplayStyle.Office2007;
            // 
            // ShutterDriveTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1431, 852);
            this.Controls.Add(this._ShutterDriveTestFormAutoHideControl);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.windowDockingArea1);
            this.Controls.Add(this._ShutterDriveTestFormUnpinnedTabAreaTop);
            this.Controls.Add(this._ShutterDriveTestFormUnpinnedTabAreaBottom);
            this.Controls.Add(this._ShutterDriveTestFormUnpinnedTabAreaLeft);
            this.Controls.Add(this._ShutterDriveTestFormUnpinnedTabAreaRight);
            this.Controls.Add(this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Bottom);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ShutterDriveTestForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Shutter Drive";
            this.Load += new System.EventHandler(this.ShutterDriveTestForm_Load);
            this.ultraExplorerBarContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox7)).EndInit();
            this.ultraGroupBox7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NumExposuresGroupBox)).EndInit();
            this.NumExposuresGroupBox.ResumeLayout(false);
            this.NumExposuresGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericEditNumOfExposures)).EndInit();
            this.ultraExplorerBarContainerControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LimitsGroupBox)).EndInit();
            this.LimitsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ledPassFail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.shutterDriveExplorerBar)).EndInit();
            this.shutterDriveExplorerBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.shutterDriveUltraFormManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.shutterDriveUltraDockManager)).EndInit();
            this.windowDockingArea1.ResumeLayout(false);
            this.dockableWindow1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Legend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.shutterWaveformGraph)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinForm.UltraFormManager shutterDriveUltraFormManager;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ShutterDriveAndRS232TestForm_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar shutterDriveExplorerBar;
        private Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarContainerControl ultraExplorerBarContainerControl1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox7;
        private Infragistics.Win.Misc.UltraGroupBox NumExposuresGroupBox;
        private System.Windows.Forms.NumericUpDown numericEditNumOfExposures;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button buttonShutterAbort;
        private System.Windows.Forms.Button buttonShutterRun;
        private Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarContainerControl ultraExplorerBarContainerControl2;
        private Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarContainerControl ultraExplorerBarContainerControl4;
        private Infragistics.Win.Misc.UltraGroupBox LimitsGroupBox;
        private Infragistics.Win.UltraWinDock.AutoHideControl _ShutterDriveTestFormAutoHideControl;
        private Infragistics.Win.UltraWinDock.UltraDockManager shutterDriveUltraDockManager;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ShutterDriveTestFormUnpinnedTabAreaTop;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ShutterDriveTestFormUnpinnedTabAreaBottom;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ShutterDriveTestFormUnpinnedTabAreaLeft;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ShutterDriveTestFormUnpinnedTabAreaRight;
        private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea1;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow1;
        private NationalInstruments.UI.WindowsForms.Led ledPassFail;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Panel panel1;
        private NationalInstruments.UI.WindowsForms.Legend Legend;
        private NationalInstruments.UI.LegendItem AI0;
        public NationalInstruments.UI.WaveformPlot AIO;
        private NationalInstruments.UI.XAxis xAxis1;
        private NationalInstruments.UI.YAxis yAxis1;
        private NationalInstruments.UI.LegendItem legendItem2;
        public NationalInstruments.UI.WaveformPlot AI1;
        private NationalInstruments.UI.LegendItem legendItem3;
        public NationalInstruments.UI.WaveformPlot AI2;
        public NationalInstruments.UI.WindowsForms.WaveformGraph shutterWaveformGraph;
		private Infragistics.Win.UltraWinToolTip.UltraToolTipManager shutterDriveUltraToolTipManager;
    }
}