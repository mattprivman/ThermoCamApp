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
        //CAMBIAR CONFIGURACIONES
        public static string getAppStringSetting(string property)                
        {
            return (string) Properties.Settings.Default[property];
        }
        public static void changeAppStringSetting(string property, string value) 
        {
            Properties.Settings.Default[property] = value;
            Properties.Settings.Default.Save();
        }

        public static int getAppIntSetting(string property)                      
        {
            return (int) Properties.Settings.Default[property];
        }
        public static void changeAppIntSetting(string property, int value)       
        {
            Properties.Settings.Default[property] = value;
            Properties.Settings.Default.Save();
        }

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
