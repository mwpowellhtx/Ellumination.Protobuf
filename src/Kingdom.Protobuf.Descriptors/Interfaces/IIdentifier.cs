using System;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="ICanRenderString"/>
    public interface IIdentifier
        : IDescriptor<string>
            , IHasParent<IReservedStatement>
            , IEquatable<Identifier>
    {
    }
}
