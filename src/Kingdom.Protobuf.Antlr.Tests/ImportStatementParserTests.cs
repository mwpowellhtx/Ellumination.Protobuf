// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Xunit;
    using Xunit.Abstractions;
    using static Domain;

    public class ImportStatementParserTests : ProtoParserTestFixtureBase<ProtoLexer, ProtoDescriptorListener>
    {
        public ImportStatementParserTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        /// <summary>
        /// We are not here to verify that an ImportPath is valid, per se, but rather that
        /// the overall statement parses as expected.
        /// </summary>
        /// <param name="modifier"></param>
        [Theory, ClassData(typeof(ImportStatementTestCases))]
        public void VerifyWithOrWithoutModifier(ImportModifierKind? modifier)
        {
            ExpectedProto.Items.Add(
                new ImportStatement {ImportPath = NewIdentityString, Modifier = modifier}
            );
        }

        [Theory, ClassData(typeof(ImportStatementWithWhiteSpaceTestCases))]
        public void VerifyWhiteSpaceAndComments(ImportModifierKind? modifier, WhiteSpaceAndCommentOption option)
        {
            ExpectedProto.Items.Add(
                new ImportStatement {ImportPath = NewIdentityString, Modifier = modifier}
            );

            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = option};
        }
    }
}
