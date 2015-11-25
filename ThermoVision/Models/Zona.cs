using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ThermoVision.Models
{
    [Serializable]
    public class Zona : ISerializable
    {
        public enum States
        {
            Vacio,
            Lleno,
            Enfriando,
            Esperando,
            Vaciando,
            Manual
        }

        #region "Variables"

        string                  _nombre;

        List<SubZona>           _children;

        Rampa                   _parent;

        public float            _maxTemp;
        public float            _minTemp;
        public double           _meanTemp;

        States                  _state;

        #endregion

        #region "Propiedades"

        public string        Nombre           // -rw      
        {
            get
            {
                return this._nombre;
            }
            set
            {
                this._nombre = value;
            }
        }
        public List<SubZona> Children         // -r       
        {
            get
            {
                return this._children;
            }
        }
        public Rampa         Parent           // -r       
        {
            get
            {
                return this._parent;
            }
        }
        public int           Posicion         // -rw      
        {
            get;
            set;
        }
        public States        State            // -rw      
        {
            get { return this._state; }
            set 
            {
                 if (this.State == States.Enfriando && value != States.Enfriando)
                {
                    if (this.zonaCoolingStop != null)
                        this.zonaCoolingStop(this, null);
                }

                if (this.State == States.Vaciando && value != States.Vaciando)
                {
                    if (this.zonaEmptyingStop != null)
                        this.zonaEmptyingStop(this, null);
                }

                lock("CambioEstado")
                    this._state = value;
            }
        }

               

        public int Width                      // -rw      
        {
            get
            {
                int ancho = 0;

                foreach (SubZona s in this._children)
                    ancho += s.Fin.X - s.Inicio.X;

                return ancho;
            }
        }

        #endregion

        #region "Delegados"

        public delegate void stateChangedDelegate(object sender, States state);

        #endregion

        #region "Eventos"

        public event stateChangedDelegate zonaStateChanged;
        public event EventHandler zonaCoolingStop;
        public event EventHandler zonaEmptyingStop;

        #endregion

        #region "Constructores"

        public Zona(String Name, Rampa parent)                        
        {
            this._nombre = Name;

            this._children = new List<SubZona>();

            this._parent = parent;
        }
        protected Zona(SerializationInfo info, StreamingContext ctxt)   
        {
            this._nombre        = (string)          info.GetValue("Nombre",         typeof(string));
            this._children      = (List<SubZona>)   info.GetValue("Children",       typeof(List<SubZona>));
            this._parent        = (Rampa)           info.GetValue("Parent",         typeof(Rampa));
        }

        #endregion

        #region "Metodos públicos"

        #region "Seleccionar y deseleccionar subzonas"

        public void selectSubZonas()                
        {
            foreach (SubZona s in this._children)
            {
                s.Selected = true;
            }
        }
        public void unSelectSubZonas()              
        {
            foreach (SubZona s in this._children)
            {
                s.Selected = false;
            }
        }

        #endregion

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctxt)    
        {
            info.AddValue("Nombre",      this._nombre);
            info.AddValue("Children",    this._children);
            info.AddValue("Parent",      this._parent);
        }

        public void ChangeState(States state)                                  
        {
            this.State = state;

            if (this.zonaStateChanged != null)
                this.zonaStateChanged(this, state);
        }

        #region "Añadir  borrar hijos"

        public void addChildren(SubZona child)      
        {
            lock ("SubZonas")
            {
                //COMPROBAR QUE NO EXISTA NINGÚNA SUBZONA CON EL MISMO NOMBRE
                if (!this._children.Exists(x => x.Nombre == child.Nombre))
                {
                    this._children.Add(child);
                    child.Parent = this;
                }
                else
                    throw new Exception("Ya existe una subzona con ese nombre");
            }
        }
        public void removeChildren(SubZona child)   
        {
            lock ("SubZonas")
            {
                if (child.ThermoParent != null)
                    child.ThermoParent = null;

                this._children.Remove(child);
            }
        }
        public SubZona getChildren(string name)     
        {
            if (this._children.Exists(x => x.Nombre == name))
                return this._children.Where(x => x.Nombre == name).First();
            else
                return null;
        }

        #endregion

        #endregion

       

    }
}
