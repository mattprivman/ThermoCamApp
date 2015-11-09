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

        public CheckBox buttonMode;
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

        public cannonControlBoolCallback CheckCannonMode;

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
            cannonControlParameterCallback      buttonDownAction,
            cannonControlBoolCallback           checkCannonMode)
        {
            this.posicion   = pos;
            this.zona       = z;

            this.buttonModeAction       = buttonModeAction;
            this.buttonActivarAction    = buttonActivarAction;
            this.buttonUpAction         = buttonUpAction;
            this.buttonLeftAction       = buttonLeftAction;
            this.buttonRigthAction      = buttonRigthAction;
            this.buttonDownAction       = buttonDownAction;
            this.CheckCannonMode        = checkCannonMode;

            this.buttonMode     = new CheckBox();
            this.buttonUp       = new Button();
            this.buttonLeft     = new Button();
            this.buttonActivar  = new Button();
            this.buttonRigth    = new Button();
            this.buttonDown     = new Button();

            this.labelEstado = new Label();

            #region "TOP Buttons"
            // 
            // buttonMode
            // 
            this.buttonMode.Appearance = Appearance.Button;
            this.buttonMode.Location = new System.Drawing.Point(p.X - 100 - 10 - 100, p.Y);
            this.buttonMode.Name = "buttonMode";
            this.buttonMode.Size = new System.Drawing.Size(100, 30);
            this.buttonMode.TabIndex = 5;
            this.buttonMode.Text = "Automatico";
            this.buttonMode.TextAlign = ContentAlignment.MiddleCenter;
            this.buttonMode.UseVisualStyleBackColor = true;
            this.buttonMode.CheckedChanged += buttonMode_CheckedChanged;
            this.buttonMode.Checked = this.CheckCannonMode(this.posicion);
            // 
            // buttonUp
            // 
            this.buttonUp.Location = new System.Drawing.Point(p.X - 50, p.Y);
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
            this.labelEstado.Location = new System.Drawing.Point(p.X + 100 + 10, p.Y);
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
            this.buttonLeft.Location = new System.Drawing.Point(p.X - 50 - 10 - 100, p.Y + 35);
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
            this.buttonActivar.Location = new System.Drawing.Point(p.X - 50, p.Y + 35);
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
            this.buttonRigth.Location = new System.Drawing.Point(p.X + 50 + 10, p.Y + 35);
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
            this.buttonDown.Location = new System.Drawing.Point(p.X - 50, p.Y + 70);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(100, 30);
            this.buttonDown.TabIndex = 5;
            this.buttonDown.Text = "▼";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += buttonDown_Click;

            #endregion

            f.Controls.Add(this.buttonMode);
            f.Controls.Add(this.buttonUp);
            f.Controls.Add(this.labelEstado);
            f.Controls.Add(this.buttonLeft);
            f.Controls.Add(this.buttonActivar);
            f.Controls.Add(this.buttonRigth);
            f.Controls.Add(this.buttonDown);

            changeButtonsState(this.buttonMode.Checked);
        }

        public void cannonModeChanged(Object value)
        {
            if (value is bool)
            {
                this.buttonMode.Checked = (bool)value;
                changeButtonsState(this.buttonMode.Checked);
                if((bool) value)
                    this.zona.triggerStateChangedEvent(ThermoVision.Models.Zona.States.Manual);
                else
                    this.zona.triggerStateChangedEvent(ThermoVision.Models.Zona.States.Vacio);
            }
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

            this.buttonUp.Visible = state;
            this.buttonLeft.Visible = state;
            this.buttonActivar.Visible = state;
            this.buttonRigth.Visible = state;
            this.buttonDown.Visible = state;
        }
    }
}
