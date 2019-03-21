// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc />
    public interface IDescriptor : ICanRenderString
    {
    }

    /// <inheritdoc cref="IDescriptor"/>
    /// <typeparam name="T"></typeparam>
    public interface IDescriptor<T>
        : IDescriptor
            , IHasName<T>
        where T : class
    {
    }
}
