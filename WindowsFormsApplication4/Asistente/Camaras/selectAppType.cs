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
        public bool cargarConfiguracion                                   // -rw    
        {
            get;
            set;
        }
        public ThermoVision.Models.Sistema _system                                  
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

            this.labelNumeroCañones.Visible     = false;
            this.comboBoxNumeroCañones.Visible  = false;
            this.comboBoxNumeroCañones.DataSource = new List<int>()
            {
                1,
                2,
                3
            };

            this.comboBoxNumeroCañones.SelectedIndexChanged += new System.EventHandler(this.comboBoxNumeroCañones_SelectedIndexChanged);
        }
        public selectAppType(int nCamaras, ThermoVision.Models.Sistema sistem)
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

                if (this._system.Mode == "Rampas")
                {
                    this.labelNumeroCañones.Visible = true;
                    this.comboBoxNumeroCañones.Visible = true;
                }
                else
                {
                    this.labelNumeroCañones.Visible = false;
                    this.comboBoxNumeroCañones.Visible = false;
                }
            }
            else
            {
                this.comboBoxAppType.SelectedIndex = 0;
                if(this.comboBoxAppType.Items[0] != null)
                    this._system.Mode = this.comboBoxAppType.Items[0].ToString();

                this.labelNumeroCañones.Visible = false;
                this.comboBoxNumeroCañones.Visible = false;
            }


            this.comboBoxNumberCameras.DataSource = new List<int>()
            {
                1,
                2,
                3
            };

            this.comboBoxNumeroCañones.DataSource = new List<int>()
            {
                1,
                2,
                3
            };

            this.comboBoxNumberCameras.SelectedIndex = nCamaras - 1;

            if (this._system.Cannons.Count > 0)
                this.comboBoxNumeroCañones.SelectedIndex = this._system.Cannons.Count - 1;

            this.comboBoxNumeroCañones.SelectedIndexChanged += new System.EventHandler(this.comboBoxNumeroCañones_SelectedIndexChanged);
        }

        void comboBoxAppType_TextChanged(object sender, EventArgs e)                
        {
            //Cambiar modo de funcionamiento de la aplicación
            this._system.Mode = this.comboBoxAppType.Text;

            if (this._system.Mode == "Rampas")
            {
                this.labelNumeroCañones.Visible = true;
                this.comboBoxNumeroCañones.Visible = true;
            }
            else
            {
                this.labelNumeroCañones.Visible = false;
                this.comboBoxNumeroCañones.Visible = false;
            }
        }
        void comboBoxNumberCameras_SelectedIndexChanged(object sender, EventArgs e) 
        {
            this.NumeroCamaras = int.Parse(this.comboBoxNumberCameras.Text.ToString());
        }
        void comboBoxNumeroCañones_SelectedIndexChanged(object sender, EventArgs e) 
        {
            if (this._system.Mode == "Rampas")
            {
                if (this._system.Cannons != null)
                    this._system.Cannons.Clear();

                for (int i = 0; i < int.Parse(this.comboBoxNumeroCañones.Text); i++)
                {
                    this._system.addCannon("Cannon" + i.ToString());
                }
            }
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
                ThermoVision.Models.Sistema s =  Helpers.deserializeSistema(this.openFileDialog1.FileName);

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
