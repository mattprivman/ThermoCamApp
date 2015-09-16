﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ThermoVision.Tipos;
using ThermoVision.Models;

namespace ThermoVision.CustomControls
{
    public partial class CamConfigControl : UserControl
    {
        Sistema          _system;
        public ThermoCam camara;

        Bitmap      bmp;
        Bitmap      bmpModified;
        Point       coordenada;
        Point       Fin;

        int         selectedIndex;
        

        public CamConfigControl()                                                   
        {
            InitializeComponent();

            this.numericTextBoxXinit.maxVal = 320;
            this.numericTextBoxYinit.maxVal = 240;
            this.numericTextBoxXfin.maxVal  = 320;
            this.numericTextBoxYfin.maxVal  = 240;

            this.numericTextBoxFilas.maxVal = 50;
            this.numericTextBoxCol.maxVal   = 50;
        }

        public void Initialize(Form f, Sistema _system)                             
        {
            //////////////////////  INICIALIZACIÓN CAMARAS //////////////////////////

            this.camara = new ThermoCam(
                f,
                CameraType.FLIR_A3X0,
                DeviceType.Ethernet16bits,
                InterfaceType.TCP);

            this._system = _system;
            this._system.addThermoCam(this.camara);     // Añadir cammara al sistema

            this.camara.ConfiguracionMode = true;

            for (int i = 0; i < this._system.Zonas.Count; i++)
            {
                this.comboBoxZonas.Items.Add(this._system.Zonas[i].Nombre);
            }

            //////////////////////  EVENTO CONEXIÓN Y DESCONEXIÓN   //////////////////
            this.camara.ThermoCamConnected      += camara_ThermoCamConnected;
            this.camara.ThermoCamDisConnected   += camara_ThermoCamDisConnected;

            //////////////////////  EVENTO CAMBIO DE NOMBRE CAMARA  //////////////////
            this.textBoxCamName.TextChanged     += textBoxCamName_TextChanged;
            this.textBoxDireccionIP.TextChanged += textBoxDireccionIP_TextChanged;

            //////////////////////  EVENTOS CONECTAR Y DESCONECTAR  //////////////////
            this.buttonConectar.Click    += buttonConectar_Click;
            this.buttonDesconectar.Click += buttonDesconectar_Click;
        }

        ////////////////////////////  EVENTOS CONEXIÓN Y DESCONEXIÓN DE LA CAMARA  /////////////
        void camara_ThermoCamConnected(object sender, EventArgs e)                  
        {
            //EVENTO RECIBIR IMAGENES Y DIVISIONES
            this.camara.ThermoCamImgReceived    += camara_ThermoCamImgReceived;
            this.camara.DivisionesChanged       += camara_DivisionesChanged;
            this.camara.SubZonaRemoved          += camara_SubZonaRemoved;

            //EVENTO CLICK EN PICTUREBOX
            this.pictureBox1.MouseDown          += pictureBox1_MouseDown;

            //EVENTOS LISTBOX - ZONAS
            this.listBoxZonas.SelectedIndexChanged += listBoxZonas_SelectedIndexChanged;
            this.textBoxDivName.TextChanged        += textBoxDivName_TextChanged;

            //EVENTOS NUMERICTEXTBOXES
            suscribeEvents();

            //EVENTOS BOTONES
            this.buttonColAdd.Click         += buttonColAdd_Click;
            this.buttonColSubstract.Click   += buttonColSubstract_Click;
            this.buttonFilAdd.Click         += buttonFilAdd_Click;
            this.buttonFilSubstract.Click   += buttonFilSubstract_Click;
            this.buttonXinitAdd.Click       += buttonXinitAdd_Click;
            this.buttonXinitSubstract.Click += buttonXinitSubstract_Click;
            this.buttonXfinAdd.Click        += buttonXfinAdd_Click;
            this.buttonXfinSubstract.Click  += buttonXfinSubstract_Click;
            this.buttonYinitAdd.Click       += buttonYinitAdd_Click;
            this.buttonYInitSubstract.Click += buttonYInitSubstract_Click;
            this.buttonYfinAdd.Click        += buttonYfinAdd_Click;
            this.buttonYfinSubstract.Click += buttonYfinSubstract_Click;

            actualizarListBox();

        }

