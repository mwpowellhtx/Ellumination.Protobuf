using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Combinatorics.Combinatorials;
    using static CollectionHelpers;
    using static Domain;
    using static IntegerRenderingOption;

    internal class OptionStatementIntegerLiteralConstantTestCases : OptionStatementTestCasesBase
    {
        private IEnumerable<long> _privateValues;

        private IEnumerable<long> PrivateValues
        {
            get
            {
                IEnumerable<long> GetAll()
                {
                    // We do not need to go nearly as deep here.
                    const int depth = 2, count = 2;

                    foreach (var y in GetRange(ZedLong)
                        .Concat(GetLongValues(MinLong, ZedLong, depth, count))
                        .Concat(GetLongValues(ZedLong, MaxLong, depth, count)))
                    {
                        yield return y;
                    }
                }

                return _privateValues ?? (_privateValues = GetAll().ToArray());
            }
        }

        protected override IEnumerable<IConstant> GetConstants()
            => PrivateValues.Select<long, IConstant>(Constant.Create);

        private static IEnumerable<IStringRenderingOptions> _renderingOptions;

        private static IEnumerable<IStringRenderingOptions> RenderingOptions
        {
            get
            {
                IEnumerable<IStringRenderingOptions> GetAll()
                {
                    // Same as with Floating Point this is much more concise like this.
                    var inputs = IntegerFormatOptions.UnmaskOptions().Select(x => (object) x)
                        .Combine(
                            IntegerCaseOptions.UnmaskOptions().Select(x => (object) x)
                            , IntegerSignageOptions.UnmaskOptions().Select(x => (object) x)
                        );

                    inputs.SilentOverflow = true;

                    for (var i = 0; i < inputs.Count; i++, ++inputs)
                    {
                        yield return new StringRenderingOptions
                        {
                            IntegerRendering = inputs.CurrentCombination.Select(
                                x => (IntegerRenderingOption) x).AggregateMask()
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
