using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ThermoVision.Enumeraciones;
using ThermoVision.Tipos;

namespace ThermoVision.Models
{
    [Serializable]
    public class SubZona : ISerializable
    {
        #region "Variables"
        
        int         _id;
        string      _nombre;

        bool        _selected;

        //ZONA A LA QUE PERTENECE ESTA DIVISIÓN
        Zona        _parent;

        ThermoCam   _thermoParent;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// COORDENADAS ZONA ÚTIL
        //////////////////////////////// Delimitar la zona útil que se quiere medir 

        Point   _inicio;
        Point   _fin;

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// REJILLA ZONA ÚTIL
        /////////////////////////////// La zona útil queda seccioada por una rejilla 

        int     _filas;
        int     _columnas;

        //Matriz de temperaturas
        public tempElement[,] tempMatrix;

        //Temperaturas subzona
        public float   _maxTemp;
        public float   _minTemp;
        public double  _meanTemp;

        public bool accessed;

        public bool vaciado = false;

        #endregion

        #region "Eventos"

        public event EventHandler NameChanged;
        public event EventHandler ParametersChanged;

        #endregion

        #region "Propiedades"

        public int  Id                               // -rw 
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;

                if (ParametersChanged != null)
                    ParametersChanged(this, null);
            }
        }
        public string   Nombre                       // -rw 
        {
            get
            {
                return this._nombre;
            }
            set
            {
                this._nombre = value;

                if (NameChanged != null)
                    NameChanged(this, null);
            }
        }

        public bool     Selected                     // -rw 
        {
            get
            {
                return this._selected;
            }
            set
            {
                this._selected = value;
            }
        }
        public bool     Visualizar                   // -rw 
        {
            get;
            set;
        }

        //ZONA DE PERTENENCIA
        public Zona         Parent                   // -rw 
        {
            set 
            {
                this._parent = value;
            }
            get 
            {
                return this._parent;
            }
        }
        public ThermoCam    ThermoParent             // -rw 
        {
            get
            {
                return this._thermoParent;
            }
            set
            {
                if (_thermoParent != null || !this._parent.Equals(value))
                {
                    lock ("Zonas")
                    {
                        lock ("SubZonas")
                        {
                            if (ThermoParent != null)
                                this.ThermoParent.removeSubZona(this);
                            this._thermoParent = value;
                            if(_thermoParent != null)   
                                this.ThermoParent.addSubZona(this);
                        }
                    }
                }
            }
        }

        // REJILLA
        // Numero de filas y columnas de la rejilla
        public int Filas                             // -rw 
        {
            get
            {
                return _filas;
            }
            set
            {
                if (value > 0)
                {
                    lock("lockRejilla")
                    {
                        _filas = value;

                        if (ParametersChanged != null)
                            ParametersChanged(this, null);
                    }
                }
            }
        }
        public int Columnas                          // -rw 
        {
            get
            {
                return _columnas;
            }
            set
            {
                if (value > 0)
                {
                    lock ("lockRejilla")
                    {
                        _columnas = value;

                        if (ParametersChanged != null)
                            ParametersChanged(this, null);
                    }
                }
            }
        }

        //Coordenadas
        public Point Inicio                          // -r  
        {
            get
            {
                return _inicio;
            }
        }
        public Point Fin                             // -r  
        {
            get
            {
                return _fin;
            }
        }
         
        #endregion

        #region "Constructores"

        public SubZona(string Name)                                             
        {
            this._nombre    = Name;

            this._filas     = 1;
            this._columnas  = 1;

            this.tempMatrix = new tempElement[this._filas, this._columnas];

            for (int i = 0; i < this._filas; i++)
            {
                for (int j = 0; j < this._columnas; j++)
                {
                    tempMatrix[i, j] = new tempElement();
                }
            }
        }
        protected SubZona(SerializationInfo info, StreamingContext ctxt)        
        {
            this._id            = (int)       info.GetValue("Id",           typeof(int));
            this._nombre        = (string)    info.GetValue("Nombre",       typeof(string));
            this._parent        = (Zona)      info.GetValue("Parent",       typeof(Zona));
            this._filas         = (int)       info.GetValue("Filas",        typeof(int));
            this._columnas      = (int)       info.GetValue("Columnas",     typeof(int));
            this._inicio        = (Point)     info.GetValue("Inicio",       typeof(Point));
            this._fin           = (Point)     info.GetValue("Fin",          typeof(Point));
            this._thermoParent  = (ThermoCam) info.GetValue("ThermoParent", typeof(ThermoCam));

            this.tempMatrix = new tempElement[this._filas, this._columnas];

            for (int i = 0; i < this._filas; i++)
            {
                for (int j = 0; j < this._columnas; j++)
                {
                    tempMatrix[i, j] = new tempElement();
                }
            }
        }

        #endregion

        #region "Métodos públicos"

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Id",             this.Id);
            info.AddValue("Nombre",         this.Nombre);
            info.AddValue("Parent",         this.Parent);
            info.AddValue("Filas",          this.Filas);
            info.AddValue("Columnas",       this.Columnas);
            info.AddValue("Inicio",         this.Inicio);
            info.AddValue("Fin",            this.Fin);
            info.AddValue("ThermoParent",   this._thermoParent);
        }

        public void addCoordinates(Point p1, Point p2)                          
        {
            if (this._thermoParent != null)
            {
                if (p1.X >= 0 && p1.Y >= 0 && p2.X > 0 && p2.Y >= 0 &&
                    p1.X < ThermoParent.Width && p1.Y < ThermoParent.Heigth &&
                    p2.X < ThermoParent.Width && p2.Y < ThermoParent.Heigth)
                {
                    lock ("Zonas")
                    {
                        // El punto de inicio sera el menor y el de fin el mayor.
                        if (p1.X > p2.X)
                        {
                            this._inicio.X = p2.X;
                            this._fin.X = p1.X;
                        }
                        else
                        {
                            this._inicio.X = p1.X;
                            this._fin.X = p2.X;
                        }

                        if (p1.Y > p2.Y)
                        {
                            this._inicio.Y = p2.Y;
                            this._fin.Y = p1.Y;
                        }
                        else
                        {
                            this._inicio.Y = p1.Y;
                            this._fin.Y = p2.Y;
                        }

                        if (ParametersChanged != null)
                            ParametersChanged(this, null);
                    }
                }
            }
        }

        #endregion
    }
}
