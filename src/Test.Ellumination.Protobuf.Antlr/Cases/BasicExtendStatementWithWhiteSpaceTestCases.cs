using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using static CollectionHelpers;
    using static Domain;

    /// <summary>
    /// Test Cases loosely based a very narrow subset of
    /// <see cref="BasicExtendStatementTestCases"/>. We do not need a full set
    /// of base cases, just one or a handful. We will mix that with the
    /// <see cref="AllWhiteSpaceAndCommentOptions"/>.
    /// </summary>
    internal class BasicExtendStatementWithWhiteSpaceTestCases : TestCasesBase
    {
        private static IEnumerable<object[]> _privateCases;

        private static IEnumerable<object[]> GetBaseCases()
        {
            yield return GetRange<object>(
                ElementTypeIdentifierPaths.First()
            ).ToArray();
        }

        protected override IEnumerable<object[]> Cases
            => _privateCases ?? (_privateCases
                   = InsertDimension(GetBaseCases(), AllWhiteSpaceAndCommentOptions).ToArray()
               );
    }
}
