namespace CabinetMgr
{
    partial class FormToolManage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormToolManage));
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbToolType = new System.Windows.Forms.ComboBox();
            this.btnAddTool = new System.Windows.Forms.Button();
            this.btnDelTool = new System.Windows.Forms.Button();
            this.tbToolSpec = new System.Windows.Forms.TextBox();
            this.tbToolName = new System.Windows.Forms.TextBox();
            this.btnModifyTool = new System.Windows.Forms.Button();
            this.cToolGrid = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cToolGrid)).BeginInit();
            this.SuspendLayout();
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
            this.button1.Location = new System.Drawing.Point(16, 11);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(185, 36);
            this.button1.TabIndex = 249;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.cToolGrid);
            this.panel1.Location = new System.Drawing.Point(8, 48);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(809, 514);
            this.panel1.TabIndex = 250;
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Controls.Add(this.cbToolType);
            this.panel2.Controls.Add(this.btnAddTool);
            this.panel2.Controls.Add(this.btnDelTool);
            this.panel2.Controls.Add(this.tbToolSpec);
            this.panel2.Controls.Add(this.tbToolName);
            this.panel2.Controls.Add(this.btnModifyTool);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(809, 35);
            this.panel2.TabIndex = 272;
            // 
            // cbToolType
            // 
            this.cbToolType.FormattingEnabled = true;
            this.cbToolType.Location = new System.Drawing.Point(721, 4);
            this.cbToolType.Margin = new System.Windows.Forms.Padding(4);
            this.cbToolType.Name = "cbToolType";
            this.cbToolType.Size = new System.Drawing.Size(87, 23);
            this.cbToolType.TabIndex = 271;
            this.cbToolType.SelectedIndexChanged += new System.EventHandler(this.cbToolType_SelectedIndexChanged);
            // 
            // btnAddTool
            // 
            this.btnAddTool.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAddTool.BackgroundImage")));
            this.btnAddTool.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddTool.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddTool.FlatAppearance.BorderSize = 0;
            this.btnAddTool.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAddTool.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAddTool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTool.Location = new System.Drawing.Point(4, 2);
            this.btnAddTool.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddTool.Name = "btnAddTool";
            this.btnAddTool.Size = new System.Drawing.Size(72, 30);
            this.btnAddTool.TabIndex = 266;
            this.btnAddTool.UseVisualStyleBackColor = true;
            this.btnAddTool.Click += new System.EventHandler(this.btnAddTool_Click);
            // 
            // btnDelTool
            // 
            this.btnDelTool.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelTool.BackgroundImage")));
            this.btnDelTool.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDelTool.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelTool.FlatAppearance.BorderSize = 0;
            this.btnDelTool.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnDelTool.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnDelTool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelTool.Location = new System.Drawing.Point(80, 2);
            this.btnDelTool.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelTool.Name = "btnDelTool";
            this.btnDelTool.Size = new System.Drawing.Size(72, 30);
            this.btnDelTool.TabIndex = 267;
            this.btnDelTool.UseVisualStyleBackColor = true;
            this.btnDelTool.Click += new System.EventHandler(this.btnDelTool_Click);
            // 
            // tbToolSpec
            // 
            this.tbToolSpec.Location = new System.Drawing.Point(619, 3);
            this.tbToolSpec.Margin = new System.Windows.Forms.Padding(4);
            this.tbToolSpec.Multiline = true;
            this.tbToolSpec.Name = "tbToolSpec";
            this.tbToolSpec.Size = new System.Drawing.Size(96, 27);
            this.tbToolSpec.TabIndex = 269;
            this.tbToolSpec.TextChanged += new System.EventHandler(this.tbToolSpec_TextChanged);
            // 
            // tbToolName
            // 
            this.tbToolName.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar;
            this.tbToolName.Location = new System.Drawing.Point(505, 3);
            this.tbToolName.Margin = new System.Windows.Forms.Padding(4);
            this.tbToolName.Multiline = true;
            this.tbToolName.Name = "tbToolName";
            this.tbToolName.Size = new System.Drawing.Size(79, 27);
            this.tbToolName.TabIndex = 270;
            this.tbToolName.TextChanged += new System.EventHandler(this.tbToolName_TextChanged);
            // 
            // btnModifyTool
            // 
            this.btnModifyTool.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnModifyTool.BackgroundImage")));
            this.btnModifyTool.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnModifyTool.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnModifyTool.FlatAppearance.BorderSize = 0;
            this.btnModifyTool.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnModifyTool.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnModifyTool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModifyTool.Location = new System.Drawing.Point(160, 2);
            this.btnModifyTool.Margin = new System.Windows.Forms.Padding(4);
            this.btnModifyTool.Name = "btnModifyTool";
            this.btnModifyTool.Size = new System.Drawing.Size(72, 30);
            this.btnModifyTool.TabIndex = 268;
            this.btnModifyTool.UseVisualStyleBackColor = true;
            this.btnModifyTool.Click += new System.EventHandler(this.btnModifyTool_Click);
            // 
            // cToolGrid
            // 
            this.cToolGrid.AllowEditing = false;
            this.cToolGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.cToolGrid.ColumnInfo = resources.GetString("cToolGrid.ColumnInfo");
            this.cToolGrid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cToolGrid.Location = new System.Drawing.Point(0, 38);
            this.cToolGrid.Margin = new System.Windows.Forms.Padding(4);
            this.cToolGrid.Name = "cToolGrid";
            this.cToolGrid.Rows.DefaultSize = 20;
            this.cToolGrid.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.cToolGrid.Size = new System.Drawing.Size(809, 476);
            this.cToolGrid.StyleInfo = resources.GetString("cToolGrid.StyleInfo");
            this.cToolGrid.TabIndex = 2;
            this.cToolGrid.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver;
            // 
            // FormToolManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::CabinetMgr.Properties.Resources.right_2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(854, 613);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormToolManage";
            this.Text = "FormToolManage";
            this.Load += new System.EventHandler(this.FormToolManage_Load);
            this.VisibleChanged += new System.EventHandler(this.FormToolManage_VisibleChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormToolManage_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cToolGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private C1.Win.C1FlexGrid.C1FlexGrid cToolGrid;
        private System.Windows.Forms.TextBox tbToolName;
        private System.Windows.Forms.TextBox tbToolSpec;
        private System.Windows.Forms.Button btnModifyTool;
        private System.Windows.Forms.Button btnDelTool;
        private System.Windows.Forms.Button btnAddTool;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cbToolType;
    }
}