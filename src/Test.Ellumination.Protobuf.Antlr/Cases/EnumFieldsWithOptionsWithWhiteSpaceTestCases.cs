﻿using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using static CollectionHelpers;
    using static Domain;
    using static Identification;

    internal class EnumFieldsWithOptionsWithWhiteSpaceTestCases : TestCasesBase
    {
        private static IEnumerable<object[]> _privateCases;

        private static IEnumerable<object[]> GetBaseCases()
        {
            yield return GetRange<object>(
                GetIdent(3) // EnumName
                , GetFieldNames(GetRange(3)).ToArray() // FieldNames
                , GetOptionIdentifierPaths(GetRange(3), GetRange(3)
                    , GetRange(3), GetRange(3)).ToArray() // OptionNames
            ).ToArray();
        }

        protected override IEnumerable<object[]> Cases
            => _privateCases ?? (_privateCases
                   = InsertDimension(GetBaseCases(), AllWhiteSpaceAndCommentOptions).ToArray()
               );
    }
}
