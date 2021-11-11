namespace CabinetMgr
{
    partial class FormToolCheck
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormToolCheck));
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cCheckGrid = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.btnExecCheck = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tbToolStandard = new System.Windows.Forms.TextBox();
            this.cbToolPosition = new System.Windows.Forms.ComboBox();
            this.tbToolName = new System.Windows.Forms.TextBox();
            this.cbToolPositionType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cCheckGrid)).BeginInit();
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
            this.button1.Location = new System.Drawing.Point(5, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 29);
            this.button1.TabIndex = 251;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.panel1.Controls.Add(this.cCheckGrid);
            this.panel1.Controls.Add(this.btnExecCheck);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.tbToolStandard);
            this.panel1.Controls.Add(this.cbToolPosition);
            this.panel1.Controls.Add(this.tbToolName);
            this.panel1.Controls.Add(this.cbToolPositionType);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(5, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(830, 561);
            this.panel1.TabIndex = 250;
            // 
            // cCheckGrid
            // 
            this.cCheckGrid.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.cCheckGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.cCheckGrid.ColumnInfo = "10,1,0,0,0,100,Columns:";
            this.cCheckGrid.Font = new System.Drawing.Font("宋体", 12F);
            this.cCheckGrid.Location = new System.Drawing.Point(4, 107);
            this.cCheckGrid.Name = "cCheckGrid";
            this.cCheckGrid.Rows.DefaultSize = 20;
            this.cCheckGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.cCheckGrid.Size = new System.Drawing.Size(823, 451);
            this.cCheckGrid.StyleInfo = resources.GetString("cCheckGrid.StyleInfo");
            this.cCheckGrid.TabIndex = 247;
            this.cCheckGrid.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver;
            this.cCheckGrid.CellChanged += new C1.Win.C1FlexGrid.RowColEventHandler(this.cCheckGrid_CellChanged);
            this.cCheckGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cCheckGrid_MouseClick);
            // 
            // btnExecCheck
            // 
            this.btnExecCheck.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExecCheck.BackgroundImage")));
            this.btnExecCheck.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExecCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExecCheck.FlatAppearance.BorderSize = 0;
            this.btnExecCheck.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExecCheck.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExecCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExecCheck.Location = new System.Drawing.Point(719, 42);
            this.btnExecCheck.Name = "btnExecCheck";
            this.btnExecCheck.Size = new System.Drawing.Size(107, 30);
            this.btnExecCheck.TabIndex = 246;
            this.btnExecCheck.UseVisualStyleBackColor = true;
            this.btnExecCheck.Visible = false;
            this.btnExecCheck.Click += new System.EventHandler(this.btnExecCheck_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.label15.Location = new System.Drawing.Point(366, 23);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(107, 25);
            this.label15.TabIndex = 243;
            this.label15.Text = "工具位置：";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.label14.Location = new System.Drawing.Point(9, 22);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(107, 25);
            this.label14.TabIndex = 242;
            this.label14.Text = "所在层级：";
            // 
            // tbToolStandard
            // 
            this.tbToolStandard.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.tbToolStandard.Location = new System.Drawing.Point(479, 62);
            this.tbToolStandard.Name = "tbToolStandard";
            this.tbToolStandard.ReadOnly = true;
            this.tbToolStandard.Size = new System.Drawing.Size(235, 32);
            this.tbToolStandard.TabIndex = 20;
            // 
            // cbToolPosition
            // 
            this.cbToolPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbToolPosition.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.cbToolPosition.FormattingEnabled = true;
            this.cbToolPosition.Location = new System.Drawing.Point(479, 19);
            this.cbToolPosition.Name = "cbToolPosition";
            this.cbToolPosition.Size = new System.Drawing.Size(235, 33);
            this.cbToolPosition.TabIndex = 18;
            this.cbToolPosition.SelectedIndexChanged += new System.EventHandler(this.cbToolPosition_SelectedIndexChanged);
            // 
            // tbToolName
            // 
            this.tbToolName.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.tbToolName.Location = new System.Drawing.Point(120, 61);
            this.tbToolName.Name = "tbToolName";
            this.tbToolName.ReadOnly = true;
            this.tbToolName.Size = new System.Drawing.Size(235, 32);
            this.tbToolName.TabIndex = 17;
            // 
            // cbToolPositionType
            // 
            this.cbToolPositionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbToolPositionType.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.cbToolPositionType.FormattingEnabled = true;
            this.cbToolPositionType.Items.AddRange(new object[] {
            "上层",
            "下层"});
            this.cbToolPositionType.Location = new System.Drawing.Point(120, 19);
            this.cbToolPositionType.Name = "cbToolPositionType";
            this.cbToolPositionType.Size = new System.Drawing.Size(235, 33);
            this.cbToolPositionType.TabIndex = 15;
            this.cbToolPositionType.SelectedIndexChanged += new System.EventHandler(this.cbToolPositionType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.label3.Location = new System.Drawing.Point(366, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "工具标准：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.label1.Location = new System.Drawing.Point(9, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "工具名称：";
            // 
            // FormToolCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::CabinetMgr.Properties.Resources.right_2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(871, 650);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormToolCheck";
            this.Text = "FormToolCheck";
            this.Load += new System.EventHandler(this.FormToolCheck_Load);
            this.VisibleChanged += new System.EventHandler(this.FormToolCheck_VisibleChanged);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cCheckGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExecCheck;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbToolStandard;
        private System.Windows.Forms.ComboBox cbToolPosition;
        private System.Windows.Forms.TextBox tbToolName;
        private System.Windows.Forms.ComboBox cbToolPositionType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private C1.Win.C1FlexGrid.C1FlexGrid cCheckGrid;
    }
}