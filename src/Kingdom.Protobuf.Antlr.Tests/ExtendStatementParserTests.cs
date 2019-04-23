using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Kingdom.Collections.Variants;
    using Xunit;
    using Xunit.Abstractions;
    using static CollectionHelpers;
    using static Domain;

    public class ExtendStatementParserTests : ProtoParserTestFixtureBase<ProtoLexer, ProtoDescriptorListener>
    {
        public ExtendStatementParserTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        // TODO: TBD: refactor to an Empty Body test fixture...
        /// <summary>
        /// Verifies that <see cref="ExtendStatement"/> with Empty
        /// <see cref="ExtendStatement.Items"/>.
        /// </summary>
        /// <param name="messageType"></param>
        [Theory, ClassData(typeof(BasicExtendStatementTestCases))]
        public void VerifyEmptyBody(ElementTypeIdentifierPath messageType)
        {
            ExpectedTopLevel = new ExtendStatement {MessageType = messageType};
        }

        [Theory, ClassData(typeof(BasicExtendStatementWithWhiteSpaceTestCases))]
        public void VerifyEmptyBodyWithWhiteSpace(ElementTypeIdentifierPath messageType
            , WhiteSpaceAndCommentOption option)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = option};
            VerifyEmptyBody(messageType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="fieldName"></param>
        /// <param name="optionName"></param>
        [Theory, ClassData(typeof(ExtendSingleFieldFieldsWithSingleOptionTestCases))]
        public void VerifySingleFieldFieldsWithSingleOption(ElementTypeIdentifierPath messageType, string fieldName
            , OptionIdentifierPath optionName)
        {
            const LabelKind label = LabelKind.Required;
            IVariant fieldType = Variant.Create(ProtoType.Double);
            IConstant optionConst = Constant.Create(true);

            IEnumerable<FieldOption> GetFieldOptions() => GetRange(
                new FieldOption {Name = optionName, Value = optionConst}
            );

            IEnumerable<NormalFieldStatement> GetNormalFields()
            {
                var fieldNumber = FieldNumber;

                yield return new NormalFieldStatement
                {
                    Name = fieldName,
                    Label = label,
                    FieldType = fieldType,
                    Number = fieldNumber,
                    Options = GetFieldOptions().ToList()
                };
            }

            ExpectedTopLevel = new ExtendStatement
            {
                MessageType = messageType,
                Items = GetNormalFields().ToList<IExtendBodyItem>()
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="fieldName"></param>
        /// <param name="optionName"></param>
        /// <param name="whiteSpaceOption"></param>
        [Theory, ClassData(typeof(ExtendSingleFieldFieldsWithSingleOptionWithWhiteSpaceTestCases))]
        public void VerifySingleFieldFieldsWithSingleOptionWithWhiteSpace(ElementTypeIdentifierPath messageType
            , string fieldName, OptionIdentifierPath optionName, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifySingleFieldFieldsWithSingleOption(messageType, fieldName, optionName);
        }

        /// <summary>
        /// <see cref="NormalFieldStatement"/> combinations are exhaustively verified in the scope
        /// of <see cref="MessageStatement"/> and <see cref="GroupFieldStatement"/> tests. No need
        /// to repeat ourselves here. Instead we want to focus on whether they can be parsed with
        /// or without options for <see cref="ExtendStatement"/> purposes.
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="fieldNames"></param>
        /// <param name="optionNames"></param>
        [Theory, ClassData(typeof(ExtendFromZeroFieldsWithOrWithoutOptionsTestCases))]
        public void VerifyFromZeroFieldsWithOrWithoutOptions(ElementTypeIdentifierPath messageType
            , string[] fieldNames, OptionIdentifierPath[] optionNames)
        {
            const LabelKind label = LabelKind.Required;
            IVariant fieldType = Variant.Create(ProtoType.Double);
            IConstant optionConst = Constant.Create(true);

            IEnumerable<FieldOption> GetFieldOptions() => optionNames.Select(
                x => new FieldOption {Name = x, Value = optionConst}
            );

            IEnumerable<NormalFieldStatement> GetNormalFields()
            {
                foreach (var fieldName in fieldNames)
                {
                    var fieldNumber = FieldNumber;

                    yield return new NormalFieldStatement
                    {
                        Name = fieldName,
                        Label = label,
                        FieldType = fieldType,
                        Number = fieldNumber,
                        Options = GetFieldOptions().ToList()
                    };
                }
            }

            ExpectedTopLevel = new ExtendStatement
            {
                MessageType = messageType,
                Items = GetNormalFields().ToList<IExtendBodyItem>()
            };
        }

        [Theory, ClassData(typeof(ExtendFromZeroFieldsWithOrWithoutOptionsWithWhiteSpaceTestCases))]
        public void VerifyFromZeroFieldsWithOrWithoutOptionsWithWhiteSpace(ElementTypeIdentifierPath messageType
            , string[] fieldNames, OptionIdentifierPath[] optionNames, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            VerifyFromZeroFieldsWithOrWithoutOptions(messageType, fieldNames, optionNames);
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
        }

        /// <summary>
        /// As with similar Test Cases, we are not here to verify the
        /// <see cref="IMessageBodyItem"/> aspects of each <see cref="GroupFieldStatement"/>.
        /// We are just here to ensure that it parses in the context of an
        /// <see cref="ExtendStatement"/>.
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="label"></param>
        /// <param name="groupNames"></param>
        /// <param name="fieldNumber"></param>
        [Theory, ClassData(typeof(ExtendFromZeroGroupsTestCases))]
        public void VerifyFromZeroGroups(ElementTypeIdentifierPath messageType, LabelKind label
            , string[] groupNames, long fieldNumber)
        {
            IEnumerable<IExtendBodyItem> GetStatementItems() => groupNames.Select<string, IExtendBodyItem>(
                x => new GroupFieldStatement {Name = x, Label = label, Number = fieldNumber}
            );

            ExpectedTopLevel = new ExtendStatement {MessageType = messageType, Items = GetStatementItems().ToList()};
        }

        [Theory, ClassData(typeof(ExtendFromZeroGroupsWithWhiteSpaceTestCases))]
        public void VerifyFromZeroGroupsWithWhiteSpace(ElementTypeIdentifierPath messageType, LabelKind label
            , string[] groupNames, long fieldNumber, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyFromZeroGroups(messageType, label, groupNames, fieldNumber);
        }
    }
}
