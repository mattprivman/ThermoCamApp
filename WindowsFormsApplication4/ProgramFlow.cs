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
        internal enum windowIds : int
        {
            appMode                 = 0,
            appCameraConfiguration  = 1,
            appCreateOPCVarsCSV     = 2
        }

        public static void Start()
        {
            int numeroCamaras   = 1;

            Sistema _system     = null;

            bool finAsistente   = false;
            int  step           = 0;

            //Borrar archivo de datos
            //System.IO.File.Delete("Data.ocl");
            //_system = Helpers.deserializeSistema("data.ocl");

            if (_system != null)
            {
                if (_system.ThermoCams.Count > 0)
                {
                    numeroCamaras = _system.ThermoCams.Count;
                    step          = (int)windowIds.appCameraConfiguration;
                }
            }

            Helpers.changeAppStringSetting("Mode", "");

            while (finAsistente == false)
            {
                switch (step)
                {
                    case (int) windowIds.appMode:                       //ELEGIR MODO DE APLICACIÓN Y NÚMERO DE CÁMARAS

                        #region "Modo de aplicación"
                        if (Helpers.getAppStringSetting("Mode") == "")
                        {
                            //_system = Helpers.deserializeSistema("data.ocl");

                            if (_system != null)
                            {
                                numeroCamaras = _system.ThermoCams.Count;
                            }

                            using (Asistente.selectAppType AppType = new Asistente.selectAppType(numeroCamaras))
                            {
                                AppType.ShowDialog();

                                if (AppType.Salir)
                                {
                                    AppType.Dispose();
                                    return;
                                }

                                numeroCamaras = AppType.NumeroCamaras;
                                AppType.Dispose();
                            }
                        }

                        step = (int) windowIds.appCameraConfiguration;
                        #endregion

                        break;
                    case (int) windowIds.appCameraConfiguration:        //CONFIGURAR CAMARAS Y CREAR FICHERO PARA ALMACENAR LAS VARIABLES

                        #region "Configurar cámaras"

                        if (numeroCamaras > 0)
                        {
                            //_system = Helpers.deserializeSistema("data.ocl");

                            Asistente.Camaras.CamerasConfiguration cc = new Asistente.Camaras.CamerasConfiguration(numeroCamaras, _system);
                            cc.ShowDialog();

                            if (cc.Salir == true)
                            {
                                cc.Dispose();
                                return;
                            }

                            if (cc.Atras)
                            {
                                step = (int)windowIds.appMode;
                                Helpers.changeAppStringSetting("Mode", "");
                                //Helpers.serializeSistema(cc.Sistema, "data.ocl");
                            }
                            else
                            {
                                step = (int)windowIds.appCreateOPCVarsCSV;         //step = (int) windowIds.appCameraNumber;
                                //Guardar sistema
                                //Helpers.serializeSistema(cc.Sistema, "data.ocl");
                            }


                            cc.Dispose();
                        }
                        else
                        {
                            step = (int) windowIds.appMode;
                        }

                        #endregion

                        break;
                    case (int) windowIds.appCreateOPCVarsCSV:           //CREAR CSV PARA VARIABLES OPC

                        #region "Crear variables OPC"

                        _system = Helpers.deserializeSistema("data.ocl");

                        #endregion

                        break;
                }

            }
        }
    }
}
