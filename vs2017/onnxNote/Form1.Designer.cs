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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_YOLOPOSE = new System.Windows.Forms.TabPage();
            this.panel_Main = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button_UnCheck = new System.Windows.Forms.Button();
            this.button_Check = new System.Windows.Forms.Button();
            this.dataGridView_PoseLines = new System.Windows.Forms.DataGridView();
            this.Column_Checked = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column_Frame = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_FrameContents = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_LoadPoseInfo = new System.Windows.Forms.Button();
            this.button_SaveFrameChecked = new System.Windows.Forms.Button();
            this.panel_YOLOPOSE_BOTTOM = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.trackBar_Conf = new System.Windows.Forms.TrackBar();
            this.label_ConfThreshold = new System.Windows.Forms.Label();
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
            this.textBox_PredictBatchSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_DeviceID = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker_posePredict = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage_YOLOPOSE.SuspendLayout();
            this.panel_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_PoseLines)).BeginInit();
            this.panel_YOLOPOSE_BOTTOM.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Conf)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_frameIndex)).BeginInit();
            this.panel_YOLOPOSE_TOP.SuspendLayout();
            this.tabPage2.SuspendLayout();
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
            this.tabControl1.Size = new System.Drawing.Size(938, 810);
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
            this.tabPage_YOLOPOSE.Size = new System.Drawing.Size(930, 784);
            this.tabPage_YOLOPOSE.TabIndex = 0;
            this.tabPage_YOLOPOSE.Text = "YOLOPOSE";
            this.tabPage_YOLOPOSE.UseVisualStyleBackColor = true;
            // 
            // panel_Main
            // 
            this.panel_Main.AutoScroll = true;
            this.panel_Main.Controls.Add(this.splitContainer1);
            this.panel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Main.Location = new System.Drawing.Point(3, 39);
            this.panel_Main.Name = "panel_Main";
            this.panel_Main.Size = new System.Drawing.Size(924, 667);
            this.panel_Main.TabIndex = 3;
            this.panel_Main.Resize += new System.EventHandler(this.panel_Main_Resize);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView_PoseLines);
            this.splitContainer1.Panel2.Controls.Add(this.button_LoadPoseInfo);
            this.splitContainer1.Panel2.Controls.Add(this.button_SaveFrameChecked);
            this.splitContainer1.Size = new System.Drawing.Size(924, 667);
            this.splitContainer1.SplitterDistance = 728;
            this.splitContainer1.TabIndex = 1;
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(38, 151);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(640, 360);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.Click += new System.EventHandler(this.pictureBox_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button_UnCheck);
            this.panel2.Controls.Add(this.button_Check);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 632);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(192, 35);
            this.panel2.TabIndex = 3;
            // 
            // button_UnCheck
            // 
            this.button_UnCheck.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_UnCheck.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button_UnCheck.Location = new System.Drawing.Point(62, 0);
            this.button_UnCheck.Name = "button_UnCheck";
            this.button_UnCheck.Size = new System.Drawing.Size(65, 35);
            this.button_UnCheck.TabIndex = 1;
            this.button_UnCheck.Text = "👉☐";
            this.button_UnCheck.UseVisualStyleBackColor = true;
            this.button_UnCheck.Click += new System.EventHandler(this.button_UnCheck_Click);
            // 
            // button_Check
            // 
            this.button_Check.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_Check.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button_Check.Location = new System.Drawing.Point(0, 0);
            this.button_Check.Margin = new System.Windows.Forms.Padding(0);
            this.button_Check.Name = "button_Check";
            this.button_Check.Size = new System.Drawing.Size(62, 35);
            this.button_Check.TabIndex = 0;
            this.button_Check.Text = "👉☑";
            this.button_Check.UseVisualStyleBackColor = true;
            this.button_Check.Click += new System.EventHandler(this.button_Check_Click);
            // 
            // dataGridView_PoseLines
            // 
            this.dataGridView_PoseLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_PoseLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_Checked,
            this.Column_Frame,
            this.Column_FrameContents});
            this.dataGridView_PoseLines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_PoseLines.Location = new System.Drawing.Point(0, 46);
            this.dataGridView_PoseLines.Name = "dataGridView_PoseLines";
            this.dataGridView_PoseLines.RowTemplate.Height = 21;
            this.dataGridView_PoseLines.Size = new System.Drawing.Size(192, 621);
            this.dataGridView_PoseLines.TabIndex = 0;
            this.dataGridView_PoseLines.CurrentCellChanged += new System.EventHandler(this.dataGridView_PoseLines_CurrentCellChanged);
            this.dataGridView_PoseLines.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_PoseLines_RowEnter);
            // 
            // Column_Checked
            // 
            this.Column_Checked.HeaderText = "";
            this.Column_Checked.Name = "Column_Checked";
            this.Column_Checked.Width = 24;
            // 
            // Column_Frame
            // 
            this.Column_Frame.HeaderText = "Frame";
            this.Column_Frame.Name = "Column_Frame";
            // 
            // Column_FrameContents
            // 
            this.Column_FrameContents.HeaderText = "FrameContents";
            this.Column_FrameContents.Name = "Column_FrameContents";
            this.Column_FrameContents.Visible = false;
            // 
            // button_LoadPoseInfo
            // 
            this.button_LoadPoseInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_LoadPoseInfo.Location = new System.Drawing.Point(0, 23);
            this.button_LoadPoseInfo.Name = "button_LoadPoseInfo";
            this.button_LoadPoseInfo.Size = new System.Drawing.Size(192, 23);
            this.button_LoadPoseInfo.TabIndex = 2;
            this.button_LoadPoseInfo.Text = "LoadPoseInfo";
            this.button_LoadPoseInfo.UseVisualStyleBackColor = true;
            this.button_LoadPoseInfo.Click += new System.EventHandler(this.button_LoadPoseInfo_Click);
            // 
            // button_SaveFrameChecked
            // 
            this.button_SaveFrameChecked.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_SaveFrameChecked.Location = new System.Drawing.Point(0, 0);
            this.button_SaveFrameChecked.Name = "button_SaveFrameChecked";
            this.button_SaveFrameChecked.Size = new System.Drawing.Size(192, 23);
            this.button_SaveFrameChecked.TabIndex = 1;
            this.button_SaveFrameChecked.Text = "SaveFrameChecked";
            this.button_SaveFrameChecked.UseVisualStyleBackColor = true;
            this.button_SaveFrameChecked.Click += new System.EventHandler(this.button_SaveFrameChecked_Click);
            // 
            // panel_YOLOPOSE_BOTTOM
            // 
            this.panel_YOLOPOSE_BOTTOM.Controls.Add(this.panel1);
            this.panel_YOLOPOSE_BOTTOM.Controls.Add(this.trackBar_frameIndex);
            this.panel_YOLOPOSE_BOTTOM.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_YOLOPOSE_BOTTOM.Location = new System.Drawing.Point(3, 706);
            this.panel_YOLOPOSE_BOTTOM.Name = "panel_YOLOPOSE_BOTTOM";
            this.panel_YOLOPOSE_BOTTOM.Size = new System.Drawing.Size(924, 75);
            this.panel_YOLOPOSE_BOTTOM.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.trackBar_Conf);
            this.panel1.Controls.Add(this.label_ConfThreshold);
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
            this.panel1.Size = new System.Drawing.Size(924, 30);
            this.panel1.TabIndex = 1;
            // 
            // trackBar_Conf
            // 
            this.trackBar_Conf.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBar_Conf.Location = new System.Drawing.Point(427, 0);
            this.trackBar_Conf.Maximum = 100;
            this.trackBar_Conf.Name = "trackBar_Conf";
            this.trackBar_Conf.Size = new System.Drawing.Size(341, 30);
            this.trackBar_Conf.TabIndex = 9;
            this.trackBar_Conf.TickFrequency = 10;
            this.trackBar_Conf.Value = 80;
            this.trackBar_Conf.Scroll += new System.EventHandler(this.trackBar_Conf_Scroll);
            this.trackBar_Conf.ValueChanged += new System.EventHandler(this.trackBar_Conf_ValueChanged);
            // 
            // label_ConfThreshold
            // 
            this.label_ConfThreshold.Dock = System.Windows.Forms.DockStyle.Right;
            this.label_ConfThreshold.Location = new System.Drawing.Point(768, 0);
            this.label_ConfThreshold.Name = "label_ConfThreshold";
            this.label_ConfThreshold.Size = new System.Drawing.Size(30, 30);
            this.label_ConfThreshold.TabIndex = 10;
            this.label_ConfThreshold.Text = "...";
            this.label_ConfThreshold.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_Save
            // 
            this.button_Save.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_Save.Location = new System.Drawing.Point(798, 0);
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
            this.button_OpenMovieFile.Location = new System.Drawing.Point(861, 0);
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
            this.trackBar_frameIndex.Size = new System.Drawing.Size(924, 45);
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
            this.panel_YOLOPOSE_TOP.Size = new System.Drawing.Size(924, 24);
            this.panel_YOLOPOSE_TOP.TabIndex = 1;
            // 
            // textBox_modelFilePath
            // 
            this.textBox_modelFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_modelFilePath.Location = new System.Drawing.Point(0, 0);
            this.textBox_modelFilePath.Name = "textBox_modelFilePath";
            this.textBox_modelFilePath.Size = new System.Drawing.Size(892, 19);
            this.textBox_modelFilePath.TabIndex = 1;
            this.textBox_modelFilePath.TextChanged += new System.EventHandler(this.textBox_modelFilePath_TextChanged);
            // 
            // button_OpenModelFile
            // 
            this.button_OpenModelFile.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_OpenModelFile.Location = new System.Drawing.Point(892, 0);
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
            this.tabPage2.AutoScroll = true;
            this.tabPage2.Controls.Add(this.textBox_PredictBatchSize);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.comboBox_DeviceID);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(930, 784);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Setting";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBox_PredictBatchSize
            // 
            this.textBox_PredictBatchSize.Location = new System.Drawing.Point(10, 112);
            this.textBox_PredictBatchSize.Name = "textBox_PredictBatchSize";
            this.textBox_PredictBatchSize.Size = new System.Drawing.Size(100, 19);
            this.textBox_PredictBatchSize.TabIndex = 3;
            this.textBox_PredictBatchSize.Text = "1024";
            this.textBox_PredictBatchSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Predict Batch Size";
            // 
            // comboBox_DeviceID
            // 
            this.comboBox_DeviceID.FormattingEnabled = true;
            this.comboBox_DeviceID.Items.AddRange(new object[] {
            "CPU",
            "GPU0",
            "GPU1"});
            this.comboBox_DeviceID.Location = new System.Drawing.Point(8, 43);
            this.comboBox_DeviceID.Name = "comboBox_DeviceID";
            this.comboBox_DeviceID.Size = new System.Drawing.Size(67, 20);
            this.comboBox_DeviceID.TabIndex = 1;
            this.comboBox_DeviceID.Text = "CPU";
            this.comboBox_DeviceID.SelectedIndexChanged += new System.EventHandler(this.comboBox_DeviceID_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "DeviceID";
            // 
            // backgroundWorker_posePredict
            // 
            this.backgroundWorker_posePredict.WorkerReportsProgress = true;
            this.backgroundWorker_posePredict.WorkerSupportsCancellation = true;
            this.backgroundWorker_posePredict.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_posePredict_DoWork);
            this.backgroundWorker_posePredict.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_posePredict_ProgressChanged);
            this.backgroundWorker_posePredict.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_posePredict_RunWorkerCompleted);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 810);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "ONNX";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_YOLOPOSE.ResumeLayout(false);
            this.tabPage_YOLOPOSE.PerformLayout();
            this.panel_Main.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_PoseLines)).EndInit();
            this.panel_YOLOPOSE_BOTTOM.ResumeLayout(false);
            this.panel_YOLOPOSE_BOTTOM.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Conf)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_frameIndex)).EndInit();
            this.panel_YOLOPOSE_TOP.ResumeLayout(false);
            this.panel_YOLOPOSE_TOP.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
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
        private System.Windows.Forms.TrackBar trackBar_Conf;
        private System.Windows.Forms.Label label_ConfThreshold;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_DeviceID;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox_PredictBatchSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridView_PoseLines;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column_Checked;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Frame;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_FrameContents;
        private System.Windows.Forms.Button button_SaveFrameChecked;
        private System.Windows.Forms.Button button_LoadPoseInfo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button_UnCheck;
        private System.Windows.Forms.Button button_Check;
    }
}

