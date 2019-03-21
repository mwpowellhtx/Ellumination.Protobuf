// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="IParentItem"/>
    public interface IMessageBodyParent
        : ICanRenderString
            , IParentItem
            , IHasBody<IMessageBodyItem>
    {
    }
}
