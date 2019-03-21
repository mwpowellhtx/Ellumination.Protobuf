// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <summary>
    /// Supports both <see cref="OptionStatement"/> as well as <see cref="EnumValueOption"/>.
    /// </summary>
    /// <inheritdoc cref="IHasName{T}" />
    /// <see cref="OptionStatement"/>
    /// <see cref="EnumValueOption"/>
    public interface IOption
        : IHasName<OptionIdentifierPath>
            , IHasConstant
            , ICanRenderString
    {
    }
}
