using System;
using System.Collections.Generic;
using System.Drawing;

namespace ThermoVision.Helpers
{
    class ColorUtils
    {
        public static List<Color> getColors()
        {
            const int n = 0xFF;

            List<Color> Colores = new List<Color>();

            //TRANSFORMACIÓN AZUL OSCURO (240) A AZUL CLARO (180) -- CANTIDAD 40%
            for (int i = 0; i < n * 0.35; i++)
            {
                int initial = 240;
                int final = 180;
                double range = n * 0.3;

                double value = (((final - initial) / range) * i) + initial;

                Colores.Add(ColorFromHSV(value, 1, 1));
            }

            //TRANSFORMACIÓN AZUL CLARO (180) A VERDE (120) -- CANTIDAD 10%
            for (int i = 0; i < n * 0.1; i++)
            {
                int initial = 180;
                int final = 120;
                double range = n * 0.1;

                Colores.Add(ColorFromHSV((((final - initial) / range) * i) + initial, 1, 1));
            }

            //TRANSFORMACIÓN VERDE (120) A AMARILLO (40) -- CANTIDAD 15%
            for (int i = 0; i < n * 0.15; i++)
            {
                int initial = 120;
                int final = 40;
                double range = n * 0.15;

                Colores.Add(ColorFromHSV((((final - initial) / range) * i) + initial, 1, 1));
            }

            //TRANSFORMACIÓN AMARILLO (40) A ROJO (0) -- CANTIDAD 15%
            for (int i = 0; i < n * 0.15; i++)
            {
                int initial = 40;
                int final = 0;
                double range = n * 0.15;

                Colores.Add(ColorFromHSV((((final - initial) / range) * i) + initial, 1, 1));
            }

            //TRANSFORMACIÓN ROJO (SATURACION 1) A BLANCO (SATURACION 0)
            for (int i = 0; i < n * 0.25; i++)
            {
                int initial = 1;
                int final = 0;
                double range = n * 0.25;

                Colores.Add(ColorFromHSV(0, (((final - initial) / range) * i) + initial, 1));
            }

            return Colores;
        }

        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

    }
}
