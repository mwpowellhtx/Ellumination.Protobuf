// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static FloatingPointRenderingOption;
    using static IntegerRenderingOption;
    using static WhiteSpaceAndCommentOption;

    /// <inheritdoc />
    public class StringRenderingOptions : IStringRenderingOptions
    {
        /// <inheritdoc />
        public FloatingPointRenderingOption FloatingPointRendering { get; set; } = NoFloatingPointRenderingOption;

        /// <inheritdoc />
        public IntegerRenderingOption IntegerRendering { get; set; } = NoIntegerRenderingOption;

        /// <inheritdoc />
        public WhiteSpaceAndCommentOption WhiteSpaceAndCommentRendering { get; set; } = NoWhiteSpaceOrCommentOption;
    }
}
