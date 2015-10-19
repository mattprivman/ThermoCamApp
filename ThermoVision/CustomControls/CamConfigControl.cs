using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ThermoVision.Enumeraciones;
using ThermoVision.Models;
using ThermoVision.Tipos;

namespace ThermoVision.CustomControls
{
    public partial class CamConfigControl : UserControl
    {
        #region "Variables"
        //Sistema          _system;
        public ThermoCam camara;

        Bitmap      bmp;
        Bitmap      bmpModified;
        Point       coordenada;
        Point       Fin;
        #endregion

        #region "Metodos públicos"

        public CamConfigControl()                                                   
        {
            InitializeComponent();
        }

        public void Initialize(Form f, ThermoCam t = null)                          
        {
            //////////////////////  INICIALIZACIÓN CAMARAS //////////////////////////
            if (t == null)
            {
                this.camara = new ThermoCam(
                    f,
                    CameraType.FLIR_A3X0,
                    DeviceType.Ethernet16bits,
                    InterfaceType.TCP);
            }
            else
            {
                t.InitializeForm(f);
                this.camara                  = t;
                this.textBoxCamName.Text     = t.Nombre;
                this.textBoxDireccionIP.Text = t.Address;
            }

            this.camara.ConfiguracionMode = true;

            //////////////////////  EVENTO CONEXIÓN Y DESCONEXIÓN   //////////////////
            this.camara.ThermoCamConnected      += camara_ThermoCamConnected;
            this.camara.ThermoCamDisConnected   += camara_ThermoCamDisConnected;

            //////////////////////  EVENTO CAMBIO DE NOMBRE CAMARA  //////////////////
            this.textBoxCamName.TextChanged     += textBoxCamName_TextChanged;
            this.textBoxDireccionIP.TextChanged += textBoxDireccionIP_TextChanged;

            //////////////////////  EVENTOS CONECTAR Y DESCONECTAR  //////////////////
            this.buttonConectar.Click           += buttonConectar_Click;
            this.buttonDesconectar.Click        += buttonDesconectar_Click;
            
            this.buttonAutoAdjust.Click                 += buttonAutoAdjust_Click;
            this.buttonAutoFocus.Click                  += buttonAutoFocus_Click;
            this.buttonInternalImageCorrection.Click    += buttonInternalImageCorrection_Click;
            this.buttonReloadCalibration.Click          += buttonReloadCalibration_Click;
            this.buttonExternalImageCorrection.Click    += buttonExternalImageCorrection_Click;
        }
        ////////////////////////////  EVENTOS CONEXIÓN Y DESCONEXIÓN DE LA CAMARA  /////////////
        void camara_ThermoCamConnected(object sender, EventArgs e)                  
        {
            //EVENTO RECIBIR IMAGENES Y DIVISIONES
            this.camara.ThermoCamImgReceived                += camara_ThermoCamImgReceived;
            this.camara.ThermoCamFrameRateChanged           += camara_ThermoCamFrameRateChanged;
            this.camara.ThermoCamImgRceceivedTimeoutChanged += camara_ThermoCamImgRceceivedTimeoutChanged;

            //EVENTO CLICK EN PICTUREBOX
            this.pictureBox1.MouseDown              += pictureBox1_MouseDown;

            this.labelConectionStatusString.Text    = "Conectado";
            this.labelConexionStatusColor.BackColor = Color.Green;
        }
        public void Conectar()                                                      
        {
            this.camara.Conectar();
        }

        #endregion

        public delegate void coordenadasEventHandler(ThermoCam sender, coordenadasEventArgs e);

        public struct coordenadasEventArgs                                          
        {
            public Point Inicio;
            public Point Fin;
        }

        public event coordenadasEventHandler CoordenadasGeneradas;

        void camara_ThermoCamDisConnected(object sender, EventArgs e)               
        {
            //CAMARA DESCONECTADA
            //EVENTO RECIBIR IMAGENES Y DIVISIONES
            this.camara.ThermoCamImgReceived                -= camara_ThermoCamImgReceived;
            this.camara.ThermoCamFrameRateChanged           -= camara_ThermoCamFrameRateChanged;
            this.camara.ThermoCamImgRceceivedTimeoutChanged -= camara_ThermoCamImgRceceivedTimeoutChanged;

            //EVENTO CLICK EN PICTUREBOX
            this.pictureBox1.MouseDown              -= pictureBox1_MouseDown;

            this.labelConectionStatusString.Text    = "Desconectado";
            this.labelConexionStatusColor.BackColor = Color.Red;
        }
        void camara_ThermoCamImgRceceivedTimeoutChanged(object sender, string e)    
        {
            this.updateText(this.labelTimeOut, e + " ms");
        }
        void camara_ThermoCamFrameRateChanged(object sender, string e)              
        {
            this.updateText(this.labelFrameRate, e + " Hz");
        }

        #region "PictureBox1"

