namespace ZkCollector
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.label1 = new System.Windows.Forms.Label();
            this.tbQueryName = new System.Windows.Forms.TextBox();
            this.btnNameQuery = new DevExpress.XtraEditors.SimpleButton();
            this.mainTheme = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.tbUserOrg = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbUserSex = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbUserAge = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbUserTel = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbFpVX2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbFpVX1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbFpV92 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbFpV91 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnCodeQuery = new DevExpress.XtraEditors.SimpleButton();
            this.tbQueryCode = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnCollectV91 = new DevExpress.XtraEditors.SimpleButton();
            this.btnCollectV92 = new DevExpress.XtraEditors.SimpleButton();
            this.btnCollectVX1 = new DevExpress.XtraEditors.SimpleButton();
            this.btnCollectVX2 = new DevExpress.XtraEditors.SimpleButton();
            this.btnSubmit = new DevExpress.XtraEditors.SimpleButton();
            this.btnOrgQuery = new DevExpress.XtraEditors.SimpleButton();
            this.tbQueryOrg = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 45);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "姓名";
            // 
            // tbQueryName
            // 
            this.tbQueryName.Location = new System.Drawing.Point(81, 40);
            this.tbQueryName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbQueryName.Name = "tbQueryName";
            this.tbQueryName.Size = new System.Drawing.Size(204, 26);
            this.tbQueryName.TabIndex = 0;
            this.tbQueryName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbFullName_KeyDown);
            // 
            // btnNameQuery
            // 
            this.btnNameQuery.Location = new System.Drawing.Point(295, 39);
            this.btnNameQuery.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnNameQuery.Name = "btnNameQuery";
            this.btnNameQuery.Size = new System.Drawing.Size(75, 29);
            this.btnNameQuery.TabIndex = 1;
            this.btnNameQuery.Text = "查询";
            this.btnNameQuery.Click += new System.EventHandler(this.btnNameQuery_Click);
            // 
            // mainTheme
            // 
            this.mainTheme.LookAndFeel.SkinName = "Office 2010 Blue";
            // 
            // tbUserOrg
            // 
            this.tbUserOrg.Location = new System.Drawing.Point(81, 52);
            this.tbUserOrg.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbUserOrg.Name = "tbUserOrg";
            this.tbUserOrg.ReadOnly = true;
            this.tbUserOrg.Size = new System.Drawing.Size(287, 26);
            this.tbUserOrg.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 58);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "班组";
            // 
            // tbUserSex
            // 
            this.tbUserSex.Location = new System.Drawing.Point(81, 118);
            this.tbUserSex.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbUserSex.Name = "tbUserSex";
            this.tbUserSex.Size = new System.Drawing.Size(287, 26);
            this.tbUserSex.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 121);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "性别";
            // 
            // tbUserAge
            // 
            this.tbUserAge.Location = new System.Drawing.Point(81, 182);
            this.tbUserAge.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbUserAge.Name = "tbUserAge";
            this.tbUserAge.Size = new System.Drawing.Size(287, 26);
            this.tbUserAge.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 188);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "年龄";
            // 
            // tbUserTel
            // 
            this.tbUserTel.Location = new System.Drawing.Point(81, 248);
            this.tbUserTel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbUserTel.Name = "tbUserTel";
            this.tbUserTel.Size = new System.Drawing.Size(287, 26);
            this.tbUserTel.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(35, 251);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 18);
            this.label5.TabIndex = 9;
            this.label5.Text = "电话";
            // 
            // tbFpVX2
            // 
            this.tbFpVX2.Location = new System.Drawing.Point(524, 249);
            this.tbFpVX2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbFpVX2.Name = "tbFpVX2";
            this.tbFpVX2.ReadOnly = true;
            this.tbFpVX2.Size = new System.Drawing.Size(287, 26);
            this.tbFpVX2.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(407, 251);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 18);
            this.label6.TabIndex = 17;
            this.label6.Text = "指纹2(工具柜)";
            // 
            // tbFpVX1
            // 
            this.tbFpVX1.Location = new System.Drawing.Point(524, 184);
            this.tbFpVX1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbFpVX1.Name = "tbFpVX1";
            this.tbFpVX1.ReadOnly = true;
            this.tbFpVX1.Size = new System.Drawing.Size(287, 26);
            this.tbFpVX1.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(407, 188);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(103, 18);
            this.label7.TabIndex = 15;
            this.label7.Text = "指纹1(工具柜)";
            // 
            // tbFpV92
            // 
            this.tbFpV92.Location = new System.Drawing.Point(524, 119);
            this.tbFpV92.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbFpV92.Name = "tbFpV92";
            this.tbFpV92.ReadOnly = true;
            this.tbFpV92.Size = new System.Drawing.Size(287, 26);
            this.tbFpV92.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(407, 121);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 18);
            this.label8.TabIndex = 13;
            this.label8.Text = "指纹2(V9)";
            // 
            // tbFpV91
            // 
            this.tbFpV91.Location = new System.Drawing.Point(524, 54);
            this.tbFpV91.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbFpV91.Name = "tbFpV91";
            this.tbFpV91.ReadOnly = true;
            this.tbFpV91.Size = new System.Drawing.Size(287, 26);
            this.tbFpV91.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(407, 58);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 18);
            this.label9.TabIndex = 11;
            this.label9.Text = "指纹1(V9)";
            // 
            // btnCodeQuery
            // 
            this.btnCodeQuery.Location = new System.Drawing.Point(820, 41);
            this.btnCodeQuery.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCodeQuery.Name = "btnCodeQuery";
            this.btnCodeQuery.Size = new System.Drawing.Size(75, 29);
            this.btnCodeQuery.TabIndex = 3;
            this.btnCodeQuery.Text = "查询";
            this.btnCodeQuery.Click += new System.EventHandler(this.btnCodeQuery_Click);
            // 
            // tbQueryCode
            // 
            this.tbQueryCode.Location = new System.Drawing.Point(524, 42);
            this.tbQueryCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbQueryCode.Name = "tbQueryCode";
            this.tbQueryCode.Size = new System.Drawing.Size(287, 26);
            this.tbQueryCode.TabIndex = 2;
            this.tbQueryCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUserCode_KeyDown);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(408, 46);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(38, 18);
            this.label10.TabIndex = 19;
            this.label10.Text = "工号";
            // 
            // btnCollectV91
            // 
            this.btnCollectV91.Location = new System.Drawing.Point(820, 52);
            this.btnCollectV91.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCollectV91.Name = "btnCollectV91";
            this.btnCollectV91.Size = new System.Drawing.Size(75, 29);
            this.btnCollectV91.TabIndex = 8;
            this.btnCollectV91.Text = "采集";
            this.btnCollectV91.Click += new System.EventHandler(this.btnCollectV91_Click);
            // 
            // btnCollectV92
            // 
            this.btnCollectV92.Location = new System.Drawing.Point(820, 118);
            this.btnCollectV92.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCollectV92.Name = "btnCollectV92";
            this.btnCollectV92.Size = new System.Drawing.Size(75, 29);
            this.btnCollectV92.TabIndex = 11;
            this.btnCollectV92.Text = "采集";
            this.btnCollectV92.Click += new System.EventHandler(this.btnCollectV92_Click);
            // 
            // btnCollectVX1
            // 
            this.btnCollectVX1.Location = new System.Drawing.Point(820, 182);
            this.btnCollectVX1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCollectVX1.Name = "btnCollectVX1";
            this.btnCollectVX1.Size = new System.Drawing.Size(75, 29);
            this.btnCollectVX1.TabIndex = 14;
            this.btnCollectVX1.Text = "采集";
            this.btnCollectVX1.Click += new System.EventHandler(this.btnCollectVX1_Click);
            // 
            // btnCollectVX2
            // 
            this.btnCollectVX2.Location = new System.Drawing.Point(820, 248);
            this.btnCollectVX2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCollectVX2.Name = "btnCollectVX2";
            this.btnCollectVX2.Size = new System.Drawing.Size(75, 29);
            this.btnCollectVX2.TabIndex = 17;
            this.btnCollectVX2.Text = "采集";
            this.btnCollectVX2.Click += new System.EventHandler(this.btnCollectVX2_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(409, 302);
            this.btnSubmit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(100, 29);
            this.btnSubmit.TabIndex = 18;
            this.btnSubmit.Text = "提交";
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnOrgQuery
            // 
            this.btnOrgQuery.Location = new System.Drawing.Point(295, 89);
            this.btnOrgQuery.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOrgQuery.Name = "btnOrgQuery";
            this.btnOrgQuery.Size = new System.Drawing.Size(75, 29);
            this.btnOrgQuery.TabIndex = 5;
            this.btnOrgQuery.Text = "查询";
            this.btnOrgQuery.Click += new System.EventHandler(this.btnOrgQuery_Click);
            // 
            // tbQueryOrg
            // 
            this.tbQueryOrg.Location = new System.Drawing.Point(81, 90);
            this.tbQueryOrg.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbQueryOrg.Name = "tbQueryOrg";
            this.tbQueryOrg.Size = new System.Drawing.Size(204, 26);
            this.tbQueryOrg.TabIndex = 4;
            this.tbQueryOrg.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbQueryOrg_KeyDown);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(35, 95);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(38, 18);
            this.label11.TabIndex = 27;
            this.label11.Text = "班组";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.btnNameQuery);
            this.groupControl1.Controls.Add(this.btnOrgQuery);
            this.groupControl1.Controls.Add(this.label1);
            this.groupControl1.Controls.Add(this.tbQueryOrg);
            this.groupControl1.Controls.Add(this.tbQueryName);
            this.groupControl1.Controls.Add(this.label11);
            this.groupControl1.Controls.Add(this.label10);
            this.groupControl1.Controls.Add(this.tbQueryCode);
            this.groupControl1.Controls.Add(this.btnCodeQuery);
            this.groupControl1.Location = new System.Drawing.Point(16, 15);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(932, 145);
            this.groupControl1.TabIndex = 30;
            this.groupControl1.Text = "查询设置";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.tbUserOrg);
            this.groupControl2.Controls.Add(this.label2);
            this.groupControl2.Controls.Add(this.btnSubmit);
            this.groupControl2.Controls.Add(this.label3);
            this.groupControl2.Controls.Add(this.btnCollectVX2);
            this.groupControl2.Controls.Add(this.tbUserSex);
            this.groupControl2.Controls.Add(this.btnCollectVX1);
            this.groupControl2.Controls.Add(this.label4);
            this.groupControl2.Controls.Add(this.btnCollectV92);
            this.groupControl2.Controls.Add(this.tbUserAge);
            this.groupControl2.Controls.Add(this.btnCollectV91);
            this.groupControl2.Controls.Add(this.label5);
            this.groupControl2.Controls.Add(this.tbFpVX2);
            this.groupControl2.Controls.Add(this.tbUserTel);
            this.groupControl2.Controls.Add(this.label6);
            this.groupControl2.Controls.Add(this.label9);
            this.groupControl2.Controls.Add(this.tbFpVX1);
            this.groupControl2.Controls.Add(this.tbFpV91);
            this.groupControl2.Controls.Add(this.label7);
            this.groupControl2.Controls.Add(this.label8);
            this.groupControl2.Controls.Add(this.tbFpV92);
            this.groupControl2.Location = new System.Drawing.Point(16, 168);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(932, 349);
            this.groupControl2.TabIndex = 31;
            this.groupControl2.Text = "人员信息";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.ClientSize = new System.Drawing.Size(964, 530);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormMain";
            this.Text = "指纹采集";
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbQueryName;
        private DevExpress.XtraEditors.SimpleButton btnNameQuery;
        private DevExpress.LookAndFeel.DefaultLookAndFeel mainTheme;
        private System.Windows.Forms.TextBox tbUserOrg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbUserSex;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbUserAge;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbUserTel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbFpVX2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbFpVX1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbFpV92;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbFpV91;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.SimpleButton btnCodeQuery;
        private System.Windows.Forms.TextBox tbQueryCode;
        private System.Windows.Forms.Label label10;
        private DevExpress.XtraEditors.SimpleButton btnCollectV91;
        private DevExpress.XtraEditors.SimpleButton btnCollectV92;
        private DevExpress.XtraEditors.SimpleButton btnCollectVX1;
        private DevExpress.XtraEditors.SimpleButton btnCollectVX2;
        private DevExpress.XtraEditors.SimpleButton btnSubmit;
        private DevExpress.XtraEditors.SimpleButton btnOrgQuery;
        private System.Windows.Forms.TextBox tbQueryOrg;
        private System.Windows.Forms.Label label11;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
    }
}

