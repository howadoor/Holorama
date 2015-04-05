using System;
using System.Drawing;

namespace Holorama.Logic.Tools
{
    /// <summary>
    /// Simulates drawing by hand
    /// </summary>
    public class HandDrawer
    {
        Random random = new Random();

        public void DrawLines(Graphics graphics, Pen pen, PointF[] points)
        {
            var image = new Bitmap(8, 8);
            for (int i = 0; i < 16; i++)
            {
                image.SetPixel(random.Next(8), random.Next(8), Color.Black);                
            }
            var brush = new TextureBrush(image);
            var cpen = new Pen(brush, pen.Width);
            for (var i = 1; i < points.Length; i++)
            {
                DrawLine(graphics, cpen, points[i - 1], points[i]);
            }
            DrawLine(graphics, cpen, points[points.Length - 1], points[0]);
        }

        public void DrawLine(Graphics graphics, Pen pen, PointF fromPoint, PointF toPoint)
        {
            const double factor = 32.0;
            var vectorX = toPoint.X - fromPoint.X;
            var vectorY = toPoint.Y - fromPoint.Y;
            var length = toPoint.DistanceTo(fromPoint);
            var fp = new PointF((float)(fromPoint.X + length * random.NextDouble() / factor - vectorX * random.NextDouble() / factor), (float)(fromPoint.Y + length * random.NextDouble() / factor - vectorY * random.NextDouble() / factor));
            var tp = new PointF((float)(toPoint.X + length * random.NextDouble() / factor + vectorX * random.NextDouble() / factor), (float)(toPoint.Y + length * random.NextDouble() / factor + vectorY * random.NextDouble() / factor));
            graphics.DrawLine(pen, fp, tp);
        }
    }
}