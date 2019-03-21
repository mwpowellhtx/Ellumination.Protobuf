using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static Collections;
    using static Domain;

    internal class OptionStatementStringLiteralConstantTestCases : OptionStatementTestCasesBase
    {
        protected override IEnumerable<IConstant> GetConstants()
            => GetRange(NewIdentityString, NewIdentityString)
                .Select<string, IConstant>(Constant.Create);
    }
}
