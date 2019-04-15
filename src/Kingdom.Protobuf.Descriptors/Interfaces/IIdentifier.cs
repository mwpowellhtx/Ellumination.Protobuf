using System;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    // ReSharper disable once PossibleInterfaceMemberAmbiguity
    /// <inheritdoc cref="ICanRenderString"/>
    public interface IIdentifier : IDescriptor<string>, IHasParent<IReservedStatement>, IEquatable<Identifier>
    {
    }
}
