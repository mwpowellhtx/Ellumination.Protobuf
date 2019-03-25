// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Xunit;
    using Xunit.Abstractions;

    public class SyntaxStatementParserTests : ProtoParserTestFixtureBase<ProtoLexer, ProtoDescriptorListener>
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

        [Theory, ClassData(typeof(SyntaxStatementWhiteSpaceAndCommentTestCases))]
        public void VerifyStatementWithComments(WhiteSpaceAndCommentOption renderingOptions)
        {
            // Same as the Default Test, we need to evaluate Expected Proto.
            Assert.NotNull(ExpectedProto);

            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = renderingOptions};
        }
    }
}
