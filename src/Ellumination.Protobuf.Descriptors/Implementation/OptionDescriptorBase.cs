// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Collections.Variants;
    using static Characters;
    using static WhiteSpaceAndCommentOption;

    /// <summary>
    /// Serves as the Base Class for <see cref="OptionStatement"/>, <see cref="FieldOption"/>,
    /// and <see cref="EnumValueOption"/>.
    /// </summary>
    /// <inheritdoc cref="DescriptorBase{T}" />
    public abstract class OptionDescriptorBase
        : DescriptorBase<OptionIdentifierPath>
            , IOption
    {
        /// <inheritdoc />
        protected OptionDescriptorBase()
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            : base(new OptionIdentifierPath { })
        {
        }

        /// <inheritdoc />
        public IVariant Value { get; set; }

        /// <summary>
        /// Returns whether <paramref name="a"/> Equals <paramref name="b"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected static bool Equals(OptionDescriptorBase a, OptionDescriptorBase b)
            => ReferenceEquals(a, b)
               || (a.Name.Equals(b.Name)
                   && a.Value.Equals(b.Value));

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
        {
            string GetComments(params WhiteSpaceAndCommentOption[] masks)
                => options.WhiteSpaceAndCommentRendering.RenderMaskedComments(masks);

            return $"{GetComments(MultiLineComment)}"
                   + $" {Name.ToDescriptorString(options)} {GetComments(MultiLineComment)}"
                   + $" {EqualSign} {GetComments(MultiLineComment)}"
                   + $" {Value.ToDescriptorString(options)} {GetComments(MultiLineComment)}"
                ;
        }
    }
}
