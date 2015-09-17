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

namespace WindowsFormsApplication4.Asistente.Camaras
{
    public partial class CamerasConfiguration : flowControl
    {
        #region "Variables"

        Sistema _system;
        int     selectedIndexZona;
        int     selectedIndexSubZona;

        #endregion

        #region "Propiedades"

        public Sistema Sistema      // -r   
        {
            get 
            {
                return this._system;
            }
        }

        #endregion

        public CamerasConfiguration(int numerocamaras, Sistema _system)         
        {
            this.Salir   = true;
            this._system = _system;

            if (this._system == null)    //Asistente iniciado por primera vez
            {
                this._system = new Sistema();
                InitializeComponent(numerocamaras);
            }

            this._system.zonasListChanged               += _system_zonasListChanged;
            this.listBoxZonas.SelectedIndexChanged      += listBoxZonas_SelectedIndexChanged;
            this.listBoxSubZonas.SelectedIndexChanged   += listBoxSubZonas_SelectedIndexChanged;

        }

        #region "BOTONES CONTROL FLUJO"

        private void buttonAtras_Click(object sender, System.EventArgs e)   
        {
            this.Salir = false;
            this.Atras = true;
            this.Close();
        }
        private void buttonNext_Click(object sender, System.EventArgs e)    
        {
            this.Salir = false;
            this.Atras = false;
            this.Close();
        }

        #endregion
        
        #region "ZONAS"

        void actualizarListBoxZonas()       
        {
            this.listBoxZonas.BeginUpdate();
            this.listBoxZonas.Items.Clear();

            if (this._system.Zonas.Count > 0)
            {                
                foreach (Zona z in this._system.Zonas)
                {
                    this.listBoxZonas.Items.Add(z.Nombre);
                }
            }

            if (this.selectedIndexZona >= 0 && this.listBoxZonas.Items.Count > 0)
            {
                this.listBoxZonas.SelectedIndex = this.selectedIndexZona;
            }
            
            this.listBoxZonas.EndUpdate();
            this.listBoxZonas.Update();
        }
        
        void listBoxZonas_SelectedIndexChanged(object sender, EventArgs e)      
        {
            this.selectedIndexZona = this.listBoxZonas.SelectedIndex;

            this._system.selectZona(this.listBoxZonas.SelectedItem.ToString());

            //Actualizar listBox SubZonas
            actualizarListBoxSubZonas();
            
        }
        void _system_zonasListChanged(object sender, EventArgs e)               
        {
            actualizarListBoxZonas();
        }    

