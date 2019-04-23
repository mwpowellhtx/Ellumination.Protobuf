using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Combinatorics.Combinatorials;
    using static CollectionHelpers;
    using static Domain;
    using static Identification;

    internal abstract class GroupStatementMessageBodyTestCasesBase : MessageBodyTestCasesBase
    {
        private static IEnumerable<object[]> _privateMiddleParts;

        /// <summary>
        /// Middle Parts in the form of <see cref="LabelKind"/>
        /// <see cref="GroupFieldStatement.Label"/>, the <see cref="string"/>
        /// <see cref="DescriptorBase{T}.Name"/>, as well as the <see cref="long"/>
        /// <see cref="FieldStatementBase{T}.Number"/>.
        /// </summary>
        private static IEnumerable<object[]> PrivateMiddleParts
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

                return _privateMiddleParts
                       ?? (_privateMiddleParts = GetAll().ToArray());
            }
        }

        private IEnumerable<object[]> _cases;

        // ReSharper disable once CommentTypo
        // ReSharper disable once InheritdocConsiderUsage
        /// <summary>
        /// Gets the Cases particular to developing the <see cref="GroupFieldStatement"/> Tests.
        /// This properly informs both the parent <see cref="MessageStatement"/> as well as the
        /// intermediate <see cref="GroupFieldStatement"/>, as well as leaving room to override
        /// the <see cref="MessageBodyTestCasesBase.RightHandSideCases"/> for purposes of the
        /// specific test cases.
        /// </summary>
        protected override IEnumerable<object[]> Cases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    // TODO: TBD: may also re-factor "middle" (i.e. Group Name) into Test Fixture as well...
                    // Left-hand side refactored into Test Fixture itself.
                    var inputs = PrivateMiddleParts.ToArray<object>() // Middle Group Field Statement Parts
                        .Combine(
                            RightHandSideCases.ToArray<object>() // Test Specific Right Hand Side Parts
                        );

                    //var inputs = LeftHandSideCases.ToArray<object>() // Left Hand Side Message Parts
                    //    .Combine(
                    //        PrivateMiddleParts.ToArray<object>() // Middle Group Field Statement Parts
                    //        , RightHandSideCases.ToArray<object>() // Test Specific Right Hand Side Parts
                    //    );

                    inputs.SilentOverflow = true;

                    for (var i = 0; i < inputs.Count; i++, ++inputs)
                    {
                        var current = inputs.CurrentCombination.ToArray();
                        var lhs = (object[]) current[0];
                        var middle = (object[]) current[1];
                        var rhs = (object[]) current[2];
                        yield return lhs.Concat(middle).Concat(rhs).ToArray();
                    }
                }

                return _cases ?? (_cases = GetAll().ToArray());
            }
        }
    }
}
