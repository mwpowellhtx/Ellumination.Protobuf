using System;
using System.Linq;
using System.Reflection;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using ArgumentTuple = Tuple<object, Type>;

    /// <summary>
    /// Provides a set of helpful Objects extension methods.
    /// </summary>
    internal static class Objects
    {
        /// <summary>
        /// Returns the <see cref="ArgumentTuple"/> corresponding to the <typeparamref name="T"/>
        /// <paramref name="obj"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ArgumentTuple ToArgument<T>(this T obj) => Tuple.Create((object) obj, typeof(T));

        /// <summary>
        /// Constructs the <typeparamref name="TResult"/> given the <paramref name="args"/>
        /// <see cref="Tuple{T1,T2}"/>. We want the First Closest Match whose arguments are
        /// assignable from the given arguments.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        public static TResult Construct<TResult>(params ArgumentTuple[] args)
            where TResult : class
        {
            var resultType = typeof(TResult);

            if (resultType.IsAbstract || !resultType.IsClass)
            {
                throw new ArgumentException($"The type '{resultType.FullName}' cannot be constructed."
                    , nameof(TResult));
            }

            var requestedTypes = args.Select(arg => arg.Item2).ToArray();

            // We want the closest Constructor matching the Argument Types.
            ConstructorInfo GetFirstOrDefaultCtor() => resultType.GetConstructors().FirstOrDefault(ctor =>
            {
                // Matches when there are No Arguments, or the Types are all Assignable.
                var actualTypes = ctor.GetParameters().Select(pi => pi.ParameterType).ToArray();
                return !(actualTypes.Any() || requestedTypes.Any())
                       || (actualTypes.Length == requestedTypes.Length
                           && actualTypes.Zip(requestedTypes, (x, y) => x.IsAssignableFrom(y)).All(z => z));
            });

            object[] GetActualParameters() => args.Select(x => x.Item1).ToArray();

            return (TResult) GetFirstOrDefaultCtor()?.Invoke(GetActualParameters());
        }

        /// <summary>
        /// Returns the <paramref name="obj"/> after Verifying Not Null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T VerifyNotNull<T>(this T obj)
            where T : class
            => obj ?? throw new ArgumentNullException(nameof(obj));
    }
}
