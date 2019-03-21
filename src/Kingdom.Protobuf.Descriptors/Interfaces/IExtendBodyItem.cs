// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="ICanRenderString" />
    public interface IExtendBodyItem
        : ICanRenderString
            , IHasParent<IExtendStatement>
    {
    }
}
