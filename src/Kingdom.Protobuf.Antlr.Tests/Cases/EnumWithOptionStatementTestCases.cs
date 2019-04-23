using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Combinatorics.Combinatorials;
    using static CollectionHelpers;
    using static Domain;
    using static Identification;

    internal class EnumWithOptionStatementTestCases : TestCasesBase
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
                            GetOptionIdentifierPaths(
                                    GetRange(1, 3), GetRange(1, 3)
                                    , GetRange(1, 3), GetRange(0, 1, 3))
                                .StaggerObject(_ => 1, x => x / 2, x => x).ToArray() // OptionNames
                        );

                    var optionValues = BuildOptionValues(true, false).ToArray();

                    inputs.SilentOverflow = true;

                    for (var i = 0; i < inputs.Count; i++, ++inputs)
                    {
                        var current = inputs.CurrentCombination.ToArray();
                        var enumName = (string) current[0];
                        var optionNames = (OptionIdentifierPath[]) current[1];
                        yield return GetRange<object>(enumName, optionNames, optionValues).ToArray();
                    }
                }

                return _privateCases ?? (_privateCases = GetAll().ToArray());
            }
        }

        protected override IEnumerable<object[]> Cases => PrivateCases;
    }
}
