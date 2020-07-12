// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <inheritdoc cref="ICanRenderString" />
    public interface IProtoBodyItem
        : ICanRenderString
            , IHasParent<IProto>
    {
    }
}
