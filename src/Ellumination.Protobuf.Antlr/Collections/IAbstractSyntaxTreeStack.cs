using System;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAbstractSyntaxTreeStack<out T>
        where T : class, new()
    {
        /// <summary>
        /// Gets the RootInstance.
        /// </summary>
        T RootInstance { get; }

        /// <summary>
        /// Pushes the <paramref name="current"/> onto the Back of the Stack.
        /// </summary>
        /// <typeparam name="TCurrent"></typeparam>
        /// <param name="current"></param>
        void PushBack<TCurrent>(TCurrent current);

        /// <summary>
        /// Pushes the <paramref name="currentFactory"/> item onto the Back of the Stack.
        /// </summary>
        /// <typeparam name="TCurrent"></typeparam>
        /// <param name="currentFactory"></param>
        void PushBack<TCurrent>(StackItemFactory<TCurrent> currentFactory);

        /// <summary>
        /// Tries to Reduce the Stack given the <paramref name="reduction"/> callback.
        /// </summary>
        /// <typeparam name="TPrevious"></typeparam>
        /// <typeparam name="TCurrent"></typeparam>
        /// <param name="reduction"></param>
        /// <returns></returns>
        bool TryReduce<TPrevious, TCurrent>(ReduceStackToPreviousCallback<TPrevious, TCurrent> reduction);

        /// <summary>
        /// Validates the First Item.
        /// </summary>
        /// <typeparam name="TCurrent"></typeparam>
        /// <param name="validate"></param>
        /// <param name="render"></param>
        /// <param name="visit"></param>
        void ValidateFirst<TCurrent>(Func<TCurrent, bool> validate, Func<TCurrent, string> render = null, Action<InvalidOperationException> visit = null);

        /// <summary>
        /// Validates the Last Item.
        /// </summary>
        /// <typeparam name="TCurrent"></typeparam>
        /// <param name="validate"></param>
        /// <param name="render"></param>
        /// <param name="visit"></param>
        void ValidateLast<TCurrent>(Func<TCurrent, bool> validate, Func<TCurrent, string> render = null, Action<InvalidOperationException> visit = null);

        /// <summary>
        /// Gets the number of Items in the Stack.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Clears the Items from the Stack.
        /// </summary>
        void Clear();
    }
}
