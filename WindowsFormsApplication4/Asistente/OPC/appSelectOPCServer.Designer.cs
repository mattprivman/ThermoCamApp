namespace WindowsFormsApplication4.Asistente.OPC
{
    partial class appSelectOPCServer
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
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.comboBoxOPCServers = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.treeViewBranch = new System.Windows.Forms.TreeView();
            this.listBoxItems = new System.Windows.Forms.ListBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonGenerateOPCVars = new System.Windows.Forms.Button();
            this.textBoxInitialMem = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.numericTextBOffset = new ThermoVision.CustomControls.NumericTextBox();
            this.checkBoxMax = new System.Windows.Forms.CheckBox();
            this.checkBoxMean = new System.Windows.Forms.CheckBox();
            this.checkBoxMin = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(944, 334);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 0;
            this.buttonNext.Text = "Siguiente >>";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(853, 334);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(75, 23);
            this.buttonBack.TabIndex = 1;
            this.buttonBack.Text = "<< Atrás";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // comboBoxOPCServers
            // 
            this.comboBoxOPCServers.FormattingEnabled = true;
            this.comboBoxOPCServers.Location = new System.Drawing.Point(518, 15);
            this.comboBoxOPCServers.Name = "comboBoxOPCServers";
            this.comboBoxOPCServers.Size = new System.Drawing.Size(178, 21);
            this.comboBoxOPCServers.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(441, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Servidor OPC";
            // 
            // treeViewBranch
            // 
            this.treeViewBranch.Location = new System.Drawing.Point(444, 55);
            this.treeViewBranch.Name = "treeViewBranch";
            this.treeViewBranch.Size = new System.Drawing.Size(269, 264);
            this.treeViewBranch.TabIndex = 4;
            // 
            // listBoxItems
            // 
            this.listBoxItems.FormattingEnabled = true;
            this.listBoxItems.Location = new System.Drawing.Point(719, 55);
            this.listBoxItems.Name = "listBoxItems";
            this.listBoxItems.Size = new System.Drawing.Size(218, 264);
            this.listBoxItems.TabIndex = 5;
            // 
            // textBox1
            // 
            this.textBox1.CausesValidation = false;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(403, 350);
            this.textBox1.TabIndex = 6;
            // 
            // buttonGenerateOPCVars
            // 
            this.buttonGenerateOPCVars.Location = new System.Drawing.Point(455, 334);
            this.buttonGenerateOPCVars.Name = "buttonGenerateOPCVars";
            this.buttonGenerateOPCVars.Size = new System.Drawing.Size(97, 23);
            this.buttonGenerateOPCVars.TabIndex = 7;
            this.buttonGenerateOPCVars.Text = "Generar *.csv";
            this.buttonGenerateOPCVars.UseVisualStyleBackColor = true;
            this.buttonGenerateOPCVars.Click += new System.EventHandler(this.buttonGenerateOPCVars_Click);
            // 
            // textBoxInitialMem
            // 
            this.textBoxInitialMem.Location = new System.Drawing.Point(802, 15);
            this.textBoxInitialMem.Name = "textBoxInitialMem";
            this.textBoxInitialMem.Size = new System.Drawing.Size(58, 20);
            this.textBoxInitialMem.TabIndex = 8;
            this.textBoxInitialMem.Text = "MW";
            this.textBoxInitialMem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(744, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Dir. Mem.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(891, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Offset";
            // 
            // numericTextBOffset
            // 
            this.numericTextBOffset.Location = new System.Drawing.Point(932, 16);
            this.numericTextBOffset.Name = "numericTextBOffset";
            this.numericTextBOffset.Size = new System.Drawing.Size(57, 20);
            this.numericTextBOffset.TabIndex = 12;
            this.numericTextBOffset.Texto = "0";
            // 
            // checkBoxMax
            // 
            this.checkBoxMax.AutoSize = true;
            this.checkBoxMax.Location = new System.Drawing.Point(944, 134);
            this.checkBoxMax.Name = "checkBoxMax";
            this.checkBoxMax.Size = new System.Drawing.Size(79, 17);
            this.checkBoxMax.TabIndex = 13;
            this.checkBoxMax.Text = "Max. Temp";
            this.checkBoxMax.UseVisualStyleBackColor = true;
            // 
            // checkBoxMean
            // 
            this.checkBoxMean.AutoSize = true;
            this.checkBoxMean.Location = new System.Drawing.Point(944, 157);
            this.checkBoxMean.Name = "checkBoxMean";
            this.checkBoxMean.Size = new System.Drawing.Size(83, 17);
            this.checkBoxMean.TabIndex = 14;
            this.checkBoxMean.Text = "Mean Temp";
            this.checkBoxMean.UseVisualStyleBackColor = true;
            // 
            // checkBoxMin
            // 
            this.checkBoxMin.AutoSize = true;
            this.checkBoxMin.Location = new System.Drawing.Point(944, 180);
            this.checkBoxMin.Name = "checkBoxMin";
            this.checkBoxMin.Size = new System.Drawing.Size(76, 17);
            this.checkBoxMin.TabIndex = 15;
            this.checkBoxMin.Text = "Min. Temp";
            this.checkBoxMin.UseVisualStyleBackColor = true;
            // 
            // appSelectOPCServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1049, 374);
            this.ControlBox = false;
            this.Controls.Add(this.checkBoxMin);
            this.Controls.Add(this.checkBoxMean);
            this.Controls.Add(this.checkBoxMax);
            this.Controls.Add(this.numericTextBOffset);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxInitialMem);
            this.Controls.Add(this.buttonGenerateOPCVars);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.listBoxItems);
            this.Controls.Add(this.treeViewBranch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxOPCServers);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.buttonNext);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "appSelectOPCServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Seleccionar servidor OPC";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.ComboBox comboBoxOPCServers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView treeViewBranch;
        private System.Windows.Forms.ListBox listBoxItems;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonGenerateOPCVars;
        private System.Windows.Forms.TextBox textBoxInitialMem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label3;
        private ThermoVision.CustomControls.NumericTextBox numericTextBOffset;
        private System.Windows.Forms.CheckBox checkBoxMax;
        private System.Windows.Forms.CheckBox checkBoxMean;
        private System.Windows.Forms.CheckBox checkBoxMin;
    }
}