        private void buttonAddZona_Click(object sender, EventArgs e)            
        {
            //Añadir zona
            try
            {
                CustomControls.AddZona f = new CustomControls.AddZona();
                f.ShowDialog();

                if (f.zonaName != null && f.zonaName != "")
                {
                    this.selectedIndexZona = this.listBoxZonas.Items.Count;
                    this._system.addZona(new Zona(f.zonaName, this._system));
                    f.Dispose();

                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ya existe una zona con el mismo nombre", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void buttonBorrarZona_Click(object sender, EventArgs e)         
        {
            //Borrar zona
            this.selectedIndexZona--;
            this._system.removeZona(this._system.getZona(this.listBoxZonas.SelectedItem.ToString()));

        }

        #endregion        

        #region "Eventos numericTextBoxes"

        void suscribeEvents()                                               
        {
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
        }   //NUMERIC TEXTBOX
        void unsusribeEvents()                                              
        {
            this.buttonAddXIni.Click        -= new System.EventHandler(this.buttonAddXIni_Click);
            this.buttonSubstractXIni.Click  -= new System.EventHandler(this.buttonSubstractXIni_Click);
            this.buttonSubstractYini.Click  -= new System.EventHandler(this.buttonSubstractYini_Click);
            this.buttonAddYini.Click        -= new System.EventHandler(this.buttonAddYini_Click);
            this.buttonSubstractXfin.Click  -= new System.EventHandler(this.buttonSubstractXfin_Click);
            this.buttonAddXFin.Click        -= new System.EventHandler(this.buttonAddXFin_Click);
            this.buttonSubstractYFin.Click  -= new System.EventHandler(this.buttonSubstractYFin_Click);
            this.buttonAddYFin.Click        -= new System.EventHandler(this.buttonAddYFin_Click);
            this.buttonSubstracFilas.Click  -= new System.EventHandler(this.buttonSubstracFilas_Click);
            this.buttonAddFilas.Click       -= new System.EventHandler(this.buttonAddFilas_Click);
            this.buttonSubstractCol.Click   -= new System.EventHandler(this.buttonSubstractCol_Click);
            this.buttonAddCol.Click         -= new System.EventHandler(this.buttonAddCol_Click);
        }   //NUMERIC TEXTBOX

        private void buttonAddXIni_Click(object sender, EventArgs e)        
        {
            //Añadir X ini

        }
        private void buttonSubstractXIni_Click(object sender, EventArgs e)  
        {
            //Quitar X ini

        }
        private void buttonAddYini_Click(object sender, EventArgs e)        
        {
            //Añadir Y ini

        }
        private void buttonSubstractYini_Click(object sender, EventArgs e)  
        {
            //Quitar Y ini

        }
        private void buttonAddXFin_Click(object sender, EventArgs e)        
        {
            //Añadir X fin

        }
        private void buttonSubstractXfin_Click(object sender, EventArgs e)  
        {
            //Quitar Y fin

        }
        private void buttonAddYFin_Click(object sender, EventArgs e)        
        {
            //Añadir Y fin

        }
        private void buttonSubstractYFin_Click(object sender, EventArgs e)  
        {
            //Quitar Y fin

        }
        private void buttonAddFilas_Click(object sender, EventArgs e)       
        {
            //Añadir fila

        }
        private void buttonSubstracFilas_Click(object sender, EventArgs e)  
        {
            //Substract fila

        }
        private void buttonAddCol_Click(object sender, EventArgs e)         
        {
            //Añadir columna

        }
        private void buttonSubstractCol_Click(object sender, EventArgs e)   
        {
            //Substract col

        }
        #endregion

        #region "SubZonas"

        private void buttonAddSubZona_Click(object sender, EventArgs e)         
        {
            if (this._system.selectedZona != null)
            {
                this.selectedIndexSubZona = this.listBoxSubZonas.Items.Count;

                CustomControls.AddSubZona f = new CustomControls.AddSubZona();
                f.ShowDialog();

                if(f.nameSubZona != "" && f.nameSubZona != null)
                    this._system.selectedZona.addChildren(new SubZona(f.nameSubZona));

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
            this.selectedIndexSubZona--;

            if (this._system.selectedZona != null)
            {
                this._system.selectedZona.removeChildren(this._system.selectedZona.getChildren(this.listBoxSubZonas.SelectedItem.ToString()));
                actualizarListBoxSubZonas();
            }
        }

        void actualizarListBoxSubZonas()                                        
        {
            this.listBoxSubZonas.BeginUpdate();
            this.listBoxSubZonas.Items.Clear();

            if (this._system.selectedZona != null)
            {
                foreach (SubZona s in this._system.selectedZona.Children)
                {
                    this.listBoxSubZonas.Items.Add(s.Nombre);
                }
            }

            if(this.selectedIndexSubZona >= 0 && this.listBoxSubZonas.Items.Count > 0 )
            {
                this.listBoxSubZonas.SelectedIndex = this.selectedIndexSubZona;
            }

            this.listBoxSubZonas.EndUpdate();
            this.listBoxSubZonas.Update();
        }
        void listBoxSubZonas_SelectedIndexChanged(object sender, EventArgs e)   
        {
            this.selectedIndexSubZona = this.listBoxSubZonas.SelectedIndex;
        }

        #endregion
    }
}
