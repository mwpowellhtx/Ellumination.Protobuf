// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Xunit;
    using Xunit.Abstractions;
    using static Collections;

    public class OptionStatementFloatingPointLiteralConstantTests
        : ProtoParserTestFixtureBase<ProtoLexer, ProtoDescriptorListener>
    {
        public OptionStatementFloatingPointLiteralConstantTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        /// <summary>
        /// Verifies that <see cref="OptionStatement"/> <see cref="double"/>
        /// <see cref="IConstant"/> based cases work correctly.
        /// </summary>
        /// <param name="optionName"></param>
        /// <param name="constant"></param>
        /// <param name="options"></param>
        /// <remarks>This is a horrible performer simply due to the breadth of the test cases.
        /// Avoid runners such as JetBrains ReSharper for purposes of doing this one. Should
        /// be better performing under the XUnit Visual Test Runner through the Test Explorer,
        /// however.</remarks>
        [Theory, ClassData(typeof(OptionStatementFloatingPointLiteralConstantTestCases))]
        public void VerifyStatement(OptionIdentifierPath optionName, IConstant constant, IStringRenderingOptions options)
        {
            /* TODO: TBD: This is STILL an issue some FOUR MONTHS later.
             * http://youtrack.jetbrains.com/issue/RSRP-471906 / "R# ignoring my xunit user furnished configuration" */

            GetRange<object>(optionName, constant, options).AllNotNull();

            RenderingOptions = options;

            ExpectedProto.Items.Add(
                new OptionStatement {Name = optionName, Value = Assert.IsAssignableFrom<IConstant<double>>(constant)}
            );
        }
    }
}
