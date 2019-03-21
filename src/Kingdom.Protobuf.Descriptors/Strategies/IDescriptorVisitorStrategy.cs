// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <summary>
    /// Represents a <typeparamref name="T"/> <see cref="IDescriptor"/> Visitor Strategy.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDescriptorVisitorStrategy<in T>
        where T : IDescriptor
    {
        /// <summary>
        /// Visits the <paramref name="descriptor"/>.
        /// </summary>
        /// <param name="descriptor"></param>
        void Visit(T descriptor);
    }
}
