// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHasIdentifierPath<T>
        where T : class, IIdentifierPath
    {
        /// <summary>
        /// Gets or Sets the Path.
        /// </summary>
        T Path { get; set; }
    }

    /// <inheritdoc cref="IHasIdentifierPath{T}"/>
    public interface IHasIdentifierPath : IHasIdentifierPath<IdentifierPath>
    {
    }
}
