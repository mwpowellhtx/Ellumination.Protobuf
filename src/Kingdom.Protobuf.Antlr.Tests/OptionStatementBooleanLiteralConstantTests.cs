// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Xunit;
    using Xunit.Abstractions;
    using static Collections;

    public class OptionStatementBooleanLiteralConstantTests
        : ProtoParserTestFixtureBase<ProtoLexer, ProtoDescriptorListener>
    {
        public OptionStatementBooleanLiteralConstantTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        /// <summary>
        /// Verifies the <see cref="bool"/> <see cref="IConstant"/> variant works correctly.
        /// </summary>
        /// <param name="optionName"></param>
        /// <param name="constant"></param>
        [Theory, ClassData(typeof(OptionStatementBooleanConstantTestCases))]
        public void VerifyStatement(OptionIdentifierPath optionName, IConstant constant)
        {
            GetRange<object>(optionName, constant).AllNotNull();

            ExpectedProto.Items.Add(
                new OptionStatement {Name = optionName, Value = Assert.IsAssignableFrom<IConstant<bool>>(constant)}
            );
        }
    }
}
