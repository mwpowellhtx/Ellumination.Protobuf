// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="ICanRenderString" />
    public interface IProtoBodyItem
        : ICanRenderString
            , IHasParent<IProto>
    {
    }
}
