// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <inheritdoc cref="IParentItem"/>
    public interface IExtendStatement : IDescriptor, IParentItem, IHasBody<IExtendBodyItem>
    {
    }
}
