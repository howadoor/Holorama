using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holorama.Logic.Image_Synthesis
{
    /// <summary>
    /// Synthesizes image based on instance of <see cref="Synthesis"/>.
    /// </summary>
    public class Synthesizer
    {
        /// <summary>
        /// Creates a bitmap based on instance of <see cref="Synthesis"/>.
        /// </summary>
        /// <param name="synthesis">The synthesis.</param>
        /// <param name="area"></param>
        /// <param name="bitmapSize"></param>
        /// <returns></returns>
        public static Bitmap SynthesizeBitmap(Synthesis synthesis, Bitmap backgroundBitmap, Size bitmapSize, RectangleF area)
        {
            Bitmap bitmap;
            if (backgroundBitmap != null)
            {
                if (backgroundBitmap.Size != bitmapSize) throw new ArgumentException("Size of background bitmap must be the same as in argument bitmapSize when background bitmap provided.");
                bitmap = (Bitmap) backgroundBitmap.Clone();
            }
            else
            {
                bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            }
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Transform = new Matrix(area, new PointF[] { new PointF(0, 0), new PointF(bitmapSize.Width, 0), new PointF(0, bitmapSize.Height) });
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                GenerateBitmap(synthesis, area, graphics);
            }
            return bitmap;
        }

        /// <summary>
        /// Creates a bitmap based on instance of <see cref="Synthesis"/>.
        /// </summary>
        /// <param name="synthesis">The synthesis.</param>
        /// <param name="area"></param>
        /// <param name="graphics"></param>
        private static void GenerateBitmap(Synthesis synthesis, RectangleF area, Graphics graphics)
        {
            foreach (var synthesisItem in synthesis.Items) synthesisItem.Render(graphics);
        }
    }
}
