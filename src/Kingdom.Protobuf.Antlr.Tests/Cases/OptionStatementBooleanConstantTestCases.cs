using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static Collections;

    internal class OptionStatementBooleanConstantTestCases : OptionStatementTestCasesBase
    {
        protected override IEnumerable<IConstant> GetConstants()
            => GetRange(true, false)
                .Select<bool, IConstant>(Constant.Create);
    }
}
