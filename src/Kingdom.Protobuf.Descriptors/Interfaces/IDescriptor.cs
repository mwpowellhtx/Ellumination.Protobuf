// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="ICanRenderString"/>
    public interface IDescriptor : ICanRenderString, IHasParent<IParentItem>
    {
    }

    /// <inheritdoc cref="IDescriptor"/>
    /// <typeparam name="T"></typeparam>
    public interface IDescriptor<T> : IDescriptor, IHasName<T>
        where T : class
    {
    }
}
