using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using static Domain;

    internal class ImportStatementWithWhiteSpaceTestCases : ImportStatementTestCases
    {
        private static IEnumerable<object[]> _privateCases;

        protected override IEnumerable<object[]> Cases
            => _privateCases ?? (_privateCases
                   = InsertDimension(base.Cases, AllWhiteSpaceAndCommentOptions).ToArray()
               );
    }
}
