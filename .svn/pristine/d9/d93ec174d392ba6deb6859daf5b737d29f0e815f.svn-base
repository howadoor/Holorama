using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Holorama.Logic.Abstract;
using Holorama.Logic.Tools;

namespace Holorama.Logic.Concrete.Generators
{
    public class SignsGenerator : GeneratorBase
    {
        public override void GenerateBitmap(RectangleF area, Graphics graphics, IGeneratorOptions options)
        {
            var size = Math.Min(area.Width, area.Height)/32;
            var countX = area.Width/size;
            var countY = area.Height/size;
            var sizeX = area.Width/countX;
            var sizeY = area.Height/countY;
            for (int x = 0; x < countX; x++)
            {
                for (int y = 0; y < countY; y++)
                {
                    var signArea = new RectangleF(area.Left + x*sizeX, area.Top + y*sizeY, sizeX, sizeY);
                    GenerateSign(signArea, graphics);
                }
            }
        }

        public void GenerateSign(RectangleF area, Graphics graphics, Color? signColor = null)
        {
            switch (random.Next(32))
            {
                case 0:
                    {
                        GenerateMosaic(area, graphics, signColor);
                        break;
                    }

                case 1:
                    {
                        GenerateTree(area, graphics, signColor);
                        break;
                    }

                case 2:
                    {
                        GenerateAxisSymmetric(area, graphics, signColor);
                        break;
                    }

                default:
                {
                    GeneratePolygon(area, graphics, signColor);
                    break;
                }
            }
        }

        private void GenerateAxisSymmetric(RectangleF area, Graphics graphics, Color? signColor = null)
        {
            if (signColor == null) signColor = Color.Black;
            area.Inflate(-area.Width / 8.0f, -area.Height / 8.0f);
            var size = Math.Min(area.Width, area.Height);
            var signArea = new RectangleF(area.Left + (area.Width - size * 0.8f)/2.0f, area.Top + (area.Height - size * 0.8f)/2.0f, size, size);
            var center = area.GetCenter();
            var numberOfSides = 2 + random.Next(2);
            var rotationAngle = (float)(Math.PI * 2.0 / 4.0) * random.Next(4);
            if (random.Next(2) == 0) rotationAngle += (float)(Math.PI / numberOfSides);
            var pointsList = DrawingTools.GetPolygonPoints(numberOfSides, center, size / 2.0f, rotationAngle).ToList();

            var points = pointsList.ToArray();
            var signBrush = new SolidBrush(signColor.Value);
            var isInteriorFilled = random.Next(5) == 0;
            var interiorBrush = isInteriorFilled ? signBrush : Brushes.White;
            var thickPen = new Pen(signColor.Value, size / 16.0f);
            var littlePen = new Pen(signColor.Value, thickPen.Width * 0.6f);
            // graphics.DrawPolygon(3 + random.Next(4), point, size/10.0f, 0.0f, fill ? penFromCenterToSide : littlePen, fill ? interiorBrush : Brushes.White);
            var radius = size / 10.0f;

            var leftSide = new PointF[numberOfSides + 1];
            var rightSide = new PointF[numberOfSides + 1];

            bool isVertical = random.Next(2) == 0;
            if (isVertical)
            {
                for (int i = 0; i <= numberOfSides; i++)
                {
                    var y = signArea.Top + signArea.Height/numberOfSides*i;
                    var dx = (float) (signArea.Width*random.NextDouble());
                    leftSide[i] = new PointF(signArea.Left + dx, y);
                    rightSide[i] = new PointF(signArea.Right - dx, y);
                }
            }
            else
            {
                for (int i = 0; i <= numberOfSides; i++)
                {
                    var x = signArea.Left + signArea.Width / numberOfSides * i;
                    var dy = (float)(signArea.Height * random.NextDouble());
                    leftSide[i] = new PointF(x, signArea.Top + dy);
                    rightSide[i] = new PointF(x, signArea.Bottom - dy);
                }
            }

            var allPoints = leftSide.Concat(rightSide.Reverse());
            switch (random.Next(6))
            {
                case 0:
                    graphics.DrawCurve(thickPen, leftSide);
                    graphics.DrawCurve(thickPen, rightSide);
                    break;

                case 1:
                    graphics.DrawCurve(thickPen, allPoints.ToArray());
                    break;

                case 2:
                    if (isInteriorFilled)
                    {
                        graphics.FillClosedCurve(interiorBrush, allPoints.ToArray());
                    }
                    graphics.DrawClosedCurve(thickPen, allPoints.ToArray());
                    break;

                case 3:
                    if (isInteriorFilled)
                    {
                        graphics.FillPolygon(interiorBrush, allPoints.ToArray());
                    }
                    graphics.DrawPolygon(thickPen, allPoints.ToArray());
                    break;

                case 4:
                    graphics.DrawPolygon(thickPen, leftSide);
                    graphics.DrawPolygon(thickPen, rightSide);
                    break;

                default:
                    graphics.DrawClosedCurve(thickPen, leftSide);
                    graphics.DrawClosedCurve(thickPen, rightSide);
                    break;
            }
        }

