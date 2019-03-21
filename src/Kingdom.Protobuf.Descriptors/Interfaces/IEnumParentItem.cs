// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="IParentItem" />
    public interface IEnumParentItem
        : IParentItem
            , IHasParent<IParentItem>
    {
    }
}
