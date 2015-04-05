using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Holorama.Logic.Tools.Voronoi
{
    /// <summary>
    /// Methods related to <see cref="VoronoiGraph"/> and <see cref="Vector"/> class
    /// </summary>
    public static class VoronoiTools
    {
        public static IEnumerable<IEnumerable<PointF>> GetCells(this VoronoiGraph graph)
        {
            var points = graph.GetPoints().ToList();
            return points.Select(graph.GetCell);
        }

        public static IEnumerable<PointF> GetCell(this VoronoiGraph graph, Vector vertex)
        {
            var edges = graph.Edges.Where(edge => edge.LeftData.Equals(vertex) || edge.RightData.Equals(vertex)).ToList();
            return GetCell(edges);
        }


        private static IEnumerable<PointF> GetCell(List<VoronoiEdge> edges)
        {
            if (edges.Count > 3 && !edges.Any(e => e.LeftData == Fortune.VVUnkown || e.RightData == Fortune.VVUnkown))
            {
                var currentEdge = edges[edges.Count() - 1];
                edges.RemoveAt(edges.Count() - 1);
                var point = currentEdge.VVertexA;
                var firstPoint = point;
                yield return point.ToPointF();
                point = currentEdge.VVertexB;
                for (var newEdge = edges.FirstOrDefault(e => point.Equals(e.VVertexA) || point.Equals(e.VVertexB)); newEdge != null; )
                {
                    if (!edges.Remove(newEdge)) throw new InvalidOperationException("Internal error: Cannot removed edge from a list");
                    yield return point.ToPointF();
                    if (point.Equals(newEdge.VVertexA))
                    {
                        point = newEdge.VVertexB;
                    }
                    else
                    {
                        point = newEdge.VVertexA;
                    }
                    newEdge = edges.FirstOrDefault(e => point.Equals(e.VVertexA) || point.Equals(e.VVertexB));
                }
                //yield return firstPoint.ToPointF();
            }
        }

        public static PointF ToPointF(this Vector vector)
        {
            return new PointF((float) vector[0], (float) vector[1]);
        }

        public static bool Equals(this Vector vector1, Vector vector2)
        {
            return vector1 [0] == vector2 [0] && vector1 [1] == vector2[1];
        }

        public static IEnumerable<Vector> GetPoints(this VoronoiGraph graph)
        {
            return graph.Edges.Select(edge => edge.LeftData).Concat(graph.Edges.Select(edge => edge.RightData)).Distinct();
        }
    }
}