        void pictureBox1_MouseDown(object sender, MouseEventArgs e)                 
        {
            if (this.camara.Conectado == true && e.Button == System.Windows.Forms.MouseButtons.Left )
            {
                bmp = (Bitmap)pictureBox1.Image;
                coordenada = new Point(e.X * camara.Width / ((PictureBox)sender).Width,
                                e.Y * camara.Heigth / ((PictureBox)sender).Height);

                this.pictureBox1.MouseUp   += pictureBox1_MouseUp;
                this.pictureBox1.MouseMove += pictureBox1_MouseMove;
                this.pictureBox1.MouseDown -= pictureBox1_MouseDown;

                this.camara.ThermoCamImgReceived -= camara_ThermoCamImgReceived;
            }
        }   //MOUSE LEFT DOWN
        void pictureBox1_MouseMove(object sender, MouseEventArgs e)                 
        {
            if (this.bmp != null)
            {
                this.bmpModified = new Bitmap(this.bmp);

                int x = e.X * camara.Width / ((PictureBox)sender).Width;
                int y = e.Y * camara.Heigth / ((PictureBox)sender).Height;

                if (x > bmp.Width)
                    x = bmp.Width;
                if (x < 0)
                    x = 0;
                if (y > bmp.Height)
                    y = bmp.Height;
                if (y < 0)
                    y = 0;

                int inicioX;
                int finX;
                int inicioY;
                int finY;

                if (coordenada.X < x)
                {
                    inicioX = coordenada.X;
                    finX = x;
                }
                else
                {
                    inicioX = x;
                    finX = coordenada.X;
                }

                if (coordenada.Y < y)
                {
                    inicioY = coordenada.Y;
                    finY = y;
                }
                else
                {
                    inicioY = y;
                    finY = coordenada.Y;
                }

                int azul;

                for (int i = inicioX; i < finX; i++)
                {
                    for (int j = inicioY; j < finY; j++)
                    {
                        Color c = bmp.GetPixel(i, j);

                        azul = c.B + 50;

                        if (azul > 255)
                            azul = 255;

                        bmpModified.SetPixel(i, j, Color.FromArgb(c.R, c.G, azul));
                    }
                }

                updatePictureBox(pictureBox1, ref bmpModified);
            }
        }   //MOUSE LEFT MOVE WHILE CLIKED
        void pictureBox1_MouseUp(object sender, MouseEventArgs e)                   
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.pictureBox1.MouseUp   -= pictureBox1_MouseUp;
                this.pictureBox1.MouseMove -= pictureBox1_MouseMove;
                this.pictureBox1.MouseDown += pictureBox1_MouseDown;

                this.camara.ThermoCamImgReceived += camara_ThermoCamImgReceived;

                //Add coordinate
                Fin = new Point(e.X * camara.Width / ((PictureBox)sender).Width,
                    e.Y * camara.Heigth / ((PictureBox)sender).Height);

                //Lanzar evento coordenadas Generadas
                if(this.CoordenadasGeneradas != null)
                {
                    this.CoordenadasGeneradas(this.camara, new coordenadasEventArgs()
                                    {
                                        Inicio = this.coordenada,
                                        Fin    = this.Fin
                                    });
                }
            }
        }   //MOUSE LEFT UP

        void camara_ThermoCamImgReceived(object sender, ThermoCamImgArgs e)         
        {
            if (this.bmp != null)
                this.bmp.Dispose();

            bmp = (Bitmap) e.Imagen.Clone();

            updatePictureBox(this.pictureBox1, ref bmp);
        }   //IMAGE RECEIVED

        #endregion

        #region "ConexionTab"
        void textBoxCamName_TextChanged(object sender, EventArgs e)                 
        {
            this.camara.Nombre = this.textBoxCamName.Text;
        }   //CAMERA NAME CHANGED
        void textBoxDireccionIP_TextChanged(object sender, EventArgs e)             
        {
            this.camara.Address = this.textBoxDireccionIP.Text;
        }   //IP CHANGED

        void buttonConectar_Click(object sender, EventArgs e)                       
        {
            this.camara.Conectar();
        }   //BUTTON CONECTAR CLICKED
        void buttonDesconectar_Click(object sender, EventArgs e)                    
        {
            this.camara.Desconectar();
        }   //BUTTON DESCONECTAR CLICKED
        #endregion

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
                    catch (Exception e) { }
                }
                else
                {
                    p.Image = bmp;
                    p.Refresh();
                }
            }
        }
        #endregion

        #region "Camara settings"

        private void buttonAutoAdjust_Click(object sender, EventArgs e)                 
        {
            this.camara.autoAdjust();
        }
        private void buttonAutoFocus_Click(object sender, EventArgs e)                  
        {
            this.camara.autoFocus();
        }
        private void buttonInternalImageCorrection_Click(object sender, EventArgs e)    
        {
            this.camara.InternalImageCorrection();
        }
        private void buttonExternalImageCorrection_Click(object sender, EventArgs e)    
        {
            this.camara.ExternalImageCorrection();
        }
        private void buttonReloadCalibration_Click(object sender, EventArgs e)          
        {
            this.camara.reloadCalibration();
        }

        #endregion
    }
}