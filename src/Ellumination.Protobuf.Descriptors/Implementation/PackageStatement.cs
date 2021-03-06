﻿// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    using static Characters;
    using static WhiteSpaceAndCommentOption;

    /// <inheritdoc cref="DescriptorBase" />
    public class PackageStatement
        : DescriptorBase
            , IPackageStatement
            , IProtoBodyItem
    {
        IProto IHasParent<IProto>.Parent
        {
            get => Parent as IProto;
            set => Parent = value;
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
