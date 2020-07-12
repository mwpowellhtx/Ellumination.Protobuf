using System;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using static Math;
    using static BitConverter;

    internal static class RandomExtensionMethods
    {
        /// <summary>
        /// Returns the Next Double from Zed (Zero) through <paramref name="max"/>.
        /// </summary>
        /// <param name="random"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        /// <see cref="Random.NextDouble"/>
        public static double NextDouble(this Random random, double max) => NextDouble(random, 0d, max);

        /// <summary>
        /// Returns the Next Double using <see cref="Random.NextDouble"/>. Note that it suffers
        /// the same limitations in that it will never return <paramref name="max"/>, for example.
        /// Otherwise, this does return the range.
        /// </summary>
        /// <param name="random"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        /// <see cref="Random.NextDouble"/>
        public static double NextDouble(this Random random, double min, double max)
        {
            var minActual = Min(min, max);
            var maxActual = Max(min, max);
            var result = random.NextDouble() * (maxActual - minActual);
            return result + minActual;
        }

        /// <summary>
        /// Returns a Uniformly Random <see cref="ulong"/> between <see cref="ulong.MinValue"/>
        /// inclusive and <see cref="ulong.MaxValue"/> and inclusive.
        /// </summary>
        /// <param name="random"></param>
        /// <returns></returns>
        public static ulong NextULong(this Random random)
        {
            var buf = new byte[sizeof(ulong)];
            random.NextBytes(buf);
            return ToUInt64(buf, 0);
        }

        /// <summary>
        /// Returns a Uniformly Random <see cref="ulong"/> between <see cref="ulong.MinValue"/>
        /// and <see cref="ulong.MaxValue"/> without modulo bias.
        /// </summary>
        /// <param name="random"></param>
        /// <param name="max"></param>
        /// <param name="inclusiveUpperBound"></param>
        /// <returns></returns>
        public static ulong NextULong(this Random random, ulong max, bool inclusiveUpperBound = false)
        {
            return random.NextULong(ulong.MinValue, max, inclusiveUpperBound);
        }

        /// <summary>
        /// Returns a Uniformly Random <see cref="ulong"/> between <see cref="ulong.MinValue"/>
        /// and <see cref="ulong.MaxValue"/> without modulo bias.
        /// </summary>
        /// <param name="random"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="inclusiveUpperBound"></param>
        /// <returns></returns>
        public static ulong NextULong(this Random random, ulong min, ulong max, bool inclusiveUpperBound = false)
        {
            var range = max - min;

            if (inclusiveUpperBound)
            {
                if (range == ulong.MaxValue)
                {
                    return random.NextULong();
                }

                range++;
            }

            if (range <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    $"'{nameof(max)}' must be greater than '{nameof(min)}' when '{nameof(inclusiveUpperBound)}'"
                    + " is false and greater than or equal to when true", nameof(max));
            }

            var limit = ulong.MaxValue - ulong.MaxValue % range;
            ulong r;
            do
            {
                r = random.NextULong();
            } while (r > limit);

            return r % range + min;
        }

        /// <summary>
        /// Returns a Uniformly Random <see cref="long"/> between <see cref="long.MinValue"/>
        /// inclusive and <see cref="long.MaxValue"/> inclusive.
        /// </summary>
        /// <param name="random"></param>
        /// <returns></returns>
        public static long NextLong(this Random random)
        {
            var buf = new byte[sizeof(long)];
            random.NextBytes(buf);
            return ToInt64(buf, 0);
        }

        /// <summary>
        /// Returns a Uniformly Random <see cref="long"/> between <see cref="long.MinValue"/>
        /// and <see cref="long.MaxValue"/> without modulo bias.
        /// </summary>
        /// <param name="random"></param>
        /// <param name="max"></param>
        /// <param name="inclusiveUpperBound"></param>
        /// <returns></returns>
        public static long NextLong(this Random random, long max, bool inclusiveUpperBound = false)
        {
            return random.NextLong(long.MinValue, max, inclusiveUpperBound);
        }

        /// <summary>
        /// Returns a Uniformly Random <see cref="long"/> between <see cref="long.MinValue"/>
        /// and <see cref="long.MaxValue"/> without modulo bias.
        /// </summary>
        /// <param name="random"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="inclusiveUpperBound"></param>
        /// <returns></returns>
        public static long NextLong(this Random random, long min, long max, bool inclusiveUpperBound = false)
        {
            var range = (ulong) (max - min);

            if (inclusiveUpperBound)
            {
                if (range == ulong.MaxValue)
                {
                    return random.NextLong();
                }

                range++;
            }

            if (range <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    $"'{nameof(max)}' must be greater than '{nameof(min)}' when '{nameof(inclusiveUpperBound)}'"
                    + " is false and greater than or equal to when true", nameof(max));
            }

            var limit = ulong.MaxValue - ulong.MaxValue % range;
            ulong r;
            do
            {
                r = random.NextULong();
            } while (r > limit);

            return (long) (r % range + (ulong) min);
        }
    }
}
