namespace CabinetMgr
{
    partial class FormBorrowRecord
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBorrowRecord));
            this.panel1 = new System.Windows.Forms.Panel();
            this.cHistoryGrid = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cHistoryGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cHistoryGrid);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(815, 517);
            this.panel1.TabIndex = 254;
            // 
            // cHistoryGrid
            // 
            this.cHistoryGrid.AllowEditing = false;
            this.cHistoryGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.cHistoryGrid.ColumnInfo = resources.GetString("cHistoryGrid.ColumnInfo");
            this.cHistoryGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cHistoryGrid.Location = new System.Drawing.Point(0, 0);
            this.cHistoryGrid.Margin = new System.Windows.Forms.Padding(4);
            this.cHistoryGrid.Name = "cHistoryGrid";
            this.cHistoryGrid.Rows.DefaultSize = 20;
            this.cHistoryGrid.Size = new System.Drawing.Size(815, 517);
            this.cHistoryGrid.StyleInfo = resources.GetString("cHistoryGrid.StyleInfo");
            this.cHistoryGrid.TabIndex = 1;
            this.cHistoryGrid.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver;
            this.cHistoryGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cHistoryGrid_MouseClick);
            // 
            // FormBorrowRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 517);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormBorrowRecord";
            this.Text = "FormReturnRecord";
            this.Load += new System.EventHandler(this.FormReturnRecord_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cHistoryGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private C1.Win.C1FlexGrid.C1FlexGrid cHistoryGrid;
    }
}