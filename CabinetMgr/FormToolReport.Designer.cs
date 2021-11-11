namespace CabinetMgr
{
    partial class FormToolReport
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormToolReport));
            this.panelUpper = new System.Windows.Forms.Panel();
            this.lbl_Floor = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cUpperGrid = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.cBottomGrid = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.tmGridUpdater = new System.Windows.Forms.Timer(this.components);
            this.panelUpper.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cUpperGrid)).BeginInit();
            this.panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cBottomGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panelUpper
            // 
            this.panelUpper.BackColor = System.Drawing.SystemColors.Control;
            this.panelUpper.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUpper.Controls.Add(this.lbl_Floor);
            this.panelUpper.Controls.Add(this.pictureBox1);
            this.panelUpper.Controls.Add(this.cUpperGrid);
            this.panelUpper.Location = new System.Drawing.Point(2, 10);
            this.panelUpper.Margin = new System.Windows.Forms.Padding(4);
            this.panelUpper.Name = "panelUpper";
            this.panelUpper.Size = new System.Drawing.Size(815, 551);
            this.panelUpper.TabIndex = 5;
            // 
            // lbl_Floor
            // 
            this.lbl_Floor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Floor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(130)))), ((int)(((byte)(193)))));
            this.lbl_Floor.Font = new System.Drawing.Font("黑体", 22F);
            this.lbl_Floor.ForeColor = System.Drawing.Color.White;
            this.lbl_Floor.Location = new System.Drawing.Point(553, 5);
            this.lbl_Floor.Name = "lbl_Floor";
            this.lbl_Floor.Size = new System.Drawing.Size(238, 35);
            this.lbl_Floor.TabIndex = 4;
            this.lbl_Floor.Text = "当前层：";
            this.lbl_Floor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_Floor.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(815, 52);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // cUpperGrid
            // 
            this.cUpperGrid.AllowEditing = false;
            this.cUpperGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.cUpperGrid.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.cUpperGrid.ColumnInfo = resources.GetString("cUpperGrid.ColumnInfo");
            this.cUpperGrid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cUpperGrid.DrawMode = C1.Win.C1FlexGrid.DrawModeEnum.OwnerDraw;
            this.cUpperGrid.Font = new System.Drawing.Font("宋体", 10F);
            this.cUpperGrid.Location = new System.Drawing.Point(0, -99);
            this.cUpperGrid.Margin = new System.Windows.Forms.Padding(4);
            this.cUpperGrid.Name = "cUpperGrid";
            this.cUpperGrid.Rows.DefaultSize = 22;
            this.cUpperGrid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.cUpperGrid.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Cell;
            this.cUpperGrid.Size = new System.Drawing.Size(815, 650);
            this.cUpperGrid.StyleInfo = resources.GetString("cUpperGrid.StyleInfo");
            this.cUpperGrid.TabIndex = 1;
            this.cUpperGrid.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.System;
            this.cUpperGrid.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.cUpperGrid_OwnerDrawCell);
            this.cUpperGrid.Click += new System.EventHandler(this.cUpperGrid_Click);
            this.cUpperGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cUpperGrid_MouseClick);
            // 
            // panelBottom
            // 
            this.panelBottom.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelBottom.BackgroundImage")));
            this.panelBottom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelBottom.Controls.Add(this.cBottomGrid);
            this.panelBottom.Location = new System.Drawing.Point(894, 479);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(4);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(872, 232);
            this.panelBottom.TabIndex = 6;
            this.panelBottom.Visible = false;
            // 
            // cBottomGrid
            // 
            this.cBottomGrid.AllowEditing = false;
            this.cBottomGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.cBottomGrid.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.cBottomGrid.ColumnInfo = resources.GetString("cBottomGrid.ColumnInfo");
            this.cBottomGrid.DrawMode = C1.Win.C1FlexGrid.DrawModeEnum.OwnerDraw;
            this.cBottomGrid.Font = new System.Drawing.Font("宋体", 10F);
            this.cBottomGrid.Location = new System.Drawing.Point(0, 49);
            this.cBottomGrid.Margin = new System.Windows.Forms.Padding(4);
            this.cBottomGrid.Name = "cBottomGrid";
            this.cBottomGrid.Rows.DefaultSize = 22;
            this.cBottomGrid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.cBottomGrid.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Cell;
            this.cBottomGrid.Size = new System.Drawing.Size(867, 183);
            this.cBottomGrid.StyleInfo = resources.GetString("cBottomGrid.StyleInfo");
            this.cBottomGrid.TabIndex = 2;
            this.cBottomGrid.Visible = false;
            this.cBottomGrid.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.System;
            this.cBottomGrid.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.cBottomGrid_OwnerDrawCell);
            this.cBottomGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cBottomGrid_MouseClick);
            // 
            // tmGridUpdater
            // 
            this.tmGridUpdater.Enabled = true;
            this.tmGridUpdater.Interval = 1000;
            this.tmGridUpdater.Tick += new System.EventHandler(this.tmGridUpdater_Tick);
            // 
            // FormToolReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::CabinetMgr.Properties.Resources.right_2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(854, 613);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelUpper);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormToolReport";
            this.Text = "FormToolReport";
            this.Load += new System.EventHandler(this.FormToolReport_Load);
            this.Shown += new System.EventHandler(this.FormToolReport_Shown);
            this.panelUpper.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cUpperGrid)).EndInit();
            this.panelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cBottomGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelUpper;
        private C1.Win.C1FlexGrid.C1FlexGrid cUpperGrid;
        private System.Windows.Forms.Panel panelBottom;
        private C1.Win.C1FlexGrid.C1FlexGrid cBottomGrid;
        private System.Windows.Forms.Timer tmGridUpdater;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbl_Floor;
    }
}