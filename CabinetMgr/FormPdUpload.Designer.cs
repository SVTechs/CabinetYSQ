namespace CabinetMgr
{
    partial class FormPdUpload
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPdID = new System.Windows.Forms.TextBox();
            this.tbImagePath = new System.Windows.Forms.TextBox();
            this.btnUpload = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(74, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "PdID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(74, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Path";
            // 
            // tbPdID
            // 
            this.tbPdID.Location = new System.Drawing.Point(131, 46);
            this.tbPdID.Name = "tbPdID";
            this.tbPdID.Size = new System.Drawing.Size(296, 21);
            this.tbPdID.TabIndex = 3;
            // 
            // tbImagePath
            // 
            this.tbImagePath.Location = new System.Drawing.Point(131, 105);
            this.tbImagePath.Name = "tbImagePath";
            this.tbImagePath.Size = new System.Drawing.Size(296, 21);
            this.tbImagePath.TabIndex = 4;
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(209, 156);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 23);
            this.btnUpload.TabIndex = 5;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // FormPdUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 214);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.tbImagePath);
            this.Controls.Add(this.tbPdID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormPdUpload";
            this.Text = "工序修改";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbPdID;
        private System.Windows.Forms.TextBox tbImagePath;
        private System.Windows.Forms.Button btnUpload;
    }
}