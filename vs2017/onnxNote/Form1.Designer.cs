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
            this.dataGridView_PoseLines = new System.Windows.Forms.DataGridView();
            this.Column_Checked = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Filename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Frame = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_FrameContents = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Label = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button_CopyFromTop = new System.Windows.Forms.Button();
            this.button_UnCheck = new System.Windows.Forms.Button();
            this.button_Check = new System.Windows.Forms.Button();
            this.button_LoadPoseInfo = new System.Windows.Forms.Button();
            this.button_SaveFrameChecked = new System.Windows.Forms.Button();
            this.panel_Left = new System.Windows.Forms.Panel();
            this.textBox_PoseInfo = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.button_SwitchOverLapShoulder = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.numericUpDown_shoulderOverlapTh = new System.Windows.Forms.NumericUpDown();
            this.panel5 = new System.Windows.Forms.Panel();
            this.button_SwitchOverLapTolso = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDown_tolsoOverlapTh = new System.Windows.Forms.NumericUpDown();
            this.panel4 = new System.Windows.Forms.Panel();
            this.button_SwitchOverLapBbox = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDown_bboxOverlapTh = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
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
            this.panel_topDirectoryPath = new System.Windows.Forms.Panel();
            this.textBox_topDirectoryPath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel_modelPath = new System.Windows.Forms.Panel();
            this.textBox_modelFilePath = new System.Windows.Forms.TextBox();
            this.label_modelPath = new System.Windows.Forms.Label();
            this.button_OpenModelFile = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button_LoadWorkSetting = new System.Windows.Forms.Button();
            this.button_SaveWorkSetting = new System.Windows.Forms.Button();
            this.textBox_WorkTitle = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_LabelList = new System.Windows.Forms.TextBox();
            this.textBox_PredictBatchSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_DeviceID = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_PoseLines)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel_Left.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_shoulderOverlapTh)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_tolsoOverlapTh)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_bboxOverlapTh)).BeginInit();
            this.panel_YOLOPOSE_BOTTOM.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Conf)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_frameIndex)).BeginInit();
            this.panel_YOLOPOSE_TOP.SuspendLayout();
            this.panel_topDirectoryPath.SuspendLayout();
            this.panel_modelPath.SuspendLayout();
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
            this.tabControl1.Size = new System.Drawing.Size(1223, 821);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage_YOLOPOSE
            // 
            this.tabPage_YOLOPOSE.Controls.Add(this.panel_Main);
            this.tabPage_YOLOPOSE.Controls.Add(this.panel_Left);
            this.tabPage_YOLOPOSE.Controls.Add(this.panel_YOLOPOSE_BOTTOM);
            this.tabPage_YOLOPOSE.Controls.Add(this.panel_YOLOPOSE_TOP);
            this.tabPage_YOLOPOSE.Location = new System.Drawing.Point(4, 22);
            this.tabPage_YOLOPOSE.Name = "tabPage_YOLOPOSE";
            this.tabPage_YOLOPOSE.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_YOLOPOSE.Size = new System.Drawing.Size(1215, 795);
            this.tabPage_YOLOPOSE.TabIndex = 0;
            this.tabPage_YOLOPOSE.Text = "YOLOPOSE";
            this.tabPage_YOLOPOSE.UseVisualStyleBackColor = true;
            this.tabPage_YOLOPOSE.Enter += new System.EventHandler(this.tabPage_YOLOPOSE_Enter);
            // 
            // panel_Main
            // 
            this.panel_Main.AutoScroll = true;
            this.panel_Main.Controls.Add(this.splitContainer1);
            this.panel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Main.Location = new System.Drawing.Point(3, 50);
            this.panel_Main.Name = "panel_Main";
            this.panel_Main.Size = new System.Drawing.Size(1032, 667);
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
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView_PoseLines);
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Panel2.Controls.Add(this.button_LoadPoseInfo);
            this.splitContainer1.Panel2.Controls.Add(this.button_SaveFrameChecked);
            this.splitContainer1.Size = new System.Drawing.Size(1032, 667);
            this.splitContainer1.SplitterDistance = 720;
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
            // dataGridView_PoseLines
            // 
            this.dataGridView_PoseLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_PoseLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_Checked,
            this.Filename,
            this.Column_Frame,
            this.Column_FrameContents,
            this.Column_Label});
            this.dataGridView_PoseLines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_PoseLines.Location = new System.Drawing.Point(0, 46);
            this.dataGridView_PoseLines.Name = "dataGridView_PoseLines";
            this.dataGridView_PoseLines.RowHeadersWidth = 26;
            this.dataGridView_PoseLines.RowTemplate.Height = 21;
            this.dataGridView_PoseLines.Size = new System.Drawing.Size(308, 586);
            this.dataGridView_PoseLines.TabIndex = 0;
            this.dataGridView_PoseLines.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_PoseLines_CellMouseEnter);
            this.dataGridView_PoseLines.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView_PoseLines_CellValidating);
            this.dataGridView_PoseLines.CurrentCellChanged += new System.EventHandler(this.dataGridView_PoseLines_CurrentCellChanged);
            this.dataGridView_PoseLines.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_PoseLines_RowEnter);
            // 
            // Column_Checked
            // 
            this.Column_Checked.HeaderText = "";
            this.Column_Checked.Name = "Column_Checked";
            this.Column_Checked.Width = 24;
            // 
            // Filename
            // 
            this.Filename.HeaderText = "File";
            this.Filename.Name = "Filename";
            this.Filename.Width = 70;
            // 
            // Column_Frame
            // 
            this.Column_Frame.HeaderText = "Frame";
            this.Column_Frame.Name = "Column_Frame";
            this.Column_Frame.Width = 60;
            // 
            // Column_FrameContents
            // 
            this.Column_FrameContents.HeaderText = "FrameContents";
            this.Column_FrameContents.Name = "Column_FrameContents";
            this.Column_FrameContents.Visible = false;
            // 
            // Column_Label
            // 
            this.Column_Label.HeaderText = "Label";
            this.Column_Label.Name = "Column_Label";
            this.Column_Label.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button_CopyFromTop);
            this.panel2.Controls.Add(this.button_UnCheck);
            this.panel2.Controls.Add(this.button_Check);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 632);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(308, 35);
            this.panel2.TabIndex = 3;
            // 
            // button_CopyFromTop
            // 
            this.button_CopyFromTop.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_CopyFromTop.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button_CopyFromTop.Location = new System.Drawing.Point(127, 0);
            this.button_CopyFromTop.Name = "button_CopyFromTop";
            this.button_CopyFromTop.Size = new System.Drawing.Size(65, 35);
            this.button_CopyFromTop.TabIndex = 2;
            this.button_CopyFromTop.Text = "👉▶⬇";
            this.button_CopyFromTop.UseVisualStyleBackColor = true;
            this.button_CopyFromTop.Click += new System.EventHandler(this.button_CopyFromTop_Click);
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
            // button_LoadPoseInfo
            // 
            this.button_LoadPoseInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_LoadPoseInfo.Location = new System.Drawing.Point(0, 23);
            this.button_LoadPoseInfo.Name = "button_LoadPoseInfo";
            this.button_LoadPoseInfo.Size = new System.Drawing.Size(308, 23);
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
            this.button_SaveFrameChecked.Size = new System.Drawing.Size(308, 23);
            this.button_SaveFrameChecked.TabIndex = 1;
            this.button_SaveFrameChecked.Text = "SaveFrameChecked";
            this.button_SaveFrameChecked.UseVisualStyleBackColor = true;
            this.button_SaveFrameChecked.Click += new System.EventHandler(this.button_SaveFrameChecked_Click);
            // 
            // panel_Left
            // 
            this.panel_Left.Controls.Add(this.textBox_PoseInfo);
            this.panel_Left.Controls.Add(this.panel3);
            this.panel_Left.Controls.Add(this.label5);
            this.panel_Left.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_Left.Location = new System.Drawing.Point(1035, 50);
            this.panel_Left.Name = "panel_Left";
            this.panel_Left.Size = new System.Drawing.Size(177, 667);
            this.panel_Left.TabIndex = 1;
            // 
            // textBox_PoseInfo
            // 
            this.textBox_PoseInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_PoseInfo.Location = new System.Drawing.Point(0, 12);
            this.textBox_PoseInfo.Multiline = true;
            this.textBox_PoseInfo.Name = "textBox_PoseInfo";
            this.textBox_PoseInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_PoseInfo.Size = new System.Drawing.Size(177, 535);
            this.textBox_PoseInfo.TabIndex = 1;
            this.textBox_PoseInfo.WordWrap = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel7);
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 547);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(177, 120);
            this.panel3.TabIndex = 2;
            // 
            // panel7
            // 
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 90);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(177, 30);
            this.panel7.TabIndex = 2;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.button_SwitchOverLapShoulder);
            this.panel6.Controls.Add(this.label9);
            this.panel6.Controls.Add(this.numericUpDown_shoulderOverlapTh);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 60);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(177, 30);
            this.panel6.TabIndex = 1;
            // 
            // button_SwitchOverLapShoulder
            // 
            this.button_SwitchOverLapShoulder.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_SwitchOverLapShoulder.Location = new System.Drawing.Point(84, 0);
            this.button_SwitchOverLapShoulder.Name = "button_SwitchOverLapShoulder";
            this.button_SwitchOverLapShoulder.Size = new System.Drawing.Size(21, 30);
            this.button_SwitchOverLapShoulder.TabIndex = 4;
            this.button_SwitchOverLapShoulder.UseVisualStyleBackColor = true;
            this.button_SwitchOverLapShoulder.Click += new System.EventHandler(this.button_SwitchOverLapShoulder_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Left;
            this.label9.Location = new System.Drawing.Point(0, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 12);
            this.label9.TabIndex = 3;
            this.label9.Text = "shoulder O.L.";
            // 
            // numericUpDown_shoulderOverlapTh
            // 
            this.numericUpDown_shoulderOverlapTh.DecimalPlaces = 2;
            this.numericUpDown_shoulderOverlapTh.Dock = System.Windows.Forms.DockStyle.Right;
            this.numericUpDown_shoulderOverlapTh.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numericUpDown_shoulderOverlapTh.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown_shoulderOverlapTh.Location = new System.Drawing.Point(105, 0);
            this.numericUpDown_shoulderOverlapTh.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_shoulderOverlapTh.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDown_shoulderOverlapTh.Name = "numericUpDown_shoulderOverlapTh";
            this.numericUpDown_shoulderOverlapTh.Size = new System.Drawing.Size(72, 26);
            this.numericUpDown_shoulderOverlapTh.TabIndex = 2;
            this.numericUpDown_shoulderOverlapTh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown_shoulderOverlapTh.Value = new decimal(new int[] {
            8,
            0,
            0,
            65536});
            this.numericUpDown_shoulderOverlapTh.ValueChanged += new System.EventHandler(this.numericUpDown_shoulderOverlapTh_ValueChanged);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.button_SwitchOverLapTolso);
            this.panel5.Controls.Add(this.label8);
            this.panel5.Controls.Add(this.numericUpDown_tolsoOverlapTh);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 30);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(177, 30);
            this.panel5.TabIndex = 1;
            // 
            // button_SwitchOverLapTolso
            // 
            this.button_SwitchOverLapTolso.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_SwitchOverLapTolso.Location = new System.Drawing.Point(84, 0);
            this.button_SwitchOverLapTolso.Name = "button_SwitchOverLapTolso";
            this.button_SwitchOverLapTolso.Size = new System.Drawing.Size(21, 30);
            this.button_SwitchOverLapTolso.TabIndex = 4;
            this.button_SwitchOverLapTolso.UseVisualStyleBackColor = true;
            this.button_SwitchOverLapTolso.Click += new System.EventHandler(this.button_SwitchOverLapTolso_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Left;
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 12);
            this.label8.TabIndex = 3;
            this.label8.Text = "tolso O.L.";
            // 
            // numericUpDown_tolsoOverlapTh
            // 
            this.numericUpDown_tolsoOverlapTh.DecimalPlaces = 2;
            this.numericUpDown_tolsoOverlapTh.Dock = System.Windows.Forms.DockStyle.Right;
            this.numericUpDown_tolsoOverlapTh.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numericUpDown_tolsoOverlapTh.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown_tolsoOverlapTh.Location = new System.Drawing.Point(105, 0);
            this.numericUpDown_tolsoOverlapTh.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_tolsoOverlapTh.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDown_tolsoOverlapTh.Name = "numericUpDown_tolsoOverlapTh";
            this.numericUpDown_tolsoOverlapTh.Size = new System.Drawing.Size(72, 26);
            this.numericUpDown_tolsoOverlapTh.TabIndex = 2;
            this.numericUpDown_tolsoOverlapTh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown_tolsoOverlapTh.Value = new decimal(new int[] {
            8,
            0,
            0,
            65536});
            this.numericUpDown_tolsoOverlapTh.ValueChanged += new System.EventHandler(this.numericUpDown_tolsoOverlapTh_ValueChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.button_SwitchOverLapBbox);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.numericUpDown_bboxOverlapTh);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(177, 30);
            this.panel4.TabIndex = 0;
            // 
            // button_SwitchOverLapBbox
            // 
            this.button_SwitchOverLapBbox.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_SwitchOverLapBbox.Location = new System.Drawing.Point(84, 0);
            this.button_SwitchOverLapBbox.Name = "button_SwitchOverLapBbox";
            this.button_SwitchOverLapBbox.Size = new System.Drawing.Size(21, 30);
            this.button_SwitchOverLapBbox.TabIndex = 2;
            this.button_SwitchOverLapBbox.UseVisualStyleBackColor = true;
            this.button_SwitchOverLapBbox.Click += new System.EventHandler(this.button_SwitchOverLapBbox_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Left;
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "bbox O.L.";
            // 
            // numericUpDown_bboxOverlapTh
            // 
            this.numericUpDown_bboxOverlapTh.DecimalPlaces = 2;
            this.numericUpDown_bboxOverlapTh.Dock = System.Windows.Forms.DockStyle.Right;
            this.numericUpDown_bboxOverlapTh.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numericUpDown_bboxOverlapTh.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown_bboxOverlapTh.Location = new System.Drawing.Point(105, 0);
            this.numericUpDown_bboxOverlapTh.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_bboxOverlapTh.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericUpDown_bboxOverlapTh.Name = "numericUpDown_bboxOverlapTh";
            this.numericUpDown_bboxOverlapTh.Size = new System.Drawing.Size(72, 26);
            this.numericUpDown_bboxOverlapTh.TabIndex = 0;
            this.numericUpDown_bboxOverlapTh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown_bboxOverlapTh.Value = new decimal(new int[] {
            8,
            0,
            0,
            65536});
            this.numericUpDown_bboxOverlapTh.ValueChanged += new System.EventHandler(this.numericUpDown_bboxOverlapTh_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "PoseInfo";
            // 
            // panel_YOLOPOSE_BOTTOM
            // 
            this.panel_YOLOPOSE_BOTTOM.Controls.Add(this.panel1);
            this.panel_YOLOPOSE_BOTTOM.Controls.Add(this.trackBar_frameIndex);
            this.panel_YOLOPOSE_BOTTOM.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_YOLOPOSE_BOTTOM.Location = new System.Drawing.Point(3, 717);
            this.panel_YOLOPOSE_BOTTOM.Name = "panel_YOLOPOSE_BOTTOM";
            this.panel_YOLOPOSE_BOTTOM.Size = new System.Drawing.Size(1209, 75);
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
            this.panel1.Size = new System.Drawing.Size(1209, 30);
            this.panel1.TabIndex = 1;
            // 
            // trackBar_Conf
            // 
            this.trackBar_Conf.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBar_Conf.Location = new System.Drawing.Point(427, 0);
            this.trackBar_Conf.Maximum = 100;
            this.trackBar_Conf.Name = "trackBar_Conf";
            this.trackBar_Conf.Size = new System.Drawing.Size(626, 30);
            this.trackBar_Conf.TabIndex = 9;
            this.trackBar_Conf.TickFrequency = 10;
            this.trackBar_Conf.Value = 80;
            this.trackBar_Conf.Scroll += new System.EventHandler(this.trackBar_Conf_Scroll);
            this.trackBar_Conf.ValueChanged += new System.EventHandler(this.trackBar_Conf_ValueChanged);
            // 
            // label_ConfThreshold
            // 
            this.label_ConfThreshold.Dock = System.Windows.Forms.DockStyle.Right;
            this.label_ConfThreshold.Location = new System.Drawing.Point(1053, 0);
            this.label_ConfThreshold.Name = "label_ConfThreshold";
            this.label_ConfThreshold.Size = new System.Drawing.Size(30, 30);
            this.label_ConfThreshold.TabIndex = 10;
            this.label_ConfThreshold.Text = "...";
            this.label_ConfThreshold.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_Save
            // 
            this.button_Save.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_Save.Location = new System.Drawing.Point(1083, 0);
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
            this.button_OpenMovieFile.Location = new System.Drawing.Point(1146, 0);
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
            this.trackBar_frameIndex.Size = new System.Drawing.Size(1209, 45);
            this.trackBar_frameIndex.TabIndex = 0;
            this.trackBar_frameIndex.ValueChanged += new System.EventHandler(this.trackBar_frameIndex_ValueChanged);
            // 
            // panel_YOLOPOSE_TOP
            // 
            this.panel_YOLOPOSE_TOP.Controls.Add(this.panel_topDirectoryPath);
            this.panel_YOLOPOSE_TOP.Controls.Add(this.panel_modelPath);
            this.panel_YOLOPOSE_TOP.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_YOLOPOSE_TOP.Location = new System.Drawing.Point(3, 3);
            this.panel_YOLOPOSE_TOP.Name = "panel_YOLOPOSE_TOP";
            this.panel_YOLOPOSE_TOP.Size = new System.Drawing.Size(1209, 47);
            this.panel_YOLOPOSE_TOP.TabIndex = 1;
            // 
            // panel_topDirectoryPath
            // 
            this.panel_topDirectoryPath.Controls.Add(this.textBox_topDirectoryPath);
            this.panel_topDirectoryPath.Controls.Add(this.label6);
            this.panel_topDirectoryPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_topDirectoryPath.Location = new System.Drawing.Point(0, 22);
            this.panel_topDirectoryPath.Name = "panel_topDirectoryPath";
            this.panel_topDirectoryPath.Size = new System.Drawing.Size(1209, 22);
            this.panel_topDirectoryPath.TabIndex = 5;
            // 
            // textBox_topDirectoryPath
            // 
            this.textBox_topDirectoryPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox_topDirectoryPath.Location = new System.Drawing.Point(101, 0);
            this.textBox_topDirectoryPath.Name = "textBox_topDirectoryPath";
            this.textBox_topDirectoryPath.Size = new System.Drawing.Size(1108, 19);
            this.textBox_topDirectoryPath.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.label6.Size = new System.Drawing.Size(101, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "topDirectoryPath";
            // 
            // panel_modelPath
            // 
            this.panel_modelPath.Controls.Add(this.textBox_modelFilePath);
            this.panel_modelPath.Controls.Add(this.label_modelPath);
            this.panel_modelPath.Controls.Add(this.button_OpenModelFile);
            this.panel_modelPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_modelPath.Location = new System.Drawing.Point(0, 0);
            this.panel_modelPath.Name = "panel_modelPath";
            this.panel_modelPath.Size = new System.Drawing.Size(1209, 22);
            this.panel_modelPath.TabIndex = 4;
            // 
            // textBox_modelFilePath
            // 
            this.textBox_modelFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_modelFilePath.Location = new System.Drawing.Point(68, 0);
            this.textBox_modelFilePath.Name = "textBox_modelFilePath";
            this.textBox_modelFilePath.Size = new System.Drawing.Size(1109, 19);
            this.textBox_modelFilePath.TabIndex = 1;
            this.textBox_modelFilePath.TextChanged += new System.EventHandler(this.textBox_modelFilePath_TextChanged);
            // 
            // label_modelPath
            // 
            this.label_modelPath.AutoSize = true;
            this.label_modelPath.Dock = System.Windows.Forms.DockStyle.Left;
            this.label_modelPath.Location = new System.Drawing.Point(0, 0);
            this.label_modelPath.Name = "label_modelPath";
            this.label_modelPath.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.label_modelPath.Size = new System.Drawing.Size(68, 12);
            this.label_modelPath.TabIndex = 0;
            this.label_modelPath.Text = "modelPath";
            // 
            // button_OpenModelFile
            // 
            this.button_OpenModelFile.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_OpenModelFile.Location = new System.Drawing.Point(1177, 0);
            this.button_OpenModelFile.Name = "button_OpenModelFile";
            this.button_OpenModelFile.Size = new System.Drawing.Size(32, 22);
            this.button_OpenModelFile.TabIndex = 0;
            this.button_OpenModelFile.Text = "...";
            this.button_OpenModelFile.UseVisualStyleBackColor = true;
            this.button_OpenModelFile.Click += new System.EventHandler(this.button_OpenModelFile_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.AutoScroll = true;
            this.tabPage2.Controls.Add(this.button_LoadWorkSetting);
            this.tabPage2.Controls.Add(this.button_SaveWorkSetting);
            this.tabPage2.Controls.Add(this.textBox_WorkTitle);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.textBox_LabelList);
            this.tabPage2.Controls.Add(this.textBox_PredictBatchSize);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.comboBox_DeviceID);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1215, 795);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Setting";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button_LoadWorkSetting
            // 
            this.button_LoadWorkSetting.Location = new System.Drawing.Point(91, 52);
            this.button_LoadWorkSetting.Name = "button_LoadWorkSetting";
            this.button_LoadWorkSetting.Size = new System.Drawing.Size(75, 23);
            this.button_LoadWorkSetting.TabIndex = 7;
            this.button_LoadWorkSetting.Text = "Load";
            this.button_LoadWorkSetting.UseVisualStyleBackColor = true;
            this.button_LoadWorkSetting.Click += new System.EventHandler(this.button_LoadWorkSetting_Click);
            // 
            // button_SaveWorkSetting
            // 
            this.button_SaveWorkSetting.Location = new System.Drawing.Point(10, 52);
            this.button_SaveWorkSetting.Name = "button_SaveWorkSetting";
            this.button_SaveWorkSetting.Size = new System.Drawing.Size(75, 23);
            this.button_SaveWorkSetting.TabIndex = 7;
            this.button_SaveWorkSetting.Text = "Save";
            this.button_SaveWorkSetting.UseVisualStyleBackColor = true;
            this.button_SaveWorkSetting.Click += new System.EventHandler(this.button_SaveWorkSetting_Click);
            // 
            // textBox_WorkTitle
            // 
            this.textBox_WorkTitle.Location = new System.Drawing.Point(10, 27);
            this.textBox_WorkTitle.Name = "textBox_WorkTitle";
            this.textBox_WorkTitle.Size = new System.Drawing.Size(288, 19);
            this.textBox_WorkTitle.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "WorklTitle";
            // 
            // textBox_LabelList
            // 
            this.textBox_LabelList.Location = new System.Drawing.Point(147, 119);
            this.textBox_LabelList.Multiline = true;
            this.textBox_LabelList.Name = "textBox_LabelList";
            this.textBox_LabelList.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_LabelList.Size = new System.Drawing.Size(151, 226);
            this.textBox_LabelList.TabIndex = 4;
            this.textBox_LabelList.WordWrap = false;
            // 
            // textBox_PredictBatchSize
            // 
            this.textBox_PredictBatchSize.Location = new System.Drawing.Point(10, 188);
            this.textBox_PredictBatchSize.Name = "textBox_PredictBatchSize";
            this.textBox_PredictBatchSize.Size = new System.Drawing.Size(100, 19);
            this.textBox_PredictBatchSize.TabIndex = 3;
            this.textBox_PredictBatchSize.Text = "1024";
            this.textBox_PredictBatchSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 173);
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
            this.comboBox_DeviceID.Location = new System.Drawing.Point(8, 119);
            this.comboBox_DeviceID.Name = "comboBox_DeviceID";
            this.comboBox_DeviceID.Size = new System.Drawing.Size(67, 20);
            this.comboBox_DeviceID.TabIndex = 1;
            this.comboBox_DeviceID.Text = "CPU";
            this.comboBox_DeviceID.SelectedIndexChanged += new System.EventHandler(this.comboBox_DeviceID_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(145, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "LabelList";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 104);
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
            this.ClientSize = new System.Drawing.Size(1223, 821);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "ONNX";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_YOLOPOSE.ResumeLayout(false);
            this.panel_Main.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_PoseLines)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel_Left.ResumeLayout(false);
            this.panel_Left.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_shoulderOverlapTh)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_tolsoOverlapTh)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_bboxOverlapTh)).EndInit();
            this.panel_YOLOPOSE_BOTTOM.ResumeLayout(false);
            this.panel_YOLOPOSE_BOTTOM.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Conf)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_frameIndex)).EndInit();
            this.panel_YOLOPOSE_TOP.ResumeLayout(false);
            this.panel_topDirectoryPath.ResumeLayout(false);
            this.panel_topDirectoryPath.PerformLayout();
            this.panel_modelPath.ResumeLayout(false);
            this.panel_modelPath.PerformLayout();
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
        private System.Windows.Forms.Button button_SaveFrameChecked;
        private System.Windows.Forms.Button button_LoadPoseInfo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button_UnCheck;
        private System.Windows.Forms.Button button_Check;
        private System.Windows.Forms.TextBox textBox_LabelList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_CopyFromTop;
        private System.Windows.Forms.TextBox textBox_WorkTitle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_LoadWorkSetting;
        private System.Windows.Forms.Button button_SaveWorkSetting;
        private System.Windows.Forms.Panel panel_Left;
        private System.Windows.Forms.TextBox textBox_PoseInfo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel_topDirectoryPath;
        private System.Windows.Forms.TextBox textBox_topDirectoryPath;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel_modelPath;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column_Checked;
        private System.Windows.Forms.DataGridViewTextBoxColumn Filename;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Frame;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_FrameContents;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column_Label;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.NumericUpDown numericUpDown_bboxOverlapTh;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numericUpDown_shoulderOverlapTh;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDown_tolsoOverlapTh;
        private System.Windows.Forms.Button button_SwitchOverLapShoulder;
        private System.Windows.Forms.Button button_SwitchOverLapTolso;
        private System.Windows.Forms.Button button_SwitchOverLapBbox;
    }
}

