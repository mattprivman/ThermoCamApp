using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication4.Asistente
{
    public partial class selectAppType : flowControl
    {
        public int NumeroCamaras                                          // -rw    
        {
            get;
            set;
        }

        public selectAppType()                                                      
        {
            InitializeComponent();

            this.Salir = true;

            this.comboBoxAppType.TextChanged += comboBoxAppType_TextChanged;

            this.comboBoxAppType.DataSource = new List<String>()
                {
                    "Standart",
                    "Tuberias",
                    "Coque"
                };
            this.comboBoxNumberCameras.DataSource = new List<int>()
            {
                1,
                2,
                3
            };
        }
        public selectAppType(int nCamaras)
        {
            InitializeComponent();

            this.Salir = true;

            this.comboBoxAppType.TextChanged += comboBoxAppType_TextChanged;

            this.comboBoxAppType.DataSource = new List<String>()
                {
                    "Standart",
                    "Tuberias",
                    "Coque"
                };
            this.comboBoxNumberCameras.DataSource = new List<int>()
            {
                1,
                2,
                3
            };

            this.comboBoxNumberCameras.SelectedIndex = nCamaras - 1;
        }

        void comboBoxAppType_TextChanged(object sender, EventArgs e)                
        {
            //Cambiar modo de funcionamiento de la aplicación
            Helpers.changeAppStringSetting("Mode", this.comboBoxAppType.Text);
        }
        void comboBoxNumberCameras_SelectedIndexChanged(object sender, EventArgs e) 
        {
            this.NumeroCamaras = int.Parse(this.comboBoxNumberCameras.Text.ToString());
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            this.Salir = false;
            this.Close();
        }
    }
}
