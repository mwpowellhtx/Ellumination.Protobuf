using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static String;

    /// <inheritdoc cref="IdentifierPath"/>
    public class OptionIdentifierPath
        : IdentifierPath
            , IEquatable<OptionIdentifierPath>
    {
        private bool? _prefixGrouped;

        /// <summary>
        /// Gets or Sets whether IsPrefixGrouped. The Prefix may be Grouped, that is, Enclosed in
        /// Parentheses. The state of <see cref="SuffixStartIndex"/> can override the default
        /// condition of <see cref="IsPrefixGrouped"/>.
        /// </summary>
        public bool IsPrefixGrouped
        {
            get => (_prefixGrouped ?? false) || SuffixStartIndex > 1;
            set => _prefixGrouped = value;
        }

        /// <summary>
        /// Gets or Sets the SuffixStartIndex. Not all Paths of a Suffix, except
        /// in the case of Option Name. This Rule may optionally have a Suffix starting
        /// at the specified Index.
        /// </summary>
        /// <remarks>This property is useful when representing the Option Name Identifier Path,
        /// which has a clear starting index at which point an optional Suffix begins.</remarks>
        public int? SuffixStartIndex { get; set; }

        /// <inheritdoc />
        public OptionIdentifierPath()
            : this(GetRange<Identifier>())
        {
        }

        /// <inheritdoc />
        public OptionIdentifierPath(IEnumerable<Identifier> path)
            : base(path)
        {
        }

        /// <summary>
        /// Returns whether <paramref name="a"/> Equals <paramref name="b"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool Equals(OptionIdentifierPath a, OptionIdentifierPath b)
            => IdentifierPath.Equals(a, b)
               && a.IsPrefixGrouped == b.IsPrefixGrouped
               && a.SuffixStartIndex == b.SuffixStartIndex;

        /// <inheritdoc />
        public bool Equals(OptionIdentifierPath other) => Equals(this, other);

        /// <inheritdoc />
        public override bool Equals(IdentifierPath other) => Equals(this, other as OptionIdentifierPath);

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
        {
            int MaxStartIndex() => Count;

            var suffixStartIndex = SuffixStartIndex ?? MaxStartIndex();

            const string dot = ".";

            string RenderPrefix()
            {
                var renderedPrefix = Join(dot, this.Take(suffixStartIndex).Select(
                    i => i.ToDescriptorString(options))
                );
                bool ShouldEnclose() => IsPrefixGrouped || suffixStartIndex > 1;
                return ShouldEnclose() ? $"({renderedPrefix})" : $"{renderedPrefix}";
            }

            string RenderSuffix() => Join(dot, this.Skip(suffixStartIndex).Select(
                i => i.ToDescriptorString(options))
            );

            var x = RenderPrefix();
            var y = RenderSuffix();

            return IsNullOrEmpty(y) ? x : Join(dot, RenderPrefix(), RenderSuffix());
        }
    }
}
