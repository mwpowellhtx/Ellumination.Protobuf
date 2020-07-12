using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using static String;
    using TOption = IntegerRenderingOption;
    using static IntegerRenderingOption;

    internal static class IntegerRenderingExtensionMethods
    {
        /// <summary>
        /// Returns the set of Primitive, if you will, <see cref="TOption"/> values.
        /// Does not matter what the leading value was, only that it was of type
        /// <see cref="TOption"/>. We intentionally avoid referencing any of the Derived
        /// masks for purposes of returning the Simple ones.
        /// </summary>
        /// <param name="_"></param>
        /// <param name="includeNone"></param>
        /// <returns></returns>
        public static IEnumerable<TOption> SimpleValues(this TOption _, bool includeNone = false)
        {
            if (includeNone)
            {
                yield return NoIntegerRenderingOption;
            }

            yield return HexadecimalInteger;
            yield return OctalInteger;
            yield return DecimalInteger;
            yield return UpperCaseInteger;
            yield return LowerCaseInteger;
            yield return IntegerForcedSignage;
        }

        public static IEnumerable<TOption> UnmaskOptions(this TOption value, bool includeNone = true)
        {
            if (includeNone)
            {
                yield return NoIntegerRenderingOption;
            }

            foreach (var x in value.SimpleValues())
            {
                if ((value & x) == x)
                {
                    yield return x;
                }
            }
        }

        /// <summary>
        /// <see cref="NoIntegerRenderingOption"/>
        /// </summary>
        private const TOption None = NoIntegerRenderingOption;

        // ReSharper disable once InconsistentNaming
        private static IEnumerable<T> GetRange<T>(params T[] values)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var x in values)
            {
                yield return x;
            }
        }

        /// <summary>
        /// Collates several useful Integer Base values.
        /// </summary>

        private static IDictionary<TOption, int> IntegerFormatBases { get; } = new Dictionary<TOption, int>
        {
            {None, 10},
            {HexadecimalInteger, 16},
            {OctalInteger, 8},
            {DecimalInteger, 10},
        };

        /// <summary>
        /// Collates several useful Integer Prefixes.
        /// </summary>
        private static IDictionary<TOption, string> IntegerPrefixes { get; } = new Dictionary<TOption, string>
        {
            {None, Empty},
            {HexadecimalInteger, "0x"},
            {OctalInteger, "0"},
            {DecimalInteger, Empty},
        };

        private delegate string IntegerCaseCallback(string s);

        /// <summary>
        /// Collates several useful <see cref="IntegerCaseCallback"/> functions.
        /// </summary>
        private static IDictionary<TOption, IntegerCaseCallback> IntegerCaseCallbacks { get; }
            = new Dictionary<TOption, IntegerCaseCallback>
            {
                {None, s => s},
                {LowerCaseInteger, s => s.ToLower()},
                {UpperCaseInteger, s => s.ToUpper()},
            };

        /// <summary>
        /// Verifies that the <paramref name="option"/> aligns with the Aggregate
        /// <paramref name="masks"/>.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="option"></param>
        /// <param name="masks"></param>
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

        /// <summary>
        /// Returns the <paramref name="value"/> Rendered according to the
        /// <paramref name="options"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string RenderLong(this long value, TOption options)
        {
            VerifyOption("More than one format specified.", options, None
                , HexadecimalInteger, OctalInteger, DecimalInteger);
            VerifyOption("More than one case specified.", options, None
                , LowerCaseInteger, UpperCaseInteger);

            var formatMask = HexadecimalInteger.AggregateMask(OctalInteger, DecimalInteger);
            var toBase = IntegerFormatBases[options & formatMask];
            var s = IntegerPrefixes[options & formatMask] + Convert.ToString(value, toBase);

            var caseMask = LowerCaseInteger.AggregateMask(UpperCaseInteger);
            var rendered = IntegerCaseCallbacks[options & caseMask].Invoke(s);

            // Includes Decimal as well as Octal use cases.
            bool IsDigit(char c)
            {
                const string digits = "0123456789";
                return digits.Contains(c);
            }

            // Including a check on the Value itself for cross verification.
            if (value >= 0
                && IsDigit(rendered.First())
                && options.Intersects(IntegerForcedSignage)
                && !options.Intersects(OctalInteger.AggregateMask(HexadecimalInteger)))
            {
                // This is the only case it can be at this point.
                rendered = $"+{rendered}";
            }

            return rendered;
        }

        /// <summary>
        /// Returns the Mask combining <paramref name="option"/> and <paramref name="others"/>.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="others"></param>
        /// <returns></returns>
        public static TOption AggregateMask(this TOption option, params TOption[] others)
            => others.Aggregate(option, (g, x) => g | x);

        /// <summary>
        /// Returns the Mask combining <paramref name="options"/>.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static TOption AggregateMask(this IEnumerable<TOption> options)
            => (options ?? GetRange<TOption>()).Aggregate(None, (g, x) => g | x);

        /// <summary>
        /// Returns whether <paramref name="options"/> and <paramref name="mask"/> Intersect.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        private static bool Intersects(this TOption options, TOption mask) => (options & mask) != None;
    }
}
