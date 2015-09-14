namespace WindowsFormsApplication4.Asistente.Camaras
{
    partial class ZoneConfiguration
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
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.labels     =   new System.Windows.Forms.Label[this.nZonas];
            this.textBoxes  = new CustomControls.customTextBox[this.nZonas];
            this.SuspendLayout();
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(60, 
                                                            this.nZonas * 33 + 32 + 10);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(75, 23);
            this.buttonBack.TabIndex = 0;
            this.buttonBack.Text = "<< Atras";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(160, 
                                                            this.nZonas * 33 + 32 + 10);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 1;
            this.buttonNext.Text = "Siguiente >>";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);

            //
            //  labels and textBoxes
            //
            for (int i = 0; i < this.nZonas; i++)
            {
                //Labels
                this.labels[i] = new System.Windows.Forms.Label();
                this.labels[i].AutoSize = true;
                this.labels[i].Location = new System.Drawing.Point(15, 42 + i * 30);
                this.labels[i].Name = "label1";
                this.labels[i].Size = new System.Drawing.Size(41, 13);
                this.labels[i].TabIndex = i+100;
                this.labels[i].Text = "Zona " + i;

                //TextBoxes
                this.textBoxes[i] = new CustomControls.customTextBox();
                this.textBoxes[i].Id = i;
                this.textBoxes[i].Location = new System.Drawing.Point(75, 
                                                                    39 + i * 30);
                this.textBoxes[i].Name = "textBoxZona" + i;
                this.textBoxes[i].Size = new System.Drawing.Size(170, 20);
                this.textBoxes[i].TabIndex = i + 200;

                this.textBoxes[i].textChanged += textBox_TextChanged;

                this._system.addZona(new ThermoVision.Models.Zona(this._system)
                    {
                        Nombre = ""
                    });
            }            
            // 
            // ZoneConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 
                                                    this.nZonas * 42 + 50);
            this.ControlBox = false;

            for (int i = 0; i < this.nZonas; i++)
            {
                this.Controls.Add(this.labels[i]);
                this.Controls.Add(this.textBoxes[i]);
            }

            //this.Controls.Add(this.textBox1);
            //this.Controls.Add(this.label1);

            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonBack);
            this.Name = "ZoneConfiguration";
            this.Text = "Escribir nombre de zonas";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button     buttonBack;
        private System.Windows.Forms.Button     buttonNext;
        private System.Windows.Forms.Label[]    labels;
        private CustomControls.customTextBox[]  textBoxes;
    }
}