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

namespace ThermoCamApp.Forms
{
    public partial class VisualizacionConfig : Form
    {
        Sistema _system;

        public VisualizacionConfig(Sistema _system)
        {
            if (_system == null)
                return;

            this._system = _system;

            InitializeComponent();

            this.comboBoxEscala.Items.Add("Grises");
            this.comboBoxEscala.Items.Add("Rainbow");

            if (_system.ThermoCams.Count > 0)
            {
                this.checkBoxRejillaApagado.Checked = this._system.ThermoCams[0].RejillaApagado;
                this.checkBoxRejillaVaciado.Checked = this._system.ThermoCams[0].RejillaVaciado;
                this.checkBoxHornos.Checked         = this._system.ThermoCams[0].RejillaHornos;

                if (this._system.ThermoCams[0].EscalaGrises == true)
                {
                    this.comboBoxEscala.SelectedIndex = 0;
                }
                else
                {
                    this.comboBoxEscala.SelectedIndex = 1;
                }
            }
        }

        private void checkBoxRejillaApagado_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ThermoCam t in this._system.ThermoCams)
            {
                t.RejillaApagado = this.checkBoxRejillaApagado.Checked;
            }
        }

        private void checkBoxRejillaVaciado_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ThermoCam t in this._system.ThermoCams)
            {
                t.RejillaVaciado = this.checkBoxRejillaVaciado.Checked;
            }
        }

        private void checkBoxHornos_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ThermoCam t in this._system.ThermoCams)
            {
                t.RejillaHornos = this.checkBoxHornos.Checked;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBoxEscala_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEscala.Text == "Grises")
            {
                foreach (ThermoCam t in this._system.ThermoCams)
                {
                    t.EscalaGrises = true;
                }
            }
            if (this.comboBoxEscala.Text == "Rainbow")
            {
                foreach (ThermoCam t in this._system.ThermoCams)
                {
                    t.EscalaGrises = false;
                }
            }
        }
    }
}
