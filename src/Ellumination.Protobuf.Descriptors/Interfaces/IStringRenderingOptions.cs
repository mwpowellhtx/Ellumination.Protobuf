// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <summary>
    /// 
    /// </summary>
    /// <inheritdoc cref="IFloatingPointRenderingOptions"/>
    public interface IStringRenderingOptions
        : IIntegerRenderingOptions
            , IFloatingPointRenderingOptions
            , IWhiteSpaceAndCommentRenderingOptions
    {
    }
}
