namespace CabinetMgr
{
    partial class FormReturnRecord
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReturnRecord));
            this.panel1 = new System.Windows.Forms.Panel();
            this.cHistoryGrid = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cHistoryGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cHistoryGrid);
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(820, 510);
            this.panel1.TabIndex = 254;
            // 
            // cHistoryGrid
            // 
            this.cHistoryGrid.AllowEditing = false;
            this.cHistoryGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.cHistoryGrid.ColumnInfo = "10,1,0,0,0,100,Columns:";
            this.cHistoryGrid.Location = new System.Drawing.Point(7, 6);
            this.cHistoryGrid.Name = "cHistoryGrid";
            this.cHistoryGrid.Rows.DefaultSize = 20;
            this.cHistoryGrid.Size = new System.Drawing.Size(810, 501);
            this.cHistoryGrid.StyleInfo = resources.GetString("cHistoryGrid.StyleInfo");
            this.cHistoryGrid.TabIndex = 1;
            this.cHistoryGrid.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver;
            this.cHistoryGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cHistoryGrid_MouseClick);
            // 
            // FormReturnRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 513);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormReturnRecord";
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