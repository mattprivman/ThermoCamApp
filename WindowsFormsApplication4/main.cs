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
        public Sistema _system              
        {
            get;
            set;
        }
        private List<CamControl> camaras = new List<CamControl>();

        private ThermoVision.CustomControls.NumericTextBox numericTextBoxTempApagadoLimite;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private ThermoVision.CustomControls.NumericTextBox numericTextBoxTempLimiteVaciado;
        private System.Windows.Forms.Label[] EstadosLabels;
        private System.Windows.Forms.Button buttonAsistente;

        PictureBox pictureBoxRampa;

        public main(Sistema _system)
        {
            this._system = _system;

            InitializeComponent();

            int counter = 0;

            this.SuspendLayout();

            #region "Cámaras"
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
            #endregion

            // 
            // buttonAsistente
            // 
            this.buttonAsistente.Location = new System.Drawing.Point(this.Width - 95  - 50, this.Height - 77 - 50);
            this.buttonAsistente.Name = "button1";
            this.buttonAsistente.Size = new System.Drawing.Size(95, 77);
            this.buttonAsistente.TabIndex = 0;
            this.buttonAsistente.Text = "Asistente";
            this.buttonAsistente.UseVisualStyleBackColor = true;
            this.buttonAsistente.Click += buttonAsistente_Click;

            #region "Rampas"
            if (this._system.Mode == "Rampas")
            {
                this.numericTextBoxTempApagadoLimite = new NumericTextBox();
                this.numericTextBoxTempLimiteVaciado = new NumericTextBox();
                this.label1 = new Label();
                this.label2 = new Label();

                this.numericTextBoxTempApagadoLimite.maxVal = 2000;
                this.numericTextBoxTempApagadoLimite.minVal = 0;
                this.numericTextBoxTempLimiteVaciado.maxVal = 2000;
                this.numericTextBoxTempLimiteVaciado.minVal = 0;

                this.numericTextBoxTempApagadoLimite.Texto = this._system.estados.tempLimiteHayQueEnfriar.ToString(); ;
                this.numericTextBoxTempLimiteVaciado.Texto = this._system.estados.tempLimiteHayMaterial.ToString();

                this.pictureBoxRampa = new PictureBox();
                ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRampa)).BeginInit();
                // 
                // pictureBox1
                // 
                this.pictureBoxRampa.Location = new System.Drawing.Point(10, 500);
                this.pictureBoxRampa.Name = "pictureBox1";
                this.pictureBoxRampa.Size = new System.Drawing.Size(this.Width - 40, 262);
                this.pictureBoxRampa.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                this.pictureBoxRampa.TabIndex = 0;
                this.pictureBoxRampa.TabStop = false;
                this.pictureBoxRampa.BorderStyle = BorderStyle.FixedSingle;
                //this.pictureBoxRampa.B

                this.Controls.Add(this.pictureBoxRampa);

                ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRampa)).EndInit();

                this.EstadosLabels = new Label[this._system.Zonas.Count];

                for (int i = 0; i < this._system.Zonas.Count; i++)
                {
                    this.EstadosLabels[i] = new Label();
                    // 
                    // label3
                    //
                    int xPos = i * (this.Width / this._system.Zonas.Count) + (this.Width / this._system.Zonas.Count) / 2;

                    this.EstadosLabels[i].AutoSize  = true;
                    this.EstadosLabels[i].Location  = new System.Drawing.Point(xPos, 262 + 500 + 15);
                    this.EstadosLabels[i].Name      = "label" + i.ToString();
                    this.EstadosLabels[i].Size      = new System.Drawing.Size(40, 13);
                    this.EstadosLabels[i].TabIndex  = 4;
                    this.EstadosLabels[i].Text      = "Estado";
                    this.EstadosLabels[i].Font = new Font(
                                                   new FontFamily("Microsoft Sans Serif"),
                                                   16,
                                                   FontStyle.Regular,
                                                   GraphicsUnit.Pixel);
                    this.EstadosLabels[i].Padding = new Padding() { All = 10 };
                    this.EstadosLabels[i].BorderStyle = BorderStyle.FixedSingle;
                    

                    this._system.Zonas[i].Posicion  = i;
                    this._system.Zonas[i].stateChanged += main_stateChanged;
                }
                // 
                // numericTextBoxTempApagadoLimite
                // 
                this.numericTextBoxTempApagadoLimite.Location = new System.Drawing.Point(this.camaras[0].Width * counter + 148, 85);
                this.numericTextBoxTempApagadoLimite.Name = "numericTextBoxTempApagadoLimite";
                this.numericTextBoxTempApagadoLimite.Size = new System.Drawing.Size(102, 20);
                this.numericTextBoxTempApagadoLimite.TabIndex = 0;
                this.numericTextBoxTempApagadoLimite.textoCambiado += numericTextBoxTempApagadoLimite_textoCambiado;
                // 
                // label1
                // 
                this.label1.AutoSize = true;
                this.label1.Location = new System.Drawing.Point(this.camaras[0].Width * counter + 10, 85);
                this.label1.Name = "label1";
                this.label1.Size = new System.Drawing.Size(138, 13);
                this.label1.TabIndex = 1;
                this.label1.Text = "Temperatura limite apagado";
                // 
                // label2
                // 
                this.label2.AutoSize = true;
                this.label2.Location = new System.Drawing.Point(this.camaras[0].Width * counter + 10, 148);
                this.label2.Name = "label2";
                this.label2.Size = new System.Drawing.Size(134, 13);
                this.label2.TabIndex = 3;
                this.label2.Text = "Temperatura limite vaciado";
                // 
                // numericTextBoxTempLimiteVaciado
                // 
                this.numericTextBoxTempLimiteVaciado.Location = new System.Drawing.Point(this.camaras[0].Width * counter + 148, 148);
                this.numericTextBoxTempLimiteVaciado.Name = "numericTextBoxTempLimiteVaciado";
                this.numericTextBoxTempLimiteVaciado.Size = new System.Drawing.Size(102, 20);
                this.numericTextBoxTempLimiteVaciado.textoCambiado += numericTextBoxTempLimiteVaciado_textoCambiado;

                for(int i = 0; i < this.EstadosLabels.Length; i++)
                    this.Controls.Add(this.EstadosLabels[i]);
                this.Controls.Add(this.label2);
                this.Controls.Add(this.numericTextBoxTempLimiteVaciado);
                this.Controls.Add(this.label1);
                this.Controls.Add(this.numericTextBoxTempApagadoLimite);

                this._system.estados.ThermoCamImgCuadradosGenerated += estados_ThermoCamImgCuadradosGenerated;
            }
            #endregion

            this.ResumeLayout();

            this.FormClosing    += main_FormClosing;

            this._system.conectarClienteOPC();
            this._system.modoConfiguracion = false;


            foreach(Zona zVaciado in this._system.ZonasVaciado)
            {
                foreach (SubZona s in zVaciado.Children)
                {
                    s.Parent.zonasContenidas.Clear();

                    //Buscar zonas de apagado superpuestas 
                    foreach (Zona zApagado in this._system.Zonas)
                    {
                        foreach (SubZona sApagado in zApagado.Children)
                        {
                            if (sApagado.ThermoParent != null && sApagado.ThermoParent.Equals(s.ThermoParent))
                            {
                                int sWidth = s.Fin.X - s.Inicio.X;
                                int sHeight = s.Fin.Y - s.Inicio.Y;

                                //Esquina superior izquierda
                                if (s.Inicio.X > sApagado.Inicio.X && s.Inicio.X < sApagado.Fin.X &&
                                    s.Inicio.Y > sApagado.Inicio.Y && s.Inicio.Y < sApagado.Fin.Y)
                                {
                                    //PERTENECE
                                    if (!s.Parent.zonasContenidas.Contains(zApagado))
                                    {
                                        s.Parent.zonasContenidas.Add(zApagado);
                                    }
                                }
                                //Esquina superior derecha
                                if (s.Inicio.X + sWidth > sApagado.Inicio.X && s.Inicio.X + sWidth < sApagado.Fin.X &&
                                    s.Inicio.Y + sHeight > sApagado.Inicio.Y && s.Inicio.Y < sApagado.Fin.Y)
                                {
                                    //PERTENECE
                                    if (!s.Parent.zonasContenidas.Contains(zApagado))
                                    {
                                        s.Parent.zonasContenidas.Add(zApagado);
                                    }
                                }
                                //Esquina inferior izquierda
                                if (s.Inicio.X > sApagado.Inicio.X && s.Inicio.X < sApagado.Fin.X &&
                                    s.Inicio.Y + sHeight > sApagado.Inicio.Y && s.Inicio.Y + sHeight < sApagado.Fin.Y)
                                {
                                    //PERTENECE
                                    if (!s.Parent.zonasContenidas.Contains(zApagado))
                                    {
                                        s.Parent.zonasContenidas.Add(zApagado);
                                    }
                                }
                                //////Esquina inferior derecha
                                if (s.Inicio.X + sWidth > sApagado.Inicio.X && s.Inicio.X + sWidth < sApagado.Fin.X &&
                                    s.Inicio.Y + sHeight > sApagado.Inicio.Y && s.Inicio.Y + sHeight < sApagado.Fin.Y)
                                {
                                    //PERTENECE
                                    if (!s.Parent.zonasContenidas.Contains(zApagado))
                                    {
                                        s.Parent.zonasContenidas.Add(zApagado);
                                    }
                                }
                                if (s.Inicio.X <= sApagado.Inicio.X && s.Fin.X >= sApagado.Fin.X ||
                                s.Inicio.Y <= sApagado.Inicio.Y && s.Fin.Y >= sApagado.Fin.Y)
                                {
                                    //PERTENECE
                                    if (!s.Parent.zonasContenidas.Contains(zApagado))
                                    {
                                        s.Parent.zonasContenidas.Add(zApagado);
                                    }
                                }
                            }
                        }//foreach
                    }//foreach
                }
            }//foreach
        }

        void buttonAsistente_Click(object sender, EventArgs e)
        {
            this.Salir = false;
            this.Atras = true;
            this.Close();
        }

        void main_stateChanged(object sender, Zona.States state)
        {
            updateLabelTextProperty(EstadosLabels[((Zona) sender).Posicion], state.ToString());

            switch (state)
            {
                case Zona.States.Vacio:
                    updateLabelBackColor(EstadosLabels[((Zona)sender).Posicion], Color.Transparent);
                    break;
                case Zona.States.Lleno:
                    updateLabelBackColor(EstadosLabels[((Zona)sender).Posicion], Color.Green);
                    break;
                case Zona.States.Enfriando:
                    updateLabelBackColor(EstadosLabels[((Zona)sender).Posicion], Color.Orange);
                    break;
                case Zona.States.Vaciando:
                    updateLabelBackColor(EstadosLabels[((Zona)sender).Posicion], Color.Blue);
                    break;
            }
        }

        void numericTextBoxTempApagadoLimite_textoCambiado(object sender, EventArgs e)
        {
            this._system.estados.tempLimiteHayQueEnfriar = int.Parse(this.numericTextBoxTempApagadoLimite.Texto);
        }
        void numericTextBoxTempLimiteVaciado_textoCambiado(object sender, EventArgs e)
        {
            this._system.estados.tempLimiteHayMaterial = int.Parse(this.numericTextBoxTempLimiteVaciado.Texto);
        }


        void estados_ThermoCamImgCuadradosGenerated(object sender, ThermoVision.Tipos.ThemoCamImgCuadradosArgs e) 
        {
            updatePictureBox(this.pictureBoxRampa, ref e.Imagen);
        }

        void main_FormClosing(object sender, FormClosingEventArgs e)                  
        {
            foreach (CamControl c in camaras)
            {
                c.Desconectar();
            }
        }

        private delegate void updatePictureBoxCallback(PictureBox p, ref System.Drawing.Bitmap bmp);
        private void updatePictureBox(PictureBox p, ref System.Drawing.Bitmap bmp)    
        {
            if (p.IsDisposed == false)
            {
                if (p.InvokeRequired)
                {
                    try
                    {
                        p.Invoke(new updatePictureBoxCallback(updatePictureBox), p, bmp);
                    }
                    catch (Exception ex) 
                    {
                        ex.ToString();
                    }
                }
                else
                {
                    p.Image = bmp;
                    p.Refresh();
                }
            }
        }

        private delegate void updateLabelTexPropertyDelegate(Label l, string text);
        private void updateLabelTextProperty(Label l, string text)                    
        {
            if (l.InvokeRequired)
            {
                l.Invoke(new updateLabelTexPropertyDelegate(updateLabelTextProperty), l, text);
            }
            else
            {
                l.Text = text;
            }
        }

        private delegate void updateLabelBackColorDelegate(Label l, Color c);
        private void updateLabelBackColor(Label l, Color c)
        {
            if (l.InvokeRequired)
            {
                l.Invoke(new updateLabelBackColorDelegate(updateLabelBackColor), l, c);
            }
            else
            {
                l.BackColor = c;
            }
        }
    }
}
