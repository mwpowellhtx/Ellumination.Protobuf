// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Xunit;
    using Xunit.Abstractions;

    public class SyntaxStatementParserTests
        : ProtoParserTestFixtureBase<ProtoLexer, ProtoDescriptorListener>
    {
        public SyntaxStatementParserTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public void VerifyDefaultStatement()
        {
            // We do not need to do anything, but we do need to ensure it has an instance backing.
            Assert.NotNull(ExpectedProto);
        }
    }
}
