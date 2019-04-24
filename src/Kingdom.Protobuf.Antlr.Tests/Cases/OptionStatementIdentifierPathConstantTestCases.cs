﻿using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Kingdom.Collections.Variants;
    using static CollectionHelpers;
    using static Identification;

    internal class OptionStatementIdentifierPathConstantTestCases : OptionStatementTestCasesBase
    {
        protected override IEnumerable<IVariant> GetConstants()
            => GetFullIdents(GetRange(1, 3), GetRange(1, 3))
                .Select<IdentifierPath, IVariant>(Constant.Create);
    }
}
