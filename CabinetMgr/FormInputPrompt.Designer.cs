namespace CabinetMgr
{
    partial class FormInputPrompt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInputPrompt));
            this.tbItemContent = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.lbItemName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbItemContent
            // 
            this.tbItemContent.Location = new System.Drawing.Point(90, 34);
            this.tbItemContent.Name = "tbItemContent";
            this.tbItemContent.Size = new System.Drawing.Size(451, 21);
            this.tbItemContent.TabIndex = 0;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(243, 77);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.Text = "提交";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // lbItemName
            // 
            this.lbItemName.AutoSize = true;
            this.lbItemName.Location = new System.Drawing.Point(17, 37);
            this.lbItemName.Name = "lbItemName";
            this.lbItemName.Size = new System.Drawing.Size(35, 12);
            this.lbItemName.TabIndex = 2;
            this.lbItemName.Text = "Item:";
            // 
            // FormInputPrompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 121);
            this.Controls.Add(this.lbItemName);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.tbItemContent);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormInputPrompt";
            this.Text = "请输入";
            this.Load += new System.EventHandler(this.FormInoputPrompt_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbItemContent;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Label lbItemName;
    }
}