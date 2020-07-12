// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <inheritdoc cref="ICanRenderString" />
    public interface IOneOfBodyItem
        : ICanRenderString
            , IHasParent<IOneOfStatement>
    {
    }
}
