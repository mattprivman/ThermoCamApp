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
    class ProgramFlow
    {
        public static void Start()
        {
            bool atras = false;

            //Borrar archivo de datos
            //System.IO.File.Delete("Data.ocl");

            do
            {
                List<ThermoCam> _ThermoCams = Helpers.deserializeThermoCams("Data.ocl");

                //IF THERMOCAMS IS NULL START FROM SELECTING NUMBER OF CAMERAS
                #region "Inicio"
                if (_ThermoCams == null)
                {
                    //Elegir numero de camaras
                    Asistente.Camaras.CameraNumberSelection cns = new Asistente.Camaras.CameraNumberSelection();
                    cns.ShowDialog();

                    if (cns.Salir == true)
                    {
                        cns.Dispose();
                        return;
                    }

                    //Configurar camaras
                    Asistente.Camaras.CamerasConfiguration cc = new Asistente.Camaras.CamerasConfiguration(cns.NumeroCamaras);
                    cns.Dispose();
                    cc.ShowDialog();

                    Helpers.serializeThermoCams(cc.getThermoCams(), "Data.ocl");            //SERIALIZE THERMOCAM OBJECTS

                    if (cc.Salir == true)
                    {
                        cc.Dispose();
                        return;
                    }

                    atras = cc.Atras;
                    cc.Dispose();

                    _ThermoCams = Helpers.deserializeThermoCams("Data.ocl");
                }
                #endregion    

                //Configurar camaras
                Asistente.Camaras.CamerasConfiguration ccWith = new Asistente.Camaras.CamerasConfiguration(_ThermoCams);
                ccWith.ShowDialog();

                //Helpers.serializeThermoCams(ccWith.getThermoCams(), "Data.ocl");            //SERIALIZE THERMOCAM OBJECTS

                if (ccWith.Salir == true)
                {
                    ccWith.Dispose();
                    return;
                }

                atras = ccWith.Atras;
                ccWith.Dispose();


            } while (atras == true);
        }

        private static void startCameraAsistant()
        {
            
        }

        private static void startOPCServerAsistant()
        {
            
        }
    }
}
