// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="IParentItem"/>
    public interface IExtensionsStatement
        : ICanRenderString
            , IHasBody<RangeDescriptor>
            , IParentItem
    {
    }
}
