using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static Collections;
    using static Domain;
    using static Identification;

    /// <summary>
    /// Test Cases based on <see cref="ExtendFromZeroGroupsTestCases"/>. We do not
    /// need a full set of base cases, just one or a handful. We will mix that with
    /// the <see cref="AllWhiteSpaceAndCommentOptions"/>.
    /// </summary>
    internal class ExtendFromZeroGroupsWithWhiteSpaceTestCases : TestCasesBase
    {
        private static IEnumerable<object[]> _privateCases;

        private static IEnumerable<object[]> GetBaseCases()
        {
            yield return GetRange<object>(
                ElementTypes.First() // MessageType
                , LabelKinds.First() // Label
                , GetGroupNames(GetRange(1, 3)).ToArray() // GroupName
                , FieldNumber
            ).ToArray();
        }

        protected override IEnumerable<object[]> Cases
            => _privateCases ?? (_privateCases
                   = InsertDimension(GetBaseCases(), AllWhiteSpaceAndCommentOptions).ToArray()
               );
    }
}
