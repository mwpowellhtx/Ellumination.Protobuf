using System;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="IIdentifierPath"/>
    public interface IElementTypeIdentifierPath
        : IIdentifierPath
            , IEquatable<ElementTypeIdentifierPath>
    {
        /// <summary>
        /// Gets or Sets whether IsGlobalScope.
        /// </summary>
        bool IsGlobalScope { get; set; }
    }
}
