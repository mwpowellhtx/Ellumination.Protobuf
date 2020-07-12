// ReSharper disable once IdentifierTypo
namespace Ellumination.Protobuf
{
    /// <inheritdoc cref="IFieldStatement" />
    public interface IGroupFieldStatement
        : IFieldStatement
            , IMessageBodyParent
            , IHasGroupName
            , IHasLabel
    {
    }
}
