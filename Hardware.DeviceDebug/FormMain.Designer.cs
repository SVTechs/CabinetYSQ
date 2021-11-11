namespace Hardware.DeviceDebug
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCloseRed = new System.Windows.Forms.Button();
            this.btnOpenYellow = new System.Windows.Forms.Button();
            this.btnCloseYellow = new System.Windows.Forms.Button();
            this.btnOpenRed = new System.Windows.Forms.Button();
            this.btnOpenLed = new System.Windows.Forms.Button();
            this.btnCloseLed = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnLockDoor = new System.Windows.Forms.Button();
            this.btnUnlockDoor = new System.Windows.Forms.Button();
            this.btnLockDrawer = new System.Windows.Forms.Button();
            this.btnUnlockDrawer = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbLedCount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOpenLight = new System.Windows.Forms.Button();
            this.btnCloseLight = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCloseRed);
            this.groupBox1.Controls.Add(this.btnOpenYellow);
            this.groupBox1.Controls.Add(this.btnCloseYellow);
            this.groupBox1.Controls.Add(this.btnOpenRed);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 115);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "报警灯测试";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // btnCloseRed
            // 
            this.btnCloseRed.Location = new System.Drawing.Point(111, 70);
            this.btnCloseRed.Name = "btnCloseRed";
            this.btnCloseRed.Size = new System.Drawing.Size(75, 23);
            this.btnCloseRed.TabIndex = 5;
            this.btnCloseRed.Text = "红灯关";
            this.btnCloseRed.UseVisualStyleBackColor = true;
            this.btnCloseRed.Click += new System.EventHandler(this.btnCloseRed_Click);
            // 
            // btnOpenYellow
            // 
            this.btnOpenYellow.Location = new System.Drawing.Point(15, 31);
            this.btnOpenYellow.Name = "btnOpenYellow";
            this.btnOpenYellow.Size = new System.Drawing.Size(75, 23);
            this.btnOpenYellow.TabIndex = 1;
            this.btnOpenYellow.Text = "黄灯开";
            this.btnOpenYellow.UseVisualStyleBackColor = true;
            this.btnOpenYellow.Click += new System.EventHandler(this.btnOpenYellow_Click);
            // 
            // btnCloseYellow
            // 
            this.btnCloseYellow.Location = new System.Drawing.Point(15, 70);
            this.btnCloseYellow.Name = "btnCloseYellow";
            this.btnCloseYellow.Size = new System.Drawing.Size(75, 23);
            this.btnCloseYellow.TabIndex = 4;
            this.btnCloseYellow.Text = "黄灯关";
            this.btnCloseYellow.UseVisualStyleBackColor = true;
            this.btnCloseYellow.Click += new System.EventHandler(this.btnCloseYellow_Click);
            // 
            // btnOpenRed
            // 
            this.btnOpenRed.Location = new System.Drawing.Point(111, 31);
            this.btnOpenRed.Name = "btnOpenRed";
            this.btnOpenRed.Size = new System.Drawing.Size(75, 23);
            this.btnOpenRed.TabIndex = 2;
            this.btnOpenRed.Text = "红灯开";
            this.btnOpenRed.UseVisualStyleBackColor = true;
            this.btnOpenRed.Click += new System.EventHandler(this.btnOpenRed_Click);
            // 
            // btnOpenLed
            // 
            this.btnOpenLed.Location = new System.Drawing.Point(40, 70);
            this.btnOpenLed.Name = "btnOpenLed";
            this.btnOpenLed.Size = new System.Drawing.Size(75, 23);
            this.btnOpenLed.TabIndex = 0;
            this.btnOpenLed.Text = "工具灯开";
            this.btnOpenLed.UseVisualStyleBackColor = true;
            this.btnOpenLed.Click += new System.EventHandler(this.btnOpenLed_Click);
            // 
            // btnCloseLed
            // 
            this.btnCloseLed.Location = new System.Drawing.Point(121, 70);
            this.btnCloseLed.Name = "btnCloseLed";
            this.btnCloseLed.Size = new System.Drawing.Size(75, 23);
            this.btnCloseLed.TabIndex = 3;
            this.btnCloseLed.Text = "工具灯关";
            this.btnCloseLed.UseVisualStyleBackColor = true;
            this.btnCloseLed.Click += new System.EventHandler(this.btnCloseLed_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCloseLight);
            this.groupBox2.Controls.Add(this.btnOpenLight);
            this.groupBox2.Controls.Add(this.btnLockDoor);
            this.groupBox2.Controls.Add(this.btnUnlockDoor);
            this.groupBox2.Controls.Add(this.btnLockDrawer);
            this.groupBox2.Controls.Add(this.btnUnlockDrawer);
            this.groupBox2.Location = new System.Drawing.Point(12, 133);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(296, 124);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "柜体测试";
            // 
            // btnLockDoor
            // 
            this.btnLockDoor.Location = new System.Drawing.Point(111, 34);
            this.btnLockDoor.Name = "btnLockDoor";
            this.btnLockDoor.Size = new System.Drawing.Size(75, 23);
            this.btnLockDoor.TabIndex = 3;
            this.btnLockDoor.Text = "锁定柜门";
            this.btnLockDoor.UseVisualStyleBackColor = true;
            this.btnLockDoor.Click += new System.EventHandler(this.btnLockDoor_Click);
            // 
            // btnUnlockDoor
            // 
            this.btnUnlockDoor.Location = new System.Drawing.Point(15, 34);
            this.btnUnlockDoor.Name = "btnUnlockDoor";
            this.btnUnlockDoor.Size = new System.Drawing.Size(75, 23);
            this.btnUnlockDoor.TabIndex = 2;
            this.btnUnlockDoor.Text = "解锁柜门";
            this.btnUnlockDoor.UseVisualStyleBackColor = true;
            this.btnUnlockDoor.Click += new System.EventHandler(this.btnUnlockDoor_Click);
            // 
            // btnLockDrawer
            // 
            this.btnLockDrawer.Location = new System.Drawing.Point(111, 74);
            this.btnLockDrawer.Name = "btnLockDrawer";
            this.btnLockDrawer.Size = new System.Drawing.Size(75, 23);
            this.btnLockDrawer.TabIndex = 1;
            this.btnLockDrawer.Text = "锁定抽屉";
            this.btnLockDrawer.UseVisualStyleBackColor = true;
            this.btnLockDrawer.Click += new System.EventHandler(this.btnLockDrawer_Click);
            // 
            // btnUnlockDrawer
            // 
            this.btnUnlockDrawer.Location = new System.Drawing.Point(15, 74);
            this.btnUnlockDrawer.Name = "btnUnlockDrawer";
            this.btnUnlockDrawer.Size = new System.Drawing.Size(75, 23);
            this.btnUnlockDrawer.TabIndex = 0;
            this.btnUnlockDrawer.Text = "解锁抽屉";
            this.btnUnlockDrawer.UseVisualStyleBackColor = true;
            this.btnUnlockDrawer.Click += new System.EventHandler(this.btnUnlockDrawer_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tbLedCount);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.btnOpenLed);
            this.groupBox3.Controls.Add(this.btnCloseLed);
            this.groupBox3.Location = new System.Drawing.Point(314, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(344, 115);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "工具灯测试";
            // 
            // tbLedCount
            // 
            this.tbLedCount.Location = new System.Drawing.Point(95, 31);
            this.tbLedCount.Name = "tbLedCount";
            this.tbLedCount.Size = new System.Drawing.Size(100, 21);
            this.tbLedCount.TabIndex = 5;
            this.tbLedCount.Text = "40";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "灯数量";
            // 
            // btnOpenLight
            // 
            this.btnOpenLight.Location = new System.Drawing.Point(206, 34);
            this.btnOpenLight.Name = "btnOpenLight";
            this.btnOpenLight.Size = new System.Drawing.Size(75, 23);
            this.btnOpenLight.TabIndex = 4;
            this.btnOpenLight.Text = "开照明灯";
            this.btnOpenLight.UseVisualStyleBackColor = true;
            this.btnOpenLight.Click += new System.EventHandler(this.btnOpenLight_Click);
            // 
            // btnCloseLight
            // 
            this.btnCloseLight.Location = new System.Drawing.Point(206, 74);
            this.btnCloseLight.Name = "btnCloseLight";
            this.btnCloseLight.Size = new System.Drawing.Size(75, 23);
            this.btnCloseLight.TabIndex = 5;
            this.btnCloseLight.Text = "关照明灯";
            this.btnCloseLight.UseVisualStyleBackColor = true;
            this.btnCloseLight.Click += new System.EventHandler(this.btnCloseLight_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 298);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "设备测试";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOpenLed;
        private System.Windows.Forms.Button btnCloseRed;
        private System.Windows.Forms.Button btnOpenYellow;
        private System.Windows.Forms.Button btnCloseYellow;
        private System.Windows.Forms.Button btnOpenRed;
        private System.Windows.Forms.Button btnCloseLed;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnUnlockDrawer;
        private System.Windows.Forms.Button btnLockDrawer;
        private System.Windows.Forms.Button btnLockDoor;
        private System.Windows.Forms.Button btnUnlockDoor;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbLedCount;
        private System.Windows.Forms.Button btnOpenLight;
        private System.Windows.Forms.Button btnCloseLight;
    }
}

