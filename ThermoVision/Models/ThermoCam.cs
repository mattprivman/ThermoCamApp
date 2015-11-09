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
using ThermoVision.Enumeraciones;
using ThermoVision.Tipos;

namespace ThermoVision.Models
{
    [Serializable()]
    public class ThermoCam : ISerializable, IDisposable
    {
        #region VARIABLES

        string                  _name;

        Thread                  tGetImages;                             //THREAD PARA LEER LAS IMAGENES

        AxLVCam                 camara          = new AxLVCam();        //OBJETO DELA CAMARA
        float[]                 lutTable;
        short[,]                imgData;

        bool                    imgReceived;                            //Indica si se ha recibido la imagen

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
        bool                    _rampMode;
        bool                    _matrixTemp;

        public List<SubZona>    SubZonas;

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


        List    <Color>         colorPalette;

        #endregion

        #region PROPIEDADES

        //CAMARA
        public string Nombre                        // -rw 
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
        public bool   Conectado                     // -r  
        {
            get
            {
                return this.connected;
            }
        }
        public string Address                       // -rw 
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

        public bool   Rejilla                       // -w  
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
        public bool   ConfiguracionMode             // -w  
        {
            set
            {
                this._configuracionMode = value;
            }
        }
        public bool   RampMode                      // -w  
        {
            get
            {
                return this._rampMode;
            }
            set
            {
                this._rampMode = value;
            }
        }
        public bool   MatrixTemp                    // -w  
        {
            set
            {
                this._matrixTemp = value;
            }
        }

        // Ancho y alto de la imagen
        public int    Width                         // -r  
        {
            get
            {
                return _width;
            }
        }
        public int    Heigth                        // -r  
        {
            get
            {
                return _height;
            }
        }

        public bool   ImagenRecibida                // -rw 
        {
            get
            {
                return this.imgReceived;
            }
            set
            {
                this.imgReceived = true;
            }
        }

        public Sistema Parent                       // -rw 
        {
            get;
            set;
        }

        #endregion

        #region DELEGADOS

        public delegate void ThermoCamImgEventCallback(object sender, ThermoCamImgArgs e);
        public delegate void ThermoCamEventCallback(object sender, EventArgs e);
        public delegate void ThermoCamStringCallback(object sender, string e);

        #endregion

        #region EVENTOS

        public event ThermoCamImgEventCallback      ThermoCamImgReceived;
        public event ThermoCamEventCallback         ThermoCamNameChanged;
        
        //CONEXIÓN
        public event ThermoCamEventCallback         ThermoCamConnected;
        public event ThermoCamEventCallback         ThermoCamDisConnected;
        public event ThermoCamEventCallback         ThermoCamAddressChanged;
        public event ThermoCamStringCallback        ThermoCamFrameRateChanged;
        public event ThermoCamStringCallback        ThermoCamImgRceceivedTimeoutChanged;

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
            this.Parent         = (Sistema)         info.GetValue("Parent"   , typeof(Sistema));

        }
        public void InitializeForm(System.Windows.Forms.Form f)           
        {
            if (this.camara == null)
            {
                this.camara = new AxLVCam();
            }

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
            info.AddValue("Parent"   , this.Parent);
        }

        #endregion

