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
        bool            _hasChildren;

        Sistema         _parent;

        #endregion

        #region "Propiedades"

        public string Nombre                        
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
        public bool HasChildren                     
        {
            get
            {
                return this._hasChildren;
            }

        }

        #endregion

        #region "Constructores"

        public Zona(String Name, Sistema parent)                        
        {
            this._nombre = Name;

            this._hasChildren = false;
            this._children = new List<SubZona>();

            this._parent = parent;
        }
        public Zona(SerializationInfo info, StreamingContext ctxt)      
        {
            this._nombre        = (string)          info.GetValue("Nombre",         typeof(string));
            this._children      = (List<SubZona>)   info.GetValue("Children",       typeof(List<SubZona>));
            this._hasChildren   = (bool)            info.GetValue("HasChildren",    typeof(bool));
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
            info.AddValue("HasChildren", this._hasChildren);
            info.AddValue("Parent",      this._parent);
        }

        #region "Añadir  borrar hijos"

        public void addChildren(SubZona child)      
        {
            this._children.Add(child);

            if (!this._hasChildren)
                this._hasChildren = true;
        }
        public void removeChildren(SubZona child)   
        {
            this._children.Remove(child);

            if (this._children.Count == 0)
                this._hasChildren = false;
        }

        #endregion

        #endregion
    }
}
