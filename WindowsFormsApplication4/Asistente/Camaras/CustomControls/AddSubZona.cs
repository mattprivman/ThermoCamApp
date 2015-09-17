using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication4.Asistente.Camaras.CustomControls
{
    public partial class AddSubZona : Form
    {
        public string nameSubZona       
        {
            get;
            set;
        }

        public AddSubZona()             
        {
            InitializeComponent();
        }

        private void textBoxNameZona_TextChanged(object sender, EventArgs e)    
        {
            this.nameSubZona = textBoxNameZona.Text;
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            this.nameSubZona = "";
            this.Close();
        }
    }
}
