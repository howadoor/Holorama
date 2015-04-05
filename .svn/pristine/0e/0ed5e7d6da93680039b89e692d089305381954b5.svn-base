using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using GeoLib;
using Holorama.Logic.Abstract;
using Holorama.Logic.Tools;
using Holorama.Logic.Tools.Voronoi;

namespace Holorama.Logic.Concrete.Generators
{
    /// <summary>
    /// Creates an image of modulated orbitals
    /// </summary>
    public class ModulatedOrbitalsGenerator : GeneratorBase
    {
        private SignsGenerator signsGenerator = new SignsGenerator();

        public override void GenerateBitmap(RectangleF area, Graphics graphics, IGeneratorOptions options)
        {
            GenerateModulatedOrbitals(area, graphics);
        }

        public void GenerateModulatedOrbitals(RectangleF area, Graphics graphics)
        {
            if (random.Next(3) == 0)
            {
                DrawVoronoiGrid(area, graphics);
                return;
            }
            DrawGrid(area, graphics);
            var i = 0;
            var curves = Enumerable.ToArray<PointF[]>(CreatePointCurves(area));
            foreach (var curve in curves)
            {
                var x = (int) (i*128/curves.Count());
                var color = Color.FromArgb(x, x, x);
                var width = (float) 0.01;
                var pen = new Pen(color, width);
                graphics.DrawClosedCurve(pen, curve);
                if (random.Next(3) == 0)
                {
                    DrawCurveMarks(graphics, pen, curve);
                }
                i++;
            }
            Color[] colors = new Color[] {Color.Black, Color.Red, Color.DarkRed, Color.DarkGreen, Color.DarkGray};
            for (; random.Next(2) > 0;)
            {
                var curve = curves[random.Next(curves.Count())];
                DrawSpecialCurve(graphics, curve, colors[random.Next(colors.Count())]);
                // DrawRiver(graphics, curve, 0.3);
            }
        }

        private void DrawRiver(Graphics graphics, PointF[] curve, double d)
        {
            var leftSide = new PointF[curve.Count()];
            var rightSide = new PointF[curve.Count()];
            var i = 0;
            var gridPen = new Pen(Color.Black, (float)0.01);
            foreach (var at in curve.GetCurveAngles())
            {
                var rotatedAngle = at.Item2 + Math.PI/2;
                var leftPoint = new PointF((float) (at.Item1.X + Math.Sin(rotatedAngle)*d), (float) (at.Item1.Y + Math.Cos(rotatedAngle)*d));
                var rightPoint = new PointF((float)(at.Item1.X - Math.Sin(rotatedAngle) * d), (float)(at.Item1.Y - Math.Cos(rotatedAngle) * d));
                leftSide[i] = leftPoint;
                rightSide[curve.Count() - i - 1] = rightPoint;
                //graphics.DrawLine(gridPen, leftPoint, rightPoint);
                //graphics.DrawCircle(leftPoint, 0.05f, gridPen);
                i++;
            }
            //var path = new GraphicsPath(FillMode.Alternate);
            //path.AddCurve(leftSide);
            //path.AddCurve(rightSide);
            //path.CloseFigure();
            //graphics.FillPath(new SolidBrush(Color.FromArgb(60, Color.Wheat)), path);
            var pen = new Pen(Color.FromArgb(60, Color.Wheat), 0.03f);
            graphics.DrawCurve(pen, leftSide);
            graphics.DrawCurve(pen, rightSide);
        }

        private void DrawCurveMarks(Graphics graphics, Pen pen, PointF[] curve)
        {
            for (int i = 0; i < curve.Count(); i += 32)
            {
                // graphics.DrawCircle(curve[i], 0.02f, pen, Brushes.White);
                var point = curve[i];
                graphics.DrawPolygon(3 + random.Next(4), point, 0.05f, 0.0f, pen, random.Next(16) == 0 ? Brushes.Gray : Brushes.White);
                if (random.Next(16) == 0)
                {
                    graphics.DrawCircle(point, 0.07f, pen);
                }
            }
        }

