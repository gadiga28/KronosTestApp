
namespace KronosCameraTestApp.View
{
    partial class TestStationCalibrationReminder
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestStationCalibrationReminder));
            this.ultraGroupBox7 = new Infragistics.Win.Misc.UltraGroupBox();
            this.grpBoxCalibrationDetails = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblCalibratedTestStation = new Infragistics.Win.Misc.UltraLabel();
            this.lblCalibratedLastDate = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.settingsGroupBox = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.btnCalibrationComplete = new System.Windows.Forms.Button();
            this.buttonReminder = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox7)).BeginInit();
            this.ultraGroupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxCalibrationDetails)).BeginInit();
            this.grpBoxCalibrationDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.settingsGroupBox)).BeginInit();
            this.settingsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraGroupBox7
            // 
            this.ultraGroupBox7.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.None;
            this.ultraGroupBox7.Controls.Add(this.grpBoxCalibrationDetails);
            this.ultraGroupBox7.Controls.Add(this.settingsGroupBox);
            this.ultraGroupBox7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox7.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox7.Margin = new System.Windows.Forms.Padding(2);
            this.ultraGroupBox7.Name = "ultraGroupBox7";
            this.ultraGroupBox7.Size = new System.Drawing.Size(449, 425);
            this.ultraGroupBox7.TabIndex = 63;
            this.ultraGroupBox7.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // grpBoxCalibrationDetails
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            this.grpBoxCalibrationDetails.Appearance = appearance1;
            this.grpBoxCalibrationDetails.Controls.Add(this.lblCalibratedTestStation);
            this.grpBoxCalibrationDetails.Controls.Add(this.lblCalibratedLastDate);
            this.grpBoxCalibrationDetails.Controls.Add(this.ultraLabel3);
            this.grpBoxCalibrationDetails.Controls.Add(this.ultraLabel6);
            this.grpBoxCalibrationDetails.Location = new System.Drawing.Point(11, 318);
            this.grpBoxCalibrationDetails.Name = "grpBoxCalibrationDetails";
            this.grpBoxCalibrationDetails.Size = new System.Drawing.Size(427, 97);
            this.grpBoxCalibrationDetails.TabIndex = 81;
            this.grpBoxCalibrationDetails.Text = "Calibration Details";
            this.grpBoxCalibrationDetails.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            this.grpBoxCalibrationDetails.Visible = false;
            // 
            // lblCalibratedTestStation
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.lblCalibratedTestStation.Appearance = appearance2;
            this.lblCalibratedTestStation.Location = new System.Drawing.Point(104, 30);
            this.lblCalibratedTestStation.Name = "lblCalibratedTestStation";
            this.lblCalibratedTestStation.Size = new System.Drawing.Size(149, 23);
            this.lblCalibratedTestStation.TabIndex = 78;
            // 
            // lblCalibratedLastDate
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            this.lblCalibratedLastDate.Appearance = appearance3;
            this.lblCalibratedLastDate.Location = new System.Drawing.Point(104, 68);
            this.lblCalibratedLastDate.Name = "lblCalibratedLastDate";
            this.lblCalibratedLastDate.Size = new System.Drawing.Size(108, 23);
            this.lblCalibratedLastDate.TabIndex = 80;
            // 
            // ultraLabel3
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel3.Appearance = appearance4;
            this.ultraLabel3.Location = new System.Drawing.Point(5, 30);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(75, 23);
            this.ultraLabel3.TabIndex = 77;
            this.ultraLabel3.Text = "Test Station:";
            // 
            // ultraLabel6
            // 
            appearance5.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel6.Appearance = appearance5;
            this.ultraLabel6.Location = new System.Drawing.Point(5, 68);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(87, 23);
            this.ultraLabel6.TabIndex = 79;
            this.ultraLabel6.Text = "Calibrated Date:";
            // 
            // settingsGroupBox
            // 
            this.settingsGroupBox.Controls.Add(this.ultraLabel1);
            this.settingsGroupBox.Controls.Add(this.btnCalibrationComplete);
            this.settingsGroupBox.Controls.Add(this.buttonReminder);
            this.settingsGroupBox.Location = new System.Drawing.Point(11, 11);
            this.settingsGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.settingsGroupBox.Name = "settingsGroupBox";
            this.settingsGroupBox.Size = new System.Drawing.Size(427, 291);
            this.settingsGroupBox.TabIndex = 75;
            this.settingsGroupBox.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // ultraLabel1
            // 
            appearance6.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel1.Appearance = appearance6;
            this.ultraLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(16, 6);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(405, 249);
            this.ultraLabel1.TabIndex = 0;
            this.ultraLabel1.Text = resources.GetString("ultraLabel1.Text");
            // 
            // btnCalibrationComplete
            // 
            this.btnCalibrationComplete.BackColor = System.Drawing.Color.Transparent;
            this.btnCalibrationComplete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalibrationComplete.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalibrationComplete.Location = new System.Drawing.Point(198, 260);
            this.btnCalibrationComplete.Margin = new System.Windows.Forms.Padding(2);
            this.btnCalibrationComplete.Name = "btnCalibrationComplete";
            this.btnCalibrationComplete.Size = new System.Drawing.Size(167, 23);
            this.btnCalibrationComplete.TabIndex = 15;
            this.btnCalibrationComplete.Text = "Calibration Complete";
            this.btnCalibrationComplete.UseVisualStyleBackColor = false;
            this.btnCalibrationComplete.Click += new System.EventHandler(this.btnCalibrationComplete_Click);
            // 
            // buttonReminder
            // 
            this.buttonReminder.BackColor = System.Drawing.Color.Transparent;
            this.buttonReminder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonReminder.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonReminder.Location = new System.Drawing.Point(16, 260);
            this.buttonReminder.Margin = new System.Windows.Forms.Padding(2);
            this.buttonReminder.Name = "buttonReminder";
            this.buttonReminder.Size = new System.Drawing.Size(167, 23);
            this.buttonReminder.TabIndex = 14;
            this.buttonReminder.Text = "Remind Me Again In A Week";
            this.buttonReminder.UseVisualStyleBackColor = false;
            this.buttonReminder.Click += new System.EventHandler(this.buttonReminder_Click);
            // 
            // TestStationCalibrationReminder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 425);
            this.Controls.Add(this.ultraGroupBox7);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(465, 465);
            this.Name = "TestStationCalibrationReminder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Test Station Calibration Reminder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TestStationCalibrationReminder_FormClosing);
            this.Load += new System.EventHandler(this.TestStationCalibrationReminder_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox7)).EndInit();
            this.ultraGroupBox7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxCalibrationDetails)).EndInit();
            this.grpBoxCalibrationDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.settingsGroupBox)).EndInit();
            this.settingsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox7;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private System.Windows.Forms.Button btnCalibrationComplete;
        private System.Windows.Forms.Button buttonReminder;
        private Infragistics.Win.Misc.UltraGroupBox settingsGroupBox;
        private Infragistics.Win.Misc.UltraGroupBox grpBoxCalibrationDetails;
        private Infragistics.Win.Misc.UltraLabel lblCalibratedTestStation;
        private Infragistics.Win.Misc.UltraLabel lblCalibratedLastDate;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
    }
}