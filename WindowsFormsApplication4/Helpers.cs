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
        public static void serializeThermoCams(List<ThermoVision.Models.ThermoCam> ThermoCams, string _file)
        {
            try
            {
                using (Stream stream = File.Open(_file, FileMode.Create))
                {
                    BinaryFormatter bformatter = new BinaryFormatter();

                    bformatter.Serialize(stream, ThermoCams);
                    stream.Close();
                }
            }
            catch (SerializationException ex)
            {
                ex.ToString();
            }
        }
        public static List<ThermoCam> deserializeThermoCams(string _file)                                   
        {
            List<ThermoCam> ThermoCams = null;

            try
            {
                using (Stream stream = File.Open(_file, FileMode.Open))
                {
                    BinaryFormatter bformatter = new BinaryFormatter();

                    ThermoCams = (List<ThermoCam>)bformatter.Deserialize(stream);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return ThermoCams;
        }

    }
}
