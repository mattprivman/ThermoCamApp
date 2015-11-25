using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using System.Drawing;

namespace ThermoVision.Models
{
    [Serializable]
    public sealed class ZonaVaciado : Zona, ISerializable
    {
        public List<ZonaApagado> zonasContenidas = new List<ZonaApagado>();          //Para zonas de apagado

        public Point EmptyingPoint             // -rw      
        {
            get;
            set;
        }
        public int   EmptyingSubZone             // -rw      
        {
            get;
            set;
        }
        public bool  Emptying                   // -rw      
        {
            get;
            set;
        }

        public ZonaVaciado(String name, Rampa parent) 
            : base(name, parent)
        {
            this.EmptyingPoint = new Point(0, 0);
        }

        protected ZonaVaciado(SerializationInfo info, StreamingContext ctxt)
            : base (info, ctxt)
        {
            this.EmptyingPoint = new Point(0, 0);
        }

        public void coordinateXVaciadoChanged(object x) 
        {
            if (x is int)
            {
                if (this.EmptyingSubZone < this.Children.Count &&
                   (int) x < this.Children[this.EmptyingSubZone].Filas &&
                   this.EmptyingPoint.Y < this.Children[this.EmptyingSubZone].Columnas)
                {
                    this.EmptyingPoint = new Point((int)x, this.EmptyingPoint.Y);

                    this.Children[this.EmptyingSubZone].tempMatrix[this.EmptyingPoint.X, this.EmptyingPoint.Y].hayMaterial = false;
                    this.Children[this.EmptyingSubZone].tempMatrix[this.EmptyingPoint.X, this.EmptyingPoint.Y].estaCaliente = false;
                }
            }
        }
        public void coordinateYVaciadoChanged(object y) 
        {
            if (y is int)
            {
                if (this.EmptyingSubZone < this.Children.Count &&
                   this.EmptyingPoint.X < this.Children[this.EmptyingSubZone].Filas &&
                   (int) y < this.Children[this.EmptyingSubZone].Columnas)
                {
                    this.EmptyingPoint = new Point(this.EmptyingPoint.X, (int)y);

                    this.Children[this.EmptyingSubZone].tempMatrix[this.EmptyingPoint.X, this.EmptyingPoint.Y].hayMaterial = false;
                    this.Children[this.EmptyingSubZone].tempMatrix[this.EmptyingPoint.X, this.EmptyingPoint.Y].estaCaliente = false;
                }
            }
        }
        public void subZonaNVaciadoChanged(object n)    
        {
            if (n is int)
            {
                if ((int) n < this.Children.Count && 
                    this.EmptyingPoint.X < this.Children[this.EmptyingSubZone].Filas &&
                    this.EmptyingPoint.Y < this.Children[this.EmptyingSubZone].Columnas)
                {
                    this.EmptyingSubZone = (int)n;

                    this.Children[this.EmptyingSubZone].tempMatrix[this.EmptyingPoint.X, this.EmptyingPoint.Y].hayMaterial = false;
                    this.Children[this.EmptyingSubZone].tempMatrix[this.EmptyingPoint.X, this.EmptyingPoint.Y].estaCaliente = false;
                }
            }
        }
        public void emptyingStateChanged(object state)  
        {
            if (state is bool)
            {
                this.Emptying = (bool)state;
                if (this.Emptying)
                {
                    this.State = States.Vaciando;
                }
                else
                {
                    this.State = States.Esperando;
                }
            }
        }
    }
}
