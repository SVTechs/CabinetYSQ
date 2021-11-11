namespace ZkCollector
{
    partial class FormUserList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUserList));
            this.cResultGrid = new C1.Win.C1FlexGrid.C1FlexGrid();
            ((System.ComponentModel.ISupportInitialize)(this.cResultGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // cResultGrid
            // 
            this.cResultGrid.AllowEditing = false;
            this.cResultGrid.ColumnInfo = "10,1,0,0,0,100,Columns:";
            this.cResultGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cResultGrid.Location = new System.Drawing.Point(0, 0);
            this.cResultGrid.Name = "cResultGrid";
            this.cResultGrid.Rows.DefaultSize = 20;
            this.cResultGrid.Size = new System.Drawing.Size(862, 447);
            this.cResultGrid.StyleInfo = resources.GetString("cResultGrid.StyleInfo");
            this.cResultGrid.TabIndex = 0;
            this.cResultGrid.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Blue;
            this.cResultGrid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.cResultGrid_MouseDoubleClick);
            // 
            // FormUserList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 447);
            this.Controls.Add(this.cResultGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FormUserList";
            this.Text = "用户列表";
            this.Load += new System.EventHandler(this.FormUserList_Load);
            this.Shown += new System.EventHandler(this.FormUserList_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormUserList_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.cResultGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private C1.Win.C1FlexGrid.C1FlexGrid cResultGrid;
    }
}