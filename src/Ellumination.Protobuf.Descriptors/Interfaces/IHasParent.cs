// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <summary>
    /// Indicates whether Having a <typeparamref name="TParent"/> Parent.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    public interface IHasParent<TParent>
        where TParent : IParentItem
    {
        /// <summary>
        /// Gets or Sets the <typeparamref name="TParent"/> Parent.
        /// </summary>
        TParent Parent { get; set; }
    }
}
