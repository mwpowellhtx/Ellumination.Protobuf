using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    internal static class ArgumentExtensionMethods
    {
        public static IEnumerable<Tuple<object, Type>> AddArgument<T>(this IEnumerable<Tuple<object, Type>> args, T obj)
        {
            return args.Concat(new[] {Tuple.Create((object) obj, typeof(T))});
        }
    }
}
