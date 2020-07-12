using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Combinatorics.Combinatorials;
    using Ellumination.Collections.Variants;
    using Xunit;
    using Xunit.Abstractions;

    public class EnumStatementParserTests : ProtoParserTestFixtureBase<ProtoLexer, ProtoDescriptorListener>
    {
        public EnumStatementParserTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        public static IEnumerable<object[]> BasicEnumStatementCases { get; } = new BasicEnumStatementTestCases().ToArray();

        // TODO: TBD: may refactor to an "empty body" test fixture...
        /// <summary>
        /// An <see cref="EnumStatement"/> with an Empty <see cref="IList{T}"/> of type
        /// <see cref="IEnumBodyItem"/> should parse correctly. That is, literally, the
        /// Body contains no Items.
        /// </summary>
        /// <param name="enumName"></param>
        [Theory
            , MemberData(nameof(BasicEnumStatementCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(BasicEnumStatementTestCases))
            ]
        public void VerifyEmptyBody(string enumName)
        {
            ExpectedTopLevel = new EnumStatement {Name = enumName};
        }

        public static IEnumerable<object[]> BasicEnumStatementWithWhiteSpaceCases { get; } = new BasicEnumStatementWithWhiteSpaceTestCases().ToArray();

        [Theory
            , MemberData(nameof(BasicEnumStatementWithWhiteSpaceCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(BasicEnumStatementWithWhiteSpaceTestCases))
            ]
        public void VerifyEmptyBodyWithWhiteSpace(string enumName, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyEmptyBody(enumName);
        }

        public static IEnumerable<object[]> EnumWithOptionStatementCases { get; } = new EnumWithOptionStatementTestCases().ToArray();

        /// <summary>
        /// Does not really matter what we call <see cref="optionNames"/> or
        /// <see cref="optionConsts"/> since we are not here to verify that. This should already
        /// be working via the <see cref="OptionStatement"/> tests. Therefore, we should keep that
        /// cross section to an absolute minimum. For instance, we may focus only on
        /// <see cref="IVariant{Boolean}"/> and keep things brief as possible.
        /// </summary>
        /// <param name="enumName"></param>
        /// <param name="optionNames"></param>
        /// <param name="optionConsts"></param>
        [Theory
            , MemberData(nameof(EnumWithOptionStatementCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(EnumWithOptionStatementTestCases))
            ]
        public void VerifyWithOptionStatement(string enumName, OptionIdentifierPath[] optionNames
            // ReSharper disable once IdentifierTypo
            , IVariant[] optionConsts)
        {
            IEnumerable<IEnumBodyItem> GetEnumBodyItems()
            {
                var inputs = optionNames.ToArray<object>() // optionName
                    .Combine(
                        optionConsts.ToArray<object>() // optionConst
                    );

                inputs.SilentOverflow = true;

                for (var i = 0; i < inputs.Count; i++, ++inputs)
                {
                    var current = inputs.CurrentCombination.ToArray();
                    var optionName = (OptionIdentifierPath) current[0];
                    var optionConst = (IVariant) current[1];
                    yield return new OptionStatement {Name = optionName, Value = optionConst};
                }
            }

            ExpectedTopLevel = new EnumStatement {Name = enumName, Items = GetEnumBodyItems().ToList()};
        }

        public static IEnumerable<object[]> EnumWithOptionStatementWithWhiteSpaceCases { get; } = new EnumWithOptionStatementWithWhiteSpaceTestCases().ToArray();

        [Theory
            , MemberData(nameof(EnumWithOptionStatementWithWhiteSpaceCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(EnumWithOptionStatementWithWhiteSpaceTestCases))
            ]
        public void VerifyWithOptionStatementWithWhiteSpace(string enumName, OptionIdentifierPath[] optionNames
            // ReSharper disable once IdentifierTypo
            , IVariant[] optionConsts, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyWithOptionStatement(enumName, optionNames, optionConsts);
        }

        public static IEnumerable<object[]> EnumWithEnumFieldsCases { get; } = new EnumWithEnumFieldsTestCases().ToArray();

        /// <summary>
        /// An <see cref="EnumStatement"/> with At Least One <see cref="EnumFieldDescriptor"/>
        /// should parse correctly. As with other Enum Statement test cases, we are not here to
        /// further vet the Integer Literal parsing. That is fairly well thoroughly covered by
        /// virtue of <see cref="OptionStatement"/> test cases. Rather, our focus here is on
        /// <see cref="EnumStatement"/> and <see cref="EnumFieldDescriptor"/>.
        /// </summary>
        /// <param name="enumName"></param>
        /// <param name="fieldNames"></param>
        [Theory
            , MemberData(nameof(EnumWithEnumFieldsCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(EnumWithEnumFieldsTestCases))
            ]
        public void VerifyEnumFields(string enumName, string[] fieldNames)
        {
            IEnumerable<IEnumBodyItem> GetEnumFields() => fieldNames.Select<string, IEnumBodyItem>(
                x => new EnumFieldDescriptor {Name = x}
            );

            ExpectedTopLevel = new EnumStatement {Name = enumName, Items = GetEnumFields().ToList()};
        }

        public static IEnumerable<object[]> EnumWithEnumFieldsWithWhiteSpaceCases { get; } = new EnumWithEnumFieldsWithWhiteSpaceTestCases().ToArray();

        [Theory
            , MemberData(nameof(EnumWithEnumFieldsWithWhiteSpaceCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(EnumWithEnumFieldsWithWhiteSpaceTestCases))
            ]
        public void VerifyEnumFieldsWithWhiteSpace(string enumName, string[] fieldNames
            , WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyEnumFields(enumName, fieldNames);
        }

        public static IEnumerable<object[]> EnumFieldsWithOptionsCases { get; } = new EnumFieldsWithOptionsTestCases().ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumName"></param>
        /// <param name="fieldNames"></param>
        /// <param name="optionNames"></param>
        [Theory
            , MemberData(nameof(EnumFieldsWithOptionsCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(EnumFieldsWithOptionsTestCases))
            ]
        public void VerifyEnumFieldsWithOptions(string enumName, string[] fieldNames
            , OptionIdentifierPath[] optionNames)
        {
            var optionConst = Constant.Create(true);

            IEnumerable<EnumValueOption> GetEnumValueOptions() => optionNames.Select(
                x => new EnumValueOption {Name = x, Value = optionConst}
            );

            IEnumerable<IEnumBodyItem> GetEnumFields() => fieldNames.Select<string, IEnumBodyItem>(
                x => new EnumFieldDescriptor {Name = x, Options = GetEnumValueOptions().ToList()}
            );

            ExpectedTopLevel = new EnumStatement {Name = enumName, Items = GetEnumFields().ToList()};
        }

        public static IEnumerable<object[]> EnumFieldsWithOptionsWithWhiteSpaceCases { get; } = new EnumFieldsWithOptionsWithWhiteSpaceTestCases().ToArray();

        [Theory
            , MemberData(nameof(EnumFieldsWithOptionsWithWhiteSpaceCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(EnumFieldsWithOptionsWithWhiteSpaceTestCases))
            ]
        public void VerifyEnumFieldsWithOptionsWithWhiteSpace(string enumName, string[] fieldNames
            , OptionIdentifierPath[] optionNames, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyEnumFieldsWithOptions(enumName, fieldNames, optionNames);
        }
    }
}
