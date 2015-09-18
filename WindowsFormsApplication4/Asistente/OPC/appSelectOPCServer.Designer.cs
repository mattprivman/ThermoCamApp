namespace WindowsFormsApplication4.Asistente.OPC
{
    partial class appSelectOPCServer
    {
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
        private void InitializeComponent()
        {
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.comboBoxOPCServers = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.treeViewBranch = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(218, 280);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 0;
            this.buttonNext.Text = "Siguiente >>";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(127, 280);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(75, 23);
            this.buttonBack.TabIndex = 1;
            this.buttonBack.Text = "<< Atrás";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // comboBoxOPCServers
            // 
            this.comboBoxOPCServers.FormattingEnabled = true;
            this.comboBoxOPCServers.Location = new System.Drawing.Point(98, 27);
            this.comboBoxOPCServers.Name = "comboBoxOPCServers";
            this.comboBoxOPCServers.Size = new System.Drawing.Size(195, 21);
            this.comboBoxOPCServers.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Servidor OPC";
            // 
            // treeViewBranch
            // 
            this.treeViewBranch.Location = new System.Drawing.Point(24, 67);
            this.treeViewBranch.Name = "treeViewBranch";
            this.treeViewBranch.Size = new System.Drawing.Size(269, 198);
            this.treeViewBranch.TabIndex = 4;
            // 
            // appSelectOPCServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 315);
            this.ControlBox = false;
            this.Controls.Add(this.treeViewBranch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxOPCServers);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.buttonNext);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "appSelectOPCServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Seleccionar servidor OPC";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.ComboBox comboBoxOPCServers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView treeViewBranch;
    }
}