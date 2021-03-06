﻿using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Collections.Variants;
    using static FieldNumbers;
    using static SyntaxKind;

    /// <inheritdoc />
    public abstract class ProtoDescriptorVisitorBase : DescriptorVisitorBase<ProtoDescriptor>
    {
        /// <summary>
        /// Reports an <see cref="InvalidOperationException"/> given an
        /// unexpected <typeparamref name="T"/> <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        protected static InvalidOperationException ReportUnexpectedAlternative<T>(T value = null)
            where T : class
        {
            string GetItemTypeOrNull() => value == null ? "null" : $"{typeof(T).FullName}";
            return new InvalidOperationException($"Unexpected kind of Item '{GetItemTypeOrNull()}'");
        }

        /// <summary>
        /// Visits the <paramref name="statement"/>.
        /// </summary>
        /// <param name="statement"></param>
        protected virtual void VisitSyntaxStatement(SyntaxStatement statement)
        {
            if (statement.Syntax == Proto2)
            {
                return;
            }

            var message = $"Unexpected '{nameof(SyntaxStatement.Syntax)}' value '{statement.Syntax}'.";
            throw new ArgumentException(message, nameof(statement));
        }

        /// <summary>
        /// Visits the <paramref name="statement"/>.
        /// </summary>
        /// <param name="statement"></param>
        protected virtual void VisitEmptyStatement(EmptyStatement statement)
        {
        }

        /// <summary>
        /// Visits the <paramref name="value"/>.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void VisitLabel(LabelKind value)
        {
        }

        /// <summary>
        /// Visits the <paramref name="identifier"/>.
        /// </summary>
        /// <param name="identifier"></param>
        protected virtual void VisitIdentifier(Identifier identifier)
        {
            if (identifier != null)
            {
                return;
            }

            throw ReportUnexpectedAlternative<Identifier>();
        }

        /// <summary>
        /// Visits the <paramref name="path"/>.
        /// </summary>
        /// <param name="path"></param>
        protected virtual void VisitElementTypeIdentifierPath(ElementTypeIdentifierPath path)
        {
        }

        /// <summary>
        /// Visits the <paramref name="path"/>.
        /// </summary>
        /// <param name="path"></param>
        protected virtual void VisitOptionIdentifierPath(OptionIdentifierPath path)
        {
        }

        /// <summary>
        /// Visits the <paramref name="path"/>.
        /// </summary>
        /// <param name="path"></param>
        protected virtual void VisitIdentifierPath(IdentifierPath path)
        {
        }

        /// <summary>
        /// Visits the <paramref name="ordinal"/>.
        /// </summary>
        /// <param name="ordinal"></param>
        protected virtual void VisitEnumFieldOrdinal(long ordinal)
        {
        }

        /// <summary>
        /// Visits the <paramref name="descriptor"/>.
        /// </summary>
        /// <param name="descriptor"></param>
        protected virtual void VisitEnumFieldDescriptor(EnumFieldDescriptor descriptor)
        {
            VisitEnumFieldOrdinal(descriptor.Ordinal);
            VisitIdentifier(descriptor.Name);
            descriptor.Options.ToList().ForEach(VisitOptionDescriptor);
        }

        /// <summary>
        /// Visits the <paramref name="statement"/>.
        /// </summary>
        /// <param name="statement"></param>
        protected virtual void VisitEnumStatement(EnumStatement statement)
        {
            VisitIdentifier(statement.Name);
            foreach (var item in statement.Items)
            {
                switch (item)
                {
                    case EmptyStatement statementItem:
                        VisitEmptyStatement(statementItem);
                        break;
                    case OptionStatement statementItem:
                        VisitOptionDescriptor(statementItem);
                        break;
                    case EnumFieldDescriptor descriptorItem:
                        VisitEnumFieldDescriptor(descriptorItem);
                        break;
                    case null:
                        throw ReportUnexpectedAlternative<IEnumBodyItem>();
                    default:
                        throw ReportUnexpectedAlternative(item);
                }
            }
        }

        /// <summary>
        /// Visits the <paramref name="statement"/>.
        /// </summary>
        /// <param name="statement"></param>
        protected virtual void VisitExtensionsStatement(ExtensionsStatement statement)
        {
            foreach (var item in statement.Items)
            {
                switch (item)
                {
                    case null:
                        throw ReportUnexpectedAlternative<RangeDescriptor>();
                    default:
                        VisitRange(item, VisitFieldNumber);
                        break;
                }
            }
        }

        /// <summary>
        /// Visits the <paramref name="statement"/>.
        /// </summary>
        /// <param name="statement"></param>
        protected virtual void VisitExtendStatement(ExtendStatement statement)
        {
            VisitElementTypeIdentifierPath(statement.MessageType);
            foreach (var item in statement.Items)
            {
                switch (item)
                {
                    case NormalFieldStatement normalField:
                        VisitNormalFieldStatement(normalField);
                        break;
                    case GroupFieldStatement groupField:
                        VisitGroupFieldStatement(groupField);
                        break;
                    case EmptyStatement emptyStatement:
                        VisitEmptyStatement(emptyStatement);
                        break;
                    case null:
                        throw ReportUnexpectedAlternative<IExtendBodyItem>();
                    default:
                        throw ReportUnexpectedAlternative(item);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        protected delegate void NumberVisitationCallback(long number);

        /// <summary>
        /// Visits the <paramref name="index"/>.
        /// </summary>
        /// <param name="index"></param>
        protected virtual void VisitFieldNumber(long index)
        {
        }

        /// <summary>
        /// Visits the <paramref name="statement"/>.
        /// </summary>
        /// <param name="statement"></param>
        protected virtual void VisitFieldNamesReservedStatement(FieldNamesReservedStatement statement)
        {
            foreach (var identifier in statement.Items)
            {
                switch (identifier)
                {
                    case null:
                        throw ReportUnexpectedAlternative<Identifier>();
                    default:
                        VisitIdentifier(identifier);
                        break;
                }
            }
        }

        /// <summary>
        /// Visits the <paramref name="descriptor"/>.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="callback"></param>
        protected virtual void VisitRange(RangeDescriptor descriptor, NumberVisitationCallback callback)
        {
            // Only evaluate it when we need to.
            long GetMaximumFieldNumber() => MaximumFieldNumber;

            var actualMaximum = descriptor.Maximum ?? GetMaximumFieldNumber();

            for (var i = descriptor.Minimum; i < actualMaximum; ++i)
            {
                callback(i);
            }
        }

        /// <summary>
        /// Visits the <paramref name="statement"/>.
        /// </summary>
        /// <param name="statement"></param>
        protected virtual void VisitRangesReservedStatement(RangesReservedStatement statement)
        {
            foreach (var descriptor in statement.Items)
            {
                switch (descriptor)
                {
                    case null:
                        throw ReportUnexpectedAlternative<RangeDescriptor>();
                    default:
                        VisitRange(descriptor, VisitFieldNumber);
                        break;
                }
            }
        }

        /// <summary>
        /// Visits the <paramref name="statement"/>.
        /// </summary>
        /// <param name="statement"></param>
        protected virtual void VisitReservedStatement(ReservedStatement statement)
        {
            switch (statement)
            {
                case FieldNamesReservedStatement reservedStatement:
                    VisitFieldNamesReservedStatement(reservedStatement);
                    break;
                case RangesReservedStatement reservedStatement:
                    VisitRangesReservedStatement(reservedStatement);
                    break;
            }
        }

        /// <summary>
        /// Visits the <paramref name="fieldType"/>.
        /// </summary>
        /// <param name="fieldType"></param>
        protected virtual void VisitFieldType(IVariant fieldType)
        {
        }

        /// <summary>
        /// Visits the <paramref name="statement"/>.
        /// </summary>
        /// <param name="statement"></param>
        protected virtual void VisitOneOfFieldStatement(OneOfFieldStatement statement)
        {
            //// TODO: TBD: this is a case where having the separate Variant type is somewhat helpful...
            //// TODO: TBD: however, if I didn't still think the pattern was close enough that in fact it justifies merging Constant with Variant...
            //// TODO: TBD: or vice versa and just doing the appropriate verification/visitation when necessary...
            //// TODO: TBD: verify the Variant Type, either element-name/identifier-path or actual type
            VisitFieldType(statement.FieldType);
            VisitIdentifier(statement.Name);
            VisitFieldNumber(statement.Number);
            statement.Options.ToList().ForEach(VisitOptionDescriptor);
        }

        /// <summary>
        /// Visits the <paramref name="statement"/>.
        /// </summary>
        /// <param name="statement"></param>
        protected virtual void VisitOneOfStatement(OneOfStatement statement)
        {
            VisitIdentifier(statement.Name);
            foreach (var item in statement.Items)
            {
                switch (item)
                {
                    case OneOfFieldStatement oneOfField:
                        VisitOneOfFieldStatement(oneOfField);
                        break;
                    case EmptyStatement emptyStatement:
                        VisitEmptyStatement(emptyStatement);
                        break;
                    case null:
                        throw ReportUnexpectedAlternative<IOneOfBodyItem>();
                    default:
                        throw ReportUnexpectedAlternative(item);
                }
            }
        }

        /// <summary>
        /// Visits the <paramref name="statement"/>.
        /// </summary>
        /// <param name="statement"></param>
        protected virtual void VisitNormalFieldStatement(NormalFieldStatement statement)
        {
            VisitLabel(statement.Label);
            VisitFieldType(statement.FieldType);
            VisitIdentifier(statement.Name);
            VisitFieldNumber(statement.Number);
            statement.Options.ToList().ForEach(VisitOptionDescriptor);
        }

        /// <summary>
        /// Visits the <paramref name="statement"/>.
        /// </summary>
        /// <param name="statement"></param>
        protected virtual void VisitGroupFieldStatement(GroupFieldStatement statement)
        {
            VisitLabel(statement.Label);
            VisitIdentifier(statement.Name);
            VisitFieldNumber(statement.Number);
            VisitMessageBody(statement.Items);
        }

        /// <summary>
        /// Visits the <paramref name="keyType"/>.
        /// </summary>
        /// <param name="keyType"></param>
        protected virtual void VisitMapFieldKeyType(KeyType keyType)
        {
        }

        /// <summary>
        /// Visits the <paramref name="valueType"/>.
        /// </summary>
        /// <param name="valueType"></param>
        protected virtual void VisitMapFieldValueType(IVariant valueType)
        {
        }

        /// <summary>
        /// Visits the <paramref name="statement"/>.
        /// </summary>
        /// <param name="statement"></param>
        protected virtual void VisitMapFieldStatement(MapFieldStatement statement)
        {
            VisitMapFieldKeyType(statement.KeyType);
            VisitMapFieldValueType(statement.ValueType);
            VisitIdentifier(statement.Name);
            VisitFieldNumber(statement.Number);
            statement.Options.ToList().ForEach(VisitOptionDescriptor);
        }

        /// <summary>
        /// Visits the <paramref name="items"/>.
        /// </summary>
        /// <param name="items"></param>
        protected virtual void VisitMessageBody(IEnumerable<IMessageBodyItem> items)
        {
            foreach (var item in items)
            {
                switch (item)
                {
                    case NormalFieldStatement normalField:
                        VisitNormalFieldStatement(normalField);
                        break;
                    case EnumStatement enumStatement:
                        VisitEnumStatement(enumStatement);
                        break;
                    case MessageStatement messageStatement:
                        VisitMessageStatement(messageStatement);
                        break;
                    case ExtendStatement extendStatement:
                        VisitExtendStatement(extendStatement);
                        break;
                    case ExtensionsStatement extensionStatement:
                        VisitExtensionsStatement(extensionStatement);
                        break;
                    case GroupFieldStatement groupField:
                        VisitGroupFieldStatement(groupField);
                        break;
                    case OptionStatement optionStatement:
                        VisitOptionDescriptor(optionStatement);
                        break;
                    case ReservedStatement reservedStatement:
                        VisitReservedStatement(reservedStatement);
                        break;
                    case OneOfStatement oneOfStatement:
                        VisitOneOfStatement(oneOfStatement);
                        break;
                    case MapFieldStatement mapField:
                        VisitMapFieldStatement(mapField);
                        break;
                    case null:
                        throw ReportUnexpectedAlternative<IMessageBodyItem>();
                    default:
                        throw ReportUnexpectedAlternative(item);
                }
            }
        }

        /// <summary>
        /// Visits the <paramref name="statement"/>.
        /// </summary>
        /// <param name="statement"></param>
        protected virtual void VisitMessageStatement(MessageStatement statement)
        {
            VisitIdentifier(statement.Name);
            VisitMessageBody(statement.Items);
        }

        /// <summary>
        /// Verifies the <paramref name="topLevel"/>.
        /// </summary>
        /// <param name="topLevel"></param>
        protected virtual void VisitTopLevelDefinition(ITopLevelDefinition topLevel)
        {
            switch (topLevel)
            {
                case EnumStatement statement:
                    VisitEnumStatement(statement);
                    break;
                case ExtensionsStatement statement:
                    VisitExtensionsStatement(statement);
                    break;
                case MessageStatement statement:
                    VisitMessageStatement(statement);
                    break;
            }
        }

        /// <summary>
        /// Visits the <paramref name="path"/>.
        /// </summary>
        /// <param name="path"></param>
        protected virtual void VisitImportPath(string path)
        {
        }

        /// <summary>
        /// Visits the <paramref name="modifier"/>.
        /// </summary>
        /// <param name="modifier"></param>
        protected virtual void VisitImportModifier(ImportModifierKind? modifier)
        {
        }

        /// <summary>
        /// Visits the <paramref name="statement"/>.
        /// </summary>
        /// <param name="statement"></param>
        protected virtual void VisitImportStatement(ImportStatement statement)
        {
            VisitImportPath(statement.ImportPath);   
            VisitImportModifier(statement.Modifier);
        }

        /// <summary>
        /// Visits the <paramref name="statement"/>.
        /// </summary>
        /// <param name="statement"></param>
        protected virtual void VisitPackageStatement(PackageStatement statement)
        {
            VisitIdentifierPath(statement.PackagePath);
        }

        /// <summary>
        /// Visits the <paramref name="constant"/>.
        /// </summary>
        /// <param name="constant"></param>
        protected virtual void VisitOptionValue(IVariant constant)
        {
        }

        /// <summary>
        /// Visits the <paramref name="descriptor"/>.
        /// </summary>
        /// <typeparamref name="T"/>
        /// <param name="descriptor"></param>
        protected virtual void VisitOptionDescriptor<T>(T descriptor)
            where T : OptionDescriptorBase
        {
            void VisitDescriptorName(IIdentifierPath path)
            {
                switch (path)
                {
                    case ElementTypeIdentifierPath identifierPath:
                        VisitElementTypeIdentifierPath(identifierPath);
                        break;
                    case OptionIdentifierPath identifierPath:
                        VisitOptionIdentifierPath(identifierPath);
                        break;
                    case IdentifierPath identifierPath:
                        VisitIdentifierPath(identifierPath);
                        break;
                    case null:
                        throw ReportUnexpectedAlternative<IIdentifierPath>();
                    default:
                        throw ReportUnexpectedAlternative(path);
                }
            }

            VisitDescriptorName(descriptor.Name);
            VisitOptionValue(descriptor.Value);
        }

        /// <summary>
        /// Gets the Current <see cref="ProtoDescriptor"/>.
        /// </summary>
        protected ProtoDescriptor CurrentDescriptor { get; private set; }

        /// <summary>
        /// Occurs On Visit Finished.
        /// </summary>
        protected virtual void OnFinishedVisit()
        {
        }

        /// <summary>
        /// Occurs On <paramref name="exception"/> Visited.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exception"></param>
        protected virtual void OnExceptionVisited<T>(T exception)
        {
        }

        /// <summary>
        /// Visits the <paramref name="exception"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exception"></param>
        /// <returns></returns>
        private T VisitException<T>(T exception)
            where T : Exception
        {
            OnExceptionVisited(exception);
            return exception;
        }

        /// <inheritdoc />
        public override void Visit(ProtoDescriptor descriptor)
        {
            CurrentDescriptor = descriptor;

            try
            {
                VisitSyntaxStatement(descriptor?.Syntax);

                foreach (var item in descriptor?.Items ?? GetRange<IProtoBodyItem>().ToList())
                {
                    switch (item)
                    {
                        case EmptyStatement statement:
                            VisitEmptyStatement(statement);
                            break;
                        case ImportStatement statement:
                            VisitImportStatement(statement);
                            break;
                        case PackageStatement statement:
                            VisitPackageStatement(statement);
                            break;
                        case OptionStatement statement:
                            VisitOptionDescriptor(statement);
                            break;
                        case ITopLevelDefinition topLevel:
                            VisitTopLevelDefinition(topLevel);
                            break;
                        case null:
                            throw ReportUnexpectedAlternative<IProtoBodyItem>();
                        default:
                            throw ReportUnexpectedAlternative(item);
                    }
                }

                OnFinishedVisit();
            }
            catch (InvalidOperationException ex)
            {
                throw VisitException(ex);
            }
            catch (Exception ex)
            {
                throw VisitException(ex);
            }
        }
    }
}
