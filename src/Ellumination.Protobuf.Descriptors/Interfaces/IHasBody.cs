using System.Collections.Generic;

// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHasBody<T>
    {
        /// <summary>
        /// Gets or sets the Items.
        /// </summary>
        IList<T> Items { get; set; }
    }
}
