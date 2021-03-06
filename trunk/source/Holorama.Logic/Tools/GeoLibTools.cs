﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GeoLib;

namespace Holorama.Logic.Tools
{
    /// <summary>
    /// Extends GeoLib and GeoPolygon classes
    /// </summary>
    public static class GeoLibTools
    {
        #region Points

        /// <summary>
        /// Converts <see cref="point"/> to instance of <see cref="PointF"/>.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static PointF ToPointF(this C2DPoint point)
        {
            return new PointF((float)point.x, (float)point.y);
        }

        /// <summary>
        /// Converts <see cref="points"/> to instances of <see cref="PointF"/>.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static IEnumerable<PointF> ToPointsF(this IEnumerable<C2DPoint> points)
        {
            return points.Select(ToPointF);
        }

        /// <summary>
        /// Converts <see cref="point"/> to instance of <see cref="C2DPoint"/>.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static C2DPoint ToC2DPoint(this PointF point)
        {
            return new C2DPoint(point.X, point.Y);
        }

        #endregion

        #region Polygons

        /// <summary>
        /// Converts <see cref="polygon"/> to instance of <see cref="C2DHoledPolygon"/>.
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static C2DHoledPolygon ToHoledPolygon(this PointF[] polygon)
        {
            return new C2DHoledPolygon(ToPolygon(polygon));
        }

        /// <summary>
        /// Converts <see cref="polygon"/> to instance of <see cref="C2DPolygon"/>.
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static C2DPolygon ToPolygon(this IEnumerable <PointF> polygon)
        {
            return new C2DPolygon(polygon.Select(ToC2DPoint).ToList(), false);
        }

        /// <summary>
        /// Converts areas defined by <see cref="IEnumerable{PointF}"/> in <see cref="areas"/> argument to instances of <see cref="C2DPolygon"/>.
        /// </summary>
        /// <param name="areas">Sequence of polygons defined by <see cref="IEnumerable{PointF}"/>.</param>
        /// <returns>New instances Of <see cref="C2DPolygon"/>.</returns>
        public static IEnumerable<C2DPolygon> ToPolygons(this IEnumerable<IEnumerable<PointF>> areas)
        {
            return areas.Select(c => c.ToPolygon());
        }

        /// <summary>
        /// Returns vertices of <see cref="polyBase"/>.
        /// </summary>
        /// <param name="polyBase"></param>
        /// <returns></returns>
        public static IEnumerable<C2DPoint> GetVertices(this C2DPolyBase polyBase)
        {
            return polyBase.Lines.Select(line => line.GetPointFrom());
        }

        /// <summary>
        /// Creates and returns convex hull of <see cref="polygon"/>.
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static C2DPolygon CreateConvexHull(this C2DPolygon polygon)
        {
            var result = new C2DPolygon();
            result.CreateConvexHull(polygon);
            return result;
        }

        /// <summary>
        /// Unions all <see cref="polygons"/>.
        /// </summary>
        /// <param name="polygons"></param>
        /// <returns></returns>
        public static C2DHoledPolyBaseSet ToUnion(this IEnumerable<C2DHoledPolyBase> polygons)
        {
            C2DHoledPolyBaseSet union = new C2DHoledPolyBaseSet();
            foreach (var polygon in polygons)
            {
                union.Add(polygon);
            }
            var grid = new CGrid { DegenerateHandling = CGrid.eDegenerateHandling.RandomPerturbation };
            union.UnifyProgressive(grid);
            return union;
        }
        
        public static C2DHoledPolyBaseSet ToUnion(this IEnumerable<C2DPolyBase> polygons)
        {
            return polygons.Select(p => new C2DHoledPolygon(p)).ToUnion();
        }

        public static C2DPolygon CreateInflated(this C2DPolyBase polygon, double offset)
        {
            var inflatedPoints = new List<C2DPoint>();
            C2DLine firstLine = null;
            C2DLine prevLine = null;
            foreach (var line in polygon.Lines.Cast<C2DLine>())
            {
                var newLine = new C2DLine(line);
                newLine.MoveLeft(-offset);
                if (prevLine == null)
                {
                    firstLine = newLine;
                }
                else
                {
                    inflatedPoints.Add(GetIntersection(prevLine, newLine));
                }
                prevLine = newLine;
            }
            inflatedPoints.Add(GetIntersection(prevLine, firstLine));
            var inflated = new C2DPolygon();
            inflated.Create(inflatedPoints, true);
            return inflated;
        }
        
        #endregion

        #region Lines

        public static void MoveLeft(this C2DLine line, double offset)
        {
            var angle = line.vector.AngleFromNorth() + Math.PI / 2.0;
            var addX = Math.Sin(angle)*offset;
            var addY = Math.Cos(angle)*offset;
            line.Move(new C2DVector(addX, addY));
        }
        
        public static C2DPoint GetIntersection(C2DLine line1, C2DLine line2)
        {
            PointF intersection;
            if (GeometryTools.TryGetIntersection(line1.GetPointFrom().ToPointF(), line1.GetPointTo().ToPointF(),
                line2.GetPointFrom().ToPointF(), line2.GetPointTo().ToPointF(), out intersection))
            {
                return intersection.ToC2DPoint();
            }
            else
            {
                return line2.GetPointTo();
            }
        }
        
        #endregion

        #region Drawing
        public static void Draw(this Graphics graphics, IEnumerable<C2DPolyBase> polygons, Pen pen)
        {
            foreach (var polygon in polygons) Draw(graphics, polygon, pen);
        }

        public static void Draw(this Graphics graphics, IEnumerable<C2DHoledPolyBase> polygons, Pen pen)
        {
            foreach (var polygon in polygons) Draw(graphics, polygon, pen);
        }

        public static void Fill(this Graphics graphics, IEnumerable<C2DHoledPolyBase> polygons, Brush brush)
        {
            foreach (var polygon in polygons) Fill(graphics, polygon, brush);
        }

        public static void Draw(this Graphics graphics, C2DHoledPolyBase polygon, Pen pen)
        {
            var points = polygon.Rim.GetVertices().ToPointsF().ToArray();
            if (points.Length < 3) return;
            graphics.DrawPolygon(pen, points);
        }

        public static void Fill(this Graphics graphics, C2DHoledPolyBase polygon, Brush brush)
        {
            var points = polygon.Rim.GetVertices().ToPointsF().ToArray();
            if (points.Length < 3) return;
            graphics.FillPolygon(brush, points);
        }

        public static void Draw(this Graphics graphics, C2DPolyBase polygon, Pen pen)
        {
            var points = polygon.GetVertices().ToPointsF().ToArray();
            if (points.Length < 3) return;
            graphics.DrawPolygon(pen, points);
        }

        public static void Draw(this Graphics graphics, C2DLine line, Pen pen)
        {
            graphics.DrawLine(pen, line.GetPointFrom().ToPointF(), line.GetPointTo().ToPointF());
        }
        
        #endregion
    }
}