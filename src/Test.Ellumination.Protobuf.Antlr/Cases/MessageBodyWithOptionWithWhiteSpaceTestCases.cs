using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using static CollectionHelpers;
    using static Domain;

    internal class MessageBodyWithOptionWithWhiteSpaceTestCases : TestCasesBase
    {
        private static IEnumerable<object[]> _privateCases;

        private static IEnumerable<object[]> GetBaseCases()
        {
            yield return GetRange<object>(
                GetOptionIdentifierPaths(GetRange(3), GetRange(3)
                    , GetRange(3), GetRange(3)).ToArray() // OptionNames
                , BuildOptionValues(true).ToArray() // OptionValues
            ).ToArray();
        }

        protected override IEnumerable<object[]> Cases
            => _privateCases ?? (_privateCases
                   = InsertDimension(GetBaseCases(), AllWhiteSpaceAndCommentOptions).ToArray()
               );
    }
}
