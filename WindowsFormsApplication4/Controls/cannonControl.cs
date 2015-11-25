using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ThermoCamApp.Controls
{
    class cannonControl
    {
        public int posicion;
        public ThermoVision.Models.Zona zona;
        public ThermoVision.Models.Zona.States LastState;

        public GroupBox groupBox;

        public CheckBox buttonMode;
        public Button buttonReset;
        public Button buttonUp;
        public Button buttonLeft;
        public Button buttonActivar;
        public Button buttonRigth;
        public Button buttonDown;

        public Label labelEstado;

        public delegate void cannonControlBoolParameterCallback(int CannonNumber, bool arg);
        public delegate void cannonControlParameterCallback(int CannonNumber);
        public delegate bool cannonControlBoolCallback(int CannonNumber);

        public cannonControlBoolParameterCallback buttonModeAction;
        public cannonControlBoolParameterCallback buttonActivarAction;
        public cannonControlParameterCallback buttonUpAction;
        public cannonControlParameterCallback buttonLeftAction;
        public cannonControlParameterCallback buttonRigthAction;
        public cannonControlParameterCallback buttonDownAction;

        //public cannonControlBoolCallback CheckCannonMode;

        public cannonControl(
            Form f,
            ThermoVision.Models.Zona z,
            Point p, 
            int pos,
            cannonControlBoolParameterCallback  buttonModeAction,
            cannonControlBoolParameterCallback  buttonActivarAction,
            cannonControlParameterCallback      buttonUpAction,
            cannonControlParameterCallback      buttonLeftAction,
            cannonControlParameterCallback      buttonRigthAction,
            cannonControlParameterCallback      buttonDownAction)
        {
            this.posicion   = pos;
            this.zona       = z;

            this.buttonModeAction       = buttonModeAction;
            this.buttonActivarAction    = buttonActivarAction;
            this.buttonUpAction         = buttonUpAction;
            this.buttonLeftAction       = buttonLeftAction;
            this.buttonRigthAction      = buttonRigthAction;
            this.buttonDownAction       = buttonDownAction;

            this.buttonMode     = new CheckBox();
            this.buttonReset    = new Button();
            this.buttonUp       = new Button();
            this.buttonLeft     = new Button();
            this.buttonActivar  = new Button();
            this.buttonRigth    = new Button();
            this.buttonDown     = new Button();

            this.labelEstado = new Label();

            #region "TOP Buttons"
            //
            // groupBox
            //
            this.groupBox = new GroupBox();
            
            this.groupBox.Location = new System.Drawing.Point(p.X - 100 - 50 - 20, p.Y);
            this.groupBox.Name = "groupBox2";
            this.groupBox.Size = new System.Drawing.Size(100 * 3 + 10 * 4, 
                                                            5 * 5 + 3 * 30);
            this.groupBox.TabIndex = 61;
            this.groupBox.TabStop = false;
            // 
            // buttonMode
            // 
            this.buttonMode.Appearance = Appearance.Button;
            this.buttonMode.Location = new System.Drawing.Point(this.groupBox.Location.X - 100 - 10, p.Y);
            this.buttonMode.Name = "buttonMode";
            this.buttonMode.Size = new System.Drawing.Size(100, 30);
            this.buttonMode.TabIndex = 5;
            this.buttonMode.Text = "Automatico";
            this.buttonMode.TextAlign = ContentAlignment.MiddleCenter;
            this.buttonMode.UseVisualStyleBackColor = true;
            this.buttonMode.CheckedChanged += buttonMode_CheckedChanged;
            //this.buttonMode.Checked = this.CheckCannonMode(this.posicion);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(this.buttonMode.Location.X - 100 - 15, p.Y);
            this.buttonReset.Name = "buttonUp";
            this.buttonReset.Size = new System.Drawing.Size(100, 30);
            this.buttonReset.TabIndex = 5;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += buttonReset_Click;
            // 
            // buttonUp
            // 
            this.buttonUp.Location = new System.Drawing.Point(this.groupBox.Width / 2 - 50, 10);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(100, 30);
            this.buttonUp.TabIndex = 5;
            this.buttonUp.Text = "▲";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += buttonUp_Click;
            // 
            // Estado
            // 
            this.labelEstado.AutoSize = true;
            this.labelEstado.Location = new System.Drawing.Point(p.X + 100 + 30 + 50, p.Y);
            this.labelEstado.AutoSize = false;
            this.labelEstado.Name = "label";
            this.labelEstado.Size = new System.Drawing.Size(100, 30);
            this.labelEstado.TabIndex = 4;
            this.labelEstado.Text = "Vacio";
            this.labelEstado.Font = new Font(
                                           new FontFamily("Microsoft Sans Serif"),
                                           14 -1,
                                           FontStyle.Regular,
                                           GraphicsUnit.Pixel);
            this.labelEstado.TextAlign = ContentAlignment.MiddleCenter;
            this.labelEstado.BorderStyle = BorderStyle.FixedSingle;
            #endregion

            #region "MID Buttons"
            // 
            // buttonLeft
            // 
            this.buttonLeft.Location = new System.Drawing.Point(this.groupBox.Width / 2 - 50 - 10 - 100, 10 + 35);
            this.buttonLeft.Name = "buttonLeft";
            this.buttonLeft.Size = new System.Drawing.Size(100, 30);
            this.buttonLeft.TabIndex = 5;
            this.buttonLeft.Text = "◄";
            this.buttonLeft.Font = new Font(
                                           new FontFamily("Microsoft Sans Serif"),
                                           27,
                                           FontStyle.Regular,
                                           GraphicsUnit.Pixel);
            this.buttonLeft.UseVisualStyleBackColor = true;
            this.buttonLeft.Click += buttonLeft_Click;
            // 
            // buttonActivar
            // 
            this.buttonActivar.Location = new System.Drawing.Point(this.groupBox.Width / 2 - 50, 10 + 35);
            this.buttonActivar.Name = "buttonActivar";
            this.buttonActivar.Size = new System.Drawing.Size(100, 30);
            this.buttonActivar.TabIndex = 5;
            this.buttonActivar.Text = "Activar";
            this.buttonActivar.UseVisualStyleBackColor = true;
            this.buttonActivar.MouseDown += buttonActivar_MouseDown;
            this.buttonActivar.MouseUp += buttonActivar_MouseUp;
            // 
            // buttonRight
            // 
            this.buttonRigth.Location = new System.Drawing.Point(this.groupBox.Width / 2 + 50 + 10, 10 + 35);
            this.buttonRigth.Name = "buttonRigth";
            this.buttonRigth.Size = new System.Drawing.Size(100, 30);
            this.buttonRigth.TabIndex = 5;
            this.buttonRigth.Text = "►";
            this.buttonRigth.Font = new Font(
                                           new FontFamily("Microsoft Sans Serif"),
                                           27,
                                           FontStyle.Regular,
                                           GraphicsUnit.Pixel);
            this.buttonRigth.UseVisualStyleBackColor = true;
            this.buttonRigth.Click += buttonRigth_Click;

            #endregion

            #region "BOTTOM BUTTONS"

            // 
            // buttonDown
            // 
            this.buttonDown.Location = new System.Drawing.Point(this.groupBox.Width / 2 - 50, 10 + 70);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(100, 30);
            this.buttonDown.TabIndex = 5;
            this.buttonDown.Text = "▼";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += buttonDown_Click;

            #endregion

            f.Controls.Add(this.buttonMode);
            f.Controls.Add(this.buttonReset);
            this.groupBox.Controls.Add(this.buttonUp);
            f.Controls.Add(this.labelEstado);
            this.groupBox.Controls.Add(this.buttonLeft);
            this.groupBox.Controls.Add(this.buttonActivar);
            this.groupBox.Controls.Add(this.buttonRigth);
            this.groupBox.Controls.Add(this.buttonDown);
            f.Controls.Add(this.groupBox);

            changeButtonsState(this.buttonMode.Checked);
        }

        void buttonReset_Click(object sender, EventArgs e)
        {
            this.LastState = ThermoVision.Models.Zona.States.Vacio;
        }

        public void cannonModeChanged(Object value)                  
        {
            if (value is bool)
            {
                this.buttonMode.CheckedChanged -= buttonMode_CheckedChanged;

                this.buttonMode.Checked = (bool)value;

                changeButtonsState(this.buttonMode.Checked);
                if ((bool)value)
                {
                    if(this.zona.State != ThermoVision.Models.Zona.States.Manual)
                        this.LastState = this.zona.State;
                    this.zona.ChangeState(ThermoVision.Models.Zona.States.Manual);
                }
                else
                {
                    if (this.LastState == ThermoVision.Models.Zona.States.Enfriando)
                        this.LastState = ThermoVision.Models.Zona.States.Esperando;
                      this.zona.ChangeState(this.LastState);
                }

                this.buttonMode.CheckedChanged += buttonMode_CheckedChanged;
            } //if
        }

        void buttonDown_Click(object sender, EventArgs e)            
        {
            if (this.buttonDownAction != null)
                this.buttonDownAction(this.posicion);
        }
        void buttonRigth_Click(object sender, EventArgs e)           
        {
            if (this.buttonRigthAction != null)
                this.buttonRigthAction(this.posicion);
        }
        void buttonActivar_MouseUp(object sender, MouseEventArgs e)  
        {
            if (this.buttonActivarAction != null)
                this.buttonActivarAction(this.posicion, false);
        }
        void buttonActivar_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.buttonActivarAction != null)
                this.buttonActivarAction(this.posicion, true);
        }
        void buttonLeft_Click(object sender, EventArgs e)            
        {
            if (this.buttonLeftAction != null)
                this.buttonLeftAction(this.posicion);
        }
        void buttonUp_Click(object sender, EventArgs e)              
        {
            if (this.buttonUpAction != null)
                this.buttonUpAction(this.posicion);
        }
        void buttonMode_CheckedChanged(object sender, EventArgs e)   
        {
            if (sender is CheckBox)
            {
                //Mandar señal al cañon
                if (this.buttonModeAction != null)
                    this.buttonModeAction(this.posicion, (sender as CheckBox).Checked);
            }
        }

        void changeButtonsState(bool state)                          
        {
            if (state)
                this.buttonMode.Text = "Manual";
            else
                this.buttonMode.Text = "Automatico";

            this.buttonReset.Enabled    = state;
            this.groupBox.Visible       = state;
            this.groupBox.BringToFront();
            this.buttonUp.Visible       = state;
            this.buttonUp.BringToFront();
            this.buttonLeft.Visible     = state;
            this.buttonLeft.BringToFront();
            this.buttonActivar.Visible  = state;
            this.buttonActivar.BringToFront();
            this.buttonRigth.Visible    = state;
            this.buttonRigth.BringToFront();
            this.buttonDown.Visible     = state;
            this.buttonDown.BringToFront();
        }
    }
}
