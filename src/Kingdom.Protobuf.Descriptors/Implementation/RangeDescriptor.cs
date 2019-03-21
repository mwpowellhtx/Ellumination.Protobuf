using System;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static FieldNumbers;
    using static String;

    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc cref="DescriptorBase" />
    public class RangeDescriptor
        : DescriptorBase
            , IRangeDescriptor
            , IEquatable<RangeDescriptor>
    {
        // TODO: TBD: may refactor to base classes...
        private IParentItem _parent;

        IExtensionsStatement IHasParent<IExtensionsStatement>.Parent
        {
            get => _parent as IExtensionsStatement;
            set => _parent = value;
        }

        IReservedStatement IHasParent<IReservedStatement>.Parent
        {
            get => _parent as IReservedStatement;
            set => _parent = value;
        }

        /// <inheritdoc />
        public long Minimum { get; set; }

        /// <inheritdoc />
        public long? Maximum { get; set; }

        /// <summary>
        /// Returns whether <paramref name="a"/> Equals <paramref name="b"/>.
        /// We must take special care with <see cref="IRangeDescriptor.Maximum"/>
        /// possibly being Null.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool Equals(IRangeDescriptor a, IRangeDescriptor b)
            => ReferenceEquals(a, b)
               || (a.Minimum == b.Minimum && a.Maximum == b.Maximum);

        /// <inheritdoc />
        public bool Equals(IRangeDescriptor other) => Equals(this, other);

        /// <inheritdoc />
        public bool Equals(RangeDescriptor other) => Equals(this, other);

        private static bool Equals(long value, long min, long? max) => max == null && value == min;

        /// <inheritdoc />
        /// <summary>
        /// Returns whether the Range exactly Equals the <paramref name="value"/>.
        /// This necessarily requires <see cref="Maximum"/> to be Null.
        /// </summary>
        public bool Equals(long value) => Equals(value, Minimum, Maximum);

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
        {
            string RenderMinimum() => $"{Minimum.RenderLong(options.IntegerRendering)}";

            string RenderMaximum()
            {
                string RenderSpecifiedMaximum(long value)
                {
                    const string to = nameof(to);

                    string RenderMaximumValue() => $" {to} {value.RenderLong(options.IntegerRendering)}";

                    string RenderMaxMaximum()
                    {
                        const string max = nameof(max);
                        return $" {to} {max}";
                    }

                    return value >= MaximumFieldNumber ? RenderMaxMaximum() : RenderMaximumValue();
                }

                switch (Maximum)
                {
                    case null: return Empty;
                    default: return RenderSpecifiedMaximum(Maximum.Value);
                }
            }

            return $"{RenderMinimum()}{RenderMaximum()}";
        }

        /// <summary>
        /// Exactly Matches <paramref name="value"/> with <see cref="Minimum"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool Matches(long value) => !Maximum.HasValue && value == Minimum;

        /// <inheritdoc />
        public virtual bool Contains(long value) => Matches(value) || (value >= Minimum && value <= Maximum);

        /// <inheritdoc />
        public virtual bool Contains(IRangeDescriptor other)
            => Contains(other.Minimum) && (
                   !other.Maximum.HasValue || Contains(other.Maximum.Value)
               );

        /// <inheritdoc />
        public virtual RangeDescriptor Intersect(IRangeDescriptor other)
        {
            // ReSharper disable once ImplicitlyCapturedClosure
            RangeDescriptor IntersectMinimum(long min)
            {
                switch (other.Maximum)
                {
                    case null:
                        return other.Equals(min) ? new RangeDescriptor {Minimum = min} : null;
                    default:
                        return other.Contains(min) ? new RangeDescriptor {Minimum = min} : null;
                }
            }

            RangeDescriptor IntersectRange(long min, long max)
            {
                var otherMinimum = other.Minimum;
                var otherMaximumValue = other.Maximum ?? 0L;

                switch (other.Maximum)
                {
                    case null:
                        return Contains(otherMinimum) ? new RangeDescriptor {Minimum = otherMinimum} : null;
                    default:
                        return Contains(otherMinimum)
                               || Contains(otherMaximumValue)
                               || other.Contains(min) || other.Contains(max)
                            ? new RangeDescriptor
                            {
                                Minimum = Math.Max(min, otherMinimum),
                                Maximum = Math.Min(max, otherMaximumValue)
                            }
                            : null;
                }
            }

            switch (Maximum)
            {
                case null:
                    return IntersectMinimum(Minimum);
                default:
                    return IntersectRange(Minimum, Maximum.Value);
            }
        }

        /// <inheritdoc />
        public virtual bool TryIntersect(IRangeDescriptor other, out RangeDescriptor intersection)
            => (intersection = Intersect(other)) == null;

        /// <inheritdoc />
        public virtual bool Intersects(IRangeDescriptor other) => TryIntersect(other, out _);

        /// <summary>
        /// Returns a <see cref="RangeDescriptor"/> corresponding to the <paramref name="min"/>
        /// and <paramref name="max"/>.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static RangeDescriptor CreateRange(long min, long? max = null)
            => new RangeDescriptor {Minimum = min, Maximum = max};

        /// <inheritdoc />
        public string ToRangeNotation() => Maximum.HasValue ? $"[{Minimum}, {Maximum.Value}]" : $"[{Minimum}]";
    }
}
