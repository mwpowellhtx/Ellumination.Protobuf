using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static FieldNumbers;

    /// <inheritdoc />
    public abstract class DescriptorVisitorFieldNumberingStrategyBase<T> : DescriptorVisitorStrategyBase<T>
        where T : DescriptorBase
    {
        /// <inheritdoc />
        protected DescriptorVisitorFieldNumberingStrategyBase(T descriptor)
            : base(descriptor)
        {
        }

        /// <summary>
        /// Returns a <see cref="RangeDescriptor"/> corresponding to the
        /// <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static RangeDescriptor CreateOneRange(long min, long? max = null) => new RangeDescriptor
        {
            Minimum = min,
            Maximum = max
        };

        // TODO: TBD: separate key data and decision making elements into a visitor policy...
        private IEnumerable<RangeDescriptor> _defaultReservedRanges;

        /// <summary>
        /// Gets the Default Reserved Ranges.
        /// </summary>
        /// <see cref="MinimumReservedFieldNumber"/>
        /// <see cref="MaximumReservedFieldNumber"/>
        protected IEnumerable<RangeDescriptor> DefaultReservedRanges
            => _defaultReservedRanges ?? (_defaultReservedRanges = GetRange(
                   CreateOneRange(MinimumReservedFieldNumber, MaximumReservedFieldNumber)
               ).ToArray());

        private IEnumerable<RangeDescriptor> _allowedFieldNumberRanges;

        /// <summary>
        /// A known Range including 1 to 2^29-1 or 536,870,911.
        /// </summary>
        /// <see cref="!:http://developers.google.com/protocol-buffers/docs/proto#assigning-field-numbers"/>
        protected IEnumerable<RangeDescriptor> AllowedFieldNumberRanges
        {
            get
            {
                long Pow(long @base, long exponent) => (long) Math.Pow(@base, exponent);

                const long min = 1L;

                return _allowedFieldNumberRanges ?? (_allowedFieldNumberRanges = GetRange(
                           CreateOneRange(min, Pow(2L, 29L) - 1L)
                       ).ToArray());
            }
        }
    }
}
