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
using ThermoVision.Tipos;

namespace ThermoVision.CustomControls
{
    public partial class CamCannonControl : UserControl
    {
        public ThermoCam camara;

        public delegate void CoordinateSelectedEventHandler(object sender, CoordinateSelectedEventArgs e);

        public event CoordinateSelectedEventHandler ThermoCam_CoordinateSelected;

        public struct CoordinateSelectedEventArgs
        {
            public ThermoCam t;
            public SubZona s;

            public int X;
            public int Y;
        }

        public CamCannonControl()                            
        {
            InitializeComponent();
        }

        public void Initialize(Form f, ThermoCam t)  
        {
            this.camara = t;
            this.camara.ConfiguracionMode = true;
            //////////////////////  INICIALIZACIÓN CAMARAS //////////////////////////
            this.camara.InitializeForm(f);

            this.camara.ThermoCamImgReceived         += camara_ThermoCamImgEvent;
            this.pictureBoxCam.MouseClick += pictureBoxCam_MouseClick;

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
            updatePictureBox(this.pictureBoxCam, ref e.Imagen);
        }

        void pictureBoxCam_MouseClick(object sender, MouseEventArgs e)
        {
            int coordenadaX = e.X * this.camara.Width / this.pictureBoxCam.Width ;
            int coordenadaY = e.Y * this.camara.Heigth / this.pictureBoxCam.Height;

            foreach (SubZona s in this.camara.SubZonas)
            {
                if (s.Selected)
                {
                    if (coordenadaX >= s.Inicio.X && coordenadaX <= s.Fin.X &&
                        coordenadaY >= s.Inicio.Y && coordenadaY <= s.Fin.Y)
                    {
                        //Pertenece a esta subzona
                        //¿A que coordenada de la matriz pertenece?
                        int X = (coordenadaY - s.Inicio.Y) / ((s.Fin.Y - s.Inicio.Y) / s.Filas);
                        int Y = (coordenadaX - s.Inicio.X) / ((s.Fin.X - s.Inicio.X) / s.Columnas);

                        if (this.ThermoCam_CoordinateSelected != null)
                            this.ThermoCam_CoordinateSelected(this, new CoordinateSelectedEventArgs() 
                            { 
                                t = this.camara, 
                                s = s, 
                                X = X, 
                                Y = Y 
                            });

                        return;
                    }
                }
            }
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
