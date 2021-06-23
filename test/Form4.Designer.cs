
namespace test
{
    partial class Form4
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.StateInfo = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.AnalysisTable = new System.Windows.Forms.DataGridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.StepTable = new System.Windows.Forms.DataGridView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Step = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StateStack = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SymbolStack = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InputSet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Production = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.state = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StateNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectSet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StateInfo)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AnalysisTable)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StepTable)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 718);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "产生式";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(6, 24);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(256, 688);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.StateInfo);
            this.groupBox2.Location = new System.Drawing.Point(298, 60);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(364, 348);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "状态信息";
            // 
            // StateInfo
            // 
            this.StateInfo.AllowUserToAddRows = false;
            this.StateInfo.AllowUserToDeleteRows = false;
            this.StateInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.StateInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.StateInfo.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.StateInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.StateInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.StateInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.StateInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StateNum,
            this.ProjectSet});
            this.StateInfo.Location = new System.Drawing.Point(8, 23);
            this.StateInfo.Name = "StateInfo";
            this.StateInfo.ReadOnly = true;
            this.StateInfo.RowHeadersVisible = false;
            this.StateInfo.RowHeadersWidth = 51;
            this.StateInfo.RowTemplate.Height = 27;
            this.StateInfo.Size = new System.Drawing.Size(348, 315);
            this.StateInfo.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.AnalysisTable);
            this.groupBox3.Location = new System.Drawing.Point(668, 61);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(536, 347);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "LR预测表";
            // 
            // AnalysisTable
            // 
            this.AnalysisTable.AllowUserToAddRows = false;
            this.AnalysisTable.AllowUserToDeleteRows = false;
            this.AnalysisTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.AnalysisTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.AnalysisTable.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.AnalysisTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.AnalysisTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.AnalysisTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AnalysisTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.state,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6});
            this.AnalysisTable.Location = new System.Drawing.Point(6, 22);
            this.AnalysisTable.Name = "AnalysisTable";
            this.AnalysisTable.ReadOnly = true;
            this.AnalysisTable.RowHeadersVisible = false;
            this.AnalysisTable.RowHeadersWidth = 51;
            this.AnalysisTable.RowTemplate.Height = 27;
            this.AnalysisTable.Size = new System.Drawing.Size(524, 315);
            this.AnalysisTable.TabIndex = 1;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.StepTable);
            this.groupBox4.Location = new System.Drawing.Point(298, 455);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(906, 323);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "分析表";
            // 
            // StepTable
            // 
            this.StepTable.AllowUserToAddRows = false;
            this.StepTable.AllowUserToDeleteRows = false;
            this.StepTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.StepTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.StepTable.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.StepTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.StepTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.StepTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Step,
            this.StateStack,
            this.SymbolStack,
            this.InputSet,
            this.Production});
            this.StepTable.Location = new System.Drawing.Point(8, 24);
            this.StepTable.Name = "StepTable";
            this.StepTable.ReadOnly = true;
            this.StepTable.RowHeadersVisible = false;
            this.StepTable.RowHeadersWidth = 51;
            this.StepTable.RowTemplate.Height = 27;
            this.StepTable.Size = new System.Drawing.Size(892, 293);
            this.StepTable.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(369, 423);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(481, 25);
            this.textBox1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(293, 429);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "分析句子";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(1098, 423);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(100, 26);
            this.button7.TabIndex = 15;
            this.button7.Text = "单步显示";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(992, 423);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(100, 26);
            this.button6.TabIndex = 14;
            this.button6.Text = "一键显示";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(1068, 27);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(130, 30);
            this.button5.TabIndex = 20;
            this.button5.Text = "退出";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(915, 27);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(130, 30);
            this.button4.TabIndex = 19;
            this.button4.Text = "构造预测分析表";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(150, 25);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(130, 30);
            this.button3.TabIndex = 18;
            this.button3.Text = "保存文件";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(286, 25);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(130, 30);
            this.button2.TabIndex = 17;
            this.button2.Text = "确认文件";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 25);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 30);
            this.button1.TabIndex = 16;
            this.button1.Text = "打开文件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "LR(0)分析",
            "SLR(1)分析",
            "LR(1)分析"});
            this.comboBox1.Location = new System.Drawing.Point(778, 29);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(131, 25);
            this.comboBox1.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(657, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 15);
            this.label2.TabIndex = 22;
            this.label2.Text = "选择分析方式";
            // 
            // Step
            // 
            this.Step.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Step.DefaultCellStyle = dataGridViewCellStyle5;
            this.Step.HeaderText = "步骤";
            this.Step.MinimumWidth = 6;
            this.Step.Name = "Step";
            this.Step.ReadOnly = true;
            this.Step.Width = 66;
            // 
            // StateStack
            // 
            this.StateStack.HeaderText = "状态栈";
            this.StateStack.MinimumWidth = 6;
            this.StateStack.Name = "StateStack";
            this.StateStack.ReadOnly = true;
            // 
            // SymbolStack
            // 
            this.SymbolStack.HeaderText = "符号栈";
            this.SymbolStack.MinimumWidth = 6;
            this.SymbolStack.Name = "SymbolStack";
            this.SymbolStack.ReadOnly = true;
            // 
            // InputSet
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.InputSet.DefaultCellStyle = dataGridViewCellStyle6;
            this.InputSet.HeaderText = "输入串";
            this.InputSet.MinimumWidth = 6;
            this.InputSet.Name = "InputSet";
            this.InputSet.ReadOnly = true;
            // 
            // Production
            // 
            this.Production.HeaderText = "所用产生式";
            this.Production.MinimumWidth = 6;
            this.Production.Name = "Production";
            this.Production.ReadOnly = true;
            // 
            // state
            // 
            this.state.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.state.DefaultCellStyle = dataGridViewCellStyle4;
            this.state.HeaderText = "状态";
            this.state.MinimumWidth = 6;
            this.state.Name = "state";
            this.state.ReadOnly = true;
            this.state.Width = 66;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "a";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "b";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "c";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "d";
            this.Column4.MinimumWidth = 6;
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "#";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "E";
            this.Column6.MinimumWidth = 6;
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // StateNum
            // 
            this.StateNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.StateNum.DefaultCellStyle = dataGridViewCellStyle2;
            this.StateNum.HeaderText = "状态编号";
            this.StateNum.MinimumWidth = 6;
            this.StateNum.Name = "StateNum";
            this.StateNum.ReadOnly = true;
            this.StateNum.Width = 96;
            // 
            // ProjectSet
            // 
            this.ProjectSet.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ProjectSet.HeaderText = "项目集";
            this.ProjectSet.MinimumWidth = 6;
            this.ProjectSet.Name = "ProjectSet";
            this.ProjectSet.ReadOnly = true;
            this.ProjectSet.Width = 81;
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1223, 807);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form4";
            this.Text = "LR(1)分析";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StateInfo)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AnalysisTable)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StepTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView StateInfo;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView AnalysisTable;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView StepTable;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn StateNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn state;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Step;
        private System.Windows.Forms.DataGridViewTextBoxColumn StateStack;
        private System.Windows.Forms.DataGridViewTextBoxColumn SymbolStack;
        private System.Windows.Forms.DataGridViewTextBoxColumn InputSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn Production;
    }
}