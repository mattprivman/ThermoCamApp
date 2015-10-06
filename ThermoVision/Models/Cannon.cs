using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;

namespace ThermoVision.Models
{
    [Serializable]
    public class Cannon : ISerializable
    {
        public string Name;
        public List<SubZona> SubZonas;

        public Cannon(string Name)
        {
            this.Name = Name;
            this.SubZonas = new List<SubZona>();
        }

        protected Cannon(SerializationInfo info, StreamingContext ctxt)
        {
            this.Name     = (string)        info.GetValue("Name",     typeof(string));
            this.SubZonas = (List<SubZona>) info.GetValue("SubZonas", typeof(List<SubZona>));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Name",       this.Name);
            info.AddValue("SubZonas",   this.SubZonas);
        }

        public void AddSubZona(SubZona s)    
        {
                s.Cannon = this;
                this.SubZonas.Add(s);
        }
        public void removeSubZona(SubZona s) 
        {
                if (this.SubZonas.Contains(s))
                    this.SubZonas.Remove(s);

                s.Cannon = null;
        }
    }
}
