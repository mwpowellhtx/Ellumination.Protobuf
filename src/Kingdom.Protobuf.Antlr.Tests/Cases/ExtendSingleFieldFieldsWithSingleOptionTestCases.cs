using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Combinatorics.Combinatorials;
    using static Collections;
    using static Domain;
    using static Identification;

    internal class ExtendSingleFieldFieldsWithSingleOptionTestCases : TestCasesBase
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
                            GetFieldNames(GetRange(1, 3)).ToArray<object>() // FieldName
                            , GetOptionIdentifierPaths(GetRange(1, 3), GetRange(1, 3)
                                , GetRange(1, 3), GetRange(0, 1, 3)).ToArray<object>() // OptionName
                        );

                    inputs.SilentOverflow = true;

                    for (var i = 0; i < inputs.Count; i++, ++inputs)
                    {
                        yield return inputs.CurrentCombination.ToArray();
                    }
                }

                return _privateCases ?? (_privateCases = GetAll().ToArray());
            }
        }

        protected override IEnumerable<object[]> Cases => PrivateCases;
    }
}
