using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Ellumination.Collections.Variants;
    using static CollectionHelpers;

    internal class OptionStatementBooleanConstantTestCases : OptionStatementTestCasesBase
    {
        protected override IEnumerable<IVariant> GetConstants()
            => GetRange(true, false)
                .Select<bool, IVariant>(Constant.Create);
    }
}
