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

    public struct tempElement
    {
        public float max;
        public float min;
        public double mean;

        public bool hayMaterial;
        public bool estaCaliente;

        public Point CannonCoordinate;
        public bool  selected;
    }
}