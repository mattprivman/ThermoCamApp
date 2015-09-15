using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }
        public void addSubZona(SubZona s)
        {
            this.selectedZona.addChildren(s);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt) 
        {
            info.AddValue("Zonas",      this._zonas);
            info.AddValue("ThermoCams", this._thermoCams);
        }

        public void addZona(Zona z)                 
        {
            lock ("Zonas")
            {
                this._zonas.Add(z);
            }
        }
        public void removeZona(Zona z)              
        {
            lock ("Zonas")
            {
                this._zonas.Remove(z);
            }
        }

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
