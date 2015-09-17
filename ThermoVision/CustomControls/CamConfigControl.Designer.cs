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
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.buttonReloadCalibration = new System.Windows.Forms.Button();
            this.buttonAutoFocus = new System.Windows.Forms.Button();
            this.buttonExternalImageCorrection = new System.Windows.Forms.Button();
            this.buttonAutoAdjust = new System.Windows.Forms.Button();
            this.buttonInternalImageCorrection = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelConexionStatusColor = new System.Windows.Forms.Label();
            this.labelConectionStatusString = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxCamName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxDireccionIP = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonDesconectar = new System.Windows.Forms.Button();
            this.buttonConectar = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.label2 = new System.Windows.Forms.Label();
            this.labelFrameRate = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelTimeOut = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabControl1.SuspendLayout();
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
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(632, 187);
            this.tabPage2.TabIndex = 5;
            this.tabPage2.Text = "Configuración";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.buttonReloadCalibration);
            this.groupBox2.Controls.Add(this.buttonAutoFocus);
            this.groupBox2.Controls.Add(this.buttonExternalImageCorrection);
            this.groupBox2.Controls.Add(this.buttonAutoAdjust);
            this.groupBox2.Controls.Add(this.buttonInternalImageCorrection);
            this.groupBox2.Location = new System.Drawing.Point(25, 23);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(553, 145);
            this.groupBox2.TabIndex = 61;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Image Settings";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(35, 116);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(55, 13);
            this.label15.TabIndex = 65;
            this.label15.Text = "Recalibrar";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(35, 74);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(44, 13);
            this.label14.TabIndex = 64;
            this.label14.Text = "Enfocar";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(35, 31);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(63, 13);
            this.label13.TabIndex = 63;
            this.label13.Text = "Auto ajustar";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(234, 92);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(147, 13);
            this.label12.TabIndex = 62;
            this.label12.Text = "Corregir imagen externamente";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(234, 49);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(144, 13);
            this.label11.TabIndex = 61;
            this.label11.Text = "Corregir imagen internamente";
            // 
            // buttonReloadCalibration
            // 
            this.buttonReloadCalibration.Location = new System.Drawing.Point(113, 111);
            this.buttonReloadCalibration.Name = "buttonReloadCalibration";
            this.buttonReloadCalibration.Size = new System.Drawing.Size(106, 23);
            this.buttonReloadCalibration.TabIndex = 60;
            this.buttonReloadCalibration.Text = "Reload calibration";
            this.buttonReloadCalibration.UseVisualStyleBackColor = true;
            // 
            // buttonAutoFocus
            // 
            this.buttonAutoFocus.Location = new System.Drawing.Point(113, 69);
            this.buttonAutoFocus.Name = "buttonAutoFocus";
            this.buttonAutoFocus.Size = new System.Drawing.Size(106, 23);
            this.buttonAutoFocus.TabIndex = 58;
            this.buttonAutoFocus.Text = "Auto focus";
            this.buttonAutoFocus.UseVisualStyleBackColor = true;
            // 
            // buttonExternalImageCorrection
            // 
            this.buttonExternalImageCorrection.Location = new System.Drawing.Point(384, 87);
            this.buttonExternalImageCorrection.Name = "buttonExternalImageCorrection";
            this.buttonExternalImageCorrection.Size = new System.Drawing.Size(152, 23);
            this.buttonExternalImageCorrection.TabIndex = 59;
            this.buttonExternalImageCorrection.Text = "External image correction";
            this.buttonExternalImageCorrection.UseVisualStyleBackColor = true;
            // 
            // buttonAutoAdjust
            // 
            this.buttonAutoAdjust.Location = new System.Drawing.Point(113, 26);
            this.buttonAutoAdjust.Name = "buttonAutoAdjust";
            this.buttonAutoAdjust.Size = new System.Drawing.Size(106, 23);
            this.buttonAutoAdjust.TabIndex = 56;
            this.buttonAutoAdjust.Text = "Auto adjust";
            this.buttonAutoAdjust.UseVisualStyleBackColor = true;
            // 
            // buttonInternalImageCorrection
            // 
            this.buttonInternalImageCorrection.Location = new System.Drawing.Point(384, 44);
            this.buttonInternalImageCorrection.Name = "buttonInternalImageCorrection";
            this.buttonInternalImageCorrection.Size = new System.Drawing.Size(152, 23);
            this.buttonInternalImageCorrection.TabIndex = 57;
            this.buttonInternalImageCorrection.Text = "Internal image correction";
            this.buttonInternalImageCorrection.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.groupBox3);
            this.tabPage4.Controls.Add(this.buttonDesconectar);
            this.tabPage4.Controls.Add(this.buttonConectar);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(632, 187);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Conexión";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.labelTimeOut);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.labelFrameRate);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.labelConexionStatusColor);
            this.groupBox3.Controls.Add(this.labelConectionStatusString);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.textBoxCamName);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.textBoxDireccionIP);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(19, 21);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(568, 118);
            this.groupBox3.TabIndex = 42;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Parametros";
            // 
            // labelConexionStatusColor
            // 
            this.labelConexionStatusColor.BackColor = System.Drawing.Color.Red;
            this.labelConexionStatusColor.Location = new System.Drawing.Point(393, 31);
            this.labelConexionStatusColor.Name = "labelConexionStatusColor";
            this.labelConexionStatusColor.Size = new System.Drawing.Size(13, 16);
            this.labelConexionStatusColor.TabIndex = 54;
            // 
            // labelConectionStatusString
            // 
            this.labelConectionStatusString.AutoSize = true;
            this.labelConectionStatusString.Location = new System.Drawing.Point(412, 34);
            this.labelConectionStatusString.Name = "labelConectionStatusString";
            this.labelConectionStatusString.Size = new System.Drawing.Size(75, 13);
            this.labelConectionStatusString.TabIndex = 53;
            this.labelConectionStatusString.Text = "No conectado";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(347, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 52;
            this.label1.Text = "Status:";
            // 
            // textBoxCamName
            // 
            this.textBoxCamName.Location = new System.Drawing.Point(124, 31);
            this.textBoxCamName.Name = "textBoxCamName";
            this.textBoxCamName.Size = new System.Drawing.Size(129, 20);
            this.textBoxCamName.TabIndex = 50;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(47, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Dirección IP: ";
            // 
            // textBoxDireccionIP
            // 
            this.textBoxDireccionIP.Location = new System.Drawing.Point(124, 76);
            this.textBoxDireccionIP.Name = "textBoxDireccionIP";
            this.textBoxDireccionIP.Size = new System.Drawing.Size(129, 20);
            this.textBoxDireccionIP.TabIndex = 51;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(47, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Nombre:";
            // 
            // buttonDesconectar
            // 
            this.buttonDesconectar.Location = new System.Drawing.Point(483, 145);
            this.buttonDesconectar.Name = "buttonDesconectar";
            this.buttonDesconectar.Size = new System.Drawing.Size(85, 23);
            this.buttonDesconectar.TabIndex = 54;
            this.buttonDesconectar.Text = "Desconectar";
            this.buttonDesconectar.UseVisualStyleBackColor = true;
            // 
            // buttonConectar
            // 
            this.buttonConectar.Location = new System.Drawing.Point(389, 145);
            this.buttonConectar.Name = "buttonConectar";
            this.buttonConectar.Size = new System.Drawing.Size(75, 23);
            this.buttonConectar.TabIndex = 53;
            this.buttonConectar.Text = "Conectar";
            this.buttonConectar.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 483);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(640, 213);
            this.tabControl1.TabIndex = 41;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(281, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 55;
            this.label2.Text = "Frame rate:";
            // 
            // labelFrameRate
            // 
            this.labelFrameRate.AutoSize = true;
            this.labelFrameRate.Location = new System.Drawing.Point(358, 79);
            this.labelFrameRate.Name = "labelFrameRate";
            this.labelFrameRate.Size = new System.Drawing.Size(0, 13);
            this.labelFrameRate.TabIndex = 56;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(412, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 57;
            this.label6.Text = "Time out:";
            // 
            // labelTimeOut
            // 
            this.labelTimeOut.AutoSize = true;
            this.labelTimeOut.Location = new System.Drawing.Point(469, 79);
            this.labelTimeOut.Name = "labelTimeOut";
            this.labelTimeOut.Size = new System.Drawing.Size(0, 13);
            this.labelTimeOut.TabIndex = 58;
            // 
            // CamConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pictureBox1);
            this.DoubleBuffered = true;
            this.Name = "CamConfigControl";
            this.Size = new System.Drawing.Size(640, 700);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button buttonReloadCalibration;
        private System.Windows.Forms.Button buttonAutoFocus;
        private System.Windows.Forms.Button buttonExternalImageCorrection;
        private System.Windows.Forms.Button buttonAutoAdjust;
        private System.Windows.Forms.Button buttonInternalImageCorrection;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label labelConexionStatusColor;
        private System.Windows.Forms.Label labelConectionStatusString;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxCamName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxDireccionIP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonDesconectar;
        private System.Windows.Forms.Button buttonConectar;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Label labelTimeOut;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelFrameRate;
        private System.Windows.Forms.Label label2;
    }
}
