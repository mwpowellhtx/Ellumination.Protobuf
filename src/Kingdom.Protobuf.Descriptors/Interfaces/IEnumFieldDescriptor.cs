// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="IParentItem"/>
    public interface IEnumFieldDescriptor
        : IParentItem
            , IDescriptor
            , IEnumBodyItem
            , IHasOptions<EnumValueOption>
    {
    }
}
