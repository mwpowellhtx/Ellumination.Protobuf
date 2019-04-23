using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static CollectionHelpers;
    using static Identification;

    internal class BasicEnumStatementTestCases : TestCasesBase
    {
        private static IEnumerable<object[]> _privateCases;

        /// <summary>
        /// Represents Test Cases wherein there is either no noteworthy Body Item, or
        /// <see cref="EmptyStatement"/> is present.
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
