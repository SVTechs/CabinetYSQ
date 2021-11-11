namespace ZkCollector
{
    partial class FormCollect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCollect));
            this.pbFpImage = new System.Windows.Forms.PictureBox();
            this.lbProgress = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbFpImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pbFpImage
            // 
            this.pbFpImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbFpImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.pbFpImage.Location = new System.Drawing.Point(0, 0);
            this.pbFpImage.Name = "pbFpImage";
            this.pbFpImage.Size = new System.Drawing.Size(386, 388);
            this.pbFpImage.TabIndex = 0;
            this.pbFpImage.TabStop = false;
            // 
            // lbProgress
            // 
            this.lbProgress.AutoSize = true;
            this.lbProgress.Location = new System.Drawing.Point(128, 407);
            this.lbProgress.Name = "lbProgress";
            this.lbProgress.Size = new System.Drawing.Size(131, 12);
            this.lbProgress.TabIndex = 1;
            this.lbProgress.Text = "请按压指纹3次进行注册";
            // 
            // FormCollect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 440);
            this.Controls.Add(this.lbProgress);
            this.Controls.Add(this.pbFpImage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCollect";
            this.Text = "指纹采集";
            this.Load += new System.EventHandler(this.FormCollect_Load);
            this.Shown += new System.EventHandler(this.FormCollect_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pbFpImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbFpImage;
        private System.Windows.Forms.Label lbProgress;
    }
}