        private void GenerateTree(RectangleF area, Graphics graphics, Color? signColor = null)
        {
            if (signColor == null) signColor = Color.Black;
            area.Inflate(-area.Width / 8.0f, -area.Height / 8.0f);
            var size = Math.Min(area.Width, area.Height);
            area.Inflate(-area.Width / 16.0f, -area.Height / 16.0f);
            var center = area.GetCenter();
            var numberOfSides = 2 + random.Next(2);
            var rotationAngle = (float)(Math.PI * 2.0 / 4.0) * random.Next(4);
            if (random.Next(2) == 0) rotationAngle += (float)(Math.PI / numberOfSides);
            var pointsList = DrawingTools.GetPolygonPoints(numberOfSides, center, size / 2.0f, rotationAngle).ToList();

            var points = pointsList.ToArray();
            var signBrush = new SolidBrush(signColor.Value);
            var isInteriorFilled = random.Next(5) == 0;
            var interiorBrush = isInteriorFilled ? signBrush : Brushes.White;
            var thickPen = new Pen(signColor.Value, size / 16.0f);
            var littlePen = new Pen(signColor.Value, thickPen.Width * 0.6f);
            // graphics.DrawPolygon(3 + random.Next(4), point, size/10.0f, 0.0f, fill ? penFromCenterToSide : littlePen, fill ? interiorBrush : Brushes.White);
            var radius = size / 10.0f;
            var treePoints = DrawTree(area, graphics, thickPen, radius, numberOfSides).ToArray();
            foreach (var treePoint in treePoints)
            {
                if (random.Next(3) == 0)
                {
                    var fill = random.Next(2) == 0;
                    if (random.Next(2) == 0)
                    {
                        graphics.DrawCircle(treePoint, radius, littlePen, fill ? interiorBrush : Brushes.White);
                    }
                    else
                    {
                        graphics.FillRectangle(fill ? signBrush : Brushes.White, treePoint.X - radius, treePoint.Y - radius, radius * 2.0f, radius * 2.0f);
                        graphics.DrawRectangle(littlePen, treePoint.X - radius, treePoint.Y - radius, radius * 2.0f, radius * 2.0f);
                    }
                }
            }

        }

        private IEnumerable<PointF> DrawTree(RectangleF area, Graphics graphics, Pen thickPen, float radius, int numberOfSides)
        {
            var topPoint = new PointF((area.Left + area.Right)/2.0f, area.Top);
            var bottomPoint = new PointF((area.Left + area.Right)/2.0f, area.Bottom);
            graphics.DrawLine(thickPen, topPoint, bottomPoint);
            for (int i = 0; i <= numberOfSides; i++)
            {
                var y = area.Top + area.Height/numberOfSides*i;
                var prevY = i > 0 ? area.Top + area.Height/numberOfSides*(i - 1) : y;
                var nextY = i < numberOfSides - 1 ? area.Top + area.Height/numberOfSides*(i + 1) : y;
                if (random.Next(2) == 0)
                {
                    var theY = random.Next(2) == 0 ? y : (random.Next(2) == 0 ? prevY : nextY);
                    graphics.DrawLine(thickPen, area.Left + radius, y, topPoint.X, theY);
                    yield return new PointF(area.Left + radius, y);
                }
                if (random.Next(2) == 0)
                {
                    var theY = random.Next(2) == 0 ? y : (random.Next(2) == 0 ? prevY : nextY);
                    graphics.DrawLine(thickPen, area.Right - radius, y, topPoint.X, theY);
                    yield return new PointF(area.Right - radius, y);
                }
                yield return new PointF(topPoint.X, y);
            }
        }

