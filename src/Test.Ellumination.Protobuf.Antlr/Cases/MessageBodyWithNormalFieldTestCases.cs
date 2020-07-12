using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Ellumination.Collections.Variants;
    using Kingdom.Combinatorics.Combinatorials;
    using static CollectionHelpers;
    using static Domain;
    using static Identification;

    internal class MessageBodyWithNormalFieldTestCases : MessageBodyTestCasesBase
    {
        private static IEnumerable<object[]> _privateTestCases;

        private static IEnumerable<object[]> PrivateTestCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    var fieldNumber = FieldNumber;

                    var inputs = GetIdents(GetRange(1, 3)).ToArray<object>() // MessageName
                        .Combine(
                            LabelKinds.Select(x => (object) x).ToArray() // Label
                            , Types.ToArray<object>() // TypeValue: IVariant of ProtoType, ElementTypePath
                            , GetFieldNames(GetRange(1, 3)).ToArray<object>() // FieldName
                        );

                    inputs.SilentOverflow = true;

                    for (var i = 0; i < inputs.Count; i++, ++inputs)
                    {
                        var current = inputs.CurrentCombination.ToArray();
                        var label = (LabelKind) current[1];
                        var fieldType = (IVariant) current[2];
                        var fieldName = (string) current[3];
                        yield return GetRange<object>(label, fieldType, fieldName, fieldNumber).ToArray();
                    }
                }

                return _privateTestCases ?? (_privateTestCases = GetAll().ToArray());
            }
        }

        protected override IEnumerable<object[]> RightHandSideCases => PrivateTestCases;
    }
}
