using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static CollectionHelpers;
    using static Identification;

    internal class MessageBodyWithFieldNamesReservedTestCases : MessageBodyTestCasesBase
    {
        private static IEnumerable<object[]> _privateTestCases;

        private static IEnumerable<object[]> PrivateTestCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    foreach (object fieldNames in GetIdents(GetRange(1, 3, 5, 7, 11))
                        .Select<string, Identifier>(x => x).Stagger(_ => 1, x => x / 2, x => x))
                    {
                        yield return GetRange(fieldNames).ToArray();
                    }
                }

                return _privateTestCases ?? (_privateTestCases = GetAll().ToArray());
            }
        }

        protected override IEnumerable<object[]> RightHandSideCases => PrivateTestCases;
    }
}
