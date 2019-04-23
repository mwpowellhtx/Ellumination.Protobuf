using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static CollectionHelpers;
    using static ImportModifierKind;

    internal class ImportStatementTestCases : TestCasesBase
    {
        private static IEnumerable<object[]> _privateCases;


        private static IEnumerable<object[]> PrivateCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    yield return GetRange<object>((ImportModifierKind?) null).ToArray();
                    yield return GetRange<object>((ImportModifierKind?) Public).ToArray();
                    yield return GetRange<object>((ImportModifierKind?) Weak).ToArray();
                }

                return _privateCases ?? (_privateCases = GetAll().ToArray());
            }
        }

        protected override IEnumerable<object[]> Cases => PrivateCases;
    }
}
