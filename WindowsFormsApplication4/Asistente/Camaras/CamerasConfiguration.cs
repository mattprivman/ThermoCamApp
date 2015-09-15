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
        Sistema _system;
        int     selectedIndex;

        public CamerasConfiguration(int numerocamaras, Sistema _system)
        {
            this._system = _system;

            this.Salir   = true;
            InitializeComponent(numerocamaras);

            this._system.zonasListChanged += _system_zonasListChanged;

            this.listBoxZonas.SelectedIndexChanged  += listBoxZonas_SelectedIndexChanged;
            this.buttonAddZone.Click                += buttonAddZone_Click;
            this.buttonRemoveZona.Click             += buttonRemoveZona_Click;
        }

        void _system_zonasListChanged(object sender, EventArgs e)           
        {
            this.listBoxZonas.BeginUpdate();
            this.listBoxZonas.Items.Clear();
            //Actualizar listBox
            foreach (Zona z in this._system.Zonas)
            {
                this.listBoxZonas.Items.Add(z.Nombre);
            }

            //Seleccionar item
            if (this.selectedIndex != -1 && this.selectedIndex > 0 && this.selectedIndex < this.listBoxZonas.Items.Count)
            {
                this.listBoxZonas.SelectedIndex = this.selectedIndex;
            }
            this.listBoxZonas.EndUpdate();

            this.listBoxZonas.Update();
        }
        void listBoxZonas_SelectedIndexChanged(object sender, EventArgs e)  
        {
            if (this.listBoxZonas.SelectedIndex != -1)
            {
                this._system.selectZona(this.listBoxZonas.Items[this.listBoxZonas.SelectedIndex].ToString());

                //Notificar a los controles que la zona ha cambiado
                foreach (ThermoVision.CustomControls.CamConfigControl cc in camaras)
                {
                    cc.ZonaChanged();
                }
            }
            else
            {
                this._system.selectedZona = null;
            }
        }
        void buttonAddZone_Click(object sender, EventArgs e)                
        {
            lock("Zonas")
            {
                if (this.textBoxZonaName.Text != "")
                {
                    try
                    {
                        this._system.addZona(new Zona(this.textBoxZonaName.Text, this._system));

                        //Actualizar listBox
                        if (this.listBoxZonas.SelectedIndex != -1)
                        {
                            this.selectedIndex = this._system.Zonas.Count;
                        }

                        //Borrar textBOx
                        this.textBoxZonaName.Text = "";

                        //Notificar a los controles que la zona ha cambiado
                        foreach (ThermoVision.CustomControls.CamConfigControl cc in camaras)
                        {
                            cc.ZonaChanged();
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ya existe una zona con ese nombre", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } //IF (this.textBoxZonaName.Text != "")
            }
        }
        void buttonRemoveZona_Click(object sender, EventArgs e)             
        {
            if (this.listBoxZonas.SelectedIndex != -1)
            {
                lock ("Zonas")
                {
                    Zona z = this._system.getZona(this.listBoxZonas.SelectedItem.ToString());

                    if (z != null)
                        this._system.removeZona(z);

                    //Borrar textBOx
                    this.textBoxZonaName.Text = "";

                    //Notificar a los controles que la zona ha cambiado
                    foreach (ThermoVision.CustomControls.CamConfigControl cc in camaras)
                    {
                        cc.ZonaChanged();
                    }
                }
            }
        }


        #region "BOTONES"

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
    }
}
