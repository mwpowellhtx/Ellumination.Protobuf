// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc />
    public class FieldNamesReservedStatement : ReservedStatement<Identifier>
    {
        /// <inheritdoc />
        protected override string RenderItem(Identifier item, IStringRenderingOptions options)
            => item.ToDescriptorString(options);
    }
}
