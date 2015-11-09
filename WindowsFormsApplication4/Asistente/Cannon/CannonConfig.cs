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

namespace ThermoCamApp.Asistente.Cannon
{
    public partial class CannonConfig : flowControl
    {
        List<ThermoCam>         thermoCams;
        List<CamCannonControl>  camCannonControls;

        SubZona selectedSubZona;
        int X;
        int Y;

        private System.Windows.Forms.TextBox textBoxXCoordinate;
        private System.Windows.Forms.Label labelXCoordinate;

        private System.Windows.Forms.TextBox textBoxYCoordinate;
        private System.Windows.Forms.Label labelYCoordinate;

        private System.Windows.Forms.Button buttonOPCRead;
        private System.Windows.Forms.Button buttonNext;

        public CannonConfig(Zona z)                                 
        {
            InitializeComponent();

            this.thermoCams = new List<ThermoCam>();
            this.camCannonControls = new List<CamCannonControl>();

            foreach (SubZona s in z.Children)
            {
                if(!this.thermoCams.Contains(s.ThermoParent))
                    this.thermoCams.Add(s.ThermoParent);
            }

            this.SuspendLayout();

            this.Width = 640 * this.thermoCams.Count;
            this.Height = 480 + 200;

            this.textBoxXCoordinate = new System.Windows.Forms.TextBox();
            this.labelXCoordinate = new System.Windows.Forms.Label();

            this.labelYCoordinate = new System.Windows.Forms.Label();
            this.textBoxYCoordinate = new System.Windows.Forms.TextBox();

            this.buttonOPCRead = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();

            // 
            // labelX
            // 
            this.labelXCoordinate.AutoSize = true;
            this.labelXCoordinate.Location = new System.Drawing.Point(10, 500);
            this.labelXCoordinate.Name = "label1";
            this.labelXCoordinate.Size = new System.Drawing.Size(15, 13);
            this.labelXCoordinate.TabIndex = 0;
            this.labelXCoordinate.Text = "X";
            // 
            // textBoxX
            // 
            this.textBoxXCoordinate.Location = new System.Drawing.Point(35, 500);
            this.textBoxXCoordinate.Name = "textBoxX";
            this.textBoxXCoordinate.Size = new System.Drawing.Size(100, 20);
            this.textBoxXCoordinate.TabIndex = 1;
            this.textBoxXCoordinate.TextChanged += textBoxXCoordinate_TextChanged;
            // 
            // labelY
            // 
            this.labelYCoordinate.AutoSize = true;
            this.labelYCoordinate.Location = new System.Drawing.Point(135 + 10, 500);
            this.labelYCoordinate.Name = "label1";
            this.labelYCoordinate.Size = new System.Drawing.Size(15, 13);
            this.labelYCoordinate.TabIndex = 2;
            this.labelYCoordinate.Text = "Y";
            // 
            // textBoxY
            // 
            this.textBoxYCoordinate.Location = new System.Drawing.Point(155 + 10, 500);
            this.textBoxYCoordinate.Name = "textBoxY";
            this.textBoxYCoordinate.Size = new System.Drawing.Size(100, 20);
            this.textBoxYCoordinate.TabIndex = 3;
            this.textBoxYCoordinate.TextChanged += textBoxYCoordinate_TextChanged;
            // 
            // buttonOPCRead
            // 
            this.buttonOPCRead.Location = new System.Drawing.Point(this.Width - 220, 500);
            this.buttonOPCRead.Name = "buttonOPCRead";
            this.buttonOPCRead.Size = new System.Drawing.Size(102, 23);
            this.buttonOPCRead.TabIndex = 4;
            this.buttonOPCRead.Text = "Leer coordenadas";
            this.buttonOPCRead.UseVisualStyleBackColor = true;
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(this.Width - 110 , 500);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(92, 23);
            this.buttonNext.TabIndex = 5;
            this.buttonNext.Text = "Siguiente >>";
            this.buttonNext.UseVisualStyleBackColor = true;

            this.Controls.Add(this.labelXCoordinate);
            this.Controls.Add(this.textBoxXCoordinate);

            this.Controls.Add(this.labelYCoordinate);
            this.Controls.Add(this.textBoxYCoordinate);

            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonOPCRead);

            for(int i = 0; i < this.thermoCams.Count; i++)
            {
                CamCannonControl c = new CamCannonControl();
                // 
                // camCannonControl
                // 
                c.Location = new System.Drawing.Point(i * 640, 0);
                c.Name = "camCannonControl1";
                c.Size = new System.Drawing.Size(640, 480);
                c.TabIndex = 100 + i;

                c.Initialize(this, this.thermoCams[i]);
                this.thermoCams[i].RampMode = false;

                c.ThermoCam_CoordinateSelected += c_ThermoCam_CoordinateSelected;

                this.Controls.Add(c);
                this.camCannonControls.Add(c);
            }

            this.ResumeLayout();
        }

        
        void textBoxXCoordinate_TextChanged(object sender, EventArgs e)
        {
            int valueX = 0;
            int valueY = 0;

            if(int.TryParse(this.textBoxXCoordinate.Text, out valueX))
            {
                if(int.TryParse(this.textBoxYCoordinate.Text, out valueY))
                {
                    this.selectedSubZona.tempMatrix[this.X, this.Y].CannonCoordinate = new Point(valueX, valueY);
                }//if
            }//if
        }

        void textBoxYCoordinate_TextChanged(object sender, EventArgs e)
        {
            int valueX = 0;
            int valueY = 0;

            if (int.TryParse(this.textBoxXCoordinate.Text, out valueX))
            {
                if (int.TryParse(this.textBoxYCoordinate.Text, out valueY))
                {
                    this.selectedSubZona.tempMatrix[this.X, this.Y].CannonCoordinate = new Point(valueX, valueY);
                }//if
            }//if
        }

        void c_ThermoCam_CoordinateSelected(object sender, CamCannonControl.CoordinateSelectedEventArgs e)
        {
            //Deseleccionar todas las coordenadas
            foreach (ThermoCam t in this.thermoCams)
                t.unSelectSubZonaCoordinates();
            e.t.selectSubZonaCoordinate(e.s, e.X, e.Y);

            this.selectedSubZona = e.s;
            this.X = e.X;
            this.Y = e.Y;

            updateLabel(this.textBoxXCoordinate, e.s.tempMatrix[e.X, e.Y].CannonCoordinate.X.ToString());
            updateLabel(this.textBoxYCoordinate, e.s.tempMatrix[e.X, e.Y].CannonCoordinate.Y.ToString());
        }

        #region "Actualizar controles"

        delegate void updateLabelCallback(TextBox l, string text);
        void updateLabel(TextBox l, string text)                      
        {
            if (l.InvokeRequired)
            {
                l.Invoke(new updateLabelCallback(updateLabel), l, text);
            }
            else
            {
                l.Text = text;
            }
        }

        #endregion
    }
}
