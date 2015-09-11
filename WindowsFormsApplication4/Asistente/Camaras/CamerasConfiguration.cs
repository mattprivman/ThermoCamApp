using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThermoVision;
using ThermoVision.CustomControls;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace WindowsFormsApplication4.Asistente.Camaras
{
    public partial class CamerasConfiguration : flowControl
    {
        public CamerasConfiguration(int numerocamaras)
        {
            InitializeComponent(numerocamaras);
            this.Salir = true;
        }

        public CamerasConfiguration(List<ThermoCam> _thermoCams)
        {
            InitializeComponent(_thermoCams.Count, _thermoCams);
            this.Salir = true;
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

        public List<ThermoCam> getThermoCams()
        {
            List<ThermoCam> thermoCams = new List<ThermoCam>();

            foreach (CamConfigControl cc in this.camaras)
            {
                thermoCams.Add(cc.camara);
            }

            return thermoCams;
        }
    }
}
