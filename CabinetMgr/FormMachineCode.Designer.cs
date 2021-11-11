namespace CabinetMgr
{
    partial class FormMachineCode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMachineCode));
            this.lbMachineCode = new System.Windows.Forms.ListBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbMachineCode = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbMachineCode
            // 
            this.lbMachineCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbMachineCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbMachineCode.Font = new System.Drawing.Font("宋体", 12F);
            this.lbMachineCode.FormattingEnabled = true;
            this.lbMachineCode.ItemHeight = 16;
            this.lbMachineCode.Location = new System.Drawing.Point(0, 0);
            this.lbMachineCode.Name = "lbMachineCode";
            this.lbMachineCode.Size = new System.Drawing.Size(297, 288);
            this.lbMachineCode.TabIndex = 0;
            this.lbMachineCode.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbMachineCode_MouseDoubleClick);
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.BackgroundImage = global::CabinetMgr.Properties.Resources.btnconfirm;
            this.btnConfirm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnConfirm.FlatAppearance.BorderSize = 0;
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.Font = new System.Drawing.Font("宋体", 12F);
            this.btnConfirm.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnConfirm.Location = new System.Drawing.Point(12, 331);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(123, 32);
            this.btnConfirm.TabIndex = 1;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BackgroundImage = global::CabinetMgr.Properties.Resources.btnconfirm;
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("宋体", 12F);
            this.btnExit.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnExit.Location = new System.Drawing.Point(160, 331);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(123, 32);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "关闭";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 304);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "搜索条件";
            // 
            // tbMachineCode
            // 
            this.tbMachineCode.Location = new System.Drawing.Point(69, 301);
            this.tbMachineCode.Name = "tbMachineCode";
            this.tbMachineCode.Size = new System.Drawing.Size(214, 21);
            this.tbMachineCode.TabIndex = 4;
            this.tbMachineCode.TextChanged += new System.EventHandler(this.tbMachineCode_TextChanged);
            // 
            // FormMachineCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 375);
            this.Controls.Add(this.tbMachineCode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.lbMachineCode);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMachineCode";
            this.Text = "部件码选择";
            this.Load += new System.EventHandler(this.FormMachineCode_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbMachineCode;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbMachineCode;
    }
}