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
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ThermoCamApp.Asistente.Camaras
{
    public partial class CamerasConfiguration : flowControl
    {
        #region "Variables"

        Rampa _system;
        int     selectedIndexZona;
        int     selectedIndexZonaVaciado;
        int     selectedIndexSubZona;

        #endregion

        #region "Propiedades"

        public Rampa Sistema      // -r   
        {
            get 
            {
                return this._system;
            }
        }

        #endregion

        public CamerasConfiguration(int numerocamaras, Rampa _system)         
        {

            this.Salir   = true;
            this._system = _system;

            if (this._system == null)    //Asistente iniciado por primera vez
            {
                this._system = new Rampa();
                InitializeComponent(numerocamaras);
            }
            else
            {
                if (this._system.ThermoCams.Count != numerocamaras)
                {
                    string modo          = this._system.Mode;

                    this._system         = new Rampa();
                    this._system.Mode    = modo;
                    InitializeComponent(numerocamaras);
                }
                else
                {
                    InitializeWithComponent();
                    actualizarListBoxZonas();
                    actualizarListBoxSubZonas();
                }
            }

            this._system.modoConfiguracion = true;

            this.numericTextBoxCol.maxVal   = 100;
            this.numericTextBoxCol.minVal   = 0;
            this.numericTextBoxFil.maxVal   = 100;
            this.numericTextBoxFil.minVal   = 0;

            this.numericTextBoxXIni.maxVal  = 320;
            this.numericTextBoxXIni.minVal  = 0;
            this.numericTextBoxXfin.maxVal  = 320;
            this.numericTextBoxXfin.minVal  = 0;

            this.numericTextBoxYinit.maxVal = 240;
            this.numericTextBoxYinit.minVal = 0;
            this.numericTextBoxYFin.maxVal  = 240;
            this.numericTextBoxYFin.minVal  = 0;

            this._system.zonasListChanged               += _system_zonasListChanged;
            this.listBoxZonas.SelectedIndexChanged      += listBoxZonas_SelectedIndexChanged;
            this.listBoxSubZonas.SelectedIndexChanged   += listBoxSubZonas_SelectedIndexChanged;

            //Suscribir eventos de camCOnfigControl para recibir coordenadas
            foreach (CamConfigControl c in camaras)
            {
                c.CoordenadasGeneradas += c_CoordenadasGeneradas;
            }

            //EVENTOS BOTONES CONFIGURACIÓN SUBZONA
            this.buttonAddXIni.Click        += new System.EventHandler(this.buttonAddXIni_Click);
            this.buttonSubstractXIni.Click  += new System.EventHandler(this.buttonSubstractXIni_Click);
            this.buttonSubstractYini.Click  += new System.EventHandler(this.buttonSubstractYini_Click);
            this.buttonAddYini.Click        += new System.EventHandler(this.buttonAddYini_Click);
            this.buttonSubstractXfin.Click  += new System.EventHandler(this.buttonSubstractXfin_Click);
            this.buttonAddXFin.Click        += new System.EventHandler(this.buttonAddXFin_Click);
            this.buttonSubstractYFin.Click  += new System.EventHandler(this.buttonSubstractYFin_Click);
            this.buttonAddYFin.Click        += new System.EventHandler(this.buttonAddYFin_Click);
            this.buttonSubstracFilas.Click  += new System.EventHandler(this.buttonSubstracFilas_Click);
            this.buttonAddFilas.Click       += new System.EventHandler(this.buttonAddFilas_Click);
            this.buttonSubstractCol.Click   += new System.EventHandler(this.buttonSubstractCol_Click);
            this.buttonAddCol.Click         += new System.EventHandler(this.buttonAddCol_Click);

            this._system.SelectedZona = null;

            this.selectedIndexZona          = -1;
            this.selectedIndexZonaVaciado   = -1;
            this.selectedIndexSubZona       = -1;

            actualizarListBoxZonas();
            actualizarListBoxSubZonas();

            //COnfiguración de las zonas
            systemModeConfiguration();
        }

        #region "BOTONES CONTROL FLUJO"

        private void buttonAtras_Click(object sender, System.EventArgs e)       
        {
            //DESCONECTAR CAMARAS
            foreach (ThermoCam t in this._system.ThermoCams)
            {
                t.Desconectar();
            }
            this.Salir = false;
            this.Atras = true;
            this.Close();
        }
        private void buttonNext_Click(object sender, System.EventArgs e)        
        {
            //Comprobación de modos
            switch (this._system.Mode)
            {
                case "Rampas":
                    
                    break;
                case "Tuberias":

                    break;
            }

            //DESCONECTAR CAMARAS
            foreach (ThermoCam t in this._system.ThermoCams)
            {
                t.Desconectar();
            }

            this.Salir = false;
            this.Atras = false;
            this.Close();
        }

        #endregion
        
        #region "ZONAS"

        void listBoxZonas_SelectedIndexChanged(object sender, EventArgs e)      
        {
            this.selectedIndexZona = this.listBoxZonas.SelectedIndex;
            this.selectedIndexSubZona = -1;
            this.selectedIndexZonaVaciado = -1;

            if (this.listBoxZonas.SelectedIndex != -1)
            {
                if (this._system.Mode == "Rampas")
                {
                    foreach (Zona z in this._system.Zonas)
                    {
                        foreach (SubZona s in z.Children)
                            s.Visualizar = true;
                    }
                    foreach (Zona z in this._system.ZonasVaciado)
                    {
                        foreach (SubZona s in z.Children)
                            s.Visualizar = false;
                    }
                }
                else
                {
                    this._system.selectZona(this.listBoxZonas.SelectedItem.ToString());
                }

                //Actualizar listBoxes Zonas
                actualizarListBoxZonas();
            }
        }
        void _system_zonasListChanged(object sender, EventArgs e)               
        {
            actualizarListBoxZonas();
        }    

        private void buttonAddZona_Click(object sender, EventArgs e)            
        {
            //Añadir zona
            //try
            //{
                CustomControls.AddZona f = new CustomControls.AddZona();
                f.ShowDialog();

                if (f.zonaName != null && f.zonaName != "")
                {
                    this.selectedIndexZona = this.listBoxZonas.Items.Count;
                    this.selectedIndexZonaVaciado = -1;
                    this._system.addZona(new ZonaApagado(f.zonaName.Trim(), this._system));
                    f.Dispose();
                    actualizarListBoxZonas();

                    return;
                }
            //}
            //catch (Exception ex)
            //{
                MessageBox.Show("Ya existe una zona con el mismo nombre", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }
        private void buttonBorrarZona_Click(object sender, EventArgs e)         
        {
            //Borrar zona
            if (this.listBoxZonas.SelectedIndex != -1)
            {
                this.selectedIndexZona--;
                this.selectedIndexZonaVaciado = -1;
                this._system.removeZona((ZonaApagado)this._system.getZona(this.listBoxZonas.SelectedItem.ToString()));
            }
        }

        #endregion        

        #region "ZONAS VACIADO"

        void listBoxZonasApagado_SelectedIndexChanged(object sender, EventArgs e)
        {
                this.selectedIndexZona = -1;
                this.selectedIndexSubZona = -1;
                this.selectedIndexZonaVaciado = this.listBoxZonasVaciado.SelectedIndex;

                if (this.listBoxZonasVaciado.SelectedIndex != -1)
                {
                    if (this._system.Mode == "Rampas")
                    {
                        foreach (Zona z in this._system.Zonas)
                        {
                            foreach (SubZona s in z.Children)
                                s.Visualizar = false;
                        }
                        foreach (Zona z in this._system.ZonasVaciado)
                        {
                            foreach (SubZona s in z.Children)
                                s.Visualizar = true;
                        }
                    }
                    else
                    {
                        this._system.selectZona(this.listBoxZonas.SelectedItem.ToString());
                    }

                    //Actualizar listBoxes Zonas
                    actualizarListBoxZonas();
                }
        }

        private void buttonAddZonaVaciado_Click(object sender, EventArgs e)     
        {
            //Añadir zona
            try
            {
                CustomControls.AddZona f = new CustomControls.AddZona();
                f.ShowDialog();

                if (f.zonaName != null && f.zonaName != "")
                {
                    this.selectedIndexZona = -1;
                    this.selectedIndexZonaVaciado = this.listBoxZonasVaciado.Items.Count;
                    this._system.addZonaVaciado(new ZonaVaciado(f.zonaName.Trim(), this._system));
                    f.Dispose();
                    actualizarListBoxZonas();

                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ya existe una zona con el mismo nombre", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void buttonBorrarZonaVaciado_Click(object sender, EventArgs e)  
        {
            if (this.listBoxZonasVaciado.SelectedIndex != -1)
            {
                //Borrar zona
                this.selectedIndexZona = -1;
                this.selectedIndexZonaVaciado--;
                this._system.removeZonaVaciado((ZonaVaciado)this._system.getZonaVaciado(this.listBoxZonasVaciado.SelectedItem.ToString()));
            }
        }

        #endregion

        void actualizarListBoxZonas()                                           
        {
            this.listBoxZonas.SelectedIndexChanged -= listBoxZonas_SelectedIndexChanged;
            this.listBoxZonasVaciado.SelectedIndexChanged -= listBoxZonasApagado_SelectedIndexChanged;

            this.listBoxZonas.BeginUpdate();
            this.listBoxZonasVaciado.BeginUpdate();

            this.listBoxZonas.Items.Clear();
            this.listBoxZonasVaciado.Items.Clear();

            //Actualizar listBox de zonas de apagado
            if (this._system.Zonas.Count > 0)
            {
                foreach (Zona z in this._system.Zonas)
                {
                    this.listBoxZonas.Items.Add(z.Nombre);
                }
            }
            //Actualizar listBox de zonas de Vaciado
            if (this._system.ZonasVaciado.Count > 0)
            {
                foreach (Zona z in this._system.ZonasVaciado)
                {
                    this.listBoxZonasVaciado.Items.Add(z.Nombre);
                }
            }

            if (this.selectedIndexZona >= 0 && this.listBoxZonas.Items.Count > 0)
            {
                this.listBoxZonas.SelectedIndex = this.selectedIndexZona;
                this._system.selectZona(this.listBoxZonas.SelectedItem.ToString());
            }
            if (this.selectedIndexZonaVaciado >= 0 && this.listBoxZonasVaciado.Items.Count > 0)
            {
                this.listBoxZonasVaciado.SelectedIndex = this.selectedIndexZonaVaciado;
                this._system.selectZonaVaciado(this.listBoxZonasVaciado.SelectedItem.ToString());
            }

            actualizarListBoxSubZonas();

            this.listBoxZonas.EndUpdate();
            this.listBoxZonas.Update();
            this.listBoxZonasVaciado.EndUpdate();
            this.listBoxZonasVaciado.Update();

            this.listBoxZonas.SelectedIndexChanged        += listBoxZonas_SelectedIndexChanged;
            this.listBoxZonasVaciado.SelectedIndexChanged += listBoxZonasApagado_SelectedIndexChanged;
        }

        #region "Eventos numericTextBoxes"

        //void suscribeEvents()
        //{
        //    this.numericTextBoxCol.textoCambiado += numericTextBoxCol_textoCambiado;
        //    this.numericTextBoxFil.textoCambiado += numericTextBoxFil_textoCambiado;
        //    this.numericTextBoxXfin.textoCambiado += numericTextBoxXfin_textoCambiado;
        //    this.numericTextBoxXIni.textoCambiado += numericTextBoxXIni_textoCambiado;
        //    this.numericTextBoxYinit.textoCambiado += numericTextBoxYinit_textoCambiado;
        //    this.numericTextBoxYFin.textoCambiado += numericTextBoxYFin_textoCambiado;
        //}//NUMERIC TEXTBOX
        //void unsusribeEvents()
        //{
        //    this.numericTextBoxCol.textoCambiado += numericTextBoxCol_textoCambiado;
        //    this.numericTextBoxFil.textoCambiado += numericTextBoxFil_textoCambiado;
        //    this.numericTextBoxXfin.textoCambiado += numericTextBoxXfin_textoCambiado;
        //    this.numericTextBoxXIni.textoCambiado += numericTextBoxXIni_textoCambiado;
        //    this.numericTextBoxYinit.textoCambiado += numericTextBoxYinit_textoCambiado;
        //    this.numericTextBoxYFin.textoCambiado += numericTextBoxYFin_textoCambiado;
        //}   //NUMERIC TEXTBOX

        private void buttonAddXIni_Click(object sender, EventArgs e)            
        {
            //Añadir X ini
            SubZona s = this.getSelectedSubZona();

            if (s != null)
            {
                s.addCoordinates(new Point(s.Inicio.X + 1, s.Inicio.Y),
                                        s.Fin);
            }

        }
        private void buttonSubstractXIni_Click(object sender, EventArgs e)      
        {
            //Quitar X ini
            SubZona s = this.getSelectedSubZona();

            if (s != null)
            {
                s.addCoordinates(new Point(s.Inicio.X - 1, s.Inicio.Y),
                                        s.Fin);
            }

        }
        private void buttonAddYini_Click(object sender, EventArgs e)            
        {
            //Añadir Y ini
            SubZona s = this.getSelectedSubZona();

            if (s != null)
            {
                s.addCoordinates(new Point(s.Inicio.X, s.Inicio.Y + 1),
                                        s.Fin);
            }

        }
        private void buttonSubstractYini_Click(object sender, EventArgs e)      
        {
            //Quitar Y ini
            SubZona s = this.getSelectedSubZona();

            if (s != null)
            {
                s.addCoordinates(new Point(s.Inicio.X, s.Inicio.Y - 1),
                                        s.Fin);
            }

        }
        private void buttonAddXFin_Click(object sender, EventArgs e)            
        {
            //Añadir X fin
            SubZona s = this.getSelectedSubZona();

            if (s != null)
            {
                s.addCoordinates(s.Inicio,
                            new Point(s.Fin.X + 1, s.Fin.Y));
            }

        }
        private void buttonSubstractXfin_Click(object sender, EventArgs e)      
        {
            //Quitar Y fin
            SubZona s = this.getSelectedSubZona();

            if (s != null)
            {
                s.addCoordinates(s.Inicio,
                            new Point(s.Fin.X - 1, s.Fin.Y));
            }

        }
        private void buttonAddYFin_Click(object sender, EventArgs e)            
        {
            //Añadir Y fin
            SubZona s = this.getSelectedSubZona();

            if (s != null)
            {
                s.addCoordinates(s.Inicio,
                                            new Point(s.Fin.X, s.Fin.Y + 1));
            }

        }
        private void buttonSubstractYFin_Click(object sender, EventArgs e)      
        {
            //Quitar Y fin
            SubZona s = this.getSelectedSubZona();

            if (s != null)
            {
                s.addCoordinates(s.Inicio,
                                            new Point(s.Fin.X, s.Fin.Y - 1));
            }

        }
        private void buttonAddFilas_Click(object sender, EventArgs e)           
        {
            //Añadir fila
            SubZona s = this.getSelectedSubZona();

            if (s != null)
            {
                s.Filas++;
            }
        }
        private void buttonSubstracFilas_Click(object sender, EventArgs e)      
        {
            //Substract fila
            SubZona s = this.getSelectedSubZona();

            if (s != null)
            {
                s.Filas--;
            }
        }
        private void buttonAddCol_Click(object sender, EventArgs e)             
        {
            //Añadir columna
            SubZona s = this.getSelectedSubZona();

            if (s != null)
            {
                s.Columnas++;
            }
        }
        private void buttonSubstractCol_Click(object sender, EventArgs e)       
        {
            //Substract col
            SubZona s = this.getSelectedSubZona();

            if (s != null)
            {
                s.Columnas--;
            }

        }

        void numericTextBoxCol_textoCambiado(object sender, EventArgs e)        
        {
            SubZona s = getSelectedSubZona();

            if (s != null)
            {
                s.Columnas = int.Parse(this.numericTextBoxCol.Texto);
            }
        }
        void numericTextBoxXfin_textoCambiado(object sender, EventArgs e)       
        {
            SubZona s = getSelectedSubZona();

            if (s != null)
            {
                s.addCoordinates(s.Inicio,
                                     new Point(int.Parse(this.numericTextBoxXfin.Texto), s.Fin.Y));
            }
        } 
        void numericTextBoxFil_textoCambiado(object sender, EventArgs e)        
        {
            SubZona s = getSelectedSubZona();

            if (s != null)
            {
                s.Filas = int.Parse(this.numericTextBoxFil.Texto);
            }
        }
        void numericTextBoxXIni_textoCambiado(object sender, EventArgs e)       
        {
            SubZona s = getSelectedSubZona();

            if (s != null)
            {
                s.addCoordinates(new Point(int.Parse(this.numericTextBoxXIni.Texto), s.Inicio.Y),
                                    s.Fin);
            }
        }
        void numericTextBoxYFin_textoCambiado(object sender, EventArgs e)       
        {
            SubZona s = getSelectedSubZona();

            if (s != null)
            {
                s.addCoordinates(s.Inicio,
                                    new Point(s.Fin.X, int.Parse(this.numericTextBoxYFin.Texto)));
            }
        }
        void numericTextBoxYinit_textoCambiado(object sender, EventArgs e)      
        {
            SubZona s = getSelectedSubZona();

            if (s != null)
            {
                s.addCoordinates(new Point(s.Inicio.X, int.Parse(this.numericTextBoxXIni.Texto)),
                                    s.Fin);
            }
        }

        #endregion

        #region "SubZonas"

        private SubZona getSelectedSubZona()                                    
        {
            if (this.listBoxSubZonas.SelectedIndex != -1)
            {
                if (this._system.SelectedZona != null)
                {
                    return this._system.SelectedZona.getChildren(this.listBoxSubZonas.SelectedItem.ToString());
                }
            }
            return null;
        }

        private void buttonAddSubZona_Click(object sender, EventArgs e)         
        {
            if (this._system.SelectedZona != null)
            {
                CustomControls.AddSubZona f = new CustomControls.AddSubZona();
                f.ShowDialog();

                if (f.nameSubZona != "" && f.nameSubZona != null)
                {
                    try
                    {
                        this._system.SelectedZona.addChildren(new SubZona(f.nameSubZona.Trim()));
                        this.selectedIndexSubZona = this.listBoxSubZonas.Items.Count;
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                f.Dispose();

                actualizarListBoxSubZonas();
            }
            else
            {
                MessageBox.Show("Debe seleccionar una zona.");
            }

        }
        private void buttonBorrarSubZona_Click(object sender, EventArgs e)      
        {
            if (this.listBoxSubZonas.SelectedIndex != -1)
            {
                this.selectedIndexSubZona--;

                if (this._system.SelectedZona != null)
                {
                    this._system.SelectedZona.removeChildren(this._system.SelectedZona.getChildren(this.listBoxSubZonas.SelectedItem.ToString()));
                    actualizarListBoxSubZonas();
                }
            }
        }

        void actualizarListBoxSubZonas()                                        
        {
            this.listBoxSubZonas.BeginUpdate();
            this.listBoxSubZonas.Items.Clear();

            if (this._system.SelectedZona != null)
            {
                foreach (SubZona s in this._system.SelectedZona.Children)
                {
                    this.listBoxSubZonas.Items.Add(s.Nombre);
                }
            }

            if(this.selectedIndexSubZona >= 0 && this.listBoxSubZonas.Items.Count > 0 && this.selectedIndexSubZona < this.listBoxSubZonas.Items.Count)
            {
                this.listBoxSubZonas.SelectedIndex = this.selectedIndexSubZona;
            }

            this.listBoxSubZonas.EndUpdate();
            this.listBoxSubZonas.Update();
        }
        void listBoxSubZonas_SelectedIndexChanged(object sender, EventArgs e)   
        {
            this.selectedIndexSubZona = this.listBoxSubZonas.SelectedIndex;

            if (this.listBoxSubZonas.SelectedIndex != -1)
            {
                //this._system.selectedZona.Sub
                foreach (SubZona s in this._system.SelectedZona.Children)
                {
                        s.ParametersChanged -= sub_ParametersChanged;
                }

                SubZona sub            = this.getSelectedSubZona();
                sub.ParametersChanged += sub_ParametersChanged;

                sub_ParametersChanged(this, null);
            }
        }

        void sub_ParametersChanged(object sender, EventArgs e)                  
        {
            //unsusribeEvents();

            SubZona s = getSelectedSubZona();

            if (s != null)
            {
                this.numericTextBoxCol.Texto = s.Columnas.ToString();
                this.numericTextBoxFil.Texto = s.Filas.ToString();
                this.numericTextBoxXIni.Texto = s.Inicio.X.ToString();
                this.numericTextBoxXfin.Texto = s.Fin.X.ToString();
                this.numericTextBoxYinit.Texto = s.Inicio.Y.ToString();
                this.numericTextBoxYFin.Texto = s.Fin.Y.ToString();
            }

            //suscribeEvents();
        }

        void c_CoordenadasGeneradas(ThermoCam sender, CamConfigControl.coordenadasEventArgs e) 
        {
            SubZona s       = getSelectedSubZona();

            

            if (s != null)
            {
                s.ThermoParent = sender;
                s.Selected = true;

                s.addCoordinates(e.Inicio, e.Fin);

                
            }//if
        }

        #endregion

        #region "Configurar modo"

        private void systemModeConfiguration()                                  
        {
            switch (this._system.Mode)
            {
                case "Rampas":
                  

                    break;
                case "Tuberias":

                    break;
            }//switch
        }

        #endregion
    }
}
