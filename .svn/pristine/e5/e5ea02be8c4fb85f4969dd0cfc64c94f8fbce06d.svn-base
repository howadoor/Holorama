using System;
using System.Collections.Generic;
using System.Linq;

namespace Psychex.Logic.Helpers
{
    public static class Shuffling
    {
         private static Random random = new Random();

         public static IEnumerable<TItem> Shuffle<TItem>(this IEnumerable<TItem> items)
         {
             return items.Select(i => new KeyValuePair<int, TItem>(random.Next(), i)).OrderBy(kv => kv.Key).Select(kv => kv.Value);
         }
    }
}