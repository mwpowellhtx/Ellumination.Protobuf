// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHasName<T>
        where T : class
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        T Name { get; set; }
    }
}
