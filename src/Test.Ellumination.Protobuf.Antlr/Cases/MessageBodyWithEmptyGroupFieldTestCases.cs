using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Kingdom.Combinatorics.Combinatorials;
    using static CollectionHelpers;
    using static Domain;
    using static Identification;

    internal class MessageBodyWithEmptyGroupFieldTestCases : MessageBodyTestCasesBase
    {
        private static IEnumerable<object[]> _privateTestCases;

        private static IEnumerable<object[]> PrivateTestCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    var fieldNumber = FieldNumber;

                    var inputs = LabelKinds.Select(x => (object) x).ToArray() // Label
                        .Combine(
                            GetGroupNames(GetRange(1, 3)).ToArray<object>() // GroupName
                        );

                    inputs.SilentOverflow = true;

                    for (var i = 0; i < inputs.Count; i++, ++inputs)
                    {
                        var current = inputs.CurrentCombination.ToArray();
                        var label = (LabelKind) current[0];
                        var groupName = (string) current[1];
                        yield return GetRange<object>(label, groupName, fieldNumber).ToArray();
                    }
                }

                return _privateTestCases ?? (_privateTestCases = GetAll().ToArray());
            }
        }

        protected override IEnumerable<object[]> RightHandSideCases => PrivateTestCases;
    }
}
