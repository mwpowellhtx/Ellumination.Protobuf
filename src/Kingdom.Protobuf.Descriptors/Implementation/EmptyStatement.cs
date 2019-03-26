// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static Characters;
    using static WhiteSpaceAndCommentOption;

    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc cref="DescriptorBase" />
    public class EmptyStatement
        : DescriptorBase
            , IProtoBodyItem
            , IOneOfBodyItem
            , IEnumBodyItem
            , IMessageBodyItem
            , IExtendBodyItem
    {
        // TODO: TBD: may refactor to base classes...
        private IParentItem _parent;

        IProto IHasParent<IProto>.Parent
        {
            get => _parent as IProto;
            set => _parent = value;
        }

        IOneOfStatement IHasParent<IOneOfStatement>.Parent
        {
            get => _parent as IOneOfStatement;
            set => _parent = value;
        }

        IEnumStatement IHasParent<IEnumStatement>.Parent
        {
            get => _parent as IEnumStatement;
            set => _parent = value;
        }

        IMessageBodyParent IHasParent<IMessageBodyParent>.Parent
        {
            get => _parent as IMessageBodyParent;
            set => _parent = value;
        }

        IExtendStatement IHasParent<IExtendStatement>.Parent
        {
            get => _parent as IExtendStatement;
            set => _parent = value;
        }

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
        {
            string GetComments(params WhiteSpaceAndCommentOption[] masks)
                => options.WhiteSpaceAndCommentRendering.RenderMaskedComments(masks);

            return $"{GetComments(MultiLineComment)}"
                   + $" {SemiColon}"
                   + $"{GetComments(MultiLineComment, SingleLineComment)}"
                ;
        }
    }
}
