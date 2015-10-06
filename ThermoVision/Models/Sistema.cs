using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using OPC;
using ThermoVision.Algoritmos;
using ThermoVision.Tipos;

namespace ThermoVision.Models
{
    [Serializable]
    public class Sistema : ISerializable
    {
        #region "Variables"

        string              _mode               = "";
        //Zonas predefinidas
        public string       zonaApagado         = "ZonaApagado";
        public string       zonaVaciado         = "ZonaVaciado";

        const int           limitTemp           =  20;

        public Estados      estados;

        List<Zona>          _zonas;
        List<ThermoCam>     _thermoCams;
        List<Cannon>        _cannons;

        string              _OPCServerName;
        OPCClient           _OPCClient;

        string              _path;

        public Zona         selectedZona;

        //Variables sincronización
        public bool         checkingStates;
        public bool         accessingTempElements;

        //Variable configuración
        public bool         modoConfiguracion;  // En el modo de configuración no se aplican los algoritmos ni se envian las tramas OPC al PLC

        #endregion

        #region "Eventos"

        public event EventHandler zonasListChanged;

        #endregion

        #region "Propiedades"

        public List<Zona>       Zonas                             // -r  
        {
            get
            {
                return this._zonas;
            }
        }
        public List<ThermoCam>  ThermoCams                        // -r  
        {
            get
            {
                return this._thermoCams;
            }
        }
        public List<Cannon>     Cannons                           // -rw 
        {
            get
            {
                return this._cannons;
            }
            set
            {
                this._cannons = value;
            }
        }
        public string           OPCServerName                     // -rw 
        {
            get
            {
                return this._OPCServerName;
            }
            set
            {
                this._OPCServerName = value;
            }
        }
        public OPCClient        OPCClient                         // -rw 
        {
            get
            {
                return this._OPCClient;
            }
            set
            {
                this._OPCClient = value;
            }
        }
        public string           Path                              // -rw 
        {
            get
            {
                return this._path;
            }
            set
            {
                this._path = value;
            }
        }
        public string           Mode                              // -rw 
        {
            get
            {
                return this._mode;
            }
            set
            {
                this._mode = value;
            }
        }

        #endregion

        #region "Constructores"

