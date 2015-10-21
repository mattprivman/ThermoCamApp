using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ThermoVision.Models;
using ThermoVision.Tipos;

namespace ThermoVision.Algoritmos
{
    public class Estados
    {
        #region "Variables"
        public int tempLimiteHayQueEnfriar = 27;
        public int tempLimiteHayMaterial   = 24;

        List<Zona> zonasAlgoritmoVaciar = new List<Zona>();

        Sistema _system;
        #endregion

        #region "Delegados"
        public delegate void ThermoCamImgCuadradosCallback(object sender, ThemoCamImgCuadradosArgs e);
        
        #endregion

        #region "Eventos"
        public event ThermoCamImgCuadradosCallback  ThermoCamImgCuadradosGenerated;
        #endregion

        public Estados(Sistema s)                        
        {
            this._system = s;
        }

        public void ejecutarAlgoritmos()                 
        {
            algoritmoEnfriar();                        //Ejecutar algoritmo ENFRIAR
            algoritmoVaciar();                         //Ejecutar algoritmo VACIAR

            //Dibujar los estados en la imagen
            this._system.dibujarEstados();
        }

        private void algoritmoEnfriar()                  
        {
            foreach (Zona z in this._system.Zonas)
            {
                if (!isZonaCooling(z))      //Si la zona no se esta enfriando
                {
                    z.CoolingPoint = new Point(-1, -1);

                    if (hayMaterialEnZona(z))   //Si hay material en la zona
                    {
                        if (hayPuntosCalientesEnZona(z))    //Si hay puntos calientes en la zona
                        {
                            //COMPROBAR QUE NO HAYA NINGUNA ZONA PERTENECIENTE A ESTA ZONA DE REJILLAS QUE ESTE ENFRIANDO (EVITAR EL PROBLEMA DEL CHORRO)

                            
                            //HAY QUE ENFRIAR
                            this._system.enfriarZona(z);    //Activar variable de activacion de cañon y enviar la matriz completa de temperaturas
                        }
                        else        //Si no hay puntos calientes en la zona
                        {
                            if (z.State != Zona.States.Vaciando)
                                z.triggerStateChangedEvent(Zona.States.Lleno);   
                            this._system.noHayQueEnfriar(z);
                        }
                    }//if hayMaterialEnZona
                    else      //Si no hay material en la zona
                    {
                        //RAMPA VACIA
                        if(z.State != Zona.States.Vaciando)
                            z.triggerStateChangedEvent(Zona.States.Vacio);                                                 //ESTADO VACIO
                        this._system.noHayQueEnfriar(z);
                    }//else hayMaterialEnZona
                }//if NOT isZonaCooling
                else        //Si la zona se esta enfriando
                {
                    //ZONA ENFRIANDO
                    if(z.State != Zona.States.Enfriando)
                        z.triggerStateChangedEvent(Zona.States.Enfriando);                                  //ZONA ENFRIANDO
                    try
                    {
                        int n = 0;
                        z.CoolingPoint = getCoolingCoordinates(z, ref n);
                        z.CoolingSubZone = n;
                    }
                    catch (Exception ex)
                    {
                        z.CoolingPoint = new Point(-1, -1);
                    }
                }
            }//foreach zona
        }   //AlgoritmoEnfriar
        private void algoritmoVaciar()                   
        {
            foreach(Zona z in this._system.ZonasVaciado)
            {
                //if (!isEmptying(z))     //Esta vaciando
                //{                    
                //Comprobar que no haya ninguna zona de calentamiento contenida enfriando

                bool zonaContenidaEnfriando = false;

                foreach (Zona zApagado in z.zonasContenidas)
                {
                    if (zApagado.State == Zona.States.Enfriando)
                        zonaContenidaEnfriando = true;
                    else
                    {
                        if (hayMaterialEnZona(zApagado))
                            if(!(z.State == Zona.States.Vaciando))
                                zApagado.triggerStateChangedEvent(Zona.States.Lleno);
                        else
                            if (!(z.State == Zona.States.Vaciando))
                                zApagado.triggerStateChangedEvent(Zona.States.Vacio);
                    }
                }//foreach

                if (zonaContenidaEnfriando == false)
                {
                    //No hay ninguna zona que este siendo vaciada en esta zona
                    //Comprobar que haya material

                    if (hayMaterialEnZona(z))
                    {
                        if (!hayPuntosCalientesEnZona(z))
                        {
                            if (!isEmptying(z))
                            {
                                this._system.vaciarZona(z);

                                foreach (Zona zApagado in z.zonasContenidas)
                                    if (hayMaterialEnZona(zApagado))
                                        zApagado.triggerStateChangedEvent(Zona.States.Lleno);
                                    else
                                        zApagado.triggerStateChangedEvent(Zona.States.Vacio);

                                if (hayMaterialEnZona(z))
                                    z.State = Zona.States.Lleno;
                                else
                                    z.State = Zona.States.Vacio;
                            }
                            else
                            {
                                foreach (Zona zApagado in z.zonasContenidas)
                                    if (hayMaterialEnZona(z))
                                    {
                                        if (z.State != Zona.States.Vaciando)
                                            zApagado.triggerStateChangedEvent(Zona.States.Vaciando);
                                    }
                                z.State = Zona.States.Vaciando;
                            }
                        }
                        else
                        {
                            this._system.noHAyQueVaciar(z);

                            if (!isEmptying(z))
                            {
                                foreach (Zona zApagado in z.zonasContenidas)
                                    if (hayMaterialEnZona(zApagado))
                                        zApagado.triggerStateChangedEvent(Zona.States.Lleno);
                                    else
                                        zApagado.triggerStateChangedEvent(Zona.States.Vacio);
                            }//if
                        }//if
                    }//if
                    else
                    {
                        this._system.noHAyQueVaciar(z);
                        if (!isEmptying(z))
                        {
                            foreach (Zona zApagado in z.zonasContenidas)
                                if (!hayMaterialEnZona(zApagado))
                                    zApagado.triggerStateChangedEvent(Zona.States.Vacio);
                        }
                    }
                }//IF ninguna zona enfriando
                //}//if NOT isEmptying
                //else
                //{
                //    foreach (Zona zApagado in z.zonasContenidas)
                //        zApagado.triggerStateChangedEvent(Zona.States.Vaciando);
                //}
            }//foreach zona
        }   //AlgoritmoVaciar