        #region "CONEXIÓN"
        public void Conectar()                  
        {
            if (this._address != null && this._address != "")
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
                GC.SuppressFinalize(camara);
                this.camara = null;
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
        public void reloadCalibration()         
        {
            this.camara.DoCameraAction(15);
        }

        #endregion

        #region "SUBZONAS"

        public void addSubZona(SubZona s)       
        {
            lock ("SubZonas")
            {
                this.SubZonas.Add(s);
            }
        }
        public void removeSubZona(SubZona s)    
        {
            lock ("SubZonas")
            {
                if (this.SubZonas.Contains(s))
                    this.SubZonas.Remove(s);
            }
        }

        public void selectSubZonaCoordinate(SubZona s, int fila, int columna)
        {
            for (int x = 0; x < s.tempMatrix.GetLength(0); x++)
            {
                for (int y = 0; y < s.tempMatrix.GetLength(1); y++)
                {
                    if (x == fila && y == columna)
                        s.tempMatrix[x, y].selected = true;
                    else
                        s.tempMatrix[x, y].selected = false;
                }//for y
            }//for x
        }
        public void unSelectSubZonaCoordinates()
        {
            foreach (SubZona s in this.SubZonas)
            {
                if (s.tempMatrix != null)
                {
                    //Deseleccioanr todas las coordenadas
                    for (int x = 0; x < s.tempMatrix.GetLength(0); x++)
                    {
                        for (int y = 0; y < s.tempMatrix.GetLength(1); y++)
                        {
                            s.tempMatrix[x, y].selected = false;
                        }//for y
                    }//for x
                }//if tempMatrix not null
            }//foreach subzona
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
        private short doCameraAction(short action)   
        {
            try
            {
                if (this.connected)
                    return this.camara.DoCameraAction(action);
                else
                    return -1;
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

            while (this.connected == true && this.camara != null)
            {
                try
                {
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
                }
                catch (Exception ex)
                {
                    // Excepción producida por que el objecto camara ha sido Disposed
                    if (ex.ToString().Contains("No se puede utilizar un objeto COM que se ha separado de su RCW subyacente."))
                        return;
                    else if (ex.ToString().Contains("No se puede evaluar la expresión porque el código está optimizado o existe un marco nativo en la parte superior de la pila de llamadas."))
                        return;


                    procesarExcepcion(ex, "getImages");
                }
            }
        }
        private void getImage()                     
        {
            try
            {
                if (this.connected == true)
                {
                    object data = null;

                    if(this.camara != null)
                        data = this.camara.GetImage(0);

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

                                    if (ThermoCamImgRceceivedTimeoutChanged != null)
                                    {
                                        ThermoCamImgRceceivedTimeoutChanged(this, ((int) actualTimeout + 100).ToString());
                                    }                                        
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
            #region "Dibujar"

            if (this._configuracionMode == true)
            {
                maxZonaNoUtil = 0x0;             // Reinicializar las variables 
                minZonaNoUtil = 0xFFFF;          // para

                //Buscar los extremos para dibujar la imagen en blanco y negro
                for (int i = 0; i < this.imgData.GetLength(0); i++)
                {
                    for (int j = 0; j < this.imgData.GetLength(1); j++)
                    {
                        if ((ushort)this.imgData[i, j] > this.maxZonaNoUtil)
                            this.maxZonaNoUtil = (ushort)this.imgData[i, j];

                        if ((ushort)imgData[i, j] < minZonaNoUtil)
                            this.minZonaNoUtil = (ushort)this.imgData[i, j];
                    }
                }

                //Definir rango
                rangeZonaNoUtil = maxZonaNoUtil - minZonaNoUtil;

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////// PROCESAR IMAGENES CON ESCALA DE GRISES 
                ////////////////// Se procesa la parte de la imagen no correspondiente a la zona útil con escala de GRISES
                ////////////////// y la parte correspondiente a la zona util se procesa  con escala RAINBOW

                for (int i = 0; i < this._width; i++)
                {
                    for (int j = 0; j < this._height; j++)
                    {
                        this.Val = (uint)this.imgData[i, j];

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
                            if (ex.Message.Contains("No se puede evaluar la expresión porque el código está optimizado o existe un marco nativo en la parte superior de la pila de llamadas."))
                                return;
                            ex.ToString();
                        }//try
                    }//for j
                }//for i
            }
            else
            {
                //Calcular los másximos y los mínimos de todas las divisones
                uint maxValue = 0x0000;
                uint minValue = 0xFFFF;
                uint range;

                for (int x = 0; x < this.imgData.GetLength(0); x++)
                {
                    for (int y = 0; y < this.imgData.GetLength(1); y++)
                    {
                        if (this.imgData[x, y] > maxValue)
                            maxValue = (uint) this.imgData[x, y];
                        if (this.imgData[x, y] < minValue)
                            minValue = (uint) imgData[x, y];
                    }//for y
                }//for x

                range = maxValue - minValue;

                for (int x = 0; x < this._width; x++)
                {
                    for (int y = 0; y < this._height; y++)
                    {
                        this.Val = (uint)this.imgData[x, y];

                        //////////////////////////////////////////////// NO PERTENECE A LA ZONA ÚTIL O NO ESTA EL MODO DE CONFIGURACIÓN
                        // Escalar valores a escala 255- 0

                        this.pixel = (uint) (((this.Val - minValue) * 255) / range);

                        // Dibujar pixel
                        this.bmp.SetPixel(x, y, this.colorPalette[(int)this.pixel]); 
                    }//for j
                }//for i
            }

            #endregion

            #region "MODO CONFIGURACIÓN"

            if (this._configuracionMode)
            {
                //BLOQUEO PARA EVITAR MODIFICACIONES EN LA LISTA DE DIVISIONES MIENTRAS SE EJECUTA ESTE CÓDIGO
                lock ("SubZonas")
                {
                    //Si esta el modo de configuración activado se dibujan las divisiones seleccionadas con escala Rainbow

                    //Calcular los másximos y los mínimos de todas las divisones
                    int maxValue = 0x0000;
                    int minValue = 0xFFFF;
                    int range;

                    foreach (SubZona s in this.SubZonas)
                    {
                        if (s.Selected || this._rampMode)
                        {
                            for (int x = s.Inicio.X; x < s.Fin.X; x++)
                            {
                                for (int y = s.Inicio.Y; y < s.Fin.Y; y++)
                                {
                                    if (this.imgData[x, y] > maxValue)
                                        maxValue = this.imgData[x, y];
                                    if (this.imgData[x, y] < minValue)
                                        minValue = imgData[x, y];
                                }//for y
                            }//for x
                        }//if selected
                    }
                    range = maxValue - minValue;

                    // Se dibujan las zonas en escala Raibow
                    foreach (SubZona s in SubZonas)
                    {
                        if (s.Selected || this._rampMode)
                        {
                            //Dibujar subzonas con escala rainbow
                            for (int x = s.Inicio.X; x < s.Fin.X; x++)
                            {
                                for (int y = s.Inicio.Y; y < s.Fin.Y; y++)
                                {
                                    this.Val = (uint)this.imgData[x, y];

                                    if (range > 0)
                                    {
                                        Color c = this.colorPalette[(int)(((this.Val - minValue) * 255) / range)];
                                        this.bmp.SetPixel(x, y, c);
                                    }//if range
                                }//for y
                            }//for x

                            if (s.Selected)
                            {
                                //Columnas
                                for (int i = 1; i < s.Columnas; i++)
                                {
                                    int x = (i * (s.Fin.X - s.Inicio.X) / s.Columnas) + s.Inicio.X;

                                    for (int y = s.Inicio.Y; y < s.Fin.Y; y++)
                                    {
                                        this.bmp.SetPixel(x, y, Color.Black);
                                    }//For  y
                                }//for columnas

                                //Filas
                                for (int i = 1; i < s.Filas; i++)
                                {
                                    int y = (i * (s.Fin.Y - s.Inicio.Y) / s.Filas) + s.Inicio.Y;

                                    for (int x = s.Inicio.X; x < s.Fin.X; x++)
                                    {
                                        this.bmp.SetPixel(x, y, Color.Black);
                                    }//For x
                                }//For filas

                                //Escribir el nombre de la división
                                using (Graphics graphics = Graphics.FromImage(this.bmp))
                                {
                                    using (Font arialFont = new Font("Calibri Light", 6))
                                    {
                                        //test1 = (((j) * this._widthPerCol) + this._inicio.X);
                                        //test2 = (((i + 1) * this._heigthPerRow) + this._inicio.Y) - 10;
                                        graphics.DrawString(s.Nombre,
                                            arialFont,
                                            Brushes.Black,
                                            new Point(
                                                s.Inicio.X + 2,
                                                s.Inicio.Y + 2));
                                    }//Using font
                                }//Using raphics

                                //Dibujar celda seleccionada

                                if (s.tempMatrix != null)
                                {
                                    int colWidth = ((s.Fin.X - s.Inicio.X) / s.Columnas);
                                    int filHeigth = ((s.Fin.Y - s.Inicio.Y) / s.Filas);

                                    for (int i = 0; i < s.tempMatrix.GetLength(0); i++)
                                    {
                                        for (int j = 0; j < s.tempMatrix.GetLength(1); j++)
                                        {
                                            if (s.tempMatrix[i, j].selected)
                                            {
                                                //Dibujar bordes horizontales
                                                for (int x = j * ((s.Fin.X - s.Inicio.X) / s.Columnas); x < (j + 1) * ((s.Fin.X - s.Inicio.X) / s.Columnas); x++)
                                                {
                                                    this.bmp.SetPixel(s.Inicio.X + x, s.Inicio.Y + i * ((s.Fin.Y - s.Inicio.Y) / s.Filas), Color.White);
                                                    this.bmp.SetPixel(s.Inicio.X + x, s.Inicio.Y + (i + 1) * ((s.Fin.Y - s.Inicio.Y) / s.Filas), Color.White);
                                                }//for x
                                                //Dibujar bordes verticales
                                                for (int y = i * ((s.Fin.Y - s.Inicio.Y) / s.Filas); y < (i + 1) * ((s.Fin.Y - s.Inicio.Y) / s.Filas); y++)
                                                {
                                                    this.bmp.SetPixel(s.Inicio.X + j * ((s.Fin.X - s.Inicio.X) / s.Columnas), s.Inicio.Y + y, Color.White);
                                                    this.bmp.SetPixel(s.Inicio.X + (j + 1) * ((s.Fin.X - s.Inicio.X) / s.Columnas), s.Inicio.Y + y, Color.White);
                                                }//for x
                                            }//if
                                        }//for j
                                    }//for i
                                }//tempMatrix not null
                            }//if selected
                        }//if selected
                    }//foreach subzona
                }//lock subzona
            }//if configuracionmode
            #endregion


            #region "Modo funcionamiento"

            if (this.Parent != null && this.Parent.accessingTempElements == false)
            {
                try
                {
                    this.Parent.accessingTempElements = true;

                    //Obtener temperaturas maximas, mínima y media de cada división de cada subzona
                    lock ("SubZonas")                    //Bloqueo para evitar cambios en la coleccion de subzonas
                    {
                        foreach (SubZona s in this.SubZonas)
                        {
                            lock ("lockRejilla")        //Bloqueo para evitar cambios en las rejillas
                            {
                                //Reinicializar variables de temperatura para cada subZona
                                s._maxTemp = 0;
                                s._minTemp = this.lutTable[this.lutTable.Length - 1];
                                s._meanTemp = 0D;

                                //Redimensionar matriz de temperaturas
                                if (s.tempMatrix == null || s.tempMatrix.GetLength(0) != s.Filas || s.tempMatrix.GetLength(1) != s.Columnas)
                                    s.tempMatrix = new tempElement[s.Filas, s.Columnas];

                                //Reinicializar variables
                                for (int i = 0; i < s.Filas; i++)
                                {
                                    for (int j = 0; j < s.Columnas; j++)
                                    {
                                        s.tempMatrix[i, j].max = this.lutTable[0]; ;
                                        s.tempMatrix[i, j].min = this.lutTable[this.lutTable.Length - 1];
                                        s.tempMatrix[i, j].mean = 0D;
                                    }//for columnas
                                }//for filas

                                int Heigth = (s.Fin.Y - s.Inicio.Y);    //Altura de la subzona
                                int Width = (s.Fin.X - s.Inicio.X);     //Ancho de la subzona

                                int elements = Heigth / s.Filas * Width / s.Columnas;          //Numero de elementos

                                for (int x = s.Inicio.X; x < s.Fin.X; x++)
                                {
                                    for (int y = s.Inicio.Y; y < s.Fin.Y; y++)
                                    {
                                        //Coordenadas de la matriz de temperaturas
                                        int fila = (y - s.Inicio.Y) * s.Filas / Heigth;
                                        int columna = (x - s.Inicio.X) * s.Columnas / Width;

                                        short actualValue = this.imgData[x, y];
                                        float actualTemp = this.lutTable[actualValue];

                                        float maxTemp = s.tempMatrix[fila, columna].max + 273.15f;
                                        float minTemp = s.tempMatrix[fila, columna].min + 273.15f;

                                        //DIVISION

                                        if (this.lutTable[this.imgData[x, y]] > (s.tempMatrix[fila, columna].max + 273.15f))                    //Maximo division
                                            s.tempMatrix[fila, columna].max = this.lutTable[this.imgData[x, y]] - 273.15f;

                                        if (this.lutTable[this.imgData[x, y]] < (s.tempMatrix[fila, columna].min + 273.15f))                    //Mínimo división
                                            s.tempMatrix[fila, columna].min = this.lutTable[this.imgData[x, y]] - 273.15f;

                                        s.tempMatrix[fila, columna].mean += (this.lutTable[this.imgData[x, y]] - 273.15f) / elements;           //Media división

                                        // SUBZONA

                                        if (this.lutTable[this.imgData[x, y]] > (s._maxTemp + 273.15f))                                         //Maximo division
                                            s._maxTemp = this.lutTable[this.imgData[x, y]] - 273.15f;

                                        if (this.lutTable[this.imgData[x, y]] < (s._minTemp + 273.15f))                                         //Mínimo división
                                            s._minTemp = this.lutTable[this.imgData[x, y]] - 273.15f;

                                        s._meanTemp += (this.lutTable[this.imgData[x, y]] - 273.15f) / (Heigth * Width);                        //Media división
                                    }//for y
                                }//for x

                            }//lock lockRejilla
                        }//foreach subzona
                    }//lockSubzonas
                    this.Parent.accessingTempElements = false;
                }
                catch (Exception ex)
                {
                    this.Parent.accessingTempElements = false;
                }
            }//PARENT != null
            #endregion
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
                    MaxTemp = this.maxTemp,
                    MinTemp = this.minTemp,
                });
            }
        }

        #endregion

        #region EVENTOS

        private void start()                            
        {
            
            //try
            //{
            if (this.connected == false)
            {
                this.connected = true;

                if (this.tGetImages != null)
                {
                    try
                    {
                        if(this.tGetImages.IsAlive)
                            this.tGetImages.Abort();
                    }
                    catch (Exception e)
                    { 
                    }
                }

                tGetImages = new Thread(new ThreadStart(getImages));
                tGetImages.Name = "Thread GetImages";
                tGetImages.IsBackground = true;
                tGetImages.Priority = ThreadPriority.Normal;
                tGetImages.Start();

                if (this.ThermoCamConnected != null)
                    this.ThermoCamConnected(this, null);

                object actualTimeout = this.camara.GetCameraProperty(93);

                if (actualTimeout is int)
                {
                    if (this.ThermoCamImgRceceivedTimeoutChanged != null)
                        this.ThermoCamImgRceceivedTimeoutChanged(this, ((int)actualTimeout).ToString());
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    procesarExcepcion(ex, "Evento conectado");
            //}
        }

        void camara_CameraEvent(object sender, _DLVCamEvents_CameraEventEvent e)                        
        {
            switch (e.id)
            {
                case 2:
                   //EVENTO CONECTADO
                    this.start();
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
                    this.start();
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

                    if (frame is double)
                    {
                        if (this.ThermoCamFrameRateChanged != null)
                            this.ThermoCamFrameRateChanged(this, ((double)frame).ToString());
                    }
                    break;

                case 16:
                    //MEASUREMENT RANGE TABLE AVAILABLE (PROPERTY 46)
                    break;

                case 17:
                    //MEASUREMENT RANGE CHANGE COMPLETED (AFTER SETTIG PROPERTY 12)
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