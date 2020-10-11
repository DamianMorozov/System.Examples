namespace WinForms.System.MemoryLimit
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private global::System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonCreate = new global::System.Windows.Forms.Button();
            this.richTextBoxTask = new global::System.Windows.Forms.RichTextBox();
            this.buttonDispose = new global::System.Windows.Forms.Button();
            this.buttonMoreMemory = new global::System.Windows.Forms.Button();
            this.numericUpDown = new global::System.Windows.Forms.NumericUpDown();
            this.numericUpDownMemoryLimit = new global::System.Windows.Forms.NumericUpDown();
            this.checkBoxMemoryLimit = new global::System.Windows.Forms.CheckBox();
            this.comboBoxMemoryLimit = new global::System.Windows.Forms.ComboBox();
            this.richTextBoxMoreMemory = new global::System.Windows.Forms.RichTextBox();
            this.checkBoxSimulateMode = new global::System.Windows.Forms.CheckBox();
            this.labelMb1 = new global::System.Windows.Forms.Label();
            this.labelMb2 = new global::System.Windows.Forms.Label();
            ((global::System.ComponentModel.ISupportInitialize)(this.numericUpDown)).BeginInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.numericUpDownMemoryLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCreate
            // 
            this.buttonCreate.Location = new global::System.Drawing.Point(16, 15);
            this.buttonCreate.Margin = new global::System.Windows.Forms.Padding(4);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new global::System.Drawing.Size(265, 28);
            this.buttonCreate.TabIndex = 0;
            this.buttonCreate.Text = "Create task for memory limit";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new global::System.EventHandler(this.buttonCreate_Click);
            // 
            // richTextBoxTask
            // 
            this.richTextBoxTask.Dock = global::System.Windows.Forms.DockStyle.Bottom;
            this.richTextBoxTask.Enabled = false;
            this.richTextBoxTask.Font = new global::System.Drawing.Font("Courier New", 9F, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBoxTask.Location = new global::System.Drawing.Point(0, 212);
            this.richTextBoxTask.Margin = new global::System.Windows.Forms.Padding(4);
            this.richTextBoxTask.Name = "richTextBoxTask";
            this.richTextBoxTask.ReadOnly = true;
            this.richTextBoxTask.ScrollBars = global::System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBoxTask.Size = new global::System.Drawing.Size(634, 100);
            this.richTextBoxTask.TabIndex = 7;
            this.richTextBoxTask.Text = "";
            // 
            // buttonDispose
            // 
            this.buttonDispose.Location = new global::System.Drawing.Point(304, 15);
            this.buttonDispose.Margin = new global::System.Windows.Forms.Padding(4);
            this.buttonDispose.Name = "buttonDispose";
            this.buttonDispose.Size = new global::System.Drawing.Size(305, 28);
            this.buttonDispose.TabIndex = 1;
            this.buttonDispose.Text = "Send stop";
            this.buttonDispose.UseVisualStyleBackColor = true;
            this.buttonDispose.Click += new global::System.EventHandler(this.buttonDispose_Click);
            // 
            // buttonMoreMemory
            // 
            this.buttonMoreMemory.Location = new global::System.Drawing.Point(16, 50);
            this.buttonMoreMemory.Margin = new global::System.Windows.Forms.Padding(4);
            this.buttonMoreMemory.Name = "buttonMoreMemory";
            this.buttonMoreMemory.Size = new global::System.Drawing.Size(265, 28);
            this.buttonMoreMemory.TabIndex = 2;
            this.buttonMoreMemory.Text = "More memory";
            this.buttonMoreMemory.UseVisualStyleBackColor = true;
            this.buttonMoreMemory.Click += new global::System.EventHandler(this.buttonMoreMemory_Click);
            // 
            // numericUpDown
            // 
            this.numericUpDown.Location = new global::System.Drawing.Point(304, 54);
            this.numericUpDown.Margin = new global::System.Windows.Forms.Padding(4);
            this.numericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown.Name = "numericUpDown";
            this.numericUpDown.Size = new global::System.Drawing.Size(100, 22);
            this.numericUpDown.TabIndex = 3;
            this.numericUpDown.TextAlign = global::System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // numericUpDownMemoryLimit
            // 
            this.numericUpDownMemoryLimit.Location = new global::System.Drawing.Point(304, 86);
            this.numericUpDownMemoryLimit.Margin = new global::System.Windows.Forms.Padding(4);
            this.numericUpDownMemoryLimit.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDownMemoryLimit.Name = "numericUpDownMemoryLimit";
            this.numericUpDownMemoryLimit.Size = new global::System.Drawing.Size(100, 22);
            this.numericUpDownMemoryLimit.TabIndex = 5;
            this.numericUpDownMemoryLimit.TextAlign = global::System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownMemoryLimit.Value = new decimal(new int[] {
            45,
            0,
            0,
            0});
            this.numericUpDownMemoryLimit.ValueChanged += new global::System.EventHandler(this.numericUpDownMemoryLimit_ValueChanged);
            // 
            // checkBoxMemoryLimit
            // 
            this.checkBoxMemoryLimit.AutoSize = true;
            this.checkBoxMemoryLimit.Checked = true;
            this.checkBoxMemoryLimit.CheckState = global::System.Windows.Forms.CheckState.Checked;
            this.checkBoxMemoryLimit.Enabled = false;
            this.checkBoxMemoryLimit.Location = new global::System.Drawing.Point(462, 88);
            this.checkBoxMemoryLimit.Margin = new global::System.Windows.Forms.Padding(4);
            this.checkBoxMemoryLimit.Name = "checkBoxMemoryLimit";
            this.checkBoxMemoryLimit.Size = new global::System.Drawing.Size(123, 20);
            this.checkBoxMemoryLimit.TabIndex = 6;
            this.checkBoxMemoryLimit.Text = "Memory limit";
            this.checkBoxMemoryLimit.UseVisualStyleBackColor = true;
            // 
            // comboBoxMemoryLimit
            // 
            this.comboBoxMemoryLimit.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMemoryLimit.FormattingEnabled = true;
            this.comboBoxMemoryLimit.Items.AddRange(new object[] {
            "Exit",
            "Restart"});
            this.comboBoxMemoryLimit.Location = new global::System.Drawing.Point(16, 84);
            this.comboBoxMemoryLimit.Margin = new global::System.Windows.Forms.Padding(4);
            this.comboBoxMemoryLimit.Name = "comboBoxMemoryLimit";
            this.comboBoxMemoryLimit.Size = new global::System.Drawing.Size(265, 24);
            this.comboBoxMemoryLimit.TabIndex = 4;
            // 
            // richTextBoxMoreMemory
            // 
            this.richTextBoxMoreMemory.Dock = global::System.Windows.Forms.DockStyle.Bottom;
            this.richTextBoxMoreMemory.Font = new global::System.Drawing.Font("Courier New", 9F, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBoxMoreMemory.Location = new global::System.Drawing.Point(0, 112);
            this.richTextBoxMoreMemory.Margin = new global::System.Windows.Forms.Padding(4);
            this.richTextBoxMoreMemory.Name = "richTextBoxMoreMemory";
            this.richTextBoxMoreMemory.ReadOnly = true;
            this.richTextBoxMoreMemory.ScrollBars = global::System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBoxMoreMemory.Size = new global::System.Drawing.Size(634, 100);
            this.richTextBoxMoreMemory.TabIndex = 8;
            this.richTextBoxMoreMemory.Text = "";
            // 
            // checkBoxSimulateMode
            // 
            this.checkBoxSimulateMode.AutoSize = true;
            this.checkBoxSimulateMode.Enabled = false;
            this.checkBoxSimulateMode.Location = new global::System.Drawing.Point(462, 58);
            this.checkBoxSimulateMode.Margin = new global::System.Windows.Forms.Padding(4);
            this.checkBoxSimulateMode.Name = "checkBoxSimulateMode";
            this.checkBoxSimulateMode.Size = new global::System.Drawing.Size(131, 20);
            this.checkBoxSimulateMode.TabIndex = 9;
            this.checkBoxSimulateMode.Text = "Simulate mode";
            this.checkBoxSimulateMode.UseVisualStyleBackColor = true;
            // 
            // labelMb1
            // 
            this.labelMb1.AutoSize = true;
            this.labelMb1.Location = new global::System.Drawing.Point(420, 60);
            this.labelMb1.Name = "labelMb1";
            this.labelMb1.Size = new global::System.Drawing.Size(24, 16);
            this.labelMb1.TabIndex = 10;
            this.labelMb1.Text = "MB";
            // 
            // labelMb2
            // 
            this.labelMb2.AutoSize = true;
            this.labelMb2.Location = new global::System.Drawing.Point(420, 90);
            this.labelMb2.Name = "labelMb2";
            this.labelMb2.Size = new global::System.Drawing.Size(24, 16);
            this.labelMb2.TabIndex = 11;
            this.labelMb2.Text = "MB";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new global::System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new global::System.Drawing.Size(634, 312);
            this.Controls.Add(this.labelMb2);
            this.Controls.Add(this.labelMb1);
            this.Controls.Add(this.checkBoxSimulateMode);
            this.Controls.Add(this.richTextBoxMoreMemory);
            this.Controls.Add(this.comboBoxMemoryLimit);
            this.Controls.Add(this.checkBoxMemoryLimit);
            this.Controls.Add(this.numericUpDownMemoryLimit);
            this.Controls.Add(this.numericUpDown);
            this.Controls.Add(this.buttonMoreMemory);
            this.Controls.Add(this.buttonDispose);
            this.Controls.Add(this.richTextBoxTask);
            this.Controls.Add(this.buttonCreate);
            this.Font = new global::System.Drawing.Font("Courier New", 9.75F, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new global::System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Example memory limit";
            this.Load += new global::System.EventHandler(this.Form1_Load);
            ((global::System.ComponentModel.ISupportInitialize)(this.numericUpDown)).EndInit();
            ((global::System.ComponentModel.ISupportInitialize)(this.numericUpDownMemoryLimit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private global::System.Windows.Forms.Button buttonCreate;
        private global::System.Windows.Forms.RichTextBox richTextBoxTask;
        private global::System.Windows.Forms.Button buttonDispose;
        private global::System.Windows.Forms.Button buttonMoreMemory;
        private global::System.Windows.Forms.NumericUpDown numericUpDown;
        private global::System.Windows.Forms.NumericUpDown numericUpDownMemoryLimit;
        private global::System.Windows.Forms.CheckBox checkBoxMemoryLimit;
        private global::System.Windows.Forms.ComboBox comboBoxMemoryLimit;
        private global::System.Windows.Forms.RichTextBox richTextBoxMoreMemory;
        private global::System.Windows.Forms.CheckBox checkBoxSimulateMode;
        private global::System.Windows.Forms.Label labelMb1;
        private global::System.Windows.Forms.Label labelMb2;
    }
}

