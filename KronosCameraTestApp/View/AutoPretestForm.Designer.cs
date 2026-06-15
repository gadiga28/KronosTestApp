namespace KronosCameraTestApp.View
{
    partial class AutoPretestForm
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
            Infragistics.Win.UltraWinListView.UltraListViewItem ultraListViewItem1 = new Infragistics.Win.UltraWinListView.UltraListViewItem("Red,  Blue & UV", null, null);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinListView.UltraListViewItem ultraListViewItem2 = new Infragistics.Win.UltraWinListView.UltraListViewItem("Defects", null, null);
            Infragistics.Win.UltraWinListView.UltraListViewItem ultraListViewItem3 = new Infragistics.Win.UltraWinListView.UltraListViewItem("Mean Variance", null, null);
            Infragistics.Win.UltraWinListView.UltraListViewItem ultraListViewItem4 = new Infragistics.Win.UltraWinListView.UltraListViewItem("Read Noise Vs NDROs", null, null);
            Infragistics.Win.UltraWinListView.UltraListViewItem ultraListViewItem5 = new Infragistics.Win.UltraWinListView.UltraListViewItem("Dark Current", null, null);
            Infragistics.Win.UltraWinListView.UltraListViewItem ultraListViewItem6 = new Infragistics.Win.UltraWinListView.UltraListViewItem("Photoresponse", null, null);
            Infragistics.Win.UltraWinListView.UltraListViewItem ultraListViewItem7 = new Infragistics.Win.UltraWinListView.UltraListViewItem("Injection Efficiency", null, null);
            Infragistics.Win.UltraWinListView.UltraListViewItem ultraListViewItem8 = new Infragistics.Win.UltraWinListView.UltraListViewItem("Shutter Drive", null, null);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoPretestForm));
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.ultraListView1 = new Infragistics.Win.UltraWinListView.UltraListView();
            this.buttonCancelAutoPretest = new System.Windows.Forms.Button();
            this.buttonRunAutoPretest = new System.Windows.Forms.Button();
            this._AutoPretestForm_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AutoPretestForm_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AutoPretestForm_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AutoPretestForm_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraListView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // ultraListView1
            // 
            appearance1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ultraListView1.Appearance = appearance1;
            ultraListViewItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            ultraListViewItem1.Key = "RBT";
            appearance2.Image = global::KronosCameraTestApp.Properties.Resources.line_graph_old_b_86262;
            ultraListViewItem1.SelectedAppearance = appearance2;
            ultraListViewItem2.Key = "DET";
            ultraListViewItem3.Key = "MVT";
            ultraListViewItem4.Key = "RNT";
            ultraListViewItem5.Key = "DDT";
            ultraListViewItem6.Key = "PRT";
            ultraListViewItem7.Key = "IET";
            ultraListViewItem8.Key = "SDT";
            this.ultraListView1.Items.AddRange(new Infragistics.Win.UltraWinListView.UltraListViewItem[] {
            ultraListViewItem1,
            ultraListViewItem2,
            ultraListViewItem3,
            ultraListViewItem4,
            ultraListViewItem5,
            ultraListViewItem6,
            ultraListViewItem7,
            ultraListViewItem8});
            this.ultraListView1.Location = new System.Drawing.Point(8, 11);
            this.ultraListView1.Name = "ultraListView1";
            this.ultraListView1.Size = new System.Drawing.Size(147, 155);
            this.ultraListView1.TabIndex = 4;
            this.ultraListView1.Text = "autoPretestUltraListView";
            this.ultraListView1.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.List;
            this.ultraListView1.ViewSettingsDetails.CheckBoxStyle = Infragistics.Win.UltraWinListView.CheckBoxStyle.TriState;
            this.ultraListView1.ViewSettingsList.CheckBoxStyle = Infragistics.Win.UltraWinListView.CheckBoxStyle.CheckBox;
            this.ultraListView1.ViewSettingsList.ImageSize = new System.Drawing.Size(0, 0);
            // 
            // buttonCancelAutoPretest
            // 
            this.buttonCancelAutoPretest.BackColor = System.Drawing.Color.Transparent;
            this.buttonCancelAutoPretest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancelAutoPretest.Location = new System.Drawing.Point(87, 186);
            this.buttonCancelAutoPretest.Name = "buttonCancelAutoPretest";
            this.buttonCancelAutoPretest.Size = new System.Drawing.Size(68, 23);
            this.buttonCancelAutoPretest.TabIndex = 3;
            this.buttonCancelAutoPretest.Text = "Cancel";
            this.buttonCancelAutoPretest.UseVisualStyleBackColor = false;
            this.buttonCancelAutoPretest.Click += new System.EventHandler(this.buttonCancelAutoPretest_Click);
            // 
            // buttonRunAutoPretest
            // 
            this.buttonRunAutoPretest.BackColor = System.Drawing.Color.Transparent;
            this.buttonRunAutoPretest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRunAutoPretest.Location = new System.Drawing.Point(8, 186);
            this.buttonRunAutoPretest.Name = "buttonRunAutoPretest";
            this.buttonRunAutoPretest.Size = new System.Drawing.Size(68, 23);
            this.buttonRunAutoPretest.TabIndex = 2;
            this.buttonRunAutoPretest.Text = "Run";
            this.buttonRunAutoPretest.UseVisualStyleBackColor = false;
            this.buttonRunAutoPretest.Click += new System.EventHandler(this.buttonRunAutoPretest_Click);
            // 
            // _AutoPretestForm_UltraFormManager_Dock_Area_Left
            // 
            this._AutoPretestForm_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AutoPretestForm_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AutoPretestForm_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._AutoPretestForm_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AutoPretestForm_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._AutoPretestForm_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._AutoPretestForm_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._AutoPretestForm_UltraFormManager_Dock_Area_Left.Name = "_AutoPretestForm_UltraFormManager_Dock_Area_Left";
            this._AutoPretestForm_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 227);
            // 
            // _AutoPretestForm_UltraFormManager_Dock_Area_Right
            // 
            this._AutoPretestForm_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AutoPretestForm_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AutoPretestForm_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._AutoPretestForm_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AutoPretestForm_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._AutoPretestForm_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._AutoPretestForm_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(172, 32);
            this._AutoPretestForm_UltraFormManager_Dock_Area_Right.Name = "_AutoPretestForm_UltraFormManager_Dock_Area_Right";
            this._AutoPretestForm_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 227);
            // 
            // _AutoPretestForm_UltraFormManager_Dock_Area_Top
            // 
            this._AutoPretestForm_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AutoPretestForm_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AutoPretestForm_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._AutoPretestForm_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AutoPretestForm_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._AutoPretestForm_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._AutoPretestForm_UltraFormManager_Dock_Area_Top.Name = "_AutoPretestForm_UltraFormManager_Dock_Area_Top";
            this._AutoPretestForm_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(180, 32);
            // 
            // _AutoPretestForm_UltraFormManager_Dock_Area_Bottom
            // 
            this._AutoPretestForm_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AutoPretestForm_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AutoPretestForm_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._AutoPretestForm_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AutoPretestForm_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._AutoPretestForm_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._AutoPretestForm_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 259);
            this._AutoPretestForm_UltraFormManager_Dock_Area_Bottom.Name = "_AutoPretestForm_UltraFormManager_Dock_Area_Bottom";
            this._AutoPretestForm_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(180, 8);
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.buttonCancelAutoPretest);
            this.ultraGroupBox1.Controls.Add(this.ultraListView1);
            this.ultraGroupBox1.Controls.Add(this.buttonRunAutoPretest);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox1.Location = new System.Drawing.Point(8, 32);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(164, 227);
            this.ultraGroupBox1.TabIndex = 5;
            this.ultraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // AutoPretestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(180, 267);
            this.Controls.Add(this.ultraGroupBox1);
            this.Controls.Add(this._AutoPretestForm_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._AutoPretestForm_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._AutoPretestForm_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._AutoPretestForm_UltraFormManager_Dock_Area_Bottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AutoPretestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Auto Pretest ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AutoPretestForm_FormClosing);
            this.Load += new System.EventHandler(this.AutoPretestForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraListView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AutoPretestForm_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AutoPretestForm_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AutoPretestForm_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AutoPretestForm_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.Button buttonCancelAutoPretest;

        private System.Windows.Forms.Button buttonRunAutoPretest;
        private Infragistics.Win.UltraWinListView.UltraListView ultraListView1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
    }
}