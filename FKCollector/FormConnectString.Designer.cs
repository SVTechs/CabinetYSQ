namespace FKCollector
{
    partial class FormConnectString
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
            this.tbConnStr = new System.Windows.Forms.TextBox();
            this.btnConnSave = new C1.Win.C1Input.C1Button();
            ((System.ComponentModel.ISupportInitialize)(this.btnConnSave)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "连接字符串";
            // 
            // tbConnStr
            // 
            this.tbConnStr.Location = new System.Drawing.Point(100, 19);
            this.tbConnStr.Name = "tbConnStr";
            this.tbConnStr.Size = new System.Drawing.Size(651, 25);
            this.tbConnStr.TabIndex = 1;
            // 
            // btnConnSave
            // 
            this.btnConnSave.Location = new System.Drawing.Point(263, 77);
            this.btnConnSave.Name = "btnConnSave";
            this.btnConnSave.Size = new System.Drawing.Size(83, 29);
            this.btnConnSave.TabIndex = 12;
            this.btnConnSave.Text = "保存";
            this.btnConnSave.UseVisualStyleBackColor = true;
            this.btnConnSave.Click += new System.EventHandler(this.btnConnSave_Click);
            // 
            // FormConnectString
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 158);
            this.Controls.Add(this.btnConnSave);
            this.Controls.Add(this.tbConnStr);
            this.Controls.Add(this.label1);
            this.Name = "FormConnectString";
            this.Text = "连接修改";
            this.VisualStyleHolder = C1.Win.C1Ribbon.VisualStyle.Office2010Blue;
            this.Load += new System.EventHandler(this.FormConnectString_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btnConnSave)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbConnStr;
        private C1.Win.C1Input.C1Button btnConnSave;
    }
}