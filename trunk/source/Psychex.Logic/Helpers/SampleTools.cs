using System.Collections.Generic;
using System.Linq;
using Meta.Numerics.Statistics;

namespace Psychex.Logic.Helpers
{
    public static class SampleTools
    {
         public static Sample ToSample (this IEnumerable<double> items)
         {
             return new Sample(items);
         }
    }
}