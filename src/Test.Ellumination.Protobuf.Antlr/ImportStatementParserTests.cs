// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Xunit.Abstractions;
    using static Domain;

    public class ImportStatementParserTests : ProtoParserTestFixtureBase<ProtoLexer, ProtoDescriptorListener>
    {
        public ImportStatementParserTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        public static IEnumerable<object[]> ImportStatementCases { get; }
            = new ImportStatementTestCases().ToArray();

        /// <summary>
        /// We are not here to verify that an ImportPath is valid, per se, but rather that
        /// the overall statement parses as expected.
        /// </summary>
        /// <param name="modifier"></param>
        [Theory
            , MemberData(nameof(ImportStatementCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(ImportStatementTestCases))
            ]
        public void VerifyWithOrWithoutModifier(ImportModifierKind? modifier)
        {
            ExpectedProto.Items.Add(
                new ImportStatement {ImportPath = NewIdentityString, Modifier = modifier}
            );
        }

        public static IEnumerable<object[]> ImportStatementWithWhiteSpaceCases { get; }
            = new ImportStatementWithWhiteSpaceTestCases().ToArray();

        [Theory
            , MemberData(nameof(ImportStatementWithWhiteSpaceCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(ImportStatementWithWhiteSpaceTestCases))
            ]
        public void VerifyWhiteSpaceAndComments(ImportModifierKind? modifier, WhiteSpaceAndCommentOption option)
        {
            ExpectedProto.Items.Add(
                new ImportStatement {ImportPath = NewIdentityString, Modifier = modifier}
            );

            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = option};
        }
    }
}
