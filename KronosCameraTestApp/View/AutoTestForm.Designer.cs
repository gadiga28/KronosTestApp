namespace KronosCameraTestApp.View
{
    partial class AutoTestForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoTestForm));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._AutoTestForm_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AutoTestForm_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AutoTestForm_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AutoTestForm_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.AutoTestResultsFilePath = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraPictureBoxTestResultsPath = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.buttonRun = new System.Windows.Forms.Button();
            this.listBoxAvailableTests = new System.Windows.Forms.ListBox();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.textBoxParamterID = new System.Windows.Forms.TextBox();
            this.SaveTestResultsPath = new System.Windows.Forms.Button();
            this.ultraLabelNote = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.AbortTestCheckBox = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.MoveTestDown = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.MoveTestUp = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.ultraGroupBox3 = new Infragistics.Win.Misc.UltraGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AutoTestResultsFilePath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AbortTestCheckBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).BeginInit();
            this.ultraGroupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            this.ultraFormManager1.FormStyleSettings.Style = Infragistics.Win.UltraWinForm.UltraFormStyle.Office2010;
            this.ultraFormManager1.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            // 
            // _AutoTestForm_UltraFormManager_Dock_Area_Left
            // 
            this._AutoTestForm_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AutoTestForm_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AutoTestForm_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._AutoTestForm_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AutoTestForm_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._AutoTestForm_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._AutoTestForm_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._AutoTestForm_UltraFormManager_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(2);
            this._AutoTestForm_UltraFormManager_Dock_Area_Left.Name = "_AutoTestForm_UltraFormManager_Dock_Area_Left";
            this._AutoTestForm_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 328);
            // 
            // _AutoTestForm_UltraFormManager_Dock_Area_Right
            // 
            this._AutoTestForm_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AutoTestForm_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AutoTestForm_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._AutoTestForm_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AutoTestForm_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._AutoTestForm_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._AutoTestForm_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(556, 31);
            this._AutoTestForm_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(2);
            this._AutoTestForm_UltraFormManager_Dock_Area_Right.Name = "_AutoTestForm_UltraFormManager_Dock_Area_Right";
            this._AutoTestForm_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 328);
            // 
            // _AutoTestForm_UltraFormManager_Dock_Area_Top
            // 
            this._AutoTestForm_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AutoTestForm_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AutoTestForm_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._AutoTestForm_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AutoTestForm_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._AutoTestForm_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._AutoTestForm_UltraFormManager_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(2);
            this._AutoTestForm_UltraFormManager_Dock_Area_Top.Name = "_AutoTestForm_UltraFormManager_Dock_Area_Top";
            this._AutoTestForm_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(564, 31);
            // 
            // _AutoTestForm_UltraFormManager_Dock_Area_Bottom
            // 
            this._AutoTestForm_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AutoTestForm_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AutoTestForm_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._AutoTestForm_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AutoTestForm_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._AutoTestForm_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._AutoTestForm_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 359);
            this._AutoTestForm_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(2);
            this._AutoTestForm_UltraFormManager_Dock_Area_Bottom.Name = "_AutoTestForm_UltraFormManager_Dock_Area_Bottom";
            this._AutoTestForm_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(564, 8);
            // 
            // AutoTestResultsFilePath
            // 
            this.AutoTestResultsFilePath.Enabled = false;
            this.AutoTestResultsFilePath.Location = new System.Drawing.Point(7, 34);
            this.AutoTestResultsFilePath.Margin = new System.Windows.Forms.Padding(2);
            this.AutoTestResultsFilePath.Name = "AutoTestResultsFilePath";
            this.AutoTestResultsFilePath.Size = new System.Drawing.Size(411, 21);
            this.AutoTestResultsFilePath.TabIndex = 0;
            // 
            // ultraPictureBoxTestResultsPath
            // 
            this.ultraPictureBoxTestResultsPath.AutoSize = true;
            this.ultraPictureBoxTestResultsPath.BackColor = System.Drawing.Color.Transparent;
            this.ultraPictureBoxTestResultsPath.BorderShadowColor = System.Drawing.Color.Empty;
            this.ultraPictureBoxTestResultsPath.Enabled = false;
            this.ultraPictureBoxTestResultsPath.Image = ((object)(resources.GetObject("ultraPictureBoxTestResultsPath.Image")));
            this.ultraPictureBoxTestResultsPath.ImageTransparentColor = System.Drawing.SystemColors.ActiveCaption;
            this.ultraPictureBoxTestResultsPath.Location = new System.Drawing.Point(425, 30);
            this.ultraPictureBoxTestResultsPath.Margin = new System.Windows.Forms.Padding(2);
            this.ultraPictureBoxTestResultsPath.Name = "ultraPictureBoxTestResultsPath";
            this.ultraPictureBoxTestResultsPath.Size = new System.Drawing.Size(27, 27);
            this.ultraPictureBoxTestResultsPath.TabIndex = 6;
            this.ultraPictureBoxTestResultsPath.Click += new System.EventHandler(this.ultraPictureBoxTestResultsPath_Click);
            // 
            // buttonRun
            // 
            this.buttonRun.BackColor = System.Drawing.Color.Transparent;
            this.buttonRun.Enabled = false;
            this.buttonRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRun.Location = new System.Drawing.Point(463, 147);
            this.buttonRun.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(56, 26);
            this.buttonRun.TabIndex = 44;
            this.buttonRun.Text = "RUN";
            this.buttonRun.UseVisualStyleBackColor = false;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // listBoxAvailableTests
            // 
            this.listBoxAvailableTests.FormattingEnabled = true;
            this.listBoxAvailableTests.Items.AddRange(new object[] {
            "Red, Blue & UV",
            "Defects",
            "Mean Variance",
            "ReadNoiseVsNDRO",
            "Dark Current",
            "Photoresponse",
            "Injection Efficiency",
            "Shutter Drive"});
            this.listBoxAvailableTests.Location = new System.Drawing.Point(7, 37);
            this.listBoxAvailableTests.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxAvailableTests.Name = "listBoxAvailableTests";
            this.listBoxAvailableTests.Size = new System.Drawing.Size(183, 108);
            this.listBoxAvailableTests.TabIndex = 13;
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.Rounded;
            this.ultraGroupBox1.Controls.Add(this.textBoxParamterID);
            this.ultraGroupBox1.Controls.Add(this.SaveTestResultsPath);
            this.ultraGroupBox1.Controls.Add(this.ultraLabelNote);
            this.ultraGroupBox1.Controls.Add(this.AutoTestResultsFilePath);
            this.ultraGroupBox1.Controls.Add(this.ultraPictureBoxTestResultsPath);
            this.ultraGroupBox1.Location = new System.Drawing.Point(5, 5);
            this.ultraGroupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(534, 115);
            this.ultraGroupBox1.TabIndex = 49;
            this.ultraGroupBox1.Text = "Test Data Logging Details ";
            this.ultraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // textBoxParamterID
            // 
            this.textBoxParamterID.Location = new System.Drawing.Point(482, 71);
            this.textBoxParamterID.Name = "textBoxParamterID";
            this.textBoxParamterID.Size = new System.Drawing.Size(37, 20);
            this.textBoxParamterID.TabIndex = 46;
            this.textBoxParamterID.Visible = false;
            // 
            // SaveTestResultsPath
            // 
            this.SaveTestResultsPath.BackColor = System.Drawing.Color.Transparent;
            this.SaveTestResultsPath.Enabled = false;
            this.SaveTestResultsPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveTestResultsPath.Location = new System.Drawing.Point(463, 31);
            this.SaveTestResultsPath.Margin = new System.Windows.Forms.Padding(2);
            this.SaveTestResultsPath.Name = "SaveTestResultsPath";
            this.SaveTestResultsPath.Size = new System.Drawing.Size(56, 26);
            this.SaveTestResultsPath.TabIndex = 45;
            this.SaveTestResultsPath.Text = "Save";
            this.SaveTestResultsPath.UseVisualStyleBackColor = false;
            this.SaveTestResultsPath.Click += new System.EventHandler(this.SaveTestResultsPath_Click);
            // 
            // ultraLabelNote
            // 
            appearance1.FontData.BoldAsString = "True";
            appearance1.TextHAlignAsString = "Left";
            appearance1.TextVAlignAsString = "Middle";
            this.ultraLabelNote.Appearance = appearance1;
            this.ultraLabelNote.Location = new System.Drawing.Point(7, 60);
            this.ultraLabelNote.Name = "ultraLabelNote";
            this.ultraLabelNote.Size = new System.Drawing.Size(411, 49);
            this.ultraLabelNote.TabIndex = 7;
            this.ultraLabelNote.Text = "Note: \r\n1) Only a Admin user can change the Global Test Results Path. \r\n2) Local " +
    "Test Results will be saved at :";
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.Rounded;
            this.ultraGroupBox2.Controls.Add(this.buttonRun);
            this.ultraGroupBox2.Controls.Add(this.AbortTestCheckBox);
            this.ultraGroupBox2.Controls.Add(this.MoveTestDown);
            this.ultraGroupBox2.Controls.Add(this.MoveTestUp);
            this.ultraGroupBox2.Controls.Add(this.listBoxAvailableTests);
            this.ultraGroupBox2.Location = new System.Drawing.Point(5, 126);
            this.ultraGroupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(534, 193);
            this.ultraGroupBox2.TabIndex = 50;
            this.ultraGroupBox2.Text = "List of Characterization Tests to Run:";
            this.ultraGroupBox2.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // AbortTestCheckBox
            // 
            this.AbortTestCheckBox.Location = new System.Drawing.Point(7, 153);
            this.AbortTestCheckBox.Name = "AbortTestCheckBox";
            this.AbortTestCheckBox.Size = new System.Drawing.Size(321, 20);
            this.AbortTestCheckBox.TabIndex = 46;
            this.AbortTestCheckBox.Text = "Abort Auto Test If Any Characterization Test Fails";
            this.AbortTestCheckBox.CheckedChanged += new System.EventHandler(this.AbortTestCheckBox_CheckedChanged);
            // 
            // MoveTestDown
            // 
            this.MoveTestDown.BackColor = System.Drawing.Color.Transparent;
            this.MoveTestDown.BorderShadowColor = System.Drawing.Color.Empty;
            this.MoveTestDown.Image = ((object)(resources.GetObject("MoveTestDown.Image")));
            this.MoveTestDown.Location = new System.Drawing.Point(194, 95);
            this.MoveTestDown.Name = "MoveTestDown";
            this.MoveTestDown.Size = new System.Drawing.Size(48, 35);
            this.MoveTestDown.TabIndex = 45;
            this.MoveTestDown.Visible = false;
            this.MoveTestDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // MoveTestUp
            // 
            this.MoveTestUp.BackColor = System.Drawing.Color.Transparent;
            this.MoveTestUp.BorderShadowColor = System.Drawing.Color.Empty;
            this.MoveTestUp.Image = ((object)(resources.GetObject("MoveTestUp.Image")));
            this.MoveTestUp.Location = new System.Drawing.Point(194, 41);
            this.MoveTestUp.Name = "MoveTestUp";
            this.MoveTestUp.Size = new System.Drawing.Size(48, 35);
            this.MoveTestUp.TabIndex = 44;
            this.MoveTestUp.Visible = false;
            this.MoveTestUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // ultraGroupBox3
            // 
            this.ultraGroupBox3.Controls.Add(this.ultraGroupBox1);
            this.ultraGroupBox3.Controls.Add(this.ultraGroupBox2);
            this.ultraGroupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox3.Location = new System.Drawing.Point(8, 31);
            this.ultraGroupBox3.Name = "ultraGroupBox3";
            this.ultraGroupBox3.Size = new System.Drawing.Size(548, 328);
            this.ultraGroupBox3.TabIndex = 55;
            this.ultraGroupBox3.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // AutoTestForm
            // 
            this.AcceptButton = this.buttonRun;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(564, 367);
            this.Controls.Add(this.ultraGroupBox3);
            this.Controls.Add(this._AutoTestForm_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._AutoTestForm_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._AutoTestForm_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._AutoTestForm_UltraFormManager_Dock_Area_Bottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AutoTestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Auto Test";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AutoTestForm_FormClosing);
            this.Load += new System.EventHandler(this.AutoTestForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AutoTestResultsFilePath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AbortTestCheckBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).EndInit();
            this.ultraGroupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AutoTestForm_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AutoTestForm_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AutoTestForm_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AutoTestForm_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor AutoTestResultsFilePath;
        private Infragistics.Win.UltraWinEditors.UltraPictureBox ultraPictureBoxTestResultsPath;
        private System.Windows.Forms.ListBox listBoxAvailableTests;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private System.Windows.Forms.Button buttonRun;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private Infragistics.Win.UltraWinEditors.UltraPictureBox MoveTestDown;
        private Infragistics.Win.UltraWinEditors.UltraPictureBox MoveTestUp;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor AbortTestCheckBox;
        private Infragistics.Win.Misc.UltraLabel ultraLabelNote;
        private System.Windows.Forms.Button SaveTestResultsPath;

        private System.Windows.Forms.TextBox textBoxParamterID;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox3;
    }
}