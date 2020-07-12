using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /*
     * Building out support for the MessageBody portion of the Message-Group formation:
     * http://developers.google.com/protocol-buffers/docs/reference/proto2-spec#message_definition
     * http://developers.google.com/protocol-buffers/docs/reference/proto2-spec#group_field
     * // group = label "group" groupName "=" fieldNumber messageBody
     * // message = "message" messageName messageBody
     * // messageBody = "{" { field | enum | message | extend | extensions | group | option | oneof | mapField | reserved | emptyStatement } "}"
     *                                                                                                                      ?
     *                                                                                                           ?
     *                                                                                                ?
     *                                                                                        ?
     *                                                                               ?
     *                                                                       ?
     *                                                          ?
     *                                                 ?
     *                                       ?
     *                                ?
     *                        ?
     *
     */
    using Ellumination.Collections.Variants;
    using Xunit;
    using Xunit.Abstractions;
    using static CollectionHelpers;
    using static Domain;
    using static Identification;
    using static Statements;
    using FieldTupleType = Tuple<ProtoType, string, long>;

    // ReSharper disable once UnusedTypeParameter
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="THavingBody"></typeparam>
    /// <inheritdoc />
    public abstract class MessageBodyParserTestFixtureBase<THavingBody>
        : ProtoParserTestFixtureBase<ProtoLexer, ProtoDescriptorListener>
        where THavingBody : class, IHasBody<IMessageBodyItem>, new()
    {
        protected MessageBodyParserTestFixtureBase(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        protected MessageStatement ExpectedMessage
        {
            get
            {
                if (!(ExpectedTopLevel is MessageStatement))
                {
                    ExpectedTopLevel = new MessageStatement {Name = GetIdent(3)};
                }

                return Assert.IsType<MessageStatement>(ExpectedTopLevel);
            }
        }

        protected abstract IList<IMessageBodyItem> ExpectedBody { get; set; }

        public static IEnumerable<object[]> MessageBodyWithNormalFieldCases { get; }
            = new MessageBodyWithNormalFieldTestCases().ToArray();

        /// <summary>
        /// We only need to verify the simplest of <see cref="NormalFieldStatement"/> test cases
        /// here. We especially do not need to exhaust ones involving Options and so forth as
        /// these are verified elsewhere.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="fieldType"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldNumber"></param>
        [Theory
            , MemberData(nameof(MessageBodyWithNormalFieldCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(MessageBodyWithNormalFieldTestCases))
            ]
        public virtual void VerifyMessageBodyWithNormalField(LabelKind label, IVariant fieldType, string fieldName
            , long fieldNumber)
        {
            ExpectedBody.Add(new NormalFieldStatement
            {
                Label = label,
                FieldType = fieldType,
                Name = fieldName,
                Number = fieldNumber
            });
        }

        public static IEnumerable<object[]> MessageBodyWithNormalFieldWithWhiteSpaceCases { get; }
            = new MessageBodyWithNormalFieldWithWhiteSpaceTestCases().ToArray();

        [Theory
            , MemberData(nameof(MessageBodyWithNormalFieldWithWhiteSpaceCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(MessageBodyWithNormalFieldWithWhiteSpaceTestCases))
            ]
        public virtual void VerifyMessageBodyWithNormalFieldWithWhiteSpace(LabelKind label, IVariant fieldType
            , string fieldName, long fieldNumber, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyMessageBodyWithNormalField(label, fieldType, fieldName, fieldNumber);
        }

        public static IEnumerable<object[]> MessageBodyWithEmptyEnumCases { get; }
            = new MessageBodyWithEmptyEnumTestCases().ToArray();

        /// <summary>
        /// We only need to verify the simplest of <see cref="NormalFieldStatement"/> test cases
        /// here. We especially do not need to exhaust ones involving Options and so forth as
        /// these are verified elsewhere.
        /// </summary>
        /// <param name="enumName"></param>
        [Theory
            , MemberData(nameof(MessageBodyWithEmptyEnumCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(MessageBodyWithEmptyEnumTestCases))
            ]
        public void VerifyMessageBodyWithEmptyEnum(string enumName)
        {
            ExpectedBody.Add(new EnumStatement {Name = enumName});
        }

        public static IEnumerable<object[]> MessageBodyWithEmptyInnerMessageCases { get; }
            = new MessageBodyWithEmptyInnerMessageTestCases().ToArray();

        /// <summary>
        /// We only need to verify the simplest of <see cref="MessageStatement"/> test cases
        /// here. We especially do not need to exhaust ones involving Options and so forth as
        /// these are verified elsewhere.
        /// </summary>
        /// <param name="innerMessageName"></param>
        [Theory
            , MemberData(nameof(MessageBodyWithEmptyInnerMessageCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(MessageBodyWithEmptyInnerMessageTestCases))
            ]
        public void VerifyMessageBodyWithEmptyMessage(string innerMessageName)
        {
            ExpectedBody.Add(new MessageStatement {Name = innerMessageName});
        }

        public static IEnumerable<object[]> MessageBodyWithEmptyInnerMessageWithWhiteSpaceCases { get; }
            = new MessageBodyWithEmptyInnerMessageWithWhiteSpaceTestCases().ToArray();

        [Theory
            , MemberData(nameof(MessageBodyWithEmptyInnerMessageWithWhiteSpaceCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(MessageBodyWithEmptyInnerMessageWithWhiteSpaceTestCases))
            ]
        public void VerifyMessageBodyWithEmptyMessageWithWhiteSpace(string innerMessageName
            , WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyMessageBodyWithEmptyMessage(innerMessageName);
        }

        public static IEnumerable<object[]> MessageBodyWithExtendCases { get; }
            = new MessageBodyWithExtendTestCases().ToArray();

        /// <summary>
        /// We only need to verify the simplest of <see cref="MessageStatement"/> test cases
        /// here. We especially do not need to exhaust ones involving Options and so forth as
        /// these are verified elsewhere.
        /// </summary>
        /// <param name="messageTypes"></param>
        [Theory
            , MemberData(nameof(MessageBodyWithExtendCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(MessageBodyWithExtendTestCases))
            ]
        public void VerifyMessageBodyWithEmptyExtend(ElementTypeIdentifierPath[] messageTypes)
        {
            Assert.NotNull(messageTypes);

            ExpectedBody.AddRange(
                messageTypes.Select<ElementTypeIdentifierPath, IMessageBodyItem>(
                    x => new ExtendStatement {MessageType = x}).ToArray()
            );
        }

        public static IEnumerable<object[]> MessageBodyWithStarRangesCases { get; }
            = new MessageBodyWithStarRangesTestCases().ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ranges"></param>
        [Theory
            , MemberData(nameof(MessageBodyWithStarRangesCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(MessageBodyWithStarRangesTestCases))
            ]
        public void VerifyMessageBodyWithExtensions(Tuple<long, long?>[] ranges)
        {
            ExpectedBody.Add(new ExtensionsStatement {Items = ranges.ToRangeDescriptors().ToList()});
        }

        public static IEnumerable<object[]> MessageBodyWithStarRangesWithWhiteSpaceCases { get; }
            = new MessageBodyWithStarRangesWithWhiteSpaceTestCases().ToArray();

        [Theory
            , MemberData(nameof(MessageBodyWithStarRangesWithWhiteSpaceCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(MessageBodyWithStarRangesWithWhiteSpaceTestCases))
            ]
        public void VerifyMessageBodyWithExtensionsWithWhiteSpace(Tuple<long, long?>[] ranges
            , WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyMessageBodyWithExtensions(ranges);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ranges"></param>
        [Theory
            , MemberData(nameof(MessageBodyWithStarRangesCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(MessageBodyWithStarRangesTestCases))
            ]
        public void VerifyMessageBodyWithRangesReserved(Tuple<long, long?>[] ranges)
        {
            ExpectedBody.Add(new RangesReservedStatement {Items = ranges.ToRangeDescriptors().ToList()});
        }

        [Theory
            , MemberData(nameof(MessageBodyWithStarRangesWithWhiteSpaceCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(MessageBodyWithStarRangesWithWhiteSpaceTestCases))
            ]
        public void VerifyMessageBodyWithRangesReservedWithWhiteSpace(Tuple<long, long?>[] ranges
            , WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyMessageBodyWithRangesReserved(ranges);
        }

        public static IEnumerable<object[]> MessageBodyWithFieldNamesReservedCases { get; }
            = new MessageBodyWithFieldNamesReservedTestCases().ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldNames"></param>
        [Theory
            , MemberData(nameof(MessageBodyWithFieldNamesReservedCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(MessageBodyWithFieldNamesReservedTestCases))
            ]
        public void VerifyMessageBodyWithFieldNamesReserved(Identifier[] fieldNames)
        {
            Assert.NotNull(fieldNames);
            Assert.NotEmpty(fieldNames);

            ExpectedBody.Add(new FieldNamesReservedStatement {Items = fieldNames.ToList()});
        }

        public static IEnumerable<object[]> MessageBodyWithFieldNamesReservedWithWhiteSpaceCases { get; }
            = new MessageBodyWithFieldNamesReservedWithWhiteSpaceTestCases().ToArray();

        [Theory
            , MemberData(nameof(MessageBodyWithFieldNamesReservedWithWhiteSpaceCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(MessageBodyWithFieldNamesReservedWithWhiteSpaceTestCases))
            ]
        public void VerifyMessageBodyWithFieldNamesReservedWithWhiteSpace(Identifier[] fieldNames
            , WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyMessageBodyWithFieldNamesReserved(fieldNames);
        }

        public static IEnumerable<object[]> MessageBodyWithEmptyOneOfCases { get; }
            = new MessageBodyWithEmptyOneOfTestCases().ToArray();

        /// <summary>
        /// Verify that a <see cref="OneOfStatement"/> having <see cref="EmptyStatement"/> in the
        /// <see cref="OneOfStatement.Items"/>.
        /// </summary>
        /// <param name="oneOfName"></param>
        [Theory
            , MemberData(nameof(MessageBodyWithEmptyOneOfCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(MessageBodyWithEmptyOneOfTestCases))
            ]
        public void VerifyMessageBodyWithEmptyOneOf(string oneOfName)
        {
            ExpectedBody.Add(new OneOfStatement
            {
                Name = oneOfName,
                Items = GetRange<IOneOfBodyItem>(NewEmptyStatement).ToList()
            });
        }

        public static IEnumerable<object[]> MessageBodyWithEmptyOneOfWithWhiteSpaceCases { get; }
            = new MessageBodyWithEmptyOneOfWithWhiteSpaceTestCases().ToArray();

        [Theory
            , MemberData(nameof(MessageBodyWithEmptyOneOfWithWhiteSpaceCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(MessageBodyWithEmptyOneOfWithWhiteSpaceTestCases))
            ]
        public void VerifyMessageBodyWithEmptyOneOfWithWhiteSpace(string oneOfName
            , WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyMessageBodyWithEmptyOneOf(oneOfName);
        }

        public static IEnumerable<object[]> MessageBodyWithOneOfCases { get; }
            = new MessageBodyWithOneOfTestCases().ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oneOfName"></param>
        /// <param name="fieldTuples"></param>
        /// <param name="optionNames"></param>
        /// <param name="optionValues"></param>
        [Theory
            , MemberData(nameof(MessageBodyWithOneOfCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(MessageBodyWithOneOfTestCases))
            ]
        public void VerifyMessageBodyWithOneOf(string oneOfName, FieldTupleType[] fieldTuples
            , OptionIdentifierPath[] optionNames, IVariant[] optionValues)
        {
            Assert.NotNull(fieldTuples);
            Assert.NotEmpty(fieldTuples);

            IEnumerable<IOneOfBodyItem> GetOneOfBodyItems()
            {
                foreach (var (protoType, fieldName, fieldNumber) in fieldTuples)
                {
                    yield return new OneOfFieldStatement
                    {
                        // TODO: TBD: may further generalize this in the test cases themselves...
                        FieldType = Variant.Create(protoType),
                        Name = fieldName,
                        Number = fieldNumber,
                        Options = ElaborateOptions<FieldOption>(optionNames, optionValues).ToList()
                    };
                }
            }

            ExpectedBody.Add(new OneOfStatement {Name = oneOfName, Items = GetOneOfBodyItems().ToList()});
        }

        public static IEnumerable<object[]> MessageBodyWithOneOfWithWhiteSpaceCases { get; }
            = new MessageBodyWithOneOfWithWhiteSpaceTestCases().ToArray();

        [Theory
            , MemberData(nameof(MessageBodyWithOneOfWithWhiteSpaceCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(MessageBodyWithOneOfWithWhiteSpaceTestCases))
            ]
        public void VerifyMessageBodyWithOneOfWithWhiteSpace(string oneOfName, FieldTupleType[] fieldTuples
            , OptionIdentifierPath[] optionNames, IVariant[] optionValues, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyMessageBodyWithOneOf(oneOfName, fieldTuples, optionNames, optionValues);
        }

        public static IEnumerable<object[]> MessageBodyWithMapFieldCases { get; }
            = new MessageBodyWithMapFieldTestCases().ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyType"></param>
        /// <param name="valueType"></param>
        /// <param name="mapName"></param>
        /// <param name="fieldNumber"></param>
        /// <param name="optionNames"></param>
        /// <param name="optionValues"></param>
        [Theory
            , MemberData(nameof(MessageBodyWithMapFieldCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(MessageBodyWithMapFieldTestCases))
            ]
        public void VerifyMessageBodyWithMapField(KeyType keyType, IVariant valueType, string mapName, long fieldNumber
            , OptionIdentifierPath[] optionNames, IVariant[] optionValues)
        {
            ExpectedBody.Add(new MapFieldStatement
            {
                KeyType = keyType,
                ValueType = valueType,
                Name = mapName,
                Number = fieldNumber,
                Options = ElaborateOptions<FieldOption>(optionNames, optionValues).ToList()
            });
        }

        public static IEnumerable<object[]> MessageBodyWithMapFieldWithWhiteSpaceCases { get; }
            = new MessageBodyWithMapFieldWithWhiteSpaceTestCases().ToArray();

        [Theory
            , MemberData(nameof(MessageBodyWithMapFieldWithWhiteSpaceCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(MessageBodyWithMapFieldWithWhiteSpaceTestCases))
            ]
        public void VerifyMessageBodyWithMapFieldWithWhiteSpace(KeyType keyType, IVariant valueType
            , string mapName, long fieldNumber, OptionIdentifierPath[] optionNames, IVariant[] optionValues
            , WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyMessageBodyWithMapField(keyType, valueType, mapName, fieldNumber, optionNames, optionValues);
        }

        public static IEnumerable<object[]> MessageBodyWithOptionCases { get; }
            = new MessageBodyWithOptionTestCases().ToArray();

        /// <summary>
        /// As with other Test Cases, this is not supposed to be an exhaustive verification of
        /// either <see cref="IVariant"/> or <see cref="OptionStatement"/>, but rather to verify
        /// that it can land in the context of <see cref="MessageStatement.Items"/> correctly.
        /// </summary>
        /// <param name="optionNames"></param>
        /// <param name="optionValues"></param>
        [Theory
            , MemberData(nameof(MessageBodyWithOptionCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(MessageBodyWithOptionTestCases))
            ]
        public void VerifyMessageBodyWithOption(OptionIdentifierPath[] optionNames, IVariant[] optionValues)
        {
            ExpectedBody.AddRange(
                ElaborateOptions<OptionStatement>(optionNames, optionValues).ToArray<IMessageBodyItem>()
            );
        }

        public static IEnumerable<object[]> MessageBodyWithOptionWithWhiteSpaceCases { get; }
            = new MessageBodyWithOptionWithWhiteSpaceTestCases().ToArray();

        [Theory
            , MemberData(nameof(MessageBodyWithOptionWithWhiteSpaceCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(MessageBodyWithOptionWithWhiteSpaceTestCases))
            ]
        public void VerifyMessageBodyWithOptionWithWhiteSpace(OptionIdentifierPath[] optionNames
            , IVariant[] optionValues, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyMessageBodyWithOption(optionNames, optionValues);
        }

        public static IEnumerable<object[]> MessageBodyWithEmptyGroupFieldCases { get; }
            = new MessageBodyWithEmptyGroupFieldTestCases().ToArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="groupName"></param>
        /// <param name="fieldNumber"></param>
        [Theory
            , MemberData(nameof(MessageBodyWithEmptyGroupFieldCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(MessageBodyWithEmptyGroupFieldTestCases))
            ]
        public void VerifyMessageBodyWithEmptyGroupField(LabelKind label, string groupName, long fieldNumber)
        {
            ExpectedBody.Add(
                new GroupFieldStatement {Label = label, Name = groupName, Number = fieldNumber}
            );
        }

        public static IEnumerable<object[]> MessageBodyWithEmptyGroupFieldWithWhiteSpaceCases { get; }
            = new MessageBodyWithEmptyGroupFieldWithWhiteSpaceTestCases().ToArray();

        [Theory
            , MemberData(nameof(MessageBodyWithEmptyGroupFieldWithWhiteSpaceCases), DisableDiscoveryEnumeration = true)
            //, ClassData(typeof(MessageBodyWithEmptyGroupFieldWithWhiteSpaceTestCases))
            ]
        public void VerifyMessageBodyWithEmptyGroupFieldWithWhiteSpace(LabelKind label, string groupName
            , long fieldNumber, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyMessageBodyWithEmptyGroupField(label, groupName, fieldNumber);
        }
    }
}
