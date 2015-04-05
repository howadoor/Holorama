using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using Holorama.Logic.Abstract.General;
using Holorama.Logic.Image_Synthesis.Abstract;
using Holorama.Logic.Tools;

namespace Holorama.Logic.Image_Synthesis
{
    /// <summary>
    /// Creates an instance of <see cref="PolygonalItem"/>.
    /// </summary>
    public class PolygonalItemFactory : IFactory<ISynthesisItem, RectangleF>
    {
        private const int minPoints = 3;
        private const int maxPoints = 3;

        /// <summary>
        /// Creates a new instance of <see cref="PolygonalItem"/> with random polygons.
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public ISynthesisItem Create(RectangleF area)
        {
            return new PolygonalItem {Color = ColorEx.GetRandomArgb(), Polygon = GetRandomPolygon(area, minPoints, maxPoints)};
        }

        private static PointF[] GetRandomPolygon(RectangleF area, int minPoints, int maxPoints)
        {
            return GetRandomPoints(area, minPoints + ColorEx.Random.Next(maxPoints - minPoints)).ToArray();
        }

        private static IEnumerable<PointF> GetRandomPoints(RectangleF area, int count)
        {
            var dx = area.Width * ColorEx.Random.NextDouble();
            var dy = area.Height * ColorEx.Random.NextDouble();
            var centerX = area.GetCenter().X + area.Width * (ColorEx.Random.NextDouble() - 0.5);
            var centerY = area.GetCenter().Y + area.Height * (ColorEx.Random.NextDouble() - 0.5);
            for (var i = 0; i < count; i++)
            {
                yield return new PointF((float) (centerX + dx * (ColorEx.Random.NextDouble() - 0.5)), (float) (centerY + dy * (ColorEx.Random.NextDouble() - 0.5)));
            }
        }
    }
}