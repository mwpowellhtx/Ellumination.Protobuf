using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Combinatorics.Combinatorials;
    using static Collections;
    using static Domain;
    using static Identification;

    internal class MessageBodyWithMapFieldTestCases : MessageBodyTestCasesBase
    {
        private static IEnumerable<object[]> _privateTestCases;

        private static IEnumerable<object[]> PrivateTestCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    var fieldNumber = FieldNumber;
                    var optionValues = BuildOptionValues(true).ToArray();

                    var inputs = KeyTypes.Select(x => (object) x).ToArray() // Key
                        .Combine(
                            Types.ToArray<object>() // Type
                            , GetIdents(GetRange(1, 3)).ToArray<object>() // MapName
                            , GetOptionIdentifierPaths(
                                    GetRange(3), GetRange(3)
                                    , GetRange(3), GetRange(0, 1, 3))
                                .StaggerObject(_ => 0, _ => 1, x => x / 2, x => x).ToArray() // OptionNames
                        );

                    inputs.SilentOverflow = true;

                    for (var i = 0; i < inputs.Count; i++, ++inputs)
                    {
                        var current = inputs.CurrentCombination.ToArray();
                        var keyType = (KeyType) current[0];
                        var valueType = (IVariant) current[1];
                        var mapName = (string) current[2];
                        var optionNames = (OptionIdentifierPath[]) current[3];
                        yield return GetRange<object>(keyType, valueType, mapName, fieldNumber, optionNames, optionValues).ToArray();
                    }
                }

                return _privateTestCases ?? (_privateTestCases = GetAll().ToArray());
            }
        }

        protected override IEnumerable<object[]> RightHandSideCases => PrivateTestCases;
    }
}