        private bool hayMaterialEnZona(Zona z)           
        {
            bool res = false;   //Valor devuelto que indicará si hay material o no en alguna divisón de alguna subzona

            //Existe material en alguna subzona
            foreach (SubZona s in z.Children)
            {
                //Se cumple la condición en cada una de las subzonas
                if (s.tempMatrix != null)
                {
                    //Recorrer cada subzona
                    for (int x = 0; x < s.tempMatrix.GetLength(0); x++)
                    {
                        for (int y = 0; y < s.tempMatrix.GetLength(1); y++)
                        {
                            //Comprobar si se cumple la condición en la subzona
                            s.tempMatrix[x, y].hayMaterial = (s.tempMatrix[x, y].max > tempLimiteHayMaterial) ? true : false;

                            if (s.tempMatrix[x, y].hayMaterial)
                                res = true;
                            else
                                s.tempMatrix[x, y].estaCaliente = false;
                        }//for columnas                            
                    }//for filas
                }//if tempMatrix != null
            }//foreach subzona

            return res;
        }   //Actualizar subzonas que tienen material          --> Devuelve si hay alguna zona con material
        private bool hayPuntosCalientesEnZona(Zona z)    
        {
            bool res = false;   //Valor devuelto que indicará si hay material o no en alguna divisón de alguna subzona

            //Existe material en alguna subzona
            foreach (SubZona s in z.Children)
            {
                //Se cumple la condición en cada una de las subzonas
                if (s.tempMatrix != null)
                {
                    //Recorrer cada subzona
                    for (int x = 0; x < s.tempMatrix.GetLength(0); x++)
                    {
                        for (int y = 0; y < s.tempMatrix.GetLength(1); y++)
                        {
                            //COmprobar si se cumple la condición en la subzona
                            s.tempMatrix[x, y].estaCaliente = (s.tempMatrix[x, y].max > tempLimiteHayQueEnfriar) ? true : false;

                            if (s.tempMatrix[x, y].estaCaliente)
                                res = true;
                        }//for columnas                            
                    }//for filas
                }//if tempMatrix != null
            }//foreach SubZona

            return res;
        }   //Actualizar subzonas que tienen puntos calientes  --> Devuelve si hay alguna zona con algun punto caliente

