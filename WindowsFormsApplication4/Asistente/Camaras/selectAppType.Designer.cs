namespace WindowsFormsApplication4.Asistente
{
    partial class selectAppType
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
            this.comboBoxAppType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonNext = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxNumberCameras = new System.Windows.Forms.ComboBox();
            this.buttonLoadCfg = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // comboBoxAppType
            // 
            this.comboBoxAppType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAppType.FormattingEnabled = true;
            this.comboBoxAppType.Location = new System.Drawing.Point(154, 34);
            this.comboBoxAppType.Name = "comboBoxAppType";
            this.comboBoxAppType.Size = new System.Drawing.Size(135, 21);
            this.comboBoxAppType.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tipo de applicación";
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(214, 124);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 2;
            this.buttonNext.Text = "Siguiente >>";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Nº de camaras";
            // 
            // comboBoxNumberCameras
            // 
            this.comboBoxNumberCameras.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxNumberCameras.FormattingEnabled = true;
            this.comboBoxNumberCameras.Location = new System.Drawing.Point(154, 79);
            this.comboBoxNumberCameras.Name = "comboBoxNumberCameras";
            this.comboBoxNumberCameras.Size = new System.Drawing.Size(135, 21);
            this.comboBoxNumberCameras.TabIndex = 3;
            this.comboBoxNumberCameras.SelectedIndexChanged += new System.EventHandler(this.comboBoxNumberCameras_SelectedIndexChanged);
            // 
            // buttonLoadCfg
            // 
            this.buttonLoadCfg.Location = new System.Drawing.Point(31, 124);
            this.buttonLoadCfg.Name = "buttonLoadCfg";
            this.buttonLoadCfg.Size = new System.Drawing.Size(127, 23);
            this.buttonLoadCfg.TabIndex = 5;
            this.buttonLoadCfg.Text = "Cargar configuración";
            this.buttonLoadCfg.UseVisualStyleBackColor = true;
            this.buttonLoadCfg.Click += new System.EventHandler(this.buttonLoadCfg_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // selectAppType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 159);
            this.ControlBox = false;
            this.Controls.Add(this.buttonLoadCfg);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxNumberCameras);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxAppType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "selectAppType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Seleccione el tipo de aplicación";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxAppType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxNumberCameras;
        private System.Windows.Forms.Button buttonLoadCfg;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}