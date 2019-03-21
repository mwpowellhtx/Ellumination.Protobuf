// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="IFieldOption"/>
    public interface IMapFieldOption
        : IFieldOption
            , IHasParent<IMapFieldStatement>
    {
    }
}
