using System;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <inheritdoc cref="ICanRenderString"/>
    public interface IEnumValueOption
        : ICanRenderString
            , IHasParent<IEnumFieldDescriptor>
            , IEquatable<EnumValueOption>
    {
    }
}
