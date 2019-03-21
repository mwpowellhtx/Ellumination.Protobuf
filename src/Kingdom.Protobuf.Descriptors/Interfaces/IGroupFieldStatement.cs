// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
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
