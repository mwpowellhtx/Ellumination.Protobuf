// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <inheritdoc cref="OptionDescriptorBase"/>
    public class EnumValueOption
        : OptionDescriptorBase
            , IEnumValueOption
    {
        IEnumFieldDescriptor IHasParent<IEnumFieldDescriptor>.Parent
        {
            get => Parent as IEnumFieldDescriptor;
            set => Parent = value;
        }

        /// <inheritdoc />
        public bool Equals(EnumValueOption other) => Equals(this, other);
    }
}
