using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ThermoVision.Models
{
    public class Sistema : ISerializable
    {
        #region "Variables"

        List<Zona>      _zonas;
        List<ThermoCam> _thermoCams;

        public Zona            selectedZona;

        #endregion

        #region "Eventos"

        public event EventHandler zonasListChanged;

        #endregion

        #region "Propiedades"

        public List<Zona>       Zonas            // -r 
        {
            get
            {
                return this._zonas;
            }
        }
        public List<ThermoCam>  ThermoCams       // -r 
        {
            get
            {
                return this._thermoCams;
            }
        }

        #endregion

        #region "Constructores"

        public Sistema()                                                
        {
            this._zonas      = new List<Zona>();
            this._thermoCams = new List<ThermoCam>();
        }
        public Sistema(SerializationInfo info, StreamingContext ctxt)   
        {
            this._zonas      =  (List<Zona>)      info.GetValue("Zonas",        typeof(List<Zona>));
            this._thermoCams =  (List<ThermoCam>) info.GetValue("ThermoCams",   typeof(List<ThermoCam>));
        }

        #endregion

        #region "Métodos públicos"

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt) 
        {
            info.AddValue("Zonas",      this._zonas);
            info.AddValue("ThermoCams", this._thermoCams);
        }

        #region "ZONAS"
        public void addZona(Zona z)                 
        {
            lock ("Zonas")
            {
                //Comprobar que no haya una zona con el mismo nombre
                foreach (Zona item in this._zonas)
                {
                    if (item.Nombre == z.Nombre)
                    {
                        throw new Exception("Ya hay una zona con el nombre " + z.Nombre + ".");
                    }
                }

                this._zonas.Add(z);

                if (zonasListChanged != null)
                    zonasListChanged(this, null);
            }
        }       //AÑADIR ZONA
        public void removeZona(Zona z)              
        {
            lock ("Zonas")
            {
                foreach (SubZona t in z.Children)
                {
                    t.ThermoParent.SubZonas.Remove(t);
                }

                z.Children.Clear();
                this._zonas.Remove(z);

                this.selectedZona = null;

                if (zonasListChanged != null)
                    zonasListChanged(this, null);
            }
        }       //BORRAR ZONA
        public Zona getZona(string Nombre)          
        {
            if (this._zonas.Exists(x => x.Nombre == Nombre))
                return this._zonas.Where(x => x.Nombre == Nombre).First();

            return null;
        }       //DEVOLVER ZONA
        public void selectZona(string name)         
        {
            foreach (Zona z in this._zonas)                                             //Deseleccionar todas las zonas
                z.unSelectSubZonas();

            if (this._zonas.Exists(x => x.Nombre == name))                              //Seleccionar las subzonas de la subzona a activar
            {
                Zona z = this._zonas.Where(x => x.Nombre == name).First();
                this.selectedZona = z;
                z.selectSubZonas();
            }
        }       //SELECCIONAR ZONA
        #endregion

        public void addThermoCam(ThermoCam t)       
        {
            lock("ThermoCams")
            {
                this._thermoCams.Add(t);
            }
        }
        public void removeThermoCam(ThermoCam t)    
        {
            lock ("ThermoCams")
            {
                this._thermoCams.Remove(t);
            }
        }

        #endregion
    }
}
