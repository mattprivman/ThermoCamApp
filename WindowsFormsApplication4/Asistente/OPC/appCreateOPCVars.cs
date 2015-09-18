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

namespace WindowsFormsApplication4.Asistente.OPC
{
    public partial class appCreateOPCVars : flowControl
    {
        Sistema _system;
        Label[] etiquetas;
        Button[] botones;

        public appCreateOPCVars(Sistema _system)    
        {
            this._system = _system;

            etiquetas = new Label[this._system.Zonas.Count];
            botones   = new Button[this._system.Zonas.Count];

            this.Salir = true;

            InitializeComponent();
            this.textBox1.Text     = "Genere los archivos *.csv de cada zona seleccionada  y a continuación agregelos al servidor OPC" +
                " que se encargará de la comunicación con el automáta. Recuerde que debera agregar dichas variables tambien en el plc.";
            this.pictureBox1.Image = System.Drawing.Bitmap.FromFile("Resources/FLIR_A320.jpg");

            generarComponentesDinamicos();
        }

        private void generarComponentesDinamicos()
        {
            this.SuspendLayout();

            for (int i = 0; i < this._system.Zonas.Count; i++)
            {
                this.botones[i] = new Button();

                this.botones[i].Location = new System.Drawing.Point(217, 125 + i * 38);
                this.botones[i].Name = "buttonCamZona1";
                this.botones[i].Size = new System.Drawing.Size(75, 23);
                this.botones[i].TabIndex = 100 + i;
                this.botones[i].Text = "Generar";
                this.botones[i].UseVisualStyleBackColor = true;
                this.botones[i].Click += appCreateOPCVars_Click;

                this.groupBox1.Controls.Add(this.botones[i]);

                this.etiquetas[i] = new Label();

                this.etiquetas[i].AutoSize = true;
                this.etiquetas[i].Location = new System.Drawing.Point(20, 130 + i * 38);
                this.etiquetas[i].Name = "label1";
                this.etiquetas[i].Size = new System.Drawing.Size(141, 13);
                this.etiquetas[i].TabIndex = 1;
                this.etiquetas[i].Text = "Generar \"*.csv\" para zona " + this._system.Zonas[i].Nombre + ".";

                this.groupBox1.Controls.Add(this.etiquetas[i]);
            }

            this.groupBox1.Update();

            this.ResumeLayout(true);
        }

        void appCreateOPCVars_Click(object sender, EventArgs e)
        {
            this.saveFileDialog1.Filter = "Archivo de valores separados por comas (*.csv)|*.csv";
            this.saveFileDialog1.ShowDialog();
            this.saveFileDialog1.AddExtension = true;
            string ruta = this.saveFileDialog1.FileName;

            Zona z = this._system.Zonas[((Button) sender).TabIndex - 100];

            if (z != null)
            {
                using (System.IO.StreamWriter w = new System.IO.StreamWriter(ruta, false))
                {
                    w.WriteLine("Tag Name,Address,Data Type,Respect Data Type,Client Access,Scan Rate,Scaling,Raw Low,Raw High,Scaled Low,Scaled High,Scaled Data Type,Clamp Low,Clamp High,Eng Units,Description,Negate Value");

                    int index = 0;

                    foreach (SubZona s in z.Children)
                    {
                        for (int x = 0; x < s.Columnas; x++)
                        {
                            for (int y = 0; y < s.Filas; y++)
                            {
                                w.WriteLine("\"" + s.Nombre + "[" + x + "," + y + "]" + "\",\"DB3.DBW" + index  + "\",Word,1,R/W,100,,,,,,,,,,\"\",");
                                index++;
                            }
                        }
                    }

                    w.Close();
                    w.Dispose();
                }
            }
        }

        public void generarCSV()
        {
            using (System.IO.StreamWriter w = new System.IO.StreamWriter("File.csv", false))
            {
                w.WriteLine("Tag Name,Address,Data Type,Respect Data Type,Client Access,Scan Rate,Scaling,Raw Low,Raw High,Scaled Low,Scaled High,Scaled Data Type,Clamp Low,Clamp High,Eng Units,Description,Negate Value");

                foreach (Zona s in this._system.Zonas)
                {

                }
            }
        }

        #region "BOTONES FLUJO"
        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Salir = false;
            this.Atras = true;

            this.Close();
        }
        private void buttonNext_Click(object sender, EventArgs e)
        {
            this.Salir = false;
            this.Atras = false;

            this.Close();
        }
        #endregion
    }
}
