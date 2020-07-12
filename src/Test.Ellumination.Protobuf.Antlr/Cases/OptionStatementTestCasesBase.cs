using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Ellumination.Collections.Variants;
    using Kingdom.Combinatorics.Combinatorials;
    using static CollectionHelpers;
    using static Domain;

    internal abstract class OptionStatementTestCasesBase : TestCasesBase
    {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Returns the set of <see cref="IVariant"/> Values.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<IVariant> GetConstants();

        private IEnumerable<IVariant> _constants;

        protected IEnumerable<IVariant> Constants => _constants ?? (_constants = GetConstants().ToArray());

        private static IEnumerable<OptionIdentifierPath> _optionNames;

        private static IEnumerable<OptionIdentifierPath> OptionNames
        {
            get
            {
                IEnumerable<OptionIdentifierPath> GetAll()
                    => GetOptionIdentifierPaths(GetRange(3), GetRange(3)
                        , GetRange(3), GetRange(0, 3));

                return _optionNames ?? (_optionNames = GetAll().ToArray());
            }
        }

        private IEnumerable<object[]> _cases;

        protected override IEnumerable<object[]> Cases
        {
            get
            {
                IEnumerable<object[]> GetAll()
                {
                    var inputs = OptionNames.ToArray<object>() // OptionName
                        .Combine(
                            Constants.ToArray<object>() // Constant
                        );

                    inputs.SilentOverflow = true;

                    for (var i = 0; i < inputs.Count; i++, ++inputs)
                    {
                        yield return inputs.CurrentCombination.ToArray();
                    }
                }

                return _cases ?? (_cases = GetAll().ToArray());
            }
        }
    }
}
