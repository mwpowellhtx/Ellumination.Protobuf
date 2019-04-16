using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static DescriptorStackContext;
    using static FieldNumbers;
    using static RangeDescriptor;
    using static String;
    using EnumBody = List<IEnumBodyItem>;
    using EnumValueOptionList = List<EnumValueOption>;
    using FieldNameList = List<Identifier>;
    using FieldOptionList = List<FieldOption>;
    using RangeList = List<RangeDescriptor>;

    // TODO: TBD: and with a dependency on our Descriptors, i.e. read: Abstract Syntax Tree (AST), then we need to provide a listener/visitor pattern in order to build that out from the parsed lexicon...
    // ReSharper disable once UnusedMember.Global
    /// <inheritdoc />
    public class ProtoDescriptorListener : ProtoDescriptorListenerBase
    {
        /// <summary>
        /// <see cref="double.PositiveInfinity"/>
        /// </summary>
        private const double Infinity = double.PositiveInfinity;

        /// <summary>
        /// <see cref="double.PositiveInfinity"/>
        /// </summary>
        private const double PositiveInfinity = double.PositiveInfinity;

        /// <summary>
        /// <see cref="double.NegativeInfinity"/>
        /// </summary>
        private const double NegativeInfinity = double.NegativeInfinity;

        /// <summary>
        /// <see cref="double.NaN"/>
        /// </summary>
        private const double NaN = double.NaN;

        /// <inheritdoc />
        public override void EnterIdent(ProtoParser.IdentContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new Identifier { });
        }

        /// <inheritdoc />
        public override void ExitIdent(ProtoParser.IdentContext context)
        {
            // ReSharper disable once ImplicitlyCapturedClosure
            void HydrateIdentifier(IHasName<Identifier> x, Identifier y)
            {
                y.Name = context.GetText();
                x.Name = y;
            }

            // ReSharper disable once ImplicitlyCapturedClosure
            void HydrateIdentifierPath(IIdentifierPath x, Identifier y)
            {
                y.Name = context.GetText();
                x.Add(y);
            }

            // TODO: TBD: we may be able to leverage IHasName<string> here...
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref EnumFieldDescriptor x, Identifier y) => HydrateIdentifier(x, y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref EnumStatement x, Identifier y) => HydrateIdentifier(x, y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref NormalFieldStatement x, Identifier y) => HydrateIdentifier(x, y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref MapFieldStatement x, Identifier y) => HydrateIdentifier(x, y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref GroupFieldStatement x, Identifier y) => HydrateIdentifier(x, y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref OneOfFieldStatement x, Identifier y) => HydrateIdentifier(x, y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref OneOfStatement x, Identifier y) => HydrateIdentifier(x, y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref MessageStatement x, Identifier y) => HydrateIdentifier(x, y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref IdentifierPath x, Identifier y) => HydrateIdentifierPath(x, y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref ElementTypeIdentifierPath x, Identifier y) => HydrateIdentifierPath(x, y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref OptionIdentifierPath x, Identifier y) => HydrateIdentifierPath(x, y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref Identifier x, Identifier y) =>
                        {
                            y.Name = context.GetText();
                            x = y;
                        })
                    , () => TryOnExitResolveSynthesizedAttribute((ref string _, string __) => { })
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterFullIdent(ProtoParser.FullIdentContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new IdentifierPath { });
        }

        /// <inheritdoc />
        public override void ExitFullIdent(ProtoParser.FullIdentContext context)
        {
            void TransferIdentifierPath(IIdentifierPath x, IdentifierPath y)
            {
                foreach (var z in y)
                {
                    x.Add(z);
                }
            }

            // TODO: TBD: I expect this would be informed by more than just Package ...
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref PackageStatement x, IdentifierPath y) => x.PackagePath = y
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref ElementTypeIdentifierPath x, IdentifierPath y) => TransferIdentifierPath(x, y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref IdentifierPath x, IdentifierPath y) => TransferIdentifierPath(x, y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref OptionIdentifierPath x, IdentifierPath y) => TransferIdentifierPath(x, y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref Constant<IIdentifierPath> x, IdentifierPath y) => x.Value = y
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterGroupedOptionNamePrefix(ProtoParser.GroupedOptionNamePrefixContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => new OptionIdentifierPath {IsPrefixGrouped = true});
        }

        /// <inheritdoc />
        public override void ExitGroupedOptionNamePrefix(ProtoParser.GroupedOptionNamePrefixContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref OptionIdentifierPath x, OptionIdentifierPath y) =>
                        {
                            // ReSharper disable once InconsistentNaming
                            var y_Count = y.Count;

                            y.SuffixStartIndex = y_Count;
                            x = y;
                        })
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterSingleOptionNamePrefix(ProtoParser.SingleOptionNamePrefixContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new OptionIdentifierPath { });
        }

        /// <inheritdoc />
        public override void ExitSingleOptionNamePrefix(ProtoParser.SingleOptionNamePrefixContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref OptionIdentifierPath x, OptionIdentifierPath y) =>
                        {
                            // ReSharper disable once InconsistentNaming
                            var y_Count = y.Count;

                            // We will set it by Count, but we are really only expecting the Single.
                            y.SuffixStartIndex = y_Count;
                            x = y;
                        })
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterOptionNamePrefix(ProtoParser.OptionNamePrefixContext context)
        {
            OptionIdentifierPath GetDefaultPath() => null;
            OnEnterSynthesizeAttribute(context, ctx => GetDefaultPath());
        }

        /// <inheritdoc />
        public override void ExitOptionNamePrefix(ProtoParser.OptionNamePrefixContext context)
        {
            // Which should work whether this was Fully Qualified or a single Identifier.
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref OptionIdentifierPath x, OptionIdentifierPath y) => x = y)
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterOptionNameSuffix(ProtoParser.OptionNameSuffixContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new IdentifierPath { });
        }

        /// <inheritdoc />
        public override void ExitOptionNameSuffix(ProtoParser.OptionNameSuffixContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // The Suffix Start Index shall have already been set by virtue of the Prefix having been relayed.
                        (ref OptionIdentifierPath x, IdentifierPath y) =>
                        {
                            foreach (var z in y)
                            {
                                x.Add(z);
                            }
                        })
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterOptionName(ProtoParser.OptionNameContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new OptionIdentifierPath { });
        }

        /// <inheritdoc />
        public override void ExitOptionName(ProtoParser.OptionNameContext context)
        {
            // Relay the instance itself on account there may be secondary properties indicated.
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref EnumValueOption x, OptionIdentifierPath y) => x.Name = y
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref OptionStatement x, OptionIdentifierPath y) => x.Name = y
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref FieldOption x, OptionIdentifierPath y) => x.Name = y
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterBooleanFalse(ProtoParser.BooleanFalseContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => false);
        }

        /// <inheritdoc />
        public override void ExitBooleanFalse(ProtoParser.BooleanFalseContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref bool x, bool y) => x = y
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterBooleanTrue(ProtoParser.BooleanTrueContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => true);
        }

        /// <inheritdoc />
        public override void ExitBooleanTrue(ProtoParser.BooleanTrueContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref bool x, bool y) => x = y
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterBooleanLit(ProtoParser.BooleanLitContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => false);
        }

        /// <inheritdoc />
        public override void ExitBooleanLit(ProtoParser.BooleanLitContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref IConstant x, bool y) => x = Constant.Create(y)
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterHexLit(ProtoParser.HexLitContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => Empty);
        }

        /// <inheritdoc />
        public override void ExitHexLit(ProtoParser.HexLitContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref long x, string _) =>
                        {
                            const int intBase = 16;
                            x = context.GetText().Substring(2).ParseLong(intBase);
                        }
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterOctLit(ProtoParser.OctLitContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => Empty);
        }

        /// <inheritdoc />
        public override void ExitOctLit(ProtoParser.OctLitContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref long x, string _) =>
                        {
                            const int intBase = 8;
                            /* When we have a Single character, that is a bit of a special case.
                             * We do expect Zed or Zero, but we can safely assume just Parse the Long. */
                            var s = context.GetText();
                            x = s.Length == 1
                                ? s.ParseLong()
                                : s.Substring(1).ParseLong(intBase);
                        }
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterDecLit(ProtoParser.DecLitContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => Empty);
        }

        /// <inheritdoc />
        public override void ExitDecLit(ProtoParser.DecLitContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref long x, string _) => x = context.GetText().ParseLong()
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterIntLit(ProtoParser.IntLitContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => default(long));
        }

        /// <inheritdoc />
        public override void ExitIntLit(ProtoParser.IntLitContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute((ref Constant<long> x, long y) => x.Value = y)
                    // ReSharper disable once RedundantAssignment
                    , () => TryOnExitResolveSynthesizedAttribute((ref long x, long y) => x = y)
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterFieldNumber(ProtoParser.FieldNumberContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => default(long));
        }

        /// <inheritdoc />
        public override void ExitFieldNumber(ProtoParser.FieldNumberContext context)
        {
            string RenderRange(long min, long max) => CreateRange(min, max).ToRangeNotation();

            ValidateCurrentDescriptorItem((long x) => x.IsValidFieldNumber()
                , x => $"Field number '{x}' invalid expected to"
                       + $" fall within range {RenderRange(MinimumFieldNumber, MaximumFieldNumber)}."
            );

            ValidateCurrentDescriptorItem((long x) => !x.IsReservedByGoogleProtocolBuffers()
                , x => "Field number must not be within the Google Reserved"
                       + $" range {RenderRange(MinimumReservedFieldNumber, MaximumReservedFieldNumber)}."
            );

            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute((ref NormalFieldStatement x, long y) => x.Number = y)
                    , () => TryOnExitResolveSynthesizedAttribute((ref OneOfFieldStatement x, long y) => x.Number = y)
                    , () => TryOnExitResolveSynthesizedAttribute((ref MapFieldStatement x, long y) => x.Number = y)
                    , () => TryOnExitResolveSynthesizedAttribute((ref GroupFieldStatement x, long y) => x.Number = y)
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterSignedIntLit(ProtoParser.SignedIntLitContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => Constant.Create<long>());
        }

        /// <inheritdoc />
        public override void ExitSignedIntLit(ProtoParser.SignedIntLitContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref IConstant x, Constant<long> y) =>
                        {
                            bool IsNegativelySigned(string s) => s.Any() && s[0] == '-';
                            y.Value = IsNegativelySigned(context.GetText()) ? -y.Value : y.Value;
                            x = y;
                        })
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterInf(ProtoParser.InfContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => Infinity);
        }

        /// <inheritdoc />
        public override void ExitInf(ProtoParser.InfContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref double x, double y) => x = y
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterNan(ProtoParser.NanContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => NaN);
        }

        /// <inheritdoc />
        public override void ExitNan(ProtoParser.NanContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref double x, double y) => x = y
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterNumericFloatLit(ProtoParser.NumericFloatLitContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => default(double));
        }

        /// <inheritdoc />
        public override void ExitNumericFloatLit(ProtoParser.NumericFloatLitContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref double x, double y) => x = context.GetText().ParseDouble()
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterFloatLit(ProtoParser.FloatLitContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => default(double));
        }

        /// <inheritdoc />
        public override void ExitFloatLit(ProtoParser.FloatLitContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute((ref Constant<double> x, double y) => x.Value = y)
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterSignedFloatLit(ProtoParser.SignedFloatLitContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            // Here we can know that the Floating Point Literal is destined as a Constant.
            OnEnterSynthesizeAttribute(context, ctx => new Constant<double> { });
        }

        /// <inheritdoc />
        public override void ExitSignedFloatLit(ProtoParser.SignedFloatLitContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref IConstant x, Constant<double> y) =>
                        {
                            bool IsNaN(double z) => double.IsNaN(z);

                            // Remember to comprehend Signage.
                            bool IsSigned(string s, char sign) => s.Any() && s[0] == sign;
                            bool IsContextSigned(char sign) => IsSigned(context.GetText(), sign);
                            bool IsNegativelySigned() => IsContextSigned('-');
                            bool IsPositivelySigned() => IsContextSigned('+');

                            // ReSharper disable once ArrangeRedundantParentheses
                            // Signage is Optional in the Positive Case.
                            bool IsPositiveInfinity(double z)
                                => (IsPositivelySigned() && double.IsPositiveInfinity(z))
                                   || double.IsPositiveInfinity(z);

                            // Signage is Not Optional in the Negative Case.
                            bool IsNegativeInfinity(double z) => IsNegativelySigned() && double.IsPositiveInfinity(z);

                            // Recall we may have any of these cases.
                            if (IsNaN(y.Value))
                            {
                                y.Value = NaN;
                            }
                            else if (IsNegativeInfinity(y.Value))
                            {
                                // Rule Out the Negative Case before considering the Positive Case.
                                y.Value = NegativeInfinity;
                            }
                            else if (IsPositiveInfinity(y.Value))
                            {
                                // Because Signage is Relaxed in the Positive Case, must Rule Out Negative first.
                                y.Value = PositiveInfinity;
                            }
                            else
                            {
                                // Averting, as much, as possible potential for Rounding Errors following Multiplication.
                                y.Value = IsNegativelySigned() ? -y.Value : y.Value;
                            }

                            x = y;
                        })
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterStrLit(ProtoParser.StrLitContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => Empty);
        }

        /// <inheritdoc />
        public override void ExitStrLit(ProtoParser.StrLitContext context)
        {
            // Interpret the String Literal sans the enclosing tick/quotation marks.
            string GetContextTextSans()
            {
                var s = context.GetText();
                return s.Substring(1, s.Length - 2);
            }

            using (CreateContext(context, Stack
                    // ReSharper disable once RedundantAssignment
                    , () => TryOnExitResolveSynthesizedAttribute((ref string x, string y) => x = GetContextTextSans())
                    // ReSharper disable once RedundantAssignment
                    , () => TryOnExitResolveSynthesizedAttribute((ref IConstant x, string y) => x = Constant.Create(GetContextTextSans()))
                    , () => TryOnExitResolveSynthesizedAttribute((ref ImportStatement x, string y) => x.ImportPath = GetContextTextSans())
                    , () => TryOnExitResolveSynthesizedAttribute((ref SyntaxStatement x, string y) => x.Syntax = GetContextTextSans().ToSyntaxKind())
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterFullIdentLit(ProtoParser.FullIdentLitContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new Constant<IIdentifierPath> { });
        }

        /// <inheritdoc />
        public override void ExitFullIdentLit(ProtoParser.FullIdentLitContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref IConstant x, Constant<IIdentifierPath> y) => x = y
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterConstant(ProtoParser.ConstantContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => GetDefault<IConstant>());
        }

        /// <inheritdoc />
        public override void ExitConstant(ProtoParser.ConstantContext context)
        {
            // Should support any of the model Having a Constant.
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref EnumValueOption x, IConstant y) => x.Value = y
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref OptionStatement x, IConstant y) => x.Value = y
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref FieldOption x, IConstant y) => x.Value = y
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterEmptyDecl(ProtoParser.EmptyDeclContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new EmptyStatement { });
        }

        /// <inheritdoc />
        public override void ExitEmptyDecl(ProtoParser.EmptyDeclContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref ProtoDescriptor x, EmptyStatement y) => x.Items.Add(y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref EnumBody x, EmptyStatement y) => x.Add(y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref ExtendStatement x, EmptyStatement y) => x.Items.Add(y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref OneOfStatement x, EmptyStatement y) => x.Items.Add(y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref GroupFieldStatement x, EmptyStatement y) => x.Items.Add(y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref MessageStatement x, EmptyStatement y) => x.Items.Add(y)
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterOptionDecl(ProtoParser.OptionDeclContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new OptionStatement { });
        }

        /// <inheritdoc />
        public override void ExitOptionDecl(ProtoParser.OptionDeclContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref ProtoDescriptor x, OptionStatement y) => x.Items.Add(y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref EnumBody x, OptionStatement y) => x.Add(y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref GroupFieldStatement x, OptionStatement y) => x.Items.Add(y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref MessageStatement x, OptionStatement y) => x.Items.Add(y)
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterPackageDecl(ProtoParser.PackageDeclContext context)
        {
            ValidateFirstDescriptorItem((ProtoDescriptor x) => !x.Items.OfType<PackageStatement>().Any()
                , x => $"Cannot have more than one '{typeof(PackageStatement).FullName}'.");

            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new PackageStatement { });
        }

        /// <inheritdoc />
        public override void ExitPackageDecl(ProtoParser.PackageDeclContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref ProtoDescriptor x, PackageStatement y) => x.Items.Add(y)
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterImportModifier(ProtoParser.ImportModifierContext context)
        {
            ImportModifierKind? GetDefaultImportModifier() => null;
            OnEnterSynthesizeAttribute(context, ctx => GetDefaultImportModifier());
        }

        /// <inheritdoc />
        public override void ExitImportModifier(ProtoParser.ImportModifierContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref ImportStatement x, ImportModifierKind? _)
                            => x.Modifier = context.GetText().ToImportModifier()
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterImportDecl(ProtoParser.ImportDeclContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new ImportStatement { });
        }

        /// <inheritdoc />
        public override void ExitImportDecl(ProtoParser.ImportDeclContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref ProtoDescriptor x, ImportStatement y) => x.Items.Add(y)
                    )
                )
            )
            {
            }
        }

        ///// <inheritdoc />
        //public override void EnterSyntaxValue(ProtoParser.SyntaxValueContext context)
        //{
        //    OnEnterSynthesizeAttribute(context, ctx => SyntaxKind.Proto2);
        //}

        ///// <inheritdoc />
        //public override void ExitSyntaxValue(ProtoParser.SyntaxValueContext context)
        //{
        //    using (CreateContext(context, Stack
        //            , () => TryOnExitResolveSynthesizedAttribute(
        //                (ref SyntaxStatement x, SyntaxKind y) => x.Syntax = y
        //            )
        //        )
        //    )
        //    {
        //    }
        //}

        /// <inheritdoc />
        public override void EnterSyntaxDecl(ProtoParser.SyntaxDeclContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new SyntaxStatement { });
        }

        /// <inheritdoc />
        public override void ExitSyntaxDecl(ProtoParser.SyntaxDeclContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref ProtoDescriptor x, SyntaxStatement y) => x.Syntax = y
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterEnumValueOption(ProtoParser.EnumValueOptionContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new EnumValueOption { });
        }

        /// <inheritdoc />
        public override void ExitEnumValueOption(ProtoParser.EnumValueOptionContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref EnumValueOptionList x, EnumValueOption y) => x.Add(y)
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterEnumValueOptions(ProtoParser.EnumValueOptionsContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new EnumValueOptionList { });
        }

        /// <inheritdoc />
        public override void ExitEnumValueOptions(ProtoParser.EnumValueOptionsContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref EnumFieldDescriptor x, EnumValueOptionList y) => x.Options = y
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterEnumFieldOrdinal(ProtoParser.EnumFieldOrdinalContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => default(long));
        }

        /// <inheritdoc />
        public override void ExitEnumFieldOrdinal(ProtoParser.EnumFieldOrdinalContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute((ref EnumFieldDescriptor x, long y) => x.Ordinal = y)
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterEnumFieldDecl(ProtoParser.EnumFieldDeclContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new EnumFieldDescriptor { });
        }

        /// <inheritdoc />
        public override void ExitEnumFieldDecl(ProtoParser.EnumFieldDeclContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref EnumBody x, EnumFieldDescriptor y) => x.Add(y)
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterEnumBody(ProtoParser.EnumBodyContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new EnumBody { });
        }

        /// <inheritdoc />
        public override void ExitEnumBody(ProtoParser.EnumBodyContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref EnumStatement x, EnumBody y) => x.Items = y
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterEnumDecl(ProtoParser.EnumDeclContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new EnumStatement { });
        }

        /// <inheritdoc />
        public override void ExitEnumDecl(ProtoParser.EnumDeclContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref ITopLevelDefinition x, EnumStatement y) => x = y
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref MessageStatement x, EnumStatement y) => x.Items.Add(y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref GroupFieldStatement x, EnumStatement y) => x.Items.Add(y)
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterFieldOption(ProtoParser.FieldOptionContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new FieldOption { });
        }

        /// <inheritdoc />
        public override void ExitFieldOption(ProtoParser.FieldOptionContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref FieldOptionList x, FieldOption y) => x.Add(y)
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterFieldOptions(ProtoParser.FieldOptionsContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new FieldOptionList { });
        }

        /// <inheritdoc />
        public override void ExitFieldOptions(ProtoParser.FieldOptionsContext context)
        {
            // Should handle any case involving Having Field Option Options.
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref NormalFieldStatement x, FieldOptionList y) => x.Options = y
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref OneOfFieldStatement x, FieldOptionList y) => x.Options = y
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref MapFieldStatement x, FieldOptionList y) => x.Options = y
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterLabel(ProtoParser.LabelContext context)
        {
            LabelKind GetDefaultLabel() => LabelKind.Required;
            OnEnterSynthesizeAttribute(context, ctx => GetDefaultLabel());
        }

        /// <inheritdoc />
        public override void ExitLabel(ProtoParser.LabelContext context)
        {
            LabelKind GetParsedLabel() => context.GetText().ParseLabelKind();

            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref NormalFieldStatement x, LabelKind _) => x.Label = GetParsedLabel()
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref GroupFieldStatement x, LabelKind _) => x.Label = GetParsedLabel()
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterKeyType(ProtoParser.KeyTypeContext context)
        {
            KeyType GetKeyPlaceholder() => KeyType.Int32;
            OnEnterSynthesizeAttribute(context, ctx => GetKeyPlaceholder());
        }

        /// <inheritdoc />
        public override void ExitKeyType(ProtoParser.KeyTypeContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref MapFieldStatement x, KeyType _) => x.KeyType = context.GetText().ParseKeyType()
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterProtoType(ProtoParser.ProtoTypeContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => Variant.Create<ProtoType>());
        }

        /// <inheritdoc />
        public override void ExitProtoType(ProtoParser.ProtoTypeContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref IVariant x, Variant<ProtoType> y) =>
                        {
                            y.Value = context.GetText().ParseProtoType();
                            x = y;
                        })
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterType(ProtoParser.TypeContext context)
        {
            IVariant GetVariantPlaceholder() => Variant.Create<ProtoType>();
            OnEnterSynthesizeAttribute(context, ctx => GetVariantPlaceholder());
        }

        /// <inheritdoc />
        public override void ExitType(ProtoParser.TypeContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref NormalFieldStatement x, IVariant y) => x.FieldType = y
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref OneOfFieldStatement x, IVariant y) => x.FieldType = y
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref MapFieldStatement x, IVariant y) => x.ValueType = y
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterFieldDecl(ProtoParser.FieldDeclContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new NormalFieldStatement { });
        }

        /// <inheritdoc />
        public override void ExitFieldDecl(ProtoParser.FieldDeclContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref ExtendStatement x, NormalFieldStatement y) => x.Items.Add(y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref GroupFieldStatement x, NormalFieldStatement y) => x.Items.Add(y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref MessageStatement x, NormalFieldStatement y) => x.Items.Add(y)
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterGroupName(ProtoParser.GroupNameContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => Empty);
        }

        /// <summary>
        /// We can leverage the same <see cref="RangeDescriptor"/> domain model here for Listener
        /// Synthesis Validation purposes.
        /// </summary>
        private RangeDescriptor AyeZedRange { get; } = new RangeDescriptor {Minimum = 'A', Maximum = 'Z'};

        /// <inheritdoc />
        public override void ExitGroupName(ProtoParser.GroupNameContext context)
        {
            var s = context.GetText();

            ValidateCurrentDescriptorItem((string _) => s.Any() && AyeZedRange.Contains(s.First())
                // ReSharper disable once ImplicitlyCapturedClosure
                , _ => $"Group Name '{s}' must begin with a Capital Letter.");

            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref GroupFieldStatement x, string _) => x.Name = s
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterGroupDecl(ProtoParser.GroupDeclContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            // The Group Field Statement Message Body will be much more fleshed out when this is all done.
            OnEnterSynthesizeAttribute(context, ctx => new GroupFieldStatement { });
        }

        /// <inheritdoc />
        public override void ExitGroupDecl(ProtoParser.GroupDeclContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref ExtendStatement x, GroupFieldStatement y) => x.Items.Add(y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref GroupFieldStatement x, GroupFieldStatement y) => x.Items.Add(y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref MessageStatement x, GroupFieldStatement y) => x.Items.Add(y)
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterElementTypeGlobalScope(ProtoParser.ElementTypeGlobalScopeContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => true);
        }

        /// <inheritdoc />
        public override void ExitElementTypeGlobalScope(ProtoParser.ElementTypeGlobalScopeContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref ElementTypeIdentifierPath x, bool y) => x.IsGlobalScope = y
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterElementType(ProtoParser.ElementTypeContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new ElementTypeIdentifierPath { });
        }

        /// <inheritdoc />
        public override void ExitElementType(ProtoParser.ElementTypeContext context)
        {
            using (CreateContext(context, Stack
                    /* Variant here most commonly used for Type, but we will give the benefit of the doubt here,
                     * especially if we decide to merge Constant and Variant concerns under one umbrella. */
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref ExtendStatement x, ElementTypeIdentifierPath y) => x.MessageType = y
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref IVariant x, ElementTypeIdentifierPath y) => x = Variant.Create(y)
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterExtendDecl(ProtoParser.ExtendDeclContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new ExtendStatement { });
        }

        /// <inheritdoc />
        public override void ExitExtendDecl(ProtoParser.ExtendDeclContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref ITopLevelDefinition x, ExtendStatement y) => x = y
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref MessageStatement x, ExtendStatement y) => x.Items.Add(y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref GroupFieldStatement x, ExtendStatement y) => x.Items.Add(y)
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterRangeMinimumLit(ProtoParser.RangeMinimumLitContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => default(long));
        }

        /// <inheritdoc />
        public override void ExitRangeMinimumLit(ProtoParser.RangeMinimumLitContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref RangeDescriptor x, long y) => x.Minimum = y
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterRangeMaximumLit(ProtoParser.RangeMaximumLitContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => default(long));
        }

        /// <inheritdoc />
        public override void ExitRangeMaximumLit(ProtoParser.RangeMaximumLitContext context)
        {
            // TODO: TBD: consider whether parser, or at worst (best?) AST listener synthesis, can validate the basic value for range validity
            // TODO: TBD: i.e. throw when out of range for min/max, falling in reserved range, etc
            // TODO: TBD: stopping short of message/linkage validation...
            // TODO: TBD: rinse and repeat for *BOTH* Minimum and Maximum values...
            // TODO: TBD: which whose test cases should break for invalid ranges...
            // TODO: TBD: and for which will need to do a bit of shuffling for valid/invalid range test cases...
            // TODO: TBD: similar sort of validation should happen for any field number? or maybe we just route that validation through a single rule..
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref long? x, long y) => x = y
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterRangeMaximumMax(ProtoParser.RangeMaximumMaxContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => MaximumFieldNumber);
        }

        /// <inheritdoc />
        public override void ExitRangeMaximumMax(ProtoParser.RangeMaximumMaxContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref long? x, long y) => x = y
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterRangeMaximum(ProtoParser.RangeMaximumContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            // Seems like a legit enough default Maximum value.
            OnEnterSynthesizeAttribute(context, ctx => new long? { });
        }

        /// <inheritdoc />
        public override void ExitRangeMaximum(ProtoParser.RangeMaximumContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref RangeDescriptor x, long? y) => x.Maximum = y
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterRangeDecl(ProtoParser.RangeDeclContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new RangeDescriptor { });
        }

        /// <inheritdoc />
        public override void ExitRangeDecl(ProtoParser.RangeDeclContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref RangeList x, RangeDescriptor y) => x.Add(y)
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterRangesDecl(ProtoParser.RangesDeclContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new RangeList { });
        }

        /// <inheritdoc />
        public override void ExitRangesDecl(ProtoParser.RangesDeclContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref ExtensionsStatement x, RangeList y) => x.Items = y
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref RangesReservedStatement x, RangeList y) => x.Items = y
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterExtensionsDecl(ProtoParser.ExtensionsDeclContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new ExtensionsStatement { });
        }

        /// <inheritdoc />
        public override void ExitExtensionsDecl(ProtoParser.ExtensionsDeclContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref GroupFieldStatement x, ExtensionsStatement y) => x.Items.Add(y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref MessageStatement x, ExtensionsStatement y) => x.Items.Add(y)
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterFieldName(ProtoParser.FieldNameContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new Identifier { });
        }

        /// <inheritdoc />
        public override void ExitFieldName(ProtoParser.FieldNameContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref FieldNameList x, Identifier y) => x.Add(y)
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterFieldNames(ProtoParser.FieldNamesContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new FieldNameList { });
        }

        /// <inheritdoc />
        public override void ExitFieldNames(ProtoParser.FieldNamesContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref FieldNamesReservedStatement statement, FieldNameList x) => statement.Items = x
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterFieldNamesReservedDecl(ProtoParser.FieldNamesReservedDeclContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new FieldNamesReservedStatement { });
        }

        /// <inheritdoc />
        public override void ExitFieldNamesReservedDecl(ProtoParser.FieldNamesReservedDeclContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref ReservedStatement x, FieldNamesReservedStatement y) => x = y
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterRangesReservedDecl(ProtoParser.RangesReservedDeclContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new RangesReservedStatement { });
        }

        /// <inheritdoc />
        public override void ExitRangesReservedDecl(ProtoParser.RangesReservedDeclContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref ReservedStatement x, RangesReservedStatement y) => x = y
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterReservedDecl(ProtoParser.ReservedDeclContext context)
        {
            OnEnterSynthesizeAttribute(context, ctx => GetDefault<ReservedStatement>());
        }

        /// <inheritdoc />
        public override void ExitReservedDecl(ProtoParser.ReservedDeclContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref GroupFieldStatement x, ReservedStatement y) => x.Items.Add(y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref MessageStatement x, ReservedStatement y) => x.Items.Add(y)
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterOneOfField(ProtoParser.OneOfFieldContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new OneOfFieldStatement { });
        }

        /// <inheritdoc />
        public override void ExitOneOfField(ProtoParser.OneOfFieldContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref OneOfStatement x, OneOfFieldStatement y) => x.Items.Add(y)
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterOneOfDecl(ProtoParser.OneOfDeclContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new OneOfStatement { });
        }

        /// <inheritdoc />
        public override void ExitOneOfDecl(ProtoParser.OneOfDeclContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref MessageStatement x, OneOfStatement y) => x.Items.Add(y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref GroupFieldStatement x, OneOfStatement y) => x.Items.Add(y)
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterMapFieldDecl(ProtoParser.MapFieldDeclContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new MapFieldStatement { });
        }

        /// <inheritdoc />
        public override void ExitMapFieldDecl(ProtoParser.MapFieldDeclContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref GroupFieldStatement x, MapFieldStatement y) => x.Items.Add(y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref MessageStatement x, MapFieldStatement y) => x.Items.Add(y)
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterMessageDecl(ProtoParser.MessageDeclContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new MessageStatement { });
        }

        /// <inheritdoc />
        public override void ExitMessageDecl(ProtoParser.MessageDeclContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        // ReSharper disable once RedundantAssignment
                        (ref ITopLevelDefinition x, MessageStatement y) => x = y
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref GroupFieldStatement x, MessageStatement y) => x.Items.Add(y)
                    )
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref MessageStatement x, MessageStatement y) => x.Items.Add(y)
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterTopLevelDef(ProtoParser.TopLevelDefContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => GetDefault<ITopLevelDefinition>());
        }

        /// <inheritdoc />
        public override void ExitTopLevelDef(ProtoParser.TopLevelDefContext context)
        {
            using (CreateContext(context, Stack
                    , () => TryOnExitResolveSynthesizedAttribute(
                        (ref ProtoDescriptor x, ITopLevelDefinition y) => x.Items.Add(y)
                    )
                )
            )
            {
            }
        }

        /// <inheritdoc />
        public override void EnterProtoDecl(ProtoParser.ProtoDeclContext context)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            OnEnterSynthesizeAttribute(context, ctx => new ProtoDescriptor { });
        }

        /// <inheritdoc />
        public override void ExitProtoDecl(ProtoParser.ProtoDeclContext context)
        {
            ActualProto = Stack.RootInstance;
            Stack.Clear();
        }
    }
}
