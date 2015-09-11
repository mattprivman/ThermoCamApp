namespace ThermoVision.CustomControls
{
    partial class CamControl
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.DoubleBuffered = true;
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
            // CamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelCam1TotalTime);
            this.Controls.Add(this.labelCam1TimeProcessing);
            this.Controls.Add(this.labelCam1MinTemp);
            this.Controls.Add(this.labelCam1MaxTemp);
            this.Name = "CamControl";
            this.Size = new System.Drawing.Size(640, 518);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelCam1TotalTime;
        private System.Windows.Forms.Label labelCam1TimeProcessing;
        private System.Windows.Forms.Label labelCam1MinTemp;
        private System.Windows.Forms.Label labelCam1MaxTemp;
    }
}
