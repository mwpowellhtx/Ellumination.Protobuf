using System.Collections.Generic;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <inheritdoc />
    public abstract class DescriptorStrategyBase<T> : IDescriptorVisitorStrategy<T>
        where T : IDescriptor
    {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Returns the Range corresponding to <paramref name="values"/>.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        protected static IEnumerable<TItem> GetRange<TItem>(params TItem[] values)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var x in values)
            {
                yield return x;
            }
        }

        /// <inheritdoc />
        public abstract void Visit(T descriptor);
    }
}