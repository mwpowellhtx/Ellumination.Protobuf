using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Antlr4.Runtime;
    using DescriptorTuple = Tuple<Type, object>;

    /// <summary>
    /// <see cref="ProtoDescriptorListenerBase.Descriptors"/> Stack Context.
    /// </summary>
    /// <inheritdoc />
    public abstract class DescriptorStackContext : IDisposable
    {
        /// <summary>
        /// Callback provides in order to Try
        /// <see cref="ProtoDescriptorListenerBase.Descriptors"/> Reduction.
        /// </summary>
        /// <returns></returns>
        public delegate bool TryReduceDescriptorStackCallback();

        /// <summary>
        /// Gets or Sets the Try Callbacks.
        /// </summary>
        protected IEnumerable<TryReduceDescriptorStackCallback> TryCallbacks { get; set; }

        private IList<Tuple<Type, object>> PrivateDescriptors { get; }

        private int StartCount { get; }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="descriptors"></param>
        protected DescriptorStackContext(IList<Tuple<Type, object>> descriptors)
        {
            PrivateDescriptors = descriptors;
            StartCount = descriptors.Count;
        }

        /// <summary>
        /// Creates a new Context given the arguments.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="context"></param>
        /// <param name="descriptors"></param>
        /// <param name="callbacks"></param>
        /// <returns></returns>
        public static DescriptorStackContext<TContext> CreateContext<TContext>(TContext context
            , IList<Tuple<Type, object>> descriptors, params TryReduceDescriptorStackCallback[] callbacks)
            where TContext : RuleContext
            => new DescriptorStackContext<TContext>(context, descriptors, callbacks);

        /// <summary>
        /// Disposes the Object.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (!TryCallbacks.Any(tryCallback => tryCallback()))
            {
                throw new InvalidOperationException(
                    $"Descriptor Reduction did not occur: TryCallbacks.Count = {TryCallbacks.Count()}."
                );
            }

            if (PrivateDescriptors.Count == StartCount)
            {
                throw new InvalidOperationException(
                    $"Did not Reduce an item from the Descriptors stack: Count = {PrivateDescriptors.Count}"
                );
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
        }
    }

    /// <inheritdoc />
    /// <typeparam name="TContext"></typeparam>
    public class DescriptorStackContext<TContext> : DescriptorStackContext
        where TContext : RuleContext
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private TContext Context { get; }

        internal DescriptorStackContext(TContext context, IList<DescriptorTuple> descriptors
            , params TryReduceDescriptorStackCallback[] tryCallbacks)
            : base(descriptors)
        {
            Context = context;
            TryCallbacks = tryCallbacks;
        }
    }
}