        void camara_ThermoCamDisConnected(object sender, EventArgs e)               
        {
            //CAMARA DESCONECTADA
            //EVENTO RECIBIR IMAGENES Y DIVISIONES
            this.camara.ThermoCamImgReceived        -= camara_ThermoCamImgReceived;
            this.camara.DivisionesChanged           -= camara_DivisionesChanged;

            //EVENTO CAMBIO DE NOMBRE CAMARA
            this.textBoxCamName.TextChanged         -= textBoxCamName_TextChanged;
            this.textBoxDireccionIP.TextChanged     -= textBoxDireccionIP_TextChanged;

            //EVENTO CLICK EN PICTUREBOX
            this.pictureBox1.MouseDown              -= pictureBox1_MouseDown;

            //EVENTOS LISTBOX - ZONAS
            this.listBoxZonas.SelectedIndexChanged  -= listBoxZonas_SelectedIndexChanged;
            this.textBoxDivName.TextChanged         -= textBoxDivName_TextChanged;

            //EVENTOS NUMERICTEXTBOXES
            suscribeEvents();

            //EVENTOS BOTONES
            this.buttonColAdd.Click         -= buttonColAdd_Click;
            this.buttonColSubstract.Click   -= buttonColSubstract_Click;
            this.buttonFilAdd.Click         -= buttonFilAdd_Click;
            this.buttonFilSubstract.Click   -= buttonFilSubstract_Click;
            this.buttonXinitAdd.Click       -= buttonXinitAdd_Click;
            this.buttonXinitSubstract.Click -= buttonXinitSubstract_Click;
            this.buttonXfinAdd.Click        -= buttonXfinAdd_Click;
            this.buttonXfinSubstract.Click  -= buttonXfinSubstract_Click;
            this.buttonYinitAdd.Click       -= buttonYinitAdd_Click;
            this.buttonYInitSubstract.Click -= buttonYInitSubstract_Click;
            this.buttonYfinAdd.Click        -= buttonYfinAdd_Click;
            this.buttonYfinSubstract.Click  -= buttonYfinSubstract_Click;

            this.borrarListBox();
        }

        #region "BOTONES"

        void buttonYfinSubstract_Click(object sender, EventArgs e)  
        {
            this.numericTextBoxYfin.decrement();
        }
        void buttonYfinAdd_Click(object sender, EventArgs e)        
        {
            this.numericTextBoxYfin.increment();
        }
        void buttonYInitSubstract_Click(object sender, EventArgs e) 
        {
            this.numericTextBoxYinit.decrement();
        }
        void buttonYinitAdd_Click(object sender, EventArgs e)       
        {
            this.numericTextBoxYinit.increment();
        }
        void buttonXfinSubstract_Click(object sender, EventArgs e)  
        {
            this.numericTextBoxXfin.decrement();
        }
        void buttonXfinAdd_Click(object sender, EventArgs e)        
        {
            this.numericTextBoxXfin.increment();
        }
        void buttonXinitSubstract_Click(object sender, EventArgs e) 
        {
            this.numericTextBoxXinit.decrement();
        }
        void buttonXinitAdd_Click(object sender, EventArgs e)       
        {
            this.numericTextBoxXinit.increment();
        }
        void buttonFilSubstract_Click(object sender, EventArgs e)   
        {
            this.numericTextBoxFilas.decrement();
        }
        void buttonFilAdd_Click(object sender, EventArgs e)         
        {
            this.numericTextBoxFilas.increment();
        }
        void buttonColSubstract_Click(object sender, EventArgs e)   
        {
            this.numericTextBoxCol.decrement();
        }
        void buttonColAdd_Click(object sender, EventArgs e)         
        {
            this.numericTextBoxCol.increment();
        }

        #endregion

        #region "Zonas"

        public void ZonaChanged()                                                   
        {
            unSuscribeEvents();
            clearTextBoxes();
            //ACTUALIZAR LISTBOX
            actualizarListBox();
            suscribeEvents();

        }   //RECIBIR EVENTO DE QUE LA ZONA SELECCIONADA HA CAMBIADO

        void suscribeEvents()                                                       
        {
            this.numericTextBoxXinit.suscribeEvent(updateDivision);
            this.numericTextBoxYinit.suscribeEvent(updateDivision);
            this.numericTextBoxXfin.suscribeEvent(updateDivision);
            this.numericTextBoxYfin.suscribeEvent(updateDivision);

            this.numericTextBoxFilas.suscribeEvent(updateDivision);
            this.numericTextBoxCol.suscribeEvent(updateDivision);
        }   //NUMERICTEXTBOXES
        void unSuscribeEvents()                                                     
        {
            this.numericTextBoxXinit.unsuscribeEvent(updateDivision);
            this.numericTextBoxYinit.unsuscribeEvent(updateDivision);
            this.numericTextBoxXfin.unsuscribeEvent(updateDivision);
            this.numericTextBoxYfin.unsuscribeEvent(updateDivision);

            this.numericTextBoxFilas.unsuscribeEvent(updateDivision);
            this.numericTextBoxCol.unsuscribeEvent(updateDivision);
        }   //NUMERICTEXTBOXES

