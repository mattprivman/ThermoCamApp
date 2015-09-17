using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ThermoVision.Models
{
    [Serializable]
    public class Zona : ISerializable
    {
        #region "Variables"

        string          _nombre;

        List<SubZona>   _children;

        Sistema         _parent;

        #endregion

        #region "Propiedades"

        public string        Nombre                 
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
        public List<SubZona> Children               
        {
            get
            {
                return this._children;
            }
        }

        #endregion

        #region "Constructores"

        public Zona(String Name, Sistema parent)                        
        {
            this._nombre = Name;

            this._children = new List<SubZona>();

            this._parent = parent;
        }
        public Zona(SerializationInfo info, StreamingContext ctxt)      
        {
            this._nombre        = (string)          info.GetValue("Nombre",         typeof(string));
            this._children      = (List<SubZona>)   info.GetValue("Children",       typeof(List<SubZona>));
            this._parent        = (Sistema)         info.GetValue("Parent",         typeof(Sistema));
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

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)    
        {
            info.AddValue("Nombre",      this._nombre);
            info.AddValue("Children",    this._children);
            info.AddValue("Parent",      this._parent);
        }

        #region "Añadir  borrar hijos"

        public void addChildren(SubZona child)      
        {
            //COMPROBAR QUE NO EXISTA NINGÚNA SUBZONA CON EL MISMO NOMBRE
            if (!this._children.Exists(x => x.Nombre == child.Nombre))
                this._children.Add(child);
            else
                throw new Exception("Ya existe una subzona con ese nombre");
        }
        public void removeChildren(SubZona child)   
        {
            this._children.Remove(child);
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
