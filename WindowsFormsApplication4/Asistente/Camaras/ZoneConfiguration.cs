using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ThermoVision.Models;

namespace WindowsFormsApplication4.Asistente.Camaras
{
    public partial class ZoneConfiguration : flowControl
    {
        Sistema _system;
        int     nZonas;

        public Sistema Sistema            // -r  
        {
            get     
            {
                return this._system;
            }
        }

        public ZoneConfiguration(int nZonas)    
        {
            this.Salir = true;

            this.nZonas = nZonas;

            

            this._system = new Sistema();
            InitializeComponent();
        }

        void textBox_TextChanged(object sender, System.EventArgs e)
        {
            CustomControls.customTextBox c = (CustomControls.customTextBox)sender;

            this._system.Zonas[c.Id].Nombre = c.textBox.Text;
        }


        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Salir = false;
            this.Atras = true;
            this.Close();
        }
        private void buttonNext_Click(object sender, EventArgs e)
        {
            this.Salir = false;
            this.Atras = false;
            this.Close();
        }
    }
}
