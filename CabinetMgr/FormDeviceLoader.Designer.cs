namespace CabinetMgr
{
    partial class FormDeviceLoader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDeviceLoader));
            this.cStatusGrid = new C1.Win.C1FlexGrid.C1FlexGrid();
            ((System.ComponentModel.ISupportInitialize)(this.cStatusGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // cStatusGrid
            // 
            this.cStatusGrid.AllowEditing = false;
            this.cStatusGrid.ColumnInfo = resources.GetString("cStatusGrid.ColumnInfo");
            this.cStatusGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.cStatusGrid.Location = new System.Drawing.Point(0, 0);
            this.cStatusGrid.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cStatusGrid.Name = "cStatusGrid";
            this.cStatusGrid.Rows.DefaultSize = 20;
            this.cStatusGrid.Size = new System.Drawing.Size(651, 500);
            this.cStatusGrid.StyleInfo = resources.GetString("cStatusGrid.StyleInfo");
            this.cStatusGrid.TabIndex = 0;
            this.cStatusGrid.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Blue;
            // 
            // FormDeviceLoader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 501);
            this.Controls.Add(this.cStatusGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "FormDeviceLoader";
            this.Text = "设备初始化";
            this.Load += new System.EventHandler(this.FormDeviceLoader_Load);
            this.Shown += new System.EventHandler(this.FormDeviceLoader_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.cStatusGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private C1.Win.C1FlexGrid.C1FlexGrid cStatusGrid;
    }
}