        private void GenerateMosaic(RectangleF area, Graphics graphics, Color? signColor = null)
        {
            if (signColor == null) signColor = Color.Black;
            area.Inflate(-area.Width / 8.0f, -area.Height / 8.0f);
            var size = Math.Min(area.Width, area.Height);
            var center = area.GetCenter();
            var numberOfSides = 2 + random.Next(4);
            var rotationAngle = (float)(Math.PI * 2.0 / 4.0) * random.Next(4);
            if (random.Next(2) == 0) rotationAngle += (float)(Math.PI / numberOfSides);
            var pointsList = DrawingTools.GetPolygonPoints(numberOfSides, center, size / 2.0f, rotationAngle).ToList();

            var points = pointsList.ToArray();
            var signBrush = new SolidBrush(signColor.Value);
            var isInteriorFilled = random.Next(5) == 0;
            var interiorBrush = isInteriorFilled ? signBrush : Brushes.White;
            var thickPen = new Pen(signColor.Value, size / 16.0f);

            foreach (var point in points)
            {
                var mid = center.GetMidPoint(point);
                if (random.Next(1) == 0)
                {
                    graphics.DrawCircle(mid, size / 10.0f, thickPen, random.Next(2) == 0 ? Brushes.White : signBrush);
                }
            }
        }

        private void GeneratePolygon(RectangleF area, Graphics graphics, Color? signColor = null)
        {
            if (signColor == null) signColor = Color.Black;
            area.Inflate(-area.Width / 8.0f, -area.Height / 8.0f);
            var size = Math.Min(area.Width, area.Height);
            var center = area.GetCenter();
            var numberOfSides = 3 + random.Next(5);
            var rotationAngle = GetPolygonRotationAngle(numberOfSides);
            var pointsList = DrawingTools.GetPolygonPoints(numberOfSides, center, size/2.0f, rotationAngle).ToList();
            if (random.Next(4) == 0) pointsList = DrawingTools.GetStarPoints(numberOfSides, center, size/2.0f, size/4.0f, rotationAngle).ToList();
            for (; pointsList.Count() > 3 && random.Next(2) == 0;)
            {
                pointsList.RemoveAt(random.Next(pointsList.Count));
            }
            var points = pointsList.ToArray();
            var signBrush = new SolidBrush(signColor.Value);
            var isInteriorFilled = random.Next(4) == 0;
            var interiorBrush = isInteriorFilled ? signBrush : Brushes.White;
            var thickPen = new Pen(signColor.Value, size / 16.0f);
            if (random.Next(4) > 0)
            {
                if (random.Next(numberOfSides) != 0)
                {
                    graphics.FillPath(interiorBrush, points.ToClosedGraphicsPath());
                    graphics.DrawPath(thickPen, points.ToClosedGraphicsPath());
                }
                else
                {
                    graphics.FillClosedCurve(interiorBrush, points);
                    graphics.DrawClosedCurve(thickPen, points);
                }
            }
            else
            {
                if (random.Next(4) == 0)
                {
                    foreach (var point in points) graphics.DrawLine(thickPen, point, center);
                    isInteriorFilled = false;
                }
                else
                {
                    graphics.DrawCircle(center, size / 2.0f, thickPen, interiorBrush);
                }
            }
            var littlePen = new Pen(signColor.Value, thickPen.Width * 0.6f);
            var penFromCenterToSide = new Pen(isInteriorFilled ? Color.White : signColor.Value, littlePen.Width);
            var drawnSomethingInside = false;
            foreach (var point in points)
            {
                if (random.Next(8) == 0)
                {
                    graphics.DrawLine(penFromCenterToSide, point, center);
                    drawnSomethingInside = true;
                }
                if (random.Next(8) == 0)
                {
                    var fill = isInteriorFilled || random.Next(3) == 0;
                    // graphics.DrawPolygon(3 + random.Next(4), point, size/10.0f, 0.0f, fill ? penFromCenterToSide : littlePen, fill ? interiorBrush : Brushes.White);
                    graphics.DrawCircle(point, size/10.0f, fill ? penFromCenterToSide : littlePen, fill ? signBrush : Brushes.White);
                    drawnSomethingInside = !isInteriorFilled || !fill;
                }
            }
            if (random.Next(drawnSomethingInside ? 7 : 2) == 0)
            {
                var fill = isInteriorFilled || random.Next(3) == 0;
                switch (random.Next(5))
                {
                    case 0:
                        var sides = 3 + random.Next(4);
                        graphics.DrawPolygon(sides, center, size / 10.0f * (random.Next(3) + 1), GetPolygonRotationAngle(sides), fill ? penFromCenterToSide : littlePen, fill ? signBrush : Brushes.White);
                        break;

                    default:
                        graphics.DrawCircle(center, size / 10.0f * (random.Next(3) + 1), fill ? penFromCenterToSide : littlePen, fill ? signBrush : Brushes.White);
                        break;
                }
                drawnSomethingInside = !isInteriorFilled || !fill;
            }
        }

        private float GetPolygonRotationAngle(int numberOfSides)
        {
            var rotationAngle = (float) (Math.PI*2.0/4.0)*random.Next(4);
            if (random.Next(2) == 0) rotationAngle += (float) (Math.PI/numberOfSides);
            return rotationAngle;
        }
    }
}