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

namespace ThermoCamApp
{
    public partial class main : Asistente.flowControl
    {
        public Rampa _system                                      
        {
            get;
            set;
        }
        private List<CamControl> camaras = new List<CamControl>();

        private System.Windows.Forms.Label                  labelEstadoOPC;
        private System.Windows.Forms.Label[]                labelsEstadoCamaras;
        private Controls.cannonControl[]                    ccs;
        private Controls.RejillasControl[]                  rcs;
        private System.Windows.Forms.Label[]                labelsCannonNames;
        private System.Windows.Forms.Label                  labelTitulo;
        private System.Windows.Forms.Button buttonVisualizacion;
        private System.Windows.Forms.Button                 buttonConfiguracion;
        private System.Windows.Forms.Button                 buttonAsistente;
        private System.Windows.Forms.Button                 buttonSalir;
        private System.Windows.Forms.DataGridView           listViewEventos;
        private System.Windows.Forms.Label                  labelTime;
        private System.Timers.Timer                         timerHora;

        PictureBox pictureBoxRampa;
        PictureBox pictureBoxRejillas;
        PictureBox pictureBoxLogo;
        PictureBox pictureBoxLogoCliente;

        GroupBox groupBox;

        int nHornos = 22;

        public main(Rampa _system)                                                    
        {
            this._system = _system;

            InitializeComponent();

            this._system.OPCClientOnConnecting += _system_OPCClientOnConnecting;

            int counter = 0;

            this.SuspendLayout();

            #region "Cámaras"
            foreach (ThermoCam t in this._system.ThermoCams)
            {
                // 
                // camControl1
                // 
                CamControl c = new CamControl();

                c.Location = new System.Drawing.Point(c.Width * counter, 100);
                c.Name = "camControl1";
                c.Size = new System.Drawing.Size(c.Width, c.Height);
                c.TabIndex = 100 + counter;

                t.nHornos = nHornos / this._system.ThermoCams.Count;

                t.hornoStart = (this._system.ThermoCams.Count - counter - 1) * t.nHornos;
                t.hornoFin = (this._system.ThermoCams.Count - counter) * t.nHornos;

                c.Initialize(this, t);
                camaras.Add(c);
                this.Controls.Add(c);

                counter++;
            }
            #endregion
            // 
            // listView1
            // 
            this.listViewEventos = new System.Windows.Forms.DataGridView();

            ((System.ComponentModel.ISupportInitialize)(this.listViewEventos)).BeginInit();

            DataGridViewTextBoxColumn Fecha = new System.Windows.Forms.DataGridViewTextBoxColumn()       { HeaderText = "Fecha",        Name = "Fecha" };
            DataGridViewTextBoxColumn Tipo = new System.Windows.Forms.DataGridViewTextBoxColumn()        { HeaderText = "Tipo",         Name = "Tipo" };
            DataGridViewTextBoxColumn Description = new System.Windows.Forms.DataGridViewTextBoxColumn() { HeaderText = "Descripcion",  Name = "Descripcion" };

            this.listViewEventos.ReadOnly = true;
            this.listViewEventos.AllowUserToDeleteRows = false;
            this.listViewEventos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                                                                                Fecha,
                                                                                Tipo,
                                                                                Description});

