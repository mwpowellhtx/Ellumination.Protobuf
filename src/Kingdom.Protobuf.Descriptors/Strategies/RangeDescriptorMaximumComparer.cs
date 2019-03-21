using System.Collections.Generic;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <summary>
    /// It does not make sense to compare both <see cref="IRangeDescriptor.Minimum"/> and
    /// <see cref="IRangeDescriptor.Maximum"/> at the same time. They must be compared
    /// separately, first Minimum, then Maximum, in order for a correct ordering to take place.
    /// </summary>
    /// <inheritdoc cref="IComparer{T}"/>
    public class RangeDescriptorMaximumComparer : IComparer<IRangeDescriptor>, IComparer<RangeDescriptor>
    {
        /// <summary>
        /// Internal Default Constructor.
        /// </summary>
        internal RangeDescriptorMaximumComparer()
        {
        }

        private static int PrivateCompare(IRangeDescriptor x, IRangeDescriptor y)
        {
            // May need to Invert the Result depending upon which direction is Compared.
            int CompareNonNull(long a, long? b) => b == null ? 1 : a.CompareTo(b.Value);

            int CompareNullable(long? a, long? b)
            {
                switch (a)
                {
                    case null:
                        // ReSharper disable once ExpressionIsAlwaysNull
                        // Yes, we know A will always be Null, just to be Clear what is going on here.
                        return 0 - (b == null ? 0 : CompareNonNull(b.Value, a));
                    default:
                        return CompareNonNull(a.Value, b);
                }
            }

            return CompareNullable(x?.Maximum, y?.Maximum);
        }

        /// <inheritdoc />
        public int Compare(IRangeDescriptor x, IRangeDescriptor y) => PrivateCompare(x, y);

        /// <inheritdoc />
        public int Compare(RangeDescriptor x, RangeDescriptor y) => PrivateCompare(x, y);
    }
}
