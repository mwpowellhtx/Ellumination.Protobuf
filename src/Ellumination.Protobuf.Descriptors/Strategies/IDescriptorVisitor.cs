// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <summary>
    /// Represents a <typeparamref name="T"/> <see cref="IDescriptor"/> Visitor pattern.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDescriptorVisitor<in T>
        where T : IDescriptor
    {
        /// <summary>
        /// Visits the <paramref name="descriptor"/>.
        /// </summary>
        /// <param name="descriptor"></param>
        void Visit(T descriptor);
    }

    /// <inheritdoc />
    /// <summary>
    /// Represents an <see cref="T:Kingdom.OrTools.Sat.IDescriptorVisitor`1" /> with a <typeparamref name="TResult" />.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IDescriptorVisitor<in T, out TResult> : IDescriptorVisitor<T>
        where T : IDescriptor
    {
        /// <summary>
        /// Gets the <typeparamref name="TResult"/> after visitation.
        /// </summary>
        TResult Result { get; }

        /// <summary>
        /// Visits the <paramref name="descriptor"/> With the <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        TResult VisitWithResult(T descriptor);
    }
}