        /// <summary>
        /// Draws a grid
        /// </summary>
        /// <param name="area"></param>
        /// <param name="graphics"></param>
        private void DrawGrid(RectangleF area, Graphics graphics)
        {
            switch (random.Next(6))
            {
                case 0:
                    DrawSimpleGrid(area, graphics);
                    break;
                case 1:
                    DrawPolarGrid(area, graphics);
                    break;
                case 2:
                    DrawSegmentsGrid(area, graphics);
                    break;
                case 3:
                    DrawSpiralGrid(area, graphics);
                    break;
                case 4:
                    DrawVoronoiGrid(area, graphics);
                    break;
            }
        }

        private void DrawVoronoiGrid(RectangleF area, Graphics graphics)
        {
            var gridPen = new Pen(Color.FromArgb(220, 220, 220), 0.01f);
            var gridPoints = GeneratorTools.GetSquareGridPoints(area, 16).Concat(GeneratorTools.GetSquareGridPoints(area, 231)).Concat(GeneratorTools.GetRandomPoints(area, 512)).ToArray(); // GeneratorTools.GetRandomPoints(area, 512).ToArray();
            var oversizedArea = area.Grow(1.3f);
            var graph = Fortune.ComputeVoronoiGraph(gridPoints.Select(p => new Vector(p.X, p.Y)));
            var random = new Random();
            var importantCells = new List<PointF[]>();
            foreach (var cell in graph.GetCells())
            {
                var points = cell.ToArray();
                if (!points.IsSimplePolygon() || !points.Any(area.Contains)) continue;
                var isImportantCell = random.Next(5) == 0 && points.All(oversizedArea.Contains);
                // var brush = new SolidBrush(Color.FromArgb(random.Next(30), Color.Black));
                var opacity = random.Next(128) + (isImportantCell ? 128 : 0);
                var brush = points.GetGradientBrush(Color.FromArgb(opacity, Color.Bisque), Color.FromArgb(opacity, Color.CornflowerBlue));
                graphics.FillPolygon(brush, points);
                if (isImportantCell)
                {
                    importantCells.Add(points);
                }
            }
            foreach (var edge in graph.Edges)
            {
                graphics.DrawLine(gridPen, edge.VVertexA.ToPointF(), edge.VVertexB.ToPointF());
            }
            var cellPen = new Pen(Color.Black, 0.01f);
            var littlePen = new Pen(Color.FromArgb(200, cellPen.Color), cellPen.Width / 2.0f);
            TestUnion(graphics, area, importantCells); 
            foreach (var importantCell in importantCells)
            {
                graphics.DrawPolygon(cellPen, importantCell);
                var centroid = importantCell.GetCentroid();
                if (centroid.IsEmpty) continue;
                var midPoints = new List<PointF>();
                foreach (var point in importantCell)
                {
                    var midPoint = point.GetMidPoint(centroid).GetMidPoint(point);
                    midPoints.Add(midPoint);
                    var point1 = point;
                    var point2 = midPoint;
                    if (area.Contains(point1) && area.Contains(point2)) graphics.DrawLine(littlePen, point1, point2);
                }
                var cellPolygon = importantCell.ToPolygon();
                var smaller = new C2DPolygon(cellPolygon);
                // smaller.Grow(0.5);
                // graphics.Draw(smaller, littlePen);
                var signSize = 0.1f;
                graphics.Draw(cellPolygon.CreateInflated(0.05), littlePen);
                foreach (var line in smaller.Lines.Cast<C2DLine>())
                {
                    C2DPointSet intersections = new C2DPointSet();
                    cellPolygon.CrossesRay(line, intersections);
                    foreach (var intersection in intersections.Select(i => i.ToPointF()))
                    {
                        graphics.DrawCircle(intersection, signSize * 0.6f, cellPen, Brushes.White);
                        var signArea = new RectangleF(intersection.X - signSize/2.0f, intersection.Y - signSize/2.0f,
                            signSize, signSize);
                        signsGenerator.GenerateSign(signArea, graphics, Color.Black);
                    }
                }
                // graphics.DrawPolygon(littlePen, midPoints.ToArray());
                var size = Math.Min(area.Width, area.Height) / 32.0f;
                signsGenerator.GenerateSign(new RectangleF(centroid.X - size / 2.0f, centroid.Y - size / 2.0f, size, size), graphics);
            }
        }

