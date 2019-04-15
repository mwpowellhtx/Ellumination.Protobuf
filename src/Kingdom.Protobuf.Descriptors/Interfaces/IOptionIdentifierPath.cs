using System;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="IIdentifierPath"/>
    public interface IOptionIdentifierPath : IIdentifierPath, IEquatable<OptionIdentifierPath>
    {
        /// <summary>
        /// Gets or Sets whether IsPrefixGrouped. The Prefix may be Grouped, that is, Enclosed in
        /// Parentheses. The state of <see cref="SuffixStartIndex"/> can override the default
        /// condition of <see cref="IsPrefixGrouped"/>.
        /// </summary>
        bool IsPrefixGrouped { get; set; }

        /// <summary>
        /// Gets or Sets the SuffixStartIndex. Not all Paths of a Suffix, except
        /// in the case of Option Name. This Rule may optionally have a Suffix starting
        /// at the specified Index.
        /// </summary>
        /// <remarks>This property is useful when representing the Option Name Identifier Path,
        /// which has a clear starting index at which point an optional Suffix begins.</remarks>
        int? SuffixStartIndex { get; set; }
    }
}
