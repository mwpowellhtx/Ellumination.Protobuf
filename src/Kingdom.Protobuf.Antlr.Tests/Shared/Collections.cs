using System.Collections.Generic;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    internal static class Collections
    {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Returns a new <see cref="IEnumerable{T}"/> Range corresponding to the
        /// <paramref name="values"/>. It is important that this actually iterate
        /// the values and not return the collection, because the enumeration is not
        /// the same thing a the array itself being returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        internal static IEnumerable<T> GetRange<T>(params T[] values)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var x in values)
            {
                yield return x;
            }
        }

        internal static void AddRange<T>(this IList<T> list, T item, params T[] others)
        {
            list.Add(item);
            foreach (var other in others)
            {
                list.Add(other);
            }
        }

        internal static void AddRange<T>(this IList<T> list, IEnumerable<T> values)
        {
            values = values ?? GetRange<T>();
            foreach (var value in values)
            {
                list.Add(value);
            }
        }
    }
}
