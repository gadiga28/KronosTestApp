namespace KronosCameraTestApp.View
{
    partial class UserLoginForm
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserLoginForm));
            this.ultraTextEditorUserName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraTextEditorPassword = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.UserLoginUltraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._LoginForm_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._LoginForm_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._LoginForm_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._LoginForm_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.userLoginUltraToolTipManager = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraPictureBox1 = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorUserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserLoginUltraFormManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTextEditorUserName
            // 
            this.ultraTextEditorUserName.AutoSize = false;
            this.ultraTextEditorUserName.Location = new System.Drawing.Point(148, 27);
            this.ultraTextEditorUserName.Margin = new System.Windows.Forms.Padding(2);
            this.ultraTextEditorUserName.Name = "ultraTextEditorUserName";
            this.ultraTextEditorUserName.Size = new System.Drawing.Size(137, 23);
            this.ultraTextEditorUserName.TabIndex = 1;
            // 
            // ultraTextEditorPassword
            // 
            this.ultraTextEditorPassword.AutoSize = false;
            this.ultraTextEditorPassword.Location = new System.Drawing.Point(148, 59);
            this.ultraTextEditorPassword.Margin = new System.Windows.Forms.Padding(2);
            this.ultraTextEditorPassword.Name = "ultraTextEditorPassword";
            this.ultraTextEditorPassword.PasswordChar = '*';
            this.ultraTextEditorPassword.Size = new System.Drawing.Size(137, 23);
            this.ultraTextEditorPassword.TabIndex = 2;
            // 
            // buttonLogin
            // 
            this.buttonLogin.BackColor = System.Drawing.Color.Transparent;
            this.buttonLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLogin.Location = new System.Drawing.Point(150, 102);
            this.buttonLogin.Margin = new System.Windows.Forms.Padding(2);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(56, 25);
            this.buttonLogin.TabIndex = 3;
            this.buttonLogin.Text = "Login";
            this.buttonLogin.UseVisualStyleBackColor = false;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Location = new System.Drawing.Point(229, 102);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(56, 25);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // UserLoginUltraFormManager
            // 
            this.UserLoginUltraFormManager.Form = this;
            this.UserLoginUltraFormManager.FormStyleSettings.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.RoundedSizable;
            this.UserLoginUltraFormManager.FormStyleSettings.IsGlassSupported = true;
            this.UserLoginUltraFormManager.FormStyleSettings.Style = Infragistics.Win.UltraWinForm.UltraFormStyle.Office2010;
            // 
            // _LoginForm_UltraFormManager_Dock_Area_Left
            // 
            this._LoginForm_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._LoginForm_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._LoginForm_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._LoginForm_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._LoginForm_UltraFormManager_Dock_Area_Left.FormManager = this.UserLoginUltraFormManager;
            this._LoginForm_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._LoginForm_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._LoginForm_UltraFormManager_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(2);
            this._LoginForm_UltraFormManager_Dock_Area_Left.Name = "_LoginForm_UltraFormManager_Dock_Area_Left";
            this._LoginForm_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 142);
            // 
            // _LoginForm_UltraFormManager_Dock_Area_Right
            // 
            this._LoginForm_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._LoginForm_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._LoginForm_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._LoginForm_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._LoginForm_UltraFormManager_Dock_Area_Right.FormManager = this.UserLoginUltraFormManager;
            this._LoginForm_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._LoginForm_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(307, 31);
            this._LoginForm_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(2);
            this._LoginForm_UltraFormManager_Dock_Area_Right.Name = "_LoginForm_UltraFormManager_Dock_Area_Right";
            this._LoginForm_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 142);
            // 
            // _LoginForm_UltraFormManager_Dock_Area_Top
            // 
            this._LoginForm_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._LoginForm_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._LoginForm_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._LoginForm_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._LoginForm_UltraFormManager_Dock_Area_Top.FormManager = this.UserLoginUltraFormManager;
            this._LoginForm_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._LoginForm_UltraFormManager_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(2);
            this._LoginForm_UltraFormManager_Dock_Area_Top.Name = "_LoginForm_UltraFormManager_Dock_Area_Top";
            this._LoginForm_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(315, 31);
            // 
            // _LoginForm_UltraFormManager_Dock_Area_Bottom
            // 
            this._LoginForm_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._LoginForm_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._LoginForm_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._LoginForm_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._LoginForm_UltraFormManager_Dock_Area_Bottom.FormManager = this.UserLoginUltraFormManager;
            this._LoginForm_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._LoginForm_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 173);
            this._LoginForm_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(2);
            this._LoginForm_UltraFormManager_Dock_Area_Bottom.Name = "_LoginForm_UltraFormManager_Dock_Area_Bottom";
            this._LoginForm_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(315, 8);
            // 
            // userLoginUltraToolTipManager
            // 
            this.userLoginUltraToolTipManager.ContainingControl = this;
            this.userLoginUltraToolTipManager.ToolTipTextStyle = Infragistics.Win.ToolTipTextStyle.Formatted;
            // 
            // ultraLabel1
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.TextHAlignAsString = "Left";
            appearance1.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance1;
            this.ultraLabel1.Location = new System.Drawing.Point(72, 27);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(71, 23);
            this.ultraLabel1.TabIndex = 15;
            this.ultraLabel1.Text = "User Name:";
            // 
            // ultraLabel2
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.TextHAlignAsString = "Left";
            appearance2.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance2;
            this.ultraLabel2.Location = new System.Drawing.Point(72, 59);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(71, 23);
            this.ultraLabel2.TabIndex = 16;
            this.ultraLabel2.Text = "Password:";
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.ultraPictureBox1);
            this.ultraGroupBox1.Controls.Add(this.ultraTextEditorUserName);
            this.ultraGroupBox1.Controls.Add(this.ultraTextEditorPassword);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel1);
            this.ultraGroupBox1.Controls.Add(this.buttonLogin);
            this.ultraGroupBox1.Controls.Add(this.buttonCancel);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel2);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox1.Location = new System.Drawing.Point(8, 31);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(299, 142);
            this.ultraGroupBox1.TabIndex = 29;
            this.ultraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // ultraPictureBox1
            // 
            this.ultraPictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.ultraPictureBox1.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraPictureBox1.BorderShadowColor = System.Drawing.Color.Empty;
            this.ultraPictureBox1.Image = ((object)(resources.GetObject("ultraPictureBox1.Image")));
            this.ultraPictureBox1.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.ultraPictureBox1.Location = new System.Drawing.Point(6, 32);
            this.ultraPictureBox1.Name = "ultraPictureBox1";
            this.ultraPictureBox1.Size = new System.Drawing.Size(60, 50);
            this.ultraPictureBox1.TabIndex = 17;
            // 
            // UserLoginForm
            // 
            this.AcceptButton = this.buttonLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(315, 181);
            this.Controls.Add(this.ultraGroupBox1);
            this.Controls.Add(this._LoginForm_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._LoginForm_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._LoginForm_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._LoginForm_UltraFormManager_Dock_Area_Bottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UserLoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User Login";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserLoginForm_FormClosing);
            this.Load += new System.EventHandler(this.UserLoginForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorUserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserLoginUltraFormManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraTextEditor ultraTextEditorUserName;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor ultraTextEditorPassword;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.Button buttonCancel;
        private Infragistics.Win.UltraWinForm.UltraFormManager UserLoginUltraFormManager;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _LoginForm_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _LoginForm_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _LoginForm_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _LoginForm_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolTip.UltraToolTipManager userLoginUltraToolTipManager;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.UltraWinEditors.UltraPictureBox ultraPictureBox1;
    }
}