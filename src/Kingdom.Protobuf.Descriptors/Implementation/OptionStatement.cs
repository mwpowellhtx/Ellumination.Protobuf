// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static Characters;
    using static WhiteSpaceAndCommentOption;

    /// <inheritdoc cref="DescriptorBase"/>
    public class OptionStatement
        : OptionDescriptorBase
            , IOptionStatement
            , IProtoBodyItem
            , IMessageBodyItem
            , IEnumBodyItem
    {
        IProto IHasParent<IProto>.Parent
        {
            get => Parent as IProto;
            set => Parent = value;
        }

        IMessageBodyParent IHasParent<IMessageBodyParent>.Parent
        {
            get => Parent as IMessageBodyParent;
            set => Parent = value;
        }

        IEnumStatement IHasParent<IEnumStatement>.Parent
        {
            get => Parent as IEnumStatement;
            set => Parent = value;
        }

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
        {
            const string option = nameof(option);

            string GetComments(params WhiteSpaceAndCommentOption[] masks)
                => options.WhiteSpaceAndCommentRendering.RenderMaskedComments(masks);

            return
                $" {GetComments(MultiLineComment)}"
                + $" {option}"
                + $" {base.ToDescriptorString(options)}"
                + $" {SemiColon}{GetComments(MultiLineComment, SingleLineComment)}"
                ;
        }
    }
}
