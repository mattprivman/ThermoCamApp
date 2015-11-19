using System.Collections.Generic;
using System.Drawing;


namespace ThermoVision.Tipos
{
    public struct ThermoCamImgArgs
    {
        public Bitmap   Imagen;
        public float    MaxTemp;
        public float    MinTemp;

        public tempElement[,] tempMatrix; 
    }

    public struct ThemoCamImgCuadradosArgs
    {
        public Bitmap ImagenRampa;
        public Bitmap ImagenRejillas;
    }

    public class tempElement
    {
        public float max;
        public float min;
        public double mean;

        public bool hayMaterial;
        public bool estaCaliente;
        public bool activo;

        public Point CannonCoordinate;
        public bool  selected;

        public void stateChanged(object value)
        {
            if (value is bool)
                this.changeStateValue((bool)value);
        }

        void changeStateValue(bool state)
        {
            this.activo = state;

            if (this.activo)
            {
                this.hayMaterial = false;
                this.estaCaliente = false;
            }
        }
    }
}