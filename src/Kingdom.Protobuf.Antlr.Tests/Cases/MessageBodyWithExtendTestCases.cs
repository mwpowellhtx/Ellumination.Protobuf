using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static Collections;
    using static Domain;

    internal class MessageBodyWithExtendTestCases : MessageBodyTestCasesBase
    {
        private static IEnumerable<object[]> _privateTestCases;

        private static IEnumerable<object[]> PrivateTestCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    foreach (object messageTypes in ElementTypeIdentifierPaths
                        .Stagger(_ => 1, x => x / 2, x => x).Select(x => x.ToArray()))
                    {
                        yield return GetRange(messageTypes).ToArray();
                    }
                }

                return _privateTestCases ?? (_privateTestCases = GetAll().ToArray());
            }
        }

        protected override IEnumerable<object[]> RightHandSideCases => PrivateTestCases;
    }
}
