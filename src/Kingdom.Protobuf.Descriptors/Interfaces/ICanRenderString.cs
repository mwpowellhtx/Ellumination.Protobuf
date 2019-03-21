// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICanRenderString
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string ToDescriptorString();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string ToDescriptorString(IStringRenderingOptions options);
    }
}
