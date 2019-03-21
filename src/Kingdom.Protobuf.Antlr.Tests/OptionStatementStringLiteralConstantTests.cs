// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Xunit;
    using Xunit.Abstractions;
    using static Collections;

    public class OptionStatementStringLiteralConstantTests
        : ProtoParserTestFixtureBase<ProtoLexer, ProtoDescriptorListener>
    {
        public OptionStatementStringLiteralConstantTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        /// <summary>
        /// Verifies that <see cref="OptionStatement"/> <see cref="string"/>
        /// <see cref="IConstant"/> works correctly.
        /// </summary>
        /// <param name="optionName"></param>
        /// <param name="constant"></param>
        [Theory, ClassData(typeof(OptionStatementStringLiteralConstantTestCases))]
        public void VerifyStatement(OptionIdentifierPath optionName, IConstant constant)
        {
            GetRange<object>(optionName, constant).AllNotNull();

            ExpectedProto.Items.Add(
                new OptionStatement {Name = optionName, Value = Assert.IsAssignableFrom<IConstant<string>>(constant)}
            );
        }
    }
}
