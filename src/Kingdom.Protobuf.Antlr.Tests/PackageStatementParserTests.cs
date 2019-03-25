using System;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Xunit;
    using Xunit.Abstractions;

    public class PackageStatementParserTests : ProtoParserTestFixtureBase<ProtoLexer, ProtoDescriptorListener>
    {
        public PackageStatementParserTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        /// <summary>
        /// We are not really here to verify FullIdent or even Ident behavior, but rather that
        /// Package can be parsed. We can save the Ident and FullIdent testing for another set
        /// of tests.
        /// </summary>
        /// <param name="packagePath"></param>
        [Theory, ClassData(typeof(PackageStatementTestCases))]
        public void VerifyGivenFullIdentifierSpecs(IdentifierPath packagePath)
        {
            ExpectedProto.Items.Add(new PackageStatement {PackagePath = packagePath});
        }

        [Theory, ClassData(typeof(PackageStatementTestCases))]
        public void CannotHaveMoreThanOneStatement(IdentifierPath packagePath)
        {
            var expectedProtoItems = ExpectedProto.Items;

            expectedProtoItems.Add(new PackageStatement {PackagePath = packagePath});
            expectedProtoItems.Add(new PackageStatement {PackagePath = packagePath});

            var expectedMessage = $"Cannot have more than one '{typeof(PackageStatement).FullName}'.";

            // Most times we can wait for Dispose to process the Expected Target, but this is an Exception.
            VerifyParse(x => x.ToDescriptorString()
                , (InvalidOperationException actualException)
                    => Assert.Equal(expectedMessage, actualException.VerifyNotNull().Message)
            );
        }

        [Theory, ClassData(typeof(PackageStatementWhiteSpaceAndCommentTestCases))]
        public void VerifyStatementWithComments(IdentifierPath packagePath, WhiteSpaceAndCommentOption options)
        {
            ExpectedProto.Items.Add(new PackageStatement {PackagePath = packagePath});
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = options};
        }
    }
}
