using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThermoVision.Models;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace ThermoCamApp
{
    class Helpers
    {
        //DESERIALIZAR
        public static void serializeSistema(Rampa sistema, string _file)       
        {
            try
            {
                using (Stream stream = File.Open(_file, FileMode.Create))
                {
                    BinaryFormatter bformatter = new BinaryFormatter();

                    bformatter.Serialize(stream, sistema);
                    stream.Close();
                }
            }
            catch (SerializationException ex)
            {
                ex.ToString();
            }
        }
        public static Rampa deserializeSistema(string _file)                   
        {
            Rampa sistema = null;

            try
            {
                using (Stream stream = File.Open(_file, FileMode.Open))
                {
                    BinaryFormatter bformatter = new BinaryFormatter();

                    sistema = (Rampa)bformatter.Deserialize(stream);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return sistema;
        }
    }
}
