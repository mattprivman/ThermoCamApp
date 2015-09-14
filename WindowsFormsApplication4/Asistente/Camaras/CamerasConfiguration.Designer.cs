using System.Collections.Generic;

namespace WindowsFormsApplication4.Asistente.Camaras
{
    partial class CamerasConfiguration
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
        private void InitializeComponent(int numCamaras)
        {
            this.camaras = new List<ThermoVision.CustomControls.CamConfigControl>();
            this.buttonAtras = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.SuspendLayout();

            //
            // camaras
            //
            for (int i = 0; i < numCamaras; i++)
            {
                ThermoVision.CustomControls.CamConfigControl c = new ThermoVision.CustomControls.CamConfigControl();

                c.Location = new System.Drawing.Point(i * c.Width, 0);
                c.Name = "camConfigControl";
                c.Size = new System.Drawing.Size(644, 753);
                c.TabIndex = i;

                this.Controls.Add(c);

                c.Initialize(this);

                this.camaras.Add(c);
                this._System.addThermoCam(c.camara);
            }
            // 
            // buttonAtras
            // 
            this.buttonAtras.Location = new System.Drawing.Point(               //COORDENADAS DE DIBUJO
                                                            numCamaras * this.camaras[0].Width - 2 * (10 + 75),     
                                                            757);
            this.buttonAtras.Name       = "buttonBack";
            this.buttonAtras.Size       = new System.Drawing.Size(75, 23);      //TAMAÑO
            this.buttonAtras.TabIndex   = 2;
            this.buttonAtras.Text       = "<< Atras";
            this.buttonAtras.UseVisualStyleBackColor = true;
            this.buttonAtras.Click      += new System.EventHandler(this.buttonAtras_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(                //COORDENADAS DE DIBUJO
                numCamaras * this.camaras[0].Width - 10 - 75, 
                757);
            this.buttonNext.Name        = "buttonNext";
            this.buttonNext.Size        = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex    = 2;
            this.buttonNext.Text        = "Siguiente >>";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click       += new System.EventHandler(this.buttonNext_Click);
            // 
            // CamerasConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode          = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize             = new System.Drawing.Size(
                                                            numCamaras * this.camaras[0].Width, 
                                                            790);
            this.Controls.Add(buttonAtras);
            this.Controls.Add(buttonNext);
            this.Name                   = "CamerasConfiguration";
            this.Text                   = "CamerasConfiguration";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonAtras;
        private List<ThermoVision.CustomControls.CamConfigControl> camaras;
    }
}

// Codigo generación controles dinámicos

/*

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent(int numCamaras, List<ThermoVision.ThermoCam> _thermoCams = null)
        {
            this.camaras = new List<ThermoVision.CustomControls.CamConfigControl>();
            this.buttonAtras = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.SuspendLayout();

            //
            // camaras
            //
            for (int i = 0; i < numCamaras; i++)
            {
                ThermoVision.CustomControls.CamConfigControl c = new ThermoVision.CustomControls.CamConfigControl();

                c.Location = new System.Drawing.Point(i * c.Width, 0);
                c.Name = "camConfigControl";
                c.Size = new System.Drawing.Size(644, 753);
                c.TabIndex = i;

                this.Controls.Add(c);

                if (_thermoCams == null)
                    c.Initialize(this);
                else
                    c.Initialize(this, _thermoCams[i]);

                this.camaras.Add(c);
            }
            // 
            // buttonAtras
            // 
            this.buttonAtras.Location = new System.Drawing.Point(10, 757);
            this.buttonAtras.Name = "buttonNext";
            this.buttonAtras.Size = new System.Drawing.Size(75, 23);
            this.buttonAtras.TabIndex = 2;
            this.buttonAtras.Text = "<< Atras";
            this.buttonAtras.UseVisualStyleBackColor = true;
            this.buttonAtras.Click += new System.EventHandler(this.buttonAtras_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(90, 757);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 2;
            this.buttonNext.Text = "Siguiente >>";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // CamerasConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(
                numCamaras * this.camaras[0].Width, 
                790);
            this.Controls.Add(buttonAtras);
            this.Controls.Add(buttonNext);
            this.Name = "CamerasConfiguration";
            this.Text = "CamerasConfiguration";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonAtras;
        private List<ThermoVision.CustomControls.CamConfigControl> camaras;
*/