using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThermoVision.Models;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace WindowsFormsApplication4
{
    class Helpers
    {
        //DESERIALIZAR
        public static void serializeSistema(Sistema sistema, string _file)       
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
        public static Sistema deserializeSistema(string _file)                   
        {
            Sistema sistema = null;

            try
            {
                using (Stream stream = File.Open(_file, FileMode.Open))
                {
                    BinaryFormatter bformatter = new BinaryFormatter();

                    sistema = (Sistema)bformatter.Deserialize(stream);
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
