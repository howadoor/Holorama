using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Holorama.Logic.Abstract;
using Holorama.Logic.Abstract.General;
using Holorama.Logic.Tools;

namespace Holorama.Logic.Concrete.Space_Divisors
{
    /// <summary>
    /// Divides space defined by rectangle to square cells
    /// </summary>
    public class SquareTilesDivisor : IFactory<ISpaceDivision, RectangleF, SpaceDivisionOptions>
    {
        public ISpaceDivision Create(RectangleF spaceDefinition, SpaceDivisionOptions options)
        {
            return new SquareTilesDivision(spaceDefinition, 5, 5);
        }
    }

    public class SquareTilesDivision : ISpaceDivision
    {
        private readonly RectangleF spaceDefinition;
        private PointF startPoint;
        private float tileSize;
        private int countX;
        private int countY;

        /// <summary>
        /// Creates instance and sets in members according to parameters.
        /// </summary>
        /// <param name="spaceDefinition">Rectangular space area which should be divided into square tiles.</param>
        /// <param name="countW">Desired count of tiles in x-axis direction.</param>
        /// <param name="countH">Desired count of tiles in y-axis direction.</param>
        public SquareTilesDivision(RectangleF spaceDefinition, int countW, int countH)
        {
            SetDivision(spaceDefinition, countW, countH);
        }

        /// <summary>
        /// Sets members of this instance of<see cref="SquareTilesDivision"/> according to method parameters.
        /// </summary>
        /// <param name="rect">Rectangular space area which should be divided into square tiles.</param>
        /// <param name="countW">Desired count of tiles in x-axis direction.</param>
        /// <param name="countH">Desired count of tiles in y-axis direction.</param>
        private void SetDivision(RectangleF rect, int countW, int countH)
        {
            tileSize = Math.Min(rect.Width/countW, rect.Height/countY);
            countX = (int) Math.Ceiling(rect.Width/tileSize);
            countY = (int) Math.Ceiling(rect.Height / tileSize);
            startPoint = new PointF(rect.Left - (tileSize * countX - rect.Width) / 2.0f, rect.Top - (tileSize * countY - rect.Height) / 2.0f);
        }

        /// <summary>
        /// Returns all space cells (tiles) creates by this space division.
        /// </summary>
        public IEnumerable<IEnumerable<PointF>> Cells
        {
            get { return Rectangles.Select(r => r.GetCorners()); }
        }

        /// <summary>
        /// Returns all space rectangles (tiles) creates by this space division.
        /// </summary>
        private IEnumerable<RectangleF> Rectangles
        {
            get
            {
                for (int iy = 0; iy < countY; iy++)
                {
                    for (int ix = 0; ix < countX; ix++)
                    {
                        yield return GetRectangle(ix, iy);
                    }
                }
            } 
        }

        /// <summary>
        /// Returns rectangle of square tile by index.
        /// </summary>
        /// <param name="ix">X-Index of the tile.</param>
        /// <param name="iy">Y-Index of the tile.</param>
        /// <returns></returns>
        private RectangleF GetRectangle(int ix, int iy)
        {
            return new RectangleF(startPoint.X + ix * tileSize, startPoint.Y + iy * tileSize, tileSize, tileSize);
        }
    }
}