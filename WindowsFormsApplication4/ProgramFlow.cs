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
    class ProgramFlow
    {
        internal enum windowIds : int
        {
            appMode                 = 0,
            appCameraConfiguration  = 1,
            appSelectOPCServer      = 2,
            appMain                 = 3,
            appCannonConfig         = 4,
            salir                   = 5
        }

        public static void Start()
        {
            int numeroCamaras   = 1;

            Sistema _system     = null;

            bool finAsistente   = false;
            int  step           = 0;

            //Borrar archivo de datos
            //System.IO.File.Delete("Data.ocl");
            if(System.IO.File.Exists("data.ocl"));
                _system = Helpers.deserializeSistema("data.ocl");

            if (_system != null)
            {
                if (_system.ThermoCams.Count > 0)
                {
                    numeroCamaras = _system.ThermoCams.Count;
                    step          = (int) windowIds.appMain;
                }
            }

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
                            _system.SelectedZona = null;
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
                            _system.SelectedZona = null;
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
                                    numeroCamaras = _system.ThermoCams.Count;
                                    //Helpers.serializeSistema(cc.Sistema, "data.ocl");
                                }
                                else
                                {
                                    //SIGUIENTE
                                    if (_system.Mode == "Rampas")
                                        step = (int)windowIds.appCannonConfig;
                                    else
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

                    case (int) windowIds.appCannonConfig:

                        #region "Configuración cañon"
                        if (_system != null)
                        {
                            _system.SelectedZona = null;
                            foreach (Zona z in _system.Zonas)
                            {
                                _system.selectZona(z.Nombre);
                                using (Asistente.Cannon.CannonConfig cnc = new Asistente.Cannon.CannonConfig(z))
                                {
                                    cnc.ShowDialog();

                                    if (cnc.Salir == true)
                                    {
                                        //SALIR
                                        cnc.Dispose();
                                        return;
                                    }

                                    if (cnc.Atras)
                                    {
                                        //ATRAS
                                        step = (int)windowIds.appSelectOPCServer;
                                        //Helpers.serializeSistema(cc.Sistema, "data.ocl");
                                    }
                                    else
                                    {
                                        //SIGUIENTE
                                        step = (int)windowIds.appMain;         //step = (int) windowIds.appCameraNumber;
                                    }

                                    cnc.Dispose();
                                }
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
                            _system.SelectedZona = null;
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
                                    step = (int)windowIds.appMode;
                                    _system = m._system;
                                }
                                else
                                {
                                    //SIGUIENTE
                                    step = (int)windowIds.salir;
                                }

                                Helpers.serializeSistema(_system, "data.ocl");

                                m.Dispose();
                            }
                        }
                        else
                        {
                            step = (int)windowIds.appMode;
                        }

                        #endregion

                        break;
                    case (int) windowIds.salir:
                        finAsistente = true;
                        break;
                }

            }

            _system.Dispose();
        }
    }
}
