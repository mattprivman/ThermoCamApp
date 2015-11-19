using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using ThermoVision.Models;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThermoCamApp.Forms
{
    public partial class RampaConfig : Form
    {
        public Sistema _system
        {
            get;
            set;
        }

        Action<object> TempEnfriarReadedAction;
        Action<object> CoolingTimeChangedAction;
        Action<object> CoolingStartTimeChangedAction;
        Action<object> EmptyingTimeChangedAction;

        public RampaConfig(Sistema _system)
        {
            this._system = _system;

            InitializeComponent();

            TempEnfriarReadedAction = new Action<object>(TempEnfriarReaded);
            this._system.suscribeOPCItem("RAMPAS.CONFIGURACION", "TempEnfriar", TempEnfriarReadedAction);
            CoolingTimeChangedAction =  new Action<object>(CoolingTimeChanged);
            this._system.suscribeOPCItem("RAMPAS.CONFIGURACION", "CoolingTime", CoolingTimeChangedAction);
            CoolingStartTimeChangedAction = new Action<object>(CoolingStartTimeChanged);
            this._system.suscribeOPCItem("RAMPAS.CONFIGURACION", "CoolingStartTime", CoolingStartTimeChangedAction);
            EmptyingTimeChangedAction = new Action<object>(EmptyingTimeChanged);
            this._system.suscribeOPCItem("RAMPAS.CONFIGURACION", "EmptyingTime", EmptyingTimeChangedAction);

            this.numericUpDownhayMaterial.Value = this._system.estados.tempLimiteHayMaterial;

            this._system.OPCClient.RefreshAsync(new StringBuilder(this._system.Path).Append(".RAMPAS.CONFIGURACION").ToString());

            this.FormClosed += RampaConfig_FormClosed;
        }

        void RampaConfig_FormClosed(object sender, FormClosedEventArgs e)
        {
            this._system.OPCClient.UnsuscribeGroup(new StringBuilder(this._system.Path).Append(".RAMPAS.CONFIGURACION").ToString(), "TempEnfriar", TempEnfriarReadedAction);
            this._system.OPCClient.UnsuscribeGroup(new StringBuilder(this._system.Path).Append(".RAMPAS.CONFIGURACION").ToString(), "CoolingTime", CoolingTimeChangedAction);
            this._system.OPCClient.UnsuscribeGroup(new StringBuilder(this._system.Path).Append(".RAMPAS.CONFIGURACION").ToString(), "CoolingStartTime", CoolingStartTimeChanged);
            this._system.OPCClient.UnsuscribeGroup(new StringBuilder(this._system.Path).Append(".RAMPAS.CONFIGURACION").ToString(), "EmptyingTime", EmptyingTimeChanged);
        }

        private void TempEnfriarReaded(object value) 
        {
            if (value is int)
            {
                this.setTempEnfriar((int)value);
            }
        }
        private void setTempEnfriar(int value)       
        {
            this.numericUpDownTempEnfriar.Value = value;
        }

        private void CoolingTimeChanged(object value)
        {
            if (value is int)
            {
                this.setCoolingTime((int)value);
            }
        }
        private void setCoolingTime(int value)       
        {
            this.numericUpDownCoolingTime.Value = value;
        }

        private void CoolingStartTimeChanged(object value)   
        {
            if (value is int)
                this.setCoolingStartTime((int)value);
        }
        private void setCoolingStartTime(int value)  
        {
            this.numericUpDownCoolingStartTime.Value = value;
        }

        private void EmptyingTimeChanged(object value)
        {
            if(value is int)
                setEmptyingTime((int) value);
        }
        private void setEmptyingTime(int value)
        {
            this.numericUpDownTimeVaciado.Value = value;
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            OPC.OPCGroupValues gv = new OPC.OPCGroupValues(new StringBuilder(this._system.Path).Append(".RAMPAS.CONFIGURACION").ToString());

            gv.Items.Add(new OPC.OPCItemValue("TempEnfriar", this.numericUpDownTempEnfriar.Value));
            gv.Items.Add(new OPC.OPCItemValue("CoolingTime", this.numericUpDownCoolingTime.Value));
            gv.Items.Add(new OPC.OPCItemValue("CoolingStartTime", this.numericUpDownCoolingStartTime.Value));
            gv.Items.Add(new OPC.OPCItemValue("EmptyingTime", this.numericUpDownTimeVaciado.Value));

            this._system.estados.tempLimiteHayMaterial = (int)this.numericUpDownhayMaterial.Value;

            this._system.OPCClient.WriteAsync(gv);
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            this._system.OPCClient.RefreshAsync(new StringBuilder(this._system.Path).Append(".RAMPAS.CONFIGURACION").ToString());
            this.numericUpDownhayMaterial.Value = this._system.estados.tempLimiteHayMaterial;
        }

    }
}
