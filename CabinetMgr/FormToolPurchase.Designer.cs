namespace CabinetMgr
{
    partial class FormToolPurchase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormToolPurchase));
            this.button1 = new System.Windows.Forms.Button();
            this.panelToolPurchase = new System.Windows.Forms.Panel();
            this.cRequestGrid = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.tbToolCount = new System.Windows.Forms.TextBox();
            this.tbToolSpec = new System.Windows.Forms.TextBox();
            this.tbToolName = new System.Windows.Forms.TextBox();
            this.cbReqStatus = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelToolPurchase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cRequestGrid)).BeginInit();
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
            this.button1.Location = new System.Drawing.Point(3, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 29);
            this.button1.TabIndex = 252;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // panelToolPurchase
            // 
            this.panelToolPurchase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.panelToolPurchase.Controls.Add(this.cRequestGrid);
            this.panelToolPurchase.Controls.Add(this.btnSubmit);
            this.panelToolPurchase.Controls.Add(this.tbToolCount);
            this.panelToolPurchase.Controls.Add(this.tbToolSpec);
            this.panelToolPurchase.Controls.Add(this.tbToolName);
            this.panelToolPurchase.Controls.Add(this.cbReqStatus);
            this.panelToolPurchase.Controls.Add(this.label6);
            this.panelToolPurchase.Controls.Add(this.label4);
            this.panelToolPurchase.Controls.Add(this.label2);
            this.panelToolPurchase.Controls.Add(this.label1);
            this.panelToolPurchase.Location = new System.Drawing.Point(3, 39);
            this.panelToolPurchase.Name = "panelToolPurchase";
            this.panelToolPurchase.Size = new System.Drawing.Size(833, 560);
            this.panelToolPurchase.TabIndex = 251;
            // 
            // cRequestGrid
            // 
            this.cRequestGrid.AllowEditing = false;
            this.cRequestGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.cRequestGrid.ColumnInfo = "10,1,0,0,0,100,Columns:";
            this.cRequestGrid.Location = new System.Drawing.Point(5, 122);
            this.cRequestGrid.Name = "cRequestGrid";
            this.cRequestGrid.Rows.DefaultSize = 20;
            this.cRequestGrid.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.cRequestGrid.Size = new System.Drawing.Size(823, 435);
            this.cRequestGrid.StyleInfo = resources.GetString("cRequestGrid.StyleInfo");
            this.cRequestGrid.TabIndex = 240;
            this.cRequestGrid.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver;
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSubmit.BackgroundImage")));
            this.btnSubmit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSubmit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSubmit.FlatAppearance.BorderSize = 0;
            this.btnSubmit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSubmit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.Location = new System.Drawing.Point(656, 46);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(115, 30);
            this.btnSubmit.TabIndex = 239;
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // tbToolCount
            // 
            this.tbToolCount.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.tbToolCount.Location = new System.Drawing.Point(113, 71);
            this.tbToolCount.Name = "tbToolCount";
            this.tbToolCount.Size = new System.Drawing.Size(182, 32);
            this.tbToolCount.TabIndex = 11;
            // 
            // tbToolSpec
            // 
            this.tbToolSpec.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.tbToolSpec.Location = new System.Drawing.Point(413, 20);
            this.tbToolSpec.Name = "tbToolSpec";
            this.tbToolSpec.Size = new System.Drawing.Size(182, 32);
            this.tbToolSpec.TabIndex = 10;
            // 
            // tbToolName
            // 
            this.tbToolName.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.tbToolName.Location = new System.Drawing.Point(113, 20);
            this.tbToolName.Name = "tbToolName";
            this.tbToolName.Size = new System.Drawing.Size(182, 32);
            this.tbToolName.TabIndex = 9;
            // 
            // cbReqStatus
            // 
            this.cbReqStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReqStatus.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.cbReqStatus.FormattingEnabled = true;
            this.cbReqStatus.Items.AddRange(new object[] {
            "未解决",
            "解决中",
            "已完成"});
            this.cbReqStatus.Location = new System.Drawing.Point(413, 71);
            this.cbReqStatus.Name = "cbReqStatus";
            this.cbReqStatus.Size = new System.Drawing.Size(182, 33);
            this.cbReqStatus.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.label6.Location = new System.Drawing.Point(310, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 25);
            this.label6.TabIndex = 6;
            this.label6.Text = "申请状态：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.label4.Location = new System.Drawing.Point(12, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 25);
            this.label4.TabIndex = 4;
            this.label4.Text = "申请数量：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.label2.Location = new System.Drawing.Point(310, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "工具规格：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "工具名称：";
            // 
            // FormToolPurchase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::CabinetMgr.Properties.Resources.right_2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(871, 650);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panelToolPurchase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormToolPurchase";
            this.Text = "工具采购";
            this.VisibleChanged += new System.EventHandler(this.FormToolPurchase_VisibleChanged);
            this.panelToolPurchase.ResumeLayout(false);
            this.panelToolPurchase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cRequestGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panelToolPurchase;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.TextBox tbToolCount;
        private System.Windows.Forms.TextBox tbToolSpec;
        private System.Windows.Forms.TextBox tbToolName;
        private System.Windows.Forms.ComboBox cbReqStatus;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private C1.Win.C1FlexGrid.C1FlexGrid cRequestGrid;
    }
}