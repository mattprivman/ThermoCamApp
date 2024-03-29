﻿using System;
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
            appCameraNumber         = 1,
            appZoneConfiguration    = 2,
            appCameraConfiguration  = 3,
            appCreateOPCVarsCSV     = 4
        }

        public static void Start()
        {
            int numeroCamaras   = 0;
            int numeroZonas     = 0;

            Sistema _system     = null;

            bool finAsistente   = false;
            int  step           = 0;

            //Borrar archivo de datos
            //System.IO.File.Delete("Data.ocl");
            Helpers.changeAppStringSetting("Mode", "");

            while (finAsistente == false)
            {
                switch (step)
                {
                    case (int) windowIds.appMode:                       //ELEGIR MODO DE APLICACIÓN

                        #region "Modo de aplicación"
                        if (Helpers.getAppStringSetting("Mode") == "")
                        {
                            using (Asistente.selectAppType AppType = new Asistente.selectAppType())
                            {
                                AppType.ShowDialog();

                                if (AppType.Salir)
                                {
                                    AppType.Dispose();
                                    return;
                                }
                                AppType.Dispose();
                            }
                        }

                        step = (int) windowIds.appCameraNumber;
                        #endregion

                        break;
                    case (int) windowIds.appCameraNumber:               //ELEGIR NÚMERO DE CAMARAS Y NUMERO DE ZONAS

                        #region "Número de camaras"

                        using (Asistente.Camaras.CameraNumberSelection cns = new Asistente.Camaras.CameraNumberSelection())
                        {
                            cns.ShowDialog();
                            numeroCamaras   = cns.NumeroCamaras;          //Establecer el numero de camaras elegido para la aplicación
                            numeroZonas     = cns.NumeroZonas;            //Establecer el numero de zonas elegido para la aplicación

                            if (cns.Salir == true)
                            {
                                cns.Dispose();
                                return;
                            }

                            if (cns.Atras)
                            {
                                step = (int)windowIds.appMode; ;
                                Helpers.changeAppStringSetting("Mode", "");
                            }
                            else
                                step = (int)windowIds.appZoneConfiguration; ;

                            cns.Dispose();
                        }

                        #endregion

                        break;
                    case (int) windowIds.appZoneConfiguration:          //CONFIGURAR NOMBRES DE ZONAS

                        #region "Nombres de zonas"
                        if(numeroZonas > 0)
                        {
                            using (Asistente.Camaras.ZoneConfiguration zC = new Asistente.Camaras.ZoneConfiguration(numeroZonas))
                            {
                                zC.ShowDialog();

                                if (zC.Salir == true)
                                {
                                    zC.Dispose();
                                    return;
                                }

                                if (zC.Atras)
                                {
                                    step = (int)windowIds.appCameraNumber;
                                    Helpers.changeAppStringSetting("Mode", "");
                                }
                                else
                                    step = (int)windowIds.appCameraConfiguration; ;

                                _system = zC.Sistema;

                                zC.Dispose();
                            }
                        } 
                        else
                        {
                            step = (int)windowIds.appCameraNumber;
                        }

                        #endregion

                        break;
                    case (int) windowIds.appCameraConfiguration:        //CONFIGURAR CAMARAS Y CREAR FICHERO PARA ALMACENAR LAS VARIABLES

                        #region "Configurar cámaras"

                        if (numeroCamaras > 0 && _system != null)
                        {
                            Asistente.Camaras.CamerasConfiguration cc = new Asistente.Camaras.CamerasConfiguration(numeroCamaras, _system);
                            cc.ShowDialog();

                            if (cc.Salir == true)
                            {
                                cc.Dispose();
                                return;
                            }

                            if (cc.Atras)
                                step = (int) windowIds.appZoneConfiguration;
                            else
                                step = (int) windowIds.appCreateOPCVarsCSV;         //step = (int) windowIds.appCameraNumber;

                            cc.Dispose();
                        }
                        else
                        {
                            step = (int) windowIds.appCameraNumber;
                        }

                        #endregion

                        break;
                    case (int) windowIds.appCreateOPCVarsCSV:           //CREAR CSV PARA VARIABLES OPC

                        #region "Crear variables OPC"



                        #endregion

                        break;
                }

            }
        }
    }
}
