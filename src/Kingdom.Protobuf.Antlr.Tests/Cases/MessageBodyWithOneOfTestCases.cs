using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Combinatorics.Combinatorials;
    using static Collections;
    using static Domain;
    using static Identification;
    using FieldTupleType = Tuple<ProtoType, string, long>;

    internal class MessageBodyWithOneOfTestCases : MessageBodyTestCasesBase
    {
        private static IEnumerable<object[]> _privateTestCases;

        private static IEnumerable<object[]> PrivateTestCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<FieldTupleType> GetFieldTuples()
                    {
                        var fieldNumber = FieldNumber;

                        var tupleInputs = ProtoTypes.Select(x => (object) x).ToArray() // ProtoType
                            .Combine(
                                GetIdents(GetRange(1, 3, 5)).ToArray<object>() // fieldName
                            );

                        for (; !tupleInputs.Exhausted; ++tupleInputs)
                        {
                            var current = tupleInputs.CurrentCombination.ToArray();
                            var protoType = (ProtoType) current[0];
                            var fieldName = (string) current[1];
                            yield return Tuple.Create(protoType, fieldName, fieldNumber);
                        }
                    }

                    var inputs = GetIdents(GetRange(1, 3)).ToArray<object>() // oneOfName
                        .Combine(
                            GetFieldTuples()
                                .Stagger(_ => 1, x => x / 2, x => x).ToArray<object>() // oneOfField Tuples
                            , GetOptionIdentifierPaths(
                                    GetRange(1, 3), GetRange(1, 3)
                                    , GetRange(1, 3), GetRange(0, 1, 3))
                                .StaggerObject(_ => 0, _ => 1, x => x / 2, x => x).ToArray() // optionNames
                        );

                    inputs.SilentOverflow = true;

                    var optionValues = BuildOptionValues(true, false).ToArray();

                    for (var i = 0; i < inputs.Count; i++, ++inputs)
                    {
                        var current = inputs.CurrentCombination.ToArray();
                        var oneOfName = (string) current[0];
                        var oneOfFieldTuples = (FieldTupleType[]) current[1];
                        var optionNames = (OptionIdentifierPath[]) current[2];
                        yield return GetRange<object>(oneOfName, oneOfFieldTuples, optionNames, optionValues).ToArray();
                    }
                }

                return _privateTestCases ?? (_privateTestCases = GetAll().ToArray());
            }
        }

        protected override IEnumerable<object[]> RightHandSideCases => PrivateTestCases;
    }
}
