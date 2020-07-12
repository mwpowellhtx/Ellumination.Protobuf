using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Xunit;

    internal static class AssertExtensionMethods
    {
        // TODO: TBD: this looks like an interesting one to consider for use with xunit.fluently.assert...
        public static IEnumerable<T> AssertAllDifferent<T>(this IEnumerable<T> values)
            where T : class
        {
            // ReSharper disable PossibleMultipleEnumeration
            var count = values.Count();

            for (var i = 0; i < count; ++i)
            {
                for (var j = 0; j < count; ++j)
                {
                    // Bypass the same index.
                    if (j == i)
                    {
                        continue;
                    }

                    // TODO: TBD: and could extend to include comparers, comparable, equatable, etc...
                    values.ElementAt(i).AssertNotSame(values.ElementAt(j));
                }
            }

            return values;
            // ReSharper restore PossibleMultipleEnumeration
        }
    }
}
