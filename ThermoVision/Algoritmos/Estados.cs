using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using ThermoVision.Models;
using ThermoVision.Tipos;

namespace ThermoVision.Algoritmos
{
    public class Estados
    {
        #region "Variables"
        public int tempLimiteHayQueEnfriar = 30;
        public int tempLimiteHayMaterial   = 27;

        Bitmap bitmapCuadrados;
        Bitmap bitmapRejillas;
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
            Action[] actions = new Action[2];

            actions[0] = algoritmoEnfriar;
            actions[1] = this._system.dibujarEstados;

            Parallel.Invoke(actions);


            //algoritmoEnfriar();                        //Ejecutar algoritmo ENFRIAR

            ////Dibujar los estados en la imagen
            //this._system.dibujarEstados();
        }

        private void algoritmoEnfriar()                             
        {
            Parallel.ForEach<Zona>(this._system.Zonas, (z) =>
            {
                if (!isZonaCooling(z))
                {
                    //getCannonCoordinates(z);

                    switch (z.State)
                    {
                        case Zona.States.Vacio:
                            #region "VACIO"
                            if (hayMaterialEnZona(z))
                                z.triggerStateChangedEvent(Zona.States.Lleno);
                            break;
                            #endregion
                        case Zona.States.Lleno:
                            #region "LLENO"
                            if (hayMaterialEnZona(z))
                            {
                                if (hayPuntosCalientesEnZona(z))
                                {
                                    this._system.enfriarZona(z);

                                    foreach (Zona zVaciado in z.zonasContenidas)
                                        addMaterialEnZona(zVaciado);

                                }//if
                                else
                                {
                                    this._system.noHayQueEnfriar(z);
                                }
                            }//if 
                            else
                            {
                                this._system.noHayQueEnfriar(z);
                                z.triggerStateChangedEvent(Zona.States.Vacio);
                            }
                            break;
                            #endregion
                        case Zona.States.Enfriando:
                            #region "ENFRIANDO"
                            z.triggerStateChangedEvent(Zona.States.Esperando);

                            break;
                            #endregion
                        case Zona.States.Esperando:
                            #region "ESPERANDO"
                            //addMaterialEnZona(z);
                            if (hayPuntosCalientesEnZona(z))
                            {
                                foreach (Zona zVaciado in z.zonasContenidas)
                                    addMaterialEnZona(zVaciado);
                                this._system.enfriarZona(z);
                            }//if
                            else
                            {
                                this._system.noHayQueEnfriar(z);
                                //Vaciar
                                foreach (Zona zVaciado in z.zonasContenidas)
                                {
                                    //Comprobar que no haya ninguna zona enfriando ni con puntos calientes y vaciar
                                    if (zVaciado.zonasContenidas.Where((x) => x.State == Zona.States.Enfriando || x.State == Zona.States.Manual).Count() == 0 &&
                                        z.zonasContenidas.Where((x) => x.State == Zona.States.Enfriando).Count() == 0 &&
                                        this._system.ZonasVaciado.Where((x) => x.State == Zona.States.Vaciando).Count() == 0 &&
                                        this._system.Zonas.Where((x) => x.State == Zona.States.Vaciando).Count() == 0)
                                    {
                                        z.triggerStateChangedEvent(Zona.States.Vaciando);
                                        zVaciado.State = Zona.States.Vaciando;
                                        this._system.vaciarZona(zVaciado);
                                    }
                                    else
                                    {
                                        if (z.zonasContenidas.Where((x) => x.State == Zona.States.Vaciando).Count() == 0)
                                        {
                                            this._system.noHAyQueVaciar(zVaciado);
                                            zVaciado.State = Zona.States.Esperando;
                                        }
                                    }
                                }
                            }//else

                            break;
                            #endregion
                        case Zona.States.Vaciando:
                            #region "VACIANDO"

                            bool todoVacio = true;

                            foreach (Zona zVaciado in z.zonasContenidas)
                            {

                                if (!isEmptying(zVaciado))
                                {
                                    zVaciado.State = Zona.States.Esperando;

                                    //zVaciado.CoolingSubZone = -1;

                                    if (z.zonasContenidas.Where((x) => x.State == Zona.States.Vaciando).Count() == 0 &&
                                        z.zonasContenidas.Where((x) => x.State == Zona.States.Enfriando).Count() == 0)
                                    {

                                    }
                                }
                                else
                                {
                                    zVaciado.State = Zona.States.Vaciando;
                                    //int subzone = -1;
                                    //zVaciado.CoolingPoint = getEmptyingCoordinates(zVaciado, ref subzone);
                                    //zVaciado.CoolingSubZone = subzone;

                                    //zVaciado.Children[subzone].tempMatrix[zVaciado.CoolingPoint.X, zVaciado.CoolingPoint.Y].hayMaterial = false;
                                    //zVaciado.Children[subzone].tempMatrix[zVaciado.CoolingPoint.X, zVaciado.CoolingPoint.Y].estaCaliente = false;
                                }

                                foreach (SubZona s in zVaciado.Children)
                                {
                                    for (int x = 0; x < s.tempMatrix.GetLength(0); x++)
                                        for (int y = 0; y < s.tempMatrix.GetLength(1); y++)
                                            if (s.tempMatrix[x, y].hayMaterial || s.tempMatrix[x, y].estaCaliente)
                                                todoVacio = false;
                                }
                            }
                            if (todoVacio == false && this._system.ZonasVaciado.Where(x => x.State == Zona.States.Vaciando).Count() == 0)
                            {
                                foreach (Zona zVaciado in z.zonasContenidas)
                                {
                                    bool hayMaterial = false;
                                    foreach (SubZona s in zVaciado.Children)
                                    {
                                        for (int x = 0; x < s.tempMatrix.GetLength(0); x++)
                                            for (int y = 0; y < s.tempMatrix.GetLength(1); y++)
                                                if (s.tempMatrix[x, y].hayMaterial || s.tempMatrix[x, y].estaCaliente)
                                                    hayMaterial = true; ;
                                    }

                                    if (hayMaterial &&
                                        this._system.ZonasVaciado.Where(x => x.State == Zona.States.Vaciando).Count() == 0 &&
                                        zVaciado.zonasContenidas.Where((x) => x.State == Zona.States.Enfriando || x.State == Zona.States.Manual).Count() == 0 &&
                                        z.zonasContenidas.Where((x) => x.State == Zona.States.Enfriando).Count() == 0 &&
                                        this._system.Zonas.Where((x) => x.State == Zona.States.Vaciando).Count() == 1)
                                    {
                                        zVaciado.State = Zona.States.Vaciando;
                                        this._system.vaciarZona(zVaciado);
                                    }
                                }
                            }

                            if (todoVacio == true && z.zonasContenidas.Where(x => x.State == Zona.States.Vaciando).Count() == 0)
                                z.triggerStateChangedEvent(Zona.States.Vacio);

                            break;
                            #endregion
                        case Zona.States.Manual:
                            #region "MANUAL"
                            hayMaterialEnZona(z);
                            hayPuntosCalientesEnZona(z);

                            foreach (Zona zVaciado in this._system.ZonasVaciado)
                            {
                                if(zVaciado.zonasContenidas.Where((x) => (x.State == Zona.States.Enfriando || x.State == Zona.States.Vaciando || x.State == Zona.States.Esperando)).Count() == 0)
                                {
                                    hayMaterialEnZona(zVaciado);
                                }
                            }
                            //getCannonCoordinates(z);

                            break;
                            #endregion

                    }//switch
                }//if zona not cooling
                else
                {
                    if (z.State != Zona.States.Enfriando)
                        z.triggerStateChangedEvent(Zona.States.Enfriando);

                    foreach (Zona zVaciado in z.zonasContenidas)
                        zVaciado.State = Zona.States.Enfriando;
                    //getCannonCoordinates(z);
                }
            });
        }   //AlgoritmoEnfriar

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
        private void addMaterialEnZona(Zona z)                      
        {
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
                            if (s.tempMatrix[x, y].max > tempLimiteHayMaterial)
                                s.tempMatrix[x, y].hayMaterial = true;
                        }//for columnas                            
                    }//for filas
                }//if tempMatrix != null
            }//foreach subzona
        }
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

            //int width = (widthRampa > widthRejilla) ? widthRampa : widthRejilla;      //////// 1
            int width = widthRampa;

            if (widthRampa > 0 && heightRampa > 0)
            {
                int height = (int)(heightRampa +
                                    heightRampa * 0.05);      //margen entre la rampa y las rejillas;


                bitmapCuadrados = new Bitmap(width, height);
                Color c;
                bitmapRejillas = new Bitmap(widthRejilla, (int) (height * 0.1));

                int offsetX = 0;
                int inicio = 0;

                //Comprobar si la rejilla es más ancha que la rampa
                if (widthRejilla > widthRampa)
                {
                    offsetX = widthRejilla - widthRampa;
                    inicio = offsetX;
                }

                offsetX = 0;    /////////////////////////////////////////////////////////////// 1

                //Dibujar rampa     /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #region rampa
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

                            int columnaAnterior = 0;
                            int filaAnterior = 0;

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
                                    if (fila == z.CoolingPoint.X && columna == z.CoolingPoint.Y && z.CoolingSubZone == subZoneNumber && z.Cooling)
                                        c = Color.Blue;
                                    if (columna != columnaAnterior)
                                        c = Color.DarkBlue;
                                    if (fila != filaAnterior)
                                    {
                                        c = Color.DarkBlue;

                                        for (; x < (s.Fin.X - s.Inicio.X); x++)
                                        {
                                            bitmapCuadrados.SetPixel(offsetX + x, diferenciaEjeY + y, c);
                                        }
                                    }
                                    else
                                        bitmapCuadrados.SetPixel(offsetX + x, diferenciaEjeY + y, c);

                                    columnaAnterior = columna;
                                    filaAnterior = fila;
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
                                        this.bitmapCuadrados.SetPixel(offsetX + x, y, c);
                                    }//for x
                                }//for y
                            }//if altura

                            offsetX += s.Fin.X - s.Inicio.X;
                        }//IF
                        subZoneNumber++;

                    }//FOREACH SUBZONA

                    //Dibujar coordenadas del cañon
                    if (true)
                    {
                        int y0 = (int)(heightRampa +
                            heightRampa * 0.05 / 2);

                        int x0 = offsetX - z.Width / 2;

                        //Calcular coordenadas del cañon en la imagen

                        int ancho = 0;
                        int ColWidth = 0;
                        for (int i = 0; i < z.Children.Count; i++)
                        {
                            if (i < z.CoolingSubZone)
                                ancho += z.Children[i].Fin.X - z.Children[i].Inicio.X;
                            if (i == z.CoolingSubZone)
                            {
                                ColWidth = (z.Children[i].Fin.X - z.Children[i].Inicio.X) / z.Children[i].Columnas;
                                break;
                            }
                        }

                        int x1 = offsetX - z.Width + ancho + z.CoolingPoint.Y * ColWidth + ColWidth / 2;

                        int diferenciaEjeY  = heightRampa - (z.Children[z.CoolingSubZone].Fin.Y - z.Children[z.CoolingSubZone].Inicio.Y);
                        int FilHeight       = (z.Children[z.CoolingSubZone].Fin.Y - z.Children[z.CoolingSubZone].Inicio.Y) / z.Children[z.CoolingSubZone].Filas;
                        int y1              = diferenciaEjeY + z.CoolingPoint.X * FilHeight + FilHeight / 2;

                        using (Graphics g = Graphics.FromImage(bitmapCuadrados))
                        {
                            g.DrawLine(Pens.DarkBlue, new Point(x0, y0), new Point(x1, y1));
                        }
                    }

                }//FOREACH ZONA

                int borderX = 0;

                for(int i = 0; i < this._system.Zonas.Count - 1; i++)
                {
                    for (int j = 0; j < this._system.Zonas[i].Children.Count; j++)
                    {
                        borderX += this._system.Zonas[i].Children[j].Fin.X - this._system.Zonas[i].Children[j].Inicio.X;
                    }

                    //Dibujar separacion de rejilla
                    for (int y = 0; y < heightRampa; y++)
                    {
                        try
                        {
                            bitmapCuadrados.SetPixel(borderX, y, Color.Black);
                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                        }
                    }//for
                }//for

                #endregion

                //Dibujar zona de las trampillas ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #region "TRAMPILLAS"
                if (widthRampa > widthRejilla)
                {
                    offsetX = widthRampa - widthRejilla;
                    inicio = offsetX;
                }
                else
                {
                    offsetX = 0;
                    inicio = 0;
                }
                offsetX = 0;                                    ////////////////////////////////////// 1
                int offsetY = 0;
                //int offsetY = (int) (heightRampa +
                //                    heightRampa * 0.05);

                foreach (Zona zonaVaciado in zonasVaciado)
                {
                    int subZoneApagadoNumber = 0;
                    foreach (SubZona s in zonaVaciado.Children)
                    {
                        int diferenciaEjeY = heightRampa - (s.Fin.Y - s.Inicio.Y);

                        if (s.tempMatrix != null)
                        {
                            int colWidth = (int)((s.Fin.X - s.Inicio.X) / s.Columnas) + 1;
                            int filHeigth = (int)((s.Fin.Y - s.Inicio.Y) / s.Filas) + 1;

                            int columnaAnterior = 0;

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
                                        c = Color.Green;
                                    if (fila == zonaVaciado.CoolingPoint.X && columna == zonaVaciado.CoolingPoint.Y && zonaVaciado.CoolingSubZone == subZoneApagadoNumber && zonaVaciado.Emptying)
                                        c = Color.Blue;

                                    if (columna != columnaAnterior)
                                        c = Color.DarkBlue;

                                    this.bitmapRejillas.SetPixel(offsetX + x, offsetY + y, c);

                                    columnaAnterior = columna;

                                }//for x
                            }//for y
                        }//tempMatrix != null
                        offsetX += s.Fin.X - s.Inicio.X;
                        subZoneApagadoNumber++;
                    }//foreach subzona
                }//FOREACH zona

                //Dibujar separación entre zonas de apagado
                borderX = 0;

                for (int i = 0; i < this._system.ZonasVaciado.Count; i++)
                {
                    for(int j = 0; j < this._system.ZonasVaciado[i].Children.Count; j++)
                    {
                        borderX += this._system.ZonasVaciado[i].Children[j].Fin.X - this._system.ZonasVaciado[i].Children[j].Inicio.X;
                    }//for

                    for (int y = 0; y < (int)(heightRampa * 0.1); y++)
                    {
                        if (inicio + borderX < bitmapCuadrados.Width)
                        {
                            this.bitmapRejillas.SetPixel(inicio + borderX - 1, offsetY + y, Color.Black);
                            this.bitmapRejillas.SetPixel(inicio + borderX, offsetY + y, Color.Black);
                        }
                    }//for
                }//for


                #endregion

                //LanzarEvento
                if (this.ThermoCamImgCuadradosGenerated != null)
                {
                    this.ThermoCamImgCuadradosGenerated(this, new ThemoCamImgCuadradosArgs()
                    {
                        ImagenRampa = bitmapCuadrados,
                        ImagenRejillas = bitmapRejillas
                    });
                }//if alguien suscrito
                
                //bitmapCuadrados.Dispose();
            }//IF WIDHT AND HEIGHT > 0
        }
    }
}
