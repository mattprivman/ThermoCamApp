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
        public ThermoVision.Models.ZonaVaciado zona;

        public CheckBox buttonMode;
        public Button[] buttonsRejillas;

        public delegate void trampillaControlBoolParameterCallback(int CannonNumber, bool arg);
        public delegate void trampillaControlParameterCallback(int CannonNumber);
        public delegate bool trampillaControlBoolCallback(int CannonNumber);
        public delegate void cambiarTrampillaEstadoCallback(ThermoVision.Models.ZonaVaciado z, int subzona, int col, bool activar);

        public trampillaControlBoolParameterCallback buttonModeAction;
        public cambiarTrampillaEstadoCallback        cambiarTrampillaEstadoAction;

        public RejillasControl(Form f,
            int posicion,
            ThermoVision.Models.ZonaVaciado zona,
            Point p,
            int zonaWidth,
            int totalWidth,
            trampillaControlBoolParameterCallback buttonModeAction,
            cambiarTrampillaEstadoCallback cambiarTrampillaEstadoAction)
        {
            this.Posicion = posicion;
            this.zona     = zona;

            this.buttonModeAction = buttonModeAction;
            this.cambiarTrampillaEstadoAction = cambiarTrampillaEstadoAction;

            this.buttonMode = new CheckBox();
            //
            // buttonMode
            //
            this.buttonMode.Appearance  = Appearance.Button;
            this.buttonMode.Location    = new System.Drawing.Point(p.X, p.Y + 40);
            this.buttonMode.Name        = "buttonMode";
            this.buttonMode.Size        = new System.Drawing.Size(100, 30);
            this.buttonMode.TabIndex    = 5;
            this.buttonMode.Text        = "Automatico";
            this.buttonMode.TextAlign   = ContentAlignment.MiddleCenter;
            this.buttonMode.UseVisualStyleBackColor = true;
            this.buttonMode.CheckedChanged += buttonMode_CheckedChanged;
            //this.buttonMode.Checked = this.CheckCannonMode(this.posicion);

            f.Controls.Add(this.buttonMode);

            //
            // buttonsRejillas
            //
            // Calcular numero de rejillas
            int contador = 0;
            for (int i = 0; i < this.zona.Children.Count; i++)
            {
                contador += this.zona.Children[i].Columnas;
            }

            this.buttonsRejillas = new Button[contador];

            for (int i = 0; i < this.buttonsRejillas.Length; i++)
            {                
                // 
                // buttonDown
                // 
                this.buttonsRejillas[i] = new Button();
                this.buttonsRejillas[i].Location = new System.Drawing.Point((int)((float)p.X + (float)calcularXCoordinate(i) * (float)((float)totalWidth / (float)zonaWidth)) - 25, p.Y);
                this.buttonsRejillas[i].Name = "buttonDown";
                this.buttonsRejillas[i].Size = new System.Drawing.Size(50, 30);
                this.buttonsRejillas[i].TabIndex = i;
                this.buttonsRejillas[i].Text = "▲";
                this.buttonsRejillas[i].UseVisualStyleBackColor = true;
                this.buttonsRejillas[i].MouseDown += RejillasControl_MouseDown;
                this.buttonsRejillas[i].MouseUp += RejillasControl_MouseUp;

                f.Controls.Add(this.buttonsRejillas[i]);
            }
            changeButtonsState(false);
        }

        void RejillasControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (sender is Button)
            {
                Button b = (sender as Button);

                int index = b.TabIndex;
                int i = 0;
                bool found = false;

                while (!found)
                {
                    if (index / zona.Children[i].Columnas == 0)
                    {
                        index = index % zona.Children[i].Columnas;
                        found = true;
                    }
                    else
                    {
                        index -= zona.Children[i].Columnas;

                        if (i == zona.Children.Count - 1)
                            return;
                    }

                    i++;
                }

                int columna = index;
                int subzona = i;

                this.cambiarTrampillaEstadoAction(this.zona, subzona, index, false);
            }
        }

        void RejillasControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Button)
            {
                Button b = (sender as Button);

                int index = b.TabIndex;
                int i = 0;
                bool found = false;

                while (!found)
                {
                    if (index / zona.Children[i].Columnas == 0)
                    {
                        index = index % zona.Children[i].Columnas;
                        found = true;
                    }
                    else
                    {
                        index -= zona.Children[i].Columnas;

                        if (i == zona.Children.Count - 1)
                            return;
                    }

                    i++;
                }

                int columna = index;
                int subzona = i;

                this.cambiarTrampillaEstadoAction(this.zona, subzona, index, true);
            }
        }

        float calcularXCoordinate(int pos)
        {
            int columnasRestates = pos;
            float ancho = 0;

            for (int i = 0; i < this.zona.Children.Count; i++)
            {
                if (columnasRestates < zona.Children[i].Columnas)
                {
                    ancho += columnasRestates * (zona.Children[i].Fin.X - zona.Children[i].Inicio.X) / zona.Children[i].Columnas + ((zona.Children[i].Fin.X - zona.Children[i].Inicio.X) / zona.Children[i].Columnas) / 2;
                    return ancho;
                }
                else
                {
                    columnasRestates = columnasRestates - zona.Children[i].Columnas;
                    ancho           += zona.Children[i].Fin.X - zona.Children[i].Inicio.X;
                }
            }
            return -100;
        }

        public void trampillaModeChanged(object value)
        {
            if (value is bool)
            {
                this.buttonMode.CheckedChanged -= buttonMode_CheckedChanged;

                bool state = (bool)value;

                this.buttonMode.Checked = state;
                changeButtonsState(state);

                if (state)
                    this.zona.ChangeState(ThermoVision.Models.Zona.States.Manual);
                else
                    this.zona.ChangeState(ThermoVision.Models.Zona.States.Esperando);

                this.buttonMode.CheckedChanged += buttonMode_CheckedChanged;
            }
        }

        void buttonMode_CheckedChanged(object sender, EventArgs e)
        {
            if(this.buttonModeAction != null)
                this.buttonModeAction(this.Posicion, this.buttonMode.Checked);
        }

        void changeButtonsState(bool state)
        {
            this.buttonMode.Text = state ? "Manual" : "Automatico";

            foreach (Button b in this.buttonsRejillas)
            {
                b.Visible = state;
            }
        }
    }
}
