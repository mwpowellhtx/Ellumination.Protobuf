// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="IParentItem"/>
    public interface IExtendStatement
        : ICanRenderString
            , IParentItem
            , IHasBody<IExtendBodyItem>
    {
    }
}
