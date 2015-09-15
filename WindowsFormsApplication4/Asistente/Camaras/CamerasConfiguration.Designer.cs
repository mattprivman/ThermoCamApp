using System.Collections.Generic;

namespace WindowsFormsApplication4.Asistente.Camaras
{
    partial class CamerasConfiguration
    {
        const int camarasXstart = 0;
        const int treeViewXstart = 0;
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
            this.camaras            = new List<ThermoVision.CustomControls.CamConfigControl>();
            this.buttonAtras        = new System.Windows.Forms.Button();
            this.buttonNext         = new System.Windows.Forms.Button();
            this.textBoxZonaName    = new System.Windows.Forms.TextBox();
            this.buttonAddZone      = new System.Windows.Forms.Button();
            this.buttonRemoveZona   = new System.Windows.Forms.Button();
            this.listBoxZonas       = new System.Windows.Forms.ListBox();
            this.SuspendLayout();

            //
            // camaras
            //
            for (int i = 0; i < numCamaras; i++)
            {
                ThermoVision.CustomControls.CamConfigControl c = new ThermoVision.CustomControls.CamConfigControl();

                c.Location = new System.Drawing.Point(i * c.Width + camarasXstart, 
                                                    0);
                c.Name = "camConfigControl";
                c.Size = new System.Drawing.Size(644, 753);
                c.TabIndex = 100 + i;

                this.Controls.Add(c);

                c.Initialize(this, this._system);

                this.camaras.Add(c);
            }
            // 
            // buttonAtras
            // 
            this.buttonAtras.Location = new System.Drawing.Point(               //COORDENADAS DE DIBUJO
                                                            numCamaras * this.camaras[0].Width - 2 * (10 + 75),     
                                                            915);
            this.buttonAtras.Name       = "buttonBack";
            this.buttonAtras.Size       = new System.Drawing.Size(75, 23);      //TAMAÑO
            this.buttonAtras.TabIndex   = 5;
            this.buttonAtras.Text       = "<< Atras";
            this.buttonAtras.UseVisualStyleBackColor = true;
            this.buttonAtras.Click      += new System.EventHandler(this.buttonAtras_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(                //COORDENADAS DE DIBUJO
                numCamaras * this.camaras[0].Width - 10 - 75, 
                915);
            this.buttonNext.Name        = "buttonNext";
            this.buttonNext.Size        = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex    = 4;
            this.buttonNext.Text        = "Siguiente >>";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click       += new System.EventHandler(this.buttonNext_Click);
            // 
            // listBoxZonas
            // 
            this.listBoxZonas.Location = new System.Drawing.Point(15, 757);
            this.listBoxZonas.Name = "listBoxZonas";
            this.listBoxZonas.Size = new System.Drawing.Size(275, 150);
            this.listBoxZonas.TabIndex = 0;
            // 
            // textBoxZonaName
            // 
            this.textBoxZonaName.Location = new System.Drawing.Point(15, 915);
            this.textBoxZonaName.Name = "textBoxZonaName";
            this.textBoxZonaName.Size = new System.Drawing.Size(100, 20);
            this.textBoxZonaName.TabIndex = 1;
            // 
            // buttonAddZone
            // 
            this.buttonAddZone.Location = new System.Drawing.Point(                //COORDENADAS DE DIBUJO
                125,
                915);
            this.buttonAddZone.Name = "buttonAddZone";
            this.buttonAddZone.Size = new System.Drawing.Size(75, 23);
            this.buttonAddZone.TabIndex = 2;
            this.buttonAddZone.Text = "Añadir";
            this.buttonAddZone.UseVisualStyleBackColor = true;
            // 
            // buttonRemoveZone
            // 
            this.buttonRemoveZona.Location = new System.Drawing.Point(                //COORDENADAS DE DIBUJO
                215,
                915);
            this.buttonRemoveZona.Name = "buttonRemoveZone";
            this.buttonRemoveZona.Size = new System.Drawing.Size(75, 23);
            this.buttonRemoveZona.TabIndex = 3;
            this.buttonRemoveZona.Text = "Borrar";
            this.buttonRemoveZona.UseVisualStyleBackColor = true;
            // 
            // CamerasConfiguration
            // 
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode          = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize             = new System.Drawing.Size(
                                                            numCamaras * this.camaras[0].Width, 
                                                            950);
            this.Controls.Add(buttonAtras);
            this.Controls.Add(buttonNext);
            this.Controls.Add(listBoxZonas);
            this.Controls.Add(textBoxZonaName);
            this.Controls.Add(buttonAddZone);
            this.Controls.Add(buttonRemoveZona);
            this.Name                   = "CamerasConfiguration";
            this.Text                   = "CamerasConfiguration";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button     buttonNext;
        private System.Windows.Forms.Button     buttonAtras;
        private System.Windows.Forms.TextBox    textBoxZonaName;
        private System.Windows.Forms.Button     buttonAddZone;
        private System.Windows.Forms.Button     buttonRemoveZona;
        private List<ThermoVision.CustomControls.CamConfigControl> camaras;
        private System.Windows.Forms.ListBox    listBoxZonas;
    }
}

// Codigo generación controles dinámicos

/*
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

                c.Initialize(this, this._system);

                this.camaras.Add(c);
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
            this.AutoScaleMode          = System.Windows.Forms.AutoScaleMode.Fon
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
*/