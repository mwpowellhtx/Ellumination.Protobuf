// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
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
