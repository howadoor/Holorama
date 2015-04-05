using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Holorama.Logic.Abstract.General;
using Holorama.Logic.Tools;

namespace Holorama.Logic.Concrete.General
{
    /// <summary>
    /// Returns randomly choosen one of values.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class RandomAwareSelector<TValue> : IAware<TValue>
    {
        private readonly TValue[] values;

        public RandomAwareSelector(params TValue[] values)
        {
            this.values = values;
        }

        /// <summary>
        /// Returns instance of <see cref="TValue"/> randomly choosen from <see cref="values"/>.
        /// </summary>
        public static TValue GetValue(params TValue[] instances)
        {
            return instances[ColorEx.Random.Next(instances.Length)];
        }

        /// <summary>
        /// Returns instance of <see cref="TValue"/> randomly choosen from <see cref="values"/>.
        /// </summary>
        public TValue GetAwared()
        {
            return GetValue(values);
        }
    }
}
