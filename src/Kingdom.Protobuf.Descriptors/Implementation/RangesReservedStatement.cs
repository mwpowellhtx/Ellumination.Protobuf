// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc />
    public class RangesReservedStatement : ReservedStatement<RangeDescriptor>
    {
        /// <inheritdoc />
        protected override string RenderItem(RangeDescriptor item, IStringRenderingOptions options)
            => $"{item.ToDescriptorString(options)}";
    }
}
