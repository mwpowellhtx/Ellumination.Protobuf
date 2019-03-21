// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="IParentItem"/>
    public interface IOneOfStatement
        : ICanRenderString
            , IParentItem
            , IHasName<Identifier>
            , IHasBody<IOneOfBodyItem>
    {
    }
}
