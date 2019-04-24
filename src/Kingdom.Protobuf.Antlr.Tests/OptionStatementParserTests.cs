// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Kingdom.Collections.Variants;
    using Xunit;
    using Xunit.Abstractions;
    using static CollectionHelpers;

    public class OptionStatementParserTests : ProtoParserTestFixtureBase<ProtoLexer, ProtoDescriptorListener>
    {
        public OptionStatementParserTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        /// <summary>
        /// Verifies the <see cref="bool"/> <see cref="IVariant"/> variant works correctly.
        /// </summary>
        /// <param name="optionName"></param>
        /// <param name="constant"></param>
        [Theory, ClassData(typeof(OptionStatementBooleanConstantTestCases))]
        public void BooleanConstantSupported(OptionIdentifierPath optionName, IVariant constant)
        {
            GetRange<object>(optionName, constant).AllNotNull();

            ExpectedProto.Items.Add(
                new OptionStatement {Name = optionName, Value = Assert.IsAssignableFrom<IVariant<bool>>(constant)}
            );
        }

        [Theory, ClassData(typeof(OptionStatementBooleanConstantWithWhiteSpaceTestCases))]
        public void BooleanConstantSupportedWithWhiteSpace(OptionIdentifierPath optionName
            , IVariant constant, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            BooleanConstantSupported(optionName, constant);
        }

        /// <summary>
        /// Verifies that <see cref="OptionStatement"/> <see cref="IdentifierPath"/>
        /// <see cref="IVariant"/> works correctly.
        /// </summary>
        /// <param name="optionName"></param>
        /// <param name="constant"></param>
        [Theory, ClassData(typeof(OptionStatementIdentifierPathConstantTestCases))]
        public void IdentifierPathConstantSupported(OptionIdentifierPath optionName, IVariant constant)
        {
            GetRange<object>(optionName, constant).AllNotNull();

            ExpectedProto.Items.Add(
                new OptionStatement {Name = optionName, Value = Assert.IsAssignableFrom<IVariant<IdentifierPath>>(constant)}
            );
        }

        [Theory, ClassData(typeof(OptionStatementIdentifierPathConstantWithWhiteSpaceTestCases))]
        public void IdentifierPathConstantSupportedWithWhiteSpace(OptionIdentifierPath optionName
            , IVariant constant, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            IdentifierPathConstantSupported(optionName, constant);
        }

        /// <summary>
        /// Verifies that <see cref="OptionStatement"/> <see cref="double"/>
        /// <see cref="IVariant"/> based cases work correctly.
        /// </summary>
        /// <param name="optionName"></param>
        /// <param name="constant"></param>
        /// <param name="options"></param>
        /// <remarks>This is a horrible performer simply due to the breadth of the test cases.
        /// Avoid runners such as JetBrains ReSharper for purposes of doing this one. Should
        /// be better performing under the XUnit Visual Test Runner through the Test Explorer,
        /// however.</remarks>
        [Theory, ClassData(typeof(OptionStatementFloatingPointLiteralConstantTestCases))]
        public void FloatingPointLiteralConstantSupported(OptionIdentifierPath optionName, IVariant constant, IStringRenderingOptions options)
        {
            /* TODO: TBD: This is STILL an issue some FOUR MONTHS later.
             * http://youtrack.jetbrains.com/issue/RSRP-471906 / "R# ignoring my xunit user furnished configuration" */

            GetRange<object>(optionName, constant, options).AllNotNull();

            RenderingOptions = options;

            ExpectedProto.Items.Add(
                new OptionStatement { Name = optionName, Value = Assert.IsAssignableFrom<IVariant<double>>(constant) }
            );
        }

        [Theory, ClassData(typeof(OptionStatementFloatingPointLiteralConstantWithWhiteSpaceTestCases))]
        public void FloatingPointLiteralConstantSupportedWithWhiteSpace(OptionIdentifierPath optionName
            , IVariant constant, WhiteSpaceAndCommentOption whiteSpaceOption)
            => FloatingPointLiteralConstantSupported(optionName, constant
                , new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption});

        /// <summary>
        /// Verifies that <see cref="OptionStatement"/> supports the <see cref="long"/> based
        /// <see cref="IVariant"/>.
        /// </summary>
        /// <param name="optionName"></param>
        /// <param name="constant"></param>
        /// <param name="options"></param>
        [Theory, ClassData(typeof(OptionStatementIntegerLiteralConstantTestCases))]
        public void IntegerLiteralConstantSupported(OptionIdentifierPath optionName, IVariant constant
            , IStringRenderingOptions options)
        {
            /* TODO: TBD: This is STILL an issue some FOUR MONTHS later.
             * http://youtrack.jetbrains.com/issue/RSRP-471906 / "R# ignoring my xunit user furnished configuration" */

            GetRange<object>(optionName, constant, options).AllNotNull();

            RenderingOptions = options;

            ExpectedProto.Items.Add(
                new OptionStatement {Name = optionName, Value = Assert.IsAssignableFrom<IVariant<long>>(constant)}
            );
        }

        [Theory, ClassData(typeof(OptionStatementIntegerLiteralConstantWithWhiteSpaceTestCases))]
        public void IntegerLiteralConstantSupportedWithWhiteSpace(OptionIdentifierPath optionName
            , IVariant constant, WhiteSpaceAndCommentOption whiteSpaceOption)
            => IntegerLiteralConstantSupported(optionName, constant
                , new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption});

        /// <summary>
        /// Verifies that <see cref="OptionStatement"/> <see cref="string"/>
        /// <see cref="IVariant"/> works correctly.
        /// </summary>
        /// <param name="optionName"></param>
        /// <param name="constant"></param>
        [Theory, ClassData(typeof(OptionStatementStringLiteralConstantTestCases))]
        public void StringLiteralConstantSupported(OptionIdentifierPath optionName, IVariant constant)
        {
            GetRange<object>(optionName, constant).AllNotNull();

            ExpectedProto.Items.Add(
                new OptionStatement {Name = optionName, Value = Assert.IsAssignableFrom<IVariant<string>>(constant)}
            );
        }

        [Theory, ClassData(typeof(OptionStatementStringLiteralConstantWithWhiteSpaceTestCases))]
        public void StringLiteralConstantSupportedWithWhiteSpace(OptionIdentifierPath optionName
            , IVariant constant, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            StringLiteralConstantSupported(optionName, constant);
        }
    }
}
