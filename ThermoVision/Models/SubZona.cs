using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ThermoVision.Tipos;

namespace ThermoVision.Models
{
    [Serializable]
    public class SubZona : ISerializable
    {
        #region "Variables"
        
        int     _id;
        string  _Nombre;

        bool    _selected;

        //ZONA A LA QUE PERTENECE ESTA DIVISIÓN
        Zona    _parent;
        bool    _hasParent;

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

        #endregion

        #region "Eventos"

        public event EventHandler NameChanged;
        public event EventHandler ParametersChanged;

        #endregion

        #region "Propiedades"

        public int Id                               // -rw 
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
        public string Nombre                        // -rw 
        {
            get
            {
                return this._Nombre;
            }
            set
            {
                this._Nombre = value;

                if (NameChanged != null)
                    NameChanged(this, null);
            }
        }

        public bool Selected                        // -rw 
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

        //ZONA DE PERTENENCIA
        public Zona Parent                          // -rw 
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
        public bool HasParent                       // -rw 
        {
            get
            {
                return this._hasParent;
            }
            set
            {
                this._hasParent = value;
            }
        }

        // REJILLA
        // Numero de filas y columnas de la rejilla
        public int Filas                            // -rw 
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
        public int Columnas                         // -rw 
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
        public Point Inicio     
        {
            get
            {
                return _inicio;
            }
        }
        public Point Fin        
        {
            get
            {
                return _fin;
            }
        }

        #endregion

        #region "Constructores"

        public SubZona()                                             
        {
            this._filas     = 1;
            this._columnas  = 1;
        }
        public SubZona(SerializationInfo info, StreamingContext ctxt)           
        {
            this._id        = (int)     info.GetValue("Id", typeof(int));
            this._Nombre    = (string)  info.GetValue("Nombre", typeof(string));
            this._parent    = (Zona)    info.GetValue("Parent", typeof(Zona));
            this._hasParent = (bool)    info.GetValue("HasParent", typeof(bool));
            this._filas     = (int)     info.GetValue("Filas", typeof(int));
            this._columnas  = (int)     info.GetValue("Columnas", typeof(int));
            this._inicio    = (Point)   info.GetValue("Inicio", typeof(Point));
            this._fin       = (Point)   info.GetValue("Fin", typeof(Point));
        }

        #endregion

        #region "Métodos públicos"

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Id",        this.Id);
            info.AddValue("Nombre",    this.Nombre);
            info.AddValue("Parent",    this.Parent);
            info.AddValue("HasParent", this.HasParent);
            info.AddValue("Filas",     this.Filas);
            info.AddValue("Columnas",  this.Columnas);
            info.AddValue("Inicio",    this.Inicio);
            info.AddValue("Fin",       this.Fin);
        }

        public void addCoordinates(Point p1, Point p2)                          
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

        #endregion
    }
}
