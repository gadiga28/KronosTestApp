namespace KronosCameraTestApp.View
{
    partial class TestGraphPreferences
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestGraphPreferences));
            this.testGraphPrefUltraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.ultraCheckEditorMonochromeGraph = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.numericEditPlotLineWidth = new System.Windows.Forms.NumericUpDown();
            this.buttonCancelPreferences = new System.Windows.Forms.Button();
            this.buttonSavePreferences = new System.Windows.Forms.Button();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.testGraphPrefUltraFormManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCheckEditorMonochromeGraph)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericEditPlotLineWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // testGraphPrefUltraFormManager
            // 
            this.testGraphPrefUltraFormManager.Form = this;
            // 
            // ultraCheckEditorMonochromeGraph
            // 
            this.ultraCheckEditorMonochromeGraph.BackColor = System.Drawing.Color.Transparent;
            this.ultraCheckEditorMonochromeGraph.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraCheckEditorMonochromeGraph.Location = new System.Drawing.Point(181, 14);
            this.ultraCheckEditorMonochromeGraph.Name = "ultraCheckEditorMonochromeGraph";
            this.ultraCheckEditorMonochromeGraph.Size = new System.Drawing.Size(130, 25);
            this.ultraCheckEditorMonochromeGraph.TabIndex = 64;
            this.ultraCheckEditorMonochromeGraph.Text = "Monochrome Graphs";
            // 
            // numericEditPlotLineWidth
            // 
            this.numericEditPlotLineWidth.Location = new System.Drawing.Point(112, 16);
            this.numericEditPlotLineWidth.Margin = new System.Windows.Forms.Padding(2);
            this.numericEditPlotLineWidth.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericEditPlotLineWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericEditPlotLineWidth.Name = "numericEditPlotLineWidth";
            this.numericEditPlotLineWidth.Size = new System.Drawing.Size(43, 20);
            this.numericEditPlotLineWidth.TabIndex = 63;
            this.numericEditPlotLineWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // buttonCancelPreferences
            // 
            this.buttonCancelPreferences.BackColor = System.Drawing.Color.Transparent;
            this.buttonCancelPreferences.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancelPreferences.Location = new System.Drawing.Point(236, 67);
            this.buttonCancelPreferences.Name = "buttonCancelPreferences";
            this.buttonCancelPreferences.Size = new System.Drawing.Size(75, 23);
            this.buttonCancelPreferences.TabIndex = 9;
            this.buttonCancelPreferences.Text = "Cancel";
            this.buttonCancelPreferences.UseVisualStyleBackColor = false;
            this.buttonCancelPreferences.Click += new System.EventHandler(this.buttonCancelPreferences_Click);
            // 
            // buttonSavePreferences
            // 
            this.buttonSavePreferences.BackColor = System.Drawing.Color.Transparent;
            this.buttonSavePreferences.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSavePreferences.Location = new System.Drawing.Point(142, 67);
            this.buttonSavePreferences.Name = "buttonSavePreferences";
            this.buttonSavePreferences.Size = new System.Drawing.Size(75, 23);
            this.buttonSavePreferences.TabIndex = 8;
            this.buttonSavePreferences.Text = "OK";
            this.buttonSavePreferences.UseVisualStyleBackColor = false;
            this.buttonSavePreferences.Click += new System.EventHandler(this.buttonSavePreferences_Click);
            // 
            // ultraLabel2
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel2.Appearance = appearance2;
            this.ultraLabel2.Location = new System.Drawing.Point(6, 18);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(101, 21);
            this.ultraLabel2.TabIndex = 4;
            this.ultraLabel2.Text = "Line Width of Plot:";
            // 
            // _TestGraphPreferences_UltraFormManager_Dock_Area_Left
            // 
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Left.FormManager = this.testGraphPrefUltraFormManager;
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Left.Name = "_TestGraphPreferences_UltraFormManager_Dock_Area_Left";
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 118);
            // 
            // _TestGraphPreferences_UltraFormManager_Dock_Area_Right
            // 
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Right.FormManager = this.testGraphPrefUltraFormManager;
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(337, 31);
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Right.Name = "_TestGraphPreferences_UltraFormManager_Dock_Area_Right";
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 118);
            // 
            // _TestGraphPreferences_UltraFormManager_Dock_Area_Top
            // 
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Top.FormManager = this.testGraphPrefUltraFormManager;
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Top.Name = "_TestGraphPreferences_UltraFormManager_Dock_Area_Top";
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(345, 31);
            // 
            // _TestGraphPreferences_UltraFormManager_Dock_Area_Bottom
            // 
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Bottom.FormManager = this.testGraphPrefUltraFormManager;
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 149);
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Bottom.Name = "_TestGraphPreferences_UltraFormManager_Dock_Area_Bottom";
            this._TestGraphPreferences_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(345, 8);
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.buttonCancelPreferences);
            this.ultraGroupBox1.Controls.Add(this.ultraCheckEditorMonochromeGraph);
            this.ultraGroupBox1.Controls.Add(this.buttonSavePreferences);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel2);
            this.ultraGroupBox1.Controls.Add(this.numericEditPlotLineWidth);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox1.Location = new System.Drawing.Point(8, 31);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(329, 118);
            this.ultraGroupBox1.TabIndex = 5;
            this.ultraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // TestGraphPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(345, 157);
            this.Controls.Add(this.ultraGroupBox1);
            this.Controls.Add(this._TestGraphPreferences_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._TestGraphPreferences_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._TestGraphPreferences_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._TestGraphPreferences_UltraFormManager_Dock_Area_Bottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TestGraphPreferences";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Test Graph Preferences";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TestGraphPreferences_FormClosing);
            this.Load += new System.EventHandler(this.TestGraphPreferences_Load);
            ((System.ComponentModel.ISupportInitialize)(this.testGraphPrefUltraFormManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCheckEditorMonochromeGraph)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericEditPlotLineWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinForm.UltraFormManager testGraphPrefUltraFormManager;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TestGraphPreferences_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TestGraphPreferences_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TestGraphPreferences_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TestGraphPreferences_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private System.Windows.Forms.Button buttonCancelPreferences;
        private System.Windows.Forms.Button buttonSavePreferences;
        private System.Windows.Forms.NumericUpDown numericEditPlotLineWidth;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor ultraCheckEditorMonochromeGraph;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
    }
}