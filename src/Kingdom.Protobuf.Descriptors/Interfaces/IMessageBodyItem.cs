// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="ICanRenderString" />
    public interface IMessageBodyItem
        : ICanRenderString
            , IHasParent<IMessageBodyParent>
    {
    }
}
