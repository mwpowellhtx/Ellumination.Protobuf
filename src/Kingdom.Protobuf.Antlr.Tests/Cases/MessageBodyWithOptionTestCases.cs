using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static CollectionHelpers;
    using static Domain;

    internal class MessageBodyWithOptionTestCases : MessageBodyTestCasesBase
    {
        private static IEnumerable<object[]> _privateTestCases;

        private static IEnumerable<object[]> PrivateTestCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    var optionValues = BuildOptionValues(false, true).ToArray();

                    foreach (var optionNames in GetOptionIdentifierPaths(
                            GetRange(1, 3), GetRange(1, 3)
                            , GetRange(1, 3), GetRange(0, 1, 3))
                        .Stagger(_ => 1, x => x / 2, x => x))
                    {
                        yield return GetRange<object>(optionNames, optionValues).ToArray();
                    }
                }

                return _privateTestCases ?? (_privateTestCases = GetAll().ToArray());
            }
        }

        protected override IEnumerable<object[]> RightHandSideCases => PrivateTestCases;
    }
}
