namespace UserSync
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
            this.lbSysLog = new System.Windows.Forms.ListBox();
            this.mainNotify = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // lbSysLog
            // 
            this.lbSysLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbSysLog.FormattingEnabled = true;
            this.lbSysLog.ItemHeight = 15;
            this.lbSysLog.Location = new System.Drawing.Point(0, 0);
            this.lbSysLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lbSysLog.Name = "lbSysLog";
            this.lbSysLog.Size = new System.Drawing.Size(912, 475);
            this.lbSysLog.TabIndex = 0;
            // 
            // mainNotify
            // 
            this.mainNotify.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.mainNotify.Text = "任务同步工具";
            this.mainNotify.Visible = true;
            this.mainNotify.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.mainNotify_MouseDoubleClick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 380);
            this.Controls.Add(this.lbSysLog);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "任务同步工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.SizeChanged += new System.EventHandler(this.FormMain_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbSysLog;
        private System.Windows.Forms.NotifyIcon mainNotify;
    }
}

