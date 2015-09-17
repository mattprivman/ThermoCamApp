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

        public void Initialize(Form f)  
        {

            //////////////////////  INICIALIZACIÓN CAMARAS //////////////////////////
            camara = new ThermoCam(f,
                CameraType.FLIR_A3X0,
                DeviceType.Ethernet16bits,
                InterfaceType.TCP);

            this.camara.Address = "172.16.100.1";

            this.camara.ThermoCamImgReceived         += camara_ThermoCamImgEvent;
        }

        public void Conectar()          
        {
            this.camara.Conectar();
        }

        //CAMARA

        void camara_ThermoCamImgEvent(object sender, ThermoCamImgArgs e) 
        {
            //using (Graphics graphics = Graphics.FromImage(e.Imagen))
            //{
            //    using (Font arialFont = new Font("Calibri Light", 7))
            //    {
            //        for (int i = 0; i < e.tempMatrix.GetLength(0); i++)
            //        {
            //            for (int j = 0; j < e.tempMatrix.GetLength(1); j++)
            //            {
            //                graphics.DrawString(e.tempMatrix[i, j].maxTemp.ToString("0.00"), arialFont, Brushes.White, new Point(0, 0));
            //            }
            //        }
            //    }
            //}
            updatePictureBox(this.pictureBox1, ref e.Imagen);

            updateText(labelCam1MaxTemp,            "Max. Temp: "       + e.MaxTemp.ToString()                + " ºC");
            updateText(labelCam1MinTemp,            "Min. Temp: "       + e.MinTemp.ToString()                + " ºC");
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
