using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Holorama.Logic.Tools
{
    /// <summary>
    /// Methods for variuos geometry computations
    /// </summary>
    public static class GeometryTools
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
        /// Returns center of the <see cref="points"/>
        /// </summary>
        /// <param name="points">Rectangle which center will be computed</param>
        /// <returns>Center of the <see cref="points"/></returns>
        public static PointF GetCenter(this IEnumerable<PointF> points)
        {
            return new PointF(points.Average(p => p.X), points.Average(p => p.Y));
        }

        /// <summary>
        /// Method to compute the centroid of a polygon. This does NOT work for a complex polygon.
        /// </summary>
        /// <param name="poly">points that define the polygon</param>
        /// <returns>centroid point, or PointF.Empty if something wrong</returns>
        public static PointF GetCentroid(this PointF[] poly)
        {
            float accumulatedArea = 0.0f;
            float centerX = 0.0f;
            float centerY = 0.0f;

            for (int i = 0, j = poly.Length - 1; i < poly.Length; j = i++)
            {
                float temp = poly[i].X * poly[j].Y - poly[j].X * poly[i].Y;
                accumulatedArea += temp;
                centerX += (poly[i].X + poly[j].X) * temp;
                centerY += (poly[i].Y + poly[j].Y) * temp;
            }

            if (Math.Abs(accumulatedArea) < 1E-12f)
                return PointF.Empty;  // Avoid division by zero

            accumulatedArea *= 3f;
            return new PointF(centerX / accumulatedArea, centerY / accumulatedArea);
        }

        /// <summary>
        /// Checks if <see cref="points"/> constitutes simple polygon = at least 3 points with no NaN coordinates.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static bool IsSimplePolygon(this PointF[] points)
        {
            if (points.Count() < 3) return false;
            return points.All(p => !float.IsNaN(p.X) && !float.IsInfinity(p.X) && !float.IsNaN(p.Y) && !float.IsInfinity(p.Y));
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
        /// Grows <see cref="area"/> by <see cref="growFactor"/>.
        /// </summary>
        /// <param name="area"></param>
        /// <param name="growFactor"></param>
        /// <returns></returns>
        public static RectangleF Grow(this RectangleF area, float growFactor)
        {
            return area.Grow(growFactor, growFactor);
        }

        /// <summary>
        /// Grows <see cref="area"/> by <see cref="widthGrowFactor"/> and heightGrowFactor.
        /// </summary>
        /// <param name="area"></param>
        /// <param name="widthGrowFactor"></param>
        /// <param name="heightGrowFactor"></param>
        /// <returns></returns>
        private static RectangleF Grow(this RectangleF area, float widthGrowFactor, float heightGrowFactor)
        {
            area.Inflate(area.Width * (widthGrowFactor - 1.0f), area.Height * (heightGrowFactor - 1.0f));
            return area;
        }

        public static bool TryGetIntersection(PointF ps1, PointF pe1, PointF ps2, PointF pe2, out PointF intersection, double minimumDelta = 0.000001)
        {
            // Get A,B,C of first line - points : ps1 to pe1
            float A1 = pe1.Y - ps1.Y;
            float B1 = ps1.X - pe1.X;
            float C1 = A1 * ps1.X + B1 * ps1.Y;

            // Get A,B,C of second line - points : ps2 to pe2
            float A2 = pe2.Y - ps2.Y;
            float B2 = ps2.X - pe2.X;
            float C2 = A2 * ps2.X + B2 * ps2.Y;

            // Get delta and check if the lines are parallel
            float delta = A1 * B2 - A2 * B1;
            if (Math.Abs(delta) < minimumDelta)
            {
                intersection = default(PointF);
                return false;
            }
            // now return the Vector2 intersection point
            intersection =  new PointF((B2 * C1 - B1 * C2) / delta, (A1 * C2 - A2 * C1) / delta);
            return true;
        }
    }
}