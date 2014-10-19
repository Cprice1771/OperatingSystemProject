namespace OperatingSystemFrontend
{
    partial class OperatingSystemForm
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
            this.comboBoxLTS = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxRAMSize = new System.Windows.Forms.TextBox();
            this.textBoxCPUCount = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.richTextBoxRAM = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.fileNameTextBox = new System.Windows.Forms.TextBox();
            this.selectFileButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxLTS
            // 
            this.comboBoxLTS.FormattingEnabled = true;
            this.comboBoxLTS.Items.AddRange(new object[] {
            "FCFS",
            "Priority",
            "Shortest"});
            this.comboBoxLTS.Location = new System.Drawing.Point(102, 40);
            this.comboBoxLTS.Name = "comboBoxLTS";
            this.comboBoxLTS.Size = new System.Drawing.Size(91, 21);
            this.comboBoxLTS.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "LTS Algorithm:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Size of RAM:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Number of CPUs:";
            // 
            // textBoxRAMSize
            // 
            this.textBoxRAMSize.Location = new System.Drawing.Point(102, 70);
            this.textBoxRAMSize.Name = "textBoxRAMSize";
            this.textBoxRAMSize.Size = new System.Drawing.Size(38, 20);
            this.textBoxRAMSize.TabIndex = 4;
            this.textBoxRAMSize.Text = "100";
            // 
            // textBoxCPUCount
            // 
            this.textBoxCPUCount.Location = new System.Drawing.Point(102, 96);
            this.textBoxCPUCount.Name = "textBoxCPUCount";
            this.textBoxCPUCount.Size = new System.Drawing.Size(38, 20);
            this.textBoxCPUCount.TabIndex = 5;
            this.textBoxCPUCount.Text = "4";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(301, 529);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Run!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // richTextBoxRAM
            // 
            this.richTextBoxRAM.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxRAM.Location = new System.Drawing.Point(12, 141);
            this.richTextBoxRAM.Name = "richTextBoxRAM";
            this.richTextBoxRAM.ReadOnly = true;
            this.richTextBoxRAM.Size = new System.Drawing.Size(364, 382);
            this.richTextBoxRAM.TabIndex = 7;
            this.richTextBoxRAM.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "File:";
            // 
            // fileNameTextBox
            // 
            this.fileNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileNameTextBox.Location = new System.Drawing.Point(48, 13);
            this.fileNameTextBox.Name = "fileNameTextBox";
            this.fileNameTextBox.Size = new System.Drawing.Size(297, 20);
            this.fileNameTextBox.TabIndex = 9;
            // 
            // selectFileButton
            // 
            this.selectFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectFileButton.Location = new System.Drawing.Point(351, 13);
            this.selectFileButton.Name = "selectFileButton";
            this.selectFileButton.Size = new System.Drawing.Size(25, 23);
            this.selectFileButton.TabIndex = 10;
            this.selectFileButton.Text = "...";
            this.selectFileButton.UseVisualStyleBackColor = true;
            this.selectFileButton.Click += new System.EventHandler(this.selectFileButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "RAM:";
            // 
            // OperatingSystemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 564);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.selectFileButton);
            this.Controls.Add(this.fileNameTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.richTextBoxRAM);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxCPUCount);
            this.Controls.Add(this.textBoxRAMSize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxLTS);
            this.Name = "OperatingSystemForm";
            this.Text = "OperatingSystem";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxLTS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxRAMSize;
        private System.Windows.Forms.TextBox textBoxCPUCount;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox richTextBoxRAM;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox fileNameTextBox;
        private System.Windows.Forms.Button selectFileButton;
        private System.Windows.Forms.Label label5;
    }
}