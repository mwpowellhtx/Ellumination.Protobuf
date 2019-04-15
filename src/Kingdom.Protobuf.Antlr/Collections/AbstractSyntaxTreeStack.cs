using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf.Collections
{
    using StackTuple = Tuple<Type, object>;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPrevious"></typeparam>
    /// <typeparam name="TCurrent"></typeparam>
    /// <param name="previous"></param>
    /// <param name="current"></param>
    public delegate void ReduceStackToPreviousCallback<TPrevious, in TCurrent>(ref TPrevious previous, TCurrent current);

    /// <inheritdoc />
    public class AbstractSyntaxTreeStack<T> : IAbstractSyntaxTreeStack<T>
        where T : class, new()
    {
        /// <inheritdoc />
        public int Count => ProtectedStack.Count;

        /// <inheritdoc />
        public void Clear() => ProtectedStack.Clear();

        // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
        /// <summary>
        /// Exposes the Stack collection property for use in derived assets.
        /// </summary>
        protected IList<StackTuple> ProtectedStack { get; } = new List<StackTuple> { };

        /// <inheritdoc />
        public T RootInstance => (T)ProtectedStack?.FirstOrDefault()?.Item2;

        /// <inheritdoc />
        public virtual void PushBack<TCurrent>(TCurrent current)
            => ProtectedStack.Add(Tuple.Create(typeof(TCurrent), (object) current));

        /// <inheritdoc />
        public virtual void PushBack<TCurrent>(StackItemFactory<TCurrent> currentFactory) => PushBack(currentFactory());

        /// <inheritdoc />
        public virtual bool TryReduce<TPrevious, TCurrent>(ReduceStackToPreviousCallback<TPrevious, TCurrent> reduction)
        {
            // Short circuit when we fail to match the types.
            var count = ProtectedStack.Count;

            // The Match needs to be a little stronger, including the actual originating Type.
            bool DoesMatch<TSubject>(Func<IEnumerable<StackTuple>, StackTuple> getter)
            {
                var tuple = getter(ProtectedStack);

                // We identify the Tuple first because we may already have visited the Descriptors.
                if (tuple == null)
                {
                    return false;
                }

                // We do not care about the Value at this level.
                var (candidateType, _) = tuple;
                // The match must be Strong. We cannot fall back on the loose definition.
                return candidateType == typeof(TSubject) /*|| value is T || value == null*/;
            }

            // ReSharper disable once InvertIf
            if (DoesMatch<TPrevious>(x => x.NextToLast())
                && DoesMatch<TCurrent>(x => x.Last()))
            {
                var (previousType, previous) = ProtectedStack.NextToLast();
                var current = ProtectedStack.Last();

                var previousInstance = (TPrevious) previous;
                var currentInstance = (TCurrent) current.Item2;

                reduction(ref previousInstance, currentInstance);

                ProtectedStack.TryRemoveAt(ProtectedStack.Count - 1);

                /* Whatever work was done to Previous, needs to be replace. This is especially
                 * true when working with integral types. Remember to reintroduce the Previous
                 * including the now-collapsed Instance. */

                ProtectedStack[ProtectedStack.Count - 1] = Tuple.Create(previousType, (object) previousInstance);
            }

            // TODO: TBD: may want a more specific Count verification here...
            // TODO: TBD: even throw an exception if Count is dramatically different, not less/greater as potentially expected, etc
            return ProtectedStack.Count != count;
        }

       private void Validate<TCurrent>(Func<IEnumerable<Tuple<Type, object>>, Tuple<Type, object>> filter
            , Func<TCurrent, bool> validate, Func<TCurrent, string> render = null
            , Action<InvalidOperationException> visit = null)
        {
            if (!ProtectedStack.Any())
            {
                throw new InvalidOperationException("There are no Descriptors in the stack.");
            }

            var currentType = typeof(TCurrent);
            var (actualType, objValue) = filter(ProtectedStack);

            if (actualType != currentType)
            {
                throw new InvalidOperationException($"'{actualType.FullName}' other than expected '{currentType.FullName}'.");
            }

            var actualValue = (TCurrent) objValue;

            if (validate(actualValue))
            {
                return;
            }

            var message = (render ?? (x => $"Unexpected or invalid '{currentType}' value '{x}'.")).Invoke(actualValue);

            var ex = new InvalidOperationException(message);

            visit?.Invoke(ex);

            throw ex;
        }

       /// <inheritdoc />
        public virtual void ValidateFirst<TCurrent>(Func<TCurrent, bool> validate
           , Func<TCurrent, string> render = null, Action<InvalidOperationException> visit = null)
            => Validate(x => x.First(), validate, render, visit);

        /// <inheritdoc />
        public virtual void ValidateLast<TCurrent>(Func<TCurrent, bool> validate
            , Func<TCurrent, string> render = null, Action<InvalidOperationException> visit = null)
            => Validate(x => x.Last(), validate, render, visit);
    }
}
