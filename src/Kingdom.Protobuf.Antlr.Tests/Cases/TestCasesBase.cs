using System.Collections;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static Collections;

    internal abstract class TestCasesBase : IEnumerable<object[]>
    {

        protected abstract IEnumerable<object[]> Cases { get; }

        public IEnumerator<object[]> GetEnumerator() => Cases.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected static IEnumerable<object[]> InsertDimension<T>(IEnumerable<object[]> cases, IEnumerable<T> dimension)
            => cases.SelectMany(x => dimension, (x, y) => x.Concat(GetRange<object>(y)).ToArray());
    }
}
