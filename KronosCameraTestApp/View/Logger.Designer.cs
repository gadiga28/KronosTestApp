namespace KronosCameraTestApp.View
{
    partial class Logger
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
            this.firmwareLoggerUltraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.Logger_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.filterLevelGroupBox = new Infragistics.Win.Misc.UltraGroupBox();
            this.enableFWLogCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.debugRadioButton = new System.Windows.Forms.RadioButton();
            this.fatalRadioButton = new System.Windows.Forms.RadioButton();
            this.errorRadioButton = new System.Windows.Forms.RadioButton();
            this.warningRadioButton = new System.Windows.Forms.RadioButton();
            this.infoRadioButton = new System.Windows.Forms.RadioButton();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.noneRadioButton = new System.Windows.Forms.RadioButton();
            this.fileRadioButton = new System.Windows.Forms.RadioButton();
            this.consoleRadioButton = new System.Windows.Forms.RadioButton();
            this._Logger_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._Logger_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._Logger_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._Logger_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.firmwareLoggerUltraFormManager)).BeginInit();
            this.Logger_Fill_Panel.ClientArea.SuspendLayout();
            this.Logger_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filterLevelGroupBox)).BeginInit();
            this.filterLevelGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // firmwareLoggerUltraFormManager
            // 
            this.firmwareLoggerUltraFormManager.Form = this;
            // 
            // Logger_Fill_Panel
            // 
            // 
            // Logger_Fill_Panel.ClientArea
            // 
            this.Logger_Fill_Panel.ClientArea.Controls.Add(this.buttonCancel);
            this.Logger_Fill_Panel.ClientArea.Controls.Add(this.buttonOK);
            this.Logger_Fill_Panel.ClientArea.Controls.Add(this.filterLevelGroupBox);
            this.Logger_Fill_Panel.ClientArea.Controls.Add(this.ultraGroupBox1);
            this.Logger_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.Logger_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Logger_Fill_Panel.Location = new System.Drawing.Point(8, 32);
            this.Logger_Fill_Panel.Name = "Logger_Fill_Panel";
            this.Logger_Fill_Panel.Size = new System.Drawing.Size(270, 416);
            this.Logger_Fill_Panel.TabIndex = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Location = new System.Drawing.Point(171, 380);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.BackColor = System.Drawing.Color.Transparent;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOK.Location = new System.Drawing.Point(22, 380);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // filterLevelGroupBox
            // 
            this.filterLevelGroupBox.Controls.Add(this.enableFWLogCheckBox);
            this.filterLevelGroupBox.Controls.Add(this.label1);
            this.filterLevelGroupBox.Controls.Add(this.debugRadioButton);
            this.filterLevelGroupBox.Controls.Add(this.fatalRadioButton);
            this.filterLevelGroupBox.Controls.Add(this.errorRadioButton);
            this.filterLevelGroupBox.Controls.Add(this.warningRadioButton);
            this.filterLevelGroupBox.Controls.Add(this.infoRadioButton);
            this.filterLevelGroupBox.Enabled = false;
            this.filterLevelGroupBox.Location = new System.Drawing.Point(22, 148);
            this.filterLevelGroupBox.Name = "filterLevelGroupBox";
            this.filterLevelGroupBox.Size = new System.Drawing.Size(224, 215);
            this.filterLevelGroupBox.TabIndex = 1;
            this.filterLevelGroupBox.Text = "Select Filter Level";
            this.filterLevelGroupBox.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // enableFWLogCheckBox
            // 
            this.enableFWLogCheckBox.AutoSize = true;
            this.enableFWLogCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.enableFWLogCheckBox.Location = new System.Drawing.Point(17, 182);
            this.enableFWLogCheckBox.Name = "enableFWLogCheckBox";
            this.enableFWLogCheckBox.Size = new System.Drawing.Size(145, 17);
            this.enableFWLogCheckBox.TabIndex = 17;
            this.enableFWLogCheckBox.Text = "Enable Firmware Logging";
            this.enableFWLogCheckBox.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(6, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "*Include logging from selected level down";
            // 
            // debugRadioButton
            // 
            this.debugRadioButton.AutoSize = true;
            this.debugRadioButton.BackColor = System.Drawing.Color.Transparent;
            this.debugRadioButton.Location = new System.Drawing.Point(18, 57);
            this.debugRadioButton.Name = "debugRadioButton";
            this.debugRadioButton.Size = new System.Drawing.Size(57, 17);
            this.debugRadioButton.TabIndex = 15;
            this.debugRadioButton.TabStop = true;
            this.debugRadioButton.Text = "Debug";
            this.debugRadioButton.UseVisualStyleBackColor = false;
            // 
            // fatalRadioButton
            // 
            this.fatalRadioButton.AutoSize = true;
            this.fatalRadioButton.BackColor = System.Drawing.Color.Transparent;
            this.fatalRadioButton.Location = new System.Drawing.Point(17, 150);
            this.fatalRadioButton.Name = "fatalRadioButton";
            this.fatalRadioButton.Size = new System.Drawing.Size(48, 17);
            this.fatalRadioButton.TabIndex = 14;
            this.fatalRadioButton.TabStop = true;
            this.fatalRadioButton.Text = "Fatal";
            this.fatalRadioButton.UseVisualStyleBackColor = false;
            // 
            // errorRadioButton
            // 
            this.errorRadioButton.AutoSize = true;
            this.errorRadioButton.BackColor = System.Drawing.Color.Transparent;
            this.errorRadioButton.Location = new System.Drawing.Point(17, 128);
            this.errorRadioButton.Name = "errorRadioButton";
            this.errorRadioButton.Size = new System.Drawing.Size(47, 17);
            this.errorRadioButton.TabIndex = 13;
            this.errorRadioButton.TabStop = true;
            this.errorRadioButton.Text = "Error";
            this.errorRadioButton.UseVisualStyleBackColor = false;
            // 
            // warningRadioButton
            // 
            this.warningRadioButton.AutoSize = true;
            this.warningRadioButton.BackColor = System.Drawing.Color.Transparent;
            this.warningRadioButton.Location = new System.Drawing.Point(17, 105);
            this.warningRadioButton.Name = "warningRadioButton";
            this.warningRadioButton.Size = new System.Drawing.Size(65, 17);
            this.warningRadioButton.TabIndex = 12;
            this.warningRadioButton.TabStop = true;
            this.warningRadioButton.Text = "Warning";
            this.warningRadioButton.UseVisualStyleBackColor = false;
            // 
            // infoRadioButton
            // 
            this.infoRadioButton.AutoSize = true;
            this.infoRadioButton.BackColor = System.Drawing.Color.Transparent;
            this.infoRadioButton.Location = new System.Drawing.Point(18, 81);
            this.infoRadioButton.Name = "infoRadioButton";
            this.infoRadioButton.Size = new System.Drawing.Size(85, 17);
            this.infoRadioButton.TabIndex = 11;
            this.infoRadioButton.TabStop = true;
            this.infoRadioButton.Text = "Informational";
            this.infoRadioButton.UseVisualStyleBackColor = false;
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.noneRadioButton);
            this.ultraGroupBox1.Controls.Add(this.fileRadioButton);
            this.ultraGroupBox1.Controls.Add(this.consoleRadioButton);
            this.ultraGroupBox1.Location = new System.Drawing.Point(22, 20);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(224, 107);
            this.ultraGroupBox1.TabIndex = 0;
            this.ultraGroupBox1.Text = "Location";
            this.ultraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // noneRadioButton
            // 
            this.noneRadioButton.AutoSize = true;
            this.noneRadioButton.BackColor = System.Drawing.Color.Transparent;
            this.noneRadioButton.Location = new System.Drawing.Point(19, 28);
            this.noneRadioButton.Name = "noneRadioButton";
            this.noneRadioButton.Size = new System.Drawing.Size(51, 17);
            this.noneRadioButton.TabIndex = 8;
            this.noneRadioButton.TabStop = true;
            this.noneRadioButton.Text = "None";
            this.noneRadioButton.UseVisualStyleBackColor = false;
            // 
            // fileRadioButton
            // 
            this.fileRadioButton.AutoSize = true;
            this.fileRadioButton.BackColor = System.Drawing.Color.Transparent;
            this.fileRadioButton.Location = new System.Drawing.Point(19, 81);
            this.fileRadioButton.Name = "fileRadioButton";
            this.fileRadioButton.Size = new System.Drawing.Size(41, 17);
            this.fileRadioButton.TabIndex = 10;
            this.fileRadioButton.TabStop = true;
            this.fileRadioButton.Text = "File";
            this.fileRadioButton.UseVisualStyleBackColor = false;
            // 
            // consoleRadioButton
            // 
            this.consoleRadioButton.AutoSize = true;
            this.consoleRadioButton.BackColor = System.Drawing.Color.Transparent;
            this.consoleRadioButton.Location = new System.Drawing.Point(19, 53);
            this.consoleRadioButton.Name = "consoleRadioButton";
            this.consoleRadioButton.Size = new System.Drawing.Size(63, 17);
            this.consoleRadioButton.TabIndex = 9;
            this.consoleRadioButton.TabStop = true;
            this.consoleRadioButton.Text = "Console";
            this.consoleRadioButton.UseVisualStyleBackColor = false;
            // 
            // _Logger_UltraFormManager_Dock_Area_Left
            // 
            this._Logger_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Logger_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._Logger_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._Logger_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Logger_UltraFormManager_Dock_Area_Left.FormManager = this.firmwareLoggerUltraFormManager;
            this._Logger_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._Logger_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._Logger_UltraFormManager_Dock_Area_Left.Name = "_Logger_UltraFormManager_Dock_Area_Left";
            this._Logger_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 416);
            // 
            // _Logger_UltraFormManager_Dock_Area_Right
            // 
            this._Logger_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Logger_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._Logger_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._Logger_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Logger_UltraFormManager_Dock_Area_Right.FormManager = this.firmwareLoggerUltraFormManager;
            this._Logger_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._Logger_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(278, 32);
            this._Logger_UltraFormManager_Dock_Area_Right.Name = "_Logger_UltraFormManager_Dock_Area_Right";
            this._Logger_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 416);
            // 
            // _Logger_UltraFormManager_Dock_Area_Top
            // 
            this._Logger_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Logger_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._Logger_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._Logger_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Logger_UltraFormManager_Dock_Area_Top.FormManager = this.firmwareLoggerUltraFormManager;
            this._Logger_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._Logger_UltraFormManager_Dock_Area_Top.Name = "_Logger_UltraFormManager_Dock_Area_Top";
            this._Logger_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(286, 32);
            // 
            // _Logger_UltraFormManager_Dock_Area_Bottom
            // 
            this._Logger_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Logger_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._Logger_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._Logger_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Logger_UltraFormManager_Dock_Area_Bottom.FormManager = this.firmwareLoggerUltraFormManager;
            this._Logger_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._Logger_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 448);
            this._Logger_UltraFormManager_Dock_Area_Bottom.Name = "_Logger_UltraFormManager_Dock_Area_Bottom";
            this._Logger_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(286, 8);
            // 
            // Logger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(286, 456);
            this.Controls.Add(this.Logger_Fill_Panel);
            this.Controls.Add(this._Logger_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._Logger_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._Logger_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._Logger_UltraFormManager_Dock_Area_Bottom);
            this.Name = "Logger";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Firmware Logger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Logger_FormClosing);
            this.Load += new System.EventHandler(this.Logger_Load);
            ((System.ComponentModel.ISupportInitialize)(this.firmwareLoggerUltraFormManager)).EndInit();
            this.Logger_Fill_Panel.ClientArea.ResumeLayout(false);
            this.Logger_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.filterLevelGroupBox)).EndInit();
            this.filterLevelGroupBox.ResumeLayout(false);
            this.filterLevelGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinForm.UltraFormManager firmwareLoggerUltraFormManager;
        private Infragistics.Win.Misc.UltraPanel Logger_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _Logger_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _Logger_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _Logger_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _Logger_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private Infragistics.Win.Misc.UltraGroupBox filterLevelGroupBox;
        private System.Windows.Forms.CheckBox enableFWLogCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton debugRadioButton;
        private System.Windows.Forms.RadioButton fatalRadioButton;
        private System.Windows.Forms.RadioButton errorRadioButton;
        private System.Windows.Forms.RadioButton warningRadioButton;
        private System.Windows.Forms.RadioButton infoRadioButton;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private System.Windows.Forms.RadioButton noneRadioButton;
        private System.Windows.Forms.RadioButton fileRadioButton;
        private System.Windows.Forms.RadioButton consoleRadioButton;
    }
}