        private void TestUnion(Graphics graphics, RectangleF area, List<PointF[]> importantCells)
        {
            var pen = new Pen(Color.FromArgb(128, Color.Tomato), 0.01f);
            var placeBrush = new SolidBrush(Color.FromArgb(100, pen.Color));
            List<C2DHoledPolygon> polygons = importantCells.Select(c => new C2DHoledPolygon(c.ToPolygon().CreateInflated(0.00001))).ToList();
            var union = polygons.ToUnion();
            graphics.Draw(union, pen);

            // var poly = new C2DPolygon(polygon.Rim);
            // var circle = new C2DCircle();
            // poly.GetBoundingCircle(circle);
            // graphics.DrawCircle(ToPointF(circle.Centre), (float) circle.Radius, pen, null);
            var hulls = union.Select(unionPoly =>
            {
                var hull = new C2DPolygon(unionPoly.Rim).CreateConvexHull();
                List<C2DHoledPolyBase> places = new List<C2DHoledPolyBase>();
                var hullArea = hull.GetArea();
                var uniPoly = new C2DPolygon(unionPoly.Rim);
                var unionPolyArea = uniPoly.GetArea();
                if (Math.Abs(hullArea - unionPolyArea) > 0.001)
                {
                    uniPoly.Grow(1.001);
                    hull.GetNonOverlaps(uniPoly, places, new CGrid {DegenerateHandling = CGrid.eDegenerateHandling.RandomPerturbation});
                    graphics.Fill(places, placeBrush);
                    foreach (var place in places.Select(p => new C2DPolygon(p.Rim)))
                    {
                        var signSize = Math.Min(area.Width, area.Height)/64.0f;
                        var signCenter = place.GetCentroid().ToPointF();
                        var signArea = new RectangleF(signCenter.X - signSize/2.0f, signCenter.Y - signSize/2.0f,
                            signSize, signSize);
                        signsGenerator.GenerateSign(signArea, graphics, Color.Tomato);
                    }
                }
                return hull;
            }).ToArray();
            graphics.Draw(hulls, pen);
            hulls = hulls.Select(h => h.CreateInflated(0.25)).ToArray();
            union = hulls.ToUnion();
            graphics.Draw (union, pen);
        }

        private void DrawSpiralGrid(RectangleF area, Graphics graphics)
        {
            var gridPen = new Pen(Color.FromArgb(240, 240, 240), (float)0.01);
            var spiral = GeneratorTools.GetSpiralPoints(area, 512).ToArray();
            graphics.DrawCurve(gridPen, spiral);
            var graph = Fortune.ComputeVoronoiGraph(spiral.Select(p => new Vector(p.X, p.Y)));
            foreach (var edge in graph.Edges.Cast<VoronoiEdge>())
            {
                //graphics.DrawLine(pen, edge.VVertexA.ToPointF(), edge.VVertexB.ToPointF());
            }
            foreach (var edge in graph.Edges.Cast<VoronoiEdge>())
            {
                graphics.DrawLine(gridPen, edge.VVertexA.ToPointF(), edge.VVertexB.ToPointF());
            }
    }

    private void DrawSegmentsGrid(RectangleF area, Graphics graphics)
    {
        var length = Math.Max(area.Width, area.Height) * 2;
        var center = GetOrbitalsCenter(area);
        var segmentColors = new Color[] { Color.Wheat, Color.GhostWhite, Color.Cornsilk, Color.LavenderBlush, Color.LemonChiffon, Color.Empty };
        for (; random.Next(6) > 0; )
        {
            var segment = GenerateSegment(graphics, center, length);
            graphics.FillPath(new SolidBrush(Color.FromArgb(64, segmentColors[random.Next(segmentColors.Count())])), segment);
            // graphics.DrawPath(gridPen3, segment);
        }
    }

