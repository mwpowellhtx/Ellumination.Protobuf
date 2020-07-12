using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using static CollectionHelpers;
    using static Identification;

    internal class EmptyEnumStatementTestCases : TestCasesBase
    {
        private static IEnumerable<object[]> _privateCases;

        /// <summary>
        /// Gets the test cases concerning <see cref="EnumStatement"/> with
        /// <see cref="EmptyStatement"/> in the <see cref="IList{T}"/> of type
        /// <see cref="IEnumBodyItem"/>.
        /// </summary>
        private static IEnumerable<object[]> PrivateCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    foreach (var length in GetRange(1, 3))
                    {
                        yield return GetRange<object>(GetIdent(length)).ToArray();
                    }
                }

                return _privateCases ?? (_privateCases = GetAll().ToArray());
            }
        }

        protected override IEnumerable<object[]> Cases => PrivateCases;
    }
}
