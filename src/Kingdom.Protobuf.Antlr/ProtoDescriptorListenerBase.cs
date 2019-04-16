using System;
using Kingdom.Protobuf.Collections;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;

    /// <inheritdoc cref="BaseErrorListener"/>
    public abstract class ProtoDescriptorListenerBase : BaseErrorListener, IProtoListener
    {
        /// <summary>
        /// Gets the AST Stack.
        /// </summary>
        protected AbstractSyntaxTreeStack<ProtoDescriptor> Stack { get; }
            = new AbstractSyntaxTreeStack<ProtoDescriptor>();

        /// <summary>
        /// Gets the Actual <see cref="ProtoDescriptor"/> instance.
        /// </summary>
        public ProtoDescriptor ActualProto { get; protected set; }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Returns the Default <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected static T GetDefault<T>() => default(T);

        /// <summary>
        /// Callback provided in order to Synthesize the <typeparamref name="TCurrent"/> value.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TCurrent"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        protected delegate TCurrent SynthesizeCallback<in TContext, out TCurrent>(TContext context)
            where TContext : RuleContext;

        /// <summary>
        /// Callers must specify the Synthesized Type as strongly as possible.
        /// This becomes critical when the Stack is poised to unwind properly.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TCurrent"></typeparam>
        /// <param name="context"></param>
        /// <param name="synthesize"></param>
        protected void OnEnterSynthesizeAttribute<TContext, TCurrent>(TContext context,
            SynthesizeCallback<TContext, TCurrent> synthesize)
            where TContext : RuleContext
            => Stack.PushBack(synthesize(context));

        /// <summary>
        /// The tricky part about unwinding the Stack properly is that the Caller must know the
        /// precise strongly typed context in which the unwind is to take place. In and of itself,
        /// this does not seem like a big deal, but it has the potential to be highly ambiguous in
        /// situations involving statements like Message, Enum, and Extend, which can land as
        /// either Top Level Definitions interface or rolled up in terms of a Message Body list
        /// constituent. The trade off is subtle, but it seems we cannot leverage CSharp language
        /// nuances such as the Is keyword, As, and so forth, when implying the desired type.
        /// Instead, the caller must know precisely the Type in order for the Stack to unwind
        /// correctly.
        /// </summary>
        /// <typeparam name="TPrevious"></typeparam>
        /// <typeparam name="TCurrent"></typeparam>
        /// <param name="reduction"></param>
        protected bool TryOnExitResolveSynthesizedAttribute<TPrevious, TCurrent>(
            ReduceStackToPreviousCallback<TPrevious, TCurrent> reduction)
            => Stack.TryReduce(reduction);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="validate"></param>
        /// <param name="render"></param>
        /// <param name="visit"></param>
        protected void ValidateFirstDescriptorItem<TValue>(Func<TValue, bool> validate
            , Func<TValue, string> render = null, Action<InvalidOperationException> visit = null)
            => Stack.ValidateFirst(validate, render, visit);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="validate"></param>
        /// <param name="render"></param>
        /// <param name="visit"></param>
        protected void ValidateLastDescriptorItem<TValue>(Func<TValue, bool> validate
            , Func<TValue, string> render = null, Action<InvalidOperationException> visit = null)
            => Stack.ValidateLast(validate, render, visit);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="validate"></param>
        /// <param name="render"></param>
        /// <param name="visit"></param>
        protected void ValidateCurrentDescriptorItem<TValue>(Func<TValue, bool> validate
            , Func<TValue, string> render = null, Action<InvalidOperationException> visit = null)
            => Stack.ValidateLast(validate, render, visit);

        /// <inheritdoc />
        public void VisitTerminal(ITerminalNode node)
        {
        }

        /// <inheritdoc />
        public virtual void VisitErrorNode(IErrorNode node)
        {
        }

        /// <inheritdoc />
        public virtual void EnterEveryRule(ParserRuleContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitEveryRule(ParserRuleContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterIdent(ProtoParser.IdentContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitIdent(ProtoParser.IdentContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterGroupName(ProtoParser.GroupNameContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitGroupName(ProtoParser.GroupNameContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterFullIdent(ProtoParser.FullIdentContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitFullIdent(ProtoParser.FullIdentContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterGroupedOptionNamePrefix(ProtoParser.GroupedOptionNamePrefixContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitGroupedOptionNamePrefix(ProtoParser.GroupedOptionNamePrefixContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterSingleOptionNamePrefix(ProtoParser.SingleOptionNamePrefixContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitSingleOptionNamePrefix(ProtoParser.SingleOptionNamePrefixContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterOptionNamePrefix(ProtoParser.OptionNamePrefixContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitOptionNamePrefix(ProtoParser.OptionNamePrefixContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterOptionNameSuffix(ProtoParser.OptionNameSuffixContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitOptionNameSuffix(ProtoParser.OptionNameSuffixContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterOptionName(ProtoParser.OptionNameContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitOptionName(ProtoParser.OptionNameContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterLabel(ProtoParser.LabelContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitLabel(ProtoParser.LabelContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterElementTypeGlobalScope(ProtoParser.ElementTypeGlobalScopeContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitElementTypeGlobalScope(ProtoParser.ElementTypeGlobalScopeContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterElementType(ProtoParser.ElementTypeContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitElementType(ProtoParser.ElementTypeContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterProtoType(ProtoParser.ProtoTypeContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitProtoType(ProtoParser.ProtoTypeContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterType(ProtoParser.TypeContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitType(ProtoParser.TypeContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterKeyType(ProtoParser.KeyTypeContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitKeyType(ProtoParser.KeyTypeContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterBooleanFalse(ProtoParser.BooleanFalseContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitBooleanFalse(ProtoParser.BooleanFalseContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterBooleanTrue(ProtoParser.BooleanTrueContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitBooleanTrue(ProtoParser.BooleanTrueContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterBooleanLit(ProtoParser.BooleanLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitBooleanLit(ProtoParser.BooleanLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterSign(ProtoParser.SignContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitSign(ProtoParser.SignContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterHexLit(ProtoParser.HexLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitHexLit(ProtoParser.HexLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterOctLit(ProtoParser.OctLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitOctLit(ProtoParser.OctLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterDecLit(ProtoParser.DecLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitDecLit(ProtoParser.DecLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterIntLit(ProtoParser.IntLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitIntLit(ProtoParser.IntLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterFieldNumber(ProtoParser.FieldNumberContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitFieldNumber(ProtoParser.FieldNumberContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterSignedIntLit(ProtoParser.SignedIntLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitSignedIntLit(ProtoParser.SignedIntLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterInf(ProtoParser.InfContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitInf(ProtoParser.InfContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterNan(ProtoParser.NanContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitNan(ProtoParser.NanContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterNumericFloatLit(ProtoParser.NumericFloatLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitNumericFloatLit(ProtoParser.NumericFloatLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterFloatLit(ProtoParser.FloatLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitFloatLit(ProtoParser.FloatLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterSignedFloatLit(ProtoParser.SignedFloatLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitSignedFloatLit(ProtoParser.SignedFloatLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterStrLit(ProtoParser.StrLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitStrLit(ProtoParser.StrLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterFullIdentLit(ProtoParser.FullIdentLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitFullIdentLit(ProtoParser.FullIdentLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterConstant(ProtoParser.ConstantContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitConstant(ProtoParser.ConstantContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterEmptyDecl(ProtoParser.EmptyDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitEmptyDecl(ProtoParser.EmptyDeclContext context)
        {
        }

        ///// <inheritdoc />
        //public virtual void EnterSyntaxValue(ProtoParser.SyntaxValueContext context)
        //{
        //}

        ///// <inheritdoc />
        //public virtual void ExitSyntaxValue(ProtoParser.SyntaxValueContext context)
        //{
        //}

        /// <inheritdoc />
        public virtual void EnterSyntaxDecl(ProtoParser.SyntaxDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitSyntaxDecl(ProtoParser.SyntaxDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterImportModifier(ProtoParser.ImportModifierContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitImportModifier(ProtoParser.ImportModifierContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterImportDecl(ProtoParser.ImportDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitImportDecl(ProtoParser.ImportDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterPackageDecl(ProtoParser.PackageDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitPackageDecl(ProtoParser.PackageDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterOptionDecl(ProtoParser.OptionDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitOptionDecl(ProtoParser.OptionDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterFieldOption(ProtoParser.FieldOptionContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitFieldOption(ProtoParser.FieldOptionContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterFieldOptions(ProtoParser.FieldOptionsContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitFieldOptions(ProtoParser.FieldOptionsContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterFieldDecl(ProtoParser.FieldDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitFieldDecl(ProtoParser.FieldDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterGroupDecl(ProtoParser.GroupDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitGroupDecl(ProtoParser.GroupDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterOneOfDecl(ProtoParser.OneOfDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitOneOfDecl(ProtoParser.OneOfDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterOneOfField(ProtoParser.OneOfFieldContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitOneOfField(ProtoParser.OneOfFieldContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterMapFieldDecl(ProtoParser.MapFieldDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitMapFieldDecl(ProtoParser.MapFieldDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterRangeMinimumLit(ProtoParser.RangeMinimumLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitRangeMinimumLit(ProtoParser.RangeMinimumLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterRangeMaximumLit(ProtoParser.RangeMaximumLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitRangeMaximumLit(ProtoParser.RangeMaximumLitContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterRangeMaximumMax(ProtoParser.RangeMaximumMaxContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitRangeMaximumMax(ProtoParser.RangeMaximumMaxContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterRangeMaximum(ProtoParser.RangeMaximumContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitRangeMaximum(ProtoParser.RangeMaximumContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterRangeDecl(ProtoParser.RangeDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitRangeDecl(ProtoParser.RangeDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterRangesDecl(ProtoParser.RangesDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitRangesDecl(ProtoParser.RangesDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterExtensionsDecl(ProtoParser.ExtensionsDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitExtensionsDecl(ProtoParser.ExtensionsDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterRangesReservedDecl(ProtoParser.RangesReservedDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitRangesReservedDecl(ProtoParser.RangesReservedDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterFieldName(ProtoParser.FieldNameContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitFieldName(ProtoParser.FieldNameContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterFieldNames(ProtoParser.FieldNamesContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitFieldNames(ProtoParser.FieldNamesContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterFieldNamesReservedDecl(ProtoParser.FieldNamesReservedDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitFieldNamesReservedDecl(ProtoParser.FieldNamesReservedDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterReservedDecl(ProtoParser.ReservedDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitReservedDecl(ProtoParser.ReservedDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterEnumDecl(ProtoParser.EnumDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitEnumDecl(ProtoParser.EnumDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterEnumBody(ProtoParser.EnumBodyContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitEnumBody(ProtoParser.EnumBodyContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterEnumFieldOrdinal(ProtoParser.EnumFieldOrdinalContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitEnumFieldOrdinal(ProtoParser.EnumFieldOrdinalContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterEnumFieldDecl(ProtoParser.EnumFieldDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitEnumFieldDecl(ProtoParser.EnumFieldDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterEnumValueOptions(ProtoParser.EnumValueOptionsContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitEnumValueOptions(ProtoParser.EnumValueOptionsContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterEnumValueOption(ProtoParser.EnumValueOptionContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitEnumValueOption(ProtoParser.EnumValueOptionContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterMessageDecl(ProtoParser.MessageDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitMessageDecl(ProtoParser.MessageDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterMessageBody(ProtoParser.MessageBodyContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitMessageBody(ProtoParser.MessageBodyContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterExtendDecl(ProtoParser.ExtendDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitExtendDecl(ProtoParser.ExtendDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterProtoDecl(ProtoParser.ProtoDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitProtoDecl(ProtoParser.ProtoDeclContext context)
        {
        }

        /// <inheritdoc />
        public virtual void EnterTopLevelDef(ProtoParser.TopLevelDefContext context)
        {
        }

        /// <inheritdoc />
        public virtual void ExitTopLevelDef(ProtoParser.TopLevelDefContext context)
        {
        }
    }
}
