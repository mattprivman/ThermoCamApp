using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThermoVision.Models;

namespace WindowsFormsApplication4.Asistente.Cannons
{
    public partial class CannonSelector : Form
    {
        List<Cannon> Cannons;

        public Cannon selectedCannon        
        {
            get
            {
                if (this.listBox1.SelectedIndex != .1)
                {
                    if (this.Cannons.Exists(x => x.Name == this.listBox1.SelectedItem))
                    {
                        return this.Cannons.Where(x => x.Name == this.listBox1.SelectedItem).First();
                    }
                }
                return null;
            }
        }

        public CannonSelector(List<Cannon> l)
        {
            this.Cannons = l;

            InitializeComponent();
            actualizarListBoxConCannons();

            this.listBox1.DoubleClick += listBox1_DoubleClick;
        }

        void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex != -1)
            {
                this.Close();
            }
        }

        private void actualizarListBoxConCannons()
        {
            this.listBox1.BeginUpdate();
            foreach (Cannon c in this.Cannons)
            {
                this.listBox1.Items.Add(c.Name);
            }
            this.listBox1.EndUpdate();
            this.listBox1.Refresh();
        }
    }
}
