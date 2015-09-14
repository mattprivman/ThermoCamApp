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
        const int nZonas = 20;

        public int NumeroCamaras                       // -rw    
        { 
            get; 
            set; 
        }
        public int NumeroZonas                         // -rw    
        {
            get;
            set;
        }

        public CameraNumberSelection()                           
        {
            InitializeComponent();

            Salir = true;
            this.comboBoxNumberCameras.DataSource = new List<int>()
            {
                1,
                2,
                3
            };

            List<int> zonas = new List<int>();

            for(int i = 1; i <= nZonas; i++)
            {
                zonas.Add(i);
            }

            this.comboBoxNumeroZonas.DataSource = zonas;
        }

        private void comboBoxNumberCameras_SelectedIndexChanged(object sender, EventArgs e) 
        {
            this.NumeroCamaras = int.Parse(this.comboBoxNumberCameras.Text.ToString());
        }
        private void comboBoxNumeroZonas_SelectedIndexChanged(object sender, EventArgs e)   
        {
            this.NumeroZonas = int.Parse(this.comboBoxNumeroZonas.Text.ToString());
        }

        //BOTONES CONTROL FLUJO DEL PROGRAMA
        private void buttonNext_Click(object sender, EventArgs e)
        {
            this.Salir = false;
            this.Atras = false;

            this.Close();
        }
        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Salir = false;
            this.Atras = true;

            this.Close();
        }
    }
}
