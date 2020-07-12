using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using TOption = WhiteSpaceAndCommentOption;
    using static WhiteSpaceAndCommentOption;

    // TODO: TBD: I am doing this enough, and there is enough boilerplate here, should consider a flags-based-enum code analysis/code generator.
    // TODO: TBD: if I should pursue that avenue, can make them partial so that additional methods may be exposed...
    // TODO: TBD: doing this may hold some refactoring value for me, but it is definitely a non-trivial rabbit hole I am not willing to dive into just yet
    // TODO: TBD: I want to make some more in roads verifying the parser first, especially handling comments
    // TODO: TBD: then see about making some concrete steps on the sat parameters extrapolation / code generation etc
    // TODO: TBD: then time and space permitting dive into the enum extensions code generator
    internal static partial class WhiteSpaceAndCommentOptionExtensionMethods
    {
        private const TOption ZedValue = NoWhiteSpaceOrCommentOption;

        public static IEnumerable<TOption> SimpleValues(this TOption _, bool includeNone = false)
        {
            if (includeNone)
            {
                yield return ZedValue;
            }

            yield return CommentBefore;
            yield return CommentAfter;
            yield return CommentSameLine;
            yield return EmbeddedComments;
            yield return WithLineSeparatorNewLine;
            yield return WithLineSeparatorCarriageReturnNewLine;
            yield return SingleLineComment;
            yield return MultiLineComment;
        }

        public static IEnumerable<TOption> UnmaskOptions(this TOption value, bool includeNone = true)
        {
            if (includeNone)
            {
                yield return ZedValue;
            }

            foreach (var x in value.SimpleValues())
            {
                if ((value & x) == x)
                {
                    yield return x;
                }
            }
        }

        // TODO: TBD: verify positive option, verify negative option, perhaps receive a function for comparison?
        private static void VerifyOption(string message, TOption option, params TOption[] masks)
        {
            if (!masks.Any())
            {
                throw new InvalidOperationException("At least one mask expected.");
            }

            if (masks.Contains(option & masks.AggregateMask()))
            {
                return;
            }

            throw new InvalidOperationException(message);
        }

        private static IEnumerable<T> GetRange<T>(params T[] values)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var x in values)
            {
                yield return x;
            }
        }

        /// <summary>
        /// Returns the Mask Made from the <paramref name="option"/> and
        /// <paramref name="others"/>,
        /// </summary>
        /// <param name="option"></param>
        /// <param name="others"></param>
        /// <returns></returns>
        public static TOption AggregateMask(this TOption option, params TOption[] others)
            => others.Aggregate(option, (g, x) => g | x);

        /// <summary>
        /// Returns the Mask Made from the <paramref name="options"/>.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static TOption AggregateMask(this IEnumerable<TOption> options)
            => (options ?? GetRange<TOption>()).Aggregate(ZedValue, (g, x) => g | x);

        /// <summary>
        /// Returns whether <paramref name="mask"/> and <paramref name="options"/> Intersect.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static bool Intersects(this TOption options, TOption mask) => (options & mask) != ZedValue;
    }
}
