using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static Characters;
    using static String;

    // TODO: TBD: possibly a migration path to use in lieu of ElementTypePath ...
    // TODO: TBD: potentially a migration path from FullyQualifiedIdentity...
    // TODO: TBD: also from OptionName
    // TODO: TBD: possibly from FieldName
    // TODO: TBD: Identifier/Path almost definitely from the naked String "ident"
    // TODO: TBD: which affords an opportunity to consider how to build "paths" that map across a Proto hierarchy...
    /// <inheritdoc cref="DescriptorBase"/>
    public class IdentifierPath
        : DescriptorBase
            , IIdentifierPath
    {
        private readonly List<Identifier> _path;

        // ReSharper disable once UnusedMember.Global
        /// <inheritdoc />
        public IdentifierPath()
            : this(GetRange<Identifier>())
        {
        }

        /// <summary>
        /// <paramref name="path"/> Constructor.
        /// </summary>
        /// <param name="path"></param>
        public IdentifierPath(IEnumerable<Identifier> path)
        {
            _path = path.ToList();
        }

        private delegate void PathActionDelegate(IList<Identifier> path);

        private delegate TResult PathResultDelegate<out TResult>(IList<Identifier> path);

        private void PathAction(PathActionDelegate callback) => callback(_path);

        // ReSharper disable once InconsistentNaming
        private TResult GetPathResult<TResult>(PathResultDelegate<TResult> callback) => callback(_path);

        /// <inheritdoc />
        public IEnumerator<Identifier> GetEnumerator() => GetPathResult(x => x.GetEnumerator());

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        public void Add(Identifier item) => PathAction(x => x.Add(item));

        /// <inheritdoc />
        public void Clear() => PathAction(x => x.Clear());

        /// <inheritdoc />
        public bool Contains(Identifier item) => GetPathResult(x => x.Contains(item));

        /// <inheritdoc />
        public void CopyTo(Identifier[] array, int arrayIndex) => PathAction(x => x.CopyTo(array, arrayIndex));

        /// <inheritdoc />
        public bool Remove(Identifier item) => GetPathResult(x => x.Remove(item));

        /// <inheritdoc />
        public int Count => GetPathResult(x => x.Count);

        /// <inheritdoc />
        public bool IsReadOnly => GetPathResult(x => x.IsReadOnly);

        /// <inheritdoc />
        public int IndexOf(Identifier item) => GetPathResult(x => x.IndexOf(item));

        /// <inheritdoc />
        public void Insert(int index, Identifier item) => PathAction(x => x.Insert(index, item));

        /// <inheritdoc />
        public void RemoveAt(int index) => PathAction(x => x.RemoveAt(index));

        /// <inheritdoc />
        public Identifier this[int index]
        {
            get => GetPathResult(x => x[index]);
            set => PathAction(x => x[index] = value);
        }

        /// <summary>
        /// Returns whether <paramref name="a"/> Equals <paramref name="b"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected internal static bool Equals(IdentifierPath a, IdentifierPath b)
            => !(a == null || b == null)
               && (ReferenceEquals(a, b)
                   || (a.Count == b.Count
                       && a.Zip(b, (x, y) => x?.Equals(y)).All(z => z ?? false)
                   )
               );

        /// <inheritdoc />
        public virtual bool Equals(IdentifierPath other) => Equals(this, other);

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
            => Join($"{Dot}", this.Select(x => x.ToDescriptorString(options)));

        /// <summary>
        /// Returns a new <see cref="IdentifierPath"/> instance appending <paramref name="s"/>
        /// to the <paramref name="path"/>. Leverages the implicit type conversion operator from
        /// <see cref="string"/> <see cref="Identifier"/> <see cref="Identifier.op_Implicit"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static IdentifierPath operator /(IdentifierPath path, string s)
            => new IdentifierPath(path.Concat(GetRange<Identifier>(s)));

        /// <summary>
        /// Returns a new <see cref="IdentifierPath"/> instance appending <paramref name="x"/>
        /// to the <paramref name="path"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static IdentifierPath operator /(IdentifierPath path, Identifier x)
            => new IdentifierPath(path.Concat(GetRange(x)));

        /// <summary>
        /// Returns a new <see cref="IdentifierPath"/> instance appending <paramref name="other"/>
        /// to the <paramref name="path"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static IdentifierPath operator /(IdentifierPath path, IdentifierPath other)
            => new IdentifierPath(path.Concat(other));
    }
}
