using System;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    // ReSharper disable once PossibleInterfaceMemberAmbiguity
    /// <inheritdoc cref="IReservedBodyItem" />
    public interface IRangeDescriptor
        : IReservedBodyItem
            , ICanRenderString
            , IHasParent<IExtensionsStatement>
            , IHasParent<IReservedStatement>
            , IEquatable<IRangeDescriptor>
            , IEquatable<long>
    {
        /// <summary>
        /// Gets or sets the Minimum. This value is always required. Acts like an
        /// Exact Match instead of a true Range when <see cref="Maximum"/> is Null.
        /// </summary>
        long Minimum { get; set; }

        /// <summary>
        /// Gets or sets the Maximum. This is an Optional member, it may or may not be specified.
        /// Furthermore, we must do special rendering for <see cref="long.MaxValue"/>.
        /// </summary>
        long? Maximum { get; set; }

        /// <summary>
        /// Returns whether the Range Contains the <paramref name="value"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Contains(long value);

        /// <summary>
        /// Returns whether the Range Contains the <paramref name="other"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        bool Contains(IRangeDescriptor other);

        /// <summary>
        /// Returns the Intersection of this object with the <paramref name="other"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        RangeDescriptor Intersect(IRangeDescriptor other);

        /// <summary>
        /// Returns whether <see cref="Intersect"/> is possible.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="intersection"></param>
        /// <returns></returns>
        bool TryIntersect(IRangeDescriptor other, out RangeDescriptor intersection);

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// Returns whether Intersects with the <paramref name="other"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        bool Intersects(IRangeDescriptor other);

        /// <summary>
        /// Returns the Range in terms of Range Notation.
        /// </summary>
        /// <returns></returns>
        string ToRangeNotation();
    }
}
