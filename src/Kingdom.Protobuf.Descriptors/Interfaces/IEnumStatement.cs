// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="IParentItem"/>
    public interface IEnumStatement
        : ICanRenderString
            , IParentItem
            , IHasBody<IEnumBodyItem>
    {
    }
}
