using System;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using static String;

    /// <inheritdoc cref="IdentifierPath"/>
    public class ElementTypeIdentifierPath
        : IdentifierPath
            , IElementTypeIdentifierPath
    {
        /// <inheritdoc />
        public bool IsGlobalScope { get; set; }

        /// <summary>
        /// Returns whether <paramref name="a"/> Equals <paramref name="b"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected internal static bool Equals(ElementTypeIdentifierPath a, ElementTypeIdentifierPath b)
            => IdentifierPath.Equals(a, b)
               && a.IsGlobalScope == b.IsGlobalScope;

        /// <inheritdoc />
        public bool Equals(ElementTypeIdentifierPath other) => Equals(this, other);

        /// <inheritdoc />
        public override bool Equals(IdentifierPath other) => Equals(this, other as ElementTypeIdentifierPath);

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
        {
            const string dot = ".";

            string RenderGlobalScope() => IsGlobalScope ? dot : Empty;

            return RenderGlobalScope()
                   + Join(dot, this.Select(x => x.ToDescriptorString(options)))
                ;
        }
    }
}
