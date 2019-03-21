// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="ICanRenderString"/>
    public interface IPackageStatement : ICanRenderString
    {
        /// <summary>
        /// Gets or Sets the PackagePath.
        /// </summary>
        IdentifierPath PackagePath { get; set; }
    }
}
