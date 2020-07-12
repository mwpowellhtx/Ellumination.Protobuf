// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <inheritdoc cref="IParentItem"/>
    public interface IReservedStatement : IDescriptor, IParentItem
    {
    }

    /// <inheritdoc cref="IReservedStatement"/>
    /// <typeparam name="T"></typeparam>
    public interface IReservedStatement<T> : IReservedStatement, IHasBody<T>
        where T : IHasParent<IReservedStatement>
    {
    }
}
