// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <inheritdoc cref="ICanRenderString" />
    public interface IExtendBodyItem
        : ICanRenderString
            , IHasParent<IExtendStatement>
    {
    }
}
