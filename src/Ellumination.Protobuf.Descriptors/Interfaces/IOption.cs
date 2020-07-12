// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <summary>
    /// Supports both <see cref="OptionStatement"/> as well as <see cref="EnumValueOption"/>.
    /// </summary>
    /// <inheritdoc cref="IHasName{T}" />
    /// <see cref="OptionStatement"/>
    /// <see cref="EnumValueOption"/>
    public interface IOption : IDescriptor, IHasName<OptionIdentifierPath>, IHasConstant
    {
    }
}
