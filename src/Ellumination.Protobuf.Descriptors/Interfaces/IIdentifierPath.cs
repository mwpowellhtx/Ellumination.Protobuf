using System;
using System.Collections.Generic;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <inheritdoc cref="IDescriptor"/>
    public interface IIdentifierPath
        : IDescriptor
            , IList<Identifier>
            , IEquatable<IdentifierPath>
    {
    }
}
