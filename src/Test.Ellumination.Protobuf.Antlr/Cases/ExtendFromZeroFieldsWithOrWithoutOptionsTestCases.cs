using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Kingdom.Combinatorics.Combinatorials;
    using static CollectionHelpers;
    using static Domain;
    using static Identification;

    internal class ExtendFromZeroFieldsWithOrWithoutOptionsTestCases : TestCasesBase
    {
        private static IEnumerable<object[]> _privateCases;

        private static IEnumerable<object[]> PrivateCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    var inputs = ElementTypes.ToArray<object>() // MessageType
                        .Combine(
                            GetFieldNames(GetRange(1, 3, 5))
                                .Stagger(_ => 0, _ => 1, x => x / 2, x => x).ToArray<object>() // FieldNames
                            , GetOptionIdentifierPaths(GetRange(1), GetRange(1, 3)
                                    , GetRange(1), GetRange(0, 1, 3))
                                .Stagger(_ => 0, x => x / 2, x => x).ToArray<object>() // OptionNames
                        );

                    inputs.SilentOverflow = true;

                    for (var i = 0; i < inputs.Count; i++, ++inputs)
                    {
                        var current = inputs.CurrentCombination.ToArray();

                        var fieldNames = (string[]) current[1];
                        var optionNames = (OptionIdentifierPath[]) current[2];

                        /* Avoids specifying test cases unnecessarily. In other words, in this
                         * case, there would be no Field Options on account of there being no
                         * Normal Fields. So, better to not specify the test case at all. */

                        if (optionNames.Any() && !fieldNames.Any())
                        {
                            continue;
                        }

                        // Now obtain the remaining elements.
                        var messageType = (ElementTypeIdentifierPath) current[0];

                        yield return GetRange<object>(messageType, fieldNames, optionNames).ToArray();
                    }
                }

                return _privateCases ?? (_privateCases = GetAll().ToArray());
            }
        }

        protected override IEnumerable<object[]> Cases => PrivateCases;
    }
}
