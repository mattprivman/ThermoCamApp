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
using ThermoVision.CustomControls;

namespace WindowsFormsApplication4
{
    public partial class main : Asistente.flowControl
    {
        private Sistema _system;
        private List<CamControl> camaras = new List<CamControl>();

        public main(Sistema _system)
        {
            this._system = _system;

            InitializeComponent();

            int counter = 0;

            this.SuspendLayout();

            foreach (ThermoCam t in this._system.ThermoCams)
            {
                // 
                // camControl1
                // 
                CamControl c = new CamControl();

                c.Location = new System.Drawing.Point(c.Width * counter, 0);
                c.Name = "camControl1";
                c.Size = new System.Drawing.Size(c.Width, c.Height);
                c.TabIndex = 100 + counter;

                c.Initialize(this, t);
                camaras.Add(c);
                this.Controls.Add(c);

                counter++;
            }
            this.ResumeLayout();

            this.FormClosing += main_FormClosing;

            this._system.conectarClienteOPC();
        }

        void main_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (CamControl c in camaras)
            {
                c.Desconectar();
            }
        }
    }
}
