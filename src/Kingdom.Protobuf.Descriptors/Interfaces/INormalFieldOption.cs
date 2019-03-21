// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="IFieldOption"/>
    public interface INormalFieldOption
        : IFieldOption
            , IHasParent<INormalFieldStatement>
    {
    }
}