        private bool isZonaCooling(Zona z)               
        {
            bool value = false;

            try
            {
                object res = this._system.OPCClient.readSync(this._system.Path + ".RAMPAS.APAGADO." + z.Nombre, "Cooling");

                if (res is bool)
                    value = (bool)res;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return value;
        }
        private bool isEmptying(Zona z)                  
        {
            bool value = false;

            try
            {
                object res = this._system.OPCClient.readSync(this._system.Path + ".RAMPAS.VACIADO." + z.Nombre, "Emptying");

                if (res is bool)
                    value = (bool)res;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return value;
        }      

        private Point getCoolingCoordinates(Zona z, ref int n)      
        {
            object resX = this._system.OPCClient.readSync(this._system.Path + ".RAMPAS.APAGADO." + z.Nombre, "X");
            object resY = this._system.OPCClient.readSync(this._system.Path + ".RAMPAS.APAGADO." + z.Nombre, "Y");
            object resN = this._system.OPCClient.readSync(this._system.Path + ".RAMPAS.APAGADO." + z.Nombre, "n");

            if (resX != null && resY != null && resN != null)
            {
                if (resX is int && resY is int && resN is int)
                {
                    n = (int) resN;
                    return  new Point((int) resX, (int) resY);
                }
            }

            throw new Exception("Imposible leer las coordenadas de la zona " + z.Nombre + ".");
        }

        public void crearImagenCuadrados(List<Zona> zonasApagado, List<Zona> zonasVaciado) 
        {
            //Calcular altura y ancho de la rampa
            int widthRampa = 0;
            int heightRampa = 0;

            foreach (Zona z in zonasApagado)
            {
                foreach (SubZona s in z.Children)
                {
                    widthRampa += s.Fin.X - s.Inicio.X;

                    if (s.Fin.Y - s.Inicio.Y > heightRampa)
                    {
                        heightRampa = s.Fin.Y - s.Inicio.Y;
                    }//if
                }//foreach
            }//foreach

            //Calcular ancho de la zona de rejilla
            int widthRejilla = 0;

            foreach (Zona z in zonasVaciado)
            {
                foreach (SubZona s in z.Children)
                {
                    widthRejilla += s.Fin.X - s.Inicio.X;
                }//Foreach
            }//foreach

            int width = (widthRampa > widthRejilla) ? widthRampa : widthRejilla;

            if (widthRampa > 0 && heightRampa > 0)
            {
                int height = (int) (heightRampa +
                                    heightRampa * 0.05 +      //margen entre la rampa y las rejillas;
                                    heightRampa * 0.1);       //margen de las rejillas

                Bitmap bitmapCuadrados = new Bitmap(width, height);
                Color c;

                int offsetX = 0;

                //Comprobar si la rejilla es más ancha que la rampa
                if (widthRejilla > widthRampa)
                    offsetX = widthRejilla - widthRampa;

                foreach (Zona z in zonasApagado)
                {
                    int subZoneNumber = 0;
                    foreach (SubZona s in z.Children)
                    {
                        if (s.tempMatrix != null)
                        {
                            int diferenciaEjeY = heightRampa - (s.Fin.Y - s.Inicio.Y);

                            int colWidth = (int)((s.Fin.X - s.Inicio.X) / s.Columnas) + 1;
                            int filHeigth = (int)((s.Fin.Y - s.Inicio.Y) / s.Filas) + 1;

                            for (int y = 0; y < (s.Fin.Y - s.Inicio.Y); y++)
                            {
                                for (int x = 0; x < (s.Fin.X - s.Inicio.X); x++)
                                {
                                    c = Color.LightGray;

                                    //Calcular a que división pertenece estas coordenadas x e y
                                    int columna = x / colWidth;
                                    int fila = y / filHeigth;

                                    if (s.tempMatrix[fila, columna].hayMaterial)
                                        c = Color.DarkGray;
                                    if (s.tempMatrix[fila, columna].estaCaliente)
                                        c = Color.Orange;
                                    if (fila == z.CoolingPoint.X && columna == z.CoolingPoint.Y && z.CoolingSubZone == subZoneNumber)
                                        c = Color.Blue;

                                    bitmapCuadrados.SetPixel(offsetX + x, diferenciaEjeY + y, c);
                                }//FOR
                            }//FOR

                            //Comprobar que la altura llena la rampa
                            if (s.Fin.Y - s.Inicio.Y < heightRampa)
                            {
                                c = Color.LightGray;

                                for (int y = 0; y < diferenciaEjeY; y++)
                                {
                                    for (int x = 0; x < s.Fin.X - s.Inicio.X; x++)
                                    {
                                        bitmapCuadrados.SetPixel(offsetX + x, y, c);
                                    }//for x
                                }//for y
                            }//if altura

                            offsetX += s.Fin.X - s.Inicio.X;
                        }//IF

                        subZoneNumber++;
                    }//FOREACH SUBZONA
                }//FOREACH ZONA

                //Dibujar zona de las trampillas

                if (widthRampa > widthRejilla)
                    offsetX = widthRampa - widthRejilla;
                else
                    offsetX = 0;

                int offsetY = (int) (heightRampa +
                                    heightRampa * 0.05);

                foreach (Zona zonaVaciado in zonasVaciado)
                {
                    foreach (SubZona s in zonaVaciado.Children)
                    {
                        int diferenciaEjeY = heightRampa - (s.Fin.Y - s.Inicio.Y);

                        if (s.tempMatrix != null)
                        {
                            int colWidth = (int)((s.Fin.X - s.Inicio.X) / s.Columnas) + 1;
                            int filHeigth = (int)((s.Fin.Y - s.Inicio.Y) / s.Filas) + 1;

                            for (int y = 0; y < (int)(heightRampa * 0.1); y++)
                            {
                                for (int x = 0; x < (s.Fin.X - s.Inicio.X); x++)
                                {
                                    c = Color.LightGray;

                                    //Calcular a que división pertenece estas coordenadas x e y
                                    int columna = x / colWidth;
                                    int fila = y / filHeigth;

                                    if (s.tempMatrix[fila, columna].hayMaterial && !s.tempMatrix[fila, columna].estaCaliente)
                                        c = Color.Green;
                                    if (s.tempMatrix[fila, columna].estaCaliente)
                                        c = Color.Black;

                                    bitmapCuadrados.SetPixel(offsetX + x, offsetY + y, c);
                                }//for x
                            }//for y
                        }//tempMatrix != null
                        offsetX += s.Fin.X - s.Inicio.X;
                    }//foreach subzona
                }//FOREACH zona

                //LanzarEvento
                if (this.ThermoCamImgCuadradosGenerated != null)
                {
                    this.ThermoCamImgCuadradosGenerated(this, new ThemoCamImgCuadradosArgs()
                    {
                        Imagen = bitmapCuadrados
                    });
                }//if alguien suscrito
                
                //bitmapCuadrados.Dispose();
            }//IF WIDHT AND HEIGHT > 0
        }
    }
}
