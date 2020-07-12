using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Kingdom.Combinatorics.Combinatorials;
    using static CollectionHelpers;
    using static Domain;
    using static Identification;

    internal class EnumFieldsWithOptionsTestCases : TestCasesBase
    {
        private static IEnumerable<object[]> _privateCases;

        public static IEnumerable<object[]> PrivateCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    var inputs = GetIdents(GetRange(1, 3)).ToArray<object>() // EnumName
                        .Combine(
                            GetIdents(GetRange(1, 3))
                                .Stagger(_ => 1, x => x / 2, x => x).ToArray<object>() // FieldNames
                            , GetOptionIdentifierPaths(GetRange(1, 3), GetRange(1, 3)
                                    , GetRange(1, 3), GetRange(0, 1, 3))
                                .Stagger(_ => 0, _ => 1, x => x / 2, x => x).ToArray<object>() // OptionNames
                        );

                    inputs.SilentOverflow = true;

                    for (var i = 0; i < inputs.Count; i++, ++inputs)
                    {
                        var current = inputs.CurrentCombination.ToArray();
                        var enumName = (string) current[0];
                        var fieldNames = (string[]) current[1];
                        var optionNames = (OptionIdentifierPath[]) current[2];
                        yield return GetRange<object>(enumName, fieldNames, optionNames).ToArray();
                    }
                }

                return _privateCases ?? (_privateCases = GetAll().ToArray());
            }
        }

        protected override IEnumerable<object[]> Cases => PrivateCases;
    }
}
