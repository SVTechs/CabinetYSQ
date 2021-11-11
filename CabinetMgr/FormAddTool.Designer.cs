namespace CabinetMgr
{
    partial class FormAddTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddTool));
            this.tbDeviationPositive = new System.Windows.Forms.TextBox();
            this.tbStdValue = new System.Windows.Forms.TextBox();
            this.cbToolType = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbManager = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbToolSpec = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbToolName = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.cbToolPosition = new System.Windows.Forms.ComboBox();
            this.panelMain = new System.Windows.Forms.Panel();
            this.tbHardWareId = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cbCheckIntervalType = new System.Windows.Forms.ComboBox();
            this.tbDeviationNegative = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbCheckInterval = new System.Windows.Forms.TextBox();
            this.tbCardId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbToolPositionType = new System.Windows.Forms.ComboBox();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbDeviationPositive
            // 
            this.tbDeviationPositive.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbDeviationPositive.Location = new System.Drawing.Point(121, 176);
            this.tbDeviationPositive.Name = "tbDeviationPositive";
            this.tbDeviationPositive.Size = new System.Drawing.Size(214, 29);
            this.tbDeviationPositive.TabIndex = 264;
            // 
            // tbStdValue
            // 
            this.tbStdValue.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbStdValue.Location = new System.Drawing.Point(121, 275);
            this.tbStdValue.Name = "tbStdValue";
            this.tbStdValue.Size = new System.Drawing.Size(214, 29);
            this.tbStdValue.TabIndex = 263;
            // 
            // cbToolType
            // 
            this.cbToolType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbToolType.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbToolType.FormattingEnabled = true;
            this.cbToolType.Items.AddRange(new object[] {
            "扭力扳手",
            "测量工具",
            "普通工具"});
            this.cbToolType.Location = new System.Drawing.Point(121, 77);
            this.cbToolType.Name = "cbToolType";
            this.cbToolType.Size = new System.Drawing.Size(214, 29);
            this.cbToolType.TabIndex = 261;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(366, 178);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(94, 21);
            this.label12.TabIndex = 259;
            this.label12.Text = "工具柜位置:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(55, 180);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 21);
            this.label11.TabIndex = 258;
            this.label11.Text = "正误差:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(55, 279);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(62, 21);
            this.label10.TabIndex = 257;
            this.label10.Text = "标准值:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(7, 80);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(110, 21);
            this.label9.TabIndex = 256;
            this.label9.Text = "配置工具类型:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(366, 129);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 21);
            this.label7.TabIndex = 255;
            this.label7.Text = "工具柜层级:";
            // 
            // tbComment
            // 
            this.tbComment.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbComment.Location = new System.Drawing.Point(121, 323);
            this.tbComment.Name = "tbComment";
            this.tbComment.Size = new System.Drawing.Size(557, 29);
            this.tbComment.TabIndex = 254;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(71, 327);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 21);
            this.label6.TabIndex = 253;
            this.label6.Text = "备注:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(398, 278);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 21);
            this.label5.TabIndex = 251;
            this.label5.Text = "有效期:";
            // 
            // tbManager
            // 
            this.tbManager.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbManager.Location = new System.Drawing.Point(464, 225);
            this.tbManager.Name = "tbManager";
            this.tbManager.Size = new System.Drawing.Size(214, 29);
            this.tbManager.TabIndex = 250;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(398, 229);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 21);
            this.label1.TabIndex = 249;
            this.label1.Text = "保管人:";
            // 
            // tbToolSpec
            // 
            this.tbToolSpec.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbToolSpec.Location = new System.Drawing.Point(464, 26);
            this.tbToolSpec.Name = "tbToolSpec";
            this.tbToolSpec.Size = new System.Drawing.Size(214, 29);
            this.tbToolSpec.TabIndex = 244;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(382, 30);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 21);
            this.label8.TabIndex = 243;
            this.label8.Text = "工具规格:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(39, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 21);
            this.label3.TabIndex = 242;
            this.label3.Text = "工具名称:";
            // 
            // tbToolName
            // 
            this.tbToolName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbToolName.Location = new System.Drawing.Point(121, 26);
            this.tbToolName.Name = "tbToolName";
            this.tbToolName.Size = new System.Drawing.Size(214, 29);
            this.tbToolName.TabIndex = 241;
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSubmit.BackgroundImage")));
            this.btnSubmit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSubmit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSubmit.FlatAppearance.BorderSize = 0;
            this.btnSubmit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSubmit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.Location = new System.Drawing.Point(295, 377);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(115, 30);
            this.btnSubmit.TabIndex = 267;
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // cbToolPosition
            // 
            this.cbToolPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbToolPosition.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbToolPosition.FormattingEnabled = true;
            this.cbToolPosition.Location = new System.Drawing.Point(464, 175);
            this.cbToolPosition.Name = "cbToolPosition";
            this.cbToolPosition.Size = new System.Drawing.Size(214, 29);
            this.cbToolPosition.TabIndex = 268;
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.tbHardWareId);
            this.panelMain.Controls.Add(this.label13);
            this.panelMain.Controls.Add(this.cbCheckIntervalType);
            this.panelMain.Controls.Add(this.tbDeviationNegative);
            this.panelMain.Controls.Add(this.label4);
            this.panelMain.Controls.Add(this.tbCheckInterval);
            this.panelMain.Controls.Add(this.tbCardId);
            this.panelMain.Controls.Add(this.label2);
            this.panelMain.Controls.Add(this.cbToolPositionType);
            this.panelMain.Controls.Add(this.tbToolName);
            this.panelMain.Controls.Add(this.cbToolPosition);
            this.panelMain.Controls.Add(this.label3);
            this.panelMain.Controls.Add(this.label8);
            this.panelMain.Controls.Add(this.tbDeviationPositive);
            this.panelMain.Controls.Add(this.tbToolSpec);
            this.panelMain.Controls.Add(this.tbStdValue);
            this.panelMain.Controls.Add(this.label1);
            this.panelMain.Controls.Add(this.tbManager);
            this.panelMain.Controls.Add(this.cbToolType);
            this.panelMain.Controls.Add(this.label5);
            this.panelMain.Controls.Add(this.label12);
            this.panelMain.Controls.Add(this.label11);
            this.panelMain.Controls.Add(this.label6);
            this.panelMain.Controls.Add(this.label10);
            this.panelMain.Controls.Add(this.tbComment);
            this.panelMain.Controls.Add(this.label9);
            this.panelMain.Controls.Add(this.label7);
            this.panelMain.Location = new System.Drawing.Point(0, 2);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(697, 363);
            this.panelMain.TabIndex = 269;
            // 
            // tbHardWareId
            // 
            this.tbHardWareId.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbHardWareId.Location = new System.Drawing.Point(464, 77);
            this.tbHardWareId.Name = "tbHardWareId";
            this.tbHardWareId.Size = new System.Drawing.Size(214, 29);
            this.tbHardWareId.TabIndex = 276;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(382, 80);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(78, 21);
            this.label13.TabIndex = 277;
            this.label13.Text = "配置编号:";
            // 
            // cbCheckIntervalType
            // 
            this.cbCheckIntervalType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCheckIntervalType.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.cbCheckIntervalType.FormattingEnabled = true;
            this.cbCheckIntervalType.Items.AddRange(new object[] {
            "天",
            "月",
            "年"});
            this.cbCheckIntervalType.Location = new System.Drawing.Point(624, 275);
            this.cbCheckIntervalType.Name = "cbCheckIntervalType";
            this.cbCheckIntervalType.Size = new System.Drawing.Size(54, 29);
            this.cbCheckIntervalType.TabIndex = 275;
            // 
            // tbDeviationNegative
            // 
            this.tbDeviationNegative.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbDeviationNegative.Location = new System.Drawing.Point(121, 226);
            this.tbDeviationNegative.Name = "tbDeviationNegative";
            this.tbDeviationNegative.Size = new System.Drawing.Size(214, 29);
            this.tbDeviationNegative.TabIndex = 274;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(55, 230);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 21);
            this.label4.TabIndex = 273;
            this.label4.Text = "负误差:";
            // 
            // tbCheckInterval
            // 
            this.tbCheckInterval.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbCheckInterval.Location = new System.Drawing.Point(464, 275);
            this.tbCheckInterval.Name = "tbCheckInterval";
            this.tbCheckInterval.Size = new System.Drawing.Size(154, 29);
            this.tbCheckInterval.TabIndex = 272;
            // 
            // tbCardId
            // 
            this.tbCardId.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbCardId.Location = new System.Drawing.Point(121, 126);
            this.tbCardId.Name = "tbCardId";
            this.tbCardId.Size = new System.Drawing.Size(214, 29);
            this.tbCardId.TabIndex = 270;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(39, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 21);
            this.label2.TabIndex = 271;
            this.label2.Text = "工具卡号:";
            // 
            // cbToolPositionType
            // 
            this.cbToolPositionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbToolPositionType.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbToolPositionType.FormattingEnabled = true;
            this.cbToolPositionType.Items.AddRange(new object[] {
            "上层",
            "下层"});
            this.cbToolPositionType.Location = new System.Drawing.Point(464, 126);
            this.cbToolPositionType.Name = "cbToolPositionType";
            this.cbToolPositionType.Size = new System.Drawing.Size(214, 29);
            this.cbToolPositionType.TabIndex = 269;
            // 
            // FormAddTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.ClientSize = new System.Drawing.Size(698, 425);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.btnSubmit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormAddTool";
            this.Text = "工具管理-增加";
            this.Load += new System.EventHandler(this.FormAddTool_Load);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.TextBox tbDeviationPositive;
        private System.Windows.Forms.TextBox tbStdValue;
        private System.Windows.Forms.ComboBox cbToolType;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbComment;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbManager;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbToolSpec;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbToolName;
        private System.Windows.Forms.ComboBox cbToolPosition;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.ComboBox cbToolPositionType;
        private System.Windows.Forms.TextBox tbCardId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbCheckInterval;
        private System.Windows.Forms.TextBox tbDeviationNegative;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbCheckIntervalType;
        private System.Windows.Forms.TextBox tbHardWareId;
        private System.Windows.Forms.Label label13;
    }
}