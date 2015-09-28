using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using OPC;

namespace ThermoVision.Models
{
    [Serializable]
    public class Sistema : ISerializable
    {
        #region "Variables"

        List<Zona>          _zonas;
        List<ThermoCam>     _thermoCams;

        string              _OPCServerName;
        OPCClient           _OPCClient;

        string              _path;

        public Zona         selectedZona;

        //Variables sincronización
        bool                OPCWritting;

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
        public bool             OPCWritten                        // -rw 
        {
            get;
            set;
        }

        #endregion

        #region "Constructores"

        public Sistema()                                                   
        {
            this._zonas      = new List<Zona>();
            this._thermoCams = new List<ThermoCam>();

            this.OPCWritten = true;
        }
        protected Sistema(SerializationInfo info, StreamingContext ctxt)   
        {
            this._zonas         = (List<Zona>)      info.GetValue("Zonas",         typeof(List<Zona>));
            this._thermoCams    = (List<ThermoCam>) info.GetValue("ThermoCams",    typeof(List<ThermoCam>));
            this.selectedZona   = (Zona)            info.GetValue("SelectedZona",  typeof(Zona));
            this._OPCServerName = (string)          info.GetValue("OPCServerName", typeof(string));
            this._path          = (string)          info.GetValue("Path",          typeof(string));
            this.selectedZona   = null;

            this.OPCWritten = true;
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
                foreach (SubZona t in z.Children)
                {
                    if(t.ThermoParent != null)
                        t.ThermoParent.SubZonas.Remove(t);
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

        void t_ThermoCamImgReceived(object sender, Enumeraciones.ThermoCamImgArgs e)
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


            if (this.OPCWritten == false && this.OPCWritting == false)
            {
                System.Threading.Thread _thread = new System.Threading.Thread(new System.Threading.ThreadStart(escribirOPC));
                _thread.IsBackground = true;
                _thread.Priority = System.Threading.ThreadPriority.Highest;
                _thread.Name = "Escribir OPC";
                _thread.Start();
            }
        }
        void escribirOPC()
        {
            //TODOS LOS VALORES SE HAN RECIBIDO
            //Escrbir variables en servidor OPC

            if (this.OPCClient != null)
            {
                this.OPCWritting = true;

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


                        //this.OPCClient.Writte(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre, "Max", (int)s._maxTemp);
                        //this.OPCClient.Writte(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre, "Min", (int)s._minTemp);
                        //this.OPCClient.Writte(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre, "Mean", (int)s._meanTemp);

                        if (s.tempMatrix != null)
                        {
                            for (int x = 0; x < s.tempMatrix.GetLength(0); x++)
                            {
                                for (int y = 0; y < s.tempMatrix.GetLength(1); y++)
                                {
                                    OPCGroupValues groupSubZone = new OPCGroupValues(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre + ".MAX");

                                    groupSubZone.Items.Add(new OPCItemValue("[" + y + "," + x + "]", (int)s.tempMatrix[x, y].max));
                                    groupSubZone.Items.Add(new OPCItemValue("[" + y + "," + x + "]", (int)s.tempMatrix[x, y].min));
                                    groupSubZone.Items.Add(new OPCItemValue("[" + y + "," + x + "]", (int)s.tempMatrix[x, y].mean));

                                    this._OPCClient.WriteAsync(groupSystem);

                                    //this.OPCClient.Writte(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre + ".MAX", "[" + y + "," + x + "]", (int)s.tempMatrix[x, y].max);
                                    //this.OPCClient.Writte(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre + ".MIN", "[" + y + "," + x + "]", (int)s.tempMatrix[x, y].min);
                                    //this.OPCClient.Writte(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre + ".MEAN", "[" + y + "," + x + "]", (int)s.tempMatrix[x, y].mean);
                                }
                            }
                        }
                    }//FOREACH SUBZONA
                }//FOREACH ZONA
                this.OPCWritting = false;
            }//IF OPCClient != null

            //REINICIAR LAS VARIABLES QUE INDICAN LA RECEPCIÓN DE LA IMAGEN DE CADA CAMARA
            foreach (ThermoCam t in this._thermoCams)
            {
                t.ImagenRecibida = false;
            }
            //System.Threading.Thread.Sleep(1000);
            this.OPCWritten = true;
        }
        #endregion

        #endregion
    }
}