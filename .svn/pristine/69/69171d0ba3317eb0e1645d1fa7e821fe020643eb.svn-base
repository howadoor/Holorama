using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Holorama.Logic.Tools
{
    /// <summary>
    /// Tools related to generation
    /// </summary>
    public static class GeneratorTools
    {
        /// <summary>
        /// Generates spiral points
        /// </summary>
        /// <param name="area"></param>
        /// <param name="count"></param>
        /// <param name="overSizeFactor"></param>
        /// <returns></returns>
        public static  IEnumerable<PointF> GetSpiralPoints(RectangleF area, int count, float overSizeFactor = 1.5f)
        {
            var center = area.GetCenter();
            var radius = center.DistanceTo(area.Location) * overSizeFactor;
            foreach (var pointF in GetSpiralPoints(center, radius, count)) yield return pointF;
        }

        /// <summary>
        /// Generates spiral points
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<PointF> GetSpiralPoints(PointF center, double radius, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var r = radius*i*i/count/count;
                var angle = i*0.1;
                var x = center.X + Math.Sin(angle)*r;
                var y = center.Y + Math.Cos(angle)*r;
                yield return new PointF((float) x, (float) y);
            }
        }

        public static IEnumerable<PointF> GetSquareGridPoints(RectangleF area, int count, float overSizeFactor = 1.5f)
        {
            const double perturbationFactor = 0.001;
            var random = new Random();
            area = area.Grow(overSizeFactor);
            var ratio = area.Width / (area.Width + area.Height);
            var heightCount = count*ratio/2.0f *0.1f;
            var elementHeight = area.Height / (int) (heightCount + 0.5);
            var widthCount = count*(1 - ratio/2.0f) * 0.1f;
            var elementWidth = area.Width / (int)(widthCount + 0.5);
            var size = Math.Min(elementHeight, elementWidth);
            var cx = (int) (area.Width/size);
            var cy = (int) (area.Height/size);
            var startX = area.Left + (area.Width - (cx*size))/2.0f;
            var startY = area.Top + (area.Height - (cy*size))/2.0f;
            for (int ix = 0; ix <= cx; ix++)
            {
                var x = startX + ix*size*(float) (1.0 + random.NextDouble()*perturbationFactor);
                for (int iy = 0; iy <= cy; iy++)
                {
                    yield return new PointF(x, startY + iy * size * (float)(1.0 + random.NextDouble() * perturbationFactor));
                }
            }
        }

        public static IEnumerable<PointF> GetRandomPoints(RectangleF area, int count, float overSizeFactor = 1.5f)
        {
            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                var x = area.Left - area.Width * (overSizeFactor - 1.0) / 2.0 + random.NextDouble() * area.Width * overSizeFactor;
                var y = area.Top - area.Height * (overSizeFactor - 1.0) / 2.0 + random.NextDouble() * area.Height * overSizeFactor;
                yield return new PointF((float)x, (float)y);
            }
        }

    }
}