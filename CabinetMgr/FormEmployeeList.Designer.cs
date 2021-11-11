namespace CabinetMgr
{
    partial class FormEmployeeList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmployeeList));
            this.cEmployeeGrid = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.btnFpRegister = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.cEmployeeGrid)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cEmployeeGrid
            // 
            this.cEmployeeGrid.AllowEditing = false;
            this.cEmployeeGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.cEmployeeGrid.ColumnInfo = "10,1,0,0,0,100,Columns:";
            this.cEmployeeGrid.Location = new System.Drawing.Point(0, 36);
            this.cEmployeeGrid.Name = "cEmployeeGrid";
            this.cEmployeeGrid.Rows.DefaultSize = 20;
            this.cEmployeeGrid.Size = new System.Drawing.Size(836, 488);
            this.cEmployeeGrid.StyleInfo = resources.GetString("cEmployeeGrid.StyleInfo");
            this.cEmployeeGrid.TabIndex = 0;
            this.cEmployeeGrid.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver;
            // 
            // btnFpRegister
            // 
            this.btnFpRegister.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFpRegister.BackgroundImage")));
            this.btnFpRegister.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFpRegister.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFpRegister.FlatAppearance.BorderSize = 0;
            this.btnFpRegister.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnFpRegister.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnFpRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFpRegister.Location = new System.Drawing.Point(130, 6);
            this.btnFpRegister.Name = "btnFpRegister";
            this.btnFpRegister.Size = new System.Drawing.Size(54, 24);
            this.btnFpRegister.TabIndex = 264;
            this.btnFpRegister.UseVisualStyleBackColor = true;
            this.btnFpRegister.Click += new System.EventHandler(this.btnFpRegister_Click);
            // 
            // button1
            // 
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(2, 44);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 29);
            this.button1.TabIndex = 266;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.cEmployeeGrid);
            this.panel1.Controls.Add(this.btnFpRegister);
            this.panel1.Location = new System.Drawing.Point(2, 73);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(840, 525);
            this.panel1.TabIndex = 267;
            // 
            // button2
            // 
            this.button2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button2.BackgroundImage")));
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(10, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(54, 24);
            this.button2.TabIndex = 265;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // FormEmployeeList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::CabinetMgr.Properties.Resources.right_2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(871, 650);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormEmployeeList";
            this.Text = "职工信息";
            this.Load += new System.EventHandler(this.FormEmployeeList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cEmployeeGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private C1.Win.C1FlexGrid.C1FlexGrid cEmployeeGrid;
        private System.Windows.Forms.Button btnFpRegister;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
    }
}