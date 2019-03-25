using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static Collections;
    using static Domain;
    using static Identification;

    internal class BasicEnumStatementWithWhiteSpaceTestCases : TestCasesBase
    {
        private static IEnumerable<object[]> _privateCases;

        private static IEnumerable<object[]> GetBaseCases()
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var enumName in GetIdents(GetRange(1, 3)))
            {
                yield return GetRange<object>(enumName).ToArray();
            }
        }

        protected override IEnumerable<object[]> Cases
            => _privateCases ?? (_privateCases =
                   InsertDimension(GetBaseCases(), AllWhiteSpaceAndCommentOptions).ToArray()
               );
    }
}
