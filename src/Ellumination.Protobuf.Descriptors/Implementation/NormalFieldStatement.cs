using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Collections.Generic;
    using Collections.Variants;
    using static Characters;
    using static LabelKind;
    using static WhiteSpaceAndCommentOption;

    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc cref="FieldStatementBase" />
    public class NormalFieldStatement
        : FieldStatementBase<Identifier>
            , INormalFieldStatement
            , IMessageBodyItem
            , IExtendBodyItem
    {
        IMessageBodyParent IHasParent<IMessageBodyParent>.Parent
        {
            get => Parent as IMessageBodyParent;
            set => Parent = value;
        }

        IExtendStatement IHasParent<IExtendStatement>.Parent
        {
            get => Parent as IExtendStatement;
            set => Parent = value;
        }

        /// <inheritdoc />
        public LabelKind Label { get; set; } = Optional;

        /// <inheritdoc />
        public IVariant FieldType { get; set; }

        private IList<FieldOption> _options = GetRange<FieldOption>().ToList();

        private IList<FieldOption> _bidiOptions;

        /// <inheritdoc />
        public IList<FieldOption> Options
        {

#pragma warning disable IDE0074 // Use compound assignment
            get => _bidiOptions ?? (_bidiOptions = _options.ToBidirectionalList(
                onAdded: x => ((IHasParent<INormalFieldStatement>) x).Parent = this
                , onRemoved: x => ((IHasParent<INormalFieldStatement>) x).Parent = null)
            );
#pragma warning restore IDE0074 // Use compound assignment

            set
            {
                Options?.Clear();
                _options = (value ?? GetRange<FieldOption>()).ToList();
                _bidiOptions = null;
            }
        }

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
        {
            string GetComments(params WhiteSpaceAndCommentOption[] masks)
                => options.WhiteSpaceAndCommentRendering.RenderMaskedComments(masks);

            return $"{GetComments(MultiLineComment)}"
                   + $" {Label.ToDescriptorString(options)} {GetComments(MultiLineComment)}"
                   + $" {FieldType.ToDescriptorString(options)} {GetComments(MultiLineComment)}"
                   + $" {Name.ToDescriptorString(options)} {GetComments(MultiLineComment)}"
                   + $" {EqualSign} {GetComments(MultiLineComment)}"
                   + $" {Number} {GetComments(MultiLineComment)}"
                   + $" {Options.RenderOptions(options)} {GetComments(MultiLineComment)} {SemiColon}"
                ;
        }
    }
}
