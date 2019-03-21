// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="ICanRenderString" />
    public interface IEnumBodyItem
        : ICanRenderString
            , IHasParent<IEnumStatement>
    {
    }
}
