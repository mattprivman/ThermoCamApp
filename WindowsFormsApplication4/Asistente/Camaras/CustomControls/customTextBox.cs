using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThermoCamApp.Asistente.Camaras.CustomControls
{
    public partial class customTextBox : UserControl
    {
        public int Id { get; set; }

        public event EventHandler textChanged;

        public customTextBox()
        {
            InitializeComponent();

            this.textBox.TextChanged += textBox_TextChanged;
        }

        void textBox_TextChanged(object sender, EventArgs e)
        {
            if (textChanged != null)
            {
                textChanged(this, null);
            }
        }
    }
}
