// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="ICanRenderString" />
    public interface IOneOfBodyItem
        : ICanRenderString
            , IHasParent<IOneOfStatement>
    {
    }
}
