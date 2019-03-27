// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static Characters;
    using static WhiteSpaceAndCommentOption;

    /// <inheritdoc cref="DescriptorBase" />
    public class PackageStatement
        : DescriptorBase
            , IPackageStatement
            , IProtoBodyItem
    {
        private IParentItem _parent;

        IProto IHasParent<IProto>.Parent
        {
            get => _parent as IProto;
            set => _parent = value;
        }

        // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
        /// <inheritdoc />
        public IdentifierPath PackagePath { get; set; } = new IdentifierPath { };

        /// <inheritdoc />
        public override string ToDescriptorString(IStringRenderingOptions options)
        {
            const string package = nameof(package);

            string GetComments(params WhiteSpaceAndCommentOption[] masks)
                => options.WhiteSpaceAndCommentRendering.RenderMaskedComments(masks);

            return $"{GetComments(MultiLineComment)}"
                   + $" {package} {GetComments(MultiLineComment)}"
                   + $" {PackagePath.ToDescriptorString(options)} {GetComments(MultiLineComment)} {SemiColon}"
                ;
        }
    }
}
