namespace KronosCameraTestApp.View.UserControls
{
    partial class InjectionEfficiencyLimitsUserControl
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.DefinedDate = new System.Windows.Forms.TextBox();
            this.LimitsID = new System.Windows.Forms.TextBox();
            this.UserModified = new System.Windows.Forms.TextBox();
            this.InjectionEfficiencyLimit = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.ultraLabel57 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel17 = new Infragistics.Win.Misc.UltraLabel();
            this.ECONumber = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InjectionEfficiencyLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DefinedDate
            // 
            this.DefinedDate.Enabled = false;
            this.DefinedDate.Location = new System.Drawing.Point(282, 105);
            this.DefinedDate.Name = "DefinedDate";
            this.DefinedDate.Size = new System.Drawing.Size(33, 20);
            this.DefinedDate.TabIndex = 95;
            this.DefinedDate.Visible = false;
            // 
            // LimitsID
            // 
            this.LimitsID.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.LimitsID.Enabled = false;
            this.LimitsID.Location = new System.Drawing.Point(285, 11);
            this.LimitsID.Name = "LimitsID";
            this.LimitsID.Size = new System.Drawing.Size(78, 20);
            this.LimitsID.TabIndex = 92;
            this.LimitsID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // UserModified
            // 
            this.UserModified.Enabled = false;
            this.UserModified.Location = new System.Drawing.Point(321, 105);
            this.UserModified.Name = "UserModified";
            this.UserModified.Size = new System.Drawing.Size(33, 20);
            this.UserModified.TabIndex = 112;
            this.UserModified.Visible = false;
            // 
            // InjectionEfficiencyLimit
            // 
            this.InjectionEfficiencyLimit.Enabled = false;
            this.InjectionEfficiencyLimit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InjectionEfficiencyLimit.Location = new System.Drawing.Point(194, 67);
            this.InjectionEfficiencyLimit.Margin = new System.Windows.Forms.Padding(2);
            this.InjectionEfficiencyLimit.Name = "InjectionEfficiencyLimit";
            this.InjectionEfficiencyLimit.Size = new System.Drawing.Size(78, 19);
            this.InjectionEfficiencyLimit.TabIndex = 1;
            this.InjectionEfficiencyLimit.Value = 250D;
            // 
            // ultraLabel57
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel57.Appearance = appearance1;
            this.ultraLabel57.AutoSize = true;
            this.ultraLabel57.Location = new System.Drawing.Point(95, 72);
            this.ultraLabel57.Name = "ultraLabel57";
            this.ultraLabel57.Size = new System.Drawing.Size(125, 14);
            this.ultraLabel57.TabIndex = 113;
            this.ultraLabel57.Text = "Injection Efficiency Limit";
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.ultraLabel1);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel17);
            this.ultraGroupBox1.Controls.Add(this.ECONumber);
            this.ultraGroupBox1.Controls.Add(this.InjectionEfficiencyLimit);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel57);
            this.ultraGroupBox1.Controls.Add(this.UserModified);
            this.ultraGroupBox1.Controls.Add(this.DefinedDate);
            this.ultraGroupBox1.Controls.Add(this.LimitsID);
            this.ultraGroupBox1.Location = new System.Drawing.Point(257, 251);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(369, 131);
            this.ultraGroupBox1.TabIndex = 67;
            this.ultraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // ultraLabel1
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel1.Appearance = appearance2;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(241, 14);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(40, 14);
            this.ultraLabel1.TabIndex = 118;
            this.ultraLabel1.Text = "LimitID";
            // 
            // ultraLabel17
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel17.Appearance = appearance3;
            this.ultraLabel17.AutoSize = true;
            this.ultraLabel17.Location = new System.Drawing.Point(6, 11);
            this.ultraLabel17.Name = "ultraLabel17";
            this.ultraLabel17.Size = new System.Drawing.Size(38, 14);
            this.ultraLabel17.TabIndex = 117;
            this.ultraLabel17.Text = "ECO #";
            // 
            // ECONumber
            // 
            this.ECONumber.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ECONumber.Location = new System.Drawing.Point(50, 8);
            this.ECONumber.Name = "ECONumber";
            this.ECONumber.ReadOnly = true;
            this.ECONumber.Size = new System.Drawing.Size(78, 20);
            this.ECONumber.TabIndex = 116;
            this.ECONumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // InjectionEfficiencyLimitsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.Controls.Add(this.ultraGroupBox1);
            this.Name = "InjectionEfficiencyLimitsUserControl";
            this.Size = new System.Drawing.Size(882, 633);
            this.Load += new System.EventHandler(this.InjectionEfficiencyTestLimitsUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InjectionEfficiencyLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private System.Windows.Forms.TextBox DefinedDate;
        private System.Windows.Forms.TextBox LimitsID;
        private System.Windows.Forms.TextBox UserModified;
        private NationalInstruments.UI.WindowsForms.NumericEdit InjectionEfficiencyLimit;
        private Infragistics.Win.Misc.UltraLabel ultraLabel57;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel17;
        private System.Windows.Forms.TextBox ECONumber;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
    }
}
