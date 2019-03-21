using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using TOption = FloatingPointRenderingOption;
    using static FloatingPointRenderingOption;

    internal static class FloatingPointRenderingExtensionMethods
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
                yield return NoFloatingPointRenderingOption;
            }

            yield return FloatingPointGeneral;
            yield return FloatingPointRoundTrip;
            yield return FloatingPointFixed;
            yield return FloatingPointScientific;
            yield return FloatingPointUpperCase;
            yield return FloatingPointLowerCase;
            yield return FloatingPointForceLeadingDot;
            yield return FloatingPointForceTrailingDot;
            yield return FloatingPointForceSignage;
        }

        public static IEnumerable<TOption> UnmaskOptions(this TOption value, bool includeNone = true)
        {
            if (includeNone)
            {
                yield return NoFloatingPointRenderingOption;
            }

            foreach (var x in value.SimpleValues())
            {
                if ((value & x) == x)
                {
                    yield return x;
                }
            }
        }

        private const TOption None = NoFloatingPointRenderingOption;

        // TODO: TBD: may need more or less than 99 here, but this seems to work for us for the time being...
        private static IDictionary<TOption, string> FloatingPointSpecifiers { get; } = new Dictionary<TOption, string>
        {
            {None, "g99"},
            {FloatingPointLowerCase, "g99"},
            {FloatingPointUpperCase, "G99"},
            {FloatingPointGeneral, "g99"},
            {FloatingPointGeneral | FloatingPointLowerCase, "g99"},
            {FloatingPointGeneral | FloatingPointUpperCase, "G99"},
            {FloatingPointRoundTrip, "r99"},
            {FloatingPointRoundTrip | FloatingPointLowerCase, "r99"},
            {FloatingPointRoundTrip | FloatingPointUpperCase, "R99"},
            {FloatingPointScientific, "e99"},
            {FloatingPointScientific | FloatingPointLowerCase, "e99"},
            {FloatingPointScientific | FloatingPointUpperCase, "E99"},
#pragma warning disable 618
            {FloatingPointFixed, "f99"},
            {FloatingPointFixed | FloatingPointLowerCase, "f99"},
            {FloatingPointFixed | FloatingPointUpperCase, "F99"},
#pragma warning restore 618
        };

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

        public static string RenderDouble(this double value, TOption options)
        {
#pragma warning disable 618
            VerifyOption("More than one format specified.", options, None
                , FloatingPointGeneral, FloatingPointRoundTrip, FloatingPointScientific, FloatingPointFixed);
#pragma warning restore 618
            VerifyOption("More than one case specified.", options, None
                , FloatingPointLowerCase, FloatingPointUpperCase);
            VerifyOption("More than one dot specified.", options, None
                , FloatingPointForceLeadingDot, FloatingPointForceTrailingDot);

            // Includes both "Format" as well as Case.
#pragma warning disable 618
            var formatMask = FloatingPointGeneral.AggregateMask(FloatingPointLowerCase, FloatingPointUpperCase
                , FloatingPointRoundTrip, FloatingPointScientific, FloatingPointFixed);
#pragma warning restore 618
            var dotMask = FloatingPointForceLeadingDot.AggregateMask(FloatingPointForceTrailingDot);

            // ReSharper disable once UnusedParameter.Local
            // Receiving a y here for debugging purposes since value cannot be seen by the breakpoints.
            string RenderWithDot(double y, string s, TOption o)
            {
                // Scientific format Cannot be Rendered with a Dot.
                bool ShallNotRenderWithDot() => "eE".Any(e => s.IndexOf(e) >= 0);

                if (ShallNotRenderWithDot())
                {
                    return s;
                }

                const char zed = '0', dot = '.';

                var hasDot = s.IndexOf(dot) >= 0;

                // ReSharper disable once SwitchStatementMissingSomeCases
                // Normalize the Options, ensure that we yield at least a Trailing Dot.
                switch (o == None ? FloatingPointForceTrailingDot : o)
                {
                    // We can safely trim Leading Zed no matter what.
                    case FloatingPointForceLeadingDot:

                        // Test case permitting the Leading will be a Dot.
                        while (s.Any() && s.First() == zed)
                        {
                            s = s.Substring(1);
                        }

                        // If there is Nothing remaining then that is ".0" or Dot Zed.
                        if (!s.Any())
                        {
                            s = $"{dot}{zed}";
                        }
                        else if (!hasDot)
                        {
                            /* However, Dot Digit is not the same given Digit.
                             * In this case, fall back on Trailing Dot. */
                            s += dot;
                        }

                        break;

                    case FloatingPointForceTrailingDot:

                        // Trimming Trailing Zed is contingent upon Having a Dot.
                        if (hasDot)
                        {
                            // Test case permitting the Trailing will be a Dot.
                            while (s.Any() && s.Last() == zed)
                            {
                                s = s.Substring(0, s.Length - 1);
                            }
                        }
                        else
                        {
                            // Nothing to Trim. Does not Have a Dot therefore simply Append Dot.
                            s += dot;
                        }

                        break;
                }

                return s;
            }

            var spec = FloatingPointSpecifiers[options & formatMask];
            value.ToString(spec).ExtrapolateSignage(options, out var signage, out var rendered);
            var renderedWithDot = RenderWithDot(value, rendered, options & dotMask);
            return renderedWithDot.RenderWithPrependedSignage(signage);
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
            => (options ?? GetRange<TOption>()).Aggregate(None, (g, x) => g | x);

        /// <summary>
        /// Returns whether <paramref name="mask"/> and <paramref name="options"/> Intersect.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        private static bool Intersects(this TOption options, TOption mask) => (options & mask) != None;

        /// <summary>
        /// Given the formatted <paramref name="s"/> and optional <paramref name="signage"/>,
        /// Extrapolates the Signage as well as the remaining <paramref name="rendered"/>.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="options"></param>
        /// <param name="signage"></param>
        /// <param name="rendered"></param>
        private static void ExtrapolateSignage(this string s, TOption options, out char? signage, out string rendered)
        {
            signage = null;

            const char plusSign = '+';

            if (s.Any() && GetRange('-', plusSign).Contains(s.First()))
            {
                signage = s.First();
            }

            const TOption signageMask = FloatingPointForceSignage;

            // This is the appropriate time to calculate Rendered.
            rendered = signage.HasValue ? s.Substring(1) : s;

            // Because we turn around and may Force a Signage that was not there to begin with.
            if (options.Intersects(signageMask) && !signage.HasValue)
            {
                signage = plusSign;
            }
        }

        /// <summary>
        /// Returns the fully Rendered <paramref name="s"/> including any Prepended
        /// <paramref name="sign"/> Signage.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        private static string RenderWithPrependedSignage(this string s, char? sign)
            => sign.HasValue ? $"{sign.Value}{s}" : s;
    }
}
