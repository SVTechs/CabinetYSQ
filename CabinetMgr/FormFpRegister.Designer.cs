namespace CabinetMgr
{
    partial class FormFpRegister
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
            this.pbFpImage = new System.Windows.Forms.PictureBox();
            this.lbHint = new System.Windows.Forms.Label();
            this.lbRepeatCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbFpImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pbFpImage
            // 
            this.pbFpImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.pbFpImage.Location = new System.Drawing.Point(0, 0);
            this.pbFpImage.Name = "pbFpImage";
            this.pbFpImage.Size = new System.Drawing.Size(274, 306);
            this.pbFpImage.TabIndex = 0;
            this.pbFpImage.TabStop = false;
            // 
            // lbHint
            // 
            this.lbHint.AutoSize = true;
            this.lbHint.Location = new System.Drawing.Point(30, 318);
            this.lbHint.Name = "lbHint";
            this.lbHint.Size = new System.Drawing.Size(155, 12);
            this.lbHint.TabIndex = 1;
            this.lbHint.Text = "请在指纹仪上按压手指,还需";
            // 
            // lbRepeatCount
            // 
            this.lbRepeatCount.AutoSize = true;
            this.lbRepeatCount.Location = new System.Drawing.Point(186, 318);
            this.lbRepeatCount.Name = "lbRepeatCount";
            this.lbRepeatCount.Size = new System.Drawing.Size(23, 12);
            this.lbRepeatCount.TabIndex = 2;
            this.lbRepeatCount.Text = "3次";
            // 
            // FormFpRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 339);
            this.Controls.Add(this.lbRepeatCount);
            this.Controls.Add(this.lbHint);
            this.Controls.Add(this.pbFpImage);
            this.Name = "FormFpRegister";
            this.Text = "指纹录入";
            this.Load += new System.EventHandler(this.FormFpRegister_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbFpImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbFpImage;
        private System.Windows.Forms.Label lbHint;
        private System.Windows.Forms.Label lbRepeatCount;
    }
}