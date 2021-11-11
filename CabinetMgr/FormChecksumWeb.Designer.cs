namespace CabinetMgr
{
    partial class FormChecksumWeb
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChecksumWeb));
            this.tbToolCode = new System.Windows.Forms.TextBox();
            this.btnQuery = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnQuery)).BeginInit();
            this.SuspendLayout();
            // 
            // tbToolCode
            // 
            this.tbToolCode.Font = new System.Drawing.Font("宋体", 34F);
            this.tbToolCode.Location = new System.Drawing.Point(152, 8);
            this.tbToolCode.Name = "tbToolCode";
            this.tbToolCode.Size = new System.Drawing.Size(568, 59);
            this.tbToolCode.TabIndex = 0;
            this.tbToolCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnQuery
            // 
            this.btnQuery.Image = ((System.Drawing.Image)(resources.GetObject("btnQuery.Image")));
            this.btnQuery.Location = new System.Drawing.Point(743, 8);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(81, 59);
            this.btnQuery.TabIndex = 1;
            this.btnQuery.TabStop = false;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // FormChecksumWeb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.tbToolCode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormChecksumWeb";
            this.Text = "工具校验";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormChecksumWeb_FormClosing);
            this.Load += new System.EventHandler(this.FormChecksumWeb_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btnQuery)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbToolCode;
        private System.Windows.Forms.PictureBox btnQuery;
    }
}