    private GraphicsPath GenerateSegment(Graphics graphics, PointF center, double radius)
    {
        var inR = radius*random.NextDouble();
        var outR = inR + (radius - inR) * random.NextDouble() * random.NextDouble() * random.NextDouble();
        var startAngle = Math.PI*2*random.NextDouble();
        var sweepAngle = Math.PI*2*random.NextDouble();
        var path = new GraphicsPath();
        path.AddArc((float) (center.X - inR), (float) (center.Y - inR), (float) (inR*2), (float) (inR*2), (float) (startAngle * 180.0 / Math.PI), (float) (sweepAngle * 180.0 / Math.PI));
        path.AddLine((float) (center.X + Math.Cos(startAngle + sweepAngle) * inR), 
            (float) (center.Y + Math.Sin(startAngle + sweepAngle) * inR), 
            (float) (center.X + Math.Cos(startAngle + sweepAngle) * outR), 
            (float) (center.Y + Math.Sin(startAngle + sweepAngle) * outR));
        path.AddArc((float)(center.X - outR), (float)(center.Y - outR), (float)(outR * 2), (float)(outR * 2), (float)((startAngle + sweepAngle) * 180.0 / Math.PI), (float)-(sweepAngle* 180.0 / Math.PI));
        path.AddLine((float)(center.X + Math.Cos(startAngle) * outR),
            (float)(center.Y + Math.Sin(startAngle) * outR),
            (float)(center.X + Math.Cos(startAngle) * inR),
            (float)(center.Y + Math.Sin(startAngle) * inR));
        path.CloseFigure();
        return path;
    }

    private void DrawPolarGrid(RectangleF area, Graphics graphics)
    {
        var gridPen = new Pen(Color.FromArgb(240, 240, 240), (float)0.01);
        var gridPen2 = new Pen(Color.FromArgb(230, 230, 230), (float)0.01);
        var gridPen3 = new Pen(Color.FromArgb(220, 220, 220), (float)0.01);
        var length = Math.Max(area.Width, area.Height) * 2;
        var center = GetOrbitalsCenter(area);
        var segmentColors = new Color[] {Color.Wheat, Color.GhostWhite, Color.Cornsilk, Color.LavenderBlush, Color.LemonChiffon, Color.Empty};
        for (; random.Next(6) > 0; )
        {
            var segment = GenerateSegment(graphics, center, length);
            graphics.FillPath(new SolidBrush(Color.FromArgb(64, segmentColors[random.Next(segmentColors.Count())])), segment);
            // graphics.DrawPath(gridPen3, segment);
        }
        for (int a = 0; a < 360; a += 15)
        {
            var radAngle = a*Math.PI/180.0;
            var target = new PointF((float) (center.X + Math.Sin(radAngle) * length), (float) (center.Y + Math.Cos(radAngle) * length));
            graphics.DrawLine(gridPen, center, target);
        }
        for (var r = 1; r < length; r++)
        {
            graphics.DrawEllipse(gridPen, center.X - r/2.0f, center.Y - r/2.0f, r, r);
        }
        if (random.Next(2) == 0)
        {
            var q = 0.05f;
            for (var r = 1; r < length; r += 5)
            {
                graphics.DrawEllipse(gridPen2, center.X - (r + q)/2.0f, center.Y - (r + q)/2.0f, r + q, r + q);
            }
        }
    }

    private void DrawSimpleGrid(RectangleF area, Graphics graphics)
    {
        DrawSegmentsGrid(area, graphics);
        var originalTransform = graphics.Transform;
        var newTransform = originalTransform.Clone();
        newTransform.RotateAt((float)(random.NextDouble() * 90), area.GetCenter());
        graphics.Transform = newTransform;
        var gridPen = new Pen(Color.FromArgb(240, 240, 240), (float) 0.01);
        for (var x = Math.Floor(area.Left - area.Width); x < Math.Ceiling(area.Right + area.Width); x += 1.0)
        {
            graphics.DrawLine(gridPen, (float) x, (float) area.Top - area.Height, (float) x, (float) area.Bottom + area.Height);
        }
        for (var y = Math.Floor(area.Top - area.Height); y < Math.Ceiling(area.Bottom + area.Height); y += 1.0)
        {
            graphics.DrawLine(gridPen, (float) area.Left - area.Width, (float) y, (float) area.Right + area.Width, (float) y);
        }
        graphics.Transform = originalTransform;
    }

