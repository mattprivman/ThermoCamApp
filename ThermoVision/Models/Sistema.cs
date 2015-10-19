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

        const int           limitTemp           =  20;

        public Estados      estados;

        List<Zona>          _zonas;
        List<ThermoCam>     _thermoCams;

        List<Zona>          _zonasVaciado;      //solo para la aplicación de rampas

        string              _OPCServerName;
        OPCClient           _OPCClient;

        string              _path;

        Zona                _selectedZona;

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

        public List<Zona>       Zonas                               // -r  
        {
            get
            {
                return this._zonas;
            }
        }
        public List<Zona>       ZonasVaciado                        // -r  
        {
            get
            {
                return this._zonasVaciado;
            }
        }
        public List<ThermoCam>  ThermoCams                          // -r  
        {
            get
            {
                return this._thermoCams;
            }
        }
        public string           OPCServerName                       // -rw 
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
        public OPCClient        OPCClient                           // -rw 
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
        public string           Path                                // -rw 
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
        public Zona SelectedZona
        {
            get
            {
                return this._selectedZona;
            }
            set
            {
                this._selectedZona = value;
                if (this._selectedZona == null)
                    this.unSelectSubZonas();
            }
        }
        public string           Mode                                // -rw 
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
            this.estados = new Estados(this);

            this._zonas        = new List<Zona>();
            this._thermoCams   = new List<ThermoCam>();
            this._zonasVaciado = new List<Zona>();
        }
        protected Sistema(SerializationInfo info, StreamingContext ctxt)   
        {
            try
            {
                this._zonas         = (List<Zona>)      info.GetValue("Zonas",          typeof(List<Zona>));
                this._zonasVaciado  = (List<Zona>)      info.GetValue("ZonasVaciado",   typeof(List<Zona>));
                this._thermoCams    = (List<ThermoCam>) info.GetValue("ThermoCams",     typeof(List<ThermoCam>));
                this._OPCServerName = (string)          info.GetValue("OPCServerName",  typeof(string));
                this._path          = (string)          info.GetValue("Path",           typeof(string));
                this._mode          = (string)          info.GetValue("Mode",           typeof(string));

                this.estados = new Estados(this);
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
            info.AddValue("ZonasVaciado",  this._zonasVaciado);
            info.AddValue("ThermoCams",    this._thermoCams);
            info.AddValue("OPCServerName", this._OPCServerName);
            info.AddValue("Path",          this._path);
            info.AddValue("Mode",          this._mode);
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


            foreach (Zona z in this._zonasVaciado)
                z.Children.Clear();

            this._zonasVaciado.Clear();

        }

        public void unSelectSubZonas()                  
        {
            foreach (Zona z in this._zonas)
                z.unSelectSubZonas();
            foreach (Zona z in this._zonasVaciado)
                z.unSelectSubZonas();
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
                    if (s.ThermoParent != null)
                        s.ThermoParent.removeSubZona(s);
                }

                z.Children.Clear();
                this._zonas.Remove(z);

                this.SelectedZona = null;

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

                foreach (Zona z in this._zonasVaciado)                                      //Deseleccionar todas las subzonas de apagado
                    z.unSelectSubZonas();

                if (this._zonas.Exists(x => x.Nombre == name))                              //Seleccionar las subzonas de la subzona a activar
                {
                    Zona z = this._zonas.Where(x => x.Nombre == name).First();
                    this.SelectedZona = z;
                    z.selectSubZonas();
                }
                else
                {
                    this.SelectedZona = null;
                }
            }
        }       //SELECCIONAR ZONA
        #endregion

        #region "ZONAS VACIADO"
        public void addZonaVaciado(Zona z)              
        {
            lock ("Zonas")
            {
                //Comprobar que no haya una zona con el mismo nombre
                foreach (Zona item in this._zonasVaciado)
                {
                    if (item.Nombre == z.Nombre)
                    {
                        throw new Exception("Ya hay una zona con el nombre " + z.Nombre + ".");
                    }
                }

                this._zonasVaciado.Add(z);

                if (zonasListChanged != null)
                    zonasListChanged(this, null);
            }
        }       //AÑADIR ZONA
        public void removeZonaVaciado(Zona z)           
        {
            lock ("Zonas")
            {
                foreach (SubZona s in z.Children)
                {
                    if (s.ThermoParent != null)
                        s.ThermoParent.SubZonas.Remove(s);
                }

                z.Children.Clear();
                this._zonasVaciado.Remove(z);

                this.SelectedZona = null;   //Solo se va a poder eliminar si esta seleccionada

                if (zonasListChanged != null)
                    zonasListChanged(this, null);
            }
        }       //BORRAR ZONA
        public Zona getZonaVaciado(string Nombre)       
        {
            if (this._zonasVaciado.Exists(x => x.Nombre == Nombre))
                return this._zonasVaciado.Where(x => x.Nombre == Nombre).First();

            return null;
        }       //DEVOLVER ZONA
        public void selectZonaVaciado(string name)      
        {
            lock ("Zonas")
            {
                foreach (Zona z in this._zonas)                                                     //Deseleccionar todas las zonas
                    z.unSelectSubZonas();

                foreach (Zona z in this._zonasVaciado)                                              //Deseleccionar todas las zonas de apagado
                    z.unSelectSubZonas();

                if (this._zonasVaciado.Exists(x => x.Nombre == name))                              //Seleccionar las subzonas de la subzona a activar
                {
                    Zona z = this._zonasVaciado.Where(x => x.Nombre == name).First();
                    this.SelectedZona = z;
                    z.selectSubZonas();
                }
                else
                {
                    this.SelectedZona = null;
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
        }       //AÑADIR THERMOCAM
        public void removeThermoCam(ThermoCam t)        
        {
            lock ("ThermoCams")
            {
                this._thermoCams.Remove(t);
                t.ThermoCamImgReceived -= t_ThermoCamImgReceived;
            }
        }       //BORRAR THERMOCAM

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

                    Task t = new Task(delegate { checkStates(); });
                    t.Start();
                }
            }
        }

        void checkStates()                              
        {
            try
            {
                if (this._mode == "Rampas")
                {
                    //Ejecutar el checkeo de los algoritmos y ejecutar las acciones pertinentes tambien
                    this.estados.ejecutarAlgoritmos();
                }
                if (this._mode == "Standart")
                {
                    // 
                    sendTempMatrixViaOPCStandarMode();
                }
            }
            catch (Exception ex)
            {
                this.checkingStates        = false;
                this.accessingTempElements = false;
            }

            //escribirOPC();
            //if(estados.
            this.checkingStates = false;
        }
        public void sendTempMatrixViaOPCStandarMode()   
        {
            try
            {
                //TODOS LOS VALORES SE HAN RECIBIDO
                //Escrbir variables en servidor OPC

                if (this.OPCClient != null && this.accessingTempElements == false)
                {
                    this.accessingTempElements = true;  //BLOQUEO

                    foreach (Zona z in this._zonas)
                    {
                        OPCGroupValues groupSystem = new OPCGroupValues(this._path + ".TEMPERATURES." + z.Nombre);

                        groupSystem.Items.Add(new OPCItemValue("Max", (int)z._maxTemp));
                        groupSystem.Items.Add(new OPCItemValue("Min", (int)z._minTemp));
                        groupSystem.Items.Add(new OPCItemValue("Mean", (int)z._meanTemp));

                        this._OPCClient.WriteAsync(groupSystem);

                        //this._OPCClient.Writte(this._path + ".TEMPERATURES." + z.Nombre, "Max", (int)z._maxTemp);
                        //this._OPCClient.Writte(this._path + ".TEMPERATURES." + z.Nombre, "Min", (int)z._minTemp);
                        //this._OPCClient.Writte(this._path + ".TEMPERATURES." + z.Nombre, "Mean", (int)z._meanTemp);

                        foreach (SubZona s in z.Children)
                        {
                            OPCGroupValues groupZone = new OPCGroupValues(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre);

                            groupZone.Items.Add(new OPCItemValue("Max", (int)s._maxTemp));
                            groupZone.Items.Add(new OPCItemValue("Min", (int)s._minTemp));
                            groupZone.Items.Add(new OPCItemValue("Mean", (int)s._meanTemp));

                            this.OPCClient.WriteAsync(groupZone);


                            //this.OPCClient.Writte(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre, "Max", (int)s._maxTemp);
                            //this.OPCClient.Writte(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre, "Min", (int)s._minTemp);
                            //this.OPCClient.Writte(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre, "Mean", (int)s._meanTemp);

                            if (s.tempMatrix != null)
                            {
                                OPCGroupValues groupSubZoneMax = new OPCGroupValues(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre + ".MAX");
                                OPCGroupValues groupSubZoneMin = new OPCGroupValues(this._path + ".TEMPERATURES." + z.Nombre + "." + s.Nombre + ".MIN");
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
            }
            catch (Exception e)
            {
                e.ToString();
            }
            //System.Threading.Thread.Sleep(1000);
        }

        public void enfriarZona(Zona z)                 
        {
            try
            {
                //ACTIVAR LA VARIABLE DE ACTIVACIÓN DEL CAÑON
                this._OPCClient.WriteAsync(this.Path + ".RAMPAS.APAGADO." + z.Nombre, "Activar", true);

                //ENVIAR LAS VARIABLES DE TEMPERATURA DE CADA ZONA
                foreach (SubZona s in z.Children)
                {
                    OPCGroupValues groupSubZoneMax = new OPCGroupValues(this._path + ".RAMPAS.APAGADO." + z.Nombre + "." + s.Nombre);

                    for (int x = 0; x < s.tempMatrix.GetLength(0); x++)
                    {
                        for (int y = 0; y < s.tempMatrix.GetLength(1); y++)
                        {
                            groupSubZoneMax.Items.Add(new OPCItemValue("[" + x + "," + y + "]", (int)s.tempMatrix[x, y].max));
                        }//for
                    }//for

                    this._OPCClient.WriteAsync(groupSubZoneMax);

                }//foreach subzona
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
        public void noHayQueEnfriar(Zona z)             
        {
            try
            {
                //DESACTIVAR LA VARIABLE DE ACTIVACIÓN DEL CAÑÓN
                this._OPCClient.WriteAsync(this.Path + ".RAMPAS.APAGADO." + z.Nombre, "Activar", false);

                ////ENVIAR LAS VARIABLES DE TEMPERATURA DE CADA ZONA
                //foreach (SubZona s in z.Children)
                //{
                //    OPCGroupValues groupSubZoneMax = new OPCGroupValues(this._path + ".RAMPAS.APAGADO." + z.Nombre + "." + s.Nombre);

                //    for (int x = 0; x < s.tempMatrix.GetLength(0); x++)
                //    {
                //        for (int y = 0; y < s.tempMatrix.GetLength(1); y++)
                //        {
                //            groupSubZoneMax.Items.Add(new OPCItemValue("[" + x + "," + y + "]", (int)s.tempMatrix[x, y].max));
                //        }//for
                //    }//for

                //    this._OPCClient.WriteAsync(groupSubZoneMax);
                //}//foreach subzona
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public void vaciarZona(Zona z)                  
        {
            try
            {
                //ACTIVAR LA VARIABLE DE ACTIVACIÓN DEL CAÑON
                this._OPCClient.WriteAsync(this.Path + ".RAMPAS.VACIADO." + z.Nombre, "Activar", true);

                foreach (SubZona s in z.Children)
                {
                    OPCGroupValues groupSubZoneMax = new OPCGroupValues(this._path + ".RAMPAS.VACIADO." + z.Nombre + "." + s.Nombre);

                    for (int x = 0; x < s.tempMatrix.GetLength(0); x++)
                    {
                        for (int y = 0; y < s.tempMatrix.GetLength(1); y++)
                        {
                            groupSubZoneMax.Items.Add(new OPCItemValue("[" + y + "," + x + "]", s.tempMatrix[x, y].hayMaterial));
                        }//for
                    }//for

                    this._OPCClient.WriteAsync(groupSubZoneMax);
                }//foreach
            }
            catch (Exception e)
            {
                e.ToString();
            }

        }
        public void noHAyQueVaciar(Zona z)
        {
            try
            {
                //DESACTIVAR LA VARIABLE DE ACTIVACIÓN DEL CAÑON
                this._OPCClient.WriteAsync(this.Path + ".RAMPAS.VACIADO." + z.Nombre, "Activar", false);
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

        public void dibujarEstados()                    
        {
            estados.crearImagenCuadrados(this.Zonas,
                this.ZonasVaciado);
        }
        #endregion

        #endregion
    }
}