            this.listViewEventos.Columns["Fecha"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.listViewEventos.Columns["Tipo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.listViewEventos.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            if(camaras.Count == 1 || camaras.Count == 2)
                this.listViewEventos.Location = new System.Drawing.Point(camaras.Count * camaras[0].Width + 5, 100);

            this.listViewEventos.Name = "listView1";
            
            switch(camaras.Count)
            {
                case 1:
                    this.listViewEventos.Size = new System.Drawing.Size(camaras[0].Width * 2 - 15, camaras[0].Height);
                    break;
                case 2:
                    this.listViewEventos.Size = new System.Drawing.Size(camaras[0].Width - 10, camaras[0].Height);
                    break;
            }
            this.listViewEventos.TabIndex = 0;
            this.Controls.Add(this.listViewEventos);
            ((System.ComponentModel.ISupportInitialize)(this.listViewEventos)).EndInit();
            //
            // groupBox
            //
            this.groupBox = new GroupBox();

            this.groupBox.Location = new System.Drawing.Point(10, this.Height - 60);
            this.groupBox.Name = "groupBox2";
            this.groupBox.Size = new System.Drawing.Size(this.Width - 40, 60);
            //this.groupBox.BackColor = Color.Gray;
            this.groupBox.TabIndex = 61;
            this.groupBox.TabStop = false;
            
            this.Controls.Add(this.groupBox);
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo = new PictureBox();
            this.pictureBoxLogo.SizeMode   = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxLogo.Location = new System.Drawing.Point(10, 10);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(270, 80);
            this.pictureBoxLogo.TabIndex = 16;
            this.pictureBoxLogo.TabStop = false;
            this.pictureBoxLogo.Image = System.Drawing.Bitmap.FromFile("Resources\\logo.jpg");
            this.Controls.Add(this.pictureBoxLogo);
            //
            // labelTime
            //
            this.labelTime = new Label();
            this.labelTime.AutoSize = false;
            this.labelTime.Name = "labelEstadoOPC";
            this.labelTime.Size = new System.Drawing.Size(250, 50);
            this.labelTime.TabIndex = 4;
            this.labelTime.Text = "Hora";
            this.labelTime.Location = new System.Drawing.Point(this.pictureBoxLogo.Location.X + this.pictureBoxLogo.Width + 35, 40);
            this.labelTime.Font = new Font(
                                           new FontFamily("Microsoft Sans Serif"),
                                           20,
                                           FontStyle.Bold,
                                           GraphicsUnit.Pixel);
            //this.labelTitulo.Padding = new Padding() { All = 7 };
            //this.labelTitulo.BorderStyle = BorderStyle.FixedSingle;

            this.Controls.Add(this.labelTime);

            this.timerHora = new System.Timers.Timer(1000);
            this.timerHora.Enabled = true;
            this.timerHora.Elapsed += timerHora_Elapsed;
            //
            //pictureBoxLogoCliente
            // 
            this.pictureBoxLogoCliente = new PictureBox();
            this.pictureBoxLogoCliente.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxLogoCliente.Location = new System.Drawing.Point(this.Width - 550, 10);
            this.pictureBoxLogoCliente.Name = "pictureBoxLogo";
            this.pictureBoxLogoCliente.Size = new System.Drawing.Size(525, 80);
            this.pictureBoxLogoCliente.TabIndex = 16;
            this.pictureBoxLogoCliente.TabStop = false;
            this.pictureBoxLogoCliente.Image = System.Drawing.Bitmap.FromFile("Resources\\logoEmpresa.bmp");

            this.Controls.Add(this.pictureBoxLogoCliente);
            //
            // labelTitulo
            //
            this.labelTitulo = new Label();
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Name = "labelEstadoOPC";
            this.labelTitulo.Size = new System.Drawing.Size(150, 50);
            this.labelTitulo.TabIndex = 4;
            this.labelTitulo.Text = "APLICACIÓN PARA EL APAGADO AUTOMATIZADO DE RAMPAS DE COQUE";
            this.labelTitulo.Location = new System.Drawing.Point(this.Width / 2 - 400, 30);
            this.labelTitulo.Font = new Font(
                                           new FontFamily("Microsoft Sans Serif"),
                                           20,
                                           FontStyle.Regular,
                                           GraphicsUnit.Pixel);
            this.labelTitulo.Padding = new Padding() { All = 7 };
            this.labelTitulo.BorderStyle = BorderStyle.FixedSingle;

            this.Controls.Add(this.labelTitulo);
            //
            // buttonConfiguracion
            //
            this.buttonConfiguracion = new System.Windows.Forms.Button();
            this.buttonConfiguracion.Location = new System.Drawing.Point(this.groupBox.Width - 3 * 95 - 2 * 15 - 50 - 20 - 75, 15);
            this.buttonConfiguracion.Name = "button1";
            this.buttonConfiguracion.Size = new System.Drawing.Size(95, 30);
            this.buttonConfiguracion.TabIndex = 0;
            this.buttonConfiguracion.Text = "Configuración";
            this.buttonConfiguracion.UseVisualStyleBackColor = true;
            this.buttonConfiguracion.Click += buttonConfiguracion_Click;

            this.groupBox.Controls.Add(this.buttonConfiguracion);
            //
            // buttonVisualizacion
            //
            this.buttonVisualizacion = new System.Windows.Forms.Button();
            this.buttonVisualizacion.Location = new System.Drawing.Point(this.buttonConfiguracion.Location.X - 95 - 15, 15);
            this.buttonVisualizacion.Name = "button1";
            this.buttonVisualizacion.Size = new System.Drawing.Size(95, 30);
            this.buttonVisualizacion.TabIndex = 0;
            this.buttonVisualizacion.Text = "Visualizacion";
            this.buttonVisualizacion.UseVisualStyleBackColor = true;
            this.buttonVisualizacion.Click += buttonVisualizacion_Click;

            this.groupBox.Controls.Add(this.buttonVisualizacion);
            //
            // buttonAsistente
            //
            this.buttonAsistente = new System.Windows.Forms.Button();
            this.buttonAsistente.Location = new System.Drawing.Point(this.groupBox.Width - 2 * 95 - 15 - 50, 15);
            this.buttonAsistente.Name = "button1";
            this.buttonAsistente.Size = new System.Drawing.Size(95, 30);
            this.buttonAsistente.TabIndex = 0;
            this.buttonAsistente.Text = "Asistente";
            this.buttonAsistente.UseVisualStyleBackColor = true;
            this.buttonAsistente.Click += buttonAsistente_Click;
            this.groupBox.Controls.Add(this.buttonAsistente);
            //
            // buttonSalir
            //
            this.buttonSalir = new System.Windows.Forms.Button();
            this.buttonSalir.Location = new System.Drawing.Point(this.groupBox.Width - 95 - 50, 15);
            this.buttonSalir.Name = "button1";
            this.buttonSalir.Size = new System.Drawing.Size(95, 30);
            this.buttonSalir.TabIndex = 0;
            this.buttonSalir.Text = "Salir";
            this.buttonSalir.UseVisualStyleBackColor = true;
            this.buttonSalir.Click += buttonSalir_Click;
            this.groupBox.Controls.Add(this.buttonSalir);
            //
            // labelEstadoOPC
            //
            this.labelEstadoOPC = new Label();
            this.labelEstadoOPC.AutoSize = false;
            this.labelEstadoOPC.Location = new System.Drawing.Point(10, 15);
            this.labelEstadoOPC.Name = "labelEstadoOPC";
            this.labelEstadoOPC.Size = new System.Drawing.Size(150, 30);
            this.labelEstadoOPC.TabIndex = 4;
            this.labelEstadoOPC.Text = "OPC No conectado";
            this.labelEstadoOPC.Font = new Font(
                                           new FontFamily("Microsoft Sans Serif"),
                                           12,
                                           FontStyle.Regular,
                                           GraphicsUnit.Pixel);
            this.labelEstadoOPC.Padding = new Padding() { All = 7 };
            this.labelEstadoOPC.BorderStyle = BorderStyle.FixedSingle;

            this.groupBox.Controls.Add(this.labelEstadoOPC);
            //
            // labelsEstadoCamaras
            //
            this.labelsEstadoCamaras = new Label[this.camaras.Count];

            for (int i = 0; i < this.labelsEstadoCamaras.Length; i++)
            {
                this.labelsEstadoCamaras[i] = new Label();
                this.labelsEstadoCamaras[i].AutoSize = false;
                this.labelsEstadoCamaras[i].Name = "labelEstadoCamara";
                this.labelsEstadoCamaras[i].Size = new System.Drawing.Size(150, 30);
                this.labelsEstadoCamaras[i].Location = new System.Drawing.Point(
                    this.labelEstadoOPC.Location.X + this.labelEstadoOPC.Width + 20 + i * (this.labelsEstadoCamaras[i].Width + 10),
                     15);
                this.labelsEstadoCamaras[i].TabIndex = 4;
                this.labelsEstadoCamaras[i].Text = "Camara " + (i + 1) + ": Desconectada";
                this.labelsEstadoCamaras[i].BackColor = Color.Red;
                this.labelsEstadoCamaras[i].Font = new Font(
                                               new FontFamily("Microsoft Sans Serif"),
                                               12,
                                               FontStyle.Regular,
                                               GraphicsUnit.Pixel);
                this.labelsEstadoCamaras[i].Padding = new Padding() { All = 7 };
                this.labelsEstadoCamaras[i].BorderStyle = BorderStyle.FixedSingle;
                this.labelsEstadoCamaras[i].Click += main_Click;

                this.groupBox.Controls.Add(this.labelsEstadoCamaras[i]);

                this.camaras[i].camara.ThermoCamConnected += camara_ThermoCamConnected;
                this.camaras[i].camara.ThermoCamDisConnected += camara_ThermoCamDisConnected;
            }

            //

            #region "Rampas"
            if (this._system.Mode == "Rampas")
            {
                this.pictureBoxRampa = new PictureBox();
                ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRampa)).BeginInit();
                // 
                // pictureBoxRampa
                // 
                this.pictureBoxRampa.Location = new System.Drawing.Point(10, this.camaras[0].Location.Y + this.camaras[0].Height + 25);
                this.pictureBoxRampa.Name       = "pictureBoxRampa";
                this.pictureBoxRampa.Size       = new System.Drawing.Size(this.Width - 40, 235);
                //this.pictureBoxRampa.SizeMode   = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                this.pictureBoxRampa.TabIndex   = 0;
                this.pictureBoxRampa.TabStop    = false;
                this.pictureBoxRampa.BorderStyle = BorderStyle.FixedSingle;

                this.Controls.Add(this.pictureBoxRampa);

                ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRampa)).EndInit();

                this.pictureBoxRejillas = new PictureBox();
                ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRejillas)).BeginInit();
                // 
                // pictureBoxRejillas
                // 
                this.pictureBoxRejillas.Location    = new System.Drawing.Point(10, this.pictureBoxRampa.Location.Y + this.pictureBoxRampa.Height + 45);
                this.pictureBoxRejillas.Name        = "pictureBoxRejillas";
                this.pictureBoxRejillas.Size        = new System.Drawing.Size(this.Width - 40, 26);
                this.pictureBoxRejillas.SizeMode    = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                this.pictureBoxRejillas.TabIndex    = 0;
                this.pictureBoxRejillas.TabStop     = false;
                this.pictureBoxRejillas.BorderStyle = BorderStyle.FixedSingle;

                this.Controls.Add(this.pictureBoxRejillas);

                ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRejillas)).EndInit();

                //this.EstadosLabels = new Label[this._system.Zonas.Count];

                //for (int i = 0; i < this._system.Zonas.Count; i++)
                //{
                //    this.EstadosLabels[i] = new Label();
                //    // 
                //    // label3
                //    //
                //    int xPos = i * (this.Width / this._system.Zonas.Count) + (this.Width / this._system.Zonas.Count) / 2;

                //    this.EstadosLabels[i].AutoSize  = true;
                //    this.EstadosLabels[i].Location  = new System.Drawing.Point(xPos, 262 + 500 + 15);
                //    this.EstadosLabels[i].Name      = "label" + i.ToString();
                //    this.EstadosLabels[i].Size      = new System.Drawing.Size(40, 13);
                //    this.EstadosLabels[i].TabIndex  = 4;
                //    this.EstadosLabels[i].Text      = "Vacio";
                //    this.EstadosLabels[i].Font = new Font(
                //                                   new FontFamily("Microsoft Sans Serif"),
                //                                   16,
                //                                   FontStyle.Regular,
                //                                   GraphicsUnit.Pixel);
                //    this.EstadosLabels[i].Padding = new Padding() { All = 10 };
                //    this.EstadosLabels[i].BorderStyle = BorderStyle.FixedSingle;
                    

                //    this._system.Zonas[i].Posicion  = i;
                //    this._system.Zonas[i].zonaStateChanged += main_stateChanged;
                //    this._system.Zonas[i].zonaCoolingStop += main_zonaCoolingStop;
                //    this._system.Zonas[i].zonaEmptyingStop += main_zonaEmptyingStop;

                //    this.Controls.Add(this.EstadosLabels[i]);
                //}                

                this._system.estados.ThermoCamImgCuadradosGenerated += estados_ThermoCamImgCuadradosGenerated;
            }
            #endregion

            this.ResumeLayout();

            this.FormClosing    += main_FormClosing;
            this._system.modoConfiguracion = false;

            Action[] actions = new Action[2];
            actions[0] = new Action(conectarOPC);
            actions[1] = new Action(containVaciadoZones);

            Parallel.Invoke(actions);

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////// ZONAS DE APAGADO ////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            if (this._system.Mode == "Rampas")
            {
                this.labelsCannonNames = new Label[this._system.Zonas.Count];

                for (int i = 0; i < this.labelsCannonNames.Length; i++)
                {                    
                    // 
                    // label1
                    // 
                    this.labelsCannonNames[i] = new Label();
                    this.labelsCannonNames[i].AutoSize = true;
                    this.labelsCannonNames[i].Location = new System.Drawing.Point(i * (this.Width / this._system.Zonas.Count) + (this.Width / this._system.Zonas.Count) / 2 - 70, 
                                                                            this.pictureBoxRampa.Location.Y - 23);
                    this.labelsCannonNames[i].Name = "label1";
                    this.labelsCannonNames[i].Size = new System.Drawing.Size(140, 13);
                    this.labelsCannonNames[i].TabIndex = 1;
                    this.labelsCannonNames[i].Text = this._system.Zonas[i].Nombre;
                    this.labelsCannonNames[i].Font = new Font(
                                                  new FontFamily("Microsoft Sans Serif"),
                                                  16,
                                                  FontStyle.Regular,
                                                  GraphicsUnit.Pixel);

                    this.Controls.Add(this.labelsCannonNames[i]);
                }

                ccs = new ThermoCamApp.Controls.cannonControl[this._system.Zonas.Count];

                for (int i = 0; i < ccs.Count(); i++)
                {
                    ccs[i] = new Controls.cannonControl(this,
                            this._system.Zonas[i],
                            new Point(i * (this.Width / this._system.Zonas.Count) + (this.Width / this._system.Zonas.Count) / 2,
                            this.pictureBoxRampa.Location.Y + this.pictureBoxRampa.Height + 5),
                            i,
                            new Controls.cannonControl.cannonControlBoolParameterCallback(this._system.changeCannonModeState),
                            new Controls.cannonControl.cannonControlBoolParameterCallback(this._system.changeCannonActivation),
                            new Controls.cannonControl.cannonControlParameterCallback(this._system.decrementCannonXCoordinate),
                            new Controls.cannonControl.cannonControlParameterCallback(this._system.decrementCannonYCoordinate),
                            new Controls.cannonControl.cannonControlParameterCallback(this._system.incrementCannonYCoordinate),
                            new Controls.cannonControl.cannonControlParameterCallback(this._system.incrementCannonXCoordinate));

                    this._system.Zonas[i].Posicion          = i;
                    this._system.Zonas[i].zonaStateChanged += main_stateChanged;
                    this._system.Zonas[i].zonaCoolingStop  += main_zonaCoolingStop;
                    this._system.Zonas[i].zonaEmptyingStop += main_zonaEmptyingStop;

                    this._system.suscribeOPCItem(new StringBuilder("RAMPAS.APAGADO.").Append(this._system.Zonas[i].Nombre).ToString(),
                        "Mode",
                        ccs[i].cannonModeChanged);
                }

                foreach (ZonaApagado z in this._system.Zonas)
                {
                    foreach (SubZona s in z.Children)
                    {
                        s.Selected = true;
                    }
                    try
                    {
                        this._system.getCannonCoordinates(z);
                    }
                    catch (Exception ex)
                    {
                        addElementToListBox(this.listViewEventos, new Evento(DateTime.Now, "Grave", ex.Message));
                    }

                    this._system.suscribeOPCItem(new StringBuilder("RAMPAS.APAGADO.").Append(z.Nombre).ToString(), /////////////////////////////////////////////////////
                        "xPos",
                        new Action<object>(z.coordinateXChanged));
                    this._system.suscribeOPCItem(new StringBuilder("RAMPAS.APAGADO.").Append(z.Nombre).ToString(), /////////////////////////////////////////////////////
                       "yPos",
                       new Action<object>(z.coordinateYChanged));
                    this._system.suscribeOPCItem(new StringBuilder("RAMPAS.APAGADO.").Append(z.Nombre).ToString(), /////////////////////////////////////////////////////
                       "nPos",
                       new Action<object>(z.subZonaNChanged));
                    this._system.suscribeOPCItem(new StringBuilder("RAMPAS.APAGADO.").Append(z.Nombre).ToString(), /////////////////////////////////////////////////////
                       "Valvula",
                       new Action<object>(z.ValvulaStateChanged));
                    this._system.suscribeOPCItem(new StringBuilder("RAMPAS.APAGADO.").Append(z.Nombre).ToString(),
                        "Cooling",
                        new Action<object>(z.coolingStateChanged));

                    this._system.OPCClient.RefreshAsync(new StringBuilder(this._system.Path).Append(".RAMPAS.APAGADO.").Append(z.Nombre).ToString());
                }

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////// ZONAS DE VACIADO ////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                rcs = new Controls.RejillasControl[this._system.ZonasVaciado.Count];

                int totalWidth = 0;

                for (int i = 0; i < this._system.ZonasVaciado.Count; i++)
                {
                    totalWidth += this._system.ZonasVaciado[i].Width;
                }

                for (int i = 0; i < rcs.Length; i++)
                {
                    rcs[i] = new Controls.RejillasControl(
                        this,
                        i,
                        this._system.ZonasVaciado[i],
                        new Point(calcularCoordenadaXInicioZona(i) + this.pictureBoxRejillas.Location.X,
                            this.pictureBoxRejillas.Location.Y + this.pictureBoxRejillas.Height + 10),
                            totalWidth,
                            this.pictureBoxRejillas.Width,
                            new Controls.RejillasControl.trampillaControlBoolParameterCallback(this._system.changeTrampillaModeState),
                            new Controls.RejillasControl.cambiarTrampillaEstadoCallback(this._system.activateRejillaAsync));

                    this._system.suscribeOPCItem(new StringBuilder("RAMPAS.VACIADO.").Append(this._system.ZonasVaciado[i].Nombre).ToString(),
                        "Mode",
                        new Action<object>(rcs[i].trampillaModeChanged));

                    this._system.OPCClient.RefreshAsync(new StringBuilder(this._system.Path).Append(".RAMPAS.VACIADO.").Append(this._system.ZonasVaciado[i].Nombre).ToString());

                }

                foreach (ZonaVaciado z in this._system.ZonasVaciado)
                {
                    int index = 1;

                    foreach (SubZona s in z.Children)
                    {
                        s.vaciado = true;

                        for (int i = 0; i < s.tempMatrix.GetLength(0); i++)
                        {
                            for (int j = 0; j < s.tempMatrix.GetLength(1); j++)
                            {
                                this._system.suscribeOPCItem(new StringBuilder("RAMPAS.VACIADO.").Append(z.Nombre).Append(".").Append(index).Append("_OUTPUTS").ToString(),
                                    new StringBuilder("[").Append(j).Append(",").Append(i).Append("]").ToString(),
                                    new Action<object>(s.tempMatrix[i, j].stateChanged));

                                this._system.OPCClient.RefreshAsync(new StringBuilder(this._system.Path).Append(".RAMPAS.VACIADO.").Append(z.Nombre).Append(".").Append(index).Append("_OUTPUTS").ToString());
                            }
                        }
                        
                        index++;
                    }

                    //this._system.suscribeOPCItem(new StringBuilder("RAMPAS.VACIADO.").Append(z.Nombre).ToString(),
                    //    "X",
                    //    new Action<object>(z.coordinateXVaciadoChanged));
                    //this._system.suscribeOPCItem(new StringBuilder("RAMPAS.VACIADO.").Append(z.Nombre).ToString(),
                    //    "Y",
                    //    new Action<object>(z.coordinateYVaciadoChanged));
                    //this._system.suscribeOPCItem(new StringBuilder("RAMPAS.VACIADO.").Append(z.Nombre).ToString(),
                    //    "n",
                    //    new Action<object>(z.subZonaNVaciadoChanged));
                    this._system.suscribeOPCItem(new StringBuilder("RAMPAS.VACIADO.").Append(z.Nombre).ToString(),
                        "Emptying",
                        new Action<object>(z.emptyingStateChanged));

                    this._system.OPCClient.RefreshAsync(new StringBuilder(this._system.Path).Append(".RAMPAS.VACIADO.").Append(z.Nombre).ToString());
                }

                this._system.estados.widthFinalRampa = this.pictureBoxRampa.Width;
                this._system.estados.heightFinalRampa = this.pictureBoxRampa.Height;

                this._system.estados.widthFinalTrampilla = this.pictureBoxRejillas.Width;
                this._system.estados.heightFinalTrampilla = this.pictureBoxRejillas.Height;

                this._system.OPCClient.SuscribeGroup(new StringBuilder(this._system.Path).Append(".RAMPAS.CONFIGURACION").ToString(), "TempEnfriar", new Action<object>(setTemEnfriar));
                this._system.OPCClient.RefreshAsync(new StringBuilder(this._system.Path).Append(".RAMPAS.CONFIGURACION").ToString());

                //foreach(Zona z in this._system.ZonasVaciado)
                //    this._system.OPCClient.RefreshAsync(new StringBuilder(this._system.Path).Append(".RAMPAS.VACIADO.").Append(z.Nombre).ToString());

            }
        }

        void buttonVisualizacion_Click(object sender, EventArgs e)                      
        {
            Forms.VisualizacionConfig f = new Forms.VisualizacionConfig(this._system);
            f.ShowDialog();
            f.Dispose();
        }
        void timerHora_Elapsed(object sender, System.Timers.ElapsedEventArgs e)         
        {
            changeTextLabel(this.labelTime, DateTime.Now.ToString());
        }

        delegate void changeTextLabelCallback(Label l, string text);
        void changeTextLabel(Label l, string text)                                      
        {
            try
            {
                if (l.InvokeRequired)
                {
                    l.Invoke(new changeTextLabelCallback(changeTextLabel), l, text);
                }
                else
                {
                    l.Text = text;
                }
            }
            catch { }
        }

        void setTemEnfriar(object Value)                                                
        {
            if (Value is int)
                this._system.estados.tempLimiteHayQueEnfriar = (int)Value;
        }

        void buttonConfiguracion_Click(object sender, EventArgs e)                      
        {
            Forms.RampaConfig f = new Forms.RampaConfig(this._system);
            f.ShowDialog();
            f.Dispose();
        }

        void main_Click(object sender, EventArgs e)                                     
        {
            for (int i = 0; i < this.labelsEstadoCamaras.Length; i++)
            {
                if(this.labelsEstadoCamaras[i].Equals(sender))
                {
                    if(!this._system.ThermoCams[i].Conectado)
                        this._system.ThermoCams[i].Conectar();
                }
            }
        }

        void buttonSalir_Click(object sender, EventArgs e)                              
        {
            this.Close();
        }

        int calcularCoordenadaXInicioZona(int posicion)                                 
        {
            int res = 0;

            for (int i = 0; i < posicion; i++)
            {
                res += this._system.ZonasVaciado[i].Width;
            }

            int totalWidth = 0;

            for (int i = 0; i < this._system.ZonasVaciado.Count; i++)
            {
                totalWidth += this._system.ZonasVaciado[i].Width;
            }

            res = res * this.pictureBoxRejillas.Width / totalWidth;

            return res;
        }

        void conectarOPC()                                                              
        {
            this._system.conectarClienteOPC();
        }

        void camara_ThermoCamConnected(object sender, EventArgs e)                      
        {
            if (sender is ThermoCam)
            {
                for (int i = 0; i < this.camaras.Count; i++)
                {
                    if (this.camaras[i].camara.Equals((sender as ThermoCam)))
                    {
                        addElementToListBox(this.listViewEventos, new Evento(DateTime.Now, "Evento", "Camara " + (i + 1) + ": Conectada"));

                        updateLabelTextProperty(this.labelsEstadoCamaras[i], "Camara " + (i + 1) + ": Conectada");
                        updateLabelBackColor(this.labelsEstadoCamaras[i], Color.Green);
                    }//if
                }//for                
            }//if
        }
        void camara_ThermoCamDisConnected(object sender, EventArgs e)                   
        {
            if (sender is ThermoCam)
            {
                for (int i = 0; i < this.camaras.Count; i++)
                {
                    if (this.camaras[i].camara.Equals((sender as ThermoCam)))
                    {
                        addElementToListBox(this.listViewEventos, new Evento(DateTime.Now, "Grave", "Camara " + (i + 1) + ": Desconectada"));

                        updateLabelTextProperty(this.labelsEstadoCamaras[i], "Camara " + (i + 1) + ": Desconectada");
                        updateLabelBackColor(this.labelsEstadoCamaras[i], Color.Red);
                    }//if
                }//for
            }//if
        }

        void containVaciadoZones()                                                      
        {
            object _lock = new object();

            foreach (ZonaVaciado zVaciado in this._system.ZonasVaciado)
            //Parallel.ForEach<Zona>(this._system.ZonasVaciado, (zVaciado) =>
            {
                zVaciado.zonasContenidas.Clear();

                foreach (SubZona s in zVaciado.Children)
                {
                    //Buscar zonas de apagado superpuestas 
                    foreach (ZonaApagado zApagado in this._system.Zonas)
                    {
                        foreach (SubZona sApagado in zApagado.Children)
                        {
                            if (sApagado.ThermoParent != null && sApagado.ThermoParent.Equals(s.ThermoParent))
                            {
                                int sWidth = s.Fin.X - s.Inicio.X;
                                int sHeight = s.Fin.Y - s.Inicio.Y;

                                if (s.Inicio.Y <= sApagado.Fin.Y && s.Fin.Y >= sApagado.Fin.Y &&
                                    s.Inicio.X <= sApagado.Fin.X && s.Fin.X >= sApagado.Fin.X)
                                {
                                    //PERTENECE
                                    if (!((ZonaVaciado)(s.Parent)).zonasContenidas.Contains(zApagado))
                                    {
                                        lock (_lock)
                                            ((ZonaVaciado)(s.Parent)).zonasContenidas.Add(zApagado);
                                    }
                                    if (!zApagado.zonasContenidas.Contains(s.Parent))
                                    {
                                        lock (_lock)
                                            zApagado.zonasContenidas.Add((ZonaVaciado)s.Parent);
                                    }
                                }

                                if (sApagado.Inicio.Y <= s.Fin.Y && sApagado.Fin.Y >= s.Fin.Y &&
                                    sApagado.Inicio.X <= s.Fin.X && sApagado.Fin.X >= s.Fin.X)
                                {
                                    //PERTENECE
                                    if (!((ZonaVaciado)(s.Parent)).zonasContenidas.Contains(zApagado))
                                    {
                                        lock (_lock)
                                            ((ZonaVaciado)(s.Parent)).zonasContenidas.Add(zApagado);
                                    }
                                    if (!zApagado.zonasContenidas.Contains(s.Parent))
                                    {
                                        lock (_lock)
                                            zApagado.zonasContenidas.Add((ZonaVaciado)s.Parent);
                                    }
                                }

                                if (s.Inicio.Y <= sApagado.Inicio.Y && s.Fin.Y >= sApagado.Inicio.Y &&
                                    s.Inicio.X <= sApagado.Inicio.X && s.Fin.X >= sApagado.Inicio.X)
                                {
                                    //PERTENECE
                                    if (!((ZonaVaciado)(s.Parent)).zonasContenidas.Contains(zApagado))
                                    {
                                        lock (_lock)
                                            ((ZonaVaciado)(s.Parent)).zonasContenidas.Add(zApagado);
                                    }
                                    if (!zApagado.zonasContenidas.Contains(s.Parent))
                                    {
                                        lock (_lock)
                                            zApagado.zonasContenidas.Add((ZonaVaciado)s.Parent);
                                    }
                                }
                                if (sApagado.Inicio.Y <= s.Inicio.Y && sApagado.Fin.Y >= s.Inicio.Y &&
                                    sApagado.Inicio.X <= s.Inicio.X && sApagado.Fin.X >= s.Inicio.X)
                                {
                                    //PERTENECE
                                    if (!((ZonaVaciado)(s.Parent)).zonasContenidas.Contains(zApagado))
                                    {
                                        lock (_lock)
                                            ((ZonaVaciado)(s.Parent)).zonasContenidas.Add(zApagado);
                                    }
                                    if (!zApagado.zonasContenidas.Contains(s.Parent))
                                    {
                                        lock (_lock)
                                            zApagado.zonasContenidas.Add((ZonaVaciado)s.Parent);
                                    }
                                }
                            }
                        }//foreach
                    }//foreach
                }
            }//foreach
        }

        void _system_OPCClientOnConnecting(object sender, EventArgs e)                  
        {            
            this._system.OPCClient.Connected    += OPCClient_Connected;
            this._system.OPCClient.DataSent     += OPCClient_DataSent;
            this._system.OPCClient.Disconnected += OPCClient_Disconnected;
            this._system.OPCClient.OPCError     += OPCClient_OPCError;
            this._system.OPCClient.OPCWrittingError += OPCClient_OPCWrittingError;
        }

        void OPCClient_DataSent(object sender, EventArgs e)                             
        {
            updateLabelTextProperty(this.labelEstadoOPC, "OPC Conectado");
            updateLabelBackColor(this.labelEstadoOPC, Color.Green);
        }   
     
        void OPCClient_Connected(object sender, EventArgs e)                            
        {
            addElementToListBox(this.listViewEventos, new Evento(DateTime.Now, "Evento", "Cliente OPC conectado."));

            updateLabelTextProperty(this.labelEstadoOPC, "OPC Conectado");
            updateLabelBackColor(this.labelEstadoOPC, Color.Green);
        }
        void OPCClient_Disconnected(object sender, EventArgs e)                         
        {
            addElementToListBox(this.listViewEventos, new Evento(DateTime.Now, "Grave", "Cliente OPC desconectado."));

            updateLabelTextProperty(this.labelEstadoOPC, "OPC Desconectado");
            updateLabelBackColor(this.labelEstadoOPC, Color.Red);

            this._system.OPCClient.Connected        -= OPCClient_Connected;
            this._system.OPCClient.DataSent         -= OPCClient_DataSent;
            this._system.OPCClient.Disconnected     -= OPCClient_Disconnected;
            this._system.OPCClient.OPCError         -= OPCClient_OPCError;
            this._system.OPCClient.OPCWrittingError -= OPCClient_OPCWrittingError;

            //if (!this._system.OPCClient._connected && !this._system.OPCClient._connecting)
            //{
            //    Task taskConectar = new Task(delegate { this._system.conectarClienteOPC(); });
            //    taskConectar.Start();
            //}
        }
        void OPCClient_OPCError(object sender, string gravity, string message)          
        {
            addElementToListBox(this.listViewEventos, new Evento(DateTime.Now, gravity, message));
        }
        void OPCClient_OPCWrittingError(object sender, string gravity, string message)  
        {
            addElementToListBox(this.listViewEventos, new Evento(DateTime.Now, gravity, message));

            updateLabelTextProperty(this.labelEstadoOPC, "OPC Error");
            updateLabelBackColor(this.labelEstadoOPC, Color.Red);
        }
        
        void buttonAsistente_Click(object sender, EventArgs e)                          
        {
            this.Salir = false;
            this.Atras = true;
            this.Close();
        }
        void main_stateChanged(object sender, Zona.States state)                        
        {
            updateLabelTextProperty(ccs[((Zona) sender).Posicion].labelEstado, state.ToString());

            switch (state)
            {
                case Zona.States.Manual:
                    updateLabelBackColor(ccs[((Zona)sender).Posicion].labelEstado, Color.Yellow);
                    break;
                case Zona.States.Vacio:
                    updateLabelBackColor(ccs[((Zona) sender).Posicion].labelEstado, Color.Transparent);
                    break;
                case Zona.States.Lleno:
                    updateLabelBackColor(ccs[((Zona)sender).Posicion].labelEstado, Color.Green);
                    break;
                case Zona.States.Enfriando:
                    updateLabelBackColor(ccs[((Zona)sender).Posicion].labelEstado, Color.Orange);
                    addElementToListBox(this.listViewEventos, new Evento(DateTime.Now, "Estado", "Zona " + (sender as Zona).Posicion + ": Empieza a enfriar."));
                    break;
                case Zona.States.Esperando:
                    updateLabelBackColor(ccs[((Zona)sender).Posicion].labelEstado, Color.Gray);
                    break;
                case Zona.States.Vaciando:
                    updateLabelBackColor(ccs[((Zona)sender).Posicion].labelEstado, Color.Blue);
                    addElementToListBox(this.listViewEventos, new Evento(DateTime.Now, "Estado", "Zona " + (sender as Zona).Posicion + ": Empieza a vaciar."));
                    break;
            }
        }
        void main_zonaCoolingStop(object sender, EventArgs e)                           
        {
            addElementToListBox(this.listViewEventos, new Evento(DateTime.Now, "Estado", "Zona " + (sender as Zona).Posicion + ": Ha parado de enfriar."));
        }
        void main_zonaEmptyingStop(object sender, EventArgs e)                          
        {
            addElementToListBox(this.listViewEventos, new Evento(DateTime.Now, "Estado", "Zona " + (sender as Zona).Posicion + ": Ha parado de vaciar."));
        }

        void estados_ThermoCamImgCuadradosGenerated(object sender, ThermoVision.Tipos.ThemoCamImgCuadradosArgs e) 
        {
            updatePictureBox(this.pictureBoxRampa, ref e.ImagenRampa);
            updatePictureBox(this.pictureBoxRejillas, ref e.ImagenRejillas);
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
        private void updateLabelTextColor(Label l, Color c)                             
        {
            if (l.InvokeRequired)
            {
                l.Invoke(new updateLabelBackColorDelegate(updateLabelBackColor), l, c);
            }
            else
            {
                l.ForeColor = c;
            }
        }

        private delegate void addElementToListBoxCallback(DataGridView l, Evento element);
        private void addElementToListBox(DataGridView l, Evento element)                
        {
            if (l.InvokeRequired)
            {
                l.Invoke(new addElementToListBoxCallback(addElementToListBox), l, element);
            }
            else
            {

                try
                {
                    DataGridViewRow row = (DataGridViewRow)l.Rows[0].Clone();
                    row.Cells[0].Value = element.Fecha;
                    row.Cells[1].Value = element.Tipo;
                    row.Cells[2].Value = element.Message;

                    switch (element.Tipo)
                    {
                        case "Fallo":
                            row.DefaultCellStyle.BackColor = Color.Red;
                            break;
                        case "Evento":
                            row.DefaultCellStyle.BackColor = Color.Green;
                            break;
                        default:
                            row.DefaultCellStyle.BackColor = Color.White;
                            break;
                    }

                    l.Rows.Add(row);
                    l.FirstDisplayedScrollingRowIndex = l.RowCount - 1;

                    l.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    if (l.Columns[0].Width + l.Columns[1].Width + l.Columns[2].Width > l.Width)
                    {
                        l.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }
                    else
                    {
                        l.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }

                    l.Refresh();
                }
                catch(Exception ex) 
                {
                    ex.ToString();
                }
            }
        }
    }
}

public class Evento                                                                     
{
    public DateTime Fecha;
    public string   Message;
    public string   Tipo;

    public Evento(DateTime fecha, string gravedad, string message)  
    {
        this.Fecha = fecha;
        this.Message = message;
        this.Tipo = gravedad;
    }
    public override string ToString()                               
    {
        return this.Fecha.ToString("d") + " - " + this.Tipo.ToUpper() + " - " + this.Message;
    }
}