    private void DrawSpecialCurve(Graphics graphics, PointF[] curve, Color color)
    {
        /*
        var center = curve[random.Next(curve.Count())];
        var gridPen = new Pen(Color.FromArgb(240, 240, 240), (float)0.01);
        for (int pI = 0; pI < curve.Count(); pI += curve.Count() / 64)
        {
            var p = curve[pI];
            graphics.DrawLine(gridPen, p, center);
        }*/
            var fonts = new string[] { "WingDings", "WingDings 2", "WingDings 3", "Zipher" };
            //var fonts = new string[] { "Zipher" };
            var font = new Font(fonts[random.Next(fonts.Count())], (float) 0.22);
            graphics.DrawClosedCurve(new Pen(color, (float)0.03), curve);
            var dim = 0.2;
            for (int pI = 0; pI < curve.Count(); pI++)
            {
                var p = curve[pI];
                if (random.Next(32) != 0) continue;
                DrawBubble(graphics, p, pI > 0 ? curve[pI - 1] : curve[pI + 1], color, font);
                continue;
            }
        }

        private void DrawBubble(Graphics graphics, PointF p, PointF p2, Color bubbleColor, Font font)
        {
            var dimPoint = 0.1;
            graphics.FillEllipse(new SolidBrush(bubbleColor), (float)(p.X - dimPoint / 2), (float)(p.Y - dimPoint / 2), (float)dimPoint, (float)dimPoint);
            var dist = 0.5;
            var angle = Math.Atan2(p2.X - p.X, p2.Y - p.Y);
            angle += random.Next(2) == 0 ? Math.PI/2 : -Math.PI/2;
            var bubblePoint = new PointF((float) (p.X + Math.Sin(angle)*dist), (float) (p.Y + Math.Cos(angle)*dist));
            var dim = 0.4f;
            var bubblePen = new Pen(bubbleColor, 0.02f);
            graphics.DrawLine(bubblePen, p, bubblePoint);
            p = bubblePoint;
            var bubbleRect = new RectangleF(p.X - dim / 2.0f, p.Y - dim / 2.0f, dim, dim);
            graphics.FillEllipse(Brushes.LightGray, bubbleRect);
            graphics.DrawEllipse(bubblePen, bubbleRect);
            if (random.Next(2) == 10)
            {
                var stringFormat = new StringFormat {Alignment = StringAlignment.Center};
                const string text = "ABCDEFGHIJKLMNOPQRTSUVWXYZabcdefghijklmnopqrstuvwxyz";
                graphics.DrawString(text.Substring(random.Next(text.Length), 1), font, new SolidBrush(bubbleColor), p.X, (float) (p.Y - 0.15), stringFormat);
            }
            else
            {
                bubbleRect.Inflate(-dim / 10.0f, -dim / 10.0f);
                signsGenerator.GenerateSign(bubbleRect, graphics, bubbleColor);
            }
        }

        private IEnumerable<PointF[]> CreatePointCurves(RectangleF area)
        {
            var center = GetOrbitalsCenter(area);
            var distanceOfCorner = center.DistanceTo(area.Location);
            for (var radius = distanceOfCorner / 20; radius < distanceOfCorner * 1.5; radius = radius * (1 + random.NextDouble()))
            {
                var points = new PointF[360];
                var frequencies = GenerateFrequencies(random.Next(10) + 1);
                for (int angle = 0; angle < 360; angle++)
                {
                    var radAngle = angle*Math.PI/180.0;
                    var r = radius;
                    for (int i = 0; i < Enumerable.Count<double>(frequencies); i++)
                    {
                        r += radius * Math.Sin(radAngle * (i*23+1)+ frequencies[i] * (i + 1) * 185) * frequencies[i]  * 21.11 / (i * 523.5137 + 3.51);
                    }
                    var x = Math.Sin(radAngle)*r;
                    var y = Math.Cos(radAngle)*r;
                    points[angle] = new PointF((float) (center.X + x), (float) (center.Y + y));
                }
                yield return points;
            }
        }

        private PointF GetOrbitalsCenter(RectangleF area)
        {
            return new PointF((float) (area.Left + random.NextDouble() * area.Width), (float) (area.Top + random.NextDouble() * area.Height));
        }

        private double[] GenerateFrequencies(int count)
        {
            var frequencies = new double[count];
            for (int i = 0; i < count; i++)
            {
                frequencies[i] = random.NextDouble();
            }
            return frequencies;
        }
    }
}