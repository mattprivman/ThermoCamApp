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

            Rampa rampa     = null;

            bool finAsistente   = false;
            int  step           = 0;

            //Borrar archivo de datos
            //System.IO.File.Delete("Data.ocl");
            if (System.IO.File.Exists("data.ocl")) ;
                rampa = Helpers.deserializeSistema("data.ocl");

            if (rampa != null)
            {
                if (rampa.ThermoCams.Count > 0)
                {
                    numeroCamaras = rampa.ThermoCams.Count;
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

                        if (rampa != null)
                        {
                            numeroCamaras = rampa.ThermoCams.Count;
                        }
                        else
                        {
                            rampa = new Rampa();
                        }

                        using (Asistente.selectAppType AppType = new Asistente.selectAppType(numeroCamaras, rampa))
                        {
                            AppType.ShowDialog();

                            if (AppType.Salir)      
                            {
                                AppType.Dispose();
                                return;
                            }
                            if (AppType.cargarConfiguracion)
                            {
                                rampa = AppType._system;
                                step = (int)windowIds.appMain;
                                break;
                            }

                           //Continuar
                           rampa = AppType._system;

                            numeroCamaras = AppType.NumeroCamaras;
                            AppType.Dispose();
                        }

                        step = (int) windowIds.appCameraConfiguration;
                        #endregion

                        break;
                    case (int) windowIds.appCameraConfiguration:        //CONFIGURAR CAMARAS Y CREAR FICHERO PARA ALMACENAR LAS VARIABLES

                        #region "Configurar cámaras"

                        if (numeroCamaras > 0 && rampa != null)
                        {
                            //_system = Helpers.deserializeSistema("data.ocl");
                            rampa.SelectedZona = null;
                            using (Asistente.Camaras.CamerasConfiguration cc = new Asistente.Camaras.CamerasConfiguration(numeroCamaras, rampa))
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
                                                                
                                rampa = cc.Sistema;
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

                        if (rampa != null)
                        {
                            rampa.SelectedZona = null;
                            using (Asistente.OPC.appSelectOPCServer sos = new Asistente.OPC.appSelectOPCServer(rampa))
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
                                    numeroCamaras = rampa.ThermoCams.Count;
                                    //Helpers.serializeSistema(cc.Sistema, "data.ocl");
                                }
                                else
                                {
                                    //SIGUIENTE
                                    if (rampa.Mode == "Rampas")
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
                        if (rampa != null)
                        {
                            rampa.SelectedZona = null;
                            foreach (Zona z in rampa.Zonas)
                            {
                                rampa.selectZona(z.Nombre);
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

                        if (rampa != null)
                        {
                            rampa.SelectedZona = null;
                            using (main m = new main(rampa))
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
                                    rampa = m._system;
                                }
                                else
                                {
                                    //SIGUIENTE
                                    step = (int)windowIds.salir;
                                }

                                Helpers.serializeSistema(rampa, "data.ocl");
                                Helpers.serializeSistema(rampa, "data_backup.ocl");

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

            rampa.Dispose();
        }
    }
}
