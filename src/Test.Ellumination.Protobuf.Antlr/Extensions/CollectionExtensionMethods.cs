using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    internal static class CollectionExtensionMethods
    {
        public delegate int CalculateStaggeredLengthValue(int value);

        //// ReSharper disable once InconsistentNaming
        /// <summary>
        /// Returns the Tapered <paramref name="values"/> given the <paramref name="callbacks"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="callbacks"></param>
        /// <returns></returns>
        public static IEnumerable<T[]> Stagger<T>(this IEnumerable<T> values
            , params CalculateStaggeredLengthValue[] callbacks)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            var length = values.Count();
            foreach (var callback in callbacks)
            {
                // ReSharper disable once PossibleMultipleEnumeration
                yield return values.Take(callback(length)).ToArray();
            }
        }

        public static IEnumerable<object> StaggerObject<T>(this IEnumerable<T> values
            , params CalculateStaggeredLengthValue[] callbacks)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            var length = values.Count();
            foreach (var callback in callbacks)
            {
                // ReSharper disable once PossibleMultipleEnumeration
                yield return values.Take(callback(length)).ToArray();
            }
        }
    }
}
