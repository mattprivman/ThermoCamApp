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

    public struct tempElement
    {
        public float maxTemp;
        public float minTemp;
        public float meanTemp;
    }

    public struct dataElement
    {
        public uint maxTemp;
        public uint minTemp;
        public float meanTemp;
    }
}