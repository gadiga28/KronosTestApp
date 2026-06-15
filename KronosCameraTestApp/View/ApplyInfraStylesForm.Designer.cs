namespace KronosCameraTestApp.View
{
    partial class ApplyInfraStylesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplyInfraStylesForm));
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.cboStyles = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraButton2 = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.cboStyles)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Location = new System.Drawing.Point(43, 41);
            this.ultraLabel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(40, 19);
            this.ultraLabel1.TabIndex = 0;
            this.ultraLabel1.Text = "Styles";
            // 
            // ultraButton1
            // 
            this.ultraButton1.Location = new System.Drawing.Point(78, 75);
            this.ultraButton1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(56, 25);
            this.ultraButton1.TabIndex = 2;
            this.ultraButton1.Text = "Apply";
            this.ultraButton1.Click += new System.EventHandler(this.ultraButton1_Click);
            // 
            // cboStyles
            // 
            this.cboStyles.Location = new System.Drawing.Point(78, 41);
            this.cboStyles.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cboStyles.Name = "cboStyles";
            this.cboStyles.Size = new System.Drawing.Size(197, 21);
            this.cboStyles.TabIndex = 3;
            // 
            // ultraButton2
            // 
            this.ultraButton2.Location = new System.Drawing.Point(219, 75);
            this.ultraButton2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ultraButton2.Name = "ultraButton2";
            this.ultraButton2.Size = new System.Drawing.Size(56, 25);
            this.ultraButton2.TabIndex = 4;
            this.ultraButton2.Text = "Close";
            this.ultraButton2.Click += new System.EventHandler(this.ultraButton2_Click);
            // 
            // ApplyInfraStylesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 159);
            this.Controls.Add(this.ultraButton2);
            this.Controls.Add(this.cboStyles);
            this.Controls.Add(this.ultraButton1);
            this.Controls.Add(this.ultraLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ApplyInfraStylesForm";
            this.Text = "Styles";
            this.Load += new System.EventHandler(this.ApplyInfraStylesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cboStyles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraButton ultraButton1;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboStyles;
        private Infragistics.Win.Misc.UltraButton ultraButton2;
    }
}