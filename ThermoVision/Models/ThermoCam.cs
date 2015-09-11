using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using AxCAMCTRLLib;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ThermoVision.Helpers;
using ThermoVision.Tipos;

namespace ThermoVision.Models
{
    [Serializable()]
    public class ThermoCam : ISerializable
    {
        #region VARIABLES

        string                  _name;

        Thread                  tGetImages;                             //THREAD PARA LEER LAS IMAGENES

        AxLVCam                 camara          = new AxLVCam();        //OBJETO DELA CAMARA
        float[]                 lutTable;
        short[,]                imgData;

        //TABLA CONVERSION DE TEMPERATURAS

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// PARAMETROS CONEXION
        CameraType              _camType;
        DeviceType              _devType;
        InterfaceType           _interfaceType;
        string                  _address;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// PARAMETROS IMAGEN

        int                     _width;
        int                     _height;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// PROPIEDADES PROCESAMIENT

        bool                    connected;

        bool                    _rejilla;
        bool                    _configuracionMode;
        bool                    _matrixTemp;

        public List<SubZona>    SubZonas;
        object                  sync            = new object();

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// VARIABLES PROCESAMIENTO
        /////////////////////////////// Variables para procesar las imagenes 
        Bitmap                  bmp;

        float                   maxTemp;
        float                   minTemp;

        uint                    Val;
        uint                    pixel;

        uint                    maxZonaNoUtil;
        uint                    minZonaNoUtil;
        uint                    rangeZonaNoUtil;

        uint                    maxZonaUtil;
        uint                    minZonaUtil;
        uint                    rangeZonaUtil;

        List    <Color>         colorPalette;

        #endregion

        #region PROPIEDADES

        //CAMARA
        public string Nombre
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;

