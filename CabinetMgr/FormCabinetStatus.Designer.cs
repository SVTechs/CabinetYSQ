namespace CabinetMgr
{
    partial class FormCabinetStatus
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCabinetStatus));
            this.cUpperGrid = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.cBottomGrid = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.tmGridUpdater = new System.Windows.Forms.Timer(this.components);
            this.panelUpper = new System.Windows.Forms.Panel();
            this.lbl_Floor = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.cHistoryGrid = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.tmRtRefresher = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.cUpperGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cBottomGrid)).BeginInit();
            this.panelUpper.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cHistoryGrid)).BeginInit();
            this.SuspendLayout();
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
            this.cUpperGrid.Location = new System.Drawing.Point(0, 58);
            this.cUpperGrid.Margin = new System.Windows.Forms.Padding(4);
            this.cUpperGrid.Name = "cUpperGrid";
            this.cUpperGrid.Rows.DefaultSize = 22;
            this.cUpperGrid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.cUpperGrid.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Cell;
            this.cUpperGrid.Size = new System.Drawing.Size(1102, 350);
            this.cUpperGrid.StyleInfo = resources.GetString("cUpperGrid.StyleInfo");
            this.cUpperGrid.TabIndex = 1;
            this.cUpperGrid.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.System;
            this.cUpperGrid.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.cUpperGrid_OwnerDrawCell);
            // 
            // cBottomGrid
            // 
            this.cBottomGrid.AllowEditing = false;
            this.cBottomGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.cBottomGrid.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.cBottomGrid.ColumnInfo = resources.GetString("cBottomGrid.ColumnInfo");
            this.cBottomGrid.DrawMode = C1.Win.C1FlexGrid.DrawModeEnum.OwnerDraw;
            this.cBottomGrid.Font = new System.Drawing.Font("宋体", 10F);
            this.cBottomGrid.Location = new System.Drawing.Point(0, 65);
            this.cBottomGrid.Margin = new System.Windows.Forms.Padding(4);
            this.cBottomGrid.Name = "cBottomGrid";
            this.cBottomGrid.Rows.DefaultSize = 22;
            this.cBottomGrid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.cBottomGrid.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Cell;
            this.cBottomGrid.Size = new System.Drawing.Size(437, 339);
            this.cBottomGrid.StyleInfo = resources.GetString("cBottomGrid.StyleInfo");
            this.cBottomGrid.TabIndex = 2;
            this.cBottomGrid.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.System;
            this.cBottomGrid.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.cBottomGrid_OwnerDrawCell);
            // 
            // tmGridUpdater
            // 
            this.tmGridUpdater.Enabled = true;
            this.tmGridUpdater.Interval = 1000;
            this.tmGridUpdater.Tick += new System.EventHandler(this.tmGridUpdater_Tick);
            // 
            // panelUpper
            // 
            this.panelUpper.BackColor = System.Drawing.SystemColors.Control;
            this.panelUpper.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelUpper.Controls.Add(this.lbl_Floor);
            this.panelUpper.Controls.Add(this.pictureBox1);
            this.panelUpper.Controls.Add(this.cUpperGrid);
            this.panelUpper.Location = new System.Drawing.Point(16, 26);
            this.panelUpper.Margin = new System.Windows.Forms.Padding(4);
            this.panelUpper.Name = "panelUpper";
            this.panelUpper.Size = new System.Drawing.Size(1102, 408);
            this.panelUpper.TabIndex = 4;
            // 
            // lbl_Floor
            // 
            this.lbl_Floor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(130)))), ((int)(((byte)(193)))));
            this.lbl_Floor.Font = new System.Drawing.Font("黑体", 22F);
            this.lbl_Floor.ForeColor = System.Drawing.Color.White;
            this.lbl_Floor.Location = new System.Drawing.Point(727, 10);
            this.lbl_Floor.Name = "lbl_Floor";
            this.lbl_Floor.Size = new System.Drawing.Size(238, 35);
            this.lbl_Floor.TabIndex = 3;
            this.lbl_Floor.Text = "当前层：";
            this.lbl_Floor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_Floor.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = global::CabinetMgr.Properties.Resources.head;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1102, 58);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // panelBottom
            // 
            this.panelBottom.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelBottom.BackgroundImage")));
            this.panelBottom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelBottom.Controls.Add(this.cBottomGrid);
            this.panelBottom.Location = new System.Drawing.Point(886, 462);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(4);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(437, 408);
            this.panelBottom.TabIndex = 5;
            this.panelBottom.Visible = false;
            // 
            // cHistoryGrid
            // 
            this.cHistoryGrid.AllowEditing = false;
            this.cHistoryGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.cHistoryGrid.ColumnInfo = resources.GetString("cHistoryGrid.ColumnInfo");
            this.cHistoryGrid.Font = new System.Drawing.Font("宋体", 10F);
            this.cHistoryGrid.Location = new System.Drawing.Point(1142, 26);
            this.cHistoryGrid.Margin = new System.Windows.Forms.Padding(4);
            this.cHistoryGrid.Name = "cHistoryGrid";
            this.cHistoryGrid.Rows.DefaultSize = 26;
            this.cHistoryGrid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.cHistoryGrid.Size = new System.Drawing.Size(1295, 406);
            this.cHistoryGrid.StyleInfo = resources.GetString("cHistoryGrid.StyleInfo");
            this.cHistoryGrid.TabIndex = 6;
            this.cHistoryGrid.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver;
            // 
            // tmRtRefresher
            // 
            this.tmRtRefresher.Enabled = true;
            this.tmRtRefresher.Interval = 1000;
            this.tmRtRefresher.Tick += new System.EventHandler(this.tmRtRefresher_Tick);
            // 
            // FormCabinetStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::CabinetMgr.Properties.Resources.right_2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1942, 478);
            this.Controls.Add(this.cHistoryGrid);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelUpper);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormCabinetStatus";
            this.Text = "FormCabinetStatus";
            this.Load += new System.EventHandler(this.FormCabinetStatus_Load);
            this.Shown += new System.EventHandler(this.FormCabinetStatus_Shown);
            this.VisibleChanged += new System.EventHandler(this.FormCabinetStatus_VisibleChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormCabinetStatus_KeyDown);
            this.Resize += new System.EventHandler(this.FormCabinetStatus_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.cUpperGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cBottomGrid)).EndInit();
            this.panelUpper.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cHistoryGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private C1.Win.C1FlexGrid.C1FlexGrid cUpperGrid;
        private C1.Win.C1FlexGrid.C1FlexGrid cBottomGrid;
        private System.Windows.Forms.Timer tmGridUpdater;
        private System.Windows.Forms.Panel panelUpper;
        private System.Windows.Forms.Panel panelBottom;
        private C1.Win.C1FlexGrid.C1FlexGrid cHistoryGrid;
        private System.Windows.Forms.Timer tmRtRefresher;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbl_Floor;
    }
}