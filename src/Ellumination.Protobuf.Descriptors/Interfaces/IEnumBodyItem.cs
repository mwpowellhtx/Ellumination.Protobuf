// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <inheritdoc cref="ICanRenderString" />
    public interface IEnumBodyItem
        : ICanRenderString
            , IHasParent<IEnumStatement>
    {
    }
}
