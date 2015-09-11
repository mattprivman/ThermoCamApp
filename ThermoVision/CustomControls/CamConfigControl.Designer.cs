namespace ThermoVision.CustomControls
{
    partial class CamConfigControl
    {
        /// <summary> 
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (this.camara != null)
            {
                this.camara.Dispose();
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar 
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelCam1TotalTime = new System.Windows.Forms.Label();
            this.labelCam1TimeProcessing = new System.Windows.Forms.Label();
            this.labelCam1MinTemp = new System.Windows.Forms.Label();
            this.labelCam1MaxTemp = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.textBoxCamName = new System.Windows.Forms.TextBox();
            this.textBoxDireccionIP = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonDesconectar = new System.Windows.Forms.Button();
            this.buttonConectar = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonColSubstract = new System.Windows.Forms.Button();
            this.buttonColAdd = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.buttonFilSubstract = new System.Windows.Forms.Button();
            this.buttonFilAdd = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.buttonYfinSubstract = new System.Windows.Forms.Button();
            this.buttonYfinAdd = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonYInitSubstract = new System.Windows.Forms.Button();
            this.buttonYinitAdd = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonXfinSubstract = new System.Windows.Forms.Button();
            this.buttonXfinAdd = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonXinitSubstract = new System.Windows.Forms.Button();
            this.buttonXinitAdd = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonRemoveZona = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDivName = new System.Windows.Forms.TextBox();
            this.buttonAddZone = new System.Windows.Forms.Button();
            this.listBoxZonas = new System.Windows.Forms.ListBox();
            this.buttonAutoAdjust = new System.Windows.Forms.Button();
            this.buttonInternalImageCorrection = new System.Windows.Forms.Button();
            this.buttonAutoFocus = new System.Windows.Forms.Button();
            this.buttonExternalImageCorrection = new System.Windows.Forms.Button();
            this.numericTextBoxCol = new ThermoVision.CustomControls.NumericTextBox();
            this.numericTextBoxYfin = new ThermoVision.CustomControls.NumericTextBox();
            this.numericTextBoxXfin = new ThermoVision.CustomControls.NumericTextBox();
            this.numericTextBoxFilas = new ThermoVision.CustomControls.NumericTextBox();
            this.numericTextBoxYinit = new ThermoVision.CustomControls.NumericTextBox();
            this.numericTextBoxXinit = new ThermoVision.CustomControls.NumericTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(640, 480);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // labelCam1TotalTime
            // 
            this.labelCam1TotalTime.AutoSize = true;
            this.labelCam1TotalTime.Location = new System.Drawing.Point(463, 492);
            this.labelCam1TotalTime.Name = "labelCam1TotalTime";
            this.labelCam1TotalTime.Size = new System.Drawing.Size(56, 13);
            this.labelCam1TotalTime.TabIndex = 20;
            this.labelCam1TotalTime.Text = "Total time:";
            // 
            // labelCam1TimeProcessing
            // 
            this.labelCam1TimeProcessing.AutoSize = true;
            this.labelCam1TimeProcessing.Location = new System.Drawing.Point(289, 492);
            this.labelCam1TimeProcessing.Name = "labelCam1TimeProcessing";
            this.labelCam1TimeProcessing.Size = new System.Drawing.Size(87, 13);
            this.labelCam1TimeProcessing.TabIndex = 19;
            this.labelCam1TimeProcessing.Text = "Time processing:";
            // 
            // labelCam1MinTemp
            // 
            this.labelCam1MinTemp.AutoSize = true;
            this.labelCam1MinTemp.Location = new System.Drawing.Point(159, 492);
            this.labelCam1MinTemp.Name = "labelCam1MinTemp";
            this.labelCam1MinTemp.Size = new System.Drawing.Size(60, 13);
            this.labelCam1MinTemp.TabIndex = 16;
            this.labelCam1MinTemp.Text = "Min. Temp:";
            // 
            // labelCam1MaxTemp
            // 
            this.labelCam1MaxTemp.AutoSize = true;
            this.labelCam1MaxTemp.Location = new System.Drawing.Point(10, 492);
            this.labelCam1MaxTemp.Name = "labelCam1MaxTemp";
            this.labelCam1MaxTemp.Size = new System.Drawing.Size(63, 13);
            this.labelCam1MaxTemp.TabIndex = 15;
            this.labelCam1MaxTemp.Text = "Max. Temp:";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.groupBox3);
            this.tabPage4.Controls.Add(this.groupBox2);
            this.tabPage4.Controls.Add(this.buttonDesconectar);
            this.tabPage4.Controls.Add(this.buttonConectar);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(632, 200);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Conexión";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // textBoxCamName
            // 
            this.textBoxCamName.Location = new System.Drawing.Point(86, 28);
            this.textBoxCamName.Name = "textBoxCamName";
            this.textBoxCamName.Size = new System.Drawing.Size(129, 20);
            this.textBoxCamName.TabIndex = 50;
            // 
            // textBoxDireccionIP
            // 
            this.textBoxDireccionIP.Location = new System.Drawing.Point(86, 71);
            this.textBoxDireccionIP.Name = "textBoxDireccionIP";
            this.textBoxDireccionIP.Size = new System.Drawing.Size(129, 20);
            this.textBoxDireccionIP.TabIndex = 51;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Nombre:";
            // 
            // buttonDesconectar
            // 
            this.buttonDesconectar.Location = new System.Drawing.Point(541, 171);
            this.buttonDesconectar.Name = "buttonDesconectar";
            this.buttonDesconectar.Size = new System.Drawing.Size(85, 23);
            this.buttonDesconectar.TabIndex = 54;
            this.buttonDesconectar.Text = "Desconectar";
            this.buttonDesconectar.UseVisualStyleBackColor = true;
            // 
            // buttonConectar
            // 
            this.buttonConectar.Location = new System.Drawing.Point(460, 171);
            this.buttonConectar.Name = "buttonConectar";
            this.buttonConectar.Size = new System.Drawing.Size(75, 23);
            this.buttonConectar.TabIndex = 53;
            this.buttonConectar.Text = "Conectar";
            this.buttonConectar.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Dirección IP: ";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(0, 522);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(640, 226);
            this.tabControl1.TabIndex = 41;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.buttonRemoveZona);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.textBoxDivName);
            this.tabPage1.Controls.Add(this.buttonAddZone);
            this.tabPage1.Controls.Add(this.listBoxZonas);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(632, 200);
            this.tabPage1.TabIndex = 4;
            this.tabPage1.Text = "Subzonas";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numericTextBoxCol);
            this.groupBox1.Controls.Add(this.numericTextBoxYfin);
            this.groupBox1.Controls.Add(this.numericTextBoxXfin);
            this.groupBox1.Controls.Add(this.numericTextBoxFilas);
            this.groupBox1.Controls.Add(this.numericTextBoxYinit);
            this.groupBox1.Controls.Add(this.numericTextBoxXinit);
            this.groupBox1.Controls.Add(this.buttonColSubstract);
            this.groupBox1.Controls.Add(this.buttonColAdd);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.buttonFilSubstract);
            this.groupBox1.Controls.Add(this.buttonFilAdd);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.buttonYfinSubstract);
            this.groupBox1.Controls.Add(this.buttonYfinAdd);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.buttonYInitSubstract);
            this.groupBox1.Controls.Add(this.buttonYinitAdd);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.buttonXfinSubstract);
            this.groupBox1.Controls.Add(this.buttonXfinAdd);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.buttonXinitSubstract);
            this.groupBox1.Controls.Add(this.buttonXinitAdd);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(7, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(397, 158);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rejilla";
            // 
            // buttonColSubstract
            // 
            this.buttonColSubstract.Location = new System.Drawing.Point(344, 121);
            this.buttonColSubstract.Name = "buttonColSubstract";
            this.buttonColSubstract.Size = new System.Drawing.Size(41, 23);
            this.buttonColSubstract.TabIndex = 27;
            this.buttonColSubstract.Text = "-";
            this.buttonColSubstract.UseVisualStyleBackColor = true;
            // 
            // buttonColAdd
            // 
            this.buttonColAdd.Location = new System.Drawing.Point(297, 121);
            this.buttonColAdd.Name = "buttonColAdd";
            this.buttonColAdd.Size = new System.Drawing.Size(41, 23);
            this.buttonColAdd.TabIndex = 24;
            this.buttonColAdd.Text = "+";
            this.buttonColAdd.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(271, 98);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(22, 13);
            this.label10.TabIndex = 25;
            this.label10.Text = "Col";
            // 
            // buttonFilSubstract
            // 
            this.buttonFilSubstract.Location = new System.Drawing.Point(344, 54);
            this.buttonFilSubstract.Name = "buttonFilSubstract";
            this.buttonFilSubstract.Size = new System.Drawing.Size(41, 23);
            this.buttonFilSubstract.TabIndex = 23;
            this.buttonFilSubstract.Text = "-";
            this.buttonFilSubstract.UseVisualStyleBackColor = true;
            // 
            // buttonFilAdd
            // 
            this.buttonFilAdd.Location = new System.Drawing.Point(297, 54);
            this.buttonFilAdd.Name = "buttonFilAdd";
            this.buttonFilAdd.Size = new System.Drawing.Size(41, 23);
            this.buttonFilAdd.TabIndex = 20;
            this.buttonFilAdd.Text = "+";
            this.buttonFilAdd.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(271, 31);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(28, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Filas";
            // 
            // buttonYfinSubstract
            // 
            this.buttonYfinSubstract.Location = new System.Drawing.Point(208, 121);
            this.buttonYfinSubstract.Name = "buttonYfinSubstract";
            this.buttonYfinSubstract.Size = new System.Drawing.Size(41, 23);
            this.buttonYfinSubstract.TabIndex = 19;
            this.buttonYfinSubstract.Text = "-";
            this.buttonYfinSubstract.UseVisualStyleBackColor = true;
            // 
            // buttonYfinAdd
            // 
            this.buttonYfinAdd.Location = new System.Drawing.Point(161, 121);
            this.buttonYfinAdd.Name = "buttonYfinAdd";
            this.buttonYfinAdd.Size = new System.Drawing.Size(41, 23);
            this.buttonYfinAdd.TabIndex = 16;
            this.buttonYfinAdd.Text = "+";
            this.buttonYfinAdd.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(135, 98);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(28, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Y.fin";
            // 
            // buttonYInitSubstract
            // 
            this.buttonYInitSubstract.Location = new System.Drawing.Point(208, 54);
            this.buttonYInitSubstract.Name = "buttonYInitSubstract";
            this.buttonYInitSubstract.Size = new System.Drawing.Size(41, 23);
            this.buttonYInitSubstract.TabIndex = 15;
            this.buttonYInitSubstract.Text = "-";
            this.buttonYInitSubstract.UseVisualStyleBackColor = true;
            // 
            // buttonYinitAdd
            // 
            this.buttonYinitAdd.Location = new System.Drawing.Point(161, 54);
            this.buttonYinitAdd.Name = "buttonYinitAdd";
            this.buttonYinitAdd.Size = new System.Drawing.Size(41, 23);
            this.buttonYinitAdd.TabIndex = 12;
            this.buttonYinitAdd.Text = "+";
            this.buttonYinitAdd.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(135, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Y.Init";
            // 
            // buttonXfinSubstract
            // 
            this.buttonXfinSubstract.Location = new System.Drawing.Point(80, 121);
            this.buttonXfinSubstract.Name = "buttonXfinSubstract";
            this.buttonXfinSubstract.Size = new System.Drawing.Size(41, 23);
            this.buttonXfinSubstract.TabIndex = 11;
            this.buttonXfinSubstract.Text = "-";
            this.buttonXfinSubstract.UseVisualStyleBackColor = true;
            // 
            // buttonXfinAdd
            // 
            this.buttonXfinAdd.Location = new System.Drawing.Point(33, 121);
            this.buttonXfinAdd.Name = "buttonXfinAdd";
            this.buttonXfinAdd.Size = new System.Drawing.Size(41, 23);
            this.buttonXfinAdd.TabIndex = 8;
            this.buttonXfinAdd.Text = "+";
            this.buttonXfinAdd.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 98);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "X.fin";
            // 
            // buttonXinitSubstract
            // 
            this.buttonXinitSubstract.Location = new System.Drawing.Point(80, 54);
            this.buttonXinitSubstract.Name = "buttonXinitSubstract";
            this.buttonXinitSubstract.Size = new System.Drawing.Size(41, 23);
            this.buttonXinitSubstract.TabIndex = 7;
            this.buttonXinitSubstract.Text = "-";
            this.buttonXinitSubstract.UseVisualStyleBackColor = true;
            // 
            // buttonXinitAdd
            // 
            this.buttonXinitAdd.Location = new System.Drawing.Point(33, 54);
            this.buttonXinitAdd.Name = "buttonXinitAdd";
            this.buttonXinitAdd.Size = new System.Drawing.Size(41, 23);
            this.buttonXinitAdd.TabIndex = 6;
            this.buttonXinitAdd.Text = "+";
            this.buttonXinitAdd.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "X.init";
            // 
            // buttonRemoveZona
            // 
            this.buttonRemoveZona.Location = new System.Drawing.Point(288, 7);
            this.buttonRemoveZona.Name = "buttonRemoveZona";
            this.buttonRemoveZona.Size = new System.Drawing.Size(98, 23);
            this.buttonRemoveZona.TabIndex = 4;
            this.buttonRemoveZona.Text = "Borrar subzona";
            this.buttonRemoveZona.UseVisualStyleBackColor = true;
            this.buttonRemoveZona.Click += new System.EventHandler(this.buttonRemoveZona_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Nombre";
            // 
            // textBoxDivName
            // 
            this.textBoxDivName.Location = new System.Drawing.Point(56, 9);
            this.textBoxDivName.Name = "textBoxDivName";
            this.textBoxDivName.Size = new System.Drawing.Size(100, 20);
            this.textBoxDivName.TabIndex = 2;
            // 
            // buttonAddZone
            // 
            this.buttonAddZone.Location = new System.Drawing.Point(180, 7);
            this.buttonAddZone.Name = "buttonAddZone";
            this.buttonAddZone.Size = new System.Drawing.Size(98, 23);
            this.buttonAddZone.TabIndex = 1;
            this.buttonAddZone.Text = "Agregar subzona";
            this.buttonAddZone.UseVisualStyleBackColor = true;
            this.buttonAddZone.Click += new System.EventHandler(this.buttonAddZone_Click);
            // 
            // listBoxZonas
            // 
            this.listBoxZonas.FormattingEnabled = true;
            this.listBoxZonas.Location = new System.Drawing.Point(419, 3);
            this.listBoxZonas.Name = "listBoxZonas";
            this.listBoxZonas.Size = new System.Drawing.Size(207, 186);
            this.listBoxZonas.TabIndex = 0;
            // 
            // buttonAutoAdjust
            // 
            this.buttonAutoAdjust.Location = new System.Drawing.Point(21, 26);
            this.buttonAutoAdjust.Name = "buttonAutoAdjust";
            this.buttonAutoAdjust.Size = new System.Drawing.Size(75, 23);
            this.buttonAutoAdjust.TabIndex = 56;
            this.buttonAutoAdjust.Text = "Auto adjust";
            this.buttonAutoAdjust.UseVisualStyleBackColor = true;
            this.buttonAutoAdjust.Click += new System.EventHandler(this.buttonAutoAdjust_Click);
            // 
            // buttonInternalImageCorrection
            // 
            this.buttonInternalImageCorrection.Location = new System.Drawing.Point(142, 26);
            this.buttonInternalImageCorrection.Name = "buttonInternalImageCorrection";
            this.buttonInternalImageCorrection.Size = new System.Drawing.Size(152, 23);
            this.buttonInternalImageCorrection.TabIndex = 57;
            this.buttonInternalImageCorrection.Text = "Internal image correction";
            this.buttonInternalImageCorrection.UseVisualStyleBackColor = true;
            this.buttonInternalImageCorrection.Click += new System.EventHandler(this.buttonInternalImageCorrection_Click);
            // 
            // buttonAutoFocus
            // 
            this.buttonAutoFocus.Location = new System.Drawing.Point(21, 69);
            this.buttonAutoFocus.Name = "buttonAutoFocus";
            this.buttonAutoFocus.Size = new System.Drawing.Size(75, 23);
            this.buttonAutoFocus.TabIndex = 58;
            this.buttonAutoFocus.Text = "Auto focus";
            this.buttonAutoFocus.UseVisualStyleBackColor = true;
            this.buttonAutoFocus.Click += new System.EventHandler(this.buttonAutoFocus_Click);
            // 
            // buttonExternalImageCorrection
            // 
            this.buttonExternalImageCorrection.Location = new System.Drawing.Point(142, 69);
            this.buttonExternalImageCorrection.Name = "buttonExternalImageCorrection";
            this.buttonExternalImageCorrection.Size = new System.Drawing.Size(152, 23);
            this.buttonExternalImageCorrection.TabIndex = 59;
            this.buttonExternalImageCorrection.Text = "External image correction";
            this.buttonExternalImageCorrection.UseVisualStyleBackColor = true;
            this.buttonExternalImageCorrection.Click += new System.EventHandler(this.buttonExternalImageCorrection_Click);
            // 
            // numericTextBoxCol
            // 
            this.numericTextBoxCol.Location = new System.Drawing.Point(305, 94);
            this.numericTextBoxCol.Name = "numericTextBoxCol";
            this.numericTextBoxCol.Size = new System.Drawing.Size(64, 23);
            this.numericTextBoxCol.TabIndex = 33;
            this.numericTextBoxCol.Texto = "0";
            // 
            // numericTextBoxYfin
            // 
            this.numericTextBoxYfin.Location = new System.Drawing.Point(169, 94);
            this.numericTextBoxYfin.Name = "numericTextBoxYfin";
            this.numericTextBoxYfin.Size = new System.Drawing.Size(64, 23);
            this.numericTextBoxYfin.TabIndex = 32;
            this.numericTextBoxYfin.Texto = "0";
            // 
            // numericTextBoxXfin
            // 
            this.numericTextBoxXfin.Location = new System.Drawing.Point(41, 94);
            this.numericTextBoxXfin.Name = "numericTextBoxXfin";
            this.numericTextBoxXfin.Size = new System.Drawing.Size(64, 23);
            this.numericTextBoxXfin.TabIndex = 31;
            this.numericTextBoxXfin.Texto = "0";
            // 
            // numericTextBoxFilas
            // 
            this.numericTextBoxFilas.Location = new System.Drawing.Point(305, 27);
            this.numericTextBoxFilas.Name = "numericTextBoxFilas";
            this.numericTextBoxFilas.Size = new System.Drawing.Size(64, 23);
            this.numericTextBoxFilas.TabIndex = 30;
            this.numericTextBoxFilas.Texto = "0";
            // 
            // numericTextBoxYinit
            // 
            this.numericTextBoxYinit.Location = new System.Drawing.Point(173, 27);
            this.numericTextBoxYinit.Name = "numericTextBoxYinit";
            this.numericTextBoxYinit.Size = new System.Drawing.Size(64, 23);
            this.numericTextBoxYinit.TabIndex = 29;
            this.numericTextBoxYinit.Texto = "0";
            // 
            // numericTextBoxXinit
            // 
            this.numericTextBoxXinit.Location = new System.Drawing.Point(42, 27);
            this.numericTextBoxXinit.Name = "numericTextBoxXinit";
            this.numericTextBoxXinit.Size = new System.Drawing.Size(64, 23);
            this.numericTextBoxXinit.TabIndex = 28;
            this.numericTextBoxXinit.Texto = "0";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonAutoFocus);
            this.groupBox2.Controls.Add(this.buttonExternalImageCorrection);
            this.groupBox2.Controls.Add(this.buttonAutoAdjust);
            this.groupBox2.Controls.Add(this.buttonInternalImageCorrection);
            this.groupBox2.Location = new System.Drawing.Point(278, 33);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(310, 109);
            this.groupBox2.TabIndex = 60;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Image Settings";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBoxCamName);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.textBoxDireccionIP);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(20, 33);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(233, 110);
            this.groupBox3.TabIndex = 42;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Parametros";
            // 
            // CamConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.labelCam1TotalTime);
            this.Controls.Add(this.labelCam1TimeProcessing);
            this.Controls.Add(this.labelCam1MinTemp);
            this.Controls.Add(this.labelCam1MaxTemp);
            this.Controls.Add(this.pictureBox1);
            this.DoubleBuffered = true;
            this.Name = "CamConfigControl";
            this.Size = new System.Drawing.Size(640, 751);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelCam1TotalTime;
        private System.Windows.Forms.Label labelCam1TimeProcessing;
        private System.Windows.Forms.Label labelCam1MinTemp;
        private System.Windows.Forms.Label labelCam1MaxTemp;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox textBoxCamName;
        private System.Windows.Forms.TextBox textBoxDireccionIP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonDesconectar;
        private System.Windows.Forms.Button buttonConectar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListBox listBoxZonas;
        private System.Windows.Forms.Button buttonAddZone;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDivName;
        private System.Windows.Forms.Button buttonRemoveZona;
        private System.Windows.Forms.GroupBox groupBox1;
        private CustomControls.NumericTextBox numericTextBoxCol;
        private CustomControls.NumericTextBox numericTextBoxYfin;
        private CustomControls.NumericTextBox numericTextBoxXfin;
        private CustomControls.NumericTextBox numericTextBoxFilas;
        private CustomControls.NumericTextBox numericTextBoxYinit;
        private CustomControls.NumericTextBox numericTextBoxXinit;
        private System.Windows.Forms.Button buttonColSubstract;
        private System.Windows.Forms.Button buttonColAdd;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button buttonFilSubstract;
        private System.Windows.Forms.Button buttonFilAdd;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button buttonYfinSubstract;
        private System.Windows.Forms.Button buttonYfinAdd;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button buttonYInitSubstract;
        private System.Windows.Forms.Button buttonYinitAdd;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonXfinSubstract;
        private System.Windows.Forms.Button buttonXfinAdd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonXinitSubstract;
        private System.Windows.Forms.Button buttonXinitAdd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonExternalImageCorrection;
        private System.Windows.Forms.Button buttonAutoFocus;
        private System.Windows.Forms.Button buttonInternalImageCorrection;
        private System.Windows.Forms.Button buttonAutoAdjust;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}
