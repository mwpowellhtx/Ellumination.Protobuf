using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
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

        public static IEnumerable<object[]> SyntaxStatementWhiteSpaceAndCommentCases { get; }
            = new SyntaxStatementWhiteSpaceAndCommentTestCases().ToArray();

        [Theory
            , MemberData(nameof(SyntaxStatementWhiteSpaceAndCommentCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(SyntaxStatementWhiteSpaceAndCommentTestCases))
            ]
        public void VerifyStatementWithComments(WhiteSpaceAndCommentOption renderingOptions)
        {
            // Same as the Default Test, we need to evaluate Expected Proto.
            Assert.NotNull(ExpectedProto);

            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = renderingOptions};
        }
    }
}
