using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
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
    using Kingdom.Collections.Variants;
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

        /// <summary>
        /// We only need to verify the simplest of <see cref="NormalFieldStatement"/> test cases
        /// here. We especially do not need to exhaust ones involving Options and so forth as
        /// these are verified elsewhere.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="fieldType"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldNumber"></param>
        [Theory, ClassData(typeof(MessageBodyWithNormalFieldTestCases))]
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

        [Theory, ClassData(typeof(MessageBodyWithNormalFieldWithWhiteSpaceTestCases))]
        public virtual void VerifyMessageBodyWithNormalFieldWithWhiteSpace(LabelKind label, IVariant fieldType
            , string fieldName, long fieldNumber, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyMessageBodyWithNormalField(label, fieldType, fieldName, fieldNumber);
        }

        /// <summary>
        /// We only need to verify the simplest of <see cref="NormalFieldStatement"/> test cases
        /// here. We especially do not need to exhaust ones involving Options and so forth as
        /// these are verified elsewhere.
        /// </summary>
        /// <param name="enumName"></param>
        [Theory, ClassData(typeof(MessageBodyWithEmptyEnumTestCases))]
        public void VerifyMessageBodyWithEmptyEnum(string enumName)
        {
            ExpectedBody.Add(new EnumStatement {Name = enumName});
        }

        /// <summary>
        /// We only need to verify the simplest of <see cref="MessageStatement"/> test cases
        /// here. We especially do not need to exhaust ones involving Options and so forth as
        /// these are verified elsewhere.
        /// </summary>
        /// <param name="innerMessageName"></param>
        [Theory, ClassData(typeof(MessageBodyWithEmptyInnerMessageTestCases))]
        public void VerifyMessageBodyWithEmptyMessage(string innerMessageName)
        {
            ExpectedBody.Add(new MessageStatement {Name = innerMessageName});
        }

        [Theory, ClassData(typeof(MessageBodyWithEmptyInnerMessageWithWhiteSpaceTestCases))]
        public void VerifyMessageBodyWithEmptyMessageWithWhiteSpace(string innerMessageName
            , WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyMessageBodyWithEmptyMessage(innerMessageName);
        }

        /// <summary>
        /// We only need to verify the simplest of <see cref="MessageStatement"/> test cases
        /// here. We especially do not need to exhaust ones involving Options and so forth as
        /// these are verified elsewhere.
        /// </summary>
        /// <param name="messageTypes"></param>
        [Theory, ClassData(typeof(MessageBodyWithExtendTestCases))]
        public void VerifyMessageBodyWithEmptyExtend(ElementTypeIdentifierPath[] messageTypes)
        {
            Assert.NotNull(messageTypes);

            ExpectedBody.AddRange(
                messageTypes.Select<ElementTypeIdentifierPath, IMessageBodyItem>(
                    x => new ExtendStatement {MessageType = x}).ToArray()
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ranges"></param>
        [Theory, ClassData(typeof(MessageBodyWithStarRangesTestCases))]
        public void VerifyMessageBodyWithExtensions(Tuple<long, long?>[] ranges)
        {
            ExpectedBody.Add(new ExtensionsStatement {Items = ranges.ToRangeDescriptors().ToList()});
        }

        [Theory, ClassData(typeof(MessageBodyWithStarRangesWithWhiteSpaceTestCases))]
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
        [Theory, ClassData(typeof(MessageBodyWithStarRangesTestCases))]
        public void VerifyMessageBodyWithRangesReserved(Tuple<long, long?>[] ranges)
        {
            ExpectedBody.Add(new RangesReservedStatement {Items = ranges.ToRangeDescriptors().ToList()});
        }

        [Theory, ClassData(typeof(MessageBodyWithStarRangesWithWhiteSpaceTestCases))]
        public void VerifyMessageBodyWithRangesReservedWithWhiteSpace(Tuple<long, long?>[] ranges
            , WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyMessageBodyWithRangesReserved(ranges);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldNames"></param>
        [Theory, ClassData(typeof(MessageBodyWithFieldNamesReservedTestCases))]
        public void VerifyMessageBodyWithFieldNamesReserved(Identifier[] fieldNames)
        {
            Assert.NotNull(fieldNames);
            Assert.NotEmpty(fieldNames);

            ExpectedBody.Add(new FieldNamesReservedStatement {Items = fieldNames.ToList()});
        }

        [Theory, ClassData(typeof(MessageBodyWithFieldNamesReservedWithWhiteSpaceTestCases))]
        public void VerifyMessageBodyWithFieldNamesReservedWithWhiteSpace(Identifier[] fieldNames
            , WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyMessageBodyWithFieldNamesReserved(fieldNames);
        }

        /// <summary>
        /// Verify that a <see cref="OneOfStatement"/> having <see cref="EmptyStatement"/> in the
        /// <see cref="OneOfStatement.Items"/>.
        /// </summary>
        /// <param name="oneOfName"></param>
        [Theory, ClassData(typeof(MessageBodyWithEmptyOneOfTestCases))]
        public void VerifyMessageBodyWithEmptyOneOf(string oneOfName)
        {
            ExpectedBody.Add(new OneOfStatement
            {
                Name = oneOfName,
                Items = GetRange<IOneOfBodyItem>(NewEmptyStatement).ToList()
            });
        }

        [Theory, ClassData(typeof(MessageBodyWithEmptyOneOfWithWhiteSpaceTestCases))]
        public void VerifyMessageBodyWithEmptyOneOfWithWhiteSpace(string oneOfName
            , WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyMessageBodyWithEmptyOneOf(oneOfName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oneOfName"></param>
        /// <param name="fieldTuples"></param>
        /// <param name="optionNames"></param>
        /// <param name="optionValues"></param>
        [Theory, ClassData(typeof(MessageBodyWithOneOfTestCases))]
        public void VerifyMessageBodyWithOneOf(string oneOfName, FieldTupleType[] fieldTuples
            , OptionIdentifierPath[] optionNames, IConstant[] optionValues)
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

        [Theory, ClassData(typeof(MessageBodyWithOneOfWithWhiteSpaceTestCases))]
        public void VerifyMessageBodyWithOneOfWithWhiteSpace(string oneOfName, FieldTupleType[] fieldTuples
            , OptionIdentifierPath[] optionNames, IConstant[] optionValues, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyMessageBodyWithOneOf(oneOfName, fieldTuples, optionNames, optionValues);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyType"></param>
        /// <param name="valueType"></param>
        /// <param name="mapName"></param>
        /// <param name="fieldNumber"></param>
        /// <param name="optionNames"></param>
        /// <param name="optionValues"></param>
        [Theory, ClassData(typeof(MessageBodyWithMapFieldTestCases))]
        public void VerifyMessageBodyWithMapField(KeyType keyType, IVariant valueType, string mapName, long fieldNumber
            , OptionIdentifierPath[] optionNames, IConstant[] optionValues)
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

        [Theory, ClassData(typeof(MessageBodyWithMapFieldWithWhiteSpaceTestCases))]
        public void VerifyMessageBodyWithMapFieldWithWhiteSpace(KeyType keyType, IVariant valueType
            , string mapName, long fieldNumber, OptionIdentifierPath[] optionNames, IConstant[] optionValues
            , WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyMessageBodyWithMapField(keyType, valueType, mapName, fieldNumber, optionNames, optionValues);
        }

        /// <summary>
        /// As with other Test Cases, this is not supposed to be an exhaustive verification of
        /// either <see cref="IConstant"/> or <see cref="OptionStatement"/>, but rather to verify
        /// that it can land in the context of <see cref="MessageStatement.Items"/> correctly.
        /// </summary>
        /// <param name="optionNames"></param>
        /// <param name="optionValues"></param>
        [Theory, ClassData(typeof(MessageBodyWithOptionTestCases))]
        public void VerifyMessageBodyWithOption(OptionIdentifierPath[] optionNames, IConstant[] optionValues)
        {
            ExpectedBody.AddRange(
                ElaborateOptions<OptionStatement>(optionNames, optionValues).ToArray<IMessageBodyItem>()
            );
        }

        [Theory, ClassData(typeof(MessageBodyWithOptionWithWhiteSpaceTestCases))]
        public void VerifyMessageBodyWithOptionWithWhiteSpace(OptionIdentifierPath[] optionNames
            , IConstant[] optionValues, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyMessageBodyWithOption(optionNames, optionValues);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="groupName"></param>
        /// <param name="fieldNumber"></param>
        [Theory, ClassData(typeof(MessageBodyWithEmptyGroupFieldTestCases))]
        public void VerifyMessageBodyWithEmptyGroupField(LabelKind label, string groupName, long fieldNumber)
        {
            ExpectedBody.Add(
                new GroupFieldStatement {Label = label, Name = groupName, Number = fieldNumber}
            );
        }

        [Theory, ClassData(typeof(MessageBodyWithEmptyGroupFieldWithWhiteSpaceTestCases))]
        public void VerifyMessageBodyWithEmptyGroupFieldWithWhiteSpace(LabelKind label, string groupName
            , long fieldNumber, WhiteSpaceAndCommentOption whiteSpaceOption)
        {
            RenderingOptions = new StringRenderingOptions {WhiteSpaceAndCommentRendering = whiteSpaceOption};
            VerifyMessageBodyWithEmptyGroupField(label, groupName, fieldNumber);
        }
    }
}
