using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Collections.Generic;
    using Collections.Variants;
    using static Characters;
    using static WhiteSpaceAndCommentOption;

    /// <inheritdoc cref="FieldStatementBase{T}" />
    public class MapFieldStatement
        : FieldStatementBase<Identifier>
            , IMapFieldStatement
            , IMessageBodyItem
    {
        IMessageBodyParent IHasParent<IMessageBodyParent>.Parent
        {
            get => Parent as IMessageBodyParent;
            set => Parent = value;
        }

        /// <inheritdoc />
        public KeyType KeyType { get; set; }

        /// <inheritdoc />
        public IVariant ValueType { get; set; }

        private IList<FieldOption> _options = GetRange<FieldOption>().ToList();

        private IList<FieldOption> _bidiOptions;

        /// <inheritdoc />
        public IList<FieldOption> Options
        {

#pragma warning disable IDE0074 // Use compound assignment
            get => _bidiOptions ?? (_bidiOptions = _options.ToBidirectionalList(
                onAdded: x => ((IHasParent<IMapFieldStatement>) x).Parent = this
                , onRemoved: x => ((IHasParent<IMapFieldStatement>) x).Parent = null)
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
            const string map = nameof(map);

            string GetComments(params WhiteSpaceAndCommentOption[] masks)
                => options.WhiteSpaceAndCommentRendering.RenderMaskedComments(masks);

            return $"{GetComments(MultiLineComment)}"
                   + $" {map}{OpenAngleBracket} {GetComments(MultiLineComment)}"
                   + $" {KeyType.ToDescriptorString(options)} {GetComments(MultiLineComment)}"
                   + $" {Comma} {GetComments(MultiLineComment)}"
                   + $" {ValueType.ToDescriptorString(options)} {GetComments(MultiLineComment)}"
                   + $" {CloseAngleBracket} {GetComments(MultiLineComment)} "
                   + $" {Name.ToDescriptorString(options)} {GetComments(MultiLineComment)}"
                   + $" {EqualSign} {GetComments(MultiLineComment)}"
                   + $" {Number.RenderLong(options.IntegerRendering)} {GetComments(MultiLineComment)}"
                   + $" {Options.RenderOptions(options)} {GetComments(MultiLineComment)} {SemiColon}"
                ;
        }
    }
}
