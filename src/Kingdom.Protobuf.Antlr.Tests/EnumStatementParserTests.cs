using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Combinatorics.Combinatorials;
    using Kingdom.Collections.Variants;
    using Xunit;
    using Xunit.Abstractions;

    public class EnumStatementParserTests : ProtoParserTestFixtureBase<ProtoLexer, ProtoDescriptorListener>
    {
        public EnumStatementParserTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        // TODO: TBD: may refactor to an "empty body" test fixture...
        /// <summary>
        /// An <see cref="EnumStatement"/> with an Empty <see cref="IList{T}"/> of type
        /// <see cref="IEnumBodyItem"/> should parse correctly. That is, literally, the
        /// Body contains no Items.
        /// </summary>
        /// <param name="enumName"></param>
        [Theory, ClassData(typeof(BasicEnumStatementTestCases))]
        public void VerifyEmptyBody(string enumName)
        {
            ExpectedTopLevel = new EnumStatement {Name = enumName};
        }

        [Theory, ClassData(typeof(BasicEnumStatementWithWhiteSpaceTestCases))]
        public void VerifyEmptyBodyWithWhiteSpace(string enumName, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyEmptyBody(enumName);
        }

        /// <summary>
        /// Does not really matter what we call <see cref="optionNames"/> or
        /// <see cref="optionConsts"/> since we are not here to verify that. This should already
        /// be working via the <see cref="OptionStatement"/> tests. Therefore, we should keep that
        /// cross section to an absolute minimum. For instance, we may focus only on
        /// <see cref="IVariant{Boolean}"/> and keep things brief as possible.
        /// </summary>
        /// <param name="enumName"></param>
        /// <param name="optionNames"></param>
        /// <param name="optionConsts"></param>
        [Theory, ClassData(typeof(EnumWithOptionStatementTestCases))]
        public void VerifyWithOptionStatement(string enumName, OptionIdentifierPath[] optionNames
            // ReSharper disable once IdentifierTypo
            , IVariant[] optionConsts)
        {
            IEnumerable<IEnumBodyItem> GetEnumBodyItems()
            {
                var inputs = optionNames.ToArray<object>() // optionName
                    .Combine(
                        optionConsts.ToArray<object>() // optionConst
                    );

                inputs.SilentOverflow = true;

                for (var i = 0; i < inputs.Count; i++, ++inputs)
                {
                    var current = inputs.CurrentCombination.ToArray();
                    var optionName = (OptionIdentifierPath) current[0];
                    var optionConst = (IVariant) current[1];
                    yield return new OptionStatement {Name = optionName, Value = optionConst};
                }
            }

            ExpectedTopLevel = new EnumStatement {Name = enumName, Items = GetEnumBodyItems().ToList()};
        }

        [Theory, ClassData(typeof(EnumWithOptionStatementWithWhiteSpaceTestCases))]
        public void VerifyWithOptionStatementWithWhiteSpace(string enumName, OptionIdentifierPath[] optionNames
            // ReSharper disable once IdentifierTypo
            , IVariant[] optionConsts, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyWithOptionStatement(enumName, optionNames, optionConsts);
        }

        /// <summary>
        /// An <see cref="EnumStatement"/> with At Least One <see cref="EnumFieldDescriptor"/>
        /// should parse correctly. As with other Enum Statement test cases, we are not here to
        /// further vet the Integer Literal parsing. That is fairly well thoroughly covered by
        /// virtue of <see cref="OptionStatement"/> test cases. Rather, our focus here is on
        /// <see cref="EnumStatement"/> and <see cref="EnumFieldDescriptor"/>.
        /// </summary>
        /// <param name="enumName"></param>
        /// <param name="fieldNames"></param>
        [Theory, ClassData(typeof(EnumWithEnumFieldsTestCases))]
        public void VerifyEnumFields(string enumName, string[] fieldNames)
        {
            IEnumerable<IEnumBodyItem> GetEnumFields() => fieldNames.Select<string, IEnumBodyItem>(
                x => new EnumFieldDescriptor {Name = x}
            );

            ExpectedTopLevel = new EnumStatement {Name = enumName, Items = GetEnumFields().ToList()};
        }

        [Theory, ClassData(typeof(EnumWithEnumFieldsWithWhiteSpaceTestCases))]
        public void VerifyEnumFieldsWithWhiteSpace(string enumName, string[] fieldNames
            , WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyEnumFields(enumName, fieldNames);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumName"></param>
        /// <param name="fieldNames"></param>
        /// <param name="optionNames"></param>
        [Theory, ClassData(typeof(EnumFieldsWithOptionsTestCases))]
        public void VerifyEnumFieldsWithOptions(string enumName, string[] fieldNames
            , OptionIdentifierPath[] optionNames)
        {
            var optionConst = Constant.Create(true);

            IEnumerable<EnumValueOption> GetEnumValueOptions() => optionNames.Select(
                x => new EnumValueOption {Name = x, Value = optionConst}
            );

            IEnumerable<IEnumBodyItem> GetEnumFields() => fieldNames.Select<string, IEnumBodyItem>(
                x => new EnumFieldDescriptor {Name = x, Options = GetEnumValueOptions().ToList()}
            );

            ExpectedTopLevel = new EnumStatement {Name = enumName, Items = GetEnumFields().ToList()};
        }

        [Theory, ClassData(typeof(EnumFieldsWithOptionsWithWhiteSpaceTestCases))]
        public void VerifyEnumFieldsWithOptionsWithWhiteSpace(string enumName, string[] fieldNames
            , OptionIdentifierPath[] optionNames, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyEnumFieldsWithOptions(enumName, fieldNames, optionNames);
        }
    }
}
