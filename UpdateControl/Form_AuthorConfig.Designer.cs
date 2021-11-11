namespace UpdateControl
{
    partial class Form_AuthorConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_AuthorConfig));
            this.label1 = new System.Windows.Forms.Label();
            this.tb_AuthorName = new System.Windows.Forms.TextBox();
            this.btn_Confirm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "您的用户标识";
            // 
            // tb_AuthorName
            // 
            this.tb_AuthorName.Location = new System.Drawing.Point(114, 27);
            this.tb_AuthorName.Name = "tb_AuthorName";
            this.tb_AuthorName.Size = new System.Drawing.Size(214, 21);
            this.tb_AuthorName.TabIndex = 1;
            // 
            // btn_Confirm
            // 
            this.btn_Confirm.Location = new System.Drawing.Point(144, 71);
            this.btn_Confirm.Name = "btn_Confirm";
            this.btn_Confirm.Size = new System.Drawing.Size(75, 23);
            this.btn_Confirm.TabIndex = 2;
            this.btn_Confirm.Text = "确定";
            this.btn_Confirm.UseVisualStyleBackColor = true;
            this.btn_Confirm.Click += new System.EventHandler(this.btn_Confirm_Click);
            // 
            // Form_AuthorConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 109);
            this.Controls.Add(this.btn_Confirm);
            this.Controls.Add(this.tb_AuthorName);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_AuthorConfig";
            this.Text = "用户标识";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_AuthorConfig_FormClosing);
            this.Load += new System.EventHandler(this.Form_AuthorConfig_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_AuthorName;
        private System.Windows.Forms.Button btn_Confirm;
    }
}