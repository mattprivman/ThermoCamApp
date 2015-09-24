using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThermoVision.Enumeraciones;
using ThermoVision.Models;

namespace ThermoVision.CustomControls
{
    public partial class CamControl : UserControl
    {
        public ThermoCam camara;

        public CamControl()                            
        {
            InitializeComponent();
        }

        public void Initialize(Form f, ThermoCam t)  
        {
            this.camara = t;
            this.camara.ConfiguracionMode = false;
            //////////////////////  INICIALIZACIÓN CAMARAS //////////////////////////
            this.camara.InitializeForm(f);

            this.camara.ThermoCamImgReceived         += camara_ThermoCamImgEvent;

            this.Conectar();
        }

        public void Conectar()          
        {
            this.camara.Conectar();
        }

        public void Desconectar()
        {
            this.camara.Desconectar();
        }

        //CAMARA

        void camara_ThermoCamImgEvent(object sender, ThermoCamImgArgs e) 
        {
            updatePictureBox(this.pictureBox1, ref e.Imagen);
        }

        #region ACTUALIZAR CONTROLES
        private delegate void updateTextCallback(object o, string text);
        private void updateText(object o, string text)                               
        {
            if (o is Control)
            {
                Control c = (Control)o;

                if (c.InvokeRequired)
                {
                    c.BeginInvoke(new updateTextCallback(updateText), o, text);
                }
                else
                {
                    try
                    {
                        c.Text = text;
                    }
                    catch (Exception e) { e.ToString(); }
                }
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
                    catch (Exception ex) { }
                }
                else
                {
                    p.Image = bmp;
                    p.Refresh();
                }
            }
        }
        #endregion

    }
}
