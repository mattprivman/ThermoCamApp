using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThermoCamApp.Asistente
{
    public partial class selectAppType : flowControl
    {
        public int NumeroCamaras                                          // -rw    
        {
            get;
            set;
        }
        public bool cargarConfiguracion                                   // -rw    
        {
            get;
            set;
        }
        public ThermoVision.Models.Rampa _system                                  
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
        public selectAppType(int nCamaras, ThermoVision.Models.Rampa sistem)
        {
            this._system = sistem;

            InitializeComponent();

            this.Salir = true;


            this.comboBoxAppType.DataSource = new List<String>()
                {
                    "Standart",
                    "Tuberias",
                    "Rampas"
                };

            this.comboBoxAppType.TextChanged += comboBoxAppType_TextChanged;

            if (this._system.Mode != "")
            {
                this.comboBoxAppType.SelectedItem = this._system.Mode;                                
            }
            else
            {
                this.comboBoxAppType.SelectedIndex = 0;
                if(this.comboBoxAppType.Items[0] != null)
                    this._system.Mode = this.comboBoxAppType.Items[0].ToString();
            }


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
            this._system.Mode = this.comboBoxAppType.Text;
            
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

        private void buttonLoadCfg_Click(object sender, EventArgs e)
        {
            this.Salir = false;

            this.openFileDialog1.Title = "Seleccione el arichivo de configuración";
            this.openFileDialog1.Filter = "Archivos de configuración (*.ocl) | *.ocl";
            this.openFileDialog1.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            this.openFileDialog1.FileName = "";
            this.openFileDialog1.ShowDialog();

            if (this.openFileDialog1.FileName != null && this.openFileDialog1.FileName != "")
            {
                ThermoVision.Models.Rampa s = Helpers.deserializeSistema(this.openFileDialog1.FileName);

                if (s != null && s.ThermoCams.Count > 0)
                {
                    this._system = s;

                    this.cargarConfiguracion = true;
                    this.Close();
                }
                else
                    MessageBox.Show("No se pudo cargar el archivo seleccionado",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }
    }
}
