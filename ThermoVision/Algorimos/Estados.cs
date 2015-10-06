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
        int tempLimite;
        const int tempLimiteHayMaterial   = 33;
        const int tempLimiteHayQueEnfriar = 38;

        Sistema _system;
        #endregion

        #region "Delegados"
        public delegate void ThermoCamImgCuadradosCallback(object sender, ThemoCamImgCuadradosArgs e);
        #endregion

        #region "Eventos"
        public event ThermoCamImgCuadradosCallback ThermoCamImgCuadradosGenerated;
        #endregion


        public Estados(int tempLimite, Sistema s)
        {
            this._system = s;
            this.tempLimite = tempLimite;
        }

        public void ejecutarAlgoritmos()
        {
            if (this._system.getZona(this._system.zonaApagado) != null)
            {
                //Comprobar si existe material en las zonas de apagado

                if (algoritmoCompararSubzonaConTemLimitHayMaterial(this._system.getZona(this._system.zonaApagado)))             //HAY MATERIAL EN ZONA DE APAGADO
                {
                    //Hay material
                    //Diferenciar zonas con material


                    //Detectar si el material esta caliente (Por encima del límite soportable por la cinta)
                    if (algoritmoCompararSubzonaConTemLimitHayQueEnfriar(this._system.getZona(this._system.zonaApagado)))       //HAY MATERIAL CON PUNTOS CALIENTES
                    {
                        //Hay que enfriar para ello se envía al PLC a través de OPC las temperaturas de cada subzona
                        //Buscar a que PLC hay que enviar los datos


                    }//if puntos calientes
                }//if material
                
                if(algoritmoCompararSubzonaConTemLimitHayMaterial(this._system.getZona(this._system.zonaVaciado)))              //HAY MATERIAL EN ZONA DE VACIADO
                {
                    //Hay material

                    if (!algoritmoCompararSubzonaConTemLimitHayQueEnfriar(this._system.getZona(this._system.zonaVaciado)))       //NO HAY MATERIAL CON PUNTOS CALIENTES
                    {
                        //Se puece vaciar la zona
                    }//if puntos calientes
                }//if material

                //DIbujar los estados en la imagen
                this._system.dibujarEstados();

            } //if zona exists
        }

        private bool algoritmoCompararSubzonaConTemLimitHayMaterial(Zona z)  
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
                            s.tempMatrix[x, y].hayMaterial = (s.tempMatrix[x, y].max > tempLimiteHayMaterial) ? true : false;

                            if (s.tempMatrix[x, y].hayMaterial)
                                res = true;
                        }//for columnas                            
                    }//for filas
                }//if tempMatrix != null
            }//foreach

            return res;
        }   //Actualizar subzonas que tienen material          --> Devuelve si hay alguna zona con material
        private bool algoritmoCompararSubzonaConTemLimitHayQueEnfriar(Zona z)
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
            }//foreach

            return res;
        }   //Actualizar subzonas que tienen puntos calientes  --> Devuelve si hay alguna zona con algun punto caliente

        public void crearImagenCuadrados(Zona zonaApagado, Zona zonaVaciado)
        {
            //Calcular altura y ancho de la rampa
            int widthRampa = 0;
            int heightRampa = 0;

            foreach (SubZona s in zonaApagado.Children)
            {
                widthRampa += s.Fin.X - s.Inicio.X;

                if (s.Fin.Y - s.Inicio.Y > heightRampa)
                {
                    heightRampa = s.Fin.Y - s.Inicio.Y;
                }
            }

            //Calcular ancho de la zona de rejilla
            int widthRejilla = 0;

            foreach (SubZona s in zonaVaciado.Children)
            {
                widthRejilla += s.Fin.X - s.Inicio.X;
            }

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

               
                foreach (SubZona s in zonaApagado.Children)
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
                                    c = Color.DarkRed;

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
                }//FOREACH SUBZONA

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //                                                                                                                                  //
                //                                         CONTINUAR                                                                                //
                //                      Dibujar trampillas de vaciado y cañones de agua.                                                            //
                //                                                                                                                                  //
                //                                                                                                                                  //
                //                                                                                                                                  //
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                //Dibujar zona de las trampillas

                if (widthRampa > widthRejilla)
                    offsetX = widthRampa - widthRejilla;
                else
                    offsetX = 0;

                int offsetY = (int) (heightRampa +
                                    heightRampa * 0.05);

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
                }//foreach

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

        private void indentificarZonasConMaterial(Zona z)
        {
          
        }
    }
}
