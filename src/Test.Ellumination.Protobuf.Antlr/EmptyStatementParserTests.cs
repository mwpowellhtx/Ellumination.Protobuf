using System.Collections.Generic;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using System.Linq;
    using Xunit;
    using Xunit.Abstractions;
    using static Statements;

    public class EmptyStatementParserTests
        : ProtoParserTestFixtureBase<ProtoLexer, ProtoDescriptorListener>
    {
        public EmptyStatementParserTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public void VerifyProtoLevelEmptyStatement()
        {
            ExpectedProto.Items.Add(NewEmptyStatement);
        }

        public static IEnumerable<object[]> EnumStatementWithEmptyStatementCases { get; } = new EmptyEnumStatementTestCases().ToArray();

        /// <summary>
        /// The <see cref="IList{T}"/> of type <see cref="IEnumBodyItem"/> Contains One
        /// <see cref="EmptyStatement"/> Item.
        /// </summary>
        /// <param name="enumName"></param>
        [Theory
            , MemberData(nameof(EnumStatementWithEmptyStatementCases), DisableDiscoveryEnumeration = true)
            ]
        public void VerifyEnumStatementWithEmptyStatement(string enumName)
        {
            ExpectedTopLevel = new EnumStatement {Name = enumName, Items = {NewEmptyStatement}};
        }

        public static IEnumerable<object[]> EmptyEnumStatementWithWhiteSpaceCases { get; } = new EmptyEnumStatementWithWhiteSpaceTestCases().ToArray();

        [Theory
            //, ClassData(typeof(EmptyEnumStatementWithWhiteSpaceTestCases))
            , MemberData(nameof(EmptyEnumStatementWithWhiteSpaceCases), DisableDiscoveryEnumeration = true)
            ]
        public void VerifyEnumStatementWithEmptyStatementAndWhiteSpace(string enumName, WhiteSpaceAndCommentOption option)
        {
            ExpectedTopLevel = new EnumStatement {Name = enumName, Items = {NewEmptyStatement}};
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = option};
        }

        public static IEnumerable<object[]> BasicExtendStatementCases { get; } = new BasicExtendStatementTestCases().ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageType"></param>
        [Theory
                //, ClassData(typeof(BasicExtendStatementTestCases))
                , MemberData(nameof(BasicExtendStatementCases), DisableDiscoveryEnumeration = true)
            ]
        public void VerifyExtendStatementWithEmptyStatement(ElementTypeIdentifierPath messageType)
        {
            ExpectedTopLevel = new ExtendStatement {MessageType = messageType, Items = {NewEmptyStatement}};
        }
    }
}
