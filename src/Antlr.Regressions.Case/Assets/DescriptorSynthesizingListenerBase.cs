using System;
using System.Collections.Generic;
using System.Linq;

namespace Kingdom.Antlr.Regressions.Case
{
    using Antlr4.Runtime;
    using DescriptorTuple = Tuple<Type, object>;

    public abstract class DescriptorSynthesizingListenerBase
    {
        /// <summary>
        /// Gets the Actual <see cref="GroupDescriptor"/> instance.
        /// </summary>
        public GroupDescriptor RootDescriptor { get; protected set; }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Returns the Default <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected static T GetDefault<T>() => default(T);

        private IList<DescriptorTuple> _descriptors;

        /// <summary>
        /// Gets the Descriptors.
        /// </summary>
        protected internal IList<DescriptorTuple> Descriptors
            => _descriptors ?? (_descriptors
                   // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
                   = new List<DescriptorTuple> { }
               );

        /// <summary>
        /// Callback provided in order to Synthesize the <typeparamref name="TCurrent"/> value.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TCurrent"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        protected delegate TCurrent SynthesizeCallback<in TContext, out TCurrent>(TContext context)
            where TContext : RuleContext;

        private static DescriptorTuple CreateDescriptor(Type type, object value) => new DescriptorTuple(type, value);

        /// <summary>
        /// Callers must specify the Synthesized Type as strongly as possible.
        /// This becomes critical when the Stack is poised to unwind properly.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <typeparam name="TCurrent"></typeparam>
        /// <param name="context"></param>
        /// <param name="synthesize"></param>
        protected void OnEnterSynthesizeAttribute<TContext, TCurrent>(TContext context,
            SynthesizeCallback<TContext, TCurrent> synthesize)
            where TContext : RuleContext
            => Descriptors.Add(CreateDescriptor(typeof(TCurrent), synthesize(context)));

        /// <summary>
        /// Callback provided in order to Collapse the <see cref="Descriptors"/> to the Previous
        /// state.
        /// </summary>
        /// <typeparam name="TPrevious"></typeparam>
        /// <typeparam name="TCurrent"></typeparam>
        /// <param name="previous"></param>
        /// <param name="current"></param>
        protected delegate void CollapseToPreviousCallback<TPrevious, in TCurrent>(ref TPrevious previous
            , TCurrent current);

        /// <summary>
        /// The tricky part about unwinding the Stack properly is that the Caller must know the
        /// precise strongly typed context in which the unwind is to take place. In and of itself,
        /// this does not seem like a big deal, but it has the potential to be highly ambiguous in
        /// situations involving statements like Message, Enum, and Extend, which can land as
        /// either Top Level Definitions interface or rolled up in terms of a Message Body list
        /// constituent. The trade off is subtle, but it seems we cannot leverage CSharp language
        /// nuances such as the Is keyword, As, and so forth, when implying the desired type.
        /// Instead, the caller must know precisely the Type in order for the Stack to unwind
        /// correctly.
        /// </summary>
        /// <typeparam name="TPrevious"></typeparam>
        /// <typeparam name="TCurrent"></typeparam>
        /// <param name="callback"></param>
        protected bool TryOnExitResolveSynthesizedAttribute<TPrevious, TCurrent>(
            CollapseToPreviousCallback<TPrevious, TCurrent> callback)
        {
            // Short circuit when we fail to match the types.
            var d = Descriptors;

            var count = d.Count;

            // The Match needs to be a little stronger, including the actual originating Type.
            bool DoesMatch<T>(Func<IEnumerable<DescriptorTuple>, DescriptorTuple> getter)
            {
                var tuple = getter(Descriptors);

                // We identify the Tuple first because we may already have visited the Descriptors.
                if (tuple == null)
                {
                    return false;
                }

                // We do not care about the Value at this level.
                var (candidateType, _) = tuple;
                // The match must be Strong. We cannot fall back on the loose definition.
                return candidateType == typeof(T) /*|| value is T || value == null*/;
            }

            // ReSharper disable once InvertIf
            if (DoesMatch<TPrevious>(x => x.NextToLast())
                && DoesMatch<TCurrent>(x => x.Last()))
            {
                var (previousType, previous) = d.NextToLast();
                var current = d.Last();

                var previousInstance = (TPrevious) previous;
                var currentInstance = (TCurrent) current.Item2;

                callback(ref previousInstance, currentInstance);

                d.TryRemoveAt(d.Count - 1);

                /* Whatever work was done to Previous, needs to be replace. This is especially
                 * true when working with integral types. Remember to reintroduce the Previous
                 * including the now-collapsed Instance. */

                d[d.Count - 1] = CreateDescriptor(previousType, previousInstance);
            }

            // TODO: TBD: may want a more specific Count verification here...
            // TODO: TBD: even throw an exception if Count is dramatically different, not less/greater as potentially expected, etc
            return d.Count != count;
        }

        /// <summary>
        /// Callback returns whether <paramref name="value"/> Validation is correct.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        protected delegate bool DescriptorItemValidationCallback<in T>(T value);

        /// <summary>
        /// Renders the <typeparamref name="T"/> <paramref name="value"/>
        /// as a <see cref="string"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        protected delegate string RenderDescriptorItemCallback<in T>(T value);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validate"></param>
        /// <param name="render"></param>
        protected void ValidateCurrentDescriptorItem<T>(DescriptorItemValidationCallback<T> validate
            , RenderDescriptorItemCallback<T> render = null)
        {
            if (!Descriptors.Any())
            {
                throw new InvalidOperationException("There are no Descriptors in the stack.");
            }

            var requestedType = typeof(T);
            var (actualType, objValue) = Descriptors.Last();

            if (actualType != requestedType)
            {
                throw new InvalidOperationException($"'{actualType.FullName}' other than expected '{requestedType.FullName}'.");
            }

            var actualValue = (T)objValue;

            if (validate(actualValue))
            {
                return;
            }

            var message = (render ?? (x => $"Unexpected or invalid '{requestedType}' value '{x}'.")).Invoke(actualValue);

            throw new InvalidOperationException(message);
        }
    }
}