                if (this.ThermoCamNameChanged != null)
                    ThermoCamNameChanged(this, null);
            }
        }

        //CONEXIÓN
        public bool   Conectado                    // -r  
        {
            get
            {
                return this.connected;
            }
        }
        public string Address                      // -rw 
        {
            get
            {
                return this._address;
            }
            set
            {
                this._address = value;

                if (this.ThermoCamAddressChanged != null)
                    ThermoCamAddressChanged(this, null);
            }
        }

        public bool  Rejilla                       // -w  
        {
            get
            {
                return this._rejilla;
            }
            set
            {
               this._rejilla = value;
            }
        }
        public bool  ConfiguracionMode             // -w  
        {
            set
            {
                this._configuracionMode = value;
            }
        }
        public bool  MatrixTemp                    // -w  
        {
            set
            {
                this._matrixTemp = value;
            }
        }

        // Ancho y alto de la imagen
        public int   Width                         // -r  
        {
            get
            {
                return _width;
            }
        }
        public int   Heigth                        // -r  
        {
            get
            {
                return _height;
            }
        }

        #endregion

        #region DELEGADOS

        public delegate void ThermoCamImgEventCallback(object sender, ThermoCamImgArgs e);
        public delegate void ThermoCamEventCallback(object sender, EventArgs e);

        #endregion

        #region EVENTOS

        public event ThermoCamEventCallback     ThermoCamConnected;
        public event ThermoCamEventCallback     ThermoCamDisConnected;
        public event ThermoCamImgEventCallback  ThermoCamImgReceived;
        public event ThermoCamEventCallback     DivisionesChanged;
        public event ThermoCamEventCallback     ThermoCamNameChanged;
        public event ThermoCamEventCallback     ThermoCamAddressChanged;

        #endregion

        #region "CONSTRUCTORES"

        public ThermoCam(System.Windows.Forms.Form f, CameraType camType, DeviceType devType, InterfaceType interfaceType)
        {
            this._camType = camType;
            this._devType = devType;
            this._interfaceType = interfaceType;

            //Crear nueva lista de zonas
            this.SubZonas = new List<SubZona>();

            ((System.ComponentModel.ISupportInitialize)(camara)).BeginInit();
            camara.Visible = false;
            f.Controls.Add(camara);
            camara.CameraEvent += camara_CameraEvent;
            ((System.ComponentModel.ISupportInitialize)(camara)).EndInit();
        }

        #region "DESERIALIZE"
        public ThermoCam(SerializationInfo info, StreamingContext ctxt)   
        {
            this._name          = (string)          info.GetValue("Nombre"   , typeof(string));
            this._address       = (string)          info.GetValue("Address"  , typeof(string));
            this._camType       = (CameraType)      info.GetValue("CamType"  , typeof(CameraType));
            this._devType       = (DeviceType)      info.GetValue("DevType"  , typeof(DeviceType));
            this._interfaceType = (InterfaceType)   info.GetValue("InterType", typeof(InterfaceType));
            this.SubZonas       = (List<SubZona>)   info.GetValue("SubZonas" , typeof(List<SubZona>));

        }
        public void InitializeForm(System.Windows.Forms.Form f)
        {
            ((System.ComponentModel.ISupportInitialize)(camara)).BeginInit();
            camara.Visible = false;
            f.Controls.Add(camara);
            camara.CameraEvent += camara_CameraEvent;
            ((System.ComponentModel.ISupportInitialize)(camara)).EndInit();
        }
        #endregion

        #endregion

        #region METODOS PUBLICOS

        #region "SERIALIZE"

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)    
        {
            info.AddValue("Nombre"   , this.Nombre);
            info.AddValue("Address"  , this.Address);
            info.AddValue("CamType"  , this._camType);
            info.AddValue("DevType"  , this._devType);
            info.AddValue("InterType", this._interfaceType);
            info.AddValue("SubZonas" , this.SubZonas);
            int a;
            a = 1;
        }

        #endregion

        #region "CONEXIÓN"
        public void Conectar()                  
        {
            if (this._address != "")
            {
                Thread tConectar = new Thread(new ThreadStart(_conectar));
                tConectar.Name = "Thread Conectar";
                tConectar.Start();
            }
        }
        public void Desconectar()               
        {
            try
            {
                if (this.connected)
                    this.camara.Disconnect();
            }
            catch (Exception ex)
            {
                procesarExcepcion(ex, "Desconectar");
            }
        }
        public void Dispose()                   
        {
            try
            {
                this.Desconectar();
                this.camara.Dispose();
            }
            catch (Exception ex)
            {
                procesarExcepcion(ex, "Dispose");
            }
        }

        public void autoAdjust()
        {
            doCameraAction(10);
        }
        public void autoFocus()
        {
            doCameraAction(12);
        }
        public void InternalImageCorrection()
        {
            doCameraAction(8);
        }
        public void ExternalImageCorrection()
        {
            doCameraAction(9);
        }

        #endregion

        #region "SUBZONAS"

        void ThermoCam_DivisionesChanged(object sender, EventArgs e)    
        {
            if (DivisionesChanged != null)
            {
                DivisionesChanged(this, null);
            }
        }

        public void addDivision(SubZona d)        
        {
            lock (this.sync)
            {
                if (this.SubZonas.Exists(x => x.Id == d.Id))
                {
                    //Ya existe, hay que modificarla
                    this.SubZonas.Where(x => x.Id == d.Id).First().Nombre = d.Nombre;
                    this.SubZonas.Where(x => x.Id == d.Id).First().addCoordinates(new Point(d.Inicio.X, d.Inicio.Y),
                        new Point(d.Fin.X, d.Fin.Y));
                    this.SubZonas.Where(x => x.Id == d.Id).First().Filas = d.Filas;
                    this.SubZonas.Where(x => x.Id == d.Id).First().Columnas = d.Columnas;

                    //Trigger event that indicates that a division has been modified
                    if (DivisionesChanged != null)
                    {
                        DivisionesChanged(this, null);
                    }
                }
                else
                {
                    //No existe dicha división, se crea.
                    this.SubZonas.Add(d);

                    //Trigger event that indicates that a division has been added
                    if (DivisionesChanged != null)
                    {
                        DivisionesChanged(this, null);
                    }

                    //this.DivisionesChanged += ThermoCam_DivisionesChanged;
                }
            }
        }
        public void RemoveDivision(int id)        
        {
            lock (this.sync)
            {
                this.SubZonas.Remove(this.SubZonas.Where(x => x.Id == id).First());

                //Reordenar ids
                int index = 0;

                for (int i = 0; i < this.SubZonas.Count; i++)
                {
                    this.SubZonas[i].Id = index;
                    index++;
                }

                //Trigger event that indicates that a division has been removed
                if (DivisionesChanged != null)
                {
                    DivisionesChanged(this, null);
                }
            }
        }

        #endregion

        #endregion

        #region METODOS PRIVADOS

        #region "Conectar"

        private void _conectar()                    
        {
            try
            {
                if (this.connected == false)
                {
                    //CONECTAR PARAMETROS
                    this.camara.Connect((short)this._camType,
                        0, 
                        (short)this._devType,
                        (short)this._interfaceType,
                        this._address);
                }

            }
            catch (Exception ex)
            {
                procesarExcepcion(ex, "Conectar");
            }
        }
        public short doCameraAction(short action)   
        {
            try
            {
                return this.camara.DoCameraAction(action);
            }
            catch (Exception ex)
            {
                procesarExcepcion(ex, "DoCameraAction");
                return -1;
            }
        }

        #endregion

        #region "GET IMAGES"

        private void getImages()                    
        {
            // Generar paleta de colores para reproducir la escala RAINBOW
            this.colorPalette = ColorUtils.getColors();

            while (this.connected == true)
            {

                //try
                //{
                    //Request Image
                    getImage();

                    if (this.imgData != null)
                    {
                        this._width = this.imgData.GetLength(0);
                        this._height = this.imgData.GetLength(1);
                        this.bmp = new Bitmap(this._width, this._height);

                        //Calcular la longitud de cada celda y de cada columna si la rejilla esta definida
                        //if (this._matrixTemp)
                        //{
                        //    if (this._filas != 0 && this._columnas != 0)
                        //    {
                        //        this._heigthPerRow = (this._fin.Y - this._inicio.Y) / this._filas;
                        //        this._widthPerCol = (this._fin.X - this._inicio.X) / this._columnas;

                        //        this._elementsPerCell = this._heigthPerRow * this._widthPerCol;
                        //    }
                        //    this.tempMatrix = new tempElement[this._columnas, this._filas];
                        //    this._matrixDataCell = new dataElement[this._columnas, this._filas];
                        //}

                        procesarImagen();

                        triggerImgReceivedEvent();

                    }
                //}
                //catch (Exception ex)
                //{
                //    // Excepción producida por que el objecto camara ha sido Disposed
                //    if (ex.ToString().Contains("No se puede utilizar un objeto COM que se ha separado de su RCW subyacente."))
                //        return;
                //    else if (ex.ToString().Contains("No se puede evaluar la expresión porque el código está optimizado o existe un marco nativo en la parte superior de la pila de llamadas."))
                //        return;


                //    procesarExcepcion(ex, "getImages");
                //}
            }
        }
        private void getImage()                     
        {
            try
            {
                if (this.connected == true)
                {
                    object data = this.camara.GetImage(0);

                    if (data is short[,])
                        this.imgData = (short[,])data;
                    else
                    {
                        this.imgData = null;
                        if (data is short)
                        {
                            if ((short) data == 14)     //TIMEOUT
                            {
                                //Poner el Framerate más alto
                                object framesObject = this.camara.GetCameraProperty(75);
                                //Subir el timeout 100 ms
                                object actualTimeout = this.camara.GetCameraProperty(93);

                                short status;

                                if (actualTimeout is int)
                                {
                                    status = this.camara.SetCameraProperty(93, (int)actualTimeout + 100);
                                }
                                

                                if (framesObject is double[])
                                {
                                    //CAMBIAR FRAMERATE 
                                    double[] frames = (double[])framesObject;

                                    status = this.camara.SetCameraProperty(43, frames[0]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("No se puede utilizar un objeto COM que se ha separado de su RCW subyacente."))
                    return;

                procesarExcepcion(ex, "GetImage");
            }
        }
        private void getLutTable()                  
        {
            try
            {
                //lock (new object())
                //{
                    object table = this.camara.GetLUT((short)lookUPTable._16BitsPixel);     //Request LUT TABLE con 16 bits por pixel

                    // Comprobar que la tabla sea la esperada
                    if (table is float[])
                        this.lutTable = ((float[])table);
                    else
                        this.lutTable = null;
                //}
            }
            catch (Exception ex)
            {
                procesarExcepcion(ex, "GetLuTTable");
            }
        }
        private void procesarImagen()               
        {
            #region "Dibujar en blanco y negro"

            maxZonaNoUtil = 0x0;             // Reinicializar las variables 
            minZonaNoUtil = 0xFFFF;          // para

            maxZonaUtil  = 0x0;              // encontrar
            minZonaUtil  = 0xFFFF;           // los extremos

            //Buscar los extremos para dibujar la imagen en blanco y negro
            for (int i = 0; i < this.imgData.GetLength(0); i++)
            {
                for (int j = 0; j < this.imgData.GetLength(1); j++)
                {
                    if ((ushort)this.imgData[i, j] > this.maxZonaNoUtil)
                        this.maxZonaNoUtil = (ushort) this.imgData[i, j];

                    if ((ushort)imgData[i, j] < minZonaNoUtil)
                        this.minZonaNoUtil = (ushort) this.imgData[i, j];
                }
            }

            rangeZonaNoUtil = maxZonaNoUtil - minZonaNoUtil;

            //Calcular las temperaturas máximas de la zona útil y de la zona no útil
            //Comprobar que haya zona útil
            if (minZonaNoUtil < maxZonaUtil)
            {
                this.maxTemp = lutTable[maxZonaUtil] - 273.15f;
                this.minTemp = lutTable[minZonaUtil] - 273.15f;
            }
            else
            {
                this.maxTemp = 0;
                this.minTemp = 0;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////// PROCESAR IMAGENES CON ESCALA DE GRISES 
            ////////////////// Se procesa la parte de la imagen no correspondiente a la zona útil con escala de GRISES
            ////////////////// y la parte correspondiente a la zona util se procesa  con escala RAINBOW

            for (int i = 0; i < this._width; i++)
            {
                for (int j = 0; j < this._height; j++)
                {
                    this.Val = (uint) this.imgData[i, j];

                    //////////////////////////////////////////////// NO PERTENECE A LA ZONA ÚTIL O NO ESTA EL MODO DE CONFIGURACIÓN
                    // Escalar valores a escala 255- 0
                    try
                    {
                        this.pixel = ((this.Val - this.minZonaNoUtil) * 255) / this.rangeZonaNoUtil;

                        // Dibujar pixel
                        this.bmp.SetPixel(i, j, System.Drawing.Color.FromArgb((int)pixel, (int)pixel, (int)pixel));
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                }
            }

            #endregion

            #region "MODO CONFIGURACIÓN"
            if (this._configuracionMode)
            {
                //BLOQUEO PARA EVITAR MODIFICACIONES EN LA LISTA DE DIVISIONES MIENTRAS SE EJECUTA ESTE CÓDIGO
                lock (this.sync)
                {
                    //Si esta el modo de configuración activado se dibujan las divisiones seleccionadas con escala Rainbow

                    //Calcular los másximos y los mínimos de todas las divisones
                    int maxValue = 0x0000;
                    int minValue = 0xFFFF;
                    int range;

                    foreach (SubZona d in this.SubZonas)
                    {
                        for (int x = d.Inicio.X; x < d.Fin.X; x++)
                        {
                            for (int y = d.Inicio.Y; y < d.Fin.Y; y++)
                            {
                                if (this.imgData[x, y] > maxValue)
                                    maxValue = this.imgData[x, y];
                                if (this.imgData[x, y] < minValue)
                                    minValue = imgData[x, y];
                            }
                        }
                    }
                    range = maxValue - minValue;

                    // Se dibujan las zonas en escala Raibow
                    foreach (SubZona d in SubZonas)
                    {
                        //Dibujar subzonas con escala rainbow
                        for (int x = d.Inicio.X; x < d.Fin.X; x++)
                        {
                            for (int y = d.Inicio.Y; y < d.Fin.Y; y++)
                            {
                                this.Val = (uint)this.imgData[x, y];

                                Color c = this.colorPalette[(int)(((this.Val - minValue) * 255) / range)];
                                this.bmp.SetPixel(x, y, c);
                            }
                        }

                        //Columnas
                        for (int i = 1; i < d.Columnas; i++)
                        {
                            int x = (i * (d.Fin.X - d.Inicio.X) / d.Columnas) + d.Inicio.X;

                            for (int y = d.Inicio.Y; y < d.Fin.Y; y++)
                            {
                                this.bmp.SetPixel(x, y, Color.Black);
                            }
                        }

                        //Filas
                        for (int i = 1; i < d.Filas; i++)
                        {
                            int y = (i * (d.Fin.Y - d.Inicio.Y) / d.Filas) + d.Inicio.Y;

                            for (int x = d.Inicio.X; x < d.Fin.X; x++)
                            {
                                this.bmp.SetPixel(x, y, Color.Black);
                            }
                        }

                        //Escribir el nombre de la división
                        using (Graphics graphics = Graphics.FromImage(this.bmp))
                        {
                            using (Font arialFont = new Font("Calibri Light", 6))
                            {
                                //test1 = (((j) * this._widthPerCol) + this._inicio.X);
                                //test2 = (((i + 1) * this._heigthPerRow) + this._inicio.Y) - 10;
                                graphics.DrawString(d.Nombre,
                                    arialFont,
                                    Brushes.Black,
                                    new Point(
                                        d.Inicio.X + 2,
                                        d.Inicio.Y + 2));
                            }
                        }
                    }
                }
            }
            #endregion

            #region "Modo funcionamiento"

            //Obtener temperaturas maximas, mínima y media de cada división de cada subzona
            lock (this.sync)                    //Bloqueo para evitar cambios en la coleccion de subzonas
            {
                foreach (SubZona s in this.SubZonas)
                {
                    lock ("lockRejilla")        //Bloqueo para evitar cambios en las rejillas
                    {
                        //Redimensionar matriz de temperaturas
                        if(s.tempMatrix == null || s.tempMatrix.GetLength(0) != s.Filas || s.tempMatrix.GetLength(1) != s.Columnas)
                            s.tempMatrix = new tempElement[s.Filas, s.Columnas];

                        //Reinicializar variables
                        for (int i = 0; i < s.Filas; i++)
                        {
                            for (int j = 0; j < s.Columnas; j++)
                            {
                                s.tempMatrix[i, j].max = this.lutTable[0]; ;
                                s.tempMatrix[i, j].min = this.lutTable[this.lutTable.Length - 1];
                                s.tempMatrix[i, j].mean = 0D;
                            }
                        }

                        int Heigth = (s.Fin.Y - s.Inicio.Y);    //Altura de la subzona
                        int Width = (s.Fin.X - s.Inicio.X);    //Ancho de la subzona

                        int elements = Heigth / s.Filas * Width / s.Columnas;          //Numero de elementos

                        for (int x = s.Inicio.X; x < s.Fin.X; x++)
                        {
                            for (int y = s.Inicio.Y; y < s.Fin.Y; y++)
                            {
                                //Coordenadas de la matriz de temperaturas
                                int fila    = (y - s.Inicio.Y) *    s.Filas / Heigth;
                                int columna = (x - s.Inicio.X) * s.Columnas /  Width;

                                short actualValue = this.imgData[x, y];
                                float actualTemp = this.lutTable[actualValue];

                                float maxTemp = s.tempMatrix[fila, columna].max + 273.15f;
                                float minTemp = s.tempMatrix[fila, columna].min + 273.15f;

                                if (this.lutTable[this.imgData[x, y]] > (s.tempMatrix[fila, columna].max + 273.15f))                    //Maximo
                                    s.tempMatrix[fila, columna].max = this.lutTable[this.imgData[x, y]] - 273.15f;

                                if (this.lutTable[this.imgData[x, y]] < (s.tempMatrix[fila, columna].min + 273.15f))                    //Mínimo
                                    s.tempMatrix[fila, columna].min = this.lutTable[this.imgData[x, y]] - 273.15f;

                                s.tempMatrix[fila, columna].mean += (this.lutTable[this.imgData[x, y]] - 273.15f) / elements;
                            }
                        }
                    }
                }
            }

            #endregion

            //Dibujar rejilla en caso de ser necesario
            //if (this._rejilla)
            //{
            //    //try
            //    //{
            //        Color c = (this._configuracionMode) ? Color.FromArgb(0, 0, 0) : Color.FromArgb(255, 255, 255);
            //        // Lineas verticales
            //        for (int i = 0; i <= this._columnas; i++)
            //        {
            //            for (int y = this._inicio.Y; y < this._fin.Y; y++)
            //            {
            //                if (this._inicio.X + (i * (this._fin.X - this._inicio.X) / this._columnas) < this._width)
            //                    this.bmp.SetPixel(this._inicio.X + (i * (this._fin.X - this._inicio.X) / this._columnas), y, c);
            //                else
            //                    this.bmp.SetPixel((this._inicio.X + (i * (this._fin.X - this._inicio.X)/ this._columnas) - 1), y, c);
            //            }
            //        }

            //        // Lineas horizontales
            //        for (int i = 0; i <= this._filas; i++)
            //        {
            //            for (int x = this._inicio.X; x < this._fin.X; x++)
            //            {
            //                if ((this._inicio.Y + (i * (this._fin.Y - this._inicio.Y) / this._filas) < this._height))
            //                    this.bmp.SetPixel(x, this._inicio.Y + (i * (this._fin.Y - this._inicio.Y) / this._filas), c);
            //                else
            //                    this.bmp.SetPixel(x, (this._inicio.Y + (i * (this._fin.Y - this._inicio.Y) / this._filas) - 1), c);
            //            }
            //        }
            //    //}
            //    //catch (Exception ex)
            //    //{
            //    //    procesarExcepcion(ex, "Poner rejilla");
            //    //}
            //}

            ////GET TEMP MATRIX SI LA CONDICIÓN ES CIERTA
            //if (this._matrixTemp)
            //{
            //    try
            //    {
            //        // Calcular máximo, mínimo, y media por celda
            //        for (int i = 0; i < this._columnas; i++)
            //        {
            //            for (int j = 0; j < this._filas; j++)
            //            {
            //                this._matrixDataCell[i, j].maxTemp = 0x0000;
            //                this._matrixDataCell[i, j].minTemp = 0xFFFF;
            //                this._matrixDataCell[i, j].meanTemp = 0;

            //                for (int y = ((j * this._heigthPerRow) + this._inicio.Y);
            //                    y <  (((this._fin.Y - this._inicio.Y) / this._filas) * (j + 1)) + this._inicio.Y;
            //                    y++)
            //                {
            //                    for (int x = ((i * this._widthPerCol) + this._inicio.X);
            //                        x < (((this._fin.X - this._inicio.X) / this._columnas) * (i + 1)) + this._inicio.X;
            //                        x++)
            //                    {
            //                        //// Maximo

            //                        //if (x < this._width && y < this._height)
            //                        //{

            //                        if (this.imgData[x, y] > this._matrixDataCell[i, j].maxTemp)
            //                        {
            //                            this._matrixDataCell[i, j].maxTemp = (uint)this.imgData[x, y];
            //                        }

            //                        //// Minimo
            //                        test1 = this.imgData[x, y];
            //                        test2 = (int)this._matrixDataCell[i, j].minTemp;

            //                        if (this.imgData[x, y] < this._matrixDataCell[i, j].minTemp)
            //                            this._matrixDataCell[i, j].minTemp = (uint)this.imgData[x, y];
            //                        //// Media
            //                        this._matrixDataCell[i, j].meanTemp += this.lutTable[this.imgData[x, y]] / this._elementsPerCell;
            //                        //}
            //                        //dataStruct.bmp.SetPixel(x, y, Color.FromArgb(0, 0, 0));
            //                    }
            //                }

            //                //COPIAR VALORES A LA TABLA
            //                this.tempMatrix[i, j].maxTemp = this.lutTable[this._matrixDataCell[i, j].maxTemp] - 273.15f;
            //                this.tempMatrix[i, j].minTemp = this.lutTable[this._matrixDataCell[i, j].minTemp] - 273.15f;
            //                this.tempMatrix[i, j].meanTemp = this._matrixDataCell[i, j].meanTemp - 273.15f;

            //                using (Graphics graphics = Graphics.FromImage(this.bmp))
            //                {
            //                    using (Font arialFont = new Font("Calibri Light", 6))
            //                    {
            //                        //test1 = (((j) * this._widthPerCol) + this._inicio.X);
            //                        //test2 = (((i + 1) * this._heigthPerRow) + this._inicio.Y) - 10;
            //                        graphics.DrawString("M: " + this.tempMatrix[i, j].maxTemp,
            //                            arialFont,
            //                            Brushes.White,
            //                            new Point(
            //                                (((i) * this._widthPerCol) + this._inicio.X),
            //                                (((j) * this._heigthPerRow) + this._inicio.Y)));
            //                    }
            //                    using (Font arialFont = new Font("Calibri Light", 6))
            //                    {
            //                        //test1 = (((j) * this._widthPerCol) + this._inicio.X);
            //                        //test2 = (((i + 1) * this._heigthPerRow) + this._inicio.Y) - 10;
            //                        graphics.DrawString("m: " + this.tempMatrix[i, j].minTemp,
            //                            arialFont,
            //                            Brushes.White,
            //                            new Point(
            //                                (((i) * this._widthPerCol) + this._inicio.X),
            //                                (((j) * this._heigthPerRow) + this._inicio.Y + 8)));
            //                    }

            //                    using (Font arialFont = new Font("Calibri Light", 6))
            //                    {
            //                        //test1 = (((j) * this._widthPerCol) + this._inicio.X);
            //                        //test2 = (((i + 1) * this._heigthPerRow) + this._inicio.Y) - 10;
            //                        graphics.DrawString("X: " + this.tempMatrix[i, j].meanTemp,
            //                            arialFont,
            //                            Brushes.White,
            //                            new Point(
            //                                (((i) * this._widthPerCol) + this._inicio.X),
            //                                (((j) * this._heigthPerRow) + this._inicio.Y + 16)));
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        procesarExcepcion(ex, "Temperaturas por celda");
            //    }
            //}
        }

        #endregion

        private void triggerImgReceivedEvent()
        {
            // Desencadenar evento
            if (ThermoCamImgReceived != null)
            {
                ThermoCamImgReceived(this, new ThermoCamImgArgs()
                {
                    Imagen = this.bmp,
                    //tempMatrix = this.tempMatrix,
                    MaxTemp = this.maxTemp,
                    MinTemp = this.minTemp,
                });
            }
        }

        #endregion

        #region EVENTOS

        void camara_CameraEvent(object sender, _DLVCamEvents_CameraEventEvent e)                        
        {
            switch (e.id)
            {
                case 2:
                    //EVENTO CONECTADO
                    try
                    {
                        if (this.connected == false)
                        {
                            this.connected = true;

                            tGetImages = new Thread(new ThreadStart(getImages));
                            tGetImages.Name = "Thread GetImages";
                            tGetImages.IsBackground = true;
                            tGetImages.Priority = ThreadPriority.Highest;
                            tGetImages.Start();
                            
                            if (this.ThermoCamConnected != null)
                                this.ThermoCamConnected(this, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        procesarExcepcion(ex, "Evento conectado");
                    }

                    break;

                case 3:
                    //DEVICE DISCONNECTED
                    this.connected = false;

                    if (this.ThermoCamDisConnected != null)
                        this.ThermoCamDisConnected(this, null);

                    break;

                case 4:
                    //DEVICE CONNECTION BROKEN
                    this.connected = false;

                    if (this.ThermoCamDisConnected != null)
                        this.ThermoCamDisConnected(this, null);

                    break;

                case 5:
                    //DEVICE RECCONECTED FROM BROKEN CONNECTION
                    if (this.connected == false)
                    {
                        this.connected = true;

                        tGetImages = new Thread(new ThreadStart(getImages));
                        tGetImages.Name = "Thread GetImages Reconnected";
                        tGetImages.IsBackground = true;
                        tGetImages.Start();

                        if (this.ThermoCamConnected != null)
                            this.ThermoCamConnected(this, null);
                    }

                    break;

                case 6:
                    //DEVICE IS IN DISCONNECTING PHASE
                    break;

                case 7:
                    //AUTO ADJUST EVENT
                    break;

                case 8:
                    //START OF SHUTTER OPERATION
                    break;

                case 9:
                    //END OF SHUTTER OPERATION
                    break;

                case 10:
                    //LUT Table Uptdated
                    getLutTable();

                    break;

                case 11:
                    //RECORDING CONNECTIONS CHANGED
                    break;

                case 12:
                    //IMAGE CAPTURED
                    break;

                case 13:
                    //ALL CAMERA SETTINGS RETRIEVED
                    break;

                case 14:
                    //FRAME RATE TABLE AVAILABLE (PROPERTY 75)
                    object framesObject = this.camara.GetCameraProperty(75);

                    if (framesObject is double[])
                    {
                        double[] frames = (double[]) framesObject;

                        short status = this.camara.SetCameraProperty(43, frames[0]);
                    }

                    break;

                case 15:
                    //FRAME RATE CHANGED COMPLETED (PROPERTY 43)
                    object frame = this.camara.GetCameraProperty(43);
                    frame.ToString();

                    break;

                case 16:
                    //MEASUREMENT RANGE TABLE AVAILABLE (PROPERTY 46)
                    break;

                case 17:
                    //MEASUREMENT RANE CHANGECOMPLETED (AFTER SETTIG PROPERTY 12)
                    break;

                case 18:
                    //IMAGE SIZE HAS CHANGED

                    break;
            }
        }

        private void procesarExcepcion      (Exception ex, string parent)                               
        {
            ex.ToString();
        }

        #endregion
    }
}