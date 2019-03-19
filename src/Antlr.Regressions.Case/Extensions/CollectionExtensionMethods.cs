using System.Collections.Generic;
using System.Linq;

namespace Kingdom.Antlr.Regressions.Case
{
    internal static class CollectionExtensionMethods
    {
        public static T Next<T>(this IEnumerable<T> values)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            var count = values.Count();
            // ReSharper disable once PossibleMultipleEnumeration
            return count > 1 ? values.ElementAt(1) : default(T);
        }

        public static T NextToLast<T>(this IEnumerable<T> values)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            var count = values.Count();
            // ReSharper disable once PossibleMultipleEnumeration
            return count > 1 ? values.ElementAt(count - 2) : default(T);
        }

        public static bool TryRemoveAt<T>(this IList<T> values, int index)
        {
            values.RemoveAt(index);
            return true;
        }
    }
}
