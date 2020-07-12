// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <inheritdoc cref="ICanRenderString" />
    public interface IMessageBodyItem
        : ICanRenderString
            , IHasParent<IMessageBodyParent>
    {
    }
}
