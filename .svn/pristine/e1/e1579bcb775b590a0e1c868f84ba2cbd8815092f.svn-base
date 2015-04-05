using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Holorama.Logic.Image_Synthesis;

namespace Holorama.Logic.Tools
{
    /// <summary>
    /// Extends <see cref="Color"/> class.
    /// </summary>
    public static class ColorEx
    {
        public static Random Random = new Random();

        public static Color GetRandomArgb()
        {
            return Color.FromArgb(Random.Next(256), Random.Next(256), Random.Next(256), Random.Next(256));
        }

        public static Color Average(Color colorA, Color colorB)
        {
            var a = (colorA.A + colorB.A) / 2;
            var r = (colorA.R + colorB.R) / 2;
            var g = (colorA.G + colorB.G) / 2;
            var b = (colorA.B + colorB.B) / 2;
            return Color.FromArgb(a, r, g, b);
        }
    }
}
