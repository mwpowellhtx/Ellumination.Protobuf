using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using static Characters;
    using static String;
    using static WhiteSpaceAndCommentOption;

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
            => values.RenderOptions(
                // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
                new StringRenderingOptions { }
            );

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
        {
            string GetComments(params WhiteSpaceAndCommentOption[] masks)
                => options.WhiteSpaceAndCommentRendering.RenderMaskedComments(masks);

            return values.Any()
                ? $"{GetComments(MultiLineComment)}"
                  + $"{OpenSquareBracket}"
                  + $"{GetComments(MultiLineComment)}"
                  // ReSharper disable once PossibleMultipleEnumeration
                  + $"{Join($"{Comma} ", values.Select(x => x.ToDescriptorString(options)))}"
                  + $"{GetComments(MultiLineComment)}"
                  + $"{CloseSquareBracket}"
                  + $"{GetComments(MultiLineComment)}"
                : Empty;
        }
    }
}
