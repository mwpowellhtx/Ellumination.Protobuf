using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
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
                    yield return Collections.GetRange<object>((ImportModifierKind?) null).ToArray();
                    yield return Collections.GetRange<object>((ImportModifierKind?) Public).ToArray();
                    yield return Collections.GetRange<object>((ImportModifierKind?) Weak).ToArray();
                }

                return _privateCases ?? (_privateCases = GetAll().ToArray());
            }
        }

        protected override IEnumerable<object[]> Cases => PrivateCases;
    }
}
