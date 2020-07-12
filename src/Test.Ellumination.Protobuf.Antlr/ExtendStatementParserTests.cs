using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Ellumination.Collections.Variants;
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

        public static IEnumerable<object[]> BasicExtendStatementCases { get; }
            = new BasicExtendStatementTestCases().ToArray();

        // TODO: TBD: refactor to an Empty Body test fixture...
        /// <summary>
        /// Verifies that <see cref="ExtendStatement"/> with Empty
        /// <see cref="ExtendStatement.Items"/>.
        /// </summary>
        /// <param name="messageType"></param>
        [Theory
            , MemberData(nameof(BasicExtendStatementCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(BasicExtendStatementTestCases))
            ]
        public void VerifyEmptyBody(ElementTypeIdentifierPath messageType)
        {
            ExpectedTopLevel = new ExtendStatement {MessageType = messageType};
        }

        public static IEnumerable<object[]> BasicExtendStatementWithWhiteSpaceCases { get; }
            = new BasicExtendStatementWithWhiteSpaceTestCases().ToArray();

        [Theory
            , MemberData(nameof(BasicExtendStatementWithWhiteSpaceCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(BasicExtendStatementWithWhiteSpaceTestCases))
            ]
        public void VerifyEmptyBodyWithWhiteSpace(ElementTypeIdentifierPath messageType
            , WhiteSpaceAndCommentOption option)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = option};
            VerifyEmptyBody(messageType);
        }

        public static IEnumerable<object[]> ExtendSingleFieldFieldsWithSingleOptionCases { get; }
            = new ExtendSingleFieldFieldsWithSingleOptionTestCases().ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="fieldName"></param>
        /// <param name="optionName"></param>
        [Theory
            , MemberData(nameof(ExtendSingleFieldFieldsWithSingleOptionCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(ExtendSingleFieldFieldsWithSingleOptionTestCases))
            ]
        public void VerifySingleFieldFieldsWithSingleOption(ElementTypeIdentifierPath messageType, string fieldName
            , OptionIdentifierPath optionName)
        {
            const LabelKind label = LabelKind.Required;
            IVariant fieldType = Variant.Create(ProtoType.Double);
            IVariant optionConst = Constant.Create(true);

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

        public static IEnumerable<object[]> ExtendSingleFieldFieldsWithSingleOptionWithWhiteSpaceCases { get; }
            = new ExtendSingleFieldFieldsWithSingleOptionWithWhiteSpaceTestCases().ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="fieldName"></param>
        /// <param name="optionName"></param>
        /// <param name="whiteSpaceOption"></param>
        [Theory
            , MemberData(nameof(ExtendSingleFieldFieldsWithSingleOptionWithWhiteSpaceCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(ExtendSingleFieldFieldsWithSingleOptionWithWhiteSpaceTestCases))
            ]
        public void VerifySingleFieldFieldsWithSingleOptionWithWhiteSpace(ElementTypeIdentifierPath messageType
            , string fieldName, OptionIdentifierPath optionName, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifySingleFieldFieldsWithSingleOption(messageType, fieldName, optionName);
        }

        public static IEnumerable<object[]> ExtendFromZeroFieldsWithOrWithoutOptionsCases { get; }
            = new ExtendFromZeroFieldsWithOrWithoutOptionsTestCases().ToArray();

        /// <summary>
        /// <see cref="NormalFieldStatement"/> combinations are exhaustively verified in the scope
        /// of <see cref="MessageStatement"/> and <see cref="GroupFieldStatement"/> tests. No need
        /// to repeat ourselves here. Instead we want to focus on whether they can be parsed with
        /// or without options for <see cref="ExtendStatement"/> purposes.
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="fieldNames"></param>
        /// <param name="optionNames"></param>
        [Theory
            , MemberData(nameof(ExtendFromZeroFieldsWithOrWithoutOptionsCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(ExtendFromZeroFieldsWithOrWithoutOptionsTestCases))
            ]
        public void VerifyFromZeroFieldsWithOrWithoutOptions(ElementTypeIdentifierPath messageType
            , string[] fieldNames, OptionIdentifierPath[] optionNames)
        {
            const LabelKind label = LabelKind.Required;
            IVariant fieldType = Variant.Create(ProtoType.Double);
            IVariant optionConst = Constant.Create(true);

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

        public static IEnumerable<object[]> ExtendFromZeroFieldsWithOrWithoutOptionsWithWhiteSpaceCases { get; }
            = new ExtendFromZeroFieldsWithOrWithoutOptionsWithWhiteSpaceTestCases().ToArray();

        [Theory
            , MemberData(nameof(ExtendFromZeroFieldsWithOrWithoutOptionsWithWhiteSpaceCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(ExtendFromZeroFieldsWithOrWithoutOptionsWithWhiteSpaceTestCases))
            ]
        public void VerifyFromZeroFieldsWithOrWithoutOptionsWithWhiteSpace(ElementTypeIdentifierPath messageType
            , string[] fieldNames, OptionIdentifierPath[] optionNames, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            VerifyFromZeroFieldsWithOrWithoutOptions(messageType, fieldNames, optionNames);
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
        }

        public static IEnumerable<object[]> ExtendFromZeroGroupsCases { get; }
            = new ExtendFromZeroGroupsTestCases().ToArray();

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
        [Theory
            , MemberData(nameof(ExtendFromZeroGroupsCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(ExtendFromZeroGroupsTestCases))
            ]
        public void VerifyFromZeroGroups(ElementTypeIdentifierPath messageType, LabelKind label
            , string[] groupNames, long fieldNumber)
        {
            IEnumerable<IExtendBodyItem> GetStatementItems() => groupNames.Select<string, IExtendBodyItem>(
                x => new GroupFieldStatement {Name = x, Label = label, Number = fieldNumber}
            );

            ExpectedTopLevel = new ExtendStatement {MessageType = messageType, Items = GetStatementItems().ToList()};
        }

        public static IEnumerable<object[]> ExtendFromZeroGroupsWithWhiteSpaceCases { get; }
            = new ExtendFromZeroGroupsWithWhiteSpaceTestCases().ToArray();

        [Theory
            , MemberData(nameof(ExtendFromZeroGroupsWithWhiteSpaceCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(ExtendFromZeroGroupsWithWhiteSpaceTestCases))
            ]
        public void VerifyFromZeroGroupsWithWhiteSpace(ElementTypeIdentifierPath messageType, LabelKind label
            , string[] groupNames, long fieldNumber, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyFromZeroGroups(messageType, label, groupNames, fieldNumber);
        }
    }
}
