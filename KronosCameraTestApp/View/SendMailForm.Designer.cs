namespace KronosCameraTestApp.View
{
    partial class SendMailForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendMailForm));
            this.sendMailUltraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxEmailPassword = new System.Windows.Forms.TextBox();
            this.richTextBoxAttachments = new System.Windows.Forms.RichTextBox();
            this.ultraPictureBoxOpenFileDialog = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.lblAttachments = new System.Windows.Forms.Label();
            this.richTextBoxMailBody = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxCC = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxEmailSubject = new System.Windows.Forms.TextBox();
            this.textBoxEmailToAddress = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.btnSendMail = new System.Windows.Forms.Button();
            this._SendMailForm_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._SendMailForm_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._SendMailForm_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._SendMailForm_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.sendMailOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.sendMailUltraFormManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sendMailUltraFormManager
            // 
            this.sendMailUltraFormManager.Form = this;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(6, 636);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Email Password:";
            // 
            // textBoxEmailPassword
            // 
            this.textBoxEmailPassword.Location = new System.Drawing.Point(90, 632);
            this.textBoxEmailPassword.Name = "textBoxEmailPassword";
            this.textBoxEmailPassword.PasswordChar = '*';
            this.textBoxEmailPassword.Size = new System.Drawing.Size(204, 20);
            this.textBoxEmailPassword.TabIndex = 13;
            // 
            // richTextBoxAttachments
            // 
            this.richTextBoxAttachments.Location = new System.Drawing.Point(94, 123);
            this.richTextBoxAttachments.Name = "richTextBoxAttachments";
            this.richTextBoxAttachments.Size = new System.Drawing.Size(632, 50);
            this.richTextBoxAttachments.TabIndex = 12;
            this.richTextBoxAttachments.Text = "";
            // 
            // ultraPictureBoxOpenFileDialog
            // 
            this.ultraPictureBoxOpenFileDialog.BackColor = System.Drawing.Color.Transparent;
            this.ultraPictureBoxOpenFileDialog.BorderShadowColor = System.Drawing.Color.Empty;
            this.ultraPictureBoxOpenFileDialog.Image = ((object)(resources.GetObject("ultraPictureBoxOpenFileDialog.Image")));
            this.ultraPictureBoxOpenFileDialog.Location = new System.Drawing.Point(732, 123);
            this.ultraPictureBoxOpenFileDialog.Name = "ultraPictureBoxOpenFileDialog";
            this.ultraPictureBoxOpenFileDialog.Size = new System.Drawing.Size(45, 51);
            this.ultraPictureBoxOpenFileDialog.TabIndex = 11;
            this.ultraPictureBoxOpenFileDialog.Click += new System.EventHandler(this.ultraPictureBoxOpenFileDialog_Click);
            // 
            // lblAttachments
            // 
            this.lblAttachments.AutoSize = true;
            this.lblAttachments.BackColor = System.Drawing.Color.Transparent;
            this.lblAttachments.Location = new System.Drawing.Point(6, 144);
            this.lblAttachments.Name = "lblAttachments";
            this.lblAttachments.Size = new System.Drawing.Size(66, 13);
            this.lblAttachments.TabIndex = 10;
            this.lblAttachments.Text = "Attachments";
            // 
            // richTextBoxMailBody
            // 
            this.richTextBoxMailBody.Location = new System.Drawing.Point(94, 185);
            this.richTextBoxMailBody.Name = "richTextBoxMailBody";
            this.richTextBoxMailBody.Size = new System.Drawing.Size(683, 408);
            this.richTextBoxMailBody.TabIndex = 8;
            this.richTextBoxMailBody.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(6, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Cc...";
            // 
            // textBoxCC
            // 
            this.textBoxCC.Location = new System.Drawing.Point(94, 51);
            this.textBoxCC.Name = "textBoxCC";
            this.textBoxCC.Size = new System.Drawing.Size(683, 20);
            this.textBoxCC.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(6, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Subject";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "To...";
            // 
            // textBoxEmailSubject
            // 
            this.textBoxEmailSubject.Location = new System.Drawing.Point(94, 90);
            this.textBoxEmailSubject.Name = "textBoxEmailSubject";
            this.textBoxEmailSubject.Size = new System.Drawing.Size(683, 20);
            this.textBoxEmailSubject.TabIndex = 3;
            // 
            // textBoxEmailToAddress
            // 
            this.textBoxEmailToAddress.Location = new System.Drawing.Point(94, 14);
            this.textBoxEmailToAddress.Name = "textBoxEmailToAddress";
            this.textBoxEmailToAddress.Size = new System.Drawing.Size(683, 20);
            this.textBoxEmailToAddress.TabIndex = 2;
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Location = new System.Drawing.Point(702, 624);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // btnSendMail
            // 
            this.btnSendMail.BackColor = System.Drawing.Color.Transparent;
            this.btnSendMail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendMail.Location = new System.Drawing.Point(595, 624);
            this.btnSendMail.Name = "btnSendMail";
            this.btnSendMail.Size = new System.Drawing.Size(75, 23);
            this.btnSendMail.TabIndex = 0;
            this.btnSendMail.Text = "Send";
            this.btnSendMail.UseVisualStyleBackColor = false;
            this.btnSendMail.Click += new System.EventHandler(this.SendMail_Click);
            // 
            // _SendMailForm_UltraFormManager_Dock_Area_Left
            // 
            this._SendMailForm_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SendMailForm_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._SendMailForm_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._SendMailForm_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SendMailForm_UltraFormManager_Dock_Area_Left.FormManager = this.sendMailUltraFormManager;
            this._SendMailForm_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._SendMailForm_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._SendMailForm_UltraFormManager_Dock_Area_Left.Name = "_SendMailForm_UltraFormManager_Dock_Area_Left";
            this._SendMailForm_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 661);
            // 
            // _SendMailForm_UltraFormManager_Dock_Area_Right
            // 
            this._SendMailForm_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SendMailForm_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._SendMailForm_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._SendMailForm_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SendMailForm_UltraFormManager_Dock_Area_Right.FormManager = this.sendMailUltraFormManager;
            this._SendMailForm_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._SendMailForm_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(809, 31);
            this._SendMailForm_UltraFormManager_Dock_Area_Right.Name = "_SendMailForm_UltraFormManager_Dock_Area_Right";
            this._SendMailForm_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 661);
            // 
            // _SendMailForm_UltraFormManager_Dock_Area_Top
            // 
            this._SendMailForm_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SendMailForm_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._SendMailForm_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._SendMailForm_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SendMailForm_UltraFormManager_Dock_Area_Top.FormManager = this.sendMailUltraFormManager;
            this._SendMailForm_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._SendMailForm_UltraFormManager_Dock_Area_Top.Name = "_SendMailForm_UltraFormManager_Dock_Area_Top";
            this._SendMailForm_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(817, 31);
            // 
            // _SendMailForm_UltraFormManager_Dock_Area_Bottom
            // 
            this._SendMailForm_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SendMailForm_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._SendMailForm_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._SendMailForm_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SendMailForm_UltraFormManager_Dock_Area_Bottom.FormManager = this.sendMailUltraFormManager;
            this._SendMailForm_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._SendMailForm_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 692);
            this._SendMailForm_UltraFormManager_Dock_Area_Bottom.Name = "_SendMailForm_UltraFormManager_Dock_Area_Bottom";
            this._SendMailForm_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(817, 8);
            // 
            // sendMailOpenFileDialog
            // 
            this.sendMailOpenFileDialog.FileName = "openFileDialog1";
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.buttonCancel);
            this.ultraGroupBox1.Controls.Add(this.label4);
            this.ultraGroupBox1.Controls.Add(this.btnSendMail);
            this.ultraGroupBox1.Controls.Add(this.textBoxEmailPassword);
            this.ultraGroupBox1.Controls.Add(this.label3);
            this.ultraGroupBox1.Controls.Add(this.label1);
            this.ultraGroupBox1.Controls.Add(this.richTextBoxMailBody);
            this.ultraGroupBox1.Controls.Add(this.ultraPictureBoxOpenFileDialog);
            this.ultraGroupBox1.Controls.Add(this.richTextBoxAttachments);
            this.ultraGroupBox1.Controls.Add(this.label2);
            this.ultraGroupBox1.Controls.Add(this.lblAttachments);
            this.ultraGroupBox1.Controls.Add(this.textBoxEmailToAddress);
            this.ultraGroupBox1.Controls.Add(this.textBoxCC);
            this.ultraGroupBox1.Controls.Add(this.textBoxEmailSubject);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox1.Location = new System.Drawing.Point(8, 31);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(801, 661);
            this.ultraGroupBox1.TabIndex = 5;
            this.ultraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // SendMailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(817, 700);
            this.Controls.Add(this.ultraGroupBox1);
            this.Controls.Add(this._SendMailForm_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._SendMailForm_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._SendMailForm_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._SendMailForm_UltraFormManager_Dock_Area_Bottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SendMailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Send eMail";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SendMailForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.sendMailUltraFormManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinForm.UltraFormManager sendMailUltraFormManager;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _SendMailForm_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _SendMailForm_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _SendMailForm_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _SendMailForm_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.Button btnSendMail;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxEmailSubject;
        private System.Windows.Forms.TextBox textBoxEmailToAddress;
        private System.Windows.Forms.RichTextBox richTextBoxMailBody;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxCC;
        private System.Windows.Forms.Label lblAttachments;
        private System.Windows.Forms.OpenFileDialog sendMailOpenFileDialog;
        private Infragistics.Win.UltraWinEditors.UltraPictureBox ultraPictureBoxOpenFileDialog;
        private System.Windows.Forms.RichTextBox richTextBoxAttachments;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxEmailPassword;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
    }
}