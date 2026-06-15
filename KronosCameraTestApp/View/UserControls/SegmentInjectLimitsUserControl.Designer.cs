namespace KronosCameraTestApp.View.UserControls
{
    partial class SegmentInjectLimitsUserControl
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
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel17 = new Infragistics.Win.Misc.UltraLabel();
            this.ECONumber = new System.Windows.Forms.TextBox();
            this.InjectDisturbance_adu = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.InjectLevel_adu = new NationalInstruments.UI.WindowsForms.NumericEdit();
            this.ultraLabel57 = new Infragistics.Win.Misc.UltraLabel();
            this.UserModified = new System.Windows.Forms.TextBox();
            this.DefinedDate = new System.Windows.Forms.TextBox();
            this.LimitsID = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InjectDisturbance_adu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InjectLevel_adu)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.ultraLabel2);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel17);
            this.ultraGroupBox1.Controls.Add(this.ECONumber);
            this.ultraGroupBox1.Controls.Add(this.InjectDisturbance_adu);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel1);
            this.ultraGroupBox1.Controls.Add(this.InjectLevel_adu);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel57);
            this.ultraGroupBox1.Controls.Add(this.UserModified);
            this.ultraGroupBox1.Controls.Add(this.DefinedDate);
            this.ultraGroupBox1.Controls.Add(this.LimitsID);
            this.ultraGroupBox1.Location = new System.Drawing.Point(263, 236);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(421, 161);
            this.ultraGroupBox1.TabIndex = 69;
            this.ultraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // ultraLabel2
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel2.Appearance = appearance1;
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Location = new System.Drawing.Point(295, 13);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(40, 14);
            this.ultraLabel2.TabIndex = 118;
            this.ultraLabel2.Text = "LimitID";
            // 
            // ultraLabel17
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel17.Appearance = appearance2;
            this.ultraLabel17.AutoSize = true;
            this.ultraLabel17.Location = new System.Drawing.Point(9, 13);
            this.ultraLabel17.Name = "ultraLabel17";
            this.ultraLabel17.Size = new System.Drawing.Size(38, 14);
            this.ultraLabel17.TabIndex = 117;
            this.ultraLabel17.Text = "ECO #";
            // 
            // ECONumber
            // 
            this.ECONumber.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ECONumber.Location = new System.Drawing.Point(48, 10);
            this.ECONumber.Name = "ECONumber";
            this.ECONumber.ReadOnly = true;
            this.ECONumber.Size = new System.Drawing.Size(94, 20);
            this.ECONumber.TabIndex = 116;
            this.ECONumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // InjectDisturbance_adu
            // 
            this.InjectDisturbance_adu.Enabled = false;
            this.InjectDisturbance_adu.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InjectDisturbance_adu.Location = new System.Drawing.Point(338, 101);
            this.InjectDisturbance_adu.Margin = new System.Windows.Forms.Padding(2);
            this.InjectDisturbance_adu.Name = "InjectDisturbance_adu";
            this.InjectDisturbance_adu.Size = new System.Drawing.Size(78, 19);
            this.InjectDisturbance_adu.TabIndex = 2;
            this.InjectDisturbance_adu.Value = 250D;
            // 
            // ultraLabel1
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel1.Appearance = appearance3;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(212, 103);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(124, 14);
            this.ultraLabel1.TabIndex = 115;
            this.ultraLabel1.Text = "Inject Disturbance (adu)";
            // 
            // InjectLevel_adu
            // 
            this.InjectLevel_adu.Enabled = false;
            this.InjectLevel_adu.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InjectLevel_adu.Location = new System.Drawing.Point(100, 101);
            this.InjectLevel_adu.Margin = new System.Windows.Forms.Padding(2);
            this.InjectLevel_adu.Name = "InjectLevel_adu";
            this.InjectLevel_adu.Size = new System.Drawing.Size(78, 19);
            this.InjectLevel_adu.TabIndex = 1;
            this.InjectLevel_adu.Value = 250D;
            // 
            // ultraLabel57
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel57.Appearance = appearance4;
            this.ultraLabel57.AutoSize = true;
            this.ultraLabel57.Location = new System.Drawing.Point(4, 103);
            this.ultraLabel57.Name = "ultraLabel57";
            this.ultraLabel57.Size = new System.Drawing.Size(91, 14);
            this.ultraLabel57.TabIndex = 113;
            this.ultraLabel57.Text = "Inject Level (adu)";
            // 
            // UserModified
            // 
            this.UserModified.Enabled = false;
            this.UserModified.Location = new System.Drawing.Point(377, 135);
            this.UserModified.Name = "UserModified";
            this.UserModified.Size = new System.Drawing.Size(33, 20);
            this.UserModified.TabIndex = 112;
            this.UserModified.Visible = false;
            // 
            // DefinedDate
            // 
            this.DefinedDate.Enabled = false;
            this.DefinedDate.Location = new System.Drawing.Point(338, 135);
            this.DefinedDate.Name = "DefinedDate";
            this.DefinedDate.Size = new System.Drawing.Size(33, 20);
            this.DefinedDate.TabIndex = 95;
            this.DefinedDate.Visible = false;
            // 
            // LimitsID
            // 
            this.LimitsID.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.LimitsID.Enabled = false;
            this.LimitsID.Location = new System.Drawing.Point(338, 10);
            this.LimitsID.Name = "LimitsID";
            this.LimitsID.Size = new System.Drawing.Size(80, 20);
            this.LimitsID.TabIndex = 92;
            this.LimitsID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SegmentInjectLimitsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.Controls.Add(this.ultraGroupBox1);
            this.Name = "SegmentInjectLimitsUserControl";
            this.Size = new System.Drawing.Size(882, 633);
            this.Load += new System.EventHandler(this.SegmentInjectLimitsUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InjectDisturbance_adu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InjectLevel_adu)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private NationalInstruments.UI.WindowsForms.NumericEdit InjectLevel_adu;
        private Infragistics.Win.Misc.UltraLabel ultraLabel57;
        private System.Windows.Forms.TextBox UserModified;
        private System.Windows.Forms.TextBox DefinedDate;
        private System.Windows.Forms.TextBox LimitsID;
        private NationalInstruments.UI.WindowsForms.NumericEdit InjectDisturbance_adu;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel17;
        private System.Windows.Forms.TextBox ECONumber;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
    }
}
