using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using Antlr4.Runtime;
    using Collections;

    /// <summary>
    /// <see cref="ProtoDescriptorListenerBase.Stack"/> Stack Context.
    /// </summary>
    /// <inheritdoc />
    public abstract class DescriptorStackContext : IDisposable
    {
        /// <summary>
        /// Callback provides in order to Try
        /// <see cref="ProtoDescriptorListenerBase.Stack"/> Reduction.
        /// </summary>
        /// <returns></returns>
        public delegate bool TryReduceDescriptorStackCallback();

        /// <summary>
        /// Gets or Sets the Try Callbacks.
        /// </summary>
        protected IEnumerable<TryReduceDescriptorStackCallback> TryCallbacks { get; set; }

        private AbstractSyntaxTreeStack<ProtoDescriptor> PrivateStack { get; }

        private int StartCount { get; }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="stack"></param>
        protected DescriptorStackContext(AbstractSyntaxTreeStack<ProtoDescriptor> stack)
        {
            PrivateStack = stack;
            StartCount = stack.Count;
        }

        /// <summary>
        /// Creates a new Context given the arguments.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="context"></param>
        /// <param name="stack"></param>
        /// <param name="callbacks"></param>
        /// <returns></returns>
        public static DescriptorStackContext<TContext> CreateContext<TContext>(TContext context
            , AbstractSyntaxTreeStack<ProtoDescriptor> stack, params TryReduceDescriptorStackCallback[] callbacks)
            where TContext : RuleContext
            => new DescriptorStackContext<TContext>(context, stack, callbacks);

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

            if (PrivateStack.Count == StartCount)
            {
                throw new InvalidOperationException(
                    $"Did not Reduce an item from the Descriptors stack: Count = {PrivateStack.Count}"
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

        internal DescriptorStackContext(TContext context, AbstractSyntaxTreeStack<ProtoDescriptor> stack
            , params TryReduceDescriptorStackCallback[] tryCallbacks)
            : base(stack)
        {
            Context = context;
            TryCallbacks = tryCallbacks;
        }
    }
}
