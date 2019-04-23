using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Combinatorics.Combinatorials;
    using Kingdom.Collections.Variants;
    using Xunit;
    using static CollectionHelpers;
    using static Identification;
    using static Randomizer;
    using static FieldNumbers;
    using static WhiteSpaceAndCommentOption;

    internal static class Domain
    {
        /// <summary>
        /// 0L
        /// </summary>
        [Obsolete("Interesting, but also potentially obsolete")]
        internal const long ZedLong = 0L;

        /// <summary>
        /// <see cref="long.MaxValue"/>
        /// </summary>
        [Obsolete("Potentially obsolete")]
        internal const long MaxLong = long.MaxValue;

        /// <summary>
        /// <see cref="long.MinValue"/>
        /// </summary>
        [Obsolete("Potentially obsolete")]
        internal const long MinLong = long.MinValue;

        /// <summary>
        /// 0d
        /// </summary>
        internal const double ZedFloatingPoint = 0d;

        /// <summary>
        /// 1d
        /// </summary>
        internal const double OneFloatingPoint = 1d;

        /// <summary>
        /// <see cref="double.MaxValue"/>
        /// </summary>
        internal const double MaxFloatingPoint = double.MaxValue;

        /// <summary>
        /// <see cref="double.MinValue"/>
        /// </summary>
        internal const double MinFloatingPoint = double.MinValue;

        /// <summary>
        /// <see cref="double.NaN"/>
        /// </summary>
        internal const double NaN = double.NaN;

        /// <summary>
        /// <see cref="double.NegativeInfinity"/>
        /// </summary>
        internal const double NegativeInfinity = double.NegativeInfinity;

        /// <summary>
        /// <see cref="double.PositiveInfinity"/>
        /// </summary>
        internal const double PositiveInfinity = double.PositiveInfinity;

        /// <summary>
        /// Gets a <see cref="string"/> representation of <see cref="NewId"/>.
        /// </summary>
        internal static string NewIdentityString => $"{NewId:N}";

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Returns the Next presumed Invalid Field Number assuming <paramref name="min"/>
        /// and <paramref name="max"/> as well as <paramref name="predicate"/>.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        private static long GetNextInvalidFieldNumber(long min, long max, Func<long, bool> predicate)
        {
            bool TryNextCandidate(out long x) => predicate(x = Rnd.NextLong(min, max));

            long result;

            // Which, trusting we do not get stuck in this look forever.
            while (TryNextCandidate(out result))
            {
            }

            return result;
        }

        /// <summary>
        /// Gets the Next Valid Random Field Number. Literally, this yields any value between
        /// <see cref="MinimumFieldNumber"/> and <see cref="MaximumFieldNumber"/> excluding those
        /// values between <see cref="MinimumReservedFieldNumber"/> and
        /// <see cref="MaximumReservedFieldNumber"/>
        /// </summary>
        /// <see cref="ZedLong"/>
        /// <see cref="MaxLong"/>
        internal static long FieldNumber => GetNextInvalidFieldNumber(
            MinimumFieldNumber, MaximumFieldNumber
            , x => x.IsReservedByGoogleProtocolBuffers()
        );

        /// <summary>
        /// Gets the Next Reserved Field Number. Literally, this yields any value between
        /// <see cref="MinimumReservedFieldNumber"/> and <see cref="MaximumReservedFieldNumber"/>.
        /// </summary>
        internal static long ReservedFieldNumber => Rnd.NextLong(
            MinimumReservedFieldNumber, MaximumReservedFieldNumber);

        /// <summary>
        /// Gets the Next Invalid Field Number. Literally, this yields any value between
        /// <see cref="long.MinValue"/> and <see cref="long.MaxValue"/> that is outside of
        /// the range <see cref="MinimumFieldNumber"/> through <see cref="MaximumFieldNumber"/>.
        /// </summary>
        internal static long InvalidFieldNumber => GetNextInvalidFieldNumber(
            long.MinValue, long.MaxValue, x => x.IsValidFieldNumber());

        internal static IEnumerable<IConstant> BuildOptionValues<T>(params T[] values)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var value in values)
            {
                yield return Constant.Create(value);
            }
        }

        /// <summary>
        /// Gets the set of <see cref="LabelKind"/> values.
        /// </summary>
        /// <see cref="LabelKindExtensionMethods.Values"/>
        internal static IEnumerable<LabelKind> LabelKinds => LabelKindExtensionMethods.Values;

        /// <summary>
        /// Gets the set of <see cref="ProtoType"/> values.
        /// </summary>
        /// <see cref="ProtoTypeExtensionMethods.ProtoTypeValues"/>
        internal static IEnumerable<ProtoType> ProtoTypes => ProtoTypeExtensionMethods.ProtoTypeValues;

        /// <summary>
        /// Gets the set of <see cref="KeyType"/> values.
        /// </summary>
        /// <see cref="ProtoTypeExtensionMethods.KeyTypeValues"/>

        internal static IEnumerable<KeyType> KeyTypes => ProtoTypeExtensionMethods.KeyTypeValues;

        /// <summary>
        /// Gets a set of <see cref="ElementTypeIdentifierPath"/> instances.
        /// </summary>
        internal static IEnumerable<ElementTypeIdentifierPath> ElementTypeIdentifierPaths
        {
            get
            {
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var path in GetElementTypeIdentifierPaths(GetRange(1, 3)
                    , GetRange(1, 3), GetRange(false, true)))
                {
                    yield return path;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <see cref="ProtoType"/>
        /// <see cref="Variant.Create{T}()"/>
        /// <see cref="IVariant"/>
        internal static IEnumerable<IVariant> Types
        {
            get
            {
                foreach (var type in ProtoTypes)
                {
                    yield return Variant.Create(type);
                }

                foreach (var path in ElementTypeIdentifierPaths)
                {
                    yield return Variant.Create(path);
                }
            }
        }

        /// <summary>
        /// Returns the Identifier corresponding with the <paramref name="length"/>.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        internal static string GetFieldName(int length) => GetIdent(length);

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Returns the Identifiers corresponding with the <paramref name="lengths"/> for
        /// use with <see cref="NormalFieldStatement"/>.
        /// </summary>
        /// <param name="lengths"></param>
        /// <returns></returns>
        internal static IEnumerable<string> GetFieldNames(IEnumerable<int> lengths)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var length in lengths)
            {
                yield return GetFieldName(length);
            }
        }

        internal static OptionIdentifierPath GetOptionIdentifierPath(int prefixLength, int prefixParts
            , int suffixLength, int suffixParts, bool prefixGrouped)
        {
            var prefix = GetFullIdent(prefixLength, prefixParts);
            return new OptionIdentifierPath(prefix.Concat(GetFullIdent(suffixLength, suffixParts)))
            {
                SuffixStartIndex = prefix.Count,
                IsPrefixGrouped = prefixGrouped
            };
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Returns a set of <see cref="OptionIdentifierPath"/> instances corresponding to the
        /// Parameters.
        /// </summary>
        /// <param name="prefixLengths"></param>
        /// <param name="prefixParts"></param>
        /// <param name="suffixLengths"></param>
        /// <param name="suffixParts"></param>
        /// <returns></returns>
        internal static IEnumerable<OptionIdentifierPath> GetOptionIdentifierPaths(
            IEnumerable<int> prefixLengths, IEnumerable<int> prefixParts
            , IEnumerable<int> suffixLengths, IEnumerable<int> suffixParts)
        {
            var inputs = prefixLengths.Select(x => (object) x).ToArray().Combine(
                prefixParts.Select(x => (object) x).ToArray()
                , suffixLengths.Select(x => (object) x).ToArray()
                , suffixParts.Select(x => (object) x).ToArray()
                , GetRange<object>(true, false).ToArray()
            );

            inputs.SilentOverflow = true;

            for (var i = 0; i < inputs.Count; i++, ++inputs)
            {
                var current = inputs.CurrentCombination.ToArray();
                yield return GetOptionIdentifierPath((int) current[0], (int) current[1]
                    , (int) current[2], (int) current[3], (bool) current[4]);
            }
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Returns the <see cref="long"/> Values Ranging from <paramref name="min"/> to
        /// <paramref name="max"/>. This is Recursive given <paramref name="depth"/> and
        /// <paramref name="count"/> at each Depth.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="depth"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        internal static IEnumerable<long> GetLongValues(long min, long max, int depth, int count)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            var results = new List<long> { };
            if (depth > 1)
            {
                var mid = (max - min) / 2 + min;
                var next = depth - 1;
                results.AddRange(GetLongValues(min, mid, next, count));
                results.AddRange(GetLongValues(mid, max, next, count));
            }

            while (count-- > 0)
            {
                var x = Rnd.NextLong(min, max);
                if (!results.Contains(x))
                {
                    results.Add(x);
                }
            }

            return results;
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Returns the Floating Point Values given the parameters.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="depth"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        internal static IEnumerable<double> GetFloatingPointValues(double min, double max, int depth, int count)
        {
            // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
            var results = new List<double> { };
            if (depth > 1)
            {
                var mid = (max - min) / 2;
                var next = depth - 1;
                results.AddRange(GetFloatingPointValues(min, mid, next, count));
                results.AddRange(GetFloatingPointValues(mid, max, next, count));
            }

            while (count-- > 0)
            {
                var x = Rnd.NextDouble(min, max);
                if (!results.Contains(x))
                {
                    results.Add(x);
                }
            }

            return results;
        }

        internal static IEnumerable<RangeDescriptor> ToRangeDescriptors(this IEnumerable<Tuple<long, long?>> ranges)
        {
            Assert.NotNull(ranges);
            // ReSharper disable once PossibleMultipleEnumeration
            Assert.NotEmpty(ranges);

            // ReSharper disable once PossibleMultipleEnumeration
            foreach (var (minimum, maximum) in ranges)
            {
                yield return new RangeDescriptor {Minimum = minimum, Maximum = maximum};
            }
        }

        internal static IEnumerable<TOption> ElaborateOptions<TOption>(OptionIdentifierPath[] optionNames
            , IConstant[] optionValues)
            where TOption : IOption, new()
        {
            var inputs = optionNames.ToArray<object>() // OptionName
                .Combine(
                    optionValues.ToArray<object>() // OptionValue
                );

            inputs.SilentOverflow = true;

            for (var i = 0; i < inputs.Count; i++, ++inputs)
            {
                var current = inputs.CurrentCombination.ToArray();
                var optionName = (OptionIdentifierPath) current[0];
                var optionValue = (IConstant) current[1];
                yield return new TOption {Name = optionName, Value = optionValue};
            }
        }

        private static IEnumerable<object> _whiteSpaceAndCommentOption;

        internal static IEnumerable<object> AllWhiteSpaceAndCommentOptions
        {
            get
            {
                IEnumerable<object> GetAll()
                {
                    yield return NoWhiteSpaceOrCommentOption;

                    var inputs = GetRange<object>(CommentBefore, CommentAfter, CommentSameLine)
                        .Combine(
                            GetRange<object>(MultiLineComment, SingleLineComment)
                            , GetRange<object>(NoWhiteSpaceOrCommentOption, EmbeddedComments)
                            , GetRange<object>(WithLineSeparatorCarriageReturnNewLine, WithLineSeparatorNewLine)
                        );

                    inputs.SilentOverflow = true;

                    // ReSharper disable once IdentifierTypo
                    WhiteSpaceAndCommentOption Convert(IReadOnlyList<object> objs, ref int index)
                        => (WhiteSpaceAndCommentOption) objs[++index];

                    for (var i = 0; i < inputs.Count; i++, ++inputs)
                    {
                        var index = -1;
                        var current = inputs.CurrentCombination.ToArray();
                        yield return Convert(current, ref index)
                                     | Convert(current, ref index)
                                     | Convert(current, ref index) | Convert(current, ref index);
                    }
                }

                return _whiteSpaceAndCommentOption
                       ?? (_whiteSpaceAndCommentOption = GetAll().ToArray());
            }
        }
    }
}
