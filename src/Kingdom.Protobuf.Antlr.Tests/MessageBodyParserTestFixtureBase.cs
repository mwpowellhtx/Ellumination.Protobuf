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
    using Xunit;
    using Xunit.Abstractions;
    using static Collections;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ranges"></param>
        [Theory, ClassData(typeof(MessageBodyWithStarRangesTestCases))]
        public void VerifyMessageBodyWithRangesReserved(Tuple<long, long?>[] ranges)
        {
            ExpectedBody.Add(new RangesReservedStatement {Items = ranges.ToRangeDescriptors().ToList()});
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="groupName"></param>
        /// <param name="fieldNumber"></param>
        [Theory, ClassData(typeof(MessageBodyWithEmptyGroupFieldTestCases))]
        public void VerifyMessageBodyWithEmptyGroupField(LabelKind label, string groupName, long fieldNumber)
        {
            ExpectedBody.Add(new GroupFieldStatement {Label = label, Name = groupName, Number = fieldNumber});
        }
    }
}
