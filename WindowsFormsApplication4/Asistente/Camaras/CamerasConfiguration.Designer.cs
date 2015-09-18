using System.Collections.Generic;

namespace WindowsFormsApplication4.Asistente.Camaras
{
    partial class CamerasConfiguration
    {
        const int camarasXstart = 0;
        const int treeViewXstart = 0;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent(int numCamaras)
        {
            this.buttonAddZona      = new System.Windows.Forms.Button();
            this.buttonBorrarZona   = new System.Windows.Forms.Button();
            this.groupBox1          = new System.Windows.Forms.GroupBox();
            this.listBoxZonas       = new System.Windows.Forms.ListBox();
            this.groupBox2          = new System.Windows.Forms.GroupBox();
            this.listBoxSubZonas    = new System.Windows.Forms.ListBox();
            this.buttonBorrarSubZona = new System.Windows.Forms.Button();
            this.buttonAddSubZona   = new System.Windows.Forms.Button();
            this.buttonSubstractCol = new System.Windows.Forms.Button();
            this.buttonAddCol       = new System.Windows.Forms.Button();
            this.numericTextBoxCol  = new ThermoVision.CustomControls.NumericTextBox();
            this.label6             = new System.Windows.Forms.Label();
            this.buttonSubstracFilas = new System.Windows.Forms.Button();
            this.buttonAddFilas     = new System.Windows.Forms.Button();
            this.numericTextBoxFil  = new ThermoVision.CustomControls.NumericTextBox();
            this.label5             = new System.Windows.Forms.Label();
            this.buttonSubstractYFin = new System.Windows.Forms.Button();
            this.buttonAddYFin      = new System.Windows.Forms.Button();
            this.numericTextBoxYFin = new ThermoVision.CustomControls.NumericTextBox();
            this.label4             = new System.Windows.Forms.Label();
            this.buttonSubstractXfin = new System.Windows.Forms.Button();
            this.buttonAddXFin      = new System.Windows.Forms.Button();
            this.numericTextBoxXfin = new ThermoVision.CustomControls.NumericTextBox();
            this.label3             = new System.Windows.Forms.Label();
            this.buttonSubstractYini = new System.Windows.Forms.Button();
            this.buttonAddYini      = new System.Windows.Forms.Button();
            this.numericTextBoxYinit = new ThermoVision.CustomControls.NumericTextBox();
            this.label2             = new System.Windows.Forms.Label();
            this.buttonSubstractXIni = new System.Windows.Forms.Button();
            this.buttonAddXIni      = new System.Windows.Forms.Button();
            this.numericTextBoxXIni = new ThermoVision.CustomControls.NumericTextBox();
            this.label1             = new System.Windows.Forms.Label();
            this.camaras            = new List<ThermoVision.CustomControls.CamConfigControl>();
            this.buttonNext         = new System.Windows.Forms.Button();
            this.buttonBack         = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonAddZona
            // 
            this.buttonAddZona.Location     = new System.Drawing.Point(6, 131);
            this.buttonAddZona.Name         = "buttonAddZona";
            this.buttonAddZona.Size         = new System.Drawing.Size(63, 23);
            this.buttonAddZona.TabIndex     = 0;
            this.buttonAddZona.Text         = "Agregar";
            this.buttonAddZona.UseVisualStyleBackColor = true;
            this.buttonAddZona.Click += new System.EventHandler(this.buttonAddZona_Click);
            // 
            // buttonBorrarZona
            // 
            this.buttonBorrarZona.Location  = new System.Drawing.Point(75, 131);
            this.buttonBorrarZona.Name      = "buttonBorrarZona";
            this.buttonBorrarZona.Size      = new System.Drawing.Size(61, 23);
            this.buttonBorrarZona.TabIndex  = 1;
            this.buttonBorrarZona.Text      = "Borrar";
            this.buttonBorrarZona.UseVisualStyleBackColor = true;
            this.buttonBorrarZona.Click += new System.EventHandler(this.buttonBorrarZona_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBoxZonas);
            this.groupBox1.Controls.Add(this.buttonAddZona);
            this.groupBox1.Controls.Add(this.buttonBorrarZona);
            this.groupBox1.Location = new System.Drawing.Point(12, 706);
            this.groupBox1.Name     = "groupBox1";
            this.groupBox1.Size     = new System.Drawing.Size(156, 161);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop  = false;
            this.groupBox1.Text     = "Zonas";
            // 
            // listBoxZonas
            // 
            this.listBoxZonas.FormattingEnabled = true;
            this.listBoxZonas.Location = new System.Drawing.Point(6, 19);
            this.listBoxZonas.Name      = "listBoxZonas";
            this.listBoxZonas.Size      = new System.Drawing.Size(144, 95);
            this.listBoxZonas.TabIndex  = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBoxSubZonas);
            this.groupBox2.Controls.Add(this.buttonBorrarSubZona);
            this.groupBox2.Controls.Add(this.buttonAddSubZona);
            this.groupBox2.Controls.Add(this.buttonSubstractCol);
            this.groupBox2.Controls.Add(this.buttonAddCol);
            this.groupBox2.Controls.Add(this.numericTextBoxCol);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.buttonSubstracFilas);
            this.groupBox2.Controls.Add(this.buttonAddFilas);
            this.groupBox2.Controls.Add(this.numericTextBoxFil);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.buttonSubstractYFin);
            this.groupBox2.Controls.Add(this.buttonAddYFin);
            this.groupBox2.Controls.Add(this.numericTextBoxYFin);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.buttonSubstractXfin);
            this.groupBox2.Controls.Add(this.buttonAddXFin);
            this.groupBox2.Controls.Add(this.numericTextBoxXfin);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.buttonSubstractYini);
            this.groupBox2.Controls.Add(this.buttonAddYini);
            this.groupBox2.Controls.Add(this.numericTextBoxYinit);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.buttonSubstractXIni);
            this.groupBox2.Controls.Add(this.buttonAddXIni);
            this.groupBox2.Controls.Add(this.numericTextBoxXIni);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(174, 706);
            this.groupBox2.Name     = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(538, 161);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop  = false;
            this.groupBox2.Text     = "Subzonas";
            // 
            // listBoxSubZonas
            // 
            this.listBoxSubZonas.FormattingEnabled = true;
            this.listBoxSubZonas.Location   = new System.Drawing.Point(7, 19);
            this.listBoxSubZonas.Name       = "listBoxSubZonas";
            this.listBoxSubZonas.Size       = new System.Drawing.Size(130, 95);
            this.listBoxSubZonas.TabIndex   = 4;
            // 
            // buttonBorrarSubZona
            // 
            this.buttonBorrarSubZona.Location   = new System.Drawing.Point(76, 131);
            this.buttonBorrarSubZona.Name       = "buttonBorrarSubZona";
            this.buttonBorrarSubZona.Size       = new System.Drawing.Size(61, 23);
            this.buttonBorrarSubZona.TabIndex   = 4;
            this.buttonBorrarSubZona.Text       = "Borrar";
            this.buttonBorrarSubZona.UseVisualStyleBackColor = true;
            this.buttonBorrarSubZona.Click += new System.EventHandler(this.buttonBorrarSubZona_Click);
            // 
            // buttonAddSubZona
            // 
            this.buttonAddSubZona.Location  = new System.Drawing.Point(7, 131);
            this.buttonAddSubZona.Name      = "buttonAddSubZona";
            this.buttonAddSubZona.Size      = new System.Drawing.Size(63, 23);
            this.buttonAddSubZona.TabIndex  = 4;
            this.buttonAddSubZona.Text      = "Agregar";
            this.buttonAddSubZona.UseVisualStyleBackColor = true;
            this.buttonAddSubZona.Click += new System.EventHandler(this.buttonAddSubZona_Click);
            // 
            // buttonSubstractCol
            // 
            this.buttonSubstractCol.Location    = new System.Drawing.Point(488, 116);
            this.buttonSubstractCol.Name        = "buttonSubstractCol";
            this.buttonSubstractCol.Size        = new System.Drawing.Size(42, 23);
            this.buttonSubstractCol.TabIndex    = 25;
            this.buttonSubstractCol.Text        = "-";
            this.buttonSubstractCol.UseVisualStyleBackColor = true;
            // 
            // buttonAddCol
            // 
            this.buttonAddCol.Location  = new System.Drawing.Point(440, 116);
            this.buttonAddCol.Name      = "buttonAddCol";
            this.buttonAddCol.Size      = new System.Drawing.Size(42, 23);
            this.buttonAddCol.TabIndex  = 24;
            this.buttonAddCol.Text      = "+";
            this.buttonAddCol.UseVisualStyleBackColor = true;
            // 
            // numericTextBoxCol
            // 
            this.numericTextBoxCol.Location = new System.Drawing.Point(472, 90);
            this.numericTextBoxCol.Name     = "numericTextBoxCol";
            this.numericTextBoxCol.Size     = new System.Drawing.Size(33, 20);
            this.numericTextBoxCol.TabIndex = 23;
            this.numericTextBoxCol.Texto    = "0";
            // 
            // label6
            // 
            this.label6.AutoSize    = true;
            this.label6.Location    = new System.Drawing.Point(414, 93);
            this.label6.Name        = "label6";
            this.label6.Size        = new System.Drawing.Size(53, 13);
            this.label6.TabIndex    = 22;
            this.label6.Text        = "Columnas";
            // 
            // buttonSubstracFilas
            // 
            this.buttonSubstracFilas.Location   = new System.Drawing.Point(488, 56);
            this.buttonSubstracFilas.Name       = "buttonSubstracFilas";
            this.buttonSubstracFilas.Size       = new System.Drawing.Size(42, 23);
            this.buttonSubstracFilas.TabIndex   = 21;
            this.buttonSubstracFilas.Text       = "-";
            this.buttonSubstracFilas.UseVisualStyleBackColor = true;
            // 
            // buttonAddFilas
            // 
            this.buttonAddFilas.Location    = new System.Drawing.Point(440, 56);
            this.buttonAddFilas.Name        = "buttonAddFilas";
            this.buttonAddFilas.Size        = new System.Drawing.Size(42, 23);
            this.buttonAddFilas.TabIndex    = 20;
            this.buttonAddFilas.Text        = "+";
            this.buttonAddFilas.UseVisualStyleBackColor = true;
            // 
            // numericTextBoxFil
            // 
            this.numericTextBoxFil.Location = new System.Drawing.Point(472, 30);
            this.numericTextBoxFil.Name     = "numericTextBoxFil";
            this.numericTextBoxFil.Size     = new System.Drawing.Size(33, 20);
            this.numericTextBoxFil.TabIndex = 19;
            this.numericTextBoxFil.Texto    = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(439, 33);
            this.label5.Name     = "label5";
            this.label5.Size     = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 18;
            this.label5.Text     = "Filas";
            // 
            // buttonSubstractYFin
            // 
            this.buttonSubstractYFin.Location   = new System.Drawing.Point(343, 116);
            this.buttonSubstractYFin.Name       = "buttonSubstractYFin";
            this.buttonSubstractYFin.Size       = new System.Drawing.Size(42, 23);
            this.buttonSubstractYFin.TabIndex   = 17;
            this.buttonSubstractYFin.Text       = "-";
            this.buttonSubstractYFin.UseVisualStyleBackColor = true;
            // 
            // buttonAddYFin
            // 
            this.buttonAddYFin.Location = new System.Drawing.Point(295, 116);
            this.buttonAddYFin.Name     = "buttonAddYFin";
            this.buttonAddYFin.Size     = new System.Drawing.Size(42, 23);
            this.buttonAddYFin.TabIndex = 16;
            this.buttonAddYFin.Text     = "+";
            this.buttonAddYFin.UseVisualStyleBackColor = true;
            // 
            // numericTextBoxYFin
            // 
            this.numericTextBoxYFin.Location    = new System.Drawing.Point(327, 90);
            this.numericTextBoxYFin.Name        = "numericTextBoxYFin";
            this.numericTextBoxYFin.Size        = new System.Drawing.Size(33, 20);
            this.numericTextBoxYFin.TabIndex    = 15;
            this.numericTextBoxYFin.Texto       = "0";
            // 
            // label4
            // 
            this.label4.AutoSize    = true;
            this.label4.Location    = new System.Drawing.Point(294, 93);
            this.label4.Name        = "label4";
            this.label4.Size        = new System.Drawing.Size(28, 13);
            this.label4.TabIndex    = 14;
            this.label4.Text        = "Y fin";
            // 
            // buttonSubstractXfin
            // 
            this.buttonSubstractXfin.Location   = new System.Drawing.Point(212, 117);
            this.buttonSubstractXfin.Name       = "buttonSubstractXfin";
            this.buttonSubstractXfin.Size       = new System.Drawing.Size(42, 23);
            this.buttonSubstractXfin.TabIndex   = 13;
            this.buttonSubstractXfin.Text       = "-";
            this.buttonSubstractXfin.UseVisualStyleBackColor = true;
            // 
            // buttonAddXFin
            // 
            this.buttonAddXFin.Location = new System.Drawing.Point(164, 117);
            this.buttonAddXFin.Name     = "buttonAddXFin";
            this.buttonAddXFin.Size     = new System.Drawing.Size(42, 23);
            this.buttonAddXFin.TabIndex = 12;
            this.buttonAddXFin.Text     = "+";
            this.buttonAddXFin.UseVisualStyleBackColor = true;
            // 
            // numericTextBoxXfin
            // 
            this.numericTextBoxXfin.Location    = new System.Drawing.Point(196, 91);
            this.numericTextBoxXfin.Name        = "numericTextBoxXfin";
            this.numericTextBoxXfin.Size        = new System.Drawing.Size(33, 20);
            this.numericTextBoxXfin.TabIndex    = 11;
            this.numericTextBoxXfin.Texto       = "0";
            // 
            // label3
            // 
            this.label3.AutoSize    = true;
            this.label3.Location    = new System.Drawing.Point(163, 94);
            this.label3.Name        = "label3";
            this.label3.Size        = new System.Drawing.Size(28, 13);
            this.label3.TabIndex    = 10;
            this.label3.Text = "X fin";
            // 
            // buttonSubstractYini
            // 
            this.buttonSubstractYini.Location   = new System.Drawing.Point(343, 56);
            this.buttonSubstractYini.Name       = "buttonSubstractYini";
            this.buttonSubstractYini.Size       = new System.Drawing.Size(42, 23);
            this.buttonSubstractYini.TabIndex   = 9;
            this.buttonSubstractYini.Text       = "-";
            this.buttonSubstractYini.UseVisualStyleBackColor = true;
            // 
            // buttonAddYini
            // 
            this.buttonAddYini.Location = new System.Drawing.Point(295, 56);
            this.buttonAddYini.Name     = "buttonAddYini";
            this.buttonAddYini.Size     = new System.Drawing.Size(42, 23);
            this.buttonAddYini.TabIndex = 8;
            this.buttonAddYini.Text     = "+";
            this.buttonAddYini.UseVisualStyleBackColor = true;
            // 
            // numericTextBoxYinit
            // 
            this.numericTextBoxYinit.Location   = new System.Drawing.Point(327, 30);
            this.numericTextBoxYinit.Name       = "numericTextBoxYinit";
            this.numericTextBoxYinit.Size       = new System.Drawing.Size(33, 20);
            this.numericTextBoxYinit.TabIndex   = 7;
            this.numericTextBoxYinit.Texto      = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(294, 33);
            this.label2.Name     = "label2";
            this.label2.Size     = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Y ini";
            // 
            // buttonSubstractXIni
            // 
            this.buttonSubstractXIni.Location   = new System.Drawing.Point(212, 56);
            this.buttonSubstractXIni.Name       = "buttonSubstractXIni";
            this.buttonSubstractXIni.Size       = new System.Drawing.Size(42, 23);
            this.buttonSubstractXIni.TabIndex   = 5;
            this.buttonSubstractXIni.Text       = "-";
            this.buttonSubstractXIni.UseVisualStyleBackColor = true;
            // 
            // buttonAddXIni
            // 
            this.buttonAddXIni.Location = new System.Drawing.Point(164, 56);
            this.buttonAddXIni.Name     = "buttonAddXIni";
            this.buttonAddXIni.Size     = new System.Drawing.Size(42, 23);
            this.buttonAddXIni.TabIndex = 4;
            this.buttonAddXIni.Text     = "+";
            this.buttonAddXIni.UseVisualStyleBackColor = true;
            // 
            // numericTextBoxXIni
            // 
            this.numericTextBoxXIni.Location = new System.Drawing.Point(196, 30);
            this.numericTextBoxXIni.Name = "numericTextBoxXIni";
            this.numericTextBoxXIni.Size = new System.Drawing.Size(33, 20);
            this.numericTextBoxXIni.TabIndex = 3;
            this.numericTextBoxXIni.Texto = "0";
            // 
            // label1
            // 
            this.label1.AutoSize    = true;
            this.label1.Location    = new System.Drawing.Point(163, 33);
            this.label1.Name        = "label1";
            this.label1.Size        = new System.Drawing.Size(27, 13);
            this.label1.TabIndex    = 2;
            this.label1.Text        = "X ini";
            //
            // camaras
            //
            for (int i = 0; i < numCamaras; i++)
            {
                ThermoVision.CustomControls.CamConfigControl c = new ThermoVision.CustomControls.CamConfigControl();

                c.Location = new System.Drawing.Point(i * c.Width, 0);
                c.Name = "camConfigControl";
                c.Size = new System.Drawing.Size(640, 700);
                c.TabIndex = 100 + i;

                this.Controls.Add(c);

                c.Initialize(this);
                this._system.addThermoCam(c.camara);

                this.camaras.Add(c);
            }

            int width = numCamaras * this.camaras[0].Width;

            if (width < 730)
            {
                width = 730;
            }

            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(width - 90 - 10, 870);
            this.buttonNext.Name     = "buttonNext";
            this.buttonNext.Size     = new System.Drawing.Size(90, 23);
            this.buttonNext.TabIndex = 50;
            this.buttonNext.Text     = "Siguiente >>";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(buttonNext_Click);
            //
            // buttonBack
            //
            this.buttonBack.Location = new System.Drawing.Point(width - 2 * 90  - 2 * 10 , 870);
            this.buttonBack.Name     = "buttonBack";
            this.buttonBack.Size     = new System.Drawing.Size(90, 23);
            this.buttonBack.TabIndex = 51;
            this.buttonBack.Text     = "<< Atrás";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(buttonAtras_Click);
            // 
            // CamerasConfiguration
            // 
            this.ClientSize = new System.Drawing.Size(width, 900);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonBack);
            this.Name = "CamerasConfiguration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
        }

        private void InitializeWithComponent()
        {
            this.buttonAddZona = new System.Windows.Forms.Button();
            this.buttonBorrarZona = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBoxZonas = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBoxSubZonas = new System.Windows.Forms.ListBox();
            this.buttonBorrarSubZona = new System.Windows.Forms.Button();
            this.buttonAddSubZona = new System.Windows.Forms.Button();
            this.buttonSubstractCol = new System.Windows.Forms.Button();
            this.buttonAddCol = new System.Windows.Forms.Button();
            this.numericTextBoxCol = new ThermoVision.CustomControls.NumericTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonSubstracFilas = new System.Windows.Forms.Button();
            this.buttonAddFilas = new System.Windows.Forms.Button();
            this.numericTextBoxFil = new ThermoVision.CustomControls.NumericTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonSubstractYFin = new System.Windows.Forms.Button();
            this.buttonAddYFin = new System.Windows.Forms.Button();
            this.numericTextBoxYFin = new ThermoVision.CustomControls.NumericTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonSubstractXfin = new System.Windows.Forms.Button();
            this.buttonAddXFin = new System.Windows.Forms.Button();
            this.numericTextBoxXfin = new ThermoVision.CustomControls.NumericTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonSubstractYini = new System.Windows.Forms.Button();
            this.buttonAddYini = new System.Windows.Forms.Button();
            this.numericTextBoxYinit = new ThermoVision.CustomControls.NumericTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSubstractXIni = new System.Windows.Forms.Button();
            this.buttonAddXIni = new System.Windows.Forms.Button();
            this.numericTextBoxXIni = new ThermoVision.CustomControls.NumericTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.camaras = new List<ThermoVision.CustomControls.CamConfigControl>();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonAddZona
            // 
            this.buttonAddZona.Location = new System.Drawing.Point(6, 131);
            this.buttonAddZona.Name = "buttonAddZona";
            this.buttonAddZona.Size = new System.Drawing.Size(63, 23);
            this.buttonAddZona.TabIndex = 0;
            this.buttonAddZona.Text = "Agregar";
            this.buttonAddZona.UseVisualStyleBackColor = true;
            this.buttonAddZona.Click += new System.EventHandler(this.buttonAddZona_Click);
            // 
            // buttonBorrarZona
            // 
            this.buttonBorrarZona.Location = new System.Drawing.Point(75, 131);
            this.buttonBorrarZona.Name = "buttonBorrarZona";
            this.buttonBorrarZona.Size = new System.Drawing.Size(61, 23);
            this.buttonBorrarZona.TabIndex = 1;
            this.buttonBorrarZona.Text = "Borrar";
            this.buttonBorrarZona.UseVisualStyleBackColor = true;
            this.buttonBorrarZona.Click += new System.EventHandler(this.buttonBorrarZona_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBoxZonas);
            this.groupBox1.Controls.Add(this.buttonAddZona);
            this.groupBox1.Controls.Add(this.buttonBorrarZona);
            this.groupBox1.Location = new System.Drawing.Point(12, 706);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(156, 161);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Zonas";
            // 
            // listBoxZonas
            // 
            this.listBoxZonas.FormattingEnabled = true;
            this.listBoxZonas.Location = new System.Drawing.Point(6, 19);
            this.listBoxZonas.Name = "listBoxZonas";
            this.listBoxZonas.Size = new System.Drawing.Size(144, 95);
            this.listBoxZonas.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBoxSubZonas);
            this.groupBox2.Controls.Add(this.buttonBorrarSubZona);
            this.groupBox2.Controls.Add(this.buttonAddSubZona);
            this.groupBox2.Controls.Add(this.buttonSubstractCol);
            this.groupBox2.Controls.Add(this.buttonAddCol);
            this.groupBox2.Controls.Add(this.numericTextBoxCol);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.buttonSubstracFilas);
            this.groupBox2.Controls.Add(this.buttonAddFilas);
            this.groupBox2.Controls.Add(this.numericTextBoxFil);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.buttonSubstractYFin);
            this.groupBox2.Controls.Add(this.buttonAddYFin);
            this.groupBox2.Controls.Add(this.numericTextBoxYFin);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.buttonSubstractXfin);
            this.groupBox2.Controls.Add(this.buttonAddXFin);
            this.groupBox2.Controls.Add(this.numericTextBoxXfin);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.buttonSubstractYini);
            this.groupBox2.Controls.Add(this.buttonAddYini);
            this.groupBox2.Controls.Add(this.numericTextBoxYinit);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.buttonSubstractXIni);
            this.groupBox2.Controls.Add(this.buttonAddXIni);
            this.groupBox2.Controls.Add(this.numericTextBoxXIni);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(174, 706);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(538, 161);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Subzonas";
            // 
            // listBoxSubZonas
            // 
            this.listBoxSubZonas.FormattingEnabled = true;
            this.listBoxSubZonas.Location = new System.Drawing.Point(7, 19);
            this.listBoxSubZonas.Name = "listBoxSubZonas";
            this.listBoxSubZonas.Size = new System.Drawing.Size(130, 95);
            this.listBoxSubZonas.TabIndex = 4;
            // 
            // buttonBorrarSubZona
            // 
            this.buttonBorrarSubZona.Location = new System.Drawing.Point(76, 131);
            this.buttonBorrarSubZona.Name = "buttonBorrarSubZona";
            this.buttonBorrarSubZona.Size = new System.Drawing.Size(61, 23);
            this.buttonBorrarSubZona.TabIndex = 4;
            this.buttonBorrarSubZona.Text = "Borrar";
            this.buttonBorrarSubZona.UseVisualStyleBackColor = true;
            this.buttonBorrarSubZona.Click += new System.EventHandler(this.buttonBorrarSubZona_Click);
            // 
            // buttonAddSubZona
            // 
            this.buttonAddSubZona.Location = new System.Drawing.Point(7, 131);
            this.buttonAddSubZona.Name = "buttonAddSubZona";
            this.buttonAddSubZona.Size = new System.Drawing.Size(63, 23);
            this.buttonAddSubZona.TabIndex = 4;
            this.buttonAddSubZona.Text = "Agregar";
            this.buttonAddSubZona.UseVisualStyleBackColor = true;
            this.buttonAddSubZona.Click += new System.EventHandler(this.buttonAddSubZona_Click);
            // 
            // buttonSubstractCol
            // 
            this.buttonSubstractCol.Location = new System.Drawing.Point(488, 116);
            this.buttonSubstractCol.Name = "buttonSubstractCol";
            this.buttonSubstractCol.Size = new System.Drawing.Size(42, 23);
            this.buttonSubstractCol.TabIndex = 25;
            this.buttonSubstractCol.Text = "-";
            this.buttonSubstractCol.UseVisualStyleBackColor = true;
            // 
            // buttonAddCol
            // 
            this.buttonAddCol.Location = new System.Drawing.Point(440, 116);
            this.buttonAddCol.Name = "buttonAddCol";
            this.buttonAddCol.Size = new System.Drawing.Size(42, 23);
            this.buttonAddCol.TabIndex = 24;
            this.buttonAddCol.Text = "+";
            this.buttonAddCol.UseVisualStyleBackColor = true;
            // 
            // numericTextBoxCol
            // 
            this.numericTextBoxCol.Location = new System.Drawing.Point(472, 90);
            this.numericTextBoxCol.Name = "numericTextBoxCol";
            this.numericTextBoxCol.Size = new System.Drawing.Size(33, 20);
            this.numericTextBoxCol.TabIndex = 23;
            this.numericTextBoxCol.Texto = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(414, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Columnas";
            // 
            // buttonSubstracFilas
            // 
            this.buttonSubstracFilas.Location = new System.Drawing.Point(488, 56);
            this.buttonSubstracFilas.Name = "buttonSubstracFilas";
            this.buttonSubstracFilas.Size = new System.Drawing.Size(42, 23);
            this.buttonSubstracFilas.TabIndex = 21;
            this.buttonSubstracFilas.Text = "-";
            this.buttonSubstracFilas.UseVisualStyleBackColor = true;
            // 
            // buttonAddFilas
            // 
            this.buttonAddFilas.Location = new System.Drawing.Point(440, 56);
            this.buttonAddFilas.Name = "buttonAddFilas";
            this.buttonAddFilas.Size = new System.Drawing.Size(42, 23);
            this.buttonAddFilas.TabIndex = 20;
            this.buttonAddFilas.Text = "+";
            this.buttonAddFilas.UseVisualStyleBackColor = true;
            // 
            // numericTextBoxFil
            // 
            this.numericTextBoxFil.Location = new System.Drawing.Point(472, 30);
            this.numericTextBoxFil.Name = "numericTextBoxFil";
            this.numericTextBoxFil.Size = new System.Drawing.Size(33, 20);
            this.numericTextBoxFil.TabIndex = 19;
            this.numericTextBoxFil.Texto = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(439, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Filas";
            // 
            // buttonSubstractYFin
            // 
            this.buttonSubstractYFin.Location = new System.Drawing.Point(343, 116);
            this.buttonSubstractYFin.Name = "buttonSubstractYFin";
            this.buttonSubstractYFin.Size = new System.Drawing.Size(42, 23);
            this.buttonSubstractYFin.TabIndex = 17;
            this.buttonSubstractYFin.Text = "-";
            this.buttonSubstractYFin.UseVisualStyleBackColor = true;
            // 
            // buttonAddYFin
            // 
            this.buttonAddYFin.Location = new System.Drawing.Point(295, 116);
            this.buttonAddYFin.Name = "buttonAddYFin";
            this.buttonAddYFin.Size = new System.Drawing.Size(42, 23);
            this.buttonAddYFin.TabIndex = 16;
            this.buttonAddYFin.Text = "+";
            this.buttonAddYFin.UseVisualStyleBackColor = true;
            // 
            // numericTextBoxYFin
            // 
            this.numericTextBoxYFin.Location = new System.Drawing.Point(327, 90);
            this.numericTextBoxYFin.Name = "numericTextBoxYFin";
            this.numericTextBoxYFin.Size = new System.Drawing.Size(33, 20);
            this.numericTextBoxYFin.TabIndex = 15;
            this.numericTextBoxYFin.Texto = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(294, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Y fin";
            // 
            // buttonSubstractXfin
            // 
            this.buttonSubstractXfin.Location = new System.Drawing.Point(212, 117);
            this.buttonSubstractXfin.Name = "buttonSubstractXfin";
            this.buttonSubstractXfin.Size = new System.Drawing.Size(42, 23);
            this.buttonSubstractXfin.TabIndex = 13;
            this.buttonSubstractXfin.Text = "-";
            this.buttonSubstractXfin.UseVisualStyleBackColor = true;
            // 
            // buttonAddXFin
            // 
            this.buttonAddXFin.Location = new System.Drawing.Point(164, 117);
            this.buttonAddXFin.Name = "buttonAddXFin";
            this.buttonAddXFin.Size = new System.Drawing.Size(42, 23);
            this.buttonAddXFin.TabIndex = 12;
            this.buttonAddXFin.Text = "+";
            this.buttonAddXFin.UseVisualStyleBackColor = true;
            // 
            // numericTextBoxXfin
            // 
            this.numericTextBoxXfin.Location = new System.Drawing.Point(196, 91);
            this.numericTextBoxXfin.Name = "numericTextBoxXfin";
            this.numericTextBoxXfin.Size = new System.Drawing.Size(33, 20);
            this.numericTextBoxXfin.TabIndex = 11;
            this.numericTextBoxXfin.Texto = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(163, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "X fin";
            // 
            // buttonSubstractYini
            // 
            this.buttonSubstractYini.Location = new System.Drawing.Point(343, 56);
            this.buttonSubstractYini.Name = "buttonSubstractYini";
            this.buttonSubstractYini.Size = new System.Drawing.Size(42, 23);
            this.buttonSubstractYini.TabIndex = 9;
            this.buttonSubstractYini.Text = "-";
            this.buttonSubstractYini.UseVisualStyleBackColor = true;
            // 
            // buttonAddYini
            // 
            this.buttonAddYini.Location = new System.Drawing.Point(295, 56);
            this.buttonAddYini.Name = "buttonAddYini";
            this.buttonAddYini.Size = new System.Drawing.Size(42, 23);
            this.buttonAddYini.TabIndex = 8;
            this.buttonAddYini.Text = "+";
            this.buttonAddYini.UseVisualStyleBackColor = true;
            // 
            // numericTextBoxYinit
            // 
            this.numericTextBoxYinit.Location = new System.Drawing.Point(327, 30);
            this.numericTextBoxYinit.Name = "numericTextBoxYinit";
            this.numericTextBoxYinit.Size = new System.Drawing.Size(33, 20);
            this.numericTextBoxYinit.TabIndex = 7;
            this.numericTextBoxYinit.Texto = "0";
            // 
            // label2
            // 
            this.label2.AutoSize    = true;
            this.label2.Location    = new System.Drawing.Point(294, 33);
            this.label2.Name        = "label2";
            this.label2.Size        = new System.Drawing.Size(27, 13);
            this.label2.TabIndex    = 6;
            this.label2.Text        = "Y ini";
            // 
            // buttonSubstractXIni
            // 
            this.buttonSubstractXIni.Location   = new System.Drawing.Point(212, 56);
            this.buttonSubstractXIni.Name       = "buttonSubstractXIni";
            this.buttonSubstractXIni.Size       = new System.Drawing.Size(42, 23);
            this.buttonSubstractXIni.TabIndex   = 5;
            this.buttonSubstractXIni.Text       = "-";
            this.buttonSubstractXIni.UseVisualStyleBackColor = true;
            // 
            // buttonAddXIni
            // 
            this.buttonAddXIni.Location = new System.Drawing.Point(164, 56);
            this.buttonAddXIni.Name     = "buttonAddXIni";
            this.buttonAddXIni.Size     = new System.Drawing.Size(42, 23);
            this.buttonAddXIni.TabIndex = 4;
            this.buttonAddXIni.Text     = "+";
            this.buttonAddXIni.UseVisualStyleBackColor = true;
            // 
            // numericTextBoxXIni
            // 
            this.numericTextBoxXIni.Location = new System.Drawing.Point(196, 30);
            this.numericTextBoxXIni.Name = "numericTextBoxXIni";
            this.numericTextBoxXIni.Size = new System.Drawing.Size(33, 20);
            this.numericTextBoxXIni.TabIndex = 3;
            this.numericTextBoxXIni.Texto = "0";
            // 
            // label1
            // 
            this.label1.AutoSize    = true;
            this.label1.Location    = new System.Drawing.Point(163, 33);
            this.label1.Name        = "label1";
            this.label1.Size        = new System.Drawing.Size(27, 13);
            this.label1.TabIndex    = 2;
            this.label1.Text        = "X ini";
            //
            // camaras
            //
            for (int i = 0; i < this._system.ThermoCams.Count; i++)
            {
                ThermoVision.CustomControls.CamConfigControl c = new ThermoVision.CustomControls.CamConfigControl();

                c.Location  = new System.Drawing.Point(i * c.Width, 0);
                c.Name      = "camConfigControl";
                c.Size      = new System.Drawing.Size(640, 700);
                c.TabIndex  = 100 + i;

                this.Controls.Add(c);

                c.Initialize(this, this._system.ThermoCams[i]);
                c.Conectar();

                this.camaras.Add(c);
            }

            int width = this._system.ThermoCams.Count * this.camaras[0].Width;

            if (width < 730)
            {
                width = 730;
            }

            // 
            // buttonNext
            // 
            this.buttonNext.Location    = new System.Drawing.Point(width - 90 - 10, 870);
            this.buttonNext.Name        = "buttonNext";
            this.buttonNext.Size        = new System.Drawing.Size(90, 23);
            this.buttonNext.TabIndex    = 50;
            this.buttonNext.Text        = "Siguiente >>";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click      += new System.EventHandler(buttonNext_Click);
            //
            // buttonBack
            //
            this.buttonBack.Location    = new System.Drawing.Point(width - 2 * 90 - 2 * 10, 870);
            this.buttonBack.Name        = "buttonBack";
            this.buttonBack.Size        = new System.Drawing.Size(90, 23);
            this.buttonBack.TabIndex    = 51;
            this.buttonBack.Text        = "<< Atrás";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click      += new System.EventHandler(buttonAtras_Click);
            // 
            // CamerasConfiguration
            // 
            this.ClientSize = new System.Drawing.Size(width, 900);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonBack);
            this.Name = "CamerasConfiguration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button     buttonAddZona;
        private System.Windows.Forms.Button     buttonBorrarZona;
        private System.Windows.Forms.GroupBox   groupBox1;
        private System.Windows.Forms.ListBox    listBoxZonas;
        private System.Windows.Forms.GroupBox   groupBox2;
        private System.Windows.Forms.Button     buttonSubstractCol;
        private System.Windows.Forms.Button     buttonAddCol;
        private ThermoVision.CustomControls.NumericTextBox numericTextBoxCol;
        private System.Windows.Forms.Label      label6;
        private System.Windows.Forms.Button     buttonSubstracFilas;
        private System.Windows.Forms.Button     buttonAddFilas;
        private ThermoVision.CustomControls.NumericTextBox numericTextBoxFil;
        private System.Windows.Forms.Label      label5;
        private System.Windows.Forms.Button     buttonSubstractYFin;
        private System.Windows.Forms.Button     buttonAddYFin;
        private ThermoVision.CustomControls.NumericTextBox numericTextBoxYFin;
        private System.Windows.Forms.Label      label4;
        private System.Windows.Forms.Button     buttonSubstractXfin;
        private System.Windows.Forms.Button     buttonAddXFin;
        private ThermoVision.CustomControls.NumericTextBox numericTextBoxXfin;
        private System.Windows.Forms.Label      label3;
        private System.Windows.Forms.Button     buttonSubstractYini;
        private System.Windows.Forms.Button     buttonAddYini;
        private ThermoVision.CustomControls.NumericTextBox numericTextBoxYinit;
        private System.Windows.Forms.Label      label2;
        private System.Windows.Forms.Button     buttonSubstractXIni;
        private System.Windows.Forms.Button     buttonAddXIni;
        private ThermoVision.CustomControls.NumericTextBox numericTextBoxXIni;
        private System.Windows.Forms.Label      label1;
        private System.Windows.Forms.ListBox    listBoxSubZonas;
        private System.Windows.Forms.Button     buttonBorrarSubZona;
        private System.Windows.Forms.Button     buttonAddSubZona;
        private List<ThermoVision.CustomControls.CamConfigControl> camaras;
        private System.Windows.Forms.Button     buttonNext;
        private System.Windows.Forms.Button     buttonBack;


    }
}

