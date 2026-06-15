namespace KronosCameraTestApp.View
{
    partial class BurnInAnalysis
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
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BurnInAnalysis));
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.burnInResultOptionSet = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.reportLocation = new System.Windows.Forms.LinkLabel();
            this.burnInAnalysisData = new System.Windows.Forms.RichTextBox();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.burnInResultOptionSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.burnInResultOptionSet);
            this.ultraGroupBox1.Controls.Add(this.reportLocation);
            this.ultraGroupBox1.Controls.Add(this.burnInAnalysisData);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox1.Location = new System.Drawing.Point(8, 32);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(952, 768);
            this.ultraGroupBox1.TabIndex = 0;
            this.ultraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // burnInResultOptionSet
            // 
            this.burnInResultOptionSet.BackColor = System.Drawing.Color.Transparent;
            this.burnInResultOptionSet.BackColorInternal = System.Drawing.Color.Transparent;
            this.burnInResultOptionSet.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.burnInResultOptionSet.GlyphInfo = Infragistics.Win.UIElementDrawParams.Office2007RadioButtonGlyphInfo;
            valueListItem1.DataValue = "Default Item";
            valueListItem1.DisplayText = "Pass";
            valueListItem2.DataValue = "ValueListItem1";
            valueListItem2.DisplayText = "Fail";
            valueListItem3.DataValue = "ValueListItem2";
            valueListItem3.DisplayText = "Cancel";
            this.burnInResultOptionSet.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3});
            this.burnInResultOptionSet.ItemSpacingHorizontal = 10;
            this.burnInResultOptionSet.Location = new System.Drawing.Point(765, 740);
            this.burnInResultOptionSet.Name = "burnInResultOptionSet";
            this.burnInResultOptionSet.Size = new System.Drawing.Size(175, 20);
            this.burnInResultOptionSet.TabIndex = 43;
            this.burnInResultOptionSet.ValueChanged += new System.EventHandler(this.burnInResultOptionSet_ValueChanged);
            // 
            // reportLocation
            // 
            this.reportLocation.AutoSize = true;
            this.reportLocation.BackColor = System.Drawing.Color.Transparent;
            this.reportLocation.Location = new System.Drawing.Point(6, 15);
            this.reportLocation.Name = "reportLocation";
            this.reportLocation.Size = new System.Drawing.Size(109, 13);
            this.reportLocation.TabIndex = 42;
            this.reportLocation.TabStop = true;
            this.reportLocation.Text = "BurnIn Files Location:";
            this.reportLocation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.reportLocation_LinkClicked);
            // 
            // burnInAnalysisData
            // 
            this.burnInAnalysisData.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.burnInAnalysisData.Location = new System.Drawing.Point(9, 31);
            this.burnInAnalysisData.Name = "burnInAnalysisData";
            this.burnInAnalysisData.Size = new System.Drawing.Size(931, 696);
            this.burnInAnalysisData.TabIndex = 6;
            this.burnInAnalysisData.Text = "";
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _BurnInAnalysis_UltraFormManager_Dock_Area_Left
            // 
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Left.Name = "_BurnInAnalysis_UltraFormManager_Dock_Area_Left";
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 768);
            // 
            // _BurnInAnalysis_UltraFormManager_Dock_Area_Right
            // 
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(960, 32);
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Right.Name = "_BurnInAnalysis_UltraFormManager_Dock_Area_Right";
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 768);
            // 
            // _BurnInAnalysis_UltraFormManager_Dock_Area_Top
            // 
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Top.Name = "_BurnInAnalysis_UltraFormManager_Dock_Area_Top";
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(968, 32);
            // 
            // _BurnInAnalysis_UltraFormManager_Dock_Area_Bottom
            // 
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 800);
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Bottom.Name = "_BurnInAnalysis_UltraFormManager_Dock_Area_Bottom";
            this._BurnInAnalysis_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(968, 8);
            // 
            // BurnInAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(968, 808);
            this.Controls.Add(this.ultraGroupBox1);
            this.Controls.Add(this._BurnInAnalysis_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._BurnInAnalysis_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._BurnInAnalysis_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._BurnInAnalysis_UltraFormManager_Dock_Area_Bottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BurnInAnalysis";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Burn-In Analysis";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BurnInAnalysis_FormClosing);
            this.Load += new System.EventHandler(this.BurnInAnalysis_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.burnInResultOptionSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _BurnInAnalysis_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _BurnInAnalysis_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _BurnInAnalysis_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _BurnInAnalysis_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.RichTextBox burnInAnalysisData;
        private System.Windows.Forms.LinkLabel reportLocation;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet burnInResultOptionSet;
    }
}