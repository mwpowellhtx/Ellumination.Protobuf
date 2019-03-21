// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="IParentItem"/>
    public interface IProto
        : IParentItem
            , IHasBody<IProtoBodyItem>
    {
    }
}
