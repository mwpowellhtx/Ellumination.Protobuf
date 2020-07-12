using System;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using static Convert;

    internal static class ParserExtensionMethods
    {
        /// <summary>
        /// Returns the Parsed <paramref name="s"/> in terms of <see cref="bool"/>.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ParseBoolean(this string s) => bool.Parse(s);

        /// <summary>
        /// 10
        /// </summary>
        private const int DefaultLongBase = 10;

        /// <summary>
        /// Returns the Parsed <paramref name="s"/> using the appropriate
        /// <paramref name="fromBase"/>. Default is Base Ten (10).
        /// </summary>
        /// <param name="s"></param>
        /// <param name="fromBase">Specifies the Base from which to Convert to <see cref="long"/>.</param>
        /// <returns></returns>
        /// <see cref="DefaultLongBase"/>
        public static long ParseLong(this string s, int fromBase = DefaultLongBase) => ToInt64(s, fromBase);

        /// <summary>
        /// Returns the Parsed <paramref name="s"/> in terms of <see cref="double"/>.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double ParseDouble(this string s) => double.Parse(s);
    }
}
