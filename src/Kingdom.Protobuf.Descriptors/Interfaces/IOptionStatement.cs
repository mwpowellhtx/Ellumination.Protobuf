// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="IParentItem"/>
    public interface IOptionStatement
        : ICanRenderString
            , IHasParent<IEnumStatement>
    {
    }
}
