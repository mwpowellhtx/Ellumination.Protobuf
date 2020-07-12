// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
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
