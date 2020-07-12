using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using static CollectionHelpers;
    using static Identification;

    // TODO: TBD: Random ClassData not running / https://github.com/xunit/xunit/issues/2117
    // TODO: TBD: same issue as with the MemberData, class data test cases are not running...
    internal class MessageBodyWithEmptyInnerMessageTestCases : MessageBodyTestCasesBase
    {
        private static IEnumerable<object[]> _privateTestCases;

        private static IEnumerable<object[]> PrivateTestCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    foreach (var innerMessageName in GetIdents(GetRange(1, 3)))
                    {
                        yield return GetRange<object>(innerMessageName).ToArray();
                    }
                }

                return _privateTestCases ?? (_privateTestCases = GetAll().ToArray());
            }
        }

        protected override IEnumerable<object[]> RightHandSideCases => PrivateTestCases;
    }
}
