using System;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <summary>
    /// Provides help Default Field Number values especially according to the Language Guide.
    /// </summary>
    /// <see cref="!:http://developers.google.com/protocol-buffers/docs/proto#assigning-field-numbers"/>
    public static class FieldNumbers
    {
        /// <summary>
        /// The Minimum Field Number, or 1L.
        /// </summary>
        /// <value>1</value>
        public const long MinimumFieldNumber = 1L;

        /// <summary>
        /// Raises <paramref name="x"/> to the Power <paramref name="y"/> in terms of
        /// <see cref="long"/> as contrasted with the default <see cref="double"/> based Power.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        /// <see cref="Math.Pow"/>
        private static long Pow(long x, long y) => (long) Math.Pow(x, y);

        /// <summary>
        /// The Maximum Field Number according to the Language Guide, 2^29 - 1, or 536,870,911.
        /// </summary>
        /// <value>536870911</value>
        public static long MaximumFieldNumber = Pow(2L, 29L) - 1L;

        /// <summary>
        /// The Minimum Reserved Field Number, or 19000L.
        /// </summary>
        /// <value>19000L</value>
        public const long MinimumReservedFieldNumber = 19000L;

        /// <summary>
        /// The Maximum Reserved Field Number, or 19999L.
        /// </summary>
        /// <value>19999L</value>
        public const long MaximumReservedFieldNumber = 19999L;

        /// <summary>
        /// Returns whether the <paramref name="value"/> Is Within Range of
        /// <see cref="MinimumFieldNumber"/> and <see cref="MaximumFieldNumber"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValidFieldNumber(this long value)
            => value >= MinimumFieldNumber && value <= MaximumFieldNumber;

        /// <summary>
        /// Returns whether the <paramref name="value"/> Is Reserved within the range of
        /// <see cref="MinimumReservedFieldNumber"/> and <see cref="MaximumReservedFieldNumber"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsReservedByGoogleProtocolBuffers(this long value)
            => value >= MinimumReservedFieldNumber && value <= MaximumReservedFieldNumber;
    }
}
