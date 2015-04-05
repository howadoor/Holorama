using System.Drawing;

namespace Holorama.Logic.Image_Synthesis.Abstract
{
    /// <summary>
    /// Represents an item in <see cref="Synthesis"/>.
    /// </summary>
    public interface ISynthesisItem
    {
        /// <summary>
        /// Renreds an item on <see cref="graphics"/>.
        /// </summary>
        /// <param name="graphics"></param>
        void Render(Graphics graphics);

        /// <summary>
        /// Creates mutated object based on this instance of <see cref="ISynthesisItem"/>.
        /// </summary>
        /// <returns></returns>
        ISynthesisItem CreateMutated(RectangleF area);
    }
}