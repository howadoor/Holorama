using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Holorama.Logic.Abstract.General;
using Holorama.Logic.Image_Synthesis.Abstract;
using Ninject;

namespace Holorama.Logic.Image_Synthesis
{
    /// <summary>
    /// Creates instance of <see cref="Synthesis"/>.
    /// </summary>
    public class SynthesisFactory : IFactory<Synthesis, RectangleF>
    {
        private static Random random = new Random();

        private int minItems = 1;
        private int maxItems = 1;

        private IFactory<ISynthesisItem, RectangleF> itemFactory;

        /// <summary>
        /// Creates an instance and Initializes it's members.
        /// </summary>
        /// <param name="itemFactory">The factory used for creating items.</param>
        public SynthesisFactory(IFactory<ISynthesisItem, RectangleF> itemFactory)
        {
            this.itemFactory = itemFactory;
        }

        /// <summary>
        /// Creates instance of <see cref="Synthesis"/>.
        /// </summary>
        public Synthesis Create(RectangleF area)
        {
            // return new Synthesis {Items = CreatePolygons(area, minItems + random.Next(maxItems - minItems)).ToArray()};
            return new Synthesis { Items = CreateItems(area, minItems + random.Next(maxItems - minItems)).ToArray() };
        }

        /// <summary>
        /// Creates <see cref="count"/> instances of <see cref="PolygonalItem"/> within <see cref="area"/>.
        /// </summary>
        /// <param name="area"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private IEnumerable<ISynthesisItem> CreateItems(RectangleF area, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return itemFactory.Create(area);
            }
        }
    }
}
