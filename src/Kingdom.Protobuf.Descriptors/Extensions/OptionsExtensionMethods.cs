using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static String;

    /// <summary>
    /// Provides a set of helpful <see cref="IOption"/> extension methods.
    /// </summary>
    internal static class OptionsExtensionMethods
    {
        /// <summary>
        /// Renders the <see cref="IOption"/> <typeparamref name="T"/> <paramref name="values"/>
        /// assuming default <see cref="IStringRenderingOptions"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string RenderOptions<T>(this IEnumerable<T> values)
            where T : IOption
            => values.RenderOptions(new StringRenderingOptions { });

        // ReSharper disable once PossibleMultipleEnumeration
        /// <summary>
        /// Renders the <see cref="IOption"/> <typeparamref name="T"/> <paramref name="values"/>
        /// given <paramref name="options"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string RenderOptions<T>(this IEnumerable<T> values, IStringRenderingOptions options)
            where T : IOption // TODO: TBD: perhaps IOption? or ICanRenderString
            => values.Any()
                // ReSharper disable once PossibleMultipleEnumeration
                ? $"[{Join(", ", values.Select(x => x.ToDescriptorString(options)))}]"
                : Empty;
    }
}
