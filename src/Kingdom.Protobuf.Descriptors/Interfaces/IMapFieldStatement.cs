// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    /// <inheritdoc cref="IFieldStatement"/>
    public interface IMapFieldStatement
        : IFieldStatement
            , IHasOptions<FieldOption>
    {
        /// <summary>
        /// Gets or Sets the Key Type.
        /// </summary>
        KeyType KeyType { get; set; }

        /// <summary>
        /// Gets or Sets the Value Type.
        /// </summary>
        IVariant ValueType { get; set; }
    }
}
