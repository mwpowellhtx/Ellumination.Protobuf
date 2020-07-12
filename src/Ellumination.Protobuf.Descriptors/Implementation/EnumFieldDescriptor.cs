using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Collections.Generic;
    using static Characters;
    using static String;
    using static WhiteSpaceAndCommentOption;

    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc cref="DescriptorBase" />
    public class EnumFieldDescriptor
        : DescriptorBase<Identifier>
            , IEnumFieldDescriptor
    {
        /// <inheritdoc />
        public EnumFieldDescriptor()
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            : base(new Identifier { })
        {
        }

        IEnumStatement IHasParent<IEnumStatement>.Parent
        {
            get => Parent as IEnumStatement;
            set => Parent = value;
        }

        /// <summary>
        /// Gets or sets the Ordinal.
        /// </summary>
        public long Ordinal { get; set; }

        private IList<EnumValueOption> _options = GetRange<EnumValueOption>().ToList();

        private IList<EnumValueOption> _bidiOptions;

        /// <inheritdoc />
        public IList<EnumValueOption> Options
        {

#pragma warning disable IDE0074 // Use compound assignment
            get => _bidiOptions ?? (_bidiOptions = _options.ToBidirectionalList(
                onAdded: x => ((IEnumValueOption) x).Parent = this
                , onRemoved: x => ((IEnumValueOption) x).Parent = null)
            );
#pragma warning restore IDE0074 // Use compound assignment

            set
            {
                Options?.Clear();
                _options = (value ?? GetRange<EnumValueOption>()).ToList();
                _bidiOptions = null;
            }
        }

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
        {
            var lineSeparator = WithLineSeparatorCarriageReturnNewLine.RenderLineSeparator();

            // ReSharper disable once ImplicitlyCapturedClosure
            string GetComments(params WhiteSpaceAndCommentOption[] masks)
                => options.WhiteSpaceAndCommentRendering.RenderMaskedComments(masks);

            // TODO: TBD: this one has been refactored, has it not?
            string RenderOptions()
            {
                // This one makes sense to leverage String Join.
                return Options.Any()
                    ? $" {GetComments(MultiLineComment)}"
                      + $" {OpenSquareBracket}{GetComments(MultiLineComment, SingleLineComment)}{lineSeparator}"
                      + $" {Join($"{GetComments(MultiLineComment)} {Comma} {GetComments(MultiLineComment)}", Options.Select(x => x.ToDescriptorString(options)))}"
                      + $" {GetComments(MultiLineComment)}"
                      + $" {CloseSquareBracket}{GetComments(MultiLineComment, SingleLineComment)}{lineSeparator}"
                    : Empty;
            }

            /* Do not jump through any String Join hoops unless we absolutely have to.
             * And, plus, we can leverage Integer Literal Rendering just the same. */

            return $" {GetComments(MultiLineComment)}"
                   + $" {Name.ToDescriptorString(options)} {GetComments(MultiLineComment)}"
                   + $" {EqualSign} {GetComments(MultiLineComment)}"
                   + $" {Ordinal.RenderLong(options.IntegerRendering)} {GetComments(MultiLineComment)}"
                   + $" {RenderOptions()} {GetComments(MultiLineComment)} {SemiColon}"
                ;
        }
    }
}
