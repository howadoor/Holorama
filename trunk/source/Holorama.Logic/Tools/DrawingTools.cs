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
        public static void DrawPolygon(this Graphics graphics, int numberOfSides, PointF center, float radius, float rotationAngle, Pen pen = null, Brush brush = null)
        {
            var polygon = GetPolygonCurve(numberOfSides, center, radius, rotationAngle);
            if (brush != null)
            {
                graphics.FillPath(brush, polygon);
            }
            if (pen != null)
            {
                graphics.DrawPath(pen, polygon);
            }
        }

        public static GraphicsPath GetPolygonCurve(int numberOfSides, PointF center, float radius, float rotationAngle)
        {
            var points = GetPolygonPoints(numberOfSides, center, radius, rotationAngle).ToArray();
            return ToClosedGraphicsPath(points);
        }

        public static GraphicsPath ToClosedGraphicsPath(this PointF[] points)
        {
            var path = new GraphicsPath();
            path.AddLines(points);
            path.CloseFigure();
            return path;
        }

        public static IEnumerable<PointF> GetPolygonPoints(int numberOfSides, PointF center, float radius, float rotationAngle)
        {
            for (int i = 0; i < numberOfSides; i++)
            {
                var angle = Math.PI*2*i/numberOfSides + rotationAngle;
                var point = new PointF((float) (center.X + Math.Cos(angle)*radius), (float) (center.Y + Math.Sin(angle)*radius));
                yield return point;
            }
        }

        public static IEnumerable<PointF> GetStarPoints(int numberOfSides, PointF center, float radius1, float radius2, float rotationAngle)
        {
            var halfAngle = Math.PI/numberOfSides;
            for (int i = 0; i < numberOfSides; i++)
            {
                var angle = Math.PI * 2 * i / numberOfSides + rotationAngle; 
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

        public static double GetAngle(PointF point1, PointF point2)
        {
            return Math.Atan2(point2.X - point1.X, point2.Y - point1.Y);
        }

        public static PointF GetMidPoint(this PointF p1, PointF p2)
        {
            return new PointF((p1.X + p2.X) / 2.0f, (p1.Y + p2.Y) / 2.0f);
        }

        public static LinearGradientBrush GetGradientBrush(this PointF[] points, Color color1, Color color2)
        {
            var rectangle = GetBoundingRectangle(points);
            var angle = (float) (new Random().NextDouble() * 360.0);
            return new LinearGradientBrush(rectangle, color1, color2, angle);
        }

        private static RectangleF GetBoundingRectangle(this IEnumerable<PointF> points)
        {
            var left = points.Min(p => p.X);
            var right = points.Max(p => p.X);
            var top = points.Min(p => p.Y);
            var bottom = points.Max(p => p.Y);
            return RectangleF.FromLTRB(left, top, right, bottom);
        }
    }
}