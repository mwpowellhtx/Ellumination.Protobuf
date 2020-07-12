// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <inheritdoc cref="IDescriptorVisitor{T, TResult}"/>
    public abstract class DescriptorVisitorWithResultBase<T, TResult>
        : DescriptorVisitorBase<T>
            , IDescriptorVisitor<T, TResult>
        where T : IDescriptor
    {
        /// <inheritdoc />
        public abstract TResult Result { get; }

        /// <inheritdoc />
        public virtual TResult VisitWithResult(T descriptor)
        {
            ResolveVisit(descriptor);
            return Result;
        }

        /// <summary>
        /// Override in order to determine what an appropriate <see cref="Result"/> ought to be
        /// following the <see cref="IDescriptorVisitor{T}.Visit"/>.
        /// </summary>
        /// <param name="descriptor"></param>
        protected virtual void ResolveVisit(T descriptor) => Visit(descriptor);
    }
}
