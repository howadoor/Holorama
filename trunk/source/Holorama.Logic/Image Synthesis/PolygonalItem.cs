using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Holorama.Logic.Image_Synthesis.Abstract;
using Holorama.Logic.Tools;

namespace Holorama.Logic.Image_Synthesis
{
    /// <summary>
    /// Polygon with color. Item of the image synthesis.
    /// </summary>
    public class PolygonalItem : ISynthesisItem
    {
        /// <summary>
        /// Color of the polygon
        /// </summary>
        public Color Color { get; set; }
        
        /// <summary>
        /// Definition of the polygon
        /// </summary>
        public PointF[] Polygon { get; set; }

        public void Render(Graphics graphics)
        {
            graphics.FillClosedCurve(new SolidBrush(Color), Polygon);
        }

        public ISynthesisItem CreateMutated(RectangleF area)
        {
            var mutated = new PolygonalItem { Color = this.Color, Polygon = (PointF[])this.Polygon.Clone() };
            for (var changesCounter = 0; changesCounter < 1; )
            {
                if (ColorEx.Random.Next(8) == 0)
                {
                    var newColor = ColorEx.GetRandomArgb();
                    mutated.Color = ColorEx.Average(mutated.Color, newColor);
                    changesCounter++;
                    continue;
                }
                if (ColorEx.Random.Next(8) == 0)
                {
                    mutated.Color = ColorEx.GetRandomArgb();
                    changesCounter++;
                    continue;
                }
                if (ColorEx.Random.Next(4) == 0)
                {
                    var dx = (float)((ColorEx.Random.NextDouble() - 0.5) * area.Width / 10);
                    var dy = (float)((ColorEx.Random.NextDouble() - 0.5) * area.Height / 10);
                    for (int i = 0; i < mutated.Polygon.Length; i++)
                    {
                        mutated.Polygon[i] = new PointF(mutated.Polygon[i].X + dx * (float)ColorEx.Random.NextDouble() - 0.5f, mutated.Polygon[i].Y + dy * (float)ColorEx.Random.NextDouble() - 0.5f);
                    }
                    changesCounter++;
                    continue;
                }
                if (ColorEx.Random.Next(4) == 0)
                {
                    var i = ColorEx.Random.Next(mutated.Polygon.Length);
                    var point = mutated.Polygon[i];
                    var newPoint = new PointF(point.X + (float)((ColorEx.Random.NextDouble() - 0.5) * area.Width / 10),
                        point.Y + (float)((ColorEx.Random.NextDouble() - 0.5) * area.Height / 10));
                    mutated.Polygon[i] = newPoint;
                    changesCounter++;
                    continue;
                }
                if (ColorEx.Random.Next(4) == 0)
                {
                    var i1 = ColorEx.Random.Next(mutated.Polygon.Length);
                    var i2 = ColorEx.Random.Next(mutated.Polygon.Length);
                    var point = mutated.Polygon[i1];
                    mutated.Polygon[i1] = mutated.Polygon[i2];
                    mutated.Polygon[i2] = point;
                    changesCounter++;
                    continue;
                }
                if (mutated.Color.A == 0)
                {
                    mutated.Color = ColorEx.GetRandomArgb();
                    changesCounter++;
                    continue;
                }
            }
            return mutated;
        }
    }
}
