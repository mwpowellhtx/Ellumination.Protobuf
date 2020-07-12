// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using static Characters;
    using static WhiteSpaceAndCommentOption;

    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc cref="DescriptorBase" />
    public class ImportStatement : DescriptorBase, IImportStatement
    {
        IProto IHasParent<IProto>.Parent
        {
            get => Parent as IProto;
            set => Parent = value;
        }

        /// <summary>
        /// Gets or Sets the Modifier.
        /// </summary>
        public ImportModifierKind? Modifier { get; set; }

        /// <summary>
        /// Gets or Sets the Import Path.
        /// </summary>
        public string ImportPath { get; set; }

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
        {
            // TODO: TBD: borderline, introduce GetRenderedModifier() local here...
            string GetComments(params WhiteSpaceAndCommentOption[] masks)
                => options.WhiteSpaceAndCommentRendering.RenderMaskedComments(masks);

            const string import = nameof(import);

            // TODO: TBD: escape delimit the string...
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (Modifier)
            {
                case null:
                    return $"{GetComments(MultiLineComment)}"
                           + $" {import} {GetComments(MultiLineComment)}"
                           + $" {OpenQuote}{ImportPath}{CloseQuote} {GetComments(MultiLineComment)} {SemiColon}"
                        ;
                default:
                    return $"{GetComments(MultiLineComment)}"
                           + $" {import} {GetComments(MultiLineComment)}"
                           + $" {$"{Modifier.Value}".ToLower()} {GetComments(MultiLineComment)}"
                           + $" {OpenQuote}{ImportPath}{CloseQuote} {GetComments(MultiLineComment)} {SemiColon}"
                        ;
            }
        }
    }
}