        void actualizarListBox()                                                    
        {
            if (this._system.selectedZona != null)
            {
                this.listBoxZonas.BeginUpdate();
                this.listBoxZonas.Items.Clear();

                foreach (SubZona d in this._system.selectedZona.Children)
                {
                    //Añadir unicamente los que pertenezcan a esta cámara
                    if (d.ThermoParent.Equals(this.camara))
                        this.listBoxZonas.Items.Add(d.Nombre);
                }

                this.listBoxZonas.Refresh();

                this.listBoxZonas.EndUpdate();

                if (this.selectedIndex != -1)
                    if (this.listBoxZonas.Items.Count > 0 && this.selectedIndex < this.listBoxZonas.Items.Count)
                        this.listBoxZonas.SelectedIndex = this.selectedIndex;
            }
            else
                borrarListBox();
        }   //ACTUALIZAR LISTBOX
        void borrarListBox()                                                        
        {
            this.listBoxZonas.BeginUpdate();
            this.listBoxZonas.Items.Clear();
            this.selectedIndex = -1;
            this.listBoxZonas.EndUpdate();
        }

        void clearTextBoxes()
        {
            this.textBoxDivName.Text        = "";
            this.numericTextBoxXfin.Texto   = "";
            this.numericTextBoxYfin.Texto   = "";
            this.numericTextBoxXinit.Texto  = "";
            this.numericTextBoxYinit.Texto  = "";
            this.numericTextBoxCol.Texto    = "";
            this.numericTextBoxFilas.Texto  = "";
        }

        void camara_DivisionesChanged(object sender, EventArgs e)                   
        {
            actualizarListBox();
        }   //EVENTO CAMARA                     ->  Las subzonas han cambiado
        void listBoxZonas_SelectedIndexChanged(object sender, EventArgs e)          
        {
            if (this.listBoxZonas.SelectedIndex != -1)
            {
                this.selectedIndex = this.listBoxZonas.SelectedIndex;

                if (this.camara.SubZonas.Exists(x => x.Nombre == this.listBoxZonas.SelectedItem.ToString()))
                {
                    SubZona d = this.camara.SubZonas.Where(x => x.Nombre == this.listBoxZonas.SelectedItem.ToString()).First();

                    //updateText(this.textBoxDivName, d.Nombre);

                    this.numericTextBoxXinit.textoCambiado -= updateDivision;
                    this.numericTextBoxYinit.textoCambiado -= updateDivision;
                    this.numericTextBoxXfin.textoCambiado -= updateDivision;
                    this.numericTextBoxYfin.textoCambiado -= updateDivision;
                    this.numericTextBoxCol.textoCambiado -= updateDivision;
                    this.numericTextBoxFilas.textoCambiado -= updateDivision;

                    this.numericTextBoxCol.Texto = d.Columnas.ToString();
                    this.numericTextBoxFilas.Texto = d.Filas.ToString();
                    this.numericTextBoxXinit.Texto = d.Inicio.X.ToString();
                    this.numericTextBoxYinit.Texto = d.Inicio.Y.ToString();
                    this.numericTextBoxXfin.Texto = d.Fin.X.ToString();
                    this.numericTextBoxYfin.Texto = d.Fin.Y.ToString();

                    this.numericTextBoxXinit.textoCambiado += updateDivision;
                    this.numericTextBoxYinit.textoCambiado += updateDivision;
                    this.numericTextBoxXfin.textoCambiado += updateDivision;
                    this.numericTextBoxYfin.textoCambiado += updateDivision;
                    this.numericTextBoxCol.textoCambiado += updateDivision;
                    this.numericTextBoxFilas.textoCambiado += updateDivision;
                }
            }
        }   //ACTUALIZAR DATOS DE LA DIVISION
        void textBoxDivName_TextChanged(object sender, EventArgs e)                 
        {
            //if (this.listBoxZonas.SelectedIndex != -1)
            //{
            //    this.camara.SubZonas[this.listBoxZonas.SelectedIndex].Nombre = this.textBoxDivName.Text;
            //    actualizarListBox();
            //}

        }   //NOMBRE ZONA CHANGED
        void buttonAddSubZone_Click(object sender, EventArgs e)                     
        {
            if (this._system.selectedZona != null && this.textBoxDivName.Text != "")
            {
                unSuscribeEvents();

                //Comprobar que no exista nunguna otra ubzona con ese nombre
                foreach (SubZona sz in this.camara.SubZonas)
                {
                    if (sz.Nombre == this.textBoxDivName.Text)
                    {
                        MessageBox.Show("Ya existe una subzona con el mismo nombre");
                        return;
                    }
                }
                this.selectedIndex = this.camara.SubZonas.Count;

                SubZona s       = new SubZona();
                s.Id = this.camara.SubZonas.Count;
                s.Nombre        = this.textBoxDivName.Text;
                s.Selected      = true;
                s.addCoordinates(new Point(0, 0), new Point(0, 0));
                s.Filas         = 0;
                s.Columnas      = 0;
                s.ThermoParent  = this.camara;

                this._system.selectedZona.addChildren(s);
                this.camara.addDivision(s);

                this.textBoxDivName.Text = "";

                suscribeEvents();
            }

        }   //AÑADIR ZONA
        void buttonRemoveSubZona_Click(object sender, EventArgs e)                  
        {
            if (this.listBoxZonas.SelectedIndex != -1)
            {
                this.selectedIndex = this.camara.SubZonas.Count - 2;
                this.camara.RemoveDivision(this.listBoxZonas.SelectedItem.ToString());
                
            }
        }   //BORRAR ZONA

