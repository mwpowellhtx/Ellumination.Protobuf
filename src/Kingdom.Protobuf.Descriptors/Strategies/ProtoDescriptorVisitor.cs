using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    // ReSharper disable once UnusedMember.Global
    /// <inheritdoc />
    public class ProtoDescriptorVisitor : DescriptorVisitorBase<ProtoDescriptor>
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
            //// TODO: TBD: verify Syntax is Proto2... or according to the furnished visitor policy...
            // statement.Syntax
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
            // TODO: TBD: what to do here, if anything...
        }

        /// <summary>
        /// Visits the <paramref name="identifier"/>.
        /// </summary>
        /// <param name="identifier"></param>
        protected virtual void VisitIdentifier(Identifier identifier)
        {
            void VisitIdentifier()
            {
                // TODO: TBD: visit the Identifier using scoping rules...
            }

            switch (identifier)
            {
                case null:
                    throw ReportUnexpectedAlternative<Identifier>();
                default:
                    VisitIdentifier();
                    break;
            }
        }

        /// <summary>
        /// Visits the <paramref name="path"/>.
        /// </summary>
        /// <param name="path"></param>
        protected virtual void VisitIdentifierPath(IIdentifierPath path)
        {
            switch (path)
            {
                case ElementTypeIdentifierPath _ /*identifierPath*/:
                    // TODO: TBD: verify along scoping rules...
                    //identifierPath.IsGlobalScope
                    break;
                case OptionIdentifierPath _ /*identifierPath*/:
                    // TODO: TBD: verify along scoping rules...
                    //identifierPath.IsPrefixGrouped
                    //identifierPath.SuffixStartIndex
                    break;
                case IdentifierPath _ /*identifierPath*/:
                    // TODO: TBD: verify along scoping rules...
                    break;
                case null:
                    throw ReportUnexpectedAlternative<IIdentifierPath>();
                default:
                    throw ReportUnexpectedAlternative(path);
            }
        }

        /// <summary>
        /// Visits the <paramref name="descriptor"/>.
        /// </summary>
        /// <param name="descriptor"></param>
        protected virtual void VisitEnumFieldDescriptor(EnumFieldDescriptor descriptor)
        {
            //// TODO: TBD: verifies a valid ordinal, for aliasing, etc.
            //descriptor.Ordinal
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
                        VisitRange(item, VisitNumber);
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
            //// TODO: TBD: verify that MessageType really does reference a Message assuming scoping rules...
            //statement.MessageType
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
        protected virtual void VisitNumber(long index)
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
            const long defaultMaximum = int.MaxValue;

            var actualMaximum = descriptor.Maximum ?? defaultMaximum;

            // TODO: TBD: verifies the Minimum and Maximum in the range are valid and parts in between...
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
                        VisitRange(descriptor, VisitNumber);
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
        /// Visits the <paramref name="statement"/>.
        /// </summary>
        /// <param name="statement"></param>
        protected virtual void VisitOneOfFieldStatement(OneOfFieldStatement statement)
        {
            //// TODO: TBD: this is a case where having the separate Variant type is somewhat helpful...
            //// TODO: TBD: however, if I didn't still think the pattern was close enough that in fact it justifies merging Constant with Variant...
            //// TODO: TBD: or vice versa and just doing the appropriate verification/visitation when necessary...
            //// TODO: TBD: verify the Variant Type, either element-name/identifier-path or actual type
            //statement.Type
            VisitIdentifier(statement.Name);
            VisitNumber(statement.Number);
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
            //// TODO: TBD: ditto Variant visitation...
            //statement.Type
            VisitIdentifier(statement.Name);
            VisitNumber(statement.Number);
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
            VisitNumber(statement.Number);
            VisitMessageBody(statement.Items);
        }

        /// <summary>
        /// Visits the <paramref name="statement"/>.
        /// </summary>
        /// <param name="statement"></param>
        protected virtual void VisitMapFieldStatement(MapFieldStatement statement)
        {
            //// TODO: TBD: verify the Key...
            //statement.Key
            //// TODO: TBD: ditto Variant visitation...
            //statement.Type
            VisitIdentifier(statement.Name);
            VisitNumber(statement.Number);
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
        /// Visits the <paramref name="statement"/>.
        /// </summary>
        /// <param name="statement"></param>
        protected virtual void VisitImportStatement(ImportStatement statement)
        {
            //// TODO: TBD: what to do about Modifier?
            //// TODO: TBD: what to do about ImportPath? path must be a valid path, file may exist? or be in a specific PROTO_IMPORT_PATH?
            //statement.ImportPath
            //statement.Modifier
        }

        /// <summary>
        /// Visits the <paramref name="statement"/>.
        /// </summary>
        /// <param name="statement"></param>
        protected virtual void VisitPackageStatement(PackageStatement statement)
        {
            //// TODO: TBD: what to do about Package Path, scoping rules, etc.
            //statement.PackagePath
        }

        /// <summary>
        /// Visits the <paramref name="descriptor"/>.
        /// </summary>
        /// <typeparamref name="T"/>
        /// <param name="descriptor"></param>
        protected virtual void VisitOptionDescriptor<T>(T descriptor)
            where T : OptionDescriptorBase
        {
            VisitIdentifierPath(descriptor.Name);
            //// TODO: TBD: verify the Constant...
            //statement.Value
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
