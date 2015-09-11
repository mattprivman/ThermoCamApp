using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication4.Asistente.Camaras
{
    public partial class CameraNumberSelection : flowControl
    {
        public int NumeroCamaras { get; set; } 

        public CameraNumberSelection()
        {
            InitializeComponent();

            Salir = true;
            this.comboBoxNumberCameras.DataSource = new List<int>(){
                1,
                2,
                3
            };
        }

        private void comboBoxNumberCameras_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.NumeroCamaras = int.Parse(this.comboBoxNumberCameras.Text.ToString());
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            this.Salir = false;
            this.Atras = false;

            this.Close();
        }
    }
}
