using System.Collections.Generic;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc />
    public abstract class DescriptorBase : IDescriptor
    {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Returns the Range corresponding to <paramref name="values"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        protected static IEnumerable<T> GetRange<T>(params T[] values)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var x in values)
            {
                yield return x;
            }
        }

        // TODO: TBD: override when required...
        // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
        /// <inheritdoc />
        /// <see cref="ToDescriptorString(IStringRenderingOptions)"/>
        public virtual string ToDescriptorString() => ToDescriptorString(new StringRenderingOptions { });

        /// <inheritdoc />
        public abstract string ToDescriptorString(IStringRenderingOptions options);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <inheritdoc cref="DescriptorBase" />
    public abstract class DescriptorBase<T>
        : DescriptorBase
            , IDescriptor<T>
        where T : class
    {
        /// <inheritdoc cref="DescriptorBase"/>
        public T Name { get; set; }

        /// <inheritdoc />
        protected DescriptorBase()
            : this(default(T))
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="name"></param>
        protected DescriptorBase(T name)
        {
            Name = name;
        }
    }
}
