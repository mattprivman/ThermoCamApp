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

            this.listBoxZonas.SelectedIndexChanged += listBoxZonas_SelectedIndexChanged;
            this.buttonAddZone.Click += buttonAddZone_Click;
            this.buttonRemoveZona.Click += buttonRemoveZona_Click;
        }

        void listBoxZonas_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._system.selectZona(this.listBoxZonas.Items[this.listBoxZonas.SelectedIndex].ToString());
        }

        void buttonAddZone_Click(object sender, EventArgs e)
        {
            if (this.textBoxZonaName.Text != "")
            {
                //Comprobar que no haya una zona con el mismo nombre
                foreach (var item in this.listBoxZonas.Items)
                {
                    if (item.Equals(this.textBoxZonaName.Text))
                    {
                        MessageBox.Show("Ya hay una zona con el nombre " + this.textBoxZonaName.Text + ".",
                            "Error", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error);
                        return;
                    }
                }

                this._system.addZona(new Zona(this.textBoxZonaName.Text, this._system));

                this.listBoxZonas.BeginUpdate();

                this.listBoxZonas.Items.Add(this.textBoxZonaName.Text);

                if (selectedIndex > 0)
                    this.listBoxZonas.SelectedIndex = this.selectedIndex;

                this.listBoxZonas.EndUpdate();
                this.listBoxZonas.Refresh();

                //Anunciar que hay una zona
            }
        }

        void buttonRemoveZona_Click(object sender, EventArgs e)
        {
            if (this.listBoxZonas.SelectedIndex != -1)
            {
                this.selectedIndex = this.listBoxZonas.SelectedIndex;

                this.listBoxZonas.BeginUpdate();

                this.listBoxZonas.Items.Remove(this.listBoxZonas.SelectedItem);
                this.selectedIndex --;

                if (this.selectedIndex > 0)
                    this.listBoxZonas.SelectedIndex = this.selectedIndex;

                this.listBoxZonas.EndUpdate();
                this.listBoxZonas.Refresh();

                //Anunciar que se ha borrado una zona
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
