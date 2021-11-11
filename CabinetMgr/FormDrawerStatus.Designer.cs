namespace CabinetMgr
{
    partial class FormDrawerStatus
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDrawerStatus));
            this.cDrawerGrid = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.tmStatusUpdater = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.cDrawerGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // cDrawerGrid
            // 
            this.cDrawerGrid.AllowEditing = false;
            this.cDrawerGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.cDrawerGrid.ColumnInfo = "10,1,0,0,0,100,Columns:";
            this.cDrawerGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cDrawerGrid.Location = new System.Drawing.Point(0, 0);
            this.cDrawerGrid.Name = "cDrawerGrid";
            this.cDrawerGrid.Rows.DefaultSize = 20;
            this.cDrawerGrid.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Cell;
            this.cDrawerGrid.Size = new System.Drawing.Size(832, 417);
            this.cDrawerGrid.StyleInfo = resources.GetString("cDrawerGrid.StyleInfo");
            this.cDrawerGrid.TabIndex = 241;
            this.cDrawerGrid.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver;
            this.cDrawerGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cDrawerGrid_MouseClick);
            // 
            // tmStatusUpdater
            // 
            this.tmStatusUpdater.Enabled = true;
            this.tmStatusUpdater.Interval = 3000;
            this.tmStatusUpdater.Tick += new System.EventHandler(this.tmStatusUpdater_Tick);
            // 
            // FormDrawerStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 417);
            this.Controls.Add(this.cDrawerGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDrawerStatus";
            this.Text = "抽屉状态";
            this.Load += new System.EventHandler(this.FormDrawerStatus_Load);
            this.Shown += new System.EventHandler(this.FormDrawerStatus_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.cDrawerGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private C1.Win.C1FlexGrid.C1FlexGrid cDrawerGrid;
        private System.Windows.Forms.Timer tmStatusUpdater;
    }
}