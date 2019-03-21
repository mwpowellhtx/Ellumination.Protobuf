// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Xunit;
    using Xunit.Abstractions;
    using static Collections;

    public class OptionStatementIdentifierPathConstantTests
        : ProtoParserTestFixtureBase<ProtoLexer, ProtoDescriptorListener>
    {
        public OptionStatementIdentifierPathConstantTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionName"></param>
        /// <param name="constant"></param>
        [Theory, ClassData(typeof(OptionStatementIdentifierPathConstantTestCases))]
        public void VerifyStatement(OptionIdentifierPath optionName, IConstant constant)
        {
            GetRange<object>(optionName, constant).AllNotNull();

            ExpectedProto.Items.Add(
                new OptionStatement {Name = optionName, Value = Assert.IsAssignableFrom<IConstant<IdentifierPath>>(constant)}
            );
        }
    }
}
