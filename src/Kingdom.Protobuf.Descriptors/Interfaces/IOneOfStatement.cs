// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="IParentItem"/>
    public interface IOneOfStatement
        : IDescriptor
            , IParentItem
            , IHasName<Identifier>
            , IHasBody<IOneOfBodyItem>
    {
    }
}
