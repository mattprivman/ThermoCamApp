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
        List<Zona>      _zonas;
        List<ThermoCam> _thermoCams;

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

        public void addZona(Zona z)                 
        {
            this._zonas.Add(z);
        }
        public void removeZona(Zona z)              
        {
            this._zonas.Remove(z);
        }

        public void addThermoCam(ThermoCam t)       
        {
            this._thermoCams.Add(t);
        }
        public void removeThermoCam(ThermoCam t)    
        {
            this._thermoCams.Remove(t);
        }

        #endregion
    }
}
