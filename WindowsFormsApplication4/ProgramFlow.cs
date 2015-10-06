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
            appSelectOPCServer      = 2,
            appMain                 = 3
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
                    step          = (int)windowIds.appMain;
                }
            }

            Helpers.changeAppStringSetting("Mode", "");

            while (finAsistente == false)
            {
                switch (step)
                {
                    case (int) windowIds.appMode:                       //ELEGIR MODO DE APLICACIÓN Y NÚMERO DE CÁMARAS

                        #region "Modo de aplicación"

                        //_system = Helpers.deserializeSistema("data.ocl");

                        if (_system != null)
                        {
                            numeroCamaras = _system.ThermoCams.Count;
                        }
                        else
                        {
                            _system = new Sistema();
                        }

                        using (Asistente.selectAppType AppType = new Asistente.selectAppType(numeroCamaras, _system))
                        {
                            AppType.ShowDialog();

                            if (AppType.Salir)
                            {
                                AppType.Dispose();
                                return;
                            }
                            if (AppType.cargarConfiguracion)
                            {
                                _system = AppType._system;
                                Helpers.serializeSistema(_system, "data.ocl");
                                step = (int)windowIds.appMain;
                                break;
                            }

                           //Continuar
                           _system = AppType._system;

                            numeroCamaras = AppType.NumeroCamaras;
                            AppType.Dispose();
                        }

                        step = (int) windowIds.appCameraConfiguration;
                        #endregion

                        break;
                    case (int) windowIds.appCameraConfiguration:        //CONFIGURAR CAMARAS Y CREAR FICHERO PARA ALMACENAR LAS VARIABLES

                        #region "Configurar cámaras"

                        if (numeroCamaras > 0 && _system != null)
                        {
                            //_system = Helpers.deserializeSistema("data.ocl");
                            _system.selectedZona = null;
                            using (Asistente.Camaras.CamerasConfiguration cc = new Asistente.Camaras.CamerasConfiguration(numeroCamaras, _system))
                            {
                                cc.ShowDialog();

                                if (cc.Salir == true)
                                {
                                    cc.Dispose();
                                    return;
                                }

                                if (cc.Atras)
                                {
                                    step = (int)windowIds.appMode;
                                    //Helpers.serializeSistema(cc.Sistema, "data.ocl");
                                }
                                else
                                {
                                    step = (int)windowIds.appSelectOPCServer;         //step = (int) windowIds.appCameraNumber;
                                    //Guardar sistema
                                    //Helpers.serializeSistema(cc.Sistema, "data.ocl");
                                }

                                _system = cc.Sistema;
                                cc.Dispose();
                            }
                        }
                        else
                        {
                            step = (int) windowIds.appMode;
                        }

                        #endregion

                        break;
                    case (int) windowIds.appSelectOPCServer:

                        #region "Seleccionar servidor OPC"

                        if (_system != null)
                        {
                            _system.selectedZona = null;
                            using (Asistente.OPC.appSelectOPCServer sos = new Asistente.OPC.appSelectOPCServer(_system))
                            {
                                sos.ShowDialog();

                                if (sos.Salir == true)
                                {
                                    //SALIR
                                    sos.Dispose();
                                    return;
                                }

                                if (sos.Atras)
                                {
                                    //ATRAS
                                    step = (int)windowIds.appCameraConfiguration;
                                    //Helpers.serializeSistema(cc.Sistema, "data.ocl");
                                }
                                else
                                {
                                    //SIGUIENTE
                                    step = (int)windowIds.appMain;         //step = (int) windowIds.appCameraNumber;
                                    //Guardar sistema
                                    Helpers.serializeSistema(sos.Sistema, "data.ocl");
                                }

                                sos.Dispose();
                            }
                        }
                        else
                        {
                            step = (int)windowIds.appMode;
                        }

                        #endregion

                        break;
                    case (int)windowIds.appMain:

                        #region "Cargar aplicación"

                        if (_system != null)
                        {
                            _system.selectedZona = null;
                            using (main m = new main(_system))
                            {
                                m.ShowDialog();

                                if (m.Salir == true)
                                {
                                    //SALIR
                                    m.Dispose();
                                    return;
                                }

                                if (m.Atras)
                                {
                                    //ATRAS
                                    step = (int)windowIds.appCameraConfiguration;
                                    //Helpers.serializeSistema(cc.Sistema, "data.ocl");
                                }
                                else
                                {
                                    //SIGUIENTE
                                    //step = (int)windowIds.appSelectOPCServer;         //step = (int) windowIds.appCameraNumber;
                                    //Guardar sistema
                                    return;
                                }

                                m.Dispose();
                            }
                        }
                        else
                        {
                            Helpers.changeAppStringSetting("Mode", "");
                            step = (int)windowIds.appMode;
                        }

                        #endregion

                        break;

                }

            }

            _system.Dispose();
        }
    }
}
