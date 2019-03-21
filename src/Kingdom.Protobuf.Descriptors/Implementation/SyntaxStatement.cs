// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static SyntaxKind;

    // TODO: TBD: we could call this an ISyntaxStatement just for consistency, but we will leave this alone for the time being...
    /// <inheritdoc cref="DescriptorBase" />
    public class SyntaxStatement
        : DescriptorBase
            , IHasParent<ProtoDescriptor>
    {
        /// <inheritdoc />
        public ProtoDescriptor Parent { get; set; }

        /// <summary>
        /// Gets or Sets the Syntax.
        /// </summary>
        public SyntaxKind Syntax { get; set; } = Proto2;

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
        {
            const string syntax = nameof(syntax);
            return $"{syntax} = '{nameof(Proto2).ToLower()}';";
        }
    }
}
