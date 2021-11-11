namespace CabinetMgr
{
    partial class FormToolPlan
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
            this.btnMissionInfo = new System.Windows.Forms.Button();
            this.panelWindow = new System.Windows.Forms.Panel();
            this.panelTab = new System.Windows.Forms.Panel();
            this.panelTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnMissionInfo
            // 
            this.btnMissionInfo.BackgroundImage = global::CabinetMgr.Properties.Resources.tab_task_n;
            this.btnMissionInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMissionInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMissionInfo.FlatAppearance.BorderSize = 0;
            this.btnMissionInfo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnMissionInfo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnMissionInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMissionInfo.Location = new System.Drawing.Point(0, 15);
            this.btnMissionInfo.Margin = new System.Windows.Forms.Padding(4);
            this.btnMissionInfo.Name = "btnMissionInfo";
            this.btnMissionInfo.Size = new System.Drawing.Size(185, 36);
            this.btnMissionInfo.TabIndex = 251;
            this.btnMissionInfo.Tag = "0";
            this.btnMissionInfo.UseVisualStyleBackColor = true;
            this.btnMissionInfo.Click += new System.EventHandler(this.btnMissionInfo_Click);
            // 
            // panelWindow
            // 
            this.panelWindow.Location = new System.Drawing.Point(5, 51);
            this.panelWindow.Margin = new System.Windows.Forms.Padding(4);
            this.panelWindow.Name = "panelWindow";
            this.panelWindow.Size = new System.Drawing.Size(868, 649);
            this.panelWindow.TabIndex = 253;
            this.panelWindow.SizeChanged += new System.EventHandler(this.panelWindow_SizeChanged);
            // 
            // panelTab
            // 
            this.panelTab.BackColor = System.Drawing.Color.Transparent;
            this.panelTab.Controls.Add(this.btnMissionInfo);
            this.panelTab.Location = new System.Drawing.Point(9, 0);
            this.panelTab.Margin = new System.Windows.Forms.Padding(4);
            this.panelTab.Name = "panelTab";
            this.panelTab.Size = new System.Drawing.Size(557, 51);
            this.panelTab.TabIndex = 254;
            // 
            // FormToolPlan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::CabinetMgr.Properties.Resources.right_2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(908, 759);
            this.Controls.Add(this.panelTab);
            this.Controls.Add(this.panelWindow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormToolPlan";
            this.Text = "FormToolPlan";
            this.Load += new System.EventHandler(this.FormToolPlan_Load);
            this.Shown += new System.EventHandler(this.FormToolPlan_Shown);
            this.panelTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnMissionInfo;
        private System.Windows.Forms.Panel panelWindow;
        private System.Windows.Forms.Panel panelTab;
    }
}