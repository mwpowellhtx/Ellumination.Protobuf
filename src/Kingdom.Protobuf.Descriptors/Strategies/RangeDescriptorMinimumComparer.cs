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
    public class RangeDescriptorMinimumComparer : IComparer<IRangeDescriptor>, IComparer<RangeDescriptor>
    {
        /// <summary>
        /// Internal Default Constructor.
        /// </summary>
        internal RangeDescriptorMinimumComparer()
        {
        }

        private static int PrivateCompare(IRangeDescriptor x, IRangeDescriptor y)
        {
            int CompareNull(IRangeDescriptor b) => b == null ? 0 : -1;
            int CompareOther(IRangeDescriptor a, IRangeDescriptor b) => b == null ? 0 : a.Minimum.CompareTo(b.Minimum);

            return x == null ? CompareNull(y) : CompareOther(x, y);
        }

        /// <inheritdoc />
        public int Compare(IRangeDescriptor x, IRangeDescriptor y) => PrivateCompare(x, y);

        /// <inheritdoc />
        public int Compare(RangeDescriptor x, RangeDescriptor y) => PrivateCompare(x, y);
    }
}
