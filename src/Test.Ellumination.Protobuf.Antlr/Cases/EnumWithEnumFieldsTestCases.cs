using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Combinatorics.Combinatorials;
    using static CollectionHelpers;
    using static Identification;

    internal class EnumWithEnumFieldsTestCases : TestCasesBase
    {
        private static IEnumerable<object[]> _privateCases;

        public static IEnumerable<object[]> PrivateCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    var inputs = GetIdents(GetRange(1, 3)).ToArray<object>() // EnumLength
                        .Combine(
                            GetIdents(GetRange(1, 3, 5))
                                .Stagger(_ => 1, x => x / 2, x => x).ToArray<object>() // FieldNames
                        );

                    inputs.SilentOverflow = true;

                    for (var i = 0; i < inputs.Count; i++, ++inputs)
                    {
                        var current = inputs.CurrentCombination.ToArray();
                        var enumName = (string) current[0];
                        var fieldNames = (string[]) current[1];
                        yield return GetRange<object>(enumName, fieldNames).ToArray();
                    }
                }

                return _privateCases ?? (_privateCases = GetAll().ToArray());
            }
        }

        protected override IEnumerable<object[]> Cases => PrivateCases;
    }
}
