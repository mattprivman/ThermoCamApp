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
        }

        void comboBoxAppType_TextChanged(object sender, EventArgs e)
        {
            //Cambiar modo de funcionamiento de la aplicación
            Helpers.changeAppStringSetting("Mode", this.comboBoxAppType.Text);
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            this.Salir = false;
            this.Close();
        }
    }
}
