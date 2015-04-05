using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Holorama.Logic.Abstract.General;
using Holorama.Logic.Image_Synthesis.Abstract;
using Holorama.Logic.Tools;

namespace Holorama.Logic.Image_Synthesis
{
    /// <summary>
    /// Creates an instance of <see cref="CircularItem"/>.
    /// </summary>
    public class CircularItemFactory : IFactory<ISynthesisItem, RectangleF>
    {
        public ISynthesisItem Create(RectangleF area)
        {
            return new CircularItem { Color = ColorEx.GetRandomArgb(), Center = GeneratorTools.GetRandomPoints(area, 1, 0.2f).First(), Radius = Math.Max(area.Width, area.Height) * (float) ColorEx.Random.NextDouble() };
        }
    }
}