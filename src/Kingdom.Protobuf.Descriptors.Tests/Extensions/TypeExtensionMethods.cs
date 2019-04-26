using System;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    internal static class TypeExtensionMethods
    {
        public static bool IsStatic(this Type type) => type != null && type.IsAbstract && type.IsSealed;
    }
}
