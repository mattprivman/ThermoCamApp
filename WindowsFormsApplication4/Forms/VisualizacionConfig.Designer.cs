namespace ThermoCamApp.Forms
{
    partial class VisualizacionConfig
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
            this.checkBoxRejillaApagado = new System.Windows.Forms.CheckBox();
            this.checkBoxRejillaVaciado = new System.Windows.Forms.CheckBox();
            this.checkBoxHornos = new System.Windows.Forms.CheckBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.comboBoxEscala = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxRejillaApagado
            // 
            this.checkBoxRejillaApagado.AutoSize = true;
            this.checkBoxRejillaApagado.Location = new System.Drawing.Point(158, 25);
            this.checkBoxRejillaApagado.Name = "checkBoxRejillaApagado";
            this.checkBoxRejillaApagado.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxRejillaApagado.Size = new System.Drawing.Size(15, 14);
            this.checkBoxRejillaApagado.TabIndex = 0;
            this.checkBoxRejillaApagado.UseVisualStyleBackColor = true;
            this.checkBoxRejillaApagado.CheckedChanged += new System.EventHandler(this.checkBoxRejillaApagado_CheckedChanged);
            // 
            // checkBoxRejillaVaciado
            // 
            this.checkBoxRejillaVaciado.AutoSize = true;
            this.checkBoxRejillaVaciado.Location = new System.Drawing.Point(158, 45);
            this.checkBoxRejillaVaciado.Name = "checkBoxRejillaVaciado";
            this.checkBoxRejillaVaciado.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxRejillaVaciado.Size = new System.Drawing.Size(15, 14);
            this.checkBoxRejillaVaciado.TabIndex = 1;
            this.checkBoxRejillaVaciado.UseVisualStyleBackColor = true;
            this.checkBoxRejillaVaciado.CheckedChanged += new System.EventHandler(this.checkBoxRejillaVaciado_CheckedChanged);
            // 
            // checkBoxHornos
            // 
            this.checkBoxHornos.AutoSize = true;
            this.checkBoxHornos.Location = new System.Drawing.Point(158, 65);
            this.checkBoxHornos.Name = "checkBoxHornos";
            this.checkBoxHornos.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxHornos.Size = new System.Drawing.Size(15, 14);
            this.checkBoxHornos.TabIndex = 2;
            this.checkBoxHornos.UseVisualStyleBackColor = true;
            this.checkBoxHornos.CheckedChanged += new System.EventHandler(this.checkBoxHornos_CheckedChanged);
            // 
            // comboBoxEscala
            // 
            this.comboBoxEscala.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEscala.FormattingEnabled = true;
            this.comboBoxEscala.Location = new System.Drawing.Point(52, 36);
            this.comboBoxEscala.Name = "comboBoxEscala";
            this.comboBoxEscala.Size = new System.Drawing.Size(121, 21);
            this.comboBoxEscala.TabIndex = 3;
            this.comboBoxEscala.SelectedIndexChanged += new System.EventHandler(this.comboBoxEscala_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Escala";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Visualizar hornos";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Visualizar rejilla de vaciado";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Visualizar rejilla de apagado";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(110, 186);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 8;
            this.buttonSave.Text = "Guardar";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.checkBoxRejillaApagado);
            this.groupBox1.Controls.Add(this.checkBoxRejillaVaciado);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.checkBoxHornos);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(179, 91);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Visualizacion camaras";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBoxEscala);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 109);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(179, 71);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Escala";
            // 
            // VisualizacionConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(206, 220);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VisualizacionConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configurar visualizacion";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxRejillaApagado;
        private System.Windows.Forms.CheckBox checkBoxRejillaVaciado;
        private System.Windows.Forms.CheckBox checkBoxHornos;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox comboBoxEscala;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}