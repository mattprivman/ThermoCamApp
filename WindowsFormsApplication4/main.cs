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
        public Sistema _system                                      
        {
            get;
            set;
        }
        private List<CamControl> camaras = new List<CamControl>();

        private ThermoVision.CustomControls.NumericTextBox  numericTextBoxTempApagadoLimite;
        private System.Windows.Forms.Label                  label1;
        private System.Windows.Forms.Label                  labelEstadoOPC;
        private System.Windows.Forms.Label[]                labelsEstadoCamaras;
        private System.Windows.Forms.Label                  label2;
        private ThermoVision.CustomControls.NumericTextBox  numericTextBoxTempLimiteVaciado;
        private Controls.cannonControl[]                    ccs;
        private Controls.RejillasControl[]                  rcs;
        private System.Windows.Forms.Button                 buttonAsistente;
        private System.Windows.Forms.DataGridView           listViewEventos;

        PictureBox pictureBoxRampa;
        PictureBox pictureBoxRejillas;

        public main(Sistema _system)                                                    
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
                this.listViewEventos.Location = new System.Drawing.Point(camaras.Count * camaras[0].Width + 5, 10);

            this.listViewEventos.Name = "listView1";
            
            switch(camaras.Count)
            {
                case 1:
                    this.listViewEventos.Size = new System.Drawing.Size(camaras[0].Width * 2 - 15, camaras[0].Height -10);
                    break;
                case 2:
                    this.listViewEventos.Size = new System.Drawing.Size(camaras[0].Width - 10, camaras[0].Height - 10);
                    break;
            }
            this.listViewEventos.TabIndex = 0;
            this.Controls.Add(this.listViewEventos);
            ((System.ComponentModel.ISupportInitialize)(this.listViewEventos)).EndInit();
            //
            // buttonAsistente
            //
            this.buttonAsistente = new System.Windows.Forms.Button();
            this.buttonAsistente.Location = new System.Drawing.Point(this.Width - 95  - 50, this.Height - 77 - 50);
            this.buttonAsistente.Name = "button1";
            this.buttonAsistente.Size = new System.Drawing.Size(95, 77);
            this.buttonAsistente.TabIndex = 0;
            this.buttonAsistente.Text = "Asistente";
            this.buttonAsistente.UseVisualStyleBackColor = true;
            this.buttonAsistente.Click += buttonAsistente_Click;
            this.Controls.Add(this.buttonAsistente);
            //
            // labelEstadoOPC
            //
            this.labelEstadoOPC = new Label();
            this.labelEstadoOPC.AutoSize = false;
            this.labelEstadoOPC.Location = new System.Drawing.Point(10, this.Height - 100);
            this.labelEstadoOPC.Name = "labelEstadoOPC";
            this.labelEstadoOPC.Size = new System.Drawing.Size(170, 40);
            this.labelEstadoOPC.TabIndex = 4;
            this.labelEstadoOPC.Text = "OPC No conectado";
            this.labelEstadoOPC.Font = new Font(
                                           new FontFamily("Microsoft Sans Serif"),
                                           16,
                                           FontStyle.Regular,
                                           GraphicsUnit.Pixel);
            this.labelEstadoOPC.Padding = new Padding() { All = 7 };
            this.labelEstadoOPC.BorderStyle = BorderStyle.FixedSingle;

            this.Controls.Add(this.labelEstadoOPC);
            //
            // labelsEstadoCamaras
            //
            this.labelsEstadoCamaras = new Label[this.camaras.Count];

            for (int i = 0; i < this.labelsEstadoCamaras.Length; i++)
            {
                this.labelsEstadoCamaras[i] = new Label();
                this.labelsEstadoCamaras[i].AutoSize = false;
                this.labelsEstadoCamaras[i].Name = "labelEstadoCamara";
                this.labelsEstadoCamaras[i].Size = new System.Drawing.Size(250, 40);
                this.labelsEstadoCamaras[i].Location = new System.Drawing.Point(
                    2 * 10 + this.labelEstadoOPC.Width + i * (this.labelsEstadoCamaras[i].Width + 10), 
                    this.Height - 100);
                this.labelsEstadoCamaras[i].TabIndex = 4;
                this.labelsEstadoCamaras[i].Text = "Camara " + (i + 1) + ": Desconectada";
                this.labelsEstadoCamaras[i].BackColor = Color.Red;
                this.labelsEstadoCamaras[i].Font = new Font(
                                               new FontFamily("Microsoft Sans Serif"),
                                               16,
                                               FontStyle.Regular,
                                               GraphicsUnit.Pixel);
                this.labelsEstadoCamaras[i].Padding = new Padding() { All = 7 };
                this.labelsEstadoCamaras[i].BorderStyle = BorderStyle.FixedSingle;

                this.Controls.Add(this.labelsEstadoCamaras[i]);

                this.camaras[i].camara.ThermoCamConnected += camara_ThermoCamConnected;
                this.camaras[i].camara.ThermoCamDisConnected += camara_ThermoCamDisConnected;
            }

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
                // pictureBoxRampa
                // 
                this.pictureBoxRampa.Location   = new System.Drawing.Point(10, 486);
                this.pictureBoxRampa.Name       = "pictureBoxRampa";
                this.pictureBoxRampa.Size       = new System.Drawing.Size(this.Width - 40, 235);
                this.pictureBoxRampa.SizeMode   = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
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
                this.pictureBoxRejillas.Location    = new System.Drawing.Point(10, this.pictureBoxRampa.Location.Y + this.pictureBoxRampa.Height + 110);
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

                // 
                // numericTextBoxTempApagadoLimite
                // 
                this.numericTextBoxTempApagadoLimite.Location = new System.Drawing.Point(this.camaras[0].Width * counter + 148, this.Height - 140);
                this.numericTextBoxTempApagadoLimite.Name = "numericTextBoxTempApagadoLimite";
                this.numericTextBoxTempApagadoLimite.Size = new System.Drawing.Size(102, 20);
                this.numericTextBoxTempApagadoLimite.TabIndex = 0;
                this.numericTextBoxTempApagadoLimite.textoCambiado += numericTextBoxTempApagadoLimite_textoCambiado;
                // 
                // label1
                // 
                this.label1.AutoSize = true;
                this.label1.Location = new System.Drawing.Point(this.camaras[0].Width * counter + 10, this.Height - 140);
                this.label1.Name = "label1";
                this.label1.Size = new System.Drawing.Size(138, 13);
                this.label1.TabIndex = 1;
                this.label1.Text = "Temperatura limite apagado";
                // 
                // label2
                // 
                this.label2.AutoSize = true;
                this.label2.Location = new System.Drawing.Point(this.camaras[0].Width * counter + 10, this.Height - 90);
                this.label2.Name = "label2";
                this.label2.Size = new System.Drawing.Size(134, 13);
                this.label2.TabIndex = 3;
                this.label2.Text = "Temperatura limite vaciado";
                // 
                // numericTextBoxTempLimiteVaciado
                // 
                this.numericTextBoxTempLimiteVaciado.Location = new System.Drawing.Point(this.camaras[0].Width * counter + 148, this.Height - 90);
                this.numericTextBoxTempLimiteVaciado.Name = "numericTextBoxTempLimiteVaciado";
                this.numericTextBoxTempLimiteVaciado.Size = new System.Drawing.Size(102, 20);
                this.numericTextBoxTempLimiteVaciado.textoCambiado += numericTextBoxTempLimiteVaciado_textoCambiado;

                //this.Controls.Add(this.label2);
                //this.Controls.Add(this.numericTextBoxTempLimiteVaciado);
                //this.Controls.Add(this.label1);
                //this.Controls.Add(this.numericTextBoxTempApagadoLimite);

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

            if (this._system.Mode == "Rampas")
            {
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
                            new Controls.cannonControl.cannonControlParameterCallback(this._system.incrementCannonXCoordinate),
                            new Controls.cannonControl.cannonControlBoolCallback(this._system.readCannonState));

                    this._system.Zonas[i].Posicion          = i;
                    this._system.Zonas[i].zonaStateChanged += main_stateChanged;
                    this._system.Zonas[i].zonaCoolingStop  += main_zonaCoolingStop;
                    this._system.Zonas[i].zonaEmptyingStop += main_zonaEmptyingStop;

                    this._system.suscribeOPCItem(new StringBuilder("RAMPAS.APAGADO.").Append(this._system.Zonas[i].Nombre).ToString(),
                        "Mode",
                        ccs[i].cannonModeChanged);
                }

                foreach (Zona z in this._system.Zonas)
                {
                    try
                    {
                        this._system.getCannonCoordinates(z);
                    }
                    catch (Exception ex)
                    {
                        addElementToListBox(this.listViewEventos, new Evento(DateTime.Now, "Grave", ex.Message));
                    }

                    this._system.suscribeOPCItem(new StringBuilder("RAMPAS.APAGADO.").Append(z.Nombre).ToString(),
                        "X",
                        new Action<object>(z.coordinateXChanged));
                    this._system.suscribeOPCItem(new StringBuilder("RAMPAS.APAGADO.").Append(z.Nombre).ToString(),
                       "Y",
                       new Action<object>(z.coordinateYChanged));
                    this._system.suscribeOPCItem(new StringBuilder("RAMPAS.APAGADO.").Append(z.Nombre).ToString(),
                       "n",
                       new Action<object>(z.subZonaNChanged));
                    this._system.suscribeOPCItem(new StringBuilder("RAMPAS.APAGADO.").Append(z.Nombre).ToString(),
                       "Valvula",
                       new Action<object>(z.ValvulaStateChanged));
                }

                rcs = new Controls.RejillasControl[this._system.ZonasVaciado.Count];

                for (int i = 0; i < rcs.Length; i++)
                {
                    rcs[i] = new Controls.RejillasControl(
                        this,
                        i,
                        this._system.ZonasVaciado[i],
                        new Point(i * (this.Width / this._system.ZonasVaciado.Count) + (this.Width / this._system.ZonasVaciado.Count) / 2,
                            this.pictureBoxRampa.Location.Y + this.pictureBoxRampa.Height + 110 + 26 + 10));

                }

                foreach (Zona z in this._system.ZonasVaciado)
                {
                    this._system.suscribeOPCItem(new StringBuilder("RAMPAS.VACIADO.").Append(z.Nombre).ToString(),
                        "X",
                        new Action<object>(z.coordinateXVaciadoChanged));
                    this._system.suscribeOPCItem(new StringBuilder("RAMPAS.VACIADO.").Append(z.Nombre).ToString(),
                        "Y",
                        new Action<object>(z.coordinateYVaciadoChanged));
                    this._system.suscribeOPCItem(new StringBuilder("RAMPAS.VACIADO.").Append(z.Nombre).ToString(),
                        "n",
                        new Action<object>(z.subZonaNVaciadoChanged));
                    this._system.suscribeOPCItem(new StringBuilder("RAMPAS.VACIADO.").Append(z.Nombre).ToString(),
                        "Emptying",
                        new Action<object>(z.emptyingStateChanged));
                }
            }
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

            foreach (Zona zVaciado in this._system.ZonasVaciado)
            //Parallel.ForEach<Zona>(this._system.ZonasVaciado, (zVaciado) =>
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

                                if (s.Inicio.Y <= sApagado.Fin.Y && s.Fin.Y >= sApagado.Fin.Y &&
                                    s.Inicio.X <= sApagado.Fin.X && s.Fin.X >= sApagado.Fin.X)
                                {
                                    //PERTENECE
                                    if (!s.Parent.zonasContenidas.Contains(zApagado))
                                    {
                                        lock (_lock)
                                            s.Parent.zonasContenidas.Add(zApagado);
                                    }
                                    if (!zApagado.zonasContenidas.Contains(s.Parent))
                                    {
                                        lock (_lock)
                                            zApagado.zonasContenidas.Add(s.Parent);
                                    }
                                }

                                if (s.Inicio.Y <= sApagado.Inicio.Y && s.Fin.Y >= sApagado.Inicio.Y &&
                                    s.Inicio.X <= sApagado.Inicio.X && s.Fin.X >= sApagado.Inicio.X)
                                {
                                    //PERTENECE
                                    if (!s.Parent.zonasContenidas.Contains(zApagado))
                                    {
                                        lock (_lock)
                                            s.Parent.zonasContenidas.Add(zApagado);
                                    }
                                    if (!zApagado.zonasContenidas.Contains(s.Parent))
                                    {
                                        lock (_lock)
                                            zApagado.zonasContenidas.Add(s.Parent);
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
            this._system.OPCClient.Connected += OPCClient_Connected;
            this._system.OPCClient.DataSent += OPCClient_DataSent;
            this._system.OPCClient.Disconnected += OPCClient_Disconnected;
            this._system.OPCClient.OPCError += OPCClient_OPCError;
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
