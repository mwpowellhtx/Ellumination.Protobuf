using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static CollectionHelpers;
    using static Domain;

    internal class SyntaxStatementWhiteSpaceAndCommentTestCases : TestCasesBase
    {
        private static IEnumerable<object[]> _privateCases;

        private static IEnumerable<object[]> PrivateCases
            => _privateCases ?? (_privateCases
                   = AllWhiteSpaceAndCommentOptions.Select(x => GetRange(x).ToArray()).ToArray()
               );

        protected override IEnumerable<object[]> Cases => PrivateCases;
    }
}
