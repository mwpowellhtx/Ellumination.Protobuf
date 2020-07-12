using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Collections.Generic;
    using Collections.Variants;
    using static Characters;
    using static WhiteSpaceAndCommentOption;

    /// <inheritdoc cref="FieldStatementBase" />
    public class OneOfFieldStatement
        : FieldStatementBase<Identifier>
            , IOneOfFieldStatement
            , IOneOfBodyItem
    {
        IOneOfStatement IHasParent<IOneOfStatement>.Parent
        {
            get => Parent as IOneOfStatement;
            set => Parent = value;
        }

        /// <inheritdoc />
        public IVariant FieldType { get; set; }

        private IList<FieldOption> _options = new List<FieldOption>();

        private IList<FieldOption> _bidiOptions;

        /// <inheritdoc />
        public IList<FieldOption> Options
        {
            get => _bidiOptions ?? (_bidiOptions = _options.ToBidirectionalList(
                       onAdded: x => ((IHasParent<IOneOfFieldStatement>) x).Parent = this
                       , onRemoved: x => ((IHasParent<IOneOfFieldStatement>) x).Parent = null)
                   );
            set
            {
                Options?.Clear();
                _options = value ?? GetRange<FieldOption>().ToList();
                _bidiOptions = null;
            }
        }

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
        {
            string GetComments(params WhiteSpaceAndCommentOption[] masks)
                => options.WhiteSpaceAndCommentRendering.RenderMaskedComments(masks);

            return $"{FieldType.ToDescriptorString(options)} {GetComments(MultiLineComment)}"
                   + $" {Name.ToDescriptorString(options)} {GetComments(MultiLineComment)}"
                   + $" {EqualSign} {GetComments(MultiLineComment)}"
                   + $" {Number.RenderLong(options.IntegerRendering)} {GetComments(MultiLineComment)}"
                   + $" {Options.RenderOptions(options)} {GetComments(MultiLineComment)} {SemiColon}"
                ;
        }
    }
}