        void camara_SubZonaRemoved(object sender, string e)
        {
            if (this._system.selectedZona.Children.Exists(x => x.Nombre == e))
                this._system.selectedZona.removeChildren(this._system.selectedZona.Children.Where(x => x.Nombre == e).First());
        }

        #endregion

        #region "PictureBox1"

        void pictureBox1_MouseDown(object sender, MouseEventArgs e)                 
        {
            if (this.camara.Conectado == true && 
                e.Button == System.Windows.Forms.MouseButtons.Left &&
                this.listBoxZonas.SelectedItem != "")
            {
                bmp = (Bitmap)pictureBox1.Image;
                coordenada = new Point(e.X * camara.Width / ((PictureBox)sender).Width,
                                e.Y * camara.Heigth / ((PictureBox)sender).Height);

                this.pictureBox1.MouseUp   += pictureBox1_MouseUp;
                this.pictureBox1.MouseMove += pictureBox1_MouseMove;
                this.pictureBox1.MouseDown -= pictureBox1_MouseDown;

                this.camara.ThermoCamImgReceived -= camara_ThermoCamImgReceived;

                unSuscribeEvents();

                this.numericTextBoxXinit.Texto = coordenada.X.ToString();
                this.numericTextBoxYinit.Texto = coordenada.Y.ToString();

                suscribeEvents();
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

                unSuscribeEvents();

                this.numericTextBoxXfin.Texto = Fin.X.ToString();
                this.numericTextBoxYfin.Texto = Fin.Y.ToString();

                updateDivision(this, null);

                suscribeEvents();

            }
        }   //MOUSE LEFT UP

        void camara_ThermoCamImgReceived(object sender, ThermoCamImgArgs e)         
        {
            bmp = e.Imagen;

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

        private void updateDivision(object sender, EventArgs e)                     
        {
            if (this.listBoxZonas.SelectedIndex != -1 && this._system.selectedZona != null)
            {
                SubZona sub = new SubZona();

                sub.Id          = this.listBoxZonas.SelectedIndex;
                sub.Nombre      = this.listBoxZonas.SelectedItem.ToString();
                sub.Filas       = int.Parse(this.numericTextBoxFilas.Texto);
                sub.Columnas    = int.Parse(this.numericTextBoxCol.Texto);
                sub.addCoordinates(new Point(int.Parse(this.numericTextBoxXinit.Texto), int.Parse(this.numericTextBoxYinit.Texto)), 
                                   new Point(int.Parse(this.numericTextBoxXfin.Texto) , int.Parse(this.numericTextBoxYfin.Texto)));

                //SI NO HAY NINGUNA ZONA SELECCIONADA NO TIENEN QUE APARECER LAS SUBZONAS EN EL LISTBOX
                camara.addDivision(sub);
            }
        }   //UPDATE DIVISION 

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