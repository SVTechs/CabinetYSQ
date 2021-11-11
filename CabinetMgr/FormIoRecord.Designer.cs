namespace CabinetMgr
{
    partial class FormIoRecord
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
            this.btnBorrowRecord = new System.Windows.Forms.Button();
            this.panelWindow = new System.Windows.Forms.Panel();
            this.panelTab = new System.Windows.Forms.Panel();
            this.panelTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBorrowRecord
            // 
            this.btnBorrowRecord.BackgroundImage = global::CabinetMgr.Properties.Resources.tab_borrow_n;
            this.btnBorrowRecord.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBorrowRecord.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBorrowRecord.FlatAppearance.BorderSize = 0;
            this.btnBorrowRecord.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBorrowRecord.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBorrowRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBorrowRecord.Location = new System.Drawing.Point(4, 12);
            this.btnBorrowRecord.Margin = new System.Windows.Forms.Padding(4);
            this.btnBorrowRecord.Name = "btnBorrowRecord";
            this.btnBorrowRecord.Size = new System.Drawing.Size(185, 36);
            this.btnBorrowRecord.TabIndex = 250;
            this.btnBorrowRecord.Tag = "0";
            this.btnBorrowRecord.UseVisualStyleBackColor = true;
            this.btnBorrowRecord.Click += new System.EventHandler(this.btnBorrowRecord_Click);
            // 
            // panelWindow
            // 
            this.panelWindow.Location = new System.Drawing.Point(5, 51);
            this.panelWindow.Margin = new System.Windows.Forms.Padding(4);
            this.panelWindow.Name = "panelWindow";
            this.panelWindow.Size = new System.Drawing.Size(815, 517);
            this.panelWindow.TabIndex = 254;
            // 
            // panelTab
            // 
            this.panelTab.BackColor = System.Drawing.Color.Transparent;
            this.panelTab.Controls.Add(this.btnBorrowRecord);
            this.panelTab.Location = new System.Drawing.Point(7, 2);
            this.panelTab.Margin = new System.Windows.Forms.Padding(4);
            this.panelTab.Name = "panelTab";
            this.panelTab.Size = new System.Drawing.Size(603, 49);
            this.panelTab.TabIndex = 256;
            // 
            // FormIoRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::CabinetMgr.Properties.Resources.right_2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(854, 613);
            this.Controls.Add(this.panelTab);
            this.Controls.Add(this.panelWindow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormIoRecord";
            this.Text = "FormIoRecord";
            this.Load += new System.EventHandler(this.FormIoRecord_Load);
            this.Shown += new System.EventHandler(this.FormIoRecord_Shown);
            this.panelTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBorrowRecord;
        private System.Windows.Forms.Panel panelWindow;
        private System.Windows.Forms.Panel panelTab;
    }
}