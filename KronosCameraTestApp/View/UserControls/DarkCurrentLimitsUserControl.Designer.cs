namespace KronosCameraTestApp.View.UserControls
{
    partial class DarkCurrentLimitsUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.DarkCurrentUpperLimit = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.DarkCurrentLowerLimit = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel17 = new Infragistics.Win.Misc.UltraLabel();
            this.ECONumber = new System.Windows.Forms.TextBox();
            this.DarkCurrentMaxDarkROI = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.ultraLabel57 = new Infragistics.Win.Misc.UltraLabel();
            this.UserModified = new System.Windows.Forms.TextBox();
            this.DefinedDate = new System.Windows.Forms.TextBox();
            this.LimitsID = new System.Windows.Forms.TextBox();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.DarkCurrentMaxDarkROITol = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DarkCurrentUpperLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DarkCurrentLowerLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DarkCurrentMaxDarkROI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DarkCurrentMaxDarkROITol)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.DarkCurrentMaxDarkROITol);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel4);
            this.ultraGroupBox1.Controls.Add(this.DarkCurrentUpperLimit);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel3);
            this.ultraGroupBox1.Controls.Add(this.DarkCurrentLowerLimit);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel2);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel1);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel17);
            this.ultraGroupBox1.Controls.Add(this.ECONumber);
            this.ultraGroupBox1.Controls.Add(this.DarkCurrentMaxDarkROI);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel57);
            this.ultraGroupBox1.Controls.Add(this.UserModified);
            this.ultraGroupBox1.Controls.Add(this.DefinedDate);
            this.ultraGroupBox1.Controls.Add(this.LimitsID);
            this.ultraGroupBox1.Location = new System.Drawing.Point(157, 250);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(667, 137);
            this.ultraGroupBox1.TabIndex = 65;
            this.ultraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // DarkCurrentUpperLimit
            // 
            this.DarkCurrentUpperLimit.Enabled = false;
            this.DarkCurrentUpperLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DarkCurrentUpperLimit.Location = new System.Drawing.Point(176, 93);
            this.DarkCurrentUpperLimit.Margin = new System.Windows.Forms.Padding(2);
            this.DarkCurrentUpperLimit.Name = "DarkCurrentUpperLimit";
            this.DarkCurrentUpperLimit.Size = new System.Drawing.Size(77, 19);
            this.DarkCurrentUpperLimit.TabIndex = 121;
            this.DarkCurrentUpperLimit.Value = 250D;
            // 
            // ultraLabel3
            // 
            appearance8.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel3.Appearance = appearance8;
            this.ultraLabel3.AutoSize = true;
            this.ultraLabel3.Location = new System.Drawing.Point(14, 93);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(157, 14);
            this.ultraLabel3.TabIndex = 122;
            this.ultraLabel3.Text = "Max Dark ROI UCL (e/pix/sec)";
            // 
            // DarkCurrentLowerLimit
            // 
            this.DarkCurrentLowerLimit.Enabled = false;
            this.DarkCurrentLowerLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DarkCurrentLowerLimit.Location = new System.Drawing.Point(176, 59);
            this.DarkCurrentLowerLimit.Margin = new System.Windows.Forms.Padding(2);
            this.DarkCurrentLowerLimit.Name = "DarkCurrentLowerLimit";
            this.DarkCurrentLowerLimit.Size = new System.Drawing.Size(77, 19);
            this.DarkCurrentLowerLimit.TabIndex = 119;
            this.DarkCurrentLowerLimit.Value = 250D;
            // 
            // ultraLabel2
            // 
            appearance9.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel2.Appearance = appearance9;
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Location = new System.Drawing.Point(16, 59);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(155, 14);
            this.ultraLabel2.TabIndex = 120;
            this.ultraLabel2.Text = "Max Dark ROI LCL (e/pix/sec)";
            // 
            // ultraLabel1
            // 
            appearance10.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel1.Appearance = appearance10;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(240, 13);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(40, 14);
            this.ultraLabel1.TabIndex = 118;
            this.ultraLabel1.Text = "LimitID";
            // 
            // ultraLabel17
            // 
            appearance11.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel17.Appearance = appearance11;
            this.ultraLabel17.AutoSize = true;
            this.ultraLabel17.Location = new System.Drawing.Point(6, 10);
            this.ultraLabel17.Name = "ultraLabel17";
            this.ultraLabel17.Size = new System.Drawing.Size(38, 14);
            this.ultraLabel17.TabIndex = 117;
            this.ultraLabel17.Text = "ECO #";
            // 
            // ECONumber
            // 
            this.ECONumber.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ECONumber.Location = new System.Drawing.Point(50, 7);
            this.ECONumber.Name = "ECONumber";
            this.ECONumber.ReadOnly = true;
            this.ECONumber.Size = new System.Drawing.Size(77, 20);
            this.ECONumber.TabIndex = 116;
            this.ECONumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // DarkCurrentMaxDarkROI
            // 
            this.DarkCurrentMaxDarkROI.Enabled = false;
            this.DarkCurrentMaxDarkROI.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DarkCurrentMaxDarkROI.Location = new System.Drawing.Point(458, 57);
            this.DarkCurrentMaxDarkROI.Margin = new System.Windows.Forms.Padding(2);
            this.DarkCurrentMaxDarkROI.Name = "DarkCurrentMaxDarkROI";
            this.DarkCurrentMaxDarkROI.Size = new System.Drawing.Size(77, 19);
            this.DarkCurrentMaxDarkROI.TabIndex = 1;
            this.DarkCurrentMaxDarkROI.Value = 250D;
            // 
            // ultraLabel57
            // 
            appearance12.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel57.Appearance = appearance12;
            this.ultraLabel57.AutoSize = true;
            this.ultraLabel57.Location = new System.Drawing.Point(298, 59);
            this.ultraLabel57.Name = "ultraLabel57";
            this.ultraLabel57.Size = new System.Drawing.Size(158, 14);
            this.ultraLabel57.TabIndex = 113;
            this.ultraLabel57.Text = "Max Dark ROI Limit (e/pix/sec)";
            // 
            // UserModified
            // 
            this.UserModified.Enabled = false;
            this.UserModified.Location = new System.Drawing.Point(613, 10);
            this.UserModified.Name = "UserModified";
            this.UserModified.Size = new System.Drawing.Size(33, 20);
            this.UserModified.TabIndex = 112;
            this.UserModified.Visible = false;
            // 
            // DefinedDate
            // 
            this.DefinedDate.Enabled = false;
            this.DefinedDate.Location = new System.Drawing.Point(574, 10);
            this.DefinedDate.Name = "DefinedDate";
            this.DefinedDate.Size = new System.Drawing.Size(33, 20);
            this.DefinedDate.TabIndex = 95;
            this.DefinedDate.Visible = false;
            // 
            // LimitsID
            // 
            this.LimitsID.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.LimitsID.Enabled = false;
            this.LimitsID.Location = new System.Drawing.Point(284, 10);
            this.LimitsID.Name = "LimitsID";
            this.LimitsID.Size = new System.Drawing.Size(77, 20);
            this.LimitsID.TabIndex = 92;
            this.LimitsID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // DarkCurrentMaxDarkROITol
            // 
            this.DarkCurrentMaxDarkROITol.Enabled = false;
            this.DarkCurrentMaxDarkROITol.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DarkCurrentMaxDarkROITol.Location = new System.Drawing.Point(458, 88);
            this.DarkCurrentMaxDarkROITol.Margin = new System.Windows.Forms.Padding(2);
            this.DarkCurrentMaxDarkROITol.Name = "DarkCurrentMaxDarkROITol";
            this.DarkCurrentMaxDarkROITol.Size = new System.Drawing.Size(77, 19);
            this.DarkCurrentMaxDarkROITol.TabIndex = 123;
            this.DarkCurrentMaxDarkROITol.Value = 250D;
            // 
            // ultraLabel4
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel4.Appearance = appearance7;
            this.ultraLabel4.AutoSize = true;
            this.ultraLabel4.Location = new System.Drawing.Point(298, 91);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(94, 14);
            this.ultraLabel4.TabIndex = 124;
            this.ultraLabel4.Text = "Max Dark ROI Tol";
            // 
            // DarkCurrentLimitsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.Controls.Add(this.ultraGroupBox1);
            this.Name = "DarkCurrentLimitsUserControl";
            this.Size = new System.Drawing.Size(882, 633);
            this.Load += new System.EventHandler(this.DarkCurrentTestLimitsUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DarkCurrentUpperLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DarkCurrentLowerLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DarkCurrentMaxDarkROI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DarkCurrentMaxDarkROITol)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private System.Windows.Forms.TextBox UserModified;
        private System.Windows.Forms.TextBox DefinedDate;
        private System.Windows.Forms.TextBox LimitsID;
        private NationalInstruments.UI.WindowsForms.NumericEdit DarkCurrentMaxDarkROI;
        private Infragistics.Win.Misc.UltraLabel ultraLabel57;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel17;
        private System.Windows.Forms.TextBox ECONumber;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private NationalInstruments.UI.WindowsForms.NumericEdit DarkCurrentUpperLimit;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private NationalInstruments.UI.WindowsForms.NumericEdit DarkCurrentLowerLimit;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private NationalInstruments.UI.WindowsForms.NumericEdit DarkCurrentMaxDarkROITol;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
    }
}
