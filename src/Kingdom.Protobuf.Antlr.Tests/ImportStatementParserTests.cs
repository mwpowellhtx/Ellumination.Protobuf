// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Xunit;
    using Xunit.Abstractions;
    using static Domain;
    using static ImportModifierKind;

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
        [Theory
         , InlineData(null)
         , InlineData(Weak)
         , InlineData(Public)]
        public void VerifyWithOrWithoutModifier(ImportModifierKind? modifier)
        {
            ExpectedProto.Items.Add(
                new ImportStatement {ImportPath = NewIdentityString, Modifier = modifier}
            );
        }
    }
}
