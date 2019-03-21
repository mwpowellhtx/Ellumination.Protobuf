using System;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="ICanRenderString"/>
    public interface IEnumValueOption
        : ICanRenderString
            , IHasParent<IEnumFieldDescriptor>
            , IEquatable<EnumValueOption>
    {
    }
}
