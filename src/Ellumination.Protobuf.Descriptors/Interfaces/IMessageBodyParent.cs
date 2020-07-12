// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <inheritdoc cref="IParentItem"/>
    public interface IMessageBodyParent : IDescriptor, IParentItem, IHasBody<IMessageBodyItem>
    {
    }
}
