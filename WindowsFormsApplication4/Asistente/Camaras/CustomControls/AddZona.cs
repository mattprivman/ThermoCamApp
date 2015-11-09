using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThermoCamApp.Asistente.Camaras.CustomControls
{
    public partial class AddZona : Form
    {
        public string zonaName
        {
            get;
            set;
        }

        public AddZona()
        {
            InitializeComponent();
        }

        private void textBoxNameZona_TextChanged(object sender, EventArgs e)
        {
            this.zonaName = textBoxNameZona.Text;
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            zonaName = "";
            this.Close();
        }
    }
}
