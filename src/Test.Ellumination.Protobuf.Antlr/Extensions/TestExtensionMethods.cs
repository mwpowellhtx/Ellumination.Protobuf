using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Xunit;

    internal static class TestExtensionMethods
    {
        public static void AllNotNull<T>(this IEnumerable<T> values)
            where T : class
        {
            Assert.NotNull(values);
            // ReSharper disable once PossibleMultipleEnumeration
            if (!values.Any())
            {
                return;
            }

            // ReSharper disable once PossibleMultipleEnumeration
            Assert.All(values, Assert.NotNull);
        }
    }
}
