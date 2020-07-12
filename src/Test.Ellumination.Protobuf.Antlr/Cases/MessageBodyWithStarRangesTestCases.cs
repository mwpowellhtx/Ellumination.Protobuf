using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Kingdom.Combinatorics.Combinatorials;
    using static CollectionHelpers;
    using static Domain;
    using static FieldNumbers;
    using static Math;

    internal class MessageBodyWithStarRangesTestCases : MessageBodyTestCasesBase
    {
        private static IEnumerable<object[]> _privateTestCases;

        private static IEnumerable<object[]> PrivateTestCases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    IEnumerable<Tuple<long, long?>> GetRangeTuples()
                    {
                        const long minimum = MinimumFieldNumber;
                        var maximum = MaximumFieldNumber;

                        const int depth = 2, count = 2;

                        IEnumerable<long?> GetMaximumValues()
                        {
                            foreach (var x in GetLongValues(minimum, maximum - 1, depth, count))
                            {
                                yield return x;
                            }

                            yield return MaximumFieldNumber;
                            yield return null;
                        }

                        Tuple<long, long?> BuildNormalizedTuple(ref long min, ref long? max)
                        {
                            // ReSharper disable once InvertIf
                            if (max.HasValue)
                            {
                                var x = Min(min, max.Value);
                                var y = Max(min, max.Value);
                                min = x;
                                max = y;
                            }

                            return Tuple.Create(min, max);
                        }

                        var inputs = GetLongValues(minimum, maximum - 1, depth, count).Select(x => (object) x).ToArray() // Minimum
                            .Combine(
                                GetMaximumValues().Select(x => (object) x).ToArray() // Maximum
                            );

                        inputs.SilentOverflow = true;

                        for (var i = 0; i < inputs.Count; i++, ++inputs)
                        {
                            var current = inputs.CurrentCombination.ToArray();
                            var currentMin = (long) current[0];
                            var currentMax = (long?) current[1];
                            yield return BuildNormalizedTuple(ref currentMin, ref currentMax);
                        }
                    }

                    foreach (object currentTuples in GetRangeTuples().Stagger(_ => 1, x => x / 2, x => x))
                    {
                        yield return GetRange(currentTuples).ToArray();
                    }
                }

                return _privateTestCases ?? (_privateTestCases = GetAll().ToArray());
            }
        }

        protected override IEnumerable<object[]> RightHandSideCases => PrivateTestCases;
    }
}
