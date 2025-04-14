namespace onnxNote
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_YOLOPOSE = new System.Windows.Forms.TabPage();
            this.panel_Main = new System.Windows.Forms.Panel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.panel_YOLOPOSE_BOTTOM = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_Save = new System.Windows.Forms.Button();
            this.button_frameForward10 = new System.Windows.Forms.Button();
            this.button_frameForward05 = new System.Windows.Forms.Button();
            this.button_frameForward01 = new System.Windows.Forms.Button();
            this.label_FrameCount = new System.Windows.Forms.Label();
            this.button_frameBackward01 = new System.Windows.Forms.Button();
            this.button_frameBackward05 = new System.Windows.Forms.Button();
            this.button_frameBackward10 = new System.Windows.Forms.Button();
            this.button_OpenMovieFile = new System.Windows.Forms.Button();
            this.trackBar_frameIndex = new System.Windows.Forms.TrackBar();
            this.panel_YOLOPOSE_TOP = new System.Windows.Forms.Panel();
            this.textBox_modelFilePath = new System.Windows.Forms.TextBox();
            this.button_OpenModelFile = new System.Windows.Forms.Button();
            this.label_modelPath = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.backgroundWorker_posePredict = new System.ComponentModel.BackgroundWorker();
            this.tabControl1.SuspendLayout();
            this.tabPage_YOLOPOSE.SuspendLayout();
            this.panel_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.panel_YOLOPOSE_BOTTOM.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_frameIndex)).BeginInit();
            this.panel_YOLOPOSE_TOP.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_YOLOPOSE);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(790, 810);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage_YOLOPOSE
            // 
            this.tabPage_YOLOPOSE.Controls.Add(this.panel_Main);
            this.tabPage_YOLOPOSE.Controls.Add(this.panel_YOLOPOSE_BOTTOM);
            this.tabPage_YOLOPOSE.Controls.Add(this.panel_YOLOPOSE_TOP);
            this.tabPage_YOLOPOSE.Controls.Add(this.label_modelPath);
            this.tabPage_YOLOPOSE.Location = new System.Drawing.Point(4, 22);
            this.tabPage_YOLOPOSE.Name = "tabPage_YOLOPOSE";
            this.tabPage_YOLOPOSE.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_YOLOPOSE.Size = new System.Drawing.Size(782, 784);
            this.tabPage_YOLOPOSE.TabIndex = 0;
            this.tabPage_YOLOPOSE.Text = "YOLOPOSE";
            this.tabPage_YOLOPOSE.UseVisualStyleBackColor = true;
            // 
            // panel_Main
            // 
            this.panel_Main.AutoScroll = true;
            this.panel_Main.Controls.Add(this.pictureBox);
            this.panel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Main.Location = new System.Drawing.Point(3, 39);
            this.panel_Main.Name = "panel_Main";
            this.panel_Main.Size = new System.Drawing.Size(776, 667);
            this.panel_Main.TabIndex = 3;
            this.panel_Main.Resize += new System.EventHandler(this.panel_Main_Resize);
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(54, 140);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(640, 360);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.Click += new System.EventHandler(this.pictureBox_Click);
            // 
            // panel_YOLOPOSE_BOTTOM
            // 
            this.panel_YOLOPOSE_BOTTOM.Controls.Add(this.panel1);
            this.panel_YOLOPOSE_BOTTOM.Controls.Add(this.trackBar_frameIndex);
            this.panel_YOLOPOSE_BOTTOM.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_YOLOPOSE_BOTTOM.Location = new System.Drawing.Point(3, 706);
            this.panel_YOLOPOSE_BOTTOM.Name = "panel_YOLOPOSE_BOTTOM";
            this.panel_YOLOPOSE_BOTTOM.Size = new System.Drawing.Size(776, 75);
            this.panel_YOLOPOSE_BOTTOM.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.button_Save);
            this.panel1.Controls.Add(this.button_frameForward10);
            this.panel1.Controls.Add(this.button_frameForward05);
            this.panel1.Controls.Add(this.button_frameForward01);
            this.panel1.Controls.Add(this.label_FrameCount);
            this.panel1.Controls.Add(this.button_frameBackward01);
            this.panel1.Controls.Add(this.button_frameBackward05);
            this.panel1.Controls.Add(this.button_frameBackward10);
            this.panel1.Controls.Add(this.button_OpenMovieFile);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 45);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(776, 30);
            this.panel1.TabIndex = 1;
            // 
            // button_Save
            // 
            this.button_Save.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_Save.Location = new System.Drawing.Point(650, 0);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(63, 30);
            this.button_Save.TabIndex = 8;
            this.button_Save.Text = "Save";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // button_frameForward10
            // 
            this.button_frameForward10.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_frameForward10.Location = new System.Drawing.Point(369, 0);
            this.button_frameForward10.Name = "button_frameForward10";
            this.button_frameForward10.Size = new System.Drawing.Size(58, 30);
            this.button_frameForward10.TabIndex = 6;
            this.button_frameForward10.Text = ">>>";
            this.button_frameForward10.UseVisualStyleBackColor = true;
            this.button_frameForward10.Click += new System.EventHandler(this.button_frameShift_Click);
            // 
            // button_frameForward05
            // 
            this.button_frameForward05.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_frameForward05.Location = new System.Drawing.Point(311, 0);
            this.button_frameForward05.Name = "button_frameForward05";
            this.button_frameForward05.Size = new System.Drawing.Size(58, 30);
            this.button_frameForward05.TabIndex = 5;
            this.button_frameForward05.Text = ">>";
            this.button_frameForward05.UseVisualStyleBackColor = true;
            this.button_frameForward05.Click += new System.EventHandler(this.button_frameShift_Click);
            // 
            // button_frameForward01
            // 
            this.button_frameForward01.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_frameForward01.Location = new System.Drawing.Point(253, 0);
            this.button_frameForward01.Name = "button_frameForward01";
            this.button_frameForward01.Size = new System.Drawing.Size(58, 30);
            this.button_frameForward01.TabIndex = 4;
            this.button_frameForward01.Text = ">";
            this.button_frameForward01.UseVisualStyleBackColor = true;
            this.button_frameForward01.Click += new System.EventHandler(this.button_frameShift_Click);
            // 
            // label_FrameCount
            // 
            this.label_FrameCount.Dock = System.Windows.Forms.DockStyle.Left;
            this.label_FrameCount.Location = new System.Drawing.Point(174, 0);
            this.label_FrameCount.Name = "label_FrameCount";
            this.label_FrameCount.Size = new System.Drawing.Size(79, 30);
            this.label_FrameCount.TabIndex = 7;
            this.label_FrameCount.Text = "...";
            this.label_FrameCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_frameBackward01
            // 
            this.button_frameBackward01.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_frameBackward01.Location = new System.Drawing.Point(116, 0);
            this.button_frameBackward01.Name = "button_frameBackward01";
            this.button_frameBackward01.Size = new System.Drawing.Size(58, 30);
            this.button_frameBackward01.TabIndex = 3;
            this.button_frameBackward01.Text = "<";
            this.button_frameBackward01.UseVisualStyleBackColor = true;
            this.button_frameBackward01.Click += new System.EventHandler(this.button_frameShift_Click);
            // 
            // button_frameBackward05
            // 
            this.button_frameBackward05.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_frameBackward05.Location = new System.Drawing.Point(58, 0);
            this.button_frameBackward05.Name = "button_frameBackward05";
            this.button_frameBackward05.Size = new System.Drawing.Size(58, 30);
            this.button_frameBackward05.TabIndex = 2;
            this.button_frameBackward05.Text = "<<";
            this.button_frameBackward05.UseVisualStyleBackColor = true;
            this.button_frameBackward05.Click += new System.EventHandler(this.button_frameShift_Click);
            // 
            // button_frameBackward10
            // 
            this.button_frameBackward10.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_frameBackward10.Location = new System.Drawing.Point(0, 0);
            this.button_frameBackward10.Name = "button_frameBackward10";
            this.button_frameBackward10.Size = new System.Drawing.Size(58, 30);
            this.button_frameBackward10.TabIndex = 1;
            this.button_frameBackward10.Text = "<<<";
            this.button_frameBackward10.UseVisualStyleBackColor = true;
            this.button_frameBackward10.Click += new System.EventHandler(this.button_frameShift_Click);
            // 
            // button_OpenMovieFile
            // 
            this.button_OpenMovieFile.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_OpenMovieFile.Location = new System.Drawing.Point(713, 0);
            this.button_OpenMovieFile.Name = "button_OpenMovieFile";
            this.button_OpenMovieFile.Size = new System.Drawing.Size(63, 30);
            this.button_OpenMovieFile.TabIndex = 0;
            this.button_OpenMovieFile.Text = "Open";
            this.button_OpenMovieFile.UseVisualStyleBackColor = true;
            this.button_OpenMovieFile.Click += new System.EventHandler(this.button_OpenMovieFile_Click);
            // 
            // trackBar_frameIndex
            // 
            this.trackBar_frameIndex.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBar_frameIndex.Location = new System.Drawing.Point(0, 0);
            this.trackBar_frameIndex.Name = "trackBar_frameIndex";
            this.trackBar_frameIndex.Size = new System.Drawing.Size(776, 45);
            this.trackBar_frameIndex.TabIndex = 0;
            this.trackBar_frameIndex.ValueChanged += new System.EventHandler(this.trackBar_frameIndex_ValueChanged);
            // 
            // panel_YOLOPOSE_TOP
            // 
            this.panel_YOLOPOSE_TOP.Controls.Add(this.textBox_modelFilePath);
            this.panel_YOLOPOSE_TOP.Controls.Add(this.button_OpenModelFile);
            this.panel_YOLOPOSE_TOP.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_YOLOPOSE_TOP.Location = new System.Drawing.Point(3, 15);
            this.panel_YOLOPOSE_TOP.Name = "panel_YOLOPOSE_TOP";
            this.panel_YOLOPOSE_TOP.Size = new System.Drawing.Size(776, 24);
            this.panel_YOLOPOSE_TOP.TabIndex = 1;
            // 
            // textBox_modelFilePath
            // 
            this.textBox_modelFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_modelFilePath.Location = new System.Drawing.Point(0, 0);
            this.textBox_modelFilePath.Name = "textBox_modelFilePath";
            this.textBox_modelFilePath.Size = new System.Drawing.Size(744, 19);
            this.textBox_modelFilePath.TabIndex = 1;
            this.textBox_modelFilePath.TextChanged += new System.EventHandler(this.textBox_modelFilePath_TextChanged);
            // 
            // button_OpenModelFile
            // 
            this.button_OpenModelFile.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_OpenModelFile.Location = new System.Drawing.Point(744, 0);
            this.button_OpenModelFile.Name = "button_OpenModelFile";
            this.button_OpenModelFile.Size = new System.Drawing.Size(32, 24);
            this.button_OpenModelFile.TabIndex = 0;
            this.button_OpenModelFile.Text = "...";
            this.button_OpenModelFile.UseVisualStyleBackColor = true;
            this.button_OpenModelFile.Click += new System.EventHandler(this.button_OpenModelFile_Click);
            // 
            // label_modelPath
            // 
            this.label_modelPath.AutoSize = true;
            this.label_modelPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_modelPath.Location = new System.Drawing.Point(3, 3);
            this.label_modelPath.Name = "label_modelPath";
            this.label_modelPath.Size = new System.Drawing.Size(58, 12);
            this.label_modelPath.TabIndex = 0;
            this.label_modelPath.Text = "modelPath";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(782, 784);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Setting";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // backgroundWorker_posePredict
            // 
            this.backgroundWorker_posePredict.WorkerReportsProgress = true;
            this.backgroundWorker_posePredict.WorkerSupportsCancellation = true;
            this.backgroundWorker_posePredict.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_posePredict_DoWork);
            this.backgroundWorker_posePredict.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_posePredict_ProgressChanged);
            this.backgroundWorker_posePredict.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_posePredict_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 810);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "ONNX";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_YOLOPOSE.ResumeLayout(false);
            this.tabPage_YOLOPOSE.PerformLayout();
            this.panel_Main.ResumeLayout(false);
            this.panel_Main.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.panel_YOLOPOSE_BOTTOM.ResumeLayout(false);
            this.panel_YOLOPOSE_BOTTOM.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_frameIndex)).EndInit();
            this.panel_YOLOPOSE_TOP.ResumeLayout(false);
            this.panel_YOLOPOSE_TOP.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_YOLOPOSE;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel_YOLOPOSE_TOP;
        private System.Windows.Forms.TextBox textBox_modelFilePath;
        private System.Windows.Forms.Button button_OpenModelFile;
        private System.Windows.Forms.Label label_modelPath;
        private System.Windows.Forms.Panel panel_YOLOPOSE_BOTTOM;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_OpenMovieFile;
        private System.Windows.Forms.TrackBar trackBar_frameIndex;
        private System.Windows.Forms.Panel panel_Main;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button button_frameForward10;
        private System.Windows.Forms.Button button_frameForward05;
        private System.Windows.Forms.Button button_frameForward01;
        private System.Windows.Forms.Button button_frameBackward01;
        private System.Windows.Forms.Button button_frameBackward05;
        private System.Windows.Forms.Button button_frameBackward10;
        private System.Windows.Forms.Label label_FrameCount;
        private System.Windows.Forms.Button button_Save;
        private System.ComponentModel.BackgroundWorker backgroundWorker_posePredict;
    }
}

