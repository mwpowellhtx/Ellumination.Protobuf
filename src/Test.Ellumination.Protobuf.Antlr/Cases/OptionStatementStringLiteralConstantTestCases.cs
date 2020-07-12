using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Ellumination.Collections.Variants;
    using static CollectionHelpers;
    using static Domain;

    internal class OptionStatementStringLiteralConstantTestCases : OptionStatementTestCasesBase
    {
        protected override IEnumerable<IVariant> GetConstants()
            => GetRange(NewIdentityString, NewIdentityString)
                .Select<string, IVariant>(Constant.Create);
    }
}
