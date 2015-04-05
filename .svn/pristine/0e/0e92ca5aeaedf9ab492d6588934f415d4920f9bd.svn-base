using System.Drawing;

namespace Holorama.Logic.Abstract
{
    /// <summary>
    /// Interface of holorama image generator
    /// </summary>
    public interface IGenerator
    {
        /// <summary>
        /// Generates new bitmap image 
        /// </summary>
        /// <param name="bitmapSize">Desired size of the returned bitmap</param>
        /// <param name="area">Area in abstract holospace to be drawn into returned bitmap</param>
        /// <param name="options">Options definining the drawing</param>
        /// <returns>Newly created bitmap</returns>
        Bitmap GenerateBitmap(Size bitmapSize, RectangleF area, IGeneratorOptions options);

        /// <summary>
        /// Creates an image of size <see cref="area"/> on <see cref="graphics"/> using <see cref="options"/>.
        /// </summary>
        /// <param name="area"></param>
        /// <param name="graphics"></param>
        /// <param name="options"></param>
        void GenerateBitmap(RectangleF area, Graphics graphics, IGeneratorOptions options);
    }
}