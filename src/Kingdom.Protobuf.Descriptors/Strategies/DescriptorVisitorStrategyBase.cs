// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc />
    public abstract class DescriptorVisitorStrategyBase<T> : DescriptorStrategyBase<T>
        where T : class, IDescriptor
    {
        /// <summary>
        /// Gets the Descriptor.
        /// </summary>
        protected virtual T Descriptor { get; }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="descriptor"></param>
        protected DescriptorVisitorStrategyBase(T descriptor)
        {
            Descriptor = descriptor;
        }
    }
}
