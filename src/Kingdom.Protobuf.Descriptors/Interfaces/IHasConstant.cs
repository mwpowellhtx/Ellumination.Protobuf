// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using Collections.Variants;

    /// <summary>
    /// 
    /// </summary>
    public interface IHasConstant
    {
        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        IVariant Value { get; set; }
    }
}
