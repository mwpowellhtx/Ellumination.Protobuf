using System.Collections.Generic;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
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

        /// <summary>
        /// The <see cref="IList{T}"/> of type <see cref="IEnumBodyItem"/> Contains One
        /// <see cref="EmptyStatement"/> Item.
        /// </summary>
        /// <param name="enumName"></param>
        [Theory, ClassData(typeof(EmptyEnumStatementTestCases))]
        public void VerifyEnumStatementWithEmptyStatement(string enumName)
        {
            ExpectedTopLevel = new EnumStatement {Name = enumName, Items = {NewEmptyStatement}};
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageType"></param>
        [Theory, ClassData(typeof(BasicExtendStatementTestCases))]
        public void VerifyExtendStatementWithEmptyStatement(ElementTypeIdentifierPath messageType)
        {
            ExpectedTopLevel = new ExtendStatement {MessageType = messageType, Items = {NewEmptyStatement}};
        }
    }
}
