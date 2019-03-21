using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static Collections;
    using static Identification;

    internal class OptionStatementIdentifierPathConstantTestCases : OptionStatementTestCasesBase
    {
        protected override IEnumerable<IConstant> GetConstants()
            => GetFullIdents(GetRange(1, 3), GetRange(1, 3))
                .Select<IdentifierPath, IConstant>(Constant.Create);
    }
}
