using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using static CollectionHelpers;
    using static Identification;

    internal class PackageStatementTestCases : TestCasesBase
    {
        private static IEnumerable<object[]> _privateCases;

        private static IEnumerable<object[]> PrivateCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    foreach (var fullIdent in GetFullIdents(GetRange(1, 3), GetRange(1, 3)))
                    {
                        yield return GetRange<object>(fullIdent).ToArray();
                    }
                }

                return _privateCases ?? (_privateCases = GetAll().ToArray());
            }
        }

        protected override IEnumerable<object[]> Cases => PrivateCases;
    }
}
