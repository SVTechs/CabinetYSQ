namespace FKCollector
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCodeQuery = new C1.Win.C1Input.C1Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbQueryCode = new System.Windows.Forms.TextBox();
            this.btnUpdate = new C1.Win.C1Input.C1Button();
            this.btnGetData = new C1.Win.C1Input.C1Button();
            this.btnConnStr = new C1.Win.C1Input.C1Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSaveUser = new C1.Win.C1Input.C1Button();
            this.label10 = new System.Windows.Forms.Label();
            this.tbFaceTemplateV10 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbRightTemplateV10 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbLeftTemplateV10 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbFACETEMPLATE = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbRIGHTTEMPLATE = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbLEFTTEMPLATE = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbEnrollId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbOrgId = new System.Windows.Forms.TextBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rbDevice = new System.Windows.Forms.RadioButton();
            this.rbDB = new System.Windows.Forms.RadioButton();
            this.lblDataCount = new System.Windows.Forms.Label();
            this.lblDeviceType = new System.Windows.Forms.Label();
            this.lbRemain = new System.Windows.Forms.ListBox();
            this.c1Button1 = new C1.Win.C1Input.C1Button();
            this.c1Button2 = new C1.Win.C1Input.C1Button();
            this.lbChosen = new System.Windows.Forms.ListBox();
            this.btnSaveOrg = new C1.Win.C1Input.C1Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnCodeQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGetData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnConnStr)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Button1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Button2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveOrg)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCodeQuery);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbQueryCode);
            this.groupBox1.Location = new System.Drawing.Point(900, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(932, 89);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询设置";
            this.groupBox1.Visible = false;
            // 
            // btnCodeQuery
            // 
            this.btnCodeQuery.Location = new System.Drawing.Point(305, 39);
            this.btnCodeQuery.Name = "btnCodeQuery";
            this.btnCodeQuery.Size = new System.Drawing.Size(75, 29);
            this.btnCodeQuery.TabIndex = 11;
            this.btnCodeQuery.Text = "查询";
            this.btnCodeQuery.UseVisualStyleBackColor = true;
            this.btnCodeQuery.Click += new System.EventHandler(this.btnCodeQuery_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "工号";
            // 
            // tbQueryCode
            // 
            this.tbQueryCode.Location = new System.Drawing.Point(73, 39);
            this.tbQueryCode.Name = "tbQueryCode";
            this.tbQueryCode.Size = new System.Drawing.Size(204, 25);
            this.tbQueryCode.TabIndex = 6;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("宋体", 10F);
            this.btnUpdate.Location = new System.Drawing.Point(589, 270);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(126, 29);
            this.btnUpdate.TabIndex = 14;
            this.btnUpdate.Text = "更新";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnGetData
            // 
            this.btnGetData.Font = new System.Drawing.Font("宋体", 10F);
            this.btnGetData.Location = new System.Drawing.Point(438, 271);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(126, 29);
            this.btnGetData.TabIndex = 13;
            this.btnGetData.Text = "获取数据";
            this.btnGetData.UseVisualStyleBackColor = true;
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // btnConnStr
            // 
            this.btnConnStr.Location = new System.Drawing.Point(261, 499);
            this.btnConnStr.Name = "btnConnStr";
            this.btnConnStr.Size = new System.Drawing.Size(126, 29);
            this.btnConnStr.TabIndex = 12;
            this.btnConnStr.Text = "数据库连接修改";
            this.btnConnStr.UseVisualStyleBackColor = true;
            this.btnConnStr.Visible = false;
            this.btnConnStr.Click += new System.EventHandler(this.btnConnStr_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSaveUser);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.tbFaceTemplateV10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.tbRightTemplateV10);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.tbLeftTemplateV10);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.tbFACETEMPLATE);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.tbRIGHTTEMPLATE);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.tbLEFTTEMPLATE);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tbEnrollId);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.tbOrgId);
            this.groupBox2.Controls.Add(this.tbName);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(900, 130);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(917, 279);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "人员信息";
            this.groupBox2.Visible = false;
            // 
            // btnSaveUser
            // 
            this.btnSaveUser.Location = new System.Drawing.Point(423, 244);
            this.btnSaveUser.Name = "btnSaveUser";
            this.btnSaveUser.Size = new System.Drawing.Size(75, 29);
            this.btnSaveUser.TabIndex = 12;
            this.btnSaveUser.Text = "保存";
            this.btnSaveUser.UseVisualStyleBackColor = true;
            this.btnSaveUser.Visible = false;
            this.btnSaveUser.Click += new System.EventHandler(this.btnSaveUser_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(420, 193);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 15);
            this.label10.TabIndex = 17;
            this.label10.Text = "中控人脸";
            // 
            // tbFaceTemplateV10
            // 
            this.tbFaceTemplateV10.Location = new System.Drawing.Point(533, 188);
            this.tbFaceTemplateV10.Name = "tbFaceTemplateV10";
            this.tbFaceTemplateV10.Size = new System.Drawing.Size(294, 25);
            this.tbFaceTemplateV10.TabIndex = 16;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(420, 138);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(99, 15);
            this.label9.TabIndex = 15;
            this.label9.Text = "中控V10指纹2";
            // 
            // tbRightTemplateV10
            // 
            this.tbRightTemplateV10.Location = new System.Drawing.Point(533, 133);
            this.tbRightTemplateV10.Name = "tbRightTemplateV10";
            this.tbRightTemplateV10.Size = new System.Drawing.Size(294, 25);
            this.tbRightTemplateV10.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(420, 83);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(99, 15);
            this.label8.TabIndex = 13;
            this.label8.Text = "中控V10指纹1";
            // 
            // tbLeftTemplateV10
            // 
            this.tbLeftTemplateV10.Location = new System.Drawing.Point(533, 78);
            this.tbLeftTemplateV10.Name = "tbLeftTemplateV10";
            this.tbLeftTemplateV10.Size = new System.Drawing.Size(294, 25);
            this.tbLeftTemplateV10.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(420, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 15);
            this.label7.TabIndex = 11;
            this.label7.Text = "zoko人脸";
            // 
            // tbFACETEMPLATE
            // 
            this.tbFACETEMPLATE.Location = new System.Drawing.Point(533, 23);
            this.tbFACETEMPLATE.Name = "tbFACETEMPLATE";
            this.tbFACETEMPLATE.Size = new System.Drawing.Size(294, 25);
            this.tbFACETEMPLATE.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 248);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 15);
            this.label6.TabIndex = 9;
            this.label6.Text = "zoko指纹2";
            // 
            // tbRIGHTTEMPLATE
            // 
            this.tbRIGHTTEMPLATE.Location = new System.Drawing.Point(99, 248);
            this.tbRIGHTTEMPLATE.Name = "tbRIGHTTEMPLATE";
            this.tbRIGHTTEMPLATE.Size = new System.Drawing.Size(294, 25);
            this.tbRIGHTTEMPLATE.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 193);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 15);
            this.label5.TabIndex = 7;
            this.label5.Text = "zoko指纹1";
            // 
            // tbLEFTTEMPLATE
            // 
            this.tbLEFTTEMPLATE.Location = new System.Drawing.Point(99, 188);
            this.tbLEFTTEMPLATE.Name = "tbLEFTTEMPLATE";
            this.tbLEFTTEMPLATE.Size = new System.Drawing.Size(294, 25);
            this.tbLEFTTEMPLATE.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "序号";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tbEnrollId
            // 
            this.tbEnrollId.Location = new System.Drawing.Point(99, 133);
            this.tbEnrollId.Name = "tbEnrollId";
            this.tbEnrollId.Size = new System.Drawing.Size(294, 25);
            this.tbEnrollId.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "分组";
            // 
            // tbOrgId
            // 
            this.tbOrgId.Location = new System.Drawing.Point(99, 78);
            this.tbOrgId.Name = "tbOrgId";
            this.tbOrgId.Size = new System.Drawing.Size(294, 25);
            this.tbOrgId.TabIndex = 2;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(99, 23);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(294, 25);
            this.tbName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "姓名";
            // 
            // rbDevice
            // 
            this.rbDevice.AutoSize = true;
            this.rbDevice.Font = new System.Drawing.Font("宋体", 10F);
            this.rbDevice.Location = new System.Drawing.Point(202, 276);
            this.rbDevice.Name = "rbDevice";
            this.rbDevice.Size = new System.Drawing.Size(80, 21);
            this.rbDevice.TabIndex = 15;
            this.rbDevice.TabStop = true;
            this.rbDevice.Text = "考勤机";
            this.rbDevice.UseVisualStyleBackColor = true;
            // 
            // rbDB
            // 
            this.rbDB.AutoSize = true;
            this.rbDB.Font = new System.Drawing.Font("宋体", 10F);
            this.rbDB.Location = new System.Drawing.Point(296, 275);
            this.rbDB.Name = "rbDB";
            this.rbDB.Size = new System.Drawing.Size(80, 21);
            this.rbDB.TabIndex = 16;
            this.rbDB.TabStop = true;
            this.rbDB.Text = "数据库";
            this.rbDB.UseVisualStyleBackColor = true;
            // 
            // lblDataCount
            // 
            this.lblDataCount.AutoSize = true;
            this.lblDataCount.Font = new System.Drawing.Font("宋体", 10F);
            this.lblDataCount.Location = new System.Drawing.Point(147, 321);
            this.lblDataCount.Name = "lblDataCount";
            this.lblDataCount.Size = new System.Drawing.Size(76, 17);
            this.lblDataCount.TabIndex = 17;
            this.lblDataCount.Text = "数据共：";
            // 
            // lblDeviceType
            // 
            this.lblDeviceType.AutoSize = true;
            this.lblDeviceType.Font = new System.Drawing.Font("宋体", 10F);
            this.lblDeviceType.Location = new System.Drawing.Point(51, 279);
            this.lblDeviceType.Name = "lblDeviceType";
            this.lblDeviceType.Size = new System.Drawing.Size(85, 17);
            this.lblDeviceType.TabIndex = 18;
            this.lblDeviceType.Text = "设备类型:";
            // 
            // lbRemain
            // 
            this.lbRemain.FormattingEnabled = true;
            this.lbRemain.ItemHeight = 15;
            this.lbRemain.Location = new System.Drawing.Point(54, 29);
            this.lbRemain.Name = "lbRemain";
            this.lbRemain.Size = new System.Drawing.Size(153, 229);
            this.lbRemain.TabIndex = 19;
            // 
            // c1Button1
            // 
            this.c1Button1.Font = new System.Drawing.Font("宋体", 10F);
            this.c1Button1.Location = new System.Drawing.Point(250, 72);
            this.c1Button1.Name = "c1Button1";
            this.c1Button1.Size = new System.Drawing.Size(126, 29);
            this.c1Button1.TabIndex = 20;
            this.c1Button1.Text = ">>>";
            this.c1Button1.UseVisualStyleBackColor = true;
            this.c1Button1.Click += new System.EventHandler(this.c1Button1_Click);
            // 
            // c1Button2
            // 
            this.c1Button2.Font = new System.Drawing.Font("宋体", 10F);
            this.c1Button2.Location = new System.Drawing.Point(250, 158);
            this.c1Button2.Name = "c1Button2";
            this.c1Button2.Size = new System.Drawing.Size(126, 29);
            this.c1Button2.TabIndex = 21;
            this.c1Button2.Text = "<<<";
            this.c1Button2.UseVisualStyleBackColor = true;
            this.c1Button2.Click += new System.EventHandler(this.c1Button2_Click);
            // 
            // lbChosen
            // 
            this.lbChosen.FormattingEnabled = true;
            this.lbChosen.ItemHeight = 15;
            this.lbChosen.Location = new System.Drawing.Point(411, 29);
            this.lbChosen.Name = "lbChosen";
            this.lbChosen.Size = new System.Drawing.Size(153, 229);
            this.lbChosen.TabIndex = 22;
            // 
            // btnSaveOrg
            // 
            this.btnSaveOrg.Font = new System.Drawing.Font("宋体", 10F);
            this.btnSaveOrg.Location = new System.Drawing.Point(589, 59);
            this.btnSaveOrg.Name = "btnSaveOrg";
            this.btnSaveOrg.Size = new System.Drawing.Size(126, 29);
            this.btnSaveOrg.TabIndex = 23;
            this.btnSaveOrg.Text = "保存";
            this.btnSaveOrg.UseVisualStyleBackColor = true;
            this.btnSaveOrg.Click += new System.EventHandler(this.btnSaveOrg_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 411);
            this.Controls.Add(this.btnSaveOrg);
            this.Controls.Add(this.lbChosen);
            this.Controls.Add(this.c1Button2);
            this.Controls.Add(this.c1Button1);
            this.Controls.Add(this.lbRemain);
            this.Controls.Add(this.lblDeviceType);
            this.Controls.Add(this.lblDataCount);
            this.Controls.Add(this.rbDB);
            this.Controls.Add(this.rbDevice);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnGetData);
            this.Controls.Add(this.btnConnStr);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "信息采集";
            this.VisualStyleHolder = C1.Win.C1Ribbon.VisualStyle.Office2010Blue;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnCodeQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGetData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnConnStr)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Button1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1Button2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveOrg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbQueryCode;
        private C1.Win.C1Input.C1Button btnCodeQuery;
        private C1.Win.C1Input.C1Button btnConnStr;
        private C1.Win.C1Input.C1Button btnGetData;
        private C1.Win.C1Input.C1Button btnUpdate;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbOrgId;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbFaceTemplateV10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbRightTemplateV10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbLeftTemplateV10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbFACETEMPLATE;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbRIGHTTEMPLATE;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbLEFTTEMPLATE;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbEnrollId;
        private C1.Win.C1Input.C1Button btnSaveUser;
        private System.Windows.Forms.RadioButton rbDevice;
        private System.Windows.Forms.RadioButton rbDB;
        private System.Windows.Forms.Label lblDataCount;
        private System.Windows.Forms.Label lblDeviceType;
        private System.Windows.Forms.ListBox lbRemain;
        private C1.Win.C1Input.C1Button c1Button1;
        private C1.Win.C1Input.C1Button c1Button2;
        private System.Windows.Forms.ListBox lbChosen;
        private C1.Win.C1Input.C1Button btnSaveOrg;
    }
}