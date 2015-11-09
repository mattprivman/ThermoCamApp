using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ThermoCamApp.Controls
{
    class RejillasControl
    {
        public int Posicion;
        public ThermoVision.Models.Zona zona;

        public CheckBox buttonMode;

        public RejillasControl(Form f,
            int posicion,
            ThermoVision.Models.Zona zona,
            Point p)
        {

            this.buttonMode = new CheckBox();
            //
            // buttonMode
            //
            this.buttonMode.Appearance = Appearance.Button;
            this.buttonMode.Location = new System.Drawing.Point(p.X - 100 - 10 - 100, p.Y);
            this.buttonMode.Name = "buttonMode";
            this.buttonMode.Size = new System.Drawing.Size(100, 30);
            this.buttonMode.TabIndex = 5;
            this.buttonMode.Text = "Automatico";
            this.buttonMode.TextAlign = ContentAlignment.MiddleCenter;
            this.buttonMode.UseVisualStyleBackColor = true;
            //this.buttonMode.CheckedChanged += buttonMode_CheckedChanged;
            //this.buttonMode.Checked = this.CheckCannonMode(this.posicion);

            f.Controls.Add(this.buttonMode);
        }
    }
}
