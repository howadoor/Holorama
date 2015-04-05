using System.Collections.Generic;
using System.Drawing;

namespace Holorama.Logic.Tools
{
    /// <summary>
    /// Extandes <see cref="RectangleF"/> class.
    /// </summary>
    public static class RectangleFloatEx
    {
        /// <summary>
        /// Returns corner points of <see cref="rectangle"/>. First left top corner then others in clockwise order.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static IEnumerable<PointF> GetCorners(this RectangleF rectangle)
        {
            yield return rectangle.GetLeftTop();
            yield return rectangle.GetRightTop();
            yield return rectangle.GetRightBottom();
            yield return rectangle.GetLeftBottom();
        }

        /// <summary>
        /// Returns left top corner of <see cref="rectangle"/>.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static PointF GetLeftTop(this RectangleF rectangle)
        {
            return new PointF(rectangle.Left, rectangle.Top);
        }

        /// <summary>
        /// Returns right top corner of <see cref="rectangle"/>.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static PointF GetRightTop(this RectangleF rectangle)
        {
            return new PointF(rectangle.Right, rectangle.Top);
        }
        
        /// <summary>
        /// Returns right bottom corner of <see cref="rectangle"/>.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static PointF GetRightBottom(this RectangleF rectangle)
        {
            return new PointF(rectangle.Right, rectangle.Bottom);
        }
        
        /// <summary>
        /// Returns left bottom corner of <see cref="rectangle"/>.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static PointF GetLeftBottom(this RectangleF rectangle)
        {
            return new PointF(rectangle.Left, rectangle.Bottom);
        }

        /// <summary>
        /// Inflates <see cref="rectangle"/> by scale factors <see cref="factorX"/> and <see cref="factorY"/>.
        /// </summary>
        /// <param name="factorX">Width scale factor.</param>
        /// <param name="factorY">Height scale factor.</param>
        /// <returns></returns>
        public static RectangleF InflateByScale(this RectangleF rectangle, double factorX, double factorY)
        {
            var newWidth = rectangle.Width*factorX;
            var newHeight = rectangle.Height*factorY;
            return new RectangleF((float) (rectangle.Location.X - (newWidth - rectangle.Width) / 2), (float) (rectangle.Location.Y - (newHeight - rectangle.Height) / 2), (float) newWidth, (float) newHeight);
        }
    }
}