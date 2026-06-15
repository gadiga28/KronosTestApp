namespace KronosCameraTestApp.View
{
    partial class DownloadUpdatedFirmware
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadUpdatedFirmware));
            this.downloadFirmwareUltraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.cancelDownloadButton = new System.Windows.Forms.Button();
            this.downloadFirmwareButton = new System.Windows.Forms.Button();
            this.ultraGroupBox3 = new Infragistics.Win.Misc.UltraGroupBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.currentVersionListBox = new System.Windows.Forms.ListBox();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.availableListBox = new System.Windows.Forms.ListBox();
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ultraGroupBox4 = new Infragistics.Win.Misc.UltraGroupBox();
            this.restartFirmware = new System.Windows.Forms.Button();
            this.ultraGroupBox5 = new Infragistics.Win.Misc.UltraGroupBox();
            this.linuxFileTypeRadioButton = new Infragistics.Win.UltraWinEditors.UltraRadioButton();
            this.fpgaFileTypeRadioButton = new Infragistics.Win.UltraWinEditors.UltraRadioButton();
            this.firmwareFileTypeRadioButton = new Infragistics.Win.UltraWinEditors.UltraRadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.downloadFirmwareUltraFormManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).BeginInit();
            this.ultraGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox4)).BeginInit();
            this.ultraGroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox5)).BeginInit();
            this.ultraGroupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.linuxFileTypeRadioButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpgaFileTypeRadioButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.firmwareFileTypeRadioButton)).BeginInit();
            this.SuspendLayout();
            // 
            // downloadFirmwareUltraFormManager
            // 
            this.downloadFirmwareUltraFormManager.Form = this;
            this.downloadFirmwareUltraFormManager.FormStyleSettings.Style = Infragistics.Win.UltraWinForm.UltraFormStyle.Office2010;
            // 
            // cancelDownloadButton
            // 
            this.cancelDownloadButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelDownloadButton.Location = new System.Drawing.Point(340, 498);
            this.cancelDownloadButton.Name = "cancelDownloadButton";
            this.cancelDownloadButton.Size = new System.Drawing.Size(75, 23);
            this.cancelDownloadButton.TabIndex = 5;
            this.cancelDownloadButton.Text = "Cancel";
            this.cancelDownloadButton.UseVisualStyleBackColor = true;
            this.cancelDownloadButton.Click += new System.EventHandler(this.cancelDownloadButton_Click);
            // 
            // downloadFirmwareButton
            // 
            this.downloadFirmwareButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downloadFirmwareButton.Location = new System.Drawing.Point(178, 498);
            this.downloadFirmwareButton.Name = "downloadFirmwareButton";
            this.downloadFirmwareButton.Size = new System.Drawing.Size(75, 23);
            this.downloadFirmwareButton.TabIndex = 4;
            this.downloadFirmwareButton.Text = "Update";
            this.downloadFirmwareButton.UseVisualStyleBackColor = true;
            this.downloadFirmwareButton.Click += new System.EventHandler(this.downloadFirmwareButton_Click);
            // 
            // ultraGroupBox3
            // 
            this.ultraGroupBox3.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.Rounded;
            this.ultraGroupBox3.Controls.Add(this.loginButton);
            this.ultraGroupBox3.Controls.Add(this.label2);
            this.ultraGroupBox3.Controls.Add(this.passwordTextBox);
            this.ultraGroupBox3.Controls.Add(this.label1);
            this.ultraGroupBox3.Controls.Add(this.usernameTextBox);
            this.ultraGroupBox3.Location = new System.Drawing.Point(5, 352);
            this.ultraGroupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.ultraGroupBox3.Name = "ultraGroupBox3";
            this.ultraGroupBox3.Size = new System.Drawing.Size(410, 115);
            this.ultraGroupBox3.TabIndex = 3;
            this.ultraGroupBox3.Text = "Current Version (Server)";
            this.ultraGroupBox3.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            this.ultraGroupBox3.Visible = false;
            // 
            // loginButton
            // 
            this.loginButton.Enabled = false;
            this.loginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loginButton.Location = new System.Drawing.Point(325, 73);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 23);
            this.loginButton.TabIndex = 8;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(10, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Password:";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(85, 74);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(218, 20);
            this.passwordTextBox.TabIndex = 11;
            this.passwordTextBox.UseSystemPasswordChar = true;
            this.passwordTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.passwordTextBox_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(10, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Username:";
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Location = new System.Drawing.Point(85, 38);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(218, 20);
            this.usernameTextBox.TabIndex = 9;
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.Rounded;
            this.ultraGroupBox2.Controls.Add(this.currentVersionListBox);
            this.ultraGroupBox2.Location = new System.Drawing.Point(5, 160);
            this.ultraGroupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(410, 120);
            this.ultraGroupBox2.TabIndex = 2;
            this.ultraGroupBox2.Text = "Current Version (Server)";
            this.ultraGroupBox2.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // currentVersionListBox
            // 
            this.currentVersionListBox.FormattingEnabled = true;
            this.currentVersionListBox.Location = new System.Drawing.Point(5, 19);
            this.currentVersionListBox.Name = "currentVersionListBox";
            this.currentVersionListBox.Size = new System.Drawing.Size(398, 95);
            this.currentVersionListBox.TabIndex = 2;
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.Rounded;
            this.ultraGroupBox1.Controls.Add(this.availableListBox);
            this.ultraGroupBox1.Location = new System.Drawing.Point(5, 5);
            this.ultraGroupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(410, 145);
            this.ultraGroupBox1.TabIndex = 0;
            this.ultraGroupBox1.Text = "Available Versions (Client)";
            this.ultraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // availableListBox
            // 
            this.availableListBox.FormattingEnabled = true;
            this.availableListBox.Location = new System.Drawing.Point(5, 28);
            this.availableListBox.Name = "availableListBox";
            this.availableListBox.Size = new System.Drawing.Size(398, 108);
            this.availableListBox.TabIndex = 1;
            // 
            // _DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Left
            // 
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Left.FormManager = this.downloadFirmwareUltraFormManager;
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(2);
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Left.Name = "_DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Left";
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 528);
            // 
            // _DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Right
            // 
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Right.FormManager = this.downloadFirmwareUltraFormManager;
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(432, 31);
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(2);
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Right.Name = "_DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Right";
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 528);
            // 
            // _DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Top
            // 
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Top.FormManager = this.downloadFirmwareUltraFormManager;
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(2);
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Top.Name = "_DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Top";
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(440, 31);
            // 
            // _DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Bottom
            // 
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Bottom.FormManager = this.downloadFirmwareUltraFormManager;
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 559);
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(2);
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Bottom.Name = "_DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Bottom";
            this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(440, 8);
            // 
            // ultraGroupBox4
            // 
            this.ultraGroupBox4.Controls.Add(this.restartFirmware);
            this.ultraGroupBox4.Controls.Add(this.ultraGroupBox5);
            this.ultraGroupBox4.Controls.Add(this.cancelDownloadButton);
            this.ultraGroupBox4.Controls.Add(this.ultraGroupBox1);
            this.ultraGroupBox4.Controls.Add(this.downloadFirmwareButton);
            this.ultraGroupBox4.Controls.Add(this.ultraGroupBox2);
            this.ultraGroupBox4.Controls.Add(this.ultraGroupBox3);
            this.ultraGroupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox4.Location = new System.Drawing.Point(8, 31);
            this.ultraGroupBox4.Name = "ultraGroupBox4";
            this.ultraGroupBox4.Size = new System.Drawing.Size(424, 528);
            this.ultraGroupBox4.TabIndex = 5;
            this.ultraGroupBox4.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // restartFirmware
            // 
            this.restartFirmware.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.restartFirmware.Location = new System.Drawing.Point(259, 498);
            this.restartFirmware.Name = "restartFirmware";
            this.restartFirmware.Size = new System.Drawing.Size(75, 23);
            this.restartFirmware.TabIndex = 7;
            this.restartFirmware.Text = "Restart";
            this.restartFirmware.UseVisualStyleBackColor = true;
            this.restartFirmware.Click += new System.EventHandler(this.restartFirmware_Click);
            // 
            // ultraGroupBox5
            // 
            this.ultraGroupBox5.Controls.Add(this.linuxFileTypeRadioButton);
            this.ultraGroupBox5.Controls.Add(this.fpgaFileTypeRadioButton);
            this.ultraGroupBox5.Controls.Add(this.firmwareFileTypeRadioButton);
            this.ultraGroupBox5.Location = new System.Drawing.Point(5, 290);
            this.ultraGroupBox5.Name = "ultraGroupBox5";
            this.ultraGroupBox5.Size = new System.Drawing.Size(410, 52);
            this.ultraGroupBox5.TabIndex = 6;
            this.ultraGroupBox5.Text = "File Type";
            this.ultraGroupBox5.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // linuxFileTypeRadioButton
            // 
            this.linuxFileTypeRadioButton.BackColor = System.Drawing.Color.Transparent;
            this.linuxFileTypeRadioButton.BackColorInternal = System.Drawing.Color.Transparent;
            this.linuxFileTypeRadioButton.Location = new System.Drawing.Point(173, 19);
            this.linuxFileTypeRadioButton.Name = "linuxFileTypeRadioButton";
            this.linuxFileTypeRadioButton.Size = new System.Drawing.Size(88, 20);
            this.linuxFileTypeRadioButton.TabIndex = 2;
            this.linuxFileTypeRadioButton.TabStop = false;
            this.linuxFileTypeRadioButton.Text = "Linux(BSP)";
            // 
            // fpgaFileTypeRadioButton
            // 
            this.fpgaFileTypeRadioButton.BackColor = System.Drawing.Color.Transparent;
            this.fpgaFileTypeRadioButton.BackColorInternal = System.Drawing.Color.Transparent;
            this.fpgaFileTypeRadioButton.Location = new System.Drawing.Point(93, 19);
            this.fpgaFileTypeRadioButton.Name = "fpgaFileTypeRadioButton";
            this.fpgaFileTypeRadioButton.Size = new System.Drawing.Size(74, 20);
            this.fpgaFileTypeRadioButton.TabIndex = 1;
            this.fpgaFileTypeRadioButton.TabStop = false;
            this.fpgaFileTypeRadioButton.Text = "FPGA";
            // 
            // firmwareFileTypeRadioButton
            // 
            this.firmwareFileTypeRadioButton.BackColor = System.Drawing.Color.Transparent;
            this.firmwareFileTypeRadioButton.BackColorInternal = System.Drawing.Color.Transparent;
            this.firmwareFileTypeRadioButton.Checked = true;
            this.firmwareFileTypeRadioButton.Location = new System.Drawing.Point(13, 19);
            this.firmwareFileTypeRadioButton.Name = "firmwareFileTypeRadioButton";
            this.firmwareFileTypeRadioButton.Size = new System.Drawing.Size(74, 20);
            this.firmwareFileTypeRadioButton.TabIndex = 0;
            this.firmwareFileTypeRadioButton.Text = "Firmware";
            // 
            // DownloadUpdatedFirmware
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(440, 567);
            this.Controls.Add(this.ultraGroupBox4);
            this.Controls.Add(this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Bottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DownloadUpdatedFirmware";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Download Updated Firmware";
            ((System.ComponentModel.ISupportInitialize)(this.downloadFirmwareUltraFormManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).EndInit();
            this.ultraGroupBox3.ResumeLayout(false);
            this.ultraGroupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox4)).EndInit();
            this.ultraGroupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox5)).EndInit();
            this.ultraGroupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.linuxFileTypeRadioButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpgaFileTypeRadioButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.firmwareFileTypeRadioButton)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinForm.UltraFormManager downloadFirmwareUltraFormManager;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _DownloadFUpdatedFirmware_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.Button cancelDownloadButton;
        private System.Windows.Forms.Button downloadFirmwareButton;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox3;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox usernameTextBox;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private System.Windows.Forms.ListBox currentVersionListBox;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private System.Windows.Forms.ListBox availableListBox;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox4;
        private System.Windows.Forms.Button restartFirmware;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox5;
        private Infragistics.Win.UltraWinEditors.UltraRadioButton linuxFileTypeRadioButton;
        private Infragistics.Win.UltraWinEditors.UltraRadioButton fpgaFileTypeRadioButton;
        private Infragistics.Win.UltraWinEditors.UltraRadioButton firmwareFileTypeRadioButton;
    }
}