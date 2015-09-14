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
        Sistema _System;

        public CamerasConfiguration(int numerocamaras, Sistema system)
        {
            this._System = system;

            this.Salir = true;
            InitializeComponent(numerocamaras);
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
