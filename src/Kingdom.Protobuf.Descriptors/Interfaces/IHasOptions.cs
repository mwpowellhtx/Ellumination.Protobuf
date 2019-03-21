using System.Collections.Generic;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHasOptions<T>
    {
        /// <summary>
        /// Gets or sets the Options.
        /// </summary>
        IList<T> Options { get; set; }
    }
}
