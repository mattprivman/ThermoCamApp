using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ThermoVision.Models
{
    [Serializable]
    public sealed class ZonaApagado :Zona
    {
        public List<ZonaVaciado> zonasContenidas = new List<ZonaVaciado>();          //Para zonas de vaciado

        public Point CoolingPoint             // -rw      
        {
            get;
            set;
        }
        public int   CoolingSubZone           // -rw      
        {
            get;
            set;
        }
        public bool  Cooling                  // -rw      
        {
            get;
            set;
        }
        public bool  Valvula                  // -rw      
        {
            get;
            set;
        }

        public ZonaApagado(String name, Rampa parent)
            : base(name, parent)
        {
            this.CoolingPoint = new Point(0, 0);
        }

         protected ZonaApagado(SerializationInfo info, StreamingContext ctxt)
            : base (info, ctxt)
        {
            this.CoolingPoint = new Point(0, 0);
        }

        public void coolingStateChanged(object state)
        {
            if (state is bool && this.State != States.Manual)
            {
                this.Cooling = (bool)state;

                if (this.Cooling)
                {
                    if (this.State != States.Enfriando)
                        this.ChangeState(States.Enfriando);
                }
                else
                {
                    if (this.State != States.Esperando)
                        this.ChangeState(States.Esperando);
                }
            }
        }
        public void coordinateXChanged(object x)        
        {
            if(x is int)
                this.CoolingPoint = new Point((int) x, this.CoolingPoint.Y);
        }
        public void coordinateYChanged(object y)        
        {
            if (y is int)
                this.CoolingPoint = new Point(this.CoolingPoint.X, (int) y);
        }
        public void subZonaNChanged(object n)           
        {
            if (n is int)
                this.CoolingSubZone = (int) n;
        }
        public void ValvulaStateChanged(object state)   
        {
            if(state is bool)
                this.Valvula = (bool) state;
        }

    }
}
