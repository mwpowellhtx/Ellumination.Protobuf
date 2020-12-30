using System;

// TODO: TBD: we very intentionally identify the namespace so that...
// TODO: TBD: if when our development pipeline "sees" the package update with contributions, then we can respond accordingly...
namespace Google.Protobuf.WellKnownTypes
{
    using static Duration;

    /// <summary>
    /// Provides a set of extension methods supporting bits concerning the Google bits.
    /// </summary>
    /// <remarks>There is not a lot more we can add to the <em>Google.Protobuf</em>
    /// package, but we can allow for a more fluent usage in key areas.</remarks>
    public static class DurationExtensionMethods
    {
        /// <summary>
        /// Bounds the <paramref name="value"/> By the <paramref name="min"/>
        /// and <paramref name="max"/> Values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static T BoundedBy<T>(this T value, T min, T max)
            where T : struct, IComparable<T>
        {
            const int eq = default;

            if (value.CompareTo(min) < eq)
            {
                return min;
            }

            if (value.CompareTo(max) > eq)
            {
                return max;
            }

            return value;
        }

        /// <summary>
        /// Returns the normalized <paramref name="value"/> Bounded By <paramref name="bounds"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="bounds"></param>
        /// <returns></returns>
        private static T BoundedBy<T>(this T value, (T min, T max) bounds)
            where T : struct, IComparable<T> =>
            value.BoundedBy(bounds.min, bounds.max);

        /// <summary>
        /// Signed seconds of the span of time. Must be from <c>-315,576,000,000</c> to
        /// <c>+315,576,000,000</c>, inclusive. Note, these bounds are computed from:
        /// <c>60 sec/min * 60 min/hr * 24 hr/day * 365.25 days/year * 10000 years</c>.
        /// </summary>
        /// <see cref="Duration"/>
        /// <see cref="MinSeconds"/>
        /// <see cref="MaxSeconds"/>
        private static (long min, long max) IntegerSecondBounds { get; } = (
            MinSeconds
            , MaxSeconds
        );

        /// <summary>
        /// Signed fractions of a second at nanosecond resolution of the span of time. Durations
        /// less than one second are represented with a 0 <em>seconds</em> field and a positive
        /// or negative <em>nanos</em> field. For durations of one second or more, a non-zero
        /// value for the <em>nanos</em> field must be of the same sign as the <em>seconds</em>
        /// field. Must be from <c>-999,999,999</c> to <c>+999,999,999</c> inclusive.
        /// </summary>
        /// <see cref="Duration"/>
        /// <see cref="NanosecondsPerSecond"/>
        private static (int min, int max) IntegerNanosBounds { get; } = (
            -NanosecondsPerSecond + 1
            , NanosecondsPerSecond
        );

        // TODO: TBD: re: Google.Protobuf contributions...
        /// <summary>
        /// Returns the <see cref="Duration"/> corresponding with the <paramref name="seconds"/>
        /// and optional <paramref name="nanos"/>.
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="nanos"></param>
        /// <returns>The <see cref="Duration"/> corresponding to the <paramref name="seconds"/>
        /// and optional <paramref name="nanos"/>.</returns>
        /// <remarks>We should consider another <em>Google.Protobuf</em> contribution that adds
        /// <c>implicit</c> conversion from <see cref="long"/> and or <see cref="int"/>, and
        /// possibly even <see cref="ValueTuple{Int64, Int32}"/>.</remarks>
        public static Duration AsDuration(this int seconds, int? nanos = default) =>
            ((long)seconds).AsDuration(nanos);

        /// <summary>
        /// Returns the <see cref="Duration"/> corresponding with the <paramref name="seconds"/>
        /// and optional <paramref name="nanos"/>.
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="nanos"></param>
        /// <returns>The <see cref="Duration"/> corresponding to the <paramref name="seconds"/>
        /// and optional <paramref name="nanos"/>.</returns>
        public static Duration AsDuration(this long seconds, int? nanos = default)
        {
            seconds = seconds.BoundedBy(IntegerSecondBounds);

            // TODO: TBD: with perhaps more seconds/nanos interactions at the seconds edges...
            if (nanos.HasValue)
            {
                var args = (
                    seconds
                    , nanos: (nanos ?? default).BoundedBy(IntegerNanosBounds)
                );

                return new Duration { Seconds = args.seconds, Nanos = args.nanos };
            }

            // Unspecified at this level means Zero, or its Default, to Duration.
            return new Duration { Seconds = seconds };
        }

        /// <summary>
        /// Returns the <see cref="Duration"/> corresponding with the <see cref="Duration.Nanos"/>
        /// <paramref name="value"/>. Leaves the <see cref="Duration.Seconds"/> off.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Duration AsNanoDuration(this int value) => new Duration
        {
            Nanos = value.BoundedBy(IntegerNanosBounds.min, IntegerNanosBounds.max)
        };


        // TODO: TBD: note that this one is on the way...
        // TODO: TBD: drafting feature/duration-deconstruction branch vis-a-vis:
        // TODO: TBD: https://github.com/mwpowellhtx/protobuf/tree/feature/duration-deconstruction
        // TODO: TBD: PR pending draft changes, https://github.com/protocolbuffers/protobuf/pull/8173
        // TODO: TBD: we have established that Int32 is more appropriate than Nullable<Int32>.
        // TODO: TBD: pending clarification re: LABEL checks... re: CHANGES.txt (?)
        // TODO: TBD: which we may or may not also "squash" commits, but not considering that nearly as critical
        /// <summary>
        /// Deconstructs the <paramref name="value"/> in terms of <paramref name="seconds"/>
        /// and, optionally, <paramref name="nanoseconds"/>.
        /// </summary>
        /// <param name="value">The Value being deconstructed.</param>
        /// <param name="seconds">Receives the <see cref="Duration.Seconds"/> component.</param>
        /// <param name="nanoseconds">Receives the <see cref="Duration.Nanos"/> component.</param>
        /// <remarks>There is no value in deconstructing in only <paramref name="seconds"/>
        /// terms, from a language, or even simple POCO, perspective.</remarks>
        public static void Deconstruct(this Duration value, out long seconds, out int nanoseconds) =>
            (seconds, nanoseconds) = (value.Seconds, value.Nanos);
    }
}
