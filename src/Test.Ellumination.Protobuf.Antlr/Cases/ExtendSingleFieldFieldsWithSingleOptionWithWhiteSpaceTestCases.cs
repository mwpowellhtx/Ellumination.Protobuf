using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using static CollectionHelpers;
    using static Domain;
    using static Identification;

    /// <summary>
    /// Test Cases based on <see cref="ExtendSingleFieldFieldsWithSingleOptionTestCases"/>.
    /// We do not need a full set of base cases, just one or a handful. We will mix that with
    /// the <see cref="AllWhiteSpaceAndCommentOptions"/>.
    /// </summary>
    internal class ExtendSingleFieldFieldsWithSingleOptionWithWhiteSpaceTestCases : TestCasesBase
    {
        private static IEnumerable<object[]> _privateCases;

        private static IEnumerable<object[]> GetBaseCases()
        {
            yield return GetRange<object>(
                ElementTypes.First()
                , GetFieldName(3)
                , GetOptionIdentifierPath(3, 3, 3, 3, true)
            ).ToArray();
        }

        protected override IEnumerable<object[]> Cases
            => _privateCases ?? (_privateCases
                   = InsertDimension(GetBaseCases(), AllWhiteSpaceAndCommentOptions).ToArray()
               );
    }
}
