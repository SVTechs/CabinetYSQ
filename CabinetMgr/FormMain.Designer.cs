namespace CabinetMgr
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.panelWindow = new System.Windows.Forms.Panel();
            this.tmUhfInit = new System.Windows.Forms.Timer(this.components);
            this.panelMenu = new System.Windows.Forms.Panel();
            this.btnAboutApp = new System.Windows.Forms.Button();
            this.btnIoRecord = new System.Windows.Forms.Button();
            this.btnToolReport = new System.Windows.Forms.Button();
            this.lbInitTime = new System.Windows.Forms.Label();
            this.btnToolCheck = new System.Windows.Forms.Button();
            this.btnToolPurchase = new System.Windows.Forms.Button();
            this.btnToolManage = new System.Windows.Forms.Button();
            this.btnToolPlan = new System.Windows.Forms.Button();
            this.panelTop = new System.Windows.Forms.Panel();
            this.pbAvatar = new System.Windows.Forms.PictureBox();
            this.lbUserName = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.AxRealSvrOcxTcp1 = new AxRealSvrOcxTcpLib.AxRealSvrOcxTcp();
            this.panelMenu.SuspendLayout();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAvatar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AxRealSvrOcxTcp1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelWindow
            // 
            this.panelWindow.BackColor = System.Drawing.SystemColors.Control;
            this.panelWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWindow.Location = new System.Drawing.Point(170, 155);
            this.panelWindow.Margin = new System.Windows.Forms.Padding(4);
            this.panelWindow.Name = "panelWindow";
            this.panelWindow.Size = new System.Drawing.Size(854, 613);
            this.panelWindow.TabIndex = 4;
            this.panelWindow.SizeChanged += new System.EventHandler(this.panelWindow_SizeChanged);
            // 
            // tmUhfInit
            // 
            this.tmUhfInit.Interval = 1000;
            this.tmUhfInit.Tick += new System.EventHandler(this.tmUhfInit_Tick);
            // 
            // panelMenu
            // 
            this.panelMenu.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelMenu.BackgroundImage")));
            this.panelMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelMenu.Controls.Add(this.btnAboutApp);
            this.panelMenu.Controls.Add(this.btnIoRecord);
            this.panelMenu.Controls.Add(this.btnToolReport);
            this.panelMenu.Controls.Add(this.lbInitTime);
            this.panelMenu.Controls.Add(this.btnToolCheck);
            this.panelMenu.Controls.Add(this.btnToolPurchase);
            this.panelMenu.Controls.Add(this.btnToolManage);
            this.panelMenu.Controls.Add(this.btnToolPlan);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMenu.Location = new System.Drawing.Point(0, 155);
            this.panelMenu.Margin = new System.Windows.Forms.Padding(4);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(170, 613);
            this.panelMenu.TabIndex = 3;
            // 
            // btnAboutApp
            // 
            this.btnAboutApp.BackgroundImage = global::CabinetMgr.Properties.Resources.leftbtn_about_n;
            this.btnAboutApp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAboutApp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAboutApp.FlatAppearance.BorderSize = 0;
            this.btnAboutApp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAboutApp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAboutApp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAboutApp.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAboutApp.Location = new System.Drawing.Point(13, 178);
            this.btnAboutApp.Margin = new System.Windows.Forms.Padding(4);
            this.btnAboutApp.Name = "btnAboutApp";
            this.btnAboutApp.Size = new System.Drawing.Size(143, 48);
            this.btnAboutApp.TabIndex = 249;
            this.btnAboutApp.Tag = "6";
            this.btnAboutApp.UseVisualStyleBackColor = true;
            this.btnAboutApp.Click += new System.EventHandler(this.btnAboutApp_Click);
            // 
            // btnIoRecord
            // 
            this.btnIoRecord.BackgroundImage = global::CabinetMgr.Properties.Resources.leftbtn_jhjl;
            this.btnIoRecord.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnIoRecord.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIoRecord.FlatAppearance.BorderSize = 0;
            this.btnIoRecord.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnIoRecord.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnIoRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIoRecord.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnIoRecord.Location = new System.Drawing.Point(13, 9);
            this.btnIoRecord.Margin = new System.Windows.Forms.Padding(4);
            this.btnIoRecord.Name = "btnIoRecord";
            this.btnIoRecord.Size = new System.Drawing.Size(143, 48);
            this.btnIoRecord.TabIndex = 248;
            this.btnIoRecord.Tag = "1";
            this.btnIoRecord.UseVisualStyleBackColor = true;
            this.btnIoRecord.Click += new System.EventHandler(this.btnIoRecord_Click);
            // 
            // btnToolReport
            // 
            this.btnToolReport.BackgroundImage = global::CabinetMgr.Properties.Resources.leftbtn_gjzt_n1;
            this.btnToolReport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnToolReport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToolReport.FlatAppearance.BorderSize = 0;
            this.btnToolReport.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnToolReport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnToolReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToolReport.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnToolReport.Location = new System.Drawing.Point(13, 66);
            this.btnToolReport.Margin = new System.Windows.Forms.Padding(4);
            this.btnToolReport.Name = "btnToolReport";
            this.btnToolReport.Size = new System.Drawing.Size(143, 48);
            this.btnToolReport.TabIndex = 247;
            this.btnToolReport.Tag = "4";
            this.btnToolReport.UseVisualStyleBackColor = true;
            this.btnToolReport.Click += new System.EventHandler(this.btnToolReport_Click);
            // 
            // lbInitTime
            // 
            this.lbInitTime.AutoSize = true;
            this.lbInitTime.BackColor = System.Drawing.Color.Transparent;
            this.lbInitTime.ForeColor = System.Drawing.SystemColors.Window;
            this.lbInitTime.Location = new System.Drawing.Point(10, 376);
            this.lbInitTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbInitTime.Name = "lbInitTime";
            this.lbInitTime.Size = new System.Drawing.Size(98, 15);
            this.lbInitTime.TabIndex = 245;
            this.lbInitTime.Text = "启动中,剩余:";
            // 
            // btnToolCheck
            // 
            this.btnToolCheck.BackgroundImage = global::CabinetMgr.Properties.Resources.leftbtn_gjjy;
            this.btnToolCheck.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnToolCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToolCheck.FlatAppearance.BorderSize = 0;
            this.btnToolCheck.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnToolCheck.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnToolCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToolCheck.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnToolCheck.Location = new System.Drawing.Point(13, 302);
            this.btnToolCheck.Margin = new System.Windows.Forms.Padding(4);
            this.btnToolCheck.Name = "btnToolCheck";
            this.btnToolCheck.Size = new System.Drawing.Size(143, 48);
            this.btnToolCheck.TabIndex = 243;
            this.btnToolCheck.Tag = "3";
            this.btnToolCheck.UseVisualStyleBackColor = true;
            this.btnToolCheck.Visible = false;
            this.btnToolCheck.Click += new System.EventHandler(this.btnToolCheck_Click);
            // 
            // btnToolPurchase
            // 
            this.btnToolPurchase.BackgroundImage = global::CabinetMgr.Properties.Resources.leftbtn_gjsl;
            this.btnToolPurchase.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnToolPurchase.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToolPurchase.FlatAppearance.BorderSize = 0;
            this.btnToolPurchase.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnToolPurchase.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnToolPurchase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToolPurchase.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnToolPurchase.Location = new System.Drawing.Point(35, 812);
            this.btnToolPurchase.Margin = new System.Windows.Forms.Padding(4);
            this.btnToolPurchase.Name = "btnToolPurchase";
            this.btnToolPurchase.Size = new System.Drawing.Size(143, 48);
            this.btnToolPurchase.TabIndex = 242;
            this.btnToolPurchase.Tag = "2";
            this.btnToolPurchase.UseVisualStyleBackColor = true;
            this.btnToolPurchase.Visible = false;
            this.btnToolPurchase.Click += new System.EventHandler(this.btnToolPurchase_Click);
            // 
            // btnToolManage
            // 
            this.btnToolManage.BackgroundImage = global::CabinetMgr.Properties.Resources.leftbtn_tzgl;
            this.btnToolManage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnToolManage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToolManage.FlatAppearance.BorderSize = 0;
            this.btnToolManage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnToolManage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnToolManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToolManage.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnToolManage.Location = new System.Drawing.Point(13, 122);
            this.btnToolManage.Margin = new System.Windows.Forms.Padding(4);
            this.btnToolManage.Name = "btnToolManage";
            this.btnToolManage.Size = new System.Drawing.Size(143, 48);
            this.btnToolManage.TabIndex = 239;
            this.btnToolManage.Tag = "5";
            this.btnToolManage.UseVisualStyleBackColor = true;
            this.btnToolManage.Click += new System.EventHandler(this.btnToolManage_Click);
            // 
            // btnToolPlan
            // 
            this.btnToolPlan.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnToolPlan.BackgroundImage")));
            this.btnToolPlan.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnToolPlan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToolPlan.FlatAppearance.BorderSize = 0;
            this.btnToolPlan.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnToolPlan.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnToolPlan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToolPlan.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnToolPlan.Location = new System.Drawing.Point(13, 246);
            this.btnToolPlan.Margin = new System.Windows.Forms.Padding(4);
            this.btnToolPlan.Name = "btnToolPlan";
            this.btnToolPlan.Size = new System.Drawing.Size(143, 48);
            this.btnToolPlan.TabIndex = 238;
            this.btnToolPlan.Tag = "0";
            this.btnToolPlan.UseVisualStyleBackColor = true;
            this.btnToolPlan.Visible = false;
            this.btnToolPlan.Click += new System.EventHandler(this.btnToolPlan_Click);
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.SystemColors.Control;
            this.panelTop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelTop.BackgroundImage")));
            this.panelTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelTop.Controls.Add(this.pbAvatar);
            this.panelTop.Controls.Add(this.lbUserName);
            this.panelTop.Controls.Add(this.pictureBox2);
            this.panelTop.Controls.Add(this.pictureBox1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(4);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1024, 155);
            this.panelTop.TabIndex = 2;
            // 
            // pbAvatar
            // 
            this.pbAvatar.BackColor = System.Drawing.Color.Transparent;
            this.pbAvatar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbAvatar.BackgroundImage")));
            this.pbAvatar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbAvatar.Location = new System.Drawing.Point(748, 84);
            this.pbAvatar.Margin = new System.Windows.Forms.Padding(4);
            this.pbAvatar.Name = "pbAvatar";
            this.pbAvatar.Size = new System.Drawing.Size(43, 50);
            this.pbAvatar.TabIndex = 3;
            this.pbAvatar.TabStop = false;
            this.pbAvatar.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pbAvatar_MouseDoubleClick);
            // 
            // lbUserName
            // 
            this.lbUserName.AutoSize = true;
            this.lbUserName.BackColor = System.Drawing.Color.Transparent;
            this.lbUserName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbUserName.Font = new System.Drawing.Font("微软雅黑", 15F);
            this.lbUserName.ForeColor = System.Drawing.Color.White;
            this.lbUserName.Location = new System.Drawing.Point(795, 93);
            this.lbUserName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbUserName.Name = "lbUserName";
            this.lbUserName.Size = new System.Drawing.Size(142, 34);
            this.lbUserName.TabIndex = 2;
            this.lbUserName.Text = "当前用户：";
            this.lbUserName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbUserName.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbUserName_MouseDoubleClick);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.BackgroundImage")));
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(245, 20);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(475, 86);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Location = new System.Drawing.Point(15, 20);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(292, 68);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // AxRealSvrOcxTcp1
            // 
            this.AxRealSvrOcxTcp1.Enabled = true;
            this.AxRealSvrOcxTcp1.Location = new System.Drawing.Point(659, 50);
            this.AxRealSvrOcxTcp1.Margin = new System.Windows.Forms.Padding(4);
            this.AxRealSvrOcxTcp1.Name = "AxRealSvrOcxTcp1";
            this.AxRealSvrOcxTcp1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("AxRealSvrOcxTcp1.OcxState")));
            this.AxRealSvrOcxTcp1.Size = new System.Drawing.Size(32, 32);
            this.AxRealSvrOcxTcp1.TabIndex = 16;
            this.AxRealSvrOcxTcp1.Visible = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.panelWindow);
            this.Controls.Add(this.panelMenu);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.AxRealSvrOcxTcp1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMain";
            this.Text = "工具柜管理";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyDown);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.panelMenu.ResumeLayout(false);
            this.panelMenu.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAvatar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AxRealSvrOcxTcp1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.PictureBox pbAvatar;
        private System.Windows.Forms.Label lbUserName;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Button btnToolCheck;
        private System.Windows.Forms.Button btnToolPurchase;
        private System.Windows.Forms.Button btnToolManage;
        private System.Windows.Forms.Button btnToolPlan;
        private System.Windows.Forms.Panel panelWindow;
        private System.Windows.Forms.Label lbInitTime;
        private System.Windows.Forms.Timer tmUhfInit;
        private System.Windows.Forms.Button btnToolReport;
        private System.Windows.Forms.Button btnIoRecord;
        private System.Windows.Forms.Button btnAboutApp;
        private AxRealSvrOcxTcpLib.AxRealSvrOcxTcp AxRealSvrOcxTcp1;
    }
}

