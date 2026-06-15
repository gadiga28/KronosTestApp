namespace KronosCameraTestApp.View
{
    partial class TemperatureGraphForm
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemperatureGraphForm));
            this.tempGraphFormUltraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.TemperatureGraphForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.waveformGraph2 = new NationalInstruments.UI.WindowsForms.WaveformGraph();
            this.waveformPlot2 = new NationalInstruments.UI.WaveformPlot();
            this.xAxis2 = new NationalInstruments.UI.XAxis();
            this.yAxis2 = new NationalInstruments.UI.YAxis();
            this.waveformGraph1 = new NationalInstruments.UI.WindowsForms.WaveformGraph();
            this.waveformPlot1 = new NationalInstruments.UI.WaveformPlot();
            this.xAxis1 = new NationalInstruments.UI.XAxis();
            this.yAxis1 = new NationalInstruments.UI.YAxis();
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.TemperatureLimit = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.tempGraphFormUltraFormManager)).BeginInit();
            this.TemperatureGraphForm_Fill_Panel.ClientArea.SuspendLayout();
            this.TemperatureGraphForm_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.waveformGraph2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.waveformGraph1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TemperatureLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // tempGraphFormUltraFormManager
            // 
            this.tempGraphFormUltraFormManager.Form = this;
            // 
            // TemperatureGraphForm_Fill_Panel
            // 
            // 
            // TemperatureGraphForm_Fill_Panel.ClientArea
            // 
            this.TemperatureGraphForm_Fill_Panel.ClientArea.Controls.Add(this.waveformGraph2);
            this.TemperatureGraphForm_Fill_Panel.ClientArea.Controls.Add(this.waveformGraph1);
            this.TemperatureGraphForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.TemperatureGraphForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TemperatureGraphForm_Fill_Panel.Location = new System.Drawing.Point(8, 31);
            this.TemperatureGraphForm_Fill_Panel.Name = "TemperatureGraphForm_Fill_Panel";
            this.TemperatureGraphForm_Fill_Panel.Size = new System.Drawing.Size(974, 740);
            this.TemperatureGraphForm_Fill_Panel.TabIndex = 0;
            // 
            // waveformGraph2
            // 
            this.waveformGraph2.BackColor = System.Drawing.Color.Transparent;
            this.waveformGraph2.Caption = "Relative Humidity";
            this.waveformGraph2.Location = new System.Drawing.Point(15, 440);
            this.waveformGraph2.Name = "waveformGraph2";
            this.waveformGraph2.Plots.AddRange(new NationalInstruments.UI.WaveformPlot[] {
            this.waveformPlot2});
            this.waveformGraph2.Size = new System.Drawing.Size(953, 286);
            this.waveformGraph2.TabIndex = 2;
            this.waveformGraph2.UseColorGenerator = true;
            this.waveformGraph2.XAxes.AddRange(new NationalInstruments.UI.XAxis[] {
            this.xAxis2});
            this.waveformGraph2.YAxes.AddRange(new NationalInstruments.UI.YAxis[] {
            this.yAxis2});
            // 
            // waveformPlot2
            // 
            this.waveformPlot2.XAxis = this.xAxis2;
            this.waveformPlot2.YAxis = this.yAxis2;
            // 
            // waveformGraph1
            // 
            this.waveformGraph1.BackColor = System.Drawing.Color.Transparent;
            this.waveformGraph1.Caption = "Image Sensor Temperature";
            this.waveformGraph1.Location = new System.Drawing.Point(15, 103);
            this.waveformGraph1.Name = "waveformGraph1";
            this.waveformGraph1.Plots.AddRange(new NationalInstruments.UI.WaveformPlot[] {
            this.waveformPlot1});
            this.waveformGraph1.Size = new System.Drawing.Size(953, 286);
            this.waveformGraph1.TabIndex = 1;
            this.waveformGraph1.UseColorGenerator = true;
            this.waveformGraph1.XAxes.AddRange(new NationalInstruments.UI.XAxis[] {
            this.xAxis1});
            this.waveformGraph1.YAxes.AddRange(new NationalInstruments.UI.YAxis[] {
            this.yAxis1});
            // 
            // waveformPlot1
            // 
            this.waveformPlot1.XAxis = this.xAxis1;
            this.waveformPlot1.YAxis = this.yAxis1;
            // 
            // _TemperatureGraphForm_UltraFormManager_Dock_Area_Left
            // 
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Left.FormManager = this.tempGraphFormUltraFormManager;
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Left.Name = "_TemperatureGraphForm_UltraFormManager_Dock_Area_Left";
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 740);
            // 
            // _TemperatureGraphForm_UltraFormManager_Dock_Area_Right
            // 
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Right.FormManager = this.tempGraphFormUltraFormManager;
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(982, 31);
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Right.Name = "_TemperatureGraphForm_UltraFormManager_Dock_Area_Right";
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 740);
            // 
            // _TemperatureGraphForm_UltraFormManager_Dock_Area_Top
            // 
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Top.FormManager = this.tempGraphFormUltraFormManager;
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Top.Name = "_TemperatureGraphForm_UltraFormManager_Dock_Area_Top";
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(990, 31);
            // 
            // _TemperatureGraphForm_UltraFormManager_Dock_Area_Bottom
            // 
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Bottom.FormManager = this.tempGraphFormUltraFormManager;
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 771);
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Bottom.Name = "_TemperatureGraphForm_UltraFormManager_Dock_Area_Bottom";
            this._TemperatureGraphForm_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(990, 8);
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.TemperatureLimit);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel1);
            this.ultraGroupBox1.Location = new System.Drawing.Point(23, 46);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(953, 67);
            this.ultraGroupBox1.TabIndex = 3;
            this.ultraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // TemperatureLimit
            // 
            appearance1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.TemperatureLimit.Appearance = appearance1;
            this.TemperatureLimit.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.TemperatureLimit.Enabled = false;
            this.TemperatureLimit.Location = new System.Drawing.Point(108, 16);
            this.TemperatureLimit.Name = "TemperatureLimit";
            this.TemperatureLimit.Size = new System.Drawing.Size(100, 21);
            this.TemperatureLimit.TabIndex = 2;
            // 
            // ultraLabel1
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel1.Appearance = appearance2;
            this.ultraLabel1.Location = new System.Drawing.Point(12, 20);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel1.TabIndex = 0;
            this.ultraLabel1.Text = "Temperature Limit:";
            // 
            // TemperatureGraphForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(990, 779);
            this.Controls.Add(this.ultraGroupBox1);
            this.Controls.Add(this.TemperatureGraphForm_Fill_Panel);
            this.Controls.Add(this._TemperatureGraphForm_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._TemperatureGraphForm_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._TemperatureGraphForm_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._TemperatureGraphForm_UltraFormManager_Dock_Area_Bottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TemperatureGraphForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Camera Temperature";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TemperatureGraphForm_FormClosing);
            this.Load += new System.EventHandler(this.TemperatureGraphForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tempGraphFormUltraFormManager)).EndInit();
            this.TemperatureGraphForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.TemperatureGraphForm_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.waveformGraph2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.waveformGraph1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TemperatureLimit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinForm.UltraFormManager tempGraphFormUltraFormManager;
        private Infragistics.Win.Misc.UltraPanel TemperatureGraphForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TemperatureGraphForm_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TemperatureGraphForm_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TemperatureGraphForm_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TemperatureGraphForm_UltraFormManager_Dock_Area_Bottom;
        public  NationalInstruments.UI.WindowsForms.WaveformGraph waveformGraph1;
        public NationalInstruments.UI.WaveformPlot waveformPlot1;
        private NationalInstruments.UI.XAxis xAxis1;
        private NationalInstruments.UI.YAxis yAxis1;
        public NationalInstruments.UI.WindowsForms.WaveformGraph waveformGraph2;
        public NationalInstruments.UI.WaveformPlot waveformPlot2;
        private NationalInstruments.UI.XAxis xAxis2;
        private NationalInstruments.UI.YAxis yAxis2;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor TemperatureLimit;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
    }
}