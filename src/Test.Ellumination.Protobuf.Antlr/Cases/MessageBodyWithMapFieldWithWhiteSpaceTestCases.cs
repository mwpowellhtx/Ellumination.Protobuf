using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Ellumination.Collections.Variants;
    using static CollectionHelpers;
    using static Domain;
    using static Identification;

    internal class MessageBodyWithMapFieldWithWhiteSpaceTestCases : TestCasesBase
    {
        private static IEnumerable<object[]> _privateCases;

        private static IEnumerable<object[]> GetBaseCases()
        {
            yield return GetRange<object>(
                KeyType.Bool // KeyType
                , Variant.Create(ProtoType.Bool) // ValueType
                , GetIdent(3) // MapName
                , FieldNumber // FieldNumber
                , GetOptionIdentifierPaths(GetRange(3), GetRange(3)
                    , GetRange(3), GetRange(3)).ToArray() // OptionNames
                , GetRange<IVariant>(Constant.Create(true)).ToArray() // OptionValues
            ).ToArray();
        }

        protected override IEnumerable<object[]> Cases
            => _privateCases ?? (_privateCases
                   = InsertDimension(GetBaseCases(), AllWhiteSpaceAndCommentOptions).ToArray()
               );
    }
}
