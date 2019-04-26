using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Xunit;

    internal static class AssertExtensionMethods
    {
        public static T AssertNotNull<T>(this T obj)
        {
            Assert.NotNull(obj);
            return obj;
        }

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

                    Assert.NotSame(values.ElementAt(i), values.ElementAt(j));
                }
            }

            return values;
            // ReSharper restore PossibleMultipleEnumeration
        }
    }
}
