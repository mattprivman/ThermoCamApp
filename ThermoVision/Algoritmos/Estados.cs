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

        public float widthFinalRampa = 1;
        public float heightFinalRampa = 1;

        public float widthFinalTrampilla = 1;
        public float heightFinalTrampilla = 1;

        Bitmap bitmapCuadrados;
        Bitmap bitmapRejillas;

        Rampa _system;
        #endregion

        #region "Delegados"
        public delegate void ThermoCamImgCuadradosCallback(object sender, ThemoCamImgCuadradosArgs e);
        
        #endregion

        #region "Eventos"
        public event ThermoCamImgCuadradosCallback  ThermoCamImgCuadradosGenerated;
        #endregion

        public Estados(Rampa s)                                                          
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
            foreach(ZonaApagado z in this._system.Zonas)
            {
                if (z.State != Zona.States.Manual)
                {
                    //getCannonCoordinates(z);

                    switch (z.State)
                    {
                        case Zona.States.Vacio:
                            #region "VACIO"
                            if (hayMaterialEnZona(z))
                                z.ChangeState(Zona.States.Lleno);
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
                                        hayMaterialEnZona(zVaciado);

                                }//if
                                else
                                {
                                    this._system.noHayQueEnfriar(z);
                                }
                            }//if 
                            else
                            {
                                this._system.noHayQueEnfriar(z);
                                z.ChangeState(Zona.States.Vacio);
                            }
                            break;
                            #endregion
                        case Zona.States.Enfriando:
                            #region "ENFRIANDO"
                            foreach (Zona zVaciado in z.zonasContenidas)
                                if (zVaciado.State != Zona.States.Manual)
                                    zVaciado.State = Zona.States.Enfriando;

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
                                foreach (ZonaVaciado zVaciado in z.zonasContenidas)
                                {
                                    if (zVaciado.State != Zona.States.Manual)
                                    {
                                        //Comprobar que no haya ninguna zona enfriando ni con puntos calientes y vaciar
                                        if (zVaciado.zonasContenidas.Where((x) => x.State == Zona.States.Enfriando || x.State == Zona.States.Manual).Count() == 0 &&
                                            /*z.zonasContenidas.Where((x) => x.State == Zona.States.Enfriando).Count() == 0 &&*/
                                            !hayPuntosCalientesEnZona(zVaciado) &&
                                            this._system.ZonasVaciado.Where((x) => x.State == Zona.States.Vaciando).Count() == 0 &&
                                            this._system.Zonas.Where((x) => x.State == Zona.States.Vaciando).Count() == 0)
                                        {
                                            z.ChangeState(Zona.States.Vaciando);
                                            zVaciado.State = Zona.States.Vaciando;
                                            this._system.vaciarZona(zVaciado);
                                        }
                                       
                                    }
                                }
                            }//else

                            break;
                            #endregion
                        case Zona.States.Vaciando:
                            #region "VACIANDO"

                            bool todoVacio = true;

                            foreach (ZonaVaciado zVaciado in z.zonasContenidas)
                            {
                                if (!zVaciado.Emptying)
                                {
                                    if(zVaciado.State != Zona.States.Manual)
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
                                foreach (ZonaVaciado zVaciado in z.zonasContenidas)
                                {
                                    if (zVaciado.State != Zona.States.Manual)
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
                                            zVaciado.State != Zona.States.Manual &&
                                            this._system.ZonasVaciado.Where(x => x.State == Zona.States.Vaciando).Count() == 0 &&
                                            zVaciado.zonasContenidas.Where((x) => x.State == Zona.States.Enfriando || x.State == Zona.States.Manual).Count() == 0 &&
                                            !hayPuntosCalientesEnZona(zVaciado) &&
                                            /*z.zonasContenidas.Where((x) => x.State == Zona.States.Enfriando).Count() == 0 &&*/
                                            this._system.Zonas.Where((x) => x.State == Zona.States.Vaciando).Count() == 1)
                                        {
                                            zVaciado.State = Zona.States.Vaciando;
                                            this._system.vaciarZona(zVaciado);
                                        }
                                    }
                                }
                            }

                            if (todoVacio == true && z.zonasContenidas.Where(x => x.State == Zona.States.Vaciando).Count() == 0)
                                z.ChangeState(Zona.States.Vacio);

                            break;
                            #endregion
                        case Zona.States.Manual:
                            #region "MANUAL"
                            hayMaterialEnZona(z);
                            hayPuntosCalientesEnZona(z);

                            foreach (ZonaVaciado zVaciado in this._system.ZonasVaciado)
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
                }//if zona not manual
                //else
                //{
                //    if (z.State != Zona.States.Enfriando)
                //        z.triggerStateChangedEvent(Zona.States.Enfriando);

                //    foreach (Zona zVaciado in z.zonasContenidas)
                //        if(zVaciado.State != Zona.States.Manual)
                //            zVaciado.State = Zona.States.Enfriando;
                //    //getCannonCoordinates(z);
                //}
            }
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

        //private bool isZonaCooling(Zona z)                                                 
        //{
        //    bool value = false;

        //    try
        //    {
        //        object res = this._system.OPCClient.ReadSync(this._system.Path + ".RAMPAS.APAGADO." + z.Nombre, "Cooling");

        //        if (res is bool)
        //            value = (bool)res;
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }

        //    return value;
        //}
        //private bool isEmptying(Zona z)                                                    
        //{
        //    bool value = false;

        //    try
        //    {
        //        object res = this._system.OPCClient.ReadSync(this._system.Path + ".RAMPAS.VACIADO." + z.Nombre, "Emptying");

        //        if (res is bool)
        //            value = (bool)res;
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }

        //    return value;
        //}

        public void crearImagenCuadrados(List<ZonaApagado> zonasApagado, List<ZonaVaciado> zonasVaciado) 
        {
            lock ("lockRejilla")
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

                float escalaX = widthFinalRampa / widthRampa;
                float escalaY = heightFinalRampa / heightRampa;

                float escalaXTramp = widthFinalTrampilla / widthRejilla;
                float escalaYTramp = heightFinalTrampilla / (int)(heightRampa +
                                        heightRampa * 0.05); 

                //int width = (widthRampa > widthRejilla) ? widthRampa : widthRejilla;      //////// 1
                int width = (int)(widthRampa * escalaX);

                if (widthRampa > 0 && heightRampa > 0)
                {
                    int height = (int)((heightRampa +
                                        heightRampa * 0.05) * escalaY);      //margen entre la rampa y las rejillas;

                    int heightReji = (int)(heightRampa +
                                        heightRampa * 0.05);      //margen entre la rampa y las rejillas;

                    bitmapCuadrados = new Bitmap(width, height);
                    Color c;
                    bitmapRejillas = new Bitmap((int)(widthRejilla * escalaXTramp), (int)heightFinalTrampilla);

                    float offsetX = 0;
                    int inicio = 0;

                    //Comprobar si la rejilla es más ancha que la rampa
                    if (widthRejilla > widthRampa)
                    {
                        offsetX = widthRejilla - widthRampa;
                        inicio = (int) offsetX;
                    }

                    offsetX = 0;    /////////////////////////////////////////////////////////////// 1

                    //Dibujar rampa     /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    #region rampa
                    foreach (ZonaApagado z in zonasApagado)
                    {
                        int subZoneNumber = 0;
                        foreach (SubZona s in z.Children)
                        {
                            if (s.tempMatrix != null)
                            {
                                int diferenciaEjeY = heightRampa - (s.Fin.Y - s.Inicio.Y);

                                float colWidth = (float)((float)((float)s.Fin.X - (float)s.Inicio.X) / (float)s.Columnas) * escalaX ;
                                float filHeigth = (float)((float)((float)s.Fin.Y - (float)s.Inicio.Y) / (float)s.Filas) * escalaY;

                                int columnaAnterior = 0;
                                int filaAnterior = 0;

                                for (int y = 0; y < (s.Fin.Y - s.Inicio.Y) * escalaY; y++)
                                {
                                    for (int x = 0; x < (s.Fin.X - s.Inicio.X) * escalaX; x++)
                                    {
                                        c = Color.LightGray;

                                        //Calcular a que división pertenece estas coordenadas x e y
                                        int columna = (int)((float)x/ colWidth);
                                        int fila = (int)((float)y / filHeigth);

                                        if (s.tempMatrix[fila, columna].hayMaterial)
                                            c = Color.FromArgb(0x3D, 0x3C, 0x3C);
                                        if (s.tempMatrix[fila, columna].estaCaliente)
                                            c = Color.Orange;
                                        if (fila == z.CoolingPoint.X && columna == z.CoolingPoint.Y && z.CoolingSubZone == subZoneNumber && z.Valvula)
                                            c = Color.Blue;
                                        if (columna != columnaAnterior)
                                            c = Color.DarkGray;
                                        if (fila != filaAnterior)
                                        {
                                            c = Color.DarkGray;

                                            for (; x < (s.Fin.X - s.Inicio.X) * escalaX; x++)
                                            {
                                                if(offsetX + x < this.bitmapCuadrados.Width)
                                                    bitmapCuadrados.SetPixel((int)offsetX + x, diferenciaEjeY + y, c);
                                            }
                                        }
                                        else
                                            if (offsetX + x < this.bitmapCuadrados.Width)
                                                bitmapCuadrados.SetPixel((int)offsetX + x, diferenciaEjeY + y, c);

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
                                            this.bitmapCuadrados.SetPixel((int)offsetX + x, y, c);
                                        }//for x
                                    }//for y
                                }//if altura

                                //Dibujar valores temperatura
                                Brush b = Brushes.Black;

                                for (int i = 0; i < s.tempMatrix.GetLength(0); i++)
                                {
                                    int filas = s.tempMatrix.GetLength(0);  //4
                                    int col = s.tempMatrix.GetLength(1);    //6
                                    for (int j = 0; j < s.tempMatrix.GetLength(1); j++)
                                    {
                                        int x = (int)(j * colWidth + colWidth / 2 - colWidth / 4);
                                        int y = (int)(i * filHeigth + filHeigth / 2 - filHeigth / 4);

                                        using (Graphics g = Graphics.FromImage(bitmapCuadrados))
                                        {
                                            if (!(s.tempMatrix[i, j].max == 0))
                                            {
                                                if (s.tempMatrix[i, j].hayMaterial || s.tempMatrix[i, j].estaCaliente)
                                                    b = Brushes.White;
                                                else
                                                    b = Brushes.Black;

                                                g.DrawString(s.tempMatrix[i, j].max.ToString("0"), new Font("Arial", 10), b, new PointF(offsetX + x, y));
                                            }
                                        }
                                    }
                                }

                                offsetX += (float)((s.Fin.X - s.Inicio.X) * escalaX);

                            }//IF tempMatrix != null
                            subZoneNumber++;
                        }//FOREACH SUBZONA

                        //Dibujar coordenadas del cañon
                        if (true)
                        {
                            int y0 = (int) ((heightRampa +
                                heightRampa * 0.05 / 2) * escalaY);

                            int x0 = ((int)offsetX - (int)(z.Width / 2 * escalaX));

                            //Calcular coordenadas del cañon en la imagen

                            int ancho = 0;
                            int ColWidth = 0;
                            for (int i = 0; i < z.Children.Count; i++)
                            {
                                if (i < z.CoolingSubZone)
                                    ancho += z.Children[i].Fin.X - z.Children[i].Inicio.X;
                                if (i == z.CoolingSubZone)
                                {
                                    ColWidth = (int)(((z.Children[i].Fin.X - z.Children[i].Inicio.X) / z.Children[i].Columnas) * escalaX);
                                    break;
                                }
                            }

                            int x1 = (((int)offsetX - (int)(z.Width * escalaX) + (int)(ancho * escalaX) + z.CoolingPoint.Y * ColWidth + ColWidth / 2));

                            int diferenciaEjeY = (int)((heightRampa - (z.Children[z.CoolingSubZone].Fin.Y - z.Children[z.CoolingSubZone].Inicio.Y)) * escalaY);
                            int FilHeight = (int)(((z.Children[z.CoolingSubZone].Fin.Y - z.Children[z.CoolingSubZone].Inicio.Y) / z.Children[z.CoolingSubZone].Filas) * escalaY);
                            int y1 = (diferenciaEjeY + z.CoolingPoint.X * FilHeight + FilHeight / 2);

                            using (Graphics g = Graphics.FromImage(bitmapCuadrados))
                            {
                                g.DrawLine(new Pen(Color.DarkBlue, 4), new Point(x0, y0), new Point(x1, y1));
                            }
                        }

                    }//FOREACH ZONA

                    int borderX = 0;

                    for (int i = 0; i < this._system.Zonas.Count - 1; i++)
                    {
                        for (int j = 0; j < this._system.Zonas[i].Children.Count; j++)
                        {
                            borderX += this._system.Zonas[i].Children[j].Fin.X - this._system.Zonas[i].Children[j].Inicio.X;
                        }

                        //Dibujar separacion de rejilla
                        for (int y = 0; y < heightRampa * escalaY; y++)
                        {
                            try
                            {
                                bitmapCuadrados.SetPixel((int)(borderX * escalaX), y, Color.Black);
                                bitmapCuadrados.SetPixel((int)(borderX * escalaX - 1), y, Color.Black);
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
                        inicio = (int)offsetX;
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
                                int colWidth = (int)((((s.Fin.X - s.Inicio.X) / s.Columnas) + 1) * escalaXTramp);
                                int filHeigth = (int)((((s.Fin.Y - s.Inicio.Y) / s.Filas) + 1) * escalaYTramp);

                                int columnaAnterior = 0;

                                for (int y = 0; y < bitmapRejillas.Height; y++)
                                {
                                    for (int x = 0; x < (s.Fin.X - s.Inicio.X) * escalaXTramp; x++)
                                    {
                                        c = Color.LightGray;

                                        //Calcular a que división pertenece estas coordenadas x e y
                                        int columna = x / colWidth;
                                        int fila = 0;

                                        if (s.tempMatrix[fila, columna].hayMaterial && !s.tempMatrix[fila, columna].estaCaliente)
                                            c = Color.FromArgb(0x3D, 0x3C, 0x3C);
                                        if (s.tempMatrix[fila, columna].estaCaliente)
                                            c = Color.FromArgb(0x3D, 0x3C, 0x3C);
                                        if(s.tempMatrix[fila, columna].activo)
                                            c = Color.White;
                                        //if (fila == zonaVaciado.CoolingPoint.X && columna == zonaVaciado.CoolingPoint.Y && zonaVaciado.CoolingSubZone == subZoneApagadoNumber && zonaVaciado.Emptying)
                                        //    c = Color.White;

                                        if (columna != columnaAnterior)
                                            c = Color.DarkGray;

                                        if(offsetX + x < this.bitmapRejillas.Width)
                                            this.bitmapRejillas.SetPixel((int)offsetX + x, offsetY + y, c);

                                        columnaAnterior = columna;

                                    }//for x
                                }//for y
                            }//tempMatrix != null
                            offsetX += (float)((s.Fin.X - s.Inicio.X) * escalaXTramp);

                            for (int y = 0; y < this.bitmapRejillas.Height; y++)
                            {
                                if (offsetX - 1 < this.bitmapRejillas.Width)
                                    this.bitmapRejillas.SetPixel((int)offsetX - 1, y, Color.DarkGray);
                            }

                            subZoneApagadoNumber++;
                        }//foreach subzona

                        for (int y = 0; y < this.bitmapRejillas.Height; y++)
                        {
                            if (offsetX - 1 < this.bitmapRejillas.Width)
                            {
                                this.bitmapRejillas.SetPixel((int)offsetX - 1, y, Color.Black);
                                this.bitmapRejillas.SetPixel((int)offsetX - 2, y, Color.Black);
                            }
                        }

                    }//FOREACH zona

                    ////Dibujar separación entre zonas de apagado
                    //borderX = 0;

                    //for (int i = 0; i < this._system.ZonasVaciado.Count; i++)
                    //{
                    //    for (int j = 0; j < this._system.ZonasVaciado[i].Children.Count; j++)
                    //    {
                    //        borderX += this._system.ZonasVaciado[i].Children[j].Fin.X - this._system.ZonasVaciado[i].Children[j].Inicio.X;
                    //    }//for

                    //    borderX = (int)(borderX * escalaXTramp);
                    //    inicio = (int) (inicio * escalaXTramp);

                    //    for (int y = 0; y < bitmapRejillas.Height; y++)
                    //    {
                    //        if (inicio + borderX < bitmapRejillas.Width)
                    //        {
                    //            this.bitmapRejillas.SetPixel(inicio + borderX - 1, offsetY + y, Color.Black);
                    //            this.bitmapRejillas.SetPixel(inicio + borderX, offsetY + y, Color.Black);
                    //        }
                    //    }//for
                    //}//for


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
}
