using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Combinatorics.Combinatorials;
    using static CollectionHelpers;
    using static Domain;
    using static Randomizer;
    using static FloatingPointRenderingOption;

    internal class OptionStatementFloatingPointLiteralConstantTestCases : OptionStatementTestCasesBase
    {
        private static IEnumerable<double> _privateValues;

        private static IEnumerable<double> PrivateValues
        {
            get
            {
                IEnumerable<double> GetAll()
                {
                    /* Including handful of obvious corner cases, such as Positive and Negative Infinity. Also Not
                     * a Number. Also represent cases that can obvious have a Preceding as well as a Trailing Dot. */

                    // Starting from a base Empty Set and Concat from there for Troubleshooting purposes.
                    foreach (var y in GetRange(NaN, NegativeInfinity, PositiveInfinity, ZedFloatingPoint)
                        .Concat(GetRange(Rnd.NextDouble(ZedFloatingPoint, OneFloatingPoint), Rnd.NextLong()))
                    )
                    {
                        yield return y;
                    }

                    // We do not need to go nearly as deep here.
                    const int depth = 2, count = 2;

                    foreach (var y in GetFloatingPointValues(ZedFloatingPoint, MaxFloatingPoint, depth, count)
                        .Concat(GetFloatingPointValues(MinFloatingPoint, ZedFloatingPoint, depth, count)))
                    {
                        yield return y;
                    }
                }

                return _privateValues ?? (_privateValues = GetAll().ToArray());
            }
        }

        protected override IEnumerable<IConstant> GetConstants()
            => PrivateValues.Select<double, IConstant>(Constant.Create);

        private static IEnumerable<IStringRenderingOptions> _renderingOptions;

        private static IEnumerable<IStringRenderingOptions> RenderingOptions
        {
            get
            {
                IEnumerable<IStringRenderingOptions> GetAll()
                {
                    // This is much more concise with a bit of strategic refactoring.
                    var inputs = FloatingPointFormatOptions.UnmaskOptions().Select(x => (object) x).ToArray()
                        .Combine(
                            FloatingPointCaseOptions.UnmaskOptions().Select(x => (object) x).ToArray()
                            , FloatingPointDottageOptions.UnmaskOptions().Select(x => (object) x).ToArray()
                            , FloatingPointForceSignage.UnmaskOptions().Select(x => (object) x).ToArray()
                        );

                    inputs.SilentOverflow = true;

                    for (var i = 0; i < inputs.Count; i++, ++inputs)
                    {
                        yield return new StringRenderingOptions
                        {
                            FloatingPointRendering = inputs.CurrentCombination.Select(
                                x => (FloatingPointRenderingOption) x).AggregateMask()
                        };
                    }
                }

                return _renderingOptions ?? (_renderingOptions = GetAll().ToArray());
            }
        }

        private IEnumerable<object[]> _privateCases;

        protected override IEnumerable<object[]> Cases
            => _privateCases ?? (_privateCases
                   = InsertDimension(base.Cases, RenderingOptions).ToArray());
    }
}
