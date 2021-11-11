namespace UpdateControl
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.cb_upd_unit = new System.Windows.Forms.ComboBox();
            this.cb_upd_app = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_SelfUpload = new System.Windows.Forms.CheckBox();
            this.cb_IncludeDir = new System.Windows.Forms.CheckBox();
            this.m_UpdProgress = new System.Windows.Forms.ProgressBar();
            this.cb_IncludeINI = new System.Windows.Forms.CheckBox();
            this.btn_Browse = new System.Windows.Forms.Button();
            this.tb_AppPath = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_upd_log = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_upd_ver = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_Update = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_Add = new System.Windows.Forms.Button();
            this.tb_new_app = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_new_unit = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cb_upd_unit
            // 
            this.cb_upd_unit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_upd_unit.FormattingEnabled = true;
            this.cb_upd_unit.Location = new System.Drawing.Point(76, 40);
            this.cb_upd_unit.Name = "cb_upd_unit";
            this.cb_upd_unit.Size = new System.Drawing.Size(198, 20);
            this.cb_upd_unit.TabIndex = 0;
            this.cb_upd_unit.SelectedIndexChanged += new System.EventHandler(this.cb_upd_unit_SelectedIndexChanged);
            // 
            // cb_upd_app
            // 
            this.cb_upd_app.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_upd_app.FormattingEnabled = true;
            this.cb_upd_app.Location = new System.Drawing.Point(337, 40);
            this.cb_upd_app.Name = "cb_upd_app";
            this.cb_upd_app.Size = new System.Drawing.Size(220, 20);
            this.cb_upd_app.TabIndex = 1;
            this.cb_upd_app.SelectedIndexChanged += new System.EventHandler(this.cb_upd_app_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "单位";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(292, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "程序";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_SelfUpload);
            this.groupBox1.Controls.Add(this.cb_IncludeDir);
            this.groupBox1.Controls.Add(this.m_UpdProgress);
            this.groupBox1.Controls.Add(this.cb_IncludeINI);
            this.groupBox1.Controls.Add(this.btn_Browse);
            this.groupBox1.Controls.Add(this.tb_AppPath);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.tb_upd_log);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.tb_upd_ver);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.btn_Update);
            this.groupBox1.Controls.Add(this.cb_upd_unit);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cb_upd_app);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(577, 267);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "快速更新";
            // 
            // cb_SelfUpload
            // 
            this.cb_SelfUpload.AutoSize = true;
            this.cb_SelfUpload.Location = new System.Drawing.Point(324, 234);
            this.cb_SelfUpload.Name = "cb_SelfUpload";
            this.cb_SelfUpload.Size = new System.Drawing.Size(84, 16);
            this.cb_SelfUpload.TabIndex = 14;
            this.cb_SelfUpload.Text = "已手动上传";
            this.cb_SelfUpload.UseVisualStyleBackColor = true;
            // 
            // cb_IncludeDir
            // 
            this.cb_IncludeDir.AutoSize = true;
            this.cb_IncludeDir.Location = new System.Drawing.Point(234, 234);
            this.cb_IncludeDir.Name = "cb_IncludeDir";
            this.cb_IncludeDir.Size = new System.Drawing.Size(84, 16);
            this.cb_IncludeDir.TabIndex = 13;
            this.cb_IncludeDir.Text = "包含文件夹";
            this.cb_IncludeDir.UseVisualStyleBackColor = true;
            // 
            // m_UpdProgress
            // 
            this.m_UpdProgress.Location = new System.Drawing.Point(417, 230);
            this.m_UpdProgress.Name = "m_UpdProgress";
            this.m_UpdProgress.Size = new System.Drawing.Size(140, 23);
            this.m_UpdProgress.Step = 100;
            this.m_UpdProgress.TabIndex = 12;
            // 
            // cb_IncludeINI
            // 
            this.cb_IncludeINI.AutoSize = true;
            this.cb_IncludeINI.Location = new System.Drawing.Point(162, 234);
            this.cb_IncludeINI.Name = "cb_IncludeINI";
            this.cb_IncludeINI.Size = new System.Drawing.Size(66, 16);
            this.cb_IncludeINI.TabIndex = 11;
            this.cb_IncludeINI.Text = "包含INI";
            this.cb_IncludeINI.UseVisualStyleBackColor = true;
            // 
            // btn_Browse
            // 
            this.btn_Browse.Location = new System.Drawing.Point(482, 83);
            this.btn_Browse.Name = "btn_Browse";
            this.btn_Browse.Size = new System.Drawing.Size(75, 23);
            this.btn_Browse.TabIndex = 10;
            this.btn_Browse.Text = "浏览";
            this.btn_Browse.UseVisualStyleBackColor = true;
            this.btn_Browse.Click += new System.EventHandler(this.btn_Browse_Click);
            // 
            // tb_AppPath
            // 
            this.tb_AppPath.Location = new System.Drawing.Point(76, 84);
            this.tb_AppPath.Name = "tb_AppPath";
            this.tb_AppPath.Size = new System.Drawing.Size(388, 21);
            this.tb_AppPath.TabIndex = 8;
            this.tb_AppPath.TextChanged += new System.EventHandler(this.tb_AppPath_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 87);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 9;
            this.label7.Text = "主程序";
            // 
            // tb_upd_log
            // 
            this.tb_upd_log.Location = new System.Drawing.Point(76, 169);
            this.tb_upd_log.Multiline = true;
            this.tb_upd_log.Name = "tb_upd_log";
            this.tb_upd_log.Size = new System.Drawing.Size(481, 52);
            this.tb_upd_log.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 169);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "描述";
            // 
            // tb_upd_ver
            // 
            this.tb_upd_ver.Location = new System.Drawing.Point(76, 127);
            this.tb_upd_ver.Name = "tb_upd_ver";
            this.tb_upd_ver.Size = new System.Drawing.Size(481, 21);
            this.tb_upd_ver.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "版本号";
            // 
            // btn_Update
            // 
            this.btn_Update.Location = new System.Drawing.Point(76, 230);
            this.btn_Update.Name = "btn_Update";
            this.btn_Update.Size = new System.Drawing.Size(73, 22);
            this.btn_Update.TabIndex = 4;
            this.btn_Update.Text = "更新";
            this.btn_Update.UseVisualStyleBackColor = true;
            this.btn_Update.Click += new System.EventHandler(this.btn_Update_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_Add);
            this.groupBox2.Controls.Add(this.tb_new_app);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tb_new_unit);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(9, 285);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(580, 71);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "程序新增";
            // 
            // btn_Add
            // 
            this.btn_Add.Location = new System.Drawing.Point(485, 27);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(75, 23);
            this.btn_Add.TabIndex = 4;
            this.btn_Add.Text = "新增";
            this.btn_Add.UseVisualStyleBackColor = true;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // tb_new_app
            // 
            this.tb_new_app.Location = new System.Drawing.Point(297, 29);
            this.tb_new_app.Name = "tb_new_app";
            this.tb_new_app.Size = new System.Drawing.Size(178, 21);
            this.tb_new_app.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(259, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "程序";
            // 
            // tb_new_unit
            // 
            this.tb_new_unit.Location = new System.Drawing.Point(79, 29);
            this.tb_new_unit.Name = "tb_new_unit";
            this.tb_new_unit.Size = new System.Drawing.Size(167, 21);
            this.tb_new_unit.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "单位";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 368);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "升级控制";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_upd_unit;
        private System.Windows.Forms.ComboBox cb_upd_app;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_Update;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tb_new_app;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_new_unit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.TextBox tb_upd_ver;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_upd_log;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_AppPath;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_Browse;
        private System.Windows.Forms.CheckBox cb_IncludeINI;
        private System.Windows.Forms.ProgressBar m_UpdProgress;
        private System.Windows.Forms.CheckBox cb_IncludeDir;
        private System.Windows.Forms.CheckBox cb_SelfUpload;
    }
}

