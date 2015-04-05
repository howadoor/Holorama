using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Holorama.Logic.Tools
{
    /// <summary>
    /// Extends functionality of objects related to <see cref="System.Drawing"/> namespace
    /// </summary>
    public static class DrawingTools
    {
        /// <summary>
        /// Returns center of the <see cref="rectangle"/>
        /// </summary>
        /// <param name="rectangle">Rectangle which center will be computed</param>
        /// <returns>Center of the <see cref="rectangle"/></returns>
        public static PointF GetCenter(this RectangleF rectangle)
         {
             return new PointF(rectangle.Left + rectangle.Width/2, rectangle.Top + rectangle.Height/2);
         }

        /// <summary>
        /// Computes distance between two points
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static double DistanceTo(this PointF point1, PointF point2)
        {
            var dx = point1.X - point2.X;
            var dy = point1.Y - point2.Y;
            return Math.Sqrt(dx*dx + dy*dy);
        }

        /// <summary>
        /// Draws a filled circle
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="pen"></param>
        /// <param name="brush"></param>
        public static void DrawCircle(this Graphics graphics, PointF center, float radius, Pen pen = null, Brush brush = null)
        {
            if (brush != null)
            {
                graphics.FillEllipse(brush, center.X - radius, center.Y - radius, radius * 2, radius * 2);
            }
            if (pen != null)
            {
                graphics.DrawEllipse(pen, center.X - radius, center.Y - radius, radius * 2, radius * 2);
            }
        }

        /// <summary>
        /// Draws a filled circle
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="pen"></param>
        /// <param name="brush"></param>
        public static void DrawPolygon(this Graphics graphics, int numberOfSides, PointF center, float radius, Pen pen = null, Brush brush = null)
        {
            var polygon = GetPolygonCurve(numberOfSides, center, radius);
            if (brush != null)
            {
                graphics.FillPath(brush, polygon);
            }
            if (pen != null)
            {
                graphics.DrawPath(pen, polygon);
            }
        }

        public static GraphicsPath GetPolygonCurve(int numberOfSides, PointF center, float radius)
        {
            var path = new GraphicsPath();
            path.AddLines(GetPolygonPoints(numberOfSides, center, radius).ToArray());
            path.CloseFigure();
            return path;
        }

        public static IEnumerable<PointF> GetPolygonPoints(int numberOfSides, PointF center, float radius)
        {
            for (int i = 0; i < numberOfSides; i++)
            {
                var angle = Math.PI*2*i/numberOfSides;
                var point = new PointF((float) (center.X + Math.Cos(angle)*radius), (float) (center.Y + Math.Sin(angle)*radius));
                yield return point;
            }
        }

        public static IEnumerable<PointF> GetStarPoints(int numberOfSides, PointF center, float radius1, float radius2)
        {
            var halfAngle = Math.PI/numberOfSides;
            for (int i = 0; i < numberOfSides; i++)
            {
                var angle = Math.PI * 2 * i / numberOfSides;
                var point = new PointF((float)(center.X + Math.Cos(angle) * radius1), (float)(center.Y + Math.Sin(angle) * radius1));
                yield return point;
                var point2 = new PointF((float)(center.X + Math.Cos(angle+halfAngle) * radius2), (float)(center.Y + Math.Sin(angle+halfAngle) * radius2));
                yield return point2;
            }
        }

        public static IEnumerable<Tuple<PointF, double>> GetCurveAngles(this PointF[] curve)
        {
            for (int i = 0; i < curve.Count(); i++)
            {
                var point = curve[i];
                if (i == 0)
                {
                    var angle = GetAngle(point, curve[1]);
                    yield return new Tuple<PointF, double>(point, angle);
                    continue;
                }
                yield return new Tuple<PointF, double>(point, GetAngle(curve [i-1], point));
            }
        }

        private static double GetAngle(PointF point1, PointF point2)
        {
            return Math.Atan2(point2.X - point1.X, point2.Y - point1.Y);
        }

        public static PointF GetMidPoint(this PointF p1, PointF p2)
        {
            return new PointF((p1.X + p2.X) / 2.0f, (p1.Y + p2.Y) / 2.0f);
        }
    }
}