using System;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static String;

    /// <inheritdoc cref="DescriptorBase"/>
    public class Identifier
        : DescriptorBase<string>
            , IIdentifier
    {
        IReservedStatement IHasParent<IReservedStatement>.Parent
        {
            get => Parent as IReservedStatement;
            set => Parent = value;
        }

        /// <inheritdoc />
        public Identifier()
            :this(Empty)
        {
        }

        /// <inheritdoc />
        public Identifier(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Returns whether <paramref name="a"/> Equals <paramref name="b"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool Equals(Identifier a, Identifier b)
            => !(a == null || b == null)
               && (ReferenceEquals(a, b)
                   || a.Name == b.Name);

        /// <inheritdoc />
        public bool Equals(Identifier other) => Equals(this, other);

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options) => $"{Name}";

        /// <summary>
        /// Type conversion operator from <see cref="string"/> <paramref name="s"/> to
        /// <see cref="Identifier"/>.
        /// </summary>
        /// <param name="s"></param>
        public static implicit operator Identifier(string s) => new Identifier(s);

        /// <summary>
        /// Returns the Merged <see cref="IdentifierPath"/> consisting of <paramref name="a"/>
        /// followed by <paramref name="b"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static IdentifierPath operator /(Identifier a, Identifier b)
            => new IdentifierPath(GetRange(a, b));
    }
}
