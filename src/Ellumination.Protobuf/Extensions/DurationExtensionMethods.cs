using System;

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

        /// <summary>
        /// Returns the <see cref="Duration"/> corresponding with the <paramref name="seconds"/>
        /// and optional <paramref name="nanos"/>.
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="nanos"></param>
        /// <returns>The <see cref="Duration"/> corresponding to the <paramref name="seconds"/>
        /// and optional <paramref name="nanos"/>.</returns>
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

            // We leave Nanos out when there is no specified value.
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
    }
}
