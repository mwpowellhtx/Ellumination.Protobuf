using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Antlr4.Runtime;
    using Xunit;
    using Xunit.Abstractions;
    using static CollectionHelpers;
    using static Double;
    using static Statements;
    using EvaluateCallbackDelegate = AntlrEvaluateParserContextDelegate<ProtoParser, ProtoParser.ProtoDeclContext>;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TListener"></typeparam>
    /// <inheritdoc cref="AntlrParserTestFixtureBase{TSource,TStream,TParser,TContext,TListener,TTarget}" />
    public abstract class ProtoParserTestFixtureBase<TSource, TListener> : AntlrParserTestFixtureBase<
        TSource, CommonTokenStream, ProtoParser, ProtoParser.ProtoDeclContext, TListener, ProtoDescriptor>
        , IDisposable
        where TSource : class, ITokenSource
        where TListener : ProtoDescriptorListenerBase
    {
        protected ProtoParserTestFixtureBase(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        /// <inheritdoc />
        /// <see cref="ProtoParser.proto"/>
        protected override EvaluateCallbackDelegate EvaluateCallback { get; } = parser => parser.protoDecl();

        protected IStringRenderingOptions RenderingOptions { get; set; } = new StringRenderingOptions();

        protected override ProtoDescriptor ExpectedTarget
        {
            get => _expectedProto;
            set => _expectedProto = value;
        }

        private ProtoDescriptor _expectedProto;

        protected virtual ProtoDescriptor ExpectedProto
        {
            get
            {
                ProtoDescriptor CreateDescriptor()
                {
                    var syntax = NewSyntaxStatement;
                    var descriptor = new ProtoDescriptor {Syntax = syntax};
                    Assert.NotNull(descriptor.Syntax);
                    Assert.Same(syntax, descriptor.Syntax);
                    Assert.NotNull(descriptor);
                    return descriptor;
                }

                return _expectedProto ?? (_expectedProto = CreateDescriptor());
            }
        }

        protected virtual ITopLevelDefinition ExpectedTopLevel
        {
            get => ExpectedProto.Items.OfType<ITopLevelDefinition>().FirstOrDefault();
            set
            {
                Assert.NotNull(value);

                if (ExpectedProto.Items.Any())
                {
                    ExpectedProto.Items[0] = value;
                    return;
                }

                ExpectedProto.Items.Add(value);
            }
        }

        protected bool IsDisposed { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || IsDisposed)
            {
                return;
            }

            /* Changing the Protocol a little bit.
             * Verify Parse becomes an ExpectedProto Consumer and clears the instance. */

            if (ExpectedTarget == null)
            {
                return;
            }

            var o = RenderingOptions;

            VerifyParse(x => o != null ? x.ToDescriptorString(o) : x.ToDescriptorString());
        }

        /// <summary>
        /// These tests are all about Arranging the Expected Proto. That is all the Verification
        /// Methods need to do is the Arranging. We will do Verification upon Disposal.
        /// </summary>
        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            IsDisposed = true;
        }

        private static InvalidOperationException ReportUnexpectedAlternative<T>(T value)
        {
            string GetItemTypeOrNull() => value == null ? "null" : $"{typeof(T).FullName}";
            return new InvalidOperationException($"Unexpected kind of Item '{GetItemTypeOrNull()}'");
        }

        protected virtual void VerifyIdentifier(Identifier expectedIdentifier, Identifier actualIdentifier)
        {
            GetRange(expectedIdentifier, actualIdentifier).AllNotNull();
            Assert.NotSame(expectedIdentifier, actualIdentifier);
            Assert.Equal(expectedIdentifier.Name, actualIdentifier.Name);
            Assert.Equal(expectedIdentifier, actualIdentifier);
        }

        protected virtual void VerifyIdentifierPath(IdentifierPath expectedPath, IdentifierPath actualPath)
        {
            GetRange(expectedPath, actualPath).AllNotNull();
            Assert.NotSame(expectedPath, actualPath);
            Assert.Equal(expectedPath.Count, actualPath.Count);
            foreach (var (expectedIdentifier, actualIdentifier) in expectedPath.Zip(actualPath, Tuple.Create))
            {
                VerifyIdentifier(expectedIdentifier, actualIdentifier);
            }
        }

        protected virtual void VerifyOptionIdentifierPath(OptionIdentifierPath expectedPath
            , OptionIdentifierPath actualPath)
        {
            VerifyIdentifierPath(expectedPath, actualPath);
            // TODO: TBD: requires a little work extending out from the Parser Grammar...
            Assert.Equal(expectedPath.SuffixStartIndex, actualPath.SuffixStartIndex);
            Assert.Equal(expectedPath.IsPrefixGrouped, actualPath.IsPrefixGrouped);
        }

        protected virtual void VerifyElementTypeIdentifierPath(ElementTypeIdentifierPath expectedPath
            , ElementTypeIdentifierPath actualPath)
        {
            VerifyIdentifierPath(expectedPath, actualPath);
            Assert.Equal(expectedPath.IsGlobalScope, actualPath.IsGlobalScope);
        }

        protected virtual void VerifyConstant(IConstant<double> expectedConstant, IConstant<double> actualConstant)
        {
            Assert.NotNull(expectedConstant);
            Assert.NotSame(expectedConstant, actualConstant);

            if (IsNaN(expectedConstant.Value) || IsInfinity(expectedConstant.Value))
            {
                if (IsNaN(expectedConstant.Value))
                {
                    Assert.True(IsNaN(actualConstant.Value));
                }
                else if (IsPositiveInfinity(expectedConstant.Value))
                {
                    Assert.True(IsPositiveInfinity(actualConstant.Value));
                }
                else if (IsNegativeInfinity(expectedConstant.Value))
                {
                    Assert.True(IsNegativeInfinity(actualConstant.Value));
                }

                // Infinity and NaN should never be Equal.
                Assert.False(actualConstant.Equals(expectedConstant));
                return;
            }

            OutputHelper.WriteLine($"{nameof(expectedConstant)}.{nameof(IConstant.Value)} = {expectedConstant.Value:f99}");
            OutputHelper.WriteLine($"{nameof(actualConstant)}.{nameof(IConstant.Value)} = {actualConstant.Value:f99}");

            var result = actualConstant.Equals(expectedConstant);

            /* Otherwise, I think we can do a "normal" comparison.
             * No need to invoke xUnit Assert.Equal, I think. */
            Assert.True(result
                , $"Failed verifying '{typeof(IEquatable<IConstant>).FullName}"
                  + $".{nameof(IEquatable<IConstant>.Equals)}' invocation."
            );
        }

        protected virtual void VerifyConstant(IConstant expectedConstant, IConstant actualConstant)
        {
            // We need to treat Double Verification a little bit differently on account of NaN, Infinity, etc.
            if (expectedConstant is IConstant<double> expectedConstantDouble)
            {
                if (actualConstant is IConstant<double> actualConstantDouble)
                {
                    VerifyConstant(expectedConstantDouble, actualConstantDouble);
                }
                else
                {
                    Assert.True(actualConstant is IConstant<double>
                        , $"Expected {nameof(actualConstant)}"
                          + $"to be '{typeof(IConstant<double>).FullName}'");   
                }

                return;
            }

            Assert.NotNull(expectedConstant);
            Assert.NotSame(expectedConstant, actualConstant);

            // Otherwise, I think we can do a "normal" comparison.
            Assert.True(actualConstant.Equals(expectedConstant)
                , $"Failed verifying {typeof(IEquatable<IConstant>).FullName}"
                  + $".{nameof(IEquatable<IConstant>.Equals)} invocation."
            );

            // TODO: TBD: or this way ?
            Assert.Equal(expectedConstant, actualConstant);
        }

        protected virtual void VerifyEnumValueOption(EnumValueOption expectedOption, EnumValueOption actualOption)
        {
            GetRange(expectedOption, actualOption).AllNotNull();
            Assert.NotSame(expectedOption, actualOption);
            VerifyOptionIdentifierPath(expectedOption.Name, actualOption.Name);
            VerifyConstant(expectedOption.Value, actualOption.Value);
        }

        protected virtual void VerifyEnumField(EnumFieldDescriptor expectedDescriptor
            , EnumFieldDescriptor actualDescriptor)
        {
            GetRange(expectedDescriptor, actualDescriptor).AllNotNull();
            Assert.NotSame(expectedDescriptor, actualDescriptor);
            VerifyIdentifier(expectedDescriptor.Name, actualDescriptor.Name);
            Assert.Equal(expectedDescriptor.Ordinal, actualDescriptor.Ordinal);
            Assert.NotSame(expectedDescriptor.Options, actualDescriptor.Options);
            Assert.Equal(expectedDescriptor.Options.Count, actualDescriptor.Options.Count);
            expectedDescriptor.Options.VerifyBidirectionalParentage<IEnumFieldDescriptor, EnumValueOption>(expectedDescriptor);
            actualDescriptor.Options.VerifyBidirectionalParentage<IEnumFieldDescriptor, EnumValueOption>(actualDescriptor);
            foreach (var (expectedItem, actualItem) in expectedDescriptor.Options.Zip(actualDescriptor.Options, Tuple.Create))
            {
                VerifyEnumValueOption(expectedItem, actualItem);
            }
        }

        protected virtual void VerifyEnumStatement(EnumStatement expectedStatement, EnumStatement actualStatement)
        {
            Assert.NotNull(actualStatement);
            Assert.NotSame(expectedStatement, actualStatement);
            VerifyIdentifier(expectedStatement.Name, actualStatement.Name);
            GetRange(expectedStatement.Items, actualStatement.Items).AllNotNull();
            Assert.NotSame(expectedStatement.Items, actualStatement.Items);
            Assert.Equal(expectedStatement.Items.Count, actualStatement.Items.Count);
            expectedStatement.Items.VerifyBidirectionalParentage<IEnumStatement, IEnumBodyItem>(expectedStatement);
            actualStatement.Items.VerifyBidirectionalParentage<IEnumStatement, IEnumBodyItem>(actualStatement);
            foreach (var (expectedItem, actualItem) in expectedStatement.Items.Zip(actualStatement.Items, Tuple.Create))
            {
                switch (expectedItem)
                {
                    case OptionStatement optionStatement:
                        VerifyOptionStatement(optionStatement, actualItem as OptionStatement);
                        break;
                    case EnumFieldDescriptor enumField:
                        VerifyEnumField(enumField, actualItem as EnumFieldDescriptor);
                        break;
                    case EmptyStatement emptyStatement:
                        VerifyEmptyStatement(emptyStatement, actualItem as EmptyStatement);
                        break;
                    default:
                        throw ReportUnexpectedAlternative(expectedItem);
                }
            }
        }

        protected virtual void VerifyMessageBody(
            IMessageBodyParent expectedParent, IList<IMessageBodyItem> expectedItems
            , IMessageBodyParent actualParent, IList<IMessageBodyItem> actualItems)
        {
            GetRange<object>(expectedParent, expectedItems, actualParent, actualItems).AllNotNull();
            Assert.NotSame(expectedParent, expectedItems);
            Assert.NotSame(expectedItems, actualItems);
            Assert.Equal(expectedItems.Count, actualItems.Count);
            expectedParent.Items.VerifyParentage(expectedParent);
            actualParent.Items.VerifyParentage(actualParent);
            foreach (var (expectedItem, actualItem) in expectedItems.Zip(actualItems, Tuple.Create))
            {
                switch (expectedItem)
                {
                    case NormalFieldStatement normalField:
                        VerifyNormalFieldStatement(normalField, actualItem as NormalFieldStatement);
                        break;
                    case EnumStatement enumStatement:
                        VerifyEnumStatement(enumStatement, actualItem as EnumStatement);
                        break;
                    case MessageStatement messageStatement:
                        VerifyMessageStatement(messageStatement, actualItem as MessageStatement);
                        break;
                    case ExtendStatement extendStatement:
                        VerifyExtendStatement(extendStatement, actualItem as ExtendStatement);
                        break;
                    case ExtensionsStatement extensionStatement:
                        VerifyExtensionStatement(extensionStatement, actualItem as ExtensionsStatement);
                        break;
                    case GroupFieldStatement groupFieldStatement:
                        VerifyGroupFieldStatement(groupFieldStatement, actualItem as GroupFieldStatement);
                        break;
                    case OptionStatement optionStatement:
                        VerifyOptionStatement(optionStatement, actualItem as OptionStatement);
                        break;
                    case ReservedStatement reservedStatement:
                        VerifyReservedStatement(reservedStatement, actualItem as ReservedStatement);
                        break;
                    case OneOfStatement oneOfStatement:
                        VerifyOneOfStatement(oneOfStatement, actualItem as OneOfStatement);
                        break;
                    case MapFieldStatement mapFieldStatement:
                        VerifyMapFieldStatement(mapFieldStatement, actualItem as MapFieldStatement);
                        break;
                    default:
                        throw ReportUnexpectedAlternative(expectedItem);
                }
            }
        }

        protected virtual void VerifyNormalFieldStatement(NormalFieldStatement expectedField
            , NormalFieldStatement actualField)
        {
            Assert.NotNull(actualField);
            Assert.NotSame(expectedField, actualField);
            VerifyIdentifier(expectedField.Name, actualField.Name);
            Assert.Equal(expectedField.Label, actualField.Label);
            Assert.Equal(expectedField.FieldType, actualField.FieldType);
            Assert.Equal(expectedField.Number, actualField.Number);
            Assert.Equal(expectedField.Options.Count, actualField.Options.Count);
            expectedField.Options.VerifyBidirectionalParentage<INormalFieldStatement, FieldOption>(expectedField);
            actualField.Options.VerifyBidirectionalParentage<INormalFieldStatement, FieldOption>(actualField);
            foreach (var (expectedOption, actualOption) in expectedField.Options.Zip(actualField.Options, Tuple.Create))
            {
                VerifyFieldOption(expectedOption,actualOption);
            }
        }

        protected virtual void VerifyGroupFieldStatement(GroupFieldStatement expectedField
            , GroupFieldStatement actualField)
        {
            Assert.NotNull(expectedField);
            Assert.NotSame(expectedField, actualField);
            VerifyIdentifier(expectedField.Name, actualField.Name);
            Assert.Equal(expectedField.Label, actualField.Label);
            Assert.Equal(expectedField.Number, actualField.Number);
            VerifyMessageBody(expectedField, expectedField.Items, actualField, actualField.Items);
        }

        protected virtual void VerifyExtendStatement(ExtendStatement expectedStatement, ExtendStatement actualStatement)
        {
            Assert.NotNull(actualStatement);
            Assert.NotSame(expectedStatement, actualStatement);
            VerifyElementTypeIdentifierPath(expectedStatement.MessageType, actualStatement.MessageType);
            Assert.NotSame(expectedStatement.Items, actualStatement.Items);
            Assert.Equal(expectedStatement.Items.Count, actualStatement.Items.Count);
            expectedStatement.Items.VerifyBidirectionalParentage<IExtendStatement, IExtendBodyItem>(expectedStatement);
            actualStatement.Items.VerifyBidirectionalParentage<IExtendStatement, IExtendBodyItem>(actualStatement);
            foreach (var (expectedItem, actualItem) in expectedStatement.Items.Zip(actualStatement.Items, Tuple.Create))
            {
                switch (expectedItem)
                {
                    // TODO: TBD: fill in the blanks here...
                    case NormalFieldStatement normalField:
                        VerifyNormalFieldStatement(normalField, actualItem as NormalFieldStatement);
                        break;
                    case GroupFieldStatement groupField:
                        VerifyGroupFieldStatement(groupField, actualItem as GroupFieldStatement);
                        break;
                    case EmptyStatement emptyStatement:
                        VerifyEmptyStatement(emptyStatement, actualItem as EmptyStatement);
                        break;
                    default:
                        throw ReportUnexpectedAlternative(expectedItem);
                }
            }
        }

        /// <summary>
        /// Verifies the <see cref="RangeDescriptor"/> use case.
        /// </summary>
        /// <param name="expectedRange"></param>
        /// <param name="actualRange"></param>
        /// <see cref="RangeDescriptor"/>
        /// <see cref="IRangeDescriptor"/>
        /// <see cref="IEquatable{RangeDescriptor}"/>
        /// <see cref="IEquatable{IRangeDescriptor}"/>
        protected virtual void VerifyRangeDescriptor(RangeDescriptor expectedRange, RangeDescriptor actualRange)
        {
            GetRange(expectedRange, actualRange).AllNotNull();
            Assert.NotSame(expectedRange, actualRange);
            // The bits of Range Descriptor should Equal as well.
            Assert.Equal(expectedRange.Minimum, actualRange.Minimum);
            Assert.Equal(expectedRange.Maximum, actualRange.Maximum);
            // Verify both versions of IEquatable implementation.
            Assert.Equal(expectedRange, actualRange);
            Assert.Equal<IRangeDescriptor>(expectedRange, actualRange);
        }

        protected virtual void VerifyExtensionStatement(ExtensionsStatement expectedStatement
            , ExtensionsStatement actualStatement)
        {
            Assert.NotNull(actualStatement);
            Assert.NotSame(expectedStatement, actualStatement);
            GetRange(expectedStatement.Items, actualStatement.Items).AllNotNull();
            Assert.NotSame(expectedStatement.Items, actualStatement.Items);
            Assert.Equal(expectedStatement.Items.Count, actualStatement.Items.Count);
            expectedStatement.Items.VerifyBidirectionalParentage<IExtensionsStatement, RangeDescriptor>(expectedStatement);
            actualStatement.Items.VerifyBidirectionalParentage<IExtensionsStatement, RangeDescriptor>(actualStatement);
            foreach (var (expectedRange, actualRange) in expectedStatement.Items.Zip(actualStatement.Items, Tuple.Create))
            {
                VerifyRangeDescriptor(expectedRange, actualRange);
            }
        }

        protected virtual void VerifyFieldNamesReservedStatement(FieldNamesReservedStatement expectedStatement
            , FieldNamesReservedStatement actualStatement)
        {
            Assert.NotNull(actualStatement);
            Assert.NotSame(expectedStatement, actualStatement);
            GetRange(expectedStatement.Items, actualStatement.Items).AllNotNull();
            Assert.NotSame(expectedStatement.Items, actualStatement.Items);
            Assert.Equal(expectedStatement.Items.Count, actualStatement.Items.Count);
            expectedStatement.Items.VerifyBidirectionalParentage<IReservedStatement, Identifier>(expectedStatement);
            actualStatement.Items.VerifyBidirectionalParentage<IReservedStatement, Identifier>(actualStatement);
            foreach (var (expectedItem, actualItem) in expectedStatement.Items.Zip(actualStatement.Items, Tuple.Create))
            {
                Assert.Equal(expectedItem, actualItem);
            }
        }

        protected virtual void VerifyRangesReservedStatement(RangesReservedStatement expectedStatement
            , RangesReservedStatement actualStatement)
        {
            Assert.NotNull(actualStatement);
            Assert.NotSame(expectedStatement, actualStatement);
            Assert.NotSame(expectedStatement.Items, actualStatement.Items);
            GetRange(expectedStatement.Items, actualStatement.Items).AllNotNull();
            Assert.Equal(expectedStatement.Items.Count, actualStatement.Items.Count);
            expectedStatement.Items.VerifyBidirectionalParentage<IReservedStatement, RangeDescriptor>(expectedStatement);
            actualStatement.Items.VerifyBidirectionalParentage<IReservedStatement, RangeDescriptor>(actualStatement);
            foreach (var (expectedItem, actualItem) in expectedStatement.Items.Zip(actualStatement.Items, Tuple.Create))
            {
                VerifyRangeDescriptor(expectedItem, actualItem);
            }
        }

        protected virtual void VerifyReservedStatement(ReservedStatement expectedStatement
            , ReservedStatement actualStatement)
        {
            Assert.NotNull(actualStatement);
            Assert.NotSame(expectedStatement, actualStatement);
            switch (expectedStatement)
            {
                case FieldNamesReservedStatement fieldNamesReserved:
                    VerifyFieldNamesReservedStatement(fieldNamesReserved, actualStatement as FieldNamesReservedStatement);
                    break;
                case RangesReservedStatement rangesReserved:
                    VerifyRangesReservedStatement(rangesReserved, actualStatement as RangesReservedStatement);
                    break;
                default:
                    throw ReportUnexpectedAlternative(expectedStatement);
            }
        }

        protected virtual void VerifyFieldOption(FieldOption expectedOption, FieldOption actualOption)
        {
            Assert.NotSame(expectedOption, actualOption);
            GetRange(expectedOption.Value, actualOption.Value).AllNotNull();
            Assert.Equal(expectedOption.Value, actualOption.Value);
            VerifyOptionIdentifierPath(expectedOption.Name, actualOption.Name);
        }

        protected virtual void VerifyOneOfFieldStatement(OneOfFieldStatement expectedStatement
            , OneOfFieldStatement actualStatement)
        {
            Assert.NotNull(actualStatement);
            Assert.NotSame(expectedStatement, actualStatement);
            VerifyIdentifier(expectedStatement.Name, actualStatement.Name);
            Assert.Equal(expectedStatement.FieldType, actualStatement.FieldType);
            Assert.Equal(expectedStatement.Number, actualStatement.Number);
            GetRange(expectedStatement.Options, actualStatement.Options).AllNotNull();
            Assert.NotSame(expectedStatement.Options, actualStatement.Options);
            Assert.Equal(expectedStatement.Options.Count, actualStatement.Options.Count);
            expectedStatement.Options.VerifyBidirectionalParentage<IOneOfFieldStatement, FieldOption>(expectedStatement);
            actualStatement.Options.VerifyBidirectionalParentage<IOneOfFieldStatement, FieldOption>(actualStatement);
            foreach (var (expectedOption, actualOption) in expectedStatement.Options.Zip(actualStatement.Options, Tuple.Create))
            {
                VerifyFieldOption(expectedOption, actualOption);
            }
        }

        protected virtual void VerifyOneOfStatement(OneOfStatement expectedStatement, OneOfStatement actualStatement)
        {
            Assert.NotNull(actualStatement);
            Assert.NotSame(expectedStatement, actualStatement);
            VerifyIdentifier(expectedStatement.Name, actualStatement.Name);
            GetRange(expectedStatement.Items, actualStatement.Items).AllNotNull();
            Assert.NotSame(expectedStatement.Items, actualStatement.Items);
            Assert.Equal(expectedStatement.Items.Count, actualStatement.Items.Count);
            expectedStatement.Items.VerifyBidirectionalParentage<IOneOfStatement, IOneOfBodyItem>(expectedStatement);
            actualStatement.Items.VerifyBidirectionalParentage<IOneOfStatement, IOneOfBodyItem>(actualStatement);
            foreach (var (expectedItem, actualItem) in expectedStatement.Items.Zip(actualStatement.Items, Tuple.Create))
            {
                switch (expectedItem)
                {
                    case OneOfFieldStatement oneOfField:
                        VerifyOneOfFieldStatement(oneOfField, actualItem as OneOfFieldStatement);
                        break;
                    case EmptyStatement emptyStatement:
                        VerifyEmptyStatement(emptyStatement, actualItem as EmptyStatement);
                        break;
                    default:
                        throw ReportUnexpectedAlternative(expectedItem);
                }
            }
        }

        protected virtual void VerifyMapFieldStatement(MapFieldStatement expectedStatement
            , MapFieldStatement actualStatement)
        {
            Assert.NotNull(actualStatement);
            Assert.NotSame(expectedStatement, actualStatement);
            VerifyIdentifier(expectedStatement.Name, actualStatement.Name);
            Assert.Equal(expectedStatement.KeyType, actualStatement.KeyType);
            Assert.NotSame(expectedStatement.ValueType, actualStatement.ValueType);
            Assert.Equal(expectedStatement.ValueType, actualStatement.ValueType);
            Assert.Equal(expectedStatement.Number, actualStatement.Number);
            GetRange(expectedStatement.Options, actualStatement.Options).AllNotNull();
            Assert.NotSame(expectedStatement.Options, actualStatement.Options);
            Assert.Equal(expectedStatement.Options.Count, actualStatement.Options.Count);
            expectedStatement.Options.VerifyBidirectionalParentage<IMapFieldStatement, FieldOption>(expectedStatement);
            actualStatement.Options.VerifyBidirectionalParentage<IMapFieldStatement, FieldOption>(actualStatement);
            foreach (var (expectedOption, actualOption) in expectedStatement.Options.Zip(actualStatement.Options, Tuple.Create))
            {
                VerifyFieldOption(expectedOption, actualOption);
            }
        }

        protected virtual void VerifyMessageStatement(MessageStatement expectedStatement
            , MessageStatement actualStatement)
        {
            Assert.NotNull(actualStatement);
            Assert.NotSame(expectedStatement, actualStatement);
            VerifyIdentifier(expectedStatement.Name, actualStatement.Name);
            VerifyMessageBody(expectedStatement, expectedStatement.Items, actualStatement, actualStatement.Items);
        }

        protected virtual void VerifyTopLevelDefinition(ITopLevelDefinition expectedTopLevel
            , ITopLevelDefinition actualTopLevel)
        {
            Assert.NotNull(actualTopLevel);
            Assert.NotSame(expectedTopLevel, actualTopLevel);

            switch (expectedTopLevel)
            {
                case EnumStatement enumStatement:
                    VerifyEnumStatement(enumStatement, actualTopLevel as EnumStatement);
                    break;
                case ExtendStatement extendStatement:
                    VerifyExtendStatement(extendStatement, actualTopLevel as ExtendStatement);
                    break;
                case MessageStatement messageStatement:
                    VerifyMessageStatement(messageStatement, actualTopLevel as MessageStatement);
                    break;
                default:
                    throw ReportUnexpectedAlternative(expectedTopLevel);
            }
        }

        protected virtual void VerifyOptionStatement(OptionStatement expectedStatement, OptionStatement actualStatement)
        {
            Assert.NotNull(expectedStatement);
            Assert.NotSame(expectedStatement, actualStatement);
            VerifyOptionIdentifierPath(expectedStatement.Name, actualStatement.Name);
            VerifyConstant(expectedStatement.Value, actualStatement.Value);
        }

        protected virtual void VerifyPackageStatement(PackageStatement expectedStatement, PackageStatement actualStatement)
        {
            Assert.NotNull(expectedStatement);
            Assert.NotSame(expectedStatement, actualStatement);
            Assert.NotSame(expectedStatement.PackagePath, actualStatement.PackagePath);
            Assert.Equal(expectedStatement.PackagePath, actualStatement.PackagePath);
        }

        protected virtual void VerifyImportStatement(ImportStatement expectedStatement, ImportStatement actualStatement)
        {
            Assert.NotNull(expectedStatement);
            Assert.NotSame(expectedStatement, actualStatement);
            Assert.Equal(expectedStatement.Modifier, actualStatement.Modifier);
            Assert.Equal(expectedStatement.ImportPath, actualStatement.ImportPath);
        }

        protected virtual void VerifyEmptyStatement(EmptyStatement expectedStatement, EmptyStatement actualStatement)
        {
            Assert.NotNull(expectedStatement);
            Assert.NotSame(expectedStatement, actualStatement);
        }

        protected virtual void VerifySyntaxStatement(SyntaxStatement expectedStatement, SyntaxStatement actualStatement)
        {
            Assert.NotNull(expectedStatement);
            Assert.NotSame(expectedStatement, actualStatement);
            Assert.Equal(expectedStatement.Syntax, actualStatement.Syntax);
        }

        protected virtual void VerifyProto(ProtoDescriptor expectedProto, ProtoDescriptor actualProto)
        {
            Assert.NotNull(expectedProto);
            Assert.NotSame(expectedProto, actualProto);
            GetRange(expectedProto.Syntax).VerifyParentage(expectedProto);
            GetRange(actualProto.Syntax).VerifyParentage(actualProto);
            VerifySyntaxStatement(expectedProto.Syntax, actualProto.Syntax);
            GetRange(expectedProto.Items, actualProto.Items).AllNotNull();
            Assert.NotSame(expectedProto.Items, actualProto.Items);
            Assert.Equal(expectedProto.Items.Count, actualProto.Items.Count);
            expectedProto.Items.VerifyBidirectionalParentage<IProto, IProtoBodyItem>(expectedProto);
            actualProto.Items.VerifyBidirectionalParentage<IProto, IProtoBodyItem>(actualProto);
            foreach (var (expectedItem, actualItem) in expectedProto.Items.Zip(actualProto.Items, Tuple.Create))
            {
                switch (expectedItem)
                {
                    case EmptyStatement emptyStatement:
                        VerifyEmptyStatement(emptyStatement, actualItem as EmptyStatement);
                        break;
                    case ImportStatement importStatement:
                        VerifyImportStatement(importStatement, actualItem as ImportStatement);
                        break;
                    case PackageStatement packageStatement:
                        VerifyPackageStatement(packageStatement, actualItem as PackageStatement);
                        break;
                    case OptionStatement optionStatement:
                        VerifyOptionStatement(optionStatement, actualItem as OptionStatement);
                        break;
                    case ITopLevelDefinition topLevel:
                        VerifyTopLevelDefinition(topLevel, actualItem as ITopLevelDefinition);
                        break;
                    default:
                        throw ReportUnexpectedAlternative(expectedItem);
                }
            }
        }

        protected virtual void VerifyProto(ProtoDescriptor actualProto)
        {
            Assert.NotNull(actualProto);
            VerifyProto(ExpectedProto, actualProto);
        }

        protected override void VerifyListener(TListener listener)
        {
            base.VerifyListener(listener);
            VerifyProto(listener.ActualProto);
        }
    }
}
