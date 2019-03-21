using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Combinatorics.Combinatorials;
    using static Collections;
    using static Domain;
    using static Identification;

    internal class ExtendFromZeroGroupsTestCases : TestCasesBase
    {
        private delegate int CalculateExpectedLengthCallback(int length);

        private static IEnumerable<object[]> _privateCases;

        private static IEnumerable<object[]> PrivateCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    var fieldNumber = FieldNumber;

                    IEnumerable<string[]> GetAllGroupNames(IEnumerable<int> lengths,
                        params CalculateExpectedLengthCallback[] callbacks)
                    {
                        var groupNames = GetGroupNames(lengths).ToArray();
                        foreach (var callback in callbacks)
                        {
                            var length = callback(groupNames.Length);
                            yield return groupNames.Take(length).ToArray();
                        }
                    }

                    var inputs = ElementTypes.ToArray<object>() // MessageType
                        .Combine(
                            LabelKinds.Select(x => (object) x).ToArray() // Label
                            , GetAllGroupNames(GetRange(1, 3), _ => 0, x => x / 2, x => x).ToArray<object>() // GroupNames
                        );

                    inputs.SilentOverflow = true;

                    for (var i = 0; i < inputs.Count; i++, ++inputs)
                    {
                        var current = inputs.CurrentCombination.ToArray();
                        var messageType = (ElementTypeIdentifierPath) current[0];
                        var label = (LabelKind) current[1];
                        var groupNames = (string[]) current[2];
                        yield return GetRange<object>(messageType, label, groupNames, fieldNumber).ToArray();
                    }
                }

                return _privateCases ?? (_privateCases = GetAll().ToArray());
            }
        }

        /// <inheritdoc />
        protected override IEnumerable<object[]> Cases => PrivateCases;
    }
}
