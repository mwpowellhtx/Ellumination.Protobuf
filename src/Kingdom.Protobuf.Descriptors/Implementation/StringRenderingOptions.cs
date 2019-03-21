// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static FloatingPointRenderingOption;
    using static IntegerRenderingOption;

    /// <inheritdoc />
    public class StringRenderingOptions : IStringRenderingOptions
    {
        /// <inheritdoc />
        public FloatingPointRenderingOption FloatingPointRendering { get; set; } = NoFloatingPointRenderingOption;

        /// <inheritdoc />
        public IntegerRenderingOption IntegerRendering { get; set; } = NoIntegerRenderingOption;
    }
}
