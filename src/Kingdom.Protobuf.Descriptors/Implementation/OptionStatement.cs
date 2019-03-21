// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
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
            return $"{option} {base.ToDescriptorString(options)};";
        }
    }
}
