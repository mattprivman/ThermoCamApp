namespace WindowsFormsApplication4.Asistente.Camaras
{
    partial class CameraNumberSelection
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
            this.comboBoxNumberCameras = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxNumeroZonas = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // comboBoxNumberCameras
            // 
            this.comboBoxNumberCameras.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxNumberCameras.FormattingEnabled = true;
            this.comboBoxNumberCameras.Location = new System.Drawing.Point(136, 24);
            this.comboBoxNumberCameras.Name = "comboBoxNumberCameras";
            this.comboBoxNumberCameras.Size = new System.Drawing.Size(121, 21);
            this.comboBoxNumberCameras.TabIndex = 0;
            this.comboBoxNumberCameras.SelectedIndexChanged += new System.EventHandler(this.comboBoxNumberCameras_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Nº de camaras";
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(182, 131);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 2;
            this.buttonNext.Text = "Siguiente >>";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(80, 131);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(75, 23);
            this.buttonBack.TabIndex = 3;
            this.buttonBack.Text = "<< Atras";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Nº de zonas";
            // 
            // comboBoxNumeroZonas
            // 
            this.comboBoxNumeroZonas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxNumeroZonas.FormattingEnabled = true;
            this.comboBoxNumeroZonas.Location = new System.Drawing.Point(136, 77);
            this.comboBoxNumeroZonas.Name = "comboBoxNumeroZonas";
            this.comboBoxNumeroZonas.Size = new System.Drawing.Size(121, 21);
            this.comboBoxNumeroZonas.TabIndex = 4;
            this.comboBoxNumeroZonas.SelectedIndexChanged += new System.EventHandler(this.comboBoxNumeroZonas_SelectedIndexChanged);
            // 
            // CameraNumberSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 177);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxNumeroZonas);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxNumberCameras);
            this.Name = "CameraNumberSelection";
            this.Text = "Seleccione numero de camaras";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxNumberCameras;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxNumeroZonas;

    }
}