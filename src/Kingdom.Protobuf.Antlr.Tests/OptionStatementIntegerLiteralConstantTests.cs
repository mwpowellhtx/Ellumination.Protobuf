// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Xunit;
    using Xunit.Abstractions;
    using static Collections;

    public class OptionStatementIntegerLiteralConstantTests
        : ProtoParserTestFixtureBase<ProtoLexer, ProtoDescriptorListener>
    {
        public OptionStatementIntegerLiteralConstantTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        /// <summary>
        /// Verifies that <see cref="OptionStatement"/> supports the <see cref="long"/> based
        /// <see cref="IConstant"/>.
        /// </summary>
        /// <param name="optionName"></param>
        /// <param name="constant"></param>
        /// <param name="options"></param>
        [Theory, ClassData(typeof(OptionStatementIntegerLiteralConstantTestCases))]
        public void VerifyStatement(OptionIdentifierPath optionName, IConstant constant, IStringRenderingOptions options)
        {
            /* TODO: TBD: This is STILL an issue some FOUR MONTHS later.
             * http://youtrack.jetbrains.com/issue/RSRP-471906 / "R# ignoring my xunit user furnished configuration" */

            GetRange<object>(optionName, constant, options).AllNotNull();

            RenderingOptions = options;

            ExpectedProto.Items.Add(
                new OptionStatement {Name = optionName, Value = Assert.IsAssignableFrom<IConstant<long>>(constant)}
            );
        }
    }
}