        public Sistema()                                                   
        {
            this.estados = new Estados(limitTemp, this);

            this._zonas      = new List<Zona>();
            this._thermoCams = new List<ThermoCam>();
            this._cannons    = new List<Cannon>();
        }
        protected Sistema(SerializationInfo info, StreamingContext ctxt)   
        {
            try
            {
                this._zonas         = (List<Zona>)      info.GetValue("Zonas",          typeof(List<Zona>));
                this._thermoCams    = (List<ThermoCam>) info.GetValue("ThermoCams",     typeof(List<ThermoCam>));
                this.selectedZona   = (Zona)            info.GetValue("SelectedZona",   typeof(Zona));
                this._OPCServerName = (string)          info.GetValue("OPCServerName",  typeof(string));
                this._path          = (string)          info.GetValue("Path",           typeof(string));
                this._mode          = (string)          info.GetValue("Mode",           typeof(string));
                this._cannons       = (List<Cannon>)    info.GetValue("Cannons",        typeof(List<Cannon>));
                this.selectedZona   = null;

                this.estados = new Estados(limitTemp, this);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region "Métodos públicos"

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctxt) 
        {
            info.AddValue("Zonas",         this._zonas);
            info.AddValue("ThermoCams",    this._thermoCams);
            info.AddValue("SelectedZona",  this.selectedZona);
            info.AddValue("OPCServerName", this._OPCServerName);
            info.AddValue("Path",          this._path);
            info.AddValue("Mode",          this._mode);
            info.AddValue("Cannons",       this._cannons);
        }
        public void conectarClienteOPC()                                                 
        {
            if (this._OPCClient == null)
            {
                this._OPCClient = new OPCClient(this.OPCServerName);
            }
            if(this.OPCServerName != null && this.OPCServerName != "")
            {
                this._OPCClient.Conectar();
                this._OPCClient.Browse();
            }

            foreach (ThermoCam t in this.ThermoCams)
            {
                t.ThermoCamImgReceived += t_ThermoCamImgReceived;
            }
        }
        public void Dispose()                                                            
        {
            if (this._OPCClient != null)
                this._OPCClient.Dispose();

            foreach (ThermoCam t in this._thermoCams)
                t.Dispose();
            this.ThermoCams.Clear();

            foreach (Zona z in this._zonas)
                z.Children.Clear();

            this._zonas.Clear();
        }

        #region "ZONAS"
        public void addZona(Zona z)                 
        {
            lock ("Zonas")
            {
                //Comprobar que no haya una zona con el mismo nombre
                foreach (Zona item in this._zonas)
                {
                    if (item.Nombre == z.Nombre)
                    {
                        throw new Exception("Ya hay una zona con el nombre " + z.Nombre + ".");
                    }
                }

                this._zonas.Add(z);

                if (zonasListChanged != null)
                    zonasListChanged(this, null);
            }
        }       //AÑADIR ZONA
        public void removeZona(Zona z)              
        {
            lock ("Zonas")
            {
                foreach (SubZona s in z.Children)
                {
                    if(s.ThermoParent != null)
                        s.ThermoParent.SubZonas.Remove(s);

                    if (s.Cannon != null)
                        s.Cannon.removeSubZona(s);
                }

                z.Children.Clear();
                this._zonas.Remove(z);

                this.selectedZona = null;

                if (zonasListChanged != null)
                    zonasListChanged(this, null);
            }
        }       //BORRAR ZONA
        public Zona getZona(string Nombre)          
        {
            if (this._zonas.Exists(x => x.Nombre == Nombre))
                return this._zonas.Where(x => x.Nombre == Nombre).First();

            return null;
        }       //DEVOLVER ZONA
        public void selectZona(string name)         
        {
            lock ("Zonas")
            {
                foreach (Zona z in this._zonas)                                             //Deseleccionar todas las zonas
                    z.unSelectSubZonas();

                if (this._zonas.Exists(x => x.Nombre == name))                              //Seleccionar las subzonas de la subzona a activar
                {
                    Zona z = this._zonas.Where(x => x.Nombre == name).First();
                    this.selectedZona = z;
                    z.selectSubZonas();
                }
                else
                {
                    this.selectedZona = null;
                }
            }
        }       //SELECCIONAR ZONA
        #endregion

        #region "THERMOCAMS"
        public void addThermoCam(ThermoCam t)       
        {
            lock("ThermoCams")
            {
                this._thermoCams.Add(t);
                t.Parent                = this;
                t.ThermoCamImgReceived += t_ThermoCamImgReceived;
            }
        }
        public void removeThermoCam(ThermoCam t)    
        {
            lock ("ThermoCams")
            {
                this._thermoCams.Remove(t);
                t.ThermoCamImgReceived -= t_ThermoCamImgReceived;
            }
        }

        void t_ThermoCamImgReceived(object sender, ThermoCamImgArgs e)
        {
            if (this.modoConfiguracion == false)
            {
                if (sender is ThermoCam)
                {
                    ((ThermoCam)sender).ImagenRecibida = true;
                }

                foreach (ThermoCam t in this._thermoCams)
                {
                    if (t.ImagenRecibida == false)
                        return; //No se han recibido todas las imagenes
                }
                //SE HAN RECIBIDO TODAS LAS IMAGENES DE LAS CAMARAS CONECTADAS
                //Procesar zonas

                foreach (Zona z in this._zonas)
                {
                    //Reiniciar variables
                    z._maxTemp = 0F;
                    z._minTemp = 10000F;
                    z._meanTemp = 0D;

                    foreach (SubZona s in z.Children)
                    {
                        //Máximo
                        if (s._maxTemp > z._maxTemp)
                            z._maxTemp = s._maxTemp;
                        //Mínimo
                        if (s._minTemp < z._minTemp)
                            z._minTemp = s._minTemp;
                        //Media
                        z._meanTemp += s._meanTemp / z.Children.Count;
                    }
                }


                if (this.accessingTempElements == false && this.checkingStates == false)
                {
                    this.checkingStates = true;

                    Task t = new Task(() => checkStates());
                    t.Start();
                }
            }
        }

        void checkStates()                  
        {
            if(this._mode == "Rampas")
            {
                // Ejecutar el checkeo de los algoritmos y ejecutar las acciones pertinentes tambien
                this.estados.ejecutarAlgoritmos();
            }
            if(this._mode == "Standart")
            {
                // 
                sendTempMatrixViaOPCStandarMode();
            }

            
            //escribirOPC();
            //if(estados.
            this.checkingStates = false;
        }
        public void sendTempMatrixViaOPCStandarMode()  
        {
            //TODOS LOS VALORES SE HAN RECIBIDO
            //Escrbir variables en servidor OPC

            if (this.OPCClient != null && this.accessingTempElements == false)
            {
                this.accessingTempElements = true;  //BLOQUEO

                foreach (Zona z in this._zonas)
                {
                    OPCGroupValues groupSystem = new OPCGroupValues(this._path + ".TEMPERATURES." + z.Nombre);

                    groupSystem.Items.Add(new OPCItemValue("Max",  (int) z._maxTemp));
                    groupSystem.Items.Add(new OPCItemValue("Min",  (int) z._minTemp));
                    groupSystem.Items.Add(new OPCItemValue("Mean", (int)z._meanTemp));

                    this._OPCClient.WriteAsync(groupSystem);

                    //this._OPCClient.Writte(this._path + ".TEMPERATURES." + z.Nombre, "Max", (int)z._maxTemp);
                    //this._OPCClient.Writte(this._path + ".TEMPERATURES." + z.Nombre, "Min", (int)z._minTemp);
                    //this._OPCClient.Writte(this._path + ".TEMPERATURES." + z.Nombre, "Mean", (int)z._meanTemp);

                    foreach (SubZona s in z.Children)
                    {
                        OPCGroupValues groupZone = new OPCGroupValues(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre);

                        groupZone.Items.Add(new OPCItemValue("Max",  (int) s._maxTemp));
                        groupZone.Items.Add(new OPCItemValue("Min",  (int) s._minTemp));
                        groupZone.Items.Add(new OPCItemValue("Mean", (int) s._meanTemp));

                        this.OPCClient.WriteAsync(groupZone);


                        //this.OPCClient.Writte(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre, "Max", (int)s._maxTemp);
                        //this.OPCClient.Writte(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre, "Min", (int)s._minTemp);
                        //this.OPCClient.Writte(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre, "Mean", (int)s._meanTemp);

                        if (s.tempMatrix != null)
                        {
                            OPCGroupValues groupSubZoneMax  = new OPCGroupValues(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre + ".MAX");
                            OPCGroupValues groupSubZoneMin  = new OPCGroupValues(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre + ".MIN");
                            OPCGroupValues groupSubZoneMean = new OPCGroupValues(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre + ".MEAN");

                            for (int x = 0; x < s.tempMatrix.GetLength(0); x++)
                            {
                                for (int y = 0; y < s.tempMatrix.GetLength(1); y++)
                                {
                                    groupSubZoneMax.Items.Add(new OPCItemValue("[" + y + "," + x + "]", (int)s.tempMatrix[x, y].max));
                                    groupSubZoneMin.Items.Add(new OPCItemValue("[" + y + "," + x + "]", (int)s.tempMatrix[x, y].min));
                                    groupSubZoneMean.Items.Add(new OPCItemValue("[" + y + "," + x + "]", (int)s.tempMatrix[x, y].mean));

                                    //this.OPCClient.Writte(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre + ".MAX", "[" + y + "," + x + "]", (int)s.tempMatrix[x, y].max);
                                    //this.OPCClient.Writte(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre + ".MIN", "[" + y + "," + x + "]", (int)s.tempMatrix[x, y].min);
                                    //this.OPCClient.Writte(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre + ".MEAN", "[" + y + "," + x + "]", (int)s.tempMatrix[x, y].mean);
                                }
                            }

                            this._OPCClient.WriteAsync(groupSubZoneMax);
                            this._OPCClient.WriteAsync(groupSubZoneMin);
                            this._OPCClient.WriteAsync(groupSubZoneMean);

                        }//IF s.tempMatrix != null
                    }//FOREACH SUBZONA
                }//FOREACH ZONA
                this.accessingTempElements = false;  //DESBLOQUEO
            }//IF OPCClient != null

            //REINICIAR LAS VARIABLES QUE INDICAN LA RECEPCIÓN DE LA IMAGEN DE CADA CAMARA
            foreach (ThermoCam t in this._thermoCams)
            {
                t.ImagenRecibida = false;
            }
            //System.Threading.Thread.Sleep(1000);
        }

        public void dibujarEstados()
        {
            estados.crearImagenCuadrados(this.getZona(this.zonaApagado),
                this.getZona(this.zonaVaciado));
        }
        #endregion

        #region "Cañones"

        public void addCannon(string name)
        {
            lock ("SubZonas")
            {
                this._cannons.Add(new Cannon(name));
            }
        }
        public void deleteCannon(string name)
        {
            lock ("SubZonas")
            {
                if (this._cannons.Exists(x => x.Name == name))
                    this._cannons.Remove(this._cannons.Where(x => x.Name == name).First());
            }
        }

        #endregion

        #endregion
    }
}