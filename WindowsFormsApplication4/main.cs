using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThermoVision.Models;
using ThermoVision.CustomControls;

namespace WindowsFormsApplication4
{
    public partial class main : Asistente.flowControl
    {
        private Sistema _system;
        private List<CamControl> camaras = new List<CamControl>();

        PictureBox pictureBoxRampa;

        public main(Sistema _system)
        {
            this._system = _system;

            InitializeComponent();

            int counter = 0;

            this.SuspendLayout();

            foreach (ThermoCam t in this._system.ThermoCams)
            {
                // 
                // camControl1
                // 
                CamControl c = new CamControl();

                c.Location = new System.Drawing.Point(c.Width * counter, 0);
                c.Name = "camControl1";
                c.Size = new System.Drawing.Size(c.Width, c.Height);
                c.TabIndex = 100 + counter;

                c.Initialize(this, t);
                camaras.Add(c);
                this.Controls.Add(c);

                counter++;
            }

            if (this._system.Mode == "Rampas")
            {
                this.pictureBoxRampa = new PictureBox();
                ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRampa)).BeginInit();
                // 
                // pictureBox1
                // 
                this.pictureBoxRampa.Location = new System.Drawing.Point(10, 500);
                this.pictureBoxRampa.Name = "pictureBox1";
                this.pictureBoxRampa.Size = new System.Drawing.Size(this.Width - 40, 262);
                this.pictureBoxRampa.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                this.pictureBoxRampa.TabIndex = 0;
                this.pictureBoxRampa.TabStop = false;
                this.pictureBoxRampa.BorderStyle = BorderStyle.FixedSingle;
                //this.pictureBoxRampa.B

                this.Controls.Add(this.pictureBoxRampa);

                ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRampa)).EndInit();

                this._system.estados.ThermoCamImgCuadradosGenerated += estados_ThermoCamImgCuadradosGenerated;
            }

            this.ResumeLayout();

            this.FormClosing += main_FormClosing;

            this._system.conectarClienteOPC();
            this._system.modoConfiguracion = false;
        }


        void estados_ThermoCamImgCuadradosGenerated(object sender, ThermoVision.Tipos.ThemoCamImgCuadradosArgs e)
        {
            updatePictureBox(this.pictureBoxRampa, ref e.Imagen);
        }

        void main_FormClosing(object sender, FormClosingEventArgs e)              
        {
            foreach (CamControl c in camaras)
            {
                c.Desconectar();
            }
        }

        private delegate void updatePictureBoxCallback(PictureBox p, ref System.Drawing.Bitmap bmp);
        private void updatePictureBox(PictureBox p, ref System.Drawing.Bitmap bmp)                      
        {
            if (p.IsDisposed == false)
            {
                if (p.InvokeRequired)
                {
                    try
                    {
                        p.Invoke(new updatePictureBoxCallback(updatePictureBox), p, bmp);
                    }
                    catch (Exception ex) 
                    {
                        ex.ToString();
                    }
                }
                else
                {
                    p.Image = bmp;
                    p.Refresh();
                }
            }
        }
    }
}
