using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Holorama.Logic.Abstract
{
    public abstract class GeneratorBase : IGenerator
    {
        protected Random random = new Random();

        public Bitmap GenerateBitmap(Size bitmapSize, RectangleF area, IGeneratorOptions options)
        {
            var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Transform = new Matrix(area, new PointF[] { new PointF(0, 0), new PointF(bitmapSize.Width, 0), new PointF(0, bitmapSize.Height) });
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                GenerateBitmap(area, graphics, options);
            }
            return bitmap;
        }

        public abstract void GenerateBitmap(RectangleF bitmapSize, Graphics graphics, IGeneratorOptions options);
    }
}