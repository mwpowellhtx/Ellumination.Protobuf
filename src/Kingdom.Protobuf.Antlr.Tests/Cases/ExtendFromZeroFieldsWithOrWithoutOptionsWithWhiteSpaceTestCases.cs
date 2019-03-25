using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static Collections;
    using static Domain;
    using static Identification;

    internal class ExtendFromZeroFieldsWithOrWithoutOptionsWithWhiteSpaceTestCases : TestCasesBase
    {
        private static IEnumerable<object[]> _privateCases;

        private static IEnumerable<object[]> GetBaseCases()
        {
            yield return GetRange<object>(
                ElementTypes.First() // MessageType
                , GetFieldNames(GetRange(1, 3)).ToArray() // FieldNames
                , GetOptionIdentifierPaths(GetRange(3), GetRange(3)
                    , GetRange(3), GetRange(3)).ToArray() // OptionNames
            ).ToArray();
        }

        protected override IEnumerable<object[]> Cases
            => _privateCases ?? (_privateCases
                   = InsertDimension(GetBaseCases(), AllWhiteSpaceAndCommentOptions).ToArray()
               );
    }
}
