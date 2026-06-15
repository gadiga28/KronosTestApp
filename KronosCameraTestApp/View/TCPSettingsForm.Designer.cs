namespace KronosCameraTestApp.View
{
    partial class TCPSettingsForm
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TCPSettingsForm));
            this.tcpSettingsUltraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.ultraTextEditorPort = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraTextEditorIPAddress = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.tcpSettingsUltraFormManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorIPAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcpSettingsUltraFormManager
            // 
            this.tcpSettingsUltraFormManager.Form = this;
            this.tcpSettingsUltraFormManager.FormStyleSettings.Style = Infragistics.Win.UltraWinForm.UltraFormStyle.Office2010;
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Location = new System.Drawing.Point(176, 117);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(71, 26);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.BackColor = System.Drawing.Color.Transparent;
            this.buttonConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonConnect.Location = new System.Drawing.Point(91, 117);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(2);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(71, 26);
            this.buttonConnect.TabIndex = 4;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = false;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // ultraTextEditorPort
            // 
            this.ultraTextEditorPort.Location = new System.Drawing.Point(119, 63);
            this.ultraTextEditorPort.Margin = new System.Windows.Forms.Padding(2);
            this.ultraTextEditorPort.Name = "ultraTextEditorPort";
            this.ultraTextEditorPort.Size = new System.Drawing.Size(128, 21);
            this.ultraTextEditorPort.TabIndex = 3;
            this.ultraTextEditorPort.Text = "47151";
            this.ultraTextEditorPort.ValueChanged += new System.EventHandler(this.ultraTextEditor2_ValueChanged);
            // 
            // ultraTextEditorIPAddress
            // 
            this.ultraTextEditorIPAddress.Location = new System.Drawing.Point(119, 19);
            this.ultraTextEditorIPAddress.Margin = new System.Windows.Forms.Padding(2);
            this.ultraTextEditorIPAddress.Name = "ultraTextEditorIPAddress";
            this.ultraTextEditorIPAddress.Size = new System.Drawing.Size(128, 21);
            this.ultraTextEditorIPAddress.TabIndex = 2;
            this.ultraTextEditorIPAddress.Text = "192.168.1.2";
            // 
            // ultraLabel2
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel2.Appearance = appearance2;
            this.ultraLabel2.Location = new System.Drawing.Point(5, 66);
            this.ultraLabel2.Margin = new System.Windows.Forms.Padding(2);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(75, 19);
            this.ultraLabel2.TabIndex = 1;
            this.ultraLabel2.Text = "Port:";
            // 
            // ultraLabel1
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel1.Appearance = appearance1;
            this.ultraLabel1.Location = new System.Drawing.Point(5, 22);
            this.ultraLabel1.Margin = new System.Windows.Forms.Padding(2);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(110, 19);
            this.ultraLabel1.TabIndex = 0;
            this.ultraLabel1.Text = "Camera IP Address:";
            // 
            // _TCPSettingsForm_UltraFormManager_Dock_Area_Left
            // 
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Left.FormManager = this.tcpSettingsUltraFormManager;
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 30);
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(2);
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Left.Name = "_TCPSettingsForm_UltraFormManager_Dock_Area_Left";
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 163);
            // 
            // _TCPSettingsForm_UltraFormManager_Dock_Area_Right
            // 
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Right.FormManager = this.tcpSettingsUltraFormManager;
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(276, 30);
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(2);
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Right.Name = "_TCPSettingsForm_UltraFormManager_Dock_Area_Right";
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 163);
            // 
            // _TCPSettingsForm_UltraFormManager_Dock_Area_Top
            // 
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Top.FormManager = this.tcpSettingsUltraFormManager;
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(2);
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Top.Name = "_TCPSettingsForm_UltraFormManager_Dock_Area_Top";
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(284, 30);
            // 
            // _TCPSettingsForm_UltraFormManager_Dock_Area_Bottom
            // 
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Bottom.FormManager = this.tcpSettingsUltraFormManager;
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 193);
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(2);
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Bottom.Name = "_TCPSettingsForm_UltraFormManager_Dock_Area_Bottom";
            this._TCPSettingsForm_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(284, 8);
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.buttonCancel);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel1);
            this.ultraGroupBox1.Controls.Add(this.buttonConnect);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel2);
            this.ultraGroupBox1.Controls.Add(this.ultraTextEditorPort);
            this.ultraGroupBox1.Controls.Add(this.ultraTextEditorIPAddress);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox1.Location = new System.Drawing.Point(8, 30);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(268, 163);
            this.ultraGroupBox1.TabIndex = 5;
            this.ultraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // TCPSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(284, 201);
            this.Controls.Add(this.ultraGroupBox1);
            this.Controls.Add(this._TCPSettingsForm_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._TCPSettingsForm_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._TCPSettingsForm_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._TCPSettingsForm_UltraFormManager_Dock_Area_Bottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TCPSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TCP Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TCPSettingsForm_FormClosing);
            this.Load += new System.EventHandler(this.TCPSettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tcpSettingsUltraFormManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorIPAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinForm.UltraFormManager tcpSettingsUltraFormManager;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TCPSettingsForm_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TCPSettingsForm_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TCPSettingsForm_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TCPSettingsForm_UltraFormManager_Dock_Area_Bottom;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor ultraTextEditorPort;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor ultraTextEditorIPAddress;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonConnect;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
    }
}