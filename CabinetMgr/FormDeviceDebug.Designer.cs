namespace CabinetMgr
{
    partial class FormDeviceDebug
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDeviceDebug));
            this.btnOpenGreen = new System.Windows.Forms.Button();
            this.btnOpenYellow = new System.Windows.Forms.Button();
            this.btnOpenRed = new System.Windows.Forms.Button();
            this.btnCloseRed = new System.Windows.Forms.Button();
            this.btnCloseYellow = new System.Windows.Forms.Button();
            this.btnCloseGreen = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbLedNo = new System.Windows.Forms.TextBox();
            this.btnCloseSingleLed = new System.Windows.Forms.Button();
            this.btnOpenSingleLed = new System.Windows.Forms.Button();
            this.btnCloseToolLed = new System.Windows.Forms.Button();
            this.btnOpenToolLed = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClearUser = new System.Windows.Forms.Button();
            this.btnExposeConfig = new System.Windows.Forms.Button();
            this.btnUploadIfaceUser = new System.Windows.Forms.Button();
            this.cbLockDrawerCode = new System.Windows.Forms.ComboBox();
            this.cbUnlockDrawerCode = new System.Windows.Forms.ComboBox();
            this.btnLockSingleDrawer = new System.Windows.Forms.Button();
            this.btnUnlockSingleDrawer = new System.Windows.Forms.Button();
            this.btnLockDoor = new System.Windows.Forms.Button();
            this.btnUnlockDoor = new System.Windows.Forms.Button();
            this.btnLockDrawer = new System.Windows.Forms.Button();
            this.btnUnlockDrawer = new System.Windows.Forms.Button();
            this.btnUploadUser = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOpenGreen
            // 
            this.btnOpenGreen.Location = new System.Drawing.Point(20, 39);
            this.btnOpenGreen.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOpenGreen.Name = "btnOpenGreen";
            this.btnOpenGreen.Size = new System.Drawing.Size(100, 29);
            this.btnOpenGreen.TabIndex = 0;
            this.btnOpenGreen.Text = "绿灯开";
            this.btnOpenGreen.UseVisualStyleBackColor = true;
            // 
            // btnOpenYellow
            // 
            this.btnOpenYellow.Location = new System.Drawing.Point(148, 39);
            this.btnOpenYellow.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOpenYellow.Name = "btnOpenYellow";
            this.btnOpenYellow.Size = new System.Drawing.Size(100, 29);
            this.btnOpenYellow.TabIndex = 1;
            this.btnOpenYellow.Text = "黄灯开";
            this.btnOpenYellow.UseVisualStyleBackColor = true;
            this.btnOpenYellow.Click += new System.EventHandler(this.btnOpenYellow_Click);
            // 
            // btnOpenRed
            // 
            this.btnOpenRed.Location = new System.Drawing.Point(276, 39);
            this.btnOpenRed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOpenRed.Name = "btnOpenRed";
            this.btnOpenRed.Size = new System.Drawing.Size(100, 29);
            this.btnOpenRed.TabIndex = 2;
            this.btnOpenRed.Text = "红灯开";
            this.btnOpenRed.UseVisualStyleBackColor = true;
            this.btnOpenRed.Click += new System.EventHandler(this.btnOpenRed_Click);
            // 
            // btnCloseRed
            // 
            this.btnCloseRed.Location = new System.Drawing.Point(276, 88);
            this.btnCloseRed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCloseRed.Name = "btnCloseRed";
            this.btnCloseRed.Size = new System.Drawing.Size(100, 29);
            this.btnCloseRed.TabIndex = 5;
            this.btnCloseRed.Text = "红灯关";
            this.btnCloseRed.UseVisualStyleBackColor = true;
            this.btnCloseRed.Click += new System.EventHandler(this.btnCloseRed_Click);
            // 
            // btnCloseYellow
            // 
            this.btnCloseYellow.Location = new System.Drawing.Point(148, 88);
            this.btnCloseYellow.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCloseYellow.Name = "btnCloseYellow";
            this.btnCloseYellow.Size = new System.Drawing.Size(100, 29);
            this.btnCloseYellow.TabIndex = 4;
            this.btnCloseYellow.Text = "黄灯关";
            this.btnCloseYellow.UseVisualStyleBackColor = true;
            this.btnCloseYellow.Click += new System.EventHandler(this.btnCloseYellow_Click);
            // 
            // btnCloseGreen
            // 
            this.btnCloseGreen.Location = new System.Drawing.Point(20, 88);
            this.btnCloseGreen.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCloseGreen.Name = "btnCloseGreen";
            this.btnCloseGreen.Size = new System.Drawing.Size(100, 29);
            this.btnCloseGreen.TabIndex = 3;
            this.btnCloseGreen.Text = "绿灯关";
            this.btnCloseGreen.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbLedNo);
            this.groupBox1.Controls.Add(this.btnCloseSingleLed);
            this.groupBox1.Controls.Add(this.btnOpenSingleLed);
            this.groupBox1.Controls.Add(this.btnCloseToolLed);
            this.groupBox1.Controls.Add(this.btnOpenToolLed);
            this.groupBox1.Controls.Add(this.btnOpenGreen);
            this.groupBox1.Controls.Add(this.btnCloseRed);
            this.groupBox1.Controls.Add(this.btnOpenYellow);
            this.groupBox1.Controls.Add(this.btnCloseYellow);
            this.groupBox1.Controls.Add(this.btnOpenRed);
            this.groupBox1.Controls.Add(this.btnCloseGreen);
            this.groupBox1.Location = new System.Drawing.Point(16, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(785, 144);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "LED测试";
            // 
            // tbLedNo
            // 
            this.tbLedNo.Location = new System.Drawing.Point(640, 40);
            this.tbLedNo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbLedNo.Name = "tbLedNo";
            this.tbLedNo.Size = new System.Drawing.Size(132, 25);
            this.tbLedNo.TabIndex = 10;
            // 
            // btnCloseSingleLed
            // 
            this.btnCloseSingleLed.Location = new System.Drawing.Point(532, 88);
            this.btnCloseSingleLed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCloseSingleLed.Name = "btnCloseSingleLed";
            this.btnCloseSingleLed.Size = new System.Drawing.Size(100, 29);
            this.btnCloseSingleLed.TabIndex = 9;
            this.btnCloseSingleLed.Text = "关单灯";
            this.btnCloseSingleLed.UseVisualStyleBackColor = true;
            this.btnCloseSingleLed.Click += new System.EventHandler(this.btnCloseSingleLed_Click);
            // 
            // btnOpenSingleLed
            // 
            this.btnOpenSingleLed.Location = new System.Drawing.Point(532, 39);
            this.btnOpenSingleLed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOpenSingleLed.Name = "btnOpenSingleLed";
            this.btnOpenSingleLed.Size = new System.Drawing.Size(100, 29);
            this.btnOpenSingleLed.TabIndex = 8;
            this.btnOpenSingleLed.Text = "开单灯";
            this.btnOpenSingleLed.UseVisualStyleBackColor = true;
            this.btnOpenSingleLed.Click += new System.EventHandler(this.btnOpenSingleLed_Click);
            // 
            // btnCloseToolLed
            // 
            this.btnCloseToolLed.Location = new System.Drawing.Point(407, 88);
            this.btnCloseToolLed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCloseToolLed.Name = "btnCloseToolLed";
            this.btnCloseToolLed.Size = new System.Drawing.Size(100, 29);
            this.btnCloseToolLed.TabIndex = 7;
            this.btnCloseToolLed.Text = "工具灯关";
            this.btnCloseToolLed.UseVisualStyleBackColor = true;
            this.btnCloseToolLed.Click += new System.EventHandler(this.btnCloseToolLed_Click);
            // 
            // btnOpenToolLed
            // 
            this.btnOpenToolLed.Location = new System.Drawing.Point(407, 39);
            this.btnOpenToolLed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOpenToolLed.Name = "btnOpenToolLed";
            this.btnOpenToolLed.Size = new System.Drawing.Size(100, 29);
            this.btnOpenToolLed.TabIndex = 6;
            this.btnOpenToolLed.Text = "工具灯开";
            this.btnOpenToolLed.UseVisualStyleBackColor = true;
            this.btnOpenToolLed.Click += new System.EventHandler(this.btnOpenToolLed_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnUploadUser);
            this.groupBox2.Controls.Add(this.btnClearUser);
            this.groupBox2.Controls.Add(this.btnExposeConfig);
            this.groupBox2.Controls.Add(this.btnUploadIfaceUser);
            this.groupBox2.Controls.Add(this.cbLockDrawerCode);
            this.groupBox2.Controls.Add(this.cbUnlockDrawerCode);
            this.groupBox2.Controls.Add(this.btnLockSingleDrawer);
            this.groupBox2.Controls.Add(this.btnUnlockSingleDrawer);
            this.groupBox2.Controls.Add(this.btnLockDoor);
            this.groupBox2.Controls.Add(this.btnUnlockDoor);
            this.groupBox2.Controls.Add(this.btnLockDrawer);
            this.groupBox2.Controls.Add(this.btnUnlockDrawer);
            this.groupBox2.Location = new System.Drawing.Point(16, 166);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(785, 266);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "柜体测试";
            // 
            // btnClearUser
            // 
            this.btnClearUser.Location = new System.Drawing.Point(532, 42);
            this.btnClearUser.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClearUser.Name = "btnClearUser";
            this.btnClearUser.Size = new System.Drawing.Size(100, 29);
            this.btnClearUser.TabIndex = 10;
            this.btnClearUser.Text = "清除人员";
            this.btnClearUser.UseVisualStyleBackColor = true;
            this.btnClearUser.Click += new System.EventHandler(this.btnClearUser_Click);
            // 
            // btnExposeConfig
            // 
            this.btnExposeConfig.Location = new System.Drawing.Point(276, 42);
            this.btnExposeConfig.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExposeConfig.Name = "btnExposeConfig";
            this.btnExposeConfig.Size = new System.Drawing.Size(100, 29);
            this.btnExposeConfig.TabIndex = 9;
            this.btnExposeConfig.Text = "配置生成";
            this.btnExposeConfig.UseVisualStyleBackColor = true;
            this.btnExposeConfig.Click += new System.EventHandler(this.btnExposeConfig_Click);
            // 
            // btnUploadIfaceUser
            // 
            this.btnUploadIfaceUser.Location = new System.Drawing.Point(532, 92);
            this.btnUploadIfaceUser.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnUploadIfaceUser.Name = "btnUploadIfaceUser";
            this.btnUploadIfaceUser.Size = new System.Drawing.Size(100, 29);
            this.btnUploadIfaceUser.TabIndex = 8;
            this.btnUploadIfaceUser.Text = "下发人员";
            this.btnUploadIfaceUser.UseVisualStyleBackColor = true;
            this.btnUploadIfaceUser.Click += new System.EventHandler(this.btnUploadIfaceUser_Click);
            // 
            // cbLockDrawerCode
            // 
            this.cbLockDrawerCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLockDrawerCode.FormattingEnabled = true;
            this.cbLockDrawerCode.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.cbLockDrawerCode.Location = new System.Drawing.Point(148, 208);
            this.cbLockDrawerCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbLockDrawerCode.Name = "cbLockDrawerCode";
            this.cbLockDrawerCode.Size = new System.Drawing.Size(99, 23);
            this.cbLockDrawerCode.TabIndex = 7;
            // 
            // cbUnlockDrawerCode
            // 
            this.cbUnlockDrawerCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUnlockDrawerCode.FormattingEnabled = true;
            this.cbUnlockDrawerCode.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.cbUnlockDrawerCode.Location = new System.Drawing.Point(148, 160);
            this.cbUnlockDrawerCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbUnlockDrawerCode.Name = "cbUnlockDrawerCode";
            this.cbUnlockDrawerCode.Size = new System.Drawing.Size(99, 23);
            this.cbUnlockDrawerCode.TabIndex = 6;
            // 
            // btnLockSingleDrawer
            // 
            this.btnLockSingleDrawer.Location = new System.Drawing.Point(20, 205);
            this.btnLockSingleDrawer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLockSingleDrawer.Name = "btnLockSingleDrawer";
            this.btnLockSingleDrawer.Size = new System.Drawing.Size(100, 29);
            this.btnLockSingleDrawer.TabIndex = 5;
            this.btnLockSingleDrawer.Text = "锁定抽屉";
            this.btnLockSingleDrawer.UseVisualStyleBackColor = true;
            this.btnLockSingleDrawer.Click += new System.EventHandler(this.btnLockSingleDrawer_Click);
            // 
            // btnUnlockSingleDrawer
            // 
            this.btnUnlockSingleDrawer.Location = new System.Drawing.Point(20, 158);
            this.btnUnlockSingleDrawer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnUnlockSingleDrawer.Name = "btnUnlockSingleDrawer";
            this.btnUnlockSingleDrawer.Size = new System.Drawing.Size(100, 29);
            this.btnUnlockSingleDrawer.TabIndex = 4;
            this.btnUnlockSingleDrawer.Text = "解锁抽屉";
            this.btnUnlockSingleDrawer.UseVisualStyleBackColor = true;
            this.btnUnlockSingleDrawer.Click += new System.EventHandler(this.btnUnlockSingleDrawer_Click);
            // 
            // btnLockDoor
            // 
            this.btnLockDoor.Location = new System.Drawing.Point(148, 42);
            this.btnLockDoor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLockDoor.Name = "btnLockDoor";
            this.btnLockDoor.Size = new System.Drawing.Size(100, 29);
            this.btnLockDoor.TabIndex = 3;
            this.btnLockDoor.Text = "锁定柜门";
            this.btnLockDoor.UseVisualStyleBackColor = true;
            this.btnLockDoor.Click += new System.EventHandler(this.btnLockDoor_Click);
            // 
            // btnUnlockDoor
            // 
            this.btnUnlockDoor.Location = new System.Drawing.Point(20, 42);
            this.btnUnlockDoor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnUnlockDoor.Name = "btnUnlockDoor";
            this.btnUnlockDoor.Size = new System.Drawing.Size(100, 29);
            this.btnUnlockDoor.TabIndex = 2;
            this.btnUnlockDoor.Text = "解锁柜门";
            this.btnUnlockDoor.UseVisualStyleBackColor = true;
            this.btnUnlockDoor.Click += new System.EventHandler(this.btnUnlockDoor_Click);
            // 
            // btnLockDrawer
            // 
            this.btnLockDrawer.Location = new System.Drawing.Point(148, 92);
            this.btnLockDrawer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLockDrawer.Name = "btnLockDrawer";
            this.btnLockDrawer.Size = new System.Drawing.Size(100, 29);
            this.btnLockDrawer.TabIndex = 1;
            this.btnLockDrawer.Text = "锁定抽屉";
            this.btnLockDrawer.UseVisualStyleBackColor = true;
            this.btnLockDrawer.Click += new System.EventHandler(this.btnLockDrawer_Click);
            // 
            // btnUnlockDrawer
            // 
            this.btnUnlockDrawer.Location = new System.Drawing.Point(20, 92);
            this.btnUnlockDrawer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnUnlockDrawer.Name = "btnUnlockDrawer";
            this.btnUnlockDrawer.Size = new System.Drawing.Size(100, 29);
            this.btnUnlockDrawer.TabIndex = 0;
            this.btnUnlockDrawer.Text = "解锁抽屉";
            this.btnUnlockDrawer.UseVisualStyleBackColor = true;
            this.btnUnlockDrawer.Click += new System.EventHandler(this.btnUnlockDrawer_Click);
            // 
            // btnUploadUser
            // 
            this.btnUploadUser.Location = new System.Drawing.Point(532, 145);
            this.btnUploadUser.Margin = new System.Windows.Forms.Padding(4);
            this.btnUploadUser.Name = "btnUploadUser";
            this.btnUploadUser.Size = new System.Drawing.Size(100, 29);
            this.btnUploadUser.TabIndex = 11;
            this.btnUploadUser.Text = "上传人员";
            this.btnUploadUser.UseVisualStyleBackColor = true;
            this.btnUploadUser.Click += new System.EventHandler(this.btnUploadUser_Click);
            // 
            // FormDeviceDebug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 448);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormDeviceDebug";
            this.Text = "设备测试";
            this.Load += new System.EventHandler(this.FormDeviceDebug_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOpenGreen;
        private System.Windows.Forms.Button btnOpenYellow;
        private System.Windows.Forms.Button btnOpenRed;
        private System.Windows.Forms.Button btnCloseRed;
        private System.Windows.Forms.Button btnCloseYellow;
        private System.Windows.Forms.Button btnCloseGreen;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnLockDoor;
        private System.Windows.Forms.Button btnUnlockDoor;
        private System.Windows.Forms.Button btnLockDrawer;
        private System.Windows.Forms.Button btnUnlockDrawer;
        private System.Windows.Forms.Button btnLockSingleDrawer;
        private System.Windows.Forms.Button btnUnlockSingleDrawer;
        private System.Windows.Forms.ComboBox cbLockDrawerCode;
        private System.Windows.Forms.ComboBox cbUnlockDrawerCode;
        private System.Windows.Forms.Button btnCloseToolLed;
        private System.Windows.Forms.Button btnOpenToolLed;
        private System.Windows.Forms.TextBox tbLedNo;
        private System.Windows.Forms.Button btnCloseSingleLed;
        private System.Windows.Forms.Button btnOpenSingleLed;
        private System.Windows.Forms.Button btnUploadIfaceUser;
        private System.Windows.Forms.Button btnExposeConfig;
        private System.Windows.Forms.Button btnClearUser;
        private System.Windows.Forms.Button btnUploadUser;
    }
}