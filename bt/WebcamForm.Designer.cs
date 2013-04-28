namespace bt
{
    partial class WebcamForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cameraView = new System.Windows.Forms.PictureBox();
            this.comPorts = new System.Windows.Forms.ComboBox();
            this.connectBtn = new System.Windows.Forms.Button();
            this.statusMsg = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.resetPathBtn = new System.Windows.Forms.Button();
            this.rgbLabel = new System.Windows.Forms.Label();
            this.fpsLabel = new System.Windows.Forms.Label();
            this.trainLeftBtn = new System.Windows.Forms.Button();
            this.trainRightBtn = new System.Windows.Forms.Button();
            this.activeChk = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cameraView)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.cameraView, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.comPorts, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.connectBtn, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.statusMsg, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 470F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(846, 487);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // cameraView
            // 
            this.cameraView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraView.Location = new System.Drawing.Point(183, 3);
            this.cameraView.Name = "cameraView";
            this.tableLayoutPanel1.SetRowSpan(this.cameraView, 4);
            this.cameraView.Size = new System.Drawing.Size(671, 554);
            this.cameraView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.cameraView.TabIndex = 1;
            this.cameraView.TabStop = false;
            this.cameraView.Paint += new System.Windows.Forms.PaintEventHandler(this.cameraView_Paint);
            this.cameraView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cameraView_MouseDown);
            this.cameraView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.cameraView_MouseMove);
            this.cameraView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cameraView_MouseUp);
            // 
            // comPorts
            // 
            this.comPorts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comPorts.FormattingEnabled = true;
            this.comPorts.Items.AddRange(new object[] {
            "COM5"});
            this.comPorts.Location = new System.Drawing.Point(3, 3);
            this.comPorts.Name = "comPorts";
            this.comPorts.Size = new System.Drawing.Size(174, 21);
            this.comPorts.TabIndex = 3;
            // 
            // connectBtn
            // 
            this.connectBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.connectBtn.Location = new System.Drawing.Point(3, 33);
            this.connectBtn.Name = "connectBtn";
            this.connectBtn.Size = new System.Drawing.Size(174, 24);
            this.connectBtn.TabIndex = 2;
            this.connectBtn.Text = "Connect";
            this.connectBtn.UseVisualStyleBackColor = true;
            this.connectBtn.Click += new System.EventHandler(this.connectBtn_Click);
            // 
            // statusMsg
            // 
            this.statusMsg.AutoSize = true;
            this.statusMsg.Dock = System.Windows.Forms.DockStyle.Top;
            this.statusMsg.Location = new System.Drawing.Point(3, 60);
            this.statusMsg.Name = "statusMsg";
            this.statusMsg.Size = new System.Drawing.Size(174, 13);
            this.statusMsg.TabIndex = 4;
            this.statusMsg.Text = "Disconnected";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.resetPathBtn, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.rgbLabel, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.fpsLabel, 0, 8);
            this.tableLayoutPanel2.Controls.Add(this.trainLeftBtn, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.trainRightBtn, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.activeChk, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 93);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 9;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 122F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(163, 394);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 53);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(157, 116);
            this.panel1.TabIndex = 2;
            // 
            // resetPathBtn
            // 
            this.resetPathBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resetPathBtn.Location = new System.Drawing.Point(3, 175);
            this.resetPathBtn.Name = "resetPathBtn";
            this.resetPathBtn.Size = new System.Drawing.Size(157, 24);
            this.resetPathBtn.TabIndex = 3;
            this.resetPathBtn.Text = "Reset path";
            this.resetPathBtn.UseVisualStyleBackColor = true;
            this.resetPathBtn.Click += new System.EventHandler(this.resetPathBtn_Click);
            // 
            // rgbLabel
            // 
            this.rgbLabel.AutoSize = true;
            this.rgbLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgbLabel.Location = new System.Drawing.Point(3, 202);
            this.rgbLabel.Name = "rgbLabel";
            this.rgbLabel.Size = new System.Drawing.Size(157, 20);
            this.rgbLabel.TabIndex = 5;
            this.rgbLabel.Text = "RGB:";
            // 
            // fpsLabel
            // 
            this.fpsLabel.AutoSize = true;
            this.fpsLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.fpsLabel.Location = new System.Drawing.Point(3, 366);
            this.fpsLabel.Name = "fpsLabel";
            this.fpsLabel.Size = new System.Drawing.Size(157, 13);
            this.fpsLabel.TabIndex = 6;
            this.fpsLabel.Text = "FPS: 0";
            // 
            // trainLeftBtn
            // 
            this.trainLeftBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trainLeftBtn.Location = new System.Drawing.Point(3, 225);
            this.trainLeftBtn.Name = "trainLeftBtn";
            this.trainLeftBtn.Size = new System.Drawing.Size(157, 24);
            this.trainLeftBtn.TabIndex = 7;
            this.trainLeftBtn.Text = "Train Left";
            this.trainLeftBtn.UseVisualStyleBackColor = true;
            this.trainLeftBtn.Click += new System.EventHandler(this.trainLeftBtn_Click);
            // 
            // trainRightBtn
            // 
            this.trainRightBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trainRightBtn.Location = new System.Drawing.Point(3, 255);
            this.trainRightBtn.Name = "trainRightBtn";
            this.trainRightBtn.Size = new System.Drawing.Size(157, 24);
            this.trainRightBtn.TabIndex = 8;
            this.trainRightBtn.Text = "Train Right";
            this.trainRightBtn.UseVisualStyleBackColor = true;
            this.trainRightBtn.Click += new System.EventHandler(this.trainRightBtn_Click);
            // 
            // activeChk
            // 
            this.activeChk.AutoSize = true;
            this.activeChk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.activeChk.Location = new System.Drawing.Point(3, 3);
            this.activeChk.Name = "activeChk";
            this.activeChk.Size = new System.Drawing.Size(157, 24);
            this.activeChk.TabIndex = 9;
            this.activeChk.Text = "Active";
            this.activeChk.UseVisualStyleBackColor = true;
            this.activeChk.CheckedChanged += new System.EventHandler(this.activeChk_CheckedChanged);
            // 
            // WebcamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 487);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "WebcamForm";
            this.Text = "WebcamForm";
            this.Load += new System.EventHandler(this.WebcamForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cameraView)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox cameraView;
        private System.Windows.Forms.Button connectBtn;
        private System.Windows.Forms.ComboBox comPorts;
        private System.Windows.Forms.Label statusMsg;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button resetPathBtn;
        private System.Windows.Forms.Label rgbLabel;
        private System.Windows.Forms.Label fpsLabel;
        private System.Windows.Forms.Button trainLeftBtn;
        private System.Windows.Forms.Button trainRightBtn;
        private System.Windows.Forms.CheckBox activeChk;

    }
}